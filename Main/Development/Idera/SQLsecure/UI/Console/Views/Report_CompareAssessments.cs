using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Infragistics.Win;

using Idera.SQLsecure.Core.Accounts;
using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Utility;
using Infragistics.Win.UltraWinEditors;

namespace Idera.SQLsecure.UI.Console.Views
{
    public partial class Report_CompareAssessments : Idera.SQLsecure.UI.Console.Controls.BaseReport, Interfaces.IView
    {
        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Views.Report_CompareAssessments");

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

        private bool selectionChanged = false;

        #endregion

        #region Ctors

        public Report_CompareAssessments()
        {
            InitializeComponent();
            
            //set the global title, the report title in the header graphic, and the title in the
            //status bar
            m_Title                     =
                _label_ReportTitle.Text =
                _label_Status.Text      = Utility.Constants.ReportTitle_CompareAssessments;

            //set the description and getting started text
            _label_Description.Text = Utility.Constants.ReportSummary_CompareAssessments;

            int i = 1;
            StringBuilder instructions = new StringBuilder(Utility.Constants.ReportRunInstructions_MultiStep);
            instructions.Append(newline);
            instructions.AppendFormat(instructionformat, i++, Utility.Constants.ReportRunInstructions_UsePolicy, newline);
            instructions.AppendFormat(instructionformat, i++, Utility.Constants.ReportRunInstructions_AssessmentsToCompare, newline);
            instructions.AppendFormat(instructionformat, i++, Utility.Constants.ReportRunInstructions_Server, newline);
            instructions.AppendFormat(instructionformat, i++, Utility.Constants.ReportRunInstructions_ShowDifferencesOnly, newline);
            instructions.AppendFormat(instructionformat, i, Utility.Constants.ReportRunInstructions_NoParameters, newline);
            _label_Instructions.Text = instructions.ToString();

            _checkbox_showDiffsOnly.Checked = true;
            _button_RunReport.Enabled = false;
        }
        
        #endregion

        #region Queries & Constants

        private const string QueryDataSource1 = @"SQLsecure.dbo.isp_sqlsecure_getassessmentcomparison";
        private const string DataSourceName1 = @"ReportsDataset_isp_sqlsecure_report_getassessmentcomparison";

        private const string QueryDataSource2 = @"SQLsecure.dbo.isp_sqlsecure_getpolicyserver";
        private const string DataSourceName2 = @"ReportsDataset_isp_sqlsecure_getpolicyserver";

        private const string QueryDataSource5 = @"SQLsecure.dbo.isp_sqlsecure_getpolicyserver";
        private const string DataSourceName5 = @"ServerSummaryAssessment2";

        private const string QueryDataSource3 = @"SQLsecure.dbo.isp_sqlsecure_report_getpolicyinfo";
        private const string DataSourceName3 = @"ReportsDataset_isp_sqlsecure_report_getpolicyinfo";

        private const string QueryDataSource4 = @"SQLsecure.dbo.isp_sqlsecure_report_getpolicyinfo";
        private const string DataSourceName4 = @"ReportsDataset_isp_sqlsecure_report_getpolicyinfo2";

        private const string ComboTextChooseAssessment = "<Choose an assessment to compare>";
        private const string CurrentAssesment = "Policy Assessment";
        private const string DraftAssesment = "Draft Assessment";
        private const string PublishedAssesment = "Published Assessment";
        private const string ApprovedAssesment = "Approved Assessment";

        #endregion

        #region Helpers

        protected internal override void checkSelections()
        {
            _button_RunReport.Enabled = false;

            //they need to pick an assessment
            if (ultraCombo_Assessment1.SelectedItem == null || ultraCombo_Assessment1.SelectedItem.Tag == null) return;

            //they need to pick another assessment
            if (ultraCombo_Assessment2.SelectedItem == null || ultraCombo_Assessment2.SelectedItem.Tag == null) return;

            //they need to pick a server
            if (_comboBox_Server.SelectedValue == null || _comboBox_Server.Text.Length == 0) return;

            _button_RunReport.Enabled = true;
        }

        protected internal override void runReport()
        {
            try
            {
                if (_comboBox_Server.SelectedValue == null || _comboBox_Server.Text.Length == 0) return;

                if (ultraCombo_Assessment1.SelectedItem == null || ultraCombo_Assessment1.SelectedItem.Tag == null) return;

                if (ultraCombo_Assessment2.SelectedItem == null || ultraCombo_Assessment2.SelectedItem.Tag == null) return;

                m_ServerId = (int) _comboBox_Server.SelectedValue;
                m_ServerName = _comboBox_Server.Text;
                m_assessmentid1 = ((Sql.Policy) ultraCombo_Assessment1.SelectedItem.Tag).AssessmentId;
                m_assessmentid2 = ((Sql.Policy)ultraCombo_Assessment2.SelectedItem.Tag).AssessmentId;

                base.runReport();

                logX.loggerX.Info(@"Retrieve data for report Compare Assessments");

                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Setup stored procedures
                    SqlCommand cmd = new SqlCommand(QueryDataSource1, connection);
                    SqlCommand cmd2 = new SqlCommand(QueryDataSource2, connection);
                    SqlCommand cmd3 = new SqlCommand(QueryDataSource3, connection);
                    SqlCommand cmd4 = new SqlCommand(QueryDataSource4, connection);
                    SqlCommand cmd5 = new SqlCommand(QueryDataSource5, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd3.CommandType = CommandType.StoredProcedure;
                    cmd4.CommandType = CommandType.StoredProcedure;
                    cmd5.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                    cmd2.CommandTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                    cmd3.CommandTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                    cmd4.CommandTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                    cmd5.CommandTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();

                    // Build parameters
                    SqlParameter paramPolicyId = new SqlParameter(SqlParamPolicyid, m_policyid);
                    SqlParameter paramAssessmentId1 = new SqlParameter(SqlParamAssessmentid1, m_assessmentid1);
                    SqlParameter paramAssessmentId2 = new SqlParameter(SqlParamAssessmentid2, m_assessmentid2);
                    SqlParameter paramRegisteredServerId = new SqlParameter(SqlParamServerid, m_ServerId);
                    SqlParameter paramDiffsOnly = new SqlParameter(SqlParamDiffsOnly, _checkbox_showDiffsOnly.Checked);

                    cmd.Parameters.Add(paramPolicyId);
                    cmd.Parameters.Add(paramAssessmentId1);
                    cmd.Parameters.Add(paramAssessmentId2);
                    cmd.Parameters.Add(paramRegisteredServerId);
                    cmd.Parameters.Add(paramDiffsOnly);

                    SqlParameter paramPolicyid2 = new SqlParameter(SqlParamPolicyid, m_policyid);
                    SqlParameter paramAssessmentid2 = new SqlParameter(SqlParamAssessmentid, m_assessmentid1);
                    SqlParameter paramRegisteredServerId2 = new SqlParameter(SqlParamServerid, m_ServerId);
                    SqlParameter paramUsebaseline2 = new SqlParameter(SqlParamUsebaseline, m_useBaseline);
                    SqlParameter paramRunDate2 = new SqlParameter(SqlParamRunDate, m_reportDate);
                    cmd2.Parameters.Add(paramPolicyid2);
                    cmd2.Parameters.Add(paramAssessmentid2);
                    cmd2.Parameters.Add(paramRegisteredServerId2);
                    cmd2.Parameters.Add(paramUsebaseline2);
                    cmd2.Parameters.Add(paramRunDate2);

                    SqlParameter paramPolicyid5 = new SqlParameter(SqlParamPolicyid, m_policyid);
                    SqlParameter paramAssessmentid5 = new SqlParameter(SqlParamAssessmentid, m_assessmentid2);
                    SqlParameter paramRegisteredServerId5 = new SqlParameter(SqlParamServerid, m_ServerId);
                    SqlParameter paramUsebaseline5 = new SqlParameter(SqlParamUsebaseline, m_useBaseline);
                    SqlParameter paramRunDate5 = new SqlParameter(SqlParamRunDate, m_reportDate);
                    cmd5.Parameters.Add(paramPolicyid5);
                    cmd5.Parameters.Add(paramAssessmentid5);
                    cmd5.Parameters.Add(paramRegisteredServerId5);
                    cmd5.Parameters.Add(paramUsebaseline5);
                    cmd5.Parameters.Add(paramRunDate5);


                    SqlParameter paramPolicyid3 = new SqlParameter(SqlParamPolicyid, m_policyid);
                    SqlParameter paramAssessmentid3 = new SqlParameter(SqlParamAssessmentid, m_assessmentid1);
                    cmd3.Parameters.Add(paramPolicyid3);
                    cmd3.Parameters.Add(paramAssessmentid3);

                    SqlParameter paramPolicyid4 = new SqlParameter(SqlParamPolicyid, m_policyid);
                    SqlParameter paramAssessmentid4 = new SqlParameter(SqlParamAssessmentid, m_assessmentid2);
                    cmd4.Parameters.Add(paramPolicyid4);
                    cmd4.Parameters.Add(paramAssessmentid4);

                    
                    // Get data for first dataset
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    ReportDataSource rds = new ReportDataSource();
                    rds.Name = DataSourceName1;
                    rds.Value = ds.Tables[0];
                    _reportViewer.LocalReport.DataSources.Clear();
                    _reportViewer.LocalReport.DataSources.Add(rds);

                    // Get data for second dataset
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    DataSet ds2 = new DataSet();
                    da2.Fill(ds2);

                    ReportDataSource rds2 = new ReportDataSource();
                    rds2.Name = DataSourceName2;
                    rds2.Value = ds2.Tables[0];
                    _reportViewer.LocalReport.DataSources.Add(rds2);

                    // Get data for third dataset
                    SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                    DataSet ds3 = new DataSet();
                    da3.Fill(ds3);

                    ReportDataSource rds3 = new ReportDataSource();
                    rds3.Name = DataSourceName3;
                    rds3.Value = ds3.Tables[0];
                    _reportViewer.LocalReport.DataSources.Add(rds3);

                    // Get data for fourth dataset
                    SqlDataAdapter da4 = new SqlDataAdapter(cmd4);
                    DataSet ds4 = new DataSet();
                    da4.Fill(ds4);

                    ReportDataSource rds4 = new ReportDataSource();
                    rds4.Name = DataSourceName4;
                    rds4.Value = ds4.Tables[0];
                    _reportViewer.LocalReport.DataSources.Add(rds4);

                    // Get data for fifth dataset
                    SqlDataAdapter da5 = new SqlDataAdapter(cmd5);
                    DataSet ds5 = new DataSet();
                    da5.Fill(ds5);

                    ReportDataSource rds5 = new ReportDataSource();
                    rds5.Name = DataSourceName5;
                    rds5.Value = ds5.Tables[0];
                    _reportViewer.LocalReport.DataSources.Add(rds5);

                
                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error(@"Error - Unable to get report data", ex);
                MsgBox.ShowError(m_Title, ErrorMsgs.CantGetReportData, ex);
                return;
            } 

            //add report parameters
            ReportParameter[] Param = new ReportParameter[2];
            Param[0] = new ReportParameter("ReportTitle", string.Format(Utility.Constants.REPORTS_TITLE_STR, m_Title));
            Param[1] = new ReportParameter("Expand_All", _isExpanded.ToString());

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

        protected override void showRefresh()
        {
            _button_RunReport.Enabled = false;
            loadCombos();
        }

        private void loadCombos()
        {
            ultraCombo_Assessment1.Items.Clear();
            AddCategoryToComboBox(ultraCombo_Assessment1, ComboTextChooseAssessment);
            ultraCombo_Assessment2.Items.Clear();
            AddCategoryToComboBox(ultraCombo_Assessment2, ComboTextChooseAssessment);

            _comboBox_Server_DropDown(this, null);
        }

        private void LoadAssessmentComboBox(UltraComboEditor combo)
        {
            combo.Items.Clear();

            // Add Current Assessment
            // ----------------------
            foreach (Sql.Policy p in Program.gController.ReportPolicy.Assessments)
            {
                if (p.AssessmentState == Utility.Policy.AssessmentState.Current)
                {
                    AddCategoryToComboBox(combo, p.PolicyName);
                    AddPolicyToComboxBox(combo, p);
                    break;
                }
            }
            // Load Draft Assessments
            // ----------------------
            bool bFirstDraftLoaded = false;
            foreach (Sql.Policy p in Program.gController.ReportPolicy.Assessments)
            {
                if (p.AssessmentState == Utility.Policy.AssessmentState.Draft)
                {
                    if (bFirstDraftLoaded == false)
                    {
                        bFirstDraftLoaded = true;
                        AddCategoryToComboBox(combo, DraftAssesment);
                    }
                    AddPolicyToComboxBox(combo, p);
                }
            }
            // Load Published Assessments
            // --------------------------
            bool bFirstPublishedLoaded = false;
            foreach (Sql.Policy p in Program.gController.ReportPolicy.Assessments)
            {
                if (p.AssessmentState == Utility.Policy.AssessmentState.Published)
                {
                    if (bFirstPublishedLoaded == false)
                    {
                        bFirstPublishedLoaded = true;
                        AddCategoryToComboBox(combo, PublishedAssesment);
                    }
                    AddPolicyToComboxBox(combo, p);
                }
            }
            // Load Approved Assessments
            // --------------------------
            bool bFirstApprovedLoaded = false;
            foreach (Sql.Policy p in Program.gController.ReportPolicy.Assessments)
            {
                if (p.AssessmentState == Utility.Policy.AssessmentState.Approved)
                {
                    if (bFirstApprovedLoaded == false)
                    {
                        bFirstApprovedLoaded = true;
                        AddCategoryToComboBox(combo, ApprovedAssesment);
                    }
                    AddPolicyToComboxBox(combo, p);
                }
            }
        }

        private void AddCategoryToComboBox(Infragistics.Win.UltraWinEditors.UltraComboEditor combo, string category)
        {
            ValueListItem item = new ValueListItem();
            item.DisplayText = category;
            item.Tag = null;
            item.Appearance.FontData.Bold = DefaultableBoolean.True;
            combo.Items.Add(item);
        }

        private string GetDisplayNameForAssessmentInCombo(Sql.Policy p)
        {
            string displayName = string.Empty;
            if (p.IsPolicy)
            {
                string displayFmt = "as of {0}{1}";
                displayName =
                    string.Format(displayFmt,
                                  Program.gController.PolicyTime.HasValue ? Program.gController.PolicyTime.Value.ToLocalTime().ToString(Utility.Constants.DATETIME_FORMAT) : "Now",
                                  Program.gController.PolicyUseBaselineSnapshots ? " (using Baseline)" : string.Empty);
            }
            else
            {
                displayName = p.AssessmentName;

            }
            return displayName;
        }

        private void AddPolicyToComboxBox(Infragistics.Win.UltraWinEditors.UltraComboEditor combo, Sql.Policy p)
        {
            ValueListItem item = new ValueListItem();
            item.DisplayText = "   " + GetDisplayNameForAssessmentInCombo(p);
            item.Tag = item.Tag = p;
            combo.Items.Add(item);
        }

        #endregion

        #region Events

        private void Report_CompareAssessments_Load(object sender, EventArgs e)
        {
            //start off filling server list
            _comboBox_Server_DropDown(this, null);
            AddCategoryToComboBox(ultraCombo_Assessment1, ComboTextChooseAssessment);
            AddCategoryToComboBox(ultraCombo_Assessment2, ComboTextChooseAssessment);
        }

        private void _comboBox_Server_DropDown(object sender, EventArgs e)
        {
            //Initialize the combobox for server selection to refresh on every open
            List<MyComboBoxItem> li = new List<MyComboBoxItem>();
            li.Add(new MyComboBoxItem(Utility.Constants.ReportSelect_AllServers, 0));

            //Keep the last selection for the user
            string selection = _comboBox_Server.Text;

            if (ultraCombo_Assessment1.SelectedItem != null && ultraCombo_Assessment1.SelectedItem.Tag is Sql.Policy)
            {
                Sql.Policy assessment = (Sql.Policy) ultraCombo_Assessment1.SelectedItem.Tag;

                foreach (Sql.RegisteredServer server in assessment.GetMemberServers())
                {
                    li.Add(new MyComboBoxItem(server.ConnectionName, server.RegisteredServerId));
                }
            }

            _comboBox_Server.DisplayMember = "Label";
            _comboBox_Server.ValueMember = "Index";
            _comboBox_Server.DataSource = li;

            //Put the last selection back for the user
            _comboBox_Server.Text = selection;
        }

        private void _comboBox_Server_SelectionChangeCommitted(object sender, EventArgs e)
        {
            checkSelections();
        }

        private void ultraCombo_Assessment1_BeforeDropDown(object sender, CancelEventArgs e)
        {
            LoadAssessmentComboBox(ultraCombo_Assessment1);
        }

        private void ultraCombo_Assessment2_BeforeDropDown(object sender, CancelEventArgs e)
        {
            LoadAssessmentComboBox(ultraCombo_Assessment2);
        }

        private void ultraCombo_Assessment1_SelectionChanged(object sender, EventArgs e)
        {
            selectionChanged = true;

            _comboBox_Server_DropDown(this, null);

            checkSelections();
        }

        private void ultraCombo_Assessment2_SelectionChanged(object sender, EventArgs e)
        {
            selectionChanged = true;

            checkSelections();
        }

        private void ultraCombo_Assessment1_AfterCloseUp(object sender, EventArgs e)
        {
            if (selectionChanged
                && (ultraCombo_Assessment1.SelectedItem == null || ultraCombo_Assessment1.SelectedItem.Tag == null))
            {
                ultraCombo_Assessment1.DropDown();
                selectionChanged = false;
            }
        }

        private void ultraCombo_Assessment2_AfterCloseUp(object sender, EventArgs e)
        {
            if (selectionChanged
                && (ultraCombo_Assessment2.SelectedItem == null || ultraCombo_Assessment2.SelectedItem.Tag == null))
            {
                ultraCombo_Assessment2.DropDown();
                selectionChanged = false;
            }
        }

        #endregion

        #region MyComboBoxItem

        public class MyComboBoxItem
        {
            private string _name;
            private int _value;

            public MyComboBoxItem(string name, int value)
            {
                _name = name;
                _value = value;
            }

            public string Label
            {
                get { return _name; }
                set { _name = value; }
            }
            public int Index
            {
                get { return _value; }
                set { _value = value; }
            }
        }

        #endregion
    }
}