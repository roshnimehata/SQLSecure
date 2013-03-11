namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_DeleteSnapshot
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
            this.label1 = new System.Windows.Forms.Label();
            this._listView_Servers = new System.Windows.Forms.ListView();
            this._col_DateTime = new System.Windows.Forms.ColumnHeader();
            this._col_Baseline = new System.Windows.Forms.ColumnHeader();
            this._col_Status = new System.Windows.Forms.ColumnHeader();
            this._col_Msg = new System.Windows.Forms.ColumnHeader();
            this._button_Delete = new System.Windows.Forms.Button();
            this._button_Cancel = new System.Windows.Forms.Button();
            this._button_Help = new System.Windows.Forms.Button();
            this._button_OK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(193, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "The following Snapshots will be deleted";
            // 
            // _listView_Servers
            // 
            this._listView_Servers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._listView_Servers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._col_DateTime,
            this._col_Baseline,
            this._col_Status,
            this._col_Msg});
            this._listView_Servers.FullRowSelect = true;
            this._listView_Servers.LabelWrap = false;
            this._listView_Servers.Location = new System.Drawing.Point(20, 30);
            this._listView_Servers.MultiSelect = false;
            this._listView_Servers.Name = "_listView_Servers";
            this._listView_Servers.ShowItemToolTips = true;
            this._listView_Servers.Size = new System.Drawing.Size(437, 194);
            this._listView_Servers.TabIndex = 1;
            this._listView_Servers.UseCompatibleStateImageBehavior = false;
            this._listView_Servers.View = System.Windows.Forms.View.Details;
            this._listView_Servers.Resize += new System.EventHandler(this._listView_Servers_Resize);
            // 
            // _col_DateTime
            // 
            this._col_DateTime.Text = "Snapshot Taken";
            this._col_DateTime.Width = 199;
            // 
            // _col_Baseline
            // 
            this._col_Baseline.Text = "Baseline";
            this._col_Baseline.Width = 55;
            // 
            // _col_Status
            // 
            this._col_Status.Text = "Delete Status";
            this._col_Status.Width = 79;
            // 
            // _col_Msg
            // 
            this._col_Msg.Text = "Message";
            this._col_Msg.Width = 100;
            // 
            // _button_Delete
            // 
            this._button_Delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._button_Delete.Location = new System.Drawing.Point(220, 234);
            this._button_Delete.Name = "_button_Delete";
            this._button_Delete.Size = new System.Drawing.Size(75, 23);
            this._button_Delete.TabIndex = 2;
            this._button_Delete.Text = "&Delete";
            this._button_Delete.UseVisualStyleBackColor = true;
            this._button_Delete.Click += new System.EventHandler(this._button_Delete_Click);
            // 
            // _button_Cancel
            // 
            this._button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._button_Cancel.Location = new System.Drawing.Point(301, 234);
            this._button_Cancel.Name = "_button_Cancel";
            this._button_Cancel.Size = new System.Drawing.Size(75, 23);
            this._button_Cancel.TabIndex = 3;
            this._button_Cancel.Text = "&Cancel";
            this._button_Cancel.UseVisualStyleBackColor = true;
            // 
            // _button_Help
            // 
            this._button_Help.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._button_Help.Location = new System.Drawing.Point(382, 234);
            this._button_Help.Name = "_button_Help";
            this._button_Help.Size = new System.Drawing.Size(75, 23);
            this._button_Help.TabIndex = 4;
            this._button_Help.Text = "&Help";
            this._button_Help.UseVisualStyleBackColor = true;
            this._button_Help.Click += new System.EventHandler(this._button_Help_Click);
            // 
            // _button_OK
            // 
            this._button_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._button_OK.Location = new System.Drawing.Point(21, 234);
            this._button_OK.Name = "_button_OK";
            this._button_OK.Size = new System.Drawing.Size(75, 23);
            this._button_OK.TabIndex = 12;
            this._button_OK.Text = "&Close";
            this._button_OK.UseVisualStyleBackColor = true;
            this._button_OK.Visible = false;
            // 
            // Form_DeleteSnapshot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.CancelButton = this._button_Cancel;
            this.ClientSize = new System.Drawing.Size(477, 268);
            this.Controls.Add(this._button_OK);
            this.Controls.Add(this._button_Help);
            this.Controls.Add(this._button_Delete);
            this.Controls.Add(this._button_Cancel);
            this.Controls.Add(this._listView_Servers);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MinimumSize = new System.Drawing.Size(485, 195);
            this.Name = "Form_DeleteSnapshot";
            this.Text = "Delete Snapshots";
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.Form_DeleteSnapshot_HelpRequested);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView _listView_Servers;
        private System.Windows.Forms.ColumnHeader _col_DateTime;
        private System.Windows.Forms.ColumnHeader _col_Baseline;
        private System.Windows.Forms.Button _button_Delete;
        private System.Windows.Forms.Button _button_Cancel;
        private System.Windows.Forms.Button _button_Help;
        private System.Windows.Forms.ColumnHeader _col_Status;
        private System.Windows.Forms.Button _button_OK;
        private System.Windows.Forms.ColumnHeader _col_Msg;
    }
}
