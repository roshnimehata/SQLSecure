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
    public partial class SelectServer : UserControl
    {

        #region CTOR

        public SelectServer()
        {
            InitializeComponent();

        }

        #endregion

        #region Fields

        public delegate void SelectedUserChanged(bool UserSelected);
        SelectedUserChanged m_SelectedUserChangedDelegate = null;

        private Sql.RegisteredServer m_SelectedServer;

        #endregion

        #region Properties

        public Sql.RegisteredServer SelectedServer
        {
            get { return m_SelectedServer; }
        }

        public void RegisterUserChangeDelegate(SelectedUserChanged value)
        {
            m_SelectedUserChangedDelegate += value; 
        }

        #endregion

        #region Constants

        private const string HeadingAllServers = @"All Audited Servers";
        private const string HeadingValidServers = @"All Audited Servers with Audit Data";

        #endregion

        #region Helpers

        private void ShowHelpTopic()
        {
            Program.gController.ShowTopic(Utility.Help.SelectServerHelpTopic);
        }

        public void LoadServers(bool showValidOnly)
        {
            // set the heading based on the selection
            if (showValidOnly)
            {
                ultraListViewSelectServer.MainColumn.Text = HeadingValidServers;
            }
            else
            {
                ultraListViewSelectServer.MainColumn.Text = HeadingAllServers;
            }

            // Get the server list from the currently loaded array without doing a refresh

            // Fill the list view.
            ultraListViewSelectServer.Items.Clear();
            foreach (Sql.RegisteredServer server in Program.gController.Repository.RegisteredServers)
            {
                if (!showValidOnly || server.LastCollectionSnapshotId > 0)
                {
                    UltraListViewItem li = ultraListViewSelectServer.Items.Add(null, server.ConnectionName);
                    li.Tag = server;
                }
            }

            if (ultraListViewSelectServer.Items.Count > 0)
            {
                ultraListViewSelectServer.SelectedItems.Clear();
                ultraListViewSelectServer.SelectedItems.Add(ultraListViewSelectServer.Items[0]);
            }
        }

        #endregion

        #region Events
       
        private void ultraListViewSelectServer_ItemSelectionChanged(object sender, Infragistics.Win.UltraWinListView.ItemSelectionChangedEventArgs e)
        {
            bool isUserSelected = false;
            if(ultraListViewSelectServer.SelectedItems.Count > 0)
            {
                m_SelectedServer = (Sql.RegisteredServer) ultraListViewSelectServer.SelectedItems[0].Tag;
                isUserSelected = true;
            }
            if(m_SelectedUserChangedDelegate != null)
            {
                m_SelectedUserChangedDelegate(isUserSelected);
            }
        }


        #endregion

    }
}
