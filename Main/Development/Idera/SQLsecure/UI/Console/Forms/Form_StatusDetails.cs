using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_StatusDetails : Form
    {
        private const string LicenseMsg = "License is not valid.  Click here for details.";
        private const string LicenseExpiringMsg = "License will expire shortly.  Click here for details.";
        private const string LicenseMsgLinkArea = "Click here for details";
        private const string AgentMsg = "SQLsecure uses the Repository SQL Server Agent for data collection.   The SQL Server Agent is not running, it must be started for SQLsecure to collect data.";
        private const string ServersMsg = "SQLsecure was unable to collect data for some of the audited servers.  Please check server status.";

        private void licenseClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Program.gController.Repository.ResetLicense();
            Forms.Form_License form = new Form_License(Program.gController.Repository.bbsProductLicense);
            form.ShowDialog();
            Program.gController.RefreshCurrentView();
        }

        private Form_StatusDetails(
                bool isConnected,
                bool isAgentOk,
                bool isLicenseOk,
                bool isLicenseNotExpiring,
                bool areAllServersOk
            )
        {
            InitializeComponent();

            // Create the link label array.
            LinkLabel[] lnkarray = new LinkLabel[] { _lbl_0, _lbl_1, _lbl_2, _lbl_3 };

            // If connected, display rest of the messages.
            int lnkindex = 0;
            if (isConnected)
            {
                if (!isLicenseOk)
                {
                    lnkarray[lnkindex].Text = LicenseMsg;
                    lnkarray[lnkindex].LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(licenseClicked);
                    lnkarray[lnkindex].LinkArea = new LinkArea(LicenseMsg.Length - LicenseMsgLinkArea.Length - 1, LicenseMsgLinkArea.Length);
                    ++lnkindex;
                }
                else
                {
                    if (!isLicenseNotExpiring)
                    {
                        lnkarray[lnkindex].Text = LicenseExpiringMsg;
                        lnkarray[lnkindex].LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(licenseClicked);
                        lnkarray[lnkindex].LinkArea = new LinkArea(LicenseExpiringMsg.Length - LicenseMsgLinkArea.Length - 1, LicenseMsgLinkArea.Length);
                        ++lnkindex;
                    }
                }

                if (!isAgentOk)
                {
                    lnkarray[lnkindex].Text = AgentMsg;
                    lnkarray[lnkindex].LinkArea = new LinkArea(0, 0);
                    ++lnkindex;
                }

                if (!areAllServersOk)
                {
                    lnkarray[lnkindex].Text = ServersMsg;
                    lnkarray[lnkindex].LinkArea = new LinkArea(0, 0);
                    ++lnkindex;
                }

            }
            else
            {
                lnkarray[lnkindex].Text = "SQLsecure Console is not connected to a Repository.";
                lnkarray[lnkindex].LinkArea = new LinkArea(0, 0);
                ++lnkindex;
            }

            // Hide the rest of the link labels.
            while (lnkindex < lnkarray.Length)
            {
                lnkarray[lnkindex].Visible = false;
                ++lnkindex;
            }
        }

        public static void Process(
                bool isConnected,
                bool isAgentOk,
                bool isLicenseOk,
                bool isLicenseNotExpiring,
                bool areAllServersOk
            )
        {
            Form_StatusDetails form = new Form_StatusDetails(isConnected,isAgentOk,isLicenseOk,isLicenseNotExpiring,areAllServersOk);
            form.ShowDialog();
        }
    }
}