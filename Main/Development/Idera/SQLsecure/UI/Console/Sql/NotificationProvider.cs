/******************************************************************
 * Name: NotificationProvider.cs
 *
 * Description: Encapsulates a Notification Provider.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2007 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Diagnostics;

using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Utility;

namespace Idera.SQLsecure.UI.Console.Sql
{
    /// <summary>
    /// Encapsulates a Notification Provider
    /// </summary>
    public class NotificationProvider
    {
        #region Fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Sql.NotificationProvider");
        private SqlInt32 m_NotificationProviderId;
        private SqlString m_ProviderName;
        private SqlString m_ProviderType;
        private SqlString m_ServerName;
        private SqlInt32 m_Port;
        private SqlInt32 m_Timeout;
        private SqlBoolean m_RequiresAuthentication;
        private SqlString m_UserName;
        private SqlString m_Password;
        private SqlString m_SenderName;
        private SqlString m_SenderEmail;

        #endregion

        #region Ctors

        public NotificationProvider(SqlDataReader rdr)
        {
            setValues(rdr);
        }

        #endregion

        #region Properties

        public int NotificationProviderId { get { return m_NotificationProviderId.Value; } }
        public string ProviderName { get { return m_ProviderName.Value; } }
        public string ProviderType { get { return m_ProviderType.Value; } }
        public string ServerName { get { return m_ServerName.Value; } }
        public int Port { get { return m_Port.Value; } }
        public int Timeout { get { return m_Timeout.IsNull ? 0 : m_Timeout.Value; } }
        public bool RequiresAuthentication { get { return m_RequiresAuthentication.IsNull ? false : m_RequiresAuthentication.Value; } }
        public string UserName { get { return (m_UserName.IsNull ? string.Empty : m_UserName.Value); } }
        public string Password { get { return (m_Password.IsNull ? string.Empty : m_Password.Value); } }
        public string SenderName { get { return (m_SenderName.IsNull ? string.Empty : m_SenderName.Value); } }
        public string SenderEmail { get { return (m_SenderEmail.IsNull ? string.Empty : m_SenderEmail.Value); } }

        #endregion

        #region Queries

        // Get registered servers.
        private const string QueryGetAllProviders =
                    @"SELECT 
                        notificationproviderid,
                        providername,
                        providertype,
                        servername, 
                        port,
                        timeout,
                        requiresauthentication,
                        username,
                        password,
                        sendername,
                        senderemail
                      FROM SQLsecure.dbo.vwnotificationprovider";

        private static string QueryGetProvider = QueryGetAllProviders +
                    @" WHERE notificationproviderid = @notificationproviderid";
        private const string ParamId = "notificationproviderid";

        // This is the column index to use when obtaining fields from the policy query
        private enum ProviderColumn
        {
            NotificationProviderId = 0,
            ProviderName,
            ProviderType,
            ServerName,
            Port,
            Timeout,
            RequiresAuthentication,
            UserName,
            Password,
            SenderName,
            SenderEmail
        }

        // Add provider.
        //private const string NonQueryAddProvider = @"SQLsecure.dbo.isp_sqlsecure_addnotificationprovider";

        // Delete provider.
        //private const string NonQueryRemoveProvider = @"SQLsecure.dbo.isp_sqlsecure_removenotificationprovider";

        // Update provider.
        private const string NonQueryUpdateProvider = @"SQLsecure.dbo.isp_sqlsecure_updatenotificationprovider";

        private const string ParamNotificationProviderId = "@notificationproviderid";
        private const string ParamProviderName = "@providername";
        private const string ParamProviderType = "@providertype";
        private const string ParamServerName = "@servername";
        private const string ParamPort = "@port";
        private const string ParamTimeout = "@timeout";
        private const string ParamRequiresAuthentication = "@requiresauthentication";
        private const string ParamUserName = "@username";
        private const string ParamPassword = "@password";
        private const string ParamSenderName = "@sendername";
        private const string ParamSenderEmail = "@senderemail";

        #endregion

        #region Helpers

        private void setValues(SqlDataReader rdr)
        {
            m_NotificationProviderId = rdr.GetSqlInt32((int)ProviderColumn.NotificationProviderId);
            m_ProviderName = rdr.GetSqlString((int)ProviderColumn.ProviderName);
            m_ProviderType = rdr.GetSqlString((int)ProviderColumn.ProviderType);
            m_ServerName = rdr.GetSqlString((int)ProviderColumn.ServerName);
            m_Port = rdr.GetSqlInt32((int)ProviderColumn.Port);
            m_Timeout = rdr.GetSqlInt32((int)ProviderColumn.Timeout);
            m_RequiresAuthentication = rdr.GetSqlBoolean((int)ProviderColumn.RequiresAuthentication);
            m_UserName = rdr.GetSqlString((int)ProviderColumn.UserName);
            m_Password = rdr.GetSqlString((int)ProviderColumn.Password);
            m_SenderName = rdr.GetSqlString((int)ProviderColumn.SenderName);
            m_SenderEmail = rdr.GetSqlString((int)ProviderColumn.SenderEmail);
        }

        #endregion

        #region Methods

        //public static Repository.NotificationProviderList LoadProviders(string connectionString)
        //{
        //    Repository.NotificationProviderList providerlist = new Repository.NotificationProviderList();

        //    try
        //    {
        //        if (!string.IsNullOrEmpty(connectionString))
        //        {
        //            // Retrieve server information.
        //            logX.loggerX.Info("Retrieve Notification Providers");

        //            using (SqlConnection connection = new SqlConnection(connectionString)
        //                )
        //            {
        //                // Open the connection.
        //                connection.Open();

        //                using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
        //                                                                       QueryGetAllProviders, null))
        //                {
        //                    while (rdr.Read())
        //                    {
        //                        NotificationProvider provider = new NotificationProvider(rdr);

        //                        providerlist.Add(provider);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        logX.loggerX.Error(string.Format(Utility.ErrorMsgs.ErrorStub, Utility.ErrorMsgs.CantGetProviders), ex);
        //        MsgBox.ShowError(Utility.ErrorMsgs.CantGetProviders, ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        logX.loggerX.Error(string.Format(Utility.ErrorMsgs.ErrorStub, Utility.ErrorMsgs.CantGetProviders), ex);
        //        MsgBox.ShowError(Utility.ErrorMsgs.CantGetProviders, ex.Message);
        //    }

        //    return providerlist;
        //}

        ///// <summary>
        ///// Add a new Notification Provider
        ///// </summary>
        ///// <param name="id">Identifier of the provider</param>
        ///// <param name="providerName">Name of the provider</param>
        ///// <param name="providerType">Type of provider</param>
        ///// <param name="serverName">Name of the outgoing server to be used for notification</param>
        ///// <param name="port">The port to connect over</param>
        ///// <param name="timeout">The timout interval used when connecting</param>
        ///// <param name="requiresauthentication">Whether the server requires authentication</param>
        ///// <param name="userName">The user name for authentication</param>
        ///// <param name="password">The password for authentication</param>
        ///// <param name="senderName">The name to use on outgoing notifications</param>
        ///// <param name="senderEmail">The return email address to use on outgoing notifications</param>
        //public static void AddProvider(
        //        int id,
        //        string providerName,
        //        string providerType,
        //        string serverName,
        //        int port,
        //        int timeout,
        //        bool requiresauthentication,
        //        string userName,
        //        string password,
        //        string senderName,
        //        string senderEmail
        //    )
        //{
        //    Debug.Assert(!string.IsNullOrEmpty(providerName));

        //    // Open connection to repository and add server.
        //    using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
        //    {
        //        // Open the connection.
        //        connection.Open();

        //        // Setup register server params.
        //        SqlParameter paramId = new SqlParameter(ParamNotificationProviderId, id);
        //        SqlParameter paramProviderName = new SqlParameter(ParamProviderName, providerName);
        //        SqlParameter paramProviderType = new SqlParameter(ParamProviderType, providerType);
        //        SqlParameter paramServerName = new SqlParameter(ParamServerName, serverName);
        //        SqlParameter paramPort = new SqlParameter(ParamPort, port);
        //        SqlParameter paramTimeout = new SqlParameter(ParamTimeout, timeout);
        //        SqlParameter paramRequiresAuthentication = new SqlParameter(ParamRequiresAuthentication, requiresauthentication);
        //        SqlParameter paramUserName = new SqlParameter(ParamUserName, userName);
        //        SqlParameter paramPassword = new SqlParameter(ParamPassword, password);
        //        SqlParameter paramSenderName = new SqlParameter(ParamSenderName, senderName);
        //        SqlParameter paramSenderEmail = new SqlParameter(ParamSenderEmail, senderEmail);

        //        Sql.SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure,
        //                        NonQueryAddProvider, new SqlParameter[] { paramId, paramProviderName, paramProviderType, 
        //                                                paramServerName, paramServerName, paramPort, paramTimeout, paramRequiresAuthentication,
        //                                                paramUserName, paramPassword, paramSenderName, paramSenderEmail});
        //    }
        //}

        /// <summary>
        /// Update Notification Provider information
        /// </summary>
        /// <param name="id">Identifier of the provider</param>
        /// <param name="providerName">Name of the provider</param>
        /// <param name="providerType">Type of provider</param>
        /// <param name="serverName">Name of the outgoing server to be used for notification</param>
        /// <param name="port">The port to connect over</param>
        /// <param name="timeout">The timout interval used when connecting</param>
        /// <param name="requiresauthentication">Whether the server requires authentication</param>
        /// <param name="userName">The user name for authentication</param>
        /// <param name="password">The password for authentication</param>
        /// <param name="senderName">The name to use on outgoing notifications</param>
        /// <param name="senderEmail">The return email address to use on outgoing notifications</param>
        public static void UpdateProvider(
                int id,
                string providerName,
                string providerType,
                string serverName,
                int port,
                int timeout,
                bool requiresauthentication,
                string userName,
                string password,
                string senderName,
                string senderEmail
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(providerName));

            // Open connection to repository and add server.
            using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
            {
                // Open the connection.
                connection.Open();

                // Setup register server params.
                SqlParameter paramId = new SqlParameter(ParamNotificationProviderId, id);
                SqlParameter paramProviderName = new SqlParameter(ParamProviderName, providerName);
                SqlParameter paramProviderType = new SqlParameter(ParamProviderType, providerType);
                SqlParameter paramServerName = new SqlParameter(ParamServerName, serverName);
                SqlParameter paramPort = new SqlParameter(ParamPort, port);
                SqlParameter paramTimeout = new SqlParameter(ParamTimeout, timeout);
                SqlParameter paramRequiresAuthentication = new SqlParameter(ParamRequiresAuthentication, requiresauthentication);
                SqlParameter paramUserName = new SqlParameter(ParamUserName, userName);
                SqlParameter paramPassword = new SqlParameter(ParamPassword, password);
                SqlParameter paramSenderName = new SqlParameter(ParamSenderName, senderName);
                SqlParameter paramSenderEmail = new SqlParameter(ParamSenderEmail, senderEmail);

                Sql.SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure,
                                NonQueryUpdateProvider, new SqlParameter[] { paramId, paramProviderName, paramProviderType, 
                                                        paramServerName, paramServerName, paramPort, paramTimeout, paramRequiresAuthentication,
                                                        paramUserName, paramPassword, paramSenderName, paramSenderEmail});
            }
        }

        //public static void RemovePolicy(int id)
        //{
        //    GetProvider(id);

        //    // Open connection to repository and remove policy.
        //    using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
        //    {
        //        // Open the connection.
        //        connection.Open();

        //        // Setup register server params.
        //        SqlParameter param = new SqlParameter(ParamId, id);
        //        Sql.SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure,
        //                        NonQueryRemovePolicy, new SqlParameter[] { param });

        //    }
        //}

        public static NotificationProvider GetProvider(int id)
        {
            NotificationProvider provider = null;

            // Open connection to repository and get policy properties.
            using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
            {
                // Open the connection.
                connection.Open();

                // Get the policy.
                SqlParameter param = new SqlParameter(ParamId, id);
                using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                QueryGetProvider, new SqlParameter[] { param }))
                {
                    if (rdr.HasRows && rdr.Read())
                    {
                        provider = new NotificationProvider(rdr);
                    }
                }
            }

            return provider;
        }

        #endregion
    }
}
