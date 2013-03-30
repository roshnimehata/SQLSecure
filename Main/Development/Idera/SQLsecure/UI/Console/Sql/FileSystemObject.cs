/******************************************************************
 * Name: FileSystemObject.cs
 *
 * Description: Encapsulates a File System Object that is not a registry key
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
using Idera.SQLsecure.UI.Console.Utility;

namespace Idera.SQLsecure.UI.Console.Sql
{
    class FileSystemObject
    {
        #region Fields

        private int m_Id;
        private string m_Type;
        private int? m_DbId;
        private string m_Database;
        private string m_Name;
        private string m_LongName;
        private Sid m_OwnerSid;
        private string m_OwnerName;
        private string m_DiskType;

        #endregion

        #region Helpers

        #endregion

        #region Queries

        // Get objects saved in a specific snapshot.
        private const string QueryGetSnapshotOsObjects
                                = @"SELECT
                                        osobjectid,
                                        objecttype,
                                        dbid,
                                        databasename,
                                        objectname,
                                        longname,
                                        ownersid,
                                        ownername,
                                        disktype
                                    FROM SQLsecure.dbo.vwfilesystemobject
                                    WHERE snapshotid = @snapshotid";
        private const string QueryGetSnapshotOsObject
                                = @"SELECT
                                        osobjectid,
                                        objecttype,
                                        dbid,
                                        databasename,
                                        objectname,
                                        longname,
                                        ownersid,
                                        ownername,
                                        disktype
                                    FROM SQLsecure.dbo.vwfilesystemobject
                                    WHERE snapshotid = @snapshotid AND osobjectid = @osobjectid";
        private const string ParamSnapshotid = "snapshotid";
        private const string ParamObjectId = "osobjectid";
        private enum ObjectColumns
        {
            Id = 0,
            Type,
            DbId,
            DbName,
            Name,
            LongName,
            OwnerSid,
            OwnerName,
            DiskType
        }

        #endregion

        #region Ctors

        public FileSystemObject(
                SqlInt32 osObjectId,
                SqlString type,
                SqlInt32 dbId,
                SqlString databaseName,
                SqlString name,
                SqlString longName,
                SqlBytes ownerSid,
                SqlString ownerName,
                SqlString diskType
            )
        {
            m_Id = osObjectId.Value;
            m_Type = type.Value;
            m_DbId = dbId.IsNull ? (int?)null : dbId.Value;
            m_Database = databaseName.Value;
            m_Name = name.Value;
            m_LongName = longName.Value;
            m_OwnerSid = new Sid(ownerSid.Value);
            m_OwnerName = ownerName.Value;
            m_DiskType = diskType.Value;
        }

        #endregion

        #region Properties

        public int OsObjectId
        {
            get { return m_Id; }
        }

        public string ObjectType
        {
            get { return m_Type; }
        }

        public string TypeDescription
        {
            get
            {
                switch (m_Type)
                {
                    case OsObjectType.DB:
                        return DescriptionHelper.GetDescription(typeof(OsObjectType), "DB");
                    case OsObjectType.Disk:
                        return DescriptionHelper.GetDescription(typeof(OsObjectType), "Disk");
                    case OsObjectType.File:
                        return DescriptionHelper.GetDescription(typeof(OsObjectType), "File");
                    case OsObjectType.FileDirectory:
                        return DescriptionHelper.GetDescription(typeof(OsObjectType), "FileDirectory");
                    case OsObjectType.InstallDirectory:
                        return DescriptionHelper.GetDescription(typeof(OsObjectType), "InstallDirectory");
                }
                return DescriptionHelper.GetDescription(typeof(OsObjectType), "Unknown");
           }
        }

        public string Database
        {
            get { return m_Database; }
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

        public string DiskType
        {
            get { return m_DiskType; }
        }

        #endregion

        #region Methods

        public static List<FileSystemObject> GetSnapshotObjects(
                int snapshotid
            )
        {
            Debug.Assert(snapshotid != 0);

            // Init return.
            List<FileSystemObject> fsobjects = new List<FileSystemObject>();

            // Open connection to repository and retrieve objects.
            using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
            {
                // Open the connection.
                connection.Open();

                // Query the repository for file system objects for the server.
                SqlParameter paramSnapshotid = new SqlParameter(ParamSnapshotid, snapshotid);
                using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                            QueryGetSnapshotOsObjects, new SqlParameter[] { paramSnapshotid }))
                {
                    while (rdr.Read())
                    {
                        // Read the fields.
                        SqlInt32 id = rdr.GetSqlInt32((int)ObjectColumns.Id);
                        SqlString type = rdr.GetSqlString((int)ObjectColumns.Type);
                        SqlInt32 dbid = rdr.GetSqlInt32((int)ObjectColumns.DbId);
                        SqlString databasename = rdr.GetSqlString((int)ObjectColumns.DbName);
                        SqlString name = rdr.GetSqlString((int)ObjectColumns.Name);
                        SqlString longname = rdr.GetSqlString((int)ObjectColumns.LongName);
                        SqlBytes ownerSid = rdr.GetSqlBytes((int)ObjectColumns.OwnerSid);
                        SqlString ownerName = rdr.GetSqlString((int)ObjectColumns.OwnerName);
                        SqlString disktype = rdr.GetSqlString((int)ObjectColumns.DiskType);

                        // Create the object and add to list.
                        FileSystemObject f = new FileSystemObject(id, type, dbid, databasename, name, longname, ownerSid, ownerName, disktype);
                        fsobjects.Add(f);
                    }
                }
            }

            return fsobjects;
        }

        public static FileSystemObject GetSnapshotObject(
                int snapshotid,
                int objectId
            )
        {
            Debug.Assert(snapshotid != 0);

            // Init return.
            FileSystemObject fsobject = null;

            // Open connection to repository and retrieve rules.
            using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
            {
                // Open the connection.
                connection.Open();

                // Query the repository for file system objects for the server.
                SqlParameter paramSnapshotid = new SqlParameter(ParamSnapshotid, snapshotid);
                SqlParameter paramObjectId = new SqlParameter(ParamObjectId, objectId);
                using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                            QueryGetSnapshotOsObject, new SqlParameter[] { paramSnapshotid, paramObjectId }))
                {
                    if (rdr.Read())
                    {
                        // Read the fields.
                        SqlInt32 id = rdr.GetSqlInt32((int)ObjectColumns.Id);
                        SqlString type = rdr.GetSqlString((int)ObjectColumns.Type);
                        SqlInt32 dbid = rdr.GetSqlInt32((int)ObjectColumns.DbId);
                        SqlString databasename = rdr.GetSqlString((int)ObjectColumns.DbName);
                        SqlString name = rdr.GetSqlString((int)ObjectColumns.Name);
                        SqlString longname = rdr.GetSqlString((int)ObjectColumns.LongName);
                        SqlBytes ownerSid = rdr.GetSqlBytes((int)ObjectColumns.OwnerSid);
                        SqlString ownerName = rdr.GetSqlString((int)ObjectColumns.OwnerName);
                        SqlString disktype = rdr.GetSqlString((int)ObjectColumns.DiskType);

                        // Create the object.
                        fsobject = new FileSystemObject(id, type, dbid, databasename, name, longname, ownerSid, ownerName, disktype);
                    }
                }
            }

            return fsobject;
        }

        #endregion
    }
}
