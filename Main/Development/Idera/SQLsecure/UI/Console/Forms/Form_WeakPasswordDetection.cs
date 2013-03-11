using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Idera.SQLsecure.Core.Accounts;
using Idera.SQLsecure.Core.Logger;
using System.Reflection;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_WeakPasswordDetection : Idera.SQLsecure.UI.Console.Controls.BaseDialogForm
    {
        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Forms.Form_WeakPasswordDetection");
        private WeakPasswordSetting passwordSettings;

        public WeakPasswordSetting PasswordSettings
        {
            get { return passwordSettings; }
            set { passwordSettings = value; }
        }

        public bool PasswordCheckingEnabled
        {
            get { return this.enableCheck.Checked; }
        }

        public Form_WeakPasswordDetection(bool allowEdit, WeakPasswordSetting passwordSettings)
        {
            InitializeComponent();

            _bf_MainPanel.Enabled = allowEdit;
            this.passwordSettings = passwordSettings;
            enableCheck.Checked = passwordSettings.PasswordCheckingEnabled;
            additionalListTextbox.Text = WeakPasswordSetting.ConvertListToString(passwordSettings.AdditionalPasswordList);

            if (passwordSettings.AdditionalListUpdated == DateTime.MinValue)
                additionalUpdatedLabel.Text = "Last Updated: Never";
            else
                additionalUpdatedLabel.Text = String.Format("{0}", passwordSettings.AdditionalListUpdated.ToLocalTime().ToString());

            if (passwordSettings.CustomListUpdated == DateTime.MinValue)
                customUpdatedLabel.Text = "Last Updated: Never";
            else
                customUpdatedLabel.Text = String.Format("Last Updated: {0}",  passwordSettings.CustomListUpdated.ToLocalTime().ToString());
        }

        private void enableCheck_CheckedChanged(object sender, EventArgs e)
        {
            settingsGroupBox.Enabled = enableCheck.Checked;
            passwordSettings.PasswordCheckingEnabled = enableCheck.Checked;
        }

        static public void Process(bool allowEdit)
        {
            List<WeakPasswordSetting> passwordSettings = WeakPasswordSetting.GetWeakPasswordSettings(Program.gController.Repository.ConnectionString);

            //right now there will only be one custom password list but that might change if we allow for each server to have a custom list.
            Form_WeakPasswordDetection dlg = new Form_WeakPasswordDetection(allowEdit, passwordSettings[0]);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                WeakPasswordSetting.UpdateWeakPasswordSettings(dlg.PasswordSettings, Program.gController.Repository.ConnectionString);
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            //only update the list if it has changed.
            if (!String.Equals(additionalListTextbox.Text, WeakPasswordSetting.ConvertListToString(passwordSettings.AdditionalPasswordList)))
            {
                passwordSettings.AdditionalPasswordList.Clear();
                passwordSettings.AdditionalPasswordList.AddRange(additionalListTextbox.Text.Split(new char[] { ';' }));
                passwordSettings.AdditionalListUpdated = DateTime.Now.ToUniversalTime();
            }

            string filename = customListTextbox.Text;

            if (!String.IsNullOrEmpty(filename))
            {
                try
                {
                    DialogResult result = DialogResult.Yes;

                    if (passwordSettings.CustomPasswordList.Count > 0)
                    {
                        result = Utility.MsgBox.ShowConfirm("Override Custom Password List", String.Format("Uploading a new custom password list will override {0} existing custom passwords.  Do you wish to continue?", passwordSettings.CustomPasswordList.Count));
                    }

                    if (result == DialogResult.Yes)
                    {
                        StreamReader fileStream;

                        //copy the file to the same directory as the console and collector
                        using (fileStream = File.OpenText(filename))
                        {
                            //empty out the old custom list
                            passwordSettings.CustomPasswordList.Clear();
                            string password;

                            while ((password = fileStream.ReadLine()) != null)
                            {
                                passwordSettings.CustomPasswordList.Add(password);
                            }
                        }
                        Utility.MsgBox.ShowInfo("Upload Successful", "Uploading of the custom password list was successful.");
                        passwordSettings.CustomListUpdated = DateTime.Now.ToUniversalTime();
                    }
                    else
                    {
                        this.DialogResult = DialogResult.None;
                        return;
                    }
                }
                catch (Exception exception)
                {
                    Utility.MsgBox.ShowError("Unable to load", "Unable to load the custom password list.", exception);
                    logX.loggerX.Error(String.Format("Unable to load the custom password list.  Error:{0}", exception.ToString()));
                    this.DialogResult = DialogResult.None;
                    return;
                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void helpButton_Click(object sender, EventArgs e)
        {
            ShowHelpTopic();
        }

        private void Form_WeakPasswordDetection_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            ShowHelpTopic();
        }
    
        private void ShowHelpTopic()
        {
            Program.gController.ShowTopic(Utility.Help.ConfigureEmailProviderHelpTopic);
        }

        private void viewPasswordsLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            Form_PasswordList listDisplay = new Form_PasswordList("Default Password List", LoadDefaultPasswordList());
            this.Cursor = Cursors.Default;
            listDisplay.ShowDialog(this);
        }

        private List<string> LoadDefaultPasswordList()
        {
            List<string> defaultPasswordList = new List<string>();

            try
            {
                //read the passwords from the file.
                Assembly assembly = Assembly.Load("Idera.SQLsecure.Core.Accounts");
                using (Stream stream = assembly.GetManifestResourceStream("Idera.SQLsecure.Core.Accounts.PasswordList.PasswordList.txt"))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string password;

                        while ((password = reader.ReadLine()) != null)
                        {
                            defaultPasswordList.Add(password);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error(String.Format("Unable to load the default password list.  Error:{0}", ex.ToString()));
            } 
            return defaultPasswordList;
        }

        private void addCustomListButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog passwordFile = new OpenFileDialog();
            passwordFile.Filter = "txt files (*.txt)|*.txt";
            passwordFile.FilterIndex = 0;
            passwordFile.CheckFileExists = true;
            passwordFile.Multiselect = false;
            passwordFile.InitialDirectory = Environment.SpecialFolder.MyDocuments.ToString();
            passwordFile.Title = "Select a password file";

            if (passwordFile.ShowDialog() == DialogResult.OK)
            {
                string filename = passwordFile.FileName;

                if (File.Exists(filename))
                {
                    customListTextbox.Text = filename;
                }
            }
        }

        private void customPasswordLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            Form_PasswordList listDisplay = new Form_PasswordList("Custom Password List", passwordSettings.CustomPasswordList);
            this.Cursor = Cursors.Default;
            listDisplay.ShowDialog(this);
        }

        private void clearCustomListButton_Click(object sender, EventArgs e)
        {
            DialogResult result = Utility.MsgBox.ShowConfirm("Clear Custom Password List", "This will clear the custom password list.  Do you wish to continue?");

            if (result == DialogResult.Yes)
            {
                passwordSettings.CustomPasswordList.Clear();
                passwordSettings.CustomListUpdated = DateTime.Now.ToUniversalTime();
            }
        }
    }
}

