namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_WizardNewLogin
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
            this._page_Login = new Divelements.WizardFramework.WizardPage();
            this._grpbx_ConnectUsing = new System.Windows.Forms.GroupBox();
            this._static_Desc = new System.Windows.Forms.Label();
            this._rdbtn_DenyAccess = new System.Windows.Forms.RadioButton();
            this._rdbtn_GrantAccess = new System.Windows.Forms.RadioButton();
            this._edit_login_Name = new System.Windows.Forms.TextBox();
            this._lbl_Server = new System.Windows.Forms.Label();
            this._page_Credentials = new Divelements.WizardFramework.WizardPage();
            this._rdbtn_No = new System.Windows.Forms.RadioButton();
            this._rdbtn_Yes = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this._page_Finish = new Divelements.WizardFramework.FinishPage();
            this._rtb_Finish = new System.Windows.Forms.RichTextBox();
            this._page_Introduction = new Divelements.WizardFramework.IntroductionPage();
            this._rtb_Introduction = new System.Windows.Forms.RichTextBox();
            this._wizard.SuspendLayout();
            this._page_Login.SuspendLayout();
            this._grpbx_ConnectUsing.SuspendLayout();
            this._page_Credentials.SuspendLayout();
            this._page_Finish.SuspendLayout();
            this._page_Introduction.SuspendLayout();
            this.SuspendLayout();
            // 
            // _wizard
            // 
            this._wizard.BannerImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.SQLsecure_Users_49;
            this._wizard.Controls.Add(this._page_Finish);
            this._wizard.Controls.Add(this._page_Login);
            this._wizard.Controls.Add(this._page_Introduction);
            this._wizard.Controls.Add(this._page_Credentials);
            this._wizard.HelpVisible = true;
            this._wizard.Location = new System.Drawing.Point(0, 0);
            this._wizard.MarginImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.SQLServerLogin_Insert;
            this._wizard.Name = "_wizard";
            this._wizard.SelectedPage = this._page_Finish;
            this._wizard.Size = new System.Drawing.Size(501, 356);
            this._wizard.TabIndex = 1;
            this._wizard.HelpRequested += new System.Windows.Forms.HelpEventHandler(this._wizard_HelpRequested);
            // 
            // _page_Login
            // 
            this._page_Login.Controls.Add(this._grpbx_ConnectUsing);
            this._page_Login.Controls.Add(this._edit_login_Name);
            this._page_Login.Controls.Add(this._lbl_Server);
            this._page_Login.Description = "Specify a Windows user account name that will be given access to SQLsecure.";
            this._page_Login.Location = new System.Drawing.Point(19, 73);
            this._page_Login.Name = "_page_Login";
            this._page_Login.NextPage = this._page_Credentials;
            this._page_Login.PreviousPage = this._page_Introduction;
            this._page_Login.Size = new System.Drawing.Size(463, 223);
            this._page_Login.TabIndex = 0;
            this._page_Login.Text = "Add a new SQL Server login";
            this._page_Login.BeforeMoveNext += new Divelements.WizardFramework.WizardPageEventHandler(this._page_Login_BeforeMoveNext);
            this._page_Login.BeforeDisplay += new System.EventHandler(this._page_Login_BeforeDisplay);
            // 
            // _grpbx_ConnectUsing
            // 
            this._grpbx_ConnectUsing.Controls.Add(this._static_Desc);
            this._grpbx_ConnectUsing.Controls.Add(this._rdbtn_DenyAccess);
            this._grpbx_ConnectUsing.Controls.Add(this._rdbtn_GrantAccess);
            this._grpbx_ConnectUsing.Location = new System.Drawing.Point(24, 57);
            this._grpbx_ConnectUsing.Name = "_grpbx_ConnectUsing";
            this._grpbx_ConnectUsing.Size = new System.Drawing.Size(416, 163);
            this._grpbx_ConnectUsing.TabIndex = 2;
            this._grpbx_ConnectUsing.TabStop = false;
            // 
            // _static_Desc
            // 
            this._static_Desc.Location = new System.Drawing.Point(16, 21);
            this._static_Desc.Name = "_static_Desc";
            this._static_Desc.Size = new System.Drawing.Size(387, 27);
            this._static_Desc.TabIndex = 8;
            this._static_Desc.Text = "Do you want to grant this user access to SQLsecure?";
            // 
            // _rdbtn_DenyAccess
            // 
            this._rdbtn_DenyAccess.AutoSize = true;
            this._rdbtn_DenyAccess.Location = new System.Drawing.Point(16, 74);
            this._rdbtn_DenyAccess.Name = "_rdbtn_DenyAccess";
            this._rdbtn_DenyAccess.Size = new System.Drawing.Size(87, 17);
            this._rdbtn_DenyAccess.TabIndex = 1;
            this._rdbtn_DenyAccess.Text = "&Deny access";
            this._rdbtn_DenyAccess.UseVisualStyleBackColor = true;
            this._rdbtn_DenyAccess.CheckedChanged += new System.EventHandler(this._rdbtn_DenyAccess_CheckedChanged);
            // 
            // _rdbtn_GrantAccess
            // 
            this._rdbtn_GrantAccess.AutoSize = true;
            this._rdbtn_GrantAccess.Location = new System.Drawing.Point(16, 51);
            this._rdbtn_GrantAccess.Name = "_rdbtn_GrantAccess";
            this._rdbtn_GrantAccess.Size = new System.Drawing.Size(88, 17);
            this._rdbtn_GrantAccess.TabIndex = 0;
            this._rdbtn_GrantAccess.Text = "&Grant access";
            this._rdbtn_GrantAccess.UseVisualStyleBackColor = true;
            // 
            // _edit_login_Name
            // 
            this._edit_login_Name.Location = new System.Drawing.Point(77, 11);
            this._edit_login_Name.Name = "_edit_login_Name";
            this._edit_login_Name.Size = new System.Drawing.Size(325, 20);
            this._edit_login_Name.TabIndex = 1;
            this._edit_login_Name.TextChanged += new System.EventHandler(this._edit_login_Name_TextChanged);
            // 
            // _lbl_Server
            // 
            this._lbl_Server.AutoSize = true;
            this._lbl_Server.Location = new System.Drawing.Point(30, 15);
            this._lbl_Server.Name = "_lbl_Server";
            this._lbl_Server.Size = new System.Drawing.Size(35, 13);
            this._lbl_Server.TabIndex = 0;
            this._lbl_Server.Text = "Name";
            // 
            // _page_Credentials
            // 
            this._page_Credentials.Controls.Add(this._rdbtn_No);
            this._page_Credentials.Controls.Add(this._rdbtn_Yes);
            this._page_Credentials.Controls.Add(this.label2);
            this._page_Credentials.Controls.Add(this.label1);
            this._page_Credentials.Description = "Specify the permission level of the new user.";
            this._page_Credentials.Location = new System.Drawing.Point(19, 73);
            this._page_Credentials.Name = "_page_Credentials";
            this._page_Credentials.NextPage = this._page_Finish;
            this._page_Credentials.PreviousPage = this._page_Login;
            this._page_Credentials.Size = new System.Drawing.Size(463, 223);
            this._page_Credentials.TabIndex = 1009;
            this._page_Credentials.Text = "Set the SQLsecure permission level";
            this._page_Credentials.BeforeDisplay += new System.EventHandler(this._page_Credentials_BeforeDisplay);
            // 
            // _rdbtn_No
            // 
            this._rdbtn_No.Location = new System.Drawing.Point(35, 128);
            this._rdbtn_No.Name = "_rdbtn_No";
            this._rdbtn_No.Size = new System.Drawing.Size(328, 24);
            this._rdbtn_No.TabIndex = 12;
            this._rdbtn_No.Text = "&No, only allow this user the ability to view audit data.";
            this._rdbtn_No.UseVisualStyleBackColor = true;
            // 
            // _rdbtn_Yes
            // 
            this._rdbtn_Yes.Checked = true;
            this._rdbtn_Yes.Location = new System.Drawing.Point(35, 101);
            this._rdbtn_Yes.Name = "_rdbtn_Yes";
            this._rdbtn_Yes.Size = new System.Drawing.Size(328, 24);
            this._rdbtn_Yes.TabIndex = 11;
            this._rdbtn_Yes.TabStop = true;
            this._rdbtn_Yes.Text = "&Yes, grant this user permission to configure SQLsecure.";
            this._rdbtn_Yes.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(27, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(387, 24);
            this.label2.TabIndex = 10;
            this.label2.Text = "Do you want to add this login to the sysadmin server role?";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(27, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(387, 49);
            this.label1.TabIndex = 9;
            this.label1.Text = "To grant permissions to configure audit settings, the login will be added to the " +
                "sysadmin role on the SQL Server that hosts the Repository.";
            // 
            // _page_Finish
            // 
            this._page_Finish.Controls.Add(this._rtb_Finish);
            this._page_Finish.FinishText = "";
            this._page_Finish.Location = new System.Drawing.Point(177, 66);
            this._page_Finish.Name = "_page_Finish";
            this._page_Finish.PreviousPage = this._page_Credentials;
            this._page_Finish.ProceedText = "To create this login, click Finish";
            this._page_Finish.Size = new System.Drawing.Size(311, 230);
            this._page_Finish.TabIndex = 1006;
            this._page_Finish.Text = "Completing SQLsecure New Login Wizard";
            this._page_Finish.CollectSettings += new Divelements.WizardFramework.WizardFinishPageEventHandler(this._page_Finish_CollectSettings);
            this._page_Finish.BeforeMoveNext += new Divelements.WizardFramework.WizardPageEventHandler(this._page_Finish_BeforeMoveNext);
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
            this._rtb_Finish.Size = new System.Drawing.Size(308, 206);
            this._rtb_Finish.TabIndex = 1;
            this._rtb_Finish.Text = "";
            // 
            // _page_Introduction
            // 
            this._page_Introduction.Controls.Add(this._rtb_Introduction);
            this._page_Introduction.IntroductionText = "";
            this._page_Introduction.Location = new System.Drawing.Point(177, 66);
            this._page_Introduction.Name = "_page_Introduction";
            this._page_Introduction.NextPage = this._page_Login;
            this._page_Introduction.Size = new System.Drawing.Size(311, 230);
            this._page_Introduction.TabIndex = 1004;
            this._page_Introduction.Text = "Welcome to the SQLsecure New Login Wizard";
            // 
            // _rtb_Introduction
            // 
            this._rtb_Introduction.BackColor = System.Drawing.SystemColors.Window;
            this._rtb_Introduction.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._rtb_Introduction.Location = new System.Drawing.Point(0, 0);
            this._rtb_Introduction.Name = "_rtb_Introduction";
            this._rtb_Introduction.ReadOnly = true;
            this._rtb_Introduction.Size = new System.Drawing.Size(311, 211);
            this._rtb_Introduction.TabIndex = 0;
            this._rtb_Introduction.Text = "";
            // 
            // Form_WizardNewLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(501, 356);
            this.Controls.Add(this._wizard);
            this.ForeColor = System.Drawing.Color.Navy;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form_WizardNewLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SQLsecure New Login";
            this._wizard.ResumeLayout(false);
            this._page_Login.ResumeLayout(false);
            this._page_Login.PerformLayout();
            this._grpbx_ConnectUsing.ResumeLayout(false);
            this._grpbx_ConnectUsing.PerformLayout();
            this._page_Credentials.ResumeLayout(false);
            this._page_Finish.ResumeLayout(false);
            this._page_Introduction.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Divelements.WizardFramework.IntroductionPage _page_Introduction;
        private System.Windows.Forms.RichTextBox _rtb_Introduction;
        private Divelements.WizardFramework.WizardPage _page_Login;
        private System.Windows.Forms.GroupBox _grpbx_ConnectUsing;
        private System.Windows.Forms.Label _static_Desc;
        private System.Windows.Forms.RadioButton _rdbtn_DenyAccess;
        private System.Windows.Forms.RadioButton _rdbtn_GrantAccess;
        private System.Windows.Forms.TextBox _edit_login_Name;
        private System.Windows.Forms.Label _lbl_Server;
        private Divelements.WizardFramework.WizardPage _page_Credentials;
        private Divelements.WizardFramework.FinishPage _page_Finish;
        private System.Windows.Forms.RichTextBox _rtb_Finish;
        private Divelements.WizardFramework.Wizard _wizard;
        private System.Windows.Forms.RadioButton _rdbtn_Yes;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton _rdbtn_No;


    }
}