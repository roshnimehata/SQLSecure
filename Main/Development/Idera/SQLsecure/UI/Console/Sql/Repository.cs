/******************************************************************
 * Name: Repository.cs
 *
 * Description: Encapsulates the SQLsecure Repository.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;

using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Utility;
using Idera.SQLsecure.Core.Accounts;


namespace Idera.SQLsecure.UI.Console.Sql
{
    /// <summary>
    /// Encapsulates SQLsecure Repository connection.
    /// </summary>
    public class Repository
    {
        #region Types

        public class RegisteredServerList : List<RegisteredServer>
        {
            /// <summary>
            /// Searches for the Registered Server with a ConnectionName matching the passed Name ignoring any port number
            /// 
            /// Returns a RegisteredServer object or null if no match is found
            /// </summary>
            /// <param name="Name">The ConnectionName of the server</param>
            /// <returns>RegisteredServer</returns>
            public RegisteredServer Find(string Name)
            {
                string name = Name.Split(',')[0];
                foreach (RegisteredServer server in (RegisteredServerList)this)
                {
                    if (server.Equals(name))
                    {
                        return server;
                    }
                }

                return null;
            }

            /// <summary>
            /// Searches for the Registered Server with a RegisteredServerId matching the passed Id
            /// 
            /// Returns a RegisteredServer object or null if no match is found
            /// </summary>
            /// <param name="Id">The RegisteredServerId of the server</param>
            /// <returns>Policy</returns>
            public RegisteredServer Find(int Id)
            {
                foreach (RegisteredServer server in (RegisteredServerList)this)
                {
                    if (server.RegisteredServerId.Equals(Id))
                    {
                        return server;
                    }
                }

                return null;
            }
        }

        public class PolicyList : List<Policy>
        {
            /// <summary>
            /// Searches for the Policy with a PolicyName matching the passed Name
            /// 
            /// Returns a Policy object or null if no match is found
            /// </summary>
            /// <param name="Name">The PolicyName of the policy</param>
            /// <returns>Policy</returns>
            public Policy Find(string Name)
            {
                foreach (Policy policy in (PolicyList)this)
                {
                    if (string.Compare(policy.PolicyName, Name, true) == 0)
                    {
                        return policy;
                    }
                }

                return null;
            }

            /// <summary>
            /// Searches for the Policy with a PolicyId matching the passed Id
            /// 
            /// Returns a Policy object or null if no match is found
            /// </summary>
            /// <param name="Id">The PolicyId of the policy</param>
            /// <returns>Policy</returns>
            public Policy Find(int Id)
            {
                foreach (Policy policy in (PolicyList)this)
                {
                    if (policy.PolicyId.Equals(Id))
                    {
                        return policy;
                    }
                }

                return null;
            }
        }

        public class AssessmentList : List<Policy>
        {
            /// <summary>
            /// Searches for the Assessment with an AssessmentName matching the passed Name
            /// 
            /// Returns a Policy object or null if no match is found
            /// </summary>
            /// <param name="Name">The AssessmentName of the policy</param>
            /// <returns>Policy</returns>
            public Policy Find(string Name)
            {
                foreach (Policy policy in (AssessmentList)this)
                {
                    if (string.Compare(policy.AssessmentName, Name, true) == 0)
                    {
                        return policy;
                    }
                }

                return null;
            }

            /// <summary>
            /// Searches for the Assessment with an AssessmentId matching the passed Id
            /// 
            /// Returns a Policy object or null if no match is found
            /// </summary>
            /// <param name="Id">The AssessmentId of the Assessment</param>
            /// <returns>Policy</returns>
            public Policy Find(int Id)
            {
                foreach (Policy policy in (AssessmentList)this)
                {
                    if (policy.AssessmentId.Equals(Id))
                    {
                        return policy;
                    }
                }

                return null;
            }

            public AssessmentList FindByState(string assessmentState)
            {
                AssessmentList list = new AssessmentList();
                foreach (Policy policy in (AssessmentList)this)
                {
                    if (policy.AssessmentState.Equals(assessmentState))
                    {
                        list.Add(policy);
                    }
                }

                return list;
            }

            public AssessmentList SavedAssessments
            {
                get
                {
                    AssessmentList list = new AssessmentList();
                    foreach (Policy policy in (AssessmentList)this)
                    {
                        if (!policy.AssessmentState.Equals(Utility.Policy.AssessmentState.Current))
                        {
                            list.Add(policy);
                        }
                    }

                    return list;
                }
            }
        }

        #endregion

        #region Ctors

        public Repository()
        {
            m_IsValid = false;
        }
        public Repository(
                string instance,
                string user,
                string password
            )
        {
            Connect(instance, user, password);
        }

        #endregion

        #region Fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Sql.Repository");
        private SqlConnectionStringBuilder m_ConnectionStringBuilder;

        private string m_Instance = null;                   // last good connection in this session
        private string m_User = null;                       // last good connection user
        private string m_Password = null;                   // last good connection password
        private string m_ServerName = null;                 // server to attempt connection to
        private string m_RepositoryComputerName = null;

        // current repository
        private bool m_IsValid;
        private Sql.ServerVersion m_SQLServerVersion;       // SQL Server version of repository
        private string m_SQLServerFullVersion;              // SQL Server version number of repository
        private int m_DALVersion;
        private int m_SchemaVersion;
        private string m_DbSize;                            // file size of repository on disk

        private BBSProductLicense m_BBSProductLicense = null;

        // policies
        static private PolicyList m_Policies = new PolicyList();

        // registered servers
        static private RegisteredServerList m_RegisteredServers = new RegisteredServerList();

        // notification providers
        static private Idera.SQLsecure.Core.Accounts.NotificationProvider m_notificationProvider = null;


        #endregion

        #region Properties

        public BBSProductLicense bbsProductLicense
        {
            get { return m_BBSProductLicense; }
        }

        public string ConnectionString
        {
            get { return m_IsValid ? m_ConnectionStringBuilder.ConnectionString : ""; }
        }

        public SqlConnection GetOpennedConnection()
        {
            var conn = new SqlConnection(Program.gController.Repository.ConnectionString);
            conn.Open();
            return conn;
        }



        public string UserName
        {
            get { return m_User; }
        }

        public string Password
        {
            get { return m_Password; }
        }

        public string Instance
        {
            get { return m_Instance; }
        }

        public string RepositoryComputerName
        {
            get { return m_RepositoryComputerName; }
        }

        public bool IsConnectionValid
        {
            get { return m_IsValid; }
        }

        public bool IsValid
        {
            get { return m_IsValid && IsLicenseOk(); }
        }

        public string SQLServerVersion
        {
            get { return m_SQLServerVersion.ToString(); }
        }

        public string SQLServerFullVersion
        {
            get { return m_SQLServerFullVersion; }
        }

        public string SQLServerVersionFriendly
        {
            get { return Sql.SqlHelper.ParseVersionFriendly(m_SQLServerFullVersion); }
        }

        public string SQLServerVersionFriendlyLong
        {
            get { return Sql.SqlHelper.ParseVersionFriendly(m_SQLServerFullVersion, true); }
        }

        public int DALVersion
        {
            get { return m_DALVersion; }
        }

        public int SchemaVersion
        {
            get { return m_SchemaVersion; }
        }

        public string DbSize
        {
            get { return m_DbSize; }
        }


        public PolicyList Policies
        {
            get { return m_Policies; }
        }

        public RegisteredServerList RegisteredServers
        {
            get { return m_RegisteredServers; }
        }

        public Idera.SQLsecure.Core.Accounts.NotificationProvider NotificationProvider
        {
            get { return m_notificationProvider; }
        }

        #endregion

        #region Queries
        private const string QueryServername =
            @"SELECT SERVERPROPERTY('MachineName')";

        private const string QueryCheckSQLsecureDbExists =
                   @"SELECT 1 FROM master.dbo.sysdatabases WHERE name='SQLsecure'";
        private const string QueryGetDbInfo =
                    @"EXEC SQLsecure.dbo.isp_sqlsecure_getrepositorydbinfo";
        private const string QueryGetDALSchemaVersion =
                    @"SELECT * FROM SQLsecure.dbo.vwcurrentversion";
        private const string QuerySchemaDALVersionCompatible =
                    @"EXEC SQLsecure.dbo.isp_sqlsecure_isversioncompatible @dtype=N'{0}', @dversion={1}";
        private const string QueryCheckAllCredentials =
                    @"EXEC SQLsecure.dbo.isp_sqlsecure_checkallcredentials";
        private const string NonQueryUpdateAllCredentials =
                    @"SQLsecure.dbo.isp_sqlsecure_updateallcredentials";

        private const string ParamLogin = "serverlogin";
        private const string ParamPassword = "serverpassword";

        // Columns for handling the version query results
        private enum VersionColumn
        {
            dalversion = 0,
            schemaversion
        }

        private const string ColRepositoryInfo_DbSize = "db_size";

        #endregion

        #region Helpers

        #region Connection Management

        //-----------------------------------------------------------------------------
        // Connect to the Repository
        //-----------------------------------------------------------------------------
        public bool Connect(string instance)
        {
            // Connect using the current Windows user and password
            return Connect(instance, null, null);
        }


        public bool Connect(string instance, string user, string password)
        {
            try
            {
                if ((instance.ToUpper() + "\\").StartsWith("(LOCAL)\\"))
                {
                    m_ServerName = instance.ToUpper().Replace("(LOCAL)", Environment.MachineName).ToUpper();
                }
                else if ((instance + "\\").StartsWith(".\\"))
                {
                    m_ServerName = instance.Replace(".", Environment.MachineName).ToUpper();
                }
                else if ((instance + "\\").StartsWith("localhost\\"))
                {
                    m_ServerName = instance.Replace("localhost", Environment.MachineName).ToUpper();
                }
                else
                {
                    m_ServerName = instance.ToUpper();
                }

                m_ConnectionStringBuilder = Sql.SqlHelper.ConstructConnectionString(m_ServerName, user, password, Utility.Activity.TypeServerOnPremise);
                if (Connect())
                {
                    //SQLsecure 3.1 (Tushar)--Saving UserName and Password in UserData object which will be saved in config file on closure of UI.
                    //m_User = user;
                    //m_Password = password;
                    UserData.Current.RepositoryInfo.UserName = m_User = user;
                    UserData.Current.RepositoryInfo.Password = m_Password = password;
                }

            }
            catch (SqlException ex)
            {
                logX.loggerX.Error(string.Format(Utility.ErrorMsgs.ErrorStub, Utility.ErrorMsgs.CantConnectRepository), ex);
                MsgBox.ShowError(Utility.ErrorMsgs.CantConnectRepository, ex);
            }

            return m_IsValid;
        }

        public bool Connect()
        {
            try
            {
                m_IsValid = OpenRepository();

                if (m_IsValid)
                {
                    UserData.Current.RepositoryInfo.ServerName = m_Instance = m_ServerName;
                    ResetLicense();
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error(string.Format(Utility.ErrorMsgs.ErrorStub, Utility.ErrorMsgs.CantConnectRepository), ex);
                MsgBox.ShowError(Utility.ErrorMsgs.CantConnectRepository, ex);

                m_IsValid = false;
            }
            if (!m_IsValid)
            {
                m_Instance = "";
                m_User = "";
                m_Password = "";
                m_ServerName = "";
                m_RegisteredServers.Clear();
            }

            return m_IsValid;
        }
        
        private bool OpenRepository()
        {
            using (SqlConnection connection = new SqlConnection(m_ConnectionStringBuilder.ConnectionString))
            {
                // Open connection to the repository SQL Server.  Make sure we are connected
                // to SQL Server 2000 or 2005.
                try
                {
                    //Clear any previous access if there is an error
                    Program.gController.Permissions.Clear();

                    // Open the connection.
                    logX.loggerX.Info("Opening connection to repository");
                    try
                    {
                        connection.Open();
                    }
                    catch (SqlException ex)
                    {
                        logX.loggerX.Error("ERROR - Can't open connection to Repository", ex);
                        MsgBox.ShowError(Utility.ErrorMsgs.CantConnectRepository, Utility.ErrorMsgs.FailedToConnectMsg);
                        return false;
                    }

                    // Check the SQL Server version.
                    logX.loggerX.Info("Checking SQL Server version of repository");
                    m_SQLServerVersion = Sql.SqlHelper.ParseVersion(connection.ServerVersion);
                    if (m_SQLServerVersion == Sql.ServerVersion.Unsupported)
                    {
                        logX.loggerX.Error(string.Format(Utility.ErrorMsgs.ErrorStub, Utility.ErrorMsgs.VersionNotSupported), connection.ServerVersion);
                        MsgBox.ShowError(Utility.ErrorMsgs.VersionNotSupported, Utility.ErrorMsgs.SQLServerVersionNotSupported);
                        return false;
                    }
                    //Save the version info
                    m_SQLServerFullVersion = connection.ServerVersion;

                    // Check if SQLsecure database is in the repository.
                    logX.loggerX.Info("Checking for SQLsecure database in the repository");
                    using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                    QueryCheckSQLsecureDbExists, null))
                    {
                        if (!rdr.HasRows) // No rows found, no SQLsecure database
                        {
                            logX.loggerX.Error(string.Format(Utility.ErrorMsgs.ErrorStub, Utility.ErrorMsgs.RepositoryNotFound));
                            MsgBox.ShowError(Utility.ErrorMsgs.RepositoryNotFound, Utility.ErrorMsgs.RepositoryDBNotFound);
                            return false;
                        }
                    }

                    // Check schema/DAL versions.
                    logX.loggerX.Info("Checking SQLsecure DAL version");
                    using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                    String.Format(QuerySchemaDALVersionCompatible, "DAL", Utility.Constants.DalVersion), null))
                    {
                        bool isDalCompatible = false;
                        // This table has only one column and row with a Y or N in it.
                        if (rdr.Read())
                        {
                            // Get DAL & schema versions.
                            if (rdr[0].ToString().ToUpper() == "Y")
                                isDalCompatible = true;
                        }
                        // If versions don't match, bail out.
                        if (!isDalCompatible)
                        {
                            logX.loggerX.Error(string.Format(Utility.ErrorMsgs.ErrorStub, Utility.ErrorMsgs.DalNotSupported), Utility.Constants.DalVersion);
                            MsgBox.ShowError(Utility.ErrorMsgs.VersionNotSupported,
                                             String.Format(Utility.ErrorMsgs.DalNotSupported, m_ServerName,
                                                           Utility.Constants.DalVersion));
                            return false;
                        }
                    }
                    // Get the Schema and DAL versions for display if valid
                    logX.loggerX.Info("Retrieving SQLsecure DAL and Schema version");
                    using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                    String.Format(QueryGetDALSchemaVersion), null))
                    {
                        // This table has only one column and row with a Y or N in it.
                        if (rdr.Read())
                        {
                            // Get DAL & schema versions.
                            m_SchemaVersion = Convert.ToInt32(rdr[(int)VersionColumn.schemaversion]);
                            m_DALVersion = Convert.ToInt32(rdr[(int)VersionColumn.dalversion]);
                        }
                    }

                    Program.gController.Permissions.checkRepositorySecurity(m_ConnectionStringBuilder.ConnectionString);

                    // populate the lists of policies, registered servers and notification providers if user has view security
                    if (Program.gController.Permissions.isViewer)
                    {
                        LoadPolicies();
                        LoadRegisteredServers();
                        LoadNotificationProvider();
                    }

                    using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                QueryServername, null))
                    {
                        if (rdr.Read())
                        {
                            m_RepositoryComputerName = (string)rdr[0];
                        }
                        else
                        {
                            logX.loggerX.Warn("WARN - failed to read server name");
                        }
                    }

                    // get repository info - currently, only the size is used
                    using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                QueryGetDbInfo, null))
                    {
                        m_DbSize = string.Empty;
                        if (rdr.Read())
                        {
                            m_DbSize = rdr[ColRepositoryInfo_DbSize].ToString().Trim();
                        }
                        else
                        {
                            logX.loggerX.Warn("WARN - failed to read repository server info");
                        }
                    }
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 229)
                    {
                        logX.loggerX.Error("ERROR - user has no permissions to the repository.", ex);
                        MsgBox.ShowError(Utility.ErrorMsgs.CantConnectRepository, Utility.ErrorMsgs.UserHasNoPermission);
                    }
                    else
                    {
                        logX.loggerX.Error("ERROR - exception raised when validating the repository.", ex);
                        MsgBox.ShowError(Utility.ErrorMsgs.CantConnectRepository, Utility.ErrorMsgs.CantValidateRepository, ex);
                    }
                    return false;
                }

                return true;
            }
        }
        #endregion


        static public void addLogin(string storedProcName, string loginName)
        {
            string safeLoginName = SqlHelper.CreateSafeString(loginName);
            StringBuilder sql = new StringBuilder();
            try
            {
                sql.AppendFormat("EXEC {0} {1} ", storedProcName, safeLoginName);
                sql.AppendFormat(" EXEC sp_defaultdb {0}, 'SQLsecure' ", safeLoginName);
                sql.AppendFormat(" USE SQLsecure  EXEC sp_grantdbaccess {0} ", safeLoginName);
                sql.AppendFormat(" EXEC sp_addrolemember 'SQLSecureView', {0} ", safeLoginName);

                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sql.ToString(), connection);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                string message = string.Empty;
                switch ((uint)ex.Number)
                {
                    case 0x3ab0:  // Group already exist
                    case 0x3aaf: // Login already exist, don't show error
                                 // Do nothing
                        break;
                    case 0x3c29: // Name format is OK, but name not found
                    case 0x3c2f: // Not valid format for windows name DOMAIN\user
                        message = string.Format(Utility.ErrorMsgs.AddLoginErrorInvalidUser, safeLoginName);
                        Utility.MsgBox.ShowError(Utility.ErrorMsgs.AddLoginCaption, message);
                        break;
                    default:
                        message = string.Format(Utility.ErrorMsgs.AddLoginErrorMsg, safeLoginName);
                        Utility.MsgBox.ShowError(Utility.ErrorMsgs.AddLoginCaption, message, ex);
                        break;
                }
            }
            catch (Exception ex)
            {
                string message = string.Format(Utility.ErrorMsgs.AddLoginErrorMsg, safeLoginName);
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.AddLoginCaption, message, ex);
            }
        }

        static public void deleteLogin(string loginName)
        {
            string sql = String.Format("exec master..sp_revokelogin {0}",
                                         SqlHelper.CreateSafeString(loginName));
            try
            {
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError("Add Login", ex);
            }

        }

        //--------------------------------------------------------------------
        // AddToRole
        //--------------------------------------------------------------------
        static public void AddToRole(string login, string rolename)
        {
            string sql = String.Format("exec master..sp_addsrvrolemember {0}, {1}",
                                        SqlHelper.CreateSafeString(login),
                                        SqlHelper.CreateSafeString(rolename));
            try
            {
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError("Add Role", ex);

            }

        }



        //--------------------------------------------------------------------
        // RemoveFromRole
        //--------------------------------------------------------------------
        static public void RemoveFromRole(string login, string rolename)
        {
            string sql = String.Format("exec master..sp_dropsrvrolemember {0}, {1}",
                                        SqlHelper.CreateSafeString(login),
                                        SqlHelper.CreateSafeString(rolename));
            try
            {
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError("Remove Role", ex);
            }
        }

        // Policy handling
        public void LoadPolicies()
        {
            m_Policies = Policy.LoadPolicies(m_ConnectionStringBuilder.ConnectionString);
        }

        public void RefreshPolicies()
        {
            m_Policies = Policy.RefreshPolicies();
        }

        public Sql.Policy GetPolicy(int policyId)
        {
            Debug.Assert(m_Policies != null);

            return m_Policies.Find(policyId);
        }

        public Sql.Policy GetPolicy(string name)
        {
            Debug.Assert(m_Policies != null);

            return m_Policies.Find(name);
        }

        // Registered Server handling
        public void LoadRegisteredServers()
        {
            m_RegisteredServers = RegisteredServer.LoadRegisteredServers(m_ConnectionStringBuilder.ConnectionString);
        }

        public void RefreshRegisteredServers()
        {
            m_RegisteredServers = RegisteredServer.RefreshRegisteredServers();
        }

        public Sql.RegisteredServer GetServer(string serverName)
        {
            Debug.Assert(m_RegisteredServers != null);

            foreach (Sql.RegisteredServer svr in m_RegisteredServers)
            {
                if (svr.ConnectionName.Equals(serverName, StringComparison.CurrentCultureIgnoreCase))
                {
                    return svr;
                }
            }

            return null;
        }

        // Notification Provider handling
        public void LoadNotificationProvider()
        {
            m_notificationProvider = Idera.SQLsecure.Core.Accounts.NotificationProvider.LoadNotificationProvider(m_ConnectionStringBuilder.ConnectionString, Utility.SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry());
        }

        public void RefreshNotificationProvider()
        {
            m_notificationProvider = Idera.SQLsecure.Core.Accounts.NotificationProvider.LoadNotificationProvider(m_ConnectionStringBuilder.ConnectionString, Utility.SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry());
        }


        #region License Management

        public void ResetLicense()
        {
            if (m_IsValid)
            {
                m_BBSProductLicense = null;
                m_BBSProductLicense = new BBSProductLicense(ConnectionString, Instance,
                                                            Utility.Constants.SQLsecureProductID, Utility.Constants.SQLsecureLicenseProductVersionStr);
                m_BBSProductLicense.IsProductLicensed();
            }
        }

        public bool IsLicenseTrial()
        {
            bool isTrial = false;
            if (m_IsValid)
            {
                if (m_BBSProductLicense == null)
                {
                    logX.loggerX.Info("Checking License");
                    m_BBSProductLicense = new BBSProductLicense(ConnectionString, Instance,
                                                                Utility.Constants.SQLsecureProductID, Utility.Constants.SQLsecureLicenseProductVersionStr);
                    m_BBSProductLicense.IsProductLicensed();
                }
                isTrial = m_BBSProductLicense.CombinedLicense.isTrial;
            }
            return isTrial;

        }

        public int GetNumLicensedServers()
        {
            int numServers = 0;
            if (m_IsValid)
            {
                if (m_BBSProductLicense == null)
                {
                    logX.loggerX.Info("Checking License");
                    m_BBSProductLicense = new BBSProductLicense(ConnectionString, Instance,
                                                                Utility.Constants.SQLsecureProductID, Utility.Constants.SQLsecureLicenseProductVersionStr);
                    m_BBSProductLicense.IsProductLicensed();
                }
                numServers = m_BBSProductLicense.CombinedLicense.numLicensedServers;
            }
            return numServers;
        }

        public string GetStrLicensedServers()
        {
            string numServers = "0";
            if (m_IsValid)
            {
                if (m_BBSProductLicense == null)
                {
                    logX.loggerX.Info("Checking License");
                    m_BBSProductLicense = new BBSProductLicense(ConnectionString, Instance,
                                                                Utility.Constants.SQLsecureProductID, Utility.Constants.SQLsecureLicenseProductVersionStr);
                    m_BBSProductLicense.IsProductLicensed();
                }
                numServers = m_BBSProductLicense.CombinedLicense.numLicensedServersStr;
            }
            return numServers;
        }

        public bool IsLicenseOk()
        {
            bool isOK = false;
            if (m_IsValid)
            {
                if (m_BBSProductLicense == null)
                {
                    logX.loggerX.Info("Checking License");
                    m_BBSProductLicense = new BBSProductLicense(ConnectionString, Instance,
                                                                Utility.Constants.SQLsecureProductID, Utility.Constants.SQLsecureLicenseProductVersionStr);
                    m_BBSProductLicense.IsProductLicensed();
                }
                isOK = (m_BBSProductLicense.CombinedLicense.licState == BBSProductLicense.LicenseState.Valid);
            }
            return isOK;
        }

        #endregion

        //SQLsecure 3.1 (Tushar)--Added parameter for type of server and authentication.
        public int getRepositoryVersion(string Server_Name,string userName,string password,int typeOfServer,int typeOfAuthentication)
        {
            int m_SchemaVersion = 0;
            try
            {
                //SQLsecure 3.1 (Tushar)--On Basis of type of server and authentication constructing the connectionStringBuilder.
                SqlConnectionStringBuilder m_ConnectionStringBuilder = null;
                switch (typeOfServer)
                {
                    case 3://on-premise local sql server.
                        m_ConnectionStringBuilder = Sql.SqlHelper.ConstructConnectionString(Server_Name, null, null, Utility.Activity.TypeServerOnPremise);
                        break;
                    case 2://remote sql server.
                        m_ConnectionStringBuilder = Sql.SqlHelper.ConstructConnectionString(Server_Name, userName, password, Utility.Activity.TypeServerOnPremise);
                        break;
                    case 1://Azure DB
                        m_ConnectionStringBuilder = Sql.SqlHelper.ConstructConnectionString(Server_Name, userName, password, Utility.Activity.TypeServerAzureDB);
                        break;
                    case 0://Azure VM
                        m_ConnectionStringBuilder = Sql.SqlHelper.ConstructConnectionString(Server_Name, userName, password, Utility.Activity.TypeServerAzureVM);
                        break;
                }
                using (SqlConnection connection = new SqlConnection(m_ConnectionStringBuilder.ConnectionString))
                {
                    connection.Open();
                    using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                        String.Format(QueryGetDALSchemaVersion), null))
                    {
                        // This table has only one column and row with a Y or N in it.
                        if (rdr.Read())
                        {
                            // Get DAL & schema versions.
                            m_SchemaVersion = Convert.ToInt32(rdr[(int)VersionColumn.schemaversion]);
                            m_DALVersion = Convert.ToInt32(rdr[(int)VersionColumn.dalversion]);
                        }
                    }
             
                }
            }
            catch (Exception e)
            {
            }
            return m_SchemaVersion;
        }

        public static bool checkAllCredentialsEntered(string connectionString)
        {
            bool retval = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open connection to the repository SQL Server.  
                try
                {
                    connection.Open();
                    retval = checkAllCredentialsEntered(connection);
                }
                catch (SqlException ex)
                {
                    logX.loggerX.Error("Failed to open connection to Repository to check for all credentials entered.");
                }
            }
            return retval;
        }

        /// <summary>
        /// Check that all servers have been properly upgraded with the changes in credentials for version 2.0
        /// </summary>
        /// <returns>true if all registered servers have credentials, otherwise false</returns>
        private static bool checkAllCredentialsEntered(SqlConnection connection)
        {
            bool retval = false;

            logX.loggerX.Info("Checking All Registered Server Credentials");
            using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                            String.Format(QueryCheckAllCredentials), null))
            {
                // This table has only one column and row with a Y or N in it.
                if (rdr.Read())
                {
                    string credentialsfilled = rdr[0].ToString();

                    retval = credentialsfilled == "Y" ? true : false;
                }
            }

            return retval;
        }


        public static void updateAllCredentials(string connectionString, string login, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open connection to the repository SQL Server.  
                try
                {
                    connection.Open();
                    updateAllCredentials(connection, login, password);
                }
                catch (SqlException ex)
                {
                    logX.loggerX.Error("Failed to open connection to Repository to update all credentials.");
                }
            }
        }

        /// <summary>
        /// Update all blank registered server credentials for version 2.0
        /// </summary>
        /// <returns></returns>
        private static void updateAllCredentials(SqlConnection connection, string login, string password)
        {
            logX.loggerX.Info("Updating All Registered Server Credentials");

            // Encrypt password before saving it to the repository
            string cipherPassword = Encryptor.Encrypt(password);

            SqlParameter paramLogin = new SqlParameter(ParamLogin, login);
            SqlParameter paramPassword = new SqlParameter(ParamPassword, cipherPassword);

            SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure,
                                        NonQueryUpdateAllCredentials, new SqlParameter[] { paramLogin, paramPassword });
        }

        #endregion
    }

}
