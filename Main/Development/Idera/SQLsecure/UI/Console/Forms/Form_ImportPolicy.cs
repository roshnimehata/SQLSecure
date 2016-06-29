using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Sql;


namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_ImportPolicy : Idera.SQLsecure.UI.Console.Controls.BaseDialogForm
    {
        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Forms.Form_ImportPolicy");
        private string m_PolicyFilename = string.Empty;
        private Dictionary<string, Policy> m_Templates = new Dictionary<string, Policy>();

        public Form_ImportPolicy()
        {
            InitializeComponent();

            string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            path = path.Substring(0, path.LastIndexOf(Path.DirectorySeparatorChar));
            string policyPath = path + @"\" + Utility.Constants.Policy_Install_Folder;

            if (Directory.Exists(policyPath))
            {
                string[] files = Directory.GetFiles(policyPath);
                foreach (string f in files)
                {
                    Policy p = new Policy();
                    p.ImportPolicyFromXMLFile(f, false);
                    ListViewItem i = new ListViewItem();
                    i.Tag = f;
                    i.Text = p.PolicyName;
                    i.SubItems.Add(p.MetricCount.ToString());
                    listView_IderaPolicies.Items.Add(i);

                    //save the policy for displaying the description
                    m_Templates.Add(f, p);
                }
            }

            radioButtonTemplate.Checked = true;
            textBox_PolicyImportFile.Enabled = false;
            button_Browse.Enabled = false;

            setTemplateDescription();
        }
       

        public string PolicyFilename
        {
            get { return m_PolicyFilename; }
        }

        public static string Process()
        {
            Form_ImportPolicy dlg = new Form_ImportPolicy();
            if(dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return dlg.PolicyFilename;                
            }
            else
            {
                return null;
            }
        }


        private void button_Browse_Click(object sender, EventArgs e)
        {
            if (openFileDialog_ImportPolicy.ShowDialog() == DialogResult.OK)
            {
                m_PolicyFilename = openFileDialog_ImportPolicy.FileName;
                textBox_PolicyImportFile.Text = m_PolicyFilename;
            }

        }

        private void _bdf_OKBtn_Click(object sender, EventArgs e)
        {
            if (radioButtonTemplate.Checked)
            {
                if (listView_IderaPolicies.SelectedItems.Count > 0)
                {
                    m_PolicyFilename = (string) listView_IderaPolicies.SelectedItems[0].Tag;
                }
            }
            else
            {
                m_PolicyFilename = textBox_PolicyImportFile.Text;
            }
        }

        private void radioButtonTemplate_CheckedChanged(object sender, EventArgs e)
        {
            bool enabled = radioButtonExported.Checked;

            textBox_PolicyImportFile.Enabled = enabled;
            button_Browse.Enabled = enabled;

            listView_IderaPolicies.Enabled =
                groupBox_PolicyName.Enabled = !enabled;

            // Make the selection show so that the description makes sense
            if (listView_IderaPolicies.Enabled)
            {
                listView_IderaPolicies.Focus();
            }
            setTemplateDescription();
        }

        private void radioButtonExported_CheckedChanged(object sender, EventArgs e)
        {
            bool enabled = radioButtonExported.Checked;

            textBox_PolicyImportFile.Enabled = enabled;
            button_Browse.Enabled = enabled;

            listView_IderaPolicies.Enabled =
                groupBox_PolicyName.Enabled = !enabled;

            setTemplateDescription();
        }

        private void listView_IderaPolicies_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            setTemplateDescription();
        }

        private void ultraButton_Help_Click(object sender, EventArgs e)
        {
            ShowHelp();
        }

        private void Form_ImportPolicy_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            ShowHelp();
        }

        private void ShowHelp()
        {
            Program.gController.ShowTopic(Utility.Help.PolicyImportHelpTopic);
        }

        private void setTemplateDescription()
        {
            if (radioButtonTemplate.Checked && listView_IderaPolicies.SelectedItems.Count > 0)
            {
                Policy p;
                if (m_Templates.TryGetValue((string)listView_IderaPolicies.SelectedItems[0].Tag, out p))
                {
                    groupBox_PolicyName.Text = p.PolicyName;
                    label_Description.Text = p.PolicyDescription;
                }
                else
                {
                    groupBox_PolicyName.Text = 
                        label_Description.Text = string.Empty;
                }
            }
            else
            {
                groupBox_PolicyName.Text = 
                    label_Description.Text = string.Empty;
            }
        }

        private void _linkLabel_HelpTemplates_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Program.gController.ShowTopic(Utility.Help.PolicyTemplatesHelpTopic);
        }
    }
}
