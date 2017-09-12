using System;
using Idera.SQLsecure.UI.Console.Utility;
using Infragistics.Win.UltraWinTabControl;

namespace Idera.SQLsecure.UI.Console.Controls
{
    partial class controlConfigurePolicyVulnerabilities
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("PolicyMetric", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PolicyId");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AssessmentId");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PolicyName");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("MetricId");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("MetricType");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("MetricName");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("MetricDescription");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("IsUserEntered");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("IsMultiSelect");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ValidValues");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn11 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ValueDescription");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn12 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("IsEnabled");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn13 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ReportKey");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn14 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ReportText");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn15 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Severity");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn16 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SeverityValues");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn17 = new Infragistics.Win.UltraWinGrid.UltraGridColumn(Constants.POLICY_METRIC_COLUMN_ADB_METRIC_NAME);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn18 = new Infragistics.Win.UltraWinGrid.UltraGridColumn(Constants.POLICY_METRIC_COLUMN_ADB_METRIC_DESCRIPTION);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn19 = new Infragistics.Win.UltraWinGrid.UltraGridColumn(Constants.POLICY_METRIC_COLUMN_ADB_REPORT_KEY);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn20 = new Infragistics.Win.UltraWinGrid.UltraGridColumn(Constants.POLICY_METRIC_COLUMN_ADB_REPORT_TEXT);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn21 = new Infragistics.Win.UltraWinGrid.UltraGridColumn(Constants.POLICY_METRIC_COLUMN_ADB_SEVERITY);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn22 = new Infragistics.Win.UltraWinGrid.UltraGridColumn(Constants.POLICY_METRIC_COLUMN_ADB_SEVERITY_VALUES);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn23 = new Infragistics.Win.UltraWinGrid.UltraGridColumn(Constants.POLICY_METRIC_COLUMN_APPLICABLE_AZUREDB);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn24 = new Infragistics.Win.UltraWinGrid.UltraGridColumn(Constants.POLICY_METRIC_COLUMN_APPLICABLE_PREMISE);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn25 = new Infragistics.Win.UltraWinGrid.UltraGridColumn(Constants.POLICY_METRIC_COLUMN_METRIC_DISPLAY_NAME, -1, null, 1, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn26 = new Infragistics.Win.UltraWinGrid.UltraGridColumn(Constants.POLICY_METRIC_COLUMN_ADB_VALID_VALUES);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn27 = new Infragistics.Win.UltraWinGrid.UltraGridColumn(Constants.POLICY_METRIC_COLUMN_ADB_VALUE_DESCRIPTION);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn28 = new Infragistics.Win.UltraWinGrid.UltraGridColumn(Constants.POLICY_METRIC_COLUMN_AZURE_DB);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn29 = new Infragistics.Win.UltraWinGrid.UltraGridColumn(Constants.POLICY_METRIC_VALUE_IS_SELECTED);
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(controlConfigurePolicyVulnerabilities));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._panel_Metrics = new System.Windows.Forms.Panel();
            this.ultraGridPolicyMetrics = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.button_Clear = new Infragistics.Win.Misc.UltraButton();
            this.checkBox_GroupByCategories = new System.Windows.Forms.CheckBox();
            this.button_Import = new Infragistics.Win.Misc.UltraButton();
            this.button_ResetToDefaults = new Infragistics.Win.Misc.UltraButton();
            this.sqlServerCriteriaControl = new controlConfigureMetricCriteria(m_ControlType);
            this.azureSQLDatabaseCriteriaControl = new controlConfigureMetricCriteria(m_ControlType);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.openFileDialog_ImportPolicy = new System.Windows.Forms.OpenFileDialog();
            this._ultraPrintPreviewDialog = new Infragistics.Win.Printing.UltraPrintPreviewDialog(this.components);
            this._ultraGridPrintDocument = new Infragistics.Win.UltraWinGrid.UltraGridPrintDocument(this.components);
            this._ultraGridExcelExporter = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.policyMetricBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this._headerStrip = new Idera.SQLsecure.UI.Console.Controls.HeaderStrip();
            this._label_Header = new System.Windows.Forms.ToolStripLabel();
            this._toolStripButton_Print = new System.Windows.Forms.ToolStripButton();
            this._toolStripButton_Save = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this._toolStripButton_GroupBy = new System.Windows.Forms.ToolStripButton();
            this._toolStripButton_ColumnChooser = new System.Windows.Forms.ToolStripButton();
            this.ultraTabControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            this.groupBox1.SuspendLayout();
            this._panel_Metrics.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridPolicyMetrics)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.policyMetricBindingSource1)).BeginInit();
            this._headerStrip.SuspendLayout();
            this.ultraTabPageControl1.SuspendLayout();
            this.ultraTabPageControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).BeginInit();
            this.ultraTabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // azureSQLDatabaseCriteriaControl
            // 
            this.sqlServerCriteriaControl.BackColor = System.Drawing.Color.Transparent;
            this.sqlServerCriteriaControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sqlServerCriteriaControl.Location = new System.Drawing.Point(0, 0);
            this.sqlServerCriteriaControl.Name = "azureSQLDatabaseCriteriaControl";
            this.sqlServerCriteriaControl.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.sqlServerCriteriaControl.Size = new System.Drawing.Size(410, 495);
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.sqlServerCriteriaControl);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(410, 495);
            this.ultraTabPageControl1.Padding = new System.Windows.Forms.Padding(2, 3, 2, 0);
            // 
            // azureSQLDatabaseCriteriaControl
            // 
            this.azureSQLDatabaseCriteriaControl.BackColor = System.Drawing.Color.Transparent;
            this.azureSQLDatabaseCriteriaControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.azureSQLDatabaseCriteriaControl.Location = new System.Drawing.Point(0, 0);
            this.azureSQLDatabaseCriteriaControl.Name = "azureSQLDatabaseCriteriaControl";
            this.azureSQLDatabaseCriteriaControl.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.azureSQLDatabaseCriteriaControl.Size = new System.Drawing.Size(410, 495);
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this.azureSQLDatabaseCriteriaControl);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(410, 495);
            this.ultraTabPageControl2.Padding = new System.Windows.Forms.Padding(2, 3, 2, 0);
            // 
            // ultraTabControl1
            // 
            appearance5.BackColor = System.Drawing.Color.White;
            appearance5.BackColor2 = System.Drawing.Color.White;
            this.ultraTabControl1.ActiveTabAppearance = appearance5;
            appearance6.BackColor = System.Drawing.Color.Transparent;
            appearance6.BackColor2 = System.Drawing.Color.Transparent;
            this.ultraTabControl1.Appearance = appearance6;
            this.ultraTabControl1.ClientAreaAppearance = appearance6;
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl1);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl2);
            this.ultraTabControl1.Location = new System.Drawing.Point(0, 5);
            this.ultraTabControl1.Name = "ultraTabControl1";
            this.ultraTabControl1.Size = new System.Drawing.Size(410, 495);
            this.ultraTabControl1.TabIndex = 0;
            this.ultraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            ultraTab1.Key = "SQLServer";
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "SQL Server";
            ultraTab2.Key = "AzureSQLDatabase";
            ultraTab2.TabPage = this.ultraTabPageControl2;
            ultraTab2.Text = "Azure SQL Database";
            this.ultraTabControl1.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2});
            this.ultraTabControl1.SelectedTab = ultraTab1;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this._panel_Metrics);
            this.groupBox1.Controls.Add(this.button_Clear);
            this.groupBox1.Controls.Add(this.checkBox_GroupByCategories);
            this.groupBox1.Controls.Add(this.button_Import);
            this.groupBox1.Controls.Add(this.button_ResetToDefaults);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(313, 528);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            // 
            // _panel_Metrics
            // 
            this._panel_Metrics.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._panel_Metrics.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._panel_Metrics.Controls.Add(this.ultraGridPolicyMetrics);
            this._panel_Metrics.Controls.Add(this._headerStrip);
            this._panel_Metrics.Location = new System.Drawing.Point(0, 5);
            this._panel_Metrics.Name = "_panel_Metrics";
            this._panel_Metrics.Size = new System.Drawing.Size(313, 482);
            this._panel_Metrics.TabIndex = 17;
            // 
            // ultraGridPolicyMetrics
            // 
            this.ultraGridPolicyMetrics.DataSource = this.policyMetricBindingSource1;
            appearance1.BackColor = System.Drawing.Color.Transparent;
            this.ultraGridPolicyMetrics.DisplayLayout.Appearance = appearance1;
            ultraGridColumn1.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn1.ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;
            ultraGridColumn1.Header.VisiblePosition = 8;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn2.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn2.ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;
            ultraGridColumn2.Header.VisiblePosition = 2;
            ultraGridColumn2.Hidden = true;
            ultraGridColumn3.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn3.ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;
            ultraGridColumn3.Header.VisiblePosition = 11;
            ultraGridColumn3.Hidden = true;
            ultraGridColumn4.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn4.ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;
            ultraGridColumn4.Header.VisiblePosition = 10;
            ultraGridColumn4.Hidden = true;
            ultraGridColumn5.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn5.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            ultraGridColumn5.Header.Caption = "Category";
            ultraGridColumn5.Header.VisiblePosition = 1;
            ultraGridColumn5.Width = 78;
            ultraGridColumn6.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn6.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            ultraGridColumn6.Header.Caption = "Name";
            ultraGridColumn6.Header.VisiblePosition = 25;
            ultraGridColumn6.ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;
            ultraGridColumn6.Hidden = true;
            ultraGridColumn6.Width = 237;
            ultraGridColumn7.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn7.ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;
            ultraGridColumn7.Header.VisiblePosition = 9;
            ultraGridColumn7.Hidden = true;
            ultraGridColumn8.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn8.ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;
            ultraGridColumn8.Header.VisiblePosition = 15;
            ultraGridColumn8.Hidden = true;
            ultraGridColumn9.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn9.ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;
            ultraGridColumn9.Header.VisiblePosition = 6;
            ultraGridColumn9.Hidden = true;
            ultraGridColumn10.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn10.ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;
            ultraGridColumn10.Header.VisiblePosition = 7;
            ultraGridColumn10.Hidden = true;
            ultraGridColumn11.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn11.ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;
            ultraGridColumn11.Header.VisiblePosition = 12;
            ultraGridColumn11.Hidden = true;
            ultraGridColumn12.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn12.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;   // Enabled
            ultraGridColumn12.Header.Caption = "Enabled";
            ultraGridColumn12.Header.Fixed = true;
            ultraGridColumn12.Header.VisiblePosition = 0;
            ultraGridColumn12.Width = 49;
            ultraGridColumn13.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;  // ReportKey
            ultraGridColumn13.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            ultraGridColumn13.Header.Caption = "Cross Ref";
            ultraGridColumn13.Header.VisiblePosition = 4;
            ultraGridColumn13.Hidden = true;
            ultraGridColumn13.Width = 87;
            ultraGridColumn14.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;  // ReportText
            ultraGridColumn14.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            ultraGridColumn14.ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.False;
            ultraGridColumn14.Header.Caption = "Report Text";
            ultraGridColumn14.Header.VisiblePosition = 13;
            ultraGridColumn14.Hidden = true;
            ultraGridColumn14.Width = 237;
            ultraGridColumn15.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None;    // Severity
            ultraGridColumn15.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            ultraGridColumn15.Header.Caption = "Risk Level";
            ultraGridColumn15.Header.VisiblePosition = 3;
            ultraGridColumn15.Hidden = true;
            ultraGridColumn15.Width = 87;
            ultraGridColumn16.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;  // SeverityValues
            ultraGridColumn16.ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;
            ultraGridColumn16.Header.VisiblePosition = 14;
            ultraGridColumn16.Hidden = true;
            ultraGridColumn17.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;   // ADB MetricName
            ultraGridColumn17.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            ultraGridColumn17.Header.Caption = "Name (Azure)";
            ultraGridColumn17.Header.VisiblePosition =17;
            ultraGridColumn17.Width = 237;
            ultraGridColumn17.Hidden = true;
            ultraGridColumn18.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;       // ADB MetricDescription
            ultraGridColumn18.Header.VisiblePosition = 18;
            ultraGridColumn18.Hidden = true;
            ultraGridColumn19.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;  // ADB ReportKey
            ultraGridColumn19.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            ultraGridColumn19.Header.Caption = "Cross Ref (Azure)";
            ultraGridColumn19.Header.VisiblePosition = 19;
            ultraGridColumn19.Hidden = true;
            ultraGridColumn19.Width = 87;
            ultraGridColumn20.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;  // ADB ReportText
            ultraGridColumn20.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            ultraGridColumn20.Header.Caption = "Report Text (Azure)";
            ultraGridColumn20.Header.VisiblePosition = 20;
            ultraGridColumn20.Hidden = true;
            ultraGridColumn20.Width = 237;
            ultraGridColumn21.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None;    // ADB Severity
            ultraGridColumn21.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            ultraGridColumn21.Header.Caption = "Risk Level (Azure)";
            ultraGridColumn21.Header.VisiblePosition = 21;
            ultraGridColumn21.Hidden = true;
            ultraGridColumn21.Width = 87;
            ultraGridColumn22.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;  // ADB SeverityValues
            ultraGridColumn22.ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;
            ultraGridColumn22.Header.VisiblePosition = 22;
            ultraGridColumn22.Hidden = true;
            ultraGridColumn23.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn23.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect; // ApplicableOnAzureDB
            ultraGridColumn23.ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;
            ultraGridColumn23.Hidden = true;
            ultraGridColumn24.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn24.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect; // ApplicableOnPremise
            ultraGridColumn24.ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;
            ultraGridColumn24.Hidden = true;
            ultraGridColumn25.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn25.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            ultraGridColumn25.Header.Caption = "Name";
            ultraGridColumn25.Header.VisiblePosition = 5;
            ultraGridColumn25.Width = 237;
            ultraGridColumn26.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect; // Valid Values
            ultraGridColumn26.ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;
            ultraGridColumn26.Hidden = true;
            ultraGridColumn27.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect; // Value Description
            ultraGridColumn27.ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;
            ultraGridColumn27.Hidden = true;
            ultraGridColumn28.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect; // Value Description
            ultraGridColumn28.ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;
            ultraGridColumn28.Hidden = true;
            ultraGridColumn29.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;  // IsSelected
            ultraGridColumn29.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            ultraGridColumn29.Header.VisiblePosition = 1;

            // SQLsecure 3.1 (Anshul Aggarwal) - Set control properties based on usage of the control.
            if(m_ControlType == ConfigurePolicyControlType.ImportExportSecurityCheck)
            {
                ultraGridColumn12.Header.VisiblePosition = 2;   // Move IsEnabled behind Export/Import column
            }
            else
            {
                ultraGridColumn29.ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;
                ultraGridColumn29.Hidden = true;
            }

            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6,
            ultraGridColumn7,
            ultraGridColumn8,
            ultraGridColumn9,
            ultraGridColumn10,
            ultraGridColumn11,
            ultraGridColumn12,
            ultraGridColumn13,
            ultraGridColumn14,
            ultraGridColumn15,
            ultraGridColumn16,
            ultraGridColumn17,
            ultraGridColumn18,
            ultraGridColumn19,
            ultraGridColumn20,
            ultraGridColumn21,
            ultraGridColumn22,
            ultraGridColumn23,
            ultraGridColumn24,
            ultraGridColumn25,
            ultraGridColumn26,
            ultraGridColumn27,
            ultraGridColumn28,
            ultraGridColumn29
            });
            this.ultraGridPolicyMetrics.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.ultraGridPolicyMetrics.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGridPolicyMetrics.DisplayLayout.GroupByBox.Hidden = true;
            this.ultraGridPolicyMetrics.DisplayLayout.MaxColScrollRegions = 1;
            this.ultraGridPolicyMetrics.DisplayLayout.MaxRowScrollRegions = 1;
            this.ultraGridPolicyMetrics.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.None;
            this.ultraGridPolicyMetrics.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.InsetSoft;
            appearance2.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.ultraGridPolicyMetrics.DisplayLayout.Override.CellAppearance = appearance2;
            appearance3.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.ultraGridPolicyMetrics.DisplayLayout.Override.CellButtonAppearance = appearance3;
            appearance4.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ultraGridPolicyMetrics.DisplayLayout.Override.HeaderAppearance = appearance4;
            this.ultraGridPolicyMetrics.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGridPolicyMetrics.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.ultraGridPolicyMetrics.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.ultraGridPolicyMetrics.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.ultraGridPolicyMetrics.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.ultraGridPolicyMetrics.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraGridPolicyMetrics.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ultraGridPolicyMetrics.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.ultraGridPolicyMetrics.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ultraGridPolicyMetrics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGridPolicyMetrics.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraGridPolicyMetrics.Location = new System.Drawing.Point(0, 19);
            this.ultraGridPolicyMetrics.Name = "ultraGridPolicyMetrics";
            this.ultraGridPolicyMetrics.Size = new System.Drawing.Size(309, 459);
            this.ultraGridPolicyMetrics.TabIndex = 0;
            this.ultraGridPolicyMetrics.Text = "ultraGrid1";
            this.ultraGridPolicyMetrics.DoubleClickRow += new Infragistics.Win.UltraWinGrid.DoubleClickRowEventHandler(this.ultraGridPolicyMetrics_DoubleClickRow);
            this.ultraGridPolicyMetrics.InitializeGroupByRow += new Infragistics.Win.UltraWinGrid.InitializeGroupByRowEventHandler(this.ultraGridPolicyMetrics_InitializeGroupByRow);
            this.ultraGridPolicyMetrics.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ultraGridPolicyMetrics_KeyDown);
            this.ultraGridPolicyMetrics.BeforeRowDeactivate += new System.ComponentModel.CancelEventHandler(this.ultraGridPolicyMetrics_BeforeRowDeactivate);
            this.ultraGridPolicyMetrics.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ultraGridPolicyMetrics_MouseClick);
            this.ultraGridPolicyMetrics.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ultraGridPolicyMetrics_InitializeLayout);
            this.ultraGridPolicyMetrics.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.ultraGridPolicyMetrics_AfterSelectChange);
            // 
            // button_Clear
            // 
            this.button_Clear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_Clear.Location = new System.Drawing.Point(112, 494);
            this.button_Clear.Name = "button_Clear";
            this.button_Clear.Size = new System.Drawing.Size(98, 23);
            this.button_Clear.TabIndex = 2;
            this.button_Clear.Text = "Uncheck All";
            this.button_Clear.Click += new System.EventHandler(this.button_Clear_Click);
            // 
            // checkBox_GroupByCategories
            // 
            this.checkBox_GroupByCategories.AutoSize = true;
            this.checkBox_GroupByCategories.ForeColor = System.Drawing.Color.Navy;
            this.checkBox_GroupByCategories.Location = new System.Drawing.Point(7, 17);
            this.checkBox_GroupByCategories.Name = "checkBox_GroupByCategories";
            this.checkBox_GroupByCategories.Size = new System.Drawing.Size(122, 17);
            this.checkBox_GroupByCategories.TabIndex = 4;
            this.checkBox_GroupByCategories.Text = "Group by Categories";
            this.checkBox_GroupByCategories.UseVisualStyleBackColor = true;
            this.checkBox_GroupByCategories.Visible = false;
            this.checkBox_GroupByCategories.CheckedChanged += new System.EventHandler(this.checkBox_GroupByCategories_CheckedChanged);
            // 
            // button_Import
            // 
            this.button_Import.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_Import.Location = new System.Drawing.Point(216, 494);
            this.button_Import.MaximumSize = new System.Drawing.Size(98, 23);
            this.button_Import.MinimumSize = new System.Drawing.Size(98, 23);
            this.button_Import.Name = "button_Import";
            this.button_Import.Size = new System.Drawing.Size(98, 23);
            this.button_Import.TabIndex = 3;
            this.button_Import.Text = "Import Settings...";
            this.button_Import.Click += new System.EventHandler(this.button_Import_Click);
            // 
            // button_ResetToDefaults
            // 
            this.button_ResetToDefaults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_ResetToDefaults.Location = new System.Drawing.Point(8, 494);
            this.button_ResetToDefaults.Name = "button_ResetToDefaults";
            this.button_ResetToDefaults.Size = new System.Drawing.Size(98, 23);
            this.button_ResetToDefaults.TabIndex = 1;
            this.button_ResetToDefaults.Text = "Reset to Defaults";
            this.button_ResetToDefaults.Click += new System.EventHandler(this.button_ResetToDefaults_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1MinSize = 50;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ultraTabControl1);
            this.splitContainer1.Panel2MinSize = 50;
            // 
            // splitContainer1
            //
            this.splitContainer1.Size = new System.Drawing.Size(708, 528);
            this.splitContainer1.SplitterDistance = 313;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 10;
            // 
            // openFileDialog_ImportPolicy
            // 
            this.openFileDialog_ImportPolicy.FileName = "SQLsecurePolicy.xml";
            this.openFileDialog_ImportPolicy.Filter = "SQLsecure Policy files|*.xml|All files|*.*";
            this.openFileDialog_ImportPolicy.Title = "Import SQLsecure Policy Settings";
            // 
            // _ultraPrintPreviewDialog
            // 
            this._ultraPrintPreviewDialog.Document = this._ultraGridPrintDocument;
            this._ultraPrintPreviewDialog.Name = "_ultraPrintPreviewDialog";
            // 
            // _ultraGridPrintDocument
            // 
            this._ultraGridPrintDocument.SaveSettingsFormat = Infragistics.Win.SaveSettingsFormat.Xml;
            this._ultraGridPrintDocument.SettingsKey = "UserPermissions._ultraGridPrintDocument";
            // 
            // _saveFileDialog
            // 
            this._saveFileDialog.DefaultExt = "xls";
            this._saveFileDialog.Filter = "Excel Workbook (*.xls)|*.xls";
            this._saveFileDialog.Title = "Save as Excel Spreadsheet";
            // 
            // policyMetricBindingSource1
            // 
            this.policyMetricBindingSource1.DataSource = typeof(Idera.SQLsecure.UI.Console.Sql.PolicyMetric);
            // 
            // _headerStrip
            // 
            this._headerStrip.AutoSize = false;
            this._headerStrip.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this._headerStrip.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this._headerStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._headerStrip.HeaderStyle = Idera.SQLsecure.UI.Console.Controls.AreaHeaderStyle.Small;
            this._headerStrip.HotTrackEnabled = false;
            this._headerStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._label_Header,
            this._toolStripButton_Print,
            this._toolStripButton_Save,
            this.toolStripSeparator2,
            this._toolStripButton_GroupBy,
            this._toolStripButton_ColumnChooser});
            this._headerStrip.Location = new System.Drawing.Point(0, 0);
            this._headerStrip.Name = "_headerStrip";
            this._headerStrip.Size = new System.Drawing.Size(319, 19);
            this._headerStrip.TabIndex = 0;
            // 
            // _label_Header
            // 
            this._label_Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._label_Header.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this._label_Header.Name = "_label_Header";
            this._label_Header.Size = new System.Drawing.Size(84, 16);
            this._label_Header.Text = "Security Checks";
            // 
            // _toolStripButton_Print
            // 
            this._toolStripButton_Print.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._toolStripButton_Print.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._toolStripButton_Print.Image = ((System.Drawing.Image)(resources.GetObject("_toolStripButton_Print.Image")));
            this._toolStripButton_Print.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolStripButton_Print.Margin = new System.Windows.Forms.Padding(0);
            this._toolStripButton_Print.Name = "_toolStripButton_Print";
            this._toolStripButton_Print.Size = new System.Drawing.Size(23, 19);
            this._toolStripButton_Print.Text = "Print";
            this._toolStripButton_Print.Click += new System.EventHandler(this._toolStripButton_GridPrint_Click);
            // 
            // _toolStripButton_Save
            // 
            this._toolStripButton_Save.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._toolStripButton_Save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._toolStripButton_Save.Image = ((System.Drawing.Image)(resources.GetObject("_toolStripButton_Save.Image")));
            this._toolStripButton_Save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolStripButton_Save.Margin = new System.Windows.Forms.Padding(0);
            this._toolStripButton_Save.Name = "_toolStripButton_Save";
            this._toolStripButton_Save.Size = new System.Drawing.Size(23, 19);
            this._toolStripButton_Save.Text = "Save";
            this._toolStripButton_Save.Click += new System.EventHandler(this._toolStripButton_GridSave_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 19);
            // 
            // _toolStripButton_GroupBy
            // 
            this._toolStripButton_GroupBy.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._toolStripButton_GroupBy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._toolStripButton_GroupBy.Image = ((System.Drawing.Image)(resources.GetObject("_toolStripButton_GroupBy.Image")));
            this._toolStripButton_GroupBy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolStripButton_GroupBy.Margin = new System.Windows.Forms.Padding(0);
            this._toolStripButton_GroupBy.Name = "_toolStripButton_GroupBy";
            this._toolStripButton_GroupBy.Size = new System.Drawing.Size(23, 19);
            this._toolStripButton_GroupBy.Text = "Group By Box";
            this._toolStripButton_GroupBy.Click += new System.EventHandler(this._toolStripButton_GridGroupBy_Click);
            // 
            // _toolStripButton_ColumnChooser
            // 
            this._toolStripButton_ColumnChooser.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._toolStripButton_ColumnChooser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._toolStripButton_ColumnChooser.Image = ((System.Drawing.Image)(resources.GetObject("_toolStripButton_ColumnChooser.Image")));
            this._toolStripButton_ColumnChooser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolStripButton_ColumnChooser.Margin = new System.Windows.Forms.Padding(0);
            this._toolStripButton_ColumnChooser.Name = "_toolStripButton_ColumnChooser";
            this._toolStripButton_ColumnChooser.Size = new System.Drawing.Size(23, 19);
            this._toolStripButton_ColumnChooser.Text = "Select Columns";
            this._toolStripButton_ColumnChooser.ToolTipText = "Select Columns";
            this._toolStripButton_ColumnChooser.Click += new System.EventHandler(this._toolStripButton_ColumnChooser_Click);
            // 
            // controlConfigurePolicyVulnerabilities
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.splitContainer1);
            this.Name = "controlConfigurePolicyVulnerabilities";
            this.Size = new System.Drawing.Size(708, 528);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this._panel_Metrics.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridPolicyMetrics)).EndInit();
            this.ultraTabPageControl1.ResumeLayout(false);
            this.ultraTabPageControl1.PerformLayout();
            this.ultraTabPageControl2.ResumeLayout(false);
            this.ultraTabPageControl2.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.policyMetricBindingSource1)).EndInit();
            this._headerStrip.ResumeLayout(false);
            this._headerStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).EndInit();
            this.ultraTabControl1.ResumeLayout(false);
            this.ultraTabControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private Infragistics.Win.Misc.UltraButton button_Import;
        private Infragistics.Win.Misc.UltraButton button_ResetToDefaults;
        private Infragistics.Win.UltraWinGrid.UltraGrid ultraGridPolicyMetrics;
        private System.Windows.Forms.BindingSource policyMetricBindingSource1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.CheckBox checkBox_GroupByCategories;
        private System.Windows.Forms.OpenFileDialog openFileDialog_ImportPolicy;
        private Infragistics.Win.Misc.UltraButton button_Clear;
        private System.Windows.Forms.Panel _panel_Metrics;
        private HeaderStrip _headerStrip;
        private System.Windows.Forms.ToolStripLabel _label_Header;
        private System.Windows.Forms.ToolStripButton _toolStripButton_Print;
        private System.Windows.Forms.ToolStripButton _toolStripButton_Save;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton _toolStripButton_GroupBy;
        private System.Windows.Forms.ToolStripButton _toolStripButton_ColumnChooser;
        private Infragistics.Win.Printing.UltraPrintPreviewDialog _ultraPrintPreviewDialog;
        private Infragistics.Win.UltraWinGrid.UltraGridPrintDocument _ultraGridPrintDocument;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter _ultraGridExcelExporter;
        private System.Windows.Forms.SaveFileDialog _saveFileDialog;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl ultraTabControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private controlConfigureMetricCriteria sqlServerCriteriaControl;
        private controlConfigureMetricCriteria azureSQLDatabaseCriteriaControl;
    }
}
