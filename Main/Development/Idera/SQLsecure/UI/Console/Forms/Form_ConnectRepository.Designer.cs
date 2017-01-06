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
            Infragistics.Win.ValueListItem connect_repository = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem deploy_repository = new Infragistics.Win.ValueListItem();
            this._label1 = new System.Windows.Forms.Label();
            this._textBox_Server = new System.Windows.Forms.TextBox();
            this._button_Lookup = new Infragistics.Win.Misc.UltraButton();
            this._button_Cancel = new Infragistics.Win.Misc.UltraButton();
            this._button_OK = new Infragistics.Win.Misc.UltraButton();
            this.label1 = new System.Windows.Forms.Label();
            this.ultraButton_Help = new Infragistics.Win.Misc.UltraButton();
            this.action_choice = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            //SQLSecure - (Mitul Kapoor) - label and textbox for usernsame and password.
            this.is_sql_auth_required = new System.Windows.Forms.CheckBox();
            this._username_label = new System.Windows.Forms.Label();
            this._password_label = new System.Windows.Forms.Label();
            this.username = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.TextBox();
            //
            this._bfd_ButtonPanel.SuspendLayout();
            this._bf_MainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.action_choice)).BeginInit();
            this.SuspendLayout();
            // 
            // _bfd_ButtonPanel
            // 
            this._bfd_ButtonPanel.Controls.Add(this.ultraButton_Help);
            this._bfd_ButtonPanel.Controls.Add(this._button_Cancel);
            this._bfd_ButtonPanel.Controls.Add(this._button_OK);
            this._bfd_ButtonPanel.Location = new System.Drawing.Point(0, 279);
            this._bfd_ButtonPanel.Size = new System.Drawing.Size(409, 40);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this._button_OK, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this._button_Cancel, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this.ultraButton_Help, 0);
            // 
            // _bf_MainPanel
            // 
            this._bf_MainPanel.Controls.Add(this.password);
            this._bf_MainPanel.Controls.Add(this.username);
            this._bf_MainPanel.Controls.Add(this._password_label);
            this._bf_MainPanel.Controls.Add(this._username_label);
            this._bf_MainPanel.Controls.Add(this.is_sql_auth_required);
            this._bf_MainPanel.Controls.Add(this.action_choice);
            this._bf_MainPanel.Controls.Add(this.label1);
            this._bf_MainPanel.Controls.Add(this._label1);
            this._bf_MainPanel.Controls.Add(this._textBox_Server);
            this._bf_MainPanel.Controls.Add(this._button_Lookup);
            this._bf_MainPanel.Size = new System.Drawing.Size(409, 226);
            // 
            // _bf_HeaderPanel
            // 
            this._bf_HeaderPanel.Size = new System.Drawing.Size(409, 53);
            // 
            // _label1
            // 
            this._label1.AutoSize = true;
            this._label1.BackColor = System.Drawing.Color.Transparent;
            this._label1.Location = new System.Drawing.Point(12, 78);
            this._label1.Name = "_label1";
            this._label1.Size = new System.Drawing.Size(65, 13);
            this._label1.TabIndex = 0;
            this._label1.Text = "SQL Server:";
            // 
            // _textBox_Server
            // 
            this._textBox_Server.Location = new System.Drawing.Point(83, 76);
            this._textBox_Server.Name = "_textBox_Server";
            this._textBox_Server.Size = new System.Drawing.Size(269, 20);
            this._textBox_Server.TabIndex = 1;
            this._textBox_Server.TextChanged += new System.EventHandler(this._textBox_Server_TextChanged);
            // 
            // _button_Lookup
            // 
            this._button_Lookup.Location = new System.Drawing.Point(358, 73);
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
            this._button_OK.Text = button_value;
            this._button_OK.Click += new System.EventHandler(this._button_OK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(12, 51);
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
            // action_choice
            // 
            this.action_choice.ItemOrigin = new System.Drawing.Point(10, 10);
            connect_repository.DataValue = "Connect";
            connect_repository.DisplayText = "Connect to Repository";
            deploy_repository.DataValue = "Deploy";
            deploy_repository.DisplayText = "Deploy Repository";
            this.action_choice.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            connect_repository,
            deploy_repository});
            this.action_choice.Location = new System.Drawing.Point(0, 0);
            this.action_choice.Margin = new System.Windows.Forms.Padding(20);
            this.action_choice.Name = "action_choice";
            this.action_choice.Size = new System.Drawing.Size(500, 50);
            this.action_choice.TabIndex = 0;
            // 
            // checkBox1
            // 
            this.is_sql_auth_required.AutoSize = true;
            this.is_sql_auth_required.Location = new System.Drawing.Point(20, 113);
            this.is_sql_auth_required.Name = "credentials_required";
            this.is_sql_auth_required.Size = new System.Drawing.Size(175, 17);
            this.is_sql_auth_required.TabIndex = 7;
            this.is_sql_auth_required.Text = "SQL Authentication";
            this.is_sql_auth_required.UseVisualStyleBackColor = true;
            this.is_sql_auth_required.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // _username_label
            // 
            this._username_label.AutoSize = true;
            this._username_label.Location = new System.Drawing.Point(15, 150);
            this._username_label.Name = "_username_label";
            this._username_label.Size = new System.Drawing.Size(60, 13);
            this._username_label.TabIndex = 8;
            this._username_label.Text = "User Name : ";
            // 
            // _password_label
            // 
            this._password_label.AutoSize = true;
            this._password_label.Location = new System.Drawing.Point(20, 184);
            this._password_label.Name = "_password_label";
            this._password_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._password_label.Size = new System.Drawing.Size(53, 13);
            this._password_label.TabIndex = 9;
            this._password_label.Text = "Password : ";
            this._password_label.Click += new System.EventHandler(this.label3_Click);
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(83, 147);
            this.username.Name = "username";
            this.username.Enabled = false;
            this.username.Size = new System.Drawing.Size(269, 20);
            this.username.TabIndex = 10;
            this.username.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(83, 179);
            this.password.Name = "password";
            this.password.Enabled = false;
            this.password.Size = new System.Drawing.Size(268, 20);
            this.password.TabIndex = 11;
            // 
            // Form_ConnectRepository
            // 
            this.AcceptButton = this._button_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(409, 319);
            this.Description = "Connect to SQLsecure Repository";
            this.Name = "Form_ConnectRepository";
            this.Text = "Connect to Repository";
            this.Picture = global::Idera.SQLsecure.UI.Console.Properties.Resources.connect_49;
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.Form_ConnectRepository_HelpRequested);
            this._bfd_ButtonPanel.ResumeLayout(false);
            this._bf_MainPanel.ResumeLayout(false);
            this._bf_MainPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.action_choice)).EndInit();
            this.ResumeLayout(false);
            //
            // Radio button for choice of action(Connect to Repository/Deploy Repository)
            //
            this.action_choice.Items.Clear();
            this.action_choice.Width = 500;
            this.action_choice.Height = 50;
            this.action_choice.Margin = new System.Windows.Forms.Padding(20);
            this.action_choice.ItemSpacingVertical += 10;
            this.action_choice.Items.Add("Connect", "Connect to Repository");
            this.action_choice.Items.Add("Deploy", "Deploy Repository");
            this.action_choice.ItemOrigin = new System.Drawing.Point(10, 10);
            this.action_choice.CheckedIndex = button_index;
            this.action_choice.ValueChanged += Action_choice_ValueChanged;
            

        }
        
        #endregion

        private System.Windows.Forms.Label _label1;
        private System.Windows.Forms.TextBox _textBox_Server;
        private Infragistics.Win.Misc.UltraButton _button_Lookup;
        private Infragistics.Win.Misc.UltraButton _button_Cancel;
        private Infragistics.Win.Misc.UltraButton _button_OK;
        private System.Windows.Forms.Label label1;
        private Infragistics.Win.Misc.UltraButton ultraButton_Help;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet action_choice;
        private System.Windows.Forms.CheckBox is_sql_auth_required;
        private System.Windows.Forms.Label _password_label;
        private System.Windows.Forms.Label _username_label;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.TextBox username;
    }
}
