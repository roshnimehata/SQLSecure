/******************************************************************
 * Name: Logins.cs
 *
 * Description: Encapsulates a SQL Server login.
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

using Idera.SQLsecure.Core.Accounts;

namespace Idera.SQLsecure.UI.Console.Sql
{
    class Login
    {
        #region Fields

        private string m_Name;
        private Sql.ObjectType.TypeEnum m_Type;
        private int m_Id;
        private Sid m_Sid;
        private bool m_ServerAccess;
        private bool m_ServerDeny;
        private bool? m_IsDisabled;
        private bool? m_IsExpirationChecked;
        private bool? m_IsPolicyChecked;
        private bool? m_IsPasswordNull;
        private string m_DefaultDatabase;
        private string m_DefaultLanguage;
        private string m_PasswordStatus;

        #endregion

        #region Helpers

        private static string yesNo(bool? flag)
        {
            return (flag.HasValue ? (flag.Value ? @"Yes" : @"No") : @"N/A");
        }

        #endregion

        #region Queries

        // Get logins saved in a specific snapshot.
        private const string QueryGetSnapshotLogins
                                = @"SELECT 
                                        name, 
                                        type, 
                                        principalid, 
                                        sid,
                                        serveraccess, 
                                        serverdeny, 
                                        disabled, 
                                        isexpirationchecked, 
                                        ispolicychecked,
                                        ispasswordnull,
                                        defaultdatabase,
                                        defaultlanguage,
                                        passwordstatus
                                    FROM SQLsecure.dbo.vwserverprincipal
                                    WHERE snapshotid = @snapshotid";
        private const string QueryGetSnapshotLogin
                                = @"SELECT 
                                        name, 
                                        type, 
                                        principalid, 
                                        sid,
                                        serveraccess, 
                                        serverdeny, 
                                        disabled, 
                                        isexpirationchecked, 
                                        ispolicychecked,
                                        ispasswordnull,
                                        defaultdatabase,
                                        defaultlanguage,
                                        passwordstatus
                                    FROM SQLsecure.dbo.vwserverprincipal
                                    WHERE snapshotid = @snapshotid AND principalid = @principalid";
        private const string QueryGetSnapshotLoginRoles
                                = @"SELECT 
                                        name, 
                                        type, 
                                        principalid 
                                    FROM SQLsecure.dbo.vwfixedserverrolemember
                                    WHERE snapshotid = @snapshotid AND memberprincipalid = @memberprincipalid";
        private const string ParamSnapshotid = "snapshotid";
        private const string ParamPrincipalid = "principalid";
        private const string ParamMemberprincipalid = "memberprincipalid";
        private enum LoginColumns
        {
            Name = 0,
            Type,
            PrincipalId,
            Sid,
            ServerAccess,
            ServerDeny,
            Disabled,
            IsExpirationChecked,
            IsPolicyChecked,
            IsPasswordNull,
            DefaultDatabase,
            DefaultLanguage,
            PasswordStatus
        }
        private enum RoleColumns
        {
            Name = 0,
            Type,
            PrincipalId
        }

        #endregion

        #region Ctors

        public Login(
                SqlString name,
                SqlString type,
                SqlInt32 id,
                SqlBinary sid,
                SqlString serveraccess,
                SqlString serverdeny,
                SqlString disabled,
                SqlString isexpirationchecked,
                SqlString ispolicychecked,
                SqlString ispasswordnull,
                SqlString defaultdatabase,
                SqlString defaultlanguage,
                SqlInt32 passwordStatus
            )
        {
            m_Name = name.Value;
            m_Type = Sql.ObjectType.LoginType(type);
            m_Id = id.Value;
            m_Sid = new Sid(sid.Value);
            m_ServerAccess = Sql.ObjectType.ToBoolean(serveraccess);
            m_ServerDeny = Sql.ObjectType.ToBoolean(serverdeny.Value);
            m_IsDisabled = Sql.ObjectType.ToNullableBoolean(disabled);
            m_IsExpirationChecked = Sql.ObjectType.ToNullableBoolean(isexpirationchecked);
            m_IsPolicyChecked = Sql.ObjectType.ToNullableBoolean(ispolicychecked);
            m_IsPasswordNull = Sql.ObjectType.ToNullableBoolean(ispasswordnull);
            m_DefaultDatabase = defaultdatabase.ToString();
            m_DefaultLanguage = defaultlanguage.ToString();
            m_PasswordStatus = passwordStatus.IsNull ? "N/A" : Utility.DescriptionHelper.GetEnumDescription((PasswordStatus)passwordStatus.Value);
        }

        #endregion

        #region Properties

        public string Name
        {
            get { return m_Name; }
        }
        public ObjectType.TypeEnum Type
        {
            get { return m_Type;  }
        }
        public string TypeStr
        {
            get { return ObjectType.TypeName(m_Type); }
        }
        public int Id
        {
            get { return m_Id; }
        }
        public Sid Sid
        {
            get { return m_Sid; }
        }
        public bool ServerAccess
        {
            get { return m_ServerAccess; }
        }
        public string ServerAccessStr
        {
            get { return yesNo(m_ServerAccess); }
        }
        public bool ServerDeny
        {
            get { return m_ServerDeny; }
        }
        public string ServerDenyStr
        {
            get { return yesNo(m_ServerDeny); }
        }
        public bool? IsDisabled
        {
            get { return m_IsDisabled; }
        }
        public string IsDisabledStr
        {
            get { return yesNo(m_IsDisabled); }
        }
        public bool? IsExpirationChecked
        {
            get { return m_IsExpirationChecked; }
        }
        public string IsExpirationCheckedStr
        {
            get { return yesNo(m_IsExpirationChecked); }
        }
        public bool? IsPolicyChecked
        {
            get { return m_IsPolicyChecked; }
        }
        public string IsPolicyCheckedStr
        {
            get { return yesNo(m_IsPolicyChecked); }
        }
        public bool? IsPasswordNull
        {
            get { return m_IsPasswordNull; }
        }
        public string IsPasswordNullStr
        {
            get { return yesNo(m_IsPasswordNull); }
        }
        public string DefaultDatabase
        {
            get { return m_DefaultDatabase; }
        }
        public string DefaultLanguage
        {
            get { return m_DefaultLanguage; }
        }
        public string PasswordStatus
        {
            get { return m_PasswordStatus; }
        }

        #endregion

        #region Methods

        public static List<Login> GetSnapshotLogins(
                int snapshotid
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

                // Query the repository for filter rules for the server.
                SqlParameter paramSnapshotid = new SqlParameter(ParamSnapshotid, snapshotid);
                using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                            QueryGetSnapshotLogins, new SqlParameter[] { paramSnapshotid }))
                {
                    while (rdr.Read())
                    {
                        // Read the fields.
                        SqlString name = rdr.GetSqlString((int)LoginColumns.Name);
                        SqlString type = rdr.GetSqlString((int)LoginColumns.Type);
                        SqlInt32 id = rdr.GetSqlInt32((int)LoginColumns.PrincipalId);
                        SqlBinary sid = rdr.GetSqlBinary((int)LoginColumns.Sid);
                        SqlString serverAccess = rdr.GetSqlString((int)LoginColumns.ServerAccess);
                        SqlString serverDeny = rdr.GetSqlString((int)LoginColumns.ServerDeny);
                        SqlString disabled = rdr.GetSqlString((int)LoginColumns.Disabled);
                        SqlString isexpirationchecked = rdr.GetSqlString((int)LoginColumns.IsExpirationChecked);
                        SqlString ispolicychecked = rdr.GetSqlString((int)LoginColumns.IsPolicyChecked);
                        SqlString ispasswordnull = rdr.GetSqlString((int)LoginColumns.IsPasswordNull);
                        SqlString defaultdatabase = rdr.GetSqlString((int)LoginColumns.DefaultDatabase);
                        SqlString defaultlanguage = rdr.GetSqlString((int)LoginColumns.DefaultLanguage);
                        SqlInt32 passwordStatus = rdr.GetSqlInt32((int)LoginColumns.PasswordStatus);

                        // Create the login and add to list.
                        Login l = new Login(name, type, id, sid, serverAccess, serverDeny, disabled, isexpirationchecked, ispolicychecked, ispasswordnull, defaultdatabase, defaultlanguage, passwordStatus);
                        logins.Add(l);
                    }
                }
            }

            return logins;
        }

        public static Login GetSnapshotLogin(
                int snapshotid,
                int principalid
            )
        {
            Debug.Assert(snapshotid != 0);

            // Init return.
            Login login = null;

            // Open connection to repository and retrieve rules.
            using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
            {
                // Open the connection.
                connection.Open();

                // Query the repository for filter rules for the server.
                SqlParameter paramSnapshotid = new SqlParameter(ParamSnapshotid, snapshotid);
                SqlParameter paramPrincipalid = new SqlParameter(ParamPrincipalid, principalid);
                using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                            QueryGetSnapshotLogin, new SqlParameter[] { paramSnapshotid, paramPrincipalid }))
                {
                    if (rdr.Read())
                    {
                        // Read the fields.
                        SqlString name = rdr.GetSqlString((int)LoginColumns.Name);
                        SqlString type = rdr.GetSqlString((int)LoginColumns.Type);
                        SqlInt32 id = rdr.GetSqlInt32((int)LoginColumns.PrincipalId);
                        SqlBinary sid = rdr.GetSqlBinary((int)LoginColumns.Sid);
                        SqlString serverAccess = rdr.GetSqlString((int)LoginColumns.ServerAccess);
                        SqlString serverDeny = rdr.GetSqlString((int)LoginColumns.ServerDeny);
                        SqlString disabled = rdr.GetSqlString((int)LoginColumns.Disabled);
                        SqlString isexpirationchecked = rdr.GetSqlString((int)LoginColumns.IsExpirationChecked);
                        SqlString ispolicychecked = rdr.GetSqlString((int)LoginColumns.IsPolicyChecked);
                        SqlString ispasswordnull = rdr.GetSqlString((int)LoginColumns.IsPasswordNull);
                        SqlString defaultdatabase = rdr.GetSqlString((int)LoginColumns.DefaultDatabase);
                        SqlString defaultlanguage = rdr.GetSqlString((int)LoginColumns.DefaultLanguage);
                        SqlInt32 passwordStatus = rdr.GetSqlInt32((int)LoginColumns.PasswordStatus);

                        // Create the login.
                        login = new Login(name, type, id, sid, serverAccess, serverDeny, disabled, isexpirationchecked, ispolicychecked, ispasswordnull, defaultdatabase, defaultlanguage, passwordStatus);
                    }
                }
            }

            return login;
        }

        public static List<ServerRole> GetSnapshotLoginRoles(
                int snapshotid,
                int principalid
            )
        {
            Debug.Assert(snapshotid != 0);

            // Init return.
            List<ServerRole> roles = new List<ServerRole>();

            // Open connection to repository and retrieve rules.
            using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
            {
                // Open the connection.
                connection.Open();

                // Query the repository for filter rules for the server.
                SqlParameter paramSnapshotid = new SqlParameter(ParamSnapshotid, snapshotid);
                SqlParameter paramMemberprincipalid = new SqlParameter(ParamMemberprincipalid, principalid);
                using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                            QueryGetSnapshotLoginRoles, new SqlParameter[] { paramSnapshotid, paramMemberprincipalid}))
                {
                    while(rdr.Read())
                    {
                        // Read the fields.
                        SqlString name = rdr.GetSqlString((int)RoleColumns.Name);
                        SqlString type = rdr.GetSqlString((int)RoleColumns.Type);
                        SqlInt32 id = rdr.GetSqlInt32((int)RoleColumns.PrincipalId);

                        // Create the role and add to list.
                        ServerRole role = new ServerRole(name, type, id);
                        roles.Add(role);
                    }
                }
            }

            return roles;
        }

        #endregion
    }

    class WindowsAccount
    {
        #region Fields
         
        public enum State
        {
            Good,
            Suspect,
            Unknown
        }

        public enum Type
        {
            LocalGroup,
            GlobalGroup,
            UniversalGroup,
            Group,
            User,
            Unknown,
            WellKnownGroup,
            //SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
            AzureADUSer,
            AzureADGroup
        }

        private bool m_IsValid;
        private string m_Domain;
        private string m_Account;
        private Type m_Type;
        private Sid m_Sid;
        private State m_State;

        #endregion

        #region Helpers

        private string typeToString(Type type)
        {
            string ret = string.Empty;
            switch (type)
            {
                case Type.LocalGroup:
                    ret = "Local Group";
                    break;
                case Type.GlobalGroup:
                    ret = "Global Group";
                    break;
                case Type.UniversalGroup:
                    ret =  "Universal Group";
                    break;
                case Type.Group:
                    ret = "Group";
                    break;
                case Type.User:
                    ret =  "User";
                    break;
                case Type.WellKnownGroup:
                    ret = "Well-known Group";
                    break;
                default:
                    //Debug.Assert(false);
                    ret =  "Unknown";
                    break;
            }

            return ret;
        }

        private State stringToState(string state)
        {
            Debug.Assert(!string.IsNullOrEmpty(state));

            State ret = State.Unknown;

            if (string.Compare(state, "G", true) == 0)
            {
                ret = State.Good;
            }
            else if (string.Compare(state, "S", true) == 0)
            {
                ret = State.Suspect;
            }
            else
            {
                Debug.Assert(false);
                ret = State.Unknown;
            }

            return ret;
        }

        private string stateToString(State state)
        {
            string ret = string.Empty;

            switch (state)
            {
                case State.Good:
                    ret = "Good";
                    break;
                case State.Suspect:
                    ret = "Suspect";
                    break;
                default:
                    ret = "Unknown";
                    break;
            }

            return ret;
        }

        #endregion

        #region Queries

        // Get bad state windows accounts.
        private const string QueryProblemAccounts
                                = @"SELECT 
                                        name, 
                                        type, 
                                        sid,
                                        state
                                    FROM SQLsecure.dbo.vwwindowsaccount
                                    WHERE snapshotid = @snapshotid AND state != 'G'";
        private const string QueryProblemOsAccounts
                                = @"SELECT 
                                        name, 
                                        type, 
                                        sid,
                                        state
                                    FROM SQLsecure.dbo.vwoswindowsaccount
                                    WHERE snapshotid = @snapshotid AND state != 'G'";

        private const string ParamSnapshotid = "snapshotid";

        private enum Columns
        {
            Name = 0,
            Type,
            Sid,
            State
        }

        #endregion

        #region Ctors

        private WindowsAccount(
                SqlString name,
                SqlString type,
                SqlBinary sid,
                SqlString state
            )
        {
            Debug.Assert(!name.IsNull);
            Debug.Assert(!type.IsNull);
            Debug.Assert(!sid.IsNull);
            Debug.Assert(!state.IsNull);

            // Check if the inputs are valid.
            m_IsValid = !name.IsNull && !type.IsNull && !sid.IsNull && !state.IsNull
                            && !string.IsNullOrEmpty(name.Value) && !string.IsNullOrEmpty(type.Value)
                                && !string.IsNullOrEmpty(state.Value);

            // Parse the name into domain & account.
            Path.SplitSamPath(name.Value, out m_Domain, out m_Account);

            // Determine the type of object.
            m_Type = StringToType(type.Value);

            // Construct the SID.
            m_Sid = new Sid(sid.Value);

            // Determine the state.
            m_State = stringToState(state.Value);
        }

        #endregion

        #region Properties

        public bool IsValid
        {
            get { return m_IsValid; }
        }
        public string Domain
        {
            get { return m_Domain; }
        }
        public string Account
        {
            get { return m_Account; }
        }
        public Type AccountType
        {
            get { return m_Type; }
        }
        public string AccountTypeString
        {
            get { return typeToString(m_Type); }
        }
        public Sid SID
        {
            get { return m_Sid; }
        }
        public State AccountState
        {
            get { return m_State; }
        }
        public string AccountStateString
        {
            get { return stateToString(m_State); }
        }

        #endregion

        #region Methods

        public static List<WindowsAccount> GetSuspectAccounts(
                int snapshotid
            )
        {
            Debug.Assert(snapshotid != 0);

            // Create return list.
            List<WindowsAccount> list = new List<WindowsAccount>();

            // Open connection to repository and retrieve windows accounts.
            try
            {
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Query the repository for suspect windows accounts.
                    SqlParameter paramSnapshotid = new SqlParameter(ParamSnapshotid, snapshotid);
                    using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                QueryProblemAccounts, new SqlParameter[] { paramSnapshotid }))
                    {
                        while (rdr.Read())
                        {
                            // Read the fields.
                            SqlString name = rdr.GetSqlString((int)Columns.Name);
                            SqlString type = rdr.GetSqlString((int)Columns.Type);
                            SqlBinary sid = rdr.GetSqlBinary((int)Columns.Sid);
                            SqlString state = rdr.GetSqlString((int)Columns.State);

                            // Create the account and add to list.
                            WindowsAccount wa = new WindowsAccount(name, type, sid, state);
                            list.Add(wa);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.UnresolvedWindowsAccountsCaption,
                                            Utility.ErrorMsgs.CantGetUnresolvedWindowsAccounts, ex);
                list = null;
            }

            return list;
        }

        public static List<WindowsAccount> GetSuspectOsAccounts(
                int snapshotid
            )
        {
            Debug.Assert(snapshotid != 0);

            // Create return list.
            List<WindowsAccount> list = new List<WindowsAccount>();

            // Open connection to repository and retrieve windows accounts.
            try
            {
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Query the repository for suspect windows accounts.
                    SqlParameter paramSnapshotid = new SqlParameter(ParamSnapshotid, snapshotid);
                    using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                QueryProblemOsAccounts, new SqlParameter[] { paramSnapshotid }))
                    {
                        while (rdr.Read())
                        {
                            // Read the fields.
                            SqlString name = rdr.GetSqlString((int)Columns.Name);
                            SqlString type = rdr.GetSqlString((int)Columns.Type);
                            SqlBinary sid = rdr.GetSqlBinary((int)Columns.Sid);
                            SqlString state = rdr.GetSqlString((int)Columns.State);

                            // Create the account and add to list.
                            WindowsAccount wa = new WindowsAccount(name, type, sid, state);
                            list.Add(wa);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.UnresolvedWindowsAccountsCaption,
                                            Utility.ErrorMsgs.CantGetUnresolvedWindowsAccounts, ex);
                list = null;
            }

            return list;
        }

        public static Type StringToType(string type)
        {
            Debug.Assert(!string.IsNullOrEmpty(type));

            Type ret = Type.Unknown;
            if(string.Compare(type, "LocalGroup", true) == 0)
            {
                ret = Type.LocalGroup;
            }
            else if(string.Compare(type, "GlobalGroup", true) == 0)
            {
                ret = Type.GlobalGroup;
            }
            else if(string.Compare(type, "UniversalGroup", true) == 0)
            {
                ret = Type.UniversalGroup;
            }
            else if(string.Compare(type, "User", true) == 0)
            {
                ret = Type.User;
            }
            else if(string.Compare(type, "Group", true) == 0)
            {
                ret = Type.Group;
            }
            else if (string.Compare(type, "WellKnownGroup", true) == 0)
            {
                ret = Type.WellKnownGroup;
            }
            else
            {
                //Debug.Assert(false);
                ret = Type.Unknown;
            }

            return ret;
        }

        #endregion
    }
}
