/******************************************************************
 * Name: ServerRoles.cs
 *
 * Description: Encapsulates SQL server role objects.
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
    class ServerRole
    {
        #region Fields

        private string m_Name;
        private ObjectType.TypeEnum m_TypeEnum = ObjectType.TypeEnum.Unknown;
        private int m_Id;

        #endregion

        #region Queries

        // Get server roles saved in a specific snapshot.
        private const string QueryGetSnapshotServerRoles
                                = @"SELECT 
                                        name, 
                                        type, 
                                        principalid
                                    FROM SQLsecure.dbo.vwfixedserverrole
                                    WHERE snapshotid = @snapshotid";
        private const string QueryGetSnapshotServerRoleMembers
                                = @"SELECT 
                                        memberprincipalid
                                    FROM SQLsecure.dbo.vwfixedserverrolemember
                                    WHERE snapshotid = @snapshotid AND principalid = @principalid";
        private const string ParamSnapshotid = "snapshotid";
        private const string ParamPrincipalid = "principalid";
        private enum SnapshotServerRolesColumns
        {
            Name = 0,
            Type,
            PrincipalId
        };
        private enum SnapshotServerRoleMemberColumns
        {
            MemberPrincipalId = 0
        }

        #endregion

        #region Ctors

        public ServerRole(
                string name,
                string type,
                int id
            )
        {
            m_Name = name;
            m_TypeEnum = ObjectType.LoginType(type);
            m_Id = id;
        }

        public ServerRole(
                SqlString name,
                SqlString type,
                SqlInt32 id
            )
        {
            m_Name = name.Value;
            m_TypeEnum = ObjectType.LoginType(type.Value);
            m_Id = id.Value;
        }

        #endregion

        #region Properties

        public string Name
        {
            get { return m_Name; }
        }
        public ObjectType.TypeEnum TypeEnum
        {
            get { return m_TypeEnum; }
        }
        public string TypeStr
        {
            get { return ObjectType.TypeName(m_TypeEnum); }
        }
        public int Id
        {
            get { return m_Id; }
        }

        #endregion

        #region Methods

        public static List<ServerRole> GetSnapshotServerRoles(
                int snapshotid
            )
        {
            Debug.Assert(snapshotid != 0);

            // Init return.
            List<ServerRole> serverRoles = new List<ServerRole>();

            // Open connection to repository and retrieve rules.
            using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
            {
                // Open the connection.
                connection.Open();

                // Query the repository for server roles.
                SqlParameter paramSnapshotid = new SqlParameter(ParamSnapshotid, snapshotid);
                using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                            QueryGetSnapshotServerRoles, new SqlParameter[] { paramSnapshotid }))
                {
                    while (rdr.Read())
                    {
                        // Read the fields.
                        SqlString name = rdr.GetSqlString((int)SnapshotServerRolesColumns.Name);
                        SqlString type = rdr.GetSqlString((int)SnapshotServerRolesColumns.Type);
                        SqlInt32 id = rdr.GetSqlInt32((int)SnapshotServerRolesColumns.PrincipalId);

                        // Create the server role and add to list.
                        ServerRole r = new ServerRole(name, type, id);
                        serverRoles.Add(r);
                    }
                }
            }

            return serverRoles;
        }

        public static List<Login> GetSnapshotServerRoleMembers(
                int snapshotid,
                int roleid
            )
        {
            Debug.Assert(snapshotid != 0);

            // Init return.
            List<Login> logins = new List<Login>();

            // Open connection to repository and retrieve rules.
            using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
            {
                // Open the connection.
                connection.Open();

                // Query the repository for server roles.
                SqlParameter paramSnapshotid = new SqlParameter(ParamSnapshotid, snapshotid);
                SqlParameter paramPrincipalid = new SqlParameter(ParamPrincipalid, roleid);
                using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                            QueryGetSnapshotServerRoleMembers, new SqlParameter[] { paramSnapshotid, paramPrincipalid }))
                {
                    while (rdr.Read())
                    {
                        // Read the fields.
                        SqlInt32 id = rdr.GetSqlInt32((int)SnapshotServerRoleMemberColumns.MemberPrincipalId);

                        // Get login associated with id.
                        if (!id.IsNull)
                        {
                            Login login = Login.GetSnapshotLogin(snapshotid, id.Value);
                            if (login != null)
                            {
                                logins.Add(login);
                            }
                        }
                    }
                }
            }

            return logins;
        }

        #endregion
    }

    static class ServerRolePermissions
    {

        #region Queries

        private const string QueryServerPrincipalPermissions =
                @"SQLsecure.dbo.isp_sqlsecure_getserverprincipalpermission";

        private const string ParamSnapshotId = @"@snapshotid";
        private const string ParamUid = @"@uid";

        #endregion

        #region Methods

        public static DataSet GetServerRolePermissions(
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
                    SqlParameter paramUid = new SqlParameter(ParamUid, tag.ObjectId);

                    // Create command object, and fill the dataset.
                    using (SqlCommand cmd = new SqlCommand(QueryServerPrincipalPermissions, connection))
                    {
                        // Set the command object.
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(paramSnapshotId);
                        cmd.Parameters.Add(paramUid);

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

                MsgBox.ShowError(ErrorMsgs.CantGetDbPrincipalPermissions, ex);
            }

            return ds;
        }

        #endregion
    }
}
