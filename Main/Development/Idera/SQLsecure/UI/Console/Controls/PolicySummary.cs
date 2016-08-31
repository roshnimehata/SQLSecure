using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Controls;
using Idera.SQLsecure.UI.Console.Utility;

namespace Idera.SQLsecure.UI.Console.Controls
{
    public partial class PolicySummary : UserControl, Interfaces.IView, Interfaces.ICommandHandler, Interfaces.IRefresh
    {
        #region IView

        void Interfaces.IView.SetContext(Interfaces.IDataContext contextIn)
        {
            m_context = (Data.PolicySummary)contextIn;
            m_policy = m_context.Policy;
            m_server = m_context.Server;

            _viewSection_PolicyStatus.Title =
                string.Format(TitleFormat,
                              m_server == null ? (m_policy.IsAssessment ? string.Format(Display_Assessment, m_policy.AssessmentStateName) : Display_Policy) : Display_Server);

            setMenuConfiguration();

            loadDataSource();
        }
        String Interfaces.IView.HelpTopic
        {
            get { return (m_server == null) ? Utility.Help.SecuritySummaryPolicyReportCardHelpTopic : Utility.Help.SecuritySummaryServerReportCardHelpTopic; }
        }
        String Interfaces.IView.ConceptTopic
        {
            get { return (m_server == null) ? Utility.Help.SecuritySummaryPolicyReportCardHelpTopic : Utility.Help.SecuritySummaryServerReportCardHelpTopic; }
        }
        String Interfaces.IView.Title
        {
            get { return string.Empty; }
        }

        #endregion

        #region ICommandHandler Members

        void Interfaces.ICommandHandler.ProcessCommand(Utility.ViewSpecificCommand command)
        {
            switch (command)
            {
                case Utility.ViewSpecificCommand.NewAuditServer:
                    showNewAuditServer();
                    break;
                case Utility.ViewSpecificCommand.NewLogin:
                    showNewLogin();
                    break;
                case Utility.ViewSpecificCommand.Baseline:
                    showBaseline();
                    break;
                case Utility.ViewSpecificCommand.Collect:
                    showCollect();
                    break;
                case Utility.ViewSpecificCommand.Configure:
                    showConfigure();
                    break;
                case Utility.ViewSpecificCommand.Delete:
                    showDelete();
                    break;
                case Utility.ViewSpecificCommand.Properties:
                    showProperties();
                    break;
                case Utility.ViewSpecificCommand.Refresh:
                    showRefresh();
                    break;
                case Utility.ViewSpecificCommand.UserPermissions:
                    showPermissions(Views.View_PermissionExplorer.Tab.UserPermissions);
                    break;
                case Utility.ViewSpecificCommand.ObjectPermissions:
                    showPermissions(Views.View_PermissionExplorer.Tab.ObjectPermissions);
                    break;
                default:
                    Debug.Assert(false, "Unknown command passed to PolicySummary");
                    break;
            }

        }

        protected virtual void showNewAuditServer()
        {
            Forms.Form_WizardRegisterSQLServer.Process();
        }

        protected virtual void showNewLogin()
        {
            Forms.Form_WizardNewLogin.Process();
        }

        protected virtual void showBaseline()
        {
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - ReportCard showBaseline command called erroneously");
        }

        protected virtual void showCollect()
        {
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - ReportCard showCollect command called erroneously");
        }

        protected virtual void showConfigure()
        {
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - ReportCard showConfigure command called erroneously");
        }

        protected virtual void showDelete()
        {
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - ReportCard showDelete command called erroneously");
        }

        protected virtual void showProperties()
        {
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - ReportCard showProperties command called erroneously");
        }

        protected virtual void showRefresh()
        {
            //loadDataSource();
            // refresh the entire view to keep all info in sync
            Program.gController.RefreshCurrentView();
        }

        protected virtual void showPermissions(Views.View_PermissionExplorer.Tab tabIn)
        {
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - ReportCard showPermissions command called erroneously");
        }

        #endregion

        #region IRefresh Members

        public void RefreshView()
        {
            showRefresh();
        }

        #endregion

        public PolicySummary()
        {
            InitializeComponent();

            // Initialize base class fields.
            m_menuConfiguration = new Utility.MenuConfiguration();

            // hook the toolbar labels to the grids so the heading can be used for printing
            _grid_Servers.Tag = _label_Servers;

            // hook the grids to the toolbars so they can be used for button processing
            _headerStrip_Servers.Tag = _grid_Servers;

            // Hookup all application images
            _toolStripButton_ServersSave.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);
            _toolStripButton_ServersPrint.Image = AppIcons.AppImage16(AppIcons.Enum.Print);

            initDataSource();

            // Hide the focus rectangles on tabs and grids
            _grid_Servers.DrawFilter = new HideFocusRectangleDrawFilter();
        }

        #region fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Controls.PolicySummary");

        private Utility.MenuConfiguration m_menuConfiguration;
        private Data.PolicySummary m_context;

        private Sql.Policy m_policy;
        private Sql.RegisteredServer m_server;

        private DataTable m_serverTable;

        private bool m_gridCellClicked = false;

        #endregion

        #region constants

        private const string colSeverity = "Severity";
        private const string colServer = "Server";
        private const string colHigh = "High";
        private const string colMedium = "Medium";
        private const string colLow = "Low";
        private const string colStatus = "Status";

        private const string TitleFormat = "{0} Status";
        private const string SelectionsFormat = "Use most current {0}data";
        private const string Display_Baseline = @"baseline ";
        private const string Display_Assessment = @"{0} Assessment";
        private const string Display_Policy = @"Policy";
        private const string Display_Server = @"Server";

        private const string NoMetrics = "No security checks exist";

        private const int BarLeft = 49;
        private const int BarMax = 176;
        private const string BarLabelDisplay = "{0} Risk";
        private const string BarHighCountDisplay = "of {0}";

        private const string HeaderServers = @"Servers";
        private const string NoRecordsValue = "No {0} found";

        private const string HeaderDisplay = "{0} Server{1}";
        private const string PrintHeaderDisplay = "{0}\n Server Status as of {1}\n\n{2}";
        private const string PrintEmptyHeaderDisplay = "{0}";

        #endregion

        #region properties

        public Policy.SeverityExplained Status
        {
            get
            {
                return _reportCard.Status;
            }
        }

        public string StatusText
        {
            get
            {
                return _reportCard.StatusText;
            }
        }

        #endregion

        #region methods

        public void RegisterReportCardMetricChangedDelegate(ReportCard.MetricChanged value)
        {
            _reportCard.RegisterMetricChangedDelegate(value);
        }

        #endregion

        #region helpers

        protected void setMenuConfiguration()
        {
            //This is all currently being handled at the view level, but leaving functions for future
            //m_menuConfiguration.EditItems[(int)Utility.MenuItems_Edit.Remove] = false;
            //m_menuConfiguration.EditItems[(int)Utility.MenuItems_Edit.Properties] = false;

            //Program.gController.SetMenuConfiguration(m_menuConfiguration, this);
        }

        private DataTable createDataSourceServers()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(colSeverity, typeof(Image));
            dt.Columns.Add(colServer, typeof(string));
            dt.Columns.Add(colHigh, typeof(int));
            dt.Columns.Add(colMedium, typeof(int));
            dt.Columns.Add(colLow, typeof(int));
            dt.Columns.Add(colStatus, typeof(string));

            return dt;
        }

        private void initDataSource()
        {
            // Initialize the servers grid
            m_serverTable = createDataSourceServers();

            _label_Servers.Text = HeaderServers;
            _grid_Servers.SetDataBinding(m_serverTable.DefaultView, null);
        }

        private void loadDataSource()
        {
            if (m_policy.IsAssessment)
            {
                if (m_server != null)
                {
                    ((Interfaces.IView) _reportCard).SetContext(new Data.ReportCard(m_policy, m_server));
                }
                else
                {
                    ((Interfaces.IView)_reportCard).SetContext(new Data.ReportCard(m_policy));
                }
            }
            else
            {
                if (m_server != null)
                {
                    ((Interfaces.IView)_reportCard).SetContext(new Data.ReportCard(m_policy, m_context.UseBaseline, m_context.SelectionDate, m_server));
                }
                else
                {
                    ((Interfaces.IView)_reportCard).SetContext(new Data.ReportCard(m_policy, m_context.UseBaseline, m_context.SelectionDate));
                }
            }

            logX.loggerX.Info("Get " + (m_policy.IsAssessment ? "Assessment" : "Policy") + (m_server == null ? string.Empty : " Server") + " Summary Security Status");

            _viewSection_PolicyStatus.SuspendLayout();

            _viewSection_PolicyStatus.Title =
                string.Format(TitleFormat,
                              m_server == null ? (m_policy.IsAssessment ? string.Format(Display_Assessment, m_policy.AssessmentStateName) : Display_Policy) : Display_Server);

            logX.loggerX.Verbose("Load Assessment Audit Data Selections");
            if (m_policy.IsAssessment)
            {
                _label_Selections.Text = string.Format(SelectionsFormat, m_policy.UseBaseline ? Display_Baseline : string.Empty);
                if (m_policy.AssessmentDate.HasValue)
                {
                    _label_Selections.Text += "\nas of " + m_policy.AssessmentDate.Value.ToLocalTime().ToString(Utility.Constants.DATETIME_FORMAT);
                }
            }
            else
            {
                _label_Selections.Text = string.Format(SelectionsFormat, Program.gController.PolicyUseBaselineSnapshots ? Display_Baseline : string.Empty);
                if (Program.gController.PolicyTime.HasValue)
                {
                    _label_Selections.Text += "\nas of " + Program.gController.PolicyTime.Value.ToLocalTime().ToString(Utility.Constants.DATETIME_FORMAT);
                }
            }

            try
            {
                List<Sql.PolicyMetric> metrics = m_policy.GetPolicyMetrics(Program.gController.Repository.ConnectionString);
                int high = 0;
                int medium = 0;
                int low = 0;
                logX.loggerX.Verbose("Process Security Check Counts");

                foreach (Sql.PolicyMetric metric in metrics)
                {
                    if (metric.IsEnabled)
                    {
                        if (metric.Severity == (int) Policy.Severity.High)
                        {
                            high++;
                        }
                        if (metric.Severity == (int) Policy.Severity.Medium)
                        {
                            medium++;
                        }
                        if (metric.Severity == (int) Policy.Severity.Low)
                        {
                            low++;
                        }
                    }
                }

                logX.loggerX.Verbose("Load " + (m_policy.IsAssessment ? "Assessment" : "Policy") + " Description");
                if (m_policy.IsAssessment)
                {
                    _label_Description.Text = string.IsNullOrEmpty(m_policy.AssessmentDescription)
                                                  ? m_policy.AssessmentName
                                                  : m_policy.AssessmentDescription;
                    _toolTip_Description.SetToolTip(_label_Description,
                                                    string.Format("{0}\n{1}", m_policy.PolicyAssessmentName,
                                                                  m_policy.AssessmentDescription));
                }
                else
                {
                    _label_Description.Text = string.IsNullOrEmpty(m_policy.PolicyDescription)
                                                  ? m_policy.PolicyName
                                                  : m_policy.PolicyDescription;
                    _toolTip_Description.SetToolTip(_label_Description,
                                                    string.Format("{0}\n{1}", m_policy.PolicyName,
                                                                  m_policy.PolicyDescription));
                }

                // Set the assessment statuses

                //High
                logX.loggerX.Verbose("Load " + (m_policy.IsAssessment ? "Assessment" : "Policy") + " High Status Bar");
                _pictureBox_SecurityStatusHigh.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.HighRisk_32;
                _label_High.Text = string.Format(BarLabelDisplay, DescriptionHelper.GetEnumDescription(Policy.Severity.High));
                _label_HighCount.Text = string.Format(BarHighCountDisplay, high);

                if (high > 0)
                {
                    _label_HighMsg.Visible = false;
                    _label_HighRiskCount.Text = _reportCard.Risks.RiskCountHigh.ToString();
                    if (_reportCard.Risks.RiskCountHigh > 0)
                    {
                        _label_HighRiskBar.Width = Convert.ToInt16(BarMax*_reportCard.Risks.RiskCountHigh/high);
                        _label_HighBar.Left = BarLeft + _label_HighRiskBar.Width;
                        _label_HighBar.Width = BarMax - _label_HighRiskBar.Width;
                    }
                    else
                    {
                        _label_HighRiskBar.Width = 0;
                        _label_HighBar.Left = BarLeft;
                        _label_HighBar.Width = BarMax;
                    }
                    _label_HighRiskBar.Visible =
                        _label_HighBar.Visible = true;
                }
                else
                {
                    _label_HighRiskCount.Text = string.Empty;
                    _label_HighMsg.Text = NoMetrics;
                    _label_HighMsg.Visible = true;
                    _label_HighRiskBar.Visible =
                        _label_HighBar.Visible = false;
                }

                //Medium
                logX.loggerX.Verbose("Load " + (m_policy.IsAssessment ? "Assessment" : "Policy") + " Medium Status Bar");
                _pictureBox_SecurityStatusMedium.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.MediumRisk_32;
                _label_Medium.Text = string.Format(BarLabelDisplay, DescriptionHelper.GetEnumDescription(Policy.Severity.Medium));
                _label_MediumCount.Text = string.Format(BarHighCountDisplay, medium);

                if (medium > 0)
                {
                    _label_MediumMsg.Visible = false;
                    _label_MediumRiskCount.Text = _reportCard.Risks.RiskCountMedium.ToString();
                    if (_reportCard.Risks.RiskCountMedium > 0)
                    {
                        _label_MediumRiskBar.Width = Convert.ToInt16(BarMax*_reportCard.Risks.RiskCountMedium/medium);
                        _label_MediumBar.Left = BarLeft + _label_MediumRiskBar.Width;
                        _label_MediumBar.Width = BarMax - _label_MediumRiskBar.Width;
                    }
                    else
                    {
                        _label_MediumRiskBar.Width = 0;
                        _label_MediumBar.Left = BarLeft;
                        _label_MediumBar.Width = BarMax;
                    }
                    _label_MediumRiskBar.Visible =
                        _label_MediumBar.Visible = true;
                }
                else
                {
                    _label_MediumRiskCount.Text = string.Empty;
                    _label_MediumMsg.Text = NoMetrics;
                    _label_MediumMsg.Visible = true;
                    _label_MediumRiskBar.Visible =
                        _label_MediumBar.Visible = false;
                }

                //Low
                logX.loggerX.Verbose("Load " + (m_policy.IsAssessment ? "Assessment" : "Policy") + " Low Status Bar");
                _pictureBox_SecurityStatusLow.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.LowRisk_32;
                _label_Low.Text = string.Format(BarLabelDisplay, DescriptionHelper.GetEnumDescription(Policy.Severity.Low));
                _label_LowCount.Text = string.Format(BarHighCountDisplay, low);

                if (low > 0)
                {
                    _label_LowMsg.Visible = false;
                    _label_LowRiskCount.Text = _reportCard.Risks.RiskCountLow.ToString();
                    if (_reportCard.Risks.RiskCountLow > 0)
                    {
                        _label_LowRiskBar.Width = Convert.ToInt16(BarMax*_reportCard.Risks.RiskCountLow/low);
                        _label_LowBar.Left = BarLeft + _label_LowRiskBar.Width;
                        _label_LowBar.Width = BarMax - _label_LowRiskBar.Width;
                    }
                    else
                    {
                        _label_LowRiskBar.Width = 0;
                        _label_LowBar.Left = BarLeft;
                        _label_LowBar.Width = BarMax;
                    }
                    _label_LowRiskBar.Visible =
                        _label_LowBar.Visible = true;
                }
                else
                {
                    _label_LowRiskCount.Text = string.Empty;
                    _label_LowMsg.Text = NoMetrics;
                    _label_LowMsg.Visible = true;
                    _label_LowRiskBar.Visible =
                        _label_LowBar.Visible = false;
                }

                if (m_server == null)
                {
                    _grid_Servers.SuspendLayout();

                    m_serverTable.Clear();

                    logX.loggerX.Verbose("Load " + (m_policy.IsAssessment ? "Assessment" : "Policy") + " Server Counts");
                    foreach (KeyValuePair<string, ReportCard.RiskCounts> server in _reportCard.ServerRisks)
                    {
                        if (server.Key.Length > 0)
                        {
                            logX.loggerX.Verbose("Loading Server " + server.Key);
                            DataRow row = m_serverTable.NewRow();
                            row[colSeverity] = server.Value.HighestRiskImage;
                            row[colServer] = server.Key;
                            row[colHigh] = server.Value.RiskCountHigh;
                            row[colMedium] = server.Value.RiskCountMedium;
                            row[colLow] = server.Value.RiskCountLow;
                            row[colStatus] = server.Value.AllRisksText;

                            m_serverTable.Rows.Add(row);
                        }
                    }

                    DataView dv = m_serverTable.DefaultView;
                    dv.Sort = colServer;

                    _grid_Servers.SetDataBinding(m_serverTable.DefaultView, null);

                    _label_Servers.Text = string.Format(HeaderDisplay,
                                                        dv.Count,
                                                        dv.Count == 1 ? string.Empty : "s");

                    _grid_Servers.ResumeLayout();

                    _viewSection_ServerSummary.Visible = true;
                    _viewSection_ServerInfo.Visible = false;
                }
                else
                {
                    logX.loggerX.Verbose("Load " + (m_policy.IsAssessment ? "Assessment" : "Policy") + " Server Info");
                    _label_Server.Text = m_server.ConnectionName;
                    _label_Version.Text = m_server.VersionFriendlyLong;
                    _label_Edition.Text = m_server.Edition;
                    _label_Os.Text = m_server.OS;

                    Dictionary<int, int> snapshots = m_policy.PolicySnapshotList;
                    int snapshotId;
                    snapshots.TryGetValue(m_server.RegisteredServerId, out snapshotId);

                    Sql.Snapshot snap = null;
                    if (snapshotId > 0)
                    {
                        snap = Sql.Snapshot.GetSnapShot(snapshotId);
                    }

                    if (snap != null)
                    {
                        _label_Audited.Text = snap.StartTime.ToLocalTime().ToString(Utility.Constants.DATETIME_FORMAT);
                    }
                    else
                    {
                        _label_Audited.Text = @"Unknown";
                    }

                    _viewSection_ServerInfo.Visible = true;
                    _viewSection_ServerSummary.Visible = false;
                    _grid_Servers.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error(@"Error - Unable to retrieve " + (m_policy.IsAssessment ? "Assessment" : "Policy") + (m_server == null ? string.Empty : " Server") + " summary info", ex);
                MsgBox.ShowError(string.Format(ErrorMsgs.CantGetPolicyInfoMsg, (m_policy.IsAssessment ? "Assessment" : "Policy") + " Summary Info"),
                                 ErrorMsgs.ErrorProcessPolicyInfo,
                                 ex);
                initDataSource();

                _grid_Servers.ResumeLayout();
            }

            _viewSection_PolicyStatus.ResumeLayout();
        }

        #region Grid

        protected void printGrid(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            Debug.Assert(grid.Tag.GetType() == typeof(ToolStripLabel));

            // Associate the print document with the grid & preview dialog here
            // in case we want to change documents for each grid
            _ultraGridPrintDocument.Grid = grid;
            _ultraGridPrintDocument.DefaultPageSettings.Color = false;
            _ultraGridPrintDocument.DocumentName = ErrorMsgs.UserPermissionsCaption;
            if (m_policy != null)
            {
                _ultraGridPrintDocument.Header.TextLeft =
                    string.Format(PrintHeaderDisplay,
                                        m_policy.PolicyName,
                                        DateTime.Now,
                                        ((ToolStripLabel)grid.Tag).Text
                                    );
            }
            else
            {
                _ultraGridPrintDocument.Header.TextLeft =
                    string.Format(PrintEmptyHeaderDisplay,
                                        ((ToolStripLabel)grid.Tag).Text
                                    );
            }
            //_ultraGridPrintDocument.Footer.TextCenter =
            //    string.Format("Page {0}",
            //                    _ultraGridPrintDocument.PageNumber,
            //                    _ultraPrintPreviewDialog.pag
            _ultraPrintPreviewDialog.Document = _ultraGridPrintDocument;

            // Call ShowDialog to show the print preview dialog.
            _ultraPrintPreviewDialog.ShowDialog();
        }

        protected void saveGrid(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            //bool iconHidden = false;
            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ////save the current state of the icon column and then hide it before exporting
                    //if (grid.DisplayLayout.Bands[0].Columns.Exists(colIcon))
                    //{
                    //    // this column doesn't exist in the raw data hack
                    //    iconHidden = grid.DisplayLayout.Bands[0].Columns[colIcon].Hidden;
                    //    grid.DisplayLayout.Bands[0].Columns[colIcon].Hidden = true;
                    //    // Fix the sub list display on the effective grid
                    //    if (grid.DisplayLayout.Bands.Count > 1
                    //        && grid.DisplayLayout.Bands[1].Columns.Exists(colIcon))
                    //    {
                    //        grid.DisplayLayout.Bands[1].Columns[colIcon].Hidden = true;
                    //    }
                    //}
                    _ultraGridExcelExporter.Export(grid, _saveFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MsgBox.ShowError(ErrorMsgs.ExportToExcelCaption, ErrorMsgs.FailedToExportToExcelFile, ex);
                }
                //if (grid.DisplayLayout.Bands[0].Columns.Exists(colIcon))
                //{
                //    // this column doesn't exist in the raw data hack
                //    grid.DisplayLayout.Bands[0].Columns[colIcon].Hidden = iconHidden;
                //}
                //// Fix the sub list display on the effective grid
                //if (grid.DisplayLayout.Bands.Count > 1
                //    && grid.DisplayLayout.Bands[1].Columns.Exists(colIcon))
                //{
                //    grid.DisplayLayout.Bands[1].Columns[colIcon].Hidden = iconHidden;
                //}
            }
        }

        #endregion

        #endregion

        #region Events

        private void PolicySummary_Enter(object sender, EventArgs e)
        {
            setMenuConfiguration();
        }

        #region grid

        private void _grid_Servers_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            UltraGridBand band = e.Layout.Bands[0];

            band.Columns[colSeverity].Header.Caption = string.Empty;
            band.Columns[colSeverity].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            band.Columns[colSeverity].Width = 20;
            EditorWithText textEditor = new EditorWithText();
            band.Columns[colSeverity].Editor = textEditor;
            band.Columns[colSeverity].Hidden = true;

            band.Columns[colServer].Width = 160;

            band.Columns[colHigh].Header.Caption = string.Empty;
            band.Columns[colHigh].Header.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.HighRisk_16;
            band.Columns[colHigh].Header.Appearance.ImageHAlign = HAlign.Center;
            band.Columns[colHigh].CellAppearance.TextHAlign = HAlign.Right;
            band.Columns[colHigh].Width = 24;

            band.Columns[colMedium].Header.Caption = string.Empty;
            band.Columns[colMedium].Header.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.MediumRisk_16;
            band.Columns[colMedium].Header.Appearance.ImageHAlign = HAlign.Center;
            band.Columns[colMedium].CellAppearance.TextHAlign = HAlign.Right;
            band.Columns[colMedium].Width = 24;

            band.Columns[colLow].Header.Caption = string.Empty;
            band.Columns[colLow].Header.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.LowRisk_16;
            band.Columns[colLow].Header.Appearance.ImageHAlign = HAlign.Center;
            band.Columns[colLow].CellAppearance.TextHAlign = HAlign.Right;
            band.Columns[colLow].Width = 24;

            band.Columns[colStatus].Header.Caption = "Findings";
            band.Columns[colStatus].Hidden = true;
        }

        // Make right click select row.
        // Also, make clicking off of an element clear selected row
        // --------------------------------------------------------
        private void _grid_Servers_MouseDown(object sender, MouseEventArgs e)
        {
            Infragistics.Win.UIElement elementMain;
            Infragistics.Win.UIElement elementUnderMouse;

            elementMain = this._grid_Servers.DisplayLayout.UIElement;

            elementUnderMouse = elementMain.ElementFromPoint(new Point(e.X, e.Y));
            if (elementUnderMouse != null)
            {
                Infragistics.Win.UltraWinGrid.UltraGridCell cell = elementUnderMouse.GetContext(typeof(Infragistics.Win.UltraWinGrid.UltraGridCell)) as Infragistics.Win.UltraWinGrid.UltraGridCell;
                if (cell != null)
                {
                    m_gridCellClicked = true;
                    Infragistics.Win.UltraWinGrid.SelectedRowsCollection sr = _grid_Servers.Selected.Rows;
                    if (sr.Count > 0)
                    {
                        foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in sr)
                        {
                            row.Selected = false;
                        }
                    }
                    cell.Row.Selected = true;
                    _grid_Servers.ActiveRow = cell.Row;
                }
                else
                {
                    m_gridCellClicked = false;
                    Infragistics.Win.UltraWinGrid.HeaderUIElement he = elementUnderMouse.GetAncestor(typeof(Infragistics.Win.UltraWinGrid.HeaderUIElement)) as Infragistics.Win.UltraWinGrid.HeaderUIElement;
                    Infragistics.Win.UltraWinGrid.ColScrollbarUIElement ce = elementUnderMouse.GetAncestor(typeof(Infragistics.Win.UltraWinGrid.ColScrollbarUIElement)) as Infragistics.Win.UltraWinGrid.ColScrollbarUIElement;
                    Infragistics.Win.UltraWinGrid.RowScrollbarUIElement re = elementUnderMouse.GetAncestor(typeof(Infragistics.Win.UltraWinGrid.RowScrollbarUIElement)) as Infragistics.Win.UltraWinGrid.RowScrollbarUIElement;
                    if (he == null && ce == null && re == null)
                    {
                        _grid_Servers.Selected.Rows.Clear();
                        _grid_Servers.ActiveRow = null;
                    }
                }
            }
        }

        private void _grid_Servers_DoubleClick(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (m_gridCellClicked && _grid_Servers.ActiveRow != null)
            {
                string server = _grid_Servers.ActiveRow.Cells[colServer].Text;
                Program.gController.SetCurrentPolicyServer(server);
            }

            Cursor = Cursors.Default;
        }

        #endregion

        #region tool strips

        private void _toolStripButton_GridPrint_Click(object sender, EventArgs e)
        {
            // Note: This event handler is used by all grid toolbars
            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            Cursor = Cursors.WaitCursor;

            printGrid(grid);

            Cursor = Cursors.Default;
        }

        private void _toolStripButton_GridSave_Click(object sender, EventArgs e)
        {
            // Note: This event handler is used by all grid toolbars
            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            Cursor = Cursors.WaitCursor;

            saveGrid(grid);

            Cursor = Cursors.Default;
        }

        #endregion

        #region context menus

        private void _cmsi_grid_Print_Click(object sender, EventArgs e)
        {
            // Note: This event handler is used by all grid context menus
            Cursor = Cursors.WaitCursor;

            UltraGrid grid = (UltraGrid)((ContextMenuStrip)((ToolStripItem)sender).Owner).SourceControl;

            printGrid(grid);

            Cursor = Cursors.Default;
        }

        private void _cmsi_grid_Save_Click(object sender, EventArgs e)
        {
            // Note: This event handler is used by all grid context menus
            Cursor = Cursors.WaitCursor;

            UltraGrid grid = (UltraGrid)((ContextMenuStrip)((ToolStripItem)sender).Owner).SourceControl;

            saveGrid(grid);

            Cursor = Cursors.Default;
        }

        #endregion

        #endregion
    }
}
