using System ;
using System.Configuration ;
using System.Windows.Forms ;
using System.IO ;
using Idera.Common.ReportsInstaller.ErrorLogging ;
using Idera.Common.ReportsInstaller.Model ;
using Idera.Common.ReportsInstaller.View ;

namespace Idera.Common.ReportsInstaller.Controller
{
	/// <summary>
	/// The main entry point for the application.
	/// The controller of the model-view-controller design pattern (MVC pattern).
	/// </summary>
	public class RsInstallationController
	{
		/// <summary>
		/// The header of the installer GUI.
		/// </summary>
		private string _header;

		/// <summary>
		/// The name of the product.
		/// </summary>
		private string _productName;

		/// <summary>
		/// The abbreviated name of the product.
		/// </summary>
		private string _productAbbrev;

		/// <summary>
		/// The name of the database.
		/// </summary>
		private string _databaseName;

		/// <summary>
		/// The name of the table in the database.
		/// </summary>
		private string _tableName;

		/// <summary>
		/// The location of the shortcut in the start menu.
		/// </summary>
		private string _startMenuPath;

		/// <summary>
		/// The name of the shortcut file.
		/// </summary>
		private string _shortcutFile;

		/// <summary>
		/// The name of a subkey in the registry.
		/// </summary>
		private string _regkey;

		/// <summary>
		/// The parent path for reporting service folders.
		/// </summary>
		private string _parentPath;

		/// <summary>
		/// The reporting services folder to store reports in.
		/// </summary>
		private string _folder;

		/// <summary>
		/// The description for the folder.
		/// </summary>
		private string _folderDescription;

		/// <summary>
		/// The name of the datasource.
		/// </summary>
		private string _dataSourceName;

		/// <summary>
		/// The description of the datasource.
		/// </summary>
		private string _dataSourceDescription;

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
		/// The image on the first panel.
		/// </summary>
		private string _firstPanelImage;

		/// <summary>
		/// The image on the last panel.
		/// </summary>
		private string _lastPanelImage;

		/// <summary>
		/// The icon in the form.
		/// </summary>
		private string _icon;

		/// <summary>
		/// The columns in the table.
		/// </summary>
		private string[] _columns;

		/// <summary>
		/// The types associated with the columns.
		/// </summary>
		private Type[] _columnTypes;
		
		/// <summary>
		/// The permissions required to run the installer.
		/// </summary>
		private string[] _requiredPermissions;
		
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

		/// <summary>
		/// The app.config was read successfully.
		/// </summary>
		private bool _success;
		
		/// <summary>
		/// The number of columns in the table.
		/// </summary>
		private string _columnsNumber;
		
		/// <summary>
		/// The number of columns in the table.
		/// </summary>
		private string _columnTypesNumber;
		
		/// <summary>
		/// The event log level.
		/// 0 for no logging, 1 for errors, 2 for errors and warnings, and 3 for 
		/// errors, warnings, and information
		/// </summary>
		private int _eventLogLevel;
		
		/// <summary>
		/// Constructor
		/// </summary>
		public RsInstallationController()
		{
			_header = "";
			_productName = "";
			_productAbbrev = "";
			_databaseName = "";
			_tableName = "";
			_startMenuPath = "";
			_shortcutFile = "";
			_regkey = "";
			_parentPath = "";
			_folder = "";
			_folderDescription = "";
			_dataSourceName = "";
			_dataSourceDescription = "";
			_xmlReportsName = "";
			_reportsXPathExpression = "";
			_firstPanelImage = "";
			_lastPanelImage = "";
			_icon = "";
			_columns = new string[0];
			_columnTypes = new Type[0];
				
			_reportingServicesTemplate = "";
			_mainTemplate = "";
			_reportsTemplate = "";
				
			string[] requiredPermissions = {"Create data source", "Create Folder", "Create Report", "Read Properties"};
			_requiredPermissions = requiredPermissions;
			
			try
			{
				_eventLogLevel = Int32.Parse(ConfigurationSettings.AppSettings["eventLogLevel"]);
			}
			catch (Exception ex)
			{
				_eventLogLevel = 0;
			}
			
			try
			{
				_success = true;
				ErrorLog.Singleton.SetLogLevel(_eventLogLevel);
				
				_header = (ConfigurationSettings.AppSettings["header"]);
				if (_header == null)
				{
					_header = "Idera Reports Installer";
				}
				_productName = (ConfigurationSettings.AppSettings["productName"]);
				if (_productName == null)
				{
					_productName = "Idera product";
				}
				_productAbbrev = (ConfigurationSettings.AppSettings["productAbbrev"]);
				if (_productAbbrev == null)
				{
					_productAbbrev = "Idera product";
				}
				_tableName = (ConfigurationSettings.AppSettings["tableName"]);
				if (_tableName == null)
				{
					_tableName = "";
				}
				_startMenuPath = (ConfigurationSettings.AppSettings["startMenuPath"]);
				if (_startMenuPath == null)
				{
					_startMenuPath = "\\Idera\\";
				}
				_shortcutFile = (ConfigurationSettings.AppSettings["shortcutFile"]);
				if (_shortcutFile == null)
				{
					_shortcutFile = "Reports.lnk";
				}
				_regkey = (ConfigurationSettings.AppSettings["regkey"]);
				if (_regkey == null)
				{
					_regkey = "Software\\Idera\\ReportsInstaller";
				}
				_parentPath = (ConfigurationSettings.AppSettings["parentPath"]);
				if (_parentPath == null)
				{
					_parentPath = "/";
				}
				_folder = (ConfigurationSettings.AppSettings["folder"]);
				if (_folder == null)
				{
					_folder = "reports";
				}
				_folderDescription = (ConfigurationSettings.AppSettings["folderDescription"]);
				if (_folderDescription == null)
				{
					_folderDescription = "Reports for Idera products";
				}
				_dataSourceName = (ConfigurationSettings.AppSettings["dataSourceName"]);
				if (_dataSourceName == null)
				{
					_dataSourceName = "Data Source";
				}
				_dataSourceDescription = (ConfigurationSettings.AppSettings["dataSourceDescription"]);
				if (_dataSourceDescription == null)
				{
					_dataSourceDescription = "Data Source for Reports";
				}
				_xmlReportsName = (ConfigurationSettings.AppSettings["xmlReportsName"]);
				if (_xmlReportsName == null)
				{
					_xmlReportsName = "rdl.xml";
				}
                _rdlFolder = (ConfigurationSettings.AppSettings["rdlFolder"]);
                if (_xmlReportsName == null)
                {
                    _xmlReportsName = "rdl.xml";
                }
				_reportsXPathExpression = (ConfigurationSettings.AppSettings["reportsXPathExpression"]);
				if (_reportsXPathExpression == null)
				{
					_reportsXPathExpression ="/folder/reportfile/name";
				}
				_firstPanelImage = (ConfigurationSettings.AppSettings["firstPanelImage"]);
                if (_firstPanelImage == null)
                {
                    _firstPanelImage = "";
                }
                else
                {
                    _firstPanelImage = Path.Combine(Application.StartupPath, _firstPanelImage);
                }
				_lastPanelImage = (ConfigurationSettings.AppSettings["lastPanelImage"]);
                if (_lastPanelImage == null)
                {
                    _lastPanelImage = "";
                }
                else
                {
                    _lastPanelImage = Path.Combine(Application.StartupPath, _lastPanelImage);
                }
				_icon = (ConfigurationSettings.AppSettings["icon"]);
                if (_icon == null)
                {
                    _icon = "";
                }
                else
                {
                    _icon = Path.Combine(Application.StartupPath, _icon);
                }
				_reportingServicesTemplate = (ConfigurationSettings.AppSettings["reportingServicesTemplate"]);
				if (_reportingServicesTemplate == null)
				{
					_reportingServicesTemplate = "{0}{1}/{2}";
				}
				_mainTemplate = (ConfigurationSettings.AppSettings["mainTemplate"]);
				if (_mainTemplate == null)
				{
					_mainTemplate = "{0}{1}/{2}/Pages/Folder.aspx?ItemPath=%2f{3}";
				}
				_reportsTemplate = (ConfigurationSettings.AppSettings["reportsTemplate"]);
				if (_reportsTemplate == null)
				{
					_reportsTemplate = "{0}{1}/{2}/Pages/Report.aspx?ItemPath=%2f{3}%2f{4}";
				}
				_columnsNumber = (ConfigurationSettings.AppSettings["columnsNumber"]);
				if (_columnsNumber == null)
				{
					_columnsNumber = "";
				}
				_columnTypesNumber = (ConfigurationSettings.AppSettings["columnTypesNumber"]);
				if (_columnTypesNumber == null)
				{
					_columnTypesNumber = "";
				}
			}
			catch (Exception ex)
			{
				_success = false;
				ErrorLog.Singleton.LogError("Configuration file is either missing or corrupted.  You cannot complete installation without the configuration file.",
					ex.Message, ex.StackTrace);
				MessageBox.Show("Configuration file is either missing or corrupted.  You cannot complete installation without the configuration file.", "Configuration File Error!");
			}
			if (_success)
			{
				try
				{
					_databaseName = (ConfigurationSettings.AppSettings["databaseName"]);
					if (_databaseName == null)
					{
						_databaseName = "";
						_success = false;
						ErrorLog.Singleton.LogError("Configuration file is either missing or corrupted.  You cannot complete installation without the configuration file.",
							"", "");
						MessageBox.Show("Configuration file is either missing or corrupted.  You cannot complete installation without the configuration file.", "Configuration File Error!");
					}
				}
				catch (Exception ex)
				{
					_success = false;
					ErrorLog.Singleton.LogError("Configuration file is either missing or corrupted.  You cannot complete installation without the configuration file.",
						ex.Message, ex.StackTrace);
					MessageBox.Show("Configuration file is either missing or corrupted.  You cannot complete installation without the configuration file.", "Configuration File Error!");
				}
			}
			if (_success)
			{
				try
				{
					if (_columnsNumber != "")
					{
						_columns = ConfigStringArray("columnsNumber", "columns");
					}
					if (_columnTypesNumber != "")
					{
						_columnTypes = ConfigTypeArray("columnTypesNumber", "columnTypes");
					}
				}
				catch (Exception ex)
				{
					string[] columns = {"reportServer", "reportFolder", "reportingServicesTemplate",
"mainTemplate", "reportemplate"};
					_columns = columns;
					Type[] columnTypes = {Type.GetType("System.String"), Type.GetType("System.String"), 
Type.GetType("System.String"), Type.GetType("System.String"), Type.GetType("System.String")};
					_columnTypes = columnTypes;
				}
			}
		}
		
		public void Start()
		{
			IModelAccessAdapter modelAdapter = null;
			IViewAccessAdapter viewAdapter = null;
			IRsModel model = null;
			IInstallerGUI view = null;
			
			if (_success)
			{
				modelAdapter = new ModelAccessAdapter();
				viewAdapter = new ViewAccessAdapter();
				
				try
				{
					ErrorLog.Singleton.LogSuccess("Calling model constructor.");
					model = new RsModel(viewAdapter, _databaseName, _tableName, _columns, 
					                    _columnTypes, _regkey, _xmlReportsName, _rdlFolder, _reportsXPathExpression,
					                    _startMenuPath, _shortcutFile, _requiredPermissions, _parentPath, 
					                    _folderDescription, _dataSourceName, _dataSourceDescription,
					                    _reportingServicesTemplate, _mainTemplate, _reportsTemplate);
					ErrorLog.Singleton.LogSuccess("model initialized.");
				}
				catch (Exception ex)
				{
					_success = false;
					ErrorLog.Singleton.LogError("Backend initialization error.  Cannot complete installation.",
						ex.Message, ex.StackTrace);
					MessageBox.Show("Backend initialization error.  Cannot complete installation.", "Initialization Error!");
				}
				if (_success)
				{
					try
					{
						view = new InstallerView(modelAdapter, _header, _productName,
							_productAbbrev, _folder, _firstPanelImage, _lastPanelImage, _icon);
					}
					catch (Exception ex)
					{
						_success = false;
						ErrorLog.Singleton.LogError("Backend initialization error.  Cannot complete installation.",
							ex.Message, ex.StackTrace);
						MessageBox.Show("Backend initialization error.  Cannot complete installation.", "Initialization Error!");
					}
					if (_success)
					{
						if ((model != null) && (view != null) && (modelAdapter != null) && (viewAdapter != null))
						{
							((ModelAccessAdapter)modelAdapter).Model = model;
							((ViewAccessAdapter)viewAdapter).View = view;
							ErrorLog.Singleton.LogSuccess("RsInstallationController initialization complete.");
							view.Run();
						}
						else
						{
							ErrorLog.Singleton.LogWarning("One or more objects was not initialized properly.  Cannot complete installation.");
							MessageBox.Show("One or more objects was not initialized properly.  Cannot complete installation.", "Initialization Error!");
						}
					}
				}
			}
		}

		/// <summary>
		/// Creates a string array for parameters in the app.config file.
		/// </summary>
		/// <param name="number">The number of elements in the array.</param>
		/// <param name="name">The base name of the array.</param>
		/// <returns>string array</returns>
		private string[] ConfigStringArray(string number, string name)
		{
			int index = 2;
			try
			{
				index = int.Parse((ConfigurationSettings.AppSettings[number]));
			}
			catch (Exception)
			{
				string[] columns = {"reportServer", "reportFolder", "reportingServicesTemplate",
									   "mainTemplate", "reportemplate"};
				return columns;
			}

			string[] array = new string[index];
			for (int i = 0; i < index; i++)
			{
				array[i] = (ConfigurationSettings.AppSettings[name+i]);
				if (array[i] == null)
				{
					string[] columns = {"reportServer", "reportFolder", "reportingServicesTemplate",
										   "mainTemplate", "reportemplate"};
					return columns;
				}
			}
			return array;
		}

		/// <summary>
		/// Creates a type array for parameters in the app.config file.
		/// </summary>
		/// <param name="number">The number of elements in the array.</param>
		/// <param name="name">The base name of the array.</param>
		/// <returns>type array</returns>
		private Type[] ConfigTypeArray(string number, string name)
		{
			int index = 2;
			try
			{
				index = int.Parse((ConfigurationSettings.AppSettings[number]));
			}
			catch (Exception)
			{
				Type[] columnTypes = {
				                     	Type.GetType("System.String"), Type.GetType("System.String"),
				                     	Type.GetType("System.String"), Type.GetType("System.String"), Type.GetType("System.String")
				                     };
				return columnTypes;
			}

			Type[] array = new Type[index];
			for (int i = 0; i < index; i++)
			{
				array[i] = Type.GetType(ConfigurationSettings.AppSettings[name+i]);
				if (array[i] == null)
				{
					Type[] columnTypes = {
					                     	Type.GetType("System.String"), Type.GetType("System.String"),
					                     	Type.GetType("System.String"), Type.GetType("System.String"), Type.GetType("System.String")
					                     };
					return columnTypes;
				}
			}
			return array;
		}

		//-----------------------------------------------------------------------------------------
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		//-----------------------------------------------------------------------------------------
		[STAThread]
		public static void Main() 
		{
			RsInstallationController controller = new RsInstallationController();
			controller.Start();
		}

	}
}
