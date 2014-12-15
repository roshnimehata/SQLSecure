/******************************************************************
 * Name: DatabaseObject.cs
 *
 * Description: Encapsulates database objects.
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

using Idera.SQLsecure.UI.Console.Utility;

namespace Idera.SQLsecure.UI.Console.Sql
{
    class DatabaseObject
    {
        #region Types

        public class Properties
        {
            #region Fields

            private SqlString m_Name;
            private SqlString m_Type;
            private SqlInt32 m_OwnerId;
            private SqlString m_Owner;
            private SqlInt32 m_SchemaId;
            private SqlString m_SchemaName;
            private SqlString m_SchemaOwner;

            #endregion

            #region Queries

            private const string QueryInfo =
                    @"SQLsecure.dbo.isp_sqlsecure_getdatabaseobjectinfo";

            private const string ParamSnapshotId = @"@snapshotid";
            private const string ParamDbId = @"@dbid";
            private const string ParamClassId = @"@classid";
            private const string ParamParentObjectId = @"@parentobjectid";
            private const string ParamObjectId = @"@objectid";

            private enum Columns
            {
                Name,
                Type,
                OwnerId,
                Owner,
                SchemaId,
                SchemaName,
                SchemaOwner
            }

            #endregion

            #region Ctor

            private Properties(
                    SqlString name,
                    SqlString type,
                    SqlInt32 ownerid,
                    SqlString owner,
                    SqlInt32 schemaid,
                    SqlString schemaname,
                    SqlString schemaowner
                )
            {
                m_Name = name;
                m_Type = type;
                m_OwnerId = ownerid;
                m_Owner = owner;
                m_SchemaId = schemaid;
                m_SchemaName = schemaname;
                m_SchemaOwner = schemaowner;
            }

            #endregion

            #region Properties

            public string Name
            {
                get { return m_Name.IsNull ? string.Empty : m_Name.Value; }
            }

            public string Type
            {
                get { return m_Type.IsNull ? string.Empty : m_Type.Value; }
            }

            public string Owner
            {
                get { return m_Owner.IsNull ? string.Empty : m_Owner.Value; }
            }

            public string SchemaName
            {
                get { return m_SchemaName.IsNull ? string.Empty : m_SchemaName.Value; }
            }

            public string SchemaOwner
            {
                get { return m_SchemaOwner.IsNull ? string.Empty : m_SchemaOwner.Value; }
            }

            #endregion

            #region Methods

            public static Properties Get (
                    int snapshotid,
                    int dbid,
                    int classid,
                    int parentobjectid,
                    int objectid
                )
            {
                Properties p = null;

                try
                {
                    using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                    {
                        // Open the connection.
                        connection.Open();

                        // Setup the params.
                        SqlParameter paramSnapshotId = new SqlParameter(ParamSnapshotId, snapshotid);
                        SqlParameter paramDbId = new SqlParameter(ParamDbId, dbid);
                        SqlParameter paramClassId = new SqlParameter(ParamClassId, classid);
                        SqlParameter paramParentObjectId = new SqlParameter(ParamParentObjectId, parentobjectid);
                        SqlParameter paramObjectId = new SqlParameter(ParamObjectId, objectid);

                        // Read info from the repository.
                        using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.StoredProcedure,
                                    QueryInfo, new SqlParameter[] { paramSnapshotId, paramDbId, paramClassId, paramParentObjectId, paramObjectId }))
                        {
                            if(rdr.Read())
                            {
                                SqlString name = rdr.GetSqlString((int)Columns.Name);
                                SqlString type = rdr.GetSqlString((int)Columns.Type);
                                SqlInt32 ownerid = rdr.GetSqlInt32((int)Columns.OwnerId);
                                SqlString owner = rdr.GetSqlString((int)Columns.Owner);
                                SqlInt32 schemaid = rdr.GetSqlInt32((int)Columns.SchemaId);
                                SqlString schemaname = rdr.GetSqlString((int)Columns.SchemaName);
                                SqlString schemaowner = rdr.GetSqlString((int)Columns.SchemaOwner);

                                p = new Properties(name, type, ownerid, owner, schemaid, schemaname, schemaowner);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    p = null;
                    MsgBox.ShowError(ErrorMsgs.CantGetDbObjectProperties, ex);
                }

                return p;
            }

            #endregion
        }

        #endregion

        #region Fields

        private int m_ClassId;
        private int m_ParentObjectId;
        private int m_ObjectId;
        private string m_Type;
        private string m_Name;

        #endregion

        #region Queries

        private const string QueryGetSnapshotTables
                                = @"SELECT 
                                        classid, 
                                        parentobjectid, 
                                        objectid,
                                        type,
                                        name
                                    FROM SQLsecure.dbo.vwdatabaseobject
                                    WHERE snapshotid = @snapshotid AND dbid = @dbid AND type IN ('S', 'IT', 'U')";

        private const string QueryGetSnapshotViews
                                = @"SELECT 
                                        classid, 
                                        parentobjectid, 
                                        objectid,
                                        type,
                                        name
                                    FROM SQLsecure.dbo.vwdatabaseobject
                                    WHERE snapshotid = @snapshotid AND dbid = @dbid AND type = 'V'";

        private const string QueryGetSnapshotSynonyms
                                = @"SELECT 
                                        classid, 
                                        parentobjectid, 
                                        objectid,
                                        type,
                                        name
                                    FROM SQLsecure.dbo.vwdatabaseobject
                                    WHERE snapshotid = @snapshotid AND dbid = @dbid AND type = 'SN'";

        private const string QueryGetSnapshotStoredProcedures
                                = @"SELECT 
                                        classid, 
                                        parentobjectid, 
                                        objectid,
                                        type,
                                        name
                                    FROM SQLsecure.dbo.vwdatabaseobject
                                    WHERE snapshotid = @snapshotid AND dbid = @dbid AND type IN ('P', 'PC')";

        private const string QueryGetSnapshotFunctions
                                = @"SELECT 
                                        classid, 
                                        parentobjectid, 
                                        objectid,
                                        type,
                                        name
                                    FROM SQLsecure.dbo.vwdatabaseobject
                                    WHERE snapshotid = @snapshotid AND dbid = @dbid AND type IN ('AF', 'FN', 'FS', 'FT', 'IF', 'TF')";

        private const string QueryGetSnapshotExtendedStoredProcedures
                                = @"SELECT 
                                        classid, 
                                        parentobjectid, 
                                        objectid,
                                        type,
                                        name
                                    FROM SQLsecure.dbo.vwdatabaseobject
                                    WHERE snapshotid = @snapshotid AND dbid = @dbid AND type = 'X'";

        private const string QueryGetSnapshotAssemblies
                                = @"SELECT 
                                        classid, 
                                        parentobjectid, 
                                        objectid,
                                        type,
                                        name
                                    FROM SQLsecure.dbo.vwdatabaseobject
                                    WHERE snapshotid = @snapshotid AND dbid = @dbid AND type = 'iASM'";

        private const string QueryGetSnapshotUserDefinedDataTypes
                                = @"SELECT 
                                        classid, 
                                        parentobjectid, 
                                        objectid,
                                        type,
                                        name
                                    FROM SQLsecure.dbo.vwdatabaseobject
                                    WHERE snapshotid = @snapshotid AND dbid = @dbid AND type = 'iUDT'";

        private const string QueryGetSnapshotXMLSchemaCollections
                                = @"SELECT 
                                        classid, 
                                        parentobjectid, 
                                        objectid,
                                        type,
                                        name
                                    FROM SQLsecure.dbo.vwdatabaseobject
                                    WHERE snapshotid = @snapshotid AND dbid = @dbid AND type = 'iXMLS'";

        private const string QueryGetSnapshotFullTextCatalogs
                                = @"SELECT 
                                        classid, 
                                        parentobjectid, 
                                        objectid,
                                        type,
                                        name
                                    FROM SQLsecure.dbo.vwdatabaseobject
                                    WHERE snapshotid = @snapshotid AND dbid = @dbid AND type = 'iFTXT'";

        private const string QueryGetSnapshotSequenceObjects
                                = @"SELECT 
                                        classid, 
                                        parentobjectid, 
                                        objectid,
                                        type,
                                        name
                                    FROM SQLsecure.dbo.vwdatabaseobject
                                    WHERE snapshotid = @snapshotid AND dbid = @dbid AND type = 'SO'";

        private const string ParamQueryGetSnapshotDatabaseObjectsSnapshotid = "snapshotid";
        private const string ParamQueryGetSnapshotDatabaseObjectsDbid = "dbid";
        private enum SnapshotDatabaseObjectColumns
        {
            ClassId = 0,
            ParentObjectId,
            ObjectId,
            Type,
            Name
        };

        #endregion

        #region Ctors

        public DatabaseObject(
                int classId,
                int parentObjectId,
                int objectId,
                string type,
                string name
            )
        {
            m_ClassId = classId;
            m_ParentObjectId = parentObjectId;
            m_ObjectId = objectId;
            m_Type = type;
            m_Name = name;
        }

        public DatabaseObject(
                SqlInt32 classId,
                SqlInt32 parentObjectId,
                SqlInt32 objectId,
                SqlString type,
                SqlString name
            )
        {
            m_ClassId = classId.Value;
            m_ParentObjectId = parentObjectId.Value;
            m_ObjectId = objectId.Value;
            m_Type = type.Value;
            m_Name = name.Value;
        }

        #endregion

        #region Properties

        public int ClassId
        {
            get { return m_ClassId; }
        }
        public int ParentObjectId
        {
            get { return m_ParentObjectId; }
        }
        public int ObjectId
        {
            get { return m_ObjectId; }
        }
        public string Type
        {
            get { return m_Type; }
        }
        public string Name
        {
            get { return m_Name; }
        }

        #endregion

        #region Methods

        private static List<DatabaseObject> getSnapshotDbObject(
                string connectionString,
                int snapshotid,
                int dbid,
                string query
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(connectionString));
            Debug.Assert(snapshotid != 0);
            Debug.Assert(!string.IsNullOrEmpty(query));

            // Init return.
            List<DatabaseObject> dbObjs = new List<DatabaseObject>();

            // Open connection to repository and get db objs.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open the connection.
                connection.Open();

                // Query the repository for db objects.
                SqlParameter paramSnapshotid = new SqlParameter(ParamQueryGetSnapshotDatabaseObjectsSnapshotid, snapshotid);
                SqlParameter paramDbid = new SqlParameter(ParamQueryGetSnapshotDatabaseObjectsDbid, dbid);
                using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                            query, new SqlParameter[] { paramSnapshotid, paramDbid }))
                {
                    while (rdr.Read())
                    {
                        // Read the fields.
                        SqlInt32 classid = rdr.GetSqlInt32((int)SnapshotDatabaseObjectColumns.ClassId);
                        SqlInt32 parentobjectid = rdr.GetSqlInt32((int)SnapshotDatabaseObjectColumns.ParentObjectId);
                        SqlInt32 objectid = rdr.GetSqlInt32((int)SnapshotDatabaseObjectColumns.ObjectId);
                        SqlString type = rdr.GetSqlString((int)SnapshotDatabaseObjectColumns.Type);
                        SqlString name = rdr.GetSqlString((int)SnapshotDatabaseObjectColumns.Name);

                        // Create the db object and add to list.
                        DatabaseObject dbobj = new DatabaseObject(classid, parentobjectid, objectid, type, name);
                        dbObjs.Add(dbobj);
                    }
                }
            }

            return dbObjs;
        }

        public static List<DatabaseObject> GetSnapshotTables(
                string connectionString,
                int snapshotid,
                int dbid
            )
        {
            return getSnapshotDbObject(connectionString, snapshotid, dbid, QueryGetSnapshotTables);
        }

        public static List<DatabaseObject> GetSnapshotViews(
                string connectionString,
                int snapshotid,
                int dbid
            )
        {
            return getSnapshotDbObject(connectionString, snapshotid, dbid, QueryGetSnapshotViews);
        }

        public static List<DatabaseObject> GetSnapshotSynonyms(
                string connectionString,
                int snapshotid,
                int dbid
            )
        {
            return getSnapshotDbObject(connectionString, snapshotid, dbid, QueryGetSnapshotSynonyms);
        }

        public static List<DatabaseObject> GetSnapshotStoredProcedures(
                string connectionString,
                int snapshotid,
                int dbid
            )
        {
            return getSnapshotDbObject(connectionString, snapshotid, dbid, QueryGetSnapshotStoredProcedures);
        }

        public static List<DatabaseObject> GetSnapshotFunctions(
                string connectionString,
                int snapshotid,
                int dbid
            )
        {
            return getSnapshotDbObject(connectionString, snapshotid, dbid, QueryGetSnapshotFunctions);
        }

        public static List<DatabaseObject> GetSnapshotExtendedStoredProcedures(
                string connectionString,
                int snapshotid,
                int dbid
            )
        {
            return getSnapshotDbObject(connectionString, snapshotid, dbid, QueryGetSnapshotExtendedStoredProcedures);
        }

        public static List<DatabaseObject> GetSnapshotAssemblies(
                string connectionString,
                int snapshotid,
                int dbid
            )
        {
            return getSnapshotDbObject(connectionString, snapshotid, dbid, QueryGetSnapshotAssemblies);
        }

        public static List<DatabaseObject> GetSnapshotUserDefinedDataTypes(
                string connectionString,
                int snapshotid,
                int dbid
            )
        {
            return getSnapshotDbObject(connectionString, snapshotid, dbid, QueryGetSnapshotUserDefinedDataTypes);
        }

        public static List<DatabaseObject> GetSnapshotXMLSchemaCollections(
                string connectionString,
                int snapshotid,
                int dbid
            )
        {
            return getSnapshotDbObject(connectionString, snapshotid, dbid, QueryGetSnapshotXMLSchemaCollections);
        }

        public static List<DatabaseObject> GetSnapshotFullTextCatalogs(
                string connectionString,
                int snapshotid,
                int dbid
            )
        {
            return getSnapshotDbObject(connectionString, snapshotid, dbid, QueryGetSnapshotFullTextCatalogs);
        }

        public static List<DatabaseObject> GetSnapshotSequenceObjects(
               string connectionString,
               int snapshotid,
               int dbid
           )
        {
            return getSnapshotDbObject(connectionString, snapshotid, dbid, QueryGetSnapshotSequenceObjects);
        }

        #endregion
    }
}
