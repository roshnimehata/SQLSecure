using Idera.Common.ReportsInstaller.Model.ModelInterfaces ;
using Idera.Common.ReportsInstaller.rs;

namespace Idera.Common.ReportsInstaller.Model.RsAdapter
{
	/// <summary>
	/// Gets a list of children of a specified folder.
	/// </summary>
	public class DoesDataSourceExistAlgo : IRsAlgo
	{
		// Singleton Pattern
		private DoesDataSourceExistAlgo(){}
		public static IRsAlgo Singleton { get { return Nested.ONLY;}}
		class Nested
		{
			static Nested() {}
			internal static readonly DoesDataSourceExistAlgo ONLY = new DoesDataSourceExistAlgo();
		}

		/// <summary>
		/// Gets a list of children of a specified folder.
		/// </summary>
		/// <param name="host">The IProxyFactory that the algorithm or 'visitor' is executed on.</param>
		/// <param name="param[0]">The name of the server that has Reporting Service.</param>
		/// <param name="param[1]">The virtual directory in the server.</param>
		/// <param name="param[2]">The full path name of the parent folder. </param>
		/// <returns>True if data source exists; false if doesn't.</returns>
		public object Execute(IProxyFactory host, params object[] param)
		{
			string server = (string)param[0];
			string virtualRoot = (string)param[1];
			string parentPath = (string)param[2];

			ReportingService rs =  host.Proxy(server, virtualRoot);
			CatalogItem[] items = rs.ListChildren(parentPath, false);
			
			bool exists = false;

			foreach (CatalogItem ci in items)
			{
				if (ci.Type == ItemTypeEnum.DataSource)
				{
					exists = true;
				}
			}

			return exists;
		}
	}
}
