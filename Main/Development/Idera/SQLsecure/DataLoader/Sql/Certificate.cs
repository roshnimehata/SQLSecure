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
    public class Certificate
    {
        private static LogX _logX = new LogX("Idera.SQLsecure.Collector.Sql.Certificate");

        private const string query = @"SELECT name ,
                               certificate_id ,
                               principal_id ,
                               pvt_key_encryption_type ,
                               pvt_key_encryption_type_desc ,
                               is_active_for_begin_dialog ,
                               issuer_name ,
                               cert_serial_number ,
                               sid ,
                               string_sid ,
                               subject ,
                               expiry_date ,
                               start_date ,
                               thumbprint ,
                               attested_by ,
                               pvt_key_last_backup_date 
                           FROM {0}.sys.certificates";

        private const string query2005 = @"SELECT name ,
                               certificate_id ,
                               principal_id ,
                               pvt_key_encryption_type ,
                               pvt_key_encryption_type_desc ,
                               is_active_for_begin_dialog ,
                               issuer_name ,
                               cert_serial_number ,
                               sid ,
                               string_sid ,
                               subject ,
                               expiry_date ,
                               start_date ,
                               thumbprint ,
                               attested_by ,
                               NULL AS pvt_key_last_backup_date 
                           FROM {0}.sys.certificates";


        public static string CreateQuery(Database database, ServerVersion serverVersion)
        {
            return String.Format(serverVersion <= ServerVersion.SQL2005 ? query2005 : query, database.Name);
        }

        public static bool Process(string targetConnection,
           string repositoryConnection, int snapshotid,
           Database database,
           ref Dictionary<Sql.SqlObjectType, Dictionary<MetricMeasureType, uint>> metricsData)
        {
            bool isOk = true;


            Debug.Assert(!string.IsNullOrEmpty(targetConnection));
            Debug.Assert(!string.IsNullOrEmpty(repositoryConnection));
            Debug.Assert(database != null);
            Program.ImpersonationContext wi = Program.SetLocalImpersonationContext();
            try
            {
                //Do the main job
                uint processedCertificatesCnt = 0;
                Stopwatch sw = new Stopwatch();
                sw.Start();
                using (SqlConnection target = new SqlConnection(targetConnection),
                    repository = new SqlConnection(repositoryConnection))
                {
                    // Open repository and target connections.
                    repository.Open();
                    Program.SetTargetSQLServerImpersonationContext();
                    target.Open();

                    var sqlServerVersion = Sql.SqlHelper.ParseVersion(target.ServerVersion);

                    // Use bulk copy object to write to repository.
                    using (SqlBulkCopy bcp = new SqlBulkCopy(repository))
                    {
                        bcp.DestinationTableName = CertificateTable.RepositoryTable;
                        bcp.BulkCopyTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();


                        using (DataTable dataTable = CertificateTable.Create())
                        {
                            var query = CreateQuery(database, sqlServerVersion);
                            using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(target, null,
                                CommandType.Text, query, null))
                            {
                                while (rdr.Read())
                                {
                                    // Retrieve the object information.

                                    SqlString certName = rdr.GetSqlString((int)DatabaseCertificateColumns.CertificateName);
                                    SqlInt32 certificateId = rdr.IsDBNull((int)DatabaseCertificateColumns.CertificateId) ? SqlInt32.Null : rdr.GetSqlInt32((int)DatabaseCertificateColumns.CertificateId);
                                    SqlInt32 principalId = rdr.IsDBNull((int)DatabaseCertificateColumns.PrincipalId) ? SqlInt32.Null : rdr.GetSqlInt32((int)DatabaseCertificateColumns.PrincipalId);
                                    SqlString keyEncryptionType = rdr.GetSqlString((int)DatabaseCertificateColumns.KeyEncryptionType);
                                    SqlString keyEncryptionTypeDesc = rdr.GetSqlString((int)DatabaseCertificateColumns.KeyEncryptionTypeDesc);
                                    SqlBoolean isActiveForBeginDialog = rdr.GetSqlBoolean((int)DatabaseCertificateColumns.IsActiveForBeginDialog);
                                    SqlString issuerName = rdr.GetSqlString((int)DatabaseCertificateColumns.IssuerName);
                                    SqlString certSerialNumber = rdr.GetSqlString((int)DatabaseCertificateColumns.CertSerialNumber);
                                    var sid = rdr.GetSqlBinary((int)DatabaseCertificateColumns.Sid);
                                    SqlString stringSid = rdr.GetSqlString((int)DatabaseCertificateColumns.StringSid);
                                    SqlString subject = rdr.GetSqlString((int)DatabaseCertificateColumns.Subject);
                                    SqlDateTime expiryDate = rdr.GetSqlDateTime((int)DatabaseCertificateColumns.ExpiryDate);
                                    SqlDateTime startDate = rdr.GetSqlDateTime((int)DatabaseCertificateColumns.StartDate);
                                    var thumbprint = rdr.GetSqlBinary((int)DatabaseCertificateColumns.Thumbprint);
                                    SqlString attestedBy = rdr.GetSqlString((int)DatabaseCertificateColumns.AttestedBy);
                                    SqlDateTime keyLastBackupDate = rdr.GetSqlDateTime((int)DatabaseCertificateColumns.KeyLastBackupDate);


                                    // Update the datatable.
                                    var dataRow = dataTable.NewRow();
                                    dataRow[CertificateTable.SnapshotId] = snapshotid;
                                    dataRow[CertificateTable.DbId] = database.DbId;
                                    dataRow[CertificateTable.CertificateName] = certName;
                                    dataRow[CertificateTable.CertificateId] = certificateId;
                                    dataRow[CertificateTable.PrincipalId] = principalId;
                                    dataRow[CertificateTable.KeyEncryptionType] = keyEncryptionType;
                                    dataRow[CertificateTable.KeyEncryptionTypeDesc] = keyEncryptionTypeDesc;
                                    dataRow[CertificateTable.IsActiveForBeginDialog] = isActiveForBeginDialog;
                                    dataRow[CertificateTable.IssuerName] = issuerName;
                                    dataRow[CertificateTable.CertSerialNumber] = certSerialNumber;
                                    dataRow[CertificateTable.Sid] = sid;
                                    dataRow[CertificateTable.StringSid] = stringSid;
                                    dataRow[CertificateTable.Subject] = subject;
                                    dataRow[CertificateTable.ExpiryDate] = expiryDate;
                                    dataRow[CertificateTable.StartDate] = startDate;
                                    dataRow[CertificateTable.Thumbprint] = thumbprint;
                                    dataRow[CertificateTable.AttestedBy] = attestedBy;
                                    dataRow[CertificateTable.KeyLastBackupDate] = keyLastBackupDate;
                                    processedCertificatesCnt++;
                                    dataTable.Rows.Add(dataRow);

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


                sw.Stop();


                //ToDo: Check if we need this
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
                de[MetricMeasureType.Count] = processedCertificatesCnt + oldMetricCount;
                de[MetricMeasureType.Time] = (uint)sw.ElapsedMilliseconds + oldMetricTime;
                metricsData[SqlObjectType.Schema] = de;
            }
            catch (Exception ex)
            {
                _logX.loggerX.Error(ex.Message);
                string strMessage = "Processing certificates";
                _logX.loggerX.Error("ERROR - " + strMessage, ex);
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
            return isOk;
        }
    }



    internal enum DatabaseCertificateColumns
    {

        CertificateName,
        CertificateId,
        PrincipalId,
        KeyEncryptionType,
        KeyEncryptionTypeDesc,
        IsActiveForBeginDialog,
        IssuerName,
        CertSerialNumber,
        Sid,
        StringSid,
        Subject,
        ExpiryDate,
        StartDate,
        Thumbprint,
        AttestedBy,
        KeyLastBackupDate
    }
}
