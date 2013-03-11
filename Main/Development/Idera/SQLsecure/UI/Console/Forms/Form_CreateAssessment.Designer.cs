namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_CreateAssessment
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
            this._button_Help = new Infragistics.Win.Misc.UltraButton();
            this._button_OK = new Infragistics.Win.Misc.UltraButton();
            this._button_Cancel = new Infragistics.Win.Misc.UltraButton();
            this._ultraGroupBox_DataSelection = new Infragistics.Win.Misc.UltraGroupBox();
            this._label_UseMostCurrent = new System.Windows.Forms.Label();
            this._checkBox_IncludeTime = new System.Windows.Forms.CheckBox();
            this._dateTimePicker_Date = new System.Windows.Forms.DateTimePicker();
            this._dateTimePicker_Time = new System.Windows.Forms.DateTimePicker();
            this._checkBox_BaselineOnly = new System.Windows.Forms.CheckBox();
            this._textBox_Description = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this._textBox_AssessmentName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this._bfd_ButtonPanel.SuspendLayout();
            this._bf_MainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._ultraGroupBox_DataSelection)).BeginInit();
            this._ultraGroupBox_DataSelection.SuspendLayout();
            this.SuspendLayout();
            // 
            // _bfd_ButtonPanel
            // 
            this._bfd_ButtonPanel.Controls.Add(this._button_OK);
            this._bfd_ButtonPanel.Controls.Add(this._button_Help);
            this._bfd_ButtonPanel.Controls.Add(this._button_Cancel);
            this._bfd_ButtonPanel.Location = new System.Drawing.Point(0, 302);
            this._bfd_ButtonPanel.Size = new System.Drawing.Size(565, 40);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this._button_Cancel, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this._button_Help, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this._button_OK, 0);
            // 
            // _bf_MainPanel
            // 
            this._bf_MainPanel.Controls.Add(this.label4);
            this._bf_MainPanel.Controls.Add(this._ultraGroupBox_DataSelection);
            this._bf_MainPanel.Controls.Add(this._textBox_Description);
            this._bf_MainPanel.Controls.Add(this._textBox_AssessmentName);
            this._bf_MainPanel.Controls.Add(this.label1);
            this._bf_MainPanel.Size = new System.Drawing.Size(565, 249);
            // 
            // _bf_HeaderPanel
            // 
            this._bf_HeaderPanel.Size = new System.Drawing.Size(565, 53);
            // 
            // _button_Help
            // 
            this._button_Help.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._button_Help.Location = new System.Drawing.Point(467, 10);
            this._button_Help.Name = "_button_Help";
            this._button_Help.Size = new System.Drawing.Size(75, 23);
            this._button_Help.TabIndex = 4;
            this._button_Help.Text = "&Help";
            this._button_Help.Click += new System.EventHandler(this._button_Help_Click);
            // 
            // _button_OK
            // 
            this._button_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._button_OK.Location = new System.Drawing.Point(306, 10);
            this._button_OK.Name = "_button_OK";
            this._button_OK.Size = new System.Drawing.Size(75, 23);
            this._button_OK.TabIndex = 2;
            this._button_OK.Text = "&OK";
            this._button_OK.Click += new System.EventHandler(this._button_OK_Click);
            // 
            // _button_Cancel
            // 
            this._button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._button_Cancel.Location = new System.Drawing.Point(386, 10);
            this._button_Cancel.Name = "_button_Cancel";
            this._button_Cancel.Size = new System.Drawing.Size(75, 23);
            this._button_Cancel.TabIndex = 3;
            this._button_Cancel.Text = "&Cancel";
            // 
            // _ultraGroupBox_DataSelection
            // 
            this._ultraGroupBox_DataSelection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._ultraGroupBox_DataSelection.Controls.Add(this._label_UseMostCurrent);
            this._ultraGroupBox_DataSelection.Controls.Add(this._checkBox_IncludeTime);
            this._ultraGroupBox_DataSelection.Controls.Add(this._dateTimePicker_Date);
            this._ultraGroupBox_DataSelection.Controls.Add(this._dateTimePicker_Time);
            this._ultraGroupBox_DataSelection.Controls.Add(this._checkBox_BaselineOnly);
            this._ultraGroupBox_DataSelection.Enabled = false;
            this._ultraGroupBox_DataSelection.Location = new System.Drawing.Point(29, 224);
            this._ultraGroupBox_DataSelection.Name = "_ultraGroupBox_DataSelection";
            this._ultraGroupBox_DataSelection.Size = new System.Drawing.Size(513, 104);
            this._ultraGroupBox_DataSelection.TabIndex = 10;
            this._ultraGroupBox_DataSelection.Text = "Data Selection";
            this._ultraGroupBox_DataSelection.UseAppStyling = false;
            this._ultraGroupBox_DataSelection.Visible = false;
            // 
            // _label_UseMostCurrent
            // 
            this._label_UseMostCurrent.AutoSize = true;
            this._label_UseMostCurrent.Location = new System.Drawing.Point(24, 23);
            this._label_UseMostCurrent.Name = "_label_UseMostCurrent";
            this._label_UseMostCurrent.Size = new System.Drawing.Size(137, 13);
            this._label_UseMostCurrent.TabIndex = 19;
            this._label_UseMostCurrent.Text = "Use most current data as of";
            // 
            // _checkBox_IncludeTime
            // 
            this._checkBox_IncludeTime.AutoSize = true;
            this._checkBox_IncludeTime.Location = new System.Drawing.Point(259, 42);
            this._checkBox_IncludeTime.Name = "_checkBox_IncludeTime";
            this._checkBox_IncludeTime.Size = new System.Drawing.Size(87, 17);
            this._checkBox_IncludeTime.TabIndex = 3;
            this._checkBox_IncludeTime.Text = "Include Time";
            this._checkBox_IncludeTime.UseVisualStyleBackColor = true;
            this._checkBox_IncludeTime.Click += new System.EventHandler(this._checkBox_IncludeTime_CheckedChanged);
            // 
            // _dateTimePicker_Date
            // 
            this._dateTimePicker_Date.CustomFormat = "";
            this._dateTimePicker_Date.Location = new System.Drawing.Point(27, 39);
            this._dateTimePicker_Date.MinDate = new System.DateTime(2006, 1, 1, 0, 0, 0, 0);
            this._dateTimePicker_Date.Name = "_dateTimePicker_Date";
            this._dateTimePicker_Date.Size = new System.Drawing.Size(199, 20);
            this._dateTimePicker_Date.TabIndex = 2;
            // 
            // _dateTimePicker_Time
            // 
            this._dateTimePicker_Time.Enabled = false;
            this._dateTimePicker_Time.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this._dateTimePicker_Time.Location = new System.Drawing.Point(353, 39);
            this._dateTimePicker_Time.Name = "_dateTimePicker_Time";
            this._dateTimePicker_Time.ShowUpDown = true;
            this._dateTimePicker_Time.Size = new System.Drawing.Size(105, 20);
            this._dateTimePicker_Time.TabIndex = 4;
            // 
            // _checkBox_BaselineOnly
            // 
            this._checkBox_BaselineOnly.AutoSize = true;
            this._checkBox_BaselineOnly.Location = new System.Drawing.Point(27, 72);
            this._checkBox_BaselineOnly.Name = "_checkBox_BaselineOnly";
            this._checkBox_BaselineOnly.Size = new System.Drawing.Size(179, 17);
            this._checkBox_BaselineOnly.TabIndex = 11;
            this._checkBox_BaselineOnly.Text = "Use baseline snapshot data only";
            this._checkBox_BaselineOnly.UseVisualStyleBackColor = true;
            // 
            // _textBox_Description
            // 
            this._textBox_Description.AcceptsReturn = true;
            this._textBox_Description.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._textBox_Description.Location = new System.Drawing.Point(101, 78);
            this._textBox_Description.Multiline = true;
            this._textBox_Description.Name = "_textBox_Description";
            this._textBox_Description.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._textBox_Description.Size = new System.Drawing.Size(441, 140);
            this._textBox_Description.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(26, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Description";
            // 
            // _textBox_AssessmentName
            // 
            this._textBox_AssessmentName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._textBox_AssessmentName.Location = new System.Drawing.Point(101, 35);
            this._textBox_AssessmentName.MaxLength = 128;
            this._textBox_AssessmentName.Name = "_textBox_AssessmentName";
            this._textBox_AssessmentName.Size = new System.Drawing.Size(441, 20);
            this._textBox_AssessmentName.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(25, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Name";
            // 
            // Form_CreateAssessment
            // 
            this.AcceptButton = this._button_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.CancelButton = this._button_Cancel;
            this.ClientSize = new System.Drawing.Size(565, 342);
            this.Description = "Create assessment for Policy Name";
            this.Name = "Form_CreateAssessment";
            this.Text = "Create Assessment";
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.Form_SelectDatabase_HelpRequested);
            this._bfd_ButtonPanel.ResumeLayout(false);
            this._bf_MainPanel.ResumeLayout(false);
            this._bf_MainPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._ultraGroupBox_DataSelection)).EndInit();
            this._ultraGroupBox_DataSelection.ResumeLayout(false);
            this._ultraGroupBox_DataSelection.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton _button_Cancel;
        private Infragistics.Win.Misc.UltraGroupBox _ultraGroupBox_DataSelection;
        private System.Windows.Forms.CheckBox _checkBox_IncludeTime;
        private System.Windows.Forms.DateTimePicker _dateTimePicker_Date;
        private System.Windows.Forms.DateTimePicker _dateTimePicker_Time;
        private System.Windows.Forms.CheckBox _checkBox_BaselineOnly;
        private System.Windows.Forms.TextBox _textBox_Description;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _textBox_AssessmentName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label _label_UseMostCurrent;
        private Infragistics.Win.Misc.UltraButton _button_Help;
        private Infragistics.Win.Misc.UltraButton _button_OK;
    }
}
