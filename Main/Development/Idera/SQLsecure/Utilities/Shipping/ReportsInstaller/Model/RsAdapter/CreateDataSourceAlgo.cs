using Idera.Common.ReportsInstaller.Model.ModelInterfaces ;
using Idera.Common.ReportsInstaller.rs;

namespace Idera.Common.ReportsInstaller.Model.RsAdapter
{
	/// <summary>
	/// Creates a new data source in the report server database.
	/// </summary>
	public class CreateDataSourceAlgo : IRsAlgo
	{
		// Singleton Pattern
		private CreateDataSourceAlgo(){}
		public static IRsAlgo Singleton { get { return Nested.ONLY;}}
		class Nested
		{
			static Nested() {}
			internal static readonly CreateDataSourceAlgo ONLY = new CreateDataSourceAlgo();
		}

		/// <summary>
		/// Creates a new data source in the report server database.
		/// </summary>
		/// <param name="host">The IProxyFactory that the algorithm or 'visitor' is executed on.</param>
		/// <param name="param[0]">The name of the server that has Reporting Service.</param>
		/// <param name="param[1]">The virtual directory in the server.</param>
		/// <param name="param[2]">The full path name of the parent folder that contains 
		/// the data source.</param>
		/// <param name="param[3]">The name of the data source.</param>
		/// <param name="param[4]">The value of a property in a data source.</param>
		/// <param name="param[5]">The database has the Reporting Services.</param>
		/// <param name="param[6]">The name of the server that has the database.</param>
		/// <param name="param[7]">The user name that the report server uses 
		/// to connect to a data source.</param>
		/// <param name="param[8]">The password that the report server uses to connect to 
		/// a data source.</param>
		/// <param name="param[9]">A Boolean expression that indicates whether an existing 
		/// data source with the same name in the location specified should be overwritten.</param>
		/// <returns>True for successful; False for otherwise</returns>
		public object Execute(IProxyFactory host, params object[] param)
		{
			string server = (string)param[0];
			string virtualRoot = (string)param[1];
			string parentPath = (string)param[2];
			string dataSourceName = (string)param[3];
			string dsDescription = (string)param[4];
			string reportDB = (string)param[5];
			string reportHost = (string)param[6];
			string reportUser = (string)param[7];
			string reportPassword = (string)param[8];
			bool overwrite = (bool)param[9];

			ReportingService rs =  host.Proxy(server, virtualRoot);
			Property setProp = new Property();
			setProp.Name="Description";
			setProp.Value=dsDescription;
			Property[] props = new Property[1];
			props[0]=setProp;

			// Define the data source definition.
			DataSourceDefinition definition = new DataSourceDefinition();
			definition.CredentialRetrieval = CredentialRetrievalEnum.Store;
			definition.ConnectString       = "data source="+reportHost+";initial catalog="+reportDB;
			definition.Enabled             = true;
			definition.EnabledSpecified    = true;
			definition.Extension           = "SQL";
			definition.UserName            = reportUser;
			definition.Password            = reportPassword;
			definition.Prompt			   = null;
			definition.WindowsCredentials  = true;
			
			rs.CreateDataSource( dataSourceName,
				parentPath,
				overwrite,
				definition,
				props);

			return true;
		}
	}
}
