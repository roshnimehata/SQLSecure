using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Sql;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_SelectDatabase : Idera.SQLsecure.UI.Console.Controls.BaseDialog
    {
        #region Ctors

        public Form_SelectDatabase(int snapshotId)
        {
            InitializeComponent();

            m_Snapshot = Snapshot.GetSnapShot(snapshotId);
        }

        #endregion

        #region Fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Forms.Form_SelectDatabase");
        private Sql.Snapshot m_Snapshot;
        private string m_SelectedDatabase;

        #endregion

        #region Properties

        public string SelectedDatabase
        {
            get { return m_SelectedDatabase; }
        }

        #endregion

        #region Queries

        private const string QueryGetDatabases = @"SELECT databasename FROM SQLsecure.dbo.vwdatabases WHERE snapshotid = {0} ORDER BY databasename";

        #endregion

        #region Helpers

        private void LoadDatabases()
        {
            // Open connection to repository and query permissions.
            logX.loggerX.Info("Retrieve Snapshot Databases");

            if (m_Snapshot != null)
            {
                List<Sql.Database> dblist = Sql.Database.GetSnapshotDatabases(m_Snapshot.SnapshotId);
                foreach (Sql.Database db in dblist)
                {
                    // Add database to the list if its available.
                    if (db.IsAvailable)
                    {
                        _listView_Databases.Items.Add(db.Name);
                    }
                }
            }
        }

        public static string GetDatabaseName(int snapshotId)
        {
            Form_SelectDatabase form = new Form_SelectDatabase(snapshotId);
            form.LoadDatabases();
            DialogResult rc = form.ShowDialog();
            if (rc == DialogResult.OK)
            {
                return form.SelectedDatabase;
            }
            else
            {
                return "";
            }
        }

        #endregion

        #region Events

        private void _button_OK_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            m_SelectedDatabase = _listView_Databases.SelectedItems[0].Text;
        }

        private void _listView_Databases_DoubleClick(object sender, EventArgs e)
        {
            _button_OK_Click(sender, e);
            DialogResult = DialogResult.OK;
        }

        private void _listView_Databases_SelectedIndexChanged(object sender, EventArgs e)
        {
            _button_OK.Enabled = _listView_Databases.SelectedItems.Count != 0;
        }

        #endregion

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
            Program.gController.ShowTopic(Utility.Help.SelectDatabaseHelpTopic);
        }
        
    }
}

