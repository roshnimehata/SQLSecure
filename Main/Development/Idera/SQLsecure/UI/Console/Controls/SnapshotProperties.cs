using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolTip;

using Idera.SQLsecure.Core.Accounts;
using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Sql;
using Idera.SQLsecure.UI.Console.Utility;
using Idera.SQLsecure.UI.Console.Forms;

namespace Idera.SQLsecure.UI.Console.Controls
{
    public partial class SnapshotProperties : UserControl, Interfaces.IView, Interfaces.ICommandHandler
    {
        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Controls.SnapshotProperties");

        #region IView

        void Interfaces.IView.SetContext(Interfaces.IDataContext contextIn)
        {
            // Get the server and snapshotid from the context.
            m_ServerInstance = ((Data.PermissionExplorer)contextIn).ServerInstance;
            if (m_ServerInstance != null)
            {
                Sql.RegisteredServer.GetServer(Program.gController.Repository.ConnectionString,
                                                    m_ServerInstance.ConnectionName, out m_ServerInstance);
            }

            if (m_ServerInstance == null)
            {
                m_SnapshotId = 0;

                loadSnapshotData();
            }
            else
            {
                //Start-SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
                if (m_ServerInstance.ServerType == ServerType.AzureSQLDatabase)
                {
                    
                    this._ultraTabControl.Tabs["_tab_WindowsAccounts"].Text = "Azure AD Accounts";
                }
                else
                {
                    this._ultraTabControl.Tabs["_tab_WindowsAccounts"].Text = "Windows Accounts";
                }
                //End-SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
                int snapshotid = ((Data.PermissionExplorer)contextIn).SnapShotId;

                if (m_SnapshotId == snapshotid)
                {
                    showRefresh();
                }
                else
                {
                    if (snapshotid == 0)
                    {
                        snapshotid = m_ServerInstance.LastCollectionSnapshotId;
                    }
                    m_SnapshotId = snapshotid;

                    loadSnapshotData();
                }
            }
        }

        String Interfaces.IView.HelpTopic
        {
            get { return Utility.Help.ExploreSnapshotsHelpTopic; }
        }
        String Interfaces.IView.ConceptTopic
        {
            get { return Utility.Help.ExploreSnapshotsConceptTopic; }
        }
        String Interfaces.IView.Title
        {
            get { return ""; }
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
                    Debug.Assert(false, "Unknown command passed to Snapshot Properties");
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
            if (Forms.Form_BaselineSnapshot.Process(m_Snapshot) == DialogResult.OK)
            {
                showRefresh();
            }
        }

        protected virtual void showCollect()
        {
            logX.loggerX.Info("Error - SnapshotProperties showCollect command called erroneously");
        }

        protected virtual void showConfigure()
        {
            Forms.Form_SqlServerProperties.Process(m_ServerInstance.ConnectionName, Forms.Form_SqlServerProperties.RequestedOperation.EditCofiguration, Program.gController.isAdmin);
        }

        protected virtual void showDelete()
        {
            logX.loggerX.Info("Error - SnapshotProperties showDelete command called erroneously");
        }

        protected virtual void showProperties()
        {
            logX.loggerX.Info("Error - SnapshotProperties showProperties command called erroneously");
        }

        protected virtual void showRefresh()
        {
            loadSnapshotData();
        }

        protected virtual void showPermissions(Views.View_PermissionExplorer.Tab tabIn)
        {
            Program.gController.ShowRootView(new Utility.NodeTag(new Data.PermissionExplorer(m_ServerInstance, m_SnapshotId, tabIn),
                                                        Utility.View.PermissionExplorer));
        }

        protected virtual void showPermissions(Sql.User user, Views.View_PermissionExplorer.Tab tabIn)
        {
            Program.gController.ShowRootView(new Utility.NodeTag(new Data.PermissionExplorer(m_ServerInstance, m_SnapshotId, user, tabIn),
                                                        Utility.View.PermissionExplorer));
        }

        #endregion

        #region Fields

        Utility.MenuConfiguration m_menuConfiguration;
        private Sql.RegisteredServer m_ServerInstance;
        private int m_SnapshotId;
        private Sql.Snapshot m_Snapshot;
        private string m_SnapshotName;
        private List<Sql.Database> m_Databases;
        private DataTable m_DataTable_Accounts;
        private DataTable m_DataTable_OsAccounts;
        private DataTable m_DataTable_Filters;
        private DataTable m_DataTable_UnresolvedAccounts;
        private DataTable m_DataTable_UnresolvedOsAccounts;
        private DataTable m_DataTable_Databases;

        private bool m_gridCellClicked = false;

        #endregion

        #region Ctors

        public SnapshotProperties() : base()
        {
            InitializeComponent();

            // Set the  imagelists.
            _ultraTabControl.ImageList = AppIcons.AppImageList16();

            // Set the icons.            
            _ultraGridWindowsAccounts.DisplayLayout.FilterDropDownButtonImage = AppIcons.AppImage16(AppIcons.Enum.GridFilter);
            _tsbtn_ColumnChooserWindowsAccounts.Image = AppIcons.AppImage16(AppIcons.Enum.GridFieldChooser);
            _tsbtn_GroupByBoxWindowsAccounts.Image = AppIcons.AppImage16(AppIcons.Enum.GridGroupBy);
            _tsbtn_PrintWindowsAccounts.Image = AppIcons.AppImage16(AppIcons.Enum.Print);
            _tsbtn_SaveAsWindowsAccounts.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);

            _ultraGridOsWindowsAccounts.DisplayLayout.FilterDropDownButtonImage = AppIcons.AppImage16(AppIcons.Enum.GridFilter);
            _tsbtn_ColumnChooserOsWindowsAccounts.Image = AppIcons.AppImage16(AppIcons.Enum.GridFieldChooser);
            _tsbtn_GroupByBoxOsWindowsAccounts.Image = AppIcons.AppImage16(AppIcons.Enum.GridGroupBy);
            _tsbtn_PrintOsWindowsAccounts.Image = AppIcons.AppImage16(AppIcons.Enum.Print);
            _tsbtn_SaveAsOsWindowsAccounts.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);

            _ultraGridUnresolvedWindowsAccounts.DisplayLayout.FilterDropDownButtonImage = AppIcons.AppImage16(AppIcons.Enum.GridFilter);
            _tsbtn_GroupByBoxUnresolvedWindowsAccounts.Image = AppIcons.AppImage16(AppIcons.Enum.GridGroupBy);
            _tsbtn_PrintUnresolvedWindowsAccounts.Image = AppIcons.AppImage16(AppIcons.Enum.Print);
            _tsbtn_SaveAsUnresolvedWindowsAccounts.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);

            _ultraGridUnresolvedOsWindowsAccounts.DisplayLayout.FilterDropDownButtonImage = AppIcons.AppImage16(AppIcons.Enum.GridFilter);
            _tsbtn_GroupByBoxUnresolvedOsWindowsAccounts.Image = AppIcons.AppImage16(AppIcons.Enum.GridGroupBy);
            _tsbtn_PrintUnresolvedOsWindowsAccounts.Image = AppIcons.AppImage16(AppIcons.Enum.Print);
            _tsbtn_SaveAsUnresolvedOsWindowsAccounts.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);

            _ultraGridUnavailableDatabases.DisplayLayout.FilterDropDownButtonImage = AppIcons.AppImage16(AppIcons.Enum.GridFilter);
            _tsbtn_GroupByBoxUnavailableDatabases.Image = AppIcons.AppImage16(AppIcons.Enum.GridGroupBy);
            _tsbtn_PrintUnavailableDatabases.Image = AppIcons.AppImage16(AppIcons.Enum.Print);
            _tsbtn_SaveAsUnavailableDatabases.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);

            _toolStripMenuItem_viewGroupMembers.Image = AppIcons.AppImage16(AppIcons.Enum.WindowsGroup);
            _toolStripMenuItem_showPermissions.Image = AppIcons.AppImage16(AppIcons.Enum.WindowsUser);
            //_toolStripMenuItem_viewGroupByBox.Image - uses a checked value/image instead of app image
            _toolStripMenuItem_Save.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);
            _toolStripMenuItem_Print.Image = AppIcons.AppImage16(AppIcons.Enum.Print);

            // Setup missing windows accounts and unavailable database intro.
            _lbl_IntroWindowsAccounts.Text = string.Format(IntroWindowsAccounts,string.Empty);
            _lbl_IntroWindowsAccounts.LinkArea = new LinkArea(_lbl_IntroWindowsAccounts.Text.Length - TellMeMoreLen - 1, TellMeMoreLen);
            _lbl_IntroFilters.Text = string.Format(IntroFilters, string.Empty);
            _lbl_IntroFilters.LinkArea = new LinkArea(_lbl_IntroFilters.Text.Length - TellMeMoreLen - 1, TellMeMoreLen);
            _lbl_IntroUnresolvedWindowsAccounts.Text = IntroUnresolvedWindowsAccounts;
            _lbl_IntroUnresolvedWindowsAccounts.LinkArea = new LinkArea(IntroUnresolvedWindowsAccounts.Length - TellMeMoreLen - 1, TellMeMoreLen);
            _lbl_IntroUnresolvedOsWindowsAccounts.Text = IntroUnresolvedWindowsAccounts;
            _lbl_IntroUnresolvedOsWindowsAccounts.LinkArea = new LinkArea(IntroUnresolvedWindowsAccounts.Length - TellMeMoreLen - 1, TellMeMoreLen);
            _lbl_IntroUnavailableDatabases.Text = IntroUnavailableDatabases;
            _lbl_IntroUnavailableDatabases.LinkArea = new LinkArea(IntroUnavailableDatabases.Length - TellMeMoreLen - 1, TellMeMoreLen);

            // Initialize base class fields.
            m_menuConfiguration = new Utility.MenuConfiguration();

            // Create the datasources for the grids
            m_DataTable_Accounts = new DataTable();
            m_DataTable_Accounts.Columns.Add(colIcon, typeof(Image));
            m_DataTable_Accounts.Columns.Add(colName, typeof(string));
            m_DataTable_Accounts.Columns.Add(colDomain, typeof(string));
            m_DataTable_Accounts.Columns.Add(colAccount, typeof(string));
            m_DataTable_Accounts.Columns.Add(colType, typeof(string));
            m_DataTable_Accounts.Columns.Add(colSid, typeof(byte[]));
            m_DataTable_Accounts.Columns.Add(colLogin, typeof(string));

            m_DataTable_OsAccounts = new DataTable();
            m_DataTable_OsAccounts.Columns.Add(colIcon, typeof(Image));
            m_DataTable_OsAccounts.Columns.Add(colName, typeof(string));
            m_DataTable_OsAccounts.Columns.Add(colDomain, typeof(string));
            m_DataTable_OsAccounts.Columns.Add(colAccount, typeof(string));
            m_DataTable_OsAccounts.Columns.Add(colType, typeof(string));
            m_DataTable_OsAccounts.Columns.Add(colSid, typeof(byte[]));
            m_DataTable_OsAccounts.Columns.Add(colLogin, typeof(string));

            m_DataTable_Filters = new DataTable();
            m_DataTable_Filters.Columns.Add(colFilterName, typeof(string));
            m_DataTable_Filters.Columns.Add(colFilterDescription, typeof(string));
            m_DataTable_Filters.Columns.Add(colFilterDetails, typeof(string));

            m_DataTable_UnresolvedAccounts = new DataTable();
            m_DataTable_UnresolvedAccounts.Columns.Add(colIcon, typeof(Image));
            m_DataTable_UnresolvedAccounts.Columns.Add(colDomain, typeof(string));
            m_DataTable_UnresolvedAccounts.Columns.Add(colAccount, typeof(string));
            m_DataTable_UnresolvedAccounts.Columns.Add(colType, typeof(string));
            m_DataTable_UnresolvedAccounts.Columns.Add(colSid, typeof(byte[]));

            m_DataTable_UnresolvedOsAccounts = new DataTable();
            m_DataTable_UnresolvedOsAccounts.Columns.Add(colIcon, typeof(Image));
            m_DataTable_UnresolvedOsAccounts.Columns.Add(colDomain, typeof(string));
            m_DataTable_UnresolvedOsAccounts.Columns.Add(colAccount, typeof(string));
            m_DataTable_UnresolvedOsAccounts.Columns.Add(colType, typeof(string));
            m_DataTable_UnresolvedOsAccounts.Columns.Add(colSid, typeof(byte[]));

            m_DataTable_Databases = new DataTable();
            m_DataTable_Databases.Columns.Add(colIcon, typeof(Image));
            m_DataTable_Databases.Columns.Add(colDatabase, typeof(string));
            m_DataTable_Databases.Columns.Add(colStatus, typeof(string));

            // hook the toolbar labels to the grids so the heading can be used for printing
            _ultraGridWindowsAccounts.Tag = _tslbl_ItemsWindowsAccounts;
            _ultraGridOsWindowsAccounts.Tag = _tslbl_ItemsOsWindowsAccounts;
            _ultraGridUnresolvedWindowsAccounts.Tag = _tslbl_ItemsUnresolvedWindowsAccounts;
            _ultraGridUnresolvedOsWindowsAccounts.Tag = _tslbl_ItemsUnresolvedOsWindowsAccounts;
            _ultraGridUnavailableDatabases.Tag = _tslbl_ItemsUnavailableDatabases;

            // hook the grids to the toolbars so they can be used for button processing
            _ts_WindowsAccounts.Tag = _ultraGridWindowsAccounts;
            _ts_OsWindowsAccounts.Tag = _ultraGridOsWindowsAccounts;
            _ts_UnresolvedWindowsAccounts.Tag = _ultraGridUnresolvedWindowsAccounts;
            _ts_UnresolvedOsWindowsAccounts.Tag = _ultraGridUnresolvedOsWindowsAccounts;
            _ts_UnavailableDatabases.Tag = _ultraGridUnavailableDatabases;

            // set all grids to start in the same initial display mode
            _ultraGridWindowsAccounts.DisplayLayout.GroupByBox.Hidden =
                _ultraGridOsWindowsAccounts.DisplayLayout.GroupByBox.Hidden =
                _ultraGridUnresolvedWindowsAccounts.DisplayLayout.GroupByBox.Hidden =
                _ultraGridUnresolvedOsWindowsAccounts.DisplayLayout.GroupByBox.Hidden =
                _ultraGridUnavailableDatabases.DisplayLayout.GroupByBox.Hidden = true;

            // Setup the print document format.
            _ultraGridPrintDocument.DefaultPageSettings.Landscape = true;
            _ultraGridPrintDocument.DefaultPageSettings.Color = false;
            _ultraGridPrintDocument.FitWidthToPages = 1;

            // Hide the focus rectangles on tabs and grids
            _ultraTabControl.DrawFilter = new HideFocusRectangleDrawFilter();
            _ultraGridWindowsAccounts.DrawFilter = new HideFocusRectangleDrawFilter();
            _ultraGridOsWindowsAccounts.DrawFilter = new HideFocusRectangleDrawFilter();
            _ultraGrid_Filters.DrawFilter = new HideFocusRectangleDrawFilter();
            _ultraGridUnresolvedWindowsAccounts.DrawFilter = new HideFocusRectangleDrawFilter();
            _ultraGridUnresolvedOsWindowsAccounts.DrawFilter = new HideFocusRectangleDrawFilter();
            _ultraGridUnavailableDatabases.DrawFilter = new HideFocusRectangleDrawFilter();
        }

        #endregion

        #region Queries, Columns & Constants

        private static string QueryGetLogins = @"SELECT name, type, login, sid FROM SQLsecure.dbo.vwallaccounts WHERE snapshotid = {0} and type in ({1}) ORDER BY name";
        private static string QueryGetOsLogins = @"SELECT name, type, login, sid FROM SQLsecure.dbo.vwallosaccounts WHERE snapshotid = {0} and type in ({1}) ORDER BY name";

        // Windows Accounts & Unresolved Accounts columns
        private const string colIcon = @"icon";
        private const string colName = @"name";
        private const string colDomain = @"domain";
        private const string colAccount = @"account";
        private const string colType = @"type";
        private const string colSid = @"sid";
        private const string colLogin = @"login";

        // Filters columns
        private const string colFilterName = "Filter";
        private const string colFilterDescription = "Description";
        private const string colFilterDetails = "Details";

        // Database columns
        private const string colDatabase = "Database";
        private const string colStatus = "Status";

        private const string NoFilters = @"No filters specified";
        private const string IntroWindowsAccounts = @"The table below contains a{0} list of Windows users and groups that have access to this SQL Server either by a direct SQL Login or inherited via a group membership.  Tell me more.";
        private const string IntroOsWindowsAccounts = @"The table below contains a{0} list of Windows accounts (granted either by a direct permission or inherited via a group membership) that have access to files or registry keys used by this SQL Server. Tell me more.";
        private const string IntroAccountsAll = @" comprehensive";
        private const string IntroAccountsPartial = @" partial";
        private const string IntroFilters = @"Audit data was collected for this snapshot using the filters listed in the table below.  Use this to verify that all needed information was collected.  Tell me more.";
        private const string IntroUnresolvedWindowsAccounts = @"SQLsecure was unable to collect data for the accounts listed in the table below.  This can happen when accounts are deleted or when SQLsecure does not have privileges to collect this information.  It can lead to incomplete SQL Server permissions information for Windows accounts.  Tell me more.";
        private const string IntroUnavailableDatabases = "SQLsecure was unable to collect SQL Server security data for the databases listed in the table below.   This can happen when a database is unavailable during SQLsecure data collection.   For example, a database being backed up is unavailable for data collection.  Tell me more.";
        private const int TellMeMoreLen = 12;
        //SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
        private const string IntroAzureADAccounts = @"The table below contains a{0} list of Azure AD users and groups that have access to this Azure SQL Database either by a direct SQL Login or inherited via a group membership.  Tell me more.";

        private const string WindowsAccounts = "Windows Accounts";
        private static string WindowsAccountsFmt = WindowsAccounts + " ({0})";
        private const string OsWindowsAccounts = "OS Windows Accounts";
        private static string OsWindowsAccountsFmt = OsWindowsAccounts + " ({0})";
        private const string Filters = "Filters";
        private static string FiltersFmt = Filters + " ({0})";
        private const string UnresolvedAccounts = "Suspect Windows Accounts";
        private static string UnresolvedAccountsFmt = UnresolvedAccounts + " ({0})";
        private const string UnresolvedOsAccounts = "Suspect OS Windows Accounts";
        private static string UnresolvedOsAccountsFmt = UnresolvedOsAccounts + " ({0})";
        private const string UnavailableDatabases = "Unavailable Databases";
        private static string UnavailableDatabasesFmt = UnavailableDatabases + " ({0})";
        private const string PrintHeaderDisplay = "{0} for {1} audited on {2}";
        //SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
        private const string AzureADAccounts = "Azure AD Accounts";
        private static string AzureADAccountsFmt = AzureADAccounts + "({0})";

        private const string DelimFieldSep = @"','";
        private const string Delim = @"'";
        private const string UnknownValue = @"Unknown";

        private const string Status_InProgress = @"In Progress";
        private const string Status_Warnings = @"Data Collection completed successfully, but the snapshot contains multiple warnings";

        #endregion

        #region Helpers

        protected void setMenuConfiguration()
        {
            m_menuConfiguration.EditItems[(int)Utility.MenuItems_Edit.Properties] = false;

            m_menuConfiguration.PermissionsItems[(int)MenuItems_Permissions.UserPermissions] = true;
            m_menuConfiguration.PermissionsItems[(int)MenuItems_Permissions.ObjectPermissions] = true;

            m_menuConfiguration.SnapshotItems[(int)MenuItems_Snapshots.Baseline] = true;
            m_menuConfiguration.SnapshotItems[(int)MenuItems_Snapshots.Collect] = false;

            Program.gController.SetMenuConfiguration(m_menuConfiguration, this);
        }

        private void loadSnapshotData()
        {
            // Retrieve snapshot & load all data.
            m_Snapshot = Sql.Snapshot.GetSnapShot(m_SnapshotId);
            if (m_Snapshot != null)
            {
                // Get snapshot status, to set the icon.
                m_SnapshotName = m_Snapshot.SnapshotName;
                fillGeneralPage();
                fillWindowsAccountsPage();
                fillOsWindowsAccountsPage();
                fillFiltersPage();
                fillSuspectAccountsPage();
                fillSuspectOsAccountsPage();
                fillUnavailableDatabases();

                // Fix the Intro text to describe Windows Accounts correctly
                if (m_DataTable_Accounts.Rows.Count > 0)
                {
                    if (m_DataTable_UnresolvedAccounts.Rows.Count == 0)
                    {
                        //Start-SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
                        if (m_ServerInstance.ServerType == ServerType.AzureSQLDatabase)
                        {
                            _lbl_IntroWindowsAccounts.Text = string.Format(IntroAzureADAccounts, IntroAccountsAll);
                        }
                        else
                        {
                            _lbl_IntroWindowsAccounts.Text = string.Format(IntroWindowsAccounts, IntroAccountsAll);
                        }
                        //End-SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
                    }
                    else
                    {
                        //Start-SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
                        if (m_ServerInstance.ServerType == ServerType.AzureSQLDatabase)
                        {
                            _lbl_IntroWindowsAccounts.Text = string.Format(IntroAzureADAccounts, IntroAccountsPartial);
                        }
                        else
                        {
                            _lbl_IntroWindowsAccounts.Text = string.Format(IntroWindowsAccounts, IntroAccountsPartial);
                        }
                        //End-SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
                    }
                }
                else
                {
                    //Start-SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
                    if (m_ServerInstance.ServerType == ServerType.AzureSQLDatabase)
                    {
                        _lbl_IntroWindowsAccounts.Text = string.Format(IntroAzureADAccounts, string.Empty);
                    }
                    else
                    {
                        _lbl_IntroWindowsAccounts.Text = string.Format(IntroWindowsAccounts, string.Empty);
                    }
                    //End-SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
                }
                _lbl_IntroWindowsAccounts.LinkArea = new LinkArea(_lbl_IntroWindowsAccounts.Text.Length - TellMeMoreLen - 1, TellMeMoreLen);

                // Fix the Intro text to describe Os Windows Accounts correctly
                if (m_DataTable_OsAccounts.Rows.Count > 0)
                {
                    if (m_DataTable_UnresolvedOsAccounts.Rows.Count == 0)
                    {
                        _lbl_IntroOsWindowsAccounts.Text = string.Format(IntroOsWindowsAccounts, IntroAccountsAll);
                    }
                    else
                    {
                        _lbl_IntroOsWindowsAccounts.Text = string.Format(IntroOsWindowsAccounts, IntroAccountsPartial);
                    }
                }
                else
                {
                    _lbl_IntroOsWindowsAccounts.Text = string.Format(IntroOsWindowsAccounts, string.Empty);
                }
                _lbl_IntroOsWindowsAccounts.LinkArea = new LinkArea(_lbl_IntroOsWindowsAccounts.Text.Length - TellMeMoreLen - 1, TellMeMoreLen);
            }
            else
            {
                //Clear all fields and display values if there is no data

                // General Info
                _pictureBox_AuditStatus.Image = AppIcons.AppImage48(AppIcons.EnumImageList48.StatusError);

                if (m_ServerInstance == null)
                {
                    _linkLabel_Status.Text = Utility.ErrorMsgs.ServerNoSnapshots;
                    _linkLabel_Status.LinkArea = new LinkArea(0,0);
                    MsgBox.ShowWarning(ErrorMsgs.ObjectExplorerCaption, ErrorMsgs.ServerNotRegistered);
                }
                else
                {
                    _linkLabel_Status.Text = Utility.ErrorMsgs.ServerMissingSnapshot;
                    _linkLabel_Status.LinkArea = new LinkArea(0, 0);
                }

                m_SnapshotName = 
                    _label_StartTime.Text =
                    _label_Duration.Text =
                    _label_Baseline.Text =
                    _label_BaselineComment.Text =
                    _label_Objects.Text =
                    _label_Permissions.Text =
                    _label_Logins.Text =
                    _label_WindowsGroups.Text =
                    _label_Databases.Text = String.Empty;

                // Windows Accounts
                //Start-SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
                if (m_ServerInstance.ServerType == ServerType.AzureSQLDatabase)
                {
                    _ultraTabControl.Tabs["_tab_WindowsAccounts"].Text =
                      _tslbl_ItemsWindowsAccounts.Text = AzureADAccounts;
                    _lbl_IntroWindowsAccounts.Text = string.Format(IntroAzureADAccounts, string.Empty);
                    _lbl_IntroWindowsAccounts.LinkArea = new LinkArea(_lbl_IntroWindowsAccounts.Text.Length - TellMeMoreLen - 1, TellMeMoreLen);
                }
                else
                {
                    _ultraTabControl.Tabs["_tab_WindowsAccounts"].Text =
                      _tslbl_ItemsWindowsAccounts.Text = WindowsAccounts;
                    _lbl_IntroWindowsAccounts.Text = string.Format(IntroWindowsAccounts, string.Empty);
                    _lbl_IntroWindowsAccounts.LinkArea = new LinkArea(_lbl_IntroWindowsAccounts.Text.Length - TellMeMoreLen - 1, TellMeMoreLen);
                }
                //End-SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
                m_DataTable_Accounts.Clear();
                _ultraGridWindowsAccounts.BeginUpdate();
                _ultraGridWindowsAccounts.SetDataBinding(m_DataTable_Accounts, null);
                _ultraGridWindowsAccounts.EndUpdate();

                // Os Windows Accounts
                _ultraTabControl.Tabs["_tab_OsWindowsAccounts"].Text =
                    _tslbl_ItemsOsWindowsAccounts.Text = OsWindowsAccounts;
                _lbl_IntroOsWindowsAccounts.Text = string.Format(IntroOsWindowsAccounts, string.Empty);
                _lbl_IntroOsWindowsAccounts.LinkArea = new LinkArea(_lbl_IntroOsWindowsAccounts.Text.Length - TellMeMoreLen - 1, TellMeMoreLen);
                m_DataTable_OsAccounts.Clear();
                _ultraGridOsWindowsAccounts.BeginUpdate();
                _ultraGridOsWindowsAccounts.SetDataBinding(m_DataTable_OsAccounts, null);
                _ultraGridOsWindowsAccounts.EndUpdate();

                // Filters
                _ultraTabControl.Tabs["_tab_Filters"].Text = Filters;
                m_DataTable_Filters.Clear();
                _ultraGrid_Filters.BeginUpdate();
                _ultraGrid_Filters.SetDataBinding(m_DataTable_Filters, null);
                _ultraGrid_Filters.EndUpdate();
                _rtbx_FilterDetails.Clear();

                // Unresolved Accounts
                _ultraTabControl.Tabs["_tab_SuspectWindowsAccounts"].Text =
                    _tslbl_ItemsUnresolvedWindowsAccounts.Text = UnresolvedAccounts;
                m_DataTable_UnresolvedAccounts.Clear();
                _ultraGridUnresolvedWindowsAccounts.BeginUpdate();
                _ultraGridUnresolvedWindowsAccounts.SetDataBinding(m_DataTable_UnresolvedAccounts, null);
                _ultraGridUnresolvedWindowsAccounts.EndUpdate();
                _label_WellKnownGroups.Text = String.Empty;

                // Unresolved Os Accounts
                _ultraTabControl.Tabs["_tab_SuspectOsWindowsAccounts"].Text =
                    _tslbl_ItemsUnresolvedOsWindowsAccounts.Text = UnresolvedOsAccounts;
                m_DataTable_UnresolvedOsAccounts.Clear();
                _ultraGridUnresolvedOsWindowsAccounts.BeginUpdate();
                _ultraGridUnresolvedOsWindowsAccounts.SetDataBinding(m_DataTable_UnresolvedOsAccounts, null);
                _ultraGridUnresolvedOsWindowsAccounts.EndUpdate();
                _label_WellKnownGroups.Text = String.Empty;

                // Unavailable databases
                _ultraTabControl.Tabs["_tab_SuspectDatabases"].Text =
                    _tslbl_ItemsUnavailableDatabases.Text = UnavailableDatabases;
                m_DataTable_Databases.Clear();
                _ultraGridUnavailableDatabases.BeginUpdate();
                _ultraGridUnavailableDatabases.SetDataBinding(m_DataTable_Databases, null);
                _ultraGridUnavailableDatabases.EndUpdate();
            }
        }

        private void fillGeneralPage()
        {
            if(m_Snapshot.SnapshotComment.Contains("In Progress"))
            {
                _linkLabel_Status.Text = Status_InProgress;
                _linkLabel_Status.LinkArea = new LinkArea(0, Status_InProgress.Length);
            }
            else
            {
                // if there are multiple warnings, show it in a tooltip with a different message
                if (m_Snapshot.Status == Utility.Snapshot.StatusWarning
                    && m_Snapshot.SnapshotComment.Contains(@" and "))
                {
                    _linkLabel_Status.Text = Status_Warnings;
                    _linkLabel_Status.LinkArea = new LinkArea(0, Status_Warnings.Length);
                }
                else
                {
                    _linkLabel_Status.Text = m_Snapshot.SnapshotComment;
                    _linkLabel_Status.LinkArea = new LinkArea(0, 0);
                }
            }
            _label_StartTime.Text = m_Snapshot.StartTime.ToLocalTime().ToString(Utility.Constants.DATETIME_FORMAT);
            if (string.Compare(m_Snapshot.Status, Utility.Snapshot.StatusInProgress) == 0)
            {
                _label_Duration.Text = Utility.Snapshot.StatusInProgressText;
            }
            else
            {
                _label_Duration.Text = m_Snapshot.Duration;
            }
            _label_Baseline.Text = m_Snapshot.Baseline;
            _label_BaselineComment.Text = m_Snapshot.BaselineComment;
            _label_Objects.Text = m_Snapshot.NumObject.ToString("n0");
            _label_Permissions.Text = m_Snapshot.NumPermission.ToString("n0");
            _label_Logins.Text = m_Snapshot.NumLogin.ToString("n0");
            //Start-SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
            if (m_ServerInstance.ServerType == ServerType.AzureSQLDatabase)
            {
                this.label5.Text = "Azure AD accounts:";
                _label_WindowsGroups.Text = m_DataTable_Accounts.Rows.Count.ToString("n0");
            }
            else
            {
                this.label5.Text = "Windows accounts:";
                _label_WindowsGroups.Text = m_Snapshot.NumWindowsGroupMember.ToString("n0");
            }
            //End-SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database

            switch (m_Snapshot.WeakPasswordDectectionEnabled)
            {
                case "Yes":
                    weakPasswordDetectionLink.Text = "Enabled";
                    break;
                case "No":
                    weakPasswordDetectionLink.Text = "Disabled";
                    break;
                default:
                    weakPasswordDetectionLink.Text = "N/A";
                    break;
            }
            weakPasswordDetectionLink.Text = m_Snapshot.WeakPasswordDectectionEnabled == "Yes" ? "Enabled" : "Disabled";

            m_Databases = Sql.Database.GetSnapshotDatabases(m_SnapshotId);
            if(m_Databases != null)
            {
                _label_Databases.Text = m_Databases.Count.ToString("n0");
            }

            if (string.Compare(m_Snapshot.Status, Utility.Snapshot.StatusSuccessful) == 0)
            {
                _pictureBox_AuditStatus.Image = AppIcons.AppImage48(AppIcons.EnumImageList48.StatusGood);
            }
            else if (string.Compare(m_Snapshot.Status, Utility.Snapshot.StatusWarning) == 0)
            {
                _pictureBox_AuditStatus.Image = AppIcons.AppImage48(AppIcons.EnumImageList48.StatusWarning);
            }
            else if (string.Compare(m_Snapshot.Status, Utility.Snapshot.StatusInProgress) == 0)
            {
                _pictureBox_AuditStatus.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.Working;
            }
            else
            {
                _pictureBox_AuditStatus.Image = AppIcons.AppImage48(AppIcons.EnumImageList48.StatusError);
            }
        }

        private void fillWindowsAccountsPage()
        {
            // Get a list of all windows accounts.
            string loginType;
            string accessType;
            string domain;
            string login;

            m_DataTable_Accounts.Clear();

            try
            {
                // Open connection to repository and query permissions.
                logX.loggerX.Info("Retrieve Snapshot Windows Accounts");

                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();
                    //Start-SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
                    string query = string.Empty;
                    if (m_ServerInstance.ServerType == ServerType.AzureSQLDatabase)
                    {
                        query = string.Format("select name, type, login=CASE WHEN a.sid IS NULL THEN 'N' ELSE 'Y' END,sid from SQLsecure.dbo.serverprincipal a where a.snapshotid = {0} and type in ('E', 'X')", m_Snapshot.SnapshotId);
                    }
                    else
                    {
                        string groups = Delim + Sql.LoginType.WindowsUser + DelimFieldSep
                                          + Sql.LoginType.WindowsGroup + DelimFieldSep
                                          + Core.Accounts.ObjectClass.User.ToString() + DelimFieldSep
                                          + Core.Accounts.ObjectClass.Group.ToString() + DelimFieldSep
                                          + Core.Accounts.ObjectClass.LocalGroup.ToString() + DelimFieldSep
                                          + Core.Accounts.ObjectClass.GlobalGroup.ToString() + DelimFieldSep
                                          + Core.Accounts.ObjectClass.UniversalGroup.ToString() + DelimFieldSep
                                          + Core.Accounts.ObjectClass.DistributionGroup.ToString() + DelimFieldSep
                                          + Core.Accounts.ObjectClass.WellknownGroup.ToString() + Delim;
                        query = string.Format(QueryGetLogins, m_Snapshot.SnapshotId, groups);
                    }
                    //End-SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
                    //Get Users
                    // Execute stored procedure and get the users.
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.Text;

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);

                        DataView dv = ds.Tables[0].DefaultView;
                        dv.Sort = colName;
                        DataRow drAccount;

                        foreach (DataRowView drvSource in dv)
                        {
                            drAccount = m_DataTable_Accounts.NewRow();

                            // Determine the image.
                            Image icon = null;
                            switch (Sql.WindowsAccount.StringToType(drvSource[colType].ToString().Trim()))
                            {
                                case Sql.WindowsAccount.Type.Group:
                                case Sql.WindowsAccount.Type.LocalGroup:
                                case Sql.WindowsAccount.Type.GlobalGroup:
                                case Sql.WindowsAccount.Type.UniversalGroup:
                                    icon = AppIcons.AppImage16(AppIcons.Enum.WindowsGroup);
                                    break;
                                case Sql.WindowsAccount.Type.WellKnownGroup:
                                case WindowsAccount.Type.AzureADGroup://SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
                                    icon = AppIcons.AppImage16(AppIcons.Enum.WindowsGroup);
                                    break;
                                case Sql.WindowsAccount.Type.User:
                                case WindowsAccount.Type.AzureADUSer://Start-SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
                                    icon = AppIcons.AppImage16(AppIcons.Enum.WindowsUser);
                                    break;
                                default:
                                    icon = AppIcons.AppImage16(AppIcons.Enum.Unknown);
                                    break;
                            }
                            drAccount[colIcon] = icon;

                            drAccount[colName] = drvSource[colName];

                            Path.SplitSamPath(drvSource[colName].ToString(), out domain, out login);
                            drAccount[colDomain] = domain;
                            drAccount[colAccount] = login;

                            switch (drvSource[colType].ToString().Trim())
                            {
                                case ServerLoginTypes.SqlLogin:
                                    loginType = ServerLoginTypes.SqlLoginText;
                                    break;
                                case ServerLoginTypes.WindowsGroup:
                                    loginType = ServerLoginTypes.WindowsGroupText;
                                    break;
                                case ServerLoginTypes.WindowsUser:
                                    loginType = ServerLoginTypes.WindowsUserText;
                                    break;
                                case ServerLoginTypes.LocalGroup:
                                    loginType = ServerLoginTypes.LocalGroupText;
                                    break;
                                case ServerLoginTypes.GlobalGroup:
                                    loginType = ServerLoginTypes.GlobalGroupText;
                                    break;
                                case ServerLoginTypes.UniversalGroup:
                                    loginType = ServerLoginTypes.UniversalGroupText;
                                    break;
                                case ServerLoginTypes.DistributionGroup:
                                    loginType = ServerLoginTypes.DistributionGroupText;
                                    break;
                                case ServerLoginTypes.WellknownGroup:
                                    loginType = ServerLoginTypes.WellknownGroupText;
                                    break;
                                case ServerLoginTypes.User:
                                    loginType = ServerLoginTypes.UserText;
                                    break;
                                //Start-SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
                                case ServerLoginTypes.AzureADUser:
                                    loginType = ServerLoginTypes.AzureADUserText;
                                    break;
                                case ServerLoginTypes.AzureADGroup:
                                    loginType = ServerLoginTypes.AzureADGrouptext;
                                    break;
                                //End-SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
                                default:
                                    //Debug.Assert(false, "Unknown status encountered");
                                    logX.loggerX.Warn("Warning - unknown User Type encountered", drvSource[colType]);
                                    loginType = UnknownValue;
                                    break;
                            }
                            drAccount[colType] = loginType;

                            switch (drvSource[colLogin].ToString())
                            {
                                case ServerAccessTypes.Direct:
                                    accessType = ServerAccessTypes.DirectText;
                                    break;
                                case ServerAccessTypes.Indirect:
                                    accessType = ServerAccessTypes.IndirectText;
                                    break;
                                default:
                                    //Debug.Assert(false, "Unknown status encountered");
                                    logX.loggerX.Warn("Warning - unknown Source (Access Type) encountered", drvSource[colLogin]);
                                    accessType = UnknownValue;
                                    break;
                            }
                            drAccount[colLogin] = accessType;
                            drAccount[colSid] = drvSource[colSid];

                            m_DataTable_Accounts.Rows.Add(drAccount);
                        }

                        _ultraGridWindowsAccounts.BeginUpdate();
                        _ultraGridWindowsAccounts.SetDataBinding(m_DataTable_Accounts, null);
                        _ultraGridWindowsAccounts.EndUpdate();

                        //Start-SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
                        if (m_ServerInstance.ServerType == ServerType.AzureSQLDatabase)
                        {
                            _ultraTabControl.Tabs["_tab_WindowsAccounts"].Text =
                                  _tslbl_ItemsWindowsAccounts.Text = string.Format(AzureADAccountsFmt, dv.Count);
                        }
                        else
                        {
                            _ultraTabControl.Tabs["_tab_WindowsAccounts"].Text =
                                  _tslbl_ItemsWindowsAccounts.Text = string.Format(WindowsAccountsFmt, dv.Count);
                        }
                        //End-SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
                    }
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Info("ERROR - unable to load Windows Accounts from the selected Snapshot.", ex);
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.SnapshotPropertiesCaption, ex.Message);
            }
        }

        private void fillOsWindowsAccountsPage()
        {
            // Get a list of all windows accounts.
            string loginType;
            string accessType;
            string domain;
            string login;

            m_DataTable_OsAccounts.Clear();

            try
            {
                // Open connection to repository and query permissions.
                logX.loggerX.Info("Retrieve Snapshot Os Windows Accounts");

                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    string groups = Delim + Sql.LoginType.WindowsUser + DelimFieldSep
                                        + Sql.LoginType.WindowsGroup + DelimFieldSep
                                        + Core.Accounts.ObjectClass.User.ToString() + DelimFieldSep
                                        + Core.Accounts.ObjectClass.Group.ToString() + DelimFieldSep
                                        + Core.Accounts.ObjectClass.LocalGroup.ToString() + DelimFieldSep
                                        + Core.Accounts.ObjectClass.GlobalGroup.ToString() + DelimFieldSep
                                        + Core.Accounts.ObjectClass.UniversalGroup.ToString() + DelimFieldSep
                                        + Core.Accounts.ObjectClass.DistributionGroup.ToString() + DelimFieldSep
                                        + Core.Accounts.ObjectClass.WellknownGroup.ToString() + Delim;
                    string query = string.Format(QueryGetOsLogins, m_Snapshot.SnapshotId, groups);

                    //Get Users
                    // Execute stored procedure and get the users.
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.Text;

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);

                        DataView dv = ds.Tables[0].DefaultView;
                        dv.Sort = colName;
                        DataRow drAccount;

                        foreach (DataRowView drvSource in dv)
                        {
                            drAccount = m_DataTable_OsAccounts.NewRow();

                            // Determine the image.
                            Image icon = null;
                            switch (Sql.WindowsAccount.StringToType(drvSource[colType].ToString().Trim()))
                            {
                                case Sql.WindowsAccount.Type.Group:
                                case Sql.WindowsAccount.Type.LocalGroup:
                                case Sql.WindowsAccount.Type.GlobalGroup:
                                case Sql.WindowsAccount.Type.UniversalGroup:
                                    icon = AppIcons.AppImage16(AppIcons.Enum.WindowsGroup);
                                    break;
                                case Sql.WindowsAccount.Type.WellKnownGroup:
                                    icon = AppIcons.AppImage16(AppIcons.Enum.WindowsGroup);
                                    break;
                                case Sql.WindowsAccount.Type.User:
                                    icon = AppIcons.AppImage16(AppIcons.Enum.WindowsUser);
                                    break;
                                default:
                                    icon = AppIcons.AppImage16(AppIcons.Enum.Unknown);
                                    break;
                            }
                            drAccount[colIcon] = icon;

                            drAccount[colName] = drvSource[colName];

                            Path.SplitSamPath(drvSource[colName].ToString(), out domain, out login);
                            drAccount[colDomain] = domain;
                            drAccount[colAccount] = login;

                            switch (drvSource[colType].ToString().Trim())
                            {
                                case ServerLoginTypes.SqlLogin:
                                    loginType = ServerLoginTypes.SqlLoginText;
                                    break;
                                case ServerLoginTypes.WindowsGroup:
                                    loginType = ServerLoginTypes.WindowsGroupText;
                                    break;
                                case ServerLoginTypes.WindowsUser:
                                    loginType = ServerLoginTypes.WindowsUserText;
                                    break;
                                case ServerLoginTypes.LocalGroup:
                                    loginType = ServerLoginTypes.LocalGroupText;
                                    break;
                                case ServerLoginTypes.GlobalGroup:
                                    loginType = ServerLoginTypes.GlobalGroupText;
                                    break;
                                case ServerLoginTypes.UniversalGroup:
                                    loginType = ServerLoginTypes.UniversalGroupText;
                                    break;
                                case ServerLoginTypes.DistributionGroup:
                                    loginType = ServerLoginTypes.DistributionGroupText;
                                    break;
                                case ServerLoginTypes.WellknownGroup:
                                    loginType = ServerLoginTypes.WellknownGroupText;
                                    break;
                                case ServerLoginTypes.User:
                                    loginType = ServerLoginTypes.UserText;
                                    break;
                                default:
                                    //Debug.Assert(false, "Unknown status encountered");
                                    logX.loggerX.Warn("Warning - unknown User Type encountered", drvSource[colType]);
                                    loginType = UnknownValue;
                                    break;
                            }
                            drAccount[colType] = loginType;

                            switch (drvSource[colLogin].ToString())
                            {
                                case ServerAccessTypes.Direct:
                                    accessType = ServerAccessTypes.DirectText;
                                    break;
                                case ServerAccessTypes.Indirect:
                                    accessType = ServerAccessTypes.IndirectText;
                                    break;
                                default:
                                    //Debug.Assert(false, "Unknown status encountered");
                                    logX.loggerX.Warn("Warning - unknown Source (Access Type) encountered", drvSource[colLogin]);
                                    accessType = UnknownValue;
                                    break;
                            }
                            drAccount[colLogin] = accessType;
                            drAccount[colSid] = drvSource[colSid];

                            m_DataTable_OsAccounts.Rows.Add(drAccount);
                        }

                        _ultraGridOsWindowsAccounts.BeginUpdate();
                        _ultraGridOsWindowsAccounts.SetDataBinding(m_DataTable_OsAccounts, null);
                        _ultraGridOsWindowsAccounts.EndUpdate();

                        _ultraTabControl.Tabs["_tab_OsWindowsAccounts"].Text =
                            _tslbl_ItemsOsWindowsAccounts.Text = string.Format(OsWindowsAccountsFmt, dv.Count);
                    }
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Info("ERROR - unable to load OS Windows Accounts from the selected Snapshot.", ex);
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.SnapshotPropertiesCaption, ex.Message);
            }
        }

        private void fillFiltersPage()
        {
            m_DataTable_Filters.Clear();

            List<Sql.DataCollectionFilter> filters = 
                    Sql.DataCollectionFilter.GetSnapshotFilters(m_Snapshot.ConnectionName, m_SnapshotId);
            // There should be some filter.
            if (filters.Count == 0)
            {
                DataRow row = m_DataTable_Filters.NewRow();
                row[colFilterName] = NoFilters;
                m_DataTable_Filters.Rows.Add(row);
            }
            else
            {
                foreach (Sql.DataCollectionFilter filter in filters)
                {
                    DataRow row = m_DataTable_Filters.NewRow();
                    row[colFilterName] = filter.FilterName;
                    row[colFilterDescription] = filter.FilterName;
                    row[colFilterDetails] = filter.FilterDetails;
                    m_DataTable_Filters.Rows.Add(row);
                }
            }
            _ultraTabControl.Tabs["_tab_Filters"].Text = string.Format(FiltersFmt, filters.Count);

            _ultraGrid_Filters.BeginUpdate();
            _ultraGrid_Filters.SetDataBinding(m_DataTable_Filters, null);
            _ultraGrid_Filters.EndUpdate();

            _ultraGrid_Filters.Rows[0].Selected = true;

            _ultraGrid_Filters.DisplayLayout.Bands[0].SortedColumns.Add(_ultraGrid_Filters.DisplayLayout.Bands[0].Columns[colFilterName], false, false);
        }

        private void fillSuspectAccountsPage()
        {
            // Get a list of suspect windows accounts.
            List<Sql.WindowsAccount> wal = Sql.WindowsAccount.GetSuspectAccounts(m_Snapshot.SnapshotId);

            // Clear the data table.
            m_DataTable_UnresolvedAccounts.Clear();

            if (wal != null)
            {
                _ultraTabControl.Tabs["_tab_SuspectWindowsAccounts"].Text = 
                    _tslbl_ItemsUnresolvedWindowsAccounts.Text = string.Format(UnresolvedAccountsFmt, wal.Count);

                // Fill the data table.
                int numWellknownGroups = 0;
                foreach (Sql.WindowsAccount wa in wal)
                {
                    // Determine the image.
                    Image icon = null;
                    switch (wa.AccountType)
                    {
                        case Sql.WindowsAccount.Type.Group:
                        case Sql.WindowsAccount.Type.LocalGroup:
                        case Sql.WindowsAccount.Type.GlobalGroup:
                        case Sql.WindowsAccount.Type.UniversalGroup:
                            icon = AppIcons.AppImage16(AppIcons.Enum.WindowsGroup);
                            break;
                        case Sql.WindowsAccount.Type.WellKnownGroup:
                            icon = AppIcons.AppImage16(AppIcons.Enum.WindowsGroup);
                            ++numWellknownGroups;
                            break;
                        case Sql.WindowsAccount.Type.User:
                            icon = AppIcons.AppImage16(AppIcons.Enum.WindowsUser);
                            break;
                        default:
                            icon = AppIcons.AppImage16(AppIcons.Enum.Unknown);
                            break;
                    }
                    //AppIcons.AppImage16(AppIcons.Enum.WindowsUser);
                    m_DataTable_UnresolvedAccounts.Rows.Add(icon, wa.Domain, wa.Account, wa.AccountTypeString, wa.SID.BinarySid);
                }

                // Set the number of wellknown groups count.
                _label_WellKnownGroups.Text = numWellknownGroups.ToString();
            }

            // Update the grid.
            _ultraGridUnresolvedWindowsAccounts.BeginUpdate();
            _ultraGridUnresolvedWindowsAccounts.SetDataBinding(m_DataTable_UnresolvedAccounts, null);
            _ultraGridUnresolvedWindowsAccounts.EndUpdate();
        }

        private void fillSuspectOsAccountsPage()
        {
            // Get a list of suspect windows accounts.
            List<Sql.WindowsAccount> wal = Sql.WindowsAccount.GetSuspectOsAccounts(m_Snapshot.SnapshotId);

            // Clear the data table.
            m_DataTable_UnresolvedOsAccounts.Clear();

            if (wal != null)
            {
                _ultraTabControl.Tabs["_tab_SuspectOsWindowsAccounts"].Text =
                    _tslbl_ItemsUnresolvedOsWindowsAccounts.Text = string.Format(UnresolvedOsAccountsFmt, wal.Count);

                // Fill the data table.
                int numWellknownGroups = 0;
                foreach (Sql.WindowsAccount wa in wal)
                {
                    // Determine the image.
                    Image icon = null;
                    switch (wa.AccountType)
                    {
                        case Sql.WindowsAccount.Type.Group:
                        case Sql.WindowsAccount.Type.LocalGroup:
                        case Sql.WindowsAccount.Type.GlobalGroup:
                        case Sql.WindowsAccount.Type.UniversalGroup:
                            icon = AppIcons.AppImage16(AppIcons.Enum.WindowsGroup);
                            break;
                        case Sql.WindowsAccount.Type.WellKnownGroup:
                            icon = AppIcons.AppImage16(AppIcons.Enum.WindowsGroup);
                            ++numWellknownGroups;
                            break;
                        case Sql.WindowsAccount.Type.User:
                            icon = AppIcons.AppImage16(AppIcons.Enum.WindowsUser);
                            break;
                        default:
                            icon = AppIcons.AppImage16(AppIcons.Enum.Unknown);
                            break;
                    }
                    //AppIcons.AppImage16(AppIcons.Enum.WindowsUser);
                    m_DataTable_UnresolvedOsAccounts.Rows.Add(icon, wa.Domain, wa.Account, wa.AccountTypeString, wa.SID.BinarySid);
                }

                // Set the number of wellknown groups count.
                _label_WellKnownGroups.Text = numWellknownGroups.ToString();
            }

            // Update the grid.
            _ultraGridUnresolvedOsWindowsAccounts.BeginUpdate();
            _ultraGridUnresolvedOsWindowsAccounts.SetDataBinding(m_DataTable_UnresolvedOsAccounts, null);
            _ultraGridUnresolvedOsWindowsAccounts.EndUpdate();
        }

        private void fillUnavailableDatabases()
        {
            // Get a list of snapshot databases.
            List<Sql.Database> list = Sql.Database.GetSnapshotDatabases(m_Snapshot.SnapshotId);

            // Clear the data table.
            m_DataTable_Databases.Clear();

            // Fill the grid.
            int num = 0;
            Image icon = Sql.ObjectType.TypeImage16(Sql.ObjectType.TypeEnum.Database);
            foreach (Sql.Database db in list)
            {
                if (!db.IsAvailable)
                {
                    // Increment count.
                    ++num;

                    // Fill data table row.
                    m_DataTable_Databases.Rows.Add(icon, db.Name, db.Status);
                }
            }

            // Update the counts.
            _ultraTabControl.Tabs["_tab_SuspectDatabases"].Text =
                _tslbl_ItemsUnavailableDatabases.Text = string.Format(UnavailableDatabasesFmt, num);

            // Update the grid.
            _ultraGridUnavailableDatabases.BeginUpdate();
            _ultraGridUnavailableDatabases.SetDataBinding(m_DataTable_Databases, null);
            _ultraGridUnavailableDatabases.EndUpdate();
        }

        #region Grid

        protected void printGrid(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            Debug.Assert(grid.Tag.GetType() == typeof(ToolStripLabel));

            // Associate the print document with the grid & preview dialog here
            // in case we want to change documents for each grid
            _ultraGridPrintDocument.Grid = grid;
            _ultraGridPrintDocument.DocumentName = ErrorMsgs.SnapshotPropertiesCaption;
            if (m_Snapshot != null)
            {
                _ultraGridPrintDocument.Header.TextLeft =
                    string.Format(PrintHeaderDisplay,
                                        ((ToolStripLabel)grid.Tag).Text,
                                        m_ServerInstance.ConnectionName,
                                        m_SnapshotName
                                    );
            }
            _ultraPrintPreviewDialog.Document = _ultraGridPrintDocument;

            // Call ShowDialog to show the print preview dialog.
            _ultraPrintPreviewDialog.ShowDialog();
        }

        protected void saveGrid(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            bool iconHidden = false;
            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //save the current state of the icon column and then hide it before exporting
                    iconHidden = grid.DisplayLayout.Bands[0].Columns[colIcon].Hidden;
                    grid.DisplayLayout.Bands[0].Columns[colIcon].Hidden = true;
                    _ultraGridExcelExporter.Export(grid, _saveFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MsgBox.ShowError(ErrorMsgs.ExportToExcelCaption, ErrorMsgs.FailedToExportToExcelFile, ex);
                }
                grid.DisplayLayout.Bands[0].Columns[colIcon].Hidden = iconHidden;
            }
        }

        protected void showGridColumnChooser(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
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

        #endregion

        #region Events

        private void _lbl_IntroWindowsAccounts_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Program.gController.ShowTopic(Utility.Help.WindowsAccountsHelpTopic);
        }

        private void _lbl_IntroOsWindowsAccounts_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Program.gController.ShowTopic(Utility.Help.WindowsOSAccountsHelpTopic);
        }

        private void _lbl_IntroUnavailableDatabases_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Program.gController.ShowTopic(Utility.Help.MissingDatabasesHelpTopic);
        }

        private void _lbl_IntroUnresolvedWindowsAccounts_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Program.gController.ShowTopic(Utility.Help.MissingUsersHelpTopic);
        }

        private void _lbl_IntroUnresolvedOsWindowsAccounts_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Program.gController.ShowTopic(Utility.Help.MissingOsUsersHelpTopic);
        }

        private void _lbl_IntroFilters_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Program.gController.ShowTopic(Utility.Help.SnapshotFiltersHelpTopic);
        }

        private void weakPasswordDetectionLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form_WeakPasswordDetection.Process(Program.gController.isAdmin);
        }


        #region Grid Events

        private void _ultraGridWindowsAccounts_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            e.Layout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;

            UltraGridBand band;

            band = e.Layout.Bands[0];

            band.Columns[colIcon].Header.Caption = @"Icon";
            band.Columns[colIcon].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            band.Columns[colIcon].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            band.Columns[colIcon].Width = 22;

            band.Columns[colName].Header.Caption = @"Name";
            band.Columns[colName].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            band.Columns[colName].Hidden = true;
            band.Columns[colName].Width = 230;

            band.Columns[colDomain].Header.Caption = @"Domain";
            band.Columns[colDomain].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            band.Columns[colDomain].Hidden = false;
            band.Columns[colDomain].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            band.Columns[colDomain].Width = 120;

            band.Columns[colAccount].Header.Caption = @"Account";
            band.Columns[colAccount].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            band.Columns[colAccount].Hidden = false;
            band.Columns[colAccount].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            band.Columns[colAccount].Width = 164;

            band.Columns[colType].Header.Caption = @"Type";
            band.Columns[colType].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            band.Columns[colType].Width = 120;

            band.Columns[colSid].Hidden = true;
            band.Columns[colSid].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            band.Columns[colLogin].Header.Caption = @"Access";
            band.Columns[colLogin].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            band.Columns[colLogin].Hidden = false;
            band.Columns[colLogin].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            band.Columns[colLogin].Width = 78;
        }

        private void _ultraGridWindowsAccounts_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (m_gridCellClicked)
            {
                UltraGrid grid = (UltraGrid)sender;

                Sql.User user = new Sql.User(grid.ActiveRow.Cells[colName].Text,
                                        new Sid(grid.ActiveRow.Cells[colSid].Value as byte[]),
                                        Sql.LoginType.WindowsLogin,
                                        Sql.User.UserSource.Snapshot);
                showPermissions(user, Views.View_PermissionExplorer.Tab.UserPermissions);
            }
            Cursor = Cursors.Default;
        }

        private void _ultraGridOsWindowsAccounts_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            e.Layout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;

            UltraGridBand band;

            band = e.Layout.Bands[0];

            band.Columns[colIcon].Header.Caption = "";
            band.Columns[colIcon].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            band.Columns[colIcon].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            band.Columns[colIcon].Width = 22;

            band.Columns[colName].Header.Caption = @"Name";
            band.Columns[colName].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            band.Columns[colName].Hidden = true;
            band.Columns[colName].Width = 230;

            band.Columns[colDomain].Header.Caption = @"Domain";
            band.Columns[colDomain].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            band.Columns[colDomain].Hidden = false;
            band.Columns[colDomain].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            band.Columns[colDomain].Width = 120;

            band.Columns[colAccount].Header.Caption = @"Account";
            band.Columns[colAccount].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            band.Columns[colAccount].Hidden = false;
            band.Columns[colAccount].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            band.Columns[colAccount].Width = 164;

            band.Columns[colType].Header.Caption = @"Type";
            band.Columns[colType].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            band.Columns[colType].Width = 120;

            band.Columns[colSid].Hidden = true;
            band.Columns[colSid].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            band.Columns[colLogin].Header.Caption = @"Access";
            band.Columns[colLogin].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            band.Columns[colLogin].Hidden = false;
            band.Columns[colLogin].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            band.Columns[colLogin].Width = 78;
        }

        private void _ultraGrid_Filters_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            e.Layout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.ColumnChooserEnabled = DefaultableBoolean.False;

            UltraGridBand band = e.Layout.Bands[0];

            band.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            e.Layout.Bands[0].Columns[colFilterName].Header.Caption = "Filter";

            e.Layout.Bands[0].Columns[colFilterDescription].Header.Caption = "Description";

            e.Layout.Bands[0].Columns[colFilterDetails].Hidden = true;
        }

        private void _ultraGrid_Filters_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            // If there is any filter selected then show the details
            // in the text box.
            if (_ultraGrid_Filters.Selected.Rows.Count > 0)
            {
                // Set filter details.
                _rtbx_FilterDetails.Rtf = _ultraGrid_Filters.Selected.Rows[0].Cells[colFilterDetails].Text;
            }
            else
            {
                _rtbx_FilterDetails.Rtf = string.Empty;
            }
        }

        private void _ultraGridUnresolvedWindowsAccounts_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            //e.Layout.Override.CellAppearance.BorderAlpha = Alpha.Transparent;
            //e.Layout.Override.RowAppearance.BorderColor = Color.White;

            e.Layout.Bands[0].Columns[colIcon].Header.Caption = "";
            e.Layout.Bands[0].Columns[colIcon].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            e.Layout.Bands[0].Columns[colIcon].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Bands[0].Columns[colIcon].Width = 22;

            e.Layout.Bands[0].Columns[colDomain].Header.Caption = "Domain";
            e.Layout.Bands[0].Columns[colDomain].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Bands[0].Columns[colDomain].AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Bands[0].Columns[colDomain].Width = 200;

            e.Layout.Bands[0].Columns[colAccount].Header.Caption = "Account";
            e.Layout.Bands[0].Columns[colAccount].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Bands[0].Columns[colAccount].AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Bands[0].Columns[colAccount].Width = 200;

            e.Layout.Bands[0].Columns[colSid].Hidden = true;
            e.Layout.Bands[0].Columns[colSid].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            
            e.Layout.Bands[0].Columns[colType].Header.Caption = "Type";
            e.Layout.Bands[0].Columns[colType].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Bands[0].Columns[colType].AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Bands[0].Columns[colType].Width = 112;
        }

        private void _ultraGridUnresolvedOsWindowsAccounts_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //e.Layout.Override.CellAppearance.BorderAlpha = Alpha.Transparent;
            //e.Layout.Override.RowAppearance.BorderColor = Color.White;

            e.Layout.Bands[0].Columns[colIcon].Header.Caption = "";
            e.Layout.Bands[0].Columns[colIcon].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            e.Layout.Bands[0].Columns[colIcon].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Bands[0].Columns[colIcon].Width = 22;

            e.Layout.Bands[0].Columns[colDomain].Header.Caption = "Domain";
            e.Layout.Bands[0].Columns[colDomain].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Bands[0].Columns[colDomain].AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Bands[0].Columns[colDomain].Width = 200;

            e.Layout.Bands[0].Columns[colSid].Hidden = true;
            e.Layout.Bands[0].Columns[colSid].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            e.Layout.Bands[0].Columns[colAccount].Header.Caption = "Account";
            e.Layout.Bands[0].Columns[colAccount].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Bands[0].Columns[colAccount].AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Bands[0].Columns[colAccount].Width = 200;

            e.Layout.Bands[0].Columns[colType].Header.Caption = "Type";
            e.Layout.Bands[0].Columns[colType].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Bands[0].Columns[colType].AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Bands[0].Columns[colType].Width = 112;
        }

        private void _ultraGridUnavailableDatabases_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //e.Layout.Override.CellAppearance.BorderAlpha = Alpha.Transparent;
            //e.Layout.Override.RowAppearance.BorderColor = Color.White;

            e.Layout.Bands[0].Columns[colIcon].Header.Caption = "";
            e.Layout.Bands[0].Columns[colIcon].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            e.Layout.Bands[0].Columns[colIcon].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Bands[0].Columns[colIcon].Width = 22;

            e.Layout.Bands[0].Columns[colDatabase].Header.Caption = "Database";
            e.Layout.Bands[0].Columns[colDatabase].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Bands[0].Columns[colDatabase].AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Bands[0].Columns[colDatabase].Width = 250;

            e.Layout.Bands[0].Columns[colStatus].Header.Caption = "Status";
            e.Layout.Bands[0].Columns[colStatus].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Bands[0].Columns[colStatus].AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Bands[0].Columns[colStatus].Width = 250;
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

        private void _tsbtn_ColumnChooserWindowsAccounts_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            showGridColumnChooser(grid);

            Cursor = Cursors.Default;
        }

        private void _tsbtn_GroupByBoxWindowsAccounts_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            toggleGridGroupByBox(grid);

            Cursor = Cursors.Default;
        }

        private void _tsbtn_SaveAsWindowsAccounts_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            saveGrid(grid);

            Cursor = Cursors.Default;
        }

        private void _tsbtn_PrintWindowsAccounts_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            printGrid(grid);

            Cursor = Cursors.Default;
        }

        private void _tsbtn_ColumnChooserOsWindowsAccounts_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            showGridColumnChooser(grid);

            Cursor = Cursors.Default;
        }

        private void _tsbtn_GroupByBoxOsWindowsAccounts_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            toggleGridGroupByBox(grid);

            Cursor = Cursors.Default;
        }

        private void _tsbtn_SaveAsOsWindowsAccounts_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            saveGrid(grid);

            Cursor = Cursors.Default;
        }

        private void _tsbtn_PrintOsWindowsAccounts_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            printGrid(grid);

            Cursor = Cursors.Default;
        }

        private void _tsbtn_GroupByBoxUnresolvedWindowsAccounts_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            toggleGridGroupByBox(grid);

            Cursor = Cursors.Default;
        }

        private void _tsbtn_SaveAsUnresolvedWindowsAccounts_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            saveGrid(grid);

            Cursor = Cursors.Default;
        }

        private void _tsbtn_PrintUnresolvedWindowsAccounts_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            printGrid(grid);

            Cursor = Cursors.Default;
        }

        private void _tsbtn_GroupByBoxUnresolvedOsWindowsAccounts_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            toggleGridGroupByBox(grid);

            Cursor = Cursors.Default;
        }

        private void _tsbtn_SaveAsUnresolvedOsWindowsAccounts_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            saveGrid(grid);

            Cursor = Cursors.Default;
        }

        private void _tsbtn_PrintUnresolvedOsWindowsAccounts_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            printGrid(grid);

            Cursor = Cursors.Default;
        }

        private void _tsbtn_PrintUnavailableDatabases_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            printGrid(grid);

            Cursor = Cursors.Default;
        }

        private void _tsbtn_SaveAsUnavailableDatabases_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            saveGrid(grid);

            Cursor = Cursors.Default;
        }

        private void _tsbtn_GroupByBoxUnavailableDatabases_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            toggleGridGroupByBox(grid);

            Cursor = Cursors.Default;
        }

        #endregion

        private void SnapshotProperties_Enter(object sender, EventArgs e)
        {
            setMenuConfiguration();
        }

        #region Context Menus

        private void _contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            UltraGrid grid = (UltraGrid)((ContextMenuStrip)sender).SourceControl;

            if (grid.ActiveRow != null)
            {
                //only enable View Groups if there is a group
                _toolStripMenuItem_viewGroupMembers.Enabled = (grid.ActiveRow.Cells[colType].Text.Contains(@"Group"));
                _toolStripMenuItem_showPermissions.Enabled = (grid.ActiveRow.Cells[colSid].Value != null);

                if (grid == _ultraGridUnresolvedWindowsAccounts || grid == _ultraGridUnresolvedOsWindowsAccounts)
                {
                    _toolStripMenuItem_ColumnChooser.Visible = false;
                }
                else
                {
                    _toolStripMenuItem_ColumnChooser.Visible = true;
                }
            }
            else
            {
                _toolStripMenuItem_viewGroupMembers.Enabled = false;
                _toolStripMenuItem_showPermissions.Enabled = false;
                _toolStripMenuItem_ColumnChooser.Enabled = false;
            }
            _toolStripMenuItem_viewGroupByBox.Checked = !grid.DisplayLayout.GroupByBox.Hidden;
        }

        private void _toolStripMenuItem_viewGroupMembers_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            UltraGrid grid = (UltraGrid)((ContextMenuStrip)((ToolStripItem)sender).Owner).SourceControl;

            Sql.User group;

            if (grid == _ultraGridUnresolvedWindowsAccounts || grid == _ultraGridUnresolvedOsWindowsAccounts)
            {

                group = new Sql.User(grid.ActiveRow.Cells[colAccount].Text,
                                                new Sid(grid.ActiveRow.Cells[colSid].Value as byte[]),
                                                Sql.LoginType.WindowsLogin,
                                                Sql.User.UserSource.Snapshot);
            }
            else
            {
                group = new Sql.User(grid.ActiveRow.Cells[colName].Text,
                                                new Sid(grid.ActiveRow.Cells[colSid].Value as byte[]),
                                                Sql.LoginType.WindowsLogin,
                                                Sql.User.UserSource.Snapshot);
            }

            bool useOsAccounts = grid == _ultraGridOsWindowsAccounts;

            Sql.User user = Forms.Form_SelectGroupMember.GetUser(m_SnapshotId, group, useOsAccounts);

            Cursor = Cursors.WaitCursor;

            if (user != null)
            {
                showPermissions(user, Views.View_PermissionExplorer.Tab.UserPermissions);
            }

            Cursor = Cursors.Default;
        }

        private void _toolStripMenuItem_showPermissions_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            UltraGrid grid = (UltraGrid)((ContextMenuStrip)((ToolStripItem)sender).Owner).SourceControl;

            if (grid == _ultraGridOsWindowsAccounts || grid == _ultraGridUnresolvedOsWindowsAccounts)
            {
                Form_OSObjectPermissions.DisplayPermissions(m_SnapshotId,
                                                            String.Format("{0}\\{1}", grid.ActiveRow.Cells[colDomain].Text, grid.ActiveRow.Cells[colAccount].Text),
                                                            grid.ActiveRow.Cells[colSid].Value as byte[]);
            }
            else
            {
                Sql.User user;

                if (grid == _ultraGridUnresolvedWindowsAccounts)
                {
                    user = new Sql.User(grid.ActiveRow.Cells[colAccount].Text,
                                            new Sid(grid.ActiveRow.Cells[colSid].Value as byte[]),
                                            Sql.LoginType.WindowsLogin,
                                            Sql.User.UserSource.Snapshot);
                }
                else
                {
                    user = new Sql.User(grid.ActiveRow.Cells[colName].Text,
                                            new Sid(grid.ActiveRow.Cells[colSid].Value as byte[]),
                                            Sql.LoginType.WindowsLogin,
                                            Sql.User.UserSource.Snapshot);
                }
                showPermissions(user, Views.View_PermissionExplorer.Tab.UserPermissions);
            }
            Cursor = Cursors.Default;
        }

        private void _toolStripMenuItem_ColumnChooser_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            UltraGrid grid = (UltraGrid)((ContextMenuStrip)((ToolStripItem)sender).Owner).SourceControl;

            showGridColumnChooser(grid);

            Cursor = Cursors.Default;
        }

        private void _toolStripMenuItem_viewGroupByBox_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            UltraGrid grid = (UltraGrid)((ContextMenuStrip)((ToolStripItem)sender).Owner).SourceControl;

            toggleGridGroupByBox(grid);

            Cursor = Cursors.Default;
        }

        private void _toolStripMenuItem_Print_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            UltraGrid grid = (UltraGrid)((ContextMenuStrip)((ToolStripItem)sender).Owner).SourceControl;

            printGrid(grid);

            Cursor = Cursors.Default;
        }

        private void _toolStripMenuItem_Save_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            UltraGrid grid = (UltraGrid)((ContextMenuStrip)((ToolStripItem)sender).Owner).SourceControl;

            saveGrid(grid);

            Cursor = Cursors.Default;
        }

        #endregion

        private void _linkLabel_Status_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_linkLabel_Status.Text == Status_InProgress)
            {
                if (m_Snapshot != null)
                {
                    Sql.RegisteredServer rServer = Program.gController.Repository.GetServer(m_Snapshot.ConnectionName);
                    if (rServer != null)
                    {
                        rServer.ShowDataCollectionProgress();
                    }
                }
            }
            else if (_linkLabel_Status.Text == Status_Warnings)
            {
                if (m_Snapshot != null)
                {
                    UltraToolTipInfo tooltip = new UltraToolTipInfo(Sql.Snapshot.GetFormattedWarnings(m_Snapshot.SnapshotComment),
                                 ToolTipImage.Warning,
                                @"Snapshot Warnings",
                                Infragistics.Win.DefaultableBoolean.False);
                    _ultraToolTipManager.SetUltraToolTip(_linkLabel_Status, tooltip);

                    _ultraToolTipManager.ShowToolTip(_linkLabel_Status);
                }
            }
        }

        #endregion

    }
}
