using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;


namespace Idera.SQLsecure.Core.Accounts
{
    public class RegisteredServerNotification
    {
        public enum MetricSeverityNotificaiton
        {
            Never = 0,
            Any,
            OnWarningAndError,
            OnlyOnError
        }
        public const string SnapshotStatusNotification_Always = "A";
        public const string SnapshotStatusNotification_OnWarningOrError = "W";
        public const string SnapshotStatusNotification_OnlyOnError = "E";
        public const string SnapshotStatusNotification_Never = "N";

        private static BBS.TracerX.Logger LOG = BBS.TracerX.Logger.GetLogger("Idera.SQLsecure.Core.Accounts.SMTPClient");
        private int m_SqlCommandTimeOut = 180;

        // Database fields
        private SqlInt32 m_RegisteredServerId = 0;
        private SqlInt32 m_ProviderId = 0;
        private SqlString m_SnapshotStatus;
        private SqlInt32 m_PolicyMetricSeverity;
        private SqlString m_Recipients;

        private bool m_isValid = false;

        public int SqlCommandTimeout
        {
            get { return m_SqlCommandTimeOut; }
            set { m_SqlCommandTimeOut = value; }
        }
        public int RegisteredServerId
        {
            get { return m_RegisteredServerId.Value; }
            set { m_RegisteredServerId = value; }
        }
        public int ProviderId
        {
            get { return m_ProviderId.Value; }
            set { m_ProviderId = value; }
        }
        public string SnapshotStatus
        {
            get { return m_SnapshotStatus.IsNull ? string.Empty : m_SnapshotStatus.Value; }
            set { m_SnapshotStatus = value; }
        }
        public int PolicyMetricSeverity
        {
            get { return m_PolicyMetricSeverity.Value; }
            set { m_PolicyMetricSeverity = value; }
        }
        public string Recipients 
        {
            get { return m_Recipients.IsNull ? string.Empty : m_Recipients.Value; }
            set { m_Recipients = value; }
        }
        
    
        // This is the column index to use when obtaining fields from the policy query
        private enum RegisteredServerNotificationColumn
        {
            ProviderId = 0,
            RegisteredServerId,
            SnapshotStatus,
            PolicyMetricSeverity,
            Recipients
        }

        // Add Notification Provider.
        private const string NonQueryAddRegisteredServerNotification = @"SQLsecure.dbo.isp_sqlsecure_addnotificationtoregisteredserver";
        private const string NonQueryUpdateRegisteredServerNotification = @"SQLsecure.dbo.isp_sqlsecure_updateregisteredservernotification";
        private const string ParamRegisteredServerId = "@registeredserverid";
        private const string ParamNotificationProviderId = "@notificationproviderid";
        private const string ParamSnapshotStatus = "@snapshotstatus";
        private const string ParamPolicyMetricsSeverity = "@policymetricseverity"; 
        private const string ParamRecipients = "@recipients"; 



        private const string QueryGetRegisteredServerNotificaiton =
            @"SELECT 	notificationproviderid,
                        registeredserverid,                        
	                    snapshotstatus,
	                    policymetricseverity,
	                    recipients
                      FROM SQLsecure.dbo.vwservernotification
                      WHERE registeredserverid = @rID and notificationproviderid = @pID";

        private const string ParamRSId = "rID";
        private const string ParamPId = "pID";

        public RegisteredServerNotification(int SqlCommandTimeoutIn)
        {
            SqlCommandTimeout = SqlCommandTimeoutIn;            
        }

        private void setValues(SqlDataReader rdr)
        {
            m_RegisteredServerId = rdr.GetSqlInt32((int)RegisteredServerNotificationColumn.RegisteredServerId);
            m_ProviderId = rdr.GetSqlInt32((int)RegisteredServerNotificationColumn.ProviderId);
            m_SnapshotStatus = rdr.GetSqlString((int)RegisteredServerNotificationColumn.SnapshotStatus);
            m_PolicyMetricSeverity = rdr.GetSqlInt32((int)RegisteredServerNotificationColumn.PolicyMetricSeverity);
            m_Recipients = rdr.GetSqlString((int)RegisteredServerNotificationColumn.Recipients);
            m_isValid = true;
        }

        public bool IsValid()
        {
            return m_isValid;            
        }

        public static RegisteredServerNotification LoadRegisteredServerNotification(string connectionString, 
            int SQLCommandTimeout,
            int registerServerId,
            int providerId)
        {
            RegisteredServerNotification provider = new RegisteredServerNotification(SQLCommandTimeout);
            try
            {
                if (!string.IsNullOrEmpty(connectionString))
                {
                    // Retrieve server information.
                    LOG.Info("Retrieve SmtpClient");

                    using (SqlConnection connection = new SqlConnection(connectionString)
                        )
                    {
                        // Open the connection.
                        connection.Open();
                        SqlParameter paramRSId = new SqlParameter(ParamRSId, registerServerId);
                        SqlParameter paramPId = new SqlParameter(ParamPId, providerId);
                        using (SqlDataReader rdr = provider.ExecuteLocalReader(connection, null, CommandType.Text,
                                                                               QueryGetRegisteredServerNotificaiton,
                                                                               new SqlParameter[] { paramRSId, paramPId }))
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
                LOG.Error(string.Format("Error loading Notification Provider: {0}"), ex);
            }
            catch (Exception ex)
            {
                LOG.Error(string.Format("Error loading Notification Provider: {0}"), ex);
            }

            return provider;
        }


        public bool UpdateNotificationProvider(string connectionString)
        {
            bool success = true;

            try
            {
                // Open connection to repository and add server.
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Setup register server params.
                    SqlParameter paramRegisteredServerId = new SqlParameter(ParamRegisteredServerId, RegisteredServerId);
                    SqlParameter paramProviderId = new SqlParameter(ParamNotificationProviderId, ProviderId);
                    SqlParameter paramSnapshotStatus = new SqlParameter(ParamSnapshotStatus, SnapshotStatus);
                    SqlParameter paramPolicyMetricsSeverity = new SqlParameter(ParamPolicyMetricsSeverity, PolicyMetricSeverity);
                    SqlParameter paramRecipients = new SqlParameter(ParamRecipients, Recipients);

                    ExecuteNonQueryLocal(connection, CommandType.StoredProcedure,
                                                  NonQueryUpdateRegisteredServerNotification, new SqlParameter[]
                                                                         {   paramRegisteredServerId,
                                                                             paramProviderId,
                                                                             paramSnapshotStatus,
                                                                             paramPolicyMetricsSeverity,
                                                                             paramRecipients
                                                                         });
                }
            }
            catch (Exception ex)
            {
                LOG.Error(string.Format("Failed to update register server notification. error message: {1}", ex.Message));
                success = false;
            }

            return success;
        }



        public bool AddNotificationProvider(string connectionString)
        {
            bool success = true;

            try
            {
                // Open connection to repository and add server.
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Setup register server params.
                    // Setup register server params.
                    SqlParameter paramRegisteredServerId = new SqlParameter(ParamRegisteredServerId, RegisteredServerId);
                    SqlParameter paramProviderId = new SqlParameter(ParamNotificationProviderId, ProviderId);
                    SqlParameter paramSnapshotStatus = new SqlParameter(ParamSnapshotStatus, SnapshotStatus);
                    SqlParameter paramPolicyMetricsSeverity = new SqlParameter(ParamPolicyMetricsSeverity, PolicyMetricSeverity);
                    SqlParameter paramRecipients = new SqlParameter(ParamRecipients, Recipients);

                    ExecuteNonQueryLocal(connection, CommandType.StoredProcedure,
                                                  NonQueryAddRegisteredServerNotification, new SqlParameter[]
                                                                         {   paramRegisteredServerId,
                                                                             paramProviderId,
                                                                             paramSnapshotStatus,
                                                                             paramPolicyMetricsSeverity,
                                                                             paramRecipients
                                                                         });
                }
            }
            catch (Exception ex)
            {
                LOG.Error(string.Format("Failed to add Registered Server notification. error message: {1}", ex.Message));
                success = false;
            }

            return success;
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
