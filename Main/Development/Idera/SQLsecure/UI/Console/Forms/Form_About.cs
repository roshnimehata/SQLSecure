using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using Idera.SQLsecure.UI.Console.Utility;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_About : Idera.SQLsecure.UI.Console.Controls.BaseForm
    {
        #region Ctors

        public Form_About()
        {
            InitializeComponent();

            this.Text = @"About " + Utility.Constants.COMPANY_STR + " " + Utility.Constants.PRODUCT_STR;
            if (!string.IsNullOrEmpty(Utility.Constants.BETA_VERSION))
            {
                this.Text += Utility.Constants.BETA_VERSION;
            }

            Description = Utility.Constants.ProductsPageText;            

            // get assembly version and show as Product version
            Assembly assembly = Assembly.GetExecutingAssembly();
            String version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            if (version != null && version.Equals("0.0.0.0"))
            {
                version = "1.2.0.0";
            }
            if (version != null && version.StartsWith("v"))
                version = version.Substring(1, version.Length - 1);

            this._label_Version.Text = Utility.Constants.PRODUCT_STR + "  " + version;

            // show the current copyright info
            this._label_Copyright.Text = Constants.COPYRIGHT_MSG;

            // get the current repository info
            Sql.Repository repository = Program.gController.Repository;
            if (repository.IsValid)
            {
                // Display repository info.
                this.groupBox_Properties.Text = @"Connection: " + repository.Instance;
                _lbl_SQLVersion.Text = repository.SQLServerVersionFriendlyLong;
                _lbl_SchemaVersion.Text = repository.SchemaVersion.ToString("g");
                _lbl_DALVersion.Text = repository.DALVersion.ToString("g");
                _lbl_AccessLvl.Text = Program.gController.Permissions.UserAccessLevel.ToString();

                // Display license info.
                label_LicenseType.Text = Program.gController.Repository.bbsProductLicense.CombinedLicense.typeStr;
                label_LicensedServers.Text = Program.gController.Repository.bbsProductLicense.CombinedLicense.numLicensedServersStr;
                label_LicenseDayToExpire.Text = Program.gController.Repository.bbsProductLicense.CombinedLicense.daysToExpireStr;
                if (Program.gController.Repository.bbsProductLicense.CombinedLicense.isAboutToExpire)
                {
                    label_LicenseDayToExpire.ForeColor = Color.Red;
                    label_LicenseDayToExpire.BackColor = label_LicenseDayToExpire.BackColor;
                }
            }
            else
            {
                this.groupBox_Properties.Text = @"Connection: Not connected to a Repository";
                //this._listView_Properties.Columns[0].Width = 300;
                //this._listView_Properties.Items.Add("Not currently connected to a repository");
            }
        }

        #endregion

        private void _button_OK_Click(object sender, EventArgs e)
        {

        }

        private void button_ManageLicenses_Click(object sender, EventArgs e)
        {
            Program.gController.Repository.ResetLicense();
            Forms.Form_License form = new Form_License(Program.gController.Repository.bbsProductLicense);
            form.ShowDialog();
            label_LicenseType.Text = Program.gController.Repository.bbsProductLicense.CombinedLicense.typeStr;
            label_LicensedServers.Text = Program.gController.Repository.bbsProductLicense.CombinedLicense.numLicensedServersStr;
            label_LicenseDayToExpire.Text = Program.gController.Repository.bbsProductLicense.CombinedLicense.daysToExpireStr;
            if (Program.gController.Repository.bbsProductLicense.CombinedLicense.isAboutToExpire)
            {
                label_LicenseDayToExpire.ForeColor = Color.Red;
                label_LicenseDayToExpire.BackColor = label_LicenseDayToExpire.BackColor;
            }
            else
            {
                label_LicenseDayToExpire.ForeColor = SystemColors.ControlText;
                label_LicenseDayToExpire.BackColor = label_LicenseDayToExpire.BackColor;
            }
        }
    }
}