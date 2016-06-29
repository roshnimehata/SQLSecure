using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Security.AccessControl;
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
    public partial class OsObjectPermissionsGrid : UserControl
    {
        #region Columns

        // columns.
        private const string colIcon = "Icon";
        private const string colFileSystemRights = "filesystemrights";
        private const string colAuditFlags = "auditflags";
        private const string colUser = "username";
        private const string colAccessType = "accesstype";
        private const string colInherited = "isinherited";

        // Column headers
        private const string colIconHdr = "";
        private const string colFileSystemRightsHdr = "Rights";
        private const string colAuditFlagsHdr = "Auditing";
        private const string colUserHdr = "User Name";
        private const string colAccessTypeHdr = "Access Type";
        private const string colInheritedHdr = "Inherited";

        #endregion

        #region Fields

        private enum DisplayedPermissions { None, Explicit, All };

        private const int IconIndex = 0;
        private const int GrantIndex = 4;
        private const int WithGrantIndex = 6;
        private const int DenyIndex = 8;

        private const string GatheringExplicitPermissions = "Gathering explicit rights";
        private const string GatheringAllPermissions = "Gathering all rights";
        private const string ExplicitPermissions = "Explicit rights";
        private const string AllPermissions = "All rights";
        private const string NoPermissions = "No rights found";

        private Sql.ServerVersion m_Version = Sql.ServerVersion.Unsupported;
        private Sql.ObjectTag m_Tag = null;
        private DisplayedPermissions m_DisplayedPermissions = DisplayedPermissions.None;

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

        private string numItemsStr(int num)
        {
            return (" (" + num.ToString() + " Items)");
        }

        private DataTable getGridDisplayTbl(DataSet ds)
        {
            // Fill the return data table.
            DataTable dt = null;
            // Create table and configure columns.
            dt = new DataTable();
            dt.Columns.Add(colIcon);
            dt.Columns.Add(colFileSystemRights);
            dt.Columns.Add(colAuditFlags);
            dt.Columns.Add(colUser);
            dt.Columns.Add(colAccessType);
            dt.Columns.Add(colInherited, typeof(bool));

            if (ds != null && ds.Tables.Count > 0)
            {
                // Process each row and store permissions in table
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    bool isInherited = (string) row[colInherited] == "Y" ? true : false;
                    if (!isInherited || !_rdbtn_ExplicitOnly.Checked)
                    {
                        string rights;
                        if (m_Tag.ObjType == Sql.ObjectType.TypeEnum.RegistryKey)
                        {
                            rights = ((RegistryRights)row[colFileSystemRights]).ToString();
                        }
                        else
                        {
                            rights = ((FileSystemRights)row[colFileSystemRights]).ToString();
                        }

                        string auditFlags = string.Empty;
                        if (row[colAuditFlags] != DBNull.Value)
                        {
                            foreach (int flag in Enum.GetValues(typeof (AuditFlags)))
                            {
                                //skip 0 because that is the None value which is not displayed in the security settings
                                if (flag > 0 && ((int)row[colAuditFlags] & flag) == flag)
                                {
                                    auditFlags += (auditFlags.Length > 0 ? ", " : string.Empty) +
                                                  DescriptionHelper.GetEnumDescription((AuditFlags) flag);
                                }
                            }
                        }

                        string access = row[colAccessType].ToString();
                        if (row[colAccessType] != DBNull.Value)
                        {
                            foreach (int flag in Enum.GetValues(typeof (AccessControlType)))
                            {
                                if ((int) row[colAccessType] == flag)
                                {
                                    // These cannot be combined
                                    access = DescriptionHelper.GetEnumDescription((AccessControlType) flag);
                                    break;
                                }
                            }
                        }

                        dt.Rows.Add(new object[]
                                        {
                                            null,
                                            rights,
                                            auditFlags,
                                            (string)row[colUser],
                                            access,
                                            (string)row[colInherited] == "Y" ? true : false
                                        });
                    }
                }
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
                using (DataSet ds = Sql.ObjectPermissions.GetOsObjectPermissions(m_Tag))
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
                            dt.Rows.Add(null, NoPermissions, null, null, null, false);
                        }

                        // Update the grid.
                        _ultraGrid.BeginUpdate();
                        _ultraGrid.SetDataBinding(dt.DefaultView, null);
                        _ultraGrid.EndUpdate();

                        // Sort by permission type.
                        _ultraGrid.DisplayLayout.Bands[0].Columns[colFileSystemRights].SortIndicator = SortIndicator.Ascending;

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
            Forms.Form_GridColumnChooser.Process(_ultraGrid, "OS Object Rights");

            // Now determine which of the columns are visible and remember it.
            UltraGridBand band = _ultraGrid.DisplayLayout.Bands[0];
            //Utility.UserData.Current.ObjectPermissions.IsIconHidden = band.Columns[colIcon].Hidden;
            //Utility.UserData.Current.ObjectPermissions.IsPermissionHidden = band.Columns[colPermission].Hidden;
            //Utility.UserData.Current.ObjectPermissions.IsGranteeHidden = band.Columns[colGrantee].Hidden;
            //Utility.UserData.Current.ObjectPermissions.IsGrantCheckBoxHidden = band.Columns[colGrantCheckBox].Hidden;
            //Utility.UserData.Current.ObjectPermissions.IsWIthGrantCheckBoxHidden = band.Columns[colWithGrantCheckBox].Hidden;
            //Utility.UserData.Current.ObjectPermissions.IsDenyCheckBoxHidden = band.Columns[colDenyCheckBox].Hidden;
            //Utility.UserData.Current.ObjectPermissions.IsGrantorHidden = band.Columns[colGrantor].Hidden;
            //Utility.UserData.Current.ObjectPermissions.IsObjPermObjectNameHidden = band.Columns[colObjectName].Hidden;
            //Utility.UserData.Current.ObjectPermissions.IsSourceNameHidden = band.Columns[colSourceName].Hidden;
            //Utility.UserData.Current.ObjectPermissions.IsSourceTypeHidden = band.Columns[colSourceType].Hidden;
            //Utility.UserData.Current.ObjectPermissions.IsSourcePermissionHidden = band.Columns[colSourcePermission].Hidden;
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
            _ultraGridPrintDocument.DocumentName = m_Tag.DatabaseName + "." + m_Tag.ObjectName + "  Rights";
            _ultraGridPrintDocument.Grid = _ultraGrid;
            _ultraPrintPreviewDialog.Document = _ultraGridPrintDocument;
            _ultraPrintPreviewDialog.ShowDialog();
        }

        #endregion

        #region Ctors

        public OsObjectPermissionsGrid()
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

            // Icon.
            band.Columns[colIcon].Header.Caption = colIconHdr;
            band.Columns[colIcon].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            band.Columns[colIcon].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            band.Columns[colIcon].ColumnChooserCaption = "Icon";
            band.Columns[colIcon].Width = 22;
            //band.Columns[colIcon].Hidden = Utility.UserData.Current.ObjectPermissions.IsIconHidden;

            // Rights
            band.Columns[colFileSystemRights].Header.Caption = colFileSystemRightsHdr;
            band.Columns[colFileSystemRights].Width = 170;
            //band.Columns[colFileSystemRights].Hidden = Utility.UserData.Current.ObjectPermissions.IsPermissionHidden;

            // Audit Flags.
            band.Columns[colAuditFlags].Header.Caption = colAuditFlagsHdr;
            band.Columns[colAuditFlags].Width = 80;
            //band.Columns[colAuditFlags].Hidden = Utility.UserData.Current.ObjectPermissions.IsGranteeHidden;

            // User
            band.Columns[colUser].Header.Caption = colUserHdr;
            band.Columns[colUser].Width = 160;
            //band.Columns[colUser].Hidden = Utility.UserData.Current.ObjectPermissions.IsGrantorHidden;

            // Access Type
            band.Columns[colAccessType].Header.Caption = colAccessTypeHdr;
            band.Columns[colAccessType].Width = 80;
            //band.Columns[colAccessType].Hidden = Utility.UserData.Current.ObjectPermissions.IsGrantorHidden;

            // Inherited
            band.Columns[colInherited].Hidden = true;
            band.Columns[colInherited].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
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
