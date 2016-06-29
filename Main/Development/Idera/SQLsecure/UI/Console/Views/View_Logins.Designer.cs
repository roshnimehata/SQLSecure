namespace Idera.SQLsecure.UI.Console.Views
{
    partial class View_Logins
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(View_Logins));
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
            this._smallTask_NewLogin = new Idera.SQLsecure.UI.Console.Controls.SmallTask();
            this._grid_Logins = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.contextMenuStripLogins = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newLoginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this._toolstrip = new Idera.SQLsecure.UI.Console.Controls.HeaderStrip();
            this.label_Header = new System.Windows.Forms.ToolStripLabel();
            this._vw_MainPanel.SuspendLayout();
            this._vw_TasksPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid_Logins)).BeginInit();
            this.contextMenuStripLogins.SuspendLayout();
            this.panel1.SuspendLayout();
            this._toolstrip.SuspendLayout();
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
            this._vw_TasksPanel.Controls.Add(this._smallTask_NewLogin);
            this._vw_TasksPanel.Size = new System.Drawing.Size(652, 72);
            this._vw_TasksPanel.Controls.SetChildIndex(this._smallTask_Help, 0);
            this._vw_TasksPanel.Controls.SetChildIndex(this._smallTask_NewLogin, 0);
            this._vw_TasksPanel.Controls.SetChildIndex(this._label_Summary, 0);
            // 
            // _smallTask_Help
            // 
            this._smallTask_Help.Location = new System.Drawing.Point(522, 30);
            this._smallTask_Help.Size = new System.Drawing.Size(120, 32);
            // 
            // _label_Summary
            // 
            this._label_Summary.Location = new System.Drawing.Point(8, 5);
            this._label_Summary.Size = new System.Drawing.Size(636, 14);
            // 
            // _smallTask_NewLogin
            // 
            this._smallTask_NewLogin.BackColor = System.Drawing.Color.Transparent;
            this._smallTask_NewLogin.Location = new System.Drawing.Point(11, 27);
            this._smallTask_NewLogin.Name = "_smallTask_NewLogin";
            this._smallTask_NewLogin.Size = new System.Drawing.Size(234, 34);
            this._smallTask_NewLogin.TabIndex = 4;
            this._smallTask_NewLogin.TaskDescription = "";
            this._smallTask_NewLogin.TaskImage = ((System.Drawing.Image)(resources.GetObject("_smallTask_NewLogin.TaskImage")));
            this._smallTask_NewLogin.TaskText = "Login";
            // 
            // _grid_Logins
            // 
            this._grid_Logins.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._grid_Logins.ContextMenuStrip = this.contextMenuStripLogins;
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this._grid_Logins.DisplayLayout.Appearance = appearance1;
            this._grid_Logins.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand1.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            ultraGridBand1.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            ultraGridBand1.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            ultraGridBand1.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this._grid_Logins.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this._grid_Logins.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this._grid_Logins.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            appearance2.BackColor2 = System.Drawing.Color.Transparent;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this._grid_Logins.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this._grid_Logins.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this._grid_Logins.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.BackColor2 = System.Drawing.Color.Transparent;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this._grid_Logins.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this._grid_Logins.DisplayLayout.MaxColScrollRegions = 1;
            this._grid_Logins.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this._grid_Logins.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.Color.White;
            appearance6.ForeColor = System.Drawing.Color.Black;
            this._grid_Logins.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this._grid_Logins.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this._grid_Logins.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this._grid_Logins.DisplayLayout.Override.AllowMultiCellOperations = Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.None;
            this._grid_Logins.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            this._grid_Logins.DisplayLayout.Override.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.False;
            this._grid_Logins.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this._grid_Logins.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this._grid_Logins.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.Color.Transparent;
            this._grid_Logins.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this._grid_Logins.DisplayLayout.Override.CellAppearance = appearance8;
            this._grid_Logins.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this._grid_Logins.DisplayLayout.Override.CellPadding = 0;
            this._grid_Logins.DisplayLayout.Override.ColumnAutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.AllRowsInBand;
            this._grid_Logins.DisplayLayout.Override.FilterEvaluationTrigger = Infragistics.Win.UltraWinGrid.FilterEvaluationTrigger.OnCellValueChange;
            this._grid_Logins.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.HeaderIcons;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this._grid_Logins.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this._grid_Logins.DisplayLayout.Override.HeaderAppearance = appearance10;
            this._grid_Logins.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this._grid_Logins.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this._grid_Logins.DisplayLayout.Override.RowAlternateAppearance = appearance11;
            appearance12.BorderColor = System.Drawing.Color.LightGray;
            appearance12.TextVAlignAsString = "Middle";
            this._grid_Logins.DisplayLayout.Override.RowAppearance = appearance12;
            this._grid_Logins.DisplayLayout.Override.RowFilterAction = Infragistics.Win.UltraWinGrid.RowFilterAction.HideFilteredOutRows;
            this._grid_Logins.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance13.BackColor = System.Drawing.SystemColors.Highlight;
            appearance13.BorderColor = System.Drawing.Color.Black;
            appearance13.ForeColor = System.Drawing.SystemColors.HighlightText;
            this._grid_Logins.DisplayLayout.Override.SelectedRowAppearance = appearance13;
            this._grid_Logins.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this._grid_Logins.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this._grid_Logins.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this._grid_Logins.DisplayLayout.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.None;
            appearance14.BackColor = System.Drawing.SystemColors.ControlLight;
            this._grid_Logins.DisplayLayout.Override.TemplateAddRowAppearance = appearance14;
            this._grid_Logins.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.None;
            this._grid_Logins.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this._grid_Logins.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this._grid_Logins.DisplayLayout.TabNavigation = Infragistics.Win.UltraWinGrid.TabNavigation.NextControl;
            this._grid_Logins.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this._grid_Logins.Location = new System.Drawing.Point(0, 21);
            this._grid_Logins.Name = "_grid_Logins";
            this._grid_Logins.Size = new System.Drawing.Size(648, 488);
            this._grid_Logins.TabIndex = 2;
            this._grid_Logins.MouseDown += new System.Windows.Forms.MouseEventHandler(this._grid_Logins_MouseDown);
            this._grid_Logins.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this._grid_Logins_InitializeLayout);
            this._grid_Logins.DoubleClickCell += new Infragistics.Win.UltraWinGrid.DoubleClickCellEventHandler(this._grid_Logins_DoubleClickCell);
            // 
            // contextMenuStripLogins
            // 
            this.contextMenuStripLogins.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newLoginToolStripMenuItem,
            this.toolStripSeparator1,
            this.deleteToolStripMenuItem,
            this.refreshToolStripMenuItem,
            this.toolStripSeparator2,
            this.propertiesToolStripMenuItem});
            this.contextMenuStripLogins.Name = "contextMenuStrip1";
            this.contextMenuStripLogins.Size = new System.Drawing.Size(147, 104);
            this.contextMenuStripLogins.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripLogins_Opening);
            // 
            // newLoginToolStripMenuItem
            // 
            this.newLoginToolStripMenuItem.Name = "newLoginToolStripMenuItem";
            this.newLoginToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.newLoginToolStripMenuItem.Text = "New Login...";
            this.newLoginToolStripMenuItem.Click += new System.EventHandler(this.newLoginToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(143, 6);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(143, 6);
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.propertiesToolStripMenuItem.Text = "Properties...";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this._toolstrip);
            this.panel1.Controls.Add(this._grid_Logins);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(652, 516);
            this.panel1.TabIndex = 3;
            // 
            // _toolstrip
            // 
            this._toolstrip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._toolstrip.AutoSize = false;
            this._toolstrip.Dock = System.Windows.Forms.DockStyle.None;
            this._toolstrip.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this._toolstrip.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(66)))), ((int)(((byte)(139)))));
            this._toolstrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._toolstrip.HeaderStyle = Idera.SQLsecure.UI.Console.Controls.AreaHeaderStyle.Small;
            this._toolstrip.HotTrackEnabled = false;
            this._toolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.label_Header});
            this._toolstrip.Location = new System.Drawing.Point(0, 0);
            this._toolstrip.Name = "_toolstrip";
            this._toolstrip.Size = new System.Drawing.Size(648, 21);
            this._toolstrip.TabIndex = 5;
            this._toolstrip.Text = "headerStrip1";
            // 
            // label_Header
            // 
            this.label_Header.Name = "label_Header";
            this.label_Header.Size = new System.Drawing.Size(70, 18);
            this.label_Header.Text = "Active Logins";
            // 
            // View_Logins
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "View_Logins";
            this.Controls.SetChildIndex(this._vw_TasksPanel, 0);
            this.Controls.SetChildIndex(this._vw_MainPanel, 0);
            this._vw_MainPanel.ResumeLayout(false);
            this._vw_TasksPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._grid_Logins)).EndInit();
            this.contextMenuStripLogins.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this._toolstrip.ResumeLayout(false);
            this._toolstrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Idera.SQLsecure.UI.Console.Controls.SmallTask _smallTask_NewLogin;
        private Infragistics.Win.UltraWinGrid.UltraGrid _grid_Logins;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripLogins;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem newLoginToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private Idera.SQLsecure.UI.Console.Controls.HeaderStrip _toolstrip;
        private System.Windows.Forms.ToolStripLabel label_Header;
    }
}
