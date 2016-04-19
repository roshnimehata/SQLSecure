using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Idera.SQLsecure.Core.Logger;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_SelectAuditData : Idera.SQLsecure.UI.Console.Controls.BaseDialog
    {
        #region Ctors

        public Form_SelectAuditData(bool useBaseline, DateTime? selectDate)
        {
            InitializeComponent();

            _checkBox_BaselineOnly.Checked = useBaseline;

            if (!selectDate.HasValue)
            {
                _checkBox_UseCurrent.Checked = true;
                _ultraGroupBox_DateSelect.Enabled = false;
                _checkBox_IncludeTime.Checked = false;
                _dateTimePicker_Time.Value = DefaultTime;
                _dateTimePicker_Time.Enabled = false;
            }
            else
            {
                _checkBox_UseCurrent.Checked = false;
                _ultraGroupBox_DateSelect.Enabled = true;
                _dateTimePicker_Date.Value = new DateTime(selectDate.Value.Date.Ticks);

                if (selectDate.Value.TimeOfDay.Ticks == DefaultTime.TimeOfDay.Ticks)
                {
                    _checkBox_IncludeTime.Checked = false;
                    _dateTimePicker_Time.Value = DefaultTime;
                    _dateTimePicker_Time.Enabled = false;
                }
                else
                {
                    _checkBox_IncludeTime.Checked = true;
                    _dateTimePicker_Time.Value = selectDate.Value;
                }
            }
        }

        #endregion

        #region Fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Forms.Form_SelectAuditData");
        private bool m_UseBaseline;
        private DateTime? m_SelectDate;
        private DateTime DefaultTime = new DateTime(DateTime.Now.Year,
                                                    DateTime.Now.Month,
                                                    DateTime.Now.Day,
                                                    23, 59, 59);

        #endregion

        #region Properties

        public bool UseBaseline
        {
            get { return m_UseBaseline; }
        }

        public DateTime? SelectDate
        {
            get { return m_SelectDate; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Show selection criteria for audit data and return the selection
        /// </summary>
        /// <param name="inBaseline">set the initial values for Use Baseline</param>
        /// <param name="inDate">set the initial values for the Selection Date</param>
        /// <param name="outBaseline">return the selected value for Use Baseline or the initial value if no selection is made</param>
        /// <param name="outDate">return the selected value for the Select Date or the initial value if no selection is made</param>
        /// <returns>true if a selection is made, otherwise false</returns>
        public static bool GetSelections(bool inBaseline, DateTime? inDate, out bool outBaseline, out DateTime? outDate)
        {
            outBaseline = inBaseline;
            outDate = inDate;

            // Create the form.
            Form_SelectAuditData form = new Form_SelectAuditData(inBaseline, inDate);

            DialogResult rc = form.ShowDialog();
            if (rc == DialogResult.OK)
            {
                outBaseline = form.UseBaseline;
                outDate = form.SelectDate;
            }

            return (rc == DialogResult.OK);
        }

        #endregion

        #region Events

        private void _checkBox_UseCurrent_Click(object sender, EventArgs e)
        {
            _ultraGroupBox_DateSelect.Enabled = !_checkBox_UseCurrent.Checked;
        }

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

            m_UseBaseline = _checkBox_BaselineOnly.Checked;
            if (_checkBox_UseCurrent.Checked)
            {
                m_SelectDate = null;
            }
            else if (_checkBox_IncludeTime.Checked)
            {
                m_SelectDate = _dateTimePicker_Date.Value.Date + _dateTimePicker_Time.Value.TimeOfDay;
            }
            else
            {
                m_SelectDate = _dateTimePicker_Date.Value.Date + new TimeSpan(DefaultTime.TimeOfDay.Ticks);
            }
        }

        private void button_Help_Click(object sender, EventArgs e)
        {
            ShowHelpTopic();
        }

        private void Form_SelectDatabase_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            ShowHelpTopic();
        }

        private void ShowHelpTopic()
        {
            Program.gController.ShowTopic(Utility.Help.SelectAuditDataHelpTopic);
        }

        #endregion
    }
}

