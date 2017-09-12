namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_WizardCreatePolicy
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
            Infragistics.Win.UltraWinListView.UltraListViewSubItemColumn ultraListViewSubItemColumn1 = new Infragistics.Win.UltraWinListView.UltraListViewSubItemColumn("CheckState");
            Infragistics.Win.UltraWinListView.UltraListViewSubItemColumn ultraListViewSubItemColumn2 = new Infragistics.Win.UltraWinListView.UltraListViewSubItemColumn("Count");
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this._wizard = new Divelements.WizardFramework.Wizard();
            this._page_Introduction = new Divelements.WizardFramework.IntroductionPage();
            this._rtb_Introduction = new System.Windows.Forms.RichTextBox();
            this._page_PolicyCreateType = new Divelements.WizardFramework.WizardPage();
            this._linkLabel_HelpTemplates = new System.Windows.Forms.LinkLabel();
            this._groupBox_PolicyName = new System.Windows.Forms.GroupBox();
            this._label_Description = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._ultraListViewStandardPolicies = new Infragistics.Win.UltraWinListView.UltraListView();
            this.radioButtonCreateFromStandard = new System.Windows.Forms.RadioButton();
            this.radioButtonCreateNew = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this._page_PolicyName = new Divelements.WizardFramework.WizardPage();
            this.textBox_Description = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_PolicyName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this._page_Vulnerabilities = new Divelements.WizardFramework.WizardPage();

            // SQLsecure 3.1 (Anshul Aggarwal) - Use the same control across application.
            this.controlConfigurePolicyVulnerabilities1 = new Controls.controlConfigurePolicyVulnerabilities(Utility.ConfigurePolicyControlType.CreatePolicySecurityCheck);

            this._page_SQLServers = new Divelements.WizardFramework.WizardPage();
            this.controlPolicyAddServers1 = new Idera.SQLsecure.UI.Console.Controls.ControlPolicyAddServers();
            this._page_Interview = new Divelements.WizardFramework.WizardPage();
            this._policyInterview = new Idera.SQLsecure.UI.Console.Controls.PolicyInterview();
            this._page_Finish = new Divelements.WizardFramework.FinishPage();
            this._rtb_Finish = new System.Windows.Forms.RichTextBox();
            this._wizard.SuspendLayout();
            this._page_Introduction.SuspendLayout();
            this._page_PolicyCreateType.SuspendLayout();
            this._groupBox_PolicyName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._ultraListViewStandardPolicies)).BeginInit();
            this._page_PolicyName.SuspendLayout();
            this._page_Vulnerabilities.SuspendLayout();
            this._page_SQLServers.SuspendLayout();
            this._page_Interview.SuspendLayout();
            this._page_Finish.SuspendLayout();
            this.SuspendLayout();
            // 
            // _wizard
            // 
            this._wizard.BannerImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.add_policy_49;
            this._wizard.Controls.Add(this._page_PolicyCreateType);
            this._wizard.Controls.Add(this._page_Introduction);
            this._wizard.Controls.Add(this._page_Vulnerabilities);
            this._wizard.Controls.Add(this._page_PolicyName);
            this._wizard.Controls.Add(this._page_Interview);
            this._wizard.Controls.Add(this._page_Finish);
            this._wizard.Controls.Add(this._page_SQLServers);
            this._wizard.Dock = System.Windows.Forms.DockStyle.Fill;
            this._wizard.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._wizard.ForeColor = System.Drawing.Color.Navy;
            this._wizard.HelpVisible = true;
            this._wizard.Location = new System.Drawing.Point(0, 0);
            this._wizard.MarginImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.AddPolicy;
            this._wizard.Name = "_wizard";
            this._wizard.SelectedPage = this._page_PolicyCreateType;
            this._wizard.Size = new System.Drawing.Size(786, 573);
            this._wizard.TabIndex = 2;
            this._wizard.Finish += new System.EventHandler(this._wizard_Finish);
            this._wizard.HelpRequested += new System.Windows.Forms.HelpEventHandler(this._wizard_HelpRequested);
            // 
            // _page_Introduction
            // 
            this._page_Introduction.Controls.Add(this._rtb_Introduction);
            this._page_Introduction.IntroductionText = "";
            this._page_Introduction.Location = new System.Drawing.Point(177, 66);
            this._page_Introduction.Name = "_page_Introduction";
            this._page_Introduction.NextPage = this._page_PolicyCreateType;
            this._page_Introduction.Size = new System.Drawing.Size(596, 447);
            this._page_Introduction.TabIndex = 1004;
            this._page_Introduction.Text = "Welcome to the SQLsecure New Policy Wizard";
            this._page_Introduction.BeforeDisplay += new System.EventHandler(this._page_Introduction_BeforeDisplay);
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
            // _page_PolicyCreateType
            // 
            this._page_PolicyCreateType.Controls.Add(this._linkLabel_HelpTemplates);
            this._page_PolicyCreateType.Controls.Add(this._groupBox_PolicyName);
            this._page_PolicyCreateType.Controls.Add(this.label3);
            this._page_PolicyCreateType.Controls.Add(this._ultraListViewStandardPolicies);
            this._page_PolicyCreateType.Controls.Add(this.radioButtonCreateFromStandard);
            this._page_PolicyCreateType.Controls.Add(this.radioButtonCreateNew);
            this._page_PolicyCreateType.Controls.Add(this.label2);
            this._page_PolicyCreateType.Description = "Create a new policy or select a policy template.";
            this._page_PolicyCreateType.DescriptionColor = System.Drawing.Color.Navy;
            this._page_PolicyCreateType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._page_PolicyCreateType.Location = new System.Drawing.Point(19, 73);
            this._page_PolicyCreateType.Name = "_page_PolicyCreateType";
            this._page_PolicyCreateType.NextPage = this._page_PolicyName;
            this._page_PolicyCreateType.PreviousPage = this._page_Introduction;
            this._page_PolicyCreateType.Size = new System.Drawing.Size(748, 440);
            this._page_PolicyCreateType.TabIndex = 0;
            this._page_PolicyCreateType.Text = "Select the Policy Template";
            this._page_PolicyCreateType.TextColor = System.Drawing.Color.Navy;
            this._page_PolicyCreateType.BeforeMoveNext += new Divelements.WizardFramework.WizardPageEventHandler(this._page_PolicyType_BeforeMoveNext);
            // 
            // _linkLabel_HelpTemplates
            // 
            this._linkLabel_HelpTemplates.AutoSize = true;
            this._linkLabel_HelpTemplates.Location = new System.Drawing.Point(202, 126);
            this._linkLabel_HelpTemplates.Name = "_linkLabel_HelpTemplates";
            this._linkLabel_HelpTemplates.Size = new System.Drawing.Size(70, 13);
            this._linkLabel_HelpTemplates.TabIndex = 11;
            this._linkLabel_HelpTemplates.TabStop = true;
            this._linkLabel_HelpTemplates.Text = "Tell me more.";
            this._linkLabel_HelpTemplates.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._linkLabel_HelpTemplates_LinkClicked);
            // 
            // _groupBox_PolicyName
            // 
            this._groupBox_PolicyName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._groupBox_PolicyName.BackColor = System.Drawing.Color.Transparent;
            this._groupBox_PolicyName.Controls.Add(this._label_Description);
            this._groupBox_PolicyName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._groupBox_PolicyName.Location = new System.Drawing.Point(484, 160);
            this._groupBox_PolicyName.Name = "_groupBox_PolicyName";
            this._groupBox_PolicyName.Size = new System.Drawing.Size(261, 229);
            this._groupBox_PolicyName.TabIndex = 10;
            this._groupBox_PolicyName.TabStop = false;
            this._groupBox_PolicyName.Text = "Policy Name";
            // 
            // _label_Description
            // 
            this._label_Description.BackColor = System.Drawing.Color.Transparent;
            this._label_Description.Dock = System.Windows.Forms.DockStyle.Fill;
            this._label_Description.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._label_Description.Location = new System.Drawing.Point(3, 16);
            this._label_Description.Name = "_label_Description";
            this._label_Description.Size = new System.Drawing.Size(255, 210);
            this._label_Description.TabIndex = 3;
            this._label_Description.Text = "Description";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(50, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(470, 36);
            this.label3.TabIndex = 9;
            this.label3.Text = "Creating a new policy lets you select which security checks you want to perform o" +
                "n specific SQL Servers in your enterprise.";
            // 
            // _ultraListViewStandardPolicies
            // 
            this._ultraListViewStandardPolicies.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this._ultraListViewStandardPolicies.ItemSettings.AllowEdit = Infragistics.Win.DefaultableBoolean.False;
            this._ultraListViewStandardPolicies.Location = new System.Drawing.Point(53, 164);
            this._ultraListViewStandardPolicies.MainColumn.Key = "Policy";
            this._ultraListViewStandardPolicies.MainColumn.Text = "Select Template";
            this._ultraListViewStandardPolicies.MainColumn.VisiblePositionInDetailsView = 1;
            this._ultraListViewStandardPolicies.MainColumn.Width = 352;
            this._ultraListViewStandardPolicies.Name = "_ultraListViewStandardPolicies";
            this._ultraListViewStandardPolicies.Size = new System.Drawing.Size(425, 225);
            ultraListViewSubItemColumn1.Key = "CheckState";
            ultraListViewSubItemColumn1.Text = " ";
            ultraListViewSubItemColumn1.VisiblePositionInDetailsView = 0;
            ultraListViewSubItemColumn1.Width = 20;
            ultraListViewSubItemColumn2.Key = "Count";
            appearance1.TextHAlignAsString = "Right";
            ultraListViewSubItemColumn2.SubItemAppearance = appearance1;
            ultraListViewSubItemColumn2.Text = "Checks";
            ultraListViewSubItemColumn2.Width = 48;
            this._ultraListViewStandardPolicies.SubItemColumns.AddRange(new Infragistics.Win.UltraWinListView.UltraListViewSubItemColumn[] {
            ultraListViewSubItemColumn1,
            ultraListViewSubItemColumn2});
            this._ultraListViewStandardPolicies.TabIndex = 4;
            this._ultraListViewStandardPolicies.Text = "ultraListView1";
            this._ultraListViewStandardPolicies.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.Details;
            this._ultraListViewStandardPolicies.ViewSettingsDetails.FullRowSelect = true;
            this._ultraListViewStandardPolicies.MouseMove += new System.Windows.Forms.MouseEventHandler(this._ultraListViewStandardPolicies_MouseMove);
            this._ultraListViewStandardPolicies.MouseDown += new System.Windows.Forms.MouseEventHandler(this._ultraListViewStandardPolicies_MouseDown);
            // 
            // radioButtonCreateFromStandard
            // 
            this.radioButtonCreateFromStandard.AutoSize = true;
            this.radioButtonCreateFromStandard.Location = new System.Drawing.Point(33, 89);
            this.radioButtonCreateFromStandard.Name = "radioButtonCreateFromStandard";
            this.radioButtonCreateFromStandard.Size = new System.Drawing.Size(155, 17);
            this.radioButtonCreateFromStandard.TabIndex = 3;
            this.radioButtonCreateFromStandard.Text = "Use existing policy template";
            this.radioButtonCreateFromStandard.UseVisualStyleBackColor = true;
            this.radioButtonCreateFromStandard.CheckedChanged += new System.EventHandler(this.radioButtonCreateFromStandard_CheckedChanged);
            // 
            // radioButtonCreateNew
            // 
            this.radioButtonCreateNew.AutoSize = true;
            this.radioButtonCreateNew.Checked = true;
            this.radioButtonCreateNew.Location = new System.Drawing.Point(33, 10);
            this.radioButtonCreateNew.Name = "radioButtonCreateNew";
            this.radioButtonCreateNew.Size = new System.Drawing.Size(109, 17);
            this.radioButtonCreateNew.TabIndex = 2;
            this.radioButtonCreateNew.TabStop = true;
            this.radioButtonCreateNew.Text = "Create new policy";
            this.radioButtonCreateNew.UseVisualStyleBackColor = true;
            this.radioButtonCreateNew.CheckedChanged += new System.EventHandler(this.radioButtonCreateNew_CheckedChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(50, 113);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(470, 48);
            this.label2.TabIndex = 8;
            this.label2.Text = "Using a policy template lets you apply consistent, pre-configured security checks" +
                " to multiple SQL Servers across your enterprise.";
            // 
            // _page_PolicyName
            // 
            this._page_PolicyName.Controls.Add(this.textBox_Description);
            this._page_PolicyName.Controls.Add(this.label1);
            this._page_PolicyName.Controls.Add(this.textBox_PolicyName);
            this._page_PolicyName.Controls.Add(this.label4);
            this._page_PolicyName.Description = "Specify a name and description for this new policy.";
            this._page_PolicyName.DescriptionColor = System.Drawing.Color.Navy;
            this._page_PolicyName.Location = new System.Drawing.Point(19, 73);
            this._page_PolicyName.Name = "_page_PolicyName";
            this._page_PolicyName.NextPage = this._page_Vulnerabilities;
            this._page_PolicyName.PreviousPage = this._page_PolicyCreateType;
            this._page_PolicyName.Size = new System.Drawing.Size(748, 440);
            this._page_PolicyName.TabIndex = 1011;
            this._page_PolicyName.Text = "Name the Policy";
            this._page_PolicyName.TextColor = System.Drawing.Color.Navy;
            this._page_PolicyName.BeforeMoveNext += new Divelements.WizardFramework.WizardPageEventHandler(this._page_PolicyName_BeforeMoveNext);
            this._page_PolicyName.BeforeDisplay += new System.EventHandler(this._page_PolicyName_BeforeDisplay);
            // 
            // textBox_Description
            // 
            this.textBox_Description.AcceptsReturn = true;
            this.textBox_Description.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Description.Location = new System.Drawing.Point(94, 60);
            this.textBox_Description.Multiline = true;
            this.textBox_Description.Name = "textBox_Description";
            this.textBox_Description.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_Description.Size = new System.Drawing.Size(613, 189);
            this.textBox_Description.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Description";
            // 
            // textBox_PolicyName
            // 
            this.textBox_PolicyName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_PolicyName.Location = new System.Drawing.Point(94, 14);
            this.textBox_PolicyName.Name = "textBox_PolicyName";
            this.textBox_PolicyName.Size = new System.Drawing.Size(613, 20);
            this.textBox_PolicyName.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Name";
            // 
            // _page_Vulnerabilities
            // 
            this._page_Vulnerabilities.Controls.Add(this.controlConfigurePolicyVulnerabilities1);
            this._page_Vulnerabilities.Description = "Specify which security checks you want this policy to perform.";
            this._page_Vulnerabilities.DescriptionColor = System.Drawing.Color.Navy;
            this._page_Vulnerabilities.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._page_Vulnerabilities.Location = new System.Drawing.Point(19, 73);
            this._page_Vulnerabilities.Name = "_page_Vulnerabilities";
            this._page_Vulnerabilities.NextPage = this._page_SQLServers;
            this._page_Vulnerabilities.PreviousPage = this._page_PolicyName;
            this._page_Vulnerabilities.Size = new System.Drawing.Size(748, 440);
            this._page_Vulnerabilities.TabIndex = 1009;
            this._page_Vulnerabilities.Text = "Configure the Policy";
            this._page_Vulnerabilities.TextColor = System.Drawing.Color.Navy;
            this._page_Vulnerabilities.BeforeMoveNext += new Divelements.WizardFramework.WizardPageEventHandler(this._page_Vulnerabilities_BeforeMoveNext);
            this._page_Vulnerabilities.BeforeMoveBack += new Divelements.WizardFramework.WizardPageEventHandler(this._page_Vulnerabilities_BeforeMoveBack);
            this._page_Vulnerabilities.BeforeDisplay += new System.EventHandler(this._page_Vulnerabilities_BeforeDisplay);
            // 
            // controlConfigurePolicyVulnerabilities1
            // 
            this.controlConfigurePolicyVulnerabilities1.BackColor = System.Drawing.Color.Transparent;
            this.controlConfigurePolicyVulnerabilities1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlConfigurePolicyVulnerabilities1.Location = new System.Drawing.Point(0, 0);
            this.controlConfigurePolicyVulnerabilities1.Name = "controlConfigurePolicyVulnerabilities1";
            this.controlConfigurePolicyVulnerabilities1.Size = new System.Drawing.Size(756, 536);
            this.controlConfigurePolicyVulnerabilities1.TabIndex = 0;
            // 
            // _page_SQLServers
            // 
            this._page_SQLServers.Controls.Add(this.controlPolicyAddServers1);
            this._page_SQLServers.Description = "Specify which SQL Server instances you want to audit with this policy.";
            this._page_SQLServers.DescriptionColor = System.Drawing.Color.Navy;
            this._page_SQLServers.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this._page_SQLServers.Location = new System.Drawing.Point(19, 73);
            this._page_SQLServers.Name = "_page_SQLServers";
            this._page_SQLServers.NextPage = this._page_Interview;
            this._page_SQLServers.PreviousPage = this._page_Vulnerabilities;
            this._page_SQLServers.Size = new System.Drawing.Size(748, 440);
            this._page_SQLServers.TabIndex = 1010;
            this._page_SQLServers.Text = "Assign SQL Servers to the Policy";
            this._page_SQLServers.TextColor = System.Drawing.Color.Navy;
            this._page_SQLServers.BeforeMoveNext += new Divelements.WizardFramework.WizardPageEventHandler(this._page_SQLServers_BeforeMoveNext);
            this._page_SQLServers.BeforeDisplay += new System.EventHandler(this._page_SQLServers_BeforeDisplay);
            // 
            // controlPolicyAddServers1
            // 
            this.controlPolicyAddServers1.BackColor = System.Drawing.Color.Transparent;
            this.controlPolicyAddServers1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlPolicyAddServers1.Location = new System.Drawing.Point(0, 0);
            this.controlPolicyAddServers1.Name = "controlPolicyAddServers1";
            this.controlPolicyAddServers1.Padding = new System.Windows.Forms.Padding(3);
            this.controlPolicyAddServers1.Size = new System.Drawing.Size(748, 440);
            this.controlPolicyAddServers1.TabIndex = 0;
            // 
            // _page_Interview
            // 
            this._page_Interview.Controls.Add(this._policyInterview);
            this._page_Interview.Description = "Specify any additional information that should be included in the policy report.";
            this._page_Interview.DescriptionColor = System.Drawing.Color.Navy;
            this._page_Interview.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this._page_Interview.Location = new System.Drawing.Point(19, 73);
            this._page_Interview.Name = "_page_Interview";
            this._page_Interview.NextPage = this._page_Finish;
            this._page_Interview.PreviousPage = this._page_SQLServers;
            this._page_Interview.Size = new System.Drawing.Size(748, 440);
            this._page_Interview.TabIndex = 1013;
            this._page_Interview.Text = "Internal Review Notes";
            this._page_Interview.TextColor = System.Drawing.Color.Navy;
            this._page_Interview.BeforeDisplay += new System.EventHandler(this._page_Interview_BeforeDisplay);
            // 
            // _policyInterview
            // 
            this._policyInterview.BackColor = System.Drawing.Color.Transparent;
            this._policyInterview.Dock = System.Windows.Forms.DockStyle.Fill;
            this._policyInterview.InterviewName = "";
            this._policyInterview.Location = new System.Drawing.Point(0, 0);
            this._policyInterview.Name = "_policyInterview";
            this._policyInterview.Padding = new System.Windows.Forms.Padding(6);
            this._policyInterview.Size = new System.Drawing.Size(748, 440);
            this._policyInterview.TabIndex = 0;
            // 
            // _page_Finish
            // 
            this._page_Finish.Controls.Add(this._rtb_Finish);
            this._page_Finish.FinishText = "";
            this._page_Finish.Location = new System.Drawing.Point(177, 66);
            this._page_Finish.Name = "_page_Finish";
            this._page_Finish.PreviousPage = this._page_Interview;
            this._page_Finish.ProceedText = "To create this policy, click Finish";
            this._page_Finish.Size = new System.Drawing.Size(596, 447);
            this._page_Finish.TabIndex = 1006;
            this._page_Finish.Text = "Completing SQLsecure New Policy Wizard";
            this._page_Finish.BeforeDisplay += new System.EventHandler(this._page_Finish_BeforeDisplay);
            // 
            // _rtb_Finish
            // 
            this._rtb_Finish.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._rtb_Finish.BackColor = System.Drawing.SystemColors.Window;
            this._rtb_Finish.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._rtb_Finish.Location = new System.Drawing.Point(0, 0);
            this._rtb_Finish.Name = "_rtb_Finish";
            this._rtb_Finish.ReadOnly = true;
            this._rtb_Finish.Size = new System.Drawing.Size(596, 375);
            this._rtb_Finish.TabIndex = 1;
            this._rtb_Finish.Text = "";
            // 
            // Form_WizardCreatePolicy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(786, 573);
            this.Controls.Add(this._wizard);
            this.ForeColor = System.Drawing.Color.Navy;
            this.MinimumSize = new System.Drawing.Size(794, 600);
            this.Name = "Form_WizardCreatePolicy";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SQLsecure Create Policy";
            this._wizard.ResumeLayout(false);
            this._page_Introduction.ResumeLayout(false);
            this._page_PolicyCreateType.ResumeLayout(false);
            this._page_PolicyCreateType.PerformLayout();
            this._groupBox_PolicyName.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._ultraListViewStandardPolicies)).EndInit();
            this._page_PolicyName.ResumeLayout(false);
            this._page_PolicyName.PerformLayout();
            this._page_Vulnerabilities.ResumeLayout(false);
            this._page_SQLServers.ResumeLayout(false);
            this._page_Interview.ResumeLayout(false);
            this._page_Finish.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Divelements.WizardFramework.Wizard _wizard;
        private Divelements.WizardFramework.IntroductionPage _page_Introduction;
        private System.Windows.Forms.RichTextBox _rtb_Introduction;
        private Divelements.WizardFramework.WizardPage _page_PolicyCreateType;
        private Divelements.WizardFramework.WizardPage _page_Vulnerabilities;
        private Divelements.WizardFramework.FinishPage _page_Finish;
        private System.Windows.Forms.RichTextBox _rtb_Finish;
        private Idera.SQLsecure.UI.Console.Controls.controlConfigurePolicyVulnerabilities controlConfigurePolicyVulnerabilities1;
        private Divelements.WizardFramework.WizardPage _page_SQLServers;
        private Idera.SQLsecure.UI.Console.Controls.ControlPolicyAddServers controlPolicyAddServers1;
        private System.Windows.Forms.RadioButton radioButtonCreateNew;
        private System.Windows.Forms.RadioButton radioButtonCreateFromStandard;
        private Infragistics.Win.UltraWinListView.UltraListView _ultraListViewStandardPolicies;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private Divelements.WizardFramework.WizardPage _page_PolicyName;
        private System.Windows.Forms.TextBox textBox_Description;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_PolicyName;
        private System.Windows.Forms.Label label4;
        private Divelements.WizardFramework.WizardPage _page_Interview;
        private Idera.SQLsecure.UI.Console.Controls.PolicyInterview _policyInterview;
        private System.Windows.Forms.GroupBox _groupBox_PolicyName;
        private System.Windows.Forms.Label _label_Description;
        private System.Windows.Forms.LinkLabel _linkLabel_HelpTemplates;
    }
}