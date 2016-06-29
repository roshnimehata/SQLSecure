namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_SelectDatabase
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
            this._button_OK = new System.Windows.Forms.Button();
            this._button_Cancel = new System.Windows.Forms.Button();
            this._listView_Databases = new System.Windows.Forms.ListView();
            this._col_Database = new System.Windows.Forms.ColumnHeader();
            this.button_Help = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _button_OK
            // 
            this._button_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._button_OK.Enabled = false;
            this._button_OK.Location = new System.Drawing.Point(76, 247);
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
            this._button_Cancel.Location = new System.Drawing.Point(156, 247);
            this._button_Cancel.Name = "_button_Cancel";
            this._button_Cancel.Size = new System.Drawing.Size(75, 23);
            this._button_Cancel.TabIndex = 2;
            this._button_Cancel.Text = "&Cancel";
            this._button_Cancel.UseVisualStyleBackColor = true;
            // 
            // _listView_Databases
            // 
            this._listView_Databases.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._listView_Databases.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._col_Database});
            this._listView_Databases.FullRowSelect = true;
            this._listView_Databases.LabelWrap = false;
            this._listView_Databases.Location = new System.Drawing.Point(12, 12);
            this._listView_Databases.MultiSelect = false;
            this._listView_Databases.Name = "_listView_Databases";
            this._listView_Databases.Size = new System.Drawing.Size(300, 229);
            this._listView_Databases.TabIndex = 0;
            this._listView_Databases.UseCompatibleStateImageBehavior = false;
            this._listView_Databases.View = System.Windows.Forms.View.Details;
            this._listView_Databases.DoubleClick += new System.EventHandler(this._listView_Databases_DoubleClick);
            this._listView_Databases.SelectedIndexChanged += new System.EventHandler(this._listView_Databases_SelectedIndexChanged);
            // 
            // _col_Database
            // 
            this._col_Database.Text = "Database";
            this._col_Database.Width = 294;
            // 
            // button_Help
            // 
            this.button_Help.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Help.Location = new System.Drawing.Point(237, 247);
            this.button_Help.Name = "button_Help";
            this.button_Help.Size = new System.Drawing.Size(75, 23);
            this.button_Help.TabIndex = 3;
            this.button_Help.Text = "&Help";
            this.button_Help.UseVisualStyleBackColor = true;
            this.button_Help.Click += new System.EventHandler(this.button_Help_Click);
            // 
            // Form_SelectDatabase
            // 
            this.AcceptButton = this._button_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.CancelButton = this._button_Cancel;
            this.ClientSize = new System.Drawing.Size(324, 282);
            this.Controls.Add(this.button_Help);
            this.Controls.Add(this._button_OK);
            this.Controls.Add(this._button_Cancel);
            this.Controls.Add(this._listView_Databases);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MinimumSize = new System.Drawing.Size(270, 200);
            this.Name = "Form_SelectDatabase";
            this.Text = "Select Database";
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.Form_SelectDatabase_HelpRequested);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _button_OK;
        private System.Windows.Forms.Button _button_Cancel;
        private System.Windows.Forms.ListView _listView_Databases;
        private System.Windows.Forms.ColumnHeader _col_Database;
        private System.Windows.Forms.Button button_Help;
    }
}
