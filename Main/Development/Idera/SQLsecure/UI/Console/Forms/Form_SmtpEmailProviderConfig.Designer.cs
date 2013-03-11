namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_SmtpEmailProviderConfig
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
            this.cancelButton = new Infragistics.Win.Misc.UltraButton();
            this.okButton = new Infragistics.Win.Misc.UltraButton();
            this.testButton = new Infragistics.Win.Misc.UltraButton();
            this.toolTipManager = new Infragistics.Win.UltraWinToolTip.UltraToolTipManager(this.components);
            this.controlSMTPEmailConfig1 = new Idera.SQLsecure.UI.Console.Controls.controlSMTPEmailConfig();
            this.helpButton = new Infragistics.Win.Misc.UltraButton();
            this._bfd_ButtonPanel.SuspendLayout();
            this._bf_MainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _bfd_ButtonPanel
            // 
            this._bfd_ButtonPanel.Controls.Add(this.helpButton);
            this._bfd_ButtonPanel.Controls.Add(this.cancelButton);
            this._bfd_ButtonPanel.Controls.Add(this.okButton);
            this._bfd_ButtonPanel.Controls.Add(this.testButton);
            this._bfd_ButtonPanel.Location = new System.Drawing.Point(0, 388);
            this._bfd_ButtonPanel.Size = new System.Drawing.Size(477, 40);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this.testButton, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this.okButton, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this.cancelButton, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this.helpButton, 0);
            // 
            // _bf_MainPanel
            // 
            this._bf_MainPanel.Controls.Add(this.controlSMTPEmailConfig1);
            this._bf_MainPanel.Size = new System.Drawing.Size(477, 335);
            // 
            // _bf_HeaderPanel
            // 
            this._bf_HeaderPanel.Size = new System.Drawing.Size(477, 53);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(304, 9);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "&Cancel";
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(223, 9);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "&OK";
            // 
            // testButton
            // 
            this.testButton.Location = new System.Drawing.Point(8, 9);
            this.testButton.Name = "testButton";
            this.testButton.Size = new System.Drawing.Size(75, 23);
            this.testButton.TabIndex = 1;
            this.testButton.Text = "&Test";
            this.testButton.Click += new System.EventHandler(this.testButton_Click);
            // 
            // toolTipManager
            // 
            this.toolTipManager.ContainingControl = this;
            this.toolTipManager.DisplayStyle = Infragistics.Win.ToolTipDisplayStyle.Office2007;
            // 
            // controlSMTPEmailConfig1
            // 
            this.controlSMTPEmailConfig1.BackColor = System.Drawing.Color.Transparent;
            this.controlSMTPEmailConfig1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlSMTPEmailConfig1.Location = new System.Drawing.Point(0, 0);
            this.controlSMTPEmailConfig1.Name = "controlSMTPEmailConfig1";
            this.controlSMTPEmailConfig1.Size = new System.Drawing.Size(477, 335);
            this.controlSMTPEmailConfig1.TabIndex = 0;
            // 
            // helpButton
            // 
            this.helpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.helpButton.Location = new System.Drawing.Point(385, 9);
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(75, 23);
            this.helpButton.TabIndex = 4;
            this.helpButton.Text = "&Help";
            this.helpButton.Click += new System.EventHandler(this.helpButton_Click);
            // 
            // Form_SmtpEmailProviderConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 428);
            this.Description = "Configure Email Provider used for SQLsecure Data Collection Notification";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Form_SmtpEmailProviderConfig";
            this.Text = "SMTP Email Provider";
            this._bfd_ButtonPanel.ResumeLayout(false);
            this._bf_MainPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton cancelButton;
        private Infragistics.Win.Misc.UltraButton okButton;
        private Infragistics.Win.Misc.UltraButton testButton;
        private Infragistics.Win.UltraWinToolTip.UltraToolTipManager toolTipManager;
        private Idera.SQLsecure.UI.Console.Controls.controlSMTPEmailConfig controlSMTPEmailConfig1;
        private Infragistics.Win.Misc.UltraButton helpButton;
    }
}