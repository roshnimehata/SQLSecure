using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_BaselineSnapshot : Idera.SQLsecure.UI.Console.Controls.BaseDialog
    {
        #region CTOR
        public Form_BaselineSnapshot(Sql.Snapshot snap)
        {
            InitializeComponent();
            m_snapshot = snap;

            _listView_Snapshots.Items.Add(snap.StartTime.ToLocalTime().ToString(Utility.Constants.DATETIME_FORMAT));
            _listView_Snapshots.Items[_listView_Snapshots.Items.Count - 1].SubItems.Add(snap.Baseline);
        }

        #endregion

        #region Fields
        private Sql.Snapshot m_snapshot;
        #endregion

        #region Events

        private void _button_OK_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            m_snapshot.MakeBaseline(textBox_Comment.Text);

            Cursor = Cursors.Default;

            DialogResult = DialogResult.OK;
        }

        private void _button_Help_Click(object sender, EventArgs e)
        {
            showHelpTopic();
        }

        private void Form_BaselineSnapshot_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            showHelpTopic();
        }

        #endregion

        #region Public
        public static DialogResult Process( Sql.Snapshot snapshot)
        {
            Form_BaselineSnapshot form = new Form_BaselineSnapshot( snapshot );
            return form.ShowDialog();
        }
        #endregion

        #region Helpers

        private void showHelpTopic()
        {
            Program.gController.ShowTopic(Utility.Help.BaselineSnapshotHelpTopic);
        }

        #endregion
    }
}

