using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinListView;

namespace Idera.SQLsecure.UI.Console.Controls
{
    public partial class SelectSnapshot : UserControl
    {
        #region CTOR

        public SelectSnapshot()
        {
            InitializeComponent();
//            m_server = server;

            //ultraListViewSnapshots.

            //_listView_Snapshots.SmallImageList = AppIcons.AppImageList16();
        }

        #endregion

        #region Fields

        private Sql.RegisteredServer m_server;
        private int m_SelectedSnapshotId;

        public delegate void SelectedSnapshotChanged(bool SnapshotSelected);
        SelectedSnapshotChanged m_SelectedSnapshotChangedDelegate = null;

        #endregion

        #region Properties

        public int SelectedSnapshotId
        {
            get { return m_SelectedSnapshotId; }
        }

        public void RegisterSnapshotChangeDelegate(SelectedSnapshotChanged value)
        {
            m_SelectedSnapshotChangedDelegate += value;
        }

        #endregion

        #region Helpers

        private void showHelpTopic()
        {
            Program.gController.ShowTopic(Utility.Help.SelectSnapshotHelpTopic);
        }

        public bool LoadSnapshots(Sql.RegisteredServer server)
        {
            m_server = server;

            // Get the snapshot list.
            Sql.Snapshot.SnapshotList snapshots = null;
            try
            {
                snapshots = Sql.Snapshot.LoadSnapshots(m_server.ConnectionName);
            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.CantGetSnapshots, ex);
                return false;
            }

            // Fill the list view.
            string status;
            //LinkLabel properties;
            ultraListViewSnapshots.Items.Clear();
            foreach (Sql.Snapshot snap in snapshots)
            {
                // make sure it is a valid snapshot to explore before adding to list 
                if ((snap.Status == Utility.Snapshot.StatusSuccessful) ||
                    (snap.Status == Utility.Snapshot.StatusWarning))
                {
                    if (String.Compare(snap.Status, Utility.Snapshot.StatusWarning, true) == 0)
                    {
                        status = snap.SnapshotComment;
                    }
                    else
                    {
                        status = snap.StatusText;
                    }

                    UltraListViewItem li = ultraListViewSnapshots.Items.Add(null, snap.SnapshotName);
                    li.Tag = snap.SnapshotId;
                    li.SubItems["Icon"].Value = snap.Icon;
                    li.SubItems["Status"].Value = status;
                    li.SubItems["Baseline"].Value = snap.Baseline;

                    //_listView_Snapshots.Items.Add("", snap.IconIndex);
                    //_listView_Snapshots.Items[_listView_Snapshots.Items.Count - 1].SubItems.Add(snap.SnapshotName);
                    //_listView_Snapshots.Items[_listView_Snapshots.Items.Count - 1].SubItems.Add(status);
                    //_listView_Snapshots.Items[_listView_Snapshots.Items.Count - 1].SubItems.Add(snap.Baseline);
                    //_listView_Snapshots.Items[_listView_Snapshots.Items.Count - 1].Tag = snap.SnapshotId;
                }
            }

            if (ultraListViewSnapshots.Items.Count > 0)
            {
                ultraListViewSnapshots.SelectedItems.Clear();
                ultraListViewSnapshots.SelectedItems.Add(ultraListViewSnapshots.Items[0]);
            }


            return true;
        }

        #endregion

        #region Events

       
        private void ultraListViewSnapshots_ItemSelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        {
            bool IsSnapshotSelected = false;

            if (ultraListViewSnapshots.SelectedItems.Count > 0)
            {
                m_SelectedSnapshotId = (int)ultraListViewSnapshots.SelectedItems[0].Tag;
                IsSnapshotSelected = true;
            }

            if (m_SelectedSnapshotChangedDelegate != null)
            {
                m_SelectedSnapshotChangedDelegate(IsSnapshotSelected);
            }
        }

        #endregion


    }
}
