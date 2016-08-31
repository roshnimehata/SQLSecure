using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Idera.SQLsecure.UI.Console.Utility;
using Idera.SQLsecure.UI.Console.Sql;
using Idera.SQLsecure.UI.Console.Controls;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_GroomingSchedule : Idera.SQLsecure.UI.Console.Controls.BaseDialogForm
    {
        #region Fields

        private ScheduleJob.ScheduleData m_scheduleData;
        private Guid m_jobID;

        #endregion

        #region CTORS

        public Form_GroomingSchedule()
        {
            InitializeComponent();

            m_jobID = Sql.ScheduleJob.GetGroomingJobSchedule(Program.gController.Repository.ConnectionString, out m_scheduleData );
            textBox_ScheduleDescription.Text = m_scheduleData.Description;

            checkBox_EnableScheduling.Checked = m_scheduleData.Enabled;

            if (Sql.ScheduleJob.IsSQLAgentStarted(Program.gController.Repository.ConnectionString))
            {
                label_AgentStatus.Text = Utility.ErrorMsgs.SQLServerAgentStarted;
                pictureBox_AgentStatus.Image = AppIcons.AppImage48(AppIcons.EnumImageList48.AgentStarted);
            }
            else
            {
                label_AgentStatus.Text = Utility.ErrorMsgs.SQLServerAgentStopped;
                pictureBox_AgentStatus.Image = AppIcons.AppImage48(AppIcons.EnumImageList48.AgentStopped);
            }

        }
        #endregion

        #region Public Methods

        public static void Process( )
        {
            Form_GroomingSchedule form = new Form_GroomingSchedule();
            
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (form.m_jobID != Guid.Empty)
                {
                    Sql.ScheduleJob.RemoveJob(Program.gController.Repository.ConnectionString, form.m_jobID, null);
                }
                Sql.ScheduleJob.AddGroomingJob(Program.gController.Repository.ConnectionString,
                                               form.m_scheduleData);
            }
        }

        #endregion

        #region Helpers

        #endregion

        #region Event Handlers

        private void button_ChangeSchedule_Click(object sender, EventArgs e)
        {
            Form_ScheduleJob form = new Form_ScheduleJob(m_scheduleData);
            if (form.ShowDialog() == DialogResult.OK)
            {
                form.GetScheduleData(out m_scheduleData);
                ScheduleJob.BuildDescription(ref m_scheduleData);
                textBox_ScheduleDescription.Text = m_scheduleData.Description;
            }
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        #endregion

        private void checkBox_EnableScheduling_CheckedChanged(object sender, EventArgs e)
        {
            m_scheduleData.Enabled = checkBox_EnableScheduling.Checked;
            button_ChangeSchedule.Enabled = m_scheduleData.Enabled;

            ScheduleJob.BuildDescription(ref m_scheduleData);
            textBox_ScheduleDescription.Text = m_scheduleData.Description;
        }

        private void button_Help_Click(object sender, EventArgs e)
        {
            ShowHelpTopic();
        }

        private void Form_GroomingSchedule_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            ShowHelpTopic();
        }
        private void ShowHelpTopic()
        {
            Program.gController.ShowTopic(Utility.Help.GroomingScheduleHelpTopic);
        }
    }
}

