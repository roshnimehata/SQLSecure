namespace Idera.SQLsecure.UI.Console.Views
{
    partial class View_PermissionExplorer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(View_PermissionExplorer));
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this._snapshotProperties = new Idera.SQLsecure.UI.Console.Controls.SnapshotProperties();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this._userPermissions = new Idera.SQLsecure.UI.Console.Controls.UserPermissions();
            this.ultraTabPageControl4 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this._rolePermissions = new Idera.SQLsecure.UI.Console.Controls.RolePermissions();
            this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this._objectPermissions = new Idera.SQLsecure.UI.Console.Controls.ObjectExplorer();
            this._smallTask_Configure = new Idera.SQLsecure.UI.Console.Controls.SmallTask();
            this._smallTask_Collect = new Idera.SQLsecure.UI.Console.Controls.SmallTask();
            this._ultraTabControl = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this._vw_MainPanel.SuspendLayout();
            this._vw_TasksPanel.SuspendLayout();
            this.ultraTabPageControl1.SuspendLayout();
            this.ultraTabPageControl2.SuspendLayout();
            this.ultraTabPageControl4.SuspendLayout();
            this.ultraTabPageControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._ultraTabControl)).BeginInit();
            this._ultraTabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // _vw_MainPanel
            // 
            this._vw_MainPanel.Controls.Add(this._ultraTabControl);
            this._vw_MainPanel.Location = new System.Drawing.Point(0, 72);
            this._vw_MainPanel.Size = new System.Drawing.Size(652, 516);
            // 
            // _vw_TasksPanel
            // 
            this._vw_TasksPanel.Controls.Add(this._smallTask_Configure);
            this._vw_TasksPanel.Controls.Add(this._smallTask_Collect);
            this._vw_TasksPanel.Size = new System.Drawing.Size(652, 72);
            this._vw_TasksPanel.Controls.SetChildIndex(this._smallTask_Help, 0);
            this._vw_TasksPanel.Controls.SetChildIndex(this._label_Summary, 0);
            this._vw_TasksPanel.Controls.SetChildIndex(this._smallTask_Collect, 0);
            this._vw_TasksPanel.Controls.SetChildIndex(this._smallTask_Configure, 0);
            // 
            // _smallTask_Help
            // 
            this._smallTask_Help.Location = new System.Drawing.Point(532, 31);
            this._smallTask_Help.Size = new System.Drawing.Size(112, 34);
            // 
            // _label_Summary
            // 
            this._label_Summary.Location = new System.Drawing.Point(1, 1);
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this._snapshotProperties);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(1, 23);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(648, 490);
            // 
            // _snapshotProperties
            // 
            this._snapshotProperties.BackColor = System.Drawing.Color.White;
            this._snapshotProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this._snapshotProperties.Location = new System.Drawing.Point(0, 0);
            this._snapshotProperties.Name = "_snapshotProperties";
            this._snapshotProperties.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this._snapshotProperties.Size = new System.Drawing.Size(648, 490);
            this._snapshotProperties.TabIndex = 0;
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this._userPermissions);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(648, 490);
            // 
            // _userPermissions
            // 
            this._userPermissions.AutoScroll = true;
            this._userPermissions.BackColor = System.Drawing.SystemColors.Window;
            this._userPermissions.Cursor = System.Windows.Forms.Cursors.Default;
            this._userPermissions.Dock = System.Windows.Forms.DockStyle.Fill;
            this._userPermissions.Location = new System.Drawing.Point(0, 0);
            this._userPermissions.Margin = new System.Windows.Forms.Padding(0);
            this._userPermissions.MinimumSize = new System.Drawing.Size(644, 300);
            this._userPermissions.Name = "_userPermissions";
            this._userPermissions.Size = new System.Drawing.Size(648, 490);
            this._userPermissions.TabIndex = 0;
            // 
            // ultraTabPageControl4
            // 
            this.ultraTabPageControl4.Controls.Add(this._rolePermissions);
            this.ultraTabPageControl4.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl4.Name = "ultraTabPageControl4";
            this.ultraTabPageControl4.Size = new System.Drawing.Size(648, 490);
            // 
            // _rolePermissions
            // 
            this._rolePermissions.AutoScroll = true;
            this._rolePermissions.BackColor = System.Drawing.Color.Transparent;
            this._rolePermissions.Dock = System.Windows.Forms.DockStyle.Fill;
            this._rolePermissions.Location = new System.Drawing.Point(0, 0);
            this._rolePermissions.Margin = new System.Windows.Forms.Padding(0);
            this._rolePermissions.MinimumSize = new System.Drawing.Size(644, 300);
            this._rolePermissions.Name = "_rolePermissions";
            this._rolePermissions.Size = new System.Drawing.Size(648, 490);
            this._rolePermissions.TabIndex = 0;
            // 
            // ultraTabPageControl3
            // 
            this.ultraTabPageControl3.Controls.Add(this._objectPermissions);
            this.ultraTabPageControl3.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl3.Name = "ultraTabPageControl3";
            this.ultraTabPageControl3.Size = new System.Drawing.Size(648, 490);
            // 
            // _objectPermissions
            // 
            this._objectPermissions.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this._objectPermissions.Dock = System.Windows.Forms.DockStyle.Fill;
            this._objectPermissions.Location = new System.Drawing.Point(0, 0);
            this._objectPermissions.Name = "_objectPermissions";
            this._objectPermissions.Size = new System.Drawing.Size(648, 490);
            this._objectPermissions.TabIndex = 0;
            // 
            // _smallTask_Configure
            // 
            this._smallTask_Configure.BackColor = System.Drawing.Color.Transparent;
            this._smallTask_Configure.Location = new System.Drawing.Point(177, 31);
            this._smallTask_Configure.Name = "_smallTask_Configure";
            this._smallTask_Configure.Size = new System.Drawing.Size(160, 34);
            this._smallTask_Configure.TabIndex = 7;
            this._smallTask_Configure.TaskDescription = "";
            this._smallTask_Configure.TaskImage = ((System.Drawing.Image)(resources.GetObject("_smallTask_Configure.TaskImage")));
            this._smallTask_Configure.TaskText = "Configure";
            // 
            // _smallTask_Collect
            // 
            this._smallTask_Collect.BackColor = System.Drawing.Color.Transparent;
            this._smallTask_Collect.Location = new System.Drawing.Point(11, 31);
            this._smallTask_Collect.Name = "_smallTask_Collect";
            this._smallTask_Collect.Size = new System.Drawing.Size(160, 34);
            this._smallTask_Collect.TabIndex = 6;
            this._smallTask_Collect.TaskDescription = "";
            this._smallTask_Collect.TaskImage = ((System.Drawing.Image)(resources.GetObject("_smallTask_Collect.TaskImage")));
            this._smallTask_Collect.TaskText = "Collect";
            // 
            // _ultraTabControl
            // 
            this._ultraTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._ultraTabControl.Controls.Add(this.ultraTabSharedControlsPage1);
            this._ultraTabControl.Controls.Add(this.ultraTabPageControl1);
            this._ultraTabControl.Controls.Add(this.ultraTabPageControl2);
            this._ultraTabControl.Controls.Add(this.ultraTabPageControl3);
            this._ultraTabControl.Controls.Add(this.ultraTabPageControl4);
            this._ultraTabControl.Location = new System.Drawing.Point(0, 0);
            this._ultraTabControl.Name = "_ultraTabControl";
            this._ultraTabControl.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this._ultraTabControl.Size = new System.Drawing.Size(652, 516);
            this._ultraTabControl.TabIndex = 3;
            appearance1.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.camera_16;
            ultraTab1.Appearance = appearance1;
            ultraTab1.Key = "_tab_Summary";
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "Snapshot Summary";
            appearance2.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_UserPermissions_16;
            ultraTab2.Appearance = appearance2;
            ultraTab2.Key = "_tab_Users";
            ultraTab2.TabPage = this.ultraTabPageControl2;
            ultraTab2.Text = "User Permissions";
            appearance4.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_ServerRoles_16;
            ultraTab4.Appearance = appearance4;
            ultraTab4.Key = "_tab_Roles";
            ultraTab4.TabPage = this.ultraTabPageControl4;
            ultraTab4.Text = "Role Permissions";
            appearance3.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.box_view_16;
            ultraTab3.Appearance = appearance3;
            ultraTab3.Key = "_tab_Objects";
            ultraTab3.TabPage = this.ultraTabPageControl3;
            ultraTab3.Text = "Object Permissions";
            this._ultraTabControl.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2,
            ultraTab4,
            ultraTab3});
            this._ultraTabControl.SelectedTabChanging += new Infragistics.Win.UltraWinTabControl.SelectedTabChangingEventHandler(this._ultraTabControl_SelectedTabChanging);
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(648, 490);
            // 
            // View_PermissionExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScrollMinSize = new System.Drawing.Size(652, 500);
            this.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.Name = "View_PermissionExplorer";
            this.Enter += new System.EventHandler(this.View_PermissionExplorer_Enter);
            this.Leave += new System.EventHandler(this.View_PermissionExplorer_Leave);
            this._vw_MainPanel.ResumeLayout(false);
            this._vw_TasksPanel.ResumeLayout(false);
            this.ultraTabPageControl1.ResumeLayout(false);
            this.ultraTabPageControl2.ResumeLayout(false);
            this.ultraTabPageControl4.ResumeLayout(false);
            this.ultraTabPageControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._ultraTabControl)).EndInit();
            this._ultraTabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Idera.SQLsecure.UI.Console.Controls.UserPermissions _userPermissions;
        private Idera.SQLsecure.UI.Console.Controls.ObjectExplorer _objectPermissions;
        private Idera.SQLsecure.UI.Console.Controls.SmallTask _smallTask_Configure;
        private Idera.SQLsecure.UI.Console.Controls.SmallTask _smallTask_Collect;
        private Idera.SQLsecure.UI.Console.Controls.SnapshotProperties _snapshotProperties;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl _ultraTabControl;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl4;
        private Idera.SQLsecure.UI.Console.Controls.RolePermissions _rolePermissions;
    }
}
