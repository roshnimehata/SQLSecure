using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Security.AccessControl;

using Idera.SQLsecure.Core.Accounts;
using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Controls;
using Idera.SQLsecure.UI.Console.Sql;
using Idera.SQLsecure.UI.Console.Utility;

using Infragistics.Win.UltraWinGrid;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_OSObjectPermissions : Idera.SQLsecure.UI.Console.Controls.BaseDialog
    {
        #region Ctors

        public Form_OSObjectPermissions(int snapshotId, string name, byte[] sid)
        {
            InitializeComponent();

            _toolStripButton_ColumnChooser.Image = AppIcons.AppImage16(AppIcons.Enum.GridFieldChooser);
            _toolStripButton_GroupBy.Image = AppIcons.AppImage16(AppIcons.Enum.GridGroupBy);
            _toolStripButton_Save.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);
            _toolStripButton_Print.Image = AppIcons.AppImage16(AppIcons.Enum.Print);

            groupsSave.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);
            groupsPrint.Image = AppIcons.AppImage16(AppIcons.Enum.Print);

            _grid.DisplayLayout.GroupByBox.Hidden = Utility.Constants.InitialState_GroupByBoxHidden;

            groupsGrid.DisplayLayout.GroupByBox.Hidden = Utility.Constants.InitialState_GroupByBoxHidden;
            
            m_snapshotId = snapshotId;
            m_name = name;
            m_sid = sid;
            m_snapshotId = snapshotId;
            _labelPermission.Text = userGroupsLabel.Text = String.Format("Account: {0}", name);
        }

        #endregion

        #region Fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Forms.Form_OSObjectPermissions");

        private int m_snapshotId;
        private string m_name;
        private byte[] m_sid;

        #endregion

        #region Properties

        #endregion

        #region Queries, Columns & Constants

        private const string QueryOsObjectPermissions = "SELECT\r\n" +
                                                        "cast(isnull(obj.longname, obj.objectname) as nvarchar(max)) AS longname, \r\n" +
                                                        "obj.objecttype, \r\n" +
                                                        "NULL as accesstype,\r\n" +
                                                        "NULL as auditflags,\r\n" +
                                                        "NULL as filesystemrights, \r\n" +
                                                        "isnull(own.name, master.dbo.fn_varbintohexstr(obj.ownersid)) AS ownername\r\n" +
                                                        "FROM  SQLsecure.dbo.serverosobject AS obj \r\n" +
                                                        "LEFT JOIN SQLsecure.dbo.serveroswindowsaccount own ON (obj.snapshotid = own.snapshotid AND obj.ownersid = own.sid)\r\n" +
                                                        "where obj.snapshotid = {0} and obj.ownersid = {1}\r\n" +
                                                        "UNION\r\n" +
                                                        "Select cast(longname as nvarchar(max))as longname, objecttype, accesstype, auditflags, filesystemrights, ownername from SQLsecure.dbo.vwosobjectpermission \r\n" +
                                                        "where snapshotid = {0} and sid = {1}\r\n";
        private const string QueryGroupNames =  "select distinct b.name from SQLsecure.dbo.vwoswindowsgroupmembers a " +
                                                "INNER JOIN SQLsecure.dbo.serveroswindowsaccount AS b on a.snapshotid = b.snapshotid and a.groupsid = b.sid " +
                                                "where a.snapshotid = {0} and a.sid = {1}";



        // Columns for handling the Snapshot query results
        private const string colName = @"longname";
        private const string colType = @"objecttype";
        private const string colOwner = @"ownername";
        private const string colGroupName = @"name";
        private const string colRights = @"filesystemrights";
        private const string colAuditFlags = @"auditflags";
        private const string colAccessType = @"accesstype";
        private const string colTextRights = @"textrights";
        private const string colTextAuditFlags = @"textauditflags";
        private const string colTextAccessType = @"textaccesstype";

        #endregion

        #region Helpers

        private bool LoadPermissions()
        {
            bool isOK = true;
            try
            {
                // Open connection to repository and query group members
                logX.loggerX.InfoFormat("Retrieve Snapshot OS Permissions for user: {0}", m_name);

                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    string sidString = "0x";
                    foreach (Byte sidchar in m_sid)
                    {
                        sidString += sidchar.ToString("X2");
                    }
                    string query = string.Format(QueryOsObjectPermissions, m_snapshotId, sidString);

                    //Get Permissions
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.Text;

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);

                        ds.Tables[0].Columns.Add(colTextRights);
                        ds.Tables[0].Columns.Add(colTextAuditFlags);
                        ds.Tables[0].Columns.Add(colTextAccessType);
                        DataView dv = new DataView(ds.Tables[0]);
                        dv.Sort = colName;

                        _grid.BeginUpdate();
                        _grid.SetDataBinding(dv, null);
                        _grid.EndUpdate();

                        foreach (DataRowView drv in dv)
                        {
                            string objectType;
                            string rights = string.Empty;

                            switch (drv[colType].ToString().Trim())
                            {
                                case OsObjectType.DB:
                                    objectType = DescriptionHelper.GetDescription(typeof(OsObjectType), "DB");
                                    if (drv[colRights] != DBNull.Value)
                                        rights = ((FileSystemRights)drv[colRights]).ToString(); 
                                    break;
                                case OsObjectType.Disk:
                                    objectType = DescriptionHelper.GetDescription(typeof(OsObjectType), "Disk");
                                    if (drv[colRights] != DBNull.Value)
                                        rights = ((FileSystemRights)drv[colRights]).ToString(); 
                                    break;
                                case OsObjectType.File:
                                    objectType = DescriptionHelper.GetDescription(typeof(OsObjectType), "File");
                                    if (drv[colRights] != DBNull.Value)
                                        rights = ((FileSystemRights)drv[colRights]).ToString(); 
                                    break;
                                case OsObjectType.FileDirectory:
                                    objectType = DescriptionHelper.GetDescription(typeof(OsObjectType), "FileDirectory");
                                    if (drv[colRights] != DBNull.Value)
                                        rights = ((FileSystemRights)drv[colRights]).ToString(); 
                                    break;
                                case OsObjectType.InstallDirectory:
                                    objectType = DescriptionHelper.GetDescription(typeof(OsObjectType), "InstallDirectory");
                                    if (drv[colRights] != DBNull.Value)
                                        rights = ((FileSystemRights)drv[colRights]).ToString(); 
                                    break;
                                case OsObjectType.RegistryKey:
                                    objectType = DescriptionHelper.GetDescription(typeof(OsObjectType), "RegistryKey");
                                    if (drv[colRights] != DBNull.Value)
                                        rights = ((RegistryRights)drv[colRights]).ToString(); 
                                    break;
                                default:
                                    logX.loggerX.Warn("Warning - unknown OS Object Type encountered", drv[colType]);
                                    objectType = DescriptionHelper.GetDescription(typeof(OsObjectType), "Unknown");
                                    rights = "Unknown";
                                    break;
                            }
                            drv[colType] = objectType;
                            drv[colTextRights] = rights;

                            //decode Audit flags
                            string auditFlags = string.Empty;

                            if (drv[colAuditFlags] != DBNull.Value)
                            {
                                foreach (int flag in Enum.GetValues(typeof(AuditFlags)))
                                {
                                    //skip 0 because that is the None value which is not displayed in the security settings
                                    if (flag > 0 && ((int)drv[colAuditFlags] & flag) == flag)
                                    {
                                        auditFlags += (auditFlags.Length > 0 ? ", " : string.Empty) +
                                                      DescriptionHelper.GetEnumDescription((AuditFlags)flag);
                                    }
                                }
                            }
                            drv[colTextAuditFlags] = auditFlags;

                            //decode access types
                            string access = drv[colAccessType].ToString();
                            if (drv[colAccessType] != DBNull.Value)
                            {
                                foreach (int flag in Enum.GetValues(typeof(AccessControlType)))
                                {
                                    if ((int)drv[colAccessType] == flag)
                                    {
                                        // These cannot be combined
                                        access = DescriptionHelper.GetEnumDescription((AccessControlType)flag);
                                        break;
                                    }
                                }
                            }
                            drv[colTextAccessType] = access;
                        }
                    }
                    _label_Header.Text = string.Format("OS Objects ({0} items)", _grid.Rows.Count);
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Info("ERROR - unable to load OS Permissions from the selected Snapshot.", ex);
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.OSPermissionsCaption, ex.Message);
                isOK = false;
            }
            catch (Exception ex)
            {
                isOK = false;
                logX.loggerX.Info("ERROR - unable to load OS Permissions from the selected Snapshot.", ex);
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.OSPermissionsCaption, Utility.ErrorMsgs.CantGetSnapshot);

            }
            return isOK;
        }

        private bool LoadGroups()
        {
            bool isOK = true;
            try
            {
                // Open connection to repository and query group members
                logX.loggerX.InfoFormat("Retrieve Snapshot Group listing for user: {0}", m_name);

                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    string sidString = "0x";
                    foreach (Byte sidchar in m_sid)
                    {
                        sidString += sidchar.ToString("X2");
                    }
                    string query = string.Format(QueryGroupNames, m_snapshotId, sidString);

                    //Get Permissions
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.Text;

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);

                        DataView dv = new DataView(ds.Tables[0]);
                        dv.Sort = colGroupName;

                        groupsGrid.BeginUpdate();
                        groupsGrid.SetDataBinding(dv, null);
                        groupsGrid.EndUpdate();
                    }
                    groupsHeaderLabel.Text = string.Format("Groups ({0} items)", groupsGrid.Rows.Count);
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Info("ERROR - unable to load Group Listing from the selected Snapshot.", ex);
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.OSPermissionsCaption, ex.Message);
                isOK = false;
            }
            catch (Exception ex)
            {
                isOK = false;
                logX.loggerX.Info("ERROR - unable to load Group Listing from the selected Snapshot.", ex);
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.OSPermissionsCaption, Utility.ErrorMsgs.CantGetSnapshot);

            }
            return isOK;
        }
        
        protected void showGridColumnChooser(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            Forms.Form_GridColumnChooser.Process(grid, "OS Objects");
        }

        protected void toggleGridGroupByBox(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            grid.DisplayLayout.GroupByBox.Hidden = !grid.DisplayLayout.GroupByBox.Hidden;
        }

        protected void printGrid(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            // Associate the print document with the grid & preview dialog here
            // for consistency with other forms that require it
            _ultraGridPrintDocument.Grid = grid;
            _ultraGridPrintDocument.DefaultPageSettings.Landscape = true;
            _ultraGridPrintDocument.DefaultPageSettings.Color = false;
            _ultraGridPrintDocument.FitWidthToPages = 1;
            
            if (grid == groupsGrid)
            {           
                _ultraGridPrintDocument.Header.TextLeft = string.Format("Group Members for '{0}' as of {1}", m_name, DateTime.Now.ToShortDateString());
                _ultraGridPrintDocument.DocumentName = "Group Members";
            }   
            else
            {
                _ultraGridPrintDocument.DocumentName = "OS Objects";
                _ultraGridPrintDocument.Header.TextLeft = string.Format("OS objects for '{0}' as of {1}", m_name, DateTime.Now.ToShortDateString());
            }
            _ultraPrintPreviewDialog.Document = _ultraGridPrintDocument;

            // Call ShowDialog to show the print preview dialog.
            _ultraPrintPreviewDialog.ShowDialog();
        }

        protected void saveGrid(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _ultraGridExcelExporter.Export(grid, _saveFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    Utility.MsgBox.ShowError(Utility.ErrorMsgs.ExportToExcelCaption, Utility.ErrorMsgs.FailedToExportToExcelFile, ex);
                }
            }
        }

        #endregion

        #region Methods

        public static void DisplayPermissions(int snapshotId, string name, byte[] sid)
        {
            Form_OSObjectPermissions form = new Form_OSObjectPermissions(snapshotId, name, sid);
            if (form.LoadPermissions() && form.LoadGroups())
            {
                form.ShowDialog();
            }
        }

        #endregion

        #region Events

        private void _toolStripButton_ColumnChooser_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showGridColumnChooser(_grid);

            Cursor = Cursors.Default;
        }

        private void _toolStripButton_GridGroupBy_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            toggleGridGroupByBox(_grid);

            Cursor = Cursors.Default;
        }

        private void _toolStripButton_GridPrint_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            printGrid(_grid);

            Cursor = Cursors.Default;
        }

        private void _toolStripButton_GridSave_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            saveGrid(_grid);

            Cursor = Cursors.Default;
        }

        private void groupsPrint_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            printGrid(groupsGrid);

            Cursor = Cursors.Default;
        }

        private void groupsSave_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            saveGrid(groupsGrid);

            Cursor = Cursors.Default;
        }

        private void _grid_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            e.Layout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;

            UltraGridBand band;

            band = e.Layout.Bands[0];

            band.Columns[colName].Header.Caption = @"Name";
            band.Columns[colName].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            band.Columns[colName].Width = 400;

            band.Columns[colType].Header.Caption = @"Type";
            band.Columns[colType].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;

            band.Columns[colOwner].Header.Caption = @"Owner";
            band.Columns[colOwner].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;

            band.Columns[colTextRights].Header.Caption = @"Rights";
            band.Columns[colTextRights].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            
            band.Columns[colTextAuditFlags].Header.Caption = @"Auditing";
            band.Columns[colTextAuditFlags].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            
            band.Columns[colTextAccessType].Header.Caption = @"Access Type";
            band.Columns[colTextAccessType].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;

            band.Columns[colRights].Hidden = true;
            band.Columns[colRights].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            band.Columns[colAuditFlags].Hidden = true;
            band.Columns[colAuditFlags].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            band.Columns[colAccessType].Hidden = true;
            band.Columns[colAccessType].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        }

        private void _groupsGrid_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            e.Layout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;

            UltraGridBand band;

            band = e.Layout.Bands[0];

            band.Columns[colGroupName].Header.Caption = @"Name";
            band.Columns[colGroupName].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            band.Columns[colGroupName].Width = 500;
        }
    
        #endregion
    }
}

