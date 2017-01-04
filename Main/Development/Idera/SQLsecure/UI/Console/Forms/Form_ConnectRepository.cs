using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Utility;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_ConnectRepository : Idera.SQLsecure.UI.Console.Controls.BaseDialogForm
    {
        #region Ctors

        bool isConnect = true;
        string button_value = "Connect";
        int button_index = 0;
        bool areCredentialsRequired = false;
        public Form_ConnectRepository(bool isConnect = true)
        {
            this.isConnect = isConnect;
            //Show button text as per Connect/Deploy Repository
            if(isConnect == false)
            {
                button_value = "Deploy";
                button_index = 1;
            }
            InitializeComponent();
        }

        #endregion

        #region Fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Forms.Form_ConnectRepository");
        private string m_User = "";
        private string m_Password = "";

        #endregion

        #region Properties

        public string Server
        {
            get { return _textBox_Server.Text; }
            set { _textBox_Server.Text = value; }
        }

        public string User
        {
            get { return m_User; }
        }

        public string Password
        {
            get { return m_Password; }
        }

        #endregion

        #region Events

        private void _button_Lookup_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            Form_SelectServer dlg = new Form_SelectServer();

            try
            {
                if (dlg.LoadServers())
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        _textBox_Server.Text = dlg.SelectedServer;
                    }
                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error(string.Format(Utility.ErrorMsgs.ErrorStub, Utility.ErrorMsgs.CantValidateRepository), ex);
                MsgBox.ShowError(Utility.ErrorMsgs.CantValidateRepository, ex);
            }
            this.Cursor = Cursors.Default;
        }

        private void _textBox_Server_TextChanged(object sender, EventArgs e)
        {
            if (_textBox_Server.Text.Trim().Length == 0)
            {
                _button_OK.Enabled = false;
            }
            else
            {
                _button_OK.Enabled = true;
            }
        }

        private void _button_OK_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            MainForm.Server_Name = this._textBox_Server.Text;
            if (areCredentialsRequired)
            {
                MainForm.UserName = this.username.Text;
                MainForm.Password = this.password.Text;
            }
        }

        #endregion

        private void ultraButton_Help_Click(object sender, EventArgs e)
        {
            ShowHelp();
        }

        private void Form_ConnectRepository_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            ShowHelp();
        }

        private void ShowHelp()
        {
            Program.gController.ShowTopic(Utility.Help.ConnectRepositoryHelpTopic);
        }

        //SQLSecure3.1 (Mitul Kapoor) - update button based on user selection of radio buttons
        private void Action_choice_ValueChanged(object sender, System.EventArgs e)
        {
            var selection = sender as Infragistics.Win.UltraWinEditors.UltraOptionSet;
            if (selection.CheckedItem.DataValue.Equals("Connect"))
            {
                this._button_OK.Text = "Connect";
                MainForm.isConnect = true;
                this.Text = "Connect to Repository";
                this.Description = "Connect to SQLsecure Repository";
            }
            else
            {
                this._button_OK.Text = "Deploy";
                MainForm.isConnect = false;
                this.Text = "Deploy Repository";
                this.Description = "Designate a database instance that will host the SQLsecure Repository. Select SQL Server and then select the appropriate options to install the Repository.";
            }
          }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(areCredentialsRequired == false)
            {
                this.username.Enabled = true;
                this.password.Enabled = true;
                areCredentialsRequired = true;
            }
            else
            {
                this.username.Enabled = false;
                this.password.Enabled = false;
                areCredentialsRequired = false;
            }

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
