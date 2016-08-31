using System;

namespace Idera.Common.ReportsInstaller.Model
{
	/// <summary>
	/// The abstraction of the logic of the application.
	/// The model of the model-view-controller design pattern (MVC pattern).
	/// </summary>
	public interface IRsModel
	{
		/// <summary>
		/// Gives the model access to certain methods in the view.
		/// </summary>
		IViewAccessAdapter ViewAdapter
		{
			get;
		}

		/// <summary>
		/// Determines whether the user has the permission to use all of the
		/// methods in this adapter.
		/// </summary>
		/// <param name="server">The name of the server that has Reporting Service.</param>
		/// <param name="virtualRoot">The virtual directory in the server.</param>
		/// <returns>True is has requested permissions; False if doesn't</returns>
		bool HasAppropriatePermissions(string server, string virtualRoot);

		/// <summary>
		/// Creates a new data source in the report server database.
		/// </summary>
		/// <param name="server">The name of the server that has Reporting Service.</param>
		/// <param name="virtualRoot">The virtual directory in the server.</param>
		/// <param name="parentPath">The full path name of the parent 
		/// folder that contains the data source.</param>
		/// <param name="reportHost">The name of the server that has the database.</param>
		/// <param name="reportUser">The user name that the report server uses 
		/// to connect to a data source.</param>
		/// <param name="reportPassword">The password that the report server 
		/// uses to connect to a data source.</param>
		/// <param name="overwrite">A Boolean expression that indicates whether 
		/// an existing data source with the same name in the location 
		/// specified should be overwritten. </param>
		/// <returns> True for successful; False for otherwise</returns>
		bool CreateDataSource (string server, string virtualRoot, string parentPath, 
			string reportHost, string reportUser, string reportPassword, bool overwrite);

		/// <summary>
		/// Adds a folder to the report server database.
		/// </summary>
		/// <param name="server">The name of the server that has Reporting Service.</param>
		/// <param name="virtualRoot">The virtual directory in the server.</param>
		/// <param name="folderName">The name of the new folder. </param>
		/// <returns> True for successful; False for otherwise</returns>
		bool CreateFolder (string server, string virtualRoot, string folderName);

		/// <summary>
		/// Determines if a folder already exists.
		/// </summary>
		/// <param name="server">The name of the server that has Reporting Service.</param>
		/// <param name="virtualRoot">The virtual directory in the server.</param>
		/// <param name="folderPath">The path name of the folder.</param>
		/// <returns>True if folder exists; False for otherwise</returns>
		bool DoesFolderExist (string server, string virtualRoot, string folderPath );

		/// <summary>
		/// Adds a new report to the report server database.
		/// </summary>
		/// <param name="server">The name of the server that has Reporting Service.</param>
		/// <param name="virtualRoot">The virtual directory in the server.</param>
		/// <param name="folderPath">The full path name of the folder.</param>
		/// <param name="reportName">The name of the new report.</param>
		/// <returns>True for successful; False for otherwise</returns>
		bool DeployReport(string server, string virtualRoot,string folderPath, 
			string reportName);

		/// <summary>
		/// Generates a list of Reporting Service Folders
		/// </summary>
		/// <param name="server">The name of the server that has Reporting Service.</param>
		/// <param name="virtualRoot">The virtual directory in the server.</param>
		void PopulateFolderList(string server, string virtualRoot);

		/// <summary>
		/// Generates a list of Reporting Service folders.
		/// </summary>
		/// <param name="server">The name of the server that has Reporting Service.</param>
		/// <param name="virtualRoot">The virtual directory in the server.</param>
		/// <param name="folderPath">The full path name of the parent folder. </param>
		/// <returns>True if data source exists; false if doesn't.</param>
		bool DoesDataSourceExist(string server, string virtualRoot, string folderPath);

		/// <summary>
		/// Generates a list of reports in the specified folder as well as the reports
		/// that have not been installed yet.
		/// </summary>
		/// <param name="server">The name of the server that has Reporting Service.</param>
		/// <param name="virtualRoot">The virtual directory in the server.</param>
		/// <param name="folderPath">The full path name of the folder. </param>
		void ReportsFoundInFolder(string server, string virtualRoot, string folderPath);

		/// <summary>
		/// Verifies the url of the host computer or site.
		/// </summary>
		/// <param name="computerName">The name of the computer.</param>
		/// <returns>True if valid connection; false if not.</returns>
		bool TestHostComputer(string computerName);

		/// <summary>
		/// Verifies the url of the Report Manager.
		/// </summary>
		/// <param name="computerName">The name of the computer.</param>
		/// <param name="managerName">The name of the report manager.</param>
		/// <returns>True if valid connection; false if not.</returns>
		bool TestReportManager(string computerName, string managerName);

		/// <summary>
		/// Verifies the url of the Report Manager.
		/// </summary>
		/// <param name="computerName">The name of the computer.</param>
		/// <param name="directoryName">The name of the report server.</param>
		/// <returns>True if valid connection; false if not.</returns>
		bool TestReportServer(string computerName, string directoryName);

		/// <summary>
		/// Verifies the existence of the database in the constructor in the specified
		/// server.
		/// </summary>
		/// <param name="serverName">The name of the server.</param>
		bool TestConnection(string serverName);

		/// <summary>
		/// Tries to insert the server name and folder name into the specified database.
		/// </summary>
		/// <param name="serverName">The name of the server that has the specified database.</param>
		/// <param name="server">The name of the server that has reporting services.</param>
		/// <param name="folder">The name of the folder that the reports are stored in.</param>
		bool InsertServerAndFolder(string serverName, string server, string folder,
			string reportManager);

		/// <summary>
		/// Loads perferences from local registry.
		/// </summary>
		/// <param name="name">The name of the value to retrieve.</param>
		/// <param name="defaultValue">The value to return if the name does not
		/// exist in the registry.</param>
		/// <returns>retrieved preference</returns>
		string ReadPreferences(string name, object defaultValue);

		/// <summary>
		/// Writes perferences from local registry.
		/// </summary>
		/// <param name="name">The name of the value to store the data in.</param>
		/// <param name="newValue">The data to store.</param>
		void WritePreferences(string name, object newValue);

		/// <summary>
		/// Loads reports data (name, file name and description) from the rdl.xml file.
		/// </summary>
		bool ReportDescriptor();

		/// <summary>
		/// Checks the local computer for all reports on the rdl file.
		/// </summary>
		void AvailableReports();

		/// <summary>
		/// Looks for the reports XML file on the local machine.
		/// </summary>
		/// <returns>True if exists; False if doesn't.</returns>
		bool DoesReportsXMLFileExist();

		/// <summary>
		/// Verifies the username and password.
		/// </summary>
		/// <param name="username">username</param>
		/// <param name="password">password</param>
		/// <returns>True if password matches the username; False if it doesn't.</returns>
		bool ConfirmPassword(String username, String password);

		/// <summary>
		/// Extracts the host site name for the url.
		/// For example, the following:
		/// http://smcgeel/
		/// http://smcgeel
		/// smcgeel
		/// will return "smcgeel".
		/// </summary>
		/// <param name="hostSite">The report server host site.</param>
		/// <returns>The computer name or url.</returns>
		string ParseHostSite(string hostSite);

		/// <summary>
		/// Validates the account name.
		/// The account name cannot be empty.
		/// </summary>
		/// <param name="accountName">account name</param>
		/// <returns>True if account is valid; False if not.</returns>
		bool ValidateAccountName(string accountName);

		/// <summary>
		/// Determines if a folder already exists.
		/// </summary>
		/// <param name="server">The name of the server that has Reporting Service.</param>
		/// <param name="virtualRoot">The virtual directory in the server.</param>
		/// <param name="folderPath">The path name of the folder.</param>
		/// <returns>True if folder exists; False for otherwise</returns>
		bool ValidateFolderName (string server, string virtualRoot, string folderPath);

		/// <summary>
		/// Creates a shortcut link to reporting services.
		/// </summary>
		/// <param name="computerName">The name of the computer that has reporting services.</param>
		/// <param name="managerName">The name of the report manager.</param>
		/// <returns>The shortcut url.</returns>
		string CreateShortcutLink(string computerName, string managerName);

		/// <summary>
		/// Creates a shortcut to reporting services for the specifed computer.
		/// </summary>
		/// <param name="computerName">The name of the computer that has reporting services.</param>
		/// <param name="managerName">The name of the report manager.</param>
		/// <returns>The shortcut url.</returns>
		bool CreateShortcut(string computerName, string managerName);

		/// <summary>
		/// Lists all available SQL servers.
		/// </summary>
		void ListAvailableServers();

		/// <summary>
		/// Sets properties for normal connection.
		/// </summary>
		void SetNormalProxy();

		/// <summary>
		/// Sets properties for secure connection.
		/// </summary>
		void SetSslProxy();

	}
}
