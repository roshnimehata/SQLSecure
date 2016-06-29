using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Idera.SQLsecure.UI.Console.Utility;
using System.Data.SqlClient;

using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Sql;
using Idera.SQLsecure.UI.Console.Controls;

namespace Idera.SQLsecure.UI.Console.Views
{
    public partial class View_Server : Idera.SQLsecure.UI.Console.Controls.BaseView, Interfaces.IView, Interfaces.IRefresh
    {
        #region IView

        void Interfaces.IView.SetContext(Interfaces.IDataContext contextIn)
        {
            m_context = (Data.Server)contextIn;
            m_serverInstance = ((Data.Server)contextIn).ServerInstance;

            if (m_serverInstance != null)
            {
                Title = Utility.Constants.ViewTitle_Server + ": " + m_serverInstance.ConnectionName;
            }

            RefreshView();
        }
        String Interfaces.IView.HelpTopic
        {
            get { return Utility.Help.ServerHelpTopic; }
        }
        String Interfaces.IView.ConceptTopic
        {
            get { return Utility.Help.ServerConceptTopic; }
        }
        String Interfaces.IView.Title
        {
            get { return Title; }
        }

        #endregion

        #region ICommand

        protected override void showPermissions(Views.View_PermissionExplorer.Tab tabIn)
        {
            if (m_serverInstance != null && Sql.RegisteredServer.IsServerRegistered(m_serverInstance.ConnectionName))
            {
                Program.gController.SetCurrentServer(m_serverInstance);
                Program.gController.ShowRootView(new Utility.NodeTag(new Data.PermissionExplorer(m_serverInstance, tabIn),
                                                            Utility.View.PermissionExplorer));
            }
            else
            {
                Utility.MsgBox.ShowError(this.Title, Utility.ErrorMsgs.ServerNotRegistered);
                Program.gController.SignalRefreshServersEvent(false, null);
            }
        }

        protected override void showRefresh()
        {
            RefreshView();

            if (m_serverInstance != null)
            {
                Program.gController.RefreshServerInTree(m_serverInstance.ConnectionName);
            }
        }

        #endregion

        #region IRefresh Members

        public void RefreshView()
        {
            if (m_serverInstance == null
                || ! Sql.RegisteredServer.IsServerRegistered(m_serverInstance.ConnectionName))
            {
                _label_Server.Text = Utility.ErrorMsgs.ServerNotRegistered;
                _label_Version.Text =
                    _label_Edition.Text =
                    _label_Os.Text =

                    _label_CurrentSnapshotTime.Text =
                    _label_LastSuccessfulTime.Text =
                    _label_NextAuditTime.Text =

                    _label_Duration.Text =
                    _label_Objects.Text =
                    _label_Permissions.Text =
                    _label_Logins.Text =
                    _label_GroupMembers.Text =
                    _label_Databases.Text =
                    _label_WellKnownGroups.Text = String.Empty;

                _commonTask1.Enabled =
                    _commonTask2.Enabled =
                    _commonTask3.Enabled =
                    _commonTask4.Enabled = false;
            }
            else
            {
                bool isValid = (m_serverInstance.LastCollectionSnapshotId != 0);
                _commonTask1.Enabled = Program.gController.Permissions.hasSecurity(Utility.Security.Functions.ConfigureAuditSettings);
                _commonTask2.Enabled = Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Collect);
                _commonTask3.Enabled = isValid && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.UserPermissions);
                _commonTask4.Enabled = isValid && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.ObjectPermissions);

                loadData();
            }
            ((Interfaces.IView)_snapshots).SetContext(m_context);
        }

        #endregion

        #region fields

        private Data.Server m_context;
        private Sql.RegisteredServer m_serverInstance;

        #endregion

        #region Ctors

        public View_Server()
            : base()
        {
            InitializeComponent();

            this._label_Summary.Text = Utility.Constants.ViewSummary_Server;

            _snapshots.m_RefreshParentView += new UI.Console.Controls.Snapshots.RefreshViewHandlerDelegate(showRefresh);

            // Initialize base class fields.
            m_menuConfiguration = new Utility.MenuConfiguration();

            // Configure tasks
            _commonTask1.TaskImage = AppIcons.AppImageCommonTaskSmall(AppIcons.EnumCommonTaskSmall.ConfigureAuditSettingsCTSmall);
            _commonTask1.HoverTaskImage = AppIcons.AppImageCommonTask(AppIcons.EnumCommonTask.ConfigureAuditSettingsHoverCT);
            _commonTask1.TaskText = Utility.Constants.Task_Title_Configure_Short;
            _commonTask1.TaskDescription = Utility.Constants.Task_Descr_Configure;
            _commonTask1.TaskHandler += new System.EventHandler(this.configureDataCollection);

            _commonTask2.TaskImage = AppIcons.AppImageCommonTaskSmall(AppIcons.EnumCommonTaskSmall.CollectDataCTSmall);
            _commonTask2.HoverTaskImage = AppIcons.AppImageCommonTask(AppIcons.EnumCommonTask.CollectDataHoverCT);
            _commonTask2.TaskText = Utility.Constants.Task_Title_Collect;
            _commonTask2.TaskDescription = Utility.Constants.Task_Descr_Collect;
            _commonTask2.TaskHandler += new System.EventHandler(this.takeSnapshot);

            _commonTask3.TaskImage = AppIcons.AppImageCommonTaskSmall(AppIcons.EnumCommonTaskSmall.UserPermissionsCTSmall);
            _commonTask3.HoverTaskImage = AppIcons.AppImageCommonTask(AppIcons.EnumCommonTask.UserPermissionsHoverCT);
            _commonTask3.TaskText = Utility.Constants.Task_Title_User_Short;
            _commonTask3.TaskDescription = Utility.Constants.Task_Descr_User;
            _commonTask3.TaskHandler += new System.EventHandler(this.exploreUserPermissions);

            _commonTask4.TaskImage = AppIcons.AppImageCommonTaskSmall(AppIcons.EnumCommonTaskSmall.ObjectExplorerCTSmall);
            _commonTask4.HoverTaskImage = AppIcons.AppImageCommonTask(AppIcons.EnumCommonTask.ObjectExplorerHoverCT);
            _commonTask4.TaskText = Utility.Constants.Task_Title_Object_Short;
            _commonTask4.TaskDescription = Utility.Constants.Task_Descr_Object;
            _commonTask4.TaskHandler += new System.EventHandler(this.exploreObjectPermissions);
        }

        #endregion

        #region Helpers

        protected void loadData()
        {
            m_serverInstance.RefreshServer();
            _label_Server.Text = m_serverInstance.ConnectionName;
            _label_Version.Text = m_serverInstance.VersionFriendlyLong;
            _label_Edition.Text = m_serverInstance.Edition;
            _label_Os.Text = m_serverInstance.OS;

            _label_CurrentSnapshotTime.Text = m_serverInstance.CurrentCollectionTime;
            _label_LastSuccessfulTime.Text = m_serverInstance.LastCollectionTime;
            _label_NextAuditTime.Text = m_serverInstance.NextCollectionTime;

            _pictureBox_AuditStatus.Image = null;
            Sql.Snapshot snap = Sql.Snapshot.GetSnapShot(m_serverInstance.LastCollectionSnapshotId);
            if (snap != null)
            {
                _label_Duration.Text = snap.Duration;
                _label_Objects.Text = snap.NumObject.ToString("n0");
                _label_Permissions.Text = snap.NumPermission.ToString("n0");
                _label_Logins.Text = snap.NumLogin.ToString("n0");
                _label_GroupMembers.Text = snap.NumWindowsGroupMember.ToString("n0");

                if (string.Compare(snap.Status, Utility.Snapshot.StatusSuccessful) == 0)
                {
                    _pictureBox_AuditStatus.Image = AppIcons.AppImage48(AppIcons.EnumImageList48.StatusGood);
                }
                else if (string.Compare(snap.Status, Utility.Snapshot.StatusWarning) == 0)
                {
                    _pictureBox_AuditStatus.Image = AppIcons.AppImage48(AppIcons.EnumImageList48.StatusWarning);
                }
                else
                {
                    _pictureBox_AuditStatus.Image = AppIcons.AppImage48(AppIcons.EnumImageList48.StatusError);
                }

                List<Sql.Database> databases = Sql.Database.GetSnapshotDatabases(snap.SnapshotId);
                if (databases != null)
                {
                    _label_Databases.Text = databases.Count.ToString();
                }
                else
                {
                    _label_Databases.Text = String.Empty;
                }

                List<Sql.WindowsAccount> accounts = Sql.WindowsAccount.GetSuspectAccounts(snap.SnapshotId);
                if (accounts != null)
                {
                    int wellknownaccounts = 0;
                    foreach (Sql.WindowsAccount acct in accounts)
                    {
                        wellknownaccounts += (acct.AccountType == Sql.WindowsAccount.Type.WellKnownGroup) ? 1 : 0;
                    }
                    _label_WellKnownGroups.Text = wellknownaccounts.ToString("n0");
                }
                else
                {
                    _label_WellKnownGroups.Text = String.Empty;
                }
            }
            else
            {
                _label_Duration.Text =
                    _label_Objects.Text =
                    _label_Permissions.Text =
                    _label_Logins.Text =
                    _label_GroupMembers.Text =
                    _label_Databases.Text =
                    _label_WellKnownGroups.Text = String.Empty;
            }
        }

        protected void setMenuConfiguration()
        {
            m_menuConfiguration.EditItems[(int)Utility.MenuItems_Edit.Remove] = false;
            m_menuConfiguration.EditItems[(int)Utility.MenuItems_Edit.Properties] = false;
            m_menuConfiguration.EditItems[(int)Utility.MenuItems_View.Refresh] = true;

            Program.gController.SetMenuConfiguration(m_menuConfiguration, this);
        }

        #endregion

        #region Tasks

        private void configureDataCollection(object sender, EventArgs e)
        {
            Forms.Form_SqlServerProperties.Process(m_serverInstance.ConnectionName, Forms.Form_SqlServerProperties.RequestedOperation.EditCofiguration, Program.gController.isAdmin);

            this.RefreshView();
        }

        private void takeSnapshot(object sender, EventArgs e)
        {
            Forms.Form_StartSnapshotJobAndShowProgress.Process(m_serverInstance.ConnectionName);

            this.RefreshView();
        }

        private void exploreUserPermissions(object sender, EventArgs e)
        {
            showPermissions(Views.View_PermissionExplorer.Tab.UserPermissions);
        }

        private void exploreObjectPermissions(object sender, EventArgs e)
        {
            showPermissions(Views.View_PermissionExplorer.Tab.ObjectPermissions);
        }

        #endregion
    }
}