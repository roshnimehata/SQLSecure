using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinTabControl;

using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Controls;
using Idera.SQLsecure.UI.Console.Utility;

namespace Idera.SQLsecure.UI.Console.Controls
{
    public partial class ReportCard : ViewSection, Interfaces.IView, Interfaces.ICommandHandler, Interfaces.IRefresh
    {
        #region types

        public class RiskCounts
        {
            private int m_countOk = 0;
            private int m_countLowExplained = 0;
            private int m_countMediumExplained = 0;
            private int m_countHighExplained = 0;
            private int m_countLow = 0;
            private int m_countMedium = 0;
            private int m_countHigh = 0;
            private int m_countUndetermined = 0;
            private DateTime m_assessmentDateTime = DateTime.MinValue;


            private Dictionary<int, RiskCounts> m_MetricCounts = new Dictionary<int, RiskCounts>();

            public DateTime CollectionDateTime
            {
                get { return m_assessmentDateTime; }
                set { m_assessmentDateTime = value;}
            }

            public int Total
            {
                get { return m_countOk + m_countLowExplained + m_countMediumExplained + m_countHighExplained + m_countLow + m_countMedium + m_countHigh + m_countUndetermined; }
            }

            public int RiskCount
            {
                get
                {
                    return m_countLow + m_countMedium + m_countHigh;
                }
            }

            public int RiskCountHigh
            {
                get
                {
                    return m_countHigh;
                }
            }

            public int RiskCountMedium
            {
                get
                {
                    return m_countMedium;
                }
            }

            public int RiskCountLow
            {
                get
                {
                    return m_countLow;
                }
            }

            public int RiskCountHighExplained
            {
                get
                {
                    return m_countHighExplained;
                }
            }

            public int RiskCountMediumExplained
            {
                get
                {
                    return m_countMediumExplained;
                }
            }

            public int RiskCountLowExplained
            {
                get
                {
                    return m_countLowExplained;
                }
            }

            public Policy.SeverityExplained HighestRisk
            {
                get
                {
                    Policy.SeverityExplained severity = Policy.SeverityExplained.Undetermined;

                    if (m_countHigh > 0)
                        severity = Policy.SeverityExplained.High;
                    else if (m_countMedium > 0)
                        severity = Policy.SeverityExplained.Medium;
                    else if (m_countLow > 0)
                        severity = Policy.SeverityExplained.Low;
                    else if (m_countHighExplained > 0)
                        severity = Policy.SeverityExplained.HighExplained;
                    else if (m_countMediumExplained > 0)
                        severity = Policy.SeverityExplained.MediumExplained;
                    else if (m_countLowExplained > 0)
                        severity = Policy.SeverityExplained.LowExplained;
                    else if (m_countOk > 0)
                        severity = Policy.SeverityExplained.Ok;

                    return severity;
                }
            }

            public Image HighestRiskImage
            {
                get
                {
                    Image image;

                    switch ((int)HighestRisk)
                    {
                        case (int)Policy.SeverityExplained.Ok:
                            image = global::Idera.SQLsecure.UI.Console.Properties.Resources.check_16;
                            break;
                        case (int)Policy.SeverityExplained.LowExplained:
                            image = global::Idera.SQLsecure.UI.Console.Properties.Resources.LowRiskExplained_16;
                            break;
                        case (int)Policy.SeverityExplained.MediumExplained:
                            image = global::Idera.SQLsecure.UI.Console.Properties.Resources.MediumRiskExplained_16;
                            break;
                        case (int)Policy.SeverityExplained.HighExplained:
                            image = global::Idera.SQLsecure.UI.Console.Properties.Resources.HighRiskExplained_16;
                            break;
                        case (int)Policy.SeverityExplained.Low:
                            image = global::Idera.SQLsecure.UI.Console.Properties.Resources.LowRisk_16;
                            break;
                        case (int)Policy.SeverityExplained.Medium:
                            image = global::Idera.SQLsecure.UI.Console.Properties.Resources.MediumRisk_16;
                            break;
                        case (int)Policy.SeverityExplained.High:
                            image = global::Idera.SQLsecure.UI.Console.Properties.Resources.HighRisk_16;
                            break;
                        default:
                            image = AppIcons.AppImage16(AppIcons.Enum.Unknown);
                            break;
                    }

                    return image;
                }
            }

            public string HighestRiskText
            {
                get
                {
                    string text = DescriptionHelper.GetEnumDescription(HighestRisk);

                    if (HighestRisk != Policy.SeverityExplained.Ok)
                    {
                        text += @" Risk";

                        if (RiskCount != 1)
                        {
                            text = DescriptionHelper.GetPlural(text);
                        }
                    }
 
                    switch ((int)HighestRisk)
                    {
                        case (int)Policy.SeverityExplained.Low:
                            if (RiskCountLowExplained > 0)
                            {
                                text += string.Format(" + {0} Explained", RiskCountLowExplained);
                            }
                            break;
                        case (int)Policy.SeverityExplained.Medium:
                            if (RiskCountMediumExplained > 0)
                            {
                                text += string.Format(" + {0} Explained", RiskCountMediumExplained);
                            }
                            break;
                        case (int)Policy.SeverityExplained.High:
                            if (RiskCountHighExplained > 0)
                            {
                                text += string.Format(" + {0} Explained", RiskCountHighExplained);
                            }
                            break;
                    }

                    return text;
                }
            }

            public string AllRisksText
            {
                get
                {
                    StringBuilder sb = new StringBuilder();
                    Policy.SeverityExplained risk;

                    if (HighestRisk == Policy.SeverityExplained.Ok)
                    {
                        sb.Append(NoRisks);
                    }
                    else
                    {
                        if (m_countHigh > 0)
                        {
                            risk = Policy.SeverityExplained.High;
                            sb.Append(m_countHigh.ToString(NumericFormat));
                            sb.Append(" ");
                            sb.Append(GetRiskText(risk));
                        }

                        if (m_countMedium > 0)
                        {
                            if (m_countHigh > 0)
                            {
                                sb.Append(m_countLow == 0 ? AndDisplay : CommaDisplay);
                            }

                            risk = Policy.SeverityExplained.Medium;
                            sb.Append(m_countMedium.ToString(NumericFormat));
                            sb.Append(" ");
                            sb.Append(GetRiskText(risk));
                        }

                        if (m_countLow > 0)
                        {
                            sb.Append(sb.Length == 0 ? string.Empty : AndDisplay);
                            risk = Policy.SeverityExplained.Low;
                            sb.Append(m_countLow.ToString(NumericFormat));
                            sb.Append(" ");
                            sb.Append(GetRiskText(risk));
                        }
                    }

                    return sb.ToString();
                }
            }

            public Dictionary<int, RiskCounts> Metrics
            {
                get { return m_MetricCounts; }
            }

            // methods that make life easier
            public void Clear()
            {
                m_countOk =
                    m_countLowExplained =
                    m_countMediumExplained =
                    m_countHighExplained =
                    m_countLow = 
                    m_countMedium = 
                    m_countHigh = 
                    m_countUndetermined = 0;
            }

            public void AddCounts(RiskCounts newCounts)
            {
                m_countOk += newCounts.m_countOk;
                m_countLowExplained += newCounts.m_countLowExplained;
                m_countMediumExplained += newCounts.m_countMediumExplained;
                m_countHighExplained += newCounts.m_countHighExplained;
                m_countLow += newCounts.m_countLow;
                m_countMedium += newCounts.m_countMedium;
                m_countHigh += newCounts.m_countHigh;
                m_countUndetermined += newCounts.m_countUndetermined;
            }

            public void IncrementCount(Policy.Severity severity)
            {
                switch (severity)
                {
                    case Policy.Severity.Ok:
                        m_countOk++;
                        break;
                    case Policy.Severity.Low:
                        m_countLow++;
                        break;
                    case Policy.Severity.Medium:
                        m_countMedium++;
                        break;
                    case Policy.Severity.High:
                        m_countHigh++;
                        break;
                    case Policy.Severity.Undetermined:
                        m_countUndetermined++;
                        break;
                }
            }

            public void IncrementCount(Policy.SeverityExplained severity)
            {
                switch (severity)
                {
                    case Policy.SeverityExplained.Ok:
                        m_countOk++;
                        break;
                    case Policy.SeverityExplained.LowExplained:
                        m_countLowExplained++;
                        break;
                    case Policy.SeverityExplained.MediumExplained:
                        m_countMediumExplained++;
                        break;
                    case Policy.SeverityExplained.HighExplained:
                        m_countHighExplained++;
                        break;
                    case Policy.SeverityExplained.Low:
                        m_countLow++;
                        break;
                    case Policy.SeverityExplained.Medium:
                        m_countMedium++;
                        break;
                    case Policy.SeverityExplained.High:
                        m_countHigh++;
                        break;
                    case Policy.SeverityExplained.Undetermined:
                        m_countUndetermined++;
                        break;
                }
            }

            public int GetRiskCount(Policy.SeverityExplained risk)
            {
                int count = 0;

                switch (risk)
                {
                    case Policy.SeverityExplained.Ok:
                        count = m_countOk;
                        break;
                    case Policy.SeverityExplained.LowExplained:
                        count = m_countLowExplained;
                        break;
                    case Policy.SeverityExplained.MediumExplained:
                        count = m_countMediumExplained;
                        break;
                    case Policy.SeverityExplained.HighExplained:
                        count = m_countHighExplained;
                        break;
                    case Policy.SeverityExplained.Low:
                        count = m_countLow;
                        break;
                    case Policy.SeverityExplained.Medium:
                        count = m_countMedium;
                        break;
                    case Policy.SeverityExplained.High:
                        count = m_countHigh;
                        break;
                    case Policy.SeverityExplained.Undetermined:
                        count = m_countUndetermined++;
                        break;
                }

                return count;
            }

            public string GetRiskText(Policy.SeverityExplained risk)
            {
                string text = DescriptionHelper.GetEnumDescription(risk);

                if (GetRiskCount(risk) != 1)
                {
                    text = DescriptionHelper.GetPlural(text);
                }

                return text;
            }
        }

        private enum DetailsTab
        {
            None = -1,
            Details = 0,
            ExplanationNotes = 1
        }

        #endregion           

        #region IView

        void Interfaces.IView.SetContext(Interfaces.IDataContext contextIn)
        {
            m_context = (Data.ReportCard)contextIn;
            m_policy = m_context.Policy;
            m_serverInstance = m_context.Server;

            this.Title = (m_serverInstance == null ? @"Enterprise" : @"Server") +
                                            @" Security Report Card";

            setMenuConfiguration();

            if (m_serverInstance != null && !Sql.RegisteredServer.IsServerRegistered(m_serverInstance.ConnectionName))
            {
                MsgBox.ShowWarning(ErrorMsgs.ObjectExplorerCaption, ErrorMsgs.ServerNotRegistered);
            }

            _ultraTabControl_Details.Tabs[(int)DetailsTab.ExplanationNotes].Visible = m_policy.IsAssessment;

            loadDataSource();
        }

        String Interfaces.IView.HelpTopic
        {
            get { return (m_serverInstance == null) ? Utility.Help.SecuritySummaryPolicyReportCardHelpTopic : Utility.Help.SecuritySummaryServerReportCardHelpTopic; }
        }

        String Interfaces.IView.ConceptTopic
        {
            get { return (m_serverInstance == null) ? Utility.Help.SecuritySummaryPolicyReportCardHelpTopic : Utility.Help.SecuritySummaryServerReportCardHelpTopic; }
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
                    Debug.Assert(false, "Unknown command passed to ReportCard");
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
            logX.loggerX.Error("Error - ReportCard showBaseline command called erroneously");
        }

        protected virtual void showCollect()
        {
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - ReportCard showCollect command called erroneously");
        }

        protected virtual void showConfigure()
        {
            Forms.Form_SqlServerProperties.Process(m_serverInstance.ConnectionName, Forms.Form_SqlServerProperties.RequestedOperation.EditCofiguration, Program.gController.isAdmin);
        }

        protected virtual void showDelete()
        {
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - ReportCard showDelete command called erroneously");
        }

        protected virtual void showProperties()
        {
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - ReportCard showProperties command called erroneously");
        }

        protected virtual void showRefresh()
        {
            //loadDataSource();
            // refresh the entire view to keep all info in sync
            Program.gController.RefreshCurrentView();
        }

        protected virtual void showPermissions(Views.View_PermissionExplorer.Tab tabIn)
        {
            Program.gController.ShowRootView(new Utility.NodeTag(new Data.PermissionExplorer(m_serverInstance, m_snapshotId, tabIn),
                                                        Utility.View.PermissionExplorer));
        }

        #endregion

        #region IRefresh Members

        public void RefreshView()
        {
            showRefresh();
        }

        #endregion

        #region ctors

        public ReportCard()
        {
            InitializeComponent();
 
            // Initialize base class fields.
            m_menuConfiguration = new Utility.MenuConfiguration();

            _ultraTabControl_ReportCard.ImageList = AppIcons.AppImageList16();

            // hook the toolbar labels to the grids so the heading can be used for printing
            _grid_ReportCard.Tag = _label_ReportCard;
            _grid_ExplanationNotes.Tag = _label_ExplanationNotes;

            // hook the grids to the toolbars so they can be used for button processing
            _headerStrip_ReportCard.Tag = _grid_ReportCard;
            _headerStrip_ExplanationNotes.Tag = _grid_ExplanationNotes;

            // Hookup all application images
            _toolStripButton_ReportCardColumnChooser.Image = AppIcons.AppImage16(AppIcons.Enum.GridFieldChooser);
            _toolStripButton_ReportGroupBy.Image = AppIcons.AppImage16(AppIcons.Enum.GridGroupBy);
            _toolStripButton_ReportSave.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);
            _toolStripButton_ExplanationNotesSave.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);
            _toolStripButton_ReportPrint.Image = AppIcons.AppImage16(AppIcons.Enum.Print);
            _toolStripButton_ExplanationNotesPrint.Image = AppIcons.AppImage16(AppIcons.Enum.Print);

            // set all grids to start in the same initial display mode
            _grid_ReportCard.DisplayLayout.GroupByBox.Hidden = Utility.Constants.InitialState_GroupByBoxHidden;
            _grid_ExplanationNotes.DisplayLayout.GroupByBox.Hidden = Utility.Constants.InitialState_GroupByBoxHidden;

            // load value lists for grid display
            ValueList severityValueList = new ValueList();
            severityValueList.Key = valueListSeverity;
            severityValueList.DisplayStyle = ValueListDisplayStyle.Picture;
            severityValueList.Appearance.ImageVAlign = VAlign.Top;
            _grid_ReportCard.DisplayLayout.ValueLists.Add(severityValueList);

            ValueListItem listItem;

            severityValueList.ValueListItems.Clear();
            listItem = new ValueListItem(Policy.SeverityExplained.Ok, DescriptionHelper.GetEnumDescription(Policy.SeverityExplained.Ok));
            listItem.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.check_16;
            listItem.Appearance.ImageVAlign = VAlign.Top;
            severityValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(Policy.SeverityExplained.LowExplained, DescriptionHelper.GetEnumDescription(Policy.SeverityExplained.LowExplained));
            listItem.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.LowRiskExplained_16;
            listItem.Appearance.ImageVAlign = VAlign.Top;
            severityValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(Policy.SeverityExplained.MediumExplained, DescriptionHelper.GetEnumDescription(Policy.SeverityExplained.MediumExplained));
            listItem.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.MediumRiskExplained_16;
            listItem.Appearance.ImageVAlign = VAlign.Top;
            severityValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(Policy.SeverityExplained.HighExplained, DescriptionHelper.GetEnumDescription(Policy.SeverityExplained.HighExplained));
            listItem.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.HighRiskExplained_16;
            listItem.Appearance.ImageVAlign = VAlign.Top;
            severityValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(Policy.SeverityExplained.Low, DescriptionHelper.GetEnumDescription(Policy.SeverityExplained.Low));
            listItem.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.LowRisk_16;
            listItem.Appearance.ImageVAlign = VAlign.Top;
            severityValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(Policy.SeverityExplained.Medium, DescriptionHelper.GetEnumDescription(Policy.SeverityExplained.Medium));
            listItem.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.MediumRisk_16;
            listItem.Appearance.ImageVAlign = VAlign.Top;
            severityValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(Policy.SeverityExplained.High, DescriptionHelper.GetEnumDescription(Policy.SeverityExplained.High));
            listItem.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.HighRisk_16;
            listItem.Appearance.ImageVAlign = VAlign.Top;
            severityValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(Policy.SeverityExplained.Undetermined, DescriptionHelper.GetEnumDescription(Policy.SeverityExplained.Undetermined));
            listItem.Appearance.Image = AppIcons.AppImage16(AppIcons.Enum.Unknown);
            listItem.Appearance.ImageVAlign = VAlign.Top;
            severityValueList.ValueListItems.Add(listItem);

            ValueList severityValueList2 = severityValueList.Clone();
            _grid_ExplanationNotes.DisplayLayout.ValueLists.Add(severityValueList2);

            // Initialize the grids
            initDataSources();

            // Hide the focus rectangles on tabs and grids
            _ultraTabControl_ReportCard.DrawFilter = new HideFocusRectangleDrawFilter();
            _ultraTabControl_Details.DrawFilter = new HideFocusRectangleDrawFilter();
            _grid_ReportCard.DrawFilter = new HideFocusRectangleDrawFilter();
            _grid_ExplanationNotes.DrawFilter = new HideFocusRectangleDrawFilter();
        }

        #endregion

        #region fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Controls.ReportCard");
        private bool m_Initialized = false;

        Dictionary<int, Color> m_metricColorDict = new Dictionary<int, Color>();
        private Utility.MenuConfiguration m_menuConfiguration;
        private Data.ReportCard m_context;
        private Control m_focused = null; // To prevent focus from switching to the splitters, etc.
        private bool m_gridCellClicked = false;

        private Sql.Policy m_policy;
        private Sql.RegisteredServer m_serverInstance;
        private int m_snapshotId = 0;
        private DataTable m_AssessmentTable;
        private DataTable m_ReportCardTable;
        private DataTable m_ExplanationTable;
        private int m_Status = (int)Policy.Severity.Undetermined;
        private string m_StatusText;
        private Dictionary<string, RiskCounts> m_Categories = new Dictionary<string, RiskCounts>();
        private Dictionary<string, RiskCounts> m_Servers = new Dictionary<string, RiskCounts>();

        public delegate void MetricChanged(int? metricId);
        private MetricChanged m_MetricChangedDelegate = null;

        #endregion

        #region query, columns and constants

        private const string QueryGetAssessment = @"SQLsecure.dbo.isp_sqlsecure_getpolicyassessment";
        private const string ParamPolicyId = @"@policyid";
        private const string ParamAssessmentId = @"@assessmentid";
        private const string ParamRegisteredServerId = @"@registeredserverid";
        private const string ParamAlertsOnly = @"@alertsonly";
        private const string ParamBaselineOnly = @"@usebaseline";
        private const string ParamRunDate = @"@rundate";

        // Assessment details columns
        private const string colConnection = @"connectionname";
        private const string colSnapshotId = @"snapshotid";
        private const string colRegisteredServerId = @"registeredserverid";
        private const string colCollectionTime = @"collectiontime";
        private const string colMetricId = @"metricid";
        private const string colMetricName = @"metricname";
        private const string colMetricType = @"metrictype";
        private const string colMetricSeverityCode = @"metricseveritycode";
        private const string colMetricSeverity = @"metricseverity";
        private const string colMetricDescription = @"metricdescription";
        private const string colReportKey = @"metricreportkey";
        private const string colReportText = @"metricreporttext";
        private const string colSeverityCode = @"severitycode";
        private const string colSeverity = @"severity";
        private const string colCurrentValue = @"currentvalue";
        private const string colThresholdValue = @"thresholdvalue";
        private const string colIsExplained = @"isexplained";
        private const string colSeverityCodeExplained = @"severitycodeexplained";
        private const string colNotes = @"notes";

        // Scorecard columns
        private const string colSummary = @"summary";
        private const string colResult = @"result";
        private const string colResultRTF = @"resultrtf";

        private const string valueListSeverity = @"Severity";


        private const string DetailsRtfHeader = @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fswiss\fcharset0 Arial;}}{\colortbl ;\red0\green0\blue0;}\viewkind4\uc1";
        private const string DetailsRtfEnd = @"\par\r\n}";

        private const string DetailsMetricFormat = "Security Check: {0} {1} - {2} | " +
                                                   "Risk Level: {3} - {4} | " +
                                                   "Findings:";

        private const string DetailsRtfMetricFormat = "{0}\\pard\\f0\\fs16\\b\\cf1 Security Check:\\tab {1} {2}\\b0\\par\r\n" +
                                                      "\\pard      {3}\\par\r\n" +
                                                      "\\b\\cf1 Risk Level:\\tab {4}\\b0\\par\r\n" +
                                                      "\\pard      {5}\\par\r\n" +
                                                      "\\b\\ul\\cf1 Findings:\\ul0\\b0\\tab\\par\r\n";

        private const string DetailsMetricIdFormat = " ({0})";
        private const string DetailsFindingFormat = " | {0}: {1}";
        private const string DetailsRtfFindingFormat = "\\pard\\b {0}\\b0\t {1}\\par\r\n";
        private const string DisplayNoFindings = "No issues were found for this security check";
        private const string DisplayRtfNoFindings = "\\pard " + DisplayNoFindings + "\\par\r\n";
        private const string PolicySummaryFormat = "{0} {1}";
        private const string DisplayNoMetric = @"No security checks available";
        private const string NoRisks = @"no risks found";
        private const string RisksFormat = "Security Risks Exist - There are {0}";
        private const string AndDisplay = @" and ";
        private const string CommaDisplay = @", ";
        private const string TypeFilter = "{0} = '{1}'";
        private const string AllChecks = @"All";

        private const string NumericFormat = @"N0";

        private const string HeaderReportCard = @"Security Checks";
        private const string HeaderExplanationNotes = @"Explanation Notes";
        private const string HeaderDisplayExplanationNotes = @"{0} Server{1}, {2} Risk{3}, {4} Explained";
        private const string NoRecordsValue = "No {0} found";

        private const string HeaderDisplay = "{0} Security Check{1}";
        private static string HeaderDisplayLong = HeaderDisplay + " - {2} Risk{3} {4}";
        private const string PrintHeaderDisplay = "{0}\nSecurity Risk Report Card as of {1}\n\n{2}";
        private const string PrintEmptyHeaderDisplay = "{0}";

        #endregion

        #region properties

        public RiskCounts Risks
        {
            get
            {
                RiskCounts allCounts;
                
                if (!m_Categories.TryGetValue(AllChecks, out allCounts))
                {
                    allCounts = new RiskCounts();
                }

                return allCounts;
            }
        }
        public Dictionary<string, RiskCounts> ServerRisks
        {
            get
            {
                return m_Servers;
            }
        }

        public Policy.SeverityExplained Status
        {
            get
            {
                return Risks.HighestRisk;
            }
        }

        public string StatusText
        {
            get
            {
                string text = Risks.AllRisksText;

                if ((int)Risks.HighestRisk > (int)Policy.SeverityExplained.Ok)
                {
                    text = string.Format(RisksFormat, text);
                }

                return text;
            }
        }

        #endregion

        #region helpers

        protected void setMenuConfiguration()
        {
            //This is all currently being handled at the view level, but leaving functions for future
            //m_menuConfiguration.EditItems[(int)Utility.MenuItems_Edit.Remove] = false;
            //m_menuConfiguration.EditItems[(int)Utility.MenuItems_Edit.Properties] = false;

            //Program.gController.SetMenuConfiguration(m_menuConfiguration, this);
        }

        // Find the control with focus

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

        private DataTable createDataSourceAssessmentDetails()
        {
            // Create Assessment Details default datasources
            DataTable dt = new DataTable();
            dt.Columns.Add(colSnapshotId, typeof(int));
            dt.Columns.Add(colRegisteredServerId, typeof(int));
            dt.Columns.Add(colConnection, typeof(string));
            dt.Columns.Add(colCollectionTime, typeof(DateTime));
            dt.Columns.Add(colMetricId, typeof(int));
            dt.Columns.Add(colMetricName, typeof(string));
            dt.Columns.Add(colMetricType, typeof(string));
            dt.Columns.Add(colMetricSeverityCode, typeof(int));
            dt.Columns.Add(colMetricSeverity, typeof(string));
            dt.Columns.Add(colMetricDescription, typeof(string));
            dt.Columns.Add(colReportKey, typeof(string));
            dt.Columns.Add(colReportText, typeof(string));
            dt.Columns.Add(colSeverityCode, typeof(int));
            dt.Columns.Add(colSeverity, typeof(string));
            dt.Columns.Add(colCurrentValue, typeof(string));
            dt.Columns.Add(colThresholdValue, typeof(string));
            dt.Columns.Add(colIsExplained, typeof(bool));
            dt.Columns.Add(colSeverityCodeExplained, typeof(int));
            dt.Columns.Add(colNotes, typeof(string));

            return dt;
        }

        private DataTable createDataSourceReportCard()
        {
            // Create Report Card default datasources
            DataTable dt = new DataTable();
            dt.Columns.Add(colSeverityCode, typeof(int));
            dt.Columns.Add(colMetricId, typeof(int));
            dt.Columns.Add(colMetricName, typeof(string));
            dt.Columns.Add(colMetricDescription, typeof(string));
            dt.Columns.Add(colReportKey, typeof(string));
            dt.Columns.Add(colReportText, typeof(string));
            dt.Columns.Add(colMetricType, typeof(string));
            dt.Columns.Add(colSummary, typeof(string));
            dt.Columns.Add(colResult, typeof(string));
            dt.Columns.Add(colResultRTF, typeof(string));
            dt.Columns.Add(colIsExplained, typeof(bool));
            dt.Columns.Add(colNotes, typeof(string));

            return dt;
        }

        private DataTable createDataSourceExplanations()
        {
            // Create Report Card default datasources
            DataTable dt = new DataTable();

            dt.Columns.Add(colConnection, typeof(string));
            dt.Columns.Add(colSeverityCode, typeof(int));
            dt.Columns.Add(colIsExplained, typeof(bool));
            dt.Columns.Add(colNotes, typeof(string));

            return dt;
        }

        public DataTable ReportCardTable
        {
            get { return m_ReportCardTable; }
        }

        public string GetRTFDetails(int metricId)
        {
            string rtfDetails = string.Empty;
            string expression = string.Format("metricId = {0}", metricId);
            DataRow[] foundRows = m_ReportCardTable.Select(expression);

            if(foundRows.Length == 1)
            {
                rtfDetails = (string)foundRows[0][colResultRTF];
            }

            return rtfDetails;
        }
       
        private void initDataSources()
        {
            // Initialize the details
            m_AssessmentTable = createDataSourceAssessmentDetails();

            // Initialize Scorecard grid
            m_ReportCardTable = createDataSourceReportCard();

            _label_ReportCard.Text = HeaderReportCard;
            _grid_ReportCard.SetDataBinding(m_ReportCardTable.DefaultView, null);

            // Initialize Explanations grid
            m_ExplanationTable = createDataSourceExplanations();

            _label_ExplanationNotes.Text = HeaderExplanationNotes;
            _grid_ExplanationNotes.SetDataBinding(m_ExplanationTable.DefaultView, null);
        }

        private void loadDataSource()
        {
            logX.loggerX.Info("Retrieve Policy Report Card");

            _ultraTabControl_ReportCard.BeginUpdate();
            _ultraTabControl_ReportCard.SuspendLayout();

            try
            {
                _richTextBox_Details.Text = DisplayNoMetric;

                // Open connection to repository and query permissions.
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Setup parameters for all queries
                    SqlParameter paramPolicyId = new SqlParameter(ParamPolicyId, m_policy.PolicyId);
                    SqlParameter paramAssessmentId = new SqlParameter(ParamAssessmentId, m_policy.AssessmentId);
                    SqlParameter paramRegisteredServerId = new SqlParameter(ParamRegisteredServerId, (m_serverInstance == null ? DBNull.Value : (object)m_serverInstance.RegisteredServerId));
                    paramRegisteredServerId.IsNullable = true;
                    SqlParameter paramAlertsOnly = new SqlParameter(ParamAlertsOnly, SqlDbType.Bit, 0);
                    paramAlertsOnly.Value = 0;
                    SqlParameter paramBaselineOnly = new SqlParameter(ParamBaselineOnly, SqlDbType.Bit, 0);
                    paramBaselineOnly.Value = m_context.UseBaseline;
                    SqlParameter paramRunDate = new SqlParameter(ParamRunDate, m_context.SelectionDate);
                    if (m_serverInstance == null)//TODO CHECK IF THIS WORKS !!!! AND REMOVE
                    {
                        paramAlertsOnly.Value = 1;
                        //   return;
                    }
                    // Get Assessment
                    SqlCommand cmd = new SqlCommand(QueryGetAssessment, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();

                    cmd.Parameters.Add(paramPolicyId);
                    cmd.Parameters.Add(paramAssessmentId);
                    cmd.Parameters.Add(paramRegisteredServerId);
                    cmd.Parameters.Add(paramAlertsOnly);
                    cmd.Parameters.Add(paramBaselineOnly);
                    cmd.Parameters.Add(paramRunDate);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    m_AssessmentTable = ds.Tables[0];
                    m_AssessmentTable.DefaultView.Sort = colMetricId + ", " + colConnection;

                    // Create a new scorecard by processing the detail and creating a summary
                    m_ReportCardTable = createDataSourceReportCard();

                    DataRow newRow = m_ReportCardTable.NewRow();
                    int severity = 0;
                    RiskCounts categoryCounts;
                    RiskCounts serverCounts;
                    RiskCounts metricCounts = new RiskCounts();

                    string metrictype = AllChecks;
                    string server = string.Empty;
                    int selectedMetric = 0;

                    if (_grid_ReportCard.Rows.Count > 0 && _grid_ReportCard.Selected.Rows.Count > 0)
                    {
                        selectedMetric = (int)_grid_ReportCard.Selected.Rows[0].Cells[colMetricId].Value;
                    }

                    string selectedTab = _ultraTabControl_ReportCard.SelectedTab == null
                                             ? metrictype
                                             : _ultraTabControl_ReportCard.SelectedTab.Text;

                    m_Categories.Clear();
                    m_Servers.Clear();
                    _ultraTabControl_ReportCard.Tabs.Clear();
                    _ultraTabControl_ReportCard.Tabs.Add(metrictype, metrictype);
                    m_Categories.Add(metrictype, new RiskCounts());

                    string details = string.Empty;
                    string detailsRtf = string.Empty;
                    string summary;

                    foreach (DataRowView drv in m_AssessmentTable.DefaultView)
                    {
                        metrictype = (string)drv[colMetricType];
                        
                        if (!m_Categories.ContainsKey(metrictype))
                        {
                            _ultraTabControl_ReportCard.Tabs.Add(metrictype, metrictype);
                            m_Categories.Add(metrictype, new RiskCounts());
                            
                            if (metrictype == selectedTab)
                            {
                                _ultraTabControl_ReportCard.SelectedTab = _ultraTabControl_ReportCard.Tabs[metrictype];
                            }
                        }

                        server = (string)drv[colConnection];

                        if (!m_Servers.ContainsKey(server))
                        {
                            m_Servers.Add(server, new RiskCounts());
                        }

                        if (newRow[colMetricId] != DBNull.Value && (int)drv[colMetricId] != (int)newRow[colMetricId])
                        {
                            // fix the result and add the row
                            if (metricCounts.HighestRisk == Policy.SeverityExplained.Ok)
                            {
                                summary = metricCounts.HighestRiskText;
                                details += DisplayNoFindings;
                                detailsRtf += DisplayRtfNoFindings;
                            }
                            else
                            {
                                if (m_serverInstance == null && metricCounts.RiskCount > 0)
                                {
                                    summary =
                                        string.Format(PolicySummaryFormat,
                                                      metricCounts.RiskCount.ToString(NumericFormat),
                                                      metricCounts.HighestRiskText);
                                }
                                else
                                {
                                    summary = metricCounts.HighestRiskText;
                                }
                            }

                            newRow[colSummary] = summary;
                            newRow[colResult] = details;
                            newRow[colResultRTF] = detailsRtf + DetailsRtfEnd;
                            m_ReportCardTable.Rows.Add(newRow);
                            m_Categories.TryGetValue((string)newRow[colMetricType], out categoryCounts);
                            categoryCounts.IncrementCount(metricCounts.HighestRisk);
                            categoryCounts.Metrics.Add((int)newRow[colMetricId], metricCounts);

                            // reset values for the new metric
                            metricCounts = new RiskCounts();

                            // create the header for the new metric
                            details = string.Format(DetailsMetricFormat,
                                                    (string)drv[colMetricName],
                                                    drv[colReportKey].ToString().Trim().Length > 0
                                                        ? string.Format(DetailsMetricIdFormat, (string)drv[colReportKey])
                                                        : string.Empty,
                                                    (string)drv[colMetricDescription],
                                                    (string)drv[colMetricSeverity],
                                                    (string)drv[colThresholdValue]
                                                    );
                            detailsRtf = string.Format(DetailsRtfMetricFormat,
                                                        DetailsRtfHeader,
                                                        getRtfString((string)drv[colMetricName]),
                                                        drv[colReportKey].ToString().Trim().Length > 0
                                                            ? string.Format(DetailsMetricIdFormat,
                                                                            getRtfString((string)drv[colReportKey]))
                                                            : string.Empty,
                                                        getRtfString((string)drv[colMetricDescription]),
                                                        getRtfString((string)drv[colMetricSeverity]),
                                                        getRtfString((string)drv[colThresholdValue])
                                                    );

                            //reset the row and counts
                            newRow = m_ReportCardTable.NewRow();
                            severity = 0;
                        }
                        else
                        {
                            if (details.Length == 0)
                            {
                                // Fix first time in
                                details = string.Format(DetailsMetricFormat,
                                                        (string)drv[colMetricName],
                                                        drv[colReportKey].ToString().Trim().Length > 0
                                                            ? string.Format(DetailsMetricIdFormat, (string)drv[colReportKey])
                                                            : string.Empty,
                                                        (string)drv[colMetricDescription],
                                                        (string)drv[colMetricSeverity],
                                                        (string)drv[colThresholdValue]
                                                        );
                            }
                            if (detailsRtf.Length == 0)
                            {
                                // Fix first time in
                                detailsRtf = string.Format(DetailsRtfMetricFormat,
                                                             DetailsRtfHeader,
                                                             getRtfString((string)drv[colMetricName]),
                                                             drv[colReportKey].ToString().Trim().Length > 0
                                                                 ? string.Format(DetailsMetricIdFormat, getRtfString((string)drv[colReportKey]))
                                                                 : string.Empty,
                                                             getRtfString((string)drv[colMetricDescription]),
                                                             getRtfString((string)drv[colMetricSeverity]),
                                                             getRtfString((string)drv[colThresholdValue])
                                                         );
                            }
                        }

                        newRow[colMetricId] = drv[colMetricId];
                        newRow[colMetricName] = drv[colMetricName];
                        newRow[colReportKey] = drv[colReportKey];
                        newRow[colReportText] = drv[colReportText];
                        newRow[colMetricDescription] = drv[colMetricDescription];
                        newRow[colMetricType] = drv[colMetricType];

                        metricCounts.IncrementCount((Policy.SeverityExplained)drv[colSeverityCodeExplained]);

                        if ((int)drv[colSeverityCode] > 0)
                        {
                            severity = (int)drv[colSeverityCodeExplained];
                            details +=
                                string.Format(DetailsFindingFormat,
                                                  (string)drv[colConnection],
                                                  drv[colCurrentValue].ToString()
                                              );
                            detailsRtf +=
                                string.Format(DetailsRtfFindingFormat,
                                                  getRtfString((string)drv[colConnection] + ((bool)drv[colIsExplained] == true ? " (Explained)" : string.Empty) ),
                                                  getRtfString(drv[colCurrentValue].ToString())
                                                );
                        }
                        if (newRow[colSeverityCode] is DBNull || severity > (int)newRow[colSeverityCode])
                        {
                            newRow[colSeverityCode] = severity;
                        }

                        // Add to server counts for every metric
                        m_Servers.TryGetValue(server, out serverCounts);
                        if (drv[colCollectionTime] != DBNull.Value)
                        {
                            // collection time can be null on the Snapshot data missing metric
                            serverCounts.CollectionDateTime = (DateTime) drv[colCollectionTime];
                        }
                        serverCounts.IncrementCount((Policy.SeverityExplained)drv[colSeverityCodeExplained]);
                    }

                    //save the last row, but check for an empty in case of an error
                    if (newRow[colMetricId] != DBNull.Value)
                    {
                        // fix the result and add the row
                        if (metricCounts.HighestRisk == Policy.SeverityExplained.Ok)
                        {
                            summary = metricCounts.HighestRiskText;
                            details += DisplayNoFindings;
                            detailsRtf += DisplayRtfNoFindings;
                        }
                        else
                        {
                            if (m_serverInstance == null && metricCounts.RiskCount > 0)
                            {
                                summary =
                                    string.Format(PolicySummaryFormat,
                                                  metricCounts.RiskCount.ToString(NumericFormat),
                                                  metricCounts.HighestRiskText);
                            }
                            else
                            {
                                summary = metricCounts.HighestRiskText;
                            }
                        }

                        newRow[colSummary] = summary;
                        newRow[colResult] = details;
                        newRow[colResultRTF] = detailsRtf + DetailsRtfEnd;
                        m_ReportCardTable.Rows.Add(newRow);
                        m_Categories.TryGetValue(metrictype, out categoryCounts);
                        categoryCounts.IncrementCount(metricCounts.HighestRisk);
                        categoryCounts.Metrics.Add((int)newRow[colMetricId], metricCounts);
                    }

                    //fix the counts for the All category and set the tab images
                    RiskCounts allCounts;
                    m_Categories.TryGetValue(AllChecks, out allCounts);
                    
                    foreach (KeyValuePair<string, RiskCounts> category in m_Categories)
                    {
                        allCounts.AddCounts(category.Value);

                        _ultraTabControl_ReportCard.Tabs[category.Key].Appearance.Image =
                            category.Value.HighestRiskImage;
                    }

                    _ultraTabControl_ReportCard.Tabs[AllChecks].Appearance.Image =
                        allCounts.HighestRiskImage;

                    _grid_ReportCard.SuspendLayout();

                    filterByType(_ultraTabControl_ReportCard.SelectedTab.Text, selectedMetric);

                    if (!m_Initialized)
                    {
                        int size = 0;
                        foreach (UltraGridColumn col in _grid_ReportCard.DisplayLayout.Bands[0].Columns)
                        {
                            if (col.Key != colSeverityCode)
                            {
                                col.PerformAutoResize(PerformAutoSizeType.AllRowsInBand, true);
                            }
                            if (col.Hidden == false)
                            {
                                size += col.Width;
                            }
                        }
                        if (m_policy.IsPolicy)
                        {
                            if (size < _grid_ReportCard.Width)
                            {
                                _grid_ReportCard.DisplayLayout.Bands[0].Columns[colMetricName].Width +=
                                    _grid_ReportCard.Width - size;
                            }
                        }
                        else if (m_policy.IsAssessment)
                        {
                            if (size < _grid_ReportCard.Width)
                            {
                                _grid_ReportCard.DisplayLayout.Bands[0].Columns[colNotes].Width += _grid_ReportCard.Width - size;
                            }
                            else if (_grid_ReportCard.DisplayLayout.Bands[0].Columns[colNotes].Width > _grid_ReportCard.Width / 2)
                            {
                                _grid_ReportCard.DisplayLayout.Bands[0].Columns[colNotes].Width = _grid_ReportCard.Width / 2;
                            }
                        }

                        _grid_ReportCard.DisplayLayout.Bands[0].SortedColumns.Add(colSeverityCode, true, false);
                        _grid_ReportCard.DisplayLayout.Bands[0].SortedColumns.Add(colMetricName, false, false);

                        m_Initialized = true;
                    }

                    _grid_ReportCard.ResumeLayout();
                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error(@"Error - Unable to retrieve policy report card", ex);
                MsgBox.ShowError(string.Format(ErrorMsgs.CantGetPolicyInfoMsg, "Policy Report Card"),
                                 ErrorMsgs.ErrorProcessPolicyInfo,
                                 ex);
                initDataSources();

                _grid_ReportCard.ResumeLayout();
            }

            _ultraTabControl_ReportCard.ResumeLayout();
            _ultraTabControl_ReportCard.EndUpdate();
        }

        private void filterByType(string metrictype, int selectedMetric)
        {
            string filter = string.Empty;

            if (metrictype != AllChecks)
            {
                filter = string.Format(TypeFilter,
                                        colMetricType,
                                        metrictype);
            }

            DataView dv = new DataView(m_ReportCardTable);
            dv.RowFilter = filter;

            // Save the user configuration of the grid to restore it after setting the datasource again
            Utility.GridSettings gridSettings = GridSettings.GetSettings(_grid_ReportCard);

            _grid_ReportCard.SetDataBinding(dv, null);

            // Reapply the user's settings after rebuilding the grid if not initializing
            if (m_Initialized)
            {
                GridSettings.ApplySettingsToGrid(gridSettings, _grid_ReportCard, true);
            }

            RiskCounts category;
            if (m_Categories.TryGetValue(metrictype, out category))
            {
                _label_ReportCard.Text =
                    string.Format(HeaderDisplayLong,
                                    dv.Count,
                                    dv.Count == 1 ? string.Empty : "s",
                                    category.RiskCount,
                                    category.RiskCount == 1 ? string.Empty : "s",
                                    category.AllRisksText.Length == 0 ? string.Empty : string.Format("({0})", category.AllRisksText));
            }

            foreach (UltraGridRow row in _grid_ReportCard.DisplayLayout.Rows.GetAllNonGroupByRows())
            {
                if ((int)((DataRowView)row.ListObject)[colMetricId] == selectedMetric)
                {
                    row.Selected = true;
                    row.Activate();
                    if (m_MetricChangedDelegate != null)
                    {
                        m_MetricChangedDelegate(selectedMetric);
                    }
                    break;
                }
            }
            
            if (_grid_ReportCard.Rows.Count > 0 && _grid_ReportCard.Selected.Rows.Count == 0)
            {
                foreach (UltraGridRow row in _grid_ReportCard.DisplayLayout.Rows)
                {
                    if (row.IsDataRow)
                    {
                        row.Selected = true;
                        row.Activate();

                        if (m_MetricChangedDelegate != null)
                        {
                            m_MetricChangedDelegate((int)((DataRowView)row.ListObject)[colMetricId]);
                        }
                        break;
                    }
                }
            }

            if (m_metricColorDict != null)
            {
                Color color;
                foreach (UltraGridRow row in _grid_ReportCard.DisplayLayout.Rows.GetAllNonGroupByRows())
                {
                    if (m_metricColorDict.TryGetValue((int)row.Cells[colMetricId].Value, out color))
                    {
                        row.Appearance.BackColor = color;
                    }
                }
            }

        }

        private static string getRtfString(string value)
        {
            return value.Replace(@"\", @"\\").Replace("{", @"\{").Replace("}", @"\}");
        }

        #region Grid

        protected void printGrid(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            Debug.Assert(grid.Tag.GetType() == typeof(ToolStripLabel));

            // Associate the print document with the grid & preview dialog here
            // in case we want to change documents for each grid
            _ultraGridPrintDocument.Grid = grid;
            _ultraGridPrintDocument.DefaultPageSettings.Color = false;
            _ultraGridPrintDocument.DocumentName = ErrorMsgs.UserPermissionsCaption;
            
            if (m_policy != null)
            {
                _ultraGridPrintDocument.Header.TextLeft =
                    string.Format(PrintHeaderDisplay,
                                        m_policy.PolicyName,
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

            Forms.Form_GridColumnChooser.Process(grid, gridHeading);
        }

        protected void toggleGridGroupByBox(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            grid.DisplayLayout.GroupByBox.Hidden = !grid.DisplayLayout.GroupByBox.Hidden;
        }

        #endregion

        public void RegisterMetricChangedDelegate(MetricChanged value)
        {
            m_MetricChangedDelegate += value;
        }

        #endregion

        #region Events

        #region tool strips

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

        #region Context Menu events

        private void _contextMenuStrip_Metrics_Opening(object sender, CancelEventArgs e)
        {
            // Enable/disable based on the node type.
            _cmsi_Metrics_ConfigureSecurityCheck.Enabled = Program.gController.isAdmin && m_policy != null && !m_policy.IsApprovedAssessment && _grid_ReportCard.Rows.Count > 0 && _grid_ReportCard.Selected.Rows.Count > 0;
            _cmsi_Metrics_EditExplanationNotes.Enabled = Program.gController.isAdmin && m_policy != null && !m_policy.IsApprovedAssessment && _grid_ReportCard.Rows.Count > 0 && _grid_ReportCard.Selected.Rows.Count > 0;
            _cmsi_Metrics_EditExplanationNotes.Visible = m_policy.IsAssessment;

            _cmsi_Metrics_Separator1.Visible =
                _cmsi_Metrics_ColumnChooser.Visible =
                _cmsi_Metrics_viewGroupByBox.Visible =
                _cmsi_Metrics_Separator2.Visible =
                _cmsi_Metrics_Print.Visible =
                _cmsi_Metrics_Save.Visible = (((ContextMenuStrip)sender).SourceControl is UltraGrid);
        }

        private void _cmsi_Metrics_ConfigureSecurityCheck_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (m_policy != null)
            {
                if (_grid_ReportCard.Rows.Count > 0 && _grid_ReportCard.Selected.Rows.Count > 0)
                {
                    int selectedMetric = (int) _grid_ReportCard.Selected.Rows[0].Cells[colMetricId].Value;

                    // returns true if updated
                    if (Forms.Form_PolicyProperties.Process(m_policy.PolicyId,
                                                            m_policy.AssessmentId,
                                                            Program.gController.isAdmin,
                                                            Forms.Form_PolicyProperties.RequestedOperation.
                                                                ConfigureMetrics,
                                                            selectedMetric))
                    {
                        Program.gController.SignalRefreshPoliciesEvent(m_policy.PolicyId);
                    }
                }
            }

            Cursor = Cursors.Default;
        }

        private void _cmsi_Metrics_EditExplanationNotes_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (m_policy != null)
            {
                if (_grid_ReportCard.Rows.Count > 0 && _grid_ReportCard.Selected.Rows.Count > 0)
                {
                    int selectedMetric = (int)_grid_ReportCard.Selected.Rows[0].Cells[colMetricId].Value;

                    // returns true if updated
                    if (Forms.Form_ExplanationNotes.Process(m_policy.PolicyId,
                                                            m_policy.AssessmentId,
                                                            selectedMetric,
                                                            m_serverInstance == null ? null : m_serverInstance.ConnectionName))
                    {
                        Program.gController.SignalRefreshPoliciesEvent(m_policy.PolicyId);
                    }
                }
            }

            Cursor = Cursors.Default;
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

        #endregion

        #region Grids

        private void _grid_ReportCard_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            e.Layout.Override.GroupByRowExpansionStyle = GroupByRowExpansionStyle.Disabled;
            e.Layout.Override.GroupByRowInitialExpansionState = GroupByRowInitialExpansionState.Expanded;
            e.Layout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.ColumnChooserEnabled = DefaultableBoolean.False;

            UltraGridBand band = e.Layout.Bands[0];

            band.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            band.Columns[colMetricId].Hidden = true;
            band.Columns[colMetricId].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            band.Columns[colMetricName].Header.Caption = "Security Check";
            band.Columns[colMetricName].MinWidth = 100;

            band.Columns[colReportKey].Header.Caption = "Cross Ref";
            band.Columns[colReportKey].Hidden = true;

            band.Columns[colReportText].Header.Caption = "Report Text";
            band.Columns[colReportText].Hidden = true;

            band.Columns[colMetricDescription].Header.Caption = "Description";
            band.Columns[colMetricDescription].Hidden = true;

            band.Columns[colMetricType].Header.Caption = "Category";
            band.Columns[colMetricType].Hidden = true;

            band.Columns[colSeverityCode].Header.Caption = "Risk";
            band.Columns[colSeverityCode].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            band.Columns[colSeverityCode].ValueList = e.Layout.ValueLists[valueListSeverity];
            band.Columns[colSeverityCode].Width = 40;
            band.Columns[colSeverityCode].MinWidth = 40;
            band.Columns[colSeverityCode].MaxWidth = 40;
            band.Columns[colSeverityCode].LockedWidth = true;
            EditorWithText textEditor = new EditorWithText();
            band.Columns[colSeverityCode].Editor = textEditor;

            band.Columns[colSummary].Header.Caption = "Findings";

            // This column is for exporting the details. It remains hidden, but is always exported.
            band.Columns[colResult].Header.Caption = "Details";
            band.Columns[colResult].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            band.Columns[colResult].Hidden = true;

            band.Columns[colResultRTF].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            band.Columns[colResultRTF].Hidden = true;

            if (m_policy == null || m_policy.IsAssessment)
            {
                band.Columns[colIsExplained].Header.Caption = "Explained";
                band.Columns[colIsExplained].Width = 55;
                band.Columns[colIsExplained].MinWidth = 55;
                band.Columns[colIsExplained].MaxWidth = 55;
                band.Columns[colIsExplained].LockedWidth = true;
                band.Columns[colIsExplained].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
                band.Columns[colIsExplained].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                band.Columns[colIsExplained].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                band.Columns[colIsExplained].Hidden = true;

                band.Columns[colNotes].Header.Caption = "Explanation Notes";
                band.Columns[colNotes].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                band.Columns[colNotes].Hidden = true;
            }
            else
            {
                band.Columns[colIsExplained].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                band.Columns[colIsExplained].Hidden = true;

                band.Columns[colNotes].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                band.Columns[colNotes].Hidden = true;
            }
        }

        private void _grid_ReportCard_InitializeGroupByRow(object sender, InitializeGroupByRowEventArgs e)
        {
            UltraGrid grid = (UltraGrid) sender;
            UltraGridColumn col = e.Row.Column;

            if (col.ValueList != null)
            {
                if (col.ValueList.ShouldDisplayImage)
                {
                    e.Row.Appearance.Image = grid.DisplayLayout.ValueLists[((ValueList)col.ValueList).Key].FindByDataValue((int)e.Row.Value).Appearance.Image;
                }
                e.Row.Description = string.Format("{0} : {1} ({2} item{3})",
                                                    col.Header.Caption,
                                                    grid.DisplayLayout.ValueLists[((ValueList)col.ValueList).Key].FindByDataValue((int)e.Row.Value).DisplayText,
                                                    e.Row.Rows.Count,
                                                    e.Row.Rows.Count == 1 ? string.Empty : "s"
                                                    );
            }
        }

        private void _grid_ReportCard_AfterRowActivate(object sender, EventArgs e)
        {
            UltraGrid grid = (UltraGrid)sender;
 
            UltraGridRow currentRow = null;

            if (grid.ActiveRow.IsDataRow)
            {
                currentRow = grid.ActiveRow;
            }
            else if (grid.Selected.Rows.Count > 0)
            {
                currentRow = grid.Selected.Rows[0];
            }

            m_ExplanationTable.Rows.Clear();

            if (currentRow != null)
            {
                // Update the details area
                _richTextBox_Details.Rtf = currentRow.Cells[colResultRTF].Text;

                // Update the explanation notes
                string expression = string.Format("{0} = {1}", colMetricId, (int)((DataRowView)currentRow.ListObject)[colMetricId]);
                DataRow[] foundRows = m_AssessmentTable.Select(expression);
                int riskCount = 0;
                int explainedCount = 0;
                foreach (DataRow row in foundRows)
                {
                    if ((int) row[colSeverityCode] > 0)
                    {
                        riskCount++;
                    }
                    if ((bool) row[colIsExplained])
                    {
                        explainedCount++;
                    }

                    DataRow newRow = m_ExplanationTable.NewRow();
                    newRow[colConnection] = row[colConnection];
                    int severityCode = (int) row[colSeverityCode];
                    severityCode += severityCode > 0 && severityCode < 10 ? 10 : 0;
                    newRow[colSeverityCode] = severityCode;
                    newRow[colIsExplained] = row[colIsExplained];
                    newRow[colNotes] = row[colNotes];

                    m_ExplanationTable.Rows.Add(newRow);
                }

                m_ExplanationTable.DefaultView.Sort = colConnection;
                _grid_ExplanationNotes.SetDataBinding(m_ExplanationTable.DefaultView, null);

                _label_ExplanationNotes.Text =
                    string.Format(HeaderDisplayExplanationNotes,
                        foundRows.Length,
                        foundRows.Length == 1 ? string.Empty : "s",
                        riskCount,
                        riskCount == 1 ? string.Empty : "s",
                        explainedCount);

                if (m_MetricChangedDelegate != null)
                {
                    m_MetricChangedDelegate((int)((DataRowView)currentRow.ListObject)[colMetricId]);
                }
            }
            else
            {
                _richTextBox_Details.Rtf = string.Empty;
                _grid_ExplanationNotes.SetDataBinding(m_ExplanationTable.DefaultView, null);
            }
        }

        private void _grid_ReportCard_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            _cmsi_Metrics_ConfigureSecurityCheck_Click(sender, new EventArgs());
        }

        private void _grid_ExplanationNotes_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            e.Layout.Override.RowSizing = RowSizing.AutoFree;
            e.Layout.Override.CellAppearance.TextVAlign = VAlign.Top;
            e.Layout.Override.CellAppearance.ImageVAlign = VAlign.Top;

            UltraGridBand band = e.Layout.Bands[0];

            band.Columns[colConnection].Width = 130;
            band.Columns[colConnection].Header.Caption = "Server";
            band.Columns[colConnection].AutoSizeMode = ColumnAutoSizeMode.VisibleRows;
            band.Columns[colConnection].CellAppearance.ImageVAlign = VAlign.Top;

            band.Columns[colSeverityCode].Header.Caption = @"Risk";
            band.Columns[colSeverityCode].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            band.Columns[colSeverityCode].ValueList = e.Layout.ValueLists[valueListSeverity];
            band.Columns[colSeverityCode].Width = 40;
            band.Columns[colSeverityCode].MinWidth = 40;
            band.Columns[colSeverityCode].MaxWidth = 40;
            band.Columns[colSeverityCode].LockedWidth = true;
            EditorWithText textEditor = new EditorWithText();
            band.Columns[colSeverityCode].Editor = textEditor;

            band.Columns[colIsExplained].Width = 60;
            band.Columns[colIsExplained].MaxWidth = 60;
            band.Columns[colIsExplained].Header.Caption = "Explained";
            band.Columns[colIsExplained].AutoSizeMode = ColumnAutoSizeMode.VisibleRows;
            band.Columns[colIsExplained].CellAppearance.ImageVAlign = VAlign.Top;

            band.Columns[colNotes].Width = 200;
            band.Columns[colNotes].Header.Caption = "Notes";
            band.Columns[colNotes].AutoSizeMode = ColumnAutoSizeMode.VisibleRows;
            band.Columns[colNotes].CellMultiLine = DefaultableBoolean.True;
            band.Columns[colNotes].CellAppearance.ImageVAlign = VAlign.Top;
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
                        m_gridCellClicked = true;
                        grid.Selected.Rows.Clear();
                        cell.Row.Selected = true;
                        grid.ActiveRow = cell.Row;
                        if (grid == _grid_ReportCard && m_MetricChangedDelegate != null)
                        {
                            m_MetricChangedDelegate((int) ((DataRowView) grid.ActiveRow.ListObject)[colMetricId]);
                        }
                    }
                }
                else
                {
                    m_gridCellClicked = false;
                    Infragistics.Win.UltraWinGrid.HeaderUIElement he = elementUnderMouse.GetAncestor(typeof(Infragistics.Win.UltraWinGrid.HeaderUIElement)) as Infragistics.Win.UltraWinGrid.HeaderUIElement;
                    Infragistics.Win.UltraWinGrid.ColScrollbarUIElement ce = elementUnderMouse.GetAncestor(typeof(Infragistics.Win.UltraWinGrid.ColScrollbarUIElement)) as Infragistics.Win.UltraWinGrid.ColScrollbarUIElement;
                    Infragistics.Win.UltraWinGrid.RowScrollbarUIElement re = elementUnderMouse.GetAncestor(typeof(Infragistics.Win.UltraWinGrid.RowScrollbarUIElement)) as Infragistics.Win.UltraWinGrid.RowScrollbarUIElement;

                    if (he == null && ce == null && re == null)
                    {
                        grid.Selected.Rows.Clear();
                        grid.ActiveRow = null;

                        if (grid == _grid_ReportCard)
                        {
                            _richTextBox_Details.Rtf = string.Empty;
                            m_ExplanationTable = createDataSourceExplanations();
                            _grid_ExplanationNotes.SetDataBinding(m_ExplanationTable, null);

                            if (m_MetricChangedDelegate != null)
                            {
                                m_MetricChangedDelegate(null);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Splitter Handlers

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

        #region Tabs

        private void _ultraTabControl_ReportCard_SelectedTabChanged(object sender, SelectedTabChangedEventArgs e)
        {
            _grid_ReportCard.SuspendLayout();

            filterByType(e.Tab.Text, 0);

            _grid_ReportCard.ResumeLayout();

        }

        #endregion

        private void ReportCard_Enter(object sender, EventArgs e)
        {
            setMenuConfiguration();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (this.IsHandleCreated)
            {   // repost to continue propagation
                this.BeginInvoke((MethodInvoker)delegate { base.OnSizeChanged(e); });
                return;
            }
            base.OnSizeChanged(e);
        }

        #endregion
    }
}