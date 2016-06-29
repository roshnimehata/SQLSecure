using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Security.Principal;
using Idera.SQLsecure.Core.Accounts;
using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Utility;


namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_GetMissingCredentials : Idera.SQLsecure.UI.Console.Controls.BaseDialogForm
    {
        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Forms.Form_GetMissingCredentials");

        public Form_GetMissingCredentials()
        {
            InitializeComponent();

            button_OK.Enabled = false;
        }


        public string Username
        {
            get { return _textBox_Username.Text; }
        }

        public string Password
        {
            get { return _textBox_Password.Text; }
        }

        private void _textBox_TextChanged(object sender, EventArgs e)
        {
            button_OK.Enabled = !string.IsNullOrEmpty(_textBox_Username.Text)
                                && !string.IsNullOrEmpty(_textBox_Password.Text);
        }


        private void _btn_Help_Click(object sender, EventArgs e)
        {
            string helpTopic = Utility.Help.UpdateMissingCredentials;

            Program.gController.ShowTopic(helpTopic);
        }

        private void Form_PolicyProperties_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            _btn_Help_Click(sender, new EventArgs());
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            bool isCredentialsValid = true;
            WindowsImpersonationContext targetImpersonationContext = null;
            StringBuilder msgBldr = new StringBuilder();
            Forms.ShowWorkingProgress showWorking = new Forms.ShowWorkingProgress();

            // Do a very nominal validation of Username and password

            // Check if the account format is correct.
            if (isCredentialsValid)
            {
                string domain = string.Empty;
                string user = string.Empty;
                Path.SplitSamPath(_textBox_Username.Text, out domain, out user);
                if (string.IsNullOrEmpty(domain) || string.IsNullOrEmpty(user))
                {
                    if (msgBldr.Length > 0) { msgBldr.Append("\n\n"); }
                    msgBldr.Append(Utility.ErrorMsgs.SqlLoginWindowsUserNotSpecifiedMsg);
                    isCredentialsValid = false;
                }
            }

            // Check if Username Password can be validated on local computer
            if(isCredentialsValid)
            {
                showWorking.Show("Verifying SQL Server Credentials...", this);
                try
                {
                    WindowsIdentity wi =
                        Impersonation.GetCurrentIdentity(_textBox_Username.Text, _textBox_Password.Text);
                    targetImpersonationContext = wi.Impersonate();
                }
                catch (Exception ex)
                {
                    if (msgBldr.Length > 0) { msgBldr.Append("\n\n"); }
                    msgBldr.AppendFormat(string.Format("Could not validate the credentials {0}.", _textBox_Username.Text));
                    msgBldr.AppendFormat("\r\nError: {0}", ex.Message);
                    logX.loggerX.Error(string.Format("Error Impersonating {0} for Missing Credentials Check", _textBox_Username.Text, ex));
                    isCredentialsValid = false;
                }
                finally
                {
                    showWorking.Close();
                    Activate();
                    if (targetImpersonationContext != null)
                    {
                        targetImpersonationContext.Undo();
                        targetImpersonationContext.Dispose();
                        targetImpersonationContext = null;
                    }
                }
            }


            if(!isCredentialsValid)
            {
                msgBldr.Append("\r\n\r\n");
                msgBldr.Append("Register Anyway?");
                System.Windows.Forms.DialogResult dr = MsgBox.ShowConfirm(ErrorMsgs.RegisterSqlServerCaption, msgBldr.ToString());
                if (dr == DialogResult.Yes)
                {
                    isCredentialsValid = true;
                }                
                else
                {
                    DialogResult = DialogResult.None;
                }
            }

        }


        public static void Process()
        {
            if(!Program.gController.isAdmin)
            {
                return;
            }
            Form_GetMissingCredentials dlg = new Form_GetMissingCredentials();

            if(dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SQLsecure.UI.Console.Sql.Repository.updateAllCredentials(Program.gController.Repository.ConnectionString, dlg.Username, dlg.Password);
            }
        }

    }
}

