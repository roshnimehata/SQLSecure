using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Idera.SQLsecure.Core.Logger;

namespace Idera.SQLsecure.UI.Console.Controls
{
    public partial class BaseView : UserControl, Interfaces.ICommandHandler, Interfaces.IRefresh
    {
        private static readonly LogX logX = new LogX("Idera.SQLsecure.UI.Console.Controls.BaseView");

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
                    Debug.Assert(false, "Unknown command passed to BaseView");
                    break;
            }

        }
        protected virtual void showBaseline()
        {
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - BaseView showBaseline command called erroneously");
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
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - BaseView showRefresh command called erroneously");
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

        public BaseView()
        {
            InitializeComponent();

            this._smallTask_Help.TaskImage = AppIcons.AppImage32(AppIcons.Enum32.Help);
            this._smallTask_Help.TaskHandler += new System.EventHandler(this.showHelp);
        }

        #endregion

        #region User Data Event Handling

        public void viewDataHandler()
        {
            _vw_TasksPanel.Visible = Utility.UserData.Current.View.TaskPanelVisible;
        }

        #endregion

        #region Fields

        private String m_Title;
        protected Utility.MenuConfiguration m_menuConfiguration = new Utility.MenuConfiguration();

        #endregion

        #region Properties

        public String Title
        {
            get { return m_Title; }
            set { m_Title = value; }
        }

        #endregion

        #region Helpers

        protected void showHelp(object sender, EventArgs e)
        {
            Program.gController.ShowViewHelp();
        }

        #endregion

        #region Event Handlers

        protected void BaseView_Enter(object sender, EventArgs e)
        {
            Program.gController.SetMenuConfiguration(m_menuConfiguration, this); 
        }

        #endregion
    }
}