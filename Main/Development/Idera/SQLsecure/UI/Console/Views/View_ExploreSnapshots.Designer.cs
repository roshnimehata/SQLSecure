namespace Idera.SQLsecure.UI.Console.Views
{
    partial class View_ExploreSnapshots
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(View_ExploreSnapshots));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
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
            this._contextMenuStrip_Snapshot = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._cmsi_Snapshot_exploreUserPermissions = new System.Windows.Forms.ToolStripMenuItem();
            this._cmsi_Snapshot_exploreSnapshot = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this._cmsi_Snapshot_collectData = new System.Windows.Forms.ToolStripMenuItem();
            this._cmsi_Snapshot_baselineSnapshot = new System.Windows.Forms.ToolStripMenuItem();
            this._cmsi_Snapshot_deleteSnapshot = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this._cmsi_Snapshot_refresh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this._cmsi_Snapshot_properties = new System.Windows.Forms.ToolStripMenuItem();
            this._imageList = new System.Windows.Forms.ImageList(this.components);
            this._grid = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this._vw_MainPanel.SuspendLayout();
            this._vw_TasksPanel.SuspendLayout();
            this._contextMenuStrip_Snapshot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
            this.SuspendLayout();
            // 
            // _vw_MainPanel
            // 
            this._vw_MainPanel.Controls.Add(this._grid);
            this._vw_MainPanel.Controls.SetChildIndex(this._grid, 0);
            // 
            // _contextMenuStrip_Snapshot
            // 
            this._contextMenuStrip_Snapshot.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._cmsi_Snapshot_exploreUserPermissions,
            this._cmsi_Snapshot_exploreSnapshot,
            this.toolStripSeparator4,
            this._cmsi_Snapshot_collectData,
            this._cmsi_Snapshot_baselineSnapshot,
            this._cmsi_Snapshot_deleteSnapshot,
            this.toolStripSeparator5,
            this._cmsi_Snapshot_refresh,
            this.toolStripSeparator6,
            this._cmsi_Snapshot_properties});
            this._contextMenuStrip_Snapshot.Name = "_contextMenuStrip_Snapshot";
            this._contextMenuStrip_Snapshot.Size = new System.Drawing.Size(204, 176);
            this._contextMenuStrip_Snapshot.Opening += new System.ComponentModel.CancelEventHandler(this._contextMenuStrip_Snapshot_Opening);
            // 
            // _cmsi_Snapshot_exploreUserPermissions
            // 
            this._cmsi_Snapshot_exploreUserPermissions.Name = "_cmsi_Snapshot_exploreUserPermissions";
            this._cmsi_Snapshot_exploreUserPermissions.Size = new System.Drawing.Size(203, 22);
            this._cmsi_Snapshot_exploreUserPermissions.Text = "Explore user permissions";
            this._cmsi_Snapshot_exploreUserPermissions.Click += new System.EventHandler(this._cmsi_Snapshot_exploreUserPermissions_Click);
            // 
            // _cmsi_Snapshot_exploreSnapshot
            // 
            this._cmsi_Snapshot_exploreSnapshot.Name = "_cmsi_Snapshot_exploreSnapshot";
            this._cmsi_Snapshot_exploreSnapshot.Size = new System.Drawing.Size(203, 22);
            this._cmsi_Snapshot_exploreSnapshot.Text = "Explore snapshot";
            this._cmsi_Snapshot_exploreSnapshot.Click += new System.EventHandler(this._cmsi_Snapshot_exploreSnapshot_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(200, 6);
            // 
            // _cmsi_Snapshot_collectData
            // 
            this._cmsi_Snapshot_collectData.Name = "_cmsi_Snapshot_collectData";
            this._cmsi_Snapshot_collectData.Size = new System.Drawing.Size(203, 22);
            this._cmsi_Snapshot_collectData.Text = "Collect data snapshot";
            this._cmsi_Snapshot_collectData.Click += new System.EventHandler(this._cmsi_Snapshot_collectData_Click);
            // 
            // _cmsi_Snapshot_baselineSnapshot
            // 
            this._cmsi_Snapshot_baselineSnapshot.Name = "_cmsi_Snapshot_baselineSnapshot";
            this._cmsi_Snapshot_baselineSnapshot.Size = new System.Drawing.Size(203, 22);
            this._cmsi_Snapshot_baselineSnapshot.Text = "Baseline snapshot";
            this._cmsi_Snapshot_baselineSnapshot.Click += new System.EventHandler(this._cmsi_Snapshot_baselineSnapshot_Click);
            // 
            // _cmsi_Snapshot_deleteSnapshot
            // 
            this._cmsi_Snapshot_deleteSnapshot.Name = "_cmsi_Snapshot_deleteSnapshot";
            this._cmsi_Snapshot_deleteSnapshot.Size = new System.Drawing.Size(203, 22);
            this._cmsi_Snapshot_deleteSnapshot.Text = "Delete snapshot(s)";
            this._cmsi_Snapshot_deleteSnapshot.Click += new System.EventHandler(this._cmsi_Snapshot_deleteSnapshot_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(200, 6);
            // 
            // _cmsi_Snapshot_refresh
            // 
            this._cmsi_Snapshot_refresh.Name = "_cmsi_Snapshot_refresh";
            this._cmsi_Snapshot_refresh.Size = new System.Drawing.Size(203, 22);
            this._cmsi_Snapshot_refresh.Text = "Refresh";
            this._cmsi_Snapshot_refresh.Click += new System.EventHandler(this._cmsi_Snapshot_refresh_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(200, 6);
            // 
            // _cmsi_Snapshot_properties
            // 
            this._cmsi_Snapshot_properties.Name = "_cmsi_Snapshot_properties";
            this._cmsi_Snapshot_properties.Size = new System.Drawing.Size(203, 22);
            this._cmsi_Snapshot_properties.Text = "Properties";
            this._cmsi_Snapshot_properties.Click += new System.EventHandler(this._cmsi_Snapshot_properties_Click);
            // 
            // _imageList
            // 
            this._imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_imageList.ImageStream")));
            this._imageList.TransparentColor = System.Drawing.Color.Transparent;
            this._imageList.Images.SetKeyName(0, "Snapshot.ico");
            this._imageList.Images.SetKeyName(1, "Snapshot_Error.bmp");
            this._imageList.Images.SetKeyName(2, "Snapshot_Baselined.bmp");
            this._imageList.Images.SetKeyName(3, "clock_icon.jpg");
            // 
            // _grid
            // 
            this._grid.ContextMenuStrip = this._contextMenuStrip_Snapshot;
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this._grid.DisplayLayout.Appearance = appearance1;
            this._grid.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
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
            appearance10.TextHAlign = Infragistics.Win.HAlign.Left;
            this._grid.DisplayLayout.Override.HeaderAppearance = appearance10;
            this._grid.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this._grid.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this._grid.DisplayLayout.Override.RowAlternateAppearance = appearance11;
            appearance12.BorderColor = System.Drawing.Color.LightGray;
            appearance12.TextVAlign = Infragistics.Win.VAlign.Middle;
            this._grid.DisplayLayout.Override.RowAppearance = appearance12;
            this._grid.DisplayLayout.Override.RowFilterAction = Infragistics.Win.UltraWinGrid.RowFilterAction.HideFilteredOutRows;
            this._grid.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance13.BackColor = System.Drawing.SystemColors.Highlight;
            appearance13.BorderColor = System.Drawing.Color.Black;
            appearance13.ForeColor = System.Drawing.SystemColors.HighlightText;
            this._grid.DisplayLayout.Override.SelectedRowAppearance = appearance13;
            this._grid.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this._grid.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this._grid.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.ExtendedAutoDrag;
            this._grid.DisplayLayout.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.None;
            appearance14.BackColor = System.Drawing.SystemColors.ControlLight;
            this._grid.DisplayLayout.Override.TemplateAddRowAppearance = appearance14;
            this._grid.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.None;
            this._grid.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this._grid.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this._grid.DisplayLayout.TabNavigation = Infragistics.Win.UltraWinGrid.TabNavigation.NextControl;
            this._grid.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this._grid.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grid.ImageList = this._imageList;
            this._grid.Location = new System.Drawing.Point(0, 25);
            this._grid.Name = "_grid";
            this._grid.Size = new System.Drawing.Size(652, 463);
            this._grid.TabIndex = 1;
            this._grid.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this._grid_AfterSelectChange);
            this._grid.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this._grid_InitializeLayout);
            this._grid.Enter += new System.EventHandler(this._grid_Enter);
            this._grid.Leave += new System.EventHandler(this._grid_Leave);
            // 
            // View_ExploreSnapshots
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "View_ExploreSnapshots";
            this.Enter += new System.EventHandler(this.View_ExploreSnapshots_Enter);
            this.Leave += new System.EventHandler(this.View_ExploreSnapshots_Leave);
            this.Controls.SetChildIndex(this._vw_TasksPanel, 0);
            this.Controls.SetChildIndex(this._vw_MainPanel, 0);
            this._vw_MainPanel.ResumeLayout(false);
            this._vw_TasksPanel.ResumeLayout(false);
            this._contextMenuStrip_Snapshot.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.ContextMenuStrip _contextMenuStrip_Snapshot;
        private System.Windows.Forms.ToolStripMenuItem _cmsi_Snapshot_exploreUserPermissions;
        private System.Windows.Forms.ToolStripMenuItem _cmsi_Snapshot_exploreSnapshot;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem _cmsi_Snapshot_collectData;
        private System.Windows.Forms.ToolStripMenuItem _cmsi_Snapshot_baselineSnapshot;
        private System.Windows.Forms.ToolStripMenuItem _cmsi_Snapshot_deleteSnapshot;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem _cmsi_Snapshot_refresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem _cmsi_Snapshot_properties;
        private System.Windows.Forms.ImageList _imageList;
        private Infragistics.Win.UltraWinGrid.UltraGrid _grid;

    }
}
