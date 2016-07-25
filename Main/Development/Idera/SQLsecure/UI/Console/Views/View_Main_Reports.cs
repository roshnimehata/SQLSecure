using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

using Idera.SQLsecure.UI.Console.Controls;
using Idera.SQLsecure.UI.Console.Forms;

using Infragistics.Win.UltraWinTabControl;

namespace Idera.SQLsecure.UI.Console.Views
{
    public partial class View_Main_Reports : Idera.SQLsecure.UI.Console.Controls.BaseView, Interfaces.IView, Interfaces.IRefresh
    {
        public enum Tab
        {
            None = -1,
            General = 0,
            Entitlement = 1,
            Vulnerability = 2,
            Comparison = 3
        }

        #region IView

        void Interfaces.IView.SetContext(Interfaces.IDataContext contextIn)
        {
            Title = "Reports";

            _linkLabel_Configure.Enabled = Program.gController.isAdmin;

            Data.Main_Reports context = (Data.Main_Reports)contextIn;

            if (context.Tab != Tab.None)
            {
                ultraTabControl1.SelectedTab = ultraTabControl1.Tabs[(int)context.Tab];
            }

            loadData();
        }

        String Interfaces.IView.HelpTopic
        {
            get { return Utility.Help.ReportHelpTopic; }
        }

        String Interfaces.IView.ConceptTopic
        {
            get { return Utility.Help.ReportHelpTopic; }
        }

        String Interfaces.IView.Title
        {
            get { return Title; }
        }

        #endregion

        #region IRefresh Members

        void Interfaces.IRefresh.RefreshView()
        {
            loadData();
        }

        #endregion

        #region Fields

        private SQL.ReportsRecord m_ReportsRecord;

        #endregion

        #region Ctors

        public View_Main_Reports()
        {
            InitializeComponent();

            // Initialize base class fields.
            _label_Summary.Text = Utility.Constants.ViewSummary_Main_Reports;
            m_menuConfiguration = new Utility.MenuConfiguration();

            // General Reports
            commonTask_GeneralReport_1.TaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_AuditedSQLServers_32;
            commonTask_GeneralReport_1.HoverTaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_AuditedSQLServers_48;
            commonTask_GeneralReport_1.TaskText = Utility.Constants.ReportTitle_AuditedServers;
            commonTask_GeneralReport_1.TaskDescription = Utility.Constants.ReportSummary_AuditedServers;
            commonTask_GeneralReport_1.TaskHandler += new System.EventHandler(showAuditedServers);

            commonTask_GeneralReport_2.TaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_CrossServerLoginCheck_32;
            commonTask_GeneralReport_2.HoverTaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_CrossServerLoginCheck_48;
            commonTask_GeneralReport_2.TaskText = Utility.Constants.ReportTitle_CrossServerLoginCheck;
            commonTask_GeneralReport_2.TaskDescription = Utility.Constants.ReportSummary_CrossServerLoginCheck;
            commonTask_GeneralReport_2.TaskHandler += new System.EventHandler(showServerAccess);

            commonTask_GeneralReport_3.TaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_DataCollectionFilters_32;
            commonTask_GeneralReport_3.HoverTaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_DataCollectionFilters_48;
            commonTask_GeneralReport_3.TaskText = Utility.Constants.ReportTitle_Filters;
            commonTask_GeneralReport_3.TaskDescription = Utility.Constants.ReportSummary_Filters;
            commonTask_GeneralReport_3.TaskHandler += new System.EventHandler(showFilters);

            commonTask_GeneralReport_4.TaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_ActivityHistory_32;
            commonTask_GeneralReport_4.HoverTaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_ActivityHistory_48;
            commonTask_GeneralReport_4.TaskText = Utility.Constants.ReportTitle_ActivityHistory;
            commonTask_GeneralReport_4.TaskDescription = Utility.Constants.ReportSummary_ActivityHistory;
            commonTask_GeneralReport_4.TaskHandler += new System.EventHandler(showActivityHistory);

            commonTask_GeneralReport_5.TaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_SQLsecureUsers_32;
            commonTask_GeneralReport_5.HoverTaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_SQLsecureUsers_48;
            commonTask_GeneralReport_5.TaskText = Utility.Constants.ReportTitle_Users;
            commonTask_GeneralReport_5.TaskDescription = Utility.Constants.ReportSummary_Users;
            commonTask_GeneralReport_5.TaskHandler += new System.EventHandler(showUsers);

            commonTask_GeneralReport_6.TaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_RiskAssessment_32;
            commonTask_GeneralReport_6.HoverTaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_RiskAssessment_48;
            commonTask_GeneralReport_6.TaskText = Utility.Constants.ReportTitle_RiskAssessment;
            commonTask_GeneralReport_6.TaskDescription = Utility.Constants.ReportSummary_RiskAssessment;
            commonTask_GeneralReport_6.TaskHandler += new System.EventHandler(showRiskAssessment);

            // Entitlement Reports
            commonTask_EntitlementReport_1.TaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_SuspectWindowsAccounts_32;
            commonTask_EntitlementReport_1.HoverTaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_SuspectWindowsAccounts_48;
            commonTask_EntitlementReport_1.TaskText = Utility.Constants.ReportTitle_SuspectWindowsAccounts;
            commonTask_EntitlementReport_1.TaskDescription = Utility.Constants.ReportSummary_SuspectWindowsAccounts;
            commonTask_EntitlementReport_1.TaskHandler += new System.EventHandler(showOrphanedLogins);

            commonTask_EntitlementReport_7.TaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_SuspectSqlLogins_32;
            commonTask_EntitlementReport_7.HoverTaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_SuspectSqlLogins_48;
            commonTask_EntitlementReport_7.TaskText = Utility.Constants.ReportTitle_SuspectSqlLogins;
            commonTask_EntitlementReport_7.TaskDescription = Utility.Constants.ReportSummary_SuspectSqlLogins;
            commonTask_EntitlementReport_7.TaskHandler += new System.EventHandler(showSqlLogins);


            commonTask_EntitlementReport_2.TaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_ServerLoginsAndUserMappings_32;
            commonTask_EntitlementReport_2.HoverTaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_ServerLoginsAndUserMappings_48;
            commonTask_EntitlementReport_2.TaskText = Utility.Constants.ReportTitle_ServerLoginsAndUserMappings;
            commonTask_EntitlementReport_2.TaskDescription = Utility.Constants.ReportSummary_ServerLoginsAndUserMappings;
            commonTask_EntitlementReport_2.TaskHandler += new System.EventHandler(showServerDatabaseUsers);

            commonTask_EntitlementReport_3.TaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_UserPermissions_32;
            commonTask_EntitlementReport_3.HoverTaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_UserPermissions_48;
            commonTask_EntitlementReport_3.TaskText = Utility.Constants.ReportTitle_UsersPermissions;
            commonTask_EntitlementReport_3.TaskDescription = Utility.Constants.ReportSummary_UsersPermissions;
            commonTask_EntitlementReport_3.TaskHandler += new System.EventHandler(showUserPermissions);

            commonTask_EntitlementReport_4.TaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_AllUserPermissions_32;
            commonTask_EntitlementReport_4.HoverTaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_AllUserPermissions_48;
            commonTask_EntitlementReport_4.TaskText = Utility.Constants.ReportTitle_AllObjectsWithPermissions;
            commonTask_EntitlementReport_4.TaskDescription = Utility.Constants.ReportSummary_AllObjectsWithPermissions;
            commonTask_EntitlementReport_4.TaskHandler += new System.EventHandler(showAllObjectsWithPermissions);

            commonTask_EntitlementReport_5.TaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_ServerRoles_32;
            commonTask_EntitlementReport_5.HoverTaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_ServerRoles_48;
            commonTask_EntitlementReport_5.TaskText = Utility.Constants.ReportTitle_ServerRoles;
            commonTask_EntitlementReport_5.TaskDescription = Utility.Constants.ReportSummary_ServerRoles;
            commonTask_EntitlementReport_5.TaskHandler += new System.EventHandler(showServerRoles);

            commonTask_EntitlementReport_6.TaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_DatabaseRoles_32;
            commonTask_EntitlementReport_6.HoverTaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_DatabaseRoles_48;
            commonTask_EntitlementReport_6.TaskText = Utility.Constants.ReportTitle_DatabaseRoles;
            commonTask_EntitlementReport_6.TaskDescription = Utility.Constants.ReportSummary_DatabaseRoles;
            commonTask_EntitlementReport_6.TaskHandler += new System.EventHandler(showDatabaseRoles);

            // Vulnerability Reports
            commonTask_VulnerabilityReport_1.TaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_MixedModeAuthentication_32;
            commonTask_VulnerabilityReport_1.HoverTaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_MixedModeAuthentication_48;
            commonTask_VulnerabilityReport_1.TaskText = Utility.Constants.ReportTitle_MixedModeAuthentication;
            commonTask_VulnerabilityReport_1.TaskDescription = Utility.Constants.ReportSummary_MixedModeAuthentication;
            commonTask_VulnerabilityReport_1.TaskHandler += new System.EventHandler(showAuthentication);

            commonTask_VulnerabilityReport_2.TaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_GuestEnabledDatabases_32;
            commonTask_VulnerabilityReport_2.HoverTaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_GuestEnabledDatabases_48;
            commonTask_VulnerabilityReport_2.TaskText = Utility.Constants.ReportTitle_GuestEnabledDatabases;
            commonTask_VulnerabilityReport_2.TaskDescription = Utility.Constants.ReportSummary_GuestEnabledDatabases;
            commonTask_VulnerabilityReport_2.TaskHandler += new System.EventHandler(showGuestEnabledDatabases);

            commonTask_VulnerabilityReport_3.TaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_OSVunerabilityviaXSPs_32;
            commonTask_VulnerabilityReport_3.HoverTaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_OSVunerabilityviaXSPs_48;
            commonTask_VulnerabilityReport_3.TaskText = Utility.Constants.ReportTitle_OSVulnerability;
            commonTask_VulnerabilityReport_3.TaskDescription = Utility.Constants.ReportSummary_OSVulnerability;
            commonTask_VulnerabilityReport_3.TaskHandler += new System.EventHandler(showSPsWithGuestAccess);

            commonTask_VulnerabilityReport_4.TaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_VunerableFixedRoles_32;
            commonTask_VulnerabilityReport_4.HoverTaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_VunerableFixedRoles_48;
            commonTask_VulnerabilityReport_4.TaskText = Utility.Constants.ReportTitle_VulnerableFixedRoles;
            commonTask_VulnerabilityReport_4.TaskDescription = Utility.Constants.ReportSummary_VulnerableFixedRoles;
            commonTask_VulnerabilityReport_4.TaskHandler += new System.EventHandler(showFixedRolesAssignedToPublic);

            commonTask_VulnerabilityReport_5.TaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_SystemAdministratorVunerability_32;
            commonTask_VulnerabilityReport_5.HoverTaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_SystemAdministratorVunerability_48;
            commonTask_VulnerabilityReport_5.TaskText = Utility.Constants.ReportTitle_SystemAdministratorVulnerability;
            commonTask_VulnerabilityReport_5.TaskDescription = Utility.Constants.ReportSummary_SystemAdministratorVulnerability;
            commonTask_VulnerabilityReport_5.TaskHandler += new System.EventHandler(showBuiltAdmin);

            commonTask_VulnerabilityReport_6.TaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_DangerousWindowsGroups_32a;
            commonTask_VulnerabilityReport_6.HoverTaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_DangerousWindowsGroups_48a;
            commonTask_VulnerabilityReport_6.TaskText = Utility.Constants.ReportTitle_ServersWithDangerousGroups;
            commonTask_VulnerabilityReport_6.TaskDescription = Utility.Constants.ReportSummary_ServersWithDangerousGroups;
            commonTask_VulnerabilityReport_6.TaskHandler += new System.EventHandler(showEveryoneGroupAccess);

            commonTask_VulnerabilityReport_7.TaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_DatabaseChainingEnabled_32;
            commonTask_VulnerabilityReport_7.HoverTaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_DatabaseChainingEnabled_48;
            commonTask_VulnerabilityReport_7.TaskText = Utility.Constants.ReportTitle_DatabaseChaining;
            commonTask_VulnerabilityReport_7.TaskDescription = Utility.Constants.ReportSummary_DatabaseChaining;
            commonTask_VulnerabilityReport_7.TaskHandler += new System.EventHandler(showDatabaseChaining);

            commonTask_VulnerabilityReport_8.TaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_MailVunerability_32;
            commonTask_VulnerabilityReport_8.HoverTaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_MailVunerability_48;
            commonTask_VulnerabilityReport_8.TaskText = Utility.Constants.ReportTitle_MailVulnerability;
            commonTask_VulnerabilityReport_8.TaskDescription = Utility.Constants.ReportSummary_MailVulnerability;
            commonTask_VulnerabilityReport_8.TaskHandler += new System.EventHandler(showMailVulnerability);

            commonTask_VulnerabilityReport_9.TaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_VulnerableLogins_32;
            commonTask_VulnerabilityReport_9.HoverTaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_VulnerableLogins_48;
            commonTask_VulnerabilityReport_9.TaskText = Utility.Constants.ReportTitle_LoginVulnerability;
            commonTask_VulnerabilityReport_9.TaskDescription = Utility.Constants.ReportSummary_LoginVulnerability;
            commonTask_VulnerabilityReport_9.TaskHandler += new System.EventHandler(showLoginVulnerability);

            // Comparison Reports
            commonTask_ComparisonReport_1.TaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_AssessmentCompare_32;
            commonTask_ComparisonReport_1.HoverTaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_AssessmentCompare_48;
            commonTask_ComparisonReport_1.TaskText = Utility.Constants.ReportTitle_CompareAssessments;
            commonTask_ComparisonReport_1.TaskDescription = Utility.Constants.ReportSummary_CompareAssessments;
            commonTask_ComparisonReport_1.TaskHandler += new System.EventHandler(showCompareAssessmentsReport);

            commonTask_ComparisonReport_2.TaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_SnapshotCompare_32;
            commonTask_ComparisonReport_2.HoverTaskImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_SnapshotCompare_48;
            commonTask_ComparisonReport_2.TaskText = Utility.Constants.ReportTitle_CompareSnapshots;
            commonTask_ComparisonReport_2.TaskDescription = Utility.Constants.ReportSummary_CompareSnapshots;
            commonTask_ComparisonReport_2.TaskHandler += new System.EventHandler(showCompareSnapshotsReport);





            //This code fails in design mode and renders the form unviewable at times
            try
            {
                // Set the security for the tasks
                commonTask_GeneralReport_1.Enabled =
                commonTask_GeneralReport_2.Enabled =
                commonTask_GeneralReport_3.Enabled =
                commonTask_GeneralReport_4.Enabled =
                commonTask_GeneralReport_5.Enabled =

                commonTask_EntitlementReport_1.Enabled =
                commonTask_EntitlementReport_2.Enabled =
                commonTask_EntitlementReport_3.Enabled =
                commonTask_EntitlementReport_4.Enabled =
                commonTask_EntitlementReport_5.Enabled =
                commonTask_EntitlementReport_6.Enabled =

                commonTask_VulnerabilityReport_1.Enabled =
                commonTask_VulnerabilityReport_2.Enabled =
                commonTask_VulnerabilityReport_3.Enabled =
                commonTask_VulnerabilityReport_4.Enabled =
                commonTask_VulnerabilityReport_5.Enabled =
                commonTask_VulnerabilityReport_6.Enabled =
                commonTask_VulnerabilityReport_7.Enabled =
                commonTask_VulnerabilityReport_8.Enabled =
                commonTask_VulnerabilityReport_9.Enabled =

                commonTask_ComparisonReport_1.Enabled =
                commonTask_ComparisonReport_2.Enabled = Program.gController.isViewer;
            }
            catch { }

            //Setup reporting services section
            _label_ReportingServicesSummary.Text = Utility.Constants.ReportSummary_ReportingServices;

            ultraTabControl1.DrawFilter = new Utility.HideFocusRectangleDrawFilter();
        }

        #endregion

        #region Constants

        private const string NotConfigured = @"Not Configured";
        private const string AllReportsTask = @"Show All Reports...";
        private const string AllReportsDescription = @"Click here to display all available reports";

        #endregion

        #region Helpers

        public void loadData()
        {
            bool installed;

            m_ReportsRecord = new SQL.ReportsRecord();
            m_ReportsRecord.Read();

            if (m_ReportsRecord.ReportsDeployed)
            {
                installed = true;
                _label_Computer.Text = m_ReportsRecord.ReportServer;
                _label_Folder.Text = m_ReportsRecord.TargetDirectory;
            }
            else
            {
                installed = false;
                _label_Computer.Text = NotConfigured;
                _label_Folder.Text = string.Empty;
            }

            _linkLabel_Launch.Enabled = installed && Program.gController.isViewer;
        }

        private void launchReportingServices()
        {
            try
            {
                // Launch Reporting Services
                string url = m_ReportsRecord.GetReportManagerUrl(true);
                if (url != string.Empty)
                {
                    System.Diagnostics.Process.Start(url);
                }
            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.ReportsCaption, Utility.ErrorMsgs.CantRunReportingServices, ex);
            }
        }

        #endregion

        #region Built-in Reports

        private void showAuditedServers(object sender, EventArgs e)
        {
            Program.gController.SetCurrentReport(Utility.Constants.ReportNode_AuditedServers);
            Program.gController.ShowRootView(new Utility.NodeTag(new Data.Report(string.Empty),
                                                        Utility.View.Report_AuditedServers));
        }

        private void showFilters(object sender, EventArgs e)
        {
            Program.gController.SetCurrentReport(Utility.Constants.ReportTitle_Filters);
            Program.gController.ShowRootView(new Utility.NodeTag(new Data.Report(string.Empty),
                                                        Utility.View.Report_Filters));
        }

        private void showActivityHistory(object sender, EventArgs e)
        {
            Program.gController.SetCurrentReport(Utility.Constants.ReportTitle_ActivityHistory);
            Program.gController.ShowRootView(new Utility.NodeTag(new Data.Report(string.Empty),
                                                        Utility.View.Report_ActivityHistory));
        }

        private void showUsers(object sender, EventArgs e)
        {
            Program.gController.SetCurrentReport(Utility.Constants.ReportTitle_Users);
            Program.gController.ShowRootView(new Utility.NodeTag(new Data.Report(string.Empty),
                                                        Utility.View.Report_Users));
        }

        private void showGuestEnabledDatabases(object sender, EventArgs e)
        {
            Program.gController.SetCurrentReport(Utility.Constants.ReportNode_GuestEnabledServers);
            Program.gController.ShowRootView(new Utility.NodeTag(new Data.Report(string.Empty),
                                                        Utility.View.Report_GuestEnabledDatabases));
        }

        private void showServerAccess(object sender, EventArgs e)
        {
            Program.gController.SetCurrentReport(Utility.Constants.ReportNode_CrossServerLoginCheck);
            Program.gController.ShowRootView(new Utility.NodeTag(new Data.Report(string.Empty),
                                                        Utility.View.Report_CrossServerLoginCheck));
        }

        private void showAuthentication(object sender, EventArgs e)
        {
            Program.gController.SetCurrentReport(Utility.Constants.ReportNode_MixedModeAuthentication);
            Program.gController.ShowRootView(new Utility.NodeTag(new Data.Report(string.Empty),
                                                        Utility.View.Report_MixedModeAuthentication));
        }

        private void showEveryoneGroupAccess(object sender, EventArgs e)
        {
            Program.gController.SetCurrentReport(Utility.Constants.ReportNode_ServersWithDangerousGroups);
            Program.gController.ShowRootView(new Utility.NodeTag(new Data.Report(string.Empty),
                                                        Utility.View.Report_ServersWithDangerousGroups));
        }

        private void showBuiltAdmin(object sender, EventArgs e)
        {
            Program.gController.SetCurrentReport(Utility.Constants.ReportNode_SystemAdministratorVulnerability);
            Program.gController.ShowRootView(new Utility.NodeTag(new Data.Report(string.Empty),
                                                        Utility.View.Report_SystemAdministratorVulnerability));
        }

        private void showOrphanedLogins(object sender, EventArgs e)
        {
            Program.gController.SetCurrentReport(Utility.Constants.ReportNode_SuspectWindowsAccounts);
            Program.gController.ShowRootView(new Utility.NodeTag(new Data.Report(string.Empty),
                                                        Utility.View.Report_SuspectWindowsAccounts));
        }
        private void showSqlLogins(object sender, EventArgs e)
        {
            Program.gController.SetCurrentReport(Utility.Constants.ReportNode_SuspectSqlLogins);
            Program.gController.ShowRootView(new Utility.NodeTag(new Data.Report(string.Empty),
                                                        Utility.View.Report_SuspectSqlLogins));
        }

        private void showFixedRolesAssignedToPublic(object sender, EventArgs e)
        {
            Program.gController.SetCurrentReport(Utility.Constants.ReportNode_VulnerableFixedRoles);
            Program.gController.ShowRootView(new Utility.NodeTag(new Data.Report(string.Empty),
                                                        Utility.View.Report_VulnerableFixedRoles));
        }

        private void showCmdShell(object sender, EventArgs e)
        {
            Program.gController.SetCurrentReport(Utility.Constants.ReportNode_CMDShellVulnerability);
            Program.gController.ShowRootView(new Utility.NodeTag(new Data.Report(string.Empty),
                                                        Utility.View.Report_CMDShellVulnerability));
        }

        private void showMailVulnerability(object sender, EventArgs e)
        {
            Program.gController.SetCurrentReport(Utility.Constants.ReportNode_MailVulnerability);
            Program.gController.ShowRootView(new Utility.NodeTag(new Data.Report(string.Empty),
                                                        Utility.View.Report_MailVulnerability));
        }

        private void showLoginVulnerability(object sender, EventArgs e)
        {
            Program.gController.SetCurrentReport(Utility.Constants.ReportNode_LoginVulnerability);
            Program.gController.ShowRootView(new Utility.NodeTag(new Data.Report(string.Empty),
                                                        Utility.View.Report_LoginVulnerability));
        }

        private void showServerDatabaseUsers(object sender, EventArgs e)
        {
            Program.gController.SetCurrentReport(Utility.Constants.ReportNode_ServerLoginsAndUserMappings);
            Program.gController.ShowRootView(new Utility.NodeTag(new Data.Report(string.Empty),
                                                        Utility.View.Report_ServerLoginsAndUserMappings));
        }

        private void showUserPermissions(object sender, EventArgs e)
        {
            Program.gController.SetCurrentReport(Utility.Constants.ReportNode_UserPermissions);
            Program.gController.ShowRootView(new Utility.NodeTag(new Data.Report(string.Empty),
                                                        Utility.View.Report_UsersPermissions));
        }

        private void showAllObjectsWithPermissions(object sender, EventArgs e)
        {
            Program.gController.SetCurrentReport(Utility.Constants.ReportNode_AllObjectsWithPermissions);
            Program.gController.ShowRootView(new Utility.NodeTag(new Data.Report(string.Empty),
                                                        Utility.View.Report_AllObjectsWithPermissions));
        }

        private void showServerRoles(object sender, EventArgs e)
        {
            Program.gController.SetCurrentReport(Utility.Constants.ReportNode_ServerRoles);
            Program.gController.ShowRootView(new Utility.NodeTag(new Data.Report(string.Empty),
                                                        Utility.View.Report_ServerRoles));
        }

        private void showDatabaseRoles(object sender, EventArgs e)
        {
            Program.gController.SetCurrentReport(Utility.Constants.ReportNode_DatabaseRoles);
            Program.gController.ShowRootView(new Utility.NodeTag(new Data.Report(string.Empty),
                                                        Utility.View.Report_DatabaseRoles));
        }

        private void showDatabaseChaining(object sender, EventArgs e)
        {
            Program.gController.SetCurrentReport(Utility.Constants.ReportNode_DatabaseChaining);
            Program.gController.ShowRootView(new Utility.NodeTag(new Data.Report(string.Empty),
                                                        Utility.View.Report_DatabaseChaining));
        }

        private void showSPsWithGuestAccess(object sender, EventArgs e)
        {
            Program.gController.SetCurrentReport(Utility.Constants.ReportNode_OSVulnerability);
            Program.gController.ShowRootView(new Utility.NodeTag(new Data.Report(string.Empty),
                                                        Utility.View.Report_OSVulnerability));
        }

        private void showRiskAssessment(object sender, EventArgs e)
        {
            Program.gController.SetCurrentReport(Utility.Constants.ReportNode_RiskAssessment);
            Program.gController.ShowRootView(new Utility.NodeTag(new Data.Report(string.Empty),
                                                        Utility.View.Report_RiskAssessment));
        }


        private void showCompareAssessmentsReport(object sender, EventArgs e)
        {
            Program.gController.SetCurrentReport(Utility.Constants.reportNode_CompareAssessments);
            Program.gController.ShowRootView(new Utility.NodeTag(new Data.Report(string.Empty),
                                                        Utility.View.Report_CompareAssessments));
        }

        private void showCompareSnapshotsReport(object sender, EventArgs e)
        {
            Program.gController.SetCurrentReport(Utility.Constants.reportNode_CompareSnapshots);
            Program.gController.ShowRootView(new Utility.NodeTag(new Data.Report(string.Empty),
                                                        Utility.View.Report_CompareSnapshots));
        }


        #endregion

        #region Events

        private void ultraTabControl1_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            Program.gController.SetCurrentReport(e.Tab.Text);
        }

        #region Microsoft Reporting Services

        private void _linkLabel_Configure_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            using (Form_WizardDeployReports frm = new Form_WizardDeployReports())
            {
                frm.ShowDialog(this);
            }

            loadData();

            Cursor = Cursors.Default;
        }

        private void _linkLabel_Launch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            loadData();
            launchReportingServices();

            Cursor = Cursors.Default;
        }

        #endregion

        #endregion
    }
}