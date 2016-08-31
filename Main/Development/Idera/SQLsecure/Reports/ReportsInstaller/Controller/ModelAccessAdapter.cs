using System;

using Idera.Common.ReportsInstaller.View;
using Idera.Common.ReportsInstaller.Model;

namespace Idera.Common.ReportsInstaller.Controller
{
	/// <summary>
	/// Gives the view access to certain methods in the model.
	/// </summary>
	public class ModelAccessAdapter : IModelAccessAdapter
	{
		private IRsModel _model;
		/// <summary>
		/// The model that the adapter can decide to expose.
		/// </summary>
		public IRsModel Model
		{
			get
			{
				return _model;
			}
			set
			{
				_model = value;
			}
		}

		/// <summary>
		/// Determines whether the user has the permission to use all of the
		/// methods in this adapter.
		/// </summary>
		/// <param name="server">The name of the server that has Reporting Service.</param>
		/// <param name="virtualRoot">The virtual directory in the server.</param>
		/// <returns>True is has requested permissions; False if doesn't</returns>
		public bool HasAppropriatePermissions(string server, string virtualRoot)
		{
			return _model.HasAppropriatePermissions(server, virtualRoot);
		}

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
		public bool CreateDataSource (string server, string virtualRoot, string parentPath, 
			string reportHost, string reportUser, string reportPassword, bool overwrite)
		{
			return _model.CreateDataSource(server, virtualRoot, parentPath,
				reportHost, reportUser,reportPassword, overwrite);
		}

		/// <summary>
		/// Adds a folder to the report server database.
		/// </summary>
		/// <param name="server">The name of the server that has Reporting Service.</param>
		/// <param name="virtualRoot">The virtual directory in the server.</param>
		/// <param name="folderName">The name of the new folder. </param>
		/// <returns> True for successful; False for otherwise</returns>
		public bool CreateFolder (string server, string virtualRoot, string folderName)
		{
			return _model.CreateFolder(server, virtualRoot, folderName);
		}

		/// <summary>
		/// Determines if a folder already exists.
		/// </summary>
		/// <param name="server">The name of the server that has Reporting Service.</param>
		/// <param name="virtualRoot">The virtual directory in the server.</param>
		/// <param name="folderPath">The path name of the folder.</param>
		/// <returns>True if folder exists; False for otherwise</returns>
		public bool DoesFolderExist(string server, string virtualRoot, string folderPath)
		{
			return _model.DoesFolderExist(server, virtualRoot, folderPath);
		}

		/// <summary>
		/// Adds a new report to the report server database.
		/// </summary>
		/// <param name="server">The name of the server that has Reporting Service.</param>
		/// <param name="virtualRoot">The virtual directory in the server.</param>
		/// <param name="folderPath">The full path name of the folder.</param>
		/// <param name="reportName">The name of the new report.</param>
		/// <returns>True for successful; False for otherwise</returns>
		public bool DeployReport(string server, string virtualRoot,string folderPath, 
			string reportName)
		{
			return _model.DeployReport(server, virtualRoot, folderPath, reportName);
		}

		/// <summary>
		/// Generates a list of Reporting Service Folders
		/// </summary>
		/// <param name="server">The name of the server that has Reporting Service.</param>
		/// <param name="virtualRoot">The virtual directory in the server.</param>
		public void PopulateFolderList(string server, string virtualRoot)
		{
			_model.PopulateFolderList(server, virtualRoot);
		}

		/// <summary>
		/// Generates a list of Reporting Service folders.
		/// </summary>
		/// <param name="server">The name of the server that has Reporting Service.</param>
		/// <param name="virtualRoot">The virtual directory in the server.</param>
		/// <param name="folderPath">The full path name of the parent folder. </param>
		/// <returns>True if data source exists; false if doesn't.</param>
		public bool DoesDataSourceExist(string server, string virtualRoot, string folderPath)
		{
			return _model.DoesDataSourceExist(server, virtualRoot, folderPath);
		}

		/// <summary>
		/// Generates a list of reports in the specified folder as well as the reports
		/// that have not been installed yet.
		/// </summary>
		/// <param name="server">The name of the server that has Reporting Service.</param>
		/// <param name="virtualRoot">The virtual directory in the server.</param>
		/// <param name="folderPath">The full path name of the folder. </param>
		public void ReportsFoundInFolder(string server, string virtualRoot,
			string folderPath)
		{
			_model.ReportsFoundInFolder(server, virtualRoot, folderPath);
		}

		/// <summary>
		/// Verifies the url of the host computer or site.
		/// </summary>
		/// <param name="computerName">The name of the computer.</param>
		/// <returns>True if valid connection; false if not.</returns>
		public bool TestHostComputer(string computerName)
		{
			return _model.TestHostComputer(computerName);
		}


		/// <summary>
		/// Verifies the url of the Report Manager.
		/// </summary>
		/// <param name="computerName">The name of the computer.</param>
		/// <param name="managerName">The name of the report manager.</param>
		/// <returns>True if valid connection; false if not.</returns>
		public bool TestReportManager(string computerName, string managerName)
		{
			return _model.TestReportManager(computerName, managerName);
		}

		/// <summary>
		/// Verifies the url of the Report Manager.
		/// </summary>
		/// <param name="computerName">The name of the computer.</param>
		/// <param name="directoryName">The name of the report server.</param>
		/// <returns>True if valid connection; false if not.</returns>
		public bool TestReportServer(string computerName, string directoryName)
		{
			return _model.TestReportServer(computerName, directoryName);
		}

		/// <summary>
		/// Verifies the existence of the database in the constructor in the specified
		/// server.
		/// </summary>
		/// <param name="serverName">The name of the server.</param>
		public bool TestConnection(string serverName)
		{
			return _model.TestConnection(serverName);
		}

		/// <summary>
		/// Tries to insert the server name and folder name into the specified database.
		/// </summary>
		/// <param name="serverName">The name of the server that has the specified database.</param>
		/// <param name="server">The name of the server that has reporting services.</param>
		/// <param name="folder">The name of the folder that the reports are stored in.</param>
		public bool InsertServerAndFolder(string serverName, string server, string folder,
			string reportManager)
		{
			return _model.InsertServerAndFolder(serverName, server, folder, reportManager);
		}

		/// <summary>
		/// Loads perferences from local registry.
		/// </summary>
		/// <param name="name">The name of the value to retrieve.</param>
		/// <param name="defaultValue">The value to return if the name does not
		/// exist in the registry.</param>
		/// <returns>retrieved preference</returns>
		public string ReadPreferences(string name, object defaultValue)
		{
			return _model.ReadPreferences(name, defaultValue);
		}

		/// <summary>
		/// Writes perferences from local registry.
		/// </summary>
		/// <param name="name">The name of the value to store the data in.</param>
		/// <param name="newValue">The data to store.</param>
		public void WritePreferences(string name, object newValue)
		{
			_model.WritePreferences(name, newValue);
		}

		/// <summary>
		/// Loads reports data (name, file name and description) from the rdl.xml file.
		/// </summary>
		public bool ReportDescriptor()
		{
			return _model.ReportDescriptor();
		}

		/// <summary>
		/// Checks the local computer for all reports on the rdl file.
		/// </summary>
		public void AvailableReports()
		{
			_model.AvailableReports();
		}

		/// <summary>
		/// Looks for the reports XML file on the local machine.
		/// </summary>
		/// <returns>True if exists; False if doesn't.</returns>
		public bool DoesReportsXMLFileExist()
		{
			return _model.DoesReportsXMLFileExist();
		}

		/// <summary>
		/// Verifies the username and password.
		/// </summary>
		/// <param name="username">username</param>
		/// <param name="password">password</param>
		/// <returns>True if password matches the username; False if it doesn't.</returns>
		public bool ConfirmPassword(String username, String password)
		{
			return _model.ConfirmPassword(username, password);
		}

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
		public string ParseHostSite(string hostSite)
		{
			return _model.ParseHostSite(hostSite);
		}

		/// <summary>
		/// Validates the account name.
		/// The account name cannot be empty.
		/// </summary>
		/// <param name="accountName">account name</param>
		/// <returns>True if account is valid; False if not.</returns>
		public bool ValidateAccountName(string accountName)
		{
			return _model.ValidateAccountName(accountName);
		}

		/// <summary>
		/// Determines if a folder already exists.
		/// </summary>
		/// <param name="server">The name of the server that has Reporting Service.</param>
		/// <param name="virtualRoot">The virtual directory in the server.</param>
		/// <param name="folderPath">The path name of the folder.</param>
		/// <returns>True if folder exists; False for otherwise</returns>
		public bool ValidateFolderName (string server, string virtualRoot, string folderPath)
		{
			return _model.ValidateFolderName(server, virtualRoot, folderPath);
		}

		/// <summary>
		/// Creates a shortcut link to reporting services.
		/// </summary>
		/// <param name="computerName">The name of the computer that has reporting services.</param>
		/// <param name="managerName">The name of the report manager.</param>
		/// <returns>The shortcut url.</returns>
		public string CreateShortcutLink(string computerName, string managerName)
		{
			return _model.CreateShortcutLink(computerName, managerName);
		}

		/// <summary>
		/// Creates a shortcut to reporting services for the specifed computer.
		/// </summary>
		/// <param name="computerName">The name of the computer that has reporting services.</param>
		/// <param name="managerName">The name of the report manager.</param>
		/// <returns>The shortcut url.</returns>
		public bool CreateShortcut(string computerName, string managerName)
		{
			return _model.CreateShortcut(computerName, managerName);
		}

		/// <summary>
		/// Lists all available SQL servers.
		/// </summary>
		public void ListAvailableServers()
		{
			_model.ListAvailableServers();
		}

		/// <summary>
		/// Sets properties for normal connection.
		/// </summary>
		public void SetNormalProxy()
		{
			_model.SetNormalProxy();
		}

		/// <summary>
		/// Sets properties for secure connection.
		/// </summary>
		public void SetSslProxy()
		{
			_model.SetSslProxy();
		}

	}
}
