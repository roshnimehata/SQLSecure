/******************************************************************
 * Name: Repository.cs
 *
 * Description: Encapsulates the SQLsecure Repository.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.Core.Accounts;

namespace Idera.SQLsecure.Collector
{
    /// <summary>
    /// Encapsulates SQLsecure Repository connection.
    /// </summary>
    internal class Repository
    {
        public const int ServerGraceCount = 2;
        #region Fields
        private SqlConnectionStringBuilder m_ConnectionStringBuilder;
        private Sql.ServerVersion m_VersionEnum;
        private bool m_IsValid;
        private int m_NumRegisteredServers;
        private int m_RegisteredServerId;
        private static LogX logX = new LogX("Idera.SQLsecure.Collector.Repository");
        #endregion

        #region Queries
        private const string QueryCheckSQLsecureDbExists = 
                   @"SELECT 1 FROM master.dbo.sysdatabases WHERE name='SQLsecure'";
        private const string QueryGetSchemaDALVersion =
                    @"SELECT dalversion, schemaversion FROM SQLsecure.dbo.currentversion";
        private const string QueryGetSQLsecurePermissions =
                    @"EXEC SQLsecure.dbo.isp_sqlsecure_getuserapplicationrole";

        private const string QueryIsTargetRegistered = 
                    @"SELECT 1 FROM SQLsecure.dbo.registeredserver WHERE connectionname = @connectionname";
        private const string QueryGetTargetServerInfo =
                    @"SELECT registeredserverid, servername, sqlserverlogin, sqlserverpassword, sqlserverauthtype, serverlogin, serverpassword, connectionport FROM SQLsecure.dbo.registeredserver WHERE connectionname = @connectionname";
        private const string QueryGetTargetServerInfoParam = "connectionname";

        private const string QueryGetCountTargetServers = @"SELECT COUNT(*) from SQLsecure.dbo.registeredserver";
        #endregion

        #region Helpers
        private bool isRepositoryValid()
        {
            using (logX.loggerX.DebugCall())
            {
                using (SqlConnection connection = new SqlConnection(m_ConnectionStringBuilder.ConnectionString))
                {
                    // Open connection to the repository SQL Server.  Make sure we are connected
                    // to SQL Server 2000 or 2005.
                    try
                    {
                        // Open the connection.
                        connection.Open();

                        // Check the SQL Server version.
                        m_VersionEnum = Sql.SqlHelper.ParseVersion(connection.ServerVersion);
                        if (m_VersionEnum == Sql.ServerVersion.Unsupported)
                        {
                            logX.loggerX.Error("ERROR - Repository SQL Server version: ", connection.ServerVersion,
                                                    " is not supported.");
                            AppLog.WriteAppEventError(SQLsecureEvent.DlErrInvalidRepositoryVersionMsg,
                                                        SQLsecureCat.DlValidationCat, 
                                                            m_ConnectionStringBuilder.DataSource, 
                                                                connection.ServerVersion);
                            return false;
                        }

                        // Check if SQLsecure database is in the repository.
                        logX.loggerX.Info("Checking for SQLsecure database in the repository");
                        using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                        QueryCheckSQLsecureDbExists, null))
                        {
                            if (!rdr.HasRows) // No rows found, no SQLsecure database
                            {
                                logX.loggerX.Error("ERROR - SQLsecure database not found.");
                                AppLog.WriteAppEventError(SQLsecureEvent.DlErrSQLsecureDbNotFound, 
                                                                SQLsecureCat.DlValidationCat, 
                                                                    m_ConnectionStringBuilder.DataSource);
                                return false;
                            }
                        }

                        // Check schema/DAL versions.
                        logX.loggerX.Info("Checking SQLsecure schema/DAL versions");
                        using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                        QueryGetSchemaDALVersion, null))
                        {
                            // This table has only one row.
                            int schemaVer = 0, dalVer = 0;
                            if (rdr.Read())
                            {
                                // Get DAL & schema versions.
                                dalVer = (int)rdr[0];
                                schemaVer = (int)rdr[1];
                            }
                            // If versions don't match, bail out.
                            if (dalVer != Constants.DalVersion || schemaVer != Constants.SchemaVersion)
                            {
                                logX.loggerX.Error("ERROR - DAL: ", dalVer, " Schema: ", schemaVer, " is not supported.");
                                AppLog.WriteAppEventError(SQLsecureEvent.DlErrSchemaVerNotCompatible, SQLsecureCat.DlValidationCat,
                                                        dalVer.ToString(), schemaVer.ToString(),
                                                            Constants.DalVersion.ToString(), Constants.SchemaVersion.ToString());
                                return false;
                            }
                        }

                        // Check permissions in the repository.
                        logX.loggerX.Info("Checking for permissions to update Repository");
                        using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                            QueryGetSQLsecurePermissions, null))
                        {
                            // Check for permissions, the result set has only one column.
                            bool havePermissions = false;
                            while (rdr.Read() && !havePermissions)
                            {
                                string role = (string)rdr[0];
                                havePermissions = String.Compare(role, Constants.AdminRole, true) == 0
                                                        || String.Compare(role, Constants.LoaderRole, true) == 0;
                            }

                            // If no permission, bail out.
                            if (!havePermissions)
                            {
                                logX.loggerX.Error("ERROR - Data Loader does not have permissions to load data in SQLsecure database.");
                                AppLog.WriteAppEventError(SQLsecureEvent.DlErrNoSQLsecurePermissions, SQLsecureCat.DlValidationCat);
                                return false;
                            }
                        }

                        using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                    QueryGetCountTargetServers, null))
                        {
                            if (rdr.Read())
                            {
                                m_NumRegisteredServers = (int)rdr[0];
                            }
                            else
                            {
                                logX.loggerX.Warn("WARN - failed to read server name");
                            }
                        }

                    }
                    catch (SqlException ex)
                    {
                        logX.loggerX.Error("ERROR - exception raised when validating the repository.", ex);
                        AppLog.WriteAppEventError(SQLsecureEvent.ExErrExceptionRaised, SQLsecureCat.DlValidationCat,
                                                    "Repository Validation", ex.Message);
                        return false;
                    }
                }

                return true;
            }
        }
        #endregion

        #region Ctors
        public Repository (
                string instance,
                string user,
                string password
            )
       {
            m_ConnectionStringBuilder = Sql.SqlHelper.ConstructConnectionString(instance, user, password);
            m_IsValid = isRepositoryValid();
        }  
        #endregion
        
        #region Properties
        public bool IsValid
        {
            get { return m_IsValid; }
        }

        public int RegisteredServerId
        {
            get { return m_RegisteredServerId; }
        }

        /// <summary>
        /// Get the repository connection string.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                Debug.Assert(IsValid);
                return m_ConnectionStringBuilder.ConnectionString;
            }
        }
        #endregion
        
        #region Methods
        public bool IsLicenseOk()
        {
            bool isOK = false;
            Core.Accounts.BBSProductLicense bbsProductLicense = null;
            using (logX.loggerX.DebugCall())
            {
                Debug.Assert(IsValid);
                logX.loggerX.Info("Checking License");
                bbsProductLicense = new BBSProductLicense(ConnectionString, m_ConnectionStringBuilder.DataSource.Split(',')[0],
                                                                Constants.SQLsecureProductID, Constants.SQLsecureLicenseProductVersionStr);
                bbsProductLicense.IsProductLicensed();
            }
            isOK = (bbsProductLicense.CombinedLicense.licState == BBSProductLicense.LicenseState.Valid);
            if (isOK)
            {
                if (bbsProductLicense.CombinedLicense.numLicensedServers == -1)
                {
                    // Unlimited licensed servers
                    isOK = true;
                }
                else if (m_NumRegisteredServers > bbsProductLicense.CombinedLicense.numLicensedServers + ServerGraceCount)
                {
                    isOK = false;
                }
            }

            return isOK;
        }
            

        public bool IsTargetRegistered(string targetInstance)
        {
            using (logX.loggerX.DebugCall())
            {
                Debug.Assert(IsValid);

                using (SqlConnection connection = new SqlConnection(m_ConnectionStringBuilder.ConnectionString))
                {
                    try
                    {
                        // Open connection to the repository SQL Server.
                        connection.Open();

                        // See if the target instance is registered.
                        SqlParameter param = new SqlParameter(QueryGetTargetServerInfoParam, targetInstance);
                        using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                        QueryIsTargetRegistered, new SqlParameter[] { param }))
                        {
                            // Read only 1 row.
                            if (!rdr.HasRows)
                            {
                                logX.loggerX.Error("ERROR - ", targetInstance, " is not registered in SQLsecure");
                                AppLog.WriteAppEventError(SQLsecureEvent.DlErrTargetNotRegistered, SQLsecureCat.DlValidationCat,
                                                                targetInstance);
                                return false;
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        logX.loggerX.Error("ERROR - failed to check if target instance is registered.", ex);
                        AppLog.WriteAppEventError(SQLsecureEvent.ExErrExceptionRaised, SQLsecureCat.DlValidationCat,
                                                    "Checking target instance is registered", ex.Message);
                        return false;
                    }
                }

                return true;
            }
        }

        public bool GetTargetCredentials(
                string targetInstance,
                out string server,
                out int? port,
                out string sqlLogin,
                out string sqlPassword,
                out string sqlAuthType,
                out string srvrLogin,
                out string srvrPassword
            )
        {
            using (logX.loggerX.DebugCall())
            {
                Debug.Assert(IsValid);
                Program.ImpersonationContext wi = Program.SetLocalImpersonationContext();
                // Init returns.
                server = null;
                port = null;
                sqlLogin = null;
                sqlPassword = null;
                sqlAuthType = null;
                srvrLogin = null;
                srvrPassword = null;

                using (SqlConnection connection = new SqlConnection(m_ConnectionStringBuilder.ConnectionString))
                {
                    // Open connection to the repository SQL Server.
                    try
                    {
                        // Open the connection.
                        connection.Open();

                        // Retrieve target credential information.
                        logX.loggerX.Info("Retrieve target credentials");
                        SqlParameter param = new SqlParameter(QueryGetTargetServerInfoParam, targetInstance);
                        using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                        QueryGetTargetServerInfo, new SqlParameter[] { param }))
                        {
                            // Read only 1 row.
                            if (rdr.Read())
                            {
                                m_RegisteredServerId = (int) rdr[0];
                                server = (string)rdr[1];
                                sqlLogin = (string)rdr[2];
                                sqlPassword = Encryptor.Decrypt((string)rdr[3]);
                                sqlAuthType = (string)rdr[4];
                                srvrLogin = (string)rdr[5];
                                srvrPassword = Encryptor.Decrypt((string)rdr[6]);
                                port = rdr[7] == DBNull.Value ? null : (int?)rdr[7];
                            }
                            else
                            {
                                logX.loggerX.Error("ERROR - ", targetInstance, " is not registered in SQLsecure");
                                AppLog.WriteAppEventError(SQLsecureEvent.DlErrTargetNotRegistered, SQLsecureCat.DlValidationCat,
                                                                targetInstance);
                                return false;
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        logX.loggerX.Error("ERROR - exception raised when retrieving target credentials", ex);
                        AppLog.WriteAppEventError(SQLsecureEvent.ExErrExceptionRaised, SQLsecureCat.DlValidationCat,
                                                    "Retrieve target credentials", ex.Message);
                        return false;
                    }
                    finally
                    {
                        Program.RestoreImpersonationContext(wi);
                    }
                }

                return true;
            }
        }

        public bool GetCollectionFilters(
                string targetInstance,
                out List<Sql.Filter> filterList
            )
        {
            // Init returns.
            bool isOk = true;
            filterList = null;
            Program.ImpersonationContext wi = Program.SetLocalImpersonationContext();
            // Validate input.
            if (string.IsNullOrEmpty(targetInstance))
            {
                logX.loggerX.Error("ERROR - invalid target instance specified to collect filter rules");
                isOk = false;
            }

            // Collect filter rules.
            if (isOk)
            {
                if (!Sql.CollectionFilter.GetFilterRules(m_ConnectionStringBuilder.ConnectionString, targetInstance, out filterList))
                {
                    logX.loggerX.Error("ERROR - failed to retrieve data collection filter rules");
                    isOk = false;
                }
            }

            Program.RestoreImpersonationContext(wi);

            return isOk;
        }

        private const string NonQueryCreateErrorSnapshot =
                    @"INSERT INTO SQLsecure.dbo.serversnapshot 
                            (connectionname, servername, instancename, authenticationmode,
                             starttime, endtime, status, snapshotcomment)
                      VALUES (@connectionname, @servername, @instancename, @authenticationmode,
                              @starttime, @endtime, @status, @snapshotcomment)";
        private const string ParamConnectionname = "connectionname";
        private const string ParamServerName = "servername";
        private const string ParamInstanceName = "instancename";
        private const string ParamAuthenticationmode = "authenticationmode";
        private const string ParamStatus = "status";
        private const string ParamStartTime = "starttime";
        private const string ParamEndTime = "endtime";
        private const string ParamSnapshotComment = "snapshotcomment";

        public int CreateErrorSnapshot(string serverInstance)
        {
            int snapshotID = 0;
            string server = serverInstance;
            string instance = string.Empty;
            if (serverInstance.Contains("\\"))
            {
                int nPos = serverInstance.IndexOf("\\");
                server = serverInstance.Substring(0, nPos);
                instance = serverInstance.Substring(nPos + 1);            
            }

            using (SqlConnection repository = new SqlConnection(ConnectionString))
            {
                try
                {
                    // Open the connection.
                    repository.Open();

                    // Create a snapshot instance.
                    SqlParameter paramConnectionname = new SqlParameter(ParamConnectionname, serverInstance);
                    SqlParameter paramServerName = new SqlParameter(ParamServerName, server);
                    SqlParameter paramInstanceName = new SqlParameter(ParamInstanceName, instance);
                    SqlParameter paramStarttime = new SqlParameter(ParamStartTime, DateTime.Now.ToUniversalTime());
                    SqlParameter paramEndtime = new SqlParameter(ParamEndTime, DateTime.Now.ToUniversalTime());
                    SqlParameter paramStatus = new SqlParameter(ParamStatus, Constants.StatusError);
                    SqlParameter paramAuthenticationmode = new SqlParameter(ParamAuthenticationmode, 'm');
                    SqlParameter paramSnapshotComment = new SqlParameter(ParamSnapshotComment, Target.lastErrorMsg);

                    Sql.SqlHelper.ExecuteNonQuery(repository, CommandType.Text,
                                    NonQueryCreateErrorSnapshot, new SqlParameter[] { paramConnectionname, 
                                            paramServerName, paramInstanceName, paramAuthenticationmode, paramStarttime, 
                                            paramEndtime, paramStatus, paramSnapshotComment });

                    // Query to get the snapshotid.
                    using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(repository, null,
                                    CommandType.Text, Target.QuerySnapshotId, new SqlParameter[] { paramConnectionname }))
                    {
                        if (rdr.Read())
                        {
                            snapshotID = (int)rdr[0];
                        }
                    }

                    // Now Create the snapshot histroy
                    // ------------------------------
                    SqlParameter paramSnapshotID = new SqlParameter(Target.ParamSnapshotID, snapshotID);
                    SqlParameter paramStartTime = new SqlParameter(ParamStartTime, DateTime.Now.ToUniversalTime());
                    SqlParameter paramNumErrors = new SqlParameter(Target.ParamNumberofErrors, Convert.ToInt32(1));
                    Sql.SqlHelper.ExecuteNonQuery(repository, CommandType.Text,
                                    Target.NonQueryCreateSnapshotHistory,
                                    new SqlParameter[] 
                                            { paramSnapshotID, paramStartTime, 
                                              paramNumErrors, paramStatus });



                }
                catch (SqlException ex)
                {
                    logX.loggerX.Error("ERROR - exception raised when creating a snapshot entry", ex);
                    snapshotID = 0;
                }
            }
            return snapshotID;
        }
        #endregion

    }
}
