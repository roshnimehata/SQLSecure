namespace Idera.SQLsecure.UI.Console.Views
{
    partial class Report_Filters
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
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource3 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource4 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource5 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource6 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource7 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource8 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource9 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource10 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this._comboBox_Server = new System.Windows.Forms.ComboBox();
            this._label_Server = new System.Windows.Forms.Label();
            this._panel_Report.SuspendLayout();
            this._gradientPanel_Selections.SuspendLayout();
            this.SuspendLayout();
            // 
            // _gradientPanel_Selections
            // 
            this._gradientPanel_Selections.Controls.Add(this._comboBox_Server);
            this._gradientPanel_Selections.Controls.Add(this._label_Server);
            this._gradientPanel_Selections.Controls.SetChildIndex(this._label_Server, 0);
            this._gradientPanel_Selections.Controls.SetChildIndex(this._comboBox_Server, 0);
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
            reportDataSource9.Name = "ReportsDataset_isp_sqlsecure_report_getguestenabledservers";
            reportDataSource9.Value = null;
            reportDataSource10.Name = "ReportsDataset_isp_sqlsecure_report_getguestenabledservers";
            reportDataSource10.Value = null;
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource1);
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource2);
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource3);
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource4);
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource5);
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource6);
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource7);
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource8);
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource9);
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource10);
            this._reportViewer.LocalReport.EnableHyperlinks = true;
            this._reportViewer.LocalReport.ReportEmbeddedResource = "Idera.SQLsecure.UI.Console.Reports.Report_Filters.rdlc";
            // 
            // _comboBox_Server
            // 
            this._comboBox_Server.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._comboBox_Server.FormattingEnabled = true;
            this._comboBox_Server.Location = new System.Drawing.Point(77, 13);
            this._comboBox_Server.Name = "_comboBox_Server";
            this._comboBox_Server.Size = new System.Drawing.Size(427, 21);
            this._comboBox_Server.TabIndex = 16;
            this._comboBox_Server.SelectionChangeCommitted += new System.EventHandler(this._comboBox_Server_SelectionChangeCommitted);
            this._comboBox_Server.DropDown += new System.EventHandler(this._comboBox_Server_DropDown);
            // 
            // _label_Server
            // 
            this._label_Server.AutoSize = true;
            this._label_Server.BackColor = System.Drawing.Color.Transparent;
            this._label_Server.Location = new System.Drawing.Point(6, 16);
            this._label_Server.Name = "_label_Server";
            this._label_Server.Size = new System.Drawing.Size(65, 13);
            this._label_Server.TabIndex = 17;
            this._label_Server.Text = "SQL Server:";
            // 
            // Report_Filters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "Report_Filters";
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

    }
}