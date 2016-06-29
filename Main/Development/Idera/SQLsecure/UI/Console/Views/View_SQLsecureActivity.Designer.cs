namespace Idera.SQLsecure.UI.Console.Views
{
    partial class View_SQLsecureActivity
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(View_SQLsecureActivity));
            this._grid = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this._contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._cmsi_refresh = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this._headerStrip_Activity = new Idera.SQLsecure.UI.Console.Controls.HeaderStrip();
            this._label_Activity = new System.Windows.Forms.ToolStripLabel();
            this._toolStripButton_ActivityPrint = new System.Windows.Forms.ToolStripButton();
            this._toolStripButton_ActivitySave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._toolStripButton_ActivityGroupBy = new System.Windows.Forms.ToolStripButton();
            this._toolStripButton_ActivityColumnChooser = new System.Windows.Forms.ToolStripButton();
            this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this._ultraPrintPreviewDialog = new Infragistics.Win.Printing.UltraPrintPreviewDialog(this.components);
            this._ultraGridPrintDocument = new Infragistics.Win.UltraWinGrid.UltraGridPrintDocument(this.components);
            this._ultraGridExcelExporter = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this._cmsi_Grid_ColumnChooser = new System.Windows.Forms.ToolStripMenuItem();
            this._cmsi_Grid_viewGroupByBox = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this._cmsi_Grid_Save = new System.Windows.Forms.ToolStripMenuItem();
            this._cmsi_Grid_Print = new System.Windows.Forms.ToolStripMenuItem();
            this._vw_MainPanel.SuspendLayout();
            this._vw_TasksPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
            this._contextMenuStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            this._headerStrip_Activity.SuspendLayout();
            this.SuspendLayout();
            // 
            // _vw_MainPanel
            // 
            this._vw_MainPanel.Controls.Add(this.panel1);
            this._vw_MainPanel.Location = new System.Drawing.Point(0, 72);
            this._vw_MainPanel.Size = new System.Drawing.Size(652, 516);
            // 
            // _vw_TasksPanel
            // 
            this._vw_TasksPanel.Size = new System.Drawing.Size(652, 72);
            // 
            // _smallTask_Help
            // 
            this._smallTask_Help.Location = new System.Drawing.Point(533, 31);
            this._smallTask_Help.Size = new System.Drawing.Size(111, 34);
            // 
            // _label_Summary
            // 
            this._label_Summary.Location = new System.Drawing.Point(1, 1);
            // 
            // _grid
            // 
            this._grid.ContextMenuStrip = this._contextMenuStrip;
            appearance15.BackColor = System.Drawing.Color.White;
            appearance15.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this._grid.DisplayLayout.Appearance = appearance15;
            this._grid.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand1.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            ultraGridBand1.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            ultraGridBand1.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            ultraGridBand1.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this._grid.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this._grid.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this._grid.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance16.BackColor = System.Drawing.Color.Transparent;
            appearance16.BackColor2 = System.Drawing.Color.Transparent;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance16.BorderColor = System.Drawing.SystemColors.Window;
            this._grid.DisplayLayout.GroupByBox.Appearance = appearance16;
            appearance17.BackColor = System.Drawing.Color.Transparent;
            appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance17.ForeColor = System.Drawing.SystemColors.GrayText;
            this._grid.DisplayLayout.GroupByBox.BandLabelAppearance = appearance17;
            this._grid.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance18.BackColor = System.Drawing.Color.Transparent;
            appearance18.BackColor2 = System.Drawing.Color.Transparent;
            appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance18.ForeColor = System.Drawing.SystemColors.GrayText;
            this._grid.DisplayLayout.GroupByBox.PromptAppearance = appearance18;
            this._grid.DisplayLayout.MaxColScrollRegions = 1;
            this._grid.DisplayLayout.MaxRowScrollRegions = 1;
            appearance19.BackColor = System.Drawing.SystemColors.Window;
            appearance19.ForeColor = System.Drawing.SystemColors.ControlText;
            this._grid.DisplayLayout.Override.ActiveCellAppearance = appearance19;
            appearance20.BackColor = System.Drawing.Color.White;
            appearance20.ForeColor = System.Drawing.Color.Black;
            this._grid.DisplayLayout.Override.ActiveRowAppearance = appearance20;
            this._grid.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this._grid.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this._grid.DisplayLayout.Override.AllowMultiCellOperations = Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.None;
            this._grid.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            this._grid.DisplayLayout.Override.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.False;
            this._grid.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this._grid.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this._grid.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance21.BackColor = System.Drawing.Color.Transparent;
            this._grid.DisplayLayout.Override.CardAreaAppearance = appearance21;
            appearance22.BorderColor = System.Drawing.Color.Silver;
            appearance22.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this._grid.DisplayLayout.Override.CellAppearance = appearance22;
            this._grid.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this._grid.DisplayLayout.Override.CellPadding = 0;
            this._grid.DisplayLayout.Override.ColumnAutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.AllRowsInBand;
            this._grid.DisplayLayout.Override.FilterEvaluationTrigger = Infragistics.Win.UltraWinGrid.FilterEvaluationTrigger.OnCellValueChange;
            this._grid.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.HeaderIcons;
            appearance23.BackColor = System.Drawing.SystemColors.Control;
            appearance23.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance23.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance23.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance23.BorderColor = System.Drawing.SystemColors.Window;
            this._grid.DisplayLayout.Override.GroupByRowAppearance = appearance23;
            appearance24.TextHAlignAsString = "Left";
            this._grid.DisplayLayout.Override.HeaderAppearance = appearance24;
            this._grid.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this._grid.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance25.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this._grid.DisplayLayout.Override.RowAlternateAppearance = appearance25;
            appearance26.BorderColor = System.Drawing.Color.LightGray;
            appearance26.TextVAlignAsString = "Middle";
            this._grid.DisplayLayout.Override.RowAppearance = appearance26;
            this._grid.DisplayLayout.Override.RowFilterAction = Infragistics.Win.UltraWinGrid.RowFilterAction.HideFilteredOutRows;
            this._grid.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance27.BackColor = System.Drawing.SystemColors.Highlight;
            appearance27.BorderColor = System.Drawing.Color.Black;
            appearance27.ForeColor = System.Drawing.SystemColors.HighlightText;
            this._grid.DisplayLayout.Override.SelectedRowAppearance = appearance27;
            this._grid.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this._grid.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this._grid.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this._grid.DisplayLayout.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.None;
            appearance28.BackColor = System.Drawing.SystemColors.ControlLight;
            this._grid.DisplayLayout.Override.TemplateAddRowAppearance = appearance28;
            this._grid.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.None;
            this._grid.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this._grid.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this._grid.DisplayLayout.TabNavigation = Infragistics.Win.UltraWinGrid.TabNavigation.NextControl;
            this._grid.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this._grid.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grid.Location = new System.Drawing.Point(0, 19);
            this._grid.Name = "_grid";
            this._grid.Size = new System.Drawing.Size(648, 493);
            this._grid.TabIndex = 2;
            this._grid.MouseDown += new System.Windows.Forms.MouseEventHandler(this._grid_MouseDown);
            this._grid.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this._grid_InitializeLayout);
            // 
            // _contextMenuStrip
            // 
            this._contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._cmsi_Grid_ColumnChooser,
            this._cmsi_Grid_viewGroupByBox,
            this.toolStripSeparator2,
            this._cmsi_Grid_Save,
            this._cmsi_Grid_Print,
            this.toolStripSeparator3,
            this._cmsi_refresh});
            this._contextMenuStrip.Name = "_contextMenuStrip";
            this._contextMenuStrip.Size = new System.Drawing.Size(158, 126);
            this._contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this._contextMenuStrip_Opening);
            // 
            // _cmsi_refresh
            // 
            this._cmsi_refresh.Name = "_cmsi_refresh";
            this._cmsi_refresh.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this._cmsi_refresh.Size = new System.Drawing.Size(157, 22);
            this._cmsi_refresh.Text = "Refresh";
            this._cmsi_refresh.Click += new System.EventHandler(this._cmsi_refresh_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this._grid);
            this.panel1.Controls.Add(this._headerStrip_Activity);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(652, 516);
            this.panel1.TabIndex = 0;
            // 
            // _headerStrip_Activity
            // 
            this._headerStrip_Activity.AutoSize = false;
            this._headerStrip_Activity.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this._headerStrip_Activity.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(66)))), ((int)(((byte)(139)))));
            this._headerStrip_Activity.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._headerStrip_Activity.HeaderStyle = Idera.SQLsecure.UI.Console.Controls.AreaHeaderStyle.Small;
            this._headerStrip_Activity.HotTrackEnabled = false;
            this._headerStrip_Activity.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._label_Activity,
            this._toolStripButton_ActivityPrint,
            this._toolStripButton_ActivitySave,
            this.toolStripSeparator1,
            this._toolStripButton_ActivityGroupBy,
            this._toolStripButton_ActivityColumnChooser});
            this._headerStrip_Activity.Location = new System.Drawing.Point(0, 0);
            this._headerStrip_Activity.Name = "_headerStrip_Activity";
            this._headerStrip_Activity.Size = new System.Drawing.Size(648, 19);
            this._headerStrip_Activity.TabIndex = 13;
            // 
            // _label_Activity
            // 
            this._label_Activity.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._label_Activity.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this._label_Activity.Name = "_label_Activity";
            this._label_Activity.Size = new System.Drawing.Size(118, 16);
            this._label_Activity.Text = "SQLsecure Activity Log";
            // 
            // _toolStripButton_ActivityPrint
            // 
            this._toolStripButton_ActivityPrint.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._toolStripButton_ActivityPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._toolStripButton_ActivityPrint.Image = ((System.Drawing.Image)(resources.GetObject("_toolStripButton_ActivityPrint.Image")));
            this._toolStripButton_ActivityPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolStripButton_ActivityPrint.Margin = new System.Windows.Forms.Padding(0);
            this._toolStripButton_ActivityPrint.Name = "_toolStripButton_ActivityPrint";
            this._toolStripButton_ActivityPrint.Size = new System.Drawing.Size(23, 19);
            this._toolStripButton_ActivityPrint.Text = "Print";
            this._toolStripButton_ActivityPrint.Click += new System.EventHandler(this._toolStripButton_GridPrint_Click);
            // 
            // _toolStripButton_ActivitySave
            // 
            this._toolStripButton_ActivitySave.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._toolStripButton_ActivitySave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._toolStripButton_ActivitySave.Image = ((System.Drawing.Image)(resources.GetObject("_toolStripButton_ActivitySave.Image")));
            this._toolStripButton_ActivitySave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolStripButton_ActivitySave.Margin = new System.Windows.Forms.Padding(0);
            this._toolStripButton_ActivitySave.Name = "_toolStripButton_ActivitySave";
            this._toolStripButton_ActivitySave.Size = new System.Drawing.Size(23, 19);
            this._toolStripButton_ActivitySave.Text = "Save";
            this._toolStripButton_ActivitySave.Click += new System.EventHandler(this._toolStripButton_GridSave_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 19);
            // 
            // _toolStripButton_ActivityGroupBy
            // 
            this._toolStripButton_ActivityGroupBy.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._toolStripButton_ActivityGroupBy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._toolStripButton_ActivityGroupBy.Image = ((System.Drawing.Image)(resources.GetObject("_toolStripButton_ActivityGroupBy.Image")));
            this._toolStripButton_ActivityGroupBy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolStripButton_ActivityGroupBy.Margin = new System.Windows.Forms.Padding(0);
            this._toolStripButton_ActivityGroupBy.Name = "_toolStripButton_ActivityGroupBy";
            this._toolStripButton_ActivityGroupBy.Size = new System.Drawing.Size(23, 19);
            this._toolStripButton_ActivityGroupBy.Text = "Group By Box";
            this._toolStripButton_ActivityGroupBy.Click += new System.EventHandler(this._toolStripButton_GridGroupBy_Click);
            // 
            // _toolStripButton_ActivityColumnChooser
            // 
            this._toolStripButton_ActivityColumnChooser.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._toolStripButton_ActivityColumnChooser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._toolStripButton_ActivityColumnChooser.Image = ((System.Drawing.Image)(resources.GetObject("_toolStripButton_ActivityColumnChooser.Image")));
            this._toolStripButton_ActivityColumnChooser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolStripButton_ActivityColumnChooser.Margin = new System.Windows.Forms.Padding(0);
            this._toolStripButton_ActivityColumnChooser.Name = "_toolStripButton_ActivityColumnChooser";
            this._toolStripButton_ActivityColumnChooser.Size = new System.Drawing.Size(23, 19);
            this._toolStripButton_ActivityColumnChooser.Text = "Select Columns";
            this._toolStripButton_ActivityColumnChooser.ToolTipText = "Select Columns";
            this._toolStripButton_ActivityColumnChooser.Click += new System.EventHandler(this._toolStripButton_ColumnChooser_Click);
            // 
            // _saveFileDialog
            // 
            this._saveFileDialog.DefaultExt = "xls";
            this._saveFileDialog.Filter = "Excel Workbook (*.xls)|*.xls";
            this._saveFileDialog.Title = "Save as Excel Spreadsheet";
            // 
            // _ultraPrintPreviewDialog
            // 
            this._ultraPrintPreviewDialog.Document = this._ultraGridPrintDocument;
            this._ultraPrintPreviewDialog.Name = "_ultraPrintPreviewDialog";
            // 
            // _ultraGridPrintDocument
            // 
            this._ultraGridPrintDocument.Grid = this._grid;
            this._ultraGridPrintDocument.SaveSettingsFormat = Infragistics.Win.SaveSettingsFormat.Xml;
            this._ultraGridPrintDocument.SettingsKey = "View_Main_SecuritySummary._ultraGridPrintDocument";
            // 
            // _cmsi_Grid_ColumnChooser
            // 
            this._cmsi_Grid_ColumnChooser.Name = "_cmsi_Grid_ColumnChooser";
            this._cmsi_Grid_ColumnChooser.Size = new System.Drawing.Size(157, 22);
            this._cmsi_Grid_ColumnChooser.Text = "Select Columns";
            this._cmsi_Grid_ColumnChooser.Click += new System.EventHandler(this._cmsi_grid_ColumnChooser_Click);
            // 
            // _cmsi_Grid_viewGroupByBox
            // 
            this._cmsi_Grid_viewGroupByBox.Name = "_cmsi_Grid_viewGroupByBox";
            this._cmsi_Grid_viewGroupByBox.Size = new System.Drawing.Size(157, 22);
            this._cmsi_Grid_viewGroupByBox.Text = "Group By Box";
            this._cmsi_Grid_viewGroupByBox.Click += new System.EventHandler(this._cmsi_grid_viewGroupByBox_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(154, 6);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(154, 6);
            // 
            // _cmsi_Grid_Save
            // 
            this._cmsi_Grid_Save.Name = "_cmsi_Grid_Save";
            this._cmsi_Grid_Save.Size = new System.Drawing.Size(157, 22);
            this._cmsi_Grid_Save.Text = "Save to Excel";
            this._cmsi_Grid_Save.Click += new System.EventHandler(this._cmsi_grid_Save_Click);
            // 
            // _cmsi_Grid_Print
            // 
            this._cmsi_Grid_Print.Name = "_cmsi_Grid_Print";
            this._cmsi_Grid_Print.Size = new System.Drawing.Size(157, 22);
            this._cmsi_Grid_Print.Text = "Print";
            this._cmsi_Grid_Print.Click += new System.EventHandler(this._cmsi_grid_Print_Click);
            // 
            // View_SQLsecureActivity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "View_SQLsecureActivity";
            this.Enter += new System.EventHandler(this.View_SQLsecureActivity_Enter);
            this.Leave += new System.EventHandler(this.View_SQLsecureActivity_Leave);
            this.Controls.SetChildIndex(this._vw_TasksPanel, 0);
            this.Controls.SetChildIndex(this._vw_MainPanel, 0);
            this._vw_MainPanel.ResumeLayout(false);
            this._vw_TasksPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
            this._contextMenuStrip.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this._headerStrip_Activity.ResumeLayout(false);
            this._headerStrip_Activity.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinGrid.UltraGrid _grid;
        private System.Windows.Forms.ContextMenuStrip _contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem _cmsi_refresh;
        private System.Windows.Forms.Panel panel1;
        private Idera.SQLsecure.UI.Console.Controls.HeaderStrip _headerStrip_Activity;
        private System.Windows.Forms.ToolStripLabel _label_Activity;
        private System.Windows.Forms.ToolStripButton _toolStripButton_ActivityPrint;
        private System.Windows.Forms.ToolStripButton _toolStripButton_ActivitySave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton _toolStripButton_ActivityGroupBy;
        private System.Windows.Forms.ToolStripButton _toolStripButton_ActivityColumnChooser;
        private System.Windows.Forms.SaveFileDialog _saveFileDialog;
        private Infragistics.Win.Printing.UltraPrintPreviewDialog _ultraPrintPreviewDialog;
        private Infragistics.Win.UltraWinGrid.UltraGridPrintDocument _ultraGridPrintDocument;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter _ultraGridExcelExporter;
        private System.Windows.Forms.ToolStripMenuItem _cmsi_Grid_ColumnChooser;
        private System.Windows.Forms.ToolStripMenuItem _cmsi_Grid_viewGroupByBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem _cmsi_Grid_Save;
        private System.Windows.Forms.ToolStripMenuItem _cmsi_Grid_Print;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}
