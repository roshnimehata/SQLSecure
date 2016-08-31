/******************************************************************
 * Name: Endpoint.cs
 *
 * Description: Encapsulates SQL Server Endpoint objects.
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
using System.Data.SqlTypes;
using System.Diagnostics;
using Idera.SQLsecure.Core.Logger;

namespace Idera.SQLsecure.Collector.Sql
{
    /// <summary>
    /// 
    /// </summary>
    internal static class Endpoint
    {
        private static LogX logX = new LogX("Idera.SQLsecure.Collector.Sql.Endpoint");

        private const int FieldName = 0;
        private const int FieldId = 1;
        private const int FieldPrincipalId = 2;
        private const int FieldProtocol = 3;
        private const int FieldType = 4;
        private const int FieldState = 5;
        private const int FieldIsAdminEndpoint = 6;

        private static string createQuery()
        {
            return @"SELECT 
                        name, 
                        endpoint_id, 
                        principal_id, 
                        protocol_desc, 
                        type_desc, 
                        state_desc, 
                        is_admin_endpoint_c = CASE WHEN is_admin_endpoint = 1 THEN 'Y' ELSE 'N' END
                     FROM sys.endpoints";
        }

        public static bool Process(
                string targetConnection,
                string repositoryConnection,
                int snapshotid,
                ref Dictionary<Sql.SqlObjectType, Dictionary<MetricMeasureType, uint>> metricsData
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(targetConnection));
            Debug.Assert(!string.IsNullOrEmpty(repositoryConnection));
            Stopwatch sw = new Stopwatch();
            sw.Start();
            uint numProcessedEndpoints = 0;

            // Init return.
            bool isOk = true;
            Program.ImpersonationContext wi = Program.SetLocalImpersonationContext();
            // Process endpoints.
            List<int> epList = new List<int>();
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
                        bcp.DestinationTableName = EndPointDataTable.RepositoryTable;
                        bcp.BulkCopyTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                        // Create the datatable to write to the repository.
                        using (DataTable dataTable = EndPointDataTable.Create())
                        {
                            // Create the query.
                            string query = createQuery();
                            Debug.Assert(!string.IsNullOrEmpty(query));

                            // Query to get the table objects.
                            using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(target, null,
                                                CommandType.Text, query, null))
                            {
                                while (rdr.Read())
                                {
                                    // Retrieve information.
                                    SqlString name = rdr.GetSqlString(FieldName);
                                    SqlInt32 id = rdr.GetInt32(FieldId);
                                    SqlInt32 principalid = rdr.GetInt32(FieldPrincipalId);
                                    SqlString state = rdr.GetSqlString(FieldState);
                                    SqlString protocol = rdr.GetSqlString(FieldProtocol);
                                    SqlString type = rdr.GetSqlString(FieldType);
                                    SqlString isadminendpoint = rdr.GetSqlString(FieldIsAdminEndpoint);

                                    // Add endpoint id to the list for permission procesing.
                                    Debug.Assert(!id.IsNull);
                                    epList.Add(id.Value);

                                    // Update the datatable.
                                    DataRow dr = dataTable.NewRow();
                                    dr[EndPointDataTable.ParamSnapshotid] = snapshotid;
                                    dr[EndPointDataTable.ParamPrincipalid] = principalid;
                                    dr[EndPointDataTable.ParamEndpointid] = id;
                                    dr[EndPointDataTable.ParamName] = name;
                                    dr[EndPointDataTable.ParamType] = type;
                                    dr[EndPointDataTable.ParamProtocol] = protocol;
                                    dr[EndPointDataTable.ParamState] = state;
                                    dr[EndPointDataTable.ParamIsadminendpoint] = isadminendpoint;
                                    dr[EndPointDataTable.ParamHashkey] = "";
                                    dataTable.Rows.Add(dr);

                                    numProcessedEndpoints++;
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
                catch (SqlException ex)
                {
                    string strMessage = "Processing endpoints";
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

            // Load endpoint permissions.
            if (isOk)
            {
                if (!ServerPermission.Process(targetConnection, repositoryConnection, snapshotid, SqlObjectType.Endpoint, epList))
                {
                    logX.loggerX.Error("ERROR - error encountered in processing  end point permissions");
                    isOk = false;
                }
            }

            uint oldMetricCount = 0;
            uint oldMetricTime = 0;
            // See if User is already in Endpoint Dictionary
            // ----------------------------------------------
            Dictionary<MetricMeasureType, uint> de;
            if (metricsData.TryGetValue(SqlObjectType.Endpoint, out de))
            {
                de.TryGetValue(MetricMeasureType.Count, out oldMetricCount);
                de.TryGetValue(MetricMeasureType.Time, out oldMetricTime);
            }
            else
            {
                de = new Dictionary<MetricMeasureType, uint>();
            }
            de[MetricMeasureType.Count] = numProcessedEndpoints + oldMetricCount;
            de[MetricMeasureType.Time] = (uint)sw.ElapsedMilliseconds + oldMetricTime;
            metricsData[SqlObjectType.Endpoint] = de;

            return isOk;
        }
    }
}
