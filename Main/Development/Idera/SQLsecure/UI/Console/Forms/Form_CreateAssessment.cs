using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Controls;
using Idera.SQLsecure.UI.Console.Utility;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_CreateAssessment : Idera.SQLsecure.UI.Console.Controls.BaseDialogForm
    {
        #region Constants

        private const string TITLEFMT = "Save as New Assessment";
        private const string DESCR_GENERAL_FMT = "Create a new draft assessment from this {0}:\n\n      {1}";

        private const string DEFAULTNAME_FMT = "Assessment created {0}";

        private const string ERRORTITLE_CREATE = @"Error Creating Assessment";
        private const string ERRORMSGNAMEEMPTY = "The assessment must have a name. Please enter a name before attempting to create the assessment.";
        private const string ERRORMSGDUPLICATENAME = "Cannot create assessment {0}. An assessment with the name you specified already exists for policy {1}. Specify a different assessment name.";

        #endregion

        #region Ctors

        public Form_CreateAssessment(Sql.Policy policy)
        {
            InitializeComponent();

            this.Text = TITLEFMT;

            string copytype = policy.IsPolicy ? "policy" : string.Format("{0} {1}", policy.AssessmentStateName.ToLower(), "assessment");

            Description = String.Format(DESCR_GENERAL_FMT, copytype, policy.PolicyAssessmentName);
            DateTime? selectDate = null;

            //_textBox_AssessmentName.Text = string.Format(DEFAULTNAME_FMT, DateTime.Now.ToLongDateString());
            _textBox_Description.Text = policy.AssessmentDescription;
            m_Policy = policy;

            if (policy.IsAssessment)
            {
                _ultraGroupBox_DataSelection.Visible = false;
                selectDate = policy.AssessmentDate.Value;
                _checkBox_BaselineOnly.Checked = policy.UseBaseline;
            }
            else
            {
                _ultraGroupBox_DataSelection.Visible = false; //= true; This is no longer a user selectable option, but will still be filled and used for creation
                if (Program.gController.PolicyTime.HasValue)
                {
                    selectDate = Program.gController.PolicyTime.Value;
                }
                else
                {
                    selectDate = DateTime.Now;
                }
                _checkBox_BaselineOnly.Checked = Program.gController.PolicyUseBaselineSnapshots;
            }

            if (!selectDate.HasValue)
            {
                _checkBox_IncludeTime.Checked = false;
                _dateTimePicker_Time.Value = DefaultTime;
                _dateTimePicker_Time.Enabled = false;
            }
            else
            {
                _ultraGroupBox_DataSelection.Enabled = true;
                _dateTimePicker_Date.Value = new DateTime(selectDate.Value.ToLocalTime().Date.Ticks);

                if (selectDate.Value.ToLocalTime().TimeOfDay.Ticks == DefaultTime.TimeOfDay.Ticks)
                {
                    _checkBox_IncludeTime.Checked = false;
                    _dateTimePicker_Time.Value = DefaultTime;
                    _dateTimePicker_Time.Enabled = false;
                }
                else
                {
                    _checkBox_IncludeTime.Checked = true;
                    _dateTimePicker_Time.Value = selectDate.Value.ToLocalTime();
                }
            }
        }

        #endregion

        #region Fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Forms.Form_CreateAssessment");
        private Sql.Policy m_Policy;
        private bool m_UseBaseline;
        private DateTime? m_SelectDate;
        private DateTime DefaultTime = new DateTime(DateTime.Now.Year,
                                                    DateTime.Now.Month,
                                                    DateTime.Now.Day,
                                                    23, 59, 59);

        #endregion

        #region Properties

        public string AssessmentName
        {
            get { return _textBox_AssessmentName.Text; }
        }

        public string AssessmentDescription
        {
            get { return _textBox_Description.Text; }
        }

        public DateTime SelectDate
        {
            get { return m_SelectDate.Value; }
        }

        public bool UseBaseline
        {
            get { return m_UseBaseline; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Show selection criteria for audit data and return the selection
        /// </summary>
        /// <param name="policy">the policy or assessment object to copy when creating the new assessment</param>
        /// <returns>the assessmentid of the new assessment, otherwise 0</returns>
        public static int Process(Sql.Policy policy)
        {
            int newAssessmentId = 0;

            // Create the form.
            Form_CreateAssessment form = new Form_CreateAssessment(policy);

            if (DialogResult.OK == form.ShowDialog())
            {
                newAssessmentId = Sql.Policy.CreateAssessment(policy.PolicyId, policy.AssessmentId, form.AssessmentName, form.AssessmentDescription, form.SelectDate, form.UseBaseline);
                Program.gController.SignalRefreshPoliciesEvent(policy.PolicyId);
            }

            return newAssessmentId;
        }

        #endregion

        #region Events

        private void _checkBox_IncludeTime_CheckedChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (_checkBox_IncludeTime.Checked)
            {
                _dateTimePicker_Time.Enabled = true;
            }
            else
            {
                _dateTimePicker_Time.Enabled = false;
            }

            Cursor = Cursors.Default;
        }

        private void _button_OK_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (string.IsNullOrEmpty(_textBox_AssessmentName.Text.Trim()))
            {
                MsgBox.ShowError(ERRORTITLE_CREATE,
                                ERRORMSGNAMEEMPTY);
                DialogResult = DialogResult.None;

                Cursor = Cursors.Default;
                return;
            }
            else
            {
                Sql.Policy policy = Program.gController.Repository.Policies.Find(m_Policy.PolicyId);
                if (policy != null)
                {
                    if (policy.HasAssessment(_textBox_AssessmentName.Text))
                    {
                        MsgBox.ShowError(ERRORTITLE_CREATE,
                                         string.Format(ERRORMSGDUPLICATENAME, _textBox_AssessmentName.Text, m_Policy.PolicyName));
                        DialogResult = DialogResult.None;

                        Cursor = Cursors.Default;
                        return;
                    }
                }
            }

            m_UseBaseline = _checkBox_BaselineOnly.Checked;
            if (_checkBox_IncludeTime.Checked)
            {
                m_SelectDate = _dateTimePicker_Date.Value.Date + _dateTimePicker_Time.Value.TimeOfDay;
            }
            else
            {
                m_SelectDate = _dateTimePicker_Date.Value.Date + new TimeSpan(DefaultTime.TimeOfDay.Ticks);
            }
            m_SelectDate = m_SelectDate.Value.ToUniversalTime();
        }

        private void _button_Help_Click(object sender, EventArgs e)
        {
            ShowHelpTopic();
        }

        private void Form_SelectDatabase_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            ShowHelpTopic();
        }

        private void ShowHelpTopic()
        {
            Program.gController.ShowTopic(Utility.Help.CreateAssessmentHelpTopic);
        }

        #endregion
    }
}

