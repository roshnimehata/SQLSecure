/******************************************************************
 * Name: Program.cs
 *
 * Description: The data loader Main function is defined in this file.
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

using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.Collector.Utility;
using Idera.SQLsecure.Core.Accounts;
using System.Management;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Security;

namespace Idera.SQLsecure.Collector
{

    class Program
    {
        public enum ImpersonationContext
        {
            Local,          // Used to connect to Repository
            TargetComputer, // Used to connect to Target Computer - For 2.0 Bind is used instead of impersonation
            TargetSQLServer // Used to connect to Target SQL Server
        }

        private static ImpersonationContext m_ImpersonationContext = ImpersonationContext.Local;
        #region Target & Repository
        static private Repository m_Repository;
        static private Target m_Target;
        static private WindowsIdentity m_targetIdentity;
        static private WindowsImpersonationContext m_targetImpersionationContext = null;
        static private WindowsIdentity m_targetSQLServerIdentity;
        static private WindowsImpersonationContext m_targetSQLServerImpersionationContext = null;
        static private WindowsIdentity m_LocalIdentity;
        static private WindowsImpersonationContext m_LocalImpersionationContext = null;
        private static string m_targetUserName;
        private static string m_targetUserPassword;
        private static bool m_UserSQLAuthentication = false;

        private static LogX logX = new LogX("Idera.SQLsecure.Collector.Program");
        #endregion

        public static Server TargetServer
        {
            get { return m_Target == null ? null : m_Target.TargetServer; }
        }

        public static string TargetUserName
        {
            get { return m_targetUserName; }
        }
        public static string targetUserPassword
        {
            get { return m_targetUserPassword; }
        }

        public static ImpersonationContext SetTargetImpersonationContext()
        {
            ImpersonationContext ic = m_ImpersonationContext;
            m_ImpersonationContext = ImpersonationContext.TargetComputer;
            if (ic != ImpersonationContext.TargetComputer)
                {
                using (logX.loggerX.VerboseCall())
                {
                    if (ic == ImpersonationContext.TargetSQLServer)
                    {
                        if (m_targetSQLServerImpersionationContext != null)
                        {
                            logX.loggerX.Verbose(string.Format("Leaving Target SQL Server Impersonation Context: {0}", WindowsIdentity.GetCurrent().Name));
                            m_targetSQLServerImpersionationContext.Undo();
                            m_targetSQLServerImpersionationContext.Dispose();
                            m_targetSQLServerImpersionationContext = null;
                        }
                    }
                    else if(ic == ImpersonationContext.Local)
                    {
                        logX.loggerX.Verbose(string.Format("Leaving Local Impersonation Context: {0}", WindowsIdentity.GetCurrent().Name));
                        //if (m_LocalImpersionationContext != null)
                        //{
                        //    m_LocalImpersionationContext.Undo();
                        //    m_LocalImpersionationContext.Dispose();
                        //    m_LocalImpersionationContext = null;
                        //}
                    }
                    if (m_targetIdentity != null)
                    {
                        m_targetImpersionationContext = m_targetIdentity.Impersonate();
                        logX.loggerX.Verbose(string.Format("Entering Target Computer Impersonation Context: {0}", WindowsIdentity.GetCurrent().Name));
                    }
                    else
                    { 
                        if (TargetServer != null)
                        {
                            bool results = TargetServer.Bind();
                            logX.loggerX.Verbose(string.Format("Entering Target Computer Bind Context: {0}", m_targetUserName));
                        }
                    }
                }
            }
            return ic;
        }

        public static ImpersonationContext SetTargetSQLServerImpersonationContext()
        {
            ImpersonationContext ic = m_ImpersonationContext;
            m_ImpersonationContext = ImpersonationContext.TargetSQLServer;
            if (ic != ImpersonationContext.TargetSQLServer)
            {
                using (logX.loggerX.VerboseCall())
                {
                    if (ic == ImpersonationContext.TargetComputer)
                    {
                        if (m_targetImpersionationContext != null)
                        {
                            logX.loggerX.Verbose(string.Format("Leaving Target Computer Impersonation Context: {0}", WindowsIdentity.GetCurrent().Name));
                            m_targetImpersionationContext.Undo();
                            m_targetImpersionationContext.Dispose();
                            m_targetImpersionationContext = null;
                        }
                        else
                        {
                            if (TargetServer != null)
                            {
                                logX.loggerX.Verbose(string.Format("Leaving Target Computer Bind Context: {0}", m_targetUserName));
                                TargetServer.Unbind();
                            }
                        }
                    }
                    else if(ic == ImpersonationContext.Local)
                    {
                        logX.loggerX.Verbose(string.Format("Leaving Local Impersonation Context: {0}", WindowsIdentity.GetCurrent().Name));
                        //if (m_LocalImpersionationContext != null)
                        //{
                        //    m_LocalImpersionationContext.Undo();
                        //    m_LocalImpersionationContext.Dispose();
                        //    m_LocalImpersionationContext = null;
                        //}                        
                    }
                    if (m_targetSQLServerIdentity != null)
                    {
                        m_targetSQLServerImpersionationContext = m_targetSQLServerIdentity.Impersonate();
                        logX.loggerX.Verbose(string.Format("Entering Target SQL Server Impersonation Context: {0}", WindowsIdentity.GetCurrent().Name));
                    }
                    else
                    {
                        if (m_UserSQLAuthentication)
                        {
                            logX.loggerX.Verbose("Using SQL Server Credentials");
                        }
                        else
                        {
                            logX.loggerX.Verbose("Failed to Enter Target SQL Server Impersonation Context");
                            logX.loggerX.Verbose(string.Format("Using Local User Context for Target SQL Server: {0}", WindowsIdentity.GetCurrent().Name));
                        }
                    }
                }
            }
            return ic;
        }

        public static ImpersonationContext SetLocalImpersonationContext()
        {
            ImpersonationContext ic = m_ImpersonationContext;
            m_ImpersonationContext = ImpersonationContext.Local;
            if (ic != ImpersonationContext.Local)
            {
                using (logX.loggerX.VerboseCall())
                {
                    if (ic == ImpersonationContext.TargetComputer)
                    {
                        if (m_targetImpersionationContext != null)
                        {
                            logX.loggerX.Verbose(string.Format("Leaving Target Computer Impersonation Context: {0}", WindowsIdentity.GetCurrent().Name));
                            m_targetImpersionationContext.Undo();
                            m_targetImpersionationContext.Dispose();
                            m_targetImpersionationContext = null;
                        }
                        else
                        {
                            logX.loggerX.Verbose(string.Format("Leaving Target Computer Bind Context: {0}", m_targetUserName));
                            if (TargetServer != null)
                            {
                                TargetServer.Unbind();
                            }
                        }
                    }
                    else if (ic == ImpersonationContext.TargetSQLServer)
                    {
                        if (m_targetSQLServerImpersionationContext != null)
                        {
                            logX.loggerX.Verbose(string.Format("Leaving Target SQL Server Impersonation Context: {0}", WindowsIdentity.GetCurrent().Name));
                            m_targetSQLServerImpersionationContext.Undo();
                            m_targetSQLServerImpersionationContext.Dispose();
                            m_targetSQLServerImpersionationContext = null;
                        }
                    }

                    //if (m_LocalIdentity != null)
                    //{
                    //    m_LocalImpersionationContext = m_LocalIdentity.Impersonate();
                    //    logX.loggerX.Verbose(string.Format("Entering Local Impersonation Context: {0}", WindowsIdentity.GetCurrent().Name));
                    //}
                    logX.loggerX.Verbose(string.Format("Entering Local Impersonation Context: {0}", WindowsIdentity.GetCurrent().Name));
                }
            }
            return ic;
        }

        public static void RestoreImpersonationContext(ImpersonationContext ic)
        {
            using (logX.loggerX.VerboseCall())
            {
                // Reset old if different than new
                // -------------------------------
                if (m_ImpersonationContext == ImpersonationContext.TargetComputer &&
                    ic != ImpersonationContext.TargetComputer)
                {
                    if (m_targetImpersionationContext != null)
                    {
                        logX.loggerX.Verbose(
                            string.Format("Leaving Target Computer Impersonation Context: {0}", WindowsIdentity.GetCurrent().Name));
                        m_targetImpersionationContext.Undo();
                        m_targetImpersionationContext.Dispose();
                        m_targetImpersionationContext = null;
                    }
                    else
                    {
                        if (TargetServer != null)
                        {
                            logX.loggerX.Verbose(string.Format("Leaving Target Computer Bind Context: {0}", m_targetUserName));
                            TargetServer.Unbind();
                        }
                    }
                }
                else if (m_ImpersonationContext == ImpersonationContext.TargetSQLServer &&
                         ic != ImpersonationContext.TargetSQLServer)
                {
                    if (m_targetSQLServerImpersionationContext != null)
                    {
                        logX.loggerX.Verbose(
                            string.Format("Leaving Target SQL Server Impersonation Context: {0}", WindowsIdentity.GetCurrent().Name));
                        m_targetSQLServerImpersionationContext.Undo();
                        m_targetSQLServerImpersionationContext.Dispose();
                        m_targetSQLServerImpersionationContext = null;
                    }
                }
                else if (m_ImpersonationContext == ImpersonationContext.Local &&
                         ic != ImpersonationContext.Local)
                {
                    logX.loggerX.Verbose(
                        string.Format("Leaving Local Impersonation Context: {0}", WindowsIdentity.GetCurrent().Name));
                    //if (m_LocalImpersionationContext != null)
                    //{
                    //    m_LocalImpersionationContext.Undo();
                    //    m_LocalImpersionationContext.Dispose();
                    //    m_LocalImpersionationContext = null;
                    //}
                }

                // Set new if different than old
                // -----------------------------
                if (ic == ImpersonationContext.TargetComputer &&
                    m_ImpersonationContext != ImpersonationContext.TargetComputer)
                {
                    if (m_targetIdentity != null)
                    {
                        m_targetImpersionationContext = m_targetIdentity.Impersonate();
                        logX.loggerX.Verbose(
                            string.Format("Entering Target Computer Impersonation Context: {0}", WindowsIdentity.GetCurrent().Name));
                    }
                    else
                    {
                        if (TargetServer != null)
                        {
                            TargetServer.Bind();
                            logX.loggerX.Verbose(string.Format("Entering Target Computer Bind Context: {0}", m_targetUserName));
                        }
                    }
                }
                else if (ic == ImpersonationContext.TargetSQLServer &&
                         m_ImpersonationContext != ImpersonationContext.TargetSQLServer)
                {
                    if (m_targetSQLServerIdentity != null)
                    {
                        m_targetSQLServerImpersionationContext = m_targetSQLServerIdentity.Impersonate();
                        logX.loggerX.Verbose(
                            string.Format("Entering Target SQL Server Impersonation Context: {0}", WindowsIdentity.GetCurrent().Name));
                    }
                    else
                    {
                        if (m_UserSQLAuthentication)
                        {
                            logX.loggerX.Verbose("Using SQL Server Credentials");
                        }
                        else
                        {
                            logX.loggerX.Verbose("Failed to Enter Target SQL Server Impersonation Context");
                            logX.loggerX.Verbose(string.Format("Using Local User Context for Target SQL Server: {0}", WindowsIdentity.GetCurrent().Name));
                        }
                    }
                }
                else if (ic == ImpersonationContext.Local &&
                    m_ImpersonationContext != ImpersonationContext.Local)
                {
                    logX.loggerX.Verbose(
                        string.Format("Entering Local Impersonation Context: {0}", WindowsIdentity.GetCurrent().Name));
                    //if (m_LocalIdentity != null)
                    //{
                    //    m_LocalImpersionationContext = m_LocalIdentity.Impersonate();
                    //    logX.loggerX.Verbose(
                    //        string.Format("Entering Local Impersonation Context: {0}", WindowsIdentity.GetCurrent().Name));
                    //}
                }
                m_ImpersonationContext = ic;
            }
        }


        #region Helpers
        private static void startup(Utility.ProgramArgs programArgs, string args)
        {
            // Initialize diagnostic logging.
            if (programArgs.Valid)
            {
                LogX.Initialize(programArgs.TargetInstance.Replace(@"\", "$"));
            }
            else
            {
                LogX.Initialize();
            }
            logX = new LogX("Idera.SQLsecure.Collector.Program");

            // Log to event log that data loader is starting.
            AppLog.WriteAppEventInfo(SQLsecureEvent.DlInfoStartMsg, SQLsecureCat.DlStartCat, 
                                        DateTime.Now.ToString(), args);
            logX.loggerX.Info("Collector started with parameters: ", args);
        }

        private static void shutdown()
        {
            using (logX.loggerX.DebugCall())
            {
                // Log to event log that data loader is ending.
                AppLog.WriteAppEventInfo(SQLsecureEvent.DlInfoEndMsg, SQLsecureCat.DlEndCat, 
                                            DateTime.Now.ToString());
                logX.m_logX.Info("Collector ended at ", DateTime.Now.ToString());
            }
        }
        #endregion

        #region Main
        static void Main(string[] args)
        {
            //for manual run (for developers)
            /*args = new string[4];
            args[0] = "-TargetInstance";
            args[1] = "SS_SQL_SECURE\\MSSQLSERVER2012";
            args[2] = "-Repository";
            args[3] = "SS_SQL_SECURE\\MSSQLSERVER2014";*/

            string targetName = string.Empty;
            bool isOK = true;
            bool needToWriteFailedSnapshot = false;
            System.Diagnostics.Stopwatch swTotal = new System.Diagnostics.Stopwatch();
            swTotal.Start();
            try
            {
                // Initialize the data loader utility.
                string argsString = Utility.ProgramArgs.ArgsToString(args);
                // Parse the command line arguments.   
                Utility.ProgramArgs programArgs = new Utility.ProgramArgs(args);
                if (string.IsNullOrEmpty(programArgs.EncryptedPassword))
                {
                    startup(programArgs, argsString);
                    using (logX.m_logX.DebugCall())
                    {
                        if (programArgs.Valid)
                        {
                            targetName = programArgs.TargetInstance;
                            // Initialize and validate the repository. 
                            m_Repository = new Repository(programArgs.Repository, programArgs.RepositoryUser,
                                                          programArgs.RepositoryPassword);
                            if (m_Repository.IsValid)
                            {
                                // Check license.
                                if (m_Repository.IsLicenseOk())
                                {
                                    Sql.Database.CreateApplicationActivityEventInRepository(
                                        m_Repository.ConnectionString,
                                        programArgs.TargetInstance,
                                        0,
                                        Collector.Constants.
                                            ActivityType_Info,
                                        Collector.Constants.
                                            ActivityEvent_Start,
                                        "Starting snapshot collection for " +
                                        programArgs.TargetInstance);
                                    // Check if the target instance is registered in the Repository.
                                    if (m_Repository.IsTargetRegistered(programArgs.TargetInstance))
                                    {
                                        // Retrieve target instance credentials from the repository.
                                        string server, sqlLogin, sqlPassword, sqlAuthType, serverLogin, serverPassword,serverType;
                                        int? port;
                                        if (m_Repository.GetTargetCredentials(programArgs.TargetInstance, 
                                                                              out server, out port,
                                                                              out sqlLogin, out sqlPassword,
                                                                              out sqlAuthType,
                                                                              out serverLogin, out serverPassword,out serverType))
                                        {
                                            m_targetUserName = serverLogin;
                                            m_targetUserPassword = serverPassword;
                                            if (string.IsNullOrEmpty(serverLogin))
                                            {
                                                // Only issue warning for this case
                                                Sql.Database.CreateApplicationActivityEventInRepository(m_Repository.ConnectionString,
                                                                                                        targetName,
                                                                                                        0,
                                                                                                        Collector.Constants.ActivityType_Warning,
                                                                                                        Collector.Constants.ActivityEvent_Start,
                                                                                                        string.Format("No credentials specified for Operating System and Active Directory, using SQLsecure Collector user {0}", WindowsIdentity.GetCurrent().Name));

                                            }
                                            if (string.IsNullOrEmpty(sqlLogin))
                                            {
                                                throw new Exception("No credentials specified for collecting SQL Server security.");
                                            }
                                            if (serverType == "OP")
                                            {
                                                GetIdentitiesForImpersonation(sqlLogin, sqlPassword, sqlAuthType, serverLogin, serverPassword);
                                            }
                                            else if(serverType=="ADB" && sqlAuthType=="W")
                                            {
                                                //AuthenticationResult authenticationResult= AzureDatabase.GetConnectionToken(serverLogin, serverPassword);
                                            }
                                            //SQLsecure 3.1 (Tushar)--Support for Azure VM.
                                            else if (serverType == "AVM")
                                            {
                                                GetIdentitiesForImpersonation(sqlLogin, sqlPassword, sqlAuthType, serverLogin, serverPassword);
                                            }
                                        }
                                        Program.ImpersonationContext wi;
                                        // Initialize and validate the target.
                                        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                                        sw.Start();
                                        if (serverType == "OP")
                                        {
                                            
                                            wi = SetTargetSQLServerImpersonationContext();
                                            m_Target = new Target(programArgs.TargetInstance, m_Repository);
                                            RestoreImpersonationContext(wi);
                                            
                                        }
                                        else if(serverType=="ADB")
                                        {
                                            m_Target = new Target(programArgs.TargetInstance, m_Repository);
                                        }
                                        //SQLsecure 3.1 (Tushar)--Support for Azure VM.
                                        else if (serverType == "AVM")
                                        {
                                            wi = SetTargetSQLServerImpersonationContext();
                                            m_Target = new Target(programArgs.TargetInstance, m_Repository);
                                            RestoreImpersonationContext(wi);
                                        }
                                        sw.Stop();
                                        logX.loggerX.Verbose("TIMING - Time to initialize and validate target = " +
                                                        sw.ElapsedMilliseconds.ToString() + " msec");
                                        if (m_Target.IsValid )
                                        {
                                            if (serverType == "OP")
                                            {
                                                wi = SetTargetImpersonationContext();

                                                // Load the permissions data.
                                                m_Target.LoadData(programArgs.AutomatedRun);

                                                RestoreImpersonationContext(wi);
                                            }
                                            else if(serverType=="ADB")
                                            {
                                                m_Target.LoadDataAzureDB(programArgs.AutomatedRun,serverType);
                                            }
                                            //SQLsecure 3.1 (Tushar)--Support for Azure VM.
                                            else if (serverType == "AVM")
                                            {
                                                m_Target.LoadDataForAzureVM(programArgs.AutomatedRun);
                                            }
                                        }
                                        else
                                        {
                                            needToWriteFailedSnapshot = true;
                                            logX.loggerX.Error("ERROR - target instance is not valid");
                                            Sql.Database.CreateApplicationActivityEventInRepository(
                                                m_Repository.ConnectionString,
                                                programArgs.TargetInstance,
                                                0,
                                                Constants.ActivityType_Error,
                                                Constants.ActivityEvent_Error,
                                                "Target " + programArgs.TargetInstance + " could not be found");
                                            AppLog.WriteAppEventError(SQLsecureEvent.DlErrOpenTargetConnectionFailed,
                                                                      SQLsecureCat.DlValidationCat,
                                                                      "Target " + programArgs.TargetInstance +
                                                                      " could not be found");
                                            isOK = false;
                                        }
                                    }
                                    else
                                    {
                                        needToWriteFailedSnapshot = true;
                                        logX.loggerX.Error("ERROR - target instance is not registered");
                                        Sql.Database.CreateApplicationActivityEventInRepository(
                                            m_Repository.ConnectionString,
                                            programArgs.TargetInstance,
                                            0,
                                            Collector.Constants.ActivityType_Error,
                                            Collector.Constants.ActivityEvent_Error,
                                            "Target " + programArgs.TargetInstance + " is not registered");
                                        AppLog.WriteAppEventError(SQLsecureEvent.DlErrTargetNotRegistered,
                                                                  SQLsecureCat.DlValidationCat,
                                                                  "Target " + programArgs.TargetInstance +
                                                                  " is not registered");

                                        isOK = false;
                                    }
                                    if (needToWriteFailedSnapshot)
                                    {
                                        int snapshotID = m_Repository.CreateErrorSnapshot(programArgs.TargetInstance);
                                        if (snapshotID != 0)
                                        {
                                            Sql.Database.UpdateRepositoryRegisteredServerTable(
                                                m_Repository.ConnectionString, snapshotID, Constants.StatusError);
                                        }
                                    }
                                }
                                else
                                {
                                    logX.loggerX.Error("ERROR - license check failed.");
                                    Sql.Database.CreateApplicationActivityEventInRepository(
                                        m_Repository.ConnectionString,
                                        programArgs.TargetInstance,
                                        0,
                                        Collector.Constants.
                                            ActivityType_Error,
                                        Collector.Constants.
                                            ActivityEvent_Error,
                                        "The SQLsecure Collector was unable to aquire a valid license");
                                    AppLog.WriteAppEventInfo(SQLsecureEvent.DlErrNoLicense, SQLsecureCat.DlValidationCat);
                                    isOK = false;
                                }
                            }
                            else
                            {
                                logX.loggerX.Error("ERROR - SQLsecure Repository is invalid.");
                                isOK = false;
                            }
                        }
                        else // Invalid args display usage
                        {
                            if (logX == null)
                            {
                                logX = new LogX("Idera.SQLsecure.Collector.Program");
                            }
                            logX.loggerX.Error("ERROR: Failed to parse the arguments");
                            Console.WriteLine(Constants.CopyrightMsg);
                            Console.WriteLine(Constants.UsageMsg);
                            isOK = false;
                        }
                    }
                }
                else        //handle output of the encrypted password
	            {
                    Console.WriteLine(string.Format("Encrypted Password: {0}", programArgs.EncryptedPassword));
	            }
            }
            catch(Exception ex)
            {
                string msg = "Collection Error: " + ex.Message;
                if(m_Repository != null && m_Repository.IsValid)
                {
                    Sql.Database.CreateApplicationActivityEventInRepository(m_Repository.ConnectionString, 
                                                                            targetName,
                                                                            0,
                                                                            Collector.Constants.ActivityType_Error,
                                                                            Collector.Constants.ActivityEvent_Error,
                                                                            msg);
                }
                AppLog.WriteAppEventError(SQLsecureEvent.DlErrOpenTargetConnectionFailed,
                                          SQLsecureCat.DlValidationCat,
                                          "Target " + targetName +
                                          " " + ex.Message);

                if (logX == null)
                {
                    logX = new LogX("Idera.SQLsecure.Collector.Program");
                }
                logX.loggerX.Error(msg);
            }
            finally 
            {
                // Undo the impersonation
                if (m_targetImpersionationContext != null) 
                {
                    m_targetImpersionationContext.Undo();
                    m_targetImpersionationContext.Dispose();
                }
                if (m_targetSQLServerImpersionationContext != null)
                {
                    m_targetSQLServerImpersionationContext.Undo();
                    m_targetSQLServerImpersionationContext.Dispose();
                }
                if (TargetServer != null)
                {
                    TargetServer.RealUnbind();
                }
            }

            swTotal.Stop();
            logX.loggerX.Verbose("TIMING - Total Time for Collector = " + swTotal.ElapsedMilliseconds.ToString() +
                            " msec");
            // Exiting utility, do shutdown processing.
            shutdown();
            Environment.ExitCode = isOK ? 0 : 1;
        
        }

        private static void GetIdentitiesForImpersonation(string sqlLogin, string sqlPassword, string sqlAuthType, string serverLogin, string serverPassword)
        {
            if (!string.IsNullOrEmpty(serverLogin))
            {
                try
                {
                    // Set the current identity from the remote user, used for impersonation                             
                    m_targetIdentity = Impersonation.GetCurrentIdentity(serverLogin, serverPassword);
                }
                catch (Exception e)
                {
                    logX.loggerX.Warn(string.Format("Error Impersonating User {0}:  {1}", serverLogin, e.Message));
                    logX.loggerX.Warn(string.Format("Using SQLsecure Collector user {0}", WindowsIdentity.GetCurrent().Name));
                    //Sql.Database.CreateApplicationActivityEventInRepository(m_Repository.ConnectionString,
                    //                                                        targetName,
                    //                                                        0,
                    //                                                        Collector.Constants.ActivityType_Warning,
                    //                                                        Collector.Constants.ActivityEvent_Start,
                    //                                                        string.Format("Failed to Impersonate Operating System and Active Directory credentials for {0}, using SQLsecure Collector user {1}", serverLogin, WindowsIdentity.GetCurrent().Name));
                }
            }
            if (sqlAuthType != "S")
            {
                try
                {
                    // Set the current identity from the remote user, used for impersonation                             
                    m_targetSQLServerIdentity = Impersonation.GetCurrentIdentity(sqlLogin, sqlPassword);
                }
                catch (Exception e)
                {
                    logX.loggerX.Error(string.Format("Error Impersonating SQL Server User {0}:  {1}", sqlLogin, e.Message));
                    throw new Exception(string.Format("Failed to validate Target SQL Server credentials {0}", sqlLogin));
                }
            }
            else
            {
                m_UserSQLAuthentication = true;
            }
        }

        private static UserCredential GetUserCredential(string username , string password)
        {
            string pwd = password;
            string userId = username;

            SecureString securePassword = new SecureString();

            foreach (char c in pwd) { securePassword.AppendChar(c); }
            securePassword.MakeReadOnly();

            var userCredential = new UserPasswordCredential(userId, securePassword);

            return userCredential;
        }
        
        #endregion
    }
}
