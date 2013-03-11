/******************************************************************
 * Name: DiagLog.cs
 *
 * Description: Diagnostic logging class.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

using System.Diagnostics;
using System.IO;
using System.Data.SqlClient;

using BBS.TracerX;

namespace Idera.SQLsecure.Core.Logger
{
    //#region LoggingLevel
    //public enum LoggingLevel
    //{
    //    Off   = 0,
    //    Error = 1,
    //    Warn  = 2,
    //    Info  = 3,
    //    Debug = 4,
    //    All   = 100
    //}
    //#endregion

    //#region Log4Net helper class
    //public sealed class Log4Net : IDisposable
    //{
    //    #region Fields
    //    private System.Object m_Lock = new System.Object();

    //    private log4net.ILog m_Log;
    //    private log4net.Appender.RollingFileAppender m_RollingFileAppender;

    //    private String m_LogFileName;
    //    private log4net.Core.Level m_Level;
    //    #endregion

    //    #region Helpers
    //    private static string assemblyName
    //    {
    //        get { return System.Reflection.Assembly.GetEntryAssembly().GetName().Name; }
    //    }
    //    private static string defaultFileName
    //    {
    //        get { return (assemblyName + ".log"); }
    //    }
    //    private static log4net.Layout.PatternLayout patternLayout
    //    {
    //        get
    //        {
    //            string pattern = "%timestamp [" + Process.GetCurrentProcess().Id.ToString() + "] [%thread] - %message%newline";
    //            log4net.Layout.PatternLayout ret = new log4net.Layout.PatternLayout(pattern);
    //            ret.Header = "TIME_STAMP [PROCESS_ID] [THREAD_ID] - MESSAGE\n";
    //            return ret;
    //        }
    //    }
    //    private static log4net.Core.Level getLevel(LoggingLevel lvlIn)
    //    {
    //        log4net.Core.Level retLvl;// = log4net.Core.Level.Fatal;
    //        switch (lvlIn)
    //        {
    //            case LoggingLevel.All:
    //                retLvl = log4net.Core.Level.All;
    //                break;

    //            case LoggingLevel.Debug:
    //                retLvl = log4net.Core.Level.Debug;
    //                break;

    //            case LoggingLevel.Info:
    //                retLvl = log4net.Core.Level.Info;
    //                break;

    //            case LoggingLevel.Warn:
    //                retLvl = log4net.Core.Level.Warn;
    //                break;

    //            case LoggingLevel.Error:
    //                retLvl = log4net.Core.Level.Error;
    //                break;

    //            default:
    //            case LoggingLevel.Off:
    //                retLvl = log4net.Core.Level.Fatal;
    //                break;
    //        }
    //        return retLvl;
    //    }
    //    private static LoggingLevel getLoggingLevel(log4net.Core.Level lvlIn)
    //    {
    //        if (lvlIn == log4net.Core.Level.All)
    //        {
    //            return LoggingLevel.All;
    //        }
    //        else if (lvlIn == log4net.Core.Level.Debug)
    //        {
    //            return LoggingLevel.Debug;
    //        }
    //        else if (lvlIn == log4net.Core.Level.Info)
    //        {
    //            return LoggingLevel.Info;
    //        }
    //        else if (lvlIn == log4net.Core.Level.Warn)
    //        {
    //            return LoggingLevel.Warn;
    //        }
    //        else if (lvlIn == log4net.Core.Level.Error)
    //        {
    //            return LoggingLevel.Error;
    //        }
    //        else 
    //        {
    //            return LoggingLevel.Off;
    //        }
    //    }
    //    private static String getSqlExceptionStr(SqlException exception)
    //    {
    //        StringBuilder errorMessages = new StringBuilder();
    //        for (int i = 0; i < exception.Errors.Count; i++)
    //        {
    //            errorMessages.Append("\t\tIndex #" + i + "\n" +
    //                "\t\t\tMessage: " + exception.Errors[i].Message + "\n" +
    //                "\t\t\tClass: " + exception.Errors[i].Class.ToString() + "\n" +
    //                "\t\t\tState: " + exception.Errors[i].State.ToString() + "\n" +
    //                "\t\t\tNumber: " + exception.Errors[i].Number.ToString() + "\n" +
    //                "\t\t\tSource: " + exception.Errors[i].Source + "\n" +
    //                "\t\t\tServer: " + exception.Errors[i].Server + "\n" +
    //                "\t\t\tProcedure: " + exception.Errors[i].Procedure + "\n" +
    //                "\t\t\tLineNumber: " + exception.Errors[i].LineNumber.ToString());
    //        }
    //        return errorMessages.ToString();
    //    }
    //    private static String getMsgStr(object[] paramsIn)
    //    {
    //        // Construct a string build object.
    //        StringBuilder sb = new StringBuilder();

    //        if(paramsIn == null)
    //        {
    //            sb.Append("");
    //        }
    //        else 
    //        { 
    //            foreach (object o in paramsIn)
    //            {
    //                if (o != null)
    //                {
    //                    // Append a spacer.
    //                    sb.Append(" ");

    //                    // Try to keep the more likely types toward the top for efficiency.
    //                    Type T = o.GetType();
    //                    if (T == typeof(string)) { sb.Append(o); continue; }
    //                    if (T == typeof(StringBuilder)) { sb.Append(o.ToString()); continue; }
    //                    if (T == typeof(SqlException)) 
    //                    {
    //                        sb.Append("\n\tSQL Exception\n");
    //                        sb.Append(getSqlExceptionStr((SqlException)o));
    //                        continue; 
    //                    }
    //                    if (T == typeof(int)) { sb.Append((int)o); continue; }
    //                    if (T == typeof(uint)) { sb.Append((uint)o); continue; }
    //                    if (T == typeof(bool)) { sb.Append((bool)o); continue; }
    //                    if (T == typeof(double)) { sb.Append((double)o); continue; }
    //                    if (T == typeof(DateTime)) { sb.Append(((DateTime)o).ToLocalTime().ToFileTime()); continue; } // Since ToFileTime() adds in the UTC offset, call ToLocalTime() to take it out.
    //                    if (T == typeof(sbyte)) { sb.Append((sbyte)o); continue; }
    //                    if (T == typeof(short)) { sb.Append((short)o); continue; }
    //                    if (T == typeof(long)) { sb.Append((long)o); continue; }
    //                    if (T == typeof(byte)) { sb.Append((byte)o); continue; }
    //                    if (T == typeof(char)) { sb.Append((char)o); continue; }
    //                    if (T == typeof(ushort)) { sb.Append((ushort)o); continue; }
    //                    if (T == typeof(ulong)) { sb.Append((ulong)o); continue; }
    //                    if (T == typeof(float)) { sb.Append((float)o); continue; }
    //                    if (T == typeof(decimal)) { sb.Append((decimal)o); continue; }
    //                    if (T == typeof(Guid)) { sb.Append((Guid)o); continue; }

    //                    // If we get this far, the object's type is not one we support.
    //                    // Note that if the user called Log() instead, he would discover this
    //                    // at compile time.
    //                    string Msg = "<unsupported type '" + T.ToString() + "' passed to Log()>";
    //                    sb.Append(Msg);
    //                }
    //            }
    //        }

    //        return sb.ToString();
    //    }
    //    private bool isLevelEnabled(LoggingLevel lvlIn)
    //    {
    //        bool isEnabled = false;
    //        switch (lvlIn)
    //        {
    //            case LoggingLevel.Debug:
    //                isEnabled = m_Log.IsDebugEnabled;
    //                break;

    //            case LoggingLevel.Info:
    //                isEnabled = m_Log.IsInfoEnabled;
    //                break;

    //            case LoggingLevel.Warn:
    //                isEnabled = m_Log.IsWarnEnabled;
    //                break;

    //            case LoggingLevel.Error:
    //                isEnabled = m_Log.IsErrorEnabled;
    //                break;

    //            default:
    //                isEnabled = false;
    //                break;
    //        }
    //        return isEnabled;
    //    }
    //    private void levelLog(LoggingLevel lvlIn, String msgIn)
    //    {
    //        switch (lvlIn)
    //        {
    //            case LoggingLevel.Debug:
    //                m_Log.Debug(msgIn);
    //                break;

    //            case LoggingLevel.Info:
    //                m_Log.Info(msgIn);
    //                break;

    //            case LoggingLevel.Warn:
    //                m_Log.Warn(msgIn);
    //                break;

    //            case LoggingLevel.Error:
    //                m_Log.Error(msgIn);
    //                break;

    //            default:
    //                // Do nothing.
    //                break;
    //        }
    //    }
    //    #endregion

    //    #region Ctors
    //    public Log4Net()
    //    {
    //        // Setup default log level and file name.
    //        m_Level = getLevel(LoggingLevel.Off);
    //        m_LogFileName = defaultFileName;

    //        // Create log4net objects.
    //        m_Log = log4net.LogManager.GetLogger(typeof(Log4Net));
    //        m_RollingFileAppender = new log4net.Appender.RollingFileAppender();

    //        // Initialize the log4net objects.
    //        m_RollingFileAppender.RollingStyle = log4net.Appender.RollingFileAppender.RollingMode.Size;
    //        m_RollingFileAppender.MaximumFileSize = "1MB";
    //        m_RollingFileAppender.MaxSizeRollBackups = 5;
    //        m_RollingFileAppender.Layout = patternLayout;
    //        m_RollingFileAppender.Threshold = m_Level;
    //        m_RollingFileAppender.File = m_LogFileName;
    //        m_RollingFileAppender.AppendToFile = true;

    //        // Activate the log4net settings.
    //        m_RollingFileAppender.ActivateOptions();
    //        log4net.Config.BasicConfigurator.Configure(m_RollingFileAppender);
    //    }
    //    #endregion

    //    #region Properties
    //    public LoggingLevel Level
    //    {
    //        get
    //        {
    //            lock (m_Lock)
    //            {
    //                return getLoggingLevel(m_Level);
    //            }
    //        }
    //        set
    //        {
    //            lock (m_Lock)
    //            {
    //                bool isChanged = m_Level != getLevel(value);
    //                if (isChanged)
    //                {
    //                    m_Level = getLevel(value);
    //                    m_RollingFileAppender.Threshold = m_Level;
    //                    m_RollingFileAppender.ActivateOptions();
    //                }
    //            }
    //        }
    //    }
    //    public string LogFileName
    //    {
    //        get
    //        {
    //            lock(m_Lock)
    //            {
    //                return m_RollingFileAppender.File;
    //            }
    //        }
    //        set
    //        {
    //            if(value != null)
    //            {
    //                lock(m_Lock)
    //                {
    //                    bool isChanged = String.Compare(value, m_RollingFileAppender.File, true) != 0;
    //                    if(isChanged)
    //                    {
    //                        m_RollingFileAppender.File = value;
    //                        m_RollingFileAppender.ActivateOptions();
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    #endregion

    //    #region Methods
    //    /// <summary>
    //    /// Always logs the message string.
    //    /// </summary>
    //    /// <param name="msgIn"></param>
    //    public void Log(
    //            String msgIn
    //        )
    //    {
    //        lock (m_Lock)
    //        {
    //            m_Log.Fatal(msgIn);
    //        }
    //    }
    //    /// <summary>
    //    /// Always logs the messages.
    //    /// </summary>
    //    /// <param name="itemsIn">Messages to log</param>
    //    public void Log(
    //            params object[] itemsIn
    //        )
    //    {
    //        lock (m_Lock)
    //        {
    //            m_Log.Fatal(getMsgStr(itemsIn));
    //        }
    //    }
    //    /// <summary>
    //    /// Logs if the test evaluates to true.
    //    /// </summary>
    //    /// <param name="testIn">boolean test</param>
    //    /// <param name="msgIn">log message</param>
    //    public void LogIf(
    //            bool testIn,
    //            String msgIn
    //        )
    //    {
    //        if (testIn)
    //        {
    //            lock (m_Lock)
    //            {
    //                m_Log.Fatal(msgIn);
    //            }
    //        }
    //    }
    //    /// <summary>
    //    /// Logs if the test evaluates to true.
    //    /// </summary>
    //    /// <param name="testIn">boolean test</param>
    //    /// <param name="itemsIn">log messages</param>
    //    public void LogIf(
    //            bool testIn, 
    //            params object[] itemsIn
    //        )
    //    {
    //        if (testIn)
    //        {
    //            lock (m_Lock)
    //            {
    //                m_Log.Fatal(getMsgStr(itemsIn));
    //            }
    //        }
    //    }
    //    /// <summary>
    //    /// Level log message
    //    /// </summary>
    //    /// <param name="lvlIn">Log level</param>
    //    /// <param name="msgIn">Log message</param>
    //    public void LogLevel(
    //            LoggingLevel lvlIn,
    //            String msgIn
    //        )
    //    {
    //        lock (m_Lock)
    //        {
    //            if (isLevelEnabled(lvlIn))
    //            {
    //                levelLog(lvlIn, msgIn);
    //            }
    //        }
    //    }
    //    /// <summary>
    //    /// Level log message
    //    /// </summary>
    //    /// <param name="lvlIn">Log level</param>
    //    /// <param name="itemsIn">Log message</param>
    //    public void LogLevel(
    //            LoggingLevel lvlIn,
    //            params object[] itemsIn
    //        )
    //    {
    //        lock (m_Lock)
    //        {
    //            if (isLevelEnabled(lvlIn))
    //            {
    //                levelLog(lvlIn, getMsgStr(itemsIn));
    //            }
    //        }
    //    }
    //    #endregion

    //    #region IDisposable methods
    //    public void Dispose()
    //    {
    //        log4net.LogManager.Shutdown();
    //    }
    //    #endregion
    //}
    //#endregion

    public class LogX
    {
        private static bool isTracerXInit = false;
        public BBS.TracerX.Logger m_logX = null;


        public LogX(string moduleName)
        {
            m_logX = BBS.TracerX.Logger.GetLogger(moduleName);                 
        }

        public static void Initialize()
        {
            Initialize("");
        }

        public static void Initialize(string fileName)
        {
            if (!isTracerXInit)
            {
                BBS.TracerX.Logger.Xml.Configure();
                if (!string.IsNullOrEmpty(fileName))
                {
                    BBS.TracerX.Logger.FileLogging.Name = fileName;
                }
                isTracerXInit = BBS.TracerX.Logger.FileLogging.Open();
            }
        }

        public BBS.TracerX.Logger loggerX
        {
            get { return m_logX; }
        }



    }

    //public static class DiagLog
    //{
    //    #region Singleton logger object
    //    private static Log4Net m_Logger;
    //    private static Log4Net logger
    //    {
    //        get
    //        {
    //            if (m_Logger == null)
    //            {
    //                lock (typeof(Log4Net))
    //                {
    //                    if (m_Logger == null)
    //                    {
    //                        m_Logger = new Log4Net();
    //                    }
    //                }
    //            }
    //            return m_Logger;
    //        }
    //    }
    //    #endregion

    //    #region Properties
    //    public static LoggingLevel Level
    //    {
    //        get { return logger.Level; }
    //        set { logger.Level = value; }
    //    }
    //    public static string LogFileName
    //    {
    //        get { return logger.LogFileName; }
    //        set { logger.LogFileName = value; }
    //    }
    //    #endregion

    //    #region Logging Methods
    //    public static void Log(String msgIn)
    //    {
    //        logger.Log(msgIn);
    //    }
    //    public static void Log(params object [] itemsIn)
    //    {
    //        logger.Log(itemsIn);
    //    }
    //    public static void LogIf(
    //            bool testIn,
    //            String msgIn
    //        )
    //    {
    //        logger.LogIf(testIn, msgIn);
    //    }
    //    public static void LogIf(
    //            bool testIn,
    //            params object[] itemsIn
    //        )
    //    {
    //        logger.LogIf(testIn, itemsIn);
    //    }
    //    public static void LogLevel(
    //            LoggingLevel lvlIn,
    //            String msgIn
    //        )
    //    {
    //        logger.LogLevel(lvlIn, msgIn);
    //    }
    //    public static void LogLevel(
    //            LoggingLevel lvlIn,
    //            params object[] itemsIn
    //        )
    //    {
    //        logger.LogLevel(lvlIn, itemsIn);
    //    }
    //    public static void LogError(String msgIn)
    //    {
    //        LogLevel(LoggingLevel.Error, msgIn);
    //    }
    //    public static void LogError(params object[] itemsIn)
    //    {
    //        LogLevel(LoggingLevel.Error, itemsIn);
    //    }
    //    public static void LogWarn(String msgIn)
    //    {
    //        LogLevel(LoggingLevel.Warn, msgIn);
    //    }
    //    public static void LogWarn(params object[] itemsIn)
    //    {
    //        LogLevel(LoggingLevel.Warn, itemsIn);
    //    }
    //    public static void LogInfo(String msgIn)
    //    {
    //        LogLevel(LoggingLevel.Info, msgIn);
    //    }
    //    public static void LogInfo(params object[] itemsIn)
    //    {
    //        LogLevel(LoggingLevel.Info, itemsIn);
    //    }
    //    public static void LogDebug(String msgIn)
    //    {
    //        LogLevel(LoggingLevel.Debug, msgIn);
    //    }
    //    public static void LogDebug(params object[] itemsIn)
    //    {
    //        LogLevel(LoggingLevel.Debug, itemsIn);
    //    }
    //    #endregion
    //}

    //public sealed class LogBlock : IDisposable
    //{
    //    #region Fields
    //    private string m_BlockName;

    //    #endregion

    //    #region Ctors
    //    public LogBlock(string blockName)
    //    {
    //        m_BlockName = blockName;
    //        DiagLog.LogDebug("Start Block - " + m_BlockName);
    //    }

    //    #endregion

    //    #region IDisposable methods
    //    public void Dispose()
    //    {
    //        DiagLog.LogDebug("End Block - " + m_BlockName);
    //    }
    //    #endregion
    //}
}
