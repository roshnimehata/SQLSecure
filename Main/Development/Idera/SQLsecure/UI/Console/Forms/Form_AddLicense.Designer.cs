namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_AddLicense
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
            this.button_OK = new Infragistics.Win.Misc.UltraButton();
            this.button_Cancel = new Infragistics.Win.Misc.UltraButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_GenerateTrialLicense = new Infragistics.Win.Misc.UltraButton();
            this.textBox_NewKey = new System.Windows.Forms.TextBox();
            this.label_NewUser1 = new System.Windows.Forms.Label();
            this.label_NewUser2 = new System.Windows.Forms.Label();
            this._bfd_ButtonPanel.SuspendLayout();
            this._bf_MainPanel.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // _bfd_ButtonPanel
            // 
            this._bfd_ButtonPanel.Controls.Add(this.button_OK);
            this._bfd_ButtonPanel.Controls.Add(this.button_Cancel);
            this._bfd_ButtonPanel.Location = new System.Drawing.Point(0, 232);
            this._bfd_ButtonPanel.Size = new System.Drawing.Size(569, 40);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this.button_Cancel, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this.button_OK, 0);
            // 
            // _bf_MainPanel
            // 
            this._bf_MainPanel.Controls.Add(this.label_NewUser1);
            this._bf_MainPanel.Controls.Add(this.groupBox2);
            this._bf_MainPanel.Controls.Add(this.label_NewUser2);
            this._bf_MainPanel.Size = new System.Drawing.Size(569, 179);
            // 
            // _bf_HeaderPanel
            // 
            this._bf_HeaderPanel.Size = new System.Drawing.Size(569, 53);
            // 
            // button_OK
            // 
            this.button_OK.Location = new System.Drawing.Point(382, 9);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(75, 23);
            this.button_OK.TabIndex = 2;
            this.button_OK.Text = "&OK";
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(474, 9);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.button_Cancel.TabIndex = 3;
            this.button_Cancel.Text = "&Cancel";
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.button_GenerateTrialLicense);
            this.groupBox2.Controls.Add(this.textBox_NewKey);
            this.groupBox2.Location = new System.Drawing.Point(12, 76);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(538, 72);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Enter License Key";
            // 
            // button_GenerateTrialLicense
            // 
            this.button_GenerateTrialLicense.Location = new System.Drawing.Point(382, 25);
            this.button_GenerateTrialLicense.Name = "button_GenerateTrialLicense";
            this.button_GenerateTrialLicense.Size = new System.Drawing.Size(150, 23);
            this.button_GenerateTrialLicense.TabIndex = 12;
            this.button_GenerateTrialLicense.Text = "&Generate Trial License";
            this.button_GenerateTrialLicense.Click += new System.EventHandler(this.button_GenerateTrialLicense_Click);
            // 
            // textBox_NewKey
            // 
            this.textBox_NewKey.Location = new System.Drawing.Point(11, 27);
            this.textBox_NewKey.Name = "textBox_NewKey";
            this.textBox_NewKey.Size = new System.Drawing.Size(356, 20);
            this.textBox_NewKey.TabIndex = 0;
            this.textBox_NewKey.TextChanged += new System.EventHandler(this.textBox_NewKey_TextChanged);
            // 
            // label_NewUser1
            // 
            this.label_NewUser1.BackColor = System.Drawing.Color.Transparent;
            this.label_NewUser1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_NewUser1.Location = new System.Drawing.Point(12, 15);
            this.label_NewUser1.Name = "label_NewUser1";
            this.label_NewUser1.Size = new System.Drawing.Size(342, 19);
            this.label_NewUser1.TabIndex = 7;
            this.label_NewUser1.Text = "Welcome to SQLsecure";
            // 
            // label_NewUser2
            // 
            this.label_NewUser2.BackColor = System.Drawing.Color.Transparent;
            this.label_NewUser2.Location = new System.Drawing.Point(12, 38);
            this.label_NewUser2.Name = "label_NewUser2";
            this.label_NewUser2.Size = new System.Drawing.Size(538, 30);
            this.label_NewUser2.TabIndex = 8;
            this.label_NewUser2.Text = "SQLsecure requires a license to run.  Please enter your production license or gen" +
                "erate a 14 day trial license.";
            // 
            // Form_AddLicense
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(569, 272);
            this.Description = "Enter license for SQLsecure";
            this.Name = "Form_AddLicense";
            this.Text = "Idera SQLsecure";
            this._bfd_ButtonPanel.ResumeLayout(false);
            this._bf_MainPanel.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            //(SQLSecure 3.1 Barkha Khatri)SQLSECU-1755 fix 
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton button_OK;
        private Infragistics.Win.Misc.UltraButton button_Cancel;
        private System.Windows.Forms.GroupBox groupBox2;
        private Infragistics.Win.Misc.UltraButton button_GenerateTrialLicense;
        private System.Windows.Forms.TextBox textBox_NewKey;
        private System.Windows.Forms.Label label_NewUser1;
        private System.Windows.Forms.Label label_NewUser2;
    }
}
