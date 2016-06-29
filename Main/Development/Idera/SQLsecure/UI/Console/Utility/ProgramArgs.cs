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

namespace Idera.SQLsecure.UI.Console.Utility
{
    /// <summary>
    /// This class encapsulates the program command line
    /// args.   It provides services for parsing and getting
    /// individual fields.
    /// </summary>
    internal class ProgramArgs
    {
        #region Fields
        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Utility.ProgramArgs");
        private readonly bool m_Valid;
        private readonly string m_Repository;
        private readonly bool m_SetDefault;
        private readonly bool m_Verbose;
        #endregion

        #region Helpers
        private static bool parseArgs(
                string[] args,
                out string repository,
                out bool setDefault,
                out bool verbose
            )
        {
            // Init returns.
            repository = "";
            setDefault = false;
            verbose = false;

            // Get number of command line args.
            int numArgs = args != null ? args.GetLength(0) : 0;
 
            bool isOk = true;
            if (numArgs > 0)
            {
                // Parse arguments.
                int i = 0;
                while (isOk && i < numArgs)
                {
                    // Default Repository
                    if (String.Compare(args[i], Constants.SetDefault, true) == 0)
                    {
                        // the next arg is the default repository
                        ++i;
                        isOk = i < numArgs;
                        if (isOk) { setDefault = true; repository = args[i]; }
                    }
                    // Repository
                    else if (String.Compare(args[i], Constants.Repository, true) == 0)
                    {
                        // the next arg is the repository
                        ++i;
                        isOk = i < numArgs;
                        if (isOk) { repository = args[i]; }
                    }
                    // Verbose
                    else if (String.Compare(args[i], Constants.Verbose, true) == 0)
                    {
                        verbose = true;
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

                // If invalid parsing, clear the return fields.
                if (!isOk)
                {
                    repository = null;
                    setDefault = false;
                    verbose = false;
                }
            }

            return isOk;
        }
        #endregion
        
        #region Ctors
        public ProgramArgs (string[] args)
        {
            m_Valid = parseArgs(args, out m_Repository, out m_SetDefault, out m_Verbose);
        }  
        #endregion
        
        #region Properties
        public bool Valid
        {
            get { return m_Valid; }
        }
        public string Repository
        {
            get { return m_Repository; }
        }
        public bool SetDefault
        {
            get { return m_SetDefault; }
        }
        public bool Verbose
        {
            get { return m_Verbose; }
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
