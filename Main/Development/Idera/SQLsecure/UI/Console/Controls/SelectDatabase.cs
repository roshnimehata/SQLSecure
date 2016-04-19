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
    public partial class SelectDatabase : UserControl
    {
        public SelectDatabase()
        {
            InitializeComponent();

            radioButton_ServerOnly.Checked = true;

            ultraListViewDatabases.Enabled = false;
        }

        #region Fields

        public delegate void SelectedDatabaseChanged(bool DBorServerOnlySelected);
        SelectedDatabaseChanged m_SelectedDatabaseChangedDelegate = null;

        private int m_SnapshotID;
        private string m_SelectedDatabase;

        #endregion


        #region Properties

        public void RegisterDatabaseChangeDelegate(SelectedDatabaseChanged value)
        {
            m_SelectedDatabaseChangedDelegate += value;
        }


        public string SelectedDatabase
        {
            get { return radioButton_ServerOnly.Checked ? string.Empty : m_SelectedDatabase; }
        }

        #endregion

        #region Queries

        private const string QueryGetDatabases = @"SELECT databasename FROM SQLsecure.dbo.vwdatabases WHERE snapshotid = {0} ORDER BY databasename";

        #endregion

        #region Helpers

        public void LoadDatabases(int snapshotID)
        {
            m_SnapshotID = snapshotID;
            // Open connection to repository and query permissions.
//            DiagLog.LogInfo("Retrieve Snapshot Databases");
            ultraListViewDatabases.Items.Clear();
            List<Sql.Database> dblist = Sql.Database.GetSnapshotDatabases(m_SnapshotID);
            foreach (Sql.Database db in dblist)
            {
                // Add database to the list if its available.
                if (db.IsAvailable)
                {
                    UltraListViewItem li = ultraListViewDatabases.Items.Add(null, db.Name);
                    li.Tag = db.Name;
                }
            }

            if (ultraListViewDatabases.Items.Count > 0)
            {
                ultraListViewDatabases.SelectedItems.Clear();
                ultraListViewDatabases.SelectedItems.Add(ultraListViewDatabases.Items[0]);
            }

        }


        #endregion    

        #region Events

        private void SetSelectedDB()
        {
            bool bSelected = radioButton_ServerOnly.Checked;

            if(!bSelected && ultraListViewDatabases.SelectedItems.Count > 0)
            {
                m_SelectedDatabase = (string)ultraListViewDatabases.SelectedItems[0].Tag;
                bSelected = true;
            }

            // Notify all listeners
            if (m_SelectedDatabaseChangedDelegate != null)
            {
                m_SelectedDatabaseChangedDelegate(bSelected);
            }

        }


        private void ultraListViewDatabases_ItemSelectionChanged(object sender, Infragistics.Win.UltraWinListView.ItemSelectionChangedEventArgs e)
        {
            SetSelectedDB();
        }

        private void radioButton_ServerOnly_CheckedChanged(object sender, EventArgs e)
        {
            ultraListViewDatabases.Enabled = false;
            SetSelectedDB();
        }

        private void radioButton_ServerAndDatabase_CheckedChanged(object sender, EventArgs e)
        {
            ultraListViewDatabases.Enabled = true;
            ultraListViewDatabases.SelectedItems.Clear();

            SetSelectedDB();
        }

        #endregion

       

    }

}
