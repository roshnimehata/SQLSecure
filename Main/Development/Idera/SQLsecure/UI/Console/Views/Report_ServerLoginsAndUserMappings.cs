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
    public partial class Report_ServerLoginsAndUserMappings : Idera.SQLsecure.UI.Console.Controls.BaseReport, Interfaces.IView
    {
        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Views.Report_ServerLoginsAndUserMappings");

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

        private Sql.User m_user;

        // SQLsecure 3.1 (Anshul Aggarwal) - Dictionary that maps connection name to RegisteredServer so we can extract ServerType based on server dropdown.
        private Dictionary<string, RegisteredServer> _connectionNameToServer = new Dictionary<string, RegisteredServer>();

        #endregion

        #region Ctors

        public Report_ServerLoginsAndUserMappings()
        {
            InitializeComponent();

            //set the global title, the report title in the header graphic, and the title in the
            //status bar
            m_Title                     =
                _label_ReportTitle.Text =
                _label_Status.Text      = Utility.Constants.ReportTitle_ServerLoginsAndUserMappings;

            //set the description and getting started text
            _label_Description.Text = Utility.Constants.ReportSummary_ServerLoginsAndUserMappings;

            int i = 1;
            StringBuilder instructions = new StringBuilder(Utility.Constants.ReportRunInstructions_MultiStep);
            instructions.Append(newline);
            instructions.AppendFormat(instructionformat, i++, Utility.Constants.ReportRunInstructions_UseSelection, newline);
            instructions.AppendFormat(instructionformat, i++, Utility.Constants.ReportRunInstructions_Server_OP_OR_ADB, newline);
            instructions.AppendFormat(instructionformat, i++, Utility.Constants.ReportRunInstructions_Database, newline);
            instructions.AppendFormat(instructionformat, i++, Utility.Constants.ReportRunInstructions_LoginType, newline);
            instructions.AppendFormat(instructionformat, i++, Utility.Constants.ReportRunInstructions_UserName, newline);
            instructions.AppendFormat(instructionformat, i, Utility.Constants.ReportRunInstructions_NoParameters, newline);
            _label_Instructions.Text = instructions.ToString();

            _reportViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubreportProcessingEventHandler);

            _button_RunReport.Enabled = false;

            //start off with the default value for servers
            _comboBox_Server.Items.Clear();
            _comboBox_Server.Items.Add(Utility.Constants.ReportSelect_AllServers);
            _comboBox_Server.Text = Utility.Constants.ReportSelect_AllServers;
            
            PopulateLoginTypesDropdown(Utility.Constants.ReportSelect_AllServers);  // SQLsecure 3.1 (Anshul Aggarwal) - Add login types for 'All Servers'
            _comboBox_Login.SelectedItem = Utility.Constants.ReportSelect_LoginTypes_All;

            checkSelections();
        }

        #endregion

        #region Queries & Constants

        // Main Report
        private const string QueryDataSource = @"SQLsecure.dbo.isp_sqlsecure_report_getuserinfo";
        private const string DataSourceName = @"ReportsDataset_isp_sqlsecure_report_getuserinfo";

        private const string SqlColumnServer = "connectionname";
        private const string SqlColumnLogin = "loginname";
        private const string SqlColumnLoginType = "logintype";
        private const string SqlColumnDatabase = "databasename";

        private const string SqlValueWindowsUser = "Windows User";
        private const string SqlValueWindowsGroup = "Windows Group";
        private const string SqlValueSQLLogin = "SQL Login";

        private const string SqlValueAzureAccounts = "Azure AD*";   // SQLsecure 3.1 (Anshul Aggarwal) - Add support for Azure Users and Groups.
        private const string SqlValueAzureAGUser = "Azure AD User";
        private const string SqlValueAzureADGroup = "Azure AD Group";

        #endregion

        #region Helpers

        protected internal override void checkSelections()
        {
            _button_RunReport.Enabled = false;

            //they need to pick a server
            if (_comboBox_Server.SelectedItem == null) return;

            //they also need to enter a user name
            _textBox_Login.Text = _textBox_Login.Text.Trim();
            if (_textBox_Login.Text.Length == 0) _textBox_Login.Text = "*";

            //and a database
            _textBox_Database.Text = _textBox_Database.Text.Trim();
            if (_textBox_Database.Text.Length == 0) _textBox_Database.Text = "*";

            //and choose a login type
            if (_comboBox_Login.SelectedItem == null) return;

            _button_RunReport.Enabled = true;
        }

        #region SubReport

        void SubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Clear();
            e.DataSources.Add(getServerListDataSet());
        }

        #endregion

        protected internal override void runReport()
        {
            base.runReport();

            logX.loggerX.Info(@"Retrieve data for report Server Database Users");

            try
            {
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Setup stored procedure
                    SqlCommand cmd = new SqlCommand(QueryDataSource, connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Build parameters
                    SqlParameter paramRunDate = new SqlParameter(SqlParamRunDate, m_reportDate);
                    SqlParameter paramPolicyid = new SqlParameter(SqlParamPolicyid, m_policyid);
                    SqlParameter paramServer = new SqlParameter(SqlParamServerName2, getServerName(_comboBox_Server.Text));
                    SqlParameter paramUseBaseline = new SqlParameter(SqlParamUsebaseline, m_useBaseline);

                    cmd.Parameters.Add(paramRunDate);
                    cmd.Parameters.Add(paramPolicyid);
                    cmd.Parameters.Add(paramServer);
                    cmd.Parameters.Add(paramUseBaseline);

                    // Get data
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    string loginTypeFilter = GetLoginTypeFilter();
                    string loginFilter = GetLoginFilter();
                    string databaseFilter = GetDatabaseFilter();

                    StringBuilder filter = new StringBuilder();
                    
                    if (!string.IsNullOrEmpty(loginTypeFilter))
                    {
                        if (filter.Length > 0)
                        {
                            filter.Append(string.Format(" and {0}", loginTypeFilter));
                        }
                        else
                        {
                            filter.Append(loginTypeFilter);
                        }
                    }
                    if (!string.IsNullOrEmpty(loginFilter))
                    {
                        if (filter.Length > 0)
                        {
                            filter.Append(string.Format(" and {0}", loginFilter));
                        }
                        else
                        {
                            filter.Append(loginFilter);
                        }
                    }
                    if (!string.IsNullOrEmpty(databaseFilter))
                    {
                        if (filter.Length > 0)
                        {
                            filter.Append(string.Format(" and {0}", databaseFilter));
                        }
                        else
                        {
                            filter.Append(databaseFilter);
                        }
                    }

                    ds.Tables[0].DefaultView.RowFilter = filter.ToString();                    

                    ReportDataSource rds = new ReportDataSource();
                    rds.Name = DataSourceName;
                    rds.Value = ds.Tables[0].DefaultView;
                    _reportViewer.LocalReport.DataSources.Clear();
                    _reportViewer.LocalReport.DataSources.Add(rds);
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error(@"Error - Unable to get report data", ex);
                MsgBox.ShowError(m_Title, ErrorMsgs.CantGetReportData, ex);
            }

            //add report parameters
            ReportParameter[] Param = new ReportParameter[7];
            Param[0] = new ReportParameter("ReportTitle", string.Format(Utility.Constants.REPORTS_TITLE_STR, m_Title));
            Param[1] = new ReportParameter("UserRange", m_reportDateDisplay);
            Param[2] = new ReportParameter("userName", _textBox_Login.Text);
            Param[3] = new ReportParameter("usertype", GetUserTypeValue());
            Param[4] = new ReportParameter("serverName", _comboBox_Server.Text);
            Param[5] = new ReportParameter("databaseName", _textBox_Database.Text);
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

        /// <summary>
        /// SQLsecure 3.1 (Anshul Aggarwal) - Pupulate Login Types Dropdown based on selected server dropdown value.
        /// </summary>
        private void PopulateLoginTypesDropdown(string serverDropdownValue)
        {
            if (serverDropdownValue == Utility.Constants.ReportSelect_AllServers)
            {
                _comboBox_Login.Items.Clear();
                _comboBox_Login.Items.Add(Utility.Constants.ReportSelect_LoginTypes_All);
                _comboBox_Login.Items.Add(Utility.Constants.ReportSelect_LoginTypes_AllWindows);
                _comboBox_Login.Items.Add(Utility.Constants.ReportSelect_LoginTypes_WindowsUsers);
                _comboBox_Login.Items.Add(Utility.Constants.ReportSelect_LoginTypes_WindowsGroup);
                _comboBox_Login.Items.Add(Utility.Constants.ReportSelect_LoginTypes_AzureADAccounts);
                _comboBox_Login.Items.Add(Utility.Constants.ReportSelect_LoginTypes_AzureADUser);
                _comboBox_Login.Items.Add(Utility.Constants.ReportSelect_LoginTypes_AzureADGroup);
                _comboBox_Login.Items.Add(Utility.Constants.ReportSelect_LoginTypes_SQLLogins);
            }
            else if (_connectionNameToServer.ContainsKey(serverDropdownValue))
            {
                var server = _connectionNameToServer[serverDropdownValue];
                _comboBox_Login.Items.Clear();
                _comboBox_Login.Items.Add(Utility.Constants.ReportSelect_LoginTypes_All);
                if (server.ServerType == ServerType.AzureSQLDatabase)
                {
                    _comboBox_Login.Items.Add(Utility.Constants.ReportSelect_LoginTypes_AzureADAccounts);
                    _comboBox_Login.Items.Add(Utility.Constants.ReportSelect_LoginTypes_AzureADUser);
                    _comboBox_Login.Items.Add(Utility.Constants.ReportSelect_LoginTypes_AzureADGroup);
                }
                else
                {
                    _comboBox_Login.Items.Add(Utility.Constants.ReportSelect_LoginTypes_AllWindows);
                    _comboBox_Login.Items.Add(Utility.Constants.ReportSelect_LoginTypes_WindowsUsers);
                    _comboBox_Login.Items.Add(Utility.Constants.ReportSelect_LoginTypes_WindowsGroup);
                }
                _comboBox_Login.Items.Add(Utility.Constants.ReportSelect_LoginTypes_SQLLogins);
            }
        }
        #endregion

        #region Filters

        private string GetLoginFilter()
        {
            string filter = _textBox_Login.Text;
            
            if (string.IsNullOrEmpty(filter))
            {
                return string.Empty;
            }
            
            if (Sql.SqlHelper.SqlInjectionChars(filter))
            {
                Utility.MsgBox.ShowError("Login Filter", Utility.ErrorMsgs.NameMatchInvalidCharsMsg);
                return string.Empty;
            }

            filter = filter.Replace("*", "%");
            filter = filter.Replace("?", "_");
            
            return string.Format("{0} like '{1}'", SqlColumnLogin, filter);
        }

        private string GetDatabaseFilter()
        {
            string filter = _textBox_Database.Text;
            
            if (string.IsNullOrEmpty(filter))
            {
                return string.Empty;
            }

            if (Sql.SqlHelper.SqlInjectionChars(filter))
            {
                Utility.MsgBox.ShowError("Database Filter", Utility.ErrorMsgs.NameMatchInvalidCharsMsg);
                return string.Empty;
            }

            filter = filter.Replace("*", "%");
            filter = filter.Replace("?", "_");

            return string.Format("{0} like '{1}'", SqlColumnDatabase, filter);
        }

        private string GetUserTypeValue()
        {
            string value = string.Empty;

            string LoginType = _comboBox_Login.SelectedItem.ToString();

            switch (LoginType)
            {
                case Utility.Constants.ReportSelect_LoginTypes_All:
                    value = "*";
                    break;
                case Utility.Constants.ReportSelect_LoginTypes_AllWindows:
                    value = "Windows*";
                    break;
                case Utility.Constants.ReportSelect_LoginTypes_WindowsUsers:
                    value = SqlValueWindowsUser;
                    break;
                case Utility.Constants.ReportSelect_LoginTypes_WindowsGroup:
                    value = SqlValueWindowsGroup;
                    break;
                case Utility.Constants.ReportSelect_LoginTypes_SQLLogins:
                    value = SqlValueSQLLogin;
                    break;
                case Utility.Constants.ReportSelect_LoginTypes_AzureADAccounts:  // SQLsecure 3.1 (Anshul Aggarwal) - Add support for Azure Users and Groups.
                    value = SqlValueAzureAccounts;
                    break;
                case Utility.Constants.ReportSelect_LoginTypes_AzureADUser:
                    value = SqlValueAzureAGUser;
                    break;
                case Utility.Constants.ReportSelect_LoginTypes_AzureADGroup:
                    value = SqlValueAzureADGroup;
                    break;
            }

            return value;
        }

        private string GetLoginTypeFilter()
        {
            string loginType = GetUserTypeValue();
            string filter = string.Empty;

            if (loginType.CompareTo("*") != 0) 
            {
                filter = string.Format("{0} like '{1}'", SqlColumnLoginType, loginType);
            }
            
            return filter;
        }

        #endregion

        #region Events

        private void _comboBox_Server_DropDown(object sender, EventArgs e)
        {
            //Initialize the combobox for server selection to refresh on every open
            string selection = _comboBox_Server.Text;
            _comboBox_Server.Items.Clear();
            _comboBox_Server.Items.Add(Utility.Constants.ReportSelect_AllServers);
            _connectionNameToServer.Clear();     // SQLsecure 3.1 (Anshul Aggarwal) - Repopulate the dictionary when server dropdown populates.

            foreach (Sql.RegisteredServer server in Program.gController.ReportPolicy.GetMemberServers())
            {
                _comboBox_Server.Items.Add(server.ConnectionName);

                // SQLsecure 3.1 (Anshul Aggarwal) - Add support for Azure Users and Groups.
                if (!_connectionNameToServer.ContainsKey(server.ConnectionName))
                    _connectionNameToServer.Add(server.ConnectionName, server);
            }

            //Keep the last selection for the user
            _comboBox_Server.Text = selection;
        }

        private void _comboBox_Server_SelectionChangeCommitted(object sender, EventArgs e)
        {
            RefreshLoginTypesDropdown(); // SQLsecure 3.1 (Anshul Aggarwal) - Change login types dropdown based on type of server.
            checkSelections();
        }

        /// <summary>
        /// /SQLsecure 3.1 (Anshul Aggarwal) - Change login types dropdown based on type of server.
        /// </summary>
        private void RefreshLoginTypesDropdown()
        {
            string connectionname = _comboBox_Server.SelectedItem as string;
            if (connectionname != null)
            {
                _comboBox_Login.SuspendLayout();    // Suspend changes to dropdown

                string selectedLoginType = _comboBox_Login.SelectedItem as string;
                PopulateLoginTypesDropdown(connectionname); // Populate login types dropdown.
                
                // If current selected type is not found in the possible dropdown values, change to 'All Types'.
                if (!_comboBox_Login.Items.Contains(selectedLoginType))
                {
                    _comboBox_Login.SelectedItem = Utility.Constants.ReportSelect_LoginTypes_All;
                }
                else
                {
                    _comboBox_Login.SelectedItem = selectedLoginType;
                }

                SetUsersButtonState(); 
                _comboBox_Login.ResumeLayout(); // Resume changes to dropdown
            }
        }

        private void _comboBox_Login_SelectionChangeCommitted(object sender, EventArgs e)
        {
            checkSelections();
            SetUsersButtonState();   
        }

        /// <summary>
        /// SQLsecure 3.1 (Anshul Aggarwal) - Enables/Disables Browser Users Button based on type of login.
        /// </summary>
        private void SetUsersButtonState()
        {
            //enable or disable the browse for users button
            _button_BrowseUsers.Enabled = (
                ((_comboBox_Login.Text).CompareTo(Idera.SQLsecure.UI.Console.Utility.Constants.ReportSelect_LoginTypes_AllWindows) == 0) ||
                ((_comboBox_Login.Text).CompareTo(Idera.SQLsecure.UI.Console.Utility.Constants.ReportSelect_LoginTypes_WindowsUsers) == 0) ||
                ((_comboBox_Login.Text).CompareTo(Idera.SQLsecure.UI.Console.Utility.Constants.ReportSelect_LoginTypes_WindowsGroup) == 0));
        }

        private void _textBox_User_TextChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            m_user = null;

            checkSelections();

            Cursor = Cursors.Default;
        }

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
                        _textBox_Login.Text = results[i].SamAccountName;
                        m_user = new Sql.User(results[i].SamAccountName,
                                                results[i].Sid,
                                                Sql.LoginType.WindowsLogin,
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

        #endregion
    }
}