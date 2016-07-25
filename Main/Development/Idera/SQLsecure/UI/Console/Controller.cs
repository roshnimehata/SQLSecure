/******************************************************************
 * Name: Controller.cs
 *
 * Description: Manages resources that are shared by different
 * views, such as tool bars, menu bars, etc.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;
using System.Diagnostics;
using Idera.SQLsecure.UI.Console.Controls;
using Idera.SQLsecure.UI.Console.Utility;
using Idera.SQLsecure.UI.Console.Views;
using View=Idera.SQLsecure.UI.Console.Utility.View;

using Idera.SQLsecure.Core.Logger;

namespace Idera.SQLsecure.UI.Console
{
    internal class Controller
    {
        #region Fields

        private static readonly LogX logX = new LogX("Idera.SQLsecure.UI.Console.Controller");

        #region Data

        // These fields refer to data related objects
        private Utility.Security m_Permissions = new Utility.Security();
        private Sql.Repository m_Repository = new Sql.Repository();

        #endregion

        #region Visual Elements

        // These fields refer to visual elements of the UI.
        private MainForm m_MainForm;
        private TreeView m_PolicyTree;
        private TreeView m_PolicyServerTree;
        private TreeView m_ExplorerTree;
        private TreeView m_ReportsTree;
        private Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar m_ExplorerBar;
        private System.Windows.Forms.Control m_RightPane;

        // Security Summary group views
        private Views.View_Main_SecuritySummary m_View_Main_SecuritySummary;
        private Views.View_PolicyAssessment m_View_PolicyAssessment;

        // Explore permissions group views
        private Views.View_Main_ExplorePermissions m_View_Main_ExplorePermissions;
        private Views.View_Server m_View_Server;
        private Views.View_PermissionExplorer m_View_PermissionExplorer;

        // Manage SQLsecure group views.
        private Views.View_Main_ManageSQLsecure m_View_Main_ManageSQLsecure;
        private Views.View_SQLsecureActivity m_View_SQLsecureActivity;
        private Views.View_Logins m_View_Logins;
        private Views.View_ManagePolicies m_View_ManagePolicies;
        private Views.View_ServerTags m_View_ServerTags;

        // Reports group views
        private Views.View_Main_Reports m_View_Main_Reports;
        private Views.ControlAuditedInstancesReport m_View_Report_AuditedServers;
        private Views.Report_Filters m_View_Report_Filters;
        private Views.Report_GuestEnabledDatabases m_View_Report_GuestEnabledServers;
        private Views.Report_CrossServerLoginCheck m_View_Report_CrossServerLoginCheck;
        private Views.Report_MixedModeAuthentication m_View_Report_MixedModeAuthentication;
        private Views.Report_ServersWithDangerousGroups m_View_Report_ServersWithDangerousGroups;
        private Views.Report_SystemAdministratorVulnerability m_View_Report_SystemAdministratorVulnerability;
        private Views.Report_SuspectWindowsAccounts m_View_Report_SuspectWindowsAccounts;
        private Views.Report_VulnerableFixedRoles m_View_Report_VulnerableFixedRoles;
        private Views.Report_MailVulnerability m_View_Report_MailVulnerability;
        private Views.Report_LoginVulnerability m_View_Report_LoginVulnerability;
        private Views.Report_ServerLoginsAndUserMappings m_View_Report_ServerLoginsAndUserMappings;
        private Views.Report_DatabaseChaining m_View_Report_DatabaseChaining;
        private Views.Report_OSVulnerability m_View_Report_SPsWithGuestAccess;
        private Views.Report_UsersPermissions m_View_Report_UsersPermissions;
        private Views.Report_AllObjectsWithPermissions m_View_Report_AllObjectsWithPermissions;
        private Views.Report_DatabaseRoles m_View_Report_DatabaseRoles;
        private Views.Report_ServerRoles m_View_Report_ServerRoles;
        private Views.Report_Users m_View_Report_Users;
        private Views.Report_ActivityHistory m_View_Report_ActivityHistory;
        private Views.Report_RiskAssessment m_View_Report_RiskAssessment;
        private Views.Report_CompareAssessments m_View_Report_CompareAssessments;
        private Views.Report_CompareSnapshots m_View_Report_CompareSnapshots;
        private Views.Report_SuspectSqlLogins m_View_Report_SuspectSqlLogins;

        #endregion

        #region View Stacks and Menu Configuration

        // Control stacks for each of the Explorer Bar groups.
        private Stack<Control> m_SecuritySummaryViewStack = new Stack<Control>();
        private Stack<Control> m_ExplorePermissionsViewStack = new Stack<Control>();
        private Stack<Control> m_ReportsViewStack = new Stack<Control>();
        private Stack<Control> m_ManageSQLsecureViewStack = new Stack<Control>();

        private Utility.MenuConfiguration m_SecuritySummaryMenuConfiguration = new Utility.MenuConfiguration();
        private Utility.MenuConfiguration m_ExplorePermissionsMenuConfiguration = new Utility.MenuConfiguration();
        private Utility.MenuConfiguration m_ReportsMenuConfiguration = new Utility.MenuConfiguration();
        private Utility.MenuConfiguration m_ManageSQLsecureMenuConfiguration = new Utility.MenuConfiguration();

        #endregion

        #region Current State

        private Utility.ExplorerBarGroup m_CurrentGroup;
        private Stack<Control> m_CurrentViewStack;

        // State vars for handling menu/tool bars.
        private Utility.MenuConfiguration m_CurrentMenuConfiguration = new Utility.MenuConfiguration();
        private Interfaces.ICommandHandler m_CurrentCommandHandler; 

        #endregion

        #endregion

        #region Helpers

        private Stack<Control> getGroupStack(Utility.ExplorerBarGroup groupIn)
        {
            Stack<Control> ret = null;

            switch (groupIn)
            {
                case Utility.ExplorerBarGroup.SecuritySummary:
                    ret = m_SecuritySummaryViewStack;
                    break;
                case Utility.ExplorerBarGroup.ExplorePermissions:
                    ret = m_ExplorePermissionsViewStack;
                    break;
                case Utility.ExplorerBarGroup.Reports:
                    ret = m_ReportsViewStack;
                    break;
                case Utility.ExplorerBarGroup.ManageSQLsecure:
                    ret = m_ManageSQLsecureViewStack; 
                    break;
                default:
                    Debug.Assert(false, "Invalid group");
                    break;
            }

            return ret;
        }

        private Utility.MenuConfiguration getGroupMenuConfiguration(Utility.ExplorerBarGroup groupIn)
        {
            Utility.MenuConfiguration ret = null;

            switch (groupIn)
            {
                case Utility.ExplorerBarGroup.SecuritySummary:
                    ret =m_SecuritySummaryMenuConfiguration;
                    break;
                case Utility.ExplorerBarGroup.ExplorePermissions:
                    ret = m_ExplorePermissionsMenuConfiguration;
                    break;
                case Utility.ExplorerBarGroup.Reports:
                    ret = m_ReportsMenuConfiguration;
                    break;
                case Utility.ExplorerBarGroup.ManageSQLsecure:
                    ret = m_ManageSQLsecureMenuConfiguration;
                    break;
                default:
                    Debug.Assert(false, "Invalid group");
                    break;
            }

            return ret;
        }

        private Control getGroupDefaultView(Utility.ExplorerBarGroup groupIn)
        {
            Control ret = null;

            switch (groupIn)
            {
                case Utility.ExplorerBarGroup.SecuritySummary:
                    ret = m_View_Main_SecuritySummary;
                    break;
                case Utility.ExplorerBarGroup.ExplorePermissions:
                    ret = m_View_Main_ExplorePermissions;
                    break;
                case Utility.ExplorerBarGroup.Reports:
                    ret = m_View_Main_Reports;
                    break;
                case Utility.ExplorerBarGroup.ManageSQLsecure:
                    ret = m_View_Main_ManageSQLsecure;
                    break;
                default:
                    Debug.Assert(false, "Invalid group");
                    break;
            }

            return ret;
        }

        private Control getView(
                Stack<Control> stackIn,
                Utility.NodeTag nodeTagIn
            )
        {
            Debug.Assert(nodeTagIn != null);

            // Identify the view.
            Control ret = null;
            switch (nodeTagIn.View)
            {
                // Security summary group views.
                case Utility.View.Main_SecuritySummary:
                    ret = m_View_Main_SecuritySummary;
                    break;
                case Utility.View.PolicyAssessment:
                    ret = m_View_PolicyAssessment;
                    break;

                // Explore permissions group views.
                case Utility.View.Main_ExplorePermissions:
                    ret = m_View_Main_ExplorePermissions;
                    break;
                case Utility.View.Server:
                    ret = m_View_Server;
                    break;
                case Utility.View.PermissionExplorer:
                    ret = m_View_PermissionExplorer;
                    break;

                // Manage SQLsecure group views.
                case Utility.View.Main_ManageSQLsecure:
                    ret = m_View_Main_ManageSQLsecure;
                    break;
                case Utility.View.SQLsecureActivity:
                    ret = m_View_SQLsecureActivity;
                    break;
                case Utility.View.Logins:
                    ret = m_View_Logins;
                    break;
                case Utility.View.ManagePolicies:
                    ret = m_View_ManagePolicies;
                    break;
                case Utility.View.ServerGroupTags:
                    ret = m_View_ServerTags;
                    break;

                // Reports group views
                case Utility.View.Main_Reports:
                    ret = m_View_Main_Reports;
                    break;
                case Utility.View.Report_AuditedServers:
                    ret = m_View_Report_AuditedServers;
                    break;
                case Utility.View.Report_Filters:
                    ret = m_View_Report_Filters;
                    break;
                case Utility.View.Report_GuestEnabledDatabases:
                    ret = m_View_Report_GuestEnabledServers;
                    break;
                case Utility.View.Report_CrossServerLoginCheck:
                    ret = m_View_Report_CrossServerLoginCheck;
                    break;
                case Utility.View.Report_MixedModeAuthentication:
                    ret = m_View_Report_MixedModeAuthentication;
                    break;
                case Utility.View.Report_ServersWithDangerousGroups:
                    ret = m_View_Report_ServersWithDangerousGroups;
                    break;
                case Utility.View.Report_SystemAdministratorVulnerability:
                    ret = m_View_Report_SystemAdministratorVulnerability;
                    break;
                case Utility.View.Report_SuspectWindowsAccounts:
                    ret = m_View_Report_SuspectWindowsAccounts;
                    break;
                case Utility.View.Report_SuspectSqlLogins:
                    ret = m_View_Report_SuspectSqlLogins;
                    break;
                case Utility.View.Report_VulnerableFixedRoles:
                    ret = m_View_Report_VulnerableFixedRoles;
                    break;                
                case Utility.View.Report_MailVulnerability:
                    ret = m_View_Report_MailVulnerability;
                    break;
                case Utility.View.Report_LoginVulnerability:
                    ret = m_View_Report_LoginVulnerability;
                    break;
                case Utility.View.Report_ServerLoginsAndUserMappings:
                    ret = m_View_Report_ServerLoginsAndUserMappings;
                    break;
                case Utility.View.Report_DatabaseChaining:
                    ret = m_View_Report_DatabaseChaining;
                    break;
                case Utility.View.Report_OSVulnerability:
                    ret = m_View_Report_SPsWithGuestAccess;
                    break;
                case Utility.View.Report_UsersPermissions:
                    ret = m_View_Report_UsersPermissions;
                    break;
                case Utility.View.Report_AllObjectsWithPermissions:
                    ret = m_View_Report_AllObjectsWithPermissions;
                    break;
                case Utility.View.Report_DatabaseRoles:
                    ret = m_View_Report_DatabaseRoles;
                    break;
                case Utility.View.Report_ServerRoles:
                    ret = m_View_Report_ServerRoles;
                    break;
                case Utility.View.Report_Users:
                    ret = m_View_Report_Users;
                    break;
                case Utility.View.Report_ActivityHistory:
                    ret = m_View_Report_ActivityHistory;
                    break;
                case Utility.View.Report_RiskAssessment:
                    ret = m_View_Report_RiskAssessment;
                    break;
                case Utility.View.Report_CompareAssessments:
                    ret = m_View_Report_CompareAssessments;
                    break;
                case Utility.View.Report_CompareSnapshots:
                    ret = m_View_Report_CompareSnapshots;
                    break;

                default:
                    Debug.Assert(false, "Unknown view");
                    break;
            }

            // Setup the view context.
            if (ret != null)
            {
                // Set the context.
                ((Interfaces.IView)ret).SetContext(nodeTagIn.DataContext);
            }

            return ret;
        }

        private Control getCurrentView()
        {
            Debug.Assert(m_CurrentViewStack.Count != 0, "Empty stack");
            return m_CurrentViewStack.Peek();
        }

        private String getCurrentHelpTopic()
        {
            Debug.Assert(m_CurrentViewStack.Count != 0, "Empty stack");
            return ((Interfaces.IView)m_CurrentViewStack.Peek()).HelpTopic;
        }

        private String getCurrentConceptTopic()
        {
            Debug.Assert(m_CurrentViewStack.Count != 0, "Empty stack");
            return ((Interfaces.IView)m_CurrentViewStack.Peek()).ConceptTopic;
        }

        private void displayCurrentView()
        {
            using (logX.loggerX.DebugCall("Controller displayCurrentView"))
            {
                // Get the view from the stack.
                System.Windows.Forms.Control control = getCurrentView();

                // Set the title.
                m_MainForm.ResultHeaderTitle = ((Interfaces.IView) control).Title;

                // Set the right side control
                logX.loggerX.Debug("Clear right pane");
                m_RightPane.Controls.Clear();
                control.Dock = DockStyle.Fill;
                logX.loggerX.Debug("Load right pane with current view");
                m_RightPane.Controls.Add(control);

                // Register the view with user data to manage configuration changes
                Controls.BaseView bv = control as Controls.BaseView;
                if (bv != null)
                {
                    // Show the show tasks button.
                    m_MainForm.ShowHideButtonVisible = true;

                    // Set up the handler for show/hide tasks.
                    Utility.UserData.Current.View.ViewDataEvent += bv.viewDataHandler;

                    // Set the task pane visibility.
                    bv.viewDataHandler();
                }
                else
                {
                    // Hide the show tasks button.
                    m_MainForm.ShowHideButtonVisible = false;
                }
            }
        }

        private static void refreshView(Interfaces.IRefresh refreshableView)
        {
            refreshableView.RefreshView();
        }

        public void ResetViews()
        {
            using (logX.loggerX.DebugCall("Controller ResetViews"))
            {
                m_SecuritySummaryViewStack.Clear();
                m_ExplorePermissionsViewStack.Clear();
                m_ReportsViewStack.Clear();
                m_ManageSQLsecureViewStack.Clear();

                m_SecuritySummaryMenuConfiguration = new Utility.MenuConfiguration();
                m_ExplorePermissionsMenuConfiguration = new Utility.MenuConfiguration();
                m_ReportsMenuConfiguration = new Utility.MenuConfiguration();
                m_ManageSQLsecureMenuConfiguration = new Utility.MenuConfiguration();

                // reconstruct the views to clear any old data
                // Security summary group views
                m_View_Main_SecuritySummary = new Views.View_Main_SecuritySummary();
                m_View_PolicyAssessment = new Views.View_PolicyAssessment();

                // Explore permissions group views
                m_View_Main_ExplorePermissions = new Views.View_Main_ExplorePermissions();
                m_View_Server = new Views.View_Server();
                m_View_PermissionExplorer = new Views.View_PermissionExplorer();

                // Manage SQLsecure group views
                m_View_Main_ManageSQLsecure = new Views.View_Main_ManageSQLsecure();
                m_View_SQLsecureActivity = new Views.View_SQLsecureActivity();
                m_View_Logins = new Views.View_Logins();
                m_View_ManagePolicies = new Views.View_ManagePolicies();
                m_View_ServerTags=new View_ServerTags();

                // Reports group views
                m_View_Main_Reports = new Views.View_Main_Reports();
                m_View_Report_AuditedServers = new Views.ControlAuditedInstancesReport();
                m_View_Report_Filters = new Views.Report_Filters();
                m_View_Report_GuestEnabledServers = new Views.Report_GuestEnabledDatabases();
                m_View_Report_CrossServerLoginCheck = new Views.Report_CrossServerLoginCheck();
                m_View_Report_MixedModeAuthentication = new Views.Report_MixedModeAuthentication();
                m_View_Report_ServersWithDangerousGroups = new Views.Report_ServersWithDangerousGroups();
                m_View_Report_SystemAdministratorVulnerability = new Views.Report_SystemAdministratorVulnerability();
                m_View_Report_SuspectWindowsAccounts = new Views.Report_SuspectWindowsAccounts();
                m_View_Report_SuspectSqlLogins = new Report_SuspectSqlLogins();
                m_View_Report_VulnerableFixedRoles = new Views.Report_VulnerableFixedRoles();
                m_View_Report_MailVulnerability = new Views.Report_MailVulnerability();
                m_View_Report_LoginVulnerability = new Views.Report_LoginVulnerability();
                m_View_Report_DatabaseChaining = new Views.Report_DatabaseChaining();
                m_View_Report_ServerLoginsAndUserMappings = new Views.Report_ServerLoginsAndUserMappings();
                m_View_Report_SPsWithGuestAccess = new Views.Report_OSVulnerability();
                m_View_Report_UsersPermissions = new Views.Report_UsersPermissions();
                m_View_Report_AllObjectsWithPermissions = new Views.Report_AllObjectsWithPermissions();
                m_View_Report_DatabaseRoles = new Views.Report_DatabaseRoles();
                m_View_Report_ServerRoles = new Views.Report_ServerRoles();
                m_View_Report_ActivityHistory = new Views.Report_ActivityHistory();
                m_View_Report_RiskAssessment = new Views.Report_RiskAssessment();
                m_View_Report_CompareAssessments = new Views.Report_CompareAssessments();
                m_View_Report_CompareSnapshots = new Views.Report_CompareSnapshots();
                m_View_Report_Users = new Views.Report_Users();

                m_CurrentGroup = Utility.ExplorerBarGroup.SecuritySummary;
                m_CurrentViewStack = m_SecuritySummaryViewStack;
            }
        }

        #endregion

        #region Ctors

        public Controller()
        {
            ResetViews();
        }

        #endregion

        #region Properties

        public DateTime ReportTime
        {
            get { return m_MainForm.ReportTime; }
        }

        public Sql.Policy ReportPolicy
        {
            get { return m_MainForm.ReportPolicy; }
        }

        public bool UseReportTimeWithDate
        {
            get { return m_MainForm.UseReportTimeWithDate; }
        }

        public bool ReportUseBaseline
        {
            get { return m_MainForm.ReportUseBaseline; }
        }

        public DateTime? PolicyTime
        {
            get { return m_View_Main_SecuritySummary.SelectionTime; }
        }

        public bool PolicyUseBaselineSnapshots
        {
            get { return m_View_Main_SecuritySummary.UseBaselineSnapshots; }
        }

        public Sql.Repository Repository
        {
            get { return m_Repository; }
            set { m_Repository = value; }
        }
        public Utility.Security Permissions
        {
            get { return m_Permissions; }
            set { m_Permissions = value; }
        }
        public bool isAdmin
        {
            get { return Permissions.isAdmin; }
        }

        public bool isViewer
        {
            get { return Permissions.isViewer; }

        }

        public MainForm Main_Form
        {
            set { m_MainForm = value; }
        }
        public System.Windows.Forms.Control RightPane
        {
            set { m_RightPane = value; }
        }
        public Utility.ExplorerBarGroup CurrentGroup
        {
            get { return m_CurrentGroup; }
        }
        public Utility.MenuConfiguration MenuConfiguration
        {
            get { return m_CurrentMenuConfiguration; }
        }
        public TreeView PolicyTree
        {
            set { m_PolicyTree = value; }
        }
        public TreeView PolicyServerTree
        {
            set { m_PolicyServerTree = value; }
        }
        public TreeView ExplorerTree
        {
            set { m_ExplorerTree = value; }
        }
        public TreeView ReportsTree
        {
            set { m_ReportsTree = value; }
        }
        public Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar ExplorerBar
        {
            set { m_ExplorerBar = value; }
        }
        #endregion

        #region Methods

        public void SetTitle()
        {
            m_MainForm.setTitle();
        }

        public void SetMenuConfiguration(Utility.MenuConfiguration configIn)
        {
            // Set the current menu configuration, and configure the tool bar.
            m_CurrentMenuConfiguration = configIn;
            m_MainForm.ToolBarConfiguration = configIn.ToolStandardItems;
            m_CurrentCommandHandler = null;
        }

        public void SetMenuConfiguration(
                Utility.MenuConfiguration configIn,
                Interfaces.ICommandHandler cmdHandler
            )
        {
            Debug.Assert(cmdHandler != null);

            // Set the current menu configuration, and configure the tool bar.
            m_CurrentMenuConfiguration = configIn;
            m_MainForm.ToolBarConfiguration = configIn.ToolStandardItems;
            m_CurrentCommandHandler = cmdHandler;
        }

        public void ProcessCommand (Utility.ViewSpecificCommand command)
        {
            Debug.Assert(m_CurrentCommandHandler != null);

            if (m_CurrentCommandHandler != null)
            {
                m_CurrentCommandHandler.ProcessCommand(command);
            }
        }

        public void SwitchGroup(Utility.ExplorerBarGroup groupIn)
        {
            if (m_CurrentGroup != groupIn) // If the group is not changing don't do anything.
            {
                m_ExplorerBar.ActiveGroup = m_ExplorerBar.Groups[(int)groupIn];
                m_ExplorerBar.PerformAction(
                    Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarAction.ClickActiveGroup);
            }
        }

        public void SetCurrentGroup(Utility.ExplorerBarGroup groupIn)
        {
            if (m_CurrentGroup != groupIn) // If the group is not changing don't do anything.
            {
                // Set the current group and menu configuration.
                m_CurrentGroup = groupIn;
                m_CurrentViewStack = getGroupStack(m_CurrentGroup);
                m_CurrentMenuConfiguration = getGroupMenuConfiguration(m_CurrentGroup);

                // If the group stack is empty, fill the group
                // default view.
                if(m_CurrentViewStack.Count == 0)
                {
                    m_CurrentViewStack.Push(getGroupDefaultView(m_CurrentGroup));
                }

                // Reconfigure the tool bar.
                m_MainForm.ToolBarConfiguration = m_CurrentMenuConfiguration.ToolStandardItems;

                // Display the view.
                displayCurrentView();
            }
        }

        public void SetCurrentPolicy(Sql.Policy policy)
        {
            m_ExplorerBar.SelectedGroup = m_ExplorerBar.Groups[Utility.Constants.ExplorerBar_GroupKey_Summary];

            // If the policy comes in null then it was deleted and we need to refresh everything
            if (policy == null || !Sql.Policy.IsPolicyRegistered(policy.PolicyId))
            {
                m_RefreshPoliciesEvent(0);
            }
            else
            {
                m_PolicyTree.SelectedNode = m_PolicyTree.Nodes[policy.PolicyName];
            }
        }

        public void SetCurrentPolicyAssessment(Sql.Policy assessment)
        {
            m_ExplorerBar.SelectedGroup = m_ExplorerBar.Groups[Utility.Constants.ExplorerBar_GroupKey_Summary];

            // If the policy comes in null then it was deleted and we need to refresh everything
            if (assessment == null || !Sql.Policy.IsPolicyRegistered(assessment.PolicyId) || !assessment.IsAssessmentFound(assessment.AssessmentId))
            {
                m_RefreshPoliciesEvent(0);
            }
            else
            {
                TreeNode selectedNode = null;
                foreach (TreeNode typenode in m_PolicyTree.Nodes[assessment.PolicyName].Nodes)
                {
                    foreach (TreeNode node in typenode.Nodes)
                    {
                        if (node.Name == assessment.AssessmentName)
                        {
                            selectedNode = node;
                            break;
                        }
                    }

                    if (selectedNode != null)
                    {
                        break;
                    }
                }
                m_PolicyTree.SelectedNode = selectedNode;
            }
        }

        public TreeNode AddAssessmentToPolicy(TreeNode PolicyNode, Sql.Policy Assessment)
        {
            TreeNode assessmentnode = new TreeNode(Assessment.AssessmentName);
            assessmentnode.Name = Assessment.AssessmentName;
            assessmentnode.ImageIndex =
                assessmentnode.SelectedImageIndex = Assessment.FindingIconIndex;
            assessmentnode.Tag = Assessment;

            TreeNode[] nodes = PolicyNode.Nodes.Find(Utility.Policy.AssessmentState.DisplayName(Assessment.AssessmentState), false);
            TreeNode typenode;

            if (nodes.Length > 0)
            {
                typenode = nodes[0];

                //Add the found assessment to the list in order by assessmentid or in the last position
                int nodeIdx = typenode.Nodes.Count;

                foreach (TreeNode node in typenode.Nodes)
                {
                    if (node.Tag != null && node.Tag.GetType() == typeof(Sql.Policy))
                    {
                        // make sure it isn't already in the tree
                        if (((Sql.Policy)node.Tag).AssessmentId == Assessment.AssessmentId)
                        {
                            return node;
                        }
                        else if (((Sql.Policy)node.Tag).AssessmentId < Assessment.AssessmentId)
                        {
                            nodeIdx = node.Index;
                            break;
                        }
                    }
                    else
                    {
                        nodeIdx = node.Index;
                        break;
                    }
                }
                typenode.Nodes.Insert(nodeIdx, assessmentnode);
            }
            else
            {
                typenode = new TreeNode(Utility.Policy.AssessmentState.DisplayName(Assessment.AssessmentState));
                typenode.Name = typenode.Text;
                typenode.ImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.Folder);
                typenode.Nodes.Add(assessmentnode);
                PolicyNode.Nodes.Add(typenode);
            }
            typenode.Expand();

            return assessmentnode;
        }

        public void RefreshPolicyInTree(Sql.Policy policy)
        {
            TreeNode[] nodes = m_PolicyTree.Nodes[0].Nodes.Find(policy.PolicyName, false);
            if (nodes.GetLength(0) == 1)
            {
                if (policy.IsAssessment)
                {
                    foreach(TreeNode typenode in nodes[0].Nodes)
                    {
                        foreach (TreeNode node in typenode.Nodes)
                        {
                            if (node.Tag is Sql.Policy)
                            {
                                if (policy.AssessmentId == ((Sql.Policy)node.Tag).AssessmentId)
                                {
                                    m_MainForm.refreshPolicyTreeNode(node);
                                    return;
                                }
                            }
                        }
                    }
                }
                else
                {
                    m_MainForm.refreshPolicyTreeNode(nodes[0]);
                }
            }
        }

        public void SetCurrentPolicyServer(string server)
        {
            m_ExplorerBar.SelectedGroup = m_ExplorerBar.Groups[Utility.Constants.ExplorerBar_GroupKey_Summary];

            // If the policy comes in null then it was deleted and we need to refresh everything
            if (m_PolicyTree.Nodes[0].Tag is Sql.Policy)
            {
                Sql.Policy policy = (Sql.Policy) m_PolicyTree.Nodes[0].Tag;
                if (policy == null || !Sql.Policy.IsPolicyRegistered(policy.PolicyId))
                {
                    m_RefreshPoliciesEvent(0);
                }
                else
                {
                    m_PolicyServerTree.Focus();
                    m_PolicyServerTree.SelectedNode = m_PolicyServerTree.Nodes[0].Nodes[server];
                }
            }
        }

        public void SetCurrentServer(Sql.RegisteredServer server)
        {
            m_ExplorerBar.SelectedGroup = m_ExplorerBar.Groups[Utility.Constants.ExplorerBar_GroupKey_Explore];

            // If the server comes in null then it was deleted and we need to refresh everything
            if (server == null || ! Sql.RegisteredServer.IsServerRegistered(server.ConnectionName))
            {
                m_RefreshServersEvent(false, string.Empty);
            }
            else
            {
                m_ExplorerTree.SelectedNode = m_ExplorerTree.Nodes[0].Nodes[server.ConnectionName];
            }
        }

        public void SetCurrentSnapshot(Sql.RegisteredServer server, int SnapshotId)
        {
            bool isFound = false;
            m_ExplorerBar.SelectedGroup = m_ExplorerBar.Groups[Utility.Constants.ExplorerBar_GroupKey_Explore];
            if (server == null || !Sql.RegisteredServer.IsServerRegistered(server.ConnectionName))
            {
                m_RefreshServersEvent(false, string.Empty);
            }
            else
            {
                TreeNode serverNode = m_ExplorerTree.Nodes[0].Nodes[server.ConnectionName];
                if (serverNode != null)
                {
                    TreeNode selectedNode = null;
                    foreach (TreeNode node in serverNode.Nodes)
                    {
                        if (node.Tag != null && node.Tag.GetType() == typeof(Sql.Snapshot))
                        {
                            if (((Sql.Snapshot)node.Tag).SnapshotId == SnapshotId)
                            {
                                isFound = true;
                                selectedNode = node;
                                break;
                            }
                        }
                    }
                    // if we didn't find it, then try to add it, or get current on something
                    if (!isFound)
                    {
                        Sql.Snapshot snap = Sql.Snapshot.GetSnapShot(SnapshotId);
                        if (snap == null)
                        {
                            // If we don't find the snapshot, then go back to the server node
                            selectedNode = m_ExplorerTree.Nodes[0].Nodes[server.ConnectionName];
                        }
                        else
                        {
                            TreeNode snapshotNode = AddSnapshotToServer(serverNode, snap);
                            selectedNode = snapshotNode;
                        }
                    }
                    m_ExplorerTree.SelectedNode = selectedNode;
                }
            }
        }

        public void AddSnapshotToServer(String Server, int SnapshotId)
        {

            TreeNode[] nodes = m_ExplorerTree.Nodes[0].Nodes.Find(Server, false);
            Sql.Snapshot snap = Sql.Snapshot.GetSnapShot(SnapshotId);

            if (nodes.GetLength(0) == 1 && snap != null)
            {
                AddSnapshotToServer(nodes[0], Sql.Snapshot.GetSnapShot(SnapshotId));
            }
        }

        public TreeNode AddSnapshotToServer(TreeNode ServerNode, Sql.Snapshot Snapshot)
        {
            TreeNode snapshotnode = new TreeNode(Snapshot.SnapshotName);
            snapshotnode.Name = Snapshot.SnapshotName;
            snapshotnode.ImageIndex =
                snapshotnode.SelectedImageIndex = Snapshot.IconIndex;
            snapshotnode.Tag = Snapshot;

            //Add the found snapshot to the list in order by date or in the last position
            int nodeIdx = ServerNode.Nodes.Count;

            foreach (TreeNode node in ServerNode.Nodes)
            {
                if (node.Tag != null && node.Tag.GetType() == typeof(Sql.Snapshot))
                {
                    // make sure it isn't already in the tree
                    if (((Sql.Snapshot)node.Tag).SnapshotId == Snapshot.SnapshotId)
                    {
                        return node;
                    }
                    else if (((Sql.Snapshot)node.Tag).StartTime < Snapshot.StartTime)
                    {
                        nodeIdx = node.Index;
                        break;
                    }
                }
                else
                {
                    nodeIdx = node.Index;
                    break;
                }
            }
            ServerNode.Nodes.Insert(nodeIdx, snapshotnode);

            return snapshotnode;
        }

        public void RefreshServerInTree(String Server)
        {
            TreeNode[] nodes = m_ExplorerTree.Nodes[0].Nodes.Find(Server, false);
            if (nodes.GetLength(0) == 1)
            {
                m_MainForm.refreshExplorerTreeNode(nodes[0]);
            }
        }


        public void RefreshSnapshot(String Server, int SnapshotId)
        {
            TreeNode[] nodes = m_ExplorerTree.Nodes[0].Nodes.Find(Server, false);
            Sql.Snapshot Snapshot = Sql.Snapshot.GetSnapShot(SnapshotId);

            if (nodes.GetLength(0) == 1 && Snapshot != null)
            {
                foreach (TreeNode snapshotnode in nodes[0].Nodes)
                {
                    if (snapshotnode.Tag != null && snapshotnode.Tag.GetType() == typeof(Sql.Snapshot))
                    {
                        if (((Sql.Snapshot)snapshotnode.Tag).SnapshotId == SnapshotId)
                        {
                            if (((Sql.Snapshot)snapshotnode.Tag).Status != Snapshot.Status)
                            {
                                m_Repository.RefreshRegisteredServers();
                                RefreshServerInTree(Server);
                            }
                            snapshotnode.Text =
                                snapshotnode.Name = Snapshot.SnapshotName;
                            snapshotnode.ImageIndex =
                                snapshotnode.SelectedImageIndex = Snapshot.IconIndex;
                            snapshotnode.Tag = Snapshot;

                            RefreshCurrentView();
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Set the current report in the report tree
        /// </summary>
        /// <param name="report">The name of the node to find in the report tree</param>
        public void SetCurrentReport(string report)
        {
            // set the report tree to the selected report or the root if passed report not found
            if (m_ReportsTree.Nodes.Find(report, true).Length == 1)
            {
                m_ReportsTree.SelectedNode = m_ReportsTree.Nodes.Find(report, true)[0];
            }
            else
            {
                m_ReportsTree.SelectedNode = m_ReportsTree.Nodes[0];
            }
        }

        /// <summary>
        /// Change to the Reports Explorer Group and Show the requested Report
        /// </summary>
        /// <param name="nodeTagIn">The nodeTag used to setup the requested report</param>
        public void ShowReport(Utility.NodeTag nodeTagIn)
        {
            SetRootView(m_ReportsViewStack, nodeTagIn);
            SetCurrentReport(nodeTagIn.DataContext.Name);
            m_ExplorerBar.SelectedGroup = m_ExplorerBar.Groups[(int)Utility.ExplorerBarGroup.Reports];
        }

        public void ShowReportMenu(int policyid, DateTime? runtime, bool usebaseline)
        {
            m_MainForm.SetReportSelection(policyid, runtime, usebaseline);

            SwitchGroup(Utility.ExplorerBarGroup.Reports);
            ShowRootView(new NodeTag(new Data.Main_Reports(string.Empty, Views.View_Main_Reports.Tab.General),
                                                            View.Main_Reports));
        }

        public void RefreshReportsView()
        {
            // This function is basically so mainform can refresh the reports view when it isn't shown after enabling Reporting Services
            // If anything else needs to do this, then this just needs to become an event driven refresh
            refreshView(m_View_Main_Reports);
        }

        public void ShowCompareDialog(int policyId, int assessmentId, Sql.RegisteredServer server)
        {
            Form compare = new Forms.Form_AssessmentComparison(policyId, assessmentId, server);
            compare.ShowDialog();

        }

        public void RefreshCurrentView()
        {
            using (logX.loggerX.DebugCall("Controller RefreshCurrentView"))
            {
                // Make sure there is a current view first.
                if (m_CurrentViewStack.Count != 0)
                {
                    refreshView((Interfaces.IRefresh)getCurrentView());
                }
            }
        }

        public void ShowRootView(Utility.NodeTag nodeTagIn)
        {
            using (logX.loggerX.DebugCall("Controller ShowRootView"))
            {
                // Put the view on top of the stack
                SetRootView(m_CurrentViewStack, nodeTagIn);
                // Display the control stack.
                logX.loggerX.Debug("Call displayCurrentView");
                displayCurrentView();
            }
        }

        public void SetRootView(Utility.ExplorerBarGroup group, Utility.NodeTag nodeTagIn)
        {
            Stack<Control> stack = null;

            if (group == Utility.ExplorerBarGroup.SecuritySummary)
            {
                stack = m_SecuritySummaryViewStack;
            }
            else if (group == Utility.ExplorerBarGroup.ExplorePermissions)
            {
                stack = m_ExplorePermissionsViewStack;
            }
            else if (group == Utility.ExplorerBarGroup.Reports)
            {
                stack = m_ReportsViewStack;
            }
            else if (group == Utility.ExplorerBarGroup.ManageSQLsecure)
            {
                stack = m_ManageSQLsecureViewStack;
            }
            else
            {
                Debug.Assert(false, "Invalid Group Passed to SetRootView");
            }

            SetRootView(stack, nodeTagIn);
        }

        private void SetRootView(Stack<Control> stack, Utility.NodeTag nodeTagIn)
        {
            // Clear the current view stack.
            // NOTE: if the stack is not cleared then it will
            // setup the bread crumbs.  For the root view
            // there should not be any bread crumbs.
            stack.Clear();

            // Get view node based on input node tag,
            // and set the data context of the view.
            Control vw = getView(stack, nodeTagIn);

            // Clear the current control stack and add the new view.
            stack.Push(vw);
        }

        public void ShowChildView(Utility.NodeTag nodeTagIn)
        {
            Debug.Assert(m_CurrentViewStack.Count > 0, "View stack is empty");

            // Get view node based on input node tag,
            // and set the data context of the view.
            Control vw = getView(m_CurrentViewStack, nodeTagIn);

            // Clear the current control stack and add the new view.
            m_CurrentViewStack.Push(vw);

            // Display the control stack.
            displayCurrentView();
        }

        public void ShowCrumbView(int indexIn)
        {
            // Pop the stack till we reach the index.
            int i = m_CurrentViewStack.Count - 1;
            while (i > indexIn)
            {
                m_CurrentViewStack.Pop();
                --i;
            }

            // Display the control stack.
            displayCurrentView();
        }

        public void ShowViewHelp()
        {
            ShowTopic(getCurrentHelpTopic());
        }

        public void ShowViewConcept()
        {
            ShowTopic(getCurrentConceptTopic());
        }

        public void ShowTopic(String topicUrlIn)
        {
            if (topicUrlIn == null || topicUrlIn.Length == 0)
            {
                Utility.Help.ShowTableOfContents(m_MainForm);
            }
            else
            {
                Utility.Help.ShowTopic(m_MainForm, topicUrlIn);
            }
        }
        #endregion

        #region Events/Delegates

        // These events/delegates are used to notify whoever subscribes
        // to event indicating that a policy or server list has changed.
        // Typically the add/remove server task will notify the event that a change
        // has been done to the servers list.   Also typically the main form
        // explorer bar servers tree view will subscribe to this event to 
        // refresh the tree view. The same is true for policies.
        public delegate void RefreshPoliciesEventHandler(int policyId);
        private event RefreshPoliciesEventHandler m_RefreshPoliciesEvent;
        public event RefreshPoliciesEventHandler RefreshPoliciesEvent
        {
            add { m_RefreshPoliciesEvent += value; }
            remove { m_RefreshPoliciesEvent -= value; }
        }
        public void SignalRefreshPoliciesEvent(
                int policyId
            )
        {
            if (m_RefreshPoliciesEvent != null)
            {
                m_RefreshPoliciesEvent(policyId);
            }
        }

        public delegate void RefreshServersEventHandler(bool isAdd, string server);
        private event RefreshServersEventHandler m_RefreshServersEvent;
        public event RefreshServersEventHandler RefreshServersEvent
        {
            add { m_RefreshServersEvent += value; }
            remove { m_RefreshServersEvent -= value; }
        }
        public void SignalRefreshServersEvent(
                bool isAdd,
                string server
            )
        {
            //Debug.Assert(!string.IsNullOrEmpty(server));

            if (m_RefreshServersEvent != null)
            {
                m_RefreshServersEvent(isAdd, server);
            }
        }

        #endregion
    }
}
