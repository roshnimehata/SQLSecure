namespace Idera.SQLsecure.UI.Console.Controls
{
    partial class Snapshots
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Snapshots));
            this._grid = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this._contextMenuStrip_Snapshot = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._cmsi_Snapshot_exploreUserPermissions = new System.Windows.Forms.ToolStripMenuItem();
            this._cmsi_Snapshot_exploreSnapshot = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this._cmsi_Snapshot_baselineSnapshot = new System.Windows.Forms.ToolStripMenuItem();
            this._cmsi_Snapshot_deleteSnapshot = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this._cmsi_Snapshot_groupBy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this._cmsi_Snapshot_Save = new System.Windows.Forms.ToolStripMenuItem();
            this._cmsi_Snapshot_Print = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this._cmsi_Snapshot_refresh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this._cmsi_Snapshot_properties = new System.Windows.Forms.ToolStripMenuItem();
            this._ultraGridExcelExporter = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this._ultraPrintPreviewDialog = new Infragistics.Win.Printing.UltraPrintPreviewDialog(this.components);
            this._ultraGridPrintDocument = new Infragistics.Win.UltraWinGrid.UltraGridPrintDocument(this.components);
            this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this._headerStrip = new Idera.SQLsecure.UI.Console.Controls.HeaderStrip();
            this._label_Snapshot = new System.Windows.Forms.ToolStripLabel();
            this._toolStripButton_GridPrint = new System.Windows.Forms.ToolStripButton();
            this._toolStripButton_GridSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._toolStripButton_GridGroupBy = new System.Windows.Forms.ToolStripButton();
            this._toolStripButton_GridColumnChooser = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
            this._contextMenuStrip_Snapshot.SuspendLayout();
            this._headerStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _grid
            // 
            this._grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._grid.ContextMenuStrip = this._contextMenuStrip_Snapshot;
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this._grid.DisplayLayout.Appearance = appearance1;
            this._grid.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand1.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            ultraGridBand1.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            ultraGridBand1.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            ultraGridBand1.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended;
            this._grid.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this._grid.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this._grid.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            appearance2.BackColor2 = System.Drawing.Color.Transparent;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this._grid.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this._grid.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this._grid.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.BackColor2 = System.Drawing.Color.Transparent;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this._grid.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this._grid.DisplayLayout.MaxColScrollRegions = 1;
            this._grid.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this._grid.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.Color.White;
            appearance6.ForeColor = System.Drawing.Color.Black;
            this._grid.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this._grid.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this._grid.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this._grid.DisplayLayout.Override.AllowMultiCellOperations = Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.None;
            this._grid.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            this._grid.DisplayLayout.Override.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.False;
            this._grid.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this._grid.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this._grid.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.Color.Transparent;
            this._grid.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this._grid.DisplayLayout.Override.CellAppearance = appearance8;
            this._grid.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this._grid.DisplayLayout.Override.CellPadding = 0;
            this._grid.DisplayLayout.Override.ColumnAutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.AllRowsInBand;
            this._grid.DisplayLayout.Override.FilterEvaluationTrigger = Infragistics.Win.UltraWinGrid.FilterEvaluationTrigger.OnCellValueChange;
            this._grid.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.HeaderIcons;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this._grid.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this._grid.DisplayLayout.Override.HeaderAppearance = appearance10;
            this._grid.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this._grid.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this._grid.DisplayLayout.Override.RowAlternateAppearance = appearance11;
            appearance12.BorderColor = System.Drawing.Color.LightGray;
            appearance12.TextVAlignAsString = "Middle";
            this._grid.DisplayLayout.Override.RowAppearance = appearance12;
            this._grid.DisplayLayout.Override.RowFilterAction = Infragistics.Win.UltraWinGrid.RowFilterAction.HideFilteredOutRows;
            this._grid.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance13.BackColor = System.Drawing.SystemColors.Highlight;
            appearance13.BorderColor = System.Drawing.Color.Black;
            appearance13.ForeColor = System.Drawing.SystemColors.HighlightText;
            this._grid.DisplayLayout.Override.SelectedRowAppearance = appearance13;
            this._grid.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this._grid.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this._grid.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended;
            this._grid.DisplayLayout.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.None;
            appearance14.BackColor = System.Drawing.SystemColors.ControlLight;
            this._grid.DisplayLayout.Override.TemplateAddRowAppearance = appearance14;
            this._grid.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.None;
            this._grid.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this._grid.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this._grid.DisplayLayout.TabNavigation = Infragistics.Win.UltraWinGrid.TabNavigation.NextControl;
            this._grid.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this._grid.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this._grid.Location = new System.Drawing.Point(0, 19);
            this._grid.Name = "_grid";
            this._grid.Size = new System.Drawing.Size(640, 431);
            this._grid.TabIndex = 2;
            this._grid.MouseDown += new System.Windows.Forms.MouseEventHandler(this._grid_MouseDown);
            this._grid.DoubleClick += new System.EventHandler(this._grid_DoubleClick);
            this._grid.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this._grid_AfterSelectChange);
            this._grid.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this._grid_InitializeLayout);
            this._grid.AfterRowActivate += new System.EventHandler(this._grid_AfterRowActivate);
            this._grid.Enter += new System.EventHandler(this._grid_Enter);
            this._grid.Leave += new System.EventHandler(this._grid_Leave);
            // 
            // _contextMenuStrip_Snapshot
            // 
            this._contextMenuStrip_Snapshot.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._cmsi_Snapshot_exploreUserPermissions,
            this._cmsi_Snapshot_exploreSnapshot,
            this.toolStripSeparator4,
            this._cmsi_Snapshot_baselineSnapshot,
            this._cmsi_Snapshot_deleteSnapshot,
            this.toolStripSeparator2,
            this._cmsi_Snapshot_groupBy,
            this.toolStripSeparator3,
            this._cmsi_Snapshot_Save,
            this._cmsi_Snapshot_Print,
            this.toolStripSeparator5,
            this._cmsi_Snapshot_refresh,
            this.toolStripSeparator6,
            this._cmsi_Snapshot_properties});
            this._contextMenuStrip_Snapshot.Name = "_contextMenuStrip_Snapshot";
            this._contextMenuStrip_Snapshot.Size = new System.Drawing.Size(215, 232);
            this._contextMenuStrip_Snapshot.Opening += new System.ComponentModel.CancelEventHandler(this._contextMenuStrip_Snapshot_Opening);
            // 
            // _cmsi_Snapshot_exploreUserPermissions
            // 
            this._cmsi_Snapshot_exploreUserPermissions.Name = "_cmsi_Snapshot_exploreUserPermissions";
            this._cmsi_Snapshot_exploreUserPermissions.Size = new System.Drawing.Size(214, 22);
            this._cmsi_Snapshot_exploreUserPermissions.Text = "Explore User Permissions";
            this._cmsi_Snapshot_exploreUserPermissions.Click += new System.EventHandler(this._cmsi_Snapshot_exploreUserPermissions_Click);
            // 
            // _cmsi_Snapshot_exploreSnapshot
            // 
            this._cmsi_Snapshot_exploreSnapshot.Name = "_cmsi_Snapshot_exploreSnapshot";
            this._cmsi_Snapshot_exploreSnapshot.Size = new System.Drawing.Size(214, 22);
            this._cmsi_Snapshot_exploreSnapshot.Text = "Explore Object Permissions";
            this._cmsi_Snapshot_exploreSnapshot.Click += new System.EventHandler(this._cmsi_Snapshot_exploreSnapshot_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(211, 6);
            // 
            // _cmsi_Snapshot_baselineSnapshot
            // 
            this._cmsi_Snapshot_baselineSnapshot.Name = "_cmsi_Snapshot_baselineSnapshot";
            this._cmsi_Snapshot_baselineSnapshot.Size = new System.Drawing.Size(214, 22);
            this._cmsi_Snapshot_baselineSnapshot.Text = "Mark as Baseline...";
            this._cmsi_Snapshot_baselineSnapshot.Click += new System.EventHandler(this._cmsi_Snapshot_baselineSnapshot_Click);
            // 
            // _cmsi_Snapshot_deleteSnapshot
            // 
            this._cmsi_Snapshot_deleteSnapshot.Name = "_cmsi_Snapshot_deleteSnapshot";
            this._cmsi_Snapshot_deleteSnapshot.Size = new System.Drawing.Size(214, 22);
            this._cmsi_Snapshot_deleteSnapshot.Text = "Delete Snapshot...";
            this._cmsi_Snapshot_deleteSnapshot.Click += new System.EventHandler(this._cmsi_Snapshot_deleteSnapshot_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(211, 6);
            // 
            // _cmsi_Snapshot_groupBy
            // 
            this._cmsi_Snapshot_groupBy.Name = "_cmsi_Snapshot_groupBy";
            this._cmsi_Snapshot_groupBy.Size = new System.Drawing.Size(214, 22);
            this._cmsi_Snapshot_groupBy.Text = "Group By Box";
            this._cmsi_Snapshot_groupBy.Click += new System.EventHandler(this._cmsi_Snapshot_groupBy_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(211, 6);
            // 
            // _cmsi_Snapshot_Save
            // 
            this._cmsi_Snapshot_Save.Name = "_cmsi_Snapshot_Save";
            this._cmsi_Snapshot_Save.Size = new System.Drawing.Size(214, 22);
            this._cmsi_Snapshot_Save.Text = "Save to Excel";
            this._cmsi_Snapshot_Save.Click += new System.EventHandler(this._cmsi_Snapshot_Save_Click);
            // 
            // _cmsi_Snapshot_Print
            // 
            this._cmsi_Snapshot_Print.Name = "_cmsi_Snapshot_Print";
            this._cmsi_Snapshot_Print.Size = new System.Drawing.Size(214, 22);
            this._cmsi_Snapshot_Print.Text = "Print";
            this._cmsi_Snapshot_Print.Click += new System.EventHandler(this._cmsi_Snapshot_Print_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(211, 6);
            // 
            // _cmsi_Snapshot_refresh
            // 
            this._cmsi_Snapshot_refresh.Name = "_cmsi_Snapshot_refresh";
            this._cmsi_Snapshot_refresh.Size = new System.Drawing.Size(214, 22);
            this._cmsi_Snapshot_refresh.Text = "Refresh";
            this._cmsi_Snapshot_refresh.Click += new System.EventHandler(this._cmsi_Snapshot_refresh_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(211, 6);
            // 
            // _cmsi_Snapshot_properties
            // 
            this._cmsi_Snapshot_properties.Name = "_cmsi_Snapshot_properties";
            this._cmsi_Snapshot_properties.Size = new System.Drawing.Size(214, 22);
            this._cmsi_Snapshot_properties.Text = "Properties...";
            this._cmsi_Snapshot_properties.Click += new System.EventHandler(this._cmsi_Snapshot_properties_Click);
            // 
            // _ultraPrintPreviewDialog
            // 
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
            // _headerStrip
            // 
            this._headerStrip.AutoSize = false;
            this._headerStrip.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this._headerStrip.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(66)))), ((int)(((byte)(139)))));
            this._headerStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._headerStrip.HeaderStyle = Idera.SQLsecure.UI.Console.Controls.AreaHeaderStyle.Small;
            this._headerStrip.HotTrackEnabled = false;
            this._headerStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._label_Snapshot,
            this._toolStripButton_GridPrint,
            this._toolStripButton_GridSave,
            this.toolStripSeparator1,
            this._toolStripButton_GridGroupBy,
            this._toolStripButton_GridColumnChooser});
            this._headerStrip.Location = new System.Drawing.Point(0, 0);
            this._headerStrip.Name = "_headerStrip";
            this._headerStrip.Size = new System.Drawing.Size(640, 19);
            this._headerStrip.TabIndex = 13;
            // 
            // _label_Snapshot
            // 
            this._label_Snapshot.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._label_Snapshot.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this._label_Snapshot.Name = "_label_Snapshot";
            this._label_Snapshot.Size = new System.Drawing.Size(91, 16);
            this._label_Snapshot.Text = "Server Snapshots";
            // 
            // _toolStripButton_GridPrint
            // 
            this._toolStripButton_GridPrint.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._toolStripButton_GridPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._toolStripButton_GridPrint.Image = ((System.Drawing.Image)(resources.GetObject("_toolStripButton_GridPrint.Image")));
            this._toolStripButton_GridPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolStripButton_GridPrint.Margin = new System.Windows.Forms.Padding(0);
            this._toolStripButton_GridPrint.Name = "_toolStripButton_GridPrint";
            this._toolStripButton_GridPrint.Size = new System.Drawing.Size(23, 19);
            this._toolStripButton_GridPrint.Text = "Print";
            this._toolStripButton_GridPrint.Click += new System.EventHandler(this._toolStripButton_GridPrint_Click);
            // 
            // _toolStripButton_GridSave
            // 
            this._toolStripButton_GridSave.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._toolStripButton_GridSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._toolStripButton_GridSave.Image = ((System.Drawing.Image)(resources.GetObject("_toolStripButton_GridSave.Image")));
            this._toolStripButton_GridSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolStripButton_GridSave.Margin = new System.Windows.Forms.Padding(0);
            this._toolStripButton_GridSave.Name = "_toolStripButton_GridSave";
            this._toolStripButton_GridSave.Size = new System.Drawing.Size(23, 19);
            this._toolStripButton_GridSave.Text = "Save";
            this._toolStripButton_GridSave.Click += new System.EventHandler(this._toolStripButton_GridSave_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 19);
            // 
            // _toolStripButton_GridGroupBy
            // 
            this._toolStripButton_GridGroupBy.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._toolStripButton_GridGroupBy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._toolStripButton_GridGroupBy.Image = ((System.Drawing.Image)(resources.GetObject("_toolStripButton_GridGroupBy.Image")));
            this._toolStripButton_GridGroupBy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolStripButton_GridGroupBy.Margin = new System.Windows.Forms.Padding(0);
            this._toolStripButton_GridGroupBy.Name = "_toolStripButton_GridGroupBy";
            this._toolStripButton_GridGroupBy.Size = new System.Drawing.Size(23, 19);
            this._toolStripButton_GridGroupBy.Text = "Group By Box";
            this._toolStripButton_GridGroupBy.Click += new System.EventHandler(this._toolStripButton_GridGroupBy_Click);
            // 
            // _toolStripButton_GridColumnChooser
            // 
            this._toolStripButton_GridColumnChooser.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._toolStripButton_GridColumnChooser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._toolStripButton_GridColumnChooser.Image = ((System.Drawing.Image)(resources.GetObject("_toolStripButton_GridColumnChooser.Image")));
            this._toolStripButton_GridColumnChooser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolStripButton_GridColumnChooser.Margin = new System.Windows.Forms.Padding(0);
            this._toolStripButton_GridColumnChooser.Name = "_toolStripButton_GridColumnChooser";
            this._toolStripButton_GridColumnChooser.Size = new System.Drawing.Size(23, 19);
            this._toolStripButton_GridColumnChooser.Text = "Select Columns";
            this._toolStripButton_GridColumnChooser.ToolTipText = "Select Columns";
            this._toolStripButton_GridColumnChooser.Click += new System.EventHandler(this._toolStripButton_GridColumnChooser_Click);
            // 
            // Snapshots
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._headerStrip);
            this.Controls.Add(this._grid);
            this.Name = "Snapshots";
            this.Size = new System.Drawing.Size(640, 450);
            this.Enter += new System.EventHandler(this.Snapshots_Enter);
            this.VisibleChanged += new System.EventHandler(this.Snapshots_VisibleChanged);
            this.Leave += new System.EventHandler(this.Snapshots_Leave);
            ((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
            this._contextMenuStrip_Snapshot.ResumeLayout(false);
            this._headerStrip.ResumeLayout(false);
            this._headerStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinGrid.UltraGrid _grid;
        internal System.Windows.Forms.ContextMenuStrip _contextMenuStrip_Snapshot;
        private System.Windows.Forms.ToolStripMenuItem _cmsi_Snapshot_exploreUserPermissions;
        private System.Windows.Forms.ToolStripMenuItem _cmsi_Snapshot_exploreSnapshot;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem _cmsi_Snapshot_baselineSnapshot;
        private System.Windows.Forms.ToolStripMenuItem _cmsi_Snapshot_deleteSnapshot;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem _cmsi_Snapshot_refresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem _cmsi_Snapshot_properties;
        private HeaderStrip _headerStrip;
        private System.Windows.Forms.ToolStripLabel _label_Snapshot;
        private System.Windows.Forms.ToolStripButton _toolStripButton_GridPrint;
        private System.Windows.Forms.ToolStripButton _toolStripButton_GridSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton _toolStripButton_GridGroupBy;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter _ultraGridExcelExporter;
        private Infragistics.Win.Printing.UltraPrintPreviewDialog _ultraPrintPreviewDialog;
        private Infragistics.Win.UltraWinGrid.UltraGridPrintDocument _ultraGridPrintDocument;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem _cmsi_Snapshot_groupBy;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem _cmsi_Snapshot_Save;
        private System.Windows.Forms.ToolStripMenuItem _cmsi_Snapshot_Print;
        private System.Windows.Forms.SaveFileDialog _saveFileDialog;
        private System.Windows.Forms.ToolStripButton _toolStripButton_GridColumnChooser;
    }
}
