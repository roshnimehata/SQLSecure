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
    public partial class Form_PolicyProperties : Idera.SQLsecure.UI.Console.Controls.BaseDialogForm
    {
        #region Constants and enums

        private enum FormTabs
        {
            General,
            SecurityChecks,
            Servers,
            Interview
        }

        public enum RequestedOperation
        {
            Default,
            ConfigureMetrics,
            ManageServers,
        }

        private const string TITLE_POLICY = "Policy";
        private const string TITLE_ASSESSMENT = "Assessment";
        private string TITLE_NAME
        {
            get { return m_policy == null ? TITLE_POLICY : (m_policy.IsAssessment ? TITLE_ASSESSMENT : TITLE_POLICY); } 
        }
        private string TITLEFMT
        {
            get { return TITLE_NAME + " Properties - {0}"; }
        }
        private string IMPORTTITLEFMT
        {
            get { return "Importing " + TITLE_NAME + " - {0}"; }
        }
        private string DESCR_GENERAL
        {
            get { return "Change the " + TITLE_NAME + " name or description."; }
        }
        private string DESCR_SECURITYCHECKS
        {
            get { return "Specify which security checks you want this " + TITLE_NAME + " to perform."; }
        }
        private string DESCR_SERVERS
        {
            get { return "Specify which SQL Server instances you want to audit with this " + TITLE_NAME + "."; }
        }
        private const string DESCR_INTERVIEW = @"Specify any additional information that should be included in the assessment report.";

        private string ERRORTITLE_UPDATE
        {
            get { return "Error Updating " + TITLE_NAME ; }
        }
        private string ERRORTITLE_CREATE
        {
            get { return "Error Creating " + TITLE_NAME ; }
        }
        private string ERRORMSGFMT
        {
            get { return "Failed to Update " + TITLE_NAME + " {0} error message: {1}"; }
        }
        private string ERRORMSGDUP
        {
            get { return "Cannot create " + TITLE_NAME + " {0}. " + (m_policy.IsAssessment ? "An " : "A ") + TITLE_NAME + " with the name you specified already exists. Specify a different " + TITLE_NAME + " name."; }
        }
        private string ERRORMSGNAMEEMPTY
        {
            get { return "Cannot create " + TITLE_NAME + " with an empty name. Specify a unique " + TITLE_NAME + " name."; }
        }
        private string WARNINGTITLE
        {
            get { return "Update " + TITLE_NAME; }
        }

        #endregion

        #region fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Forms.Form_PolicyProperties");

     //   private bool m_allowEdit = true;
        private bool m_isCreateNew = true;
        private Policy m_policy = null;
        private int m_metricId = 0;
        private RequestedOperation m_RequestedOperation = RequestedOperation.Default;

        #endregion

        #region ctors

        public Form_PolicyProperties()
        {
            InitializeComponent();

            ultraTabControl1.DrawFilter = new HideFocusRectangleDrawFilter();
        }

        public Form_PolicyProperties(RequestedOperation op)
        {
            InitializeComponent();

            m_RequestedOperation = op;
           
            ultraTabControl1.DrawFilter = new HideFocusRectangleDrawFilter();
        }

        #endregion

        #region helpers

        private void InitializeDialogCommon(bool allowEdit)
        {
            if(m_policy == null)
            {
                return;
            }
            this.Picture = (m_policy.IsAssessment)
                     ? Idera.SQLsecure.UI.Console.Properties.Resources.Assessment_EditSettings_48
                     : Idera.SQLsecure.UI.Console.Properties.Resources.edit_policy_49;

            bool isSystem = (m_policy.IsPolicy) ? m_policy.IsSystemPolicy : false;
            bool isApproved = (m_policy.IsAssessment) ? m_policy.IsApprovedAssessment : false;

            bool allowEditSecurityChecks = allowEdit
                && ((m_policy.IsAssessment) ? !m_policy.IsApprovedAssessment : true);

            bool allowChangeServers = allowEdit 
                && ((m_policy.IsAssessment) ? !m_policy.IsApprovedAssessment : true);  

            bool allowChangeInterview = allowEdit
                && ((m_policy.IsAssessment) ? !m_policy.IsApprovedAssessment : true);  

            if (!allowEdit || isSystem || isApproved)
            {
                textBox_PolicyName.ReadOnly =
                    textBox_Description.ReadOnly = true;               
                textBox_PolicyName.ForeColor =
                    textBox_Description.ForeColor = Color.SlateGray;
                textBox_PolicyName.BackColor =
                    textBox_Description.BackColor = Color.GhostWhite;
                               
                if(!allowEdit)
                {
                    _textBox_Notes.ReadOnly = true;
                    _textBox_Notes.ForeColor = Color.SlateGray;
                    _textBox_Notes.BackColor = Color.GhostWhite;

                    button_OK.Visible = false;
                    button_Cancel.Text = "&Close";
                    AcceptButton = button_Cancel;
                }                   
            }

            if (m_policy.IsAssessment)
            {
                textBox_PolicyName.Text = m_policy.AssessmentName;
                textBox_Description.Text = m_policy.AssessmentDescription;
                _textBox_Notes.Text = m_policy.AssessmentNotes;
            }
            else
            {
                textBox_PolicyName.Text = m_policy.PolicyName;
                textBox_Description.Text = m_policy.PolicyDescription;
                _textBox_Notes.Visible = false;
                _label_Notes.Visible = false;
            }
            _policyInterview.InterviewName = m_policy.InterviewName;
            _policyInterview.SetInterviewText(m_policy.InterviewText);
            _policyInterview.InitializeControl(allowChangeInterview);
            controlConfigurePolicyVulnerabilities1.InitializeControl(m_policy, m_metricId, allowEditSecurityChecks);
            controlPolicyAddServers1.InitializeControl(m_policy, allowChangeServers);
        }

        #endregion

        #region methods

        public void InitializeDialog(string fileName, bool allowEdit)
        {
            m_isCreateNew = true;
            m_policy = new Policy();
            m_policy.ImportPolicyFromXMLFile(fileName, false);
            m_policy.IsSystemPolicy = false;
            this.Text = string.Format(IMPORTTITLEFMT, m_policy.PolicyName);
  
            InitializeDialogCommon(allowEdit);

        }

        public void InitializeDialog(int policyID, int assessmentId, bool allowEdit)
        {
            if (policyID == 0)
            {
                m_isCreateNew = true;
                button_OK.Enabled = false;
                m_policy = new Policy();
     //           m_allowEdit = allowEdit;
            }
            else
            {
                m_isCreateNew = false;
                m_policy = Policy.GetPolicy(policyID, assessmentId);
            //    m_allowEdit = allowEdit && !m_policy.IsApprovedAssessment;
                this.Text = string.Format(TITLEFMT, m_policy.PolicyAssessmentName);
            }

            InitializeDialogCommon(allowEdit);
        }

        public void InitializeDialog(int policyID, int assessmentId, bool allowEdit, int MetricId)
        {
            m_metricId = MetricId;
            InitializeDialog(policyID, assessmentId, allowEdit);
        }

        // policyId is 0 if creating a new policy
        public static bool Process(int policyId, int assessmentId, bool allowEdit)
        {
            Form_PolicyProperties frm = new Form_PolicyProperties();
            frm.InitializeDialog(policyId, assessmentId, allowEdit);

            // return true if updated, otherwise false
            return DialogResult.Cancel != frm.ShowDialog();
        }

        public static bool Process(int policyId, int assessmentId, bool allowEdit, RequestedOperation op)
        {
            Form_PolicyProperties frm = new Form_PolicyProperties(op);
            frm.InitializeDialog(policyId, assessmentId, allowEdit);

            // return true if updated, otherwise false
            return DialogResult.Cancel != frm.ShowDialog();
        }

        public static bool Process(int policyId, int assessmentId, bool allowEdit, RequestedOperation op, int metricId)
        {
            Form_PolicyProperties frm = new Form_PolicyProperties(op);
            frm.InitializeDialog(policyId, assessmentId, allowEdit, metricId);

            // return true if updated, otherwise false
            return DialogResult.Cancel != frm.ShowDialog();
        }

        public static bool Process(string fileName, bool allowEdit, RequestedOperation op)
        {
            Form_PolicyProperties frm = new Form_PolicyProperties(op);
            frm.InitializeDialog(fileName, allowEdit);

            // return true if updated, otherwise false
            return DialogResult.Cancel != frm.ShowDialog();
        }

        #endregion

        #region events

        private void button_OK_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox_PolicyName.Text.Trim()))
                {
                    ultraTabControl1.SelectedTab = ultraTabControl1.Tabs[(int)FormTabs.General];
                    textBox_PolicyName.Focus();
                    MsgBox.ShowError(ERRORTITLE_CREATE, ERRORMSGNAMEEMPTY);
                    DialogResult = DialogResult.None;
                    return;
                }

                if (m_policy.IsPolicy)
                {
                    // Is Policy name already used
                    Policy p = Program.gController.Repository.GetPolicy(textBox_PolicyName.Text);
                    if (p != null && p.PolicyId != m_policy.PolicyId)
                    {
                        ultraTabControl1.SelectedTab = ultraTabControl1.Tabs[(int)FormTabs.General];
                        textBox_PolicyName.Focus();
                        MsgBox.ShowError(ERRORTITLE_CREATE,
                                         string.Format(ERRORMSGDUP, textBox_PolicyName.Text));
                        DialogResult = DialogResult.None;
                        return;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(textBox_PolicyName.Text))
                    {
                        ultraTabControl1.SelectedTab = ultraTabControl1.Tabs[(int)FormTabs.General];
                        textBox_PolicyName.Focus();
                        MsgBox.ShowError(ERRORTITLE_CREATE, ERRORMSGNAMEEMPTY);
                        DialogResult = DialogResult.None;
                        return;
                    }

                    Policy p = Program.gController.Repository.GetPolicy(m_policy.PolicyId);
                    if (p.HasAssessment(textBox_PolicyName.Text))
                    {
                        Policy a = p.GetAssessment(textBox_PolicyName.Text);
                        if (a.AssessmentId != m_policy.AssessmentId)
                        {
                            MsgBox.ShowError(ERRORTITLE_UPDATE,
                                             string.Format(ERRORMSGDUP, textBox_PolicyName.Text, p.PolicyName));
                            DialogResult = DialogResult.None;

                            Cursor = Cursors.Default;
                            return;
                        }
                    }
                }
                if (!controlConfigurePolicyVulnerabilities1.OKToSave())
                {
                    ultraTabControl1.SelectedTab = ultraTabControl1.Tabs[(int)FormTabs.SecurityChecks];
                    DialogResult = DialogResult.None;
                    return;                    
                }
                if (controlConfigurePolicyVulnerabilities1.NumSecurityChecks == 0)
                {
                    if (DialogResult.No == MsgBox.ShowWarningConfirm(WARNINGTITLE, ErrorMsgs.NoPolicyMetricsMsg))
                    {
                        ultraTabControl1.SelectedTab = ultraTabControl1.Tabs[(int)FormTabs.SecurityChecks];
                        DialogResult = DialogResult.None;
                        return;
                    }
                }
                if (controlPolicyAddServers1.NumServers == 0)
                {
                    if (DialogResult.No == MsgBox.ShowWarningConfirm(WARNINGTITLE, ErrorMsgs.NoPolicyServersMsg))
                    {
                        ultraTabControl1.SelectedTab = ultraTabControl1.Tabs[(int)FormTabs.Servers];
                        DialogResult = DialogResult.None;
                        return;
                    }
                }

                List<RegisteredServer> serversToAdd = null;
                List<RegisteredServer> serversToRemove = null;
                string dynamicSelection = null;
                bool isDynamic;
                controlPolicyAddServers1.GetServers(m_isCreateNew, out serversToAdd, out serversToRemove,
                                                    out dynamicSelection, out isDynamic);

                int policyId = -1;
                if (m_isCreateNew)
                {
                    policyId =
                        Policy.AddPolicy(textBox_PolicyName.Text, textBox_Description.Text, isDynamic, dynamicSelection, _policyInterview.InterviewName, _policyInterview.GetInterviewText());
                    if (policyId != -1)
                    {
                        m_policy = Policy.GetPolicy(policyId);
                        controlConfigurePolicyVulnerabilities1.SaveMetricChanges(m_policy);
                        m_policy.SavePolicyToRepository(Program.gController.Repository.ConnectionString);
                    }
                }
                else
                {
                    policyId = m_policy.PolicyId;
                    controlConfigurePolicyVulnerabilities1.SaveMetricChanges(m_policy);
                    if (m_policy.IsAssessment)
                    {
                        m_policy.AssessmentName = textBox_PolicyName.Text;
                        m_policy.AssessmentDescription = textBox_Description.Text;
                        m_policy.AssessmentNotes = _textBox_Notes.Text;
                    }
                    else
                    {
                        m_policy.SetPolicyName(textBox_PolicyName.Text);
                        m_policy.PolicyDescription = textBox_Description.Text;
                    }
                    m_policy.IsDynamic = isDynamic;
                    m_policy.DynamicSelection = dynamicSelection;
                    m_policy.InterviewName = _policyInterview.InterviewName;
                    m_policy.InterviewText = _policyInterview.GetInterviewText();

                    m_policy.SavePolicyToRepository(Program.gController.Repository.ConnectionString);
                }

                //list should be empty if dynamic
                if (!isDynamic)
                {
                    foreach (RegisteredServer rServer in serversToRemove)
                    {
                        RegisteredServer.RemoveRegisteredServerFromPolicy(rServer.RegisteredServerId, m_policy.PolicyId, m_policy.AssessmentId);
                    }

                    foreach (RegisteredServer rServer in serversToAdd)
                    {
                        RegisteredServer.AddRegisteredServerToPolicy(rServer.RegisteredServerId, policyId, m_policy.AssessmentId);
                    }
                }

                Program.gController.SignalRefreshPoliciesEvent(0);
            }
            catch (Exception ex)
            {
                string title = ERRORTITLE_UPDATE;
                string msg =
                    string.Format(ERRORMSGFMT, textBox_PolicyName.Text, ex.Message);
                if (m_isCreateNew)
                {
                    title = ERRORTITLE_CREATE;
                    msg =
                        string.Format(ERRORMSGFMT, textBox_PolicyName.Text, ex.Message);
                }
                logX.loggerX.Error(msg);
                MsgBox.ShowError(title, msg);
            }
        }

        private void ultraTabControl1_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            switch (ultraTabControl1.SelectedTab.Index)
            {
                case (int)FormTabs.General:
                    Description = DESCR_GENERAL;
                    break;
                case (int)FormTabs.SecurityChecks:
                    Description = DESCR_SECURITYCHECKS;
                    break;
                case (int)FormTabs.Servers:
                    Description = DESCR_SERVERS;
                    break;
                case (int)FormTabs.Interview:
                    Description = DESCR_INTERVIEW;
                    break;
            }
        }

        private void _btn_Help_Click(object sender, EventArgs e)
        {
            string helpTopic = Utility.Help.PolicyGeneralHelpTopic;
            if (m_policy == null || m_policy.IsPolicy)
            {
                if (ultraTabControl1.SelectedTab.Index == (int) FormTabs.General)
                    helpTopic = Utility.Help.PolicyGeneralHelpTopic;
                else if (ultraTabControl1.SelectedTab.Index == (int) FormTabs.SecurityChecks)
                    helpTopic = Utility.Help.PolicySecurityChecksHelpTopic;
                else if (ultraTabControl1.SelectedTab.Index == (int) FormTabs.Servers)
                    helpTopic = Utility.Help.PolicyServersHelpTopic;
                else if (ultraTabControl1.SelectedTab.Index == (int) FormTabs.Interview)
                    helpTopic = Utility.Help.PolicyInterviewHelpTopic;
            }
            else
            {
                if (ultraTabControl1.SelectedTab.Index == (int)FormTabs.General)
                    helpTopic = Utility.Help.AssessmentGeneralHelpTopic;
                else if (ultraTabControl1.SelectedTab.Index == (int)FormTabs.SecurityChecks)
                    helpTopic = Utility.Help.AssessmentSecurityChecksHelpTopic;
                else if (ultraTabControl1.SelectedTab.Index == (int)FormTabs.Servers)
                    helpTopic = Utility.Help.AssessmentServersHelpTopic;
                else if (ultraTabControl1.SelectedTab.Index == (int)FormTabs.Interview)
                    helpTopic = Utility.Help.AssessmentInterviewHelpTopic;
                
            }

            Program.gController.ShowTopic(helpTopic);
        }

        private void Form_PolicyProperties_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            _btn_Help_Click(sender, new EventArgs());
        }

        private void ultraTabControl1_SelectedTabChanging(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangingEventArgs e)
        {
            if (!controlConfigurePolicyVulnerabilities1.OKToSave())
            {
                e.Cancel = true;
            }
        }

        #endregion

        private void Form_PolicyProperties_Load(object sender, EventArgs e)
        {
            switch (m_RequestedOperation)
            {
                case RequestedOperation.ConfigureMetrics:
                    ultraTabControl1.SelectedTab = ultraTabControl1.Tabs[(int)FormTabs.SecurityChecks];
                    controlConfigurePolicyVulnerabilities1.Select();
                    break;
                case RequestedOperation.ManageServers:
                    ultraTabControl1.SelectedTab = ultraTabControl1.Tabs[(int)FormTabs.Servers];
                    controlPolicyAddServers1.Select();
                    break;
            }

        }
    }
}
