using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Controls;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_SelectSnapshot : Idera.SQLsecure.UI.Console.Controls.BaseDialog
    {
        #region Ctors

        public Form_SelectSnapshot(Sql.RegisteredServer server)
        {
            InitializeComponent();

            m_server = server;

            _listView_Snapshots.SmallImageList = AppIcons.AppImageList16();
        }

        #endregion

        #region Fields

        private Sql.RegisteredServer m_server;
        private int m_SelectedSnapshotId;

        #endregion

        #region Properties

        public int SelectedSnapshotId
        {
            get { return m_SelectedSnapshotId; }
        }

        #endregion

        #region Helpers

        private void showHelpTopic()
        {
            Program.gController.ShowTopic(Utility.Help.SelectSnapshotHelpTopic);
        }

        private bool LoadSnapshots()
        {
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

                    _listView_Snapshots.Items.Add("", snap.IconIndex);
                    _listView_Snapshots.Items[_listView_Snapshots.Items.Count - 1].SubItems.Add(snap.SnapshotName);
                    _listView_Snapshots.Items[_listView_Snapshots.Items.Count - 1].SubItems.Add(status);
                    _listView_Snapshots.Items[_listView_Snapshots.Items.Count - 1].SubItems.Add(snap.Baseline);
                    _listView_Snapshots.Items[_listView_Snapshots.Items.Count - 1].Tag = snap.SnapshotId;
                }
            }

            return true;
        }

        public static int GetSnapshotId(Sql.RegisteredServer serverName)
        {
            // Create the form.
            Form_SelectSnapshot form = new Form_SelectSnapshot(serverName);

            // If the snapshots are loaded, display it to the user for selection.
            int id = 0;
            if (form.LoadSnapshots())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    id = form.SelectedSnapshotId;
                }
            }

            return id;
        }

        #endregion

        #region Events

        private void _button_Properties_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            int snapshotId = (int)_listView_Snapshots.SelectedItems[0].Tag;

            Forms.Form_SnapshotProperties.Process(new Sql.ObjectTag(snapshotId, Sql.ObjectType.TypeEnum.Snapshot));

            Cursor = Cursors.Default;
        }

        private void _button_OK_Click(object sender, EventArgs e)
        {
            m_SelectedSnapshotId = (int)_listView_Snapshots.SelectedItems[0].Tag;
        }

        private void _listView_Snapshots_DoubleClick(object sender, EventArgs e)
        {
            _button_OK_Click(sender, e);
            DialogResult = DialogResult.OK;
        }

        private void _listView_Snapshots_SelectedIndexChanged(object sender, EventArgs e)
        {
            _button_Properties.Enabled = _button_OK.Enabled = _listView_Snapshots.SelectedItems.Count != 0;
        }

        private void Form_SelectSnapshot_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showHelpTopic();

            Cursor = Cursors.Default;
        }

        #endregion

        private void button_Help_Click(object sender, EventArgs e)
        {
           // showHelpTopic();
        }

        private void Form_SelectSnapshot_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
           // showHelpTopic();
        }
    }
}

