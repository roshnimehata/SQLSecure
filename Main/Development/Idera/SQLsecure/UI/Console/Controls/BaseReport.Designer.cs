using Microsoft.Reporting.WinForms;

namespace Idera.SQLsecure.UI.Console.Controls
{
    partial class BaseReport
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
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this._panel_Report = new System.Windows.Forms.Panel();
            this._panel_preRun = new System.Windows.Forms.Panel();
            this._label_GettingStartedTitle = new System.Windows.Forms.Label();
            this._label_Description = new System.Windows.Forms.Label();
            this._label_DescriptionTitle = new System.Windows.Forms.Label();
            this._label_Instructions = new System.Windows.Forms.Label();
            this._label_ReportTitle = new System.Windows.Forms.Label();
            this._reportViewer = new Microsoft.Reporting.WinForms.ReportViewer();
            this._timer_Print = new System.Windows.Forms.Timer(this.components);
            this._statusStrip = new System.Windows.Forms.StatusStrip();
            this._label_Status = new System.Windows.Forms.ToolStripStatusLabel();
            this._toolStripButton_ShowSelections = new System.Windows.Forms.ToolStripButton();
            this._gradientPanel_Selections = new Idera.SQLsecure.UI.Console.Controls.GradientPanel();
            this._panel_line = new System.Windows.Forms.Panel();
            this._panel_buttonHolder = new System.Windows.Forms.Panel();
            this._button_RunReport = new Infragistics.Win.Misc.UltraButton();
            this._buttonExpand_All = new Infragistics.Win.Misc.UltraButton();
            this._panel_Report.SuspendLayout();
            this._panel_preRun.SuspendLayout();
            this._statusStrip.SuspendLayout();
            this._gradientPanel_Selections.SuspendLayout();
            this._panel_buttonHolder.SuspendLayout();
            this.SuspendLayout();
            // 
            // _panel_Report
            // 
            this._panel_Report.AutoScroll = true;
            this._panel_Report.AutoSize = true;
            this._panel_Report.BackColor = System.Drawing.Color.White;
            this._panel_Report.Controls.Add(this._panel_preRun);
            this._panel_Report.Controls.Add(this._reportViewer);
            this._panel_Report.Dock = System.Windows.Forms.DockStyle.Fill;
            this._panel_Report.Location = new System.Drawing.Point(0, 80);
            this._panel_Report.Name = "_panel_Report";
            this._panel_Report.Size = new System.Drawing.Size(652, 486);
            this._panel_Report.TabIndex = 28;
            // 
            // _panel_preRun
            // 
            this._panel_preRun.AutoScroll = true;
            this._panel_preRun.AutoSize = true;
            this._panel_preRun.Controls.Add(this._label_GettingStartedTitle);
            this._panel_preRun.Controls.Add(this._label_Description);
            this._panel_preRun.Controls.Add(this._label_DescriptionTitle);
            this._panel_preRun.Controls.Add(this._label_Instructions);
            this._panel_preRun.Controls.Add(this._label_ReportTitle);
            this._panel_preRun.Dock = System.Windows.Forms.DockStyle.Fill;
            this._panel_preRun.Location = new System.Drawing.Point(0, 0);
            this._panel_preRun.Name = "_panel_preRun";
            this._panel_preRun.Size = new System.Drawing.Size(652, 486);
            this._panel_preRun.TabIndex = 15;
            // 
            // _label_GettingStartedTitle
            // 
            this._label_GettingStartedTitle.AutoSize = true;
            this._label_GettingStartedTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._label_GettingStartedTitle.Location = new System.Drawing.Point(11, 110);
            this._label_GettingStartedTitle.Name = "_label_GettingStartedTitle";
            this._label_GettingStartedTitle.Size = new System.Drawing.Size(115, 16);
            this._label_GettingStartedTitle.TabIndex = 16;
            this._label_GettingStartedTitle.Text = "Getting Started:";
            // 
            // _label_Description
            // 
            this._label_Description.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._label_Description.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._label_Description.Location = new System.Drawing.Point(132, 65);
            this._label_Description.Name = "_label_Description";
            this._label_Description.Size = new System.Drawing.Size(512, 35);
            this._label_Description.TabIndex = 11;
            this._label_Description.Text = "Report Description";
            // 
            // _label_DescriptionTitle
            // 
            this._label_DescriptionTitle.AutoSize = true;
            this._label_DescriptionTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._label_DescriptionTitle.Location = new System.Drawing.Point(11, 65);
            this._label_DescriptionTitle.Name = "_label_DescriptionTitle";
            this._label_DescriptionTitle.Size = new System.Drawing.Size(91, 16);
            this._label_DescriptionTitle.TabIndex = 15;
            this._label_DescriptionTitle.Text = "Description:";
            // 
            // _label_Instructions
            // 
            this._label_Instructions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._label_Instructions.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._label_Instructions.Location = new System.Drawing.Point(132, 110);
            this._label_Instructions.Name = "_label_Instructions";
            this._label_Instructions.Size = new System.Drawing.Size(512, 361);
            this._label_Instructions.TabIndex = 12;
            this._label_Instructions.Text = "Run Instructions";
            // 
            // _label_ReportTitle
            // 
            this._label_ReportTitle.AutoEllipsis = true;
            this._label_ReportTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this._label_ReportTitle.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._label_ReportTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this._label_ReportTitle.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.UI_header_notagline_with_bo;
            this._label_ReportTitle.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this._label_ReportTitle.Location = new System.Drawing.Point(0, 0);
            this._label_ReportTitle.Name = "_label_ReportTitle";
            this._label_ReportTitle.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this._label_ReportTitle.Size = new System.Drawing.Size(652, 56);
            this._label_ReportTitle.TabIndex = 14;
            this._label_ReportTitle.Text = "Report Title";
            this._label_ReportTitle.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // _reportViewer
            // 
            this._reportViewer.AutoScroll = true;
            this._reportViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "ReportsDataset_isp_sqlsecure_report_getguestenabledservers";
            reportDataSource1.Value = null;
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource1);
            this._reportViewer.LocalReport.EnableHyperlinks = true;
            this._reportViewer.LocalReport.ReportEmbeddedResource = "Idera.SQLsecure.UI.Console.Reports.Report_GuestEnabledDatabases.rdlc";
            this._reportViewer.Location = new System.Drawing.Point(0, 0);
            this._reportViewer.Name = "_reportViewer";
            this._reportViewer.Size = new System.Drawing.Size(652, 486);
            this._reportViewer.TabIndex = 3;
            this._reportViewer.Visible = false;

            //next line replaced because of compilation error for studio 2013
            this._reportViewer.Print += new System.ComponentModel.CancelEventHandler(this._reportViewer_Print);

            this._reportViewer.Hyperlink += new Microsoft.Reporting.WinForms.HyperlinkEventHandler(this._reportViewer_Hyperlink);
            // 
            // _timer_Print
            // 
            this._timer_Print.Tick += new System.EventHandler(this._timer_Print_Tick);
            // 
            // _statusStrip
            // 
            this._statusStrip.AutoSize = false;
            this._statusStrip.GripMargin = new System.Windows.Forms.Padding(0);
            this._statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._label_Status,
            this._toolStripButton_ShowSelections});
            this._statusStrip.Location = new System.Drawing.Point(0, 566);
            this._statusStrip.Name = "_statusStrip";
            this._statusStrip.Size = new System.Drawing.Size(652, 22);
            this._statusStrip.SizingGrip = false;
            this._statusStrip.TabIndex = 30;
            // 
            // _label_Status
            // 
            this._label_Status.AutoToolTip = true;
            this._label_Status.BackColor = System.Drawing.Color.Transparent;
            this._label_Status.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._label_Status.Name = "_label_Status";
            this._label_Status.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this._label_Status.Size = new System.Drawing.Size(538, 17);
            this._label_Status.Spring = true;
            this._label_Status.Text = "Report Status";
            this._label_Status.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _toolStripButton_ShowSelections
            // 
            this._toolStripButton_ShowSelections.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._toolStripButton_ShowSelections.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.TaskHide_161;
            this._toolStripButton_ShowSelections.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._toolStripButton_ShowSelections.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this._toolStripButton_ShowSelections.Name = "_toolStripButton_ShowSelections";
            this._toolStripButton_ShowSelections.Size = new System.Drawing.Size(99, 20);
            this._toolStripButton_ShowSelections.Text = "Hide Selections";
            this._toolStripButton_ShowSelections.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._toolStripButton_ShowSelections.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this._toolStripButton_ShowSelections.Click += new System.EventHandler(this._toolStripButton_ShowSelections_Click);
            // 
            // _gradientPanel_Selections
            // 
            this._gradientPanel_Selections.BackColor = System.Drawing.Color.Transparent;
            this._gradientPanel_Selections.Controls.Add(this._panel_line);
            this._gradientPanel_Selections.Controls.Add(this._panel_buttonHolder);
            this._gradientPanel_Selections.Dock = System.Windows.Forms.DockStyle.Top;
            this._gradientPanel_Selections.GradientColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this._gradientPanel_Selections.Location = new System.Drawing.Point(0, 0);
            this._gradientPanel_Selections.Name = "_gradientPanel_Selections";
            this._gradientPanel_Selections.Rotation = 90F;
            this._gradientPanel_Selections.Size = new System.Drawing.Size(652, 80);
            this._gradientPanel_Selections.TabIndex = 27;
            // 
            // _panel_line
            // 
            this._panel_line.BackColor = System.Drawing.Color.Black;
            this._panel_line.Dock = System.Windows.Forms.DockStyle.Right;
            this._panel_line.Location = new System.Drawing.Point(546, 0);
            this._panel_line.Margin = new System.Windows.Forms.Padding(0);
            this._panel_line.Name = "_panel_line";
            this._panel_line.Size = new System.Drawing.Size(1, 80);
            this._panel_line.TabIndex = 13;
            // 
            // _panel_buttonHolder
            // 
            this._panel_buttonHolder.BackColor = System.Drawing.Color.Transparent;
            this._panel_buttonHolder.Controls.Add(this._button_RunReport);
            this._panel_buttonHolder.Controls.Add(this._buttonExpand_All);
            this._panel_buttonHolder.Dock = System.Windows.Forms.DockStyle.Right;
            this._panel_buttonHolder.Location = new System.Drawing.Point(547, 0);
            this._panel_buttonHolder.Name = "_panel_buttonHolder";
            this._panel_buttonHolder.Size = new System.Drawing.Size(105, 80);
            this._panel_buttonHolder.TabIndex = 11;
            // 
            // _button_RunReport
            // 
            this._button_RunReport.AcceptsFocus = false;
            appearance2.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.RunReport;
            this._button_RunReport.Appearance = appearance2;
            this._button_RunReport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsVistaButton;
            this._button_RunReport.Location = new System.Drawing.Point(6, 12);
            this._button_RunReport.Name = "_button_RunReport";
            this._button_RunReport.Size = new System.Drawing.Size(94, 27);
            this._button_RunReport.TabIndex = 13;
            this._button_RunReport.Text = "View Report";
            this._button_RunReport.Click += new System.EventHandler(this._button_RunReport_Click);
            // 
            // _buttonExpand_All
            // 
            this._buttonExpand_All.AcceptsFocus = false;
            appearance1.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.TaskHide_161;
            this._buttonExpand_All.Appearance = appearance1;
            this._buttonExpand_All.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsVistaButton;
            this._buttonExpand_All.Enabled = false;
            this._buttonExpand_All.Location = new System.Drawing.Point(6, 44);
            this._buttonExpand_All.Name = "_buttonExpand_All";
            this._buttonExpand_All.Size = new System.Drawing.Size(94, 27);
            this._buttonExpand_All.TabIndex = 12;
            this._buttonExpand_All.Text = "Collapse All";
            this._buttonExpand_All.Click += new System.EventHandler(this._buttonExpand_All_Click);
            // 
            // BaseReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this._panel_Report);
            this.Controls.Add(this._statusStrip);
            this.Controls.Add(this._gradientPanel_Selections);
            this.Name = "BaseReport";
            this.Size = new System.Drawing.Size(652, 588);
            this.Leave += new System.EventHandler(this.BaseReport_Leave);
            this.Enter += new System.EventHandler(this.BaseReport_Enter);
            this._panel_Report.ResumeLayout(false);
            this._panel_Report.PerformLayout();
            this._panel_preRun.ResumeLayout(false);
            this._panel_preRun.PerformLayout();
            this._statusStrip.ResumeLayout(false);
            this._statusStrip.PerformLayout();
            this._gradientPanel_Selections.ResumeLayout(false);
            this._panel_buttonHolder.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.Panel _panel_Report;
        protected GradientPanel _gradientPanel_Selections;
        protected System.Windows.Forms.Timer _timer_Print;
        protected Microsoft.Reporting.WinForms.ReportViewer _reportViewer;
        protected System.Windows.Forms.Label _label_Description;
        protected System.Windows.Forms.Label _label_ReportTitle;
        private System.Windows.Forms.Panel _panel_preRun;
        private System.Windows.Forms.Label _label_GettingStartedTitle;
        private System.Windows.Forms.Label _label_DescriptionTitle;
        protected System.Windows.Forms.Label _label_Instructions;
        private System.Windows.Forms.Panel _panel_buttonHolder;
        private System.Windows.Forms.Panel _panel_line;
        private System.Windows.Forms.StatusStrip _statusStrip;
        protected System.Windows.Forms.ToolStripStatusLabel _label_Status;
        private System.Windows.Forms.ToolStripButton _toolStripButton_ShowSelections;
        protected Infragistics.Win.Misc.UltraButton _button_RunReport;
        protected Infragistics.Win.Misc.UltraButton _buttonExpand_All;
    }
}