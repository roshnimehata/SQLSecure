using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_StartSnapshotJobAndShowProgress : Idera.SQLsecure.UI.Console.Controls.BaseDialogForm
    {
        public const string dataCollectionInProgress = "Collecting...";


        private Guid m_jobID;
        private int m_currentStep = 0;
        private int m_snapshotID = 0;
        private int m_savedsnapshotID;
        private bool m_jobStarted = false;
        private bool m_jobFinished = false;
        private bool m_jobErrored = false;
        private int m_TimerCount = 0;
        private int m_JobFinishedCount = 0;
        private Sql.RegisteredServer m_rServer;
        private bool m_snapshotAddedToTree = false;
        Form m_mainForm = null;
        private int m_hideCount = 0;
        private int m_showCount = 0;

        public Form_StartSnapshotJobAndShowProgress(Form mainForm, Sql.RegisteredServer rServer, int lastsnapshotID)
        {
            InitializeComponent();

            m_jobID = Guid.Empty;
            m_mainForm = mainForm;
            m_rServer = rServer;
            m_snapshotID = 0;
            m_jobStarted = false;
            m_jobFinished = false;
            m_jobErrored = false;
            m_savedsnapshotID = lastsnapshotID;


            timer_Status.Interval = 500; // seconds
            textBox_FinalStatus.Text = dataCollectionInProgress;
            pictureBox_FinalStatus.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.Working;
            UpdateStepGraphics();

            button_HideandNotify.Enabled = false;
            button_OK.Enabled = false;

            this.Text = string.Format("Snapshot Data Collection - {0}", rServer.ConnectionName);
        }
        
        public static void Process(string newConnection, Guid jobID)
        {
            Guid newJobID = jobID;

            // Get server from internal cache
            // ------------------------------
            Sql.RegisteredServer rServer = Program.gController.Repository.GetServer(newConnection);

            if (rServer == null)
            {
                Program.gController.Repository.RefreshRegisteredServers();
                rServer = Program.gController.Repository.GetServer(newConnection);
            }

            if (rServer != null)
            {
                int snapshotID = rServer.GetLatestSnapshotId();

                Form mainForm = null;
                FormCollection fc = Application.OpenForms;
                foreach(Form f in fc)
                {
                    if(f.Name == "MainForm")
                    {
                        mainForm = f;
                        break;
                    }
                }

                Form_StartSnapshotJobAndShowProgress form = new Forms.Form_StartSnapshotJobAndShowProgress(mainForm, rServer, snapshotID);
                Guid realJobID;
                if (rServer.StartJob(out realJobID))
                {
                    rServer.SetJobId(realJobID);
                    form.m_jobID = realJobID;
                    form.m_rServer.SetStartSnapshotForm(form);
                    form.timer_Status.Start();
                    form.Show(mainForm);
                }
                else
                {
                    form.Close();
                }
            }

        }

        public static void Process(string newConnection)
        {
            Sql.RegisteredServer rServer = null;
            Sql.RegisteredServer.GetServer(Program.gController.Repository.ConnectionString, newConnection, out rServer);

            if (rServer == null)
            {
                Utility.MsgBox.ShowWarning(Utility.ErrorMsgs.SQLsecureDataCollection, Utility.ErrorMsgs.ServerNotRegistered);
                return;
            }

            Process(newConnection, rServer.JobId);
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            m_showCount = -1;
            m_hideCount = 10;
            m_rServer.ShowWhenDataCollectionCompletes = false;
            if (m_jobFinished)
            {
                timer_Status.Stop();
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                timer_Status.Start();
            }
        }

        private void button_HideandNotify_Click(object sender, EventArgs e)
        {
            m_rServer.ShowWhenDataCollectionCompletes = true;
            m_hideCount = 1;
            m_showCount = -1;
            timer_Status.Stop();
            timer_Status.Interval = 10;
            timer_Status.Start();
        }


        private void UpdateStepGraphics()
        {
            Font fontBold = new Font(label_Step1.Font, FontStyle.Bold);
            Font fontRegular = new Font(label_Step1.Font, FontStyle.Regular);

            switch (m_currentStep)
            {
                case 0:
                case 1:
                    pictureBox_Step1.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.Working;
                    pictureBox_Step2.Image = null;
                    pictureBox_Step3.Image = null;
                    pictureBox_Step4.Image = null;
                    pictureBox_Step5.Image = null;
                    pictureBox_Step6.Image = null;
                    label_Step1.Font = fontBold;
                    label_Step2.Font = 
                    label_Step3.Font =
                    label_Step4.Font =
                    label_Step5.Font =
                    label_Step6.Font = fontRegular;
                    break;
                case 2:
                    pictureBox_Step1.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.check_32;
                    pictureBox_Step2.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.Working;
                    pictureBox_Step3.Image = null;
                    pictureBox_Step4.Image = null;
                    pictureBox_Step5.Image = null;
                    pictureBox_Step6.Image = null;
                    label_Step1.Font = 
                    label_Step2.Font = fontBold;
                    label_Step3.Font =
                    label_Step4.Font =
                    label_Step5.Font =
                    label_Step6.Font = fontRegular;
                    break;
                case 3:
                    pictureBox_Step1.Image =
                    pictureBox_Step2.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.check_32;
                    pictureBox_Step3.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.Working;
                    pictureBox_Step4.Image = null;
                    pictureBox_Step5.Image = null;
                    pictureBox_Step6.Image = null;
                    label_Step1.Font =
                    label_Step2.Font = 
                    label_Step3.Font = fontBold;
                    label_Step4.Font =
                    label_Step5.Font =
                    label_Step6.Font = fontRegular;
                    break;
                case 4:
                    pictureBox_Step1.Image = 
                    pictureBox_Step2.Image =
                    pictureBox_Step3.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.check_32;
                    pictureBox_Step4.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.Working;
                    pictureBox_Step5.Image = null;
                    pictureBox_Step6.Image = null;
                    label_Step1.Font =
                    label_Step2.Font =
                    label_Step3.Font =
                    label_Step4.Font = fontBold;
                    label_Step5.Font =
                    label_Step6.Font = fontRegular;
                    break;
                case 5:
                    pictureBox_Step1.Image = 
                    pictureBox_Step2.Image = 
                    pictureBox_Step3.Image =
                    pictureBox_Step4.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.check_32;
                    pictureBox_Step5.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.Working;
                    pictureBox_Step6.Image = null;
                    label_Step1.Font =
                    label_Step2.Font =
                    label_Step3.Font = 
                    label_Step4.Font =
                    label_Step5.Font = fontBold;
                    label_Step6.Font = fontRegular; 
                    break;
                case 6:
                    pictureBox_Step1.Image = 
                    pictureBox_Step2.Image = 
                    pictureBox_Step3.Image = 
                    pictureBox_Step4.Image =
                    pictureBox_Step5.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.check_32;
                    pictureBox_Step6.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.Working;
                    label_Step1.Font =
                    label_Step2.Font =
                    label_Step3.Font =
                    label_Step4.Font = 
                    label_Step5.Font = fontBold;
                    label_Step6.Font = fontRegular;
                    break;

                case 7:
                    pictureBox_Step1.Image =
                    pictureBox_Step2.Image =
                    pictureBox_Step3.Image =
                    pictureBox_Step4.Image =
                    pictureBox_Step5.Image =
                    pictureBox_Step6.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.check_32;
                    label_Step1.Font =
                    label_Step2.Font =
                    label_Step3.Font =
                    label_Step4.Font = 
                    label_Step5.Font = 
                    label_Step6.Font = fontBold;
                    break;
            }
            this.Refresh();

        }

        private void SetStepImageToComplete()
        {
            Font fontBold = new Font(label_Step4.Font, FontStyle.Bold);
            pictureBox_Step1.Image =
            pictureBox_Step2.Image =
            pictureBox_Step3.Image =
            pictureBox_Step4.Image =
            pictureBox_Step5.Image =
            pictureBox_Step6.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.check_32;
            label_Step1.Font =
            label_Step2.Font =
            label_Step3.Font =
            label_Step4.Font =
            label_Step5.Font =
            label_Step6.Font = fontBold;

        }

        private void SetStepImageToError()
        {
            switch (m_currentStep)
            {
                case 0:
                case 1:
                    pictureBox_Step1.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.error_32;
                    break;
                case 2:
                    pictureBox_Step1.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.check_32;
                    pictureBox_Step2.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.error_32;
                    break;
                case 3:
                    pictureBox_Step1.Image =
                    pictureBox_Step2.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.check_32;
                    pictureBox_Step3.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.error_32;
                    break;
                case 4:
                    pictureBox_Step1.Image =
                    pictureBox_Step2.Image =
                    pictureBox_Step3.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.check_32;
                    pictureBox_Step4.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.error_32;
                    break;
                case 5:
                    pictureBox_Step1.Image =
                    pictureBox_Step2.Image =
                    pictureBox_Step3.Image =
                    pictureBox_Step4.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.check_32;
                    pictureBox_Step5.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.error_32;
                    break;
                case 6:
                    pictureBox_Step1.Image =
                    pictureBox_Step2.Image =
                    pictureBox_Step3.Image =
                    pictureBox_Step4.Image =
                    pictureBox_Step5.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.check_32;
                    pictureBox_Step6.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.error_32;
                    break;
            }           
        }

        private void HideDialogWithFade()
        {
            timer_Status.Stop();
            timer_Status.Interval = 10;
            if (m_hideCount > 10)
            {
                this.Opacity = 0.0;
                m_hideCount = -1;
                this.Visible = false;
                m_mainForm.Focus();
            }
            else
            {
                this.Opacity = ((float)(10 - m_hideCount) / 10);
                m_hideCount++;
            }
            this.Refresh();
            timer_Status.Start();
        }

        public void ShowDialogFromFade()
        {
            timer_Status.Stop();
            timer_Status.Interval = 10;
            this.Visible = true;
            if (m_showCount < 0)
            {
                m_showCount = 0;
            }
            if (m_showCount > 10)
            {
                this.Opacity = 1.0;
                m_showCount = 0;
            }
            else
            {
                this.Opacity = ((float)(m_showCount) / 10);
                m_showCount++;
            }
            this.Refresh();
            timer_Status.Start();
        }

        private void SetProgressImages(ref bool bStateChange, ref Image FinalImage, ref string FinalText)
        {
            string comment;
            m_snapshotID = m_rServer.GetLatestSnapshotId();
            if (m_savedsnapshotID != m_snapshotID)
            {
                if (!m_snapshotAddedToTree)
                {
                    Program.gController.AddSnapshotToServer(m_rServer.ConnectionName, m_snapshotID);
                    m_snapshotAddedToTree = true;
                    button_OK.Enabled = true;
                    button_HideandNotify.Enabled = true;
                    m_currentStep = 2;
                    bStateChange = true;
                }

                string status = m_rServer.GetCurrentJobStatus(out comment, m_snapshotID);

                if (string.Compare(status, Utility.Snapshot.StatusInProgress, true) == 0)
                {
                    string stepFmt = ("In Progress - completed steps {0} of 7");
                    string stepText = string.Format(stepFmt, 1);
                    int x = 1;
                    for (x = 1; x < 8; x++)
                    {
                        stepText = string.Format(stepFmt, x);
                        if (string.Compare(comment, stepText, true) == 0)
                        {
                            if (x > m_currentStep)
                            {
                                m_currentStep = x;
                                bStateChange = true;
                            }
                            break;
                        }
                    }
                }
                else if ((string.Compare(status, Utility.Snapshot.StatusSuccessful, true) == 0))
                {
                    SetStepImageToComplete();
                    FinalImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.check_50;
                    FinalText = Utility.ErrorMsgs.JobSucceededMsg;
                    m_jobFinished = true;
                }
                else if (string.Compare(status, Utility.Snapshot.StatusError, true) == 0)
                {
                    SetStepImageToError();
                    FinalImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.error_50;
                    FinalText = Utility.ErrorMsgs.JobFailedMsg;
                    FinalText += string.Format("\r\n\r\nError:  {0}", comment);
                    m_jobFinished = true;
                }
                else if (string.Compare(status, Utility.Snapshot.StatusWarning, true) == 0)
                {
                    SetStepImageToComplete();
                    FinalImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.warning_50;
                    FinalText = Sql.Snapshot.GetFormattedWarnings(comment);
                    m_jobFinished = true;
                }
            }
        }


        private void SetJobErroredState(ref Image FinalImage, ref string FinalText)
        {
            m_snapshotID = m_rServer.GetLatestSnapshotId();
            if (m_savedsnapshotID != m_snapshotID)
            {
                if (!m_snapshotAddedToTree)
                {
                    Program.gController.AddSnapshotToServer(m_rServer.ConnectionName, m_snapshotID);
                    m_snapshotAddedToTree = true;
                }
            }
            m_jobFinished = true;
            m_jobErrored = true;
            button_HideandNotify.Enabled = false;
            button_OK.Enabled = true;
            FinalText = Utility.ErrorMsgs.JobFailedMsg;
            FinalImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.error_50;
            SetStepImageToError();
        }

        private void timer_Staus_Tick(object sender, EventArgs e)
        {
            if(Guid.Empty == m_jobID)
            {
                timer_Status.Stop();
                if (!this.Visible)
                {
                    Close();
                    return;
                }
            }
            bool bStateChange = false;
            if (m_hideCount > 0)
            {
                HideDialogWithFade();
                return;
            }
            else if (m_showCount > 0)
            {
                ShowDialogFromFade();
                return;
            }
            string FinalText = dataCollectionInProgress;
            Image FinalImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.Working;
            m_TimerCount++;
            if (!m_jobStarted && m_TimerCount > 10)
            {
                SetJobErroredState(ref FinalImage, ref FinalText);
            }

            timer_Status.Stop();
            timer_Status.Interval = 500;
            string jobStatus = Sql.ScheduleJob.GetJobStatus(Program.gController.Repository.ConnectionString, m_jobID);

            if (string.Compare(jobStatus, Sql.ScheduleJob.JobStatus_Succeeded, true) == 0)
            {
                if (m_jobStarted && !m_jobFinished)
                {
                    m_JobFinishedCount++;
                    if (m_JobFinishedCount > 10)
                    {
                        SetJobErroredState(ref FinalImage, ref FinalText);
                    }
                }
                else if(m_jobErrored)
                {
                    SetJobErroredState(ref FinalImage, ref FinalText);
                }
            }
            else if (string.Compare(jobStatus, Sql.ScheduleJob.JobStatus_Running, true) == 0)
            {
                if (!m_jobStarted)
                {
                    m_jobStarted = true;
                    timer_Status.Interval = 1000;
                    m_rServer.DataCollectionInProgress = true;
                    button_OK.Enabled = true;
                    button_HideandNotify.Enabled = true;
                }
            }
            else if (string.Compare(jobStatus, Sql.ScheduleJob.JobStatus_Failed, true) == 0)
            {
                if (m_jobStarted)
                {
                    SetJobErroredState(ref FinalImage, ref FinalText);
                }
            }
            else if (string.Compare(jobStatus, Sql.ScheduleJob.JobStatus_NotRunning, true) == 0)
            {
                // Do nothing, just keep waiting.
            }
            else
            {
                FinalText = Utility.ErrorMsgs.JobFailedMsg;
                FinalImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.error_50;
            }

            if (m_jobStarted && !m_jobErrored)
            {
                SetProgressImages(ref bStateChange, ref FinalImage, ref FinalText);
            }

            if (textBox_FinalStatus.Text != FinalText)
            {
                textBox_FinalStatus.Text = FinalText;
                pictureBox_FinalStatus.Image = FinalImage;
                this.Refresh();
            }
            if (!m_jobFinished)
            {
                if (bStateChange)
                {
                    UpdateStepGraphics();
                }
            }
            else
            {
                if (m_hideCount < 0 && m_showCount < 0)
                {
                    Close();
                    return;
                }
                button_HideandNotify.Enabled = false;
                button_OK.Enabled = true;
            }
            if (!m_jobFinished)
            {
                timer_Status.Start();
            }
        }

        private void Form_StartSnapshotJobAndShowProgress_Load(object sender, EventArgs e)
        {
            if(m_mainForm != null)
            {
                Point winLoc = m_mainForm.DesktopLocation;
                int x = winLoc.X + (m_mainForm.ClientRectangle.Width - ClientRectangle.Width) / 2;
                int y = winLoc.Y + (m_mainForm.ClientRectangle.Height - ClientRectangle.Height) / 2;
                this.SetDesktopLocation(x, y);

                Activate();
            }
        }


    }
}

