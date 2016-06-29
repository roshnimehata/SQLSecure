using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Idera.SQLsecure.UI.Console.Forms
{


    public partial class Form_WizardNewLogin : Form
    {
        #region Constants
        private const string WizardIntroText = @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fswiss\fcharset0 Microsoft Sans Serif;}{\f1\fnil\fcharset2 Symbol;}}
{\*\generator Msftedit 5.41.15.1507;}\viewkind4\uc1\pard\f0\fs16 This wizard allows you to add a new SQL Server login for the purposes of using SQLsecure. With this wizard you will:\par
\par
\pard{\pntext\f1\'B7\tab}{\*\pn\pnlvlblt\pnf1\pnindent360{\pntxtb\'B7}}\fi-360\li720\tx720 Add a new SQL Server login\par
{\pntext\f1\'B7\tab}Set the permissions level\par
}";
        private const string WizardFinishTextPrefix = @"{\rtf1\ansi\ansicpg1252\deff0{\fonttbl{\f0\fswiss\fcharset0 Microsoft Sans Serif;}{\f1\fswiss\fprq2\fcharset0 Microsoft Sans Serif;}{\f2\fswiss\fcharset0 Arial;}}
{\*\generator Msftedit 5.41.15.1507;}\viewkind4\uc1\pard\lang1033\f0\fs16 You have entered the following data for adding a new SQL Server login to SQLsecure.\par
\par
";
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

        #region Fields

        private string m_loginName = string.Empty;
        private bool m_bAccess = false;
        private bool m_bPermission = false;
        #endregion

        #region CTOR
        public Form_WizardNewLogin()
        {
            InitializeComponent();

            // Set the intro text in the wizard.
            _rtb_Introduction.Rtf = WizardIntroText;

            // Select the intro page.
            _wizard.SelectedPage = _page_Introduction;

            _rdbtn_GrantAccess.Checked = true;
            _rdbtn_No.Checked = true;

        }
        #endregion

        #region Methods

        public static string Process()
        {
            string loginName = null;
            // Display the wizard.
            Form_WizardNewLogin form = new Form_WizardNewLogin();
            DialogResult rc = form.ShowDialog();

            // Process if user hit finish.
            if (rc == DialogResult.OK)
            {
                string StoredProcName = form.m_bAccess ? "sp_grantlogin" : "sp_denylogin";
                Sql.Repository.addLogin(StoredProcName, form.m_loginName);

                if (form.m_bPermission)
                {
                    Sql.Repository.AddToRole(form.m_loginName, "sysadmin");
                }
                loginName = form.m_loginName;
            }
            return loginName;
        }

        #endregion

        #region Events
        
        private void _wizard_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            string helpTopic = Utility.Help.RegisterNewLoginWizardHelpTopic;
            if (_page_Introduction.Visible)
                helpTopic = Utility.Help.RegisterNewLoginWizardHelpTopic;
            else if (_page_Login.Visible)
                helpTopic = Utility.Help.RegisterNewLoginWizardSpecifyLoginHelpTopic;
            else if (_page_Credentials.Visible)
                helpTopic = Utility.Help.RegisterNewLoginWizardPermissionsHelpTopic;
            else if (_page_Finish.Visible)
                helpTopic = Utility.Help.RegisterNewLoginWizardFinishHelpTopic;

            Program.gController.ShowTopic(helpTopic);
        }

        #endregion


        #region Login Page

        private void checkAllowNext()
        {
            bool bAllowNext = true;
            if (String.IsNullOrEmpty(_edit_login_Name.Text))
            {
                bAllowNext = false;
            }

            _page_Login.AllowMoveNext = bAllowNext;

        }

        private void _page_Login_BeforeDisplay(object sender, EventArgs e)
        {
            checkAllowNext();
        }

        private void _edit_login_Name_TextChanged(object sender, EventArgs e)
        {
            checkAllowNext();
        }

        private void _rdbtn_DenyAccess_CheckedChanged(object sender, EventArgs e)
        {
            if( _rdbtn_DenyAccess.Checked == false )
            {
                _rdbtn_No.Checked = true;
                _rdbtn_Yes.Checked = false;
            }
        }

        private void _page_Login_BeforeMoveNext(object sender, CancelEventArgs e)
        {
            bool isOK = true;
            // Check valid user name
            //if( _edit_login_Name.Text )

            if (_rdbtn_DenyAccess.Checked == true)
            {
                _page_Login.NextPage = _page_Finish;
            }
            else
            {
                _page_Login.NextPage = _page_Credentials;
            }

            if (!isOK)
            {
                e.Cancel = true;
            }

        }

        #endregion

        #region Finish_Page

        private string buildWizardFinishSummary(
                string login,
                string access,
                string permission
            )
        {
            StringBuilder summary = new StringBuilder();
            summary.Append(WizardFinishTextPrefix);
            summary.Append(WizardFinishTextSQLLogin);
            summary.Append(login);
            summary.Append(WizardFinishTextAccess);
            summary.Append(access);
            summary.Append(WizardFinishTextPermission);
            summary.Append(permission);
            summary.Append(WizardFinishTextSuffix);
            return summary.ToString();
        }

        private void _page_Finish_BeforeDisplay(object sender, EventArgs e)
        {
            string loginName;
            string access;
            string permission;

            loginName = _edit_login_Name.Text.Replace("\\", "\\\\");

            access = (_rdbtn_GrantAccess.Checked) ? "grant access" : "deny access";

            permission = (_rdbtn_Yes.Checked) ? "user can configure SQLsecure" : "user can only view SQLsecure results";

            _rtb_Finish.Rtf = buildWizardFinishSummary(loginName, access, permission);
        }

        private void _page_Finish_BeforeMoveBack(object sender, CancelEventArgs e)
        {
            if (_rdbtn_DenyAccess.Checked == true)
            {
                _page_Finish.PreviousPage = _page_Login;
            }
            else
            {
                _page_Finish.PreviousPage = _page_Credentials;
            }

        }

        #endregion

        private void _page_Finish_CollectSettings(object sender, Divelements.WizardFramework.WizardFinishPageEventArgs e)
        {
            m_loginName = _edit_login_Name.Text;

            m_bAccess = _rdbtn_GrantAccess.Checked ? true : false;

            m_bPermission = _rdbtn_Yes.Checked ? true : false;
        }

        private void _page_Credentials_BeforeDisplay(object sender, EventArgs e)
        {

        }

        private void _page_Finish_BeforeMoveNext(object sender, CancelEventArgs e)
        {
                if (DialogResult.No ==
                        Utility.MsgBox.ShowWarningConfirm(Utility.ErrorMsgs.AddLoginCaption,
                                                    string.Format(Utility.ErrorMsgs.AddLoginWarningMsg,
                                                                    m_bAccess ? Utility.ErrorMsgs.LoginGrantAccessMsg : Utility.ErrorMsgs.LoginRevokeAccessMsg,
                                                                    m_bPermission ? Utility.ErrorMsgs.LoginSysadminAccessMsg : Utility.ErrorMsgs.LoginNotSysadminAccessMsg,
                                                                    m_loginName)))
                {
                    e.Cancel = true;
                }
        }
    }
}