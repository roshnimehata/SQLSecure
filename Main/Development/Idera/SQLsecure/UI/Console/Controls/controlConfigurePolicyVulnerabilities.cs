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
using Infragistics.Win.UltraWinTabControl;

namespace Idera.SQLsecure.UI.Console.Controls
{
    internal partial class controlConfigurePolicyVulnerabilities : UserControl
    {
        #region fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Controls.controlConfigurePolicyVulnerabilities");
        private Policy m_policy;
        List<PolicyMetric> m_metrics = null;
        private bool m_InternalUpdate = false;
        private bool m_importing = false;

        private bool m_allowEdit = true;
        private ConfigurePolicyControlType m_ControlType;

        #endregion
        
        #region ctors

        internal controlConfigurePolicyVulnerabilities(ConfigurePolicyControlType state)
        {
            // SQLsecure 3.1 (Anshul Aggarwal) - Represents current state of control - 'Configure Security Check' or 'Export/Import Policy'.
            m_ControlType = state;

            InitializeComponent();

            ultraTabControl1.DrawFilter = new HideFocusRectangleDrawFilter();
            
            _toolStripButton_ColumnChooser.Image = AppIcons.AppImage16(AppIcons.Enum.GridFieldChooser);
            _toolStripButton_GroupBy.Image = AppIcons.AppImage16(AppIcons.Enum.GridGroupBy);
            _toolStripButton_Save.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);
            _toolStripButton_Print.Image = AppIcons.AppImage16(AppIcons.Enum.Print);

            // load value lists for grid display
            ValueListItem listItem;
            ValueList severityValueList = new ValueList();
            severityValueList.Key = Utility.Constants.POLICY_METRIC_VALUE_LIST_SERVERITY;
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
            enabledValueList.Key = Utility.Constants.POLICY_METRIC_VALUE_LIST_ENABLED;
            enabledValueList.DisplayStyle = ValueListDisplayStyle.DisplayText;
            ultraGridPolicyMetrics.DisplayLayout.ValueLists.Add(enabledValueList);
            listItem = new ValueListItem(true, "Yes");
            enabledValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(false, "No");
            enabledValueList.ValueListItems.Add(listItem);
            
            RefreshState();
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
        
        public string IsSelectColumnDisplayText
        {
            get { return ultraGridPolicyMetrics.DisplayLayout.Bands[0].Columns[Utility.Constants.POLICY_METRIC_VALUE_IS_SELECTED].Header.Caption; }
            set { ultraGridPolicyMetrics.DisplayLayout.Bands[0].Columns[Utility.Constants.POLICY_METRIC_VALUE_IS_SELECTED].Header.Caption = value; }
        }

        #endregion

        #region methods

        /// <summary>
        /// SQLsecure 3.1 (Anshul Aggarwal) - Initializes control using specified configuration values.
        /// </summary>
        public void InitializeControl(Policy policy)
        {
            m_policy = policy;
            m_importing = m_policy.PolicyId == 0;
            checkBox_GroupByCategories.Checked = true;
            
            sqlServerCriteriaControl.InitializeControl();      // SQLsecure 3.1 (Anshul Aggarwal) - Initialize both tabs.
            azureSQLDatabaseCriteriaControl.InitializeControl(GetAzureSQLDBGridColumnMapping());

            loadPolicyMetrics();
        }
        
        /// <summary>
        /// SQLsecure 3.1 (Anshul Aggarwal) - Initializes control using specified configuration values.
        /// </summary>
        public void InitializeControl(Policy policy, int metricId, bool allowEdit)
        {
            m_policy = policy;
            m_importing = m_policy.PolicyId == 0;
            checkBox_GroupByCategories.Checked = true;
            
            sqlServerCriteriaControl.InitializeControl(allowEdit);     // SQLsecure 3.1 (Anshul Aggarwal) - Initialize both tabs.
            azureSQLDatabaseCriteriaControl.InitializeControl(allowEdit, GetAzureSQLDBGridColumnMapping());

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
                    button_ResetToDefaults.Enabled = false;
            }
        }

        /// <summary>
        /// SQLsecure 3.1 (Anshul Aggarwal) - Checks if current configuration is valid or not.
        /// </summary>
        public bool OKToSave()
        {
            UltraGridRow row = ultraGridPolicyMetrics.ActiveRow;
            if (row != null && row.IsDataRow)
            {
                UltraGridCell cell = row.Cells[Utility.Constants.POLICY_METRIC_COLUMN_IS_ENABLED];
                var policyMetric = row.ListObject as PolicyMetric;
                if (policyMetric != null && cell != null && cell.Value is bool && (bool)cell.Value == true)
                {
                    if (row.IsDataRow && policyMetric != null)
                    {
                        bool okOnPremise = false;
                        bool okADB = false;
                        if (policyMetric.ApplicableOnPremise)
                        {
                            okOnPremise = sqlServerCriteriaControl.OKToSave();
                        }

                        // SQLsecure 3.1 (Anshul Aggarwal) - Even if a single tab is valid, we will accept it.
                        if (!policyMetric.ApplicableOnPremise || (policyMetric.ApplicableOnAzureDB && !okOnPremise))
                        {
                            okADB = azureSQLDatabaseCriteriaControl.OKToSave();
                        }


                        bool ok = (policyMetric.ApplicableOnPremise && okOnPremise) || (policyMetric.ApplicableOnAzureDB && okADB);
                        if (!ok)
                        {
                            //  SQLsecure 3.1 (Anshul Aggarwal) - Switch to the tab that is invalid.
                            if (policyMetric.ApplicableOnPremise && !okOnPremise)
                            {
                                ultraTabControl1.SelectedTab = ultraTabControl1.Tabs[0];
                            }
                            else if (policyMetric.ApplicableOnAzureDB && !okADB)
                            {
                                ultraTabControl1.SelectedTab = ultraTabControl1.Tabs[1];
                            }

                            MsgBox.ShowError("Security Checks",
                                             "This security check requires at least one criteria be specified.\n\nEither specify a criteria or disable this security check.");
                        }
                        return ok;
                    }
                }
            }
            return true;
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

        /// <summary>
        /// SQLsecure 3.1 (Anshul Aggarwal) - Gets grid column name mapping of Azure SQL Database.
        /// </summary>
        private static Dictionary<PolicyMetricConfigurationColumn, string> GetAzureSQLDBGridColumnMapping()
        {
            return new Dictionary<PolicyMetricConfigurationColumn, string>() {
                    { PolicyMetricConfigurationColumn.MetricDescription, Utility.Constants.POLICY_METRIC_COLUMN_ADB_METRIC_DESCRIPTION},
                    { PolicyMetricConfigurationColumn.MetricName, Utility.Constants.POLICY_METRIC_COLUMN_ADB_METRIC_NAME},
                    { PolicyMetricConfigurationColumn.ReportKey, Utility.Constants.POLICY_METRIC_COLUMN_ADB_REPORT_KEY},
                    { PolicyMetricConfigurationColumn.ReportText, Utility.Constants.POLICY_METRIC_COLUMN_ADB_REPORT_TEXT},
                    { PolicyMetricConfigurationColumn.Severity, Utility.Constants.POLICY_METRIC_COLUMN_ADB_SEVERITY},
                    { PolicyMetricConfigurationColumn.SeverityValues, Utility.Constants.POLICY_METRIC_COLUMN_ADB_SEVERITY_VALUES},
                    { PolicyMetricConfigurationColumn.ValidValues, Utility.Constants.POLICY_METRIC_COLUMN_ADB_VALID_VALUES},
                    { PolicyMetricConfigurationColumn.ValueDescription, Utility.Constants.POLICY_METRIC_COLUMN_ADB_VALUE_DESCRIPTION},
                };
        }

        #region Grid functions

        protected void showGridColumnChooser(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            Forms.Form_GridColumnChooser.Process(grid, Utility.Constants.POLICY_METRIC_PROPERTIES_PRINT_TITLE);
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
            _ultraGridPrintDocument.DocumentName = Utility.Constants.POLICY_METRIC_PROPERTIES_PRINT_TITLE;
            _ultraGridPrintDocument.FitWidthToPages = 1;
            _ultraGridPrintDocument.Header.TextLeft =
                    string.Format(Utility.Constants.POLICY_METRIC_PROPERTIES_PRINT_HEADER_DISPLAY,
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
                    grid.DisplayLayout.Bands[0].Columns[Utility.Constants.POLICY_METRIC_COLUMN_IS_ENABLED].ValueList = grid.DisplayLayout.ValueLists[Utility.Constants.POLICY_METRIC_VALUE_LIST_ENABLED];

                    _ultraGridExcelExporter.Export(grid, _saveFileDialog.FileName);

                    grid.DisplayLayout.Bands[0].Columns[Utility.Constants.POLICY_METRIC_COLUMN_IS_ENABLED].ValueList = null;
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

        /// <summary>
        /// SQLsecure 3.1 (Anshul Aggarwal) - Set values into gridrow from UI.
        /// </summary>
        private bool RetrieveValuesFromUI()
        {
            bool bAllowContinue = true;
            if (Visible && !m_InternalUpdate)
            {
                if(m_ControlType == ConfigurePolicyControlType.ConfigureSecurityCheck && !OKToSave())
                {
                    return false;
                }
              
                UltraGridRow row = ultraGridPolicyMetrics.ActiveRow;
                if (row != null)
                {
                    var policyMetric = row.ListObject as PolicyMetric;
                    if (row.IsDataRow && policyMetric != null)
                    {
                        bool refreshSort = false;

                        if (policyMetric.ApplicableOnPremise)
                        {
                            sqlServerCriteriaControl.RetrieveValuesFromUI(row, out refreshSort);
                        }

                        if (policyMetric.ApplicableOnAzureDB)
                        {
                            azureSQLDatabaseCriteriaControl.RetrieveValuesFromUI(row, out refreshSort);
                        }

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
            _label_Header.Text = string.Format(Utility.Constants.POLICY_METRIC_PROPERTIES_HEADER_DISPLAY, enabledCount);
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

        /// <summary>
        /// SQLsecure 3.1 (Anshul Aggarwal) - Updates UI using grid row.
        /// </summary>
        private void UpdateUIWithMetric()
        {
            foreach (UltraGridRow row in ultraGridPolicyMetrics.Selected.Rows)
            {
                if (row.Cells != null)
                {
                    var policyMetric = row.ListObject as PolicyMetric;
                    if (policyMetric == null)
                        continue;

                    if (policyMetric.ApplicableOnPremise)
                    {
                        ultraTabControl1.Tabs[0].Visible = true;
                        sqlServerCriteriaControl.UpdateUIWithMetric(row);
                    }
                    else
                    {
                        ultraTabControl1.Tabs[0].Visible = false;
                    }
                    
                    if(policyMetric.ApplicableOnAzureDB)
                    {
                        ultraTabControl1.Tabs[1].Visible = true;
                        azureSQLDatabaseCriteriaControl.UpdateUIWithMetric(row);
                    }
                    else
                    {
                        ultraTabControl1.Tabs[1].Visible = false;
                    }
                }
            }
        }

        private void RefreshState()
        {
            if (m_ControlType == ConfigurePolicyControlType.ImportExportSecurityCheck)
            {
                button_Import.Visible = button_Clear.Visible = false;
                checkBox_GroupByCategories.Visible = false;
                button_ResetToDefaults.Visible = false;

                button_Import.Enabled =
                   button_Clear.Enabled =
                   button_ResetToDefaults.Enabled = false;
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
                        // SQLsecure 3.1 (Anshul Aggarwal) - Type of control decides editable columns in the grid.
                        if(m_ControlType == ConfigurePolicyControlType.ImportExportSecurityCheck)
                        {
                            UltraGridCell cell = selectedElement.GetContext(typeof(UltraGridCell)) as UltraGridCell;
                            if (cell != null &&
                                (cell.Column.Key == Utility.Constants.POLICY_METRIC_VALUE_IS_SELECTED))
                            {
                                cell.Value = !((bool)cell.Value);
                                UpdateEnabledCount();
                            }
                        }
                        else if (m_ControlType == ConfigurePolicyControlType.ConfigureSecurityCheck)
                        {
                            Infragistics.Win.UltraWinGrid.UltraGridCell cell = row.Cells[Utility.Constants.POLICY_METRIC_COLUMN_IS_ENABLED];
                            if (cell != null && cell.Value is bool)
                            {
                                cell.Value = !(bool)cell.Value;
                                UpdateEnabledCount();
                            }
                        }
                    }
                }
            }
        }

        private void ultraGridPolicyMetrics_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            UltraGridBand band = e.Layout.Bands[0];

            band.Columns[Utility.Constants.POLICY_METRIC_COLUMN_SEVERITY].ValueList = e.Layout.ValueLists[Utility.Constants.POLICY_METRIC_VALUE_LIST_SERVERITY];
            EditorWithText textEditor = new EditorWithText();
            band.Columns[Utility.Constants.POLICY_METRIC_COLUMN_SEVERITY].Editor = textEditor;

            if (ultraGridPolicyMetrics.Visible)
            {
                ultraGridPolicyMetrics.Focus();
            }
        }

        private void ultraGridPolicyMetrics_InitializeGroupByRow(object sender, Infragistics.Win.UltraWinGrid.InitializeGroupByRowEventArgs e)
        {
            UltraGrid grid = (UltraGrid)sender;
            e.Row.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
            if (e.Row.Column.Key == Utility.Constants.POLICY_METRIC_COLUMN_METRIC_TYPE || e.Row.Column.Key == Utility.Constants.POLICY_METRIC_COLUMN_SEVERITY ||
                e.Row.Column.Key == Utility.Constants.POLICY_METRIC_COLUMN_IS_ENABLED || e.Row.Column.Key == Utility.Constants.POLICY_METRIC_COLUMN_REPORT_KEY)
            {
                //e.Row.ExpansionIndicator = ShowExpansionIndicator.Never;
                int count = GetRowMetricCount(e.Row);
                string descr = e.Row.ValueAsDisplayText;
                if (e.Row.Column.Key == Utility.Constants.POLICY_METRIC_COLUMN_IS_ENABLED)
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
                        Infragistics.Win.UltraWinGrid.UltraGridCell cell = row.Cells[Utility.Constants.POLICY_METRIC_COLUMN_IS_ENABLED];
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
                Infragistics.Win.UltraWinGrid.UltraGridCell cell = row.Cells[Utility.Constants.POLICY_METRIC_COLUMN_IS_ENABLED];
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
        
        private void checkBox_GroupByCategories_CheckedChanged(object sender, EventArgs e)
        {
            updateVulnerabilityLayout();
        }

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
