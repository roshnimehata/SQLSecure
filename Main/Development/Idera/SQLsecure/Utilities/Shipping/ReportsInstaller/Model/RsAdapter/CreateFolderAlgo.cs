using Idera.Common.ReportsInstaller.Model.ModelInterfaces ;
using Idera.Common.ReportsInstaller.rs;

namespace Idera.Common.ReportsInstaller.Model.RsAdapter
{
	/// <summary>
	/// Adds a folder to the report server database.
	/// </summary>
	public class CreateFolderAlgo : IRsAlgo
	{
		// Singleton Pattern
		private CreateFolderAlgo(){}
		public static IRsAlgo Singleton { get { return Nested.ONLY;}}
		class Nested
		{
			static Nested() {}
			internal static readonly CreateFolderAlgo ONLY = new CreateFolderAlgo();
		}

		/// <summary>
		/// Adds a folder to the report server database.
		/// </summary>
		/// <param name="host">The IProxyFactory that the algorithm or 'visitor' is executed on.</param>
		/// <param name="param[0]">The name of the server that has Reporting Service.</param>
		/// <param name="param[1]">The virtual directory in the server.</param>
		/// <param name="param[2]">The full path name of the parent folder to which to add 
		/// the new folder. </param>
		/// <param name="param[3]">The name of the new folder. </param>
		/// <param name="param[4]">The value of a property for a folder.</param>
		/// <returns>True for successful; False for otherwise</returns>
		public object Execute(IProxyFactory host, params object[] param)
		{
			string server = (string)param[0];
			string virtualRoot = (string)param[1];
			string parentPath = (string)param[2];
			string folderName = (string)param[3];
			string folderDescription = (string)param[4];

			ReportingService rs =  host.Proxy(server, virtualRoot);	
			Property setProp = new Property();
			setProp.Name  = "Description";
			setProp.Value = folderDescription;
			Property[] props = new Property[1];
			props[0]=setProp;
			
			rs.CreateFolder( folderName,
				parentPath,
				props);

			return true;
		}
	}
}
