/******************************************************************
 * Name: ObjectPermissions.cs
 *
 * Description: This class provides static functions to retrieve
 * object permissions.
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
using System.Diagnostics;

using Idera.SQLsecure.UI.Console.Utility;

namespace Idera.SQLsecure.UI.Console.Sql
{
    static class ObjectPermissions
    {
        #region Fields

        #endregion

        #region Queries

        private const string QueryObjectPermissions =
                @"SQLsecure.dbo.isp_sqlsecure_getobjectpermission";
         private const string QueryOsObjectPermissions =
                @"Select * from SQLsecure.dbo.vwosobjectpermission where snapshotid = @snapshotid and osobjectid = @objectid";

        private const string ParamSnapshotId = @"@snapshotid";
        private const string ParamDbId = @"@databaseid";
        private const string ParamObjectId = @"@objectid";
        private const string ParamClassId = @"@classid";
        private const string ParamPermissionType = @"@permissiontype"; // B - both, E - effective, X - explicit
        private const string ParamIsColumn = @"@iscolumn";
        private const string ParamParentObjectId = @"@parentobjectid";

        #endregion

        #region Helpers

        #endregion

        #region Methods

        public static DataSet GetObjectPermissions (
                ObjectTag tag,
                bool isExplicit
            )
        {
            DataSet ds = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Setup the params.
                    SqlParameter paramSnapshotId = new SqlParameter(ParamSnapshotId, tag.SnapshotId);
                    SqlParameter paramDbId = new SqlParameter(ParamDbId, tag.DatabaseId);
                    SqlParameter paramClassId = new SqlParameter(ParamClassId, tag.ClassId);
                    SqlParameter paramParentObjectId = new SqlParameter(ParamParentObjectId, tag.ParentObjectId);
                    SqlParameter paramObjectId = new SqlParameter(ParamObjectId, tag.ObjectId);
                    SqlParameter paramPermissionType = new SqlParameter(ParamPermissionType, (isExplicit ? "X" : "E"));

                    // Create command object, and fill the dataset.
                    using (SqlCommand cmd = new SqlCommand(QueryObjectPermissions, connection))
                    {
                        // Set the command object.
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                        cmd.Parameters.Add(paramSnapshotId);
                        cmd.Parameters.Add(paramDbId);
                        cmd.Parameters.Add(paramClassId);
                        cmd.Parameters.Add(paramObjectId);
                        cmd.Parameters.Add(paramPermissionType);

                        // Create the data adapter object.
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            // Fill the dataset.
                            ds = new DataSet();
                            da.Fill(ds);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ds != null) { ds.Dispose(); }
                ds = null;

                MsgBox.ShowError(ErrorMsgs.CantGetServerObjectPermissions, ex);
            }

            return ds;
        }

        public static DataSet GetOsObjectPermissions(
                ObjectTag tag
            )
        {
            DataSet ds = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Setup the params.
                    SqlParameter paramSnapshotId = new SqlParameter(ParamSnapshotId, tag.SnapshotId);
                    SqlParameter paramObjectId = new SqlParameter(ParamObjectId, tag.ObjectId);

                    // Create command object, and fill the dataset.
                    using (SqlCommand cmd = new SqlCommand(QueryOsObjectPermissions, connection))
                    {
                        // Set the command object.
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                        cmd.Parameters.Add(paramSnapshotId);
                        cmd.Parameters.Add(paramObjectId);

                        // Create the data adapter object.
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            // Fill the dataset.
                            ds = new DataSet();
                            da.Fill(ds);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ds != null) { ds.Dispose(); }
                ds = null;

                MsgBox.ShowError(ErrorMsgs.CantGetServerOsObjectPermissions, ex);
            }

            return ds;
        }

        #endregion
    }
}
