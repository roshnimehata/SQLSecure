/******************************************************************
 * Name: SQLServer.cs
 *
 * Description: Encapsulates an unregistered SQL Server instance.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.SQL;

namespace Idera.SQLsecure.UI.Console.Sql
{
    /// <summary>
    /// Encapsulates unregistered SQL Server instance
    /// </summary>
    public static class SqlServer
    {
        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Sql.SqlServer");

        public static void GetSqlServerProperties(
                string instance,
                string sqlLogin,
                string sqlPassword,
                out string version,
                out string machineName,
                out string instanceName,
                out string fullName
            )
        {
            // Init return.
            version = string.Empty;
            machineName = string.Empty;
            instanceName = string.Empty;
            fullName = string.Empty;

            var serverProperties = GetSqlServerProperties(instance, sqlLogin, sqlPassword);
            var isServerInAoag = serverProperties.HadrManagerStatus == HadrManagerStatus.StartedAndRunning;

            if (isServerInAoag)//todo check this 
            {
                var nodeProperties = GetSqlServerProperties(serverProperties.ServerName, sqlLogin, sqlPassword);
                var isWhantConnectToTheClusterNotToTheNode = nodeProperties.LocalNetAddress != serverProperties.LocalNetAddress;

                if (isWhantConnectToTheClusterNotToTheNode)
                {
                    version = serverProperties.Version;
                    instanceName = serverProperties.InstanceName;
                    fullName =
                    machineName = serverProperties.HadrClusterName;
                    return;
                }
            }

            version = serverProperties.Version;
            machineName = serverProperties.MachineName;
            instanceName = serverProperties.InstanceName;
            fullName = serverProperties.ServerName;
        }

        public static SQLServerProperties GetSqlServerProperties(string instance, string sqlLogin, string sqlPassword)
        {
            Debug.Assert(!string.IsNullOrEmpty(instance));

            // Validate input.
            if (string.IsNullOrEmpty(instance))
            {
                logX.loggerX.Error("ERROR - no instance specified for getting SQL Server properties");
                throw new ArgumentNullException("Instance is not specified");
            }

            instance = instance.Trim();
            var result = new SQLServerProperties();
            var bldr = SqlHelper.ConstructConnectionString(instance, sqlLogin, sqlPassword);

            using (var connection = new SqlConnection(bldr.ConnectionString))
            {
                connection.Open();

                var confQuery = @"select isnull(CONNECTIONPROPERTY('local_net_address'),'') AS LocalNetAddress, 
                                          isnull(SERVERPROPERTY('HadrManagerStatus'),0) as HadrManagerStatus,
                                          isnull(SERVERPROPERTY('MachineName'),'')  as MachineName,
                                          isnull(SERVERPROPERTY('ServerName'),'') as ServerName,
                                          isnull(SERVERPROPERTY('InstanceName'),'') as InstanceName;

                                   SELECT top 1 cluster_name as HadrClusterName FROM  sys.dm_hadr_cluster;";
                using (var rdr = SqlHelper.ExecuteReader(connection, null, CommandType.Text, confQuery, null))
                {
                    if (rdr.HasRows && rdr.Read())
                    {
                        result.Version = connection.ServerVersion;
                        result.InstanceName = rdr["InstanceName"].ToString();
                        result.MachineName = rdr["MachineName"].ToString();
                        result.ServerName = rdr["ServerName"].ToString();
                        result.LocalNetAddress = rdr["LocalNetAddress"].ToString();
                        result.HadrManagerStatus = GetHadrManagerStatus(rdr["HadrManagerStatus"].ToString());

                        if (result.HadrManagerStatus == HadrManagerStatus.StartedAndRunning &&
                            rdr.NextResult() &&
                            rdr.HasRows &&
                            rdr.Read())
                        {
                            result.HadrClusterName = rdr["HadrClusterName"].ToString();
                        }
                    }
                }

                SqlConnection.ClearPool(connection);
            }

            return result;
        }

        private static HadrManagerStatus GetHadrManagerStatus(string hadrManagerStatus)
        {
            int status;
            int.TryParse(hadrManagerStatus, out status);

            switch (status)
            {
                case 0:
                    return HadrManagerStatus.NotStarted;
                case 1:
                    return HadrManagerStatus.StartedAndRunning;
                case 2:
                    return HadrManagerStatus.NotStartedAndFailed;
                default:
                    return HadrManagerStatus.NotApplicable;
            }
        }

        public static bool ValidateSqlServerCredentials(
                string instance,
                string sqlLogin,
                string sqlPassword
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(instance));

            // Validate input.
            if (string.IsNullOrEmpty(instance))
            {
                logX.loggerX.Error("ERROR - no instance specified for getting SQL Server properties");
                throw new ArgumentNullException("Instance is not specified");
            }

            // Build the connection string.
            SqlConnectionStringBuilder bldr = Sql.SqlHelper.ConstructConnectionString(instance, sqlLogin, sqlPassword);

            // Connect to the sql instance.
            using (SqlConnection connection = new SqlConnection(bldr.ConnectionString))
            {
                // Open connection.
                connection.Open();
            }

            // If we have got so far, the credentials are okay, return true.
            return true;
        }
    }
}