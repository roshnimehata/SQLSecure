using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinDataSource;
using Infragistics.Win.UltraWinGrid;


using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Utility;
using Idera.SQLsecure.UI.Console.Controls;
using Idera.SQLsecure.UI.Console.Sql;
using Policy=Idera.SQLsecure.UI.Console.Sql.Policy;

namespace Idera.SQLsecure.UI.Console.Views
{
    public partial class View_ManagePolicies : Idera.SQLsecure.UI.Console.Controls.BaseView, Interfaces.IView
    {
        #region IView

        void Interfaces.IView.SetContext(Interfaces.IDataContext contextIn)
        {
            Title = "Manage Policies"; // Move to constants
            loadPolicyDataSource(true);
        }
        String Interfaces.IView.HelpTopic
        {
            get { return Utility.Help.ManagePoliciesHelpTopic; }
        }
        String Interfaces.IView.ConceptTopic
        {
            get { return Utility.Help.ManagePoliciesConceptTopic; }
        }
        String Interfaces.IView.Title
        {
            get { return Title; }
        }

        #endregion


        #region fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Views.View_ManagePolicies");
        private DataTable _dt_Policies = new DataTable();
        private DataTable _dt_Assessments = new DataTable();
        private bool m_gridCellClicked = false;


        // UI Column Headings
        private const string colTypeIcon = "TypeIcon";
        private const string colHeaderName = "Name";
        private const string colHeaderHiddenPolicyId = "PolicyId";
        private const string colHeaderHiddenAssessmentId = "AssessmentId";
        private const string colHeaderDescription = "Description";
        private const string colHeaderDate = "Date";
        private const string colHeaderState = "State";
        private const string colHeaderStatus = "Status";
        private const string colHeaderConfiguredHigh = "ConfiguredHigh";
        private const string colHeaderConfiguredMedium = "ConfiguredMedium";
        private const string colHeaderConfiguredLow = "ConfiguredLow";
        private const string colHeaderRiskHigh = "RiskHigh";
        private const string colHeaderRiskMedium = "RiskMedium";
        private const string colHeaderRiskLow = "RiskLow";
        private const string colHeaderHasInterview = "HasInderview";
        private const string colHeaderDynamicServerSelection = "DynamicServerSelection";
        private const string colHeaderAssessmentCount = "AssessmentsCount";

        private const string PolicyHeaderText = @"Policies";
        private const string PolicyHeaderDisplay = PolicyHeaderText + " ({0} items)";

        private const string AssessmentsHeaderText = @"Assessments";
        private const string AssessmentsHeaderDisplay = AssessmentsHeaderText + " - {0} ({1} items)";

        private const string PrintHeaderDisplay = "{0} ({1} items)";
        private const string PrintHeaderTimeDisplay = "{0} as of {1}";

        private const string valueListSeverity = @"Severity";

        private const string RiskConfigured = "{0} configured";
        private const string RiskFindings = "{0} of {1}";

        #endregion


        #region Overrides

        protected override void ShowRefresh()
        {
            Cursor = Cursors.WaitCursor;
            this._grid_Policies.BeginUpdate();

            string activeRowName = null;
            if (_grid_Policies.ActiveRow != null && _grid_Policies.ActiveRow.IsDataRow)
            {
                activeRowName = _grid_Policies.ActiveRow.Cells[colHeaderName].Text;
            }

            loadPolicyDataSource(false);

            SetActiveRow(_grid_Policies, activeRowName);

            this._grid_Policies.EndUpdate();

            Cursor = Cursors.Default;
        }

        #endregion


        #region CTOR

        public View_ManagePolicies()
        {
            InitializeComponent();

            _label_Summary.Text = "Manage Policies";

            _toolStripButton_PoliciesColumnChooser.Image =
                _toolStripButton_AuditsColumnChooser.Image = AppIcons.AppImage16(AppIcons.Enum.GridFieldChooser);
            _toolStripButton_PoliciesGroupBy.Image =
                _toolStripButton_AuditsGroupBy.Image = AppIcons.AppImage16(AppIcons.Enum.GridGroupBy);
            _toolStripButton_PoliciesSave.Image =
                _toolStripButton_AuditsSave.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);
            _toolStripButton_PoliciesPrint.Image =
                _toolStripButton_AuditsPrint.Image = AppIcons.AppImage16(AppIcons.Enum.Print);


            _smallTask_CreatePolicy.TaskHandler += new System.EventHandler(this.CreatePolicy);
            _smallTask_EditPolicy.TaskHandler += new System.EventHandler(this.EditPolicy);


            _grid_Assessments.Tag = _label_Assessments;
            _grid_Policies.Tag = _label_Policies;

            _dt_Policies.Columns.Add(colHeaderName, typeof(String));
            _dt_Policies.Columns.Add(colHeaderRiskHigh, typeof(int));
            _dt_Policies.Columns.Add(colHeaderRiskMedium, typeof(int));
            _dt_Policies.Columns.Add(colHeaderRiskLow, typeof(int));
            _dt_Policies.Columns.Add(colHeaderAssessmentCount, typeof(int));
            _dt_Policies.Columns.Add(colHeaderDynamicServerSelection, typeof(bool));
            _dt_Policies.Columns.Add(colHeaderHasInterview, typeof(bool));
            _dt_Policies.Columns.Add(colHeaderDescription, typeof(String));
            _dt_Policies.Columns.Add(colHeaderHiddenPolicyId, typeof(int));
            _dt_Policies.Columns.Add(colHeaderHiddenAssessmentId, typeof(int));

            _dt_Assessments.Columns.Add(colHeaderStatus, typeof(int));
            _dt_Assessments.Columns.Add(colHeaderName, typeof(String));
            _dt_Assessments.Columns.Add(colHeaderConfiguredHigh, typeof(int));
            _dt_Assessments.Columns.Add(colHeaderRiskHigh, typeof(int));
            _dt_Assessments.Columns.Add(colHeaderConfiguredMedium, typeof(int));
            _dt_Assessments.Columns.Add(colHeaderRiskMedium, typeof(int));
            _dt_Assessments.Columns.Add(colHeaderConfiguredLow, typeof(int));
            _dt_Assessments.Columns.Add(colHeaderRiskLow, typeof(int));
            _dt_Assessments.Columns.Add(colHeaderState, typeof(String));
            _dt_Assessments.Columns.Add(colHeaderDate, typeof(String));
            _dt_Assessments.Columns.Add(colHeaderDescription, typeof(String));
            _dt_Assessments.Columns.Add(colHeaderHiddenPolicyId, typeof(int));
            _dt_Assessments.Columns.Add(colHeaderHiddenAssessmentId, typeof(int));


            _grid_Policies.DrawFilter = new Utility.HideFocusRectangleDrawFilter();
            _grid_Assessments.DrawFilter = new Utility.HideFocusRectangleDrawFilter();


            // load value lists for grid display
            ValueListItem listItem;
            ValueList severityValueList = new ValueList();
            severityValueList.Key = valueListSeverity;
            severityValueList.DisplayStyle = ValueListDisplayStyle.Picture;
            _grid_Assessments.DisplayLayout.ValueLists.Add(severityValueList);
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


        }
        #endregion


        #region Helpers

        private void loadPolicyDataSource(bool bUpdate)
        {
            _dt_Policies.Clear();
            try
            {
                // Retrieve policy information.
                Program.gController.Repository.RefreshPolicies();
                Repository.PolicyList policyList = Program.gController.Repository.Policies;

                foreach (Policy p in policyList)
                {
                    DataRow newRow = _dt_Policies.NewRow();

                    newRow[colHeaderName] = p.PolicyName;
                    newRow[colHeaderRiskHigh] = p.MetricCountHigh;
                    newRow[colHeaderRiskMedium] = p.MetricCountMedium;
                    newRow[colHeaderRiskLow] = p.MetricCountLow;
                    int count = 0;
                    foreach (Policy a in p.Assessments)
                    {
                        if (!a.IsCurrentAssessment)
                        {
                            count++;
                        }
                    }
                    newRow[colHeaderAssessmentCount] = count;
                    newRow[colHeaderDynamicServerSelection] = p.IsDynamic;
                    newRow[colHeaderHasInterview] = !string.IsNullOrEmpty(p.InterviewText);
                    newRow[colHeaderDescription] = p.PolicyDescription;
                    newRow[colHeaderHiddenPolicyId] = p.PolicyId;
                    newRow[colHeaderHiddenAssessmentId] = p.AssessmentId;

                    _dt_Policies.Rows.Add(newRow);

                }

            }
            catch (Exception ex)
            {
                logX.loggerX.Error(@"Error - Unable to retrieve policies", ex);
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.CantGetLoginsCaption, ex);
            }

            this._grid_Policies.BeginUpdate();
            this._grid_Policies.DataSource = _dt_Policies;
            _grid_Policies.Selected.Rows.Clear();
            _grid_Policies.Selected.Rows.Add(_grid_Policies.Rows[0]);

            this._grid_Policies.DataMember = "";
            if (bUpdate)
            {
                this._grid_Policies.EndUpdate();
            }

            _label_Policies.Text = string.Format(PolicyHeaderDisplay, _grid_Policies.Rows.Count.ToString());

        }

        private void loadAssessmentsDataSource(int policyId, bool bUpdate)
        {
            _dt_Assessments.Clear();
            try
            {
                // Retrieve policy information.
                Repository.AssessmentList assessments = Program.gController.Repository.Policies.Find(policyId).Assessments;

                foreach (Sql.Policy p in assessments)
                {
                    if(!p.IsAssessment)
                    {
                        continue;
                    }

                    DataRow newRow = _dt_Assessments.NewRow();

                    newRow[colHeaderName] = p.AssessmentName;
                    newRow[colHeaderDate] = p.AssessmentDate.Value.ToLocalTime().ToString(Utility.Constants.DATETIME_FORMAT);
                    newRow[colHeaderConfiguredHigh] = p.MetricCountHigh;
                    newRow[colHeaderConfiguredMedium] = p.MetricCountMedium;
                    newRow[colHeaderConfiguredLow] = p.MetricCountLow;
                    newRow[colHeaderRiskHigh] = p.FindingCountHigh;
                    newRow[colHeaderRiskMedium] = p.FindingCountMedium;
                    newRow[colHeaderRiskLow] = p.FindingCountLow;
                    newRow[colHeaderDescription] = p.AssessmentDescription;
                    newRow[colHeaderHiddenPolicyId] = p.PolicyId; 
                    newRow[colHeaderHiddenAssessmentId] = p.AssessmentId;
                    newRow[colHeaderState] = p.AssessmentStateName;

                    if (p.FindingCountHigh > 0)
                    {
                        newRow[colHeaderStatus] = Utility.Policy.SeverityExplained.High;
                    }
                    else if (p.FindingCountMedium > 0)
                    {
                        newRow[colHeaderStatus] = Utility.Policy.SeverityExplained.Medium;
                    }
                    else if (p.FindingCountLow > 0)
                    {
                        newRow[colHeaderStatus] = Utility.Policy.SeverityExplained.Low;
                    }
                    else if (p.FindingCountHighExplained > 0)
                    {
                        newRow[colHeaderStatus] = Utility.Policy.SeverityExplained.HighExplained;
                    }
                    else if (p.FindingCountMediumExplained > 0)
                    {
                        newRow[colHeaderStatus] = Utility.Policy.SeverityExplained.MediumExplained;
                    }
                    else if (p.FindingCountLowExplained > 0)
                    {
                        newRow[colHeaderStatus] = Utility.Policy.SeverityExplained.LowExplained;
                    }
                    else
                    {
                        newRow[colHeaderStatus] = Utility.Policy.SeverityExplained.Ok;
                    }

                    _dt_Assessments.Rows.Add(newRow);
                }

            }
            catch (Exception ex)
            {
                logX.loggerX.Error(@"Error - Unable to retrieve Assessments", ex);
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.CantGetLoginsCaption, ex);
            }

            this._grid_Assessments.BeginUpdate();
            this._grid_Assessments.DataSource = _dt_Assessments;
            this._grid_Assessments.DataMember = "";
            if (bUpdate)
            {
                this._grid_Assessments.EndUpdate();
            }

            Policy policy = Program.gController.Repository.GetPolicy(policyId);
            _label_Assessments.Text = string.Format(AssessmentsHeaderDisplay, policy.PolicyName, _grid_Assessments.Rows.Count.ToString());

            if(_grid_Assessments.Rows.Count == 0)
            {
                _label_NoAssessments.Visible = true;
                _grid_Assessments.Visible = false;
            }
            else
            {
                _label_NoAssessments.Visible = false;
                _grid_Assessments.Visible = true;
            }

        }



        private void SetActiveRow(Infragistics.Win.UltraWinGrid.UltraGrid grid, string policyName)
        {
            if (!string.IsNullOrEmpty(policyName))
            {
                Infragistics.Win.UltraWinGrid.RowsCollection sr = grid.Rows;
                if (sr.Count > 0)
                {
                    foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in sr)
                    {
                        row.Selected = false;
                        if (row.Cells[colHeaderName].Text == policyName)
                        {
                            row.Selected = true;
                            grid.ActiveRow = row;
                        }
                    }
                }
            }
        }

        #endregion


        #region Small Task Handlers


        private void EditPolicy(int policyId, int assessmentId)
        {
            if (Sql.Policy.IsPolicyRegistered(policyId))
            {
                Forms.Form_PolicyProperties.Process(policyId, assessmentId, Program.gController.isAdmin,
                                                    Forms.Form_PolicyProperties.RequestedOperation.ConfigureMetrics);
            }
            
        }

        private void EditPolicy(object sender, EventArgs e)
        {
            if (_grid_Policies.ActiveRow != null && _grid_Policies.ActiveRow.IsDataRow)
            {
                string policyName = _grid_Policies.ActiveRow.Cells[colHeaderName].Text;

                int policyId = (int)_grid_Policies.ActiveRow.Cells[colHeaderHiddenPolicyId].Value;
                int assessmentId = (int)_grid_Policies.ActiveRow.Cells[colHeaderHiddenAssessmentId].Value;

                EditPolicy(policyId, assessmentId);

                ShowRefresh();
                SetActiveRow(_grid_Policies, policyName);
            }
        }

        private void CreatePolicy(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Forms.Form_WizardCreatePolicy.Process();

            ShowRefresh();

            Cursor = Cursors.Default;

        }

        private void DeletePolicy(object sender, EventArgs e)
        {
            if (_grid_Policies.ActiveRow != null && _grid_Policies.ActiveRow.IsDataRow)
            {
                int policyId = (int) _grid_Policies.ActiveRow.Cells[colHeaderHiddenPolicyId].Value;
                Policy p = Program.gController.Repository.GetPolicy(policyId);

                DeletePolicy(p);

            }
        }

        #endregion

        #region Grid Events

        private void _grid_Policies_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            UltraGridBand band = e.Layout.Bands[0];

            band.Columns[colHeaderName].Width = 200;
            band.Columns[colHeaderName].Header.ToolTipText = "Policy Name";

            band.Columns[colHeaderRiskHigh].Header.Caption = "High Configured";
            band.Columns[colHeaderRiskHigh].Header.ToolTipText = "Number of Configured High Risk Security Checks";
            band.Columns[colHeaderRiskHigh].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[colHeaderRiskMedium].Header.Caption = "Medium Configured";
            band.Columns[colHeaderRiskMedium].Header.ToolTipText = "Number of Configured Medium Risk Security Checks";
            band.Columns[colHeaderRiskMedium].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[colHeaderRiskLow].Header.Caption = "Low Configured";
            band.Columns[colHeaderRiskLow].Header.ToolTipText = "Number of Configured Low Risk Security Checks";
            band.Columns[colHeaderRiskLow].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[colHeaderAssessmentCount].Header.Caption = "Assessments";
            band.Columns[colHeaderAssessmentCount].Header.ToolTipText = "Number of Saved Assessments";
            band.Columns[colHeaderAssessmentCount].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[colHeaderDynamicServerSelection].Header.Caption = "Dynamic";
            band.Columns[colHeaderDynamicServerSelection].Header.ToolTipText = "Does policy use dynamic server selection?";
            band.Columns[colHeaderHasInterview].Width = 60;

            band.Columns[colHeaderHasInterview].Header.Caption = "Review Notes";
            band.Columns[colHeaderHasInterview].Header.ToolTipText = "Does policy contain internal review notes?";
            band.Columns[colHeaderHasInterview].Width = 80;

            band.Columns[colHeaderDescription].Header.ToolTipText = "Policy Description";

            band.Columns[colHeaderHiddenPolicyId].Header.Caption = "Policy ID";
            band.Columns[colHeaderHiddenPolicyId].Header.ToolTipText = "Policy ID used in Repository";
            band.Columns[colHeaderHiddenPolicyId].Hidden = true;

            band.Columns[colHeaderHiddenAssessmentId].Header.Caption = "Assessment ID";
            band.Columns[colHeaderHiddenAssessmentId].Header.ToolTipText = "Assessment ID used in Repository";
            band.Columns[colHeaderHiddenAssessmentId].Hidden = true;
            band.Columns[colHeaderHiddenAssessmentId].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

        }

        private void _grid_Assessments_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            EditorWithText textEditor = new EditorWithText();

            UltraGridBand band = e.Layout.Bands[0];

            band.Columns[colHeaderStatus].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            band.Columns[colHeaderStatus].ValueList = e.Layout.ValueLists[valueListSeverity];
            band.Columns[colHeaderStatus].Width = 40;
            band.Columns[colHeaderStatus].MinWidth = 40;
            band.Columns[colHeaderStatus].MaxWidth = 40;
            band.Columns[colHeaderStatus].LockedWidth = true;
            band.Columns[colHeaderStatus].Editor = textEditor;
            band.Columns[colHeaderStatus].Hidden = true;

            band.Columns[colHeaderName].Width = 200;
            band.Columns[colHeaderName].Header.ToolTipText = "Assessment Name";

            band.Columns[colHeaderState].Width = 70;
            band.Columns[colHeaderState].Header.ToolTipText = "Assessment State: Draft, Published or Approved";

            band.Columns[colHeaderDate].Width = 136;
            band.Columns[colHeaderDate].Header.ToolTipText = "Assessment Date";

            band.Columns[colHeaderConfiguredHigh].Header.Caption = "High Configured";
            band.Columns[colHeaderConfiguredHigh].Header.ToolTipText = "Number of Configured High Risk Security Checks";
            band.Columns[colHeaderConfiguredHigh].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[colHeaderRiskHigh].Header.Caption = string.Empty;
            band.Columns[colHeaderRiskHigh].Header.ToolTipText = "Number of High Risk Findings";
            band.Columns[colHeaderRiskHigh].Header.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.HighRisk_16;
            band.Columns[colHeaderRiskHigh].Header.Appearance.ImageHAlign = HAlign.Center;
            band.Columns[colHeaderRiskHigh].CellAppearance.TextHAlign = HAlign.Right;
            band.Columns[colHeaderRiskHigh].Width = 30;

            band.Columns[colHeaderConfiguredMedium].Header.Caption = "Medium Configured";
            band.Columns[colHeaderConfiguredMedium].Header.ToolTipText = "Number of Configured Medium Risk Security Checks";
            band.Columns[colHeaderConfiguredMedium].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[colHeaderRiskMedium].Header.Caption = string.Empty;
            band.Columns[colHeaderRiskMedium].Header.ToolTipText = "Number of Medium Risk Findings";
            band.Columns[colHeaderRiskMedium].Header.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.MediumRisk_16;
            band.Columns[colHeaderRiskMedium].Header.Appearance.ImageHAlign = HAlign.Center;
            band.Columns[colHeaderRiskMedium].CellAppearance.TextHAlign = HAlign.Right;
            band.Columns[colHeaderRiskMedium].Width = 30;

            band.Columns[colHeaderConfiguredLow].Header.Caption = "Low Configured";
            band.Columns[colHeaderConfiguredLow].Header.ToolTipText = "Number of Configured Low Risk Security Checks";
            band.Columns[colHeaderConfiguredLow].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[colHeaderRiskLow].Header.Caption = string.Empty;
            band.Columns[colHeaderRiskLow].Header.ToolTipText = "Number of Low Risk Findings";
            band.Columns[colHeaderRiskLow].Header.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.LowRisk_16;
            band.Columns[colHeaderRiskLow].Header.Appearance.ImageHAlign = HAlign.Center;
            band.Columns[colHeaderRiskLow].CellAppearance.TextHAlign = HAlign.Right;
            band.Columns[colHeaderRiskLow].Width = 30;

            band.Columns[colHeaderDescription].Header.ToolTipText = "Assessment Description";

            band.Columns[colHeaderHiddenAssessmentId].Header.Caption = "Assessment ID";
            band.Columns[colHeaderHiddenAssessmentId].Header.ToolTipText = "Assessment ID used in Repository";
            band.Columns[colHeaderHiddenAssessmentId].Hidden = true;

            band.Columns[colHeaderHiddenPolicyId].Header.Caption = "Policy ID";
            band.Columns[colHeaderHiddenPolicyId].Header.ToolTipText = "Policy ID used in Repository";
            band.Columns[colHeaderHiddenPolicyId].Hidden = true;
            band.Columns[colHeaderHiddenPolicyId].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

        }

        private void _grid_Policies_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            EditPolicy(null, null);
        }

        private void _grid_Policies_AfterRowActivate(object sender, EventArgs e)
        {
            bool enabled = false;
            if (_grid_Policies.ActiveRow != null && _grid_Policies.ActiveRow.IsDataRow)
            {
                int policyId = (int)_grid_Policies.ActiveRow.Cells[colHeaderHiddenPolicyId].Value;
                Program.gController.Repository.RefreshPolicies();                
                loadAssessmentsDataSource(policyId, true);
                Policy p = Program.gController.Repository.GetPolicy(policyId);
                if(!p.HasAssessments() && !p.IsSystemPolicy)
                {
                    enabled = true;
                }               
            }
            _cmsi_Policies_Delete.Enabled = enabled;
        }

        private void _grid_Assessments_AfterRowActivate(object sender, EventArgs e)
        {
            bool enabled = false;
            if (_grid_Assessments.ActiveRow != null && _grid_Assessments.ActiveRow.IsDataRow)
            {
                int policyId = (int)_grid_Assessments.ActiveRow.Cells[colHeaderHiddenPolicyId].Value;
                Program.gController.Repository.RefreshPolicies();
                int assessmentId = (int)_grid_Assessments.ActiveRow.Cells[colHeaderHiddenAssessmentId].Value;
                Repository.AssessmentList assessments = Program.gController.Repository.Policies.Find(policyId).Assessments;
                Policy p = assessments.Find(assessmentId);
                if (!p.IsApprovedAssessment && !p.IsCurrentAssessment)
                {
                    enabled = true;
                }
            }
            _cmsi_Assessments_Delete.Enabled = enabled;
            _cmsi_Assessments_Configure.Enabled = enabled;
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
                    }
                }
            }
        }

        #endregion

        #region Common Grid Functions

        protected void printGrid(Infragistics.Win.UltraWinGrid.UltraGrid grid, string headerText)
        {
            // Associate the print document with the grid & preview dialog here
            // in case we want to change documents for each grid
            _ultraGridPrintDocument.Grid = grid;
            _ultraGridPrintDocument.DefaultPageSettings.Landscape = true;
            _ultraGridPrintDocument.DefaultPageSettings.Color = false;
            _ultraGridPrintDocument.DocumentName = ErrorMsgs.UserPermissionsCaption;
            //_ultraGridPrintDocument.FitWidthToPages = 2;
            if (grid.Rows.Count > 0)
            {
                _ultraGridPrintDocument.Header.TextLeft =
                    string.Format(PrintHeaderTimeDisplay,
                                   string.Format(PrintHeaderDisplay, headerText, grid.Rows.FilteredInNonGroupByRowCount),
                                   DateTime.Now);
            }
            else
            {
                _ultraGridPrintDocument.Header.TextLeft = headerText;
            }
            _ultraPrintPreviewDialog.Document = _ultraGridPrintDocument;

            // Call ShowDialog to show the print preview dialog.
            _ultraPrintPreviewDialog.ShowDialog();
        }

        protected void saveGrid(Infragistics.Win.UltraWinGrid.UltraGrid grid, string fileName)
        {
            _saveFileDialog.FileName = fileName;
            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _ultraGridExcelExporter.Export(grid, _saveFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MsgBox.ShowError(ErrorMsgs.ExportToExcelCaption, ErrorMsgs.FailedToExportToExcelFile, ex);
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

        private void _toolStripButton_PoliciesSave_Click(object sender, EventArgs e)
        {
            saveGrid(_grid_Policies, "Policies.xls");
        }

        private void _toolStripButton_PoliciesPrint_Click(object sender, EventArgs e)
        {
            printGrid(_grid_Policies, PolicyHeaderText);
        }

        private void _toolStripButton_AssessmentsSave_Click(object sender, EventArgs e)
        {
            saveGrid(_grid_Assessments, "Assessments.xls");
        }

        private void _toolStripButton_AssessmentsPrint_Click(object sender, EventArgs e)
        {
            printGrid(_grid_Assessments, AssessmentsHeaderText);
        }

        private void _toolStripButton_PoliciesColumnChooser_Click(object sender, EventArgs e)
        {
            showGridColumnChooser(_grid_Policies);
        }

        private void _toolStripButton_PoliciesGroupBy_Click(object sender, EventArgs e)
        {
            toggleGridGroupByBox(_grid_Policies);
        }

        private void _toolStripButton_AuditsColumnChooser_Click(object sender, EventArgs e)
        {
            showGridColumnChooser(_grid_Assessments);
        }

        private void _toolStripButton_AuditsGroupBy_Click(object sender, EventArgs e)
        {
            toggleGridGroupByBox(_grid_Assessments);
        }

        private void _cmsi_Assessments_Delete_Click(object sender, EventArgs e)
        {
            if (_grid_Assessments.ActiveRow != null && _grid_Assessments.ActiveRow.IsDataRow)
            {
                string policyName = _grid_Assessments.ActiveRow.Cells[colHeaderName].Text;

                int policyId = (int)_grid_Assessments.ActiveRow.Cells[colHeaderHiddenPolicyId].Value;
                int assessmentId = (int)_grid_Assessments.ActiveRow.Cells[colHeaderHiddenAssessmentId].Value;

                Repository.AssessmentList assessments = Program.gController.Repository.Policies.Find(policyId).Assessments;
                Policy p = assessments.Find(assessmentId);

                DeletePolicy(p);

            }
        }



        private void DeletePolicy(Sql.Policy policy)
        {
            Cursor = Cursors.WaitCursor;

            if (Sql.Policy.IsPolicyRegistered(policy.PolicyId))
            {
                if (policy.IsPolicy)
                {
                    if (!policy.IsSystemPolicy)
                    {
                        // Display confirmation, if user confirms remove the policy.
                        string caption = Utility.ErrorMsgs.RemovePolicyCaption + " - " +
                                         policy.PolicyName;
                        if (Utility.MsgBox.ShowConfirm(caption, Utility.ErrorMsgs.RemovePolicyConfirmMsg) ==
                            DialogResult.Yes)
                        {
                            try
                            {
                                Sql.Policy.RemovePolicy(policy.PolicyId);
                            }
                            catch (Exception ex)
                            {
                                Utility.MsgBox.ShowError(Utility.ErrorMsgs.RemovePolicyCaption,
                                                         string.Format(Utility.ErrorMsgs.CantRemovePolicyMsg,
                                                                       policy.PolicyName),
                                                         ex);
                            }

                            Program.gController.SignalRefreshPoliciesEvent(0);
                        }
                    }
                    else
                    {
                        Utility.MsgBox.ShowError(Utility.ErrorMsgs.RemovePolicyCaption,
                                                 Utility.ErrorMsgs.RemoveSystemPolicyMsg);
                    }
                    loadPolicyDataSource(true);

                }
                else if (policy.IsAssessment)
                {
                    if (!policy.IsApprovedAssessment)
                    {
                        // Display confirmation, if user confirms remove the policy.
                        string caption = Utility.ErrorMsgs.RemoveAssessmentCaption + " - " +
                                         policy.PolicyAssessmentName;
                        if (Utility.MsgBox.ShowConfirm(caption, Utility.ErrorMsgs.RemoveAssessmentConfirmMsg) ==
                            DialogResult.Yes)
                        {
                            try
                            {
                                Sql.Policy.RemoveAssessment(policy.PolicyId, policy.AssessmentId);
                            }
                            catch (Exception ex)
                            {
                                Utility.MsgBox.ShowError(Utility.ErrorMsgs.RemoveAssessmentCaption,
                                                         string.Format(Utility.ErrorMsgs.CantRemoveAssessmentMsg,
                                                                       policy.PolicyAssessmentName),
                                                         ex);
                            }

                            Program.gController.SignalRefreshPoliciesEvent(0);
                        }
                    }
                    else
                    {
                        Utility.MsgBox.ShowError(Utility.ErrorMsgs.RemoveAssessmentCaption,
                                                 Utility.ErrorMsgs.RemoveApprovedAssessmentMsg);
                    }
                    loadAssessmentsDataSource(policy.PolicyId, true);

                }
            }
            else
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.RemovePolicyCaption,
                                         Utility.ErrorMsgs.RemoveSystemPolicyMsg);
                Program.gController.SignalRefreshPoliciesEvent(0);
            }


            Cursor = Cursors.Default;
        }

        private void _cmsi_Assessments_Configure_Click(object sender, EventArgs e)
        {
            if (_grid_Assessments.ActiveRow != null && _grid_Assessments.ActiveRow.IsDataRow)
            {
                string policyName = _grid_Assessments.ActiveRow.Cells[colHeaderName].Text;

                int policyId = (int)_grid_Assessments.ActiveRow.Cells[colHeaderHiddenPolicyId].Value;
                int assessmentId = (int)_grid_Assessments.ActiveRow.Cells[colHeaderHiddenAssessmentId].Value;


                EditPolicy(policyId, assessmentId);
                
                ShowRefresh();

                SetActiveRow(_grid_Assessments, policyName);

            }
        }

        private void _grid_Policies_InitializeRow(object sender, InitializeRowEventArgs e)
        {
//            e.Row.Cells[colHeaderRiskHigh].Text = string.Format(RiskConfigured, e.Row.Cells[colHeaderRiskHigh].Value);            
        }

  

    }


}

