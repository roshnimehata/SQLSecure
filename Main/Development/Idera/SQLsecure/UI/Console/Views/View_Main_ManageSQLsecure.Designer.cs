namespace Idera.SQLsecure.UI.Console.Views
{
    partial class View_Main_ManageSQLsecure
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(View_Main_ManageSQLsecure));
            this._grid = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this._contextMenuStrip_Server = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._cmsi_Server_exploreUserPermissions = new System.Windows.Forms.ToolStripMenuItem();
            this._cmsi_Server_exploreSnapshot = new System.Windows.Forms.ToolStripMenuItem();
            this._cmsi_Server_viewAuditHistory = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._cmsi_Server_registerSQLServer = new System.Windows.Forms.ToolStripMenuItem();
            this._cmsi_Server_removeSQLServer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this._cmsi_Server_configureDataCollection = new System.Windows.Forms.ToolStripMenuItem();
            this._cmsi_Server_collectDataSnapshot = new System.Windows.Forms.ToolStripMenuItem();
            this._cmsi_Server_refresh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this._cmsi_Server_properties = new System.Windows.Forms.ToolStripMenuItem();
            this._smallTask_Register = new Idera.SQLsecure.UI.Console.Controls.SmallTask();
            this._smallTask_Collect = new Idera.SQLsecure.UI.Console.Controls.SmallTask();
            this._smallTask_Configure = new Idera.SQLsecure.UI.Console.Controls.SmallTask();
            this._toolstrip = new Idera.SQLsecure.UI.Console.Controls.HeaderStrip();
            this.toolStripLabel_Header = new System.Windows.Forms.ToolStripLabel();
            this._panel_Status = new System.Windows.Forms.Panel();
            this._tableLayoutPanel_Status = new System.Windows.Forms.TableLayoutPanel();
            this._viewSection_LicenseSummary = new Idera.SQLsecure.UI.Console.Controls.ViewSection();
            this.label_RemainingLicensesCount = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._label_RemainingLicensesBar = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this._label_LicenseBar = new System.Windows.Forms.Label();
            this.label_AuditedCount = new System.Windows.Forms.Label();
            this.label_LicensedCount = new System.Windows.Forms.Label();
            this._label_AuditedBar = new System.Windows.Forms.Label();
            this._viewSection_AgentStatus = new Idera.SQLsecure.UI.Console.Controls.ViewSection();
            this.label_AgentStatus = new System.Windows.Forms.Label();
            this._pictureBox_AgentStatus = new System.Windows.Forms.PictureBox();
            this._label_AgentStatus = new System.Windows.Forms.Label();
            this._systemStatus = new Idera.SQLsecure.UI.Console.Controls.SystemStatus();
            this.panel4 = new System.Windows.Forms.Panel();
            this._viewSection_Servers = new Idera.SQLsecure.UI.Console.Controls.ViewSection();
            this._vw_MainPanel.SuspendLayout();
            this._vw_TasksPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
            this._contextMenuStrip_Server.SuspendLayout();
            this._toolstrip.SuspendLayout();
            this._panel_Status.SuspendLayout();
            this._tableLayoutPanel_Status.SuspendLayout();
            this._viewSection_LicenseSummary.ViewPanel.SuspendLayout();
            this._viewSection_LicenseSummary.SuspendLayout();
            this._viewSection_AgentStatus.ViewPanel.SuspendLayout();
            this._viewSection_AgentStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox_AgentStatus)).BeginInit();
            this.panel4.SuspendLayout();
            this._viewSection_Servers.ViewPanel.SuspendLayout();
            this._viewSection_Servers.SuspendLayout();
            this.SuspendLayout();
            // 
            // _vw_MainPanel
            // 
            this._vw_MainPanel.Controls.Add(this._viewSection_Servers);
            this._vw_MainPanel.Controls.Add(this._panel_Status);
            this._vw_MainPanel.Location = new System.Drawing.Point(0, 72);
            this._vw_MainPanel.Size = new System.Drawing.Size(652, 516);
            // 
            // _vw_TasksPanel
            // 
            this._vw_TasksPanel.Controls.Add(this._smallTask_Configure);
            this._vw_TasksPanel.Controls.Add(this._smallTask_Collect);
            this._vw_TasksPanel.Controls.Add(this._smallTask_Register);
            this._vw_TasksPanel.Size = new System.Drawing.Size(652, 72);
            this._vw_TasksPanel.Controls.SetChildIndex(this._smallTask_Register, 0);
            this._vw_TasksPanel.Controls.SetChildIndex(this._smallTask_Help, 0);
            this._vw_TasksPanel.Controls.SetChildIndex(this._label_Summary, 0);
            this._vw_TasksPanel.Controls.SetChildIndex(this._smallTask_Collect, 0);
            this._vw_TasksPanel.Controls.SetChildIndex(this._smallTask_Configure, 0);
            // 
            // _smallTask_Help
            // 
            this._smallTask_Help.Location = new System.Drawing.Point(534, 31);
            this._smallTask_Help.Size = new System.Drawing.Size(110, 34);
            // 
            // _label_Summary
            // 
            this._label_Summary.Location = new System.Drawing.Point(1, 1);
            // 
            // _grid
            // 
            this._grid.ContextMenuStrip = this._contextMenuStrip_Server;
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
            this._grid.Size = new System.Drawing.Size(646, 254);
            this._grid.TabIndex = 3;
            this._grid.MouseDown += new System.Windows.Forms.MouseEventHandler(this._grid_MouseDown);
            this._grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this._grid_KeyDown);
            this._grid.DoubleClick += new System.EventHandler(this._grid_DoubleClick);
            this._grid.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this._grid_InitializeLayout);
            this._grid.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this._grid_AfterSelectChange);
            // 
            // _contextMenuStrip_Server
            // 
            this._contextMenuStrip_Server.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._cmsi_Server_exploreUserPermissions,
            this._cmsi_Server_exploreSnapshot,
            this._cmsi_Server_viewAuditHistory,
            this.toolStripSeparator1,
            this._cmsi_Server_registerSQLServer,
            this._cmsi_Server_removeSQLServer,
            this.toolStripSeparator2,
            this._cmsi_Server_configureDataCollection,
            this._cmsi_Server_collectDataSnapshot,
            this._cmsi_Server_refresh,
            this.toolStripSeparator3,
            this._cmsi_Server_properties});
            this._contextMenuStrip_Server.Name = "_contextMenuStrip_ExplorePermission";
            this._contextMenuStrip_Server.Size = new System.Drawing.Size(220, 220);
            this._contextMenuStrip_Server.Opening += new System.ComponentModel.CancelEventHandler(this._contextMenuStrip_Server_Opening);
            // 
            // _cmsi_Server_exploreUserPermissions
            // 
            this._cmsi_Server_exploreUserPermissions.Name = "_cmsi_Server_exploreUserPermissions";
            this._cmsi_Server_exploreUserPermissions.Size = new System.Drawing.Size(219, 22);
            this._cmsi_Server_exploreUserPermissions.Text = "Explore User Permissions";
            this._cmsi_Server_exploreUserPermissions.ToolTipText = "Explore SQL Server permissions assigned to a user";
            this._cmsi_Server_exploreUserPermissions.Click += new System.EventHandler(this._cmsi_Server_exploreUserPermissions_Click);
            // 
            // _cmsi_Server_exploreSnapshot
            // 
            this._cmsi_Server_exploreSnapshot.Name = "_cmsi_Server_exploreSnapshot";
            this._cmsi_Server_exploreSnapshot.Size = new System.Drawing.Size(219, 22);
            this._cmsi_Server_exploreSnapshot.Text = "Explore Object Permissions";
            this._cmsi_Server_exploreSnapshot.ToolTipText = "Explore SQL Server permissions of an audited SQL Server Snapshot";
            this._cmsi_Server_exploreSnapshot.Click += new System.EventHandler(this._cmsi_Server_exploreSnapshot_Click);
            // 
            // _cmsi_Server_viewAuditHistory
            // 
            this._cmsi_Server_viewAuditHistory.Name = "_cmsi_Server_viewAuditHistory";
            this._cmsi_Server_viewAuditHistory.Size = new System.Drawing.Size(219, 22);
            this._cmsi_Server_viewAuditHistory.Text = "View Server Summary";
            this._cmsi_Server_viewAuditHistory.Click += new System.EventHandler(this._cmsi_Server_viewAuditHistory_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(216, 6);
            // 
            // _cmsi_Server_registerSQLServer
            // 
            this._cmsi_Server_registerSQLServer.Name = "_cmsi_Server_registerSQLServer";
            this._cmsi_Server_registerSQLServer.Size = new System.Drawing.Size(219, 22);
            this._cmsi_Server_registerSQLServer.Text = "Register a SQL Server...";
            this._cmsi_Server_registerSQLServer.ToolTipText = "Register a new SQL Server instance to audit";
            this._cmsi_Server_registerSQLServer.Click += new System.EventHandler(this._cmsi_Server_registerSQLServer_Click);
            // 
            // _cmsi_Server_removeSQLServer
            // 
            this._cmsi_Server_removeSQLServer.Name = "_cmsi_Server_removeSQLServer";
            this._cmsi_Server_removeSQLServer.Size = new System.Drawing.Size(219, 22);
            this._cmsi_Server_removeSQLServer.Text = "Remove SQL Server";
            this._cmsi_Server_removeSQLServer.ToolTipText = "Remove selected SQL Server";
            this._cmsi_Server_removeSQLServer.Click += new System.EventHandler(this._cmsi_Server_removeSQLServer_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(216, 6);
            // 
            // _cmsi_Server_configureDataCollection
            // 
            this._cmsi_Server_configureDataCollection.Name = "_cmsi_Server_configureDataCollection";
            this._cmsi_Server_configureDataCollection.Size = new System.Drawing.Size(219, 22);
            this._cmsi_Server_configureDataCollection.Text = "Configure Data Collection...";
            this._cmsi_Server_configureDataCollection.ToolTipText = "Configure data collection schedule, objects collected, etc.";
            this._cmsi_Server_configureDataCollection.Click += new System.EventHandler(this._cmsi_Server_configureDataCollection_Click);
            // 
            // _cmsi_Server_collectDataSnapshot
            // 
            this._cmsi_Server_collectDataSnapshot.Name = "_cmsi_Server_collectDataSnapshot";
            this._cmsi_Server_collectDataSnapshot.Size = new System.Drawing.Size(219, 22);
            this._cmsi_Server_collectDataSnapshot.Text = "Take Snapshot Now";
            this._cmsi_Server_collectDataSnapshot.ToolTipText = "Collect SQL Server security data";
            this._cmsi_Server_collectDataSnapshot.Click += new System.EventHandler(this._cmsi_Server_collectDataSnapshot_Click);
            // 
            // _cmsi_Server_refresh
            // 
            this._cmsi_Server_refresh.Name = "_cmsi_Server_refresh";
            this._cmsi_Server_refresh.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this._cmsi_Server_refresh.Size = new System.Drawing.Size(219, 22);
            this._cmsi_Server_refresh.Text = "Refresh";
            this._cmsi_Server_refresh.Click += new System.EventHandler(this._cmsi_Server_refresh_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(216, 6);
            // 
            // _cmsi_Server_properties
            // 
            this._cmsi_Server_properties.Name = "_cmsi_Server_properties";
            this._cmsi_Server_properties.Size = new System.Drawing.Size(219, 22);
            this._cmsi_Server_properties.Text = "Properties...";
            this._cmsi_Server_properties.Click += new System.EventHandler(this._cmsi_Server_properties_Click);
            // 
            // _smallTask_Register
            // 
            this._smallTask_Register.BackColor = System.Drawing.Color.Transparent;
            this._smallTask_Register.Location = new System.Drawing.Point(11, 31);
            this._smallTask_Register.Name = "_smallTask_Register";
            this._smallTask_Register.Size = new System.Drawing.Size(160, 34);
            this._smallTask_Register.TabIndex = 3;
            this._smallTask_Register.TaskDescription = "";
            this._smallTask_Register.TaskImage = ((System.Drawing.Image)(resources.GetObject("_smallTask_Register.TaskImage")));
            this._smallTask_Register.TaskText = "Register";
            // 
            // _smallTask_Collect
            // 
            this._smallTask_Collect.BackColor = System.Drawing.Color.Transparent;
            this._smallTask_Collect.Location = new System.Drawing.Point(177, 31);
            this._smallTask_Collect.Name = "_smallTask_Collect";
            this._smallTask_Collect.Size = new System.Drawing.Size(160, 34);
            this._smallTask_Collect.TabIndex = 4;
            this._smallTask_Collect.TaskDescription = "";
            this._smallTask_Collect.TaskImage = ((System.Drawing.Image)(resources.GetObject("_smallTask_Collect.TaskImage")));
            this._smallTask_Collect.TaskText = "Collect";
            // 
            // _smallTask_Configure
            // 
            this._smallTask_Configure.BackColor = System.Drawing.Color.Transparent;
            this._smallTask_Configure.Location = new System.Drawing.Point(343, 31);
            this._smallTask_Configure.Name = "_smallTask_Configure";
            this._smallTask_Configure.Size = new System.Drawing.Size(160, 34);
            this._smallTask_Configure.TabIndex = 5;
            this._smallTask_Configure.TaskDescription = "";
            this._smallTask_Configure.TaskImage = ((System.Drawing.Image)(resources.GetObject("_smallTask_Configure.TaskImage")));
            this._smallTask_Configure.TaskText = "Configure";
            // 
            // _toolstrip
            // 
            this._toolstrip.AutoSize = false;
            this._toolstrip.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this._toolstrip.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this._toolstrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._toolstrip.HeaderStyle = Idera.SQLsecure.UI.Console.Controls.AreaHeaderStyle.Small;
            this._toolstrip.HotTrackEnabled = false;
            this._toolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel_Header});
            this._toolstrip.Location = new System.Drawing.Point(0, 0);
            this._toolstrip.Name = "_toolstrip";
            this._toolstrip.Size = new System.Drawing.Size(646, 19);
            this._toolstrip.TabIndex = 6;
            this._toolstrip.Text = "headerStrip1";
            // 
            // toolStripLabel_Header
            // 
            this.toolStripLabel_Header.Name = "toolStripLabel_Header";
            this.toolStripLabel_Header.Size = new System.Drawing.Size(78, 16);
            this.toolStripLabel_Header.Text = "toolStripLabel1";
            // 
            // _panel_Status
            // 
            this._panel_Status.Controls.Add(this._tableLayoutPanel_Status);
            this._panel_Status.Dock = System.Windows.Forms.DockStyle.Top;
            this._panel_Status.Location = new System.Drawing.Point(0, 0);
            this._panel_Status.Name = "_panel_Status";
            this._panel_Status.Size = new System.Drawing.Size(652, 223);
            this._panel_Status.TabIndex = 5;
            // 
            // _tableLayoutPanel_Status
            // 
            this._tableLayoutPanel_Status.ColumnCount = 2;
            this._tableLayoutPanel_Status.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tableLayoutPanel_Status.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tableLayoutPanel_Status.Controls.Add(this._viewSection_LicenseSummary, 1, 0);
            this._tableLayoutPanel_Status.Controls.Add(this._viewSection_AgentStatus, 1, 1);
            this._tableLayoutPanel_Status.Controls.Add(this._systemStatus, 0, 0);
            this._tableLayoutPanel_Status.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel_Status.Location = new System.Drawing.Point(0, 0);
            this._tableLayoutPanel_Status.Name = "_tableLayoutPanel_Status";
            this._tableLayoutPanel_Status.RowCount = 2;
            this._tableLayoutPanel_Status.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tableLayoutPanel_Status.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tableLayoutPanel_Status.Size = new System.Drawing.Size(652, 223);
            this._tableLayoutPanel_Status.TabIndex = 0;
            // 
            // _viewSection_LicenseSummary
            // 
            this._viewSection_LicenseSummary.BackColor = System.Drawing.Color.White;
            this._viewSection_LicenseSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this._viewSection_LicenseSummary.HeaderGradientBorderStyle = Idera.SQLsecure.UI.Console.Controls.GradientPanel.GradientBorderStyle.Fixed3DOut;
            this._viewSection_LicenseSummary.HeaderGradientColor = System.Drawing.Color.DarkGray;
            this._viewSection_LicenseSummary.HeaderGradientCornerStyle = Idera.SQLsecure.UI.Console.Controls.GradientPanel.GradientCornerStyle.RoundTop;
            this._viewSection_LicenseSummary.HeaderTextColor = System.Drawing.SystemColors.ControlText;
            this._viewSection_LicenseSummary.Location = new System.Drawing.Point(329, 3);
            this._viewSection_LicenseSummary.Name = "_viewSection_LicenseSummary";
            this._viewSection_LicenseSummary.Size = new System.Drawing.Size(320, 105);
            this._viewSection_LicenseSummary.TabIndex = 2;
            this._viewSection_LicenseSummary.Title = "License Summary";
            // 
            // _viewSection_LicenseSummary.Panel
            // 
            this._viewSection_LicenseSummary.ViewPanel.BackColor = System.Drawing.Color.Transparent;
            this._viewSection_LicenseSummary.ViewPanel.Controls.Add(this.label_RemainingLicensesCount);
            this._viewSection_LicenseSummary.ViewPanel.Controls.Add(this.label3);
            this._viewSection_LicenseSummary.ViewPanel.Controls.Add(this._label_RemainingLicensesBar);
            this._viewSection_LicenseSummary.ViewPanel.Controls.Add(this.label2);
            this._viewSection_LicenseSummary.ViewPanel.Controls.Add(this.label4);
            this._viewSection_LicenseSummary.ViewPanel.Controls.Add(this._label_LicenseBar);
            this._viewSection_LicenseSummary.ViewPanel.Controls.Add(this.label_AuditedCount);
            this._viewSection_LicenseSummary.ViewPanel.Controls.Add(this.label_LicensedCount);
            this._viewSection_LicenseSummary.ViewPanel.Controls.Add(this._label_AuditedBar);
            this._viewSection_LicenseSummary.ViewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._viewSection_LicenseSummary.ViewPanel.GradientColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this._viewSection_LicenseSummary.ViewPanel.Location = new System.Drawing.Point(0, 20);
            this._viewSection_LicenseSummary.ViewPanel.Name = "Panel";
            this._viewSection_LicenseSummary.ViewPanel.Rotation = 270F;
            this._viewSection_LicenseSummary.ViewPanel.Size = new System.Drawing.Size(320, 85);
            this._viewSection_LicenseSummary.ViewPanel.TabIndex = 1;
            // 
            // label_RemainingLicensesCount
            // 
            this.label_RemainingLicensesCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_RemainingLicensesCount.BackColor = System.Drawing.Color.Transparent;
            this.label_RemainingLicensesCount.Location = new System.Drawing.Point(321, 62);
            this.label_RemainingLicensesCount.Name = "label_RemainingLicensesCount";
            this.label_RemainingLicensesCount.Size = new System.Drawing.Size(56, 13);
            this.label_RemainingLicensesCount.TabIndex = 11;
            this.label_RemainingLicensesCount.Text = "Unlimited";
            this.label_RemainingLicensesCount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(5, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Server Licenses:";
            // 
            // _label_RemainingLicensesBar
            // 
            this._label_RemainingLicensesBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._label_RemainingLicensesBar.BackColor = System.Drawing.Color.Green;
            this._label_RemainingLicensesBar.Location = new System.Drawing.Point(132, 62);
            this._label_RemainingLicensesBar.Name = "_label_RemainingLicensesBar";
            this._label_RemainingLicensesBar.Size = new System.Drawing.Size(183, 13);
            this._label_RemainingLicensesBar.TabIndex = 10;
            this._label_RemainingLicensesBar.Paint += new System.Windows.Forms.PaintEventHandler(this.label_RemainingLicensesBar_Paint);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(5, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Audited Servers:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(5, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(128, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Remaining Licenses: ";
            // 
            // _label_LicenseBar
            // 
            this._label_LicenseBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._label_LicenseBar.BackColor = System.Drawing.Color.SteelBlue;
            this._label_LicenseBar.Location = new System.Drawing.Point(132, 12);
            this._label_LicenseBar.Name = "_label_LicenseBar";
            this._label_LicenseBar.Size = new System.Drawing.Size(183, 13);
            this._label_LicenseBar.TabIndex = 5;
            this._label_LicenseBar.Paint += new System.Windows.Forms.PaintEventHandler(this.label_LicenseBar_Paint);
            // 
            // label_AuditedCount
            // 
            this.label_AuditedCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_AuditedCount.BackColor = System.Drawing.Color.Transparent;
            this.label_AuditedCount.Location = new System.Drawing.Point(321, 37);
            this.label_AuditedCount.Name = "label_AuditedCount";
            this.label_AuditedCount.Size = new System.Drawing.Size(56, 13);
            this.label_AuditedCount.TabIndex = 8;
            this.label_AuditedCount.Text = "20";
            this.label_AuditedCount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label_LicensedCount
            // 
            this.label_LicensedCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_LicensedCount.BackColor = System.Drawing.Color.Transparent;
            this.label_LicensedCount.Location = new System.Drawing.Point(321, 12);
            this.label_LicensedCount.Name = "label_LicensedCount";
            this.label_LicensedCount.Size = new System.Drawing.Size(56, 13);
            this.label_LicensedCount.TabIndex = 6;
            this.label_LicensedCount.Text = "20";
            this.label_LicensedCount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // _label_AuditedBar
            // 
            this._label_AuditedBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._label_AuditedBar.BackColor = System.Drawing.Color.Gold;
            this._label_AuditedBar.Location = new System.Drawing.Point(132, 37);
            this._label_AuditedBar.Name = "_label_AuditedBar";
            this._label_AuditedBar.Size = new System.Drawing.Size(183, 13);
            this._label_AuditedBar.TabIndex = 7;
            this._label_AuditedBar.Paint += new System.Windows.Forms.PaintEventHandler(this.label_AuditedBar_Paint);
            // 
            // _viewSection_AgentStatus
            // 
            this._viewSection_AgentStatus.BackColor = System.Drawing.Color.White;
            this._viewSection_AgentStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this._viewSection_AgentStatus.HeaderGradientBorderStyle = Idera.SQLsecure.UI.Console.Controls.GradientPanel.GradientBorderStyle.Fixed3DOut;
            this._viewSection_AgentStatus.HeaderGradientColor = System.Drawing.Color.DarkGray;
            this._viewSection_AgentStatus.HeaderGradientCornerStyle = Idera.SQLsecure.UI.Console.Controls.GradientPanel.GradientCornerStyle.RoundTop;
            this._viewSection_AgentStatus.HeaderTextColor = System.Drawing.SystemColors.ControlText;
            this._viewSection_AgentStatus.Location = new System.Drawing.Point(329, 114);
            this._viewSection_AgentStatus.Name = "_viewSection_AgentStatus";
            this._viewSection_AgentStatus.Size = new System.Drawing.Size(320, 106);
            this._viewSection_AgentStatus.TabIndex = 3;
            this._viewSection_AgentStatus.Title = "SQL Server Agent Status";
            // 
            // _viewSection_AgentStatus.Panel
            // 
            this._viewSection_AgentStatus.ViewPanel.BackColor = System.Drawing.Color.Transparent;
            this._viewSection_AgentStatus.ViewPanel.Controls.Add(this.label_AgentStatus);
            this._viewSection_AgentStatus.ViewPanel.Controls.Add(this._pictureBox_AgentStatus);
            this._viewSection_AgentStatus.ViewPanel.Controls.Add(this._label_AgentStatus);
            this._viewSection_AgentStatus.ViewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._viewSection_AgentStatus.ViewPanel.GradientColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this._viewSection_AgentStatus.ViewPanel.Location = new System.Drawing.Point(0, 20);
            this._viewSection_AgentStatus.ViewPanel.Name = "Panel";
            this._viewSection_AgentStatus.ViewPanel.Rotation = 270F;
            this._viewSection_AgentStatus.ViewPanel.Size = new System.Drawing.Size(320, 86);
            this._viewSection_AgentStatus.ViewPanel.TabIndex = 1;
            // 
            // label_AgentStatus
            // 
            this.label_AgentStatus.AutoSize = true;
            this.label_AgentStatus.BackColor = System.Drawing.Color.Transparent;
            this.label_AgentStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_AgentStatus.Location = new System.Drawing.Point(8, 67);
            this.label_AgentStatus.Name = "label_AgentStatus";
            this.label_AgentStatus.Size = new System.Drawing.Size(48, 13);
            this.label_AgentStatus.TabIndex = 11;
            this.label_AgentStatus.Text = "Started";
            // 
            // _pictureBox_AgentStatus
            // 
            this._pictureBox_AgentStatus.BackColor = System.Drawing.Color.Transparent;
            this._pictureBox_AgentStatus.Image = ((System.Drawing.Image)(resources.GetObject("_pictureBox_AgentStatus.Image")));
            this._pictureBox_AgentStatus.Location = new System.Drawing.Point(8, 8);
            this._pictureBox_AgentStatus.Name = "_pictureBox_AgentStatus";
            this._pictureBox_AgentStatus.Size = new System.Drawing.Size(48, 56);
            this._pictureBox_AgentStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this._pictureBox_AgentStatus.TabIndex = 10;
            this._pictureBox_AgentStatus.TabStop = false;
            // 
            // _label_AgentStatus
            // 
            this._label_AgentStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._label_AgentStatus.BackColor = System.Drawing.Color.Transparent;
            this._label_AgentStatus.Location = new System.Drawing.Point(81, 8);
            this._label_AgentStatus.Name = "_label_AgentStatus";
            this._label_AgentStatus.Size = new System.Drawing.Size(227, 73);
            this._label_AgentStatus.TabIndex = 9;
            this._label_AgentStatus.Text = "SQLsecure uses the Repository SQL Server Agent for data collection and grooming. " +
                " The SQL Server Agent must be started for SQLsecure to collect data or groom the" +
                " Repository.";
            // 
            // _systemStatus
            // 
            this._systemStatus.BackColor = System.Drawing.Color.Transparent;
            this._systemStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this._systemStatus.HeaderGradientCornerStyle = Idera.SQLsecure.UI.Console.Controls.GradientPanel.GradientCornerStyle.RoundTop;
            this._systemStatus.Location = new System.Drawing.Point(3, 3);
            this._systemStatus.Name = "_systemStatus";
            this._tableLayoutPanel_Status.SetRowSpan(this._systemStatus, 2);
            this._systemStatus.Size = new System.Drawing.Size(320, 217);
            this._systemStatus.TabIndex = 4;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this._grid);
            this.panel4.Controls.Add(this._toolstrip);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(646, 273);
            this.panel4.TabIndex = 6;
            // 
            // _viewSection_Servers
            // 
            this._viewSection_Servers.BackColor = System.Drawing.Color.White;
            this._viewSection_Servers.Dock = System.Windows.Forms.DockStyle.Fill;
            this._viewSection_Servers.HeaderGradientBorderStyle = Idera.SQLsecure.UI.Console.Controls.GradientPanel.GradientBorderStyle.Fixed3DOut;
            this._viewSection_Servers.HeaderGradientColor = System.Drawing.Color.DarkGray;
            this._viewSection_Servers.HeaderGradientCornerStyle = Idera.SQLsecure.UI.Console.Controls.GradientPanel.GradientCornerStyle.RoundTop;
            this._viewSection_Servers.HeaderTextColor = System.Drawing.SystemColors.ControlText;
            this._viewSection_Servers.Location = new System.Drawing.Point(0, 223);
            this._viewSection_Servers.Name = "_viewSection_Servers";
            this._viewSection_Servers.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this._viewSection_Servers.Size = new System.Drawing.Size(652, 293);
            this._viewSection_Servers.TabIndex = 7;
            this._viewSection_Servers.Title = "Audited SQL Servers Status";
            // 
            // _viewSection_Servers.Panel
            // 
            this._viewSection_Servers.ViewPanel.BackColor = System.Drawing.Color.Transparent;
            this._viewSection_Servers.ViewPanel.Controls.Add(this.panel4);
            this._viewSection_Servers.ViewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._viewSection_Servers.ViewPanel.GradientColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._viewSection_Servers.ViewPanel.Location = new System.Drawing.Point(3, 20);
            this._viewSection_Servers.ViewPanel.Name = "Panel";
            this._viewSection_Servers.ViewPanel.Rotation = 270F;
            this._viewSection_Servers.ViewPanel.Size = new System.Drawing.Size(646, 273);
            this._viewSection_Servers.ViewPanel.TabIndex = 1;
            // 
            // View_Main_ManageSQLsecure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "View_Main_ManageSQLsecure";
            this.Load += new System.EventHandler(this.View_Main_ManageSQLsecure_Load);
            this.Leave += new System.EventHandler(this.View_Main_ManageSQLsecure_Leave);
            this.Enter += new System.EventHandler(this.View_Main_ManageSQLsecure_Enter);
            this.Controls.SetChildIndex(this._vw_TasksPanel, 0);
            this.Controls.SetChildIndex(this._vw_MainPanel, 0);
            this._vw_MainPanel.ResumeLayout(false);
            this._vw_TasksPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
            this._contextMenuStrip_Server.ResumeLayout(false);
            this._toolstrip.ResumeLayout(false);
            this._toolstrip.PerformLayout();
            this._panel_Status.ResumeLayout(false);
            this._tableLayoutPanel_Status.ResumeLayout(false);
            this._viewSection_LicenseSummary.ViewPanel.ResumeLayout(false);
            this._viewSection_LicenseSummary.ViewPanel.PerformLayout();
            this._viewSection_LicenseSummary.ResumeLayout(false);
            this._viewSection_AgentStatus.ViewPanel.ResumeLayout(false);
            this._viewSection_AgentStatus.ViewPanel.PerformLayout();
            this._viewSection_AgentStatus.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox_AgentStatus)).EndInit();
            this.panel4.ResumeLayout(false);
            this._viewSection_Servers.ViewPanel.ResumeLayout(false);
            this._viewSection_Servers.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinGrid.UltraGrid _grid;
        private System.Windows.Forms.ContextMenuStrip _contextMenuStrip_Server;
        private System.Windows.Forms.ToolStripMenuItem _cmsi_Server_exploreUserPermissions;
        private System.Windows.Forms.ToolStripMenuItem _cmsi_Server_exploreSnapshot;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem _cmsi_Server_registerSQLServer;
        private System.Windows.Forms.ToolStripMenuItem _cmsi_Server_removeSQLServer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem _cmsi_Server_configureDataCollection;
        private System.Windows.Forms.ToolStripMenuItem _cmsi_Server_collectDataSnapshot;
        private System.Windows.Forms.ToolStripMenuItem _cmsi_Server_refresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem _cmsi_Server_properties;
        private Idera.SQLsecure.UI.Console.Controls.SmallTask _smallTask_Register;
        private Idera.SQLsecure.UI.Console.Controls.SmallTask _smallTask_Configure;
        private Idera.SQLsecure.UI.Console.Controls.SmallTask _smallTask_Collect;
        private System.Windows.Forms.ToolStripMenuItem _cmsi_Server_viewAuditHistory;
        private Idera.SQLsecure.UI.Console.Controls.HeaderStrip _toolstrip;
        private System.Windows.Forms.Panel _panel_Status;
        private System.Windows.Forms.Label _label_AgentStatus;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_Status;
        private System.Windows.Forms.Label label_AgentStatus;
        private System.Windows.Forms.PictureBox _pictureBox_AgentStatus;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label _label_LicenseBar;
        private System.Windows.Forms.Label label_LicensedCount;
        private System.Windows.Forms.Label label_AuditedCount;
        private System.Windows.Forms.Label _label_AuditedBar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label_RemainingLicensesCount;
        private System.Windows.Forms.Label _label_RemainingLicensesBar;
        private Idera.SQLsecure.UI.Console.Controls.ViewSection _viewSection_LicenseSummary;
        private Idera.SQLsecure.UI.Console.Controls.ViewSection _viewSection_AgentStatus;
        private Idera.SQLsecure.UI.Console.Controls.ViewSection _viewSection_Servers;
        private System.Windows.Forms.ToolStripLabel toolStripLabel_Header;
        private Idera.SQLsecure.UI.Console.Controls.SystemStatus _systemStatus;
    }
}
