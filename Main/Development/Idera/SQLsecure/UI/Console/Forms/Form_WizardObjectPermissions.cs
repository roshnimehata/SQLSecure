using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using Idera.SQLsecure.UI.Console.Utility;

namespace Idera.SQLsecure.UI.Console.Forms
{


    public partial class Form_WizardObjectPermissions : Form
    {
        #region Constants
        private const string WizardIntroText = @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fswiss\fcharset0 Arial;}}
{\*\generator Msftedit 5.41.15.1507;}\viewkind4\uc1\pard\f0\fs18 The SQLsecure object browser allows you to view security settings so you can evaluate your SQL Server security model and determine \b what \b0 it is and \b why \b0 it works that way:\par
\par
\b What  \b0 is represented in the Explicit permissions analysis.  \par
\par
\b Explicit permissions\b0  \endash  Those permissions created directly on an object.\par
\par
\b Why \b0 is reflected in the All permissions analysis.\par
\par
\b All permissions\b0  \endash  All permissions that apply to an object including any granted to higher level server or database objects or roles which can affect the selected object.
\par
}";
        private const string WizardFinishTextPrefix = @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fswiss\fcharset0 Arial;}}
{\*\generator Msftedit 5.41.15.1507;}\viewkind4\uc1\pard\f0\fs20 You are now ready to explore object permissions.\par
\par
Permissions can be viewed for every object collected in the snapshot. \par
\par
\b Tips:\b0\par
Drill down through the tree and select an object \par
\par
View in window or right click to view properties and permissions\par
}";
        private const string WizardFinishTextSQLLogin = @"Windows user\tab : ";
        private const string WizardFinishTextAccess = @"\par
Access\tab\tab : 
";
        private const string WizardFinishTextPermission = @"\par
Permission\tab : 
";
        private const string WizardFinishTextSuffix = @"\par
\pard\tx720\par
\pard\f1 This Windows user will now be added as a SQL Server login.\f2\fs20\par
}";
        #endregion

        #region CTOR
        public Form_WizardObjectPermissions()
        {
            InitializeComponent();

            // Set the intro text in the wizard.
            _rtb_Introduction.Rtf = WizardIntroText;

            // Select the intro page.
            _wizard.SelectedPage = _page_Introduction;

            m_SelectedSnapshotID = 0;
            m_SelectedServer = null;

            InitializeSelectServerPage();
            InitializeSelectSnapshotPage();             

        }
        #endregion

        #region Fields

        private Sql.RegisteredServer m_SelectedServer;
        private int m_SelectedSnapshotID;

        #endregion

        #region Properties

        public Sql.RegisteredServer SelectedServer
        {
            get { return m_SelectedServer; }
        }

        public int SelectedSnapshotID
        {
            get { return m_SelectedSnapshotID; }
        }


        #endregion

        #region Methods

        public static void Process()
        {
            // Display the wizard.
            Form_WizardObjectPermissions form = new Form_WizardObjectPermissions();
            DialogResult rc = form.ShowDialog();

            // Process if user hit finish.
            if (rc == DialogResult.OK)
            {
                if (form.SelectedServer != null &&
                    form.SelectedSnapshotID != 0)
                {
                    Program.gController.SetCurrentServer(form.SelectedServer);
                    Program.gController.ShowRootView(new Utility.NodeTag(new Data.PermissionExplorer(form.SelectedServer, 
                                                                                                     form.SelectedSnapshotID, 
                                                                                                     Views.View_PermissionExplorer.Tab.ObjectPermissions),
                                                                    Utility.View.PermissionExplorer));
                }
             
            }
        }

        #endregion

        #region Events
        

        #endregion


        #region Server Page

        private void InitializeSelectServerPage()
        {
            _page_SelectServer.AllowMoveNext = false;
            _selectServer.RegisterUserChangeDelegate(SelectedUserChanged);
            _selectServer.LoadServers(true);
        }

        private void _page_SelectServerPage_BeforeDisplay(object sender, EventArgs e)
        {
        }

        private void _page_SelectServerPage_BeforeMoveNext(object sender, CancelEventArgs e)
        {
            m_SelectedServer = _selectServer.SelectedServer;

            InitializeSelectSnapshotPage();
        }

        public void SelectedUserChanged(bool UserSelected)
        {
            _page_SelectServer.AllowMoveNext = UserSelected;
        }


        #endregion

        #region Select Snapshot Page

        private void InitializeSelectSnapshotPage()
        {
            _page_SelectSnapshot.AllowMoveNext = false;
            if (m_SelectedServer != null)
            {
                _selectSnapshot.RegisterSnapshotChangeDelegate(SelectedSnapshotChanged);
                _selectSnapshot.LoadSnapshots(m_SelectedServer);
            }
        }

        private void _page_SelectSnapshot_BeforeMoveNext(object sender, CancelEventArgs e)
        {
            m_SelectedSnapshotID = _selectSnapshot.SelectedSnapshotId;

        }

        public void SelectedSnapshotChanged(bool SnapshotSelected)
        {
            _page_SelectSnapshot.AllowMoveNext = SnapshotSelected;
        }


        #endregion


        #region Finish_Page

        private string buildWizardFinishSummary()
        {
            StringBuilder summary = new StringBuilder();
            summary.Append(WizardFinishTextPrefix);
//            summary.Append(WizardFinishTextSQLLogin);
//            summary.Append(login);
//            summary.Append(WizardFinishTextAccess);
//            summary.Append(access);
//            summary.Append(WizardFinishTextPermission);
//            summary.Append(permission);
//            summary.Append(WizardFinishTextSuffix);
            return summary.ToString();
        }

        private void _page_Finish_BeforeDisplay(object sender, EventArgs e)
        {

            _rtb_Finish.Rtf = buildWizardFinishSummary();
        }

        private void _page_Finish_BeforeMoveBack(object sender, CancelEventArgs e)
        {
        }

        #endregion

        private void _page_Finish_CollectSettings(object sender, Divelements.WizardFramework.WizardFinishPageEventArgs e)
        {

        }




        #region Helpers


        #endregion

        private void _page_SelectSnapshot_BeforeDisplay(object sender, EventArgs e)
        {

        }

        private void _Page_SelectDatabase_BeforeDisplay(object sender, EventArgs e)
        {

        }


        private void ShowHelpTopic()
        {
            Program.gController.ShowTopic(Utility.Help.ObjectPermissionWizardTopic);
        }

        private void Form_WizardObjectPermissions_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            ShowHelpTopic();
        }

        private void Form_WizardObjectPermissions_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            ShowHelpTopic();
        }


    }
}