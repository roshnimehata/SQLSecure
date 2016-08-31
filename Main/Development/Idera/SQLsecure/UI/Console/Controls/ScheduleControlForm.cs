using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Idera.SQLsecure.UI.Console.Sql;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class ScheduleControlForm : UserControl
    {
        #region Constants
        public const string MonthlyOccuranceFirstStr = "1st";
        public const string MonthlyOccuranceSecondStr = "2nd";
        public const string MonthlyOccuranceThirdStr = "3rd";
        public const string MonthlyOccuranceFourthStr = "4th";
        public const string MonthlyOccuranceLastStr = "Last";

        public const string MonthlyDaySundayStr = "Sunday";
        public const string MonthlyDayMondayStr = "Monday";
        public const string MonthlyDayTuesdayStr = "Tuesday";
        public const string MonthlyDayWednesdayStr = "Wednesday";
        public const string MonthlyDayThursdayStr = "Thursday";
        public const string MonthlyDayFridayStr = "Friday";
        public const string MonthlyDaySaturdayStr = "Saturday";
        public const string MonthlyDayEveryDay = "Day";
        public const string MonthlyDayWeekdayStr = "Weekday";
        public const string MonthlyDayWeekendDayStr = "Weekend Day";

        public const string FrequencyUnitMinutesStr = "Minute(s)";
        public const string FrequencyUnitHourStr = "Hour(s)";

        #endregion

        #region Fields

        ScheduleJob.ScheduleData m_scheduleData;

        #endregion

        #region DataTypes

        public struct ComboItemData
        {
            public string name;
            public int value;

            public override string ToString()
            {
                return name;
            }
        }

        #endregion

        #region CTOR

        public ScheduleControlForm()
        {
            InitializeComponent();

            DoubleBuffered = true;

        }

        #endregion

        #region Public Methods

        public void setData(ScheduleJob.ScheduleData scheduleDataLocal)
        {
            m_scheduleData = scheduleDataLocal;
            loadData();
        }

        public void getData(out ScheduleJob.ScheduleData scheduleDataOut)
        {
            getDataFromForm();
            Sql.ScheduleJob.BuildDescription(ref m_scheduleData);

            scheduleDataOut = m_scheduleData;

        }
 
        #endregion

        #region Helpers

        private void getDataFromForm()
        {
            //m_scheduleData.Enabled = checkBox_EnableScheduling.Checked;

            //m_scheduleData.snapshotretentionPeriod = (int) numericUpDown_KeepSnapshotDays.Value;

            if(radioButton_Daily.Checked == true)
            {
                m_scheduleData.occurType = ScheduleJob.OccurType.OccursDaily;
            }
            else if (radioButton_Weekly.Checked == true)
            {
                m_scheduleData.occurType = ScheduleJob.OccurType.OccursWeekly;
            }
            else
            {
                m_scheduleData.occurType = ScheduleJob.OccurType.OccursMonthly;
            }

            // Daily values
            // ------------
            m_scheduleData.daily_RepeatRate = (uint)numericUpDown_DailyDays.Value;

            // Weekly Values
            // -------------
            m_scheduleData.weekly_RepeatRate = (uint)numericUpDown_WeeklyWeeks.Value;
            m_scheduleData.weekly_isMonday = checkBox_WeeklyMonday.Checked;
            m_scheduleData.weekly_isTuesday = checkBox_WeeklyTuesday.Checked;
            m_scheduleData.weekly_isWednesday = checkBox_WeeklyWednesday.Checked;
            m_scheduleData.weekly_isThursday = checkBox_WeeklyThursday.Checked;
            m_scheduleData.weekly_isFriday = checkBox_WeeklyFriday.Checked;
            m_scheduleData.weekly_isSaturday = checkBox_WeeklySaturday.Checked;
            m_scheduleData.weekly_isSunday = checkBox_WeeklySunday.Checked;

            // Monthly Values
            // --------------
            m_scheduleData.monthly_type = radioButton_MonthlyDay.Checked ?
                ScheduleJob.MonthlyOccurType.MonthlyOccurDay : ScheduleJob.MonthlyOccurType.MonthlyOccurSpecificDay;
            m_scheduleData.monthly_dayOfMonth = (uint)numericUpDown_MonthlyDay.Value;
            m_scheduleData.monthly_repeatRate = (uint)numericUpDown_MonthlyMonth.Value;
            ComboItemData cid = (ComboItemData)comboBox_MonthlyDay.SelectedItem;
            m_scheduleData.monthly_SpecificDay = (ScheduleJob.MonthlyDay)cid.value;
            cid = (ComboItemData)comboBox_MonthlyDayCount.SelectedItem;
            m_scheduleData.monthly_SpecificOccurance = (ScheduleJob.MonthlyWhichOccurance)cid.value;
            m_scheduleData.monthly_SpecificRepeatRate = (uint)numericUpDown_MonthlyTheMonths.Value;

            // Frequency Values
            // ----------------
            m_scheduleData.freq_Type = radioButton_FreqOnce.Checked ?
                ScheduleJob.FrequencyType.FrequencyOnce : ScheduleJob.FrequencyType.FrequencyEvery;
            m_scheduleData.freq_OnceAtTime = dateTimePicker_FreqOnceTime.Value;
            m_scheduleData.freq_RepeatRate = (uint)numericUpDown_FreqEveryCount.Value;
            cid = (ComboItemData)comboBox_FreqEveryUnit.SelectedItem;
            m_scheduleData.freq_Unit = (ScheduleJob.FrequencyUnit)cid.value;
            m_scheduleData.freq_Start = dateTimePicker_FreqEveryStart.Value;
            m_scheduleData.freq_End = dateTimePicker_FreqEveryEnd.Value;




        }

        private void loadData()
        {
            // Load const
            // ----------
            loadControlsWithConstants();

            EnableControls(m_scheduleData.Enabled, true);

            switch (m_scheduleData.occurType)
            {
                case ScheduleJob.OccurType.OccursDaily:
                    {
                        radioButton_Daily.Checked = true;
                        break;
                    }
                case ScheduleJob.OccurType.OccursWeekly:
                    {
                        radioButton_Weekly.Checked = true;
                        break;
                    }
                case ScheduleJob.OccurType.OccursMonthly:
                    {
                        radioButton_Monthly.Checked = true;
                        break;
                    }
            }


            // Daily values
            // ------------
            if (m_scheduleData.daily_RepeatRate > numericUpDown_DailyDays.Minimum &&
                m_scheduleData.daily_RepeatRate < numericUpDown_DailyDays.Maximum)
            {
                numericUpDown_DailyDays.Value = m_scheduleData.daily_RepeatRate;
            }

            // Weekly values
            // -------------
            if (m_scheduleData.weekly_RepeatRate > numericUpDown_WeeklyWeeks.Minimum &&
                m_scheduleData.weekly_RepeatRate < numericUpDown_WeeklyWeeks.Maximum)
            {
                numericUpDown_WeeklyWeeks.Value = m_scheduleData.weekly_RepeatRate;
            }
            checkBox_WeeklyMonday.Checked = m_scheduleData.weekly_isMonday;
            checkBox_WeeklyTuesday.Checked = m_scheduleData.weekly_isTuesday;
            checkBox_WeeklyWednesday.Checked = m_scheduleData.weekly_isWednesday;
            checkBox_WeeklyThursday.Checked = m_scheduleData.weekly_isThursday;
            checkBox_WeeklyFriday.Checked = m_scheduleData.weekly_isFriday;
            checkBox_WeeklySaturday.Checked = m_scheduleData.weekly_isSaturday;
            checkBox_WeeklySunday.Checked = m_scheduleData.weekly_isSunday;

            // Monthly values
            // --------------
            switch (m_scheduleData.monthly_type)
            {
                case ScheduleJob.MonthlyOccurType.MonthlyOccurDay:
                    {
                        radioButton_MonthlyDay.Checked = true;
                        break;
                    }
                case ScheduleJob.MonthlyOccurType.MonthlyOccurSpecificDay:
                    {
                        radioButton_MonthlyThe.Checked = true;
                        break;
                    }
            }
            if (m_scheduleData.monthly_dayOfMonth > numericUpDown_MonthlyDay.Minimum &&
                m_scheduleData.monthly_dayOfMonth < numericUpDown_MonthlyDay.Maximum)
            {
                numericUpDown_MonthlyDay.Value = m_scheduleData.monthly_dayOfMonth;
            }
            if (m_scheduleData.monthly_repeatRate > numericUpDown_MonthlyMonth.Minimum &&
                m_scheduleData.monthly_repeatRate < numericUpDown_MonthlyMonth.Maximum)
            {
                numericUpDown_MonthlyMonth.Value = m_scheduleData.monthly_repeatRate;
            }
            foreach (ComboItemData cid in comboBox_MonthlyDayCount.Items)
            {
                if (cid.value == (int)m_scheduleData.monthly_SpecificOccurance)
                {
                    comboBox_MonthlyDayCount.SelectedItem = cid;
                    break;
                }
            }
            foreach (ComboItemData cid in comboBox_MonthlyDay.Items)
            {
                if (cid.value == (int)m_scheduleData.monthly_SpecificDay)
                {
                    comboBox_MonthlyDay.SelectedItem = cid;
                    break;
                }
            }
            if (m_scheduleData.monthly_SpecificRepeatRate > numericUpDown_MonthlyMonth.Minimum &&
                m_scheduleData.monthly_SpecificRepeatRate < numericUpDown_MonthlyMonth.Maximum)
            {
                numericUpDown_MonthlyTheMonths.Value = m_scheduleData.monthly_SpecificRepeatRate;
            }

            // Frequency values
            // ----------------
            switch (m_scheduleData.freq_Type)
            {
                case ScheduleJob.FrequencyType.FrequencyOnce:
                    {
                        radioButton_FreqOnce.Checked = true;
                        break;
                    }
                case ScheduleJob.FrequencyType.FrequencyEvery:
                    {
                        radioButton_FreqEvery.Checked = true;
                        break;
                    }
            }
            if (m_scheduleData.freq_OnceAtTime > dateTimePicker_FreqOnceTime.MinDate &&
                m_scheduleData.freq_OnceAtTime < dateTimePicker_FreqOnceTime.MaxDate)
            {
                dateTimePicker_FreqOnceTime.Value = m_scheduleData.freq_OnceAtTime;
            }
            if (m_scheduleData.freq_RepeatRate > numericUpDown_FreqEveryCount.Minimum &&
                m_scheduleData.freq_RepeatRate < numericUpDown_FreqEveryCount.Maximum)
            {
                numericUpDown_FreqEveryCount.Value = m_scheduleData.freq_RepeatRate;
            }
            foreach (ComboItemData cid in comboBox_FreqEveryUnit.Items)
            {
                if (cid.value == (int)m_scheduleData.freq_Unit)
                {
                    comboBox_FreqEveryUnit.SelectedItem = cid;
                    switch (m_scheduleData.freq_Unit)
                    {
                        case ScheduleJob.FrequencyUnit.FreqencyUnitHours:
                            {
                                numericUpDown_FreqEveryCount.Maximum = 24;
                                break;
                            }
                        case ScheduleJob.FrequencyUnit.FreqencyUnitMinutes:
                            {
                                numericUpDown_FreqEveryCount.Maximum = 1440;
                                break;
                            }
                    }
                    break;
                }
            }
            if (m_scheduleData.freq_Start > dateTimePicker_FreqEveryStart.MinDate &&
                m_scheduleData.freq_Start < dateTimePicker_FreqEveryStart.MaxDate)
            {
                dateTimePicker_FreqEveryStart.Value = m_scheduleData.freq_Start;
            }
            if (m_scheduleData.freq_End > dateTimePicker_FreqEveryEnd.MinDate &&
                m_scheduleData.freq_End < dateTimePicker_FreqEveryEnd.MaxDate)
            {
                dateTimePicker_FreqEveryEnd.Value = m_scheduleData.freq_End;
            }


        }

        private void loadControlsWithConstants()
        {
            ComboItemData cData;

            // ComboBox Freq Units
            comboBox_FreqEveryUnit.Items.Clear();
            cData.name = FrequencyUnitMinutesStr;
            cData.value = (int)ScheduleJob.FrequencyUnit.FreqencyUnitMinutes;
            int nPos = comboBox_FreqEveryUnit.Items.Add(cData);
            comboBox_FreqEveryUnit.SelectedIndex = nPos;

            cData.name = FrequencyUnitHourStr;
            cData.value = (int)ScheduleJob.FrequencyUnit.FreqencyUnitHours;
            comboBox_FreqEveryUnit.Items.Add(cData);

            // ComboBox Monthly Count
            comboBox_MonthlyDayCount.Items.Clear();
            cData.name = MonthlyOccuranceFirstStr;
            cData.value = (int)ScheduleJob.MonthlyWhichOccurance.MonthlyOccuranceFirst;
            nPos = comboBox_MonthlyDayCount.Items.Add(cData);
            comboBox_MonthlyDayCount.SelectedIndex = nPos;

            cData.name = MonthlyOccuranceSecondStr;
            cData.value = (int)ScheduleJob.MonthlyWhichOccurance.MonthlyOccuranceSecond;
            comboBox_MonthlyDayCount.Items.Add(cData);

            cData.name = MonthlyOccuranceThirdStr;
            cData.value = (int)ScheduleJob.MonthlyWhichOccurance.MonthlyOccuranceThird;
            comboBox_MonthlyDayCount.Items.Add(cData);

            cData.name = MonthlyOccuranceFourthStr;
            cData.value = (int)ScheduleJob.MonthlyWhichOccurance.MonthlyOccuranceFouth;
            comboBox_MonthlyDayCount.Items.Add(cData);

            cData.name = MonthlyOccuranceLastStr;
            cData.value = (int)ScheduleJob.MonthlyWhichOccurance.MonthlyOccuranceLast;
            comboBox_MonthlyDayCount.Items.Add(cData);

            // ComboBox Monthly Day
            comboBox_MonthlyDay.Items.Clear();
            cData.name = MonthlyDaySundayStr;
            cData.value = (int)ScheduleJob.MonthlyDay.MonthlyDaySunday;
            nPos = comboBox_MonthlyDay.Items.Add(cData);
            comboBox_MonthlyDay.SelectedIndex = nPos;

            cData.name = MonthlyDayMondayStr;
            cData.value = (int)ScheduleJob.MonthlyDay.MonthlyDayMonday;
            nPos = comboBox_MonthlyDay.Items.Add(cData);

            cData.name = MonthlyDayTuesdayStr;
            cData.value = (int)ScheduleJob.MonthlyDay.MonthlyDayTuesday;
            nPos = comboBox_MonthlyDay.Items.Add(cData);

            cData.name = MonthlyDayWednesdayStr;
            cData.value = (int)ScheduleJob.MonthlyDay.MonthlyDayWednesday;
            nPos = comboBox_MonthlyDay.Items.Add(cData);

            cData.name = MonthlyDayThursdayStr;
            cData.value = (int)ScheduleJob.MonthlyDay.MonthlyDayThursday;
            nPos = comboBox_MonthlyDay.Items.Add(cData);

            cData.name = MonthlyDayFridayStr;
            cData.value = (int)ScheduleJob.MonthlyDay.MonthlyDayFriday;
            nPos = comboBox_MonthlyDay.Items.Add(cData);

            cData.name = MonthlyDaySaturdayStr;
            cData.value = (int)ScheduleJob.MonthlyDay.MonthlyDaySaturday;
            nPos = comboBox_MonthlyDay.Items.Add(cData);

            cData.name = MonthlyDayEveryDay;
            cData.value = (int)ScheduleJob.MonthlyDay.MonthlyDayEveryDay;
            nPos = comboBox_MonthlyDay.Items.Add(cData);

            cData.name = MonthlyDayWeekdayStr;
            cData.value = (int)ScheduleJob.MonthlyDay.MonthlyDayWeekday;
            nPos = comboBox_MonthlyDay.Items.Add(cData);

            cData.name = MonthlyDayWeekendDayStr;
            cData.value = (int)ScheduleJob.MonthlyDay.MonthlyDayWeekendDay;
            nPos = comboBox_MonthlyDay.Items.Add(cData);


        }

        private void EnableControls(bool enable, bool enableScheduleCheckbutton)
        {
            if (enable == false || m_scheduleData.Enabled)
            {
                EnableOccurControls(enable);
                EnableMonthlyControls(enable);
                EnableWeeklyControls(enable);
                EnableDailyControls(enable);
                EnableFreqControls(enable);

                if (enableScheduleCheckbutton)
                {
                    if (radioButton_MonthlyDay.Checked)
                    {
                        comboBox_MonthlyDayCount.Enabled = false;
                        comboBox_MonthlyDay.Enabled = false;
                        numericUpDown_MonthlyTheMonths.Enabled = false;
                    }
                    else
                    {
                        numericUpDown_MonthlyDay.Enabled = false;
                        numericUpDown_MonthlyMonth.Enabled = false;
                    }
                    if (radioButton_FreqOnce.Checked)
                    {
                        numericUpDown_FreqEveryCount.Enabled = false;
                        comboBox_FreqEveryUnit.Enabled = false;
                        dateTimePicker_FreqEveryStart.Enabled = false;
                        dateTimePicker_FreqEveryEnd.Enabled = false;
                    }
                    else
                    {
                        dateTimePicker_FreqOnceTime.Enabled = false;
                    }
                }
            }
        }

        private void EnableOccurControls(bool enable)
        {
            //foreach (Control control in groupBox_Occurs.Controls)
            //{
            //    control.Enabled = enable;
            //}
            groupBox_Occurs.Enabled = enable;
        }

        private void EnableMonthlyControls(bool enable)
        {
            //foreach (Control control in groupBox_Monthly.Controls)
            //{
            //    control.Enabled = enable;
            //}
            groupBox_Monthly.Enabled = enable;
        }

        private void EnableWeeklyControls(bool enable)
        {
            //foreach (Control control in groupBox_Weekly.Controls)
            //{
            //    control.Enabled = enable;
            //}
            groupBox_Weekly.Enabled = enable;
        }

        private void EnableDailyControls(bool enable)
        {
            //foreach (Control control in groupBox_Daily.Controls)
            //{
            //    control.Enabled = enable;
            //}
            groupBox_Daily.Enabled = enable;
        }

        private void EnableFreqControls(bool enable)
        {
            //foreach (Control control in groupBox_Freq.Controls)
            //{
            //    control.Enabled = enable;
            //}
            groupBox_Freq.Enabled = enable;
        }

        private void EnableFreqOnceControls(bool enable)
        {
            dateTimePicker_FreqOnceTime.Enabled = enable;
        }

        private void EnableFregEveryControls(bool enable)
        {
            numericUpDown_FreqEveryCount.Enabled = enable;
            comboBox_FreqEveryUnit.Enabled = enable;
            dateTimePicker_FreqEveryStart.Enabled = enable;
            dateTimePicker_FreqEveryEnd.Enabled = enable;
        }

        private void EnableMontlyDayControls(bool enable)
        {
            numericUpDown_MonthlyDay.Enabled = enable;
            numericUpDown_MonthlyMonth.Enabled = enable;
        }

        private void EnableMonthlyTheControls(bool enable)
        {
            numericUpDown_MonthlyTheMonths.Enabled = enable;
            comboBox_MonthlyDay.Enabled = enable;
            comboBox_MonthlyDayCount.Enabled = enable;
        }

        #endregion

        #region Events

        private void radioButton_Daily_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Daily.Checked)
            {
                groupBox_Daily.Visible = true;
                groupBox_Weekly.Visible = false;
                groupBox_Monthly.Visible = false;

                groupBox_Daily.Show();
            }
        }

        private void radioButton_Weekly_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Weekly.Checked)
            {
                groupBox_Daily.Visible = false;
                groupBox_Weekly.Visible = true;
                groupBox_Monthly.Visible = false;

                groupBox_Weekly.Show();
            }

        }

        private void radioButton_Monthly_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Monthly.Checked)
            {
                groupBox_Daily.Visible = false;
                groupBox_Weekly.Visible = false;
                groupBox_Monthly.Visible = true;

                groupBox_Monthly.Show();
            }

        }

        private void radioButton_FreqOnce_CheckedChanged(object sender, EventArgs e)
        {
            EnableFreqOnceControls(radioButton_FreqOnce.Checked);
            EnableFregEveryControls(!radioButton_FreqOnce.Checked);
        }

        private void radioButton_FreqEvery_CheckedChanged(object sender, EventArgs e)
        {
            EnableFreqOnceControls(!radioButton_FreqEvery.Checked);
            EnableFregEveryControls(radioButton_FreqEvery.Checked);

        }

        private void radioButton_MonthlyDay_CheckedChanged(object sender, EventArgs e)
        {
            EnableMontlyDayControls(radioButton_MonthlyDay.Checked);
            EnableMonthlyTheControls(!radioButton_MonthlyDay.Checked);
        }

        private void radioButton_MonthlyThe_CheckedChanged(object sender, EventArgs e)
        {
            EnableMontlyDayControls(!radioButton_MonthlyThe.Checked);
            EnableMonthlyTheControls(radioButton_MonthlyThe.Checked);
        }

        private void comboBox_FreqEveryUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboItemData cid = (ComboItemData)comboBox_FreqEveryUnit.SelectedItem;
            switch ((ScheduleJob.FrequencyUnit)cid.value)
            {
                case ScheduleJob.FrequencyUnit.FreqencyUnitHours:
                    {
                        numericUpDown_FreqEveryCount.Maximum = 24;
                        break;
                    }
                case ScheduleJob.FrequencyUnit.FreqencyUnitMinutes:
                    {
                        numericUpDown_FreqEveryCount.Maximum = 1440;
                        break;
                    }
            }
            
        }

        private void ScheduleControlForm_Load(object sender, EventArgs e)
        {
        }
   
        #endregion

    }
}
