/******************************************************************
 * Name: SQLServer.cs
 *
 * Description: Encapsulates a server snapshot.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;

using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Controls;
using Idera.SQLsecure.UI.Console.Utility;

namespace Idera.SQLsecure.UI.Console.Sql
{
    /// <summary>
    /// Encapsulates a snapshot instance
    /// </summary>
    public class Snapshot
    {
        #region Types
        public class SnapshotList : List<Snapshot>
        {
        }
        #endregion

        #region Ctors

        public Snapshot()
        {
            m_ConnectionName = "";
            m_SnapshotId = 0;
        }

        public Snapshot (int snapshotId, DateTime startTime, string baseline)
        {
            m_SnapshotId = snapshotId;
            m_StartTime = startTime;
            if (string.Compare(baseline, "yes", true) == 0)
            {
                m_Baseline = "Y";
            }
            else
            {
                m_Baseline = "N";
            }
        }

        public Snapshot(SqlDataReader rdr)
        {
            m_ConnectionName = SqlHelper.GetString(rdr, (int)SnapshotListColumn.connectionname);
            m_ServerName = SqlHelper.GetString(rdr, (int)SnapshotListColumn.servername);
            m_InstanceName = SqlHelper.GetString(rdr, (int)SnapshotListColumn.instancename);
            m_AuthenticationMode = SqlHelper.GetString(rdr, (int)SnapshotListColumn.authenticationmode);
            m_OS = SqlHelper.GetString(rdr, (int)SnapshotListColumn.os);
            m_Version = SqlHelper.GetString(rdr, (int)SnapshotListColumn.version);
            m_Edition = SqlHelper.GetString(rdr, (int)SnapshotListColumn.edition);
            m_Status = SqlHelper.GetString(rdr, (int)SnapshotListColumn.status);
            m_StartTime = SqlHelper.GetDateTime(rdr, (int)SnapshotListColumn.starttime);
            m_EndTime = SqlHelper.GetDateTime(rdr, (int)SnapshotListColumn.endtime);
            m_Automated = SqlHelper.GetString(rdr, (int)SnapshotListColumn.automated);
            m_NumObject = SqlHelper.GetInt32(rdr, (int)SnapshotListColumn.numobject);
            m_NumPermission = SqlHelper.GetInt32(rdr, (int)SnapshotListColumn.numpermission);
            m_NumLogin = SqlHelper.GetInt32(rdr, (int)SnapshotListColumn.numlogin);
            m_NumWindowsGroupMember = SqlHelper.GetInt32(rdr, (int)SnapshotListColumn.numwindowsgroupmember);
            m_Baseline = SqlHelper.GetString(rdr, (int)SnapshotListColumn.baseline);
            m_BaselineComment = SqlHelper.GetString(rdr, (int)SnapshotListColumn.baselinecomment);
            m_SnapshotComment = SqlHelper.GetString(rdr, (int)SnapshotListColumn.snapshotcomment);
            m_LoginAuditMode = SqlHelper.GetString(rdr, (int)SnapshotListColumn.loginauditmode);
            m_EnableProxyAccount = SqlHelper.GetString(rdr, (int)SnapshotListColumn.enableproxyaccount);
            m_EnableC2AuditTrace = SqlHelper.GetString(rdr, (int)SnapshotListColumn.enablec2audittrace);
            m_CrossDBOwnershipChaining = SqlHelper.GetString(rdr, (int)SnapshotListColumn.crossdbownershipchaining);
            m_CaseSensitiveMode = SqlHelper.GetString(rdr, (int)SnapshotListColumn.casesensitivemode);
            m_Hashkey = SqlHelper.GetString(rdr, (int)SnapshotListColumn.hashkey);
            m_SnapshotId = SqlHelper.GetInt32(rdr, (int)SnapshotListColumn.snapshotid);
            m_RegisteredServerId = SqlHelper.GetInt32(rdr, (int)SnapshotListColumn.registeredserverid);
            m_CollectorVersion = SqlHelper.GetString(rdr, (int)SnapshotListColumn.collectorversion);
            m_AllowSystemTableUpdates = SqlHelper.GetString(rdr, (int)SnapshotListColumn.allowsystemtableupdates);
            m_RemoteAdminConnectionsEnabled = SqlHelper.GetString(rdr, (int)SnapshotListColumn.remoteadminconnectionsenabled);
            m_RemoteAccessEnabled = SqlHelper.GetString(rdr, (int)SnapshotListColumn.remoteaccessenabled);
            m_ScanForStartupProcsEnabled = SqlHelper.GetString(rdr, (int)SnapshotListColumn.scanforstartupprocsenabled);
            m_SqlMailXpsEnabled = SqlHelper.GetString(rdr, (int)SnapshotListColumn.sqlmailxpsenabled);
            m_DatabaseMailXpsEnabled = SqlHelper.GetString(rdr, (int)SnapshotListColumn.databasemailxpsenabled);
            m_OleAutomationProceduresEnabled = SqlHelper.GetString(rdr, (int)SnapshotListColumn.oleautomationproceduresenabled);
            m_WebassistantProceduresEnabled = SqlHelper.GetString(rdr, (int)SnapshotListColumn.webassistantproceduresenabled);
            m_Xp_cmdshellEnabled = SqlHelper.GetString(rdr, (int)SnapshotListColumn.xp_cmdshellenabled);
            m_AgentMailProfile = SqlHelper.GetString(rdr, (int)SnapshotListColumn.agentmailprofile);
            m_HideInstance = SqlHelper.GetString(rdr, (int)SnapshotListColumn.hideinstance);
            m_AgentSysadminOnly = SqlHelper.GetString(rdr, (int)SnapshotListColumn.agentsysadminonly);
            m_ServerIsDomainController = SqlHelper.GetString(rdr, (int)SnapshotListColumn.serverisdomaincontroller);
            m_ReplicationEnabled = SqlHelper.GetString(rdr, (int)SnapshotListColumn.replicationenabled);
            m_SaPasswordEmpty = SqlHelper.GetString(rdr, (int)SnapshotListColumn.sapasswordempty);
            m_SystemDrive = SqlHelper.GetString(rdr, (int)SnapshotListColumn.systemdrive);
            m_AdHocDistributedQueriesEnabled = SqlHelper.GetString(rdr, (int)SnapshotListColumn.adhocdistributedqueriesenabled);
            m_WeakPasswordDetectionEnabled = SqlHelper.GetString(rdr, (int)SnapshotListColumn.weakpassworddectectionenabled);
        }

        #endregion

        #region Queries & Constants

        private const string QueryGetSnapshotBase =
                    @"SELECT 
                            connectionname,
                            servername,
                            instancename,
                            authenticationmode,
                            os,
                            version,
                            edition,
                            status,
                            starttime,
                            endtime,
                            automated,
                            numobject,
                            numpermission,
                            numlogin,
                            numwindowsgroupmember,
                            baseline,
                            baselinecomment,
                            snapshotcomment,
                            loginauditmode,
                            enableproxyaccount,
                            enablec2audittrace,
                            crossdbownershipchaining,
                            casesensitivemode,
                            hashkey,
                            snapshotid,
	                        registeredserverid,
	                        collectorversion,
	                        allowsystemtableupdates,
	                        remoteadminconnectionsenabled,
	                        remoteaccessenabled,
	                        scanforstartupprocsenabled,
	                        sqlmailxpsenabled,
	                        databasemailxpsenabled,
	                        oleautomationproceduresenabled,
	                        webassistantproceduresenabled,
	                        xp_cmdshellenabled,
	                        agentmailprofile,
	                        hideinstance,
	                        agentsysadminonly,
	                        serverisdomaincontroller,
	                        replicationenabled,
	                        sapasswordempty,
	                        systemdrive,
	                        adhocdistributedqueriesenabled,
                            weakpassworddectectionenabled
                        FROM SQLsecure.dbo.vwserversnapshot";
        private const string QueryGetSnapshot = QueryGetSnapshotBase + 
                    @" WHERE snapshotid = {0}";
        private const string QueryGetSnapshotList = QueryGetSnapshotBase + 
                    @" WHERE connectionname = '{0}' ORDER BY starttime desc";
        private const string QueryGetSnapshotCount =
                    @"SELECT count(snapshotid) FROM SQLsecure.dbo.vwserversnapshot WHERE connectionname = '{0}'";
        private const string QueryGetWellKnownGroupList =
                    @"SELECT windowsgroupname FROM SQLsecure.dbo.vwancillarywindowsgroup WHERE snapshotid = {0} ORDER BY windowsgroupname";
        private const string QueryGetProtocols =
                    @"SELECT * FROM SQLsecure.dbo.vwserverprotocol WHERE snapshotid = {0}";
        private const string QueryDeleteShapshot = @"SQLsecure.dbo.isp_sqlsecure_removesnapshot";
        private const string ParamDeleteShapshotSnapshotID = "@snapshotid";

        private const string QueryBaselineShapshot = @"SQLsecure.dbo.isp_sqlsecure_marksnapshotbaseline";
        private const string ParamBaselineSnapshotID = "@snapshotid";
        private const string ParamBaselineComment = "@baselinecomment";

        private const string MissingUsers = @"unable to load all windows accounts";

        // Columns for handling the Snapshot query results
        private enum SnapshotListColumn
        {
            connectionname = 0,
            servername,
            instancename,
            authenticationmode,
            os,
            version,
            edition,
            status,
            starttime,
            endtime,
            automated,
            numobject,
            numpermission,
            numlogin,
            numwindowsgroupmember,
            baseline,
            baselinecomment,
            snapshotcomment,
            loginauditmode,
            enableproxyaccount,
            enablec2audittrace,
            crossdbownershipchaining,
            casesensitivemode,
            hashkey,
            snapshotid,
	        registeredserverid,
	        collectorversion,
	        allowsystemtableupdates,
	        remoteadminconnectionsenabled,
	        remoteaccessenabled,
	        scanforstartupprocsenabled,
	        sqlmailxpsenabled,
	        databasemailxpsenabled,
	        oleautomationproceduresenabled,
	        webassistantproceduresenabled,
	        xp_cmdshellenabled,
	        agentmailprofile,
	        hideinstance,
	        agentsysadminonly,
	        serverisdomaincontroller,
	        replicationenabled,
	        sapasswordempty,
            systemdrive,
            adhocdistributedqueriesenabled,
            weakpassworddectectionenabled
        }

        #endregion

        #region Fields
        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Sql.Snapshot");
        private String m_ConnectionName;
        private String m_ServerName;
        private String m_InstanceName;
        private String m_AuthenticationMode;
        private String m_OS;
        private String m_Version;
        private String m_Edition;
        private String m_Status;
        private DateTime m_StartTime;
        private DateTime m_EndTime;
        private String m_Automated;
        private int m_NumObject;
        private int m_NumPermission;
        private int m_NumLogin;
        private int m_NumWindowsGroupMember;
        private String m_Baseline;
        private String m_BaselineComment;
        private String m_SnapshotComment;
        private String m_LoginAuditMode;
        private String m_EnableProxyAccount;
        private String m_EnableC2AuditTrace;
        private String m_CrossDBOwnershipChaining;
        private String m_CaseSensitiveMode;
        private String m_Hashkey;
        private int m_SnapshotId;
        private int m_RegisteredServerId;
        private String m_CollectorVersion;
        private String m_AllowSystemTableUpdates;
        private String m_RemoteAdminConnectionsEnabled;
        private String m_RemoteAccessEnabled;
        private String m_ScanForStartupProcsEnabled;
        private String m_SqlMailXpsEnabled;
        private String m_DatabaseMailXpsEnabled;
        private String m_OleAutomationProceduresEnabled;
        private String m_WebassistantProceduresEnabled;
        private String m_Xp_cmdshellEnabled;
        private String m_AgentMailProfile;
        private String m_HideInstance;
        private String m_AgentSysadminOnly;
        private String m_ServerIsDomainController;
        private String m_ReplicationEnabled;
        private String m_SaPasswordEmpty;
        private String m_SystemDrive;
        private String m_AdHocDistributedQueriesEnabled;
        private String m_WeakPasswordDetectionEnabled;
        private DataTable m_Protocols;
        #endregion

        #region Properties
        public String ConnectionName {get{return m_ConnectionName;}}
        public String ServerName { get { return m_ServerName; } }
        public String InstanceName { get { return m_InstanceName; } }
        public String FullName { get { return Sql.RegisteredServer.FullNameStr(m_ServerName, m_InstanceName); } }
        public String AuthenticationMode { get { return Sql.RegisteredServer.AuthenticationModeStr(m_AuthenticationMode); } }
        public String OS { get { return m_OS; } }
        public String Version { get { return m_Version; } }
        public String Edition { get { return m_Edition; } }
        public String Status { get { return m_Status; } }
        public String StatusText { get { return GetStatusStr(m_Status); } }
        public DateTime StartTime { get { return m_StartTime; } }
        public DateTime EndTime { get { return m_EndTime; } }
        public String Automated { get { return m_Automated; } }
        public int NumObject { get { return m_NumObject; } }
        public int NumPermission { get { return m_NumPermission; } }
        public int NumLogin { get { return m_NumLogin; } }
        public int NumWindowsGroupMember { get { return m_NumWindowsGroupMember; } }
        public String Baseline { get { return string.IsNullOrEmpty(m_Baseline) ? "No" : Sql.RegisteredServer.YesNoStr(m_Baseline); } }
        public String BaselineComment { get { return m_BaselineComment; } }
        public String SnapshotComment { get { return m_SnapshotComment; } }
        public String LoginAuditMode { get { return Sql.RegisteredServer.LoginAuditModeStr (m_LoginAuditMode); } }
        public String EnableProxyAccount { get { return Sql.RegisteredServer.YesNoStr(m_EnableProxyAccount); } }
        public String EnableC2AuditTrace { get { return Sql.RegisteredServer.YesNoStr(m_EnableC2AuditTrace); } }
        public String CrossDBOwnershipChaining { get { return Sql.RegisteredServer.YesNoStr(m_CrossDBOwnershipChaining); } }
        public String CaseSensitiveMode { get { return Sql.RegisteredServer.YesNoStr(m_CaseSensitiveMode); } }
        public String Hashkey { get { return m_Hashkey; } }
        public int SnapshotId { get { return m_SnapshotId; } }
        public int RegisteredServerId { get { return m_RegisteredServerId; } }
        public String CollectorVersion { get { return m_CollectorVersion; } }
        public String AllowSystemTableUpdates { get { return Sql.RegisteredServer.YesNoStr(m_AllowSystemTableUpdates); } }
        public String RemoteAdminConnectionsEnabled { get { return Sql.RegisteredServer.YesNoStr(m_RemoteAdminConnectionsEnabled); } }
        public String RemoteAccessEnabled { get { return Sql.RegisteredServer.YesNoStr(m_RemoteAccessEnabled); } }
        public String ScanForStartupProcsEnabled { get { return Sql.RegisteredServer.YesNoStr(m_ScanForStartupProcsEnabled); } }
        public String SqlMailXpsEnabled { get { return Sql.RegisteredServer.YesNoStr(m_SqlMailXpsEnabled); } }
        public String DatabaseMailXpsEnabled { get { return Sql.RegisteredServer.YesNoStr(m_DatabaseMailXpsEnabled); } }
        public String OleAutomationProceduresEnabled { get { return Sql.RegisteredServer.YesNoStr(m_OleAutomationProceduresEnabled); } }
        public String WebassistantProceduresEnabled { get { return Sql.RegisteredServer.YesNoStr(m_WebassistantProceduresEnabled); } }
        public String Xp_cmdshellEnabled { get { return Sql.RegisteredServer.YesNoStr(m_Xp_cmdshellEnabled); } }
        public String AgentMailProfile { get { return m_AgentMailProfile; } }
        public String HideInstance { get { return Sql.RegisteredServer.YesNoStr(m_HideInstance); } }
        public String AgentSysadminOnly { get { return Sql.RegisteredServer.YesNoStr(m_AgentSysadminOnly); } }
        public String ServerIsDomainController { get { return Sql.RegisteredServer.YesNoStr(m_ServerIsDomainController); } }
        public String ReplicationEnabled { get { return Sql.RegisteredServer.YesNoStr(m_ReplicationEnabled); } }
        public String SaPasswordEmpty { get { return Sql.RegisteredServer.YesNoStr(m_SaPasswordEmpty); } }
        public String SystemDrive { get { return SystemDriveStr(m_SystemDrive); } }
        public String AdHocDistributedQueriesEnabled { get { return Sql.RegisteredServer.YesNoStr(m_AdHocDistributedQueriesEnabled); } }
        public String WeakPasswordDectectionEnabled { get { return Sql.RegisteredServer.YesNoStr(m_WeakPasswordDetectionEnabled); } }
        public DataTable Protocols { get { return m_Protocols ?? GetProtocols(); } }
        public bool IsMissingWindowsUsers { get { return (SnapshotComment.ToLower().Contains(MissingUsers) ? true : false); } }
        // New Property to support snapshot naming in the future
        public String SnapshotName { get { return StartTime.ToLocalTime().ToString(Utility.Constants.DATETIME_FORMAT); } }
        /// <summary>
        /// returns the Icon index in AppIcons.Enum associated with the Snapshot state
        /// </summary>
        public int IconIndex { get { return GetIconIndex(m_Status, m_Baseline); } }
        /// <summary>
        /// returns the Snapshot Icon image from AppIcons associated with the Snapshot state
        /// </summary>
        public Image Icon { get { return GetIconImage(m_Status, m_Baseline); } }
        /// <summary>
        /// returns true if the snapshot contains permission data that can be viewed (Snapshot status is Successful or Warning)
        /// </summary>
        public bool HasValidPermissions { get { return ((m_Status == Utility.Snapshot.StatusWarning) || (m_Status == Utility.Snapshot.StatusSuccessful)); } }
        public string Duration
        {
            get
            {
                StringBuilder txt = new StringBuilder();

                TimeSpan duration = m_EndTime - m_StartTime;
                if (duration.Hours > 0)
                {
                    txt.Append(duration.Hours + " hour");
                    if (duration.Hours > 1)
                    {
                        txt.Append("s");
                    }
                    txt.Append(" ");
                }
                if (duration.Minutes > 0)
                {
                    txt.Append(duration.Minutes + " minute");
                    if(duration.Minutes > 1)
                    {
                        txt.Append("s");
                    }
                    txt.Append(" ");
                }
                if(duration.Seconds >= 0)
                {
                    txt.Append(duration.Seconds + " second");
                    if (duration.Seconds != 1)
                    {
                        txt.Append("s");
                    }
                }

                return txt.ToString();
            }
        }
        public String VersionFriendly { get { return Sql.SqlHelper.ParseVersionFriendly(m_Version); } }
        public String VersionFriendlyLong { get { return Sql.SqlHelper.ParseVersionFriendly(m_Version, true); } }

        #endregion

        #region Helpers

        static public string SystemDriveStr(string ans)
        {
            return string.IsNullOrEmpty(ans) ? "N/A" : ans;
        }

        public bool MakeBaseline(string comment)
        {
            try
            {
                // Retrieve server information.
                logX.loggerX.Info("Baseline a snapshot");
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    connection.Open();

                    SqlParameter paramId = new SqlParameter(ParamBaselineSnapshotID, m_SnapshotId);
                    SqlParameter paramComment = new SqlParameter(ParamBaselineComment, comment);
                    Sql.SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure,
                            QueryBaselineShapshot, new SqlParameter[] { paramId, paramComment });
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error(@"Error - Unable to baseline snapshot", ex);
                return false;
            }
            return true;
        }

        public bool Delete(out string message)
        {
            try
            {
                // Retrieve server information.
                logX.loggerX.Info("Delete a snapshot");
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    connection.Open();

                    SqlParameter paramId = new SqlParameter(ParamDeleteShapshotSnapshotID, m_SnapshotId);
                            Sql.SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure,
                                    QueryDeleteShapshot, new SqlParameter[] { paramId });
                    {
                        message = "Snapshot deleted successfully";
                        return true;
                    }
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error(@"Error - Unable to delete snapshot", ex);
                message = ex.Message;
                return false;
            }
        }

        static public Snapshot GetSnapShot(int snapshotId)
        {
            Snapshot snap = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    connection.Open();
                    using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                    string.Format(QueryGetSnapshot, snapshotId), null))
                    {
                        while (rdr.Read())
                        {
                            snap = new Snapshot(rdr);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error(@"Error - Unable to get snapshot", ex);
                MsgBox.ShowError(ErrorMsgs.CantGetSnapshot, ex);
                snap = null;
            }

            return snap;
        }

        static public Snapshot.SnapshotList LoadSnapshots(string serverInstance)
        {
            // Create return snapshot list.
            Snapshot.SnapshotList snapshotList = new SnapshotList();

            // Retrieve snapshot information for a server.
            try
            {
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    connection.Open();
                    using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                    string.Format(QueryGetSnapshotList,serverInstance), null))
                    {
                        while (rdr.Read())
                        {
                            Snapshot snap = new Snapshot(rdr);

                            snapshotList.Add(snap);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error(@"Error - Unable to get snapshot list", ex);
                MsgBox.ShowError(ErrorMsgs.CantGetSnapshots, ex);
            }

            return snapshotList;
        }

        static public int SnapshotCount(string serverInstance)
        {
            int count = 0;

            // Retrieve snapshot information for a server.
            try
            {
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    connection.Open();
                    using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                    string.Format(QueryGetSnapshotCount, serverInstance), null))
                    {
                        while (rdr.Read())
                        {
                            count = Convert.ToInt32(rdr[0]);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error(@"Error - Unable to get snapshot count", ex);
                MsgBox.ShowError(ErrorMsgs.CantGetSnapshots, ex);
            }

            return count;
        }

        static public List<string> GetWellKnownGroups(int snapshotID)
        {
            // Create return snapshot list.
            List<string> groupList = new List<string>();

            // Retrieve well known group information for a snapshot.
            logX.loggerX.Info(@"Retrieve well known group list for a snapshot");
            try
            {
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    connection.Open();
                    using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                    string.Format(QueryGetWellKnownGroupList, snapshotID), null))
                    {
                        while (rdr.Read())
                        {
                            groupList.Add(rdr[0].ToString());
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error(@"Error - Unable to get well known group list", ex);
                MsgBox.ShowError(ErrorMsgs.CantGetWellKnownGroups, ex);
            }

            return groupList;
        }

        public static string GetStatusStr(string status)
        {
            string ret;

            switch (status)
            {
                case Utility.Snapshot.StatusInProgress:
                    ret = Utility.Snapshot.StatusInProgressText;
                    break;
                case Utility.Snapshot.StatusWarning:
                    ret = Utility.Snapshot.StatusWarningText;
                    break;
                case Utility.Snapshot.StatusError:
                    ret = Utility.Snapshot.StatusErrorText;
                    break;
                case Utility.Snapshot.StatusSuccessful:
                    ret = Utility.Snapshot.StatusSuccessfulText;
                    break;
                default:
                    //Debug.Assert(false, "Unknown status encountered");
                    logX.loggerX.Warn("Warning - unknown Snapshot status encountered", status);
                    ret = Utility.Snapshot.StatusUnknownText;
                    break;
            }

            return ret;
        }
        public static string GetFormattedWarnings(string details)
        {
            StringBuilder message = new StringBuilder();
            message.Append(Utility.ErrorMsgs.JobSucceededWarningsMsg);
            message.Append("\r\n\r\nWarning Details:");
            message.Append("\r\n");
            string[] warning = details.Split(new string[] { " and " }, StringSplitOptions.RemoveEmptyEntries);
            if (warning.GetLength(0) > 1)
            {
                for (int x = 0; x < warning.GetLength(0); x++)
                {
                    message.AppendFormat("  {0}) {1}\r\n", x + 1, warning[x].Trim());
                }
            }
            else
            {
                message.AppendFormat("  {0} \r\n", warning[0].Trim());
            }

            return message.ToString();
        }

        public static int GetIconIndex(string status, string baseline)
        {
            int iconIndex;

            switch (status)
            {
                case Utility.Snapshot.StatusInProgress:
                    iconIndex = AppIcons.AppImageIndex16(AppIcons.Enum.SnapshotInProgress);
                    break;
                case Utility.Snapshot.StatusWarning:
                    if (string.Compare(baseline, Utility.Snapshot.BaselineTrue, true) == 0)
                    {
                        iconIndex = AppIcons.AppImageIndex16(AppIcons.Enum.SnapshotWarningPlusBaseline);
                    }
                    else
                    {
                        iconIndex = AppIcons.AppImageIndex16(AppIcons.Enum.SnapshotWarnings);
                    }
                    break;
                case Utility.Snapshot.StatusError:
                    iconIndex = AppIcons.AppImageIndex16(AppIcons.Enum.SnapshotError);
                    break;
                case Utility.Snapshot.StatusSuccessful:
                    // Compare only the first char because sometime the full Yes/No string is passed
                    if (baseline.Length > 0
                        && string.Compare(baseline.Substring(0, 1), Utility.Snapshot.BaselineTrue, true) == 0)
                    {
                        iconIndex = AppIcons.AppImageIndex16(AppIcons.Enum.SnapshotBaseline);
                    }
                    else
                    {
                        iconIndex = AppIcons.AppImageIndex16(AppIcons.Enum.SnapshotSM);
                    }
                    break;
                default:
                    //Debug.Assert(false, "Unknown status encountered");
                    logX.loggerX.Warn("Warning - unknown Snapshot status encountered", status);
                    iconIndex = AppIcons.AppImageIndex16(AppIcons.Enum.Unknown);
                    break;
            }

            return iconIndex;
        }

        public static Image GetIconImage(string status, string baseline)
        {
            return AppIcons.AppImage16((AppIcons.Enum)GetIconIndex(status, baseline));
        }

        private DataTable GetProtocols()
        {
            // Create return snapshot list.
            DataTable dt = new DataTable();

            dt.Columns.Add("Name");
            dt.Columns.Add("Address");
            dt.Columns.Add("DynamicPort");
            dt.Columns.Add("Port");
            dt.Columns.Add("Enabled");
            dt.Columns.Add("Active");

            // Retrieve protocol information for a snapshot.
            logX.loggerX.Info(@"Retrieve protocol list for a snapshot");
            try
            {
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    connection.Open();
                    using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                    string.Format(QueryGetProtocols, m_SnapshotId), null))
                    {
                        while (rdr.Read())
                        {
                            dt.Rows.Add(new object[]
                                            {
                                                rdr["protocolname"].ToString(),
                                                rdr["ipaddress"].ToString(),
                                                rdr["dynamicport"].ToString(),
                                                rdr["port"].ToString(),
                                                rdr["enabled"].ToString(),
                                                rdr["active"].ToString()
                                            });
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error(@"Error - Unable to get protocol list", ex);
                MsgBox.ShowError(ErrorMsgs.CantGetProtocols, ex);
            }

            return dt;
        }

        #endregion
    }
}
