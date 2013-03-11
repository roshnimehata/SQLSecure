using System.Text;
using System.Diagnostics;

namespace Idera.Common.ReportsInstaller.ErrorLogging
{
	/// <summary>
	/// Summary description for ErrorLog.
	/// </summary>
	public class ErrorLog
	{
		private EventLog _log;
		
		private int _level;
		
		// Singleton Pattern
		private ErrorLog()
		{
//			EventLog.DeleteEventSource("ReportsInstaller", "Application");
//			EventLog.DeleteEventSource("Reports Installer", "Reports Installer Log");
			_level = 0;
			if (!EventLog.SourceExists("Reports Installer"))
			{
//				EventLog.DeleteEventSource("Reports Installer");
				EventLog.CreateEventSource("Reports Installer", "Application");
			}
//			EventLog.CreateEventSource("Reports Installer", "Application");
			_log = new EventLog("Application", ".", "Reports Installer");
		}
		public static ErrorLog Singleton { get { return Nested.ONLY;}}
		class Nested
		{
			static Nested() {}
			internal static readonly ErrorLog ONLY = new ErrorLog();
		}
		
		public void SetLogLevel(int level)
		{
			_level = level;
		}
		
		public void LogError(string comment, string message, string trace)
		{
			switch (_level)
			{
				case 0:
					{
						break;
					}
				case 1:
					{
						StringBuilder sb = new StringBuilder();
						sb.Append(comment);
						sb.Append("/r/n");
						sb.Append("Error Message: ");
						sb.Append(message);
						sb.Append("/r/n");
						sb.Append("Stack Trace: ");
						sb.Append(trace);
						sb.Append("/r/n");
						_log.WriteEntry(sb.ToString(), EventLogEntryType.Error);
						break;
					}
				case 2:
					{
						StringBuilder sb = new StringBuilder();
						sb.Append(comment);
						sb.Append("/r/n");
						sb.Append("Error Message: ");
						sb.Append(message);
						sb.Append("/r/n");
						sb.Append("Stack Trace: ");
						sb.Append(trace);
						sb.Append("/r/n");
						_log.WriteEntry(sb.ToString(), EventLogEntryType.Error);
						break;
					}
				case 3:
					{
						StringBuilder sb = new StringBuilder();
						sb.Append(comment);
						sb.Append("/r/n");
						sb.Append("Error Message: ");
						sb.Append(message);
						sb.Append("/r/n");
						sb.Append("Stack Trace: ");
						sb.Append(trace);
						sb.Append("/r/n");
						_log.WriteEntry(sb.ToString(), EventLogEntryType.Error);
						break;
					}
				default:
					{
						break;
					}
			}
		}
		
		public void LogSuccess(string comment)
		{
			switch (_level)
			{
				case 0:
				{
					break;
				}
				case 1:
				{
					break;
				}
				case 2:
				{
					break;
				}
				case 3:
				{
					_log.WriteEntry(comment, EventLogEntryType.Information);
					break;
				}
				default:
				{
					break;
				}
			}
		}
		
		public void LogWarning(string comment)
		{
			
			switch (_level)
			{
				case 0:
				{
					break;
				}
				case 1:
				{
					break;
				}
				case 2:
				{
					_log.WriteEntry(comment, EventLogEntryType.Warning);
					break;
				}
				case 3:
				{
					_log.WriteEntry(comment, EventLogEntryType.Warning);
					break;
				}
				default:
				{
					break;
				}
			}
		}
	}
}
