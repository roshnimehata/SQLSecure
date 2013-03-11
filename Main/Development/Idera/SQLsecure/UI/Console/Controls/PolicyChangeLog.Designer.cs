namespace Idera.SQLsecure.UI.Console.Controls
{
    partial class PolicyChangeLog
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
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PolicyChangeLog));
            this._panel_ChangeLog = new System.Windows.Forms.Panel();
            this._grid_ChangeLog = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this._headerStrip_ChangeLog = new Idera.SQLsecure.UI.Console.Controls.HeaderStrip();
            this._label_ChangeLog = new System.Windows.Forms.ToolStripLabel();
            this._toolStripButton_Print = new System.Windows.Forms.ToolStripButton();
            this._toolStripButton_Save = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this._toolStripButton_GroupBy = new System.Windows.Forms.ToolStripButton();
            this._toolStripButton_ColumnChooser = new System.Windows.Forms.ToolStripButton();
            this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this._ultraGridPrintDocument = new Infragistics.Win.UltraWinGrid.UltraGridPrintDocument(this.components);
            this._ultraPrintPreviewDialog = new Infragistics.Win.Printing.UltraPrintPreviewDialog(this.components);
            this._ultraGridExcelExporter = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this._splitContainer = new System.Windows.Forms.SplitContainer();
            this._textBox_Description = new System.Windows.Forms.TextBox();
            this._headerStrip_Description = new Idera.SQLsecure.UI.Console.Controls.HeaderStrip();
            this._label_Details = new System.Windows.Forms.ToolStripLabel();
            this._panel_ChangeLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid_ChangeLog)).BeginInit();
            this._headerStrip_ChangeLog.SuspendLayout();
            this._splitContainer.Panel1.SuspendLayout();
            this._splitContainer.Panel2.SuspendLayout();
            this._splitContainer.SuspendLayout();
            this._headerStrip_Description.SuspendLayout();
            this.SuspendLayout();
            // 
            // _panel_ChangeLog
            // 
            this._panel_ChangeLog.Controls.Add(this._grid_ChangeLog);
            this._panel_ChangeLog.Controls.Add(this._headerStrip_ChangeLog);
            this._panel_ChangeLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this._panel_ChangeLog.Location = new System.Drawing.Point(2, 2);
            this._panel_ChangeLog.Name = "_panel_ChangeLog";
            this._panel_ChangeLog.Padding = new System.Windows.Forms.Padding(2, 2, 0, 0);
            this._panel_ChangeLog.Size = new System.Drawing.Size(420, 296);
            this._panel_ChangeLog.TabIndex = 23;
            // 
            // _grid_ChangeLog
            // 
            appearance31.BackColor = System.Drawing.Color.White;
            appearance31.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this._grid_ChangeLog.DisplayLayout.Appearance = appearance31;
            this._grid_ChangeLog.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridBand1.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            ultraGridBand1.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            ultraGridBand1.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            ultraGridBand1.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this._grid_ChangeLog.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this._grid_ChangeLog.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this._grid_ChangeLog.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance32.BackColor = System.Drawing.Color.Transparent;
            appearance32.BackColor2 = System.Drawing.Color.Transparent;
            appearance32.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance32.BorderColor = System.Drawing.SystemColors.Window;
            this._grid_ChangeLog.DisplayLayout.GroupByBox.Appearance = appearance32;
            appearance33.BackColor = System.Drawing.Color.Transparent;
            appearance33.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance33.ForeColor = System.Drawing.SystemColors.GrayText;
            this._grid_ChangeLog.DisplayLayout.GroupByBox.BandLabelAppearance = appearance33;
            this._grid_ChangeLog.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this._grid_ChangeLog.DisplayLayout.GroupByBox.Hidden = true;
            appearance34.BackColor = System.Drawing.Color.Transparent;
            appearance34.BackColor2 = System.Drawing.Color.Transparent;
            appearance34.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance34.ForeColor = System.Drawing.SystemColors.GrayText;
            this._grid_ChangeLog.DisplayLayout.GroupByBox.PromptAppearance = appearance34;
            this._grid_ChangeLog.DisplayLayout.MaxColScrollRegions = 1;
            this._grid_ChangeLog.DisplayLayout.MaxRowScrollRegions = 1;
            this._grid_ChangeLog.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this._grid_ChangeLog.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this._grid_ChangeLog.DisplayLayout.Override.AllowMultiCellOperations = Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.None;
            this._grid_ChangeLog.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            this._grid_ChangeLog.DisplayLayout.Override.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.False;
            this._grid_ChangeLog.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            appearance35.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this._grid_ChangeLog.DisplayLayout.Override.CellAppearance = appearance35;
            this._grid_ChangeLog.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this._grid_ChangeLog.DisplayLayout.Override.CellPadding = 0;
            this._grid_ChangeLog.DisplayLayout.Override.ColumnAutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.AllRowsInBand;
            this._grid_ChangeLog.DisplayLayout.Override.FilterEvaluationTrigger = Infragistics.Win.UltraWinGrid.FilterEvaluationTrigger.OnCellValueChange;
            this._grid_ChangeLog.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.HeaderIcons;
            appearance36.BackColor = System.Drawing.SystemColors.Control;
            appearance36.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance36.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance36.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance36.BorderColor = System.Drawing.SystemColors.Window;
            this._grid_ChangeLog.DisplayLayout.Override.GroupByRowAppearance = appearance36;
            appearance37.TextHAlignAsString = "Left";
            this._grid_ChangeLog.DisplayLayout.Override.HeaderAppearance = appearance37;
            this._grid_ChangeLog.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this._grid_ChangeLog.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance38.BorderColor = System.Drawing.Color.LightGray;
            appearance38.TextVAlignAsString = "Middle";
            this._grid_ChangeLog.DisplayLayout.Override.RowAppearance = appearance38;
            this._grid_ChangeLog.DisplayLayout.Override.RowFilterAction = Infragistics.Win.UltraWinGrid.RowFilterAction.HideFilteredOutRows;
            this._grid_ChangeLog.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance39.ForeColor = System.Drawing.SystemColors.HighlightText;
            this._grid_ChangeLog.DisplayLayout.Override.SelectedRowAppearance = appearance39;
            this._grid_ChangeLog.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this._grid_ChangeLog.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this._grid_ChangeLog.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this._grid_ChangeLog.DisplayLayout.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.None;
            appearance40.BackColor = System.Drawing.SystemColors.ControlLight;
            this._grid_ChangeLog.DisplayLayout.Override.TemplateAddRowAppearance = appearance40;
            this._grid_ChangeLog.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.None;
            this._grid_ChangeLog.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this._grid_ChangeLog.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this._grid_ChangeLog.DisplayLayout.TabNavigation = Infragistics.Win.UltraWinGrid.TabNavigation.NextControl;
            this._grid_ChangeLog.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this._grid_ChangeLog.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this._grid_ChangeLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grid_ChangeLog.Location = new System.Drawing.Point(2, 21);
            this._grid_ChangeLog.MinimumSize = new System.Drawing.Size(250, 100);
            this._grid_ChangeLog.Name = "_grid_ChangeLog";
            this._grid_ChangeLog.Size = new System.Drawing.Size(418, 275);
            this._grid_ChangeLog.TabIndex = 21;
            this._grid_ChangeLog.Text = "ultraGrid1";
            this._grid_ChangeLog.MouseDown += new System.Windows.Forms.MouseEventHandler(this._grid_MouseDown);
            this._grid_ChangeLog.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this._grid_ChangeLog_InitializeLayout);
            this._grid_ChangeLog.AfterRowActivate += new System.EventHandler(this._grid_ChangeLog_AfterRowActivate);
            // 
            // _headerStrip_ChangeLog
            // 
            this._headerStrip_ChangeLog.AutoSize = false;
            this._headerStrip_ChangeLog.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this._headerStrip_ChangeLog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(66)))), ((int)(((byte)(139)))));
            this._headerStrip_ChangeLog.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._headerStrip_ChangeLog.HeaderStyle = Idera.SQLsecure.UI.Console.Controls.AreaHeaderStyle.Small;
            this._headerStrip_ChangeLog.HotTrackEnabled = false;
            this._headerStrip_ChangeLog.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._label_ChangeLog,
            this._toolStripButton_Print,
            this._toolStripButton_Save,
            this.toolStripSeparator3,
            this._toolStripButton_GroupBy,
            this._toolStripButton_ColumnChooser});
            this._headerStrip_ChangeLog.Location = new System.Drawing.Point(2, 2);
            this._headerStrip_ChangeLog.Name = "_headerStrip_ChangeLog";
            this._headerStrip_ChangeLog.Size = new System.Drawing.Size(418, 19);
            this._headerStrip_ChangeLog.TabIndex = 20;
            // 
            // _label_ChangeLog
            // 
            this._label_ChangeLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._label_ChangeLog.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this._label_ChangeLog.Name = "_label_ChangeLog";
            this._label_ChangeLog.Size = new System.Drawing.Size(65, 16);
            this._label_ChangeLog.Text = "Change Log";
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
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 19);
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
            // _saveFileDialog
            // 
            this._saveFileDialog.DefaultExt = "xls";
            this._saveFileDialog.Filter = "Excel Workbook (*.xls)|*.xls";
            this._saveFileDialog.Title = "Save as Excel Spreadsheet";
            // 
            // _ultraGridPrintDocument
            // 
            this._ultraGridPrintDocument.Grid = this._grid_ChangeLog;
            this._ultraGridPrintDocument.SaveSettingsFormat = Infragistics.Win.SaveSettingsFormat.Xml;
            this._ultraGridPrintDocument.SettingsKey = "PolicyChangeLog._ultraGridPrintDocument";
            // 
            // _ultraPrintPreviewDialog
            // 
            this._ultraPrintPreviewDialog.Document = this._ultraGridPrintDocument;
            this._ultraPrintPreviewDialog.Name = "_ultraPrintPreviewDialog";
            // 
            // _splitContainer
            // 
            this._splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this._splitContainer.Location = new System.Drawing.Point(0, 0);
            this._splitContainer.Name = "_splitContainer";
            this._splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // _splitContainer.Panel1
            // 
            this._splitContainer.Panel1.Controls.Add(this._panel_ChangeLog);
            this._splitContainer.Panel1.Padding = new System.Windows.Forms.Padding(2);
            // 
            // _splitContainer.Panel2
            // 
            this._splitContainer.Panel2.Controls.Add(this._textBox_Description);
            this._splitContainer.Panel2.Controls.Add(this._headerStrip_Description);
            this._splitContainer.Panel2.Padding = new System.Windows.Forms.Padding(2);
            this._splitContainer.Size = new System.Drawing.Size(424, 457);
            this._splitContainer.SplitterDistance = 300;
            this._splitContainer.TabIndex = 24;
            this._splitContainer.MouseDown += new System.Windows.Forms.MouseEventHandler(this._splitContainer_MouseDown);
            this._splitContainer.MouseUp += new System.Windows.Forms.MouseEventHandler(this._splitContainer_MouseUp);
            // 
            // _textBox_Description
            // 
            this._textBox_Description.BackColor = System.Drawing.Color.White;
            this._textBox_Description.Dock = System.Windows.Forms.DockStyle.Fill;
            this._textBox_Description.Location = new System.Drawing.Point(2, 21);
            this._textBox_Description.Multiline = true;
            this._textBox_Description.Name = "_textBox_Description";
            this._textBox_Description.ReadOnly = true;
            this._textBox_Description.Size = new System.Drawing.Size(420, 130);
            this._textBox_Description.TabIndex = 3;
            // 
            // _headerStrip_Description
            // 
            this._headerStrip_Description.AutoSize = false;
            this._headerStrip_Description.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this._headerStrip_Description.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(66)))), ((int)(((byte)(139)))));
            this._headerStrip_Description.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._headerStrip_Description.HeaderStyle = Idera.SQLsecure.UI.Console.Controls.AreaHeaderStyle.Small;
            this._headerStrip_Description.HotTrackEnabled = false;
            this._headerStrip_Description.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._label_Details});
            this._headerStrip_Description.Location = new System.Drawing.Point(2, 2);
            this._headerStrip_Description.Name = "_headerStrip_Description";
            this._headerStrip_Description.Size = new System.Drawing.Size(420, 19);
            this._headerStrip_Description.TabIndex = 2;
            // 
            // _label_Details
            // 
            this._label_Details.Name = "_label_Details";
            this._label_Details.Size = new System.Drawing.Size(64, 16);
            this._label_Details.Text = "Description:";
            // 
            // PolicyChangeLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this._splitContainer);
            this.Name = "PolicyChangeLog";
            this.Size = new System.Drawing.Size(424, 457);
            this.Enter += new System.EventHandler(this.PolicyChangeLog_Enter);
            this._panel_ChangeLog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._grid_ChangeLog)).EndInit();
            this._headerStrip_ChangeLog.ResumeLayout(false);
            this._headerStrip_ChangeLog.PerformLayout();
            this._splitContainer.Panel1.ResumeLayout(false);
            this._splitContainer.Panel2.ResumeLayout(false);
            this._splitContainer.Panel2.PerformLayout();
            this._splitContainer.ResumeLayout(false);
            this._headerStrip_Description.ResumeLayout(false);
            this._headerStrip_Description.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel _panel_ChangeLog;
        private Infragistics.Win.UltraWinGrid.UltraGrid _grid_ChangeLog;
        private HeaderStrip _headerStrip_ChangeLog;
        private System.Windows.Forms.ToolStripLabel _label_ChangeLog;
        private System.Windows.Forms.ToolStripButton _toolStripButton_Print;
        private System.Windows.Forms.ToolStripButton _toolStripButton_Save;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton _toolStripButton_GroupBy;
        private System.Windows.Forms.ToolStripButton _toolStripButton_ColumnChooser;
        private System.Windows.Forms.SaveFileDialog _saveFileDialog;
        private Infragistics.Win.UltraWinGrid.UltraGridPrintDocument _ultraGridPrintDocument;
        private Infragistics.Win.Printing.UltraPrintPreviewDialog _ultraPrintPreviewDialog;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter _ultraGridExcelExporter;
        private System.Windows.Forms.SplitContainer _splitContainer;
        private HeaderStrip _headerStrip_Description;
        private System.Windows.Forms.ToolStripLabel _label_Details;
        private System.Windows.Forms.TextBox _textBox_Description;
    }
}
