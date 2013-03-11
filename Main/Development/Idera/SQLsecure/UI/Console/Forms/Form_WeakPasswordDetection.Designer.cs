namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_WeakPasswordDetection
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
            this.helpButton = new Infragistics.Win.Misc.UltraButton();
            this.cancelButton = new Infragistics.Win.Misc.UltraButton();
            this.okButton = new Infragistics.Win.Misc.UltraButton();
            this.enableCheck = new System.Windows.Forms.CheckBox();
            this.settingsGroupBox = new System.Windows.Forms.GroupBox();
            this.viewPasswordsLink = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.additionalUpdatedLabel = new System.Windows.Forms.Label();
            this.additionalListTextbox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.clearCustomListButton = new System.Windows.Forms.Button();
            this.customPasswordLink = new System.Windows.Forms.LinkLabel();
            this.customListTextbox = new System.Windows.Forms.TextBox();
            this.customUpdatedLabel = new System.Windows.Forms.Label();
            this.addCustomListButton = new System.Windows.Forms.Button();
            this._bfd_ButtonPanel.SuspendLayout();
            this._bf_MainPanel.SuspendLayout();
            this.settingsGroupBox.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _bfd_ButtonPanel
            // 
            this._bfd_ButtonPanel.Controls.Add(this.helpButton);
            this._bfd_ButtonPanel.Controls.Add(this.cancelButton);
            this._bfd_ButtonPanel.Controls.Add(this.okButton);
            this._bfd_ButtonPanel.Location = new System.Drawing.Point(0, 354);
            this._bfd_ButtonPanel.Size = new System.Drawing.Size(516, 40);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this.okButton, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this.cancelButton, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this.helpButton, 0);
            // 
            // _bf_MainPanel
            // 
            this._bf_MainPanel.Controls.Add(this.settingsGroupBox);
            this._bf_MainPanel.Controls.Add(this.enableCheck);
            this._bf_MainPanel.Size = new System.Drawing.Size(516, 301);
            // 
            // _bf_HeaderPanel
            // 
            this._bf_HeaderPanel.Size = new System.Drawing.Size(516, 53);
            // 
            // helpButton
            // 
            this.helpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.helpButton.Location = new System.Drawing.Point(426, 6);
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(75, 23);
            this.helpButton.TabIndex = 7;
            this.helpButton.Text = "&Help";
            this.helpButton.Click += new System.EventHandler(this.helpButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(345, 6);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(264, 6);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 5;
            this.okButton.Text = "&OK";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // enableCheck
            // 
            this.enableCheck.AutoSize = true;
            this.enableCheck.Location = new System.Drawing.Point(10, 9);
            this.enableCheck.Name = "enableCheck";
            this.enableCheck.Size = new System.Drawing.Size(183, 17);
            this.enableCheck.TabIndex = 0;
            this.enableCheck.Text = "Enable weak password detection";
            this.enableCheck.UseVisualStyleBackColor = true;
            this.enableCheck.CheckedChanged += new System.EventHandler(this.enableCheck_CheckedChanged);
            // 
            // settingsGroupBox
            // 
            this.settingsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsGroupBox.Controls.Add(this.viewPasswordsLink);
            this.settingsGroupBox.Controls.Add(this.label1);
            this.settingsGroupBox.Controls.Add(this.groupBox2);
            this.settingsGroupBox.Controls.Add(this.groupBox1);
            this.settingsGroupBox.Enabled = false;
            this.settingsGroupBox.Location = new System.Drawing.Point(7, 32);
            this.settingsGroupBox.Name = "settingsGroupBox";
            this.settingsGroupBox.Size = new System.Drawing.Size(494, 266);
            this.settingsGroupBox.TabIndex = 1;
            this.settingsGroupBox.TabStop = false;
            this.settingsGroupBox.Text = "Detection Settings";
            // 
            // viewPasswordsLink
            // 
            this.viewPasswordsLink.AutoSize = true;
            this.viewPasswordsLink.Location = new System.Drawing.Point(22, 56);
            this.viewPasswordsLink.Name = "viewPasswordsLink";
            this.viewPasswordsLink.Size = new System.Drawing.Size(136, 13);
            this.viewPasswordsLink.TabIndex = 5;
            this.viewPasswordsLink.TabStop = true;
            this.viewPasswordsLink.Text = "View Default Passwords list";
            this.viewPasswordsLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.viewPasswordsLink_LinkClicked);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(22, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(466, 48);
            this.label1.TabIndex = 7;
            this.label1.Text = "SQLsecure analyzes password health per a Default Weak Passwords list of over 2400" +
                "+ words.  Expand this analysis by adding specific words or attaching a custom li" +
                "st.";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.additionalUpdatedLabel);
            this.groupBox2.Controls.Add(this.additionalListTextbox);
            this.groupBox2.Location = new System.Drawing.Point(21, 81);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(458, 78);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Additional Passwords (semi-colon delimited list)";
            // 
            // additionalUpdatedLabel
            // 
            this.additionalUpdatedLabel.AutoSize = true;
            this.additionalUpdatedLabel.Location = new System.Drawing.Point(12, 19);
            this.additionalUpdatedLabel.Name = "additionalUpdatedLabel";
            this.additionalUpdatedLabel.Size = new System.Drawing.Size(180, 13);
            this.additionalUpdatedLabel.TabIndex = 9;
            this.additionalUpdatedLabel.Text = "Last Updated: 05/04/2012 14:25:12";
            // 
            // additionalListTextbox
            // 
            this.additionalListTextbox.Location = new System.Drawing.Point(15, 41);
            this.additionalListTextbox.Name = "additionalListTextbox";
            this.additionalListTextbox.Size = new System.Drawing.Size(426, 20);
            this.additionalListTextbox.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.clearCustomListButton);
            this.groupBox1.Controls.Add(this.customPasswordLink);
            this.groupBox1.Controls.Add(this.customListTextbox);
            this.groupBox1.Controls.Add(this.customUpdatedLabel);
            this.groupBox1.Controls.Add(this.addCustomListButton);
            this.groupBox1.Location = new System.Drawing.Point(21, 173);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(458, 73);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Custom List";
            // 
            // clearCustomListButton
            // 
            this.clearCustomListButton.Location = new System.Drawing.Point(366, 35);
            this.clearCustomListButton.Name = "clearCustomListButton";
            this.clearCustomListButton.Size = new System.Drawing.Size(75, 23);
            this.clearCustomListButton.TabIndex = 10;
            this.clearCustomListButton.Text = "Remove List";
            this.clearCustomListButton.UseVisualStyleBackColor = true;
            this.clearCustomListButton.Click += new System.EventHandler(this.clearCustomListButton_Click);
            // 
            // customPasswordLink
            // 
            this.customPasswordLink.AutoSize = true;
            this.customPasswordLink.Location = new System.Drawing.Point(223, 16);
            this.customPasswordLink.Name = "customPasswordLink";
            this.customPasswordLink.Size = new System.Drawing.Size(136, 13);
            this.customPasswordLink.TabIndex = 9;
            this.customPasswordLink.TabStop = true;
            this.customPasswordLink.Text = "View Custom Password List";
            this.customPasswordLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.customPasswordLink_LinkClicked);
            // 
            // customListTextbox
            // 
            this.customListTextbox.Location = new System.Drawing.Point(15, 37);
            this.customListTextbox.Name = "customListTextbox";
            this.customListTextbox.Size = new System.Drawing.Size(309, 20);
            this.customListTextbox.TabIndex = 3;
            // 
            // customUpdatedLabel
            // 
            this.customUpdatedLabel.AutoSize = true;
            this.customUpdatedLabel.Location = new System.Drawing.Point(14, 16);
            this.customUpdatedLabel.Name = "customUpdatedLabel";
            this.customUpdatedLabel.Size = new System.Drawing.Size(185, 13);
            this.customUpdatedLabel.TabIndex = 8;
            this.customUpdatedLabel.Text = "Last Uploaded: 05/04/2012 14:25:12";
            // 
            // addCustomListButton
            // 
            this.addCustomListButton.Location = new System.Drawing.Point(330, 35);
            this.addCustomListButton.Name = "addCustomListButton";
            this.addCustomListButton.Size = new System.Drawing.Size(30, 23);
            this.addCustomListButton.TabIndex = 4;
            this.addCustomListButton.Text = "...";
            this.addCustomListButton.UseVisualStyleBackColor = true;
            this.addCustomListButton.Click += new System.EventHandler(this.addCustomListButton_Click);
            // 
            // Form_WeakPasswordDetection
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(516, 394);
            this.Description = "Configure Weak Password Detection";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Form_WeakPasswordDetection";
            this.Text = "Weak Password Detection";
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.Form_WeakPasswordDetection_HelpRequested);
            this._bfd_ButtonPanel.ResumeLayout(false);
            this._bf_MainPanel.ResumeLayout(false);
            this._bf_MainPanel.PerformLayout();
            this.settingsGroupBox.ResumeLayout(false);
            this.settingsGroupBox.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton helpButton;
        private Infragistics.Win.Misc.UltraButton cancelButton;
        private Infragistics.Win.Misc.UltraButton okButton;
        private System.Windows.Forms.GroupBox settingsGroupBox;
        private System.Windows.Forms.CheckBox enableCheck;
        private System.Windows.Forms.Button addCustomListButton;
        private System.Windows.Forms.TextBox customListTextbox;
        private System.Windows.Forms.TextBox additionalListTextbox;
        private System.Windows.Forms.LinkLabel viewPasswordsLink;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.LinkLabel customPasswordLink;
        private System.Windows.Forms.Label customUpdatedLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label additionalUpdatedLabel;
        private System.Windows.Forms.Button clearCustomListButton;
    }
}
