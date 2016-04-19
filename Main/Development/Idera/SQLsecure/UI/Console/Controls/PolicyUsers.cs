using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Idera.SQLsecure.UI.Console.Sql;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinTabControl;

using Idera.SQLsecure.Core.Accounts;
using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Controls;
using Idera.SQLsecure.UI.Console.Utility;

namespace Idera.SQLsecure.UI.Console.Controls
{
    public partial class PolicyUsers : UserControl, Interfaces.IView, Interfaces.ICommandHandler, Interfaces.IRefresh
    {
        #region IView

        void Interfaces.IView.SetContext(Interfaces.IDataContext contextIn)
        {
            m_context = (Data.PolicyUsers)contextIn;
            m_policy = m_context.Policy;
            m_serverInstance = m_context.Server;

            setMenuConfiguration();

            loadDataSource();
        }
        String Interfaces.IView.HelpTopic
        {
            get { return (m_serverInstance == null) ? Utility.Help.SecuritySummaryPolicyUsersHelpTopic : Utility.Help.SecuritySummaryServerUsersHelpTopic; }
        }
        String Interfaces.IView.ConceptTopic
        {
            get { return (m_serverInstance == null) ? Utility.Help.SecuritySummaryPolicyUsersHelpTopic : Utility.Help.SecuritySummaryServerUsersHelpTopic; }
        }
        String Interfaces.IView.Title
        {
            get { return string.Empty; }
        }

        #endregion

        #region ICommandHandler Members

        void Interfaces.ICommandHandler.ProcessCommand(Utility.ViewSpecificCommand command)
        {
            switch (command)
            {
                case Utility.ViewSpecificCommand.NewAuditServer:
                    showNewAuditServer();
                    break;
                case Utility.ViewSpecificCommand.NewLogin:
                    showNewLogin();
                    break;
                case Utility.ViewSpecificCommand.Baseline:
                    showBaseline();
                    break;
                case Utility.ViewSpecificCommand.Collect:
                    showCollect();
                    break;
                case Utility.ViewSpecificCommand.Configure:
                    showConfigure();
                    break;
                case Utility.ViewSpecificCommand.Delete:
                    showDelete();
                    break;
                case Utility.ViewSpecificCommand.Properties:
                    showProperties();
                    break;
                case Utility.ViewSpecificCommand.Refresh:
                    showRefresh();
                    break;
                case Utility.ViewSpecificCommand.UserPermissions:
                    showPermissions(Views.View_PermissionExplorer.Tab.UserPermissions);
                    break;
                case Utility.ViewSpecificCommand.ObjectPermissions:
                    showPermissions(Views.View_PermissionExplorer.Tab.ObjectPermissions);
                    break;
                default:
                    Debug.Assert(false, "Unknown command passed to PolicyUsers");
                    break;
            }
        }

        protected virtual void showNewAuditServer()
        {
            Forms.Form_WizardRegisterSQLServer.Process();
        }

        protected virtual void showNewLogin()
        {
            Forms.Form_WizardNewLogin.Process();
        }

        protected virtual void showBaseline()
        {
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - SqlServerSettings showBaseline command called erroneously");
        }

        protected virtual void showCollect()
        {
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - SqlServerSettings showCollect command called erroneously");
        }

        protected virtual void showConfigure()
        {
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - SqlServerSettings showConfigure command called erroneously");
        }

        protected virtual void showDelete()
        {
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - SqlServerSettings showDelete command called erroneously");
        }

        protected virtual void showProperties()
        {
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - SqlServerSettings showProperties command called erroneously");
        }

        protected virtual void showRefresh()
        {
            //loadDataSource();
            // refresh the entire view to keep all info in sync
            Program.gController.RefreshCurrentView();
        }

        protected virtual void showPermissions(Views.View_PermissionExplorer.Tab tabIn)
        {
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - SqlServerSettings showPermissions command called erroneously");
        }

        #endregion

        #region IRefresh Members

        public void RefreshView()
        {
            showRefresh();
        }

        #endregion

        #region ctors

        public PolicyUsers()
        {
            InitializeComponent();

            // Initialize base class fields.
            m_menuConfiguration = new Utility.MenuConfiguration();

            _ultraTabControl_Users.ImageList = AppIcons.AppImageList16();

            // hook the toolbar labels to the grids so the heading can be used for printing
            _grid_Users.Tag = _label_Users;

            // hook the grids to the toolbars so they can be used for button processing
            _headerStrip_Users.Tag = _grid_Users;

            // Hookup all application images
            _toolStripButton_ColumnChooser.Image = AppIcons.AppImage16(AppIcons.Enum.GridFieldChooser);
            _toolStripButton_GroupBy.Image = AppIcons.AppImage16(AppIcons.Enum.GridGroupBy);
            _toolStripButton_Save.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);
            _toolStripButton_Print.Image = AppIcons.AppImage16(AppIcons.Enum.Print);

            // set all grids to start in the same initial display mode
            _grid_Users.DisplayLayout.GroupByBox.Hidden = Utility.Constants.InitialState_GroupByBoxHidden;


            // load value lists for grid display
            ValueList severityValueList = new ValueList();
            severityValueList.Key = ValueListYesNo;
            severityValueList.DisplayStyle = ValueListDisplayStyle.DisplayText;
            _grid_Users.DisplayLayout.ValueLists.Add(severityValueList);

            ValueListItem listItem;

            severityValueList.ValueListItems.Clear();
            listItem = new ValueListItem("Y", "Yes");
            severityValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem("N", "No");
            severityValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(string.Empty, "N/A");
            severityValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(" ", "N/A");
            severityValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(null, "N/A");
            severityValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(DBNull.Value, "N/A");
            severityValueList.ValueListItems.Add(listItem);

            // Initialize the tabs
            _ultraTabControl_Users.SuspendLayout();
            _ultraTabControl_Users.Tabs.Clear();
            foreach (string usertype in new string[] { tabKey_Windows, tabKey_Sql, tabKey_All })
            {
                _ultraTabControl_Users.Tabs.Add(usertype, usertype);
            }
            _ultraTabControl_Users.ResumeLayout();

            // Initialize the grids
            initDataSource();

            // Hide the focus rectangles on tabs and grids
            _ultraTabControl_Users.DrawFilter = new HideFocusRectangleDrawFilter();
            _grid_Users.DrawFilter = new HideFocusRectangleDrawFilter();
        }

        #endregion

        #region fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Controls.PolicyUsers");
        private bool m_Initialized = false;

        private Utility.MenuConfiguration m_menuConfiguration;
        private Data.PolicyUsers m_context;
        private Control m_focused = null; // To prevent focus from switching to the splitters, etc.
        private bool m_gridCellClicked = false;

        private Sql.Policy m_policy;
        private Sql.RegisteredServer m_serverInstance;
        private DataTable m_UsersTable;

        #endregion

        #region query, columns and constants

        private const string QueryGetUsers =
                    @"SQLsecure.dbo.isp_sqlsecure_getpolicyserverprincipal";
        private const string ParamPolicyId = @"@policyid";
        private const string ParamBaselineOnly = @"@usebaseline";
        private const string ParamRunDate = @"@rundate";

        // Common columns
        private const string colIcon = @"Icon";
        private const string colSnapshotId = @"snapshotid";
        private const string colConnection = @"connectionname";
        private const string colPrincipalName = @"name";
        private const string colSid = @"sid";
        private const string colPrincipalType = @"type";
        private const string colPrincipalTypeName = @"typename";
        private const string colPrincipalId = @"principalid";
        private const string colServerAccess = @"serveraccess";
        private const string colServerDeny = @"serverdeny";
        private const string colDisabled = @"disabled";
        private const string colExpirationChecked = @"isexpirationchecked";
        private const string colPolicyChecked = @"ispolicychecked";
        private const string colPasswordStatus = @"passwordstatus";
        private const string colDefaultDatabase = @"defaultdatabase";
        private const string colDefaultLanguage = @"defaultlanguage";


        private const string ServerFilter = "and connectionname = '{0}'";

        private const string tabKey_Windows = @"Windows Users and Groups";
        private const string tabKey_Sql = @"SQL Logins";
        private const string tabKey_All = @"All Logins";

        private const string NumericFormat = @"N0";

        private const string HeaderUsers = @"Users";
        private const string NoRecordsValue = "No {0} found";

        private const string HeaderDisplay = "{0} ({1} items)";
        private const string PrintHeaderDisplay = "Policy {0} as of {1}\n\n{2}";
        private const string PrintEmptyHeaderDisplay = "{0}";

        private const string ValueListYesNo = @"YesNo";

        #endregion

        #region properties

        public Sql.User SelectedUser
        {
            get
            {
                if (_grid_Users.Rows.Count > 0 && _grid_Users.Selected.Rows.Count == 1)
                {
                    DataRowView row = (DataRowView) _grid_Users.Selected.Rows[0].ListObject;
                    User user = new User((string)row[colPrincipalName], new Sid((Byte[])row[colSid]), (string)row[colPrincipalType], User.UserSource.Snapshot);
                    return user;
                }

                return null;
            }
        }

        public Sql.RegisteredServer SelectedServer
        {
            get
            {
                if (m_serverInstance != null)
                {
                    return m_serverInstance;
                }
                else
                {
                    if (_grid_Users.Rows.Count > 0 && _grid_Users.Selected.Rows.Count == 1)
                    {
                        DataRowView row = (DataRowView) _grid_Users.Selected.Rows[0].ListObject;
                        Sql.RegisteredServer server = Program.gController.Repository.RegisteredServers.Find((string) row[colConnection]);
                        return server;
                    }
                }

                return null;
            }
        }

        #endregion

        #region methods

        #endregion

        #region helpers

        protected void setMenuConfiguration()
        {
            //This is all currently being handled at the view level, but leaving functions for future
            //m_menuConfiguration.EditItems[(int)Utility.MenuItems_Edit.Remove] = false;
            //m_menuConfiguration.EditItems[(int)Utility.MenuItems_Edit.Properties] = false;

            //Program.gController.SetMenuConfiguration(m_menuConfiguration, this);
        }

        private DataTable createDataSource()
        {
            // Create Users default datasource
            DataTable dt = new DataTable();
            dt.Columns.Add(colIcon, typeof(Image));
            dt.Columns.Add(colSnapshotId, typeof(int));
            dt.Columns.Add(colPrincipalName, typeof(string));
            dt.Columns.Add(colSid, typeof(byte[]));
            dt.Columns.Add(colPrincipalType, typeof(string));
            dt.Columns.Add(colPrincipalTypeName, typeof(string));
            dt.Columns.Add(colPrincipalId, typeof(int));
            dt.Columns.Add(colConnection, typeof(string));
            dt.Columns.Add(colServerAccess, typeof(string));
            dt.Columns.Add(colServerDeny, typeof(string));
            dt.Columns.Add(colDisabled, typeof(string));
            dt.Columns.Add(colExpirationChecked, typeof(string));
            dt.Columns.Add(colPolicyChecked, typeof(string));
            dt.Columns.Add(colPasswordStatus, typeof(string));
            dt.Columns.Add(colDefaultDatabase, typeof(string));
            dt.Columns.Add(colDefaultLanguage, typeof(string));
            return dt;
        }

        private void initDataSource()
        {
            // Initialize the details
            m_UsersTable = createDataSource();

            _label_Users.Text = HeaderUsers;
            _grid_Users.SetDataBinding(m_UsersTable.DefaultView, null);
        }

        private void loadDataSource()
        {
            logX.loggerX.Info("Retrieve Users");

            _ultraTabControl_Users.BeginUpdate();
            _ultraTabControl_Users.SuspendLayout();

            try
            {
                // Open connection to repository and query permissions.
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Setup parameters for all queries
                    SqlParameter paramPolicyId = new SqlParameter(ParamPolicyId, m_policy.PolicyId);
                    SqlParameter paramBaselineOnly = new SqlParameter(ParamBaselineOnly, SqlDbType.Bit, 0);
                    paramBaselineOnly.Value = m_context.UseBaseline;
                    SqlParameter paramRunDate = new SqlParameter(ParamRunDate, m_context.SelectionDate);

                    // Get Users
                    SqlCommand cmd = new SqlCommand(QueryGetUsers, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(paramPolicyId);
                    cmd.Parameters.Add(paramBaselineOnly);
                    cmd.Parameters.Add(paramRunDate);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    m_UsersTable = ds.Tables[0];


                    //Add the Icon, Checkbox columns for manual processing
                    //dt.Columns.Add(colIcon, typeof (Image));
                    //dt.Columns[colIcon].SetOrdinal(0);
                    //dt.Columns.Add(colDisabledCheckBox, typeof (bool));
                    //dt.Columns[colDisabledCheckBox].SetOrdinal(dt.Columns[colDisabled].Ordinal + 1);

                    _grid_Users.SuspendLayout();

                    filterByType(_ultraTabControl_Users.SelectedTab.Text);

                    if (!m_Initialized)
                    {
                        foreach (UltraGridColumn col in _grid_Users.DisplayLayout.Bands[0].Columns)
                        {
                            if (col.Key != colPrincipalName)    // These can be too long
                            {
                                col.PerformAutoResize(PerformAutoSizeType.AllRowsInBand, true);
                            }
                        }
                        _grid_Users.DisplayLayout.Bands[0].SortedColumns.Add(colConnection, false, false);
                    }

                    _grid_Users.ResumeLayout();

                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error(@"Error - Unable to retrieve users", ex);
                MsgBox.ShowError(string.Format(ErrorMsgs.CantGetPolicyInfoMsg, "Users"),
                                 ErrorMsgs.ErrorProcessPolicyInfo,
                                 ex);
                initDataSource();

                _grid_Users.ResumeLayout();
            }

            _ultraTabControl_Users.ResumeLayout();
            _ultraTabControl_Users.EndUpdate();
        }

        private void filterByType(string usertype)
        {
            string filter;

            if (usertype == tabKey_Windows)
            {
                filter = colPrincipalType + " in ('" + Sql.ServerPrincipalTypes.WindowsGroup+ "', '" + Sql.ServerPrincipalTypes.WindowsUser + "')";
            }
            else if (usertype == tabKey_Sql)
            {
                filter = colPrincipalType + " = '" + Sql.ServerPrincipalTypes.SqlLogin + "'";
            }
            else
            {
                filter = colPrincipalType + " not = '" + Sql.ServerPrincipalTypes.ServerRole + "'";
            }

            if (m_serverInstance != null)
            {
                filter += string.Format(ServerFilter, m_serverInstance.ConnectionName);
            } 
            
            DataView dv = new DataView(m_UsersTable);
            dv.RowFilter = filter;

            //Show the record count in the header, and stuff a record in the grid to say none returned
            _label_Users.Text = string.Format(HeaderDisplay, HeaderUsers, dv.Count.ToString());

            _grid_Users.SetDataBinding(dv, null);
        }

        #endregion

        #region Grid

        protected void printGrid(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            Debug.Assert(grid.Tag.GetType() == typeof(ToolStripLabel));

            // Associate the print document with the grid & preview dialog here
            // in case we want to change documents for each grid
            _ultraGridPrintDocument.Grid = grid;
            _ultraGridPrintDocument.DefaultPageSettings.Landscape = true;
            _ultraGridPrintDocument.DefaultPageSettings.Color = false;
            _ultraGridPrintDocument.DocumentName = ErrorMsgs.UserPermissionsCaption;
            //_ultraGridPrintDocument.FitWidthToPages = 2;
            if (m_policy != null)
            {
                _ultraGridPrintDocument.Header.TextLeft =
                    string.Format(PrintHeaderDisplay,
                                        m_policy.PolicyName,
                                        DateTime.Now,
                                        ((ToolStripLabel)grid.Tag).Text
                                    );
            }
            else
            {
                _ultraGridPrintDocument.Header.TextLeft =
                    string.Format(PrintEmptyHeaderDisplay,
                                        ((ToolStripLabel)grid.Tag).Text
                                    );
            }
            //_ultraGridPrintDocument.Footer.TextCenter =
            //    string.Format("Page {0}",
            //                    _ultraGridPrintDocument.PageNumber,
            //                    _ultraPrintPreviewDialog.pag
            _ultraPrintPreviewDialog.Document = _ultraGridPrintDocument;

            // Call ShowDialog to show the print preview dialog.
            _ultraPrintPreviewDialog.ShowDialog();
        }

        protected void saveGrid(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            //bool iconHidden = false;
            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ////save the current state of the icon column and then hide it before exporting
                    //if (grid.DisplayLayout.Bands[0].Columns.Exists(colIcon))
                    //{
                    //    // this column doesn't exist in the raw data hack
                    //    iconHidden = grid.DisplayLayout.Bands[0].Columns[colIcon].Hidden;
                    //    grid.DisplayLayout.Bands[0].Columns[colIcon].Hidden = true;
                    //    // Fix the sub list display on the effective grid
                    //    if (grid.DisplayLayout.Bands.Count > 1
                    //        && grid.DisplayLayout.Bands[1].Columns.Exists(colIcon))
                    //    {
                    //        grid.DisplayLayout.Bands[1].Columns[colIcon].Hidden = true;
                    //    }
                    //}
                    _ultraGridExcelExporter.Export(grid, _saveFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MsgBox.ShowError(ErrorMsgs.ExportToExcelCaption, ErrorMsgs.FailedToExportToExcelFile, ex);
                }
                //if (grid.DisplayLayout.Bands[0].Columns.Exists(colIcon))
                //{
                //    // this column doesn't exist in the raw data hack
                //    grid.DisplayLayout.Bands[0].Columns[colIcon].Hidden = iconHidden;
                //}
                //// Fix the sub list display on the effective grid
                //if (grid.DisplayLayout.Bands.Count > 1
                //    && grid.DisplayLayout.Bands[1].Columns.Exists(colIcon))
                //{
                //    grid.DisplayLayout.Bands[1].Columns[colIcon].Hidden = iconHidden;
                //}
            }
        }

        protected void showGridColumnChooser(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            // set any column chooser options before showing????
            string gridHeading = ((ToolStripLabel)grid.Tag).Text;
            if (gridHeading.IndexOf("(") > 0)
            {
                gridHeading = gridHeading.Remove(gridHeading.IndexOf("(") - 1);
            }

            Forms.Form_GridColumnChooser.Process(grid, gridHeading);
        }

        protected void toggleGridGroupByBox(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            grid.DisplayLayout.GroupByBox.Hidden = !grid.DisplayLayout.GroupByBox.Hidden;
        }

        #endregion

        #region Events

        #region tool strips

        private void _toolStripButton_GridGroupBy_Click(object sender, EventArgs e)
        {
            // Note: This event handler is used by all grid toolbars
            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            Cursor = Cursors.WaitCursor;

            toggleGridGroupByBox(grid);

            Cursor = Cursors.Default;
        }

        private void _toolStripButton_GridPrint_Click(object sender, EventArgs e)
        {
            // Note: This event handler is used by all grid toolbars
            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            Cursor = Cursors.WaitCursor;

            printGrid(grid);

            Cursor = Cursors.Default;
        }

        private void _toolStripButton_GridSave_Click(object sender, EventArgs e)
        {
            // Note: This event handler is used by all grid toolbars
            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            Cursor = Cursors.WaitCursor;

            saveGrid(grid);

            Cursor = Cursors.Default;
        }

        private void _toolStripButton_ColumnChooser_Click(object sender, EventArgs e)
        {
            // Note: This event handler is used by all grid toolbars
            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            Cursor = Cursors.WaitCursor;

            showGridColumnChooser(grid);

            Cursor = Cursors.Default;
        }

        #endregion

        #region Tabs

        private void _ultraTabControl_Settings_SelectedTabChanged(object sender, SelectedTabChangedEventArgs e)
        {
            _grid_Users.SuspendLayout();

            filterByType(e.Tab.Text);

            _grid_Users.ResumeLayout();
        }

        #endregion

        #region Context Menu events

        private void _contextMenuStrip_Users_Opening(object sender, CancelEventArgs e)
        {
            UltraGrid grid = (UltraGrid)((ContextMenuStrip)sender).SourceControl;
            _cmsi_Users_viewGroupMembers.Visible =
                _cmsi_Users_showPermissions.Visible =
                _toolStripSeparator_Server.Visible = true;
            if (grid.ActiveRow != null)
            {
                _cmsi_Users_viewGroupMembers.Enabled =
                    (grid.ActiveRow.Cells[colPrincipalType].Text == DescriptionHelper.GetEnumDescription(Sql.ServerPrincipalTypes.WindowsGroup));

                _cmsi_Users_showPermissions.Enabled =
                    (grid.ActiveRow.Cells[colSid].Value != null);
            }
            else
            {
                _cmsi_Users_viewGroupMembers.Enabled =
                    _cmsi_Users_showPermissions.Enabled = false;
            }

            _cmsi_Users_viewGroupByBox.Checked = !grid.DisplayLayout.GroupByBox.Hidden;
        }

        private void _cmsi_Users_viewGroupMembers_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            UltraGrid grid = (UltraGrid)((ContextMenuStrip)((ToolStripItem)sender).Owner).SourceControl;

            Sql.User group = new Sql.User(grid.ActiveRow.Cells[colPrincipalName].Text,
                                            new Sid(grid.ActiveRow.Cells[colSid].Value as byte[]),
                                            DescriptionHelper.GetEnumDescription(Sql.ServerPrincipalTypes.WindowsGroup),
                                            Sql.User.UserSource.Snapshot);

            int snapshotid = (int)grid.ActiveRow.Cells[colSnapshotId].Value;
            Sql.User user = Forms.Form_SelectGroupMember.GetUser(snapshotid, group);

            if (user != null)
            {
                string server = grid.ActiveRow.Cells[colConnection].Text;
                Sql.RegisteredServer registeredServer = Program.gController.Repository.RegisteredServers.Find(server);

                Program.gController.ShowRootView(new Utility.NodeTag(new Data.PermissionExplorer(registeredServer, snapshotid, user, Views.View_PermissionExplorer.Tab.UserPermissions),
                                                                Utility.View.PermissionExplorer));
            }

            Cursor = Cursors.Default;
        }

        private void _cmsi_Users_showPermissions_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            UltraGrid grid = (UltraGrid)((ContextMenuStrip)((ToolStripItem)sender).Owner).SourceControl;

            string server = grid.ActiveRow.Cells[colConnection].Text;
            Sql.RegisteredServer registeredServer = Program.gController.Repository.RegisteredServers.Find(server);

            int snapshotid = (int)grid.ActiveRow.Cells[colSnapshotId].Value;

            Sql.User user = new Sql.User(grid.ActiveRow.Cells[colPrincipalName].Text,
                                    new Sid(grid.ActiveRow.Cells[colSid].Value as byte[]),
                                    grid.ActiveRow.Cells[colPrincipalType].Text,
                                    Sql.User.UserSource.Snapshot);

            Program.gController.ShowRootView(new Utility.NodeTag(new Data.PermissionExplorer(registeredServer, snapshotid, user, Views.View_PermissionExplorer.Tab.UserPermissions),
                                                            Utility.View.PermissionExplorer));

            Cursor = Cursors.Default;
        }

        private void _cmsi_grid_ColumnChooser_Click(object sender, EventArgs e)
        {
            // Note: This event handler is used by all grid context menus
            Cursor = Cursors.WaitCursor;

            UltraGrid grid = (UltraGrid)((ContextMenuStrip)((ToolStripItem)sender).Owner).SourceControl;

            showGridColumnChooser(grid);

            Cursor = Cursors.Default;
        }

        private void _cmsi_grid_viewGroupByBox_Click(object sender, EventArgs e)
        {
            // Note: This event handler is used by all grid context menus
            Cursor = Cursors.WaitCursor;

            UltraGrid grid = (UltraGrid)((ContextMenuStrip)((ToolStripItem)sender).Owner).SourceControl;

            toggleGridGroupByBox(grid);

            Cursor = Cursors.Default;
        }

        private void _cmsi_grid_Print_Click(object sender, EventArgs e)
        {
            // Note: This event handler is used by all grid context menus
            Cursor = Cursors.WaitCursor;

            UltraGrid grid = (UltraGrid)((ContextMenuStrip)((ToolStripItem)sender).Owner).SourceControl;

            printGrid(grid);

            Cursor = Cursors.Default;
        }

        private void _cmsi_grid_Save_Click(object sender, EventArgs e)
        {
            // Note: This event handler is used by all grid context menus
            Cursor = Cursors.WaitCursor;

            UltraGrid grid = (UltraGrid)((ContextMenuStrip)((ToolStripItem)sender).Owner).SourceControl;

            saveGrid(grid);

            Cursor = Cursors.Default;
        }

        #endregion

        #region Grids

        private void _grid_Users_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            e.Layout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Override.FixedHeaderIndicator = FixedHeaderIndicator.None;
            e.Layout.ColumnChooserEnabled = DefaultableBoolean.True;
            e.Layout.UseFixedHeaders = true;

            UltraGridBand band = e.Layout.Bands[0];

            //band.Columns[colIcon].Header.Caption = colIconHdr;
            //band.Columns[colIcon].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            //band.Columns[colIcon].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            //band.Columns[colIcon].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            //band.Columns[colIcon].Width = 22;

            band.Columns[colSnapshotId].Hidden = true;
            band.Columns[colSnapshotId].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            band.Columns[colPrincipalName].Header.Caption = "Login Name";
            band.Columns[colPrincipalName].Header.Fixed = true;
            band.Columns[colPrincipalName].Width = 240;

            band.Columns[colSid].Hidden = true;
            band.Columns[colSid].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            band.Columns[colPrincipalType].Hidden = true;
            band.Columns[colPrincipalType].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            band.Columns[colPrincipalTypeName].Header.Caption = "Type";

            band.Columns[colPrincipalId].Hidden = true;
            band.Columns[colPrincipalId].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            band.Columns[colConnection].Header.Caption = "SQL Server";
            band.Columns[colConnection].Header.Fixed = true;
            band.Columns[colConnection].Header.SetVisiblePosition(1, false);
            band.Columns[colConnection].Hidden = (m_serverInstance != null);

            band.Columns[colServerAccess].Header.Caption = "Server Access";
            band.Columns[colServerAccess].ValueList = e.Layout.ValueLists[ValueListYesNo];

            band.Columns[colServerDeny].Header.Caption = "Server Deny";
            band.Columns[colServerDeny].ValueList = e.Layout.ValueLists[ValueListYesNo];

            band.Columns[colDisabled].Header.Caption = "Disabled";
            band.Columns[colDisabled].ValueList = e.Layout.ValueLists[ValueListYesNo];

            band.Columns[colExpirationChecked].Header.Caption = "Expiration Checked";
            band.Columns[colExpirationChecked].ValueList = e.Layout.ValueLists[ValueListYesNo];

            band.Columns[colPolicyChecked].Header.Caption = "Policy Checked";
            band.Columns[colPolicyChecked].ValueList = e.Layout.ValueLists[ValueListYesNo];

            band.Columns[colPasswordStatus].Header.Caption = "Password Health";

            band.Columns[colDefaultDatabase].Header.Caption = "Default Database";

            band.Columns[colDefaultLanguage].Header.Caption = "Default Language";
        }

        private void _grid_Users_InitializeGroupByRow(object sender, InitializeGroupByRowEventArgs e)
        {
            UltraGrid grid = (UltraGrid)sender;
            if (grid.DisplayLayout.Bands[0].SortedColumns.Count == 2
                && grid.DisplayLayout.Bands[0].SortedColumns[0].Key == colPrincipalName
                && grid.DisplayLayout.Bands[0].SortedColumns[0].IsGroupByColumn
                && grid.DisplayLayout.Bands[0].SortedColumns[1].Key == colConnection
                && !grid.DisplayLayout.Bands[0].SortedColumns[1].IsGroupByColumn)
            {
                if (e.Row.Column.Equals(grid.DisplayLayout.Bands[0].Columns[colPrincipalName])
                    && !String.IsNullOrEmpty(e.Row.Value.ToString()))
                {
                    e.Row.ExpansionIndicator = ShowExpansionIndicator.Never;
                    e.Row.Description = e.Row.Value.ToString()
                                        + " (" + e.Row.Rows.Count.ToString() + " server" +
                                        (e.Row.Rows.Count == 1 ? string.Empty : "s") + ")";
                }

                e.Row.Appearance.BackColor = Color.White;
                e.Row.Appearance.BackColor2 = Color.White;
            }
        }

        private void _grid_MouseDown(object sender, MouseEventArgs e)
        {
            // Note: this event handler is used for the MouseDown event on all grids
            UltraGrid grid = (UltraGrid)sender;

            Infragistics.Win.UIElement elementMain;
            Infragistics.Win.UIElement elementUnderMouse;

            elementMain = grid.DisplayLayout.UIElement;

            elementUnderMouse = elementMain.ElementFromPoint(new Point(e.X, e.Y));
            if (elementUnderMouse != null)
            {
                Infragistics.Win.UltraWinGrid.UltraGridCell cell = elementUnderMouse.GetContext(typeof(Infragistics.Win.UltraWinGrid.UltraGridCell)) as Infragistics.Win.UltraWinGrid.UltraGridCell;
                if (cell != null)
                {
                    m_gridCellClicked = true;
                    grid.Selected.Rows.Clear();
                    cell.Row.Selected = true;
                    grid.ActiveRow = cell.Row;
                }
                else
                {
                    m_gridCellClicked = false;
                    Infragistics.Win.UltraWinGrid.HeaderUIElement he = elementUnderMouse.GetAncestor(typeof(Infragistics.Win.UltraWinGrid.HeaderUIElement)) as Infragistics.Win.UltraWinGrid.HeaderUIElement;
                    Infragistics.Win.UltraWinGrid.ColScrollbarUIElement ce = elementUnderMouse.GetAncestor(typeof(Infragistics.Win.UltraWinGrid.ColScrollbarUIElement)) as Infragistics.Win.UltraWinGrid.ColScrollbarUIElement;
                    Infragistics.Win.UltraWinGrid.RowScrollbarUIElement re = elementUnderMouse.GetAncestor(typeof(Infragistics.Win.UltraWinGrid.RowScrollbarUIElement)) as Infragistics.Win.UltraWinGrid.RowScrollbarUIElement;
                    if (he == null && ce == null && re == null)
                    {
                        grid.Selected.Rows.Clear();
                        grid.ActiveRow = null;
                    }
                }
            }
        }

        #endregion

        #endregion
    }
}
