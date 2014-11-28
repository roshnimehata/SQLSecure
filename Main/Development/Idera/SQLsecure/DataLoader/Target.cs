/******************************************************************
 * Name: Target.cs
 *
 * Description: Encapsulates the target SQL Server instance.
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
using System.Security.Principal;
using System.Management;
using System.Reflection;

using Wintellect;
using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.Core.Accounts;
using Idera.SQLsecure.Collector.Sql;


namespace Idera.SQLsecure.Collector
{
    public enum MetricMeasureType
    {
        Count,
        Time
    }

    /// <summary>
    /// Encapsulates the target server instance.
    /// </summary>
    internal class Target
    {
        #region Fields
        private Repository m_Repository;
        private SqlConnectionStringBuilder m_ConnectionStringBuilder;
        private Sql.ServerVersion m_VersionEnum;
        private Core.Accounts.Server m_Server;
        private List<Sql.Filter> m_FilterList;
        private bool m_IsValid;
        private int m_snapshotId;
        private static LogX logX = new LogX("Idera.SQLsecure.Collector.Target");
        private string sqlLogin, sqlPassword, sqlAuthType, serverLogin, serverPassword;
        private FilePermissions filePermissions = null;
        private RegistryPermissions registryPermissions = null;
        private SQLServices sqlServices = null;
        private string[] m_auditFolders = null;

        #endregion

        Server.WriteActivityToRepositoryDelegate WriteAppActivityToRepository;


        #region Statics
        // These are needed for error or metrics reporting throghout the application
        // -------------------------------------------------------------------------
        public static string TargetInstance = string.Empty;
        public static string RepositoryConnectionString = string.Empty;
        public static uint numDatabaseObjectsCollected = 0;
        public static uint numPermissionsCollected = 0;
        public static uint numLoginsCollected = 0;
        public static uint numGroupMembersCollected = 0;
        public static string lastErrorMsg = string.Empty;
        #endregion


        #region Queries
        private const string QueryIsSysadmin =
                    @"SELECT is_srvrolemember('sysadmin')";
        private const string QueryAuthenticationMode =
                    @"SELECT SERVERPROPERTY('IsIntegratedSecurityOnly')";
        private const string QueryVersion =
                    @"SELECT SERVERPROPERTY('ProductVersion')";
        private const string QueryEdition =
                    @"SELECT SERVERPROPERTY('Edition')";
        private const string QueryServername =
                    @"SELECT SERVERPROPERTY('MachineName')";
        private const string QueryInstancename =
                    @"select instancename = CAST(ServerProperty('InstanceName') AS nvarchar)";
        private const string QueryCollation =
                    @"SELECT SERVERPROPERTY('Collation')";
        //private const string QueryLoginAuditMode =
        //            @"EXEC master.dbo.xp_instance_regread N'HKEY_LOCAL_MACHINE', N'SOFTWARE\Microsoft\MSSQLServer\MSSQLServer', N'AuditLevel'";
        private const string QueryProxyEnabled =
                    @"SELECT s.name AS [Name] FROM sys.credentials AS s 
                      WHERE (s.name=N'##xp_cmdshell_proxy_account##')";
        private const string QueryIssaPasswordNull2K =
            @"SELECT COUNT(name) FROM master.dbo.syslogins WHERE sid = 0x01 and (password is null or pwdcompare('', password) = 1)";
        private const string QueryIssaPasswordNull2K5 =
            @"SELECT COUNT(name) FROM sys.sql_logins WHERE sid = 0x01 and pwdcompare('', password_hash) = 1";
        private const string QueryConfigurations2K =
            @"SELECT config, value FROM master.dbo.sysconfigures
                      WHERE config in (102, 117, 400, 544, 1547)";
        private const string QueryConfigurations2K5 =
            @"SELECT configuration_id, value_in_use FROM sys.configurations
                      WHERE configuration_id in (102, 117, 400, 544, 1547, 1576, 16385, 16386, 16388, 16389, 16390, 16391, 1562, 1568)";
        private const string QuerySysAdminOnlyForSQLAgentCmdExecJobs2k5 =
            "EXEC msdb.dbo.sp_enum_proxy_for_subsystem @subsystem_id = 3";
        private const string QuerySysAdminOnlyForSQLAgentCmdExecJobs2k =
            "EXEC master.dbo.xp_sqlagent_proxy_account N'GET'";
        private const string QueryReplicationEnabled = @"EXEC sp_helpreplicationdboption";

        private const string QueryDistributorEnabled = @"EXEC sp_get_distributor";

        private const string NonQueryCreateSnapshot =
                    @"SET DATEFORMAT mdy;
                        INSERT INTO SQLsecure.dbo.serversnapshot 
                            (connectionname, servername, instancename, starttime, authenticationmode, 
                                os, version, edition, status,
                                crossdbownershipchaining, enablec2audittrace, 
                                enableproxyaccount,casesensitivemode, registeredserverid,
                                collectorversion, allowsystemtableupdates, remoteadminconnectionsenabled,
                                remoteaccessenabled, scanforstartupprocsenabled, sqlmailxpsenabled,
                                databasemailxpsenabled, oleautomationproceduresenabled,
                                webassistantproceduresenabled, xp_cmdshellenabled, serverisdomaincontroller,
                                sapasswordempty, agentsysadminonly, replicationenabled, systemdrive, adhocdistributedqueriesenabled,
                                isweakpassworddetectionenabled, isdistributor, ispublisher, hasremotepublisher, isclrenabled, isdefaulttraceenabled)
                      select    connectionname = '{0}', 
                                servername = '{1}', 
                                instancename = '{2}', 
                                starttime = '{3}', 
                                authenticationmode = '{4}', 
                                os = '{5}', 
                                version = '{6}', 
                                edition = '{7}', 
                                status = '{8}',
                                crossdbownershipchaining = '{9}', 
                                enablec2audittrace = '{10}', 
                                enableproxyaccount = '{11}', 
                                casesensitivemode = '{12}',
                                registeredserverid,
                                collectorversion = '{13}',
                                allowsystemtableupdates = '{14}',
                                remoteadminconnectionsenabled = '{15}',
                                remoteaccessenabled = '{16}',
                                scanforstartupprocsenabled = '{17}',
                                sqlmailxpsenabled = '{18}',
                                databasemailxpsenabled = '{19}',
                                oleautomationproceduresenabled = '{20}',
                                webassistantproceduresenabled = '{21}',
                                xp_cmdshellenabled = '{22}',
                                serverisdomaincontroller = '{23}',
                                sapasswordempty = '{24}',
                                agentsysadminonly = '{25}',
                                replicationenabled = '{26}',
                                systemdrive = '{27}',
                                adhocdistributedqueriesenabled = '{28}',
                                isweakpassworddetectionenabled = '{29}',
                                isdistributor = '{30}',
                                ispublisher = '{31}', 
                                hasremotepublisher = '{32}',
                                isclrenabled = '{33}',
                                isdefaulttraceenabled = '{34}'
                      from SQLsecure.dbo.registeredserver where connectionname = '{0}'";

        public const string NonQueryCreateSnapshotHistory =
                    @"INSERT INTO SQLsecure.dbo.snapshothistory
                            (snapshotid, starttime, numberoferror, status)
                      VALUES (@snapshotid, @starttime, @numberoferror, @status)";
        public const string QuerySnapshotId =
                    @"SELECT TOP 1 snapshotid FROM SQLsecure.dbo.serversnapshot 
                        WHERE connectionname = @connectionname
                        ORDER BY snapshotid DESC";
        private const string ParamConnectionname = "connectionname";
        private const string ParamServerName = "servername";
        private const string ParamInstanceName = "instancename";
        private const string ParamStarttime = "starttime";
        private const string ParamAuthenticationmode = "authenticationmode";
        private const string ParamOs = "os";
        private const string ParamVersion = "version";
        private const string ParamEdition = "edition";
        private const string ParamSnapshotComment = "snapshotcomment";
        private const string ParamLoginauditmode = "loginauditmode";
        private const string ParamCrossdbownershipchaining = "crossdbownershipchaining";
        private const string ParamEnablec2audittrace = "enablec2audittrace";
        private const string ParamEnableProxyAccount = "enableproxyaccount";
        private const string ParamCaseSensitiveMode = "casesensitivemode";

        // Snapshot History Table
        // ----------------------
        public const string ParamSnapshotHistoryID = "snapshothistoryid";
        public const string ParamSnapshotID = "snapshotid";
        public const string ParamScheduletime = "scheduletime";
        public const string ParamStartTime = "starttime";
        public const string ParamEndTime = "endtime";
        public const string ParamNumberofErrors = "numberoferror";
        public const string ParamStatus = "status";

        #endregion

        #region Helpers
        private bool isValid()
        {
            // Validate the server object.
            if (!m_Server.IsValid)
            {
                logX.loggerX.Error("ERROR - Invalid member server object");

                return false;
            }
            // Connect to the target instance and validate.                                 
            using (SqlConnection connection = new SqlConnection(m_ConnectionStringBuilder.ConnectionString))
            {
                // Open connection to the target SQL Server.  
                try
                {
                    // Open the connection.
                    connection.Open();

                    // Check the SQL Server version.
                    m_VersionEnum = Sql.SqlHelper.ParseVersion(connection.ServerVersion);
                    if (m_VersionEnum == Sql.ServerVersion.Unsupported)
                    {
                        string message = "Target SQL Server version: " + connection.ServerVersion
                                                + " is not supported.";
                        logX.loggerX.Error("ERROR - ", message);
                        AppLog.WriteAppEventError(SQLsecureEvent.DlErrInvalidTargetVersionMsg,
                                                    SQLsecureCat.DlValidationCat, m_ConnectionStringBuilder.DataSource,
                                                        connection.ServerVersion);
                        Sql.Database.CreateApplicationActivityEventInRepository(m_Repository.ConnectionString,
                                    m_ConnectionStringBuilder.DataSource,
                                    0,
                                    Collector.Constants.ActivityType_Error,
                                    Collector.Constants.ActivityEvent_Error,
                                    message);

                        return false;
                    }

                    // Check for permissions to read data from SQL Server instance.
                    logX.loggerX.Info("Checking for permissions to read data from target SQL Server instance");
                    using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                    QueryIsSysadmin, null))
                    {
                        int havePermissions = 0;
                        if (rdr.Read()) // Only one row expected.
                        {
                            havePermissions = (int)rdr[0];
                        }
                        if (havePermissions != 1)
                        {
                            string message = "SQLsecure doesn't have permissions to load security data from target instance.";
                            logX.loggerX.Error("ERROR - ", message);
                            AppLog.WriteAppEventError(SQLsecureEvent.DlErrNoTargetPermissions,
                                                        SQLsecureCat.DlValidationCat,
                                                            m_ConnectionStringBuilder.DataSource);
                            Sql.Database.CreateApplicationActivityEventInRepository(m_Repository.ConnectionString,
                                        m_ConnectionStringBuilder.DataSource,
                                        0,
                                        Collector.Constants.ActivityType_Error,
                                        Collector.Constants.ActivityEvent_Error,
                                        message);

                            return false;
                        }
                    }
                }
                catch (SqlException ex)
                {
                    string message = "Exception raised when checking if target is valid. " + ex.Message;
                    logX.loggerX.Error("ERROR - ", message);
                    AppLog.WriteAppEventError(SQLsecureEvent.ExErrExceptionRaised, SQLsecureCat.DlValidationCat,
                        " SQL Server = " + m_ConnectionStringBuilder.DataSource +
                        "Target validation", ex.Message);
                    Sql.Database.CreateApplicationActivityEventInRepository(m_Repository.ConnectionString,
                                m_ConnectionStringBuilder.DataSource,
                                0,
                                Collector.Constants.ActivityType_Error,
                                Collector.Constants.ActivityEvent_Error,
                                message);

                    return false;
                }
                connection.Close();
                return true;
            }
        }
        #endregion

        #region Ctors
        /// <summary>
        /// Target ctor.
        /// </summary>
        /// <param name="targetInstance"></param>
        /// <param name="repository"></param>
        public Target(
                string targetInstance,
                Repository repository
            )
        {
            // Set the repository
            m_Repository = repository;
            RepositoryConnectionString = repository.ConnectionString;
            m_IsValid = true;
            m_snapshotId = 0;
            WriteAppActivityToRepository = new Server.WriteActivityToRepositoryDelegate(CreateApplicationActivityEventInRepository);

            // Retrieve target instance credentials from the repository.
            string server;
            int? port;
            if (m_Repository.GetTargetCredentials(targetInstance, out server, out port, out sqlLogin, out sqlPassword,
                                                    out sqlAuthType, out serverLogin,
                                                        out serverPassword))
            {
                try
                {
                    string login = string.Empty;
                    string password = string.Empty;
                    if (sqlAuthType.ToUpper() == "S")
                    {
                        login = sqlLogin;
                        password = sqlPassword;
                    }
                    m_ConnectionStringBuilder = Sql.SqlHelper.ConstructConnectionString(targetInstance, port, login,
                                                                                            password);
                    TargetInstance = targetInstance;

                    Program.ImpersonationContext wi2 = Program.SetTargetImpersonationContext();
                    m_Server = new Idera.SQLsecure.Core.Accounts.Server(server, serverLogin, serverPassword,
                                                                        WriteAppActivityToRepository);
                    Program.RestoreImpersonationContext(wi2);

                    m_IsValid = isValid();

                }
                catch (Exception ex)
                {
                    logX.loggerX.Error("ERROR - exception raised when creating Target object", ex.Message);
                    m_ConnectionStringBuilder = null;
                    m_VersionEnum = Sql.ServerVersion.Unsupported;
                    m_Server = null;
                    m_IsValid = false;
                }
            }
            else
            {
                logX.loggerX.Error("ERROR - failed to retrieve target credentials.");
                m_IsValid = false;
            }

            //Retrieve audit folders 
            m_auditFolders = m_Repository.GetAuditFolders(targetInstance);

            // Retrieve the filter rules.
            if (m_IsValid)
            {
                if (!m_Repository.GetCollectionFilters(targetInstance, out m_FilterList))
                {
                    logX.loggerX.Error("ERROR - failed to retrieve collection filter rules");
                    m_IsValid = false;
                }
            }
        }

        #endregion

        #region Properties
        public bool IsValid
        {
            get { return m_IsValid; }
        }

        /// <summary>
        /// Target server connection string.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                Debug.Assert(IsValid);
                return m_ConnectionStringBuilder.ConnectionString;
            }
        }
        public string SqlLogin
        {
            get { return sqlLogin; }
        }
        public string SqlPassword
        {
            get { return sqlPassword; }
        }
        public bool IsSQLLoginType
        {
            get { return (sqlAuthType == "SQL" ? true : false); }
        }
        public string ServerLogin
        {
            get { return serverLogin; }
        }
        public string ServerPassword
        {
            get { return serverPassword; }
        }

        public Server TargetServer
        {
            get { return m_Server; }
        }

        #endregion

        #region Data Load Functions



        // 
        private Constants.CollectionStatus createSnapshot(out int snapshotid)
        {
            // Init returns.
            Constants.CollectionStatus enumStatus = Constants.CollectionStatus.StatusSuccess;
            bool isOk = true;
            snapshotid = 0;

            // Connect to the target and retrieve instance properties.
            char authenticationMode = Constants.MixedAuthentication;
            string version = string.Empty;
            string edition = string.Empty;
            char crossDbOwnership = Constants.Unknown;
            char enableC2AuditTrace = Constants.Unknown;
            char enableProxyAcct = Constants.No;
            string servername = string.Empty;
            string instancename = string.Empty;
            string casesensitivemode = string.Empty;
            char enabledRemoteAccess = Constants.Unknown;
            char enabledDAC = Constants.Unknown;
            char enabledAllowUpdates = Constants.Unknown;
            char enabledScanForStartupSP = Constants.Unknown;
            char isDefaultTraceEnabled = Constants.Unknown;
            char enabledSQLmailXPs = Constants.Unknown;
            char enabledDatabaseMailXPs = Constants.Unknown;
            char enabledOLEAutomationXPs = Constants.Unknown;
            char enabledWebAsstXPs = Constants.Unknown;
            char enabledXP_CMDshell = Constants.Unknown;
            char enabledAdHocDistributedQueries = Constants.Unknown;
            char isDomainControler = Constants.Unknown;
            char issaPasswordNull = Constants.Unknown;
            char isSysAdminOnlyCmdExec = Constants.Unknown;
            char isReplicationEnabled = Constants.Unknown;
            char isDistributor = Constants.Unknown;
            char isPublisher = Constants.Unknown;
            char hasRemotePublisher = Constants.Unknown;
            char isWeakPasswordDetectionEnabled = Constants.Unknown;
            char isClrEnabled = Constants.Unknown;
            string systemDrive = Constants.Unknown.ToString();

            using (logX.loggerX.DebugCall())
            {
                Program.ImpersonationContext wi = Program.SetTargetSQLServerImpersonationContext();
                using (SqlConnection target = new SqlConnection(ConnectionString))
                {
                    try
                    {
                        // Open the connection.
                        target.Open();

                        // authentication mode.
                        using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(target, null, CommandType.Text,
                                                                               QueryAuthenticationMode, null))
                        {
                            try
                            {
                                if (rdr.Read())
                                {
                                    authenticationMode = ((int)rdr[0]) == 1
                                                             ?
                                                                 Constants.WindowsAuthentication
                                                             : Constants.MixedAuthentication;
                                }
                                else
                                {
                                    logX.loggerX.Warn("WARN - failed to read auhtentication mode");
                                }
                            }
                            catch (SqlException ex)
                            {
                                logX.loggerX.Error("ERROR - reading authentication mode raised an exception", ex);
                                isOk = false;
                                enumStatus = Constants.CollectionStatus.StatusError;
                            }
                        }

                        // version
                        if (isOk)
                        {
                            using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(target, null, CommandType.Text,
                                                                                   QueryVersion, null))
                            {
                                if (rdr.Read())
                                {
                                    version = (string)rdr[0];
                                }
                                else
                                {
                                    logX.loggerX.Warn("WARN - failed to read version");
                                    enumStatus = Constants.CollectionStatus.StatusWarning;
                                }
                            }
                        }

                        // edition
                        if (isOk)
                        {
                            using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(target, null, CommandType.Text,
                                                                                   QueryEdition, null))
                            {
                                if (rdr.Read())
                                {
                                    edition = (string)rdr[0];
                                }
                                else
                                {
                                    logX.loggerX.Warn("WARN - failed to read edition");
                                    enumStatus = Constants.CollectionStatus.StatusWarning;
                                }
                            }
                        }

                        // Servername
                        if (isOk)
                        {
                            using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(target, null, CommandType.Text,
                                                                                   QueryServername, null))
                            {
                                if (rdr.Read())
                                {
                                    servername = (string)rdr[0];
                                }
                                else
                                {
                                    logX.loggerX.Warn("WARN - failed to read server name");
                                    enumStatus = Constants.CollectionStatus.StatusWarning;
                                }
                            }
                        }

                        // Instance name
                        if (isOk)
                        {
                            using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(target, null, CommandType.Text,
                                                                                   QueryInstancename, null))
                            {
                                if (rdr.HasRows && rdr.Read())
                                {
                                    System.Data.SqlTypes.SqlString insname = rdr.GetSqlString(0);
                                    instancename = insname.IsNull ? string.Empty : insname.Value;
                                }
                                else
                                {
                                    logX.loggerX.Warn("WARN - failed to read instance name");
                                    enumStatus = Constants.CollectionStatus.StatusWarning;
                                }
                            }
                        }

                        // Case sensitive
                        if (isOk)
                        {
                            using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(target, null, CommandType.Text,
                                                                                   QueryCollation, null))
                            {
                                if (rdr.Read())
                                {
                                    casesensitivemode = (string)rdr[0];
                                    casesensitivemode = casesensitivemode.Contains("_CS_") ? "Y" : "N";
                                }
                                else
                                {
                                    logX.loggerX.Warn("WARN - failed to read collation");
                                    enumStatus = Constants.CollectionStatus.StatusWarning;
                                }
                            }
                        }

                        // enableproxyaccount
                        if (m_VersionEnum != Sql.ServerVersion.SQL2000)
                        {
                            if (isOk)
                            {
                                using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(target, null, CommandType.Text,
                                                                                       QueryProxyEnabled, null))
                                {
                                    if (rdr.HasRows && rdr.Read())
                                    {
                                        System.Data.SqlTypes.SqlString insname = rdr.GetSqlString(0);
                                        if (!insname.IsNull)
                                        {
                                            enableProxyAcct = Constants.Yes;
                                        }
                                        else
                                        {
                                            logX.loggerX.Warn("WARN - failed to read enable proxy account");
                                            enumStatus = Constants.CollectionStatus.StatusWarning;
                                        }
                                    }
                                }
                            }
                        }

                        // Is sa account password NULL
                        if (isOk)
                        {
                            string query = string.Empty;
                            if (m_VersionEnum == Sql.ServerVersion.SQL2000)
                            {
                                query = QueryIssaPasswordNull2K;
                            }
                            else
                            {
                                query = QueryIssaPasswordNull2K5;
                            }
                            using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(target, null, CommandType.Text,
                                                                                       query, null))
                            {
                                if (rdr.Read())
                                {
                                    issaPasswordNull = (int)rdr[0] == 1 ? Constants.Yes : Constants.No;
                                }
                                else
                                {
                                    logX.loggerX.Warn("WARN - failed to read is sa account password NULL");
                                    enumStatus = Constants.CollectionStatus.StatusWarning;
                                }
                            }
                        }


                        // Is SysAdmin only allowed to execute cmdExec SQL Server Agent Jobs
                        if (isOk)
                        {
                            string query = string.Empty;
                            if (m_VersionEnum == Sql.ServerVersion.SQL2000)
                            {
                                query = QuerySysAdminOnlyForSQLAgentCmdExecJobs2k;
                            }
                            else
                            {
                                query = QuerySysAdminOnlyForSQLAgentCmdExecJobs2k5;
                            }
                            try
                            {
                                using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(target, null, CommandType.Text,
                                                                                    query, null))
                                {
                                    if (rdr.HasRows && rdr.Read())
                                    {
                                        isSysAdminOnlyCmdExec = Constants.No;
                                        if (m_VersionEnum == Sql.ServerVersion.SQL2000)
                                        {
                                            enableProxyAcct = Constants.Yes;
                                        }
                                    }
                                    else
                                    {
                                        isSysAdminOnlyCmdExec = Constants.Yes;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                logX.loggerX.Warn(string.Format("WARN - failed to read SysAdmin only allowed to execute cmdExec SQL Server Agent Jobs\n{0}", ex.Message));
                                enumStatus = Constants.CollectionStatus.StatusWarning;
                            }
                        }


                        if (isOk)
                        {
                            // Replication Enabled
                            string query = QueryReplicationEnabled;
                            try
                            {
                                using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(target, null, CommandType.Text,
                                                                                    query, null))
                                {
                                    isReplicationEnabled = Constants.No;
                                    while (rdr.Read())
                                    {
                                        // name | id | transpublish | mergepublish | dbowner | dbreadonly
                                        if (Convert.ToInt32(rdr[2]) == 1 || Convert.ToInt32(rdr[3]) == 1)
                                        {
                                            isReplicationEnabled = Constants.Yes;
                                            break;
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                logX.loggerX.Warn(string.Format("WARN - failed to read SysAdmin only allowed to execute cmdExec SQL Server Agent Jobs\n{0}", ex.Message));
                                enumStatus = Constants.CollectionStatus.StatusWarning;
                            }

                            //Is Server Distributor, isPublisher, HasRemotePublisher
                            try
                            {
                                using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(target, null, CommandType.Text, QueryDistributorEnabled, null))
                                {
                                    if (rdr.Read())
                                    {

                                        // installed  | distribution server | distribution db installed  | is distribution publisher  | has remote distribution publisher
                                        isDistributor = Convert.ToInt32(rdr[0]) == 1 ? Constants.Yes : Constants.No;
                                        isPublisher = Convert.ToInt32(rdr[3]) == 1 ? Constants.Yes : Constants.No;
                                        hasRemotePublisher = Convert.ToInt32(rdr[4]) == 1 ? Constants.Yes : Constants.No;

                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                logX.loggerX.Warn(string.Format("WARN - failed to read if server is distributor/publisher or has remote publisher.\n{0}", ex.Message));
                                enumStatus = Constants.CollectionStatus.StatusWarning;
                            }

                        }

                        // Read Security configuration information
                        if (m_VersionEnum == Sql.ServerVersion.SQL2000)
                        {
                            if (isOk)
                            {
                                using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(target, null, CommandType.Text,
                                                                                       QueryConfigurations2K, null))
                                {
                                    while (rdr.Read())
                                    {
                                        int id = Convert.ToInt32(rdr[0]);
                                        char value = ((int)rdr[1] == 1) ? Constants.Yes : Constants.No;
                                        switch (id)
                                        {
                                            case 102:
                                                enabledAllowUpdates = value;
                                                break;
                                            case 117:
                                                enabledRemoteAccess = value;
                                                break;
                                            case 400:
                                                crossDbOwnership = value;
                                                break;
                                            case 544:
                                                enableC2AuditTrace = value;
                                                break;
                                            case 1547:
                                                enabledScanForStartupSP = value;
                                                break;
                                            case 1568:
                                                isDefaultTraceEnabled = value;
                                                break;
                                            default:
                                                logX.loggerX.Warn("WARN - Read unknown configuration id: ", id);
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (isOk)
                            {
                                using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(target, null, CommandType.Text,
                                                                                       QueryConfigurations2K5, null))
                                {
                                    while (rdr.Read())
                                    {
                                        int id = (int)rdr[0];
                                        char value = ((int)rdr[1] == 1) ? Constants.Yes : Constants.No;
                                        switch (id)
                                        {
                                            case 102:
                                                enabledAllowUpdates = value;
                                                break;
                                            case 117:
                                                enabledRemoteAccess = value;
                                                break;
                                            case 400:
                                                crossDbOwnership = value;
                                                break;
                                            case 544:
                                                enableC2AuditTrace = value;
                                                break;
                                            case 1568:
                                                isDefaultTraceEnabled = value;
                                                break;
                                            case 1547:
                                                enabledScanForStartupSP = value;
                                                break;
                                            case 1576:
                                                enabledDAC = value;
                                                break;
                                            case 16385:
                                                enabledSQLmailXPs = value;
                                                break;
                                            case 16386:
                                                enabledDatabaseMailXPs = value;
                                                break;
                                            case 16388:
                                                enabledOLEAutomationXPs = value;
                                                break;
                                            case 16389:
                                                enabledWebAsstXPs = value;
                                                break;
                                            case 16390:
                                                enabledXP_CMDshell = value;
                                                break;
                                            case 16391:
                                                enabledAdHocDistributedQueries = value;
                                                break;
                                            case 1562:
                                                isClrEnabled = value;
                                                break;
                                            default:
                                                logX.loggerX.Warn("WARN - Read unknown configuration id: ", id);
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        logX.loggerX.Warn("WARN - exception raised when getting instance properties", ex);
                        enumStatus = Constants.CollectionStatus.StatusWarning;
                    }
                    Program.RestoreImpersonationContext(wi);
                }

                // Is Server Domain Controller
                if (isOk)
                {
                    systemDrive = m_Server.SystemDrive;
                }

                // Is Server Domain Controller
                if (isOk)
                {
                    isDomainControler = m_Server.IsDomainController == true
                                                           ? Constants.Yes
                                                           : Constants.No;
                }

                // Connect to the repository, create snapshot entry and get
                // the snapshotid.
                if (isOk)
                {
                    Program.ImpersonationContext ic = Program.SetLocalImpersonationContext();
                    using (SqlConnection repository = new SqlConnection(m_Repository.ConnectionString))
                    {
                        try
                        {
                            // Open the connection.
                            repository.Open();

                            List<WeakPasswordSetting> settings = WeakPasswordSetting.GetWeakPasswordSettings(repository);

                            if (settings.Count > 0)
                            {
                                isWeakPasswordDetectionEnabled = settings[0].PasswordCheckingEnabled ? Constants.Yes : Constants.No;
                            }

                            // Create a snapshot instance.
                            string instance = m_ConnectionStringBuilder.DataSource.Split(',')[0];
                            SqlParameter paramConnectionname =
                                new SqlParameter(ParamConnectionname, instance);
                            SqlParameter paramStarttime =
                                new SqlParameter(ParamStarttime, DateTime.Now.ToUniversalTime());
                            string os = m_Server.Product;
                            if (!string.IsNullOrEmpty(m_Server.ServicePack))
                            {
                                os += (", " + m_Server.ServicePack);
                            }
                            SqlParameter paramStatus = new SqlParameter(ParamStatus, Constants.StatusInProgress);
                            String collectorVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                            if (collectorVersion != null && collectorVersion.Equals("0.0.0.0"))
                            {
                                collectorVersion = Constants.SQLsecureCollectorVersion;
                            }
                            System.Globalization.CultureInfo culture =
                                new System.Globalization.CultureInfo("en-US", false);
                            string startTime = DateTime.Now.ToUniversalTime().ToString(culture);
                            string query = string.Format(NonQueryCreateSnapshot,
                                                    instance,
                                                    servername,
                                                    instancename,
                                                    startTime,
                                                    authenticationMode,
                                                    os,
                                                    version,
                                                    edition,
                                                    Constants.StatusInProgress,
                                // loginAuditMode,
                                                    crossDbOwnership,
                                                    enableC2AuditTrace,
                                                    enableProxyAcct,
                                                    casesensitivemode,
                                                    collectorVersion,
                                                    enabledAllowUpdates,
                                                    enabledDAC,
                                                    enabledRemoteAccess,
                                                    enabledScanForStartupSP,
                                                    enabledSQLmailXPs,
                                                    enabledDatabaseMailXPs,
                                                    enabledOLEAutomationXPs,
                                                    enabledWebAsstXPs,
                                                    enabledXP_CMDshell,
                                                    isDomainControler,
                                                    issaPasswordNull,
                                                    isSysAdminOnlyCmdExec,
                                                    isReplicationEnabled,
                                                    systemDrive,
                                                    enabledAdHocDistributedQueries,
                                                    isWeakPasswordDetectionEnabled,
                                                    isDistributor,
                                                    isPublisher,
                                                    hasRemotePublisher,
                                                    isClrEnabled,
                                                    isDefaultTraceEnabled);

                            Sql.SqlHelper.ExecuteNonQuery(repository, CommandType.Text, query);

                            // Query to get the snapshotid.
                            using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(repository, null,
                                                                                   CommandType.Text, QuerySnapshotId,
                                                                                   new SqlParameter[] { paramConnectionname }))
                            {
                                if (rdr.Read())
                                {
                                    snapshotid = (int)rdr[0];
                                }
                            }

                            // Now Create the snapshot histroy
                            // ------------------------------
                            SqlParameter paramSnapshotID = new SqlParameter(ParamSnapshotID, snapshotid);
                            SqlParameter paramStartTime =
                                new SqlParameter(ParamStartTime, DateTime.Now.ToUniversalTime());
                            SqlParameter paramNumErrors = new SqlParameter(ParamNumberofErrors, Convert.ToInt32(0));
                            Sql.SqlHelper.ExecuteNonQuery(repository, CommandType.Text,
                                                          NonQueryCreateSnapshotHistory,
                                                          new SqlParameter[]
                                                              {
                                                                  paramSnapshotID, paramStartTime,
                                                                  paramNumErrors, paramStatus
                                                              });
                        }
                        catch (SqlException ex)
                        {
                            logX.loggerX.Error("ERROR - exception raised when creating a snapshot entry", ex);
                            isOk = false;
                            enumStatus = Constants.CollectionStatus.StatusError;
                        }
                    }
                    Program.RestoreImpersonationContext(ic);
                }

                //get info about sql server jobs
                if (isOk)
                {
                    Program.ImpersonationContext ic = Program.SetLocalImpersonationContext();

                    try
                    {
                  
                        isOk = SqlJob.ProcessProxies(m_VersionEnum, ConnectionString,
                            m_Repository.ConnectionString, snapshotid, servername);

                    }
                    catch (SqlException ex)
                    {
                        logX.loggerX.Error("ERROR - exception raised when getting info about sql server jobs proxies", ex);
                        isOk = false;
                        enumStatus = Constants.CollectionStatus.StatusError;
                    }
                    Program.RestoreImpersonationContext(ic);
                }
                if (isOk)
                {
                    Program.ImpersonationContext ic = Program.SetLocalImpersonationContext();

                    try
                    {
                        isOk = SqlJob.Process(m_VersionEnum, ConnectionString,
                            m_Repository.ConnectionString, snapshotid, servername);
                    }
                    catch (SqlException ex)
                    {
                        logX.loggerX.Error("ERROR - exception raised when getting info about sql server jobs", ex);
                        isOk = false;
                        enumStatus = Constants.CollectionStatus.StatusError;
                    }
                    Program.RestoreImpersonationContext(ic);
                }
                if (isOk)
                {
                    Program.ImpersonationContext ic = Program.SetLocalImpersonationContext();

                    try
                    {
                        isOk = AvailabilityGroup.ProcessGroups(m_VersionEnum, ConnectionString,
                            m_Repository.ConnectionString, snapshotid, servername);
                    }
                    catch (SqlException ex)
                    {
                        logX.loggerX.Error("ERROR - exception raised when getting info about sql server availability groups", ex);
                        isOk = false;
                        enumStatus = Constants.CollectionStatus.StatusError;
                    }
                    Program.RestoreImpersonationContext(ic);
                }

            }

            if (!isOk)
            {
                enumStatus = Constants.CollectionStatus.StatusError;
            }

            return enumStatus;
        }



        private bool processServerObjects(
                int snapshotId,
                List<Sql.Filter.Rule> rules,
                out List<Account> users,
                out List<Account> windowsGroupLogins,
                ref Dictionary<Sql.SqlObjectType, Dictionary<MetricMeasureType, uint>> metricsData
            )
        {
            Debug.Assert(rules != null);
            // Init returns.
            bool isOk = true;
            users = null;
            windowsGroupLogins = null;
            using (logX.loggerX.DebugCall())
            {
                // Proces server level statement permissions, for SQL 2005.
                if (m_VersionEnum != Sql.ServerVersion.SQL2000)
                {
                    List<int> serveridlist = new List<int>();
                    serveridlist.Add(0);
                    if (!Sql.ServerPermission.Process(ConnectionString, m_Repository.ConnectionString,
                                                      snapshotId, Sql.SqlObjectType.Server,
                                                      serveridlist))
                    {
                        logX.loggerX.Error(
                            "ERROR - error encountered when processing server level statement permissions");
                        isOk = false;
                    }
                }

                // Process server principals, role members and if SQL 2005 then
                // server level permissions.
                if (isOk)
                {
                    if (
                        !Sql.ServerPrincipal.Process(m_VersionEnum, ConnectionString, m_Repository.ConnectionString,
                                                     snapshotId,
                                                     out users, out windowsGroupLogins))
                    {
                        logX.loggerX.Error("ERROR - error encountered when processing server principals");
                        isOk = false;
                    }
                }

                // Process endpoints only if the server version is 2005.
                if (isOk)
                {
                    if (m_VersionEnum != Sql.ServerVersion.SQL2000)
                    {
                        if (
                            !Sql.Endpoint.Process(ConnectionString, m_Repository.ConnectionString, snapshotId,
                                                  ref metricsData))
                        {
                            logX.loggerX.Error("ERROR - error encountered when processing endpoints");
                            isOk = false;
                        }
                    }
                }
            }
            return isOk;
        }

        private bool processDatabaseObjects(
                int snapshotId,
                List<Sql.Database> databases,
                Dictionary<string, Dictionary<int, List<Sql.Filter.Rule>>> databaseRules,
                List<Sql.Database> badDbs,
                ref Dictionary<Sql.SqlObjectType, Dictionary<MetricMeasureType, uint>> metricsData
            )
        {
            Debug.Assert(databases != null && databaseRules != null);
            using (logX.loggerX.DebugCall())
            {
                bool processDatabaseObjects = false;
                // If no databases to process, exit.
                if (databaseRules.Count == 0)
                {
                    logX.loggerX.Info("No databases to process");
                    return true;
                }

                // Process each database.
                foreach (Sql.Database db in databases)
                {
                    // If no database rules for the db, then process the next one.
                    bool isOk = true;
                    Dictionary<int, List<Sql.Filter.Rule>> dbObjRules = null;
                    processDatabaseObjects = true;
                    if (!databaseRules.TryGetValue(db.Name, out dbObjRules))
                    {
                        logX.loggerX.Info(db.Name, " not selected for a snapshot");
                        processDatabaseObjects = false;
                    }
                    else
                    {
                        logX.loggerX.Info(db.Name, " selected for a snapshot");
                    }

                    // Save the database to the repository.
                    // This order is important because there are forgein key constraints.
                    char isAudited = processDatabaseObjects == true ? Constants.Yes : Constants.No;
                    bool isDbWritten = Sql.Database.SaveToRepositorySqlDatabaseTable(m_Repository.ConnectionString,
                                                                                     snapshotId, db, isAudited);
                    if (!isDbWritten)
                    {
                        logX.loggerX.Error("ERROR - failed to update database ", db.Name,
                                           " to the repository sqldatabase table");
                        isOk = false;
                    }

                    // Process database principals, role members, schemas and their permissions.
                    // NOTE : principal & role permissions are only loaded for 2005.
                    if (isOk)
                    {
                        // Process database principals and role memberships.
                        bool isGuestEnabled = false;
                        if (!Sql.DatabasePrincipal.Process(m_VersionEnum, ConnectionString,
                                                           m_Repository.ConnectionString, snapshotId,
                                                           db, out isGuestEnabled, ref metricsData))
                        {
                            logX.loggerX.Error("ERROR - failed to process database principals and role members for ",
                                               db.Name);
                            isOk = false;
                        }

                        // Save database to the database object table.   This should be done after the
                        // database principals are saved otherwise it will create a constraint error.
                        if (isOk)
                        {
                            if (!Sql.Database.SaveToRepositoryDatabaseObjectTable(m_Repository.ConnectionString,
                                                                                  snapshotId, db))
                            {
                                logX.loggerX.Error("ERROR - failed to update database ", db.Name,
                                                   " to the repository databaseobject table");
                                isOk = false;
                            }
                        }

                        // If guest is enabled then update the database field in the repository.
                        if (isOk && isGuestEnabled)
                        {
                            db.IsGuestEnabled = isGuestEnabled;
                            if (!Sql.Database.UpdateRepositorySqlDatabaseGuestEnabled(m_Repository.ConnectionString,
                                                                                      snapshotId, db))
                            {
                                logX.loggerX.Error("ERROR - failed to update database ", db.Name,
                                                   " to the repository databaseobject table");
                                isOk = false;
                            }
                        }

                        // If this is SQL 2005, process all schemas.
                        if (isOk && m_VersionEnum != Sql.ServerVersion.SQL2000)
                        {
                            if (!Sql.Schema.Process(ConnectionString, m_Repository.ConnectionString,
                                                     snapshotId, db, ref metricsData))
                            {
                                logX.loggerX.Error("ERROR - error in processing schemas for db - ", db.Name);
                                isOk = false;
                            }
                        }
                    }

                    // Process database permissions.
                    if (isOk && processDatabaseObjects)
                    {
                        Sql.ObjIdCollection dbIdCollection = new Sql.ObjIdCollection();
                        dbIdCollection.Add(new Sql.ObjId(0, 0, 0));
                        if (!Sql.DatabaseObjectPermission.Process(m_VersionEnum, ConnectionString,
                                                                  m_Repository.ConnectionString, snapshotId,
                                                                  db, dbIdCollection))
                        {
                            logX.loggerX.Error("ERROR - failed to process database permissions for db- ", db.Name);
                            isOk = false;
                        }
                    }

                    // Force processing of Stored Procedures and Extended Stored Procedures
                    if (isOk)
                    {
                        List<Filter.Rule> rules = new List<Filter.Rule>();
                        Filter.Rule rule = new Filter.Rule(0, (int)SqlObjectType.StoredProcedure, "A", "");
                        rules.Add(rule);
                        if (!Sql.DatabaseObject.Process(m_VersionEnum, ConnectionString,
                                                        m_Repository.ConnectionString,
                                                        SqlObjectType.StoredProcedure, rules, snapshotId, db, ref metricsData))
                        {
                            logX.loggerX.Error("ERROR - failed to load db: ", db.Name, " ", SqlObjectType.StoredProcedure.ToString());
                            isOk = false;
                        }
                        rule = new Filter.Rule(1, (int)SqlObjectType.ExtendedStoredProcedure, "A", "");
                        rules.Add(rule);
                        if (!Sql.DatabaseObject.Process(m_VersionEnum, ConnectionString,
                                                        m_Repository.ConnectionString,
                                                        SqlObjectType.ExtendedStoredProcedure, rules, snapshotId, db, ref metricsData))
                        {
                            logX.loggerX.Error("ERROR - failed to load db: ", db.Name, " ", SqlObjectType.ExtendedStoredProcedure.ToString());
                            isOk = false;
                        }
                    }

                    // Process the various database objects, based on the selections in the filter
                    // rule.
                    if (isOk && processDatabaseObjects)
                    {
                        foreach (KeyValuePair<int, List<Sql.Filter.Rule>> kvp in dbObjRules)
                        {
                            Sql.SqlObjectType oType = (Sql.SqlObjectType)kvp.Key;//
                            if (oType == SqlObjectType.StoredProcedure || oType == SqlObjectType.ExtendedStoredProcedure)
                            {
                                // Processed above for all Databases.
                                continue;
                            }
                            List<Sql.Filter.Rule> rules = kvp.Value;
                            if (!Sql.DatabaseObject.Process(m_VersionEnum, ConnectionString,
                                                            m_Repository.ConnectionString,
                                                            oType, rules, snapshotId, db, ref metricsData))
                            {
                                logX.loggerX.Error("ERROR - failed to load db: ", db.Name, " ", oType.ToString());
                                isOk = false;
                                break;
                            }
                        }
                    }

                    // If database was written and there is any failure,
                    // then get the database status and write to the repository.
                    if (isDbWritten && !isOk)
                    {
                        // Get status and update db object.
                        string status = string.Empty;
                        Sql.Database.GetDabaseStatus(m_VersionEnum, ConnectionString, m_Repository.ConnectionString,
                                                     snapshotId, db.DbId, out status);
                        db.IsAvailable = false;
                        db.Status = status;

                        // Write the status field to the repository.
                        Sql.Database.UpdateRepositorySqlDatabaseStatus(m_Repository.ConnectionString, snapshotId, db);

                        // Add to the bad db list.
                        badDbs.Add(db);
                    }

                    // If any error was encountered, log the error
                    // and delete any database data stored in the repository.
                    if (!isOk)
                    {
                        // Delete any db data that was stored in the repository.
                        Sql.Database.RemoveDatabaseData(m_Repository.ConnectionString, snapshotId, db.DbId);
                    }
                }
            }
            return true;
        }

        private bool processFilters(int snapshotId)
        {
            return Sql.CollectionFilter.SaveSnapshotFilterRules(m_Repository.ConnectionString, snapshotId, m_FilterList);
        }

        private int loadDomainInformation(
                int snapshotid,
                bool isOSData,
                List<Account> users,
                List<Account> groupList,
                out List<string> wellKnownGroups
            )
        {
            Debug.Assert(users != null);
            Debug.Assert(groupList != null);
            int numWarnings = 0;
            Program.ImpersonationContext wi = Program.SetTargetImpersonationContext();
            using (logX.loggerX.DebugCall())
            {
                wellKnownGroups = null;
                Dictionary<Account, List<Account>> groupMemberships = new Dictionary<Account, List<Account>>();

                // No groups, bail out.
                if (groupList.Count == 0)
                {
                    logX.loggerX.Info("No group logins to process memberships");
                }
                else
                {
                    // Load group memberships from AD/SAM.
                    numWarnings = m_Server.ResolveGroupMembers(groupList, out groupMemberships, out wellKnownGroups);
                    if (logX.loggerX.IsVerboseEnabled)
                    {
                        logX.loggerX.Verbose(
                            "=======================================================================================");
                        foreach (KeyValuePair<Account, List<Account>> kvp in groupMemberships)
                        {
                            logX.loggerX.Verbose("Group Information - ");
                            logX.loggerX.Verbose("Group: ", kvp.Key.SamPath, "(SID : ", kvp.Key.SID.ToString(), ")");
                            foreach (Account mem in kvp.Value)
                            {
                                logX.loggerX.Verbose("    Acct: ", mem.SamPath, ", SID: ", mem.SID.ToString(),
                                                     ", Class: ",
                                                     mem.Class.ToString());
                            }
                        }
                        logX.loggerX.Verbose(
                            "=======================================================================================");
                    }
                    else
                    {
                        logX.loggerX.Info(string.Format("Processed {0} groups", groupList.Count));
                        logX.loggerX.Info(string.Format("Processed {0} user logins", users.Count));
                    }
                }

                if (users.Count == 0)
                {
                    logX.loggerX.Info("No user logins to process");
                }
                else
                {
                    if (users.Count > 0)
                    {
                        if (m_Server.Bind())
                        {
                            for (int x = 0; x < users.Count; x++)
                            {
                                Account ac = users[x];
                                if (!ac.IsValidSid(m_Server.Name))
                                {
                                    ac.AccountStatus = Account.AccountStatusEnum.Account_Suspected;
                                    users[x] = ac;
                                }
                            }
                            m_Server.Unbind();
                        }
                        else
                        {
                            for (int x = 0; x < users.Count; x++)
                            {
                                Account ac = users[x];
                                ac.AccountStatus = Account.AccountStatusEnum.Account_Suspected;
                                users[x] = ac;
                            }
                            numWarnings++;
                        }
                    }
                }

                if (isOSData)
                {
                    // Save group membership information to the repository.
                    if (!Sql.WindowsAccount.ProcessOSObjects(m_Repository.ConnectionString, snapshotid, users, groupMemberships))
                    {
                        logX.loggerX.Error(
                            "WARNING - error encountered when loading OS group membership information in the repository");
                        numWarnings++;
                    }

                }
                else
                {
                    // Save group membership information to the repository.
                    if (!Sql.WindowsAccount.Process(m_Repository.ConnectionString, snapshotid, users, groupMemberships))
                    {
                        logX.loggerX.Error(
                            "WARNING - error encountered when loading group membership information in the repository");
                        numWarnings++;
                    }
                }
            }
            Program.RestoreImpersonationContext(wi);
            return numWarnings;
        }

        public string BuildMetricsString(
            Dictionary<Sql.SqlObjectType, Dictionary<MetricMeasureType, uint>> metricsData
            )
        {
            StringBuilder strMetrics = new StringBuilder();
            string strTotal = ("Processed {0} objects in {1} msec");
            string strTemplateMultiple = "{0} {1}s in {2} msec";
            string strTemplateSingle = "{0} {1} in {2} msec";
            uint totalTime = 0;

            foreach (KeyValuePair<Sql.SqlObjectType, Dictionary<MetricMeasureType, uint>> kvp in metricsData)
            {
                string strType = Sql.SQLTypes.GetTextFromType(kvp.Key);
                uint Count = kvp.Value[MetricMeasureType.Count];
                uint Time = kvp.Value[MetricMeasureType.Time];
                if (!string.IsNullOrEmpty(strType) && Count > 0)
                {
                    numDatabaseObjectsCollected += Count;
                    totalTime += Time;
                    strMetrics.Append(", ");
                    if (Count == 1)
                    {
                        strMetrics.Append(string.Format(strTemplateSingle, Count, strType, Time));
                    }
                    else
                    {
                        strMetrics.Append(string.Format(strTemplateMultiple, Count, strType, Time));
                    }
                }
            }

            return (string.Format(strTotal, numDatabaseObjectsCollected, totalTime) + strMetrics.ToString());
        }

        public bool LoadData(bool bAutomatedRun)
        {
            char snapshotStatus = Constants.StatusSuccess;
            string strErrorMessage = string.Empty;
            string strWarnMessage = string.Empty;
            string strNewMessage = string.Empty;
            string strProgressFmt = "In Progress - completed steps {0} of {1}";
            int nStep = 0;
            const int nTotalSteps = 7;
            bool isOk = true;
            using (logX.loggerX.DebugCall())
            {
                // If any existing snapshot are left in "in progress" state set them to error.
                UpdateOldSnapshotStatus();

                // metricsData is a dictionary of database objects
                // containing a dictionary of MetricMeasureType (count, time) for the object
                // -------------------------------------------------------------------------
                Dictionary<Sql.SqlObjectType, Dictionary<MetricMeasureType, uint>> metricsData =
                    new Dictionary<Idera.SQLsecure.Collector.Sql.SqlObjectType, Dictionary<MetricMeasureType, uint>>();


                // If not valid target object return false.
                if (!IsValid)
                {
                    logX.loggerX.Error("ERROR - target object is invalid, data will not be loaded");
                    return false;
                }

                // Get instance level properties, and create a snapshot.
                Stopwatch sw = new Stopwatch();
                sw.Start();

                Constants.CollectionStatus status = createSnapshot(out m_snapshotId);
                if (status == Constants.CollectionStatus.StatusError)
                {
                    strErrorMessage = "Failed to create snapshot entry in the repository";
                    logX.loggerX.Error(strErrorMessage);
                    isOk = false;
                    snapshotStatus = Constants.StatusError;
                }
                else if (status == Constants.CollectionStatus.StatusWarning)
                {
                    strNewMessage = "Failed to load some configuration options for target SQL Server";
                    PostActivityMessage(ref strWarnMessage, strNewMessage, Collector.Constants.ActivityType_Warning);
                    snapshotStatus = Constants.StatusWarning;
                }

                // Check if we read OS information successfull, & issue warning if we didn't
                // -------------------------------------------------------------------------
                if (m_Server.NumWarnings > 0)
                {
                    strNewMessage = "Failed to load some OS settings";
                    PostActivityMessage(ref strWarnMessage, strNewMessage, Collector.Constants.ActivityType_Warning);
                    snapshotStatus = Constants.StatusWarning;
                }


                // Update Application and Event Log with start status
                // -------------------------------------------------
                string strBeginMessage = " SQL Server = " +
                                         m_ConnectionStringBuilder.DataSource + "; Snapshot ID = " + m_snapshotId;
                AppLog.WriteAppEventInfo(SQLsecureEvent.DlInfoStartMsg, SQLsecureCat.DlStartCat, DateTime.Now.ToString(),
                                         strBeginMessage);
                Sql.Database.CreateApplicationActivityEventInRepository(m_Repository.ConnectionString,
                                                                        m_snapshotId,
                                                                        Collector.Constants.ActivityType_Info,
                                                                        Collector.Constants.ActivityEvent_Start,
                                                                        "Collecting snapshot ID " +
                                                                        m_snapshotId.ToString());

                sw.Stop();
                if (isOk)
                {
                    Sql.Database.UpdateRepositorySnapshotProgress(m_Repository.ConnectionString, m_snapshotId,
                                                                  string.Format(strProgressFmt, ++nStep, nTotalSteps));
                    logX.loggerX.Verbose("TIMING - Time to create snapshot = " + sw.ElapsedMilliseconds.ToString() +
                                         " msec");
                }

                if (isOk)
                {
                    // Load SQLServer Settings from Registry of Target Server
                    if (registryPermissions == null)
                    {
                        registryPermissions =
                            new RegistryPermissions(m_snapshotId, m_Server.Name,
                                                    Idera.SQLsecure.Core.Accounts.Path.GetInstanceFromSQLServerInstance(TargetInstance),
                                                    m_VersionEnum);
                        int numWarnings = registryPermissions.LoadRegistrySettings();
                        if (numWarnings > 0)
                        {
                            //isOk = false;
                            strNewMessage = "Failed to load some registry configuration options for target SQL Server";
                            PostActivityMessage(ref strWarnMessage, strNewMessage, Collector.Constants.ActivityType_Warning);
                            snapshotStatus = Constants.StatusWarning;
                        }
                        //else
                        {
                            registryPermissions.WriteRegistrySettingsToRepository(m_Repository.ConnectionString);
                        }
                    }
                }

                // Get SQLServer Services from Target
                if (isOk)
                {
                    string instanceName = Path.GetInstanceFromSQLServerInstance(TargetInstance);
                    string computerName = Path.WhackPrefixComputer(m_Server.Name);
                    sqlServices = new SQLServices(computerName, instanceName, m_VersionEnum);
                    if (sqlServices.GetSQLServices(m_Repository.ConnectionString, m_snapshotId) != 0)
                    {
                        // Don't abort if GetSQLServices Fails
                        strNewMessage = "Failed to load properties for some SQL Services on target SQL Server";
                        PostActivityMessage(ref strWarnMessage, strNewMessage, Collector.Constants.ActivityType_Warning);
                        snapshotStatus = Constants.StatusWarning;
                    }
                }

                // Load Registry Permissions
                if (isOk)
                {
                    if (registryPermissions != null)
                    {
                        if (registryPermissions.ProcessRegistryPermissions(sqlServices.Services) != 0)
                        {
                            strNewMessage = "Failed to load registry permissions for target SQL Server";
                            PostActivityMessage(ref strWarnMessage, strNewMessage, Collector.Constants.ActivityType_Warning);
                            snapshotStatus = Constants.StatusWarning;
                        }
                        registryPermissions.WriteRegistryPermissionToRepository(m_Repository.ConnectionString, 0);
                    }
                }

                // Load File Permissions
                if (isOk)
                {
                    if (filePermissions == null)
                    {
                        filePermissions = new FilePermissions(m_snapshotId, m_Server.Name, m_VersionEnum);
                    }
                    if (filePermissions.LoadFilePermissionsForInstallationDirectory(registryPermissions.InstallPath) != 0)
                    {
                        strNewMessage = "Failed to load file permissions for target SQL Server";
                        PostActivityMessage(ref strWarnMessage, strNewMessage, Collector.Constants.ActivityType_Warning);
                        snapshotStatus = Constants.StatusWarning;
                    }
                    if (filePermissions.LoadFilePermissionForServices(sqlServices.Services) != 0)
                    {
                        strNewMessage = "Failed to load file permissions for SQL Services on target SQL Server";
                        PostActivityMessage(ref strWarnMessage, strNewMessage, Collector.Constants.ActivityType_Warning);
                        snapshotStatus = Constants.StatusWarning;
                    }
                }

                // Get a list of databases, from the target SQL Server.
                sw.Reset();
                sw.Start();
                List<Sql.Database> databases = null;
                if (isOk)
                {
                    if (
                        !Sql.Database.GetTargetDatabases(m_Server, m_VersionEnum,
                                                         m_ConnectionStringBuilder.ConnectionString,
                                                         m_Repository.ConnectionString, m_snapshotId,
                                                         m_ConnectionStringBuilder.UserID, out databases,
                                                         ref metricsData))
                    {
                        strNewMessage = "Failed to get a list of databases from the target SQL Server";
                        isOk = false;
                        PostActivityMessage(ref strErrorMessage, strNewMessage, Collector.Constants.ActivityType_Error);
                        snapshotStatus = Constants.StatusError;
                    }
                }

                sw.Stop();
                if (isOk)
                {
                    Sql.Database.UpdateRepositorySnapshotProgress(m_Repository.ConnectionString, m_snapshotId,
                                                                  string.Format(strProgressFmt, ++nStep, nTotalSteps));
                    logX.loggerX.Verbose("TIMING - Time to Get all databases = " + sw.ElapsedMilliseconds.ToString() +
                                         " msec");
                }

                // Process Database File Permissions
                if (isOk)
                {
                    if (filePermissions.GetDatabaseFilePermissions(databases) != 0)
                    {
                        strNewMessage = "Failed to load some Database File permissions for target SQL Server";
                        PostActivityMessage(ref strWarnMessage, strNewMessage, Collector.Constants.ActivityType_Warning);
                        snapshotStatus = Constants.StatusWarning;
                    }

                    filePermissions.WriteFilePermissionToRepository(m_Repository.ConnectionString,
                                                                        registryPermissions.NumOSObjectsWrittenToRepository);
                }
                if (isOk)
                {
                    List<Account> users = new List<Account>();
                    List<Account> groups = new List<Account>();
                    List<string> wellKnownAccounts = null;
                    int numWarn = filePermissions.GetUsersAndGroups(ref users, ref groups);
                    numWarn += registryPermissions.GetUsersAndGroups(ref users, ref groups);
                    if (loadDomainInformation(m_snapshotId, true, users, groups, out wellKnownAccounts) != 0 || numWarn != 0)
                    {
                        UpdateSuspectAccounts(true);
                        strNewMessage = "Suspect Windows accounts encountered processing OS objects";
                        PostActivityMessage(ref strWarnMessage, strNewMessage, Collector.Constants.ActivityType_Warning);
                        snapshotStatus = Constants.StatusWarning;
                    }
                    // Sql.Database.SaveWellKnownGroups(m_Repository.ConnectionString, m_snapshotId, wellKnownAccounts);

                }

                // Optimize the filters.
                sw.Reset();
                sw.Start();
                List<Sql.Filter.Rule> serverObjectRules = null;
                Dictionary<string, Dictionary<int, List<Sql.Filter.Rule>>> databaseRules = null;
                if (isOk)
                {
                    if (
                        !Sql.CollectionFilter.OptimizeRules(databases, m_FilterList, out serverObjectRules,
                                                            out databaseRules))
                    {
                        strNewMessage = "Unable to optimize the filter rules";
                        PostActivityMessage(ref strErrorMessage, strNewMessage, Collector.Constants.ActivityType_Error);
                        isOk = false;
                        snapshotStatus = Constants.StatusError;
                    }
                }
                sw.Stop();
                if (isOk)
                {
                    Sql.Database.UpdateRepositorySnapshotProgress(m_Repository.ConnectionString, m_snapshotId,
                                                                  string.Format(strProgressFmt, ++nStep, nTotalSteps));
                    logX.loggerX.Verbose("TIMING - Time to optimize filters = " + sw.ElapsedMilliseconds.ToString() +
                                         " msec");
                }
                // Start loading the data.
                if (isOk)
                {
                    // Process server level objects.
                    List<Account> users = null;
                    List<Account> windowsGroupLogins = null;
                    List<string> wellKnownAccounts = null;
                    sw.Reset();
                    sw.Start();
                    isOk =
                        processServerObjects(m_snapshotId, serverObjectRules, out users, out windowsGroupLogins,
                                             ref metricsData);
                    if (!isOk)
                    {
                        strNewMessage = "Failed to process server objects";
                        PostActivityMessage(ref strErrorMessage, strNewMessage, Collector.Constants.ActivityType_Error);
                        snapshotStatus = Constants.StatusError;
                    }
                    sw.Stop();
                    Sql.Database.UpdateRepositorySnapshotProgress(m_Repository.ConnectionString, m_snapshotId,
                                                                  string.Format(strProgressFmt, ++nStep, nTotalSteps));
                    logX.loggerX.Verbose("TIMING - Time to Process Server Objects = " +
                                         sw.ElapsedMilliseconds.ToString() + " msec");

                    // Load group memberships.
                    sw.Reset();
                    sw.Start();
                    if (isOk)
                    {
                        if (loadDomainInformation(m_snapshotId, false, users, windowsGroupLogins, out wellKnownAccounts) != 0)
                        {
                            UpdateSuspectAccounts(false);
                            strNewMessage = "Suspect Windows accounts encountered processing SQL Server logins";
                            PostActivityMessage(ref strWarnMessage, strNewMessage, Collector.Constants.ActivityType_Warning);
                            snapshotStatus = Constants.StatusWarning;
                        }
                        Sql.Database.SaveWellKnownGroups(m_Repository.ConnectionString, m_snapshotId, wellKnownAccounts);
                    }
                    sw.Stop();
                    Sql.Database.UpdateRepositorySnapshotProgress(m_Repository.ConnectionString, m_snapshotId,
                                                                  string.Format(strProgressFmt, ++nStep, nTotalSteps));
                    logX.loggerX.Verbose("TIMING - Time to Load Group Memberships = " +
                                         sw.ElapsedMilliseconds.ToString() + " msec");

                    // Process database level objects.
                    sw.Reset();
                    sw.Start();
                    if (isOk)
                    {
                        List<Sql.Database> badDbs = new List<Sql.Database>();
                        processDatabaseObjects(m_snapshotId, databases, databaseRules, badDbs, ref metricsData);
                        if (badDbs.Count > 0)
                        {
                            // Note: the warn message is appended if it has account warn message.
                            strNewMessage = "Some databases were unavailable for auditing";
                            PostActivityMessage(ref strWarnMessage, strNewMessage, Collector.Constants.ActivityType_Warning);
                            snapshotStatus = Constants.StatusWarning;
                        }
                    }
                    sw.Stop();
                    Sql.Database.UpdateRepositorySnapshotProgress(m_Repository.ConnectionString, m_snapshotId,
                                                                  string.Format(strProgressFmt, ++nStep, nTotalSteps));
                    logX.loggerX.Verbose("TIMING - Time to Process Database Objects = " +
                                         sw.ElapsedMilliseconds.ToString() + " msec");

                    // Save the snapshot filters being used to the repository.
                    sw.Reset();
                    sw.Start();
                    if (isOk)
                    {
                        isOk = processFilters(m_snapshotId);
                        if (!isOk)
                        {
                            strNewMessage = "Failed to save filters to repository";
                            PostActivityMessage(ref strErrorMessage, strNewMessage, Collector.Constants.ActivityType_Error);
                            snapshotStatus = Constants.StatusError;
                        }
                    }
                    sw.Stop();
                    Sql.Database.UpdateRepositorySnapshotProgress(m_Repository.ConnectionString, m_snapshotId,
                                                                  string.Format(strProgressFmt, ++nStep, nTotalSteps));
                    logX.loggerX.Verbose("TIMING - Time to Save Filters = " + sw.ElapsedMilliseconds.ToString() +
                                         " msec");
                }

                int numErrorsAndWarnings = 0;
                string strDoneStatus = null;
                if (isOk)
                {
                    strDoneStatus = BuildMetricsString(metricsData);
                }
                else
                {
                    numErrorsAndWarnings = 1;
                    strDoneStatus = "Error collecting snapshot";
                }

                // Update Application and Event Log with done status
                // -------------------------------------------------
                string msg = string.Empty;
                if (snapshotStatus == Constants.StatusSuccess)
                {
                    msg = "OK";
                }
                else if (snapshotStatus == Constants.StatusWarning)
                {
                    msg = strWarnMessage;
                }
                else if (snapshotStatus == Constants.StatusError)
                {
                    msg = string.IsNullOrEmpty(strErrorMessage) ? lastErrorMsg : strErrorMessage;
                }
                Sql.Database.UpdateRepositorySnapshotHistory(m_Repository.ConnectionString, m_snapshotId, bAutomatedRun,
                                                             snapshotStatus, numErrorsAndWarnings, msg);
                string strActivityType = Constants.ActivityType_Info;
                if (snapshotStatus == Constants.StatusError)
                {
                    strActivityType = Constants.ActivityType_Error;
                }

                // Write to System Application Event Log
                AppLog.WriteAppEventInfo(SQLsecureEvent.DlInfoEndMsg, SQLsecureCat.DlEndCat,
                                         DateTime.Now.ToString() +
                                         " SQL Server = " + m_ConnectionStringBuilder.DataSource +
                                         " Snapshot ID = " + m_snapshotId.ToString() + "; " +
                                         strDoneStatus);

                // Write to SQLSecure Activity Log
                Sql.Database.CreateApplicationActivityEventInRepository(m_Repository.ConnectionString, m_snapshotId,
                                                                        strActivityType,
                                                                        Collector.Constants.ActivityEvent_Metrics,
                                                                        "Snapshot ID = " + m_snapshotId.ToString() +
                                                                        "; " + strDoneStatus);

                // Update the RegisteredServer Table if snapshot was created successfully
                // ----------------------------------------------------------------------
                Sql.Database.UpdateRepositoryRegisteredServerTable(m_Repository.ConnectionString, m_snapshotId,
                                                                   snapshotStatus);

                // Process Notifications
                // ---------------------
                Program.ImpersonationContext ic = Program.SetLocalImpersonationContext();

                Notification notification = new Notification(WriteAppActivityToRepository,
                                                       m_ConnectionStringBuilder.DataSource.Split(',')[0],
                                                       m_Repository.ConnectionString,
                                                       SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry(),
                                                       m_Repository.RegisteredServerId);
                notification.ProcessStatusNotification(snapshotStatus,
                                                       msg,
                                                       strDoneStatus);
                if (snapshotStatus != Constants.StatusError)
                {
                    notification.ProcessFindingNotification();
                }
                Program.RestoreImpersonationContext(ic);
            }

            return isOk;
        }



        private void UpdateOldSnapshotStatus()
        {
            Program.ImpersonationContext ic = Program.SetLocalImpersonationContext();
            string sql = string.Format("exec SQLsecure.dbo.isp_sqlsecure_updatesnapshotstatus '{0}'", TargetInstance);
            try
            {
                using (SqlConnection connection = new SqlConnection(m_Repository.ConnectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    cmd.CommandTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error("ERROR - Updating existing In Progress snapshots:  ", ex.Message);
            }
            Program.RestoreImpersonationContext(ic);
        }

        public void UpdateSuspectAccounts(bool useOSTables)
        {
            Program.ImpersonationContext ic = Program.SetLocalImpersonationContext();

            string sql = string.Empty;
            if (useOSTables)
            {
                sql = String.Format("exec SQLsecure.dbo.isp_sqlsecure_processsuspectaccountsos {0}",
                              m_snapshotId);
            }
            else
            {
                sql = String.Format("exec SQLsecure.dbo.isp_sqlsecure_processsuspectaccounts {0}",
                              m_snapshotId);
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(m_Repository.ConnectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    cmd.CommandTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error("ERROR - Updating Suspect Accounts ", ex.Message);
            }

            Program.RestoreImpersonationContext(ic);
        }


        public void CreateApplicationActivityEventInRepository(string activityType,
                                                               string eventcode,
                                                               string description)
        {
            Sql.Database.CreateApplicationActivityEventInRepository(m_Repository.ConnectionString,
                                                                    m_snapshotId,
                                                                    activityType,
                                                                    eventcode,
                                                                    description);
        }

        private void PostActivityMessage(ref string strErrorMessage, string newMessage, string activityType)
        {
            if (strErrorMessage.Length > 0)
            {
                strErrorMessage = strErrorMessage + " and " + newMessage;
            }
            else
            {
                strErrorMessage = newMessage;
            }
            if (activityType == Collector.Constants.ActivityType_Error)
            {
                logX.loggerX.Error(string.Format("{0} - {1}", activityType, newMessage));
                AppLog.WriteAppEventError(SQLsecureEvent.DlErrGeneralMsg, SQLsecureCat.DlDataLoadCat,
                                          DateTime.Now.ToString() +
                                          " SQL Server = " + m_ConnectionStringBuilder.DataSource +
                                          " Snapshot ID = " + m_snapshotId.ToString() + "; " + newMessage);
            }
            else if (activityType == Collector.Constants.ActivityType_Warning)
            {
                logX.loggerX.Warn(string.Format("{0} - {1}", activityType, newMessage));
                AppLog.WriteAppEventWarning(SQLsecureEvent.DlErrGeneralMsg, SQLsecureCat.DlDataLoadCat,
                                          DateTime.Now.ToString() +
                                          " SQL Server = " + m_ConnectionStringBuilder.DataSource +
                                          " Snapshot ID = " + m_snapshotId.ToString() + "; " + newMessage);
            }
            else
            {
                logX.loggerX.Info(string.Format("{0} - {1}", activityType, newMessage));
            }

            WriteAppActivityToRepository(activityType,
                                         activityType,
                                         newMessage);

        }

        #endregion
    }

}
