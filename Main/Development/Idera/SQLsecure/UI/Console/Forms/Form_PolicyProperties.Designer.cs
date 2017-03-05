namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_PolicyProperties
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this._textBox_Notes = new System.Windows.Forms.TextBox();
            this._label_Notes = new System.Windows.Forms.Label();
            this.textBox_Description = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_PolicyName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.controlConfigurePolicyVulnerabilities1 = new Idera.SQLsecure.UI.Console.Controls.controlConfigurePolicyVulnerabilities(Utility.ConfigurePolicyControlType.ConfigureSecurityCheck);
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.controlPolicyAddServers1 = new Idera.SQLsecure.UI.Console.Controls.ControlPolicyAddServers();
            this.ultraTabPageControl4 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this._policyInterview = new Idera.SQLsecure.UI.Console.Controls.PolicyInterview();
            this.button_Cancel = new Infragistics.Win.Misc.UltraButton();
            this.button_OK = new Infragistics.Win.Misc.UltraButton();
            this.ultraTabControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this._btn_Help = new Infragistics.Win.Misc.UltraButton();
            this._bfd_ButtonPanel.SuspendLayout();
            this._bf_MainPanel.SuspendLayout();
            this.ultraTabPageControl3.SuspendLayout();
            this.ultraTabPageControl1.SuspendLayout();
            this.ultraTabPageControl2.SuspendLayout();
            this.ultraTabPageControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).BeginInit();
            this.ultraTabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _bfd_ButtonPanel
            // 
            this._bfd_ButtonPanel.Controls.Add(this.button_OK);
            this._bfd_ButtonPanel.Controls.Add(this.button_Cancel);
            this._bfd_ButtonPanel.Controls.Add(this._btn_Help);
            this._bfd_ButtonPanel.Location = new System.Drawing.Point(0, 589);
            this._bfd_ButtonPanel.Size = new System.Drawing.Size(756, 43);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this._btn_Help, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this.button_Cancel, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this.button_OK, 0);
            // 
            // _bf_MainPanel
            // 
            this._bf_MainPanel.Controls.Add(this.ultraTabControl1);
            this._bf_MainPanel.Size = new System.Drawing.Size(756, 536);
            // 
            // _bf_HeaderPanel
            // 
            this._bf_HeaderPanel.Size = new System.Drawing.Size(756, 53);
            // 
            // ultraTabPageControl3
            // 
            this.ultraTabPageControl3.Controls.Add(this._textBox_Notes);
            this.ultraTabPageControl3.Controls.Add(this._label_Notes);
            this.ultraTabPageControl3.Controls.Add(this.textBox_Description);
            this.ultraTabPageControl3.Controls.Add(this.label1);
            this.ultraTabPageControl3.Controls.Add(this.textBox_PolicyName);
            this.ultraTabPageControl3.Controls.Add(this.label2);
            this.ultraTabPageControl3.Location = new System.Drawing.Point(2, 24);
            this.ultraTabPageControl3.Name = "ultraTabPageControl3";
            this.ultraTabPageControl3.Size = new System.Drawing.Size(752, 510);
            // 
            // _textBox_Notes
            // 
            this._textBox_Notes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._textBox_Notes.Location = new System.Drawing.Point(96, 298);
            this._textBox_Notes.Multiline = true;
            this._textBox_Notes.Name = "_textBox_Notes";
            this._textBox_Notes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._textBox_Notes.Size = new System.Drawing.Size(632, 189);
            this._textBox_Notes.TabIndex = 3;
            // 
            // _label_Notes
            // 
            this._label_Notes.AutoSize = true;
            this._label_Notes.Location = new System.Drawing.Point(11, 301);
            this._label_Notes.Name = "_label_Notes";
            this._label_Notes.Size = new System.Drawing.Size(35, 13);
            this._label_Notes.TabIndex = 10;
            this._label_Notes.Text = "Notes";
            // 
            // textBox_Description
            // 
            this.textBox_Description.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Description.Location = new System.Drawing.Point(96, 88);
            this.textBox_Description.MaxLength = 2048;
            this.textBox_Description.Multiline = true;
            this.textBox_Description.Name = "textBox_Description";
            this.textBox_Description.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_Description.Size = new System.Drawing.Size(632, 189);
            this.textBox_Description.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Description";
            // 
            // textBox_PolicyName
            // 
            this.textBox_PolicyName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_PolicyName.Location = new System.Drawing.Point(96, 42);
            this.textBox_PolicyName.MaxLength = 128;
            this.textBox_PolicyName.Name = "textBox_PolicyName";
            this.textBox_PolicyName.Size = new System.Drawing.Size(632, 20);
            this.textBox_PolicyName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Name";
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.controlConfigurePolicyVulnerabilities1);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(750, 510);
            // 
            // controlConfigurePolicyVulnerabilities1
            // 
            this.controlConfigurePolicyVulnerabilities1.BackColor = System.Drawing.Color.Transparent;
            this.controlConfigurePolicyVulnerabilities1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlConfigurePolicyVulnerabilities1.Location = new System.Drawing.Point(0, 0);
            this.controlConfigurePolicyVulnerabilities1.Name = "controlConfigurePolicyVulnerabilities1";
            this.controlConfigurePolicyVulnerabilities1.Padding = new System.Windows.Forms.Padding(3);
            this.controlConfigurePolicyVulnerabilities1.Size = new System.Drawing.Size(750, 510);
            this.controlConfigurePolicyVulnerabilities1.TabIndex = 4;
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this.controlPolicyAddServers1);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(750, 510);
            // 
            // controlPolicyAddServers1
            // 
            this.controlPolicyAddServers1.BackColor = System.Drawing.Color.Transparent;
            this.controlPolicyAddServers1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlPolicyAddServers1.Location = new System.Drawing.Point(0, 0);
            this.controlPolicyAddServers1.Name = "controlPolicyAddServers1";
            this.controlPolicyAddServers1.Padding = new System.Windows.Forms.Padding(3);
            this.controlPolicyAddServers1.Size = new System.Drawing.Size(750, 510);
            this.controlPolicyAddServers1.TabIndex = 5;
            // 
            // ultraTabPageControl4
            // 
            this.ultraTabPageControl4.Controls.Add(this._policyInterview);
            this.ultraTabPageControl4.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl4.Name = "ultraTabPageControl4";
            this.ultraTabPageControl4.Size = new System.Drawing.Size(750, 510);
            // 
            // _policyInterview
            // 
            this._policyInterview.BackColor = System.Drawing.Color.Transparent;
            this._policyInterview.Dock = System.Windows.Forms.DockStyle.Fill;
            this._policyInterview.InterviewName = "";
            this._policyInterview.Location = new System.Drawing.Point(0, 0);
            this._policyInterview.Name = "_policyInterview";
            this._policyInterview.Padding = new System.Windows.Forms.Padding(6);
            this._policyInterview.Size = new System.Drawing.Size(750, 510);
            this._policyInterview.TabIndex = 6;
            // 
            // button_Cancel
            // 
            this.button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(574, 10);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.button_Cancel.TabIndex = 8;
            this.button_Cancel.Text = "&Cancel";
            // 
            // button_OK
            // 
            this.button_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_OK.Location = new System.Drawing.Point(484, 10);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(75, 23);
            this.button_OK.TabIndex = 7;
            this.button_OK.Text = "&OK";
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // ultraTabControl1
            // 
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.White;
            this.ultraTabControl1.ActiveTabAppearance = appearance1;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            appearance2.BackColor2 = System.Drawing.Color.Transparent;
            this.ultraTabControl1.Appearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.BackColor2 = System.Drawing.Color.Transparent;
            this.ultraTabControl1.ClientAreaAppearance = appearance3;
            this.ultraTabControl1.Controls.Add(this.ultraTabSharedControlsPage1);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl1);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl2);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl3);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl4);
            this.ultraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.ultraTabControl1.Name = "ultraTabControl1";
            this.ultraTabControl1.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.ultraTabControl1.Size = new System.Drawing.Size(756, 536);
            this.ultraTabControl1.TabIndex = 0;
            ultraTab3.Key = "General";
            ultraTab3.TabPage = this.ultraTabPageControl3;
            ultraTab3.Text = "General";
            ultraTab1.Key = "Vulnerabilities";
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "Security Checks";
            ultraTab2.Key = "SQLServerInstances";
            ultraTab2.TabPage = this.ultraTabPageControl2;
            ultraTab2.Text = "Audited SQL Servers";
            ultraTab4.Key = "Interview";
            ultraTab4.TabPage = this.ultraTabPageControl4;
            ultraTab4.Text = "Internal Review Notes";
            this.ultraTabControl1.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab3,
            ultraTab1,
            ultraTab2,
            ultraTab4});
            this.ultraTabControl1.SelectedTabChanging += new Infragistics.Win.UltraWinTabControl.SelectedTabChangingEventHandler(this.ultraTabControl1_SelectedTabChanging);
            this.ultraTabControl1.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.ultraTabControl1_SelectedTabChanged);
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(752, 510);
            // 
            // _btn_Help
            // 
            this._btn_Help.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btn_Help.Location = new System.Drawing.Point(664, 10);
            this._btn_Help.Name = "_btn_Help";
            this._btn_Help.Size = new System.Drawing.Size(75, 23);
            this._btn_Help.TabIndex = 9;
            this._btn_Help.Text = "&Help";
            this._btn_Help.Click += new System.EventHandler(this._btn_Help_Click);
            // 
            // Form_PolicyProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(756, 632);
            this.Description = "Manage Policy Settings";
            this.Name = "Form_PolicyProperties";
            this.Picture = global::Idera.SQLsecure.UI.Console.Properties.Resources.edit_policy_49;
            this.Text = "Policy Properties";
            this.Load += new System.EventHandler(this.Form_PolicyProperties_Load);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.Form_PolicyProperties_HelpRequested);
            this._bfd_ButtonPanel.ResumeLayout(false);
            this._bf_MainPanel.ResumeLayout(false);
            this.ultraTabPageControl3.ResumeLayout(false);
            this.ultraTabPageControl3.PerformLayout();
            this.ultraTabPageControl1.ResumeLayout(false);
            this.ultraTabPageControl2.ResumeLayout(false);
            this.ultraTabPageControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).EndInit();
            this.ultraTabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton button_Cancel;
        private Infragistics.Win.Misc.UltraButton button_OK;
        private Idera.SQLsecure.UI.Console.Controls.controlConfigurePolicyVulnerabilities controlConfigurePolicyVulnerabilities1;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl ultraTabControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private Idera.SQLsecure.UI.Console.Controls.ControlPolicyAddServers controlPolicyAddServers1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl4;
        private System.Windows.Forms.TextBox textBox_Description;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_PolicyName;
        private System.Windows.Forms.Label label2;
        private Idera.SQLsecure.UI.Console.Controls.PolicyInterview _policyInterview;
        private Infragistics.Win.Misc.UltraButton _btn_Help;
        private System.Windows.Forms.TextBox _textBox_Notes;
        private System.Windows.Forms.Label _label_Notes;
    }
}
