using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Utility;
using Idera.SQLsecure.Core.Accounts;
using Idera.SQLsecure.UI.Console.Sql;
using Policy=Idera.SQLsecure.UI.Console.Sql.Policy;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_CreatePolicy : Idera.SQLsecure.UI.Console.Controls.BaseDialog
    {
        private static LogX logX = new LogX("IIdera.SQLsecure.UI.Console.Forms.Form_CreatePolicy");

        private bool m_allowEdit = true;
        private bool m_isCreateNew = true;
        private Policy m_policy = null;
        private List<RegisteredServer> m_orginalServersInPolicy = null;

        public Form_CreatePolicy()
        {
            InitializeComponent();

            button_OK.Enabled = false;

            foreach (RegisteredServer r in Program.gController.Repository.RegisteredServers)
            {
                ListViewItem lvItem = new ListViewItem();
                lvItem.Tag = r;
                lvItem.Text = r.ServerName;
                listView_AddServers.Items.Add(lvItem);
            }
        }

        public void InitializeDialog(string fileName, bool allowEdit)
        {
            m_isCreateNew = false;
            m_policy = new Policy();
            m_policy.ImportPolicyFromXMLFile(fileName);
            textBox_PolicyName.Text = m_policy.PolicyName;
            this.Text = "Importing Policy - " + m_policy.PolicyName;

            if (!allowEdit)
            {
                textBox_PolicyName.Enabled = false;
                listView_AddServers.Enabled = false;
            }
            else
            {
                if (m_policy != null && m_policy.IsDynamic)
                {
                    listView_AddServers.Items.Clear();
                    listView_AddServers.Enabled = false;
                    listView_AddServers.Items.Add("This is a dynamic policy servers are added at run time");
                }
                if (m_policy != null && m_policy.IsSystemPolicy)
                {
                    textBox_PolicyName.Enabled = false;
                    controlConfigurePolicyVulnerabilities1.Enabled = false;
                }
            }

            controlConfigurePolicyVulnerabilities1.InitilizeControl(m_policy);

        }


        public void InitializeDialog(int policyID, bool allowEdit)
        {
            m_allowEdit = allowEdit;
            if (policyID == 0)
            {
                m_isCreateNew = true;
                m_policy = new Policy();
            }
            else
            {
                m_isCreateNew = false;
                m_policy = Policy.GetPolicy(policyID);
                textBox_PolicyName.Text = m_policy.PolicyName;
                this.Text = "Policy Properties - " + m_policy.PolicyName;
                m_orginalServersInPolicy = m_policy.GetMemberServers();

                foreach (ListViewItem server in listView_AddServers.Items)
                {
                    if (m_orginalServersInPolicy.Contains((RegisteredServer)server.Tag))
                    {
                        server.Checked = true;
                    }
                }
            }

            if(!allowEdit)
            {
                textBox_PolicyName.Enabled = false;
                listView_AddServers.Enabled = false;
            }
            else
            {
                if(m_policy != null && m_policy.IsDynamic)
                {
                    listView_AddServers.Items.Clear();
                    listView_AddServers.Enabled = false;
                    listView_AddServers.Items.Add("This is a dynamic policy servers are added at run time");
                }
                if (m_policy != null && m_policy.IsSystemPolicy)
                {
                    textBox_PolicyName.Enabled = false;
                    controlConfigurePolicyVulnerabilities1.Enabled = false;
                }
            }

            controlConfigurePolicyVulnerabilities1.InitilizeControl(m_policy);

        }

        private void textBox_PolicyName_TextChanged(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(textBox_PolicyName.Text))
            {
                button_OK.Enabled = false; 
            }
            else
            {
                button_OK.Enabled = true;
            }

        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            try
            {
                // Is Policy name already used
                Policy p = Program.gController.Repository.GetPolicy(textBox_PolicyName.Text);
                if (p != null)
                {
                    MsgBox.ShowError("Error Creating Policy",
                                     string.Format("Cannot create policy {0}. A policy with the name you specified already exist. Specify a different policy name.", textBox_PolicyName.Text));
                    DialogResult = DialogResult.None;
                    return;
                }

                int policyId = -1;
                List<RegisteredServer> serversToAdd = new List<RegisteredServer>();
                List<RegisteredServer> serversToRemove = new List<RegisteredServer>();

                foreach (ListViewItem server in listView_AddServers.CheckedItems)
                {
                    serversToAdd.Add((RegisteredServer)server.Tag);
                }

                if (!m_isCreateNew && m_orginalServersInPolicy != null)
                {
                    foreach (RegisteredServer r in m_orginalServersInPolicy)
                    {
                        bool isStillChecked = false;
                        foreach (ListViewItem server in listView_AddServers.CheckedItems)
                        {
                            if ((RegisteredServer)server.Tag == r)
                            {
                                isStillChecked = true;
                                break;
                            }
                        }
                        if (!isStillChecked)
                        {
                            serversToRemove.Add(r);
                        }
                    }

                    foreach (RegisteredServer rServer in serversToRemove)
                    {
                        RegisteredServer.RemoveRegisteredServerFromPolicy(rServer.RegisteredServerId, m_policy.PolicyId);
                    }
                }

                if (m_isCreateNew)
                {
                    policyId = Policy.AddPolicy(textBox_PolicyName.Text, false, string.Empty);
                    if (policyId != -1)
                    {
                        m_policy = Policy.GetPolicy(policyId);
                        controlConfigurePolicyVulnerabilities1.SaveMetricChanges(m_policy);
                        m_policy.SavePolicyToRepository(Program.gController.Repository.ConnectionString);
                    }
                }
                else
                {
                    controlConfigurePolicyVulnerabilities1.SaveMetricChanges(m_policy);
                    m_policy.SetPolicyName(textBox_PolicyName.Text);
                    m_policy.SavePolicyToRepository(Program.gController.Repository.ConnectionString);
                }

                foreach (RegisteredServer rServer in serversToAdd)
                {
                    RegisteredServer.AddRegisteredServerToPolicy(rServer.RegisteredServerId, policyId);
                }
            }
            catch (Exception ex)
            {
                string title = "Error Updating Policy";
                string msg =
                    string.Format("Failed to Update policy {0} error message: {1}", textBox_PolicyName.Text, ex.Message);
                if (m_isCreateNew)
                {
                    title = "Error Creating Policy";
                    msg =
                        string.Format("Failed to Create policy {0} error message: {1}", textBox_PolicyName.Text,
                                      ex.Message);
                }
                logX.loggerX.Error(msg);
                MsgBox.ShowError(title, msg);
            }

        }

        // policyId is 0 if creating a new policy
        public static void Process(int policyId, bool allowEdit)
        {
            Form_CreatePolicy formCreatePolicy = new Form_CreatePolicy();
            formCreatePolicy.InitializeDialog(policyId, allowEdit);

            formCreatePolicy.ShowDialog();
        }

        public static void Process(string fileName, bool allowEdit)
        {
            Form_CreatePolicy formCreatePolicy = new Form_CreatePolicy();
            formCreatePolicy.InitializeDialog(fileName, allowEdit);

            formCreatePolicy.ShowDialog();
        }

        private void button_Configure_Click(object sender, EventArgs e)
        {
            Forms.Form_CreatePolicy.Process(m_policy.PolicyId, Program.gController.isAdmin);
        }
    }
}

