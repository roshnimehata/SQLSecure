using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_ScheduleJob : Form
    {

        public Form_ScheduleJob(Sql.ScheduleJob.ScheduleData scheduledata)
        {
            InitializeComponent();

            scheduleControlForm1.setData(scheduledata);
        }

        public void GetScheduleData(out Sql.ScheduleJob.ScheduleData scheduledata)
        {
            scheduleControlForm1.getData(out scheduledata);
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            Sql.ScheduleJob.ScheduleData scheduleData;
            scheduleControlForm1.getData(out scheduleData);
            if (scheduleData.occurType == Idera.SQLsecure.UI.Console.Sql.ScheduleJob.OccurType.OccursWeekly &&
                !scheduleData.weekly_isSunday &&
                !scheduleData.weekly_isMonday &&
                !scheduleData.weekly_isTuesday &&
                !scheduleData.weekly_isWednesday &&
                !scheduleData.weekly_isThursday &&
                !scheduleData.weekly_isFriday &&
                !scheduleData.weekly_isSaturday)
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.ScheduleCaption, Utility.ErrorMsgs.ScheduleInvalidNoWeekdaySelected);
                return;
            }
            DialogResult = DialogResult.OK;
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void button_Help_Click(object sender, EventArgs e)
        {
           // ShowHelpTopic();
        }

        private void Form_ScheduleJob_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
          //  ShowHelpTopic();
        }

        private void ShowHelpTopic()
        {
            Program.gController.ShowTopic(Utility.Help.ScheduleHelpTopic);
        }

    }
}