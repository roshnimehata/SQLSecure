using Idera.Common.ReportsInstaller.rs;

namespace Idera.Common.ReportsInstaller.Model.ModelInterfaces
{
	/// <summary>
	/// The abstraction of a connection to Reporting Services.
	/// </summary>
	public interface IProxyFactory
	{
		/// <summary>
		/// Creates an instance of ReportingService.
		/// </summary>
		/// <param name="server">The name of the server that has Reporting Service.</param>
		/// <param name="virtualRoot">The virtual directory in the server.</param>
		/// <returns>ReportingService object.</returns>
		ReportingService Proxy(string server, string virtualRoot);

		/// <summary>
		/// Executes a Reporting Services method.
		/// Uses the visitor design pattern.
		/// </summary>
		/// <param name="algo">The algorithm or 'visitor' that is being executed.</param>
		/// <param name="param">The parameters required to execute the visitor.</param>
		/// <returns>The execution of the visitor.</returns>
		object Execute(IRsAlgo algo, params object[] param);
	}
}
