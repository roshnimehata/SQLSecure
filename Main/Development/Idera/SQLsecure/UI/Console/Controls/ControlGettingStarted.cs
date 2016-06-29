using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Idera.SQLsecure.UI.Console.Utility;
using Idera.SQLsecure.UI.Console.Sql;
using Infragistics.Win.UltraWinListView;
using Policy=Idera.SQLsecure.UI.Console.Sql.Policy;

namespace Idera.SQLsecure.UI.Console.Controls
{
    public partial class ControlGettingStarted : UserControl
    {

        public delegate void TabPageChanged(int pageToGoBack, int pagesToGoForward);
        TabPageChanged m_TabPageChangedDelegate = null;


        public ControlGettingStarted()
        {
            InitializeComponent();

            ultraTabControl1.DrawFilter = new HideFocusRectangleDrawFilter();
        }

        #region Public Methods

        public void LoadData()
        {

            //Registered Servers
            Program.gController.Repository.RefreshRegisteredServers();
            ultraListView_RegisterServer.Items.Clear();
            foreach( RegisteredServer r in Program.gController.Repository.RegisteredServers)
            {
                UltraListViewItem li = ultraListView_RegisterServer.Items.Add(null, r.ConnectionName);
                li.Tag = r;
                li.SubItems["Manage"].Appearance.ForeColor = SystemColors.ActiveCaption;
                li.SubItems["Manage"].Appearance.FontData.Underline = Infragistics.Win.DefaultableBoolean.True;
                li.SubItems["Manage"].Value = "Manage " + r.ConnectionName;
            }

            // Data Collection
            ultraListView_CollectData.Items.Clear();
            foreach (RegisteredServer r in Program.gController.Repository.RegisteredServers)
            {
                UltraListViewItem li = ultraListView_CollectData.Items.Add(null, r.ConnectionName);
                li.Tag = r;
                string jobStatusStr = Sql.ScheduleJob.GetJobStatus(Program.gController.Repository.ConnectionString,
                                                                                   r.JobId);
                if (string.Compare(jobStatusStr, Sql.ScheduleJob.JobStatus_Running, true) == 0)
                {
                    li.SubItems["Last"].Value = "In Progress";
                    li.SubItems["Collect"].Value = "In Progress";
                }
                else
                {
                    li.SubItems["Last"].Value = r.LastCollectionTime;
                    li.SubItems["Collect"].Appearance.ForeColor = SystemColors.ActiveCaption;
                    li.SubItems["Collect"].Appearance.FontData.Underline = Infragistics.Win.DefaultableBoolean.True;
                    li.SubItems["Collect"].Value = "Collect Data Now";
                }
                li.SubItems["Next"].Appearance.ForeColor = SystemColors.ActiveCaption;
                li.SubItems["Next"].Appearance.FontData.Underline = Infragistics.Win.DefaultableBoolean.True;
                if (string.IsNullOrEmpty(r.NextCollectionTime))
                {
                    li.SubItems["Next"].Value = "Enable Schedule";
                }
                else
                {
                    li.SubItems["Next"].Value = r.NextCollectionTime;
                }
            }

            // Policies
            Program.gController.Repository.RefreshPolicies();
            ultraListViewPolicies.Items.Clear();
            foreach( Policy p in Program.gController.Repository.Policies)
            {
                UltraListViewItem li = ultraListViewPolicies.Items.Add(null, p.PolicyName);
                li.Tag = p;
                li.SubItems["Manage"].Appearance.ForeColor = SystemColors.ActiveCaption;
                li.SubItems["Manage"].Appearance.FontData.Underline = Infragistics.Win.DefaultableBoolean.True;
                li.SubItems["Manage"].Value = "Manage " + p.PolicyName;                
            }
        }

        public bool NextPage()
        {
            int nPage = ultraTabControl1.SelectedTab.Index + 1;
            if(nPage < ultraTabControl1.Tabs.Count)
            {
                ultraTabControl1.SelectedTab = ultraTabControl1.Tabs[nPage];
            }

            return nPage < ultraTabControl1.Tabs.Count-1;
        }

        public bool BackPage()
        {
            int nPage = ultraTabControl1.SelectedTab.Index - 1;
            if(nPage >= 0)
            {
                ultraTabControl1.SelectedTab = ultraTabControl1.Tabs[nPage];
            }

            return (nPage > 0);
        }

        public void RegisterTabPageChangedDelegate(TabPageChanged value)
        {
            m_TabPageChangedDelegate += value;
        }

        #endregion


        #region Events

        private void ultraButton1_Click(object sender, EventArgs e)
        {
            Forms.Form_WizardRegisterSQLServer.Process();
            LoadData();
        }

        private void ultraButton2_Click(object sender, EventArgs e)
        {
            Forms.Form_WizardCreatePolicy.Process();
            LoadData();
        }
      

        private void ultraListView_RegisterServer_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                UltraListViewSubItem item;
                item = ultraListView_RegisterServer.SubItemFromPoint(e.Location);
                if (item != null)
                {
                    if (item.Key == "Manage")
                    {
                        RegisteredServer rServer = (RegisteredServer)item.Item.Tag;
                        if(rServer != null)
                        {
                            Forms.Form_SqlServerProperties.Process(rServer.ConnectionName, Forms.Form_SqlServerProperties.RequestedOperation.GeneralProperties, Program.gController.isAdmin);
                            LoadData();
                        }
                    }
                }
            }
        }

        private void ultraListView_RegisterServer_MouseMove(object sender, MouseEventArgs e)
        {
            UltraListViewSubItem item;
            item = ultraListView_RegisterServer.SubItemFromPoint(e.Location);
            if (item != null)
            {
                if (item.Key == "Manage")
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

        private void ultraListView_CollectData_MouseMove(object sender, MouseEventArgs e)
        {
            UltraListViewSubItem item;
            item = ultraListView_CollectData.SubItemFromPoint(e.Location);
            if (item != null)
            {
                if (item.Key == "Collect" || item.Key == "Next")
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

        private void ultraListView_CollectData_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                UltraListViewSubItem item;
                item = ultraListView_CollectData.SubItemFromPoint(e.Location);
                if (item != null)
                {
                    if (item.Key == "Collect")
                    {
                        RegisteredServer rServer = (RegisteredServer)item.Item.Tag;
                        if (rServer != null)
                        {
                            try
                            {
                                Forms.Form_StartSnapshotJobAndShowProgress.Process(rServer.ConnectionName);
                                System.Threading.Thread.Sleep(1000);
                                LoadData();
                            }
                            catch (Exception ex)
                            {
                                Utility.MsgBox.ShowError(Utility.ErrorMsgs.SQLsecureDataCollection, Utility.ErrorMsgs.CantRunDataCollection, ex);
                            }
                        }
                    }
                    else if(item.Key == "Next")
                    {
                        RegisteredServer rServer = (RegisteredServer)item.Item.Tag;
                        if (rServer != null)
                        {
                            Forms.Form_SqlServerProperties.Process(rServer.ConnectionName, Forms.Form_SqlServerProperties.RequestedOperation.EditSchedule, Program.gController.isAdmin);
                            LoadData();
                        }                        
                    }
                }
            }
        }

        private void ultraListViewPolicies_MouseMove(object sender, MouseEventArgs e)
        {
            UltraListViewSubItem item;
            item = ultraListViewPolicies.SubItemFromPoint(e.Location);
            if (item != null)
            {
                if (item.Key == "Manage")
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

        private void ultraListViewPolicies_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                UltraListViewSubItem item;
                item = ultraListViewPolicies.SubItemFromPoint(e.Location);
                if (item != null)
                {
                    if (item.Key == "Manage")
                    {
                        Policy policy = (Policy)item.Item.Tag;
                        if (policy != null)
                        {
                            Forms.Form_PolicyProperties.Process(policy.PolicyId, policy.AssessmentId, Program.gController.isAdmin, Forms.Form_PolicyProperties.RequestedOperation.ConfigureMetrics);
                            LoadData();
                        }
                    }
                }
            }
        }

        private void ultraTabControl1_ActiveTabChanged(object sender, Infragistics.Win.UltraWinTabControl.ActiveTabChangedEventArgs e)
        {
            int nPage = e.Tab.Index;

            int nPagesForward = ultraTabControl1.Tabs.Count - nPage - 1;
            int nPagesBack = nPage;

            if(m_TabPageChangedDelegate != null)
            {
                m_TabPageChangedDelegate(nPagesBack, nPagesForward);
            }

        }

        #endregion




    }
}
