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
    public partial class Report_CrossServerLoginCheck : Idera.SQLsecure.UI.Console.Controls.BaseReport, Interfaces.IView
    {
        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Views.Report_CrossServerLoginCheck");
        
        #region IView

        void Interfaces.IView.SetContext(Interfaces.IDataContext contextIn)
        {
            Data.Report context = (Data.Report)contextIn;
            checkSelections();
            
            if (context.RunReport && _button_RunReport.Enabled)
            {
                runReport();
            }
        }

        #endregion

        #region Fields

        private string m_loginType;
        private Sql.User m_user;

        #endregion

        #region Ctors

        public Report_CrossServerLoginCheck()
        {
            InitializeComponent();

            m_loginType = Sql.LoginType.WindowsLogin;
            _radioButton_WindowsUser.Checked = true;

            //set the global title, the report title in the header graphic, and the title in the
            //status bar
            m_Title                     =
                _label_ReportTitle.Text =
                _label_Status.Text      = Utility.Constants.ReportTitle_CrossServerLoginCheck;

            //set the description and getting started text
            _label_Description.Text = Utility.Constants.ReportSummary_CrossServerLoginCheck;

            int i = 1;
            StringBuilder instructions = new StringBuilder(Utility.Constants.ReportRunInstructions_MultiStep);
            instructions.Append(newline);
            instructions.AppendFormat(instructionformat, i++, Utility.Constants.ReportRunInstructions_UseSelection, newline);
            instructions.AppendFormat(instructionformat, i++, Utility.Constants.ReportRunInstructions_Server, newline);
            instructions.AppendFormat(instructionformat, i++, Utility.Constants.ReportRunInstructions_LoginType, newline);
            instructions.AppendFormat(instructionformat, i++, Utility.Constants.ReportRunInstructions_UserName, newline);
            instructions.AppendFormat(instructionformat, i, Utility.Constants.ReportRunInstructions_NoParameters, newline);
            _label_Instructions.Text = instructions.ToString();

            _reportViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubreportProcessingEventHandler);

            _button_RunReport.Enabled = true;

            //start off with the default value
            _comboBox_Server.Items.Add(Utility.Constants.ReportSelect_AllServers);
            _comboBox_Server.Text = Utility.Constants.ReportSelect_AllServers;
        }
        
        #endregion

        #region Queries & Constants

        // Main Report
        private const string QueryDataSource = @"SQLsecure.dbo.isp_sqlsecure_report_getuserserveraccess";
        private const string DataSourceName = @"ReportsDataset_isp_sqlsecure_report_getuserserveraccess";

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

            logX.loggerX.Info(@"Retrieve data for report Server Access");

            if (m_user == null)
            {
                if (m_loginType.Equals(Sql.LoginType.SqlLogin))
                {
                    // if it is a SQL Login, it will not be validated, so just create the m_user without a sid
                    m_user = new Idera.SQLsecure.UI.Console.Sql.User(_textBox_User.Text, null, m_loginType, Sql.User.UserSource.UserEntry);
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
                    SqlCommand cmd = new SqlCommand(QueryDataSource, connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Build parameters
                    SqlParameter paramLoginType = new SqlParameter(SqlParamLoginType, m_loginType);
                    SqlParameter paramSqlLogin = new SqlParameter(SqlParamUser, m_user.Name);
                    SqlParameter paramRunDate = new SqlParameter(SqlParamRunDate, m_reportDate);
                    SqlParameter paramPolicyid = new SqlParameter(SqlParamPolicyid, m_policyid);
                    SqlParameter paramServerName = new SqlParameter(SqlParamServerName, getServerName(_comboBox_Server.Text));
                    SqlParameter paramUseBaseline = new SqlParameter(SqlParamUsebaseline, m_useBaseline);
                    cmd.Parameters.Add(paramRunDate);
                    cmd.Parameters.Add(paramLoginType);
                    cmd.Parameters.Add(paramSqlLogin);
                    cmd.Parameters.Add(paramPolicyid);
                    cmd.Parameters.Add(paramServerName);
                    cmd.Parameters.Add(paramUseBaseline);

                    // Get data
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    ReportDataSource rds = new ReportDataSource();
                    rds.Name = DataSourceName;
                    rds.Value = ds.Tables[0];
                    _reportViewer.LocalReport.DataSources.Clear();
                    _reportViewer.LocalReport.DataSources.Add(rds);
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error(@"Error - Unable to get report data", ex);

                MsgBox.ShowError(m_Title, ErrorMsgs.CantGetReportData, ex);
                
                return;
            }

            //now show the report panel
            base.runReport();

            //add report parameters
            ReportParameter[] Param = new ReportParameter[6];
            Param[0] = new ReportParameter("ReportTitle", string.Format(Utility.Constants.REPORTS_TITLE_STR, m_Title));
            Param[1] = new ReportParameter("UserRange", m_reportDateDisplay);
            Param[2] = new ReportParameter("userName", m_user.Name);
            Param[3] = new ReportParameter("logintype", m_loginType);
            Param[4] = new ReportParameter("serverName", _comboBox_Server.Text);
            Param[5] = new ReportParameter("Expand_All", _isExpanded.ToString());

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

        #endregion

        #region Events

        private void _comboBox_Server_DropDown(object sender, EventArgs e)
        {
            //Initialize the combobox for server selection to refresh on every open
            string selection = _comboBox_Server.Text;
            _comboBox_Server.Items.Clear();
            _comboBox_Server.Items.Add(Utility.Constants.ReportSelect_AllServers);

            foreach (Sql.RegisteredServer server in Program.gController.ReportPolicy.GetMemberServers())
            {
                _comboBox_Server.Items.Add(server.ConnectionName);
            }

            //Keep the last selection for the user
            _comboBox_Server.Text = selection;
        }

        private void _comboBox_Server_SelectionChangeCommitted(object sender, EventArgs e)
        {
            checkSelections(); 
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

        private void _radioButton_SQLLogin_Click(object sender, EventArgs e)
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

            Cursor = Cursors.Default;
        }

        private void _radioButton_WindowsUser_Click(object sender, EventArgs e)
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

        private void _textBox_User_TextChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            m_user = null;

            checkSelections();

            Cursor = Cursors.Default;
        }

        #endregion
    }
}