using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolTip;

using Idera.SQLsecure.Core.Logger;

namespace Idera.SQLsecure.UI.Console.Controls
{
    public partial class SystemStatus : UserControl
    {
        #region Fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Controls.SystemStatus");

        private bool m_IsConnected = true;
        private string m_DbSize = string.Empty;
        private bool m_IsAgentOk = true;
        private bool m_IsLicenseOk = true;
        private bool m_IsLicenseNotExpiring = true;
        private bool m_AllServersOk = true;
        private int m_ServersWarn = 0;
        private int m_ServersError = 0;
        private DateTime? m_LastGroomed = null;

        private List<Sql.RegisteredServer> m_Servers = null;

        #endregion

        #region Ctors

        public SystemStatus()
        {
            InitializeComponent();
        }

        #endregion

        #region Queries, Columns and Constants

        private const string colIcon = "Icon";
        private const string colServer = "Server";
        private const string colVersion = "Version";
        private const string colCollectionTime = "CollectionTime";
        private const string colCollectionStatus = "CollectionStatus";

        // Constants
        private const string SystemNormal = "All features are functioning normally";
        private const string SystemNotConnected = "Not connected";

        private const string ServerFmt = "{0}\r\n{1}";
        private const string LicenseFmt = "{0} servers";

        private const string StatusAttend_Agent = @"SQL Server Agent needs attention";
        private const string StatusAttend_License = @"License needs attention";
        private const string StatusAttend_Servers = @"Audited Servers need attention";

        private const string ToolTip_AgentTitle = @"Repository SQL Server Agent Status";
        private const string ToolTip_AgentOK = @"The SQL Server Agent is running on the Repository and Data Collection should function normally.";
        private const string ToolTip_AgentUnknown = @"SQLsecure was unable to obtain the status of the SQL Server Agent because you do not have authority to view this information on the Repository server.";
        private const string ToolTip_AgentBad = "The SQL Server Agent is not running on the Repository SQL Server. Data Collection is not currently available.\nSQLsecure uses the SQL Server Agent on the Repository to collect data from the registered servers. Start the Agent to enable data collection.";
        private const string ToolTip_ServersTitle = @"Audited SQL Servers Status";
        private const string ToolTip_ServersOK = @"All audited SQL Servers have collected valid audit data on the last snapshot taken";
        private static string ToolTip_ServersWarn = "All audited SQL Servers have collected valid audit data. However, {0} servers have warnings associated with the data. Check the server status and configure the affected servers to enable gathering all data if necessary.";
        private static string ToolTip_ServersBad = "SQLsecure was unable to collect data for {0} of the audited servers during the last attempt.  Check the server status and configure the affected servers to enable auditing data.";
        private static string ToolTip_ServersWarnBad = "SQLsecure was unable to collect data for {0} of the audited servers during the last attempt. Additionally, {1} servers have warnings associated with the data.  Check the server status and configure the affected servers to enable auditing all data.";
        private const string ToolTip_ServersAllBad = @"SQLsecure was unable to collect data for any of the registered servers.  There is no audit data to review. Configure your audited servers to enable auditing data.";

        private const string Status_LicenseOK = @"OK";
        private const string Status_LicenseExpiring = @"Expiring";
        private const string Status_LicenseBad = @"Not valid";

        private const string Status_ServersOK = @"OK";
        private static string Status_ServersBad = "{0} errors";

        #endregion

        #region properties

        public GradientPanel.GradientCornerStyle HeaderGradientCornerStyle
        {
            get { return _viewSection_Status.HeaderGradientCornerStyle; }
            set { _viewSection_Status.HeaderGradientCornerStyle = value; }
        }

        #endregion

        #region Helpers

        private void loadData()
        {
            // Reset the fields.
            m_IsConnected =
                m_IsAgentOk =
                m_IsLicenseOk =
                m_IsLicenseNotExpiring =
                m_AllServersOk = false;
            m_ServersWarn =
                m_ServersError = 0;
            m_Servers = null;

            // Update the status fields.
            m_IsConnected = Program.gController.Repository.IsValid;
            if (m_IsConnected)
            {
                // Get the connection string.
                string connectionString = Program.gController.Repository.ConnectionString;

                m_DbSize = Program.gController.Repository.DbSize;

                // Check if SQL Server agent is running.
                // Note this uses an extended stored procedure that will not run as a Viewer
                // so only check if Admin
                if (Program.gController.isAdmin)
                {
                    m_IsAgentOk = Sql.ScheduleJob.IsSQLAgentStarted(connectionString);
                }
                else
                {
                    // set the value to true so no warnings will be displayed
                    m_IsAgentOk = true;
                }

                // Check if license is valid.
                m_IsLicenseOk = Program.gController.Repository.IsLicenseOk();
                if (m_IsLicenseOk)
                {
                    m_IsLicenseNotExpiring = !Program.gController.Repository.bbsProductLicense.CombinedLicense.isAboutToExpire;
                }

                // Get a current list of registered servers.
                m_Servers = Sql.RegisteredServer.LoadRegisteredServers(Program.gController.Repository.ConnectionString);

                // Process each server to check its status and also get additional counts.
                m_AllServersOk = true;
                foreach (Sql.RegisteredServer srvr in m_Servers)
                {
                    // Check server status.
                    m_AllServersOk = m_AllServersOk && string.Compare(srvr.CurrentCollectionStatus, 
                                                            Utility.Snapshot.StatusErrorText, true) != 0;
                    m_ServersError += string.Compare(srvr.CurrentCollectionStatus,
                                                Utility.Snapshot.StatusErrorText, true) == 0 ? 1 : 0;
                    m_ServersWarn += string.Compare(srvr.CurrentCollectionStatus,
                                                Utility.Snapshot.StatusWarningText, true) == 0 ? 1 : 0;

                    //// Get counts from last successful collection.
                    //if (srvr.LastCollectionSnapshotId != 0)
                    //{
                    //    Sql.Snapshot snapshot = Sql.Snapshot.GetSnapShot(srvr.LastCollectionSnapshotId);
                    //    if (snapshot != null)
                    //    {
                    //        List<Sql.Database> dbs = Sql.Database.GetSnapshotDatabases(snapshot.SnapshotId);
                    //        m_NumDatabases += dbs.Count;
                    //        m_NumObjects += snapshot.NumObject;
                    //        m_NumPermissions += snapshot.NumPermission;
                    //    }
                    //}
                }

                //Fill grooming job last run date
                Sql.ScheduleJob.GetGroomingJobLastRun(Program.gController.Repository.ConnectionString, out m_LastGroomed);
            }
        }

        private void updateSystemStatus()
        {
            string status = string.Empty;
            UltraToolTipInfo tooltip;
            // Set the status.
            if (m_IsConnected)
            {
                // get SQL Server Agent Status
                if (m_IsAgentOk)
                {
                    if (Program.gController.isAdmin)
                    {
                        _pictureBox_Agent.Image = AppIcons.AppImage48(AppIcons.EnumImageList48.AgentStarted);
                        _linkLabel_AgentStatus.Text = Utility.ErrorMsgs.SQLServerAgentStarted;
                        tooltip = new UltraToolTipInfo(ToolTip_AgentOK,
                                                         ToolTipImage.Info,
                                                        ToolTip_AgentTitle,
                                                        Infragistics.Win.DefaultableBoolean.False);
                    }
                    else
                    {
                        _pictureBox_Agent.Image = AppIcons.AppImage16(AppIcons.Enum.Unknown);
                        _linkLabel_AgentStatus.Text = Utility.ErrorMsgs.SQLServerAgentUnknown;
                        tooltip = new UltraToolTipInfo(ToolTip_AgentUnknown,
                                                         ToolTipImage.Warning,
                                                        ToolTip_AgentTitle,
                                                        Infragistics.Win.DefaultableBoolean.False);
                    }
                }
                else
                {
                    _pictureBox_Agent.Image = AppIcons.AppImage48(AppIcons.EnumImageList48.AgentStopped);
                    _linkLabel_AgentStatus.Text = Utility.ErrorMsgs.SQLServerAgentStopped;
                    tooltip = new UltraToolTipInfo(ToolTip_AgentBad,
                                                     ToolTipImage.Error,
                                                    ToolTip_AgentTitle,
                                                    Infragistics.Win.DefaultableBoolean.False);
                    status += (status.Length == 0 ? string.Empty : "\n") + StatusAttend_Agent;
                }
                _linkLabel_AgentStatus.LinkArea = new LinkArea(0, _linkLabel_AgentStatus.Text.Length);
                _ultraToolTipManager.SetUltraToolTip(_linkLabel_AgentStatus, tooltip);

                // get License Status
                if (m_IsLicenseOk)
                {
                    if (m_IsLicenseNotExpiring)
                    {
                        _pictureBox_License.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.check_32;
                        _linkLabel_LicenseStatus.Text = Status_LicenseOK;
                    }
                    else
                    {
                        _linkLabel_LicenseStatus.Text = Status_LicenseExpiring;
                        _pictureBox_License.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.warning_32;
                        status += (status.Length == 0 ? string.Empty : "\n") + StatusAttend_License;
                    }
                }
                else
                {
                    _linkLabel_LicenseStatus.Text = Status_LicenseBad;
                    _pictureBox_License.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.error_32;
                    status += (status.Length == 0 ? string.Empty : "\n") + StatusAttend_License;
                }
                if (Program.gController.Permissions.hasSecurity(Utility.Security.Functions.ManageLicense))
                {
                    _linkLabel_LicenseStatus.LinkArea = new LinkArea(0, _linkLabel_LicenseStatus.Text.Length);
                }
                else
                {
                    _linkLabel_LicenseStatus.LinkArea = new LinkArea(0, 0);
                }

                // get Audited Servers Status
                if (m_AllServersOk)
                {
                    _pictureBox_Servers.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources._32_SystemOK;
                    _linkLabel_ServersStatus.Text = Status_ServersOK;
                    tooltip = new UltraToolTipInfo(m_ServersWarn == 0 ? ToolTip_ServersOK : String.Format(ToolTip_ServersWarn, m_ServersWarn.ToString()),
                                                     ToolTipImage.Info,
                                                    ToolTip_ServersTitle,
                                                    Infragistics.Win.DefaultableBoolean.False);
                }
                else
                {
                    if (m_ServersError == m_Servers.Count)
                    {
                        _pictureBox_Servers.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources._32_SystemWarn;
                        _linkLabel_ServersStatus.Text = String.Format(Status_ServersBad, m_ServersError.ToString());
                        tooltip = new UltraToolTipInfo(ToolTip_ServersAllBad,
                                                         ToolTipImage.Error,
                                                        ToolTip_ServersTitle,
                                                        Infragistics.Win.DefaultableBoolean.False);
                    }
                    else
                    {
                        _pictureBox_Servers.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources._32_SystemWarn;
                        _linkLabel_ServersStatus.Text = String.Format(Status_ServersBad, m_ServersError.ToString());
                        tooltip = new UltraToolTipInfo(m_ServersWarn == 0 ? String.Format(ToolTip_ServersBad, m_ServersWarn.ToString())
                                                                          : String.Format(ToolTip_ServersWarnBad, m_ServersError.ToString(), m_ServersWarn.ToString()),
                                                             ToolTipImage.Error,
                                                        ToolTip_ServersTitle,
                                                        Infragistics.Win.DefaultableBoolean.False);
                    }
                    status += (status.Length == 0 ? string.Empty : "\n") + StatusAttend_Servers;
                }
                _linkLabel_ServersStatus.LinkArea = new LinkArea(0, _linkLabel_ServersStatus.Text.Length);
                _ultraToolTipManager.SetUltraToolTip(_linkLabel_ServersStatus, tooltip);


                // Overall repository status
                if (m_IsAgentOk && m_IsLicenseOk && m_IsLicenseNotExpiring && m_AllServersOk)
                {
                    _pictureBox_ServerStatus.Image = AppIcons.AppImage48(AppIcons.EnumImageList48.StatusGood);
                    _linkLabel_ServerStatus.Text = SystemNormal;
                }
                else
                {
                    if (m_ServersError == m_Servers.Count)
                    {
                        _pictureBox_ServerStatus.Image = AppIcons.AppImage48(AppIcons.EnumImageList48.StatusError);
                        _linkLabel_ServerStatus.Text = status;
                    }
                    else
                    {
                        _pictureBox_ServerStatus.Image = AppIcons.AppImage48(AppIcons.EnumImageList48.StatusWarning);
                        _linkLabel_ServerStatus.Text = status;
                    }
                }

                // Repository Info
                _lbl_Repository.Text = String.Format(ServerFmt, Program.gController.Repository.Instance, Program.gController.Repository.SQLServerVersionFriendlyLong);
                _lbl_License.Text = String.Format(LicenseFmt, Program.gController.Repository.GetStrLicensedServers());
                _lbl_Size.Text = m_DbSize;
                _lbl_LastGroomed.Text = m_LastGroomed == null ? "Never" : m_LastGroomed.Value.ToString(Utility.Constants.DATETIME_FORMAT);
            }
            else
            {
                //_pictureBox_ServerStatus.Image = AppIcons.AppImage48(AppIcons.EnumImageList48.StatusWarning); 
                //_linkLabel_ServerStatus.Text = SystemNotConnected;

                _lbl_Repository.Text = SystemNotConnected;
                _lbl_License.Text =
                    _lbl_Size.Text =
                    _lbl_LastGroomed.Text = String.Empty;
            }
        }

        #endregion

        #region Methods

        public void UpdateStatus()
        {
            // Collect data.
            loadData();

            updateSystemStatus();
        }

        #endregion

        #region Event Handlers

        #region Repository Status Links

        private void _lbl_Status_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Forms.Form_StatusDetails.Process(m_IsConnected, m_IsAgentOk, m_IsLicenseOk, m_IsLicenseNotExpiring, m_AllServersOk);
        }

        private void _linkLabel_AgentStatus_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _ultraToolTipManager.ShowToolTip(_linkLabel_AgentStatus);
        }

        private void _linkLabel_LicenseStatus_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Program.gController.Repository.ResetLicense();
            Forms.Form_License dlg = new Forms.Form_License(Program.gController.Repository.bbsProductLicense);
            dlg.ShowDialog();
            Program.gController.SetTitle();
            Program.gController.RefreshCurrentView();
        }

        private void _linkLabel_ServersStatus_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _ultraToolTipManager.ShowToolTip(_linkLabel_ServersStatus);
        }

        #endregion

        #endregion
    }
}
