/******************************************************************
 * Name: EventLog.cs
 *
 * Description: NT event logging class.
 *
 *
 * Assemblies/DLLs needed: 
 *          Idera.SQLsecure.Core.Logger.dll
 *          Idera.SQLsecure.Core.EventMessages.dll
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Idera.SQLsecure.Core.Logger
{
    /// <summary>
    /// Static class for logging NT application event logs.
    /// </summary>
    public static class AppLog
    {
        #region EventLogger Object
        private static LogX logX = new LogX("Idera.SQLsecure.Core.Logger.AppLog");
        private static EventLog m_AppEventLog;
        private static EventLog appEventLog
        {
            get
            {
                if (m_AppEventLog == null)
                {
                    lock (typeof(EventLog))
                    {
                        if (m_AppEventLog == null)
                        {
                            m_AppEventLog = new EventLog("Application", ".", "Idera.SQLsecure");
                        }
                    }
                }
                return m_AppEventLog;
            }
        }
        #endregion

        #region Event logging functions
        /*
        /// <summary>
        /// Write an event to the application log.
        /// </summary>
        /// <param name="eventIdIn">Event ID</param>
        /// <param name="categoryIdIn">Category ID</param>
        /// <param name="paramIn">String replacement param</param>
        public static void WriteAppEvent(
                SQLsecureEvent eventIdIn,
                SQLsecureCat categoryIdIn
            )
        {
            EventInstance ei = new EventInstance((long)eventIdIn, (int)categoryIdIn);
            appEventLog.WriteEvent(ei);
        }

        /// <summary>
        /// Write an event to the application log.
        /// </summary>
        /// <param name="eventIdIn">Event ID</param>
        /// <param name="categoryIdIn">Category ID</param>
        /// <param name="paramIn">String replacement param</param>
        public static void WriteAppEvent(
                SQLsecureEvent eventIdIn,
                SQLsecureCat categoryIdIn,
                String paramIn
            )
        {
            EventInstance ei = new EventInstance((long)eventIdIn, (int)categoryIdIn);
            appEventLog.WriteEvent(ei, paramIn);
        }

        /// <summary>
        /// Write an event to the application log.
        /// </summary>
        /// <param name="eventIdIn">Event ID</param>
        /// <param name="categoryIdIn">Category ID</param>
        /// <param name="param0In">String replacement param</param>
        /// <param name="param1In">String replacement param</param>
        public static void WriteAppEvent(
                SQLsecureEvent eventIdIn,
                SQLsecureCat categoryIdIn,
                String param0In,
                String param1In
            )
        {
            EventInstance ei = new EventInstance((long)eventIdIn, (int)categoryIdIn);
            String[] param = { param0In, param1In };
            appEventLog.WriteEvent(ei, param);
        }

        /// <summary>
        /// 
        /// </summary>
        /// Write an event to the application log.
        /// </summary>
        /// <param name="eventIdIn">Event ID</param>
        /// <param name="categoryIdIn">Category ID</param>
        /// <param name="param0In">String replacement param</param>
        /// <param name="param1In">String replacement param</param>
        /// <param name="param2In">String replacement param</param>
        public static void WriteAppEvent(
                SQLsecureEvent eventIdIn,
                SQLsecureCat categoryIdIn,
                String param0In,
                String param1In,
                String param2In
            )
        {
            EventInstance ei = new EventInstance((long)eventIdIn, (int)categoryIdIn);
            String[] param = { param0In, param1In, param2In };
            appEventLog.WriteEvent(ei, param);
        }
         */

        /// <summary>
        /// Write an event to the application log.
        /// </summary>
        /// <param name="eventIdIn">Event ID</param>
        /// <param name="categoryIdIn">Category ID</param>
        /// <param name="paramsIn">String replacement param</param>
        public static void WriteAppEventInfo(
                SQLsecureEvent eventIdIn,
                SQLsecureCat categoryIdIn,
                params String [] paramsIn
            )
        {
            try
            {
                EventInstance ei = new EventInstance((long)eventIdIn, (int)categoryIdIn, EventLogEntryType.Information);
                appEventLog.WriteEvent(ei, paramsIn);
            }
            catch (Exception ex)
            {
                logX.loggerX.Error("Error writing to Application Event log - " + ex.Message);
            }
        }

        /// <summary>
        /// Write an event to the application log.
        /// </summary>
        /// <param name="eventIdIn">Event ID</param>
        /// <param name="categoryIdIn">Category ID</param>
        /// <param name="paramsIn">String replacement param</param>
        public static void WriteAppEventError(
                SQLsecureEvent eventIdIn,
                SQLsecureCat categoryIdIn,
                params String[] paramsIn
            )
        {
            try
            {
                EventInstance ei = new EventInstance((long)eventIdIn, (int)categoryIdIn, EventLogEntryType.Error);
                appEventLog.WriteEvent(ei, paramsIn);
            }
            catch( Exception ex )
            {
                logX.loggerX.Error("Error writing to Application Event log - " + ex.Message);
            }
        }

        /// <summary>
        /// Write an event to the application log.
        /// </summary>
        /// <param name="eventIdIn">Event ID</param>
        /// <param name="categoryIdIn">Category ID</param>
        /// <param name="paramsIn">String replacement param</param>
        public static void WriteAppEventWarning(
                SQLsecureEvent eventIdIn,
                SQLsecureCat categoryIdIn,
                params String[] paramsIn
            )
        {
            try
            {
                EventInstance ei = new EventInstance((long)eventIdIn, (int)categoryIdIn, EventLogEntryType.Warning);
                appEventLog.WriteEvent(ei, paramsIn);
            }
            catch (Exception ex)
            {
                logX.loggerX.Error("Error writing to Application Event log - " + ex.Message);
            }
        }


    #endregion
    }
}
