
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using Idera.SQLsecure.Core.Logger;

namespace Idera.SQLsecure.Collector.Sql
{

    internal static class LinkedServerPrincipal
    {
        private static LogX logX = new LogX("Idera.SQLsecure.Collector.Sql.LinkedServerPrincipal");

        private const int FieldServerId = 0;
        private const string FieldPrincipalName = "name";

        private static string CreateQuery(string linkedServerName, ServerVersion serverVersion)
        {

            const string Sql2000Query = @"SELECT DISTINCT
                                                ISNULL(ll.rmtloginame, sp.name) AS [login],
                                                srv.name AS servername INTO #linklogins
                                        FROM sysservers AS srv
                                        INNER JOIN sysoledbusers ll
                                                ON ll.rmtsrvid = CAST(srv.srvid AS int)
                                        LEFT OUTER JOIN syslogins sp
                                                ON ll.loginsid = sp.sid
                                        WHERE ISNULL(ll.rmtloginame, sp.name) IS NOT NULL
                                        SELECT
                                                l.name
                                        FROM [{0}].master.dbo.syslogins AS l
                                        WHERE (l.sysadmin = 1
                                        OR l.securityadmin = 1)
                                        AND EXISTS (SELECT
                                                1
                                        FROM #linklogins
                                        WHERE [login] COLLATE SQL_Latin1_General_CP1_CI_AI = l.name COLLATE SQL_Latin1_General_CP1_CI_AI
                                        AND [servername] COLLATE SQL_Latin1_General_CP1_CI_AI = '{0}')
                                        IF OBJECT_ID('tempdb..#linklogins') IS NOT NULL
                                                DROP TABLE #linklogins;";

            const string SqlQuery = @";WITH LinkedLogins([login], [servername])
                                    AS
                                    (
                                        SELECT
                                            DISTINCT ISNULL(ll.remote_name, sp.name),
                                            srv.name
                                        FROM
                                        sys.servers AS srv
                                        INNER JOIN sys.linked_logins ll ON ll.server_id=CAST(srv.server_id AS int)
                                        LEFT OUTER JOIN sys.server_principals sp ON ll.local_principal_id = sp.principal_id
                                        WHERE ISNULL(ll.remote_name, sp.name) is not null
                                    )

                                    SELECT l.name
                                    FROM [{0}].master.sys.server_principals AS l 
                                    WHERE 
                                        (IS_SRVROLEMEMBER('sysadmin', name) = 1 OR IS_SRVROLEMEMBER('securityadmin', name) = 1) AND
                                        EXISTS(SELECT 1 FROM LinkedLogins WHERE [login] COLLATE SQL_Latin1_General_CP1_CI_AI  = l.name COLLATE SQL_Latin1_General_CP1_CI_AI  
                                        AND [servername] COLLATE SQL_Latin1_General_CP1_CI_AI = '{0}' )";
            return string.Format(serverVersion < ServerVersion.SQL2005 ? Sql2000Query : SqlQuery, linkedServerName);
        }

        public static bool Process(
                string targetConnection,
                string repositoryConnection,
                int snapshotid,
                string serverName,
                int serverId,
                ref Dictionary<Sql.SqlObjectType, Dictionary<MetricMeasureType, uint>> metricsData
            )
        {
            Debugger.Launch();
            Debug.Assert(!string.IsNullOrEmpty(targetConnection));
            Debug.Assert(!string.IsNullOrEmpty(repositoryConnection));
            Stopwatch sw = new Stopwatch();
            sw.Start();
            uint numProcessedLinkedServers = 0;

            // Init return.
            bool isOk = true;
            Program.ImpersonationContext wi = Program.SetTargetImpersonationContext();
            
            // Process linked server principals.
            List<int> serverList = new List<int>();
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
                        bcp.DestinationTableName = LinkedServerPrincipalDataTable.RepositoryTable;
                        bcp.BulkCopyTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                        // Create the datatable to write to the repository.
                        using (DataTable dataTable = LinkedServerPrincipalDataTable.Create())
                        {
                            var sqlServerVersion = Sql.SqlHelper.ParseVersion(target.ServerVersion);
                            // Create the query.
                            string query = CreateQuery(serverName, sqlServerVersion);
                            Debug.Assert(!string.IsNullOrEmpty(query));
                            var linkedServerUsersCount = 0;
                            // Query to get the table objects.
                            using (SqlDataReader rdr = SqlHelper.ExecuteReader(target, null,
                                                CommandType.Text, query, null))
                            {
                                while (rdr.Read())
                                {
                                    // Retrieve information.
                                    SqlString principalName = rdr.GetSqlString(rdr.GetOrdinal(FieldPrincipalName));

                                    // Update the datatable.
                                    DataRow dr = dataTable.NewRow();
                                    dr[LinkedServerPrincipalDataTable.ParamSnapshotId] = snapshotid;
                                    dr[LinkedServerPrincipalDataTable.ParamServerId] = serverId;
                                    dr[LinkedServerPrincipalDataTable.ParamPrincipalName] = principalName;
                                    dataTable.Rows.Add(dr);
                                    linkedServerUsersCount++;
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
                            if (linkedServerUsersCount == 0) logX.loggerX.InfoFormat("No admin users found for linked server {0}", serverName);
                        }
                    }
                }
                catch (Exception ex)
                {

                    string strMessage = "Processing linked server principals";
                    logX.loggerX.Error("ERROR - " + strMessage, ex);
                    isOk = false;
                }
                finally
                {
                    Program.RestoreImpersonationContext(wi);
                }
            }
           
            uint oldMetricCount = 0;
            uint oldMetricTime = 0;
            // See if User is already in Endpoint Dictionary
            // ----------------------------------------------
            Dictionary<MetricMeasureType, uint> de;
            if (metricsData.TryGetValue(SqlObjectType.LinkedServerPrincipals, out de))
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
            metricsData[SqlObjectType.LinkedServerPrincipals] = de;

            return isOk;
        }
       
    }
}
