/******************************************************************
 * Name: Schema.cs
 *
 * Description: SQL Server schema object encapsulation.
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
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Diagnostics;
using Idera.SQLsecure.Core.Accounts;
using Idera.SQLsecure.Core.Logger;

namespace Idera.SQLsecure.Collector.Sql
{
    internal static class Schema
    {
        private static LogX logX = new LogX("Idera.SQLsecure.Collector.Sql.Schema");
        private const int FieldSchemaid = 0;
        private const int FieldUid = 1;
        private const int FieldSchemaname = 2;

        private static string createSchemaQuery(
                Database database
            )
        {
            Debug.Assert(database != null);

            string query = @"SELECT 
	                            schemaid = schema_id,
	                            uid = principal_id,
	                            schemaname = name "
                           + @"FROM " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + @".sys.schemas";

            return query;
        }

        public static bool Process(
                string targetConnection,
                string repositoryConnection,
                int snapshotid,
                Database database,
                ref Dictionary<Sql.SqlObjectType, Dictionary<MetricMeasureType, uint>> metricsData
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(targetConnection));
            Debug.Assert(!string.IsNullOrEmpty(repositoryConnection));
            Debug.Assert(database != null);
            uint processedSchemaCnt = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Program.ImpersonationContext wi = Program.SetLocalImpersonationContext();
            bool isOk = true;
            List<int> schemaidList = new List<int>();
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
                        bcp.DestinationTableName = DatabaseSchemaDataTable.RepositoryTable;
                        bcp.BulkCopyTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                        // Create the datatable to write to the repository.
                        using (DataTable dataTable = DatabaseSchemaDataTable.Create())
                        {
                            // Create the query based on the rule.
                            string query = createSchemaQuery(database);
                            Debug.Assert(!string.IsNullOrEmpty(query));

                            // Query to get the table objects.
                            using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(target, null,
                                                CommandType.Text, query, null))
                            {
                                while (rdr.Read())
                                {
                                    // Retrieve the object information.
                                    SqlInt32 schemaid = rdr.GetSqlInt32(FieldSchemaid);
                                    SqlInt32 uid = rdr.GetSqlInt32(FieldUid);
                                    SqlString schemaname = rdr.GetSqlString(FieldSchemaname);

                                    // Add schema id to the list.
                                    schemaidList.Add(schemaid.Value);

                                    // Update the datatable.
                                    DataRow dr = dataTable.NewRow();
                                    dr[DatabaseSchemaDataTable.ParamDbid] = database.DbId;
                                    dr[DatabaseSchemaDataTable.ParamUid] = uid;
                                    dr[DatabaseSchemaDataTable.ParamSnapshotid] = snapshotid;
                                    dr[DatabaseSchemaDataTable.ParamSchemaid] = schemaid;
                                    dr[DatabaseSchemaDataTable.ParamSchemaname] = schemaname;
                                    dr[DatabaseSchemaDataTable.ParamHashkey] = "";
                                    dataTable.Rows.Add(dr);
                                    processedSchemaCnt++;
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
                    string strMessage = "Processing schema";
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

            // Load object permissions.
            if (isOk)
            {
                if (!DatabaseSchemaPermission.Process(targetConnection, repositoryConnection, snapshotid, database, schemaidList))
                {
                    logX.loggerX.Error("ERROR - error encountered in processing schema permissions");
                    isOk = false;
                }
            }

            sw.Stop();

            uint oldMetricCount = 0;
            uint oldMetricTime = 0;            
            // See if Schema is already in Metrics Dictionary
            // ----------------------------------------------
            Dictionary<MetricMeasureType, uint> de;
            if (metricsData.TryGetValue(SqlObjectType.Schema, out de))
            {
                de.TryGetValue(MetricMeasureType.Count, out oldMetricCount);
                de.TryGetValue(MetricMeasureType.Time, out oldMetricTime);
            }
            else
            {
                de = new Dictionary<MetricMeasureType, uint>();
            }
            de[MetricMeasureType.Count] = processedSchemaCnt + oldMetricCount;
            de[MetricMeasureType.Time] = (uint)sw.ElapsedMilliseconds + oldMetricTime;
            metricsData[SqlObjectType.Schema] = de;

            return isOk;
        }
    }
}
