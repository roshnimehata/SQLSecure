using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

using Idera.SQLsecure.UI.Console.Utility;
using Policy = Idera.SQLsecure.UI.Console.Sql.Policy;
using Idera.SQLsecure.UI.Console.Sql;
using Idera.SQLsecure.Core.Logger;
using Infragistics.Win.UltraWinListView;
using Infragistics.Win;
using Idera.SQLsecure.UI.Console.Controls;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_WizardCreatePolicy : Form
    {
        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Forms.Form_WizardCreatePolicy");
        private Policy m_policy;
        private Dictionary<string, Policy> m_Templates = new Dictionary<string, Policy>();

        #region Constants
        private const string WizardIntroText = @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fswiss\fcharset0 Microsoft Sans Serif;}{\f1\fnil\fcharset2 Symbol;}}
{\*\generator Msftedit 5.41.15.1507;}\viewkind4\uc1\pard\f0\fs16 This wizard allows you to add a Policy to SQLsecure.  With this wizard you will:\par
\par
\pard{\pntext\f1\'B7\tab}{\*\pn\pnlvlblt\pnf1\pnindent360{\pntxtb\'B7}}\fi-360\li720\tx720 Specify the Policy Name\par
{\pntext\f1\'B7\tab}Specify the Policy Description\par
{\pntext\f1\'B7\tab}Specify the security checks to assess with the Policy\par
{\pntext\f1\'B7\tab}Select which SQL Server to assess with the Policy\par
{\pntext\f1\'B7\tab}Specify Internal Review Notes for the Policy\fs18\par
}";

        private const string WizardFinishTextPrefix = @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fswiss\fprq2\fcharset0 Microsoft Sans Serif;}{\f1\fswiss\fcharset0 Arial;}}
{\*\generator Msftedit 5.41.15.1507;}\viewkind4\uc1\pard\nowidctlpar\fi-1584\li1584\tx1440\f0\fs16";
        private const string WizardFinishTextPolicyName = @"\b Name\b0\tab :  ";
        private const string WizardFinishTextPolicyDescription = @"\par\b Description\b0\tab :  ";
        private const string WizardFinishTextSecurityChecks = @"\par\b Security Checks\b0\tab :  ";
        private const string WizardFinishTextServers = @"\par\b SQL Servers\b0\tab :  ";
        private const string WizardFinishTextInterview = @"\par\b Notes\b0\tab :  ";
        private const string WizardFinishTextSuffix = @"\par\pard\tx720\par
\pard\f1 Text Goes Here. \f2\fs20\par
}";
        #endregion


        public Form_WizardCreatePolicy()
        {
            InitializeComponent();

            // Set the intro text in the wizard.
            _rtb_Introduction.Rtf = WizardIntroText;

            // Select the intro page.
            _wizard.SelectedPage = _page_Introduction;

            m_policy = new Policy();
            m_policy.IsDynamic = true;
            m_policy.DynamicSelection = string.Empty;

            radioButtonCreateNew.Checked = true;

            // load value lists for listview display
            ValueList radioButtonValueList = new ValueList();
            radioButtonValueList.Key = "RadioButton";
            radioButtonValueList.DisplayStyle = ValueListDisplayStyle.Picture;
            ValueListItem listItem;
            radioButtonValueList.ValueListItems.Clear();
            listItem = new ValueListItem(CheckState.Checked);
            listItem.DisplayText = "Checked";
            listItem.Appearance.Image = AppIcons.AppImage16(AppIcons.Enum.RadioButtonChecked);
            radioButtonValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(CheckState.Unchecked);
            listItem.DisplayText = "UnChecked";
            listItem.Appearance.Image = AppIcons.AppImage16(AppIcons.Enum.RadioButtonUnChecked);
            radioButtonValueList.ValueListItems.Add(listItem);
            _ultraListViewStandardPolicies.SubItemColumns["CheckState"].ValueList = radioButtonValueList;

        
            // Load builtin standard Policies
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
                    UltraListViewItem li = _ultraListViewStandardPolicies.Items.Add(p.PolicyName, p.PolicyName);
                    li.Tag = f;
                    li.SubItems["CheckState"].Value = CheckState.Unchecked;
                    li.SubItems["Count"].Value = p.MetricCount;

                    //save the policy for displaying the description
                    m_Templates.Add(f, p);
                }
                if (_ultraListViewStandardPolicies.Items.Count > 0)
                {
                    _ultraListViewStandardPolicies.Items[0].SubItems["CheckState"].Value = CheckState.Checked;
                }
                _ultraListViewStandardPolicies.Enabled = false;
            }

            setTemplateDescription();
        }


        public static void Process()
        {
            Form_WizardCreatePolicy dlg = new Form_WizardCreatePolicy();
            dlg.ShowDialog();
        }

        private void setTemplateDescription()
        {
            if (radioButtonCreateFromStandard.Checked)
            {
                string path = string.Empty;
                foreach (UltraListViewItem li in _ultraListViewStandardPolicies.Items)
                {
                    if ((CheckState)li.SubItems["CheckState"].Value == CheckState.Checked)
                    {
                        path = (string)li.Tag;
                        break;
                    }
                }
                Policy p;
                if (m_Templates.TryGetValue(path, out p))
                {
                    _groupBox_PolicyName.Text = p.PolicyName;
                    _label_Description.Text = p.PolicyDescription;
                }
                else
                {
                    _groupBox_PolicyName.Text =
                        _label_Description.Text = string.Empty;
                }
            }
            else
            {
                _groupBox_PolicyName.Text =
                    _label_Description.Text = string.Empty;
            }
        }

        private void _page_PolicyType_BeforeMoveNext(object sender, CancelEventArgs e)
        {
           
            if(radioButtonCreateFromStandard.Checked)
            {
                string fileName = (string)textBox_PolicyName.Tag;
                if (!string.IsNullOrEmpty(fileName))
                {
                    m_policy.ImportPolicyFromXMLFile(fileName, false);
                    _policyInterview.SetInterviewText(string.Empty);
                    _policyInterview.InterviewName = string.Empty;
                }
                else
                {
                    MsgBox.ShowError("Error Creating Policy", "Must specify a Standard Policy.");
                    e.Cancel = true;
                }
            }           
            else
            {
                textBox_PolicyName.Tag = null;
                m_policy = new Policy();
                m_policy.IsDynamic = true;
                m_policy.DynamicSelection = string.Empty;
            }
           
        }

        public string PolicyName
        {
            get { return textBox_PolicyName.Text; }
        }

        public string PolicyDescription
        {
            get { return textBox_Description.Text; }
        }

        private void _page_Finish_BeforeDisplay(object sender, EventArgs e)
        {
            StringBuilder strBldr = new StringBuilder(WizardFinishTextPrefix);
            strBldr.Append(WizardFinishTextPolicyName);
            strBldr.Append(PolicyName);
            strBldr.Append(WizardFinishTextPolicyDescription);
            strBldr.Append(PolicyDescription);
            strBldr.Append(WizardFinishTextSecurityChecks);
            strBldr.AppendFormat("{0} Configured", controlConfigurePolicyVulnerabilities1.NumSecurityChecks);
            strBldr.AppendFormat(@"\par{{\f1\tab}}        {0} High Risk", controlConfigurePolicyVulnerabilities1.NumHighSecurityChecks);
            strBldr.AppendFormat(@"\par{{\f1\tab}}        {0} Medium Risk", controlConfigurePolicyVulnerabilities1.NumMediumSecurityChecks);
            strBldr.AppendFormat(@"\par{{\f1\tab}}        {0} Low Risk", controlConfigurePolicyVulnerabilities1.NumLowSecurityChecks);
            strBldr.Append(WizardFinishTextServers);
            bool firstServer = true;
            foreach(string s in controlPolicyAddServers1.GetServerText())
            {
                // Escape the back slash for instances so the instance name will appear in RTF control
                string svr = s.Replace(@"\", @"\\");
                if (firstServer)
                {
                    strBldr.Append(svr);
                    firstServer = false;
                }
                else
                {
                    strBldr.AppendFormat(@"\par{{\f1\tab}}   {0}", svr);
                }
            }
            strBldr.Append(WizardFinishTextInterview);
            if (!string.IsNullOrEmpty(_policyInterview.InterviewName) || !string.IsNullOrEmpty(_policyInterview.GetInterviewText()))
            {
                strBldr.Append("Internal Review Notes added");
                string title = _policyInterview.InterviewName;
                if(!string.IsNullOrEmpty(title))
                {
                    strBldr.AppendFormat(@"\par{{\f1\tab}}   Title - {0}", title);
                }                
            }
            else
            {
                strBldr.Append("No Internal Review Notes added");
            }


            _rtb_Finish.Rtf = strBldr.ToString();

        }

        private void _wizard_Finish(object sender, EventArgs e)
        {
            try
            {
                if (!controlConfigurePolicyVulnerabilities1.OKToSave())
                {                   
                    DialogResult = DialogResult.None;
                    return;
                }

                List<RegisteredServer> serversToAdd = null;
                List<RegisteredServer> serversToRemove = null;
                string dynamicSelection = null;
                bool isDynamic;
                controlPolicyAddServers1.GetServers(true, out serversToAdd, out serversToRemove, out dynamicSelection, out isDynamic);

                int policyId = -1;
                policyId = Policy.AddPolicy(PolicyName, PolicyDescription, isDynamic, dynamicSelection, _policyInterview.InterviewName, _policyInterview.GetInterviewText());
                if (policyId != -1)
                {
                    // Notify controller that a new server was added.
                    Program.gController.SignalRefreshPoliciesEvent(0);

                    // Now add the metrics to the policy
                    m_policy = Policy.GetPolicy(policyId);
                    controlConfigurePolicyVulnerabilities1.SaveMetricChanges(m_policy);
                    m_policy.SavePolicyToRepository(Program.gController.Repository.ConnectionString);

                    // Now add the selected servers to the policy
                    foreach (RegisteredServer rServer in serversToAdd)
                    {
                        RegisteredServer.AddRegisteredServerToPolicy(rServer.RegisteredServerId, m_policy.PolicyId, m_policy.AssessmentId);
                    }
                }
            }
            catch (Exception ex)
            {
                string title = "Error Creating Policy";
                string msg =
                    string.Format("Failed to Create policy {0} error message: {1}", textBox_PolicyName.Text,
                                  ex.Message);
                logX.loggerX.Error(msg);
                MsgBox.ShowError(title, msg);
            }
        }

        private void _page_Vulnerabilities_BeforeDisplay(object sender, EventArgs e)
        {

        }

        private void _page_SQLServers_BeforeDisplay(object sender, EventArgs e)
        {

        }

        private void _page_PolicyName_BeforeDisplay(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty((string)textBox_PolicyName.Tag))
            {
                Policy p = new Policy();
                p.ImportPolicyFromXMLFile((string) textBox_PolicyName.Tag, false);

                textBox_Description.Text = p.PolicyDescription;
            }
        }

        private void _ultraListViewStandardPolicies_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                UltraListViewSubItem item;
                item = _ultraListViewStandardPolicies.SubItemFromPoint(e.Location);
                if (item != null)
                {
                    if (item.Key == "CheckState")
                    {
                        if ((CheckState)item.Value == CheckState.Unchecked)
                        {
                            _ultraListViewStandardPolicies.BeginUpdate();
                            foreach (UltraListViewItem li in _ultraListViewStandardPolicies.Items)
                            {
                                li.SubItems["CheckState"].Value = CheckState.Unchecked;
                            }
                            item.Value = CheckState.Checked;
                            textBox_PolicyName.Text = (string)item.Item.Value;
                            textBox_PolicyName.Tag = item.Item.Tag;
                            _ultraListViewStandardPolicies.EndUpdate();

                            setTemplateDescription();
                        }
                    }
                }
            }
        }

        private void _ultraListViewStandardPolicies_MouseMove(object sender, MouseEventArgs e)
        {
            UltraListViewSubItem item;
            item = _ultraListViewStandardPolicies.SubItemFromPoint(e.Location);
            if (item != null)
            {
                if (item.Key == "CheckState")
                {
                    Cursor = Cursors.Hand;
                }
                else
                {
                    Cursor = Cursors.Default;
                }
            }
            else
            {
                if (Cursor == Cursors.Hand)
                {
                    Cursor = Cursors.Default;
                }
            }
        }

        private void radioButtonCreateNew_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonCreateNew.Checked)
            {
                _ultraListViewStandardPolicies.Enabled =
                    _groupBox_PolicyName.Enabled = false;
                textBox_PolicyName.Text = string.Empty;
                textBox_Description.Text = string.Empty;
            }

            setTemplateDescription();
        }

        private void radioButtonCreateFromStandard_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonCreateFromStandard.Checked)
            {
                _ultraListViewStandardPolicies.Enabled =
                    _groupBox_PolicyName.Enabled = true;
                foreach (UltraListViewItem li in _ultraListViewStandardPolicies.Items)
                {
                    if ((CheckState)li.SubItems["CheckState"].Value == CheckState.Checked)
                    {
                        textBox_PolicyName.Text = (string)li.Value;
                        textBox_PolicyName.Tag = li.Tag;
                        break;
                    }
                }
            }

            setTemplateDescription();
        }

        private void _page_PolicyName_BeforeMoveNext(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(PolicyName))
            {
                MsgBox.ShowError("Error Creating Policy", "Must specify a name for the new policy.");
                e.Cancel = true;
            }
            else
            {
                Policy p = Program.gController.Repository.GetPolicy(PolicyName);
                if (p != null)
                {
                    MsgBox.ShowError("Error Creating Policy",
                                     string.Format(
                                         "Cannot create policy {0}. A policy with the name you specified already exists. Specify a different policy name.",
                                         textBox_PolicyName.Text));
                    e.Cancel = true;
                }
            }

            if (!e.Cancel)
            {
                m_policy.IsDynamic = true;
                controlConfigurePolicyVulnerabilities1.InitializeControl(m_policy);
                controlPolicyAddServers1.InitializeControl(m_policy, true);
            }

        }

        private void _page_Vulnerabilities_BeforeMoveBack(object sender, CancelEventArgs e)
        {
            if (controlConfigurePolicyVulnerabilities1.OKToSave())
            {
                controlConfigurePolicyVulnerabilities1.LeavingControl();
            }
            else
            {
                e.Cancel = true;
            }
        }
        private void _page_Vulnerabilities_BeforeMoveNext(object sender, CancelEventArgs e)
        {
            if (controlConfigurePolicyVulnerabilities1.OKToSave())
            {
                controlConfigurePolicyVulnerabilities1.LeavingControl();
                if (controlConfigurePolicyVulnerabilities1.NumSecurityChecks == 0)
                {
                    if (DialogResult.No == MsgBox.ShowWarningConfirm(@"Create Policy", ErrorMsgs.NoPolicyMetricsMsg))
                    {
                        e.Cancel = true;
                    }
                }
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void _page_SQLServers_BeforeMoveNext(object sender, CancelEventArgs e)
        {
            if (controlPolicyAddServers1.NumServers == 0)
            {
                if (DialogResult.No == MsgBox.ShowWarningConfirm(@"Create Policy", ErrorMsgs.NoPolicyServersMsg))
                {
                    e.Cancel = true;
                }
            }
        }

        private void _page_Interview_BeforeDisplay(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty((string)textBox_PolicyName.Tag))
            {
                if (string.IsNullOrEmpty(_policyInterview.InterviewName) &&
                      string.IsNullOrEmpty(_policyInterview.GetInterviewText()))
                {
                    Policy p = new Policy();
                    p.ImportPolicyFromXMLFile((string) textBox_PolicyName.Tag, false);

                    _policyInterview.SetInterviewText(p.InterviewText);
                    _policyInterview.InterviewName = p.InterviewName;
                }
            }
        }

        private void _page_Introduction_BeforeDisplay(object sender, EventArgs e)
        {

        }

        private void _wizard_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            string helpTopic = Utility.Help.CreatePolicyWizardHelpTopic;
            if (_page_Introduction.Visible)
                helpTopic = Utility.Help.CreatePolicyWizardHelpTopic;
            else if (_page_PolicyName.Visible)
                helpTopic = Utility.Help.AddPolicyNameHelpTopic;
            else if (_page_PolicyCreateType.Visible)
                helpTopic = Utility.Help.AddPolicyTypeHelpTopic;
            else if (_page_Vulnerabilities.Visible)
                helpTopic = Utility.Help.AddPolicySecurityChecksHelpTopic;
            else if (_page_SQLServers.Visible)
                helpTopic = Utility.Help.AddPolicyServersHelpTopic;
            else if (_page_Interview.Visible)
                helpTopic = Utility.Help.AddPolicyInterviewHelpTopic;
            else if (_page_Finish.Visible)
                helpTopic = Utility.Help.AddPolicySummaryHelpTopic;

            Program.gController.ShowTopic(helpTopic);
        }

        private void _linkLabel_HelpTemplates_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Program.gController.ShowTopic(Utility.Help.PolicyTemplatesHelpTopic);
        }
    }
}