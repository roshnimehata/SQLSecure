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
    public partial class Form_RefreshAuditData : Idera.SQLsecure.UI.Console.Controls.BaseDialogForm
    {
        #region Constants

        private const string TITLEFMT = "Refresh Audit Data";
        private const string DESCR_GENERAL_FMT = "Choose new audit data for this assessment";

        #endregion

        #region Ctors

        public Form_RefreshAuditData(Sql.Policy policy)
        {
            InitializeComponent();

            //Icon = AppIcons.AppImage16(AppIcons.Enum.Policy);
            this.Text = TITLEFMT;

            Description = DESCR_GENERAL_FMT;

            DateTime? selectDate = null;

            m_Policy = policy;
            selectDate = m_Policy.AssessmentDate;
            _checkBox_BaselineOnly.Checked = m_Policy.UseBaseline;

            if (!selectDate.HasValue)
            {
                _checkBox_IncludeTime.Checked = false;
                _dateTimePicker_Time.Value = DefaultTime;
                _dateTimePicker_Time.Enabled = false;
            }
            else
            {
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

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Forms.Form_RefreshAuditData");
        private Sql.Policy m_Policy;
        private DateTime DefaultTime = new DateTime(DateTime.Now.Year,
                                                    DateTime.Now.Month,
                                                    DateTime.Now.Day,
                                                    23, 59, 59);

        #endregion

        #region Properties

        public DateTime SelectDate
        {
            get
            {
                DateTime selectDate;

                if (_checkBox_IncludeTime.Checked)
                {
                    selectDate = _dateTimePicker_Date.Value.Date + _dateTimePicker_Time.Value.TimeOfDay;
                }
                else
                {
                    selectDate = _dateTimePicker_Date.Value.Date + new TimeSpan(DefaultTime.TimeOfDay.Ticks);
                }

                return selectDate.ToUniversalTime();
            }
        }

        public bool UseBaseline
        {
            get { return _checkBox_BaselineOnly.Checked; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Show selection criteria for audit data and apply the selection to the policy
        /// </summary>
        /// <param name="policy">the policy or assessment object to refresh assessment data on</param>
        /// <returns>true if the selection was applied, otherwise 0</returns>
        public static bool Process(Sql.Policy policy)
        {
            bool updated = false;
            // Create the form.
            Form_RefreshAuditData form = new Form_RefreshAuditData(policy);

            if (DialogResult.OK == form.ShowDialog())
            {
                policy.AssessmentDate = form.SelectDate;
                policy.UseBaseline = form.UseBaseline;

                if (policy.SavePolicyToRepository(Program.gController.Repository.ConnectionString))
                {
                    updated = true;
                    Program.gController.SignalRefreshPoliciesEvent(policy.PolicyId);
                }
            }

            return updated;
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
        }

        private void _button_Help_Click(object sender, EventArgs e)
        {
            ShowHelpTopic();
        }

        private void _button_ViewSnapshots_Click(object sender, EventArgs e)
        {
            Form_PolicySnapshots.ShowSnapshots(m_Policy, UseBaseline, SelectDate);
        }

        private void Form_SelectDatabase_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            ShowHelpTopic();
        }

        private void ShowHelpTopic()
        {
            Program.gController.ShowTopic(Utility.Help.RefreshAuditDataHelpTopic);
        }

        #endregion
    }
}

