using System;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinEditors;
using System.ComponentModel;

using Idera.SQLsecure.Core.Logger;


namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_SmtpEmailProviderConfig : Idera.SQLsecure.UI.Console.Controls.BaseDialogForm
    {
        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Forms.Form_WizardRegisterSQLServer");

        public Form_SmtpEmailProviderConfig(bool allowEdit)
        {
            InitializeComponent();
            okButton.Enabled = false;

            if(!allowEdit)
            {
                controlSMTPEmailConfig1.Enabled = false;
                testButton.Enabled = false;
            }
        }

        static public void Process(bool allowEdit)
        {
            Form_SmtpEmailProviderConfig dlg = new Form_SmtpEmailProviderConfig(allowEdit);
            bool createNew = !Program.gController.Repository.NotificationProvider.IsValid();
            
            dlg.controlSMTPEmailConfig1.RegisterDataOKDelegate(dlg.UpdateOKButton);
            dlg.controlSMTPEmailConfig1.InitializeControl(Program.gController.Repository.NotificationProvider);

            if(dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (createNew)
                {
                    dlg.controlSMTPEmailConfig1.GetNotificationProvider().AddNotificationProvider(Program.gController.Repository.ConnectionString);
                }
                else
                {
                    dlg.controlSMTPEmailConfig1.GetNotificationProvider().UpdateNotificationProvider(Program.gController.Repository.ConnectionString);
                }
                Program.gController.Repository.RefreshNotificationProvider();
            }
        }
       
        private void UpdateOKButton(bool enable)
        {
            okButton.Enabled = enable;
        }

        protected override void OnHelpRequested(HelpEventArgs hevent) {
            ShowHelpTopic();
        }

        private void ShowHelpTopic()
        {
            Program.gController.ShowTopic(Utility.Help.ConfigureEmailProviderHelpTopic);
        }

        private void testButton_Click(object sender, EventArgs args)
        {
            controlSMTPEmailConfig1.SendTestEmail();
        }

        private void helpButton_Click(object sender, EventArgs e)
        {
            ShowHelpTopic();
        }
    }
}
