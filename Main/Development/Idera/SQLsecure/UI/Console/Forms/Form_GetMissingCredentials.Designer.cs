namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_GetMissingCredentials
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_GetMissingCredentials));
            this.button_OK = new Infragistics.Win.Misc.UltraButton();
            this.button_Cancel = new Infragistics.Win.Misc.UltraButton();
            this._btn_Help = new Infragistics.Win.Misc.UltraButton();
            this._label_DescriptiveText = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._textBox_Username = new System.Windows.Forms.TextBox();
            this._textBox_Password = new System.Windows.Forms.TextBox();
            this._bfd_ButtonPanel.SuspendLayout();
            this._bf_MainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _bfd_ButtonPanel
            // 
            this._bfd_ButtonPanel.Controls.Add(this.button_OK);
            this._bfd_ButtonPanel.Controls.Add(this.button_Cancel);
            this._bfd_ButtonPanel.Controls.Add(this._btn_Help);
            this._bfd_ButtonPanel.Location = new System.Drawing.Point(0, 224);
            this._bfd_ButtonPanel.Size = new System.Drawing.Size(508, 40);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this._btn_Help, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this.button_Cancel, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this.button_OK, 0);
            // 
            // _bf_MainPanel
            // 
            this._bf_MainPanel.Controls.Add(this._textBox_Password);
            this._bf_MainPanel.Controls.Add(this._textBox_Username);
            this._bf_MainPanel.Controls.Add(this.label3);
            this._bf_MainPanel.Controls.Add(this.label2);
            this._bf_MainPanel.Controls.Add(this._label_DescriptiveText);
            this._bf_MainPanel.Size = new System.Drawing.Size(508, 171);
            // 
            // _bf_HeaderPanel
            // 
            this._bf_HeaderPanel.Size = new System.Drawing.Size(508, 53);
            // 
            // button_OK
            // 
            this.button_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_OK.Location = new System.Drawing.Point(236, 9);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(75, 23);
            this.button_OK.TabIndex = 6;
            this.button_OK.Text = "&OK";
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(326, 9);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.button_Cancel.TabIndex = 5;
            this.button_Cancel.Text = "&Cancel";
            // 
            // _btn_Help
            // 
            this._btn_Help.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btn_Help.Location = new System.Drawing.Point(416, 9);
            this._btn_Help.Name = "_btn_Help";
            this._btn_Help.Size = new System.Drawing.Size(75, 23);
            this._btn_Help.TabIndex = 7;
            this._btn_Help.Text = "&Help";
            this._btn_Help.Click += new System.EventHandler(this._btn_Help_Click);
            // 
            // _label_DescriptiveText
            // 
            this._label_DescriptiveText.BackColor = System.Drawing.Color.Transparent;
            this._label_DescriptiveText.Dock = System.Windows.Forms.DockStyle.Top;
            this._label_DescriptiveText.Location = new System.Drawing.Point(0, 0);
            this._label_DescriptiveText.Name = "_label_DescriptiveText";
            this._label_DescriptiveText.Padding = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this._label_DescriptiveText.Size = new System.Drawing.Size(508, 74);
            this._label_DescriptiveText.TabIndex = 0;
            this._label_DescriptiveText.Text = resources.GetString("_label_DescriptiveText.Text");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(11, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Username";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(11, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Password";
            // 
            // _textBox_Username
            // 
            this._textBox_Username.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._textBox_Username.Location = new System.Drawing.Point(70, 77);
            this._textBox_Username.Name = "_textBox_Username";
            this._textBox_Username.Size = new System.Drawing.Size(421, 20);
            this._textBox_Username.TabIndex = 3;
            this._textBox_Username.TextChanged += new System.EventHandler(this._textBox_TextChanged);
            // 
            // _textBox_Password
            // 
            this._textBox_Password.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._textBox_Password.Location = new System.Drawing.Point(70, 107);
            this._textBox_Password.Name = "_textBox_Password";
            this._textBox_Password.Size = new System.Drawing.Size(421, 20);
            this._textBox_Password.TabIndex = 4;
            this._textBox_Password.UseSystemPasswordChar = true;
            this._textBox_Password.TextChanged += new System.EventHandler(this._textBox_TextChanged);
            // 
            // Form_GetMissingCredentials
            // 
            this.AcceptButton = this.button_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(508, 264);
            this.Description = "Specify credentials for collecting SQL Server and Operating System security infor" +
                "mation.";
            this.MinimumSize = new System.Drawing.Size(300, 260);
            this.Name = "Form_GetMissingCredentials";
            this.Picture = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_CrossServerLoginCheck_48;
            this.Text = "Specify Credentials";
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.Form_PolicyProperties_HelpRequested);
            this._bfd_ButtonPanel.ResumeLayout(false);
            this._bf_MainPanel.ResumeLayout(false);
            this._bf_MainPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton button_OK;
        private Infragistics.Win.Misc.UltraButton button_Cancel;
        private Infragistics.Win.Misc.UltraButton _btn_Help;
        private System.Windows.Forms.TextBox _textBox_Password;
        private System.Windows.Forms.TextBox _textBox_Username;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label _label_DescriptiveText;
    }
}
