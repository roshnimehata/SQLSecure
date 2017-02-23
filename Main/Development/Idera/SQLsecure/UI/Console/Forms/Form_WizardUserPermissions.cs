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


    public partial class Form_WizardUserPermissions : Form
    {
        #region Constants
        private const string WizardIntroText = @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fswiss\fcharset0 Arial;}}
{\*\generator Msftedit 5.41.15.1507;}\viewkind4\uc1\pard\f0\fs16 Use this wizard to analyze, in detail, the rights and permissions of all users or groups on your SQL Server instance.\par
\par
The permissions explorer allows you to view and analyze all user security settings to determine \b what \b0 a user can do and \b why \b0 they are able to do it.\par
\par
\b What \b0\endash  Determine what a user can do by analyzing Effective permissions.\par
\par
\b Effective permissions\b0  \endash  The resulting permissions after all Assigned Permissions are combined and applied to the affected objects.
\par
\par
\b Why \b0\endash  Determine why a user is able to do what they do by analyzing Assigned permissions.\par
\par
\b Assigned permissions\b0  \endash  Those permissions granted directly or received through group membership as well as all permissions inherited from other server or database objects.
\par
\par
SQLsecure collects all group membership data at both the domain and local level. \par
\par
All users (or groups) are available for selection whether from a direct SQL Server assignment or from a group membership.\par
}";
        private const string WizardFinishTextPrefix = @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fswiss\fcharset0 Arial;}}
{\colortbl ;\red0\green0\blue0;}
{\*\generator Msftedit 5.41.15.1507;}\viewkind4\uc1\pard\cf1\fs20 You are now ready to explore user permissions.\par
Permissions can be viewed on the \par
Summary, Assigned, and Effective tabs.\par
\par
\b Tips:\b0\par
You can drill up and down from user to group and \par
group to user by right clicking on the login name \par
in server logins. \par
\par
Permissions shown in the Effective permissions \par
Tab also provide the exact source of a right by\par
Expanding the permission (click on +)  \cf0\f1\par
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
        public Form_WizardUserPermissions()
        {
            InitializeComponent();

            // Set the intro text in the wizard.
            _rtb_Introduction.Rtf = WizardIntroText;

            // Select the intro page.
            _wizard.SelectedPage = _page_Introduction;

            m_SelectedSnapshotID = 0;
            m_SelectedServer = null;
            m_SelectedDatabase = null;

            m_loginType = null;
            m_SelectedUser = null;

            _radioButton_WindowsUser.Checked = true;
            m_loginType = Sql.LoginType.WindowsLogin;

            InitializeSelectServerPage();

        }
        #endregion

        #region Fields

        private Sql.RegisteredServer m_SelectedServer;
        private int m_SelectedSnapshotID;
        private string m_SelectedDatabase;
        private Sql.User m_SelectedUser;

        private string m_loginType;


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

        public Sql.User SelectedUser
        {
            get { return m_SelectedUser; }
        }

        public string SelectedDatabase
        {
            get { return m_SelectedDatabase; }
        }


        #endregion

        #region Methods

        public static void Process()
        {
            // Display the wizard.
            Form_WizardUserPermissions form = new Form_WizardUserPermissions();
            DialogResult rc = form.ShowDialog();

            // Process if user hit finish.
            if (rc == DialogResult.OK)
            {
                if (form.SelectedServer != null &&
                    form.SelectedUser != null &&
                    form.SelectedSnapshotID != 0 &&
                    form.SelectedDatabase != null)
                {
                    Program.gController.SetCurrentServer(form.SelectedServer);
                    Program.gController.ShowRootView(new Utility.NodeTag(new Data.PermissionExplorer(form.SelectedServer, 
                                                                                                     form.SelectedSnapshotID, 
                                                                                                     form.SelectedUser,
                                                                                                     form.SelectedDatabase,
                                                                                                     Views.View_PermissionExplorer.Tab.UserPermissions),
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

            InitializeSelectUserPage();
            InitializeSelectDatabasePage();
        }

        public void SelectedSnapshotChanged(bool SnapshotSelected)
        {
            _page_SelectSnapshot.AllowMoveNext = SnapshotSelected;
        }

        #endregion

        #region Select User Page

        private void InitializeSelectUserPage()
        {
            SelectUserPage_CheckAllowNext();
        }

        private void SelectUserPage_CheckAllowNext()
        {
            if (!string.IsNullOrEmpty(_textBox_User.Text))
            {
                _Page_SelectUser.AllowMoveNext = true;
            }
            else
            {
                _Page_SelectUser.AllowMoveNext = false;
            }
        }

        private void _Page_SelectUser_BeforeMoveNext(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrEmpty(_textBox_User.Text))
            {
                ValidateUser();

                if (m_SelectedUser != null &&
                    m_SelectedUser.IsVerified == true)
                {
                    _Page_SelectUser.NextPage = _Page_SelectDatabase;
                }
                else
                {
                    _Page_SelectUser.AllowMoveNext = false;
                    e.Cancel = true;
                }
            }
        }

        private void _textBox_User_TextChanged(object sender, EventArgs e)
        {
            m_SelectedUser = null;
            SelectUserPage_CheckAllowNext();
        }

        private void _button_BrowseUsers_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            //Start-SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
            Sql.User user = Forms.Form_SelectUser.GetUser(m_SelectedSnapshotID, m_loginType, m_SelectedServer.ServerType);

            if (user != null)
            {
                _textBox_User.Text = user.Name;
                // DO NOT put this before the textBox update because it clears m_user in the TextChanged event
                m_SelectedUser = user;
            }

            SelectUserPage_CheckAllowNext();
            Cursor = Cursors.Default;
        }

        private void _radioButton_WindowsUser_CheckedChanged(object sender, EventArgs e)
        {

            Cursor = Cursors.WaitCursor;

            if (((RadioButton)sender).Checked)
            {
                if (m_loginType != Sql.LoginType.WindowsLogin)
                {
                    m_SelectedUser = null;
                    // try to be smart and not clear the user if it was not validated to type previously
                    if (m_SelectedUser != null && m_SelectedUser.Sid != null)
                    {
                        _textBox_User.Text = String.Empty;
                    }
                    m_loginType = Sql.LoginType.WindowsLogin;
                }
            }
            Cursor = Cursors.Default;
            SelectUserPage_CheckAllowNext();
        }

        private void _radioButton_SQLLogin_CheckedChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (((RadioButton)sender).Checked)
            {
                if (m_loginType != Sql.LoginType.SqlLogin)
                {
                    m_SelectedUser = null;
                    // try to be smart and not clear the user if it was not validated to type previously
                    if (m_SelectedUser != null && m_SelectedUser.Sid != null)
                    {
                        _textBox_User.Text = String.Empty;
                    }
                    m_loginType = Sql.LoginType.SqlLogin;
                }
            }
            Cursor = Cursors.Default;

        }


        #endregion

        #region Select Database Page

        private void InitializeSelectDatabasePage()
        {
            if (m_SelectedSnapshotID != 0)
            {
                _selectDatabase.RegisterDatabaseChangeDelegate(SelectedDatabaseChanged);
                _selectDatabase.LoadDatabases(m_SelectedSnapshotID);
            }
        }

        // Is database selected or Server Only
        public void SelectedDatabaseChanged(bool DatabaseSelected)
        {
            _Page_SelectDatabase.AllowMoveNext = DatabaseSelected;
        }


        private void _Page_SelectDatabase_BeforeMoveNext(object sender, CancelEventArgs e)
        {
            m_SelectedDatabase = _selectDatabase.SelectedDatabase;
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

        private void ValidateUser()
        {
            bool isCancel = false;

            // Validate User
            if (m_SelectedUser == null)
            {
                // Source: User entered a new user
                //      m_user is set to null only by the user changing the value in the textbox
                _textBox_User.Text = _textBox_User.Text.Trim();
                if (_textBox_User.Text.Length == 0)
                {
                    // there was no user entered, so just alert the user
                    MsgBox.ShowInfo(ErrorMsgs.UserPermissionsCaption, ErrorMsgs.NoUserSelectedMsg);
                }
                else
                {
                    if (m_loginType.Equals(Sql.LoginType.SqlLogin))
                    {
                        // if it is a SQL Login, it will not be validated, so just create the m_user without a sid
                        m_SelectedUser = new Idera.SQLsecure.UI.Console.Sql.User(_textBox_User.Text, null, m_loginType, Sql.User.UserSource.UserEntry);
                        if (m_SelectedUser != null)
                        {
                            m_SelectedUser.IsVerified = true;
                        }
                    }
                    else
                    {
                        // Check the domain for the user
                        m_SelectedUser = Idera.SQLsecure.UI.Console.Sql.User.GetDomainUser(_textBox_User.Text, m_loginType, false);
                        if (m_SelectedUser == null)
                        {
                            // If not found in domain, check the snapshot
                            m_SelectedUser = Sql.User.GetSnapshotWindowsUser(m_SelectedSnapshotID, _textBox_User.Text, false);
                            if (m_SelectedUser == null)
                            {
                                // this user is not found anywhere, so alert the user
                                MsgBox.ShowInfo(ErrorMsgs.UserPermissionsCaption,
                                    string.Format(ErrorMsgs.WindowsUserNotFoundMsg, _textBox_User.Text));
                                //m_user = new Sql.User(_textBox_User.Text, new Sid(new byte[] { 0 }), m_loginType, Sql.User.UserSource.UserEntry);
                            }
                            else
                            {
                                // this user was found only in the snapshot, so alert the user
                                if (DialogResult.No == MsgBox.ShowWarningConfirm(ErrorMsgs.UserPermissionsCaption,
                                                            string.Format(ErrorMsgs.WindowsUserNotFoundDomainMsg, _textBox_User.Text)))
                                {
                                    m_SelectedUser = null;
                                    return;
                                }
                            }
                        }
                        else
                        {
                            isCancel = verifyActiveDirectoryUser();
                        }
                    }
                }
            }
            else
            {
                if (m_SelectedUser.LoginType == Sql.LoginType.SqlLogin)
                {
                    // SqlLogin types are processed by name whether valid or not, so it is valid to try it
                    m_SelectedUser.IsVerified = true;
                }
                else
                {
                    Debug.Assert(m_SelectedUser.LoginType == Sql.LoginType.WindowsLogin);

                    // Source: User entered
                    if (!m_SelectedUser.IsVerified)
                    {
                        switch (m_SelectedUser.Source)
                        {
                            case Sql.User.UserSource.ActiveDirectory:
                                isCancel = verifyActiveDirectoryUser();
                                break;
                            case Sql.User.UserSource.Snapshot:
                                //Validate the user by name
                                Sql.User user = Idera.SQLsecure.UI.Console.Sql.User.GetDomainUser(m_SelectedUser.Name, m_loginType, false);
                                if (user == null)
                                {
                                    //If not found by name, then try by Sid
                                    user = Idera.SQLsecure.UI.Console.Sql.User.GetDomainUser(m_SelectedUser.Sid, m_loginType, false);
                                    if (user == null)
                                    {
                                        //If not found, there is no mismatch but the user has been
                                        //  deleted from AD or can't be verified on a reachable DC)
                                        m_SelectedUser.IsVerified = true;
                                    }
                                    else
                                    {
                                        if (String.Compare(m_SelectedUser.Domain, user.Domain, true) == 0)
                                        {
                                            // this is only found if the domains match
                                            // otherwise, it is probably a well-known account
                                            // which uses the same SID on every machine
                                            MsgBox.ShowWarning(ErrorMsgs.UserPermissionsCaption,
                                                string.Format(ErrorMsgs.UserNameChangedSnapshotMsg, user.Name));
                                        }
                                        m_SelectedUser.IsVerified = true;
                                    }
                                }
                                else
                                {
                                    //The name matched, so verify it is the same user by Sid
                                    if (m_SelectedUser.Sid.Equals(user.Sid) && String.Compare(m_SelectedUser.Domain, user.Domain, true) == 0)
                                    {
                                        m_SelectedUser.IsVerified = true;
                                    }
                                    else
                                    {
                                        //No Sid match, so ask the user which one
                                        switch (MsgBox.ShowQuestion(ErrorMsgs.UserPermissionsCaption, ErrorMsgs.UserNotMatchedSnapshotQuestion))
                                        {
                                            case DialogResult.Yes:
                                                m_SelectedUser.IsVerified = true;
                                                break;
                                            case DialogResult.No:
                                                //set this first because it clears m_user
                                                _textBox_User.Text = user.Name;
                                                m_SelectedUser = user;
                                                m_SelectedUser.IsVerified = true;
                                                break;
                                            case DialogResult.Cancel:
                                                isCancel = true;
                                                break;
                                        }
                                    }
                                }
                                break;
                            case Sql.User.UserSource.UserEntry:
                                Debug.Assert(false, @"Unverified existing user-entered user encountered");
                                //How did we get here ???
                                break;
                            default:

                                break;
                        }
                    }
                }
            }
        }

        protected bool verifyActiveDirectoryUser()
        {
            bool isCancel = false;

            // verify user is in the snapshot
            Sql.User user = Sql.User.GetSnapshotWindowsUser(m_SelectedSnapshotID, m_SelectedUser.Name, false);
            if (user == null)
            {
                user = Sql.User.GetSnapshotWindowsUser(m_SelectedSnapshotID, m_SelectedUser.Sid, false);
                if (user == null)
                {
                    // not found, but no conflict so process on through
                    m_SelectedUser.IsVerified = true;
                }
                else
                {
                    MsgBox.ShowWarning(ErrorMsgs.UserPermissionsCaption,
                        string.Format(ErrorMsgs.UserNameChangedADMsg, m_SelectedUser.Name));
                    m_SelectedUser.IsVerified = true;
                }
            }
            else
            {
                //The name matched, so verify it is the same user by Sid
                if (m_SelectedUser.Sid.Equals(user.Sid) && String.Compare(m_SelectedUser.Domain, user.Domain, true) == 0)
                {
                    m_SelectedUser.IsVerified = true;
                }
                else
                {
                    //No Sid match, so ask the user which one
                    switch (MsgBox.ShowQuestion(ErrorMsgs.UserPermissionsCaption, ErrorMsgs.UserNotMatchedADQuestion))
                    {
                        case DialogResult.Yes:
                            m_SelectedUser.IsVerified = true;
                            break;
                        case DialogResult.No:
                            //set this first because it clears m_user
                            _textBox_User.Text = user.Name;
                            m_SelectedUser = user;
                            m_SelectedUser.IsVerified = true;
                            break;
                        case DialogResult.Cancel:
                            isCancel = true;
                            break;
                    }
                }
            }

            return isCancel;
        }


        #endregion

        private void _page_SelectSnapshot_BeforeDisplay(object sender, EventArgs e)
        {

        }

        private void _Page_SelectDatabase_BeforeDisplay(object sender, EventArgs e)
        {

        }

        private void _Page_SelectUser_BeforeDisplay(object sender, EventArgs e)
        {

        }


        private void ShowHelpTopic()
        {
            Program.gController.ShowTopic(Utility.Help.UserPermissionWizardTopic);
        }

        private void Form_WizardUserPermissions_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            ShowHelpTopic();
        }

        private void Form_WizardUserPermissions_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            ShowHelpTopic();
        }


    }
}