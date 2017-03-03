using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

using Wintellect.PowerCollections;

using Idera.SQLsecure.Core.Accounts;
using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Forms;
using Idera.SQLsecure.UI.Console.Controls;
using Idera.SQLsecure.UI.Console.Sql;
using Idera.SQLsecure.UI.Console.Utility;
using Policy = Idera.SQLsecure.UI.Console.Sql.Policy;
using System.IO;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;

namespace Idera.SQLsecure.UI.Console
{
    public partial class MainForm : Form, Interfaces.ICommandHandler
    {
        #region ICommandHandler Members

        void Interfaces.ICommandHandler.ProcessCommand(Utility.ViewSpecificCommand command)
        {
            Sql.RegisteredServer rServer;
            Sql.Policy policy;
            string server;
            switch (_explorerBar.SelectedGroup.Index)
            {
                case (int)Utility.ExplorerBarGroup.SecuritySummary:
                    policy = null;
                    rServer = null;
                    switch (command)
                    {
                        case Utility.ViewSpecificCommand.ObjectPermissions:
                            Debug.Assert(_explorerBar_SecuritySummaryTreeView.SelectedNode != null);

                            showTreePermissions(_explorerBar_SecuritySummaryTreeView.SelectedNode, Views.View_PermissionExplorer.Tab.ObjectPermissions);
                            break;
                        case Utility.ViewSpecificCommand.UserPermissions:
                            Debug.Assert(_explorerBar_SecuritySummaryTreeView.SelectedNode != null);

                            showTreePermissions(_explorerBar_SecuritySummaryTreeView.SelectedNode, Views.View_PermissionExplorer.Tab.UserPermissions);
                            break;
                        case Utility.ViewSpecificCommand.Delete:
                            Debug.Assert(_explorerBar_SecuritySummaryTreeView.SelectedNode != null);
                            Debug.Assert(_explorerBar_SecuritySummaryTreeView.SelectedNode.Tag != null);

                            if (_explorerBar_SecuritySummaryTreeView.SelectedNode.Level == 0)
                            {
                                if (_explorerBar_SecuritySummaryTreeView.SelectedNode.Tag is Sql.Policy)
                                {
                                    policy = ((Sql.Policy)_explorerBar_SecuritySummaryTreeView.SelectedNode.Tag);
                                    if (Sql.Policy.IsPolicyRegistered(policy.PolicyId))
                                    {
                                        if (policy.IsPolicy)
                                        {
                                            if (!policy.IsSystemPolicy)
                                            {
                                                // Display confirmation, if user confirms remove the policy.
                                                string caption = Utility.ErrorMsgs.RemovePolicyCaption + " - " + policy.PolicyName;
                                                if (DialogResult.Yes == Utility.MsgBox.ShowWarningConfirm(caption, Utility.ErrorMsgs.RemovePolicyConfirmMsg))
                                                {
                                                    try
                                                    {
                                                        Sql.Policy.RemovePolicy(policy.PolicyId);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Utility.MsgBox.ShowError(Utility.ErrorMsgs.RemovePolicyCaption,
                                                                                 string.Format(Utility.ErrorMsgs.CantRemovePolicyMsg, policy.PolicyName), ex);
                                                    }

                                                    Program.gController.SignalRefreshPoliciesEvent(0);
                                                }
                                            }
                                            else
                                            {
                                                Utility.MsgBox.ShowError(Utility.ErrorMsgs.RemovePolicyCaption, Utility.ErrorMsgs.RemoveSystemPolicyMsg);
                                            }
                                        }
                                        else if ((policy.IsAssessment))
                                        {
                                            if (!policy.IsApprovedAssessment)
                                            {
                                                // Display confirmation, if user confirms remove the assessment.
                                                string caption = Utility.ErrorMsgs.RemoveAssessmentCaption;
                                                if (DialogResult.Yes == Utility.MsgBox.ShowWarningConfirm(Utility.ErrorMsgs.RemoveAssessmentCaption,
                                                    string.Format(Utility.ErrorMsgs.RemoveAssessmentConfirmMsg, policy.PolicyAssessmentName)))
                                                {
                                                    try
                                                    {
                                                        Sql.Policy.RemoveAssessment(policy.PolicyId, policy.AssessmentId);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Utility.MsgBox.ShowError(Utility.ErrorMsgs.RemoveAssessmentCaption,
                                                                                 string.Format(Utility.ErrorMsgs.CantRemoveAssessmentMsg, policy.PolicyAssessmentName), ex);
                                                    }

                                                    Program.gController.SignalRefreshPoliciesEvent(0);
                                                }
                                            }
                                            else
                                            {
                                                Utility.MsgBox.ShowError(Utility.ErrorMsgs.RemoveAssessmentCaption, Utility.ErrorMsgs.RemoveApprovedAssessmentMsg);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Utility.MsgBox.ShowError(Utility.ErrorMsgs.RemovePolicyCaption, Utility.ErrorMsgs.RemoveSystemPolicyMsg);
                                        Program.gController.SignalRefreshPoliciesEvent(0);
                                    }
                                }
                            }
                            else if (_explorerBar_SecuritySummaryTreeView.SelectedNode.Level == 1)
                            {
                                if (_explorerBar_SecuritySummaryTreeView.SelectedNode.Tag is Sql.RegisteredServer)
                                {
                                    rServer = ((Sql.RegisteredServer)_explorerBar_SecuritySummaryTreeView.SelectedNode.Tag);
                                    if (_explorerBar_SecuritySummaryTreeView.SelectedNode.Parent.Tag is Sql.Policy)
                                    {
                                        policy = (Sql.Policy)_explorerBar_SecuritySummaryTreeView.SelectedNode.Parent.Tag;

                                        if (!policy.IsDynamic)
                                        {
                                            string caption = policy.IsAssessment
                                                                 ? Utility.ErrorMsgs.RemoveServerFromAssessmentCaption
                                                                 : Utility.ErrorMsgs.RemoveServerFromPolicyCaption;
                                            string msg = policy.IsAssessment
                                                                 ? string.Format(Utility.ErrorMsgs.RemoveServerFromAssessmentConfirmMsg, rServer.ConnectionName, policy.PolicyAssessmentName)
                                                                 : string.Format(Utility.ErrorMsgs.RemoveServerFromPolicyConfirmMsg, rServer.ConnectionName, policy.PolicyName);
                                            if (DialogResult.Yes == Utility.MsgBox.ShowWarningConfirm(caption, msg))
                                            {
                                                RegisteredServer.RemoveRegisteredServerFromPolicy(rServer.RegisteredServerId, policy.PolicyId, policy.AssessmentId);
                                                refreshPolicyServerTree();
                                            }
                                        }
                                        else
                                        {
                                            Utility.MsgBox.ShowError(Utility.ErrorMsgs.PolicyCaption, Utility.ErrorMsgs.RemoveDynamicPolicyServerMsg);
                                        }
                                    }
                                }
                            }
                            break;
                        case Utility.ViewSpecificCommand.Refresh:
                            refreshPolicyServerTreeNode(_explorerBar_SecuritySummaryTreeView.SelectedNode);
                            break;
                        case Utility.ViewSpecificCommand.Configure:
                            Debug.Assert(_explorerBar_SecuritySummaryTreeView.SelectedNode != null);
                            Debug.Assert(_explorerBar_SecuritySummaryTreeView.SelectedNode != _explorerBar_SecuritySummaryTreeView.Nodes[0]);

                            if (_explorerBar_SecuritySummaryTreeView.SelectedNode.Tag is Sql.Policy)
                            {
                                policy = ((Sql.Policy)_explorerBar_SecuritySummaryTreeView.SelectedNode.Tag);
                                if (Forms.Form_PolicyProperties.Process(policy.PolicyId, policy.AssessmentId, Program.gController.isAdmin, Forms.Form_PolicyProperties.RequestedOperation.ConfigureMetrics))
                                {
                                    refreshSecuritySummaryGroup();
                                }
                            }
                            else if (_explorerBar_SecuritySummaryTreeView.SelectedNode.Tag is Sql.RegisteredServer)
                            {
                                rServer = ((Sql.RegisteredServer)_explorerBar_SecuritySummaryTreeView.SelectedNode.Tag);
                                Forms.Form_SqlServerProperties.Process(rServer.ConnectionName, Forms.Form_SqlServerProperties.RequestedOperation.EditCofiguration,
                                                                       Program.gController.isAdmin);
                                Program.gController.RefreshCurrentView();
                            }
                            break;
                        case Utility.ViewSpecificCommand.Collect:
                            Debug.Assert(_explorerBar_SecuritySummaryTreeView.SelectedNode != null);

                            if (_explorerBar_SecuritySummaryTreeView.SelectedNode.Tag is Sql.Policy)
                            {
                                rServer = Forms.Form_SelectRegisteredServer.GetServer();
                            }
                            else if (_explorerBar_SecuritySummaryTreeView.SelectedNode.Tag is Sql.RegisteredServer)
                            {
                                rServer = ((Sql.RegisteredServer)_explorerBar_SecuritySummaryTreeView.SelectedNode.Tag);
                            }

                            if (rServer != null)
                            {
                                if (Sql.RegisteredServer.IsServerRegistered(rServer.ConnectionName))
                                {
                                    Forms.Form_StartSnapshotJobAndShowProgress.Process(rServer.ConnectionName);
                                }
                                else
                                {
                                    Utility.MsgBox.ShowError(Utility.ErrorMsgs.SQLsecureDataCollection, Utility.ErrorMsgs.ServerNotRegistered);
                                    Program.gController.SignalRefreshServersEvent(false, string.Empty);
                                }
                            }
                            break;
                        case Utility.ViewSpecificCommand.Properties:
                            Debug.Assert(_explorerBar_SecuritySummaryTreeView.SelectedNode != null);

                            if (_explorerBar_SecuritySummaryTreeView.SelectedNode.Tag is Sql.Policy)
                            {
                                policy = ((Sql.Policy)_explorerBar_SecuritySummaryTreeView.SelectedNode.Tag);
                                if (Forms.Form_PolicyProperties.Process(policy.PolicyId, policy.AssessmentId, Program.gController.isAdmin))
                                {
                                    refreshSecuritySummaryGroup();
                                }
                            }
                            else if (_explorerBar_SecuritySummaryTreeView.SelectedNode.Tag is Sql.RegisteredServer)
                            {
                                rServer = ((Sql.RegisteredServer)_explorerBar_SecuritySummaryTreeView.SelectedNode.Tag);
                                Forms.Form_SqlServerProperties.Process(rServer.ConnectionName, Forms.Form_SqlServerProperties.RequestedOperation.GeneralProperties,
                                                                       Program.gController.isAdmin);
                                Program.gController.RefreshCurrentView();
                            }
                            break;
                        case Utility.ViewSpecificCommand.NewLogin:
                            Forms.Form_WizardNewLogin.Process();
                            break;
                        case Utility.ViewSpecificCommand.NewAuditServer:
                            Forms.Form_WizardRegisterSQLServer.Process();
                            break;
                        default:
                            break;
                    }
                    break;
                case (int)Utility.ExplorerBarGroup.ExplorePermissions:
                    rServer = null;
                    Sql.Snapshot snapshot = null;
                    switch (command)
                    {
                        case Idera.SQLsecure.UI.Console.Utility.ViewSpecificCommand.ObjectPermissions:
                            showTreePermissions(Views.View_PermissionExplorer.Tab.ObjectPermissions);
                            break;
                        case Idera.SQLsecure.UI.Console.Utility.ViewSpecificCommand.UserPermissions:
                            showTreePermissions(Views.View_PermissionExplorer.Tab.UserPermissions);
                            break;
                        case Utility.ViewSpecificCommand.Delete:
                            Debug.Assert(_explorerBar_ExplorePermissionsTreeView.SelectedNode != null);
                            Debug.Assert(_explorerBar_ExplorePermissionsTreeView.SelectedNode != _explorerBar_ExplorePermissionsTreeView.Nodes[0]);
                            Debug.Assert(_explorerBar_ExplorePermissionsTreeView.SelectedNode.Tag != null);

                            if (_explorerBar_ExplorePermissionsTreeView.SelectedNode.Level == 1)
                            {
                                server = ((Sql.RegisteredServer)_explorerBar_ExplorePermissionsTreeView.SelectedNode.Tag).ConnectionName;
                                Forms.Form_RemoveRegisteredServer.Process(server);
                            }
                            else if (_explorerBar_ExplorePermissionsTreeView.SelectedNode.Level == 2)
                            {
                                snapshot = (Sql.Snapshot)_explorerBar_ExplorePermissionsTreeView.SelectedNode.Tag;
                                Sql.Snapshot.SnapshotList snapshotlist = new Sql.Snapshot.SnapshotList();
                                snapshotlist.Add(snapshot);
                                if (Forms.Form_DeleteSnapshot.Process(snapshotlist) == DialogResult.OK)
                                {
                                    refreshExplorerTreeNode(_explorerBar_ExplorePermissionsTreeView.SelectedNode.Parent);
                                }
                            }
                            break;
                        case Utility.ViewSpecificCommand.Properties:
                            Debug.Assert(_explorerBar_ExplorePermissionsTreeView.SelectedNode != null);
                            Debug.Assert(_explorerBar_ExplorePermissionsTreeView.SelectedNode != _explorerBar_ExplorePermissionsTreeView.Nodes[0]);

                            System.Type tagType = _explorerBar_ExplorePermissionsTreeView.SelectedNode.Tag.GetType();
                            if (tagType == typeof(Sql.RegisteredServer))
                            {
                                server = ((Sql.RegisteredServer)_explorerBar_ExplorePermissionsTreeView.SelectedNode.Tag).ConnectionName;
                                Forms.Form_SqlServerProperties.Process(server, Forms.Form_SqlServerProperties.RequestedOperation.GeneralProperties, Program.gController.isAdmin);
                                Program.gController.RefreshCurrentView();
                            }
                            else if (tagType == typeof(Sql.Snapshot))
                            {
                                Forms.Form_SnapshotProperties.Process(new Sql.ObjectTag(((Sql.Snapshot)_explorerBar_ExplorePermissionsTreeView.SelectedNode.Tag).SnapshotId, Sql.ObjectType.TypeEnum.Snapshot));
                            }
                            break;
                        case Utility.ViewSpecificCommand.Refresh:
                            refreshExplorerTreeNode(_explorerBar_ExplorePermissionsTreeView.SelectedNode);
                            break;
                        case Utility.ViewSpecificCommand.Configure:
                            Debug.Assert(_explorerBar_ExplorePermissionsTreeView.SelectedNode != null);
                            Debug.Assert(_explorerBar_ExplorePermissionsTreeView.SelectedNode != _explorerBar_ExplorePermissionsTreeView.Nodes[0]);

                            rServer = ((Sql.RegisteredServer)_explorerBar_ExplorePermissionsTreeView.SelectedNode.Tag);
                            Forms.Form_SqlServerProperties.Process(rServer.ConnectionName, Forms.Form_SqlServerProperties.RequestedOperation.EditCofiguration, Program.gController.isAdmin);
                            Program.gController.RefreshCurrentView();
                            break;
                        case Utility.ViewSpecificCommand.Collect:
                            Debug.Assert(_explorerBar_ExplorePermissionsTreeView.SelectedNode != null);
                            Debug.Assert(_explorerBar_ExplorePermissionsTreeView.SelectedNode != _explorerBar_ExplorePermissionsTreeView.Nodes[0]);

                            rServer = ((Sql.RegisteredServer)_explorerBar_ExplorePermissionsTreeView.SelectedNode.Tag);
                            Forms.Form_StartSnapshotJobAndShowProgress.Process(rServer.ConnectionName);
                            break;
                        case Utility.ViewSpecificCommand.Baseline:
                            Debug.Assert(_explorerBar_ExplorePermissionsTreeView.SelectedNode != null);
                            Debug.Assert(_explorerBar_ExplorePermissionsTreeView.SelectedNode.Tag != null);

                            TreeNode node = _explorerBar_ExplorePermissionsTreeView.SelectedNode;
                            if (node.Level == 2 &&
                                node.Tag != null &&
                                node.Tag.GetType() == typeof(Sql.Snapshot))
                            {
                                if (Forms.Form_BaselineSnapshot.Process((Sql.Snapshot)node.Tag) == DialogResult.OK)
                                {
                                    refreshExplorerTreeNode(node.Parent);
                                }
                            }
                            break;
                        case Utility.ViewSpecificCommand.NewAuditServer:
                            Forms.Form_WizardRegisterSQLServer.Process();
                            break;
                        case Utility.ViewSpecificCommand.NewLogin:
                            Forms.Form_WizardNewLogin.Process();
                            break;
                        default:
                            break;
                    }
                    break;
                case (int)Utility.ExplorerBarGroup.ManageSQLsecure:
                    switch (command)
                    {
                        case Idera.SQLsecure.UI.Console.Utility.ViewSpecificCommand.ObjectPermissions:
                            showPermissions(Views.View_PermissionExplorer.Tab.ObjectPermissions);
                            break;
                        case Idera.SQLsecure.UI.Console.Utility.ViewSpecificCommand.UserPermissions:
                            showPermissions(Views.View_PermissionExplorer.Tab.UserPermissions);
                            break;
                        case Utility.ViewSpecificCommand.Refresh:
                            refreshManageSQLsecureGroup();
                            break;
                        case Utility.ViewSpecificCommand.NewLogin:
                            Forms.Form_WizardNewLogin.Process();
                            refreshManageSQLsecureGroup();
                            break;
                        case Utility.ViewSpecificCommand.NewAuditServer:
                            Forms.Form_WizardRegisterSQLServer.Process();
                            refreshManageSQLsecureGroup();
                            break;
                        default:
                            break;
                    }
                    break;
                case (int)Utility.ExplorerBarGroup.Reports:
                    switch (command)
                    {
                        case Idera.SQLsecure.UI.Console.Utility.ViewSpecificCommand.ObjectPermissions:
                            showPermissions(Views.View_PermissionExplorer.Tab.ObjectPermissions);
                            break;
                        case Idera.SQLsecure.UI.Console.Utility.ViewSpecificCommand.UserPermissions:
                            showPermissions(Views.View_PermissionExplorer.Tab.UserPermissions);
                            break;
                        case Utility.ViewSpecificCommand.Refresh:
                            refreshManageSQLsecureGroup();
                            break;
                        case Utility.ViewSpecificCommand.NewLogin:
                            Forms.Form_WizardNewLogin.Process();
                            refreshManageSQLsecureGroup();
                            break;
                        case Utility.ViewSpecificCommand.NewAuditServer:
                            Forms.Form_WizardRegisterSQLServer.Process();
                            refreshManageSQLsecureGroup();
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region constants

        private static readonly Color MENU_TEXT_COLOR_DROPDOWN = Color.FromArgb(75, 75, 75);
        private static readonly Color MENU_TEXT_COLOR_NORMAL = Color.White;

        #endregion

        #region Ctor

        public MainForm()
        {
            InitializeComponent();

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            // set min size
            this.MinimumSize = this.Size;

            // turn off refresh in default menu
            m_menuConfiguration_Explore.ViewItems[(int)Utility.MenuItems_View.Refresh] = false;

            // File Menu
            _menuStrip_File_Connect.Image = AppIcons.AppImage16(AppIcons.Enum.Connect);
            // deleted - _menuStrip_File_ConnectionProperties.Image = AppIcons.AppImage16(AppIcons.Enum.Properties);
            _menuStrip_File_NewSQLServer.Image = AppIcons.AppImage16(AppIcons.Enum.AuditSQLServer);
            _menuStrip_File_NewLogin.Image = AppIcons.AppImage16(AppIcons.Enum.NewSQLsecureLogin);
            _menuStrip_File_ManageLicense.Image = AppIcons.AppImage16(AppIcons.Enum.ManageLicense);
            _menuStrip_File_ImportSqlServers.Image = AppIcons.AppImage16(AppIcons.Enum.ImportServers);
            //deleted - _menuStrip_File_Print.Image = AppIcons.AppImage16(AppIcons.Enum.Print);
            //_menuStrip_File_Exit.Image = AppIcons.AppImage16(AppIcons.Enum.Exit);         - no image

            // Edit Menu
            _menuStrip_Edit_Remove.Image = AppIcons.AppImage16(AppIcons.Enum.Remove);
            _menuStrip_Edit_ConfigureDataCollection.Image = AppIcons.AppImage16(AppIcons.Enum.ConfigureAuditSettingsSM);
            _menuStrip_Edit_Properties.Image = AppIcons.AppImage16(AppIcons.Enum.Properties);

            // View Menu
            _menuStrip_View_Refresh.Image = AppIcons.AppImage16(AppIcons.Enum.Refresh);
            _menuStrip_View_CollapseAll.Visible = false;        // not currently implemented
            _menuStrip_View_ExpandAll.Visible = false;          // not currently implemented
            _menuStrip_View_Sep1.Visible = false;               // hide divider for not implemented features

            // Permissions Menu
            _menuStrip_Permissions_UserPermissions.Image = AppIcons.AppImage16(AppIcons.Enum.UserPermissions);
            _menuStrip_Permissions_ObjectPermissions.Image = AppIcons.AppImage16(AppIcons.Enum.ObjectExplorer);

            // Snapshots Menu
            _menuStrip_Snapshots_Collect.Image = AppIcons.AppImage16(AppIcons.Enum.CollectDataSnapshot);
            _menuStrip_Snapshots_Baseline.Image = AppIcons.AppImage16(AppIcons.Enum.MarkAsBaseline);
            _menuStrip_Snapshots_GroomingSchedule.Image = AppIcons.AppImage16(AppIcons.Enum.GroomingSchedule);

            // Tools Menu
            loadToolsMenu();

            // Help Menu
            _menuStrip_Help_ThisWindow.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.Help_16;

            // Toolbar
            _toolStrip_NewSQLServer.Image = AppIcons.AppImage16(AppIcons.Enum.AuditSQLServer);
            _toolStrip_NewLogin.Image = AppIcons.AppImage16(AppIcons.Enum.NewSQLsecureLogin);
            // deleted - _toolStrip_Print.Image = AppIcons.AppImage16(AppIcons.Enum.Print);
            _toolStrip_Remove.Image = AppIcons.AppImage16(AppIcons.Enum.Remove);
            _toolStrip_ConfigureDataCollection.Image = AppIcons.AppImage16(AppIcons.Enum.ConfigureAuditSettingsSM);
            _toolStrip_Properties.Image = AppIcons.AppImage16(AppIcons.Enum.Properties);
            _toolStrip_Refresh.Image = AppIcons.AppImage16(AppIcons.Enum.Refresh);
            _toolStrip_UserPermissions.Image = AppIcons.AppImage16(AppIcons.Enum.UserPermissions);
            _toolStrip_ObjectPermissions.Image = AppIcons.AppImage16(AppIcons.Enum.ObjectExplorer);
            _toolStrip_Collect.Image = AppIcons.AppImage16(AppIcons.Enum.CollectDataSnapshot);
            _toolStrip_Baseline.Image = AppIcons.AppImage16(AppIcons.Enum.MarkAsBaseline);
            _toolStrip_Help.Image = AppIcons.AppImage16(AppIcons.Enum.HelpSM);
            _toolStrip_ImportServers.Image = AppIcons.AppImage16(AppIcons.Enum.ImportServers);

            // Explorer Bar
            _explorerBar.Groups[(int)Utility.ExplorerBarGroup.SecuritySummary].Settings.AppearancesLarge.HeaderAppearance.Image =
                    AppIcons.AppImage32(AppIcons.Enum32.SecuritySummary);
            _explorerBar.Groups[(int)Utility.ExplorerBarGroup.SecuritySummary].Settings.AppearancesSmall.HeaderAppearance.Image =
                    AppIcons.AppImage16(AppIcons.Enum.SecuritySummary);
            _explorerBar.Groups[(int)Utility.ExplorerBarGroup.ExplorePermissions].Settings.AppearancesLarge.HeaderAppearance.Image =
                    AppIcons.AppImage32(AppIcons.Enum32.ExplorePermissions);
            _explorerBar.Groups[(int)Utility.ExplorerBarGroup.ExplorePermissions].Settings.AppearancesSmall.HeaderAppearance.Image =
                    AppIcons.AppImage16(AppIcons.Enum.ExplorePermissions);
            _explorerBar.Groups[(int)Utility.ExplorerBarGroup.Reports].Settings.AppearancesLarge.HeaderAppearance.Image =
                    AppIcons.AppImage32(AppIcons.Enum32.Reports);
            _explorerBar.Groups[(int)Utility.ExplorerBarGroup.Reports].Settings.AppearancesSmall.HeaderAppearance.Image =
                    AppIcons.AppImage16(AppIcons.Enum.ReportsSM);
            _explorerBar.Groups[(int)Utility.ExplorerBarGroup.ManageSQLsecure].Settings.AppearancesLarge.HeaderAppearance.Image =
                    AppIcons.AppImage32(AppIcons.Enum32.ManageSqlSecure);
            _explorerBar.Groups[(int)Utility.ExplorerBarGroup.ManageSQLsecure].Settings.AppearancesSmall.HeaderAppearance.Image =
                    AppIcons.AppImage16(AppIcons.Enum.ManageSqlSecure);

            // Explorer Bar Trees (hookup imagelist for indexed selection)
            _explorerBar_SecuritySummaryPolicyTreeView.ImageList =
                _explorerBar_SecuritySummaryTreeView.ImageList =
                _explorerBar_ExplorePermissionsTreeView.ImageList =
                _explorerBar_ReportsTreeView.ImageList =
                _explorerBar_ManageSQLsecureTreeView.ImageList = AppIcons.AppImageList16();

            // These images aren't available except in the global cache, so load them at startup because they are needed in an image list
            //AppIcons.AppImageList16().Images.Add(global::Idera.SQLsecure.UI.Console.Properties.Resources.HighRisk_16);
            //AppIcons.AppImageList16().Images.Add(global::Idera.SQLsecure.UI.Console.Properties.Resources.MediumRisk_16);
            //AppIcons.AppImageList16().Images.Add(global::Idera.SQLsecure.UI.Console.Properties.Resources.LowRisk_16);
            //AppIcons.AppImageList16().Images.Add(global::Idera.SQLsecure.UI.Console.Properties.Resources.HighRiskExplained_16);
            //AppIcons.AppImageList16().Images.Add(global::Idera.SQLsecure.UI.Console.Properties.Resources.MediumRiskExplained_16);
            //AppIcons.AppImageList16().Images.Add(global::Idera.SQLsecure.UI.Console.Properties.Resources.LowRiskExplained_16);
            //AppIcons.AppImageList16().Images.Add(global::Idera.SQLsecure.UI.Console.Properties.Resources.check_16);

            // Context Menu
            _cmsi_Server_auditSQLServer.Image = AppIcons.AppImage16(AppIcons.Enum.AuditSQLServer);
            _cmsi_Server_collectDataSnapshot.Image = AppIcons.AppImage16(AppIcons.Enum.CollectDataSnapshot);
            _cmsi_Server_configureAuditSettings.Image = AppIcons.AppImage16(AppIcons.Enum.ConfigureAuditSettingsSM);
            _cmsi_Server_removeSQLServer.Image = AppIcons.AppImage16(AppIcons.Enum.Remove);
            _cmsi_Server_exploreUserPermissions.Image = AppIcons.AppImage16(AppIcons.Enum.UserPermissions);
            _cmsi_Server_exploreSnapshot.Image = AppIcons.AppImage16(AppIcons.Enum.ObjectExplorer);
            _cmsi_Server_refresh.Image = AppIcons.AppImage16(AppIcons.Enum.Refresh);
            _cmsi_Server_properties.Image = AppIcons.AppImage16(AppIcons.Enum.Properties);

            _checkBox_Report_IncludeTime.Checked = false;
            _dateTimePicker_Reports.Value = new DateTime(DateTime.Now.Year,
                                                            DateTime.Now.Month,
                                                            DateTime.Now.Day);
            _dateTimePicker_Report_Time.Value = new DateTime(DateTime.Now.Year,
                                                            DateTime.Now.Month,
                                                            DateTime.Now.Day,
                                                            23, 59, 59);
            _dateTimePicker_Report_Time.Enabled = false;
            //isRepositoryUpdated();
        }

        #endregion

        #region Fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.MainForm");

        private string m_RepositoryName;
        private bool m_Init;
        private bool m_Initialized = false;         // used to stop multiple refreshes while launching
        private bool m_Refresh = false;
        private string m_UserName;
        private string m_Password;

        private TreeNode m_ClickedNode;             // Used by Context menus to trick node clicks
        private TreeNode m_NodeToProcess;           // Used by Context menus to process the correct node after clicking
        private TreeNode m_PreviousSelectedNode;
        private bool m_ReportNodeShowView = false;
        private bool m_SettingPolicyViewFromTree = false;

        private Utility.MenuConfiguration m_menuConfiguration_Default = new Utility.MenuConfiguration();
        private Utility.MenuConfiguration m_menuConfiguration_Policy = new Utility.MenuConfiguration();
        private Utility.MenuConfiguration m_menuConfiguration_PolicyTree = new Utility.MenuConfiguration();
        private Utility.MenuConfiguration m_menuConfiguration_Explore = new Utility.MenuConfiguration();
        private Utility.MenuConfiguration m_menuConfiguration_Reports = new Utility.MenuConfiguration();
        private Utility.MenuConfiguration m_menuConfiguration_Manage = new Utility.MenuConfiguration();

        private Control m_focused = null; // To prevent focus from switching to the splitters, etc.

        private SQL.ReportsRecord m_ReportsRecord;

        #endregion

        #region Properties

        public DateTime ReportTime
        {
            get
            {
                return (((DateTime)_dateTimePicker_Reports.Value).Date + _dateTimePicker_Report_Time.Value.TimeOfDay).ToUniversalTime();
            }
        }

        public Sql.Policy ReportPolicy
        {
            get
            {
                return (Sql.Policy)_comboBox_Report_Policies.SelectedItem;
            }
        }

        public bool UseReportTimeWithDate
        {
            get { return _checkBox_Report_IncludeTime.Checked; }
        }

        public bool ReportUseBaseline
        {
            get { return _checkBox_Report_BaselineOnly.Checked; }
        }

        public String ResultHeaderTitle
        {
            get { return _resultTitleLabel.Text; }
            set { _resultTitleLabel.Text = value; }
        }

        public Utility.ToolStandardItemsState ToolBarConfiguration
        {
            set
            {
                _toolStrip_NewSQLServer.Enabled = value[(int)Utility.ToolItems_Standard.NewSQLServer] && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.AuditSQLServer);
                _toolStrip_NewLogin.Enabled = value[(int)Utility.ToolItems_Standard.NewLogin] && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.NewLogin);
                // deleted - _toolStrip_Print.Enabled = value[(int)Utility.ToolItems_Standard.Print] && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Print);
                _toolStrip_Remove.Enabled = value[(int)Utility.ToolItems_Standard.Remove] && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Delete);
                _toolStrip_ConfigureDataCollection.Enabled = value[(int)Utility.ToolItems_Standard.ConfigureDataCollection] && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.ConfigureAuditSettings);
                _toolStrip_Refresh.Enabled = value[(int)Utility.ToolItems_Standard.Refresh] && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Refresh);
                _toolStrip_Properties.Enabled = value[(int)Utility.ToolItems_Standard.Properties] && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Properties);
                _toolStrip_UserPermissions.Enabled = value[(int)Utility.ToolItems_Standard.UserPermissions] && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.UserPermissions);
                _toolStrip_ObjectPermissions.Enabled = value[(int)Utility.ToolItems_Standard.ObjectPermissions] && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.ObjectPermissions);
                _toolStrip_Collect.Enabled = value[(int)Utility.ToolItems_Standard.Collect] && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Collect);
                _toolStrip_Baseline.Enabled = value[(int)Utility.ToolItems_Standard.Baseline] && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Baseline);
            }
        }

        public TreeView ServerTree
        {
            get { return _explorerBar_ExplorePermissionsTreeView; }
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

        public void setTitle()
        {
            string connectionInfo;
            if (Program.gController.Repository.IsValid)
            {
                connectionInfo = @"Connected To Repository " + Program.gController.Repository.Instance;
            }
            else
            {
                connectionInfo = @"Not Connected";
            }

            // This is code is temporarily commented out for beta version.
            if (string.IsNullOrEmpty(Utility.Constants.BETA_VERSION))
            {
                if (Program.gController.Repository.IsLicenseTrial())
                {
                    connectionInfo += @"  -  ( Evaluation Version )";
                }
            }
            else
            {
                connectionInfo += Utility.Constants.BETA_VERSION;
            }

            this.Text = string.Format(Utility.Constants.APP_TITLE_STR, connectionInfo);

            string text = string.Format(Utility.Constants.APP_TITLE_STR, Program.gController.Repository.Instance);
            if (text.Length < 64)
            {
                notifyIcon1.Text = text;
            }
            else
            {
                notifyIcon1.Text = text.Substring(0, 61) + "...";
            }
        }

        private void loadToolsMenu()
        {
            Utility.RegisteredToolList companyTools = Utility.ToolFinder.GetRegisteredTools(Utility.Constants.RegKey_IderaProducts);
            foreach (Utility.RegisteredTool tool in companyTools)
            {
                if (tool.IsValid)
                {
                    _menuStrip_Tools_Separator_ReportingServices.Visible = true;

                    ToolStripMenuItem menuEntry = new ToolStripMenuItem(tool.Name, null, new EventHandler(tool.LaunchEvent));
                    _menuStrip_Tools.DropDownItems.Add(menuEntry);
                }
            }
        }

        private void setMenuConfigurationSecurityPolicy()
        {
            // Determine if a server or policy node is selected.
            bool isValid = Program.gController.Repository.IsValid;
            bool isPolicy = false;
            bool isAssessment = false;
            bool canRemovePolicy = false;
            // Get node type that has been selected.
            // There are 3 types of nodes:
            // the policy at level 0
            // the assessment type at level 1 which is not selectable
            // the assessment at level 2
            TreeNode node = _explorerBar_SecuritySummaryPolicyTreeView.SelectedNode;
            if (node != null)
            {
                if (node.Level == 0)
                {
                    Sql.Policy policy = (Sql.Policy)node.Tag;
                    isPolicy = !policy.IsAssessment;
                    canRemovePolicy = !policy.IsSystemPolicy && node.Nodes.Count == 0;
                    isAssessment = policy.IsAssessment;
                }
                else if (node.Level == 2)
                {
                    Sql.Policy policy = (Sql.Policy)node.Tag;
                    isAssessment = policy.IsAssessment;
                    canRemovePolicy = !policy.IsApprovedAssessment;
                }
            }

            // Setup all the File menu items.
            m_menuConfiguration_Policy.FileItems[(int)Utility.MenuItems_File.ConnectionProperties] = isValid;
            m_menuConfiguration_Policy.FileItems[(int)Utility.MenuItems_File.NewSQLServer] = isValid;
            m_menuConfiguration_Policy.FileItems[(int)Utility.MenuItems_File.NewLogin] = isValid;
            m_menuConfiguration_Policy.FileItems[(int)Utility.MenuItems_File.ManageLicense] = isValid;
            m_menuConfiguration_Policy.FileItems[(int)Utility.MenuItems_File.Print] = isValid;

            // Setup all the Edit menu items.
            m_menuConfiguration_Policy.EditItems[(int)Utility.MenuItems_Edit.Remove] = isValid && (isPolicy || isAssessment) && canRemovePolicy;
            m_menuConfiguration_Policy.EditItems[(int)Utility.MenuItems_Edit.ConfigureDataCollection] = false;
            m_menuConfiguration_Policy.EditItems[(int)Utility.MenuItems_Edit.Properties] = isValid && (isPolicy || isAssessment);

            // Setup all the View menu items
            m_menuConfiguration_Policy.ViewItems[(int)Utility.MenuItems_View.CollapseAll] = false;
            m_menuConfiguration_Policy.ViewItems[(int)Utility.MenuItems_View.ExpandAll] = false;
            m_menuConfiguration_Policy.ViewItems[(int)Utility.MenuItems_View.GroupByColumn] = false;
            m_menuConfiguration_Policy.ViewItems[(int)Utility.MenuItems_View.Refresh] = isValid;

            // Setup all the Permissions items.
            m_menuConfiguration_Policy.PermissionsItems[(int)Utility.MenuItems_Permissions.UserPermissions] = isValid;
            m_menuConfiguration_Policy.PermissionsItems[(int)Utility.MenuItems_Permissions.ObjectPermissions] = isValid;

            // Setup all the Snapshot items.
            m_menuConfiguration_Policy.SnapshotItems[(int)Utility.MenuItems_Snapshots.Collect] = isValid;
            m_menuConfiguration_Policy.SnapshotItems[(int)Utility.MenuItems_Snapshots.Baseline] = false;

            Program.gController.SetMenuConfiguration(m_menuConfiguration_Policy, this);
        }

        private void setMenuConfigurationSecurityPolicyServerTree()
        {
            // Determine if a server or policy node is selected.
            bool isValid = Program.gController.Repository.IsValid;
            bool isPolicy = false;
            bool isAssessment = false;
            bool isServer = false;
            bool canExplore = false;
            bool canRemovePolicy = false;
            bool canRemoveServer = false;

            // Get node type that has been selected.
            // There are 2 types of nodes:
            // the policy or assessment at level 0
            // the servers at level 1
            TreeNode node = _explorerBar_SecuritySummaryTreeView.SelectedNode;
            if (node != null)
            {
                if (node.Level == 0)
                {
                    if (node.Tag is Sql.Policy)
                    {
                        Sql.Policy policy = (Sql.Policy)node.Tag;
                        isPolicy = !policy.IsAssessment;
                        canRemovePolicy = !policy.IsSystemPolicy && node.Nodes.Count == 0;
                        isAssessment = policy.IsAssessment;
                    }
                }
                else if (node.Level == 1)
                {
                    if (node.Parent.Tag is Sql.Policy)
                    {
                        Sql.Policy policy = (Sql.Policy)node.Parent.Tag;
                        canRemoveServer = (policy.IsPolicy || (policy.IsAssessment && !policy.IsApprovedAssessment)) && !policy.IsDynamic;
                    }
                    isServer = true;
                    canExplore = true;
                }
            }

            // Setup all the File menu items.
            m_menuConfiguration_PolicyTree.FileItems[(int)Utility.MenuItems_File.ConnectionProperties] = isValid;
            m_menuConfiguration_PolicyTree.FileItems[(int)Utility.MenuItems_File.NewSQLServer] = isValid;
            m_menuConfiguration_PolicyTree.FileItems[(int)Utility.MenuItems_File.NewLogin] = isValid;
            m_menuConfiguration_PolicyTree.FileItems[(int)Utility.MenuItems_File.ManageLicense] = isValid;
            m_menuConfiguration_PolicyTree.FileItems[(int)Utility.MenuItems_File.Print] = isValid;

            // Setup all the Edit menu items.
            m_menuConfiguration_PolicyTree.EditItems[(int)Utility.MenuItems_Edit.Remove] = isValid && ((isServer && canRemoveServer) || ((isPolicy || isAssessment) && canRemovePolicy));
            m_menuConfiguration_PolicyTree.EditItems[(int)Utility.MenuItems_Edit.ConfigureDataCollection] = isValid && isServer;
            m_menuConfiguration_PolicyTree.EditItems[(int)Utility.MenuItems_Edit.Properties] = isValid && (isServer || isPolicy || isAssessment);

            // Setup all the View menu items
            m_menuConfiguration_PolicyTree.ViewItems[(int)Utility.MenuItems_View.CollapseAll] = !isServer;
            m_menuConfiguration_PolicyTree.ViewItems[(int)Utility.MenuItems_View.ExpandAll] = !isServer;
            m_menuConfiguration_PolicyTree.ViewItems[(int)Utility.MenuItems_View.GroupByColumn] = false;
            m_menuConfiguration_PolicyTree.ViewItems[(int)Utility.MenuItems_View.Refresh] = isValid;

            // Setup all the Permissions items.
            m_menuConfiguration_PolicyTree.PermissionsItems[(int)Utility.MenuItems_Permissions.UserPermissions] = isValid && canExplore;
            m_menuConfiguration_PolicyTree.PermissionsItems[(int)Utility.MenuItems_Permissions.ObjectPermissions] = isValid && canExplore;

            // Setup all the Snapshot items.
            m_menuConfiguration_PolicyTree.SnapshotItems[(int)Utility.MenuItems_Snapshots.Collect] = isValid;
            m_menuConfiguration_PolicyTree.SnapshotItems[(int)Utility.MenuItems_Snapshots.Baseline] = false;

            Program.gController.SetMenuConfiguration(m_menuConfiguration_PolicyTree, this);
        }

        private void setMenuConfigurationExplorePermissions()
        {
            // Determine if a server node is selected, the selected node is not null
            // and the selected node has a parent.   The root node Audited SQL Servers does
            // not have a parent node.
            bool isValid = Program.gController.Repository.IsValid;
            bool isServer = false;
            bool isSnapshot = false;
            bool canExplore = false;
            // Get node type that has been selected.
            // There are 4 types of nodes:
            // the root at level 0
            // the servers at level 1
            // the snapshots at level 2
            // the more snapshots link at level 2
            TreeNode node = _explorerBar_ExplorePermissionsTreeView.SelectedNode;
            if (node != null)
            {
                if (node.Level == 0)
                {
                    canExplore = isValid;     // explorer will ask for server
                }
                else if (node.Level == 1)
                {
                    isServer = true;
                    canExplore = false;     // don't allow exploring until a valid snapshot is found
                    foreach (TreeNode snapnode in node.Nodes)
                    {
                        if (snapnode.Tag != null &&
                            snapnode.Tag.GetType() == typeof(Sql.Snapshot))
                        {
                            // there is a valid snapshot, so allow exploring
                            canExplore = ((Sql.Snapshot)snapnode.Tag).HasValidPermissions;
                        }
                        if (canExplore)
                        {
                            // a valid snapshot exists, so no more searching is needed
                            break;
                        }
                    }
                }
                else if (node.Level == 2)
                {
                    if (node.Tag != null)   // more link will have no tag
                    {
                        isSnapshot = true;
                        if (node.Tag.GetType() == typeof(Sql.Snapshot))
                        {
                            canExplore = ((Sql.Snapshot)node.Tag).HasValidPermissions;
                        }
                    }

                }
            }

            // Setup all the File menu items.
            m_menuConfiguration_Explore.FileItems[(int)Utility.MenuItems_File.ConnectionProperties] = isValid;
            m_menuConfiguration_Explore.FileItems[(int)Utility.MenuItems_File.NewSQLServer] = isValid;
            m_menuConfiguration_Explore.FileItems[(int)Utility.MenuItems_File.NewLogin] = isValid;
            m_menuConfiguration_Explore.FileItems[(int)Utility.MenuItems_File.ManageLicense] = isValid;
            m_menuConfiguration_Explore.FileItems[(int)Utility.MenuItems_File.Print] = isValid;

            // Setup all the Edit menu items.
            m_menuConfiguration_Explore.EditItems[(int)Utility.MenuItems_Edit.Remove] = isServer || isSnapshot;
            m_menuConfiguration_Explore.EditItems[(int)Utility.MenuItems_Edit.ConfigureDataCollection] = isServer;
            m_menuConfiguration_Explore.EditItems[(int)Utility.MenuItems_Edit.Properties] = isServer || isSnapshot;

            // Setup all the View menu items
            m_menuConfiguration_Explore.ViewItems[(int)Utility.MenuItems_View.CollapseAll] = !isServer;
            m_menuConfiguration_Explore.ViewItems[(int)Utility.MenuItems_View.ExpandAll] = !isServer;
            m_menuConfiguration_Explore.ViewItems[(int)Utility.MenuItems_View.GroupByColumn] = false;
            m_menuConfiguration_Explore.ViewItems[(int)Utility.MenuItems_View.Refresh] = isValid;

            // Setup all the Permissions items.
            m_menuConfiguration_Explore.PermissionsItems[(int)Utility.MenuItems_Permissions.UserPermissions] = canExplore;
            m_menuConfiguration_Explore.PermissionsItems[(int)Utility.MenuItems_Permissions.ObjectPermissions] = canExplore;

            // Setup all the Snapshot items.
            m_menuConfiguration_Explore.SnapshotItems[(int)Utility.MenuItems_Snapshots.Collect] = isServer;
            m_menuConfiguration_Explore.SnapshotItems[(int)Utility.MenuItems_Snapshots.Baseline] = isSnapshot && canExplore;

            // Set menu configuration.
            Program.gController.SetMenuConfiguration(m_menuConfiguration_Explore, this);
        }

        private void setMenuConfigurationReports()
        {
            Program.gController.SetMenuConfiguration(m_menuConfiguration_Reports, this);
        }

        private void setMenuConfigurationManage()
        {
            Program.gController.SetMenuConfiguration(m_menuConfiguration_Manage, this);
        }

        private void showPermissions(Views.View_PermissionExplorer.Tab tabIn)
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

        private void showTreePermissions(Views.View_PermissionExplorer.Tab showTab)
        {
            //Get the server from the tree
            TreeNode node = _explorerBar_ExplorePermissionsTreeView.SelectedNode;

            showTreePermissions(node, showTab);
        }

        private void showTreePermissions(TreeNode node, Views.View_PermissionExplorer.Tab showTab)
        {
            Sql.RegisteredServer server = null;
            Sql.Snapshot snapshot = null;
            int snapshotId = 0;

            if (node != null)
            {
                if (node.Level == 1)
                {
                    server = (Sql.RegisteredServer)node.Tag;
                    if (_explorerBar.SelectedGroup.Index == (int)Utility.ExplorerBarGroup.SecuritySummary)
                    {
                        snapshotId = Sql.RegisteredServer.GetSnapshotIdByDate(server.ConnectionName,
                                                                                Program.gController.PolicyTime,
                                                                                Program.gController.PolicyUseBaselineSnapshots);
                    }
                    else if (node.TreeView == _explorerBar_ExplorePermissionsTreeView)
                    {
                        // if the server was selected, then use the most current valid snapshot
                        foreach (TreeNode snapnode in node.Nodes)
                        {
                            if (snapnode.Tag != null &&
                                snapnode.Tag.GetType() == typeof(Sql.Snapshot))
                            {
                                snapshot = (Sql.Snapshot)snapnode.Tag;
                                if (snapshot.HasValidPermissions)
                                {
                                    snapshotId = snapshot.SnapshotId;
                                    // this is a valid snapshot, so stop processing now
                                    break;
                                }
                                else
                                {
                                    // this is not a valid snapshot, so clear it and continue
                                    snapshot = null;
                                }
                            }
                        }
                    }
                }
                else if (node.Level == 2)
                {
                    if (node.Tag != null &&
                       node.Tag.GetType() == typeof(Sql.Snapshot))
                    {
                        server = (Sql.RegisteredServer)node.Parent.Tag;
                        snapshot = (Sql.Snapshot)node.Tag;
                        snapshotId = snapshot.SnapshotId;
                    }
                }

                if (server == null)
                {
                    //if it wasn't a server, then ask the user to select a server
                    showPermissions(showTab);
                }
                else
                {
                    if (Sql.RegisteredServer.IsServerRegistered(server.ConnectionName))
                    {
                        if (snapshotId == 0)
                        {
                            //if a server was selected, then show the view
                            Program.gController.ShowRootView(new Utility.NodeTag(new Data.PermissionExplorer(server, showTab),
                                                                                 Utility.View.PermissionExplorer));
                            Program.gController.SetCurrentServer(server);
                        }
                        else
                        {
                            //if a snapshot was selected, then show the view on the snapshot
                            Program.gController.ShowRootView(new Utility.NodeTag(new Data.PermissionExplorer(server, snapshotId, showTab),
                                                                                 Utility.View.PermissionExplorer));
                            Program.gController.SetCurrentSnapshot(server, snapshotId);
                        }
                    }
                    else
                    {
                        Utility.MsgBox.ShowError(Utility.ErrorMsgs.UserPermissionsCaption, Utility.ErrorMsgs.ServerNotRegistered);
                        Program.gController.SignalRefreshServersEvent(false, string.Empty);
                    }
                }
            }
        }

        #region Explorer Bar

        private Utility.ExplorerBarGroup explorerBarGroupKeyToEnum(
                String keyIn
            )
        {
            Debug.Assert(keyIn != null && keyIn.Length != 0, "Invalid Explorer Bar group key input");

            Utility.ExplorerBarGroup ret = Utility.ExplorerBarGroup.SecuritySummary;
            if (String.Compare(keyIn, Utility.Constants.ExplorerBar_GroupKey_Summary, true) == 0) // Security Summary
            {
                ret = Utility.ExplorerBarGroup.SecuritySummary;
            }
            else if (String.Compare(keyIn, Utility.Constants.ExplorerBar_GroupKey_Explore, true) == 0) // Explore Permissions
            {
                ret = Utility.ExplorerBarGroup.ExplorePermissions;
            }
            else if (String.Compare(keyIn, Utility.Constants.ExplorerBar_GroupKey_Reports, true) == 0) // Reports
            {
                ret = Utility.ExplorerBarGroup.Reports;
            }
            else if (String.Compare(keyIn, Utility.Constants.ExplorerBar_GroupKey_Manage, true) == 0) // Manage SQLsecure
            {
                ret = Utility.ExplorerBarGroup.ManageSQLsecure;
            }
            else
            {
                Debug.Assert(false, "Not a valid Explorer Bar group key");
            }

            return ret;
        }

        private String explorerBarGroupEnumToKey(
                Utility.ExplorerBarGroup enumIn
            )
        {
            String ret = null;
            switch (enumIn)
            {
                case Utility.ExplorerBarGroup.SecuritySummary:
                    ret = Utility.Constants.ExplorerBar_GroupKey_Summary;
                    break;

                case Utility.ExplorerBarGroup.ExplorePermissions:
                    ret = Utility.Constants.ExplorerBar_GroupKey_Explore;
                    break;

                case Utility.ExplorerBarGroup.Reports:
                    ret = Utility.Constants.ExplorerBar_GroupKey_Reports;
                    break;

                case Utility.ExplorerBarGroup.ManageSQLsecure:
                    ret = Utility.Constants.ExplorerBar_GroupKey_Manage;
                    break;

                default:
                    Debug.Assert(false, "Not a valid Explorer Bar group enum");
                    break;
            }

            return ret;
        }
        #endregion

        public void SetReportSelection(int policyid, DateTime? runtime, bool usebaseline)
        {
            DateTime time_default = new DateTime(DateTime.Now.Year,
                                                 DateTime.Now.Month,
                                                 DateTime.Now.Day,
                                                 23, 59, 59);
            foreach (Sql.Policy policy in _comboBox_Report_Policies.Items)
            {
                if (policy.PolicyId == policyid)
                {
                    _comboBox_Report_Policies.SelectedItem = policy;
                    break;
                }
            }
            if (runtime.HasValue)
            {
                _dateTimePicker_Reports.Value = runtime.Value.Date;
                _dateTimePicker_Report_Time.Value = runtime.Value;
                _checkBox_Report_IncludeTime.Checked =
                    _dateTimePicker_Report_Time.Enabled = (_dateTimePicker_Report_Time.Value.TimeOfDay.Ticks != time_default.TimeOfDay.Ticks);
            }
            else
            {
                _dateTimePicker_Reports.Value = DateTime.Now.Date;
                _checkBox_Report_IncludeTime.Checked = false;
                _dateTimePicker_Report_Time.Value = time_default;
                _dateTimePicker_Report_Time.Enabled = false;
            }
        }

        #endregion

        #region UserData save/load methods

        private void initFromUserData()
        {
            using (logX.loggerX.DebugCall("initFromUserData"))
            {
                Utility.UserData.MainFormData mfd = Utility.UserData.Current.MainForm;

                // Set size, location and start position.
                this.Size = mfd.Size;
                this.Location = mfd.Location;
                this.StartPosition = mfd.StartPosition;

                // Set splitter, menu & tool locations.
                this._splitContainer.SplitterDistance = mfd.SplitterDistance;
                this._menuStrip.Location = mfd.MenuStripLocation;
                this._toolStrip.Location = mfd.ToolStripLocation;

                // Hide or show task pane.
                showHideTaskPane(Utility.UserData.Current.View.TaskPanelVisible);

                // Set the window state
                logX.loggerX.Debug("Set the window state");
                this.WindowState = mfd.WindowState;
            }
        }
        private void saveToUserData()
        {
            Utility.UserData.MainFormData mfd = Utility.UserData.Current.MainForm;

            // Save the window state, if its not normal
            // then make it normal before saving size, location & start position.
            mfd.WindowState = this.WindowState;
            if (this.WindowState != FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Normal;
            }

            // Get the splitter, menu bar & tool bar locations.
            mfd.SplitterDistance = this._splitContainer.SplitterDistance;
            mfd.MenuStripLocation = this._menuStrip.Location;
            mfd.ToolStripLocation = this._toolStrip.Location;

            // Save size location and start position.
            mfd.Size = this.Size;
            mfd.Location = this.Location;
            mfd.StartPosition = this.StartPosition;
        }
        #endregion

        #region Show Hide Task Pane Handling

        private void showHideTaskPane(bool showIn)
        {
            if (showIn)
            {
                _resultShowHideButton.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.TaskHide_161;
                _resultShowHideButton.Text = _resultShowHideButton.ToolTipText = Utility.Constants.HideTasks;
            }
            else
            {
                _resultShowHideButton.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.TaskShow_16;
                _resultShowHideButton.Text = _resultShowHideButton.ToolTipText = Utility.Constants.ShowTasks;
            }
            Utility.UserData.Current.View.TaskPanelVisible = showIn;
        }

        private void _ShowHideButton_Click(object sender, EventArgs e)
        {
            showHideTaskPane(!Utility.UserData.Current.View.TaskPanelVisible);
        }

        public bool ShowHideButtonVisible
        {
            get { return _resultShowHideButton.Visible; }
            set { _resultShowHideButton.Visible = value; }
        }

        #endregion

        #region Repository Connection

        public static string Server_Name;
        public static string UserName = null;
        public static string Password = null;
        public static bool isConnect = true;
        public static int typeOfAuthentication;
        public static int typeOfServer;
        //Show a pop up dialog to get the server name
        private bool promptForConnection(bool isConnect1 = true)
        {
            bool bConnected = false;
            bool bConnecting = true;
            isConnect = isConnect1;
            // Remember the currently connected server, and prompt
            // user for new server.
            string currentServer = Program.gController.Repository.Instance;
            while (bConnecting)
            {
                // Create and show the select server dialog 
                // If the user has clicked OK, then attempt to connect to the new server.
                // Else go back to the current server if it exists.
                Form_ConnectRepository dlg = new Form_ConnectRepository(isConnect);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    // If connect to server fails, then prompt for the server name again.
                    this.Cursor = Cursors.WaitCursor;

                    if (isConnect)
                    {
                        //isRepositoryUpdated();
                        if (connectToServer(dlg.Server))
                        {
                                bConnected = true;
                                bConnecting = false;
                                #region SQLSecure3.1 - (Mitul Kapoor)Perform action based on user select of Create Repository/Deploy Repository

                                // If the server has changed reset the views
                                bool isServerChanged = false;
                                if (string.Compare(dlg.Server, currentServer, true) != 0)
                                {
                                    Program.gController.ResetViews();
                                    isServerChanged = true;
                                }

                                // Refresh explorer bar.
                                refreshExplorerBar(isServerChanged); // if server has changed then it will go to explore permissions
                                                                     // else it will stay on the current view if valid
                                #endregion
                            
                        }
                    }
                    //SQLSecure 3.1 (Mitul Kapoor) - functionality for "Deploy Repository". 
                    else
                    {
                        isRepositoryUpdated();
                        //Add functionality to perform action to be performed when "Deploy Repository" is selected.
                        if (connectToServer(dlg.Server))
                        {
                            bConnected = true;
                            bConnecting = false;
                            #region SQLSecure3.1 - (Mitul Kapoor)Perform action based on user select of Create Repository/Deploy Repository

                            // If the server has changed reset the views
                            bool isServerChanged = false;
                            if (string.Compare(dlg.Server, currentServer, true) != 0)
                            {
                                Program.gController.ResetViews();
                                isServerChanged = true;
                            }

                            // Refresh explorer bar.
                            refreshExplorerBar(isServerChanged); // if server has changed then it will go to explore permissions
                                                                 // else it will stay on the current view if valid.
                        }
                        else
                        {
                            MsgBox.ShowError(Utility.ErrorMsgs.CantConnectRepository, Utility.ErrorMsgs.FailedToConnectMsg);
                            return false;
                        }                       
                    }
                    #endregion
                            
                    this.Cursor = Cursors.Default;
                }
                else
                {
                    // If current server exists then reconnect to the current server.
                    if (!string.IsNullOrEmpty(currentServer))
                    {
                        this.Cursor = Cursors.WaitCursor;
                        connectToServer(currentServer);
                        refreshExplorerBar(false); // keep the existing view current
                        this.Cursor = Cursors.Default;
                    }
                    bConnecting = false;
                }
            }

            // If we are connected, check product license.
            if (bConnected)
            {
                CheckProductLicense();
                // If the license isn't valid, then it will exit the application, so stop processing
                if (!Program.gController.Repository.IsLicenseOk())
                {
                    return false;
                }

                // If no servers registered then Welcome users
                if (Program.gController.Repository.RegisteredServers.Count == 0)
                {
                    Form_Welcome.Process();
                }

                // Verify all Registered Servers have Credentials
                if (!Repository.checkAllCredentialsEntered(Program.gController.Repository.ConnectionString))
                {
                    Form_GetMissingCredentials.Process();
                }
            }
            return bConnected;
        }
        
        //SQLSecure3.1 - (Mitul Kapoor) - check if the repository is in latest version. if not ask user and upgrade.
        bool isRepositoryUpdated()
        {
            try
            {
                int m_SchemaVersion = 0;  
                //retrieve information from the database to check for repository version.
                m_SchemaVersion = Program.gController.Repository.getRepositoryVersion(Server_Name,UserName,Password,typeOfServer,typeOfAuthentication);
                if(m_SchemaVersion == 0)
                {
                    DeployRepositoryScripts(UserName, Password, false);
                    return true;
                }
                if (m_SchemaVersion < Utility.Constants.SchemaVersion)
                {
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show(string.Format(Utility.ErrorMsgs.UpgradeRepository,m_SchemaVersion, Utility.Constants.SchemaVersion), Utility.ErrorMsgs.UpgradeRepositoryTag,buttons,MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        DeployRepositoryScripts(UserName, Password, true);                        
                    }
                    else
                    {
                        this.Close();
                    }
                    return false;
                }
                if (m_SchemaVersion == Utility.Constants.SchemaVersion)
                {
                    MessageBox.Show(Utility.ErrorMsgs.RepositoryExists,Utility.ErrorMsgs.RepositoryExistTag,MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                    return true;
                }
            }catch(Exception e)
            {
                return false;
            }
            return true;
        }

        //SQLSecure3.1 - (Mitul Kapoor) - execute the deploy repository EXE.
        void DeployRepositoryScripts(string Username,string Password, bool isUpgradeRequested = false)
        {
            string executingExe = "DeployRepository.exe";
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo();
            p.StartInfo.FileName = string.Format("{0}\\{1}", GetAssemblyPath, executingExe);
            if(String.IsNullOrEmpty(UserName) || String.IsNullOrEmpty(Password))
            {
                p.StartInfo.Arguments = Server_Name + " " + isUpgradeRequested;
            }else
            {
                //verify credentials here for remote machine.if incorrect return an error
                if(!VerifyCredentials(Server_Name, Username, Password))
                {
                    MsgBox.ShowError(Utility.ErrorMsgs.IncorrectCredentialsTag,Utility.ErrorMsgs.IncorrectCredentials);
                    return;
                }
                if(String.IsNullOrEmpty(Username) || String.IsNullOrEmpty(Password))
                {
                    p.StartInfo.Arguments = string.Format("{0} {1}",Server_Name,isUpgradeRequested);
                }else
                {
                    p.StartInfo.Arguments = string.Format("{0} {1} {2} {3}",Server_Name,isUpgradeRequested,Username,Password);
                }
                
            }
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            p.Start();
            p.WaitForExit();
            switch (p.ExitCode)
            {
                case (int)Utility.Constants.ExitCode.Success: MsgBox.ShowInfo(Utility.ErrorMsgs.SuccessTag, Utility.ErrorMsgs.RepositorySuccessfullyDeployed);break;
                case (int)Utility.Constants.ExitCode.ScriptNotExist: MsgBox.ShowInfo(Utility.ErrorMsgs.FailTag, Utility.ErrorMsgs.ScriptNotExist); break;
                case (int)Utility.Constants.ExitCode.ScriptFailure: MsgBox.ShowInfo(Utility.ErrorMsgs.FailTag,Utility.ErrorMsgs.ScriptExecutionFailed); break;
            }
        }

        //SQLSecure3.1 - (Mitul Kapoor) - verify the credentials for SQL authentication / azure VM 
        public static bool VerifyCredentials(string ServerName,string UserName,string Password)
        {
            string connectionString = @"Data Source=" + ServerName + ";Initial Catalog=master ;User ID= " + UserName + ";Password=" + Password + ";";
            bool retCode = true;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                }
            }catch(Exception e)
            {
                retCode = false;
            }
            return retCode;
        }

        //SQLSecure3.1 - (Mitul Kapoor) - get the assembly path of the application
        private string GetAssemblyPath
        {
            get
            {
                string codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return System.IO.Path.GetDirectoryName(path);
            }
        }

        private bool connectToServer(string server)
        {
            bool isConnected = Program.gController.Repository.Connect(server,UserName,Password);
            setTitle();
            return isConnected;
        }

        #endregion

        #region Event Handlers

        #region Load and Close

        private void MainForm_Load(object sender, EventArgs e)
        {
            using (logX.loggerX.DebugCall("MainForm_Load"))
            {
                Cursor = Cursors.WaitCursor;

                // Register elements with the controller.
                Program.gController.Main_Form = this;
                Program.gController.ExplorerBar = _explorerBar;
                Program.gController.PolicyTree = _explorerBar_SecuritySummaryPolicyTreeView;
                Program.gController.PolicyServerTree = _explorerBar_SecuritySummaryTreeView;
                Program.gController.ExplorerTree = _explorerBar_ExplorePermissionsTreeView;
                Program.gController.ReportsTree = _explorerBar_ReportsTreeView;
                Program.gController.RightPane = _resultPane;

                // Register for getting notification on interesting events.
                Program.gController.RefreshPoliciesEvent +=
                    new Controller.RefreshPoliciesEventHandler(gController_RefreshPoliciesEvent);
                Program.gController.RefreshServersEvent +=
                    new Controller.RefreshServersEventHandler(gController_RefreshServersEvent);

                // Init explorer bar & menu.
                m_Init = true;
                initExplorerBar();
                initMenus();

                // Init from user data this has to be done after the menus & explorer
                // bar are initialized.   Otherwise it leads to weird behavior.
                initFromUserData();
            }
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            using (logX.loggerX.DebugCall("MainForm_Activated"))
            {
                // Close the splash screen.
                // Moved outside Initialize to guarantee splash screen is closed, in case first attempt fails
                // ex. splashscreen isn't loaded before this check is done the first time.
                if (Program.splashScreen != null)
                {
                    logX.loggerX.Debug("Close splash screen");
                    Program.splashScreen.ForceClose();
                    Program.splashScreen = null;
                    this.Activate();
                }

                if (m_Init)
                {
                    m_Init = false;     // set false here, so welcome/license/etc. dialogs don't cause reinitializing when closing and mainform gets active again

                    // Get the repository name to start with based on last use
                    // then the default from the registry (done in UserData)
                    m_RepositoryName = Utility.UserData.Current.RepositoryInfo.ServerName;
                    m_UserName = Utility.UserData.Current.RepositoryInfo.UserName;
                    m_Password = Utility.UserData.Current.RepositoryInfo.Password;
                    bool isRepositoryValid = m_RepositoryName.Length != 0;

                    // If repository is specified, make sure its valid.
                    logX.loggerX.Debug("Check isRepositoryValid");
                    if (isRepositoryValid)
                    {
                        // Initialize and validate the repository.
                        logX.loggerX.Debug("Create repository object");
                        Program.gController.Repository = new Sql.Repository(m_RepositoryName, m_UserName, m_Password);
                        isRepositoryValid = Program.gController.Repository.IsConnectionValid;
                    }

                    // If the repository is not specified, prompt the user.
                    if (!isRepositoryValid)
                    {
                        promptForConnection();
                    }
                    else
                    {
                        // If repository is specified and valid, set the title
                        // and do a license check.
                        CheckProductLicense();
                        // If the license isn't valid, then it will exit the application, so stop processing
                        if (!Program.gController.Repository.IsLicenseOk())
                        {
                            return;
                        }

                        // If no servers registered then Welcome users
                        if (Program.gController.Repository.RegisteredServers.Count == 0)
                        {
                            Form_Welcome.Process();
                            //                        Form_GettingStarted.Process();                        
                        }

                        // Verify all Registered Servers have Credentials
                        if (!Repository.checkAllCredentialsEntered(Program.gController.Repository.ConnectionString))
                        {
                            Form_GetMissingCredentials.Process();
                        }
                    }

                    // Refresh the explorer bar based and reset the cursor.
                    setTitle();
                    logX.loggerX.Debug("Call refreshExplorerBar");
                    refreshExplorerBar(true);
                    //if (Program.gController.CurrentGroup == ExplorerBarGroup.SecuritySummary)
                    //{
                    //    logX.loggerX.Debug("Call refreshSecuritySummaryGroup");
                    //    refreshSecuritySummaryGroup();
                    //}
                    //else
                    //{
                    //    logX.loggerX.Debug("Call RefreshCurrentView");
                    //    Program.gController.RefreshCurrentView();
                    //}
                    m_Initialized = true;

                    Cursor = Cursors.Default;
                }
            }
        }

        private void CheckProductLicense()
        {
            if (!Program.gController.Repository.IsLicenseOk())
            {
                Form_AddLicense form = new Form_AddLicense(Program.gController.Repository.bbsProductLicense);
                form.ShowDialog();
                if (!Program.gController.Repository.IsLicenseOk())
                {
                    Utility.MsgBox.ShowError(Utility.ErrorMsgs.LicenseCaption, Utility.ErrorMsgs.LicenseNoValidLicense);
                    Utility.UserData.Current.RepositoryInfo.ServerName = string.Empty;
                    Application.Exit();
                    return;
                }
                Program.gController.RefreshCurrentView();
            }

            WarnForExpiringLicenses();
            WarnForTooManyRegisteredServers();
        }

        private void WarnForTooManyRegisteredServers()
        {
            if (!Program.gController.Repository.bbsProductLicense.IsLicneseGoodForServerCount(Program.gController.Repository.RegisteredServers.Count))
            {
                string message = Utility.ErrorMsgs.LicenseTooManyRegisteredServers;
                Utility.MsgBox.ShowWarning(Utility.ErrorMsgs.LicenseCaption, message.ToString());
            }
        }

        private void WarnForExpiringLicenses()
        {
            List<BBSProductLicense.LicenseData> licenses = Program.gController.Repository.bbsProductLicense.Licenses;
            StringBuilder message = new StringBuilder();
            foreach (BBSProductLicense.LicenseData licData in licenses)
            {
                if (licData.isAboutToExpire)
                {
                    if (message.Length > 0)
                    {
                        message.AppendFormat("\n");
                    }
                    message.AppendFormat(Utility.ErrorMsgs.LicenseExpiring, licData.key, licData.daysToExpireStr);
                }
            }
            if (message.Length > 0)
            {
                Utility.MsgBox.ShowWarning(Utility.ErrorMsgs.LicenseCaption, message.ToString());
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            notifyIcon1.Dispose();
            // Save the settings to user data.
            saveToUserData();
        }

        #endregion

        #region Menu Bar Handlers

        private void initMenus()
        {

            this._menuStrip_File_Connect.ToolTipText = Utility.Constants.Menu_Descr_File_Connect;
            
            // deleted - this._menuStrip_File_ConnectionProperties.ToolTipText = Utility.Constants.Menu_Descr_File_ConnectionProperties;
            this._menuStrip_File_NewSQLServer.ToolTipText = Utility.Constants.Menu_Descr_File_NewSQLServer;
            this._menuStrip_File_NewLogin.ToolTipText = Utility.Constants.Menu_Descr_File_NewLogin;
            this._menuStrip_File_ManageLicense.ToolTipText = Utility.Constants.Menu_Descr_File_License;

            //this._menuStrip_Edit_Remove.ToolTipText = Utility.Constants.Menu_Descr_Edit_Remove;
            //this._menuStrip_Edit_ConfigureDataCollection.ToolTipText = Utility.Constants.Menu_Descr_Edit_Configure;
            //this._menuStrip_Edit_Properties.ToolTipText = Utility.Constants.Menu_Descr_Edit_Properties;

            //this._menuStrip_View_Tasks.ToolTipText = Utility.Constants.Menu_Descr_View_Tasks;
            //this._menuStrip_View_ConsoleTree.ToolTipText = Utility.Constants.Menu_Descr_View_ConsoleTree;
            //this._menuStrip_View_Toolbar.ToolTipText = Utility.Constants.Menu_Descr_View_Toolbar;

            //this._menuStrip_Permissions_UserPermissions.ToolTipText = Utility.Constants.Menu_Descr_Permissions_User;
            //this._menuStrip_Permissions_ObjectPermissions.ToolTipText = Utility.Constants.Menu_Descr_Permissions_Object;

            //this._menuStrip_Snapshots_Collect.ToolTipText = Utility.Constants.Menu_Descr_Snapshots_Collect;
            //this._menuStrip_Snapshots_Baseline.ToolTipText = Utility.Constants.Menu_Descr_Snapshots_Baseline;
            //this._menuStrip_Snapshots_CheckIntegrity.ToolTipText = Utility.Constants.Menu_Descr_Snapshots_CheckIntegrity;
        }

        private void _menuStrip_MenuActivate(object sender, EventArgs e)
        {
            if (!(this._explorerBar.ContainsFocus || this._resultPane.ContainsFocus))
            {
                // No valid control has focus, so use the default menu until one is selected
                Program.gController.SetMenuConfiguration(m_menuConfiguration_Default);
            }
        }

        private void menu_DropDownClosed(object sender, EventArgs e)
        {
            ((ToolStripMenuItem)sender).ForeColor = MENU_TEXT_COLOR_NORMAL;
        }

        #region File

        private void _menuStrip_File_DropDownOpening(object sender, EventArgs e)
        {
            ((ToolStripMenuItem)sender).ForeColor = MENU_TEXT_COLOR_DROPDOWN;

            // Connect, Connection Properties and Exit are never disabled.
            // Get the file menu configuration and configure self.
            Utility.FileItemsState fi = Program.gController.MenuConfiguration.FileItems;
            _menuStrip_File_NewSQLServer.Enabled = fi[(int)Utility.MenuItems_File.NewSQLServer];
            _menuStrip_File_NewLogin.Enabled = fi[(int)Utility.MenuItems_File.NewLogin];
            _menuStrip_File_ManageLicense.Enabled = fi[(int)Utility.MenuItems_File.ManageLicense];
            // deleted - _menuStrip_File_Print.Enabled = fi[(int)Utility.MenuItems_File.Print];
        }

        private void _menuStrip_File_Connect_Click(object sender, EventArgs e)
        {
            //Check for user option to connect/deploy repository.
            Cursor = Cursors.WaitCursor;
            promptForConnection(true);
            Cursor = Cursors.Default;
        }

        private void _menuStrip_File_ConnectionProperties_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Form_ConnectionProperties dlg = new Form_ConnectionProperties();
            dlg.ShowDialog();

            Cursor = Cursors.Default;
        }

        private void _menuStrip_File_NewSQLServer_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Program.gController.ProcessCommand(Utility.ViewSpecificCommand.NewAuditServer);

            Cursor = Cursors.Default;
        }

        private void _menuStrip_File_NewLogin_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Program.gController.ProcessCommand(Utility.ViewSpecificCommand.NewLogin);

            Cursor = Cursors.Default;
        }

        private void _menuStrip_File_ManageLicense_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            Program.gController.Repository.ResetLicense();
            Form_License dlg = new Form_License(Program.gController.Repository.bbsProductLicense);
            dlg.ShowDialog();
            setTitle();
            Program.gController.RefreshCurrentView();
            Cursor = Cursors.Default;
        }

        private void _menuStrip_File_Print_Click(object sender, EventArgs e)
        {

        }

        private void _menuStrip_File_Exit_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Close();

            Cursor = Cursors.Default;
        }

        #endregion

        #region Edit

        private void _menuStrip_Edit_DropDownOpening(object sender, EventArgs e)
        {
            ((ToolStripMenuItem)sender).ForeColor = MENU_TEXT_COLOR_DROPDOWN;

            // Get the edit menu configuration and configure self.
            Utility.EditItemsState ei = Program.gController.MenuConfiguration.EditItems;
            _menuStrip_Edit_Remove.Enabled = ei[(int)Utility.MenuItems_Edit.Remove];
            _menuStrip_Edit_ConfigureDataCollection.Enabled = ei[(int)Utility.MenuItems_Edit.ConfigureDataCollection];
            _menuStrip_Edit_Properties.Enabled = ei[(int)Utility.MenuItems_Edit.Properties];
        }

        private void _menuStrip_Edit_Remove_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Program.gController.ProcessCommand(Utility.ViewSpecificCommand.Delete);

            Cursor = Cursors.Default;
        }

        private void _menuStrip_Edit_ConfigureDataCollection_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Program.gController.ProcessCommand(Utility.ViewSpecificCommand.Configure);

            Cursor = Cursors.Default;
        }

        private void _menuStrip_Edit_Properties_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Program.gController.ProcessCommand(Utility.ViewSpecificCommand.Properties);

            Cursor = Cursors.Default;
        }

        #endregion

        #region View

        private void _menuStrip_View_DropDownOpening(object sender, EventArgs e)
        {
            ((ToolStripMenuItem)sender).ForeColor = MENU_TEXT_COLOR_DROPDOWN;

            // Tasks, ConsoleTree, Toolbar and Refresh are never changed.
            // Get the view menu configuration and configure self.
            Utility.ViewItemsState vi = Program.gController.MenuConfiguration.ViewItems;
            if (Utility.UserData.Current.View.TaskPanelVisible)
            {
                _menuStrip_View_Tasks.Checked = true;
            }
            else
            {
                _menuStrip_View_Tasks.Checked = false;
            }
            if (_toolStrip.Visible)
            {
                _menuStrip_View_Toolbar.Checked = true;
            }
            else
            {
                _menuStrip_View_Toolbar.Checked = false;
            }
            if (_splitContainer.Panel1Collapsed)
            {
                _menuStrip_View_ConsoleTree.Checked = false;
            }
            else
            {
                _menuStrip_View_ConsoleTree.Checked = true;
            }
            _menuStrip_View_CollapseAll.Enabled = vi[(int)Utility.MenuItems_View.CollapseAll];
            _menuStrip_View_ExpandAll.Enabled = vi[(int)Utility.MenuItems_View.ExpandAll];
            _menuStrip_View_Refresh.Enabled = vi[(int)Utility.MenuItems_View.Refresh];
        }

        private void _menuStrip_View_Tasks_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showHideTaskPane(!Utility.UserData.Current.View.TaskPanelVisible);

            Cursor = Cursors.Default;
        }

        private void _menuStrip_View_ConsoleTree_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            _splitContainer.Panel1Collapsed = !_splitContainer.Panel1Collapsed;

            Cursor = Cursors.Default;
        }

        private void _menuStrip_View_Toolbar_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            _toolStrip.Visible = !_toolStrip.Visible;

            Cursor = Cursors.Default;
        }

        private void _menuStrip_View_CollapseAll_Click(object sender, EventArgs e)
        {

        }

        private void _menuStrip_View_ExpandAll_Click(object sender, EventArgs e)
        {

        }

        private void _menuStrip_View_GroupByColumn_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Program.gController.ProcessCommand(Utility.ViewSpecificCommand.GroupByBox);

            Cursor = Cursors.Default;
        }

        private void _menuStrip_View_Refresh_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Program.gController.ProcessCommand(Utility.ViewSpecificCommand.Refresh);

            Cursor = Cursors.Default;
        }

        #endregion

        #region Permissions

        private void _menuStrip_Permissions_DropDownOpening(object sender, EventArgs e)
        {
            ((ToolStripMenuItem)sender).ForeColor = MENU_TEXT_COLOR_DROPDOWN;

            // Get the permissions menu configuration and configure self.
            Utility.PermissionsItemsState pi = Program.gController.MenuConfiguration.PermissionsItems;
            _menuStrip_Permissions_UserPermissions.Enabled = pi[(int)Utility.MenuItems_Permissions.UserPermissions];
            _menuStrip_Permissions_ObjectPermissions.Enabled = pi[(int)Utility.MenuItems_Permissions.ObjectPermissions];
        }

        private void _menuStrip_Permissions_UserPermissions_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Program.gController.ProcessCommand(Utility.ViewSpecificCommand.UserPermissions);

            Cursor = Cursors.Default;
        }

        private void _menuStrip_Permissions_ObjectPermissions_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Program.gController.ProcessCommand(Utility.ViewSpecificCommand.ObjectPermissions);

            Cursor = Cursors.Default;
        }

        #endregion

        #region Snapshots

        private void _menuStrip_Snapshots_DropDownOpening(object sender, EventArgs e)
        {
            ((ToolStripMenuItem)sender).ForeColor = MENU_TEXT_COLOR_DROPDOWN;

            // Get the snapshots menu configuration and configure self.
            Utility.SnapshotsItemsState si = Program.gController.MenuConfiguration.SnapshotItems;
            _menuStrip_Snapshots_Collect.Enabled = si[(int)Utility.MenuItems_Snapshots.Collect];
            _menuStrip_Snapshots_Baseline.Enabled = si[(int)Utility.MenuItems_Snapshots.Baseline];
            _menuStrip_Snapshots_GroomingSchedule.Enabled = si[(int)Utility.MenuItems_Snapshots.GroomingSchedule];
        }

        private void _menuStrip_Snapshots_Collect_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Program.gController.ProcessCommand(Utility.ViewSpecificCommand.Collect);

            Cursor = Cursors.Default;
        }

        private void _menuStrip_Snapshots_Baseline_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Program.gController.ProcessCommand(Utility.ViewSpecificCommand.Baseline);

            Cursor = Cursors.Default;
        }

        private void _menuStrip_Snapshots_GroomingSchedule_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Form_GroomingSchedule.Process();

            Cursor = Cursors.Default;
        }

        #endregion

        #region Tools

        private void _menuStrip_Tools_DropDownOpening(object sender, EventArgs e)
        {
            ((ToolStripMenuItem)sender).ForeColor = MENU_TEXT_COLOR_DROPDOWN;

            //SQLsecure 3.1 (Tushar)--Disabling the drop down items if repository connection is not valid.
            this._menuStrip_Tools_ReportingServices.Enabled = Program.gController.Repository.IsValid;
            this.configureSMPTEmaiToolStripMenuItem.Enabled = Program.gController.Repository.IsValid;
            this.configureWeakPasswordDetectionToolStripMenuItem.Enabled = Program.gController.Repository.IsValid;
        }

        // Note: This menu is partially dynamic and other items will be built and handled at run time
        private void _menuStrip_Tools_ReportingServices_DropDownOpening(object sender, EventArgs e)
        {
            bool installed = false;

            // Check if reporting services is installed and, if not, disable launch reports
            try
            {
                // the record will be saved if found for use by the menu items
                m_ReportsRecord = new SQL.ReportsRecord();
                m_ReportsRecord.Read();
                if (m_ReportsRecord.ReportsDeployed)
                {
                    installed = true;
                }
            }
            catch
            {
                installed = false;
            }

            _menuStrip_Tools_ReportingServices_Configure.Enabled = Program.gController.isAdmin;
            _menuStrip_Tools_ReportingServices_Launch.Enabled = installed && Program.gController.isViewer;
        }

        private void _menuStrip_Tools_ReportingServices_Launch_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            try
            {
                string url = m_ReportsRecord.GetReportManagerUrl(true);
                if (url != string.Empty)
                {
                    System.Diagnostics.Process.Start(url);
                }
            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.SQLsecureDataCollection, Utility.ErrorMsgs.CantRunReportingServices, ex);
            }

            Cursor = Cursors.Default;
        }

        private void _menuStrip_Tools_ReportingServices_Configure_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            using (Form_WizardDeployReports frm = new Form_WizardDeployReports())
            {
                frm.ShowDialog(this);
            }

            Program.gController.RefreshReportsView();

            Cursor = Cursors.Default;
        }

        #endregion

        #region Help

        // Help items are never hidden they are always shown.
        private void _menuStrip_Help_DropDownOpening(object sender, EventArgs e)
        {
            ((ToolStripMenuItem)sender).ForeColor = MENU_TEXT_COLOR_DROPDOWN;
        }

        private void _helpMI_ThisWindow_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Program.gController.ShowViewHelp();

            Cursor = Cursors.Default;
        }

        private void _menuStrip_Help_GettingStarted_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Form_GettingStarted.Process();
            if (Program.gController.CurrentGroup == ExplorerBarGroup.SecuritySummary)
            {
                refreshSecuritySummaryGroup();
            }
            else
            {
                Program.gController.RefreshCurrentView();
            }

            Cursor = Cursors.Default;
        }

        private void _menuStrip_Help_HowDoI_AssessSecurity_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Program.gController.ShowTopic(Utility.Help.HowDoIAsssessSecurity);

            Cursor = Cursors.Default;
        }

        private void _menuStrip_Help_HowDoI_explorePermissions_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Program.gController.ShowTopic(Utility.Help.HowDoIExplorePermissionsHelpTopic);

            Cursor = Cursors.Default;
        }

        private void _menuStrip_Help_HowDoI_generateReports_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Program.gController.ShowTopic(Utility.Help.HowDoIGenerateReportsHelpTopic);

            Cursor = Cursors.Default;
        }

        private void _menuStrip_Help_HowDoI_manageSQLsecure_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Program.gController.ShowTopic(Utility.Help.HowDoIManageSQLsecureHelpTopic);

            Cursor = Cursors.Default;
        }

        private void _helpMI_ContactTS_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Process.Start(Utility.Help.SupportHomePage);

            Cursor = Cursors.Default;
        }
        private void _menuStrip_Help_CheckUpdates_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            string URL = string.Format(Utility.Help.CheckUpdates, Utility.Help.productID, Utility.Help.productVersion);
            Process.Start(URL);

            Cursor = Cursors.Default;
        }
        private void _menuStrip_Help_SearchKB_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Process.Start(Utility.Help.KnowledgeBaseHomePage);

            Cursor = Cursors.Default;
        }
        private void _menuStrip_Help_AboutIderaPproducts_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Process.Start(Utility.Help.IderaProducts);

            Cursor = Cursors.Default;
        }

        private void _helpMI_About_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Process.Start(Utility.Help.ABOUT_IDERA);

            Cursor = Cursors.Default;
        }

        #endregion

        #endregion

        #region Tool Bar Handlers
        private void _toolStrip_NewSQLServer_Click(object sender, EventArgs e)
        {
            _menuStrip_File_NewSQLServer_Click(sender, e);
        }

        private void _toolStrip_NewLogin_Click(object sender, EventArgs e)
        {
            _menuStrip_File_NewLogin_Click(sender, e);
        }

        private void _toolStrip_Print_Click(object sender, EventArgs e)
        {
            _menuStrip_File_Print_Click(sender, e);
        }

        private void _toolStrip_Remove_Click(object sender, EventArgs e)
        {
            _menuStrip_Edit_Remove_Click(sender, e);
        }

        private void _toolStrip_ConfigureDataCollection_Click(object sender, EventArgs e)
        {
            _menuStrip_Edit_ConfigureDataCollection_Click(sender, e);
        }

        private void _toolStrip_Properties_Click(object sender, EventArgs e)
        {
            _menuStrip_Edit_Properties_Click(sender, e);
        }

        private void _toolStrip_Refresh_Click(object sender, EventArgs e)
        {
            _menuStrip_View_Refresh_Click(sender, e);
        }

        private void _toolStrip_UserPermissions_Click(object sender, EventArgs e)
        {
            _menuStrip_Permissions_UserPermissions_Click(sender, e);
        }

        private void _toolStrip_ObjectPermissions_Click(object sender, EventArgs e)
        {
            _menuStrip_Permissions_ObjectPermissions_Click(sender, e);
        }

        private void _toolStrip_Collect_Click(object sender, EventArgs e)
        {
            _menuStrip_Snapshots_Collect_Click(sender, e);
        }

        private void _toolStrip_Baseline_Click(object sender, EventArgs e)
        {
            _menuStrip_Snapshots_Baseline_Click(sender, e);
        }

        private void _toolStrip_Help_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Program.gController.ShowViewHelp();

            Cursor = Cursors.Default;
        }

        #endregion

        #region Explorer Bar Handlers

        private void _explorerBar_SelectedGroupChanging(object sender, Infragistics.Win.UltraWinExplorerBar.CancelableGroupEventArgs e)
        {
            // Display the current group view.
            Program.gController.SetCurrentGroup(explorerBarGroupKeyToEnum(e.Group.Key));
            // Don't refresh reports because this could be costly for the user
            if (explorerBarGroupKeyToEnum(e.Group.Key) != Utility.ExplorerBarGroup.Reports)
            {
                Program.gController.RefreshCurrentView();
            }
        }

        private void _explorerBar_SelectedGroupChanged(object sender, Infragistics.Win.UltraWinExplorerBar.GroupEventArgs e)
        {
            e.Group.Container.Controls[0].Focus();
        }

        private void initExplorerBar()
        {
            using (logX.loggerX.DebugCall("initExplorerBar"))
            {
                Debug.Assert(_explorerBar_SecuritySummaryPolicyTreeView.Nodes.Count == 0);
                Debug.Assert(_explorerBar_ExplorePermissionsTreeView.Nodes.Count == 0);
                Debug.Assert(_explorerBar_ReportsTreeView.Nodes.Count == 0);
                Debug.Assert(_explorerBar_ManageSQLsecureTreeView.Nodes.Count == 0);

                // Create the Security Summary policy and server root nodes
                initSecuritySummaryGroup();

                // Create the Explore Permissions root node
                initExplorePermissionsGroup();

                // Initialize the builtin reports tree
                initReportsGroup();

                // Create the management root and function nodes.
                initManageSQLsecureGroup();
            }
        }

        private void initSecuritySummaryGroup()
        {
            using (logX.loggerX.DebugCall("initSecuritySummaryGroup"))
            {
                // Create the policy tree root node.
                TreeNode sRootNode = new TreeNode(Utility.Constants.RootNode_Policy);
                _explorerBar_SecuritySummaryPolicyTreeView.Nodes.Add(sRootNode);

                // do not create an initial server node until there is a policy
                //// Create the policy server tree root node.
                //sRootNode = new TreeNode(Utility.Constants.RootNode_Summary);
                //sRootNode.Tag = new Utility.NodeTag(new Data.Main_SecuritySummary(),
                //                                                Utility.View.Main_SecuritySummary);
                //_explorerBar_SecuritySummaryTreeView.Nodes.Add(sRootNode);
            }
        }

        private void initExplorePermissionsGroup()
        {
            // Create the explore permissions root node.
            TreeNode eRootNode = new TreeNode(Utility.Constants.RootNode_Explore);
            eRootNode.ImageIndex = eRootNode.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.SQLsecure);
            eRootNode.Tag = new Utility.NodeTag(new Data.Main_ExplorePermissions(Utility.Constants.RootNode_Explore),
                                                            Utility.View.Main_ExplorePermissions);
            _explorerBar_ExplorePermissionsTreeView.Nodes.Add(eRootNode);
        }

        private void initReportsGroup()
        {
            // Begin update
            _explorerBar_ReportsTreeView.BeginUpdate();

            // Clear the full tree.
            _explorerBar_ReportsTreeView.Nodes.Clear();

            // Create the builtin reports root node
            TreeNode rRootNode = new TreeNode(Utility.Constants.RootNode_Reports);
            rRootNode.Name = Utility.Constants.RootNode_Reports;
            rRootNode.ImageIndex = rRootNode.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.Folder);
            rRootNode.Tag = new Utility.NodeTag(new Data.Main_Reports(Utility.Constants.RootNode_Reports, Views.View_Main_Reports.Tab.None),
                                                            Utility.View.Main_Reports);
            _explorerBar_ReportsTreeView.Nodes.Add(rRootNode);

            // 1. General Category Reports
            // Create the builtin reports Category "General" node
            TreeNode CategoryNode = new TreeNode(Utility.Constants.ReportsNode_Category_General);
            CategoryNode.Name = Utility.Constants.ReportsNode_Category_General;
            CategoryNode.ImageIndex = CategoryNode.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.Folder);
            CategoryNode.Tag = new Utility.NodeTag(new Data.Main_Reports(Utility.Constants.ReportsNode_Category_General, Views.View_Main_Reports.Tab.General),
                                                            Utility.View.Main_Reports);
            rRootNode.Nodes.Add(CategoryNode);

            // Add the builtin report node Audited Servers
            TreeNode node = new TreeNode(Utility.Constants.ReportNode_AuditedServers);
            node.Name = Utility.Constants.ReportNode_AuditedServers;
            node.Tag = new Utility.NodeTag(new Data.Report(node.Name),
                                            Utility.View.Report_AuditedServers);
            node.ImageIndex = node.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.Report_AuditedSQServers);
            CategoryNode.Nodes.Add(node);

            // Add the builtin report node Server Access
            node = new TreeNode(Utility.Constants.ReportNode_CrossServerLoginCheck);
            node.Name = Utility.Constants.ReportNode_CrossServerLoginCheck;
            node.Tag = new Utility.NodeTag(new Data.Report(node.Name),
                                            Utility.View.Report_CrossServerLoginCheck);
            node.ImageIndex = node.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.Report_CrossServerLoginCheck);
            CategoryNode.Nodes.Add(node);

            // Add the builtin report node Filters
            node = new TreeNode(Utility.Constants.ReportTitle_Filters);
            node.Name = Utility.Constants.ReportTitle_Filters;
            node.Tag = new Utility.NodeTag(new Data.Report(node.Name),
                                            Utility.View.Report_Filters);
            node.ImageIndex = node.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.Report_DataCollectionFilters);
            CategoryNode.Nodes.Add(node);

            // Add the builtin report node Activity History
            node = new TreeNode(Utility.Constants.ReportTitle_ActivityHistory);
            node.Name = Utility.Constants.ReportTitle_ActivityHistory;
            node.Tag = new Utility.NodeTag(new Data.Report(node.Name),
                                            Utility.View.Report_ActivityHistory);
            node.ImageIndex = node.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.Report_ActivityHistory);
            CategoryNode.Nodes.Add(node);

            // Add the builtin report node Risk Assessment
            node = new TreeNode(Utility.Constants.ReportTitle_RiskAssessment);
            node.Name = Utility.Constants.ReportTitle_RiskAssessment;
            node.Tag = new Utility.NodeTag(new Data.Report(node.Name),
                                            Utility.View.Report_RiskAssessment);
            node.ImageIndex = node.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.Report_RiskAssessment);
            CategoryNode.Nodes.Add(node);

            // Add the builtin report node SQLsecure Users
            node = new TreeNode(Utility.Constants.ReportTitle_Users);
            node.Name = Utility.Constants.ReportTitle_Users;
            node.Tag = new Utility.NodeTag(new Data.Report(node.Name),
                                            Utility.View.Report_Users);
            node.ImageIndex = node.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.Report_Users);
            CategoryNode.Nodes.Add(node);

            // 2. Entitlement Category Reports
            // Create the builtin reports Category "Entitlement" node
            CategoryNode = new TreeNode(Utility.Constants.ReportsNode_Category_Entitlement);
            CategoryNode.Name = Utility.Constants.ReportsNode_Category_Entitlement;
            CategoryNode.ImageIndex = CategoryNode.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.Folder);
            CategoryNode.Tag = new Utility.NodeTag(new Data.Main_Reports(Utility.Constants.ReportsNode_Category_Entitlement, Views.View_Main_Reports.Tab.Entitlement),
                                                            Utility.View.Main_Reports);
            rRootNode.Nodes.Add(CategoryNode);

            // Add the builtin report node Orphaned Logins
            node = new TreeNode(Utility.Constants.ReportNode_SuspectWindowsAccounts);
            node.Name = Utility.Constants.ReportNode_SuspectWindowsAccounts;
            node.Tag = new Utility.NodeTag(new Data.Report(node.Name),
                                            Utility.View.Report_SuspectWindowsAccounts);
            node.ImageIndex = node.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.Report_SuspectWindowsAccounts);
            CategoryNode.Nodes.Add(node);

            // Add the builtin report node sql logins
            node = new TreeNode(Utility.Constants.ReportNode_SuspectSqlLogins);
            node.Name = Utility.Constants.ReportNode_SuspectSqlLogins;
            node.Tag = new Utility.NodeTag(new Data.Report(node.Name),
                                            Utility.View.Report_SuspectSqlLogins);
            node.ImageIndex = node.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.Report_SuspectSqlLogins);
            CategoryNode.Nodes.Add(node);

            // Add the builtin report node Server Database Users
            node = new TreeNode(Utility.Constants.ReportNode_ServerLoginsAndUserMappings);
            node.Name = Utility.Constants.ReportNode_ServerLoginsAndUserMappings;
            node.Tag = new Utility.NodeTag(new Data.Report(node.Name),
                                            Utility.View.Report_ServerLoginsAndUserMappings);
            node.ImageIndex = node.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.Report_ServerLogins);
            CategoryNode.Nodes.Add(node);

            // Add the builtin report node Users Permissions
            node = new TreeNode(Utility.Constants.ReportNode_UserPermissions);
            node.Name = Utility.Constants.ReportNode_UserPermissions;
            node.Tag = new Utility.NodeTag(new Data.Report(node.Name),
                                            Utility.View.Report_UsersPermissions);
            node.ImageIndex = node.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.Report_UserPermissions);
            CategoryNode.Nodes.Add(node);

            // Add the builtin report node All Objects With Permissions
            node = new TreeNode(Utility.Constants.ReportNode_AllObjectsWithPermissions);
            node.Name = Utility.Constants.ReportNode_AllObjectsWithPermissions;
            node.Tag = new Utility.NodeTag(new Data.Report(node.Name),
                                            Utility.View.Report_AllObjectsWithPermissions);
            node.ImageIndex = node.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.Report_AllObjectsWithPermissions);
            CategoryNode.Nodes.Add(node);

            // Add the builtin report node Server Roles
            node = new TreeNode(Utility.Constants.ReportNode_ServerRoles);
            node.Name = Utility.Constants.ReportNode_ServerRoles;
            node.Tag = new Utility.NodeTag(new Data.Report(node.Name),
                                            Utility.View.Report_ServerRoles);
            node.ImageIndex = node.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.Report_ServerRoles);
            CategoryNode.Nodes.Add(node);

            // Add the builtin report node Database Roles
            node = new TreeNode(Utility.Constants.ReportNode_DatabaseRoles);
            node.Name = Utility.Constants.ReportNode_DatabaseRoles;
            node.Tag = new Utility.NodeTag(new Data.Report(node.Name),
                                            Utility.View.Report_DatabaseRoles);
            node.ImageIndex = node.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.Report_DatabaseRoles);
            CategoryNode.Nodes.Add(node);

            // 3. Vulnerability Category Reports
            // Create the builtin reports Category "Vulnerability" node
            CategoryNode = new TreeNode(Utility.Constants.ReportsNode_Category_Vulnerability);
            CategoryNode.Name = Utility.Constants.ReportsNode_Category_Vulnerability;
            CategoryNode.ImageIndex = CategoryNode.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.Folder);
            CategoryNode.Tag = new Utility.NodeTag(new Data.Main_Reports(Utility.Constants.ReportsNode_Category_Vulnerability, Views.View_Main_Reports.Tab.Vulnerability),
                                                            Utility.View.Main_Reports);
            rRootNode.Nodes.Add(CategoryNode);

            // Add the builtin report node Windows Authentication
            node = new TreeNode(Utility.Constants.ReportNode_MixedModeAuthentication);
            node.Name = Utility.Constants.ReportNode_MixedModeAuthentication;
            node.Tag = new Utility.NodeTag(new Data.Report(node.Name),
                                            Utility.View.Report_MixedModeAuthentication);
            node.ImageIndex = node.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.Report_MixedModeAuth);
            CategoryNode.Nodes.Add(node);

            // Add the builtin report node Guest Enabled Servers
            node = new TreeNode(Utility.Constants.ReportNode_GuestEnabledServers);
            node.Name = Utility.Constants.ReportNode_GuestEnabledServers;
            node.Tag = new Utility.NodeTag(new Data.Report(node.Name),
                                            Utility.View.Report_GuestEnabledDatabases);
            node.ImageIndex = node.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.Report_GuestEnabledDatabases);
            CategoryNode.Nodes.Add(node);

            // Add the builtin report node OS Vulnerability
            node = new TreeNode(Utility.Constants.ReportNode_OSVulnerability);
            node.Name = Utility.Constants.ReportNode_OSVulnerability;
            node.Tag = new Utility.NodeTag(new Data.Report(node.Name),
                                            Utility.View.Report_OSVulnerability);
            node.ImageIndex = node.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.Report_OSVulnerabitlityViaXP);
            CategoryNode.Nodes.Add(node);

            // Add the builtin report node Fixed Roles assigned to public
            node = new TreeNode(Utility.Constants.ReportNode_VulnerableFixedRoles);
            node.Name = Utility.Constants.ReportNode_VulnerableFixedRoles;
            node.Tag = new Utility.NodeTag(new Data.Report(node.Name),
                                            Utility.View.Report_VulnerableFixedRoles);
            node.ImageIndex = node.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.Report_VulnerableFixedRoles);
            CategoryNode.Nodes.Add(node);

            // Add the builtin report node Builtin Admin
            node = new TreeNode(Utility.Constants.ReportNode_SystemAdministratorVulnerability);
            node.Name = Utility.Constants.ReportNode_SystemAdministratorVulnerability;
            node.Tag = new Utility.NodeTag(new Data.Report(node.Name),
                                            Utility.View.Report_SystemAdministratorVulnerability);
            node.ImageIndex = node.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.Report_SystemAdminVulnerability);
            CategoryNode.Nodes.Add(node);

            // Add the builtin report node Everyone Group Access
            node = new TreeNode(Utility.Constants.ReportNode_ServersWithDangerousGroups);
            node.Name = Utility.Constants.ReportNode_ServersWithDangerousGroups;
            node.Tag = new Utility.NodeTag(new Data.Report(node.Name),
                                            Utility.View.Report_ServersWithDangerousGroups);
            node.ImageIndex = node.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.Report_DangerousWindowsGroups);
            CategoryNode.Nodes.Add(node);

            // Add the builtin report node Database Chaining
            node = new TreeNode(Utility.Constants.ReportNode_DatabaseChaining);
            node.Name = Utility.Constants.ReportNode_DatabaseChaining;
            node.Tag = new Utility.NodeTag(new Data.Report(node.Name),
                                            Utility.View.Report_DatabaseChaining);
            node.ImageIndex = node.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.Report_DBChainingEnabled);
            CategoryNode.Nodes.Add(node);

            // Add the builtin report node Mail Vulnerability
            node = new TreeNode(Utility.Constants.ReportNode_MailVulnerability);
            node.Name = Utility.Constants.ReportNode_MailVulnerability;
            node.Tag = new Utility.NodeTag(new Data.Report(node.Name),
                                            Utility.View.Report_MailVulnerability);
            node.ImageIndex = node.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.Report_MailVulnerability);
            CategoryNode.Nodes.Add(node);

            // Add the builtin report node Login Vulnerability
            node = new TreeNode(Utility.Constants.ReportNode_LoginVulnerability);
            node.Name = Utility.Constants.ReportNode_LoginVulnerability;
            node.Tag = new Utility.NodeTag(new Data.Report(node.Name),
                                            Utility.View.Report_LoginVulnerability);
            node.ImageIndex = node.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.Report_LoginVulnerabiliy);
            CategoryNode.Nodes.Add(node);

            // 2. Comparison Category Reports
            // Create the builtin reports Category "Comparison" node
            CategoryNode = new TreeNode(Utility.Constants.ReportsNode_Category_Comparison);
            CategoryNode.Name = Utility.Constants.ReportsNode_Category_Comparison;
            CategoryNode.ImageIndex = CategoryNode.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.Folder);
            CategoryNode.Tag = new Utility.NodeTag(new Data.Main_Reports(Utility.Constants.ReportsNode_Category_Comparison, Views.View_Main_Reports.Tab.Comparison),
                                                            Utility.View.Main_Reports);
            rRootNode.Nodes.Add(CategoryNode);

            // Add the builtin report node Orphaned Logins
            node = new TreeNode(Utility.Constants.reportNode_CompareAssessments);
            node.Name = Utility.Constants.reportNode_CompareAssessments;
            node.Tag = new Utility.NodeTag(new Data.Report(node.Name),
                                            Utility.View.Report_CompareAssessments);
            node.ImageIndex = node.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.Report_AssessmentComparison);
            CategoryNode.Nodes.Add(node);

            // Add the builtin report node Server Database Users
            node = new TreeNode(Utility.Constants.reportNode_CompareSnapshots);
            node.Name = Utility.Constants.reportNode_CompareSnapshots;
            node.Tag = new Utility.NodeTag(new Data.Report(node.Name),
                                            Utility.View.Report_CompareSnapshots);
            node.ImageIndex = node.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.Report_SnapshotComparison);
            CategoryNode.Nodes.Add(node);



            rRootNode.ExpandAll();

            // End update.
            _explorerBar_ReportsTreeView.EndUpdate();

            rRootNode.Expand();
            _explorerBar_ReportsTreeView.SelectedNode = _explorerBar_ReportsTreeView.Nodes[0];
        }

        private void initManageSQLsecureGroup()
        {
            // Begin update
            _explorerBar_ManageSQLsecureTreeView.BeginUpdate();

            // Clear the full tree.
            _explorerBar_ManageSQLsecureTreeView.Nodes.Clear();

            // Add the Audited Servers main node
            TreeNode node = new TreeNode(Utility.Constants.ManagementNode_Servers);
            node.ImageIndex = node.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.SQLsecure);
            node.Tag = new Utility.NodeTag(new Data.Main_ManageSQLsecure(node.Name),
                                            Utility.View.Main_ManageSQLsecure);
            _explorerBar_ManageSQLsecureTreeView.Nodes.Add(node);

            // Add the Logins main node
            node = new TreeNode(Utility.Constants.ManagementNode_Logins);
            node.Tag = new Utility.NodeTag(new Data.Logins(node.Name),
                                            Utility.View.Logins);
            node.ImageIndex = node.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.NewSQLsecureLogin);
            _explorerBar_ManageSQLsecureTreeView.Nodes.Add(node);

            // Add the ManagePolicies main node
            node = new TreeNode(Utility.Constants.ManagementNode_ManagePolicies);
            node.Tag = new Utility.NodeTag(new Data.ManagePolicies(node.Name),
                                            Utility.View.ManagePolicies);
            node.ImageIndex = node.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.Policy);
            _explorerBar_ManageSQLsecureTreeView.Nodes.Add(node);


            // Add the SQLsecure Activity main node
            node = new TreeNode(Utility.Constants.ManagementNode_Activity);
            node.Tag = new Utility.NodeTag(new Data.SQLsecureActivity(node.Name), Utility.View.SQLsecureActivity);

            node.ImageIndex = node.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.SQLsecureActivity);
            _explorerBar_ManageSQLsecureTreeView.Nodes.Add(node);


            //Server tags
            node = new TreeNode(Utility.Constants.TManagementNode_TagsNode);
            node.Tag = new Utility.NodeTag(new Data.SQLsecureActivity(node.Name), Utility.View.ServerGroupTags);

            node.ImageIndex = node.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.ServerTags);
            _explorerBar_ManageSQLsecureTreeView.Nodes.Add(node);


            // End update.
            _explorerBar_ManageSQLsecureTreeView.EndUpdate();

            _explorerBar_ManageSQLsecureTreeView.SelectedNode = _explorerBar_ManageSQLsecureTreeView.Nodes[0];

        }

        private void refreshExplorerBar(bool isServerChanged)
        {
            using (logX.loggerX.DebugCall("refreshExplorerBar"))
            {
                // Enable/disable groups based on permissions.
                _explorerBar.Groups[Utility.Constants.ExplorerBar_GroupKey_Reports].Enabled =
                    Program.gController.isViewer;
                _explorerBar.Groups[Utility.Constants.ExplorerBar_GroupKey_Manage].Enabled = Program.gController.isAdmin;

                //SQLsecure 3.1 (Tushar)--Fix for SQLSECU-1647 and 1511
                _explorerBar.Groups[Utility.Constants.ExplorerBar_GroupKey_Explore].Enabled = Program.gController.Repository.IsValid;
                _explorerBar.Groups[Utility.Constants.ExplorerBar_GroupKey_Summary].Enabled = Program.gController.Repository.IsValid;
                
                // If the repository server has changed then go to the security summary group
                // otherwise stay on the present group if possible.
                if (isServerChanged)
                {
                    _explorerBar.SelectedGroup = _explorerBar.Groups[Utility.Constants.ExplorerBar_GroupKey_Summary];
                    initReportsGroup();
                    initManageSQLsecureGroup();
                    refreshExplorePermissionsGroup();
                    // put summary last so it will get focus
                    refreshSecuritySummaryGroup();
                }
                else
                {
                    //If the currently selected group is not enabled, then refresh/show 
                    //summary group.
                    if (!_explorerBar.SelectedGroup.Enabled)
                    {
                        _explorerBar.SelectedGroup = _explorerBar.Groups[Utility.Constants.ExplorerBar_GroupKey_Summary];
                        refreshSecuritySummaryGroup();
                    }
                    else
                    {
                        switch (explorerBarGroupKeyToEnum(_explorerBar.SelectedGroup.Key))
                        {
                            case Utility.ExplorerBarGroup.SecuritySummary:
                                refreshSecuritySummaryGroup();
                                break;

                            case Utility.ExplorerBarGroup.ExplorePermissions:
                                refreshExplorePermissionsGroup();
                                break;

                            case Utility.ExplorerBarGroup.Reports:
                                refreshReportsGroup();
                                break;

                            case Utility.ExplorerBarGroup.ManageSQLsecure:
                                refreshManageSQLsecureGroup();
                                break;

                            default:
                                Debug.Assert(false);
                                break;
                        }
                    }
                }
            }
        }

        private void refreshSecuritySummaryGroup()
        {
            using (logX.loggerX.DebugCall("refreshSecuritySummaryGroup"))
            {
                // Save the current policy to return if possible
                string selectedPolicyText = "";
                int selectedAssessmentId = 0;
                if (_explorerBar_SecuritySummaryPolicyTreeView.SelectedNode != null)
                {
                    TreeNode node = _explorerBar_SecuritySummaryPolicyTreeView.SelectedNode;
                    if (node.Level == 0)
                    {
                        selectedPolicyText = node.Text;
                    }
                    else if (node.Level == 2)
                    {
                        selectedPolicyText = node.Parent.Parent.Text;
                        if (node.Tag is Sql.Policy)
                        {
                            selectedAssessmentId = ((Sql.Policy)node.Tag).AssessmentId;
                        }
                    }
                }

                // Format is key=policyid, Pair<expand policy, Triple<expand Draft, expand Published, expand Approved>>
                Dictionary<int, Pair<bool, Triple<bool, bool, bool>>> expansions = new Dictionary<int, Pair<bool, Triple<bool, bool, bool>>>();
                foreach (TreeNode policynode in _explorerBar_SecuritySummaryPolicyTreeView.Nodes)
                {
                    if (policynode.Tag is Sql.Policy)
                    {
                        Pair<bool, Triple<bool, bool, bool>> item = new Pair<bool, Triple<bool, bool, bool>>((policynode.Nodes.Count == 0) || policynode.IsExpanded, new Triple<bool, bool, bool>(true, true, true));
                        foreach (TreeNode typenode in policynode.Nodes)
                        {
                            if (typenode.Name == Utility.Policy.AssessmentState.DisplayName(Utility.Policy.AssessmentState.Draft))
                            {
                                item.Second.First = typenode.IsExpanded;
                            }
                            else if (typenode.Name == Utility.Policy.AssessmentState.DisplayName(Utility.Policy.AssessmentState.Published))
                            {
                                item.Second.Second = typenode.IsExpanded;
                            }
                            else if (typenode.Name == Utility.Policy.AssessmentState.DisplayName(Utility.Policy.AssessmentState.Approved))
                            {
                                item.Second.Third = typenode.IsExpanded;
                            }
                        }
                        expansions.Add(((Sql.Policy)policynode.Tag).PolicyId, item);
                    }
                }

                // Begin update
                _explorerBar_SecuritySummaryPolicyTreeView.BeginUpdate();
                _comboBox_Report_Policies.BeginUpdate();

                // Clear the full tree.
                _explorerBar_SecuritySummaryPolicyTreeView.Nodes.Clear();

                // Clear the policy combobox
                _comboBox_Report_Policies.Items.Clear();

                TreeNode selectedNode = null; // rootNode;

                // refreshing the policies is not needed if we are initializing because they were just retrieved
                if (m_Initialized)
                {
                    Program.gController.Repository.RefreshPolicies();
                }

                // Fill the root node children (Policies).
                foreach (Sql.Policy policy in Program.gController.Repository.Policies)
                {
                    TreeNode policynode = new TreeNode(policy.PolicyName);
                    policynode.Name = policy.PolicyName;
                    policynode.ImageIndex =
                        policynode.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.Policy);
                    policynode.Tag = policy;
                    _explorerBar_SecuritySummaryPolicyTreeView.Nodes.Add(policynode);

                    _comboBox_Report_Policies.Items.Add(policy);

                    if (selectedPolicyText == policynode.Name)
                    {
                        selectedNode = policynode;
                    }

                    loadTreeViewPolicyAssessments(policynode, Utility.Constants.AssessmentCount, selectedAssessmentId);

                    // try to restore the expansion to the same state as before the refresh
                    if (expansions.ContainsKey(policy.PolicyId))
                    {
                        if (expansions[policy.PolicyId].First)
                        {
                            policynode.Expand();
                        }
                        else
                        {
                            policynode.Collapse();
                        }
                        foreach (TreeNode typenode in policynode.Nodes)
                        {
                            bool expand = false;
                            if (typenode.Name == Utility.Policy.AssessmentState.DisplayName(Utility.Policy.AssessmentState.Draft))
                            {
                                expand = expansions[policy.PolicyId].Second.First;
                            }
                            else if (typenode.Name == Utility.Policy.AssessmentState.DisplayName(Utility.Policy.AssessmentState.Published))
                            {
                                expand = expansions[policy.PolicyId].Second.Second;
                            }
                            else if (typenode.Name == Utility.Policy.AssessmentState.DisplayName(Utility.Policy.AssessmentState.Approved))
                            {
                                expand = expansions[policy.PolicyId].Second.Third;
                            }
                            if (expand)
                            {
                                typenode.Expand();
                            }
                            else
                            {
                                typenode.Collapse();
                            }
                        }
                    }
                    if (selectedNode == policynode && selectedAssessmentId > 0)
                    {
                        bool found = false;
                        foreach (TreeNode statenode in policynode.Nodes)
                        {
                            foreach (TreeNode node in statenode.Nodes)
                            {
                                if (node.Tag is Sql.Policy)
                                {
                                    if (selectedAssessmentId == ((Sql.Policy)node.Tag).AssessmentId)
                                    {
                                        selectedNode = node;
                                        found = true;
                                        break;
                                    }
                                }
                            }
                            if (found)
                            {
                                break;
                            }
                        }
                    }
                }

                if (_explorerBar_SecuritySummaryPolicyTreeView.Nodes.Count == 0)
                {
                    // Create the empty root node.
                    TreeNode rootNode = new TreeNode(Utility.Constants.RootNode_Policy);
                    rootNode.ImageIndex =
                        rootNode.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.SQLsecure);
                    rootNode.Tag = new Utility.NodeTag(new Data.Main_SecuritySummary(),
                                                       Utility.View.Main_SecuritySummary);
                    _explorerBar_SecuritySummaryPolicyTreeView.Nodes.Add(rootNode);
                }

                if (selectedNode == null)
                {
                    selectedNode = _explorerBar_SecuritySummaryPolicyTreeView.Nodes[0];
                }

                if (_comboBox_Report_Policies.Items.Count > 0 && _comboBox_Report_Policies.SelectedIndex == -1)
                {
                    _comboBox_Report_Policies.SelectedIndex = 0;
                }

                logX.loggerX.Debug("Set selected policy tree node to " + selectedNode.Text);
                _explorerBar_SecuritySummaryPolicyTreeView.SelectedNode = selectedNode;

                // End update.
                _explorerBar_SecuritySummaryPolicyTreeView.EndUpdate();
                // try to make sure that the selected node's policy and it's children are visible
                TreeNode visiblepolicy = _explorerBar_SecuritySummaryPolicyTreeView.SelectedNode;
                if (visiblepolicy.Level == 3)
                {
                    visiblepolicy = visiblepolicy.Parent.Parent;
                }
                if (visiblepolicy.Nodes.Count > 0)
                {
                    if (visiblepolicy.LastNode.Nodes.Count > 0)
                    {
                        visiblepolicy.LastNode.LastNode.EnsureVisible();
                    }
                    else
                    {
                        visiblepolicy.LastNode.EnsureVisible();
                    }
                }
                // always make sure the selected node is visible in case the policy tree ends up not fitting in the window
                _explorerBar_SecuritySummaryPolicyTreeView.SelectedNode.EnsureVisible();
                _comboBox_Report_Policies.EndUpdate();
            }
        }

        public void refreshPolicyTreeNode(TreeNode node)
        {
            using (logX.loggerX.DebugCall("refreshPolicyTreeNode"))
            {
                // Get node type that has been selected.
                // There are currently three types of node:
                // the policy at level 0
                // the assessmenttype at level 1 which has no functionality
                // the assessment at level 2
                if (node != null)
                {
                    if (node.Level == 0)
                    {
                        if (node.Tag != null && node.Tag is Sql.Policy)
                        {
                            // Rebuild the policy in the cache and the node tag in case properties have changed
                            Sql.Policy policy =
                                Program.gController.Repository.GetPolicy(((Sql.Policy)node.Tag).PolicyId);
                            if (policy != null && Sql.Policy.IsPolicyRegistered(policy.PolicyId))
                            {
                                policy.RefreshPolicy();
                                policy.LoadAssessments();
                                // push the refreshed policy to the node tag instead of hitting the db again
                                node.Tag = policy;
                                node.Text =
                                   node.Name = policy.PolicyName;
                                if (node.IsSelected)
                                {
                                    refreshPolicyServerTree();
                                }
                            }
                            else
                            {
                                Utility.MsgBox.ShowError(Utility.ErrorMsgs.PolicyPropertiesCaption,
                                                         Utility.ErrorMsgs.PolicyNotRegistered);
                                Program.gController.SignalRefreshPoliciesEvent(0);
                            }
                        }

                        // Begin update.
                        _explorerBar_SecuritySummaryTreeView.BeginUpdate();

                        // Refresh the assessment list for the policy
                        loadTreeViewPolicyAssessments(node, 0);

                        Program.gController.RefreshCurrentView();

                        // End update.
                        _explorerBar_SecuritySummaryTreeView.EndUpdate();
                    }
                    else if (node.Level == 2)
                    {
                        if (node.Tag != null && node.Tag is Sql.Policy)
                        {
                            Sql.Policy assessment = (Sql.Policy)node.Tag;
                            // Rebuild the policy in the cache and the node tag in case properties have changed
                            Sql.Policy policy = Program.gController.Repository.GetPolicy(assessment.PolicyId);
                            if (policy != null && Sql.Policy.IsPolicyRegistered(policy.PolicyId))
                            {
                                policy.RefreshPolicy();
                                policy.LoadAssessments();
                                // push the refreshed policy to the node tag instead of hitting the db again
                                if (policy.HasAssessment(assessment.AssessmentId))
                                {
                                    assessment = policy.Assessments.Find(assessment.AssessmentId);
                                    node.Tag = assessment;
                                    node.Text =
                                        node.Name = assessment.AssessmentName;
                                    node.ImageIndex =
                                        node.SelectedImageIndex = assessment.FindingIconIndex;
                                    if (node.IsSelected)
                                    {
                                        refreshPolicyServerTree();
                                    }
                                }
                                else
                                {
                                    Utility.MsgBox.ShowError(Utility.ErrorMsgs.PolicyCaption,
                                                             Utility.ErrorMsgs.AssessmentNotFound);
                                    Program.gController.SignalRefreshPoliciesEvent(0);
                                }
                            }
                            else
                            {
                                Utility.MsgBox.ShowError(Utility.ErrorMsgs.PolicyCaption,
                                                         Utility.ErrorMsgs.PolicyNotRegistered);
                                Program.gController.SignalRefreshPoliciesEvent(0);
                            }
                        }

                        // Begin update.
                        _explorerBar_SecuritySummaryTreeView.BeginUpdate();

                        // Refresh the assessment list for the policy
                        loadTreeViewPolicyAssessments(node, 0);

                        Program.gController.RefreshCurrentView();

                        // End update.
                        _explorerBar_SecuritySummaryTreeView.EndUpdate();
                    }
                }
            }
        }

        private void loadTreeViewPolicyAssessments(TreeNode policynode, int assessmentcount)
        {
            loadTreeViewPolicyAssessments(policynode, assessmentcount, 0);
        }

        private void loadTreeViewPolicyAssessments(TreeNode policynode, int assessmentcount, int selectedassessmentid)
        {
            if (policynode.Tag is Sql.Policy)
            {
                Sql.Policy policy = (Sql.Policy)policynode.Tag;

                // If the selected assessmentid isn't passed, try to find it from the selected node
                if (selectedassessmentid == 0)
                {
                    // If the selected node is an assessment of this policy, save the id to return to it
                    TreeNode selectedNode = _explorerBar_SecuritySummaryPolicyTreeView.SelectedNode;
                    if (selectedNode != null &&
                        selectedNode.Parent != null &&
                        selectedNode.Parent.Parent != null &&
                        selectedNode.Parent.Parent.Tag != null &&
                        selectedNode.Parent.Parent.Tag is Sql.Policy)
                    {
                        Sql.Policy selectedPolicy = (Sql.Policy)selectedNode.Parent.Parent.Tag;
                        if (selectedPolicy.PolicyId == policy.PolicyId)
                        {
                            if (selectedNode.Tag != null &&
                                selectedNode.Tag is Sql.Policy)
                            {
                                selectedassessmentid = ((Sql.Policy)selectedNode.Tag).AssessmentId;
                            }
                        }
                    }
                }

                policynode.Nodes.Clear();

                if (policy.DraftAssessments.Count > 0)
                {
                    TreeNode draftnode = new TreeNode(Utility.Policy.AssessmentState.DisplayName(Utility.Policy.AssessmentState.Draft));
                    draftnode.Name = draftnode.Text;
                    draftnode.ImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.Folder_Assessment_Draft);
                    policynode.Nodes.Add(draftnode);
                    loadTreeViewPolicyAssessments(draftnode, Utility.Policy.AssessmentState.Draft, assessmentcount, selectedassessmentid);
                }

                if (policy.PublishedAssessments.Count > 0)
                {
                    TreeNode publishednode = new TreeNode(Utility.Policy.AssessmentState.DisplayName(Utility.Policy.AssessmentState.Published));
                    publishednode.Name = publishednode.Text;
                    publishednode.ImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.Folder_Assessment_Published);
                    policynode.Nodes.Add(publishednode);
                    loadTreeViewPolicyAssessments(publishednode, Utility.Policy.AssessmentState.Published, assessmentcount, selectedassessmentid);
                }

                if (policy.ApprovedAssessments.Count > 0)
                {
                    TreeNode approvednode = new TreeNode(Utility.Policy.AssessmentState.DisplayName(Utility.Policy.AssessmentState.Approved));
                    approvednode.Name = approvednode.Text;
                    approvednode.ImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.Folder_Assessment_Approved);
                    policynode.Nodes.Add(approvednode);
                    loadTreeViewPolicyAssessments(approvednode, Utility.Policy.AssessmentState.Approved, assessmentcount, selectedassessmentid);
                }

                policynode.Expand();
            }
        }

        private void loadTreeViewPolicyAssessments(TreeNode typenode, string assessmentstate, int assessmentcount, int selectedAssessmentId)
        {
            TreeNode firstAssessmentNode = null;
            TreeNode selectedAssessmentNode = null;
            bool isPolicyNodeSelected = false;
            TreeNode policynode = typenode.Parent;

            if (policynode.Tag.GetType() == typeof(Sql.Policy))
            {
                Sql.Policy policy = (Sql.Policy)policynode.Tag;
                int count = 0;

                // If count is 0 then use the existing count
                if (assessmentcount == 0)
                {
                    //if there are nodes, get the count and adjust for a More... node if it exists
                    if (typenode.Nodes.Count > Utility.Constants.AssessmentCount)
                    {
                        count = typenode.Nodes.Count - (typenode.LastNode.Tag == null ? 1 : 0);
                    }
                    if (count == 0)
                    {
                        count = Utility.Constants.AssessmentCount;
                    }
                }
                else
                {
                    count = assessmentcount;
                }

                // Clear all the assessments and refresh the entire list to get current on all of them
                // and avoid having to do manual matching against the list in case changed by another console
                typenode.Nodes.Clear();

                // If there is no selection, then just assume found for processing
                bool selectedFound = (selectedAssessmentId == 0);
                bool moreFound = false;
                // Fill the assessment nodes
                Repository.AssessmentList assessments = Sql.Policy.LoadAssessments(policy.PolicyId);
                // Only check the saved assessments to skip the current one
                foreach (Sql.Policy assessment in assessments.FindByState(assessmentstate))
                {
                    if (assessment.AssessmentId == selectedAssessmentId)
                    {
                        TreeNode assessmentnode = Program.gController.AddAssessmentToPolicy(policynode, assessment);
                        selectedAssessmentNode = assessmentnode;
                        selectedFound = true;
                        // save the first valid one to get current on if needed
                        if (firstAssessmentNode == null)
                        {
                            firstAssessmentNode = assessmentnode;
                        }
                    }
                    else
                    {
                        // If the display count is reached, and there are more, then show a More node
                        if (typenode.Nodes.Count < count)
                        {
                            TreeNode assessmentnode = Program.gController.AddAssessmentToPolicy(policynode, assessment);
                            // save the first valid one to get current on if needed
                            if (firstAssessmentNode == null)
                            {
                                firstAssessmentNode = assessmentnode;
                            }
                        }
                        else
                        {
                            moreFound = true;
                            if (selectedFound)
                            {
                                break;
                            }
                        }
                    }
                }
                if (moreFound)
                {
                    TreeNode assessmentnode = new TreeNode(Utility.Constants.MoreAssessments);
                    assessmentnode.ImageIndex = (int)AppIcons.Enum.SnapshotMore;
                    typenode.Nodes.Add(assessmentnode);
                }
            }

            typenode.Expand();

            if (selectedAssessmentNode != null)
            {
                _explorerBar_SecuritySummaryPolicyTreeView.SelectedNode = selectedAssessmentNode;
            }
        }

        private void refreshPolicyServerTree()
        {
            using (logX.loggerX.DebugCall("refreshPolicyServerTree"))
            {
                TreeNode node = _explorerBar_SecuritySummaryPolicyTreeView.SelectedNode;
                if (node.Tag is Sql.Policy)
                {
                    // Try to get current on the same server again after refresh even if the policy is changing
                    string selectedServer = string.Empty;
                    if (_explorerBar_SecuritySummaryTreeView.SelectedNode != null)
                    {
                        selectedServer = _explorerBar_SecuritySummaryTreeView.SelectedNode.Name;
                    }

                    _explorerBar_SecuritySummaryTreeView.SuspendLayout();
                    _explorerBar_SecuritySummaryTreeView.BeginUpdate();

                    _explorerBar_SecuritySummaryTreeView.Nodes.Clear();

                    Sql.Policy policy = (Sql.Policy)node.Tag;

                    // Create the root node.
                    TreeNode rootNode = (TreeNode)node.Clone(); // new TreeNode(policy.PolicyName);
                    rootNode.Nodes.Clear();
                    if (policy.IsAssessment)
                    {
                        rootNode.ImageIndex = rootNode.SelectedImageIndex = policy.FindingIconIndex;
                    }
                    //rootNode.ImageIndex = rootNode.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.SQLsecure);
                    //rootNode.Tag = new Utility.NodeTag(new Data.Main_SecuritySummary(policy),
                    //                Utility.View.Main_SecuritySummary);
                    _explorerBar_SecuritySummaryTreeView.Nodes.Add(rootNode);

                    TreeNode selectedNode = rootNode;

                    foreach (Sql.RegisteredServer server in policy.GetMemberServers())
                    {
                        TreeNode servernode = new TreeNode(server.ConnectionName);
                        servernode.Name = server.ConnectionName;
                        servernode.ImageIndex =
                            servernode.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.ServerOK);
                        servernode.Tag = server;
                        rootNode.Nodes.Add(servernode);

                        if (selectedServer == servernode.Name)
                        {
                            selectedNode = servernode;
                        }
                    }

                    // Expand the tree
                    rootNode.Expand();

                    _explorerBar_SecuritySummaryTreeView.SelectedNode = selectedNode;

                    // End update.
                    _explorerBar_SecuritySummaryTreeView.EndUpdate();
                    _explorerBar_SecuritySummaryTreeView.ResumeLayout();
                }
                else
                {
                    // Make sure the server tree gets cleared because there is no activate if no policy to select
                    _explorerBar_SecuritySummaryTreeView.Nodes.Clear();
                }
            }
        }

        private void refreshPolicyServerTreeNode(TreeNode node)
        {
            using (logX.loggerX.DebugCall("refreshPolicyServerTreeNode"))
            {
                if (node != null)
                {
                    // Refresh the entire tree
                    if (node.Level == 0)
                    {
                        refreshPolicyServerTree();
                    }
                    else if (node.Level == 1)
                    {
                        if (node.Tag != null && node.Tag.GetType() == typeof(Sql.RegisteredServer))
                        {
                            // Rebuild the server in the cache and the node tag in case properties have changed
                            Sql.RegisteredServer server =
                                Program.gController.Repository.RegisteredServers.Find(
                                    ((Sql.RegisteredServer)node.Tag).ConnectionName);
                            if (server != null && Sql.RegisteredServer.IsServerRegistered(server.ConnectionName))
                            {
                                server.RefreshServer();
                                // push the refreshed server to the node tag instead of hitting the db again
                                node.Tag = server;
                            }
                            else
                            {
                                Utility.MsgBox.ShowError(Utility.ErrorMsgs.SqlServerPropertiesCaption,
                                                         Utility.ErrorMsgs.ServerNotRegistered);
                                Program.gController.Repository.RefreshRegisteredServers();
                                refreshExplorePermissionsGroup();
                                refreshPolicyServerTree();
                            }
                        }

                        // Begin update.
                        _explorerBar_SecuritySummaryTreeView.BeginUpdate();

                        Program.gController.RefreshCurrentView();

                        // End update.
                        _explorerBar_SecuritySummaryTreeView.EndUpdate();
                    }
                }
            }
        }

        private void refreshExplorePermissionsGroup()
        {
            using (logX.loggerX.DebugCall("refreshExplorePermissionsGroup"))
            {
                // Save the current server to return if possible
                string selectedNodeText = string.Empty;
                if (_explorerBar_ExplorePermissionsTreeView.SelectedNode != null)
                {
                    selectedNodeText = _explorerBar_ExplorePermissionsTreeView.SelectedNode.Text;
                }

                // Begin update
                _explorerBar_ExplorePermissionsTreeView.BeginUpdate();

                // Clear the full tree.
                _explorerBar_ExplorePermissionsTreeView.Nodes.Clear();

                // Create the root node.
                TreeNode rootNode = new TreeNode(Utility.Constants.RootNode_Explore);
                rootNode.ImageIndex =
                    rootNode.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.Report_AuditedSQServers);
                rootNode.Tag = new Utility.NodeTag(new Data.Main_ExplorePermissions(Utility.Constants.RootNode_Explore),
                                                   Utility.View.Main_ExplorePermissions);
                _explorerBar_ExplorePermissionsTreeView.Nodes.Add(rootNode);

                TreeNode selectedNode = rootNode;

                // Fill the root node children (Servers).
                foreach (Sql.RegisteredServer server in Program.gController.Repository.RegisteredServers)
                {
                    TreeNode servernode = new TreeNode(server.ConnectionName);
                    servernode.Name = server.ConnectionName;
                    servernode.ImageIndex =
                        servernode.SelectedImageIndex = AppIcons.AppImageIndex16(AppIcons.Enum.ServerOK);
                    servernode.Tag = server;
                    rootNode.Nodes.Add(servernode);

                    if (selectedNodeText == servernode.Name)
                    {
                        selectedNode = servernode;
                    }

                    loadTreeViewServerSnapshots(servernode, Utility.Constants.SnapshotCount);
                }

                // Expand the tree
                rootNode.Expand();

                // Select the root node.
                m_Refresh = true;
                if (selectedNodeText != selectedNode.Text && selectedNode == rootNode)
                {
                    //If the selected node is the rootnode force the view because 
                    // it won't reset the view, if it is still sitting on the default selection
                    Program.gController.SetRootView(Utility.ExplorerBarGroup.ExplorePermissions,
                                                    new Utility.NodeTag(
                                                        new Data.Main_ExplorePermissions(
                                                            Utility.Constants.RootNode_Explore),
                                                        Utility.View.Main_ExplorePermissions));
                }

                _explorerBar_ExplorePermissionsTreeView.SelectedNode = selectedNode;

                // End update.
                _explorerBar_ExplorePermissionsTreeView.EndUpdate();
            }
        }

        public void refreshExplorerTreeNode(TreeNode node)
        {
            // Get node type that has been selected.
            // There are 4 types of nodes:
            // the root at level 0
            // the servers at level 1
            // the snapshots at level 2
            // the more snapshots link at level 2
            if (node != null)
            {
                if (node.Level == 0)
                {
                    // Refresh the entire tree
                    Program.gController.Repository.RefreshRegisteredServers();
                    refreshExplorePermissionsGroup();
                }
                else if (node.Level == 1)
                {
                    if (node.Tag != null && node.Tag.GetType() == typeof(Sql.RegisteredServer))
                    {
                        // Rebuild the server in the cache and the node tag in case properties have changed
                        Sql.RegisteredServer server = Program.gController.Repository.RegisteredServers.Find(((Sql.RegisteredServer)node.Tag).ConnectionName);
                        if (server != null && Sql.RegisteredServer.IsServerRegistered(server.ConnectionName))
                        {
                            server.RefreshServer();
                            // push the refreshed server to the node tag instead of hitting the db again
                            node.Tag = server;
                        }
                        else
                        {
                            Utility.MsgBox.ShowError(Utility.ErrorMsgs.SqlServerPropertiesCaption, Utility.ErrorMsgs.ServerNotRegistered);
                            Program.gController.Repository.RefreshRegisteredServers();
                            refreshExplorePermissionsGroup();
                        }
                    }

                    // Begin update.
                    _explorerBar_ExplorePermissionsTreeView.BeginUpdate();

                    // Refresh the snapshot list for the server
                    loadTreeViewServerSnapshots(node, 0);

                    Program.gController.RefreshCurrentView();

                    // End update.
                    _explorerBar_ExplorePermissionsTreeView.EndUpdate();
                }
                else if (node.Level == 2 && node.Tag != null && node.Tag.GetType() == typeof(Sql.Snapshot))
                {
                    if (Sql.RegisteredServer.IsServerRegistered(((Sql.Snapshot)node.Tag).ConnectionName))
                    {
                        // Refresh the snapshot
                        Sql.Snapshot snap = Sql.Snapshot.GetSnapShot(((Sql.Snapshot)node.Tag).SnapshotId);
                        if (snap != null)
                        {
                            // Begin update.
                            _explorerBar_ExplorePermissionsTreeView.BeginUpdate();

                            node.ImageIndex =
                                node.SelectedImageIndex = snap.IconIndex;
                            node.Tag = snap;

                            // End update.
                            _explorerBar_ExplorePermissionsTreeView.EndUpdate();
                        }
                    }
                    else
                    {
                        Utility.MsgBox.ShowError(Utility.ErrorMsgs.SnapshotPropertiesCaption, Utility.ErrorMsgs.ServerNotRegistered);
                        Program.gController.SignalRefreshServersEvent(false, string.Empty);
                    }

                    Program.gController.RefreshCurrentView();
                }
            }
        }

        private void loadTreeViewServerSnapshots(TreeNode servernode, int snapshotcount)
        {
            bool isExpanded = servernode.IsExpanded;
            TreeNode firstSnapshotNode = null;
            TreeNode selectedSnapshotNode = null;
            bool isServerNodeSelected = false;

            if (servernode.Tag.GetType() == typeof(Sql.RegisteredServer))
            {
                Sql.RegisteredServer server = (Sql.RegisteredServer)servernode.Tag;
                int count = 0;

                // If count is 0 then use the existing count
                if (snapshotcount == 0)
                {
                    //if there are nodes, get the count and adjust for a More... node if it exists
                    if (servernode.Nodes.Count > Utility.Constants.SnapshotCount)
                    {
                        count = servernode.Nodes.Count - (servernode.LastNode.Tag == null ? 1 : 0);
                    }
                    if (count == 0)
                    {
                        count = Utility.Constants.SnapshotCount;
                    }
                }
                else
                {
                    count = snapshotcount;
                }

                // If the selected node is a snapshot of this server, save the id to return to it
                int selectedSnapshotId = 0;
                DateTime? selectedSnapshotTime = null;
                TreeNode selectedNode = _explorerBar_ExplorePermissionsTreeView.SelectedNode;
                if (selectedNode != null &&
                    selectedNode.Parent != null &&
                    selectedNode.Parent.Tag != null &&
                    selectedNode.Parent.Tag.GetType() == typeof(Sql.RegisteredServer))
                {
                    if (((Sql.RegisteredServer)selectedNode.Parent.Tag).ServerName == server.ServerName)
                    {
                        if (selectedNode.Tag != null &&
                            selectedNode.Tag.GetType() == typeof(Sql.Snapshot))
                        {
                            selectedSnapshotId = ((Sql.Snapshot)selectedNode.Tag).SnapshotId;
                            selectedSnapshotTime = ((Sql.Snapshot)selectedNode.Tag).StartTime;
                        }
                    }
                }

                if (selectedNode != null &&
                    selectedNode.Tag != null &&
                    selectedNode.Tag.GetType() == typeof(Sql.RegisteredServer))
                {
                    isServerNodeSelected = true;
                }

                // Clear all the snapshots and refresh the entire list to get current on all of them
                // and avoid having to do manual matching against the list in case changed by another console
                servernode.Nodes.Clear();

                // If there is no selection, then just assume found for processing
                bool selectedFound = (selectedSnapshotId == 0);
                bool moreFound = false;
                // Fill the Snapshot nodes
                Sql.Snapshot.SnapshotList snapshots = Sql.Snapshot.LoadSnapshots(server.ConnectionName);
                foreach (Sql.Snapshot snap in snapshots)
                {
                    if (snap.SnapshotId == selectedSnapshotId)
                    {
                        TreeNode snapshotnode = Program.gController.AddSnapshotToServer(servernode, snap);
                        selectedSnapshotNode = snapshotnode;
                        selectedFound = true;
                        // save the first valid one to get current on if needed
                        if (firstSnapshotNode == null && snap.HasValidPermissions)
                        {
                            firstSnapshotNode = snapshotnode;
                        }
                    }
                    else
                    {
                        // If the display count is reached, and there are more, then show a More node
                        if (servernode.Nodes.Count < count)
                        {
                            TreeNode snapshotnode = Program.gController.AddSnapshotToServer(servernode, snap);
                            // save the first valid one to get current on if needed
                            if (firstSnapshotNode == null && snap.HasValidPermissions)
                            {
                                firstSnapshotNode = snapshotnode;
                            }
                        }
                        else
                        {
                            moreFound = true;
                            if (selectedFound)
                            {
                                break;
                            }
                        }
                    }
                }
                if (moreFound)
                {
                    TreeNode snapshotnode = new TreeNode(Utility.Constants.MoreSnapshots);
                    snapshotnode.ImageIndex = (int)AppIcons.Enum.SnapshotMore;
                    servernode.Nodes.Add(snapshotnode);
                }
            }

            if (isExpanded)
            {
                servernode.Expand();

                // if this is a refresh, then make sure we get current on a good snapshot
                // snapshotcount will be 0 only if it is a refresh
                if (snapshotcount == 0)
                {
                    if (selectedSnapshotNode != null)
                    {
                        _explorerBar_ExplorePermissionsTreeView.SelectedNode = selectedSnapshotNode;
                    }
                    else if (firstSnapshotNode != null &&
                             !isServerNodeSelected)
                    {
                        _explorerBar_ExplorePermissionsTreeView.SelectedNode = firstSnapshotNode;
                    }
                    else
                    {
                        _explorerBar_ExplorePermissionsTreeView.SelectedNode = servernode;
                    }
                }
            }
        }

        private void refreshReportsGroup()
        {
            Debug.Assert(_explorerBar_ReportsTreeView.Nodes.Count > 0);
            Program.gController.RefreshCurrentView();
            // No dynamic content.
        }

        private void refreshManageSQLsecureGroup()
        {
            Debug.Assert(_explorerBar_ManageSQLsecureTreeView.Nodes.Count > 0);
            Program.gController.RefreshCurrentView();
            // No dynamic content.
        }

        #endregion

        #region Tree View Handlers

        // Security Summary Policy Tree
        private void _explorerBar_SecuritySummaryPolicyTreeView_Enter(object sender, EventArgs e)
        {
            setMenuConfigurationSecurityPolicy();
        }

        private void _explorerBar_SecuritySummaryPolicyTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            using (logX.loggerX.DebugCall("_explorerBar_SecuritySummaryPolicyTreeView_BeforeSelect"))
            {
                Cursor = Cursors.WaitCursor;

                // Handle the More Assessments here and cancel the selection change 
                // because we don't want or need to refresh the other nodes
                // However, if the current selection is a node in the same policy, it must be passed on 
                // and refreshed because it is deleted and recreated in the More Assessments processing
                if (e.Node.TreeView.Visible)
                {
                    if (e.Node.Level == 0)
                    {
                        if (e.Node.Tag != null && e.Node.Tag.GetType() == typeof(Sql.Policy)
                            && !Sql.Policy.IsPolicyRegistered(((Sql.Policy)e.Node.Tag).PolicyId))
                        {
                            e.Cancel = true;
                            Cursor = Cursors.Default;
                            Utility.MsgBox.ShowError(Utility.ErrorMsgs.PolicyCaption, Utility.ErrorMsgs.PolicyNotRegistered);
                            Program.gController.SignalRefreshPoliciesEvent(0);
                        }
                    }
                    else if (e.Node.Level == 1)
                    {
                        e.Cancel = true;
                        Cursor = Cursors.Default;
                    }
                    else if (e.Node.Level == 2)
                    {
                        TreeNode policyNode = e.Node.Parent.Parent;
                        TreeNode typeNode = e.Node.Parent;
                        if (e.Node.Tag == null)
                        {
                            m_PreviousSelectedNode = _explorerBar_SecuritySummaryTreeView.SelectedNode;
                            if (!e.Node.Parent.Equals(m_PreviousSelectedNode.Parent))
                            {
                                // Begin update.
                                _explorerBar_SecuritySummaryTreeView.BeginUpdate();

                                int selectedAssessmentId = 0;
                                if (m_PreviousSelectedNode != null
                                    && m_PreviousSelectedNode.Tag != null
                                    && m_PreviousSelectedNode.Tag is Sql.Policy)
                                {
                                    selectedAssessmentId = ((Sql.Policy)m_PreviousSelectedNode.Tag).AssessmentId;
                                }
                                loadTreeViewPolicyAssessments(policyNode, e.Node.Parent.Nodes.Count + Utility.Constants.SnapshotCount, selectedAssessmentId);

                                // End update.
                                _explorerBar_SecuritySummaryTreeView.EndUpdate();
                                e.Cancel = true;
                                Cursor = Cursors.Default;
                            }
                        }
                        else if (e.Node.Tag.GetType() == typeof(Sql.Policy)
                                && !((Sql.Policy)policyNode.Tag).HasAssessment(((Sql.Policy)e.Node.Tag).AssessmentId))
                        {
                            e.Cancel = true;
                            Cursor = Cursors.Default;
                            Utility.MsgBox.ShowError(Utility.ErrorMsgs.PolicyCaption, Utility.ErrorMsgs.AssessmentNotFound);
                            Program.gController.SignalRefreshPoliciesEvent(0);
                        }
                    }
                }
            }
        }

        private void _explorerBar_SecuritySummaryPolicyTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            using (logX.loggerX.DebugCall("_explorerBar_SecuritySummaryPolicyTreeView_AfterSelect"))
            {
                Cursor = Cursors.WaitCursor;

                Debug.Assert(e.Node != null);

                refreshPolicyServerTree();
                if (e.Node.TreeView.Visible)
                {
                    _explorerBar_SecuritySummaryTreeView.Focus();
                    if (_explorerBar_SecuritySummaryTreeView.SelectedNode != null)
                    {
                        _explorerBar_SecuritySummaryTreeView.Refresh();
                    }
                }

                Cursor = Cursors.Default;
            }
        }

        private void _explorerBar_SecuritySummaryPolicyTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            ////if the node is not selected, make it the selected one for either left or right click
            ////so context menu will work correctly at different levels
            //if (!(((TreeView)sender).SelectedNode.Equals(e.Node)))
            //{
            //    ((TreeView)sender).SelectedNode = e.Node;
            //}

            //save the node clicked for handling by the context menu, but don't force select on right click
            m_ClickedNode = e.Node;
        }

        private void _explorerBar_SecuritySummaryPolicyTreeView_Leave(object sender, EventArgs e)
        {
            Program.gController.SetMenuConfiguration(m_menuConfiguration_Default);
        }

        private void _explorerBar_SecuritySummaryPolicyTreeView_VisibleChanged(object sender, EventArgs e)
        {
            using (logX.loggerX.DebugCall("_explorerBar_SecuritySummaryPolicyTreeView_VisibleChanged"))
            {
                if (_explorerBar_SecuritySummaryPolicyTreeView.Visible &&
                    _explorerBar_SecuritySummaryPolicyTreeView.Focused)
                {
                    setMenuConfigurationSecurityPolicy();
                }
            }
        }

        // Security Summary Server Tree
        private void _explorerBar_SecuritySummaryTreeView_Enter(object sender, EventArgs e)
        {
            setMenuConfigurationSecurityPolicyServerTree();
        }

        private void _explorerBar_SecuritySummaryTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            using (logX.loggerX.DebugCall("_explorerBar_SecuritySummaryTreeView_AfterSelect"))
            {
                Debug.Assert(e.Node != null);

                if (e.Node.TreeView.Visible && !m_SettingPolicyViewFromTree)
                {
                    Utility.NodeTag nodeTag = null;
                    switch (e.Node.Level)
                    {
                        case 0:
                            logX.loggerX.Debug("Policy node processed");
                            // root node of the Security Summary tree, is the policy or assessment
                            if (e.Node.Tag is Sql.Policy)
                            {
                                if (((Sql.Policy)e.Node.Tag).IsAssessment)
                                {
                                    nodeTag =
                                        new Utility.NodeTag(new Data.PolicyAssessment((Sql.Policy)e.Node.Tag),
                                                            Utility.View.PolicyAssessment);
                                }
                                else
                                {
                                    nodeTag =
                                        new Utility.NodeTag(new Data.Main_SecuritySummary((Sql.Policy)e.Node.Tag),
                                                            Utility.View.Main_SecuritySummary);
                                }
                            }
                            break;
                        case 1:
                            logX.loggerX.Debug("Server node processed");
                            // server node of the Security Summary tree is child of the parent policy node
                            if (e.Node.Parent.Tag is Sql.Policy)
                            {
                                if (((Sql.Policy)e.Node.Parent.Tag).IsAssessment)
                                {
                                    nodeTag =
                                        new Utility.NodeTag(new Data.PolicyAssessment((Sql.Policy)e.Node.Parent.Tag,
                                                            (Sql.RegisteredServer)e.Node.Tag),
                                                            Utility.View.PolicyAssessment);
                                }
                                else
                                {
                                    nodeTag =
                                        new Utility.NodeTag(new Data.Main_SecuritySummary((Sql.Policy)e.Node.Parent.Tag,
                                                            (Sql.RegisteredServer)e.Node.Tag),
                                                            Utility.View.Main_SecuritySummary);
                                }
                            }
                            break;
                        default:
                            break;
                    }
                    Debug.Assert(nodeTag != null);
                    logX.loggerX.Debug("Call ShowRootView");
                    Program.gController.ShowRootView(nodeTag);
                    setMenuConfigurationSecurityPolicyServerTree();
                    if (m_Refresh & m_Initialized) // If not initialized, don't do extra refresh as tree is built
                    {
                        m_Refresh = false;
                        logX.loggerX.Debug("Call Program.gController.RefreshCurrentView");
                        Program.gController.RefreshCurrentView();
                    }
                }

                // Clear this so it is only valid during the selection process
                m_PreviousSelectedNode = null;

                Cursor = Cursors.Default;
            }
        }

        private void _explorerBar_SecuritySummaryTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            ////if the node is not selected, make it the selected one for either left or right click
            ////so context menu will work correctly at different levels
            //if (!(((TreeView)sender).SelectedNode.Equals(e.Node)))
            //{
            //    ((TreeView)sender).SelectedNode = e.Node;
            //}

            //save the node clicked for handling by the context menu, but don't force select on right click
            m_ClickedNode = e.Node;
        }

        private void _explorerBar_SecuritySummaryTreeView_Leave(object sender, EventArgs e)
        {
            Program.gController.SetMenuConfiguration(m_menuConfiguration_Default);
        }

        private void _explorerBar_SecuritySummaryTreeView_VisibleChanged(object sender, EventArgs e)
        {
            using (logX.loggerX.DebugCall("_explorerBar_SecuritySummaryTreeView_VisibleChanged"))
            {
                if (_explorerBar_SecuritySummaryTreeView.Visible && _explorerBar_SecuritySummaryTreeView.Focused)
                {
                    setMenuConfigurationSecurityPolicyServerTree();
                }
            }
        }

        // Explore Permissions Tree
        private void _explorerBar_ExplorePermissionsTreeView_Enter(object sender, EventArgs e)
        {
            setMenuConfigurationExplorePermissions();
        }

        private void _explorerBar_ExplorePermissionsTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            // Handle the More Snapshots here and cancel the selection change 
            // because we don't want or need to refresh the other nodes
            // However, if the current selection is a node in the same server, it must be passed on 
            // and refreshed because it is deleted and recreated in the More Snapshots processing
            if (e.Node.TreeView.Visible)
            {
                if (e.Node.Level == 1)
                {
                    if (e.Node.Tag != null && e.Node.Tag.GetType() == typeof(Sql.RegisteredServer)
                        && !Sql.RegisteredServer.IsServerRegistered(((Sql.RegisteredServer)e.Node.Tag).ConnectionName))
                    {
                        e.Cancel = true;
                        Cursor = Cursors.Default;
                        Utility.MsgBox.ShowError(Utility.ErrorMsgs.SqlServerPropertiesCaption, Utility.ErrorMsgs.ServerNotRegistered);
                        Program.gController.SignalRefreshServersEvent(false, String.Empty);
                    }
                }
                else if (e.Node.Level == 2)
                {
                    if (e.Node.Tag == null)
                    {
                        m_PreviousSelectedNode = _explorerBar_ExplorePermissionsTreeView.SelectedNode;
                        if (!e.Node.Parent.Equals(m_PreviousSelectedNode.Parent))
                        {
                            TreeNode serverNode = e.Node.Parent;

                            // Begin update.
                            _explorerBar_ExplorePermissionsTreeView.BeginUpdate();

                            loadTreeViewServerSnapshots(serverNode, serverNode.Nodes.Count + Utility.Constants.SnapshotCount);

                            // End update.
                            _explorerBar_ExplorePermissionsTreeView.EndUpdate();
                            e.Cancel = true;
                            Cursor = Cursors.Default;
                        }
                    }
                    else if (e.Node.Tag.GetType() == typeof(Sql.Snapshot)
                            && !Sql.RegisteredServer.IsServerRegistered(((Sql.Snapshot)e.Node.Tag).ConnectionName))
                    {
                        e.Cancel = true;
                        Cursor = Cursors.Default;
                        Utility.MsgBox.ShowError(Utility.ErrorMsgs.SnapshotPropertiesCaption, Utility.ErrorMsgs.ServerNotRegistered);
                        Program.gController.SignalRefreshServersEvent(false, String.Empty);
                    }
                }
            }
        }

        private void _explorerBar_ExplorePermissionsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Debug.Assert(e.Node != null);

            if (e.Node.TreeView.Visible)
            {
                Utility.NodeTag nodeTag = null;
                switch (e.Node.Level)
                {
                    case 0:
                        // root node of the explore permissions tree, use its tag directly.
                        nodeTag = (Utility.NodeTag)e.Node.Tag;
                        break;
                    case 1:
                        // server node of the explore permissions tree
                        nodeTag = new Utility.NodeTag(new Data.Server((Sql.RegisteredServer)e.Node.Tag),
                                                                        Utility.View.Server);
                        break;
                    case 2:
                        // snapshot node of the explore permissions tree
                        if (e.Node.Tag != null &&
                            e.Node.Tag.GetType() == typeof(Sql.Snapshot))
                        {
                            nodeTag = new Utility.NodeTag(new Data.PermissionExplorer((Sql.RegisteredServer)e.Node.Parent.Tag,
                                                                                        ((Sql.Snapshot)e.Node.Tag).SnapshotId),
                                                                            Utility.View.PermissionExplorer);
                        }
                        else
                        {
                            // This will only get here if the selection is a snapshot in the current server
                            // other More Snapshot actions are handled in the BeforeSelect event to cancel
                            // the selection
                            TreeNode serverNode = e.Node.Parent;
                            int snapshotId = 0;
                            if (m_PreviousSelectedNode != null &&
                                m_PreviousSelectedNode.Parent != null &&
                                m_PreviousSelectedNode.Parent.Equals(serverNode) &&
                                m_PreviousSelectedNode.Tag != null &&
                                m_PreviousSelectedNode.Tag.GetType() == typeof(Sql.Snapshot))
                            {
                                // save the previous selection before it is wiped out by the load
                                snapshotId = ((Sql.Snapshot)m_PreviousSelectedNode.Tag).SnapshotId;
                            }

                            // Begin update.
                            _explorerBar_ExplorePermissionsTreeView.BeginUpdate();

                            // If it is the More Snapshots node, then refresh the list and add more
                            loadTreeViewServerSnapshots(serverNode, serverNode.Nodes.Count + Utility.Constants.SnapshotCount);

                            // End update.
                            _explorerBar_ExplorePermissionsTreeView.EndUpdate();

                            nodeTag = new Utility.NodeTag(new Data.PermissionExplorer((Sql.RegisteredServer)serverNode.Tag,
                                                                                        snapshotId),
                                                                            Utility.View.PermissionExplorer);
                        }
                        break;
                    default:
                        break;
                }
                Debug.Assert(nodeTag != null);
                Program.gController.ShowRootView(nodeTag);
                setMenuConfigurationExplorePermissions();
                if (m_Refresh)
                {
                    m_Refresh = false;
                    Program.gController.RefreshCurrentView();
                }
            }

            // Clear this so it is only valid during the selection process
            m_PreviousSelectedNode = null;

            Cursor = Cursors.Default;
        }

        private void _explorerBar_ExplorePermissionsTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            ////if the node is not selected, make it the selected one for either left or right click
            ////so context menu will work correctly at different levels
            //if (!(((TreeView)sender).SelectedNode.Equals(e.Node)))
            //{
            //    ((TreeView)sender).SelectedNode = e.Node;
            //}

            //save the node clicked for handling by the context menu, but don't force select on right click
            m_ClickedNode = e.Node;
        }

        private void _explorerBar_ExplorePermissionsTreeView_Leave(object sender, EventArgs e)
        {
            Program.gController.SetMenuConfiguration(m_menuConfiguration_Default);
        }

        private void _explorerBar_ExplorePermissionsTreeView_VisibleChanged(object sender, EventArgs e)
        {
            if (_explorerBar_ExplorePermissionsTreeView.Visible)
            {
                setMenuConfigurationExplorePermissions();
            }
        }

        private void _explorerBar_ReportsTreeView_Enter(object sender, EventArgs e)
        {
            setMenuConfigurationReports();
        }

        private void _explorerBar_ReportsTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            try     // Node instance can disappear during startup, so just ignore it in that case
            {
                // Level 0 & 1 nodes all show main reports page, so don't refresh when switching between them
                //if ((e.Node.Level == 0 || e.Node.Level == 1)
                //    && (((TreeView)sender).SelectedNode != null)
                //    && (((TreeView)sender).SelectedNode.Level == 0 || ((TreeView)sender).SelectedNode.Level == 1))
                //{
                //    m_ReportNodeShowView = false;
                //}
                m_ReportNodeShowView = true;
            }
            catch { }
        }

        private void _explorerBar_ReportsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // showing the view causes problems when changing groups, so only show when requested
            if (m_ReportNodeShowView)
            {
                Program.gController.ShowRootView((Utility.NodeTag)e.Node.Tag);
            }
            m_ReportNodeShowView = false;

            setMenuConfigurationReports();

            Cursor = Cursors.Default;
        }

        private void _explorerBar_ReportsTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            m_ReportNodeShowView = true;
        }

        private void _explorerBar_ManageSQLsecureTreeView_Enter(object sender, EventArgs e)
        {
            setMenuConfigurationManage();
        }

        private void _explorerBar_ManageSQLsecureTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
        }

        private void _explorerBar_ManageSQLsecureTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Program.gController.ShowRootView((Utility.NodeTag)e.Node.Tag);
            setMenuConfigurationManage();

            Cursor = Cursors.Default;
        }

        private void _explorerBar_ManageSQLsecureTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //if the node is not selected, make it the selected one for either left or right click
            //so context menu will work correctly at different levels
            if (!(((TreeView)sender).SelectedNode.Equals(e.Node)))
            {
                ((TreeView)sender).SelectedNode = e.Node;
            }
        }


        #endregion

        #region Context Menu Handlers

        private void _contextMenuStrip_Server_Opening(object sender, CancelEventArgs e)
        {
            bool isPolicyTree = false;
            bool isServer = false;
            bool isSnapshot = false;
            bool canExplore = true;

            // use the node that was clicked if a node was clicked on
            m_NodeToProcess = m_ClickedNode;
            isPolicyTree = m_NodeToProcess.TreeView == _explorerBar_SecuritySummaryPolicyTreeView;
            // Clear the clicked node because it doesn't get set if the user clicks on the tree, but not a node
            m_ClickedNode = null;

            if (((ContextMenuStrip)sender).SourceControl.GetType() == typeof(TreeView))
            {
                if (m_NodeToProcess == null && ((TreeView)((ContextMenuStrip)sender).SourceControl).SelectedNode != null)
                {
                    // If there is no clicked node, then use the selected node for off node clicks
                    // and save for processing by the menu items
                    m_NodeToProcess = ((TreeView)((ContextMenuStrip)sender).SourceControl).SelectedNode;
                }

                // Get node type that has been selected.
                // There are 4 types of nodes:
                // the root at level 0
                // the servers at level 1
                // the snapshots at level 2
                // the more snapshots link at level 2
                TreeNode node = m_NodeToProcess;
                if (node == null)
                {
                    // If there is no node, then just don't open because there is no context
                    e.Cancel = true;
                    return;
                }
                else if (node.Level == 0)
                {
                    canExplore = true;     // explorer will ask for server
                }
                else if (node.Level == 1)
                {
                    isServer = true;
                    canExplore = false;     // don't allow exploring unless a valid snapshot exists

                    if (node.Tag != null && node.Tag.GetType() == typeof(Sql.RegisteredServer))
                    {
                        Sql.RegisteredServer server = (Sql.RegisteredServer)node.Tag;
                        if (isPolicyTree)
                        {
                            int snapshotId = Sql.RegisteredServer.GetSnapshotIdByDate(server.ConnectionName,
                                                                                Program.gController.PolicyTime,
                                                                                Program.gController.PolicyUseBaselineSnapshots);
                            canExplore = (snapshotId > 0);
                        }
                        else
                        {
                            canExplore = (server.LastCollectionSnapshotId != 0);
                        }
                    }
                }
                else if (node.Level == 2)
                {
                    if (node.Tag == null)   // more link will have no tag
                    {
                        // if more link, then don't open menu
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        isSnapshot = true;
                        if (node.Tag.GetType() == typeof(Sql.Snapshot))
                        {
                            canExplore = ((Sql.Snapshot)node.Tag).HasValidPermissions;
                        }
                    }
                }
            }
            else
            {
                // If there is no tree, then just don't open because there is no context
                e.Cancel = true;
                return;
            }

            // Enable/disable based on the node type.
            _cmsi_Server_exploreUserPermissions.Enabled = canExplore && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.UserPermissions);
            _cmsi_Server_exploreSnapshot.Enabled = canExplore && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.ObjectPermissions);
            _cmsi_Server_auditSQLServer.Enabled = Program.gController.Permissions.hasSecurity(Utility.Security.Functions.AuditSQLServer);
            _cmsi_Server_removeSQLServer.Enabled = isServer && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Delete);
            _cmsi_Server_configureAuditSettings.Enabled = isServer && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.ConfigureAuditSettings);
            _cmsi_Server_collectDataSnapshot.Enabled = isServer && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Collect);
            _cmsi_Server_deleteSnapshot.Visible = !isPolicyTree;
            _cmsi_Server_deleteSnapshot.Enabled = isSnapshot && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Delete);
            _cmsi_Server_baselineSnapshot.Visible = !isPolicyTree;
            _cmsi_Server_baselineSnapshot.Enabled = isSnapshot && canExplore && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Baseline);
            _cmsi_Server_refresh.Enabled = Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Refresh);
            _cmsi_Server_properties.Enabled = (isServer || isSnapshot) && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Properties);
        }

        private void _cmsi_Server_exploreUserPermissions_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showTreePermissions(m_NodeToProcess, Views.View_PermissionExplorer.Tab.UserPermissions);

            Cursor = Cursors.Default;
        }

        private void _cmsi_Server_exploreSnapshot_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showTreePermissions(m_NodeToProcess, Views.View_PermissionExplorer.Tab.ObjectPermissions);

            Cursor = Cursors.Default;
        }

        private void _cmsi_Server_auditSQLServer_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Forms.Form_WizardRegisterSQLServer.Process();

            Cursor = Cursors.Default;
        }

        private void _cmsi_Server_removeSQLServer_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            TreeNode node = m_NodeToProcess;
            if (node != null)
            {
                if (node.Level == 1)
                {
                    string server = ((Sql.RegisteredServer)node.Tag).ConnectionName;
                    if (Sql.RegisteredServer.IsServerRegistered(server))
                    {
                        Forms.Form_RemoveRegisteredServer.Process(server);
                    }
                    else
                    {
                        Utility.MsgBox.ShowError(Utility.ErrorMsgs.RemoveSqlServerCaption, Utility.ErrorMsgs.ServerNotRegistered);
                        Program.gController.SignalRefreshServersEvent(false, string.Empty);
                    }
                }
            }

            Cursor = Cursors.Default;
        }

        private void _cmsi_Server_configureDataCollection_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            TreeNode node = m_NodeToProcess;
            if (node != null)
            {
                if (node.Level == 1)
                {
                    string server = ((Sql.RegisteredServer)node.Tag).ConnectionName;
                    if (Sql.RegisteredServer.IsServerRegistered(server))
                    {
                        Forms.Form_SqlServerProperties.Process(server, Forms.Form_SqlServerProperties.RequestedOperation.EditCofiguration, Program.gController.isAdmin);
                        Program.gController.RefreshCurrentView();
                    }
                    else
                    {
                        Utility.MsgBox.ShowError(Utility.ErrorMsgs.SqlServerPropertiesCaption, Utility.ErrorMsgs.ServerNotRegistered);
                        Program.gController.SignalRefreshServersEvent(false, string.Empty);
                    }
                }
            }

            Cursor = Cursors.Default;
        }

        private void _cmsi_Server_collectDataSnapshot_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            TreeNode node = m_NodeToProcess;
            if (node != null)
            {
                if (node.Level == 1)
                {
                    string server = ((Sql.RegisteredServer)node.Tag).ConnectionName;
                    if (Sql.RegisteredServer.IsServerRegistered(server))
                    {
                        Forms.Form_StartSnapshotJobAndShowProgress.Process(server);
                    }
                    else
                    {
                        Utility.MsgBox.ShowError(Utility.ErrorMsgs.SQLsecureDataCollection, Utility.ErrorMsgs.ServerNotRegistered);
                        Program.gController.SignalRefreshServersEvent(false, string.Empty);
                    }
                }
            }

            Cursor = Cursors.Default;
        }

        private void _cmsi_Server_deleteSnapshot_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            TreeNode node = m_NodeToProcess;
            if (node != null && node.Level == 2)
            {
                if (node.Tag != null)   // more link will have no tag
                {
                    if (node.Tag.GetType() == typeof(Sql.Snapshot))
                    {
                        if (Sql.RegisteredServer.IsServerRegistered(((Sql.Snapshot)node.Tag).ConnectionName))
                        {
                            Sql.Snapshot.SnapshotList snapshotlist = new Sql.Snapshot.SnapshotList();
                            snapshotlist.Add((Sql.Snapshot)node.Tag);
                            Forms.Form_DeleteSnapshot.Process(snapshotlist);
                            refreshExplorerTreeNode(node.Parent);
                        }
                        else
                        {
                            Utility.MsgBox.ShowError(Utility.ErrorMsgs.DeleteSnapshotCaption, Utility.ErrorMsgs.ServerNotRegistered);
                            Program.gController.SignalRefreshServersEvent(false, string.Empty);
                        }
                    }
                }
            }

            Cursor = Cursors.Default;
        }

        private void _cmsi_Server_baselineSnapshot_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            TreeNode node = m_NodeToProcess;
            if (node != null && node.Level == 2)
            {
                if (node.Tag != null)   // more link will have no tag
                {
                    if (node.Tag.GetType() == typeof(Sql.Snapshot))
                    {
                        if (Sql.RegisteredServer.IsServerRegistered(((Sql.Snapshot)node.Tag).ConnectionName))
                        {
                            if (Forms.Form_BaselineSnapshot.Process((Sql.Snapshot)node.Tag) == DialogResult.OK)
                            {
                                refreshExplorerTreeNode(node);
                            }
                        }
                        else
                        {
                            Utility.MsgBox.ShowError(Utility.ErrorMsgs.BaselineSnapshotCaption, Utility.ErrorMsgs.ServerNotRegistered);
                            Program.gController.SignalRefreshServersEvent(false, string.Empty);
                        }
                    }
                }
            }

            Cursor = Cursors.Default;
        }

        private void _cmsi_Server_refresh_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl.GetType() == typeof(TreeView))
            {
                refreshExplorerTreeNode(m_NodeToProcess);
            }

            Cursor = Cursors.Default;
        }

        private void _cmsi_Server_properties_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl.GetType() == typeof(TreeView))
            {
                TreeView tree = (TreeView)((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
                // Get node type that has been selected.
                // There are 4 types of nodes:
                // the root at level 0
                // the servers at level 1
                // the snapshots at level 2
                // the more snapshots link at level 2
                TreeNode node = m_NodeToProcess;
                if (node.Level == 1)
                {
                    Debug.Assert(node.Tag.GetType() == typeof(Sql.RegisteredServer));

                    string server = ((Sql.RegisteredServer)node.Tag).ConnectionName;
                    if (Sql.RegisteredServer.IsServerRegistered(server))
                    {
                        Forms.Form_SqlServerProperties.Process(server, Forms.Form_SqlServerProperties.RequestedOperation.GeneralProperties, Program.gController.isAdmin);
                        if (Program.gController.CurrentGroup == ExplorerBarGroup.SecuritySummary)
                        {
                            refreshSecuritySummaryGroup();
                        }
                        else
                        {
                            Program.gController.RefreshCurrentView();
                        }
                    }
                    else
                    {
                        Utility.MsgBox.ShowError(Utility.ErrorMsgs.SqlServerPropertiesCaption, Utility.ErrorMsgs.ServerNotRegistered);
                        Program.gController.SignalRefreshServersEvent(false, string.Empty);
                    }
                }
                else if (node.Level == 2 && node.Tag != null)   // more link will have no tag
                {
                    if (node.Tag.GetType() == typeof(Sql.Snapshot))
                    {
                        if (Sql.RegisteredServer.IsServerRegistered(((Sql.Snapshot)node.Tag).ConnectionName))
                        {
                            int snapshotId = ((Sql.Snapshot)node.Tag).SnapshotId;
                            Sql.ObjectTag tag = new Sql.ObjectTag(snapshotId, Sql.ObjectType.TypeEnum.Snapshot);

                            Forms.Form_SnapshotProperties.Process(tag);
                        }
                        else
                        {
                            Utility.MsgBox.ShowError(Utility.ErrorMsgs.SnapshotPropertiesCaption, Utility.ErrorMsgs.ServerNotRegistered);
                            Program.gController.SignalRefreshServersEvent(false, string.Empty);
                        }
                    }
                }
            }

            Cursor = Cursors.Default;
        }

        private void _contextMenuStrip_Policy_Opening(object sender, CancelEventArgs e)
        {
            bool isPolicy = false;
            bool isAssessment = false;
            bool isInAssessment = false;
            bool hasLog = false;
            bool isPolicyTree = false;
            bool isServerTree = false;
            bool isServer = false;
            bool canRemovePolicy = false;
            bool canRemoveServer = false;
            bool canUpdateAssessment = false;
            bool serversExist = Program.gController.Repository.RegisteredServers.Count > 0;

            // use the node that was clicked if a node was clicked on
            m_NodeToProcess = m_ClickedNode;
            // Clear the clicked node because it doesn't get set if the user clicks on the tree, but not a node
            m_ClickedNode = null;

            if (((ContextMenuStrip)sender).SourceControl.GetType() == typeof(TreeView))
            {
                if ((m_NodeToProcess == null || m_NodeToProcess.TreeView != (TreeView)((ContextMenuStrip)sender).SourceControl)
                    && ((TreeView)((ContextMenuStrip)sender).SourceControl).SelectedNode != null)
                {
                    // If there is no clicked node, then use the selected node for off node clicks
                    // and save for processing by the menu items
                    m_NodeToProcess = ((TreeView)((ContextMenuStrip)sender).SourceControl).SelectedNode;
                }

                isPolicyTree = (((ContextMenuStrip)sender).SourceControl == _explorerBar_SecuritySummaryPolicyTreeView);
                isServerTree = (((ContextMenuStrip)sender).SourceControl == _explorerBar_SecuritySummaryTreeView);

                TreeNode node = m_NodeToProcess;
                if (node == null || node.Tag == null)
                {
                    // If there is no node or no Tag, then just don't open because there is no context
                    e.Cancel = true;
                    return;
                }
                else if (node.Level == 0)
                {
                    if (node.Tag.GetType() == typeof(Sql.Policy))
                    {
                        Sql.Policy policy = (Sql.Policy)node.Tag;
                        isPolicy = !policy.IsAssessment;
                        canRemovePolicy = !policy.IsSystemPolicy && node.Nodes.Count == 0;
                        isAssessment = policy.IsAssessment;
                        hasLog = policy.IsPublishedAssessment || policy.IsApprovedAssessment;
                    }
                }
                else if (node.Level == 1 && isServerTree)
                {
                    if (node.Parent.Tag != null && node.Tag.GetType() == typeof(Sql.RegisteredServer))
                    {
                        isServer = true;
                        Sql.Policy policy = (Sql.Policy)node.Parent.Tag;
                        isInAssessment = policy.IsAssessment;
                        canRemoveServer = (policy.IsPolicy || (policy.IsAssessment && !policy.IsApprovedAssessment)) && !policy.IsDynamic;
                        hasLog = policy.IsPublishedAssessment || policy.IsApprovedAssessment;
                    }
                }
                else if (isPolicyTree)
                {
                    if (node.Tag.GetType() == typeof(Sql.Policy))
                    {
                        Sql.Policy policy = (Sql.Policy)node.Tag;
                        isPolicy = !policy.IsAssessment;
                        canRemovePolicy = (policy.IsPolicy && !policy.IsSystemPolicy) || (policy.IsAssessment && !policy.IsApprovedAssessment);
                        isAssessment = policy.IsAssessment;
                        canUpdateAssessment = isAssessment && !policy.IsApprovedAssessment;
                        hasLog = policy.IsPublishedAssessment || policy.IsApprovedAssessment;
                    }
                }
            }
            else
            {
                // If there is no tree, then just don't open because there is no context
                e.Cancel = true;
                return;
            }

            // Make functions visible based on which tree and node level
            //_cmsi_Policy_viewSummary.Visible = true;
            _cmsi_Policy_viewMetrics.Visible = isPolicy;
            _cmsi_Policy_viewUsers.Visible = isPolicy;
            _cmsi_Policy_viewLog.Visible = hasLog;
            _cmsi_Policy_exploreUserPermissions.Visible = isServerTree && isServer;
            _cmsi_Policy_exploreRolePermissions.Visible = isServerTree && isServer;
            _cmsi_Policy_exploreSnapshot.Visible = isServerTree && isServer;
            _cmsi_Policy_configurePolicy.Visible = isPolicy || isAssessment;
            _cmsi_Policy_addPolicy.Visible = isPolicyTree;
            _cmsi_Policy_importPolicy.Visible = isPolicyTree;
            _cmsi_Policy_exportPolicy.Visible = isPolicyTree;
            _cmsi_Policy_configureAuditSettings.Visible = isServerTree && isServer;
            _cmsi_Policy_collectDataSnapshot.Visible = isServerTree && isServer;
            _cmsi_Policy_createAssessment.Visible = isPolicy || isAssessment;
            // checking the Visible properties of the menu items doesn't work here
            _cmsi_Policy_separatorImport.Visible = isPolicyTree || (isServerTree && isServer);

            string AssessmentType = (isAssessment || isInAssessment) ? "Assessment" : "Policy";
            _cmsi_Policy_configureServers.Text = string.Format("Configure {0} Servers...", AssessmentType);
            _cmsi_Policy_configureServers.Visible = isPolicy || isAssessment;
            _cmsi_Policy_removePolicy.Text = string.Format("Remove {0}", AssessmentType);
            _cmsi_Policy_removePolicy.Visible = isPolicyTree;
            _cmsi_Policy_removeServer.Text = string.Format("Remove SQL Server from {0}", AssessmentType);
            _cmsi_Policy_removeServer.Visible = isServerTree && isServer;
            // checking the Visible properties of the menu items doesn't work here
            _cmsi_Policy_separatorRemove.Visible = isPolicyTree || (isServerTree && isServer);

            // Enable/disable based on the node type.
            _cmsi_Policy_viewSummary.Enabled = (isPolicy || isAssessment || isServer);
            _cmsi_Policy_viewMetrics.Enabled = (isPolicy || isServer);
            _cmsi_Policy_viewUsers.Enabled = (isPolicy || isServer);
            _cmsi_Policy_viewLog.Enabled = (isAssessment || isServer) && hasLog && Program.gController.Permissions.isAdmin;
            _cmsi_Policy_addPolicy.Enabled = Program.gController.Permissions.isAdmin;
            _cmsi_Policy_configurePolicy.Enabled = (isPolicy || (isAssessment && canUpdateAssessment)) && Program.gController.Permissions.isAdmin;
            _cmsi_Policy_configureServers.Enabled = (isPolicy || (isAssessment && canUpdateAssessment)) && Program.gController.Permissions.isAdmin;
            _cmsi_Policy_importPolicy.Enabled = isPolicy && Program.gController.Permissions.isAdmin;
            _cmsi_Policy_exportPolicy.Enabled = isPolicy && Program.gController.Permissions.isAdmin;
            _cmsi_Policy_configureAuditSettings.Enabled = isServer && Program.gController.Permissions.isAdmin;
            _cmsi_Policy_collectDataSnapshot.Enabled = isServer && Program.gController.Permissions.isAdmin;
            _cmsi_Policy_createAssessment.Enabled = serversExist && (isPolicy || isAssessment) && Program.gController.Permissions.isAdmin;
            _cmsi_Policy_removePolicy.Enabled = (isPolicy || isAssessment) && canRemovePolicy && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Delete);
            _cmsi_Policy_removeServer.Enabled = isServer && canRemoveServer && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Delete);
            _cmsi_Policy_refresh.Enabled = Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Refresh);
            _cmsi_Policy_properties.Enabled = ((isPolicy || isAssessment) || isServer) && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Properties);
        }

        private void _cmsi_Policy_viewSummary_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl.GetType() == typeof(TreeView))
            {
                TreeView tree = (TreeView)((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
                TreeNode node = m_NodeToProcess;

                m_SettingPolicyViewFromTree = true;

                // make sure the tree stays in sync with the view
                tree.SelectedNode = m_NodeToProcess;
                if (tree == _explorerBar_SecuritySummaryPolicyTreeView)
                {
                    _explorerBar_SecuritySummaryTreeView.SelectedNode = _explorerBar_SecuritySummaryTreeView.Nodes[0];
                }

                if (node.Level == 0 || node.Level == 2)
                {
                    Debug.Assert(node.Tag.GetType() == typeof(Sql.Policy));

                    Sql.Policy policy = (Sql.Policy)node.Tag;

                    if (policy.IsAssessment)
                    {
                        Program.gController.ShowRootView(new Utility.NodeTag(new Data.PolicyAssessment(policy, Views.View_PolicyAssessment.AssessmentView.Summary),
                                                                            Utility.View.PolicyAssessment));
                    }
                    else
                    {
                        Program.gController.ShowRootView(new Utility.NodeTag(new Data.Main_SecuritySummary(policy, Views.View_Main_SecuritySummary.SecurityView.Summary),
                                                                             Utility.View.Main_SecuritySummary));
                    }
                }
                else if (node.Level == 1)
                {
                    Debug.Assert(node.Tag.GetType() == typeof(Sql.RegisteredServer));
                    Debug.Assert(node.Parent.Tag.GetType() == typeof(Sql.Policy));

                    Sql.Policy policy = (Sql.Policy)node.Parent.Tag;
                    Sql.RegisteredServer server = (Sql.RegisteredServer)node.Tag;

                    if (policy.IsAssessment)
                    {
                        Program.gController.ShowRootView(new Utility.NodeTag(new Data.PolicyAssessment(policy, server, Views.View_PolicyAssessment.AssessmentView.Summary),
                                                                            Utility.View.PolicyAssessment));
                    }
                    else
                    {
                        Program.gController.ShowRootView(new Utility.NodeTag(new Data.Main_SecuritySummary(policy, server, Views.View_Main_SecuritySummary.SecurityView.Summary),
                                                                             Utility.View.Main_SecuritySummary));
                    }
                }

                m_SettingPolicyViewFromTree = false;
            }

            Cursor = Cursors.Default;
        }

        private void _cmsi_Policy_viewMetrics_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl.GetType() == typeof(TreeView))
            {
                TreeView tree = (TreeView)((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
                TreeNode node = m_NodeToProcess;

                m_SettingPolicyViewFromTree = true;

                // make sure the tree stays in sync with the view
                tree.SelectedNode = m_NodeToProcess;
                if (tree == _explorerBar_SecuritySummaryPolicyTreeView)
                {
                    _explorerBar_SecuritySummaryTreeView.SelectedNode = _explorerBar_SecuritySummaryTreeView.Nodes[0];
                }

                if (node.Level == 0)
                {
                    Debug.Assert(node.Tag.GetType() == typeof(Sql.Policy));

                    Sql.Policy policy = (Sql.Policy)node.Tag;

                    Program.gController.ShowRootView(new Utility.NodeTag(new Data.Main_SecuritySummary(policy, Views.View_Main_SecuritySummary.SecurityView.Settings),
                                                                         Utility.View.Main_SecuritySummary));
                }
                else if (node.Level == 1)
                {
                    Debug.Assert(node.Tag.GetType() == typeof(Sql.RegisteredServer));
                    Debug.Assert(node.Parent.Tag.GetType() == typeof(Sql.Policy));

                    Sql.Policy policy = (Sql.Policy)node.Parent.Tag;
                    Sql.RegisteredServer server = (Sql.RegisteredServer)node.Tag;

                    Program.gController.ShowRootView(new Utility.NodeTag(new Data.Main_SecuritySummary(policy, server, Views.View_Main_SecuritySummary.SecurityView.Settings),
                                                                         Utility.View.Main_SecuritySummary));
                }

                m_SettingPolicyViewFromTree = false;
            }

            Cursor = Cursors.Default;
        }

        private void _cmsi_Policy_viewUsers_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl.GetType() == typeof(TreeView))
            {
                TreeView tree = (TreeView)((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
                TreeNode node = m_NodeToProcess;

                m_SettingPolicyViewFromTree = true;

                // make sure the tree stays in sync with the view
                tree.SelectedNode = m_NodeToProcess;
                if (tree == _explorerBar_SecuritySummaryPolicyTreeView)
                {
                    _explorerBar_SecuritySummaryTreeView.SelectedNode = _explorerBar_SecuritySummaryTreeView.Nodes[0];
                }

                if (node.Level == 0)
                {
                    Debug.Assert(node.Tag.GetType() == typeof(Sql.Policy));

                    Sql.Policy policy = (Sql.Policy)node.Tag;

                    Program.gController.ShowRootView(new Utility.NodeTag(new Data.Main_SecuritySummary(policy, Views.View_Main_SecuritySummary.SecurityView.Users),
                                                                         Utility.View.Main_SecuritySummary));
                }
                else if (node.Level == 1)
                {
                    Debug.Assert(node.Tag.GetType() == typeof(Sql.RegisteredServer));
                    Debug.Assert(node.Parent.Tag.GetType() == typeof(Sql.Policy));

                    Sql.Policy policy = (Sql.Policy)node.Parent.Tag;
                    Sql.RegisteredServer server = (Sql.RegisteredServer)node.Tag;

                    Program.gController.ShowRootView(new Utility.NodeTag(new Data.Main_SecuritySummary(policy, server, Views.View_Main_SecuritySummary.SecurityView.Users),
                                                                         Utility.View.Main_SecuritySummary));
                }

                m_SettingPolicyViewFromTree = false;
            }

            Cursor = Cursors.Default;
        }

        private void _cmsi_Policy_viewLog_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl.GetType() == typeof(TreeView))
            {
                TreeView tree = (TreeView)((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
                TreeNode node = m_NodeToProcess;

                m_SettingPolicyViewFromTree = true;

                // make sure the tree stays in sync with the view
                tree.SelectedNode = m_NodeToProcess;
                if (tree == _explorerBar_SecuritySummaryPolicyTreeView)
                {
                    _explorerBar_SecuritySummaryTreeView.SelectedNode = _explorerBar_SecuritySummaryTreeView.Nodes[0];
                }

                if (node.Level == 0 || node.Level == 2)
                {
                    Debug.Assert(node.Tag.GetType() == typeof(Sql.Policy));

                    Sql.Policy policy = (Sql.Policy)node.Tag;

                    Program.gController.ShowRootView(new Utility.NodeTag(new Data.PolicyAssessment(policy, Views.View_PolicyAssessment.AssessmentView.Log),
                                                                       Utility.View.PolicyAssessment));
                }
                else if (node.Level == 1)
                {
                    Debug.Assert(node.Tag.GetType() == typeof(Sql.RegisteredServer));
                    Debug.Assert(node.Parent.Tag.GetType() == typeof(Sql.Policy));

                    Sql.Policy policy = (Sql.Policy)node.Parent.Tag;
                    Sql.RegisteredServer server = (Sql.RegisteredServer)node.Tag;

                    Program.gController.ShowRootView(new Utility.NodeTag(new Data.PolicyAssessment(policy, server, Views.View_PolicyAssessment.AssessmentView.Log),
                                                                       Utility.View.PolicyAssessment));
                }

                m_SettingPolicyViewFromTree = false;
            }

            Cursor = Cursors.Default;
        }

        private void _cmsi_Policy_exploreUserPermissions_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showTreePermissions(m_NodeToProcess, Views.View_PermissionExplorer.Tab.UserPermissions);

            Cursor = Cursors.Default;
        }

        private void _cmsi_Policy_exploreRolePermissions_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showTreePermissions(m_NodeToProcess, Views.View_PermissionExplorer.Tab.RolePermissions);

            Cursor = Cursors.Default;
        }

        private void _cmsi_Policy_exploreSnapshot_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showTreePermissions(m_NodeToProcess, Views.View_PermissionExplorer.Tab.ObjectPermissions);

            Cursor = Cursors.Default;
        }

        private void _cmsi_Policy_addPolicy_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Forms.Form_WizardCreatePolicy.Process();

            refreshSecuritySummaryGroup();

            Cursor = Cursors.Default;
        }

        private void _cmsi_Policy_configurePolicy_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            TreeNode node = m_NodeToProcess;
            if (node != null)
            {
                if (node.Level == 0 && node.Tag is Sql.Policy)
                {
                    int policyId = ((Sql.Policy)node.Tag).PolicyId;
                    int assessmentId = ((Sql.Policy)node.Tag).AssessmentId;

                    if (Sql.Policy.IsPolicyRegistered(policyId))
                    {
                        if (Forms.Form_PolicyProperties.Process(policyId, assessmentId, Program.gController.isAdmin,
                                                            Forms.Form_PolicyProperties.RequestedOperation.
                                                                ConfigureMetrics))
                        {
                            refreshPolicyServerTree();
                        }
                    }
                    else
                    {
                        Utility.MsgBox.ShowError(Utility.ErrorMsgs.PolicyPropertiesCaption, Utility.ErrorMsgs.PolicyNotRegistered);
                        Program.gController.SignalRefreshPoliciesEvent(0);
                    }
                }
                else if (node.Level == 2 && node.Tag is Sql.Policy)
                {
                    int policyId = ((Sql.Policy)node.Tag).PolicyId;
                    int assessmentId = ((Sql.Policy)node.Tag).AssessmentId;

                    if (Sql.Policy.IsPolicyRegistered(policyId))
                    {
                        if (Sql.Policy.IsAssessmentFound(policyId, assessmentId))
                        {
                            if (Forms.Form_PolicyProperties.Process(policyId, assessmentId, Program.gController.isAdmin,
                                                                    Forms.Form_PolicyProperties.RequestedOperation.
                                                                        ConfigureMetrics))
                            {
                                refreshPolicyServerTree();
                            }
                        }
                        else
                        {
                            Utility.MsgBox.ShowError(Utility.ErrorMsgs.PolicyPropertiesCaption, Utility.ErrorMsgs.AssessmentNotFound);
                            Program.gController.SignalRefreshPoliciesEvent(policyId);
                        }
                    }
                    else
                    {
                        Utility.MsgBox.ShowError(Utility.ErrorMsgs.PolicyPropertiesCaption, Utility.ErrorMsgs.PolicyNotRegistered);
                        Program.gController.SignalRefreshPoliciesEvent(0);
                    }
                }
            }

            Cursor = Cursors.Default;
        }

        private void _cmsi_Policy_importPolicy_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            string fileName = Form_ImportPolicy.Process();
            if (!string.IsNullOrEmpty(fileName))
            {
                Policy policy = new Policy();
                policy.ImportPolicyFromXMLFile(fileName, false);
                policy.IsSystemPolicy = false;
                if (Form_ImportExportPolicySecuriyChecks.ProcessImport(policy, Program.gController.isAdmin))
                {
                    if (Form_PolicyProperties.Process(policy, Program.gController.isAdmin, Form_PolicyProperties.RequestedOperation.ConfigureMetrics))
                    {
                        Program.gController.SignalRefreshPoliciesEvent(0);
                    }
                }
            }

            Cursor = Cursors.Default;
        }

        private void _cmsi_Policy_exportPolicy_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl.GetType() == typeof(TreeView))
            {
                TreeView tree = (TreeView)((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
                TreeNode node = m_NodeToProcess;
                if (node.Level == 0)
                {
                    Debug.Assert(node.Tag.GetType() == typeof(Sql.Policy));
                    Policy policy = (Policy)node.Tag;
                    Form_ImportExportPolicySecuriyChecks.ProcessExport(policy, Program.gController.isAdmin);
                }
            }
            Cursor = Cursors.Default;
        }

        private void _cmsi_Policy_configureAuditSettings_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            TreeNode node = m_NodeToProcess;
            if (node != null)
            {
                if (node.Level == 1)
                {
                    string server = ((Sql.RegisteredServer)node.Tag).ConnectionName;
                    if (Sql.RegisteredServer.IsServerRegistered(server))
                    {
                        Forms.Form_SqlServerProperties.Process(server, Forms.Form_SqlServerProperties.RequestedOperation.EditCofiguration, Program.gController.isAdmin);
                        Program.gController.RefreshCurrentView();
                    }
                    else
                    {
                        Utility.MsgBox.ShowError(Utility.ErrorMsgs.SqlServerPropertiesCaption, Utility.ErrorMsgs.ServerNotRegistered);
                        Program.gController.SignalRefreshServersEvent(false, string.Empty);
                    }
                }
            }

            Cursor = Cursors.Default;
        }

        private void _cmsi_Policy_collectDataSnapshot_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            TreeNode node = m_NodeToProcess;
            if (node != null)
            {
                if (node.Level == 1)
                {
                    string server = ((Sql.RegisteredServer)node.Tag).ConnectionName;
                    if (Sql.RegisteredServer.IsServerRegistered(server))
                    {
                        Forms.Form_StartSnapshotJobAndShowProgress.Process(server);
                    }
                    else
                    {
                        Utility.MsgBox.ShowError(Utility.ErrorMsgs.SQLsecureDataCollection, Utility.ErrorMsgs.ServerNotRegistered);
                        Program.gController.SignalRefreshServersEvent(false, string.Empty);
                    }
                }
            }

            Cursor = Cursors.Default;
        }

        private void _cmsi_Policy_createAssessment_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            TreeNode node = m_NodeToProcess;
            if (node != null && node.Tag is Sql.Policy)
            {
                Sql.Policy policy = ((Sql.Policy)node.Tag);
                if (Sql.Policy.IsPolicyRegistered(policy.PolicyId))
                {
                    Form_CreateAssessment.Process(policy);
                }
                else
                {
                    Utility.MsgBox.ShowError(Utility.ErrorMsgs.RemovePolicyCaption,
                                             Utility.ErrorMsgs.RemoveSystemPolicyMsg);
                    Program.gController.SignalRefreshPoliciesEvent(0);
                }
            }

            Cursor = Cursors.Default;
        }

        private void _cmsi_Policy_removePolicy_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            TreeNode node = m_NodeToProcess;
            if (node != null)
            {
                if (node.Tag is Sql.Policy)
                {
                    Sql.Policy policy = ((Sql.Policy)node.Tag);
                    if (Sql.Policy.IsPolicyRegistered(policy.PolicyId))
                    {
                        if (policy.IsPolicy)
                        {
                            if (!policy.IsSystemPolicy)
                            {
                                // Display confirmation, if user confirms remove the policy.
                                string caption = Utility.ErrorMsgs.RemovePolicyCaption + " - " +
                                                 policy.PolicyAssessmentName;
                                if (Utility.MsgBox.ShowConfirm(caption, Utility.ErrorMsgs.RemovePolicyConfirmMsg) ==
                                    DialogResult.Yes)
                                {
                                    try
                                    {
                                        Sql.Policy.RemovePolicy(policy.PolicyId);
                                    }
                                    catch (Exception ex)
                                    {
                                        Utility.MsgBox.ShowError(Utility.ErrorMsgs.RemovePolicyCaption,
                                                                 string.Format(Utility.ErrorMsgs.CantRemovePolicyMsg,
                                                                               policy.PolicyAssessmentName),
                                                                 ex);
                                    }

                                    Program.gController.SignalRefreshPoliciesEvent(0);
                                }
                            }
                            else
                            {
                                Utility.MsgBox.ShowError(Utility.ErrorMsgs.RemovePolicyCaption,
                                                         Utility.ErrorMsgs.RemoveSystemPolicyMsg);
                            }
                        }
                        else if (policy.IsAssessment)
                        {
                            if (!policy.IsApprovedAssessment)
                            {
                                // Display confirmation, if user confirms remove the policy.
                                if (Utility.MsgBox.ShowConfirm(Utility.ErrorMsgs.RemoveAssessmentCaption, string.Format(Utility.ErrorMsgs.RemoveAssessmentConfirmMsg, policy.PolicyAssessmentName)) == DialogResult.Yes)
                                {
                                    try
                                    {
                                        Sql.Policy.RemoveAssessment(policy.PolicyId, policy.AssessmentId);
                                    }
                                    catch (Exception ex)
                                    {
                                        Utility.MsgBox.ShowError(Utility.ErrorMsgs.RemoveAssessmentCaption,
                                                                 string.Format(Utility.ErrorMsgs.CantRemoveAssessmentMsg,
                                                                               policy.PolicyAssessmentName),
                                                                 ex);
                                    }

                                    Program.gController.SignalRefreshPoliciesEvent(0);
                                }
                            }
                            else
                            {
                                Utility.MsgBox.ShowError(Utility.ErrorMsgs.RemoveAssessmentCaption,
                                                         Utility.ErrorMsgs.RemoveApprovedAssessmentMsg);
                            }
                        }
                    }
                    else
                    {
                        Utility.MsgBox.ShowError(Utility.ErrorMsgs.RemovePolicyCaption,
                                                 Utility.ErrorMsgs.RemoveSystemPolicyMsg);
                        Program.gController.SignalRefreshPoliciesEvent(0);
                    }
                }
            }

            Cursor = Cursors.Default;
        }

        private void _cmsi_Policy_refresh_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl is TreeView
                && m_NodeToProcess != null
                && ((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl == m_NodeToProcess.TreeView)
            {
                if (m_NodeToProcess.TreeView == _explorerBar_SecuritySummaryPolicyTreeView)
                {
                    refreshPolicyTreeNode(m_NodeToProcess);
                }
                else if (m_NodeToProcess.TreeView == _explorerBar_SecuritySummaryTreeView)
                {
                    if (m_NodeToProcess.Level == 0)
                    {
                        refreshPolicyTreeNode(_explorerBar_SecuritySummaryPolicyTreeView.SelectedNode);
                    }
                    else
                    {
                        refreshPolicyServerTreeNode(m_NodeToProcess);
                    }
                }
                else
                {
                    refreshSecuritySummaryGroup();
                }
            }
            else
            {
                refreshSecuritySummaryGroup();
            }

            Cursor = Cursors.Default;
        }

        private void _cmsi_Policy_addServers_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl.GetType() == typeof(TreeView))
            {
                TreeView tree = (TreeView)((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
                TreeNode node = m_NodeToProcess;
                if (node.Level == 0 && node.Tag is Sql.Policy)
                {
                    int policyId = ((Sql.Policy)node.Tag).PolicyId;
                    int assessmentId = ((Sql.Policy)node.Tag).AssessmentId;

                    if (Sql.Policy.IsPolicyRegistered(policyId))
                    {
                        if (Forms.Form_PolicyProperties.Process(policyId, assessmentId, Program.gController.isAdmin,
                                                            Forms.Form_PolicyProperties.RequestedOperation.ManageServers))
                        {
                            refreshPolicyServerTree();
                        }
                    }
                    else
                    {
                        Utility.MsgBox.ShowError(Utility.ErrorMsgs.PolicyPropertiesCaption, Utility.ErrorMsgs.PolicyNotRegistered);
                        Program.gController.SignalRefreshPoliciesEvent(0);
                    }
                }
                else if (node.Level == 2 && node.Tag is Sql.Policy)
                {
                    int policyId = ((Sql.Policy)node.Tag).PolicyId;
                    int assessmentId = ((Sql.Policy)node.Tag).AssessmentId;

                    if (Sql.Policy.IsPolicyRegistered(policyId))
                    {
                        if (Sql.Policy.IsAssessmentFound(policyId, assessmentId))
                        {
                            if (Forms.Form_PolicyProperties.Process(policyId, assessmentId, Program.gController.isAdmin,
                                                                    Forms.Form_PolicyProperties.RequestedOperation.ManageServers))
                            {
                                refreshPolicyServerTree();
                            }
                        }
                        else
                        {
                            Utility.MsgBox.ShowError(Utility.ErrorMsgs.PolicyPropertiesCaption, Utility.ErrorMsgs.AssessmentNotFound);
                            Program.gController.SignalRefreshPoliciesEvent(policyId);
                        }
                    }
                    else
                    {
                        Utility.MsgBox.ShowError(Utility.ErrorMsgs.PolicyPropertiesCaption, Utility.ErrorMsgs.PolicyNotRegistered);
                        Program.gController.SignalRefreshPoliciesEvent(0);
                    }
                }
            }

            Cursor = Cursors.Default;
        }

        private void _cmsi_Policy_removeServer_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl.GetType() == typeof(TreeView))
            {
                TreeView tree = (TreeView)((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
                TreeNode node = m_NodeToProcess;
                if (node.Level == 1)
                {
                    Debug.Assert(node.Tag.GetType() == typeof(Sql.RegisteredServer));
                    Debug.Assert(node.Parent.Tag.GetType() == typeof(Sql.Policy));

                    Sql.Policy policy = (Sql.Policy)node.Parent.Tag;
                    Sql.RegisteredServer server = (Sql.RegisteredServer)node.Tag;

                    if (!policy.IsDynamic)
                    {
                        if (policy.IsAssessment)
                        {
                            if (DialogResult.Yes == Utility.MsgBox.ShowWarningConfirm(Utility.ErrorMsgs.RemoveServerFromAssessmentCaption,
                                        string.Format(Utility.ErrorMsgs.RemoveServerFromAssessmentConfirmMsg, server.ConnectionName, policy.PolicyAssessmentName)))
                            {
                                RegisteredServer.RemoveRegisteredServerFromPolicy(server.RegisteredServerId, policy.PolicyId, policy.AssessmentId);

                                refreshSecuritySummaryGroup();
                            }
                        }
                        else
                        {
                            if (DialogResult.Yes == Utility.MsgBox.ShowWarningConfirm(Utility.ErrorMsgs.RemoveServerFromPolicyCaption,
                                        string.Format(Utility.ErrorMsgs.RemoveServerFromPolicyConfirmMsg, server.ConnectionName, policy.PolicyName)))
                            {
                                RegisteredServer.RemoveRegisteredServerFromPolicy(server.RegisteredServerId, policy.PolicyId, policy.AssessmentId);

                                refreshSecuritySummaryGroup();
                            }
                        }
                    }
                    else
                    {
                        Utility.MsgBox.ShowError(Utility.ErrorMsgs.PolicyCaption, Utility.ErrorMsgs.ConfigureDynamicPolicyMsg);
                        Program.gController.SignalRefreshServersEvent(false, string.Empty);
                        refreshPolicyServerTree();
                    }
                }
            }

            Cursor = Cursors.Default;
        }

        private void _cmsi_Policy_properties_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl.GetType() == typeof(TreeView))
            {
                TreeView tree = (TreeView)((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
                TreeNode node = m_NodeToProcess;
                if ((node.Level == 0 || node.Level == 2))
                {
                    if (node.Tag is Sql.Policy)
                    {
                        Sql.Policy policy = (Sql.Policy)node.Tag;
                        if (
                            Forms.Form_PolicyProperties.Process(policy.PolicyId, policy.AssessmentId,
                                                                Program.gController.isAdmin))
                        {
                            refreshPolicyServerTree();
                        }
                    }
                }
                else if (node.Level == 1)
                {
                    Debug.Assert(node.Tag.GetType() == typeof(Sql.RegisteredServer));

                    string server = ((Sql.RegisteredServer)node.Tag).ConnectionName;
                    if (Sql.RegisteredServer.IsServerRegistered(server))
                    {
                        Forms.Form_SqlServerProperties.Process(server, Forms.Form_SqlServerProperties.RequestedOperation.GeneralProperties, Program.gController.isAdmin);

                        refreshSecuritySummaryGroup();
                    }
                    else
                    {
                        Utility.MsgBox.ShowError(Utility.ErrorMsgs.SqlServerPropertiesCaption, Utility.ErrorMsgs.ServerNotRegistered);
                        Program.gController.SignalRefreshServersEvent(false, string.Empty);
                        refreshPolicyServerTree();
                    }
                }
            }

            Cursor = Cursors.Default;
        }

        #endregion

        #region Splitter Handlers
        private void _splitContainer_MouseDown(object sender, MouseEventArgs e)
        {
            m_focused = getFocused(this.Controls);
        }

        private void _splitContainer_MouseUp(object sender, MouseEventArgs e)
        {
            if (m_focused != null)
            {
                m_focused.Focus();
                m_focused = null;
            }

        }
        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            string notifyTitle;
            string notifyText;
            foreach (Sql.RegisteredServer server in Program.gController.Repository.RegisteredServers)
            {
                server.GetNotifyText(out notifyTitle, out notifyText);
                if (server.ShowWhenDataCollectionCompletes && !string.IsNullOrEmpty(notifyTitle) && !string.IsNullOrEmpty(notifyText))
                {
                    notifyIcon1.ShowBalloonTip(2000, notifyTitle, notifyText, ToolTipIcon.Info);
                }
            }
        }

        private void toolStripMenu_NotifyShow_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
            }
            Focus();
        }

        private void toolStripMenuItem_NotifyClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void configureSMPTEmaiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_SmtpEmailProviderConfig.Process(Program.gController.isAdmin);
        }

        private void configureWeakPasswordDetectionTollStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_WeakPasswordDetection.Process(Program.gController.isAdmin);
        }

        private void _checkBox_Report_IncludeTime_CheckedChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (_checkBox_Report_IncludeTime.Checked == true)
            {
                _dateTimePicker_Report_Time.Enabled = true;
            }
            else
            {
                _dateTimePicker_Report_Time.Value = new DateTime(DateTime.Now.Year,
                                                                DateTime.Now.Month,
                                                                DateTime.Now.Day,
                                                                23, 59, 59);
                _dateTimePicker_Report_Time.Enabled = false;
            }

            Program.gController.RefreshCurrentView();

            Cursor = Cursors.Default;
        }

        private void _comboBox_Report_Policies_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Program.gController.RefreshCurrentView();
        }

        #endregion

        #region Delegates for thingies to notify the main form.

        void gController_RefreshPoliciesEvent(int policyId)
        {
            Program.gController.Repository.RefreshPolicies();
            refreshSecuritySummaryGroup();
        }

        void gController_RefreshServersEvent(bool isAdd, string server)
        {
            Program.gController.Repository.RefreshRegisteredServers();
            refreshSecuritySummaryGroup();
            refreshExplorePermissionsGroup();
        }

        #endregion

        private void _resultPane_Paint(object sender, PaintEventArgs e)
        {

        }

        private void _toolStrip_ImportServers_Click(object sender, EventArgs e)
        {
            DoImportSqlServers();
        }

        private void DoImportSqlServers()
        {
            Form_ImportServers.Process();
            refreshManageSQLsecureGroup();
        }

        //void Log(string logQuery)
        //{
        //    try
        //    {
        //        System.IO.File.AppendAllText(string.Format("{0}\\{1}", GetAssemblyPath,ConfigurationManager.AppSettings["ExecuteSQLScriptLog.txt"])
        //            , string.Format("{0} Logged at {1} {2} {3}", Environment.NewLine, DateTime.Now.ToString(),
        //            Environment.NewLine, logQuery));
        //    }catch(Exception e)
        //    {

        //    }
        //}

        private void importSQLServersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoImportSqlServers();
        }
    }
}
