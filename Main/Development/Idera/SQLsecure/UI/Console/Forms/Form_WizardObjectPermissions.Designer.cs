namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_WizardObjectPermissions
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
            this._page_SelectSnapshot = new Divelements.WizardFramework.WizardPage();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this._page_Finish = new Divelements.WizardFramework.FinishPage();
            this._rtb_Finish = new System.Windows.Forms.RichTextBox();
            this._page_Introduction = new Divelements.WizardFramework.IntroductionPage();
            this._rtb_Introduction = new System.Windows.Forms.RichTextBox();
            this._page_SelectServer = new Divelements.WizardFramework.WizardPage();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this._selectSnapshot = new Idera.SQLsecure.UI.Console.Controls.SelectSnapshot();
            this._selectServer = new Idera.SQLsecure.UI.Console.Controls.SelectServer();
            this._wizard.SuspendLayout();
            this._page_SelectSnapshot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this._page_Finish.SuspendLayout();
            this._page_Introduction.SuspendLayout();
            this._page_SelectServer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // _wizard
            // 
            this._wizard.BannerImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.ObjectPermissions_49x49;
            this._wizard.Controls.Add(this._page_Finish);
            this._wizard.Controls.Add(this._page_SelectSnapshot);
            this._wizard.Controls.Add(this._page_SelectServer);
            this._wizard.Controls.Add(this._page_Introduction);
            this._wizard.Dock = System.Windows.Forms.DockStyle.Fill;
            this._wizard.HelpVisible = true;
            this._wizard.Location = new System.Drawing.Point(0, 0);
            this._wizard.MarginImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Welcome_ObjectPermissions1;
            this._wizard.Name = "_wizard";
            this._wizard.SelectedPage = this._page_Introduction;
            this._wizard.Size = new System.Drawing.Size(548, 508);
            this._wizard.TabIndex = 1;
            // 
            // _page_SelectSnapshot
            // 
            this._page_SelectSnapshot.Controls.Add(this.pictureBox2);
            this._page_SelectSnapshot.Controls.Add(this._selectSnapshot);
            this._page_SelectSnapshot.Description = "Snapshots represent a point in time capture of all the security collected for a s" +
                "erver. ";
            this._page_SelectSnapshot.Location = new System.Drawing.Point(19, 73);
            this._page_SelectSnapshot.Name = "_page_SelectSnapshot";
            this._page_SelectSnapshot.NextPage = this._page_Finish;
            this._page_SelectSnapshot.PreviousPage = this._page_SelectServer;
            this._page_SelectSnapshot.Size = new System.Drawing.Size(510, 375);
            this._page_SelectSnapshot.TabIndex = 1009;
            this._page_SelectSnapshot.Text = "Select Snapshot";
            this._page_SelectSnapshot.BeforeMoveNext += new Divelements.WizardFramework.WizardPageEventHandler(this._page_SelectSnapshot_BeforeMoveNext);
            this._page_SelectSnapshot.BeforeDisplay += new System.EventHandler(this._page_SelectSnapshot_BeforeDisplay);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.WizardSteps_2of2;
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(164, 375);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            // 
            // _page_Finish
            // 
            this._page_Finish.Controls.Add(this._rtb_Finish);
            this._page_Finish.FinishText = "";
            this._page_Finish.Location = new System.Drawing.Point(177, 66);
            this._page_Finish.Name = "_page_Finish";
            this._page_Finish.PreviousPage = this._page_SelectSnapshot;
            this._page_Finish.ProceedText = "To Explore Object Permissions, click Finish";
            this._page_Finish.Size = new System.Drawing.Size(358, 382);
            this._page_Finish.TabIndex = 1006;
            this._page_Finish.Text = "Completing SQLsecure Explore Object Permissions Wizard";
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
            this._rtb_Finish.Size = new System.Drawing.Size(355, 337);
            this._rtb_Finish.TabIndex = 1;
            this._rtb_Finish.Text = "";
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
            this._page_Introduction.Text = "Welcome to the SQLsecure Explore Object Permissions Wizard";
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
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.WizardSteps_1of2;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(164, 375);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
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
            // _selectServer
            // 
            this._selectServer.BackColor = System.Drawing.Color.Transparent;
            this._selectServer.Dock = System.Windows.Forms.DockStyle.Right;
            this._selectServer.Location = new System.Drawing.Point(164, 0);
            this._selectServer.Name = "_selectServer";
            this._selectServer.Size = new System.Drawing.Size(346, 375);
            this._selectServer.TabIndex = 0;
            // 
            // Form_WizardObjectPermissions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(548, 508);
            this.Controls.Add(this._wizard);
            this.ForeColor = System.Drawing.Color.Navy;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form_WizardObjectPermissions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SQLsecure Explorer Object Permissions";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.Form_WizardObjectPermissions_HelpButtonClicked);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.Form_WizardObjectPermissions_HelpRequested);
            this._wizard.ResumeLayout(false);
            this._page_SelectSnapshot.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this._page_Finish.ResumeLayout(false);
            this._page_Introduction.ResumeLayout(false);
            this._page_SelectServer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
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
        private Idera.SQLsecure.UI.Console.Controls.SelectSnapshot _selectSnapshot;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;


    }
}