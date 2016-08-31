namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_WizardUserPermissions
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
            this._wizard = new Divelements.WizardFramework.Wizard();
            this._page_Finish = new Divelements.WizardFramework.FinishPage();
            this._rtb_Finish = new System.Windows.Forms.RichTextBox();
            this._Page_SelectDatabase = new Divelements.WizardFramework.WizardPage();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this._selectDatabase = new Idera.SQLsecure.UI.Console.Controls.SelectDatabase();
            this._Page_SelectUser = new Divelements.WizardFramework.WizardPage();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this._groupBox_SelectUser = new System.Windows.Forms.GroupBox();
            this._button_BrowseUsers = new System.Windows.Forms.Button();
            this._textBox_User = new System.Windows.Forms.TextBox();
            this._radioButton_SQLLogin = new System.Windows.Forms.RadioButton();
            this._radioButton_WindowsUser = new System.Windows.Forms.RadioButton();
            this._page_SelectSnapshot = new Divelements.WizardFramework.WizardPage();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this._selectSnapshot = new Idera.SQLsecure.UI.Console.Controls.SelectSnapshot();
            this._page_SelectServer = new Divelements.WizardFramework.WizardPage();
            this._selectServer = new Idera.SQLsecure.UI.Console.Controls.SelectServer();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this._page_Introduction = new Divelements.WizardFramework.IntroductionPage();
            this._rtb_Introduction = new System.Windows.Forms.RichTextBox();
            this._wizard.SuspendLayout();
            this._page_Finish.SuspendLayout();
            this._Page_SelectDatabase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this._Page_SelectUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this._groupBox_SelectUser.SuspendLayout();
            this._page_SelectSnapshot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this._page_SelectServer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this._page_Introduction.SuspendLayout();
            this.SuspendLayout();
            // 
            // _wizard
            // 
            this._wizard.BannerImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.FindUserPermissions_49x49;
            this._wizard.Controls.Add(this._page_Finish);
            this._wizard.Controls.Add(this._page_SelectServer);
            this._wizard.Controls.Add(this._Page_SelectDatabase);
            this._wizard.Controls.Add(this._Page_SelectUser);
            this._wizard.Controls.Add(this._page_SelectSnapshot);
            this._wizard.Controls.Add(this._page_Introduction);
            this._wizard.Dock = System.Windows.Forms.DockStyle.Fill;
            this._wizard.HelpVisible = true;
            this._wizard.Location = new System.Drawing.Point(0, 0);
            this._wizard.MarginImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Welcome_FindUserPermissions1;
            this._wizard.Name = "_wizard";
            this._wizard.SelectedPage = this._page_Introduction;
            this._wizard.Size = new System.Drawing.Size(548, 508);
            this._wizard.TabIndex = 1;
            // 
            // _page_Finish
            // 
            this._page_Finish.Controls.Add(this._rtb_Finish);
            this._page_Finish.FinishText = "";
            this._page_Finish.Location = new System.Drawing.Point(177, 66);
            this._page_Finish.Name = "_page_Finish";
            this._page_Finish.PreviousPage = this._Page_SelectDatabase;
            this._page_Finish.ProceedText = "To Explore User Permissions, click Finish";
            this._page_Finish.Size = new System.Drawing.Size(358, 382);
            this._page_Finish.TabIndex = 1006;
            this._page_Finish.Text = "Completing SQLsecure Explore User Permissions Wizard";
            this._page_Finish.CollectSettings += new Divelements.WizardFramework.WizardFinishPageEventHandler(this._page_Finish_CollectSettings);
            this._page_Finish.BeforeMoveBack += new Divelements.WizardFramework.WizardPageEventHandler(this._page_Finish_BeforeMoveBack);
            this._page_Finish.BeforeDisplay += new System.EventHandler(this._page_Finish_BeforeDisplay);
            // 
            // _rtb_Finish
            // 
            this._rtb_Finish.BackColor = System.Drawing.SystemColors.Window;
            this._rtb_Finish.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._rtb_Finish.Location = new System.Drawing.Point(0, 0);
            this._rtb_Finish.Name = "_rtb_Finish";
            this._rtb_Finish.ReadOnly = true;
            this._rtb_Finish.Size = new System.Drawing.Size(355, 343);
            this._rtb_Finish.TabIndex = 1;
            this._rtb_Finish.Text = "";
            // 
            // _Page_SelectDatabase
            // 
            this._Page_SelectDatabase.BackColor = System.Drawing.SystemColors.Control;
            this._Page_SelectDatabase.Controls.Add(this.pictureBox4);
            this._Page_SelectDatabase.Controls.Add(this._selectDatabase);
            this._Page_SelectDatabase.Description = "The list below contains all the databases collected in the selected snapshot. You" +
                " can choose to view only server level permissions or both server and database pe" +
                "rmissions.";
            this._Page_SelectDatabase.Location = new System.Drawing.Point(19, 73);
            this._Page_SelectDatabase.Name = "_Page_SelectDatabase";
            this._Page_SelectDatabase.NextPage = this._page_Finish;
            this._Page_SelectDatabase.PreviousPage = this._Page_SelectUser;
            this._Page_SelectDatabase.Size = new System.Drawing.Size(510, 375);
            this._Page_SelectDatabase.TabIndex = 1011;
            this._Page_SelectDatabase.Text = "Select Database";
            this._Page_SelectDatabase.BeforeMoveNext += new Divelements.WizardFramework.WizardPageEventHandler(this._Page_SelectDatabase_BeforeMoveNext);
            this._Page_SelectDatabase.BeforeDisplay += new System.EventHandler(this._Page_SelectDatabase_BeforeDisplay);
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.WizardSteps_4of4;
            this.pictureBox4.Location = new System.Drawing.Point(0, 0);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(164, 375);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox4.TabIndex = 2;
            this.pictureBox4.TabStop = false;
            // 
            // _selectDatabase
            // 
            this._selectDatabase.BackColor = System.Drawing.Color.White;
            this._selectDatabase.Dock = System.Windows.Forms.DockStyle.Right;
            this._selectDatabase.Location = new System.Drawing.Point(164, 0);
            this._selectDatabase.Name = "_selectDatabase";
            this._selectDatabase.Size = new System.Drawing.Size(346, 375);
            this._selectDatabase.TabIndex = 0;
            // 
            // _Page_SelectUser
            // 
            this._Page_SelectUser.BackColor = System.Drawing.Color.White;
            this._Page_SelectUser.Controls.Add(this.pictureBox3);
            this._Page_SelectUser.Controls.Add(this._groupBox_SelectUser);
            this._Page_SelectUser.Description = "A user can be any SQL login or Windows User or Window Group.";
            this._Page_SelectUser.Location = new System.Drawing.Point(19, 73);
            this._Page_SelectUser.Name = "_Page_SelectUser";
            this._Page_SelectUser.NextPage = this._Page_SelectDatabase;
            this._Page_SelectUser.PreviousPage = this._page_SelectSnapshot;
            this._Page_SelectUser.Size = new System.Drawing.Size(510, 375);
            this._Page_SelectUser.TabIndex = 1010;
            this._Page_SelectUser.Text = "Select User";
            this._Page_SelectUser.BeforeMoveNext += new Divelements.WizardFramework.WizardPageEventHandler(this._Page_SelectUser_BeforeMoveNext);
            this._Page_SelectUser.BeforeDisplay += new System.EventHandler(this._Page_SelectUser_BeforeDisplay);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.WizardSteps_3of4;
            this.pictureBox3.Location = new System.Drawing.Point(0, 0);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(164, 375);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 3;
            this.pictureBox3.TabStop = false;
            // 
            // _groupBox_SelectUser
            // 
            this._groupBox_SelectUser.BackColor = System.Drawing.Color.Transparent;
            this._groupBox_SelectUser.CausesValidation = false;
            this._groupBox_SelectUser.Controls.Add(this._button_BrowseUsers);
            this._groupBox_SelectUser.Controls.Add(this._textBox_User);
            this._groupBox_SelectUser.Controls.Add(this._radioButton_SQLLogin);
            this._groupBox_SelectUser.Controls.Add(this._radioButton_WindowsUser);
            this._groupBox_SelectUser.ForeColor = System.Drawing.Color.Navy;
            this._groupBox_SelectUser.Location = new System.Drawing.Point(170, 42);
            this._groupBox_SelectUser.Name = "_groupBox_SelectUser";
            this._groupBox_SelectUser.Size = new System.Drawing.Size(337, 90);
            this._groupBox_SelectUser.TabIndex = 2;
            this._groupBox_SelectUser.TabStop = false;
            this._groupBox_SelectUser.Text = "Select User";
            // 
            // _button_BrowseUsers
            // 
            this._button_BrowseUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._button_BrowseUsers.CausesValidation = false;
            this._button_BrowseUsers.Location = new System.Drawing.Point(256, 49);
            this._button_BrowseUsers.Name = "_button_BrowseUsers";
            this._button_BrowseUsers.Size = new System.Drawing.Size(75, 23);
            this._button_BrowseUsers.TabIndex = 5;
            this._button_BrowseUsers.Text = "Find...";
            this._button_BrowseUsers.UseVisualStyleBackColor = true;
            this._button_BrowseUsers.Click += new System.EventHandler(this._button_BrowseUsers_Click);
            // 
            // _textBox_User
            // 
            this._textBox_User.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._textBox_User.BackColor = System.Drawing.SystemColors.Window;
            this._textBox_User.Location = new System.Drawing.Point(10, 51);
            this._textBox_User.Name = "_textBox_User";
            this._textBox_User.Size = new System.Drawing.Size(240, 20);
            this._textBox_User.TabIndex = 4;
            this._textBox_User.TextChanged += new System.EventHandler(this._textBox_User_TextChanged);
            // 
            // _radioButton_SQLLogin
            // 
            this._radioButton_SQLLogin.AutoSize = true;
            this._radioButton_SQLLogin.CausesValidation = false;
            this._radioButton_SQLLogin.Location = new System.Drawing.Point(167, 22);
            this._radioButton_SQLLogin.Name = "_radioButton_SQLLogin";
            this._radioButton_SQLLogin.Size = new System.Drawing.Size(75, 17);
            this._radioButton_SQLLogin.TabIndex = 3;
            this._radioButton_SQLLogin.Text = "SQL Login";
            this._radioButton_SQLLogin.UseVisualStyleBackColor = true;
            this._radioButton_SQLLogin.CheckedChanged += new System.EventHandler(this._radioButton_SQLLogin_CheckedChanged);
            // 
            // _radioButton_WindowsUser
            // 
            this._radioButton_WindowsUser.AutoSize = true;
            this._radioButton_WindowsUser.CausesValidation = false;
            this._radioButton_WindowsUser.Checked = true;
            this._radioButton_WindowsUser.Location = new System.Drawing.Point(10, 22);
            this._radioButton_WindowsUser.Name = "_radioButton_WindowsUser";
            this._radioButton_WindowsUser.Size = new System.Drawing.Size(138, 17);
            this._radioButton_WindowsUser.TabIndex = 2;
            this._radioButton_WindowsUser.TabStop = true;
            this._radioButton_WindowsUser.Text = "Windows User or Group";
            this._radioButton_WindowsUser.UseVisualStyleBackColor = true;
            this._radioButton_WindowsUser.CheckedChanged += new System.EventHandler(this._radioButton_WindowsUser_CheckedChanged);
            // 
            // _page_SelectSnapshot
            // 
            this._page_SelectSnapshot.Controls.Add(this.pictureBox2);
            this._page_SelectSnapshot.Controls.Add(this._selectSnapshot);
            this._page_SelectSnapshot.Description = "Snapshots represent a point in time capture of all the security collected for a s" +
                "erver. ";
            this._page_SelectSnapshot.Location = new System.Drawing.Point(19, 73);
            this._page_SelectSnapshot.Name = "_page_SelectSnapshot";
            this._page_SelectSnapshot.NextPage = this._Page_SelectUser;
            this._page_SelectSnapshot.PreviousPage = this._page_SelectServer;
            this._page_SelectSnapshot.Size = new System.Drawing.Size(510, 375);
            this._page_SelectSnapshot.TabIndex = 1009;
            this._page_SelectSnapshot.Text = "Select Snapshot";
            this._page_SelectSnapshot.BeforeMoveNext += new Divelements.WizardFramework.WizardPageEventHandler(this._page_SelectSnapshot_BeforeMoveNext);
            this._page_SelectSnapshot.BeforeDisplay += new System.EventHandler(this._page_SelectSnapshot_BeforeDisplay);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.WizardSteps_2of4;
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(164, 375);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            // 
            // _selectSnapshot
            // 
            this._selectSnapshot.BackColor = System.Drawing.Color.Transparent;
            this._selectSnapshot.Dock = System.Windows.Forms.DockStyle.Right;
            this._selectSnapshot.Location = new System.Drawing.Point(164, 0);
            this._selectSnapshot.Name = "_selectSnapshot";
            this._selectSnapshot.Size = new System.Drawing.Size(346, 375);
            this._selectSnapshot.TabIndex = 1;
            // 
            // _page_SelectServer
            // 
            this._page_SelectServer.BackColor = System.Drawing.SystemColors.Control;
            this._page_SelectServer.Controls.Add(this._selectServer);
            this._page_SelectServer.Controls.Add(this.pictureBox1);
            this._page_SelectServer.Description = "The list below contains the SQL Servers you have previously registered.";
            this._page_SelectServer.Location = new System.Drawing.Point(19, 73);
            this._page_SelectServer.Name = "_page_SelectServer";
            this._page_SelectServer.NextPage = this._page_SelectSnapshot;
            this._page_SelectServer.PreviousPage = this._page_Introduction;
            this._page_SelectServer.Size = new System.Drawing.Size(510, 375);
            this._page_SelectServer.TabIndex = 0;
            this._page_SelectServer.Text = "Select SQL Server";
            this._page_SelectServer.BeforeMoveNext += new Divelements.WizardFramework.WizardPageEventHandler(this._page_SelectServerPage_BeforeMoveNext);
            this._page_SelectServer.BeforeDisplay += new System.EventHandler(this._page_SelectServerPage_BeforeDisplay);
            // 
            // _selectServer
            // 
            this._selectServer.BackColor = System.Drawing.Color.Transparent;
            this._selectServer.Dock = System.Windows.Forms.DockStyle.Right;
            this._selectServer.Location = new System.Drawing.Point(164, 0);
            this._selectServer.Name = "_selectServer";
            this._selectServer.Size = new System.Drawing.Size(346, 375);
            this._selectServer.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.WizardSteps_1of4;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(164, 375);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // _page_Introduction
            // 
            this._page_Introduction.Controls.Add(this._rtb_Introduction);
            this._page_Introduction.IntroductionText = "";
            this._page_Introduction.Location = new System.Drawing.Point(177, 66);
            this._page_Introduction.Name = "_page_Introduction";
            this._page_Introduction.NextPage = this._page_SelectServer;
            this._page_Introduction.Size = new System.Drawing.Size(358, 382);
            this._page_Introduction.TabIndex = 1004;
            this._page_Introduction.Text = "Welcome to the SQLsecure Explore User Permissions Wizard";
            // 
            // _rtb_Introduction
            // 
            this._rtb_Introduction.BackColor = System.Drawing.SystemColors.Window;
            this._rtb_Introduction.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._rtb_Introduction.Location = new System.Drawing.Point(3, 0);
            this._rtb_Introduction.Name = "_rtb_Introduction";
            this._rtb_Introduction.ReadOnly = true;
            this._rtb_Introduction.Size = new System.Drawing.Size(352, 362);
            this._rtb_Introduction.TabIndex = 0;
            this._rtb_Introduction.Text = "";
            // 
            // Form_WizardUserPermissions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(548, 508);
            this.Controls.Add(this._wizard);
            this.ForeColor = System.Drawing.Color.Navy;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form_WizardUserPermissions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SQLsecure Explore User Permissions";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.Form_WizardUserPermissions_HelpButtonClicked);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.Form_WizardUserPermissions_HelpRequested);
            this._wizard.ResumeLayout(false);
            this._page_Finish.ResumeLayout(false);
            this._Page_SelectDatabase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this._Page_SelectUser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this._groupBox_SelectUser.ResumeLayout(false);
            this._groupBox_SelectUser.PerformLayout();
            this._page_SelectSnapshot.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this._page_SelectServer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this._page_Introduction.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Divelements.WizardFramework.IntroductionPage _page_Introduction;
        private System.Windows.Forms.RichTextBox _rtb_Introduction;
        private Divelements.WizardFramework.WizardPage _page_SelectServer;
        private Divelements.WizardFramework.WizardPage _page_SelectSnapshot;
        private Divelements.WizardFramework.FinishPage _page_Finish;
        private System.Windows.Forms.RichTextBox _rtb_Finish;
        private Divelements.WizardFramework.Wizard _wizard;
        private Idera.SQLsecure.UI.Console.Controls.SelectServer _selectServer;
        private Divelements.WizardFramework.WizardPage _Page_SelectUser;
        private Divelements.WizardFramework.WizardPage _Page_SelectDatabase;
        private Idera.SQLsecure.UI.Console.Controls.SelectSnapshot _selectSnapshot;
        private System.Windows.Forms.GroupBox _groupBox_SelectUser;
        private System.Windows.Forms.Button _button_BrowseUsers;
        private System.Windows.Forms.TextBox _textBox_User;
        private System.Windows.Forms.RadioButton _radioButton_SQLLogin;
        private System.Windows.Forms.RadioButton _radioButton_WindowsUser;
        private Idera.SQLsecure.UI.Console.Controls.SelectDatabase _selectDatabase;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox2;


    }
}