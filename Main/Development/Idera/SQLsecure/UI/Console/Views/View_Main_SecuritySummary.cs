using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Idera.SQLsecure.Core.Accounts;
using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Controls;
using Idera.SQLsecure.UI.Console.Utility;

using Infragistics.Win.UltraWinToolbars;

namespace Idera.SQLsecure.UI.Console.Views
{
    public partial class View_Main_SecuritySummary : UserControl, Interfaces.IView, Interfaces.ICommandHandler, Interfaces.IRefresh
    {
        #region types and enums

        public enum SecurityView
        {
            None = -1,
            Summary = 0,
            Settings = 1,
            Users = 2
        }

        private class RibbonTabView
        {
            public const string Summary = @"Summary";
            public const string Settings = @"Settings";
            public const string Users = @"Users";
        }

        private class RibbonTaskButton
        {
            public const string AddPolicy = @"_buttonTool_AddPolicy";
            public const string EditPolicy = @"_buttonTool_EditPolicy";
            public const string CopyAssessment = @"_buttonTool_CopyPolicy";
            public const string Compare = @"_buttonTool_Compare";
            public const string AddServer = @"_buttonTool_AddServer";
            public const string Configure = @"_buttonTool_Configure";
            public const string TakeSnapshot = @"_buttonTool_CollectData";
            public const string ExploreUsers = @"_buttonTool_ExploreUsers";
            public const string ExploreRoles = @"_buttonTool_ExploreRoles";
            public const string ExploreObjects = @"_buttonTool_ExploreObjects";
            public const string ViewReports = @"_buttonTool_ViewReports";
            public const string ImportServers = @"_buttonTool_ImportServers";
        }

        private class RibbonAuditData
        {
            public const string Group = @"_ribbonGroup_Summary_Selection";
            public const string Select = @"_buttonTool_SelectAuditData";
            public const string SettingsLabelBaseline = @"_labelTool_AuditDataBaseline";
            public const string SettingsLabelCurrent = @"_labelTool_AuditDataCurrent";
            public const string SettingsLabelDate = @"_labelTool_AuditDataDate";
        }

        #endregion

        #region IView

        void Interfaces.IView.SetContext(Interfaces.IDataContext contextIn)
        {
            // Set the title.
            m_Title = contextIn.Name;

            m_context = (Data.Main_SecuritySummary)contextIn;
            m_policy = m_context.Policy;
            m_server = m_context.Server;

            _ultraToolbarsManager.Ribbon.SelectedTab = _ultraToolbarsManager.Ribbon.Tabs[(int)(m_context.SecurityView == SecurityView.None ? m_selectedView : m_context.SecurityView)];

            updateRibbonAuditData();

            showRefresh();
        }
        String Interfaces.IView.HelpTopic
        {
            get
            {
                string helpTopic = Utility.Help.SecuritySummaryPolicyReportCardHelpTopic;
                if (m_server == null)
                {
                    if (_ultraToolbarsManager.Ribbon.SelectedTab.Index == (int) SecurityView.Summary)
                        helpTopic = Utility.Help.SecuritySummaryPolicyReportCardHelpTopic;
                    else if (_ultraToolbarsManager.Ribbon.SelectedTab.Index == (int) SecurityView.Settings)
                        helpTopic = Utility.Help.SecuritySummaryPolicySettingsHelpTopic;
                    else if (_ultraToolbarsManager.Ribbon.SelectedTab.Index == (int)SecurityView.Users)
                        helpTopic = Utility.Help.SecuritySummaryPolicyUsersHelpTopic;
                }
                else
                    if (_ultraToolbarsManager.Ribbon.SelectedTab.Index == (int)SecurityView.Summary)
                        helpTopic = Utility.Help.SecuritySummaryServerReportCardHelpTopic;
                    else if (_ultraToolbarsManager.Ribbon.SelectedTab.Index == (int)SecurityView.Settings)
                        helpTopic = Utility.Help.SecuritySummaryServerSettingsHelpTopic;
                    else if (_ultraToolbarsManager.Ribbon.SelectedTab.Index == (int)SecurityView.Users)
                        helpTopic = Utility.Help.SecuritySummaryServerUsersHelpTopic;

                return helpTopic;
            }
        }
        String Interfaces.IView.ConceptTopic
        {
            get
            {
                string helpTopic = Utility.Help.SecuritySummaryPolicyReportCardHelpTopic;
                if (m_server == null)
                {
                    if (_ultraToolbarsManager.Ribbon.SelectedTab.Index == (int)SecurityView.Summary)
                        helpTopic = Utility.Help.SecuritySummaryPolicyReportCardHelpTopic;
                    else if (_ultraToolbarsManager.Ribbon.SelectedTab.Index == (int)SecurityView.Settings)
                        helpTopic = Utility.Help.SecuritySummaryPolicySettingsHelpTopic;
                    else if (_ultraToolbarsManager.Ribbon.SelectedTab.Index == (int)SecurityView.Users)
                        helpTopic = Utility.Help.SecuritySummaryPolicyUsersHelpTopic;
                }
                else
                    if (_ultraToolbarsManager.Ribbon.SelectedTab.Index == (int)SecurityView.Summary)
                        helpTopic = Utility.Help.SecuritySummaryServerReportCardHelpTopic;
                    else if (_ultraToolbarsManager.Ribbon.SelectedTab.Index == (int)SecurityView.Settings)
                        helpTopic = Utility.Help.SecuritySummaryServerSettingsHelpTopic;
                    else if (_ultraToolbarsManager.Ribbon.SelectedTab.Index == (int)SecurityView.Users)
                        helpTopic = Utility.Help.SecuritySummaryServerUsersHelpTopic;

                return helpTopic;
            }
        }
        String Interfaces.IView.Title
        {
            get { return m_Title; }
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
                //case Utility.ViewSpecificCommand.NewLogin:
                //    showAddLogin();
                //    break;
                case Utility.ViewSpecificCommand.Delete:
                    showDelete();
                    break;
                case Utility.ViewSpecificCommand.Configure:
                    showConfigure();
                    break;
                case Utility.ViewSpecificCommand.UserPermissions:
                    showPermissions(Views.View_PermissionExplorer.Tab.UserPermissions);
                    break;
                case Utility.ViewSpecificCommand.ObjectPermissions:
                    showPermissions(Views.View_PermissionExplorer.Tab.ObjectPermissions);
                    break;
                case Utility.ViewSpecificCommand.Collect:
                    showCollect();
                    break;
                case Utility.ViewSpecificCommand.Refresh:
                    RefreshView();
                    break;
                case Utility.ViewSpecificCommand.Properties:
                    showProperties();
                    break;
                default:
                    Debug.Assert(false, "Unknown command passed to ProcessCommand");
                    break;
            }
        }

        #region Menu Functions (same as BaseView overrides)

        protected void showDelete()
        {
            if (m_server == null)
            {
                if (Sql.Policy.IsPolicyRegistered(m_policy.PolicyId))
                {
                    if (!m_policy.IsSystemPolicy)
                    {
                        // Display confirmation, if user confirms remove the policy.
                        string caption = Utility.ErrorMsgs.RemovePolicyCaption + " - " + m_policy.PolicyName;
                        if (DialogResult.Yes == Utility.MsgBox.ShowWarningConfirm(caption, Utility.ErrorMsgs.RemovePolicyConfirmMsg))
                        {
                            try
                            {
                                Sql.Policy.RemovePolicy(m_policy.PolicyId);
                            }
                            catch (Exception ex)
                            {
                                Utility.MsgBox.ShowError(Utility.ErrorMsgs.RemovePolicyCaption, string.Format(Utility.ErrorMsgs.CantRemovePolicyMsg, m_policy.PolicyName), ex);
                            }

                            Program.gController.SignalRefreshPoliciesEvent(0);
                        }
                    }
                    else
                    {
                        Utility.MsgBox.ShowError(Utility.ErrorMsgs.RemovePolicyCaption, Utility.ErrorMsgs.RemoveSystemPolicyMsg);
                    }
                }
                else
                {
                    Utility.MsgBox.ShowError(Utility.ErrorMsgs.RemovePolicyCaption, Utility.ErrorMsgs.PolicyNotRegistered);
                    Program.gController.SignalRefreshPoliciesEvent(0);
                }
            }
            else
            {
                if (!m_policy.IsDynamic)
                {
                    if (DialogResult.Yes == Utility.MsgBox.ShowWarningConfirm(Utility.ErrorMsgs.RemoveServerFromPolicyCaption,
                                string.Format(Utility.ErrorMsgs.RemoveServerFromPolicyConfirmMsg, m_server.ConnectionName, m_policy.PolicyName)))
                    {
                        Sql.RegisteredServer.RemoveRegisteredServerFromPolicy(m_server.RegisteredServerId, m_policy.PolicyId, m_policy.AssessmentId);

                        Program.gController.SignalRefreshPoliciesEvent(0);
                    }
                }
                else
                {
                    Utility.MsgBox.ShowError(Utility.ErrorMsgs.RemoveServerFromPolicyCaption, Utility.ErrorMsgs.RemoveDynamicPolicyServerMsg);
                }
            }
        }

        private void showPermissions(Views.View_PermissionExplorer.Tab tab)
        {
            Sql.RegisteredServer server = m_server;

            if (_ultraToolbarsManager.Ribbon.SelectedTab.Key == RibbonTabView.Users
                && tab == Views.View_PermissionExplorer.Tab.UserPermissions)
            {
                if (_policyUsers.SelectedServer != null)
                {
                    server = _policyUsers.SelectedServer;
                    if (_policyUsers.SelectedUser != null)
                    {
                        int snapshotid;
                        m_policy.PolicySnapshotList.TryGetValue(server.RegisteredServerId, out snapshotid);

                        if (snapshotid > 0)
                        {
                            Program.gController.ShowRootView(new NodeTag(new Data.PermissionExplorer(server, snapshotid, _policyUsers.SelectedUser, tab),
                                                                            Utility.View.PermissionExplorer));
                        }

                        return;
                    }
                }
            }
            else if (_ultraToolbarsManager.Ribbon.SelectedTab.Key == RibbonTabView.Settings)
            {
                if (_sqlServerSettings.SelectedServer != null)
                {
                    server = _sqlServerSettings.SelectedServer;

                    int snapshotid;
                        m_policy.PolicySnapshotList.TryGetValue(server.RegisteredServerId, out snapshotid);

                        if (snapshotid > 0)
                        {
                            Program.gController.ShowRootView(new NodeTag(new Data.PermissionExplorer(server, snapshotid, _policyUsers.SelectedUser, tab),
                                                                            Utility.View.PermissionExplorer));
                        }
                }
            }

            if (server == null)
            {
                if (tab == Views.View_PermissionExplorer.Tab.UserPermissions)
                {
                    Forms.Form_WizardUserPermissions.Process();
                }
                else if (tab == Views.View_PermissionExplorer.Tab.ObjectPermissions)
                {
                    Forms.Form_WizardObjectPermissions.Process();
                }
                else
                {
                    server = Forms.Form_SelectRegisteredServer.GetServer();
                }
            }
            if (server != null)
            {
                        int snapshotid;
                        m_policy.PolicySnapshotList.TryGetValue(server.RegisteredServerId, out snapshotid);

                        if (snapshotid > 0)
                        {
                            Program.gController.ShowRootView(new NodeTag(new Data.PermissionExplorer(server, snapshotid, tab),
                                                                         Utility.View.PermissionExplorer));
                        }
                        else
                        {
                            Program.gController.ShowRootView(new NodeTag(new Data.PermissionExplorer(server, tab),
                                                                         Utility.View.PermissionExplorer));
                        }
            }
        }

        protected void showProperties()
        {
            if (m_server == null)
            {
                if (Forms.Form_PolicyProperties.Process(m_policy.PolicyId, m_policy.AssessmentId, Program.gController.isAdmin))
                {
                    Program.gController.SignalRefreshPoliciesEvent(m_policy.PolicyId);
                }
            }
            else
            {
                Forms.Form_SqlServerProperties.Process(m_server.ConnectionName, Forms.Form_SqlServerProperties.RequestedOperation.GeneralProperties,
                                                       Program.gController.isAdmin);
            }
        }

        protected void showRefresh()
        {
            _ultraToolbarsManager.Tools[RibbonTaskButton.EditPolicy].SharedProps.Caption = string.Format("{0} Settings", Program.gController.isAdmin ? @"Edit" : @"View");

            _ultraToolbarsManager.Tools[RibbonTaskButton.AddPolicy].SharedProps.Enabled = Program.gController.isAdmin;
            _ultraToolbarsManager.Tools[RibbonTaskButton.EditPolicy].SharedProps.Enabled = true;
            _ultraToolbarsManager.Tools[RibbonTaskButton.CopyAssessment].SharedProps.Enabled = Program.gController.isAdmin && (Program.gController.Repository.RegisteredServers.Count > 0);
            _ultraToolbarsManager.Tools[RibbonTaskButton.Compare].SharedProps.Enabled = m_policy.HasAssessments();
            _ultraToolbarsManager.Tools[RibbonTaskButton.AddServer].SharedProps.Enabled = Program.gController.Permissions.hasSecurity(Utility.Security.Functions.AuditSQLServer);
            _ultraToolbarsManager.Tools[RibbonTaskButton.Configure].SharedProps.Enabled = Program.gController.Permissions.hasSecurity(Utility.Security.Functions.ConfigureAuditSettings);
            _ultraToolbarsManager.Tools[RibbonTaskButton.TakeSnapshot].SharedProps.Enabled = Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Collect);
            _ultraToolbarsManager.Tools[RibbonTaskButton.ExploreUsers].SharedProps.Enabled = Program.gController.Permissions.hasSecurity(Utility.Security.Functions.UserPermissions);
            _ultraToolbarsManager.Tools[RibbonTaskButton.ExploreRoles].SharedProps.Enabled = Program.gController.Permissions.hasSecurity(Utility.Security.Functions.UserPermissions);
            _ultraToolbarsManager.Tools[RibbonTaskButton.ExploreObjects].SharedProps.Enabled = Program.gController.Permissions.hasSecurity(Utility.Security.Functions.ObjectPermissions);
            _ultraToolbarsManager.Tools[RibbonTaskButton.ImportServers].SharedProps.Enabled = Program.gController.Permissions.hasSecurity(Utility.Security.Functions.AuditSQLServer);

            loadDataSource();
        }

        protected void showServer()
        {
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - Security Settings Summary showServer command called erroneously");
        }

        protected void showConfigure()
        {
            Sql.RegisteredServer server = m_server;

            if (_ultraToolbarsManager.Ribbon.SelectedTab.Key == RibbonTabView.Users)
            {
                if (_policyUsers.SelectedServer != null)
                {
                    server = _policyUsers.SelectedServer;
                }
            }
            else if (_ultraToolbarsManager.Ribbon.SelectedTab.Key == RibbonTabView.Settings)
            {
                if (_sqlServerSettings.SelectedServer != null)
                {
                    server = _sqlServerSettings.SelectedServer;
                }
            }

            if (server == null)
            {
                server = Forms.Form_SelectRegisteredServer.GetServer();
            }
            if (server != null)
            {
                Forms.Form_SqlServerProperties.Process(server.ConnectionName, Forms.Form_SqlServerProperties.RequestedOperation.EditCofiguration, Program.gController.isAdmin);
            }
        }

        protected void showCollect()
        {
            Sql.RegisteredServer server = m_server;

            if (_ultraToolbarsManager.Ribbon.SelectedTab.Key == RibbonTabView.Users)
            {
                if (_policyUsers.SelectedServer != null)
                {
                    server = _policyUsers.SelectedServer;
                }
            }
            else if (_ultraToolbarsManager.Ribbon.SelectedTab.Key == RibbonTabView.Settings)
            {
                if (_sqlServerSettings.SelectedServer != null)
                {
                    server = _sqlServerSettings.SelectedServer;
                }
            }

            if (server == null)
            {
                server = Forms.Form_SelectRegisteredServer.GetServer();
            }
            if (server != null)
            {
                if (Sql.RegisteredServer.IsServerRegistered(server.ConnectionName))
                {
                    Forms.Form_StartSnapshotJobAndShowProgress.Process(server.ConnectionName);
                }
                else
                {
                    Utility.MsgBox.ShowError(Utility.ErrorMsgs.SQLsecureDataCollection, Utility.ErrorMsgs.ServerNotRegistered);
                    Program.gController.SignalRefreshServersEvent(false, string.Empty);
                }
            }
        }

        protected void showNewAuditServer()
        {
            Forms.Form_WizardRegisterSQLServer.Process();

            showRefresh();
        }

        protected void showNewPolicy()
        {
            Forms.Form_WizardCreatePolicy.Process();
        }

        protected void showEditPolicy()
        {
            if (m_policy != null)
            {
                // returns true if updated
                if (Forms.Form_PolicyProperties.Process(m_policy.PolicyId, m_policy.AssessmentId, Program.gController.isAdmin))
                {
                    Program.gController.SignalRefreshPoliciesEvent(m_policy.PolicyId);
                }
            }
        }
        protected void showCopyPolicy()
        {
            if (m_policy != null)
            {
                Forms.Form_CreateAssessment.Process(m_policy);
            }
        }

        protected void showCompare()
        {
            try
            {
                Sql.Repository.AssessmentList aList = m_policy.Assessments.FindByState(Utility.Policy.AssessmentState.Current);

                Sql.Policy p = aList[0];

                Program.gController.ShowCompareDialog(m_policy.PolicyId, p.AssessmentId, m_server);
            }
            catch(Exception ex)
            {
                logX.loggerX.Error("Error - Unable to load the assessment from the Policy from the Repository", ex);
                MsgBox.ShowError(Utility.ErrorMsgs.CantGetAssessments, ex.Message);                
            }
        }

        protected void showReports()
        {
            Program.gController.ShowReportMenu(m_policy.PolicyId, m_selectDate, m_useBaseline);
        }


        #endregion

        #endregion

        #region IRefresh Members

        public void RefreshView()
        {
            showRefresh();
        }

        #endregion

        #region Fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Views.View_Main_SecuritySummary");

        private string m_Title;
        private Data.Main_SecuritySummary m_context;
        private Utility.MenuConfiguration m_MenuConfiguration;
        private bool m_ignoreToolClick = false;

        private Sql.Policy m_policy;
        private Sql.RegisteredServer m_server;
        private bool m_useBaseline = false;
        private DateTime? m_selectDate = null;
        private bool m_showMsg = true;
        private SecurityView m_selectedView = SecurityView.Summary;

        #endregion

        #region queries & constants

        private const string QueryGetPolicyServers = @"SQLsecure.dbo.isp_sqlsecure_getpolicyserverlist";
        private const string ParamPolicyId = @"@policyid";

        private const string DisplayBaseline = @"Use baseline snapshot data only";
        private const string DisplayCurrent = @"Use most current data";
        private const string DisplayDate = "as of {0}";
        private const string DisplayNoServers = @"This policy has no servers and cannot be assessed.";
        private const string DisplayGetServersError = @"Unable to retrieve policy servers from repository.";

        #endregion

        #region Ctors

        public View_Main_SecuritySummary()
        {
            InitializeComponent();

            // Initialize menu configuration.
            m_MenuConfiguration = new Utility.MenuConfiguration();

            _ultraToolbarsManager.Tools[RibbonTaskButton.AddPolicy].SharedProps.Enabled =
                _ultraToolbarsManager.Tools[RibbonTaskButton.EditPolicy].SharedProps.Enabled =
                _ultraToolbarsManager.Tools[RibbonTaskButton.CopyAssessment].SharedProps.Enabled =
                _ultraToolbarsManager.Tools[RibbonTaskButton.Compare].SharedProps.Enabled =
                _ultraToolbarsManager.Tools[RibbonTaskButton.AddServer].SharedProps.Enabled =
                _ultraToolbarsManager.Tools[RibbonTaskButton.Configure].SharedProps.Enabled =
                _ultraToolbarsManager.Tools[RibbonTaskButton.TakeSnapshot].SharedProps.Enabled =
                _ultraToolbarsManager.Tools[RibbonTaskButton.ExploreUsers].SharedProps.Enabled =
                _ultraToolbarsManager.Tools[RibbonTaskButton.ExploreRoles].SharedProps.Enabled =
                _ultraToolbarsManager.Tools[RibbonTaskButton.ExploreObjects].SharedProps.Enabled=
                _ultraToolbarsManager.Tools[RibbonTaskButton.ImportServers].SharedProps.Enabled = false;

            // Dock the controls that are not docked in the designer for visibility
            _policySummary.Dock =
                _sqlServerSettings.Dock =
                _policyUsers.Dock =
                _label_Msg.Dock = DockStyle.Fill;

            _label_Msg.Text = DisplayNoServers;

            _label_Msg.Visible =
                _sqlServerSettings.Visible =
                _policyUsers.Visible = false;
            _policySummary.Visible = true;
        }

        #endregion

        #region properties

        public bool UseBaselineSnapshots
        {
            get { return m_useBaseline; }
        }

        public DateTime? SelectionTime
        {
            get
            {
                return m_selectDate.HasValue ? (DateTime?)m_selectDate.Value.ToUniversalTime() : null;
            }
        }

        #endregion

        #region Helpers

        // Find the control with focus
        private Control getFocused(Control.ControlCollection controls)
        {
            foreach (Control c in controls)
            {
                if (c.Focused)
                {
                    return c;
                }
                else if (c.ContainsFocus)
                {
                    return getFocused(c.Controls);
                }
            }

            return null;
        }

        private void selectAuditData()
        {
            bool useBaseline;
            DateTime? selectDate;
            if (Forms.Form_SelectAuditData.GetSelections(m_useBaseline, m_selectDate, out useBaseline, out selectDate))
            {
                m_useBaseline = useBaseline;
                m_selectDate = selectDate;
                updateRibbonAuditData();
                RefreshView();
            }
        }

        private void updateRibbonAuditData()
        {
            string settings;

            settings = UseBaselineSnapshots ? DisplayBaseline : @" ";
            ((LabelTool)_ultraToolbarsManager.Ribbon.Tabs[RibbonTabView.Summary].Groups[RibbonAuditData.Group].Tools[RibbonAuditData.SettingsLabelBaseline]).SharedProps.Caption = settings;

            DateTime DefaultTime = new DateTime(DateTime.Now.Year,
                                                DateTime.Now.Month,
                                                DateTime.Now.Day,
                                                23, 59, 59);

            ((LabelTool)_ultraToolbarsManager.Ribbon.Tabs[RibbonTabView.Summary].Groups[RibbonAuditData.Group].Tools[RibbonAuditData.SettingsLabelCurrent]).SharedProps.Caption = DisplayCurrent;

            settings = SelectionTime.HasValue
                           ? string.Format(DisplayDate,
                           m_selectDate.Value.ToString(m_selectDate.Value.TimeOfDay.Ticks == DefaultTime.TimeOfDay.Ticks
                                                                           ? Utility.Constants.DATE_FORMAT
                                                                           : Utility.Constants.DATETIME_FORMAT))
                           : @" ";
            ((LabelTool)_ultraToolbarsManager.Ribbon.Tabs[RibbonTabView.Summary].Groups[RibbonAuditData.Group].Tools[RibbonAuditData.SettingsLabelDate]).SharedProps.Caption = settings;
        }

        private void loadDataSource()
        {
            logX.loggerX.Info("Retrieve Policy Servers");
            try
            {
                _panel_View.SuspendLayout();
                _ultraToolbarsManager.Enabled = true;
                //// Open connection to repository and query permissions.
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Setup parameters for all queries
                    SqlParameter paramPolicyId = new SqlParameter(ParamPolicyId, m_policy.PolicyId);

                    // Get Servers
                    SqlCommand cmd = new SqlCommand(QueryGetPolicyServers, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(paramPolicyId);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        m_showMsg = true;
                        _label_Msg.Text = DisplayNoServers;
                    }
                    else
                    {
                        m_showMsg = false;

                        // Load the policy snapshot list before passing to the sub controls so they will have it
                        m_policy.GetPolicySnapshotIds(SelectionTime, UseBaselineSnapshots);
                    }
                    setViewContext(m_selectedView);
                }
                _panel_View.ResumeLayout();
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error(@"Error - Unable to retrieve policy info", ex);
                _ultraToolbarsManager.Enabled = false;
                MsgBox.ShowError(string.Format(ErrorMsgs.CantGetPolicyInfoMsg, "Servers"),
                                 ErrorMsgs.ErrorProcessPolicyInfo,
                                 ex);
                _label_Msg.Text = DisplayGetServersError;
                m_showMsg = true;
                setViewContext(m_selectedView);
                _panel_View.ResumeLayout();
            }
        }

        protected void setMenuConfiguration()
        {
            m_MenuConfiguration.EditItems[(int)Utility.MenuItems_Edit.Remove] = (m_server != null) ? !m_policy.IsDynamic : m_policy.IsSystemPolicy;
            m_MenuConfiguration.EditItems[(int)Utility.MenuItems_Edit.Properties] = true;
            m_MenuConfiguration.EditItems[(int)Utility.MenuItems_Edit.ConfigureDataCollection] = (m_server != null);
            m_MenuConfiguration.SnapshotItems[(int)Utility.MenuItems_Snapshots.Collect] = true;

            Program.gController.SetMenuConfiguration(m_MenuConfiguration, this);
        }

        protected void setViewContext(SecurityView view)
        {
            _panel_View.SuspendLayout();
            m_ignoreToolClick = true;

            if (m_selectedView != view)
            {
                _label_Msg.Visible =
                    _policySummary.Visible =
                    _sqlServerSettings.Visible =
                    _policyUsers.Visible = false;
                m_selectedView = view;
            }

            if (m_showMsg)
            {
                _label_Msg.Visible = true;
            }
            else
            {
                _label_Msg.Visible = false;
                switch (view)
                {
                    case SecurityView.Summary:
                        if (m_server == null)
                        {
                            ((Interfaces.IView)_policySummary).SetContext(new Data.PolicySummary(m_policy, UseBaselineSnapshots, SelectionTime));
                        }
                        else
                        {
                            ((Interfaces.IView)_policySummary).SetContext(new Data.PolicySummary(m_policy, UseBaselineSnapshots, SelectionTime, m_server));
                        }
                        _policySummary.Visible = true;
                        break;
                    case SecurityView.Settings:
                        if (m_server == null)
                        {
                            ((Interfaces.IView)_sqlServerSettings).SetContext(new Data.SqlServerSettings(m_policy, UseBaselineSnapshots, SelectionTime));
                        }
                        else
                        {
                            ((Interfaces.IView)_sqlServerSettings).SetContext(new Data.SqlServerSettings(m_policy, UseBaselineSnapshots, SelectionTime, m_server));
                        }
                        _sqlServerSettings.Visible = true;
                        break;
                    case SecurityView.Users:
                        if (m_server == null)
                        {
                            ((Interfaces.IView)_policyUsers).SetContext(new Data.PolicyUsers(m_policy, UseBaselineSnapshots, SelectionTime));
                        }
                        else
                        {
                            ((Interfaces.IView)_policyUsers).SetContext(new Data.PolicyUsers(m_policy, UseBaselineSnapshots, SelectionTime, m_server));
                        }
                        _policyUsers.Visible = true;
                        break;
                }
            }

            m_ignoreToolClick = false;
            _panel_View.ResumeLayout();

            setMenuConfiguration();
        }

        #endregion

        #region Events

        private void _ultraToolbarsManager_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            //setting the Checked property on the tools for appearance fires the click event
            if (m_ignoreToolClick)
            {
                return;
            }
            if (e.Tool != null)
            {
                switch (e.Tool.Key)
                {
                    case RibbonTaskButton.AddPolicy:
                        showNewPolicy();
                        break;
                    case RibbonTaskButton.EditPolicy:
                        showEditPolicy();
                        break;
                    case RibbonTaskButton.CopyAssessment:
                        showCopyPolicy();
                        break;
                    case RibbonTaskButton.Compare:
                        showCompare();
                        break;
                    case RibbonTaskButton.AddServer:
                        showNewAuditServer();
                        break;
                    case RibbonTaskButton.Configure:
                        showConfigure();
                        break;
                    case RibbonTaskButton.TakeSnapshot:
                        showCollect();
                        break;
                    case RibbonTaskButton.ExploreUsers:
                        showPermissions(Views.View_PermissionExplorer.Tab.UserPermissions);
                        break;
                    case RibbonTaskButton.ExploreRoles:
                        showPermissions(Views.View_PermissionExplorer.Tab.RolePermissions);
                        break;
                    case RibbonTaskButton.ExploreObjects:
                        showPermissions(Views.View_PermissionExplorer.Tab.ObjectPermissions);
                        break;
                    case RibbonTaskButton.ViewReports:
                        showReports();
                        break;
                    case RibbonTaskButton.ImportServers:
                        ShowImportServers();
                        break;
                    case RibbonAuditData.Select:
                        selectAuditData();
                        break;
                }
            }
        }

        private void ShowImportServers()
        {
            Forms.Form_ImportServers.Process();
        }

        private void _ultraToolbarsManager_BeforeRibbonTabSelected(object sender, BeforeRibbonTabSelectedEventArgs e)
        {
            if (e.Tab != null)
            {
                switch (e.Tab.Key)
                {
                    case RibbonTabView.Summary:
                        setViewContext(SecurityView.Summary);
                        break;
                    case RibbonTabView.Settings:
                        setViewContext(SecurityView.Settings);
                        break;
                    case RibbonTabView.Users:
                        setViewContext(SecurityView.Users);
                        break;
                }
            }
        }

        private void _ultraToolbarsManager_BeforeToolbarListDropdown(object sender, BeforeToolbarListDropdownEventArgs e)
        {
            e.ShowQuickAccessToolbarPositionMenuItem = false;
        }

        private void View_Main_SecuritySummary_Enter(object sender, EventArgs e)
        {
            setMenuConfiguration();
        }

        private void View_Main_SecuritySummary_Leave(object sender, EventArgs e)
        {
            Program.gController.SetMenuConfiguration(new Utility.MenuConfiguration());
        }

        #endregion
    }
}
