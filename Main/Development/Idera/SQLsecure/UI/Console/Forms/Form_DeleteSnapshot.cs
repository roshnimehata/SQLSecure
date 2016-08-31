using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_DeleteSnapshot : Idera.SQLsecure.UI.Console.Controls.BaseDialog
    {
        public Form_DeleteSnapshot(Sql.Snapshot.SnapshotList deleteList)
        {
            InitializeComponent();

            m_deleteList = deleteList;

            foreach (Sql.Snapshot snap in m_deleteList)
            {
                _listView_Servers.Items.Add(snap.StartTime.ToLocalTime().ToString(Utility.Constants.DATETIME_FORMAT));
                _listView_Servers.Items[_listView_Servers.Items.Count - 1].SubItems.Add(snap.Baseline);
            }
        }

        private Sql.Snapshot.SnapshotList m_deleteList;

        private void showHelpTopic()
        {
            Program.gController.ShowTopic(Utility.Help.DeleteSnapshotHelpTopic);
        }

        #region Events

        private void _button_Delete_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            _button_Delete.Enabled = false;
            _button_Cancel.Enabled = false;

            int i = 0;
            foreach (Sql.Snapshot snap in m_deleteList)
            {
                _listView_Servers.Items[i].SubItems.Add(@"In progress");
                _listView_Servers.Items[i].SubItems.Add(string.Empty);
                _listView_Servers.Refresh();
                string message;
                try
                {
                    if (snap.Delete(out message))
                    {
                        _listView_Servers.Items[i].SubItems[2].Text = @"Deleted";
                        _listView_Servers.Items[i].SubItems[3].Text = message;
                    }
                    else
                    {
                        _listView_Servers.Items[i].SubItems[2].Text = @"Failed";
                        _listView_Servers.Items[i].SubItems[3].Text = message;
                    }
                }
                catch (Exception ex)
                {
                    _listView_Servers.Items[i].SubItems[2].Text = @"Failed";
                    _listView_Servers.Items[i].SubItems[3].Text = ex.Message;
                }

                _listView_Servers.Refresh();
                i++;
            }

            //hide the Delete and Cancel buttons and show the Close button
            //move the Close button to the right location - it is moved for design time visibility
            _button_Delete.Visible = false;
            _button_Cancel.Visible = false;
            _button_OK.Location = _button_Cancel.Location;
            _button_OK.Visible = true;
            Cursor = Cursors.Default;
        }

        private void _button_Help_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showHelpTopic();

            Cursor = Cursors.Default;
        }

        private void _listView_Servers_Resize(object sender, EventArgs e)
        {
            int colWidths = 0;

            foreach (ColumnHeader col in _listView_Servers.Columns)
            {
                colWidths += col.Width;
            }

            if (_listView_Servers.Width != colWidths)
            {
                _listView_Servers.Columns[3].Width += _listView_Servers.Width - colWidths;
            }
            _listView_Servers.Columns[3].Width = Math.Max(_listView_Servers.Columns[3].Width, 100);
        }

        private void Form_DeleteSnapshot_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Cursor = Cursors.WaitCursor;

            showHelpTopic();

            Cursor = Cursors.Default;
        }

        #endregion

        #region Helpers

        public static DialogResult Process(Sql.Snapshot.SnapshotList DeleteList)
        {
            Form_DeleteSnapshot form = new Form_DeleteSnapshot(DeleteList);
            return form.ShowDialog();
        }

        #endregion
    }
}

