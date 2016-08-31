using Idera.Common.ReportsInstaller.Model.ModelInterfaces ;
using Idera.Common.ReportsInstaller.rs ;

namespace Idera.Common.ReportsInstaller.Model.RsAdapter
{
	/// <summary>
	/// Adds a folder to the report server database.
	/// </summary>
	public class CreateSubFoldersAlgo : IRsAlgo
	{
		// Singleton Pattern
		private CreateSubFoldersAlgo(){}
		public static IRsAlgo Singleton { get { return Nested.ONLY;}}
		class Nested
		{
			static Nested() {}
			internal static readonly CreateSubFoldersAlgo ONLY = new CreateSubFoldersAlgo();
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

			int index = folderName.IndexOf(@"/");

			if (index < 0)
			{
				if ((bool)host.Execute(DoesFolderExistAlgo.Singleton, server, virtualRoot,
					parentPath+folderName))
				{
					return true;
				}
				else
				{
					if (parentPath.Equals(@"/"))
					{
						return host.Execute(CreateFolderAlgo.Singleton, server, virtualRoot, parentPath,
							folderName, folderDescription);
					}
					else
					{
						return host.Execute(CreateFolderAlgo.Singleton, server, virtualRoot, parentPath.Remove(parentPath.Length-1, 1),
							folderName, folderDescription);
					}
				}
			}
			else
			{
				string sub = folderName.Substring(0, index);
				string newFolder = folderName.Remove(0, index+1);

				host.Execute(CreateSubFoldersAlgo.Singleton, server, virtualRoot, parentPath,
					sub, folderDescription);

				return host.Execute(CreateSubFoldersAlgo.Singleton, server, virtualRoot,
					parentPath+sub+@"/", newFolder, folderDescription);
			}
		}
	}
}
