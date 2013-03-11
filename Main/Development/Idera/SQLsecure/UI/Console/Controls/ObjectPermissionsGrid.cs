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
    public partial class ObjectPermissionsGrid : UserControl
    {
        #region Columns

        // columns.
        private const string colIcon = "Icon";
        private const string colObjectType = "objecttype";
        private const string colObjectTypeName = "objecttypename";
        private const string colObjectName = "objectname";
        private const string colGrantee = "granteename";
        private const string colPermission = "permission";
        private const string colGrant = "isgrant";
        private const string colGrantCheckBox = "GrantChkBx";
        private const string colWithGrant = "isgrantwith";
        private const string colWithGrantCheckBox = "WithGrantChkBx";
        private const string colDeny = "isdeny";
        private const string colDenyCheckBox = "DenyChkBx";
        private const string colGrantor = "grantorname";
        private const string colInherited = "inherited";
        private const string colSourceName = "sourcename";
        private const string colSourceType = "sourcetype";
        private const string colSourcePermission = "sourcepermission";

        // Column headers
        private const string colIconHdr = "";
        private const string colObjectNameHdr = "Object";
        private const string colGranteeHdr = "Grantee";
        private const string colPermissionHdr = "Permission";
        private const string colGrantCheckBoxHdr = "Grant";
        private const string colWithGrantCheckBoxHdr = "With Grant";
        private const string colDenyCheckBoxHdr = "Deny";
        private const string colGrantorHdr = "Grantor";
        private const string colObjectTypeNameHdr = "Type";
        private const string colSourceNameHdr = "Source Object";
        private const string colSourceTypeHdr = "Source Type";
        private const string colSourcePermissionHdr = "Source Permission";

        #endregion

        #region Fields

        private enum DisplayedPermissions { None, Explicit, All };

        private const int IconIndex = 0;
        private const int GrantIndex = 4;
        private const int WithGrantIndex = 6;
        private const int DenyIndex = 8;

        private const string GatheringExplicitPermissions = "Gathering explicit permissions";
        private const string GatheringAllPermissions = "Gathering all permissions";
        private const string ExplicitPermissions = "Explicit permissions";
        private const string AllPermissions = "All permissions";
        private const string NoPermissions = "No permissions found";

        private Sql.ServerVersion m_Version = Sql.ServerVersion.Unsupported;
        private Sql.ObjectTag m_Tag = null;
        private DisplayedPermissions m_DisplayedPermissions = DisplayedPermissions.None;

        private bool m_IsColumn = false;

        #endregion

        #region Helpers

        private bool hideSourceInfo
        {
            get { return _rdbtn_ExplicitOnly.Checked || m_Version == Sql.ServerVersion.SQL2000; }
        }

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
                                                colGrantee, colPermission, colGrant, colWithGrant, colDeny, colGrantor, 
                                                colObjectName, colObjectTypeName, colObjectType, 
                                                colSourcePermission, colInherited, colSourceName, colSourceType
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

                // Process each row for additional info.
                m_IsColumn = false;
                foreach (DataRow row in dt.Rows)
                {
                    //Get the type of object.
                    Sql.ObjectType.TypeEnum type = Sql.ObjectType.ToTypeEnum(row[colObjectType].ToString());
                    row[colIcon] = Sql.ObjectType.TypeImage16(type);

                    // Check if type is column, this is used to display the object column.
                    if (type == Sql.ObjectType.TypeEnum.Column) 
                    {
                        m_IsColumn = true;
                    }

                    // Get the permissions and set check boxes.
                    row[colGrantCheckBox] = string.Compare(row[colGrant].ToString(), Utility.Permissions.Grants.True, true) == 0;
                    row[colWithGrantCheckBox] = string.Compare(row[colWithGrant].ToString(), Utility.Permissions.Grants.True, true) == 0;
                    row[colDenyCheckBox] = string.Compare(row[colDeny].ToString(), Utility.Permissions.Grants.True, true) == 0;

                    // Set the source information, if the permission is inherited.
                    // Only for server, database and schema.
                    string sourceName = string.Empty,
                           sourceType = string.Empty;
                    if (string.Compare(row[colInherited].ToString(), "Y", true) == 0)
                    {
                        sourceType = row[colSourceType].ToString();
                        if (string.Compare(sourceType, "Server", true) == 0
                            || string.Compare(sourceType, "Schema", true) == 0)
                        {
                            sourceName = row[colSourceName].ToString();
                            sourceType = row[colSourceType].ToString();
                        }
                        else if (string.Compare(sourceType, "Database Role", true) == 0)
                        {
                            sourceName = row[colSourceName].ToString();
                            sourceType = "Database";
                        }
                        else
                        {
                            sourceType = string.Empty;
                        }
                    }
                    row[colSourceName] = sourceName;
                    row[colSourceType] = sourceType;
                }
            }
            else
            {
                // Create table and configure columns.
                dt = new DataTable();
                dt.Columns.Add(colIcon);
                dt.Columns.Add(colGrantee);
                dt.Columns.Add(colPermission);
                dt.Columns.Add(colGrant);
                dt.Columns.Add(colGrantCheckBox, typeof(bool));
                dt.Columns.Add(colWithGrant);
                dt.Columns.Add(colWithGrantCheckBox, typeof(bool));
                dt.Columns.Add(colDeny);
                dt.Columns.Add(colDenyCheckBox, typeof(bool));
                dt.Columns.Add(colGrantor); 
                dt.Columns.Add(colObjectName);
                dt.Columns.Add(colObjectTypeName);
                dt.Columns.Add(colObjectType);
                dt.Columns.Add(colInherited);
                dt.Columns.Add(colSourceName);
                dt.Columns.Add(colSourceType);
                dt.Columns.Add(colSourcePermission);
            }

            return dt;
        }

        private void updateGrid()
        {
            Debug.Assert(m_Version != Sql.ServerVersion.Unsupported);
            Debug.Assert(m_Tag != null);

            // Check if we need to refresh the grid, based on what is being displayed.
            if (m_DisplayedPermissions == DisplayedPermissions.None
               || (m_DisplayedPermissions == DisplayedPermissions.Explicit && !_rdbtn_ExplicitOnly.Checked)
               || (m_DisplayedPermissions == DisplayedPermissions.All && _rdbtn_ExplicitOnly.Checked))
            {
                // Initialize progress status
                string inProgressStatus = _rdbtn_ExplicitOnly.Checked ? GatheringExplicitPermissions : GatheringAllPermissions;
                string newDispPermissionsStr = _rdbtn_ExplicitOnly.Checked ? ExplicitPermissions : AllPermissions;
                DisplayedPermissions newDispPermissions = _rdbtn_ExplicitOnly.Checked ? DisplayedPermissions.Explicit : DisplayedPermissions.All;

                // Retrieve the permissions data set.
                this.Cursor = Cursors.WaitCursor;
                showProgress(5, inProgressStatus);
                using (DataSet ds = Sql.ObjectPermissions.GetObjectPermissions(m_Tag, _rdbtn_ExplicitOnly.Checked))
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
                        if(numPermissions == 0)
                        {
                            dt.Rows.Add(null, NoPermissions, null, null, false, null, false, null, false, null, null, null);
                        }

                        // Update the grid.
                        _ultraGrid.BeginUpdate();
                        _ultraGrid.SetDataBinding(dt.DefaultView, null);
                        _ultraGrid.EndUpdate();

                        // Sort by permission type.
                        _ultraGrid.DisplayLayout.Bands[0].Columns[colGrantee].SortIndicator = SortIndicator.Ascending;

                        // Update progress
                        showProgress(90, inProgressStatus);
                    }

                    // Set the what is displayed label & field.
                    _lbl_PermissionType.Text = newDispPermissionsStr + numItemsStr(numPermissions);
                    m_DisplayedPermissions = newDispPermissions; 
                }

                // Finalize progress
                finishProgress();
                this.Cursor = Cursors.Default;
            }
        }

        private void selectGridColumns()
        {
            Forms.Form_GridColumnChooser.Process(_ultraGrid, "Object Permissions");

            // Now determine which of the columns are visible and remember it.
            UltraGridBand band = _ultraGrid.DisplayLayout.Bands[0];
            Utility.UserData.Current.ObjectPermissions.IsIconHidden = band.Columns[colIcon].Hidden;
            Utility.UserData.Current.ObjectPermissions.IsPermissionHidden = band.Columns[colPermission].Hidden;
            Utility.UserData.Current.ObjectPermissions.IsGranteeHidden = band.Columns[colGrantee].Hidden;
            Utility.UserData.Current.ObjectPermissions.IsGrantCheckBoxHidden = band.Columns[colGrantCheckBox].Hidden;
            Utility.UserData.Current.ObjectPermissions.IsWIthGrantCheckBoxHidden = band.Columns[colWithGrantCheckBox].Hidden;
            Utility.UserData.Current.ObjectPermissions.IsDenyCheckBoxHidden = band.Columns[colDenyCheckBox].Hidden;
            Utility.UserData.Current.ObjectPermissions.IsGrantorHidden = band.Columns[colGrantor].Hidden;
            Utility.UserData.Current.ObjectPermissions.IsObjPermObjectNameHidden = band.Columns[colObjectName].Hidden;
            if (!hideSourceInfo)
            {
                Utility.UserData.Current.ObjectPermissions.IsSourceNameHidden = band.Columns[colSourceName].Hidden;
                Utility.UserData.Current.ObjectPermissions.IsSourceTypeHidden = band.Columns[colSourceType].Hidden;
                Utility.UserData.Current.ObjectPermissions.IsSourcePermissionHidden = band.Columns[colSourcePermission].Hidden;
            }
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
            _ultraGridPrintDocument.DocumentName = m_Tag.DatabaseName + "." + m_Tag.ObjectName + "  Permissions";
            _ultraGridPrintDocument.Grid = _ultraGrid;
            _ultraPrintPreviewDialog.Document = _ultraGridPrintDocument;
            _ultraPrintPreviewDialog.ShowDialog();
        }

        #endregion

        #region Ctors

        public ObjectPermissionsGrid()
        {
            InitializeComponent();

            // Set the button images.
            _mi_ShowGroupByBox.Image = _tsbtn_GroupByBox.Image = AppIcons.AppImage16(AppIcons.Enum.GridGroupBy);
            _mi_Print.Image = _tsbtn_Print.Image = AppIcons.AppImage16(AppIcons.Enum.Print);
            _mi_SaveToExcel.Image = _tsbtn_SaveAs.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);
            _mi_ColumnChooser.Image = _tsbtn_ColumnChooser.Image = AppIcons.AppImage16(AppIcons.Enum.GridFieldChooser);
            
            // Select the explicit permissions.
            _rdbtn_ExplicitOnly.Checked = true;

            // Hide the progress controls.
            _progressBar.Visible = false;
            _lbl_Status.Visible = false;

            // Setup the print document format.
            _ultraGridPrintDocument.DefaultPageSettings.Landscape = true;
            _ultraGridPrintDocument.DefaultPageSettings.Color = false;
            //_ultraGridPrintDocument.FitWidthToPages = 1;
        }

        #endregion

        #region Methods

        public void Initialize(
                Sql.ServerVersion version,
                Sql.ObjectTag tag
            )
        {
            Debug.Assert(version != Sql.ServerVersion.Unsupported);
            Debug.Assert(tag != null);

            // Setup the fields.
            m_Version = version;
            m_Tag = tag;

            // Update the grid with permissions.
            updateGrid();
        }

        #endregion

        #region Event Handler

        private void _ultraGrid_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            e.Layout.Override.GroupByRowInitialExpansionState = GroupByRowInitialExpansionState.Collapsed;
            e.Layout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.True;

            UltraGridBand band = e.Layout.Bands[0];

            // Display widths.
            int iconWidth = 22;
            int permissionNameWidth = 175;
            int granteeorWidth = 100;
            int permWidth = 40;
            int objNameWidth = 100;
            int sourcePermissionNameWidth = 80;
            int sourceObjNameWidth = 80;
            int sourceObjTypeWidth = 60;

            // Icon.
            band.Columns[colIcon].Header.Caption = colIconHdr;
            band.Columns[colIcon].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            band.Columns[colIcon].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            band.Columns[colIcon].ColumnChooserCaption = "Icon";
            band.Columns[colIcon].Width = iconWidth;
            band.Columns[colIcon].Hidden = Utility.UserData.Current.ObjectPermissions.IsIconHidden;

            // Permission
            band.Columns[colPermission].Header.Caption = colPermissionHdr;
            band.Columns[colPermission].Width = permissionNameWidth;
            band.Columns[colPermission].Hidden = Utility.UserData.Current.ObjectPermissions.IsPermissionHidden;

            // Grantee.
            band.Columns[colGrantee].Header.Caption = colGranteeHdr;
            band.Columns[colGrantee].Width = granteeorWidth;
            band.Columns[colGrantee].Hidden = Utility.UserData.Current.ObjectPermissions.IsGranteeHidden;

            // Grant
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

            // Grantor
            band.Columns[colGrantor].Header.Caption = colGrantorHdr;
            band.Columns[colGrantor].Width = granteeorWidth;
            band.Columns[colGrantor].Hidden = Utility.UserData.Current.ObjectPermissions.IsGrantorHidden;

            // Object name, show it only if there are column permissions.
            band.Columns[colObjectName].Header.Caption = colObjectNameHdr;
            band.Columns[colObjectName].Width = objNameWidth;
            if (m_IsColumn)
            {
                band.Columns[colObjectName].Hidden = false;
            }
            else
            {
                band.Columns[colObjectName].Hidden = Utility.UserData.Current.ObjectPermissions.IsObjPermObjectNameHidden;
            }

            // If its explicit or SQL Server 2000 then hide the source information.
            if (hideSourceInfo)
            {
                // Source permission.
                band.Columns[colSourcePermission].Hidden = true;
                band.Columns[colSourcePermission].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                // Source name.
                band.Columns[colSourceName].Hidden = true;
                band.Columns[colSourceName].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                // Source type. 
                band.Columns[colSourceType].Hidden = true;
                band.Columns[colSourceType].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            }
            else
            {
                // Source permission.
                band.Columns[colSourcePermission].Header.Caption = colSourcePermissionHdr;
                band.Columns[colSourcePermission].Width = sourcePermissionNameWidth;
                band.Columns[colSourcePermission].Hidden = Utility.UserData.Current.ObjectPermissions.IsSourcePermissionHidden;
                band.Columns[colSourcePermission].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                // Source name.
                band.Columns[colSourceName].Header.Caption = colSourceNameHdr;
                band.Columns[colSourceName].Width = sourceObjNameWidth;
                band.Columns[colSourceName].Hidden = Utility.UserData.Current.ObjectPermissions.IsSourceNameHidden;
                band.Columns[colSourceName].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                // Source type. 
                band.Columns[colSourceType].Header.Caption = colSourceTypeHdr;
                band.Columns[colSourceType].Width = sourceObjTypeWidth;
                band.Columns[colSourceType].Hidden = Utility.UserData.Current.ObjectPermissions.IsSourceTypeHidden;
                band.Columns[colSourceType].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }

            // Inherited (hidden)
            band.Columns[colInherited].Hidden = true;
            band.Columns[colInherited].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            // Object type name (hidden)
            band.Columns[colObjectTypeName].Hidden = true;
            band.Columns[colObjectTypeName].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            // Object type (hidden)
            band.Columns[colObjectType].Hidden = true;
            band.Columns[colObjectType].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        }

        private void _tsbtn_ColumnChooser_Click(object sender, EventArgs e)
        {
            selectGridColumns();
        }

        private void _btn_ShowPermissions_Click(object sender, EventArgs e)
        {
            updateGrid();
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
    }
}
