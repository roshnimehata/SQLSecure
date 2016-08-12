using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using Idera.SQLsecure.Core.Logger;

namespace Idera.SQLsecure.Collector.Sql
{

    internal static class LinkedServer
    {
        private static LogX logX = new LogX("Idera.SQLsecure.Collector.Sql.LinkedServer");

        private const int FieldServerId = 0;
        private const int FieldName = 1;


        private static string CreateQuery(ServerVersion serverVersion)
        {
            const string  Sql2000Query = @"select server_id, name from sys.sysservers where isremote = 0";
            const string SqlQuery = @"select server_id, name from sys.servers where is_linked = 1";
            
            return serverVersion < ServerVersion.SQL2005 ? Sql2000Query : SqlQuery;
        }

        public static bool Process(
                string targetConnection,
                string repositoryConnection,
                int snapshotid,
                ref Dictionary<Sql.SqlObjectType, Dictionary<MetricMeasureType, uint>> metricsData
            )
        {
            Debugger.Break();
            Debug.Assert(!string.IsNullOrEmpty(targetConnection));
            Debug.Assert(!string.IsNullOrEmpty(repositoryConnection));
            Stopwatch sw = new Stopwatch();
            sw.Start();
            uint numProcessedLinkedServers = 0;

            // Init return.
            bool isOk = true;
            Program.ImpersonationContext wi = Program.SetLocalImpersonationContext();
            // Process endpoints.
            Dictionary<int, string> linkedServers = new Dictionary<int, string>();
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
                        bcp.DestinationTableName = LinkedServersDataTable.RepositoryTable;
                        bcp.BulkCopyTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                        // Create the datatable to write to the repository.
                        using (DataTable dataTable = LinkedServersDataTable.Create())
                        {
                            var sqlServerVersion = Sql.SqlHelper.ParseVersion(target.ServerVersion);
                            // Create the query.
                            string query = CreateQuery(sqlServerVersion);
                            Debug.Assert(!string.IsNullOrEmpty(query));

                            // Query to get the table objects.
                            using (SqlDataReader rdr = SqlHelper.ExecuteReader(target, null,
                                                CommandType.Text, query, null))
                            {
                                while (rdr.Read())
                                {
                                    // Retrieve information.
                                    SqlInt32 serverId = rdr.GetInt32(FieldServerId);
                                    SqlString serverName = rdr.GetSqlString(FieldName);

                                    // Add linked server to the list for logins roles processing.
                                    linkedServers.Add(serverId.Value, serverName.Value);

                                    // Update the datatable.
                                    DataRow dr = dataTable.NewRow();
                                    dr[LinkedServersDataTable.ParamSnapshotId] = snapshotid;
                                    dr[LinkedServersDataTable.ParamServerId] = serverId;
                                    dr[LinkedServersDataTable.ParamServerName] = serverName;
                                    dataTable.Rows.Add(dr);

                                    numProcessedLinkedServers++;
                                    // Write to repository if exceeds threshold.
                                    if (dataTable.Rows.Count > Constants.RowBatchSize)
                                    {
                                        bcp.WriteToServer(dataTable);
                                        dataTable.Clear();
                                    }
                                }

                                // Write any items still in the data table.
                                if (dataTable.Rows.Count > 0)
                                {
                                    bcp.WriteToServer(dataTable);
                                    dataTable.Clear();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string strMessage = "Processing linked servers";
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
            if (isOk)
            {
                foreach (var server in linkedServers)
                {
                    // Load linked servers users.
                    if (!LinkedServerPrincipal.Process(targetConnection, repositoryConnection, snapshotid, server.Value, server.Key, ref metricsData))
                    {
                        logX.loggerX.Error("ERROR - error encountered in processing linked servers");

                    }
                }
            }

            uint oldMetricCount = 0;
            uint oldMetricTime = 0;
            // See if User is already in Endpoint Dictionary
            // ----------------------------------------------
            Dictionary<MetricMeasureType, uint> de;
            if (metricsData.TryGetValue(SqlObjectType.LinkedServer, out de))
            {
                de.TryGetValue(MetricMeasureType.Count, out oldMetricCount);
                de.TryGetValue(MetricMeasureType.Time, out oldMetricTime);
            }
            else
            {
                de = new Dictionary<MetricMeasureType, uint>();
            }
            de[MetricMeasureType.Count] = numProcessedLinkedServers + oldMetricCount;
            de[MetricMeasureType.Time] = (uint)sw.ElapsedMilliseconds + oldMetricTime;
            metricsData[SqlObjectType.LinkedServer] = de;

            return isOk;
        }
    }
}
