using System ;
using System.Net ;
using Idera.Common.ReportsInstaller.Model.ModelInterfaces ;
using Idera.Common.ReportsInstaller.rs;
using Idera.Common.ReportsInstaller.ErrorLogging;

namespace Idera.Common.ReportsInstaller.Model
{
	/// <summary>
	/// Creates an instance of ReportingService with a secure connection for SSL.
	/// </summary>
	public class SslProxyFactory : IProxyFactory
	{
		/// <summary>
		/// The instance of ReportingService.
		/// </summary>
		private ReportingService _rsProxy;

		private ICertificatePolicy _policy;

		public SslProxyFactory(IViewAccessAdapter viewAdapter)
		{
			ErrorLog.Singleton.LogSuccess("enter Ssl fac");
			_rsProxy = new ReportingService();
			ErrorLog.Singleton.LogSuccess("initialize ReportingService");
			_policy = new SslValidation(viewAdapter);
			ErrorLog.Singleton.LogSuccess("initialize SslValidation");

		}
		/// <summary>
		/// Creates an instance of ReportingService.
		/// </summary>
		/// <param name="server">The name of the server that has Reporting Service.</param>
		/// <param name="virtualRoot">The virtual directory in the server.</param>
		/// <returns>ReportingService object.</returns>
		public ReportingService Proxy(string server, string virtualRoot)
		{
			
			_rsProxy.Credentials = CredentialCache.DefaultCredentials;
			_rsProxy.Url = String.Format( "https://{0}/{1}/ReportService.asmx",
				server, virtualRoot);
			ServicePointManager.CertificatePolicy = _policy;
			return _rsProxy;
			
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
