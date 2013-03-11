using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using Idera.SQLsecure.UI.Console.Utility;

using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinDataSource;
using Infragistics.Win.UltraWinGrid;

namespace Idera.SQLsecure.UI.Console.Controls
{
    public partial class ServerPrincipalPermissionsGrid : UserControl
    {
        #region Columns

        // columns.
        private const string colIcon = "Icon";
        private const string colObjectName = "objectname";
        private const string colPermission = "permission";
        private const string colGrant = "isgrant";
        private const string colGrantCheckBox = "GrantChkBx";
        private const string colWithGrant = "isgrantwith";
        private const string colWithGrantCheckBox = "WithGrantChkBx";
        private const string colDeny = "isdeny";
        private const string colDenyCheckBox = "DenyChkBx";
        private const string colGrantee = "grantee";
        private const string colGrantor = "grantor";
        private const string colObjectType = "objecttype";
        private const string colObjectTypeName = "objecttypename";

        // Column headers
        private const string colIconHdr = "";
        private const string colObjectNameHdr = "Object";
        private const string colPermissionHdr = "Permission";
        private const string colGrantCheckBoxHdr = "Grant";
        private const string colWithGrantCheckBoxHdr = "With Grant";
        private const string colDenyCheckBoxHdr = "Deny";
        private const string colGranteeHdr = "Grantee";
        private const string colGrantorHdr = "Grantor";
        private const string colObjectTypeNameHdr = "Type";

        #endregion

        #region Fields

        private const int IconIndex = 0;
        private const int GrantIndex = 4;
        private const int WithGrantIndex = 6;
        private const int DenyIndex = 8;
        private const int ObjectTypeIndex = 11;

        private const string GatheringPermissions = "Gathering permissions";
        private const string PermissionsAssigned = "Permissions";
        private const string NoPermissions = "No permissions found";

        private Sql.ServerVersion m_Version = Sql.ServerVersion.Unsupported;
        private Sql.ObjectTag m_Tag = null;

        #endregion

        #region Helpers

        private void showProgress(
                int val,
                string status
            )
        {
            _progressBar.Value = val;
            _lbl_Status.Text = status;
            _progressBar.Visible = true;
            _lbl_Status.Visible = true;
            _statusStrip.Refresh();
        }

        private void finishProgress()
        {
            _progressBar.Value = 100;
            _progressBar.Visible = true;
            _statusStrip.Refresh();

            _progressBar.Visible = false;
            _lbl_Status.Visible = false;
            _statusStrip.Refresh();
        }

        private string numItemsStr(
                int num
            )
        {
            return (" (" + num.ToString() + " Items)");
        }

        private DataTable getGridDisplayTbl(DataSet ds)
        {
            // Fill the return data table.
            DataTable dt = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                // Create data table object with selected columns.
                DataView dv = new DataView(ds.Tables[0]);
                dt = dv.ToTable("Permissions", true,
                                                new string[] { 
                                                colObjectName, colPermission, colGrant, colWithGrant, colDeny, colGrantor, //colGrantee, 
                                                colObjectType
                                            });

                // Add the icon column to the begining of the data table.
                dt.Columns.Add(colIcon, typeof(Image));
                dt.Columns[colIcon].SetOrdinal(IconIndex);

                // Add permission check boxes.
                dt.Columns.Add(colGrantCheckBox, typeof(bool));
                dt.Columns[colGrantCheckBox].SetOrdinal(GrantIndex);
                dt.Columns.Add(colWithGrantCheckBox, typeof(bool));
                dt.Columns[colWithGrantCheckBox].SetOrdinal(WithGrantIndex);
                dt.Columns.Add(colDenyCheckBox, typeof(bool));
                dt.Columns[colDenyCheckBox].SetOrdinal(DenyIndex);

                // Add object type.
                dt.Columns.Add(colObjectTypeName, typeof(string));
                dt.Columns[colObjectTypeName].SetOrdinal(ObjectTypeIndex);

                // Populate the icon column for each row.
                foreach (DataRow row in dt.Rows)
                {
                    //Get the type of object.
                    Sql.ObjectType.TypeEnum type = Sql.ObjectType.ToTypeEnum(row[colObjectType].ToString());
                    row[colIcon] = Sql.ObjectType.TypeImage16(type);

                    // Get the permissions and set check boxes.
                    row[colGrantCheckBox] = string.Compare(row[colGrant].ToString(), Utility.Permissions.Grants.True, true) == 0;
                    row[colWithGrantCheckBox] = string.Compare(row[colWithGrant].ToString(), Utility.Permissions.Grants.True, true) == 0;
                    row[colDenyCheckBox] = string.Compare(row[colDeny].ToString(), Utility.Permissions.Grants.True, true) == 0;

                    // Set the object type.
                    row[colObjectTypeName] = Sql.ObjectType.TypeName(type);
                }
            }
            else
            {
                // Create table and configure columns.
                dt = new DataTable();
                dt.Columns.Add(colIcon);
                dt.Columns.Add(colObjectName);
                dt.Columns.Add(colPermission);
                dt.Columns.Add(colGrant);
                dt.Columns.Add(colGrantCheckBox, typeof(bool));
                dt.Columns.Add(colWithGrant);
                dt.Columns.Add(colWithGrantCheckBox, typeof(bool));
                dt.Columns.Add(colDeny);
                dt.Columns.Add(colDenyCheckBox, typeof(bool));
                dt.Columns.Add(colGrantee);
                dt.Columns.Add(colGrantor);
                dt.Columns.Add(colObjectTypeName);
                dt.Columns.Add(colObjectType);
            }

            return dt;
        }

        private void updateGrid()
        {
            Debug.Assert(m_Version != Sql.ServerVersion.Unsupported);
            Debug.Assert(m_Tag != null);

            // Initialize progress status
            string inProgressStatus = GatheringPermissions;
            string newDispPermissionsStr = PermissionsAssigned;

            // Retrieve the permissions data set.
            this.Cursor = Cursors.WaitCursor;
            showProgress(5, inProgressStatus);
            using (DataSet ds = Sql.ServerRolePermissions.GetServerRolePermissions(m_Tag))
            {
                // Update progress.
                showProgress(30, inProgressStatus);

                // Display the data in the grid.
                int numPermissions = 0;
                using (DataTable dt = getGridDisplayTbl(ds))
                {
                    // Update progress
                    showProgress(60, inProgressStatus);

                    // Get num permissions.
                    numPermissions = dt.Rows.Count;

                    // If no permissions fill with "no permissions" data.
                    if (numPermissions == 0)
                    {
                        dt.Rows.Add(null, NoPermissions, null, null, false, null, false, null, false, null, null);
                    }

                    // Update the grid.
                    _ultraGrid.BeginUpdate();
                    _ultraGrid.SetDataBinding(dt.DefaultView, null);
                    _ultraGrid.EndUpdate();

                    // Update progress
                    showProgress(90, inProgressStatus);
                }

                // Set the what is displayed label & field.
                _lbl_PermissionType.Text = newDispPermissionsStr + numItemsStr(numPermissions);
            }

            // Finalize progress
            finishProgress();
            this.Cursor = Cursors.Default;
        }

        private void selectGridColumns()
        {
            Forms.Form_GridColumnChooser.Process(_ultraGrid, "Object Permissions");

            // Now determine which of the columns are visible and remember it.
            UltraGridBand band = _ultraGrid.DisplayLayout.Bands[0];
            Utility.UserData.Current.ObjectPermissions.IsIconHidden = band.Columns[colIcon].Hidden;
            Utility.UserData.Current.ObjectPermissions.IsPermissionHidden = band.Columns[colPermission].Hidden;
            //Utility.UserData.Current.ObjectPermissions.IsGranteeHidden = band.Columns[colGrantee].Hidden;
            Utility.UserData.Current.ObjectPermissions.IsGrantCheckBoxHidden = band.Columns[colGrantCheckBox].Hidden;
            Utility.UserData.Current.ObjectPermissions.IsWIthGrantCheckBoxHidden = band.Columns[colWithGrantCheckBox].Hidden;
            Utility.UserData.Current.ObjectPermissions.IsDenyCheckBoxHidden = band.Columns[colDenyCheckBox].Hidden;
            Utility.UserData.Current.ObjectPermissions.IsGrantorHidden = band.Columns[colGrantor].Hidden;
            Utility.UserData.Current.ObjectPermissions.IsObjectNameHidden = band.Columns[colObjectName].Hidden;
            Utility.UserData.Current.ObjectPermissions.IsObjectTypeNameHidden = band.Columns[colObjectTypeName].Hidden;
        }

        private void toggleGroupByBox()
        {
            _ultraGrid.DisplayLayout.GroupByBox.Hidden = !_ultraGrid.DisplayLayout.GroupByBox.Hidden;
        }

        private void saveGrid()
        {
            Cursor = Cursors.WaitCursor;
            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _ultraGridExcelExporter.Export(_ultraGrid, _saveFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MsgBox.ShowError(ErrorMsgs.ExportToExcelCaption, ErrorMsgs.FailedToExportToExcelFile, ex);
                }
            }
            Cursor = Cursors.Default;
        }

        private void printGrid()
        {
            _ultraGridPrintDocument.DocumentName = "Permissions assigned to " + m_Tag.ObjectName;
            _ultraGridPrintDocument.Grid = _ultraGrid;
            _ultraPrintPreviewDialog.Document = _ultraGridPrintDocument;
            _ultraPrintPreviewDialog.ShowDialog();
        }

        #endregion

        #region Ctors

        public ServerPrincipalPermissionsGrid()
        {
            InitializeComponent();

            // Set the button images.
            _mi_ShowGroupByBox.Image = _tsbtn_GroupByBox.Image = AppIcons.AppImage16(AppIcons.Enum.GridGroupBy);
            _mi_Print.Image = _tsbtn_Print.Image = AppIcons.AppImage16(AppIcons.Enum.Print);
            _mi_SaveToExcel.Image = _tsbtn_SaveAs.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);
            _mi_ColumnChooser.Image = _tsbtn_ColumnChooser.Image = AppIcons.AppImage16(AppIcons.Enum.GridFieldChooser);

            // Hide the progress controls.
            _progressBar.Visible = false;
            _lbl_Status.Visible = false;

            // Setup the print document format.
            _ultraGridPrintDocument.DefaultPageSettings.Landscape = true;
            _ultraGridPrintDocument.DefaultPageSettings.Color = false;
        }

        #endregion

        #region Event Handlers

        private void _ultraGrid_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            e.Layout.Override.GroupByRowInitialExpansionState = GroupByRowInitialExpansionState.Collapsed;
            e.Layout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.True;

            UltraGridBand band = e.Layout.Bands[0];

            // Icon.
            band.Columns[colIcon].Header.Caption = colIconHdr;
            band.Columns[colIcon].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            band.Columns[colIcon].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            band.Columns[colIcon].ColumnChooserCaption = "Icon";
            band.Columns[colIcon].Width = 22;
            band.Columns[colIcon].Hidden = Utility.UserData.Current.ObjectPermissions.IsIconHidden;

            // Object name.
            band.Columns[colObjectName].Header.Caption = colObjectNameHdr;
            band.Columns[colObjectName].Width = 200;
            band.Columns[colObjectName].Hidden = Utility.UserData.Current.ObjectPermissions.IsObjectNameHidden;

            // Permission
            band.Columns[colPermission].Header.Caption = colPermissionHdr;
            band.Columns[colPermission].Width = 140;
            band.Columns[colPermission].Hidden = Utility.UserData.Current.ObjectPermissions.IsPermissionHidden;

            // Grant
            int permWidth = 40;
            band.Columns[colGrant].Hidden = true;
            band.Columns[colGrant].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            band.Columns[colGrantCheckBox].Header.Caption = colGrantCheckBoxHdr;
            band.Columns[colGrantCheckBox].Width = permWidth;
            band.Columns[colGrantCheckBox].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            band.Columns[colGrantCheckBox].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            band.Columns[colGrantCheckBox].Hidden = Utility.UserData.Current.ObjectPermissions.IsGrantCheckBoxHidden;

            // With grant
            band.Columns[colWithGrant].Hidden = true;
            band.Columns[colWithGrant].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            band.Columns[colWithGrantCheckBox].Header.Caption = colWithGrantCheckBoxHdr;
            band.Columns[colWithGrantCheckBox].Width = permWidth;
            band.Columns[colWithGrantCheckBox].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            band.Columns[colWithGrantCheckBox].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            band.Columns[colWithGrantCheckBox].Hidden = Utility.UserData.Current.ObjectPermissions.IsWIthGrantCheckBoxHidden;

            // Deny
            band.Columns[colDeny].Hidden = true;
            band.Columns[colDeny].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            band.Columns[colDenyCheckBox].Header.Caption = colDenyCheckBoxHdr;
            band.Columns[colDenyCheckBox].Width = permWidth;
            band.Columns[colDenyCheckBox].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            band.Columns[colDenyCheckBox].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            band.Columns[colDenyCheckBox].Hidden = Utility.UserData.Current.ObjectPermissions.IsDenyCheckBoxHidden;

            // Grantee.
            //band.Columns[colGrantee].Header.Caption = colGranteeHdr;
            //band.Columns[colGrantee].Width = 100;
            //band.Columns[colGrantee].Hidden = Utility.UserData.Current.ObjectPermissions.IsGranteeHidden;

            // Grantor
            band.Columns[colGrantor].Header.Caption = colGrantorHdr;
            band.Columns[colGrantor].Width = 100;
            band.Columns[colGrantor].Hidden = Utility.UserData.Current.ObjectPermissions.IsGrantorHidden;

            // Object type name.
            band.Columns[colObjectTypeName].Header.Caption = colObjectTypeNameHdr;
            band.Columns[colObjectTypeName].Width = 100;
            band.Columns[colObjectTypeName].Hidden = Utility.UserData.Current.ObjectPermissions.IsObjectTypeNameHidden;

            // Object type (hidden)
            band.Columns[colObjectType].Hidden = true;
            band.Columns[colObjectType].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        }

        private void _tsbtn_ColumnChooser_Click(object sender, EventArgs e)
        {
            selectGridColumns();
        }

        private void _tsbtn_GroupByBox_Click(object sender, EventArgs e)
        {
            toggleGroupByBox();
        }

        private void _tsbtn_SaveAs_Click(object sender, EventArgs e)
        {
            saveGrid();
        }

        private void _tsbtn_Print_Click(object sender, EventArgs e)
        {
            printGrid();
        }

        private void _mi_ColumnChooser_Click(object sender, EventArgs e)
        {
            selectGridColumns();
        }

        private void _mi_ShowGroupByBox_Click(object sender, EventArgs e)
        {
            toggleGroupByBox();
        }

        private void _mi_SaveToExcel_Click(object sender, EventArgs e)
        {
            saveGrid();
        }

        private void _mi_Print_Click(object sender, EventArgs e)
        {
            printGrid();
        }

        #endregion

        #region Methods

        public void Initialize(
                Sql.ServerVersion version,
                Sql.ObjectTag tag
            )
        {
            m_Version = version;
            m_Tag = tag;
            updateGrid();
        }

        #endregion
    }
}
