namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_SelectSnapshot
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
            this._listView_Snapshots = new System.Windows.Forms.ListView();
            this._col_icon = new System.Windows.Forms.ColumnHeader();
            this._col_DateTime = new System.Windows.Forms.ColumnHeader();
            this._col_Status = new System.Windows.Forms.ColumnHeader();
            this._col_Baseline = new System.Windows.Forms.ColumnHeader();
            this._button_Properties = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _button_OK
            // 
            this._button_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._button_OK.Enabled = false;
            this._button_OK.Location = new System.Drawing.Point(274, 233);
            this._button_OK.Name = "_button_OK";
            this._button_OK.Size = new System.Drawing.Size(75, 23);
            this._button_OK.TabIndex = 2;
            this._button_OK.Text = "&OK";
            this._button_OK.UseVisualStyleBackColor = true;
            this._button_OK.Click += new System.EventHandler(this._button_OK_Click);
            // 
            // _button_Cancel
            // 
            this._button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._button_Cancel.Location = new System.Drawing.Point(358, 233);
            this._button_Cancel.Name = "_button_Cancel";
            this._button_Cancel.Size = new System.Drawing.Size(75, 23);
            this._button_Cancel.TabIndex = 3;
            this._button_Cancel.Text = "&Cancel";
            this._button_Cancel.UseVisualStyleBackColor = true;
            // 
            // _listView_Snapshots
            // 
            this._listView_Snapshots.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._listView_Snapshots.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._col_icon,
            this._col_DateTime,
            this._col_Status,
            this._col_Baseline});
            this._listView_Snapshots.FullRowSelect = true;
            this._listView_Snapshots.LabelWrap = false;
            this._listView_Snapshots.Location = new System.Drawing.Point(12, 12);
            this._listView_Snapshots.MultiSelect = false;
            this._listView_Snapshots.Name = "_listView_Snapshots";
            this._listView_Snapshots.Size = new System.Drawing.Size(423, 215);
            this._listView_Snapshots.TabIndex = 0;
            this._listView_Snapshots.UseCompatibleStateImageBehavior = false;
            this._listView_Snapshots.View = System.Windows.Forms.View.Details;
            this._listView_Snapshots.SelectedIndexChanged += new System.EventHandler(this._listView_Snapshots_SelectedIndexChanged);
            this._listView_Snapshots.DoubleClick += new System.EventHandler(this._listView_Snapshots_DoubleClick);
            // 
            // _col_icon
            // 
            this._col_icon.Text = "";
            this._col_icon.Width = 22;
            // 
            // _col_DateTime
            // 
            this._col_DateTime.Text = "Snapshot";
            this._col_DateTime.Width = 193;
            // 
            // _col_Status
            // 
            this._col_Status.Text = "Status";
            this._col_Status.Width = 127;
            // 
            // _col_Baseline
            // 
            this._col_Baseline.Text = "Baseline";
            // 
            // _button_Properties
            // 
            this._button_Properties.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._button_Properties.Enabled = false;
            this._button_Properties.Location = new System.Drawing.Point(12, 233);
            this._button_Properties.Name = "_button_Properties";
            this._button_Properties.Size = new System.Drawing.Size(75, 23);
            this._button_Properties.TabIndex = 1;
            this._button_Properties.Text = "&Properties";
            this._button_Properties.UseVisualStyleBackColor = true;
            this._button_Properties.Click += new System.EventHandler(this._button_Properties_Click);
            // 
            // Form_SelectSnapshot
            // 
            this.AcceptButton = this._button_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.CancelButton = this._button_Cancel;
            this.ClientSize = new System.Drawing.Size(447, 268);
            this.Controls.Add(this._button_Properties);
            this.Controls.Add(this._button_OK);
            this.Controls.Add(this._button_Cancel);
            this.Controls.Add(this._listView_Snapshots);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MinimumSize = new System.Drawing.Size(270, 200);
            this.Name = "Form_SelectSnapshot";
            this.Text = "Select Snapshot";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.Form_SelectSnapshot_HelpButtonClicked);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.Form_SelectSnapshot_HelpRequested);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _button_OK;
        private System.Windows.Forms.Button _button_Cancel;
        private System.Windows.Forms.ListView _listView_Snapshots;
        private System.Windows.Forms.ColumnHeader _col_DateTime;
        private System.Windows.Forms.ColumnHeader _col_Status;
        private System.Windows.Forms.ColumnHeader _col_icon;
        private System.Windows.Forms.ColumnHeader _col_Baseline;
        private System.Windows.Forms.Button _button_Properties;

    }
}
