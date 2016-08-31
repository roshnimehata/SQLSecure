namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_SelectFolder
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
            this._button_Cancel = new System.Windows.Forms.Button();
            this._button_OK = new System.Windows.Forms.Button();
            this._listView_Folders = new System.Windows.Forms.ListView();
            this._col_Folder = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // _button_Cancel
            // 
            this._button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._button_Cancel.Location = new System.Drawing.Point(259, 188);
            this._button_Cancel.Name = "_button_Cancel";
            this._button_Cancel.Size = new System.Drawing.Size(75, 23);
            this._button_Cancel.TabIndex = 2;
            this._button_Cancel.Text = "&Cancel";
            this._button_Cancel.UseVisualStyleBackColor = true;
            // 
            // _button_OK
            // 
            this._button_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._button_OK.Enabled = false;
            this._button_OK.Location = new System.Drawing.Point(178, 188);
            this._button_OK.Name = "_button_OK";
            this._button_OK.Size = new System.Drawing.Size(75, 23);
            this._button_OK.TabIndex = 1;
            this._button_OK.Text = "&OK";
            this._button_OK.UseVisualStyleBackColor = true;
            this._button_OK.Click += new System.EventHandler(this._button_OK_Click);
            // 
            // _listView_Folders
            // 
            this._listView_Folders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._listView_Folders.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._col_Folder});
            this._listView_Folders.FullRowSelect = true;
            this._listView_Folders.HideSelection = false;
            this._listView_Folders.LabelWrap = false;
            this._listView_Folders.Location = new System.Drawing.Point(12, 12);
            this._listView_Folders.MultiSelect = false;
            this._listView_Folders.Name = "_listView_Folders";
            this._listView_Folders.Size = new System.Drawing.Size(322, 166);
            this._listView_Folders.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this._listView_Folders.TabIndex = 0;
            this._listView_Folders.UseCompatibleStateImageBehavior = false;
            this._listView_Folders.View = System.Windows.Forms.View.Details;
            this._listView_Folders.SelectedIndexChanged += new System.EventHandler(this._listView_Folders_SelectedIndexChanged);
            this._listView_Folders.DoubleClick += new System.EventHandler(this._listView_Folders_DoubleClick);
            // 
            // _col_Folder
            // 
            this._col_Folder.Text = "Folder";
            this._col_Folder.Width = 318;
            // 
            // Form_SelectFolder
            // 
            this.AcceptButton = this._button_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.CancelButton = this._button_Cancel;
            this.ClientSize = new System.Drawing.Size(346, 219);
            this.Controls.Add(this._button_Cancel);
            this.Controls.Add(this._button_OK);
            this.Controls.Add(this._listView_Folders);
            this.MinimumSize = new System.Drawing.Size(300, 200);
            this.Name = "Form_SelectFolder";
            this.Text = "Select Target Folder";
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.Form_SelectFolder_HelpRequested);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _button_Cancel;
        private System.Windows.Forms.Button _button_OK;
        private System.Windows.Forms.ListView _listView_Folders;
        private System.Windows.Forms.ColumnHeader _col_Folder;
    }
}
