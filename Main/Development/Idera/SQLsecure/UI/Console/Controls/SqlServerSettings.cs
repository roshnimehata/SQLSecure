using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinTabControl;

using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Controls;
using Idera.SQLsecure.UI.Console.Utility;

namespace Idera.SQLsecure.UI.Console.Controls
{
    public partial class SqlServerSettings : UserControl, Interfaces.IView, Interfaces.ICommandHandler, Interfaces.IRefresh
    {
        #region types & enums

        private enum DisplayType
        {
            ByServer,
            BySetting
        }

        #endregion

        #region IView

        void Interfaces.IView.SetContext(Interfaces.IDataContext contextIn)
        {
            m_context = (Data.SqlServerSettings)contextIn;
            m_policy = m_context.Policy;
            m_serverInstance = m_context.Server;
            setDisplayType();

            setMenuConfiguration();

            loadDataSource();
        }
        String Interfaces.IView.HelpTopic
        {
            get { return (m_serverInstance == null) ? Utility.Help.SecuritySummaryPolicySettingsHelpTopic : Utility.Help.SecuritySummaryServerSettingsHelpTopic; }
        }
        String Interfaces.IView.ConceptTopic
        {
            get { return (m_serverInstance == null) ? Utility.Help.SecuritySummaryPolicySettingsHelpTopic : Utility.Help.SecuritySummaryServerSettingsHelpTopic; }
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
                    Debug.Assert(false, "Unknown command passed to SqlServerSettings");
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
            Debug.Assert((_grid_Settings.ActiveRow != null), "No selected server row in grid");

            if (_grid_Settings.ActiveRow != null)
            {
                string server = _grid_Settings.ActiveRow.Cells[colConnection].Text;

                try
                {
                    Forms.Form_StartSnapshotJobAndShowProgress.Process(server);

                    //this._grid_Settings.BeginUpdate();
                    //_grid_Settings.ActiveRow.Cells[colIcon].Value = AppIcons.AppImage16(AppIcons.Enum.ServerInProgress);
                    //_grid_Settings.ActiveRow.Cells[colCollectionStatus].Value = @"Running";
                    //this._grid_Settings.EndUpdate();
                }
                catch (Exception ex)
                {
                    Utility.MsgBox.ShowError(Utility.ErrorMsgs.SQLsecureDataCollection, Utility.ErrorMsgs.CantRunDataCollection, ex);
                }
            }
        }

        protected virtual void showConfigure()
        {
            Debug.Assert(!(_grid_Settings.ActiveRow == null), "No selected server row in grid");

            if (_grid_Settings.ActiveRow != null)
            {
                string server = _grid_Settings.ActiveRow.Cells[colConnection].Text;

                Forms.Form_SqlServerProperties.Process(server, Forms.Form_SqlServerProperties.RequestedOperation.EditCofiguration, Program.gController.isAdmin);
            }
        }

        protected virtual void showDelete()
        {
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - SqlServerSettings showDelete command called erroneously");
        }

        protected virtual void showProperties()
        {
            Debug.Assert(!(_grid_Settings.ActiveRow == null), "No selected server row in grid");

            if (_grid_Settings.ActiveRow != null)
            {
                string server = _grid_Settings.ActiveRow.Cells[colConnection].Text;

                Forms.Form_SqlServerProperties.Process(server, Forms.Form_SqlServerProperties.RequestedOperation.GeneralProperties, Program.gController.isAdmin);
            }
        }

        protected virtual void showRefresh()
        {
            //loadDataSource();
            // refresh the entire view to keep all info in sync
            Program.gController.RefreshCurrentView();
        }

        protected virtual void showPermissions(Views.View_PermissionExplorer.Tab tabIn)
        {
            if (m_gridDisplay == DisplayType.ByServer)
            {
                Debug.Assert(!(_grid_Settings.ActiveRow == null), "No selected server row in grid");

                if (_grid_Settings.ActiveRow != null)
                {
                    string server = _grid_Settings.ActiveRow.Cells[colConnection].Text;

                    Sql.RegisteredServer registeredServer =
                        Program.gController.Repository.RegisteredServers.Find(server);

                    Program.gController.ShowRootView(
                        new Utility.NodeTag(new Data.PermissionExplorer(registeredServer, tabIn),
                                            Utility.View.PermissionExplorer));
                }
            }
        }

        #endregion

        #region IRefresh Members

        public void RefreshView()
        {
            showRefresh();
        }

        #endregion

        #region ctors

        public SqlServerSettings()
        {
            InitializeComponent();

            // Initialize base class fields.
            m_menuConfiguration = new Utility.MenuConfiguration();

            _ultraTabControl_Settings.ImageList = AppIcons.AppImageList16();

            // Initialize the correct grid display
            _grid_Settings.Dock = DockStyle.Fill;
            _grid_Settings.Visible = true;
            _grid_PivotSettings.Dock = DockStyle.Fill;
            _grid_PivotSettings.Visible = true;

            // hook the toolbar labels to the grids so the heading can be used for printing
            _grid_Settings.Tag = _label_Settings;
            _grid_PivotSettings.Tag = _label_Settings;

            // hook the grids to the toolbars so they can be used for button processing
            _headerStrip_Settings.Tag = _grid_Settings;

            // Hookup all application images
            _toolStripButton_ColumnChooser.Image = AppIcons.AppImage16(AppIcons.Enum.GridFieldChooser);
            _toolStripButton_GroupBy.Image = AppIcons.AppImage16(AppIcons.Enum.GridGroupBy);
            _toolStripButton_Save.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);
            _toolStripButton_Print.Image = AppIcons.AppImage16(AppIcons.Enum.Print);
            _toolStripButton_ByServer.Image = AppIcons.AppImage16(AppIcons.Enum.ServerOK);
            _toolStripButton_BySetting.Image = AppIcons.AppImage16(AppIcons.Enum.ConfigureAuditSettingsSM);

            // set all grids to start in the same initial display mode
            _grid_Settings.DisplayLayout.GroupByBox.Hidden = Utility.Constants.InitialState_GroupByBoxHidden;
            _grid_PivotSettings.DisplayLayout.GroupByBox.Hidden = Utility.Constants.InitialState_GroupByBoxHidden;

            // Initialize the grids
            initDataSource();

            // Hide the focus rectangles on tabs and grids
            _ultraTabControl_Settings.DrawFilter = new HideFocusRectangleDrawFilter();
            _grid_Settings.DrawFilter = new HideFocusRectangleDrawFilter();
            _grid_PivotSettings.DrawFilter = new HideFocusRectangleDrawFilter();

            setDisplayType();
        }

        #endregion

        #region fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Controls.SqlServerSettings");
        private bool m_Initialized = false;

        private Utility.MenuConfiguration m_menuConfiguration;
        private Data.SqlServerSettings m_context;
        private bool m_gridCellClicked = false;
        private DisplayType m_gridDisplayPolicy = DisplayType.ByServer;
        private DisplayType m_gridDisplayServer = DisplayType.BySetting;
        private DisplayType m_gridDisplay
        {
            get
            {
                return (m_serverInstance == null ? m_gridDisplayPolicy : m_gridDisplayServer);
            }
            set
            {
                if (m_serverInstance == null)
                {
                    m_gridDisplayPolicy = value;
                }
                else
                {
                    m_gridDisplayServer = value;
                }
            }
        }

        private Sql.Policy m_policy;
        private Sql.RegisteredServer m_serverInstance;
        private DataTable m_SettingsTable;
        private DataTable m_SettingsPivotTable;
        private SortedList<int, string> m_versions = new SortedList<int, string>();

        #endregion

        #region query, columns and constants

        private const string QueryGetSettings =
                    @"SQLsecure.dbo.isp_sqlsecure_getpolicyserver";
        private const string ParamPolicyId = @"@policyid";
        private const string ParamBaselineOnly = @"@usebaseline";
        private const string ParamRunDate = @"@rundate";

        // Settings columns
        private const string colIcon = @"Icon";
        private const string colConnection = @"connectionname";
        private const string colSnapshotId = @"snapshotid";
        private const string colRegisteredServerId = @"registeredserverid";
        private const string colServer = @"servername";
        private const string colInstance = @"instancename";
        private const string colAuthMode = @"authenticationmode";
        private const string colOs = @"os";
        private const string colVersion = @"version";
        private const string colEdition = @"edition";
        private const string colStatus = @"status";
        private const string colStartTime = @"starttime";
        private const string colEndTime = @"endtime";
        private const string colAutomated = @"automated";
        private const string colNumObject = @"numobject";
        private const string colNumPermission = @"numpermission";
        private const string colNumLogin = @"numlogin";
        private const string colNumWindowsGroupMember = @"numwindowsgroupmember";
        private const string colBaseline = @"baseline";
        private const string colBaselineComment = @"baselinecomment";
        private const string colSnapshotComment = @"snapshotcomment";
        private const string colLoginAuditMode = @"loginauditmode";
        private const string colEnableProxy = @"enableproxyaccount";
        private const string colC2 = @"enablec2audittrace";
        private const string colCrossDb = @"crossdbownershipchaining";
        private const string colCaseSensitive = @"casesensitivemode";
        private const string colHashKey = @"hashkey";
        private const string colCollectorVersion = @"collectorversion";
        private const string colAllowSystemTableUpdates = @"allowsystemtableupdates";
        private const string colRemoteAdminConnectionsEnabled = @"remoteadminconnectionsenabled";
        private const string colRemoteAccessEnabled = @"remoteaccessenabled";
        private const string colScanForStartupProcsEnabled = @"scanforstartupprocsenabled";
        private const string colSqlMailXpsEnabled = @"sqlmailxpsenabled";
        private const string colDatabaseMailXpsEnabled = @"databasemailxpsenabled";
        private const string colOleAutomationProceduresEnabled = @"oleautomationproceduresenabled";
        private const string colWebAssistantProceduresEnabled = @"webassistantproceduresenabled";
        private const string colXp_cmdshellEnabled = @"xp_cmdshellenabled";
        private const string colAgentMailProfile = @"agentmailprofile";
        private const string colHideInstance = @"hideinstance";
        private const string colAgentSysadminOnly = @"agentsysadminonly";
        private const string colDomainController = @"serverisdomaincontroller";
        private const string colReplicationEnabled = @"replicationenabled";
        private const string colSaPasswordEmpty = @"sapasswordempty";

        private const string colMajorVersion = @"majorversion";

        //Pivot table columns
        private const string colSetting = @"Setting";


        private const string ServerFilter = "connectionname = '{0}'";

        private const string NumericFormat = @"N0";

        private const string HeaderSettings = @"SQL Servers";
        private const string NoRecordsValue = "No {0} found";

        private const string HeaderDisplay = "{0} ({1} items)";
        private const string PrintHeaderDisplay = "Policy {0} as of {1}\n\n{2}";
        private const string PrintEmptyHeaderDisplay = "{0}";

        #endregion

        #region properties

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
                    if (m_gridDisplay == DisplayType.ByServer
                        && _grid_Settings.Rows.Count > 0
                        && _grid_Settings.Selected.Rows.Count == 1)
                    {
                        DataRowView row = (DataRowView)_grid_Settings.Selected.Rows[0].ListObject;
                        Sql.RegisteredServer server = Program.gController.Repository.RegisteredServers.Find((string)row[colConnection]);
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

        protected void setDisplayType()
        {
            SuspendLayout();

            if (m_gridDisplay == DisplayType.ByServer)
            {
                _grid_Settings.Visible = true;
                _headerStrip_Settings.Tag = _grid_Settings;
                _toolStripButton_GroupBy.Enabled = true;
                _grid_PivotSettings.Visible = false;

                _toolStripButton_ByServer.Visible = false;
                _toolStripButton_BySetting.Visible = true;
            }
            else
            {
                _grid_PivotSettings.Visible = true;
                _headerStrip_Settings.Tag = _grid_PivotSettings;
                _toolStripButton_GroupBy.Enabled = false;
                _grid_Settings.Visible = false;

                _toolStripButton_ByServer.Visible = true;
                _toolStripButton_BySetting.Visible = false;
            }

            ResumeLayout();
        }

        private DataTable createDataSource()
        {
            // Create Settings default datasource
            DataTable dt = new DataTable();
            dt.Columns.Add(colIcon, typeof(Image));
            dt.Columns.Add(colSnapshotId, typeof(int));
            dt.Columns.Add(colConnection, typeof(string));
            dt.Columns.Add(colServer, typeof(string));
            dt.Columns.Add(colInstance, typeof(string));
            dt.Columns.Add(colAuthMode, typeof(string));
            dt.Columns.Add(colOs, typeof(string));
            dt.Columns.Add(colVersion, typeof(string));
            dt.Columns.Add(colEdition, typeof(string));
            dt.Columns.Add(colStatus, typeof(string));
            dt.Columns.Add(colStartTime, typeof(DateTime));
            dt.Columns.Add(colEndTime, typeof(DateTime));
            dt.Columns.Add(colAutomated, typeof(string));
            dt.Columns.Add(colNumObject, typeof(long));
            dt.Columns.Add(colNumPermission, typeof(long));
            dt.Columns.Add(colNumLogin, typeof(long));
            dt.Columns.Add(colNumWindowsGroupMember, typeof(long));
            dt.Columns.Add(colBaseline, typeof(string));
            dt.Columns.Add(colBaselineComment, typeof(string));
            dt.Columns.Add(colSnapshotComment, typeof(string));
            dt.Columns.Add(colLoginAuditMode, typeof(string));
            dt.Columns.Add(colEnableProxy, typeof(string));
            dt.Columns.Add(colC2, typeof(string));
            dt.Columns.Add(colCrossDb, typeof(string));
            dt.Columns.Add(colCaseSensitive, typeof(string));
            dt.Columns.Add(colHashKey, typeof(string));
            dt.Columns.Add(colRegisteredServerId, typeof(int));
            dt.Columns.Add(colCollectorVersion, typeof(string));
            dt.Columns.Add(colAllowSystemTableUpdates, typeof(string));
            dt.Columns.Add(colRemoteAdminConnectionsEnabled, typeof(string));
            dt.Columns.Add(colRemoteAccessEnabled, typeof(string));
            dt.Columns.Add(colScanForStartupProcsEnabled, typeof(string));
            dt.Columns.Add(colSqlMailXpsEnabled, typeof(string));
            dt.Columns.Add(colDatabaseMailXpsEnabled, typeof(string));
            dt.Columns.Add(colOleAutomationProceduresEnabled, typeof(string));
            dt.Columns.Add(colWebAssistantProceduresEnabled, typeof(string));
            dt.Columns.Add(colXp_cmdshellEnabled, typeof(string));
            dt.Columns.Add(colAgentMailProfile, typeof(string));
            dt.Columns.Add(colHideInstance, typeof(string));
            dt.Columns.Add(colAgentSysadminOnly, typeof(string));
            dt.Columns.Add(colDomainController, typeof(string));
            dt.Columns.Add(colReplicationEnabled, typeof(string));
            dt.Columns.Add(colSaPasswordEmpty, typeof(string));
            dt.Columns.Add(colMajorVersion, typeof(string));

            return dt;
        }

        private void initDataSource()
        {
            // Initialize the details
            m_SettingsTable = createDataSource();

            _label_Settings.Text = HeaderSettings;
            _grid_Settings.SetDataBinding(m_SettingsTable.DefaultView, null);
        }

        private void loadDataSource()
        {
            logX.loggerX.Info("Retrieve SQL Server Settings");

            _ultraTabControl_Settings.BeginUpdate();
            _ultraTabControl_Settings.SuspendLayout();
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

                    // Get Settings
                    SqlCommand cmd = new SqlCommand(QueryGetSettings, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(paramPolicyId);
                    cmd.Parameters.Add(paramBaselineOnly);
                    cmd.Parameters.Add(paramRunDate);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    m_SettingsTable = ds.Tables[0];

                    // Add a column for the major version name and put the values in
                    // Fix other values for display in the datatable so they will pivot correctly
                    m_SettingsTable.Columns.Add(colMajorVersion, typeof(string));
                    m_versions.Clear();
                    foreach (DataRow row in m_SettingsTable.Rows)
                    {
                        string displayValue = row[colVersion].ToString();
                        Sql.ServerVersion version = Sql.SqlHelper.ParseVersion(displayValue);
                        switch (version)
                        {
                            case Sql.ServerVersion.SQL2000:
                                displayValue = Sql.VersionName.SQL2000;
                                break;
                            case Sql.ServerVersion.SQL2005:
                                displayValue = Sql.VersionName.SQL2005;
                                break;
                            case Sql.ServerVersion.SQL2008:
                                displayValue = Sql.VersionName.SQL2008;
                                break;
                            case Sql.ServerVersion.SQL2008R2:
                                displayValue = Sql.VersionName.SQL2008R2;
                                break;
                            case Sql.ServerVersion.SQL2012:
                                displayValue = Sql.VersionName.SQL2012;
                                break;
                            default:
                                displayValue = Sql.VersionName.Unsupported;
                                break;
                        }
                        row[colMajorVersion] = displayValue;
                        if (!m_versions.ContainsKey((int)version))
                        {
                            m_versions.Add((int)version, displayValue);
                        }

                        row[colStartTime] = ((DateTime) row[colStartTime]).ToLocalTime();
                        row[colAuthMode] = Sql.RegisteredServer.AuthenticationModeStr((string)row[colAuthMode]);
                        row[colAutomated] = Sql.RegisteredServer.YesNoStr((string)row[colAutomated]);
                        row[colLoginAuditMode] = Sql.RegisteredServer.LoginAuditModeStr((string)row[colLoginAuditMode]);
                        row[colEnableProxy] = Sql.RegisteredServer.YesNoStr((string)row[colEnableProxy]);
                        row[colC2] = Sql.RegisteredServer.YesNoStr((string)row[colC2]);
                        row[colCrossDb] = Sql.RegisteredServer.YesNoStr((string)row[colCrossDb]);
                        row[colCaseSensitive] = Sql.RegisteredServer.YesNoStr((string)row[colCaseSensitive]);
                        row[colAllowSystemTableUpdates] = Sql.RegisteredServer.YesNoStr((string)row[colAllowSystemTableUpdates]);
                        row[colRemoteAdminConnectionsEnabled] = Sql.RegisteredServer.YesNoStr((string)row[colRemoteAdminConnectionsEnabled]);
                        row[colRemoteAccessEnabled] = Sql.RegisteredServer.YesNoStr((string)row[colRemoteAccessEnabled]);
                        row[colScanForStartupProcsEnabled] = Sql.RegisteredServer.YesNoStr((string)row[colScanForStartupProcsEnabled]);
                        row[colSqlMailXpsEnabled] = Sql.RegisteredServer.YesNoStr((string)row[colSqlMailXpsEnabled]);
                        row[colDatabaseMailXpsEnabled] = Sql.RegisteredServer.YesNoStr((string)row[colDatabaseMailXpsEnabled]);
                        row[colOleAutomationProceduresEnabled] = Sql.RegisteredServer.YesNoStr((string)row[colOleAutomationProceduresEnabled]);
                        row[colWebAssistantProceduresEnabled] = Sql.RegisteredServer.YesNoStr((string)row[colWebAssistantProceduresEnabled]);
                        row[colXp_cmdshellEnabled] = Sql.RegisteredServer.YesNoStr((string)row[colXp_cmdshellEnabled]);
                        row[colHideInstance] = Sql.RegisteredServer.YesNoStr((string)row[colHideInstance]);
                        row[colAgentSysadminOnly] = Sql.RegisteredServer.YesNoStr((string)row[colAgentSysadminOnly]);
                        row[colDomainController] = Sql.RegisteredServer.YesNoStr((string)row[colDomainController]);
                        row[colSaPasswordEmpty] = Sql.RegisteredServer.YesNoStr((string)row[colSaPasswordEmpty]);
                        row[colReplicationEnabled] = Sql.RegisteredServer.YesNoStr((string)row[colReplicationEnabled]);
                    }

                    //Update the tabs to determine the filter before building the dataview
                    string defaultversion = @"All";
                    string selectedTab = _ultraTabControl_Settings.SelectedTab == null
                                             ? defaultversion
                                             : _ultraTabControl_Settings.SelectedTab.Key;
                    _ultraTabControl_Settings.Tabs.Clear();
                    _ultraTabControl_Settings.Tabs.Add(defaultversion, defaultversion);
                    if (m_serverInstance == null)
                    {
                        foreach (string tabver in m_versions.Values)
                        {
                            _ultraTabControl_Settings.Tabs.Add(tabver, tabver);
                            if (tabver == selectedTab)
                            {
                                _ultraTabControl_Settings.SelectedTab = _ultraTabControl_Settings.Tabs[selectedTab];
                            }
                        }
                    }

                    _grid_Settings.SuspendLayout();

                    filterByVersion(_ultraTabControl_Settings.SelectedTab.Text);

                    if (!m_Initialized)
                    {
                        foreach (UltraGridColumn col in _grid_Settings.DisplayLayout.Bands[0].Columns)
                        {
                            col.PerformAutoResize(PerformAutoSizeType.AllRowsInBand, true);
                        }
                        _grid_Settings.DisplayLayout.Bands[0].SortedColumns.Add(colConnection, false, false);
                    }

                    _grid_Settings.ResumeLayout();

                    _grid_PivotSettings.SuspendLayout();

                    //if (!m_Initialized)
                    {
                        foreach (UltraGridColumn col in _grid_PivotSettings.DisplayLayout.Bands[0].Columns)
                        {
                            if (col.Key == colSetting)
                            {
                                col.PerformAutoResize(PerformAutoSizeType.AllRowsInBand, true);
                            }
                            else
                            {
                                col.Width = 200;
                            }
                            col.AllowGroupBy = DefaultableBoolean.False;
                        }
                    }

                    _grid_PivotSettings.ResumeLayout();

                    m_Initialized = true;
                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error(@"Error - Unable to retrieve SQL Server settings", ex);
                MsgBox.ShowError(string.Format(ErrorMsgs.CantGetPolicyInfoMsg, "SQL Server settings"),
                                 ErrorMsgs.ErrorProcessPolicyInfo,
                                 ex);
                initDataSource();

                _grid_Settings.ResumeLayout();
                _grid_PivotSettings.ResumeLayout();
            }

            _ultraTabControl_Settings.EndUpdate();
            _ultraTabControl_Settings.ResumeLayout();
        }

        private void filterByVersion(string version)
        {
            string filter = string.Empty;

            if (version != @"All")
            {
                filter = string.Format("{0} = '{1}'",
                                        colMajorVersion,
                                        version);
            }
            else if (m_serverInstance != null)
            {
                filter = string.Format(ServerFilter, m_serverInstance.ConnectionName);
            }

            DataView dv = new DataView(m_SettingsTable);
            dv.RowFilter = filter;

            _grid_Settings.SetDataBinding(dv, null);

            _label_Settings.Text = string.Format(HeaderDisplay, HeaderSettings, dv.Count);

            m_SettingsPivotTable = new DataTable();
            m_SettingsPivotTable.Columns.Add(colSetting, typeof(string));

            foreach (DataRowView row in dv)
            {
                string server = row[colConnection].ToString();
                m_SettingsPivotTable.Columns.Add(server, typeof(string));
            }

            foreach (UltraGridColumn col in _grid_Settings.DisplayLayout.Bands[0].Columns)
            {
                if (col.Hidden == false)
                {
                    DataRow pivotRow = m_SettingsPivotTable.NewRow();
                    pivotRow[colSetting] = col.Header.Caption;
                    foreach (DataRowView row in dv)
                    {
                        pivotRow[row[colConnection].ToString()] = row[col.Key];
                    }
                    m_SettingsPivotTable.Rows.Add(pivotRow);
                }
            }

            _grid_PivotSettings.SetDataBinding(m_SettingsPivotTable, null);
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

        private void _toolStripButton_ByServer_Click(object sender, EventArgs e)
        {
            m_gridDisplay = DisplayType.ByServer;
            setDisplayType();
        }

        private void _toolStripButton_BySetting_Click(object sender, EventArgs e)
        {
            m_gridDisplay = DisplayType.BySetting;
            setDisplayType();
        }

        #endregion

        #region Tabs

        private void _ultraTabControl_Settings_SelectedTabChanged(object sender, SelectedTabChangedEventArgs e)
        {
            _grid_Settings.SuspendLayout();
            _grid_PivotSettings.SuspendLayout();

            filterByVersion(e.Tab.Text);

            _grid_Settings.ResumeLayout();
            _grid_PivotSettings.ResumeLayout();
        }

        #endregion

        #region Context Menu events

        private void _contextMenuStrip_Server_Opening(object sender, CancelEventArgs e)
        {
            bool isServerView = (m_gridDisplay == DisplayType.ByServer);
            bool isServer = (_grid_Settings.ActiveRow != null);
            if (isServer)
            {
                string server = _grid_Settings.ActiveRow.Cells[colConnection].Text;

                Sql.RegisteredServer registeredServer = Program.gController.Repository.RegisteredServers.Find(server);
            }

            // Enable/disable based on the node type.
            _cmsi_Server_exploreUserPermissions.Enabled = isServer && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.UserPermissions);
            _cmsi_Server_exploreSnapshot.Enabled = isServer && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.ObjectPermissions);
            _cmsi_Server_viewAuditHistory.Enabled = isServer && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Properties);
            _cmsi_Server_registerSQLServer.Enabled = Program.gController.Permissions.hasSecurity(Utility.Security.Functions.AuditSQLServer);
            _cmsi_Server_removeSQLServer.Enabled = isServer && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Delete);
            _cmsi_Server_configureDataCollection.Enabled = isServer && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.ConfigureAuditSettings);
            _cmsi_Server_collectDataSnapshot.Enabled = isServer && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Collect);
            _cmsi_Server_refresh.Enabled = Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Refresh);
            _cmsi_Server_properties.Enabled = isServer && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Properties);

            // Fix visibility based on grid type
            _cmsi_Server_exploreUserPermissions.Visible =
                _cmsi_Server_exploreSnapshot.Visible =
                _cmsi_Server_viewAuditHistory.Visible =
                _cmsi_Server_Separator_Explore.Visible =
                _cmsi_Server_removeSQLServer.Visible =
                _cmsi_Server_configureDataCollection.Visible =
                _cmsi_Server_collectDataSnapshot.Visible =
                _cmsi_Server_properties.Visible = isServerView;
        }

        private void _cmsi_Server_exploreUserPermissions_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showPermissions(Views.View_PermissionExplorer.Tab.UserPermissions);

            Cursor = Cursors.Default;
        }

        private void _cmsi_Server_exploreSnapshot_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showPermissions(Views.View_PermissionExplorer.Tab.ObjectPermissions);

            Cursor = Cursors.Default;
        }

        private void _cmsi_Server_viewAuditHistory_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showPermissions(Views.View_PermissionExplorer.Tab.SnapshotSummary);

            Cursor = Cursors.Default;
        }

        private void _cmsi_Server_registerSQLServer_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showNewAuditServer();

            Cursor = Cursors.Default;
        }

        private void _cmsi_Server_removeSQLServer_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showDelete();

            Cursor = Cursors.Default;
        }

        private void _cmsi_Server_configureDataCollection_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showConfigure();

            Cursor = Cursors.Default;
        }

        private void _cmsi_Server_collectDataSnapshot_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showCollect();

            Cursor = Cursors.Default;
        }

        private void _cmsi_Server_refresh_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showRefresh();

            Cursor = Cursors.Default;
        }

        private void _cmsi_Server_properties_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showProperties();

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

        private void _grid_Settings_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
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

            band.Columns[colServer].Hidden = true;
            band.Columns[colServer].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            band.Columns[colInstance].Hidden = true;
            band.Columns[colInstance].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            band.Columns[colConnection].Header.Caption = "SQL Server";
            band.Columns[colConnection].Header.Fixed = true;

            band.Columns[colAuthMode].Header.Caption = "Authentication Mode";

            band.Columns[colOs].Header.Caption = "Operating System";

            band.Columns[colVersion].Header.Caption = "Version";
            band.Columns[colVersion].SortComparer = new SqlVersionComparer();

            band.Columns[colEdition].Header.Caption = "Edition";

            band.Columns[colStatus].Hidden = true;
            band.Columns[colStatus].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            band.Columns[colStartTime].Header.Caption = "Collected";
            band.Columns[colStartTime].Format = Utility.Constants.DATETIME_FORMAT;

            band.Columns[colEndTime].Hidden = true;
            band.Columns[colEndTime].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            band.Columns[colAutomated].Hidden = true;
            band.Columns[colAutomated].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            band.Columns[colNumObject].Header.Caption = "Objects";
            band.Columns[colNumObject].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            band.Columns[colNumObject].Format = NumericFormat;

            band.Columns[colNumPermission].Header.Caption = "Permissions";
            band.Columns[colNumPermission].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            band.Columns[colNumObject].Format = NumericFormat;

            band.Columns[colNumLogin].Header.Caption = "Logins";
            band.Columns[colNumLogin].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            band.Columns[colNumObject].Format = NumericFormat;

            band.Columns[colNumWindowsGroupMember].Header.Caption = "Windows Group Members";
            band.Columns[colNumWindowsGroupMember].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            band.Columns[colNumObject].Format = NumericFormat;

            band.Columns[colBaseline].Hidden = true;
            band.Columns[colBaseline].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            band.Columns[colBaselineComment].Hidden = true;
            band.Columns[colBaselineComment].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            band.Columns[colSnapshotComment].Hidden = true;
            band.Columns[colSnapshotComment].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            band.Columns[colLoginAuditMode].Header.Caption = "Login Audit Mode";

            band.Columns[colEnableProxy].Header.Caption = "Proxy";

            band.Columns[colC2].Header.Caption = "C2 Audit Trace";

            band.Columns[colCrossDb].Header.Caption = "Cross DB Ownership Chaining";

            band.Columns[colCaseSensitive].Header.Caption = "Case Sensitive";

            band.Columns[colHashKey].Hidden = true;
            band.Columns[colHashKey].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            band.Columns[colRegisteredServerId].Hidden = true;
            band.Columns[colRegisteredServerId].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            band.Columns[colCollectorVersion].Hidden = true;
            band.Columns[colCollectorVersion].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            band.Columns[colAllowSystemTableUpdates].Header.Caption = "System Table Updates";

            band.Columns[colRemoteAdminConnectionsEnabled].Header.Caption = "Remote Admin Connections";

            band.Columns[colRemoteAccessEnabled].Header.Caption = "Remote Access";

            band.Columns[colScanForStartupProcsEnabled].Header.Caption = "Scan for Startup Procs";

            band.Columns[colSqlMailXpsEnabled].Header.Caption = "SQL Mail Xps";

            band.Columns[colDatabaseMailXpsEnabled].Header.Caption = "Database Mail Xps";

            band.Columns[colOleAutomationProceduresEnabled].Header.Caption = "Ole Automation Procedures";

            band.Columns[colWebAssistantProceduresEnabled].Header.Caption = "Web Assistant Procedures";

            band.Columns[colXp_cmdshellEnabled].Header.Caption = "Xp_cmdshell";

            band.Columns[colAgentMailProfile].Header.Caption = "Agent Mail Profile";

            band.Columns[colHideInstance].Header.Caption = "Hide Instance";

            band.Columns[colAgentSysadminOnly].Header.Caption = "Agent Sysadmin Only";

            band.Columns[colDomainController].Header.Caption = "Server is Domain Controller";

            band.Columns[colReplicationEnabled].Header.Caption = "Replication";

            band.Columns[colSaPasswordEmpty].Header.Caption = "Sa Account Password Empty";

            band.Columns[colMajorVersion].Header.Caption = "Major Version";

            if ((_ultraTabControl_Settings.SelectedTab != null
                    && _ultraTabControl_Settings.SelectedTab.Text == Sql.VersionName.SQL2000) ||
                (m_serverInstance != null
                    && m_serverInstance.Version == Sql.VersionName.SQL2000))
            {
                band.Columns[colRemoteAdminConnectionsEnabled].Hidden = true;
                band.Columns[colRemoteAdminConnectionsEnabled].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                band.Columns[colSqlMailXpsEnabled].Hidden = true;
                band.Columns[colSqlMailXpsEnabled].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                band.Columns[colDatabaseMailXpsEnabled].Hidden = true;
                band.Columns[colDatabaseMailXpsEnabled].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                band.Columns[colOleAutomationProceduresEnabled].Hidden = true;
                band.Columns[colOleAutomationProceduresEnabled].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                band.Columns[colWebAssistantProceduresEnabled].Hidden = true;
                band.Columns[colWebAssistantProceduresEnabled].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                band.Columns[colXp_cmdshellEnabled].Hidden = true;
                band.Columns[colXp_cmdshellEnabled].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            }
            else
            {
                band.Columns[colRemoteAdminConnectionsEnabled].Hidden = false;
                band.Columns[colRemoteAdminConnectionsEnabled].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                band.Columns[colSqlMailXpsEnabled].Hidden = false;
                band.Columns[colSqlMailXpsEnabled].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                band.Columns[colDatabaseMailXpsEnabled].Hidden = false;
                band.Columns[colDatabaseMailXpsEnabled].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                band.Columns[colOleAutomationProceduresEnabled].Hidden = false;
                band.Columns[colOleAutomationProceduresEnabled].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                band.Columns[colWebAssistantProceduresEnabled].Hidden = false;
                band.Columns[colWebAssistantProceduresEnabled].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                band.Columns[colXp_cmdshellEnabled].Hidden = false;
                band.Columns[colXp_cmdshellEnabled].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
        }

        private void _grid_Settings_DoubleClick(object sender, EventArgs e)
        {
            if (m_gridCellClicked && _grid_Settings.ActiveRow != null)
            {
                showProperties();
            }
        }

        private void _grid_PivotSettings_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            e.Layout.Override.FixedHeaderIndicator = FixedHeaderIndicator.None;
            e.Layout.ColumnChooserEnabled = DefaultableBoolean.True;
            e.Layout.UseFixedHeaders = true;

            UltraGridBand band = e.Layout.Bands[0];

            band.Columns[colSetting].Header.Caption = "Setting";
            band.Columns[colSetting].Header.Fixed = true;
            band.Columns[colSetting].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
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

    public class SqlVersionComparer : System.Collections.IComparer
    {
        internal SqlVersionComparer()
        {

        }

        public int Compare(object x, object y)
        {
            UltraGridCell xCell = (UltraGridCell)x;
            UltraGridCell yCell = (UltraGridCell)y;
            // append a 0 to the front of single digit versions
            string xText = (xCell.Text.Substring(1, 1) == "." ? "0" : string.Empty) + xCell.Text;
            string yText = (yCell.Text.Substring(1, 1) == "." ? "0" : string.Empty) + yCell.Text;

            return xText.CompareTo(yText);
        }
    }
}
