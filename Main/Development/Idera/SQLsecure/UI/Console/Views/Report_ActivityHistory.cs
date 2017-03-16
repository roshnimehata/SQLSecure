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

namespace Idera.SQLsecure.UI.Console.Views
{
    public partial class Report_ActivityHistory : Idera.SQLsecure.UI.Console.Controls.BaseReport, Interfaces.IView
    {
        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Views.ActivityHistoryReport"); 

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

        #region Ctors

        public Report_ActivityHistory()
        {
            InitializeComponent();

            //set the global title, the report title in the header graphic, and the title in the
            //status bar
            m_Title                     =
                _label_ReportTitle.Text =
                _label_Status.Text      = Utility.Constants.ReportTitle_ActivityHistory;

            //set the description and getting started text
            _label_Description.Text = Utility.Constants.ReportSummary_ActivityHistory;

            int i = 1;
            StringBuilder instructions = new StringBuilder(Utility.Constants.ReportRunInstructions_MultiStep);
            instructions.Append(newline);
            instructions.AppendFormat(instructionformat, i++, Utility.Constants.ReportRunInstructions_Select_Target_Instance, newline);
            instructions.AppendFormat(instructionformat, i++, Utility.Constants.ReportRunInstructions_StartDate, newline);
            instructions.AppendFormat(instructionformat, i++, Utility.Constants.ReportRunInstructions_EndDate, newline);
            instructions.AppendFormat(instructionformat, i++, Utility.Constants.ReportRunInstructions_ActivityType, newline);
            instructions.AppendFormat(instructionformat, i++, Utility.Constants.ReportRunInstructions_UserName, newline);
            instructions.AppendFormat(instructionformat, i, Utility.Constants.ReportRunInstructions_NoParameters, newline);
            _label_Instructions.Text = instructions.ToString();

            _button_RunReport.Enabled = true;

            //start off some default values
            _comboBox_Server.Items.Add(Utility.Constants.ReportSelect_AllServers);
            _comboBox_Server.Text = Utility.Constants.ReportSelect_AllServers;

            _textBox_User.Text = "*";

            _comboBox_ActivityType.Items.Clear();
            _comboBox_ActivityType.Items.Add("All");
            _comboBox_ActivityType.Items.Add(Utility.Activity.TypeAuditSuccess);
            _comboBox_ActivityType.Items.Add(Utility.Activity.TypeAuditFailure);
            _comboBox_ActivityType.Items.Add(Utility.Activity.TypeInfo);
            _comboBox_ActivityType.Items.Add(Utility.Activity.TypeWarning);
            _comboBox_ActivityType.Items.Add(Utility.Activity.TypeError);
            _comboBox_ActivityType.SelectedItem = "All";

            _dateTime_End.Value = DateTime.UtcNow.Date.AddDays(1).AddSeconds(-1);
            _dateTime_Start.Value = ((DateTime)_dateTime_End.Value).AddDays(-14).AddSeconds(1);
        }

        #endregion

        #region Queries & Constants

        // Main Report
        private const string QueryDataSource = @"SQLsecure.dbo.isp_sqlsecure_report_getactivityhistory";
        private const string DataSourceName = @"ReportsDataset_isp_sqlsecure_report_getactivityhistory";

        private const string SqlParamServerName1 = @"@server";

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

            //and an activity type
            if (_comboBox_ActivityType.Text.Length == 0) return;

            _button_RunReport.Enabled = true;
        }

        protected internal override void runReport()
        {
            base.runReport();

            logX.loggerX.Info(@"Retrieve data for report Activity History");

            string statusValue = _comboBox_ActivityType.Text.Trim();

            switch (statusValue)
            {
                case Utility.Activity.TypeAuditSuccess:
                    statusValue = "Success%";
                    break;
                case Utility.Activity.TypeAuditFailure:
                    statusValue = "Failure%";
                    break;
                case "All":
                    statusValue = "%";
                    break;
            }

            string userValue = _textBox_User.Text.Trim();
            if (userValue.CompareTo("*") == 0) userValue = "%";

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
                    SqlParameter paramServer = new SqlParameter(SqlParamServerName1, getServerName(_comboBox_Server.Text));
                    SqlParameter paramStatus = new SqlParameter(SqlParamStatus, statusValue);
                    SqlParameter paramStart = new SqlParameter(SqlParamStart, (((DateTime)_dateTime_Start.Value).ToString(Utility.Constants.PARAM_DATETIME_FORMAT)));
                    SqlParameter paramEnd = new SqlParameter(SqlParamEnd, (((DateTime)_dateTime_End.Value).ToString(Utility.Constants.PARAM_DATETIME_FORMAT)));
                    SqlParameter paramlogin = new SqlParameter(SqlParamLogin, userValue);
                    SqlParameter paramPolicyid = new SqlParameter(SqlParamPolicyid, Program.gController.ReportPolicy.PolicyId);

                    cmd.Parameters.Add(paramServer);
                    cmd.Parameters.Add(paramStatus);
                    cmd.Parameters.Add(paramStart);
                    cmd.Parameters.Add(paramEnd);
                    cmd.Parameters.Add(paramlogin);
                    cmd.Parameters.Add(paramPolicyid);

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
            }

            //add report parameters
            ReportParameter[] Param = new ReportParameter[7];
            Param[0] = new ReportParameter("ReportTitle", string.Format(Utility.Constants.REPORTS_TITLE_STR, m_Title));
            Param[1] = new ReportParameter("startdate", _dateTime_Start.Value.ToString());
            Param[2] = new ReportParameter("enddate", _dateTime_End.Value.ToString());
            Param[3] = new ReportParameter("serverName", _comboBox_Server.Text);
            Param[4] = new ReportParameter("status", statusValue);
            Param[5] = new ReportParameter("loginName", userValue);
            Param[6] = new ReportParameter("Expand_All", _isExpanded.ToString());            

            _reportViewer.LocalReport.EnableHyperlinks = true;
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

        private void _textBox_User_TextChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

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
                        _textBox_User.Text = results[i].SamAccountName;
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