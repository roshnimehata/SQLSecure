using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Idera.SQLsecure.Core.Accounts;
using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Utility;

namespace Idera.SQLsecure.UI.Console.Views
{
    public partial class Report_ServersWithDangerousGroups : Idera.SQLsecure.UI.Console.Controls.BaseReport, Interfaces.IView
    {
        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Views.Report_ServersWithDangerousGroups");
        
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

        public Report_ServersWithDangerousGroups()
        {
            InitializeComponent();

            //set the global title, the report title in the header graphic, and the title in the
            //status bar
            m_Title                     =
                _label_ReportTitle.Text =
                _label_Status.Text      = Utility.Constants.ReportTitle_ServersWithDangerousGroups;

            //set the description and getting started text
            _label_Description.Text = Utility.Constants.ReportSummary_ServersWithDangerousGroups;

            int i = 1;
            StringBuilder instructions = new StringBuilder(Utility.Constants.ReportRunInstructions_MultiStep);
            instructions.Append(newline);
            instructions.AppendFormat(instructionformat, i++, Utility.Constants.ReportRunInstructions_UseSelection, newline);
            instructions.AppendFormat(instructionformat, i++, Utility.Constants.ReportRunInstructions_Server, newline);
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
        private const string QueryDataSource = @"SQLsecure.dbo.isp_sqlsecure_report_checkwellknowngroup";
        private const string DataSourceName = @"ReportsDataset_isp_sqlsecure_report_checkwellknowngroup";
       
        #endregion

        #region Helpers

        protected internal override void checkSelections()
        {
            _button_RunReport.Enabled = false;

            //they need to pick a server
            if (_comboBox_Server.Text.Length == 0) return;

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

            logX.loggerX.Info(@"Retrieve data for report Everyone Group Access");

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
                    // Build parameters
                    SqlParameter paramRunDate = new SqlParameter(SqlParamRunDate, m_reportDate);
                    SqlParameter paramPolicyid = new SqlParameter(SqlParamPolicyid, m_policyid);
                    SqlParameter paramServerName = new SqlParameter(SqlParamServerName2, getServerName(_comboBox_Server.Text));
                    SqlParameter paramUseBaseline = new SqlParameter(SqlParamUsebaseline, m_useBaseline);

                    cmd.Parameters.Add(paramRunDate);
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
            }

            //add report parameters
            ReportParameter[] Param = new ReportParameter[4];
            Param[0] = new ReportParameter("ReportTitle", string.Format(Utility.Constants.REPORTS_TITLE_STR, m_Title));
            Param[1] = new ReportParameter("UserRange", m_reportDateDisplay);
            Param[2] = new ReportParameter("serverName", _comboBox_Server.Text);
            Param[3] = new ReportParameter("Expand_All", _isExpanded.ToString());

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
                if (server.ServerType == ServerType.OnPremise || server.ServerType == ServerType.SQLServerOnAzureVM) // SQLsecure 3.1 (Anshul) - Not applicable to ADB.
                    _comboBox_Server.Items.Add(server.ConnectionName);
            }

            //Keep the last selection for the user
            _comboBox_Server.Text = selection;
        }

        private void _comboBox_Server_SelectionChangeCommitted(object sender, EventArgs e)
        {
            checkSelections();
        }

        #endregion
    }
}