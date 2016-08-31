namespace Idera.SQLsecure.UI.Console.Views
{
    partial class Report_UsersPermissions
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
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource6 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource7 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource8 = new Microsoft.Reporting.WinForms.ReportDataSource();

            this._label_Server = new System.Windows.Forms.Label();
            this._comboBox_Server = new System.Windows.Forms.ComboBox();
            this._label_PermissionType = new System.Windows.Forms.Label();
            this._comboBox_PermissionType = new System.Windows.Forms.ComboBox();
            this._label_User = new System.Windows.Forms.Label();
            this._button_BrowseUsers = new System.Windows.Forms.Button();
            this._textBox_User = new System.Windows.Forms.TextBox();
            this._radioButton_WindowsUser = new System.Windows.Forms.RadioButton();
            this._radioButton_SQLLogin = new System.Windows.Forms.RadioButton();
            this._label_LoginType = new System.Windows.Forms.Label();
            this._label_Level = new System.Windows.Forms.Label();
            this._comboBox_Level = new System.Windows.Forms.ComboBox();
            this._panel_Report.SuspendLayout();
            this._gradientPanel_Selections.SuspendLayout();
            this.SuspendLayout();
            // 
            // _panel_Report
            // 
            this._panel_Report.Location = new System.Drawing.Point(0, 98);
            this._panel_Report.Size = new System.Drawing.Size(1058, 468);
            // 
            // _gradientPanel_Selections
            // 
            this._gradientPanel_Selections.Controls.Add(this._comboBox_Level);
            this._gradientPanel_Selections.Controls.Add(this._label_Level);
            this._gradientPanel_Selections.Controls.Add(this._label_LoginType);
            this._gradientPanel_Selections.Controls.Add(this._label_User);
            this._gradientPanel_Selections.Controls.Add(this._textBox_User);
            this._gradientPanel_Selections.Controls.Add(this._button_BrowseUsers);
            this._gradientPanel_Selections.Controls.Add(this._radioButton_WindowsUser);
            this._gradientPanel_Selections.Controls.Add(this._radioButton_SQLLogin);
            this._gradientPanel_Selections.Controls.Add(this._label_PermissionType);
            this._gradientPanel_Selections.Controls.Add(this._comboBox_PermissionType);
            this._gradientPanel_Selections.Controls.Add(this._label_Server);
            this._gradientPanel_Selections.Controls.Add(this._comboBox_Server);
            this._gradientPanel_Selections.Size = new System.Drawing.Size(1058, 98);
            this._gradientPanel_Selections.Controls.SetChildIndex(this._comboBox_Server, 0);
            this._gradientPanel_Selections.Controls.SetChildIndex(this._label_Server, 0);
            this._gradientPanel_Selections.Controls.SetChildIndex(this._comboBox_PermissionType, 0);
            this._gradientPanel_Selections.Controls.SetChildIndex(this._label_PermissionType, 0);
            this._gradientPanel_Selections.Controls.SetChildIndex(this._radioButton_SQLLogin, 0);
            this._gradientPanel_Selections.Controls.SetChildIndex(this._radioButton_WindowsUser, 0);
            this._gradientPanel_Selections.Controls.SetChildIndex(this._button_BrowseUsers, 0);
            this._gradientPanel_Selections.Controls.SetChildIndex(this._textBox_User, 0);
            this._gradientPanel_Selections.Controls.SetChildIndex(this._label_User, 0);
            this._gradientPanel_Selections.Controls.SetChildIndex(this._label_LoginType, 0);
            this._gradientPanel_Selections.Controls.SetChildIndex(this._label_Level, 0);
            this._gradientPanel_Selections.Controls.SetChildIndex(this._comboBox_Level, 0);
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
            reportDataSource6.Name = "ReportsDataset_isp_sqlsecure_report_getguestenabledservers";
            reportDataSource6.Value = null;
            reportDataSource7.Name = "ReportsDataset_isp_sqlsecure_report_getguestenabledservers";
            reportDataSource7.Value = null;
            reportDataSource8.Name = "ReportsDataset_isp_sqlsecure_report_getguestenabledservers";
            reportDataSource8.Value = null;
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource1);
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource2);
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource3);
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource4);
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource5);
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource6);
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource7);
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource8);
            this._reportViewer.LocalReport.DisplayName = "User Permissions";
            this._reportViewer.LocalReport.EnableHyperlinks = true;
            this._reportViewer.LocalReport.ReportEmbeddedResource = "Idera.SQLsecure.UI.Console.Reports.Report_UsersPermissions.rdlc";
            this._reportViewer.Size = new System.Drawing.Size(652, 468);
            this._reportViewer.TabIndex = 7;
            // 
            // _label_Description
            // 
            this._label_Description.Size = new System.Drawing.Size(1027, 17);
            // 
            // _label_ReportTitle
            // 
            this._label_ReportTitle.Size = new System.Drawing.Size(1042, 56);
            // 
            // _label_Instructions
            // 
            this._label_Instructions.Size = new System.Drawing.Size(901, 361);
            // 
            // _button_RunReport
            // 
            this._button_RunReport.TabIndex = 6;
            // 
            // _label_Server
            // 
            this._label_Server.AutoSize = true;
            this._label_Server.BackColor = System.Drawing.Color.Transparent;
            this._label_Server.Location = new System.Drawing.Point(3, 18);
            this._label_Server.Name = "_label_Server";
            this._label_Server.Size = new System.Drawing.Size(65, 13);
            this._label_Server.TabIndex = 2;
            this._label_Server.Text = "SQL Server:";
            // 
            // _comboBox_Server
            // 
            this._comboBox_Server.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._comboBox_Server.FormattingEnabled = true;
            this._comboBox_Server.Location = new System.Drawing.Point(96, 13);
            this._comboBox_Server.Name = "_comboBox_Server";
            this._comboBox_Server.Size = new System.Drawing.Size(406, 21);
            this._comboBox_Server.TabIndex = 4;
            this._comboBox_Server.DropDown += new System.EventHandler(this._comboBox_Server_DropDown);
            this._comboBox_Server.SelectionChangeCommitted += new System.EventHandler(this._comboBox_Server_SelectionChangeCommitted);
            // 
            // _label_PermissionType
            // 
            this._label_PermissionType.AutoSize = true;
            this._label_PermissionType.BackColor = System.Drawing.Color.Transparent;
            this._label_PermissionType.Location = new System.Drawing.Point(3, 44);
            this._label_PermissionType.Name = "_label_PermissionType";
            this._label_PermissionType.Size = new System.Drawing.Size(87, 13);
            this._label_PermissionType.TabIndex = 4;
            this._label_PermissionType.Text = "Permission Type:";
            // 
            // _comboBox_PermissionType
            // 
            this._comboBox_PermissionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._comboBox_PermissionType.FormattingEnabled = true;
            this._comboBox_PermissionType.Location = new System.Drawing.Point(96, 41);
            this._comboBox_PermissionType.Name = "_comboBox_PermissionType";
            this._comboBox_PermissionType.Size = new System.Drawing.Size(140, 21);
            this._comboBox_PermissionType.TabIndex = 5;
            this._comboBox_PermissionType.SelectionChangeCommitted += new System.EventHandler(this._comboBox_PermissionType_SelectionChangeCommitted);
            // 
            // _label_User
            // 
            this._label_User.AutoSize = true;
            this._label_User.BackColor = System.Drawing.Color.Transparent;
            this._label_User.Location = new System.Drawing.Point(251, 44);
            this._label_User.Name = "_label_User";
            this._label_User.Size = new System.Drawing.Size(32, 13);
            this._label_User.TabIndex = 10;
            this._label_User.Text = "User:";
            // 
            // _button_BrowseUsers
            // 
            this._button_BrowseUsers.CausesValidation = false;
            this._button_BrowseUsers.Location = new System.Drawing.Point(508, 39);
            this._button_BrowseUsers.Name = "_button_BrowseUsers";
            this._button_BrowseUsers.Size = new System.Drawing.Size(24, 23);
            this._button_BrowseUsers.TabIndex = 3;
            this._button_BrowseUsers.Text = "...";
            this._button_BrowseUsers.UseVisualStyleBackColor = true;
            this._button_BrowseUsers.Click += new System.EventHandler(this._button_BrowseUsers_Click);
            // 
            // _textBox_User
            // 
            this._textBox_User.BackColor = System.Drawing.SystemColors.Window;
            this._textBox_User.Location = new System.Drawing.Point(289, 41);
            this._textBox_User.Name = "_textBox_User";
            this._textBox_User.Size = new System.Drawing.Size(213, 20);
            this._textBox_User.TabIndex = 2;
            this._textBox_User.TextChanged += new System.EventHandler(this._textBox_User_TextChanged);
            // 
            // _radioButton_WindowsUser
            // 
            this._radioButton_WindowsUser.AutoSize = true;
            this._radioButton_WindowsUser.BackColor = System.Drawing.Color.Transparent;
            this._radioButton_WindowsUser.CausesValidation = false;
            this._radioButton_WindowsUser.Checked = true;
            this._radioButton_WindowsUser.Location = new System.Drawing.Point(96, 68);
            this._radioButton_WindowsUser.Name = "_radioButton_WindowsUser";
            this._radioButton_WindowsUser.Size = new System.Drawing.Size(138, 17);
            this._radioButton_WindowsUser.TabIndex = 0;
            this._radioButton_WindowsUser.TabStop = true;
            this._radioButton_WindowsUser.Text = "Windows User or Group";
            this._radioButton_WindowsUser.UseVisualStyleBackColor = false;
            this._radioButton_WindowsUser.Click += new System.EventHandler(this._radioButton_WindowsUser_Click);
            // 
            // _radioButton_SQLLogin
            // 
            this._radioButton_SQLLogin.AutoSize = true;
            this._radioButton_SQLLogin.BackColor = System.Drawing.Color.Transparent;
            this._radioButton_SQLLogin.CausesValidation = false;
            this._radioButton_SQLLogin.Location = new System.Drawing.Point(240, 68);
            this._radioButton_SQLLogin.Name = "_radioButton_SQLLogin";
            this._radioButton_SQLLogin.Size = new System.Drawing.Size(75, 17);
            this._radioButton_SQLLogin.TabIndex = 1;
            this._radioButton_SQLLogin.Text = "SQL Login";
            this._radioButton_SQLLogin.UseVisualStyleBackColor = false;
            this._radioButton_SQLLogin.Click += new System.EventHandler(this._radioButton_SQLLogin_Click);
            // 
            // _label_LoginType
            // 
            this._label_LoginType.AutoSize = true;
            this._label_LoginType.BackColor = System.Drawing.Color.Transparent;
            this._label_LoginType.Location = new System.Drawing.Point(3, 70);
            this._label_LoginType.Name = "_label_LoginType";
            this._label_LoginType.Size = new System.Drawing.Size(63, 13);
            this._label_LoginType.TabIndex = 14;
            this._label_LoginType.Text = "Login Type:";
            // 
            // _label_Level
            // 
            this._label_Level.AutoSize = true;
            this._label_Level.BackColor = System.Drawing.Color.Transparent;
            this._label_Level.Location = new System.Drawing.Point(565, 18);
            this._label_Level.Name = "_label_Level";
            this._label_Level.Size = new System.Drawing.Size(36, 13);
            this._label_Level.TabIndex = 15;
            this._label_Level.Text = "Level:";
            // 
            // _comboBox_Level
            // 
            this._comboBox_Level.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._comboBox_Level.FormattingEnabled = true;
            this._comboBox_Level.Items.AddRange(new object[] {
            "Member",
            "User"});
            this._comboBox_Level.Location = new System.Drawing.Point(607, 13);
            this._comboBox_Level.Name = "_comboBox_Level";
            this._comboBox_Level.Size = new System.Drawing.Size(140, 21);
            this._comboBox_Level.TabIndex = 16;
            // 
            // Report_UsersPermissions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "Report_UsersPermissions";
            this.Size = new System.Drawing.Size(1058, 588);
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
        private System.Windows.Forms.ComboBox _comboBox_PermissionType;
        private System.Windows.Forms.Label _label_PermissionType;
        private System.Windows.Forms.Label _label_User;
        private System.Windows.Forms.Button _button_BrowseUsers;
        private System.Windows.Forms.TextBox _textBox_User;
        private System.Windows.Forms.RadioButton _radioButton_WindowsUser;
        private System.Windows.Forms.RadioButton _radioButton_SQLLogin;
        private System.Windows.Forms.Label _label_LoginType;
        private System.Windows.Forms.ComboBox _comboBox_Level;
        private System.Windows.Forms.Label _label_Level;
    }
}