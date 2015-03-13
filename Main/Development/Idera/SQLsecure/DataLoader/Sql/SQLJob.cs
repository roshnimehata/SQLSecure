using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Idera.SQLsecure.Core.Logger;

namespace Idera.SQLsecure.Collector.Sql
{
    public class SqlJob
    {
        private static LogX logX = new LogX("Idera.SQLsecure.Collector.Sql.SqlJob");

        public static string GetJobsQuery = "SELECT  sj.name ," +
                                            " sj.owner_sid ," +
                                            " sj.enabled ," +
                                            " sj.description ," +
                                            " st.last_run_date ," +
                                            " st.command ," +
                                            " st.step_name, " +
                                            " st.subsystem" +
                                            " FROM    msdb.dbo.sysjobs sj" +
                                            " JOIN [msdb].[sys].[servers] sv ON sj.originating_server_id = sv.server_id" +
                                            " JOIN msdb.dbo.sysjobsteps st ON sj.job_id = st.job_id" +
                                            " WHERE sv.name='{0}'";

        public static string GetJobsQuerySQL2000 = "SELECT  sj.name ," +
                                            " sj.owner_sid ," +
                                            " sj.enabled ," +
                                            " sj.description ," +
                                            " st.last_run_date ," +
                                            " st.command ," +
                                            " st.step_name, " +
                                            " st.subsystem" +
                                            " FROM    msdb.dbo.sysjobs sj" +
                                            " JOIN msdb.dbo.sysjobsteps st ON sj.job_id = st.job_id";

        public static string GetProxiesQuery = "SELECT  sp.proxy_id proxyId ," +
                                             "sp.name proxyName," +
                                             "sp.credential_id credentialId," +
                                             "enabled enabled," +
                                             "user_sid usersid," +
                                             "ss.subsystem_id subsystemid," +
                                             "subsystem subsystem, " +
                                             "sc.name credentialName ," +
                                             "credential_identity credentialIdentity " +
                                             " FROM msdb.dbo.sysproxies sp " +
                                             " JOIN msdb.dbo.sysproxysubsystem ss ON sp.proxy_id = ss.proxy_id " +
                                             " JOIN msdb.dbo.syssubsystems sb ON ss.subsystem_id = sb.subsystem_id " +
                                             " JOIN sys.credentials sc ON sc.credential_id=sp.credential_id";

        public static string GetProxiesQuerySQL2000 = "SELECT  NULL proxyId ," +
                                             "NULL proxyName," +
                                             "NULL credentialId," +
                                             "NULL enabled," +
                                             "NULL usersid," +
                                             "NULL subsystemid," +
                                             "NULL subsystem, " +
                                             "NULL credentialName ," +
                                             "NULL credentialIdentity " +
                                             " WHERE 1 = 2 ";

        public static string InsertJobsQuery = "INSERT INTO sqlsecure.dbo.sqljob" +
                                               "  ([Name]" +
                                               "  ,[Desciprion]" +
                                               "  ,[Step]" +
                                               "  ,[LastRunDate]" +
                                               "  ,[Command]" +
                                               "  ,[SubSystem]" +
                                               "  ,[Ownersid]" +
                                               "  ,[Enabled]" +
                                               "  ,[SnapshotId])" +
                                               "  VALUES" +
                                               " (@Name" +
                                               " ,@Desciprion" +
                                               " ,@Step" +
                                               " ,@LastRunDate" +
                                               " ,@Command" +
                                               " ,@SubSystem" +
                                               " ,@Ownersid" +
                                               " ,@Enabled" +
                                               ",@SnapshotId)";


        internal const int ColName = 0;
        internal const int ColOwnerSid = 1;
        internal const int ColEnabled = 2;
        internal const int ColDescription = 3;
        internal const int ColLastRunDate = 4;
        internal const int ColCommand = 5;
        internal const int ColStepName = 6;
        internal const int ColSubSystem = 7;


        public static bool Process(ServerVersion version,
            string targetConnection,
            string repositoryConnection,
            int snapshotid,
            string server)
        {
            Debug.Assert(version != ServerVersion.Unsupported);
            Debug.Assert(!string.IsNullOrEmpty(targetConnection));
            Debug.Assert(!string.IsNullOrEmpty(repositoryConnection));

            bool isOk = true;
            targetConnection = Sql.SqlHelper.AppendDatabaseToConnectionString(targetConnection, "msdb");
            Program.ImpersonationContext wi = Program.SetLocalImpersonationContext();
            using (SqlConnection target = new SqlConnection(targetConnection),
                repository = new SqlConnection(repositoryConnection))
            {
                try
                {
                    // Open repository and target connections.
                    repository.Open();
                    Program.SetTargetSQLServerImpersonationContext();
                    target.Open();

                    // Use bulk copy object to write to repository.
                    using (SqlBulkCopy bcp = new SqlBulkCopy(repository))
                    {
                        // Set the destination table.
                        bcp.DestinationTableName = SqlJobDataTable.RepositoryTable;
                        bcp.BulkCopyTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                        // Create the datatable to write to the repository.
                        using (DataTable dataTable = SqlJobDataTable.Create())
                        {
                            // Process each rule to collect the table objects.

                            string query = string.Format(SqlJob.GetJobsQuery, server);
                            if (version == ServerVersion.SQL2000)  
                            {
                                query = SqlJob.GetJobsQuerySQL2000;
                            }

                            Debug.Assert(!string.IsNullOrEmpty(query));

                            // Query to get the table objects.
                            using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(target, null, CommandType.Text, query, null))
                            {
                                while (rdr.Read())
                                {
                                    // Retrieve the object information.
                                    
                                    SqlString name = rdr.GetSqlString(ColName);
                                    SqlBinary owner = rdr.GetSqlBinary(ColOwnerSid);
                                    SqlInt16 enabled = rdr.GetByte(ColEnabled);

                                    SqlString desc = rdr.GetSqlString(ColDescription);
                                    SqlInt32 lastRunInt = rdr.GetInt32(ColLastRunDate);
                                    SqlDateTime lastRun = SqlDateTime.MinValue;
                                    if (lastRunInt != 0)
                                    {
                                        lastRun = DateTime.ParseExact(lastRunInt.ToString(), "yyyyMMdd", null);
                                    }
                                    SqlString command = rdr.GetSqlString(ColCommand);
                                    SqlString stepname = rdr.GetSqlString(ColStepName);
                                    SqlString subsystem = rdr.GetSqlString(ColSubSystem);

                                    DataRow dr = dataTable.NewRow();
                                    dr[SqlJobDataTable.ParamSnapshotId] = snapshotid;
                                    dr[SqlJobDataTable.ParamName] = name;
                                    dr[SqlJobDataTable.ParamDescription] = desc;
                                    dr[SqlJobDataTable.ParamStepName] = stepname;
                                    dr[SqlJobDataTable.ParamLastRunDate] = lastRun;
                                    dr[SqlJobDataTable.ParamCommand] = command;
                                    dr[SqlJobDataTable.ParamSubSystem] = subsystem;
                                    dr[SqlJobDataTable.ParamOwnerSid] = owner;
                                    dr[SqlJobDataTable.ParamEnabled] = enabled;

                                    dataTable.Rows.Add(dr);
                                    if (dataTable.Rows.Count > Constants.RowBatchSize)
                                    {
                                        try
                                        {
                                            bcp.WriteToServer(dataTable);
                                            dataTable.Clear();
                                        }
                                        catch (SqlException ex)
                                        {
                                            string strMessage = "Writing to Repository sql server jobs faild";

                                            logX.loggerX.Error("ERROR - " + strMessage, ex);
                                            throw;
                                        }
                                    }
                                }

                                // Write any items still in the data table.
                                if (dataTable.Rows.Count > 0)
                                {
                                    try
                                    {
                                        bcp.WriteToServer(dataTable);
                                        dataTable.Clear();
                                    }
                                    catch (SqlException ex)
                                    {
                                        string strMessage = "Writing to Repository sql server jobs failed";
                                        logX.loggerX.Error("ERROR - " + strMessage, ex);
                                        throw;
                                    }
                                }
                            }


                        }
                    }
                }
                catch (SqlException ex)
                {
                    string strMessage = "Processing sql server jobs failed";
                    logX.loggerX.Error("ERROR - " + strMessage, ex);
                    Sql.Database.CreateApplicationActivityEventInRepository(repositoryConnection,
                        snapshotid,
                        Collector.Constants.ActivityType_Error,
                        Collector.Constants.ActivityEvent_Error,
                        strMessage + ex.Message);
                    AppLog.WriteAppEventError(SQLsecureEvent.ExErrExceptionRaised, SQLsecureCat.DlDataLoadCat,
                        " SQL Server = " + new SqlConnectionStringBuilder(targetConnection).DataSource +
                        strMessage, ex.Message);
                    isOk = false;
                }
                finally
                {
                    Program.RestoreImpersonationContext(wi);
                }

            }

            return isOk;
        }

        public static bool ProcessProxies(ServerVersion version,
            string targetConnection,
            string repositoryConnection,
            int snapshotid,
            string server)
        {
            Debug.Assert(version != ServerVersion.Unsupported);  
            Debug.Assert(!string.IsNullOrEmpty(targetConnection));
            Debug.Assert(!string.IsNullOrEmpty(repositoryConnection));

            bool isOk = true;
            targetConnection = Sql.SqlHelper.AppendDatabaseToConnectionString(targetConnection, "msdb");
            Program.ImpersonationContext wi = Program.SetLocalImpersonationContext();

            using (SqlConnection target = new SqlConnection(targetConnection),
                repository = new SqlConnection(repositoryConnection))
            {
                try
                {
                    // Open repository and target connections.
                    repository.Open();
                    Program.SetTargetSQLServerImpersonationContext();
                    target.Open();

                    // Use bulk copy object to write to repository.
                    using (SqlBulkCopy bcp = new SqlBulkCopy(repository))
                    {
                        // Set the destination table.
                        bcp.DestinationTableName = SQLJobProxy.RepositoryTable;
                        bcp.BulkCopyTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();

                        // Create the datatable to write to the repository.
                        using (DataTable dataTable = SQLJobProxy.Create())
                        {

                            string query = SqlJob.GetProxiesQuery;

                            if (version == ServerVersion.SQL2000)  
                            {
                                query = SqlJob.GetProxiesQuerySQL2000;
                            }
                            Debug.Assert(!string.IsNullOrEmpty(query));

                            // Query to get the table objects.
                            using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(target, null, CommandType.Text, query, null))
                            {
                                while (rdr.Read())
                                {
                                    // Retrieve the object information.

                                    DataRow dr = dataTable.NewRow();

                                    dr[SQLJobProxy.ParamSnapshotId] = snapshotid;
                                    dr[SQLJobProxy.ParamName] = rdr.GetSqlString(SQLJobProxy.ColName);
                                    dr[SQLJobProxy.ParamEnabled] = rdr.GetByte(SQLJobProxy.ColEnabled);
                                    dr[SQLJobProxy.ParamUserSid] = rdr.GetSqlBinary(SQLJobProxy.ColUserSid);
                                    dr[SQLJobProxy.ParamSubSystemId] = rdr.GetInt32(SQLJobProxy.ColSubSystemId);
                                    dr[SQLJobProxy.ParamSubSystem] = rdr.GetSqlString(SQLJobProxy.ColSubSystem);
                                    dr[SQLJobProxy.ParamCredentialId] = rdr.GetInt32(SQLJobProxy.ColCredentialId);
                                    dr[SQLJobProxy.ParamCredentialName] = rdr.GetSqlString(SQLJobProxy.ColCredentialName);
                                    dr[SQLJobProxy.ParamCredentialIdentity] = rdr.GetSqlString(SQLJobProxy.ColCredentialIdentity);

                                    dataTable.Rows.Add(dr);
                                    if (dataTable.Rows.Count > Constants.RowBatchSize)
                                    {
                                        try
                                        {
                                            bcp.WriteToServer(dataTable);
                                            dataTable.Clear();
                                        }
                                        catch (SqlException ex)
                                        {
                                            string strMessage = "Writing to Repository sql job proxies  failed";

                                            logX.loggerX.Error("ERROR - " + strMessage, ex);
                                            throw;
                                        }
                                    }
                                }

                                // Write any items still in the data table.
                                if (dataTable.Rows.Count > 0)
                                {
                                    try
                                    {
                                        bcp.WriteToServer(dataTable);
                                        dataTable.Clear();
                                    }
                                    catch (SqlException ex)
                                    {
                                        string strMessage = "Writing to Repository sql job proxies failed";
                                        logX.loggerX.Error("ERROR - " + strMessage, ex);
                                        throw;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    string strMessage = "Processing sql job proxies  failed";  
                    logX.loggerX.Error("ERROR - " + strMessage, ex);
                    Sql.Database.CreateApplicationActivityEventInRepository(repositoryConnection,
                        snapshotid,
                        Collector.Constants.ActivityType_Error,
                        Collector.Constants.ActivityEvent_Error,
                        strMessage + ex.Message);
                    AppLog.WriteAppEventError(SQLsecureEvent.ExErrExceptionRaised, SQLsecureCat.DlDataLoadCat,
                        " SQL Server = " + new SqlConnectionStringBuilder(targetConnection).DataSource +
                        strMessage, ex.Message);
                    isOk = false;
                }
                finally
                {
                    Program.RestoreImpersonationContext(wi);
                }

            }
            return isOk;
        }



        public SqlJob()
        {

        }


    }
}
