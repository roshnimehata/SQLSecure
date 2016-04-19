using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;

using Idera.SQLsecure.Core.Accounts;
using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Utility;

using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinDataSource;
using Infragistics.Win.UltraWinGrid;
using View=Idera.SQLsecure.UI.Console.Utility.View;

namespace Idera.SQLsecure.UI.Console.Controls
{
    public partial class RolePermissions : UserControl, Interfaces.IView, Interfaces.ICommandHandler
    {
        #region IView

        void Interfaces.IView.SetContext(Interfaces.IDataContext contextIn)
        {
            m_serverInstance = ((Data.PermissionExplorer)contextIn).ServerInstance;

            setMenuConfiguration();

            if (m_serverInstance == null || ! Sql.RegisteredServer.IsServerRegistered(m_serverInstance.ConnectionName))
            {
                _groupBox_SelectRole.Enabled =
                    _groupBox_Database.Enabled =
                    _label_Run.Enabled = false;
                _pictureBox_Snapshot.Image = null;
                m_snapshotId = 0;
                _linkLabel_Snapshot.Text = Utility.ErrorMsgs.ServerNoSnapshots;
                _linkLabel_Snapshot.LinkArea = new LinkArea(0, 0);
                MsgBox.ShowWarning(ErrorMsgs.ObjectExplorerCaption, ErrorMsgs.ServerNotRegistered);
            }
            else
            {
                _groupBox_SelectRole.Enabled =
                    _groupBox_Database.Enabled =
                    _label_Run.Enabled = true;

                // if the user clicked off of the server and then back on it
                // there will be no snapshot passed in from the tree, so
                // make sure that it resets to the same snapshot that was shown
                // or else we end up using the snapshot from the other server
                if (m_serverInstance_shown != null && m_serverInstance_shown.ConnectionName == m_serverInstance.ConnectionName &&
                    ((Data.PermissionExplorer)contextIn).SnapShotId == 0 &&
                    m_snapshotId_shown_isValid)
                {
                    setSnapshot(m_snapshotId_shown);
                }
                else
                {
                    setSnapshot(((Data.PermissionExplorer)contextIn).SnapShotId);
                }
            }

            checkSelections();

            //// If a good user context is passed externally, 
            ////  go ahead and process it and reload if it is different
            //if (m_serverInstance != null &&
            //    m_snapshotId > 0 &&
            //    ((Data.PermissionExplorer)contextIn).User != null)
            //{
            //    _textBox_Role.Text = ((Data.PermissionExplorer)contextIn).User.Name;
            //    // DO NOT put this before the textBox update because it clears m_user in the TextChanged event
            //    m_user = ((Data.PermissionExplorer)contextIn).User;

            //    // If user context has Database us it
            //    if (((Data.PermissionExplorer)contextIn).DatabaseName != null)
            //    {
            //        m_database = ((Data.PermissionExplorer)contextIn).DatabaseName;
            //    }

            //    // Check and make sure the current database selection is valid, otherwise clear it
            //    if (m_database.Length > 0)
            //    {
            //        if (verifyDatabase(ref m_database))
            //        {
            //            // Fix the database name for any case insensitive server difference
            //            _textBox_Database.Text = m_database;
            //        }
            //        else
            //        {
            //            m_database =
            //                _textBox_Database.Text = String.Empty;
            //        }
            //    }
            //    else
            //    {
            //        m_database = _textBox_Database.Text = String.Empty;
            //    }

            //    if (!checkSelections())
            //    {
            //        this.Refresh();
            //        loadDataSource();

            //        // loadDataSource will set the cursor to Wait and leave it there, so it must be reset
            //        Cursor = Cursors.Default;
            //    }
            //}
        }
        String Interfaces.IView.HelpTopic
        {
            get { return Utility.Help.RolePermissionsHelpTopic; }
        }
        String Interfaces.IView.ConceptTopic
        {
            get { return Utility.Help.RolePermissionsConceptTopic; }
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
                    Debug.Assert(false, "Unknown command passed to RolePermissions");
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
            logX.loggerX.Error("Error - RolePermissions showBaseline command called erroneously");
        }

        protected virtual void showCollect()
        {
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - RolePermissions showCollect command called erroneously");
        }

        protected virtual void showConfigure()
        {
            Forms.Form_SqlServerProperties.Process(m_serverInstance.ConnectionName, Forms.Form_SqlServerProperties.RequestedOperation.EditCofiguration, Program.gController.isAdmin);
        }

        protected virtual void showDelete()
        {
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - RolePermissions showDelete command called erroneously");
        }

        protected virtual void showProperties()
        {
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - RolePermissions showProperties command called erroneously");
        }

        protected virtual void showRefresh()
        {
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - RolePermissions showRefresh command called erroneously");
        }

        protected virtual void showPermissions(Views.View_PermissionExplorer.Tab tabIn)
        {
            Program.gController.ShowRootView(new Utility.NodeTag(new Data.PermissionExplorer(m_serverInstance, m_snapshotId, tabIn),
                                                        Utility.View.PermissionExplorer));
        }

        #endregion

        #region fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Controls.RolePermissions");
        private Utility.MenuConfiguration m_menuConfiguration;

        private Sql.RegisteredServer m_serverInstance;
        private int m_snapshotId = 0;
        private string m_snapshotName;
        private DateTime m_snapshotTime;
        private string m_loginType = @"R";
        private string m_role;
        private string m_database = string.Empty;
        private int m_dbid = 0;
        private bool m_warning = false;

        private bool m_showRaw = false;

        private Sql.RegisteredServer m_serverInstance_shown;
        private int m_snapshotId_shown = 0;
        private bool m_snapshotId_shown_isValid = true;
        private string m_snapshotName_shown;
        private DateTime m_snapshotTime_shown;
        private string m_loginType_shown = string.Empty;
        private string m_role_shown = null;
        private string m_database_shown = string.Empty;

        private UltraGrid m_currentGrid = null;
        
        //private bool m_gridCellClicked = false;

        #endregion

        #region Ctors

        public RolePermissions()
        {
            InitializeComponent();
 
            // Initialize base class fields.
            m_menuConfiguration = new Utility.MenuConfiguration();

            // Hookup all application images
            _toolStripButton_RoleRolesColumnChooser.Image =
                _toolStripButton_RoleMembersColumnChooser.Image =
                _toolStripButton_ExplicitServerColumnChooser.Image =
                _toolStripButton_ExplicitDatabaseColumnChooser.Image =
                _toolStripButton_EffectiveServerColumnChooser.Image =
                _toolStripButton_EffectiveDatabaseColumnChooser.Image = AppIcons.AppImage16(AppIcons.Enum.GridFieldChooser);

            _toolStripButton_RoleRolesGroupBy.Image =
                _toolStripButton_RoleMembersGroupBy.Image = 
                _toolStripButton_ExplicitServerGroupBy.Image =
                _toolStripButton_ExplicitDatabaseGroupBy.Image =
                _toolStripButton_EffectiveServerGroupBy.Image =
                _toolStripButton_EffectiveDatabaseGroupBy.Image = AppIcons.AppImage16(AppIcons.Enum.GridGroupBy);

            _toolStripButton_RoleRolesSave.Image =
                _toolStripButton_RoleMembersSave.Image =
                _toolStripButton_ExplicitServerSave.Image =
                _toolStripButton_ExplicitDatabaseSave.Image =
                _toolStripButton_EffectiveServerSave.Image =
                _toolStripButton_EffectiveDatabaseSave.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);

            _toolStripButton_RoleRolesPrint.Image =
                _toolStripButton_RoleMembersPrint.Image =
                _toolStripButton_ExplicitServerPrint.Image =
                _toolStripButton_ExplicitDatabasePrint.Image =
                _toolStripButton_EffectiveServerPrint.Image =
                _toolStripButton_EffectiveDatabasePrint.Image = AppIcons.AppImage16(AppIcons.Enum.Print);

            _ultraTabControl.ImageList = AppIcons.AppImageList16();
            _ultraTabControl.Tabs["_tab_Summary"].Appearance.Image = (int)AppIcons.Enum.Summary;
            _ultraTabControl.Tabs["_tab_Explicit"].Appearance.Image = (int)AppIcons.Enum.AssignedPermissions;
            _ultraTabControl.Tabs["_tab_Effective"].Appearance.Image = (int)AppIcons.Enum.EffectivePermissions;

            _toolStripButton_ShowSelections.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.TaskHide_161;

            _toolStripMenuItem_viewGroupMembers.Image = AppIcons.AppImage16(AppIcons.Enum.WindowsGroup);
            _toolStripMenuItem_showPermissions.Image = AppIcons.AppImage16(AppIcons.Enum.WindowsUser);
            //_toolStripMenuItem_viewGroupByBox.Image - uses a checked value/image instead of app image
            _toolStripMenuItem_Save.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);
            _toolStripMenuItem_Print.Image = AppIcons.AppImage16(AppIcons.Enum.Print);

            // Make sure the first tab is shown on entry
            _ultraTabControl.SelectedTab = _ultraTabControl.Tabs["_tab_Summary"];
            m_currentGrid = _grid_RoleRoles;

            // TODO: remove split container and all server level grids and processing
            _splitContainer_Explicit.Panel1Collapsed =
                _splitContainer_Effective.Panel1Collapsed = true;

            initDataSource();

            _label_Effective_Warning.Top = 58;
            _label_Context.Text = _label_Effective_Warning.Text = ErrorMsgs.NoRolePermissionsShown;
            _label_Warning.Text = String.Empty;
            _linkLabel_SnapshotProperties.Enabled = false;

            // hook the toolbar labels to the grids so the heading can be used for printing
            _grid_RoleRoles.Tag = _label_RoleRoles;
            _grid_RoleMembers.Tag = _label_RoleMembers;
            _grid_ExplicitServer.Tag = _label_ExplicitServer;
            _grid_ExplicitDatabase.Tag = _label_ExplicitDatabase;
            _grid_EffectiveServer.Tag = _label_EffectiveServer;
            _grid_EffectiveDatabase.Tag = _label_ExplicitDatabase;

            // hook the grids to the toolbars so they can be used for button processing
            _headerStrip_RoleRoles.Tag = _grid_RoleRoles;
            _headerStrip_RoleMembers.Tag = _grid_RoleMembers;
            _headerStrip_ExplicitServer.Tag = _grid_ExplicitServer;
            _headerStrip_ExplicitDatabase.Tag = _grid_ExplicitDatabase;
            _headerStrip_EffectiveServer.Tag = _grid_EffectiveServer;
            _headerStrip_EffectiveDatabase.Tag = _grid_EffectiveDatabase;

            // set all grids to start in the same initial display mode
            _grid_RoleRoles.DisplayLayout.GroupByBox.Hidden = 
                _grid_RoleMembers.DisplayLayout.GroupByBox.Hidden = 
                _grid_ExplicitServer.DisplayLayout.GroupByBox.Hidden = 
                _grid_ExplicitDatabase.DisplayLayout.GroupByBox.Hidden = 
                _grid_EffectiveServer.DisplayLayout.GroupByBox.Hidden =
                _grid_EffectiveDatabase.DisplayLayout.GroupByBox.Hidden = Utility.Constants.InitialState_GroupByBoxHidden;

            // Hide the focus rectangles on tabs and grids
            _ultraTabControl.DrawFilter = new HideFocusRectangleDrawFilter();
            _grid_RoleRoles.DrawFilter = new HideFocusRectangleDrawFilter();
            _grid_RoleMembers.DrawFilter = new HideFocusRectangleDrawFilter();
            _grid_ExplicitServer.DrawFilter = new HideFocusRectangleDrawFilter();
            _grid_ExplicitDatabase.DrawFilter = new HideFocusRectangleDrawFilter();
            _grid_EffectiveServer.DrawFilter = new HideFocusRectangleDrawFilter();
            _grid_EffectiveDatabase.DrawFilter = new HideFocusRectangleDrawFilter();
        }

        #endregion

        #region Query, Columns & Constants

        private const string QueryGetDatabaseRoles =
                    @"SQLsecure.dbo.isp_sqlsecure_getdatabaseuserroles";
        private const string QueryGetDatabase =
                    @"SELECT databasename, dbid FROM SQLsecure.dbo.vwdatabases WHERE snapshotid = {0} and lower(databasename) = lower('{1}')";

        private const string QueryGetPermissions =
                    @"SQLsecure.dbo.isp_sqlsecure_getuserpermission";
        private const string ParamGetPermissionsSnapshotId = @"@snapshotid";
        private const string ParamGetPermissionsLoginType = @"@logintype";
        private const string ParamGetPermissionsSid = @"@inputsid";
        private const string ParamGetPermissionsSqlLogin = @"@sqllogin";
        private const string ParamGetPermissionsDatabase = @"@databasename";
        private const string ParamGetPermissionsType = @"@permissiontype";

        private const string ParamGetRolesDbId = @"@dbid";
        private const string ParamGetRolesRoleId = @"@inputuid";
        private const string ParamGetRolesIdOnly = @"@idonly";

        private static string FilterPermissionLevelIsServer = colPermissionLevel + @" = '" + Utility.Permissions.Level.Server + "'";
        private static string FilterPermissionLevelNotServer = colPermissionLevel + @" NOT = '" + Utility.Permissions.Level.Server + "'";

        // Columns for handling query results
        private const string colSnapshotId = @"snapshotid";
        private const string colPermissionLevel = @"permissionlevel";
        private const string colDatabaseName = @"databasename";
        private const string colDatabaseId = @"dbid";

        // Common columns
        private const string colIcon = @"Icon";

        private static string colIconHdr = String.Empty;

        // Server Login columns

        // Role and Role Member columns
        private const string colRoleName = @"name";
        private const string colRoleOwner = @"owner";
        private const string colRoleType = @"type";
        private const string colLogin = @"login";

        private const string colRoleNameHdr = @"Name";
        private const string colRoleOwnerHdr = @"Owner";
        private const string colRoleTypeHdr = @"Type";
        private const string colLoginHdr = @"Login";

        // Permissions columns
        private const string bandPermissions = @"Permissions";
        private const string bandSources = @"Sources";
        private const string colSources = @"Assigned Permissions";

        private const string colGroupName = @"groupname";
        private const string colObjectId = @"objectid";
        private const string colObjectName = @"qualifiedname";
        private const string colObjectType = @"objecttype";
        private const string colObjectTypeName = @"objecttypename";
        private const string colPermission = @"permission";
        private const string colIsGrant = @"isgrant";
        private const string colIsGrantCheckBox = @"isgrantcheckbox";
        private const string colIsGrantWith = @"isgrantwith";
        private const string colIsGrantWithCheckBox = @"isgrantwithcheckbox";
        private const string colIsDeny = @"isdeny";
        private const string colIsDenyCheckBox = @"isdenycheckbox";
        private const string colSchemaName = @"schemaname";
        private const string colOwnerName = @"ownername";
        private const string colSourcePermission = @"sourcepermission";
        private const string colGranteeName = @"granteename";
        private const string colGrantorName = @"grantorname";
        private const string colIsAliased = @"isaliased";
        private const string colIsAliasedCheckBox = @"colIsAliasedcheckbox";
        private const string colInherited = @"inherited";
        private const string colSourceName = @"sourcename";
        private const string colSourceType = @"sourcetype";
        private const string colSourceTypeName = @"sourcetypename";

        private const string colObjectNameHdr = @"Object Name";
        private const string colObjectTypeNameHdr = @"Type";
        private const string colPermissionHdr = @"Permission";
        private const string colIsGrantHdr = @"Grant";
        private const string colIsGrantWithHdr = @"With Grant";
        private const string colIsDenyHdr = @"Deny";
        private const string colGranteeNameHdr = @"Grantee";
        private const string colGrantorNameHdr = @"Grantor";
        private const string colOwnerNameHdr = @"Owner";
        private const string colIsAliasedHdr = @"Aliased";
        private const string colSourcePermissionHdr = @"Source Permission";
        private const string colSourceObjectTypeHdr = @"Source Type";
        private const string colSourceObjectNameHdr = @"Source Name";

        //Other Constants
        private const string HeaderRoleRoles = @"Roles";
        private const string HeaderRoleMembers = @"Role Members";
        private const string HeaderServerPermissions = @"Server Permissions";
        private const string HeaderDatabasePermissions = @"Database Permissions";

        private const string HeaderDisplay = "{0} ({1} items)";
        private const string SnapshotDisplay = "Snapshot: {0}";
        private const string SnapshotProperties = "Snapshot Properties";
        private const string PrintHeaderDisplay = "Permissions for {0} on {1} audited on {2}\n\n{3}";
        private const string PrintPermissionsHeaderDisplay = "{0} - {1}";
        private const string PrintEmptyHeaderDisplay = "{0}";
        private const string PermissionsContextDisplay = "Permissions for {0} in {1}\nfrom snapshot taken at {2}";
        private const string PermissionsStatusDisplay = "Permissions for {0} in {1} at {2}";
        private const string MatchedRoleToolTip = @"This is the selected Role";
        private const string UnknownValue = @"Unknown";
        private const string NoRecordsValue = "No {0} found";
        private const string NoDatabaseRecordsValue = NoRecordsValue + " or no data collected";
        private const string ValueSeparator = @", ";                //used for both display and query values
        private const string DottedNameDisplay = "{0}.{1}";
        private const string EffectiveWarningInitial = @"Choose a database and role";
        private const string EffectiveWarning = @"Please note that calculating Effective Permissions can take several minutes";
        private const string SelectionsShow = @"Show Selections";
        private const string SelectionsHide = @"Hide Selections";
        private const string GridCheckBoxTrue = @"True";

        private const string StatusValidate = @"Validating Selections";
        private const string StatusGetLogins = @"Gathering Logins";
        private const string StatusGetPermissions = @"Gathering Permissions";
        private const string StatusFillPermissions = @"Processing Permissions";

        #endregion

        #region Helpers

        private void initDataSource()
        {
            //set the grid datasource to an empty table and initialize the grids with column headers
            //but no data and fix the header displays to match

            // Initialize Roles grid
            DataTable dt = createDataSourceRoles();
            initGrid(_grid_RoleRoles, dt.DefaultView);
            _label_RoleRoles.Text = HeaderRoleRoles;

            // Initialize Members grid
            dt = createDataSourceMembers();
            initGrid(_grid_RoleMembers, dt.DefaultView);
            _label_RoleMembers.Text = HeaderRoleMembers;

            // Initialize Explicit Permissions grids
            initDataSourceExplicit();

            // Initialize Effective Permissions grids
            initDataSourceEffective();
        }

        private DataTable createDataSourceRoles()
        {
            // Create Explicit Permissions datasource
            DataTable dt = new DataTable();
            dt.Columns.Add(colIcon, typeof(Image));
            dt.Columns.Add(colRoleName, typeof(string));
            dt.Columns.Add(colRoleType, typeof(string));
            dt.Columns.Add(colRoleOwner, typeof(string));

            return dt;
        }

        private DataTable createDataSourceMembers()
        {
            // Create Explicit Permissions datasource
            DataTable dt = new DataTable();
            dt.Columns.Add(colIcon, typeof(Image));
            dt.Columns.Add(colRoleName, typeof(string));
            dt.Columns.Add(colRoleType, typeof(string));
            dt.Columns.Add(colLogin, typeof(string));

            return dt;
        }

        private DataTable createDataSourceExplicit()
        {
            // Create Explicit Permissions datasource
            DataTable dt = new DataTable();
            dt.Columns.Add(colIcon, typeof(Image));
            dt.Columns.Add(colPermissionLevel, typeof(string));
            dt.Columns.Add(colObjectName, typeof(string));
            dt.Columns.Add(colObjectType, typeof(string));
            dt.Columns.Add(colObjectTypeName, typeof(string));
            dt.Columns.Add(colPermission, typeof(string));
            dt.Columns.Add(colGranteeName, typeof(string));
            dt.Columns.Add(colIsGrant, typeof(string));
            dt.Columns.Add(colIsGrantWith, typeof(string));
            dt.Columns.Add(colIsDeny, typeof(string));
            dt.Columns.Add(colGrantorName, typeof(string));
            dt.Columns.Add(colOwnerName, typeof(string));
            dt.Columns.Add(colIsAliased, typeof(string));
            dt.Columns.Add(colIsGrantCheckBox, typeof(bool));
            dt.Columns.Add(colIsGrantWithCheckBox, typeof(bool));
            dt.Columns.Add(colIsDenyCheckBox, typeof(bool));
            dt.Columns.Add(colIsAliasedCheckBox, typeof(bool));

            return dt;
        }

        private void initDataSourceExplicit()
        {
            // Initialize Effective Permissions grids
            DataTable dt = createDataSourceExplicit();

            _label_ExplicitServer.Text = HeaderServerPermissions;
            initGrid(_grid_ExplicitServer, dt.DefaultView);
            _label_ExplicitDatabase.Text = HeaderDatabasePermissions;
            initGrid(_grid_ExplicitDatabase, dt.DefaultView);
        }

        private DataSet createDataSourceEffective()
        {
            //Create datasource for Effective Grids
            DataSet ds = new DataSet();
            ds.Tables.Add(bandPermissions);
            DataTable dt = ds.Tables[bandPermissions];
            dt.Columns.Add(colIcon, typeof(Image));
            dt.Columns.Add(colPermissionLevel, typeof(string));
            dt.Columns.Add(colObjectId, typeof(int));
            dt.Columns.Add(colGroupName, typeof(string));
            dt.Columns.Add(colObjectName, typeof(string));
            dt.Columns.Add(colObjectType, typeof(string));
            dt.Columns.Add(colObjectTypeName, typeof(string));
            dt.Columns.Add(colPermission, typeof(string));
            dt.Columns.Add(colIsGrant, typeof(string));
            dt.Columns.Add(colIsGrantCheckBox, typeof(bool));
            dt.Columns.Add(colIsGrantWith, typeof(string));
            dt.Columns.Add(colIsGrantWithCheckBox, typeof(bool));
            dt.Columns.Add(colIsDeny, typeof(string));
            dt.Columns.Add(colIsDenyCheckBox, typeof(bool));
            dt.Columns.Add(colOwnerName, typeof(string));

            ds.Tables.Add(bandSources);
            dt = ds.Tables[bandSources];

            dt.Columns.Add(colIcon, typeof(Image));
            dt.Columns.Add(colPermissionLevel, typeof(string));
            dt.Columns.Add(colObjectId, typeof(int));
            dt.Columns.Add(colPermission, typeof(string));
            dt.Columns.Add(colSourceName, typeof(string));
            dt.Columns.Add(colSourceType, typeof(string));
            dt.Columns.Add(colSourceTypeName, typeof(string));
            dt.Columns.Add(colSourcePermission, typeof(string));
            dt.Columns.Add(colIsGrant, typeof(string));
            dt.Columns.Add(colIsGrantCheckBox, typeof(bool));
            dt.Columns.Add(colIsGrantWith, typeof(string));
            dt.Columns.Add(colIsGrantWithCheckBox, typeof(bool));
            dt.Columns.Add(colIsDeny, typeof(string));
            dt.Columns.Add(colIsDenyCheckBox, typeof(bool));
            dt.Columns.Add(colGranteeName, typeof(string));
            dt.Columns.Add(colIsAliased, typeof(string));
            dt.Columns.Add(colIsAliasedCheckBox, typeof(bool));
            dt.Columns.Add(colGrantorName, typeof(string));
            dt.Columns.Add(colInherited, typeof(string));

            // Note the name of the relationship shows up in the column chooser
            ds.Relations.Add(colSources,
                new DataColumn[] {ds.Tables[bandPermissions].Columns[colPermissionLevel],
                                    ds.Tables[bandPermissions].Columns[colObjectId],
                                    ds.Tables[bandPermissions].Columns[colPermission]
                                    },
                new DataColumn[] {ds.Tables[bandSources].Columns[colPermissionLevel],
                                    ds.Tables[bandSources].Columns[colObjectId],
                                    ds.Tables[bandSources].Columns[colPermission],
                                    });

            return ds;
        }

        private void initDataSourceEffective()
        {
            // Initialize Effective Permissions grids
            DataSet ds = createDataSourceEffective();

            _label_EffectiveServer.Text = HeaderServerPermissions;
            initGrid(_grid_EffectiveServer, ds);
            _label_EffectiveDatabase.Text = HeaderDatabasePermissions;
            initGrid(_grid_EffectiveDatabase, ds);

            _groupBox_EffectiveCalculate.Visible = false;
            _splitContainer_Effective.Visible = true;
        }

        private void initGrid(Infragistics.Win.UltraWinGrid.UltraGrid grid, Object datasource)
        {
            grid.BeginUpdate();
            //foreach (UltraGridBand band in grid.DisplayLayout.Bands)
            //{
            //    band.Columns.ClearUnbound();  //clear old blank column
            //}
            grid.SetDataBinding(datasource, null);
            //foreach (UltraGridBand band in grid.DisplayLayout.Bands)
            //{
            //    band.Columns.Add();  //add a blank column at the end to fill the display
            //}
            grid.EndUpdate();
        }

        private void loadDataSource()
        {
            bool isCancel = false;

            //Initialize StatusBar
            string saveStatus = _label_Status.Text;
            updateStatusBar(0, StatusValidate);

            checkSelections();

            // validate database
            if (m_database.Length > 0)
            {
                string database = m_database;
                int dbid = m_dbid;

                if (verifyDatabase(ref database, ref dbid))
                {
                    if (m_serverInstance.CaseSensitive)
                    {
                        if (!m_database.Equals(database))
                        {
                            if (DialogResult.No == MsgBox.ShowWarningConfirm(Utility.ErrorMsgs.RolePermissionsCaption,
                                                                 String.Format(Utility.ErrorMsgs.PartialMatchDatabaseMsg, database)))
                            {
                                finishStatusBar(saveStatus);
                                return;
                            }
                        }
                    }
                    m_database =
                        _textBox_Database.Text = database;
                    m_dbid = dbid;
                }
                else
                {
                    MsgBox.ShowError(Utility.ErrorMsgs.RolePermissionsCaption, string.Format(Utility.ErrorMsgs.CantFindDatabaseMsg, m_database));
                    finishStatusBar(saveStatus);
                    return;
                }
            }
            else
            {
                MsgBox.ShowError(Utility.ErrorMsgs.RolePermissionsCaption, Utility.ErrorMsgs.NoDatabaseMsg);
                finishStatusBar(saveStatus);
                return;
            }
            incrementStatusBar(5);

            // Validate Role
            Sql.DatabasePrincipal role = null;
            if (m_role.Trim().Length == 0)
            {
                MsgBox.ShowError(Utility.ErrorMsgs.RolePermissionsCaption, Utility.ErrorMsgs.NoRoleSelectedMsg);
                finishStatusBar(saveStatus);
                return;
            }
            else
            {
                role = Sql.DatabasePrincipal.GetSnapshotDbRole(m_snapshotId, m_dbid, m_role, m_serverInstance.CaseSensitive);
                if (role == null)
                {
                    MsgBox.ShowError(Utility.ErrorMsgs.RolePermissionsCaption, string.Format(Utility.ErrorMsgs.RoleNotFoundMsg, m_role, m_database));
                    finishStatusBar(saveStatus);
                    return;
                }
            }

            // disable the button so it isn't clicked again while processing
            _button_ShowPermissions.Enabled = false;
            incrementStatusBar(5);

            try
            {
                _pictureBox_PermissionsWarning.Image = null;
                _label_Warning.Text = String.Empty;

                // Open connection to repository and query permissions.
                logX.loggerX.Info("Retrieve Role Permissions");

                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Setup permissions params.
                    SqlParameter paramSnapshotId = new SqlParameter(ParamGetPermissionsSnapshotId, m_snapshotId);
                    SqlParameter paramLoginType = new SqlParameter(ParamGetPermissionsLoginType, m_loginType);
                    SqlParameter paramSid = new SqlParameter(ParamGetPermissionsSid, new byte[] { 0 });
                    SqlParameter paramSqlLogin = new SqlParameter(ParamGetPermissionsSqlLogin, m_role);
                    SqlParameter paramDatabase = new SqlParameter(ParamGetPermissionsDatabase, m_database);
                    SqlParameter paramPermissionType = new SqlParameter(ParamGetPermissionsType, Permissions.Type.Explicit);

                    SqlParameter paramDbId = new SqlParameter(ParamGetRolesDbId, m_dbid);
                    SqlParameter paramRoleId = new SqlParameter(ParamGetRolesRoleId, role.Id);
                    SqlParameter paramGetRolesIdOnly = new SqlParameter(ParamGetRolesIdOnly, 0);
                    paramGetRolesIdOnly.Value = 0;

                    //Get Roles
                    logX.loggerX.Info("Retrieve Database Roles");
                    _label_Status.Text = StatusGetLogins;
                    _statusStrip.Refresh();
                    SqlCommand cmd = new SqlCommand(QueryGetDatabaseRoles, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(paramSnapshotId);
                    cmd.Parameters.Add(paramDbId);
                    cmd.Parameters.Add(paramRoleId);
                    cmd.Parameters.Add(paramGetRolesIdOnly);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    incrementStatusBar(5);

                    DataView dv = new DataView(ds.Tables[0]);
                    //Note, the unique option doesn't work when a Sid is returned as a data column
                    //Create a table with the unique logins in it
                    DataTable dt = dv.ToTable(true, new string[] { colRoleName, 
                                                                    colRoleOwner,
                                                                    colRoleType,
                    }
                                                );
                    //Add the Icon column for manual processing
                    dt.Columns.Add(colIcon, typeof(Image));
                    dt.Columns[colIcon].SetOrdinal(0);

                    foreach (DataRow dr in dt.Rows)
                    {
                        //Set the display value for the icon, checkboxes and roles
                        switch (dr[colRoleType].ToString())
                        {
                            case Sql.DatabasePrincipalTypes.ApplicationRole:
                                dr[colIcon] = Sql.ObjectType.TypeImage16(Sql.ObjectType.TypeEnum.ApplicationRole);
                                dr[colRoleType] = Sql.DatabasePrincipalTypes.ApplicationRoleText;
                                break;
                            case Sql.DatabasePrincipalTypes.DatabaseRole:
                                dr[colIcon] = Sql.ObjectType.TypeImage16(Sql.ObjectType.TypeEnum.DatabaseRole);
                                dr[colRoleType] = Sql.DatabasePrincipalTypes.DatabaseRoleText;
                                break;
                            default:
                                logX.loggerX.Warn("Unknown Role Type encountered in Roles data");
                                dr[colIcon] = Sql.ObjectType.TypeImage16(Sql.ObjectType.TypeEnum.Unknown);
                                dr[colRoleType] = UnknownValue;
                                break;
                        }
                    }

                    //set the view to point to the new table so it can be sorted
                    dv = dt.DefaultView;
                    dv.Sort = colRoleName;

                    //Show the record count in the header, and stuff a record in the grid to say none returned
                    _label_RoleRoles.Text = string.Format(HeaderDisplay, HeaderRoleRoles, dv.Count.ToString());
                    if (dv.Count == 0)
                    {
                        //The booleans must be filled or the checkboxes print as checked!!!!
                        dt.Rows.Add(null,
                                    string.Format(NoRecordsValue, HeaderRoleRoles),
                                    null
                                    );
                    }

                    initGrid(_grid_RoleRoles, dv);
                    incrementStatusBar(5);

                    cmd.Parameters.Clear();     // clear the parameters to allow them to be used again


                    //Get Role Members
                    logX.loggerX.Info("Retrieve Database Role Members");
                    List<Sql.DatabasePrincipal> roles = Sql.DatabasePrincipal.GetSnapshotDbRoleMembers(m_snapshotId, m_dbid, role.Id);
                    incrementStatusBar(5);

                    //Create a table with the unique roles in it
                    dt = createDataSourceMembers();

                    foreach (Sql.DatabasePrincipal dbrole in roles)
                    {
                        DataRow dr = dt.NewRow();
                        dr[colIcon] = Sql.ObjectType.TypeImage16(dbrole.TypeEnum);
                        dr[colRoleName] = dbrole.Name;
                        dr[colRoleType] = dbrole.TypeStr;
                        dr[colLogin] = dbrole.Login;
                        dt.Rows.Add(dr);
                    }

                    //set the view to point to the new table so it can be sorted
                    dv = dt.DefaultView;
                    dv.Sort = colRoleName;
 
                    //Show the record count in the header, and stuff a record in the grid to say none returned
                    _label_RoleMembers.Text = string.Format(HeaderDisplay, HeaderRoleMembers, dv.Count.ToString());
                    if (dv.Count == 0)
                    {
                        //The booleans must be filled or the checkboxes print as checked!!!!
                        dt.Rows.Add(null,
                                    string.Format(NoRecordsValue, HeaderRoleMembers)
                                   );
                    }

                    initGrid(_grid_RoleMembers, dv);
                    incrementStatusBar(5);

                    cmd.Parameters.Clear();     // clear the parameters to allow them to be used again


                    // Get Explicit Permissions
                    logX.loggerX.Info("Retrieve Explicit Permissions");
                    _label_Status.Text = StatusGetPermissions;
                    _statusStrip.Refresh();
                    cmd = new SqlCommand(QueryGetPermissions, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                    cmd.Parameters.Add(paramSnapshotId);
                    cmd.Parameters.Add(paramLoginType);
                    cmd.Parameters.Add(paramSid);
                    cmd.Parameters.Add(paramSqlLogin);
                    cmd.Parameters.Add(paramDatabase);
                    cmd.Parameters.Add(paramPermissionType);

                    da = new SqlDataAdapter(cmd);
                    ds = new DataSet();
                    da.Fill(ds);
                    incrementStatusBar(25, StatusFillPermissions);

                    // Create a new datatable to be the datasource
                    dt = createDataSourceExplicit();

                    DataRow drlast = dt.NewRow();       // Initialize for first compare
                    DataRow drnew;

                    ds.Tables[0].DefaultView.Sort = colPermissionLevel + ValueSeparator
                                                    + colObjectTypeName + ValueSeparator
                                                    + colObjectName + ValueSeparator
                                                    + colPermission + ValueSeparator
                                                    + colGranteeName + ValueSeparator
                                                    + colIsGrant + ValueSeparator
                                                    + colIsGrantWith + ValueSeparator
                                                    + colIsDeny + ValueSeparator
                                                    + colGrantorName + ValueSeparator
                                                    + colOwnerName + ValueSeparator
                                                    + colIsAliased;

                    foreach (DataRowView drv in ds.Tables[0].DefaultView)
                    {
                        // Create a new datarow and fill it with the retrieved values
                        drnew = dt.NewRow();
                        drnew[colPermissionLevel] = drv[colPermissionLevel];
                        drnew[colObjectName] = drv[colObjectName];
                        drnew[colObjectType] = drv[colObjectType];
                        drnew[colObjectTypeName] = drv[colObjectTypeName];
                        drnew[colPermission] = drv[colPermission];
                        drnew[colGranteeName] = drv[colGranteeName];
                        drnew[colIsGrant] = drv[colIsGrant];
                        drnew[colIsGrantWith] = drv[colIsGrantWith];
                        drnew[colIsDeny] = drv[colIsDeny];
                        drnew[colGrantorName] = drv[colGrantorName];
                        drnew[colOwnerName] = drv[colOwnerName];
                        drnew[colIsAliased] = drv[colIsAliased];

                        // If not a duplicate then fill remaining fields and add to table
                        if (drnew[colPermissionLevel].ToString() != drlast[colPermissionLevel].ToString() ||
                            drnew[colObjectName].ToString() != drlast[colObjectName].ToString() ||
                            drnew[colObjectType].ToString() != drlast[colObjectType].ToString() ||
                            drnew[colPermission].ToString() != drlast[colPermission].ToString() ||
                            drnew[colGranteeName].ToString() != drlast[colGranteeName].ToString() ||
                            drnew[colIsGrant].ToString() != drlast[colIsGrant].ToString() ||
                            drnew[colIsGrantWith].ToString() != drlast[colIsGrantWith].ToString() ||
                            drnew[colIsDeny].ToString() != drlast[colIsDeny].ToString() ||
                            drnew[colGrantorName].ToString() != drlast[colGrantorName].ToString() ||
                            drnew[colOwnerName].ToString() != drlast[colOwnerName].ToString() ||
                            drnew[colIsAliased].ToString() != drlast[colIsAliased].ToString())
                        {
                            drnew[colIcon] = Sql.ObjectType.TypeImage16(Sql.ObjectType.ToTypeEnum(drv[colObjectType].ToString().Trim()));
                            drnew[colIsGrantCheckBox] = (string.Equals(drv[colIsGrant].ToString(), Utility.Permissions.Grants.True, StringComparison.CurrentCultureIgnoreCase));
                            drnew[colIsGrantWithCheckBox] = (string.Equals(drv[colIsGrantWith].ToString(), Utility.Permissions.Grants.True, StringComparison.CurrentCultureIgnoreCase));
                            drnew[colIsDenyCheckBox] = (string.Equals(drv[colIsDeny].ToString(), Utility.Permissions.Grants.True, StringComparison.CurrentCultureIgnoreCase));
                            drnew[colIsAliasedCheckBox] = (string.Equals(drv[colIsAliased].ToString(), Utility.Permissions.Grants.True, StringComparison.CurrentCultureIgnoreCase));

                            dt.Rows.Add(drnew);
                            drlast = drnew;
                        }
                    }

                    dv = new DataView(dt);
                    dv.RowFilter = FilterPermissionLevelIsServer;
                    dv.Sort = colObjectTypeName + ValueSeparator + colObjectName + ValueSeparator + colPermission;

                    //Show the record count in the header, and stuff a record in the grid to say none returned
                    _label_ExplicitServer.Text = string.Format(HeaderDisplay, HeaderServerPermissions, dv.Count.ToString());
                    if (dv.Count == 0)
                    {
                        //The booleans must be initialized or the checkboxes print as checked!!!!
                        dv.AddNew();
                        dv[0][colObjectName] = string.Format(NoRecordsValue, HeaderServerPermissions);
                        dv[0][colIsGrantCheckBox] = false;
                        dv[0][colIsGrantWithCheckBox] = false;
                        dv[0][colIsDenyCheckBox] = false;
                        dv[0][colIsAliasedCheckBox] = false;
                    }

                    //Expand the groups if there is only one record, otherwise leave them closed
                    // this works around the grid persistence of groups problem with one record
                    if (dv.Count == 1)
                    {
                        _grid_ExplicitServer.DisplayLayout.Bands[0].Override.GroupByRowInitialExpansionState = GroupByRowInitialExpansionState.Expanded;
                    }
                    else
                    {
                        _grid_ExplicitServer.DisplayLayout.Bands[0].Override.GroupByRowInitialExpansionState = GroupByRowInitialExpansionState.Collapsed;
                    }

                    initGrid(_grid_ExplicitServer, dv);
                    incrementStatusBar(5);

                    if (dv.Count > 1)
                    {
                        _grid_ExplicitServer.DisplayLayout.Bands[0].SortedColumns.Add(colObjectTypeName, false, true);
                    }
                    else if (dv.Count == 1)
                    {
                        _grid_ExplicitServer.DisplayLayout.Bands[0].SortedColumns.Clear();
                        _grid_ExplicitServer.DisplayLayout.Bands[0].Columns[colObjectName].PerformAutoResize(PerformAutoSizeType.AllRowsInBand, true);
                    }

                    //set the view to contain all database level records
                    dv = new DataView(dt);
                    dv.RowFilter = FilterPermissionLevelNotServer;
                    dv.Sort = colObjectTypeName + ValueSeparator + colObjectName + ValueSeparator + colPermission;

                    //Show the record count in the header
                    _label_ExplicitDatabase.Text = string.Format(HeaderDisplay, HeaderDatabasePermissions, dv.Count.ToString());

                    //Stuff a record in the grid if none returned to let user know
                    if (dv.Count == 0)
                    {
                        //The booleans must be initialized or the checkboxes print as checked!!!!
                        dv.AddNew();
                        dv[0][colObjectName] = string.Format(NoDatabaseRecordsValue, HeaderDatabasePermissions);
                        dv[0][colIsGrantCheckBox] = false;
                        dv[0][colIsGrantWithCheckBox] = false;
                        dv[0][colIsDenyCheckBox] = false;
                        dv[0][colIsAliasedCheckBox] = false;
                    }

                    //Expand the groups if there is only one record, otherwise leave them closed
                    // this works around the grid persistence of groups problem with one record
                    if (dv.Count == 1)
                    {
                        _grid_ExplicitDatabase.DisplayLayout.Bands[0].Override.GroupByRowInitialExpansionState = GroupByRowInitialExpansionState.Expanded;
                    }
                    else
                    {
                        _grid_ExplicitDatabase.DisplayLayout.Bands[0].Override.GroupByRowInitialExpansionState = GroupByRowInitialExpansionState.Collapsed;
                    }

                    initGrid(_grid_ExplicitDatabase, dv);
                    incrementStatusBar(25);

                    //Group by type whenever more than one record is returned
                    if (dv.Count > 1)
                    {
                        _grid_ExplicitDatabase.DisplayLayout.Bands[0].SortedColumns.Add(colObjectTypeName, false, true);
                    }
                    else if (dv.Count == 1)
                    {
                        _grid_ExplicitDatabase.DisplayLayout.Bands[0].SortedColumns.Clear();
                        _grid_ExplicitDatabase.DisplayLayout.Bands[0].Columns[colObjectName].PerformAutoResize(PerformAutoSizeType.AllRowsInBand, true);
                    }


                    // Initialize Effective Permissions grids to clear old data
                    initDataSourceEffective();
                    incrementStatusBar(5);



                    // update the context for comparison to changed selections
                    m_serverInstance_shown = m_serverInstance;
                    m_snapshotId_shown = m_snapshotId;
                    m_snapshotId_shown_isValid = true;
                    m_snapshotName_shown = m_snapshotName;
                    m_snapshotTime_shown = m_snapshotTime;
                    m_loginType_shown = m_loginType;
                    m_role_shown = m_role;
                    m_database_shown = m_database;

                    incrementStatusBar(5);
                    _label_Context.Text = String.Format(PermissionsContextDisplay,
                            m_role_shown,
                            ((m_database_shown.Length > 0) ? string.Format(DottedNameDisplay,
                                                                            m_serverInstance.ConnectionName,
                                                                            m_database_shown)
                                                           : m_serverInstance.ConnectionName),
                            m_snapshotTime_shown.ToString(Utility.Constants.DATETIME_FORMAT));
                    _label_Status.Text = String.Format(PermissionsStatusDisplay,
                            m_role_shown,
                            ((m_database_shown.Length > 0) ? string.Format(DottedNameDisplay,
                                                                            m_serverInstance.ConnectionName,
                                                                            m_database_shown)
                                                           : m_serverInstance.ConnectionName),
                            m_snapshotTime_shown.ToString(Utility.Constants.DATETIME_FORMAT));
                }
                _linkLabel_SnapshotProperties.Enabled = true;
                _label_Effective_Warning.Top = 28;
                _label_Effective_Warning.Text = EffectiveWarning;
                _splitContainer_Effective.Visible = false;
                _groupBox_EffectiveCalculate.Visible =
                    _button_CalculateEffective.Visible =
                    _button_CalculateEffective.Enabled = true;
                _toolStripButton_ShowSelections.Visible = true;

                _ultraTabControl.SelectedTab = _ultraTabControl.Tabs["_tab_Summary"];
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error(@"Error - Unable to retrieve user permissions", ex);
                MsgBox.ShowError(Utility.ErrorMsgs.CantGetUserPermissionsCaption, ErrorMsgs.ErrorProcessUserPermissions, ex);
                initDataSource();
                _linkLabel_SnapshotProperties.Enabled = true;
                _label_Context.Text =
                    _label_Status.Text = ErrorMsgs.ErrorProcessUserPermissions;
                _splitContainer_Effective.Visible = true;
                _groupBox_EffectiveCalculate.Visible =
                    _button_CalculateEffective.Enabled = false;
            }

            checkSelections();
            finishStatusBar();
            _button_ShowPermissions.Enabled = true;
        }

        private void loadEffective()
        {
            _button_ShowPermissions.Enabled =
                _button_CalculateEffective.Enabled = false;

            string saveStatus = _label_Status.Text;
            updateStatusBar(10, StatusGetPermissions);

            try
            {
                // Open connection to repository and query permissions.
                logX.loggerX.Info("Retrieve User Permissions");

                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Setup permissions params using the values already shown
                    SqlParameter paramSnapshotId = new SqlParameter(ParamGetPermissionsSnapshotId, m_snapshotId_shown);
                    SqlParameter paramLoginType = new SqlParameter(ParamGetPermissionsLoginType, m_loginType_shown);
                    SqlParameter paramSid = new SqlParameter(ParamGetPermissionsSid, new byte[] { 0 });
                    SqlParameter paramSqlLogin = new SqlParameter(ParamGetPermissionsSqlLogin, m_role_shown);
                    SqlParameter paramDatabase = new SqlParameter(ParamGetPermissionsDatabase, m_database_shown);
                    SqlParameter paramPermissionType = new SqlParameter(ParamGetPermissionsType, Permissions.Type.Effective);

                    // Get Effective Permissions
                    logX.loggerX.Info("Retrieve Effective Permissions");
                    _label_Status.Text = StatusGetPermissions;
                    _statusStrip.Refresh();
                    SqlCommand cmd = new SqlCommand(QueryGetPermissions, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();        // The permissions take > 30 secs on a 2005 server
                    cmd.Parameters.Add(paramSnapshotId);
                    cmd.Parameters.Add(paramLoginType);
                    cmd.Parameters.Add(paramSid);
                    cmd.Parameters.Add(paramSqlLogin);
                    cmd.Parameters.Add(paramDatabase);
                    cmd.Parameters.Add(paramPermissionType);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    incrementStatusBar(40, StatusFillPermissions);

                    if (m_showRaw)
                    {
                        ds.Tables[0].TableName = bandPermissions;
                        initGrid(_grid_EffectiveServer, ds.Tables[0].DefaultView);
                        incrementStatusBar(20);
                        _label_EffectiveServer.Text = string.Format(HeaderDisplay, HeaderServerPermissions, ds.Tables[0].DefaultView.Count.ToString());

                        initGrid(_grid_EffectiveDatabase, ds.Tables[0]);
                        incrementStatusBar(20);
                        _label_EffectiveDatabase.Text = string.Format(HeaderDisplay, HeaderDatabasePermissions, ds.Tables[0].DefaultView.Count.ToString());
                    }
                    else
                    {
                        // Set sort order for comparison
                        ds.Tables[0].DefaultView.Sort = colPermissionLevel + ValueSeparator
                                                        + colObjectId + ValueSeparator
                                                        + colPermission + ValueSeparator
                                                        + colGranteeName + ValueSeparator
                                                        + colSourcePermission + ValueSeparator
                                                        + colSourceType + ValueSeparator
                                                        + colSourceName + ValueSeparator
                                                        + colIsAliased;

                        // Process the Server data
                        ds.Tables[0].DefaultView.RowFilter = FilterPermissionLevelIsServer;
                        DataSet dsServer = fillEffectiveData(ds.Tables[0].DefaultView);
                        DataTable dtPermissions = dsServer.Tables[bandPermissions];

                        //Show the record count in the header, and stuff a record in the grid to say none returned
                        _label_EffectiveServer.Text = string.Format(HeaderDisplay, HeaderServerPermissions, dtPermissions.DefaultView.Count.ToString());
                        if (dtPermissions.DefaultView.Count == 0)
                        {
                            //The booleans must be initialized or the checkboxes print as checked!!!!
                            DataRow drPermission = dtPermissions.NewRow();
                            drPermission[colGroupName] =
                                drPermission[colObjectName] = string.Format(NoRecordsValue, HeaderServerPermissions);
                            drPermission[colIsGrantCheckBox] =
                                drPermission[colIsGrantWithCheckBox] =
                                drPermission[colIsDenyCheckBox] = false;

                            dtPermissions.Rows.Add(drPermission);
                        }

                        //Expand the groups if there is only one record, otherwise leave them closed
                        // this works around the grid persistence of groups problem with one record
                        if (dtPermissions.DefaultView.Count == 1)
                        {
                            _grid_EffectiveServer.DisplayLayout.Bands[bandPermissions].Override.GroupByRowInitialExpansionState = GroupByRowInitialExpansionState.Expanded;
                        }
                        else
                        {
                            _grid_EffectiveServer.DisplayLayout.Bands[bandPermissions].Override.GroupByRowInitialExpansionState = GroupByRowInitialExpansionState.Collapsed;
                        }

                        initGrid(_grid_EffectiveServer, dsServer);
                        incrementStatusBar(20);

                        if (dtPermissions.DefaultView.Count > 1)
                        {
                            _grid_EffectiveServer.DisplayLayout.Bands[bandPermissions].SortedColumns.Add(colObjectTypeName, false, true);
                            _grid_EffectiveServer.DisplayLayout.Bands[bandPermissions].SortedColumns.Add(colGroupName, false, true);
                        }
                        else
                        {
                            _grid_EffectiveServer.DisplayLayout.Bands[bandPermissions].Columns[colGroupName].Hidden = false;
                            if (dtPermissions.DefaultView.Count == 1)
                            {
                                _grid_EffectiveServer.DisplayLayout.Bands[bandPermissions].SortedColumns.Clear();
                                _grid_EffectiveServer.DisplayLayout.Bands[bandPermissions].Columns[colGroupName].PerformAutoResize(PerformAutoSizeType.AllRowsInBand, true);
                            }
                        }

                        // Process the Database data
                        ds.Tables[0].DefaultView.RowFilter = FilterPermissionLevelNotServer;
                        DataSet dsDatabase = fillEffectiveData(ds.Tables[0].DefaultView);
                        dtPermissions = dsDatabase.Tables[bandPermissions];

                        //Show the record count in the header, and stuff a record in the grid to say none returned
                        _label_EffectiveDatabase.Text = string.Format(HeaderDisplay, HeaderDatabasePermissions, dtPermissions.DefaultView.Count.ToString());
                        if (dtPermissions.DefaultView.Count == 0)
                        {
                            //The booleans must be initialized or the checkboxes print as checked!!!!
                            DataRow drPermission = dtPermissions.NewRow();
                            drPermission[colGroupName] =
                                drPermission[colObjectName] = string.Format(NoDatabaseRecordsValue, HeaderDatabasePermissions);
                            drPermission[colIsGrantCheckBox] =
                                drPermission[colIsGrantWithCheckBox] =
                                drPermission[colIsDenyCheckBox] = false;

                            dtPermissions.Rows.Add(drPermission);
                        }

                        //Expand the groups if there is only one record, otherwise leave them closed
                        // this works around the grid persistence of groups problem with one record
                        if (dtPermissions.DefaultView.Count == 1)
                        {
                            _grid_EffectiveDatabase.DisplayLayout.Bands[bandPermissions].Override.GroupByRowInitialExpansionState = GroupByRowInitialExpansionState.Expanded;
                        }
                        else
                        {
                            _grid_EffectiveDatabase.DisplayLayout.Bands[bandPermissions].Override.GroupByRowInitialExpansionState = GroupByRowInitialExpansionState.Collapsed;
                        }

                        initGrid(_grid_EffectiveDatabase, dsDatabase);
                        incrementStatusBar(20);

                        if (dtPermissions.DefaultView.Count > 1)
                        {
                            _grid_EffectiveDatabase.DisplayLayout.Bands[bandPermissions].SortedColumns.Add(colObjectTypeName, false, true);
                            _grid_EffectiveDatabase.DisplayLayout.Bands[bandPermissions].SortedColumns.Add(colGroupName, false, true);
                        }
                        else
                        {
                            _grid_EffectiveDatabase.DisplayLayout.Bands[bandPermissions].Columns[colGroupName].Hidden = false;
                            if (dtPermissions.DefaultView.Count == 1)
                            {
                                _grid_EffectiveDatabase.DisplayLayout.Bands[bandPermissions].SortedColumns.Clear();
                                _grid_EffectiveDatabase.DisplayLayout.Bands[bandPermissions].Columns[colGroupName].PerformAutoResize(PerformAutoSizeType.AllRowsInBand, true);
                            }
                        }
                    }
                }
                _groupBox_EffectiveCalculate.Visible = false;
                _splitContainer_Effective.Visible = true;
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error(@"Error - Unable to retrieve role permissions", ex);
                MsgBox.ShowError(Utility.ErrorMsgs.CantGetRolePermissionsCaption, ErrorMsgs.ErrorProcessRolePermissions, ex);
                checkSelections();
            }

            m_showRaw = false;
            finishStatusBar(saveStatus);
            _button_ShowPermissions.Enabled =
                _button_CalculateEffective.Enabled = true;
        }

        private DataSet fillEffectiveData(DataView dvSource)
        {
            // Create a new DataSet to be the datasource
            DataSet dsEffective = createDataSourceEffective();
            DataTable dtPermissions = dsEffective.Tables[bandPermissions];
            DataTable dtSources = dsEffective.Tables[bandSources];
            DataRow drLastPermission = dtPermissions.NewRow();  // Initialize for first compare
            DataRow drPermission;
            DataRow drLastSource = dtSources.NewRow();          // Initialize for compare
            DataRow drSource;
            string groupname;

            // Process the incoming data and split into permissions and sources
            foreach (DataRowView drv in dvSource)
            {
                // Create a new datarow and fill it with the retrieved values
                // This is done first before the compare to make sure any conversions done
                // to place values in the new row are the same for comparison
                drPermission = dtPermissions.NewRow();
                drPermission[colPermissionLevel] = drv[colPermissionLevel];
                drPermission[colObjectId] = drv[colObjectId];
                drPermission[colObjectName] = drv[colObjectName];
                drPermission[colObjectType] = drv[colObjectType];
                drPermission[colObjectTypeName] = drv[colObjectTypeName];
                drPermission[colPermission] = drv[colPermission];
                drPermission[colIsGrant] = drv[colIsGrant];
                drPermission[colIsGrantWith] = drv[colIsGrantWith];
                drPermission[colIsDeny] = drv[colIsDeny];
                drPermission[colOwnerName] = drv[colOwnerName];

                if (drv[colPermissionLevel].ToString().Trim() == Utility.Permissions.Level.Object
                    && drv[colSchemaName].ToString().Length > 0)
                {
                    groupname = string.Format(DottedNameDisplay,
                                                drv[colSchemaName],
                                                drPermission[colObjectName].ToString()
                                                );
                }
                else
                {
                    groupname = drPermission[colObjectName].ToString();
                }

                drPermission[colGroupName] = groupname;

                // If the permission is not a duplicate then fill remaining fields and add to table
                if (drPermission[colPermissionLevel].ToString() != drLastPermission[colPermissionLevel].ToString() ||
                    drPermission[colObjectId].ToString() != drLastPermission[colObjectId].ToString() ||
                    drPermission[colPermission].ToString() != drLastPermission[colPermission].ToString()
                    )
                {
                    drPermission[colIcon] = Sql.ObjectType.TypeImage16(Sql.ObjectType.ToTypeEnum(drv[colObjectType].ToString().Trim()));
                    drPermission[colIsGrantCheckBox] = (string.Equals(drv[colIsGrant].ToString(), Utility.Permissions.Grants.True, StringComparison.CurrentCultureIgnoreCase));
                    drPermission[colIsGrantWithCheckBox] = (string.Equals(drv[colIsGrantWith].ToString(), Utility.Permissions.Grants.True, StringComparison.CurrentCultureIgnoreCase));
                    drPermission[colIsDenyCheckBox] = (string.Equals(drv[colIsDeny].ToString(), Utility.Permissions.Grants.True, StringComparison.CurrentCultureIgnoreCase));

                    dtPermissions.Rows.Add(drPermission);
                    drLastPermission = drPermission;

                    drSource = dtSources.NewRow();
                    drSource[colIcon] = Sql.ObjectType.TypeImage16(Sql.ObjectType.ToTypeEnum(drv[colSourceType].ToString().Trim()));
                    drSource[colPermissionLevel] = drv[colPermissionLevel];
                    drSource[colObjectId] = drv[colObjectId];
                    drSource[colPermission] = drv[colPermission];
                    drSource[colSourcePermission] = drv[colSourcePermission];
                    drSource[colIsGrant] = drv[colIsGrant];
                    drSource[colIsGrantWith] = drv[colIsGrantWith];
                    drSource[colIsDeny] = drv[colIsDeny];
                    drSource[colGranteeName] = drv[colGranteeName];
                    drSource[colIsAliased] = drv[colIsAliased];
                    drSource[colGrantorName] = drv[colGrantorName];
                    drSource[colInherited] = drv[colInherited];
                    drSource[colSourceName] = drv[colSourceName];
                    drSource[colSourceType] = drv[colSourceType];
                    drSource[colSourceTypeName] = drv[colSourceTypeName];
                    drSource[colIsGrantCheckBox] = (string.Equals(drv[colIsGrant].ToString(), Utility.Permissions.Grants.True, StringComparison.CurrentCultureIgnoreCase));
                    drSource[colIsGrantWithCheckBox] = (string.Equals(drv[colIsGrantWith].ToString(), Utility.Permissions.Grants.True, StringComparison.CurrentCultureIgnoreCase));
                    drSource[colIsDenyCheckBox] = (string.Equals(drv[colIsDeny].ToString(), Utility.Permissions.Grants.True, StringComparison.CurrentCultureIgnoreCase));
                    drSource[colIsAliasedCheckBox] = (string.Equals(drv[colIsAliased].ToString(), Utility.Permissions.Grants.True, StringComparison.CurrentCultureIgnoreCase));

                    dtSources.Rows.Add(drSource);
                    drLastSource = drSource;
                }
                else
                {
                    // Check and see if the permission grants need to be updated because some
                    // permissions have only the Grant and some also have the GrantWith
                    // There should never be conflicting Grant permissions, but process it just in case
                    if (drLastPermission[colIsGrant].ToString() != Utility.Permissions.Grants.True &&
                        (drPermission[colIsGrant].ToString() == Utility.Permissions.Grants.True
                        || drLastPermission[colIsGrant].ToString() == Utility.Permissions.Grants.True)
                        )
                    {
                        drLastPermission[colIsGrant] = Utility.Permissions.Grants.True;
                        drLastPermission[colIsGrantCheckBox] = true;
                    }
                    if (drLastPermission[colIsGrantWith].ToString() != Utility.Permissions.Grants.True &&
                        (drPermission[colIsGrantWith].ToString() == Utility.Permissions.Grants.True
                        || drLastPermission[colIsGrantWith].ToString() == Utility.Permissions.Grants.True)
                        )
                    {
                        drLastPermission[colIsGrantWith] = Utility.Permissions.Grants.True;
                        drLastPermission[colIsGrantWithCheckBox] = true;
                    }
                    // There should never be conflicting Deny permissions, but process it just in case
                    if (drLastPermission[colIsDeny].ToString() != Utility.Permissions.Grants.True &&
                        (drPermission[colIsDeny].ToString() == Utility.Permissions.Grants.True
                        || drLastPermission[colIsDeny].ToString() == Utility.Permissions.Grants.True)
                        )
                    {
                        drLastPermission[colIsDeny] = Utility.Permissions.Grants.True;
                        drLastPermission[colIsDenyCheckBox] = true;
                    }

                    // Create a new source record for comparison
                    drSource = dtSources.NewRow();
                    drSource[colIcon] = Sql.ObjectType.TypeImage16(Sql.ObjectType.ToTypeEnum(drv[colSourceType].ToString().Trim()));
                    drSource[colPermissionLevel] = drv[colPermissionLevel];
                    drSource[colObjectId] = drv[colObjectId];
                    drSource[colPermission] = drv[colPermission];
                    drSource[colSourcePermission] = drv[colSourcePermission];
                    drSource[colIsGrant] = drv[colIsGrant];
                    drSource[colIsGrantWith] = drv[colIsGrantWith];
                    drSource[colIsDeny] = drv[colIsDeny];
                    drSource[colGranteeName] = drv[colGranteeName];
                    drSource[colIsAliased] = drv[colIsAliased];
                    drSource[colGrantorName] = drv[colGrantorName];
                    drSource[colInherited] = drv[colInherited];
                    drSource[colSourceName] = drv[colSourceName];
                    drSource[colSourceType] = drv[colSourceType];
                    drSource[colSourceTypeName] = drv[colSourceTypeName];
                    drSource[colIsGrantCheckBox] = (string.Equals(drv[colIsGrant].ToString(), Utility.Permissions.Grants.True, StringComparison.CurrentCultureIgnoreCase));
                    drSource[colIsGrantWithCheckBox] = (string.Equals(drv[colIsGrantWith].ToString(), Utility.Permissions.Grants.True, StringComparison.CurrentCultureIgnoreCase));
                    drSource[colIsDenyCheckBox] = (string.Equals(drv[colIsDeny].ToString(), Utility.Permissions.Grants.True, StringComparison.CurrentCultureIgnoreCase));
                    drSource[colIsAliasedCheckBox] = (string.Equals(drv[colIsAliased].ToString(), Utility.Permissions.Grants.True, StringComparison.CurrentCultureIgnoreCase));

                    // If the source is not a duplicate then add to table
                    if (drSource[colSourcePermission].ToString() != drLastSource[colSourcePermission].ToString() ||
                        drSource[colGranteeName].ToString() != drLastSource[colGranteeName].ToString() ||
                        drSource[colIsAliased].ToString() != drLastSource[colIsAliased].ToString() ||
                        drSource[colSourceName].ToString() != drLastSource[colSourceName].ToString() ||
                        drSource[colSourceType].ToString() != drLastSource[colSourceType].ToString()
                        )
                    {
                        dtSources.Rows.Add(drSource);
                        drLastSource = drSource;
                    }
                }
            }

            return dsEffective;
        }

        private bool verifyDatabase(ref string dbName, ref int dbId)
        {
            bool isMatch = false;
            string database = String.Empty;
            int dbid = 0;

            //dbName = dbName.Trim();
            if (dbName.Length > 0)
            {
                try
                {
                    // Open connection to repository and query permissions.
                    logX.loggerX.Info("Verify database name");

                    using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                    {
                        // Open the connection.
                        connection.Open();

                        // Execute stored procedure and get the databases
                        using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                    string.Format(QueryGetDatabase, m_snapshotId, dbName), null))
                        {
                            isMatch = false;
                            database = String.Empty;
                            while (rdr.Read())
                            {
                                database = rdr[colDatabaseName].ToString();
                                if (m_serverInstance.CaseSensitive)
                                {
                                    if (dbName.Equals(database))
                                    {
                                        isMatch = true;
                                        dbid = (int)rdr[colDatabaseId];
                                        break;
                                    }
                                    else
                                    {
                                        if (isMatch)
                                        {
                                            // There are too many matches at this point, so it is not valid
                                            isMatch = false;
                                            database = String.Empty;
                                            dbid = 0;
                                        }
                                        else
                                        {
                                            isMatch = true;
                                            dbid = (int)rdr[colDatabaseId];
                                        }
                                    }
                                }
                                else
                                {
                                    // on a server that is not case-sensitive, it is always a match
                                    isMatch = true;
                                    dbid = (int)rdr[colDatabaseId];
                                }
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    logX.loggerX.Error(@"Error - Unable to verify database name", ex);
                    isMatch = false;
                }
            }
            else
            {
                isMatch = false;
            }

            // Fix the databasename if there was a non case-sensitive match
            //  and return the id
            if (isMatch)
            {
                dbName = database;
                dbId = dbid;
            }
            return isMatch;
        }

        protected void setSnapshot(int snapshotId)
        {
            //use the passed snapshotid if it is not 0
            if (snapshotId > 0)
            {
                if (m_snapshotId != snapshotId)
                {
                    m_snapshotId = snapshotId;
                }
            }
            else
            {
                // if no valid snapshotid passed, use any preexisting one
                // but make sure the preexisting one is for this server or clear it
                if (m_serverInstance_shown == null || m_serverInstance_shown.ConnectionName != m_serverInstance.ConnectionName)
                {
                    m_snapshotId = 0;
                }
            }

            // if not valid, get the last one from the server
            if (m_snapshotId == 0)
            {
                try
                {
                    // Get the registered server information from the repository.
                    Sql.RegisteredServer.GetServer(Program.gController.Repository.ConnectionString,
                                                        m_serverInstance.ConnectionName, out m_serverInstance);

                    // Get the current snapshot id & timestamp, server version and databases.
                    if (m_serverInstance != null)
                    {
                        // Get last succesful collection snapshot id.
                        m_snapshotId = m_serverInstance.LastCollectionSnapshotId;
                    }
                    else
                    {
                        MsgBox.ShowWarning(ErrorMsgs.RolePermissionsCaption, ErrorMsgs.ServerNotRegistered);
                        m_snapshotId = 0;
                    }
                }
                catch (Exception ex)
                {
                    MsgBox.ShowError(ErrorMsgs.RolePermissionsCaption, ErrorMsgs.GetAuditServerInfoFromRepositoryFailed, ex);
                }
                //// force reverification of the user if something has changed
                //if (m_user != null)
                //{
                //    m_user.IsVerified = false;
                //}
            }

            Sql.Snapshot snapshot = Sql.Snapshot.GetSnapShot(m_snapshotId);

            // if still not valid, disable features
            if (snapshot == null)
            {
                _groupBox_SelectRole.Enabled =
                    _groupBox_Database.Enabled =
                    _label_Run.Enabled =
                    _button_CalculateEffective.Enabled =
                    _button_ShowPermissions.Enabled = false;

                _pictureBox_PermissionsWarning.Text = string.Empty;
                if (m_snapshotId == 0 ||
                    Sql.Snapshot.SnapshotCount(m_serverInstance.ConnectionName) == 0)
                {
                    _pictureBox_Snapshot.Image = null;
                    _linkLabel_Snapshot.Enabled = true;
                    _linkLabel_Snapshot.Text = Utility.ErrorMsgs.ServerNoSnapshots;
                    _linkLabel_Snapshot.LinkArea = new LinkArea(0, _linkLabel_Snapshot.Text.Length);
                }
                else
                {
                    m_snapshotId_shown_isValid = false;
                    _pictureBox_Snapshot.Image = AppIcons.AppImage16(AppIcons.Enum.SnapshotError);
                    _linkLabel_Snapshot.Enabled = true;
                    _linkLabel_Snapshot.Text = Utility.ErrorMsgs.ServerMissingSnapshot;
                    _linkLabel_Snapshot.LinkArea = new LinkArea(0, Utility.ErrorMsgs.ServerMissingSnapshot.Length);
                }

                m_database =
                    _textBox_Database.Text = String.Empty;
                m_dbid = 0;
                m_warning = false;

                // Get current on the right snapshot in the tree even though it may not actually exist
                //   so we are consistent in what is displayed
                if (m_snapshotId != 0)
                {
                    Program.gController.SetCurrentSnapshot(m_serverInstance, m_snapshotId);
                }

                //Program.gController.SetCurrentServer(m_serverInstance);
            }
            else
            {
                _groupBox_SelectRole.Enabled =
                    _groupBox_Database.Enabled =
                    _label_Run.Enabled =
                    _linkLabel_Snapshot.Enabled = true;

                checkShowPermissions();

                // Check and make sure the current database selection is valid, otherwise clear it
                if (m_database.Length > 0)
                {
                    if (verifyDatabase(ref m_database, ref m_dbid))
                    {
                        // Fix the database name for any case insensitive server difference
                        _textBox_Database.Text = m_database;
                    }
                    else
                    {
                        m_database =
                            _textBox_Database.Text = String.Empty;
                        m_dbid = 0;
                    }
                }

                m_snapshotId_shown_isValid = true;
                m_snapshotName = snapshot.SnapshotName;
                m_snapshotTime = snapshot.StartTime.ToLocalTime();
                switch (snapshot.Status)
                {
                    case Utility.Snapshot.StatusWarning:
                        if (snapshot.IsMissingWindowsUsers)
                        {
                            m_warning = true;
                        }
                        else
                        {
                            m_warning = false;
                        }
                        break;
                    case Utility.Snapshot.StatusSuccessful:
                        m_warning = false;
                        break;
                    default:
                        //Debug.Assert(false, "Unknown status encountered");
                        m_warning = true;
                        break;
                }
                _pictureBox_Snapshot.Image = snapshot.Icon;

                _linkLabel_Snapshot.Text = string.Format(SnapshotDisplay, m_snapshotName);
                _linkLabel_Snapshot.LinkArea = new LinkArea(SnapshotDisplay.IndexOf("{"), m_snapshotName.Length);

                Program.gController.SetCurrentSnapshot(m_serverInstance, snapshot.SnapshotId);
            }
            checkSelections();
        }

        protected bool checkSnapshot()
        {
            // Server must be valid for the snapshot to be valid
            bool isValid = checkServer();

            if (isValid)
            {
                if (Sql.Snapshot.GetSnapShot(m_snapshotId) == null)
                {
                    isValid = false;
                    logX.loggerX.Error(@"Error - Snapshot is missing or deleted");
                    MsgBox.ShowError(Utility.ErrorMsgs.CantGetRolePermissionsCaption, ErrorMsgs.ServerMissingSnapshot);

                    setSnapshot(m_snapshotId);
                }
            }

            return isValid;
        }

        protected bool checkServer()
        {
            bool registered = Sql.RegisteredServer.IsServerRegistered(m_serverInstance.ConnectionName);
            // Check to make sure the server is still registered to prevent crashing if not
            if (!registered)
            {
                logX.loggerX.Error(@"Error - Server is not registered");
                MsgBox.ShowError(Utility.ErrorMsgs.CantGetRolePermissionsCaption, ErrorMsgs.ServerNotRegistered);

                setSnapshot(m_snapshotId);
            }

            return registered;
        }

        protected bool checkSnapshotShown()
        {
            // Server must be valid for the snapshot to be valid
            bool isValid = (m_serverInstance_shown == null ? false : Sql.RegisteredServer.IsServerRegistered(m_serverInstance_shown.ConnectionName));

            if (isValid)
            {
                isValid = (Sql.Snapshot.GetSnapShot(m_snapshotId_shown) != null);
            }

            if (!isValid)
            {
                logX.loggerX.Error(@"Error - Server is not registered");
                MsgBox.ShowError(Utility.ErrorMsgs.CantGetRolePermissionsCaption, ErrorMsgs.ServerMissingSnapshot);

                _linkLabel_SnapshotProperties.Enabled =
                    _button_CalculateEffective.Enabled = false;
            }

            return isValid;
        }

        protected bool checkSelections()
        {
            bool isValid = false;
            if (m_snapshotId == m_snapshotId_shown &&
                m_database == m_database_shown &&
                m_role == m_role_shown)
            {
                isValid = true;
            }

            setGridAppearance(_grid_RoleRoles, isValid);
            setGridAppearance(_grid_RoleMembers, isValid);
            setGridAppearance(_grid_ExplicitServer, isValid);
            setGridAppearance(_grid_ExplicitDatabase, isValid);
            setGridAppearance(_grid_EffectiveServer, isValid);
            setGridAppearance(_grid_EffectiveDatabase, isValid);

            _button_CalculateEffective.Enabled = isValid;

            return isValid;
        }

        protected void checkShowPermissions()
        {
            bool isValid = false;
            if (_textBox_Database.Text.Trim().Length > 0)
            {
                if (_textBox_Role.Text.Trim().Length > 0)
                {
                    isValid = true;
                }
            }
            _button_ShowPermissions.Enabled = isValid;
        }

        /// <summary>
        /// Update the status bar to show the passed progress value and description
        /// </summary>
        /// <param name="progress">the percentage to add to the shown value</param>
        /// <param name="description">the description to show on the status bar</param>
        protected void updateStatusBar(int progress, string description)
        {
            _toolStripProgressBar_Status.Value = progress;
            _toolStripProgressBar_Status.Visible = true;
            _label_Status.Text = description;
            _statusStrip.Refresh();

            //make sure the cursor is continuing to show busy
            Cursor = Cursors.WaitCursor;
        }

        /// <summary>
        /// Update the status bar by incrementing the progress value by the passed amount
        /// </summary>
        /// <param name="increment">the percentage to add to the shown value</param>
        protected void incrementStatusBar(int increment)
        {
            _toolStripProgressBar_Status.Value += increment;
            _statusStrip.Refresh();

            //make sure the cursor is continuing to show busy
            Cursor = Cursors.WaitCursor;
        }

        /// <summary>
        /// Update the status bar by incrementing the progress value by the passed amount and updating the description
        /// </summary>
        /// <param name="increment">the percentage to add to the shown value</param>
        /// <param name="description">the description to show on the status bar</param>
        protected void incrementStatusBar(int increment, string description)
        {
            updateStatusBar(_toolStripProgressBar_Status.Value + increment, description);
        }

        /// <summary>
        /// Turn off the status bar display and update the status description
        /// </summary>
        /// <param name="description"></param>
        protected void finishStatusBar(string description)
        {
            _label_Status.Text = description;
            finishStatusBar();
        }

        /// <summary>
        /// Turn off the status bar display
        /// </summary>
        protected void finishStatusBar()
        {
            _toolStripProgressBar_Status.Visible = false;
            _statusStrip.Refresh();
        }

        #region Grid

        protected void setGridAppearance(Infragistics.Win.UltraWinGrid.UltraGrid grid, bool enabled)
        {
            if (enabled)
            {
                grid.DisplayLayout.Appearance.ResetForeColor();
                grid.DisplayLayout.Override.ActiveRowAppearance.ResetForeColor();
            }
            else
            {
                grid.DisplayLayout.Appearance.ForeColor =
                    grid.DisplayLayout.Override.ActiveRowAppearance.ForeColor = Color.Gray;
            }
        }

        protected void printGrid(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            Debug.Assert(grid.Tag.GetType() == typeof(ToolStripLabel));

            // Associate the print document with the grid & preview dialog here
            // in case we want to change documents for each grid
            _ultraGridPrintDocument.Grid = grid;
            _ultraGridPrintDocument.DefaultPageSettings.Landscape = true;
            _ultraGridPrintDocument.DefaultPageSettings.Color = false;
            _ultraGridPrintDocument.DocumentName = ErrorMsgs.RolePermissionsCaption;
            //_ultraGridPrintDocument.FitWidthToPages = 2;
            if (m_role_shown != null)
            {
                _ultraGridPrintDocument.Header.TextLeft =
                    string.Format(PrintHeaderDisplay,
                                        m_role_shown,
                                        string.Format(DottedNameDisplay,
                                                        m_serverInstance_shown.ConnectionName,
                                                        m_database_shown),
                                        m_snapshotTime_shown,
                                        string.Format(PrintPermissionsHeaderDisplay,
                                                        _ultraTabControl.SelectedTab.Text,
                                                        ((ToolStripLabel)grid.Tag).Text)
                                    );
            }
            else
            {
                _ultraGridPrintDocument.Header.TextLeft =
                    string.Format(PrintEmptyHeaderDisplay,
                                        string.Format(PrintPermissionsHeaderDisplay,
                                                        _ultraTabControl.SelectedTab.Text,
                                                        ((ToolStripLabel)grid.Tag).Text)
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
            bool iconHidden = false;
            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //save the current state of the icon column and then hide it before exporting
                    if (grid.DisplayLayout.Bands[0].Columns.Exists(colIcon))
                    {
                        // this column doesn't exist in the raw data hack
                        iconHidden = grid.DisplayLayout.Bands[0].Columns[colIcon].Hidden;
                        grid.DisplayLayout.Bands[0].Columns[colIcon].Hidden = true;
                        // Fix the sub list display on the effective grid
                        if (grid.DisplayLayout.Bands.Count > 1
                            && grid.DisplayLayout.Bands[1].Columns.Exists(colIcon))
                        {
                            grid.DisplayLayout.Bands[1].Columns[colIcon].Hidden = true;
                        }
                    }
                    _ultraGridExcelExporter.Export(grid, _saveFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MsgBox.ShowError(ErrorMsgs.ExportToExcelCaption, ErrorMsgs.FailedToExportToExcelFile, ex);
                }
                if (grid.DisplayLayout.Bands[0].Columns.Exists(colIcon))
                {
                    // this column doesn't exist in the raw data hack
                    grid.DisplayLayout.Bands[0].Columns[colIcon].Hidden = iconHidden;
                }
                // Fix the sub list display on the effective grid
                if (grid.DisplayLayout.Bands.Count > 1
                    && grid.DisplayLayout.Bands[1].Columns.Exists(colIcon))
                {
                    grid.DisplayLayout.Bands[1].Columns[colIcon].Hidden = iconHidden;
                }
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

        protected void setMenuConfiguration()
        {
            m_menuConfiguration.EditItems[(int)Utility.MenuItems_Edit.Properties] = false;
            m_menuConfiguration.ViewItems[(int)Utility.MenuItems_View.Refresh] = false;

            Program.gController.SetMenuConfiguration(m_menuConfiguration, this);
        }

        #endregion

        #region Events

        #region Buttons and Links

        private void _button_BrowseDatabases_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (checkSnapshot())
            {
                string database = String.Empty;
                database = Forms.Form_SelectDatabase.GetDatabaseName(m_snapshotId);

                if (!database.Length.Equals(0))
                {
                    _textBox_Database.Text = m_database = database;
                    verifyDatabase(ref m_database, ref m_dbid);
                }

                checkSelections();
            }

            Cursor = Cursors.Default;
        }

        private void _button_BrowseRoles_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (checkSnapshot())
            {
                string role = Forms.Form_SelectRole.GetRoleName(m_snapshotId, m_dbid);

                if (!role.Length.Equals(0))
                {
                    _textBox_Role.Text = role;
                    // DO NOT put this before the textBox update because it clears m_role in the TextChanged event
                    m_role = role;
                }

                checkSelections();
            }

            Cursor = Cursors.Default;
        }

        private void _button_CalculateEffective_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (checkSnapshotShown())
            {
                loadEffective();
            }

            Cursor = Cursors.Default;
        }

        private void _button_CalculateEffective_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Modifiers & Keys.Shift) == Keys.Shift)
            {
                m_showRaw = true;
            }
        }

        private void _button_ShowPermissions_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (checkSnapshot())
            {
                loadDataSource();
                checkSelections();
            }

            Cursor = Cursors.Default;
        }

        private void _linkLabel_Snapshot_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (checkServer())
            {
                int snapshotId = Forms.Form_SelectSnapshot.GetSnapshotId(m_serverInstance);

                if (snapshotId != 0)
                {
                    setSnapshot(snapshotId);

                    checkSelections();
                }
            }

            Cursor = Cursors.Default;
        }

        private void _linkLabel_SnapshotProperties_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (checkSnapshotShown())
            {
                Forms.Form_SnapshotProperties.Process(new Sql.ObjectTag(m_snapshotId_shown, Sql.ObjectType.TypeEnum.Snapshot));
            }

            Cursor = Cursors.Default;
        }

        #endregion

        #region Grids

        private void _grid_RoleRoles_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            e.Layout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.ColumnChooserEnabled = DefaultableBoolean.True;

            UltraGridBand band;

            band = e.Layout.Bands[0];

            band.Columns[colIcon].Header.Caption = colIconHdr;
            band.Columns[colIcon].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            band.Columns[colIcon].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            band.Columns[colIcon].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            band.Columns[colIcon].Width = 22;

            band.Columns[colRoleName].Header.Caption = colRoleNameHdr;
            band.Columns[colRoleName].Width = 200;

            band.Columns[colRoleOwner].Header.Caption = colRoleOwnerHdr;
            band.Columns[colRoleOwner].Width = 200;

            band.Columns[colRoleType].Header.Caption = colRoleTypeHdr;
            band.Columns[colRoleType].Width = 90;
        }

        private void _grid_RoleRoles_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (String.Compare(e.Row.Cells[colRoleName].Value.ToString(), m_role, true) == 0)
            {
                e.Row.Appearance.ForeColor = Color.IndianRed;
                e.Row.Cells[colRoleName].ToolTipText = MatchedRoleToolTip;
            }
        }

        private void _grid_RoleMembers_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            e.Layout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.True;

            UltraGridBand band;

            band = e.Layout.Bands[0];

            band.Columns[colIcon].Header.Caption = colIconHdr;
            band.Columns[colIcon].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            band.Columns[colIcon].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            band.Columns[colIcon].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            band.Columns[colIcon].Width = 22;

            band.Columns[colRoleName].Header.Caption = colRoleNameHdr;
            band.Columns[colRoleName].Width = 200;

            band.Columns[colRoleType].Header.Caption = colRoleTypeHdr;
            band.Columns[colRoleType].Width = 90;

            band.Columns[colLogin].Header.Caption = colLoginHdr;
            band.Columns[colLogin].Width = 120;
        }

        private void _grid_Explicit_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            //Note: This is the initialization event handler for both Effective grids
            e.Layout.Override.GroupByRowInitialExpansionState = GroupByRowInitialExpansionState.Collapsed;
            e.Layout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.True;

            UltraGridBand band;

            band = e.Layout.Bands[0];

            band.Columns[colIcon].Header.Caption = colIconHdr;
            band.Columns[colIcon].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            band.Columns[colIcon].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            band.Columns[colIcon].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            band.Columns[colIcon].Width = 22;

            band.Columns[colPermissionLevel].Hidden = true;
            band.Columns[colPermissionLevel].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            band.Columns[colObjectName].Header.Caption = colObjectNameHdr;
            band.Columns[colObjectName].Width = 190;

            band.Columns[colObjectType].Hidden = true;
            band.Columns[colObjectType].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            band.Columns[colObjectTypeName].Header.Caption = colObjectTypeNameHdr;
            band.Columns[colObjectTypeName].Width = 100;

            band.Columns[colPermission].Header.Caption = colPermissionHdr;
            band.Columns[colPermission].Width = 120;

            band.Columns[colGranteeName].Header.Caption = colGranteeNameHdr;
            band.Columns[colGranteeName].Width = 100;

            band.Columns[colIsGrant].Hidden = true;
            band.Columns[colIsGrant].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            band.Columns[colIsGrantCheckBox].Header.Caption = colIsGrantHdr;
            band.Columns[colIsGrantCheckBox].Width = 60;
            band.Columns[colIsGrantCheckBox].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            band.Columns[colIsGrantCheckBox].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

            band.Columns[colIsGrantWith].Hidden = true;
            band.Columns[colIsGrantWith].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            band.Columns[colIsGrantWithCheckBox].Header.Caption = colIsGrantWithHdr;
            band.Columns[colIsGrantWithCheckBox].Width = 64;
            band.Columns[colIsGrantWithCheckBox].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;

            band.Columns[colIsDeny].Hidden = true;
            band.Columns[colIsDeny].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            band.Columns[colIsDenyCheckBox].Header.Caption = colIsDenyHdr;
            band.Columns[colIsDenyCheckBox].Width = 60;
            band.Columns[colIsDenyCheckBox].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;

            band.Columns[colGrantorName].Header.Caption = colGrantorNameHdr;
            band.Columns[colGrantorName].Width = 100;
            //            band.Columns[colGrantorName].Hidden = true;

            band.Columns[colOwnerName].Header.Caption = colOwnerNameHdr;
            band.Columns[colOwnerName].Width = 100;

            band.Columns[colIsAliased].Hidden = true;
            band.Columns[colIsAliased].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            band.Columns[colIsAliasedCheckBox].Header.Caption = colIsAliasedHdr;
            band.Columns[colIsAliasedCheckBox].Width = 60;
            band.Columns[colIsAliasedCheckBox].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
        }

        private void _grid_Effective_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            //Note: This is the initialization event handler for both Effective grids
            e.Layout.Override.GroupByRowInitialExpansionState = GroupByRowInitialExpansionState.Collapsed;
            e.Layout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Override.GroupByRowAppearance.BackColor =
                e.Layout.Override.GroupByRowAppearance.BackColor2 = Color.White;

            UltraGridBand band;

            band = e.Layout.Bands[bandPermissions];
            band.ColHeadersVisible = true;

            if (m_showRaw)
            {
                band.Columns[colPermissionLevel].Hidden = false;
                band.Columns[colObjectId].Hidden = false;
                band.Columns[colObjectType].Hidden = false;
                band.Columns[colIsGrant].Hidden = false;
                band.Columns[colIsGrantWith].Hidden = false;
                band.Columns[colIsDeny].Hidden = false;
                band.Columns[colOwnerName].Hidden = false;
                band.Columns[colIsAliased].Hidden = false;
                band.Columns[colIsDeny].Hidden = false;
                band.Columns[colInherited].Hidden = false;
                band.Columns[colSourceName].Hidden = false;
                band.Columns[colSourceType].Hidden = false;
                band.Columns[colSourcePermission].Hidden = false;
                if (band.Columns.Contains(colIsGrantCheckBox))
                {
                    band.Columns[colIsGrantCheckBox].Hidden = true;
                    band.Columns[colIsGrantWithCheckBox].Hidden = true;
                    band.Columns[colIsDenyCheckBox].Hidden = true;
                    //band.Columns[colIsAliasedCheckBox].Hidden = true;
                }
            }
            else
            {
                band.Columns[colIcon].Header.Caption = colIconHdr;
                band.Columns[colIcon].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                band.Columns[colIcon].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
                band.Columns[colIcon].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
                band.Columns[colIcon].Width = 22;

                band.Columns[colPermissionLevel].Hidden = true;
                band.Columns[colPermissionLevel].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                band.Columns[colGroupName].Header.Caption = colObjectNameHdr;
                band.Columns[colGroupName].Hidden = true;
                band.Columns[colGroupName].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                band.Columns[colObjectId].Hidden = true;
                band.Columns[colObjectId].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                band.Columns[colObjectName].Header.Caption = colObjectNameHdr;
                band.Columns[colObjectName].Hidden = true;
                band.Columns[colObjectName].MergedCellStyle = MergedCellStyle.Always;
                band.Columns[colObjectName].Width = 220;

                band.Columns[colObjectType].Hidden = true;
                band.Columns[colObjectType].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                band.Columns[colObjectTypeName].Header.Caption = colObjectTypeNameHdr;
                band.Columns[colObjectTypeName].Width = 100;

                band.Columns[colPermission].Header.Caption = colPermissionHdr;
                band.Columns[colPermission].Width = 120;

                band.Columns[colIsGrant].Hidden = true;
                band.Columns[colIsGrant].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                band.Columns[colIsGrantCheckBox].Header.Caption = colIsGrantHdr;
                band.Columns[colIsGrantCheckBox].Width = 60;
                band.Columns[colIsGrantCheckBox].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;

                band.Columns[colIsGrantWith].Hidden = true;
                band.Columns[colIsGrantWith].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                band.Columns[colIsGrantWithCheckBox].Header.Caption = colIsGrantWithHdr;
                band.Columns[colIsGrantWithCheckBox].Width = 64;
                band.Columns[colIsGrantWithCheckBox].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;

                band.Columns[colIsDeny].Hidden = true;
                band.Columns[colIsDeny].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                band.Columns[colIsDenyCheckBox].Header.Caption = colIsDenyHdr;
                band.Columns[colIsDenyCheckBox].Width = 60;
                band.Columns[colIsDenyCheckBox].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;

                band.Columns[colOwnerName].Header.Caption = colOwnerNameHdr;
                band.Columns[colOwnerName].Width = 100;

                band = e.Layout.Bands[colSources];

                band.Columns[colIcon].Header.Caption = colIconHdr;
                band.Columns[colIcon].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                band.Columns[colIcon].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
                band.Columns[colIcon].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
                band.Columns[colIcon].Width = 22;

                band.Columns[colPermissionLevel].Hidden = true;
                band.Columns[colPermissionLevel].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                band.Columns[colObjectId].Hidden = true;
                band.Columns[colObjectId].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                band.Columns[colPermission].Hidden = true;
                band.Columns[colPermission].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                band.Columns[colSourcePermission].Header.Caption = colSourcePermissionHdr;
                //band.Columns[colSourcePermission].Width = 120;

                band.Columns[colIsGrant].Hidden = true;
                band.Columns[colIsGrant].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                band.Columns[colIsGrantCheckBox].Header.Caption = colIsGrantHdr;
                band.Columns[colIsGrantCheckBox].Width = 60;
                band.Columns[colIsGrantCheckBox].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;

                band.Columns[colIsGrantWith].Hidden = true;
                band.Columns[colIsGrantWith].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                band.Columns[colIsGrantWithCheckBox].Header.Caption = colIsGrantWithHdr;
                band.Columns[colIsGrantWithCheckBox].Width = 64;
                band.Columns[colIsGrantWithCheckBox].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;

                band.Columns[colIsDeny].Hidden = true;
                band.Columns[colIsDeny].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                band.Columns[colIsDenyCheckBox].Header.Caption = colIsDenyHdr;
                band.Columns[colIsDenyCheckBox].Width = 60;
                band.Columns[colIsDenyCheckBox].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;

                band.Columns[colGranteeName].Header.Caption = colGranteeNameHdr;
                band.Columns[colGranteeName].Width = 150;

                band.Columns[colGrantorName].Header.Caption = colGrantorNameHdr;
                band.Columns[colGrantorName].Width = 180;

                band.Columns[colIsAliased].Hidden = true;
                band.Columns[colIsAliased].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                band.Columns[colIsAliasedCheckBox].Header.Caption = colIsAliasedHdr;
                //band.Columns[colIsAliasedCheckBox].Width = 64;
                band.Columns[colIsAliasedCheckBox].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;

                band.Columns[colInherited].Hidden = true;
                band.Columns[colInherited].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                band.Columns[colSourceType].Hidden = true;
                band.Columns[colSourceType].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                band.Columns[colSourceTypeName].Header.Caption = colSourceObjectTypeHdr;
                band.Columns[colSourceTypeName].Hidden = true;
                //band.Columns[colSourceType].Width = 180;

                band.Columns[colSourceName].Header.Caption = colSourceObjectNameHdr;
                //band.Columns[colSourceName].Width = 180;
            }
        }

        private void _grid_BeforeRowActivate(object sender, RowEventArgs e)
        {
            ((UltraGrid)sender).DisplayLayout.Override.ActiveRowAppearance.BackColor = e.Row.Appearance.BackColor;
            ((UltraGrid)sender).DisplayLayout.Override.ActiveRowAppearance.ForeColor = e.Row.Appearance.ForeColor;
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
                    //m_gridCellClicked = true; 
                    grid.Selected.Rows.Clear();
                    cell.Row.Selected = true;
                    grid.ActiveRow = cell.Row;
                }
                else
                {
                    //m_gridCellClicked = false;
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

        private void _grid_Effective_InitializeGroupByRow(object sender, InitializeGroupByRowEventArgs e)
        {
            // This is used by the Effective Grids only and assumes their data structure
            UltraGrid grid = (UltraGrid)sender;
            if (e.Row.Column.Equals(grid.DisplayLayout.Bands[bandPermissions].Columns[colObjectTypeName])
                && !String.IsNullOrEmpty(e.Row.Value.ToString()))
            {
                e.Row.Description = e.Row.Value.ToString().Substring(0, e.Row.Value.ToString().Length - 1)
                                        + (e.Row.Value.ToString().EndsWith("y") ? "ies" : e.Row.Value.ToString().Substring(e.Row.Value.ToString().Length - 1) + "s")
                                        + " (" + e.Row.Rows.Count.ToString() + " item" + (e.Row.Rows.Count == 1 ? string.Empty : "s") + ")";
            }
            else
            {
                if (e.Row.Column.Equals(grid.DisplayLayout.Bands[bandPermissions].Columns[colGroupName]))
                {
                    if (e.Row.Description.IndexOf(" : ") > 0)
                    {
                        if (!e.Row.Rows[0].IsGroupByRow)
                        {
                            e.Row.Description = string.Format("{0} : {1} ({2} permission{3})",
                                                e.Row.Rows[0].Cells[colObjectTypeName].Text.Trim(),
                                                e.Row.Value,
                                                e.Row.Rows.Count.ToString(),
                                                (e.Row.Rows.Count == 1 ? string.Empty : "s")
                                                );
                        }
                        else
                        {
                            e.Row.Description = string.Format("{0} ({1} permission{2})",
                                                e.Row.Value,
                                                e.Row.Rows.Count.ToString(),
                                                (e.Row.Rows.Count == 1 ? string.Empty : "s")
                                                );
                        }
                    }
                }
            }
        }

        #endregion

        #region TextBoxes

        private void _textBox_Database_Leave(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            int dbid = 0;
            if (verifyDatabase(ref m_database, ref dbid))
            {
                // Fix the database name for any case insensitive server difference
                _textBox_Database.Text = m_database;
                m_dbid = dbid;
            }
            else
            {
                m_dbid = 0;
            }

            Cursor = Cursors.Default;
        }

        private void _textBox_Database_TextChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            m_database = _textBox_Database.Text;
            m_dbid = 0;

            checkSelections();
            checkShowPermissions();

            Cursor = Cursors.Default;
        }

        private void _textBox_Role_TextChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            m_role = _textBox_Role.Text;

            checkSelections();
            checkShowPermissions();

            Cursor = Cursors.Default;
        }

        #endregion

        #region Context Menus and Tool Strips

        private void _contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            UltraGrid grid = (UltraGrid)((ContextMenuStrip)sender).SourceControl;
            if (grid.Equals(_grid_RoleRoles))
            {
                _toolStripMenuItem_viewGroupMembers.Visible = false;
                _toolStripMenuItem_showPermissions.Visible =
                    _toolStripSeparator_Server.Visible = true;
                _toolStripMenuItem_showPermissions.Enabled = (grid.ActiveRow != null &&
                                                              m_snapshotId == m_snapshotId_shown);
            }
            else if (grid.Equals(_grid_RoleMembers))
            {
                _toolStripMenuItem_viewGroupMembers.Visible =
                    _toolStripMenuItem_showPermissions.Visible =
                    _toolStripSeparator_Server.Visible = true;
                if (grid.ActiveRow != null && m_snapshotId == m_snapshotId_shown)
                {
                    _toolStripMenuItem_viewGroupMembers.Enabled =
                        (grid.ActiveRow.Cells[colRoleType].Text == Sql.ServerPrincipalTypes.WindowsGroupText && grid.ActiveRow.Cells[colLogin].Text.Length > 0);
                }
                else
                {
                    _toolStripMenuItem_viewGroupMembers.Enabled = false;
                }
                _toolStripMenuItem_showPermissions.Enabled = (grid.ActiveRow != null &&
                                                              m_snapshotId == m_snapshotId_shown);
            }
            else
            {
                _toolStripMenuItem_viewGroupMembers.Visible =
                    _toolStripMenuItem_showPermissions.Visible =
                    _toolStripSeparator_Server.Visible = false;
            }

            _toolStripMenuItem_viewGroupByBox.Checked = !grid.DisplayLayout.GroupByBox.Hidden;
        }

        private void _toolStripMenuItem_viewGroupMembers_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            UltraGrid grid = (UltraGrid)((ContextMenuStrip)((ToolStripItem)sender).Owner).SourceControl;

            if (grid.ActiveRow != null && grid.Equals(_grid_RoleMembers) && grid.ActiveRow.Cells[colLogin].Text != null)
            {
                Sql.User group =
                    Sql.User.GetSnapshotUser(m_snapshotId_shown, grid.ActiveRow.Cells[colRoleName].Text,
                                             grid.ActiveRow.Cells[colRoleType].Text, true);

                Sql.User user = Forms.Form_SelectGroupMember.GetUser(m_snapshotId_shown, group);

                if (user != null)
                {
                    Program.gController.ShowRootView(
                        new Utility.NodeTag(
                            new Data.PermissionExplorer(m_serverInstance_shown, m_snapshotId_shown, user, m_database_shown,
                                                        Views.View_PermissionExplorer.Tab.UserPermissions),
                            Utility.View.PermissionExplorer));
                }
            }

            Cursor = Cursors.Default;
        }

        private void _toolStripMenuItem_showPermissions_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            UltraGrid grid = (UltraGrid)((ContextMenuStrip)((ToolStripItem)sender).Owner).SourceControl;
            Debug.Assert(grid.Equals(_grid_RoleRoles) || grid.Equals(_grid_RoleMembers), "Show Permissions called for invalid grid");

            if (grid.Equals(_grid_RoleRoles))
            {
                m_role = _textBox_Role.Text = grid.ActiveRow.Cells[colRoleName].Text;

                loadDataSource();
            }
            else
            {
                string login = grid.ActiveRow.Cells[colLogin].Text;
                if (login.Length > 0)
                {
                    Sql.User user =
                        Sql.User.GetSnapshotUser(m_snapshotId_shown, grid.ActiveRow.Cells[colRoleName].Text,
                                                 grid.ActiveRow.Cells[colRoleType].Text, true);

                    if (user != null)
                    {
                        Program.gController.ShowRootView(
                            new Utility.NodeTag(
                                new Data.PermissionExplorer(m_serverInstance_shown, m_snapshotId_shown, user, m_database_shown,
                                                            Views.View_PermissionExplorer.Tab.UserPermissions),
                                Utility.View.PermissionExplorer));
                    }
                }
                else if (grid.ActiveRow.Cells[colRoleType].Text == Sql.DatabasePrincipalTypes.ApplicationRoleText ||
                         grid.ActiveRow.Cells[colRoleType].Text == Sql.DatabasePrincipalTypes.DatabaseRoleText)
                {
                    m_role = _textBox_Role.Text = grid.ActiveRow.Cells[colRoleName].Text;

                    loadDataSource();
                }
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

        private void _toolStripButton_ShowSelections_ButtonClick(object sender, EventArgs e)
        {
            _gradientPanel_Selections.Visible = !_gradientPanel_Selections.Visible;

            if (_gradientPanel_Selections.Visible)
            {
                _ultraTabControl.SuspendLayout();
                _ultraTabControl.Top = 101;
                _ultraTabControl.Height -= 101;
                _ultraTabControl.ResumeLayout();
                _toolStripButton_ShowSelections.Text = SelectionsHide;
                _toolStripButton_ShowSelections.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.TaskHide_161; ;
            }
            else
            {
                _ultraTabControl.SuspendLayout();
                _ultraTabControl.Top = 0;
                _ultraTabControl.Height += 101;
                _ultraTabControl.ResumeLayout();
                _toolStripButton_ShowSelections.Text = SelectionsShow;
                _toolStripButton_ShowSelections.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.TaskShow_16;
            }
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

        private void RolePermissions_Enter(object sender, EventArgs e)
        {
            setMenuConfiguration();
        }

        #endregion
    }
}
