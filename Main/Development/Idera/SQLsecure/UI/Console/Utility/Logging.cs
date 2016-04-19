/******************************************************************
 * Name: Logging.cs
 *
 * Description: Diagnostic and event logging wrapper functions are
 * defined in this file.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

using Idera.SQLsecure.Core.Logger;

namespace Idera.SQLsecure.UI.Console.Utility
{
    internal static class ActivityLog
    {
        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Utility.ActivityLog");


        // Application Activy table
        // ------------------------
        internal const string ActivityCategory_Console = "SQLsecure Console";
        internal const string ActivityCategory_Job = "Job";
        internal const string ActivityType_Error = "Error";
        internal const string ActivityType_Warning = "Warning";
        internal const string ActivityType_Info = "Information";
        internal const string ActivityEvent_Start = "Start";
        internal const string ActivityEvent_Metrics = "Metrics";
        internal const string ActivityEvent_Error = "Error";


        private const string NonQueryCreateApplicationActivity =
         @"INSERT INTO SQLsecure.dbo.applicationactivity 
                            (eventtimestamp, activitytype, applicationsource, serverlogin, 
                             eventcode, category, description, connectionname)
                      VALUES (@eventtimestamp, @activitytype, @applicationsource, @serverlogin, 
                                @eventcode, @category, @description, @connectionname)";

        // ApplicationActivity Table
        // -------------------------
        private const string ParamEventTimestamp = "eventtimestamp";
        private const string ParamActivityType = "activitytype";
        private const string ParamApplicationSource = "applicationsource";
        private const string ParamServerLogin = "serverlogin";
        private const string ParamEventCode = "eventcode";
        private const string ParamCategory = "category";
        private const string ParamDescription = "description";
        private const string ParamConnectionName = "connectionname";


        public static bool CreateApplicationActivityEventInRepository(
                           string connectionString,
                           string targetServer,
                           string activityCategory,
                           string activityType,
                           string eventcode,
                           string description
           )
        {
            bool isOK = true;
            string serverlogin = Environment.UserDomainName + @"\" + Environment.UserName;
            using (SqlConnection repository = new SqlConnection(connectionString))
            {
                try
                {
                    // Open the connection.
                    repository.Open();

                    // Now Create the application activity
                    // -----------------------------------
                    // The timestamp must be in UTC.
                    SqlParameter paramEventTimestamp = new SqlParameter(ParamEventTimestamp, DateTime.Now.ToUniversalTime());
                    SqlParameter paramActivityType = new SqlParameter(ParamActivityType, activityType);
                    SqlParameter paramApplicationSource = new SqlParameter(ParamApplicationSource, "Collector");
                    SqlParameter paramServerLogin = new SqlParameter(ParamServerLogin, serverlogin);
                    SqlParameter paramEventCode = new SqlParameter(ParamEventCode, eventcode);
                    SqlParameter paramCategory = new SqlParameter(ParamCategory, activityCategory);
                    SqlParameter paramDescription = new SqlParameter(ParamDescription, description);
                    SqlParameter paramConnectionName = new SqlParameter(ParamConnectionName, targetServer);

                    Sql.SqlHelper.ExecuteNonQuery(repository, CommandType.Text,
                                    NonQueryCreateApplicationActivity,
                                    new SqlParameter[] 
                                                    { paramEventTimestamp, paramActivityType, 
                                                      paramApplicationSource, paramServerLogin,
                                                      paramEventCode, paramCategory, 
                                                      paramDescription, paramConnectionName });
                }
                catch (SqlException ex)
                {
                    logX.loggerX.Error("ERROR - exception raised when creating a snapshot entry", ex);
                    isOK = false;
                }
            }
            return isOK;
        }
    }
}
