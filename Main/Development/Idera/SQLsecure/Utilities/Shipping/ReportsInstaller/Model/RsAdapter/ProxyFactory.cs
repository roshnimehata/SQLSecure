using System;
using Idera.Common.ReportsInstaller.rs;
using Idera.Common.ReportsInstaller.ErrorLogging;
using Idera.Common.ReportsInstaller.Model.ModelInterfaces;

namespace Idera.Common.ReportsInstaller.Model.RsAdapter
{
	/// <summary>
	/// Creates an instance of ReportingService with a regular connection because there 
	/// is no SSL.
	/// </summary>
	public class ProxyFactory : IProxyFactory
	{
		/// <summary>
		/// The instance of ReportingService.
		/// </summary>
		private ReportingService rsProxy;

		// Singleton Pattern
		private ProxyFactory()
		{
			ErrorLog.Singleton.LogSuccess("Entered ProxyFactory constructor.");
			rsProxy = new ReportingService();
			ErrorLog.Singleton.LogSuccess("Initialized ReportingService object in ProxyFactory.");
		}
		public static ProxyFactory Singleton { get { return Nested.ONLY;}}
		class Nested
		{
			static Nested() {}
			internal static readonly ProxyFactory ONLY = new ProxyFactory();
		}

		/// <summary>
		/// Creates an instance of ReportingService.
		/// </summary>
		/// <param name="server">The name of the server that has Reporting Service.</param>
		/// <param name="virtualRoot">The virtual directory in the server.</param>
		/// <returns>ReportingService object.</returns>
		public ReportingService Proxy(string server, string virtualRoot)
		{
			rsProxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
			rsProxy.Url = String.Format( "http://{0}/{1}/ReportService.asmx",
				server, virtualRoot);
			return rsProxy;	
		}

		/// <summary>
		/// Executes a Reporting Services method.
		/// Uses the visitor design pattern.
		/// </summary>
		/// <param name="algo">The algorithm or 'visitor' that is being executed.</param>
		/// <param name="param">The parameters required to execute the visitor.</param>
		/// <returns>The execution of the visitor.</returns>
		public object Execute(IRsAlgo algo, params object[] param)
		{
			return algo.Execute(this, param);
		}
	}
}
