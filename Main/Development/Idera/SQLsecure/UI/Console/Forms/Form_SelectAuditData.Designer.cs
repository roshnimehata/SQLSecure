namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_SelectAuditData
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
            this._checkBox_BaselineOnly = new System.Windows.Forms.CheckBox();
            this._checkBox_IncludeTime = new System.Windows.Forms.CheckBox();
            this._dateTimePicker_Date = new System.Windows.Forms.DateTimePicker();
            this._dateTimePicker_Time = new System.Windows.Forms.DateTimePicker();
            this._ultraGroupBox_DateSelect = new Infragistics.Win.Misc.UltraGroupBox();
            this.button_Help = new System.Windows.Forms.Button();
            this._button_OK = new System.Windows.Forms.Button();
            this._button_Cancel = new System.Windows.Forms.Button();
            this._checkBox_UseCurrent = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this._ultraGroupBox_DateSelect)).BeginInit();
            this._ultraGroupBox_DateSelect.SuspendLayout();
            this.SuspendLayout();
            // 
            // _checkBox_BaselineOnly
            // 
            this._checkBox_BaselineOnly.AutoSize = true;
            this._checkBox_BaselineOnly.Location = new System.Drawing.Point(14, 169);
            this._checkBox_BaselineOnly.Name = "_checkBox_BaselineOnly";
            this._checkBox_BaselineOnly.Size = new System.Drawing.Size(179, 17);
            this._checkBox_BaselineOnly.TabIndex = 5;
            this._checkBox_BaselineOnly.Text = "Use baseline snapshot data only";
            this._checkBox_BaselineOnly.UseVisualStyleBackColor = true;
            // 
            // _checkBox_IncludeTime
            // 
            this._checkBox_IncludeTime.AutoSize = true;
            this._checkBox_IncludeTime.Location = new System.Drawing.Point(27, 68);
            this._checkBox_IncludeTime.Name = "_checkBox_IncludeTime";
            this._checkBox_IncludeTime.Size = new System.Drawing.Size(87, 17);
            this._checkBox_IncludeTime.TabIndex = 3;
            this._checkBox_IncludeTime.Text = "Include Time";
            this._checkBox_IncludeTime.UseVisualStyleBackColor = true;
            this._checkBox_IncludeTime.CheckedChanged += new System.EventHandler(this._checkBox_IncludeTime_CheckedChanged);
            // 
            // _dateTimePicker_Date
            // 
            this._dateTimePicker_Date.CustomFormat = "";
            this._dateTimePicker_Date.Location = new System.Drawing.Point(27, 28);
            this._dateTimePicker_Date.MinDate = new System.DateTime(2006, 1, 1, 0, 0, 0, 0);
            this._dateTimePicker_Date.Name = "_dateTimePicker_Date";
            this._dateTimePicker_Date.Size = new System.Drawing.Size(199, 20);
            this._dateTimePicker_Date.TabIndex = 2;
            // 
            // _dateTimePicker_Time
            // 
            this._dateTimePicker_Time.Enabled = false;
            this._dateTimePicker_Time.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this._dateTimePicker_Time.Location = new System.Drawing.Point(121, 65);
            this._dateTimePicker_Time.Name = "_dateTimePicker_Time";
            this._dateTimePicker_Time.ShowUpDown = true;
            this._dateTimePicker_Time.Size = new System.Drawing.Size(105, 20);
            this._dateTimePicker_Time.TabIndex = 4;
            // 
            // _ultraGroupBox_DateSelect
            // 
            this._ultraGroupBox_DateSelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._ultraGroupBox_DateSelect.Controls.Add(this._checkBox_IncludeTime);
            this._ultraGroupBox_DateSelect.Controls.Add(this._dateTimePicker_Date);
            this._ultraGroupBox_DateSelect.Controls.Add(this._dateTimePicker_Time);
            this._ultraGroupBox_DateSelect.Enabled = false;
            this._ultraGroupBox_DateSelect.Location = new System.Drawing.Point(12, 45);
            this._ultraGroupBox_DateSelect.Name = "_ultraGroupBox_DateSelect";
            this._ultraGroupBox_DateSelect.Size = new System.Drawing.Size(253, 104);
            this._ultraGroupBox_DateSelect.TabIndex = 1;
            this._ultraGroupBox_DateSelect.Text = "Use most current data as of";
            this._ultraGroupBox_DateSelect.UseAppStyling = false;
            // 
            // button_Help
            // 
            this.button_Help.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Help.Location = new System.Drawing.Point(190, 218);
            this.button_Help.Name = "button_Help";
            this.button_Help.Size = new System.Drawing.Size(75, 23);
            this.button_Help.TabIndex = 8;
            this.button_Help.Text = "&Help";
            this.button_Help.UseVisualStyleBackColor = true;
            this.button_Help.Click += new System.EventHandler(this.button_Help_Click);
            // 
            // _button_OK
            // 
            this._button_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._button_OK.Location = new System.Drawing.Point(29, 218);
            this._button_OK.Name = "_button_OK";
            this._button_OK.Size = new System.Drawing.Size(75, 23);
            this._button_OK.TabIndex = 6;
            this._button_OK.Text = "&OK";
            this._button_OK.UseVisualStyleBackColor = true;
            this._button_OK.Click += new System.EventHandler(this._button_OK_Click);
            // 
            // _button_Cancel
            // 
            this._button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._button_Cancel.Location = new System.Drawing.Point(109, 218);
            this._button_Cancel.Name = "_button_Cancel";
            this._button_Cancel.Size = new System.Drawing.Size(75, 23);
            this._button_Cancel.TabIndex = 7;
            this._button_Cancel.Text = "&Cancel";
            this._button_Cancel.UseVisualStyleBackColor = true;
            // 
            // _checkBox_UseCurrent
            // 
            this._checkBox_UseCurrent.AutoSize = true;
            this._checkBox_UseCurrent.Location = new System.Drawing.Point(14, 19);
            this._checkBox_UseCurrent.Name = "_checkBox_UseCurrent";
            this._checkBox_UseCurrent.Size = new System.Drawing.Size(130, 17);
            this._checkBox_UseCurrent.TabIndex = 0;
            this._checkBox_UseCurrent.Text = "Use most current data";
            this._checkBox_UseCurrent.UseVisualStyleBackColor = true;
            this._checkBox_UseCurrent.Click += new System.EventHandler(this._checkBox_UseCurrent_Click);
            // 
            // Form_SelectAuditData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(277, 253);
            this.Controls.Add(this._checkBox_UseCurrent);
            this.Controls.Add(this.button_Help);
            this.Controls.Add(this._button_OK);
            this.Controls.Add(this._button_Cancel);
            this.Controls.Add(this._ultraGroupBox_DateSelect);
            this.Controls.Add(this._checkBox_BaselineOnly);
            this.Name = "Form_SelectAuditData";
            this.Text = "Select Audit Data";
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.Form_SelectDatabase_HelpRequested);
            ((System.ComponentModel.ISupportInitialize)(this._ultraGroupBox_DateSelect)).EndInit();
            this._ultraGroupBox_DateSelect.ResumeLayout(false);
            this._ultraGroupBox_DateSelect.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox _checkBox_BaselineOnly;
        private System.Windows.Forms.CheckBox _checkBox_IncludeTime;
        private System.Windows.Forms.DateTimePicker _dateTimePicker_Date;
        private System.Windows.Forms.DateTimePicker _dateTimePicker_Time;
        private Infragistics.Win.Misc.UltraGroupBox _ultraGroupBox_DateSelect;
        private System.Windows.Forms.Button button_Help;
        private System.Windows.Forms.Button _button_OK;
        private System.Windows.Forms.Button _button_Cancel;
        private System.Windows.Forms.CheckBox _checkBox_UseCurrent;
    }
}
