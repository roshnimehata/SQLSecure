using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Text;
using Idera.SQLsecure.Core.Logger;

namespace Idera.SQLsecure.Collector.Sql
{
    public class AvailabilityGroup
    {
        private static LogX logX = new LogX("Idera.SQLsecure.Collector.Sql.AvailabilityGroup");


        internal const int ColGroupId = 0;
        internal const int ColName = 1;
        internal const int ColResourceId = 2;
        internal const int ColResourceGroupId = 3;
        internal const int ColFailureConditionLevel = 4;
        internal const int ColHealthCheckTimeout = 5;
        internal const int ColAutomatedBackuppReference = 6;
        internal const int ColAutomatedBackuppReferenceDesc = 7;
        internal const int ColSnapshotid = 8;

        private const string MasterDatabaseName = "master";
     
        public const string GetAvailabilityGroups = @"SELECT  
                                                    groupid=group_id,
                                                    name = name,	
                                                    resourceid =resource_id,
                                                    resourcegroupid= resource_group_id,
                                                    failureconditionlevel= failure_condition_level,
                                                    healthchecktimeout = health_check_timeout,
                                                    automatedbackuppreference=  automated_backup_preference,
                                                    automatedbackuppreferencedesc= automated_backup_preference_desc
                                                    	FROM sys.availability_groups ";


        public const string GetAvailabiltyReplicas = @"select  
                                                     replicaid = replica_id,  
                                                     groupid= group_id, 
                                                     replicaservername= replica_server_name, 
                                                     ownersid=  owner_sid, 
                                                     endpointurl=endpoint_url, 
                                                     availabilitymode=availability_mode, 
                                                     availabilitymodedesc= availability_mode_desc, 
                                                     failovermode= failover_mode, 
                                                     failovermodedesc =failover_mode_desc, 
                                                     createdate=create_date, 
                                                     modifydate= modify_date, 
                                                     replicametadataid=replica_metadata_id  
                                                     from sys.availability_replicas";
    


        public static bool ProcessGroups(ServerVersion version,
           string targetConnection,
           string repositoryConnection,
           int snapshotid,
           string server,
           ServerType serverType)
        {
            Debug.Assert(version != ServerVersion.Unsupported);
            Debug.Assert(version >= ServerVersion.SQL2012);
            Debug.Assert(!String.IsNullOrEmpty(targetConnection));
            Debug.Assert(!String.IsNullOrEmpty(repositoryConnection));


            List<int> ids = new List<int>();
            bool isOk = true;
            targetConnection = SqlHelper.AppendDatabaseToConnectionString(targetConnection, MasterDatabaseName);
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
                        bcp.DestinationTableName = AvailabilityGroupTable.RepositoryTable;
                        bcp.BulkCopyTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                        // Create the datatable to write to the repository.
                        using (DataTable dataTable = AvailabilityGroupTable.Create())
                        {
                            // Process each rule to collect the table objects.


                            // Query to get the table objects.
                            using (SqlDataReader rdr = SqlHelper.ExecuteReader(target, null, CommandType.Text, GetAvailabilityGroups, null))
                            {
                                while (rdr.Read())
                                {
                                    // Retrieve the object information.

                                    DataRow dr = dataTable.NewRow();
                                    dr[AvailabilityGroupTable.ParamGroupId] = rdr.GetSqlGuid(ColGroupId);
                                    dr[AvailabilityGroupTable.ParamName] = rdr.GetSqlString(ColName);
                                    dr[AvailabilityGroupTable.ParamResourceId] = rdr.GetSqlString(ColResourceId);
                                    dr[AvailabilityGroupTable.ParamResourceGroupId] = rdr.GetSqlString(ColResourceGroupId);
                                    dr[AvailabilityGroupTable.ParamFailureConditionLevel] = rdr.GetSqlInt32(ColFailureConditionLevel);
                                    dr[AvailabilityGroupTable.ParamHealthCheckTimeout] = rdr.GetSqlInt32(ColHealthCheckTimeout);
                                    dr[AvailabilityGroupTable.ParamAutomatedBackuppReference] = rdr.GetSqlByte(ColAutomatedBackuppReference);
                                    dr[AvailabilityGroupTable.ParamAutomatedBackuppReferenceDesc] = rdr.GetSqlString(ColAutomatedBackuppReferenceDesc);
                                    dr[AvailabilityGroupTable.ParamSnapshotid] = snapshotid;



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
                                            string strMessage = "Writing to Repository sql server availability groups failed";

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
                                        string strMessage = "Writing to Repository sql server availability groups failed";
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
                    string strMessage = "Processing sql server availability groups failed";
                    logX.loggerX.Error("ERROR - " + strMessage, ex);
                    Database.CreateApplicationActivityEventInRepository(repositoryConnection,
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
                return ProcessReplicas(version, targetConnection, repositoryConnection, snapshotid, server,serverType);


            return isOk;
        }


        public static bool ProcessReplicas(ServerVersion version,
         string targetConnection,
         string repositoryConnection,
         int snapshotid,
         string server,
         ServerType serverType)
        {
            Debug.Assert(version != ServerVersion.Unsupported);
            Debug.Assert(version >= ServerVersion.SQL2012);
            Debug.Assert(!String.IsNullOrEmpty(targetConnection));
            Debug.Assert(!String.IsNullOrEmpty(repositoryConnection));

            bool isOk = true;
            targetConnection = SqlHelper.AppendDatabaseToConnectionString(targetConnection, MasterDatabaseName);
            Program.ImpersonationContext wi = Program.SetLocalImpersonationContext();
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
                        bcp.DestinationTableName = AvailabilityReplicas.RepositoryTable;
                        bcp.BulkCopyTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                        // Create the datatable to write to the repository.
                        using (DataTable dataTable = AvailabilityReplicas.Create())
                        {
                            // Process each rule to collect the table objects.


                            // Query to get the  objects.
                            using (SqlDataReader rdr = SqlHelper.ExecuteReader(target, null, CommandType.Text, GetAvailabiltyReplicas, null))
                            {
                                while (rdr.Read())
                                {
                                    // Retrieve the object information.

                                    DataRow dr = dataTable.NewRow();

                                    dr[AvailabilityReplicas.ParamReplicaId] = rdr.GetGuid(AvailabilityReplicas.ColReplicaid);
                                    dr[AvailabilityReplicas.ParamSnapshotid] = snapshotid;
                                    dr[AvailabilityReplicas.ParamGroupid] = rdr.GetGuid(AvailabilityReplicas.ColGroupId);
                                    dr[AvailabilityReplicas.ParamReplicaServerName] = rdr.GetSqlString(AvailabilityReplicas.ColReplicaServerName);
                                    dr[AvailabilityReplicas.ParamOwnersid] = rdr.GetSqlBinary(AvailabilityReplicas.ColOwnersid);
                                    dr[AvailabilityReplicas.ParamEndpointUrl] = rdr.GetSqlString(AvailabilityReplicas.ColEndpointUrl);
                                    dr[AvailabilityReplicas.ParamAvailabilityMode] = rdr.GetSqlByte(AvailabilityReplicas.ColAvailabilityMode);
                                    dr[AvailabilityReplicas.ParamAvailabilityModeDesc] = rdr.GetSqlString(AvailabilityReplicas.ColAvailabilityModeDesc);
                                    dr[AvailabilityReplicas.ParamFailoverMode] = rdr.GetSqlByte(AvailabilityReplicas.ColFailoverMode);
                                    dr[AvailabilityReplicas.ParamFailoverModeDesc] = rdr.GetSqlString(AvailabilityReplicas.ColFailoverModeDesc);
                                    dr[AvailabilityReplicas.ParamCreateDate] = rdr.GetSqlDateTime(AvailabilityReplicas.ColCreateDate);
                                    dr[AvailabilityReplicas.ParamModifyDate] = rdr.GetSqlDateTime(AvailabilityReplicas.ColModifyDate);
                                    SqlInt32 replicaMetaDataId = rdr.GetSqlInt32(AvailabilityReplicas.ColReplicaMetadataId);
                                    dr[AvailabilityReplicas.ParamReplicaMetadataId] = replicaMetaDataId;

                                    if (!replicaMetaDataId.IsNull)
                                        epList.Add(replicaMetaDataId.Value);

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
                                            string strMessage = "Writing to Repository sql server availability replicas failed";

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
                                        string strMessage = "Writing to Repository sql server availability replicas failed";
                                        logX.loggerX.Error("ERROR - " + strMessage, ex);
                                        throw;
                                    }
                                }
                            }
                        }
                    }
                    if (epList.Count != 0)
                    {
                        if (!ServerPermission.Process(targetConnection, repositoryConnection, snapshotid, SqlObjectType.AvailabilityGroup, epList,serverType))
                        {
                            logX.loggerX.Error("ERROR - error encountered in processing  availability group  permissions");
                            isOk = false;
                        }
                    }
                }
                catch (SqlException ex)
                {
                    string strMessage = "Processing sql server availability replicas failed";
                    logX.loggerX.Error("ERROR - " + strMessage, ex);
                    Database.CreateApplicationActivityEventInRepository(repositoryConnection,
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
    }
}
