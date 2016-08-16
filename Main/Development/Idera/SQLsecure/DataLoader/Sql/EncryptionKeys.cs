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
    public class EncryptionKeys
    {
        private static LogX logX = new LogX("Idera.SQLsecure.Collector.Sql.EncryptionKeys");

        public const string GetKeysQuery2008AndAbove = @"SELECT  name ,
                                                principal_id ,
                                                symmetric_key_id ,
                                                key_length ,
                                                key_algorithm ,
                                                algorithm_desc ,
                                                provider_type,
	                                            'iSK' as type
                                        FROM    {0}.sys.symmetric_keys
                                        UNION
                                        SELECT  name ,
                                                principal_id ,
                                                asymmetric_key_id ,
                                                key_length ,
                                                algorithm ,
                                                algorithm_desc ,
                                                provider_type,
                                                'iAK' as type
                                        FROM    {0}.sys.asymmetric_keys;";

        public const string GetKeysQuery2005 = @"SELECT  name ,
                                                principal_id ,
                                                symmetric_key_id ,
                                                key_length ,
                                                key_algorithm ,
                                                algorithm_desc ,
                                               '' provider_type,
	                                            'iSK' as type
                                        FROM    {0}.sys.symmetric_keys
                                        UNION
                                        SELECT  name ,
                                                principal_id ,
                                                asymmetric_key_id ,
                                                key_length ,
                                                algorithm ,
                                                algorithm_desc ,
                                                '' provider_type,
                                                'iAK' as type
                                        FROM    {0}.sys.asymmetric_keys;";





        internal const int ColName = 0;
        internal const int ColPrincipalId = 1;
        internal const int ColDbKeyId = 2;
        internal const int ColKeyLength = 3;
        internal const int ColAlgorithm = 4;
        internal const int ColAlgorithmDesc = 5;
        internal const int ColProviderType = 6;
        internal const int ColType = 7;

        private static string GetQuery(ServerVersion version)
        {
            if (version == ServerVersion.SQL2000)
                return string.Empty;
            if (version == ServerVersion.SQL2005)
                return GetKeysQuery2005;
            return GetKeysQuery2008AndAbove;
        }


        public static bool Process(ServerVersion version,
            string targetConnection,
            string repositoryConnection,
            int snapshotid,
            int databaseId, string databaseName)
        {
            Debug.Assert(version != ServerVersion.Unsupported);
            Debug.Assert(!string.IsNullOrEmpty(targetConnection));
            Debug.Assert(!string.IsNullOrEmpty(repositoryConnection));

            bool isOk = true;
            targetConnection = Sql.SqlHelper.AppendDatabaseToConnectionString(targetConnection, "master");
            Program.ImpersonationContext wi = Program.SetLocalImpersonationContext();
            if (version == ServerVersion.SQL2000) return true;

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
                        bcp.DestinationTableName = EncryptionKeyDataTable.RepositoryTable;
                        bcp.BulkCopyTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                        // Create the datatable to write to the repository.
                        using (DataTable dataTable = EncryptionKeyDataTable.Create())
                        {
                            // Process each rule to collect the table objects.

                            string query = string.Format(GetQuery(version), databaseName);


                            Debug.Assert(!string.IsNullOrEmpty(query));

                            // Query to get the table objects.
                            using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(target, null, CommandType.Text, query, null))
                            {
                                while (rdr.Read())
                                {
                                    // Retrieve the object information.

                                    SqlString name = rdr.GetSqlString(ColName);
                                    SqlString algorithm = rdr.GetSqlString(ColAlgorithm);
                                    SqlString algorithmDesc = rdr.GetSqlString(ColAlgorithmDesc);
                                    SqlString provType = rdr.GetSqlString(ColProviderType);
                                    SqlString type = rdr.GetSqlString(ColType);
                                    SqlInt32 principalId = rdr.IsDBNull(ColPrincipalId) ? SqlInt32.Null : rdr.GetInt32(ColPrincipalId);
                                    SqlInt32 dbId = rdr.IsDBNull(ColDbKeyId) ? SqlInt32.Null : rdr.GetInt32(ColDbKeyId);
                                    SqlInt32 keyLength = rdr.IsDBNull(ColKeyLength) ? SqlInt32.Null : rdr.GetInt32(ColKeyLength);

                                    //todo add database id 


                                    DataRow dr = dataTable.NewRow();
                                    dr[EncryptionKeyDataTable.ParamSnapshotId] = snapshotid;
                                    dr[EncryptionKeyDataTable.ParamName] = name;
                                    dr[EncryptionKeyDataTable.ParamAlgorithm] = algorithm;
                                    dr[EncryptionKeyDataTable.ParamAlgorithmDesc] = algorithmDesc;
                                    dr[EncryptionKeyDataTable.ParamProviderType] = provType;
                                    dr[EncryptionKeyDataTable.ParamPrincipalId] = principalId;
                                    dr[EncryptionKeyDataTable.ParamDbKeyId] = dbId;
                                    dr[EncryptionKeyDataTable.ParamKeyLength] = keyLength;
                                    dr[EncryptionKeyDataTable.ParamDatabaseId] = databaseId;
                                    dr[EncryptionKeyDataTable.ParamType] = type;

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
                                            string strMessage = "Writing to Repository sql server keys faild";
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
                                        string strMessage = "Writing to Repository sql server keys faild";
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
                    string strMessage = "Processing sql server keys failed";
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
