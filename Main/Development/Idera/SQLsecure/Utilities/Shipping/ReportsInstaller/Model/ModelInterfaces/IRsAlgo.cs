namespace Idera.Common.ReportsInstaller.Model.ModelInterfaces
{
	/// <summary>
	/// The abstraction of the algorithm or 'visitor' that is being executed by an 
	/// IProxyFactory.
	/// </summary>
	public interface IRsAlgo
	{
		/// <summary>
		/// Executes a Reporting Services method.
		/// Uses the visitor design pattern.
		/// </summary>
		/// <param name="host">The IProxyFactory that the algorithm or 'visitor' is executed on.</param>
		/// <param name="param">The parameters required to execute the visitor.</param>
		/// <returns>The execution of the visitor.</returns>
		object Execute(IProxyFactory host, params object[] param);
	}
}
