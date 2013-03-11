using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_SelectRegisteredServer : Idera.SQLsecure.UI.Console.Controls.BaseDialog
    {
        #region Ctors

        public Form_SelectRegisteredServer()
        {
            InitializeComponent();
        }

        #endregion

        #region Fields

        private Sql.RegisteredServer m_SelectedServer;

        #endregion

        #region Properties

        public Sql.RegisteredServer SelectedServer
        {
            get { return m_SelectedServer; }
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

        private void LoadServers(bool showValidOnly)
        {
            // set the heading based on the selection
            if (showValidOnly)
            {
                _listView_Servers.Columns[0].Text = HeadingValidServers;
            }
            else
            {
                _listView_Servers.Columns[0].Text = HeadingAllServers;
            }

            // Get the server list from the currently loaded array without doing a refresh

            // Fill the list view.
            foreach (Sql.RegisteredServer server in Program.gController.Repository.RegisteredServers)
            {
                if (!showValidOnly || server.LastCollectionSnapshotId > 0)
                {
                    _listView_Servers.Items.Add(server.ConnectionName);
                }
            }
        }

        /// <summary>
        /// Show a selection list of all Audited SQL Servers and return the selected server
        /// </summary>
        /// <returns>a Sql.RegisteredServer or null if no selection is made</returns>
        public static Sql.RegisteredServer GetServer()
        {
            return GetServer(false);
        }

        /// <summary>
        /// Show a selection list of Audited SQL Servers and return the selected server
        /// </summary>
        /// <param name="canExplore">bool set to true to list only servers that have valid audit data</param>
        /// <returns>a Sql.RegisteredServer or null if no selection is made</returns>
        public static Sql.RegisteredServer GetServer(bool canExplore)
        {
            // Create the form.
            Form_SelectRegisteredServer form = new Form_SelectRegisteredServer();

            Sql.RegisteredServer server = null;
            form.LoadServers(canExplore);

            if (form.ShowDialog() == DialogResult.OK)
            {
                server = form.SelectedServer;
            }

            return server;
        }

        #endregion

        #region Events

        private void _button_OK_Click(object sender, EventArgs e)
        {
            m_SelectedServer = Program.gController.Repository.GetServer(_listView_Servers.SelectedItems[0].Text);
        }

        private void _listView_Servers_DoubleClick(object sender, EventArgs e)
        {
            _button_OK_Click(sender, e);
            DialogResult = DialogResult.OK;
        }

        private void _listView_Servers_SelectedIndexChanged(object sender, EventArgs e)
        {
            _button_OK.Enabled = _listView_Servers.SelectedItems.Count != 0;
        }



        private void button_Help_Click(object sender, EventArgs e)
        {
            ShowHelpTopic();
        }

        private void Form_SelectRegisteredServer_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            ShowHelpTopic();
        }

        #endregion


    }
}

