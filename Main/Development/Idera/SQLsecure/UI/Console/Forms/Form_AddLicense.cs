using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Idera.SQLsecure.UI.Console.Utility;
using Idera.SQLsecure.Core.Accounts;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_AddLicense : Idera.SQLsecure.UI.Console.Controls.BaseDialogForm
    {
        BBSProductLicense m_BBSProductLicense = null;

   
 
        #region CTOR
        public Form_AddLicense(BBSProductLicense bpl)
        {
            m_BBSProductLicense = bpl;

            InitializeComponent();

            if (!AllowGenerateTrialLicense())
            {
                if (Program.gController.Repository.IsLicenseOk())
                {
                    label_NewUser1.Text = Utility.ErrorMsgs.LicenseInterestText;
                    label_NewUser2.Text = Utility.ErrorMsgs.LicenseEnterProductionText;
                }
                else if (Program.gController.Repository.RepositoryComputerName != System.Environment.MachineName)
                {
                    label_NewUser2.Text = Utility.ErrorMsgs.LicenseUnsupportedConfigText;
                }
                else if (m_BBSProductLicense.HasTrialLicneseBeenUsed())
                {
                    label_NewUser2.Text = Utility.ErrorMsgs.LicenseTrialExpiredText;
                }
                button_GenerateTrialLicense.Visible = false;
            }

            textBox_NewKey.Text = string.Empty;
            button_OK.Enabled = false;

        }
        #endregion

        #region Helpers

        private bool AllowGenerateTrialLicense()
        {
            bool isAllowed = false;
            if (!Program.gController.Repository.IsLicenseOk() && !m_BBSProductLicense.HasTrialLicneseBeenUsed())
            {
                if (Program.gController.Repository.RepositoryComputerName == System.Environment.MachineName)
                {
                    isAllowed = true;
                }
            }

            return isAllowed;
        }

        private bool AddNewLicenseString(string licenseStr)
        {
            bool isOK = true;
            licenseStr = licenseStr.Trim();

            BBSProductLicense.LicenseState licState = BBSProductLicense.LicenseState.InvalidKey;
            if (!string.IsNullOrEmpty(licenseStr))
            {

                if (!m_BBSProductLicense.IsLicenseStringValid(licenseStr, out licState))
                {
                    string message = null;
                    switch (licState)
                    {
                        case BBSProductLicense.LicenseState.InvalidKey:
                            message = string.Format(Utility.ErrorMsgs.LicenseInvalid, licenseStr);
                            break;
                        case BBSProductLicense.LicenseState.InvalidExpired:
                            message = string.Format(Utility.ErrorMsgs.LicenseExpired);
                            break;
                        case BBSProductLicense.LicenseState.InvalidProductID:
                            message = string.Format(Utility.ErrorMsgs.LicenseInvalidProductID);
                            break;
                        case BBSProductLicense.LicenseState.InvalidProductVersion:
                            message = string.Format(Utility.ErrorMsgs.LicenseInvalidProductVersion);
                            break;
                        case BBSProductLicense.LicenseState.InvalidScope:
                            message = string.Format(Utility.ErrorMsgs.LicenseInvalidRepository, Program.gController.Repository.Instance);
                            break;
                        case BBSProductLicense.LicenseState.InvalidDuplicateLicense:
                            message = string.Format(Utility.ErrorMsgs.LicenseInvalidDuplicate);
                            break;
                        default:
                            message = string.Format(Utility.ErrorMsgs.LicenseInvalid, licenseStr);
                            break;
                    }
                    Utility.MsgBox.ShowError(Utility.ErrorMsgs.LicenseCaption, message);
                    isOK = false;
                }

                if (isOK)
                {
                    if (!Program.gController.Repository.IsLicenseTrial())
                    {
                        if (m_BBSProductLicense.IsLicenseStringTrial(licenseStr))
                        {
                            Utility.MsgBox.ShowError(Utility.ErrorMsgs.LicenseCaption, Utility.ErrorMsgs.CantAddTrialToPermamentLicense);
                            isOK = false;
                        }
                    }

                    if (isOK)
                    {
                        if (!Program.gController.Repository.IsLicenseOk() || Program.gController.Repository.IsLicenseTrial())
                        {
                            m_BBSProductLicense.RemoveAllLicenses();
                        }
                        if (m_BBSProductLicense.IsLicenseStringTrial(licenseStr))
                        {
                            m_BBSProductLicense.TagTrialLicenseUsed();
                        }
                        m_BBSProductLicense.AddLicense(licenseStr);
                        Program.gController.Repository.ResetLicense();
                    }
                }
            }
            return isOK;
        }

        #endregion

        #region Events

        private void button_GenerateTrialLicense_Click(object sender, EventArgs e)
        {
            string licenseKey = m_BBSProductLicense.GenerateTrialLicense();

            BBSProductLicense.LicenseState licState;
            if (m_BBSProductLicense.IsLicenseStringValid(licenseKey, out licState))
            {
                textBox_NewKey.Text = licenseKey;
            }
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            if (AddNewLicenseString(textBox_NewKey.Text))
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                return;
            }
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        #endregion

        private void textBox_NewKey_TextChanged(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(textBox_NewKey.Text))
            {
                button_OK.Enabled = false;
            }
            else
            {
                button_OK.Enabled = true;
            }
        }



    }
}

