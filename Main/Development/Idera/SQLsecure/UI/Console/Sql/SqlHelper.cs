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

using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Idera.SQLsecure.Core.Logger;

namespace Idera.SQLsecure.UI.Console.Sql
{
    internal static class SqlHelper
    {
        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Sql.SqlHelper");

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

        #endregion

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
                string serverType,
                bool azureADAuth=false
            )
        {
            return ConstructConnectionString(instance, user, password, 0, serverType, azureADAuth);
        }

        public static SqlConnectionStringBuilder ConstructConnectionString(
                string instance,
                string user,
                string password,
                int timeout,
                string serverType,
                bool azureADAuth
            )
        {
            Debug.Assert(instance != null && instance.Length != 0);

            // Setup data source and application name.
            SqlConnectionStringBuilder bldr = new SqlConnectionStringBuilder();
            bldr.DataSource = CreateSafeDatabaseNameForConnectionString(instance);

            bldr.ApplicationName = Constants.SqlAppName;
            if (timeout > 0)
            {
                bldr.ConnectTimeout = timeout;
            }

            // If user is specified then its not integrated security,
            // so set the user & password.
            bldr.IntegratedSecurity = (user == null || user.Length == 0);
            if (!bldr.IntegratedSecurity)
            {
                bldr.UserID = user;
                bldr.Password = password;
            }
            //string serverType = Utility.Activity.TypeServerOnPremise;
            if (serverType == Utility.Activity.TypeServerAzureDB ||(serverType==Utility.Activity.TypeServerAzureVM && azureADAuth))
            {
                bldr.ConnectionString=ConstructConnectionString( instance, user, password,azureADAuth);
                
            }
            
            return bldr;
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
        /// Parses the connection version and returns an enum value.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns>Sql.ServerVersion</returns>
        public static ServerVersion GetVersion(string connectionString)
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
                }
            }

            return version;
        }

        /// <summary>
        /// Parses the SQL Server version string, returned by the SqlConnection object.
        /// </summary>
        /// <param name="version">version string</param>
        /// <returns></returns>
        public static ServerVersion ParseVersion(string version)
        {
            Debug.Assert(version != null && version.Length != 0);

            // Get the first non-zero number.
            int i = 0;
            for (i = 0; i < version.Length; ++i)
            {
                if (version[i] != '0') { break; }
            }
            
            // Now figure out the version.
            ServerVersion versionEnum = ServerVersion.Unsupported;
            if (String.Compare(version, i, Constants.Sql2000VerPrefix, 0,
                                        Constants.Sql2000VerPrefix.Length) == 0)
            {
                versionEnum = ServerVersion.SQL2000;
            }
            else if (String.Compare(version, i, Constants.Sql2005VerPrefix, 0,
                                        Constants.Sql2005VerPrefix.Length) == 0)
            {
                versionEnum = ServerVersion.SQL2005;
            }
            else if (String.Compare(version, i, Constants.Sql2008VerPrefix, 0,
                              Constants.Sql2008VerPrefix.Length) == 0)
            {
                if (String.Compare(version, i, Constants.Sql2008R2VerPrefix, 0,
                              Constants.Sql2008R2VerPrefix.Length) == 0)
                {
                    versionEnum = ServerVersion.SQL2008R2;
                }
                else
                {
                    versionEnum = ServerVersion.SQL2008;
                }
            }
            else if (String.Compare(version, i, Constants.Sql2012VerPrefix, 0,
                              Constants.Sql2012VerPrefix.Length) == 0)
            {
                versionEnum = ServerVersion.SQL2012;
            }
            else if (String.Compare(version, i, Constants.Sql2014VerPrefix, 0,
                         Constants.Sql2014VerPrefix.Length) == 0)
            {
                versionEnum = ServerVersion.SQL2014;
            }
            else if (String.Compare(version, i, Constants.Sql2016VerPrefix, 0,
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

        /// <summary>
        /// Parses the SQL Server version string, returned by the SqlConnection object.
        /// </summary>
        /// <param name="version">version string</param>
        /// <returns>a string containing the service pack installed or an empty string if it cannot be determined</returns>
        public static string ParseVersionBuild(string version)
        {
            Debug.Assert(version != null && version.Length != 0);

            string SP = string.Empty;
            const string dot = @".";

            ServerVersion ver = ParseVersion(version);
            string build = string.Empty;
            // Strip off the major version
            if (version.IndexOf(dot) > 0)
            {
                build = version.Substring(version.IndexOf(dot) + 1);
                // Strip off the minor version
                if (build.IndexOf(dot) > 0)
                {
                    build = build.Substring(build.IndexOf(dot) + 1);
                }
                // Strip off the hotfix version
                if (build.IndexOf(dot) > 0)
                {
                    build = build.Substring(0, build.IndexOf(dot));
                }
            }


            switch (ver)
            {
                case ServerVersion.SQL2000:

                for (int i = 0; i < ServicePack.SQL2000.Builds.Length; ++i)
                {
                    if (build == ServicePack.SQL2000.Builds[i])
                    {
                        SP = ServicePack.SQL2000.BuildNames[i];
                    }
                }
                    break;

                case ServerVersion.SQL2005:

                for (int i = 0; i < ServicePack.SQL2005.Builds.Length; ++i)
                {
                    if (build == ServicePack.SQL2005.Builds[i])
                    {
                        SP = ServicePack.SQL2005.BuildNames[i];
                    }
                }
                    break;

                case ServerVersion.SQL2008:

                for (int i = 0; i < ServicePack.SQL2008.Builds.Length; ++i)
                {
                    if (build == ServicePack.SQL2008.Builds[i])
                    {
                        SP = ServicePack.SQL2008.BuildNames[i];
                    }
                }
                    break;

                case ServerVersion.SQL2008R2:
                
                for (int i = 0; i < ServicePack.SQL2008R2.Builds.Length; ++i)
                {
                    if (build == ServicePack.SQL2008R2.Builds[i])
                    {
                        SP = ServicePack.SQL2008R2.BuildNames[i];
                    }
                }
                    break;

                case ServerVersion.SQL2012:
               
                for (int i = 0; i < ServicePack.SQL2012.Builds.Length; ++i)
                {
                    if (build == ServicePack.SQL2012.Builds[i])
                    {
                        SP = ServicePack.SQL2012.BuildNames[i];
                    }
                }
                    break;

                case ServerVersion.SQL2014:

                for (int i = 0; i < ServicePack.SQL2014.Builds.Length; ++i)
                {
                    if (build == ServicePack.SQL2014.Builds[i])
                    {
                        SP = ServicePack.SQL2014.BuildNames[i];
                    }
                }
                    break;

                case ServerVersion.SQL2016:
                
                    for (int i = 0; i < ServicePack.SQL2016.Builds.Length; ++i)
                    {
                        if (build == ServicePack.SQL2016.Builds[i])
                        {
                            SP = ServicePack.SQL2016.BuildNames[i];
                        }
            }
                    break;
            }

            return SP;
        }

        /// <summary>
        /// Parses the SQL Server version string, returned by the SqlConnection object.
        /// </summary>
        /// <param name="version">version string</param>
        /// <returns>a string containing the friendly version name of the server</returns>
        public static string ParseVersionFriendly(string version)
        {
            string ver = string.Empty;
            ServerVersion fversion = Sql.SqlHelper.ParseVersion(version);

            if (fversion == ServerVersion.SQL2000)
            {
                ver = VersionName.SQL2000;
            }
            else if (fversion == ServerVersion.SQL2005)
            {
                ver = VersionName.SQL2005;
            }
            else if (fversion == ServerVersion.SQL2008)
            {
                ver = VersionName.SQL2008;
            }
            else if (fversion == ServerVersion.SQL2008R2)
            {
                ver = VersionName.SQL2008R2;
            }
            else if (fversion == ServerVersion.SQL2012)
            {
                ver = VersionName.SQL2012;
            }
            else if (fversion == ServerVersion.SQL2014)
            {
                ver = VersionName.SQL2014;
            }
            else if (fversion == ServerVersion.SQL2016)
            {
                ver = VersionName.SQL2016;
            }
            else
            {
                ver = VersionName.Unsupported;
            }

            string build = ParseVersionBuild(version);
            if (build.Length > 0)
            {
                ver = string.Format("{0} {1}", ver, build);
            }

            return ver;
        }

        /// <summary>
        /// Parses the SQL Server version string, returned by the SqlConnection object.
        /// </summary>
        /// <param name="version">version string</param>
        /// <returns>a string containing the friendly version name of the server and an optional version number</returns>
        public static string ParseVersionFriendly(string version, bool includeversion)
        {
            string ver = Sql.SqlHelper.ParseVersionFriendly(version);

            return string.Format("{0} v{1}", ver, version);
        }

        /// <summary>
        /// Constructs a command object and calls the ExecuteReader
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
            Debug.Assert(connection != null);

            // Create/initialize a command and execute reader on it.
            SqlDataReader dataReader = null;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandTimeout = Utility.SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();

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
                    logX.loggerX.Error("ERROR: ExecuteReader encountered an exception", ex);
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

        public static int ExecuteNonQuery(
                SqlConnection connection,
                CommandType commandType,
                string commandText,
                params SqlParameter[] commandParameters
            )
        {
//            using (LogBlock b = new LogBlock("Sql.ExecuteReader"))
            {
                Debug.Assert(connection != null);

                // Create a command and prepare it for execution
                int retval = 0;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandTimeout = Utility.SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();

                    // Prepare and execute the command.
                    try
                    {
                        // Create the command object.
                        prepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters);

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
        //-----------------------------------------------------------------------
        // CreateSafeString - creates safe string parameter includes
        //                    single quotes; used to create sql parameters
        //-----------------------------------------------------------------------
        static public string CreateSafeString(string propName)
        {
            return CreateSafeString(propName, int.MaxValue, true);
        }

        //-----------------------------------------------------------------------
        // CreateSafeString - creates safe string parameter includes
        //                    single quotes; used to create sql parameters
        //-----------------------------------------------------------------------
        static public string CreateSafeString(string propName, bool quoted)
        {
            return CreateSafeString(propName, int.MaxValue, quoted);
        } 

        //-----------------------------------------------------------------------
        // CreateSafeString - creates safe string parameter includes
        //                    single quotes; used to create sql parameters with
        //                    length limit
        //-----------------------------------------------------------------------
        static public string CreateSafeString(string propName, int limit, bool quoted)
        {
            StringBuilder newName;
            string tmpValue;

            if (propName == null)
            {
                newName = new StringBuilder("null");
            }
            else
            {
                newName = new StringBuilder(quoted ? "'" : string.Empty);
                tmpValue = propName.Replace("'", "''");
                if (tmpValue.Length > limit)
                {
                    if (tmpValue[limit - 1] == '\'')
                    {
                        limit--;
                        if (tmpValue[limit - 1] == '\'')
                            limit--;
                    }
                    tmpValue = tmpValue.Remove(limit, tmpValue.Length - limit);
                }
                newName.Append(tmpValue);
                if (quoted)
                {
                    newName.Append("'");
                }
            }

            return newName.ToString();
        }


        public static bool SqlInjectionChars(string match)
        {
            bool isChars = false;
            string[] RejectChars = new string[] { ";", "'", "/*", "--", "Xp_", "XP_", "xP_", "xp_" };
            foreach (string reject in RejectChars)
            {
                if (match.Contains(reject))
                {
                    isChars = true;
                    break;
                }
            }
            return isChars;
        }

        //-----------------------------------------------------------------------
        // CreateSafeDatabaseName - creates safe db name for SQL
        //-----------------------------------------------------------------------
        static public string CreateSafeDatabaseName(string dbName)
        {
            StringBuilder newName;

            newName = new StringBuilder("[");
            newName.Append(dbName.Replace("]", "]]"));
            newName.Append("]");

            return newName.ToString();
        }

        //-----------------------------------------------------------------------
        // CreateSafeDatabaseNameForConnectionString
        //
        // (1) If database begins or ends with blanks, wrap in quotes
        // (2) If contains one of ;'" then you need to espace with ' or ". Use "
        //     unless first character is single quote then use double quote
        // (3) If string contains any escaped chars enclose in quotes
        //-----------------------------------------------------------------------
        static public string
           CreateSafeDatabaseNameForConnectionString(
              string dbName
           )
        {
            if (dbName == null || dbName.Length == 0) return dbName;

            // Use double quote as escape character unless first character is double
            // quote; then use single quote
            string doubleQuote = "\"";

            bool encloseInQuotes = false;

            // Do we need to enclose in quotes? (contains semicolon or leading or trailing spaces)
            if ((-1 != dbName.IndexOf(";")) ||
                 (dbName[0] == ' ' || dbName[dbName.Length - 1] == ' '))
            {
                encloseInQuotes = true;
            }

            if (encloseInQuotes)
            {
                // escape any double quotes
                dbName = dbName.Replace(doubleQuote, "\"\"");
                dbName = doubleQuote + dbName + doubleQuote;
            }

            return dbName;
        }

        static public string GetString(SqlDataReader rdr, int index)
        {
            string retval;

            if (!rdr.IsDBNull(index))
            {
                retval = rdr.GetString(index);
            }
            else
            {
                retval = "";
            }

            return retval;
        }

        static public DateTime GetDateTime(SqlDataReader rdr, int index)
        {
            DateTime retval;

            if (!rdr.IsDBNull(index))
            {
                retval = rdr.GetDateTime(index);
            }
            else
            {
                retval = DateTime.MinValue;
            }

            return retval;
        }

        static public int GetInt32(SqlDataReader rdr, int index)
        {
            int retval;

            if (!rdr.IsDBNull(index))
            {
                retval = rdr.GetInt32(index);
            }
            else
            {
                retval = 0;
            }

            return retval;
        }
    }
}
