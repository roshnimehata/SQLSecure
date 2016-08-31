using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Sql;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_SelectServer : Idera.SQLsecure.UI.Console.Controls.BaseDialog
    {
        #region Ctors

        public Form_SelectServer(bool alowMultiSelect = false)
        {
            InitializeComponent();

            m_SelectedServer = "";
            _listView_Servers.MultiSelect = alowMultiSelect;
            SelectedServers = new List<string>();
        }

        #endregion

        #region Fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Forms.Form_SelectServer");
        string m_SelectedServer = "";

        #endregion

        #region Properties

        public string SelectedServer
        {
            get { return m_SelectedServer; }
        }

        public List<string> SelectedServers { get; set; }
        #endregion

        #region Helpers

        public bool LoadServers()
        {
            return LoadServers(false);
        }

        public bool LoadServers(bool registeredOnly)
        {
            // Get the data source enumerator object.
            bool isOk = true;
            System.Data.Sql.SqlDataSourceEnumerator serversEnum = System.Data.Sql.SqlDataSourceEnumerator.Instance;

            // Get the data sources and fill the list view.
            try
            {
                if (!registeredOnly)
                {
                    // Get data sources.
                    using (DataTable servers = serversEnum.GetDataSources())
                    {
                        // Process each row and populate the list view.
                        foreach (DataRow row in servers.Rows)
                        {
                            // Check that the server is not null.
                            if (!row.IsNull("ServerName"))
                            {
                                // Construct the instance name.
                                string serverInstance = (string)row["ServerName"];
                                if (!row.IsNull("InstanceName"))
                                {
                                    serverInstance = serverInstance + @"\" + (string)row["InstanceName"];
                                }

                                // Update the list view.
                                _listView_Servers.Items.Add(serverInstance);
                            }
                        }
                    }
                }
                else
                {
                    var servers = RegisteredServer.LoadRegisteredServers(Program.gController.Repository.ConnectionString);
                    foreach (RegisteredServer item in servers)
                    {
                        _listView_Servers.Items.Add(item.FullName);
                    }
                }

            }
            catch (Exception ex)
            {
                logX.loggerX.Error("ERROR - exception raised when enumerating servers, ", ex);
                isOk = false;
            }

            return isOk;
        }

        #endregion

        #region Events

        private void Form_SelectServer_Load(object sender, EventArgs e)
        {
            if (_listView_Servers.Items.Count > 0)
            {
                _listView_Servers.Items[0].Selected = true;
            }
            _listView_Servers.Select();
        }

        private void _button_OK_Click(object sender, EventArgs e)
        {
            m_SelectedServer = _listView_Servers.SelectedItems[0].Text;
            foreach (ListViewItem item in _listView_Servers.SelectedItems)
            {
                SelectedServers.Add(item.Text);
            }
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

        #endregion
    }
}

