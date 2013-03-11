using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_LoginProperties : Idera.SQLsecure.UI.Console.Controls.BaseDialogForm
    {
        private string m_loginName;
        private bool m_grantaccess;
        private bool m_hasPermissions;


        public Form_LoginProperties()
        {
            InitializeComponent();

            this.MinimumSize = this.Size;
        }

        private void SetInitialValues(string name, string access, string permissions)
        {
            m_loginName = name;
            m_grantaccess = (access == Utility.Logins.Login_Permit) ? true : false;
            m_hasPermissions = (permissions == Utility.Logins.Login_CanConfigure) ? true : false;            
        }

        public static void Process(string name, string access, string permissions)
        {

            Form_LoginProperties form = new Form_LoginProperties();
            form.SetInitialValues(name, access, permissions);

            if (form.ShowDialog() == DialogResult.OK)
            {
                string StoredProcName = form.m_grantaccess ? "sp_grantlogin" : "sp_denylogin";
                Sql.Repository.addLogin(StoredProcName, form.m_loginName);

                if (form.m_hasPermissions)
                {
                    Sql.Repository.AddToRole(form.m_loginName, "sysadmin");
                }
                else
                {
                    Sql.Repository.RemoveFromRole(form.m_loginName, "sysadmin");
                }
            }
        }


        private void button_OK_Click(object sender, EventArgs e)
        {
            m_grantaccess = radioButton_GrantAccess.Checked;
            m_hasPermissions = _rdbtn_Yes.Checked;

            if (DialogResult.Yes == 
                Utility.MsgBox.ShowWarningConfirm(Utility.ErrorMsgs.SaveLoginCaption,
                                                    string.Format(Utility.ErrorMsgs.SaveLoginWarningMsg,
                                                                    m_grantaccess ? Utility.ErrorMsgs.LoginGrantAccessMsg : Utility.ErrorMsgs.LoginRevokeAccessMsg,
                                                                    m_hasPermissions ? Utility.ErrorMsgs.LoginSysadminAccessMsg : Utility.ErrorMsgs.LoginNotSysadminAccessMsg,
                                                                    m_loginName)))
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                DialogResult = DialogResult.None;
            }
        }

        private void Form_LoginProperties_Shown(object sender, EventArgs e)
        {
            textBox_LoginName.Text = m_loginName;

            if (m_hasPermissions)
            {
                _rdbtn_No.Checked = false;
                _rdbtn_Yes.Checked = true;
            }
            else
            {
                _rdbtn_No.Checked = true;
                _rdbtn_Yes.Checked = false;
            }

            if (m_grantaccess)
            {
                radioButton_DenyAccess.Checked = false;
                radioButton_GrantAccess.Checked = true;
            }
            else
            {
                radioButton_DenyAccess.Checked = true;
                radioButton_GrantAccess.Checked = false;
            }
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void radioButton_GrantAccess_CheckedChanged(object sender, EventArgs e)
        {
            _rdbtn_Yes.Enabled = true;
            _rdbtn_No.Enabled = true;
        }

        private void radioButton_DenyAccess_CheckedChanged(object sender, EventArgs e)
        {
            _rdbtn_Yes.Enabled = false;
            _rdbtn_No.Enabled = false;
            _rdbtn_No.Checked = true;
        }

        private void ultraButton_Help_Click(object sender, EventArgs e)
        {
            ShowHelp();
        }

        private void Form_LoginProperties_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            ShowHelp();
        }

        private void ShowHelp()
        {
            Program.gController.ShowTopic(Utility.Help.LoginPropertiesHelpTopic);
        }
    }
}