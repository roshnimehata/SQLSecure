namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_WizardDeployReports
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
            System.Windows.Forms.Label label9;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label13;
            System.Windows.Forms.Label label8;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label7;
            System.Windows.Forms.Label label12;
            System.Windows.Forms.Label label6;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label10;
            this._gbRepository = new System.Windows.Forms.GroupBox();
            this._tbRepository = new System.Windows.Forms.TextBox();
            this._tbPassword = new System.Windows.Forms.TextBox();
            this._pnlRepository = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._tbConfirmPassword = new System.Windows.Forms.TextBox();
            this._tbLogin = new System.Windows.Forms.TextBox();
            this._groupReportManager = new System.Windows.Forms.GroupBox();
            this._tbRMVirtualDirectory = new System.Windows.Forms.TextBox();
            this._pnlReportServer = new System.Windows.Forms.Panel();
            this._groupReportServerSettings = new System.Windows.Forms.GroupBox();
            this._cbUseSsl = new System.Windows.Forms.CheckBox();
            this._tbPort = new System.Windows.Forms.TextBox();
            this._tbRSVirtualDirectory = new System.Windows.Forms.TextBox();
            this._cbAdvancedConnectionOptions = new System.Windows.Forms.CheckBox();
            this._tbReportServer = new System.Windows.Forms.TextBox();
            this._pnlTop = new System.Windows.Forms.Panel();
            this._lblDescription = new System.Windows.Forms.Label();
            this._lblTitle = new System.Windows.Forms.Label();
            this._pnlSelectReports = new System.Windows.Forms.Panel();
            this._btnBrowse = new System.Windows.Forms.Button();
            this._cbOverwriteExisting = new System.Windows.Forms.CheckBox();
            this._tbTargetFolder = new System.Windows.Forms.TextBox();
            this._btnNext = new System.Windows.Forms.Button();
            this._lblBorder2 = new System.Windows.Forms.Label();
            this._btnCancel = new System.Windows.Forms.Button();
            this._lblBorder1 = new System.Windows.Forms.Label();
            this._pnlBottom = new System.Windows.Forms.Panel();
            this._btnBack = new System.Windows.Forms.Button();
            this._btnFinish = new System.Windows.Forms.Button();
            this._pnlIntro = new System.Windows.Forms.Panel();
            this._lblIntro = new System.Windows.Forms.Label();
            this._progressBar = new System.Windows.Forms.ProgressBar();
            this._pnlStatus = new System.Windows.Forms.Panel();
            this._linkFinalLocation = new System.Windows.Forms.LinkLabel();
            this._lblFinalStatusMessage = new System.Windows.Forms.Label();
            this._tbStatus = new System.Windows.Forms.TextBox();
            this._pnlLeft = new System.Windows.Forms.Panel();
            this._pictureBox = new System.Windows.Forms.PictureBox();
            this._lblSumPort = new System.Windows.Forms.Label();
            this._pnlCenter = new System.Windows.Forms.Panel();
            this._pnlSummary = new System.Windows.Forms.Panel();
            this.label30 = new System.Windows.Forms.Label();
            this._lblSumUseSSL = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this._lblSumRSDirectory = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this._lblSumRMDirectory = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this._lblSumOverwrite = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this._lblSumTargetFolder = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this._lblSumRepositoryLogin = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this._lblSumRepository = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this._lblSumReportServer = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this._bgWorker = new System.ComponentModel.BackgroundWorker();
            label9 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label13 = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            label12 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label10 = new System.Windows.Forms.Label();
            this._gbRepository.SuspendLayout();
            this._pnlRepository.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this._groupReportManager.SuspendLayout();
            this._pnlReportServer.SuspendLayout();
            this._groupReportServerSettings.SuspendLayout();
            this._pnlTop.SuspendLayout();
            this._pnlSelectReports.SuspendLayout();
            this._pnlBottom.SuspendLayout();
            this._pnlIntro.SuspendLayout();
            this._pnlStatus.SuspendLayout();
            this._pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox)).BeginInit();
            this._pnlCenter.SuspendLayout();
            this._pnlSummary.SuspendLayout();
            this.SuspendLayout();
            // 
            // label9
            // 
            label9.Location = new System.Drawing.Point(6, 16);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(411, 18);
            label9.TabIndex = 0;
            label9.Text = "Specify which SQL Server instance hosts the SQLsecure Repository:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(14, 40);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(65, 13);
            label2.TabIndex = 0;
            label2.Text = "SQL Server:";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new System.Drawing.Point(30, 124);
            label13.Name = "label13";
            label13.Size = new System.Drawing.Size(94, 13);
            label13.TabIndex = 7;
            label13.Text = "Confirm Password:";
            // 
            // label8
            // 
            label8.Location = new System.Drawing.Point(6, 16);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(407, 48);
            label8.TabIndex = 6;
            label8.Text = "Specify the credentials the Report Server will use to connect to the Repository. " +
                " The specified account should have permission to execute stored procedures on th" +
                "e Repository database.";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(6, 73);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(118, 13);
            label3.TabIndex = 2;
            label3.Text = "Login ID (domain\\user):";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(68, 99);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(56, 13);
            label7.TabIndex = 4;
            label7.Text = "Password:";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new System.Drawing.Point(12, 22);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(84, 13);
            label12.TabIndex = 2;
            label12.Text = "Virtual Directory:";
            // 
            // label6
            // 
            label6.Location = new System.Drawing.Point(8, 222);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(426, 33);
            label6.TabIndex = 4;
            label6.Text = "Note:  To successfully deploy reports, you must have Content Manager rights on th" +
                "e Report Server.  For more information, see the Reporting Services Books Online." +
                "";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(12, 18);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(29, 13);
            label5.TabIndex = 4;
            label5.Text = "Port:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(12, 44);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(84, 13);
            label4.TabIndex = 2;
            label4.Text = "Virtual Directory:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(16, 14);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(76, 13);
            label1.TabIndex = 3;
            label1.Text = "Report Server:";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new System.Drawing.Point(13, 32);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(73, 13);
            label10.TabIndex = 0;
            label10.Text = "Target Folder:";
            // 
            // _gbRepository
            // 
            this._gbRepository.Controls.Add(label9);
            this._gbRepository.Controls.Add(label2);
            this._gbRepository.Controls.Add(this._tbRepository);
            this._gbRepository.Location = new System.Drawing.Point(11, 8);
            this._gbRepository.Name = "_gbRepository";
            this._gbRepository.Size = new System.Drawing.Size(423, 83);
            this._gbRepository.TabIndex = 1;
            this._gbRepository.TabStop = false;
            this._gbRepository.Text = "Repository Server";
            // 
            // _tbRepository
            // 
            this._tbRepository.Location = new System.Drawing.Point(89, 37);
            this._tbRepository.Name = "_tbRepository";
            this._tbRepository.Size = new System.Drawing.Size(221, 20);
            this._tbRepository.TabIndex = 0;
            // 
            // _tbPassword
            // 
            this._tbPassword.Location = new System.Drawing.Point(147, 96);
            this._tbPassword.Name = "_tbPassword";
            this._tbPassword.PasswordChar = '*';
            this._tbPassword.Size = new System.Drawing.Size(163, 20);
            this._tbPassword.TabIndex = 1;
            // 
            // _pnlRepository
            // 
            this._pnlRepository.Controls.Add(this._gbRepository);
            this._pnlRepository.Controls.Add(this.groupBox1);
            this._pnlRepository.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlRepository.Location = new System.Drawing.Point(0, 0);
            this._pnlRepository.Name = "_pnlRepository";
            this._pnlRepository.Size = new System.Drawing.Size(446, 274);
            this._pnlRepository.TabIndex = 2;
            this._pnlRepository.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._tbConfirmPassword);
            this.groupBox1.Controls.Add(label13);
            this.groupBox1.Controls.Add(label8);
            this.groupBox1.Controls.Add(label3);
            this.groupBox1.Controls.Add(this._tbPassword);
            this.groupBox1.Controls.Add(this._tbLogin);
            this.groupBox1.Controls.Add(label7);
            this.groupBox1.Location = new System.Drawing.Point(11, 101);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(423, 154);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Repository Credentials";
            // 
            // _tbConfirmPassword
            // 
            this._tbConfirmPassword.Location = new System.Drawing.Point(147, 122);
            this._tbConfirmPassword.Name = "_tbConfirmPassword";
            this._tbConfirmPassword.PasswordChar = '*';
            this._tbConfirmPassword.Size = new System.Drawing.Size(163, 20);
            this._tbConfirmPassword.TabIndex = 2;
            // 
            // _tbLogin
            // 
            this._tbLogin.Location = new System.Drawing.Point(147, 70);
            this._tbLogin.Name = "_tbLogin";
            this._tbLogin.Size = new System.Drawing.Size(163, 20);
            this._tbLogin.TabIndex = 0;
            // 
            // _groupReportManager
            // 
            this._groupReportManager.Controls.Add(this._tbRMVirtualDirectory);
            this._groupReportManager.Controls.Add(label12);
            this._groupReportManager.Location = new System.Drawing.Point(19, 134);
            this._groupReportManager.Name = "_groupReportManager";
            this._groupReportManager.Size = new System.Drawing.Size(423, 45);
            this._groupReportManager.TabIndex = 5;
            this._groupReportManager.TabStop = false;
            this._groupReportManager.Text = "Report Manager Settings";
            this._groupReportManager.Visible = false;
            // 
            // _tbRMVirtualDirectory
            // 
            this._tbRMVirtualDirectory.Location = new System.Drawing.Point(102, 19);
            this._tbRMVirtualDirectory.Name = "_tbRMVirtualDirectory";
            this._tbRMVirtualDirectory.Size = new System.Drawing.Size(244, 20);
            this._tbRMVirtualDirectory.TabIndex = 2;
            this._tbRMVirtualDirectory.Text = "Reports";
            // 
            // _pnlReportServer
            // 
            this._pnlReportServer.Controls.Add(this._groupReportManager);
            this._pnlReportServer.Controls.Add(label6);
            this._pnlReportServer.Controls.Add(this._groupReportServerSettings);
            this._pnlReportServer.Controls.Add(this._cbAdvancedConnectionOptions);
            this._pnlReportServer.Controls.Add(this._tbReportServer);
            this._pnlReportServer.Controls.Add(label1);
            this._pnlReportServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlReportServer.Location = new System.Drawing.Point(0, 0);
            this._pnlReportServer.Name = "_pnlReportServer";
            this._pnlReportServer.Size = new System.Drawing.Size(446, 274);
            this._pnlReportServer.TabIndex = 0;
            this._pnlReportServer.Visible = false;
            // 
            // _groupReportServerSettings
            // 
            this._groupReportServerSettings.Controls.Add(this._cbUseSsl);
            this._groupReportServerSettings.Controls.Add(this._tbPort);
            this._groupReportServerSettings.Controls.Add(label5);
            this._groupReportServerSettings.Controls.Add(this._tbRSVirtualDirectory);
            this._groupReportServerSettings.Controls.Add(label4);
            this._groupReportServerSettings.Location = new System.Drawing.Point(19, 60);
            this._groupReportServerSettings.Name = "_groupReportServerSettings";
            this._groupReportServerSettings.Size = new System.Drawing.Size(423, 67);
            this._groupReportServerSettings.TabIndex = 2;
            this._groupReportServerSettings.TabStop = false;
            this._groupReportServerSettings.Text = "Report Server Settings";
            this._groupReportServerSettings.Visible = false;
            // 
            // _cbUseSsl
            // 
            this._cbUseSsl.AutoSize = true;
            this._cbUseSsl.Location = new System.Drawing.Point(171, 18);
            this._cbUseSsl.Name = "_cbUseSsl";
            this._cbUseSsl.Size = new System.Drawing.Size(68, 17);
            this._cbUseSsl.TabIndex = 1;
            this._cbUseSsl.Text = "Use SSL";
            this._cbUseSsl.UseVisualStyleBackColor = true;
            // 
            // _tbPort
            // 
            this._tbPort.Location = new System.Drawing.Point(102, 15);
            this._tbPort.Name = "_tbPort";
            this._tbPort.Size = new System.Drawing.Size(39, 20);
            this._tbPort.TabIndex = 0;
            this._tbPort.Text = "80";
            // 
            // _tbRSVirtualDirectory
            // 
            this._tbRSVirtualDirectory.Location = new System.Drawing.Point(102, 41);
            this._tbRSVirtualDirectory.Name = "_tbRSVirtualDirectory";
            this._tbRSVirtualDirectory.Size = new System.Drawing.Size(244, 20);
            this._tbRSVirtualDirectory.TabIndex = 2;
            this._tbRSVirtualDirectory.Text = "ReportServer";
            // 
            // _cbAdvancedConnectionOptions
            // 
            this._cbAdvancedConnectionOptions.AutoSize = true;
            this._cbAdvancedConnectionOptions.Location = new System.Drawing.Point(19, 37);
            this._cbAdvancedConnectionOptions.Name = "_cbAdvancedConnectionOptions";
            this._cbAdvancedConnectionOptions.Size = new System.Drawing.Size(197, 17);
            this._cbAdvancedConnectionOptions.TabIndex = 1;
            this._cbAdvancedConnectionOptions.Text = "Show advanced connection options";
            this._cbAdvancedConnectionOptions.UseVisualStyleBackColor = true;
            this._cbAdvancedConnectionOptions.CheckedChanged += new System.EventHandler(this.CheckedChanged_cbAdvancedConnectionOptions);
            // 
            // _tbReportServer
            // 
            this._tbReportServer.Location = new System.Drawing.Point(98, 11);
            this._tbReportServer.Name = "_tbReportServer";
            this._tbReportServer.Size = new System.Drawing.Size(182, 20);
            this._tbReportServer.TabIndex = 0;
            // 
            // _pnlTop
            // 
            this._pnlTop.BackColor = System.Drawing.Color.White;
            this._pnlTop.Controls.Add(this._lblDescription);
            this._pnlTop.Controls.Add(this._lblTitle);
            this._pnlTop.Location = new System.Drawing.Point(110, 0);
            this._pnlTop.Name = "_pnlTop";
            this._pnlTop.Size = new System.Drawing.Size(446, 60);
            this._pnlTop.TabIndex = 6;
            // 
            // _lblDescription
            // 
            this._lblDescription.Location = new System.Drawing.Point(14, 24);
            this._lblDescription.Name = "_lblDescription";
            this._lblDescription.Size = new System.Drawing.Size(416, 28);
            this._lblDescription.TabIndex = 0;
            this._lblDescription.Text = "Description";
            // 
            // _lblTitle
            // 
            this._lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblTitle.Location = new System.Drawing.Point(14, 8);
            this._lblTitle.Name = "_lblTitle";
            this._lblTitle.Size = new System.Drawing.Size(281, 16);
            this._lblTitle.TabIndex = 1;
            this._lblTitle.Text = "Title";
            this._lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _pnlSelectReports
            // 
            this._pnlSelectReports.Controls.Add(this._btnBrowse);
            this._pnlSelectReports.Controls.Add(this._cbOverwriteExisting);
            this._pnlSelectReports.Controls.Add(this._tbTargetFolder);
            this._pnlSelectReports.Controls.Add(label10);
            this._pnlSelectReports.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlSelectReports.Location = new System.Drawing.Point(0, 0);
            this._pnlSelectReports.Name = "_pnlSelectReports";
            this._pnlSelectReports.Size = new System.Drawing.Size(446, 274);
            this._pnlSelectReports.TabIndex = 1;
            this._pnlSelectReports.Visible = false;
            // 
            // _btnBrowse
            // 
            this._btnBrowse.Location = new System.Drawing.Point(339, 27);
            this._btnBrowse.Name = "_btnBrowse";
            this._btnBrowse.Size = new System.Drawing.Size(75, 23);
            this._btnBrowse.TabIndex = 1;
            this._btnBrowse.Text = "Browse...";
            this._btnBrowse.UseVisualStyleBackColor = true;
            this._btnBrowse.Click += new System.EventHandler(this.Click_btnBrowse);
            // 
            // _cbOverwriteExisting
            // 
            this._cbOverwriteExisting.AutoSize = true;
            this._cbOverwriteExisting.Location = new System.Drawing.Point(20, 60);
            this._cbOverwriteExisting.Name = "_cbOverwriteExisting";
            this._cbOverwriteExisting.Size = new System.Drawing.Size(144, 17);
            this._cbOverwriteExisting.TabIndex = 2;
            this._cbOverwriteExisting.Text = "Overwrite existing reports";
            this._cbOverwriteExisting.UseVisualStyleBackColor = true;
            // 
            // _tbTargetFolder
            // 
            this._tbTargetFolder.Location = new System.Drawing.Point(89, 29);
            this._tbTargetFolder.Name = "_tbTargetFolder";
            this._tbTargetFolder.Size = new System.Drawing.Size(244, 20);
            this._tbTargetFolder.TabIndex = 0;
            // 
            // _btnNext
            // 
            this._btnNext.Location = new System.Drawing.Point(350, 10);
            this._btnNext.Name = "_btnNext";
            this._btnNext.Size = new System.Drawing.Size(62, 20);
            this._btnNext.TabIndex = 1;
            this._btnNext.Text = "Next >";
            this._btnNext.Click += new System.EventHandler(this.Click_btnNext);
            // 
            // _lblBorder2
            // 
            this._lblBorder2.BackColor = System.Drawing.Color.Black;
            this._lblBorder2.Location = new System.Drawing.Point(0, 335);
            this._lblBorder2.Name = "_lblBorder2";
            this._lblBorder2.Size = new System.Drawing.Size(556, 1);
            this._lblBorder2.TabIndex = 10;
            this._lblBorder2.Text = "label2";
            // 
            // _btnCancel
            // 
            this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnCancel.Location = new System.Drawing.Point(490, 10);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(62, 20);
            this._btnCancel.TabIndex = 3;
            this._btnCancel.Text = "Cancel";
            this._btnCancel.Click += new System.EventHandler(this.Click_btnCancel);
            // 
            // _lblBorder1
            // 
            this._lblBorder1.BackColor = System.Drawing.Color.Black;
            this._lblBorder1.Location = new System.Drawing.Point(110, 60);
            this._lblBorder1.Name = "_lblBorder1";
            this._lblBorder1.Size = new System.Drawing.Size(446, 1);
            this._lblBorder1.TabIndex = 8;
            this._lblBorder1.Text = "label1";
            // 
            // _pnlBottom
            // 
            this._pnlBottom.Controls.Add(this._btnCancel);
            this._pnlBottom.Controls.Add(this._btnNext);
            this._pnlBottom.Controls.Add(this._btnBack);
            this._pnlBottom.Controls.Add(this._btnFinish);
            this._pnlBottom.Location = new System.Drawing.Point(0, 336);
            this._pnlBottom.Name = "_pnlBottom";
            this._pnlBottom.Size = new System.Drawing.Size(556, 38);
            this._pnlBottom.TabIndex = 9;
            // 
            // _btnBack
            // 
            this._btnBack.Enabled = false;
            this._btnBack.Location = new System.Drawing.Point(280, 10);
            this._btnBack.Name = "_btnBack";
            this._btnBack.Size = new System.Drawing.Size(62, 20);
            this._btnBack.TabIndex = 0;
            this._btnBack.Text = "< Back";
            this._btnBack.Click += new System.EventHandler(this.Click_btnBack);
            // 
            // _btnFinish
            // 
            this._btnFinish.Enabled = false;
            this._btnFinish.Location = new System.Drawing.Point(420, 10);
            this._btnFinish.Name = "_btnFinish";
            this._btnFinish.Size = new System.Drawing.Size(62, 20);
            this._btnFinish.TabIndex = 2;
            this._btnFinish.Text = "Finish";
            this._btnFinish.Click += new System.EventHandler(this.Click_btnFinish);
            // 
            // _pnlIntro
            // 
            this._pnlIntro.Controls.Add(this._lblIntro);
            this._pnlIntro.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlIntro.Location = new System.Drawing.Point(0, 0);
            this._pnlIntro.Name = "_pnlIntro";
            this._pnlIntro.Size = new System.Drawing.Size(446, 274);
            this._pnlIntro.TabIndex = 0;
            // 
            // _lblIntro
            // 
            this._lblIntro.Location = new System.Drawing.Point(13, 18);
            this._lblIntro.Name = "_lblIntro";
            this._lblIntro.Size = new System.Drawing.Size(423, 169);
            this._lblIntro.TabIndex = 0;
            this._lblIntro.Text = "This wizard allows you to integrate SQLsecure Reports with your existing Microsof" +
                "t Reporting Services installation.";
            // 
            // _progressBar
            // 
            this._progressBar.Location = new System.Drawing.Point(11, 204);
            this._progressBar.Name = "_progressBar";
            this._progressBar.Size = new System.Drawing.Size(423, 23);
            this._progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this._progressBar.TabIndex = 3;
            // 
            // _pnlStatus
            // 
            this._pnlStatus.Controls.Add(this._progressBar);
            this._pnlStatus.Controls.Add(this._linkFinalLocation);
            this._pnlStatus.Controls.Add(this._lblFinalStatusMessage);
            this._pnlStatus.Controls.Add(this._tbStatus);
            this._pnlStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlStatus.Location = new System.Drawing.Point(0, 0);
            this._pnlStatus.Name = "_pnlStatus";
            this._pnlStatus.Size = new System.Drawing.Size(446, 274);
            this._pnlStatus.TabIndex = 2;
            this._pnlStatus.Visible = false;
            // 
            // _linkFinalLocation
            // 
            this._linkFinalLocation.AutoSize = true;
            this._linkFinalLocation.Location = new System.Drawing.Point(8, 242);
            this._linkFinalLocation.Name = "_linkFinalLocation";
            this._linkFinalLocation.Size = new System.Drawing.Size(194, 13);
            this._linkFinalLocation.TabIndex = 2;
            this._linkFinalLocation.TabStop = true;
            this._linkFinalLocation.Text = "Reports are hosted at:  http://blah/blah";
            this._linkFinalLocation.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._linkFinalLocation_LinkClicked);
            // 
            // _lblFinalStatusMessage
            // 
            this._lblFinalStatusMessage.AutoSize = true;
            this._lblFinalStatusMessage.Location = new System.Drawing.Point(8, 204);
            this._lblFinalStatusMessage.Name = "_lblFinalStatusMessage";
            this._lblFinalStatusMessage.Size = new System.Drawing.Size(271, 13);
            this._lblFinalStatusMessage.TabIndex = 1;
            this._lblFinalStatusMessage.Text = "You have successfully deployed the SQLsecure reports.";
            this._lblFinalStatusMessage.Visible = false;
            // 
            // _tbStatus
            // 
            this._tbStatus.Location = new System.Drawing.Point(11, 5);
            this._tbStatus.Multiline = true;
            this._tbStatus.Name = "_tbStatus";
            this._tbStatus.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._tbStatus.Size = new System.Drawing.Size(423, 192);
            this._tbStatus.TabIndex = 0;
            this._tbStatus.WordWrap = false;
            // 
            // _pnlLeft
            // 
            this._pnlLeft.Controls.Add(this._pictureBox);
            this._pnlLeft.Location = new System.Drawing.Point(0, 0);
            this._pnlLeft.Name = "_pnlLeft";
            this._pnlLeft.Size = new System.Drawing.Size(110, 335);
            this._pnlLeft.TabIndex = 5;
            // 
            // _pictureBox
            // 
            this._pictureBox.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.Welcome_ObjectPermissions1;
            this._pictureBox.Location = new System.Drawing.Point(0, 0);
            this._pictureBox.Name = "_pictureBox";
            this._pictureBox.Size = new System.Drawing.Size(110, 335);
            this._pictureBox.TabIndex = 0;
            this._pictureBox.TabStop = false;
            // 
            // _lblSumPort
            // 
            this._lblSumPort.Location = new System.Drawing.Point(192, 45);
            this._lblSumPort.Name = "_lblSumPort";
            this._lblSumPort.Size = new System.Drawing.Size(224, 16);
            this._lblSumPort.TabIndex = 17;
            this._lblSumPort.Text = "Server";
            this._lblSumPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _pnlCenter
            // 
            this._pnlCenter.Controls.Add(this._pnlSummary);
            this._pnlCenter.Controls.Add(this._pnlStatus);
            this._pnlCenter.Controls.Add(this._pnlReportServer);
            this._pnlCenter.Controls.Add(this._pnlRepository);
            this._pnlCenter.Controls.Add(this._pnlSelectReports);
            this._pnlCenter.Controls.Add(this._pnlIntro);
            this._pnlCenter.Location = new System.Drawing.Point(110, 61);
            this._pnlCenter.Name = "_pnlCenter";
            this._pnlCenter.Size = new System.Drawing.Size(446, 274);
            this._pnlCenter.TabIndex = 7;
            // 
            // _pnlSummary
            // 
            this._pnlSummary.Controls.Add(this._lblSumPort);
            this._pnlSummary.Controls.Add(this.label30);
            this._pnlSummary.Controls.Add(this._lblSumUseSSL);
            this._pnlSummary.Controls.Add(this.label28);
            this._pnlSummary.Controls.Add(this._lblSumRSDirectory);
            this._pnlSummary.Controls.Add(this.label26);
            this._pnlSummary.Controls.Add(this._lblSumRMDirectory);
            this._pnlSummary.Controls.Add(this.label24);
            this._pnlSummary.Controls.Add(this._lblSumOverwrite);
            this._pnlSummary.Controls.Add(this.label22);
            this._pnlSummary.Controls.Add(this._lblSumTargetFolder);
            this._pnlSummary.Controls.Add(this.label20);
            this._pnlSummary.Controls.Add(this._lblSumRepositoryLogin);
            this._pnlSummary.Controls.Add(this.label18);
            this._pnlSummary.Controls.Add(this._lblSumRepository);
            this._pnlSummary.Controls.Add(this.label16);
            this._pnlSummary.Controls.Add(this._lblSumReportServer);
            this._pnlSummary.Controls.Add(this.label11);
            this._pnlSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlSummary.Location = new System.Drawing.Point(0, 0);
            this._pnlSummary.Name = "_pnlSummary";
            this._pnlSummary.Size = new System.Drawing.Size(446, 274);
            this._pnlSummary.TabIndex = 1;
            this._pnlSummary.Visible = false;
            // 
            // label30
            // 
            this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label30.Location = new System.Drawing.Point(17, 45);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(118, 16);
            this.label30.TabIndex = 16;
            this.label30.Text = "Port:";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lblSumUseSSL
            // 
            this._lblSumUseSSL.Location = new System.Drawing.Point(192, 66);
            this._lblSumUseSSL.Name = "_lblSumUseSSL";
            this._lblSumUseSSL.Size = new System.Drawing.Size(224, 16);
            this._lblSumUseSSL.TabIndex = 15;
            this._lblSumUseSSL.Text = "Server";
            this._lblSumUseSSL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label28
            // 
            this.label28.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label28.Location = new System.Drawing.Point(17, 66);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(118, 16);
            this.label28.TabIndex = 14;
            this.label28.Text = "Use SSL:";
            this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lblSumRSDirectory
            // 
            this._lblSumRSDirectory.Location = new System.Drawing.Point(192, 87);
            this._lblSumRSDirectory.Name = "_lblSumRSDirectory";
            this._lblSumRSDirectory.Size = new System.Drawing.Size(224, 16);
            this._lblSumRSDirectory.TabIndex = 13;
            this._lblSumRSDirectory.Text = "Server";
            this._lblSumRSDirectory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label26
            // 
            this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.Location = new System.Drawing.Point(17, 87);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(147, 16);
            this.label26.TabIndex = 12;
            this.label26.Text = "Report Server Directory:";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lblSumRMDirectory
            // 
            this._lblSumRMDirectory.Location = new System.Drawing.Point(192, 108);
            this._lblSumRMDirectory.Name = "_lblSumRMDirectory";
            this._lblSumRMDirectory.Size = new System.Drawing.Size(224, 16);
            this._lblSumRMDirectory.TabIndex = 11;
            this._lblSumRMDirectory.Text = "Server";
            this._lblSumRMDirectory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label24
            // 
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.Location = new System.Drawing.Point(17, 108);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(169, 16);
            this.label24.TabIndex = 10;
            this.label24.Text = "Report Manager Directory:";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lblSumOverwrite
            // 
            this._lblSumOverwrite.Location = new System.Drawing.Point(192, 192);
            this._lblSumOverwrite.Name = "_lblSumOverwrite";
            this._lblSumOverwrite.Size = new System.Drawing.Size(224, 16);
            this._lblSumOverwrite.TabIndex = 9;
            this._lblSumOverwrite.Text = "Server";
            this._lblSumOverwrite.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label22
            // 
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(17, 192);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(169, 16);
            this.label22.TabIndex = 8;
            this.label22.Text = "Overwrite Existing Reports:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lblSumTargetFolder
            // 
            this._lblSumTargetFolder.Location = new System.Drawing.Point(192, 171);
            this._lblSumTargetFolder.Name = "_lblSumTargetFolder";
            this._lblSumTargetFolder.Size = new System.Drawing.Size(224, 16);
            this._lblSumTargetFolder.TabIndex = 7;
            this._lblSumTargetFolder.Text = "Server";
            this._lblSumTargetFolder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label20
            // 
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(17, 171);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(118, 16);
            this.label20.TabIndex = 6;
            this.label20.Text = "Target Folder:";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lblSumRepositoryLogin
            // 
            this._lblSumRepositoryLogin.Location = new System.Drawing.Point(192, 150);
            this._lblSumRepositoryLogin.Name = "_lblSumRepositoryLogin";
            this._lblSumRepositoryLogin.Size = new System.Drawing.Size(224, 16);
            this._lblSumRepositoryLogin.TabIndex = 5;
            this._lblSumRepositoryLogin.Text = "Server";
            this._lblSumRepositoryLogin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label18
            // 
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(17, 150);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(118, 16);
            this.label18.TabIndex = 4;
            this.label18.Text = "Repository Login:";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lblSumRepository
            // 
            this._lblSumRepository.Location = new System.Drawing.Point(192, 129);
            this._lblSumRepository.Name = "_lblSumRepository";
            this._lblSumRepository.Size = new System.Drawing.Size(224, 16);
            this._lblSumRepository.TabIndex = 3;
            this._lblSumRepository.Text = "Server";
            this._lblSumRepository.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label16
            // 
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(17, 129);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(118, 16);
            this.label16.TabIndex = 2;
            this.label16.Text = "Repository Server:";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lblSumReportServer
            // 
            this._lblSumReportServer.Location = new System.Drawing.Point(192, 24);
            this._lblSumReportServer.Name = "_lblSumReportServer";
            this._lblSumReportServer.Size = new System.Drawing.Size(224, 16);
            this._lblSumReportServer.TabIndex = 1;
            this._lblSumReportServer.Text = "Server";
            this._lblSumReportServer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(17, 24);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(118, 16);
            this.label11.TabIndex = 0;
            this.label11.Text = "Report Server:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _bgWorker
            // 
            this._bgWorker.WorkerReportsProgress = true;
            this._bgWorker.WorkerSupportsCancellation = true;
            this._bgWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.DoWork_bgWorker);
            this._bgWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.RunWorkerCompleted_bgWorker);
            this._bgWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.ProgressChanged_bgWorker);
            // 
            // Form_WizardDeployReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._btnCancel;
            this.ClientSize = new System.Drawing.Size(556, 374);
            this.Controls.Add(this._pnlTop);
            this.Controls.Add(this._lblBorder2);
            this.Controls.Add(this._lblBorder1);
            this.Controls.Add(this._pnlBottom);
            this.Controls.Add(this._pnlLeft);
            this.Controls.Add(this._pnlCenter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_WizardDeployReports";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Deploy Reports to Reporting Services";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_DeployReportsWizard_FormClosing);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.Form_DeployReportsWizard_HelpRequested);
            this._gbRepository.ResumeLayout(false);
            this._gbRepository.PerformLayout();
            this._pnlRepository.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this._groupReportManager.ResumeLayout(false);
            this._groupReportManager.PerformLayout();
            this._pnlReportServer.ResumeLayout(false);
            this._pnlReportServer.PerformLayout();
            this._groupReportServerSettings.ResumeLayout(false);
            this._groupReportServerSettings.PerformLayout();
            this._pnlTop.ResumeLayout(false);
            this._pnlSelectReports.ResumeLayout(false);
            this._pnlSelectReports.PerformLayout();
            this._pnlBottom.ResumeLayout(false);
            this._pnlIntro.ResumeLayout(false);
            this._pnlStatus.ResumeLayout(false);
            this._pnlStatus.PerformLayout();
            this._pnlLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox)).EndInit();
            this._pnlCenter.ResumeLayout(false);
            this._pnlSummary.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox _gbRepository;
        private System.Windows.Forms.TextBox _tbRepository;
        private System.Windows.Forms.TextBox _tbPassword;
        private System.Windows.Forms.Panel _pnlRepository;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox _tbConfirmPassword;
        private System.Windows.Forms.TextBox _tbLogin;
        private System.Windows.Forms.GroupBox _groupReportManager;
        private System.Windows.Forms.TextBox _tbRMVirtualDirectory;
        private System.Windows.Forms.Panel _pnlReportServer;
        private System.Windows.Forms.GroupBox _groupReportServerSettings;
        private System.Windows.Forms.CheckBox _cbUseSsl;
        private System.Windows.Forms.TextBox _tbPort;
        private System.Windows.Forms.TextBox _tbRSVirtualDirectory;
        private System.Windows.Forms.CheckBox _cbAdvancedConnectionOptions;
        private System.Windows.Forms.TextBox _tbReportServer;
        private System.Windows.Forms.Panel _pnlTop;
        private System.Windows.Forms.Label _lblDescription;
        private System.Windows.Forms.Label _lblTitle;
        private System.Windows.Forms.Panel _pnlSelectReports;
        private System.Windows.Forms.Button _btnBrowse;
        private System.Windows.Forms.CheckBox _cbOverwriteExisting;
        private System.Windows.Forms.TextBox _tbTargetFolder;
        private System.Windows.Forms.Button _btnNext;
        private System.Windows.Forms.Label _lblBorder2;
        private System.Windows.Forms.Button _btnCancel;
        private System.Windows.Forms.Label _lblBorder1;
        private System.Windows.Forms.Panel _pnlBottom;
        private System.Windows.Forms.Button _btnBack;
        private System.Windows.Forms.Button _btnFinish;
        private System.Windows.Forms.Panel _pnlIntro;
        private System.Windows.Forms.Label _lblIntro;
        private System.Windows.Forms.ProgressBar _progressBar;
        private System.Windows.Forms.Panel _pnlStatus;
        private System.Windows.Forms.LinkLabel _linkFinalLocation;
        private System.Windows.Forms.Label _lblFinalStatusMessage;
        private System.Windows.Forms.TextBox _tbStatus;
        private System.Windows.Forms.Panel _pnlLeft;
        private System.Windows.Forms.PictureBox _pictureBox;
        private System.Windows.Forms.Label _lblSumPort;
        private System.Windows.Forms.Panel _pnlCenter;
        private System.Windows.Forms.Panel _pnlSummary;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label _lblSumUseSSL;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label _lblSumRSDirectory;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label _lblSumRMDirectory;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label _lblSumOverwrite;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label _lblSumTargetFolder;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label _lblSumRepositoryLogin;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label _lblSumRepository;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label _lblSumReportServer;
        private System.Windows.Forms.Label label11;
        private System.ComponentModel.BackgroundWorker _bgWorker;
    }
}