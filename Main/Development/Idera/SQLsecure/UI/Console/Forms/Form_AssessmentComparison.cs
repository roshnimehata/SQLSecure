using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Idera.SQLsecure.UI.Console;
using Idera.SQLsecure.UI.Console.Utility;
using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Sql;
using Policy=Idera.SQLsecure.UI.Console.Sql.Policy;
using Infragistics.Win;
using Idera.SQLsecure.UI.Console.Controls;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.Misc;
using System.Data.SqlClient;
using System.Diagnostics;


namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_AssessmentComparison : Idera.SQLsecure.UI.Console.Controls.BaseForm
    {
        #region Fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Forms.Form_AssessmentComparison");

        private const string TitleMsg = "Assessment Comparison - {0}";
        private const string NoAssessmentDescription = "No assessments have been selected for comparison.";
        private const string SummaryTabDescription = "Compare assessment summaries and report cards.";
        private const string SecurityChecksTabDescription = "Compare security check findings and settings.";
        private const string InternalNotesTabDescription = "Compare internal review notes.";
        
        
        private int m_policyId = 0;
        private Repository.AssessmentList m_Assessments = null;

        private int m_assessmentId1 = 0;
        private Sql.Policy m_PolicyAssessment1 = null;
        private Sql.RegisteredServer m_server1 = null;
        private Control m_focused = null; // To prevent focus from switching to the splitters, etc.

        private int m_assessmentId2 = 0;
        private Sql.Policy m_PolicyAssessment2 = null;


        //private bool isFindingDiffFound = false;
        private bool m_isSummaryDiffFound = false;
        //private bool isReportCardDiffFound = false;
        //private bool isInternalReviewNotesDiffFound = false;

        private const string NoMetrics = "No security checks exist";
        private const string BarLabelDisplay = "{0} Risk";
        private const string BarHighCountDisplay = "of {0}";
        private const string HeaderServers = @"Servers";
        private const string NoRecordsValue = "No {0} found";
        private const string HeaderDisplay = "{0} Server{1}";
        private const string PrintHeaderDisplay = "{0}\n Server Status as of {1}\n\n{2}";
        private const string PrintEmptyHeaderDisplay = "{0}";
        private const string ComparisonSummaryHeader = "{0} {1} Found";
        private const string ReportCardHeader = "{0} {1} Found";
        private const string DifferenceSingular = "Difference";
        private const string DifferencePlural = "Differences";
        private const string CompareSummaryDetails = "Details - {0}";
        private const string SelectionsFormat = "Use most current {0}data";
        private const string Display_Baseline = @"baseline ";
        private const string ToolTipDiffsFound = "Differences Found.\r\nDouble click to see details.";
        private const string ToolTipNoDiffsFound = "No Differences Found.";
        private const string FindingToolTipFormat = "Difference Found, {0}\r\nDouble click to see details.";

        private DataTable m_serverTable1;


        private DataTable m_ComparisonTable;
        private DataTable m_ComparisonSummaryTable;

        private DataTable m_ExplanationTable1;
        private DataTable m_ExplanationTable2;

        private const string tabKey_CompareSummaries = "Summaries";
        private const string tabKey_CompareFindings = "Findings";
        private const string tabKey_CompareReportCards = "ReportCards";
        private const string tabKey_CompareReportCards2 = "ReportCards2";
        private const string tabKey_CompareNotes = "Notes";


        private const string tabKey_Criteria = "Criteria";
        private const string tabKey_Findings = "Findings";
        private const string tabKey_DisplaySettings = "DisplaySettings";
        private const string tabKey_Notes = "Notes";

        // Servers Grid columns
        private const string colHigh1 = "High1";
        private const string colMedium1 = "Medium1";
        private const string colLow1 = "Low1";
        private const string colDateTime1 = "DateTime1";
        private const string colServer = "Server";
        private const string colHigh2 = "High2";
        private const string colMedium2 = "Medium2";
        private const string colLow2 = "Low2";
        private const string colDateTime2 = "DateTime2";



        // Assessment Comparison columns

        // Common
        private const string colConnection = @"connectionname";
        private const string colRegisteredServerId = @"registeredserverid";
        private const string colMetricId = @"metricid";
        private const string colMetricName = @"metricname";
        private const string colMetricType = @"metrictype";

        // For Assessment 1
        private const string colSnapshotId1 = @"snapshotid1";
        private const string colCollectionTime1 = @"collectiontime1";
        private const string colMetricSeverityCode1 = @"metricseveritycode1";
        private const string colMetricSeverity1 = @"metricseverity1";
        private const string colMetricDescription1 = @"metricdescription1";
        private const string colXRef1 = @"metricreportkey1";
        private const string colReportText1 = @"metricreporttext1";
        private const string colSeverityCode1 = @"severitycode1";
        private const string colCurrentValue1 = @"currentvalue1";
        private const string colThresholdValue1 = @"thresholdvalue1";
        private const string colIsExplained1 = @"isexplained1";
        private const string colExplanationNotes1 = @"notes1";

        // For Assessment 2
        private const string colSnapshotId2 = @"snapshotid2";
        private const string colCollectionTime2 = @"collectiontime2";
        private const string colMetricSeverityCode2 = @"metricseveritycode2";
        private const string colMetricSeverity2 = @"metricseverity2";
        private const string colMetricDescription2 = @"metricdescription2";
        private const string colXRef2 = @"metricreportkey2";
        private const string colReportText2 = @"metricreporttext2";
        private const string colSeverityCode2 = @"severitycode2";
        private const string colCurrentValue2 = @"currentvalue2";
        private const string colThresholdValue2 = @"thresholdvalue2";
        private const string colIsExplained2 = @"isexplained2";
        private const string colExplanationNotes2 = @"notes2";



        // UI Comparison Grid Summary Columns
        private const string colDifferencesFound = @"differencesfound";
        private const string colReportCardDisplayDiff = @"diffreportsettings";
        private const string colReportCardDisplayDiffText = @"diffreportsettingstext";
        private const string colSecurityCheckDiff = @"diffmetricsettings";
        private const string colSecurityCheckDiffText = @"diffmetricsettingstext";
        private const string colFindingDiff = @"difffindings";
        private const string colFindingDiffText = @"difffindingstext";
        private const string colExplanationDiff = @"diffnotes";
        private const string colExplanationDiffText = @"diffnotestext";

        private const string valueListDifference = @"Difference";
        private const string valueListSeverity = @"Severity";

        // Explanation Grid columns
        private const string colServerName = "Server";
        private const string colSnapshotId = "snapshotid";
        private const string colIsExplained = @"Is Explained";
        private const string colNotes = @"Notes";


        private const string CurrentAssesment = "Policy Assessment";
        private const string DraftAssesment = "Draft Assessment";
        private const string PublishedAssesment = "Published Assessment";
        private const string ApprovedAssesment = "Approved Assessment";

        private const string ServerSummaryText = "{0} Server Summary";
        private const string FindingsText = "{0} Findings Summary";

        private const string ServerNotInAssessment = "Not in assessment";
        private const string ServerNoAuditData = "No Audit Data";

        // Comparison Tab Text
        private const string ComparisonTextNoAuditData = "No Audit Data";
        private const string ComparisonTextNoDifferences = "No Differences Found";
        private const string ComparisonDetailsSCNotEnabled = "Security Check not Enabled";
        private const string ComparisonDetailsSCNoAuditData = "No Audit Data";
        private const string NoExplanationNotesForCurrent = "No explanation notes for current policy";

        private const string ComboTextChooseAssessment = "<Choose an assessment to compare>";

        private const string CopyEnabledToolTip = "Copy Notes and Explained to {0} for the selected Servers";
        private const string CopyDisabledToolTip = "Notes can only be copied to a draft or published assessment";
        private const string CopySelectServer = "Select Server to copy notes";

        public static Color highlightColorDifferent = Color.Yellow;
        public static Color highlightColorMissing = Color.Chartreuse;

        Dictionary<int, CompareResults> metricDifferenceDict = new Dictionary<int, CompareResults>();

        private const string ShowDiffsOnly = "Show Differences Only";
        private const string ShowAll = "Show All";
        private bool m_showDifferencesOnly = true;
        private bool m_showSCDifferencesOnly = true;

        private bool m_internalUpdate = false;

        enum CompareResults
        {
            COMPARE_MATCH = 0,
            COMPARE_MISSING = 1,
            COMPARE_DIFFERENT = 2
        }


        #endregion

        #region Queries
        // Get Policy Assessment Comparison
        private const string QueryGetAssessmentComparison = @"SQLsecure.dbo.isp_sqlsecure_getassessmentcomparison";
        private const string ParamPolicyId = "@policyid";
        private const string ParamAssessmentId1 = "@assessmentid1";
        private const string ParamAssessmentId2 = "@assessmentid2";
        private const string ParamRegisteredServerId = "@registeredserverid ";
        private const string ParamDiffsOnly = "@diffsonly";

        private const string QueryUpdateExplanationNotes = @"SQLsecure.dbo.isp_sqlsecure_updatepolicyassessmentnotes";
        private const string ParamAssessmentId = "@assessmentid";
        private const string ParamMetricId = "@metricid";
        private const string ParamSnapshotId = "@snapshotid";
        private const string ParamIsExplained = "@isexplained";
        private const string ParamExplanationNotes = "@notes";

        #endregion

        #region CTOR

        public Form_AssessmentComparison(int policyId, int assessmentId, Sql.RegisteredServer server)
        {
            InitializeComponent();
            SetDescriptionText();

            m_policyId = policyId;
            m_assessmentId1 = assessmentId;
            m_assessmentId2 = assessmentId;
            m_server1 = server;

            // Set the default icon to be unknown
            foreach(Infragistics.Win.UltraWinTabControl.UltraTab tab in _ultraTabControl_Compare.Tabs)
            {
                tab.Appearance.Image = AppIcons.AppImage16(AppIcons.Enum.Unknown);
            }

            // hook the grids to the toolbars so they can be used for button processing
            _headerStrip_ComparisonSummary.Tag = _grid_ComparisonSummary;
            _headerStrip_Servers1.Tag = _grid_Servers1;
            _headerStrip_ReportCard.Tag = _grid_ReportCard;
            _headerStrip_Notes1.Tag = _grid_ExplanationNotes1;
            _headerStrip_Notes2.Tag = _grid_ExplanationNotes2;
            _grid_ComparisonSummary.Tag = _label_ComparisonSummary;
            _grid_Servers1.Tag = _label_Servers;
            _grid_ReportCard.Tag = _label_ReportCard;
            _grid_ExplanationNotes1.Tag = _toolStripLabel_Notes1;
            _grid_ExplanationNotes2.Tag = _toolStripLabel_Notes2;

            // Hookup all application images
            _toolStripButton_ComparisonSummaryColumnChooser.Image = AppIcons.AppImage16(AppIcons.Enum.GridFieldChooser);
            _toolStripButton_ComparisonSummaryGroupBy.Image = AppIcons.AppImage16(AppIcons.Enum.GridGroupBy);
            _toolStripButton_ComparisonSummarySave.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);
            _toolStripButton_ComparisonSummaryPrint.Image = AppIcons.AppImage16(AppIcons.Enum.Print);
            _toolStripButton_ReportCardColumnChooser.Image = AppIcons.AppImage16(AppIcons.Enum.GridFieldChooser);
            _toolStripButton_ReportCardGroupBy.Image = AppIcons.AppImage16(AppIcons.Enum.GridGroupBy);
            _toolStripButton_ReportCardSave.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);
            _toolStripButton_ReportCardPrint.Image = AppIcons.AppImage16(AppIcons.Enum.Print);
            _toolStripButton_ServerSave1.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);
            _toolStripButton_ServerPrint1.Image = AppIcons.AppImage16(AppIcons.Enum.Print);
            _toolStripButton_Notes1Save.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);
            _toolStripButton_Notes1Print.Image = AppIcons.AppImage16(AppIcons.Enum.Print);
            _toolStripButton_Notes2Save.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);
            _toolStripButton_Notes2Print.Image = AppIcons.AppImage16(AppIcons.Enum.Print);

            _toolStripButton_ShowDiffsOnly.Text = m_showDifferencesOnly ? ShowAll : ShowDiffsOnly;
            _toolStripButton_SCShowDiffOnly.Text = m_showSCDifferencesOnly ? ShowAll : ShowDiffsOnly;


            // load value lists for grid display
            ValueListItem listItem;
            ValueList DifferenceValueList3 = new ValueList();
            DifferenceValueList3.Key = valueListDifference;
            DifferenceValueList3.DisplayStyle = ValueListDisplayStyle.Picture;
            _grid_Servers1.DisplayLayout.ValueLists.Add(DifferenceValueList3);
            DifferenceValueList3.ValueListItems.Clear();
            listItem = new ValueListItem(CompareResults.COMPARE_MATCH, "No Differences");
            listItem.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources._16_CompareMatch;
            DifferenceValueList3.ValueListItems.Add(listItem);
            listItem = new ValueListItem(CompareResults.COMPARE_DIFFERENT, "Difference Found");
            listItem.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources._16_CompareDifferent;
            DifferenceValueList3.ValueListItems.Add(listItem);
            listItem = new ValueListItem(CompareResults.COMPARE_MISSING, "Missing");
            listItem.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources._16_CompareMissing;
            DifferenceValueList3.ValueListItems.Add(listItem);


            ValueList DifferenceValueList = new ValueList();
            DifferenceValueList.Key = valueListDifference;
            DifferenceValueList.DisplayStyle = ValueListDisplayStyle.Picture;
            _grid_ComparisonSummary.DisplayLayout.ValueLists.Add(DifferenceValueList);
            DifferenceValueList.ValueListItems.Clear();
            listItem = new ValueListItem(CompareResults.COMPARE_MATCH, "No Differences");
            listItem.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources._16_CompareMatch;
            DifferenceValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(CompareResults.COMPARE_DIFFERENT, "Difference Found");
            listItem.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources._16_CompareDifferent;
            DifferenceValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(CompareResults.COMPARE_MISSING, "Missing");
            listItem.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources._16_CompareMissing;
            DifferenceValueList.ValueListItems.Add(listItem);

            ValueList DifferenceValueList2 = new ValueList();
            DifferenceValueList2.Key = valueListDifference;
            DifferenceValueList2.DisplayStyle = ValueListDisplayStyle.Picture;
            DifferenceValueList2.ValueListItems.Clear();
            listItem = new ValueListItem(CompareResults.COMPARE_MATCH, "No Differences");
            listItem.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources._16_CompareMatch;
            DifferenceValueList2.ValueListItems.Add(listItem);
            listItem = new ValueListItem(CompareResults.COMPARE_DIFFERENT, "Difference Found");
            listItem.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources._16_CompareDifferent;
            DifferenceValueList2.ValueListItems.Add(listItem);
            listItem = new ValueListItem(CompareResults.COMPARE_MISSING, "Missing");
            listItem.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources._16_CompareMissing;
            DifferenceValueList2.ValueListItems.Add(listItem);
            _grid_ReportCard.DisplayLayout.ValueLists.Add(DifferenceValueList2);

            // load value lists for grid display
            ValueList severityValueList = new ValueList();
            severityValueList.Key = valueListSeverity;
            severityValueList.DisplayStyle = ValueListDisplayStyle.Picture;
            _grid_ReportCard.DisplayLayout.ValueLists.Add(severityValueList);
            severityValueList.ValueListItems.Clear();
            listItem = new ValueListItem(Utility.Policy.SeverityExplained.Ok, DescriptionHelper.GetEnumDescription(Utility.Policy.SeverityExplained.Ok));
            listItem.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.check_16;
            listItem.Appearance.ImageVAlign = VAlign.Top;
            severityValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(Utility.Policy.SeverityExplained.LowExplained, DescriptionHelper.GetEnumDescription(Utility.Policy.SeverityExplained.LowExplained));
            listItem.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.LowRiskExplained_16;
            listItem.Appearance.ImageVAlign = VAlign.Top;
            severityValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(Utility.Policy.SeverityExplained.MediumExplained, DescriptionHelper.GetEnumDescription(Utility.Policy.SeverityExplained.MediumExplained));
            listItem.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.MediumRiskExplained_16;
            listItem.Appearance.ImageVAlign = VAlign.Top;
            severityValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(Utility.Policy.SeverityExplained.HighExplained, DescriptionHelper.GetEnumDescription(Utility.Policy.SeverityExplained.HighExplained));
            listItem.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.HighRiskExplained_16;
            listItem.Appearance.ImageVAlign = VAlign.Top;
            severityValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(Utility.Policy.SeverityExplained.Low, DescriptionHelper.GetEnumDescription(Utility.Policy.SeverityExplained.Low));
            listItem.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.LowRisk_16;
            listItem.Appearance.ImageVAlign = VAlign.Top;
            severityValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(Utility.Policy.SeverityExplained.Medium, DescriptionHelper.GetEnumDescription(Utility.Policy.SeverityExplained.Medium));
            listItem.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.MediumRisk_16;
            listItem.Appearance.ImageVAlign = VAlign.Top;
            severityValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(Utility.Policy.SeverityExplained.High, DescriptionHelper.GetEnumDescription(Utility.Policy.SeverityExplained.High));
            listItem.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.HighRisk_16;
            listItem.Appearance.ImageVAlign = VAlign.Top;
            severityValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(Utility.Policy.SeverityExplained.Undetermined, DescriptionHelper.GetEnumDescription(Utility.Policy.SeverityExplained.Undetermined));
            listItem.Appearance.Image = AppIcons.AppImage16(AppIcons.Enum.Unknown);
            listItem.Appearance.ImageVAlign = VAlign.Top;
            severityValueList.ValueListItems.Add(listItem);


            // Hide the focus rectangles on tabs and grids
            _ultraTabControl_Compare.DrawFilter = new HideFocusRectangleDrawFilter();
            _ultraTabControl_ComparisonDetails.DrawFilter = new HideFocusRectangleDrawFilter();
            _grid_ReportCard.DrawFilter = new HideFocusRectangleDrawFilter();
            _grid_Servers1.DrawFilter = new HideFocusRectangleDrawFilter();
            _grid_ReportCard.DrawFilter = new HideFocusRectangleDrawFilter(); 
            _grid_ComparisonSummary.DrawFilter = new HideFocusRectangleDrawFilter();
            _grid_ExplanationNotes1.DrawFilter = new HideFocusRectangleDrawFilter();
            _grid_ExplanationNotes2.DrawFilter = new HideFocusRectangleDrawFilter();
        }

        #endregion

        #region Initialize

        private void Form_AssessmentComparison_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                Program.gController.Repository.RefreshPolicies();
                m_Assessments = Program.gController.Repository.Policies.Find(m_policyId).Assessments;
                m_PolicyAssessment1 = m_Assessments.Find(m_assessmentId1);

                m_assessmentId2 = -1;

                this.Text = string.Format(TitleMsg, m_PolicyAssessment1.PolicyName);

                InitDataSource();

                LoadAssessmentComboBox(ultraCombo_Assessment1);
                AssessmentComboBoxSelectAssessmentIs(ultraCombo_Assessment1, m_assessmentId1);

                LoadAssessmentComboBox(ultraCombo_Assessment2);
                LoadRegisterServers();

       //         UpdateAssessment1ToCurrentSelection();

                ultraLabel_ChooseAssessment.BringToFront();

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                logX.loggerX.Error("Error - Unable to load the assessment from the Policy from the Repository", ex);
                MsgBox.ShowError(Utility.ErrorMsgs.CantGetAssessments, ex.Message);
            }
        }

        private void AssessmentComboBoxSelectAssessmentIs(UltraComboEditor combo, int assessmentId)
        {
            foreach (ValueListItem vi in combo.Items)
            {
                if (vi.Tag != null && ((Policy)vi.Tag).AssessmentId == assessmentId)
                {
                    combo.SelectedItem = vi;
                    break;
                }
            }            
        }

        private void LoadAssessmentComboBox(UltraComboEditor combo)
        {
            combo.Items.Clear();
            if (m_assessmentId2 == -1)
            {
                AddCategoryToComboBox(ultraCombo_Assessment2, ComboTextChooseAssessment);
            }

            // Add Current Assessment
            // ----------------------
            foreach (Sql.Policy p in m_Assessments)
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
            foreach (Sql.Policy p in m_Assessments)
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
            foreach (Sql.Policy p in m_Assessments)
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
            foreach (Sql.Policy p in m_Assessments)
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

        private string GetDisplayNameForAssessmentInCombo(Policy p)
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

        private void AddPolicyToComboxBox(Infragistics.Win.UltraWinEditors.UltraComboEditor combo, Policy p)
        {
            ValueListItem item = new ValueListItem();
            item.DisplayText = "   " + GetDisplayNameForAssessmentInCombo(p);
            item.Tag = item.Tag = p;
            combo.Items.Add(item);
        }

        private void LoadRegisterServers()
        {
            m_internalUpdate = true;
            try
            {
                ValueListItem item = new ValueListItem();
                item.Tag = null;
                item.DisplayText = "All Servers";

                ultraCombo_Server1.Items.Clear();
                ultraCombo_Server1.Items.Add(item);
                if (m_PolicyAssessment1.Members != null)
                {
                    foreach (RegisteredServer r in m_PolicyAssessment1.Members)
                    {
                        item = new ValueListItem();
                        item.Tag = r;
                        item.DisplayText = r.ConnectionName;
                        ultraCombo_Server1.Items.Add(item);
                    }
                }
                bool bFound = false;
                foreach (ValueListItem vi in ultraCombo_Server1.Items)
                {
                    if (vi.Tag != null && m_server1 != null)
                    {
                        if (((RegisteredServer)vi.Tag).RegisteredServerId == m_server1.RegisteredServerId)
                        {
                            ultraCombo_Server1.SelectedItem = vi;
                            bFound = true;
                            break;
                        }
                    }
                    else
                    {
                        if (m_server1 == null && vi.Tag == null)
                        {
                            ultraCombo_Server1.SelectedItem = vi;
                            bFound = true;
                            break;
                        }
                    }
                }
                if (!bFound)
                {
                    m_server1 = null;
                    ultraCombo_Server1.SelectedIndex = 0;
                }

            }
            catch (Exception ex)
            {
                logX.loggerX.Error("Error - Unable to load the registered servers from Assessment", ex);
                MsgBox.ShowError(Utility.ErrorMsgs.CantGetRegisteredServers, ex.Message);
            }
            m_internalUpdate = false;
        }

        private DataTable CreateDataSourceServers()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(colDateTime1, typeof(string));
            dt.Columns.Add(colHigh1, typeof(int));
            dt.Columns.Add(colMedium1, typeof(int));
            dt.Columns.Add(colLow1, typeof(int));
            dt.Columns.Add(colDifferencesFound, typeof(int));
            dt.Columns.Add(colServer, typeof(string));
            dt.Columns.Add(colHigh2, typeof(int));
            dt.Columns.Add(colMedium2, typeof(int));
            dt.Columns.Add(colLow2, typeof(int));
            dt.Columns.Add(colDateTime2, typeof (string));

            return dt;
        }

        private void InitDataSource()
        {
            // Initialize the servers grid
            m_serverTable1 = CreateDataSourceServers();

            _label_Servers.Text = HeaderServers;
            _grid_Servers1.SetDataBinding(m_serverTable1.DefaultView, null);
        }


        #endregion

        #region Update Assessment UI

        private void HighlightSummaryDifferences()
        {
            m_isSummaryDiffFound = false;
            Color color = Color.Transparent;
            if (_label_LowRiskCount.Text != _label_LowRiskCount2.Text)
            {
                m_isSummaryDiffFound = true;
                color = highlightColorDifferent;
            }            
            _label_LowRiskCount.BackColor = 
                _label_LowRiskCount2.BackColor = color;

            color = Color.Transparent;
            if (_label_LowCount.Text != _label_LowCount2.Text)
            {
                m_isSummaryDiffFound = true;
                color = highlightColorDifferent;
            }          
           _label_LowCount.BackColor = 
                _label_LowCount2.BackColor = color;


           color = Color.Transparent;
            if (_label_MediumRiskCount.Text != _label_MediumRiskCount2.Text)
            {
                m_isSummaryDiffFound = true;
                color = highlightColorDifferent;
            }            
            _label_MediumRiskCount.BackColor = 
                _label_MediumRiskCount2.BackColor = color;
            

            color = Color.Transparent;
            if (_label_MediumCount.Text != _label_MediumCount2.Text)
            {
                m_isSummaryDiffFound = true;
                color = highlightColorDifferent;
            }
            _label_MediumCount.BackColor =
                _label_MediumCount2.BackColor = color;

            color = Color.Transparent;
            if (_label_HighRiskCount.Text != _label_HighRiskCount2.Text)
            {
                m_isSummaryDiffFound = true;
                color = highlightColorDifferent;
            }
            _label_HighRiskCount.BackColor = 
                _label_HighRiskCount2.BackColor = color;

            color = Color.Transparent;
            if (_label_HighCount.Text != _label_HighCount2.Text)
            {
                m_isSummaryDiffFound = true;
                color = highlightColorDifferent;
            }            
            _label_HighCount.BackColor = 
                _label_HighCount2.BackColor = color;
            
            try
            {
                foreach (UltraGridRow row in _grid_Servers1.Rows)
                {
                    color = Color.Transparent;
                    if ((CompareResults)row.Cells[colDifferencesFound].Value == CompareResults.COMPARE_DIFFERENT)
                    {
                        // High Severity Column
                        color = Color.Transparent;
                        if ((int) row.Cells[colHigh1].Value != (int) row.Cells[colHigh2].Value)
                        {
                            m_isSummaryDiffFound = true;
                            color = highlightColorDifferent;
                        }
                        row.Cells[colHigh1].Appearance.BackColor = color;
                        row.Cells[colHigh2].Appearance.BackColor = color;

                        // Medium Severity Column
                        color = Color.Transparent;
                        if ((int) row.Cells[colMedium1].Value != (int) row.Cells[colMedium2].Value)
                        {
                            m_isSummaryDiffFound = true;
                            color = highlightColorDifferent;
                        }
                        row.Cells[colMedium1].Appearance.BackColor = color;
                        row.Cells[colMedium2].Appearance.BackColor = color;

                        // Low Severity Column
                        color = Color.Transparent;
                        if ((int) row.Cells[colLow1].Value != (int) row.Cells[colLow2].Value)
                        {
                            m_isSummaryDiffFound = true;
                            color = highlightColorDifferent;
                        }
                        row.Cells[colLow1].Appearance.BackColor = color;
                        row.Cells[colLow2].Appearance.BackColor = color;
                    }
                    else if((CompareResults)row.Cells[colDifferencesFound].Value == CompareResults.COMPARE_MISSING)
                    {
                        m_isSummaryDiffFound = true;
                        color = highlightColorDifferent;
                        row.Cells[colLow1].Appearance.BackColor = color;
                        row.Cells[colLow2].Appearance.BackColor = color;
                        row.Cells[colMedium1].Appearance.BackColor = color;
                        row.Cells[colMedium2].Appearance.BackColor = color;
                        row.Cells[colHigh1].Appearance.BackColor = color;
                        row.Cells[colHigh2].Appearance.BackColor = color;
                        row.Cells[colServer].Appearance.BackColor = color;
                    }

                }              
            }
            catch (Exception ex)
            {
                logX.loggerX.Error(@"Error - Highlighting Server differences", ex);
            }

            try
            {
                foreach (UltraGridRow row in _grid_ReportCard.Rows)
                {                    
                    color = Color.Transparent;
                    if ((CompareResults)row.Cells[colDifferencesFound].Value == CompareResults.COMPARE_DIFFERENT)
                    {
                        m_isSummaryDiffFound = true;
                        //row.Cells[colSummary1].Appearance.BackColor = highlightColorDifferent;
                        //row.Cells[colSummary2].Appearance.BackColor = highlightColorDifferent;
                        row.Cells[colDifferencesFound].ToolTipText = ToolTipDiffsFound;
                    }
                    else if ((CompareResults)row.Cells[colDifferencesFound].Value == CompareResults.COMPARE_MISSING)
                    {
                        m_isSummaryDiffFound = true;
                        //row.Cells[colSummary1].Appearance.BackColor = highlightColorDifferent;
                        //row.Cells[colSummary2].Appearance.BackColor = highlightColorDifferent;
                        row.Cells[colDifferencesFound].ToolTipText = ToolTipDiffsFound;
                    }
                    else
                    {
                        row.Cells[colDifferencesFound].ToolTipText = ToolTipNoDiffsFound;                        
                    }

                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error(@"Error - Highlighting Report Card differences", ex);
            }

            _ultraTabControl_Compare.Tabs[tabKey_CompareSummaries].Appearance.Image =
                m_isSummaryDiffFound
                    ? global::Idera.SQLsecure.UI.Console.Properties.Resources._32_CompareDifferent
                    : global::Idera.SQLsecure.UI.Console.Properties.Resources._32_CompareMatch;


        }

        private void HighlightInternalReviewNotesDifferences()
        {
            bool diffFound = false;
            if(label_InternalReviewNotesTitle1.Text != label_InternalReviewNotesTitle2.Text
                || textBox_InternalReviewNotes1.Text != textBox_InternalReviewNotes2.Text)
            {
                diffFound = true;
            }
            _ultraTabControl_Compare.Tabs[tabKey_CompareNotes].Appearance.Image =
                diffFound
                ? global::Idera.SQLsecure.UI.Console.Properties.Resources._32_CompareDifferent
                        : global::Idera.SQLsecure.UI.Console.Properties.Resources._32_CompareMatch;
        }

        private void UpdateSelectionFields(Control control, Policy p)
        {
            if (p.IsAssessment)
            {
                control.Text = string.Format(SelectionsFormat, p.UseBaseline ? Display_Baseline : string.Empty);
                if (p.AssessmentDate.HasValue)
                {
                    control.Text += "\nas of " + p.AssessmentDate.Value.ToLocalTime().ToString(Utility.Constants.DATETIME_FORMAT);
                }
            }
            else
            {
                control.Text = string.Format(SelectionsFormat, Program.gController.PolicyUseBaselineSnapshots ? Display_Baseline : string.Empty);
                if (Program.gController.PolicyTime.HasValue)
                {
                    control.Text += "\nas of " + Program.gController.PolicyTime.Value.ToLocalTime().ToString(Utility.Constants.DATETIME_FORMAT);
                }
            }            
        }

        private void UpdateAssessment1ToCurrentSelection()
        {
            SetDescriptionText();
            if (ultraCombo_Assessment1.SelectedItem != null)
            {

                ValueListItem item = ultraCombo_Assessment1.SelectedItem;
                Policy p = (Policy)item.Tag;
                m_assessmentId1 = p.AssessmentId;
                m_PolicyAssessment1 = m_Assessments.Find(m_assessmentId1);

                LoadRegisterServers();

//                UpdateAssessmentSummaryTitle(_ViewSection_Assessment1, p);                
                UpdateAssessmentSummaryTitle(_ultraGroupBox_RCDS1, p);
                UpdateAssessmentSummaryTitle(_ultraGroupBox_SCS1, p);
                UpdateAssessmentSummaryTitle(_ultraGroupBox_Finding1, p);
                UpdateAssessmentSummaryTitle(_ultraGroupBox_EN1, p);
                UpdateAssessmentSummaryTitle(viewSection_IR1, p);

                UpdateSelectionFields(_label_Selections1, p);

                _label_AssessmentName1.Text = GetDisplayNameForAssessmentInCombo(p);

                UpdateAssessmentSummaryDescription(_textBox_Description1, p);
//                UpdateAssessmentState(ultraGroupBox_Findings1, FindingsText, p);
//                UpdateAssessmentState(ultraGroupBox_ServerSummary1, ServerSummaryText, p);

                LoadReportCardAssessment1();
                LoadAssessmentSummary1();
                LoadInternalReviewNotes(label_InternalReviewNotesTitle1, textBox_InternalReviewNotes1, m_PolicyAssessment1);

                LoadComparisonDataSource();
                LoadReportCardDataSource();
                LoadServerSummaryDataSource();

                HighlightSummaryDifferences();

            }
        }

        private void UpdateAssessment2ToCurrentSelection()
        {
            SetDescriptionText();
            if (ultraCombo_Assessment2.SelectedItem != null)
            {

                ValueListItem item = ultraCombo_Assessment2.SelectedItem;
                Policy p = (Policy)item.Tag;
                m_assessmentId2 = p.AssessmentId;
                m_PolicyAssessment2 = m_Assessments.Find(m_assessmentId2);

                // Verify Registered Servers exist for both Assessments
                ValidateServerChoice();

                //UpdateAssessmentSummaryTitle(_viewSection_Assessment2, p);
                UpdateAssessmentSummaryTitle(_ultraGroupBox_RCDS2, p);
                UpdateAssessmentSummaryTitle(_ultraGroupBox_SCS2, p);
                UpdateAssessmentSummaryTitle(_ultraGroupBox_Finding2, p);
                UpdateAssessmentSummaryTitle(_ultraGroupBox_EN2, p);
                UpdateAssessmentSummaryTitle(viewSection_IR2, p);

                UpdateSelectionFields(_label_Selections2, p);

                _label_AssessmentName2.Text = GetDisplayNameForAssessmentInCombo(p);

                UpdateAssessmentSummaryDescription(_textBox_Description2, p);
//                UpdateAssessmentState(ultraGroupBox_Findings2, FindingsText, p);
//                UpdateAssessmentState(ultraGroupBox_ReportCardSummary, ServerSummaryText, p);

                LoadReportCardAssessment2();
                LoadAssessmentSummary2();
                LoadInternalReviewNotes(label_InternalReviewNotesTitle2, textBox_InternalReviewNotes2, m_PolicyAssessment2);


                // Do Comparison
                LoadComparisonDataSource();
                LoadReportCardDataSource();
                LoadServerSummaryDataSource();
                HighlightSummaryDifferences();
            }
        }

        private void LoadServerSummaryDataSource()
        {
            // Load Server Summary

           // if (m_server1 == null)
            {
                _grid_Servers1.SuspendLayout();

                m_serverTable1.Clear();

                foreach (KeyValuePair<string, ReportCard.RiskCounts> server in _reportCard1.ServerRisks)
                {
                    DataRow row = m_serverTable1.NewRow();
                    row[colServer] = server.Key;
                    row[colHigh1] = server.Value.RiskCountHigh;
                    row[colMedium1] = server.Value.RiskCountMedium;
                    row[colLow1] = server.Value.RiskCountLow;
                    row[colDateTime1] = server.Value.CollectionDateTime == DateTime.MinValue ? ServerNoAuditData : server.Value.CollectionDateTime.ToString();
                    ReportCard.RiskCounts counts2 = null;
                    if(_reportCard2.ServerRisks.TryGetValue(server.Key, out counts2))
                    {
                        row[colHigh2] = counts2.RiskCountHigh;
                        row[colMedium2] = counts2.RiskCountMedium;
                        row[colLow2] = counts2.RiskCountLow;
                        row[colDateTime2] = counts2.CollectionDateTime == DateTime.MinValue ? ServerNoAuditData : counts2.CollectionDateTime.ToString();       
                        if(row[colHigh1] == row[colHigh2]
                            && row[colMedium1] == row[colMedium2]
                            && row[colLow1] == row[colLow2])
                        {
                            row[colDifferencesFound] = CompareResults.COMPARE_MATCH;
                        }
                        else
                        {
                            row[colDifferencesFound] = CompareResults.COMPARE_DIFFERENT;
                        }
                    }
                    else
                    {
                        row[colHigh2] = 0;
                        row[colMedium2] = 0;
                        row[colLow2] = 0;
                        if (DoesServerExistInAssessment(m_PolicyAssessment2, server.Key))
                        {
                            row[colDateTime2] = ServerNoAuditData;
                        }
                        else
                        {
                            row[colDateTime2] = ServerNotInAssessment;
                        }
                        row[colDifferencesFound] = CompareResults.COMPARE_DIFFERENT;
                    }

                    m_serverTable1.Rows.Add(row);
                }

                foreach (KeyValuePair<string, ReportCard.RiskCounts> server in _reportCard2.ServerRisks)
                {
                    if (!_reportCard1.ServerRisks.ContainsKey(server.Key))
                    {
                        DataRow row = m_serverTable1.NewRow();
                        row[colServer] = server.Key;
                        row[colHigh2] = server.Value.RiskCountHigh;
                        row[colMedium2] = server.Value.RiskCountMedium;
                        row[colLow2] = server.Value.RiskCountLow;
                        row[colDateTime2] = server.Value.CollectionDateTime == DateTime.MinValue ? ServerNoAuditData : server.Value.CollectionDateTime.ToString();
                        row[colHigh1] = 0;
                        row[colMedium1] = 0;
                        row[colLow1] = 0;
                        if (DoesServerExistInAssessment(m_PolicyAssessment1, server.Key))
                        {
                            row[colDateTime1] = ServerNoAuditData;
                        }
                        else
                        {
                            row[colDateTime1] = ServerNotInAssessment;
                        }
                        row[colDifferencesFound] = CompareResults.COMPARE_DIFFERENT;
                        m_serverTable1.Rows.Add(row);
                    }
                }

                DataView dv = m_serverTable1.DefaultView;
                dv.Sort = colServer;

                _grid_Servers1.SetDataBinding(m_serverTable1.DefaultView, null);

                _toolStripLabel_Server1.Text = string.Format(HeaderDisplay,
                                                    dv.Count,
                                                    dv.Count == 1 ? string.Empty : "s");

                //Make sure the counts are always displayed correctly, they can sometimes get cut off when there is a double digit value
                _grid_Servers1.DisplayLayout.Bands[0].Columns[colHigh1].PerformAutoResize(PerformAutoSizeType.AllRowsInBand);
                _grid_Servers1.DisplayLayout.Bands[0].Columns[colMedium1].PerformAutoResize(PerformAutoSizeType.AllRowsInBand);
                _grid_Servers1.DisplayLayout.Bands[0].Columns[colLow1].PerformAutoResize(PerformAutoSizeType.AllRowsInBand);
                _grid_Servers1.DisplayLayout.Bands[0].Columns[colHigh2].PerformAutoResize(PerformAutoSizeType.AllRowsInBand);
                _grid_Servers1.DisplayLayout.Bands[0].Columns[colMedium2].PerformAutoResize(PerformAutoSizeType.AllRowsInBand);
                _grid_Servers1.DisplayLayout.Bands[0].Columns[colLow2].PerformAutoResize(PerformAutoSizeType.AllRowsInBand);

                _grid_Servers1.ResumeLayout();

            }
            //else
            //{
            //    _grid_Servers1.DataSource = null;
            //}


        }

        private void LoadAssessmentSummary1()
        {
            try
            {
                int high = m_PolicyAssessment1.MetricCountHigh;
                int medium = m_PolicyAssessment1.MetricCountMedium;
                int low = m_PolicyAssessment1.MetricCountLow;

                int highFindings = m_PolicyAssessment1.FindingCountHigh;
                int mediumFindings = m_PolicyAssessment1.FindingCountMedium;
                int lowFindings = m_PolicyAssessment1.FindingCountLow;

                if (m_server1 != null)
                {
                    m_PolicyAssessment1.GetPolicyFindingForServer(m_server1.RegisteredServerId,
                                out highFindings, out mediumFindings, out lowFindings);
                }

                // Set the assessment statuses

                //High
                _pictureBox_SecurityStatusHigh.Image =
                    global::Idera.SQLsecure.UI.Console.Properties.Resources.HighRisk_32;
                _label_High.Text =
                    string.Format(BarLabelDisplay, DescriptionHelper.GetEnumDescription(Utility.Policy.Severity.High));
                _label_HighCount.Text = string.Format(BarHighCountDisplay, high);
                _label_HighRiskCount.Text = highFindings.ToString();

                SizeRiskBar(high, highFindings, _label_HighMsg, _label_HighRiskBar, _label_HighBar);

                //Medium
                _pictureBox_SecurityStatusMedium.Image =
                    global::Idera.SQLsecure.UI.Console.Properties.Resources.MediumRisk_32;
                _label_Medium.Text =
                    string.Format(BarLabelDisplay, DescriptionHelper.GetEnumDescription(Utility.Policy.Severity.Medium));
                _label_MediumCount.Text = string.Format(BarHighCountDisplay, medium);
                _label_MediumRiskCount.Text = mediumFindings.ToString();

                SizeRiskBar(medium, mediumFindings, _label_MediumMsg, _label_MediumRiskBar, _label_MediumBar);

                //Low
                _pictureBox_SecurityStatusLow.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.LowRisk_32;
                _label_Low.Text =
                    string.Format(BarLabelDisplay, DescriptionHelper.GetEnumDescription(Utility.Policy.Severity.Low));
                _label_LowCount.Text = string.Format(BarHighCountDisplay, low);
                _label_LowRiskCount.Text = lowFindings.ToString();

                SizeRiskBar(low, lowFindings, _label_LowMsg, _label_LowRiskBar, _label_LowBar);

            }

            catch (Exception ex)
            {
                logX.loggerX.Error(@"Error - Unable to retrieve policy assessment summary info", ex);
                MsgBox.ShowError(string.Format(ErrorMsgs.CantGetPolicyInfoMsg, "Policy Assessment Summary Info"),
                                 ErrorMsgs.ErrorProcessPolicyInfo,
                                 ex);
            }
        }

        private void LoadAssessmentSummary2()
        {
            if (m_PolicyAssessment2 != null)
            {
                try
                {
                    int high = m_PolicyAssessment2.MetricCountHigh;
                    int medium = m_PolicyAssessment2.MetricCountMedium;
                    int low = m_PolicyAssessment2.MetricCountLow;

                    int highFindings = m_PolicyAssessment2.FindingCountHigh;
                    int mediumFindings = m_PolicyAssessment2.FindingCountMedium;
                    int lowFindings = m_PolicyAssessment2.FindingCountLow;

                    if (m_server1 != null)
                    {
                        m_PolicyAssessment2.GetPolicyFindingForServer(m_server1.RegisteredServerId,
                                    out highFindings, out mediumFindings, out lowFindings);
                    }

                    // Set the assessment statuses

                    //High
                    _pictureBox_SecurityStatusHigh2.Image =
                        global::Idera.SQLsecure.UI.Console.Properties.Resources.HighRisk_32;
                    _label_High2.Text =
                        string.Format(BarLabelDisplay, DescriptionHelper.GetEnumDescription(Utility.Policy.Severity.High));
                    _label_HighCount2.Text = string.Format(BarHighCountDisplay, high);
                    _label_HighRiskCount2.Text = highFindings.ToString();

                    SizeRiskBar(high, highFindings, _label_HighMsg2, _label_HighRiskBar2, _label_HighBar2);


                    //Medium
                    _pictureBox_SecurityStatusMedium2.Image =
                        global::Idera.SQLsecure.UI.Console.Properties.Resources.MediumRisk_32;
                    _label_Medium2.Text =
                        string.Format(BarLabelDisplay, DescriptionHelper.GetEnumDescription(Utility.Policy.Severity.Medium));
                    _label_MediumCount2.Text = string.Format(BarHighCountDisplay, medium);
                    _label_MediumRiskCount2.Text = mediumFindings.ToString();

                    SizeRiskBar(medium, mediumFindings, _label_MediumMsg2, _label_MediumRiskBar2, _label_MediumBar2);

                    //Low
                    _pictureBox_SecurityStatusLow2.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.LowRisk_32;
                    _label_Low2.Text =
                        string.Format(BarLabelDisplay, DescriptionHelper.GetEnumDescription(Utility.Policy.Severity.Low));
                    _label_LowCount2.Text = string.Format(BarHighCountDisplay, low);
                    _label_LowRiskCount2.Text = lowFindings.ToString();

                    SizeRiskBar(low, lowFindings, _label_LowMsg2, _label_LowRiskBar2, _label_LowBar2);                   

                }

                catch (Exception ex)
                {
                    logX.loggerX.Error(@"Error - Unable to retrieve policy assessment summary info", ex);
                    MsgBox.ShowError(string.Format(ErrorMsgs.CantGetPolicyInfoMsg, "Policy Assessment Summary Info"),
                                     ErrorMsgs.ErrorProcessPolicyInfo,
                                     ex);
                }
            }
        }

        private void SizeRiskBar(int totalRisks, int findings, Label label_Msg, Label label_Risk, Label label_TotalRisk)
        {
            int BarMax = label_Msg.Width;
            int BarLeft = label_Msg.Left;
            if (totalRisks > 0)
            {
                label_Msg.Visible =
                    label_Risk.Visible =
                    label_TotalRisk.Visible = false;

                if (findings > 0)
                {
                    label_Risk.Width = Convert.ToInt16(BarMax * findings / totalRisks);
                    label_TotalRisk.Left = label_Risk.Right - 1;
                    label_TotalRisk.Width = BarMax - label_Risk.Width;
                }
                else
                {
                    label_Risk.Width = 0;
                    label_TotalRisk.Left = BarLeft;
                    label_TotalRisk.Width = BarMax;
                }
                label_Risk.Visible =
                    label_TotalRisk.Visible = true;
            }
            else
            {
                label_Risk.Text = string.Empty;
                label_Msg.Text = NoMetrics;
                label_Msg.Visible = true;
                label_Risk.Visible =
                    label_TotalRisk.Visible = false;
            }
        }

        private void LoadReportCardAssessment1()
        {
            if (m_PolicyAssessment1.IsPolicy)
            {
                ((Interfaces.IView)_reportCard1).SetContext(new Data.ReportCard(m_PolicyAssessment1, 
                                                                Program.gController.PolicyUseBaselineSnapshots,
                                                                Program.gController.PolicyTime.HasValue ? Program.gController.PolicyTime.Value : DateTime.Now.ToUniversalTime(), 
                                                                m_server1));
            }
            else
            {
                ((Interfaces.IView)_reportCard1).SetContext(new Data.ReportCard(m_PolicyAssessment1, m_server1));
            }
        }

        private void LoadReportCardAssessment2()
        {
            if (m_PolicyAssessment2.IsPolicy)
            {
                ((Interfaces.IView)_reportCard2).SetContext(new Data.ReportCard(m_PolicyAssessment2,
                                                                Program.gController.PolicyUseBaselineSnapshots,
                                                                Program.gController.PolicyTime.HasValue ? Program.gController.PolicyTime.Value : DateTime.Now.ToUniversalTime(),
                                                                m_server1));
            }
            else
            {
                ((Interfaces.IView)_reportCard2).SetContext(new Data.ReportCard(m_PolicyAssessment2, m_server1));
            }
        }

        private void LoadInternalReviewNotes(Control controlTitle, Control controlText, Policy p)
        {
            if (string.IsNullOrEmpty(p.InterviewName))
            {
                controlTitle.Text = string.Empty;
            }
            else
            {
                controlTitle.Text = p.InterviewName;                
            }
            if(  string.IsNullOrEmpty(p.InterviewText))
            {
                controlText.Text = "No Internal Review Notes Specified";                
            }
            else
            {
                controlText.Text = p.InterviewText;
            }

            HighlightInternalReviewNotesDifferences();
        }       

        private void UpdateAssessmentSummaryTitle(ViewSection viewSection, Policy p)
        {
            viewSection.Title = GetDisplayNameForAssessmentInCombo(p);
        }

        private void UpdateAssessmentSummaryTitle(UltraGroupBox groupBox, Policy p)
        {
            groupBox.Text = GetDisplayNameForAssessmentInCombo(p);
        }

        private string GetAssessmentStateText(Policy p)
        {
            return p.AssessmentStateName;
        }

        private void UpdateAssessmentState(UltraGroupBox groupBox, string textFormat, Policy p)
        {           
            groupBox.Text = string.Format(textFormat, GetAssessmentStateText(p));
        }

        private void UpdateAssessmentSummaryDescription(TextBox textEditor, Policy p)
        {
            textEditor.Text = p.AssessmentDescription;
        }

        private bool ValidateServerChoice()
        {
            bool valid = true;
            if (m_PolicyAssessment2 != null)
            {
                if (m_server1 == null)
                {
                    _ultraTextEditor_Server2.Text = "All Servers";
                }
                else
                {
                    if (!DoesServerExistInAssessment2(m_server1.RegisteredServerId))
                    {
                        valid = false;
                        string msg =
                            string.Format(
                                "Server {0} doesn't exist in assessment {1}.\r\nPlease choose a different Server.",
                                m_server1.ConnectionName, m_PolicyAssessment2.AssessmentName);
                        MsgBox.ShowWarning("Assessment Comparison", msg);
                    }
                    else
                    {
                        _ultraTextEditor_Server2.Text = m_server1.ConnectionName;
                    }
                }
            }
            return valid;
        }

        private bool DoesServerExistInAssessment2(int serverId)
        {
            bool exist = false;
            if (m_PolicyAssessment2 != null)
            {
                foreach (RegisteredServer r in m_PolicyAssessment2.Members)
                {
                    if (r.RegisteredServerId == serverId)
                    {
                        exist = true;
                        break;
                    }
                }
            }
            return exist;
        }

        #endregion

        #region Comparison

        private DataTable createDataSourceExplanations()
        {
            // Create Report Card default datasources
            DataTable dt = new DataTable();

            dt.Columns.Add(colServerName, typeof (string));
            dt.Columns.Add(colMetricId, typeof (int));
            dt.Columns.Add(colSnapshotId, typeof (int));
            dt.Columns.Add(colIsExplained, typeof (bool));
            dt.Columns.Add(colNotes, typeof (string));

            return dt;
        }

        private DataTable createDataSourceComparisonGridTable()
        {
            // Create Report Card default datasources
            DataTable dt = new DataTable();

            dt.Columns.Add(colDifferencesFound, typeof(int));
            dt.Columns.Add(colMetricId, typeof(int));
            dt.Columns.Add(colMetricName, typeof(string));
            dt.Columns.Add(colFindingDiff, typeof(int));
            dt.Columns.Add(colFindingDiffText, typeof(string));
            dt.Columns.Add(colExplanationDiff, typeof(int));
            dt.Columns.Add(colExplanationDiffText, typeof(string));
            dt.Columns.Add(colReportCardDisplayDiff, typeof(int));
            dt.Columns.Add(colReportCardDisplayDiffText, typeof(string));
            dt.Columns.Add(colSecurityCheckDiff, typeof(int));
            dt.Columns.Add(colSecurityCheckDiffText, typeof(string));

            return dt;
        }

        private void LoadComparisonDataSource()
        {
            int nTotalDiffs = 0;
            int nMetricId = 0;
            bool bDiffFound = false;
            bool bFindingDiff = false;
            string strFindingDiff = string.Empty;
            bool bExplanationDiff = false;
            string strExplanationDiff = string.Empty;
            string strValue1 = string.Empty;
            string strValue2 = string.Empty;
            CompareResults compareResults = CompareResults.COMPARE_MATCH;

            if (m_assessmentId1 > 0 && m_assessmentId2 > 0)
            {
                try
                {
                    if (!string.IsNullOrEmpty(Program.gController.Repository.ConnectionString))
                    {
                        // Retrieve policy information.
                        logX.loggerX.Info("Retrieve Policy Assessments");
                        DataSet ds = new DataSet();

                        using (
                            SqlConnection connection =
                                new SqlConnection(Program.gController.Repository.ConnectionString)
                            )
                        {
                            // Open the connection.
                            connection.Open();

                            SqlParameter paramPolicyId = new SqlParameter(ParamPolicyId, m_policyId);
                            SqlParameter paramAssessmentId1 = new SqlParameter(ParamAssessmentId1, m_assessmentId1);
                            SqlParameter paramAssessmentId2 = new SqlParameter(ParamAssessmentId2, m_assessmentId2);
                            SqlParameter paramRegisteredServerId =
                                new SqlParameter(ParamRegisteredServerId, System.Data.SqlTypes.SqlInt32.Zero);
                            if (m_server1 != null)
                            {
                                paramRegisteredServerId =
                                    new SqlParameter(ParamRegisteredServerId, m_server1.RegisteredServerId);
                            }
                            int diffonly = (m_showSCDifferencesOnly) ? 1 : 0;
                            SqlParameter paramDiffsOnly = new SqlParameter(ParamDiffsOnly, diffonly);

                            SqlCommand cmd = new SqlCommand(QueryGetAssessmentComparison, connection);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();

                            cmd.Parameters.Add(paramPolicyId);
                            cmd.Parameters.Add(paramAssessmentId1);
                            cmd.Parameters.Add(paramAssessmentId2);
                            cmd.Parameters.Add(paramRegisteredServerId);
                            cmd.Parameters.Add(paramDiffsOnly);

                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            da.Fill(ds);
                        }
                        m_ComparisonTable = ds.Tables[0];
                        m_ComparisonTable.DefaultView.Sort =
                            string.Format("{0} DESC, {1}", colDifferencesFound, colMetricId);

                        // Create a new Comparison Summary by processing the detail and creating a summary
                        m_ComparisonSummaryTable = createDataSourceComparisonGridTable();

                        metricDifferenceDict.Clear();

                        if (m_ComparisonTable.DefaultView.Count > 0)
                        {
                            DataRow newRow = m_ComparisonSummaryTable.NewRow();
                            
                            foreach (DataRowView drv in m_ComparisonTable.DefaultView)
                            {
                                if (nMetricId != (int) drv[colMetricId])
                                {
                                    if (nMetricId > 0)
                                    {
                                        // Output previous metric;
                                        if (bDiffFound)
                                        {
                                            nTotalDiffs++;
                                        }

                                        // Store metric difference
                                        compareResults = CompareResults.COMPARE_MATCH;
                                        if (bDiffFound)
                                        {
                                            compareResults = CompareResults.COMPARE_DIFFERENT;
                                            if (string.IsNullOrEmpty(strValue1) || string.IsNullOrEmpty(strValue2))
                                            {
                                                compareResults = CompareResults.COMPARE_DIFFERENT;
                                            }
                                        }
                                        if (!metricDifferenceDict.ContainsKey(nMetricId))
                                        {
                                            metricDifferenceDict.Add(nMetricId, compareResults);

                                            newRow[colDifferencesFound] = bDiffFound
                                                                              ? CompareResults.COMPARE_DIFFERENT
                                                                              : CompareResults.COMPARE_MATCH;
                                            newRow[colFindingDiff] = bFindingDiff
                                                                         ? CompareResults.COMPARE_DIFFERENT
                                                                         : CompareResults.COMPARE_MATCH;
                                            newRow[colFindingDiffText] = strFindingDiff;
                                            newRow[colExplanationDiff] = bExplanationDiff
                                                                             ? CompareResults.COMPARE_DIFFERENT
                                                                             : CompareResults.COMPARE_MATCH;
                                            newRow[colExplanationDiffText] = strExplanationDiff;
                                            m_ComparisonSummaryTable.Rows.Add(newRow);
                                            newRow = m_ComparisonSummaryTable.NewRow();
                                        }

                                        // Reset counters
                                        bDiffFound = false;
                                        bFindingDiff = false;
                                        strFindingDiff = string.Empty;
                                        bExplanationDiff = false;
                                        strExplanationDiff = string.Empty;
                                    }

                                    // These don't change based on Server
                                    nMetricId = (int) drv[colMetricId];
                                    newRow[colMetricId] = nMetricId;
                                    newRow[colMetricName] = drv[colMetricName];
                                    newRow[colReportCardDisplayDiff] = (bool) drv[colReportCardDisplayDiff]
                                                                           ? CompareResults.COMPARE_DIFFERENT
                                                                           : CompareResults.COMPARE_MATCH;
                                    newRow[colReportCardDisplayDiffText] = drv[colReportCardDisplayDiffText];
                                    newRow[colSecurityCheckDiff] = (bool) drv[colSecurityCheckDiff]
                                                                       ? CompareResults.COMPARE_DIFFERENT
                                                                       : CompareResults.COMPARE_MATCH;
                                    newRow[colSecurityCheckDiffText] = drv[colSecurityCheckDiffText];
                                }
                                // Process all servers in this policy
                                if (!bDiffFound)
                                {
                                    bDiffFound = (bool) drv[colDifferencesFound];
                                }
                                if (!bFindingDiff)
                                {
                                    bFindingDiff = (bool) drv[colFindingDiff];
                                    strFindingDiff = (string) drv[colFindingDiffText];
                                }
                                else if (string.IsNullOrEmpty((string) drv[colFindingDiffText]))
                                {
                                    strFindingDiff = strFindingDiff + ", " + (string) drv[colFindingDiffText];
                                }
                                if (!bExplanationDiff)
                                {
                                    bExplanationDiff = (bool) drv[colExplanationDiff];
                                    strExplanationDiff = (string) drv[colExplanationDiffText];
                                }
                                else if (string.IsNullOrEmpty((string) drv[colExplanationDiffText]))
                                {
                                    strExplanationDiff = strExplanationDiff + ", " +
                                                         (string) drv[colExplanationDiffText];
                                }
                                strValue1 = drv[colCurrentValue1] == DBNull.Value
                                                ? string.Empty
                                                : (string) drv[colCurrentValue1];
                                strValue2 = drv[colCurrentValue2] == DBNull.Value
                                                ? string.Empty
                                                : (string) drv[colCurrentValue2];
                            }
                            // Output last metric
                            if (bDiffFound)
                            {
                                nTotalDiffs++;
                            }
                            // Store metric difference
                            compareResults = CompareResults.COMPARE_MATCH;
                            if (bDiffFound)
                            {
                                compareResults = CompareResults.COMPARE_DIFFERENT;
                                if (string.IsNullOrEmpty(strValue1) || string.IsNullOrEmpty(strValue2))
                                {
                                    compareResults = CompareResults.COMPARE_DIFFERENT;
                                }
                            }
                            if (!metricDifferenceDict.ContainsKey(nMetricId))
                            {
                                metricDifferenceDict.Add(nMetricId, compareResults);

                                newRow[colDifferencesFound] = bDiffFound
                                                                  ? CompareResults.COMPARE_DIFFERENT
                                                                  : CompareResults.COMPARE_MATCH;
                                newRow[colFindingDiff] = bFindingDiff
                                                             ? CompareResults.COMPARE_DIFFERENT
                                                             : CompareResults.COMPARE_MATCH;
                                newRow[colFindingDiffText] = strFindingDiff;
                                newRow[colExplanationDiff] = bExplanationDiff
                                                                 ? CompareResults.COMPARE_DIFFERENT
                                                                 : CompareResults.COMPARE_MATCH;
                                newRow[colExplanationDiffText] = strExplanationDiff;
                                m_ComparisonSummaryTable.Rows.Add(newRow);
                            }

                            if (m_ComparisonSummaryTable.Rows.Count > 0)
                            {
                                if (m_showSCDifferencesOnly && nTotalDiffs == 0)
                                {
                                    _splitContainer_CompareSummary.Visible = false;
                                    _ultraLabel_NoDiffsFound.Text = ComparisonTextNoDifferences;
                                    _ultraLabel_NoDiffsFound.Visible = true;
                                }
                                else
                                {
                                    _splitContainer_CompareSummary.Visible = true;
                                    _ultraLabel_NoDiffsFound.Visible = false;
                                }

                                // Save the user configuration of the grid to restore it after setting the datasource again
                                Utility.GridSettings gridSettings = GridSettings.GetSettings(_grid_ComparisonSummary);
                                bool initializing = _grid_ComparisonSummary.DataSource == null;
                                _grid_ComparisonSummary.SetDataBinding(m_ComparisonSummaryTable.DefaultView, null);

                                // Restore Grid Settings
                                if (!initializing)
                                {
                                    GridSettings.ApplySettingsToGrid(gridSettings, _grid_ComparisonSummary);
                                }
                                _grid_ComparisonSummary.Selected.Rows.Clear();
                                _grid_ComparisonSummary.Selected.Rows.Add(_grid_ComparisonSummary.Rows[0]);
                                try
                                {
                                    foreach (UltraGridRow row in _grid_ComparisonSummary.Rows)
                                    {
                                        if ((CompareResults) row.Cells[colFindingDiff].Value ==
                                            CompareResults.COMPARE_DIFFERENT)
                                        {
                                            row.Cells[colFindingDiff].ToolTipText =
                                                string.Format(FindingToolTipFormat, row.Cells[colFindingDiffText].Value);
                                        }
                                        else
                                        {
                                            row.Cells[colFindingDiff].ToolTipText = ToolTipNoDiffsFound;
                                        }
                                        if ((CompareResults) row.Cells[colExplanationDiff].Value ==
                                            CompareResults.COMPARE_DIFFERENT)
                                        {
                                            row.Cells[colExplanationDiff].ToolTipText =
                                                string.Format(FindingToolTipFormat,
                                                              row.Cells[colExplanationDiffText].Value);
                                        }
                                        else
                                        {
                                            row.Cells[colExplanationDiff].ToolTipText = ToolTipNoDiffsFound;
                                        }
                                        if ((CompareResults) row.Cells[colReportCardDisplayDiff].Value ==
                                            CompareResults.COMPARE_DIFFERENT)
                                        {
                                            row.Cells[colReportCardDisplayDiff].ToolTipText =
                                                string.Format(FindingToolTipFormat,
                                                              row.Cells[colReportCardDisplayDiffText].Value);
                                        }
                                        else
                                        {
                                            row.Cells[colReportCardDisplayDiff].ToolTipText = ToolTipNoDiffsFound;
                                        }
                                        if ((CompareResults) row.Cells[colSecurityCheckDiff].Value ==
                                            CompareResults.COMPARE_DIFFERENT)
                                        {
                                            row.Cells[colSecurityCheckDiff].ToolTipText =
                                                string.Format(FindingToolTipFormat,
                                                              row.Cells[colSecurityCheckDiffText].Value);
                                        }
                                        else
                                        {
                                            row.Cells[colSecurityCheckDiff].ToolTipText = ToolTipNoDiffsFound;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    logX.loggerX.Error(@"Error - Highlighting Report Card differences", ex);
                                }
                            }
                        }
                        else
                        {
                            _grid_ComparisonSummary.SetDataBinding(m_ComparisonSummaryTable.DefaultView, null);
                            if (m_showSCDifferencesOnly)
                            {
                                _splitContainer_CompareSummary.Visible = false;
                                _ultraLabel_NoDiffsFound.Text = ComparisonTextNoDifferences;
                                _ultraLabel_NoDiffsFound.Visible = true;
                            }
                            else
                            {
                                _splitContainer_CompareSummary.Visible = false;
                                _ultraLabel_NoDiffsFound.Text = ComparisonTextNoAuditData;
                                _ultraLabel_NoDiffsFound.Visible = true;
                            }
                        }

                        _label_ComparisonSummary.Text = string.Format(ComparisonSummaryHeader,
                                              nTotalDiffs == 0
                                                  ? "No"
                                                  : nTotalDiffs.ToString(),
                                              nTotalDiffs == 1
                                                  ? DifferenceSingular
                                                  : DifferencePlural);


                        _ultraTabControl_Compare.Tabs[tabKey_CompareFindings].Appearance.Image =
                            nTotalDiffs > 0
                                ? global::Idera.SQLsecure.UI.Console.Properties.Resources._32_CompareDifferent
                                : global::Idera.SQLsecure.UI.Console.Properties.Resources._32_CompareMatch;

                    }
                }
                catch (SqlException ex)
                {
                    logX.loggerX.Error(
                        string.Format(Utility.ErrorMsgs.ErrorStub, Utility.ErrorMsgs.CantGetAssessmentComparison), ex);
                    MsgBox.ShowError(Utility.ErrorMsgs.CantGetAssessmentComparison, ex.Message);
                    _splitContainer_CompareSummary.Visible = false;
                    _ultraLabel_NoDiffsFound.Text = ComparisonTextNoAuditData;
                    _ultraLabel_NoDiffsFound.Visible = true;
                }
                catch (Exception ex)
                {
                    logX.loggerX.Error(
                        string.Format(Utility.ErrorMsgs.ErrorStub, Utility.ErrorMsgs.CantGetAssessmentComparison), ex);
                    MsgBox.ShowError(Utility.ErrorMsgs.CantGetAssessmentComparison, ex.Message);
                }
            }
        }

        #endregion


        #region Common Grid Buttons

        protected void printGrid(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            Debug.Assert(grid.Tag.GetType() == typeof(ToolStripLabel));

            // Associate the print document with the grid & preview dialog here
            // in case we want to change documents for each grid
            _ultraGridPrintDocument.Grid = grid;
            _ultraGridPrintDocument.DefaultPageSettings.Color = false;
            _ultraGridPrintDocument.DocumentName = ErrorMsgs.UserPermissionsCaption;

            if (m_PolicyAssessment1 != null)
            {
                _ultraGridPrintDocument.Header.TextLeft =
                    string.Format(PrintHeaderDisplay,
                                        m_PolicyAssessment1.PolicyName,
                                        DateTime.Now,
                                        ((ToolStripLabel)grid.Tag).Text
                                    );
            }
            else
            {
                _ultraGridPrintDocument.Header.TextLeft =
                    string.Format(PrintEmptyHeaderDisplay,
                                        ((ToolStripLabel)grid.Tag).Text
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
            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Dictionary<string, ValueListDisplayStyle> styles = new Dictionary<string, ValueListDisplayStyle>();

                try
                {
                    // save the ValueList DisplayStyle settings for image columns to restore them afterwards
                    foreach (ValueList valueList in grid.DisplayLayout.ValueLists)
                    {
                        styles.Add(valueList.Key, valueList.DisplayStyle);
                        valueList.DisplayStyle = ValueListDisplayStyle.DisplayText;
                    }

                    _ultraGridExcelExporter.Export(grid, _saveFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MsgBox.ShowError(ErrorMsgs.ExportToExcelCaption, ErrorMsgs.FailedToExportToExcelFile, ex);
                }

                foreach (KeyValuePair<string, ValueListDisplayStyle> style in styles)
                {
                    grid.DisplayLayout.ValueLists[style.Key].DisplayStyle = style.Value;
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

            if (grid == _grid_ReportCard)
            {
                gridHeading = _viewSection_ReportCardComparisonSummary.Title;
            }
            else if (grid == _grid_ComparisonSummary)
            {
                gridHeading = _viewSection_CompareSummary.Title;
            }

            Forms.Form_GridColumnChooser.Process(grid, gridHeading);
        }

        protected void toggleGridGroupByBox(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            grid.DisplayLayout.GroupByBox.Hidden = !grid.DisplayLayout.GroupByBox.Hidden;
        }

        #endregion

        #region Grid

        private void _grid_ComparisonSummary_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            e.Layout.Override.GroupByRowExpansionStyle = GroupByRowExpansionStyle.Disabled;
            e.Layout.Override.GroupByRowInitialExpansionState = GroupByRowInitialExpansionState.Expanded;
            e.Layout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.ColumnChooserEnabled = DefaultableBoolean.False;

            UltraGridBand band = e.Layout.Bands[0];
            EditorWithText textEditor = new EditorWithText();

            band.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            band.Columns[colDifferencesFound].Header.Caption = "Status";
            band.Columns[colDifferencesFound].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            band.Columns[colDifferencesFound].ValueList = e.Layout.ValueLists[valueListDifference];
            band.Columns[colDifferencesFound].Editor = textEditor;
            band.Columns[colDifferencesFound].Width = 40;
            band.Columns[colDifferencesFound].MaxWidth = 40;

            band.Columns[colMetricId].Hidden = true;
            band.Columns[colMetricId].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            band.Columns[colMetricName].Header.Caption = "Security Check";
            band.Columns[colMetricName].Width = 200;
            band.Columns[colMetricName].AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.VisibleRows;

            band.Columns[colReportCardDisplayDiff].Header.Caption = "Display Settings";
            band.Columns[colReportCardDisplayDiff].Width = 110;
            band.Columns[colReportCardDisplayDiff].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            band.Columns[colReportCardDisplayDiff].ValueList = e.Layout.ValueLists[valueListDifference];
            band.Columns[colReportCardDisplayDiff].Editor = textEditor;

            band.Columns[colReportCardDisplayDiffText].Header.Caption = "Display Settings Differences";
            band.Columns[colReportCardDisplayDiffText].Hidden = true;

            band.Columns[colSecurityCheckDiff].Header.Caption = "Criteria";
            band.Columns[colSecurityCheckDiff].Width = 110;
            band.Columns[colSecurityCheckDiff].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            band.Columns[colSecurityCheckDiff].ValueList = e.Layout.ValueLists[valueListDifference];
            band.Columns[colSecurityCheckDiff].Editor = textEditor;

            band.Columns[colSecurityCheckDiffText].Header.Caption = "Security Check Differences"; 
            band.Columns[colSecurityCheckDiffText].Hidden = true;

            band.Columns[colFindingDiff].Header.Caption = "Findings";
            band.Columns[colFindingDiff].Width = 110;
            band.Columns[colFindingDiff].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            band.Columns[colFindingDiff].ValueList = e.Layout.ValueLists[valueListDifference];
            band.Columns[colFindingDiff].Editor = textEditor;

            band.Columns[colFindingDiffText].Header.Caption = "Findings Differences";
            band.Columns[colFindingDiffText].Hidden = true;

            band.Columns[colExplanationDiff].Header.Caption = "Explanation Notes";
            band.Columns[colExplanationDiff].Width = 110;
            band.Columns[colExplanationDiff].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            band.Columns[colExplanationDiff].ValueList = e.Layout.ValueLists[valueListDifference];
            band.Columns[colExplanationDiff].Editor = textEditor;

            band.Columns[colExplanationDiffText].Header.Caption = "Explanation Notes Differences";
            band.Columns[colExplanationDiffText].Hidden = true;

        }


        #endregion

        #region Events

        private void AssessmentCombo1Changed()
        {
            if (ultraCombo_Assessment1.SelectedItem != null)
            {
                if (ultraCombo_Assessment1.SelectedItem.Tag == null)
                {
                    ultraCombo_Assessment1.DropDown();
                }
                else
                {
                    SetAssessmentComboText(ultraCombo_Assessment1);

                    Cursor = Cursors.WaitCursor;
                    UpdateAssessment1ToCurrentSelection();
                    if (!ValidateServerChoice())
                    {
                        ultraCombo_Server1.SelectedIndex = 0;
                        m_server1 = null;
                        UpdateAssessment2ToCurrentSelection();
                    }
                    Cursor = Cursors.Default;
                }
            }            
        }

        private void ultraCombo_Assessment1_SelectionChanged(object sender, EventArgs e)
        {            
            AssessmentCombo1Changed();
        }

        private void SetAssessmentComboText(UltraComboEditor combo)
        {
            ValueListItem item = combo.SelectedItem;
            Policy p = (Policy)item.Tag;

            int index = combo.SelectedIndex;
            string category = string.Empty;
            for(int x = index; x >= 0; x--)
            {
                if(combo.Items[x].Tag == null)
                {
                    category = combo.Items[x].DisplayText;
                    break;
                }
            }

            item.DisplayText = category + " - " + GetDisplayNameForAssessmentInCombo(p);
        }

        private void ultraCombo_Assessment1_AfterCloseUp(object sender, EventArgs e)
        {
            if (ultraCombo_Assessment1.SelectedItem != null)
            {
                if (ultraCombo_Assessment1.SelectedItem.Tag == null)
                {
                    ultraCombo_Assessment1.DropDown();
                }
            }
        }

        private void AssessmentCombo2Changed()
        {
            if (ultraCombo_Assessment2.SelectedItem != null)
            {
                if (ultraCombo_Assessment2.SelectedItem.Tag == null)
                {
                    ultraCombo_Assessment2.DropDown();
                }
                else
                {
                    Cursor = Cursors.WaitCursor;
                    SetAssessmentComboText(ultraCombo_Assessment2);
                    ValueListItem item = ultraCombo_Assessment2.SelectedItem;
                    Policy p = (Policy)item.Tag;
                    m_assessmentId2 = p.AssessmentId;
                    m_PolicyAssessment2 = m_Assessments.Find(m_assessmentId2);
                    if (!ValidateServerChoice())
                    {
                        ultraCombo_Server1.SelectedIndex = 0;
                        m_server1 = null;
                    }
                    else
                    {
                        UpdateAssessment2ToCurrentSelection();                        
                    }
                    ultraLabel_ChooseAssessment.SendToBack();
                    Cursor = Cursors.Default;
                }
            }
        }

        private void ultraCombo_Assessment2_SelectionChanged(object sender, EventArgs e)
        {
            AssessmentCombo2Changed();
        }

        private void ultraCombo_Assessment2_AfterCloseUp(object sender, EventArgs e)
        {
            if (ultraCombo_Assessment2.SelectedItem != null)
            {
                if (ultraCombo_Assessment2.SelectedItem.Tag == null)
                {
                    ultraCombo_Assessment2.DropDown();
                }
            }
        }

        private void ServerComboChanged()
        {
            Cursor = Cursors.WaitCursor;
            if (ultraCombo_Server1.SelectedItem != null)
            {
                m_server1 = (RegisteredServer)ultraCombo_Server1.SelectedItem.Tag;
                if (m_PolicyAssessment1 != null && m_PolicyAssessment2 != null)
                {
                    if (!ValidateServerChoice())
                    {
                        ultraCombo_Server1.SelectedIndex = 0;
                        m_server1 = null;
                    }
                }
                if (m_PolicyAssessment1 != null)
                {
                    UpdateAssessment1ToCurrentSelection();
                }
                if (m_PolicyAssessment2 != null)
                {
                    UpdateAssessment2ToCurrentSelection();
                }
            }
            Cursor = Cursors.Default;
        }

        private void ultraCombo_Server1_SelectionChanged(object sender, EventArgs e)
        {
            if (!m_internalUpdate)
            {
                m_internalUpdate = true;
                ServerComboChanged();
                m_internalUpdate = false;
            }
        }       

        private void _label_Bar_SizeChanged(object sender, EventArgs e)
        {
            Label l = (Label)sender;
            if (l.Width > 0)
            {
                Image bmPhoto = new Bitmap(l.Width + 4, l.Height + 4, PixelFormat.Format24bppRgb);
                Graphics g = Graphics.FromImage(bmPhoto);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.DrawImage(l.Image, 0, 0, l.Width + 4, l.Height + 4);
                l.Image.Dispose();
                l.Image = bmPhoto;
                g.Dispose();
            }
        }

        private void Form_AssessmentComparison_SizeChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            LoadAssessmentSummary1();
            LoadAssessmentSummary2();

            Cursor = Cursors.Default;

        }

        private void _grid_Servers_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            e.Layout.Override.AllowColMoving = AllowColMoving.NotAllowed;
            UltraGridBand band = e.Layout.Bands[0];
            EditorWithText textEditor = new EditorWithText();

            band.Columns[colDifferencesFound].Header.Caption = "";
            band.Columns[colDifferencesFound].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            band.Columns[colDifferencesFound].ValueList = e.Layout.ValueLists[valueListDifference];
            band.Columns[colDifferencesFound].Editor = textEditor;
            band.Columns[colDifferencesFound].Width = 20;
            band.Columns[colDifferencesFound].Hidden = true;

            band.Columns[colServer].Width = 130;
            band.Columns[colServer].AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.VisibleRows;
            band.Columns[colServer].CellAppearance.BackColor = Color.White;
            band.Columns[colServer].CellAppearance.BackColor2 = Color.White;

            band.Columns[colDateTime1].Width = 126;
            band.Columns[colDateTime1].Header.Caption = "Audit Data Collected";
            band.Columns[colDateTime1].AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.VisibleRows;

            band.Columns[colHigh1].Header.Caption = string.Empty;
            band.Columns[colHigh1].Header.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.HighRisk_16;
            band.Columns[colHigh1].Header.Appearance.ImageHAlign = HAlign.Center;
            band.Columns[colHigh1].CellAppearance.TextHAlign = HAlign.Right;
            band.Columns[colHigh1].MinWidth = 20;
            band.Columns[colHigh1].Width = 20;

            band.Columns[colMedium1].Header.Caption = string.Empty;
            band.Columns[colMedium1].Header.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.MediumRisk_16;
            band.Columns[colMedium1].Header.Appearance.ImageHAlign = HAlign.Center;
            band.Columns[colMedium1].CellAppearance.TextHAlign = HAlign.Right;
            band.Columns[colMedium1].MinWidth = 20;
            band.Columns[colMedium1].Width = 20;

            band.Columns[colLow1].Header.Caption = string.Empty;
            band.Columns[colLow1].Header.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.LowRisk_16;
            band.Columns[colLow1].Header.Appearance.ImageHAlign = HAlign.Center;
            band.Columns[colLow1].CellAppearance.TextHAlign = HAlign.Right;
            band.Columns[colLow1].MinWidth = 20;
            band.Columns[colLow1].Width = 20;

            band.Columns[colDateTime2].Width = 126;
            band.Columns[colDateTime2].Header.Caption = "Audit Data Collected";
            band.Columns[colDateTime2].AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.VisibleRows;

            band.Columns[colHigh2].Header.Caption = string.Empty;
            band.Columns[colHigh2].Header.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.HighRisk_16;
            band.Columns[colHigh2].Header.Appearance.ImageHAlign = HAlign.Center;
            band.Columns[colHigh2].CellAppearance.TextHAlign = HAlign.Right;
            band.Columns[colHigh2].MinWidth = 20;
            band.Columns[colHigh2].Width = 20;

            band.Columns[colMedium2].Header.Caption = string.Empty;
            band.Columns[colMedium2].Header.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.MediumRisk_16;
            band.Columns[colMedium2].Header.Appearance.ImageHAlign = HAlign.Center;
            band.Columns[colMedium2].CellAppearance.TextHAlign = HAlign.Right;
            band.Columns[colMedium2].MinWidth = 20;
            band.Columns[colMedium2].Width = 20;

            band.Columns[colLow2].Header.Caption = string.Empty;
            band.Columns[colLow2].Header.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.LowRisk_16;
            band.Columns[colLow2].Header.Appearance.ImageHAlign = HAlign.Center;
            band.Columns[colLow2].CellAppearance.TextHAlign = HAlign.Right;
            band.Columns[colLow2].MinWidth = 20;
            band.Columns[colLow2].Width = 20;
        }
  
        private void _grid_ComparisonSummary_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            UpdateComparisonDetails((int)e.Cell.Row.Cells[colMetricId].Value);
            if(e.Cell.Column.Key == colFindingDiff)
            {
                _ultraTabControl_ComparisonDetails.SelectedTab = _ultraTabControl_ComparisonDetails.Tabs[tabKey_Findings];
            }
            if (e.Cell.Column.Key == colExplanationDiff)
            {
                _ultraTabControl_ComparisonDetails.SelectedTab = _ultraTabControl_ComparisonDetails.Tabs[tabKey_Notes];
            }
            if (e.Cell.Column.Key == colReportCardDisplayDiff)
            {
                _ultraTabControl_ComparisonDetails.SelectedTab = _ultraTabControl_ComparisonDetails.Tabs[tabKey_DisplaySettings];
            }
            if (e.Cell.Column.Key == colSecurityCheckDiff)
            {
                _ultraTabControl_ComparisonDetails.SelectedTab = _ultraTabControl_ComparisonDetails.Tabs[tabKey_Criteria];
            }
        }

        private void _grid_ComparisonSummary_AfterRowActivate(object sender, EventArgs e)
        {
            UltraGrid grid = (UltraGrid)sender;

            if (grid.ActiveRow.IsDataRow)
            {
                if (grid.ActiveRow.Cells[colMetricId].Value != System.DBNull.Value)
                {
                    UpdateComparisonDetails((int) grid.ActiveRow.Cells[colMetricId].Value);
                }
            }
            else if (grid.Selected.Rows.Count > 0)
            {
                if (grid.Selected.Rows[0].Cells[colMetricId].Value != System.DBNull.Value)
                {
                    UpdateComparisonDetails((int) grid.Selected.Rows[0].Cells[colMetricId].Value);
                }
            }
        }

        private Control getFocused(Control.ControlCollection controls)
        {
            foreach (Control c in controls)
            {
                if (c.Focused)
                {
                    return c;
                }
                else if (c.ContainsFocus)
                {
                    return getFocused(c.Controls);
                }
            }

            return null;
        }

        private void _splitContainer_MouseDown(object sender, MouseEventArgs e)
        {
            m_focused = getFocused(this.Controls);
        }

        private void _splitContainer_MouseUp(object sender, MouseEventArgs e)
        {
            if (m_focused != null)
            {
                m_focused.Focus();
                m_focused = null;
            }
        }

        #endregion



        private void HighLightComparisonDetailsDifferences(bool isAssessmentEnabled1, bool isAssessmentEnabled2)
        {
            Color backColor = Color.White;
            Color highlightColorMissingForDetails = Color.White;
            Color color = backColor;
            if (!isAssessmentEnabled1 || !isAssessmentEnabled2)
            {
                color = highlightColorMissingForDetails;
            }
            else if(_textBox_RiskLevel1.Text != _textBox_RiskLevel2.Text)
            {
                color = highlightColorDifferent;
            }
            _textBox_RiskLevel1.BackColor = 
                _textBox_RiskLevel2.BackColor = color;

            color = backColor;
            if (!isAssessmentEnabled1 || !isAssessmentEnabled2)
            {
                color = highlightColorMissingForDetails;
            }
            else if (_textBox_ReportText1.Text != _textBox_ReportText2.Text)
            {
                color = highlightColorDifferent;
            }
            _textBox_ReportText1.BackColor =             
                _textBox_ReportText2.BackColor = color;

            color = backColor;
            if (!isAssessmentEnabled1 || !isAssessmentEnabled2)
            {
                color = highlightColorMissingForDetails;
            }
            else if (_textBox_XRef1.Text != _textBox_XRef2.Text)
            {
                color = highlightColorDifferent;
            }
            _textBox_XRef1.BackColor = 
                _textBox_XRef2.BackColor = color;

            color = backColor;
            if (!isAssessmentEnabled1 || !isAssessmentEnabled2)
            {
                color = highlightColorMissingForDetails;
            }
            else if (_textBox_SCCriteria1.Text != _textBox_SCCriteria2.Text)
            {
                color = highlightColorDifferent;
            }
            _textBox_SCCriteria1.BackColor = 
                _textBox_SCCriteria2.BackColor = color;

            if (isAssessmentEnabled1 && isAssessmentEnabled2)
            {
                HighLightFindingDiffs(_richTextBox_Details1, _richTextBox_Details2);
            }
           
        }

        private void SetComparisonSummaryFindingImage(int metricId, int assessmentNum)
        {
            Image image = null;
            string expression = string.Format("metricId = {0}", metricId);
            DataRow[] foundRows = ReportCardTable.Select(expression);
            if (foundRows.Length > 0)
            {
                int sevCode = -1;
                if (assessmentNum == 1)
                {
                    sevCode = (int)foundRows[0][colSeverityCode1];
                }
                else
                {
                    sevCode = foundRows[0][colSeverityCode2] == DBNull.Value ? -1 : (int)foundRows[0][colSeverityCode2];
                }
                switch ((Utility.Policy.SeverityExplained)sevCode)
                {
                    case Utility.Policy.SeverityExplained.High:
                        image = global::Idera.SQLsecure.UI.Console.Properties.Resources.HighRisk_16;
                        break;
                    case Utility.Policy.SeverityExplained.Medium:
                        image = global::Idera.SQLsecure.UI.Console.Properties.Resources.MediumRisk_16;
                        break;
                    case Utility.Policy.SeverityExplained.Low:
                        image = global::Idera.SQLsecure.UI.Console.Properties.Resources.LowRisk_16;
                        break;
                    case Utility.Policy.SeverityExplained.HighExplained:
                        image = global::Idera.SQLsecure.UI.Console.Properties.Resources.HighRiskExplained_16;
                        break;
                    case Utility.Policy.SeverityExplained.MediumExplained:
                        image = global::Idera.SQLsecure.UI.Console.Properties.Resources.MediumRiskExplained_16;
                        break;
                    case Utility.Policy.SeverityExplained.LowExplained:
                        image = global::Idera.SQLsecure.UI.Console.Properties.Resources.LowRiskExplained_16;
                        break;
                    case 0:
                        image = global::Idera.SQLsecure.UI.Console.Properties.Resources.check_16;
                        break;
                }
            }
            if (assessmentNum == 1)
            {
                _ultraGroupBox_Finding1.HeaderAppearance.Image = image;
                _ultraGroupBox_RCDS1.HeaderAppearance.Image = image;
                _ultraGroupBox_SCS1.HeaderAppearance.Image = image;
                _ultraGroupBox_EN1.HeaderAppearance.Image = image;
            }
            else
            {
                _ultraGroupBox_Finding2.HeaderAppearance.Image = image;
                _ultraGroupBox_RCDS2.HeaderAppearance.Image = image;
                _ultraGroupBox_SCS2.HeaderAppearance.Image = image;
                _ultraGroupBox_EN2.HeaderAppearance.Image = image;
            }
        }



        private void ShowNoNotesForCurrent(bool current, int assessment)
        {
            if (assessment == 1)
            {
                if (current)
                {
                    label_ExplanationSCNotEnabled1.Text = NoExplanationNotesForCurrent;
                }
                label_ExplanationSCNotEnabled1.Visible = current;
                _headerStrip_Notes1.Visible = !current;
                _grid_ExplanationNotes1.Visible = !current;
            }
            if (assessment == 2)
            {
                if (current)
                {
                    label_ExplanationSCNotEnabled2.Text = NoExplanationNotesForCurrent;
                }
                label_ExplanationSCNotEnabled2.Visible = current;
                _headerStrip_Notes2.Visible = !current;
                _grid_ExplanationNotes2.Visible = !current;
            }
            
        }

        private void ShowSecurityCheckEnabled(bool scEnabled, int metricId, int assessment)
        {
            if (assessment == 1)
            {
                _richTextBox_Details1.Visible = scEnabled;
                _headerStrip_Notes1.Visible = scEnabled;
                _grid_ExplanationNotes1.Visible = scEnabled;
                _panel_RCDS1.Visible = scEnabled;
                _panel_SCS1.Visible = scEnabled;

                if(!scEnabled)
                {
                    if(IsSecurityCheckEnabledInAssessment(m_PolicyAssessment1, metricId))
                    {
                        _label_ReportDisplaySettingsSCNotEnabled1.Text = 
                        label_SCSettingsNotEnabled1.Text = 
                        label_FindingsSCNotEnabled1.Text =
                        label_ExplanationSCNotEnabled1.Text = ComparisonDetailsSCNoAuditData;
                    }
                    else
                    {
                        _label_ReportDisplaySettingsSCNotEnabled1.Text =
                        label_SCSettingsNotEnabled1.Text =
                        label_FindingsSCNotEnabled1.Text =
                        label_ExplanationSCNotEnabled1.Text = ComparisonDetailsSCNotEnabled;
                    }
                }
                _label_ReportDisplaySettingsSCNotEnabled1.Visible = !scEnabled;
                label_SCSettingsNotEnabled1.Visible = !scEnabled;
                label_FindingsSCNotEnabled1.Visible = !scEnabled;
                label_ExplanationSCNotEnabled1.Visible = !scEnabled;
            }
            else if (assessment == 2)
            {
                _richTextBox_Details2.Visible = scEnabled;
                _headerStrip_Notes2.Visible = scEnabled;
                _grid_ExplanationNotes2.Visible = scEnabled;
                _panel_RCDS2.Visible = scEnabled;
                _panel_SCS2.Visible = scEnabled;
                if (!scEnabled)
                {
                    if (IsSecurityCheckEnabledInAssessment(m_PolicyAssessment2, metricId))
                    {
                        _label_ReportDisplaySettingsSCNotEnabled2.Text =
                        label_SCSettingsNotEnabled2.Text =
                        label_FindingsSCNotEnabled2.Text =
                        label_ExplanationSCNotEnabled2.Text = ComparisonDetailsSCNoAuditData;
                    }
                    else
                    {
                        _label_ReportDisplaySettingsSCNotEnabled2.Text =
                        label_SCSettingsNotEnabled2.Text =
                        label_FindingsSCNotEnabled2.Text =
                        label_ExplanationSCNotEnabled2.Text = ComparisonDetailsSCNotEnabled;
                    }
                }
                _label_ReportDisplaySettingsSCNotEnabled2.Visible = !scEnabled;
                label_SCSettingsNotEnabled2.Visible = !scEnabled;
                label_FindingsSCNotEnabled2.Visible = !scEnabled;
                label_ExplanationSCNotEnabled2.Visible = !scEnabled;
            }
        }

        private void UpdateComparisonDetails(int metricId)
        {
            string expression = string.Format("metricId = {0}", metricId);
            bool isAssessmentEnabled1 = true;
            bool isAssessmentEnabled2 = true;
            DataRow[] foundRows = m_ComparisonTable.Select(expression);

            _toolStripLabel_Notes1.Text = GetAssessmentStateText(m_PolicyAssessment1);
            _toolStripLabel_Notes2.Text = GetAssessmentStateText(m_PolicyAssessment2);

            if(foundRows.Length > 0)
            {
                _label_CompareSummaryDetails.Text = string.Format(CompareSummaryDetails, foundRows[0][colMetricName]);
                int newMetricId = -1;
                if (foundRows[0][colSeverityCode1] != System.DBNull.Value)
                {
                    newMetricId = metricId;                    
                }
                SetComparisonSummaryFindingImage(newMetricId, 1);
                newMetricId = -1;
                if (foundRows[0][colSeverityCode2] != System.DBNull.Value)
                {
                    newMetricId = metricId;
                }
                SetComparisonSummaryFindingImage(newMetricId, 2);

                if (foundRows[0][colMetricSeverity1] == System.DBNull.Value)
                {
                    isAssessmentEnabled1 = false;
                    ShowSecurityCheckEnabled(false, metricId, 1);
                }
                else
                {
                    ShowSecurityCheckEnabled(true, metricId, 1);

                    // Report Display Tab
                    _textBox_RiskLevel1.Text = (string)foundRows[0][colMetricSeverity1];
                    _textBox_ReportText1.Text = (string)foundRows[0][colReportText1];
                    _textBox_XRef1.Text = (string)foundRows[0][colXRef1];

                    // Security Checks Tab
                    _textBox_SCCriteria1.Text = (string)foundRows[0][colThresholdValue1];

                    // Findings Tab
                    _richTextBox_Details1.Rtf = _reportCard1.GetRTFDetails(metricId);


                    // Explanation Notes
                    ShowNoNotesForCurrent(m_PolicyAssessment1.IsCurrentAssessment, 1);
                }

                if (foundRows[0][colMetricSeverity2] == System.DBNull.Value)
                {
                    isAssessmentEnabled2 = false;
                    ShowSecurityCheckEnabled(false, metricId, 2);
                }
                else
                {
                    ShowSecurityCheckEnabled(true, metricId, 2);
                    // Report Display Tab
                    _textBox_RiskLevel2.Text = (string)foundRows[0][colMetricSeverity2];
                    _textBox_ReportText2.Text = (string)foundRows[0][colReportText2];
                    _textBox_XRef2.Text = (string)foundRows[0][colXRef2];

                    // Security Checks Tab
                    _textBox_SCCriteria2.Text = (string)foundRows[0][colThresholdValue2];

                    // Findings Tab
                    _richTextBox_Details2.Rtf = _reportCard2.GetRTFDetails(metricId);

                    // Explanation Notes
                    ShowNoNotesForCurrent(m_PolicyAssessment2.IsCurrentAssessment, 2);

                }


                m_ExplanationTable1 = createDataSourceExplanations();
                m_ExplanationTable2 = createDataSourceExplanations();
                string server = string.Empty;
                bool isExplained1 = false;
                bool isExplained2 = false;
                string notes1 = string.Empty;
                string notes2 = string.Empty;
                int snapshotId1 = -1;
                int snapshotId2 = -1;
                foreach (DataRow row in foundRows)
                {
                    isExplained1 = false;
                    isExplained2 = false;
                    notes1 = string.Empty;
                    notes2 = string.Empty;
                    DataRow newRow1 = m_ExplanationTable1.NewRow();
                    DataRow newRow2 = m_ExplanationTable2.NewRow();
                    if (row[colConnection] != System.DBNull.Value)
                    {
                        server = (string) row[colConnection];
                    }
                    if (row[colIsExplained1] != System.DBNull.Value)
                    {
                        isExplained1 = (bool)row[colIsExplained1];
                    }
                    if (row[colIsExplained2] != System.DBNull.Value)
                    {
                        isExplained2 = (bool)row[colIsExplained2];
                    }
                    if (row[colExplanationNotes1] != System.DBNull.Value)
                    {
                        notes1 = (string)row[colExplanationNotes1];
                    }
                    if (row[colExplanationNotes2] != System.DBNull.Value)
                    {
                        notes2 = (string)row[colExplanationNotes2];
                    }
                    if(row[colSnapshotId1] != System.DBNull.Value)
                    {
                        snapshotId1 = (int) row[colSnapshotId1];
                    }
                    if (row[colSnapshotId2] != System.DBNull.Value)
                    {
                        snapshotId2 = (int)row[colSnapshotId2];
                    }

                    newRow1[colServerName] = server;
                    newRow1[colMetricId] = metricId;
                    newRow1[colSnapshotId] = snapshotId1;
                    newRow1[colIsExplained] = isExplained1;
                    newRow1[colNotes] = notes1;
                    newRow2[colServerName] = server;
                    newRow2[colMetricId] = metricId;
                    newRow2[colSnapshotId] = snapshotId2;
                    newRow2[colIsExplained] = isExplained2;
                    newRow2[colNotes] = notes2;
                    m_ExplanationTable1.Rows.Add(newRow1);
                    m_ExplanationTable2.Rows.Add(newRow2);
                }

                _grid_ExplanationNotes1.SetDataBinding(m_ExplanationTable1.DefaultView, null);
                _grid_ExplanationNotes2.SetDataBinding(m_ExplanationTable2.DefaultView, null);

                EnableCopyButton(1);
                EnableCopyButton(2);

                // Update Image on Tabs
                foundRows = m_ComparisonSummaryTable.Select(expression);
                if (foundRows.Length > 0)
                {
                    _ultraTabControl_ComparisonDetails.Tabs[tabKey_Findings].Appearance.Image =
                        (CompareResults) foundRows[0][colFindingDiff] == CompareResults.COMPARE_MATCH
                            ? global::Idera.SQLsecure.UI.Console.Properties.Resources._32_CompareMatch
                            : global::Idera.SQLsecure.UI.Console.Properties.Resources._32_CompareDifferent;

                    _ultraTabControl_ComparisonDetails.Tabs[tabKey_DisplaySettings].Appearance.Image =
                        (CompareResults)foundRows[0][colReportCardDisplayDiff] == CompareResults.COMPARE_MATCH
                            ? global::Idera.SQLsecure.UI.Console.Properties.Resources._32_CompareMatch
                            : global::Idera.SQLsecure.UI.Console.Properties.Resources._32_CompareDifferent;

                    _ultraTabControl_ComparisonDetails.Tabs[tabKey_Criteria].Appearance.Image =
                        (CompareResults)foundRows[0][colSecurityCheckDiff] == CompareResults.COMPARE_MATCH
                            ? global::Idera.SQLsecure.UI.Console.Properties.Resources._32_CompareMatch
                            : global::Idera.SQLsecure.UI.Console.Properties.Resources._32_CompareDifferent;

                    _ultraTabControl_ComparisonDetails.Tabs[tabKey_Notes].Appearance.Image =
                        (CompareResults)foundRows[0][colExplanationDiff] == CompareResults.COMPARE_MATCH
                            ? global::Idera.SQLsecure.UI.Console.Properties.Resources._32_CompareMatch
                            : global::Idera.SQLsecure.UI.Console.Properties.Resources._32_CompareDifferent;
                }

                // Position Label above Tab to align with tab right
                Point sc = this.PointToClient(_ultraGroupBox_Finding1.PointToScreen(_ultraGroupBox_Finding1.Location));
                sc.X = sc.X < 0 ? 20 : sc.X;

                _label_CompareSummaryDetails.Padding = new Size(sc.X, 0);


                HighLightComparisonDetailsDifferences(isAssessmentEnabled1, isAssessmentEnabled2);

            }


        }

      
        #region ReportCard 2

        // Assessment details columns
        private const string colMetricDescription = @"metricdescription";
        private const string colReportKey = @"metricreportkey";
        private const string colReportText = @"metricreporttext";

        // Scorecard columns
        private const string colSummary = @"summary";
        private const string colResult = @"result";
        private const string colResultRTF = @"resultrtf";
        private const string colSeverityCode = @"severitycode";
        private const string colSeverityCodeExplained = @"severitycodeexplained";
        private const string colSummary1 = @"summary1";
        private const string colResult1 = @"result1";
        private const string colResultRTF1 = @"resultrtf1";
        private const string colNotes1 = @"notes1";
        private const string colSummary2 = @"summary2";
        private const string colResult2 = @"result2";
        private const string colResultRTF2 = @"resultrtf2";
        private const string colNotes2 = @"notes2";
        
        private DataTable ReportCardTable = null;

        private DataTable createDataSourceReportCard()
        {
            // Create Report Card default datasources
            DataTable dt = new DataTable();
            dt.Columns.Add(colSummary1, typeof(string));
            dt.Columns.Add(colSeverityCode1, typeof(int));
            dt.Columns.Add(colResult1, typeof(string));
            dt.Columns.Add(colResultRTF1, typeof(string));
            dt.Columns.Add(colIsExplained1, typeof(bool));
            dt.Columns.Add(colNotes1, typeof(string));
            dt.Columns.Add(colDifferencesFound, typeof(int));
            dt.Columns.Add(colMetricId, typeof(int));
            dt.Columns.Add(colMetricName, typeof(string));
            dt.Columns.Add(colMetricDescription, typeof(string));
            dt.Columns.Add(colReportKey, typeof(string));
            dt.Columns.Add(colReportText, typeof(string));
            dt.Columns.Add(colMetricType, typeof(string));
            dt.Columns.Add(colSeverityCode2, typeof(int));
            dt.Columns.Add(colSummary2, typeof(string));
            dt.Columns.Add(colResult2, typeof(string));
            dt.Columns.Add(colResultRTF2, typeof(string));
            dt.Columns.Add(colIsExplained2, typeof(bool));
            dt.Columns.Add(colNotes2, typeof(string));
            
            return dt;
        }

        private void LoadReportCardDataSource()
        {
            _grid_ReportCard.SuspendLayout();

            try
            {
                ReportCardTable = createDataSourceReportCard();
                ReportCardTable.DefaultView.Sort =
                    string.Format("{0} DESC, {1}", colDifferencesFound, colMetricId);

                DataTable reportCardTable1 = _reportCard1.ReportCardTable;
                reportCardTable1.DefaultView.Sort = string.Format("{0}, {1}", colMetricType, colMetricId);
                DataTable reportCardTable2 = _reportCard2.ReportCardTable;
                string expression = string.Empty;
                DataRow[] foundRows = null;
                CompareResults diffResults = CompareResults.COMPARE_MATCH;
                int nTotalDiffs = 0;
                foreach (DataRowView drv in reportCardTable1.DefaultView)
                {
                    DataRow newRow = ReportCardTable.NewRow();

                    newRow[colMetricType] = drv[colMetricType];
                    newRow[colMetricId] = drv[colMetricId];
                    newRow[colMetricName] = drv[colMetricName];
                    newRow[colSummary1] = drv[colSummary];
                    newRow[colSeverityCode1] = drv[colSeverityCode];
                    newRow[colResult1] = drv[colResult];
                    newRow[colResultRTF1] = drv[colResultRTF];


                    expression = string.Format("{0} = {1}", colMetricId, newRow[colMetricId]);
                    foundRows = reportCardTable2.Select(expression);
                    if (foundRows.Length > 0 && foundRows[0][colSummary] != DBNull.Value)
                    {
                        newRow[colSummary2] = foundRows[0][colSummary];
                        newRow[colSeverityCode2] = foundRows[0][colSeverityCode];
                        newRow[colResult2] = foundRows[0][colResult];
                        newRow[colResultRTF2] = foundRows[0][colResultRTF];

                        //if((string)newRow[colSummary1] != (string)newRow[colSummary2])
                        if (metricDifferenceDict.TryGetValue((int) newRow[colMetricId], out diffResults))
                        {
                            newRow[colDifferencesFound] = (int) diffResults;
                            if(diffResults != CompareResults.COMPARE_MATCH)
                            {
                                nTotalDiffs++;
                            }
                        }
                        else
                        {
                            newRow[colDifferencesFound] = (int) CompareResults.COMPARE_MATCH;
                        }
                    }
                    else
                    {
                        if (IsSecurityCheckEnabledInAssessment(m_PolicyAssessment2, (int)newRow[colMetricId]))
                        {
                            newRow[colSummary2] = "No Audit Data";
                        }
                        else
                        {
                            newRow[colSummary2] = "Not Enabled";
                        }
                        newRow[colDifferencesFound] = (int) CompareResults.COMPARE_DIFFERENT;
                        nTotalDiffs++;
                    }

                   // If this metric is included in display, then add it
                    if (!m_showDifferencesOnly || (int)newRow[colDifferencesFound] != (int)CompareResults.COMPARE_MATCH)
                    {
                        ReportCardTable.Rows.Add(newRow);
                    }
                }


                foreach (DataRowView drv in reportCardTable2.DefaultView)
                {
                    expression = string.Format("{0} = {1}", colMetricId, drv[colMetricId]);

                    foundRows = ReportCardTable.Select(expression);

                    if (foundRows.Length == 0)
                    {
                        DataRow newRow = ReportCardTable.NewRow();
                        newRow[colMetricType] = drv[colMetricType];
                        newRow[colMetricId] = drv[colMetricId];
                        newRow[colMetricName] = drv[colMetricName];
                        newRow[colSummary2] = drv[colSummary];
                        newRow[colSeverityCode2] = drv[colSeverityCode];
                        newRow[colResult2] = drv[colResult];
                        newRow[colResultRTF2] = drv[colResultRTF];

                        expression = string.Format("{0} = {1}", colMetricId, newRow[colMetricId]);
                        foundRows = reportCardTable1.Select(expression);
                        if (foundRows.Length > 0 && foundRows[0][colSummary] != DBNull.Value)
                        {
                            newRow[colSummary1] = foundRows[0][colSummary];
                            newRow[colSeverityCode1] = foundRows[0][colSeverityCode];
                            newRow[colResult1] = foundRows[0][colResult];
                            newRow[colResultRTF1] = foundRows[0][colResultRTF];

                            if (metricDifferenceDict.TryGetValue((int) newRow[colMetricId], out diffResults))
                            {
                                newRow[colDifferencesFound] = (int) diffResults;
                                if (diffResults != CompareResults.COMPARE_MATCH)
                                {
                                    nTotalDiffs++;
                                }
                            }
                            else
                            {
                                newRow[colDifferencesFound] = (int) CompareResults.COMPARE_MATCH;
                            }
                        }
                        else
                        {
                            if (IsSecurityCheckEnabledInAssessment(m_PolicyAssessment1, (int)newRow[colMetricId]))
                            {
                                newRow[colSummary1] = "No Audit Data";
                            }
                            else
                            {
                                newRow[colSummary1] = "Not Enabled";
                            }
                            newRow[colDifferencesFound] = (int) CompareResults.COMPARE_DIFFERENT;
                            nTotalDiffs++;
                        }

                        // If this metric is included in display, then add it
                        if (!m_showDifferencesOnly || (int)newRow[colDifferencesFound] != (int)CompareResults.COMPARE_MATCH)
                        {
                            ReportCardTable.Rows.Add(newRow);
                        }            
                    }
                }

                // Save the user configuration of the grid to restore it after setting the datasource again
                Utility.GridSettings gridSettings = GridSettings.GetSettings(_grid_ReportCard);

                DataView dataView = _grid_ReportCard.DataSource as DataView;
                if (dataView != null)
                {
                    dataView.Dispose();
                }

                bool initializing = _grid_ReportCard.DataSource == null;

                _grid_ReportCard.SetDataBinding(ReportCardTable.DefaultView, null);

                // Restore Grid Settings
                if (!initializing)
                {
                    GridSettings.ApplySettingsToGrid(gridSettings, _grid_ReportCard);
                }

                _label_ReportCard.Text = string.Format(ReportCardHeader,
                                              nTotalDiffs == 0
                                                  ? "No"
                                                  : nTotalDiffs.ToString(),
                                              nTotalDiffs == 1
                                                  ? DifferenceSingular
                                                  : DifferencePlural);
 
                int selectedMetric = 1;
                if (_grid_ReportCard.Rows.Count > 0 && _grid_ReportCard.Selected.Rows.Count > 0)
                {
                    selectedMetric = (int) _grid_ReportCard.Selected.Rows[0].Cells[colMetricId].Value;
                }               

                if(_grid_ReportCard.Rows.Count == 0)
                {
                    ultraLabel_ReportCardNoDiffsFound.Visible = true;
                    _grid_ReportCard.Visible = false;
                }
                else
                {
                    ultraLabel_ReportCardNoDiffsFound.Visible = false;
                    _grid_ReportCard.Visible = true;
                }

            }
            catch(Exception ex)
            {
                logX.loggerX.Error(@"Error - Unable to retrieve report card compare", ex);
                MsgBox.ShowError(string.Format(ErrorMsgs.CantGetPolicyInfoMsg, "Policy Report Card Compare"),
                                 ErrorMsgs.ErrorProcessPolicyInfo,
                                 ex);                
            }
            finally
            {
                _grid_ReportCard.ResumeLayout();
            }
        }

        #endregion

        private void _grid_ReportCard_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            e.Layout.Appearance.BackColor = Color.Orange;
            e.Layout.Override.GroupByRowExpansionStyle = GroupByRowExpansionStyle.Disabled;
            e.Layout.Override.GroupByRowInitialExpansionState = GroupByRowInitialExpansionState.Expanded;
            e.Layout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.ColumnChooserEnabled = DefaultableBoolean.False;

            e.Layout.Override.RowAppearance.BackColor = Color.LightBlue;
            e.Layout.Override.RowAppearance.BackColor2 = Color.Transparent;
            e.Layout.Override.CellAppearance.BackColor = Color.Transparent; 
            e.Layout.Override.CellAppearance.BackColor2 = Color.Transparent;

            e.Layout.Override.AllowColMoving = AllowColMoving.NotAllowed;

            UltraGridBand band = e.Layout.Bands[0];

            
            EditorWithText textEditor = new EditorWithText();

            band.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            band.Columns[colDifferencesFound].Header.Caption = "";
            band.Columns[colDifferencesFound].ColumnChooserCaption = "Compare Results";
            band.Columns[colDifferencesFound].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            band.Columns[colDifferencesFound].ValueList = e.Layout.ValueLists[valueListDifference];
            band.Columns[colDifferencesFound].Editor = textEditor;
            band.Columns[colDifferencesFound].Width = 30;
            band.Columns[colDifferencesFound].MaxWidth = 30;
            band.Columns[colDifferencesFound].CellAppearance.BackColor = Color.White;
            band.Columns[colDifferencesFound].CellAppearance.BackColor2 = Color.White;

            band.Columns[colMetricId].Hidden = true;
            band.Columns[colMetricId].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            band.Columns[colMetricName].Header.Caption = "Security Check";
            band.Columns[colMetricName].Width = 120;
            band.Columns[colMetricName].CellAppearance.BackColor = Color.White;
            band.Columns[colMetricName].CellAppearance.BackColor2 = Color.White;

            band.Columns[colReportKey].Header.Caption = "Cross Ref";
            band.Columns[colReportKey].Hidden = true;

            band.Columns[colReportText].Header.Caption = "Report Text";
            band.Columns[colReportText].Hidden = true;

            band.Columns[colMetricDescription].Header.Caption = "Description";
            band.Columns[colMetricDescription].Hidden = true;

            band.Columns[colMetricType].Header.Caption = "Category";
            band.Columns[colMetricType].Hidden = true;

            band.Columns[colSeverityCode1].Header.Caption = "Risk";
            band.Columns[colSeverityCode1].ColumnChooserCaption = "Risk 1";
            band.Columns[colSeverityCode1].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            band.Columns[colSeverityCode1].ValueList = e.Layout.ValueLists[valueListSeverity];
            band.Columns[colSeverityCode1].Width = 40;
            band.Columns[colSeverityCode1].MinWidth = 40;
            band.Columns[colSeverityCode1].MaxWidth = 40;
            band.Columns[colSeverityCode1].LockedWidth = true;
            band.Columns[colSeverityCode1].Editor = textEditor;
            band.Columns[colSeverityCode1].CellAppearance.BackColor = Color.Transparent;
            band.Columns[colSeverityCode1].CellAppearance.BackColor2 = Color.Transparent;

            band.Columns[colSummary1].Header.Caption = "Findings";
            band.Columns[colSummary1].ColumnChooserCaption = "Findings 1";
            band.Columns[colSummary1].Width = 80;

            band.Columns[colResult1].Header.Caption = "Detail Findings 1";
            band.Columns[colResult1].Hidden = true;

            band.Columns[colResultRTF1].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            band.Columns[colResultRTF1].Hidden = true;

            band.Columns[colIsExplained1].Header.Caption = "Explained 1";
            band.Columns[colIsExplained1].Hidden = true;

            band.Columns[colNotes1].Header.Caption = "Notes 1";
            band.Columns[colNotes1].Hidden = true;
          
            band.Columns[colSeverityCode2].Header.Caption = "Risk";
            band.Columns[colSeverityCode2].ColumnChooserCaption = "Risk 2";
            band.Columns[colSeverityCode2].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            band.Columns[colSeverityCode2].ValueList = e.Layout.ValueLists[valueListSeverity];
            band.Columns[colSeverityCode2].Editor = textEditor;
            band.Columns[colSeverityCode2].Width = 40;
            band.Columns[colSeverityCode2].MinWidth = 40;
            band.Columns[colSeverityCode2].MaxWidth = 40;
            band.Columns[colSeverityCode2].LockedWidth = true;


            band.Columns[colSummary2].Header.Caption = "Findings";
            band.Columns[colSummary2].ColumnChooserCaption = "Findings 2";
            band.Columns[colSummary2].Width = 80;

            band.Columns[colResult2].Header.Caption = "Detail Findings 2";
            band.Columns[colResult2].Hidden = true;

            band.Columns[colResultRTF2].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            band.Columns[colResultRTF2].Hidden = true;

            band.Columns[colIsExplained2].Header.Caption = "Explained 2";
            band.Columns[colIsExplained2].Hidden = true;

            band.Columns[colNotes2].Header.Caption = "Notes 2";
            band.Columns[colNotes2].Hidden = true;
            


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
                    if (cell.Row.IsDataRow)
                    {
                        grid.Selected.Rows.Clear();
                        cell.Row.Selected = true;
                        grid.ActiveRow = cell.Row;
                    }
                }
                else
                {
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

        private void _grid_ReportCard_AfterRowActivate(object sender, EventArgs e)
        {
            UltraGrid grid = (UltraGrid)sender;

            if (grid.ActiveRow.IsDataRow)
            {
                if (grid.ActiveRow.Cells[colResultRTF1].Value != System.DBNull.Value)
                {
                    _richTextBox_Details_1.Rtf = (string)grid.ActiveRow.Cells[colResultRTF1].Value;
                }
                else
                {
                    _richTextBox_Details_1.Rtf = string.Empty;
                }
                if (grid.ActiveRow.Cells[colResultRTF2].Value != System.DBNull.Value)
                {
                    _richTextBox_Details_2.Rtf = (string)grid.ActiveRow.Cells[colResultRTF2].Value;
                }
                else
                {
                    _richTextBox_Details_2.Rtf = string.Empty;
                }
            }
            else if (grid.Selected.Rows.Count > 0)
            {
                if (grid.Selected.Rows[0].Cells[colResultRTF1].Value != System.DBNull.Value)
                {
                    _richTextBox_Details_1.Rtf = (string)grid.Selected.Rows[0].Cells[colResultRTF1].Value;
                }
                else
                {
                    _richTextBox_Details_1.Rtf = string.Empty;
                }
                if (grid.Selected.Rows[0].Cells[colResultRTF2].Value != System.DBNull.Value)
                {
                    _richTextBox_Details_2.Rtf = (string)grid.Selected.Rows[0].Cells[colResultRTF2].Value;
                }
                else
                {
                    _richTextBox_Details_2.Rtf = string.Empty;
                }
            }
            HighLightFindingDiffs(_richTextBox_Details_1, _richTextBox_Details_2);
        }

        private void _toolStripButton_ColumnChooser_Click(object sender, EventArgs e)
        {
            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            Cursor = Cursors.WaitCursor;

            showGridColumnChooser(grid);

            Cursor = Cursors.Default;

        }

        private void _toolStripButton_GroupBy_Click(object sender, EventArgs e)
        {
            // Note: This event handler is used by all grid toolbars
            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            Cursor = Cursors.WaitCursor;

            toggleGridGroupByBox(grid);

            Cursor = Cursors.Default;
        }

        private void _toolStripButton_Save_Click(object sender, EventArgs e)
        {
            // Note: This event handler is used by all grid toolbars
            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            Cursor = Cursors.WaitCursor;

            saveGrid(grid);

            Cursor = Cursors.Default;
        }

        private void _toolStripButton_Print_Click(object sender, EventArgs e)
        {
            // Note: This event handler is used by all grid toolbars
            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            Cursor = Cursors.WaitCursor;

            printGrid(grid);

            Cursor = Cursors.Default;
        }

        private void HighLightFindingDiffs(RichTextBox rtb1, RichTextBox rtb2)
        {
            try
            {
                const string searchSC = "Security Check:";
                const string searchRL = "Risk Level:";
                const string searchF = "Findings:";
                const string replaceSC = "\\cf0Security Check:";
                const string replaceRL = "\\cf0Risk Level:";
                const string replaceF = "\\cf0Findings:";

                const string searchBlue = "blue0";
                const string replaceBlue = "blue255";

                string rtf1 = rtb1.Rtf;
                string rtf2 = rtb2.Rtf;
                int nLength = rtf1.Length > rtf2.Length ? rtf1.Length : rtf2.Length;
                StringBuilder sb1 = new StringBuilder();
                StringBuilder sb2 = new StringBuilder();

                // First, Update the color table
                rtf1 = rtf1.Replace(searchBlue, replaceBlue);
                rtf2 = rtf2.Replace(searchBlue, replaceBlue);


                // Second, Find Index of Security Check
                int index1_1 = rtf1.IndexOf(searchSC);
                sb1.Append(rtf1.Substring(0, index1_1));
                sb1.Append(replaceSC);

                int index1_2 = rtf2.IndexOf(searchSC);
                sb2.Append(rtf2.Substring(0, index1_2));
                sb2.Append(replaceSC);

                // Next, Find Index of Risk Level
                index1_1 = rtf1.IndexOf(":", index1_1) + 1;
                int index2_1 = rtf1.IndexOf(searchRL);
                string sc1 = rtf1.Substring(index1_1, index2_1 - index1_1);

                index1_2 = rtf2.IndexOf(":", index1_2) + 1;
                int index2_2 = rtf2.IndexOf(searchRL);
                string sc2 = rtf2.Substring(index1_2, index2_2 - index1_2);

                DiffString(sb1, sc1, sb2, sc2);

                sb1.Append(replaceRL);
                sb2.Append(replaceRL);

                // Next, Find Index of Finding
                index1_1 = rtf1.IndexOf(":", index2_1) + 1;
                index2_1 = rtf1.IndexOf(searchF);
                string rl1 = rtf1.Substring(index1_1, index2_1 - index1_1);

                index1_2 = rtf2.IndexOf(":", index2_2) + 1;
                index2_2 = rtf2.IndexOf(searchF);
                string rl2 = rtf2.Substring(index1_2, index2_2 - index1_2);

                DiffString(sb1, rl1, sb2, rl2);

                sb1.Append(replaceF);
                sb2.Append(replaceF);

                // Finally, compare Finding
                index1_1 = rtf1.IndexOf(":", index2_1) + 1;
                string f1 = rtf1.Substring(index1_1);

                index1_2 = rtf2.IndexOf(":", index2_2) + 1;
                string f2 = rtf2.Substring(index1_2);

                DiffString(sb1, f1, sb2, f2);

                rtb1.Rtf = sb1.ToString();
                rtb2.Rtf = sb2.ToString();
            }
            catch (Exception ex)
            {
                logX.loggerX.Error(string.Format("Error Highlighting Finding Differences, {0}", ex.Message));
            }
        }

        private void DiffString(StringBuilder sb1, string ds1, StringBuilder sb2, string ds2)
        {
            int nLength = ds1.Length > ds2.Length ? ds1.Length : ds2.Length;
            bool bDiffFound = false;
            StringBuilder quotedString = null;
            for (int x = 0; x < nLength; x++)
            {
                // If we've already found a difference just copy remaining string
                if (bDiffFound)
                {
                    if (x < ds1.Length)
                    {
                        sb1.Append(ds1[x]);
                    }
                    if (x < ds2.Length)
                    {
                        sb2.Append(ds2[x]);
                    }
                }
                else 
                {
                    // Check next char for difference
                    if (x < ds1.Length && x < ds2.Length && ds1[x] == ds2[x])
                    {
                        // Is this the start or end of quoted String
                        if(ds1[x] == '\'')
                        {
                            if(quotedString == null)
                            {
                                quotedString = new StringBuilder();
                            }
                            else
                            {
                                sb1.Append(quotedString);
                                sb2.Append(quotedString);
                                quotedString = null;
                            }
                        }
                        if (quotedString != null)
                        {
                            quotedString.Append(ds1[x]);
                        }
                        else
                        {
                            sb1.Append(ds1[x]);
                            sb2.Append(ds2[x]);
                        }
                    }                
                    else
                    {
                        if (!bDiffFound)
                        {
                            if (ds1[x - 1] == '\\')
                            {
                                sb1.Append("cf1\\");
                                sb2.Append("cf1\\");
                            }
                            else
                            {
                                sb1.Append("\\cf1");
                                sb2.Append("\\cf1");
                            }
                            bDiffFound = true;
                        }
                        if(quotedString != null)
                        {
                            sb1.Append(quotedString);
                            sb2.Append(quotedString);
                        }
                        if (x < ds1.Length)
                        {
                            sb1.Append(ds1[x]);
                        }
                        if (x < ds2.Length)
                        {
                            sb2.Append(ds2[x]);
                        }
                    }
                }
            }

            
        }
        
        private void SetActiveRow(Infragistics.Win.UltraWinGrid.UltraGrid grid, int metricId)
        {
            bool found = false;
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in grid.Rows)
            {
                row.Selected = false;
                if (row.Cells[colMetricId].Value != DBNull.Value && (int)row.Cells[colMetricId].Value == metricId)
                {
                    row.Selected = true;
                    grid.ActiveRow = row;
                    found = true;
                }
            }
            if(!found && grid.Rows.Count > 0)
            {
                grid.Rows[0].Selected = true;
                grid.ActiveRow = grid.Rows[0];
            }
        }

        private void ShowSecurityCheck()
        {
            if (_grid_ReportCard.ActiveRow != null && _grid_ReportCard.ActiveRow.IsDataRow)
            {
                int selectedMetric = (int)_grid_ReportCard.ActiveRow.Cells[colMetricId].Value;

                foreach (UltraGridRow row in _grid_ComparisonSummary.DisplayLayout.Rows.GetAllNonGroupByRows())
                {
                    if ((int)((DataRowView)row.ListObject)[colMetricId] == selectedMetric)
                    {
                        row.Selected = true;
                        row.Activate();
                        break;
                    }
                }

                _ultraTabControl_Compare.SelectedTab = _ultraTabControl_Compare.Tabs[tabKey_CompareFindings];

            }            
        }

        private void _cmsi_Metrics_ConfigureSecurityCheck_Click(object sender, EventArgs e)
        {
            ShowSecurityCheck();
        }

        private void ultraCombo_Assessment2_BeforeDropDown(object sender, CancelEventArgs e)
        {
            LoadAssessmentComboBox(ultraCombo_Assessment2);
            if (m_assessmentId2 == -1)
            {
                if (ultraCombo_Assessment2.Items[0].DisplayText == ComboTextChooseAssessment)
                {
                    ultraCombo_Assessment2.Items.RemoveAt(0);
                }
            }
        }

        private void ultraGrid_ExplanationNotes_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {

            e.Layout.Override.RowSizing = RowSizing.AutoFree;
            e.Layout.Override.AllowColMoving = AllowColMoving.NotAllowed;

            UltraGridBand band = e.Layout.Bands[0];

            band.Columns[colServerName].Width = 130;
            band.Columns[colServerName].AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.VisibleRows;
            band.Columns[colServerName].CellAppearance.TextVAlign = VAlign.Top;

//            band.Columns[colIsExplained].Width = 54;
//            band.Columns[colIsExplained].MaxWidth = 54;
            band.Columns[colIsExplained].Header.Caption = "Explained";
            band.Columns[colIsExplained].AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.VisibleRows;
            band.Columns[colIsExplained].CellAppearance.TextVAlign = VAlign.Top;

            band.Columns[colNotes].Width = 200;
            band.Columns[colNotes].Header.Caption = "Notes";
            band.Columns[colNotes].AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.VisibleRows;
            band.Columns[colNotes].CellMultiLine = DefaultableBoolean.True;

            band.Columns[colMetricId].Hidden = true;
            band.Columns[colSnapshotId].Hidden = true;

        }

        private void toolStripButton_ShowDiffsOnly_Click(object sender, EventArgs e)
        {            
            m_showDifferencesOnly = !m_showDifferencesOnly;
            _toolStripButton_ShowDiffsOnly.Text = m_showDifferencesOnly ? ShowAll : ShowDiffsOnly;

            LoadReportCardDataSource();
            HighlightSummaryDifferences();
        }

        private void _grid_ReportCard_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            ShowSecurityCheck();
        }

        private void toolStripButton_SCShowDiffOnly_Click(object sender, EventArgs e)
        {
            m_showSCDifferencesOnly = !m_showSCDifferencesOnly;
            _toolStripButton_SCShowDiffOnly.Text = m_showSCDifferencesOnly ? ShowAll : ShowDiffsOnly;

            LoadComparisonDataSource();
        }

        private void _toolStripButton_Notes2Copy_Click(object sender, EventArgs e)
        {
            if (_grid_ExplanationNotes2.Selected.Rows.Count > 0)
            {
                foreach (UltraGridRow row in _grid_ExplanationNotes2.Selected.Rows)
                {
                    string expression = string.Format("{0} = '{1}'", colServer, row.Cells[colServerName].Value);
                    DataRow[] foundRows = m_ExplanationTable1.Select(expression);
                    if (foundRows.Length > 0)
                    {
                        //foundRows[0][colIsExplained] = row.Cells[colIsExplained].Value;
                        //foundRows[0][colNotes] = row.Cells[colNotes].Value;
                        //string expression2 =
                        //    string.Format("{0} = {1} and {2} = {3}", colMetricId, (int)row.Cells[colMetricId].Value,
                        //                  colSnapshotId1, (int) foundRows[0][colSnapshotId]);
                        //DataRow[] foundRows2 = m_ComparisonTable.Select(expression2);
                        //if(foundRows2.Length > 0)
                        //{
                        //    foundRows2[0][colIsExplained1] = row.Cells[colIsExplained].Value;
                        //    foundRows2[0][colNotes1] = row.Cells[colNotes].Value;
                        //}
                        int metricId = (int) row.Cells[colMetricId].Value;
                        UpdateExplanationNotes(m_assessmentId1, metricId, (int)foundRows[0][colSnapshotId],
                                               (bool)row.Cells[colIsExplained].Value, (string)row.Cells[colNotes].Value);

                        UpdateAssessment1ToCurrentSelection();
                        SetActiveRow(_grid_ComparisonSummary, metricId);
                    }
                }
            }
            
        }

        private void _toolStripButton_Notes1Copy_Click(object sender, EventArgs e)
        {
            if (_grid_ExplanationNotes1.Selected.Rows.Count > 0)
            {
                foreach(UltraGridRow row in _grid_ExplanationNotes1.Selected.Rows)
                {
                    string expression = string.Format("{0} = '{1}'", colServer, row.Cells[colServerName].Value);
                    DataRow[] foundRows = m_ExplanationTable2.Select(expression);
                    if (foundRows.Length > 0)
                    {
                        //foundRows[0][colIsExplained] = row.Cells[colIsExplained].Value;
                        //foundRows[0][colNotes] = row.Cells[colNotes].Value;
                        //string expression2 = string.Format("{0} = {1} and {2} = {3}", colMetricId, (int)row.Cells[colMetricId].Value,
                        //                                          colSnapshotId2, (int)foundRows[0][colSnapshotId]);
                        //DataRow[] foundRows2 = m_ComparisonTable.Select(expression2);
                        //if (foundRows2.Length > 0)
                        //{
                        //    foundRows2[0][colIsExplained2] = row.Cells[colIsExplained].Value;
                        //    foundRows2[0][colNotes2] = row.Cells[colNotes].Value;
                        //}
                        int metricId = (int)row.Cells[colMetricId].Value;
                        UpdateExplanationNotes(m_assessmentId2, metricId, (int)foundRows[0][colSnapshotId],
                                               (bool)row.Cells[colIsExplained].Value, (string)row.Cells[colNotes].Value);

                        UpdateAssessment2ToCurrentSelection();
                        SetActiveRow(_grid_ComparisonSummary, metricId);

                    }
                }
            }

        }

        private void EnableCopyButton(int assessment)
        {
            bool enable = false;
            string toolTipText = CopyDisabledToolTip;
            if (assessment == 1)
            {
                if(!Program.gController.isAdmin)
                {
                    enable = false;
                    toolTipText = "View only user is not allowed to copy Explaination Notes";
                }
                else if (_grid_ExplanationNotes1.Selected.Rows.Count > 0)
                {
                    if (!m_PolicyAssessment2.IsApprovedAssessment && !m_PolicyAssessment2.IsCurrentAssessment)
                    {
                        enable = true;
                        toolTipText = string.Format(CopyEnabledToolTip, m_PolicyAssessment2.AssessmentName);
                    }
                }
                else
                {
                    toolTipText = CopySelectServer;
                }
                _toolStripButton_Notes1Copy.Enabled = enable;
                _toolStripButton_Notes1Copy.ToolTipText = toolTipText;                
            }

            else if (assessment == 2)
            {
                if (!Program.gController.isAdmin)
                {
                    enable = false;
                    toolTipText = "View only user is not allowed to copy Explaination Notes";
                } 
                else if (_grid_ExplanationNotes2.Selected.Rows.Count > 0)
                {
                    if (!m_PolicyAssessment1.IsApprovedAssessment && !m_PolicyAssessment1.IsCurrentAssessment)
                    {
                        enable = true;
                        toolTipText = string.Format(CopyEnabledToolTip, m_PolicyAssessment1.AssessmentName);
                    }
                }
                else
                {
                    toolTipText = CopySelectServer;
                }
                _toolStripButton_Notes2Copy.Enabled = enable;
                _toolStripButton_Notes2Copy.ToolTipText = toolTipText;
            }
        }

        private void _grid_ExplanationNotes1_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            EnableCopyButton(1);
        }

        private void _grid_ExplanationNotes2_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            EnableCopyButton(2);            
        }

        private void UpdateExplanationNotes(int assessmentId, int metricId, int snapshotId, bool isExplained, string notes)
        {
            if (assessmentId > 0 && metricId > 0 && snapshotId > 0)
            {
                try
                {
                    if (!string.IsNullOrEmpty(Program.gController.Repository.ConnectionString))
                    {
                        // Retrieve policy information.
                        logX.loggerX.Info("Update Policy Assessments Explanation Notes");
                        DataSet ds = new DataSet();

                        using (
                            SqlConnection connection =
                                new SqlConnection(Program.gController.Repository.ConnectionString)
                            )
                        {
                            // Open the connection.
                            connection.Open();

                            SqlParameter paramPolicyId = new SqlParameter(ParamPolicyId, m_policyId);
                            SqlParameter paramAssessmentId = new SqlParameter(ParamAssessmentId, assessmentId);
                            SqlParameter paramMetricId = new SqlParameter(ParamMetricId, metricId);
                            SqlParameter paramSnapshotId = new SqlParameter(ParamSnapshotId, snapshotId);
                            SqlParameter paramIsExplained = new SqlParameter(ParamIsExplained, isExplained);
                            SqlParameter paramNotes = new SqlParameter(ParamExplanationNotes, notes);

                            SqlCommand cmd = new SqlCommand(QueryUpdateExplanationNotes, connection);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();

                            cmd.Parameters.Add(paramPolicyId);
                            cmd.Parameters.Add(paramAssessmentId);
                            cmd.Parameters.Add(paramMetricId);
                            cmd.Parameters.Add(paramSnapshotId);
                            cmd.Parameters.Add(paramIsExplained);
                            cmd.Parameters.Add(paramNotes);

                            cmd.ExecuteNonQuery();
                        }

                        Program.gController.SignalRefreshPoliciesEvent(m_policyId);
                        
                    }
                }
                catch(Exception ex)
                {
                    logX.loggerX.Error(string.Format("Error Updating Explaination Notes, {0}", ex.Message));
                }
            }
        }
      
        private void SetDescriptionText()
        {
            if (m_PolicyAssessment1 == null || m_PolicyAssessment2 == null)
            {
                Description = NoAssessmentDescription;
            }
            else
            {
                switch (_ultraTabControl_Compare.SelectedTab.Key)
                {
                    case tabKey_CompareSummaries:
                        Description = SummaryTabDescription;
                        break;
                    case tabKey_CompareFindings:
                        Description = SecurityChecksTabDescription;
                        break;
                    case tabKey_CompareNotes:
                        Description = InternalNotesTabDescription;
                        break;
                }
            }
        }

        private void _ultraTabControl_ComparisonDetails_VisibleChanged(object sender, EventArgs e)
        {
            // Position Label above Tab to align with tab right
            Point sc = this.PointToClient(_ultraGroupBox_Finding1.PointToScreen(_ultraGroupBox_Finding1.Location));
            sc.X = sc.X < 0 ? 20 : sc.X;

            _label_CompareSummaryDetails.Padding = new Size(sc.X, 0);

            SetDescriptionText();
        }

        private void _ultraTabControl_Compare_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            SetDescriptionText();
        }

        private void _grid_ComparisonSummary_InitializeRow(object sender, InitializeRowEventArgs e)
        {

        }

        #region Help

        private void _button_Help_Click(object sender, EventArgs e)
        {
            ShowHelpTopic();
        }

        private void Form_AssessmentComparison_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            ShowHelpTopic();
        }

        private void ShowHelpTopic()
        {
            string helpTopic = Utility.Help.CompareAssessmentsSummaryHelpTopic;            
            switch (_ultraTabControl_Compare.SelectedTab.Key)
            {
                case tabKey_CompareSummaries:
                    helpTopic = Utility.Help.CompareAssessmentsSummaryHelpTopic;
                    break;
                case tabKey_CompareFindings:
                    helpTopic = Utility.Help.CompareAssessmentsSecurityChecksHelpTopic;
                    break;
                case tabKey_CompareNotes:
                    helpTopic = Utility.Help.CompareAssessmentsInternalReviewNotesHelpTopic;
                    break;
            }
            Program.gController.ShowTopic(helpTopic);
        }

        #endregion

        private void ultraCombo_Assessment1_BeforeDropDown(object sender, CancelEventArgs e)
        {
            LoadAssessmentComboBox(ultraCombo_Assessment1);
        }


        private void _cmsi_grid_ColumnChooser_Click(object sender, EventArgs e)
        {
            // Note: This event handler is used by all grid context menus
            Cursor = Cursors.WaitCursor;

            UltraGrid grid = (UltraGrid)((ContextMenuStrip)((ToolStripItem)sender).Owner).SourceControl;

            showGridColumnChooser(grid);

            Cursor = Cursors.Default;
        }

        private void _cmsi_grid_viewGroupByBox_Click(object sender, EventArgs e)
        {
            // Note: This event handler is used by all grid context menus
            Cursor = Cursors.WaitCursor;

            UltraGrid grid = (UltraGrid)((ContextMenuStrip)((ToolStripItem)sender).Owner).SourceControl;

            toggleGridGroupByBox(grid);

            Cursor = Cursors.Default;
        }

        private void _cmsi_grid_Print_Click(object sender, EventArgs e)
        {
            // Note: This event handler is used by all grid context menus
            Cursor = Cursors.WaitCursor;

            UltraGrid grid = (UltraGrid)((ContextMenuStrip)((ToolStripItem)sender).Owner).SourceControl;

            printGrid(grid);

            Cursor = Cursors.Default;
        }

        private void _cmsi_grid_Save_Click(object sender, EventArgs e)
        {
            // Note: This event handler is used by all grid context menus
            Cursor = Cursors.WaitCursor;

            UltraGrid grid = (UltraGrid)((ContextMenuStrip)((ToolStripItem)sender).Owner).SourceControl;

            saveGrid(grid);

            Cursor = Cursors.Default;
        }

        private void _grid_ReportCard_InitializeGroupByRow(object sender, InitializeGroupByRowEventArgs e)
        {
            UltraGrid grid = (UltraGrid)sender;
            UltraGridColumn col = e.Row.Column;

            if (e.Row.Value != DBNull.Value)
            {
                if (col.ValueList != null)
                {
                    if (col.ValueList.ShouldDisplayImage)
                    {
                        e.Row.Appearance.Image =
                            grid.DisplayLayout.ValueLists[((ValueList) col.ValueList).Key].FindByDataValue(
                                (int) e.Row.Value).Appearance.Image;
                    }
                    e.Row.Description = string.Format("{0} : {1} ({2} item{3})",
                                                      col.Header.Caption,
                                                      grid.DisplayLayout.ValueLists[((ValueList) col.ValueList).Key].
                                                          FindByDataValue((int) e.Row.Value).DisplayText,
                                                      e.Row.Rows.Count,
                                                      e.Row.Rows.Count == 1 ? string.Empty : "s"
                        );
                }
            }
        }

        private void _grid_ComparisonSummary_InitializeGroupByRow(object sender, InitializeGroupByRowEventArgs e)
        {
            UltraGrid grid = (UltraGrid)sender;
            UltraGridColumn col = e.Row.Column;

            if (e.Row.Value != DBNull.Value)
            {
                if (col.ValueList != null)
                {
                    if (col.ValueList.ShouldDisplayImage)
                    {
                        e.Row.Appearance.Image =
                            grid.DisplayLayout.ValueLists[((ValueList) col.ValueList).Key].FindByDataValue(
                                (int) e.Row.Value).Appearance.Image;
                    }
                    e.Row.Description = string.Format("{0} : {1} ({2} item{3})",
                                                      col.Header.Caption,
                                                      grid.DisplayLayout.ValueLists[((ValueList) col.ValueList).Key].
                                                          FindByDataValue((int) e.Row.Value).DisplayText,
                                                      e.Row.Rows.Count,
                                                      e.Row.Rows.Count == 1 ? string.Empty : "s"
                        );
                }
            }
        }

        private bool DoesServerExistInAssessment(Policy assessment, string serverName)
        {
            bool bFound = false;
            if (assessment != null)
            {
                if (assessment.Members != null)
                {
                    foreach (RegisteredServer r in assessment.Members)
                    {
                        if (r.ConnectionName == serverName)
                        {
                            bFound = true;
                            break;
                        }
                    }
                }
            }
            return bFound;
        }

        private bool IsSecurityCheckEnabledInAssessment(Policy assessment, int metricId)
        {
            bool bEnabled = false;
            if (assessment != null)
            {
                if (assessment.PolicyMetrics == null)
                {
                    assessment.GetPolicyMetrics();
                }
                if (assessment.PolicyMetrics != null)
                {
                    foreach (PolicyMetric metric in assessment.PolicyMetrics)
                    {
                        if (metric.MetricId == metricId)
                        {                            
                            bEnabled = metric.IsEnabled;
                            break;
                        }
                    }
                }
            }
            return bEnabled;
        }


    }
}

