using System;
using System.Windows.Forms;
using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Controls;
using Idera.SQLsecure.UI.Console.Utility;
using Policy = Idera.SQLsecure.UI.Console.Sql.Policy;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_ImportExportPolicySecuriyChecks : BaseDialogForm
    {
        #region fields

        private static LogX m_logX = new LogX("Idera.SQLsecure.UI.Console.Forms.Form_PolicyProperties");
        private Policy m_policy = null;
        private string m_currentAction = string.Empty;

        #endregion

        #region Properties

        public string IsSelectedColumnDisplayText
        {
            get { return ctrSelectingPolicyVulnerabilities.IsSelectColumnDisplayText; }
            set { ctrSelectingPolicyVulnerabilities.IsSelectColumnDisplayText = value; }
        }

        public Policy Policy
        {
            get { return m_policy; }
        } 

        #endregion

        #region ctors

        public Form_ImportExportPolicySecuriyChecks()
        {
            InitializeComponent();
        }

        #endregion

        #region helpers

        private void InitializeDialogCommon(bool allowEdit)
        {
            if (m_policy == null)
            {
                return;
            }
            this.Picture = (m_policy.IsAssessment)
                     ?  Properties.Resources.Assessment_EditSettings_48
                     :  Properties.Resources.edit_policy_49;

            bool isSystem = (m_policy.IsPolicy) ? m_policy.IsSystemPolicy : false;
            bool isApproved = (m_policy.IsAssessment) ? m_policy.IsApprovedAssessment : false;

            bool allowEditSecurityChecks = allowEdit
                && ((m_policy.IsAssessment) ? !m_policy.IsApprovedAssessment : true);

            if (!allowEdit || isSystem || isApproved)
            {
                if (!allowEdit)
                {
                    button_OK.Visible = false;
                    button_Cancel.Text = "&Close";
                    AcceptButton = button_Cancel;
                }
            }

            ctrSelectingPolicyVulnerabilities.InitializeControl(m_policy, 0, allowEditSecurityChecks);
        }

        private string GetTitleString( string actionString, Policy policy)
        {
            m_currentAction = actionString;
            string policyType = policy == null ? Utility.Constants.TITLE_POLICY : (policy.IsAssessment ? Utility.Constants.TITLE_ASSESSMENT : Utility.Constants.TITLE_POLICY);
            return string.Format(Utility.Constants.IMPORTING_EXPORTING_FORM_TITLE_FORMAT, actionString, policyType, policy.PolicyAssessmentName);
        }

        #endregion

        #region methods

        public void InitializeDialog(int policyID, int assessmentId, bool allowEdit)
        {
            m_policy = Policy.GetPolicy(policyID, assessmentId);
            InitializeDialogCommon(allowEdit);
        }

        public void InitializeDialog(Policy policy, bool allowEdit)
        {
            m_policy = policy;
            InitializeDialogCommon(allowEdit);
        }

        public static void ProcessExport(Policy policy, bool allowEdit)
        {
            Form_ImportExportPolicySecuriyChecks frm = new Form_ImportExportPolicySecuriyChecks();
            frm.Text = frm.GetTitleString(Utility.Constants.EXPORTING, policy);
            frm.Description = string.Format(Utility.Constants.IMPORTING_EXPORTING_DESCRIPTION_FORMAT, Utility.Constants.EXPORT_COLUMN_TEXT, Utility.Constants.EXPORTING);
            frm.IsSelectedColumnDisplayText = Utility.Constants.EXPORT_COLUMN_TEXT;
            frm.InitializeDialog(policy.PolicyId, policy.AssessmentId, allowEdit);
            frm.ShowDialog();
        }

        public static bool ProcessImport(Policy policy, bool allowEdit)
        {
            Form_ImportExportPolicySecuriyChecks frm = new Form_ImportExportPolicySecuriyChecks();
            frm.Text = frm.GetTitleString(Utility.Constants.IMPORTING, policy);
            frm.Description = string.Format(Utility.Constants.IMPORTING_EXPORTING_DESCRIPTION_FORMAT, Utility.Constants.IMPORT_COLUMN_TEXT, Utility.Constants.IMPORTING);
            frm.IsSelectedColumnDisplayText = Utility.Constants.IMPORT_COLUMN_TEXT;
            frm.InitializeDialog(policy, allowEdit);
            return DialogResult.Cancel != frm.ShowDialog();
        }

        #endregion

        #region events

        private void button_OK_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_currentAction == Utility.Constants.EXPORTING)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        m_policy.SaveToXMLFile(saveFileDialog.FileName);
                    }
                }

                Close();

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                string msg =
                    string.Format("Failed {0}: {1}", Text, ex.Message);
                m_logX.loggerX.Error(msg);
                MsgBox.ShowError(string.Format("Failed Export Policy"), msg);
            }
        }

        private void _btn_Help_Click(object sender, EventArgs e)
        {
            string helpTopic;

            if (m_policy == null || m_policy.IsPolicy)
            {
                helpTopic = Utility.Help.PolicySecurityChecksHelpTopic;
            }
            else
            {
                helpTopic = Utility.Help.AssessmentSecurityChecksHelpTopic;
            }

            Program.gController.ShowTopic(helpTopic);
        }

        private void Form_PolicyProperties_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            _btn_Help_Click(sender, new EventArgs());
        }

        #endregion
    }
}
