/******************************************************************
 * Name: RegisteredServer.cs
 *
 * Description: Encapsulates a Registered Server.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - 2008 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using Idera.SQLsecure.Core.Accounts;
using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Forms;
using Idera.SQLsecure.UI.Console.Utility;

namespace Idera.SQLsecure.UI.Console.Sql
{
    /// <summary>
    /// Encapsulates SQLsecure Registered Server
    /// </summary>
    public class RegisteredServer : IEquatable<String>
    {
        #region IEquatable<String> Members

        public bool Equals(string ServerName)
        {
            return ConnectionName.ToUpper() == ServerName.ToUpper();
        }

        public bool Equals(int ServerId)
        {
            return ConnectionName.ToUpper() == ServerName.ToUpper();
        }

        #endregion

        public enum EmailNotifyType
        {
            Always,
            OnError,
            OnWarning,
            Never
        }

        #region Fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Sql.RegisteredServer");
        private SqlInt32 m_RegisteredServerId;
        private SqlString m_ConnectionName;
        private SqlInt32 m_ConnectionPort;
        private SqlString m_ServerName;
        private SqlString m_InstanceName;
        private SqlString m_SqlLogin;
        private SqlString m_SqlPassword;
        private SqlString m_WindowsUser;
        private SqlString m_WindowsPassword;
        private SqlString m_SQLServerAuthType;  // are SQLLogin and SQLPassword windows or SQL server logins
        private SqlString m_AuthenticationMode;
        private SqlString m_OS;
        private SqlString m_Version;
        private SqlString m_Edition;
        private SqlString m_LoginAuditMode;
        private SqlString m_EnableProxyAccount;
        private SqlString m_EnableC2AuditTrace;
        private SqlString m_CrossDbOwnershipChaining;
        private SqlString m_CaseSensitiveMode;
        private SqlString m_auditfoldersstring;
        private SqlGuid m_jobid;
        private SqlDateTime m_LastCollectionTime;
        private SqlInt32 m_LastCollectionSnapshotId;
        private SqlDateTime m_CurrentCollectionTime;
        private SqlString m_CurrentCollectionStatus;
        private SqlInt32 m_SnapshotRetentionPeriod;
        private bool m_IsDataCollectionInProgress;
        private bool m_ShowDataCollectionComplete;
        private String m_ServerIsDomainController;
        private String m_ReplicationEnabled;
        private String m_SaPasswordEmpty;
        private int m_LastSnapshotId;
        private ServerType m_ServerType;
        private bool m_IsUnregisteredServer;

        Form_StartSnapshotJobAndShowProgress m_StartSnapshotForm;

        #endregion

        #region Ctors

        public RegisteredServer(SqlDataReader rdr)
        {
            setValues(rdr);

            m_IsDataCollectionInProgress = false;
        }

        public RegisteredServer()
        { }

        #endregion

        #region Properties

        public int RegisteredServerId { get { return m_RegisteredServerId.Value; } }
        public string ConnectionName { get { return m_ConnectionName.IsNull? string.Empty: m_ConnectionName.Value.ToUpper(); } }
        public int? ConnectionPort { get { return m_ConnectionPort.IsNull ? (int?)null : m_ConnectionPort.Value; } }
        public string ServerName { get { return (m_ServerName.IsNull ? string.Empty : m_ServerName.Value.ToUpper()); } }
        public string InstanceName { get { return (m_InstanceName.IsNull ? string.Empty : m_InstanceName.Value.ToUpper()); } }
        public string FullName { get { return FullNameStr(ServerName, InstanceName,ServerType); } }
        public string FullConnectionName { get { return FullConnectionNameStr(ServerName, InstanceName, ConnectionPort,ServerType); } }
        public string SqlLogin { get { return (m_SqlLogin.IsNull ? string.Empty : m_SqlLogin.Value); } }
        public string SqlPassword { get { return (m_SqlPassword.IsNull ? string.Empty : Encryptor.Decrypt(m_SqlPassword.Value)); } }
        public string WindowsUser { get { return (m_WindowsUser.IsNull ? string.Empty : m_WindowsUser.Value); } }
        public string WindowsPassword { get { return (m_WindowsPassword.IsNull ? string.Empty : Encryptor.Decrypt(m_WindowsPassword.Value)); } }
        public string SQLServerAuthType { get { return (m_SQLServerAuthType.IsNull ? string.Empty : m_SQLServerAuthType.Value.ToUpper()); } }
        public string AuthenticationMode { get { return AuthenticationModeStr(m_AuthenticationMode.IsNull ? string.Empty : m_AuthenticationMode.Value); } }
        public string OS { get { return (m_OS.IsNull ? string.Empty : m_OS.Value); } }
        public string Version { get { return (m_Version.IsNull ? string.Empty : m_Version.Value); } }
        public string Edition { get { return (m_Edition.IsNull ? string.Empty : m_Edition.Value); } }
        public string LoginAuditMode { get { return LoginAuditModeStr(m_LoginAuditMode.IsNull ? string.Empty : m_LoginAuditMode.Value); } }
        public string EnableProxyAccount { get { return YesNoStr(m_EnableProxyAccount.IsNull ? string.Empty : m_EnableProxyAccount.Value); } }
        public string EnableC2AuditTrace { get { return YesNoStr(m_EnableC2AuditTrace.IsNull ? string.Empty : m_EnableC2AuditTrace.Value); } }
        public string CrossDbOwnershipChaining { get { return YesNoStr(m_CrossDbOwnershipChaining.IsNull ? string.Empty : m_CrossDbOwnershipChaining.Value); } }
        public bool CaseSensitive { get { return YesNoBool(m_CaseSensitiveMode.IsNull ? string.Empty : m_CaseSensitiveMode.Value); } }
        public string CaseSensitiveMode { get { return YesNoStr(m_CaseSensitiveMode.IsNull ? string.Empty : m_CaseSensitiveMode.Value); } }
        public Guid JobId { get { return (m_jobid.IsNull ? Guid.Empty : m_jobid.Value); } }
        public string LastCollectionTime { get { return (m_LastCollectionTime.IsNull ? string.Empty : m_LastCollectionTime.Value.ToLocalTime().ToString(Utility.Constants.DATETIME_FORMAT)); } }
        public int LastCollectionSnapshotId { get { return (m_LastCollectionSnapshotId.IsNull ? 0 : m_LastCollectionSnapshotId.Value); } }
        public string CurrentCollectionTime { get { return (m_CurrentCollectionTime.IsNull ? string.Empty : m_CurrentCollectionTime.Value.ToLocalTime().ToString(Utility.Constants.DATETIME_FORMAT)); } }
        public string CurrentCollectionStatus { get { return Snapshot.GetStatusStr(m_CurrentCollectionStatus.IsNull ? string.Empty : m_CurrentCollectionStatus.Value); } }
        public int SnapshotRetentionPeriod { get { return (m_SnapshotRetentionPeriod.IsNull ? 0 : m_SnapshotRetentionPeriod.Value); } }
        public String ServerIsDomainController { get { return YesNoStr(m_ServerIsDomainController); } }
        public String ReplicationEnabled { get { return YesNoStr(m_ReplicationEnabled); } }
        public String SaPasswordEmpty { get { return YesNoStr(m_SaPasswordEmpty); } }

        //Start-SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
        public string VersionFriendly
        { get
            { if (ServerType == ServerType.AzureSQLDatabase)
                {
                    return VersionName.AzureSQLDatabase;
                }
                else { return SqlHelper.ParseVersionFriendly(m_Version.Value); }
            }
        }
        public string VersionFriendlyLong
        {
            get
            {
                if (ServerType == ServerType.AzureSQLDatabase)
                {
                    return String.Format("{0} v{1}", VersionName.AzureSQLDatabase, m_Version.Value);
                }
                else
                {
                    return SqlHelper.ParseVersionFriendly(m_Version.Value, true);
                }
            }
        }
        //End-SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
        public string AuditFoldersString { get { return m_auditfoldersstring.IsNull ? string.Empty : m_auditfoldersstring.Value.ToLower(); } }

        public string NextCollectionTime
        {
            get
            {
                string audittime = string.Empty;
                ScheduleJob.JobData jobdata;

                if (ScheduleJob.GetJobData(Program.gController.Repository.ConnectionString, JobId, out jobdata) != Guid.Empty)
                {
                    audittime = jobdata.NextRun;
                }

                return audittime;
            }
        }

        public void SetJobId(Guid jobId)
        {
            m_jobid = jobId;
        }

        public void SetStartSnapshotForm(Form_StartSnapshotJobAndShowProgress form)
        {
            m_StartSnapshotForm = form;
        }

        public void ShowDataCollectionProgress()
        {
            if (DataCollectionInProgress && m_StartSnapshotForm != null)
            {
                m_StartSnapshotForm.ShowDialogFromFade();
            }
        }

        public bool DataCollectionInProgress
        {
            get { return m_IsDataCollectionInProgress; }
            set { m_IsDataCollectionInProgress = value; }
        }

        public bool ShowWhenDataCollectionCompletes
        {
            get { return m_ShowDataCollectionComplete; }
            set { m_ShowDataCollectionComplete = value; }
        }

        public ServerType ServerType
        {
            get { return m_ServerType; }
        }

        public bool IsUnregisteredServer
        {
            get { return m_IsUnregisteredServer; }
        }
        #endregion

        #region Queries & Constants

        // Get registered servers.
        private const string QueryGetAllServerBase =
                                    @"SELECT 
                                    registeredserverid,
                                    connectionname,
                                    connectionport,
                                    servername,
                                    instancename,
                                    sqlserverlogin, 
                                    sqlserverpassword, 
                                    serverlogin,
                                    serverpassword,
                                    sqlserverauthtype,
                                    authenticationmode,
                                    os,
                                    version,
                                    edition,
                                    loginauditmode,
                                    enableproxyaccount,
                                    enablec2audittrace,
                                    crossdbownershipchaining,
                                    casesensitivemode,
                                    jobid,
                                    lastcollectiontm,
                                    lastcollectionsnapshotid,
                                    currentcollectiontm,
                                    currentcollectionstatus,
                                    snapshotretentionperiod,
                                    serverisdomaincontroller,
                                    replicationenabled,
                                    sapasswordempty,
                                    auditfoldersstring,
                                    servertype
                                  FROM ";
        private const string QueryGetAllRegisteredServerBase =QueryGetAllServerBase+ "SQLsecure.dbo.vwregisteredserver";
        private const string QueryGetAllUnRegisteredServerBase = QueryGetAllServerBase + "SQLsecure.dbo.vwunregisteredserver";
        private static string QueryGetRegisteredServer = QueryGetAllRegisteredServerBase + @" WHERE connectionname = @instance";
        private static string QueryGetAllRegisteredServers = QueryGetAllRegisteredServerBase + @" ORDER BY connectionname";
        private const string ParamGetRegisteredServerInstance = "instance";
        private const string ParamGetRegisteredServerId = "registeredserverid";
        private const string QueryGetUnregisteredServer = QueryGetAllUnRegisteredServerBase + @" WHERE registeredserverid = @registeredserverid";
        private const string QueryIsServerAddedInAssessment = @"select distinct a.registeredserverid
                                                            from SQLsecure.dbo.policymember a
                                                                inner join SQLsecure.dbo.assessment b on a.policyid = b.policyid
                                                                    and a.assessmentid = b.assessmentid
                                                            where a.registeredserverid = @registeredserverid
                                                                and b.assessmentstate in (N'D', N'P', N'A')";
        // This is the column index to use when obtaining fields from the query
        private enum RegisteredServerColumn
        {
            RegisteredServerId = 0,
            ConnectionName,
            ConnectionPort,
            ServerName,
            InstanceName,
            SqlLogin,
            SqlPassword,
            WindowsUser,
            WindowsPassword,
            SqlServerAuthType,
            AuthenticationMode,
            OS,
            Version,
            Edition,
            LoginAuditMode,
            EnableProxyCount,
            EnableC2AuditTrace,
            CrossDBOwnershipChaining,
            CaseSensitiveMode,
            JobId,
            LastCollectionTm,
            LastCollectionSnapshotId,
            CurrentCollectionTm,
            CurrentCollectionStatus,
            SnapshotRetentionPeriod,
            ServerIsDomainController,
            ReplicationEnabled,
            SaPasswordEmpty,
            AuditFoldersString,
            ServerType
        }

        // Is server registered.
        private const string QueryIsServerRegistererd = @"SELECT TOP 1 connectionname FROM SQLsecure.dbo.vwregisteredserver WHERE connectionname = @instance";
        private const string ParamIsServerRegisteredInstance = "instance";

        // Get Policies for Server
        private const string NonQueryGetServerPolicyList = @"SQLsecure.dbo.isp_sqlsecure_getserverpolicylist";
        private const string ParamGetServerPolicyListRegisteredServerID = "@registeredserverid";

        // Register server.
        private const string NonQueryRegisterServer = @"SQLsecure.dbo.isp_sqlsecure_addregisteredserver";
        private const string ParamRegisterServerConnectionname = "@connectionname";
        private const string ParamRegisterServerConnectionport = "@connectionport";
        private const string ParamRegisterServerServername = "@servername";
        private const string ParamRegisterServerInstancename = "@instancename";
        private const string ParamRegisterServerAuthmode = "@authmode";
        private const string ParamRegisterServerLoginname = "@loginname";
        private const string ParamRegisterServerLoginpassword = "@loginpassword";
        private const string ParamRegisterServerServerlogin = "@serverlogin";
        private const string ParamRegisterServerServerpassword = "@serverpassword";
        private const string ParamRegisterServerVersion = "@version";
        private const string ParamRegisterServerRetentionPeriod = "@retentionperiod";
        private const string ParamAuditFoldersString = "@auditfoldersstring";
        private const string ParamServerType = "@servertype";

        // Remove server.
        private const string NonQueryRemoveServer = @"SQLsecure.dbo.isp_sqlsecure_removeregisteredserver";
        private const string ParamRemoveServerConnectionname = @"@connectionname";

        // Change credentials.
        private const string NonQueryChangeServerCredentials = @"SQLsecure.dbo.isp_sqlsecure_updateregisteredservercredentials";
        private const string NonQueryChangeAuditFolders = @"SQLsecure.dbo.isp_sqlsecure_updateregisteredserverauditfolders";
        private const string ParamChangeServerCredentialsConnectionname = @"@connectionname";
        private const string ParamChangeServerCredentialsLoginname = @"@loginname";
        private const string ParamChangeServerCredentialsLoginpassword = @"@loginpassword";
        private const string ParamChangeServerCredentialsAuthmode = @"@authmode";
        private const string ParamChangeServerCredentialsServerlogin = @"@serverlogin";
        private const string ParamChangeServerCredentialsServerpassword = @"@serverpassword";

        // Update RetentionPeriod
        private const string NonQueryUpdateRetentionPeriod = @"SQLsecure.dbo.isp_sqlsecure_updateretentionperiod ";
        private const string ParamUpdateRetentionPeriodConnectionname = @"@connectionname";
        private const string ParamUpdateRetentionPeriod = "@retentionperiod";

        // Get Latest SnapshotId
        private const string QueryGetLatestSnapshotId = @"SELECT TOP 1 snapshotid FROM SQLsecure.dbo.vwserversnapshot WHERE connectionname = '{0}' ORDER BY starttime desc";
        private const string QueryGetSnapshotIdByDate = @"SELECT snapshotid FROM SQLsecure.dbo.getsnapshotlist({0}, {1}) WHERE connectionname = '{2}'";

        // Add Server to Policy
        private const string NonQueryAddRegisteredServerToPolicy = @"SQLsecure.dbo.isp_sqlsecure_addregisteredservertopolicy";
        private const string ParamRegistedServerId = "@registeredserverid";
        private const string ParamPolicyId = "@policyid";
        private const string ParamAssessmentId = "@assessmentid";

        // Remove Server from Policy
        private const string NonQueryRemoveRegisteredServerFromPolicy = @"SQLsecure.dbo.isp_sqlsecure_removeregisteredserverfrompolicy";

        private const string AuditDateFmt = "'{0}'";
        private const string ParamRemoveFromAssessment = @"@removefromassessments";

        #endregion

        #region Helpers

        private void setValues(SqlDataReader rdr)
        {
            m_RegisteredServerId = rdr.GetSqlInt32((int)RegisteredServerColumn.RegisteredServerId);
            m_ConnectionName = rdr.GetSqlString((int)RegisteredServerColumn.ConnectionName);
            m_ConnectionPort = rdr.GetSqlInt32((int)RegisteredServerColumn.ConnectionPort);
            m_ServerName = rdr.GetSqlString((int)RegisteredServerColumn.ServerName);
            m_InstanceName = rdr.GetSqlString((int)RegisteredServerColumn.InstanceName);
            m_SqlLogin = rdr.GetSqlString((int)RegisteredServerColumn.SqlLogin);
            m_SqlPassword = rdr.GetSqlString((int)RegisteredServerColumn.SqlPassword);
            m_WindowsUser = rdr.GetSqlString((int)RegisteredServerColumn.WindowsUser);
            m_WindowsPassword = rdr.GetSqlString((int)RegisteredServerColumn.WindowsPassword);
            m_SQLServerAuthType = rdr.GetSqlString((int)RegisteredServerColumn.SqlServerAuthType);
            m_AuthenticationMode = rdr.GetSqlString((int)RegisteredServerColumn.AuthenticationMode);
            m_OS = rdr.GetSqlString((int)RegisteredServerColumn.OS);
            m_Version = rdr.GetSqlString((int)RegisteredServerColumn.Version);
            m_Edition = rdr.GetSqlString((int)RegisteredServerColumn.Edition);
            m_LoginAuditMode = rdr.GetSqlString((int)RegisteredServerColumn.LoginAuditMode);
            m_EnableProxyAccount = rdr.GetSqlString((int)RegisteredServerColumn.EnableProxyCount);
            m_EnableC2AuditTrace = rdr.GetSqlString((int)RegisteredServerColumn.EnableC2AuditTrace);
            m_CrossDbOwnershipChaining = rdr.GetSqlString((int)RegisteredServerColumn.CrossDBOwnershipChaining);
            m_CaseSensitiveMode = rdr.GetSqlString((int)RegisteredServerColumn.CaseSensitiveMode);
            m_jobid = rdr.GetSqlGuid((int)RegisteredServerColumn.JobId);
            m_LastCollectionTime = rdr.GetSqlDateTime((int)RegisteredServerColumn.LastCollectionTm);
            m_LastCollectionSnapshotId = rdr.GetSqlInt32((int)RegisteredServerColumn.LastCollectionSnapshotId);
            m_CurrentCollectionTime = rdr.GetSqlDateTime((int)RegisteredServerColumn.CurrentCollectionTm);
            m_CurrentCollectionStatus = rdr.GetSqlString((int)RegisteredServerColumn.CurrentCollectionStatus);
            m_SnapshotRetentionPeriod = rdr.GetSqlInt32((int)RegisteredServerColumn.SnapshotRetentionPeriod);
            m_ServerIsDomainController = SqlHelper.GetString(rdr, (int)RegisteredServerColumn.ServerIsDomainController);
            m_ReplicationEnabled = SqlHelper.GetString(rdr, (int)RegisteredServerColumn.ReplicationEnabled);
            m_SaPasswordEmpty = SqlHelper.GetString(rdr, (int)RegisteredServerColumn.SaPasswordEmpty);
            m_auditfoldersstring = rdr.GetSqlString((int)RegisteredServerColumn.AuditFoldersString);
            m_ServerType = Helper.ConvertSQLTypeStringToEnum(rdr.GetString((int)RegisteredServerColumn.ServerType));
        }

        #endregion

        #region Methods

        public int GetLatestSnapshotId()
        {
            return GetLatestSnapshotId(ConnectionName);
        }

        static public int GetLatestSnapshotId(string serverInstance)
        {
            int snapshotid = 0;

            // Retrieve snapshot information for a server.
            try
            {
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    connection.Open();
                    using (SqlDataReader rdr = SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                    string.Format(QueryGetLatestSnapshotId, serverInstance), null))
                    {
                        if (rdr.HasRows)
                        {
                            rdr.Read();

                            snapshotid = rdr.GetInt32(0);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error(@"Error - Unable to get latest SnapshotId", ex);
                MsgBox.ShowError(ErrorMsgs.CantGetSnapshots, ex);
            }

            return snapshotid;
        }

        static public List<int> GetPoliciesContainingServer(int registerServerId)
        {
            List<int> policyIds = new List<int>();

            try
            {
                // Open connection to repository and get server properties.
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Check if the instance is registered.
                    SqlParameter param = new SqlParameter(ParamGetServerPolicyListRegisteredServerID, registerServerId);
                    using (SqlDataReader rdr = SqlHelper.ExecuteReader(connection, null, CommandType.StoredProcedure,
                                                    NonQueryGetServerPolicyList, new[] { param }))
                    {
                        if (rdr.HasRows)
                        {
                            while (rdr.Read())
                            {
                                int id = rdr.GetInt32(0);
                                policyIds.Add(id);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error("Error - Unable to refresh Retrieve Policy list for server from the Repository", ex);
                //                MsgBox.ShowError(Utility.ErrorMsgs.CantGetRegisteredServer, ex.Message);
            }

            return policyIds;
        }

        static public int GetSnapshotIdByDate(string serverInstance, DateTime? auditDate, bool useBaseline)
        {
            int snapshotid = 0;

            // Retrieve snapshot information for a server.
            try
            {
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    connection.Open();
                    string query =
                        string.Format(QueryGetSnapshotIdByDate,
                        auditDate.HasValue ? string.Format(AuditDateFmt, auditDate) : @"NULL",
                                      Convert.ToInt32(useBaseline), serverInstance);

                    using (SqlDataReader rdr = SqlHelper.ExecuteReader(connection, null, CommandType.Text, query, null))
                    {
                        if (rdr.HasRows)
                        {
                            rdr.Read();

                            snapshotid = rdr.GetInt32(0);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error(@"Error - Unable to get SnapshotId by Date", ex);
                MsgBox.ShowError(ErrorMsgs.CantGetSnapshots, ex);
            }

            return snapshotid;
        }

        public string GetCurrentJobStatus(out string comment, int snapshotID)
        {
            comment = string.Empty;
            string status = string.Empty;

            Snapshot snapshot = null;

            if (snapshotID != 0)
            {
                // Retrieve snapshot & its filters.
                snapshot = Snapshot.GetSnapShot(snapshotID);

                if (snapshot != null)
                {
                    comment = snapshot.SnapshotComment.Trim();
                    status = snapshot.Status.Trim();
                }
            }

            return status;
        }

        public bool StartJob(out Guid newJobID, bool showMsgBoxes = true)
        {
            bool bStarted = false;
            newJobID = JobId;
            m_LastSnapshotId = GetLatestSnapshotId();

            ActivityLog.CreateApplicationActivityEventInRepository(
                Program.gController.Repository.ConnectionString,
                ConnectionName,
                ActivityLog.ActivityCategory_Job,
                ActivityLog.ActivityType_Info,
                ActivityLog.ActivityEvent_Start,
                string.Format("Starting manual snapshot collection for {0} ", ConnectionName));

            if (!Program.gController.Repository.bbsProductLicense.IsLicneseGoodForServerCount(Program.gController.Repository.RegisteredServers.Count))
            {
               if(showMsgBoxes) MsgBox.ShowError(ErrorMsgs.SQLsecureDataCollection, ErrorMsgs.LicenseTooManyRegisteredServers);
                return false;
            }

            ScheduleJob.StartJobReturnCode resultCode;
            resultCode = ScheduleJob.StartJob(Program.gController.Repository.ConnectionString, newJobID, showMsgBoxes);

            if (resultCode == ScheduleJob.StartJobReturnCode.JobNotFound)
            {
                ScheduleJob.ScheduleData scheduleData = new ScheduleJob.ScheduleData();
                scheduleData.SetDefaults();
                scheduleData.Enabled = false;
                newJobID = ScheduleJob.AddJob(Program.gController.Repository.ConnectionString, m_ConnectionName.Value,
                                       Program.gController.Repository.Instance, scheduleData);
                if (newJobID != Guid.Empty)
                {
                    resultCode = ScheduleJob.StartJob(Program.gController.Repository.ConnectionString, newJobID);
                }
                if (resultCode == ScheduleJob.StartJobReturnCode.Success)
                {
                    if (showMsgBoxes) MsgBox.ShowWarning(ErrorMsgs.SQLsecureDataCollection, ErrorMsgs.SQLServerNoJobFoundCreateWarning);
                }
            }
            else if (resultCode == ScheduleJob.StartJobReturnCode.AgentNotStarted)
            {
                if (showMsgBoxes) MsgBox.ShowError(ErrorMsgs.SQLsecureDataCollection, ErrorMsgs.SQLServerAgentNotStarted);
            }

            if (resultCode == ScheduleJob.StartJobReturnCode.Success)
            {
                bStarted = true;
            }

            return bStarted;
        }

        const string NotifyCollectionCompleteSuccessTitle = "SQLsecure Data Collection Completed";
        const string NotifyCollectionCompleteFailureTitle = "SQLsecure Data Collection Error";
        const string NotifyCollectionCompleteSuccessFmt = "Data Collection for SQL Server {0} is complete.";
        const string NotifyCollectionCompleteFailureFmt = "Data Collection for SQL Server {0} failed."
                                                        + " See audit history for details of collection failure.";

        public void GetNotifyText(out string notifyTitle, out string notifyText)
        {
            notifyText = null;
            notifyTitle = null;
            string jobStatus;

            if (HasCollectionEndedInThisInterval(out jobStatus))
            {
                notifyTitle = NotifyCollectionCompleteSuccessTitle;
                notifyText = string.Format(NotifyCollectionCompleteSuccessFmt, FullName);

                if ((!(string.Compare(jobStatus, ScheduleJob.JobStatus_Succeeded, true) == 0))
                    || m_LastSnapshotId == GetLatestSnapshotId())
                {
                    notifyTitle = NotifyCollectionCompleteFailureTitle;
                    notifyText = string.Format(NotifyCollectionCompleteFailureFmt, FullName);
                }
            }
        }

        private bool HasCollectionEndedInThisInterval(out string jobStatus)
        {
            jobStatus = null;
            bool isFinished = false;

            if (m_IsDataCollectionInProgress && !m_jobid.IsNull)
            {
                jobStatus = ScheduleJob.GetJobStatus(Program.gController.Repository.ConnectionString, m_jobid.Value);

                if (!(string.Compare(jobStatus, ScheduleJob.JobStatus_Running, true) == 0))
                {
                    isFinished = true;
                    m_IsDataCollectionInProgress = false;
                    // Update snapshot
                    Program.gController.RefreshSnapshot(ConnectionName, GetLatestSnapshotId(ConnectionName));

                    Program.gController.SignalRefreshServersEvent(false, m_ServerName.Value);

                }
            }
            return isFinished;
        }

        public void RefreshServer()
        {
            try
            {
                // Open connection to repository and get server properties.
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Check if the instance is registered.
                    SqlParameter param = new SqlParameter(ParamGetRegisteredServerInstance, m_ConnectionName);
                    using (SqlDataReader rdr = SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                    QueryGetRegisteredServer, new[] { param }))
                    {
                        if (rdr.HasRows && rdr.Read())
                        {
                            setValues(rdr);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error("Error - Unable to refresh Registered Server from the Repository", ex);
                MsgBox.ShowError(ErrorMsgs.CantGetRegisteredServer, ex.Message);
            }
        }

        static public Repository.RegisteredServerList RefreshRegisteredServers()
        {
            Repository.RegisteredServerList serverlist = LoadRegisteredServers(Program.gController.Repository.ConnectionString);

            if (serverlist.Count > 0 && Program.gController.Repository.RegisteredServers.Count > 0)
            {
                foreach (RegisteredServer newserver in serverlist)
                {
                    RegisteredServer oldserver = Program.gController.Repository.RegisteredServers.Find(newserver.ConnectionName);

                    if (oldserver != null)
                    {
                        newserver.DataCollectionInProgress = oldserver.DataCollectionInProgress;
                        newserver.ShowWhenDataCollectionCompletes = oldserver.ShowWhenDataCollectionCompletes;
                        newserver.m_StartSnapshotForm = oldserver.m_StartSnapshotForm;
                    }
                }
            }

            return serverlist;
        }

        static public Repository.RegisteredServerList LoadRegisteredServers(string connectionString)
        {
            Repository.RegisteredServerList serverList = new Repository.RegisteredServerList();

            try
            {
                if (!string.IsNullOrEmpty(connectionString))
                {
                    // Retrieve server information.
                    logX.loggerX.Info("Retrieve Registered Servers");

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        // Open the connection.
                        connection.Open();

                        using (SqlDataReader rdr = SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                        QueryGetAllRegisteredServers, null))
                        {
                            while (rdr.Read())
                            {
                                RegisteredServer server = new RegisteredServer(rdr);

                                serverList.Add(server);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error(string.Format(ErrorMsgs.ErrorStub, ErrorMsgs.CantGetRegisteredServers), ex);
                MsgBox.ShowError(ErrorMsgs.CantGetRegisteredServers, ex.Message);
            }
            catch (Exception ex)
            {
                logX.loggerX.Error(string.Format(ErrorMsgs.ErrorStub, ErrorMsgs.CantGetRegisteredServers), ex);
                MsgBox.ShowError(ErrorMsgs.CantGetRegisteredServers, ex.Message);
            }

            return serverList;
        }

        static public bool IsServerRegistered(string instance)
        {
            return IsServerRegistered(Program.gController.Repository.ConnectionString, instance);
        }

        static public bool IsServerRegistered(string connectionString, string instance)
        {
            Debug.Assert(!string.IsNullOrEmpty(connectionString));
            Debug.Assert(!instance.Contains(","));

            // Init return.
            bool isRegistered = false;

            if (!string.IsNullOrEmpty(instance))
            {
                try
                {
                    // Open connection to repository and check if the server exists.
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        // Open the connection.
                        connection.Open();

                        // Check if the instance is registerred.
                        SqlParameter param = new SqlParameter(ParamIsServerRegisteredInstance, instance);
                        using (SqlDataReader rdr = SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                                               QueryIsServerRegistererd,
                                                                               new[] { param }))
                        {
                            isRegistered = rdr.HasRows;
                        }
                    }
                }
                catch (SqlException ex)
                {
                    logX.loggerX.Error(string.Format(ErrorMsgs.ErrorStub, ErrorMsgs.CantGetRegisteredServer), ex);
                    MsgBox.ShowError(ErrorMsgs.RegisteredServerCaption, ErrorMsgs.CantGetRegisteredServer, ex);
                }
            }

            return isRegistered;
        }

        static public void AddServer(
                string connectionString,
                string newConnection,
                int? connectionPort,
                string machine,
                string instance,
                string sqlAuthMode,
                string sqlLogin,
                string sqlPassword,
                string windowsUser,
                string windowsPassword,
                string version,
                int retentionPeriod,
                string[] auditFolders,
                string servertype
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(connectionString));
            Debug.Assert(!string.IsNullOrEmpty(newConnection));
            Debug.Assert(!string.IsNullOrEmpty(machine));
            newConnection = newConnection.ToUpper();
            // Encrypt passwords before saving them to the repository
            string cipherSqlPassword = Encryptor.Encrypt(sqlPassword);
            string cipherWindowsPassword = Encryptor.Encrypt(windowsPassword);
            string auditFoldersString = FormAuditFoldersString(auditFolders);

            // Open connection to repository and add server.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open the connection.
                connection.Open();

                // Setup register server params.
                SqlParameter paramConnectionname = new SqlParameter(ParamRegisterServerConnectionname, newConnection);
                SqlParameter paramConnectionport = new SqlParameter(ParamRegisterServerConnectionport, connectionPort);
                SqlParameter paramServername = new SqlParameter(ParamRegisterServerServername, machine);
                SqlParameter paramInstancename = new SqlParameter(ParamRegisterServerInstancename, instance);
                SqlParameter paramAuthmode = new SqlParameter(ParamRegisterServerAuthmode, sqlAuthMode);
                SqlParameter paramLoginname = new SqlParameter(ParamRegisterServerLoginname, sqlLogin);
                SqlParameter paramLoginpassword = new SqlParameter(ParamRegisterServerLoginpassword, cipherSqlPassword);
                SqlParameter paramServerlogin = new SqlParameter(ParamRegisterServerServerlogin, windowsUser);
                SqlParameter paramServerpassword = new SqlParameter(ParamRegisterServerServerpassword, cipherWindowsPassword);
                SqlParameter paramVersion = new SqlParameter(ParamRegisterServerVersion, version);
                SqlParameter paramRetentionPeriod = new SqlParameter(ParamRegisterServerRetentionPeriod, retentionPeriod);
                SqlParameter paramAuditFoldersString = new SqlParameter(ParamAuditFoldersString, auditFoldersString);
                SqlParameter paramServertype = new SqlParameter(ParamServerType, servertype);

                SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure,
                                NonQueryRegisterServer, paramConnectionname, paramConnectionport, paramServername, paramInstancename, paramAuthmode, paramLoginname, paramLoginpassword, paramServerlogin, paramServerpassword, paramVersion, paramRetentionPeriod, paramAuditFoldersString,paramServertype);
            }
        }

        static public void RemoveServer(string connectionString, string removeConnection, bool removeFromAssessment)
        {
            Debug.Assert(!string.IsNullOrEmpty(connectionString));
            Debug.Assert(!string.IsNullOrEmpty(removeConnection));
            Debug.Assert(!removeConnection.Contains(","));

            RegisteredServer registeredServer = null;
            GetServer(connectionString, removeConnection, out registeredServer);
            ScheduleJob.RemoveJob(connectionString, registeredServer.JobId, ScheduleJob.GetSnapshotJobName(registeredServer.ConnectionName));

            // Open connection to repository and remove server.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open the connection.
                connection.Open();

                // Setup register server params.
                SqlParameter paramConnectionname = new SqlParameter(ParamRemoveServerConnectionname, removeConnection);
                SqlParameter paramRemoveFromAssessment = new SqlParameter(ParamRemoveFromAssessment, removeFromAssessment);
                SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure,
                                NonQueryRemoveServer, paramConnectionname, paramRemoveFromAssessment);
            }
        }

        static public RegisteredServer GetServer(string instance)
        {
            RegisteredServer server;

            GetServer(Program.gController.Repository.ConnectionString, instance, out server);

            return server;
        }

        static public void GetServer(string connectionString, string instance, out RegisteredServer registeredServer)
        {
            Debug.Assert(!string.IsNullOrEmpty(connectionString));
            Debug.Assert(!string.IsNullOrEmpty(instance));
            Debug.Assert(!instance.Contains(","));

            // Init return.
            registeredServer = null;

            // Open connection to repository and get server properties.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open the connection.
                connection.Open();

                // Check if the instance is registered.
                SqlParameter param = new SqlParameter(ParamGetRegisteredServerInstance, instance);
                using (SqlDataReader rdr = SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                QueryGetRegisteredServer, new[] { param }))
                {
                    if (rdr.HasRows && rdr.Read())
                    {
                        registeredServer = new RegisteredServer(rdr);
                    }
                }
            }
        }

        static public void UpdateRetentionPeriod(string connectionString, string connectionName, int retentionPeriod)
        {
            Debug.Assert(!string.IsNullOrEmpty(connectionString));
            Debug.Assert(!string.IsNullOrEmpty(connectionName));

            // Open connection to repository and check if the server exists.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open the connection.
                connection.Open();

                // Setup register server params.
                SqlParameter paramConnectionname = new SqlParameter(ParamUpdateRetentionPeriodConnectionname, connectionName);
                SqlParameter paramRetentionPeriod = new SqlParameter(ParamUpdateRetentionPeriod, retentionPeriod);

                SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure,
                                NonQueryUpdateRetentionPeriod, paramConnectionname, paramRetentionPeriod);
            }
        }

        static public void UpdateCredentials(
                string connectionString,
                string connectionName,
                string sqlLogin,
                string sqlPassword,
                string sqlAuthType,
                string windowsUser,
                string windowsPassword
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(connectionString));
            Debug.Assert(!string.IsNullOrEmpty(connectionName));

            // Encrypt the passwords before writing to the database.
            string cipherSqlPassword = Encryptor.Encrypt(sqlPassword);
            string cipherWindowsPassword = Encryptor.Encrypt(windowsPassword);

            // Open connection to repository and check if the server exists.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open the connection.
                connection.Open();

                // Setup register server params.
                SqlParameter paramConnectionname = new SqlParameter(ParamChangeServerCredentialsConnectionname, connectionName);
                SqlParameter paramLogingname = new SqlParameter(ParamChangeServerCredentialsLoginname, sqlLogin);
                SqlParameter paramLoginpassword = new SqlParameter(ParamChangeServerCredentialsLoginpassword, cipherSqlPassword);
                SqlParameter paramAuthmode = new SqlParameter(ParamChangeServerCredentialsAuthmode, sqlAuthType);
                SqlParameter paramServerlogin = new SqlParameter(ParamChangeServerCredentialsServerlogin, windowsUser);
                SqlParameter paramServerpassword = new SqlParameter(ParamChangeServerCredentialsServerpassword, cipherWindowsPassword);

                SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure,
                                NonQueryChangeServerCredentials, paramConnectionname, paramLogingname, paramLoginpassword, paramAuthmode, paramServerlogin, paramServerpassword);
            }
        }

        static public void UpdateFolders(string connectionString, string connectionName, string[] folders)
        {
            Debug.Assert(!string.IsNullOrEmpty(connectionString));

            try
            {
                string auditFoldersString = FormAuditFoldersString(folders);
                // Open connection to repository and add server.
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Setup audit folder server params.
                    SqlParameter paramConnectionname = new SqlParameter(ParamChangeServerCredentialsConnectionname, connectionName);
                    SqlParameter paramAuditFoldersString = new SqlParameter(ParamAuditFoldersString, auditFoldersString);
                    SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure,
                                                  NonQueryChangeAuditFolders, paramConnectionname, paramAuditFoldersString);
                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error(string.Format("Failed to update audit folders. Error message: {0}", ex.Message));
            }
        }

        static public void AddRegisteredServerToPolicy(int registeredServerId, int policyId)
        {
            try
            {
                // Open connection to repository and add server.
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Setup register server params.
                    SqlParameter paramRegisteredServerId = new SqlParameter(ParamRegistedServerId, registeredServerId);
                    SqlParameter paramPolicyId = new SqlParameter(ParamPolicyId, policyId);

                    SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure,
                                                  NonQueryAddRegisteredServerToPolicy, paramRegisteredServerId, paramPolicyId);
                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error(string.Format("Failed to add Registered Server to policy error message: {0}", ex.Message));
            }
        }

        static public void AddRegisteredServerToPolicy(int registeredServerId, int policyId, int assessmentId)
        {
            try
            {
                // Open connection to repository and add server.
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Setup register server params.
                    SqlParameter paramRegisteredServerId = new SqlParameter(ParamRegistedServerId, registeredServerId);
                    SqlParameter paramPolicyId = new SqlParameter(ParamPolicyId, policyId);
                    SqlParameter paramAssessmentId = new SqlParameter(ParamAssessmentId, assessmentId);

                    SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure,
                                                  NonQueryAddRegisteredServerToPolicy, paramRegisteredServerId, paramPolicyId, paramAssessmentId);
                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error(string.Format("Failed to add Registered Server to policy error message: {0}", ex.Message));
            }
        }

        static public void RemoveRegisteredServerFromPolicy(int registeredServerId, int policyId, int assessmentId)
        {
            try
            {
                // Open connection to repository and add server.
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Setup register server params.
                    SqlParameter paramRegisteredServerId = new SqlParameter(ParamRegistedServerId, registeredServerId);
                    SqlParameter paramPolicyId = new SqlParameter(ParamPolicyId, policyId);
                    SqlParameter paramAssessmentId = new SqlParameter(ParamAssessmentId, assessmentId);

                    SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure,
                                                  NonQueryRemoveRegisteredServerFromPolicy, paramRegisteredServerId, paramPolicyId, paramAssessmentId);
                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error(string.Format("Failed to remove Registered Server from policy error message: {0}", ex.Message));
            }
        }

        //Barkha Khatri(SQLSecure3.1) SQLSecure 1834 fix
        static public string FullNameStr(string server, string instance, ServerType serverType)
        {
            Debug.Assert(!string.IsNullOrEmpty(server));

            string full = server;

            if (!string.IsNullOrEmpty(instance) && serverType!=ServerType.AzureSQLDatabase )
            {
                full += @"\" + instance;
            }

            return full;
        }

        static public string FullConnectionNameStr(string server, string instance, int? port,ServerType serverType)
        {
            string full = FullNameStr(server, instance,serverType);

            if (port.HasValue)
            {
                full += @"," + port;
            }

            return full;
        }

        static public string AuthenticationModeStr(string mode)
        {
            string ret = string.Empty;

            if (string.IsNullOrEmpty(mode)) { ret = string.Empty; }
            else if (string.Compare(mode, "W", true) == 0) { ret = "Windows Authentication"; }
            else if (string.Compare(mode, "M", true) == 0) { ret = "SQL Server and Windows Authentication"; }
            else { Debug.Assert(false, "Unknown SQL authentication mode"); }

            return ret;
        }

        //Start-SQLsecure 3.1 (Tushar)--Adding support for Azure SQL Database.
        /// <summary>
        /// Return authentication mode string that needs to be viwed at UI according to server type.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        static public string AuthenticationModeStr(string mode, ServerType serverType)
        {
            string ret = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(mode)) { ret = string.Empty; }
                else if (string.Compare(mode, "W", true) == 0) { ret = "Windows Authentication"; }
                else if (string.Compare(mode, "M", true) == 0 && serverType == ServerType.AzureSQLDatabase) { ret = "SQL Login and Azure AD"; }
                else if (string.Compare(mode, "M", true) == 0) { ret = "SQL Server and Windows Authentication"; }
                else { Debug.Assert(false, "Unknown SQL authentication mode"); }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error(@"Error while converting authentication mode string.", ex);
            }
            return ret;
        }
        //End-SQLsecure 3.1 (Tushar)--Adding support for Azure SQL Database.

        static public string LoginAuditModeStr(string mode)
        {
            string ret = string.Empty;

            if (string.IsNullOrEmpty(mode)) { ret = string.Empty; }
            else if (string.Compare(mode, "Success", true) == 0) { ret = "Successful logins only"; }
            else if (string.Compare(mode, "Failure", true) == 0) { ret = "Failed logins only"; }
            else if (string.Compare(mode, "All", true) == 0) { ret = "Both failed and successful logins"; }
            else { ret = mode; }

            return ret;
        }

        static public string YesNoStr(string ans)
        {
            string ret = string.Empty;

            if (string.IsNullOrEmpty(ans)) { ret = string.Empty; }
            else if (string.Compare(ans, "Y", true) == 0) { ret = "Yes"; }
            else if (string.Compare(ans, "N", true) == 0) { ret = "No"; }
            else if (string.Compare(ans, "U", true) == 0) { ret = "N/A"; }
            else { Debug.Assert(false, "Unknown answer"); }

            return ret;
        }

        static public bool YesNoBool(string ans)
        {
            bool ret = false;

            if (string.IsNullOrEmpty(ans)) { ret = false; }
            else if (string.Compare(ans, "Y", true) == 0) { ret = true; }
            else if (string.Compare(ans, "N", true) == 0) { ret = false; }
            else { Debug.Assert(false, "Unknown answer"); }

            return ret;
        }

        private static string FormAuditFoldersString(string[] folders)
        {
            string auditFoldersString = null;

            if (folders != null && folders.Length > 0)
            {
                auditFoldersString = string.Join(Utility.Constants.AUDIT_FOLDER_DELIMITER, folders);
            }

            return auditFoldersString;
        }

        public void LoadUnregisteredServer(int svrId)
        {
            try
            {               
                // Open connection to repository and get server properties.
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Check if the instance is registered.
                    SqlParameter param = new SqlParameter(ParamGetRegisteredServerId, svrId);
                    using (SqlDataReader rdr = SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                    QueryGetUnregisteredServer, new[] { param }))
                    {
                        if (rdr.HasRows && rdr.Read())
                        {
                            setValues(rdr);
                            m_IsUnregisteredServer = true;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error("Error - Unable to refresh Registered Server from the Repository", ex);
                MsgBox.ShowError(ErrorMsgs.CantGetRegisteredServer, ex.Message);
            }
        }

        static public bool IsServerAddedInAssessment(string connectionString, string removeConnection)
        {
            Debug.Assert(!string.IsNullOrEmpty(connectionString));
            Debug.Assert(!string.IsNullOrEmpty(removeConnection));
            RegisteredServer registeredServer = null;
            GetServer(connectionString, removeConnection, out registeredServer);
            Debug.Assert(!removeConnection.Contains(",")); // Open connection to repository and remove server.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open the connection.
                connection.Open();

                // Setup register server params.
                // Check if the instance is registered.
                SqlParameter param = new SqlParameter(ParamGetRegisteredServerId, registeredServer.RegisteredServerId);
                using (SqlDataReader rdr = SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                QueryIsServerAddedInAssessment, new[] { param }))
                {
                    if (rdr.HasRows)
                        return true;
                }
            }
            return false;
        }

        #endregion
    }
}