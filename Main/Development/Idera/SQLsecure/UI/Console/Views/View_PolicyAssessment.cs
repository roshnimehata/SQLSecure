using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Data;
using System.Windows.Forms;

using Idera.SQLsecure.Core.Accounts;
using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Controls;
using Idera.SQLsecure.UI.Console.Forms;
using Idera.SQLsecure.UI.Console.Utility;

using Infragistics.Win.UltraWinToolbars;
using Policy=Idera.SQLsecure.UI.Console.Sql.Policy;

namespace Idera.SQLsecure.UI.Console.Views
{
    public partial class View_PolicyAssessment : UserControl, Interfaces.IView, Interfaces.ICommandHandler, Interfaces.IRefresh
    {
        #region types and enums

        public enum AssessmentView
        {
            None = -1,
            Summary = 0,
            Log = 1
        }

        private class RibbonTabView
        {
            public const string Summary = @"Summary";
            public const string Log = @"Change Log";
        }

        private class RibbonTaskButton
        {
            public const string EditPolicy = @"_buttonTool_EditPolicy";
            public const string UpdatePolicyFindings = @"_buttonTool_UpdateFindings";
            public const string Promote = @"_buttonTool_Promote";
            public const string CopyAssessment = @"_buttonTool_CopyPolicy";
            public const string Compare = @"_buttonTool_Compare";
            public const string Remove = @"_buttonTool_Remove";
            public const string AddServer = @"_buttonTool_AddServer";
            public const string ConfigureServer = @"_buttonTool_Configure";
            public const string TakeSnapshot = @"_buttonTool_CollectData";
            public const string ExploreUsers = @"_buttonTool_ExploreUsers";
            public const string ExploreRoles = @"_buttonTool_ExploreRoles";
            public const string ExploreObjects = @"_buttonTool_ExploreObjects";
            public const string ViewReports = @"_buttonTool_ViewReports";
            public const string ConfigureSecurityCheck = @"_buttonTool_ConfigureSecurityCheck";
            public const string EditExplanationNotes = @"_buttonTool_EditNotes";
        }

        private class RibbonGroup
        {
            public const string SummaryAssessmentActions = @"_ribbonGroup_Summary_PolicyActions";
            public const string SummarySecurityCheckActions = @"_ribbonGroup_Summary_SecurityCheckActions";
            public const string SummaryServerActions = @"_ribbonGroup_Summary_ServerActions";
            public const string LogAssessmentActions = @"_ribbonGroup_Summary_PolicyActions";
        }

        #endregion

        #region IView

        void Interfaces.IView.SetContext(Interfaces.IDataContext contextIn)
        {
            // Set the title.
            m_Title = contextIn.Name;

            m_context = (Data.PolicyAssessment)contextIn;
            m_policy = m_context.Policy;
            m_server = m_context.Server;

            _ultraToolbarsManager.Ribbon.SelectedTab = _ultraToolbarsManager.Ribbon.Tabs[(int)(m_context.AssessmentView == AssessmentView.None ? m_selectedView : m_context.AssessmentView)];

            m_selectedMetricId = null;

            showRefresh();
        }
        String Interfaces.IView.HelpTopic
        {
            get
            {
                string helpTopic = Utility.Help.SecuritySummaryPolicyReportCardHelpTopic;
                if (m_server == null)
                {
                    if (_ultraToolbarsManager.Ribbon.SelectedTab.Index == (int)AssessmentView.Summary)
                    {
                        if (m_policy.IsDraftAssessment)
                        {
                            helpTopic = Utility.Help.AssessmentSummaryDraftAllServersHelpTopic;
                        }
                        else if (m_policy.IsPublishedAssessment)
                        {
                            helpTopic = Utility.Help.AssessmentSummaryPublishedAllServersHelpTopic;
                        }
                        else if (m_policy.IsApprovedAssessment)
                        {
                            helpTopic = Utility.Help.AssessmentSummaryApprovedAllServersHelpTopic;
                        }
                    }
                    else if (_ultraToolbarsManager.Ribbon.SelectedTab.Index == (int)AssessmentView.Log)
                        helpTopic = Utility.Help.SecuritySummaryChangeLogHelpTopic;
                }
                else
                    if (_ultraToolbarsManager.Ribbon.SelectedTab.Index == (int)AssessmentView.Summary)
                    {
                        if (m_policy.IsDraftAssessment)
                        {
                            helpTopic = Utility.Help.AssessmentSummaryDraftServerHelpTopic;
                        }
                        else if (m_policy.IsPublishedAssessment)
                        {
                            helpTopic = Utility.Help.AssessmentSummaryPublishedServerHelpTopic;
                        }
                        else if (m_policy.IsApprovedAssessment)
                        {
                            helpTopic = Utility.Help.AssessmentSummaryApprovedServerHelpTopic;
                        }
                    }
                    else if (_ultraToolbarsManager.Ribbon.SelectedTab.Index == (int)AssessmentView.Log)
                        helpTopic = Utility.Help.SecuritySummaryChangeLogHelpTopic;

                return helpTopic;
            }
        }
        String Interfaces.IView.ConceptTopic
        {
            get { return Utility.Help.SecuritySummaryPolicyReportCardHelpTopic; }
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
            //    case Utility.ViewSpecificCommand.NewAuditServer:
            //        showNewAuditServer();
            //        break;
            //    //case Utility.ViewSpecificCommand.NewLogin:
            //    //    showAddLogin();
            //    //    break;
                case Utility.ViewSpecificCommand.Delete:
                    showDelete();
                    break;
            //    case Utility.ViewSpecificCommand.Configure:
            //        showConfigure();
            //        break;
            //    case Utility.ViewSpecificCommand.UserPermissions:
            //        showPermissions(Views.View_PermissionExplorer.Tab.UserPermissions);
            //        break;
            //    case Utility.ViewSpecificCommand.ObjectPermissions:
            //        showPermissions(Views.View_PermissionExplorer.Tab.ObjectPermissions);
            //        break;
            //    case Utility.ViewSpecificCommand.Collect:
            //        showCollect();
            //        break;
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
                    Sql.Policy policy = Program.gController.Repository.Policies.Find(m_policy.PolicyId);
                    if (policy != null && policy.HasAssessment(m_policy.AssessmentId))
                    {
                        // Display confirmation, if user confirms remove the policy.
                        string caption = Utility.ErrorMsgs.RemoveAssessmentCaption;
                        if (DialogResult.Yes == Utility.MsgBox.ShowWarningConfirm(caption, string.Format(Utility.ErrorMsgs.RemoveAssessmentConfirmMsg, m_policy.PolicyAssessmentName)))
                        {
                            try
                            {
                                Sql.Policy.RemoveAssessment(m_policy.PolicyId, m_policy.AssessmentId);
                            }
                            catch (Exception ex)
                            {
                                Utility.MsgBox.ShowError(Utility.ErrorMsgs.RemoveAssessmentCaption, string.Format(Utility.ErrorMsgs.CantRemoveAssessmentMsg, m_policy.PolicyAssessmentName), ex);
                            }

                            Program.gController.SignalRefreshPoliciesEvent(0);
                        }
                    }
                    else
                    {
                        Utility.MsgBox.ShowError(Utility.ErrorMsgs.RemoveAssessmentCaption, Utility.ErrorMsgs.AssessmentNotFound);
                        Program.gController.SignalRefreshPoliciesEvent(0);
                    }
                }
                else
                {
                    Utility.MsgBox.ShowError(Utility.ErrorMsgs.RemoveAssessmentCaption, Utility.ErrorMsgs.PolicyNotRegistered);
                    Program.gController.SignalRefreshPoliciesEvent(0);
                }
            }
            else
            {
                if (!m_policy.IsDynamic)
                {
                    if (DialogResult.Yes == Utility.MsgBox.ShowWarningConfirm(Utility.ErrorMsgs.RemoveServerFromAssessmentCaption,
                                string.Format(Utility.ErrorMsgs.RemoveServerFromAssessmentConfirmMsg, m_server.ConnectionName, m_policy.PolicyAssessmentName)))
                    {
                        Sql.RegisteredServer.RemoveRegisteredServerFromPolicy(m_server.RegisteredServerId, m_policy.PolicyId, m_policy.AssessmentId);

                        Program.gController.SignalRefreshPoliciesEvent(0);
                    }
                }
                else
                {
                    Utility.MsgBox.ShowError(Utility.ErrorMsgs.PolicyCaption, Utility.ErrorMsgs.RemoveDynamicAssessmentServerMsg);
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
            if (m_policy.IsDraftAssessment)
            {
                if (_ultraToolbarsManager.Ribbon.SelectedTab.Key == RibbonTabView.Log)
                {
                    _ultraToolbarsManager.Ribbon.SelectedTab = _ultraToolbarsManager.Ribbon.Tabs[RibbonTabView.Summary];
                }
                _ultraToolbarsManager.Ribbon.Tabs[RibbonTabView.Log].Visible = false;
            }
            else
            {
                _ultraToolbarsManager.Ribbon.Tabs[RibbonTabView.Log].Visible = true;
            }

            if (m_policy.IsDraftAssessment)
            {
                (_ultraToolbarsManager.Ribbon.Tabs[RibbonTabView.Summary].Groups[RibbonGroup.SummaryAssessmentActions].Tools[RibbonTaskButton.Promote]).SharedProps.Caption = @"Publish";
                (_ultraToolbarsManager.Ribbon.Tabs[RibbonTabView.Summary].Groups[RibbonGroup.SummaryAssessmentActions].
                    Tools[RibbonTaskButton.Promote]).SharedProps.AppearancesLarge.Appearance.Image =
                    global::Idera.SQLsecure.UI.Console.Properties.Resources.Assessment_Publish_48;
                (_ultraToolbarsManager.Ribbon.Tabs[RibbonTabView.Summary].Groups[RibbonGroup.SummaryAssessmentActions].Tools[RibbonTaskButton.Promote]).SharedProps.Visible = true;
            }
            else if (m_policy.IsPublishedAssessment)
            {
                (_ultraToolbarsManager.Ribbon.Tabs[RibbonTabView.Summary].Groups[RibbonGroup.SummaryAssessmentActions].Tools[RibbonTaskButton.Promote]).SharedProps.Caption = @"Approve";
                (_ultraToolbarsManager.Ribbon.Tabs[RibbonTabView.Summary].Groups[RibbonGroup.SummaryAssessmentActions].
                    Tools[RibbonTaskButton.Promote]).SharedProps.AppearancesLarge.Appearance.Image =
                    global::Idera.SQLsecure.UI.Console.Properties.Resources.Assessment_Approved_48;
                (_ultraToolbarsManager.Ribbon.Tabs[RibbonTabView.Summary].Groups[RibbonGroup.SummaryAssessmentActions].Tools[RibbonTaskButton.Promote]).SharedProps.Visible = true;
            }
            else if (m_policy.IsApprovedAssessment)
            {
                (_ultraToolbarsManager.Ribbon.Tabs[RibbonTabView.Summary].Groups[RibbonGroup.SummaryAssessmentActions].Tools[RibbonTaskButton.Promote]).SharedProps.Visible = false;
            }
            _ultraToolbarsManager.Ribbon.Tabs[RibbonTabView.Summary].Groups[RibbonGroup.SummaryServerActions].Visible = !m_policy.IsApprovedAssessment;

            _ultraToolbarsManager.Tools[RibbonTaskButton.EditPolicy].SharedProps.Caption = string.Format("{0} Settings", Program.gController.isAdmin && !m_policy.IsApprovedAssessment ? @"Edit" : @"View");
            _ultraToolbarsManager.Tools[RibbonTaskButton.EditPolicy].SharedProps.Enabled = true;
            _ultraToolbarsManager.Tools[RibbonTaskButton.UpdatePolicyFindings].SharedProps.Enabled = Program.gController.isAdmin && !m_policy.IsApprovedAssessment;
            _ultraToolbarsManager.Tools[RibbonTaskButton.Promote].SharedProps.Enabled = Program.gController.isAdmin && !m_policy.IsApprovedAssessment;
            _ultraToolbarsManager.Tools[RibbonTaskButton.CopyAssessment].SharedProps.Enabled = Program.gController.isAdmin;
            _ultraToolbarsManager.Tools[RibbonTaskButton.Compare].SharedProps.Enabled = true;
            _ultraToolbarsManager.Tools[RibbonTaskButton.Remove].SharedProps.Caption = string.Format("Remove {0}Assessment", m_server == null ? string.Empty : "from ");
            _ultraToolbarsManager.Tools[RibbonTaskButton.Remove].SharedProps.Enabled = Program.gController.isAdmin && !m_policy.IsApprovedAssessment;
            _ultraToolbarsManager.Tools[RibbonTaskButton.TakeSnapshot].SharedProps.Enabled = Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Collect);

            RefreshSecurityCheckButtons();

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

        protected void showUpdateAssessment()
        {
            if (m_policy != null)
            {
                // returns true if updated
                if (Forms.Form_RefreshAuditData.Process(m_policy))
                {
                    Program.gController.RefreshPolicyInTree(m_policy);
                    Program.gController.RefreshCurrentView();
                }
            }
        }

        protected void showCompare()
        {
            Program.gController.ShowCompareDialog(m_policy.PolicyId, m_policy.AssessmentId, m_server);
        }

        protected void showCopyPolicy()
        {
            if (m_policy != null)
            {
                Forms.Form_CreateAssessment.Process(m_policy);
            }
        }

        protected void showPromote()
        {
            if (m_policy != null)
            {
                if (m_policy.PromoteAssessment())
                {
                    Program.gController.SignalRefreshPoliciesEvent(m_policy.PolicyId);
                }
            }
        }

        private void showConfigureSecurityCheck()
        {
            if (m_policy != null && m_selectedMetricId != null)
            {
                // returns true if updated
                if (Forms.Form_PolicyProperties.Process(m_policy.PolicyId,
                                                        m_policy.AssessmentId,
                                                        Program.gController.isAdmin,
                                                        Forms.Form_PolicyProperties.RequestedOperation.
                                                            ConfigureMetrics,
                                                        m_selectedMetricId.Value))
                {
                    Program.gController.SignalRefreshPoliciesEvent(m_policy.PolicyId);
                }
            }
        }

        protected void showExplanationNotes()
        {
            if (m_selectedMetricId.HasValue)
            {
                Form_ExplanationNotes.Process(m_policy.PolicyId, m_policy.AssessmentId, m_selectedMetricId.Value, m_server == null ? null : m_server.ConnectionName);
            }
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

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Views.View_PolicyAssessment");

        private string m_Title;
        private Data.PolicyAssessment m_context;
        private Utility.MenuConfiguration m_MenuConfiguration;
        private bool m_ignoreToolClick = false;

        private Sql.Policy m_policy;
        private Sql.RegisteredServer m_server;
        private bool m_showMsg = true;
        private AssessmentView m_selectedView = AssessmentView.Summary;

        private int? m_selectedMetricId = null;

        #endregion

        #region queries & constants

        private const string QueryGetPolicyServers = @"SQLsecure.dbo.isp_sqlsecure_getpolicyserverlist";
        private const string ParamPolicyId = @"@policyid";
        private const string ParamAssessmentId = @"@assessmentid";

        private const string DisplayBaseline = @"Use baseline snapshot data only";
        private const string DisplayCurrent = @"Use most current data";
        private const string DisplayDate = "as of {0}";
        private const string DisplayNoServers = @"This policy has no servers and cannot be assessed.";
        private const string DisplayGetServersError = @"Unable to retrieve policy servers from repository.";

        #endregion

        #region Ctors

        public View_PolicyAssessment()
        {
            InitializeComponent();

            // Initialize menu configuration.
            m_MenuConfiguration = new Utility.MenuConfiguration();

            _ultraToolbarsManager.Tools[RibbonTaskButton.EditPolicy].SharedProps.Enabled =
                _ultraToolbarsManager.Tools[RibbonTaskButton.UpdatePolicyFindings].SharedProps.Enabled =
                _ultraToolbarsManager.Tools[RibbonTaskButton.Promote].SharedProps.Enabled =
                _ultraToolbarsManager.Tools[RibbonTaskButton.CopyAssessment].SharedProps.Enabled =
                _ultraToolbarsManager.Tools[RibbonTaskButton.Compare].SharedProps.Enabled =
                _ultraToolbarsManager.Tools[RibbonTaskButton.TakeSnapshot].SharedProps.Enabled = false;

            // Dock the controls that are not docked in the designer for visibility
            _policySummary.Dock =
                _policyChangeLog.Dock =
                _label_Msg.Dock = DockStyle.Fill;

            _label_Msg.Text = DisplayNoServers;

            _label_Msg.Visible =
            _policyChangeLog.Visible = false;
            _policySummary.Visible = true;
            _policySummary.RegisterReportCardMetricChangedDelegate(ReportCardMetricChanged);
        }

        #endregion

        #region properties

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
                    SqlParameter paramAssessmentId = new SqlParameter(ParamAssessmentId, m_policy.AssessmentId);

                    // Get Servers
                    SqlCommand cmd = new SqlCommand(QueryGetPolicyServers, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(paramPolicyId);
                    cmd.Parameters.Add(paramAssessmentId);

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
                        m_policy.GetPolicySnapshotIds();
                    }
                    setViewContext(m_selectedView);
                }
                _panel_View.ResumeLayout();
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error(@"Error - Unable to retrieve assessment info", ex);
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
            m_MenuConfiguration.EditItems[(int)Utility.MenuItems_Edit.Remove] = !m_policy.IsApprovedAssessment;
            m_MenuConfiguration.EditItems[(int)Utility.MenuItems_Edit.Properties] = true;
            m_MenuConfiguration.EditItems[(int)Utility.MenuItems_Edit.ConfigureDataCollection] = (m_server != null);
            m_MenuConfiguration.SnapshotItems[(int)Utility.MenuItems_Snapshots.Collect] = true;

            Program.gController.SetMenuConfiguration(m_MenuConfiguration, this);
        }

        protected void setViewContext(AssessmentView view)
        {
            _panel_View.SuspendLayout();
            m_ignoreToolClick = true;

            if (m_selectedView != view)
            {
                _label_Msg.Visible =
                    _policySummary.Visible =
                _policyChangeLog.Visible = false;
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
                    case AssessmentView.Summary:
                        if (m_server == null)
                        {
                            ((Interfaces.IView)_policySummary).SetContext(new Data.PolicySummary(m_policy));
                        }
                        else
                        {
                            ((Interfaces.IView)_policySummary).SetContext(new Data.PolicySummary(m_policy, m_server));
                        }
                        _policySummary.Visible = true;
                        break;
                    case AssessmentView.Log:
                        // The policy change log uses the PolicyAssessment data object for it's IView context
                        ((Interfaces.IView)_policyChangeLog).SetContext(m_context);
                        _policyChangeLog.Visible = true;
                        break;
                }
            }

            m_ignoreToolClick = false;
            _panel_View.ResumeLayout();

            setMenuConfiguration();
        }

        private void ReportCardMetricChanged(int? metricId)
        {
            m_selectedMetricId = metricId;
            RefreshSecurityCheckButtons();
        }

        private void RefreshSecurityCheckButtons()
        {
            _ultraToolbarsManager.Tools[RibbonTaskButton.ConfigureSecurityCheck].SharedProps.Enabled = Program.gController.isAdmin && !m_policy.IsApprovedAssessment && m_selectedMetricId != null;
            _ultraToolbarsManager.Tools[RibbonTaskButton.EditExplanationNotes].SharedProps.Enabled = Program.gController.isAdmin && !m_policy.IsApprovedAssessment && m_selectedMetricId != null;
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
                    case RibbonTaskButton.EditPolicy:
                        showEditPolicy();
                        break;
                    case RibbonTaskButton.UpdatePolicyFindings:
                        showUpdateAssessment();
                        break;
                    case RibbonTaskButton.Promote:
                        showPromote();
                        break;
                    case RibbonTaskButton.CopyAssessment:
                        showCopyPolicy();
                        break;
                    case RibbonTaskButton.Compare:
                        showCompare();
                        break;
                    case RibbonTaskButton.Remove:
                        showDelete();
                        break;
                    case RibbonTaskButton.AddServer:
                        showNewAuditServer();
                        break;
                    case RibbonTaskButton.ConfigureServer:
                        showConfigure();
                        break;
                    case RibbonTaskButton.ConfigureSecurityCheck:
                        showConfigureSecurityCheck();
                        break;
                    case RibbonTaskButton.EditExplanationNotes:
                        showExplanationNotes();
                        break;
                    case RibbonTaskButton.TakeSnapshot:
                        showCollect();
                        break;
                }
            }
        }

        private void _ultraToolbarsManager_BeforeRibbonTabSelected(object sender, BeforeRibbonTabSelectedEventArgs e)
        {
            if (e.Tab != null)
            {
                switch (e.Tab.Key)
                {
                    case RibbonTabView.Summary:
                        setViewContext(AssessmentView.Summary);
                        break;
                    case RibbonTabView.Log:
                        setViewContext(AssessmentView.Log);
                        break;
                }
            }
        }

        private void _ultraToolbarsManager_BeforeToolbarListDropdown(object sender, BeforeToolbarListDropdownEventArgs e)
        {
            e.ShowQuickAccessToolbarPositionMenuItem = false;
        }

        private void View_PolicyAssessment_Enter(object sender, EventArgs e)
        {
            setMenuConfiguration();
        }

        private void View_PolicyAssessment_Leave(object sender, EventArgs e)
        {
            Program.gController.SetMenuConfiguration(new Utility.MenuConfiguration());
        }

        #endregion
    }
}
