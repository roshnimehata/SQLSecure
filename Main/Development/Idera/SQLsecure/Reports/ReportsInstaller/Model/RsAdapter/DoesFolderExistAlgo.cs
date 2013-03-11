using Idera.Common.ReportsInstaller.Model.ModelInterfaces ;
using Idera.Common.ReportsInstaller.rs;

namespace Idera.Common.ReportsInstaller.Model.RsAdapter
{
	/// <summary>
	/// Determines if a folder already exists.
	/// </summary>
	public class DoesFolderExistAlgo : IRsAlgo
	{
		// Singleton Pattern
		private DoesFolderExistAlgo(){}
		public static IRsAlgo Singleton { get { return Nested.ONLY;}}
		class Nested
		{
			static Nested() {}
			internal static readonly DoesFolderExistAlgo ONLY = new DoesFolderExistAlgo();
		}

		/// <summary>
		/// Determines if a folder already exists.
		/// </summary>
		/// <param name="host">The IProxyFactory that the algorithm or 'visitor' is executed on.</param>
		/// <param name="param[0]">The name of the server that has Reporting Service.</param>
		/// <param name="param[1]">The virtual directory in the server.</param>
		/// <param name="param[2]">The full path name of the folder.</param>
		/// <returns>True if folder exists; False for otherwise</returns>
		public object Execute(IProxyFactory host, params object[] param)
		{
			string server = (string)param[0];
			string virtualRoot = (string)param[1];
			string folderPath = (string)param[2];

			bool folderExists = false;
			ReportingService rs = host.Proxy(server, virtualRoot);
			ItemTypeEnum itemType = rs.GetItemType( folderPath );
				
			if ( itemType == ItemTypeEnum.Folder )
			{
				folderExists = true;
			}
			return folderExists;
		}
	}
}
