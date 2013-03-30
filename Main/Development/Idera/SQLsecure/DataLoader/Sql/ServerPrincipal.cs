/******************************************************************
 * Name: ServerPrincipal.cs
 *
 * Description: Encapsulates the SQL Server Server Principal (aka Login)
 * object.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;
using Idera.SQLsecure.Core.Accounts;
using Idera.SQLsecure.Core.Logger;

namespace Idera.SQLsecure.Collector.Sql
{
    /// <summary>
    /// SQL Server server principal (aka Login).
    /// </summary>
    internal static class ServerPrincipal
    {
        #region Types

        private class Sql2000FixedServerRoles
        {
            private SqlString m_Name;
            private SqlString m_Type;
            private byte[] m_Sid;
            private int m_PrincipalId;

            public Sql2000FixedServerRoles(
                    string name,
                    string type,
                    byte[] sid,
                    int principalid
                )
            {
                m_Name = name;
                m_Type = type;
                m_Sid = sid;
                m_PrincipalId = principalid;
            }

            public SqlString Name { get { return m_Name; } }
            public SqlString Type { get { return m_Type; } }
            public byte[] Sid { get { return m_Sid; } }
            public int PrincipalId { get { return m_PrincipalId; } }
        }

        #endregion
        private static LogX logX = new LogX("Idera.SQLsecure.Collector.Sql.ServerPrincipal");

        private const int FieldPrincipalName = 0;
        private const int FieldPrincipalPid = 1;
        private const int FieldPrincipalType = 2;
        private const int FieldPrincipalSid = 3;
        private const int FieldPrincipalServeraccess = 4;
        private const int FieldPrincipalServerdeny = 5;
        private const int FieldPrincipalIsdisabled = 6;
        private const int FieldPrincipalIspolicychecked = 7;
        private const int FieldPrincipalIsexpirationchecked = 8;
        private const int FieldPrincipalIsPasswordNull = 9;
        private const int FieldPrincipalDefautDatabase = 10;
        private const int FieldPrincipalDefautLanguage = 11;
        private const int PasswordHash = 12;

        public const string PasswordSameAsLoginName = "<UserName>";
        public const string PasswordSameAsLoginNameReverse = ">emaNresU<";
        public const int MaximumWordPlusNumberTotal = 20000;

        public static List<string> defaultPasswordList = new List<string>();

        private static Sql2000FixedServerRoles[] FixedServerRoles2000 = new Sql2000FixedServerRoles [] {
                                                                            new Sql2000FixedServerRoles("sysadmin","R",new byte[] {3}, 3),
                                                                            new Sql2000FixedServerRoles("securityadmin", "R", new byte[] { 4 }, 4),
                                                                            new Sql2000FixedServerRoles("serveradmin", "R", new byte[] { 5 }, 5),
                                                                            new Sql2000FixedServerRoles("setupadmin", "R", new byte[] { 6 },  6),
                                                                            new Sql2000FixedServerRoles("processadmin", "R", new byte[] { 7 }, 7),
                                                                            new Sql2000FixedServerRoles("diskadmin", "R", new byte[] { 8 }, 8),
                                                                            new Sql2000FixedServerRoles("dbcreator", "R", new byte[] { 9 }, 9),
                                                                            new Sql2000FixedServerRoles("bulkadmin", "R", new byte[] { 10 }, 10)
                                                                        };

        private static string createPrincipalQuery(
                ServerVersion version
            )
        {
            Debug.Assert(version != ServerVersion.Unsupported);

            string query = null;

            // Create query based on the SQL Server version.
            if (version == ServerVersion.SQL2000)
            {
                query = @"SELECT DISTINCT 
                            name, 
                            principalid = 0, 
                            type = CASE 
                                      WHEN isntname = 0 THEN 'S' 
                                      WHEN isntname = 1 AND isntuser = 1 THEN 'U'
                                      WHEN isntname = 1 AND isntgroup = 1 THEN 'G' 
                                   END, 
                            sid,    
                            serveraccess = CASE 
                                              WHEN hasaccess = 1 THEN 'Y' 
                                              ELSE 'N' 
                                           END,
                            serverdeny = CASE 
                                            WHEN denylogin = 1 THEN 'Y' 
                                            ELSE 'N' 
                                         END,
                            isdisabled = ' ', 
                            ispolicychecked = ' ',
                            isexpirationchecked = ' ',
                            ispasswordblank = CASE WHEN isntname = 0 
                                                THEN 
                                                    CASE WHEN password is null 
                                                        THEN 'Y' 
                                                        ELSE 
                                                             CASE WHEN pwdcompare('', password) = 1  
															      THEN 'Y' 
															      ELSE 'N' 
															 END	
                                                    END
                                                ELSE null
                                              END, 
                            defaultdatabase = dbname,
                            defaultlanguage = language,
                            cast([password] as varbinary)
                          FROM master.dbo.syslogins";

            }
            else
            {
                query = @"SELECT DISTINCT 
                            Principals.name, 
                            principalid = Principals.principal_id, 
                            Principals.type, 
                            Principals.sid, 
                            serveraccess = CASE Permissions.state 
                                              WHEN 'G' THEN 'Y' 
                                              ELSE 'N' 
                                           END,
                            serverdeny = CASE Permissions.state 
                                            WHEN 'D' THEN 'Y' 
                                            ELSE 'N' 
                                         END, 
                            isdisabled = CASE Principals.is_disabled 
                                            WHEN 1 THEN 'Y' 
                                            ELSE 'N' 
                                         END, 
                            ispolicychecked = CASE SqlLogins.is_policy_checked
                                                 WHEN 1 THEN 'Y' 
                                                 ELSE 'N' 
                                              END, 
                            isexpirationchecked = CASE SqlLogins.is_expiration_checked 
                                                     WHEN 1 THEN 'Y' 
                                                     ELSE 'N' 
                                                  END, 
                            ispasswordblank = CASE WHEN Principals.type = 'S'
                                                    THEN 
                                                        CASE WHEN pwdcompare('', SqlLogins.password_hash) = 1  
                                                             THEN 'Y' 
                                                             ELSE 'N'                                                                   
                                                        END
                                                    ELSE null                        
                                              END,
                            defaultdatabase = Principals.default_database_name,
                            defaultlanguage =  Principals.default_language_name,
                            SqlLogins.password_hash
                          FROM sys.server_principals Principals LEFT OUTER JOIN sys.sql_logins SqlLogins ON 
                                    Principals.principal_id = SqlLogins.principal_id 
                                LEFT OUTER JOIN sys.server_permissions Permissions 
                                    ON Permissions.grantee_principal_id = Principals.principal_id AND Permissions.type = N'COSQ'";
            }

            return query;
        }

        private static string createRoleMemberQuery(
                ServerVersion version
            )
        {
            Debug.Assert(version != ServerVersion.Unsupported);

            string query = null;

            // Create query based on the SQL Server version.
            if (version == ServerVersion.SQL2000)
            {
                query = "EXEC sp_helpsrvrolemember";
            }
            else
            {
                query = @"SELECT * FROM sys.server_role_members";
            }

            return query;
        }

        private static bool processMembers(
                ServerVersion version,
                string targetConnection,
                string repositoryConnection,
                int snapshotid,
                Dictionary<string, int> nameDictionary
            )
        {
            Debug.Assert(version != ServerVersion.Unsupported);
            Debug.Assert(!string.IsNullOrEmpty(targetConnection));
            Debug.Assert(!string.IsNullOrEmpty(repositoryConnection));
            Debug.Assert(nameDictionary != null);
            Program.ImpersonationContext wi = Program.SetLocalImpersonationContext();
            // Process database role members.
            bool isOk = true;
            using (SqlConnection target = new SqlConnection(targetConnection),
                    repository = new SqlConnection(repositoryConnection))
            {
                try
                {
                    // Open repository and target connections.
                    repository.Open();
                    Program.SetTargetSQLServerImpersonationContext();
                    target.Open();

                    // Use bulk copy object to write to repository.
                    using (SqlBulkCopy bcp = new SqlBulkCopy(repository))
                    {
                        // Set the destination table.
                        bcp.DestinationTableName = ServerRoleMemberDataTable.RepositoryTable;
                        bcp.BulkCopyTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                        // Create the datatable to write to the repository.
                        using (DataTable dataTable = ServerRoleMemberDataTable.Create())
                        {
                            // Create the query.
                            string query = createRoleMemberQuery(version);
                            Debug.Assert(!string.IsNullOrEmpty(query));

                            // Query to get the table objects.
                            using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(target, null,
                                                CommandType.Text, query, null))
                            {
                                while (rdr.Read())
                                {
                                    // Retrieve and setup the table row.
                                    if (version == ServerVersion.SQL2000)
                                    {
                                        int role, member;
                                        if (nameDictionary.TryGetValue((string)rdr[0], out role)
                                            && nameDictionary.TryGetValue((string)rdr[1], out member))
                                        {
                                            DataRow dr = dataTable.NewRow();
                                            dr[ServerRoleMemberDataTable.ParamSnapshotid] = snapshotid;
                                            dr[ServerRoleMemberDataTable.ParamPrincipalid] = role;
                                            dr[ServerRoleMemberDataTable.ParamMemberprincipalid] = member;
                                            dr[DatabaseRoleMemberDataTable.ParamHashkey] = "";
                                            dataTable.Rows.Add(dr);
                                        }
                                        else
                                        {
                                            logX.loggerX.Warn("WARN - uid not found for ", (string)rdr[0], ", or ", (string)rdr[1]);
                                        }
                                    }
                                    else
                                    {
                                        DataRow dr = dataTable.NewRow();
                                        dr[ServerRoleMemberDataTable.ParamSnapshotid] = snapshotid;
                                        dr[ServerRoleMemberDataTable.ParamPrincipalid] = rdr.GetSqlInt32(0);
                                        dr[ServerRoleMemberDataTable.ParamMemberprincipalid] = rdr.GetSqlInt32(1);
                                        dr[DatabaseRoleMemberDataTable.ParamHashkey] = "";
                                        dataTable.Rows.Add(dr);
                                    }

                                    // Write to repository if exceeds threshold.
                                    if (dataTable.Rows.Count > Constants.RowBatchSize)
                                    {
                                        bcp.WriteToServer(dataTable);
                                        dataTable.Clear();
                                    }
                                }

                                // Write any items still in the data table.
                                if (dataTable.Rows.Count > 0)
                                {
                                    bcp.WriteToServer(dataTable);
                                    dataTable.Clear();
                                }
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    string strMessage = "Processing server role members";
                    logX.loggerX.Error("ERROR - " + strMessage, ex);
                    Sql.Database.CreateApplicationActivityEventInRepository(repositoryConnection, 
                                                                            snapshotid, 
                                                                            Collector.Constants.ActivityType_Error, 
                                                                            Collector.Constants.ActivityEvent_Error, 
                                                                            strMessage + ex.Message);
                    AppLog.WriteAppEventError(SQLsecureEvent.ExErrExceptionRaised, SQLsecureCat.DlDataLoadCat,
                        " SQL Server = " + new SqlConnectionStringBuilder(targetConnection).DataSource +
                        strMessage, ex.Message);


                    isOk = false;
                }
                finally
                {
                    Program.RestoreImpersonationContext(wi);
                }
            }

            return isOk;
        }

        public static bool Process(
                ServerVersion version,
                string targetConnection,
                string repositoryConnection,
                int snapshotid,
                out List<Account> users,
                out List<Account> windowsGroupLogins
            )
        {
            Debug.Assert(version != ServerVersion.Unsupported);
            Debug.Assert(!string.IsNullOrEmpty(targetConnection));
            Debug.Assert(!string.IsNullOrEmpty(repositoryConnection));

            // Init return.
            bool isOk = true;
            users = new List<Account>();
            windowsGroupLogins = new List<Account>();
            Program.ImpersonationContext wi = Program.SetLocalImpersonationContext();

            // Process logins.
            List<int> pidList = new List<int>();
            Dictionary<string, int> nameDictionary = new Dictionary<string, int>();
            using (SqlConnection target = new SqlConnection(targetConnection),
                    repository = new SqlConnection(repositoryConnection))
            {
                try
                {
                    repository.Open();
                    // Open repository and target connections.
                    Program.SetTargetSQLServerImpersonationContext();
                    target.Open();

                    //load the custom password settings. 
                    List<WeakPasswordSetting> passwordSettings = WeakPasswordSetting.GetWeakPasswordSettings(repository);

                    // Use bulk copy object to write to repository.
                    using (SqlBulkCopy bcp = new SqlBulkCopy(repository))
                    {
                        // Set the destination table.
                        bcp.DestinationTableName = ServerPrincipalDataTable.RepositoryTable;
                        bcp.BulkCopyTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                        // Create the datatable to write to the repository.
                        using (DataTable dataTable = ServerPrincipalDataTable.Create())
                        {
                            // Create the query.
                            string query = createPrincipalQuery(version);
                            Debug.Assert(!string.IsNullOrEmpty(query));

                            // Query to get the table objects.
                            using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(target, null,
                                                CommandType.Text, query, null))
                            {
                                int pid = Constants.Sql2000PidStart; // Generated PID for SQL 2000, starts at 100.
                                while (rdr.Read())
                                {
                                    // Retrieve information.
                                    SqlString name = rdr.GetSqlString(FieldPrincipalName);
                                    SqlString type = rdr.GetSqlString(FieldPrincipalType);
                                    SqlBinary sid = rdr.GetSqlBinary(FieldPrincipalSid);
                                    SqlInt32 principalid = rdr.GetSqlInt32(FieldPrincipalPid);
                                    SqlString serveraccess = rdr.GetSqlString(FieldPrincipalServeraccess);
                                    SqlString serverdeny = rdr.GetSqlString(FieldPrincipalServerdeny);
                                    SqlString isdisabled = rdr.GetSqlString(FieldPrincipalIsdisabled);
                                    SqlString isexpirationchecked = rdr.GetSqlString(FieldPrincipalIsexpirationchecked);
                                    SqlString ispolicychecked = rdr.GetSqlString(FieldPrincipalIspolicychecked);
                                    SqlString ispasswordnull = rdr.GetSqlString(FieldPrincipalIsPasswordNull);
                                    SqlString defaultdatabase = rdr.GetSqlString(FieldPrincipalDefautDatabase);
                                    SqlString defaultlanguage = rdr.GetSqlString(FieldPrincipalDefautLanguage);
                                    SqlInt32 passwordStatus = SqlInt32.Null; 

                                    //only calculate the password status for SQL Logins
                                    if (type.CompareTo(Constants.SQLLogin) == 0)
                                    {
                                        //This tells us if the password is blank.
                                        if (ispasswordnull.Value == "Y")
                                        {
                                            passwordStatus = (int)PasswordStatus.Blank;
                                        }
                                        else
                                        {
                                            if (passwordSettings.Count > 0)
                                            {
                                                if (passwordSettings[0].PasswordCheckingEnabled)
                                                {
                                                    passwordStatus = (int)DetectPasswordStatus(name.ToString(), ReadBinaryField(rdr, PasswordHash), passwordSettings[0], version);
                                                }
                                            }
                                        }
                                    }

                                    // If version is SQL 2000 generate principal id.  This makes the 
                                    // data consistent with SQL 2005.  Then add to the dictionary
                                    // for later role membership processing.
                                    if (version == ServerVersion.SQL2000)
                                    {
                                        principalid = pid++;
                                        Debug.Assert(!name.IsNull && !principalid.IsNull);
                                        nameDictionary.Add(name.Value, principalid.Value);
                                    }
                                    else // Add the pid to the pid list for later permission processing.
                                    {    // this only applies to SQL 2005.   No server level permissions in SQL 2000.
                                        Debug.Assert(!principalid.IsNull);
                                        pidList.Add(principalid.Value);
                                    }

                                    // If the principal is a windows user, add the user
                                    // to the user list.
                                    Debug.Assert(!type.IsNull);
                                    if (type.CompareTo(Constants.WindowsUser) == 0)
                                    {
                                        Debug.Assert(!sid.IsNull && !name.IsNull);
                                        Account user = null;
                                        if (Account.CreateUserAccount(new Sid(sid.Value), name.Value, out user))
                                        {
                                            users.Add(user);
                                        }
                                    }
                                    else if (type.CompareTo(Constants.WindowsGroup) == 0) // Group, add its SID for
                                    {                                                     // group enum processing.
                                        Debug.Assert(!sid.IsNull && !name.IsNull);
                                        Account group = null;
                                        if (Account.CreateGroupAccount(new Sid(sid.Value), name.Value, out group))
                                        {
                                            windowsGroupLogins.Add(group);
                                        }
                                    }

                                    // Update the datatable.
                                    DataRow dr = dataTable.NewRow();
                                    dr[ServerPrincipalDataTable.ParamSnapshotid] = snapshotid;
                                    dr[ServerPrincipalDataTable.ParamPrincipalid] = principalid;
                                    dr[ServerPrincipalDataTable.ParamSid] = sid;
                                    dr[ServerPrincipalDataTable.ParamName] = name;
                                    dr[ServerPrincipalDataTable.ParamType] = type;
                                    dr[ServerPrincipalDataTable.ParamServeraccess] = serveraccess;
                                    dr[ServerPrincipalDataTable.ParamServerdeny] = serverdeny;
                                    dr[ServerPrincipalDataTable.ParamDisabled] = isdisabled;
                                    dr[ServerPrincipalDataTable.ParamIsexpirationchecked] = isexpirationchecked;
                                    dr[ServerPrincipalDataTable.ParamIspolicychecked] = ispolicychecked;
                                    dr[ServerPrincipalDataTable.ParamIsPasswordNull] = ispasswordnull;
                                    dr[ServerPrincipalDataTable.ParamHashkey] = "";
                                    dr[ServerPrincipalDataTable.ParamDefaultDatabase] = defaultdatabase;
                                    dr[ServerPrincipalDataTable.ParamDefaultLanguage] = defaultlanguage;
                                    dr[ServerPrincipalDataTable.ParamPasswordStatus] = passwordStatus;
                                    dataTable.Rows.Add(dr);

                                    // Update count of logins collected
                                    // --------------------------------
                                    Target.numLoginsCollected++;

                                    // Write to repository if exceeds threshold.
                                    if (dataTable.Rows.Count > Constants.RowBatchSize)
                                    {
                                        bcp.WriteToServer(dataTable);
                                        dataTable.Clear();
                                    }
                                }

                                // Write any items still in the data table.
                                if (dataTable.Rows.Count > 0)
                                {
                                    bcp.WriteToServer(dataTable);
                                    dataTable.Clear();
                                }
                            }

                            // For SQL Server 2000, add the fixed server roles to the 
                            // repository.   This is done to make the data consistent
                            // with SQL Server 2005.
                            if (version == ServerVersion.SQL2000)
                            {
                                for (int i = 0; i < FixedServerRoles2000.Length; ++i)
                                {
                                    // Populate the data row.
                                    DataRow dr = dataTable.NewRow();
                                    dr[ServerPrincipalDataTable.ParamSnapshotid] = snapshotid;
                                    dr[ServerPrincipalDataTable.ParamPrincipalid] = FixedServerRoles2000[i].PrincipalId;
                                    dr[ServerPrincipalDataTable.ParamSid] = FixedServerRoles2000[i].Sid;
                                    dr[ServerPrincipalDataTable.ParamName] = FixedServerRoles2000[i].Name;
                                    dr[ServerPrincipalDataTable.ParamType] = FixedServerRoles2000[i].Type;
                                    dr[ServerPrincipalDataTable.ParamServeraccess] = " ";
                                    dr[ServerPrincipalDataTable.ParamServerdeny] = " ";
                                    dr[ServerPrincipalDataTable.ParamDisabled] = " ";
                                    dr[ServerPrincipalDataTable.ParamIsexpirationchecked] = " ";
                                    dr[ServerPrincipalDataTable.ParamIspolicychecked] = " ";
                                    dr[ServerPrincipalDataTable.ParamHashkey] = "";
                                    dataTable.Rows.Add(dr);

                                    // Populate the name dictionary for role membership resolution.
                                    if(!nameDictionary.ContainsKey(FixedServerRoles2000[i].Name.Value))
                                    {
                                        nameDictionary.Add(FixedServerRoles2000[i].Name.Value, FixedServerRoles2000[i].PrincipalId);
                                    }
                                }

                                // Write to repository.
                                bcp.WriteToServer(dataTable);
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    string strMessage = "Processing server principals";
                    logX.loggerX.Error("ERROR - " + strMessage, ex);
                    Sql.Database.CreateApplicationActivityEventInRepository(repositoryConnection, 
                                                                            snapshotid, 
                                                                            Collector.Constants.ActivityType_Error, 
                                                                            Collector.Constants.ActivityEvent_Error, 
                                                                            strMessage + ex.Message);
                    AppLog.WriteAppEventError(SQLsecureEvent.ExErrExceptionRaised, SQLsecureCat.DlDataLoadCat,
                        " SQL Server = " + new SqlConnectionStringBuilder(targetConnection).DataSource +
                        strMessage, ex.Message);

                    isOk = false;
                }
                finally
                {
                    Program.RestoreImpersonationContext(wi);
                }
            }

            // Process role memberships.
            if (isOk)
            {
                if (!processMembers(version, targetConnection, repositoryConnection, snapshotid, nameDictionary))
                {
                    logX.loggerX.Error("ERROR - error encountered in processing server role members");
                    isOk = false;
                }
            }

            // Load principal permissions, if its 2005.
            if (isOk)
            {
                if (version != ServerVersion.SQL2000)
                {
                    if (!ServerPermission.Process(targetConnection, repositoryConnection, snapshotid, SqlObjectType.Login, pidList))
                    {
                        logX.loggerX.Error("ERROR - error encountered in processing server principal permissions");
                        isOk = false;
                    }
                }
            }

            return isOk;
        }

        private static PasswordStatus DetectPasswordStatus(string loginName, byte[] passwordHash, WeakPasswordSetting passwordSettings, ServerVersion version)
        {
            byte[] mixedCasePasswordHash = null;
            byte[] passwordSalt = null;

            //extract the password salt and the different hashes. SQL Server 2000-2008R2 use SHA1 which produces a 20 byte hash with a 4 byte salt.  
            // In SQL Server 2012, the algorithm was changed to use SHA2 (SHA512) with a 4 byte salt
            if (passwordHash != null)
            {
                if (passwordHash.Length >= 6)
                {
                    passwordSalt = new byte[4];
                    Array.ConstrainedCopy(passwordHash, 2, passwordSalt, 0, 4);
                }

                //check for SQL Server 2012 hash first
                if (passwordHash.Length >= 70)
                {
                    mixedCasePasswordHash = new byte[64];
                    Array.ConstrainedCopy(passwordHash, 6, mixedCasePasswordHash, 0, 64);
                }
                else if (passwordHash.Length >= 26) //SQL Server 2000 - SQL Server 2008 R2
                {
                    mixedCasePasswordHash = new byte[20];
                    Array.ConstrainedCopy(passwordHash, 6, mixedCasePasswordHash, 0, 20);
                }
            }
            else
            {
                passwordSalt = mixedCasePasswordHash = null;
            }

            //load the list of passwords and create the common variations (it is an embedded resource)
            if (ServerPrincipal.defaultPasswordList.Count <= 0)
                ServerPrincipal.defaultPasswordList = LoadPasswordList();

            if (version == ServerVersion.SQL2012)
                return CheckPassword(passwordSettings, passwordSalt, mixedCasePasswordHash, loginName, Encryptor.HashVersion.SHA512);
            return CheckPassword(passwordSettings, passwordSalt, mixedCasePasswordHash, loginName, Encryptor.HashVersion.SHA1);
        }

        private static PasswordStatus CheckPassword(WeakPasswordSetting passwordSettings, byte[] salt, byte[] mixedPasswordHash, string loginName, Encryptor.HashVersion hashVersion)
        {
            if (mixedPasswordHash == null)
            {
                if (salt == null)
                {
                    // null password are treated like blank passwords
                    return PasswordStatus.Blank;
                }
                else //6.5 or 7.0 password uses different algorithm
                {
                    return PasswordStatus.Ok;
                }
            }

            int passwordAttempts = 0;

            if (ServerPrincipal.defaultPasswordList != null)
            {
                bool loginCheck = false;

                for (int i = 0; i < ServerPrincipal.defaultPasswordList.Count; i++)
                {
                    loginCheck = false;
                    string word = defaultPasswordList[i];
                    if (word.Contains(PasswordSameAsLoginName))
                    {
                        loginCheck = true;
                        word = word.Replace(PasswordSameAsLoginName, loginName);
                    }
                    if (word.Contains(PasswordSameAsLoginNameReverse))
                    {
                        char[] _Chars = loginName.ToCharArray();
                        Array.Reverse(_Chars);
                        string _LoginReverse = new string(_Chars);

                        loginCheck = true;
                        word = word.Replace(PasswordSameAsLoginNameReverse, _LoginReverse);
                    }

                    byte[] wordBytes = UnicodeEncoding.Unicode.GetBytes(word);
                    byte[] saltedBytes = new byte[wordBytes.Length + salt.Length];
                    wordBytes.CopyTo(saltedBytes, 0);
                    salt.CopyTo(saltedBytes, wordBytes.Length);

                    byte[] wordHash = Encryptor.GenerateHash(saltedBytes, hashVersion);

                    passwordAttempts++;
                    //we found a match - this is a weak password
                    if (Match(wordHash, mixedPasswordHash))
                    {
                        if (loginCheck)
                            return PasswordStatus.SameAsLogin;
                        return PasswordStatus.Weak;
                    }
                }
            }

            if (passwordSettings.CustomPasswordList != null)
            {
                for (int i = 0; i < passwordSettings.CustomPasswordList.Count; i++)
                {
                    string word = passwordSettings.CustomPasswordList[i];
                    byte[] wordBytes = UnicodeEncoding.Unicode.GetBytes(word);
                    byte[] saltedBytes = new byte[wordBytes.Length + salt.Length];
                    wordBytes.CopyTo(saltedBytes, 0);
                    salt.CopyTo(saltedBytes, wordBytes.Length);

                    byte[] wordHash = Encryptor.GenerateHash(saltedBytes, hashVersion);

                    passwordAttempts++;
                    //we found a match - this is a weak password
                    if (Match(wordHash, mixedPasswordHash))
                    {
                        return PasswordStatus.Weak;
                    }
                }
            }

            if (passwordSettings.AdditionalPasswordList != null)
            {
                for (int i = 0; i < passwordSettings.AdditionalPasswordList.Count; i++)
                {
                    string word = passwordSettings.AdditionalPasswordList[i];
                    byte[] wordBytes = UnicodeEncoding.Unicode.GetBytes(word);
                    byte[] saltedBytes = new byte[wordBytes.Length + salt.Length];
                    wordBytes.CopyTo(saltedBytes, 0);
                    salt.CopyTo(saltedBytes, wordBytes.Length);

                    byte[] wordHash = Encryptor.GenerateHash(saltedBytes, hashVersion);

                    passwordAttempts++;
                    //we found a match - this is a weak password
                    if (Match(wordHash, mixedPasswordHash))
                    {
                        return PasswordStatus.Weak;
                    }
                }
            }

            //if we get here, nothing mathed so the password is considered to be ok.
            return PasswordStatus.Ok;
        }

        private static bool Match(byte[] left, byte[] right)
        {
            if (left != null)
            {
                if (right == null)
                    return false;

                int length = left.Length;
                for (int i = 0; i < length; i++)
                {
                    if (left[i] != right[i])
                        return false;
                }
                return true;
            }

            return right == null;
        }

        private static List<string> LoadPasswordList()
        {
            List<string> passwordList = new List<string>();

            try
            {
                //read the passwords from the file.
                Assembly assembly = Assembly.Load("Idera.SQLsecure.Core.Accounts");
                using (Stream stream = assembly.GetManifestResourceStream("Idera.SQLsecure.Core.Accounts.PasswordList.PasswordList.txt"))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string password;

                        while ((password = reader.ReadLine()) != null)
                        {
                            passwordList.Add(password);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error(String.Format("Unable to load password list.  Error:{0}", ex.ToString()));
                return null;
            }
            return passwordList;
        }

        private static byte[] ReadBinaryField(SqlDataReader rdr, int index)
        {
            byte[] retval = null;
            if (!rdr.IsDBNull(index))
            {
                System.Data.SqlTypes.SqlBinary field = rdr.GetSqlBinary(index);
                retval = field.Value;
            }
            return retval;
        }
    }
}
