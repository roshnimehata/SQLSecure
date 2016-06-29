namespace Idera.SQLsecure.UI.Console.Controls
{
    partial class AddEditFolders
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._btn_Edit_Folder = new Infragistics.Win.Misc.UltraButton();
            this._btn_Remove_Folder = new Infragistics.Win.Misc.UltraButton();
            this._btn_Add_Folder = new Infragistics.Win.Misc.UltraButton();
            this.lstFilePermissionFolders = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // _btn_Edit_Folder
            // 
            this._btn_Edit_Folder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._btn_Edit_Folder.Enabled = false;
            this._btn_Edit_Folder.Location = new System.Drawing.Point(81, 362);
            this._btn_Edit_Folder.Name = "_btn_Edit_Folder";
            this._btn_Edit_Folder.Size = new System.Drawing.Size(75, 23);
            this._btn_Edit_Folder.TabIndex = 10;
            this._btn_Edit_Folder.Text = "Edit";
            this._btn_Edit_Folder.Click += new System.EventHandler(this._btn_Edit_Folder_Click);
            this._btn_Edit_Folder.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lstFilePermissionFolders_MouseClick);
            // 
            // _btn_Remove_Folder
            // 
            this._btn_Remove_Folder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._btn_Remove_Folder.Enabled = false;
            this._btn_Remove_Folder.Location = new System.Drawing.Point(162, 362);
            this._btn_Remove_Folder.Name = "_btn_Remove_Folder";
            this._btn_Remove_Folder.Size = new System.Drawing.Size(75, 23);
            this._btn_Remove_Folder.TabIndex = 9;
            this._btn_Remove_Folder.Text = "Remove";
            this._btn_Remove_Folder.Click += new System.EventHandler(this._btn_Remove_Folder_Click);
            this._btn_Remove_Folder.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lstFilePermissionFolders_MouseClick);
            // 
            // _btn_Add_Folder
            // 
            this._btn_Add_Folder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._btn_Add_Folder.Location = new System.Drawing.Point(0, 362);
            this._btn_Add_Folder.Name = "_btn_Add_Folder";
            this._btn_Add_Folder.Size = new System.Drawing.Size(75, 23);
            this._btn_Add_Folder.TabIndex = 8;
            this._btn_Add_Folder.Text = "Add";
            this._btn_Add_Folder.Click += new System.EventHandler(this._btn_Add_Folder_Click);
            this._btn_Add_Folder.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lstFilePermissionFolders_MouseClick);
            // 
            // lstFilePermissionFolders
            // 
            this.lstFilePermissionFolders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstFilePermissionFolders.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstFilePermissionFolders.FormattingEnabled = true;
            this.lstFilePermissionFolders.Location = new System.Drawing.Point(0, 0);
            this.lstFilePermissionFolders.Margin = new System.Windows.Forms.Padding(0);
            this.lstFilePermissionFolders.Name = "lstFilePermissionFolders";
            this.lstFilePermissionFolders.Size = new System.Drawing.Size(470, 353);
            this.lstFilePermissionFolders.TabIndex = 6;
            this.lstFilePermissionFolders.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lstFilePermissionFolders_MouseClick);
            // 
            // AddEditFolders
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._btn_Edit_Folder);
            this.Controls.Add(this._btn_Remove_Folder);
            this.Controls.Add(this._btn_Add_Folder);
            this.Controls.Add(this.lstFilePermissionFolders);
            this.Name = "AddEditFolders";
            this.Size = new System.Drawing.Size(470, 388);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton _btn_Edit_Folder;
        private Infragistics.Win.Misc.UltraButton _btn_Remove_Folder;
        private Infragistics.Win.Misc.UltraButton _btn_Add_Folder;
        private System.Windows.Forms.ListBox lstFilePermissionFolders;
    }
}
