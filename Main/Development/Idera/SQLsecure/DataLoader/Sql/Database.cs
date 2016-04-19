/******************************************************************
 * Name: Database.cs
 *
 * Description: SQL Server database object is encapsulated in this class.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;

using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using Idera.SQLsecure.Core.Accounts;
using Idera.SQLsecure.Core.Logger;

namespace Idera.SQLsecure.Collector.Sql
{
    /// <summary>
    /// 
    /// </summary>
    public class Database
    {
        private static LogX logX = new LogX("Idera.SQLsecure.Collector.Sql.Database");

        #region Queries
        private const string queryUpdateRegisteredServerTableErrorFmt =
                    @"update SQLsecure.dbo.registeredserver
                      set 
                        currentcollectiontm     = s.starttime,
                        currentcollectionstatus = s.status
                        from 
                            SQLsecure.dbo.serversnapshot s,
                            SQLsecure.dbo.registeredserver r
                        where
                            s.snapshotid = {0}
                            and r.connectionname = s.connectionname";

        private const string queryUpdateRegisteredServerTableFmt =
                    @"update SQLsecure.dbo.registeredserver
                      set 
                        authenticationmode = s.authenticationmode, 
                        os = s.os, 
                        version = s.version, 
                        edition = s.edition,
                        loginauditmode = s.loginauditmode,
                        enableproxyaccount = s.enableproxyaccount,
                        enablec2audittrace = s.enablec2audittrace,
                        crossdbownershipchaining = s.crossdbownershipchaining,
                        casesensitivemode = s.casesensitivemode,
                        lastcollectiontm        = s.starttime,
                        currentcollectiontm     = s.starttime,
                        currentcollectionstatus = s.status,
                        lastcollectionsnapshotid = s.snapshotid,
                        serverisdomaincontroller = s.serverisdomaincontroller,
                        replicationenabled = s.replicationenabled,
                        sapasswordempty = s.sapasswordempty
                        from 
                            SQLsecure.dbo.serversnapshot s,
                            SQLsecure.dbo.registeredserver r
                        where
                            s.snapshotid = {0}
                            and r.connectionname = s.connectionname";

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

        #endregion


        #region Load From Target
        public static bool GetTargetDatabases(
                Core.Accounts.Server server,
                ServerVersion sqlServerVersion,
                string targetConnectionString,
                string repositoryConnectionString,
                int snapshotid,
                string serverlogin,
                out List<Sql.Database> databaseList,
                ref Dictionary<Sql.SqlObjectType, Dictionary<MetricMeasureType, uint>> metricsData                
            )
        {
            Debug.Assert(server != null);
            Debug.Assert(!string.IsNullOrEmpty(targetConnectionString));
            Debug.Assert(sqlServerVersion != ServerVersion.Unsupported);
            uint numDatabasesProcessed = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Init returns.
            bool isOk = true;
            databaseList = new List<Database>();

            Program.ImpersonationContext wi = Program.SetTargetSQLServerImpersonationContext();

            // Check inputs.
            if (string.IsNullOrEmpty(targetConnectionString)
                || sqlServerVersion == ServerVersion.Unsupported)
            {
                string strMessage = "Invalid connection string or sql server version";
                logX.loggerX.Error( "ERROR - " + strMessage);
                Sql.Database.CreateApplicationActivityEventInRepository(repositoryConnectionString, 
                                                                        snapshotid, 
                                                                        Collector.Constants.ActivityType_Error, 
                                                                        Collector.Constants.ActivityEvent_Error, strMessage);
                AppLog.WriteAppEventError(SQLsecureEvent.DlInfoEndMsg, SQLsecureCat.DlEndCat, DateTime.Now.ToString() +
                                        " SQL Server = " + new SqlConnectionStringBuilder(targetConnectionString).DataSource +
                                        " Snapshot ID = " + snapshotid.ToString() + "; " + 
                                        strMessage);

                isOk = false;
            }

            if(isOk)
            {
                // Bind to the remote target, because we have to resolve db owner sid,
                // if needed (ignore errors).
                bool isBind = server.Bind();

                // Connect and load the databases.
                using (SqlConnection connection = new SqlConnection(targetConnectionString))
                {
                    try
                    {
                        // Open the connection.
                        connection.Open();

                        // Create the query based on server version.
                        string query = QueryDb2K;    
                        if (sqlServerVersion >= ServerVersion.SQL2012)
                            query = QueryDb2K12;
                        if (sqlServerVersion < ServerVersion.SQL2012 && sqlServerVersion > ServerVersion.SQL2000)
                            query = QueryDb2K5;
                        // Get a list of databases from the target instance.
                        using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null,
                            CommandType.Text, query, null))
                        {
                            while (rdr.Read())
                            {
                                // Retrieve the fields.
                                SqlString name = rdr.GetSqlString(FieldName);
                                SqlInt32 dbid = rdr.GetSqlInt32(FieldDbid);
                                SqlBinary ownersid = rdr.GetSqlBinary(FieldOwnersid);
                                SqlString ownername = rdr.GetSqlString(FieldOwnername);
                                SqlBoolean trustworthy = rdr.GetBoolean(FieldTrustworthy);
                                SqlBoolean isContained = rdr.GetBoolean(FieldIscontained);   

                                // Create the sid object.
                                Debug.Assert(!ownersid.IsNull);
                                Sid osid = new Sid(ownersid.Value);
                                Debug.Assert(osid.IsValid);

                                // If the owner name is null, then we have to resolve the SID to 
                                // get the owner name.
                                string owner = string.Empty;
                                if (ownername.IsNull)
                                {
                                    owner = osid.AccountName(server.Name);
                                }
                                else
                                {
                                    owner = ownername.Value;
                                }

                                // Create the database object.
                                Database db = new Database(name.Value, dbid.Value, osid, owner, server.Name, trustworthy.Value,isContained.Value);

                                db.GetDatabaseFiles(targetConnectionString);
                                // Add filter to the list.
                                databaseList.Add(db);

                                numDatabasesProcessed++;
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        string strMessage = "Getting the list of databases on target raised an exception. ";
                        logX.loggerX.Error("ERROR - " + strMessage, ex);
                        AppLog.WriteAppEventError(SQLsecureEvent.ExErrExceptionRaised, SQLsecureCat.DlDataLoadCat,
                                                    "Retrieve databases from target " + targetConnectionString, 
                                                    ex.Message);
                        Sql.Database.CreateApplicationActivityEventInRepository(repositoryConnectionString, 
                                                                                snapshotid, 
                                                                                Collector.Constants.ActivityType_Error, 
                                                                                Collector.Constants.ActivityEvent_Error, 
                                                                                strMessage + ex.Message);

                        databaseList.Clear();
                        databaseList = null;
                        isOk = false;
                    }
                }

                // Now unbind from the target, if we have bound to the target.
                if (isBind) { server.Unbind(); }
                Program.RestoreImpersonationContext(wi);
            }
            // See if Object is already in Metrics Dictionary
            // ----------------------------------------------
            sw.Stop();
            Dictionary<MetricMeasureType, uint> de;
            uint oldMetricCount = 0;
            uint oldMetricTime = 0;
            if (metricsData.TryGetValue(SqlObjectType.Database, out de))
            {
                de.TryGetValue(MetricMeasureType.Count, out oldMetricCount);
                de.TryGetValue(MetricMeasureType.Time, out oldMetricTime);
            }
            else
            {
                de = new Dictionary<MetricMeasureType, uint>();
            }
            de[MetricMeasureType.Count] = oldMetricCount + numDatabasesProcessed;
            de[MetricMeasureType.Time] = (uint)sw.ElapsedMilliseconds + oldMetricTime;
            metricsData[SqlObjectType.Database] = de;

            return isOk;
        }


        public void GetDatabaseFiles(string targetConnectionString)
        {
            // Connect and load the databases.
            using (SqlConnection connection = new SqlConnection(targetConnectionString))
            {
                try
                {
                    // Open the connection.
                    connection.Open();

                    // Create the query based on server version.
                    string query = string.Format(QueryGetDBFileNames, Sql.SqlHelper.CreateSafeDatabaseName(m_Name));

                    // Get a list of databases from the target instance.
                    using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null,
                                                                           CommandType.Text, query, null))
                    {
                        m_DatabaseFileNames = new List<string>();
                        string fileName = string.Empty;
                        while (rdr.Read())
                        {
                            fileName = (string)rdr[2];
                            DatabaseFileNames.Add(fileName.Trim());
                        }
                    }
                }
                catch(Exception ex)
                {
                    logX.loggerX.Error(string.Format("Error getting Database file name {0}: {1}", m_Name, ex.Message));
                }
            }
        }

        public static bool GetDabaseStatus(
                ServerVersion sqlServerVersion,
                string targetConnectionString,
                string repositoryConnectionString,
                int snapshotid,
                int dbid,
                out string status
            )
        {
            Debug.Assert(sqlServerVersion != ServerVersion.Unsupported);
            Debug.Assert(!string.IsNullOrEmpty(targetConnectionString));
            Debug.Assert(!string.IsNullOrEmpty(repositoryConnectionString));
            Debug.Assert(snapshotid != 0);

            // Init returns.
            bool isOk = true;
            status = "Unknown";

            // Check inputs.
            if (string.IsNullOrEmpty(targetConnectionString)
                || sqlServerVersion == ServerVersion.Unsupported)
            {
                string strMessage = "Invalid connection string or sql server version";
                logX.loggerX.Error("ERROR - " + strMessage);
                Sql.Database.CreateApplicationActivityEventInRepository(repositoryConnectionString,
                                                                        snapshotid,
                                                                        Collector.Constants.ActivityType_Error,
                                                                        Collector.Constants.ActivityEvent_Error, strMessage);
                AppLog.WriteAppEventError(SQLsecureEvent.DlInfoEndMsg, SQLsecureCat.DlEndCat, DateTime.Now.ToString() +
                                        " SQL Server = " + new SqlConnectionStringBuilder(targetConnectionString).DataSource +                                         
                                         " Snapshot ID = " + snapshotid.ToString() + "; " + 
                                         strMessage);

                isOk = false;
            }

            // If its failed check, then query target for db status.
            if (isOk)
            {
                using (SqlConnection connection = new SqlConnection(targetConnectionString))
                {
                    try
                    {
                        // Open the connection.
                        connection.Open();

                        // Create the query.
                        string query = QueryDbStatus1 + dbid.ToString() + QueryDbStatus2;

                        // Get the status.
                        using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null,
                            CommandType.Text, query, null))
                        {
                            if (rdr.Read())
                            {
                                // Retrieve the status fields.
                                SqlString s = rdr.GetSqlString(FieldDbStatus);
                                if (!s.IsNull) { status = s.Value; }
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        string strMessage = "Getting database status raised an exception. ";
                        logX.loggerX.Error("ERROR - " + strMessage, ex);
                        AppLog.WriteAppEventError(SQLsecureEvent.ExErrExceptionRaised, SQLsecureCat.DlDataLoadCat,
                                                    " SQL Server = " + new SqlConnectionStringBuilder(targetConnectionString).DataSource +
                                                    "Retrieve database status", ex.Message);
                        Sql.Database.CreateApplicationActivityEventInRepository(repositoryConnectionString,
                                                                                snapshotid,
                                                                                Collector.Constants.ActivityType_Error,
                                                                                Collector.Constants.ActivityEvent_Error,
                                                                                strMessage + ex.Message);
                    }
                }
            }

            return isOk;
        }

        #region SQL Queries

        private const string QueryGetDBFileNames = @"use {0} exec sp_helpfile";

        
        private const string QueryDb2K =
                            @"SELECT name = db.name, dbid = CAST(db.dbid AS int), ownersid = db.sid, ownername = l.loginname, trustworthy = cast(0 as bit), isContained=cast( 0 as bit)
                              FROM master.dbo.sysdatabases AS db LEFT OUTER JOIN master.dbo.syslogins AS l 
	                                    ON (db.sid = l.sid)";
        private const string QueryDb2K5 =
                            @"SELECT name = db.name, dbid = db.database_id, ownersid = db.owner_sid, ownername = l.name, trustworthy = db.is_trustworthy_on, isContained=cast( 0 as bit)
                              FROM sys.databases AS db LEFT OUTER JOIN sys.server_principals AS l
	                                    ON (db.owner_sid = l.sid)";

        private const string QueryDb2K12 =
                            @"SELECT name = db.name, dbid = db.database_id, ownersid = db.owner_sid, ownername = l.name, trustworthy = db.is_trustworthy_on, isContained=cast( db.containment as bit)
                              FROM sys.databases AS db LEFT OUTER JOIN sys.server_principals AS l
	                                    ON (db.owner_sid = l.sid)";
        private const int FieldName = 0;
        private const int FieldDbid = 1;
        private const int FieldOwnersid = 2;
        private const int FieldOwnername = 3;
        private const int FieldTrustworthy = 4;
        private const int FieldIscontained = 5;

        private const string QueryDbStatus1 = @"
                            --Declare variables
                            DECLARE 
	                            @dbname varchar(255),
	                            @dbid int,
	                            @mode smallint, 
	                            @status2 integer, 
	                            @status int  
                            	
                            SET @dbid = ";
        private const string QueryDbStatus2 = @"
                            --Get mode, status & status2 for the dabase.
                            SELECT 
	                            @dbname = name,
	                            @mode = mode,
	                            @status = isnull(convert(integer,status),-999),
	                            @status2 = status2
                            FROM 
	                            master..sysdatabases d (nolock)
                            WHERE 
	                            dbid = @dbid

                            -- Determine if database is suspect, and if so, goto Single User DB (below)
                            -- Note that databasepropertyex is SQL 2000+ and databaseproperty is provided for backwards compatibility
                            IF databasepropertyex(@dbname,'status') = 'Suspect' 
	                            GOTO single_user_db 

                            -- If database status is Single User and the current user does not have access to it, go to Single User DB (below)

                            IF @status & 4096 = 4096 
                            BEGIN
                                IF(has_dbaccess(@dbname) = 0) 
	                              GOTO single_user_db 
                            END

                            -- If the user has access to the database and:
                            IF (has_dbaccess(@dbname) = 1) 
	                            -- The database is not locked
	                            AND @mode = 0 
	                            -- The database is not in a load state
	                            AND databaseproperty(@dbname, 'isinload') = 0 
	                            -- The database is not suspect
	                            AND databaseproperty(@dbname, 'issuspect') = 0 
	                            -- The database is not in recovery
	                            AND isnull(databaseproperty(@dbname, 'isinrecovery'),0) = 0 
	                            -- The database is not in state - not recovered
	                            AND isnull(databaseproperty(@dbname, 'isnotrecovered'),0) = 0 
	                            -- The database is not offline
	                            AND databaseproperty(@dbname, 'isoffline') = 0 
	                            -- The database is not shut down
	                            AND isnull(databaseproperty(@dbname, 'isshutdown'),0) = 0 
	                            -- The database is not in a load state (according to status bits)
	                            AND @status & 32 <> 32 
	                            -- The database is not in pre-recovery (according to status bits)
	                            AND @status & 64 <> 64 
	                            -- The database is not in recovery (according to status bits)
	                            AND @status & 128 <> 128 
	                            -- The database is not in state - not recovered (according to status bits)
	                            AND @status & 256 <> 256 
	                            -- The database is not offline (according to status bits)
	                            AND @status & 512 <> 512 
                            	
	                            BEGIN
		                            --This section of code will run if the database is accessible
		                            SELECT
			                            'Available database', 
			                            @dbname, 
			                            @status, 
			                            @mode  

	                            END
                            ELSE
	                            BEGIN single_user_db:  
		                            -- If not suspect and not inaccessible
		                            IF @mode = 0 and databasepropertyex(@dbname,'status') <> 'suspect' 
			                            -- Return - loading/exlock db along with DB name, status, and mode
			                            SELECT
				                            'Database is loading or exclusively locked', 
				                            @dbname, 
				                            @status, 
				                            @mode  
		                            ELSE
			                            BEGIN
				                            -- If suspect
				                            IF databasepropertyex(@dbname,'status') = 'suspect'  
					                            -- Return - not accessible along with DB name, 256, and mode
					                            SELECT
						                            'Suspect', 
						                            @dbname, 
						                            @status, 
						                            @mode  
				                            -- If inaccessible for another reason
				                            ELSE
					                            -- Return - not accessible along with DB name, status, and mode
					                            SELECT
						                            'Not accessible', 
						                            @dbname, 
						                            @status, 
						                            @mode  
			                            END 
	                            END";
        private const int FieldDbStatus = 0;

        #endregion

        #endregion

        #region Save To Repository

        public static bool SaveToRepositorySqlDatabaseTable(
                string connectionString,
                int snapshotid,
                Database database,
                char isAudited
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(connectionString));
            Debug.Assert(database != null);

            // Validate inputs.
            bool isOk = true;
            if (string.IsNullOrEmpty(connectionString) || database == null)
            {
                logX.loggerX.Error("ERROR - invalid inputs to save database to repository sqldatabase table");
                isOk = false;
            }
            if (isOk)
            {
                Program.ImpersonationContext wi = Program.SetLocalImpersonationContext();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        // Open the connection.
                        connection.Open();

                        // Insert database table.
                        SqlParameter paramSnapshotid = new SqlParameter(ParamSnapshotid, snapshotid);
                        SqlParameter paramDbid = new SqlParameter(ParamDbid, database.DbId);
                        SqlParameter paramDatabasename = new SqlParameter(ParamDatabasename, database.Name);
                        SqlParameter paramOwner = new SqlParameter(ParamOwner, database.OwnerName);
                        SqlParameter paramGuestenabled = new SqlParameter(ParamGuestenabled, database.IsGuestEnabledChar);
                        SqlParameter paramAvailable = new SqlParameter(ParamIsAvailable, database.IsAvailableChar);
                        SqlParameter paramStatus = new SqlParameter(ParamStatus, database.Status);
                        SqlParameter paramHashkey = new SqlParameter(ParamHashkey, "");
                        SqlParameter paramIsAudited = new SqlParameter(ParamIsAudited, isAudited.ToString());
                        SqlParameter paramIsTrustworthy = new SqlParameter(ParamTrustworthy, database.IsTrustworthyChar);
                        SqlParameter paramIsContained = new SqlParameter(ParamIsContained, database.IsContained);



                        Sql.SqlHelper.ExecuteNonQuery(connection, CommandType.Text, NonQueryDatabaseInsert, 
                                            new SqlParameter[] {  paramDbid, paramSnapshotid, paramDatabasename, paramOwner, 
                                                                    paramGuestenabled, paramAvailable, paramStatus, paramHashkey, 
                                                                    paramIsAudited, paramIsTrustworthy,paramIsContained });
                    }
                    catch (SqlException ex)
                    {
                        logX.loggerX.Error("ERROR - failed to update sqldatabase table", ex);
                        isOk = false;
                    }
                    finally
                    {
                        Program.RestoreImpersonationContext(wi);
                    }

                }
            }

            return isOk;
        }

        public bool IsContained
        {
            get { return m_isContained; }
            set { m_isContained = value; }
        }


        public static bool SaveToRepositoryDatabaseObjectTable(
                string connectionString,
                int snapshotid,
                Database database
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(connectionString));
            Debug.Assert(database != null);
            // Validate inputs.
            bool isOk = true;
            if (string.IsNullOrEmpty(connectionString) || database == null)
            {
                logX.loggerX.Error("ERROR - invalid inputs to save database to repository databaseobject table");
                isOk = false;
            }

            if (isOk)
            {
                Program.ImpersonationContext wi = Program.SetLocalImpersonationContext();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        // Open the connection.
                        connection.Open();

                        // Insert into object table.
                        SqlParameter paramSnapshotid = new SqlParameter(ParamSnapshotid, snapshotid);
                        SqlParameter paramDbid = new SqlParameter(ParamDbid, database.DbId);
                        SqlParameter paramClassid = new SqlParameter(ParamClassid, Convert.ToInt32(0));
                        SqlParameter paramParentobjectid = new SqlParameter(ParamParentobjectid, Convert.ToInt32(0));
                        SqlParameter paramObjectid = new SqlParameter(ParamObjectid, Convert.ToInt32(0));
                        SqlParameter paramSchemaid = new SqlParameter(ParamSchemaid, null);
                        SqlParameter paramType = new SqlParameter(ParamType, "DB");
                        SqlParameter paramOwnerId = new SqlParameter(ParamOwnerid, 1); // Always DBO
                        SqlParameter paramName = new SqlParameter(ParamName, database.Name);
                        SqlParameter paramHashkey = new SqlParameter(ParamHashkey, "");
                        Sql.SqlHelper.ExecuteNonQuery(connection, CommandType.Text, NonQueryObjectInsert,
                                            new SqlParameter[] {  paramSnapshotid, paramDbid, paramClassid, paramParentobjectid, 
                                                                        paramObjectid, paramSchemaid, paramType, paramOwnerId, 
                                                                            paramName, paramHashkey });
                    }
                    catch (SqlException ex)
                    {
                        logX.loggerX.Error("ERROR - failed to update databaseobject table", ex);
                        isOk = false;
                    }
                    finally
                    {
                        Program.RestoreImpersonationContext(wi);
                    }
                }
            }

            return isOk;
        }


        public static bool UpdateRepositoryRegisteredServerTable(
                string connectionString,
                int snapshotid,
                char status
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(connectionString));

            // Validate inputs.
            bool isOk = true;
            if (string.IsNullOrEmpty(connectionString))
            {
                logX.loggerX.Error("ERROR - invalid inputs Update Repository Registered Server Table");
                isOk = false;
            }
            if (isOk)
            {
                Program.ImpersonationContext wi = Program.SetLocalImpersonationContext();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        // Open the connection.
                        connection.Open();

                        // Create the query to update registered server table from
                        // serversnapshot table for the given snapshotID
                        // --------------------------------------------------------
                        string query = null;
                        if (status == Collector.Constants.StatusError)
                        {
                            query = string.Format(queryUpdateRegisteredServerTableErrorFmt, snapshotid);
                        }
                        else
                        {
                            query = string.Format(queryUpdateRegisteredServerTableFmt, snapshotid);
                        }

                        // Update snapshot history table
                        // -----------------------------
                        Sql.SqlHelper.ExecuteNonQuery(connection, CommandType.Text, query, null);

                    }
                    catch (SqlException ex)
                    {
                        logX.loggerX.Error("ERROR - failed to update snapshothistory database", ex);
                        isOk = false;
                    }
                    finally
                    {
                        Program.RestoreImpersonationContext(wi);
                    }

                }
            }
            return isOk;
        }

        public static bool UpdateRepositorySnapshotProgress(
                string connectionString,
                int snapshotid,
                string strMessage
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(connectionString));

            // Validate inputs.
            bool isOk = true;
            if (string.IsNullOrEmpty(connectionString))
            {
                logX.loggerX.Error("ERROR - invalid inputs to update database guest enabled field");
                isOk = false;
            }
            if (isOk)
            {
                Program.ImpersonationContext wi = Program.SetLocalImpersonationContext();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        // Open the connection.
                        connection.Open();

                        // Create the query for the serversnapshot table
                        // ---------------------------------------------
                        string query = @"UPDATE SQLsecure.dbo.serversnapshot
                                         SET snapshotcomment = '" + strMessage + @"'"
                                     + @" WHERE snapshotid = " + snapshotid.ToString();

                        // Update snapshot history table
                        // -----------------------------
                        Sql.SqlHelper.ExecuteNonQuery(connection, CommandType.Text, query, null);

                    }
                    catch (SqlException ex)
                    {
                        logX.loggerX.Error("ERROR - failed to update snapshothistory database", ex);
                        isOk = false;
                    }
                    finally
                    {
                        Program.RestoreImpersonationContext(wi);
                    }
                }
            }
            return isOk;
        }

        public static bool UpdateRepositorySnapshotHistory(
                string connectionString,
                int snapshotid,
                bool automatedRun,
                char status,
                int numErrorsAndWarning,
                string strMessage    
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(connectionString));

            // Validate inputs.
            bool isOk = true;
            if( string.IsNullOrEmpty(connectionString) )
            {
                logX.loggerX.Error("ERROR - invalid inputs to update database guest enabled field");
                isOk = false;
            }
            if(isOk)
            {
                Program.ImpersonationContext wi = Program.SetLocalImpersonationContext();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        // Open the connection.
                        connection.Open();

                        // Create the query string for snapshothistory table
                        // -------------------------------------------------
                        const string UpdateSnapshotHistory = @"UPDATE SQLsecure.dbo.snapshothistory
                                                                      SET status = @status,
                                                                          numberoferror = @numErrorsAndWarning,
                                                                          endtime = @endTime
                                                                      WHERE snapshotid = @snapshotID";

                        SqlParameter paramStatus = new SqlParameter("status", status);
                        SqlParameter paramNumErrors = new SqlParameter("numErrorsAndWarning", numErrorsAndWarning);
                        SqlParameter paramEndTime = new SqlParameter("endTime", DateTime.Now.ToUniversalTime());
                        SqlParameter paramSnapshotID = new SqlParameter("snapshotID", snapshotid);

                        // Update snapshot history table.
                        // ------------------------------
                        Sql.SqlHelper.ExecuteNonQuery(connection, CommandType.Text, UpdateSnapshotHistory,
                                                        new SqlParameter[] { paramStatus, paramNumErrors, 
                                                                             paramEndTime, paramSnapshotID });

                        // Create the query for the serversnapshot table
                        // ---------------------------------------------
                        string automated = (automatedRun == true) ? Collector.Constants.Yes.ToString() : Collector.Constants.No.ToString();

                        string query = @"UPDATE SQLsecure.dbo.serversnapshot
                                         SET snapshotcomment = @snapshotcomment,
                                             status = @status,
                                             automated = @automated,   
                                             endtime = @endTime,
                                             numobject = @numDatabaseObjectsCollected,
                                             numlogin = @numLoginsCollected,
                                             numpermission = @numPermissionsCollected,
                                             numwindowsgroupmember = (select count(snapshotid) from SQLsecure.dbo.windowsaccount where snapshotid = @snapshotID)
                                         WHERE snapshotid = @snapshotID";

                        SqlParameter paramSnapshotComment = new SqlParameter("snapshotcomment", strMessage);
                        SqlParameter paramAutomated = new SqlParameter("automated", automated);
                        SqlParameter paramNumObjects = new SqlParameter("numDatabaseObjectsCollected", (int)Target.numDatabaseObjectsCollected);
                        SqlParameter paramNumLogins = new SqlParameter("numLoginsCollected", (int)Target.numLoginsCollected);
                        SqlParameter paramNumPermisions = new SqlParameter("numPermissionsCollected", (int)Target.numPermissionsCollected);
//                        SqlParameter paramSnapshotID1 = new SqlParameter("snapshodID1", snapshotid);

                        // Update snapshot history table
                        // -----------------------------
                        Sql.SqlHelper.ExecuteNonQuery(connection, CommandType.Text, query,
                                       new SqlParameter[] {paramSnapshotComment, paramStatus, paramAutomated, paramEndTime,
                                                           paramNumObjects, paramNumLogins, paramNumPermisions, paramSnapshotID});
                    }
                    catch (SqlException ex)
                    {
                        logX.loggerX.Error("ERROR - failed to update snapshothistory database", ex);
                        isOk = false;
                    }
                    finally
                    {
                        Program.RestoreImpersonationContext(wi);
                    }
                }
            }

            return isOk;
        }

        public static bool UpdateRepositorySqlDatabaseGuestEnabled(
                string connectionString,
                int snapshotid,
                Database database
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(connectionString));
            Debug.Assert(database != null);

            // Validate inputs.
            bool isOk = true;
            if (string.IsNullOrEmpty(connectionString) || database == null)
            {
                logX.loggerX.Error("ERROR - invalid inputs to update database guest enabled field");
                isOk = false;
            }
            if (isOk)
            {
                Program.ImpersonationContext wi = Program.SetLocalImpersonationContext();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        // Open the connection.
                        connection.Open();

                        // Calculate the new hash key for the database object columns.
                        string hash = "";

                        // Create the query string.
                        string query = @"UPDATE SQLsecure.dbo.sqldatabase
                                         SET guestenabled = '" + database.IsGuestEnabledChar + @"', hashkey = '" + hash + @"' "
                                     + @"WHERE snapshotid = " + snapshotid.ToString() + @" AND dbid = " + database.DbId;

                        // Update sql database table.
                        Sql.SqlHelper.ExecuteNonQuery(connection, CommandType.Text, query, null);
                    }
                    catch (SqlException ex)
                    {
                        logX.loggerX.Error("ERROR - failed to update database guest enabled field", ex);
                        isOk = false;
                    }
                    finally
                    {
                        Program.RestoreImpersonationContext(wi);
                    }
                }
            }

            return isOk;
        }

        public static bool UpdateRepositorySqlDatabaseStatus(
                string connectionString,
                int snapshotid,
                Database database
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(connectionString));
            Debug.Assert(database != null);

            // Validate inputs.
            bool isOk = true;
            if (string.IsNullOrEmpty(connectionString) || database == null)
            {
                logX.loggerX.Error("ERROR - invalid inputs to update database status field");
                isOk = false;
            }
            if (isOk)
            {
                Program.ImpersonationContext wi = Program.SetLocalImpersonationContext();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        // Open the connection.
                        connection.Open();

                        // Calculate the new hash key for the database object columns.
                        string hash = "";

                        // Create the query string.
                        string query = @"UPDATE SQLsecure.dbo.sqldatabase
                                         SET available = '" + database.IsAvailableChar + @"', status = '" + database.Status + @"', hashkey = '" + hash + @"' "
                                     + @"WHERE snapshotid = " + snapshotid.ToString() + @" AND dbid = " + database.DbId;

                        // Update sql database table.
                        Sql.SqlHelper.ExecuteNonQuery(connection, CommandType.Text, query, null);
                    }
                    catch (SqlException ex)
                    {
                        logX.loggerX.Error("ERROR - failed to update database available and status fields", ex);
                        isOk = false;
                    }
                    finally
                    {
                        Program.RestoreImpersonationContext(wi);
                    }
                }
            }

            return isOk;
        }

        public static bool RemoveDatabaseData(
                string connectionString,
                int snapshotid,
                int dbid            
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(connectionString));

            // Validate inputs.
            bool isOk = true;
            if (string.IsNullOrEmpty(connectionString) || snapshotid == 0)
            {
                logX.loggerX.Error("ERROR - invalid inputs to remove database data");
                isOk = false;
            }
            if (isOk)
            {
                Program.ImpersonationContext wi = Program.SetLocalImpersonationContext();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        // Create the query string.
                        string query = "EXEC SQLsecure.dbo.isp_sqlsecure_removedatabasedata "
                                       + "@snapshotid = " + snapshotid.ToString() + ", "
                                       + "@dbid = " + dbid.ToString();

                        // Delete the db data.
                        connection.Open();
                        SqlCommand cmd = new SqlCommand(query, connection);
                        cmd.CommandTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        logX.loggerX.Error("ERROR - failed to delete unavailable database data", ex);
                        isOk = false;
                    }
                    finally
                    {
                        Program.RestoreImpersonationContext(wi);
                    }
                }
            }

            return isOk;
        }

        public static bool CreateApplicationActivityEventInRepository(
                       string connectionString, 
                       int snapshotID, 
                       string activityType, 
                       string eventcode, 
                       string description)
     {
         return CreateApplicationActivityEventInRepository(connectionString, Target.TargetInstance, snapshotID,
                                                            activityType, eventcode, description);
     }


       public static bool CreateApplicationActivityEventInRepository(
                       string connectionString, 
                       string targetServer,
                       int snapshotID, 
                       string activityType, 
                       string eventcode, 
                       string description
       )
        {
            bool isOK = true;
            Program.ImpersonationContext wi = Program.SetLocalImpersonationContext();
            Target.lastErrorMsg = description;
            string serverlogin = Environment.UserDomainName + @"\" + Environment.UserName;
            string category = Collector.Constants.ActivityCategory_CollectData;
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
                    SqlParameter paramCategory = new SqlParameter(ParamCategory, category);
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
                finally
                {
                    Program.RestoreImpersonationContext(wi);
                }
            }
            return isOK;
        }

        public static bool SaveWellKnownGroups(
                       string repositoryConnectionString,
                       int snapshotid,
                       List<String> wellKnownGroupsList
                   )
        {
            Debug.Assert(!string.IsNullOrEmpty(repositoryConnectionString));
            if (wellKnownGroupsList == null || wellKnownGroupsList.Count < 1)
            {
                return true;
            }

            // Validate inputs.
            if (string.IsNullOrEmpty(repositoryConnectionString))
            {
                logX.loggerX.Error("ERROR - invalid connection string specified for saving filter rules to snapshot");
                return false;
            }

            // Empty filter list, return.
            if (wellKnownGroupsList == null || wellKnownGroupsList.Count == 0)
            {
                logX.loggerX.Info("No well Known Groups to save");
                return true;
            }

            // Save each filter header and rule to repository.
            bool isOk = true;
            Program.ImpersonationContext wi = Program.SetLocalImpersonationContext();
            using (SqlConnection connection = new SqlConnection(repositoryConnectionString))
            {
                try
                {
                    // Open the connection.
                    connection.Open();

                    // Use bulk copy to write to repository.
                    using (SqlBulkCopy bcp = new SqlBulkCopy(connection))
                    {
                        // Write all the well known groups to the repository.
                        bcp.DestinationTableName = AncillaryWindowsGroupDataTable.RepositoryTable;
                        bcp.BulkCopyTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                        using (DataTable dataTable = AncillaryWindowsGroupDataTable.Create())
                        {
                            foreach (String name in wellKnownGroupsList)
                            {
                                DataRow dr = dataTable.NewRow();
                                dr[AncillaryWindowsGroupDataTable.ParamSnapshotid] = snapshotid;
                                dr[AncillaryWindowsGroupDataTable.ParamWindowsGroupName] = name;
                                dataTable.Rows.Add(dr);
                            }

                            // Write to repository.
                            try
                            {
                                bcp.WriteToServer(dataTable);
                            }
                            catch(SqlException ex)
                            {
                                string strMessage = "Update wellknown groups table";
                                logX.loggerX.Error("ERROR - " + strMessage, ex);
                                throw ex;                                
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    string strMessage = "Update wellknown groups table";
                    logX.loggerX.Error("ERROR - " + strMessage, ex);
                    Sql.Database.CreateApplicationActivityEventInRepository(repositoryConnectionString,
                                                                            snapshotid,
                                                                            Collector.Constants.ActivityType_Error,
                                                                            Collector.Constants.ActivityEvent_Error,
                                                                            strMessage + ex.Message);
                    AppLog.WriteAppEventError(SQLsecureEvent.ExErrExceptionRaised, SQLsecureCat.DlDataLoadCat,
                                              strMessage, ex.Message);

                    
                    isOk = false;
                }
                finally
                {
                    Program.RestoreImpersonationContext(wi);
                }
            }

            return isOk;
        }


        #region SQL Queries
        private const string NonQueryDatabaseInsert =
                    @"INSERT INTO SQLsecure.dbo.sqldatabase (dbid, snapshotid, databasename, owner, guestenabled, trustworthy, available, status, hashkey, isaudited,IsContained)
                      VALUES (@dbid, @snapshotid, @databasename, @owner, @guestenabled, @trustworthy, @available, @status, @hashkey, @isaudited,@iscontained)";
        private const string NonQueryObjectInsert =
                    @"INSERT INTO SQLsecure.dbo.databaseobject (snapshotid, dbid, classid, parentobjectid, objectid, schemaid, type, owner, name, hashkey)
                      VALUES (@snapshotid, @dbid, @classid, @parentobjectid, @objectid, @schemaid, @type, @ownerid, @name, @hashkey)"; 

        private const string ParamDbid = "dbid";
        private const string ParamSnapshotid = "snapshotid";
        private const string ParamDatabasename = "databasename";
        private const string ParamOwner = "owner";
        private const string ParamGuestenabled = "guestenabled";
        private const string ParamTrustworthy = "trustworthy";
        private const string ParamIsAvailable = "available";
        private const string ParamStatus = "status";
        private const string ParamHashkey = "hashkey";
        private const string ParamIsAudited = "isaudited";

        private const string ParamClassid = "classid";
        private const string ParamParentobjectid = "parentobjectid";
        private const string ParamObjectid = "objectid";
        private const string ParamSchemaid = "schemaid";
        private const string ParamType = "type";
        private const string ParamName = "name";
        private const string ParamOwnerid = "ownerid";
        private const string ParamIsContained = "iscontained";

        #endregion

        #endregion

        #region Fields
        private string m_Name;
        private int m_DbId;
        private List<string> m_DatabaseFileNames;
        private string m_serverName;
        private Sid m_OwnerSid;
        private string m_OwnerName;
        private bool m_IsGuestEnabled;
        private bool m_IsAvailable;
        private bool m_IsTrustworthy;
        private string m_Status;
        private bool m_isContained;

        #endregion

        #region Helpers
        #endregion

        #region Ctors
        public Database(string name, int dbId, Sid ownerSid, string ownerName, string serverName, bool trustworthy, bool isContained)
        {
            Debug.Assert(!string.IsNullOrEmpty(name));
            Debug.Assert(ownerSid != null);

            m_Name = name;
            m_DbId = dbId;
            m_OwnerSid = ownerSid;
            m_OwnerName = ownerName;
            m_IsGuestEnabled = false;
            m_IsTrustworthy = trustworthy;
            m_IsAvailable = true;
            m_serverName = serverName;
            m_Status = "Available";
            m_isContained = isContained;


        }
        #endregion
        
        #region Properties
        public string Name
        {
            get { return m_Name; }
        }
        public int DbId
        {
            get { return m_DbId; }
        }
        public string ServerName
        {
            get { return m_serverName; }
        }
        public List<string> DatabaseFileNames
        {
            get { return m_DatabaseFileNames; }   
        }

        public Sid OwnerSid
        {
            get { return m_OwnerSid; }
        }
        public string OwnerName
        {
            get { return m_OwnerName; }
        }
        public bool IsSystemDb
        {
            get
            {
                return (string.Compare(m_Name, "master", true) == 0
                        || string.Compare(m_Name, "model", true) == 0
                        || string.Compare(m_Name, "msdb", true) == 0
                        || string.Compare(m_Name, "tempdb", true) == 0);
            }
        }
        public bool IsGuestEnabled
        {
            set { m_IsGuestEnabled = value; }
            get { return m_IsGuestEnabled; }
        }
        public char IsGuestEnabledChar
        {
            get { return m_IsGuestEnabled ? Collector.Constants.Yes : Collector.Constants.No; }
        }
        public bool IsTrustworthy
        {
            set { m_IsTrustworthy = value; }
            get { return m_IsTrustworthy; }
        }
        public char IsTrustworthyChar
        {
            get { return m_IsTrustworthy ? Collector.Constants.Yes : Collector.Constants.No; }
        }

        public bool IsAvailable
        {
            set { m_IsAvailable = value; }
            get { return m_IsAvailable; }
        }
        public char IsAvailableChar
        {
            get { return m_IsAvailable ? Collector.Constants.Yes : Collector.Constants.No; }
        }

        public string Status
        {
            set { m_Status = value; }
            get { return m_Status; }
        }
        #endregion
    }
          
}
