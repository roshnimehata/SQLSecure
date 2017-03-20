using System;

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
            this._username_label = new System.Windows.Forms.Label();
            this._password_label = new System.Windows.Forms.Label();
            this.username = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.TextBox();
            //this.azure_authentication = new System.Windows.Forms.RadioButton();
            this.sql_authentication = new System.Windows.Forms.RadioButton();
            this.windows_authentication = new System.Windows.Forms.RadioButton();
            this._bfd_ButtonPanel.SuspendLayout();
            this._bf_MainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _bfd_ButtonPanel
            // 
            this._bfd_ButtonPanel.Controls.Add(this.ultraButton_Help);
            this._bfd_ButtonPanel.Controls.Add(this._button_Cancel);
            this._bfd_ButtonPanel.Controls.Add(this._button_OK);
            this._bfd_ButtonPanel.Location = new System.Drawing.Point(0, 254);
            this._bfd_ButtonPanel.Size = new System.Drawing.Size(451, 40);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this._button_OK, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this._button_Cancel, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this.ultraButton_Help, 0);
            // 
            // _bf_MainPanel
            // 
            this._bf_MainPanel.Controls.Add(this.windows_authentication);
            this._bf_MainPanel.Controls.Add(this.sql_authentication);
            //this._bf_MainPanel.Controls.Add(this.azure_authentication);
            this._bf_MainPanel.Controls.Add(this.password);
            this._bf_MainPanel.Controls.Add(this.username);
            this._bf_MainPanel.Controls.Add(this._password_label);
            this._bf_MainPanel.Controls.Add(this._username_label);
            //this._bf_MainPanel.Controls.Add(this.action_choice);
            this._bf_MainPanel.Controls.Add(this.label1);
            this._bf_MainPanel.Controls.Add(this._label1);
            this._bf_MainPanel.Controls.Add(this._textBox_Server);
            this._bf_MainPanel.Controls.Add(this._button_Lookup);
            this._bf_MainPanel.Size = new System.Drawing.Size(451, 201);
            // 
            // _bf_HeaderPanel
            // 
            this._bf_HeaderPanel.Size = new System.Drawing.Size(451, 53);
            // 
            // _label1
            // 
            this._label1.AutoSize = true;
            this._label1.BackColor = System.Drawing.Color.Transparent;
            this._label1.Location = new System.Drawing.Point(12, 48);
            this._label1.Name = "_label1";
            this._label1.Size = new System.Drawing.Size(65, 13);
            this._label1.TabIndex = 0;
            this._label1.Text = "SQL Server:";
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
            this._button_Cancel.Location = new System.Drawing.Point(276, 9);
            this._button_Cancel.Name = "_button_Cancel";
            this._button_Cancel.Size = new System.Drawing.Size(75, 23);
            this._button_Cancel.TabIndex = 4;
            this._button_Cancel.Text = "&Cancel";
            // 
            // _button_OK
            // 
            this._button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._button_OK.Enabled = false;
            this._button_OK.Location = new System.Drawing.Point(194, 9);
            this._button_OK.Name = "_button_OK";
            this._button_OK.Size = new System.Drawing.Size(75, 23);
            this._button_OK.TabIndex = 3;
            this._button_OK.Text = button_value;
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
            this.ultraButton_Help.Location = new System.Drawing.Point(358, 9);
            this.ultraButton_Help.Name = "ultraButton_Help";
            this.ultraButton_Help.Size = new System.Drawing.Size(75, 23);
            this.ultraButton_Help.TabIndex = 5;
            this.ultraButton_Help.Text = "&Help";
            this.ultraButton_Help.Click += new System.EventHandler(this.ultraButton_Help_Click);
            // 
            // _username_label
            // 
            this._username_label.AutoSize = true;
            this._username_label.Location = new System.Drawing.Point(15, 120);
            this._username_label.Name = "_username_label";
            this._username_label.Size = new System.Drawing.Size(69, 13);
            this._username_label.TabIndex = 8;
            this._username_label.Text = "User Name : ";
            // 
            // _password_label
            // 
            this._password_label.AutoSize = true;
            this._password_label.Location = new System.Drawing.Point(20, 154);
            this._password_label.Name = "_password_label";
            this._password_label.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._password_label.Size = new System.Drawing.Size(62, 13);
            this._password_label.TabIndex = 9;
            this._password_label.Text = "Password : ";
            this._password_label.Click += new System.EventHandler(this.label3_Click);
            // 
            // username
            // 
            this.username.Enabled = true;
            this.username.Location = new System.Drawing.Point(83, 117);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(269, 20);
            this.username.TabIndex = 10;
            this.username.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // password
            // 
            this.password.Enabled = true;
            this.password.Location = new System.Drawing.Point(83, 149);
            this.password.Name = "password";
            this.password.PasswordChar = '*';
            this.password.Size = new System.Drawing.Size(268, 20);
            this.password.TabIndex = 11;
            // 
            // azure_authentication
            // 
            //this.azure_authentication.AutoSize = true;
            //this.azure_authentication.Location = new System.Drawing.Point(9, 83);
            //this.azure_authentication.Name = "azure_authentication";
            //this.azure_authentication.Size = new System.Drawing.Size(70, 17);
            //this.azure_authentication.TabIndex = 12;
            //this.azure_authentication.TabStop = true;
            //this.azure_authentication.Text = "Azure AD";
            //this.azure_authentication.UseVisualStyleBackColor = true;
            //this.azure_authentication.CheckedChanged += new System.EventHandler(this.azure_authentication_CheckedChanged);
            // 
            // sql_authentication
            // 
            this.sql_authentication.AutoSize = true;
            this.sql_authentication.Location = new System.Drawing.Point(77, 83);
            this.sql_authentication.Name = "sql_authentication";
            this.sql_authentication.Size = new System.Drawing.Size(117, 17);
            this.sql_authentication.TabIndex = 13;
            this.sql_authentication.TabStop = true;
            this.sql_authentication.Text = "SQL Authentication";
            this.sql_authentication.UseVisualStyleBackColor = true;
            this.sql_authentication.CheckedChanged += new System.EventHandler(this.sql_authentication_CheckedChanged);
            // 
            // windows_authentication
            // 
            this.windows_authentication.AutoSize = true;
            this.windows_authentication.Location = new System.Drawing.Point(194, 83);
            this.windows_authentication.Name = "windows_authentication";
            this.windows_authentication.Size = new System.Drawing.Size(140, 17);
            this.windows_authentication.TabIndex = 14;
            this.windows_authentication.TabStop = true;
            this.windows_authentication.Text = "Windows Authentication";
            this.windows_authentication.UseVisualStyleBackColor = true;
            this.windows_authentication.Checked = true;//SQLsecure 3.1 (Tushar)Fix for defect SQLSECU-1514--Making windows authentication checked by default.
            this.windows_authentication.CheckedChanged += new System.EventHandler(this.windows_authentication_CheckedChanged);
            // 
            // Form_ConnectRepository
            // 
            this.AcceptButton = this._button_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(451, 324);
            this.Description = "Connect to SQLsecure Repository";
            this.Name = "Form_ConnectRepository";
            this.Picture = global::Idera.SQLsecure.UI.Console.Properties.Resources.connect_49;
            this.Text = "Connect to Repository";
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.Form_ConnectRepository_HelpRequested);
            this.Load += new System.EventHandler(this.Form_Load);
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
        private System.Windows.Forms.Label _password_label;
        private System.Windows.Forms.Label _username_label;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.RadioButton sql_authentication;
        private System.Windows.Forms.RadioButton azure_authentication;
        private System.Windows.Forms.RadioButton windows_authentication;
    }
}
