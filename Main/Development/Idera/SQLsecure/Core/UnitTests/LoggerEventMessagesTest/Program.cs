/*
 * TESTING READ ME
 * 
 *  1. Register message DLL
 *      - Start the VS command prompt
 *      - Goto SQLsecure\bin\Debug (or Release) directory
 *      - Enter "InstallUtil Idera.SQLSecure.Core.Logger.dll"
 * 
 *  2. Run the test
 * 
 *  3. Verify that there are 4 event log enteries
 * 
 *  4. Remove the message DLL
 *      - Start the VS command prompt
 *      - Goto SQLsecure\bin\Debug (or Release) directory
 *      - Enter "InstallUtil /u Idera.SQLSecure.Core.Logger.dll"
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;

using Idera.SQLsecure.Core.Logger;

namespace LoggerEventMessagesTest
{
    class Program
    {
        static void eventMsgTest()
        {
            Idera.SQLsecure.Core.Logger.AppLog.WriteAppEvent(Idera.SQLsecure.Core.Logger.SQLsecureEvent.TestEvent1,
                Idera.SQLsecure.Core.Logger.SQLsecureCat.TestCategory1, "param1");
            Idera.SQLsecure.Core.Logger.AppLog.WriteAppEvent(Idera.SQLsecure.Core.Logger.SQLsecureEvent.TestEvent2,
                Idera.SQLsecure.Core.Logger.SQLsecureCat.TestCategory1, "param1", "param2");
            Idera.SQLsecure.Core.Logger.AppLog.WriteAppEvent(Idera.SQLsecure.Core.Logger.SQLsecureEvent.TestEvent3,
                Idera.SQLsecure.Core.Logger.SQLsecureCat.TestCategory1, "param1", "param2", "param3");
            String[] p = { "p1", "p2", "p3", "p4", "p5" };
            Idera.SQLsecure.Core.Logger.AppLog.WriteAppEvent(Idera.SQLsecure.Core.Logger.SQLsecureEvent.TestEventN,
                Idera.SQLsecure.Core.Logger.SQLsecureCat.TestCategory1, p);
        }

        static void diagLogTest()
        {
            DiagLog.Log("log always");
            DiagLog.LogLevel(LoggingLevel.DEBUG, "debugMsg");
            DiagLog.LogLevel(LoggingLevel.INFO, "infoMsg");
            DiagLog.LogLevel(LoggingLevel.WARN, "warnMsg");
            DiagLog.LogLevel(LoggingLevel.ERROR, "errMsg");

            DiagLog.Level = LoggingLevel.ERROR;
            DiagLog.Log("ERRROR level on");
            DiagLog.LogLevel(LoggingLevel.DEBUG, "debugMsg");
            DiagLog.LogLevel(LoggingLevel.INFO, "infoMsg");
            DiagLog.LogLevel(LoggingLevel.WARN, "warnMsg");
            DiagLog.LogLevel(LoggingLevel.ERROR, "errMsg");

            DiagLog.Level = LoggingLevel.ALL;
            DiagLog.Log("ALL level on");
            DiagLog.LogLevel(LoggingLevel.DEBUG, "debugMsg");
            DiagLog.LogLevel(LoggingLevel.INFO, "infoMsg");
            DiagLog.LogLevel(LoggingLevel.WARN, "warnMsg");
            DiagLog.LogLevel(LoggingLevel.ERROR, "errMsg");

            string fileName = DiagLog.LogFileName;
            DiagLog.LogFileName = @"C:\public\Test2.log";
            fileName = DiagLog.LogFileName;
            DiagLog.Level = LoggingLevel.ALL;
            DiagLog.Log("ALL level on");
            DiagLog.LogLevel(LoggingLevel.DEBUG, "debugMsg");
            DiagLog.LogLevel(LoggingLevel.INFO, "infoMsg");
            DiagLog.LogLevel(LoggingLevel.WARN, "warnMsg");
            DiagLog.LogLevel(LoggingLevel.ERROR, "errMsg");
                                        
        }

        static void Main(string[] args)
        {
            diagLogTest();
        }
    }
}
