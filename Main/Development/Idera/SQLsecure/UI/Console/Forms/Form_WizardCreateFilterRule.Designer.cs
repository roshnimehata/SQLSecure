namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_WizardCreateFilterRule
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
            this._page_Introduction = new Divelements.WizardFramework.IntroductionPage();
            this._rtbx_Introduction = new System.Windows.Forms.RichTextBox();
            this._page_NameAndDescription = new Divelements.WizardFramework.WizardPage();
            this._txtbx_Description = new System.Windows.Forms.TextBox();
            this._txtbx_Name = new System.Windows.Forms.TextBox();
            this._lbl_Description = new System.Windows.Forms.Label();
            this._lbl_Name = new System.Windows.Forms.Label();
            this._page_DbLevel = new Divelements.WizardFramework.WizardPage();
            this.filterSelection1 = new Idera.SQLsecure.UI.Console.Controls.FilterSelection();
            this._page_Finish = new Divelements.WizardFramework.FinishPage();
            this._rtbx_FinishSummary = new System.Windows.Forms.RichTextBox();
            this._wizard.SuspendLayout();
            this._page_Introduction.SuspendLayout();
            this._page_NameAndDescription.SuspendLayout();
            this._page_DbLevel.SuspendLayout();
            this._page_Finish.SuspendLayout();
            this.SuspendLayout();
            // 
            // _wizard
            // 
            this._wizard.BannerImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.filter_add_49;
            this._wizard.Controls.Add(this._page_NameAndDescription);
            this._wizard.Controls.Add(this._page_Introduction);
            this._wizard.Controls.Add(this._page_DbLevel);
            this._wizard.Controls.Add(this._page_Finish);
            this._wizard.HelpVisible = true;
            this._wizard.Location = new System.Drawing.Point(0, 0);
            this._wizard.MarginImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.AddFilter_Insert;
            this._wizard.Name = "_wizard";
            this._wizard.SelectedPage = this._page_Finish;
            this._wizard.Size = new System.Drawing.Size(514, 536);
            this._wizard.TabIndex = 0;
            // 
            // _page_Introduction
            // 
            this._page_Introduction.Controls.Add(this._rtbx_Introduction);
            this._page_Introduction.IntroductionText = "";
            this._page_Introduction.Location = new System.Drawing.Point(177, 66);
            this._page_Introduction.Name = "_page_Introduction";
            this._page_Introduction.NextPage = this._page_NameAndDescription;
            this._page_Introduction.Size = new System.Drawing.Size(324, 410);
            this._page_Introduction.TabIndex = 1004;
            this._page_Introduction.Text = "Welcome to the Add Filter Wizard";
            // 
            // _rtbx_Introduction
            // 
            this._rtbx_Introduction.BackColor = System.Drawing.SystemColors.Window;
            this._rtbx_Introduction.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._rtbx_Introduction.Dock = System.Windows.Forms.DockStyle.Top;
            this._rtbx_Introduction.Location = new System.Drawing.Point(0, 0);
            this._rtbx_Introduction.Name = "_rtbx_Introduction";
            this._rtbx_Introduction.ReadOnly = true;
            this._rtbx_Introduction.Size = new System.Drawing.Size(324, 204);
            this._rtbx_Introduction.TabIndex = 1;
            this._rtbx_Introduction.Text = "";
            // 
            // _page_NameAndDescription
            // 
            this._page_NameAndDescription.Controls.Add(this._txtbx_Description);
            this._page_NameAndDescription.Controls.Add(this._txtbx_Name);
            this._page_NameAndDescription.Controls.Add(this._lbl_Description);
            this._page_NameAndDescription.Controls.Add(this._lbl_Name);
            this._page_NameAndDescription.Description = "Specify the filter name and description.";
            this._page_NameAndDescription.Location = new System.Drawing.Point(19, 73);
            this._page_NameAndDescription.Name = "_page_NameAndDescription";
            this._page_NameAndDescription.NextPage = this._page_DbLevel;
            this._page_NameAndDescription.PreviousPage = this._page_Introduction;
            this._page_NameAndDescription.Size = new System.Drawing.Size(476, 403);
            this._page_NameAndDescription.TabIndex = 1007;
            this._page_NameAndDescription.Text = "Specify name and description";
            this._page_NameAndDescription.BeforeMoveNext += new Divelements.WizardFramework.WizardPageEventHandler(this._page_NameAndDescription_BeforeMoveNext);
            this._page_NameAndDescription.BeforeDisplay += new System.EventHandler(this._page_NameAndDescription_BeforeDisplay);
            // 
            // _txtbx_Description
            // 
            this._txtbx_Description.Location = new System.Drawing.Point(96, 29);
            this._txtbx_Description.Name = "_txtbx_Description";
            this._txtbx_Description.Size = new System.Drawing.Size(338, 20);
            this._txtbx_Description.TabIndex = 3;
            // 
            // _txtbx_Name
            // 
            this._txtbx_Name.Location = new System.Drawing.Point(96, 3);
            this._txtbx_Name.Name = "_txtbx_Name";
            this._txtbx_Name.Size = new System.Drawing.Size(338, 20);
            this._txtbx_Name.TabIndex = 1;
            this._txtbx_Name.TextChanged += new System.EventHandler(this._txtbx_Name_TextChanged);
            // 
            // _lbl_Description
            // 
            this._lbl_Description.AutoSize = true;
            this._lbl_Description.Location = new System.Drawing.Point(27, 32);
            this._lbl_Description.Name = "_lbl_Description";
            this._lbl_Description.Size = new System.Drawing.Size(63, 13);
            this._lbl_Description.TabIndex = 2;
            this._lbl_Description.Text = "&Description:";
            // 
            // _lbl_Name
            // 
            this._lbl_Name.AutoSize = true;
            this._lbl_Name.Location = new System.Drawing.Point(27, 6);
            this._lbl_Name.Name = "_lbl_Name";
            this._lbl_Name.Size = new System.Drawing.Size(38, 13);
            this._lbl_Name.TabIndex = 0;
            this._lbl_Name.Text = "Na&me:";
            // 
            // _page_DbLevel
            // 
            this._page_DbLevel.Controls.Add(this.filterSelection1);
            this._page_DbLevel.Description = "Specify databases and database objects for collecting permissions data.";
            this._page_DbLevel.Location = new System.Drawing.Point(19, 73);
            this._page_DbLevel.Name = "_page_DbLevel";
            this._page_DbLevel.NextPage = this._page_Finish;
            this._page_DbLevel.PreviousPage = this._page_NameAndDescription;
            this._page_DbLevel.Size = new System.Drawing.Size(476, 403);
            this._page_DbLevel.TabIndex = 0;
            this._page_DbLevel.Text = "Specify databases and database objects";
            this._page_DbLevel.BeforeMoveNext += new Divelements.WizardFramework.WizardPageEventHandler(this._page_DbLevel_BeforeMoveNext);
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
            // _page_Finish
            // 
            this._page_Finish.Controls.Add(this._rtbx_FinishSummary);
            this._page_Finish.FinishText = "";
            this._page_Finish.Location = new System.Drawing.Point(177, 66);
            this._page_Finish.Name = "_page_Finish";
            this._page_Finish.PreviousPage = this._page_NameAndDescription;
            this._page_Finish.ProceedText = "To create this filter, click Finish.";
            this._page_Finish.Size = new System.Drawing.Size(324, 410);
            this._page_Finish.TabIndex = 1006;
            this._page_Finish.Text = "Completing the Add Filter Wizard";
            this._page_Finish.BeforeDisplay += new System.EventHandler(this._page_Finish_BeforeDisplay);
            // 
            // _rtbx_FinishSummary
            // 
            this._rtbx_FinishSummary.BackColor = System.Drawing.SystemColors.Window;
            this._rtbx_FinishSummary.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._rtbx_FinishSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this._rtbx_FinishSummary.Location = new System.Drawing.Point(0, 0);
            this._rtbx_FinishSummary.Name = "_rtbx_FinishSummary";
            this._rtbx_FinishSummary.ReadOnly = true;
            this._rtbx_FinishSummary.Size = new System.Drawing.Size(324, 204);
            this._rtbx_FinishSummary.TabIndex = 0;
            this._rtbx_FinishSummary.TabStop = false;
            this._rtbx_FinishSummary.Text = "";
            // 
            // Form_WizardCreateFilterRule
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
            this.Name = "Form_WizardCreateFilterRule";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Filter";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.Form_WizardCreateFilterRule_HelpButtonClicked);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.Form_WizardCreateFilterRule_HelpRequested);
            this.Load += new System.EventHandler(this.Form_WizardCreateFilterRule_Load);
            this._wizard.ResumeLayout(false);
            this._page_Introduction.ResumeLayout(false);
            this._page_NameAndDescription.ResumeLayout(false);
            this._page_NameAndDescription.PerformLayout();
            this._page_DbLevel.ResumeLayout(false);
            this._page_DbLevel.PerformLayout();
            this._page_Finish.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Divelements.WizardFramework.Wizard _wizard;
        private Divelements.WizardFramework.IntroductionPage _page_Introduction;
        private Divelements.WizardFramework.WizardPage _page_DbLevel;
        private Divelements.WizardFramework.FinishPage _page_Finish;
        private System.Windows.Forms.RichTextBox _rtbx_Introduction;
        private Divelements.WizardFramework.WizardPage _page_NameAndDescription;
        private System.Windows.Forms.TextBox _txtbx_Description;
        private System.Windows.Forms.TextBox _txtbx_Name;
        private System.Windows.Forms.Label _lbl_Description;
        private System.Windows.Forms.Label _lbl_Name;
        private System.Windows.Forms.RichTextBox _rtbx_FinishSummary;
        private Idera.SQLsecure.UI.Console.Controls.FilterSelection filterSelection1;
    }
}