namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_SqlServerProperties
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_SqlServerProperties));
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinListView.UltraListViewSubItemColumn ultraListViewSubItemColumn1 = new Infragistics.Win.UltraWinListView.UltraListViewSubItemColumn("colDescription");
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab7 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab6 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab5 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            this.ultraTabPageControl_General = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this._grpbx_NameVersion = new System.Windows.Forms.GroupBox();
            this._lbl_SaVal = new System.Windows.Forms.Label();
            this._lbl_Sa = new System.Windows.Forms.Label();
            this._lbl_ReplicationVal = new System.Windows.Forms.Label();
            this._lbl_Replication = new System.Windows.Forms.Label();
            this._lbl_SQLServerEditionVal = new System.Windows.Forms.Label();
            this._lbl_SQLServerVersionVal = new System.Windows.Forms.Label();
            this._lbl_SQLServerEdition = new System.Windows.Forms.Label();
            this._lbl_SQLServerVersion = new System.Windows.Forms.Label();
            this._lbl_ServerVal = new System.Windows.Forms.Label();
            this._lbl_Server = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._lbl_WindowsOSVal = new System.Windows.Forms.Label();
            this._lbl_DcVal = new System.Windows.Forms.Label();
            this._lbl_Dc = new System.Windows.Forms.Label();
            this._lbl_WindowsOS = new System.Windows.Forms.Label();
            this._lbl_OsServerVal = new System.Windows.Forms.Label();
            this._lbl_OsServer = new System.Windows.Forms.Label();
            this._grpbx_Snapshot = new System.Windows.Forms.GroupBox();
            this._lbl_LastSuccessfulSnapshotAtVal = new System.Windows.Forms.Label();
            this._lbl_CurrentSnapshotStatusVal = new System.Windows.Forms.Label();
            this._lbl_CurrentSnapshotTimeVal = new System.Windows.Forms.Label();
            this._lbl_LastSuccessfulSnapshotAt = new System.Windows.Forms.Label();
            this._lbl_CurrentSnapshotStatus = new System.Windows.Forms.Label();
            this._lbl_CurrentSnapshotTime = new System.Windows.Forms.Label();
            this.ultraTabPageControl_Credentials = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this._grpbx_SQLServerCredentials = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_SQLWindowsUser = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox_SQLWindowsPassword = new System.Windows.Forms.TextBox();
            this.radioButton_WindowsAuth = new System.Windows.Forms.RadioButton();
            this.radioButton_SQLServerAuth = new System.Windows.Forms.RadioButton();
            this._lbl_SqlLogin = new System.Windows.Forms.Label();
            this.textbox_SqlLogin = new System.Windows.Forms.TextBox();
            this._lbl_SqlLoginPassword = new System.Windows.Forms.Label();
            this.textbox_SqlLoginPassword = new System.Windows.Forms.TextBox();
            this._grpbx_WindowsGMCredentials = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.checkBox_UseSameAuth = new System.Windows.Forms.CheckBox();
            this._lbl_WindowsUser = new System.Windows.Forms.Label();
            this.textbox_WindowsUser = new System.Windows.Forms.TextBox();
            this._lbl_WindowsPassword = new System.Windows.Forms.Label();
            this.textbox_WindowsPassword = new System.Windows.Forms.TextBox();
            this._pnl_CredentialsIntro = new System.Windows.Forms.Panel();
            this._lbl_CredentialsIntroSep = new System.Windows.Forms.Label();
            this._lbl_CredentialsIntroText = new System.Windows.Forms.Label();
            this.tpcAuditFolders = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.label15 = new System.Windows.Forms.Label();
            this.ultraTabPageControl_Filters = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this._spltcntnr = new System.Windows.Forms.SplitContainer();
            this.ultraListView_Filters = new Infragistics.Win.UltraWinListView.UltraListView();
            this._hdrstrip = new Idera.SQLsecure.UI.Console.Controls.HeaderStrip();
            this._tsbtn_FilterProperties = new System.Windows.Forms.ToolStripButton();
            this._tsbtn_FilterDelete = new System.Windows.Forms.ToolStripButton();
            this._tsbtn_FilterNew = new System.Windows.Forms.ToolStripButton();
            this._rtbx_FilterDetails = new System.Windows.Forms.RichTextBox();
            this._pnl_FilterDescriptionSep = new Idera.SQLsecure.UI.Console.Controls.GradientPanel();
            this._lbl_DetailsSep = new System.Windows.Forms.Label();
            this._lbl_FilterDescription = new System.Windows.Forms.Label();
            this._pnl_FilterIntro = new System.Windows.Forms.Panel();
            this._lbl_FilterIntroText = new System.Windows.Forms.Label();
            this.ultraTabPageControl_Schedule = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label_AgentStatus = new System.Windows.Forms.Label();
            this.pictureBox_AgentStatus = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this._pnl_ScheduleIntro = new System.Windows.Forms.Panel();
            this._lbl_ScheduleIntroSep = new System.Windows.Forms.Label();
            this._lbl_ScheduleIntroText = new System.Windows.Forms.Label();
            this.label_KeepSnapshotDays = new System.Windows.Forms.Label();
            this.label_KeepSnapshot = new System.Windows.Forms.Label();
            this._grpbx_Schedule = new System.Windows.Forms.GroupBox();
            this.label_Schedule = new System.Windows.Forms.Label();
            this._btn_ChangeSchedule = new Infragistics.Win.Misc.UltraButton();
            this.checkBox_EnableScheduling = new System.Windows.Forms.CheckBox();
            this.numericUpDown_KeepSnapshotDays = new System.Windows.Forms.NumericUpDown();
            this.ultraTabPageControl_Email = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.label12 = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.radioButtonSendEmailFindingHighMedium = new System.Windows.Forms.RadioButton();
            this.radioButtonSendEmailFindingAny = new System.Windows.Forms.RadioButton();
            this.radioButtonSendEmailFindingHigh = new System.Windows.Forms.RadioButton();
            this.textBox_Recipients = new System.Windows.Forms.TextBox();
            this.checkBoxEmailForCollectionStatus = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.radioButton_SendEmailWarningOrError = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.radioButton_SendEmailOnError = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.radioButtonAlways = new System.Windows.Forms.RadioButton();
            this.checkBoxEmailFindings = new System.Windows.Forms.CheckBox();
            this.ultraTabPageControl_Policies = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.ultraListView_DynamicPolicies = new Infragistics.Win.UltraWinListView.UltraListView();
            this.label11 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.ultraListView_Policies = new Infragistics.Win.UltraWinListView.UltraListView();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this._cntxtmn_FilterList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._cntxtmi_FilterNew = new System.Windows.Forms.ToolStripMenuItem();
            this._cntxtmi_FilterDelete = new System.Windows.Forms.ToolStripMenuItem();
            this._cntxtmi_FilterProperties = new System.Windows.Forms.ToolStripMenuItem();
            this._btn_Help = new Infragistics.Win.Misc.UltraButton();
            this.ultraTabControl_ServerProperties = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraButton_OK = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton_Cancel = new Infragistics.Win.Misc.UltraButton();
            this.addEditFoldersControl = new Idera.SQLsecure.UI.Console.Controls.AddEditFolders();
            this._bfd_ButtonPanel.SuspendLayout();
            this._bf_MainPanel.SuspendLayout();
            this.ultraTabPageControl_General.SuspendLayout();
            this._grpbx_NameVersion.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this._grpbx_Snapshot.SuspendLayout();
            this.ultraTabPageControl_Credentials.SuspendLayout();
            this._grpbx_SQLServerCredentials.SuspendLayout();
            this._grpbx_WindowsGMCredentials.SuspendLayout();
            this._pnl_CredentialsIntro.SuspendLayout();
            this.tpcAuditFolders.SuspendLayout();
            this.ultraTabPageControl_Filters.SuspendLayout();
            this._spltcntnr.Panel1.SuspendLayout();
            this._spltcntnr.Panel2.SuspendLayout();
            this._spltcntnr.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraListView_Filters)).BeginInit();
            this._hdrstrip.SuspendLayout();
            this._pnl_FilterDescriptionSep.SuspendLayout();
            this._pnl_FilterIntro.SuspendLayout();
            this.ultraTabPageControl_Schedule.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_AgentStatus)).BeginInit();
            this._pnl_ScheduleIntro.SuspendLayout();
            this._grpbx_Schedule.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_KeepSnapshotDays)).BeginInit();
            this.ultraTabPageControl_Email.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel2.SuspendLayout();
            this.ultraTabPageControl_Policies.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraListView_DynamicPolicies)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraListView_Policies)).BeginInit();
            this.panel1.SuspendLayout();
            this._cntxtmn_FilterList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl_ServerProperties)).BeginInit();
            this.ultraTabControl_ServerProperties.SuspendLayout();
            this.SuspendLayout();
            // 
            // _bfd_ButtonPanel
            // 
            this._bfd_ButtonPanel.Controls.Add(this.ultraButton_Cancel);
            this._bfd_ButtonPanel.Controls.Add(this.ultraButton_OK);
            this._bfd_ButtonPanel.Controls.Add(this._btn_Help);
            this._bfd_ButtonPanel.Location = new System.Drawing.Point(0, 553);
            this._bfd_ButtonPanel.Size = new System.Drawing.Size(514, 40);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this._btn_Help, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this.ultraButton_OK, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this.ultraButton_Cancel, 0);
            // 
            // _bf_MainPanel
            // 
            this._bf_MainPanel.Controls.Add(this.ultraTabControl_ServerProperties);
            this._bf_MainPanel.Size = new System.Drawing.Size(514, 540);
            // 
            // _bf_HeaderPanel
            // 
            this._bf_HeaderPanel.Size = new System.Drawing.Size(514, 53);
            // 
            // ultraTabPageControl_General
            // 
            this.ultraTabPageControl_General.Controls.Add(this._grpbx_NameVersion);
            this.ultraTabPageControl_General.Controls.Add(this.groupBox1);
            this.ultraTabPageControl_General.Controls.Add(this._grpbx_Snapshot);
            this.ultraTabPageControl_General.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl_General.Name = "ultraTabPageControl_General";
            this.ultraTabPageControl_General.Size = new System.Drawing.Size(504, 461);
            // 
            // _grpbx_NameVersion
            // 
            this._grpbx_NameVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._grpbx_NameVersion.Controls.Add(this._lbl_SaVal);
            this._grpbx_NameVersion.Controls.Add(this._lbl_Sa);
            this._grpbx_NameVersion.Controls.Add(this._lbl_ReplicationVal);
            this._grpbx_NameVersion.Controls.Add(this._lbl_Replication);
            this._grpbx_NameVersion.Controls.Add(this._lbl_SQLServerEditionVal);
            this._grpbx_NameVersion.Controls.Add(this._lbl_SQLServerVersionVal);
            this._grpbx_NameVersion.Controls.Add(this._lbl_SQLServerEdition);
            this._grpbx_NameVersion.Controls.Add(this._lbl_SQLServerVersion);
            this._grpbx_NameVersion.Controls.Add(this._lbl_ServerVal);
            this._grpbx_NameVersion.Controls.Add(this._lbl_Server);
            this._grpbx_NameVersion.Location = new System.Drawing.Point(17, 12);
            this._grpbx_NameVersion.Name = "_grpbx_NameVersion";
            this._grpbx_NameVersion.Size = new System.Drawing.Size(471, 156);
            this._grpbx_NameVersion.TabIndex = 12;
            this._grpbx_NameVersion.TabStop = false;
            this._grpbx_NameVersion.Text = "SQL Server";
            // 
            // _lbl_SaVal
            // 
            this._lbl_SaVal.AutoEllipsis = true;
            this._lbl_SaVal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lbl_SaVal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbl_SaVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lbl_SaVal.Location = new System.Drawing.Point(410, 122);
            this._lbl_SaVal.Name = "_lbl_SaVal";
            this._lbl_SaVal.Size = new System.Drawing.Size(55, 15);
            this._lbl_SaVal.TabIndex = 15;
            this._lbl_SaVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lbl_Sa
            // 
            this._lbl_Sa.AutoSize = true;
            this._lbl_Sa.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lbl_Sa.Location = new System.Drawing.Point(256, 123);
            this._lbl_Sa.Name = "_lbl_Sa";
            this._lbl_Sa.Size = new System.Drawing.Size(145, 13);
            this._lbl_Sa.TabIndex = 14;
            this._lbl_Sa.Text = "sa Account Password Empty:";
            // 
            // _lbl_ReplicationVal
            // 
            this._lbl_ReplicationVal.AutoEllipsis = true;
            this._lbl_ReplicationVal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lbl_ReplicationVal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbl_ReplicationVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lbl_ReplicationVal.Location = new System.Drawing.Point(163, 122);
            this._lbl_ReplicationVal.Name = "_lbl_ReplicationVal";
            this._lbl_ReplicationVal.Size = new System.Drawing.Size(48, 15);
            this._lbl_ReplicationVal.TabIndex = 13;
            this._lbl_ReplicationVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lbl_Replication
            // 
            this._lbl_Replication.AutoSize = true;
            this._lbl_Replication.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lbl_Replication.Location = new System.Drawing.Point(52, 123);
            this._lbl_Replication.Name = "_lbl_Replication";
            this._lbl_Replication.Size = new System.Drawing.Size(105, 13);
            this._lbl_Replication.TabIndex = 12;
            this._lbl_Replication.Text = "Replication Enabled:";
            // 
            // _lbl_SQLServerEditionVal
            // 
            this._lbl_SQLServerEditionVal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lbl_SQLServerEditionVal.AutoEllipsis = true;
            this._lbl_SQLServerEditionVal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lbl_SQLServerEditionVal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbl_SQLServerEditionVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lbl_SQLServerEditionVal.Location = new System.Drawing.Point(163, 90);
            this._lbl_SQLServerEditionVal.Name = "_lbl_SQLServerEditionVal";
            this._lbl_SQLServerEditionVal.Size = new System.Drawing.Size(302, 15);
            this._lbl_SQLServerEditionVal.TabIndex = 8;
            this._lbl_SQLServerEditionVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lbl_SQLServerVersionVal
            // 
            this._lbl_SQLServerVersionVal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lbl_SQLServerVersionVal.AutoEllipsis = true;
            this._lbl_SQLServerVersionVal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lbl_SQLServerVersionVal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbl_SQLServerVersionVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lbl_SQLServerVersionVal.Location = new System.Drawing.Point(163, 58);
            this._lbl_SQLServerVersionVal.Name = "_lbl_SQLServerVersionVal";
            this._lbl_SQLServerVersionVal.Size = new System.Drawing.Size(302, 15);
            this._lbl_SQLServerVersionVal.TabIndex = 7;
            this._lbl_SQLServerVersionVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lbl_SQLServerEdition
            // 
            this._lbl_SQLServerEdition.AutoSize = true;
            this._lbl_SQLServerEdition.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lbl_SQLServerEdition.Location = new System.Drawing.Point(57, 91);
            this._lbl_SQLServerEdition.Name = "_lbl_SQLServerEdition";
            this._lbl_SQLServerEdition.Size = new System.Drawing.Size(100, 13);
            this._lbl_SQLServerEdition.TabIndex = 5;
            this._lbl_SQLServerEdition.Text = "SQL Server Edition:";
            // 
            // _lbl_SQLServerVersion
            // 
            this._lbl_SQLServerVersion.AutoSize = true;
            this._lbl_SQLServerVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lbl_SQLServerVersion.Location = new System.Drawing.Point(54, 59);
            this._lbl_SQLServerVersion.Name = "_lbl_SQLServerVersion";
            this._lbl_SQLServerVersion.Size = new System.Drawing.Size(103, 13);
            this._lbl_SQLServerVersion.TabIndex = 4;
            this._lbl_SQLServerVersion.Text = "SQL Server Version:";
            // 
            // _lbl_ServerVal
            // 
            this._lbl_ServerVal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lbl_ServerVal.AutoEllipsis = true;
            this._lbl_ServerVal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lbl_ServerVal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbl_ServerVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lbl_ServerVal.Location = new System.Drawing.Point(163, 26);
            this._lbl_ServerVal.Name = "_lbl_ServerVal";
            this._lbl_ServerVal.Size = new System.Drawing.Size(302, 15);
            this._lbl_ServerVal.TabIndex = 2;
            this._lbl_ServerVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lbl_Server
            // 
            this._lbl_Server.AutoSize = true;
            this._lbl_Server.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lbl_Server.Location = new System.Drawing.Point(116, 27);
            this._lbl_Server.Name = "_lbl_Server";
            this._lbl_Server.Size = new System.Drawing.Size(41, 13);
            this._lbl_Server.TabIndex = 0;
            this._lbl_Server.Text = "Server:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this._lbl_WindowsOSVal);
            this.groupBox1.Controls.Add(this._lbl_DcVal);
            this.groupBox1.Controls.Add(this._lbl_Dc);
            this.groupBox1.Controls.Add(this._lbl_WindowsOS);
            this.groupBox1.Controls.Add(this._lbl_OsServerVal);
            this.groupBox1.Controls.Add(this._lbl_OsServer);
            this.groupBox1.Location = new System.Drawing.Point(17, 175);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(472, 128);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Operating System";
            // 
            // _lbl_WindowsOSVal
            // 
            this._lbl_WindowsOSVal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lbl_WindowsOSVal.AutoEllipsis = true;
            this._lbl_WindowsOSVal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lbl_WindowsOSVal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbl_WindowsOSVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lbl_WindowsOSVal.Location = new System.Drawing.Point(163, 58);
            this._lbl_WindowsOSVal.Name = "_lbl_WindowsOSVal";
            this._lbl_WindowsOSVal.Size = new System.Drawing.Size(303, 15);
            this._lbl_WindowsOSVal.TabIndex = 9;
            this._lbl_WindowsOSVal.Text = "label10";
            this._lbl_WindowsOSVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lbl_DcVal
            // 
            this._lbl_DcVal.AutoEllipsis = true;
            this._lbl_DcVal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lbl_DcVal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbl_DcVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lbl_DcVal.Location = new System.Drawing.Point(163, 91);
            this._lbl_DcVal.Name = "_lbl_DcVal";
            this._lbl_DcVal.Size = new System.Drawing.Size(48, 15);
            this._lbl_DcVal.TabIndex = 11;
            this._lbl_DcVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lbl_Dc
            // 
            this._lbl_Dc.AutoSize = true;
            this._lbl_Dc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lbl_Dc.Location = new System.Drawing.Point(20, 92);
            this._lbl_Dc.Name = "_lbl_Dc";
            this._lbl_Dc.Size = new System.Drawing.Size(137, 13);
            this._lbl_Dc.TabIndex = 10;
            this._lbl_Dc.Text = "Server is Domain Controller:";
            // 
            // _lbl_WindowsOS
            // 
            this._lbl_WindowsOS.AutoSize = true;
            this._lbl_WindowsOS.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lbl_WindowsOS.Location = new System.Drawing.Point(85, 59);
            this._lbl_WindowsOS.Name = "_lbl_WindowsOS";
            this._lbl_WindowsOS.Size = new System.Drawing.Size(72, 13);
            this._lbl_WindowsOS.TabIndex = 6;
            this._lbl_WindowsOS.Text = "Windows OS:";
            // 
            // _lbl_OsServerVal
            // 
            this._lbl_OsServerVal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lbl_OsServerVal.AutoEllipsis = true;
            this._lbl_OsServerVal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lbl_OsServerVal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbl_OsServerVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lbl_OsServerVal.Location = new System.Drawing.Point(163, 26);
            this._lbl_OsServerVal.Name = "_lbl_OsServerVal";
            this._lbl_OsServerVal.Size = new System.Drawing.Size(303, 15);
            this._lbl_OsServerVal.TabIndex = 2;
            this._lbl_OsServerVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lbl_OsServer
            // 
            this._lbl_OsServer.AutoSize = true;
            this._lbl_OsServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lbl_OsServer.Location = new System.Drawing.Point(116, 27);
            this._lbl_OsServer.Name = "_lbl_OsServer";
            this._lbl_OsServer.Size = new System.Drawing.Size(41, 13);
            this._lbl_OsServer.TabIndex = 0;
            this._lbl_OsServer.Text = "Server:";
            // 
            // _grpbx_Snapshot
            // 
            this._grpbx_Snapshot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._grpbx_Snapshot.Controls.Add(this._lbl_LastSuccessfulSnapshotAtVal);
            this._grpbx_Snapshot.Controls.Add(this._lbl_CurrentSnapshotStatusVal);
            this._grpbx_Snapshot.Controls.Add(this._lbl_CurrentSnapshotTimeVal);
            this._grpbx_Snapshot.Controls.Add(this._lbl_LastSuccessfulSnapshotAt);
            this._grpbx_Snapshot.Controls.Add(this._lbl_CurrentSnapshotStatus);
            this._grpbx_Snapshot.Controls.Add(this._lbl_CurrentSnapshotTime);
            this._grpbx_Snapshot.Location = new System.Drawing.Point(17, 310);
            this._grpbx_Snapshot.Name = "_grpbx_Snapshot";
            this._grpbx_Snapshot.Size = new System.Drawing.Size(471, 128);
            this._grpbx_Snapshot.TabIndex = 1;
            this._grpbx_Snapshot.TabStop = false;
            this._grpbx_Snapshot.Text = "Snapshot";
            // 
            // _lbl_LastSuccessfulSnapshotAtVal
            // 
            this._lbl_LastSuccessfulSnapshotAtVal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lbl_LastSuccessfulSnapshotAtVal.AutoEllipsis = true;
            this._lbl_LastSuccessfulSnapshotAtVal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lbl_LastSuccessfulSnapshotAtVal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbl_LastSuccessfulSnapshotAtVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lbl_LastSuccessfulSnapshotAtVal.Location = new System.Drawing.Point(163, 90);
            this._lbl_LastSuccessfulSnapshotAtVal.Name = "_lbl_LastSuccessfulSnapshotAtVal";
            this._lbl_LastSuccessfulSnapshotAtVal.Size = new System.Drawing.Size(302, 15);
            this._lbl_LastSuccessfulSnapshotAtVal.TabIndex = 15;
            this._lbl_LastSuccessfulSnapshotAtVal.Text = "label11";
            this._lbl_LastSuccessfulSnapshotAtVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lbl_CurrentSnapshotStatusVal
            // 
            this._lbl_CurrentSnapshotStatusVal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lbl_CurrentSnapshotStatusVal.AutoEllipsis = true;
            this._lbl_CurrentSnapshotStatusVal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lbl_CurrentSnapshotStatusVal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbl_CurrentSnapshotStatusVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lbl_CurrentSnapshotStatusVal.Location = new System.Drawing.Point(163, 58);
            this._lbl_CurrentSnapshotStatusVal.Name = "_lbl_CurrentSnapshotStatusVal";
            this._lbl_CurrentSnapshotStatusVal.Size = new System.Drawing.Size(302, 15);
            this._lbl_CurrentSnapshotStatusVal.TabIndex = 14;
            this._lbl_CurrentSnapshotStatusVal.Text = "label12";
            this._lbl_CurrentSnapshotStatusVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lbl_CurrentSnapshotTimeVal
            // 
            this._lbl_CurrentSnapshotTimeVal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lbl_CurrentSnapshotTimeVal.AutoEllipsis = true;
            this._lbl_CurrentSnapshotTimeVal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lbl_CurrentSnapshotTimeVal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbl_CurrentSnapshotTimeVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lbl_CurrentSnapshotTimeVal.Location = new System.Drawing.Point(163, 26);
            this._lbl_CurrentSnapshotTimeVal.Name = "_lbl_CurrentSnapshotTimeVal";
            this._lbl_CurrentSnapshotTimeVal.Size = new System.Drawing.Size(302, 15);
            this._lbl_CurrentSnapshotTimeVal.TabIndex = 13;
            this._lbl_CurrentSnapshotTimeVal.Text = "label13";
            this._lbl_CurrentSnapshotTimeVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lbl_LastSuccessfulSnapshotAt
            // 
            this._lbl_LastSuccessfulSnapshotAt.AutoSize = true;
            this._lbl_LastSuccessfulSnapshotAt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lbl_LastSuccessfulSnapshotAt.Location = new System.Drawing.Point(11, 91);
            this._lbl_LastSuccessfulSnapshotAt.Name = "_lbl_LastSuccessfulSnapshotAt";
            this._lbl_LastSuccessfulSnapshotAt.Size = new System.Drawing.Size(146, 13);
            this._lbl_LastSuccessfulSnapshotAt.TabIndex = 12;
            this._lbl_LastSuccessfulSnapshotAt.Text = "Last Successful Snapshot At:";
            // 
            // _lbl_CurrentSnapshotStatus
            // 
            this._lbl_CurrentSnapshotStatus.AutoSize = true;
            this._lbl_CurrentSnapshotStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lbl_CurrentSnapshotStatus.Location = new System.Drawing.Point(32, 59);
            this._lbl_CurrentSnapshotStatus.Name = "_lbl_CurrentSnapshotStatus";
            this._lbl_CurrentSnapshotStatus.Size = new System.Drawing.Size(125, 13);
            this._lbl_CurrentSnapshotStatus.TabIndex = 11;
            this._lbl_CurrentSnapshotStatus.Text = "Current Snapshot Status:";
            // 
            // _lbl_CurrentSnapshotTime
            // 
            this._lbl_CurrentSnapshotTime.AutoSize = true;
            this._lbl_CurrentSnapshotTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lbl_CurrentSnapshotTime.Location = new System.Drawing.Point(39, 27);
            this._lbl_CurrentSnapshotTime.Name = "_lbl_CurrentSnapshotTime";
            this._lbl_CurrentSnapshotTime.Size = new System.Drawing.Size(118, 13);
            this._lbl_CurrentSnapshotTime.TabIndex = 10;
            this._lbl_CurrentSnapshotTime.Text = "Current Snapshot Time:";
            // 
            // ultraTabPageControl_Credentials
            // 
            this.ultraTabPageControl_Credentials.Controls.Add(this._grpbx_SQLServerCredentials);
            this.ultraTabPageControl_Credentials.Controls.Add(this._grpbx_WindowsGMCredentials);
            this.ultraTabPageControl_Credentials.Controls.Add(this._pnl_CredentialsIntro);
            this.ultraTabPageControl_Credentials.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl_Credentials.Name = "ultraTabPageControl_Credentials";
            this.ultraTabPageControl_Credentials.Size = new System.Drawing.Size(504, 461);
            // 
            // _grpbx_SQLServerCredentials
            // 
            this._grpbx_SQLServerCredentials.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._grpbx_SQLServerCredentials.Controls.Add(this.label2);
            this._grpbx_SQLServerCredentials.Controls.Add(this.textBox_SQLWindowsUser);
            this._grpbx_SQLServerCredentials.Controls.Add(this.label9);
            this._grpbx_SQLServerCredentials.Controls.Add(this.textBox_SQLWindowsPassword);
            this._grpbx_SQLServerCredentials.Controls.Add(this.radioButton_WindowsAuth);
            this._grpbx_SQLServerCredentials.Controls.Add(this.radioButton_SQLServerAuth);
            this._grpbx_SQLServerCredentials.Controls.Add(this._lbl_SqlLogin);
            this._grpbx_SQLServerCredentials.Controls.Add(this.textbox_SqlLogin);
            this._grpbx_SQLServerCredentials.Controls.Add(this._lbl_SqlLoginPassword);
            this._grpbx_SQLServerCredentials.Controls.Add(this.textbox_SqlLoginPassword);
            this._grpbx_SQLServerCredentials.Location = new System.Drawing.Point(8, 54);
            this._grpbx_SQLServerCredentials.Name = "_grpbx_SQLServerCredentials";
            this._grpbx_SQLServerCredentials.Size = new System.Drawing.Size(490, 192);
            this._grpbx_SQLServerCredentials.TabIndex = 6;
            this._grpbx_SQLServerCredentials.TabStop = false;
            this._grpbx_SQLServerCredentials.Text = "SQL Server credentials to connect to audited SQL Server";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "&Windows User:";
            // 
            // textBox_SQLWindowsUser
            // 
            this.textBox_SQLWindowsUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_SQLWindowsUser.Location = new System.Drawing.Point(117, 43);
            this.textBox_SQLWindowsUser.Name = "textBox_SQLWindowsUser";
            this.textBox_SQLWindowsUser.Size = new System.Drawing.Size(364, 20);
            this.textBox_SQLWindowsUser.TabIndex = 7;
            this.textBox_SQLWindowsUser.TextChanged += new System.EventHandler(this.textBox_SQLWindowsUser_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(31, 72);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 13);
            this.label9.TabIndex = 5;
            this.label9.Text = "&Password:";
            // 
            // textBox_SQLWindowsPassword
            // 
            this.textBox_SQLWindowsPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_SQLWindowsPassword.Location = new System.Drawing.Point(117, 69);
            this.textBox_SQLWindowsPassword.Name = "textBox_SQLWindowsPassword";
            this.textBox_SQLWindowsPassword.PasswordChar = '*';
            this.textBox_SQLWindowsPassword.Size = new System.Drawing.Size(364, 20);
            this.textBox_SQLWindowsPassword.TabIndex = 8;
            this.textBox_SQLWindowsPassword.TextChanged += new System.EventHandler(this.textBox_SQLWindowsPassword_TextChanged);
            // 
            // radioButton_WindowsAuth
            // 
            this.radioButton_WindowsAuth.AutoSize = true;
            this.radioButton_WindowsAuth.Checked = true;
            this.radioButton_WindowsAuth.Location = new System.Drawing.Point(10, 20);
            this.radioButton_WindowsAuth.Name = "radioButton_WindowsAuth";
            this.radioButton_WindowsAuth.Size = new System.Drawing.Size(140, 17);
            this.radioButton_WindowsAuth.TabIndex = 1;
            this.radioButton_WindowsAuth.TabStop = true;
            this.radioButton_WindowsAuth.Text = "Windows Authentication";
            this.radioButton_WindowsAuth.UseVisualStyleBackColor = true;
            this.radioButton_WindowsAuth.CheckedChanged += new System.EventHandler(this.radioButton_WindowsAuth_CheckedChanged);
            // 
            // radioButton_SQLServerAuth
            // 
            this.radioButton_SQLServerAuth.AutoSize = true;
            this.radioButton_SQLServerAuth.Location = new System.Drawing.Point(10, 109);
            this.radioButton_SQLServerAuth.Name = "radioButton_SQLServerAuth";
            this.radioButton_SQLServerAuth.Size = new System.Drawing.Size(151, 17);
            this.radioButton_SQLServerAuth.TabIndex = 2;
            this.radioButton_SQLServerAuth.TabStop = true;
            this.radioButton_SQLServerAuth.Text = "SQL Server Authentication";
            this.radioButton_SQLServerAuth.UseVisualStyleBackColor = true;
            this.radioButton_SQLServerAuth.CheckedChanged += new System.EventHandler(this.radioButton_SQLServerAuth_CheckedChanged);
            // 
            // _lbl_SqlLogin
            // 
            this._lbl_SqlLogin.AutoSize = true;
            this._lbl_SqlLogin.Location = new System.Drawing.Point(31, 137);
            this._lbl_SqlLogin.Name = "_lbl_SqlLogin";
            this._lbl_SqlLogin.Size = new System.Drawing.Size(67, 13);
            this._lbl_SqlLogin.TabIndex = 0;
            this._lbl_SqlLogin.Text = "&Login Name:";
            // 
            // textbox_SqlLogin
            // 
            this.textbox_SqlLogin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textbox_SqlLogin.Location = new System.Drawing.Point(117, 134);
            this.textbox_SqlLogin.Name = "textbox_SqlLogin";
            this.textbox_SqlLogin.Size = new System.Drawing.Size(364, 20);
            this.textbox_SqlLogin.TabIndex = 3;
            this.textbox_SqlLogin.TextChanged += new System.EventHandler(this._txtbx_SqlLogin_TextChanged);
            // 
            // _lbl_SqlLoginPassword
            // 
            this._lbl_SqlLoginPassword.AutoSize = true;
            this._lbl_SqlLoginPassword.Location = new System.Drawing.Point(31, 163);
            this._lbl_SqlLoginPassword.Name = "_lbl_SqlLoginPassword";
            this._lbl_SqlLoginPassword.Size = new System.Drawing.Size(56, 13);
            this._lbl_SqlLoginPassword.TabIndex = 0;
            this._lbl_SqlLoginPassword.Text = "&Password:";
            // 
            // textbox_SqlLoginPassword
            // 
            this.textbox_SqlLoginPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textbox_SqlLoginPassword.Location = new System.Drawing.Point(117, 160);
            this.textbox_SqlLoginPassword.Name = "textbox_SqlLoginPassword";
            this.textbox_SqlLoginPassword.PasswordChar = '*';
            this.textbox_SqlLoginPassword.Size = new System.Drawing.Size(364, 20);
            this.textbox_SqlLoginPassword.TabIndex = 4;
            this.textbox_SqlLoginPassword.TextChanged += new System.EventHandler(this._txtbx_SqlPassword_TextChanged);
            // 
            // _grpbx_WindowsGMCredentials
            // 
            this._grpbx_WindowsGMCredentials.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._grpbx_WindowsGMCredentials.Controls.Add(this.label10);
            this._grpbx_WindowsGMCredentials.Controls.Add(this.checkBox_UseSameAuth);
            this._grpbx_WindowsGMCredentials.Controls.Add(this._lbl_WindowsUser);
            this._grpbx_WindowsGMCredentials.Controls.Add(this.textbox_WindowsUser);
            this._grpbx_WindowsGMCredentials.Controls.Add(this._lbl_WindowsPassword);
            this._grpbx_WindowsGMCredentials.Controls.Add(this.textbox_WindowsPassword);
            this._grpbx_WindowsGMCredentials.Location = new System.Drawing.Point(8, 268);
            this._grpbx_WindowsGMCredentials.Name = "_grpbx_WindowsGMCredentials";
            this._grpbx_WindowsGMCredentials.Size = new System.Drawing.Size(490, 173);
            this._grpbx_WindowsGMCredentials.TabIndex = 7;
            this._grpbx_WindowsGMCredentials.TabStop = false;
            this._grpbx_WindowsGMCredentials.Text = "Windows Credentials to gather Operating System and Active Directory objects";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(10, 20);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(454, 66);
            this.label10.TabIndex = 6;
            this.label10.Text = resources.GetString("label10.Text");
            // 
            // checkBox_UseSameAuth
            // 
            this.checkBox_UseSameAuth.AutoSize = true;
            this.checkBox_UseSameAuth.Location = new System.Drawing.Point(10, 89);
            this.checkBox_UseSameAuth.Name = "checkBox_UseSameAuth";
            this.checkBox_UseSameAuth.Size = new System.Drawing.Size(238, 17);
            this.checkBox_UseSameAuth.TabIndex = 0;
            this.checkBox_UseSameAuth.Text = "Use same Windows Authentication as above";
            this.checkBox_UseSameAuth.UseVisualStyleBackColor = true;
            this.checkBox_UseSameAuth.CheckedChanged += new System.EventHandler(this.checkBox_UseSameAuth_CheckedChanged);
            // 
            // _lbl_WindowsUser
            // 
            this._lbl_WindowsUser.AutoSize = true;
            this._lbl_WindowsUser.Location = new System.Drawing.Point(10, 117);
            this._lbl_WindowsUser.Name = "_lbl_WindowsUser";
            this._lbl_WindowsUser.Size = new System.Drawing.Size(79, 13);
            this._lbl_WindowsUser.TabIndex = 3;
            this._lbl_WindowsUser.Text = "Windows &User:";
            // 
            // textbox_WindowsUser
            // 
            this.textbox_WindowsUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textbox_WindowsUser.Location = new System.Drawing.Point(117, 114);
            this.textbox_WindowsUser.Name = "textbox_WindowsUser";
            this.textbox_WindowsUser.Size = new System.Drawing.Size(364, 20);
            this.textbox_WindowsUser.TabIndex = 1;
            this.textbox_WindowsUser.TextChanged += new System.EventHandler(this._txtbx_WindowsUser_TextChanged);
            // 
            // _lbl_WindowsPassword
            // 
            this._lbl_WindowsPassword.AutoSize = true;
            this._lbl_WindowsPassword.Location = new System.Drawing.Point(10, 143);
            this._lbl_WindowsPassword.Name = "_lbl_WindowsPassword";
            this._lbl_WindowsPassword.Size = new System.Drawing.Size(56, 13);
            this._lbl_WindowsPassword.TabIndex = 4;
            this._lbl_WindowsPassword.Text = "P&assword:";
            // 
            // textbox_WindowsPassword
            // 
            this.textbox_WindowsPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textbox_WindowsPassword.Location = new System.Drawing.Point(117, 140);
            this.textbox_WindowsPassword.Name = "textbox_WindowsPassword";
            this.textbox_WindowsPassword.PasswordChar = '*';
            this.textbox_WindowsPassword.Size = new System.Drawing.Size(364, 20);
            this.textbox_WindowsPassword.TabIndex = 2;
            this.textbox_WindowsPassword.TextChanged += new System.EventHandler(this._txtbx_WindowsPwd_TextChanged);
            // 
            // _pnl_CredentialsIntro
            // 
            this._pnl_CredentialsIntro.Controls.Add(this._lbl_CredentialsIntroSep);
            this._pnl_CredentialsIntro.Controls.Add(this._lbl_CredentialsIntroText);
            this._pnl_CredentialsIntro.Dock = System.Windows.Forms.DockStyle.Top;
            this._pnl_CredentialsIntro.Location = new System.Drawing.Point(0, 0);
            this._pnl_CredentialsIntro.Name = "_pnl_CredentialsIntro";
            this._pnl_CredentialsIntro.Padding = new System.Windows.Forms.Padding(3);
            this._pnl_CredentialsIntro.Size = new System.Drawing.Size(504, 40);
            this._pnl_CredentialsIntro.TabIndex = 5;
            // 
            // _lbl_CredentialsIntroSep
            // 
            this._lbl_CredentialsIntroSep.BackColor = System.Drawing.Color.Navy;
            this._lbl_CredentialsIntroSep.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._lbl_CredentialsIntroSep.Location = new System.Drawing.Point(3, 36);
            this._lbl_CredentialsIntroSep.Name = "_lbl_CredentialsIntroSep";
            this._lbl_CredentialsIntroSep.Size = new System.Drawing.Size(498, 1);
            this._lbl_CredentialsIntroSep.TabIndex = 1;
            // 
            // _lbl_CredentialsIntroText
            // 
            this._lbl_CredentialsIntroText.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lbl_CredentialsIntroText.Location = new System.Drawing.Point(3, 3);
            this._lbl_CredentialsIntroText.Name = "_lbl_CredentialsIntroText";
            this._lbl_CredentialsIntroText.Size = new System.Drawing.Size(498, 34);
            this._lbl_CredentialsIntroText.TabIndex = 0;
            this._lbl_CredentialsIntroText.Text = "This window allows you to change the credentials that are used to collect data fo" +
                "r auditing. \r\n";
            // 
            // tpcAuditFolders
            // 
            this.tpcAuditFolders.Controls.Add(this.addEditFoldersControl);
            this.tpcAuditFolders.Controls.Add(this.label15);
            this.tpcAuditFolders.Location = new System.Drawing.Point(2, 24);
            this.tpcAuditFolders.Name = "tpcAuditFolders";
            this.tpcAuditFolders.Size = new System.Drawing.Size(504, 461);
            // 
            // label15
            // 
            this.label15.ForeColor = System.Drawing.Color.Navy;
            this.label15.Location = new System.Drawing.Point(3, 3);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(498, 34);
            this.label15.TabIndex = 0;
            this.label15.Text = "This window allows you to view and modify audit folders.  You can add, edit or re" +
                "move folders.";
            // 
            // ultraTabPageControl_Filters
            // 
            this.ultraTabPageControl_Filters.Controls.Add(this._spltcntnr);
            this.ultraTabPageControl_Filters.Controls.Add(this._pnl_FilterIntro);
            this.ultraTabPageControl_Filters.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl_Filters.Name = "ultraTabPageControl_Filters";
            this.ultraTabPageControl_Filters.Size = new System.Drawing.Size(504, 461);
            // 
            // _spltcntnr
            // 
            this._spltcntnr.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._spltcntnr.Dock = System.Windows.Forms.DockStyle.Fill;
            this._spltcntnr.Location = new System.Drawing.Point(0, 33);
            this._spltcntnr.Name = "_spltcntnr";
            this._spltcntnr.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // _spltcntnr.Panel1
            // 
            this._spltcntnr.Panel1.Controls.Add(this.ultraListView_Filters);
            this._spltcntnr.Panel1.Controls.Add(this._hdrstrip);
            // 
            // _spltcntnr.Panel2
            // 
            this._spltcntnr.Panel2.BackColor = System.Drawing.Color.White;
            this._spltcntnr.Panel2.Controls.Add(this._rtbx_FilterDetails);
            this._spltcntnr.Panel2.Controls.Add(this._pnl_FilterDescriptionSep);
            this._spltcntnr.Size = new System.Drawing.Size(504, 428);
            this._spltcntnr.SplitterDistance = 243;
            this._spltcntnr.SplitterWidth = 2;
            this._spltcntnr.TabIndex = 1;
            this._spltcntnr.MouseDown += new System.Windows.Forms.MouseEventHandler(this._spltcntnr_MouseDown);
            this._spltcntnr.MouseUp += new System.Windows.Forms.MouseEventHandler(this._spltcntnr_MouseUp);
            // 
            // ultraListView_Filters
            // 
            this.ultraListView_Filters.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.ultraListView_Filters.Dock = System.Windows.Forms.DockStyle.Fill;
            appearance5.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.funnel;
            this.ultraListView_Filters.ItemSettings.Appearance = appearance5;
            this.ultraListView_Filters.Location = new System.Drawing.Point(0, 19);
            this.ultraListView_Filters.MainColumn.Text = "Filter";
            this.ultraListView_Filters.MainColumn.VisiblePositionInDetailsView = 0;
            this.ultraListView_Filters.MainColumn.Width = 200;
            this.ultraListView_Filters.Name = "ultraListView_Filters";
            this.ultraListView_Filters.Size = new System.Drawing.Size(504, 224);
            ultraListViewSubItemColumn1.Key = "colDescription";
            ultraListViewSubItemColumn1.Text = "Description";
            ultraListViewSubItemColumn1.VisiblePositionInDetailsView = 1;
            ultraListViewSubItemColumn1.Width = 300;
            this.ultraListView_Filters.SubItemColumns.AddRange(new Infragistics.Win.UltraWinListView.UltraListViewSubItemColumn[] {
            ultraListViewSubItemColumn1});
            this.ultraListView_Filters.TabIndex = 3;
            this.ultraListView_Filters.Text = "ultraListView1";
            this.ultraListView_Filters.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.Details;
            this.ultraListView_Filters.KeyDown += new System.Windows.Forms.KeyEventHandler(this._lstvw_Filters_KeyDown);
            this.ultraListView_Filters.DoubleClick += new System.EventHandler(this.ultraListView_Filters_DoubleClick);
            this.ultraListView_Filters.ItemSelectionChanged += new Infragistics.Win.UltraWinListView.ItemSelectionChangedEventHandler(this.ultraListView_Filters_ItemSelectionChanged);
            // 
            // _hdrstrip
            // 
            this._hdrstrip.AutoSize = false;
            this._hdrstrip.BackColor = System.Drawing.Color.Transparent;
            this._hdrstrip.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this._hdrstrip.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this._hdrstrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._hdrstrip.HeaderStyle = Idera.SQLsecure.UI.Console.Controls.AreaHeaderStyle.Small;
            this._hdrstrip.HotTrackEnabled = false;
            this._hdrstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._tsbtn_FilterProperties,
            this._tsbtn_FilterDelete,
            this._tsbtn_FilterNew});
            this._hdrstrip.Location = new System.Drawing.Point(0, 0);
            this._hdrstrip.Name = "_hdrstrip";
            this._hdrstrip.Size = new System.Drawing.Size(504, 19);
            this._hdrstrip.TabIndex = 2;
            this._hdrstrip.Text = "headerStrip1";
            // 
            // _tsbtn_FilterProperties
            // 
            this._tsbtn_FilterProperties.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._tsbtn_FilterProperties.Image = ((System.Drawing.Image)(resources.GetObject("_tsbtn_FilterProperties.Image")));
            this._tsbtn_FilterProperties.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._tsbtn_FilterProperties.Name = "_tsbtn_FilterProperties";
            this._tsbtn_FilterProperties.Size = new System.Drawing.Size(76, 16);
            this._tsbtn_FilterProperties.Text = "Properties";
            this._tsbtn_FilterProperties.Click += new System.EventHandler(this._tsbtn_FilterProperties_Click);
            // 
            // _tsbtn_FilterDelete
            // 
            this._tsbtn_FilterDelete.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._tsbtn_FilterDelete.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.funnel_delete;
            this._tsbtn_FilterDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._tsbtn_FilterDelete.Name = "_tsbtn_FilterDelete";
            this._tsbtn_FilterDelete.Size = new System.Drawing.Size(85, 16);
            this._tsbtn_FilterDelete.Text = "Delete Filter";
            this._tsbtn_FilterDelete.Click += new System.EventHandler(this._tsbtn_FilterDelete_Click);
            // 
            // _tsbtn_FilterNew
            // 
            this._tsbtn_FilterNew.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._tsbtn_FilterNew.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.funnel_add;
            this._tsbtn_FilterNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._tsbtn_FilterNew.Name = "_tsbtn_FilterNew";
            this._tsbtn_FilterNew.Size = new System.Drawing.Size(75, 16);
            this._tsbtn_FilterNew.Text = "New Filter";
            this._tsbtn_FilterNew.Click += new System.EventHandler(this._tsbtn_FilterNew_Click);
            // 
            // _rtbx_FilterDetails
            // 
            this._rtbx_FilterDetails.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this._rtbx_FilterDetails.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._rtbx_FilterDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this._rtbx_FilterDetails.Location = new System.Drawing.Point(0, 23);
            this._rtbx_FilterDetails.Name = "_rtbx_FilterDetails";
            this._rtbx_FilterDetails.ReadOnly = true;
            this._rtbx_FilterDetails.Size = new System.Drawing.Size(504, 160);
            this._rtbx_FilterDetails.TabIndex = 1;
            this._rtbx_FilterDetails.TabStop = false;
            this._rtbx_FilterDetails.Text = "";
            // 
            // _pnl_FilterDescriptionSep
            // 
            this._pnl_FilterDescriptionSep.BackColor = System.Drawing.Color.Transparent;
            this._pnl_FilterDescriptionSep.Controls.Add(this._lbl_DetailsSep);
            this._pnl_FilterDescriptionSep.Controls.Add(this._lbl_FilterDescription);
            this._pnl_FilterDescriptionSep.Dock = System.Windows.Forms.DockStyle.Top;
            this._pnl_FilterDescriptionSep.GradientColor = System.Drawing.Color.LightGray;
            this._pnl_FilterDescriptionSep.Location = new System.Drawing.Point(0, 0);
            this._pnl_FilterDescriptionSep.Name = "_pnl_FilterDescriptionSep";
            this._pnl_FilterDescriptionSep.Rotation = 270F;
            this._pnl_FilterDescriptionSep.Size = new System.Drawing.Size(504, 23);
            this._pnl_FilterDescriptionSep.TabIndex = 0;
            // 
            // _lbl_DetailsSep
            // 
            this._lbl_DetailsSep.BackColor = System.Drawing.Color.Black;
            this._lbl_DetailsSep.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._lbl_DetailsSep.Location = new System.Drawing.Point(0, 22);
            this._lbl_DetailsSep.Name = "_lbl_DetailsSep";
            this._lbl_DetailsSep.Size = new System.Drawing.Size(504, 1);
            this._lbl_DetailsSep.TabIndex = 1;
            // 
            // _lbl_FilterDescription
            // 
            this._lbl_FilterDescription.AutoSize = true;
            this._lbl_FilterDescription.BackColor = System.Drawing.Color.Transparent;
            this._lbl_FilterDescription.Location = new System.Drawing.Point(0, 5);
            this._lbl_FilterDescription.Name = "_lbl_FilterDescription";
            this._lbl_FilterDescription.Size = new System.Drawing.Size(62, 13);
            this._lbl_FilterDescription.TabIndex = 0;
            this._lbl_FilterDescription.Text = "Filter details";
            // 
            // _pnl_FilterIntro
            // 
            this._pnl_FilterIntro.Controls.Add(this._lbl_FilterIntroText);
            this._pnl_FilterIntro.Dock = System.Windows.Forms.DockStyle.Top;
            this._pnl_FilterIntro.Location = new System.Drawing.Point(0, 0);
            this._pnl_FilterIntro.Name = "_pnl_FilterIntro";
            this._pnl_FilterIntro.Padding = new System.Windows.Forms.Padding(3);
            this._pnl_FilterIntro.Size = new System.Drawing.Size(504, 33);
            this._pnl_FilterIntro.TabIndex = 4;
            // 
            // _lbl_FilterIntroText
            // 
            this._lbl_FilterIntroText.BackColor = System.Drawing.Color.Transparent;
            this._lbl_FilterIntroText.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lbl_FilterIntroText.Location = new System.Drawing.Point(3, 3);
            this._lbl_FilterIntroText.Name = "_lbl_FilterIntroText";
            this._lbl_FilterIntroText.Size = new System.Drawing.Size(498, 27);
            this._lbl_FilterIntroText.TabIndex = 0;
            this._lbl_FilterIntroText.Text = "This window allows you to view and modify audit filters.  You can add, delete, or" +
                " edit filter properties.";
            // 
            // ultraTabPageControl_Schedule
            // 
            this.ultraTabPageControl_Schedule.Controls.Add(this.groupBox2);
            this.ultraTabPageControl_Schedule.Controls.Add(this._pnl_ScheduleIntro);
            this.ultraTabPageControl_Schedule.Controls.Add(this.label_KeepSnapshotDays);
            this.ultraTabPageControl_Schedule.Controls.Add(this.label_KeepSnapshot);
            this.ultraTabPageControl_Schedule.Controls.Add(this._grpbx_Schedule);
            this.ultraTabPageControl_Schedule.Controls.Add(this.numericUpDown_KeepSnapshotDays);
            this.ultraTabPageControl_Schedule.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl_Schedule.Name = "ultraTabPageControl_Schedule";
            this.ultraTabPageControl_Schedule.Size = new System.Drawing.Size(504, 461);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.label_AgentStatus);
            this.groupBox2.Controls.Add(this.pictureBox_AgentStatus);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(10, 337);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(492, 106);
            this.groupBox2.TabIndex = 1023;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "SQL Server Agent";
            // 
            // label_AgentStatus
            // 
            this.label_AgentStatus.AutoSize = true;
            this.label_AgentStatus.BackColor = System.Drawing.Color.Transparent;
            this.label_AgentStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_AgentStatus.Location = new System.Drawing.Point(21, 84);
            this.label_AgentStatus.Name = "label_AgentStatus";
            this.label_AgentStatus.Size = new System.Drawing.Size(48, 13);
            this.label_AgentStatus.TabIndex = 14;
            this.label_AgentStatus.Text = "Started";
            // 
            // pictureBox_AgentStatus
            // 
            this.pictureBox_AgentStatus.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_AgentStatus.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_AgentStatus.Image")));
            this.pictureBox_AgentStatus.Location = new System.Drawing.Point(21, 24);
            this.pictureBox_AgentStatus.Name = "pictureBox_AgentStatus";
            this.pictureBox_AgentStatus.Size = new System.Drawing.Size(48, 56);
            this.pictureBox_AgentStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox_AgentStatus.TabIndex = 13;
            this.pictureBox_AgentStatus.TabStop = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(102, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(371, 59);
            this.label1.TabIndex = 12;
            this.label1.Text = "SQLsecure uses the SQL Server Agent for data collection and grooming. This agent " +
                "is located on the SQL server hosting the Repository database.";
            // 
            // _pnl_ScheduleIntro
            // 
            this._pnl_ScheduleIntro.Controls.Add(this._lbl_ScheduleIntroSep);
            this._pnl_ScheduleIntro.Controls.Add(this._lbl_ScheduleIntroText);
            this._pnl_ScheduleIntro.Dock = System.Windows.Forms.DockStyle.Top;
            this._pnl_ScheduleIntro.Location = new System.Drawing.Point(0, 0);
            this._pnl_ScheduleIntro.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this._pnl_ScheduleIntro.Name = "_pnl_ScheduleIntro";
            this._pnl_ScheduleIntro.Padding = new System.Windows.Forms.Padding(3);
            this._pnl_ScheduleIntro.Size = new System.Drawing.Size(504, 39);
            this._pnl_ScheduleIntro.TabIndex = 9;
            // 
            // _lbl_ScheduleIntroSep
            // 
            this._lbl_ScheduleIntroSep.BackColor = System.Drawing.Color.Navy;
            this._lbl_ScheduleIntroSep.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._lbl_ScheduleIntroSep.Location = new System.Drawing.Point(3, 35);
            this._lbl_ScheduleIntroSep.Name = "_lbl_ScheduleIntroSep";
            this._lbl_ScheduleIntroSep.Size = new System.Drawing.Size(498, 1);
            this._lbl_ScheduleIntroSep.TabIndex = 1;
            // 
            // _lbl_ScheduleIntroText
            // 
            this._lbl_ScheduleIntroText.BackColor = System.Drawing.Color.Transparent;
            this._lbl_ScheduleIntroText.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lbl_ScheduleIntroText.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._lbl_ScheduleIntroText.Location = new System.Drawing.Point(3, 3);
            this._lbl_ScheduleIntroText.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this._lbl_ScheduleIntroText.Name = "_lbl_ScheduleIntroText";
            this._lbl_ScheduleIntroText.Size = new System.Drawing.Size(498, 33);
            this._lbl_ScheduleIntroText.TabIndex = 0;
            this._lbl_ScheduleIntroText.Text = "This window allows you to modify the audit data collection schedule.";
            // 
            // label_KeepSnapshotDays
            // 
            this.label_KeepSnapshotDays.AutoSize = true;
            this.label_KeepSnapshotDays.Location = new System.Drawing.Point(169, 49);
            this.label_KeepSnapshotDays.Name = "label_KeepSnapshotDays";
            this.label_KeepSnapshotDays.Size = new System.Drawing.Size(282, 13);
            this.label_KeepSnapshotDays.TabIndex = 1022;
            this.label_KeepSnapshotDays.Text = "days before allowing them to be groomed. (1 - 10000 days)";
            // 
            // label_KeepSnapshot
            // 
            this.label_KeepSnapshot.AutoSize = true;
            this.label_KeepSnapshot.Location = new System.Drawing.Point(7, 49);
            this.label_KeepSnapshot.Name = "label_KeepSnapshot";
            this.label_KeepSnapshot.Size = new System.Drawing.Size(98, 13);
            this.label_KeepSnapshot.TabIndex = 1020;
            this.label_KeepSnapshot.Text = "&Keep snapshots for";
            // 
            // _grpbx_Schedule
            // 
            this._grpbx_Schedule.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._grpbx_Schedule.BackColor = System.Drawing.Color.Transparent;
            this._grpbx_Schedule.Controls.Add(this.label_Schedule);
            this._grpbx_Schedule.Controls.Add(this._btn_ChangeSchedule);
            this._grpbx_Schedule.Controls.Add(this.checkBox_EnableScheduling);
            this._grpbx_Schedule.Location = new System.Drawing.Point(10, 79);
            this._grpbx_Schedule.Name = "_grpbx_Schedule";
            this._grpbx_Schedule.Size = new System.Drawing.Size(488, 235);
            this._grpbx_Schedule.TabIndex = 8;
            this._grpbx_Schedule.TabStop = false;
            this._grpbx_Schedule.Text = "Collection Schedule";
            // 
            // label_Schedule
            // 
            this.label_Schedule.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Schedule.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.label_Schedule.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_Schedule.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label_Schedule.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.label_Schedule.Location = new System.Drawing.Point(6, 48);
            this.label_Schedule.Name = "label_Schedule";
            this.label_Schedule.Size = new System.Drawing.Size(383, 164);
            this.label_Schedule.TabIndex = 1020;
            this.label_Schedule.Text = "label_Schedule";
            // 
            // _btn_ChangeSchedule
            // 
            this._btn_ChangeSchedule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btn_ChangeSchedule.Location = new System.Drawing.Point(402, 189);
            this._btn_ChangeSchedule.Name = "_btn_ChangeSchedule";
            this._btn_ChangeSchedule.Size = new System.Drawing.Size(80, 23);
            this._btn_ChangeSchedule.TabIndex = 0;
            this._btn_ChangeSchedule.Text = "C&hange...";
            this._btn_ChangeSchedule.Click += new System.EventHandler(this._btn_ChangeSchedule_Click);
            // 
            // checkBox_EnableScheduling
            // 
            this.checkBox_EnableScheduling.AutoSize = true;
            this.checkBox_EnableScheduling.Location = new System.Drawing.Point(6, 19);
            this.checkBox_EnableScheduling.Name = "checkBox_EnableScheduling";
            this.checkBox_EnableScheduling.Size = new System.Drawing.Size(115, 17);
            this.checkBox_EnableScheduling.TabIndex = 1019;
            this.checkBox_EnableScheduling.Text = "&Enable Scheduling";
            this.checkBox_EnableScheduling.UseVisualStyleBackColor = true;
            this.checkBox_EnableScheduling.CheckedChanged += new System.EventHandler(this.checkBox_EnableScheduling_CheckedChanged);
            // 
            // numericUpDown_KeepSnapshotDays
            // 
            this.numericUpDown_KeepSnapshotDays.Location = new System.Drawing.Point(107, 46);
            this.numericUpDown_KeepSnapshotDays.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown_KeepSnapshotDays.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_KeepSnapshotDays.Name = "numericUpDown_KeepSnapshotDays";
            this.numericUpDown_KeepSnapshotDays.Size = new System.Drawing.Size(58, 20);
            this.numericUpDown_KeepSnapshotDays.TabIndex = 1021;
            this.numericUpDown_KeepSnapshotDays.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_KeepSnapshotDays.ValueChanged += new System.EventHandler(this.numericUpDown_KeepSnapshotDays_ValueChanged);
            // 
            // ultraTabPageControl_Email
            // 
            this.ultraTabPageControl_Email.Controls.Add(this.label12);
            this.ultraTabPageControl_Email.Controls.Add(this.panel7);
            this.ultraTabPageControl_Email.Controls.Add(this.panel2);
            this.ultraTabPageControl_Email.Controls.Add(this.textBox_Recipients);
            this.ultraTabPageControl_Email.Controls.Add(this.checkBoxEmailForCollectionStatus);
            this.ultraTabPageControl_Email.Controls.Add(this.label8);
            this.ultraTabPageControl_Email.Controls.Add(this.radioButton_SendEmailWarningOrError);
            this.ultraTabPageControl_Email.Controls.Add(this.label7);
            this.ultraTabPageControl_Email.Controls.Add(this.radioButton_SendEmailOnError);
            this.ultraTabPageControl_Email.Controls.Add(this.label6);
            this.ultraTabPageControl_Email.Controls.Add(this.radioButtonAlways);
            this.ultraTabPageControl_Email.Controls.Add(this.checkBoxEmailFindings);
            this.ultraTabPageControl_Email.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl_Email.Name = "ultraTabPageControl_Email";
            this.ultraTabPageControl_Email.Size = new System.Drawing.Size(504, 461);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(96, 305);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(379, 13);
            this.label12.TabIndex = 24;
            this.label12.Text = "( specify multiple email recipients by separating each address with a semicolon )" +
                "";
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.label13);
            this.panel7.Controls.Add(this.label14);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.panel7.Name = "panel7";
            this.panel7.Padding = new System.Windows.Forms.Padding(3);
            this.panel7.Size = new System.Drawing.Size(504, 39);
            this.panel7.TabIndex = 11;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.Navy;
            this.label13.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label13.Location = new System.Drawing.Point(3, 35);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(498, 1);
            this.label13.TabIndex = 1;
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label14.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label14.Location = new System.Drawing.Point(3, 3);
            this.label14.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(498, 33);
            this.label14.TabIndex = 0;
            this.label14.Text = "This window allows you to configure Email notification for Snapshots (data collec" +
                "tions).";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.radioButtonSendEmailFindingHighMedium);
            this.panel2.Controls.Add(this.radioButtonSendEmailFindingAny);
            this.panel2.Controls.Add(this.radioButtonSendEmailFindingHigh);
            this.panel2.Location = new System.Drawing.Point(11, 177);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(431, 93);
            this.panel2.TabIndex = 23;
            // 
            // radioButtonSendEmailFindingHighMedium
            // 
            this.radioButtonSendEmailFindingHighMedium.AutoSize = true;
            this.radioButtonSendEmailFindingHighMedium.Location = new System.Drawing.Point(17, 33);
            this.radioButtonSendEmailFindingHighMedium.Name = "radioButtonSendEmailFindingHighMedium";
            this.radioButtonSendEmailFindingHighMedium.Size = new System.Drawing.Size(154, 17);
            this.radioButtonSendEmailFindingHighMedium.TabIndex = 7;
            this.radioButtonSendEmailFindingHighMedium.Text = "On High and Medium Risks";
            this.radioButtonSendEmailFindingHighMedium.UseVisualStyleBackColor = true;
            this.radioButtonSendEmailFindingHighMedium.CheckedChanged += new System.EventHandler(this.RadioButtonChanged);
            // 
            // radioButtonSendEmailFindingAny
            // 
            this.radioButtonSendEmailFindingAny.AutoSize = true;
            this.radioButtonSendEmailFindingAny.Checked = true;
            this.radioButtonSendEmailFindingAny.Location = new System.Drawing.Point(17, 10);
            this.radioButtonSendEmailFindingAny.Name = "radioButtonSendEmailFindingAny";
            this.radioButtonSendEmailFindingAny.Size = new System.Drawing.Size(67, 17);
            this.radioButtonSendEmailFindingAny.TabIndex = 6;
            this.radioButtonSendEmailFindingAny.TabStop = true;
            this.radioButtonSendEmailFindingAny.Text = "Any Risk";
            this.radioButtonSendEmailFindingAny.UseVisualStyleBackColor = true;
            this.radioButtonSendEmailFindingAny.CheckedChanged += new System.EventHandler(this.RadioButtonChanged);
            // 
            // radioButtonSendEmailFindingHigh
            // 
            this.radioButtonSendEmailFindingHigh.AutoSize = true;
            this.radioButtonSendEmailFindingHigh.Location = new System.Drawing.Point(17, 56);
            this.radioButtonSendEmailFindingHigh.Name = "radioButtonSendEmailFindingHigh";
            this.radioButtonSendEmailFindingHigh.Size = new System.Drawing.Size(115, 17);
            this.radioButtonSendEmailFindingHigh.TabIndex = 8;
            this.radioButtonSendEmailFindingHigh.Text = "Only on High Risks";
            this.radioButtonSendEmailFindingHigh.UseVisualStyleBackColor = true;
            this.radioButtonSendEmailFindingHigh.CheckedChanged += new System.EventHandler(this.RadioButtonChanged);
            // 
            // textBox_Recipients
            // 
            this.textBox_Recipients.Location = new System.Drawing.Point(95, 282);
            this.textBox_Recipients.Name = "textBox_Recipients";
            this.textBox_Recipients.Size = new System.Drawing.Size(394, 20);
            this.textBox_Recipients.TabIndex = 19;
            this.textBox_Recipients.TextChanged += new System.EventHandler(this.textBox_Recipients_TextChanged);
            // 
            // checkBoxEmailForCollectionStatus
            // 
            this.checkBoxEmailForCollectionStatus.AutoSize = true;
            this.checkBoxEmailForCollectionStatus.Location = new System.Drawing.Point(8, 57);
            this.checkBoxEmailForCollectionStatus.Name = "checkBoxEmailForCollectionStatus";
            this.checkBoxEmailForCollectionStatus.Size = new System.Drawing.Size(234, 17);
            this.checkBoxEmailForCollectionStatus.TabIndex = 13;
            this.checkBoxEmailForCollectionStatus.Text = "Send Email Notification after Data Collection";
            this.checkBoxEmailForCollectionStatus.UseVisualStyleBackColor = true;
            this.checkBoxEmailForCollectionStatus.CheckedChanged += new System.EventHandler(this.checkBoxEmailForCollectionStatus_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 285);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 13);
            this.label8.TabIndex = 22;
            this.label8.Text = "Email Recipient:";
            // 
            // radioButton_SendEmailWarningOrError
            // 
            this.radioButton_SendEmailWarningOrError.AutoSize = true;
            this.radioButton_SendEmailWarningOrError.Location = new System.Drawing.Point(28, 101);
            this.radioButton_SendEmailWarningOrError.Name = "radioButton_SendEmailWarningOrError";
            this.radioButton_SendEmailWarningOrError.Size = new System.Drawing.Size(128, 17);
            this.radioButton_SendEmailWarningOrError.TabIndex = 15;
            this.radioButton_SendEmailWarningOrError.Text = "On Warning and Error";
            this.radioButton_SendEmailWarningOrError.UseVisualStyleBackColor = true;
            this.radioButton_SendEmailWarningOrError.CheckedChanged += new System.EventHandler(this.RadioButtonChanged);
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Navy;
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label7.Location = new System.Drawing.Point(242, 170);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(245, 3);
            this.label7.TabIndex = 21;
            // 
            // radioButton_SendEmailOnError
            // 
            this.radioButton_SendEmailOnError.AutoSize = true;
            this.radioButton_SendEmailOnError.Location = new System.Drawing.Point(28, 124);
            this.radioButton_SendEmailOnError.Name = "radioButton_SendEmailOnError";
            this.radioButton_SendEmailOnError.Size = new System.Drawing.Size(88, 17);
            this.radioButton_SendEmailOnError.TabIndex = 16;
            this.radioButton_SendEmailOnError.Text = "Only On Error";
            this.radioButton_SendEmailOnError.UseVisualStyleBackColor = true;
            this.radioButton_SendEmailOnError.CheckedChanged += new System.EventHandler(this.RadioButtonChanged);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Navy;
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label6.Location = new System.Drawing.Point(242, 65);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(245, 3);
            this.label6.TabIndex = 20;
            // 
            // radioButtonAlways
            // 
            this.radioButtonAlways.AutoSize = true;
            this.radioButtonAlways.Checked = true;
            this.radioButtonAlways.Location = new System.Drawing.Point(28, 78);
            this.radioButtonAlways.Name = "radioButtonAlways";
            this.radioButtonAlways.Size = new System.Drawing.Size(58, 17);
            this.radioButtonAlways.TabIndex = 14;
            this.radioButtonAlways.TabStop = true;
            this.radioButtonAlways.Text = "Always";
            this.radioButtonAlways.UseVisualStyleBackColor = true;
            this.radioButtonAlways.CheckedChanged += new System.EventHandler(this.RadioButtonChanged);
            // 
            // checkBoxEmailFindings
            // 
            this.checkBoxEmailFindings.AutoSize = true;
            this.checkBoxEmailFindings.Location = new System.Drawing.Point(8, 162);
            this.checkBoxEmailFindings.Name = "checkBoxEmailFindings";
            this.checkBoxEmailFindings.Size = new System.Drawing.Size(233, 17);
            this.checkBoxEmailFindings.TabIndex = 17;
            this.checkBoxEmailFindings.Text = "Send Email Notification for Security Findings";
            this.checkBoxEmailFindings.UseVisualStyleBackColor = true;
            this.checkBoxEmailFindings.CheckedChanged += new System.EventHandler(this.checkBoxEmailFindings_CheckedChanged);
            // 
            // ultraTabPageControl_Policies
            // 
            this.ultraTabPageControl_Policies.Controls.Add(this.panel3);
            this.ultraTabPageControl_Policies.Controls.Add(this.panel1);
            this.ultraTabPageControl_Policies.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl_Policies.Name = "ultraTabPageControl_Policies";
            this.ultraTabPageControl_Policies.Size = new System.Drawing.Size(504, 461);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 39);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(504, 422);
            this.panel3.TabIndex = 11;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 185);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(504, 237);
            this.panel5.TabIndex = 5;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.ultraListView_DynamicPolicies);
            this.panel6.Controls.Add(this.label11);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.panel6.Size = new System.Drawing.Size(504, 237);
            this.panel6.TabIndex = 3;
            // 
            // ultraListView_DynamicPolicies
            // 
            this.ultraListView_DynamicPolicies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraListView_DynamicPolicies.ItemSettings.AllowEdit = Infragistics.Win.DefaultableBoolean.False;
            appearance2.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.AuditSQLServer_16;
            this.ultraListView_DynamicPolicies.ItemSettings.Appearance = appearance2;
            this.ultraListView_DynamicPolicies.ItemSettings.DefaultImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.AuditSQLServer_16;
            this.ultraListView_DynamicPolicies.Location = new System.Drawing.Point(0, 26);
            this.ultraListView_DynamicPolicies.MainColumn.Text = "Automatic Policy Membership";
            this.ultraListView_DynamicPolicies.MainColumn.VisiblePositionInDetailsView = 0;
            this.ultraListView_DynamicPolicies.MainColumn.Width = 400;
            this.ultraListView_DynamicPolicies.Name = "ultraListView_DynamicPolicies";
            this.ultraListView_DynamicPolicies.Size = new System.Drawing.Size(504, 211);
            this.ultraListView_DynamicPolicies.TabIndex = 4;
            this.ultraListView_DynamicPolicies.Text = "ultraListView1";
            this.ultraListView_DynamicPolicies.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.Details;
            // 
            // label11
            // 
            this.label11.Dock = System.Windows.Forms.DockStyle.Top;
            this.label11.Location = new System.Drawing.Point(0, 10);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(504, 16);
            this.label11.TabIndex = 5;
            this.label11.Text = "This registered SQL Server may be added to one or more of the following policies:" +
                "";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.ultraListView_Policies);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(504, 185);
            this.panel4.TabIndex = 3;
            // 
            // ultraListView_Policies
            // 
            this.ultraListView_Policies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraListView_Policies.ItemSettings.AllowEdit = Infragistics.Win.DefaultableBoolean.False;
            appearance1.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.AuditSQLServer_16;
            this.ultraListView_Policies.ItemSettings.Appearance = appearance1;
            this.ultraListView_Policies.ItemSettings.DefaultImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.AuditSQLServer_16;
            this.ultraListView_Policies.Location = new System.Drawing.Point(0, 23);
            this.ultraListView_Policies.MainColumn.Text = "User-defined Policy Membership";
            this.ultraListView_Policies.MainColumn.VisiblePositionInDetailsView = 0;
            this.ultraListView_Policies.MainColumn.Width = 400;
            this.ultraListView_Policies.Name = "ultraListView_Policies";
            this.ultraListView_Policies.Size = new System.Drawing.Size(504, 162);
            this.ultraListView_Policies.TabIndex = 4;
            this.ultraListView_Policies.Text = "ultraListView1";
            this.ultraListView_Policies.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.Details;
            this.ultraListView_Policies.ViewSettingsDetails.CheckBoxStyle = Infragistics.Win.UltraWinListView.CheckBoxStyle.CheckBox;
            this.ultraListView_Policies.ItemCheckStateChanged += new Infragistics.Win.UltraWinListView.ItemCheckStateChangedEventHandler(this.ultraListView_Policies_ItemCheckStateChanged);
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(504, 23);
            this.label5.TabIndex = 5;
            this.label5.Text = "Select which policies should include this SQL Server.";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(3);
            this.panel1.Size = new System.Drawing.Size(504, 39);
            this.panel1.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Navy;
            this.label3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label3.Location = new System.Drawing.Point(3, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(498, 1);
            this.label3.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label4.Location = new System.Drawing.Point(3, 3);
            this.label4.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(498, 33);
            this.label4.TabIndex = 0;
            this.label4.Text = "This window allows you to modify the policies that contain this SQL Server Instan" +
                "ce.";
            // 
            // _cntxtmn_FilterList
            // 
            this._cntxtmn_FilterList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._cntxtmi_FilterNew,
            this._cntxtmi_FilterDelete,
            this._cntxtmi_FilterProperties});
            this._cntxtmn_FilterList.Name = "_cntxtmn_FilterList";
            this._cntxtmn_FilterList.Size = new System.Drawing.Size(133, 70);
            // 
            // _cntxtmi_FilterNew
            // 
            this._cntxtmi_FilterNew.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.Filter2HS;
            this._cntxtmi_FilterNew.Name = "_cntxtmi_FilterNew";
            this._cntxtmi_FilterNew.Size = new System.Drawing.Size(132, 22);
            this._cntxtmi_FilterNew.Text = "New Filter";
            this._cntxtmi_FilterNew.Click += new System.EventHandler(this._cntxtmi_FilterNew_Click);
            // 
            // _cntxtmi_FilterDelete
            // 
            this._cntxtmi_FilterDelete.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.delete2;
            this._cntxtmi_FilterDelete.Name = "_cntxtmi_FilterDelete";
            this._cntxtmi_FilterDelete.Size = new System.Drawing.Size(132, 22);
            this._cntxtmi_FilterDelete.Text = "Delete Filter";
            this._cntxtmi_FilterDelete.Click += new System.EventHandler(this._cntxtmi_FilterDelete_Click);
            // 
            // _cntxtmi_FilterProperties
            // 
            this._cntxtmi_FilterProperties.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.propertiesORoptions;
            this._cntxtmi_FilterProperties.Name = "_cntxtmi_FilterProperties";
            this._cntxtmi_FilterProperties.Size = new System.Drawing.Size(132, 22);
            this._cntxtmi_FilterProperties.Text = "Properties";
            this._cntxtmi_FilterProperties.Click += new System.EventHandler(this._cntxtmi_FilterProperties_Click);
            // 
            // _btn_Help
            // 
            this._btn_Help.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btn_Help.Location = new System.Drawing.Point(418, 9);
            this._btn_Help.Name = "_btn_Help";
            this._btn_Help.Size = new System.Drawing.Size(75, 23);
            this._btn_Help.TabIndex = 3;
            this._btn_Help.Text = "&Help";
            this._btn_Help.Click += new System.EventHandler(this._btn_Help_Click);
            // 
            // ultraTabControl_ServerProperties
            // 
            appearance6.BackColor = System.Drawing.Color.White;
            this.ultraTabControl_ServerProperties.ActiveTabAppearance = appearance6;
            this.ultraTabControl_ServerProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.BackColor2 = System.Drawing.Color.Transparent;
            this.ultraTabControl_ServerProperties.Appearance = appearance4;
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.BackColor2 = System.Drawing.Color.Transparent;
            this.ultraTabControl_ServerProperties.ClientAreaAppearance = appearance3;
            this.ultraTabControl_ServerProperties.Controls.Add(this.ultraTabSharedControlsPage1);
            this.ultraTabControl_ServerProperties.Controls.Add(this.ultraTabPageControl_General);
            this.ultraTabControl_ServerProperties.Controls.Add(this.ultraTabPageControl_Credentials);
            this.ultraTabControl_ServerProperties.Controls.Add(this.ultraTabPageControl_Filters);
            this.ultraTabControl_ServerProperties.Controls.Add(this.ultraTabPageControl_Schedule);
            this.ultraTabControl_ServerProperties.Controls.Add(this.ultraTabPageControl_Policies);
            this.ultraTabControl_ServerProperties.Controls.Add(this.ultraTabPageControl_Email);
            this.ultraTabControl_ServerProperties.Controls.Add(this.tpcAuditFolders);
            this.ultraTabControl_ServerProperties.Location = new System.Drawing.Point(3, 7);
            this.ultraTabControl_ServerProperties.Name = "ultraTabControl_ServerProperties";
            this.ultraTabControl_ServerProperties.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.ultraTabControl_ServerProperties.Size = new System.Drawing.Size(508, 487);
            this.ultraTabControl_ServerProperties.TabIndex = 1;
            ultraTab1.Key = "General";
            ultraTab1.TabPage = this.ultraTabPageControl_General;
            ultraTab1.Text = "General";
            ultraTab2.Key = "Credentials";
            ultraTab2.TabPage = this.ultraTabPageControl_Credentials;
            ultraTab2.Text = "Credentials";
            ultraTab7.TabPage = this.tpcAuditFolders;
            ultraTab7.Text = "Audit Folders";
            ultraTab3.Key = "Filters";
            ultraTab3.TabPage = this.ultraTabPageControl_Filters;
            ultraTab3.Text = "Filters";
            ultraTab4.Key = "Schedule";
            ultraTab4.TabPage = this.ultraTabPageControl_Schedule;
            ultraTab4.Text = "Schedule";
            ultraTab6.Key = "Email";
            ultraTab6.TabPage = this.ultraTabPageControl_Email;
            ultraTab6.Text = "Email";
            ultraTab5.Key = "Policies";
            ultraTab5.TabPage = this.ultraTabPageControl_Policies;
            ultraTab5.Text = "Policies";
            this.ultraTabControl_ServerProperties.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2,
            ultraTab7,
            ultraTab3,
            ultraTab4,
            ultraTab6,
            ultraTab5});
            this.ultraTabControl_ServerProperties.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.ultraTabControl1_SelectedTabChanged);
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(504, 461);
            // 
            // ultraButton_OK
            // 
            this.ultraButton_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraButton_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ultraButton_OK.Location = new System.Drawing.Point(238, 9);
            this.ultraButton_OK.Name = "ultraButton_OK";
            this.ultraButton_OK.Size = new System.Drawing.Size(75, 23);
            this.ultraButton_OK.TabIndex = 4;
            this.ultraButton_OK.Text = "&OK";
            this.ultraButton_OK.Click += new System.EventHandler(this._btn_OK_Click);
            // 
            // ultraButton_Cancel
            // 
            this.ultraButton_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraButton_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ultraButton_Cancel.Location = new System.Drawing.Point(328, 9);
            this.ultraButton_Cancel.Name = "ultraButton_Cancel";
            this.ultraButton_Cancel.Size = new System.Drawing.Size(75, 23);
            this.ultraButton_Cancel.TabIndex = 5;
            this.ultraButton_Cancel.Text = "&Cancel";
            // 
            // addEditFoldersControl
            // 
            this.addEditFoldersControl.ForeColor = System.Drawing.Color.Navy;
            this.addEditFoldersControl.Location = new System.Drawing.Point(3, 37);
            this.addEditFoldersControl.Name = "addEditFoldersControl";
            this.addEditFoldersControl.Size = new System.Drawing.Size(498, 388);
            this.addEditFoldersControl.TabIndex = 1;
            this.addEditFoldersControl.Text = "This window allows you to view and modify audit folders.  You can add, edit or re" +
                "move folders.";
            // 
            // Form_SqlServerProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(514, 593);
            this.Description = "Manage Properties of Audit SQL Server Instance";
            this.Name = "Form_SqlServerProperties";
            this.Picture = global::Idera.SQLsecure.UI.Console.Properties.Resources.configure_audit_settings_49;
            this.Text = "Audited SQL Server Properties";
            this.Load += new System.EventHandler(this.Form_SqlServerProperties_Load);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.Form_SqlServerProperties_HelpRequested);
            this.Controls.SetChildIndex(this._bf_HeaderPanel, 0);
            this.Controls.SetChildIndex(this._bf_MainPanel, 0);
            this.Controls.SetChildIndex(this._bfd_ButtonPanel, 0);
            this._bfd_ButtonPanel.ResumeLayout(false);
            this._bf_MainPanel.ResumeLayout(false);
            this.ultraTabPageControl_General.ResumeLayout(false);
            this._grpbx_NameVersion.ResumeLayout(false);
            this._grpbx_NameVersion.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this._grpbx_Snapshot.ResumeLayout(false);
            this._grpbx_Snapshot.PerformLayout();
            this.ultraTabPageControl_Credentials.ResumeLayout(false);
            this._grpbx_SQLServerCredentials.ResumeLayout(false);
            this._grpbx_SQLServerCredentials.PerformLayout();
            this._grpbx_WindowsGMCredentials.ResumeLayout(false);
            this._grpbx_WindowsGMCredentials.PerformLayout();
            this._pnl_CredentialsIntro.ResumeLayout(false);
            this.tpcAuditFolders.ResumeLayout(false);
            this.ultraTabPageControl_Filters.ResumeLayout(false);
            this._spltcntnr.Panel1.ResumeLayout(false);
            this._spltcntnr.Panel2.ResumeLayout(false);
            this._spltcntnr.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraListView_Filters)).EndInit();
            this._hdrstrip.ResumeLayout(false);
            this._hdrstrip.PerformLayout();
            this._pnl_FilterDescriptionSep.ResumeLayout(false);
            this._pnl_FilterDescriptionSep.PerformLayout();
            this._pnl_FilterIntro.ResumeLayout(false);
            this.ultraTabPageControl_Schedule.ResumeLayout(false);
            this.ultraTabPageControl_Schedule.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_AgentStatus)).EndInit();
            this._pnl_ScheduleIntro.ResumeLayout(false);
            this._grpbx_Schedule.ResumeLayout(false);
            this._grpbx_Schedule.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_KeepSnapshotDays)).EndInit();
            this.ultraTabPageControl_Email.ResumeLayout(false);
            this.ultraTabPageControl_Email.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ultraTabPageControl_Policies.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraListView_DynamicPolicies)).EndInit();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraListView_Policies)).EndInit();
            this.panel1.ResumeLayout(false);
            this._cntxtmn_FilterList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl_ServerProperties)).EndInit();
            this.ultraTabControl_ServerProperties.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton _btn_Help;
        private System.Windows.Forms.SplitContainer _spltcntnr;
        private System.Windows.Forms.RichTextBox _rtbx_FilterDetails;
        private Idera.SQLsecure.UI.Console.Controls.GradientPanel _pnl_FilterDescriptionSep;
        private System.Windows.Forms.Label _lbl_FilterDescription;
        private System.Windows.Forms.Label _lbl_FilterIntroText;
        private System.Windows.Forms.Panel _pnl_FilterIntro;
        private System.Windows.Forms.Panel _pnl_CredentialsIntro;
        private System.Windows.Forms.Label _lbl_CredentialsIntroSep;
        private System.Windows.Forms.Label _lbl_CredentialsIntroText;
        private System.Windows.Forms.Label _lbl_WindowsOS;
        private System.Windows.Forms.Label _lbl_WindowsOSVal;
        private System.Windows.Forms.GroupBox _grpbx_Snapshot;
        private System.Windows.Forms.Label _lbl_LastSuccessfulSnapshotAtVal;
        private System.Windows.Forms.Label _lbl_CurrentSnapshotStatusVal;
        private System.Windows.Forms.Label _lbl_CurrentSnapshotTimeVal;
        private System.Windows.Forms.Label _lbl_LastSuccessfulSnapshotAt;
        private System.Windows.Forms.Label _lbl_CurrentSnapshotStatus;
        private System.Windows.Forms.Label _lbl_CurrentSnapshotTime;
        private System.Windows.Forms.ContextMenuStrip _cntxtmn_FilterList;
        private System.Windows.Forms.ToolStripMenuItem _cntxtmi_FilterNew;
        private System.Windows.Forms.ToolStripMenuItem _cntxtmi_FilterDelete;
        private System.Windows.Forms.ToolStripMenuItem _cntxtmi_FilterProperties;
        private System.Windows.Forms.Label _lbl_DetailsSep;
        private System.Windows.Forms.Panel _pnl_ScheduleIntro;
        private System.Windows.Forms.Label _lbl_ScheduleIntroSep;
        private System.Windows.Forms.Label _lbl_ScheduleIntroText;
        private System.Windows.Forms.Label label_KeepSnapshotDays;
        private System.Windows.Forms.NumericUpDown numericUpDown_KeepSnapshotDays;
        private System.Windows.Forms.Label label_KeepSnapshot;
        private System.Windows.Forms.GroupBox _grpbx_Schedule;
        private Infragistics.Win.Misc.UltraButton _btn_ChangeSchedule;
        private System.Windows.Forms.CheckBox checkBox_EnableScheduling;
        private Idera.SQLsecure.UI.Console.Controls.HeaderStrip _hdrstrip;
        private System.Windows.Forms.ToolStripButton _tsbtn_FilterProperties;
        private System.Windows.Forms.ToolStripButton _tsbtn_FilterDelete;
        private System.Windows.Forms.ToolStripButton _tsbtn_FilterNew;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label_AgentStatus;
        private System.Windows.Forms.PictureBox pictureBox_AgentStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label _lbl_DcVal;
        private System.Windows.Forms.Label _lbl_Dc;
        private System.Windows.Forms.Label _lbl_OsServerVal;
        private System.Windows.Forms.Label _lbl_OsServer;
        private System.Windows.Forms.GroupBox _grpbx_NameVersion;
        private System.Windows.Forms.Label _lbl_SaVal;
        private System.Windows.Forms.Label _lbl_Sa;
        private System.Windows.Forms.Label _lbl_ReplicationVal;
        private System.Windows.Forms.Label _lbl_Replication;
        private System.Windows.Forms.Label _lbl_SQLServerEditionVal;
        private System.Windows.Forms.Label _lbl_SQLServerVersionVal;
        private System.Windows.Forms.Label _lbl_SQLServerEdition;
        private System.Windows.Forms.Label _lbl_SQLServerVersion;
        private System.Windows.Forms.Label _lbl_ServerVal;
        private System.Windows.Forms.Label _lbl_Server;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl ultraTabControl_ServerProperties;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl_General;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl_Credentials;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl_Filters;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl_Schedule;
        private Infragistics.Win.UltraWinListView.UltraListView ultraListView_Filters;
        private System.Windows.Forms.Label label_Schedule;
        private Infragistics.Win.Misc.UltraButton ultraButton_Cancel;
        private Infragistics.Win.Misc.UltraButton ultraButton_OK;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl_Policies;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl_Email;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton radioButtonSendEmailFindingHighMedium;
        private System.Windows.Forms.RadioButton radioButtonSendEmailFindingAny;
        private System.Windows.Forms.RadioButton radioButtonSendEmailFindingHigh;
        private System.Windows.Forms.TextBox textBox_Recipients;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox checkBoxEmailFindings;
        private System.Windows.Forms.RadioButton radioButtonAlways;
        private System.Windows.Forms.CheckBox checkBoxEmailForCollectionStatus;
        private System.Windows.Forms.RadioButton radioButton_SendEmailOnError;
        private System.Windows.Forms.RadioButton radioButton_SendEmailWarningOrError;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.GroupBox _grpbx_SQLServerCredentials;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_SQLWindowsUser;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox_SQLWindowsPassword;
        private System.Windows.Forms.RadioButton radioButton_WindowsAuth;
        private System.Windows.Forms.RadioButton radioButton_SQLServerAuth;
        private System.Windows.Forms.Label _lbl_SqlLogin;
        private System.Windows.Forms.TextBox textbox_SqlLogin;
        private System.Windows.Forms.Label _lbl_SqlLoginPassword;
        private System.Windows.Forms.TextBox textbox_SqlLoginPassword;
        private System.Windows.Forms.GroupBox _grpbx_WindowsGMCredentials;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox checkBox_UseSameAuth;
        private System.Windows.Forms.Label _lbl_WindowsUser;
        private System.Windows.Forms.TextBox textbox_WindowsUser;
        private System.Windows.Forms.Label _lbl_WindowsPassword;
        private System.Windows.Forms.TextBox textbox_WindowsPassword;
        private System.Windows.Forms.Label label12;
        private Infragistics.Win.UltraWinListView.UltraListView ultraListView_Policies;
        private System.Windows.Forms.Label label5;
        private Infragistics.Win.UltraWinListView.UltraListView ultraListView_DynamicPolicies;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tpcAuditFolders;
        private System.Windows.Forms.Label label15;
        private Idera.SQLsecure.UI.Console.Controls.AddEditFolders addEditFoldersControl;
    }
}