/******************************************************************
 * Name: DatabasePrincipal.cs
 *
 * Description: Encapsulates database principal object.
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
    class DatabasePrincipal
    {
        #region Fields

        private int m_Id = -1;
        private string m_Name = string.Empty;
        private ObjectType.TypeEnum m_TypeEnum = ObjectType.TypeEnum.Unknown;
        private string m_Owner = string.Empty;
        private string m_Login = string.Empty;
        private bool m_IsAlias = false;
        private string m_AltName = string.Empty;
        private bool m_HasAccess = false;
        private string m_DefaultSchemaName = string.Empty;
        private bool m_isContainedUser = false;
		private string m_authenticationType="";
        #endregion

        #region Queries

        private const string QueryGetSnapshotUsers
                                = @"SELECT 
	                                    snapshotid, 
	                                    dbid, 
	                                    uid, 
	                                    name, 
	                                    owner, 
	                                    login, 
	                                    type, 
	                                    isalias, 
	                                    altname, 
	                                    hasaccess, 
	                                    defaultschemaname ,
                                        iscontaineduser,
                                        AuthenticationType
                                    FROM SQLsecure.dbo.vwdatabaseprincipal
                                    WHERE snapshotid = @snapshotid AND dbid = @dbid AND type IN ('S', 'U', 'G')";
        private const string QueryGetSnapshotUser
                                = @"SELECT 
	                                    snapshotid, 
	                                    dbid, 
	                                    uid, 
	                                    name, 
	                                    owner, 
	                                    login, 
	                                    type, 
	                                    isalias, 
	                                    altname, 
	                                    hasaccess, 
	                                    defaultschemaname,
                                        iscontaineduser,
                                        AuthenticationType
                                    FROM SQLsecure.dbo.vwdatabaseprincipal
                                    WHERE snapshotid = @snapshotid AND dbid = @dbid AND uid = @uid AND type IN ('S', 'U', 'G')";
        private const string QueryGetSnapshotUserRoles
                                = @"SELECT 
	                                    uid, 
	                                    name, 
	                                    type
                                    FROM SQLsecure.dbo.vwdatabaserolemember
                                    WHERE snapshotid = @snapshotid AND dbid = @dbid AND rolememberuid = @rolememberuid";
        private const string QueryGetSnapshotDbRoles
                                = @"SELECT 
	                                    snapshotid, 
	                                    dbid, 
	                                    uid, 
	                                    name, 
	                                    owner, 
	                                    type
                                    FROM SQLsecure.dbo.vwdatabaseprincipal
                                    WHERE snapshotid = @snapshotid AND dbid = @dbid AND type IN ('R', 'A')";
        private const string QueryGetSnapshotDbRole
                                = @"SELECT 
	                                    snapshotid, 
	                                    dbid, 
	                                    uid, 
	                                    name, 
	                                    owner, 
	                                    type
                                    FROM SQLsecure.dbo.vwdatabaseprincipal
                                    WHERE snapshotid = @snapshotid AND dbid = @dbid AND uid = @roleid AND type IN ('R', 'A')";
        private const string QueryGetSnapshotDbRoleByName
                                = @"SELECT 
	                                    snapshotid, 
	                                    dbid, 
	                                    uid, 
	                                    name, 
	                                    owner, 
	                                    type
                                    FROM SQLsecure.dbo.vwdatabaseprincipal
                                    WHERE snapshotid = @snapshotid AND dbid = @dbid AND UPPER(name) = UPPER(@name) AND type IN ('R', 'A')";
        private const string QueryGetSnapshotDbRoleByNameCaseSensitive
                                = @"SELECT 
	                                    snapshotid, 
	                                    dbid, 
	                                    uid, 
	                                    name, 
	                                    owner, 
	                                    type
                                    FROM SQLsecure.dbo.vwdatabaseprincipal
                                    WHERE snapshotid = @snapshotid AND dbid = @dbid AND name = @name AND type IN ('R', 'A')";
        private const string QueryGetSnapshotDbRoleMembers
                                = @"SELECT 
                                        rolememberuid                                     
                                    FROM SQLsecure.dbo.vwdatabaserolemember
                                    WHERE snapshotid = @snapshotid AND dbid = @dbid AND uid = @roleid";
        private enum UserColumns
        {
            SnapshotId = 0,
            DbId,
            Uid,
            Name,
            Owner,
            Login,
            Type,
            IsAlias,
            AltName,
            HasAccess,
            DefaultSchemaName,
            IsContainedUser,
            AuthenticationType
        };
        private enum UserRoleColumns
        {
            Uid,
            Name,
            Type
        };
        private enum RoleColumns
        {
            SnapshotId = 0,
            DbId,
            Uid,
            Name,
            Owner,
            Type
        };
        private enum RoleMemberColumns
        {
            RoleMemberUid = 0
        };
        private const string ParamSnapshotid = "snapshotid";
        private const string ParamDbid = "dbid";
        private const string ParamUid = "uid";
        private const string ParamRoleid = "roleid";
        private const string ParamRoleMemberUid = "rolememberuid";
        private const string ParamName = "name";

        #endregion

        #region Helpers

        private static string yesNo(bool flag)
        {
            return (flag ? "Yes" : "No");
        }

        #endregion

        #region Ctors

        private DatabasePrincipal(
                SqlInt32 id,
                SqlString name,
                SqlString type,
                SqlString owner,
                SqlString login,
                SqlString isalias,
                SqlString altname,
                SqlString hasaccess,
                SqlString defaultschemaname,
                SqlBoolean isContainedeUser,
                SqlString authType
            )
        {
            m_Id = id.IsNull ? -1 : id.Value;
            m_Name = name.IsNull ? string.Empty : name.Value;
            m_TypeEnum = ObjectType.UserType(type);
            m_Owner = owner.IsNull ? string.Empty : owner.Value;
            m_Login = login.IsNull ? string.Empty : login.Value;
            m_IsAlias = Sql.ObjectType.ToBoolean(isalias);
            m_AltName = altname.IsNull ? string.Empty : altname.Value;
            m_HasAccess = Sql.ObjectType.ToBoolean(hasaccess);
            m_DefaultSchemaName = defaultschemaname.IsNull ? string.Empty : defaultschemaname.Value;
            m_isContainedUser = isContainedeUser.Value;
            AuthenticationType = authType.Value;
        }

        private DatabasePrincipal(
                SqlInt32 id,
                SqlString name,
                SqlString type,
                SqlString owner
            )
        {
            m_Id = id.IsNull ? -1 : id.Value;
            m_Name = name.IsNull ? string.Empty : name.Value;
            m_TypeEnum = ObjectType.UserType(type);
            m_Owner = owner.IsNull ? string.Empty : owner.Value;
        }

        private DatabasePrincipal(
                SqlInt32 id,
                SqlString name,
                SqlString type
            )
        {
            m_Id = id.IsNull ? -1 : id.Value;
            m_Name = name.IsNull ? string.Empty : name.Value;
            m_TypeEnum = ObjectType.UserType(type);
        }


        #endregion

        #region Properties

        public int Id
        {
            get { return m_Id; }
        }
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
        public string Owner
        {
            get { return m_Owner; }
        }
        public string Login
        {
            get { return m_Login; }
        }
        public bool IsAlias
        {
            get { return m_IsAlias; }
        }
        public string IsAliasStr
        {
            get { return yesNo(m_IsAlias); }
        }
        public string AltName
        {
            get { return m_AltName; }
        }
        public bool HasAccess
        {
            get { return m_HasAccess; }
        }
        public string HasAccessStr
        {
            get { return yesNo(m_HasAccess); }
        }
        public string DefaultSchemaName
        {
            get { return m_DefaultSchemaName; }
        }

        public bool IsContainedUser
        {
            get { return m_isContainedUser; }
            set { m_isContainedUser = value; }
        }
        public string AuthenticationType
		{ 
			 get { return m_authenticationType; }
             set { m_authenticationType = value; }
		}
        #endregion

        #region Methods

        public static List<DatabasePrincipal> GetSnapshotUsers(
                int snapshotid,
                int dbid
            )
        {
            Debug.Assert(snapshotid != 0);

            // Init return.
            List<DatabasePrincipal> users = new List<DatabasePrincipal>();

            // Open connection to repository and get users.
            using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
            {
                // Open the connection.
                connection.Open();

                // Query the repository for db principals.
                SqlParameter paramSnapshotid = new SqlParameter(ParamSnapshotid, snapshotid);
                SqlParameter paramDbid = new SqlParameter(ParamDbid, dbid);
                using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                            QueryGetSnapshotUsers, new SqlParameter[] { paramSnapshotid, paramDbid }))
                {
                    while (rdr.Read())
                    {
                        // Read the fields.
                        SqlInt32 id = rdr.GetSqlInt32((int)UserColumns.Uid);
                        SqlString name = rdr.GetSqlString((int)UserColumns.Name);
                        SqlString type = rdr.GetSqlString((int)UserColumns.Type);
                        SqlString owner = rdr.GetSqlString((int)UserColumns.Owner);
                        SqlString login = rdr.GetSqlString((int)UserColumns.Login);
                        SqlString isalias = rdr.GetSqlString((int)UserColumns.IsAlias);
                        SqlString altname = rdr.GetSqlString((int)UserColumns.AltName);
                        SqlString hasaccess = rdr.GetSqlString((int)UserColumns.HasAccess);
                        SqlString defaultschemaname = rdr.GetSqlString((int)UserColumns.DefaultSchemaName);
                        SqlBoolean isContainedUser = rdr.GetSqlBoolean((int)UserColumns.IsContainedUser);
                        SqlString AuthenticationType = rdr.GetSqlString((int)UserColumns.AuthenticationType);

                        // Create the user and add to list.
                        DatabasePrincipal user = new DatabasePrincipal(id, name, type, owner, login, isalias, altname, hasaccess, defaultschemaname, isContainedUser,AuthenticationType);
                        users.Add(user);
                    }
                }
            }

            return users;
        }

        public static DatabasePrincipal GetSnapshotUser(
                int snapshotid,
                int dbid,
                int uid
            )
        {
            Debug.Assert(snapshotid != 0);

            // Init return.
            DatabasePrincipal user = null;

            // Open connection to repository and get users.
            using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
            {
                // Open the connection.
                connection.Open();

                // Query the repository for db principals.
                SqlParameter paramSnapshotid = new SqlParameter(ParamSnapshotid, snapshotid);
                SqlParameter paramDbid = new SqlParameter(ParamDbid, dbid);
                SqlParameter paramUid = new SqlParameter(ParamUid, uid);
                using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                            QueryGetSnapshotUser, new SqlParameter[] { paramSnapshotid, paramDbid, paramUid }))
                {
                    if (rdr.Read())
                    {
                        // Read the fields.
                        SqlInt32 id = rdr.GetSqlInt32((int)UserColumns.Uid);
                        SqlString name = rdr.GetSqlString((int)UserColumns.Name);
                        SqlString type = rdr.GetSqlString((int)UserColumns.Type);
                        SqlString owner = rdr.GetSqlString((int)UserColumns.Owner);
                        SqlString login = rdr.GetSqlString((int)UserColumns.Login);
                        SqlString isalias = rdr.GetSqlString((int)UserColumns.IsAlias);
                        SqlString altname = rdr.GetSqlString((int)UserColumns.AltName);
                        SqlString hasaccess = rdr.GetSqlString((int)UserColumns.HasAccess);
                        SqlString defaultschemaname = rdr.GetSqlString((int)UserColumns.DefaultSchemaName);
                        SqlBoolean isContainedUser = rdr.GetSqlBoolean((int)UserColumns.IsContainedUser);
                        SqlString AuthenticationType = rdr.GetSqlString((int)UserColumns.AuthenticationType);
                        // Create the user.
                        user = new DatabasePrincipal(id, name, type, owner, login, isalias, altname, hasaccess, defaultschemaname, isContainedUser, AuthenticationType);
                    }
                }
            }

            return user;
        }

        public static List<DatabasePrincipal> GetSnapshotUserRoles(
                int snapshotid,
                int dbid,
                int uid
            )
        {
            Debug.Assert(snapshotid != 0);

            // Init return.
            List<DatabasePrincipal> roles = new List<DatabasePrincipal>();

            // Open connection to repository and get users.
            using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
            {
                // Open the connection.
                connection.Open();

                // Query the repository for db principals.
                SqlParameter paramSnapshotid = new SqlParameter(ParamSnapshotid, snapshotid);
                SqlParameter paramDbid = new SqlParameter(ParamDbid, dbid);
                SqlParameter paramRolememberid = new SqlParameter(ParamRoleMemberUid, uid);
                using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                            QueryGetSnapshotUserRoles, new SqlParameter[] { paramSnapshotid, paramDbid, paramRolememberid }))
                {
                    while (rdr.Read())
                    {
                        // Read the fields.
                        SqlInt32 id = rdr.GetSqlInt32((int)UserRoleColumns.Uid);
                        SqlString name = rdr.GetSqlString((int)UserRoleColumns.Name);
                        SqlString type = rdr.GetSqlString((int)UserRoleColumns.Type);

                        // Create the role and add to list.
                        DatabasePrincipal role = new DatabasePrincipal(id, name, type);
                        roles.Add(role);
                    }
                }
            }

            return roles;
        }

        public static List<DatabasePrincipal> GetSnapshotDbRoles(
                int snapshotid,
                int dbid
            )
        {
            Debug.Assert(snapshotid != 0);

            // Init return.
            List<DatabasePrincipal> roles = new List<DatabasePrincipal>();

            // Open connection to repository and get roles.
            using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
            {
                // Open the connection.
                connection.Open();

                // Query the repository for db roles.
                SqlParameter paramSnapshotid = new SqlParameter(ParamSnapshotid, snapshotid);
                SqlParameter paramDbid = new SqlParameter(ParamDbid, dbid);
                using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                            QueryGetSnapshotDbRoles, new SqlParameter[] { paramSnapshotid, paramDbid }))
                {
                    while (rdr.Read())
                    {
                        // Read the fields.
                        SqlInt32 id = rdr.GetSqlInt32((int)RoleColumns.Uid);
                        SqlString name = rdr.GetSqlString((int)RoleColumns.Name);
                        SqlString type = rdr.GetSqlString((int)RoleColumns.Type);
                        SqlString owner = rdr.GetSqlString((int)RoleColumns.Owner);

                        // Create the role and add to list.
                        DatabasePrincipal role = new DatabasePrincipal(id, name, type, owner);
                        roles.Add(role);
                    }
                }
            }

            return roles;
        }

        public static DatabasePrincipal GetSnapshotDbRole(
                int snapshotid,
                int dbid,
                int roleid
            )
        {
            Debug.Assert(snapshotid != 0);

            // Init return.
            DatabasePrincipal role = null;

            // Open connection to repository and get role.
            using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
            {
                // Open the connection.
                connection.Open();

                // Query the repository for db role.
                SqlParameter paramSnapshotid = new SqlParameter(ParamSnapshotid, snapshotid);
                SqlParameter paramDbid = new SqlParameter(ParamDbid, dbid);
                SqlParameter paramRoleid = new SqlParameter(ParamRoleid, roleid);
                using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                            QueryGetSnapshotDbRole, new SqlParameter[] { paramSnapshotid, paramDbid, paramRoleid }))
                {
                    if (rdr.Read())
                    {
                        // Read the fields.
                        SqlInt32 id = rdr.GetSqlInt32((int)RoleColumns.Uid);
                        SqlString name = rdr.GetSqlString((int)RoleColumns.Name);
                        SqlString type = rdr.GetSqlString((int)RoleColumns.Type);
                        SqlString owner = rdr.GetSqlString((int)RoleColumns.Owner);

                        // Create the role.
                        role = new DatabasePrincipal(id, name, type, owner);
                    }
                }
            }

            return role;
        }


        public static DatabasePrincipal GetSnapshotDbRole(
                int snapshotid,
                int dbid,
                string rolename,
                bool casesensitive
            )
        {
            Debug.Assert(snapshotid != 0);

            // Init return.
            DatabasePrincipal role = null;

            // Open connection to repository and get roles.
            using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
            {
                // Open the connection.
                connection.Open();

                // Query the repository for db roles.
                SqlParameter paramSnapshotid = new SqlParameter(ParamSnapshotid, snapshotid);
                SqlParameter paramDbid = new SqlParameter(ParamDbid, dbid);
                SqlParameter paramName = new SqlParameter(ParamName, rolename);
                using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                    casesensitive ? QueryGetSnapshotDbRoleByNameCaseSensitive : QueryGetSnapshotDbRoleByName,
                    new SqlParameter[] { paramSnapshotid, paramDbid, paramName }))
                {
                    while (rdr.Read())
                    {
                        // Read the fields.
                        SqlInt32 id = rdr.GetSqlInt32((int)RoleColumns.Uid);
                        SqlString name = rdr.GetSqlString((int)RoleColumns.Name);
                        SqlString type = rdr.GetSqlString((int)RoleColumns.Type);
                        SqlString owner = rdr.GetSqlString((int)RoleColumns.Owner);

                        if (!casesensitive || (casesensitive && name == rolename))
                        {
                            // Create the role and add to list.
                            role = new DatabasePrincipal(id, name, type, owner);
                        }
                    }
                }
            }

            return role;
        }

        public static List<DatabasePrincipal> GetSnapshotDbRoleMembers(
                int snapshotid,
                int dbid,
                int roleid
            )
        {
            Debug.Assert(snapshotid != 0);

            // Init return.
            List<DatabasePrincipal> members = new List<DatabasePrincipal>();

            // Open connection to repository.
            using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
            {
                // Open the connection.
                connection.Open();

                // Query the repository for server roles.
                SqlParameter paramSnapshotid = new SqlParameter(ParamSnapshotid, snapshotid);
                SqlParameter paramDbid = new SqlParameter(ParamDbid, dbid);
                SqlParameter paramRoleid = new SqlParameter(ParamRoleid, roleid);
                using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                            QueryGetSnapshotDbRoleMembers, new SqlParameter[] { paramSnapshotid, paramDbid, paramRoleid }))
                {
                    while (rdr.Read())
                    {
                        // Read the fields.
                        SqlInt32 id = rdr.GetSqlInt32((int)RoleMemberColumns.RoleMemberUid);

                        // Get user/role associated with id.
                        if (!id.IsNull)
                        {
                            DatabasePrincipal dbp = DatabasePrincipal.GetSnapshotUser(snapshotid, dbid, id.Value);
                            if (dbp == null)
                            {
                                dbp = DatabasePrincipal.GetSnapshotDbRole(snapshotid, dbid, id.Value);
                            }
                            if (dbp != null)
                            {
                                members.Add(dbp);
                            }
                        }
                    }
                }
            }

            return members;
        }

        #endregion
    }

    static class DatabasePrincipalPermissions
    {

        #region Queries

        private const string QueryDatabasePrincipalPermissions =
                @"SQLsecure.dbo.isp_sqlsecure_getuserallexplicitpermission";

        private const string ParamSnapshotId = @"@snapshotid";
        private const string ParamDbId = @"@dbid";
        private const string ParamUid = @"@uid";

        #endregion

        #region Methods

        public static DataSet GetDatabasePrincipalPermissions(
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
                    SqlParameter paramDbId = new SqlParameter(ParamDbId, tag.DatabaseId);
                    SqlParameter paramUid = new SqlParameter(ParamUid, tag.ObjectId);

                    // Create command object, and fill the dataset.
                    using (SqlCommand cmd = new SqlCommand(QueryDatabasePrincipalPermissions, connection))
                    {
                        // Set the command object.
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(paramSnapshotId);
                        cmd.Parameters.Add(paramDbId);
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
