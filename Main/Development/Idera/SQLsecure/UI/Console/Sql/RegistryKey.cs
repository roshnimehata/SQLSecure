/******************************************************************
 * Name: RegistryKey.cs
 *
 * Description: Encapsulates a Registry Key File System Object.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2007 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;

using Idera.SQLsecure.Core.Accounts;

namespace Idera.SQLsecure.UI.Console.Sql
{
    class RegistryKey
    {
        #region Fields

        private int m_Id;
        private string m_Name;
        private string m_LongName;
        private Sid m_OwnerSid;
        private string m_OwnerName;

        #endregion

        #region Helpers

        #endregion

        #region Queries

        // Get keys saved in a specific snapshot.
        private const string QueryGetSnapshotRegistryKeys
                                = @"SELECT
                                        osobjectid,
                                        objectname,
                                        longname,
                                        ownersid,
                                        ownername
                                    FROM SQLsecure.dbo.vwregistrykey
                                    WHERE snapshotid = @snapshotid";
        private const string QueryGetSnapshotRegistryKey
                                = @"SELECT
                                        osobjectid,
                                        objectname,
                                        longname,
                                        ownersid,
                                        ownername                                     
                                    FROM SQLsecure.dbo.vwregistrykey
                                    WHERE snapshotid = @snapshotid AND osobjectid = @osobjectid";
        private const string ParamSnapshotid = "snapshotid";
        private const string ParamObjectId = "osobjectid";
        private enum RegistryColumns
        {
            Id = 0,
            Name,
            LongName,
            OwnerSid,
            OwnerName
        }

        #endregion

        #region Ctors

        public RegistryKey(
                SqlInt32 osObjectId,
                SqlString name,
                SqlString longName,
                SqlBytes ownerSid,
                SqlString ownerName
            )
        {
            m_Id = osObjectId.Value;
            m_Name = name.Value;
            m_LongName = longName.Value;
            m_OwnerSid = new Sid(ownerSid.Value);
            m_OwnerName = ownerName.Value;
        }

        #endregion

        #region Properties

        public int OsObjectId
        {
            get { return m_Id; }
        }

        public string Name
        {
            get { return m_LongName; }
        }

        public Sid OwnerSid
        {
            get { return m_OwnerSid; }
        }

        public string Owner
        {
            get { return m_OwnerName; }
        }

        #endregion

        #region Methods

        public static List<RegistryKey> GetSnapshotKeys(
                int snapshotid
            )
        {
            Debug.Assert(snapshotid != 0);

            // Init return.
            List<RegistryKey> keys = new List<RegistryKey>();

            // Open connection to repository and retrieve keys.
            using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
            {
                // Open the connection.
                connection.Open();

                // Query the repository for registry keys for the server.
                SqlParameter paramSnapshotid = new SqlParameter(ParamSnapshotid, snapshotid);
                using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                            QueryGetSnapshotRegistryKeys, new SqlParameter[] { paramSnapshotid }))
                {
                    while (rdr.Read())
                    {
                        // Read the fields.
                        SqlInt32 id = rdr.GetSqlInt32((int)RegistryColumns.Id);
                        SqlString name = rdr.GetSqlString((int)RegistryColumns.Name);
                        SqlString longname = rdr.GetSqlString((int)RegistryColumns.LongName);
                        SqlBytes ownerSid = rdr.GetSqlBytes((int)RegistryColumns.OwnerSid);
                        SqlString ownerName = rdr.GetSqlString((int)RegistryColumns.OwnerName);

                        // Create the key and add to list.
                        RegistryKey r = new RegistryKey(id, name, longname, ownerSid, ownerName);
                        keys.Add(r);
                    }
                }
            }

            return keys;
        }

        public static RegistryKey GetSnapshotKey(
                int snapshotid,
                int objectId
            )
        {
            Debug.Assert(snapshotid != 0);

            // Init return.
            RegistryKey key = null;

            // Open connection to repository and retrieve keys.
            using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
            {
                // Open the connection.
                connection.Open();

                // Query the repository for registry keys for the server.
                SqlParameter paramSnapshotid = new SqlParameter(ParamSnapshotid, snapshotid);
                SqlParameter paramObjectId = new SqlParameter(ParamObjectId, objectId);
                using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                            QueryGetSnapshotRegistryKey, new SqlParameter[] { paramSnapshotid, paramObjectId }))
                {
                    if (rdr.Read())
                    {
                        // Read the fields.
                        SqlInt32 id = rdr.GetSqlInt32((int)RegistryColumns.Id);
                        SqlString name = rdr.GetSqlString((int)RegistryColumns.Name);
                        SqlString longname = rdr.GetSqlString((int)RegistryColumns.LongName);
                        SqlBytes ownerSid = rdr.GetSqlBytes((int)RegistryColumns.OwnerSid);
                        SqlString ownerName = rdr.GetSqlString((int)RegistryColumns.OwnerName);

                        // Create the key.
                        key = new RegistryKey(id, name, longname, ownerSid, ownerName);
                    }
                }
            }

            return key;
        }

        #endregion
    }
}
