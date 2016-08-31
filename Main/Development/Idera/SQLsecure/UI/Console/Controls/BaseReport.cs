using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

using Idera.SQLsecure.Core.Logger;

namespace Idera.SQLsecure.UI.Console.Controls
{
    public partial class BaseReport : UserControl, Interfaces.IView, Interfaces.ICommandHandler, Interfaces.IRefresh
    {
        #region Fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Controls.BaseReport");
        protected Utility.MenuConfiguration m_menuConfiguration;
        protected string m_Title;
        protected bool _isExpanded;
        protected int m_ServerId;
        protected string m_ServerName;
        protected DateTime m_reportDate;
        protected string m_reportDateDisplay;
        protected bool m_useBaseline;
        protected bool m_usePolicy;
        protected int m_policyid;
        protected int m_assessmentid;
        protected int m_assessmentid1;
        protected int m_assessmentid2;

        #endregion

        #region IView

        void Interfaces.IView.SetContext(Interfaces.IDataContext contextIn)
        {
            //Do not set the title here, it is set in the report
        }

        String Interfaces.IView.HelpTopic
        {
            get { return Utility.Help.ReportHelpTopic; }
        }

        String Interfaces.IView.ConceptTopic
        {
            get { return Utility.Help.ReportConceptTopic; }
        }

        String Interfaces.IView.Title
        {
            get { return m_Title; }
        }

        #endregion

        #region ICommandHandler Members

        // This is identical to BaseView for consistency
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
                    Debug.Assert(false, "Unknown command passed to BaseReport");
                    break;
            }
        }

        protected virtual void showBaseline()
        {
        }

        protected virtual void showCollect()
        {
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - BaseView showCollect command called erroneously");
        }

        protected virtual void showConfigure()
        {
            // This has a base function that requires no input, so can be defaulted from here or overriden
            Sql.RegisteredServer server = Forms.Form_SelectRegisteredServer.GetServer();
            
            if (server != null)
            {
                Forms.Form_SqlServerProperties.Process(server.ConnectionName, Forms.Form_SqlServerProperties.RequestedOperation.EditCofiguration, Program.gController.isAdmin);
            }
        }

        protected virtual void showDelete()
        {
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - BaseView showDelete command called erroneously");
        }

        protected virtual void showGroupByBox()
        {
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - BaseView showGroupByBox command called erroneously");
        }

        protected virtual void showProperties()
        {
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - BaseView showProperties command called erroneously");
        }

        protected virtual void showRefresh()
        {
            runReport();
        }

        protected virtual void showPermissions(Views.View_PermissionExplorer.Tab tabIn)
        {
            if (tabIn == Views.View_PermissionExplorer.Tab.UserPermissions)
            {
                Forms.Form_WizardUserPermissions.Process();
            }
            else if (tabIn == Views.View_PermissionExplorer.Tab.ObjectPermissions)
            {
                Forms.Form_WizardObjectPermissions.Process();
            }
            else
            {
                Sql.RegisteredServer server = Forms.Form_SelectRegisteredServer.GetServer(true);
                
                if (server != null)
                {
                    Program.gController.SetCurrentServer(server);
                    Program.gController.ShowRootView(new Utility.NodeTag(new Data.PermissionExplorer(server, tabIn),
                                                                Utility.View.PermissionExplorer));
                }
            }
        }

        protected virtual void showNewLogin()
        {
            Forms.Form_WizardNewLogin.Process();
        }

        protected virtual void showNewAuditServer()
        {
            Forms.Form_WizardRegisterSQLServer.Process();
        }

        #endregion

        #region IRefresh Members

        void Interfaces.IRefresh.RefreshView()
        {
            showRefresh();
        }

        #endregion

        #region Ctors

        public BaseReport()
        {
            InitializeComponent();
            
            // Initialize menu configuration.
            m_menuConfiguration = new Utility.MenuConfiguration();

            _toolStripButton_ShowSelections.Text = SelectionsHide;
            _toolStripButton_ShowSelections.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.TaskHide_161;

            //reports always start all expanded
            _isExpanded = true;

            //set default values for fields that may not be used on all reports
            m_ServerId = 0;
            m_ServerName = allserverstext;
            m_usePolicy = true;
            m_useBaseline = false;
        }

        #endregion

        #region Constants & Queries

        private const string SelectionsShow = @"Show Selections";
        private const string SelectionsHide = @"Hide Selections";

        protected const string SqlParamPolicyid = @"@policyid";
        protected const string SqlParamAssessmentid = @"@assessmentid";
        protected const string SqlParamAssessmentid1 = @"@assessmentid1";
        protected const string SqlParamAssessmentid2 = @"@assessmentid2";
        protected const string SqlParamSnapshotid = @"@snapshotid";
        protected const string SqlParamSnapshotid2 = @"@snapshotid2";
        protected const string SqlParamStatus = @"@status";
        protected const string SqlParamStart = @"@startdate";
        protected const string SqlParamEnd = @"@enddate";
        protected const string SqlParamLogin = @"@login";
        protected const string SqlParamUsebaseline = @"@usebaseline";
        protected const string SqlParamServerName = @"@servername";
        protected const string SqlParamServerName2 = @"@serverName";    // This is because of inconsistencies in SP parameter names and should be fixed asap
        protected const string SqlParamDatabaseName = @"@databaseName";
        protected const string SqlParamServer = @"@connectionname";
        protected const string SqlParamRunDate = @"@rundate";
        protected const string SqlParamLoginType = @"@logintype";
        protected const string SqlParamSqlLogin = @"@sqllogin";
        protected const string SqlParamUser = @"@user";
        protected const string SqlParamUserType = @"@usertype";
        protected const string SqlParamPermissionType = @"@permission";
        protected const string SqlParamAlertsOnly = @"@alertsonly";
        protected const string SqlParamDiffsOnly = @"@diffsonly";
        
        protected const string SqlParamServerid = @"@registeredserverid";
        protected const string SqlParamInputSid = @"@inputsid";

        protected const string instructionformat = "  {0}.  {1}{2}";
        protected const string warningformat = "Warning:  {0}";
        protected const string newline = "\n\n";
        protected const string allserverstext = "ALL";
        protected const string allserversvalue = "%";

        // Sub-Report used for most of the reports
        protected const string SubReportQueryDataSource = @"SQLsecure.dbo.isp_sqlsecure_report_getauditedinstances";
        protected const string SubReportDataSourceName = @"ReportsDataset_isp_sqlsecure_report_getauditedinstances";

        #endregion

        #region Helpers

        protected string getServerName(string comboText)
        {
            string serverName = (comboText.CompareTo(Utility.Constants.ReportSelect_AllServers) == 0) ? allserversvalue : comboText;
            m_ServerName = (serverName.CompareTo(allserversvalue) == 0) ? allserverstext : serverName; //for the sub reports
            
            return serverName;
        }

        protected void getReportSettings()
        {
            // Get the report settings from the mainform and store locally to prevent cross-threading problems
            m_reportDate = Program.gController.ReportTime;

            if (Program.gController.UseReportTimeWithDate)
            {
                m_reportDateDisplay = string.Format(Utility.Constants.ReportDateTimeFormat, Program.gController.ReportTime.ToLocalTime().ToLongDateString(), Program.gController.ReportTime.ToLocalTime().ToLongTimeString());
            }
            else
            {
                m_reportDateDisplay = string.Format(Utility.Constants.ReportDateFormat, Program.gController.ReportTime.ToLocalTime().ToLongDateString());
            }
            m_policyid = Program.gController.ReportPolicy.PolicyId;
            m_assessmentid = Program.gController.ReportPolicy.AssessmentId;
            m_useBaseline = Program.gController.ReportUseBaseline;
        }

        protected ReportDataSource getServerListDataSet()
        {
            ReportDataSource rds = new ReportDataSource();

            try
            {
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Setup stored procedure
                    SqlCommand cmd = new SqlCommand(SubReportQueryDataSource, connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Build parameters
                    SqlParameter paramRunDate = new SqlParameter(SqlParamRunDate, m_reportDate);
                    SqlParameter paramPolicyid = new SqlParameter(SqlParamPolicyid, m_policyid);
                    SqlParameter paramAssessmentid = new SqlParameter(SqlParamAssessmentid, m_assessmentid);
                    string server = (m_ServerName == "*" || m_ServerName == allserversvalue) ? allserverstext : m_ServerName;
                    SqlParameter paramServer = new SqlParameter(SqlParamServer, server);
                    SqlParameter paramUseBaseline = new SqlParameter(SqlParamUsebaseline, m_useBaseline);
                    cmd.Parameters.Add(paramServer);
                    if (m_usePolicy)
                    {
                        cmd.Parameters.Add(paramPolicyid);
                        cmd.Parameters.Add(paramAssessmentid);
                    }
                    cmd.Parameters.Add(paramRunDate);
                    cmd.Parameters.Add(paramUseBaseline);

                    // Get data
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    rds.Name = SubReportDataSourceName;
                    rds.Value = ds.Tables[0].DefaultView;
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error("Unable to create Servers Used Summary subreport datasource", ex);
            }

            return rds;
        }

        #endregion

        #region Helpers to Override

        protected internal virtual void checkSelections()
        {
        }

        protected internal virtual void runReport()
        {
            //the header graphic, instructions and description are in the preRun panel
            _panel_Report.SuspendLayout();
            _panel_preRun.Visible = false;
            _reportViewer.Visible = true;
            _panel_Report.ResumeLayout();
            getReportSettings();
        }

        #endregion

        #region Events

        #region View events

        private void BaseReport_Enter(object sender, EventArgs e)
        {
            Program.gController.SetMenuConfiguration(m_menuConfiguration, this);
        }

        private void BaseReport_Leave(object sender, EventArgs e)
        {
            Program.gController.SetMenuConfiguration(new Utility.MenuConfiguration());
        }

        #endregion

        #region Selection Area

        private void _button_RunReport_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (_buttonExpand_All.Visible) _buttonExpand_All.Enabled = true;

            runReport();

            Cursor = Cursors.Default;
        }

        private void _buttonExpand_All_Click(object sender, EventArgs e)
        {
            //if it's not visible, we have nothing to do
            if (!_buttonExpand_All.Visible) return;

            if (_isExpanded)
            {
                //that means we need to collapse everything
                _buttonExpand_All.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.TaskShow_16;
                _buttonExpand_All.Text = "Expand All";
            }
            else
            {
                //show everything
                _buttonExpand_All.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.TaskHide_161;
                _buttonExpand_All.Text = "Collapse All";
            }

            //toggle
            _isExpanded = !_isExpanded;

            //run the report
            _button_RunReport_Click(this, null);
        }

        #endregion

        #region Status Bar

        private void _toolStripButton_ShowSelections_Click(object sender, EventArgs e)
        {
            _gradientPanel_Selections.Visible = !_gradientPanel_Selections.Visible;

            if (_gradientPanel_Selections.Visible)
            {
                _panel_Report.SuspendLayout();
                _panel_Report.Top = _gradientPanel_Selections.Height + 1;
                _panel_Report.Height -= _gradientPanel_Selections.Height + 1;
                _panel_Report.ResumeLayout();
                _toolStripButton_ShowSelections.Text = SelectionsHide;
                _toolStripButton_ShowSelections.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.TaskHide_161;
            }
            else
            {
                _panel_Report.SuspendLayout();
                _panel_Report.Top = 0;
                _panel_Report.Height += _gradientPanel_Selections.Height + 1;
                _panel_Report.ResumeLayout();
                _toolStripButton_ShowSelections.Text = SelectionsShow;
                _toolStripButton_ShowSelections.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.TaskShow_16;
            }
        }

        #endregion

        #region ReportViewer
        
        private void _reportViewer_Print(object sender, CancelEventArgs e)
        {
            //workaround for print not working on first attempt
            e.Cancel = true;
            _timer_Print.Start();
        }

        private void _timer_Print_Tick(object sender, EventArgs e)
        {
            //workaround for print not working on first attempt
            _timer_Print.Stop();
            _reportViewer.PrintDialog();
        }

        /**
         * This is super cheezy, but I wasn't able to find another way to do it.  If you do- go for it.
         * What this is doing is intercepting the hyperlink event for the report viewer.  It has to be a 
         * "Jump To URL" link, and it has to have the "http://whatever/" format to fire the event.  Currently 
         * there's two hyperlinks- one on the footer that links to the idera site and one that links to the 
         * data filters report.  If we just used a "Jump to report" type link, then we'd have to pass it 
         * parameters, which isn't really a big deal, but it also needs the data source.  All that stuff is 
         * set in the .cs file, so it's more reasonable at this point to just intercept the phony event 
         * and open the report through the .cs file, just as if it was chosen from the menu.
         */
        protected void _reportViewer_Hyperlink(object sender, Microsoft.Reporting.WinForms.HyperlinkEventArgs e)
        {
            //if it's not the data filters link, then we want to just process it as-is
            if ((e.Hyperlink.ToLower()).CompareTo("http://datafiltersreport/") != 0)
                return;

            //ok, it's the data filters link.  Let's open that report as if we came from the viewer.
            e.Cancel = true;
            Program.gController.SetCurrentReport(Utility.Constants.ReportTitle_Filters);
            Program.gController.ShowRootView(new Utility.NodeTag(new Data.Report(string.Empty), Utility.View.Report_Filters));
        }

        #endregion        

        #endregion
    }
}