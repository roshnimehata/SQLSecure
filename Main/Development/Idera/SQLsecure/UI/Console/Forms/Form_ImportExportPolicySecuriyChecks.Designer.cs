namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_ImportExportPolicySecuriyChecks
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
            this.button_Cancel = new Infragistics.Win.Misc.UltraButton();
            this.button_OK = new Infragistics.Win.Misc.UltraButton();
            this._btn_Help = new Infragistics.Win.Misc.UltraButton();

            // SQLsecure 3.1 (Anshul Aggarwal) - Disable editing. 
            this.ctrSelectingPolicyVulnerabilities = new Idera.SQLsecure.UI.Console.Controls.controlConfigurePolicyVulnerabilities(Utility.ConfigurePolicyControlType.ImportExportSecurityCheck);

            this._bfd_ButtonPanel.SuspendLayout();
            this._bf_MainPanel.SuspendLayout();
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
            this._bf_MainPanel.Controls.Add(this.ctrSelectingPolicyVulnerabilities);
            this._bf_MainPanel.Size = new System.Drawing.Size(756, 536);
            // 
            // _bf_HeaderPanel
            // 
            this._bf_HeaderPanel.Size = new System.Drawing.Size(756, 53);
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
            this.button_OK.Location = new System.Drawing.Point(484, 10);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(75, 23);
            this.button_OK.TabIndex = 7;
            this.button_OK.Text = "&OK";
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
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
            // ctrSelectingPolicyVulnerabilities
            // 
            this.ctrSelectingPolicyVulnerabilities.BackColor = System.Drawing.Color.Transparent;
            this.ctrSelectingPolicyVulnerabilities.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrSelectingPolicyVulnerabilities.Location = new System.Drawing.Point(0, 0);
            this.ctrSelectingPolicyVulnerabilities.Name = "ctrSelectingPolicyVulnerabilities";
            this.ctrSelectingPolicyVulnerabilities.Padding = new System.Windows.Forms.Padding(3);
            this.ctrSelectingPolicyVulnerabilities.Size = new System.Drawing.Size(756, 536);
            this.ctrSelectingPolicyVulnerabilities.TabIndex = 5;
            // 
            // Form_SelectSecuriyChecks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(756, 632);
            this.Name = "Form_SelectSecuriyChecks";
            this.Picture = global::Idera.SQLsecure.UI.Console.Properties.Resources.edit_policy_49;
            this.Text = "Select Securiy Checks";
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.Form_PolicyProperties_HelpRequested);
            this._bfd_ButtonPanel.ResumeLayout(false);
            this._bf_MainPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton button_Cancel;
        private Infragistics.Win.Misc.UltraButton button_OK;
        private Infragistics.Win.Misc.UltraButton _btn_Help;
        private Idera.SQLsecure.UI.Console.Controls.controlConfigurePolicyVulnerabilities ctrSelectingPolicyVulnerabilities;
    }
}
