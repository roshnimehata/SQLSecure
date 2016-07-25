namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_WizardRegisterSQLServer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_WizardRegisterSQLServer));
            Infragistics.Win.UltraWinListView.UltraListViewSubItemColumn ultraListViewSubItemColumn1 = new Infragistics.Win.UltraWinListView.UltraListViewSubItemColumn("Description");
            this._wizard = new Divelements.WizardFramework.Wizard();
            this._PageTags = new Divelements.WizardFramework.WizardPage();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btAdd = new System.Windows.Forms.Button();
            this.ulTags = new Infragistics.Win.UltraWinListView.UltraListView();
            this._page_DefineFilters = new Divelements.WizardFramework.WizardPage();
            this._page_JobSchedule = new Divelements.WizardFramework.WizardPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label_AgentStatus = new System.Windows.Forms.Label();
            this.pictureBox_AgentStatus = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._txtbx_ScheduleDescription = new System.Windows.Forms.TextBox();
            this.checkBox_EnableScheduling = new System.Windows.Forms.CheckBox();
            this._btn_ChangeSchedule = new System.Windows.Forms.Button();
            this.label_KeepSnapshotDays = new System.Windows.Forms.Label();
            this.numericUpDown_KeepSnapshotDays = new System.Windows.Forms.NumericUpDown();
            this.label_KeepSnapshot = new System.Windows.Forms.Label();
            this._page_NotificationOptions = new Divelements.WizardFramework.WizardPage();
            this.label12 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioButtonSendEmailFindingHighMedium = new System.Windows.Forms.RadioButton();
            this.radioButtonSendEmailFindingAny = new System.Windows.Forms.RadioButton();
            this.radioButtonSendEmailFindingHigh = new System.Windows.Forms.RadioButton();
            this.textBox_Recipient = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.checkBoxEmailFindings = new System.Windows.Forms.CheckBox();
            this.radioButtonAlways = new System.Windows.Forms.RadioButton();
            this.checkBoxEmailForCollectionStatus = new System.Windows.Forms.CheckBox();
            this.radioButton_SendEmailOnError = new System.Windows.Forms.RadioButton();
            this.radioButton_SendEmailWarningOrError = new System.Windows.Forms.RadioButton();
            this._page_ConfigureSMTPEmail = new Divelements.WizardFramework.WizardPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button_Test = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this._page_Policies = new Divelements.WizardFramework.WizardPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.ultraListView_DynamicPolicies = new Infragistics.Win.UltraWinListView.UltraListView();
            this.label11 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.ultraListView_Policies = new Infragistics.Win.UltraWinListView.UltraListView();
            this.label3 = new System.Windows.Forms.Label();
            this._page_CollectData = new Divelements.WizardFramework.WizardPage();
            this.label13 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.checkBox_CollectData = new System.Windows.Forms.CheckBox();
            this._page_Finish = new Divelements.WizardFramework.FinishPage();
            this._rtb_Finish = new System.Windows.Forms.RichTextBox();
            this._page_FilePermissionFolders = new Divelements.WizardFramework.WizardPage();
            this._page_Credentials = new Divelements.WizardFramework.WizardPage();
            this._grpbx_SQLServerCredentials = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_SQLWindowsUser = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_SQLWindowsPassword = new System.Windows.Forms.TextBox();
            this.radioButton_WindowsAuth = new System.Windows.Forms.RadioButton();
            this.radioButton_SQLServerAuth = new System.Windows.Forms.RadioButton();
            this._lbl_SqlLogin = new System.Windows.Forms.Label();
            this.textbox_SqlLogin = new System.Windows.Forms.TextBox();
            this._lbl_SqlLoginPassword = new System.Windows.Forms.Label();
            this.textbox_SqlLoginPassword = new System.Windows.Forms.TextBox();
            this._grpbx_WindowsGMCredentials = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox_UseSameAuth = new System.Windows.Forms.CheckBox();
            this._lbl_WindowsUser = new System.Windows.Forms.Label();
            this.textbox_WindowsUser = new System.Windows.Forms.TextBox();
            this._lbl_WindowsPassword = new System.Windows.Forms.Label();
            this.textbox_WindowsPassword = new System.Windows.Forms.TextBox();
            this._page_Servers = new Divelements.WizardFramework.WizardPage();
            this._btn_BrowseServers = new System.Windows.Forms.Button();
            this._txtbx_Server = new System.Windows.Forms.TextBox();
            this._lbl_Server = new System.Windows.Forms.Label();
            this._page_Introduction = new Divelements.WizardFramework.IntroductionPage();
            this._rtb_Introduction = new System.Windows.Forms.RichTextBox();
            this.filterSelection1 = new Idera.SQLsecure.UI.Console.Controls.FilterSelection();
            this.controlSMTPEmailConfig1 = new Idera.SQLsecure.UI.Console.Controls.controlSMTPEmailConfig();
            this.addEditFoldersControl = new Idera.SQLsecure.UI.Console.Controls.AddEditFolders();
            this._wizard.SuspendLayout();
            this._PageTags.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ulTags)).BeginInit();
            this._page_DefineFilters.SuspendLayout();
            this._page_JobSchedule.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_AgentStatus)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_KeepSnapshotDays)).BeginInit();
            this._page_NotificationOptions.SuspendLayout();
            this.panel1.SuspendLayout();
            this._page_ConfigureSMTPEmail.SuspendLayout();
            this.panel2.SuspendLayout();
            this._page_Policies.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraListView_DynamicPolicies)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraListView_Policies)).BeginInit();
            this._page_CollectData.SuspendLayout();
            this._page_Finish.SuspendLayout();
            this._page_FilePermissionFolders.SuspendLayout();
            this._page_Credentials.SuspendLayout();
            this._grpbx_SQLServerCredentials.SuspendLayout();
            this._grpbx_WindowsGMCredentials.SuspendLayout();
            this._page_Servers.SuspendLayout();
            this._page_Introduction.SuspendLayout();
            this.SuspendLayout();
            // 
            // _wizard
            // 
            this._wizard.BannerImage = ((System.Drawing.Image)(resources.GetObject("_wizard.BannerImage")));
            this._wizard.Controls.Add(this._PageTags);
            this._wizard.Controls.Add(this._page_JobSchedule);
            this._wizard.Controls.Add(this._page_FilePermissionFolders);
            this._wizard.Controls.Add(this._page_CollectData);
            this._wizard.Controls.Add(this._page_DefineFilters);
            this._wizard.Controls.Add(this._page_Credentials);
            this._wizard.Controls.Add(this._page_Policies);
            this._wizard.Controls.Add(this._page_ConfigureSMTPEmail);
            this._wizard.Controls.Add(this._page_Servers);
            this._wizard.Controls.Add(this._page_Finish);
            this._wizard.Controls.Add(this._page_Introduction);
            this._wizard.Controls.Add(this._page_NotificationOptions);
            this._wizard.HelpVisible = true;
            this._wizard.Location = new System.Drawing.Point(0, 0);
            this._wizard.MarginImage = ((System.Drawing.Image)(resources.GetObject("_wizard.MarginImage")));
            this._wizard.Name = "_wizard";
            this._wizard.SelectedPage = this._PageTags;
            this._wizard.Size = new System.Drawing.Size(514, 536);
            this._wizard.TabIndex = 0;
            this._wizard.HelpRequested += new System.Windows.Forms.HelpEventHandler(this._wizard_HelpRequested);
            // 
            // _PageTags
            // 
            this._PageTags.Controls.Add(this.button3);
            this._PageTags.Controls.Add(this.button2);
            this._PageTags.Controls.Add(this.btAdd);
            this._PageTags.Controls.Add(this.ulTags);
            this._PageTags.Description = "Select tag(s) to add new server to.";
            this._PageTags.Location = new System.Drawing.Point(19, 73);
            this._PageTags.Name = "_PageTags";
            this._PageTags.NextPage = this._page_DefineFilters;
            this._PageTags.PreviousPage = this._page_FilePermissionFolders;
            this._PageTags.Size = new System.Drawing.Size(476, 403);
            this._PageTags.TabIndex = 1012;
            this._PageTags.Text = "Add to Server Group Tag";
            this._PageTags.BeforeDisplay += new System.EventHandler(this._PageTags_BeforeDisplay);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(197, 377);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "Remove";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(96, 377);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Edit";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btAdd
            // 
            this.btAdd.Location = new System.Drawing.Point(0, 377);
            this.btAdd.Name = "btAdd";
            this.btAdd.Size = new System.Drawing.Size(75, 23);
            this.btAdd.TabIndex = 6;
            this.btAdd.Text = "Add";
            this.btAdd.UseVisualStyleBackColor = true;
            this.btAdd.Click += new System.EventHandler(this.btAdd_Click);
            // 
            // ulTags
            // 
            this.ulTags.Dock = System.Windows.Forms.DockStyle.Top;
            this.ulTags.ItemSettings.AllowEdit = Infragistics.Win.DefaultableBoolean.False;
            this.ulTags.ItemSettings.DefaultImage = ((System.Drawing.Image)(resources.GetObject("ulTags.ItemSettings.DefaultImage")));
            this.ulTags.ItemSettings.SelectionType = Infragistics.Win.UltraWinListView.SelectionType.Single;
            this.ulTags.Location = new System.Drawing.Point(0, 0);
            this.ulTags.MainColumn.Text = "Server Group Tag";
            this.ulTags.MainColumn.VisiblePositionInDetailsView = 0;
            this.ulTags.MainColumn.Width = 200;
            this.ulTags.Name = "ulTags";
            this.ulTags.Size = new System.Drawing.Size(476, 345);
            ultraListViewSubItemColumn1.Key = "Description";
            ultraListViewSubItemColumn1.Width = 250;
            this.ulTags.SubItemColumns.AddRange(new Infragistics.Win.UltraWinListView.UltraListViewSubItemColumn[] {
            ultraListViewSubItemColumn1});
            this.ulTags.TabIndex = 4;
            this.ulTags.Text = "ultraListView1";
            this.ulTags.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.Details;
            this.ulTags.ViewSettingsDetails.CheckBoxStyle = Infragistics.Win.UltraWinListView.CheckBoxStyle.CheckBox;
            // 
            // _page_DefineFilters
            // 
            this._page_DefineFilters.Controls.Add(this.filterSelection1);
            this._page_DefineFilters.Description = "";
            this._page_DefineFilters.DescriptionColor = System.Drawing.Color.Navy;
            this._page_DefineFilters.Location = new System.Drawing.Point(19, 73);
            this._page_DefineFilters.Name = "_page_DefineFilters";
            this._page_DefineFilters.NextPage = this._page_JobSchedule;
            this._page_DefineFilters.PreviousPage = this._PageTags;
            this._page_DefineFilters.Size = new System.Drawing.Size(476, 403);
            this._page_DefineFilters.TabIndex = 0;
            this._page_DefineFilters.Text = "Specify which SQL Server objects to audit";
            this._page_DefineFilters.TextColor = System.Drawing.Color.Navy;
            this._page_DefineFilters.BeforeMoveNext += new Divelements.WizardFramework.WizardPageEventHandler(this._page_DefineFilters_BeforeMoveNext);
            this._page_DefineFilters.BeforeDisplay += new System.EventHandler(this._page_DefineFilters_BeforeDisplay);
            // 
            // _page_JobSchedule
            // 
            this._page_JobSchedule.Controls.Add(this.groupBox2);
            this._page_JobSchedule.Controls.Add(this.groupBox1);
            this._page_JobSchedule.Controls.Add(this.label_KeepSnapshotDays);
            this._page_JobSchedule.Controls.Add(this.numericUpDown_KeepSnapshotDays);
            this._page_JobSchedule.Controls.Add(this.label_KeepSnapshot);
            this._page_JobSchedule.Description = "Select when audit data (snapshots) should be collected.";
            this._page_JobSchedule.DescriptionColor = System.Drawing.Color.Navy;
            this._page_JobSchedule.Location = new System.Drawing.Point(19, 73);
            this._page_JobSchedule.Name = "_page_JobSchedule";
            this._page_JobSchedule.NextPage = this._page_NotificationOptions;
            this._page_JobSchedule.PreviousPage = this._page_DefineFilters;
            this._page_JobSchedule.Size = new System.Drawing.Size(476, 403);
            this._page_JobSchedule.TabIndex = 0;
            this._page_JobSchedule.Text = "Schedule Snapshots";
            this._page_JobSchedule.TextColor = System.Drawing.Color.Navy;
            this._page_JobSchedule.BeforeMoveNext += new Divelements.WizardFramework.WizardPageEventHandler(this._page_JobSchedule_BeforeMoveNext);
            this._page_JobSchedule.BeforeMoveBack += new Divelements.WizardFramework.WizardPageEventHandler(this._page_JobSchedule_BeforeMoveBack);
            this._page_JobSchedule.BeforeDisplay += new System.EventHandler(this._page_JobSchedule_BeforeDisplay);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label_AgentStatus);
            this.groupBox2.Controls.Add(this.pictureBox_AgentStatus);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(21, 218);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(435, 113);
            this.groupBox2.TabIndex = 1021;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "SQL Server Agent Status";
            // 
            // label_AgentStatus
            // 
            this.label_AgentStatus.AutoSize = true;
            this.label_AgentStatus.BackColor = System.Drawing.Color.Transparent;
            this.label_AgentStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_AgentStatus.Location = new System.Drawing.Point(10, 81);
            this.label_AgentStatus.Name = "label_AgentStatus";
            this.label_AgentStatus.Size = new System.Drawing.Size(48, 13);
            this.label_AgentStatus.TabIndex = 14;
            this.label_AgentStatus.Text = "Started";
            // 
            // pictureBox_AgentStatus
            // 
            this.pictureBox_AgentStatus.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_AgentStatus.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_AgentStatus.Image")));
            this.pictureBox_AgentStatus.Location = new System.Drawing.Point(10, 22);
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
            this.label1.Location = new System.Drawing.Point(64, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(364, 51);
            this.label1.TabIndex = 12;
            this.label1.Text = "SQLsecure uses the SQL Server Agent for data collection and grooming. This agent " +
    "is located on the SQL server hosting the Repository database.";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._txtbx_ScheduleDescription);
            this.groupBox1.Controls.Add(this.checkBox_EnableScheduling);
            this.groupBox1.Controls.Add(this._btn_ChangeSchedule);
            this.groupBox1.Location = new System.Drawing.Point(21, 44);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(435, 144);
            this.groupBox1.TabIndex = 1020;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Collection Schedule";
            // 
            // _txtbx_ScheduleDescription
            // 
            this._txtbx_ScheduleDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._txtbx_ScheduleDescription.BackColor = System.Drawing.Color.GhostWhite;
            this._txtbx_ScheduleDescription.ForeColor = System.Drawing.Color.SlateGray;
            this._txtbx_ScheduleDescription.Location = new System.Drawing.Point(10, 47);
            this._txtbx_ScheduleDescription.Multiline = true;
            this._txtbx_ScheduleDescription.Name = "_txtbx_ScheduleDescription";
            this._txtbx_ScheduleDescription.ReadOnly = true;
            this._txtbx_ScheduleDescription.Size = new System.Drawing.Size(339, 76);
            this._txtbx_ScheduleDescription.TabIndex = 1019;
            this._txtbx_ScheduleDescription.TabStop = false;
            // 
            // checkBox_EnableScheduling
            // 
            this.checkBox_EnableScheduling.AutoSize = true;
            this.checkBox_EnableScheduling.Location = new System.Drawing.Point(10, 22);
            this.checkBox_EnableScheduling.Name = "checkBox_EnableScheduling";
            this.checkBox_EnableScheduling.Size = new System.Drawing.Size(115, 17);
            this.checkBox_EnableScheduling.TabIndex = 3;
            this.checkBox_EnableScheduling.Text = "&Enable Scheduling";
            this.checkBox_EnableScheduling.UseVisualStyleBackColor = true;
            this.checkBox_EnableScheduling.CheckedChanged += new System.EventHandler(this.checkBox_EnableScheduling_CheckedChanged);
            // 
            // _btn_ChangeSchedule
            // 
            this._btn_ChangeSchedule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btn_ChangeSchedule.Location = new System.Drawing.Point(353, 100);
            this._btn_ChangeSchedule.Name = "_btn_ChangeSchedule";
            this._btn_ChangeSchedule.Size = new System.Drawing.Size(76, 23);
            this._btn_ChangeSchedule.TabIndex = 4;
            this._btn_ChangeSchedule.Text = "C&hange...";
            this._btn_ChangeSchedule.UseVisualStyleBackColor = true;
            this._btn_ChangeSchedule.Click += new System.EventHandler(this._btn_ChangeSchedule_Click);
            // 
            // label_KeepSnapshotDays
            // 
            this.label_KeepSnapshotDays.AutoSize = true;
            this.label_KeepSnapshotDays.Location = new System.Drawing.Point(192, 9);
            this.label_KeepSnapshotDays.Name = "label_KeepSnapshotDays";
            this.label_KeepSnapshotDays.Size = new System.Drawing.Size(282, 13);
            this.label_KeepSnapshotDays.TabIndex = 1018;
            this.label_KeepSnapshotDays.Text = "days before allowing them to be groomed. (1 - 10000 days)";
            // 
            // numericUpDown_KeepSnapshotDays
            // 
            this.numericUpDown_KeepSnapshotDays.Location = new System.Drawing.Point(128, 7);
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
            this.numericUpDown_KeepSnapshotDays.TabIndex = 1;
            this.numericUpDown_KeepSnapshotDays.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label_KeepSnapshot
            // 
            this.label_KeepSnapshot.AutoSize = true;
            this.label_KeepSnapshot.Location = new System.Drawing.Point(21, 9);
            this.label_KeepSnapshot.Name = "label_KeepSnapshot";
            this.label_KeepSnapshot.Size = new System.Drawing.Size(98, 13);
            this.label_KeepSnapshot.TabIndex = 1016;
            this.label_KeepSnapshot.Text = "&Keep snapshots for";
            // 
            // _page_NotificationOptions
            // 
            this._page_NotificationOptions.Controls.Add(this.label12);
            this._page_NotificationOptions.Controls.Add(this.panel1);
            this._page_NotificationOptions.Controls.Add(this.textBox_Recipient);
            this._page_NotificationOptions.Controls.Add(this.label8);
            this._page_NotificationOptions.Controls.Add(this.label7);
            this._page_NotificationOptions.Controls.Add(this.label6);
            this._page_NotificationOptions.Controls.Add(this.checkBoxEmailFindings);
            this._page_NotificationOptions.Controls.Add(this.radioButtonAlways);
            this._page_NotificationOptions.Controls.Add(this.checkBoxEmailForCollectionStatus);
            this._page_NotificationOptions.Controls.Add(this.radioButton_SendEmailOnError);
            this._page_NotificationOptions.Controls.Add(this.radioButton_SendEmailWarningOrError);
            this._page_NotificationOptions.Description = "Select whether email notifications should be sent after each snapshot.";
            this._page_NotificationOptions.DescriptionColor = System.Drawing.Color.Navy;
            this._page_NotificationOptions.Location = new System.Drawing.Point(19, 73);
            this._page_NotificationOptions.Name = "_page_NotificationOptions";
            this._page_NotificationOptions.NextPage = this._page_ConfigureSMTPEmail;
            this._page_NotificationOptions.PreviousPage = this._page_JobSchedule;
            this._page_NotificationOptions.Size = new System.Drawing.Size(476, 403);
            this._page_NotificationOptions.TabIndex = 1007;
            this._page_NotificationOptions.Text = "Configure Email Notification";
            this._page_NotificationOptions.TextColor = System.Drawing.Color.Navy;
            this._page_NotificationOptions.BeforeMoveNext += new Divelements.WizardFramework.WizardPageEventHandler(this.wizardOptionsPage_BeforeMoveNext);
            this._page_NotificationOptions.BeforeDisplay += new System.EventHandler(this.wizardOptionsPage_BeforeDisplay);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(87, 272);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(379, 13);
            this.label12.TabIndex = 13;
            this.label12.Text = "( specify multiple email recipients by separating each address with a semicolon )" +
    "";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.radioButtonSendEmailFindingHighMedium);
            this.panel1.Controls.Add(this.radioButtonSendEmailFindingAny);
            this.panel1.Controls.Add(this.radioButtonSendEmailFindingHigh);
            this.panel1.Location = new System.Drawing.Point(3, 129);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(431, 93);
            this.panel1.TabIndex = 12;
            // 
            // radioButtonSendEmailFindingHighMedium
            // 
            this.radioButtonSendEmailFindingHighMedium.AutoSize = true;
            this.radioButtonSendEmailFindingHighMedium.Location = new System.Drawing.Point(20, 33);
            this.radioButtonSendEmailFindingHighMedium.Name = "radioButtonSendEmailFindingHighMedium";
            this.radioButtonSendEmailFindingHighMedium.Size = new System.Drawing.Size(154, 17);
            this.radioButtonSendEmailFindingHighMedium.TabIndex = 7;
            this.radioButtonSendEmailFindingHighMedium.Text = "On High and Medium Risks";
            this.radioButtonSendEmailFindingHighMedium.UseVisualStyleBackColor = true;
            // 
            // radioButtonSendEmailFindingAny
            // 
            this.radioButtonSendEmailFindingAny.AutoSize = true;
            this.radioButtonSendEmailFindingAny.Checked = true;
            this.radioButtonSendEmailFindingAny.Location = new System.Drawing.Point(20, 10);
            this.radioButtonSendEmailFindingAny.Name = "radioButtonSendEmailFindingAny";
            this.radioButtonSendEmailFindingAny.Size = new System.Drawing.Size(67, 17);
            this.radioButtonSendEmailFindingAny.TabIndex = 6;
            this.radioButtonSendEmailFindingAny.TabStop = true;
            this.radioButtonSendEmailFindingAny.Text = "Any Risk";
            this.radioButtonSendEmailFindingAny.UseVisualStyleBackColor = true;
            // 
            // radioButtonSendEmailFindingHigh
            // 
            this.radioButtonSendEmailFindingHigh.AutoSize = true;
            this.radioButtonSendEmailFindingHigh.Location = new System.Drawing.Point(20, 56);
            this.radioButtonSendEmailFindingHigh.Name = "radioButtonSendEmailFindingHigh";
            this.radioButtonSendEmailFindingHigh.Size = new System.Drawing.Size(115, 17);
            this.radioButtonSendEmailFindingHigh.TabIndex = 8;
            this.radioButtonSendEmailFindingHigh.Text = "Only on High Risks";
            this.radioButtonSendEmailFindingHigh.UseVisualStyleBackColor = true;
            // 
            // textBox_Recipient
            // 
            this.textBox_Recipient.Location = new System.Drawing.Point(87, 245);
            this.textBox_Recipient.Name = "textBox_Recipient";
            this.textBox_Recipient.Size = new System.Drawing.Size(380, 20);
            this.textBox_Recipient.TabIndex = 9;
            this.textBox_Recipient.TextChanged += new System.EventHandler(this.textBox_Recipient_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(0, 248);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Email Recipient";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Navy;
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label7.Location = new System.Drawing.Point(234, 122);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(233, 3);
            this.label7.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Navy;
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label6.Location = new System.Drawing.Point(234, 11);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(233, 3);
            this.label6.TabIndex = 9;
            // 
            // checkBoxEmailFindings
            // 
            this.checkBoxEmailFindings.AutoSize = true;
            this.checkBoxEmailFindings.Location = new System.Drawing.Point(0, 114);
            this.checkBoxEmailFindings.Name = "checkBoxEmailFindings";
            this.checkBoxEmailFindings.Size = new System.Drawing.Size(233, 17);
            this.checkBoxEmailFindings.TabIndex = 5;
            this.checkBoxEmailFindings.Text = "Send Email Notification for Security Findings";
            this.checkBoxEmailFindings.UseVisualStyleBackColor = true;
            this.checkBoxEmailFindings.CheckedChanged += new System.EventHandler(this.checkBoxEmailFindings_CheckedChanged);
            // 
            // radioButtonAlways
            // 
            this.radioButtonAlways.AutoSize = true;
            this.radioButtonAlways.Checked = true;
            this.radioButtonAlways.Location = new System.Drawing.Point(20, 24);
            this.radioButtonAlways.Name = "radioButtonAlways";
            this.radioButtonAlways.Size = new System.Drawing.Size(58, 17);
            this.radioButtonAlways.TabIndex = 2;
            this.radioButtonAlways.TabStop = true;
            this.radioButtonAlways.Text = "Always";
            this.radioButtonAlways.UseVisualStyleBackColor = true;
            // 
            // checkBoxEmailForCollectionStatus
            // 
            this.checkBoxEmailForCollectionStatus.AutoSize = true;
            this.checkBoxEmailForCollectionStatus.Location = new System.Drawing.Point(0, 3);
            this.checkBoxEmailForCollectionStatus.Name = "checkBoxEmailForCollectionStatus";
            this.checkBoxEmailForCollectionStatus.Size = new System.Drawing.Size(234, 17);
            this.checkBoxEmailForCollectionStatus.TabIndex = 1;
            this.checkBoxEmailForCollectionStatus.Text = "Send Email Notification after Data Collection";
            this.checkBoxEmailForCollectionStatus.UseVisualStyleBackColor = true;
            this.checkBoxEmailForCollectionStatus.CheckedChanged += new System.EventHandler(this.checkBoxEmailForCollectionStatus_CheckedChanged);
            // 
            // radioButton_SendEmailOnError
            // 
            this.radioButton_SendEmailOnError.AutoSize = true;
            this.radioButton_SendEmailOnError.Location = new System.Drawing.Point(20, 70);
            this.radioButton_SendEmailOnError.Name = "radioButton_SendEmailOnError";
            this.radioButton_SendEmailOnError.Size = new System.Drawing.Size(88, 17);
            this.radioButton_SendEmailOnError.TabIndex = 4;
            this.radioButton_SendEmailOnError.Text = "Only On Error";
            this.radioButton_SendEmailOnError.UseVisualStyleBackColor = true;
            // 
            // radioButton_SendEmailWarningOrError
            // 
            this.radioButton_SendEmailWarningOrError.AutoSize = true;
            this.radioButton_SendEmailWarningOrError.Location = new System.Drawing.Point(20, 47);
            this.radioButton_SendEmailWarningOrError.Name = "radioButton_SendEmailWarningOrError";
            this.radioButton_SendEmailWarningOrError.Size = new System.Drawing.Size(128, 17);
            this.radioButton_SendEmailWarningOrError.TabIndex = 3;
            this.radioButton_SendEmailWarningOrError.Text = "On Warning and Error";
            this.radioButton_SendEmailWarningOrError.UseVisualStyleBackColor = true;
            // 
            // _page_ConfigureSMTPEmail
            // 
            this._page_ConfigureSMTPEmail.Controls.Add(this.controlSMTPEmailConfig1);
            this._page_ConfigureSMTPEmail.Controls.Add(this.panel2);
            this._page_ConfigureSMTPEmail.Controls.Add(this.label10);
            this._page_ConfigureSMTPEmail.Description = "Configure SQLsecure\'s SMTP Email Provider";
            this._page_ConfigureSMTPEmail.DescriptionColor = System.Drawing.Color.Navy;
            this._page_ConfigureSMTPEmail.Location = new System.Drawing.Point(19, 73);
            this._page_ConfigureSMTPEmail.Name = "_page_ConfigureSMTPEmail";
            this._page_ConfigureSMTPEmail.NextPage = this._page_Policies;
            this._page_ConfigureSMTPEmail.PreviousPage = this._page_NotificationOptions;
            this._page_ConfigureSMTPEmail.Size = new System.Drawing.Size(476, 403);
            this._page_ConfigureSMTPEmail.TabIndex = 1010;
            this._page_ConfigureSMTPEmail.Text = "Configure SMTP Provider";
            this._page_ConfigureSMTPEmail.TextColor = System.Drawing.Color.Navy;
            this._page_ConfigureSMTPEmail.BeforeMoveNext += new Divelements.WizardFramework.WizardPageEventHandler(this._page_ConfigureSMTPEmail_BeforeMoveNext);
            this._page_ConfigureSMTPEmail.BeforeDisplay += new System.EventHandler(this._page_ConfigureSMTPEmail_BeforeDisplay);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button_Test);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 367);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(476, 36);
            this.panel2.TabIndex = 8;
            // 
            // button_Test
            // 
            this.button_Test.Location = new System.Drawing.Point(3, 6);
            this.button_Test.Name = "button_Test";
            this.button_Test.Size = new System.Drawing.Size(75, 23);
            this.button_Test.TabIndex = 0;
            this.button_Test.Text = "&Test";
            this.button_Test.UseVisualStyleBackColor = true;
            this.button_Test.Click += new System.EventHandler(this.button_Test_Click);
            // 
            // label10
            // 
            this.label10.Dock = System.Windows.Forms.DockStyle.Top;
            this.label10.Location = new System.Drawing.Point(0, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(476, 32);
            this.label10.TabIndex = 6;
            this.label10.Text = "Before Email notifications can be sent an Email provider must be configured for S" +
    "QLsecure.";
            // 
            // _page_Policies
            // 
            this._page_Policies.Controls.Add(this.panel3);
            this._page_Policies.Description = "Select which policies should run security checks for this SQL Server.";
            this._page_Policies.DescriptionColor = System.Drawing.Color.Navy;
            this._page_Policies.Location = new System.Drawing.Point(19, 73);
            this._page_Policies.Name = "_page_Policies";
            this._page_Policies.NextPage = this._page_CollectData;
            this._page_Policies.PreviousPage = this._page_ConfigureSMTPEmail;
            this._page_Policies.Size = new System.Drawing.Size(476, 403);
            this._page_Policies.TabIndex = 1008;
            this._page_Policies.Text = "Add to Policies";
            this._page_Policies.TextColor = System.Drawing.Color.Navy;
            this._page_Policies.BeforeMoveNext += new Divelements.WizardFramework.WizardPageEventHandler(this._page_Policies_BeforeMoveNext);
            this._page_Policies.BeforeMoveBack += new Divelements.WizardFramework.WizardPageEventHandler(this._page_Policies_BeforeMoveBack);
            this._page_Policies.BeforeDisplay += new System.EventHandler(this.wizardPage1_BeforeDisplay);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(476, 403);
            this.panel3.TabIndex = 4;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 185);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(476, 218);
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
            this.panel6.Size = new System.Drawing.Size(476, 218);
            this.panel6.TabIndex = 3;
            // 
            // ultraListView_DynamicPolicies
            // 
            this.ultraListView_DynamicPolicies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraListView_DynamicPolicies.ItemSettings.AllowEdit = Infragistics.Win.DefaultableBoolean.False;
            this.ultraListView_DynamicPolicies.ItemSettings.DefaultImage = ((System.Drawing.Image)(resources.GetObject("ultraListView_DynamicPolicies.ItemSettings.DefaultImage")));
            this.ultraListView_DynamicPolicies.Location = new System.Drawing.Point(0, 26);
            this.ultraListView_DynamicPolicies.MainColumn.Text = "Automatic Policy Membership";
            this.ultraListView_DynamicPolicies.MainColumn.VisiblePositionInDetailsView = 0;
            this.ultraListView_DynamicPolicies.MainColumn.Width = 400;
            this.ultraListView_DynamicPolicies.Name = "ultraListView_DynamicPolicies";
            this.ultraListView_DynamicPolicies.Size = new System.Drawing.Size(476, 192);
            this.ultraListView_DynamicPolicies.TabIndex = 2;
            this.ultraListView_DynamicPolicies.Text = "ultraListView1";
            this.ultraListView_DynamicPolicies.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.Details;
            // 
            // label11
            // 
            this.label11.Dock = System.Windows.Forms.DockStyle.Top;
            this.label11.Location = new System.Drawing.Point(0, 10);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(476, 16);
            this.label11.TabIndex = 3;
            this.label11.Text = "This registered SQL Server may be added to one or more of the following policies:" +
    "";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.ultraListView_Policies);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(476, 185);
            this.panel4.TabIndex = 3;
            // 
            // ultraListView_Policies
            // 
            this.ultraListView_Policies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraListView_Policies.ItemSettings.AllowEdit = Infragistics.Win.DefaultableBoolean.False;
            this.ultraListView_Policies.ItemSettings.DefaultImage = ((System.Drawing.Image)(resources.GetObject("ultraListView_Policies.ItemSettings.DefaultImage")));
            this.ultraListView_Policies.Location = new System.Drawing.Point(0, 23);
            this.ultraListView_Policies.MainColumn.Text = "User-defined Policy Membership";
            this.ultraListView_Policies.MainColumn.VisiblePositionInDetailsView = 0;
            this.ultraListView_Policies.MainColumn.Width = 400;
            this.ultraListView_Policies.Name = "ultraListView_Policies";
            this.ultraListView_Policies.Size = new System.Drawing.Size(476, 162);
            this.ultraListView_Policies.TabIndex = 2;
            this.ultraListView_Policies.Text = "ultraListView1";
            this.ultraListView_Policies.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.Details;
            this.ultraListView_Policies.ViewSettingsDetails.CheckBoxStyle = Infragistics.Win.UltraWinListView.CheckBoxStyle.CheckBox;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(476, 23);
            this.label3.TabIndex = 3;
            this.label3.Text = "Select which policies should include this SQL Server.";
            // 
            // _page_CollectData
            // 
            this._page_CollectData.Controls.Add(this.label13);
            this._page_CollectData.Controls.Add(this.label9);
            this._page_CollectData.Controls.Add(this.checkBox_CollectData);
            this._page_CollectData.Description = "Specify whether you want to collect audit data after registering this SQL Server." +
    "";
            this._page_CollectData.DescriptionColor = System.Drawing.Color.Navy;
            this._page_CollectData.Location = new System.Drawing.Point(19, 73);
            this._page_CollectData.Name = "_page_CollectData";
            this._page_CollectData.NextPage = this._page_Finish;
            this._page_CollectData.PreviousPage = this._page_Policies;
            this._page_CollectData.Size = new System.Drawing.Size(476, 403);
            this._page_CollectData.TabIndex = 1009;
            this._page_CollectData.Text = "Take Snapshot";
            this._page_CollectData.TextColor = System.Drawing.Color.Navy;
            this._page_CollectData.BeforeMoveBack += new Divelements.WizardFramework.WizardPageEventHandler(this._page_CollectData_BeforeMoveBack);
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(0, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(476, 24);
            this.label13.TabIndex = 5;
            this.label13.Text = "Do you want to collect audit data (take a snapshot) after registering this SQL Se" +
    "rver?";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(0, 37);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(442, 56);
            this.label9.TabIndex = 4;
            this.label9.Text = "SQLsecure must collect data from the Registered Server to assess and audit securi" +
    "ty risks and access rights. This data collection can be scheduled or run manuall" +
    "y.";
            // 
            // checkBox_CollectData
            // 
            this.checkBox_CollectData.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.checkBox_CollectData.Location = new System.Drawing.Point(3, 97);
            this.checkBox_CollectData.Name = "checkBox_CollectData";
            this.checkBox_CollectData.Size = new System.Drawing.Size(421, 18);
            this.checkBox_CollectData.TabIndex = 3;
            this.checkBox_CollectData.Text = "Yes, collect data upon completion of the registration process.";
            this.checkBox_CollectData.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.checkBox_CollectData.UseVisualStyleBackColor = true;
            this.checkBox_CollectData.CheckedChanged += new System.EventHandler(this.checkBox_CollectData_CheckedChanged);
            // 
            // _page_Finish
            // 
            this._page_Finish.Controls.Add(this._rtb_Finish);
            this._page_Finish.FinishText = "";
            this._page_Finish.Location = new System.Drawing.Point(177, 66);
            this._page_Finish.Name = "_page_Finish";
            this._page_Finish.PreviousPage = this._page_CollectData;
            this._page_Finish.ProceedText = "Click the Finish button to register this SQL Server.";
            this._page_Finish.Size = new System.Drawing.Size(324, 410);
            this._page_Finish.TabIndex = 1006;
            this._page_Finish.Text = "SQL Server Registration Summary";
            this._page_Finish.BeforeMoveBack += new Divelements.WizardFramework.WizardPageEventHandler(this._page_Finish_BeforeMoveBack);
            this._page_Finish.BeforeDisplay += new System.EventHandler(this._page_Finish_BeforeDisplay);
            // 
            // _rtb_Finish
            // 
            this._rtb_Finish.BackColor = System.Drawing.SystemColors.Window;
            this._rtb_Finish.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._rtb_Finish.Cursor = System.Windows.Forms.Cursors.No;
            this._rtb_Finish.Dock = System.Windows.Forms.DockStyle.Top;
            this._rtb_Finish.ForeColor = System.Drawing.Color.Navy;
            this._rtb_Finish.Location = new System.Drawing.Point(0, 0);
            this._rtb_Finish.Name = "_rtb_Finish";
            this._rtb_Finish.ReadOnly = true;
            this._rtb_Finish.Size = new System.Drawing.Size(324, 385);
            this._rtb_Finish.TabIndex = 1;
            this._rtb_Finish.Text = "";
            // 
            // _page_FilePermissionFolders
            // 
            this._page_FilePermissionFolders.Controls.Add(this.addEditFoldersControl);
            this._page_FilePermissionFolders.Description = "Specify folders to be audited for collecting file system permission information";
            this._page_FilePermissionFolders.Location = new System.Drawing.Point(19, 73);
            this._page_FilePermissionFolders.Name = "_page_FilePermissionFolders";
            this._page_FilePermissionFolders.NextPage = this._PageTags;
            this._page_FilePermissionFolders.PreviousPage = this._page_Credentials;
            this._page_FilePermissionFolders.Size = new System.Drawing.Size(476, 403);
            this._page_FilePermissionFolders.TabIndex = 1011;
            this._page_FilePermissionFolders.Text = "Specify Audit Folders";
            // 
            // _page_Credentials
            // 
            this._page_Credentials.Controls.Add(this._grpbx_SQLServerCredentials);
            this._page_Credentials.Controls.Add(this._grpbx_WindowsGMCredentials);
            this._page_Credentials.Description = "Specify which credentials SQLsecure should use to collect audit data.";
            this._page_Credentials.DescriptionColor = System.Drawing.Color.Navy;
            this._page_Credentials.Location = new System.Drawing.Point(19, 73);
            this._page_Credentials.Name = "_page_Credentials";
            this._page_Credentials.NextPage = this._page_FilePermissionFolders;
            this._page_Credentials.PreviousPage = this._page_Servers;
            this._page_Credentials.Size = new System.Drawing.Size(476, 403);
            this._page_Credentials.TabIndex = 0;
            this._page_Credentials.Text = "Specify Connection Credentials";
            this._page_Credentials.TextColor = System.Drawing.Color.Navy;
            this._page_Credentials.BeforeMoveNext += new Divelements.WizardFramework.WizardPageEventHandler(this._page_Credentials_BeforeMoveNext);
            this._page_Credentials.AfterDisplay += new System.EventHandler(this._page_Credentials_AfterDisplay);
            this._page_Credentials.BeforeDisplay += new System.EventHandler(this._page_Credentials_BeforeDisplay);
            // 
            // _grpbx_SQLServerCredentials
            // 
            this._grpbx_SQLServerCredentials.Controls.Add(this.label4);
            this._grpbx_SQLServerCredentials.Controls.Add(this.textBox_SQLWindowsUser);
            this._grpbx_SQLServerCredentials.Controls.Add(this.label5);
            this._grpbx_SQLServerCredentials.Controls.Add(this.textBox_SQLWindowsPassword);
            this._grpbx_SQLServerCredentials.Controls.Add(this.radioButton_WindowsAuth);
            this._grpbx_SQLServerCredentials.Controls.Add(this.radioButton_SQLServerAuth);
            this._grpbx_SQLServerCredentials.Controls.Add(this._lbl_SqlLogin);
            this._grpbx_SQLServerCredentials.Controls.Add(this.textbox_SqlLogin);
            this._grpbx_SQLServerCredentials.Controls.Add(this._lbl_SqlLoginPassword);
            this._grpbx_SQLServerCredentials.Controls.Add(this.textbox_SqlLoginPassword);
            this._grpbx_SQLServerCredentials.Location = new System.Drawing.Point(3, 14);
            this._grpbx_SQLServerCredentials.Name = "_grpbx_SQLServerCredentials";
            this._grpbx_SQLServerCredentials.Size = new System.Drawing.Size(470, 192);
            this._grpbx_SQLServerCredentials.TabIndex = 0;
            this._grpbx_SQLServerCredentials.TabStop = false;
            this._grpbx_SQLServerCredentials.Text = "SQL Server credentials to connect to audited SQL Server";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(31, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "&Windows User:";
            // 
            // textBox_SQLWindowsUser
            // 
            this.textBox_SQLWindowsUser.Location = new System.Drawing.Point(117, 43);
            this.textBox_SQLWindowsUser.Name = "textBox_SQLWindowsUser";
            this.textBox_SQLWindowsUser.Size = new System.Drawing.Size(334, 20);
            this.textBox_SQLWindowsUser.TabIndex = 2;
            this.textBox_SQLWindowsUser.TextChanged += new System.EventHandler(this.textBox_SQLWindowsUser_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(31, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "&Password:";
            // 
            // textBox_SQLWindowsPassword
            // 
            this.textBox_SQLWindowsPassword.Location = new System.Drawing.Point(117, 69);
            this.textBox_SQLWindowsPassword.Name = "textBox_SQLWindowsPassword";
            this.textBox_SQLWindowsPassword.PasswordChar = '*';
            this.textBox_SQLWindowsPassword.Size = new System.Drawing.Size(334, 20);
            this.textBox_SQLWindowsPassword.TabIndex = 3;
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
            this.radioButton_SQLServerAuth.TabIndex = 4;
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
            this.textbox_SqlLogin.Location = new System.Drawing.Point(117, 134);
            this.textbox_SqlLogin.Name = "textbox_SqlLogin";
            this.textbox_SqlLogin.Size = new System.Drawing.Size(334, 20);
            this.textbox_SqlLogin.TabIndex = 5;
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
            this.textbox_SqlLoginPassword.Location = new System.Drawing.Point(117, 160);
            this.textbox_SqlLoginPassword.Name = "textbox_SqlLoginPassword";
            this.textbox_SqlLoginPassword.PasswordChar = '*';
            this.textbox_SqlLoginPassword.Size = new System.Drawing.Size(334, 20);
            this.textbox_SqlLoginPassword.TabIndex = 6;
            this.textbox_SqlLoginPassword.TextChanged += new System.EventHandler(this._txtbx_SqlLoginPassword_TextChanged);
            // 
            // _grpbx_WindowsGMCredentials
            // 
            this._grpbx_WindowsGMCredentials.Controls.Add(this.label2);
            this._grpbx_WindowsGMCredentials.Controls.Add(this.checkBox_UseSameAuth);
            this._grpbx_WindowsGMCredentials.Controls.Add(this._lbl_WindowsUser);
            this._grpbx_WindowsGMCredentials.Controls.Add(this.textbox_WindowsUser);
            this._grpbx_WindowsGMCredentials.Controls.Add(this._lbl_WindowsPassword);
            this._grpbx_WindowsGMCredentials.Controls.Add(this.textbox_WindowsPassword);
            this._grpbx_WindowsGMCredentials.Location = new System.Drawing.Point(3, 224);
            this._grpbx_WindowsGMCredentials.Name = "_grpbx_WindowsGMCredentials";
            this._grpbx_WindowsGMCredentials.Size = new System.Drawing.Size(470, 173);
            this._grpbx_WindowsGMCredentials.TabIndex = 1;
            this._grpbx_WindowsGMCredentials.TabStop = false;
            this._grpbx_WindowsGMCredentials.Text = "Windows Credentials to gather Operating System and Active Directory objects";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(10, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(454, 66);
            this.label2.TabIndex = 6;
            this.label2.Text = resources.GetString("label2.Text");
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
            this.textbox_WindowsUser.Location = new System.Drawing.Point(117, 114);
            this.textbox_WindowsUser.Name = "textbox_WindowsUser";
            this.textbox_WindowsUser.Size = new System.Drawing.Size(334, 20);
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
            this.textbox_WindowsPassword.Location = new System.Drawing.Point(117, 140);
            this.textbox_WindowsPassword.Name = "textbox_WindowsPassword";
            this.textbox_WindowsPassword.PasswordChar = '*';
            this.textbox_WindowsPassword.Size = new System.Drawing.Size(334, 20);
            this.textbox_WindowsPassword.TabIndex = 2;
            this.textbox_WindowsPassword.TextChanged += new System.EventHandler(this._txtbx_WindowsPassword_TextChanged);
            // 
            // _page_Servers
            // 
            this._page_Servers.Controls.Add(this._btn_BrowseServers);
            this._page_Servers.Controls.Add(this._txtbx_Server);
            this._page_Servers.Controls.Add(this._lbl_Server);
            this._page_Servers.Description = "Type or browse for the SQL Server you want to audit.";
            this._page_Servers.DescriptionColor = System.Drawing.Color.Navy;
            this._page_Servers.Location = new System.Drawing.Point(19, 73);
            this._page_Servers.Name = "_page_Servers";
            this._page_Servers.NextPage = this._page_Credentials;
            this._page_Servers.PreviousPage = this._page_Introduction;
            this._page_Servers.Size = new System.Drawing.Size(476, 403);
            this._page_Servers.TabIndex = 0;
            this._page_Servers.Text = "Select a SQL Server";
            this._page_Servers.TextColor = System.Drawing.Color.Navy;
            this._page_Servers.BeforeMoveNext += new Divelements.WizardFramework.WizardPageEventHandler(this._page_Servers_BeforeMoveNext);
            this._page_Servers.BeforeDisplay += new System.EventHandler(this._page_Servers_BeforeDisplay);
            // 
            // _btn_BrowseServers
            // 
            this._btn_BrowseServers.Location = new System.Drawing.Point(436, 14);
            this._btn_BrowseServers.Name = "_btn_BrowseServers";
            this._btn_BrowseServers.Size = new System.Drawing.Size(24, 23);
            this._btn_BrowseServers.TabIndex = 2;
            this._btn_BrowseServers.Text = ".&..";
            this._btn_BrowseServers.UseVisualStyleBackColor = true;
            this._btn_BrowseServers.Click += new System.EventHandler(this._btn_BrowseServers_Click);
            // 
            // _txtbx_Server
            // 
            this._txtbx_Server.Location = new System.Drawing.Point(64, 16);
            this._txtbx_Server.Name = "_txtbx_Server";
            this._txtbx_Server.Size = new System.Drawing.Size(366, 20);
            this._txtbx_Server.TabIndex = 1;
            this._txtbx_Server.TextChanged += new System.EventHandler(this._txtbx_Server_TextChanged);
            // 
            // _lbl_Server
            // 
            this._lbl_Server.AutoSize = true;
            this._lbl_Server.Location = new System.Drawing.Point(17, 19);
            this._lbl_Server.Name = "_lbl_Server";
            this._lbl_Server.Size = new System.Drawing.Size(41, 13);
            this._lbl_Server.TabIndex = 0;
            this._lbl_Server.Text = "&Server:";
            // 
            // _page_Introduction
            // 
            this._page_Introduction.Controls.Add(this._rtb_Introduction);
            this._page_Introduction.IntroductionText = "";
            this._page_Introduction.Location = new System.Drawing.Point(177, 66);
            this._page_Introduction.Name = "_page_Introduction";
            this._page_Introduction.NextPage = this._page_Servers;
            this._page_Introduction.Size = new System.Drawing.Size(324, 410);
            this._page_Introduction.TabIndex = 1004;
            this._page_Introduction.Text = "Welcome to the Register a SQL Server Wizard";
            this._page_Introduction.BeforeDisplay += new System.EventHandler(this._page_Introduction_BeforeDisplay);
            // 
            // _rtb_Introduction
            // 
            this._rtb_Introduction.BackColor = System.Drawing.SystemColors.Window;
            this._rtb_Introduction.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._rtb_Introduction.Cursor = System.Windows.Forms.Cursors.No;
            this._rtb_Introduction.ForeColor = System.Drawing.Color.Navy;
            this._rtb_Introduction.Location = new System.Drawing.Point(0, 0);
            this._rtb_Introduction.Name = "_rtb_Introduction";
            this._rtb_Introduction.ReadOnly = true;
            this._rtb_Introduction.Size = new System.Drawing.Size(321, 211);
            this._rtb_Introduction.TabIndex = 0;
            this._rtb_Introduction.Text = "";
            // 
            // filterSelection1
            // 
            this.filterSelection1.AutoSize = true;
            this.filterSelection1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filterSelection1.Location = new System.Drawing.Point(0, 0);
            this.filterSelection1.Name = "filterSelection1";
            this.filterSelection1.Size = new System.Drawing.Size(476, 403);
            this.filterSelection1.TabIndex = 0;
            // 
            // controlSMTPEmailConfig1
            // 
            this.controlSMTPEmailConfig1.BackColor = System.Drawing.Color.Transparent;
            this.controlSMTPEmailConfig1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlSMTPEmailConfig1.Location = new System.Drawing.Point(0, 32);
            this.controlSMTPEmailConfig1.Name = "controlSMTPEmailConfig1";
            this.controlSMTPEmailConfig1.Size = new System.Drawing.Size(476, 335);
            this.controlSMTPEmailConfig1.TabIndex = 7;
            // 
            // addEditFoldersControl
            // 
            this.addEditFoldersControl.AutoSize = true;
            this.addEditFoldersControl.Location = new System.Drawing.Point(3, 3);
            this.addEditFoldersControl.Name = "addEditFoldersControl";
            this.addEditFoldersControl.Size = new System.Drawing.Size(470, 388);
            this.addEditFoldersControl.TabIndex = 0;
            this.addEditFoldersControl.TargetServerName = "";
            // 
            // Form_WizardRegisterSQLServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(514, 536);
            this.Controls.Add(this._wizard);
            this.ForeColor = System.Drawing.Color.Navy;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_WizardRegisterSQLServer";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Register a SQL Server";
            this._wizard.ResumeLayout(false);
            this._PageTags.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ulTags)).EndInit();
            this._page_DefineFilters.ResumeLayout(false);
            this._page_DefineFilters.PerformLayout();
            this._page_JobSchedule.ResumeLayout(false);
            this._page_JobSchedule.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_AgentStatus)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_KeepSnapshotDays)).EndInit();
            this._page_NotificationOptions.ResumeLayout(false);
            this._page_NotificationOptions.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this._page_ConfigureSMTPEmail.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this._page_Policies.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraListView_DynamicPolicies)).EndInit();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraListView_Policies)).EndInit();
            this._page_CollectData.ResumeLayout(false);
            this._page_Finish.ResumeLayout(false);
            this._page_FilePermissionFolders.ResumeLayout(false);
            this._page_FilePermissionFolders.PerformLayout();
            this._page_Credentials.ResumeLayout(false);
            this._grpbx_SQLServerCredentials.ResumeLayout(false);
            this._grpbx_SQLServerCredentials.PerformLayout();
            this._grpbx_WindowsGMCredentials.ResumeLayout(false);
            this._grpbx_WindowsGMCredentials.PerformLayout();
            this._page_Servers.ResumeLayout(false);
            this._page_Servers.PerformLayout();
            this._page_Introduction.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Divelements.WizardFramework.Wizard _wizard;
        private Divelements.WizardFramework.IntroductionPage _page_Introduction;
        private Divelements.WizardFramework.WizardPage _page_Servers;
        private Divelements.WizardFramework.FinishPage _page_Finish;
        private System.Windows.Forms.RichTextBox _rtb_Introduction;
        private Divelements.WizardFramework.WizardPage _page_DefineFilters;
        private System.Windows.Forms.TextBox _txtbx_Server;
        private Divelements.WizardFramework.WizardPage _page_Credentials;
        private System.Windows.Forms.GroupBox _grpbx_SQLServerCredentials;
        private System.Windows.Forms.Label _lbl_SqlLogin;
        private System.Windows.Forms.TextBox textbox_SqlLoginPassword;
        private System.Windows.Forms.Label _lbl_SqlLoginPassword;
        private System.Windows.Forms.TextBox textbox_SqlLogin;
        private System.Windows.Forms.GroupBox _grpbx_WindowsGMCredentials;
        private System.Windows.Forms.TextBox textbox_WindowsPassword;
        private System.Windows.Forms.Label _lbl_WindowsPassword;
        private System.Windows.Forms.TextBox textbox_WindowsUser;
        private System.Windows.Forms.Label _lbl_WindowsUser;
        private System.Windows.Forms.RichTextBox _rtb_Finish;
        private System.Windows.Forms.Button _btn_BrowseServers;
        private System.Windows.Forms.Label _lbl_Server;
        private Divelements.WizardFramework.WizardPage _page_JobSchedule;
        private System.Windows.Forms.Label label_KeepSnapshotDays;
        private System.Windows.Forms.NumericUpDown numericUpDown_KeepSnapshotDays;
        private System.Windows.Forms.Label label_KeepSnapshot;
        private System.Windows.Forms.CheckBox checkBox_EnableScheduling;
        private System.Windows.Forms.TextBox _txtbx_ScheduleDescription;
        private System.Windows.Forms.Button _btn_ChangeSchedule;
        private Idera.SQLsecure.UI.Console.Controls.FilterSelection filterSelection1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label_AgentStatus;
        private System.Windows.Forms.PictureBox pictureBox_AgentStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButton_WindowsAuth;
        private System.Windows.Forms.RadioButton radioButton_SQLServerAuth;
        private Divelements.WizardFramework.WizardPage _page_NotificationOptions;
        private System.Windows.Forms.RadioButton radioButton_SendEmailOnError;
        private System.Windows.Forms.RadioButton radioButton_SendEmailWarningOrError;
        private System.Windows.Forms.RadioButton radioButtonSendEmailFindingAny;
        private System.Windows.Forms.RadioButton radioButtonAlways;
        private System.Windows.Forms.RadioButton radioButtonSendEmailFindingHigh;
        private System.Windows.Forms.RadioButton radioButtonSendEmailFindingHighMedium;
        private System.Windows.Forms.CheckBox checkBox_UseSameAuth;
        private System.Windows.Forms.Label label2;
        private Divelements.WizardFramework.WizardPage _page_Policies;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_SQLWindowsUser;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_SQLWindowsPassword;
        private Infragistics.Win.UltraWinListView.UltraListView ultraListView_Policies;
        private System.Windows.Forms.CheckBox checkBoxEmailFindings;
        private System.Windows.Forms.CheckBox checkBoxEmailForCollectionStatus;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private Divelements.WizardFramework.WizardPage _page_CollectData;
        private System.Windows.Forms.TextBox textBox_Recipient;
        private System.Windows.Forms.CheckBox checkBox_CollectData;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel1;
        private Divelements.WizardFramework.WizardPage _page_ConfigureSMTPEmail;
        private Idera.SQLsecure.UI.Console.Controls.controlSMTPEmailConfig controlSMTPEmailConfig1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button_Test;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private Infragistics.Win.UltraWinListView.UltraListView ultraListView_DynamicPolicies;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private Divelements.WizardFramework.WizardPage _page_FilePermissionFolders;
        private Idera.SQLsecure.UI.Console.Controls.AddEditFolders addEditFoldersControl;
        private Divelements.WizardFramework.WizardPage _PageTags;
        private Infragistics.Win.UltraWinListView.UltraListView ulTags;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btAdd;
    }
}