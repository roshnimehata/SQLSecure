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
    public partial class controlConfigureMetricCriteria : UserControl
    {
        #region Queries, Columns & Constants

        private string valueListSeverity = Utility.Constants.POLICY_METRIC_VALUE_LIST_SERVERITY;
        private string valueListEnabled = Utility.Constants.POLICY_METRIC_VALUE_LIST_ENABLED;
        private string colIsMultiSelect = Utility.Constants.POLICY_METRIC_COLUMN_IS_MULTISELECT;
        private string colIsUserEntered = Utility.Constants.POLICY_METRIC_COLUMN_IS_USER_ENTERED;
        private string colValidValues = Utility.Constants.POLICY_METRIC_COLUMN_VALID_VALUES;
        private string colValueDescription = Utility.Constants.POLICY_METRIC_COLUMN_VALUE_DESCRIPTION;

        private string colMetricDescription = Utility.Constants.POLICY_METRIC_COLUMN_METRIC_DESCRIPTION;
        private string colMetricName = Utility.Constants.POLICY_METRIC_COLUMN_METRIC_NAME;
        private string colReportKey = Utility.Constants.POLICY_METRIC_COLUMN_REPORT_KEY;
        private string colReportText = Utility.Constants.POLICY_METRIC_COLUMN_REPORT_TEXT;
        private string colSeverity = Utility.Constants.POLICY_METRIC_COLUMN_SEVERITY;
        private string colSeverityValues = Utility.Constants.POLICY_METRIC_COLUMN_SEVERITY_VALUES;
        
        #endregion
        
        #region ctors

        /// <summary>
        /// 
        /// </summary>
        public controlConfigureMetricCriteria()
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
            groupBox_TriggerSingle.Visible = groupBox_TriggerSingle.Enabled = false;

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
            groupBox_CriteriaMultiple.Visible = groupBox_CriteriaMultiple.Enabled = false;

            // Hide User Entered Multiple Selection group box
            groupBox_CriteriaUserEnterMultiple.Visible = groupBox_CriteriaUserEnterMultiple.Enabled = false;

            // Hide User Entered Single Selection group box
            groupBox_CriteriaUserEnterSingle.Visible = groupBox_CriteriaUserEnterSingle.Enabled = false;

            // Hide Enabled Disabled Only group box
            groupBox_TriggerDisabledEnabledOnly.Visible = groupBox_TriggerDisabledEnabledOnly.Enabled = false;

            radioButton_SeverityCritical.Text =
                Utility.DescriptionHelper.GetEnumDescription(Utility.Policy.Severity.High);
            radioButton_SeverityMedium.Text =
                            Utility.DescriptionHelper.GetEnumDescription(Utility.Policy.Severity.Medium);
            radioButton_SeverityLow.Text =
                            Utility.DescriptionHelper.GetEnumDescription(Utility.Policy.Severity.Low);
            
            // load value lists for grid display
            ValueListItem listItem;
            ValueList severityValueList = new ValueList();
            severityValueList.Key = valueListSeverity;
            severityValueList.DisplayStyle = ValueListDisplayStyle.DisplayText;
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
            listItem = new ValueListItem(true, "Yes");
            enabledValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(false, "No");
            enabledValueList.ValueListItems.Add(listItem);
        }

        #endregion

        #region methods

        /// <summary>
        /// SQLsecure 3.1 (Anshul Aggarwal) - Initializes control using specified configuration values.
        /// </summary>
        internal void InitializeControl(Dictionary<PolicyMetricConfigurationColumn, string> gridColumnNames = null)
        {
            button_Remove.Enabled = false;
            if(gridColumnNames != null)
            {
                SetPolicyConfigurationFieldNames(gridColumnNames);
            }
        }

        /// <summary>
        /// SQLsecure 3.1 (Anshul Aggarwal) - Initializes control using specified configuration values.
        /// </summary>
        internal void InitializeControl(bool allowEdit, Dictionary<PolicyMetricConfigurationColumn, string> gridColumnNames = null)
        {
            button_Remove.Enabled = false;
            if (gridColumnNames != null)
            {
                SetPolicyConfigurationFieldNames(gridColumnNames);
            }

            if (!allowEdit)
            {
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

        /// <summary>
        /// SQLsecure 3.1 (Anshul Aggarwal) - Updates UI using grid row.
        /// </summary>
        public void UpdateUIWithMetric(UltraGridRow row)
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
                    groupBox_TriggerDisabledEnabledOnly.Visible = groupBox_TriggerDisabledEnabledOnly.Enabled = false;
                    groupBox_CriteriaMultiple.Visible = groupBox_CriteriaMultiple.Enabled = false;
                    groupBox_TriggerSingle.Visible = groupBox_TriggerSingle.Enabled = false;
                    groupBox_CriteriaUserEnterSingle.Visible = groupBox_CriteriaUserEnterSingle.Enabled = false;
                    groupBox_CriteriaUserEnterMultiple.Visible = groupBox_CriteriaUserEnterMultiple.Enabled = true;
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
                    groupBox_TriggerDisabledEnabledOnly.Visible = groupBox_TriggerDisabledEnabledOnly.Enabled = false;
                    groupBox_CriteriaMultiple.Visible = groupBox_CriteriaMultiple.Enabled = false;
                    groupBox_TriggerSingle.Visible = groupBox_TriggerSingle.Enabled = false;
                    groupBox_CriteriaUserEnterMultiple.Visible = groupBox_CriteriaUserEnterMultiple.Enabled = false;
                    groupBox_CriteriaUserEnterSingle.Visible = groupBox_CriteriaUserEnterSingle.Enabled = true;
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
                    groupBox_TriggerDisabledEnabledOnly.Visible = groupBox_TriggerDisabledEnabledOnly.Enabled = false;
                    groupBox_TriggerSingle.Visible = groupBox_TriggerSingle.Enabled = false;
                    groupBox_CriteriaUserEnterSingle.Visible = groupBox_CriteriaUserEnterSingle.Enabled = false;
                    groupBox_CriteriaUserEnterMultiple.Visible = groupBox_CriteriaUserEnterMultiple.Enabled = false;
                    groupBox_CriteriaMultiple.Visible = groupBox_CriteriaMultiple.Enabled = true;
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
                        groupBox_TriggerSingle.Visible = groupBox_TriggerSingle.Enabled = false;
                        groupBox_CriteriaMultiple.Visible = groupBox_CriteriaMultiple.Enabled = false;
                        groupBox_CriteriaUserEnterMultiple.Visible = groupBox_CriteriaUserEnterMultiple.Enabled = false;
                        groupBox_CriteriaUserEnterSingle.Visible = groupBox_CriteriaUserEnterSingle.Enabled = false;
                        groupBox_TriggerDisabledEnabledOnly.Visible = groupBox_TriggerDisabledEnabledOnly.Enabled = true;
                        label_DescriptionEnableDisableOnly.Text = row.Cells[colValueDescription].Text;
                    }
                    else
                    {
                        groupBox_TriggerDisabledEnabledOnly.Visible = groupBox_TriggerDisabledEnabledOnly.Enabled = false;
                        groupBox_CriteriaMultiple.Visible = groupBox_CriteriaMultiple.Enabled = false;
                        groupBox_CriteriaUserEnterMultiple.Visible = groupBox_CriteriaUserEnterMultiple.Enabled = false;
                        groupBox_CriteriaUserEnterSingle.Visible = groupBox_CriteriaUserEnterSingle.Enabled = false;
                        groupBox_TriggerSingle.Visible = groupBox_TriggerSingle.Enabled = true;
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

        /// <summary>
        /// SQLsecure 3.1 (Anshul Aggarwal) - Set values into gridrow from UI.
        /// </summary>
        public bool RetrieveValuesFromUI(UltraGridRow row, out bool refreshSort)
        {
            bool bAllowContinue = true;
            refreshSort = false;
           
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
            if (groupBox_CriteriaMultiple.Enabled)
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
            else if (groupBox_TriggerSingle.Enabled)
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
            else if (groupBox_CriteriaUserEnterMultiple.Enabled)
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
            else if (groupBox_CriteriaUserEnterSingle.Enabled)
            {
                values.Append("'");
                values.Append(textBox_UserEnterSingle.Text);
                values.Append("'");
            }

            row.Cells[colSeverityValues].Value = values.ToString();
            
            return bAllowContinue;
        }

        /// <summary>
        /// SQLsecure 3.1 (Anshul Aggarwal) - Checks if current configuration is valid or not.
        /// </summary>
        public bool OKToSave()
        {
            bool ok = true;
            if (groupBox_CriteriaUserEnterMultiple.Enabled)
            {
                if (listView_MultiSelect.Items.Count < 1)
                {
                    ok = false;
                }
            }
            else if (groupBox_CriteriaUserEnterSingle.Enabled)
            {
                if (string.IsNullOrEmpty(textBox_UserEnterSingle.Text))
                {
                    ok = false;
                }
            }
            else if (groupBox_CriteriaMultiple.Enabled)
            {
                bool atLeastOneChecked = false;
                if (checkBox1.Checked)
                {
                    atLeastOneChecked = true;
                }
                else if (checkBox2.Checked)
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
                if (!atLeastOneChecked)
                {
                    ok = false;
                }
            }
            return ok;
        }

        /// <summary>
        /// SQLsecure 3.1 (Anshul Aggarwal) - Changes grid's column name based on server type.
        /// </summary>
        private void SetPolicyConfigurationFieldNames(Dictionary<PolicyMetricConfigurationColumn, string> gridColumnNames)
        {
            if (gridColumnNames.ContainsKey(PolicyMetricConfigurationColumn.MetricName))
                colMetricName = gridColumnNames[PolicyMetricConfigurationColumn.MetricName];

            if (gridColumnNames.ContainsKey(PolicyMetricConfigurationColumn.MetricDescription))
                colMetricDescription = gridColumnNames[PolicyMetricConfigurationColumn.MetricDescription];

            if (gridColumnNames.ContainsKey(PolicyMetricConfigurationColumn.ReportKey))
                colReportKey = gridColumnNames[PolicyMetricConfigurationColumn.ReportKey];

            if (gridColumnNames.ContainsKey(PolicyMetricConfigurationColumn.ReportText))
                colReportText = gridColumnNames[PolicyMetricConfigurationColumn.ReportText];

            if (gridColumnNames.ContainsKey(PolicyMetricConfigurationColumn.Severity))
                colSeverity = gridColumnNames[PolicyMetricConfigurationColumn.Severity];

            if (gridColumnNames.ContainsKey(PolicyMetricConfigurationColumn.SeverityValues))
                colSeverityValues = gridColumnNames[PolicyMetricConfigurationColumn.SeverityValues];

            if (gridColumnNames.ContainsKey(PolicyMetricConfigurationColumn.ValidValues))
                colValidValues = gridColumnNames[PolicyMetricConfigurationColumn.ValidValues];

            if (gridColumnNames.ContainsKey(PolicyMetricConfigurationColumn.ValueDescription))
                colValueDescription = gridColumnNames[PolicyMetricConfigurationColumn.ValueDescription];
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
            if (listView_MultiSelect.SelectedItems.Count > 0)
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

    }
}
