namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_SelectRole
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
            this.button_Help = new System.Windows.Forms.Button();
            this._button_OK = new System.Windows.Forms.Button();
            this._button_Cancel = new System.Windows.Forms.Button();
            this._listView_Roles = new System.Windows.Forms.ListView();
            this._col_Role = new System.Windows.Forms.ColumnHeader();
            this._col_Type = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // button_Help
            // 
            this.button_Help.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Help.Location = new System.Drawing.Point(238, 248);
            this.button_Help.Name = "button_Help";
            this.button_Help.Size = new System.Drawing.Size(75, 23);
            this.button_Help.TabIndex = 3;
            this.button_Help.Text = "&Help";
            this.button_Help.UseVisualStyleBackColor = true;
            this.button_Help.Click += new System.EventHandler(this.button_Help_Click);
            // 
            // _button_OK
            // 
            this._button_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._button_OK.Enabled = false;
            this._button_OK.Location = new System.Drawing.Point(77, 248);
            this._button_OK.Name = "_button_OK";
            this._button_OK.Size = new System.Drawing.Size(75, 23);
            this._button_OK.TabIndex = 1;
            this._button_OK.Text = "&OK";
            this._button_OK.UseVisualStyleBackColor = true;
            this._button_OK.Click += new System.EventHandler(this._button_OK_Click);
            // 
            // _button_Cancel
            // 
            this._button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._button_Cancel.Location = new System.Drawing.Point(157, 248);
            this._button_Cancel.Name = "_button_Cancel";
            this._button_Cancel.Size = new System.Drawing.Size(75, 23);
            this._button_Cancel.TabIndex = 2;
            this._button_Cancel.Text = "&Cancel";
            this._button_Cancel.UseVisualStyleBackColor = true;
            // 
            // _listView_Roles
            // 
            this._listView_Roles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._listView_Roles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._col_Role,
            this._col_Type});
            this._listView_Roles.FullRowSelect = true;
            this._listView_Roles.LabelWrap = false;
            this._listView_Roles.Location = new System.Drawing.Point(13, 13);
            this._listView_Roles.MultiSelect = false;
            this._listView_Roles.Name = "_listView_Roles";
            this._listView_Roles.Size = new System.Drawing.Size(300, 229);
            this._listView_Roles.TabIndex = 0;
            this._listView_Roles.UseCompatibleStateImageBehavior = false;
            this._listView_Roles.View = System.Windows.Forms.View.Details;
            this._listView_Roles.DoubleClick += new System.EventHandler(this._listView_Roles_DoubleClick);
            this._listView_Roles.SelectedIndexChanged += new System.EventHandler(this._listView_Roles_SelectedIndexChanged);
            // 
            // _col_Role
            // 
            this._col_Role.Text = "Role";
            this._col_Role.Width = 205;
            // 
            // _col_Type
            // 
            this._col_Type.Text = "Type";
            this._col_Type.Width = 66;
            // 
            // Form_SelectRole
            // 
            this.AcceptButton = this._button_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.CancelButton = this._button_Cancel;
            this.ClientSize = new System.Drawing.Size(326, 284);
            this.Controls.Add(this.button_Help);
            this.Controls.Add(this._button_OK);
            this.Controls.Add(this._button_Cancel);
            this.Controls.Add(this._listView_Roles);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MinimumSize = new System.Drawing.Size(270, 200);
            this.Name = "Form_SelectRole";
            this.Text = "Select Role";
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.Form_SelectRole_HelpRequested);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_Help;
        private System.Windows.Forms.Button _button_OK;
        private System.Windows.Forms.Button _button_Cancel;
        private System.Windows.Forms.ListView _listView_Roles;
        private System.Windows.Forms.ColumnHeader _col_Role;
        private System.Windows.Forms.ColumnHeader _col_Type;

    }
}
