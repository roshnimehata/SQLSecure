namespace Idera.SQLsecure.UI.Console.Views
{
    partial class View_Main_ExplorePermissions
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(View_Main_ExplorePermissions));
            this._pnl_Header = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this._lbl_Intro = new System.Windows.Forms.Label();
            this._lbl_Tag = new System.Windows.Forms.Label();
            this._pnl_Body = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel_ViewContainer = new System.Windows.Forms.Panel();
            this._auditedServers = new Idera.SQLsecure.UI.Console.Controls.AuditedServers();
            this.viewSection1 = new Idera.SQLsecure.UI.Console.Controls.ViewSection();
            this._tableLayoutPanel_tasks = new System.Windows.Forms.TableLayoutPanel();
            this._commonTask1 = new Idera.SQLsecure.Controls.CommonTask();
            this._commonTask2 = new Idera.SQLsecure.Controls.CommonTask();
            this._commonTask3 = new Idera.SQLsecure.Controls.CommonTask();
            this._commonTask4 = new Idera.SQLsecure.Controls.CommonTask();
            this._commonTask5 = new Idera.SQLsecure.Controls.CommonTask();
            this._commonTask6 = new Idera.SQLsecure.Controls.CommonTask();
            this._pnl_Header.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this._pnl_Body.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel_ViewContainer.SuspendLayout();
            this.viewSection1.ViewPanel.SuspendLayout();
            this.viewSection1.SuspendLayout();
            this._tableLayoutPanel_tasks.SuspendLayout();
            this.SuspendLayout();
            // 
            // _pnl_Header
            // 
            this._pnl_Header.Controls.Add(this.panel2);
            this._pnl_Header.Controls.Add(this._lbl_Intro);
            this._pnl_Header.Controls.Add(this._lbl_Tag);
            this._pnl_Header.Dock = System.Windows.Forms.DockStyle.Top;
            this._pnl_Header.Location = new System.Drawing.Point(0, 0);
            this._pnl_Header.Name = "_pnl_Header";
            this._pnl_Header.Size = new System.Drawing.Size(642, 106);
            this._pnl_Header.TabIndex = 1;
            this._pnl_Header.Visible = false;
            // 
            // panel2
            // 
            this.panel2.BackgroundImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.UI_header;
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(642, 50);
            this.panel2.TabIndex = 7;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.UI_header_tagline;
            this.pictureBox1.Location = new System.Drawing.Point(305, -1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(337, 50);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // _lbl_Intro
            // 
            this._lbl_Intro.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lbl_Intro.Location = new System.Drawing.Point(3, 71);
            this._lbl_Intro.Name = "_lbl_Intro";
            this._lbl_Intro.Size = new System.Drawing.Size(629, 26);
            this._lbl_Intro.TabIndex = 1;
            this._lbl_Intro.Text = "label1";
            // 
            // _lbl_Tag
            // 
            this._lbl_Tag.AutoSize = true;
            this._lbl_Tag.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lbl_Tag.Location = new System.Drawing.Point(3, 56);
            this._lbl_Tag.Name = "_lbl_Tag";
            this._lbl_Tag.Size = new System.Drawing.Size(41, 13);
            this._lbl_Tag.TabIndex = 0;
            this._lbl_Tag.Text = "label1";
            // 
            // _pnl_Body
            // 
            this._pnl_Body.Controls.Add(this.panel1);
            this._pnl_Body.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnl_Body.Location = new System.Drawing.Point(0, 0);
            this._pnl_Body.Name = "_pnl_Body";
            this._pnl_Body.Size = new System.Drawing.Size(642, 571);
            this._pnl_Body.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel_ViewContainer);
            this.panel1.Controls.Add(this._pnl_Header);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(642, 571);
            this.panel1.TabIndex = 0;
            // 
            // panel_ViewContainer
            // 
            this.panel_ViewContainer.Controls.Add(this._auditedServers);
            this.panel_ViewContainer.Controls.Add(this.viewSection1);
            this.panel_ViewContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_ViewContainer.Location = new System.Drawing.Point(0, 106);
            this.panel_ViewContainer.Name = "panel_ViewContainer";
            this.panel_ViewContainer.Size = new System.Drawing.Size(642, 465);
            this.panel_ViewContainer.TabIndex = 2;
            // 
            // _auditedServers
            // 
            this._auditedServers.BackColor = System.Drawing.Color.Transparent;
            this._auditedServers.Dock = System.Windows.Forms.DockStyle.Fill;
            this._auditedServers.Location = new System.Drawing.Point(0, 0);
            this._auditedServers.Name = "_auditedServers";
            this._auditedServers.Size = new System.Drawing.Size(642, 318);
            this._auditedServers.TabIndex = 3;
            // 
            // viewSection1
            // 
            this.viewSection1.BackColor = System.Drawing.Color.White;
            this.viewSection1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.viewSection1.HeaderGradientBorderStyle = Idera.SQLsecure.UI.Console.Controls.GradientPanel.GradientBorderStyle.Fixed3DOut;
            this.viewSection1.HeaderGradientColor = System.Drawing.Color.DarkGray;
            this.viewSection1.HeaderGradientCornerStyle = Idera.SQLsecure.UI.Console.Controls.GradientPanel.GradientCornerStyle.RoundCorners;
            this.viewSection1.HeaderTextColor = System.Drawing.SystemColors.ControlText;
            this.viewSection1.Location = new System.Drawing.Point(0, 318);
            this.viewSection1.Name = "viewSection1";
            this.viewSection1.Size = new System.Drawing.Size(642, 147);
            this.viewSection1.TabIndex = 2;
            this.viewSection1.Title = "Common Tasks";
            // 
            // viewSection1.Panel
            // 
            this.viewSection1.ViewPanel.BackColor = System.Drawing.Color.Transparent;
            this.viewSection1.ViewPanel.Controls.Add(this._tableLayoutPanel_tasks);
            this.viewSection1.ViewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewSection1.ViewPanel.GradientColor = System.Drawing.Color.Empty;
            this.viewSection1.ViewPanel.Location = new System.Drawing.Point(0, 20);
            this.viewSection1.ViewPanel.Name = "Panel";
            this.viewSection1.ViewPanel.Rotation = 270F;
            this.viewSection1.ViewPanel.Size = new System.Drawing.Size(642, 127);
            this.viewSection1.ViewPanel.TabIndex = 1;
            this.viewSection1.Visible = false;
            // 
            // _tableLayoutPanel_tasks
            // 
            this._tableLayoutPanel_tasks.ColumnCount = 3;
            this._tableLayoutPanel_tasks.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33332F));
            this._tableLayoutPanel_tasks.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this._tableLayoutPanel_tasks.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this._tableLayoutPanel_tasks.Controls.Add(this._commonTask1, 0, 0);
            this._tableLayoutPanel_tasks.Controls.Add(this._commonTask2, 1, 0);
            this._tableLayoutPanel_tasks.Controls.Add(this._commonTask3, 2, 0);
            this._tableLayoutPanel_tasks.Controls.Add(this._commonTask4, 0, 1);
            this._tableLayoutPanel_tasks.Controls.Add(this._commonTask5, 1, 1);
            this._tableLayoutPanel_tasks.Controls.Add(this._commonTask6, 2, 1);
            this._tableLayoutPanel_tasks.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel_tasks.Location = new System.Drawing.Point(0, 0);
            this._tableLayoutPanel_tasks.Margin = new System.Windows.Forms.Padding(0);
            this._tableLayoutPanel_tasks.MaximumSize = new System.Drawing.Size(0, 153);
            this._tableLayoutPanel_tasks.MinimumSize = new System.Drawing.Size(640, 121);
            this._tableLayoutPanel_tasks.Name = "_tableLayoutPanel_tasks";
            this._tableLayoutPanel_tasks.Padding = new System.Windows.Forms.Padding(1);
            this._tableLayoutPanel_tasks.RowCount = 2;
            this._tableLayoutPanel_tasks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tableLayoutPanel_tasks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tableLayoutPanel_tasks.Size = new System.Drawing.Size(642, 127);
            this._tableLayoutPanel_tasks.TabIndex = 1;
            // 
            // _commonTask1
            // 
            this._commonTask1.Dock = System.Windows.Forms.DockStyle.Fill;
            this._commonTask1.HoverTaskImage = null;
            this._commonTask1.Location = new System.Drawing.Point(4, 4);
            this._commonTask1.Name = "_commonTask1";
            this._commonTask1.Size = new System.Drawing.Size(207, 56);
            this._commonTask1.TabIndex = 1;
            this._commonTask1.TaskDescription = "Task Description";
            this._commonTask1.TaskImage = ((System.Drawing.Image)(resources.GetObject("_commonTask1.TaskImage")));
            this._commonTask1.TaskText = "Link Text";
            // 
            // _commonTask2
            // 
            this._commonTask2.Dock = System.Windows.Forms.DockStyle.Fill;
            this._commonTask2.HoverTaskImage = null;
            this._commonTask2.Location = new System.Drawing.Point(217, 4);
            this._commonTask2.Name = "_commonTask2";
            this._commonTask2.Size = new System.Drawing.Size(207, 56);
            this._commonTask2.TabIndex = 2;
            this._commonTask2.TaskDescription = "Task Description";
            this._commonTask2.TaskImage = ((System.Drawing.Image)(resources.GetObject("_commonTask2.TaskImage")));
            this._commonTask2.TaskText = "Link Text";
            // 
            // _commonTask3
            // 
            this._commonTask3.Dock = System.Windows.Forms.DockStyle.Fill;
            this._commonTask3.HoverTaskImage = null;
            this._commonTask3.Location = new System.Drawing.Point(430, 4);
            this._commonTask3.Name = "_commonTask3";
            this._commonTask3.Size = new System.Drawing.Size(208, 56);
            this._commonTask3.TabIndex = 3;
            this._commonTask3.TaskDescription = "";
            this._commonTask3.TaskImage = ((System.Drawing.Image)(resources.GetObject("_commonTask3.TaskImage")));
            this._commonTask3.TaskText = "Link Text";
            // 
            // _commonTask4
            // 
            this._commonTask4.Dock = System.Windows.Forms.DockStyle.Fill;
            this._commonTask4.HoverTaskImage = null;
            this._commonTask4.Location = new System.Drawing.Point(4, 66);
            this._commonTask4.Name = "_commonTask4";
            this._commonTask4.Size = new System.Drawing.Size(207, 57);
            this._commonTask4.TabIndex = 4;
            this._commonTask4.TaskDescription = "Task Description";
            this._commonTask4.TaskImage = ((System.Drawing.Image)(resources.GetObject("_commonTask4.TaskImage")));
            this._commonTask4.TaskText = "Link Text";
            // 
            // _commonTask5
            // 
            this._commonTask5.Dock = System.Windows.Forms.DockStyle.Fill;
            this._commonTask5.HoverTaskImage = null;
            this._commonTask5.Location = new System.Drawing.Point(217, 66);
            this._commonTask5.Name = "_commonTask5";
            this._commonTask5.Size = new System.Drawing.Size(207, 57);
            this._commonTask5.TabIndex = 5;
            this._commonTask5.TaskDescription = "Task Description";
            this._commonTask5.TaskImage = ((System.Drawing.Image)(resources.GetObject("_commonTask5.TaskImage")));
            this._commonTask5.TaskText = "Link Text";
            // 
            // _commonTask6
            // 
            this._commonTask6.Dock = System.Windows.Forms.DockStyle.Fill;
            this._commonTask6.HoverTaskImage = null;
            this._commonTask6.Location = new System.Drawing.Point(430, 66);
            this._commonTask6.Name = "_commonTask6";
            this._commonTask6.Size = new System.Drawing.Size(208, 57);
            this._commonTask6.TabIndex = 6;
            this._commonTask6.TaskDescription = "Task Description";
            this._commonTask6.TaskImage = ((System.Drawing.Image)(resources.GetObject("_commonTask6.TaskImage")));
            this._commonTask6.TaskText = "Task Title";
            // 
            // View_Main_ExplorePermissions
            // 
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(630, 300);
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this._pnl_Body);
            this.Name = "View_Main_ExplorePermissions";
            this.Size = new System.Drawing.Size(642, 571);
            this.Enter += new System.EventHandler(this.View_Main_ExplorePermissions_Enter);
            this.Leave += new System.EventHandler(this.View_Main_ExplorePermissions_Leave);
            this._pnl_Header.ResumeLayout(false);
            this._pnl_Header.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this._pnl_Body.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel_ViewContainer.ResumeLayout(false);
            this.viewSection1.ViewPanel.ResumeLayout(false);
            this.viewSection1.ResumeLayout(false);
            this._tableLayoutPanel_tasks.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel _pnl_Header;
        private System.Windows.Forms.Label _lbl_Tag;
        private System.Windows.Forms.Label _lbl_Intro;
        private System.Windows.Forms.Panel _pnl_Body;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel_ViewContainer;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel_tasks;
        private Idera.SQLsecure.Controls.CommonTask _commonTask1;
        private Idera.SQLsecure.Controls.CommonTask _commonTask2;
        private Idera.SQLsecure.Controls.CommonTask _commonTask3;
        private Idera.SQLsecure.Controls.CommonTask _commonTask4;
        private Idera.SQLsecure.Controls.CommonTask _commonTask5;
        private Idera.SQLsecure.UI.Console.Controls.ViewSection viewSection1;
        private Idera.SQLsecure.Controls.CommonTask _commonTask6;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel2;
        private Idera.SQLsecure.UI.Console.Controls.AuditedServers _auditedServers;
    }
}
