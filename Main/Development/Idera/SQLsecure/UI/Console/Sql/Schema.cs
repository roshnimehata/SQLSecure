/******************************************************************
 * Name: Schema.cs
 *
 * Description: Encapsulates database schema object.
 *
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
using System.Diagnostics;

namespace Idera.SQLsecure.UI.Console.Sql
{
    class Schema
    {
        #region Fields

        private int m_SnapshotId = -1;
        private int m_DbId = -1;
        private int m_SchemaId = -1;
        private string m_SchemaName = string.Empty;
        private string m_OwnerName = string.Empty;

        #endregion

        #region Queries

        // Get server roles saved in a specific snapshot.
        private const string QueryGetSnapshotSchemas
                                = @"SELECT 
                                        snapshotid,
                                        dbid,
                                        schemaid,
                                        schemaname, 
                                        ownerid,
                                        ownername,
                                        ownertype
                                    FROM SQLsecure.dbo.vwschema
                                    WHERE snapshotid = @snapshotid AND dbid = @dbid";
        private const string QueryGetSnapshotSchema
                                = @"SELECT 
                                        snapshotid,
                                        dbid,
                                        schemaid,
                                        schemaname, 
                                        ownerid,
                                        ownername,
                                        ownertype
                                    FROM SQLsecure.dbo.vwschema
                                    WHERE snapshotid = @snapshotid AND dbid = @dbid AND schemaid = @schemaid";
        private const string ParamSnapshotid = "snapshotid";
        private const string ParamDbid = "dbid";
        private const string ParamSchemaId = "schemaid";
        private enum Columns
        {
            SnapshotId = 0,
            DbId,
            SchemaId,
            SchemaName,
            OwnerId,
            OwnerName,
            OwnerType
        };

        #endregion

        #region Ctors

        public Schema(
                SqlInt32 snapshotid,
                SqlInt32 dbid,
                SqlInt32 schemaid,
                SqlString schemaname,
                SqlString ownername
            )
        {
            m_SnapshotId = snapshotid.IsNull ? -1 : snapshotid.Value;
            m_DbId = dbid.IsNull ? -1 : dbid.Value;
            m_SchemaId = schemaid.IsNull ? -1 : schemaid.Value; 
            m_SchemaName = schemaname.IsNull ? string.Empty : schemaname.Value;
            m_OwnerName = ownername.IsNull ? string.Empty : ownername.Value;
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

        public int SchemaId
        {
            get { return m_SchemaId; }
        }

        public int ClassId
        {
            get { return 3; }
        }

        public string SchemaName
        {
            get { return m_SchemaName; }
        }

        public string OwnerName
        {
            get { return m_OwnerName; }
        }

        #endregion

        #region Methods

        public static List<Schema> GetSnapshotSchemas(
                int snapshotid,
                int dbid
            )
        {
            Debug.Assert(snapshotid != 0);

            // Init return.
            List<Schema> schemas = new List<Schema>();

            // Open connection to repository and retrieve schemas.
            using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
            {
                // Open the connection.
                connection.Open();

                // Query the repository for schemas.
                SqlParameter paramSnapshotid = new SqlParameter(ParamSnapshotid, snapshotid);
                SqlParameter paramDbid = new SqlParameter(ParamDbid, dbid);
                using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                            QueryGetSnapshotSchemas, new SqlParameter[] { paramSnapshotid, paramDbid }))
                {
                    while (rdr.Read())
                    {
                        // Create the schema and add to list.
                        Schema s = new Schema (rdr.GetSqlInt32((int)Columns.SnapshotId), rdr.GetSqlInt32((int)Columns.DbId),
                                                    rdr.GetSqlInt32((int)Columns.SchemaId), rdr.GetSqlString((int)Columns.SchemaName),
                                                        rdr.GetSqlString((int)Columns.OwnerName));
                        schemas.Add(s);
                    }
                }
            }

            return schemas;
        }

        public static Schema GetSnapshotSchema(
                int snapshotid,
                int dbid,
                int schemaid
            )
        {
            Debug.Assert(snapshotid != 0);

            // Init return.
            Schema schema = null;

            // Open connection to repository and retrieve schemas.
            using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
            {
                // Open the connection.
                connection.Open();

                // Query the repository for schemas.
                SqlParameter paramSnapshotid = new SqlParameter(ParamSnapshotid, snapshotid);
                SqlParameter paramDbid = new SqlParameter(ParamDbid, dbid);
                SqlParameter paramSchemaid = new SqlParameter(ParamSchemaId, schemaid);
                using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                            QueryGetSnapshotSchemas, new SqlParameter[] { paramSnapshotid, paramDbid, paramSchemaid }))
                {
                    if (rdr.Read())
                    {
                        // Create the schema and add to list.
                        schema = new Schema(rdr.GetSqlInt32((int)Columns.SnapshotId), rdr.GetSqlInt32((int)Columns.DbId),
                                                    rdr.GetSqlInt32((int)Columns.SchemaId), rdr.GetSqlString((int)Columns.SchemaName),
                                                        rdr.GetSqlString((int)Columns.OwnerName));
                    }
                }
            }

            return schema;
        }

        #endregion
    }
}
