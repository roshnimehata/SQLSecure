using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using Idera.SQLsecure.UI.Console.Controls;

namespace Idera.SQLsecure.UI.Console.Views
{
    public partial class View_Main_ExplorePermissions : UserControl, Interfaces.IView, Interfaces.ICommandHandler, Interfaces.IRefresh
    {
        #region IView

        void Interfaces.IView.SetContext(Interfaces.IDataContext contextIn)
        {
            // Set the title.
            m_Title = contextIn.Name;

            _auditedServers.UpdateStatus();
        }
        String Interfaces.IView.HelpTopic
        {
            get { return Utility.Help.ExplorePermissionsHelpTopic; }
        }
        String Interfaces.IView.ConceptTopic
        {
            get { return Utility.Help.ExplorePermissionsConceptTopic; }
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
                    Forms.Form_WizardRegisterSQLServer.Process();
                    break;
                case Utility.ViewSpecificCommand.NewLogin:
                    Forms.Form_WizardNewLogin.Process();
                    break;
                case Utility.ViewSpecificCommand.Configure:
                    Sql.RegisteredServer server = Forms.Form_SelectRegisteredServer.GetServer();
                    if (server != null)
                    {
                        Forms.Form_SqlServerProperties.Process(server.ConnectionName,
                                                               Forms.Form_SqlServerProperties.RequestedOperation.EditCofiguration, 
                                                               Program.gController.isAdmin);
                    }
                    break;
                case Utility.ViewSpecificCommand.UserPermissions:
                    showPermissions(Views.View_PermissionExplorer.Tab.UserPermissions);
                    break;
                case Utility.ViewSpecificCommand.ObjectPermissions:
                    showPermissions(Views.View_PermissionExplorer.Tab.ObjectPermissions);
                    break;
                case Utility.ViewSpecificCommand.Refresh:
                    RefreshView();
                    break;
                default:
                    Debug.Assert(false, "Unknown command passed to BaseView");
                    break;
            }
        }

        private void showPermissions(Views.View_PermissionExplorer.Tab tabIn)
        {
            // This has a base function that requires no input, so can be defaulted from here or overriden
            if (tabIn == View_PermissionExplorer.Tab.UserPermissions)
            {
                Forms.Form_WizardUserPermissions.Process();
            }
            else if (tabIn == View_PermissionExplorer.Tab.ObjectPermissions)
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

        #endregion

        #region IRefresh Members

        public void RefreshView()
        {
            _auditedServers.UpdateStatus();

            _commonTask1.Enabled = Program.gController.Permissions.hasSecurity(Utility.Security.Functions.AuditSQLServer);
            _commonTask2.Enabled = Program.gController.Permissions.hasSecurity(Utility.Security.Functions.UserPermissions);
            _commonTask3.Enabled = Program.gController.isViewer;
            _commonTask4.Enabled = Program.gController.Permissions.hasSecurity(Utility.Security.Functions.ConfigureAuditSettings);
            _commonTask5.Enabled = Program.gController.Permissions.hasSecurity(Utility.Security.Functions.ObjectPermissions);
            _commonTask6.Enabled = Program.gController.Permissions.hasSecurity(Utility.Security.Functions.ManageLicense);
        }

        #endregion

        #region Fields

        private string m_Title;
        private Utility.MenuConfiguration m_MenuConfiguration;

        #endregion

        #region Ctors

        public View_Main_ExplorePermissions()
        {
            InitializeComponent();

           // pictureBox1.Image = imageList1.Images[0];

            // Display text.
            _lbl_Tag.Text = Utility.Constants.ViewTitle_Main_Explore;
            _lbl_Intro.Text = Utility.Constants.ViewSummary_Main_Explore;

            // Initialize menu configuration.
            m_MenuConfiguration = new Utility.MenuConfiguration();

            // Initialize and hook up all of the tasks
            _commonTask1.TaskImage = AppIcons.AppImageCommonTaskSmall(AppIcons.EnumCommonTaskSmall.AuditSQLServerCTSmall);
            _commonTask1.HoverTaskImage = AppIcons.AppImageCommonTask(AppIcons.EnumCommonTask.AuditSQLServerHoverCT);
            _commonTask1.TaskText = Utility.Constants.Task_Title_Register;
            _commonTask1.TaskDescription = Utility.Constants.Task_Descr_Register;
            _commonTask1.TaskHandler += new System.EventHandler(this.registerServer);

            _commonTask2.TaskImage = AppIcons.AppImageCommonTaskSmall(AppIcons.EnumCommonTaskSmall.UserPermissionsCTSmall);
            _commonTask2.HoverTaskImage = AppIcons.AppImageCommonTask(AppIcons.EnumCommonTask.UserPermissionsHoverCT);
            _commonTask2.TaskText = Utility.Constants.Task_Title_User;
            _commonTask2.TaskDescription = Utility.Constants.Task_Descr_User;
            _commonTask2.TaskHandler += new System.EventHandler(this.exploreUserPermissions);

            _commonTask3.TaskImage = AppIcons.AppImageCommonTaskSmall(AppIcons.EnumCommonTaskSmall.ReportsCTSmall);
            _commonTask3.HoverTaskImage = AppIcons.AppImageCommonTask(AppIcons.EnumCommonTask.ReportsHoverCT);
            _commonTask3.TaskText = Utility.Constants.Task_Title_Reports;
            _commonTask3.TaskDescription = Utility.Constants.Task_Descr_Reports;
            _commonTask3.TaskHandler += new System.EventHandler(this.selectReports);

            _commonTask4.TaskImage = AppIcons.AppImageCommonTaskSmall(AppIcons.EnumCommonTaskSmall.ConfigureAuditSettingsCTSmall);
            _commonTask4.HoverTaskImage = AppIcons.AppImageCommonTask(AppIcons.EnumCommonTask.ConfigureAuditSettingsHoverCT);
            _commonTask4.TaskText = Utility.Constants.Task_Title_Configure;
            _commonTask4.TaskDescription = Utility.Constants.Task_Descr_Configure;
            _commonTask4.TaskHandler += new System.EventHandler(this.configureDataCollection);

            _commonTask5.TaskImage = AppIcons.AppImageCommonTaskSmall(AppIcons.EnumCommonTaskSmall.ObjectExplorerCTSmall);
            _commonTask5.HoverTaskImage = AppIcons.AppImageCommonTask(AppIcons.EnumCommonTask.ObjectExplorerHoverCT);
            _commonTask5.TaskText = Utility.Constants.Task_Title_Object;
            _commonTask5.TaskDescription = Utility.Constants.Task_Descr_Object;
            _commonTask5.TaskHandler += new System.EventHandler(this.exploreObjectPermissions);

            _commonTask6.TaskImage = AppIcons.AppImageCommonTaskSmall(AppIcons.EnumCommonTaskSmall.SQLsecureLoginCTSmall);
            _commonTask6.HoverTaskImage = AppIcons.AppImageCommonTask(AppIcons.EnumCommonTask.SQLsecureLoginCT);
            _commonTask6.TaskText = Utility.Constants.Task_Title_NewUser;
            _commonTask6.TaskDescription = Utility.Constants.Task_Descr_NewUser;
            _commonTask6.TaskHandler += new System.EventHandler(this.AddNewUser);

            //force to initial disable because there is no security handler at initial construction
            _commonTask1.Enabled =
                _commonTask2.Enabled =
                _commonTask3.Enabled =
                _commonTask4.Enabled =
                _commonTask5.Enabled =
                _commonTask6.Enabled = false;
        }

        #endregion

        #region Helpers

        #region Tasks

        private void registerServer(object sender, EventArgs e)
        {
            Forms.Form_WizardRegisterSQLServer.Process();
        }

        private void configureDataCollection(object sender, EventArgs e)
        {
            Sql.RegisteredServer server = Forms.Form_SelectRegisteredServer.GetServer();

            //if a server was selected, then show the view
            if (server != null)
            {
                Forms.Form_SqlServerProperties.Process(server.ConnectionName,
                                                       Forms.Form_SqlServerProperties.RequestedOperation.
                                                           EditCofiguration, Program.gController.isAdmin);
            }
        }

        private void exploreUserPermissions(object sender, EventArgs e)
        {
            showPermissions(Views.View_PermissionExplorer.Tab.UserPermissions);
        }

        private void exploreObjectPermissions(object sender, EventArgs e)
        {
            showPermissions(Views.View_PermissionExplorer.Tab.ObjectPermissions);
        }

        private void selectReports(object sender, EventArgs e)
        {
            Program.gController.ShowReport(new Utility.NodeTag(new Data.Report(Utility.Constants.RootNode_Reports, false),
                                                                    Utility.View.Main_Reports));
        }

        private void AddNewUser(object sender, EventArgs e)
        {
            Forms.Form_WizardNewLogin.Process();
        }

        #endregion

        #endregion

        #region Event Handlers

        private void View_Main_ExplorePermissions_Enter(object sender, EventArgs e)
        {
            Program.gController.SetMenuConfiguration(m_MenuConfiguration, this);
        }

        private void View_Main_ExplorePermissions_Leave(object sender, EventArgs e)
        {
            Program.gController.SetMenuConfiguration(new Utility.MenuConfiguration());
        }

        #endregion
    }
}

