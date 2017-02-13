/******************************************************************
 * Name: Sql.cs
 *
 * Description: Utility class that provides SQL Server related functions.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.Collector;
using Idera.SQLsecure.Collector.Sql;
using Idera.SQLsecure.Collector.Utility;
namespace Idera.SQLsecure.Collector.Sql
{
    internal static class SqlHelper
    {
        private static LogX logX = new LogX("Idera.SQLsecure.Collector.Sql.SqlHelper");
        
        #region Helpers
        /// <summary>
        /// This method is used to attach array of SqlParameters to a SqlCommand.
        /// 
        /// This method will assign a value of DbNull to any parameter with a direction of
        /// InputOutput and a value of null.  
        /// 
        /// This behavior will prevent default values from being used, but
        /// this will be the less common case than an intended pure output parameter (derived as InputOutput)
        /// where the user provided no input value.
        /// </summary>
        /// <param name="command">The command to which the parameters will be added</param>
        /// <param name="commandParameters">An array of SqlParameters to be added to command</param>
        private static void attachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            Debug.Assert(command != null);
//            using (logX.loggerX.DebugCall())
            {
                if (commandParameters != null)
                {
                    foreach (SqlParameter p in commandParameters)
                    {
                        if (p != null)
                        {
                            // Check for derived output value with no value assigned
                            if ((p.Direction == ParameterDirection.InputOutput ||
                                p.Direction == ParameterDirection.Input) &&
                                (p.Value == null))
                            {
                                p.Value = DBNull.Value;
                            }
                            command.Parameters.Add(p);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method opens (if necessary) and assigns a connection, transaction, command type and parameters 
        /// to the provided command
        /// </summary>
        /// <param name="command">The SqlCommand to be prepared</param>
        /// <param name="connection">A valid SqlConnection, on which to execute this command</param>
        /// <param name="transaction">A valid SqlTransaction, or 'null'</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParameters to be associated with the command or 'null' if no parameters are required</param>
        private static void prepareCommand(
                SqlCommand command, 
                SqlConnection connection, 
                SqlTransaction transaction, 
                CommandType commandType, 
                string commandText, 
                SqlParameter[] commandParameters
            )
        {
            Debug.Assert(command != null);
            Debug.Assert(commandText != null || commandText.Length != 0);
            Debug.Assert(connection.State == ConnectionState.Open);
            Debug.Assert((transaction == null) ? true : transaction.Connection != null);

//            using (logX.loggerX.DebugCall())
            {
                // Associate the connection with the command
                command.Connection = connection;

                // Set the command text (stored procedure name or SQL statement)
                command.CommandText = commandText;

                // If we were provided a transaction, assign it
                if (transaction != null)
                {
                    command.Transaction = transaction;
                }

                // Set the command type
                command.CommandType = commandType;

                // Attach the command parameters if they are provided
                if (commandParameters != null)
                {
                    attachParameters(command, commandParameters);
                }
            }
        }

        #endregion

        /// <summary>
        /// Construct SQL connection string based on input parameters.
        /// </summary>
        /// <param name="instance">Instance to connect</param>
        /// <param name="port">the TCP Port to use when connecting (may be null)</param>
        /// <param name="user">SQL Login credentials (may be null)</param>
        /// <param name="password">"SQL Login password (not null if user specified)</param>
        /// <returns>SqlConnectionStringBuilder</returns>
        public static SqlConnectionStringBuilder ConstructConnectionString(
                string instance,
                int? port,
                string user,
                string password,
                ServerType serverType,
                bool azureADAuth
            )
        {
            SqlConnectionStringBuilder bldr;
            if (port.HasValue)
            {
                bldr = ConstructConnectionString(string.Format("{0},{1}", instance, port.Value), user, password,serverType,azureADAuth);
            }
            else
            {
                bldr = ConstructConnectionString(instance, user, password,serverType,azureADAuth);
            }

            return bldr;
        }

        /// <summary>
        /// Construct SQL connection string based on input parameters.
        /// </summary>
        /// <param name="instance">Instance to connect</param>
        /// <param name="user">SQL Login credentials (may be null)</param>
        /// <param name="password">"SQL Login password (not null if user specified)</param>
        /// <returns>SqlConnectionStringBuilder</returns>
        public static SqlConnectionStringBuilder ConstructConnectionString(
                string instance,
                string user,
                string password,
                ServerType serverType = ServerType.Null,
                bool azureADAuth=false
            )
        {
//            using (logX.loggerX.DebugCall())
            {
                Debug.Assert(instance != null && instance.Length != 0);
                Debug.Assert((user != null && user.Length != 0) ? (password != null && password.Length != 0) : true);

                // Setup data source and application name.
                SqlConnectionStringBuilder bldr = new SqlConnectionStringBuilder();
                bldr.DataSource = instance;
                bldr.ApplicationName = Constants.SqlAppName;
                // If user is specified then its not integrated security,
                // so set the user & password.
                bldr.IntegratedSecurity = (user == null || user.Length == 0);
                if (!bldr.IntegratedSecurity)
                {
                    bldr.UserID = user;
                    bldr.Password = password;
                }
                if (serverType == ServerType.AzureSQLDatabase || (serverType == ServerType.SQLServerOnAzureVM && azureADAuth))
                {
                    bldr.ConnectionString = ConstructConnectionString(instance, user, password, azureADAuth);

                }
                //SQLsecure (Tushar)--Added support for Azure VM
                if(serverType == ServerType.SQLServerOnAzureVM)
                    bldr.ConnectionString =     @"Data Source=" + instance + ";Initial Catalog=master ;User ID= " + user + ";Password=" + password + ";";
                return bldr;
            }
        }


        /// <summary>
        /// Constructs connection strings for azure DB connection and for Azure AD
        /// </summary>
        /// <param name="serverType">server type : on premise,azure DB, SQL server on Azure VM</param>
        /// <param name="azureADAuth">bool value : true when it is for azure Ad</param>
        public static string ConstructConnectionString(
               string instance,
               string user,
               string password,
               bool azureADAuth
           )
        {
            string connectionString;
            if (!azureADAuth)
            {
                connectionString = "Server=" + instance + ";Persist Security Info=False;User ID=" + user + ";Password=" + password + ";MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;";
            }
            else
            {
                connectionString = @"Data Source=" + instance + "; Authentication=Active Directory Password; UID=" + user + "; PWD=" + password;
            }
            return connectionString;
        }
        /// <summary>
        /// Appends database to a connection string, and returns the new connection string.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public static string AppendDatabaseToConnectionString (
                string connectionString,
                string database
            )
        {
            SqlConnectionStringBuilder csbldr = new SqlConnectionStringBuilder(connectionString);
            csbldr.InitialCatalog = database;
            return csbldr.ConnectionString;
        }

        /// <summary>
        /// Parses the connection version and returns an enum value.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns>Sql.ServerVersion</returns>
        public static ServerVersion GetVersion(string connectionString)
        {
//            using (logX.loggerX.DebugCall())
            {
                Debug.Assert(connectionString != null && connectionString.Length != 0);

                ServerVersion version = ServerVersion.Unsupported;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        // Open the connection.
                        connection.Open();

                        // Get the SQL Server version.
                        version = Sql.SqlHelper.ParseVersion(connection.ServerVersion);
                    }
                    catch (SqlException ex)
                    {
                        logX.loggerX.Error("ERROR: GetVersion encounterd an exception", ex);
                        version = ServerVersion.Unsupported;
                        throw;
                    }
                }

                return version;
            }
        }

        /// <summary>
        /// Parses the SQL Server version string, returned by the SqlConnection object.
        /// </summary>
        /// <param name="version">version string</param>
        /// <returns></returns>
        public static ServerVersion ParseVersion(string version)
        {
//            using (logX.loggerX.DebugCall())
            {
                Debug.Assert(version != null && version.Length != 0);

                ServerVersion versionEnum = ServerVersion.Unsupported;
                if (String.Compare(version, 0, Constants.Sql2000VerPrefix, 0,
                                            Constants.Sql2000VerPrefix.Length) == 0)
                {
                    versionEnum = ServerVersion.SQL2000;
                }
                else if (String.Compare(version, 0, Constants.Sql2005VerPrefix, 0,
                                            Constants.Sql2005VerPrefix.Length) == 0)
                {
                    versionEnum = ServerVersion.SQL2005;
                }
                else if (String.Compare(version, 0, Constants.Sql2008VerPrefix, 0,
                                       Constants.Sql2008VerPrefix.Length) == 0)
                {
                    if (String.Compare(version, 0, Constants.Sql2008R2VerPrefix, 0,
                                  Constants.Sql2008R2VerPrefix.Length) == 0)
                    {
                        versionEnum = ServerVersion.SQL2008R2;
                    }
                    else
                    {
                        versionEnum = ServerVersion.SQL2008;
                    }
                }
                else if (String.Compare(version, 0, Constants.Sql2012VerPrefix, 0,
                             Constants.Sql2012VerPrefix.Length) == 0)
                {
                    versionEnum = ServerVersion.SQL2012;
                }
                else if (String.Compare(version, 0, Constants.Sql2014VerPrefix, 0,
                         Constants.Sql2014VerPrefix.Length) == 0)
                {
                    versionEnum = ServerVersion.SQL2014;
                }
                else if (String.Compare(version, 0, Constants.Sql2016VerPrefix, 0,
                     Constants.Sql2016VerPrefix.Length) == 0)
                {
                    versionEnum = ServerVersion.SQL2016;
                }
                else
                {
                    versionEnum = ServerVersion.Unsupported;
                }

                return versionEnum;
            }
        }

        /// <summary>
        /// Creates a safe version of the database name that can be used 
        /// as a where clause in the queries.
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        static public string CreateSafeDatabaseName(string dbName)
        {
            StringBuilder newName = new StringBuilder("[");
            newName.Append(dbName.Replace("]", "]]"));
            newName.Append("]");
            return newName.ToString();
        }

        /// <summary>
        /// Construcs a command object and calls the ExecuteReader
        /// method.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(
                SqlConnection connection, 
                SqlTransaction transaction, 
                CommandType commandType, 
                string commandText, 
                SqlParameter[] commandParameters
            )
        {
//            using (logX.loggerX.DebugCall())
            {
                Debug.Assert(connection != null);

                // Create/initialize a command and execute reader on it.
                SqlDataReader dataReader = null;
                using (SqlCommand cmd = new SqlCommand())
                {
                    //****************************************************************************
                    // NOTE: The command timeout is hard coded at this point, but should be made 
                    // configurable in the future.
                    //****************************************************************************
                    cmd.CommandTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();

                    // Prepare and execute the command.
                    try
                    {
                        // Create the command object.
                        prepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters);

                        // Execute command.
                        dataReader = cmd.ExecuteReader();

                        // Detach the SqlParameters from the command object, so they can be used again.
                        // HACK: There is a problem here, the output parameter values are fletched 
                        // when the reader is closed, so if the parameters are detached from the command
                        // then the SqlReader can´t set its values. 
                        // When this happen, the parameters can´t be used again in other command.
                        bool canClear = true;
                        foreach (SqlParameter commandParameter in cmd.Parameters)
                        {
                            if (commandParameter.Direction != ParameterDirection.Input)
                                canClear = false;
                        }

                        if (canClear)
                        {
                            cmd.Parameters.Clear();
                        }
                    }
                    catch (SqlException ex)
                    {
                        logX.loggerX.Error("ERROR: ExecuteReader encounterd an exception", ex);
                        if (dataReader != null)
                        {
                            dataReader.Dispose();
                            dataReader = null;
                        }
                        throw;
                    }
                }

                return dataReader;
            }
        }

        public static int ExecuteNonQuery (
                SqlConnection connection,
                CommandType commandType, 
                string commandText, 
                params SqlParameter[] commandParameters
            )
        {
//            using (logX.loggerX.DebugCall())
            {
                Debug.Assert(connection != null);

                // Create a command and prepare it for execution
                int retval = 0;
                using (SqlCommand cmd = new SqlCommand())
                {
                    // Prepare and execute the command.
                    try
                    {
                        // Create the command object.
                        prepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters);

                        cmd.CommandTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();

                        // Execute the command
                        retval = cmd.ExecuteNonQuery();

                        // Detach the SqlParameters from the command object, so they can be used again
                        // Detach the SqlParameters from the command object, so they can be used again.
                        // HACK: There is a problem here, the output parameter values are fletched 
                        // when the reader is closed, so if the parameters are detached from the command
                        // then the SqlReader can´t set its values. 
                        // When this happen, the parameters can´t be used again in other command.
                        bool canClear = true;
                        foreach (SqlParameter commandParameter in cmd.Parameters)
                        {
                            if (commandParameter.Direction != ParameterDirection.Input)
                                canClear = false;
                        }

                        if (canClear)
                        {
                            cmd.Parameters.Clear();
                        }
                    }
                    catch (SqlException ex)
                    {
                        logX.loggerX.Error("ERROR: ExecuteNonQuery encounterd an exception", ex);
                        throw;
                    }
                }

                return retval;
            }
        }


       
    }

   
    #region SQL Command Timeout
    public class SQLCommandTimeout
    {
        private static LogX logX = new LogX("Idera.SQLsecure.Collector.Sql.SQLCommandTimeout");

        private const string SQLTimeoutReg = "SQL Command Timeout Collector";
        
        private static int errorCount = 0;
        private const int ERROR_COUNT_LIMIT = 3;

        public static int GetSQLCommandTimeoutFromRegistry()
        {
            int timeout = 180;
            string strTimeout = null;
            RegistryKey hkSoftware = null;
            RegistryKey hkIdera = null;
            RegistryKey hkSQLsecure = null;

            if (errorCount < ERROR_COUNT_LIMIT)
            {
                try
                {
                    hkSoftware = Registry.LocalMachine.OpenSubKey("SOFTWARE");
                    hkIdera = hkSoftware.OpenSubKey("Idera");
                    if (hkIdera != null)
                    {
                        hkSQLsecure = hkIdera.OpenSubKey("SQLsecure");
                        strTimeout = (string) hkSQLsecure.GetValue(SQLTimeoutReg);
                    }
                    if (string.IsNullOrEmpty(strTimeout))
                    {
                        WriteDefaultSQLCommandTimeout(timeout);
                    }
                    else
                    {
                        timeout = Convert.ToInt32(strTimeout);
                        errorCount = 0;
                    }
                }
                catch (Exception ex)
                {
                    logX.loggerX.Error("Error Reading SQL Command Timeout Collector from Registry HKLM: ", ex.Message);
                    errorCount++;
                }
                finally
                {
                    if (hkSoftware != null) hkSoftware.Close();
                    if (hkIdera != null) hkIdera.Close();
                    if (hkSQLsecure != null) hkSQLsecure.Close();
                }
            }
            return timeout;
            
        }

        private static void WriteDefaultSQLCommandTimeout(int defaultTimeout)
        {
            RegistryKey hkSoftware = null;
            RegistryKey hkIdera = null;
            RegistryKey hkSQLsecure = null;

            try
            {
                hkSoftware = Registry.LocalMachine.OpenSubKey("SOFTWARE", true);
                hkIdera = hkSoftware.OpenSubKey("Idera", true);
                if (hkIdera == null)
                {
                    hkIdera = hkSoftware.CreateSubKey("Idera");
                }
                hkSQLsecure = hkIdera.OpenSubKey("SQLsecure", true);
                if (hkSQLsecure == null)
                {
                    hkSQLsecure = hkIdera.CreateSubKey("SQLsecure");
                }
                hkSQLsecure.SetValue(SQLTimeoutReg, defaultTimeout.ToString(), RegistryValueKind.String);
            }
            catch (Exception ex)
            {
                logX.loggerX.Error("Error Writing SQL Command Timeout Collector to Registry HKLM: ", ex.Message);
                errorCount++;
            }
            finally
            {
                if (hkSoftware != null) hkSoftware.Close();
                if (hkIdera != null) hkIdera.Close();
                if (hkSQLsecure != null) hkSQLsecure.Close();
            }
        }

    }
    #endregion
}
