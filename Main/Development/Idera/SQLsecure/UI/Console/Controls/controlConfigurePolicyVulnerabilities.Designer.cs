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
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("MetricName", -1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(controlConfigurePolicyVulnerabilities));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._panel_Metrics = new System.Windows.Forms.Panel();
            this.ultraGridPolicyMetrics = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.button_Clear = new Infragistics.Win.Misc.UltraButton();
            this.checkBox_GroupByCategories = new System.Windows.Forms.CheckBox();
            this.button_Import = new Infragistics.Win.Misc.UltraButton();
            this.button_ResetToDefaults = new Infragistics.Win.Misc.UltraButton();
            this.groupBox_TriggerSingle = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.radioButton7 = new System.Windows.Forms.RadioButton();
            this.radioButton8 = new System.Windows.Forms.RadioButton();
            this.label_ValueDescriptionS = new System.Windows.Forms.Label();
            this.groupbox_Vulnerability = new System.Windows.Forms.GroupBox();
            this.textBox_ReportKey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButton_SeverityCritical = new System.Windows.Forms.RadioButton();
            this.radioButton_SeverityMedium = new System.Windows.Forms.RadioButton();
            this.radioButton_SeverityLow = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_ReportText = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_Description = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_Name = new System.Windows.Forms.TextBox();
            this.groupBox_CriteriaUserEnterMultiple = new System.Windows.Forms.GroupBox();
            this.label_ValueDescriptionUEM = new System.Windows.Forms.Label();
            this.button_Remove = new Infragistics.Win.Misc.UltraButton();
            this.button_Edit = new Infragistics.Win.Misc.UltraButton();
            this.listView_MultiSelect = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.groupBox_TriggerDisabledEnabledOnly = new System.Windows.Forms.GroupBox();
            this.label_DescriptionEnableDisableOnly = new System.Windows.Forms.Label();
            this.groupBox_CriteriaUserEnterSingle = new System.Windows.Forms.GroupBox();
            this.textBox_UserEnterSingle = new System.Windows.Forms.TextBox();
            this.label_ValueDescriptionUES = new System.Windows.Forms.Label();
            this.groupBox_CriteriaMultiple = new System.Windows.Forms.GroupBox();
            this.checkBox8 = new System.Windows.Forms.CheckBox();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label_ValueDescriptionM = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.openFileDialog_ImportPolicy = new System.Windows.Forms.OpenFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
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
            this.groupBox1.SuspendLayout();
            this._panel_Metrics.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridPolicyMetrics)).BeginInit();
            this.groupBox_TriggerSingle.SuspendLayout();
            this.groupbox_Vulnerability.SuspendLayout();
            this.groupBox_CriteriaUserEnterMultiple.SuspendLayout();
            this.groupBox_TriggerDisabledEnabledOnly.SuspendLayout();
            this.groupBox_CriteriaUserEnterSingle.SuspendLayout();
            this.groupBox_CriteriaMultiple.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.policyMetricBindingSource1)).BeginInit();
            this._headerStrip.SuspendLayout();
            this.SuspendLayout();
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
            this.groupBox1.Size = new System.Drawing.Size(323, 528);
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
            this._panel_Metrics.Size = new System.Drawing.Size(323, 482);
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
            ultraGridColumn6.Header.VisiblePosition = 5;
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
            ultraGridColumn12.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            ultraGridColumn12.Header.Caption = "Enabled";
            ultraGridColumn12.Header.Fixed = true;
            ultraGridColumn12.Header.VisiblePosition = 0;
            ultraGridColumn12.Width = 49;
            ultraGridColumn13.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn13.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            ultraGridColumn13.Header.Caption = "Cross Ref";
            ultraGridColumn13.Header.VisiblePosition = 4;
            ultraGridColumn13.Hidden = true;
            ultraGridColumn13.Width = 87;
            ultraGridColumn14.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn14.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            ultraGridColumn14.ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.False;
            ultraGridColumn14.Header.Caption = "Report Text";
            ultraGridColumn14.Header.VisiblePosition = 13;
            ultraGridColumn14.Hidden = true;
            ultraGridColumn14.Width = 237;
            ultraGridColumn15.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None;
            ultraGridColumn15.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            ultraGridColumn15.Header.Caption = "Risk Level";
            ultraGridColumn15.Header.VisiblePosition = 3;
            ultraGridColumn15.Hidden = true;
            ultraGridColumn15.Width = 87;
            ultraGridColumn16.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn16.ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;
            ultraGridColumn16.Header.VisiblePosition = 14;
            ultraGridColumn16.Hidden = true;
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
            ultraGridColumn16});
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
            this.ultraGridPolicyMetrics.Size = new System.Drawing.Size(319, 459);
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
            // groupBox_TriggerSingle
            // 
            this.groupBox_TriggerSingle.BackColor = System.Drawing.Color.Transparent;
            this.groupBox_TriggerSingle.Controls.Add(this.radioButton1);
            this.groupBox_TriggerSingle.Controls.Add(this.radioButton2);
            this.groupBox_TriggerSingle.Controls.Add(this.radioButton3);
            this.groupBox_TriggerSingle.Controls.Add(this.radioButton4);
            this.groupBox_TriggerSingle.Controls.Add(this.radioButton5);
            this.groupBox_TriggerSingle.Controls.Add(this.radioButton6);
            this.groupBox_TriggerSingle.Controls.Add(this.radioButton7);
            this.groupBox_TriggerSingle.Controls.Add(this.radioButton8);
            this.groupBox_TriggerSingle.Controls.Add(this.label_ValueDescriptionS);
            this.groupBox_TriggerSingle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_TriggerSingle.Location = new System.Drawing.Point(0, 0);
            this.groupBox_TriggerSingle.Name = "groupBox_TriggerSingle";
            this.groupBox_TriggerSingle.Size = new System.Drawing.Size(422, 528);
            this.groupBox_TriggerSingle.TabIndex = 0;
            this.groupBox_TriggerSingle.TabStop = false;
            this.groupBox_TriggerSingle.Text = "Criteria";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(15, 74);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(85, 17);
            this.radioButton1.TabIndex = 12;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "radioButton1";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(15, 97);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(85, 17);
            this.radioButton2.TabIndex = 11;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "radioButton2";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(15, 120);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(85, 17);
            this.radioButton3.TabIndex = 10;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "radioButton3";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(15, 143);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(85, 17);
            this.radioButton4.TabIndex = 9;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "radioButton4";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Location = new System.Drawing.Point(15, 166);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(85, 17);
            this.radioButton5.TabIndex = 8;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = "radioButton5";
            this.radioButton5.UseVisualStyleBackColor = true;
            // 
            // radioButton6
            // 
            this.radioButton6.AutoSize = true;
            this.radioButton6.Location = new System.Drawing.Point(15, 189);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new System.Drawing.Size(85, 17);
            this.radioButton6.TabIndex = 7;
            this.radioButton6.TabStop = true;
            this.radioButton6.Text = "radioButton6";
            this.radioButton6.UseVisualStyleBackColor = true;
            // 
            // radioButton7
            // 
            this.radioButton7.AutoSize = true;
            this.radioButton7.Location = new System.Drawing.Point(15, 212);
            this.radioButton7.Name = "radioButton7";
            this.radioButton7.Size = new System.Drawing.Size(85, 17);
            this.radioButton7.TabIndex = 6;
            this.radioButton7.TabStop = true;
            this.radioButton7.Text = "radioButton7";
            this.radioButton7.UseVisualStyleBackColor = true;
            // 
            // radioButton8
            // 
            this.radioButton8.AutoSize = true;
            this.radioButton8.Location = new System.Drawing.Point(15, 235);
            this.radioButton8.Name = "radioButton8";
            this.radioButton8.Size = new System.Drawing.Size(85, 17);
            this.radioButton8.TabIndex = 5;
            this.radioButton8.TabStop = true;
            this.radioButton8.Text = "radioButton8";
            this.radioButton8.UseVisualStyleBackColor = true;
            // 
            // label_ValueDescriptionS
            // 
            this.label_ValueDescriptionS.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_ValueDescriptionS.ForeColor = System.Drawing.Color.Navy;
            this.label_ValueDescriptionS.Location = new System.Drawing.Point(3, 16);
            this.label_ValueDescriptionS.Name = "label_ValueDescriptionS";
            this.label_ValueDescriptionS.Size = new System.Drawing.Size(416, 46);
            this.label_ValueDescriptionS.TabIndex = 4;
            this.label_ValueDescriptionS.Text = resources.GetString("label_ValueDescriptionS.Text");
            // 
            // groupbox_Vulnerability
            // 
            this.groupbox_Vulnerability.BackColor = System.Drawing.Color.Transparent;
            this.groupbox_Vulnerability.Controls.Add(this.textBox_ReportKey);
            this.groupbox_Vulnerability.Controls.Add(this.label1);
            this.groupbox_Vulnerability.Controls.Add(this.radioButton_SeverityCritical);
            this.groupbox_Vulnerability.Controls.Add(this.radioButton_SeverityMedium);
            this.groupbox_Vulnerability.Controls.Add(this.radioButton_SeverityLow);
            this.groupbox_Vulnerability.Controls.Add(this.label5);
            this.groupbox_Vulnerability.Controls.Add(this.textBox_ReportText);
            this.groupbox_Vulnerability.Controls.Add(this.label4);
            this.groupbox_Vulnerability.Controls.Add(this.textBox_Description);
            this.groupbox_Vulnerability.Controls.Add(this.label3);
            this.groupbox_Vulnerability.Controls.Add(this.label2);
            this.groupbox_Vulnerability.Controls.Add(this.textBox_Name);
            this.groupbox_Vulnerability.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupbox_Vulnerability.Location = new System.Drawing.Point(0, 0);
            this.groupbox_Vulnerability.Name = "groupbox_Vulnerability";
            this.groupbox_Vulnerability.Size = new System.Drawing.Size(422, 248);
            this.groupbox_Vulnerability.TabIndex = 8;
            this.groupbox_Vulnerability.TabStop = false;
            this.groupbox_Vulnerability.Text = "Display Settings";
            // 
            // textBox_ReportKey
            // 
            this.textBox_ReportKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_ReportKey.Location = new System.Drawing.Point(88, 152);
            this.textBox_ReportKey.MaxLength = 32;
            this.textBox_ReportKey.Name = "textBox_ReportKey";
            this.textBox_ReportKey.Size = new System.Drawing.Size(327, 20);
            this.textBox_ReportKey.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Navy;
            this.label1.Location = new System.Drawing.Point(8, 152);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 34);
            this.label1.TabIndex = 15;
            this.label1.Text = "External Cross Reference";
            // 
            // radioButton_SeverityCritical
            // 
            this.radioButton_SeverityCritical.AutoSize = true;
            this.radioButton_SeverityCritical.ForeColor = System.Drawing.Color.Navy;
            this.radioButton_SeverityCritical.Location = new System.Drawing.Point(88, 186);
            this.radioButton_SeverityCritical.Name = "radioButton_SeverityCritical";
            this.radioButton_SeverityCritical.Size = new System.Drawing.Size(138, 17);
            this.radioButton_SeverityCritical.TabIndex = 2;
            this.radioButton_SeverityCritical.TabStop = true;
            this.radioButton_SeverityCritical.Text = "High (loaded from code)";
            this.toolTip1.SetToolTip(this.radioButton_SeverityCritical, "A vulnerability that allows an attacker immediate access into the SQL Server or a" +
                    "llows superuser access.");
            this.radioButton_SeverityCritical.UseVisualStyleBackColor = true;
            // 
            // radioButton_SeverityMedium
            // 
            this.radioButton_SeverityMedium.AutoSize = true;
            this.radioButton_SeverityMedium.ForeColor = System.Drawing.Color.Navy;
            this.radioButton_SeverityMedium.Location = new System.Drawing.Point(88, 204);
            this.radioButton_SeverityMedium.Name = "radioButton_SeverityMedium";
            this.radioButton_SeverityMedium.Size = new System.Drawing.Size(153, 17);
            this.radioButton_SeverityMedium.TabIndex = 3;
            this.radioButton_SeverityMedium.TabStop = true;
            this.radioButton_SeverityMedium.Text = "Medium (loaded from code)";
            this.toolTip1.SetToolTip(this.radioButton_SeverityMedium, "A vulnerability that provides information that have a high potential of giving ac" +
                    "cess to an intruder.");
            this.radioButton_SeverityMedium.UseVisualStyleBackColor = true;
            // 
            // radioButton_SeverityLow
            // 
            this.radioButton_SeverityLow.AutoSize = true;
            this.radioButton_SeverityLow.ForeColor = System.Drawing.Color.Navy;
            this.radioButton_SeverityLow.Location = new System.Drawing.Point(88, 222);
            this.radioButton_SeverityLow.Name = "radioButton_SeverityLow";
            this.radioButton_SeverityLow.Size = new System.Drawing.Size(136, 17);
            this.radioButton_SeverityLow.TabIndex = 4;
            this.radioButton_SeverityLow.TabStop = true;
            this.radioButton_SeverityLow.Text = "Low (loaded from code)";
            this.toolTip1.SetToolTip(this.radioButton_SeverityLow, "A vulnerability that provides information that potentially could lead to compromi" +
                    "se.");
            this.radioButton_SeverityLow.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Navy;
            this.label5.Location = new System.Drawing.Point(8, 188);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Risk Level";
            // 
            // textBox_ReportText
            // 
            this.textBox_ReportText.AcceptsReturn = true;
            this.textBox_ReportText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_ReportText.Location = new System.Drawing.Point(88, 97);
            this.textBox_ReportText.MaxLength = 4000;
            this.textBox_ReportText.Multiline = true;
            this.textBox_ReportText.Name = "textBox_ReportText";
            this.textBox_ReportText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_ReportText.Size = new System.Drawing.Size(327, 49);
            this.textBox_ReportText.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Navy;
            this.label4.Location = new System.Drawing.Point(8, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Report Text";
            // 
            // textBox_Description
            // 
            this.textBox_Description.AcceptsReturn = true;
            this.textBox_Description.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Description.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.textBox_Description.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.textBox_Description.Location = new System.Drawing.Point(88, 42);
            this.textBox_Description.Multiline = true;
            this.textBox_Description.Name = "textBox_Description";
            this.textBox_Description.ReadOnly = true;
            this.textBox_Description.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_Description.Size = new System.Drawing.Size(327, 49);
            this.textBox_Description.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Navy;
            this.label3.Location = new System.Drawing.Point(8, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Description";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Navy;
            this.label2.Location = new System.Drawing.Point(8, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Name";
            // 
            // textBox_Name
            // 
            this.textBox_Name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Name.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.textBox_Name.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.textBox_Name.Location = new System.Drawing.Point(88, 16);
            this.textBox_Name.Name = "textBox_Name";
            this.textBox_Name.ReadOnly = true;
            this.textBox_Name.Size = new System.Drawing.Size(327, 20);
            this.textBox_Name.TabIndex = 3;
            // 
            // groupBox_CriteriaUserEnterMultiple
            // 
            this.groupBox_CriteriaUserEnterMultiple.BackColor = System.Drawing.Color.Transparent;
            this.groupBox_CriteriaUserEnterMultiple.Controls.Add(this.label_ValueDescriptionUEM);
            this.groupBox_CriteriaUserEnterMultiple.Controls.Add(this.button_Remove);
            this.groupBox_CriteriaUserEnterMultiple.Controls.Add(this.button_Edit);
            this.groupBox_CriteriaUserEnterMultiple.Controls.Add(this.listView_MultiSelect);
            this.groupBox_CriteriaUserEnterMultiple.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_CriteriaUserEnterMultiple.Location = new System.Drawing.Point(0, 248);
            this.groupBox_CriteriaUserEnterMultiple.Name = "groupBox_CriteriaUserEnterMultiple";
            this.groupBox_CriteriaUserEnterMultiple.Size = new System.Drawing.Size(422, 280);
            this.groupBox_CriteriaUserEnterMultiple.TabIndex = 11;
            this.groupBox_CriteriaUserEnterMultiple.TabStop = false;
            this.groupBox_CriteriaUserEnterMultiple.Text = "Criteria";
            // 
            // label_ValueDescriptionUEM
            // 
            this.label_ValueDescriptionUEM.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label_ValueDescriptionUEM.ForeColor = System.Drawing.Color.Navy;
            this.label_ValueDescriptionUEM.Location = new System.Drawing.Point(5, 16);
            this.label_ValueDescriptionUEM.Name = "label_ValueDescriptionUEM";
            this.label_ValueDescriptionUEM.Size = new System.Drawing.Size(411, 46);
            this.label_ValueDescriptionUEM.TabIndex = 4;
            this.label_ValueDescriptionUEM.Text = "Description of the data to be supplied for this vulnerablity. For example a list " +
                "of valid Startup Stored Proceedures";
            // 
            // button_Remove
            // 
            this.button_Remove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Remove.Location = new System.Drawing.Point(341, 96);
            this.button_Remove.Name = "button_Remove";
            this.button_Remove.Size = new System.Drawing.Size(75, 23);
            this.button_Remove.TabIndex = 3;
            this.button_Remove.Text = "Remove";
            this.button_Remove.Click += new System.EventHandler(this.button_Remove_Click);
            // 
            // button_Edit
            // 
            this.button_Edit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Edit.Location = new System.Drawing.Point(341, 65);
            this.button_Edit.Name = "button_Edit";
            this.button_Edit.Size = new System.Drawing.Size(75, 23);
            this.button_Edit.TabIndex = 2;
            this.button_Edit.Text = "Edit...";
            this.button_Edit.Click += new System.EventHandler(this.button_Edit_Click);
            // 
            // listView_MultiSelect
            // 
            this.listView_MultiSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listView_MultiSelect.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listView_MultiSelect.FullRowSelect = true;
            this.listView_MultiSelect.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listView_MultiSelect.Location = new System.Drawing.Point(7, 65);
            this.listView_MultiSelect.Name = "listView_MultiSelect";
            this.listView_MultiSelect.ShowItemToolTips = true;
            this.listView_MultiSelect.Size = new System.Drawing.Size(328, 209);
            this.listView_MultiSelect.TabIndex = 0;
            this.listView_MultiSelect.UseCompatibleStateImageBehavior = false;
            this.listView_MultiSelect.View = System.Windows.Forms.View.Details;
            this.listView_MultiSelect.Resize += new System.EventHandler(this.listView_MultiSelect_Resize);
            this.listView_MultiSelect.SelectedIndexChanged += new System.EventHandler(this.listView_MultiSelect_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 200;
            // 
            // groupBox_TriggerDisabledEnabledOnly
            // 
            this.groupBox_TriggerDisabledEnabledOnly.BackColor = System.Drawing.Color.Transparent;
            this.groupBox_TriggerDisabledEnabledOnly.Controls.Add(this.label_DescriptionEnableDisableOnly);
            this.groupBox_TriggerDisabledEnabledOnly.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_TriggerDisabledEnabledOnly.Location = new System.Drawing.Point(0, 0);
            this.groupBox_TriggerDisabledEnabledOnly.Name = "groupBox_TriggerDisabledEnabledOnly";
            this.groupBox_TriggerDisabledEnabledOnly.Size = new System.Drawing.Size(422, 528);
            this.groupBox_TriggerDisabledEnabledOnly.TabIndex = 17;
            this.groupBox_TriggerDisabledEnabledOnly.TabStop = false;
            this.groupBox_TriggerDisabledEnabledOnly.Text = "Criteria";
            // 
            // label_DescriptionEnableDisableOnly
            // 
            this.label_DescriptionEnableDisableOnly.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label_DescriptionEnableDisableOnly.ForeColor = System.Drawing.Color.Navy;
            this.label_DescriptionEnableDisableOnly.Location = new System.Drawing.Point(5, 16);
            this.label_DescriptionEnableDisableOnly.Name = "label_DescriptionEnableDisableOnly";
            this.label_DescriptionEnableDisableOnly.Size = new System.Drawing.Size(411, 46);
            this.label_DescriptionEnableDisableOnly.TabIndex = 4;
            this.label_DescriptionEnableDisableOnly.Text = "Description of the data to supplied for this vulnerablity. For example a list of " +
                "valid Startup Stored Proceedures";
            // 
            // groupBox_CriteriaUserEnterSingle
            // 
            this.groupBox_CriteriaUserEnterSingle.BackColor = System.Drawing.Color.Transparent;
            this.groupBox_CriteriaUserEnterSingle.Controls.Add(this.textBox_UserEnterSingle);
            this.groupBox_CriteriaUserEnterSingle.Controls.Add(this.label_ValueDescriptionUES);
            this.groupBox_CriteriaUserEnterSingle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_CriteriaUserEnterSingle.Location = new System.Drawing.Point(0, 248);
            this.groupBox_CriteriaUserEnterSingle.Name = "groupBox_CriteriaUserEnterSingle";
            this.groupBox_CriteriaUserEnterSingle.Size = new System.Drawing.Size(422, 280);
            this.groupBox_CriteriaUserEnterSingle.TabIndex = 13;
            this.groupBox_CriteriaUserEnterSingle.TabStop = false;
            this.groupBox_CriteriaUserEnterSingle.Text = "Criteria";
            // 
            // textBox_UserEnterSingle
            // 
            this.textBox_UserEnterSingle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_UserEnterSingle.Location = new System.Drawing.Point(5, 81);
            this.textBox_UserEnterSingle.MaxLength = 400;
            this.textBox_UserEnterSingle.Name = "textBox_UserEnterSingle";
            this.textBox_UserEnterSingle.Size = new System.Drawing.Size(411, 20);
            this.textBox_UserEnterSingle.TabIndex = 5;
            // 
            // label_ValueDescriptionUES
            // 
            this.label_ValueDescriptionUES.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label_ValueDescriptionUES.ForeColor = System.Drawing.Color.Navy;
            this.label_ValueDescriptionUES.Location = new System.Drawing.Point(5, 16);
            this.label_ValueDescriptionUES.Name = "label_ValueDescriptionUES";
            this.label_ValueDescriptionUES.Size = new System.Drawing.Size(411, 46);
            this.label_ValueDescriptionUES.TabIndex = 4;
            this.label_ValueDescriptionUES.Text = "Description of the data to supplied for this vulnerablity. For example a list of " +
                "valid Startup Stored Proceedures";
            // 
            // groupBox_CriteriaMultiple
            // 
            this.groupBox_CriteriaMultiple.BackColor = System.Drawing.Color.Transparent;
            this.groupBox_CriteriaMultiple.Controls.Add(this.checkBox8);
            this.groupBox_CriteriaMultiple.Controls.Add(this.checkBox7);
            this.groupBox_CriteriaMultiple.Controls.Add(this.checkBox6);
            this.groupBox_CriteriaMultiple.Controls.Add(this.checkBox5);
            this.groupBox_CriteriaMultiple.Controls.Add(this.checkBox4);
            this.groupBox_CriteriaMultiple.Controls.Add(this.checkBox3);
            this.groupBox_CriteriaMultiple.Controls.Add(this.checkBox2);
            this.groupBox_CriteriaMultiple.Controls.Add(this.checkBox1);
            this.groupBox_CriteriaMultiple.Controls.Add(this.label_ValueDescriptionM);
            this.groupBox_CriteriaMultiple.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_CriteriaMultiple.Location = new System.Drawing.Point(0, 248);
            this.groupBox_CriteriaMultiple.Name = "groupBox_CriteriaMultiple";
            this.groupBox_CriteriaMultiple.Size = new System.Drawing.Size(422, 280);
            this.groupBox_CriteriaMultiple.TabIndex = 12;
            this.groupBox_CriteriaMultiple.TabStop = false;
            this.groupBox_CriteriaMultiple.Text = "Criteria";
            // 
            // checkBox8
            // 
            this.checkBox8.AutoSize = true;
            this.checkBox8.Location = new System.Drawing.Point(15, 235);
            this.checkBox8.Name = "checkBox8";
            this.checkBox8.Size = new System.Drawing.Size(80, 17);
            this.checkBox8.TabIndex = 12;
            this.checkBox8.Text = "checkBox8";
            this.checkBox8.UseVisualStyleBackColor = true;
            // 
            // checkBox7
            // 
            this.checkBox7.AutoSize = true;
            this.checkBox7.Location = new System.Drawing.Point(15, 212);
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.Size = new System.Drawing.Size(80, 17);
            this.checkBox7.TabIndex = 11;
            this.checkBox7.Text = "checkBox7";
            this.checkBox7.UseVisualStyleBackColor = true;
            // 
            // checkBox6
            // 
            this.checkBox6.AutoSize = true;
            this.checkBox6.Location = new System.Drawing.Point(15, 189);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(80, 17);
            this.checkBox6.TabIndex = 10;
            this.checkBox6.Text = "checkBox6";
            this.checkBox6.UseVisualStyleBackColor = true;
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Location = new System.Drawing.Point(15, 166);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(80, 17);
            this.checkBox5.TabIndex = 9;
            this.checkBox5.Text = "checkBox5";
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(15, 143);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(80, 17);
            this.checkBox4.TabIndex = 8;
            this.checkBox4.Text = "checkBox4";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(15, 120);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(80, 17);
            this.checkBox3.TabIndex = 7;
            this.checkBox3.Text = "checkBox3";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(15, 97);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(80, 17);
            this.checkBox2.TabIndex = 6;
            this.checkBox2.Text = "checkBox2";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(15, 74);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(80, 17);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label_ValueDescriptionM
            // 
            this.label_ValueDescriptionM.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label_ValueDescriptionM.ForeColor = System.Drawing.Color.Navy;
            this.label_ValueDescriptionM.Location = new System.Drawing.Point(5, 16);
            this.label_ValueDescriptionM.Name = "label_ValueDescriptionM";
            this.label_ValueDescriptionM.Size = new System.Drawing.Size(411, 46);
            this.label_ValueDescriptionM.TabIndex = 4;
            this.label_ValueDescriptionM.Text = "Description of the data to supplied for this vulnerablity. For example a list of " +
                "valid Startup Stored Proceedures";
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
            this.splitContainer1.Panel2.Controls.Add(this.groupBox_CriteriaUserEnterMultiple);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox_CriteriaUserEnterSingle);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox_CriteriaMultiple);
            this.splitContainer1.Panel2.Controls.Add(this.groupbox_Vulnerability);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox_TriggerSingle);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox_TriggerDisabledEnabledOnly);
            this.splitContainer1.Panel2MinSize = 50;
            this.splitContainer1.Size = new System.Drawing.Size(748, 528);
            this.splitContainer1.SplitterDistance = 323;
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
            this.Size = new System.Drawing.Size(748, 528);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this._panel_Metrics.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridPolicyMetrics)).EndInit();
            this.groupBox_TriggerSingle.ResumeLayout(false);
            this.groupBox_TriggerSingle.PerformLayout();
            this.groupbox_Vulnerability.ResumeLayout(false);
            this.groupbox_Vulnerability.PerformLayout();
            this.groupBox_CriteriaUserEnterMultiple.ResumeLayout(false);
            this.groupBox_TriggerDisabledEnabledOnly.ResumeLayout(false);
            this.groupBox_CriteriaUserEnterSingle.ResumeLayout(false);
            this.groupBox_CriteriaUserEnterSingle.PerformLayout();
            this.groupBox_CriteriaMultiple.ResumeLayout(false);
            this.groupBox_CriteriaMultiple.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.policyMetricBindingSource1)).EndInit();
            this._headerStrip.ResumeLayout(false);
            this._headerStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private Infragistics.Win.Misc.UltraButton button_Import;
        private Infragistics.Win.Misc.UltraButton button_ResetToDefaults;
        private System.Windows.Forms.GroupBox groupbox_Vulnerability;
        private System.Windows.Forms.GroupBox groupBox_TriggerSingle;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.RadioButton radioButton6;
        private System.Windows.Forms.RadioButton radioButton7;
        private System.Windows.Forms.RadioButton radioButton8;
        private System.Windows.Forms.Label label_ValueDescriptionS;
        private System.Windows.Forms.GroupBox groupBox_CriteriaUserEnterMultiple;
        private System.Windows.Forms.Label label_ValueDescriptionUEM;
        private Infragistics.Win.Misc.UltraButton button_Remove;
        private Infragistics.Win.Misc.UltraButton button_Edit;
        private System.Windows.Forms.ListView listView_MultiSelect;
        private System.Windows.Forms.GroupBox groupBox_CriteriaUserEnterSingle;
        private System.Windows.Forms.TextBox textBox_UserEnterSingle;
        private System.Windows.Forms.Label label_ValueDescriptionUES;
        private System.Windows.Forms.GroupBox groupBox_CriteriaMultiple;
        private System.Windows.Forms.CheckBox checkBox8;
        private System.Windows.Forms.CheckBox checkBox7;
        private System.Windows.Forms.CheckBox checkBox6;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label_ValueDescriptionM;
        private System.Windows.Forms.RadioButton radioButton_SeverityMedium;
        private System.Windows.Forms.RadioButton radioButton_SeverityLow;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_ReportText;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_Description;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_Name;
        private Infragistics.Win.UltraWinGrid.UltraGrid ultraGridPolicyMetrics;
        private System.Windows.Forms.BindingSource policyMetricBindingSource1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RadioButton radioButton_SeverityCritical;
        private System.Windows.Forms.TextBox textBox_ReportKey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox_GroupByCategories;
        private System.Windows.Forms.OpenFileDialog openFileDialog_ImportPolicy;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox_TriggerDisabledEnabledOnly;
        private System.Windows.Forms.Label label_DescriptionEnableDisableOnly;
        private System.Windows.Forms.ColumnHeader columnHeader1;
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
    }
}
