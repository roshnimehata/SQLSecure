using Idera.Common.ReportsInstaller.Model.ModelInterfaces ;
using Idera.Common.ReportsInstaller.rs;

namespace Idera.Common.ReportsInstaller.Model.RsAdapter
{
	/// <summary>
	/// Gets a list of children of a specified folder.
	/// </summary>
	public class ListChildrenAlgo : IRsAlgo
	{
		// Singleton Pattern
		private ListChildrenAlgo(){}
		public static IRsAlgo Singleton { get { return Nested.ONLY;}}
		class Nested
		{
			static Nested() {}
			internal static readonly ListChildrenAlgo ONLY = new ListChildrenAlgo();
		}

		/// <summary>
		/// Gets a list of children of a specified folder.
		/// </summary>
		/// <param name="host">The IProxyFactory that the algorithm or 'visitor' is executed on.</param>
		/// <param name="param[0]">The name of the server that has Reporting Service.</param>
		/// <param name="param[1]">The virtual directory in the server.</param>
		/// <param name="param[2]">The full path name of the parent folder. </param>
		/// <param name="param[3]">Whether the search is recursive</param>
		/// <returns>An array of CatalogItem[] objects. If no children exist, this 
		/// method returns an empty CatalogItem object.</returns>
		public object Execute(IProxyFactory host, params object[] param)
		{
			string server = (string)param[0];
			string virtualRoot = (string)param[1];
			string parentPath = (string)param[2];
			bool recur = (bool)param[3];

			ReportingService rs =  host.Proxy(server, virtualRoot);
			CatalogItem[] items = rs.ListChildren(parentPath, recur);
			return items;
		}
	}
}
