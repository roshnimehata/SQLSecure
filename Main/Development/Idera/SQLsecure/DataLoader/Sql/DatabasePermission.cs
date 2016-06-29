/******************************************************************
 * Name: DatabasePermission.cs
 *
 * Description: This class encapsulates database permissions.
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
    /// <summary>
    /// 
    /// </summary>
    internal static class DatabaseObjectPermission
    {
        private static LogX logX = new LogX("Idera.SQLsecure.Collector.Sql.DatabasePermission");

        // ------------- Types -------------
        #region Types

        /// <summary>
        /// Encapsulates column permissions processing.
        /// </summary>
        class ColumnPermissions
        {
            // ------------- Fields -------------
            #region Fields

            byte[] m_Columns;

            #endregion
            
            // ------------- Helpers -------------
            #region Helpers
            #endregion

            // ------------- Ctors -------------
            #region Ctors

            public ColumnPermissions (SqlBinary columns)
            {
                m_Columns = columns.IsNull ? null : columns.Value;
            }  

            #endregion

            // ------------- Properties -------------
            #region Properties

            public bool IsNoColumns
            {
                get
                {
                    /*
                     * In SQL 2000 'sysprotects' column 'columns' is a bit array.   This bit array
                     * indicates which columsn the permission is assigned to. Bit 0 is reserved for
                     * all columns, if this bit is set it means permission applies to all columns.
                     * If this column is null it also means that permissions applies to all columns.
                     */
                    if (m_Columns == null)
                    {
                        return true;
                    }
                    else
                    {
                        return (m_Columns[0] & 0x01) == 0x01;
                    }
                }
            }

            public List<int> Columns
            {
                get
                {
                    List<int> columns = new List<int>();

                    /*
                     * In SQL 2000 'sysprotects' column 'columns' is a bit array.   This bit array
                     * indicates which columsn the permission is assigned to. Bit 0 is reserved for
                     * all columns, if this bit is set it means permission applies to all columns.
                     * Individual olumn indices start from bit 1.  Any bit that is set indicates that 
                     * column has been assigned the permissions.
                     */
                    if (!IsNoColumns)
                    {
                        // Process the first byte.
                        if (m_Columns.Length != 0)
                        {
                            for (int bitIndex = 1; bitIndex < 8; ++bitIndex)
                            {
                                if ((m_Columns[0] & (1 << bitIndex)) == (1 << bitIndex))
                                {
                                    columns.Add(bitIndex);
                                }
                            }
                        }

                        // Treat the rest of the bytes differently.
                        for (int byteIndex = 1; byteIndex < m_Columns.Length; ++byteIndex)
                        {
                            for (int bitIndex = 0; bitIndex < 8; ++bitIndex)
                            {
                                if ((m_Columns[byteIndex] & (1 << bitIndex)) == (1 << bitIndex))
                                {
                                    int index = byteIndex * 8 + bitIndex; 
                                    columns.Add(index);                        
                                }
                            }
                        }
                    }

                    return columns;
                }
            }

            #endregion

            // ------------- Methods -------------
            #region Methods
            #endregion
        }
          

        #endregion

        // ------------- Process -------------
        #region Process

        private const int FieldDatabasePermissionClassid = 0;
        private const int FieldDatabasePermissionObjectid = 1;
        private const int FieldDatabasePermissionParentobjectid = 2;
        private const int FieldDatabasePermissionGrantor = 3;
        private const int FieldDatabasePermissionPermission = 4;
        private const int FieldDatabasePermissionGrantee = 5;
        private const int FieldDatabasePermissionIsgrant = 6;
        private const int FieldDatabasePermissionIsgrantwith = 7;
        private const int FieldDatabasePermissionIsrevoke = 8;
        private const int FieldDatabasePermissionIsdeny = 9;
        private const int FieldDatabasePermissionColumns = 10; // SQL Server 2000 only.

        private static string createDbPermissionQuery(
                ServerVersion version,
                Database database,
                List<ObjId> objidbatch
            )
        {
            Debug.Assert(version != ServerVersion.Unsupported);
            Debug.Assert(database != null);
            Debug.Assert(objidbatch != null && objidbatch.Count > 0);

            StringBuilder query = new StringBuilder();

            if (version == ServerVersion.SQL2000)
            {
                query.Append(@"SELECT 
                            classid = " + objidbatch[0].ClassId.ToString() + @", 
                            objectid = id, 
                            parentobjectid = 0,
                            grantor = CAST(grantor AS int),
                            permission = CASE 
			                                WHEN action = 26 THEN 'REFERENCES' 
			                                WHEN action = 178 THEN 'CREATE FUNCTION'
			                                WHEN action = 193 THEN 'SELECT'
			                                WHEN action = 195 THEN 'INSERT'
			                                WHEN action = 196 THEN 'DELETE'
			                                WHEN action = 197 THEN 'UPDATE'
			                                WHEN action = 198 THEN 'CREATE TABLE'
			                                WHEN action = 203 THEN 'CREATE DATABASE'
			                                WHEN action = 207 THEN 'CREATE VIEW'
			                                WHEN action = 222 THEN 'CREATE PROCEDURE'
			                                WHEN action = 224 THEN 'EXECUTE'
			                                WHEN action = 228 THEN 'BACKUP DATABASE'
			                                WHEN action = 233 THEN 'CREATE DEFAULT'
			                                WHEN action = 235 THEN 'BACKUP LOG'
			                                WHEN action = 236 THEN 'CREATE RULE'
			                                ELSE 'UNKNOWN'
		                                 END,
                            grantee = CAST(uid AS int),
                            isgrant = CASE 
                                            WHEN protecttype = 205 THEN 'Y'  
                                            WHEN protecttype = 204 THEN 'Y' 
                                            ELSE 'N'
                                      END,
                            isgrantwith = CASE WHEN protecttype = 204 THEN 'Y' ELSE 'N' END,
                            isrevoke = 'N', 
                            isdeny = CASE WHEN protecttype = 206 THEN 'Y' ELSE 'N' END, 
                            columns "
                        + @"FROM " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + @".dbo.sysprotects "
                        + @"WHERE id IN ( ");
                for (int i = 0; i < objidbatch.Count; ++i)
                {
                    query.Append(objidbatch[i].ObjectId.ToString());
                    query.Append((i == (objidbatch.Count - 1)) ? " )" : ", ");
                }
            }
            else
            {
                query.Append(@"SELECT 
                            classid = CAST(class AS int),
                            objectid = CASE WHEN minor_id = 0 THEN major_id ELSE minor_id END,
                            parentobjectid = CASE WHEN minor_id = 0 THEN 0 ELSE major_id END,
                            grantor = grantor_principal_id,
                            permission = permission_name,
                            grantee = grantee_principal_id,
                            isgrant = CASE 
                                         WHEN state = 'G' THEN 'Y'
                                         WHEN state = 'W' THEN 'Y' 
                                         ELSE 'N'
                                      END,
                            isgrantwith = CASE WHEN state = 'W' THEN 'Y' ELSE 'N' END,
                            isrevoke = CASE WHEN state = 'R' THEN 'Y' ELSE 'N' END,
                            isdeny = CASE WHEN state = 'D' THEN 'Y' ELSE 'N' END, 
                            columns = CAST(null AS varbinary) "
                        + @"FROM " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + @".sys.database_permissions "
                        + @"WHERE class = " + objidbatch[0].ClassId.ToString() + @" AND major_id IN ( ");
                for (int i = 0; i < objidbatch.Count; ++i)
                {
                    query.Append(objidbatch[i].ObjectId.ToString());
                    query.Append((i == (objidbatch.Count - 1)) ? " )" : ", ");
                }
            }

            return query.ToString();
        }

        public static bool Process(
                ServerVersion version,
                string targetConnection,
                string repositoryConnection,
                int snapshotid,
                Database database,
                ObjIdCollection objIdCollection
            )
        {
            Debug.Assert(version != ServerVersion.Unsupported);
            Debug.Assert(!string.IsNullOrEmpty(targetConnection));
            Debug.Assert(!string.IsNullOrEmpty(repositoryConnection));
            Debug.Assert(database != null);
            Debug.Assert(objIdCollection != null);
            Program.ImpersonationContext wi = Program.SetLocalImpersonationContext();
            bool isOk = true;
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
                        bcp.DestinationTableName = DatabaseObjectPermissionDataTable.RepositoryTable;
                        bcp.BulkCopyTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                        // Create the datatable to write to the repository.
                        using (DataTable dataTable = DatabaseObjectPermissionDataTable.Create())
                        {
                            // Process each object id in the obj id collection.
                            int idcntr = objIdCollection.ObjIdSet.Count;
                            List<ObjId> objidbatch = new List<ObjId>();
                            foreach (ObjId objId in objIdCollection.ObjIdSet)
                            {
                                // Add obj id to the batch.
                                objidbatch.Add(objId);

                                // If batch count is at threshold query & process.
                                --idcntr;
                                if (objidbatch.Count == Constants.PermissionBatchSize || idcntr == 0)
                                {
                                    // Create the query based on the object.
                                    string query = createDbPermissionQuery(version, database, objidbatch);
                                    Debug.Assert(!string.IsNullOrEmpty(query));

                                    // Clear the batch.
                                    objidbatch.Clear();

                                    // Query to get the column objects.
                                    using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(target, null,
                                                        CommandType.Text, query, null))
                                    {
                                        while (rdr.Read())
                                        {
                                            // Retrieve the values.
                                            SqlInt32 classid = rdr.GetSqlInt32(FieldDatabasePermissionClassid);
                                            SqlInt32 objectid = rdr.GetSqlInt32(FieldDatabasePermissionObjectid);
                                            SqlInt32 parentobjectid = rdr.GetSqlInt32(FieldDatabasePermissionParentobjectid);
                                            SqlInt32 grantor = rdr.GetSqlInt32(FieldDatabasePermissionGrantor);
                                            SqlString permission = rdr.GetSqlString(FieldDatabasePermissionPermission);
                                            SqlInt32 grantee = rdr.GetSqlInt32(FieldDatabasePermissionGrantee);
                                            SqlString isgrant = rdr.GetSqlString(FieldDatabasePermissionIsgrant);
                                            SqlString isgrantwith = rdr.GetSqlString(FieldDatabasePermissionIsgrantwith);
                                            SqlString isrevoke = rdr.GetSqlString(FieldDatabasePermissionIsrevoke);
                                            SqlString isdeny = rdr.GetSqlString(FieldDatabasePermissionIsdeny);
                                            SqlBinary columns = rdr.GetSqlBinary(FieldDatabasePermissionColumns);

                                            // Update the datatable.
                                            ColumnPermissions columnPermissions = new ColumnPermissions(columns);
                                            if (version != ServerVersion.SQL2000
                                                || (version == ServerVersion.SQL2000 && columnPermissions.IsNoColumns))
                                            {
                                                DataRow dr = dataTable.NewRow();
                                                dr[DatabaseObjectPermissionDataTable.ParamClassid] = classid;
                                                dr[DatabaseObjectPermissionDataTable.ParamObjectid] = objectid;
                                                dr[DatabaseObjectPermissionDataTable.ParamParentobjectid] = parentobjectid;
                                                dr[DatabaseObjectPermissionDataTable.ParamGrantor] = grantor;
                                                dr[DatabaseObjectPermissionDataTable.ParamSnapshotid] = snapshotid;
                                                dr[DatabaseObjectPermissionDataTable.ParamDbid] = database.DbId;
                                                dr[DatabaseObjectPermissionDataTable.ParamPermission] = permission;
                                                dr[DatabaseObjectPermissionDataTable.ParamGrantee] = grantee;
                                                dr[DatabaseObjectPermissionDataTable.ParamIsgrant] = isgrant;
                                                dr[DatabaseObjectPermissionDataTable.ParamIsgrantwith] = isgrantwith;
                                                dr[DatabaseObjectPermissionDataTable.ParamIsrevoke] = isrevoke;
                                                dr[DatabaseObjectPermissionDataTable.ParamIsdeny] = isdeny;
                                                dr[DatabaseObjectPermissionDataTable.ParamHashkey] = "";
                                                dataTable.Rows.Add(dr);

                                                // Keep counter of number of PermissionsCollected
                                                // ----------------------------------------------
                                                Target.numPermissionsCollected++;

                                                // Write to repository if exceeds threshold.
                                                if (dataTable.Rows.Count > Constants.RowBatchSize)
                                                {
                                                    try
                                                    {
                                                        bcp.WriteToServer(dataTable);
                                                        dataTable.Clear();
                                                    }
                                                    catch (SqlException ex)
                                                    {
                                                        string strMessage = "Writing database object permissions to Repository ";
                                                        logX.loggerX.Error("ERROR - " + strMessage, ex);
                                                        throw ex;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                // Get indices to which permissions apply and create
                                                // seperate data row for each of them.
                                                List<int> colIndices = columnPermissions.Columns;
                                                foreach (int colid in colIndices)
                                                {
                                                    DataRow dr = dataTable.NewRow();
                                                    dr[DatabaseObjectPermissionDataTable.ParamClassid] = classid;
                                                    dr[DatabaseObjectPermissionDataTable.ParamObjectid] = colid;
                                                    dr[DatabaseObjectPermissionDataTable.ParamParentobjectid] = objectid;
                                                    dr[DatabaseObjectPermissionDataTable.ParamGrantor] = grantor;
                                                    dr[DatabaseObjectPermissionDataTable.ParamSnapshotid] = snapshotid;
                                                    dr[DatabaseObjectPermissionDataTable.ParamDbid] = database.DbId;
                                                    dr[DatabaseObjectPermissionDataTable.ParamPermission] = permission;
                                                    dr[DatabaseObjectPermissionDataTable.ParamGrantee] = grantee;
                                                    dr[DatabaseObjectPermissionDataTable.ParamIsgrant] = isgrant;
                                                    dr[DatabaseObjectPermissionDataTable.ParamIsgrantwith] = isgrantwith;
                                                    dr[DatabaseObjectPermissionDataTable.ParamIsrevoke] = isrevoke;
                                                    dr[DatabaseObjectPermissionDataTable.ParamIsdeny] = isdeny;
                                                    dr[DatabaseObjectPermissionDataTable.ParamHashkey] = "";
                                                    dataTable.Rows.Add(dr);

                                                    // Keep counter of number of PermissionsCollected
                                                    // ----------------------------------------------
                                                    Target.numPermissionsCollected++;

                                                    // Write to repository if exceeds threshold.
                                                    if (dataTable.Rows.Count > Constants.RowBatchSize)
                                                    {
                                                        try
                                                        {
                                                            bcp.WriteToServer(dataTable);
                                                            dataTable.Clear();
                                                        }
                                                        catch (SqlException ex)
                                                        {
                                                            string strMessage = "Writing database object permissions to Repository ";
                                                            logX.loggerX.Error("ERROR - " + strMessage, ex);
                                                            throw ex;
                                                        }
                                                    }
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
                                                string strMessage = "Writing database object permissions to Repository ";
                                                logX.loggerX.Error("ERROR - " + strMessage, ex);
                                                throw ex;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    string strMessage = "Processing database object permissions";
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

        #endregion
    }

    internal static class DatabaseSchemaPermission
    {
        private static LogX logX = new LogX("Idera.SQLsecure.Collector.Sql.DatabaseSchemaPrincipal");
        private const int FieldSchemaid = 0;
        private const int FieldGrantor = 1;
        private const int FieldGrantee = 2;
        private const int FieldClassid = 3;
        private const int FieldPermission = 4;
        private const int FieldIsgrant = 5;
        private const int FieldIsgrantwith = 6;
        private const int FieldIsrevoke = 7;
        private const int FieldIsdeny = 8;

        private static string createSchemaPermissionQuery (
                Database database,
                List<int> schemaidbatch
            )
        {
            Debug.Assert(database != null);
            Debug.Assert(schemaidbatch != null && schemaidbatch.Count > 0);

            StringBuilder query = new StringBuilder();
            query.Append(@"SELECT 
	                            schemaid = major_id,
	                            grantor = grantor_principal_id,
	                            grantee = grantee_principal_id,
	                            classid = CAST(class AS int),
	                            permission = permission_name,
	                            isgrant = CASE 
                                             WHEN state = 'G' THEN 'Y'
                                             WHEN state = 'W' THEN 'Y'
                                             ELSE 'N'
                                          END,
	                            isgrantwith = CASE WHEN state = 'W' THEN 'Y' ELSE 'N' END,
	                            isrevoke = CASE WHEN state = 'R' THEN 'Y' ELSE 'N' END,
	                            isdeny = CASE WHEN state = 'D' THEN 'Y' ELSE 'N' END "
                            + @"FROM " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + @".sys.database_permissions "
                            + @"WHERE class = 3 AND major_id IN ( ");
            for (int i = 0; i < schemaidbatch.Count; ++i)
            {
                query.Append(schemaidbatch[i].ToString());
                query.Append((i == (schemaidbatch.Count - 1)) ? " )" : ", ");
            }
            return query.ToString();
        }

        public static bool Process(
                string targetConnection,
                string repositoryConnection,
                int snapshotid,
                Database database,
                List<int> schemaidList
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(targetConnection));
            Debug.Assert(!string.IsNullOrEmpty(repositoryConnection));
            Debug.Assert(database != null);
            Debug.Assert(schemaidList != null);
            Program.ImpersonationContext wi = Program.SetLocalImpersonationContext();
            bool isOk = true;
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
                        bcp.DestinationTableName = DatabaseSchemaPermissionDataTable.RepositoryTable;
                        bcp.BulkCopyTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                        // Create the datatable to write to the repository.
                        using (DataTable dataTable = DatabaseSchemaPermissionDataTable.Create())
                        {
                            // Process each schema id in the list.
                            int schemaidcntr = schemaidList.Count;
                            List<int> schemaidbatch = new List<int>();
                            foreach(int schemaidv in schemaidList)
                            {
                                // Add schema id to the batch.
                                schemaidbatch.Add(schemaidv);

                                // If batch count is at threshold or at the end of the list query & process.
                                --schemaidcntr;
                                if (schemaidbatch.Count == Constants.PermissionBatchSize || schemaidcntr == 0)
                                {
                                    // Create the query based on the object.
                                    string query = createSchemaPermissionQuery(database, schemaidbatch);
                                    Debug.Assert(!string.IsNullOrEmpty(query));

                                    // Clear the batch.
                                    schemaidbatch.Clear();

                                    // Query to get the column objects.
                                    using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(target, null,
                                                        CommandType.Text, query, null))
                                    {
                                        while (rdr.Read())
                                        {
                                            // Retrieve the values.
                                            SqlInt32 schemaid = rdr.GetSqlInt32(FieldSchemaid);
                                            SqlInt32 grantor = rdr.GetSqlInt32(FieldGrantor);
                                            SqlInt32 grantee = rdr.GetSqlInt32(FieldGrantee);
                                            SqlInt32 classid = rdr.GetSqlInt32(FieldClassid);
                                            SqlString permission = rdr.GetSqlString(FieldPermission);
                                            SqlString isgrant = rdr.GetSqlString(FieldIsgrant);
                                            SqlString isgrantwith = rdr.GetSqlString(FieldIsgrantwith);
                                            SqlString isrevoke = rdr.GetSqlString(FieldIsrevoke);
                                            SqlString isdeny = rdr.GetSqlString(FieldIsdeny);

                                            // Update the datatable.
                                            DataRow dr = dataTable.NewRow();
                                            dr[DatabaseSchemaPermissionDataTable.ParamSnapshotid] = snapshotid;
                                            dr[DatabaseSchemaPermissionDataTable.ParamSchemaid] = schemaid;
                                            dr[DatabaseSchemaPermissionDataTable.ParamGrantor] = grantor;
                                            dr[DatabaseSchemaPermissionDataTable.ParamGrantee] = grantee;
                                            dr[DatabaseSchemaPermissionDataTable.ParamDbid] = database.DbId;
                                            dr[DatabaseSchemaPermissionDataTable.ParamClassid] = classid;
                                            dr[DatabaseSchemaPermissionDataTable.ParamPermission] = permission;
                                            dr[DatabaseSchemaPermissionDataTable.ParamIsgrant] = isgrant;
                                            dr[DatabaseSchemaPermissionDataTable.ParamIsgrantwith] = isgrantwith;
                                            dr[DatabaseSchemaPermissionDataTable.ParamIsrevoke] = isrevoke;
                                            dr[DatabaseSchemaPermissionDataTable.ParamIsdeny] = isdeny;
                                            dr[DatabaseSchemaPermissionDataTable.ParamHashkey] = "";
                                            dataTable.Rows.Add(dr);

                                            // Keep counter of number of PermissionsCollected
                                            // ----------------------------------------------
                                            Target.numPermissionsCollected++;

                                            // Write to repository if exceeds threshold.
                                            if (dataTable.Rows.Count > Constants.RowBatchSize)
                                            {
                                                try
                                                {
                                                    bcp.WriteToServer(dataTable);
                                                    dataTable.Clear();
                                                }
                                                catch (SqlException ex)
                                                {
                                                    string strMessage = "Writing database schema permissions to Repository ";
                                                    logX.loggerX.Error("ERROR - " + strMessage, ex);
                                                    throw ex;
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
                                                string strMessage = "Writing database schema permissions to Repository ";
                                                logX.loggerX.Error("ERROR - " + strMessage, ex);
                                                throw ex;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    string strMessage = "Processing database schema permissions";
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
    }

    internal static class DatabasePrincipalPermission
    {
        private static LogX logX = new LogX("Idera.SQLsecure.Collector.Sql.DatabasePrincipalPermission");
        private const int FieldUid = 0;
        private const int FieldGrantor = 1;
        private const int FieldGrantee = 2;
        private const int FieldClassid = 3;
        private const int FieldPermission = 4;
        private const int FieldIsgrant = 5;
        private const int FieldIsgrantwith = 6;
        private const int FieldIsrevoke = 7;
        private const int FieldIsdeny = 8;

        private static string createPrincipalPermissionQuery (
                Database database,
                List<int> uidbatch
            )
        {
            Debug.Assert(database != null);
            Debug.Assert(uidbatch != null && uidbatch.Count > 0);

            StringBuilder query = new StringBuilder();
            query.Append(@"SELECT 
	                            uid = major_id,
	                            grantor = grantor_principal_id,
	                            grantee = grantee_principal_id,
	                            classid = CAST(class AS int),
	                            permission = permission_name,
	                            isgrant = CASE 
                                             WHEN state = 'G' THEN 'Y'
                                             WHEN state = 'W' THEN 'Y'
                                             ELSE 'N'
                                          END,
	                            isgrantwith = CASE WHEN state = 'W' THEN 'Y' ELSE 'N' END,
	                            isrevoke = CASE WHEN state = 'R' THEN 'Y' ELSE 'N' END,
	                            isdeny = CASE WHEN state = 'D' THEN 'Y' ELSE 'N' END "
                            + @"FROM " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + @".sys.database_permissions "
                            + @"WHERE class = 4" + @" AND major_id IN ( ");
            for (int i = 0; i < uidbatch.Count; ++i)
            {
                query.Append(uidbatch[i].ToString());
                query.Append((i == (uidbatch.Count - 1)) ? " )" : ", ");
            }
            return query.ToString();
        }

        public static bool Process(
                string targetConnection,
                string repositoryConnection,
                int snapshotid,
                Database database,
                List<int> uidList
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(targetConnection));
            Debug.Assert(!string.IsNullOrEmpty(repositoryConnection));
            Debug.Assert(database != null);
            Debug.Assert(uidList != null);
            Program.ImpersonationContext wi = Program.SetLocalImpersonationContext();

            bool isOk = true;
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
                        bcp.DestinationTableName = DatabasePrincipalPermissionDataTable.RepositoryTable;
                        bcp.BulkCopyTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                        // Create the datatable to write to the repository.
                        using (DataTable dataTable = DatabasePrincipalPermissionDataTable.Create())
                        {
                            // Process each uid in the uid list.
                            int uidcntr = uidList.Count;
                            List<int> uidbatch = new List<int>();
                            foreach (int uid in uidList)
                            {
                                // Add uid to the batch.
                                uidbatch.Add(uid);

                                // If batch count is at threshold query & process.
                                --uidcntr;
                                if (uidbatch.Count == Constants.PermissionBatchSize || uidcntr == 0)
                                {
                                    // Create the query based on the object.
                                    string query = createPrincipalPermissionQuery(database, uidbatch);
                                    Debug.Assert(!string.IsNullOrEmpty(query));

                                    // Clear the batch.
                                    uidbatch.Clear();

                                    // Query to get the column objects.
                                    using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(target, null,
                                                        CommandType.Text, query, null))
                                    {
                                        while (rdr.Read())
                                        {
                                            // Retrieve the values.
                                            SqlInt32 schemaid = rdr.GetSqlInt32(FieldUid);
                                            SqlInt32 grantor = rdr.GetSqlInt32(FieldGrantor);
                                            SqlInt32 grantee = rdr.GetSqlInt32(FieldGrantee);
                                            SqlInt32 classid = rdr.GetSqlInt32(FieldClassid);
                                            SqlString permission = rdr.GetSqlString(FieldPermission);
                                            SqlString isgrant = rdr.GetSqlString(FieldIsgrant);
                                            SqlString isgrantwith = rdr.GetSqlString(FieldIsgrantwith);
                                            SqlString isrevoke = rdr.GetSqlString(FieldIsrevoke);
                                            SqlString isdeny = rdr.GetSqlString(FieldIsdeny);

                                            // Update the datatable.
                                            DataRow dr = dataTable.NewRow();
                                            dr[DatabasePrincipalPermissionDataTable.ParamSnapshotid] = snapshotid;
                                            dr[DatabasePrincipalPermissionDataTable.ParamUid] = schemaid;
                                            dr[DatabasePrincipalPermissionDataTable.ParamGrantor] = grantor;
                                            dr[DatabasePrincipalPermissionDataTable.ParamGrantee] = grantee;
                                            dr[DatabasePrincipalPermissionDataTable.ParamDbid] = database.DbId;
                                            dr[DatabasePrincipalPermissionDataTable.ParamClassid] = classid;
                                            dr[DatabasePrincipalPermissionDataTable.ParamPermission] = permission;
                                            dr[DatabasePrincipalPermissionDataTable.ParamIsgrant] = isgrant;
                                            dr[DatabasePrincipalPermissionDataTable.ParamIsgrantwith] = isgrantwith;
                                            dr[DatabasePrincipalPermissionDataTable.ParamIsrevoke] = isrevoke;
                                            dr[DatabasePrincipalPermissionDataTable.ParamIsdeny] = isdeny;
                                            dr[DatabasePrincipalPermissionDataTable.ParamHashkey] = "";
                                            dataTable.Rows.Add(dr);

                                            // Keep counter of number of PermissionsCollected
                                            // ----------------------------------------------
                                            Target.numPermissionsCollected++;

                                            // Write to repository if exceeds threshold.
                                            if (dataTable.Rows.Count > Constants.RowBatchSize)
                                            {
                                                try
                                                {
                                                    bcp.WriteToServer(dataTable);
                                                    dataTable.Clear();
                                                }
                                                catch (SqlException ex)
                                                {
                                                    logX.loggerX.Error("ERROR - writing database principal permissions to Repository, ", ex);
                                                    isOk = false;
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
                                                logX.loggerX.Error("ERROR - writing database principal permissions to Repository, ", ex);
                                                isOk = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    logX.loggerX.Error("ERROR - exception encountered when processing database principal permissions, ", ex);
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

