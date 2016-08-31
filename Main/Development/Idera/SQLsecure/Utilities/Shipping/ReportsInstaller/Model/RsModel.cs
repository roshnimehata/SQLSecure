using System ;
using System.Collections ;
using System.IO ;
using System.Net ;
using System.Runtime.InteropServices ;
using System.Text ;
using System.Xml ;
using System.Xml.XPath ;
using System.Web.Services.Protocols ;
using Idera.Common.ReportsInstaller.Model.ModelInterfaces ;
using Idera.Common.ReportsInstaller.Model.Query ;
using Idera.Common.ReportsInstaller.Model.RsAdapter ;
using Idera.Common.ReportsInstaller.rs;
using IWshRuntimeLibrary;
using SQLDMO ;
using File=System.IO.File ;
using Idera.Common.ReportsInstaller.ErrorLogging;

namespace Idera.Common.ReportsInstaller.Model
{
	/// <summary>
	/// The logic of the application.
	/// Contains methods for how to access Reporting Services, what tables
	/// to connect to, and what perferences the user has.
	/// The model of the model-view-controller design pattern (MVC pattern).
	/// </summary>
	public class RsModel : IRsModel
	{
		private IViewAccessAdapter _viewAdapter;
		/// <summary>
		/// Gives the model access to certain methods in the view.
		/// </summary>
		public IViewAccessAdapter ViewAdapter
		{
			get
			{
				return _viewAdapter;
			}
		}

		/// <summary>
		/// The name of the database.
		/// </summary>
		private string _databaseName;

		/// <summary>
		/// The name of the table in the database.
		/// </summary>
		private string _tableName;

		/// <summary>
		/// The columns in the table.
		/// </summary>
		private string[] _columns;

		/// <summary>
		/// The types associated with the columns.
		/// </summary>
		private Type[] _columnTypes;

		/// <summary>
		/// The name of a subkey in the registry.
		/// </summary>
		private string _regkey;

		/// <summary>
		/// The name of the reports file.
		/// </summary>
		private string _xmlReportsName;

        /// <summary>
        /// The name of the reports file.
        /// </summary>
        private string _rdlFolder;

		/// <summary>
		/// The XPathExpressions associated with retrieving the reports descriptors.
		/// </summary>
		private string _reportsXPathExpression;

		/// <summary>
		/// The location of the shortcut in the start menu.
		/// </summary>
		private string _startMenuPath;

		/// <summary>
		/// The name of the shortcut file.
		/// </summary>
		private string _shortcutFile;

		/// <summary>
		/// A dictionary containing rdl file names keyed by the reporting service name.
		/// </summary>
		private Hashtable _dictReportFile;

		/// <summary>
		/// A dictionary containing rdl descriptions keyed by the reporting service name.
		/// </summary>
		private Hashtable _dictReportDes;

		/// <summary>
		/// A dictionary of report names already installed.
		/// </summary>
		private Hashtable _dictReportInstalled;

		/// <summary>
		/// The current Reporting Services object factory.
		/// </summary>
		private IProxyFactory _rsFac;

		/// <summary>
		/// The beginning part of the url.
		/// </summary>
		private string _http = "http://";

		/// <summary>
		/// The required permissions a user must have use this installer.
		/// </summary>
		private string[] _requiredPermissions;

		/// <summary>
		/// The parent path for reporting service folders.
		/// </summary>
		private string _parentPath;

		/// <summary>
		/// The description for the folder.
		/// </summary>
		private string _folderDescription;

		/// <summary>
		/// The name of the datasource.
		/// </summary>
		private string _dsName;

		/// <summary>
		/// The description of the datasource.
		/// </summary>
		private string _dsDescription;

		/// <summary>
		/// The required data in the reportingServicesTemplate column.
		/// </summary>
		private string _reportingServicesTemplate;
		
		/// <summary>
		/// The required data in the mainTemplate column.
		/// </summary>
		private string _mainTemplate;
		
		/// <summary>
		/// The required data in the reportsTemplate column.
		/// </summary>
		private string _reportsTemplate;

        private string _rdlFolderPath;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="viewAdapter">The adapter that give the model access
		/// to parts of the view.</param>
		/// <param name="databaseName">The name of the database.</param>
		/// <param name="tableName">The name of the table in the database.</param>
		/// <param name="columns">The columns in the table.</param>
		/// <param name="columnTypes">The types associated with the columns.</param>
		/// <param name="regkey">The name of a subkey in the registry.</param>
		/// <param name="xmlReportsName">The name of the reports file.</param>
		/// <param name="reportsXPathExpression">The XPathExpressions associated with 
		/// retrieving the reports descriptors.</param>
		/// <param name="startMenuPath">The location of the shortcut in the start menu.</param>
		/// <param name="shortcutFile">The name of the shortcut file.</param>
		/// <param name="requiredPermissions">The required permissions a user must 
		/// have use this installer.</param>
		/// <param name="parentPath">The parent path for reporting service folders.</param>
		/// <param name="folderDescription">The description for the folder.</param>
		/// <param name="dsName">The name of the datasource.</param>
		/// <param name="dsDescription">The description of the datasource.</param>
		public RsModel(IViewAccessAdapter viewAdapter, string databaseName,
			string tableName, string[] columns, Type[] columnTypes, string regkey,
			string xmlReportsName, string rdlFolder, string reportsXPathExpression, 
			string startMenuPath, string shortcutFile, string[] requiredPermissions, 
			string parentPath, string folderDescription, string dsName, string dsDescription,
			string reportingServicesTemplate, string mainTemplate, string reportsTemplate)
		{
			_viewAdapter = viewAdapter;
			_databaseName = databaseName;
			_tableName = tableName;
			_columns = columns;
			_columnTypes = columnTypes;
			_regkey = regkey;
			_xmlReportsName = xmlReportsName;
            _rdlFolder = rdlFolder;
			_reportsXPathExpression = reportsXPathExpression;
			_startMenuPath = startMenuPath;
			_shortcutFile = shortcutFile;
			_rsFac = new SslProxyFactory(viewAdapter);
			_requiredPermissions = requiredPermissions;
			_parentPath = parentPath;
			_folderDescription = folderDescription;
			_dsName = dsName;
			_dsDescription = dsDescription;
			_reportingServicesTemplate = reportingServicesTemplate;
			_mainTemplate = mainTemplate;
			_reportsTemplate = reportsTemplate;

			_dictReportFile = new Hashtable();
			_dictReportDes = new Hashtable();
			_dictReportInstalled = new Hashtable();
			_rsFac = ProxyFactory.Singleton;

            try
            {
//                _rdlFolderPath = Path.Combine(System.Windows.Forms.Application.StartupPath, _rdlFolder);
                _rdlFolderPath = Path.Combine(@"../", _rdlFolder);
            }
            catch (Exception)
            {
                _rdlFolderPath = "";
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
			bool perm ;
			try
			{
				perm = (bool)_rsFac.Execute(PermissionsAlgo.Singleton, server, virtualRoot, "/",
					_requiredPermissions);
			}
			catch(WebException ex)
			{
				_viewAdapter.DisplayMessageBox(String.Format("Bad Reporting Services address. No server found at location {0}{1}/{2}.  As a result, the operation timed out.  Even if Reporting Services is on a named instance of SQL Server, please only specify the name of the computer.  Reporting Services knows which instance of SQL Server it is on.  To find the report server virtual directory name, please look in the Internet Information Services in the Administrative Tools of the Control Panel.", 
					                                         _http, server, virtualRoot),"Reporting Service Connection Error!");
				return false;
			}
			catch(Exception)
			{
				_viewAdapter.DisplayMessageBox(String.Format("Error communicating with reporting server {0}.", server), "Reporting Service Connection Error!");
				return false;
			}
			if (perm == false)
			{
				_viewAdapter.DisplayMessageBox("User running this application does not have appropriate role set in Report Manager (Content Manager is required).", "Permissions Error!");
			}
			return perm;
		}

		/// <summary>
		/// Creates a new data source in the report server database.
		/// </summary>
		/// <param name="server">The name of the server that has Reporting Service.</param>
		/// <param name="virtualRoot">The virtual directory in the server.</param>
		/// <param name="folderPath">The full path name of the parent 
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
		public bool CreateDataSource (string server, string virtualRoot, string folderPath, 
			string reportHost, string reportUser, string reportPassword, bool overwrite)
		{
			try
			{
				_viewAdapter.UpdateInstallerLog("Create Data Source\r\n");
				_viewAdapter.UpdateInstallerLog("Data Source : " + _parentPath+folderPath + " - " + _dsName + "-\r\n"+ _dsDescription + "\r\n");

				_rsFac.Execute(CreateDataSourceAlgo.Singleton, server, virtualRoot, _parentPath+folderPath,
					_dsName, _dsDescription, _databaseName, reportHost, reportUser,
					reportPassword, overwrite);
			}
			catch (Exception)
			{
				string message = String.Format("Error creating {0} datasource.", _dsName);
				_viewAdapter.UpdateInstallerLog("----------Error----------\r\n");
				_viewAdapter.UpdateInstallerLog(message+"\r\n");

				return false;
			}
			_viewAdapter.UpdateInstallerLog("Success\r\n");
			return true;
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
			try
			{
				_viewAdapter.UpdateInstallerLog("Create Folder\r\n");
				_viewAdapter.UpdateInstallerLog("Folder : " + _parentPath + " - " + _folderDescription + "-\r\n");

				_rsFac.Execute(CreateSubFoldersAlgo.Singleton, server, virtualRoot, _parentPath,
					folderName, _folderDescription);
			}
			catch (Exception)
			{
				string message = String.Format("Error creating {0} folder.", folderName);
				_viewAdapter.UpdateInstallerLog("----------Error----------\r\n");
				_viewAdapter.UpdateInstallerLog(message+"\r\n");

				return false;
			}
			_viewAdapter.UpdateInstallerLog("Success\r\n");
			return true;
		}

		/// <summary>
		/// Determines if a folder already exists.
		/// </summary>
		/// <param name="server">The name of the server that has Reporting Service.</param>
		/// <param name="virtualRoot">The virtual directory in the server.</param>
		/// <param name="folderPath">The path name of the folder.</param>
		/// <returns>True if folder exists; False for otherwise</returns>
		public bool DoesFolderExist (string server, string virtualRoot, string folderPath)
		{
			bool folderExists = false;

			try
			{
				folderExists = (bool)_rsFac.Execute(DoesFolderExistAlgo.Singleton,
					server, virtualRoot, _parentPath+folderPath);
			}
			catch
			{
				// folder doesn't exist if this point is reached
			}
			return folderExists;
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
			try
			{
				_rsFac.Execute(DeployReportAlgo.Singleton, server, virtualRoot, _parentPath+folderPath,
                    Path.Combine(_rdlFolderPath, (string)_dictReportFile[reportName]), 
					reportName, _dictReportDes[reportName]);
			}
			catch(IOException ex)
			{
				string message = ErrorFactory.Singleton.ShowErrorMessage(String.Format("Error reading {0} file.", _dictReportFile[reportName]), ex);
				_viewAdapter.UpdateInstallerLog("----------Error----------\r\n");
				_viewAdapter.UpdateInstallerLog(message+"\r\n");
				return false;
			}
			catch(SoapException ex)
			{
				string s = ex.Detail["ErrorCode"].InnerText;
				switch (s)
				{
					case "rsProcessingError":
						// caught from the three bad reports in the 6/12/06 build
						string message = String.Format("Error deploying {0} report.  The report file is corrupted or invalid.", reportName);
						_viewAdapter.UpdateInstallerLog("----------Error----------\r\n");
						_viewAdapter.UpdateInstallerLog(message+"\r\n");
						return false;
					default:
						string message2 = ErrorFactory.Singleton.ShowErrorMessage(String.Format("Error deploying {0} report.", reportName), ex);
						_viewAdapter.UpdateInstallerLog("----------Error----------\r\n");
						_viewAdapter.UpdateInstallerLog(message2+"\r\n");
						return false;
				}
			}
			catch(Exception ex)
			{
				string message = ErrorFactory.Singleton.ShowErrorMessage(String.Format("Error deploying {0} report.", reportName), ex);
				_viewAdapter.UpdateInstallerLog("----------Error----------\r\n");
				_viewAdapter.UpdateInstallerLog(message+"\r\n");
				return false;
			}

			_viewAdapter.UpdateInstallerLog("Success\r\n");
			_viewAdapter.UpdateInstallerLog((string)_dictReportFile[reportName]+"\r\n");
			_viewAdapter.AddToInstalledList(reportName);
			return true;
		}

		/// <summary>
		/// Generates a list of Reporting Service folders.
		/// </summary>
		/// <param name="server">The name of the server that has Reporting Service.</param>
		/// <param name="virtualRoot">The virtual directory in the server.</param>
		public void PopulateFolderList(string server, string virtualRoot)
		{
			CatalogItem[] items = ListChildren(server, virtualRoot, "/", true);
			foreach (CatalogItem ci in items)
			{
				if (ci.Type == ItemTypeEnum.Folder)
				{
					// write to folder browse
					string path = ci.Path;
					string name = path.Remove(0,1);
					_viewAdapter.AddItemToFolderList(name);
				}
			}
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
			return (bool)_rsFac.Execute(DoesDataSourceExistAlgo.Singleton, server, virtualRoot,
				_parentPath+folderPath);
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
			CatalogItem[] items = ListChildren(server, virtualRoot,
				_parentPath+folderPath, false);
			_dictReportInstalled = new Hashtable();
			
			bool check = true;
			string message = "You are missing the following reports:";
			StringBuilder sb = new StringBuilder();
			sb.Append(message);
			
			foreach (CatalogItem ci in items)
			{
				if (ci.Type == ItemTypeEnum.Report)
				{
                    if (File.Exists(Path.Combine(_rdlFolderPath, (string)_dictReportFile[ci.Name])))
					{
						_dictReportInstalled[ci.Name] = 1;
						// write to report list box
						_viewAdapter.AddToAlreadyInstalledList(ci.Name);
					}
					else
					{
						_dictReportInstalled[ci.Name] = 1;
						check = false;
						sb.Append("\r\n");
						sb.Append(_dictReportFile[ci.Name]);
					}
				}
			}

			foreach (string key in _dictReportFile.Keys)
			{
                if (File.Exists(Path.Combine(_rdlFolderPath, (string)_dictReportFile[key])))
				{
					if (!_dictReportInstalled.ContainsKey(key))
					{
						// write to checked list box
						_viewAdapter.AddToReportsList(key);
					}
				}
				else
				{
					if (!_dictReportInstalled.ContainsKey((key)))
					{
						check = false;
						sb.Append("\r\n");
						sb.Append(_dictReportFile[key]);
					}
				}
			}
			if (!check)
			{
				_viewAdapter.DisplayMessageBox(sb.ToString(), "Missing Reports!");
			}
		}

		/// <summary>
		/// Verifies the url of the host computer or site.
		/// </summary>
		/// <param name="computerName">The name of the computer.</param>
		/// <returns>True if valid connection; false if not.</returns>
		public bool TestHostComputer(string computerName)
		{
			bool check = true;
			try
			{
				WebClient wc = new WebClient();
				using (Stream stream = wc.OpenRead("http://"+computerName))
				{
				}
//				stream.Close();
			}
			catch (WebException ex)
			{
				if (ex.Message.Equals("The underlying connection was closed: The remote name could not be resolved.")
					|| ex.Status.Equals("NameResolutionFailure"))
				{
					_viewAdapter.DisplayMessageBox("Error accessing the host computer with the url: "+_http+computerName + ".  Please look at the Connect to Microsoft Reporting Services panel and make sure you put in the correct computer name.  Even if Reporting Services is on a named instance of SQL Server, please only specify the name of the computer.  Reporting Services knows which instance of SQL Server it is on.", "Connection Error!");
					check = false;
				}
				else if (ex.Message.Equals("The underlying connection was closed: Unable to connect to the remote server.")
					|| ex.Status.Equals("ConnectFailure"))
				{
					_viewAdapter.DisplayMessageBox("Error accessing the host computer with the url: "+_http+computerName + ". Please go to the advanced options and make sure that your port number is correct.", "Connection Error!");
					check = false;
				}
			}
			return check;
		}

		/// <summary>
		/// Verifies the url of the Report Manager.
		/// </summary>
		/// <param name="computerName">The name of the computer.</param>
		/// <param name="managerName">The name of the report manager.</param>
		/// <returns>True if valid connection; false if not.</returns>
		public bool TestReportManager(string computerName, string managerName)
		{
			bool check = true;
			try
			{
				WebClient wc = new WebClient();
				using (Stream stream = wc.OpenRead("http://"+computerName+"/"+managerName))
				{
				}
//				stream.Close();
			}
			catch (WebException ex)
			{
				if (ex.Message.Equals("The remote server returned an error: (404) Not Found."))
				{
					_viewAdapter.DisplayMessageBox("Error accessing the Report Manager Virtual Directory with the url: "+_http+computerName+"/"+managerName + ".  Please go to the advanced options to check the manager name.  To find the report manager virtual directory name, please look in the Internet Information Services in the Administrative Tools of the Control Panel.", "Connection Error!");
					check = false;
				}
			}
			return check;
		}

		/// <summary>
		/// Verifies the url of the Report Manager.
		/// </summary>
		/// <param name="computerName">The name of the computer.</param>
		/// <param name="directoryName">The name of the report server.</param>
		/// <returns>True if valid connection; false if not.</returns>
		public bool TestReportServer(string computerName, string directoryName)
		{
			bool check = true;
			try
			{
				WebClient wc = new WebClient();
				using (Stream stream = wc.OpenRead("http://"+computerName+"/"+directoryName))
				{
				}
//				stream.Close();
			}
			catch (WebException ex)
			{
				if (ex.Message.Equals("The remote server returned an error: (404) Not Found."))
				{
					_viewAdapter.DisplayMessageBox("Error accessing the Report Server Virtual Directory with the url: "+_http+computerName+"/"+directoryName + ".  Please go to the advanced options to check the manager name.  To find the report server virtual directory name, please look in the Internet Information Services in the Administrative Tools of the Control Panel.", "Connection Error!");
					check = false;
				}
			}
			return check;
		}

		/// <summary>
		/// Verifies the existence of the database in the constructor in the specified
		/// server.
		/// </summary>
		/// <param name="serverName">The name of the server.</param>
		/// <returns>True if valid connection; false if not.</returns>
		public bool TestConnection(string serverName)
		{
			Gateway reports = new Gateway(serverName, _databaseName, _tableName);
			try
			{
				reports.TestConnection();
			}
			catch (Exception)
			{
				_viewAdapter.DisplayMessageBox(String.Format( "Error accessing {0} database on instance {1}.",
					_databaseName, serverName), "Connection Error!");
				return false;
			}
			finally
			{
				reports.SQLConnection.Close();
			}
			return true;
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
			try
			{
				string reportingServicesTemplate = String.Format(_reportingServicesTemplate, _http, "{0}", reportManager);
				string mainTemplate = String.Format(_mainTemplate, _http, "{0}", reportManager, "{1}");
				string reportsTemplate = String.Format(_reportsTemplate, _http, "{0}", reportManager, "{1}", "{2}");

				Gateway reports = new Gateway(serverName, _databaseName, _tableName);
				IQuery update = UpdateQuery.Singleton;
				IQuery insert = InsertQuery.Singleton;

				object[] values = {server, folder, reportingServicesTemplate, mainTemplate, reportsTemplate};

				int rowsUpdated = (int)reports.ExecuteQuery(update, _columns, _columnTypes, values);
				if (rowsUpdated == 0)
				{
					reports.ExecuteQuery(insert, _columns, _columnTypes, values);
				}
				return true;
			}
			catch (Exception)
			{
				_viewAdapter.UpdateInstallerLog("The setup program is unable to insert the Reporting Services Instance into the SQLcompliance database.\r\n  Please verify that you have insert rights on this database.\r\n");
				return false;
			}
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
			return Preferences.Singleton.ReadPreferences(_regkey, name, defaultValue);
		}

		/// <summary>
		/// Writes perferences from local registry.
		/// </summary>
		/// <param name="name">The name of the value to store the data in.</param>
		/// <param name="newValue">The data to store.</param>
		public void WritePreferences(string name, object newValue)
		{
			Preferences.Singleton.WritePreferences(_regkey, name, newValue);
		}

		/// <summary>
		/// Loads reports data (name, file name and description) from the rdl.xml file.
		/// </summary>
		public bool ReportDescriptor()
		{
			try
			{
				XmlDocument xmlIni = new XmlDocument();
                xmlIni.Load(Path.Combine(_rdlFolderPath, _xmlReportsName));
				XPathNavigator xpnav =xmlIni.CreateNavigator();	
				XPathNodeIterator iterator = xpnav.Select(_reportsXPathExpression);
				_dictReportFile = new Hashtable();
				_dictReportDes = new Hashtable();
				string name ;
				
				while(iterator.MoveNext())
				{
					name = iterator.Current.Value;
					iterator.Current.MoveToNext();
					_dictReportFile[name] = iterator.Current.Value;
					iterator.Current.MoveToNext();
					_dictReportDes[name] = iterator.Current.Value;
				}
				return true;
			}
			catch (Exception)
			{
				_viewAdapter.DisplayMessageBox("Your"+_xmlReportsName+" file is corrupted.  Unable to complete installation.", "Corrupted "+_xmlReportsName+" File!");
				return false;
			}
		}

		/// <summary>
		/// Checks the local computer for all reports on the rdl file.
		/// </summary>
		public void AvailableReports()
		{
			bool check = true;
			string message = "You are missing the following reports:";
			StringBuilder sb = new StringBuilder();
			sb.Append(message);
			foreach (string key in _dictReportFile.Keys)
			{
                if (File.Exists(Path.Combine(_rdlFolderPath, (string)_dictReportFile[key])))
				{
					// write to checked list box
					_viewAdapter.AddToReportsList(key);
				}
				else
				{
					check = false;
					sb.Append("\r\n");
					sb.Append(_dictReportFile[key]);
				}
			}
			if (!check)
			{
				_viewAdapter.DisplayMessageBox(sb.ToString(), "Missing Reports!");
			}
		}

		/// <summary>
		/// Looks for the reports XML file on the local machine.
		/// </summary>
		/// <returns>True if exists; False if doesn't.</returns>
		public bool DoesReportsXMLFileExist()
		{
            bool check = File.Exists(Path.Combine(_rdlFolderPath, _xmlReportsName));
			if (check == false)
			{
				_viewAdapter.DisplayMessageBox(_xmlReportsName+" file not found",  "Error!");
			}
			return check;
		}

		[DllImport("InstallUtilLib.dll")]
		public static extern int VerifyPassword (String username, String password);

		/// <summary>
		/// Verifies the username and password.
		/// </summary>
		/// <param name="username">username</param>
		/// <param name="password">password</param>
		/// <returns>True if password matches the username; False if it doesn't.</returns>
		public bool ConfirmPassword(String username, String password)
		{
			try
			{
				int confirmed = VerifyPassword(username, password);
				if (confirmed != 0)
				{
					_viewAdapter.DisplayMessageBox("Cannot login to SQL Server with specified login account: "+username+".",
						"Cannot Login!");
					return false;
				}
				else
				{
					return true;
				}
			}
			catch(Exception)
			{
				_viewAdapter.DisplayMessageBox(String.Format( "Error verifying user credentials {0}.",
					username), "Validation Error!");
				return false;
			}
		}

		/// <summary>
		/// Validates the account name.
		/// The account name cannot be empty.
		/// </summary>
		/// <param name="accountName">account name</param>
		/// <returns>True if account is valid; False if not.</returns>
		public bool ValidateAccountName(string accountName)
		{
			string tmp = accountName.Trim();
         
			int pos = tmp.IndexOf(@"\");
			if ( pos <= 0 )
			{
				_viewAdapter.DisplayMessageBox("Logon ID must be a windows account specified in the form of 'domain\\user'.", "Error!");
				return false;
			}
			else
			{
				string domain  = tmp.Substring(0,pos);
				string account = tmp.Substring(pos+1);
            
				if ((domain == "") || (account == "" ))
				{
					_viewAdapter.DisplayMessageBox("Logon ID must be a windows account specified in the form of 'domain\\user'.", "Error!");
					return false;
				}
			}
         
			return true;
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
			bool valid = true;

			try
			{
				_rsFac.Execute(DoesFolderExistAlgo.Singleton, server, virtualRoot, 
					_parentPath+folderPath);
			}
			catch
			{
				valid = false;
				_viewAdapter.DisplayMessageBox(folderPath+" is not a valid folder path.", "Error!");
			}

			return valid;
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
			char[] chars = hostSite.ToCharArray();
			int length = chars.Length;
			ArrayList list = new ArrayList(chars);
			if (length > _http.Length)
			{
				char[] httpChar = new char[_http.Length];
				list.CopyTo(0,httpChar,0,_http.Length);
				string convert = new String(httpChar);
				if (convert.Equals(_http))
				{
					for (int i = _http.Length-1; i >= 0; i--)
					{
						list.RemoveAt(i);
						length--;
					}
				}
			}
			if (length > 0)
			{
				if ((char)list[length-1] == '/')
				{
					list.RemoveAt(length-1);
					length--;
				}
			}
			
			char[] revised = new char[length];
			list.CopyTo(0,revised,0,length);

			return new String(revised);

		}

		/// <summary>
		/// Creates a shortcut link to reporting services.
		/// </summary>
		/// <param name="computerName">The name of the computer that has reporting services.</param>
		/// <param name="managerName">The name of the report manager.</param>
		/// <returns>The shortcut url.</returns>
		public string CreateShortcutLink(string computerName, string managerName)
		{
			string url = _http+computerName+"/"+managerName;
			return url;
		}

		/// <summary>
		/// Creates a shortcut to reporting services for the specifed computer.
		/// </summary>
		/// <param name="computerName">The name of the computer that has reporting services.</param>
		/// <param name="managerName">The name of the report manager.</param>
		/// <returns>True if successful; false if not.</returns>
		public bool CreateShortcut(string computerName, string managerName)
		{
			_viewAdapter.UpdateInstallerLog("Creating shortcut\r\n");
			_viewAdapter.UpdateInstallerLog("Reports are hosted at "+_http+computerName+"/"+managerName+"\r\n");
			try
			{
				WshShellClass shell = new WshShellClass();
				// Use the Environment class to retrieve the Start Menu on the system
      		
				//the programs menu for the logged in user
				string startMenu = Environment.GetFolderPath(Environment.SpecialFolder.Programs);

				startMenu += _startMenuPath;
      		
				if(!Directory.Exists(startMenu))
				{
					Directory.CreateDirectory(startMenu);
				}
      		
				IWshShortcut shortcut = (IWshShortcut) shell.CreateShortcut(startMenu+_shortcutFile);
				shortcut.TargetPath=_http+computerName+"/"+managerName;
				shortcut.Description="Reporting Services Website";
				shortcut.Save();
				return true;
			}
			catch(Exception)
			{
				_viewAdapter.UpdateInstallerLog("The setup program is unable to create a menu shortcut for SQLcompliance Reports.\r\n  Please verify that this computer has Windows Scripting Host properly installed.\r\n  You can view reports at "+_http+computerName+"/"+managerName+ "\r\n");
				_viewAdapter.DisplayMessageBox("The setup program is unable to create a menu shortcut for SQLcompliance Reports.\r\n  Please verify that this computer has Windows Scripting Host properly installed.\r\n  You can view reports at "+_http+computerName+"/"+managerName+ "\r\n", "Shortcut Creation Error!");
				return false;
			}
		}

		/// <summary>
		/// Lists all available SQL servers.
		/// </summary>
		public void ListAvailableServers()
		{
			ApplicationClass dmoapp = new ApplicationClass();
			try
			{
				NameList nameList = dmoapp.ListAvailableSQLServers();
				if(nameList.Count > 0)
				{
					IEnumerator nameListEnum = nameList.GetEnumerator();
					while (nameListEnum.MoveNext()) 
					{
						_viewAdapter.AddItemToSQLInstanceList(nameListEnum.Current.ToString().ToUpper());
					}
				}
			}
			catch (Exception)
			{
				_viewAdapter.DisplayMessageBox("Error browsing for servers.", "Error!");
			}
		}

		/// <summary>
		/// Gets a list of children of a specified folder.
		/// </summary>
		/// <param name="server">The name of the server that has Reporting Service.</param>
		/// <param name="virtualRoot">The virtual directory in the server.</param>
		/// <param name="folderPath">The full path name of the parent folder. </param>
		/// <param name="recur">Whether the search is recursive.</param>
		/// <returns>An array of CatalogItem[] objects. If no children exist, this 
		/// method returns an empty CatalogItem object.</returns>
		public CatalogItem[] ListChildren(string server, string virtualRoot, 
			string folderPath, bool recur)
		{
			try
			{
				CatalogItem[] items = (CatalogItem[])_rsFac.Execute(ListChildrenAlgo.Singleton, server, virtualRoot, folderPath, recur);
				return items;
			}
			catch (Exception)
			{
				_viewAdapter.DisplayMessageBox("User running this application does not have appropriate role set in Report Manager(Content Manager is required).", "Permissions Error!");
				return new CatalogItem[0];
			}
		}

		/// <summary>
		/// Sets properties for normal connection.
		/// </summary>
		public void SetNormalProxy()
		{
			_http = "http://";
			_rsFac = ProxyFactory.Singleton;
		}

		/// <summary>
		/// Sets properties for secure connection.
		/// </summary>
		public void SetSslProxy()
		{
			_http = "https://";
			_rsFac = new SslProxyFactory(_viewAdapter);
		}

	}
}
