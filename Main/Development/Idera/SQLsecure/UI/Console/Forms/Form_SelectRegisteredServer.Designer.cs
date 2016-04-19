namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_SelectRegisteredServer
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
            this._listView_Servers = new System.Windows.Forms.ListView();
            this._col_Server = new System.Windows.Forms.ColumnHeader();
            this.button_Help = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _button_Cancel
            // 
            this._button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._button_Cancel.Location = new System.Drawing.Point(159, 317);
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
            this._button_OK.Location = new System.Drawing.Point(77, 317);
            this._button_OK.Name = "_button_OK";
            this._button_OK.Size = new System.Drawing.Size(75, 23);
            this._button_OK.TabIndex = 1;
            this._button_OK.Text = "&OK";
            this._button_OK.UseVisualStyleBackColor = true;
            this._button_OK.Click += new System.EventHandler(this._button_OK_Click);
            // 
            // _listView_Servers
            // 
            this._listView_Servers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._listView_Servers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._col_Server});
            this._listView_Servers.FullRowSelect = true;
            this._listView_Servers.HideSelection = false;
            this._listView_Servers.LabelWrap = false;
            this._listView_Servers.Location = new System.Drawing.Point(12, 12);
            this._listView_Servers.MultiSelect = false;
            this._listView_Servers.Name = "_listView_Servers";
            this._listView_Servers.Size = new System.Drawing.Size(305, 299);
            this._listView_Servers.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this._listView_Servers.TabIndex = 0;
            this._listView_Servers.UseCompatibleStateImageBehavior = false;
            this._listView_Servers.View = System.Windows.Forms.View.Details;
            this._listView_Servers.DoubleClick += new System.EventHandler(this._listView_Servers_DoubleClick);
            this._listView_Servers.SelectedIndexChanged += new System.EventHandler(this._listView_Servers_SelectedIndexChanged);
            // 
            // _col_Server
            // 
            this._col_Server.Text = "Server";
            this._col_Server.Width = 300;
            // 
            // button_Help
            // 
            this.button_Help.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Help.Location = new System.Drawing.Point(242, 317);
            this.button_Help.Name = "button_Help";
            this.button_Help.Size = new System.Drawing.Size(75, 23);
            this.button_Help.TabIndex = 3;
            this.button_Help.Text = "&Help";
            this.button_Help.UseVisualStyleBackColor = true;
            this.button_Help.Click += new System.EventHandler(this.button_Help_Click);
            // 
            // Form_SelectRegisteredServer
            // 
            this.AcceptButton = this._button_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.CancelButton = this._button_Cancel;
            this.ClientSize = new System.Drawing.Size(329, 352);
            this.Controls.Add(this.button_Help);
            this.Controls.Add(this._button_Cancel);
            this.Controls.Add(this._button_OK);
            this.Controls.Add(this._listView_Servers);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(270, 200);
            this.Name = "Form_SelectRegisteredServer";
            this.Text = "Select Audited SQL Server";
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.Form_SelectRegisteredServer_HelpRequested);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _button_Cancel;
        private System.Windows.Forms.Button _button_OK;
        private System.Windows.Forms.ListView _listView_Servers;
        private System.Windows.Forms.ColumnHeader _col_Server;
        private System.Windows.Forms.Button button_Help;
    }
}
