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
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win;

namespace Idera.SQLsecure.UI.Console.Views
{
    public partial class Report_RiskAssessment : Idera.SQLsecure.UI.Console.Controls.BaseReport, Interfaces.IView
    {
        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Views.Report_RiskAssessment");

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
        private bool droppingDown = false;

        #endregion

        #region Ctors

        public Report_RiskAssessment()
        {
            InitializeComponent();
            
            //set the global title, the report title in the header graphic, and the title in the
            //status bar
            m_Title                     =
                _label_ReportTitle.Text =
                _label_Status.Text      = Utility.Constants.ReportTitle_RiskAssessment;

            //set the description and getting started text
            _label_Description.Text = Utility.Constants.ReportSummary_RiskAssessment;

            int i = 1;
            StringBuilder instructions = new StringBuilder(Utility.Constants.ReportRunInstructions_MultiStep);
            instructions.Append(newline);
            instructions.AppendFormat(instructionformat, i++, Utility.Constants.ReportRunInstructions_UseSelection, newline);
            instructions.AppendFormat(instructionformat, i++, Utility.Constants.ReportRunInstructions_Assessment, newline);            
            instructions.AppendFormat(instructionformat, i++, Utility.Constants.ReportRunInstructions_Server, newline);
            instructions.AppendFormat(instructionformat, i++, Utility.Constants.ReportRunInstructions_ShowAlertsOnly, newline);
            instructions.AppendFormat(instructionformat, i, Utility.Constants.ReportRunInstructions_NoParameters, newline);
            _label_Instructions.Text = instructions.ToString();

            _button_RunReport.Enabled = false;
        }
        
        #endregion

        #region Queries & Constants

        private const string QueryDataSource1 = @"SQLsecure.dbo.isp_sqlsecure_getpolicyassessment";
        private const string DataSourceName1 = @"ReportsDataset_isp_sqlsecure_getpolicyassessment";

        private const string QueryDataSource2 = @"SQLsecure.dbo.isp_sqlsecure_getpolicyserver";
        private const string DataSourceName2 = @"ReportsDataset_isp_sqlsecure_getpolicyserver";

        private const string QueryDataSource3 = @"SQLsecure.dbo.isp_sqlsecure_report_getpolicyinfo";
        private const string DataSourceName3 = @"ReportsDataset_isp_sqlsecure_report_getpolicyinfo";

        private const string ComboTextChooseAssessment = "<Choose an assessment>";
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
            if (ultraCombo_Assessment.SelectedItem == null || ultraCombo_Assessment.SelectedItem.Tag == null) return;

            //they need to pick a server
            if (_comboBox_Server.SelectedValue == null || _comboBox_Server.Text.Length == 0) return;

            _button_RunReport.Enabled = true;
        }


        protected internal override void runReport()
        {
            try
            {
                if (_comboBox_Server.SelectedValue == null || _comboBox_Server.Text.Length == 0) return;
                if (ultraCombo_Assessment.SelectedItem == null || ultraCombo_Assessment.SelectedItem.Tag == null) return;

                m_ServerId = (int)_comboBox_Server.SelectedValue;
                m_ServerName = _comboBox_Server.Text;
                base.runReport();

                // base.runReport sets the assessment to the current policy assessment.
                // we need to override the base report setting so this must be after the base.runReport.
                m_assessmentid = ((Sql.Policy)ultraCombo_Assessment.SelectedItem.Tag).AssessmentId;

                logX.loggerX.Info(@"Retrieve data for report Risk Assessment");

                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Setup stored procedures
                    SqlCommand cmd = new SqlCommand(QueryDataSource1, connection);
                    SqlCommand cmd2 = new SqlCommand(QueryDataSource2, connection);
                    SqlCommand cmd3 = new SqlCommand(QueryDataSource3, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd3.CommandType = CommandType.StoredProcedure;

                    // Build parameters
                    SqlParameter paramRunDate = new SqlParameter(SqlParamRunDate, m_reportDate);
                    SqlParameter paramPolicyid = new SqlParameter(SqlParamPolicyid, m_policyid);
                    SqlParameter paramAssessmentid = new SqlParameter(SqlParamAssessmentid, m_assessmentid);
                    SqlParameter paramServerName = new SqlParameter(SqlParamServerid, m_ServerId);
                    SqlParameter paramUseBaseline = new SqlParameter(SqlParamUsebaseline, m_useBaseline);
                    SqlParameter paramAlertsOnly = new SqlParameter(SqlParamAlertsOnly, _checkbox_showAlertsOnly.Checked);

                    cmd.Parameters.Add(paramRunDate);
                    cmd.Parameters.Add(paramPolicyid);
                    cmd.Parameters.Add(paramAssessmentid);
                    cmd.Parameters.Add(paramServerName);
                    cmd.Parameters.Add(paramUseBaseline);
                    cmd.Parameters.Add(paramAlertsOnly);

                    SqlParameter paramPolicyid2 = new SqlParameter(SqlParamPolicyid, m_policyid);
                    SqlParameter paramAssessmentid2 = new SqlParameter(SqlParamAssessmentid, m_assessmentid);
                    SqlParameter paramRegisteredServerId2 = new SqlParameter(SqlParamServerid, m_ServerId);
                    SqlParameter paramUsebaseline2 = new SqlParameter(SqlParamUsebaseline, m_useBaseline);
                    SqlParameter paramRunDate2 = new SqlParameter(SqlParamRunDate, m_reportDate);
                    cmd2.Parameters.Add(paramPolicyid2);
                    cmd2.Parameters.Add(paramAssessmentid2);
                    cmd2.Parameters.Add(paramRegisteredServerId2);
                    cmd2.Parameters.Add(paramUsebaseline2);
                    cmd2.Parameters.Add(paramRunDate2);

                    SqlParameter paramPolicyid3 = new SqlParameter(SqlParamPolicyid, m_policyid);
                    SqlParameter paramAssessmentid3 = new SqlParameter(SqlParamAssessmentid, m_assessmentid);
                    cmd3.Parameters.Add(paramPolicyid3);
                    cmd3.Parameters.Add(paramAssessmentid3);

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

        protected override void showRefresh()
        {
            _button_RunReport.Enabled = false;
            loadCombos();
        }

        private void loadCombos()
        {
            ultraCombo_Assessment.Items.Clear();
            AddCategoryToComboBox(ultraCombo_Assessment, ComboTextChooseAssessment);

            _comboBox_Server_DropDown(this, null);
        }

        private void LoadAssessmentComboBox(UltraComboEditor combo)
        {
            Sql.Policy selected = null;
            if (combo.SelectedItem != null && combo.SelectedItem.Tag is Sql.Policy)
                selected = (Sql.Policy)combo.SelectedItem.Tag;
            combo.Items.Clear();

            // Add Current Assessment
            // ----------------------
            foreach (Sql.Policy p in Program.gController.ReportPolicy.Assessments)
            {
                if (p.AssessmentState == Utility.Policy.AssessmentState.Current)
                {
                    AddCategoryToComboBox(combo, p.PolicyName);
                    AddPolicyToComboxBox(combo, p);
                    if (p == selected)
                        combo.SelectedItem = combo.Items[combo.Items.Count - 1];
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
                    if (p == selected)
                        combo.SelectedItem = combo.Items[combo.Items.Count - 1];
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
                    if (p == selected)
                        combo.SelectedItem = combo.Items[combo.Items.Count - 1];
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
                    if (p == selected)
                        combo.SelectedItem = combo.Items[combo.Items.Count - 1];
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

        private void Report_RiskAssessment_Load(object sender, EventArgs e)
        {
            //start off filling server list
            _comboBox_Server_DropDown(this, null);
            AddCategoryToComboBox(ultraCombo_Assessment, ComboTextChooseAssessment);
        }

        private void _comboBox_Server_DropDown(object sender, EventArgs e)
        {
            //Initialize the combobox for server selection to refresh on every open
            List<MyComboBoxItem> li = new List<MyComboBoxItem>();
            li.Add(new MyComboBoxItem(Utility.Constants.ReportSelect_AllServers, 0));

            //Keep the last selection for the user
            string selection = _comboBox_Server.Text;

            if (ultraCombo_Assessment.SelectedItem != null && ultraCombo_Assessment.SelectedItem.Tag is Sql.Policy)
            {
                Sql.Policy assessment = (Sql.Policy) ultraCombo_Assessment.SelectedItem.Tag;

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

        private void ultraCombo_Assessment_AfterCloseUp(object sender, EventArgs e)
        {
            // if it isn't a valid selection, then make sure the selection drop down is still shown
            if (selectionChanged
                && (ultraCombo_Assessment.SelectedItem == null || ultraCombo_Assessment.SelectedItem.Tag == null))
            {
                ultraCombo_Assessment.DropDown();
                selectionChanged = false;
            }
        }

        private void ultraCombo_Assessment_BeforeDropDown(object sender, CancelEventArgs e)
        {
            if (!droppingDown)
            {
                droppingDown = true;

                LoadAssessmentComboBox(ultraCombo_Assessment);

                droppingDown = false;
            }
        }

        private void ultraCombo_Assessment_SelectionChanged(object sender, EventArgs e)
        {
            if (!droppingDown)
            {
                selectionChanged = true;

                _comboBox_Server_DropDown(this, null);
            }

            checkSelections();
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