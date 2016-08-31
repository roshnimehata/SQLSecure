using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Idera.SQLsecure.UI.Console.Utility;
using Infragistics.Win.UltraWinListView;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_SnapshotServerProperties : Idera.SQLsecure.UI.Console.Controls.BaseDialogForm
    {
        #region Fields

        private Sql.ServerVersion m_Version;
        private Sql.ObjectTag m_ObjectTag;
        private Sql.Snapshot m_Snapshot;
        private bool m_IsGridFilled;

        #endregion

        #region Helpers

        private void initGeneralPage()
        {
            Debug.Assert(m_Snapshot != null);

            _lbl_ServerVal.Text = m_Snapshot.FullName;
            _lbl_SQLServerVersionVal.Text = m_Snapshot.VersionFriendlyLong;
            _lbl_SQLServerEditionVal.Text = m_Snapshot.Edition;
            _lbl_ReplicationVal.Text = m_Snapshot.ReplicationEnabled;
            _lbl_SaVal.Text = m_Snapshot.SaPasswordEmpty;

            _lbl_OsServerVal.Text = m_Snapshot.ServerName;
            _lbl_WindowsOSVal.Text = m_Snapshot.OS;
            _lbl_DcVal.Text = m_Snapshot.ServerIsDomainController;
            _lbl_SysDriveVal.Text = m_Snapshot.SystemDrive;
        }

        private void initConfigPage()
        {
            Debug.Assert(m_Snapshot != null);

            _lbl_AuthenticationModeVal.Text = m_Snapshot.AuthenticationMode;
            _lbl_LoginAuditModeVal.Text = m_Snapshot.LoginAuditMode;
            _lbl_EnableProxyAccountVal.Text = m_Snapshot.EnableProxyAccount;
            _lbl_StartupProcsVal.Text = m_Snapshot.ScanForStartupProcsEnabled;
            _lbl_EnableC2AuditTraceVal.Text = m_Snapshot.EnableC2AuditTrace;
            _lbl_CrossDBOwnershipChainingVal.Text = m_Snapshot.CrossDBOwnershipChaining;
            _lbl_CaseSensitiveVal.Text = m_Snapshot.CaseSensitiveMode;
            _lbl_SystemTableUpdatesVal.Text = m_Snapshot.AllowSystemTableUpdates;

            _lbl_AgentMailProfileVal.Text = m_Snapshot.AgentMailProfile;
            _lbl_AgentSysadminVal.Text = m_Snapshot.AgentSysadminOnly;

            _lbl_HideInstanceVal.Text = m_Snapshot.HideInstance;
            foreach(DataRow row in m_Snapshot.Protocols.Rows)
            {
                UltraListViewItem li = ultraListView_Protocols.Items.Add(null, (string) row["Name"]);
                li.SubItems["IPAddress"].Value = (string) row["Address"];
                string port;
                if (row["DynamicPort"] != DBNull.Value && (string)row["DynamicPort"] == "Y")
                {
                    port = @"Dynamic";
                }
                else
                {
                    port = (string) row["Port"];
                }
                li.SubItems["TCPPort"].Value = port;
                li.SubItems["Active"].Value = (string) row["Active"];
            }

            _lbl_RemoteConnectionsVal.Text = m_Snapshot.RemoteAccessEnabled;
            _lbl_RemoteDacVal.Text = m_Snapshot.RemoteAdminConnectionsEnabled;
            _lbl_DatabaseMailVal.Text = m_Snapshot.DatabaseMailXpsEnabled;
            _lbl_OleVal.Text = m_Snapshot.OleAutomationProceduresEnabled;
            _lbl_SqlMailVal.Text = m_Snapshot.SqlMailXpsEnabled;
            _lbl_WebAssistantVal.Text = m_Snapshot.WebassistantProceduresEnabled;
            _lbl_Xp_CmdShellVal.Text = m_Snapshot.Xp_cmdshellEnabled;
            _lbl_AdHocVal.Text = m_Snapshot.AdHocDistributedQueriesEnabled;
        }

        #endregion

        #region Ctors

        private Form_SnapshotServerProperties(
                Sql.ServerVersion version,
                Sql.ObjectTag tag
            )
        {
            Debug.Assert(version != Sql.ServerVersion.Unsupported);
            Debug.Assert(tag != null);

            InitializeComponent();

            // Set minimum size.
            this.MinimumSize = this.Size;

            // Init fields.
            m_Version = version;
            m_ObjectTag = tag;
            m_Snapshot = Sql.Snapshot.GetSnapShot(tag.SnapshotId);
            m_IsGridFilled = false;

            // Init general page.
            if (m_Snapshot != null)
            {
                // Set form title.
                Text = "Snapshot SQL Server Properties - " + m_Snapshot.FullName;

                // Init general page.
                initGeneralPage();

                // Init config page.
                initConfigPage();
            }

            ultraTabControl1.DrawFilter = new HideFocusRectangleDrawFilter();
        }

        #endregion

        #region Methods

        public static void Process(
                Sql.ServerVersion version,
                Sql.ObjectTag tag
            )
        {
            // Validate inputs.
            if (tag == null || version == Sql.ServerVersion.Unsupported) { return; }

            // Create and show the form.
            Form_SnapshotServerProperties form = new Form_SnapshotServerProperties(version,tag);
            form.ShowDialog();
        }

        #endregion

        #region Event Handlers
        private void ultraTabControl1_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            if(e.Tab.Key == "Permissions")
            {
                // If SQL Server 2000, server permissions are not supported.
                if (m_Version == Sql.ServerVersion.SQL2000)
                {
                    Label lbl_NoServerPermissions = new Label();
                    lbl_NoServerPermissions.Text = "SQL Server 2000 does not support server level permissions.";
                    lbl_NoServerPermissions.TextAlign = ContentAlignment.MiddleCenter;
                    lbl_NoServerPermissions.Dock = DockStyle.Fill;
                    ultraTabPageControl3.Controls.Clear();
                    ultraTabPageControl3.Controls.Add(lbl_NoServerPermissions);
                }
                else
                {
                    if (!m_IsGridFilled)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        _permissionsGrid.Initialize(m_Version, m_ObjectTag);
                        m_IsGridFilled = true;
                        this.Cursor = Cursors.Default;
                    }
                }
                
            }
        }
     

        #endregion

       
    }
}