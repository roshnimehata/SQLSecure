/******************************************************************
 * Name: Logging.cs
 *
 * Description: Diagnostic and event logging wrapper functions are
 * defined in this file.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using Idera.SQLsecure.Core.Logger;

namespace Idera.SQLsecure.Collector.Utility
{
    //internal static class Logging
//    {
//        #region Fields
////        private static string       m_LogFolder;
//        private static LoggingLevel m_LogLevel = LoggingLevel.Error;
//        #endregion

//        #region Helpers
//        /// <summary>
//        /// Constructs the default log folder based on the assembly location.
//        /// </summary>
//        private static string defaultLogFolder
//        {
//            get
//            {
//                return ((new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location).DirectoryName)
//                                    + Constants.DfltLogFolderLeaf);
//            }
//        }
//        /// <summary>
//        /// Constructs the log file name based on the assembly name.
//        /// </summary>
//        private static string fileName
//        {
//            get
//            {
//                return ((System.Reflection.Assembly.GetEntryAssembly().GetName().Name) + Constants.DfltLogFileExtension);
//            }
//        }
//        /// <summary>
//        /// Reads the log settings from the registry (Folder and Level).  If the
//        /// settings are not present in the registry, then default values are
//        /// created.
//        /// </summary>
//        /// <param name="folder"></param>
//        /// <param name="level"></param>
//        /// <returns></returns>
//        private static bool getLogSettingsFromRegistry(
//                out string folder,
//                out LoggingLevel level
//            )
//        {
//            // Init return.
//            folder = null;
//            level = LoggingLevel.Off;

//            // Read/setup registry log settings Folder and Level.
//            bool isOk = true;
//            // Open the SQLsecure logging registry key.
//            using (RegistryKey regIderSQLsecureKey = Registry.LocalMachine.CreateSubKey(Constants.RegKey_SQLsecureLogging_Str))
//            {
//                // Read log folder and level.
//                object logFolderObj = regIderSQLsecureKey.GetValue(Constants.RegValue_LogFolder_Str);
//                object logLevelObj = regIderSQLsecureKey.GetValue(Constants.RegValue_DLLogLevel_Str);

//                // Check if the log folder & level values are valid.
//                bool logFolderValid = true;
//                bool logLevelValid = true;
//                if (logFolderObj == null || logFolderObj.GetType() != typeof(string))
//                {
//                    logFolderValid = false;
//                }
//                else
//                {
//                    // Create/open directory, if that fails then log folder is invalid.
//                    try
//                    {
//                        DirectoryInfo dirInfo = new DirectoryInfo((string)logFolderObj);
//                        dirInfo.Create();
//                    }
//                    catch
//                    {
//                        logFolderValid = false;
//                    }
//                }
//                if (logLevelObj == null || logLevelObj.GetType() != typeof(int))
//                {
//                    logLevelValid = false;
//                }

//                // If log folder is not valid, setup the default.
//                if (!logFolderValid)
//                {
//                    // Get the current assembly location.
//                    folder = defaultLogFolder;
//                    regIderSQLsecureKey.SetValue(Constants.RegValue_LogFolder_Str, folder);
//                }
//                else
//                {
//                    folder = checked((string)logFolderObj);
//                }
//                if (!logLevelValid)
//                {
//                    level = Constants.DfltLogLevel;
//                    regIderSQLsecureKey.SetValue(Constants.RegValue_DLLogLevel_Str, (int)level);
//                }
//                else
//                {
//                    level = (LoggingLevel)checked((int)logLevelObj);
//                }
//            }

//            return isOk;
//        }
//        #endregion

//        #region Methods
//        /// <summary>
//        /// This function reads parameters from the registry and 
//        /// initializes logging.
//        /// </summary>
//        public static void InitializeLogging()
//        {
//            // Read the log folder and level from the registry,
//            // and setup the logger object.
//            if (getLogSettingsFromRegistry(out m_LogFolder, out m_LogLevel))
//            {
//                DiagLog.LogFileName = Path.Combine(m_LogFolder, fileName);
//                DiagLog.Level = (LoggingLevel)m_LogLevel;
//            }
//        }

//        /// <summary>
//        /// This function returns the new log file path based on the
//        /// target name.
//        /// </summary>
//        /// <param name="targetInstance"></param>
//        /// <param name="argsString"></param>
//        /// <returns></returns>
//        public static void SwitchToTargetInstanceLogFile(
//                string targetInstance,
//                string argsString
//            )
//        {
//            // Build new log file path based on target instance.
//            StringBuilder sbldr = new StringBuilder();
//            sbldr.Append(String.Copy(targetInstance).Replace('\\','_'));
//            sbldr.Append(Constants.DfltLogFileExtension);
//            string newLogFile = Path.Combine(m_LogFolder, sbldr.ToString());

//            DiagLog.LogInfo("Switching log file to target instance named file - ", newLogFile);
//            DiagLog.LogFileName = newLogFile;
//            DiagLog.LogInfo("Logging switched to [", targetInstance, "] based log file, ", DateTime.Now.ToString());
//            DiagLog.LogInfo("Data Loader parameters are: ", argsString);
//        }

//        #endregion
//    }
}
