namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_ConnectRepository
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
            this._label1 = new System.Windows.Forms.Label();
            this._textBox_Server = new System.Windows.Forms.TextBox();
            this._button_Lookup = new Infragistics.Win.Misc.UltraButton();
            this._button_Cancel = new Infragistics.Win.Misc.UltraButton();
            this._button_OK = new Infragistics.Win.Misc.UltraButton();
            this.label1 = new System.Windows.Forms.Label();
            this.ultraButton_Help = new Infragistics.Win.Misc.UltraButton();
            this._bfd_ButtonPanel.SuspendLayout();
            this._bf_MainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _bfd_ButtonPanel
            // 
            this._bfd_ButtonPanel.Controls.Add(this.ultraButton_Help);
            this._bfd_ButtonPanel.Controls.Add(this._button_Cancel);
            this._bfd_ButtonPanel.Controls.Add(this._button_OK);
            this._bfd_ButtonPanel.Location = new System.Drawing.Point(0, 152);
            this._bfd_ButtonPanel.Size = new System.Drawing.Size(394, 40);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this._button_OK, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this._button_Cancel, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this.ultraButton_Help, 0);
            // 
            // _bf_MainPanel
            // 
            this._bf_MainPanel.Controls.Add(this.label1);
            this._bf_MainPanel.Controls.Add(this._label1);
            this._bf_MainPanel.Controls.Add(this._textBox_Server);
            this._bf_MainPanel.Controls.Add(this._button_Lookup);
            this._bf_MainPanel.Size = new System.Drawing.Size(394, 99);
            // 
            // _bf_HeaderPanel
            // 
            this._bf_HeaderPanel.Size = new System.Drawing.Size(394, 53);
            // 
            // _label1
            // 
            this._label1.AutoSize = true;
            this._label1.BackColor = System.Drawing.Color.Transparent;
            this._label1.Location = new System.Drawing.Point(12, 48);
            this._label1.Name = "_label1";
            this._label1.Size = new System.Drawing.Size(65, 13);
            this._label1.TabIndex = 0;
            this._label1.Text = "&SQL Server:";
            // 
            // _textBox_Server
            // 
            this._textBox_Server.Location = new System.Drawing.Point(83, 46);
            this._textBox_Server.Name = "_textBox_Server";
            this._textBox_Server.Size = new System.Drawing.Size(269, 20);
            this._textBox_Server.TabIndex = 1;
            this._textBox_Server.TextChanged += new System.EventHandler(this._textBox_Server_TextChanged);
            // 
            // _button_Lookup
            // 
            this._button_Lookup.Location = new System.Drawing.Point(358, 43);
            this._button_Lookup.Name = "_button_Lookup";
            this._button_Lookup.Size = new System.Drawing.Size(24, 23);
            this._button_Lookup.TabIndex = 2;
            this._button_Lookup.Text = ".&..";
            this._button_Lookup.Click += new System.EventHandler(this._button_Lookup_Click);
            // 
            // _button_Cancel
            // 
            this._button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._button_Cancel.Location = new System.Drawing.Point(226, 9);
            this._button_Cancel.Name = "_button_Cancel";
            this._button_Cancel.Size = new System.Drawing.Size(75, 23);
            this._button_Cancel.TabIndex = 4;
            this._button_Cancel.Text = "&Cancel";
            // 
            // _button_OK
            // 
            this._button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._button_OK.Enabled = false;
            this._button_OK.Location = new System.Drawing.Point(145, 9);
            this._button_OK.Name = "_button_OK";
            this._button_OK.Size = new System.Drawing.Size(75, 23);
            this._button_OK.TabIndex = 3;
            this._button_OK.Text = "C&onnect";
            this._button_OK.Click += new System.EventHandler(this._button_OK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(346, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Enter the name of the SQL Server that hosts the SQLsecure Repository.";
            // 
            // ultraButton_Help
            // 
            this.ultraButton_Help.Location = new System.Drawing.Point(307, 9);
            this.ultraButton_Help.Name = "ultraButton_Help";
            this.ultraButton_Help.Size = new System.Drawing.Size(75, 23);
            this.ultraButton_Help.TabIndex = 5;
            this.ultraButton_Help.Text = "&Help";
            this.ultraButton_Help.Click += new System.EventHandler(this.ultraButton_Help_Click);
            // 
            // Form_ConnectRepository
            // 
            this.AcceptButton = this._button_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(394, 192);
            this.Description = "Connect to SQLsecure Repository";
            this.Name = "Form_ConnectRepository";
            this.Picture = global::Idera.SQLsecure.UI.Console.Properties.Resources.connect_49;
            this.Text = "Connect to Repository";
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.Form_ConnectRepository_HelpRequested);
            this._bfd_ButtonPanel.ResumeLayout(false);
            this._bf_MainPanel.ResumeLayout(false);
            this._bf_MainPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label _label1;
        private System.Windows.Forms.TextBox _textBox_Server;
        private Infragistics.Win.Misc.UltraButton _button_Lookup;
        private Infragistics.Win.Misc.UltraButton _button_Cancel;
        private Infragistics.Win.Misc.UltraButton _button_OK;
        private System.Windows.Forms.Label label1;
        private Infragistics.Win.Misc.UltraButton ultraButton_Help;
    }
}
