using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Net.Mail;

using Idera.SQLsecure.Core.Accounts;
using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Forms;


namespace Idera.SQLsecure.UI.Console.Controls
{
    public partial class controlSMTPEmailConfig : UserControl
    {
        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Forms.Form_WizardRegisterSQLServer");

        public delegate void DataOK(bool ok);
        DataOK m_DataOKDelegate = null;
        
        private bool senderInfoChanged = false;
        private NotificationProvider m_provider = null;

        public controlSMTPEmailConfig()
        {
            InitializeComponent();
        }

        public void RegisterDataOKDelegate(DataOK value)
        {
            m_DataOKDelegate += value;
        }

        public NotificationProvider GetNotificationProvider()
        {
            updateProviderInfo();
            return m_provider;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            timeoutEditor.Value = timeoutSlider.Value;
            updateTimeoutLabel();
        }

        private void timeoutEditor_ValueChanged(object sender, EventArgs e)
        {
            if (timeoutEditor.Value == DBNull.Value)
                return;

            timeoutSlider.Value = (int)Convert.ChangeType(timeoutEditor.Value, typeof(int));
        }

        private void updateTimeoutLabel()
        {
            TimeSpan timeout = TimeSpan.FromSeconds(timeoutSlider.Value);
            int minutes = timeout.Minutes;
            int seconds = timeout.Seconds;

            StringBuilder builder = new StringBuilder();

            if (minutes > 0)
            {
                builder.AppendFormat("{0} minute", minutes);
                if (minutes > 1)
                    builder.Append('s');
                builder.Append(' ');
            }
            if (seconds > 0)
            {
                builder.AppendFormat("{0} second", seconds);
                if (seconds > 1)
                    builder.Append('s');
            }

            timeoutLabel.Text = builder.ToString();
        }

        public void InitializeControl(NotificationProvider provider)
        {
            m_provider = provider;
            textbox_ServerAddress.Text = provider.ServerName;
            ultraNumeric_ServerPort.Value = GetSafeValue(provider.Port, 25);
            timeoutEditor.Value = GetSafeValue(provider.Timeout, 90);
            checkbox_RequiresAuthentication.Checked = GetSafeValue(provider.RequiresAuthentication, false);
            if (provider.RequiresAuthentication)
            {
                textbox_LogonName.Text = GetSafeValue(provider.UserName, "");
                textbox_Password.Text = GetSafeValue(provider.Password, "");
            }

            textbox_FromAddress.Text = GetSafeValue(provider.SenderEmailAddress, "");
            textbox_FromName.Text = GetSafeValue(provider.SenderName, "");

            UpdateAuthenticationControls();

            senderInfoChanged = false;
        }


        private T GetSafeValue<T>(object value, T defaultValue)
        {
            if (value == null)
                return defaultValue;
            return (T)value;
        }

        private void updateProviderInfo()
        {
            m_provider.ServerName = textbox_ServerAddress.Text;
            m_provider.Port = (int)ultraNumeric_ServerPort.Value;
            object tev = timeoutEditor.Value;
            m_provider.Timeout = tev == null ? 90 : (int)Convert.ChangeType(tev, typeof (int));
            m_provider.RequiresAuthentication = checkbox_RequiresAuthentication.Checked;
            if (m_provider.RequiresAuthentication)
            {
                m_provider.UserName = textbox_LogonName.Text;
                m_provider.Password = textbox_Password.Text;
            }

            if (!GetSafeValue(m_provider.SenderEmailAddress, "").Equals(textbox_FromAddress.Text.Trim()) ||
                !GetSafeValue(m_provider.SenderName, "").Equals(textbox_FromName.Text.Trim()))
            {
                senderInfoChanged = true;
            }
            m_provider.SenderEmailAddress = textbox_FromAddress.Text;
            m_provider.SenderName = textbox_FromName.Text;
        }

        
        public void SendTestEmail()
        {
            updateProviderInfo();

            Forms.Form_TestSMTPEmail tse = new Form_TestSMTPEmail();
            if (tse.ShowDialog() == DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;
                try
                {                   
                    m_provider.SendMessage(tse.Recepient, "SQLsecure Notification Test", "It worked!");
                }
                catch (Exception e)
                {
                    Utility.MsgBox.ShowError("Failed to send test email. ", e);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void requiresAuthentication_CheckedChanged(object sender, EventArgs e)
        {
            UpdateAuthenticationControls();
        }

        private void UpdateAuthenticationControls()
        {
            bool check = checkbox_RequiresAuthentication.Checked;
            textbox_LogonName.Enabled = check;
            textbox_Password.Enabled = check;
            if (check)
                textbox_LogonName.Focus();
            UpdateControls();
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void SmtpProviderConfigDialog_Load(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void UpdateControls()
        {
            bool OK = true;

            if (OK && String.IsNullOrEmpty(textbox_ServerAddress.Text))
            {
                OK = false;
            }
            if (OK && checkbox_RequiresAuthentication.Checked)
            {
                if (String.IsNullOrEmpty(textbox_LogonName.Text))
                {
                    OK = false;
                }
                else
                    if (String.IsNullOrEmpty(textbox_Password.Text))
                    {
                        OK = false;
                    }
            }

            string from = textbox_FromName.Text.Trim();
            from = (from.Length == 0) ? textbox_FromAddress.Text : String.Format("{0}<{1}>", from, textbox_FromAddress.Text);
            if (OK && (String.IsNullOrEmpty(from) || !NotificationProvider.IsMailAddressValid(from, true)))
            {
                OK = false;
            }

            if (m_DataOKDelegate != null)
            {
                m_DataOKDelegate(OK);
            }

        }

       

    }
}
