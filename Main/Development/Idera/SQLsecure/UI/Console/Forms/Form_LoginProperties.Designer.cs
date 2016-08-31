namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_LoginProperties
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_LoginName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._static_Desc = new System.Windows.Forms.Label();
            this.radioButton_DenyAccess = new System.Windows.Forms.RadioButton();
            this.radioButton_GrantAccess = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._rdbtn_No = new System.Windows.Forms.RadioButton();
            this._rdbtn_Yes = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.ultraButton_Cancel = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton_OK = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton_Help = new Infragistics.Win.Misc.UltraButton();
            this._bfd_ButtonPanel.SuspendLayout();
            this._bf_MainPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // _bfd_ButtonPanel
            // 
            this._bfd_ButtonPanel.Controls.Add(this.ultraButton_Help);
            this._bfd_ButtonPanel.Controls.Add(this.ultraButton_OK);
            this._bfd_ButtonPanel.Controls.Add(this.ultraButton_Cancel);
            this._bfd_ButtonPanel.Location = new System.Drawing.Point(0, 325);
            this._bfd_ButtonPanel.Size = new System.Drawing.Size(383, 40);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this.ultraButton_Cancel, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this.ultraButton_OK, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this.ultraButton_Help, 0);
            // 
            // _bf_MainPanel
            // 
            this._bf_MainPanel.Controls.Add(this.label1);
            this._bf_MainPanel.Controls.Add(this.textBox_LoginName);
            this._bf_MainPanel.Controls.Add(this.groupBox1);
            this._bf_MainPanel.Controls.Add(this.groupBox2);
            this._bf_MainPanel.Size = new System.Drawing.Size(383, 272);
            // 
            // _bf_HeaderPanel
            // 
            this._bf_HeaderPanel.Size = new System.Drawing.Size(383, 53);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // textBox_LoginName
            // 
            this.textBox_LoginName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_LoginName.BackColor = System.Drawing.Color.GhostWhite;
            this.textBox_LoginName.ForeColor = System.Drawing.Color.SlateGray;
            this.textBox_LoginName.Location = new System.Drawing.Point(58, 19);
            this.textBox_LoginName.Name = "textBox_LoginName";
            this.textBox_LoginName.ReadOnly = true;
            this.textBox_LoginName.Size = new System.Drawing.Size(318, 20);
            this.textBox_LoginName.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this._static_Desc);
            this.groupBox1.Controls.Add(this.radioButton_DenyAccess);
            this.groupBox1.Controls.Add(this.radioButton_GrantAccess);
            this.groupBox1.Location = new System.Drawing.Point(12, 45);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(359, 96);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Security access";
            // 
            // _static_Desc
            // 
            this._static_Desc.Location = new System.Drawing.Point(6, 16);
            this._static_Desc.Name = "_static_Desc";
            this._static_Desc.Size = new System.Drawing.Size(292, 27);
            this._static_Desc.TabIndex = 9;
            this._static_Desc.Text = "Do you want to grant this user access to SQLsecure?";
            // 
            // radioButton_DenyAccess
            // 
            this.radioButton_DenyAccess.AutoSize = true;
            this.radioButton_DenyAccess.Location = new System.Drawing.Point(9, 69);
            this.radioButton_DenyAccess.Name = "radioButton_DenyAccess";
            this.radioButton_DenyAccess.Size = new System.Drawing.Size(87, 17);
            this.radioButton_DenyAccess.TabIndex = 1;
            this.radioButton_DenyAccess.TabStop = true;
            this.radioButton_DenyAccess.Text = "&Deny access";
            this.radioButton_DenyAccess.UseVisualStyleBackColor = true;
            this.radioButton_DenyAccess.CheckedChanged += new System.EventHandler(this.radioButton_DenyAccess_CheckedChanged);
            // 
            // radioButton_GrantAccess
            // 
            this.radioButton_GrantAccess.AutoSize = true;
            this.radioButton_GrantAccess.Location = new System.Drawing.Point(9, 46);
            this.radioButton_GrantAccess.Name = "radioButton_GrantAccess";
            this.radioButton_GrantAccess.Size = new System.Drawing.Size(88, 17);
            this.radioButton_GrantAccess.TabIndex = 0;
            this.radioButton_GrantAccess.TabStop = true;
            this.radioButton_GrantAccess.Text = "&Grant access";
            this.radioButton_GrantAccess.UseVisualStyleBackColor = true;
            this.radioButton_GrantAccess.CheckedChanged += new System.EventHandler(this.radioButton_GrantAccess_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this._rdbtn_No);
            this.groupBox2.Controls.Add(this._rdbtn_Yes);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(12, 147);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(359, 109);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Permissions within SQLsecure";
            // 
            // _rdbtn_No
            // 
            this._rdbtn_No.Location = new System.Drawing.Point(9, 73);
            this._rdbtn_No.Name = "_rdbtn_No";
            this._rdbtn_No.Size = new System.Drawing.Size(302, 24);
            this._rdbtn_No.TabIndex = 14;
            this._rdbtn_No.Text = "&No, only allow this user the ability to view audit data.";
            this._rdbtn_No.UseVisualStyleBackColor = true;
            // 
            // _rdbtn_Yes
            // 
            this._rdbtn_Yes.Checked = true;
            this._rdbtn_Yes.Location = new System.Drawing.Point(9, 43);
            this._rdbtn_Yes.Name = "_rdbtn_Yes";
            this._rdbtn_Yes.Size = new System.Drawing.Size(302, 24);
            this._rdbtn_Yes.TabIndex = 13;
            this._rdbtn_Yes.TabStop = true;
            this._rdbtn_Yes.Text = "&Yes, grant this user permission to configure SQLsecure.";
            this._rdbtn_Yes.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(339, 24);
            this.label2.TabIndex = 11;
            this.label2.Text = "Do you want this login to have the System Administrator server role?";
            // 
            // ultraButton_Cancel
            // 
            this.ultraButton_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ultraButton_Cancel.Location = new System.Drawing.Point(205, 9);
            this.ultraButton_Cancel.Name = "ultraButton_Cancel";
            this.ultraButton_Cancel.Size = new System.Drawing.Size(75, 23);
            this.ultraButton_Cancel.TabIndex = 1;
            this.ultraButton_Cancel.Text = "&Cancel";
            this.ultraButton_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // ultraButton_OK
            // 
            this.ultraButton_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ultraButton_OK.Location = new System.Drawing.Point(120, 9);
            this.ultraButton_OK.Name = "ultraButton_OK";
            this.ultraButton_OK.Size = new System.Drawing.Size(75, 23);
            this.ultraButton_OK.TabIndex = 2;
            this.ultraButton_OK.Text = "&OK";
            this.ultraButton_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // ultraButton_Help
            // 
            this.ultraButton_Help.Location = new System.Drawing.Point(290, 9);
            this.ultraButton_Help.Name = "ultraButton_Help";
            this.ultraButton_Help.Size = new System.Drawing.Size(75, 23);
            this.ultraButton_Help.TabIndex = 3;
            this.ultraButton_Help.Text = "&Help";
            this.ultraButton_Help.Click += new System.EventHandler(this.ultraButton_Help_Click);
            // 
            // Form_LoginProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 365);
            this.Description = "Manage Logins for the SQLsecure Repository.";
            this.Name = "Form_LoginProperties";
            this.Picture = global::Idera.SQLsecure.UI.Console.Properties.Resources.SQLServerLogin_Wiz;
            this.Text = "Login Properties";
            this.Shown += new System.EventHandler(this.Form_LoginProperties_Shown);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.Form_LoginProperties_HelpRequested);
            this._bfd_ButtonPanel.ResumeLayout(false);
            this._bf_MainPanel.ResumeLayout(false);
            this._bf_MainPanel.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_LoginName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton_DenyAccess;
        private System.Windows.Forms.RadioButton radioButton_GrantAccess;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton _rdbtn_No;
        private System.Windows.Forms.RadioButton _rdbtn_Yes;
        private System.Windows.Forms.Label _static_Desc;
        private Infragistics.Win.Misc.UltraButton ultraButton_OK;
        private Infragistics.Win.Misc.UltraButton ultraButton_Cancel;
        private Infragistics.Win.Misc.UltraButton ultraButton_Help;
    }
}