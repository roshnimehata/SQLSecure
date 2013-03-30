/******************************************************************
 * Name: ServerPermissions.cs
 *
 * Description: Server permissions are encapsulated in this file.
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
    internal static class ServerPermission
    {
        private static LogX logX = new LogX("Idera.SQLsecure.Collector.Sql.ServerPermission");
        private const int FieldClass = 0;
        private const int FieldMajorId = 1;
        private const int FieldMinorId = 2;
        private const int FieldGrantee = 3;
        private const int FieldGrantor = 4;
        private const int FieldType = 5;
        private const int FieldPermission = 6;
        private const int FieldIsGrant = 7;
        private const int FieldIsWithGrant = 8;
        private const int FieldIsRevoke = 9;
        private const int FieldIsDeny = 10;

        private static string createPermissionQuery(
                SqlObjectType otype,
                List<int> oidbatch
            )
        {
            Debug.Assert(otype == SqlObjectType.Server || otype == SqlObjectType.Login || otype == SqlObjectType.Endpoint);
            Debug.Assert(oidbatch != null && oidbatch.Count > 0);

            StringBuilder query = new StringBuilder();
            query.Append(@"SELECT 
                                classid = CAST (class AS int), 
                                major_id, 
                                minor_id, 
                                grantee_principal_id, 
                                grantor_principal_id, 
                                type, 
                                permission_name, 
	                            isgrant = CASE 
                                             WHEN state = 'G' THEN 'Y'
                                             WHEN state = 'W' THEN 'Y' 
                                             ELSE 'N'
                                          END,
                                iswithgrant = CASE WHEN state = 'W' THEN 'Y' ELSE 'N' END,
                                isrevoke = CASE WHEN state = 'R' THEN 'Y' ELSE 'N' END,
                                isdeny = CASE WHEN state = 'D' THEN 'Y' ELSE 'N' END
                              FROM sys.server_permissions
                              WHERE class = ");
            switch (otype)
	        {
		        case SqlObjectType.Server:
                    query.Append("100 ");
                    break;
                case SqlObjectType.Login:
                    query.Append("101 ");
                    break;
                case SqlObjectType.Endpoint:
                    query.Append("105 ");
                    break;
                default:
                    Debug.Assert(false);
                    break;
	        }
            query.Append("AND major_id IN ( ");
            for (int i = 0; i < oidbatch.Count; ++i)
            {
                query.Append(oidbatch[i].ToString());
                query.Append((i == (oidbatch.Count - 1)) ? " )" : ", ");
            }
            return query.ToString();
        }

        public static bool Process(
                string targetConnection,
                string repositoryConnection,
                int snapshotid,
                SqlObjectType oType,
                List<int> oidList
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(targetConnection));
            Debug.Assert(!string.IsNullOrEmpty(repositoryConnection));
            Debug.Assert(oidList != null && oidList.Count > 0);
            Program.ImpersonationContext wi = Program.SetLocalImpersonationContext();

            bool isOk = true;
            using (SqlConnection repository = new SqlConnection(repositoryConnection))
            {
                using (SqlConnection target = new SqlConnection(targetConnection))
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
                            bcp.DestinationTableName = ServerPermissionDataTable.RepositoryTable;
                            bcp.BulkCopyTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                            // Create the datatable to write to the repository.
                            using (DataTable dataTable = ServerPermissionDataTable.Create())
                            {
                                // Process each uid in the uid list.
                                int oidcntr = oidList.Count;
                                List<int> oidbatch = new List<int>();
                                foreach (int oid in oidList)
                                {
                                    // Add oid to the batch.
                                    oidbatch.Add(oid);

                                    // If batch count is at threshold query & process.
                                    --oidcntr;
                                    if (oidbatch.Count == Constants.PermissionBatchSize || oidcntr == 0)
                                    {
                                        // Create the query based on the object.
                                        string query = createPermissionQuery(oType, oidbatch);
                                        Debug.Assert(!string.IsNullOrEmpty(query));

                                        // Clear the batch.
                                        oidbatch.Clear();

                                        // Query to get the column objects.
                                        using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(target, null,
                                                                                               CommandType.Text, query,
                                                                                               null))
                                        {
                                            while (rdr.Read())
                                            {
                                                // Retrieve the values.
                                                SqlInt32 classid = rdr.GetSqlInt32(FieldClass);
                                                SqlInt32 majorid = rdr.GetSqlInt32(FieldMajorId);
                                                SqlInt32 minorid = rdr.GetSqlInt32(FieldMinorId);
                                                SqlInt32 grantee = rdr.GetSqlInt32(FieldGrantee);
                                                SqlInt32 grantor = rdr.GetSqlInt32(FieldGrantor);
                                                SqlString type = rdr.GetSqlString(FieldType);
                                                SqlString permission = rdr.GetSqlString(FieldPermission);
                                                SqlString isgrant = rdr.GetSqlString(FieldIsGrant);
                                                SqlString iswithgrant = rdr.GetSqlString(FieldIsWithGrant);
                                                SqlString isrevoke = rdr.GetSqlString(FieldIsRevoke);
                                                SqlString isdeny = rdr.GetSqlString(FieldIsDeny);

                                                // Update the datatable.
                                                DataRow dr = dataTable.NewRow();
                                                dr[ServerPermissionDataTable.ParamSnapshotid] = snapshotid;
                                                dr[ServerPermissionDataTable.ParamClassId] = classid;
                                                dr[ServerPermissionDataTable.ParamMajorId] = majorid;
                                                dr[ServerPermissionDataTable.ParamMinorId] = minorid;
                                                dr[ServerPermissionDataTable.ParamGrantee] = grantee;
                                                dr[ServerPermissionDataTable.ParamGrantor] = grantor;
                                                dr[ServerPermissionDataTable.ParamPermission] = permission;
                                                dr[ServerPermissionDataTable.ParamIsgrant] = isgrant;
                                                dr[ServerPermissionDataTable.ParamIsgrantwith] = iswithgrant;
                                                dr[ServerPermissionDataTable.ParamIsrevoke] = isrevoke;
                                                dr[ServerPermissionDataTable.ParamIsdeny] = isdeny;
                                                dr[ServerPermissionDataTable.ParamHashkey] = "";
                                                dataTable.Rows.Add(dr);

                                                // Keep counter of number of PermissionsCollected
                                                // ----------------------------------------------
                                                Target.numPermissionsCollected++;

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
                        }
                    }

                    catch (SqlException ex)
                    {
                        string strMessage = "Processing server level permissions for  " + oType;
                        logX.loggerX.Error("ERROR - " + strMessage, ex);
                        Sql.Database.CreateApplicationActivityEventInRepository(repositoryConnection,
                                                                                snapshotid,
                                                                                Collector.Constants.ActivityType_Error,
                                                                                Collector.Constants.ActivityEvent_Error,
                                                                                strMessage + ex.Message);
                        AppLog.WriteAppEventError(SQLsecureEvent.ExErrExceptionRaised, SQLsecureCat.DlDataLoadCat,
                                                  " SQL Server = " +
                                                  new SqlConnectionStringBuilder(targetConnection).DataSource +
                                                  strMessage, ex.Message);

                        isOk = false;
                    }
                }
            }
            Program.RestoreImpersonationContext(wi);
            return isOk;
        }
    }
}
