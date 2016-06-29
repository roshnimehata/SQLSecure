using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;

namespace Idera.SQLsecure.Core.Accounts
{


    public class NotificationProvider
    {
        private static BBS.TracerX.Logger LOG = BBS.TracerX.Logger.GetLogger("Idera.SQLsecure.Core.Accounts.NotificationProvider");
        private int m_SqlCommandTimeOut = 180;


        // Database fields
        private SqlInt32 m_ProviderId = 1;
        private SqlString m_ProviderName = "SQLsecure";
        private SqlString m_ProviderType = "Email";
        private SqlString m_ServerName = string.Empty;
        private SqlInt32 m_Port = 25;
        private SqlBoolean m_RequiresAuthentication = false;
        private SqlString m_UserName = string.Empty;
        private SqlString m_Password = string.Empty;
        private SqlInt32 m_Timeout = 90;
        private SqlString m_SenderName = string.Empty;
        private SqlString m_SenderEmailAddress = string.Empty;

        private bool m_isValid = false;

        public int SqlCommandTimeout
        {
            get { return m_SqlCommandTimeOut; }
            set { m_SqlCommandTimeOut = value; }
        }

        public int ProviderId
        {
            get { return  m_ProviderId.Value; }
            set { m_ProviderId = value; }
        }
        public string ProviderName
        {
            get { return m_ProviderName.IsNull ? string.Empty : m_ProviderName.Value; }
            set { m_ProviderName = value; }
        }
        public string ProviderType
        {
            get { return m_ProviderType.IsNull ? string.Empty : m_ProviderType.Value; }
            set { m_ProviderType = value; }
        }
        public string ServerName
        {
            get { return m_ServerName.IsNull ? string.Empty : m_ServerName.Value; }
            set { m_ServerName = value; }
        }
        public int Port
        {
            get { return m_Port.Value; }
            set { m_Port = value; }
        }
        public bool RequiresAuthentication
        {
            get { return m_RequiresAuthentication.IsNull ? false : m_RequiresAuthentication.Value; }
            set { m_RequiresAuthentication = value; }
        }
        public string UserName
        {
            get { return m_UserName.IsNull ? string.Empty : m_UserName.Value; }
            set { m_UserName = value; }
        }
        public string Password
        {
            get { return m_Password.IsNull ? string.Empty : m_Password.Value; }
            set { m_Password = value; }
        }
        public int Timeout
        {
            get { return m_Timeout.Value; }
            set { m_Timeout = value; }
        }
        public string SenderName
        {
            get { return m_SenderName.IsNull ? string.Empty : m_SenderName.Value; }
            set { m_SenderName = value; }
        }
        public string SenderEmailAddress
        {
            get { return m_SenderEmailAddress.IsNull ? string.Empty : m_SenderEmailAddress.Value; }
            set { m_SenderEmailAddress = value; }
        }



        // This is the column index to use when obtaining fields from the policy query
        private enum NotificationProviderColumn
        {
            ProviderId = 0,
            ProviderName,
            ProviderType,
            ServerName,
            Port,
            TimeOut,
            ReguiresAuthentication,
            UserName,
            Password,
            SenderName,
            SenderEmailAddress
        }

        // Add Notification Provider.
        private const string NonQueryAddProvider = @"SQLsecure.dbo.isp_sqlsecure_addnotificationprovider";
        private const string NonQueryUpdateProvider = @"SQLsecure.dbo.isp_sqlsecure_updatenotificationprovider";
        private const string ParamProviderName = "@providername";
        private const string ParamProviderType = "@providertype";
        private const string ParamServerName = "@servername";
        private const string ParamPort = "@port"; 
        private const string ParamTimeOut = "@timeout"; 
        private const string ParamRequiresAuthentication = "@requiresauthentication";
        private const string ParamUserName = "@username";
        private const string ParamPassword = "@password";
        private const string ParamSenderName = "@sendername";
        private const string ParamSenderEmail = "@senderemail";
        private const string ParamProviderId = "@notificationproviderid";


        private const string QueryGetNotificationProvider =
            @"SELECT 	notificationproviderid,
	                    providername, 
	                    providertype, 
	                    servername, 
	                    port,
	                    [timeout], 
	                    requiresauthentication, 
	                    username, 
	                    [password], 
	                    sendername, 
	                    senderemail
                      FROM SQLsecure.dbo.vwnotificationprovider";


        public NotificationProvider(int SqlCommandTimeoutIn)
        {
            SqlCommandTimeout = SqlCommandTimeoutIn;
        }

        public void setValues(SqlDataReader rdr)
        {
            m_ProviderId = rdr.GetSqlInt32((int)NotificationProviderColumn.ProviderId);
            m_ProviderName = rdr.GetSqlString((int)NotificationProviderColumn.ProviderName);
            m_ProviderType = rdr.GetSqlString((int)NotificationProviderColumn.ProviderType);
            m_ServerName = rdr.GetSqlString((int)NotificationProviderColumn.ServerName);
            m_Port = rdr.GetSqlInt32((int)NotificationProviderColumn.Port);
            m_Timeout = rdr.GetSqlInt32((int)NotificationProviderColumn.TimeOut);
            m_RequiresAuthentication = rdr.GetSqlBoolean((int)NotificationProviderColumn.ReguiresAuthentication);
            m_UserName = rdr.GetSqlString((int)NotificationProviderColumn.UserName);
            SqlString EncryptedPassword = rdr.GetSqlString((int) NotificationProviderColumn.Password);
            m_Password = EncryptedPassword.IsNull ? EncryptedPassword : Encryptor.Decrypt(EncryptedPassword.Value);
            m_SenderName = rdr.GetSqlString((int)NotificationProviderColumn.SenderName);
            m_SenderEmailAddress = rdr.GetSqlString((int)NotificationProviderColumn.SenderEmailAddress);
            m_isValid = true;
        }

        public bool IsValid()
        {
            return m_isValid;
        }

        
        public static NotificationProvider LoadNotificationProvider(string connectionString, int SQLCommandTimeout)
        {
            NotificationProvider provider = new NotificationProvider(SQLCommandTimeout);
            try
            {
                if (!string.IsNullOrEmpty(connectionString))
                {
                    // Retrieve server information.
                    LOG.Info("Retrieve NotificationProvider");

                    using (SqlConnection connection = new SqlConnection(connectionString)
                        )
                    {
                        // Open the connection.
                        connection.Open();

                        using (SqlDataReader rdr = provider.ExecuteLocalReader(connection, null, CommandType.Text,
                                                                               QueryGetNotificationProvider, null))
                        {
                            while (rdr.Read())
                            {
                                provider.setValues(rdr);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                LOG.Error(string.Format("Error loading Notification Provider: {0}", ex.ToString()));
            }
            catch (Exception ex)
            {
                LOG.Error(string.Format("Error loading Notification Provider: {0}", ex.ToString()));
            }

            return provider;
        }


        public bool UpdateNotificationProvider(string connectionString)
        {
            bool success = true;
            string encrptedPassword = Encryptor.Encrypt(Password);

            try
            {
                // Open connection to repository and add server.
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Setup register server params.
                    SqlParameter paramProviderId = new SqlParameter(ParamProviderId, ProviderId);
                    SqlParameter paramProviderName = new SqlParameter(ParamProviderName, ProviderName);
                    SqlParameter paramProviderType = new SqlParameter(ParamProviderType, ProviderType);
                    SqlParameter paramServerName = new SqlParameter(ParamServerName, ServerName);
                    SqlParameter paramPort = new SqlParameter(ParamPort, Port);
                    SqlParameter paramTimeout = new SqlParameter(ParamTimeOut, Timeout);
                    SqlParameter paramRequiresAuthentication = new SqlParameter(ParamRequiresAuthentication, RequiresAuthentication);
                    SqlParameter paramUsername = new SqlParameter(ParamUserName, UserName);
                    SqlParameter paramPassword = new SqlParameter(ParamPassword, encrptedPassword);
                    SqlParameter paramSenderName = new SqlParameter(ParamSenderName, SenderName);
                    SqlParameter paramSenderEmailAddress = new SqlParameter(ParamSenderEmail, SenderEmailAddress);

                    ExecuteNonQueryLocal(connection, CommandType.StoredProcedure,
                                                  NonQueryUpdateProvider, new SqlParameter[]
                                                                         {   paramProviderId,
                                                                             paramProviderName, paramProviderType,
                                                                             paramServerName, paramPort, 
                                                                             paramTimeout, paramRequiresAuthentication,
                                                                             paramUsername, paramPassword,
                                                                             paramSenderName, paramSenderEmailAddress,
                                                                         });
                }
            }
            catch (Exception ex)
            {
                LOG.Error(string.Format("Failed to update notification {0} error message: {1}", ProviderName, ex.Message));
                success = false;
            }

            return success;
        }



        public int AddNotificationProvider(string connectionString)
        {
            int id = 0;
            string encrptedPassword = Encryptor.Encrypt(Password);
            try
            {
                // Open connection to repository and add server.
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Setup register server params.
                    SqlParameter paramProviderName = new SqlParameter(ParamProviderName, ProviderName);
                    SqlParameter paramProviderType = new SqlParameter(ParamProviderType, ProviderType);
                    SqlParameter paramServerName = new SqlParameter(ParamServerName, ServerName);
                    SqlParameter paramPort = new SqlParameter(ParamPort, Port);
                    SqlParameter paramTimeout = new SqlParameter(ParamTimeOut, Timeout);
                    SqlParameter paramRequiresAuthentication =
                        new SqlParameter(ParamRequiresAuthentication, RequiresAuthentication);
                    SqlParameter paramUsername = new SqlParameter(ParamUserName, UserName);
                    SqlParameter paramPassword = new SqlParameter(ParamPassword, encrptedPassword);
                    SqlParameter paramSenderName = new SqlParameter(ParamSenderName, SenderName);
                    SqlParameter paramSenderEmailAddress = new SqlParameter(ParamSenderEmail, SenderEmailAddress);


                    SqlParameter paramOutId = new SqlParameter(ParamProviderId, DbType.Int32);
                    paramOutId.Direction = ParameterDirection.Output;

                    ExecuteNonQueryLocal(connection, CommandType.StoredProcedure,
                                         NonQueryAddProvider, new SqlParameter[]
                                                                  {
                                                                      paramProviderName, paramProviderType,
                                                                      paramServerName, paramPort,
                                                                      paramTimeout, paramRequiresAuthentication,
                                                                      paramUsername, paramPassword,
                                                                      paramSenderName, paramSenderEmailAddress,
                                                                      paramOutId
                                                                  });
                    m_ProviderId = (int) paramOutId.Value;
                    id = ProviderId;
                }
            }
            catch (Exception ex)
            {
                LOG.Error(
                    string.Format("Failed to add notification {0} error message: {1}", ProviderName, ex.Message));
                id = 0;
            }
            return id;
        }
        


        public void SendMessage(string to, string subject, string body)
        {
            bool exceptionLogged = false;
            SmtpClient client = null;
            try
            {

                client = new SmtpClient();
                client.Host = ServerName;
                if (m_Port > 0)
                    client.Port = Port;
                if (m_RequiresAuthentication && !string.IsNullOrEmpty(UserName))
                {
                    client.Timeout = Timeout * 1000;
                    client.Credentials = new NetworkCredential(UserName, Password);
                }

                MailMessage message = new MailMessage();
                message.From = new MailAddress(SenderEmailAddress);
                ParseAddressesInto(to, message.To);
                message.Subject = subject;
                message.Body = body;

                LOG.DebugFormat("Smtp notification - sending to: {0}", to);
                LOG.DebugFormat("Smtp notification - subject: {0}", subject);
                client.Send(message);
                LOG.DebugFormat("Smtp notification - server: {0}", ServerName);
            }
            catch (SmtpFailedRecipientsException sfre)
            {
                LOG.Error("Smtp notification - send failed", sfre);
                foreach (SmtpFailedRecipientException inner in sfre.InnerExceptions)
                {
                    LOG.Debug(inner.Message);
                }
                exceptionLogged = true;
                throw;
            }
            catch (Exception e)
            {
                if (!exceptionLogged)
                    LOG.Error("Smtp notification - send failed", e);
                throw;
            }
        }

        public static bool IsMailAddressValid(string value, bool single)
        {
            bool result = false;
            try
            {
                if (single)
                {
                    MailAddress address = new MailAddress(value);
                    result = true;
                }
                else
                {
                    MailAddressCollection mac = new MailAddressCollection();
                    result = ParseAddressesInto(value, mac);
                }
            }
            catch (Exception e)
            {

            }
            return result;
        }

        public static bool ParseAddressesInto(string addresses, MailAddressCollection addressCollection)
        {
            StringBuilder builder = new StringBuilder();
            string[] chunks = addresses.Split(new char[] { '@' });
            if (chunks.Length > 0)
                builder.Append(chunks[0].Trim());

            for (int i = 1; i < chunks.Length; i++)
            {
                try
                {
                    builder.Append("@");
                    int j = chunks[i].IndexOfAny(new char[] { ',', ';' });
                    if (j == -1)
                    {
                        if (i + 1 < chunks.Length)
                            throw new InvalidDataException();
                        builder.Append(chunks[i]);
                        addressCollection.Add(new MailAddress(builder.ToString()));
                        builder.Length = 0;
                        break;
                    }
                    else
                    {
                        builder.Append(chunks[i].Substring(0, j));
                        addressCollection.Add(new MailAddress(builder.ToString()));
                        builder.Length = 0;
                        builder.Append(chunks[i].Substring(j + 1));
                    }
                }
                catch (Exception e)
                {
                    LOG.ErrorFormat("Error parsing SMTP To address {0} error: {1}", addresses, e.Message);
                    return false;
                }
            }
            if (builder.Length > 0)
            {
                LOG.ErrorFormat("Error parsing SMTP To address: {0}", addresses);
                return false;
            }

            return true;
        }


        // Need local to class since this class is shared between Collector and Client
        public int ExecuteNonQueryLocal(
           SqlConnection connection,
           CommandType commandType,
           string commandText,
           params SqlParameter[] commandParameters
       )
        {
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
                        prepareCommandLocal(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters);

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
                        LOG.Error("ERROR: Notification Provider ExecuteNonQuery encounterd an exception", ex);
                        throw;
                    }
                }

                return retval;
            }
        }

        /// <summary>
        /// Constructs a command object and calls the ExecuteReader method.
        /// Need this local method since this is used in Collector and UI.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public SqlDataReader ExecuteLocalReader(
                SqlConnection connection,
                SqlTransaction transaction,
                CommandType commandType,
                string commandText,
                SqlParameter[] commandParameters
            )
        {
            // Create/initialize a command and execute reader on it.
            SqlDataReader dataReader = null;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandTimeout = SqlCommandTimeout;

                // Prepare and execute the command.
                try
                {
                    // Create the command object.
                    prepareCommandLocal(cmd, connection, transaction, commandType, commandText, commandParameters);

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
                    LOG.Error("ERROR: ExecuteReader encountered an exception", ex);
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

        private void prepareCommandLocal(
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
                attachParametersLocal(command, commandParameters);
            }
        }

        private void attachParametersLocal(SqlCommand command, SqlParameter[] commandParameters)
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


    }



}
