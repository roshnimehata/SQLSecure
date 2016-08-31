namespace Idera.SQLsecure.UI.Console.Views
{
    partial class Report_DatabaseChaining
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
            this._panel_Report.SuspendLayout();
            this.SuspendLayout();
            // 
            // _panel_Report
            // 
            this._panel_Report.Location = new System.Drawing.Point(0, 44);
            this._panel_Report.Size = new System.Drawing.Size(652, 522);
            // 
            // _gradientPanel_Selections
            // 
            this._gradientPanel_Selections.Size = new System.Drawing.Size(652, 44);
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
            reportDataSource5.Name = "ReportsDataset_isp_sqlsecure_report_checkdbchaining";
            reportDataSource5.Value = null;
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource1);
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource2);
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource3);
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource4);
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource5);
            this._reportViewer.LocalReport.EnableHyperlinks = true;
            this._reportViewer.LocalReport.ReportEmbeddedResource = "Idera.SQLsecure.UI.Console.Reports.Report_DatabaseChaining.rdlc";
            this._reportViewer.Size = new System.Drawing.Size(652, 522);
            // 
            // _button_RunReport
            // 
            this._button_RunReport.TabIndex = 0;
            // 
            // _buttonExpand_All
            // 
            this._buttonExpand_All.Visible = false;
            // 
            // Report_DatabaseChaining
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "Report_DatabaseChaining";
            this._panel_Report.ResumeLayout(false);
            this._panel_Report.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

    }
}