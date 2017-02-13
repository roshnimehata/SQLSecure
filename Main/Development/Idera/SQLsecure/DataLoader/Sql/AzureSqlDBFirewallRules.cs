using Idera.SQLsecure.Core.Logger;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Idera.SQLsecure.Collector.Sql
{
    /// <summary>
    /// SQLSecure 3.1 (Anshul Aggarwal) - New model class to represent Firewall rules for Azure SQL Database. 
    /// </summary>
    public class AzureSqlDBFirewallRules
    {
        #region fields
        private static LogX logX = new LogX("Idera.SQLsecure.Collector.Sql.AzureSqlDBFirewallRules");
        private List<AzureSqlDBFirewallRule> firewallRules = new List<AzureSqlDBFirewallRule>();
        private int m_snapshotId;
        private const string SERVER_FIREWALL_QUERY = "select name, start_ip_address, end_ip_address from master.sys.firewall_rules";
        private const string DB_FIREWALL_QUERY = "select name, start_ip_address, end_ip_address from {0}.sys.database_firewall_rules ";
        private const int FieldName = 0;
        private const int FieldStartIPAddress = 1;
        private const int FieldEndIPAddress = 2;
        #endregion

        #region CTOR
        public AzureSqlDBFirewallRules(int snapshotID)
        {
            m_snapshotId = snapshotID;
        }
        #endregion

        /// <summary>
        /// SQLSecure 3.1 (Anshul Aggarwal) - Fetches firewall rules for target server and stores then in repository.
        /// </summary>
        public bool ProcessFirewallRules(string repositoryConnectionString, string targetConnectionString, List<Database> databases)
        {
            using (logX.loggerX.DebugCall())
            {
                Debug.Assert(!string.IsNullOrEmpty(targetConnectionString));
                bool isOk = true;
                // Check inputs.
                if (string.IsNullOrEmpty(targetConnectionString))
                {
                    string strMessage = "Invalid connection string";
                    logX.loggerX.Error("ERROR - " + strMessage);
                    isOk = false;
                }

                if (isOk)
                {
                    Program.ImpersonationContext wi = Program.SetTargetImpersonationContext();
                    try
                    {
                        using (SqlConnection target = new SqlConnection(targetConnectionString))
                        {
                            target.Open();
                            foreach (Database db in databases)
                            {
                                
                                if (db.DbId == int.MinValue)
                                {
                                    logX.loggerX.Warn(
                                            string.Format("Collection firewall rules. Incorrect dbid:{0} for db: {1}", db.DbId, db.Name));
                                    continue;
                                }

                                // Fetch server level firewall rules
                                if (db.Name == Constants.MASTER_DB_NAME)
                                {
                                    // Query to get the table objects.
                                    using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(target, null,
                                                        CommandType.Text, SERVER_FIREWALL_QUERY, null))
                                    {
                                        while (rdr.Read())
                                        {
                                            SqlString name = rdr.GetSqlString(FieldName);
                                            SqlString startIPAddress = rdr.GetSqlString(FieldStartIPAddress);
                                            SqlString endIPAddress = rdr.GetSqlString(FieldEndIPAddress);
                                            AzureSqlDBFirewallRule rule = new AzureSqlDBFirewallRule(
                                                name.Value,
                                                db.DbId, true,
                                                startIPAddress.Value,
                                                endIPAddress.Value);
                                            firewallRules.Add(rule);
                                        }
                                    }
                                }

                                // Query to get the table objects.
                                using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(target, null,
                                                    CommandType.Text, string.Format(DB_FIREWALL_QUERY, db.Name), null))
                                {
                                    while (rdr.Read())
                                    {
                                        SqlString name = rdr.GetSqlString(FieldName);
                                        SqlString startIPAddress = rdr.GetSqlString(FieldStartIPAddress);
                                        SqlString endIPAddress = rdr.GetSqlString(FieldEndIPAddress);
                                        if(name == null || startIPAddress == null || endIPAddress == null)
                                        {
                                            logX.loggerX.Warn(
                                                    string.Format("Incorrect firewall rules for db: {0} name: {1} startip: {2} endip: {3}", db, name, startIPAddress, endIPAddress));
                                            continue;
                                        }
                                        AzureSqlDBFirewallRule rule = new AzureSqlDBFirewallRule(
                                            name.Value,
                                            db.DbId, false,
                                            startIPAddress.Value,
                                            endIPAddress.Value);
                                        firewallRules.Add(rule);
                                    }
                                }
                            }
                          }

                        // Wriye firewall rules to db.
                        WriteFirewallRulesToRepository(repositoryConnectionString);
                    }
                    catch (Exception ex)
                    {
                        isOk = false;
                        logX.loggerX.Error(string.Format("Error Getting Firewall Rules: {0}", ex.Message));
                    }
                    finally
                    {
                        Program.RestoreImpersonationContext(wi);
                    }
                }
                return isOk;
            }
        }

        /// <summary>
        /// SQLSecure 3.1 (Anshul Aggarwal) - Stores firewall rules in repository.
        /// </summary>
        private void WriteFirewallRulesToRepository(string repositoryConnectionString)
        {
            Program.ImpersonationContext wi = Program.SetLocalImpersonationContext();
            try
            {
                using (logX.loggerX.DebugCall())
                {
                    using (SqlConnection repository = new SqlConnection(repositoryConnectionString))
                    {
                        // Open repository connection.
                        repository.Open();
                        // Use bulk copy object to write to repository.
                        using (SqlBulkCopy bcpFirewallRules = new SqlBulkCopy(repository))
                        {
                            // Set the destination table.
                            bcpFirewallRules.DestinationTableName = AzureSqlDBFirewallRulesObjectTable.RepositoryTable;
                            bcpFirewallRules.BulkCopyTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();

                            // Create the datatable to write to the repository.
                            using (DataTable dataTableObject = AzureSqlDBFirewallRulesObjectTable.Create())
                            {
                                foreach (AzureSqlDBFirewallRule fp in firewallRules)
                                {
                                    // Update the datatable.
                                    DataRow dr = dataTableObject.NewRow();
                                    dr[AzureSqlDBFirewallRulesObjectTable.ParamSnapshotid] = m_snapshotId;
                                    dr[AzureSqlDBFirewallRulesObjectTable.ParamIsServerLevel] = fp.IsServerLevel;
                                    dr[AzureSqlDBFirewallRulesObjectTable.ParamName] = fp.Name;
                                    dr[AzureSqlDBFirewallRulesObjectTable.ParamDBId] = fp.DBId;
                                    dr[AzureSqlDBFirewallRulesObjectTable.ParamStartIPAddress] = fp.StartIPAddress;
                                    dr[AzureSqlDBFirewallRulesObjectTable.ParamEndIPAddress] = fp.EndIPAddress;
                                    dataTableObject.Rows.Add(dr);
                                    if (dataTableObject.Rows.Count >= Constants.RowBatchSize)
                                    {
                                        bcpFirewallRules.WriteToServer(dataTableObject);
                                        dataTableObject.Rows.Clear();
                                    }
                                }
                                
                                if (dataTableObject.Rows.Count > 0)
                                {
                                    bcpFirewallRules.WriteToServer(dataTableObject);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error("WriteFirewallRulesToRepository failed: ", ex.Message);
                throw;
            }
            finally
            {
                Program.RestoreImpersonationContext(wi);
            }
        }

    }
}
