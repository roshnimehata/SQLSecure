namespace Idera.SQLsecure.UI.Console.Views
{
    partial class Report_ServerLoginsAndUserMappings
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
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource3 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource4 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource5 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this._label_Login = new System.Windows.Forms.Label();
            this._textBox_Login = new System.Windows.Forms.TextBox();
            this._label_Server = new System.Windows.Forms.Label();
            this._comboBox_Server = new System.Windows.Forms.ComboBox();
            this._label_LoginType = new System.Windows.Forms.Label();
            this._comboBox_Login = new System.Windows.Forms.ComboBox();
            this._label_Database = new System.Windows.Forms.Label();
            this._textBox_Database = new System.Windows.Forms.TextBox();
            this._button_BrowseUsers = new System.Windows.Forms.Button();
            this._panel_Report.SuspendLayout();
            this._gradientPanel_Selections.SuspendLayout();
            this.SuspendLayout();
            // 
            // _panel_Report
            // 
            this._panel_Report.Location = new System.Drawing.Point(0, 98);
            this._panel_Report.Size = new System.Drawing.Size(652, 468);
            // 
            // _gradientPanel_Selections
            // 
            this._gradientPanel_Selections.Controls.Add(this._button_BrowseUsers);
            this._gradientPanel_Selections.Controls.Add(this._label_Database);
            this._gradientPanel_Selections.Controls.Add(this._textBox_Database);
            this._gradientPanel_Selections.Controls.Add(this._comboBox_Login);
            this._gradientPanel_Selections.Controls.Add(this._label_LoginType);
            this._gradientPanel_Selections.Controls.Add(this._label_Server);
            this._gradientPanel_Selections.Controls.Add(this._comboBox_Server);
            this._gradientPanel_Selections.Controls.Add(this._label_Login);
            this._gradientPanel_Selections.Controls.Add(this._textBox_Login);
            this._gradientPanel_Selections.Size = new System.Drawing.Size(652, 98);
            this._gradientPanel_Selections.Controls.SetChildIndex(this._textBox_Login, 0);
            this._gradientPanel_Selections.Controls.SetChildIndex(this._label_Login, 0);
            this._gradientPanel_Selections.Controls.SetChildIndex(this._comboBox_Server, 0);
            this._gradientPanel_Selections.Controls.SetChildIndex(this._label_Server, 0);
            this._gradientPanel_Selections.Controls.SetChildIndex(this._label_LoginType, 0);
            this._gradientPanel_Selections.Controls.SetChildIndex(this._comboBox_Login, 0);
            this._gradientPanel_Selections.Controls.SetChildIndex(this._textBox_Database, 0);
            this._gradientPanel_Selections.Controls.SetChildIndex(this._label_Database, 0);
            this._gradientPanel_Selections.Controls.SetChildIndex(this._button_BrowseUsers, 0);
            // 
            // _reportViewer
            // 
            reportDataSource1.Name = "ReportsDataset_isp_sqlsecure_report_getguestenabledservers";
            reportDataSource1.Value = null;
            reportDataSource2.Name = "ReportsDataset_isp_sqlsecure_report_getguestenabledservers";
            reportDataSource2.Value = null;
            reportDataSource3.Name = "ReportsDataset_isp_sqlsecure_report_getguestenabledservers";
            reportDataSource3.Value = null;
            reportDataSource4.Name = "ReportsDataset_isp_sqlsecure_report_getguestenabledservers";
            reportDataSource4.Value = null;
            reportDataSource5.Name = "ReportsDataset_isp_sqlsecure_report_getguestenabledservers";
            reportDataSource5.Value = null;
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource1);
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource2);
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource3);
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource4);
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource5);
            this._reportViewer.LocalReport.EnableHyperlinks = true;
            this._reportViewer.LocalReport.ReportEmbeddedResource = "Idera.SQLsecure.UI.Console.Reports.Report_ServerLoginsAndUserMappings.rdlc";
            this._reportViewer.Size = new System.Drawing.Size(652, 468);
            // 
            // _label_Description
            // 
            this._label_Description.Size = new System.Drawing.Size(621, 17);
            // 
            // _label_ReportTitle
            // 
            this._label_ReportTitle.Size = new System.Drawing.Size(636, 56);
            // 
            // _label_Instructions
            // 
            this._label_Instructions.Size = new System.Drawing.Size(495, 361);
            // 
            // _button_RunReport
            // 
            this._button_RunReport.TabIndex = 0;
            // 
            // _label_Login
            // 
            this._label_Login.AutoSize = true;
            this._label_Login.BackColor = System.Drawing.Color.Transparent;
            this._label_Login.Location = new System.Drawing.Point(6, 68);
            this._label_Login.Name = "_label_Login";
            this._label_Login.Size = new System.Drawing.Size(32, 13);
            this._label_Login.TabIndex = 0;
            this._label_Login.Text = "User:";
            // 
            // _textBox_Login
            // 
            this._textBox_Login.Location = new System.Drawing.Point(77, 65);
            this._textBox_Login.Name = "_textBox_Login";
            this._textBox_Login.Size = new System.Drawing.Size(179, 20);
            this._textBox_Login.TabIndex = 1;
            // 
            // _label_Server
            // 
            this._label_Server.AutoSize = true;
            this._label_Server.BackColor = System.Drawing.Color.Transparent;
            this._label_Server.Location = new System.Drawing.Point(6, 16);
            this._label_Server.Name = "_label_Server";
            this._label_Server.Size = new System.Drawing.Size(65, 13);
            this._label_Server.TabIndex = 2;
            this._label_Server.Text = "SQL Server:";
            // 
            // _comboBox_Server
            // 
            this._comboBox_Server.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._comboBox_Server.FormattingEnabled = true;
            this._comboBox_Server.Location = new System.Drawing.Point(77, 13);
            this._comboBox_Server.Name = "_comboBox_Server";
            this._comboBox_Server.Size = new System.Drawing.Size(427, 21);
            this._comboBox_Server.TabIndex = 3;
            this._comboBox_Server.SelectionChangeCommitted += new System.EventHandler(this._comboBox_Server_SelectionChangeCommitted);
            this._comboBox_Server.DropDown += new System.EventHandler(this._comboBox_Server_DropDown);
            // 
            // _label_LoginType
            // 
            this._label_LoginType.AutoSize = true;
            this._label_LoginType.BackColor = System.Drawing.Color.Transparent;
            this._label_LoginType.Location = new System.Drawing.Point(291, 42);
            this._label_LoginType.Name = "_label_LoginType";
            this._label_LoginType.Size = new System.Drawing.Size(63, 13);
            this._label_LoginType.TabIndex = 4;
            this._label_LoginType.Text = "Login Type:";
            // 
            // _comboBox_Login
            // 
            this._comboBox_Login.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._comboBox_Login.FormattingEnabled = true;
            this._comboBox_Login.Location = new System.Drawing.Point(360, 40);
            this._comboBox_Login.Name = "_comboBox_Login";
            this._comboBox_Login.Size = new System.Drawing.Size(144, 21);
            this._comboBox_Login.TabIndex = 5;
            this._comboBox_Login.SelectionChangeCommitted += new System.EventHandler(this._comboBox_Login_SelectionChangeCommitted);
            this._comboBox_Login.DropDown += new System.EventHandler(this._comboBox_Login_DropDown);
            // 
            // _label_Database
            // 
            this._label_Database.AutoSize = true;
            this._label_Database.BackColor = System.Drawing.Color.Transparent;
            this._label_Database.Location = new System.Drawing.Point(6, 42);
            this._label_Database.Name = "_label_Database";
            this._label_Database.Size = new System.Drawing.Size(56, 13);
            this._label_Database.TabIndex = 6;
            this._label_Database.Text = "Database:";
            // 
            // _textBox_Database
            // 
            this._textBox_Database.Location = new System.Drawing.Point(77, 40);
            this._textBox_Database.Name = "_textBox_Database";
            this._textBox_Database.Size = new System.Drawing.Size(208, 20);
            this._textBox_Database.TabIndex = 7;
            // 
            // _button_BrowseUsers
            // 
            this._button_BrowseUsers.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this._button_BrowseUsers.CausesValidation = false;
            this._button_BrowseUsers.Enabled = false;
            this._button_BrowseUsers.Location = new System.Drawing.Point(261, 63);
            this._button_BrowseUsers.Name = "_button_BrowseUsers";
            this._button_BrowseUsers.Size = new System.Drawing.Size(24, 23);
            this._button_BrowseUsers.TabIndex = 14;
            this._button_BrowseUsers.Text = "...";
            this._button_BrowseUsers.UseVisualStyleBackColor = true;
            this._button_BrowseUsers.Click += new System.EventHandler(this._button_BrowseUsers_Click);
            // 
            // Report_ServerLoginsAndUserMappings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "Report_ServerLoginsAndUserMappings";
            this._panel_Report.ResumeLayout(false);
            this._panel_Report.PerformLayout();
            this._gradientPanel_Selections.ResumeLayout(false);
            this._gradientPanel_Selections.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox _comboBox_Server;
        private System.Windows.Forms.Label _label_Server;
        private System.Windows.Forms.TextBox _textBox_Login;
        private System.Windows.Forms.Label _label_Login;
        private System.Windows.Forms.TextBox _textBox_Database;
        private System.Windows.Forms.Label _label_Database;
        private System.Windows.Forms.ComboBox _comboBox_Login;
        private System.Windows.Forms.Label _label_LoginType;
        private System.Windows.Forms.Button _button_BrowseUsers;
    }
}