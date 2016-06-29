using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;

using Idera.SQLsecure.Core.Accounts;
using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Utility;

namespace Idera.SQLsecure.UI.Console.Views
{
    public partial class View_UserPermissions : Idera.SQLsecure.UI.Console.Controls.BaseView, Interfaces.IView
    {
        #region IView

        void Interfaces.IView.SetContext(Interfaces.IDataContext contextIn)
        {
            Title = Utility.Constants.ViewTitle_UserPermissions;

            //if (((Data.UserPermissions)contextIn).ServerInstance == null)
            //{
            //    _button_BrowseServers_Click(null, null);
            //}

            m_serverInstance = ((Data.UserPermissions)contextIn).ServerInstance;

            if (m_serverInstance == null)
            {
                _groupBox_SelectUser.Enabled = false;
                _linkLabel_Snapshot.Text = "";
                _textBox_Database.Enabled = false;
                _button_BrowseDatabases.Enabled = false;
            }
            else
            {
                _groupBox_SelectUser.Enabled = true;
                _textBox_Database.Enabled = true;
                _button_BrowseDatabases.Enabled = true;

                //Get the server info and snapshot list and load the datasource for the grid
                _textBox_Server.Text = m_serverInstance.ConnectionName;

                setSnapshot( ((Data.UserPermissions)contextIn).SnapShotId);

                // Make sure logintype is set correctly to start
                _radioButton_WindowsUser.Checked = true;
                m_loginType = Sql.LoginTypes.WindowsLogin;
            }

            checkSelections();
        }
        String Interfaces.IView.HelpTopic
        {
            get { return Utility.Help.UserPermissionsHelpTopic; }
        }
        String Interfaces.IView.ConceptTopic
        {
            get { return Utility.Help.UserPermissionsConceptTopic; }
        }
        String Interfaces.IView.Title
        {
            get { return Title; }
        }

        #endregion

        #region fields

        private Sql.RegisteredServer m_serverInstance;
        private int m_snapshotId = 0;
        private string m_loginType;
        private Sql.User m_user;
        private string m_database = "";

        private int m_snapshotId_shown = 0;
        private string m_loginType_shown = "";
        private Sql.User m_user_shown = null;
        private string m_database_shown = "";

        #endregion

        #region Ctors

        public View_UserPermissions()
            : base()
        {
            InitializeComponent();

            // Initialize base class fields.
            m_menuConfiguration = new Utility.MenuConfiguration();

            _button_ShowPermissions.Enabled = checkProcessValid();
        }

        #endregion

        #region Query and Columns

        private const string QueryGetPermissions =
                    @"SQLsecure.dbo.isp_sqlsecure_getuserpermission";
        private const string ParamGetPermissionsSnapshotId = @"@snapshotid";
        private const string ParamGetPermissionsLoginType = @"@logintype";
        private const string ParamGetPermissionsSid = @"@inputsid";
        private const string ParamGetPermissionsSqlLogin = @"@sqllogin";
        private const string ParamGetPermissionsDatabase = @"@databasename";

        private static string FilterPermissionLevel = colPermissionLevel + @" = '{0}'";
        private static string FilterPermissionType = colPermissionType + @" = '{0}'";

        // Columns for handling query results
        private const string colSnapshotId = @"snapshotid";
        private const string colPermissionLevel = @"permissionlevel";
        private const string colPermissionType = @"permissiontype";
        private const string colPrincipalName = @"principalname";
        private const string colPrincipalType = @"principaltype";
        private const string colDatabasePrincipalName = @"databaseprincipal";
        private const string colDatabasePrincipalType = @"databaseprincipaltype";

        #endregion

        #region Helpers

        private void loadDataSource()
        {
            //Debug.Assert(!m_snapshotId.Equals(0));

            try
            {
                // Open connection to repository and query permissions.
                DiagLog.LogInfo("Retrieve User Permissions");

                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Setup register server params.
                    SqlParameter paramSnapshotId = new SqlParameter(ParamGetPermissionsSnapshotId, m_snapshotId);
                    SqlParameter paramLoginType = new SqlParameter(ParamGetPermissionsLoginType, m_loginType);
                    SqlParameter paramSid = new SqlParameter(ParamGetPermissionsSid, m_user.Sid.BinarySid);
                    SqlParameter paramSqlLogin = new SqlParameter(ParamGetPermissionsSqlLogin, m_user.Name);
                    SqlParameter paramDatabase = new SqlParameter(ParamGetPermissionsDatabase, m_database);

                    SqlCommand cmd = new SqlCommand(QueryGetPermissions, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 90;        // The permissions take > 30 secs on a 2005 server
                    cmd.Parameters.Add(paramSnapshotId);
                    cmd.Parameters.Add(paramLoginType);
                    cmd.Parameters.Add(paramSid);
                    cmd.Parameters.Add(paramSqlLogin);
                    cmd.Parameters.Add(paramDatabase);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    //Build Explicit Permissions datasource
                    this._grid_Explicit.BeginUpdate();
                    DataView dv_Explicit = new DataView(ds.Tables[0]);
                    dv_Explicit.RowFilter = string.Format(FilterPermissionType, Utility.Permissions.Type.Explicit);
                    this._grid_Explicit.DataSource = dv_Explicit;
                    this._grid_Explicit.DataMember = "";
                    this._grid_Explicit.EndUpdate();

                    //Build Effective/Assigned Permissions datasource
                    this._grid_Effective.BeginUpdate();
                    DataView dv_Effective = new DataView(ds.Tables[0]);
                    dv_Effective.RowFilter = string.Format(FilterPermissionType, Utility.Permissions.Type.Effective);
                    this._grid_Effective.DataSource = dv_Effective;
                    this._grid_Effective.DataMember = "";
                    this._grid_Effective.EndUpdate();

                    //Build Server Logins datasource
                    this._grid_Server.BeginUpdate();
                    DataView dv_Server = new DataView(ds.Tables[0]);
                    dv_Server.RowFilter = string.Format(@"{0} AND {1}",
                                                string.Format(FilterPermissionType, Utility.Permissions.Type.Explicit),
                                                string.Format(FilterPermissionLevel, Utility.Permissions.Level.Server)
                                            );
                    DataTable dt_Server = dv_Server.ToTable(true, new string[] { colPrincipalName, colPrincipalType });
                    dv_Server = dt_Server.DefaultView;
                    dv_Server.Sort = colPrincipalName;
                    this._grid_Server.DataSource = dv_Server;
                    this._grid_Server.DataMember = "";
                    this._grid_Server.DisplayLayout.Bands[0].Columns.Add();
                    this._grid_Server.EndUpdate();

                    //Build Database Logins datasource
                    this._grid_Database.BeginUpdate();
                    DataView dv_Database = new DataView(ds.Tables[0]);
                    dv_Database.RowFilter = string.Format(@"{0} AND {1}",
                                                string.Format(FilterPermissionType, Utility.Permissions.Type.Explicit),
                                                string.Format(FilterPermissionLevel, Utility.Permissions.Level.Database)
                                            );
                    DataTable dt_Database = dv_Database.ToTable(true, new string[] { colDatabasePrincipalName, colDatabasePrincipalType });
                    dv_Database = dt_Database.DefaultView;
                    dv_Database.Sort = colDatabasePrincipalName;
                    this._grid_Database.DataSource = dv_Database;
                    this._grid_Database.DataMember = "";
                    this._grid_Database.DisplayLayout.Bands[0].Columns.Add();
                    this._grid_Database.EndUpdate();

                    //Build Raw data grid datasource for use in testing and development
                    this._grid.BeginUpdate();
                    this._grid.DataSource = ds.Tables[0];
                    this._grid.DataMember = "";
                    this._grid.EndUpdate();

                    m_snapshotId_shown = m_snapshotId;
                    m_loginType_shown = m_loginType;
                    m_user_shown = m_user;
                    m_database_shown = m_database;
                }
            }
            catch (SqlException ex)
            {
                DiagLog.LogError(@"Error - Unable to retrieve user permissions", ex);
                MsgBox.ShowError(Utility.ErrorMsgs.CantGetUserPermissionsCaption, ex.Message);
                checkSelections();
            }
        }
        protected void setSnapshot(int snapshotId)
        {
            // 0 means no valid snapshot id, so just use any preexisting one
            if (!snapshotId.Equals(0))
            {
                m_snapshotId = snapshotId;
            }
            // if not valid, get the last one from the server
            if (m_snapshotId.Equals(0))
            {
                m_snapshotId = m_serverInstance.LastCollectionSnapshotId;
            }
            // if still not valid, disable features
            if (m_snapshotId.Equals(0))
            {
                m_snapshotId = m_serverInstance.LastCollectionSnapshotId;
            }
            else
            {
                Sql.Snapshot snapshot = Sql.Snapshot.GetSnapShot(m_snapshotId);
                _linkLabel_Snapshot.Text = @"Snapshot: " + snapshot.StartTime.ToString(Utility.Constants.DATETIME_FORMAT);
                if (snapshot.Baseline.Equals(Utility.Snapshot.BaselineTrue))
                {
                    _linkLabel_Snapshot.Text += @"  (baseline)";
                }
            }
            checkSelections();
        }
        protected bool checkProcessValid()
        {
            bool isValid = false;
            if (m_snapshotId >= 0 &&
                m_user != null &&
                m_database.Length > 0)
            {
                if (m_user.Sid != null)
                {
                    isValid = true;
                }
            }
            return isValid;
        }
        protected void checkSelections()
        {
            if (m_snapshotId == m_snapshotId_shown &&
                m_loginType  == m_loginType_shown &&
                m_user == m_user_shown &&
                m_database == m_database_shown )
            {
                switch (_tabControl_Permissions.SelectedIndex)
                {
                    case (0):
                        _grid_Explicit.Enabled = true;
                        break;
                    case (1):
                        _grid_Effective.Enabled = true;
                        break;
                    case (2):
                        _grid_Server.Enabled = true;
                        break;
                    case (3):
                        _grid_Database.Enabled = true;
                        break;
                    default:
                        _grid.Enabled = true;
                        break;
                }
                _tabControl_Permissions.Enabled = true;
                _label_NoView.SendToBack();
            }
            else
            {
                _label_NoView.BringToFront();
                _tabControl_Permissions.Enabled = false;
                switch (_tabControl_Permissions.SelectedIndex)
                {
                    case (0):
                        _grid_Explicit.Enabled = false;
                        break;
                    case (1):
                        _grid_Effective.Enabled = false;
                        break;
                    case (2):
                        _grid_Server.Enabled = false;
                        break;
                    case (3):
                        _grid_Database.Enabled = false;
                        break;
                    default:
                        _grid.Enabled = false;
                        break;
                }
            }
        }

        protected void setMenuConfiguration()
        {
            m_menuConfiguration.EditItems[(int)Utility.MenuItems_Edit.Remove] = false;
            m_menuConfiguration.EditItems[(int)Utility.MenuItems_Edit.Properties] = false;
            m_menuConfiguration.EditItems[(int)Utility.MenuItems_View.Refresh] = false;

            Program.gController.SetMenuConfiguration(m_menuConfiguration, this);
        }

        #endregion

        #region Events

        #region View events

        private void View_UserPermissions_Enter(object sender, EventArgs e)
        {
            Program.gController.SetMenuConfiguration(m_menuConfiguration, this);
        }

        private void View_UserPermissions_Leave(object sender, EventArgs e)
        {
            Program.gController.SetMenuConfiguration(new Utility.MenuConfiguration());
        }

        #endregion

        private void _button_BrowseDatabases_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            string database = "";

            database = Forms.Form_SelectDatabase.GetDatabaseName(m_snapshotId);

            if (!database.Length.Equals(0))
            {
                _textBox_Database.Text = m_database = database;

            }

            _button_ShowPermissions.Enabled = checkProcessValid();
            checkSelections();

            Cursor = Cursors.Default;
        }

        private void _button_BrowseUsers_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Sql.User user = Forms.Form_SelectUser.GetUser(m_snapshotId, m_loginType);

            if (user != null)
            {
                _textBox_User.Text = user.Name;
                // DO NOT put this before the textBox update because it clears m_user in the TextChanged event
                m_user = user;

            }

            _button_ShowPermissions.Enabled = checkProcessValid();
            checkSelections();

            Cursor = Cursors.Default;
        }

        private void _button_ShowPermissions_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            loadDataSource();
            checkSelections();

            Cursor = Cursors.Default;
        }

        private void _linkLabel_Snapshot_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            int snapshotId = 0;
            snapshotId = Forms.Form_SelectSnapshot.GetSnapshotId(m_serverInstance);

            setSnapshot(snapshotId);

            _button_ShowPermissions.Enabled = checkProcessValid();
            checkSelections();

            Cursor = Cursors.Default;
        }

        private void _radioButton_SQLLogin_CheckedChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (((RadioButton)sender).Checked)
            {
                if (m_loginType != Sql.LoginTypes.SqlLogin)
                {
                    _textBox_User.Text = "";
                    m_user = null;
                    m_loginType = Sql.LoginTypes.SqlLogin;
                }
            }
            checkSelections();

            Cursor = Cursors.Default;
        }

        private void _radioButton_WindowsUser_CheckedChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (((RadioButton)sender).Checked)
            {
                if (m_loginType != Sql.LoginTypes.WindowsLogin)
                {
                    _textBox_User.Text = "";
                    m_user = null;
                    m_loginType = Sql.LoginTypes.WindowsLogin;
                }
            }
            checkSelections();

            Cursor = Cursors.Default;
        }

        private void _textBox_Database_Leave(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            m_database = _textBox_Database.Text;
            _button_ShowPermissions.Enabled = checkProcessValid();

            Cursor = Cursors.Default;
        }

        private void _textBox_Database_TextChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            m_database = _textBox_Database.Text;
            checkSelections();

            Cursor = Cursors.Default;
        }

        //private void _textBox_Server_Leave(object sender, EventArgs e)
        //{
        //    Cursor = Cursors.WaitCursor;

        //    m_database = _textBox_Database.Text;
        //    _button_ShowPermissions.Enabled = checkProcessValid();

        //    Cursor = Cursors.Default;
        //}

        //private void _textBox_Server_TextChanged(object sender, EventArgs e)
        //{
        //    Cursor = Cursors.WaitCursor;

        //    if (_textBox_Server.Text.Length > 0 && m_serverInstance != null)
        //    {
        //        if (_textBox_Server.Text != m_serverInstance.ConnectionName)
        //        {
        //            m_serverInstance = Program.gController.Repository.GetServer(_textBox_Server.Text);
        //        }
        //    }

        //    Cursor = Cursors.Default;
        //}

        private void _textBox_User_Leave(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (m_user == null && !_textBox_User.Text.Length.Equals(0))
            {
                m_user = Sql.User.GetSnapshotUser(m_snapshotId, _textBox_User.Text, m_loginType);
            }
            _button_ShowPermissions.Enabled = checkProcessValid();

            Cursor = Cursors.Default;
        }

        private void _textBox_User_TextChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            m_user = null;
            checkSelections();

            Cursor = Cursors.Default;
        }

        #endregion

        private void _tabPage_Server_Enter(object sender, EventArgs e)
        {

        }
    }
}
