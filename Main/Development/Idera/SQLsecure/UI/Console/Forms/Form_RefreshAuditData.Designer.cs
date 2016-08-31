namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_RefreshAuditData
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
            this._button_OK = new Infragistics.Win.Misc.UltraButton();
            this._button_Help = new Infragistics.Win.Misc.UltraButton();
            this._button_Cancel = new Infragistics.Win.Misc.UltraButton();
            this._label_UseMostCurrent = new System.Windows.Forms.Label();
            this._checkBox_IncludeTime = new System.Windows.Forms.CheckBox();
            this._dateTimePicker_Date = new System.Windows.Forms.DateTimePicker();
            this._dateTimePicker_Time = new System.Windows.Forms.DateTimePicker();
            this._checkBox_BaselineOnly = new System.Windows.Forms.CheckBox();
            this._button_ViewSnapshots = new Infragistics.Win.Misc.UltraButton();
            this._label_Note = new System.Windows.Forms.Label();
            this._bfd_ButtonPanel.SuspendLayout();
            this._bf_MainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _bfd_ButtonPanel
            // 
            this._bfd_ButtonPanel.Controls.Add(this._button_ViewSnapshots);
            this._bfd_ButtonPanel.Controls.Add(this._button_OK);
            this._bfd_ButtonPanel.Controls.Add(this._button_Help);
            this._bfd_ButtonPanel.Controls.Add(this._button_Cancel);
            this._bfd_ButtonPanel.Location = new System.Drawing.Point(0, 238);
            this._bfd_ButtonPanel.Size = new System.Drawing.Size(517, 40);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this._button_Cancel, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this._button_Help, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this._button_OK, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this._button_ViewSnapshots, 0);
            // 
            // _bf_MainPanel
            // 
            this._bf_MainPanel.Controls.Add(this._label_Note);
            this._bf_MainPanel.Controls.Add(this._label_UseMostCurrent);
            this._bf_MainPanel.Controls.Add(this._checkBox_IncludeTime);
            this._bf_MainPanel.Controls.Add(this._dateTimePicker_Date);
            this._bf_MainPanel.Controls.Add(this._dateTimePicker_Time);
            this._bf_MainPanel.Controls.Add(this._checkBox_BaselineOnly);
            this._bf_MainPanel.Size = new System.Drawing.Size(517, 185);
            // 
            // _bf_HeaderPanel
            // 
            this._bf_HeaderPanel.Size = new System.Drawing.Size(517, 53);
            // 
            // _button_OK
            // 
            this._button_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._button_OK.Location = new System.Drawing.Point(250, 9);
            this._button_OK.Name = "_button_OK";
            this._button_OK.Size = new System.Drawing.Size(75, 23);
            this._button_OK.TabIndex = 5;
            this._button_OK.Text = "&OK";
            this._button_OK.Click += new System.EventHandler(this._button_OK_Click);
            // 
            // _button_Help
            // 
            this._button_Help.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._button_Help.Location = new System.Drawing.Point(411, 9);
            this._button_Help.Name = "_button_Help";
            this._button_Help.Size = new System.Drawing.Size(75, 23);
            this._button_Help.TabIndex = 7;
            this._button_Help.Text = "&Help";
            this._button_Help.Click += new System.EventHandler(this._button_Help_Click);
            // 
            // _button_Cancel
            // 
            this._button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._button_Cancel.Location = new System.Drawing.Point(330, 9);
            this._button_Cancel.Name = "_button_Cancel";
            this._button_Cancel.Size = new System.Drawing.Size(75, 23);
            this._button_Cancel.TabIndex = 6;
            this._button_Cancel.Text = "&Cancel";
            // 
            // _label_UseMostCurrent
            // 
            this._label_UseMostCurrent.AutoSize = true;
            this._label_UseMostCurrent.BackColor = System.Drawing.Color.Transparent;
            this._label_UseMostCurrent.Location = new System.Drawing.Point(23, 29);
            this._label_UseMostCurrent.Name = "_label_UseMostCurrent";
            this._label_UseMostCurrent.Size = new System.Drawing.Size(137, 13);
            this._label_UseMostCurrent.TabIndex = 19;
            this._label_UseMostCurrent.Text = "Use most current data as of";
            // 
            // _checkBox_IncludeTime
            // 
            this._checkBox_IncludeTime.AutoSize = true;
            this._checkBox_IncludeTime.BackColor = System.Drawing.Color.Transparent;
            this._checkBox_IncludeTime.Location = new System.Drawing.Point(289, 48);
            this._checkBox_IncludeTime.Name = "_checkBox_IncludeTime";
            this._checkBox_IncludeTime.Size = new System.Drawing.Size(87, 17);
            this._checkBox_IncludeTime.TabIndex = 1;
            this._checkBox_IncludeTime.Text = "Include Time";
            this._checkBox_IncludeTime.UseVisualStyleBackColor = false;
            this._checkBox_IncludeTime.CheckedChanged += new System.EventHandler(this._checkBox_IncludeTime_CheckedChanged);
            // 
            // _dateTimePicker_Date
            // 
            this._dateTimePicker_Date.CustomFormat = "";
            this._dateTimePicker_Date.Location = new System.Drawing.Point(26, 45);
            this._dateTimePicker_Date.MinDate = new System.DateTime(2006, 1, 1, 0, 0, 0, 0);
            this._dateTimePicker_Date.Name = "_dateTimePicker_Date";
            this._dateTimePicker_Date.Size = new System.Drawing.Size(199, 20);
            this._dateTimePicker_Date.TabIndex = 0;
            // 
            // _dateTimePicker_Time
            // 
            this._dateTimePicker_Time.Enabled = false;
            this._dateTimePicker_Time.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this._dateTimePicker_Time.Location = new System.Drawing.Point(381, 45);
            this._dateTimePicker_Time.Name = "_dateTimePicker_Time";
            this._dateTimePicker_Time.ShowUpDown = true;
            this._dateTimePicker_Time.Size = new System.Drawing.Size(105, 20);
            this._dateTimePicker_Time.TabIndex = 2;
            // 
            // _checkBox_BaselineOnly
            // 
            this._checkBox_BaselineOnly.AutoSize = true;
            this._checkBox_BaselineOnly.BackColor = System.Drawing.Color.Transparent;
            this._checkBox_BaselineOnly.Location = new System.Drawing.Point(26, 84);
            this._checkBox_BaselineOnly.Name = "_checkBox_BaselineOnly";
            this._checkBox_BaselineOnly.Size = new System.Drawing.Size(179, 17);
            this._checkBox_BaselineOnly.TabIndex = 3;
            this._checkBox_BaselineOnly.Text = "Use baseline snapshot data only";
            this._checkBox_BaselineOnly.UseVisualStyleBackColor = false;
            // 
            // _button_ViewSnapshots
            // 
            this._button_ViewSnapshots.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._button_ViewSnapshots.Location = new System.Drawing.Point(26, 9);
            this._button_ViewSnapshots.Name = "_button_ViewSnapshots";
            this._button_ViewSnapshots.Size = new System.Drawing.Size(182, 23);
            this._button_ViewSnapshots.TabIndex = 4;
            this._button_ViewSnapshots.Text = "&View Selected Snapshots";
            this._button_ViewSnapshots.Click += new System.EventHandler(this._button_ViewSnapshots_Click);
            // 
            // _label_Note
            // 
            this._label_Note.BackColor = System.Drawing.Color.Transparent;
            this._label_Note.ForeColor = System.Drawing.Color.Black;
            this._label_Note.Location = new System.Drawing.Point(23, 122);
            this._label_Note.Name = "_label_Note";
            this._label_Note.Size = new System.Drawing.Size(465, 48);
            this._label_Note.TabIndex = 20;
            this._label_Note.Text = "Note:\r\nChoosing new audit data may change the findings for this assessment and re" +
                "move any explanation notes you previously entered.";
            // 
            // Form_RefreshAuditData
            // 
            this.AcceptButton = this._button_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.CancelButton = this._button_Cancel;
            this.ClientSize = new System.Drawing.Size(517, 278);
            this.Description = "Check for new audit data snapshots and rerun the assessment with the updated sele" +
                "ctions.";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Form_RefreshAuditData";
            this.Text = "Refresh Audit Data";
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.Form_SelectDatabase_HelpRequested);
            this._bfd_ButtonPanel.ResumeLayout(false);
            this._bf_MainPanel.ResumeLayout(false);
            this._bf_MainPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton _button_OK;
        private Infragistics.Win.Misc.UltraButton _button_Help;
        private Infragistics.Win.Misc.UltraButton _button_Cancel;
        private System.Windows.Forms.Label _label_UseMostCurrent;
        private System.Windows.Forms.CheckBox _checkBox_IncludeTime;
        private System.Windows.Forms.DateTimePicker _dateTimePicker_Date;
        private System.Windows.Forms.DateTimePicker _dateTimePicker_Time;
        private System.Windows.Forms.CheckBox _checkBox_BaselineOnly;
        private Infragistics.Win.Misc.UltraButton _button_ViewSnapshots;
        private System.Windows.Forms.Label _label_Note;
    }
}
