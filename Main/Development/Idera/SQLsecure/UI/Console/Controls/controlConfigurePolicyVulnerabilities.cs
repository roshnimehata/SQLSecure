using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Forms;
using Idera.SQLsecure.UI.Console.Sql;
using Idera.SQLsecure.UI.Console.Utility;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Policy = Idera.SQLsecure.UI.Console.Sql.Policy;

namespace Idera.SQLsecure.UI.Console.Controls
{
    public partial class controlConfigurePolicyVulnerabilities : UserControl
    {
        #region fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Controls.controlConfigurePolicyVulnerabilities");
        private Policy m_policy;
        List<PolicyMetric> m_metrics = null;
        private bool m_InternalUpdate = false;
        private bool m_importing = false;

        private bool m_allowEdit = true;

        #endregion

        #region Queries, Columns & Constants

        private const string valueListSeverity = @"Severity";
        private const string valueListEnabled = @"Enabled";

        // Columns for handling the grid and policymetric results
        private const string colIsEnabled = @"IsEnabled";
        private const string colIsMultiSelect = @"IsMultiSelect";
        private const string colIsUserEntered = @"IsUserEntered";
        private const string colMetricDescription = @"MetricDescription";
        private const string colMetricName = @"MetricName";
        private const string colMetricType = @"MetricType";
        private const string colReportKey = @"ReportKey";
        private const string colReportText = @"ReportText";
        private const string colSeverity = @"Severity";
        private const string colSeverityValues = @"SeverityValues";
        private const string colValidValues = @"ValidValues";
        private const string colValueDescription = @"ValueDescription";

        private const string HeaderDisplay = "Security Checks ({0} enabled)";
        private const string PrintTitle = @"Policy Security Checks";
        private const string PrintHeaderDisplay = "Security Checks for '{0}' as of {1}";

        #endregion

        #region ctors

        public controlConfigurePolicyVulnerabilities()
        {
            InitializeComponent();

            // Hide all radio buttons for single selection group box
            radioButton1.Visible = false;
            radioButton2.Visible = false;
            radioButton3.Visible = false;
            radioButton4.Visible = false;
            radioButton5.Visible = false;
            radioButton6.Visible = false;
            radioButton7.Visible = false;
            radioButton8.Visible = false;

            // Hide single selectin group box
            groupBox_TriggerSingle.Visible = false;

            // Hide all checkboxes for multiple selection group box
            checkBox1.Visible = false;
            checkBox2.Visible = false;
            checkBox3.Visible = false;
            checkBox4.Visible = false;
            checkBox5.Visible = false;
            checkBox6.Visible = false;
            checkBox7.Visible = false;
            checkBox8.Visible = false;

            // Hide multiple selection group box
            groupBox_CriteriaMultiple.Visible = false;

            // Hide User Entered Multiple Selection group box
            groupBox_CriteriaUserEnterMultiple.Visible = false;

            // Hide User Entered Single Selection group box
            groupBox_CriteriaUserEnterSingle.Visible = false;

            // Hide Enabled Disabled Only group box
            groupBox_TriggerDisabledEnabledOnly.Visible = false;

            radioButton_SeverityCritical.Text =
                Utility.DescriptionHelper.GetEnumDescription(Utility.Policy.Severity.High);
            radioButton_SeverityMedium.Text =
                            Utility.DescriptionHelper.GetEnumDescription(Utility.Policy.Severity.Medium);
            radioButton_SeverityLow.Text =
                            Utility.DescriptionHelper.GetEnumDescription(Utility.Policy.Severity.Low);

            _toolStripButton_ColumnChooser.Image = AppIcons.AppImage16(AppIcons.Enum.GridFieldChooser);
            _toolStripButton_GroupBy.Image = AppIcons.AppImage16(AppIcons.Enum.GridGroupBy);
            _toolStripButton_Save.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);
            _toolStripButton_Print.Image = AppIcons.AppImage16(AppIcons.Enum.Print);

            // load value lists for grid display
            ValueListItem listItem;
            ValueList severityValueList = new ValueList();
            severityValueList.Key = valueListSeverity;
            severityValueList.DisplayStyle = ValueListDisplayStyle.DisplayText;
            ultraGridPolicyMetrics.DisplayLayout.ValueLists.Add(severityValueList);
            severityValueList.ValueListItems.Clear();
            listItem = new ValueListItem(Utility.Policy.Severity.Ok, DescriptionHelper.GetEnumDescription(Utility.Policy.Severity.Ok));
            listItem.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.check_16;
            listItem.Appearance.ImageVAlign = VAlign.Top;
            severityValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(Utility.Policy.Severity.Low, DescriptionHelper.GetEnumDescription(Utility.Policy.Severity.Low));
            listItem.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.LowRisk_16;
            listItem.Appearance.ImageVAlign = VAlign.Top;
            severityValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(Utility.Policy.Severity.Medium, DescriptionHelper.GetEnumDescription(Utility.Policy.Severity.Medium));
            listItem.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.MediumRisk_16;
            listItem.Appearance.ImageVAlign = VAlign.Top;
            severityValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(Utility.Policy.Severity.High, DescriptionHelper.GetEnumDescription(Utility.Policy.Severity.High));
            listItem.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.HighRisk_16;
            listItem.Appearance.ImageVAlign = VAlign.Top;
            severityValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(Utility.Policy.Severity.Undetermined, DescriptionHelper.GetEnumDescription(Utility.Policy.Severity.Undetermined));
            listItem.Appearance.Image = AppIcons.AppImage16(AppIcons.Enum.Unknown);
            listItem.Appearance.ImageVAlign = VAlign.Top;
            severityValueList.ValueListItems.Add(listItem);

            ValueList enabledValueList = new ValueList();
            enabledValueList.Key = valueListEnabled;
            enabledValueList.DisplayStyle = ValueListDisplayStyle.DisplayText;
            ultraGridPolicyMetrics.DisplayLayout.ValueLists.Add(enabledValueList);
            listItem = new ValueListItem(true, "Yes");
            enabledValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(false, "No");
            enabledValueList.ValueListItems.Add(listItem);
        }

        #endregion

        #region properties

        public int NumSecurityChecks
        {           
            get
            {
                int count = 0;
                foreach (PolicyMetric pm in m_metrics)
                {
                    if(pm.IsEnabled)
                    {
                        count++;
                    }
                }
                return count;                
            }
        }

        public int NumHighSecurityChecks
        {
            get
            {
                int count = 0;
                foreach (PolicyMetric pm in m_metrics)
                {
                    if (pm.IsEnabled && pm.Severity == 3)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        public int NumMediumSecurityChecks
        {
            get
            {
                int count = 0;
                foreach (PolicyMetric pm in m_metrics)
                {
                    if (pm.IsEnabled && pm.Severity == 2)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        public int NumLowSecurityChecks
        {
            get
            {
                int count = 0;
                foreach (PolicyMetric pm in m_metrics)
                {
                    if (pm.IsEnabled && pm.Severity == 1)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        #endregion

        #region methods

        public void InitializeControl(Policy policy)
        {
            m_policy = policy;
            m_importing = m_policy.PolicyId == 0;
            button_Remove.Enabled = false;
            checkBox_GroupByCategories.Checked = true;

            loadPolicyMetrics();            
        }

        public void InitializeControl(Policy policy, int metricId, bool allowEdit)
        {
            m_policy = policy;
            m_importing = m_policy.PolicyId == 0;
            button_Remove.Enabled = false;
            checkBox_GroupByCategories.Checked = true;

            loadPolicyMetrics();

            if (metricId > 0)
            {
                UltraGridRow[] rows = ultraGridPolicyMetrics.Rows.GetAllNonGroupByRows();
                foreach (UltraGridRow row in rows)
                {
                    if (row.IsDataRow && ((PolicyMetric)row.ListObject).MetricId == metricId)
                    {
                        ultraGridPolicyMetrics.Selected.Rows.Clear();
                        ultraGridPolicyMetrics.Selected.Rows.Add(row);
                        row.Activate();
                        break;
                    }
                }
            }

            m_allowEdit = allowEdit;
            if(!allowEdit)
            {
                button_Import.Enabled =
                    button_Clear.Enabled =
                    button_ResetToDefaults.Enabled =
                    button_Edit.Enabled =
                    button_Remove.Enabled = false;
                textBox_ReportKey.Enabled =
                    textBox_ReportText.Enabled =
                    textBox_UserEnterSingle.Enabled = false;
                radioButton_SeverityCritical.Enabled =
                    radioButton_SeverityMedium.Enabled =
                    radioButton_SeverityLow.Enabled = false;
                checkBox1.Enabled =
                    checkBox2.Enabled =
                    checkBox3.Enabled =
                    checkBox4.Enabled =
                    checkBox5.Enabled =
                    checkBox6.Enabled =
                    checkBox7.Enabled =
                    checkBox8.Enabled = false;
                radioButton1.Enabled =
                    radioButton2.Enabled =
                    radioButton3.Enabled =
                    radioButton4.Enabled =
                    radioButton5.Enabled =
                    radioButton6.Enabled =
                    radioButton7.Enabled =
                    radioButton8.Enabled = false;
                listView_MultiSelect.Enabled = false;
            }
        }

        public bool OKToSave()
        {
            bool ok = true;
            Infragistics.Win.UltraWinGrid.UltraGridRow row = ultraGridPolicyMetrics.ActiveRow;
            if (row != null && row.IsDataRow)
            {
                Infragistics.Win.UltraWinGrid.UltraGridCell cell = row.Cells[colIsEnabled];
                if (cell != null && cell.Value is bool && (bool)cell.Value == true)
                {
                    if (groupBox_CriteriaUserEnterMultiple.Visible)
                    {
                        if (listView_MultiSelect.Items.Count < 1)
                        {
                            ok = false;
                        }
                    }
                    else if (groupBox_CriteriaUserEnterSingle.Visible)
                    {
                        if (string.IsNullOrEmpty(textBox_UserEnterSingle.Text))
                        {
                            ok = false;
                        }
                    }
                    else if(groupBox_CriteriaMultiple.Visible)
                    {
                        bool atLeastOneChecked = false;
                        if(checkBox1.Checked)
                        {
                            atLeastOneChecked = true;                            
                        }
                        else if(checkBox2.Checked)
                        {
                            atLeastOneChecked = true;
                        }
                        else if (checkBox3.Checked)
                        {
                            atLeastOneChecked = true;
                        }
                        else if (checkBox4.Checked)
                        {
                            atLeastOneChecked = true;
                        }
                        else if (checkBox5.Checked)
                        {
                            atLeastOneChecked = true;
                        }
                        else if (checkBox6.Checked)
                        {
                            atLeastOneChecked = true;
                        }
                        else if (checkBox7.Checked)
                        {
                            atLeastOneChecked = true;
                        }
                        else if (checkBox8.Checked)
                        {
                            atLeastOneChecked = true;
                        }
                        if(!atLeastOneChecked)
                        {
                            ok = false;
                        }
                    }
                }
            }
            if(!ok)
            {
                MsgBox.ShowError("Security Checks",
                                 "This security check requires at least one criteria be specified.\n\nEither specify a criteria or disable this security check.");                
            }
            return ok;
        }

        public void SaveMetricChanges(Policy policy)
        {
            if (OKToSave())
            {
                RetrieveValuesFromUI();
                policy.SetPolicyMetrics(m_metrics);
            }
        }

        public bool LeavingControl()
        {
            return RetrieveValuesFromUI();
        }

        #endregion

        #region helpers

        private void loadPolicyMetrics()
        {
            
            //string err_section = "Security Summary Information";
            try
            {
                if (m_importing)
                {
                    m_metrics = m_policy.GetExistingPolicyMetrics(Program.gController.Repository.ConnectionString);
                }
                else
                {
                    m_metrics = m_policy.GetPolicyMetrics(Program.gController.Repository.ConnectionString);
                }

                if (m_metrics != null)
                {
                    m_InternalUpdate = true;

                    //save the current grid settings to restore after importing.
                    GridSettings settings = GridSettings.GetSettings(ultraGridPolicyMetrics);

                    ultraGridPolicyMetrics.DataSource = m_metrics;
                    updateVulnerabilityLayout();
                    ultraGridPolicyMetrics.ActiveRow = null;
                    ultraGridPolicyMetrics.Selected.Rows.Clear();

                    //restore the saved grid settings so it appears the same to the user
                    settings.ApplySettingsToGrid(ultraGridPolicyMetrics);

                    m_InternalUpdate = false;
                    UltraGridRow[] rows = ultraGridPolicyMetrics.Rows.GetAllNonGroupByRows();
                    if (rows.GetLength(0) > 0)
                    {
                        ultraGridPolicyMetrics.Selected.Rows.Add(rows[0]);
                        rows[0].Activate();
                    }                    
                }                
            }
            catch (Exception ex)
            {
                logX.loggerX.Error("Error loading PolicyMetrics", ex);
            }

            UpdateEnabledCount();
        }

        #region Grid functions

        protected void showGridColumnChooser(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            Forms.Form_GridColumnChooser.Process(grid, PrintTitle);
        }

        protected void toggleGridGroupByBox(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            grid.DisplayLayout.GroupByBox.Hidden = !grid.DisplayLayout.GroupByBox.Hidden;
        }

        protected void printGrid(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            // Associate the print document with the grid & preview dialog here
            // for consistency with other forms that require it
            _ultraGridPrintDocument.Grid = grid;
            _ultraGridPrintDocument.DefaultPageSettings.Landscape = true;
            _ultraGridPrintDocument.DefaultPageSettings.Color = false;
            _ultraGridPrintDocument.DocumentName = PrintTitle;
            _ultraGridPrintDocument.FitWidthToPages = 1;
            _ultraGridPrintDocument.Header.TextLeft =
                    string.Format(PrintHeaderDisplay,
                                        m_policy.PolicyAssessmentName,
                                        DateTime.Now.ToShortDateString()
                                    );
            _ultraPrintPreviewDialog.Document = _ultraGridPrintDocument;

            // Call ShowDialog to show the print preview dialog.
            _ultraPrintPreviewDialog.ShowDialog();
        }

        protected void saveGrid(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //fix the enabled checkboxes to export as values
                    grid.DisplayLayout.Bands[0].Columns[colIsEnabled].ValueList = grid.DisplayLayout.ValueLists[valueListEnabled];

                    _ultraGridExcelExporter.Export(grid, _saveFileDialog.FileName);

                    grid.DisplayLayout.Bands[0].Columns[colIsEnabled].ValueList = null;
                }
                catch (Exception ex)
                {
                    Utility.MsgBox.ShowError(Utility.ErrorMsgs.ExportToExcelCaption, Utility.ErrorMsgs.FailedToExportToExcelFile, ex);
                }
            }
        }

        #endregion

        protected void updateVulnerabilityLayout()
        {
            ultraGridPolicyMetrics.DisplayLayout.Bands[0].SortedColumns.Clear();
            if (checkBox_GroupByCategories.Checked)
            {
                ultraGridPolicyMetrics.DisplayLayout.Bands[0].SortedColumns.Add("MetricType", false, true);
                ultraGridPolicyMetrics.DisplayLayout.Bands[0].Override.GroupByRowInitialExpansionState = Infragistics.Win.UltraWinGrid.GroupByRowInitialExpansionState.Expanded;
            }
            ultraGridPolicyMetrics.DisplayLayout.Bands[0].SortedColumns.Add("MetricName", false);
        }


        private bool RetrieveValuesFromUI()
        {
            bool bAllowContinue = true;
            if (Visible && !m_InternalUpdate)
            {
                if(!OKToSave())
                {
                    return false;
                }
                Infragistics.Win.UltraWinGrid.UltraGridRow row = ultraGridPolicyMetrics.ActiveRow;
                if (row != null)
                {
                    if (row.IsDataRow)
                    {
                        bool refreshSort = false;
                        if (row.Cells[colReportKey].Value.ToString() != textBox_ReportKey.Text
                            || (radioButton_SeverityLow.Checked && row.Cells[colSeverity].Value.ToString() != "1")
                            || (radioButton_SeverityMedium.Checked && row.Cells[colSeverity].Value.ToString() != "2")
                            || (radioButton_SeverityCritical.Checked && row.Cells[colSeverity].Value.ToString() != "3"))
                        {
                            refreshSort = true;
                        }

                        row.Cells[colReportText].Value = textBox_ReportText.Text;
                        row.Cells[colReportKey].Value = textBox_ReportKey.Text;

                        if (radioButton_SeverityLow.Checked)
                        {
                            row.Cells[colSeverity].Value = "1";
                        }
                        else if (radioButton_SeverityMedium.Checked)
                        {
                            row.Cells[colSeverity].Value = "2";
                        }
                        else
                        {
                            row.Cells[colSeverity].Value = "3";
                        }

                        StringBuilder values = new StringBuilder();
                        if (groupBox_CriteriaMultiple.Visible)
                        {
                            if (checkBox1.Checked)
                            {
                                values.Append("'");
                                values.Append(checkBox1.Tag);
                                values.Append("'");
                            }
                            if (checkBox2.Checked)
                            {
                                if (values.Length > 0)
                                    values.Append(",");
                                values.Append("'");
                                values.Append(checkBox2.Tag);
                                values.Append("'");
                            }
                            if (checkBox3.Checked)
                            {
                                if (values.Length > 0)
                                    values.Append(",");
                                values.Append("'");
                                values.Append(checkBox3.Tag);
                                values.Append("'");
                            }
                            if (checkBox4.Checked)
                            {
                                if (values.Length > 0)
                                    values.Append(",");
                                values.Append("'");
                                values.Append(checkBox4.Tag);
                                values.Append("'");
                            }
                            if (checkBox5.Checked)
                            {
                                if (values.Length > 0)
                                    values.Append(",");
                                values.Append("'");
                                values.Append(checkBox5.Tag);
                                values.Append("'");
                            }
                            if (checkBox6.Checked)
                            {
                                if (values.Length > 0)
                                    values.Append(",");
                                values.Append("'");
                                values.Append(checkBox6.Tag);
                                values.Append("'");
                            }
                            if (checkBox7.Checked)
                            {
                                if (values.Length > 0)
                                    values.Append(",");
                                values.Append("'");
                                values.Append(checkBox7.Tag);
                                values.Append("'");
                            }
                            if (checkBox8.Checked)
                            {
                                if (values.Length > 0)
                                    values.Append(",");
                                values.Append("'");
                                values.Append(checkBox8.Tag);
                                values.Append("'");
                            }
                        }
                        else if (groupBox_TriggerSingle.Visible)
                        {
                            if (radioButton1.Checked)
                            {
                                values.Append("'");
                                values.Append(radioButton1.Tag);
                                values.Append("'");
                            }
                            if (radioButton2.Checked)
                            {
                                values.Append("'");
                                values.Append(radioButton2.Tag);
                                values.Append("'");
                            }
                            if (radioButton3.Checked)
                            {
                                values.Append("'");
                                values.Append(radioButton3.Tag);
                                values.Append("'");
                            }
                            if (radioButton4.Checked)
                            {
                                values.Append("'");
                                values.Append(radioButton4.Tag);
                                values.Append("'");
                            }
                            if (radioButton5.Checked)
                            {
                                values.Append("'");
                                values.Append(radioButton5.Tag);
                                values.Append("'");
                            }
                            if (radioButton6.Checked)
                            {
                                values.Append("'");
                                values.Append(radioButton6.Tag);
                                values.Append("'");
                            }
                            if (radioButton7.Checked)
                            {
                                values.Append("'");
                                values.Append(radioButton7.Tag);
                                values.Append("'");
                            }
                            if (radioButton8.Checked)
                            {
                                values.Append("'");
                                values.Append(radioButton8.Tag);
                                values.Append("'");
                            }
                        }
                        else if (groupBox_CriteriaUserEnterMultiple.Visible)
                        {
                            foreach (ListViewItem i in listView_MultiSelect.Items)
                            {
                                if (values.Length > 0)
                                    values.Append(",");
                                values.Append("'");
                                values.Append(i.Text);
                                values.Append("'");
                            }
                        }
                        else if (groupBox_CriteriaUserEnterSingle.Visible)
                        {
                            values.Append("'");
                            values.Append(textBox_UserEnterSingle.Text);
                            values.Append("'");
                        }

                        row.Cells[colSeverityValues].Value = values.ToString();

                        if (refreshSort)
                        {
                            ultraGridPolicyMetrics.DisplayLayout.Bands[0].SortedColumns.RefreshSort(true);
                        }

                        UpdateEnabledCount();
                    }
                }
            }

            return bAllowContinue;
        }

        private void UpdateEnabledCount()
        {
            int enabledCount = 0;
            foreach (PolicyMetric metric in m_metrics)
            {
                if (metric.IsEnabled)
                {
                    enabledCount += 1;
                }
            }
            _label_Header.Text = string.Format(HeaderDisplay, enabledCount);
        }

        private int GetRowMetricCount(UltraGridRow row)
        {
            int count = 0;
            if (row.IsDataRow)
            {
                count = 1;
            }

            if (row.IsGroupByRow)
            {
                foreach (UltraGridRow child in ((UltraGridGroupByRow)row).Rows)
                {
                    count += GetRowMetricCount(child);
                }
            }

            return count;
        }

        private void UpdateUIWithMetric()
        {
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in ultraGridPolicyMetrics.Selected.Rows)
            {
                if (row.Cells != null)
                {
                    textBox_Name.Text = row.Cells[colMetricName].Text;
                    textBox_Description.Text = row.Cells[colMetricDescription].Text;
                    textBox_ReportText.Text = row.Cells[colReportText].Text;
                    textBox_ReportKey.Text = row.Cells[colReportKey].Text;

                    switch ((Utility.Policy.Severity)row.Cells[colSeverity].Value)
                    {
                        case Utility.Policy.Severity.Low:
                            radioButton_SeverityLow.Checked = true;
                            break;
                        case Utility.Policy.Severity.Medium:
                            radioButton_SeverityMedium.Checked = true;
                            break;
                        case Utility.Policy.Severity.High:
                            radioButton_SeverityCritical.Checked = true;
                            break;
                    }

                    bool isUserEntered = row.Cells[colIsUserEntered].Text.ToUpper() == "TRUE" ? true : false;
                    bool isMultiSelect = row.Cells[colIsMultiSelect].Text.ToUpper() == "TRUE" ? true : false;

                    if (isUserEntered)
                    {
                        if (isMultiSelect)
                        {
                            groupBox_TriggerDisabledEnabledOnly.Visible = false;
                            groupBox_CriteriaMultiple.Visible = false;
                            groupBox_TriggerSingle.Visible = false;
                            groupBox_CriteriaUserEnterSingle.Visible = false;
                            groupBox_CriteriaUserEnterMultiple.Visible = true;
                            label_ValueDescriptionUEM.Text = row.Cells[colValueDescription].Text;
                            MetricValue metricValue =
                                new MetricValue(row.Cells[colValidValues].Text, row.Cells[colSeverityValues].Text);
                            listView_MultiSelect.Items.Clear();
                            foreach (string s in metricValue.CurrentValues)
                            {
                                if (!string.IsNullOrEmpty(s))
                                {
                                    listView_MultiSelect.Items.Add(s, s, null);
                                }
                            }
                        }
                        else
                        {
                            groupBox_TriggerDisabledEnabledOnly.Visible = false;
                            groupBox_CriteriaMultiple.Visible = false;
                            groupBox_TriggerSingle.Visible = false;
                            groupBox_CriteriaUserEnterMultiple.Visible = false;
                            groupBox_CriteriaUserEnterSingle.Visible = true;
                            label_ValueDescriptionUES.Text = row.Cells[colValueDescription].Text;
                            MetricValue metricValue =
                                new MetricValue(row.Cells[colValidValues].Text, row.Cells[colSeverityValues].Text);
                            if (!string.IsNullOrEmpty(metricValue.CurrentValues[0]))
                            {
                                textBox_UserEnterSingle.Text = metricValue.CurrentValues[0];
                            }
                        }
                    }
                    else
                    {
                        if (isMultiSelect)
                        {
                            groupBox_TriggerDisabledEnabledOnly.Visible = false;
                            groupBox_TriggerSingle.Visible = false;
                            groupBox_CriteriaUserEnterSingle.Visible = false;
                            groupBox_CriteriaUserEnterMultiple.Visible = false;
                            groupBox_CriteriaMultiple.Visible = true;
                            checkBox1.Visible = false;
                            checkBox2.Visible = false;
                            checkBox3.Visible = false;
                            checkBox4.Visible = false;
                            checkBox5.Visible = false;
                            checkBox6.Visible = false;
                            checkBox7.Visible = false;
                            checkBox8.Visible = false;
                            label_ValueDescriptionM.Text = row.Cells[colValueDescription].Text;
                            MetricValue metricValue =
                                new MetricValue(row.Cells[colValidValues].Text, row.Cells[colSeverityValues].Text);
                            int x = 0;
                            foreach (KeyValuePair<string, string> kvp in metricValue.PossibleValues)
                            {
                                x++;
                                switch (x)
                                {
                                    case 1:
                                        checkBox1.Visible = true;
                                        checkBox1.Text = kvp.Value;
                                        checkBox1.Tag = kvp.Key;
                                        if (metricValue.CurrentValues.Contains(kvp.Key))
                                        {
                                            checkBox1.Checked = true;
                                        }
                                        break;
                                    case 2:
                                        checkBox2.Visible = true;
                                        checkBox2.Text = kvp.Value;
                                        checkBox2.Tag = kvp.Key;
                                        if (metricValue.CurrentValues.Contains(kvp.Key))
                                        {
                                            checkBox2.Checked = true;
                                        }
                                        break;
                                    case 3:
                                        checkBox3.Visible = true;
                                        checkBox3.Text = kvp.Value;
                                        checkBox3.Tag = kvp.Key;
                                        if (metricValue.CurrentValues.Contains(kvp.Key))
                                        {
                                            checkBox3.Checked = true;
                                        }
                                        break;
                                    case 4:
                                        checkBox4.Visible = true;
                                        checkBox4.Text = kvp.Value;
                                        checkBox4.Tag = kvp.Key;
                                        if (metricValue.CurrentValues.Contains(kvp.Key))
                                        {
                                            checkBox4.Checked = true;
                                        }
                                        break;
                                    case 5:
                                        checkBox5.Visible = true;
                                        checkBox5.Text = kvp.Value;
                                        checkBox5.Tag = kvp.Key;
                                        if (metricValue.CurrentValues.Contains(kvp.Key))
                                        {
                                            checkBox5.Checked = true;
                                        }
                                        break;
                                    case 6:
                                        checkBox6.Visible = true;
                                        checkBox6.Text = kvp.Value;
                                        checkBox6.Tag = kvp.Key;
                                        if (metricValue.CurrentValues.Contains(kvp.Key))
                                        {
                                            checkBox6.Checked = true;
                                        }
                                        break;
                                    case 7:
                                        checkBox7.Visible = true;
                                        checkBox7.Text = kvp.Value;
                                        checkBox7.Tag = kvp.Key;
                                        if (metricValue.CurrentValues.Contains(kvp.Key))
                                        {
                                            checkBox7.Checked = true;
                                        }
                                        break;
                                    case 8:
                                        checkBox8.Visible = true;
                                        checkBox8.Text = kvp.Value;
                                        checkBox8.Tag = kvp.Key;
                                        if (metricValue.CurrentValues.Contains(kvp.Key))
                                        {
                                            checkBox8.Checked = true;
                                        }
                                        break;
                                }
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(row.Cells[colSeverityValues].Text))
                            {
                                groupBox_TriggerSingle.Visible = false;
                                groupBox_CriteriaMultiple.Visible = false;
                                groupBox_CriteriaUserEnterMultiple.Visible = false;
                                groupBox_CriteriaUserEnterSingle.Visible = false;
                                groupBox_TriggerDisabledEnabledOnly.Visible = true;
                                label_DescriptionEnableDisableOnly.Text = row.Cells[colValueDescription].Text;
                            }
                            else
                            {
                                groupBox_TriggerDisabledEnabledOnly.Visible = false;
                                groupBox_CriteriaMultiple.Visible = false;
                                groupBox_CriteriaUserEnterMultiple.Visible = false;
                                groupBox_CriteriaUserEnterSingle.Visible = false;
                                groupBox_TriggerSingle.Visible = true;
                                radioButton1.Visible = false;
                                radioButton2.Visible = false;
                                radioButton3.Visible = false;
                                radioButton4.Visible = false;
                                radioButton5.Visible = false;
                                radioButton6.Visible = false;
                                radioButton7.Visible = false;
                                radioButton8.Visible = false;
                                label_ValueDescriptionS.Text = row.Cells[colValueDescription].Text;
                                MetricValue metricValue =
                                    new MetricValue(row.Cells[colValidValues].Text, row.Cells[colSeverityValues].Text);
                                int x = 0;
                                foreach (KeyValuePair<string, string> kvp in metricValue.PossibleValues)
                                {
                                    x++;
                                    switch (x)
                                    {
                                        case 1:
                                            radioButton1.Visible = true;
                                            radioButton1.Text = kvp.Value;
                                            radioButton1.Tag = kvp.Key;
                                            if (metricValue.CurrentValues.Contains(kvp.Key))
                                            {
                                                radioButton1.Checked = true;
                                            }
                                            break;
                                        case 2:
                                            radioButton2.Visible = true;
                                            radioButton2.Text = kvp.Value;
                                            radioButton2.Tag = kvp.Key;
                                            if (metricValue.CurrentValues.Contains(kvp.Key))
                                            {
                                                radioButton2.Checked = true;
                                            }
                                            break;
                                        case 3:
                                            radioButton3.Visible = true;
                                            radioButton3.Text = kvp.Value;
                                            radioButton3.Tag = kvp.Key;
                                            if (metricValue.CurrentValues.Contains(kvp.Key))
                                            {
                                                radioButton3.Checked = true;
                                            }
                                            break;
                                        case 4:
                                            radioButton4.Visible = true;
                                            radioButton4.Text = kvp.Value;
                                            radioButton4.Tag = kvp.Key;
                                            if (metricValue.CurrentValues.Contains(kvp.Key))
                                            {
                                                radioButton4.Checked = true;
                                            }
                                            break;
                                        case 5:
                                            radioButton5.Visible = true;
                                            radioButton5.Text = kvp.Value;
                                            radioButton5.Tag = kvp.Key;
                                            if (metricValue.CurrentValues.Contains(kvp.Key))
                                            {
                                                radioButton5.Checked = true;
                                            }
                                            break;
                                        case 6:
                                            radioButton6.Visible = true;
                                            radioButton6.Text = kvp.Value;
                                            radioButton6.Tag = kvp.Key;
                                            break;
                                        case 7:
                                            radioButton7.Visible = true;
                                            radioButton7.Text = kvp.Value;
                                            radioButton7.Tag = kvp.Key;
                                            if (metricValue.CurrentValues.Contains(kvp.Key))
                                            {
                                                radioButton7.Checked = true;
                                            }
                                            break;
                                        case 8:
                                            radioButton8.Visible = true;
                                            radioButton8.Text = kvp.Value;
                                            radioButton8.Tag = kvp.Key;
                                            if (metricValue.CurrentValues.Contains(kvp.Key))
                                            {
                                                radioButton8.Checked = true;
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region events

        #region grid

        private void ultraGridPolicyMetrics_AfterSelectChange(object sender, Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs e)
        {
            UpdateUIWithMetric();
        }

        private void ultraGridPolicyMetrics_BeforeRowDeactivate(object sender, CancelEventArgs e)
        {
            if (!RetrieveValuesFromUI())
            {
                e.Cancel = true;
            }
        }

        private void ultraGridPolicyMetrics_MouseClick(object sender, MouseEventArgs e)
        {
            if (m_allowEdit && e.Button == MouseButtons.Left)
            {
                Infragistics.Win.UIElement selectedElement = ultraGridPolicyMetrics.DisplayLayout.UIElement.ElementFromPoint(new Point(e.X, e.Y));

                if( selectedElement is Infragistics.Win.CheckIndicatorUIElement )
                {
                    Infragistics.Win.UltraWinGrid.UltraGridRow row = selectedElement.SelectableItem as Infragistics.Win.UltraWinGrid.UltraGridRow;
                    if (row != null && row.Cells != null)
                    {
                        Infragistics.Win.UltraWinGrid.UltraGridCell cell = row.Cells[colIsEnabled];
                        if (cell != null && cell.Value is bool)
                        {
                            cell.Value = !(bool) cell.Value;
                            UpdateEnabledCount();
                        }
                    }
                }
            }
        }

        private void ultraGridPolicyMetrics_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            UltraGridBand band = e.Layout.Bands[0];

            band.Columns[colSeverity].ValueList = e.Layout.ValueLists[valueListSeverity];
            EditorWithText textEditor = new EditorWithText();
            band.Columns[colSeverity].Editor = textEditor;

            if (ultraGridPolicyMetrics.Visible)
            {
                ultraGridPolicyMetrics.Focus();
            }
        }

        private void ultraGridPolicyMetrics_InitializeGroupByRow(object sender, Infragistics.Win.UltraWinGrid.InitializeGroupByRowEventArgs e)
        {
            UltraGrid grid = (UltraGrid)sender;
            e.Row.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
            if (e.Row.Column.Key == colMetricType || e.Row.Column.Key == colSeverity || e.Row.Column.Key == colIsEnabled || e.Row.Column.Key == colReportKey)
            {
                //e.Row.ExpansionIndicator = ShowExpansionIndicator.Never;
                int count = GetRowMetricCount(e.Row);
                string descr = e.Row.ValueAsDisplayText;
                if (e.Row.Column.Key == colIsEnabled)
                {
                    descr = (bool)e.Row.Value ? "Enabled" : "Disabled";
                }
                e.Row.Description = descr
                                    + " (" + count + " check" +
                                    (count == 1 ? string.Empty : "s") + ")";
            }
            else
            {
                e.Row.Description = e.Row.ValueAsDisplayText;
            }
        }

        private void ultraGridPolicyMetrics_KeyDown(object sender, KeyEventArgs e)
        {
            if (m_allowEdit && e.KeyCode == Keys.Space)
            {
                if (ultraGridPolicyMetrics.Selected.Rows.Count > 0)
                {
                    Infragistics.Win.UltraWinGrid.UltraGridRow row = ultraGridPolicyMetrics.Selected.Rows[0];
                    if (row != null && row.Cells != null)
                    {
                        Infragistics.Win.UltraWinGrid.UltraGridCell cell = row.Cells[colIsEnabled];
                        if (cell != null && cell.Value is bool)
                        {
                            cell.Value = !(bool)cell.Value;
                            UpdateEnabledCount();
                        }
                    }
                }
            }
        }

        private void ultraGridPolicyMetrics_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            Infragistics.Win.UltraWinGrid.UltraGridRow row = e.Row;
            if (m_allowEdit && row != null && row.Cells != null)
            {
                Infragistics.Win.UltraWinGrid.UltraGridCell cell = row.Cells[colIsEnabled];
                if (cell != null && cell.Value is bool)
                {
                    cell.Value = !(bool)cell.Value;
                    UpdateEnabledCount();
                }
            }
        }

        #endregion

        #region grid support

        private void _toolStripButton_ColumnChooser_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showGridColumnChooser(ultraGridPolicyMetrics);

            Cursor = Cursors.Default;
        }

        private void _toolStripButton_GridGroupBy_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            toggleGridGroupByBox(ultraGridPolicyMetrics);

            Cursor = Cursors.Default;
        }

        private void _toolStripButton_GridPrint_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            printGrid(ultraGridPolicyMetrics);

            Cursor = Cursors.Default;
        }

        private void _toolStripButton_GridSave_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            saveGrid(ultraGridPolicyMetrics);

            Cursor = Cursors.Default;
        }

        #endregion

        #region security check data entry

        private void button_Edit_Click(object sender, EventArgs e)
        {
            // Show Add/Edit dialog
            string[] oldvalues = new string[listView_MultiSelect.Items.Count];
            int i = 0;
            foreach (ListViewItem s in listView_MultiSelect.Items)
            {
                oldvalues[i] = s.Text;
                i++;
            }
            string[] values = Forms.Form_AddMetricValue.Process(label_ValueDescriptionUEM.Text, oldvalues);

            // Clear the list before processing because it will always return all values back
            listView_MultiSelect.Items.Clear();

            foreach(string s in values)
            {
                string temp = s.Trim();

                // make sure we don't add blanks or duplicates
                if (!string.IsNullOrEmpty(temp) && !listView_MultiSelect.Items.ContainsKey(temp))
                {
                    listView_MultiSelect.Items.Add(temp, temp, null);
                }
            }
        }

        private void button_Remove_Click(object sender, EventArgs e)
        {
            foreach(ListViewItem i in listView_MultiSelect.SelectedItems)
            {
                listView_MultiSelect.Items.Remove(i);
            }
        }

        private void listView_MultiSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listView_MultiSelect.SelectedItems.Count > 0)
            {
                button_Remove.Enabled = true;
            }
            else
            {
                button_Remove.Enabled = false;
            }
        }

        private void listView_MultiSelect_Resize(object sender, EventArgs e)
        {
            listView_MultiSelect.Columns[0].Width = listView_MultiSelect.Width - 4;
        }

        #endregion

        private void checkBox_GroupByCategories_CheckedChanged(object sender, EventArgs e)
        {
            updateVulnerabilityLayout();
        }

        #region buttons for all security checks

        private void button_Import_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            string fileName = Forms.Form_ImportPolicy.Process();
            if (!string.IsNullOrEmpty(fileName) )
            {
                Policy policy = new Policy();
                policy.ImportPolicyFromXMLFile(fileName, false);
                policy.IsSystemPolicy = false;
                if (Form_ImportExportPolicySecuriyChecks.ProcessImport(policy, Program.gController.isAdmin))
                {
                m_importing = true;
                loadPolicyMetrics();
                    m_policy.UpdatePolicyMetricsFromSelectedSecurityChecks(policy);
                }
            }

            Cursor = Cursors.Default;
        }

        private void button_ResetToDefaults_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            m_metrics = PolicyMetric.GetPolicyMetrics(Program.gController.Repository.ConnectionString, 0, 0);

            foreach (PolicyMetric m in m_metrics)
            {
                m.SetMetricChanged();
            }

            if (m_metrics != null)
            {
                //save the current grid settings to restore after importing.
                GridSettings settings = GridSettings.GetSettings(ultraGridPolicyMetrics);

                ultraGridPolicyMetrics.DataSource = m_metrics;
                updateVulnerabilityLayout();
                m_InternalUpdate = true;
                ultraGridPolicyMetrics.Selected.Rows.Clear();

                //restore the saved grid settings so it appears the same to the user
                settings.ApplySettingsToGrid(ultraGridPolicyMetrics);
                
                m_InternalUpdate = false;
                UltraGridRow[] rows = ultraGridPolicyMetrics.Rows.GetAllNonGroupByRows();
                if (rows.GetLength(0) > 0)
                {
                    ultraGridPolicyMetrics.Selected.Rows.Add(rows[0]);
                    rows[0].Activate();
                }
            }

            UpdateEnabledCount();

            Cursor = Cursors.Default;
        }

        private void button_Clear_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            ultraGridPolicyMetrics.SuspendLayout();

            foreach(PolicyMetric m in m_metrics)
            {
                if (m.IsEnabled)
                {
                    m.SetMetricChanged();
                }
                m.IsEnabled = false;
            }

            ultraGridPolicyMetrics.ResumeLayout();
            ultraGridPolicyMetrics.Refresh();

            UpdateEnabledCount();

            Cursor = Cursors.Default;
        }

        #endregion

        #endregion
    }

    class MetricValue
    {
        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Controls.controlConfigurePolicyVulnerabilities.MetricValue");

        public MetricValue(string possibleValues, string currentValues)
        {
            logX.loggerX.Verbose(string.Format("Security check possible values: {0}", possibleValues));
            logX.loggerX.Verbose(string.Format("Security check current values: {0}", currentValues));
            string delim = "' ";
            string sep = "','";
            string[] values = possibleValues.Split(',');
            foreach (string s in values)
            {
                string[] t = s.Split(':');
                if (t.GetLength(0) == 2)
                {
                    m_PossibleValues.Add(t[1].Trim(delim.ToCharArray()), t[0].Trim(delim.ToCharArray()));
                }
            }
            
            if (!string.IsNullOrEmpty(currentValues))
            {
                // Process each set of single quoted items.
                StringBuilder sb = new StringBuilder();
                for(int x = 0; x<currentValues.Length; x++)
                {
                    if (currentValues[x] == '\'')
                    {
                        // Are there any more characters?
                        if (currentValues.Length > x + 1)
                        {
                            if (sb.Length == 0)
                            {
                                // First ' of new string
                                x++;
                                if (currentValues.Length > x + 1)
                                {
                                    if (currentValues[x] == '\'' && currentValues[x+1] == '\'')
                                    {
                                        // found Escaped quote add it to string
                                        sb.Append(currentValues[x]);
                                        x++;
                                    }
                                }
                                sb.Append(currentValues[x]);
                            }
                            else
                            {
                                if(currentValues[x+1] == '\'')
                                {
                                  // found Escaped quote add it to string
                                  sb.Append(currentValues[x]);
                                  x++;
                                  sb.Append(currentValues[x]);
                                }
                                else
                                {
                                    if (sb.Length > 0)
                                    {
                                        // found end of quoted string add it to values array
                                        m_CurrentValues.Add(sb.ToString());
                                        sb.Remove(0, sb.Length);
                                    }
                                }
                            }
                        }
                    }
                    else if(sb.Length > 0)
                    {
                        // found character between quotes
                        sb.Append(currentValues[x]);
                    }
                    else if(currentValues[x] != ',')
                    {
                        logX.loggerX.Error(string.Format("Invalid criteria string: {0}", currentValues));
                        m_CurrentValues.Clear();
                        sb.Remove(0, sb.Length);
                        break;
                    }
                }
                if(sb.Length > 0)
                {
                    m_CurrentValues.Add(sb.ToString());
                }               
            }
        }

        public Dictionary<string, string> PossibleValues
        {
            get { return m_PossibleValues; }
        }

        public List<string> CurrentValues
        {
            get { return m_CurrentValues; }
        }

        private Dictionary<string, string> m_PossibleValues = new Dictionary<string, string>();
        private List<string> m_CurrentValues = new List<string>();

    }
}
