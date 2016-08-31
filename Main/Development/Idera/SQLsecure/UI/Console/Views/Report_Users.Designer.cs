namespace Idera.SQLsecure.UI.Console.Views
{
    partial class Report_Users
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
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource7 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource8 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource9 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource10 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource11 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource12 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this._panel_Report.SuspendLayout();
            this.SuspendLayout();
            // 
            // _reportViewer
            // 
            reportDataSource7.Name = "ReportsDataset_isp_sqlsecure_report_getguestenabledservers";
            reportDataSource7.Value = null;
            reportDataSource8.Name = "ReportsDataset_isp_sqlsecure_report_getguestenabledservers";
            reportDataSource8.Value = null;
            reportDataSource9.Name = "ReportsDataset_isp_sqlsecure_report_getguestenabledservers";
            reportDataSource9.Value = null;
            reportDataSource10.Name = "ReportsDataset_isp_sqlsecure_report_getguestenabledservers";
            reportDataSource10.Value = null;
            reportDataSource11.Name = "ReportsDataset_isp_sqlsecure_report_getguestenabledservers";
            reportDataSource11.Value = null;
            reportDataSource12.Name = "ReportsDataset_isp_sqlsecure_report_getauditedinstances";
            reportDataSource12.Value = null;
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource7);
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource8);
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource9);
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource10);
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource11);
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource12);
            this._reportViewer.LocalReport.EnableHyperlinks = true;
            this._reportViewer.LocalReport.ReportEmbeddedResource = "Idera.SQLsecure.UI.Console.Reports.Report_Users.rdlc";
            // 
            // _button_RunReport
            // 
            this._button_RunReport.TabIndex = 0;
            // 
            // Report_Users
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "Report_Users";
            this._panel_Report.ResumeLayout(false);
            this._panel_Report.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}