using System ;
using System.Net ;
using System.Web.Services.Protocols ;

namespace Idera.Common.ReportsInstaller.Model
{
	/// <summary>
	/// Creates an error message.
	/// </summary>
	public class ErrorFactory
	{
		// Singleton Pattern
		private ErrorFactory(){}
		public static ErrorFactory Singleton { get { return Nested.Singleton;}}
		class Nested
		{
			static Nested() {}
			internal static readonly ErrorFactory Singleton = new ErrorFactory();
		}

		/// <summary>
		/// Creates an error message based on the exception.
		/// </summary>
		/// <param name="messageText">The initial message.</param>
		/// <param name="ex">The exception thrown.</param>
		/// <returns>The error message.</returns>
		public string ShowErrorMessage (string messageText, Exception ex)
		{
			if (ex != null)
			{
				if (ex is SoapException)
				{
					switch (((SoapException)ex).Detail["ErrorCode"].InnerText)
					{
						case "rsReportParameterValueNotSet":
							messageText += "\r\n" + "The report parameters do not match."; break;
						case "rsItemNotFound":
							messageText += "\r\n" + "Wrong report or report path. Please check the reportFolder element in the application configuration file."; break;
						case "rsItemAlreadyExists":
							messageText += "\r\n" + "Item already exists. Please delete the item from the report manager interface to recreate, otherwise existing item will be used."; break;
						default:
							messageText += "\r\n" + "Exception Trace:\r\n" + ((SoapException)ex).Detail.InnerXml;
							break;
					}
											
					
				}

				else if (ex is WebException) 
				{
					messageText += "\r\n" + "Cannot communicate with the Report Server. Please check the reportServer element in the application configuration file.";
					messageText += "\r\n" + "Exception Trace:\r\n" + ex.ToString();
				}
				else 
				{
					messageText += "\r\n" + ex.ToString();
				}

			}
			return messageText;
			
		}
	}
}
