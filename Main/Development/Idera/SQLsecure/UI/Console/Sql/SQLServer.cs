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
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Diagnostics;

using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Utility;

using Microsoft.SqlServer.Management.Smo;

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
            Debug.Assert(!string.IsNullOrEmpty(instance));

            // Init return.
            version = string.Empty;
            machineName = string.Empty;
            instanceName = string.Empty;
            fullName = string.Empty;

            // Validate input.
            if (string.IsNullOrEmpty(instance))
            {
                logX.loggerX.Error("ERROR - no instance specified for getting SQL Server properties");
                throw new ArgumentNullException("Instance is not specified");
            }

            // Build the connection string.
            SqlConnectionStringBuilder bldr = Sql.SqlHelper.ConstructConnectionString(instance, sqlLogin, sqlPassword);

            // Connect to the sql instance and get its properties.
            using (SqlConnection connection = new SqlConnection(bldr.ConnectionString))
            {
                // Open connection.
                connection.Open();

                // Get the version.
                version = connection.ServerVersion;
                string type = null;
                string confQuery = @"select isnull(SERVERPROPERTY('IsClustered'),0) AS IsClusterd, 
                                          isnull(SERVERPROPERTY('HadrManagerStatus'),0) as HadrManagerStatus,
                                          isnull(SERVERPROPERTY('MachineName'),'')  as MachineName,
                                          isnull(SERVERPROPERTY('ServerName'),'') as ServerName,
                                          isnull(SERVERPROPERTY('InstanceName'),'') as InstanceName

                                   SELECT top 1 cluster_name FROM  sys.dm_hadr_cluster
";


                //Get server type 
                using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                    confQuery, null))
                {
                    if (rdr.HasRows && rdr.Read())
                    {
                        if (int.Parse(rdr["IsClusterd"].ToString()) == 1)
                        {
                            type = "C";
                    }

                        instanceName = rdr["InstanceName"].ToString();
                        if (int.Parse(rdr["HadrManagerStatus"].ToString()) == 1)
                        {
                            type = "A";
                }
                        if (type == "A")
                        {

                            if (rdr.NextResult()&&rdr.HasRows)
                    {
                                rdr.Read();
                                fullName = machineName = rdr.GetString(0);
                    }
                }
                        else
                        {
                            // Get the machine name.
                            machineName = rdr["MachineName"].ToString();
                                    // this should not be null, so if it is let there be an exception.
                // Get the full name.
                            fullName = rdr["ServerName"].ToString();
                        }
                    }
                }

                SqlConnection.ClearPool(connection);
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