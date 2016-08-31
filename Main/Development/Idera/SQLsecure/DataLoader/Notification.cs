using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Idera.SQLsecure.Core.Accounts;
using Idera.SQLsecure.Core.Logger;

namespace Idera.SQLsecure.Collector
{
    class Notification
    {
        private static LogX logX = new LogX("Idera.SQLsecure.Collector.Notification");

        private RegisteredServerNotification rSN = null;
        private NotificationProvider notificationProvider = null;
        private string m_SQLServer;
        private int m_SQLServerId;
        private string m_connectionString;

        Server.WriteActivityToRepositoryDelegate WriteAppActivityToRepository;


        private const string QueryGetAssessment =
            @"SQLsecure.dbo.isp_sqlsecure_getpolicyassessment";
        private const string ParamPolicyId = @"@policyid";
        private const string ParamAssessmentId = @"@assessmentid";
        private const string ParamRegisteredServerId = @"@registeredserverid";
        private const string ParamAlertsOnly = @"@alertsonly";
        private const string ParamBaselineOnly = @"@usebaseline";
        private const string ParamRunDate = @"@rundate";

        // Assessment details columns
        private const string colConnection = @"connectionname";
        private const string colSnapshotId = @"snapshotid";
        private const string colRegisteredServerId = @"registeredserverid";
        private const string colCollectionTime = @"collectiontime";
        private const string colMetricId = @"metricid";
        private const string colMetricName = @"metricname";
        private const string colMetricType = @"metrictype";
        private const string colMetricSeverityCode = @"metricseveritycode";
        private const string colMetricSeverity = @"metricseverity";
        private const string colMetricDescription = @"metricdescription";
        private const string colReportKey = @"metricreportkey";
        private const string colReportText = @"metricreporttext";
        private const string colSeverityCode = @"severitycode";
        private const string colSeverity = @"severity";
        private const string colCurrentValue = @"currentvalue";
        private const string colThresholdValue = @"thresholdvalue";



        public Notification(Server.WriteActivityToRepositoryDelegate delegateWriteActivityToRepository, string SQLServer, string connectionString, int timeout, int rId)
        {
            try
            {
                m_SQLServer = SQLServer;
                m_SQLServerId = rId;
                m_connectionString = connectionString;
                WriteAppActivityToRepository = delegateWriteActivityToRepository;

                notificationProvider = NotificationProvider.LoadNotificationProvider(connectionString, timeout);

                rSN =
                    RegisteredServerNotification.LoadRegisteredServerNotification(connectionString,
                                                                                  timeout,
                                                                                  rId,
                                                                                  notificationProvider.ProviderId);
            }
            catch(Exception ex)
            {
                logX.loggerX.Error(string.Format("Error reading Notification Provider from Repository: {0}",ex.Message));
            }
        }


        public void ProcessStatusNotification(char snapshotstatus, string statusDetails, string collectionDetails)
        {
            try
            {
                string notificationsettingString = string.Empty;
                if(rSN == null)
                {
                    return;
                }
                switch (rSN.SnapshotStatus)
                {
                    case RegisteredServerNotification.SnapshotStatusNotification_Never:
                        // No Notification requested
                        return;
                    case RegisteredServerNotification.SnapshotStatusNotification_Always:
                        // Notification requested for all collection jobs
                        notificationsettingString = "Send Email notification after data collection always.";
                        break;
                    case RegisteredServerNotification.SnapshotStatusNotification_OnlyOnError:
                        // Notification requested for Errored collection jobs only
                        if (snapshotstatus != Constants.StatusError)
                        {
                            return;
                        }
                        notificationsettingString = "Send Email notification after data collection only on Error.";
                        break;
                    case RegisteredServerNotification.SnapshotStatusNotification_OnWarningOrError:
                        // Notification requested for Warning or Error collection jobs
                        if (!(snapshotstatus == Constants.StatusError || snapshotstatus == Constants.StatusWarning) )
                        {
                            return;
                        }
                        notificationsettingString = "Send Email notification after data collection on Error or Warning.";
                        break;
                }

                string SubjectMsg = "SQLSecure Collection {0} for {1} at {2} {3}";
                string status = string.Empty;
                string subject = string.Empty;
                if (snapshotstatus == Constants.StatusError)
                {
                    status = "Error";
                }
                else if(snapshotstatus == Constants.StatusWarning)
                {
                    status = "Warning";
                }
                else
                {
                    status = "OK";
                }
                subject =
                    string.Format(SubjectMsg, status, m_SQLServer, DateTime.Now.ToShortDateString(),
                                  DateTime.Now.ToShortTimeString());

                StringBuilder message = new StringBuilder();
                message.AppendFormat("SQL Server: {0}", m_SQLServer);
                message.AppendFormat("\r\n\r\nEmail Notification Setting: {0}", notificationsettingString);
                message.AppendFormat("\r\n\r\nCollection Time: {0} at {1}", DateTime.Now.ToShortDateString(),
                                     DateTime.Now.ToShortTimeString());
                message.AppendFormat("\r\n\r\nCollection Status:  {0}", status);
                if (snapshotstatus == Constants.StatusError)
                {
                    message.Append("\r\n Error Details:");
                    message.Append("\r\n");
                    message.Append(statusDetails);
                }
                else if (snapshotstatus == Constants.StatusWarning)
                {
                    message.Append("\r\n Warning Details:");
                    message.Append("\r\n");
                    string[] warning = statusDetails.Split(new string[] { " and " }, StringSplitOptions.RemoveEmptyEntries);
                    if (warning.GetLength(0) > 1)
                    {
                        for (int x = 0; x < warning.GetLength(0); x++)
                        {
                            message.AppendFormat("  {0}) {1}\r\n", x + 1, warning[x].Trim());
                        }
                    }
                    else
                    {
                        message.AppendFormat("  {0} \r\n", warning[0].Trim());
                    }
                }
                message.Append("\r\nCollection Details: \r\n");
                string[] details = collectionDetails.Split(',');
                for (int x = 0; x < details.GetLength(0); x++)
                {
                    message.AppendFormat("  {0}\r\n", details[x].Trim());
                }

                if (!string.IsNullOrEmpty(rSN.Recipients) && notificationProvider.IsValid())
                {
                    notificationProvider.SendMessage(rSN.Recipients, subject, message.ToString());

                    string msg =
                        string.Format("SQLSecure Collection Status Notification sent to {0} for {1}", rSN.Recipients,
                                      m_SQLServer);
                    WriteAppActivityToRepository(Constants.ActivityType_Info, Constants.ActivityType_Info, msg);
                    logX.loggerX.Info(msg);
                }
                else
                {
                    string activityType = Constants.ActivityType_Error;
                    string errorMsg;
                    if (string.IsNullOrEmpty(rSN.Recipients))
                    {
                        errorMsg = "Error sending SQLSecure Collection Status Notification: Recipient not specified";
                    }
                    else
                    {
                        errorMsg =
                            "Error sending SQLSecure Collection Status Notification: SMTP Email Provider not configured";
                    }

                    WriteAppActivityToRepository(activityType, activityType, errorMsg);

                    logX.loggerX.Error(errorMsg);
                }
            }
            catch (Exception ex)
            {
                string activityType = Constants.ActivityType_Error;
                string errorMsg = string.Format("Error sending Collection Status Notification: {0}", ex.Message);
                WriteAppActivityToRepository(activityType, activityType, errorMsg);
                logX.loggerX.Error(errorMsg);
            }

        }

        public void ProcessFindingNotification()
        {
            try
            {
                if(rSN == null || rSN.PolicyMetricSeverity ==
                             (int) RegisteredServerNotification.MetricSeverityNotificaiton.Never)
                {
                    return;
                }
                string subject = "Collector Finding";
                StringBuilder message = new StringBuilder();

                string findingString = string.Empty;               

                // Get all policies this server is a member of
                List<Policy> policies = Policy.LoadPolicies(m_connectionString, m_SQLServerId);

                foreach (Policy p in policies)
                {
                    StringBuilder messageHigh = new StringBuilder();
                    StringBuilder messageMedium = new StringBuilder();
                    StringBuilder messageLow = new StringBuilder();

                    LoadPolicyFindings(p);
                    
                    // Build Email body
                    foreach (Finding f in p.Findings)
                    {
                        // High Risk Findings
                        if ((rSN.PolicyMetricSeverity ==
                             (int) RegisteredServerNotification.MetricSeverityNotificaiton.Any
                             && f.SeverityCode == 3)
                            ||
                            (rSN.PolicyMetricSeverity ==
                             (int) RegisteredServerNotification.MetricSeverityNotificaiton.OnWarningAndError
                             && f.SeverityCode == 3)
                            ||
                            (rSN.PolicyMetricSeverity ==
                             (int) RegisteredServerNotification.MetricSeverityNotificaiton.OnlyOnError
                             && f.SeverityCode == 3))
                        {
                            p.NumHighFindings++;
                            messageHigh.AppendFormat("\r\nSecurity Check: {0}", f.MetricReportText);
                            messageHigh.AppendFormat("\r\n  {0}", f.ThresholdValue);
                            messageHigh.AppendFormat("\r\n  {0}", f.CurrentValue);
                            messageHigh.Append("\r\n");
                        }
                        // Medium Risk Findings
                        if ((rSN.PolicyMetricSeverity ==
                             (int)RegisteredServerNotification.MetricSeverityNotificaiton.Any
                             && f.SeverityCode == 2)
                            ||
                            (rSN.PolicyMetricSeverity ==
                             (int)RegisteredServerNotification.MetricSeverityNotificaiton.OnWarningAndError
                             && f.SeverityCode == 2) )
                        {
                            p.NumMediumFindings++;
                            messageMedium.AppendFormat("\r\nSecurity Check: {0}", f.MetricReportText);
                            messageMedium.AppendFormat("\r\n  {0}", f.ThresholdValue);
                            messageMedium.AppendFormat("\r\n  {0}", f.CurrentValue);
                            messageMedium.Append("\r\n");
                        }
                        // Low Risk Findings
                        if ((rSN.PolicyMetricSeverity ==
                             (int)RegisteredServerNotification.MetricSeverityNotificaiton.Any
                             && f.SeverityCode == 1) )
                        {
                            p.NumLowFindings++;
                            messageLow.AppendFormat("\r\nSecurity Check: {0}", f.MetricReportText);
                            messageLow.AppendFormat("\r\n  {0}", f.ThresholdValue);
                            messageLow.AppendFormat("\r\n  {0}", f.CurrentValue);
                            messageLow.Append("\r\n");
                        }
                    }


                    if (p.NumHighFindings + p.NumMediumFindings + p.NumLowFindings > 0)
                    {
                        p.Message.AppendFormat("\r\n\r\n---------- {0} Policy Details ----------", p.PolicyName);

                        p.Message.AppendFormat("\r\n\r\nHigh Risk Findings: {0}", p.NumHighFindings);
                        p.Message.Append("\r\n------------------\r\n");
                        p.Message.Append(messageHigh);

                        if (rSN.PolicyMetricSeverity != (int)RegisteredServerNotification.MetricSeverityNotificaiton.OnlyOnError)
                        {
                            p.Message.AppendFormat("\r\nMedium Risk Findings: {0}", p.NumMediumFindings);
                            p.Message.Append("\r\n--------------------\r\n");
                            p.Message.Append(messageMedium);
                        }
                        if (rSN.PolicyMetricSeverity == (int)RegisteredServerNotification.MetricSeverityNotificaiton.Any)
                        {
                            p.Message.AppendFormat("\r\nLow Risk Findings: {0}", p.NumLowFindings);
                            p.Message.Append("\r\n----------------\r\n");
                            p.Message.Append(messageLow);
                        }
                    }
                }


                // Build Email Body Message
                int numFindings = 0;
                message.AppendFormat("Security Risks found for {0}", m_SQLServer);
                string notifyType = "Send Email notification for security findings at any risk.";
                if(rSN.PolicyMetricSeverity == (int)RegisteredServerNotification.MetricSeverityNotificaiton.OnWarningAndError)
                {
                    notifyType = "Send Email notification for security findings on high and medium risks.";
                }
                if(rSN.PolicyMetricSeverity == (int)RegisteredServerNotification.MetricSeverityNotificaiton.OnlyOnError)
                {
                    notifyType = "Send Email notification for security findings only on high risks.";
                }
                message.AppendFormat("\r\n\r\nEmail Notification Setting: {0}", notifyType);

                if(policies.Count > 0)
                {
                    message.AppendFormat("\r\n\r\nSecurity Check Findings for {0} by Policy:", m_SQLServer);
                    foreach (Policy p in policies)
                    {
                        int totalFindings = p.NumHighFindings + p.NumMediumFindings + p.NumLowFindings;
                        message.AppendFormat("\r\n  {0}: {1} Security Checks - {2} Risk{3}({4} High", 
                            p.PolicyName, 
                            p.Findings.Count, 
                            totalFindings,
                            totalFindings > 1 ? "s " : " ",
                            p.NumHighFindings);
                        if (p.NumMediumFindings > 0)
                            message.AppendFormat(", {0} Medium", p.NumMediumFindings);
                        if(p.NumLowFindings > 0)
                            message.AppendFormat(", {0} Low", p.NumLowFindings);
                        message.Append(")");
                        numFindings += p.NumHighFindings;
                        numFindings += p.NumMediumFindings;
                        numFindings += p.NumLowFindings;
                    }
                }

                foreach (Policy pol in policies)
                {
                    message.Append(pol.Message);
                }
                
                string SubjectMsg = "SQLSecure Security Findings for {0} at {1} {2}";
                subject = string.Format(SubjectMsg, m_SQLServer, 
                                        DateTime.Now.ToShortDateString(),DateTime.Now.ToShortTimeString());
                if (numFindings > 0)
                {
                    if (!string.IsNullOrEmpty(rSN.Recipients) && notificationProvider.IsValid())
                    {
                        notificationProvider.SendMessage(rSN.Recipients, subject, message.ToString());

                        string msg =
                            string.Format("SQLSecure Collection Finding Notification sent to {0} for {1}",
                                          rSN.Recipients,
                                          m_SQLServer);
                        WriteAppActivityToRepository(Constants.ActivityType_Info, Constants.ActivityType_Info, msg);
                        logX.loggerX.Info(msg);
                    }
                    else
                    {
                        string activityType = Constants.ActivityType_Error;
                        string errorMsg;
                        if (string.IsNullOrEmpty(rSN.Recipients))
                        {
                            errorMsg =
                                "Error sending SQLSecure Collection Finding Notification: Recipient not specified";
                        }
                        else
                        {
                            errorMsg =
                                "Error sending SQLSecure Collection Finding Notification: SMPT Email Provider not configured";
                        }

                        WriteAppActivityToRepository(activityType, activityType, errorMsg);

                        logX.loggerX.Error(errorMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                string activityType = Constants.ActivityType_Error;
                string errorMsg = string.Format("Error sending Security Finding Notification: {0}", ex.Message);
                WriteAppActivityToRepository(activityType, activityType, errorMsg);
                logX.loggerX.Error(errorMsg);
            }
        }


        #region Helpers

        private void LoadPolicyFindings(Policy policy)
        {
            try
            {
                
                // Open connection to repository and query permissions.
                using (SqlConnection connection = new SqlConnection(m_connectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Setup parameters for all queries
                    SqlParameter paramPolicyId = new SqlParameter(ParamPolicyId, policy.PolicyId);
                    SqlParameter paramAssessmentId = new SqlParameter(ParamAssessmentId, policy.AssessmentId);
                    SqlParameter paramRegisteredServerId = new SqlParameter(ParamRegisteredServerId, m_SQLServerId);
                    paramRegisteredServerId.IsNullable = true;
                    SqlParameter paramAlertsOnly = new SqlParameter(ParamAlertsOnly, SqlDbType.Bit, 0);
                    paramAlertsOnly.Value = 0;
                    SqlParameter paramBaselineOnly = new SqlParameter(ParamBaselineOnly, SqlDbType.Bit, 0);
                    paramBaselineOnly.Value = false;
                    SqlParameter paramRunDate = new SqlParameter(ParamRunDate, DateTime.Now.ToUniversalTime());

                    using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.StoredProcedure,
                                                                           QueryGetAssessment,
                                                                           new SqlParameter[] { paramPolicyId, paramAssessmentId,
                                                                                                 paramRegisteredServerId, 
                                                                                                 paramAlertsOnly, paramBaselineOnly,
                                                                                                 paramRunDate}))
                    {
                        while (rdr.Read())
                        {
                            Finding f = new  Finding(policy.PolicyName, rdr);
                            policy.Findings.Add(f);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error(string.Format("Error loading Policy Findings: {0}", ex.Message));
            }
        }


        private class Finding
        {
            private string m_PolicyName;
            private string m_MetricName;
            private string m_MetricReportText;
            private string m_Severity;
            private int m_SeverityCode;
            private string m_CurrentValue;
            private string m_ThresholdValue;

            public string PolicyName
            {
                get { return m_PolicyName;}
            }
            public string MetricName
            {
                get { return m_MetricName;}
            }
            public string MetricReportText
            {
                get { return m_MetricReportText;}
            }
            public string Severity
            {
                get { return m_Severity;}
            }
            public int SeverityCode
            {
                get { return m_SeverityCode; }
            }
            public string CurrentValue
            {
                get { return m_CurrentValue;}
            }
            public string ThresholdValue
            {
                get { return m_ThresholdValue;}
            }

            private enum FindingColumn
            {
                SnapshotId = 0,
			    RegisteredServerId,
				ConnectionName,
				CollectionTime,
				MetricId,
                MetricName,
				MetricYype,
				MetricSeverityCode,
				MetricSeverity,
                MetricSeverityValues,
				MetricDescription,
				MetricReportKey,
				MetricReportText,
				SeverityCode,
				Severity,
				CurrentValue,
				ThresholdValue
            }


            public Finding(string policyName, SqlDataReader rdr)
            {
                m_PolicyName = policyName;

                m_MetricName = rdr.GetSqlString((int)FindingColumn.MetricName).Value;
                m_MetricReportText = rdr.GetSqlString((int)FindingColumn.MetricReportText).Value;
                m_Severity = rdr.GetSqlString((int)FindingColumn.Severity).Value;
                m_SeverityCode = rdr.GetSqlInt32((int)FindingColumn.SeverityCode).Value;
                m_CurrentValue = rdr.GetSqlString((int)FindingColumn.CurrentValue).Value;
                m_ThresholdValue = rdr.GetSqlString((int)FindingColumn.ThresholdValue).Value;

            }  

        }

        private class Policy
        {
            private string m_PolicyName;
            private int m_PolicyId;
            private int m_AssessmentId;
            private List<Finding> m_Findings = new List<Finding>();
            private StringBuilder m_message = new StringBuilder();
            private int m_numHighFindings = 0;
            private int m_numMediumFindings = 0;
            private int m_numLowFindings = 0;

            public string PolicyName
            {
                get { return m_PolicyName; }
                set { m_PolicyName = value;}
            }

            public int PolicyId
            {
                get { return m_PolicyId; }
                set { m_PolicyId = value; }
            }

            public int AssessmentId
            {
                get { return m_AssessmentId; }
                set { m_AssessmentId = value; }
            }

            public List<Finding> Findings
            {
                get { return m_Findings; }
                set { m_Findings = value;}
            }

            public StringBuilder Message
            {
                get { return m_message; }
            }

            public int NumHighFindings
            {
                get { return m_numHighFindings; }
                set { m_numHighFindings = value;}
            }

            public int NumMediumFindings
            {
                get { return m_numMediumFindings; }
                set { m_numMediumFindings = value; }
            }

            public int NumLowFindings
            {
                get { return m_numLowFindings; }
                set { m_numLowFindings = value; }
            }

            private const string QueryGetServerPolicyList = "SQLsecure.dbo.isp_sqlsecure_getserverpolicylist";

            private enum PolicyColumn
            {
                PolicyId = 0,
                AssessmentId,
                PolicyName,
            }

            public Policy(SqlDataReader rdr)
            {
                m_PolicyId = rdr.GetSqlInt32((int)PolicyColumn.PolicyId).Value;
                m_AssessmentId = rdr.GetSqlInt32((int)PolicyColumn.AssessmentId).Value;
                m_PolicyName = rdr.GetSqlString((int)PolicyColumn.PolicyName).Value;                
            }

            static public List<Policy> LoadPolicies(string connectionString, int rId)
            {
                List<Policy> polices = new List<Policy>();
                try
                {

                    // Open connection to repository and query permissions.
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        // Open the connection.
                        connection.Open();

                        // Setup parameters for all queries
                        SqlParameter paramRegisteredServerId = new SqlParameter(ParamRegisteredServerId, rId);
                        using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.StoredProcedure,
                                                                               QueryGetServerPolicyList,
                                                                               new SqlParameter[] { paramRegisteredServerId }))
                        {
                            while (rdr.Read())
                            {
                                Policy p = new Policy(rdr);
                                polices.Add(p);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    logX.loggerX.Error(string.Format("Error loading Policy Findings: {0}", ex.Message));
                }

                return polices;
            }
        }

        #endregion

    }
}
