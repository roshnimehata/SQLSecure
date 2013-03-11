namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_SelectServer
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
            this._label1 = new System.Windows.Forms.Label();
            this._listView_Servers = new System.Windows.Forms.ListView();
            this._col_Server = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // _button_OK
            // 
            this._button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._button_OK.Enabled = false;
            this._button_OK.Location = new System.Drawing.Point(136, 308);
            this._button_OK.Name = "_button_OK";
            this._button_OK.Size = new System.Drawing.Size(75, 23);
            this._button_OK.TabIndex = 2;
            this._button_OK.Text = "&OK";
            this._button_OK.UseVisualStyleBackColor = true;
            this._button_OK.Click += new System.EventHandler(this._button_OK_Click);
            // 
            // _button_Cancel
            // 
            this._button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._button_Cancel.Location = new System.Drawing.Point(217, 308);
            this._button_Cancel.Name = "_button_Cancel";
            this._button_Cancel.Size = new System.Drawing.Size(75, 23);
            this._button_Cancel.TabIndex = 3;
            this._button_Cancel.Text = "&Cancel";
            this._button_Cancel.UseVisualStyleBackColor = true;
            // 
            // _label1
            // 
            this._label1.AutoSize = true;
            this._label1.Location = new System.Drawing.Point(12, 9);
            this._label1.Name = "_label1";
            this._label1.Size = new System.Drawing.Size(220, 13);
            this._label1.TabIndex = 0;
            this._label1.Text = "&Select a SQL Server instance in the network:";
            // 
            // _listView_Servers
            // 
            this._listView_Servers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._col_Server});
            this._listView_Servers.FullRowSelect = true;
            this._listView_Servers.HideSelection = false;
            this._listView_Servers.LabelWrap = false;
            this._listView_Servers.Location = new System.Drawing.Point(12, 35);
            this._listView_Servers.MultiSelect = false;
            this._listView_Servers.Name = "_listView_Servers";
            this._listView_Servers.Size = new System.Drawing.Size(280, 262);
            this._listView_Servers.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this._listView_Servers.TabIndex = 1;
            this._listView_Servers.UseCompatibleStateImageBehavior = false;
            this._listView_Servers.View = System.Windows.Forms.View.Details;
            this._listView_Servers.DoubleClick += new System.EventHandler(this._listView_Servers_DoubleClick);
            this._listView_Servers.SelectedIndexChanged += new System.EventHandler(this._listView_Servers_SelectedIndexChanged);
            // 
            // _col_Server
            // 
            this._col_Server.Text = "Server";
            this._col_Server.Width = 259;
            // 
            // Form_SelectServer
            // 
            this.AcceptButton = this._button_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.CancelButton = this._button_Cancel;
            this.ClientSize = new System.Drawing.Size(304, 341);
            this.Controls.Add(this._button_Cancel);
            this.Controls.Add(this._label1);
            this.Controls.Add(this._button_OK);
            this.Controls.Add(this._listView_Servers);
            this.Name = "Form_SelectServer";
            this.Text = "Select SQL Server";
            this.Load += new System.EventHandler(this.Form_SelectServer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _button_OK;
        private System.Windows.Forms.Button _button_Cancel;
        private System.Windows.Forms.Label _label1;
        private System.Windows.Forms.ListView _listView_Servers;
        private System.Windows.Forms.ColumnHeader _col_Server;
    }
}
