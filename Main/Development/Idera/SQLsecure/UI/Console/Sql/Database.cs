/******************************************************************
 * Name: Database.cs
 *
 * Description: Encapsulates a dabase object in a snapshot.
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
using System.Data.SqlTypes;

using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Controls;

namespace Idera.SQLsecure.UI.Console.Sql
{
    public class Database : IComparable<Database>
    {
        #region Queries

        private const string QuerySnapshotDatabases
                = @"SELECT 
                        snapshotid, 
                        dbid, 
                        databasename,
                        owner,
                        guestenabled,
                        trustworthy,
                        available,
                        status
                    FROM SQLsecure.dbo.vwdatabases
                    WHERE snapshotid = @snapshotid";

        private const string QueryLastSnapshotDatabases
                = @"SELECT 
                        snapshotid, 
                        dbid, 
                        databasename,
                        owner,
                        guestenabled,
                        trustworthy,
                        available,
                        status
                    FROM SQLsecure.dbo.vwdatabases
                    WHERE snapshotid = (SELECT MAX(snapshotid) FROM SQLsecure.dbo.vwserversnapshot 
                            WHERE connectionname = @servername and status ='W')";

        private static string QuerySnapshotDatabase
                = QuerySnapshotDatabases + @" AND dbid = @dbid";
        private const string ParamSnapshotid = "snapshotid";
        private const string ParamServername = "servername";
        private const string ParamDbId = "dbid";
        private enum DbColumn
        {
            SnapshotId = 0,
            DbId,
            DatabaseName,
            Owner,
            GuestEnabled,
            Trustworthy,
            Available,
            Status
        };

        #endregion

        #region Fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Sql.Database");
        private int m_SnapshotId = 0;
        private int m_DbId = -1;
        private string m_Name = string.Empty;
        private string m_Owner = string.Empty;
        private bool m_IsGuestEnabled  = false;
        private string m_IsTrustworthy = string.Empty;
        private bool m_IsAvailable = true;
        private string m_Status = string.Empty;

        #endregion

        #region Ctors

        private Database(
                SqlInt32 snapshotid,
                SqlInt32 dbid,
                SqlString name,
                SqlString owner,
                SqlString guestenabled,
                SqlString trustworthy,
                SqlString available,
                SqlString status
            )
        {
            m_SnapshotId = snapshotid.Value;
            m_DbId = dbid.Value;
            m_Name = name.IsNull ? string.Empty : name.Value;
            m_Owner = owner.IsNull ? string.Empty : owner.Value;
            m_IsGuestEnabled = guestenabled.IsNull ? false : (string.Compare(guestenabled.Value,"Y",true) == 0);
            m_IsTrustworthy = Sql.RegisteredServer.YesNoStr(trustworthy.Value);
            m_IsAvailable = available.IsNull ? false : (string.Compare(available.Value, "Y", true) == 0);
            m_Status = status.IsNull ? string.Empty : status.Value;
        }

        #endregion

        #region Properties

        public int SnapshotId
        {
            get { return m_SnapshotId; }
        }
        public int DbId
        {
            get { return m_DbId; }
        }
        public string Name
        {
            get { return m_Name; }
        }
        public string Owner
        {
            get { return m_Owner; }
        }
        public bool IsGuestEnabled
        {
            get { return m_IsGuestEnabled; }
        }
        public string IsTrustworthy
        {
            get { return m_IsTrustworthy; }
        }
        public bool IsAvailable
        {
            get { return m_IsAvailable; }
        }
        public string Status
        {
            get { return m_Status; }
        }
        public bool IsMasterDb
        {
            get { return string.Compare(m_Name, "master", true) == 0; }
        }

        #endregion

        #region Methods

        public static List<Database> GetSnapshotDatabases(int snapshotid)
        {
            List<Database> dbList = new List<Database>();
            try
            {
                // Retrieve list of snapshot dbs.
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Connect to repository.
                    connection.Open();

                    // Create parameter.
                    SqlParameter paramSnapshotid = new SqlParameter(ParamSnapshotid, snapshotid);

                    // Query for 
                    using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text, 
                            QuerySnapshotDatabases, new SqlParameter[] { paramSnapshotid }))
                    {
                        while (rdr.Read())
                        {
                            Database db = new Database(rdr.GetSqlInt32((int)DbColumn.SnapshotId), rdr.GetSqlInt32((int)DbColumn.DbId),
                                                        rdr.GetSqlString((int)DbColumn.DatabaseName) , rdr.GetSqlString((int)DbColumn.Owner),
                                                            rdr.GetSqlString((int)DbColumn.GuestEnabled),
                                                                    rdr.GetSqlString((int)DbColumn.Trustworthy),
                                                                rdr.GetSqlString((int)DbColumn.Available),
                                                                    rdr.GetSqlString((int)DbColumn.Status));
                            dbList.Add(db);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error("Error Retrieving Database list for Snapshot", ex);
                dbList.Clear();
            }

            // Sort the db list by name.
            dbList.Sort();

            return dbList;
        }

        public static List<Database> GetServerDatabases(string serverName)
        {
            List<Database> dbList = new List<Database>();
            try
            {
                // Retrieve list of server dbs in last snapshot.
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Connect to repository.
                    connection.Open();

                    // Create parameter.
                    SqlParameter paramServername = new SqlParameter(ParamServername, serverName);

                    // Query for 
                    using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                            QueryLastSnapshotDatabases, new SqlParameter[] { paramServername }))
                    {
                        while (rdr.Read())
                        {
                            Database db = new Database(rdr.GetSqlInt32((int)DbColumn.SnapshotId), rdr.GetSqlInt32((int)DbColumn.DbId),
                                                        rdr.GetSqlString((int)DbColumn.DatabaseName), rdr.GetSqlString((int)DbColumn.Owner),
                                                            rdr.GetSqlString((int)DbColumn.GuestEnabled),
                                                                    rdr.GetSqlString((int)DbColumn.Trustworthy),
                                                                rdr.GetSqlString((int)DbColumn.Available),
                                                                    rdr.GetSqlString((int)DbColumn.Status));
                            dbList.Add(db);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error("Error Retrieving Database list for Snapshot", ex);
                dbList.Clear();
            }

            // Sort the db list by name.
            dbList.Sort();

            return dbList;
        }

        public static Database GetSnapshotDatabase(
                int snapshotid,
                int dbid
            )
        {
            Database db = null;
            try
            {
                // Retrieve list of snapshot dbs.
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Connect to repository.
                    connection.Open();

                    // Create parameter.
                    SqlParameter paramSnapshotid = new SqlParameter(ParamSnapshotid, snapshotid);
                    SqlParameter paramDbid = new SqlParameter(ParamDbId, dbid);

                    // Query for 
                    using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                            QuerySnapshotDatabase, new SqlParameter[] { paramSnapshotid, paramDbid }))
                    {
                        if (rdr.Read())
                        {
                            db = new Database(rdr.GetSqlInt32((int)DbColumn.SnapshotId), rdr.GetSqlInt32((int)DbColumn.DbId),
                                                        rdr.GetSqlString((int)DbColumn.DatabaseName), rdr.GetSqlString((int)DbColumn.Owner),
                                                            rdr.GetSqlString((int)DbColumn.GuestEnabled),
                                                                    rdr.GetSqlString((int)DbColumn.Trustworthy),
                                                                rdr.GetSqlString((int)DbColumn.Available),
                                                                    rdr.GetSqlString((int)DbColumn.Status));
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error("Error Retrieving Database from Snapshot", ex);
                db = null;
            }

            return db;
        }

        #endregion

        #region IComparable

        public int CompareTo(Database db)
        {
            return m_Name.CompareTo(db.m_Name);
        }

        #endregion

        #region Load From Target

        private const int FieldName = 0;

        private const string QueryTargetTablesAll2k = @"DECLARE @SQL NVARCHAR(4000) 
            SELECT @SQL = COALESCE(@SQL,'') + ' union ' + 
            'select name from ' + QUOTENAME(name) + '.dbo.sysobjects AS tbl WHERE tbl.type=''U'' or tbl.type=''S'' or tbl.type=''IT''' FROM master.dbo.sysdatabases {0}
            SELECT @SQL = SUBSTRING(@SQL, 8, len(@SQL)) + ' order by name'
            EXECUTE(@SQL)";

        private const string QueryTargetTablesUser2k = @"DECLARE @SQL NVARCHAR(4000) 
            SELECT @SQL = COALESCE(@SQL,'') + 
            'use ' + QUOTENAME(name) + ' insert into @ObjectsDataTable select name from ' + QUOTENAME(name) + '.dbo.sysobjects AS tbl WHERE (tbl.type=''U'' or tbl.type=''S'' or tbl.type=''IT'')
			and (CAST(CASE WHEN (OBJECTPROPERTY(tbl.id, N''IsMSShipped'')=1) THEN 1 WHEN 1 = OBJECTPROPERTY(tbl.id, N''IsSystemTable'') THEN 1 ELSE 0 END
            AS bit)=0) ' FROM master.dbo.sysdatabases {0}
            SELECT @SQL = 'DECLARE @ObjectsDataTable TABLE (name nvarchar(4000))
			' + @SQL + ' select distinct * from @ObjectsDataTable order by name'
            EXECUTE(@SQL)";

        private const string QueryTargetTablesSystem2k = @"DECLARE @SQL NVARCHAR(4000) 
            SELECT @SQL = COALESCE(@SQL,'') + 
            'use ' + QUOTENAME(name) + ' insert into @ObjectsDataTable select name from ' + QUOTENAME(name) + '.dbo.sysobjects AS tbl WHERE (tbl.type=''U'' or tbl.type=''S'' or tbl.type=''IT'')
			and (CAST(CASE WHEN (OBJECTPROPERTY(tbl.id, N''IsMSShipped'')=1) THEN 1 WHEN 1 = OBJECTPROPERTY(tbl.id, N''IsSystemTable'') THEN 1 ELSE 0 END
            AS bit)=1) ' FROM master.dbo.sysdatabases {0}
            SELECT @SQL = 'DECLARE @ObjectsDataTable TABLE (name nvarchar(4000))
			' + @SQL + ' select distinct * from @ObjectsDataTable order by name'
            EXECUTE(@SQL)";

        private const string QueryTargetViewsAll2k = @"DECLARE @SQL NVARCHAR(4000) 
            SELECT @SQL = COALESCE(@SQL,'') + ' union ' + 
            'select name from ' + QUOTENAME(name) + '.dbo.sysobjects AS tbl
            WHERE (tbl.type = ''V'')' FROM master.dbo.sysdatabases {0}
            SELECT @SQL = SUBSTRING(@SQL, 8, len(@SQL)) + ' order by name'
            EXECUTE(@SQL)";

        private const string QueryTargetViewsUser2k = @"DECLARE @SQL NVARCHAR(4000) 
            SELECT @SQL = COALESCE(@SQL,'') + 
            'use ' + QUOTENAME(name) + ' insert into @ObjectsDataTable select name from ' + QUOTENAME(name) + '.dbo.sysobjects AS tbl
            WHERE (tbl.type = ''V'') and (CAST(CASE WHEN (OBJECTPROPERTY(tbl.id, N''IsMSShipped'')=1) THEN 1 WHEN 1 = OBJECTPROPERTY(tbl.id, N''IsSystemTable'') THEN 1 ELSE 0 END
            AS bit)=0)' FROM master.dbo.sysdatabases {0}
            SELECT @SQL = 'DECLARE @ObjectsDataTable TABLE (name nvarchar(4000))
			' + @SQL + ' select distinct * from @ObjectsDataTable order by name'
            EXECUTE(@SQL)";

        private const string QueryTargetViewsSystem2k = @"DECLARE @SQL NVARCHAR(4000) 
            SELECT @SQL = COALESCE(@SQL,'') + 
            'use ' + QUOTENAME(name) + ' insert into @ObjectsDataTable select name from ' + QUOTENAME(name) + '.dbo.sysobjects AS tbl
            WHERE (tbl.type = ''V'') and (CAST(CASE WHEN (OBJECTPROPERTY(tbl.id, N''IsMSShipped'')=1) THEN 1 WHEN 1 = OBJECTPROPERTY(tbl.id, N''IsSystemTable'') THEN 1 ELSE 0 END
            AS bit)=1)' FROM master.dbo.sysdatabases {0}
            SELECT @SQL = 'DECLARE @ObjectsDataTable TABLE (name nvarchar(4000))
			' + @SQL + ' select distinct * from @ObjectsDataTable order by name'
            EXECUTE(@SQL)";

        private const string QueryTargetFunctionsAll2k = @"DECLARE @SQL NVARCHAR(4000) 
            SELECT @SQL = COALESCE(@SQL,'') + ' union ' + 
            'select name from ' + QUOTENAME(name) + '.dbo.sysobjects AS tbl
            WHERE (tbl.type in (''TF'', ''FN'', ''IF'', ''FS'', ''FT'', ''AF''))' FROM master.dbo.sysdatabases {0}
            SELECT @SQL = SUBSTRING(@SQL, 8, len(@SQL)) + ' order by name'
            EXECUTE(@SQL)";

        private const string QueryTargetFunctionsUser2k = @"DECLARE @SQL NVARCHAR(4000) 
            SELECT @SQL = COALESCE(@SQL,'') +
            'use ' + QUOTENAME(name) + ' insert into @ObjectsDataTable select name from ' + QUOTENAME(name) + '.dbo.sysobjects AS tbl
            WHERE (tbl.type in (''TF'', ''FN'', ''IF'', ''FS'', ''FT'', ''AF'')) 
            and (CAST(CASE WHEN (OBJECTPROPERTY(tbl.id, N''IsMSShipped'')=1) THEN 1 WHEN 1 = OBJECTPROPERTY(tbl.id, N''IsSystemTable'') THEN 1 ELSE 0 END
            AS bit)=0)' FROM master.dbo.sysdatabases {0}
            SELECT @SQL = 'DECLARE @ObjectsDataTable TABLE (name nvarchar(4000))
			' + @SQL + ' select distinct * from @ObjectsDataTable order by name'
            EXECUTE(@SQL)";

        private const string QueryTargetFunctionsSystem2k = @"DECLARE @SQL NVARCHAR(4000) 
            SELECT @SQL = COALESCE(@SQL,'') +
            'use ' + QUOTENAME(name) + ' insert into @ObjectsDataTable select name from ' + QUOTENAME(name) + '.dbo.sysobjects AS tbl
            WHERE (tbl.type in (''TF'', ''FN'', ''IF'', ''FS'', ''FT'', ''AF'')) 
            and (CAST(CASE WHEN (OBJECTPROPERTY(tbl.id, N''IsMSShipped'')=1) THEN 1 WHEN 1 = OBJECTPROPERTY(tbl.id, N''IsSystemTable'') THEN 1 ELSE 0 END
            AS bit)=1)' FROM master.dbo.sysdatabases {0}
            SELECT @SQL = 'DECLARE @ObjectsDataTable TABLE (name nvarchar(4000))
			' + @SQL + ' select distinct * from @ObjectsDataTable order by name'
            EXECUTE(@SQL)";

        private const string QueryTargetTablesAll = @"DECLARE @SQL NVARCHAR(MAX) 
            SELECT @SQL = COALESCE(@SQL,'') + ' union ' + 
            'select name from ' + QUOTENAME(name) + '.sys.tables AS tbl' FROM master.sys.databases {0}
            SELECT @SQL = SUBSTRING(@SQL, 8, len(@SQL)) + ' order by name'
            EXECUTE(@SQL)";

        private const string QueryTargetTablesUser = @"DECLARE @SQL NVARCHAR(MAX) 
            SELECT @SQL = COALESCE(@SQL,'') + ' union ' + 
            'select name from ' + QUOTENAME(name) + '.sys.tables AS tbl
            WHERE CAST(
             case 
                when tbl.is_ms_shipped = 1 then 1
                when (
                    select 
                        major_id 
                    from 
                        sys.extended_properties 
                    where 
                        major_id = tbl.object_id and 
                        minor_id = 0 and 
                        class = 1 and 
                        name = N''microsoft_database_tools_support'') 
                    is not null then 1
                else 0
            end AS bit) = 0' FROM master.sys.databases {0}
            SELECT @SQL = SUBSTRING(@SQL, 8, len(@SQL)) + ' order by name'
            EXECUTE(@SQL)";

        private const string QueryTargetTablesSystem = @"DECLARE @SQL NVARCHAR(MAX) 
            SELECT @SQL = COALESCE(@SQL,'') + ' union ' + 
            'select name from ' + QUOTENAME(name) + '.sys.tables AS tbl
            WHERE CAST(
             case 
                when tbl.is_ms_shipped = 1 then 1
                when (
                    select 
                        major_id 
                    from 
                        sys.extended_properties 
                    where 
                        major_id = tbl.object_id and 
                        minor_id = 0 and 
                        class = 1 and 
                        name = N''microsoft_database_tools_support'') 
                    is not null then 1
                else 0
            end AS bit) = 1' FROM master.sys.databases {0}
            SELECT @SQL = SUBSTRING(@SQL, 8, len(@SQL)) + ' order by name'
            EXECUTE(@SQL)";

        private const string QueryTargetViewsAll = @"DECLARE @SQL NVARCHAR(MAX) 
            SELECT @SQL = COALESCE(@SQL,'') + ' union ' + 
            'select name from ' + QUOTENAME(name) + '.sys.all_views AS tbl
            WHERE (tbl.type = ''V'')' FROM master.sys.databases {0}
            SELECT @SQL = SUBSTRING(@SQL, 8, len(@SQL)) + ' order by name'
            EXECUTE(@SQL)";

        private const string QueryTargetViewsUser = @"DECLARE @SQL NVARCHAR(MAX) 
            SELECT @SQL = COALESCE(@SQL,'') + ' union ' + 
            'select name from ' + QUOTENAME(name) + '.sys.all_views AS tbl
            WHERE (tbl.type = ''V'')and(CAST(
             case 
                when tbl.is_ms_shipped = 1 then 1
                when (
                    select 
                        major_id 
                    from 
                        sys.extended_properties 
                    where 
                        major_id = tbl.object_id and 
                        minor_id = 0 and 
                        class = 1 and 
                        name = N''microsoft_database_tools_support'') 
                    is not null then 1
                else 0
            end AS bit) = 0)' master.FROM sys.databases {0}
            SELECT @SQL = SUBSTRING(@SQL, 8, len(@SQL)) + ' order by name'
            EXECUTE(@SQL)";

        private const string QueryTargetViewsSystem = @"DECLARE @SQL NVARCHAR(MAX) 
            SELECT @SQL = COALESCE(@SQL,'') + ' union ' + 
            'select name from ' + QUOTENAME(name) + '.sys.all_views AS tbl
            WHERE (tbl.type = ''V'')and(CAST(
             case 
                when tbl.is_ms_shipped = 1 then 1
                when (
                    select 
                        major_id 
                    from 
                        sys.extended_properties 
                    where 
                        major_id = tbl.object_id and 
                        minor_id = 0 and 
                        class = 1 and 
                        name = N''microsoft_database_tools_support'') 
                    is not null then 1
                else 0
            end AS bit) = 1)' FROM master.sys.databases {0}
            SELECT @SQL = SUBSTRING(@SQL, 8, len(@SQL)) + ' order by name'
            EXECUTE(@SQL)";

        private const string QueryTargetFunctionsAll = @"DECLARE @SQL NVARCHAR(MAX) 
            SELECT @SQL = COALESCE(@SQL,'') + ' union ' + 
            'select name from ' + QUOTENAME(name) + '.sys.all_objects AS tbl
            WHERE (tbl.type in (''TF'', ''FN'', ''IF'', ''FS'', ''FT'', ''AF''))' FROM master.sys.databases {0}
            SELECT @SQL = SUBSTRING(@SQL, 8, len(@SQL)) + ' order by name'
            EXECUTE(@SQL)";

        private const string QueryTargetFunctionsUser = @"DECLARE @SQL NVARCHAR(MAX) 
            SELECT @SQL = COALESCE(@SQL,'') + ' union ' + 
            'select name from ' + QUOTENAME(name) + '.sys.all_objects AS tbl
            WHERE (tbl.type in (''TF'', ''FN'', ''IF'', ''FS'', ''FT'', ''AF''))and(CAST(
             case 
                when tbl.is_ms_shipped = 1 then 1
                when (
                    select 
                        major_id 
                    from 
                        sys.extended_properties 
                    where 
                        major_id = tbl.object_id and 
                        minor_id = 0 and 
                        class = 1 and 
                        name = N''microsoft_database_tools_support'') 
                    is not null then 1
                else 0
            end AS bit) = 0)' FROM master.sys.databases {0}
            SELECT @SQL = SUBSTRING(@SQL, 8, len(@SQL)) + ' order by name'
            EXECUTE(@SQL)";

        private const string QueryTargetFunctionsSystem = @"DECLARE @SQL NVARCHAR(MAX) 
            SELECT @SQL = COALESCE(@SQL,'') + ' union ' + 
            'select name from ' + QUOTENAME(name) + '.sys.all_objects AS tbl
            WHERE (tbl.type in (''TF'', ''FN'', ''IF'', ''FS'', ''FT'', ''AF''))and(CAST(
             case 
                when tbl.is_ms_shipped = 1 then 1
                when (
                    select 
                        major_id 
                    from 
                        sys.extended_properties 
                    where 
                        major_id = tbl.object_id and 
                        minor_id = 0 and 
                        class = 1 and 
                        name = N''microsoft_database_tools_support'') 
                    is not null then 1
                else 0
            end AS bit) = 1)' FROM master.sys.databases {0}
            SELECT @SQL = SUBSTRING(@SQL, 8, len(@SQL)) + ' order by name'
            EXECUTE(@SQL)";

        private static string PrepareDatabaseFilter(ServerVersion sqlServerVersion, string query, FilterObject databaseFilterObject)
        {
            string qWhere = "";
            if (databaseFilterObject != null)
            {
                if (databaseFilterObject.MatchStringList.Count > 0 && databaseFilterObject.MatchStringList[0] != "")
                {
                    string qWhereLike = "";
                    foreach (string s in databaseFilterObject.MatchStringList)
                    {
                        if (qWhereLike != "")
                            qWhereLike += " or ";
                        qWhereLike += "name like '" + s + "'";
                    }
                    if (qWhereLike != "")
                    {
                        qWhere = "WHERE (" + qWhereLike + ")";
                    }
                }
                if (databaseFilterObject.ObjectScope != RuleScope.All)
                {
                    string qWhereScope = "";
                    if (sqlServerVersion == ServerVersion.SQL2000)
                    {
                        switch (databaseFilterObject.ObjectScope)
                        {
                            case RuleScope.System: 
                                qWhereScope = "CAST(case WHEN name in ('master','model','msdb','tempdb') THEN 1 else category & 16 end AS bit) = 1";
                                break;
                            case RuleScope.User: 
                                qWhereScope = "CAST(case WHEN name in ('master','model','msdb','tempdb') THEN 1 else category & 16 end AS bit) = 0";
                                break;
                        }
                    }
                    else
                    {
                        switch (databaseFilterObject.ObjectScope)
                        {
                            case RuleScope.System: 
                                qWhereScope = "CAST(CASE WHEN name IN ('master', 'model', 'msdb', 'tempdb') THEN 1 ELSE is_distributor END As bit) = 1";
                                break;
                            case RuleScope.User: 
                                qWhereScope = "CAST(CASE WHEN name IN ('master', 'model', 'msdb', 'tempdb') THEN 1 ELSE is_distributor END As bit) = 0";
                                break;
                        }
                    }
                    if (qWhere == "")
                        qWhere = "WHERE ";
                    else
                        qWhere += "AND ";
                    qWhere += "(" + qWhereScope + ")";
                }
            }
            return string.Format(query, qWhere);
        }

        public static bool GetTargetDatabases(
                ServerVersion sqlServerVersion,
                RuleScope scope,
                string targetConnectionString,
                out List<string> databaseList
            )
        {
            // Init returns.
            bool isOk = true;
            databaseList = new List<string>();

            // Check inputs.
            if (string.IsNullOrEmpty(targetConnectionString) || sqlServerVersion == ServerVersion.Unsupported)
            {
                isOk = false;
            }

            if (isOk)
            {
                // Connect and load the databases.
                using (SqlConnection connection = new SqlConnection(targetConnectionString))
                {
                    try
                    {
                        connection.Open();

                        // Create the query
                        string query = "";

                        if (sqlServerVersion == ServerVersion.SQL2000)
                        {
                            query = "SELECT name FROM master.dbo.sysdatabases";
                            switch(scope)
                            {
                                case RuleScope.System: 
                                    query += " WHERE CAST(case WHEN name in ('master','model','msdb','tempdb') THEN 1 else category & 16 end AS bit) = 1";
                                    break;
                                case RuleScope.User: 
                                    query += " WHERE CAST(case WHEN name in ('master','model','msdb','tempdb') THEN 1 else category & 16 end AS bit) = 0";
                                    break;
                            }
                            query += " order by name";
                        }
                        else
                        {
                            query = "SELECT name FROM master.sys.databases";
                            switch (scope)
                            {
                                case RuleScope.System: 
                                    query += " WHERE CAST(CASE WHEN name IN ('master', 'model', 'msdb', 'tempdb') THEN 1 ELSE is_distributor END As bit) = 1";
                                    break;
                                case RuleScope.User: 
                                    query += " WHERE CAST(CASE WHEN name IN ('master', 'model', 'msdb', 'tempdb') THEN 1 ELSE is_distributor END As bit) = 0";
                                    break;
                            }
                            query += " order by name";
                        }

                        // Get a list of databases from the target instance.
                        using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text, query, null))
                        {
                            while (rdr.Read())
                            {
                                SqlString name = rdr.GetSqlString(FieldName);
                                databaseList.Add(name.Value);
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        databaseList.Clear();
                        databaseList = null;
                        isOk = false;
                    }
                }
            }
            return isOk;
        }

        public static bool GetTargetTables(
                ServerVersion sqlServerVersion,
                RuleScope scope,
                FilterObject databaseFilterObject,
                string targetConnectionString,
                out List<string> viewsList
            )
        {
            // Init returns.
            bool isOk = true;
            viewsList = new List<string>();

            // Check inputs.
            if (string.IsNullOrEmpty(targetConnectionString) || sqlServerVersion == ServerVersion.Unsupported)
            {
                isOk = false;
            }

            if (isOk)
            {
                // Connect and load the databases.
                using (SqlConnection connection = new SqlConnection(targetConnectionString))
                {
                    try
                    {
                        connection.Open();

                        // Create the query
                        string query = "";
                        if (sqlServerVersion == ServerVersion.SQL2000)
                        {
                            switch (scope)
                            {
                                case RuleScope.All: 
                                    query = QueryTargetTablesAll2k;
                                    break;
                                case RuleScope.System: 
                                    query = QueryTargetTablesSystem2k;
                                    break;
                                case RuleScope.User: 
                                    query = QueryTargetTablesUser2k;
                                    break;
                            }
                        }
                        else
                        {
                            switch (scope)
                            {
                                case RuleScope.All: 
                                    query = QueryTargetTablesAll;
                                    break;
                                case RuleScope.System: 
                                    query = QueryTargetTablesSystem;
                                    break;
                                case RuleScope.User: 
                                    query = QueryTargetTablesUser;
                                    break;
                            }
                        }
                        query = PrepareDatabaseFilter(sqlServerVersion, query, databaseFilterObject);

                        // Get a list of tables from the target instance.
                        using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text, query, null))
                        {
                            while (rdr.Read())
                            {
                                SqlString name = rdr.GetSqlString(FieldName);
                                viewsList.Add(name.Value);
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        viewsList.Clear();
                        viewsList = null;
                        isOk = false;
                    }
                }
            }
            return isOk;
        }
        
        public static bool GetTargetViews(
                ServerVersion sqlServerVersion,
                RuleScope scope,
                FilterObject databaseFilterObject,
                string targetConnectionString,
                out List<string> viewsList
            )
        {
            // Init returns.
            bool isOk = true;
            viewsList = new List<string>();

            // Check inputs.
            if (string.IsNullOrEmpty(targetConnectionString) || sqlServerVersion == ServerVersion.Unsupported)
            {
                isOk = false;
            }

            if (isOk)
            {
                // Connect and load the databases.
                using (SqlConnection connection = new SqlConnection(targetConnectionString))
                {
                    try
                    {
                        connection.Open();

                        // Create the query
                        string query = "";
                        if (sqlServerVersion == ServerVersion.SQL2000)
                        {
                            switch (scope)
                            {
                                case RuleScope.All: 
                                    query = QueryTargetViewsAll2k;
                                    break;
                                case RuleScope.System: 
                                    query = QueryTargetViewsSystem2k;
                                    break;
                                case RuleScope.User: 
                                    query = QueryTargetViewsUser2k;
                                    break;
                            }
                        }
                        else
                        {
                            switch (scope)
                            {
                                case RuleScope.All: 
                                    query = QueryTargetViewsAll;
                                    break;
                                case RuleScope.System: 
                                    query = QueryTargetViewsSystem;
                                    break;
                                case RuleScope.User: 
                                    query = QueryTargetViewsUser;
                                    break;
                            }
                        }
                        query = PrepareDatabaseFilter(sqlServerVersion, query, databaseFilterObject);

                        // Get a list of views from the target instance.
                        using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text, query, null))
                        {
                            while (rdr.Read())
                            {
                                SqlString name = rdr.GetSqlString(FieldName);
                                viewsList.Add(name.Value);
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        viewsList.Clear();
                        viewsList = null;
                        isOk = false;
                    }
                }
            }
            return isOk;
        }

        public static bool GetTargetFunctions(
                ServerVersion sqlServerVersion,
                RuleScope scope,
                FilterObject databaseFilterObject,
                string targetConnectionString,
                out List<string> viewsList
            )
        {
            // Init returns.
            bool isOk = true;
            viewsList = new List<string>();

            // Check inputs.
            if (string.IsNullOrEmpty(targetConnectionString) || sqlServerVersion == ServerVersion.Unsupported)
            {
                isOk = false;
            }

            if (isOk)
            {
                // Connect and load the databases.
                using (SqlConnection connection = new SqlConnection(targetConnectionString))
                {
                    try
                    {
                        connection.Open();

                        // Create the query
                        string query = "";
                        if (sqlServerVersion == ServerVersion.SQL2000)
                        {
                            switch (scope)
                            {
                                case RuleScope.All: 
                                    query = QueryTargetFunctionsAll2k;
                                    break;
                                case RuleScope.System: 
                                    query = QueryTargetFunctionsSystem2k;
                                    break;
                                case RuleScope.User: 
                                    query = QueryTargetFunctionsUser2k;
                                    break;
                            }
                        }
                        else
                        {
                            switch (scope)
                            {
                                case RuleScope.All: 
                                    query = QueryTargetFunctionsAll;
                                    break;
                                case RuleScope.System: 
                                    query = QueryTargetFunctionsSystem;
                                    break;
                                case RuleScope.User: 
                                    query = QueryTargetFunctionsUser;
                                    break;
                            }
                        }
                        query = PrepareDatabaseFilter(sqlServerVersion, query, databaseFilterObject);

                        // Get a list of views from the target instance.
                        using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text, query, null))
                        {
                            while (rdr.Read())
                            {
                                SqlString name = rdr.GetSqlString(FieldName);
                                viewsList.Add(name.Value);
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        viewsList.Clear();
                        viewsList = null;
                        isOk = false;
                    }
                }
            }
            return isOk;
        }

        #endregion
    }
}
