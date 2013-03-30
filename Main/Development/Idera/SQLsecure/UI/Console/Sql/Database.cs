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
        private static string QuerySnapshotDatabase
                = QuerySnapshotDatabases + @" AND dbid = @dbid";
        private const string ParamSnapshotid = "snapshotid";
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
    }
}
