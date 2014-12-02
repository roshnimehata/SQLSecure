/******************************************************************
 * Name: DatabasePrincipal.cs
 *
 * Description: Encapsulates database principal objects.
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
    /// SQL Server database principal object.
    /// </summary>
    class DatabasePrincipal
    {
        private static LogX logX = new LogX("Idera.SQLsecure.Collector.Sql.DatabasePrincipal");
        private const int FieldPrincipalName = 0;
        private const int FieldPrincipalUid = 1;
        private const int FieldPrincipalType = 2;
        private const int FieldPrincipalUsersid = 3;
        private const int FieldPrincipalIsalias = 4;
        private const int FieldPrincipalAltuid = 5;
        private const int FieldPrincipalHasaccess = 6;
        private const int FieldPrincipalOwner = 7;
        private const int FieldPrincipalDefaultschemaname = 8;
        private const int FieldIsContainedUser = 9;
        private const int FieldAuthenticationType = 10;

        private static string createPrincipalQuery(
                ServerVersion version,
                Database database
            )
        {
            Debug.Assert(version != ServerVersion.Unsupported);
            Debug.Assert(database != null);

            string query = null;

            // Create query based on the SQL Server version.
            if (version == ServerVersion.SQL2000)
            {
                query = @"SELECT 
                            name, 
                            uid = CAST(uid AS int), 
                            type = CASE 
	                                    WHEN islogin = 1 AND isntname = 0 AND issqluser = 1 THEN 'S'
	                                    WHEN islogin = 1 AND isntname = 1 AND isntgroup = 1 THEN 'G'
                                        WHEN islogin = 1 AND isntname = 1 AND isntuser = 1 THEN 'U'
                                        WHEN isapprole = 1 THEN 'A'
                                        ELSE 'R'
                                   END, 
                            usersid = sid, 
                            isalias = CASE WHEN isaliased = 1 THEN 'Y' ELSE 'N' END, 
                            altuid = CAST(altuid AS int), 
                            hasaccess = CASE WHEN hasdbaccess = 1 THEN 'Y' ELSE 'N' END,
                            owner = CAST (NULL AS int),
                            defaultschemaname = CAST (NULL AS NVARCHAR),"
                             + "isContainedUser =cast (0 as bit) "
                             + "authenticationtype= 'NOT SUPPORTED' "
                      + @"FROM " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + @".dbo.sysusers";
            }
            else if (version < ServerVersion.SQL2012)
            {


                query = @"SELECT
                            dp.name, 
                            uid = dp.principal_id, 
                            dp.type, 
                            usersid = dp.sid, 
                            isalias = 'N', 
                            altuid = CAST(su.altuid AS int),
                            hasaccess = CASE WHEN su.hasdbaccess = 1 THEN 'Y' ELSE 'N' END,
                            owner = dp.owning_principal_id, 
                            defaultschemaname = dp.default_schema_name, 
                            isContainedUser = cast(0 as bit) ,"
                    + "authenticationtype= 'NOT SUPPORTED' "
                      + @"FROM " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + @".sys.database_principals AS dp JOIN "
                            + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + @".sys.sysusers AS su ON (dp.principal_id = su.uid) "
                      + @"UNION ALL SELECT
                            name, 
                            uid = CAST(uid AS int), 
                            type = CASE 
	                                    WHEN islogin = 1 AND isntname = 0 AND issqluser = 1 THEN 'S'
	                                    WHEN islogin = 1 AND isntname = 1 AND isntgroup = 1 THEN 'G'
                                        WHEN islogin = 1 AND isntname = 1 AND isntuser = 1 THEN 'U'
                                        WHEN isapprole = 1 THEN 'A'
                                        ELSE 'R'
                                   END, 
                            usersid = sid, 
                            isalias = CASE WHEN isaliased = 1 THEN 'Y' ELSE 'N' END, 
                            altuid = CAST(altuid AS int), 
                            hasaccess = CASE WHEN hasdbaccess = 1 THEN 'Y' ELSE 'N' END,
                            owner = CAST (NULL AS int),
                            defaultschemaname = CAST (NULL AS NVARCHAR), "
                      + "isContainedUser = cast(0 as bit),"
                      + "authenticationtype= 'NOT SUPPORTED' "
                      + @"FROM " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + @".sys.sysusers "
                      + @"WHERE isaliased = 1";
            }
            else if (version >= ServerVersion.SQL2012)
            {
                query = @"SELECT
                            dp.name, 
                            uid = dp.principal_id, 
                            dp.type, 
                            usersid = dp.sid, 
                            isalias = 'N', 
                            altuid = CAST(su.altuid AS int),
                            hasaccess = CASE WHEN su.hasdbaccess = 1 THEN 'Y' ELSE 'N' END,
                            owner = dp.owning_principal_id, 
                            defaultschemaname = dp.default_schema_name, 
                            isContainedUser = cast(( case authentication_type
                               when 2 then 1
                               when 3 then  isnull((select top 1 0 from " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + @".sys.database_principals cdp
							    where cdp.principal_id=dp.principal_id and cdp.sid  in (SELECT sid FROM " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + @".sys.server_principals) 
								and    type in ( 'U', 'S', 'G' )
    and name not in ( 'dbo', 'guest', 'INFORMATION_SCHEMA', 'sys' )
								), 1)
								else 0
                             end ) as bit), " +
                       " authenticationtype = authentication_type_desc "
                      + @"FROM " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + @".sys.database_principals AS dp JOIN "
                            + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + @".sys.sysusers AS su ON (dp.principal_id = su.uid) "
                      + @"UNION ALL SELECT
                            name, 
                            uid = CAST(uid AS int), 
                            type = CASE 
	                                    WHEN islogin = 1 AND isntname = 0 AND issqluser = 1 THEN 'S'
	                                    WHEN islogin = 1 AND isntname = 1 AND isntgroup = 1 THEN 'G'
                                        WHEN islogin = 1 AND isntname = 1 AND isntuser = 1 THEN 'U'
                                        WHEN isapprole = 1 THEN 'A'
                                        ELSE 'R'
                                   END, 
                            usersid = sid, 
                            isalias = CASE WHEN isaliased = 1 THEN 'Y' ELSE 'N' END, 
                            altuid = CAST(altuid AS int), 
                            hasaccess = CASE WHEN hasdbaccess = 1 THEN 'Y' ELSE 'N' END,
                            owner = CAST (NULL AS int),
                            defaultschemaname = CAST (NULL AS NVARCHAR), "
                      + "isContainedUser = cast(0 as bit) ,"
                      + "authenticationtype= 'NOT SUPPORTED' "
                      + @"FROM " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + @".sys.sysusers "
                      + @"WHERE isaliased = 1";
            }



            return query;
        }

        private static string createRoleMemberQuery(
                ServerVersion version,
                Database database
            )
        {
            Debug.Assert(version != ServerVersion.Unsupported);
            Debug.Assert(database != null);

            string query = null;

            // Create query based on the SQL Server version.
            if (version == ServerVersion.SQL2000)
            {
                query = @"EXEC " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + ".dbo.sp_helprolemember";
            }
            else
            {
                query = @"SELECT * FROM " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + ".sys.database_role_members";
            }

            return query;
        }

        private static bool isUserPrincipal(string type)
        {
            Debug.Assert(!string.IsNullOrEmpty(type));

            return (string.Compare(type, "S", true) == 0 || string.Compare(type, "U", true) == 0 || string.Compare(type, "G", true) == 0);
        }

        private static bool processMembers(
                ServerVersion version,
                string targetConnection,
                string repositoryConnection,
                int snapshotid,
                Database database,
                Dictionary<string, KeyValuePair<int, string>> nameDictionary
            )
        {
            Debug.Assert(version != ServerVersion.Unsupported);
            Debug.Assert(!string.IsNullOrEmpty(targetConnection));
            Debug.Assert(!string.IsNullOrEmpty(repositoryConnection));
            Debug.Assert(database != null);
            Debug.Assert(nameDictionary != null);
            Program.ImpersonationContext wi = Program.SetLocalImpersonationContext();
            // Process database role members.
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
                        bcp.DestinationTableName = DatabaseRoleMemberDataTable.RepositoryTable;
                        bcp.BulkCopyTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                        // Create the datatable to write to the repository.
                        using (DataTable dataTable = DatabaseRoleMemberDataTable.Create())
                        {
                            // Create the query.
                            string query = createRoleMemberQuery(version, database);
                            Debug.Assert(!string.IsNullOrEmpty(query));

                            // Query to get the role member objects.
                            // Note : this query does not return public role members, so we
                            // have to do special processing for public role members.  See below.
                            using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(target, null,
                                                CommandType.Text, query, null))
                            {
                                while (rdr.Read())
                                {
                                    // Retrieve and setup the table row.
                                    if (version == ServerVersion.SQL2000)
                                    {
                                        KeyValuePair<int, string> role, member;
                                        if (nameDictionary.TryGetValue((string)rdr[0], out role)
                                            && nameDictionary.TryGetValue((string)rdr[1], out member))
                                        {
                                            DataRow dr = dataTable.NewRow();
                                            dr[DatabaseRoleMemberDataTable.ParamDbid] = database.DbId;
                                            dr[DatabaseRoleMemberDataTable.ParamGroupuid] = role.Key;
                                            dr[DatabaseRoleMemberDataTable.ParamRolememberuid] = member.Key;
                                            dr[DatabaseRoleMemberDataTable.ParamSnapshotid] = snapshotid;
                                            dr[DatabaseRoleMemberDataTable.ParamHashkey] = "";
                                            dataTable.Rows.Add(dr);
                                        }
                                        else
                                        {
                                            logX.loggerX.Warn("WARN - uid not found for db role member", (string)rdr[0], ", or ", (string)rdr[1]);
                                        }
                                    }
                                    else
                                    {
                                        DataRow dr = dataTable.NewRow();
                                        dr[DatabaseRoleMemberDataTable.ParamDbid] = database.DbId;
                                        dr[DatabaseRoleMemberDataTable.ParamGroupuid] = rdr.GetSqlInt32(0);
                                        dr[DatabaseRoleMemberDataTable.ParamRolememberuid] = rdr.GetSqlInt32(1);
                                        dr[DatabaseRoleMemberDataTable.ParamSnapshotid] = snapshotid;
                                        dr[DatabaseRoleMemberDataTable.ParamHashkey] = "";
                                        dataTable.Rows.Add(dr);
                                    }

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
                                            logX.loggerX.Error("ERROR - writing database role members to Repository, ", ex);
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
                                        logX.loggerX.Error("ERROR - writing database role members to Repository, ", ex);
                                        isOk = false;
                                    }
                                }
                            }

                            // Now write all SQL user, windows user and windows group as a
                            // member of the public role.   Note: the queries used above
                            // do not return any public role members.   So we have to do
                            // special processing here.
                            const int PublicRoleUid = 0;
                            dataTable.Clear();
                            foreach (KeyValuePair<int, string> dbprincipal in nameDictionary.Values)
                            {
                                // Add row to the data table, if a user.
                                if (isUserPrincipal(dbprincipal.Value))
                                {
                                    DataRow dr = dataTable.NewRow();
                                    dr[DatabaseRoleMemberDataTable.ParamDbid] = database.DbId;
                                    dr[DatabaseRoleMemberDataTable.ParamGroupuid] = PublicRoleUid;
                                    dr[DatabaseRoleMemberDataTable.ParamRolememberuid] = dbprincipal.Key;
                                    dr[DatabaseRoleMemberDataTable.ParamSnapshotid] = snapshotid;
                                    dr[DatabaseRoleMemberDataTable.ParamHashkey] = "";
                                    dataTable.Rows.Add(dr);
                                }

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
                                        logX.loggerX.Error("ERROR - writing database role members to Repository, ", ex);
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
                                    logX.loggerX.Error("ERROR - writing database role members to Repository, ", ex);
                                    isOk = false;
                                }
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    logX.loggerX.Error("ERROR - exception encountered when processing database role members, ", ex);
                    isOk = false;
                }
                finally
                {
                    Program.RestoreImpersonationContext(wi);
                }
            }

            return isOk;
        }

        public static bool Process(
                ServerVersion version,
                string targetConnection,
                string repositoryConnection,
                int snapshotid,
                Database database,
                out bool isGuestEnabled,
                ref Dictionary<Sql.SqlObjectType, Dictionary<MetricMeasureType, uint>> metricsData
            )
        {
            Debug.Assert(version != ServerVersion.Unsupported);
            Debug.Assert(!string.IsNullOrEmpty(targetConnection));
            Debug.Assert(!string.IsNullOrEmpty(repositoryConnection));
            Debug.Assert(database != null);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            uint numProcessedUsers = 0;

            // Init return.
            bool isOk = true;
            isGuestEnabled = false;
            Program.ImpersonationContext wi = Program.SetLocalImpersonationContext();

            // Process database users.
            List<int> uidList = new List<int>();
            Dictionary<string, KeyValuePair<int, string>> nameDictionary = new Dictionary<string, KeyValuePair<int, string>>();
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
                        bcp.DestinationTableName = DatabasePrincipalDataTable.RepositoryTable;
                        bcp.BulkCopyTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                        // Create the datatable to write to the repository.
                        using (DataTable dataTable = DatabasePrincipalDataTable.Create())
                        {
                            // Create the query.
                            string query = createPrincipalQuery(version, database);
                            Debug.Assert(!string.IsNullOrEmpty(query));

                            // Query to get the table objects.
                            using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(target, null,
                                                CommandType.Text, query, null))
                            {
                                while (rdr.Read())
                                {
                                    // Retrieve information.
                                    SqlString name = rdr.GetSqlString(FieldPrincipalName);
                                    SqlInt32 uid = rdr.GetSqlInt32(FieldPrincipalUid);
                                    SqlString type = rdr.GetSqlString(FieldPrincipalType);
                                    SqlBinary usersid = rdr.GetSqlBinary(FieldPrincipalUsersid);
                                    SqlString isalias = rdr.GetSqlString(FieldPrincipalIsalias);
                                    SqlInt32 altuid = rdr.GetSqlInt32(FieldPrincipalAltuid);
                                    SqlString hasaccess = rdr.GetSqlString(FieldPrincipalHasaccess);
                                    SqlInt32 owner = rdr.GetSqlInt32(FieldPrincipalOwner);
                                    SqlString defaultSchemaName = rdr.GetSqlString(FieldPrincipalDefaultschemaname);
                                    SqlBoolean isContained = database.IsContained && rdr.GetBoolean(FieldIsContainedUser);
                                    SqlString authenticationType = rdr.GetString(FieldAuthenticationType);

                                    // Add to uid collection for later permission processing.
                                    Debug.Assert(!uid.IsNull);
                                    uidList.Add(uid.Value);

                                    // Add to name dictionary for SQL 2000 role member processing & public role processing.
                                    Debug.Assert(!name.IsNull);
                                    Debug.Assert(!type.IsNull);
                                    nameDictionary.Add(name.Value, new KeyValuePair<int, string>(uid.Value, type.Value));

                                    // If guest account set guest enabled flag.
                                    if (uid.Value == Constants.GuestUser && string.Compare(hasaccess.Value, "Y", true) == 0)
                                    {
                                        isGuestEnabled = true;
                                    }

                                    // Update the datatable.
                                    DataRow dr = dataTable.NewRow();
                                    dr[DatabasePrincipalDataTable.ParamSnapshotid] = snapshotid;
                                    dr[DatabasePrincipalDataTable.ParamOwner] = owner;
                                    dr[DatabasePrincipalDataTable.ParamDbid] = database.DbId;
                                    dr[DatabasePrincipalDataTable.ParamUid] = uid;
                                    dr[DatabasePrincipalDataTable.ParamName] = name;
                                    dr[DatabasePrincipalDataTable.ParamUsersid] = usersid;
                                    dr[DatabasePrincipalDataTable.ParamType] = type;
                                    dr[DatabasePrincipalDataTable.ParamIsalias] = isalias;
                                    dr[DatabasePrincipalDataTable.ParamAltuid] = altuid;
                                    dr[DatabasePrincipalDataTable.ParamHasaccess] = hasaccess;
                                    dr[DatabasePrincipalDataTable.ParamDefaultschemaname] = defaultSchemaName;
                                    dr[DatabasePrincipalDataTable.ParamHashkey] = "";
                                    dr[DatabasePrincipalDataTable.ParamIsContained] = isContained;
                                    dr[DatabasePrincipalDataTable.ParamAuthenticationType] = authenticationType;

                                    dataTable.Rows.Add(dr);

                                    numProcessedUsers++;

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
                                            string strMessage = "Writing database principals to Repository ";
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
                                        string strMessage = "Writing database principals to Repository ";
                                        logX.loggerX.Error("ERROR - " + strMessage, ex);
                                        throw ex;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    string strMessage = "Processing database principals";
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

            // Process role memberships.
            if (isOk)
            {
                if (!processMembers(version, targetConnection, repositoryConnection, snapshotid, database, nameDictionary))
                {
                    logX.loggerX.Error("ERROR - error encountered in processing database role members");
                    isOk = false;
                }
            }

            // Load principal permissions, if its 2005.
            if (isOk)
            {
                if (version != ServerVersion.SQL2000)
                {
                    if(!DatabasePrincipalPermission.Process(targetConnection, repositoryConnection, snapshotid, database, uidList))
                    {
                        logX.loggerX.Error("ERROR - error encountered in processing  database principal permissions");
                        isOk = false;
                    }
                }
            }

            uint oldMetricCount = 0;
            uint oldMetricTime = 0;
            sw.Stop();
            // See if User is already in Metrics Dictionary
            // ----------------------------------------------
            Dictionary<MetricMeasureType, uint> de;
            if (metricsData.TryGetValue(SqlObjectType.User, out de))
            {
                de.TryGetValue(MetricMeasureType.Count, out oldMetricCount);
                de.TryGetValue(MetricMeasureType.Time, out oldMetricTime);
            }
            else
            {
                de = new Dictionary<MetricMeasureType, uint>();
            }
            de[MetricMeasureType.Count] = numProcessedUsers + oldMetricCount;
            de[MetricMeasureType.Time] = (uint)sw.ElapsedMilliseconds + oldMetricTime;
            metricsData[SqlObjectType.User] = de;


            return isOk;
        }
    } 
}
