using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

using Idera.SQLsecure.UI.Console.Utility;
using Idera.SQLsecure.Core.Accounts;
using Infragistics.Win.UltraWinListView;


namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_License : Idera.SQLsecure.UI.Console.Controls.BaseForm
    {
        private Color warningColor = Color.OrangeRed;
        private Color errorColor = Color.Red;

        #region Fields

        private List<BBSProductLicense.LicenseData> m_Licenses;
        private BBSProductLicense.LicenseData m_CombinedLicense;
        private BBSProductLicense m_BBSProductLicense;
        #endregion

        #region CTOR

        public Form_License(BBSProductLicense bbsProductLicense)
        {
            InitializeComponent();
            m_BBSProductLicense = bbsProductLicense;
            LoadLicenseInformation();

        }
        
        #endregion

        #region Helpers

        private void LoadLicenseInformation()
        {
            m_Licenses = m_BBSProductLicense.Licenses;
            m_CombinedLicense = m_BBSProductLicense.CombinedLicense;

            ultraListView_Licenses.Items.Clear();
            textBox_DaysToExpire.Text = string.Empty;
            textBox_LicensedFor.Text = string.Empty;
            textBox_LicensedServers.Text = string.Empty;
            textBox_LicenseExpiration.Text = string.Empty;
            textBox_LicenseType.Text = string.Empty;

            foreach (BBSProductLicense.LicenseData licDataTemp in m_Licenses)
            {
                BBSProductLicense.LicenseData licData = licDataTemp;
                if (licData.licState == BBSProductLicense.LicenseState.Valid)
                {
                    UltraListViewItem li = ultraListView_Licenses.Items.Add(null, licData.key);
                    li.Tag = licData;
                    li.SubItems["colServer"].Value = licData.numLicensedServersStr;
                    UltraListViewSubItem si = li.SubItems["colDaysToExpiration"]; 
                    si.Value = licData.daysToExpireStr;
                    if (licData.isAboutToExpire)
                    {
                        si.Appearance.ForeColor = warningColor;
                    }
                }
                else
                {
                    string message = null;
                    switch (licData.licState)
                    {
                        case BBSProductLicense.LicenseState.InvalidKey:
                            licData.typeStr = "Invalid License";
                            licData.forStr = string.Empty;
                            licData.expirationDateStr = string.Empty;
                            licData.daysToExpireStr = string.Empty;
                            message = string.Format(Utility.ErrorMsgs.LicenseInvalid, licData.key);
                            break;
                        case BBSProductLicense.LicenseState.InvalidExpired:
                            message = Utility.ErrorMsgs.LicenseExpired;
                            licData.typeStr = message;
                            break;
                        case BBSProductLicense.LicenseState.InvalidProductID:                            
                            message = Utility.ErrorMsgs.LicenseInvalidProductID;
                            licData.typeStr = message;
                            break;
                        case BBSProductLicense.LicenseState.InvalidProductVersion:
                            message = Utility.ErrorMsgs.LicenseInvalidProductVersion;
                            licData.typeStr = message;
                            break;
                        case BBSProductLicense.LicenseState.InvalidScope:
                            message = string.Format(Utility.ErrorMsgs.LicenseInvalidRepository, m_BBSProductLicense.OrginalScopeString);
                            licData.typeStr = message;
                            break;
                        case BBSProductLicense.LicenseState.InvalidDuplicateLicense:
                            message = Utility.ErrorMsgs.LicenseInvalidDuplicate;
                            licData.typeStr = message;
                            break;
                        default:
                            licData.typeStr = "Invalid License";
                            licData.forStr = string.Empty;
                            licData.expirationDateStr = string.Empty;
                            licData.daysToExpireStr = string.Empty;
                            message = string.Format(Utility.ErrorMsgs.LicenseInvalid, licData.key);
                            break;
                    }
//                    Utility.MsgBox.ShowWarning(Utility.ErrorMsgs.LicenseCaption, message);
                    UltraListViewItem li = ultraListView_Licenses.Items.Add(null, licData.key);
                    li.Tag = licData;
                    li.SubItems["colServer"].Value = licData.numLicensedServersStr;
                    li.SubItems["colDaysToExpiration"].Value = "Expired";
                    li.Appearance.ForeColor = errorColor;
                }
            }
            if (m_Licenses.Count > 1)
            {
                UltraListViewItem li = ultraListView_Licenses.Items.Add(null, m_CombinedLicense.key);
                li.Appearance.ForeColor = Color.DodgerBlue;
                li.Value = m_CombinedLicense.key;
                li.Tag = m_CombinedLicense;
                li.SubItems["colServer"].Value = m_CombinedLicense.numLicensedServersStr;
                li.SubItems["colDaysToExpiration"].Value = m_CombinedLicense.daysToExpireStr;                     
                ultraListView_Licenses.SelectedItems.Clear();
                ultraListView_Licenses.SelectedItems.Add(li);
                ultraListView_Licenses.ActiveItem = li;
                button_Delete.Enabled = false;

            }
            else if (ultraListView_Licenses.Items.Count > 0)
            {
                ultraListView_Licenses.SelectedItems.Clear();
                ultraListView_Licenses.SelectedItems.Add(ultraListView_Licenses.Items[0]);
                ultraListView_Licenses.ActiveItem = ultraListView_Licenses.Items[0];
            }
            if (ultraListView_Licenses.Items.Count == 0)
            {
                button_Delete.Enabled = false;
            }

        }

        private void ShowHelpTopic()
        {
            Program.gController.ShowTopic(Utility.Help.ManageLicenseHelpTopic);
        }


        #endregion

        #region Events

        private void button_OK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;            
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void button_Delete_Click(object sender, EventArgs e)
        {
            if (Utility.MsgBox.ShowConfirm(Utility.ErrorMsgs.DeleteLicenseCaption, Utility.ErrorMsgs.DeleteConfirmMsg)
                == DialogResult.Yes)
            {
                BBSProductLicense.LicenseData licData = (BBSProductLicense.LicenseData)ultraListView_Licenses.SelectedItems[0].Tag;

                m_BBSProductLicense.RemoveLicense(licData.licenseRepositoryID);

                Program.gController.Repository.ResetLicense();
                m_BBSProductLicense = Program.gController.Repository.bbsProductLicense;

                LoadLicenseInformation();
            }
        }

        private void button_Add_Click(object sender, EventArgs e)
        {
            Form_AddLicense form = new Form_AddLicense(m_BBSProductLicense);
            if (form.ShowDialog() == DialogResult.OK)
            {
                Program.gController.Repository.ResetLicense();
                m_BBSProductLicense = Program.gController.Repository.bbsProductLicense;
                LoadLicenseInformation();
            }
        }        


        private void _btn_Help_Click(object sender, EventArgs e)
        {
            ShowHelpTopic();
        }

        private void Form_License_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            ShowHelpTopic();
        }

        #endregion

        private void ultraListView_Licenses_ItemSelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        {
            bool enableDelete = true;
            if (ultraListView_Licenses.SelectedItems.Count > 0)
            {
                BBSProductLicense.LicenseData licData = (BBSProductLicense.LicenseData)ultraListView_Licenses.SelectedItems[0].Tag;
                textBox_LicensedFor.Text = licData.forStr;
                textBox_LicensedServers.Text = licData.numLicensedServersStr;
                textBox_LicenseType.Text = licData.typeStr;
                textBox_LicenseExpiration.Text = licData.expirationDateStr;
                textBox_DaysToExpire.Text = licData.daysToExpireStr;
                if (licData.licState != BBSProductLicense.LicenseState.Valid)
                {
                    textBox_LicenseType.ForeColor = errorColor;
                    textBox_LicenseType.BackColor = textBox_LicensedFor.BackColor;
                }
                else
                {
                    textBox_LicenseType.ForeColor = textBox_LicensedFor.ForeColor;
                    textBox_LicenseType.BackColor = textBox_LicensedFor.BackColor;
                }
                if (licData.isAboutToExpire)
                {
                    textBox_DaysToExpire.ForeColor = warningColor;
                    textBox_LicenseExpiration.ForeColor = warningColor;
                    textBox_DaysToExpire.BackColor = textBox_LicensedFor.BackColor;
                    textBox_LicenseExpiration.BackColor = textBox_LicensedFor.BackColor;
                }
                else
                {
                    textBox_DaysToExpire.ForeColor = textBox_LicensedFor.ForeColor; ;
                    textBox_LicenseExpiration.ForeColor = textBox_LicensedFor.ForeColor; ;
                }
                if (licData.key == BBSLicenseConstants.CombinedLicenses)
                {
                    enableDelete = false;
                }
            }
            else
            {
                enableDelete = false;
            }
            button_Delete.Enabled = enableDelete;


        }
       

        private void Form_License_Shown(object sender, EventArgs e)
        {
            ultraListView_Licenses.Focus();
        }     

    }
}