using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;

using Idera.SQLsecure.Core.Accounts;
using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Controls;
using Idera.SQLsecure.UI.Console.Utility;

namespace Idera.SQLsecure.UI.Console.Views
{
    public partial class View_PermissionExplorer : Idera.SQLsecure.UI.Console.Controls.BaseView, Interfaces.IView
    {
        public enum Tab
        {
            None = -1,
            SnapshotSummary = 0,
            UserPermissions = 1,
            RolePermissions = 2,
            ObjectPermissions = 3
        }

        #region IView

        void Interfaces.IView.SetContext(Interfaces.IDataContext contextIn)
        {
            //Title = Utility.Constants.ViewTitle_PermissionExplorer;

            m_context = (Data.PermissionExplorer)contextIn;
            m_serverInstance = ((Data.PermissionExplorer)contextIn).ServerInstance;
            m_snapshotId = ((Data.PermissionExplorer)contextIn).SnapShotId;

            if (m_serverInstance != null && Sql.RegisteredServer.IsServerRegistered(m_serverInstance.ConnectionName))
            {
                Title = m_serverInstance.ConnectionName;

                if (m_context.Tab != Tab.None)
                {
                    _ultraTabControl.SelectedTab = _ultraTabControl.Tabs[(int)m_context.Tab];
                }
                int tab = 0;
                if (_ultraTabControl.SelectedTab != null)
                {
                    tab = _ultraTabControl.SelectedTab.Index;
                }
                setTabContext(tab);

                _smallTask_Collect.Enabled = Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Collect);
                _smallTask_Configure.Enabled = Program.gController.Permissions.hasSecurity(Utility.Security.Functions.ConfigureAuditSettings);
            }
            else
            {
                //Program.gController.SignalRefreshServersEvent(false, null);
            }
        }
        String Interfaces.IView.HelpTopic
        {
            get
            {
                Interfaces.IView ctl = (Interfaces.IView)CurrentlyShownControl;
                if (ctl == null)
                {
                    return Utility.Help.PermissionExplorerHelpTopic;
                }
                else
                {
                    return ctl.HelpTopic;
                }
            }
        }
        String Interfaces.IView.ConceptTopic
        {
            get
            {
                Interfaces.IView ctl = (Interfaces.IView)CurrentlyShownControl;
                if (ctl == null)
                {
                    return Utility.Help.PermissionExplorerConceptTopic;
                }
                else
                {
                    return ctl.ConceptTopic;
                }
            }
        }
        String Interfaces.IView.Title
        {
            get { return Title; }
        }

        #endregion

        #region fields

        private Data.PermissionExplorer m_context;
        private Sql.RegisteredServer m_serverInstance;
        private int m_snapshotId = 0;

        #endregion

        #region Properties

        private Control CurrentlyShownControl
        {
            get
            {
                if (_userPermissions.Visible)
                {
                    return _userPermissions;
                }
                if (_rolePermissions.Visible)
                {
                    return _rolePermissions;
                }
                else if (_objectPermissions.Visible)
                {
                    return _objectPermissions;
                }
                else if (_snapshotProperties.Visible)
                {
                    return _snapshotProperties;
                }
                else
                {
                    Debug.Assert(false,"Current Control not found");
                    return null;
                }
            }
        }

        #endregion

        #region Ctors

        public View_PermissionExplorer() : base()
        {
            InitializeComponent();

            // Initialize base class fields.
            m_menuConfiguration = new Utility.MenuConfiguration();

            // Setup the summary label.
            _label_Summary.Text = Utility.Constants.ViewSummary_Permissions;

            // Setup tasks.
            _smallTask_Collect.TaskText = Utility.Constants.Task_Title_CollectData;
            _smallTask_Collect.TaskImage = AppIcons.AppImage32(AppIcons.Enum32.Snapshot);
            _smallTask_Collect.TaskHandler += new System.EventHandler(this.collectData);

            _smallTask_Configure.TaskText = Utility.Constants.Task_Title_Configure;
            _smallTask_Configure.TaskImage = AppIcons.AppImage32(AppIcons.Enum32.ConfigureAuditSettings);
            _smallTask_Configure.TaskHandler += new System.EventHandler(this.configureServer);

            //force to initial disable because there is no security handler at initial construction
            _smallTask_Collect.Enabled = false;
            _smallTask_Configure.Enabled = false;

            // Hide the focus rectangles on tabs and grids
            _ultraTabControl.DrawFilter = new HideFocusRectangleDrawFilter();
        }

        #endregion


        #region Overrides

        protected override void ShowRefresh()
        {
            Interfaces.ICommandHandler ctl = (Interfaces.ICommandHandler)CurrentlyShownControl;
            if (ctl != null)
            {
                ctl.ProcessCommand(Utility.ViewSpecificCommand.Refresh);
            }
        }
        #endregion

        #region Tasks

        private void collectData(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            try
            {
                if (m_serverInstance != null && Sql.RegisteredServer.IsServerRegistered(m_serverInstance.ConnectionName))
                {
                    Forms.Form_StartSnapshotJobAndShowProgress.Process(m_serverInstance.ConnectionName);
                }
                else
                {
                    MsgBox.ShowError(Utility.ErrorMsgs.SQLsecureDataCollection, Utility.ErrorMsgs.ServerNotRegistered);
                }
            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.SQLsecureDataCollection, Utility.ErrorMsgs.CantRunDataCollection, ex);
            }

            Cursor = Cursors.Default;
        }

        private void configureServer(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Forms.Form_SqlServerProperties.Process(m_serverInstance.ConnectionName, Forms.Form_SqlServerProperties.RequestedOperation.EditCofiguration, Program.gController.isAdmin);

            Cursor = Cursors.Default;
        }
        
        #endregion

        #region Helpers

        protected void setMenuConfiguration()
        {
            m_menuConfiguration.EditItems[(int)Utility.MenuItems_Edit.Remove] = false;
            m_menuConfiguration.EditItems[(int)Utility.MenuItems_Edit.Properties] = false;
            m_menuConfiguration.EditItems[(int)Utility.MenuItems_View.Refresh] = false;

            _smallTask_Collect.Enabled = Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Collect);
            _smallTask_Configure.Enabled = Program.gController.Permissions.hasSecurity(Utility.Security.Functions.ConfigureAuditSettings);

            Program.gController.SetMenuConfiguration(m_menuConfiguration, this);
        }

        protected void setTabContext(int tabIndex)
        {
            switch (tabIndex)
            {
                case (int)Tab.UserPermissions:
                    ((Interfaces.IView)_userPermissions).SetContext(m_context);
                    break;
                case (int)Tab.RolePermissions:
                    ((Interfaces.IView)_rolePermissions).SetContext(m_context);
                    break;
                case (int)Tab.ObjectPermissions:
                    ((Interfaces.IView)_objectPermissions).SetContext(m_context);
                    break;
                case (int)Tab.SnapshotSummary:
                    ((Interfaces.IView)_snapshotProperties).SetContext(m_context);
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Events

        #region View events

        private void View_PermissionExplorer_Enter(object sender, EventArgs e)
        {
            Program.gController.SetMenuConfiguration(m_menuConfiguration, this);
        }

        private void View_PermissionExplorer_Leave(object sender, EventArgs e)
        {
            Program.gController.SetMenuConfiguration(new Utility.MenuConfiguration());
        }

        #endregion

        #endregion

        private void _ultraTabControl_SelectedTabChanging(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangingEventArgs e)
        {
            setTabContext(e.Tab.Index);
        }

     
    }

}

