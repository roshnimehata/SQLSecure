using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

using Idera.SQLsecure.Core.Logger;

namespace Idera.SQLsecure.UI.Console.Sql
{
    public class ScheduleJob
    {
        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Sql.ScheduleJob");

        #region Queries & Constants

        private const string QueryAddJob = @"SQLsecure.dbo.isp_sqlsecure_addnewsnapshotjob";

   	    private const string ColParamConnectionname = "@connectionname";
        private const string ColParamJobName = "@snapshotjobname";
        private const string ColParamJobDescription = "@jobdescription";
        private const string ColParamTargetServer = "@targetserver";
        private const string ColParamRepository = "@repositoryname";
        private const string ColParamFreqType = "@freqtype";
        private const string ColParamFreqInterval = "@freqinterval";
        private const string ColParamFreqSubDayType = "@freqsubdaytype";
        private const string ColParamFreqSubDayInterval = "@freqsubdayinterval";
        private const string ColParamRelativeInterval = "@freqrelativeinterval";
        private const string ColParamRecurranceFactor = "@freqrecurencefactor";
        private const string ColParamStartDate = "@activestartdate";
        private const string ColParamEndDate = "@activeenddate";
        private const string ColParamStartTime = "@activestarttime";
        private const string ColParamEndTime = "@activeendtime";
        private const string ColParamIsEnabled = "@isenabled";
        private const string ColParamNotifyLevelEmail = "@notifylevelemail";
        private const string ColParamNotifyEmailOperatorName = "@notifyemailoperatorname";

        private const string QueryAddGroomingJob = @"SQLsecure.dbo.isp_sqlsecure_addnewgroomingjob";
        private const string ColParamGroomJobName = "@groomjobname";


        private const string NonQueryRemoveJob = @"SQLsecure.dbo.isp_sqlsecure_removejob";
        private const string QueryGetSchedule = @"SQLsecure.dbo.isp_sqlsecure_listjobschedule";
        private const string ColParamJobId = "@jobid";
        private const string ColRemoveJobName = "@jobname";
        private const string ColResultFreqType = "freqtype";

        private const string NonQueryStartJob = @"SQLsecure.dbo.isp_sqlsecure_startjob";

        private const string QueryGroomingJobList = @"SQLsecure.dbo.isp_sqlsecure_listallgroomingjobs";
        private const string ColParamGroomJobId = @"job_id";

        private const string QueryJobList = @"SQLsecure.dbo.isp_sqlsecure_listallsnapshotjobs";
        private const string ColJobListJobId = @"job_id";
        private const string ColJobListJobName = @"name";
        private const string ColJobListEnabled = @"enabled";
        private const string ColJobListDescription = @"description";
        private const string ColJobListLastRunDate = @"last_run_date";
        private const string ColJobListLastRunTime = @"last_run_time";
        private const string ColJobListNextRunDate = @"next_run_date";
        private const string ColJobListNextRunTime = @"next_run_time";

        private const string QueryIsSQLAgentStarted = @"SQLsecure.dbo.isp_sqlsecure_issqlagentrunning";
        private const string QueryJobStatus = @"SQLsecure.dbo.isp_sqlsecure_getjobstatus";

        #endregion

        #region Enums & Constants

        private enum SQLScheduleColumns
        {
            ScheduleID,
            ScheduleName,
            Enabled,
            FreqType,
            FreqInterval,
            FreqSubDayType,
            FreqSubDayInterval,
            FreqRelativeInterval,
            FreqRecurrenceFactor,
            ActiveStartDate,
            ActiveEndDate,
            ActiveStartTime,
            ActiveEndTime,
            DateCreated,
            Description,
            NextRunDate,
            NextRunTime,
            ScheduleUID,
            JobCount
        }

        public enum StartJobReturnCode
        {
            Success,
            JobNotFound,
            AgentNotStarted,
            JobAlreadyRunning,
            UnknownError
        }

        public enum JobStatus
        {
            JobStatus_Error,
            JobStatus_Idle,
            JobStatus_Running,
            JobStatus_Missing
        }

        public const string JobStatus_Running = "Running";
        public const string JobStatus_Failed = "Failed";
        public const string JobStatus_Succeeded = "Succeeded";
        public const string JobStatus_NotFound = "Not Found";
        public const string JobStatus_NotRunning = "Not Running";

        private const string JobDateJoin = "{0} {1}";
        private static string[] JobDateFormat = new string[]{ "yyyyMMdd Hmmss", "yyyyMMdd HHmmss", "yyyyMMdd hmmss" };
        private static string RowFilterJobID = ColJobListJobId + " = '{0}'";

        #endregion

        #region Schedule Data Types
        public enum OccurType
        {
            OccursDaily,
            OccursWeekly,
            OccursMonthly
        }
        public enum MonthlyOccurType
        {
            MonthlyOccurDay,
            MonthlyOccurSpecificDay
        }
        public enum FrequencyType
        {
            FrequencyOnce,
            FrequencyEvery
        }
        public enum MonthlyWhichOccurance
        {
            MonthlyOccuranceFirst = 1,
            MonthlyOccuranceSecond = 2,
            MonthlyOccuranceThird = 4,
            MonthlyOccuranceFouth = 8,
            MonthlyOccuranceLast = 16
        }
        public enum MonthlyDay
        {
            MonthlyDaySunday = 1,
            MonthlyDayMonday,
            MonthlyDayTuesday,
            MonthlyDayWednesday,
            MonthlyDayThursday,
            MonthlyDayFriday,
            MonthlyDaySaturday,
            MonthlyDayEveryDay,
            MonthlyDayWeekday,
            MonthlyDayWeekendDay
        }
        public enum FrequencyUnit
        {
            FreqencyUnitSeconds,
            FreqencyUnitMinutes,
            FreqencyUnitHours
        }

        public struct ScheduleData
        {
            public void SetDefaults()
            {
                // Set defaults for scheduled data
                // Every sunday at 3am, keep data for 60 days
                // ------------------------------------------
                Enabled = true;
                occurType = ScheduleJob.OccurType.OccursWeekly;
                weekly_isSunday = true;
                weekly_RepeatRate = 1;
                freq_Type = ScheduleJob.FrequencyType.FrequencyOnce;
                DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                            3, 0, 0);
                freq_OnceAtTime = dt;

                snapshotretentionPeriod = 60;

            }
            public bool Enabled;
            public string Description;

            public int snapshotretentionPeriod;

            public OccurType occurType;

            // Daily values
            // ------------
            public uint daily_RepeatRate;

            // Weekly values
            // -------------
            public uint weekly_RepeatRate;
            public bool weekly_isMonday;
            public bool weekly_isTuesday;
            public bool weekly_isWednesday;
            public bool weekly_isThursday;
            public bool weekly_isFriday;
            public bool weekly_isSaturday;
            public bool weekly_isSunday;

            // Monthly values
            // --------------
            public MonthlyOccurType monthly_type;
            public uint monthly_dayOfMonth;
            public uint monthly_repeatRate;
            public MonthlyWhichOccurance monthly_SpecificOccurance;
            public MonthlyDay monthly_SpecificDay;
            public uint monthly_SpecificRepeatRate;

            // Frequency values
            // ----------------
            public FrequencyType freq_Type;
            public DateTime freq_OnceAtTime;
            public uint freq_RepeatRate;
            public FrequencyUnit freq_Unit;
            public DateTime freq_Start;
            public DateTime freq_End;
        }

        public struct JobData
        {
            public Guid JobId;
            public string Server;
            public string JobName;
            public bool Enabled;
            public string Description;
            public DateTime LastRunDate;
            public string LastRun
            {
                get 
                {
                    if (LastRunDate == DateTime.MinValue)
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return LastRunDate.ToString(Utility.Constants.DATETIME_FORMAT);
                    }
                }
            }
            public DateTime NextRunDate;
            public string NextRun
            {
                get 
                {
                    if (NextRunDate == DateTime.MinValue)
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return NextRunDate.ToString(Utility.Constants.DATETIME_FORMAT);
                    }
                }
            }
        }

        #endregion

        #region Public Methods

        public static bool IsSQLAgentStarted(string connectionString)
        {
            Debug.Assert(!string.IsNullOrEmpty(connectionString));
            bool isSQLAgentStarted = false;

            try
            {
                // Open connection to repository and add job.
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the connection.
                    connection.Open();

                    SqlCommand cmd = new SqlCommand(QueryIsSQLAgentStarted, connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    string isStarted = (string)ds.Tables[0].Rows[0][0];
                    if( !string.IsNullOrEmpty(isStarted) )
                    {
                        if (isStarted == "Y")
                        {
                            isSQLAgentStarted = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.SQLsecureDataCollection, Utility.ErrorMsgs.DataCollectionErrorGettingAgentStatus, ex);
            }


            return isSQLAgentStarted;
        }

        public static string GetJobStatus(string connectionString, Guid jobID)
        {
            Debug.Assert(!string.IsNullOrEmpty(connectionString));
            Debug.Assert(jobID != Guid.Empty);
            string status = "Unknown";
            if(jobID == Guid.Empty)
            {
                return status;
            }
            try
            {
                // Open connection to repository and add job.
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the connection.
                    connection.Open();
                    SqlParameter paramJobId = new SqlParameter(ColParamJobId, jobID);

                    SqlCommand cmd = new SqlCommand(QueryJobStatus, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(paramJobId);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    status = (string)ds.Tables[0].Rows[0][0];
                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error(Utility.ErrorMsgs.DataCollectionErrorGettingJobStatus, ex.Message);
//                Utility.MsgBox.ShowError(Utility.ErrorMsgs.SQLsecureDataCollection, Utility.ErrorMsgs.DataCollectionErrorGettingJobStatus, ex);
            }

            return status;
        }

        public static string GetSnapshotJobName(string connectionName)
        {
            string jobName = "SQLsecure Collector Job - " + connectionName;
            return jobName;
        }

        public static StartJobReturnCode StartJob(string connectionString, Guid jobID)
        {
            Debug.Assert(!string.IsNullOrEmpty(connectionString));
            StartJobReturnCode returnCode = StartJobReturnCode.UnknownError;

            if (!IsSQLAgentStarted(connectionString))
            {
                return StartJobReturnCode.AgentNotStarted;
            }
            if (jobID == Guid.Empty)
            {
                return StartJobReturnCode.JobNotFound;
            }

            try
            {

                // Open connection to repository and add server.
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Setup remove job params.
                    SqlParameter paramJobId = new SqlParameter(ColParamJobId, jobID);

                    Sql.SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure,
                                    NonQueryStartJob, new SqlParameter[] { paramJobId });
                    returnCode = StartJobReturnCode.Success;
                }
            }
            catch (SqlException ex)
            {
                switch ((uint)ex.Number)
                {
                    case 14262:
                        returnCode = StartJobReturnCode.JobNotFound;
                        break;
                    case 22022:
                        returnCode = StartJobReturnCode.JobAlreadyRunning;
                        Utility.MsgBox.ShowError(Utility.ErrorMsgs.SQLsecureDataCollection, Utility.ErrorMsgs.JobAlreadyRunning, ex);
                        break;
                    default:
                        returnCode = StartJobReturnCode.UnknownError;
                        Utility.MsgBox.ShowError(Utility.ErrorMsgs.SQLsecureDataCollection, Utility.ErrorMsgs.CantRunDataCollection, ex);
                        break;
                }
            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.SQLsecureDataCollection, Utility.ErrorMsgs.CantRunDataCollection, ex);
                returnCode = StartJobReturnCode.UnknownError;
            }

            return returnCode;
        }

        public static Guid GetJobData(string connectionString, Guid jobID, out JobData jobData)
        {
            Debug.Assert(!string.IsNullOrEmpty(connectionString));

            Guid jobid = Guid.Empty;
            jobData = new JobData();

            try
            {
                // Open connection to repository and add server.
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // First we need to get the job id of the Groom job.
                    SqlCommand cmd = new SqlCommand(QueryJobList, connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    DataView dv = ds.Tables[0].DefaultView;
                    dv.RowFilter = string.Format(RowFilterJobID, jobID);

                    if (dv.Count > 0)
                    {
                        DataRowView drv = dv[0];
                        jobData.JobId = jobID;
                        jobData.JobName = (string)drv[ColJobListJobName];
                        jobData.Enabled = ((byte)drv[ColJobListEnabled] > 0);
                        jobData.Description = (string)drv[ColJobListDescription];
                        jobData.LastRunDate = GetDateTimeFromSQLInt((int)drv[ColJobListLastRunDate], (int)drv[ColJobListLastRunTime]);
                        jobData.NextRunDate = GetDateTimeFromSQLInt((int)drv[ColJobListNextRunDate], (int)drv[ColJobListNextRunTime]);

                        jobid = jobID;
                    }
                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error("Unable to retrieve Snapshot Job data", ex.Message);
            }

            return jobid;
        }

        public static Guid AddGroomingJob(string connectionString, ScheduleData scheduleData)
        {
            Debug.Assert(!string.IsNullOrEmpty(connectionString));

            string jobName = "SQLsecure Grooming Job";
            Guid jobID = Guid.Empty;
            try
            {
                System.Data.SqlTypes.SqlInt16 Enabled = (System.Data.SqlTypes.SqlInt16)((scheduleData.Enabled) ? 1 : 0);
                System.Data.SqlTypes.SqlInt32 FreqType = GetFrequencyType(scheduleData);
                System.Data.SqlTypes.SqlInt32 FreqInterval = GetFrequencyInterval((int)FreqType, scheduleData);
                System.Data.SqlTypes.SqlInt32 FreqSubDayType = GetFrequencySubDayType(scheduleData);
                System.Data.SqlTypes.SqlInt32 FreqSubDayInterval = GetFrequencySubDayInterval(scheduleData);
                System.Data.SqlTypes.SqlInt32 RelativeInterval = GetFreqRelativeInterval(scheduleData);
                System.Data.SqlTypes.SqlInt32 RecurrenceFactor = GetFreqRecurrenceFactor((int)FreqType, scheduleData);
                System.Data.SqlTypes.SqlInt32 StartTime = GetStartTime(scheduleData);
                System.Data.SqlTypes.SqlInt32 EndTime = GetEndTime(scheduleData);

                // Open connection to repository and add job.
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Setup add job params.
                    SqlParameter paramJobName = new SqlParameter(ColParamGroomJobName, jobName);
                    SqlParameter paramFreqType = new SqlParameter(ColParamFreqType, FreqType);
                    SqlParameter paramLFreqInterval = new SqlParameter(ColParamFreqInterval, FreqInterval);
                    SqlParameter paramFreqSubDayType = new SqlParameter(ColParamFreqSubDayType, FreqSubDayType);
                    SqlParameter paramFreqSubDayInterval = new SqlParameter(ColParamFreqSubDayInterval, FreqSubDayInterval);
                    SqlParameter paramRelativeInterval = new SqlParameter(ColParamRelativeInterval, RelativeInterval);
                    SqlParameter paramRecurranceFactor = new SqlParameter(ColParamRecurranceFactor, RecurrenceFactor);
                    SqlParameter paramStartTime = new SqlParameter(ColParamStartTime, StartTime);
                    SqlParameter paramEndTime = new SqlParameter(ColParamEndTime, EndTime);
                    SqlParameter paramEnabled = new SqlParameter(ColParamIsEnabled, Enabled);

                    SqlCommand cmd = new SqlCommand(QueryAddGroomingJob, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(paramJobName);
                    cmd.Parameters.Add(paramFreqType);
                    cmd.Parameters.Add(paramLFreqInterval);
                    cmd.Parameters.Add(paramFreqSubDayType);
                    cmd.Parameters.Add(paramFreqSubDayInterval);
                    cmd.Parameters.Add(paramRelativeInterval);
                    cmd.Parameters.Add(paramRecurranceFactor);
                    cmd.Parameters.Add(paramStartTime);
                    cmd.Parameters.Add(paramEndTime);
                    cmd.Parameters.Add(paramEnabled);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    jobID = (Guid)ds.Tables[0].Rows[0][0];


                }
            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.SQLsecureDataCollection, Utility.ErrorMsgs.DataCollectionErrorAddingGroomJob, ex);
            }

            return jobID;
        }

        public static Guid GetGroomingJobLastRun(string connectionString, out DateTime? lastGroomed)
        {
            Debug.Assert(!string.IsNullOrEmpty(connectionString));

            lastGroomed = null;
            Guid jobID = new Guid();
            DataRow row;

            try
            {
                // Open connection to repository and add server.
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // First we need to get the job id of the Groom job.
                    SqlCommand cmd = new SqlCommand(QueryGroomingJobList, connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        row = ds.Tables[0].Rows[0];
                        jobID = (Guid)row[0];
                        if (jobID != null && jobID != Guid.Empty)
                        {
                            string strval = row["last_run_date"].ToString();
                            if (strval != null && strval != "0")
                            {
                                lastGroomed = GetDateTimeFromSQLInt((int)row["last_run_date"], (int)row["last_run_time"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error("Unable to retrieve Grooming Job last run date", ex.Message);
            }

            return jobID;
        }

        public static Guid GetGroomingJobSchedule(string connectionString, out ScheduleData scheduleData)
        {
            Debug.Assert(!string.IsNullOrEmpty(connectionString));

            scheduleData = new ScheduleData();
            Guid jobID = new Guid();

            try
            {
                // Open connection to repository and add server.
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // First we need to get the job id of the Groom job.
                    SqlCommand cmd = new SqlCommand(QueryGroomingJobList, connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        jobID = (Guid)ds.Tables[0].Rows[0][0];
                    }
                    if(jobID == null || jobID == Guid.Empty)
                    {
                        // Set defaults for gromming schedule
                        // Every saturday at 3am
                        // -------------------------------
                        scheduleData.Enabled = false;
                        scheduleData.occurType = ScheduleJob.OccurType.OccursWeekly;
                        scheduleData.weekly_isSaturday = true;
                        scheduleData.weekly_RepeatRate = 1;
                        scheduleData.freq_Type = ScheduleJob.FrequencyType.FrequencyOnce;
                        DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                                    3, 0, 0);
                        scheduleData.freq_OnceAtTime = dt;

                        jobID = AddGroomingJob(Program.gController.Repository.ConnectionString, 
                                               scheduleData);
                    }

                    if (jobID != Guid.Empty)
                    {
                        GetJobSchedule(Program.gController.Repository.ConnectionString, jobID, out scheduleData);
                    }

                }
            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.SQLsecureDataCollection, Utility.ErrorMsgs.DataCollectionErrorGettingGroomJob, ex);
            }

            return jobID;
        }

        public static bool GetJobSchedule(string connectionString, Guid jobID, out ScheduleData scheduleData)
        {
            Debug.Assert(!string.IsNullOrEmpty(connectionString));
            bool isOk = true;

            scheduleData = new ScheduleData();
            scheduleData.SetDefaults();
            scheduleData.Enabled = false;

            if (jobID == Guid.Empty)
            {
                return false;
            }

            try
            {

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Setup schedule params.
                    SqlParameter paramJobId = new SqlParameter(ColParamJobId, jobID);

                    //Get Schedule
                    SqlCommand cmd = new SqlCommand(QueryGetSchedule, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(paramJobId);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    int FreqType = (int)ds.Tables[0].Rows[0][(int)SQLScheduleColumns.FreqType];
                    int FreqInterval = (int)ds.Tables[0].Rows[0][(int)SQLScheduleColumns.FreqInterval];
                    int isEnabled = (int)ds.Tables[0].Rows[0][(int)SQLScheduleColumns.Enabled];
                    int FreqSubDayType = (int)ds.Tables[0].Rows[0][(int)SQLScheduleColumns.FreqSubDayType];
                    int FreqSubDayInterval = (int)ds.Tables[0].Rows[0][(int)SQLScheduleColumns.FreqSubDayInterval];
                    int FreqRelativeInterval = (int)ds.Tables[0].Rows[0][(int)SQLScheduleColumns.FreqRelativeInterval];
                    int FreqRecurrenceFactor = (int)ds.Tables[0].Rows[0][(int)SQLScheduleColumns.FreqRecurrenceFactor];
                    int StartTime = (int)ds.Tables[0].Rows[0][(int)SQLScheduleColumns.ActiveStartTime];
                    int EndTime = (int)ds.Tables[0].Rows[0][(int)SQLScheduleColumns.ActiveEndTime]; 

                    scheduleData.Enabled = (isEnabled == 0) ? false : true;
                    SetFrequencyTypeInScheduleData(FreqType, ref scheduleData);
                    SetFrequencyIntervalInScheduleData(FreqType, FreqInterval, ref scheduleData);
                    SetFrequencySubDayTypeInScheduleData(FreqSubDayType, ref scheduleData);
                    SetFrequencySubDayIntervalInScheduleData(FreqSubDayInterval, ref scheduleData);
                    SetFreqRelativeIntervalInScheduleData(FreqRelativeInterval, ref scheduleData);
                    SetFreqRecurrenceFactorInScheduleData(FreqType, FreqRecurrenceFactor, ref scheduleData);
                    SetStartTimeInScheduleData(StartTime, ref scheduleData);
                    SetEndTimeInScheduleData(EndTime, ref scheduleData);

                    BuildDescription(ref scheduleData);
                       
                }
            }
            catch (Exception ex)
            {
                isOk = false;
                logX.loggerX.Error(Utility.ErrorMsgs.DataCollectionErrorGettingJob, ex.Message);
//                Utility.MsgBox.ShowError(Utility.ErrorMsgs.SQLsecureDataCollection, Utility.ErrorMsgs.DataCollectionErrorGettingJob, ex);
            }
            return isOk;
       }

        public static void RemoveJob(string connectionString, Guid jobID, string connectionName)
        {
            Debug.Assert(!string.IsNullOrEmpty(connectionString));
            if (jobID == Guid.Empty)
            {
                return;
            }
            try
            {

                // Open connection to repository and add server.
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Setup remove job params.
                    SqlParameter paramJobId = new SqlParameter(ColParamJobId, jobID);
                    SqlParameter paramJobName = new SqlParameter(ColRemoveJobName, GetSnapshotJobName(connectionName));

                    Sql.SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure,
                                    NonQueryRemoveJob, new SqlParameter[] { paramJobId, paramJobName });        

                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error(Utility.ErrorMsgs.DataCollectionErrorRemovingJob, ex.Message);
//                Utility.MsgBox.ShowError(Utility.ErrorMsgs.SQLsecureDataCollection, Utility.ErrorMsgs.DataCollectionErrorRemovingJob, ex);
            }
        }

        public static Guid AddJob(string connectionString, string newConnection,
            string repositoryName, ScheduleData scheduleData)
        {
            return AddJob(connectionString, newConnection, repositoryName, scheduleData, true);
        }
        public static Guid AddJob( string connectionString, string newConnection,
                                   string repositoryName, ScheduleData scheduleData, bool showError)
        {
            Debug.Assert(!string.IsNullOrEmpty(connectionString));
            Debug.Assert(!string.IsNullOrEmpty(newConnection));
            Debug.Assert(!string.IsNullOrEmpty(repositoryName));

            Guid jobID = Guid.Empty;
            try
            {
                System.Data.SqlTypes.SqlInt16 Enabled = (System.Data.SqlTypes.SqlInt16)((scheduleData.Enabled) ? 1 : 0);
                System.Data.SqlTypes.SqlInt32 FreqType = GetFrequencyType(scheduleData);
                System.Data.SqlTypes.SqlInt32 FreqInterval = GetFrequencyInterval((int)FreqType, scheduleData);
                System.Data.SqlTypes.SqlInt32 FreqSubDayType = GetFrequencySubDayType(scheduleData);
                System.Data.SqlTypes.SqlInt32 FreqSubDayInterval = GetFrequencySubDayInterval(scheduleData);
                System.Data.SqlTypes.SqlInt32 RelativeInterval = GetFreqRelativeInterval(scheduleData);
                System.Data.SqlTypes.SqlInt32 RecurrenceFactor = GetFreqRecurrenceFactor((int)FreqType, scheduleData);
                System.Data.SqlTypes.SqlInt32 StartTime = GetStartTime(scheduleData);
                System.Data.SqlTypes.SqlInt32 EndTime = GetEndTime(scheduleData);



                // Open connection to repository and add job.
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Setup add job params.
                    SqlParameter paramConnectionname = new SqlParameter(ColParamConnectionname, newConnection);
                    SqlParameter paramServername = new SqlParameter(ColParamTargetServer, newConnection);
                    SqlParameter paramJobName = new SqlParameter(ColParamJobName, GetSnapshotJobName(newConnection));
                    SqlParameter paramRepository = new SqlParameter(ColParamRepository, repositoryName);
                    SqlParameter paramFreqType = new SqlParameter(ColParamFreqType, FreqType);
                    SqlParameter paramLFreqInterval = new SqlParameter(ColParamFreqInterval, FreqInterval);
                    SqlParameter paramFreqSubDayType = new SqlParameter(ColParamFreqSubDayType, FreqSubDayType);
                    SqlParameter paramFreqSubDayInterval = new SqlParameter(ColParamFreqSubDayInterval, FreqSubDayInterval);
                    SqlParameter paramRelativeInterval = new SqlParameter(ColParamRelativeInterval, RelativeInterval);
                    SqlParameter paramRecurranceFactor = new SqlParameter(ColParamRecurranceFactor, RecurrenceFactor);
                    SqlParameter paramStartTime = new SqlParameter(ColParamStartTime, StartTime);
                    SqlParameter paramEndTime = new SqlParameter(ColParamEndTime, EndTime);
                    SqlParameter paramEnabled = new SqlParameter(ColParamIsEnabled, Enabled);

                    SqlCommand cmd = new SqlCommand(QueryAddJob, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(paramConnectionname);
                    cmd.Parameters.Add(paramServername);
                    cmd.Parameters.Add(paramJobName);
                    cmd.Parameters.Add(paramRepository);
                    cmd.Parameters.Add(paramFreqType);
                    cmd.Parameters.Add(paramLFreqInterval);
                    cmd.Parameters.Add(paramFreqSubDayType);
                    cmd.Parameters.Add(paramFreqSubDayInterval);
                    cmd.Parameters.Add(paramRelativeInterval);
                    cmd.Parameters.Add(paramRecurranceFactor);
                    cmd.Parameters.Add(paramStartTime);
                    cmd.Parameters.Add(paramEndTime);
                    cmd.Parameters.Add(paramEnabled);


                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    jobID = (Guid)ds.Tables[0].Rows[0][0];


                }
            }
            catch (Exception ex)
            {
                if (showError)
                    Utility.MsgBox.ShowError(Utility.ErrorMsgs.SQLsecureDataCollection,
                        Utility.ErrorMsgs.DataCollectionErrorAddingJob, ex);
                else throw;
            }

            return jobID;
        }

        public static void BuildDescription(ref ScheduleData scheduleData)
        {
            StringBuilder description = new StringBuilder("Occurs every ");

            if (scheduleData.Enabled == false)
            {
                scheduleData.Description = "Schedule is disabled";
                return;
            }

            switch (scheduleData.occurType)
            {
                case OccurType.OccursDaily:
                    if (scheduleData.daily_RepeatRate < 2)
                    {
                        if (scheduleData.freq_Type == FrequencyType.FrequencyOnce)
                        {
                            description.AppendFormat("day at {0}.", scheduleData.freq_OnceAtTime.ToLongTimeString());
                        }
                        else
                        {
                            description.AppendFormat("day every {0} {1} between {2} and {3}.",
                                                      scheduleData.freq_RepeatRate,
                                                      (scheduleData.freq_Unit == FrequencyUnit.FreqencyUnitHours) ? "Hour(s)" : "Minute(s)",
                                                      scheduleData.freq_Start.ToLongTimeString(),
                                                      scheduleData.freq_End.ToLongTimeString());
                        }
                    }
                    else
                    {
                        if (scheduleData.freq_Type == FrequencyType.FrequencyOnce)
                        {
                            description.AppendFormat("{0} days at {1}.", scheduleData.daily_RepeatRate, scheduleData.freq_OnceAtTime.TimeOfDay);
                        }
                        else
                        {
                            description.AppendFormat("{0} days every {1} {2} between {3} and {4}.",
                                                      scheduleData.daily_RepeatRate,
                                                      scheduleData.freq_RepeatRate,
                                                      (scheduleData.freq_Unit == FrequencyUnit.FreqencyUnitHours) ? "Hour(s)" : "Minute(s)",
                                                      scheduleData.freq_Start.ToLongTimeString(),
                                                      scheduleData.freq_End.ToLongTimeString());
                        }
                    }
                    break;
                case OccurType.OccursWeekly:
                    if (scheduleData.weekly_RepeatRate < 2)
                    {
                        if (scheduleData.freq_Type == FrequencyType.FrequencyOnce)
                        {
                            description.AppendFormat("week {0} at {1}",
                                                      BuildDaysOfWeekString(scheduleData),
                                                      scheduleData.freq_OnceAtTime.ToLongTimeString());
                        }
                        else
                        {
                            description.AppendFormat("week {0} every {1} {2} between {3} and {4}.",
                                                      BuildDaysOfWeekString(scheduleData),
                                                      scheduleData.freq_RepeatRate,
                                                      (scheduleData.freq_Unit == FrequencyUnit.FreqencyUnitHours) ? "Hour(s)" : "Minute(s)",
                                                      scheduleData.freq_Start.ToLongTimeString(),
                                                      scheduleData.freq_End.ToLongTimeString());
                        }
                    }
                    else
                    {
                        if (scheduleData.freq_Type == FrequencyType.FrequencyOnce)
                        {
                            description.AppendFormat("{0} weeks {1} at {2}.",
                                                     scheduleData.weekly_RepeatRate,
                                                     BuildDaysOfWeekString(scheduleData),
                                                     scheduleData.freq_OnceAtTime.ToLongTimeString());
                        }
                        else
                        {
                            description.AppendFormat("{0} weeks {1} every {2} {3} between {4} and {5}.",
                                                      scheduleData.weekly_RepeatRate,
                                                      BuildDaysOfWeekString(scheduleData),
                                                      scheduleData.freq_RepeatRate,
                                                      (scheduleData.freq_Unit == FrequencyUnit.FreqencyUnitHours) ? "Hour(s)" : "Minute(s)",
                                                      scheduleData.freq_Start.ToLongTimeString(),
                                                      scheduleData.freq_End.ToLongTimeString());
                        }
                    }
                    break;
                case OccurType.OccursMonthly:
                    if (scheduleData.monthly_type == MonthlyOccurType.MonthlyOccurDay)
                    {
                        if (scheduleData.monthly_repeatRate < 2)
                        {
                            if (scheduleData.freq_Type == FrequencyType.FrequencyOnce)
                            {
                                description.AppendFormat("month on day {0} at {1}",
                                                         scheduleData.monthly_dayOfMonth,
                                                         scheduleData.freq_OnceAtTime.ToLongTimeString());
                            }
                            else
                            {
                                description.AppendFormat("month on day {0} every {1} {2} between {3} and {4}.",
                                                          scheduleData.monthly_dayOfMonth,
                                                          scheduleData.freq_RepeatRate,
                                                          (scheduleData.freq_Unit == FrequencyUnit.FreqencyUnitHours) ? "Hour(s)" : "Minute(s)",
                                                          scheduleData.freq_Start.ToLongTimeString(),
                                                          scheduleData.freq_End.ToLongTimeString());
                            }
                        }
                        else
                        {
                            if (scheduleData.freq_Type == FrequencyType.FrequencyOnce)
                            {
                                description.AppendFormat("{0} months on day {1} at {2}",
                                                         scheduleData.monthly_repeatRate,
                                                         scheduleData.monthly_dayOfMonth,
                                                         scheduleData.freq_OnceAtTime.ToLongTimeString());
                            }
                            else
                            {
                                description.AppendFormat("{0} months on day {1} every {2} {3} between {4} and {5}.",
                                                          scheduleData.monthly_repeatRate,
                                                          scheduleData.monthly_dayOfMonth,
                                                          scheduleData.freq_RepeatRate,
                                                          (scheduleData.freq_Unit == FrequencyUnit.FreqencyUnitHours) ? "Hour(s)" : "Minute(s)",
                                                          scheduleData.freq_Start.ToLongTimeString(),
                                                          scheduleData.freq_End.ToLongTimeString());
                            }
                        }
                    }
                    else
                    {
                        if (scheduleData.freq_Type == FrequencyType.FrequencyOnce)
                        {
                            description.AppendFormat("{0} {1} of every {2} month(s) at {3}",
                                                   BuildOccuranceString(scheduleData),
                                                   BuildDayOfMonthString(scheduleData),
                                                   scheduleData.monthly_SpecificRepeatRate,
                                                   scheduleData.freq_OnceAtTime.ToLongTimeString());
                        }
                        else
                        {
                            description.AppendFormat("{0} {1} of every {2} month(s) every {3} {4} between {5} and {6}.",
                                                     BuildOccuranceString(scheduleData),
                                                     BuildDayOfMonthString(scheduleData),
                                                     scheduleData.monthly_SpecificRepeatRate,
                                                     scheduleData.freq_RepeatRate,
                                                     (scheduleData.freq_Unit == FrequencyUnit.FreqencyUnitHours) ? "Hour(s)" : "Minute(s)",
                                                     scheduleData.freq_Start.ToLongTimeString(),
                                                     scheduleData.freq_End.ToLongTimeString());
                        }
                    }
                    break;
            }

//            if(scheduleData.snapshotretentionPeriod > 0)
//            {
//                description.AppendFormat(" Snapshot data will be retained for {0} day(s).", scheduleData.snapshotretentionPeriod);
//            }

            scheduleData.Description = description.ToString();
        }

        #endregion

        #region Helpers

        private static string BuildDayOfMonthString(ScheduleData scheduleData)
        {
            string day = string.Empty;

            switch (scheduleData.monthly_SpecificDay)
            {
                case MonthlyDay.MonthlyDayEveryDay:
                    day = "Day";
                    break;
                case MonthlyDay.MonthlyDayFriday:
                    day = "Friday";
                    break;
                case MonthlyDay.MonthlyDayMonday:
                    day = "Monday";
                    break;
                case MonthlyDay.MonthlyDaySaturday:
                    day = "Saturday";
                    break;
                case MonthlyDay.MonthlyDaySunday:
                    day = "Sunday";
                    break;
                case MonthlyDay.MonthlyDayThursday:
                    day = "Thursday";
                    break;
                case MonthlyDay.MonthlyDayTuesday:
                    day = "Tuesday";
                    break;
                case MonthlyDay.MonthlyDayWednesday:
                    day = "Wednesday";
                    break;
                case MonthlyDay.MonthlyDayWeekday:
                    day = "Weekday";
                    break;
                case MonthlyDay.MonthlyDayWeekendDay:
                    day = "Weekend Day";
                    break;
            }

            return day;
        }

        private static string BuildOccuranceString(ScheduleData scheduleData)
        {
            string temp = string.Empty;
            switch (scheduleData.monthly_SpecificOccurance)
            {
                case MonthlyWhichOccurance.MonthlyOccuranceFirst:
                    temp = "first";
                    break;
                case MonthlyWhichOccurance.MonthlyOccuranceSecond:
                    temp = "second";
                    break;
                case MonthlyWhichOccurance.MonthlyOccuranceThird:
                    temp = "third";
                    break;
                case MonthlyWhichOccurance.MonthlyOccuranceFouth:
                    temp = "fourth";
                    break;
                case MonthlyWhichOccurance.MonthlyOccuranceLast:
                    temp = "last";
                    break;
            }
            return temp;
        }

        private static string BuildDaysOfWeekString(ScheduleData scheduleData)
        {
            StringBuilder description = new StringBuilder("");

            bool bFoundDay = false;
            if (scheduleData.weekly_isMonday)
            {
                if (bFoundDay)
                {
                    description.Append(", Monday");
                }
                else
                {
                    description.Append("on Monday");
                    bFoundDay = true;
                }
            }
            if (scheduleData.weekly_isTuesday)
            {
                if (bFoundDay)
                {
                    description.Append(", Tuesday");
                }
                else
                {
                    description.Append("on Tuesday");
                    bFoundDay = true;
                }
            }
            if (scheduleData.weekly_isWednesday)
            {
                if (bFoundDay)
                {
                    description.Append(", Wednesday");
                }
                else
                {
                    description.Append("on Wednesday");
                    bFoundDay = true;
                }
            }
            if (scheduleData.weekly_isThursday)
            {
                if (bFoundDay)
                {
                    description.Append(", Thursday");
                }
                else
                {
                    description.Append("on Thursday");
                    bFoundDay = true;
                }
            }
            if (scheduleData.weekly_isFriday)
            {
                if (bFoundDay)
                {
                    description.Append(", Friday");
                }
                else
                {
                    description.Append("on Friday");
                    bFoundDay = true;
                }
            }

            if (scheduleData.weekly_isSaturday)
            {
                if (bFoundDay)
                {
                    description.Append(", Saturday");
                }
                else
                {
                    description.Append("on Saturday");
                    bFoundDay = true;
                }
            }
            if (scheduleData.weekly_isSunday)
            {
                if (bFoundDay)
                {
                    description.Append(", Sunday");
                }
                else
                {
                    description.Append("on Sunday");
                    bFoundDay = true;
                }
            }


            return description.ToString();

        }

        private static DateTime GetDateTimeFromSQLInt(int date, int time)
        {
            int year = date / 10000;
            int month = (date - (year * 10000)) / 100;
            int day = (date - (year * 10000) - (month * 100));
            int hours = time / 10000;
            int minutes = (time - (hours * 10000)) / 100;
            int seconds = (time - (hours * 10000) - (minutes * 100));

            DateTime dt = DateTime.MinValue;

            try
            {
                dt = new DateTime(year, month, day, hours, minutes, seconds);
            }
            catch { }

            return dt;
        }

        private static DateTime GetDateTimeFromSQLInt(int time)
        {
            int hours = time / 10000;
            int minutes = (time - (hours * 10000)) / 100;
            int seconds = (time - (hours*10000) - (minutes*100));

            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                        hours, minutes, seconds);

            return dt;
        }

        private static void SetEndTimeInScheduleData(int time, ref ScheduleData scheduleData)
        {
              scheduleData.freq_End = GetDateTimeFromSQLInt(time);
        }

        private static int GetEndTime(ScheduleData scheduleData)
        {
            int time = 0;
            time = scheduleData.freq_End.Hour * 10000;
            time += scheduleData.freq_End.Minute * 100;
            time += scheduleData.freq_End.Second;

            return time;
        }

        private static void SetStartTimeInScheduleData(int time, ref ScheduleData scheduleData)
        {
            if (scheduleData.freq_Type == FrequencyType.FrequencyOnce)
            {
                scheduleData.freq_OnceAtTime = GetDateTimeFromSQLInt(time);
            }
            else
            {
                scheduleData.freq_Start = GetDateTimeFromSQLInt(time);
            }
        }

        private static int GetStartTime(ScheduleData scheduleData)
        {
            int time = 0;
            if (scheduleData.freq_Type == FrequencyType.FrequencyOnce)
            {
                time = scheduleData.freq_OnceAtTime.Hour * 10000;
                time += scheduleData.freq_OnceAtTime.Minute * 100;
                time += scheduleData.freq_OnceAtTime.Second;
            }
            else
            {
                time = scheduleData.freq_Start.Hour * 10000;
                time += scheduleData.freq_Start.Minute * 100;
                time += scheduleData.freq_Start.Second;
            }

            return time;
        }

        private static void SetFreqRecurrenceFactorInScheduleData(int FreqType, int interval, ref ScheduleData scheduleData)
        {
            switch (FreqType)
            {
                case 8:
                    scheduleData.weekly_RepeatRate = (uint)interval;
                    break;
                case 16:
                    scheduleData.monthly_repeatRate = (uint)interval;
                    break;
                case 32:
                    scheduleData.monthly_SpecificRepeatRate = (uint)interval;
                    break;
            }
            
        }

        private static int GetFreqRecurrenceFactor(int FreqType, ScheduleData scheduleData)
        {
            int repeatRate = 0;
            switch (FreqType)
            {
                case 8:
                    repeatRate = (int)scheduleData.weekly_RepeatRate;
                    break;
                case 16:
                    repeatRate = (int)scheduleData.monthly_repeatRate;
                    break;
                case 32:
                    repeatRate = (int)scheduleData.monthly_SpecificRepeatRate;
                    break;
            }

            return repeatRate;
        }

        private static void SetFreqRelativeIntervalInScheduleData(int interval, ref ScheduleData scheduleData)
        {
            scheduleData.monthly_SpecificOccurance = (MonthlyWhichOccurance)interval;
        }

        private static int GetFreqRelativeInterval(ScheduleData scheduleData)
        {
            int interval = (int)scheduleData.monthly_SpecificOccurance;

            return interval;
        }

        private static void SetFrequencySubDayIntervalInScheduleData(int interval, ref ScheduleData scheduleData)
        {
            scheduleData.freq_RepeatRate = (uint)interval;
        }

        private static int GetFrequencySubDayInterval(ScheduleData scheduleData)
        {
            int interval = (int)scheduleData.freq_RepeatRate;

            return interval;
        }

        private static void SetFrequencySubDayTypeInScheduleData(int type, ref ScheduleData scheduleData)
        {
            switch(type)
            {
                case 1:
                    scheduleData.freq_Type = FrequencyType.FrequencyOnce;
                    break;
                case 2:
                    scheduleData.freq_Type = FrequencyType.FrequencyEvery;
                    scheduleData.freq_Unit = FrequencyUnit.FreqencyUnitSeconds;
                    break;
                case 4:
                    scheduleData.freq_Type = FrequencyType.FrequencyEvery;
                    scheduleData.freq_Unit = FrequencyUnit.FreqencyUnitMinutes;
                    break;
                case 8:
                    scheduleData.freq_Type = FrequencyType.FrequencyEvery;
                    scheduleData.freq_Unit = FrequencyUnit.FreqencyUnitHours;
                    break;
            }
        }

        private static int GetFrequencySubDayType(ScheduleData scheduleData)
        {
            int type = 0;
            if (scheduleData.freq_Type == FrequencyType.FrequencyOnce)
            {
                type = 1;
            }
            else
            {
                if (scheduleData.freq_Unit == FrequencyUnit.FreqencyUnitSeconds)
                {
                    type = 2;
                }
                else if (scheduleData.freq_Unit == FrequencyUnit.FreqencyUnitMinutes)
                {
                    type = 4;
                }
                else if (scheduleData.freq_Unit == FrequencyUnit.FreqencyUnitHours)
                {
                    type = 8;
                }
            }

            return type;
        }

        private static void SetFrequencyIntervalInScheduleData(int type, 
                                                              int interval, 
                                                              ref ScheduleData scheduleData)
        {
            switch (type)
            {
                case 4:
                    scheduleData.daily_RepeatRate = (uint)interval;
                    break;
                case 8:
                    scheduleData.weekly_isSunday = (interval & 0x01) == 0x01;
                    scheduleData.weekly_isMonday = (interval & 0x02) == 0x02;
                    scheduleData.weekly_isTuesday = (interval & 0x04) == 0x04;
                    scheduleData.weekly_isWednesday = (interval & 0x08) == 0x08;
                    scheduleData.weekly_isThursday = (interval & 0x10) == 0x10;
                    scheduleData.weekly_isFriday = (interval & 0x20) == 0x20;
                    scheduleData.weekly_isSaturday = (interval & 0x40) == 0x40;
                    break;
                case 16:
                    scheduleData.monthly_dayOfMonth = (uint)interval;
                    break;
                case 32:
                    scheduleData.monthly_SpecificDay = (MonthlyDay)interval;
                    break;
            }
        }

        private static int GetFrequencyInterval(int FreqType, ScheduleData scheduleData)
        {
            int interval = 0;
            switch (FreqType)
            {
                case 4:
                    interval = (int)scheduleData.daily_RepeatRate;
                    break;
                case 8:
                    int sunday = scheduleData.weekly_isSunday ? 1 : 0;
                    int monday = scheduleData.weekly_isMonday ? 2 : 0;
                    int tuesday = scheduleData.weekly_isTuesday ? 4 : 0;
                    int wednesday = scheduleData.weekly_isWednesday ? 8 : 0;
                    int thursday = scheduleData.weekly_isThursday ? 16 : 0;
                    int friday = scheduleData.weekly_isFriday ? 32 : 0;
                    int saturday = scheduleData.weekly_isSaturday ? 64 : 0;
                    interval = sunday | monday | tuesday | wednesday | thursday | friday | saturday;
                    break;
                case 16:                    
                    interval = (int)scheduleData.monthly_dayOfMonth;
                    break;
                case 32:
                    interval = (int)scheduleData.monthly_SpecificDay;
                    break;
            }
            

            return interval;
        }

        private static void SetFrequencyTypeInScheduleData(int type, ref ScheduleData scheduleData)
        {
            switch (type)
            {
                case 4:
                    scheduleData.occurType = OccurType.OccursDaily;
                    break;
                case 8:
                    scheduleData.occurType = OccurType.OccursWeekly;
                    break;
                case 16:
                    scheduleData.occurType = OccurType.OccursMonthly;
                    scheduleData.monthly_type = MonthlyOccurType.MonthlyOccurDay;
                    break;
                case 32:
                    scheduleData.occurType = OccurType.OccursMonthly;
                    scheduleData.monthly_type = MonthlyOccurType.MonthlyOccurSpecificDay;
                    break;
            }
        }

        private static int GetFrequencyType(ScheduleData scheduleData)
        {
            int type = 0;
            switch (scheduleData.occurType)
            {
                case OccurType.OccursDaily:
                    type = 4;
                    break;
                case OccurType.OccursWeekly:
                    type = 8;
                    break;
                case OccurType.OccursMonthly:
                    if (scheduleData.monthly_type == MonthlyOccurType.MonthlyOccurDay)
                    {
                        type = 16;
                    }
                    else
                    {
                        type = 32;
                    }
                    break;
            }
            return type;
        }

        #endregion
    }
}
