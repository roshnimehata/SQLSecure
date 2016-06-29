namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_BaselineSnapshot
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
            this._listView_Snapshots = new System.Windows.Forms.ListView();
            this._col_DateTime = new System.Windows.Forms.ColumnHeader();
            this._col_Baseline = new System.Windows.Forms.ColumnHeader();
            this._button_OK = new System.Windows.Forms.Button();
            this._button_Cancel = new System.Windows.Forms.Button();
            this._button_Help = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_Comment = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(196, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "The following snapshot will be baselined";
            // 
            // _listView_Snapshots
            // 
            this._listView_Snapshots.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._col_DateTime,
            this._col_Baseline});
            this._listView_Snapshots.FullRowSelect = true;
            this._listView_Snapshots.LabelWrap = false;
            this._listView_Snapshots.Location = new System.Drawing.Point(20, 30);
            this._listView_Snapshots.MultiSelect = false;
            this._listView_Snapshots.Name = "_listView_Snapshots";
            this._listView_Snapshots.Size = new System.Drawing.Size(381, 46);
            this._listView_Snapshots.TabIndex = 1;
            this._listView_Snapshots.UseCompatibleStateImageBehavior = false;
            this._listView_Snapshots.View = System.Windows.Forms.View.Details;
            // 
            // _col_DateTime
            // 
            this._col_DateTime.Text = "Snapshot Taken";
            this._col_DateTime.Width = 256;
            // 
            // _col_Baseline
            // 
            this._col_Baseline.Text = "Baseline";
            this._col_Baseline.Width = 87;
            // 
            // _button_OK
            // 
            this._button_OK.Location = new System.Drawing.Point(161, 261);
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
            this._button_Cancel.Location = new System.Drawing.Point(243, 261);
            this._button_Cancel.Name = "_button_Cancel";
            this._button_Cancel.Size = new System.Drawing.Size(75, 23);
            this._button_Cancel.TabIndex = 3;
            this._button_Cancel.Text = "&Cancel";
            this._button_Cancel.UseVisualStyleBackColor = true;
            // 
            // _button_Help
            // 
            this._button_Help.Location = new System.Drawing.Point(325, 261);
            this._button_Help.Name = "_button_Help";
            this._button_Help.Size = new System.Drawing.Size(75, 23);
            this._button_Help.TabIndex = 4;
            this._button_Help.Text = "&Help";
            this._button_Help.UseVisualStyleBackColor = true;
            this._button_Help.Click += new System.EventHandler(this._button_Help_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(135, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Enter comment for baseline";
            // 
            // textBox_Comment
            // 
            this.textBox_Comment.Location = new System.Drawing.Point(20, 111);
            this.textBox_Comment.MaxLength = 500;
            this.textBox_Comment.Multiline = true;
            this.textBox_Comment.Name = "textBox_Comment";
            this.textBox_Comment.Size = new System.Drawing.Size(381, 136);
            this.textBox_Comment.TabIndex = 14;
            // 
            // Form_BaselineSnapshot
            // 
            this.AcceptButton = this._button_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.CancelButton = this._button_Cancel;
            this.ClientSize = new System.Drawing.Size(424, 299);
            this.Controls.Add(this.textBox_Comment);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._button_Help);
            this.Controls.Add(this._button_OK);
            this.Controls.Add(this._button_Cancel);
            this.Controls.Add(this._listView_Snapshots);
            this.Controls.Add(this.label1);
            this.Name = "Form_BaselineSnapshot";
            this.Text = "Baseline Snapshot";
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.Form_BaselineSnapshot_HelpRequested);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView _listView_Snapshots;
        private System.Windows.Forms.ColumnHeader _col_DateTime;
        private System.Windows.Forms.ColumnHeader _col_Baseline;
        private System.Windows.Forms.Button _button_OK;
        private System.Windows.Forms.Button _button_Cancel;
        private System.Windows.Forms.Button _button_Help;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_Comment;
    }
}
