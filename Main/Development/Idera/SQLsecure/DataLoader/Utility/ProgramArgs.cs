/******************************************************************
 * Name: ProgramArgs.cs
 *
 * Description: This class encapsulates command line args processing.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

using Idera.SQLsecure.Core.Logger;

namespace Idera.SQLsecure.Collector.Utility
{
    /// <summary>
    /// This class encapsulates the program command line
    /// args.   It provides services for parsing and getting
    /// individual fields.
    /// </summary>
    internal class ProgramArgs
    {
        private static byte[] CLIEncryptionKey = new byte[] { 8, 34, 76, 12, 45, 21, 142, 34, 201, 9, 172, 63, 
                                      (byte) 'c', (byte) 'E', (byte) 'w', (byte) 'n', (byte) 'O', (byte) 'm',
                                      (byte) 'x', (byte) 'd', (byte) 'B', (byte) 'f', (byte) 'G', (byte) 'a',
                                      (byte) 'q', (byte) ' ', (byte) 'j',  (byte) 'U',  (byte) 'i',  (byte) 'D',
                                      (byte) 'S', (byte) 'P'
                                    };

        #region Fields
        private static LogX logX = new LogX("Idera.SQLsecure.Collector.Utility.ProgramArgs");
        private readonly bool m_Valid;
        private readonly string m_TargetInstance;
        private readonly string m_Repository;
        private readonly string m_RepositoryUser;
        private readonly string m_RepositoryPassword;
        private readonly bool m_Verbose;
        private readonly bool m_bAutomatedRun;
        private readonly string m_EncryptedPassword;
        #endregion
        
        #region Helpers
        private static bool parseArgs(
                string[] args,
                out string targetInstance,
                out string repository,
                out string repositoryUser,
                out string repositoryPassword,
                out bool verbose,
                out bool automatedRun,
                out string encryptedPassword
            )
        {
            // Init returns.
            targetInstance = null;
            repository = null;
            repositoryUser = null;
            repositoryPassword = null;
            verbose = false;
            automatedRun = true;
            encryptedPassword = null;

            // Get number of command line args.
            int numArgs = args != null ? args.GetLength(0) : 0;
            if (numArgs <= 0)
            {
                logX.loggerX.Error("No parameters specified, at least need the target instance.");
                return false;
            }

            // Parse arguments.
            bool isOk = true;
            int i = 0;
            while (isOk && i < numArgs)
            {
                // Target instance
                if (String.Compare(args[i], Constants.TargetInstance, true) == 0)
                {
                    // the next arg is the target instance name.
                    ++i;
                    isOk = i < numArgs;
                    if (isOk) { targetInstance = args[i].ToUpper(); }
                }
                // Repository
                else if (String.Compare(args[i], Constants.Repository, true) == 0)
                {
                    // the next arg is the repository
                    ++i;
                    isOk = i < numArgs;
                    if (isOk) { repository = args[i].ToUpper(); }
                }
                // Repository user
                else if (String.Compare(args[i], Constants.RepositoryUser, true) == 0)
                {
                    // the next arg is the repository user
                    ++i;
                    isOk = i < numArgs;
                    if (isOk) { repositoryUser = args[i]; }
                }
                else if (String.Compare(args[i], Constants.RepositoryPassword, true) == 0)
                {
                    // the next arg is the repository user
                    ++i;
                    isOk = i < numArgs;
                    if (isOk) { repositoryPassword = args[i]; }
                }
                // Verbose
                else if (String.Compare(args[i], Constants.Verbose, true) == 0)
                {
                    verbose = true;
                }
                // Manual
                else if (String.Compare(args[i], Constants.Manual, true) == 0)
                {
                    automatedRun = false;
                }
                // EncryptPassword command
                else if (String.Compare(args[i], Constants.EncryptPassword, true) == 0)
                {
                    // the next arg is the password to encrypt
                    ++i;
                    isOk = i < numArgs;
                    if (isOk) { encryptedPassword = Idera.SQLsecure.Core.Accounts.Encryptor.Encrypt(args[i], CLIEncryptionKey); }
                }
                else if (String.Compare(args[i], Constants.EncryptedRepositoryPassword, true) == 0)
                {
                    // the next arg is the encrypted version of the password
                    ++i;
                    isOk = i < numArgs;
                    if (isOk) { repositoryPassword = Idera.SQLsecure.Core.Accounts.Encryptor.Decrypt(args[i], CLIEncryptionKey); }
                }
                // Unknown param.
                else
                {
                    logX.loggerX.Error(args[i], " is an unknown parameter");
                    isOk = false;
                }

                // Increment arg index.
                ++i;
            }

            // Validate the options.
            if (isOk)
            {
                isOk = !String.IsNullOrEmpty(targetInstance);
                isOk &= !String.IsNullOrEmpty(repository);
                isOk |= !String.IsNullOrEmpty(encryptedPassword);
            }

            // If invalid parsing, clear the return fields.
            if (!isOk)
            {
                targetInstance = null;
                repository = null;
                repositoryUser = null;
                repositoryPassword = null;
                verbose = false;
                automatedRun = true;
                encryptedPassword = null;
            }

            return isOk;
        }
        #endregion
        
        #region Ctors
        public ProgramArgs (string[] args)
        {
            m_Valid = parseArgs(args, out m_TargetInstance, out m_Repository, 
                                    out m_RepositoryUser, out m_RepositoryPassword, out m_Verbose, out m_bAutomatedRun, out m_EncryptedPassword);
        }  
        #endregion
        
        #region Properties
        public bool Valid
        {
            get { return m_Valid; }
        }
        public string TargetInstance
        {
            get { return m_TargetInstance; }
        }
        public string Repository
        {
            get { return m_Repository; }
        }
        public string RepositoryUser
        {
            get { return m_RepositoryUser; }
        }
        public string RepositoryPassword
        {
            get { return m_RepositoryPassword; }
        }
        public bool Verbose
        {
            get { return m_Verbose; }
        }
        public bool AutomatedRun
        {
            get { return m_bAutomatedRun; }
        }
        public string EncryptedPassword
        {
            get { return m_EncryptedPassword; }
        }
        #endregion
        
        #region Methods
        public static string ArgsToString(string[] args)
        {
            // Get number of elements in the args.
            int numArgs = args != null ? args.GetLength(0) : 0;
            if (numArgs == 0)
            {
                return "<None>";
            }

            StringBuilder argsBldr = new StringBuilder();
            for (int i = 0; i < numArgs; ++i)
            {
                argsBldr.Append("[");
                argsBldr.Append(args[i]);
                argsBldr.Append("] ");
            }

            return argsBldr.ToString();
        }
        #endregion
    }

}
