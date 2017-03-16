using Microsoft.Reporting.WinForms;
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
using Idera.SQLsecure.UI.Console.ActiveDirectory;
using Idera.SQLsecure.UI.Console.Utility;
using Idera.SQLsecure.UI.Console.Sql;

namespace Idera.SQLsecure.UI.Console.Views
{
    public partial class Report_UsersPermissions : Idera.SQLsecure.UI.Console.Controls.BaseReport, Interfaces.IView
    {
        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Views.Report_UsersPermissions");

        #region IView

        void Interfaces.IView.SetContext(Interfaces.IDataContext contextIn)
        {
            Data.Report context = (Data.Report)contextIn;

            if (context.RunReport && _button_RunReport.Enabled)
            {
                runReport();
            }
        }

        #endregion

        #region Fields

        private string m_loginType;
        private Sql.User m_user;
        private string m_permissionType;
        private DataView m_userPermissions;

        // SQLsecure 3.1 (Anshul Aggarwal) - Maintains a mapping from connection to server so we can check RegisteredServer type based on server dropdown value.
        private Dictionary<string, RegisteredServer> _connectionNameToServer = new Dictionary<string, RegisteredServer>();

        private static Point WINDOWS_RADIO_BUTTON_LOCATION = new Point(96, 68);
        private static Point AZURE_RADIO_BUTTON_LOCATION = new Point(240, 68);
        private static Point SQL_RADIO_BUTTON_LOCATION = new Point(385, 68);

        #endregion

        #region Ctors

        public Report_UsersPermissions()
        {
            InitializeComponent();

            // set default values for parameters based on initial state of controls
            m_loginType = Sql.LoginType.WindowsLogin;
            _radioButton_WindowsUser.Checked = true;

            //set the global title, the report title in the header graphic, and the title in the
            //status bar
            m_Title =
                _label_ReportTitle.Text =
                _label_Status.Text = Utility.Constants.ReportTitle_UsersPermissions;

            //set the description and getting started text
            _label_Description.Text = Utility.Constants.ReportSummary_UsersPermissions;

            int i = 1;
            StringBuilder instructions = new StringBuilder(Utility.Constants.ReportRunInstructions_MultiStep);
            instructions.Append(newline);
            instructions.AppendFormat(instructionformat, i++, Utility.Constants.ReportRunInstructions_UseSelection, newline);
            instructions.AppendFormat(instructionformat, i++, Utility.Constants.ReportRunInstructions_Server, newline);
            instructions.AppendFormat(instructionformat, i++, Utility.Constants.ReportRunInstructions_LoginType, newline);
            instructions.AppendFormat(instructionformat, i++, Utility.Constants.ReportRunInstructions_PermissionType, newline);
            instructions.AppendFormat(instructionformat, i++, Utility.Constants.ReportRunInstructions_UserName, newline);
            instructions.AppendFormat(instructionformat, i, Utility.Constants.ReportRunInstructions_NoParameters, newline);
            instructions.AppendFormat(warningformat, Utility.Constants.ReportWarning_Resources);
            _label_Instructions.Text = instructions.ToString();

            _button_RunReport.Enabled = false;

            //start off with some default values
            _comboBox_Server.Items.Add(Utility.Constants.ReportSelect_AllServers);
            _comboBox_Server.Text = Utility.Constants.ReportSelect_AllServers;

            _comboBox_PermissionType.Items.Clear();
            _comboBox_PermissionType.Items.Add(PermissionTypeExplicit);
            _comboBox_PermissionType.Items.Add(PermissionTypeEffective);
            _comboBox_PermissionType.SelectedItem = PermissionTypeExplicit;
            m_permissionType = Utility.Permissions.Type.Explicit;

            _comboBox_Level.SelectedIndex = 0;
        }

        #endregion

        #region Queries & Constants

        // Main Report
        private const string QueryDataSource = @"SQLsecure.dbo.isp_sqlsecure_report_getuserpermissions";
        private const string QueryDataSourceUser = @"SQLsecure.dbo.isp_sqlsecure_report_getuserpermissions_user";
        private const string DataSourceName = @"ReportsDataset_isp_sqlsecure_report_getuserpermissions";

        // Sub-Reports
        private const string SubReportPermissionsQueryDataSource = @"SQLsecure.dbo.isp_sqlsecure_report_getuserdatabasepermissions";
        private const string SubReportPermissionsDataSourceName = @"ReportsDataset_isp_sqlsecure_report_getuserdatabasepermissions";
        
        private const string ReportParamServer = @"server";

        private const string AllConnectionsValue = @"ALL";
        private const string AllServersValue = @"%";

        private const string PermissionTypeExplicit = @"Assigned";
        private const string PermissionTypeEffective = @"Effective";

        private const string ReportEmbeddedResource = @"Idera.SQLsecure.UI.Console.Reports.Report_UsersPermissions.rdlc";
        private const string ReportEmbeddedResourceUser = @"Idera.SQLsecure.UI.Console.Reports.Report_UsersPermissionsUser.rdlc";

        #endregion

        #region Helpers

        protected internal override void checkSelections()
        {
            _button_RunReport.Enabled = false;

            //they need to pick a server
            if (_comboBox_Server.Text.Length == 0) return;

            //they also need to enter a user name
            _textBox_User.Text = _textBox_User.Text.Trim();
            if (_textBox_User.Text.Length == 0) return;

            //and choose a permission type
            if (_comboBox_PermissionType.Text.Length == 0) return;

            _button_RunReport.Enabled = true;
        }

        #region SubReports

        void SubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            logX.loggerX.Info(@"Retrieve data for report User Permissions subreport " + e.ReportPath);

            ReportDataSource rds = new ReportDataSource();

            //This event will run once for each iteration of each subreport 
            //the ReportPath will identify which one is running and the report parameters
            //can be accessed to create a unique datasource for each pass if needed
            e.DataSources.Clear();

            switch (e.ReportPath)
            {
                case "SubReport_Logins":
                case "SubReport_LoginsUser":

                    // This is the same dataset that the primary report uses, so just pass it again
                    // The full dataset is passed since it already contains all the data on the first run 
                    // and the subreport uses the servername as a filter internally
                    rds.Name = DataSourceName;
                    rds.Value = m_userPermissions;
                    e.DataSources.Add(rds);
                    break;

                case "SubReport_UsersPermissions":
                    //the server name is passed as a parameter to the subreport
                    //so extract it to create a unique dataset on each pass
                    string server = e.Parameters[ReportParamServer].Values[0].ToString();

                    try
                    {
                        using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                        {
                            // Open the connection.
                            connection.Open();

                            //Create the permissions sub report datasource
                            SqlCommand cmd = new SqlCommand(SubReportPermissionsQueryDataSource, connection);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();

                            DataSet ds = new DataSet();

                            // Build parameters
                            SqlParameter paramRunDate = new SqlParameter(SqlParamRunDate, m_reportDate);
                            SqlParameter paramUser = new SqlParameter(SqlParamUser, _textBox_User.Text.Trim());
                            SqlParameter paramServer = new SqlParameter(ReportParamServer, server);
                            SqlParameter paramUserType = new SqlParameter(SqlParamUserType, m_loginType);
                            SqlParameter paramPermission = new SqlParameter(SqlParamPermissionType, m_permissionType);
                            cmd.Parameters.Add(paramRunDate);
                            cmd.Parameters.Add(paramUser);
                            cmd.Parameters.Add(paramServer);
                            cmd.Parameters.Add(paramUserType);
                            cmd.Parameters.Add(paramPermission);

                            // SQLsecure 3.1 (Anshul Aggarwal) - Fill Data for Azure Account (User or Group) separately.
                            if (m_loginType == Sql.LoginType.AzureADAccount)
                            {
                                paramUserType.Value = Sql.LoginType.AzureADUser;

                                // Get data for Azure AD Users
                                SqlDataAdapter da = new SqlDataAdapter(cmd);
                                da.Fill(ds);
                            
                                paramUserType.Value = Sql.LoginType.AzureADGroup;

                                // Get data for Azure AD Group
                                da = new SqlDataAdapter(cmd);
                                da.Fill(ds);

                            }
                            else
                            {
                                // Get data
                                SqlDataAdapter da = new SqlDataAdapter(cmd);
                                da.Fill(ds);
                            }

                            // Attach datasource
                            rds.Name = SubReportPermissionsDataSourceName;
                            rds.Value = ds.Tables[0].DefaultView;
                            e.DataSources.Add(rds);
                        }
                    }
                    catch (SqlException ex)
                    {
                        logX.loggerX.Error("Unable to create User Permissions Detail subreport datasource", ex);
                        return;
                    }

                    break;

                case "SubReport_ServersUsedSummary":
                    e.DataSources.Add(getServerListDataSet());

                    break;
            }
        }

        #endregion

        protected internal override void runReport()
        {
            if (_comboBox_Server.Text == Utility.Constants.ReportSelect_AllServers)
            {
                if (DialogResult.No == MsgBox.ShowWarningConfirm(Utility.Constants.ReportTitle_UsersPermissions,
                                                                 Utility.ErrorMsgs.ReportAllServersWarning))
                {
                    return;
                }
            }

            base.runReport();

            logX.loggerX.Info(@"Retrieve data for report User Permissions");

            if (m_user == null)
            {
                // SQLsecure 3.1 (Anshul Aggarwal) - Skip domain check for Azure AD Accounts as well.
                if (m_loginType.Equals(Sql.LoginType.SqlLogin) || m_loginType.Equals(Sql.LoginType.AzureADAccount))
                {
                    // if it is a SQL Login, it will not be validated, so just create the m_user without a sid
                    m_user = new Sql.User(_textBox_User.Text, null, m_loginType, Sql.User.UserSource.UserEntry);
                }
                else
                {
                    // Check the domain for the user
                    m_user = Idera.SQLsecure.UI.Console.Sql.User.GetDomainUser(_textBox_User.Text, m_loginType, false);

                    if (m_user == null)
                    {
                        // this user is not found anywhere, so alert the user
                        MsgBox.ShowInfo(ErrorMsgs.UserPermissionsCaption, string.Format(ErrorMsgs.WindowsUserNotMatchedMsg, _textBox_User.Text));

                        _button_RunReport.Enabled = false;

                        return;
                    }
                }
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Setup stored procedure
                    SqlCommand cmd = new SqlCommand(_comboBox_Level.SelectedIndex == 0 ? QueryDataSource : QueryDataSourceUser, connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DataSet ds = new DataSet();

                    // Build parameters
                    SqlParameter paramRunDate = new SqlParameter(SqlParamRunDate, m_reportDate);
                    SqlParameter paramUser = new SqlParameter(SqlParamUser, _textBox_User.Text.Trim());
                    SqlParameter paramServer = new SqlParameter(ReportParamServer, getServerName(_comboBox_Server.Text));
                    SqlParameter paramUserType = new SqlParameter(SqlParamUserType, m_loginType);
                    SqlParameter paramPolicyid = new SqlParameter(SqlParamPolicyid, m_policyid);
                    SqlParameter paramUsebaseline = new SqlParameter(SqlParamUsebaseline, m_useBaseline);
                    cmd.Parameters.Add(paramRunDate);
                    cmd.Parameters.Add(paramUser);
                    cmd.Parameters.Add(paramServer);
                    cmd.Parameters.Add(paramUserType);
                    cmd.Parameters.Add(paramPolicyid);
                    cmd.Parameters.Add(paramUsebaseline);

                    // SQLsecure 3.1 (Anshul Aggarwal) - Fill Data for Azure AD Account (User or Group) separately.
                    if (m_loginType == Sql.LoginType.AzureADAccount)
                    {
                        // Get data for Azure AD User
                        paramUserType.Value = Sql.LoginType.AzureADUser;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(ds);

                        // Get data for Azure AD Group
                        paramUserType.Value = Sql.LoginType.AzureADGroup;
                        da = new SqlDataAdapter(cmd);
                        da.Fill(ds);
                    }
                    else
                    {
                        // Get data
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(ds);
                    }

                    ReportDataSource rds = new ReportDataSource();
                    rds.Name = DataSourceName;
                    rds.Value = m_userPermissions = ds.Tables[0].DefaultView;
                    _reportViewer.Reset();
                    _reportViewer.LocalReport.DataSources.Clear();
                    _reportViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubreportProcessingEventHandler);
                    _reportViewer.LocalReport.EnableHyperlinks = true;
                    _reportViewer.LocalReport.ReportEmbeddedResource = _comboBox_Level.SelectedIndex == 0 ? ReportEmbeddedResource : ReportEmbeddedResourceUser;
                    _reportViewer.LocalReport.DataSources.Add(rds);
                }

                //add report parameters
                ReportParameter[] Param = new ReportParameter[7];
                Param[0] = new ReportParameter("ReportTitle", string.Format(Utility.Constants.REPORTS_TITLE_STR, m_Title));
                Param[1] = new ReportParameter("UserRange", m_reportDateDisplay);
                Param[2] = new ReportParameter("user", m_user.Name);
                Param[3] = new ReportParameter("usertype", m_loginType);
                Param[4] = new ReportParameter("serverName", _comboBox_Server.Text);
                Param[5] = new ReportParameter("permissiontype", m_permissionType);
                Param[6] = new ReportParameter("Expand_All", _isExpanded.ToString());

                _reportViewer.LocalReport.SetParameters(Param);

                // Make sure _reportViewer is created.
                // We had issues on some servers where the _reportViewer was not yet created
                // and this caused a crash.
                if (!_reportViewer.IsHandleCreated)
                {
                    if (!_reportViewer.Created)
                    {
                        _reportViewer.CreateControl();
                    }
                }

                _reportViewer.RefreshReport();
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error(@"Error - Unable to get report data", ex);
                MsgBox.ShowError(m_Title, ErrorMsgs.CantGetReportData, ex);
            }
        }

        #endregion

        #region Events

        private void _button_BrowseUsers_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            try
            {
                ADObject[] results;
                ObjectPickerWrapper picker = new ObjectPickerWrapper();
                results = picker.ShowObjectPicker(this.Handle);
                if (results != null)
                {
                    Debug.Assert(results.Length == 1);

                    for (int i = 0; i <= results.Length - 1; i++)
                    {
                        //do this before creating the user because it will remove m_user in the change event
                        _textBox_User.Text = results[i].SamAccountName;
                        m_user = new Sql.User(results[i].SamAccountName,
                                                results[i].Sid,
                                                m_loginType,
                                                Sql.User.UserSource.ActiveDirectory);
                    }
                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error("Error - Server Access Report unable to browse Domain for Users & Groups", ex);
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.CantGetUsersCaption, ex);
            }

            checkSelections();

            Cursor = Cursors.Default;
        }

        private void _radioButton_SQLLogin_CheckedChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            _button_BrowseUsers.Enabled = !((RadioButton)sender).Checked;

            if (((RadioButton)sender).Checked)
            {
                if (m_loginType != Sql.LoginType.SqlLogin)
                {
                    // try to be smart and not clear the user if it was not validated to type previously
                    if (m_user != null && m_user.Sid != null)
                    {
                        _textBox_User.Text = string.Empty;
                        m_user = null;
                    }
                    m_loginType = Sql.LoginType.SqlLogin;
                }
            }
            checkSelections();

            Cursor = Cursors.Default;
        }

        private void _radioButton_WindowsUser_CheckedChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            _button_BrowseUsers.Enabled = ((RadioButton)sender).Checked;

            if (((RadioButton)sender).Checked)
            {
                if (m_loginType != Sql.LoginType.WindowsLogin)
                {
                    // try to be smart and not clear the user if it was not validated to type previously
                    if (m_user != null && m_user.Sid != null)
                    {
                        _textBox_User.Text = string.Empty;
                        m_user = null;
                    }
                    m_loginType = Sql.LoginType.WindowsLogin;
                }
            }

            Cursor = Cursors.Default;
        }

        /// <summary>
        /// SQLsecure 3.1 (Anshul Aggarwal) - Initializes user for Azure AD Account.
        /// </summary>
        private void _radioButton_AzureADUserOrGroup_CheckedChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            _button_BrowseUsers.Enabled = !((RadioButton)sender).Checked;
            if (((RadioButton)sender).Checked)
            {
                if (m_loginType != Sql.LoginType.AzureADAccount)
                {
                    // try to be smart and not clear the user if it was not validated to type previously
                    if (m_user != null && m_user.Sid != null)
                    {
                        _textBox_User.Text = string.Empty;
                        m_user = null;
                    }
                    m_loginType = Sql.LoginType.AzureADAccount;
                }
            }
            checkSelections();
            Cursor = Cursors.Default;
        }
        
        private void _textBox_User_TextChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            m_user = null;

            checkSelections();

            Cursor = Cursors.Default;
        }

        private void _comboBox_Server_DropDown(object sender, EventArgs e)
        {
            //Initialize the combobox for server selection to refresh on every open
            string selection = _comboBox_Server.Text;
            _comboBox_Server.Items.Clear();
            _comboBox_Server.Items.Add(Utility.Constants.ReportSelect_AllServers);

            _connectionNameToServer.Clear();    // SQLsecure 3.1 (Anshul Aggarwal) - Clear the old mapping.

            foreach (Sql.RegisteredServer server in Program.gController.ReportPolicy.GetMemberServers())
            {
                _comboBox_Server.Items.Add(server.ConnectionName);

                if(!_connectionNameToServer.ContainsKey(server.ConnectionName)) // SQLsecure 3.1 (Anshul Aggarwal) - Populate the new mapping.
                    _connectionNameToServer.Add(server.ConnectionName, server);
            }

            //Keep the last selection for the user
            _comboBox_Server.Text = selection;
        }

        private void _comboBox_Server_SelectionChangeCommitted(object sender, EventArgs e)
        {
            RefreshLoginTypesRadioButtons(); // Change login types radio buttons based on server type.
            checkSelections();
        }

        /// <summary>
        /// SQLsecure 3.1 (Anshul Aggarwal) - Change login types radio buttons based on server type.
        /// </summary>
        private void RefreshLoginTypesRadioButtons()
        {
            string connectionname = _comboBox_Server.SelectedItem as string;
            if (connectionname != null)
            {
                if (connectionname == Utility.Constants.ReportSelect_AllServers)
                {
                    this.SuspendLayout();  // Suspend changes.

                    this._radioButton_WindowsUser.Location = WINDOWS_RADIO_BUTTON_LOCATION;
                    this._radioButton_SQLLogin.Location = SQL_RADIO_BUTTON_LOCATION;
                    this._radioButton_AzureADUserOrGroup.Location = AZURE_RADIO_BUTTON_LOCATION;
                    this._radioButton_WindowsUser.Visible = true;
                    this._radioButton_SQLLogin.Visible = true;
                    this._radioButton_AzureADUserOrGroup.Visible = true;

                    this.ResumeLayout();  // Resume changes.
                }
                else if (_connectionNameToServer.ContainsKey(connectionname))
                {
                    var server = _connectionNameToServer[connectionname];
                    if (server.ServerType == ServerType.AzureSQLDatabase)
                    {
                        this.SuspendLayout();  // Suspend changes.

                        this._radioButton_AzureADUserOrGroup.Location = WINDOWS_RADIO_BUTTON_LOCATION;
                        this._radioButton_SQLLogin.Location = AZURE_RADIO_BUTTON_LOCATION;
                        this._radioButton_WindowsUser.Visible = false;
                        this._radioButton_SQLLogin.Visible = true;
                        this._radioButton_AzureADUserOrGroup.Visible = true;

                        this.ResumeLayout();  // Resume changes.

                        // Change selected button if it does not exist for current server type.
                        if (_radioButton_WindowsUser.Checked)
                        {
                            m_user = null;
                            _radioButton_AzureADUserOrGroup.Checked = true;
                        }
                    }
                    else
                    {
                        this.SuspendLayout(); // Suspend changes.

                        this._radioButton_WindowsUser.Location = WINDOWS_RADIO_BUTTON_LOCATION;
                        this._radioButton_SQLLogin.Location = AZURE_RADIO_BUTTON_LOCATION;
                        this._radioButton_WindowsUser.Visible = true;
                        this._radioButton_SQLLogin.Visible = true;
                        this._radioButton_AzureADUserOrGroup.Visible = false;

                        this.ResumeLayout();  // Resume changes.

                       
                        if (_radioButton_AzureADUserOrGroup.Checked)
                        {
                            m_user = null;
                            _radioButton_WindowsUser.Checked = true;
                        }
                    }
                }
            }
        }

        private void _comboBox_PermissionType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            m_permissionType = (_comboBox_PermissionType.SelectedItem.ToString() == PermissionTypeEffective ?
                                            Utility.Permissions.Type.Effective :
                                            Utility.Permissions.Type.Explicit);
            checkSelections();
        }

        #endregion
    }
}