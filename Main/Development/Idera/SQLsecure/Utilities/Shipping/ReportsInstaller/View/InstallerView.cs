using System ;
using System.Collections ;
using System.Diagnostics ;
using System.Drawing ;
using System.Security.Principal ;
using System.Threading ;
using System.Windows.Forms ;
using Idera.Common.ReportsInstaller.View.HelperForms ;
using Idera.Common.ReportsInstaller.View.Wizard ;
using File=System.IO.File ;

namespace Idera.Common.ReportsInstaller.View
{
	/// <summary>
	/// Adds a report to the already installed list box (listBoxReports).
	/// </summary>
	public class InstallerView : Idera.Common.ReportsInstaller.View.Wizard.WizardForm, IInstallerGUI
	{
		/// <summary>
		/// The current panel in the wizard.
		/// </summary>
		private WizardPanel _currentPanel;

		/// <summary>
		/// The reporting services folder to store reports in.
		/// </summary>
		private string _rsFolder;

		/// <summary>
		/// An instance of the SQLInstance form.
		/// </summary>
		private SQLInstanceForm _sqlInstanceForm;

		/// <summary>
		/// An instance of the folder form.
		/// </summary>
		private FolderForm _folderForm;

		/// <summary>
		/// The state of the Install radio button (true if checked).
		/// </summary>
		private bool _radioInstallChecked;

		/// <summary>
		/// The state of the Update radio button (true if checked).
		/// </summary>
		private bool _radioUpdateChecked;

		/// <summary>
		/// The state of the Add radio button (true if checked).
		/// </summary>
		private bool _radioAddChecked;

		private string _computerName = "";
		private string _computerPortName = "";
		/// <summary>
		/// Retrieves the computer name in case it's "localhost".
		/// </summary>
		public string ComputerName
		{
			get
			{ 
				if (_computerName.Equals("localhost"))
				{
					return Environment.MachineName;
				}
				else
				{
					return _computerName;
				}
			}
		}

		/// <summary>
		/// The name of the product.
		/// </summary>
		private string _productName;
		
		/// <summary>
		/// The abbreviated name of the product.
		/// </summary>
		private string _productAbbrev;

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

		private string _sqlServerName = "";
		/// <summary>
		/// Retrieves the server name in case it's "LOCAL" or ".".
		/// </summary>
		public string SqlServerName
		{
			get
			{ 
				if ((_sqlServerName.ToUpper().StartsWith( "(LOCAL)")) || (_sqlServerName==".") || (_sqlServerName=="localhost"))
				{
					return Environment.MachineName;
				}
				else
				{
					return _sqlServerName;
				}
			}
		}

		/// <summary>
		/// Whether a new folder needs to be created.
		/// </summary>
		private bool _createNewFolder;

		/// <summary>
		/// Whether a new data source needs to be created.
		/// </summary>
		private bool _createNewDataSource;
		
		public string _formHeader;

		# region Registry Values
		/// <summary>
		/// The report server value stored in the registry.
		/// </summary>
		private string _regReportServer;

		/// <summary>
		/// The report folder value stored in the registry.
		/// </summary>
		private string _regReportFolder;

		/// <summary>
		/// The report server virtual directory value stored in the registry.
		/// </summary>
		private string _regReportDirectory;

		/// <summary>
		/// The report manager virtual directory value stored in the registry.
		/// </summary>
		private string _regReportManager;

		/// <summary>
		/// The repository sql server value stored in the registry.
		/// </summary>
		private string _regSqlServer;

		/// <summary>
		/// The login name value stored in the registry.
		/// </summary>
		private string _regLogin;

		/// <summary>
		/// The ssl checkbox value stored in the registry.
		/// </summary>
		private string _regSsl;

		/// <summary>
		/// The port number value stored in the registry.
		/// </summary>
		private string _regPort;

		/// <summary>
		/// The report server value label stored in the registry.
		/// </summary>
		private string _regReportServerName;

		/// <summary>
		/// The report folder value label stored in the registry.
		/// </summary>
		private string _regReportFolderName;

		/// <summary>
		/// The report server virtual directory value label stored in the registry.
		/// </summary>
		private string _regReportDirectoryName;

		/// <summary>
		/// The report manager virtual directory value label stored in the registry.
		/// </summary>
		private string _regReportManagerName;

		/// <summary>
		/// The repository sql server value label stored in the registry.
		/// </summary>
		private string _regSqlServerName;

		/// <summary>
		/// The login name value label stored in the registry.
		/// </summary>
		private string _regLoginName;

		/// <summary>
		/// The ssl checkbox value label stored in the registry.
		/// </summary>
		private string _regSslName;

		/// <summary>
		/// The port number value label stored in the registry.
		/// </summary>
		private string _regPortName;
		#endregion

		private Idera.Common.ReportsInstaller.View.Wizard.WizardPanel panelInstallationOptions;
		private System.Windows.Forms.PictureBox pictureBoxInstallationOptionsIntroSplash;
		private System.Windows.Forms.Panel panelInstallationOptionsIntroSplashBackground;
		private System.Windows.Forms.Label labelInstallationOptionsWelcome;
		private System.Windows.Forms.Panel panelInstallationOptionsBackground;
		private System.Windows.Forms.GroupBox groupInstallationOptions;
		private System.Windows.Forms.RadioButton radioInstall;
		private System.Windows.Forms.RadioButton radioChange;
		private Idera.Common.ReportsInstaller.View.Wizard.WizardPanel panelReportingServices;
		private System.Windows.Forms.GroupBox groupReportingServices;
		private System.Windows.Forms.Label labelReportingServicesIntro;
		private System.Windows.Forms.Label labelReportingServicesNote;
		private System.Windows.Forms.Label labelReportingServicesComputer;
		private System.Windows.Forms.TextBox textBoxReportingServicesComputer;
		private System.Windows.Forms.Button buttonReportingServicesAdvanced;
		private Idera.Common.ReportsInstaller.View.Wizard.WizardPanel panelReportingServicesAdvanced;
		private System.Windows.Forms.GroupBox groupReportingServicesAdvanced;
		private System.Windows.Forms.Label labelReportingServicesAdvancedIntro;
		private System.Windows.Forms.Label labelReportingServicesAdvancedPort;
		private System.Windows.Forms.Label labelReportingServicesAdvancedDirectory;
		private System.Windows.Forms.Label labelReportingServicesAdvancedManager;
		private System.Windows.Forms.TextBox textBoxReportingServicesAdvancedPort;
		private System.Windows.Forms.TextBox textBoxReportingServicesAdvancedDirectory;
		private System.Windows.Forms.TextBox textBoxReportingServicesAdvancedManager;
		private System.Windows.Forms.CheckBox checkBoxReportingServicesAdvancedSsl;
		private System.Windows.Forms.Button buttonReportingServicesAdvancedDefault;
		private System.Windows.Forms.Label labelReportingServicesAdvancedNote;
		private Idera.Common.ReportsInstaller.View.Wizard.WizardPanel panelRepositoryInstall;
		private System.Windows.Forms.GroupBox groupRepositoryInstall;
		private System.Windows.Forms.Label labelRepositoryInstallIntro;
		private System.Windows.Forms.Label labelRepositoryInstallServer;
		private System.Windows.Forms.TextBox textBoxRepositoryInstallServer;
		private System.Windows.Forms.Button buttonRepositoryInstallBrowse;
		private System.Windows.Forms.Label labelRepositoryInstallCredentials;
		private System.Windows.Forms.Label labelRepositoryInstallLogin;
		private System.Windows.Forms.Label labelRepositoryInstallPassword;
		private System.Windows.Forms.TextBox textBoxRepositoryInstallLogin;
		private System.Windows.Forms.TextBox textBoxRepositoryInstallPassword;
		private System.Windows.Forms.Label labelRepositoryInstallFolderCreate;
		private System.Windows.Forms.Label labelRepositoryInstallFolder;
		private System.Windows.Forms.TextBox textBoxRepositoryInstallFolder;
		private Idera.Common.ReportsInstaller.View.Wizard.WizardPanel panelRepositoryChange;
		private System.Windows.Forms.GroupBox groupBoxRepositoryChange;
		private System.Windows.Forms.TextBox textBoxRepositoryChangeFolder;
		private System.Windows.Forms.Label labelRepositoryChangeFolder;
		private System.Windows.Forms.Label labelRepositoryChangeFolderCreate;
		private System.Windows.Forms.TextBox textBoxRepositoryChangePassword;
		private System.Windows.Forms.TextBox textBoxRepositoryChangeLogin;
		private System.Windows.Forms.Label labelRepositoryChangePassword;
		private System.Windows.Forms.Label labelRepositoryChangeLogin;
		private System.Windows.Forms.Label labelRepositoryChangeCredentials;
		private System.Windows.Forms.Button buttonRepositoryChangeBrowse;
		private System.Windows.Forms.TextBox textBoxRepositoryChangeServer;
		private System.Windows.Forms.Label labelRepositoryChangeServer;
		private System.Windows.Forms.Label labelRepositoryChangeIntro;
		private System.Windows.Forms.Button buttonRepositoryChangeBrowseFolder;
		private System.Windows.Forms.GroupBox groupBoxReports;
		private System.Windows.Forms.Label labelReportsIntro;
		private System.Windows.Forms.RadioButton radioUpdate;
		private System.Windows.Forms.RadioButton radioAdd;
		private System.Windows.Forms.RadioButton radioCustom;
		private Idera.Common.ReportsInstaller.View.Wizard.WizardPanel panelReportsCustom;
		private System.Windows.Forms.GroupBox groupReportsCustom;
		private System.Windows.Forms.Label labelReportsCustomSelect;
		private System.Windows.Forms.Label labelReportsCustomUpdate;
		private System.Windows.Forms.CheckedListBox checkedListBoxReportsCustomAdd;
		private System.Windows.Forms.CheckedListBox checkedListBoxReportsCustomUpdate;
		private System.Windows.Forms.Button buttonReportsCustomAddSelect;
		private System.Windows.Forms.Button buttonReportsCustomAddUnselect;
		private System.Windows.Forms.Button buttonReportsCustomUpdateSelect;
		private System.Windows.Forms.Button buttonReportsCustomUpdateUnselect;
		private Idera.Common.ReportsInstaller.View.Wizard.WizardPanel panelSummary;
		private System.Windows.Forms.GroupBox groupSummary;
		private System.Windows.Forms.Label labelSummaryIntro;
		private System.Windows.Forms.Label labelSummaryFinish;
		private System.Windows.Forms.Label labelSummaryAdded;
		private System.Windows.Forms.Label labelSummaryUpdate;
		private Idera.Common.ReportsInstaller.View.Wizard.WizardPanel panelProgress;
		private System.Windows.Forms.GroupBox groupProgress;
		private System.Windows.Forms.RichTextBox richTextBoxProgress;
		private Idera.Common.ReportsInstaller.View.Wizard.WizardPanel panelComplete;
		private System.Windows.Forms.PictureBox pictureBoxComplete;
		private System.Windows.Forms.Panel panelCompleteIntroSplashBackground;
		private System.Windows.Forms.Label labelCompleteIntro;
		private System.Windows.Forms.Panel panelCompleteBackground;
		private System.Windows.Forms.GroupBox groupBoxComplete;
		private System.Windows.Forms.Label labelCompleteInstallIntro;
		private System.Windows.Forms.Label labelCompleteInstallChanges;
		private System.Windows.Forms.Label labelCompleteReports;
		private System.Windows.Forms.LinkLabel linkLabelComplete;
		private System.Windows.Forms.ListBox listBoxComplete;
		private Idera.Common.ReportsInstaller.View.Wizard.WizardPanel panelReports;
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label labelSummaryFolderDs;
		private System.Windows.Forms.Button buttonRepositoryInstallBrowseFolder;
		private System.Windows.Forms.Button buttonCompleteError;
		private System.Windows.Forms.Label labelSummaryList;

		private IModelAccessAdapter modelAdapter;
		/// <summary>
		/// Gives the view access to certain methods in the model.
		/// </summary>
		public IModelAccessAdapter ModelAdapter
		{
			get
			{
				return modelAdapter;
			}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="modelAdapter">Gives the view access to certain methods in the model.</param>
		/// <param name="header">The header of this GUI.</param>
		/// <param name="productName">The name of the product.</param>
		/// <param name="productAbbrev">The abbreviated name of the product.</param>
		/// <param name="folder">The name of the reports folder.</param>
		/// <param name="firstPanelImage">The image on the first panel.</param>
		/// <param name="lastPanelImage">The image on the last panel.</param>
		/// <param name="warningIcon">The icon for warnings.</param>
		/// <param name="icon">The icon in the form.</param>
		public InstallerView(IModelAccessAdapter _modelAdapter, string header,
			string productName, string productAbbrev, string folder, 
			string firstPanelImage, string lastPanelImage, string icon)
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			modelAdapter = _modelAdapter;
			this.Text = header;
			_formHeader = header;
			_sqlInstanceForm = new SQLInstanceForm();
			_folderForm = new FolderForm();
			_productName = productName;
			_productAbbrev = productAbbrev;
			_rsFolder = folder;
			_firstPanelImage = firstPanelImage;
			_lastPanelImage = lastPanelImage;
			_icon = icon;

			_regReportServerName = "ReportServer";
			_regReportFolderName = "ReportFolder";
			_regReportDirectoryName = "ReportDirectory";
			_regReportManagerName = "ReportManager";
			_regSqlServerName = "SqlServer";
			_regLoginName = "Login";
			_regSslName = "SslCheckbox";
			_regPortName = "Port";

			_regReportServer = "localhost";
			_regReportFolder = _rsFolder;
			_regReportDirectory = "reportServer";
			_regReportManager = "reports";
			_regSqlServer = "(LOCAL)";
			_regLogin = WindowsIdentity.GetCurrent().Name;
			_regSsl = ((Boolean)false).ToString();
			_regPort = "80";

			// initialize text fields with the name of the product
			this.labelCompleteInstallIntro.Text = "You have successfully completed the "+_productAbbrev+" Reports Installer.";
			this.groupBoxRepositoryChange.Text = "Change "+_productName+" Repository Configuration";
			this.labelRepositoryChangeIntro.Text = "Specify which SQL Server instance hosts the "+ _productName +" Repository:";
			this.labelRepositoryChangeFolderCreate.Text = "Specify or browse for the folder that hosts the "+_productName+" reports.";
			this.groupRepositoryInstall.Text = "Set "+_productName+" Repository Configuration";
			this.labelRepositoryInstallIntro.Text = "Specify which SQL Server instance hosts the "+ _productName +" Repository:";
			this.labelRepositoryInstallFolderCreate.Text = "Specify a new folder name that will host the "+_productName+" reports.";
			this.labelReportingServicesIntro.Text = "Specify the Report Server computer that hosts the "+_productName+" reports.";
			this.labelInstallationOptionsWelcome.Text = "Welcome to the Idera "+ _productAbbrev +" Reports Installer";
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(InstallerView));
			this.panelInstallationOptions = new Idera.Common.ReportsInstaller.View.Wizard.WizardPanel();
			this.panelInstallationOptionsBackground = new System.Windows.Forms.Panel();
			this.groupInstallationOptions = new System.Windows.Forms.GroupBox();
			this.radioChange = new System.Windows.Forms.RadioButton();
			this.radioInstall = new System.Windows.Forms.RadioButton();
			this.labelInstallationOptionsWelcome = new System.Windows.Forms.Label();
			this.panelInstallationOptionsIntroSplashBackground = new System.Windows.Forms.Panel();
			this.pictureBoxInstallationOptionsIntroSplash = new System.Windows.Forms.PictureBox();
			this.panelReportingServices = new Idera.Common.ReportsInstaller.View.Wizard.WizardPanel();
			this.groupReportingServices = new System.Windows.Forms.GroupBox();
			this.buttonReportingServicesAdvanced = new System.Windows.Forms.Button();
			this.textBoxReportingServicesComputer = new System.Windows.Forms.TextBox();
			this.labelReportingServicesComputer = new System.Windows.Forms.Label();
			this.labelReportingServicesNote = new System.Windows.Forms.Label();
			this.labelReportingServicesIntro = new System.Windows.Forms.Label();
			this.panelReportingServicesAdvanced = new Idera.Common.ReportsInstaller.View.Wizard.WizardPanel();
			this.groupReportingServicesAdvanced = new System.Windows.Forms.GroupBox();
			this.labelReportingServicesAdvancedNote = new System.Windows.Forms.Label();
			this.buttonReportingServicesAdvancedDefault = new System.Windows.Forms.Button();
			this.checkBoxReportingServicesAdvancedSsl = new System.Windows.Forms.CheckBox();
			this.textBoxReportingServicesAdvancedManager = new System.Windows.Forms.TextBox();
			this.textBoxReportingServicesAdvancedDirectory = new System.Windows.Forms.TextBox();
			this.textBoxReportingServicesAdvancedPort = new System.Windows.Forms.TextBox();
			this.labelReportingServicesAdvancedManager = new System.Windows.Forms.Label();
			this.labelReportingServicesAdvancedDirectory = new System.Windows.Forms.Label();
			this.labelReportingServicesAdvancedPort = new System.Windows.Forms.Label();
			this.labelReportingServicesAdvancedIntro = new System.Windows.Forms.Label();
			this.panelRepositoryInstall = new Idera.Common.ReportsInstaller.View.Wizard.WizardPanel();
			this.groupRepositoryInstall = new System.Windows.Forms.GroupBox();
			this.buttonRepositoryInstallBrowseFolder = new System.Windows.Forms.Button();
			this.textBoxRepositoryInstallFolder = new System.Windows.Forms.TextBox();
			this.labelRepositoryInstallFolder = new System.Windows.Forms.Label();
			this.labelRepositoryInstallFolderCreate = new System.Windows.Forms.Label();
			this.textBoxRepositoryInstallPassword = new System.Windows.Forms.TextBox();
			this.textBoxRepositoryInstallLogin = new System.Windows.Forms.TextBox();
			this.labelRepositoryInstallPassword = new System.Windows.Forms.Label();
			this.labelRepositoryInstallLogin = new System.Windows.Forms.Label();
			this.labelRepositoryInstallCredentials = new System.Windows.Forms.Label();
			this.buttonRepositoryInstallBrowse = new System.Windows.Forms.Button();
			this.textBoxRepositoryInstallServer = new System.Windows.Forms.TextBox();
			this.labelRepositoryInstallServer = new System.Windows.Forms.Label();
			this.labelRepositoryInstallIntro = new System.Windows.Forms.Label();
			this.panelRepositoryChange = new Idera.Common.ReportsInstaller.View.Wizard.WizardPanel();
			this.groupBoxRepositoryChange = new System.Windows.Forms.GroupBox();
			this.buttonRepositoryChangeBrowseFolder = new System.Windows.Forms.Button();
			this.textBoxRepositoryChangeFolder = new System.Windows.Forms.TextBox();
			this.labelRepositoryChangeFolder = new System.Windows.Forms.Label();
			this.labelRepositoryChangeFolderCreate = new System.Windows.Forms.Label();
			this.textBoxRepositoryChangePassword = new System.Windows.Forms.TextBox();
			this.textBoxRepositoryChangeLogin = new System.Windows.Forms.TextBox();
			this.labelRepositoryChangePassword = new System.Windows.Forms.Label();
			this.labelRepositoryChangeLogin = new System.Windows.Forms.Label();
			this.labelRepositoryChangeCredentials = new System.Windows.Forms.Label();
			this.buttonRepositoryChangeBrowse = new System.Windows.Forms.Button();
			this.textBoxRepositoryChangeServer = new System.Windows.Forms.TextBox();
			this.labelRepositoryChangeServer = new System.Windows.Forms.Label();
			this.labelRepositoryChangeIntro = new System.Windows.Forms.Label();
			this.panelReports = new Idera.Common.ReportsInstaller.View.Wizard.WizardPanel();
			this.groupBoxReports = new System.Windows.Forms.GroupBox();
			this.radioCustom = new System.Windows.Forms.RadioButton();
			this.radioAdd = new System.Windows.Forms.RadioButton();
			this.radioUpdate = new System.Windows.Forms.RadioButton();
			this.labelReportsIntro = new System.Windows.Forms.Label();
			this.panelReportsCustom = new Idera.Common.ReportsInstaller.View.Wizard.WizardPanel();
			this.groupReportsCustom = new System.Windows.Forms.GroupBox();
			this.buttonReportsCustomUpdateUnselect = new System.Windows.Forms.Button();
			this.buttonReportsCustomUpdateSelect = new System.Windows.Forms.Button();
			this.buttonReportsCustomAddUnselect = new System.Windows.Forms.Button();
			this.buttonReportsCustomAddSelect = new System.Windows.Forms.Button();
			this.checkedListBoxReportsCustomUpdate = new System.Windows.Forms.CheckedListBox();
			this.checkedListBoxReportsCustomAdd = new System.Windows.Forms.CheckedListBox();
			this.labelReportsCustomUpdate = new System.Windows.Forms.Label();
			this.labelReportsCustomSelect = new System.Windows.Forms.Label();
			this.panelSummary = new Idera.Common.ReportsInstaller.View.Wizard.WizardPanel();
			this.groupSummary = new System.Windows.Forms.GroupBox();
			this.labelSummaryList = new System.Windows.Forms.Label();
			this.labelSummaryFolderDs = new System.Windows.Forms.Label();
			this.labelSummaryUpdate = new System.Windows.Forms.Label();
			this.labelSummaryAdded = new System.Windows.Forms.Label();
			this.labelSummaryFinish = new System.Windows.Forms.Label();
			this.labelSummaryIntro = new System.Windows.Forms.Label();
			this.panelProgress = new Idera.Common.ReportsInstaller.View.Wizard.WizardPanel();
			this.groupProgress = new System.Windows.Forms.GroupBox();
			this.richTextBoxProgress = new System.Windows.Forms.RichTextBox();
			this.panelComplete = new Idera.Common.ReportsInstaller.View.Wizard.WizardPanel();
			this.panelCompleteBackground = new System.Windows.Forms.Panel();
			this.groupBoxComplete = new System.Windows.Forms.GroupBox();
			this.buttonCompleteError = new System.Windows.Forms.Button();
			this.listBoxComplete = new System.Windows.Forms.ListBox();
			this.linkLabelComplete = new System.Windows.Forms.LinkLabel();
			this.labelCompleteReports = new System.Windows.Forms.Label();
			this.labelCompleteInstallChanges = new System.Windows.Forms.Label();
			this.labelCompleteInstallIntro = new System.Windows.Forms.Label();
			this.labelCompleteIntro = new System.Windows.Forms.Label();
			this.panelCompleteIntroSplashBackground = new System.Windows.Forms.Panel();
			this.pictureBoxComplete = new System.Windows.Forms.PictureBox();
			this.panelInstallationOptions.SuspendLayout();
			this.panelInstallationOptionsBackground.SuspendLayout();
			this.groupInstallationOptions.SuspendLayout();
			this.panelReportingServices.SuspendLayout();
			this.groupReportingServices.SuspendLayout();
			this.panelReportingServicesAdvanced.SuspendLayout();
			this.groupReportingServicesAdvanced.SuspendLayout();
			this.panelRepositoryInstall.SuspendLayout();
			this.groupRepositoryInstall.SuspendLayout();
			this.panelRepositoryChange.SuspendLayout();
			this.groupBoxRepositoryChange.SuspendLayout();
			this.panelReports.SuspendLayout();
			this.groupBoxReports.SuspendLayout();
			this.panelReportsCustom.SuspendLayout();
			this.groupReportsCustom.SuspendLayout();
			this.panelSummary.SuspendLayout();
			this.groupSummary.SuspendLayout();
			this.panelProgress.SuspendLayout();
			this.groupProgress.SuspendLayout();
			this.panelComplete.SuspendLayout();
			this.panelCompleteBackground.SuspendLayout();
			this.groupBoxComplete.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonPrev
			// 
			this.buttonPrev.Location = new System.Drawing.Point(208, 336);
			this.buttonPrev.Name = "buttonPrev";
			this.buttonPrev.Text = "&Previous";
			// 
			// buttonNext
			// 
			this.buttonNext.Location = new System.Drawing.Point(280, 336);
			this.buttonNext.Name = "buttonNext";
			this.buttonNext.Text = "&Next";
			// 
			// buttonFinish
			// 
			this.buttonFinish.Location = new System.Drawing.Point(384, 336);
			this.buttonFinish.Name = "buttonFinish";
			this.buttonFinish.Text = "&Install";
			// 
			// buttonCancel
			// 
			this.buttonCancel.Location = new System.Drawing.Point(472, 336);
			this.buttonCancel.Name = "buttonCancel";
			// 
			// panelInstallationOptions
			// 
			this.panelInstallationOptions.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.panelInstallationOptions.Controls.Add(this.panelInstallationOptionsBackground);
			this.panelInstallationOptions.Controls.Add(this.labelInstallationOptionsWelcome);
			this.panelInstallationOptions.Controls.Add(this.panelInstallationOptionsIntroSplashBackground);
			this.panelInstallationOptions.Controls.Add(this.pictureBoxInstallationOptionsIntroSplash);
			this.panelInstallationOptions.Location = new System.Drawing.Point(0, 0);
			this.panelInstallationOptions.Name = "panelInstallationOptions";
			this.panelInstallationOptions.NextPanelEvent = null;
			this.panelInstallationOptions.PreviousPanelEvent = null;
			this.panelInstallationOptions.Size = new System.Drawing.Size(560, 336);
			this.panelInstallationOptions.TabIndex = 4;
			// 
			// panelInstallationOptionsBackground
			// 
			this.panelInstallationOptionsBackground.BackColor = System.Drawing.SystemColors.Control;
			this.panelInstallationOptionsBackground.Controls.Add(this.groupInstallationOptions);
			this.panelInstallationOptionsBackground.Location = new System.Drawing.Point(184, 72);
			this.panelInstallationOptionsBackground.Name = "panelInstallationOptionsBackground";
			this.panelInstallationOptionsBackground.Size = new System.Drawing.Size(376, 264);
			this.panelInstallationOptionsBackground.TabIndex = 3;
			// 
			// groupInstallationOptions
			// 
			this.groupInstallationOptions.Controls.Add(this.radioChange);
			this.groupInstallationOptions.Controls.Add(this.radioInstall);
			this.groupInstallationOptions.Location = new System.Drawing.Point(8, 8);
			this.groupInstallationOptions.Name = "groupInstallationOptions";
			this.groupInstallationOptions.Size = new System.Drawing.Size(360, 248);
			this.groupInstallationOptions.TabIndex = 0;
			this.groupInstallationOptions.TabStop = false;
			this.groupInstallationOptions.Text = "Select Installation Option";
			// 
			// radioChange
			// 
			this.radioChange.Location = new System.Drawing.Point(16, 112);
			this.radioChange.Name = "radioChange";
			this.radioChange.Size = new System.Drawing.Size(280, 24);
			this.radioChange.TabIndex = 5;
			this.radioChange.TabStop = true;
			this.radioChange.Text = "Change &Repository Configuration";
			this.radioChange.CheckedChanged += new System.EventHandler(this.radioChange_CheckedChanged);
			// 
			// radioInstall
			// 
			this.radioInstall.Checked = true;
			this.radioInstall.Location = new System.Drawing.Point(16, 40);
			this.radioInstall.Name = "radioInstall";
			this.radioInstall.Size = new System.Drawing.Size(128, 24);
			this.radioInstall.TabIndex = 4;
			this.radioInstall.TabStop = true;
			this.radioInstall.Text = "Install or &Upgrade";
			this.radioInstall.CheckedChanged += new System.EventHandler(this.radioInstall_CheckedChanged);
			// 
			// labelInstallationOptionsWelcome
			// 
			this.labelInstallationOptionsWelcome.Font = new System.Drawing.Font("Microsoft Sans Serif", 17F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelInstallationOptionsWelcome.Location = new System.Drawing.Point(192, 8);
			this.labelInstallationOptionsWelcome.Name = "labelInstallationOptionsWelcome";
			this.labelInstallationOptionsWelcome.Size = new System.Drawing.Size(352, 56);
			this.labelInstallationOptionsWelcome.TabIndex = 2;
			this.labelInstallationOptionsWelcome.Text = "Welcome to the Idera <productAbbrev> Reports Installer";
			// 
			// panelInstallationOptionsIntroSplashBackground
			// 
			this.panelInstallationOptionsIntroSplashBackground.BackColor = System.Drawing.SystemColors.Control;
			this.panelInstallationOptionsIntroSplashBackground.Location = new System.Drawing.Point(0, 328);
			this.panelInstallationOptionsIntroSplashBackground.Name = "panelInstallationOptionsIntroSplashBackground";
			this.panelInstallationOptionsIntroSplashBackground.Size = new System.Drawing.Size(184, 96);
			this.panelInstallationOptionsIntroSplashBackground.TabIndex = 1;
			// 
			// pictureBoxInstallationOptionsIntroSplash
			// 
			this.pictureBoxInstallationOptionsIntroSplash.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pictureBoxInstallationOptionsIntroSplash.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxInstallationOptionsIntroSplash.Image")));
			this.pictureBoxInstallationOptionsIntroSplash.Location = new System.Drawing.Point(0, 0);
			this.pictureBoxInstallationOptionsIntroSplash.Name = "pictureBoxInstallationOptionsIntroSplash";
			this.pictureBoxInstallationOptionsIntroSplash.Size = new System.Drawing.Size(184, 336);
			this.pictureBoxInstallationOptionsIntroSplash.TabIndex = 0;
			this.pictureBoxInstallationOptionsIntroSplash.TabStop = false;
			// 
			// panelReportingServices
			// 
			this.panelReportingServices.BackColor = System.Drawing.SystemColors.Control;
			this.panelReportingServices.Controls.Add(this.groupReportingServices);
			this.panelReportingServices.Location = new System.Drawing.Point(0, 0);
			this.panelReportingServices.Name = "panelReportingServices";
			this.panelReportingServices.NextPanelEvent = null;
			this.panelReportingServices.PreviousPanelEvent = null;
			this.panelReportingServices.Size = new System.Drawing.Size(560, 336);
			this.panelReportingServices.TabIndex = 5;
			// 
			// groupReportingServices
			// 
			this.groupReportingServices.Controls.Add(this.buttonReportingServicesAdvanced);
			this.groupReportingServices.Controls.Add(this.textBoxReportingServicesComputer);
			this.groupReportingServices.Controls.Add(this.labelReportingServicesComputer);
			this.groupReportingServices.Controls.Add(this.labelReportingServicesNote);
			this.groupReportingServices.Controls.Add(this.labelReportingServicesIntro);
			this.groupReportingServices.Location = new System.Drawing.Point(8, 8);
			this.groupReportingServices.Name = "groupReportingServices";
			this.groupReportingServices.Size = new System.Drawing.Size(544, 320);
			this.groupReportingServices.TabIndex = 0;
			this.groupReportingServices.TabStop = false;
			this.groupReportingServices.Text = "Connect to Microsoft Reporting Services";
			// 
			// buttonReportingServicesAdvanced
			// 
			this.buttonReportingServicesAdvanced.Location = new System.Drawing.Point(392, 192);
			this.buttonReportingServicesAdvanced.Name = "buttonReportingServicesAdvanced";
			this.buttonReportingServicesAdvanced.Size = new System.Drawing.Size(88, 23);
			this.buttonReportingServicesAdvanced.TabIndex = 5;
			this.buttonReportingServicesAdvanced.Text = "&Advanced...";
			this.buttonReportingServicesAdvanced.Click += new System.EventHandler(this.buttonReportingServicesAdvanced_Click);
			// 
			// textBoxReportingServicesComputer
			// 
			this.textBoxReportingServicesComputer.Location = new System.Drawing.Point(184, 96);
			this.textBoxReportingServicesComputer.Name = "textBoxReportingServicesComputer";
			this.textBoxReportingServicesComputer.Size = new System.Drawing.Size(296, 20);
			this.textBoxReportingServicesComputer.TabIndex = 4;
			this.textBoxReportingServicesComputer.Text = "";
			// 
			// labelReportingServicesComputer
			// 
			this.labelReportingServicesComputer.Location = new System.Drawing.Point(48, 96);
			this.labelReportingServicesComputer.Name = "labelReportingServicesComputer";
			this.labelReportingServicesComputer.Size = new System.Drawing.Size(136, 23);
			this.labelReportingServicesComputer.TabIndex = 2;
			this.labelReportingServicesComputer.Text = "Report Server C&omputer:";
			this.labelReportingServicesComputer.Enter += new System.EventHandler(this.labelReportingServicesComputer_Enter);
			// 
			// labelReportingServicesNote
			// 
			this.labelReportingServicesNote.Location = new System.Drawing.Point(16, 248);
			this.labelReportingServicesNote.Name = "labelReportingServicesNote";
			this.labelReportingServicesNote.Size = new System.Drawing.Size(520, 40);
			this.labelReportingServicesNote.TabIndex = 1;
			this.labelReportingServicesNote.Text = "Note: To successfully deploy reports using this utility, your logon account must " +
				"have Content Manager rights on the Report Server. For more information, see the " +
				"Reporting Services Books Online. ";
			// 
			// labelReportingServicesIntro
			// 
			this.labelReportingServicesIntro.Location = new System.Drawing.Point(16, 24);
			this.labelReportingServicesIntro.Name = "labelReportingServicesIntro";
			this.labelReportingServicesIntro.Size = new System.Drawing.Size(520, 40);
			this.labelReportingServicesIntro.TabIndex = 0;
			this.labelReportingServicesIntro.Text = "Specify the Report Server computer that hosts the <productName> reports.";
			// 
			// panelReportingServicesAdvanced
			// 
			this.panelReportingServicesAdvanced.BackColor = System.Drawing.SystemColors.Control;
			this.panelReportingServicesAdvanced.Controls.Add(this.groupReportingServicesAdvanced);
			this.panelReportingServicesAdvanced.Location = new System.Drawing.Point(0, 0);
			this.panelReportingServicesAdvanced.Name = "panelReportingServicesAdvanced";
			this.panelReportingServicesAdvanced.NextPanelEvent = null;
			this.panelReportingServicesAdvanced.PreviousPanelEvent = null;
			this.panelReportingServicesAdvanced.Size = new System.Drawing.Size(560, 336);
			this.panelReportingServicesAdvanced.TabIndex = 6;
			// 
			// groupReportingServicesAdvanced
			// 
			this.groupReportingServicesAdvanced.Controls.Add(this.labelReportingServicesAdvancedNote);
			this.groupReportingServicesAdvanced.Controls.Add(this.buttonReportingServicesAdvancedDefault);
			this.groupReportingServicesAdvanced.Controls.Add(this.checkBoxReportingServicesAdvancedSsl);
			this.groupReportingServicesAdvanced.Controls.Add(this.textBoxReportingServicesAdvancedManager);
			this.groupReportingServicesAdvanced.Controls.Add(this.textBoxReportingServicesAdvancedDirectory);
			this.groupReportingServicesAdvanced.Controls.Add(this.textBoxReportingServicesAdvancedPort);
			this.groupReportingServicesAdvanced.Controls.Add(this.labelReportingServicesAdvancedManager);
			this.groupReportingServicesAdvanced.Controls.Add(this.labelReportingServicesAdvancedDirectory);
			this.groupReportingServicesAdvanced.Controls.Add(this.labelReportingServicesAdvancedPort);
			this.groupReportingServicesAdvanced.Controls.Add(this.labelReportingServicesAdvancedIntro);
			this.groupReportingServicesAdvanced.Location = new System.Drawing.Point(8, 8);
			this.groupReportingServicesAdvanced.Name = "groupReportingServicesAdvanced";
			this.groupReportingServicesAdvanced.Size = new System.Drawing.Size(544, 320);
			this.groupReportingServicesAdvanced.TabIndex = 0;
			this.groupReportingServicesAdvanced.TabStop = false;
			this.groupReportingServicesAdvanced.Text = "Set Advanced Reporting Services Connection Options";
			// 
			// labelReportingServicesAdvancedNote
			// 
			this.labelReportingServicesAdvancedNote.Location = new System.Drawing.Point(16, 264);
			this.labelReportingServicesAdvancedNote.Name = "labelReportingServicesAdvancedNote";
			this.labelReportingServicesAdvancedNote.Size = new System.Drawing.Size(520, 40);
			this.labelReportingServicesAdvancedNote.TabIndex = 9;
			this.labelReportingServicesAdvancedNote.Text = "Note: To successfully deploy reports using this utility, your logon account must " +
				"have Content Manager rights on the Report Server. For more information, see the " +
				"Reporting Services Books Online. ";
			// 
			// buttonReportingServicesAdvancedDefault
			// 
			this.buttonReportingServicesAdvancedDefault.Location = new System.Drawing.Point(288, 168);
			this.buttonReportingServicesAdvancedDefault.Name = "buttonReportingServicesAdvancedDefault";
			this.buttonReportingServicesAdvancedDefault.Size = new System.Drawing.Size(112, 23);
			this.buttonReportingServicesAdvancedDefault.TabIndex = 12;
			this.buttonReportingServicesAdvancedDefault.Text = "&Restore Defaults";
			this.buttonReportingServicesAdvancedDefault.Click += new System.EventHandler(this.buttonReportingServicesAdvancedDefault_Click);
			// 
			// checkBoxReportingServicesAdvancedSsl
			// 
			this.checkBoxReportingServicesAdvancedSsl.Location = new System.Drawing.Point(408, 72);
			this.checkBoxReportingServicesAdvancedSsl.Name = "checkBoxReportingServicesAdvancedSsl";
			this.checkBoxReportingServicesAdvancedSsl.Size = new System.Drawing.Size(80, 24);
			this.checkBoxReportingServicesAdvancedSsl.TabIndex = 7;
			this.checkBoxReportingServicesAdvancedSsl.Text = "Use &SSL";
			// 
			// textBoxReportingServicesAdvancedManager
			// 
			this.textBoxReportingServicesAdvancedManager.Location = new System.Drawing.Point(216, 136);
			this.textBoxReportingServicesAdvancedManager.Name = "textBoxReportingServicesAdvancedManager";
			this.textBoxReportingServicesAdvancedManager.Size = new System.Drawing.Size(256, 20);
			this.textBoxReportingServicesAdvancedManager.TabIndex = 11;
			this.textBoxReportingServicesAdvancedManager.Text = "";
			// 
			// textBoxReportingServicesAdvancedDirectory
			// 
			this.textBoxReportingServicesAdvancedDirectory.Location = new System.Drawing.Point(216, 104);
			this.textBoxReportingServicesAdvancedDirectory.Name = "textBoxReportingServicesAdvancedDirectory";
			this.textBoxReportingServicesAdvancedDirectory.Size = new System.Drawing.Size(256, 20);
			this.textBoxReportingServicesAdvancedDirectory.TabIndex = 9;
			this.textBoxReportingServicesAdvancedDirectory.Text = "";
			// 
			// textBoxReportingServicesAdvancedPort
			// 
			this.textBoxReportingServicesAdvancedPort.Location = new System.Drawing.Point(216, 72);
			this.textBoxReportingServicesAdvancedPort.Name = "textBoxReportingServicesAdvancedPort";
			this.textBoxReportingServicesAdvancedPort.Size = new System.Drawing.Size(48, 20);
			this.textBoxReportingServicesAdvancedPort.TabIndex = 6;
			this.textBoxReportingServicesAdvancedPort.Text = "";
			// 
			// labelReportingServicesAdvancedManager
			// 
			this.labelReportingServicesAdvancedManager.Location = new System.Drawing.Point(40, 136);
			this.labelReportingServicesAdvancedManager.Name = "labelReportingServicesAdvancedManager";
			this.labelReportingServicesAdvancedManager.Size = new System.Drawing.Size(176, 23);
			this.labelReportingServicesAdvancedManager.TabIndex = 10;
			this.labelReportingServicesAdvancedManager.Text = "Report &Manager Virtual Directory:";
			this.labelReportingServicesAdvancedManager.Enter += new System.EventHandler(this.labelReportingServicesAdvancedManager_Enter);
			// 
			// labelReportingServicesAdvancedDirectory
			// 
			this.labelReportingServicesAdvancedDirectory.Location = new System.Drawing.Point(40, 104);
			this.labelReportingServicesAdvancedDirectory.Name = "labelReportingServicesAdvancedDirectory";
			this.labelReportingServicesAdvancedDirectory.Size = new System.Drawing.Size(168, 23);
			this.labelReportingServicesAdvancedDirectory.TabIndex = 8;
			this.labelReportingServicesAdvancedDirectory.Text = "Report Server Virtual &Directory:";
			this.labelReportingServicesAdvancedDirectory.Enter += new System.EventHandler(this.labelReportingServicesAdvancedDirectory_Enter);
			// 
			// labelReportingServicesAdvancedPort
			// 
			this.labelReportingServicesAdvancedPort.Location = new System.Drawing.Point(40, 72);
			this.labelReportingServicesAdvancedPort.Name = "labelReportingServicesAdvancedPort";
			this.labelReportingServicesAdvancedPort.Size = new System.Drawing.Size(160, 23);
			this.labelReportingServicesAdvancedPort.TabIndex = 5;
			this.labelReportingServicesAdvancedPort.Text = "Report Server &Port Number:";
			this.labelReportingServicesAdvancedPort.Enter += new System.EventHandler(this.labelReportingServicesAdvancedPort_Enter);
			// 
			// labelReportingServicesAdvancedIntro
			// 
			this.labelReportingServicesAdvancedIntro.Location = new System.Drawing.Point(16, 24);
			this.labelReportingServicesAdvancedIntro.Name = "labelReportingServicesAdvancedIntro";
			this.labelReportingServicesAdvancedIntro.Size = new System.Drawing.Size(520, 40);
			this.labelReportingServicesAdvancedIntro.TabIndex = 4;
			this.labelReportingServicesAdvancedIntro.Text = "Specify the port number, the Report Server virtual directory, Report Manager virt" +
				"ual directory, and whether or not your Reporting Services is set to use SSL.";
			// 
			// panelRepositoryInstall
			// 
			this.panelRepositoryInstall.BackColor = System.Drawing.SystemColors.Control;
			this.panelRepositoryInstall.Controls.Add(this.groupRepositoryInstall);
			this.panelRepositoryInstall.Location = new System.Drawing.Point(0, 0);
			this.panelRepositoryInstall.Name = "panelRepositoryInstall";
			this.panelRepositoryInstall.NextPanelEvent = null;
			this.panelRepositoryInstall.PreviousPanelEvent = null;
			this.panelRepositoryInstall.Size = new System.Drawing.Size(560, 336);
			this.panelRepositoryInstall.TabIndex = 7;
			// 
			// groupRepositoryInstall
			// 
			this.groupRepositoryInstall.Controls.Add(this.buttonRepositoryInstallBrowseFolder);
			this.groupRepositoryInstall.Controls.Add(this.textBoxRepositoryInstallFolder);
			this.groupRepositoryInstall.Controls.Add(this.labelRepositoryInstallFolder);
			this.groupRepositoryInstall.Controls.Add(this.labelRepositoryInstallFolderCreate);
			this.groupRepositoryInstall.Controls.Add(this.textBoxRepositoryInstallPassword);
			this.groupRepositoryInstall.Controls.Add(this.textBoxRepositoryInstallLogin);
			this.groupRepositoryInstall.Controls.Add(this.labelRepositoryInstallPassword);
			this.groupRepositoryInstall.Controls.Add(this.labelRepositoryInstallLogin);
			this.groupRepositoryInstall.Controls.Add(this.labelRepositoryInstallCredentials);
			this.groupRepositoryInstall.Controls.Add(this.buttonRepositoryInstallBrowse);
			this.groupRepositoryInstall.Controls.Add(this.textBoxRepositoryInstallServer);
			this.groupRepositoryInstall.Controls.Add(this.labelRepositoryInstallServer);
			this.groupRepositoryInstall.Controls.Add(this.labelRepositoryInstallIntro);
			this.groupRepositoryInstall.Location = new System.Drawing.Point(8, 8);
			this.groupRepositoryInstall.Name = "groupRepositoryInstall";
			this.groupRepositoryInstall.Size = new System.Drawing.Size(544, 320);
			this.groupRepositoryInstall.TabIndex = 0;
			this.groupRepositoryInstall.TabStop = false;
			this.groupRepositoryInstall.Text = "Set <productName> Repository Configuration";
			// 
			// buttonRepositoryInstallBrowseFolder
			// 
			this.buttonRepositoryInstallBrowseFolder.Location = new System.Drawing.Point(440, 280);
			this.buttonRepositoryInstallBrowseFolder.Name = "buttonRepositoryInstallBrowseFolder";
			this.buttonRepositoryInstallBrowseFolder.TabIndex = 16;
			this.buttonRepositoryInstallBrowseFolder.Text = "B&rowse...";
			this.buttonRepositoryInstallBrowseFolder.Click += new System.EventHandler(this.buttonRepositoryInstallBrowseFolder_Click);
			// 
			// textBoxRepositoryInstallFolder
			// 
			this.textBoxRepositoryInstallFolder.Location = new System.Drawing.Point(200, 280);
			this.textBoxRepositoryInstallFolder.Name = "textBoxRepositoryInstallFolder";
			this.textBoxRepositoryInstallFolder.Size = new System.Drawing.Size(224, 20);
			this.textBoxRepositoryInstallFolder.TabIndex = 15;
			this.textBoxRepositoryInstallFolder.Text = "";
			// 
			// labelRepositoryInstallFolder
			// 
			this.labelRepositoryInstallFolder.Location = new System.Drawing.Point(48, 280);
			this.labelRepositoryInstallFolder.Name = "labelRepositoryInstallFolder";
			this.labelRepositoryInstallFolder.Size = new System.Drawing.Size(152, 23);
			this.labelRepositoryInstallFolder.TabIndex = 14;
			this.labelRepositoryInstallFolder.Text = "Report Server F&older Name:";
			this.labelRepositoryInstallFolder.Enter += new System.EventHandler(this.labelRepositoryInstallFolder_Enter);
			// 
			// labelRepositoryInstallFolderCreate
			// 
			this.labelRepositoryInstallFolderCreate.Location = new System.Drawing.Point(16, 248);
			this.labelRepositoryInstallFolderCreate.Name = "labelRepositoryInstallFolderCreate";
			this.labelRepositoryInstallFolderCreate.Size = new System.Drawing.Size(520, 23);
			this.labelRepositoryInstallFolderCreate.TabIndex = 13;
			this.labelRepositoryInstallFolderCreate.Text = "Name the folder that will host the <productName> reports.";
			// 
			// textBoxRepositoryInstallPassword
			// 
			this.textBoxRepositoryInstallPassword.Location = new System.Drawing.Point(200, 200);
			this.textBoxRepositoryInstallPassword.Name = "textBoxRepositoryInstallPassword";
			this.textBoxRepositoryInstallPassword.PasswordChar = '*';
			this.textBoxRepositoryInstallPassword.Size = new System.Drawing.Size(224, 20);
			this.textBoxRepositoryInstallPassword.TabIndex = 12;
			this.textBoxRepositoryInstallPassword.Text = "";
			// 
			// textBoxRepositoryInstallLogin
			// 
			this.textBoxRepositoryInstallLogin.Location = new System.Drawing.Point(200, 168);
			this.textBoxRepositoryInstallLogin.Name = "textBoxRepositoryInstallLogin";
			this.textBoxRepositoryInstallLogin.Size = new System.Drawing.Size(224, 20);
			this.textBoxRepositoryInstallLogin.TabIndex = 10;
			this.textBoxRepositoryInstallLogin.Text = "";
			// 
			// labelRepositoryInstallPassword
			// 
			this.labelRepositoryInstallPassword.Location = new System.Drawing.Point(48, 200);
			this.labelRepositoryInstallPassword.Name = "labelRepositoryInstallPassword";
			this.labelRepositoryInstallPassword.Size = new System.Drawing.Size(64, 23);
			this.labelRepositoryInstallPassword.TabIndex = 11;
			this.labelRepositoryInstallPassword.Text = "Pass&word:";
			this.labelRepositoryInstallPassword.Enter += new System.EventHandler(this.labelRepositoryInstallPassword_Enter);
			// 
			// labelRepositoryInstallLogin
			// 
			this.labelRepositoryInstallLogin.Location = new System.Drawing.Point(48, 168);
			this.labelRepositoryInstallLogin.Name = "labelRepositoryInstallLogin";
			this.labelRepositoryInstallLogin.Size = new System.Drawing.Size(128, 23);
			this.labelRepositoryInstallLogin.TabIndex = 9;
			this.labelRepositoryInstallLogin.Text = "&Login ID (domain\\user):";
			this.labelRepositoryInstallLogin.Enter += new System.EventHandler(this.labelRepositoryInstallLogin_Enter);
			// 
			// labelRepositoryInstallCredentials
			// 
			this.labelRepositoryInstallCredentials.Location = new System.Drawing.Point(16, 112);
			this.labelRepositoryInstallCredentials.Name = "labelRepositoryInstallCredentials";
			this.labelRepositoryInstallCredentials.Size = new System.Drawing.Size(504, 40);
			this.labelRepositoryInstallCredentials.TabIndex = 8;
			this.labelRepositoryInstallCredentials.Text = "Specify the Windows credentials the Report Server will use to connect to the Repo" +
				"sitory.  The specified account should have permission to execute stored procedur" +
				"es on the Repository database.";
			// 
			// buttonRepositoryInstallBrowse
			// 
			this.buttonRepositoryInstallBrowse.Location = new System.Drawing.Point(344, 64);
			this.buttonRepositoryInstallBrowse.Name = "buttonRepositoryInstallBrowse";
			this.buttonRepositoryInstallBrowse.Size = new System.Drawing.Size(80, 23);
			this.buttonRepositoryInstallBrowse.TabIndex = 7;
			this.buttonRepositoryInstallBrowse.Text = "&Browse...";
			this.buttonRepositoryInstallBrowse.Click += new System.EventHandler(this.buttonRepositoryInstallBrowse_Click);
			// 
			// textBoxRepositoryInstallServer
			// 
			this.textBoxRepositoryInstallServer.Location = new System.Drawing.Point(120, 64);
			this.textBoxRepositoryInstallServer.Name = "textBoxRepositoryInstallServer";
			this.textBoxRepositoryInstallServer.Size = new System.Drawing.Size(208, 20);
			this.textBoxRepositoryInstallServer.TabIndex = 6;
			this.textBoxRepositoryInstallServer.Text = "";
			// 
			// labelRepositoryInstallServer
			// 
			this.labelRepositoryInstallServer.Location = new System.Drawing.Point(48, 64);
			this.labelRepositoryInstallServer.Name = "labelRepositoryInstallServer";
			this.labelRepositoryInstallServer.Size = new System.Drawing.Size(72, 23);
			this.labelRepositoryInstallServer.TabIndex = 5;
			this.labelRepositoryInstallServer.Text = "SQL &Server:";
			this.labelRepositoryInstallServer.Enter += new System.EventHandler(this.labelRepositoryInstallServer_Enter);
			// 
			// labelRepositoryInstallIntro
			// 
			this.labelRepositoryInstallIntro.Location = new System.Drawing.Point(16, 24);
			this.labelRepositoryInstallIntro.Name = "labelRepositoryInstallIntro";
			this.labelRepositoryInstallIntro.Size = new System.Drawing.Size(520, 23);
			this.labelRepositoryInstallIntro.TabIndex = 4;
			this.labelRepositoryInstallIntro.Text = "Specify which SQL Server instance hosts the <productName> Repository:";
			// 
			// panelRepositoryChange
			// 
			this.panelRepositoryChange.BackColor = System.Drawing.SystemColors.Control;
			this.panelRepositoryChange.Controls.Add(this.groupBoxRepositoryChange);
			this.panelRepositoryChange.Location = new System.Drawing.Point(0, 0);
			this.panelRepositoryChange.Name = "panelRepositoryChange";
			this.panelRepositoryChange.NextPanelEvent = null;
			this.panelRepositoryChange.PreviousPanelEvent = null;
			this.panelRepositoryChange.Size = new System.Drawing.Size(560, 336);
			this.panelRepositoryChange.TabIndex = 8;
			// 
			// groupBoxRepositoryChange
			// 
			this.groupBoxRepositoryChange.Controls.Add(this.buttonRepositoryChangeBrowseFolder);
			this.groupBoxRepositoryChange.Controls.Add(this.textBoxRepositoryChangeFolder);
			this.groupBoxRepositoryChange.Controls.Add(this.labelRepositoryChangeFolder);
			this.groupBoxRepositoryChange.Controls.Add(this.labelRepositoryChangeFolderCreate);
			this.groupBoxRepositoryChange.Controls.Add(this.textBoxRepositoryChangePassword);
			this.groupBoxRepositoryChange.Controls.Add(this.textBoxRepositoryChangeLogin);
			this.groupBoxRepositoryChange.Controls.Add(this.labelRepositoryChangePassword);
			this.groupBoxRepositoryChange.Controls.Add(this.labelRepositoryChangeLogin);
			this.groupBoxRepositoryChange.Controls.Add(this.labelRepositoryChangeCredentials);
			this.groupBoxRepositoryChange.Controls.Add(this.buttonRepositoryChangeBrowse);
			this.groupBoxRepositoryChange.Controls.Add(this.textBoxRepositoryChangeServer);
			this.groupBoxRepositoryChange.Controls.Add(this.labelRepositoryChangeServer);
			this.groupBoxRepositoryChange.Controls.Add(this.labelRepositoryChangeIntro);
			this.groupBoxRepositoryChange.Location = new System.Drawing.Point(8, 8);
			this.groupBoxRepositoryChange.Name = "groupBoxRepositoryChange";
			this.groupBoxRepositoryChange.Size = new System.Drawing.Size(544, 320);
			this.groupBoxRepositoryChange.TabIndex = 0;
			this.groupBoxRepositoryChange.TabStop = false;
			this.groupBoxRepositoryChange.Text = "Change <productName> Repository Configuration";
			// 
			// buttonRepositoryChangeBrowseFolder
			// 
			this.buttonRepositoryChangeBrowseFolder.Location = new System.Drawing.Point(432, 280);
			this.buttonRepositoryChangeBrowseFolder.Name = "buttonRepositoryChangeBrowseFolder";
			this.buttonRepositoryChangeBrowseFolder.Size = new System.Drawing.Size(80, 23);
			this.buttonRepositoryChangeBrowseFolder.TabIndex = 16;
			this.buttonRepositoryChangeBrowseFolder.Text = "B&rowse...";
			this.buttonRepositoryChangeBrowseFolder.Click += new System.EventHandler(this.buttonRepositoryChangeBrowseFolder_Click);
			// 
			// textBoxRepositoryChangeFolder
			// 
			this.textBoxRepositoryChangeFolder.Location = new System.Drawing.Point(168, 280);
			this.textBoxRepositoryChangeFolder.Name = "textBoxRepositoryChangeFolder";
			this.textBoxRepositoryChangeFolder.Size = new System.Drawing.Size(248, 20);
			this.textBoxRepositoryChangeFolder.TabIndex = 15;
			this.textBoxRepositoryChangeFolder.Text = "";
			// 
			// labelRepositoryChangeFolder
			// 
			this.labelRepositoryChangeFolder.Location = new System.Drawing.Point(48, 280);
			this.labelRepositoryChangeFolder.Name = "labelRepositoryChangeFolder";
			this.labelRepositoryChangeFolder.Size = new System.Drawing.Size(120, 23);
			this.labelRepositoryChangeFolder.TabIndex = 14;
			this.labelRepositoryChangeFolder.Text = "Report Server F&older:";
			this.labelRepositoryChangeFolder.Enter += new System.EventHandler(this.labelRepositoryChangeFolder_Enter);
			// 
			// labelRepositoryChangeFolderCreate
			// 
			this.labelRepositoryChangeFolderCreate.Location = new System.Drawing.Point(12, 245);
			this.labelRepositoryChangeFolderCreate.Name = "labelRepositoryChangeFolderCreate";
			this.labelRepositoryChangeFolderCreate.Size = new System.Drawing.Size(520, 23);
			this.labelRepositoryChangeFolderCreate.TabIndex = 13;
			this.labelRepositoryChangeFolderCreate.Text = "Specify or browse for the folder that hosts the <productName> reports.";
			// 
			// textBoxRepositoryChangePassword
			// 
			this.textBoxRepositoryChangePassword.Location = new System.Drawing.Point(168, 197);
			this.textBoxRepositoryChangePassword.Name = "textBoxRepositoryChangePassword";
			this.textBoxRepositoryChangePassword.PasswordChar = '*';
			this.textBoxRepositoryChangePassword.Size = new System.Drawing.Size(248, 20);
			this.textBoxRepositoryChangePassword.TabIndex = 12;
			this.textBoxRepositoryChangePassword.Text = "";
			// 
			// textBoxRepositoryChangeLogin
			// 
			this.textBoxRepositoryChangeLogin.Location = new System.Drawing.Point(168, 165);
			this.textBoxRepositoryChangeLogin.Name = "textBoxRepositoryChangeLogin";
			this.textBoxRepositoryChangeLogin.Size = new System.Drawing.Size(248, 20);
			this.textBoxRepositoryChangeLogin.TabIndex = 10;
			this.textBoxRepositoryChangeLogin.Text = "";
			// 
			// labelRepositoryChangePassword
			// 
			this.labelRepositoryChangePassword.Location = new System.Drawing.Point(44, 197);
			this.labelRepositoryChangePassword.Name = "labelRepositoryChangePassword";
			this.labelRepositoryChangePassword.Size = new System.Drawing.Size(64, 23);
			this.labelRepositoryChangePassword.TabIndex = 11;
			this.labelRepositoryChangePassword.Text = "Pass&word:";
			this.labelRepositoryChangePassword.Enter += new System.EventHandler(this.labelRepositoryChangePassword_Enter);
			// 
			// labelRepositoryChangeLogin
			// 
			this.labelRepositoryChangeLogin.Location = new System.Drawing.Point(44, 165);
			this.labelRepositoryChangeLogin.Name = "labelRepositoryChangeLogin";
			this.labelRepositoryChangeLogin.Size = new System.Drawing.Size(128, 23);
			this.labelRepositoryChangeLogin.TabIndex = 9;
			this.labelRepositoryChangeLogin.Text = "&Login ID (domain\\user):";
			this.labelRepositoryChangeLogin.Enter += new System.EventHandler(this.labelRepositoryChangeLogin_Enter);
			// 
			// labelRepositoryChangeCredentials
			// 
			this.labelRepositoryChangeCredentials.Location = new System.Drawing.Point(12, 104);
			this.labelRepositoryChangeCredentials.Name = "labelRepositoryChangeCredentials";
			this.labelRepositoryChangeCredentials.Size = new System.Drawing.Size(504, 43);
			this.labelRepositoryChangeCredentials.TabIndex = 8;
			this.labelRepositoryChangeCredentials.Text = "Specify the Windows credentials the Report Server will use to connect to the Repo" +
				"sitory.  The specified account should have permission to execute stored procedur" +
				"es on the Repository database.";
			// 
			// buttonRepositoryChangeBrowse
			// 
			this.buttonRepositoryChangeBrowse.Location = new System.Drawing.Point(340, 61);
			this.buttonRepositoryChangeBrowse.Name = "buttonRepositoryChangeBrowse";
			this.buttonRepositoryChangeBrowse.Size = new System.Drawing.Size(80, 23);
			this.buttonRepositoryChangeBrowse.TabIndex = 7;
			this.buttonRepositoryChangeBrowse.Text = "&Browse...";
			this.buttonRepositoryChangeBrowse.Click += new System.EventHandler(this.buttonRepositoryChangeBrowse_Click);
			// 
			// textBoxRepositoryChangeServer
			// 
			this.textBoxRepositoryChangeServer.Location = new System.Drawing.Point(116, 61);
			this.textBoxRepositoryChangeServer.Name = "textBoxRepositoryChangeServer";
			this.textBoxRepositoryChangeServer.Size = new System.Drawing.Size(208, 20);
			this.textBoxRepositoryChangeServer.TabIndex = 6;
			this.textBoxRepositoryChangeServer.Text = "";
			// 
			// labelRepositoryChangeServer
			// 
			this.labelRepositoryChangeServer.Location = new System.Drawing.Point(44, 61);
			this.labelRepositoryChangeServer.Name = "labelRepositoryChangeServer";
			this.labelRepositoryChangeServer.Size = new System.Drawing.Size(72, 23);
			this.labelRepositoryChangeServer.TabIndex = 5;
			this.labelRepositoryChangeServer.Text = "SQL &Server:";
			this.labelRepositoryChangeServer.Enter += new System.EventHandler(this.labelRepositoryChangeServer_Enter);
			// 
			// labelRepositoryChangeIntro
			// 
			this.labelRepositoryChangeIntro.Location = new System.Drawing.Point(12, 21);
			this.labelRepositoryChangeIntro.Name = "labelRepositoryChangeIntro";
			this.labelRepositoryChangeIntro.Size = new System.Drawing.Size(520, 23);
			this.labelRepositoryChangeIntro.TabIndex = 4;
			this.labelRepositoryChangeIntro.Text = "Specify which SQL Server instance hosts the <productName> Repository:";
			// 
			// panelReports
			// 
			this.panelReports.BackColor = System.Drawing.SystemColors.Control;
			this.panelReports.Controls.Add(this.groupBoxReports);
			this.panelReports.Location = new System.Drawing.Point(0, 0);
			this.panelReports.Name = "panelReports";
			this.panelReports.NextPanelEvent = null;
			this.panelReports.PreviousPanelEvent = null;
			this.panelReports.Size = new System.Drawing.Size(560, 336);
			this.panelReports.TabIndex = 10;
			// 
			// groupBoxReports
			// 
			this.groupBoxReports.Controls.Add(this.radioCustom);
			this.groupBoxReports.Controls.Add(this.radioAdd);
			this.groupBoxReports.Controls.Add(this.radioUpdate);
			this.groupBoxReports.Controls.Add(this.labelReportsIntro);
			this.groupBoxReports.Location = new System.Drawing.Point(8, 8);
			this.groupBoxReports.Name = "groupBoxReports";
			this.groupBoxReports.Size = new System.Drawing.Size(544, 320);
			this.groupBoxReports.TabIndex = 0;
			this.groupBoxReports.TabStop = false;
			this.groupBoxReports.Text = "Select Upgrade Type";
			// 
			// radioCustom
			// 
			this.radioCustom.Location = new System.Drawing.Point(48, 184);
			this.radioCustom.Name = "radioCustom";
			this.radioCustom.Size = new System.Drawing.Size(352, 24);
			this.radioCustom.TabIndex = 6;
			this.radioCustom.TabStop = true;
			this.radioCustom.Text = "Cu&stom - Choose which reports you want to add or update.";
			this.radioCustom.CheckedChanged += new System.EventHandler(this.radioCustom_CheckedChanged);
			// 
			// radioAdd
			// 
			this.radioAdd.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.radioAdd.Location = new System.Drawing.Point(48, 128);
			this.radioAdd.Name = "radioAdd";
			this.radioAdd.Size = new System.Drawing.Size(472, 32);
			this.radioAdd.TabIndex = 5;
			this.radioAdd.TabStop = true;
			this.radioAdd.Text = "&Add New Reports - Adds any available reports you are missing.  Will not overwrit" +
				"e existing reports.";
			this.radioAdd.CheckedChanged += new System.EventHandler(this.radioAdd_CheckedChanged);
			// 
			// radioUpdate
			// 
			this.radioUpdate.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.radioUpdate.Checked = true;
			this.radioUpdate.Location = new System.Drawing.Point(48, 72);
			this.radioUpdate.Name = "radioUpdate";
			this.radioUpdate.Size = new System.Drawing.Size(472, 32);
			this.radioUpdate.TabIndex = 4;
			this.radioUpdate.TabStop = true;
			this.radioUpdate.Text = "&Update All - Updates all installed reports and adds any available reports you ar" +
				"e missing.";
			this.radioUpdate.CheckedChanged += new System.EventHandler(this.radioUpdate_CheckedChanged);
			// 
			// labelReportsIntro
			// 
			this.labelReportsIntro.Location = new System.Drawing.Point(16, 24);
			this.labelReportsIntro.Name = "labelReportsIntro";
			this.labelReportsIntro.Size = new System.Drawing.Size(512, 23);
			this.labelReportsIntro.TabIndex = 0;
			this.labelReportsIntro.Text = "Which reports would you like to update or add?";
			// 
			// panelReportsCustom
			// 
			this.panelReportsCustom.BackColor = System.Drawing.SystemColors.Control;
			this.panelReportsCustom.Controls.Add(this.groupReportsCustom);
			this.panelReportsCustom.Location = new System.Drawing.Point(0, 0);
			this.panelReportsCustom.Name = "panelReportsCustom";
			this.panelReportsCustom.NextPanelEvent = null;
			this.panelReportsCustom.PreviousPanelEvent = null;
			this.panelReportsCustom.Size = new System.Drawing.Size(560, 336);
			this.panelReportsCustom.TabIndex = 11;
			// 
			// groupReportsCustom
			// 
			this.groupReportsCustom.Controls.Add(this.buttonReportsCustomUpdateUnselect);
			this.groupReportsCustom.Controls.Add(this.buttonReportsCustomUpdateSelect);
			this.groupReportsCustom.Controls.Add(this.buttonReportsCustomAddUnselect);
			this.groupReportsCustom.Controls.Add(this.buttonReportsCustomAddSelect);
			this.groupReportsCustom.Controls.Add(this.checkedListBoxReportsCustomUpdate);
			this.groupReportsCustom.Controls.Add(this.checkedListBoxReportsCustomAdd);
			this.groupReportsCustom.Controls.Add(this.labelReportsCustomUpdate);
			this.groupReportsCustom.Controls.Add(this.labelReportsCustomSelect);
			this.groupReportsCustom.Location = new System.Drawing.Point(8, 8);
			this.groupReportsCustom.Name = "groupReportsCustom";
			this.groupReportsCustom.Size = new System.Drawing.Size(544, 320);
			this.groupReportsCustom.TabIndex = 0;
			this.groupReportsCustom.TabStop = false;
			this.groupReportsCustom.Text = "Custom Upgrade";
			// 
			// buttonReportsCustomUpdateUnselect
			// 
			this.buttonReportsCustomUpdateUnselect.Location = new System.Drawing.Point(400, 288);
			this.buttonReportsCustomUpdateUnselect.Name = "buttonReportsCustomUpdateUnselect";
			this.buttonReportsCustomUpdateUnselect.TabIndex = 9;
			this.buttonReportsCustomUpdateUnselect.Text = "C&lear All";
			this.buttonReportsCustomUpdateUnselect.Click += new System.EventHandler(this.buttonReportsCustomUpdateUnselect_Click);
			// 
			// buttonReportsCustomUpdateSelect
			// 
			this.buttonReportsCustomUpdateSelect.Location = new System.Drawing.Point(312, 288);
			this.buttonReportsCustomUpdateSelect.Name = "buttonReportsCustomUpdateSelect";
			this.buttonReportsCustomUpdateSelect.TabIndex = 8;
			this.buttonReportsCustomUpdateSelect.Text = "S&elect All";
			this.buttonReportsCustomUpdateSelect.Click += new System.EventHandler(this.buttonReportsCustomUpdateSelect_Click);
			// 
			// buttonReportsCustomAddUnselect
			// 
			this.buttonReportsCustomAddUnselect.Location = new System.Drawing.Point(136, 288);
			this.buttonReportsCustomAddUnselect.Name = "buttonReportsCustomAddUnselect";
			this.buttonReportsCustomAddUnselect.TabIndex = 6;
			this.buttonReportsCustomAddUnselect.Text = "Clea&r All";
			this.buttonReportsCustomAddUnselect.Click += new System.EventHandler(this.buttonReportsCustomAddUnselect_Click);
			// 
			// buttonReportsCustomAddSelect
			// 
			this.buttonReportsCustomAddSelect.Location = new System.Drawing.Point(48, 288);
			this.buttonReportsCustomAddSelect.Name = "buttonReportsCustomAddSelect";
			this.buttonReportsCustomAddSelect.TabIndex = 5;
			this.buttonReportsCustomAddSelect.Text = "&Select All";
			this.buttonReportsCustomAddSelect.Click += new System.EventHandler(this.buttonReportsCustomAddSelect_Click);
			// 
			// checkedListBoxReportsCustomUpdate
			// 
			this.checkedListBoxReportsCustomUpdate.HorizontalScrollbar = true;
			this.checkedListBoxReportsCustomUpdate.Location = new System.Drawing.Point(280, 48);
			this.checkedListBoxReportsCustomUpdate.Name = "checkedListBoxReportsCustomUpdate";
			this.checkedListBoxReportsCustomUpdate.Size = new System.Drawing.Size(224, 229);
			this.checkedListBoxReportsCustomUpdate.TabIndex = 7;
			// 
			// checkedListBoxReportsCustomAdd
			// 
			this.checkedListBoxReportsCustomAdd.HorizontalScrollbar = true;
			this.checkedListBoxReportsCustomAdd.Location = new System.Drawing.Point(16, 48);
			this.checkedListBoxReportsCustomAdd.Name = "checkedListBoxReportsCustomAdd";
			this.checkedListBoxReportsCustomAdd.Size = new System.Drawing.Size(224, 229);
			this.checkedListBoxReportsCustomAdd.TabIndex = 4;
			// 
			// labelReportsCustomUpdate
			// 
			this.labelReportsCustomUpdate.Location = new System.Drawing.Point(280, 24);
			this.labelReportsCustomUpdate.Name = "labelReportsCustomUpdate";
			this.labelReportsCustomUpdate.Size = new System.Drawing.Size(256, 16);
			this.labelReportsCustomUpdate.TabIndex = 1;
			this.labelReportsCustomUpdate.Text = "Select which existing reports you want to update:";
			// 
			// labelReportsCustomSelect
			// 
			this.labelReportsCustomSelect.Location = new System.Drawing.Point(16, 24);
			this.labelReportsCustomSelect.Name = "labelReportsCustomSelect";
			this.labelReportsCustomSelect.Size = new System.Drawing.Size(232, 16);
			this.labelReportsCustomSelect.TabIndex = 0;
			this.labelReportsCustomSelect.Text = "Select which reports you like to install:";
			// 
			// panelSummary
			// 
			this.panelSummary.BackColor = System.Drawing.SystemColors.Control;
			this.panelSummary.Controls.Add(this.groupSummary);
			this.panelSummary.Location = new System.Drawing.Point(0, 0);
			this.panelSummary.Name = "panelSummary";
			this.panelSummary.NextPanelEvent = null;
			this.panelSummary.PreviousPanelEvent = null;
			this.panelSummary.Size = new System.Drawing.Size(560, 336);
			this.panelSummary.TabIndex = 12;
			// 
			// groupSummary
			// 
			this.groupSummary.Controls.Add(this.labelSummaryList);
			this.groupSummary.Controls.Add(this.labelSummaryFolderDs);
			this.groupSummary.Controls.Add(this.labelSummaryUpdate);
			this.groupSummary.Controls.Add(this.labelSummaryAdded);
			this.groupSummary.Controls.Add(this.labelSummaryFinish);
			this.groupSummary.Controls.Add(this.labelSummaryIntro);
			this.groupSummary.Location = new System.Drawing.Point(8, 8);
			this.groupSummary.Name = "groupSummary";
			this.groupSummary.Size = new System.Drawing.Size(544, 320);
			this.groupSummary.TabIndex = 0;
			this.groupSummary.TabStop = false;
			this.groupSummary.Text = "Summary";
			// 
			// labelSummaryList
			// 
			this.labelSummaryList.Location = new System.Drawing.Point(40, 56);
			this.labelSummaryList.Name = "labelSummaryList";
			this.labelSummaryList.Size = new System.Drawing.Size(216, 23);
			this.labelSummaryList.TabIndex = 7;
			this.labelSummaryList.Text = "The following actions will be performed:";
			// 
			// labelSummaryFolderDs
			// 
			this.labelSummaryFolderDs.Location = new System.Drawing.Point(72, 152);
			this.labelSummaryFolderDs.Name = "labelSummaryFolderDs";
			this.labelSummaryFolderDs.Size = new System.Drawing.Size(432, 23);
			this.labelSummaryFolderDs.TabIndex = 6;
			this.labelSummaryFolderDs.Text = "Create a new folder and a new data source.";
			// 
			// labelSummaryUpdate
			// 
			this.labelSummaryUpdate.Location = new System.Drawing.Point(72, 120);
			this.labelSummaryUpdate.Name = "labelSummaryUpdate";
			this.labelSummaryUpdate.Size = new System.Drawing.Size(432, 23);
			this.labelSummaryUpdate.TabIndex = 3;
			this.labelSummaryUpdate.Text = "Update 0 reports.";
			// 
			// labelSummaryAdded
			// 
			this.labelSummaryAdded.Location = new System.Drawing.Point(72, 88);
			this.labelSummaryAdded.Name = "labelSummaryAdded";
			this.labelSummaryAdded.Size = new System.Drawing.Size(432, 23);
			this.labelSummaryAdded.TabIndex = 2;
			this.labelSummaryAdded.Text = "Add 0 reports.";
			// 
			// labelSummaryFinish
			// 
			this.labelSummaryFinish.Location = new System.Drawing.Point(16, 240);
			this.labelSummaryFinish.Name = "labelSummaryFinish";
			this.labelSummaryFinish.Size = new System.Drawing.Size(400, 23);
			this.labelSummaryFinish.TabIndex = 1;
			this.labelSummaryFinish.Text = "Click Install to continue or Previous to change your configuration.";
			// 
			// labelSummaryIntro
			// 
			this.labelSummaryIntro.Location = new System.Drawing.Point(16, 24);
			this.labelSummaryIntro.Name = "labelSummaryIntro";
			this.labelSummaryIntro.Size = new System.Drawing.Size(512, 23);
			this.labelSummaryIntro.TabIndex = 0;
			this.labelSummaryIntro.Text = "You have finished entering the data necessary to complete the reports installatio" +
				"n.";
			// 
			// panelProgress
			// 
			this.panelProgress.BackColor = System.Drawing.SystemColors.Control;
			this.panelProgress.Controls.Add(this.groupProgress);
			this.panelProgress.Location = new System.Drawing.Point(0, 0);
			this.panelProgress.Name = "panelProgress";
			this.panelProgress.NextPanelEvent = null;
			this.panelProgress.PreviousPanelEvent = null;
			this.panelProgress.Size = new System.Drawing.Size(560, 336);
			this.panelProgress.TabIndex = 13;
			// 
			// groupProgress
			// 
			this.groupProgress.Controls.Add(this.richTextBoxProgress);
			this.groupProgress.Location = new System.Drawing.Point(8, 8);
			this.groupProgress.Name = "groupProgress";
			this.groupProgress.Size = new System.Drawing.Size(544, 320);
			this.groupProgress.TabIndex = 0;
			this.groupProgress.TabStop = false;
			this.groupProgress.Text = "Installation Progress";
			// 
			// richTextBoxProgress
			// 
			this.richTextBoxProgress.Cursor = System.Windows.Forms.Cursors.WaitCursor;
			this.richTextBoxProgress.Location = new System.Drawing.Point(8, 24);
			this.richTextBoxProgress.Name = "richTextBoxProgress";
			this.richTextBoxProgress.ReadOnly = true;
			this.richTextBoxProgress.Size = new System.Drawing.Size(528, 288);
			this.richTextBoxProgress.TabIndex = 0;
			this.richTextBoxProgress.Text = "";
			// 
			// panelComplete
			// 
			this.panelComplete.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.panelComplete.Controls.Add(this.panelCompleteBackground);
			this.panelComplete.Controls.Add(this.labelCompleteIntro);
			this.panelComplete.Controls.Add(this.panelCompleteIntroSplashBackground);
			this.panelComplete.Controls.Add(this.pictureBoxComplete);
			this.panelComplete.Location = new System.Drawing.Point(0, 0);
			this.panelComplete.Name = "panelComplete";
			this.panelComplete.NextPanelEvent = null;
			this.panelComplete.PreviousPanelEvent = null;
			this.panelComplete.Size = new System.Drawing.Size(560, 336);
			this.panelComplete.TabIndex = 14;
			// 
			// panelCompleteBackground
			// 
			this.panelCompleteBackground.BackColor = System.Drawing.SystemColors.Control;
			this.panelCompleteBackground.Controls.Add(this.groupBoxComplete);
			this.panelCompleteBackground.Location = new System.Drawing.Point(184, 48);
			this.panelCompleteBackground.Name = "panelCompleteBackground";
			this.panelCompleteBackground.Size = new System.Drawing.Size(376, 288);
			this.panelCompleteBackground.TabIndex = 3;
			// 
			// groupBoxComplete
			// 
			this.groupBoxComplete.Controls.Add(this.buttonCompleteError);
			this.groupBoxComplete.Controls.Add(this.listBoxComplete);
			this.groupBoxComplete.Controls.Add(this.linkLabelComplete);
			this.groupBoxComplete.Controls.Add(this.labelCompleteReports);
			this.groupBoxComplete.Controls.Add(this.labelCompleteInstallChanges);
			this.groupBoxComplete.Controls.Add(this.labelCompleteInstallIntro);
			this.groupBoxComplete.Location = new System.Drawing.Point(8, 8);
			this.groupBoxComplete.Name = "groupBoxComplete";
			this.groupBoxComplete.Size = new System.Drawing.Size(360, 272);
			this.groupBoxComplete.TabIndex = 0;
			this.groupBoxComplete.TabStop = false;
			// 
			// buttonCompleteError
			// 
			this.buttonCompleteError.Location = new System.Drawing.Point(16, 219);
			this.buttonCompleteError.Name = "buttonCompleteError";
			this.buttonCompleteError.Size = new System.Drawing.Size(75, 20);
			this.buttonCompleteError.TabIndex = 5;
			this.buttonCompleteError.Text = "&Error Details";
			this.buttonCompleteError.Click += new System.EventHandler(this.buttonCompleteError_Click);
			// 
			// listBoxComplete
			// 
			this.listBoxComplete.HorizontalScrollbar = true;
			this.listBoxComplete.Location = new System.Drawing.Point(8, 88);
			this.listBoxComplete.Name = "listBoxComplete";
			this.listBoxComplete.Size = new System.Drawing.Size(344, 121);
			this.listBoxComplete.TabIndex = 4;
			// 
			// linkLabelComplete
			// 
			this.linkLabelComplete.Location = new System.Drawing.Point(128, 248);
			this.linkLabelComplete.Name = "linkLabelComplete";
			this.linkLabelComplete.Size = new System.Drawing.Size(224, 16);
			this.linkLabelComplete.TabIndex = 3;
			this.linkLabelComplete.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelComplete_LinkClicked);
			// 
			// labelCompleteReports
			// 
			this.labelCompleteReports.Location = new System.Drawing.Point(16, 248);
			this.labelCompleteReports.Name = "labelCompleteReports";
			this.labelCompleteReports.Size = new System.Drawing.Size(120, 16);
			this.labelCompleteReports.TabIndex = 2;
			this.labelCompleteReports.Text = "Reports are hosted at:";
			// 
			// labelCompleteInstallChanges
			// 
			this.labelCompleteInstallChanges.Location = new System.Drawing.Point(8, 56);
			this.labelCompleteInstallChanges.Name = "labelCompleteInstallChanges";
			this.labelCompleteInstallChanges.Size = new System.Drawing.Size(344, 32);
			this.labelCompleteInstallChanges.TabIndex = 1;
			this.labelCompleteInstallChanges.Text = "You have installed the following reports:";
			// 
			// labelCompleteInstallIntro
			// 
			this.labelCompleteInstallIntro.Location = new System.Drawing.Point(8, 16);
			this.labelCompleteInstallIntro.Name = "labelCompleteInstallIntro";
			this.labelCompleteInstallIntro.Size = new System.Drawing.Size(344, 32);
			this.labelCompleteInstallIntro.TabIndex = 0;
			this.labelCompleteInstallIntro.Text = "You have successfully completed the <productAbbrev> Reports Installer.";
			// 
			// labelCompleteIntro
			// 
			this.labelCompleteIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 17F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelCompleteIntro.Location = new System.Drawing.Point(192, 8);
			this.labelCompleteIntro.Name = "labelCompleteIntro";
			this.labelCompleteIntro.Size = new System.Drawing.Size(352, 32);
			this.labelCompleteIntro.TabIndex = 2;
			this.labelCompleteIntro.Text = "Installation Complete";
			// 
			// panelCompleteIntroSplashBackground
			// 
			this.panelCompleteIntroSplashBackground.BackColor = System.Drawing.SystemColors.Control;
			this.panelCompleteIntroSplashBackground.Location = new System.Drawing.Point(0, 328);
			this.panelCompleteIntroSplashBackground.Name = "panelCompleteIntroSplashBackground";
			this.panelCompleteIntroSplashBackground.Size = new System.Drawing.Size(184, 40);
			this.panelCompleteIntroSplashBackground.TabIndex = 1;
			// 
			// pictureBoxComplete
			// 
			this.pictureBoxComplete.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pictureBoxComplete.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxComplete.Image")));
			this.pictureBoxComplete.Location = new System.Drawing.Point(0, 0);
			this.pictureBoxComplete.Name = "pictureBoxComplete";
			this.pictureBoxComplete.Size = new System.Drawing.Size(184, 336);
			this.pictureBoxComplete.TabIndex = 0;
			this.pictureBoxComplete.TabStop = false;
			// 
			// InstallerView
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(554, 363);
			this.Controls.Add(this.panelComplete);
			this.Controls.Add(this.panelProgress);
			this.Controls.Add(this.panelSummary);
			this.Controls.Add(this.panelReportsCustom);
			this.Controls.Add(this.panelReports);
			this.Controls.Add(this.panelRepositoryChange);
			this.Controls.Add(this.panelRepositoryInstall);
			this.Controls.Add(this.panelReportingServicesAdvanced);
			this.Controls.Add(this.panelReportingServices);
			this.Controls.Add(this.panelInstallationOptions);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "InstallerView";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "<Main method in RsInstallationController>";
			this.Load += new System.EventHandler(this.InstallerView_Load);
			this.Controls.SetChildIndex(this.panelInstallationOptions, 0);
			this.Controls.SetChildIndex(this.panelReportingServices, 0);
			this.Controls.SetChildIndex(this.panelReportingServicesAdvanced, 0);
			this.Controls.SetChildIndex(this.panelRepositoryInstall, 0);
			this.Controls.SetChildIndex(this.panelRepositoryChange, 0);
			this.Controls.SetChildIndex(this.panelReports, 0);
			this.Controls.SetChildIndex(this.panelReportsCustom, 0);
			this.Controls.SetChildIndex(this.panelSummary, 0);
			this.Controls.SetChildIndex(this.panelProgress, 0);
			this.Controls.SetChildIndex(this.panelComplete, 0);
			this.Controls.SetChildIndex(this.buttonPrev, 0);
			this.Controls.SetChildIndex(this.buttonNext, 0);
			this.Controls.SetChildIndex(this.buttonFinish, 0);
			this.Controls.SetChildIndex(this.buttonCancel, 0);
			this.panelInstallationOptions.ResumeLayout(false);
			this.panelInstallationOptionsBackground.ResumeLayout(false);
			this.groupInstallationOptions.ResumeLayout(false);
			this.panelReportingServices.ResumeLayout(false);
			this.groupReportingServices.ResumeLayout(false);
			this.panelReportingServicesAdvanced.ResumeLayout(false);
			this.groupReportingServicesAdvanced.ResumeLayout(false);
			this.panelRepositoryInstall.ResumeLayout(false);
			this.groupRepositoryInstall.ResumeLayout(false);
			this.panelRepositoryChange.ResumeLayout(false);
			this.groupBoxRepositoryChange.ResumeLayout(false);
			this.panelReports.ResumeLayout(false);
			this.groupBoxReports.ResumeLayout(false);
			this.panelReportsCustom.ResumeLayout(false);
			this.groupReportsCustom.ResumeLayout(false);
			this.panelSummary.ResumeLayout(false);
			this.groupSummary.ResumeLayout(false);
			this.panelProgress.ResumeLayout(false);
			this.groupProgress.ResumeLayout(false);
			this.panelComplete.ResumeLayout(false);
			this.panelCompleteBackground.ResumeLayout(false);
			this.groupBoxComplete.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private delegate void RunDelegate();
		/// <summary>
		/// Starts the GUI
		/// </summary>
		public void Run()
		{
			if (this.InvokeRequired)
			{
				this.Invoke(new RunDelegate(Run), null);
				return;
			}
			Application.Run(this);
		}

		private delegate void DisplayMessageBoxDelegate(string message, string titleBar);
		/// <summary>
		/// Displays a message box with the specified text and specified
		/// title bar text.
		/// </summary>
		/// <param name="message">The text that appears in the message box.</param>
		/// <param name="titleBar">The text that appears in the title bar
		/// of the message box.</param>
		public void DisplayMessageBox(string message, string titleBar)
		{
/*			if (this.InvokeRequired)
			{
				this.Invoke(new DisplayMessageBoxDelegate(DisplayMessageBox),
					new object[] {message, titleBar});
				return;
			}
*/			MessageBox.Show(this, message, _formHeader + ": " + titleBar);
		}

		private delegate bool DisplayMessageBoxYesNoDelegate(string message, string titleBar);
		/// <summary>
		/// Displays a message box with the specified text and specified
		/// title bar text.  Also includes YesNo buttons.
		/// </summary>
		/// <param name="message">The text that appears in the message box.</param>
		/// <param name="titleBar">The text that appears in the title bar
		/// of the message box.</param>
		/// <returns>True if Yes is chosen; False if No is chosen</returns>
		public bool DisplayMessageBoxYesNo(string message, string titleBar)
		{
/*			if (this.InvokeRequired)
			{
				return (bool)(this.Invoke(new DisplayMessageBoxYesNoDelegate(DisplayMessageBoxYesNo), 
					new object[] {message, titleBar})); 
			}*/
			DialogResult choice = MessageBox.Show(this, message, _formHeader + ": " + 
				titleBar, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (choice == DialogResult.Yes)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		private delegate void UpdateInstallerLogDelegate(string message);
		/// <summary>
		/// Adds text to the installer log (richTextProgress).
		/// </summary>
		/// <param name="message">The text that appears in the rich text box 
		/// acting as an installer log.</param>
		public void UpdateInstallerLog(string message)
		{
			if (this.InvokeRequired)
			{
				this.Invoke(new UpdateInstallerLogDelegate(UpdateInstallerLog),
					new object[] {message});
				return;
			}
			richTextBoxProgress.AppendText(message);
		}

		private delegate void AddToReportsListDelegate(string data);
		/// <summary>
		/// Adds a report to the checked list box (checkedListReports).
		/// </summary>
		/// <param name="data">The item added to the checked list box</param>
		public void AddToReportsList(string data)
		{
			if (this.InvokeRequired)
			{
				this.Invoke(new AddToReportsListDelegate(AddToReportsList),
					new object[] {data});
				return;
			}
			checkedListBoxReportsCustomAdd.Items.Add(data, true);
		}

		private delegate void AddToAlreadyInstalledListDelegate(string data);
		/// <summary>
		/// Adds a report to the already installed list box (listBoxReports).
		/// </summary>
		/// <param name="data">The item added to the list box</param>
		public void AddToAlreadyInstalledList(string data)
		{
			if (this.InvokeRequired)
			{
				this.Invoke(new AddToAlreadyInstalledListDelegate(AddToAlreadyInstalledList),
					new object[] {data});
				return;
			}
			checkedListBoxReportsCustomUpdate.Items.Add(data, true);
		}

		private delegate void AddToInstalledListDelegate(string data);
		/// <summary>
		/// Adds a report to the completed list box (listBoxComplete).
		/// </summary>
		/// <param name="data">The item added to the list box</param>
		public void AddToInstalledList(string data)
		{
			if (this.InvokeRequired)
			{
				this.Invoke(new AddToInstalledListDelegate(AddToInstalledList),
					new object[] {data});
				return;
			}
			listBoxComplete.Items.Add(data);
		}

		private delegate void AddItemToSQLInstanceListDelegate(string data);
		/// <summary>
		/// Adds an item to the list box in the SQL Instance Form.
		/// </summary>
		/// <param name="data">The item being added</param>
		public void AddItemToSQLInstanceList(string data)
		{
			if (this.InvokeRequired)
			{
				this.Invoke(new AddItemToSQLInstanceListDelegate(AddItemToSQLInstanceList),
					new object[] {data});
				return;
			}
			_sqlInstanceForm.AddItemToSQLInstanceList(data);
		}

		private delegate void AddItemToFolderListDelegate(string data);
		/// <summary>
		/// Adds an item to the list box in the folder Form.
		/// </summary>
		/// <param name="data">The item being added</param>
		public void AddItemToFolderList(string data)
		{
			if (this.InvokeRequired)
			{
				this.Invoke(new AddItemToFolderListDelegate(AddItemToFolderList),
					new object[] {data});
				return;
			}
			_folderForm.AddItemToFolderList(data);
		}

		private delegate void SetLinkDelegate(string link);
		/// <summary>
		/// Adds an item to the list box in the folder Form.
		/// </summary>
		/// <param name="data">The item being added</param>
		public void SetLink(string link)
		{
			if (this.InvokeRequired)
			{
				this.Invoke(new SetLinkDelegate(SetLink), new object[] {link});
				return;
			}
			linkLabelComplete.Text = link;
		}

		private void SQLInstanceChosen()
		{
			if (_radioInstallChecked)
			{
				textBoxRepositoryInstallServer.Text = _sqlInstanceForm.SelectedInstanceInBox();
			}
			else
			{
				textBoxRepositoryChangeServer.Text = _sqlInstanceForm.SelectedInstanceInBox();
			}
		}

		private void FolderChosen()
		{
			if (_radioInstallChecked)
			{
				textBoxRepositoryInstallFolder.Text = _folderForm.SelectedInstanceInBox();
			}
			else
			{
				textBoxRepositoryChangeFolder.Text = _folderForm.SelectedInstanceInBox();
			}
		}


		protected override void buttonPrev_Click(object sender, EventArgs e)
		{
			_currentPanel.PreviousPanel();
		}

		protected override void buttonNext_Click(object sender, EventArgs e)
		{
			_currentPanel.NextPanel();
		}

		protected override void buttonFinish_Click(object sender, EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			_currentPanel.Enabled = false;
			panelProgress.Enabled = true;
			panelProgress.BringToFront();

			_currentPanel = panelProgress;
			buttonPrev.Enabled = false;
			buttonFinish.Enabled = false;
			buttonPrev.Visible = false;
			buttonNext.Visible = false;
			buttonFinish.Visible = false;
			buttonCancel.Enabled = false;
			this.AcceptButton = null;
			richTextBoxProgress.Text = "";

			if (_radioInstallChecked)
			{
				Thread t = new Thread(new ThreadStart(DoInstall_InstallChecked));
				t.Start();
			}
			else
			{
				Thread t = new Thread(new ThreadStart(DoInstall_ChangeChecked));
				t.Start();
			}
		}

		private void DoInstall_InstallChecked()
		{
			bool success = true;

			//---------------
			// Create Folder
			//---------------
			UpdateInstallerLog("");
			UpdateInstallerLog("Start install"+"\r\n");
			bool createFolder = true;
			if (_createNewFolder)
			{
				createFolder = modelAdapter.CreateFolder(_computerPortName,
					textBoxReportingServicesAdvancedDirectory.Text,
					textBoxRepositoryInstallFolder.Text);
			}
			if (!createFolder)
			{
				// error handling
				success = false;
			}
			//--------------------
			// Create Data Source
			//--------------------
			bool createDataSource = true;
			if (_createNewDataSource)
			{
				createDataSource = modelAdapter.CreateDataSource(_computerPortName,
					textBoxReportingServicesAdvancedDirectory.Text,
					textBoxRepositoryInstallFolder.Text,
					SqlServerName, textBoxRepositoryInstallLogin.Text,
					textBoxRepositoryInstallPassword.Text, false);
			}
			if (!createDataSource)
			{
				// error handling
				success = false;
			}
			//-----------------
			// Publish Reports
			//-----------------
			UpdateInstallerLog("Publish Reports\r\n");
			bool deployReports ;
			foreach (string val in checkedListBoxReportsCustomAdd.CheckedItems)
			{
				deployReports = modelAdapter.DeployReport(_computerPortName, 
					textBoxReportingServicesAdvancedDirectory.Text, 
					textBoxRepositoryInstallFolder.Text, val);
				if (!deployReports)
				{
					// error handling
					success = false;
				}
			}
			foreach (string val in checkedListBoxReportsCustomUpdate.CheckedItems)
			{
				deployReports = modelAdapter.DeployReport(_computerPortName, 
					textBoxReportingServicesAdvancedDirectory.Text, 
					textBoxRepositoryInstallFolder.Text, val);
				if (!deployReports)
				{
					// error handling
					success = false;
				}
			}

			//-----------------
			// Create Shortcut
			//-----------------
			SetLink(modelAdapter.CreateShortcutLink(_computerPortName, textBoxReportingServicesAdvancedManager.Text));
//			linkLabelComplete.Text = modelAdapter.CreateShortcutLink(_computerPortName, textBoxReportingServicesAdvancedManager.Text);

			bool createNewShortcut = modelAdapter.CreateShortcut(_computerPortName, textBoxReportingServicesAdvancedManager.Text);
			if (!createNewShortcut)
			{
				// error handling
				success = false;
			}

			//--------------------------
			// Create Server and Folder
			//--------------------------
			bool insert = modelAdapter.InsertServerAndFolder(SqlServerName, _computerPortName,
				textBoxRepositoryInstallFolder.Text, textBoxReportingServicesAdvancedManager.Text);

			if (!insert)
			{
				// error handling
				success = false;
			}

			modelAdapter.WritePreferences(_regReportServerName, textBoxReportingServicesComputer.Text);
			modelAdapter.WritePreferences(_regReportFolderName, textBoxRepositoryInstallFolder.Text);
			modelAdapter.WritePreferences(_regReportDirectoryName, textBoxReportingServicesAdvancedDirectory.Text);
			modelAdapter.WritePreferences(_regReportManagerName, textBoxReportingServicesAdvancedManager.Text);
			modelAdapter.WritePreferences(_regSqlServerName, textBoxRepositoryInstallServer.Text);
			modelAdapter.WritePreferences(_regLoginName, textBoxRepositoryInstallLogin.Text);
			modelAdapter.WritePreferences(_regSslName, ((Boolean)checkBoxReportingServicesAdvancedSsl.Checked).ToString());
			modelAdapter.WritePreferences(_regPortName, textBoxReportingServicesAdvancedPort.Text);

			DoInstall_InstallChecked_Helper(success);
		}

		private delegate void DoInstall_InstallChecked_HelperDelegate(bool success);
		private void DoInstall_InstallChecked_Helper(bool success)
		{
			if (this.InvokeRequired)
			{
				this.Invoke(new DoInstall_InstallChecked_HelperDelegate(DoInstall_InstallChecked_Helper),
					new object[] {success});
				return;
			}
			if (success)
			{
				buttonCancel.Text = "&Finish";
				buttonCompleteError.Enabled = false;
				buttonCompleteError.Visible = false;
				base.GoToNextPanel(_currentPanel, panelComplete);
				_currentPanel = panelComplete;
				DoInstall_Helper();
			}
			else
			{
				buttonCancel.Text = "&OK";
				labelCompleteIntro.Text = "Installation Incomplete!";
				labelCompleteInstallIntro.Text = "The "+_productAbbrev+" Reports Installer was unable to complete your installation.";
				ArrayList list = new ArrayList();
				foreach (string item in listBoxComplete.Items)
				{
					list.Add(item);
				}
				listBoxComplete.Items.Clear();
				listBoxComplete.Items.Add("-----There are errors in your installation-----");
				listBoxComplete.Items.Add("Please click on Error Details for more information");
				listBoxComplete.Items.Add(" ");
				labelCompleteInstallChanges.Visible = false;

				if (list.Count > 0)
				{
					listBoxComplete.Items.Add("You have installed the following reports:");
				}

				foreach (string item in list)
				{
					listBoxComplete.Items.Add(item);
				}

				richTextBoxProgress.Cursor = Cursors.Default;
				DoInstall_Helper();
			}
		}

		private void DoInstall_ChangeChecked()
		{
			bool success = true;

			//--------------------
			// Create Data Source
			//--------------------
			bool createDataSource = modelAdapter.CreateDataSource(_computerPortName,
				textBoxReportingServicesAdvancedDirectory.Text,
				textBoxRepositoryChangeFolder.Text,
				SqlServerName, textBoxRepositoryChangeLogin.Text,
				textBoxRepositoryChangePassword.Text, true);

			if (!createDataSource)
			{
				// error handling
				success = false;
			}
//			labelCompleteInstallChanges.Text = "You have changed your data source to "+
//				SqlServerName+".";
//			listBoxComplete.Visible = false;

			//-----------------
			// Create Shortcut
			//-----------------
			SetLink(modelAdapter.CreateShortcutLink(_computerPortName, textBoxReportingServicesAdvancedManager.Text));
//			linkLabelComplete.Text = modelAdapter.CreateShortcutLink(_computerPortName, textBoxReportingServicesAdvancedManager.Text);

			bool createNewShortcut = modelAdapter.CreateShortcut(_computerPortName, textBoxReportingServicesAdvancedManager.Text);
			if (!createNewShortcut)
			{
				// error handling
				success = false;
			}

			//--------------------------
			// Create Server and Folder
			//--------------------------
			bool insert = modelAdapter.InsertServerAndFolder(SqlServerName, _computerPortName,
				textBoxRepositoryChangeFolder.Text, textBoxReportingServicesAdvancedManager.Text);
			if (!insert)
			{
				// error handling
				success = false;
			}

			modelAdapter.WritePreferences(_regReportServerName, textBoxReportingServicesComputer.Text);
			modelAdapter.WritePreferences(_regReportDirectoryName, textBoxReportingServicesAdvancedDirectory.Text);
			modelAdapter.WritePreferences(_regReportManagerName, textBoxReportingServicesAdvancedManager.Text);
			modelAdapter.WritePreferences(_regSslName, ((Boolean)checkBoxReportingServicesAdvancedSsl.Checked).ToString());
			modelAdapter.WritePreferences(_regPortName, textBoxReportingServicesAdvancedPort.Text);
			modelAdapter.WritePreferences(_regReportFolderName, textBoxRepositoryChangeFolder.Text);
			modelAdapter.WritePreferences(_regSqlServerName, textBoxRepositoryChangeServer.Text);
			modelAdapter.WritePreferences(_regLoginName, textBoxRepositoryChangeLogin.Text);

			DoInstall_ChangeChecked_Helper(success);
		}

		private delegate void DoInstall_ChangeChecked_HelperDelegate(bool success);
		private void DoInstall_ChangeChecked_Helper(bool success)
		{
			if (this.InvokeRequired)
			{
				this.Invoke(new DoInstall_ChangeChecked_HelperDelegate(DoInstall_ChangeChecked_Helper),
					new object[] {success});
				return;
			}
			labelCompleteInstallChanges.Text = "You have changed your data source to "+
				SqlServerName+".";
			listBoxComplete.Visible = false;

			if (success)
			{
				buttonCompleteError.Enabled = false;
				buttonCompleteError.Visible = false;
				buttonCancel.Text = "&Finish";
				base.GoToNextPanel(_currentPanel, panelComplete);
				_currentPanel = panelComplete;
				DoInstall_Helper();
			}
			else
			{
				buttonCancel.Text = "&OK";
				labelCompleteIntro.Text = "Installation Incomplete!";
				labelCompleteInstallIntro.Text = "The "+_productAbbrev+" Reports Installer was unable to complete your installation.";
					
				listBoxComplete.Visible = true;
				listBoxComplete.Items.Clear();
				listBoxComplete.Items.Add("-----There are errors in your installation-----");
				listBoxComplete.Items.Add("Please click on Error Details for more information");
				labelCompleteInstallChanges.Text = "The installer was unable to change your data source to "+
					SqlServerName+".";

				richTextBoxProgress.Cursor = Cursors.Default;
				DoInstall_Helper();
			}
		}

		private void DoInstall()
		{
			if (_radioInstallChecked)
			{
				DoInstall_InstallChecked();
			}
			else
			{
				DoInstall_ChangeChecked();
			}
		}

		private delegate void DoInstall_HelperDelegate();
		private void DoInstall_Helper()
		{
			if (this.InvokeRequired)
			{
				this.Invoke(new DoInstall_HelperDelegate(DoInstall_Helper),
					null);
				return;
			}
			buttonCancel.Enabled = true;
			this.AcceptButton = buttonCancel;
			this.Cursor = Cursors.Default;
		}

		protected override void buttonCancel_Click(object sender, EventArgs e)
		{
			if (buttonCancel.Text == "&Cancel")
			{
				bool choice = DisplayMessageBoxYesNo("Are you sure you want to exit the Reports Installer?",
					"Closing Installer");
				if (choice)
				{
					Application.Exit();
				}
			}
			else if(buttonCancel.Text == "&OK")
			{
				buttonCancel.Enabled = false;
				buttonCancel.Text = "&Finish";
				base.GoToNextPanel(_currentPanel, panelComplete);
				_currentPanel = panelComplete;
				buttonCancel.Enabled = true;
				this.AcceptButton = buttonCancel;
				this.Cursor = Cursors.Default;
			}
			else
			{
				Application.Exit();
			}
		}

		private void InstallerView_Load(object sender, System.EventArgs e)
		{
			if (modelAdapter.DoesReportsXMLFileExist() == false)
			{
				Application.Exit();
			}
			else
			{
				if (File.Exists(_firstPanelImage))
				{
					try
					{
						pictureBoxInstallationOptionsIntroSplash.Image = Image.FromFile(_firstPanelImage);
					}
					catch (Exception ex)
					{
					}
				}
				if (File.Exists(_lastPanelImage))
				{
					try
					{
						pictureBoxComplete.Image = Image.FromFile(_lastPanelImage);
					}
					catch (Exception ex)
					{
					}
				}
				if(File.Exists(_icon))
				{
					try
					{
						this.Icon = new Icon(_icon);
					}
					catch (Exception)
					{
					}
				}

				checkedListBoxReportsCustomAdd.Items.Clear();
				checkedListBoxReportsCustomUpdate.Items.Clear();
				
				if(!modelAdapter.ReportDescriptor())
				{
					Application.Exit();
				}

				_regReportServer = modelAdapter.ReadPreferences(_regReportServerName, _regReportServer);
				_regReportFolder = modelAdapter.ReadPreferences(_regReportFolderName, _regReportFolder);
				_regReportDirectory = modelAdapter.ReadPreferences(_regReportDirectoryName, _regReportDirectory);
				_regReportManager = modelAdapter.ReadPreferences(_regReportManagerName, _regReportManager);
				_regSqlServer = modelAdapter.ReadPreferences(_regSqlServerName, _regSqlServer);
				_regLogin = modelAdapter.ReadPreferences(_regLoginName, _regLogin);
				_regSsl = modelAdapter.ReadPreferences(_regSslName, _regSsl);
				_regPort = modelAdapter.ReadPreferences(_regPortName, _regPort);

				textBoxReportingServicesComputer.Text = _regReportServer;
				textBoxRepositoryInstallFolder.Text = _regReportFolder;
				textBoxRepositoryChangeFolder.Text = _regReportFolder;
				textBoxReportingServicesAdvancedDirectory.Text = _regReportDirectory;
				textBoxReportingServicesAdvancedManager.Text = _regReportManager;
				textBoxRepositoryInstallServer.Text = _regSqlServer;
				textBoxRepositoryChangeServer.Text = _regSqlServer;
				textBoxRepositoryInstallLogin.Text = _regLogin;
				textBoxRepositoryChangeLogin.Text = _regLogin;
				try
				{
					checkBoxReportingServicesAdvancedSsl.Checked = bool.Parse(_regSsl);
				}
				catch (Exception)
				{
					checkBoxReportingServicesAdvancedSsl.Checked = false;
				}
				textBoxReportingServicesAdvancedPort.Text = _regPort;

				_computerName = _regReportServer;
				_sqlServerName = _regSqlServer;
				_createNewFolder = true;
				_createNewDataSource = true;

				panelInstallationOptions.PreviousPanelEvent = new PreviousPanelEventHandler(PanelInstallationOptions_Previous);
				panelInstallationOptions.NextPanelEvent = new NextPanelEventHandler(PanelInstallationOptions_Next);
				panelReportingServices.PreviousPanelEvent = new PreviousPanelEventHandler(PanelReportingServices_Previous);
				panelReportingServices.NextPanelEvent = new NextPanelEventHandler(PanelReportingServices_Next);
				panelReportingServicesAdvanced.PreviousPanelEvent = new PreviousPanelEventHandler(PanelReportingServicesAdvanced_Previous);
				panelReportingServicesAdvanced.NextPanelEvent = new NextPanelEventHandler(PanelReportingServicesAdvanced_Next);
				panelRepositoryInstall.PreviousPanelEvent = new PreviousPanelEventHandler(PanelRepositoryInstall_Previous);
				panelRepositoryInstall.NextPanelEvent = new NextPanelEventHandler(PanelRepositoryInstall_Next);
				panelRepositoryChange.PreviousPanelEvent = new PreviousPanelEventHandler(PanelRepositoryChange_Previous);
				panelRepositoryChange.NextPanelEvent = new NextPanelEventHandler(PanelRepositoryChange_Next);
				panelReports.PreviousPanelEvent = new PreviousPanelEventHandler(PanelReports_Previous);
				panelReports.NextPanelEvent = new NextPanelEventHandler(PanelReports_Next);
				panelReportsCustom.PreviousPanelEvent = new PreviousPanelEventHandler(PanelReportsCustom_Previous);
				panelReportsCustom.NextPanelEvent = new NextPanelEventHandler(PanelReportsCustom_Next);
				panelSummary.PreviousPanelEvent = new PreviousPanelEventHandler(PanelSummary_Previous);
				panelSummary.NextPanelEvent = new NextPanelEventHandler(PanelSummary_Next);
				panelProgress.PreviousPanelEvent = new PreviousPanelEventHandler(PanelProgress_Previous);
				panelProgress.NextPanelEvent = new NextPanelEventHandler(PanelProgress_Next);
				panelComplete.PreviousPanelEvent = new PreviousPanelEventHandler(PanelComplete_Previous);
				panelComplete.NextPanelEvent = new NextPanelEventHandler(PanelComplete_Next);

				panelInstallationOptions.Enabled = true;
				panelReportingServices.Enabled = false;
				panelReportingServicesAdvanced.Enabled = false;
				panelRepositoryInstall.Enabled = false;
				panelRepositoryChange.Enabled = false;
				panelReports.Enabled = false;
				panelReportsCustom.Enabled = false;
				panelSummary.Enabled = false;
				panelProgress.Enabled = false;
				panelComplete.Enabled = false;

				_currentPanel = panelInstallationOptions;
				panelInstallationOptions.BringToFront();
				radioInstall.Focus();

				_radioInstallChecked = true;

				_radioUpdateChecked = true;
				_radioAddChecked = false;

				buttonPrev.Enabled = false;
				buttonNext.Enabled = true;
				buttonFinish.Enabled = false;
				buttonCancel.Enabled = true;
				this.AcceptButton = buttonNext;

				_sqlInstanceForm.Initialize(_icon);
				_folderForm.Initialize(_icon);
				_sqlInstanceForm.OkButtonEvent = new SQLInstanceFormEventHandler(SQLInstanceChosen);
				_folderForm.OkButtonEvent = new FolderFormEventHandler(FolderChosen);
			}
		}

		#region Wizard Panel Events

		public void PanelInstallationOptions_Previous()
		{
		}

		public void PanelInstallationOptions_Next()
		{
			this.Cursor = Cursors.WaitCursor;
			PanelInstallationOptions_Helper(false);
			base.GoToNextPanel(_currentPanel, panelReportingServices);
			_currentPanel = panelReportingServices;
			textBoxReportingServicesComputer.Focus();
			PanelInstallationOptions_Helper(true);
			this.AcceptButton =  buttonNext;
			this.Cursor = Cursors.Default;
		}

		private void PanelInstallationOptions_Helper(bool tf)
		{
			buttonPrev.Enabled = tf;
			buttonCancel.Enabled = tf;
			buttonNext.Enabled = tf;
			buttonCancel.Enabled = tf;
			radioInstall.Enabled = tf;
			radioChange.Enabled = tf;
		}

		public void PanelReportingServices_Previous()
		{
			this.Cursor = Cursors.WaitCursor;
			PanelReportingServices_Helper(false);

			base.GoToPreviousPanel(_currentPanel, panelInstallationOptions);
			_currentPanel = panelInstallationOptions;
			if (radioInstall.Checked)
			{
				radioInstall.Focus();
			}
			else if (radioChange.Checked)
			{
				radioChange.Focus();
			}
			PanelReportingServices_Helper(true);
			buttonPrev.Enabled = false;
		}

		public void PanelReportingServices_Next()
		{
			this.Cursor = Cursors.WaitCursor;
			PanelReportingServices_Helper(false);

			if (textBoxReportingServicesComputer.Text.Length == 0)
			{
				DisplayMessageBox("Report Server Computer is a required field.", "Empty Field!");
				PanelReportingServices_Helper(true);
			}
			else if (textBoxReportingServicesAdvancedPort.Text.Length == 0)
			{
				DisplayMessageBox("Port Number is a required field.", "Empty Field!");
				PanelReportingServices_Helper(true);
			}
			else if (textBoxReportingServicesAdvancedDirectory.Text.Length == 0)
			{
				DisplayMessageBox("Report Server Virtual Directory is a required field.", "Empty Field!");
				PanelReportingServices_Helper(true);
			}
			else if (textBoxReportingServicesAdvancedManager.Text.Length == 0)
			{
				DisplayMessageBox("Report Manager Virtual Directory is a required field.", "Empty Field!");
				PanelReportingServices_Helper(true);
			}
			else
			{
				if (checkBoxReportingServicesAdvancedSsl.Checked)
				{
					modelAdapter.SetSslProxy();
				}
				else
				{
					modelAdapter.SetNormalProxy();
				}
				_computerName = modelAdapter.ParseHostSite(textBoxReportingServicesComputer.Text);
				if ( (checkBoxReportingServicesAdvancedSsl.Checked && textBoxReportingServicesAdvancedPort.Text.Equals("443"))
					|| (!checkBoxReportingServicesAdvancedSsl.Checked && textBoxReportingServicesAdvancedPort.Text.Equals("80")))
				{
					_computerPortName = ComputerName;
				}
				else
				{
					_computerPortName = ComputerName + ":" + textBoxReportingServicesAdvancedPort.Text;
				}
				if (modelAdapter.TestHostComputer(_computerPortName))
				{
					if (modelAdapter.TestReportManager(_computerPortName, textBoxReportingServicesAdvancedManager.Text))
					{
						if (modelAdapter.TestReportServer(_computerPortName, textBoxReportingServicesAdvancedDirectory.Text))
						{
							if (modelAdapter.HasAppropriatePermissions(_computerPortName, textBoxReportingServicesAdvancedDirectory.Text))
							{
								if (_radioInstallChecked)
								{
									base.GoToNextPanel(_currentPanel, panelRepositoryInstall);
									_currentPanel = panelRepositoryInstall;
									textBoxRepositoryInstallServer.Focus();
								}
								else
								{
									base.GoToNextPanel(_currentPanel, panelRepositoryChange);
									_currentPanel = panelRepositoryChange;
									textBoxRepositoryChangeServer.Focus();
								}
							}
						}
					}
				}
				PanelReportingServices_Helper(true);
			}
		}

		private void PanelReportingServices_Helper(bool tf)
		{
			buttonPrev.Enabled = tf;
			buttonNext.Enabled = tf;
			buttonCancel.Enabled = tf;
			textBoxReportingServicesComputer.Enabled = tf;
			buttonReportingServicesAdvanced.Enabled = tf;

			if (tf)
			{
				this.AcceptButton =  buttonNext;
				this.Cursor = Cursors.Default;
			}
		}

		public void PanelReportingServicesAdvanced_Previous()
		{
			this.Cursor = Cursors.WaitCursor;
			PanelReportingServicesAdvanced_Helper(false);

			if (textBoxReportingServicesAdvancedPort.Text.Length == 0)
			{
				DisplayMessageBox("Port Number is a required field.", "Empty Field!");
				PanelReportingServicesAdvanced_Helper(true);
			}
			else if (textBoxReportingServicesAdvancedDirectory.Text.Length == 0)
			{
				DisplayMessageBox("Report Server Virtual Directory is a required field.", "Empty Field!");
				PanelReportingServicesAdvanced_Helper(true);
			}
			else if (textBoxReportingServicesAdvancedManager.Text.Length == 0)
			{
				DisplayMessageBox("Report Manager Virtual Directory is a required field.", "Empty Field!");
				PanelReportingServicesAdvanced_Helper(true);
			}
			else
			{
				PanelReportingServicesAdvanced_Helper(true);

				base.GoToPreviousPanel(_currentPanel, panelReportingServices);
				_currentPanel = panelReportingServices;
				textBoxReportingServicesComputer.Focus();
			}
		}

		public void PanelReportingServicesAdvanced_Next()
		{
			this.Cursor = Cursors.WaitCursor;
			PanelReportingServicesAdvanced_Helper(false);

			if (textBoxReportingServicesComputer.Text.Length == 0)
			{
				DisplayMessageBox("Report Server Computer is a required field.", "Empty Field!");
				PanelReportingServicesAdvanced_Helper(true);
			}
			else if (textBoxReportingServicesAdvancedPort.Text.Length == 0)
			{
				DisplayMessageBox("Port Number is a required field.", "Empty Field!");
				PanelReportingServicesAdvanced_Helper(true);
			}
			else if (textBoxReportingServicesAdvancedDirectory.Text.Length == 0)
			{
				DisplayMessageBox("Report Server Virtual Directory is a required field.", "Empty Field!");
				PanelReportingServicesAdvanced_Helper(true);
			}
			else if (textBoxReportingServicesAdvancedManager.Text.Length == 0)
			{
				DisplayMessageBox("Report Manager Virtual Directory is a required field.", "Empty Field!");
				PanelReportingServicesAdvanced_Helper(true);
			}
			else
			{
				if (checkBoxReportingServicesAdvancedSsl.Checked)
				{
					modelAdapter.SetSslProxy();
				}
				else
				{
					modelAdapter.SetNormalProxy();
				}
				_computerName = modelAdapter.ParseHostSite(textBoxReportingServicesComputer.Text);
				if ( (checkBoxReportingServicesAdvancedSsl.Checked && textBoxReportingServicesAdvancedPort.Text.Equals("443"))
					|| (!checkBoxReportingServicesAdvancedSsl.Checked && textBoxReportingServicesAdvancedPort.Text.Equals("80")))
				{
					_computerPortName = ComputerName;
				}
				else
				{
					_computerPortName = ComputerName + ":" + textBoxReportingServicesAdvancedPort.Text;
				}
				if (modelAdapter.TestHostComputer(_computerPortName))
				{
					if (modelAdapter.TestReportManager(_computerPortName, textBoxReportingServicesAdvancedManager.Text))
					{
						if (modelAdapter.TestReportServer(_computerPortName, textBoxReportingServicesAdvancedDirectory.Text))
						{
							if (modelAdapter.HasAppropriatePermissions(_computerPortName, textBoxReportingServicesAdvancedDirectory.Text))
							{
								if (_radioInstallChecked)
								{
									base.GoToNextPanel(_currentPanel, panelRepositoryInstall);
									_currentPanel = panelRepositoryInstall;
									textBoxRepositoryInstallServer.Focus();
								}
								else
								{
									base.GoToNextPanel(_currentPanel, panelRepositoryChange);
									_currentPanel = panelRepositoryChange;
									textBoxRepositoryChangeServer.Focus();
								}
							}
						}
					}
				}
				PanelReportingServicesAdvanced_Helper(true);
			}
		}

		private void PanelReportingServicesAdvanced_Helper(bool tf)
		{
			buttonPrev.Enabled = tf;
			buttonNext.Enabled = tf;
			buttonCancel.Enabled = tf;
			buttonReportingServicesAdvancedDefault.Enabled = tf;
			textBoxReportingServicesAdvancedPort.Enabled = tf;
			checkBoxReportingServicesAdvancedSsl.Enabled = tf;
			textBoxReportingServicesAdvancedDirectory.Enabled = tf;
			textBoxReportingServicesAdvancedManager.Enabled = tf;
			if (tf)
			{
				this.AcceptButton =  buttonNext;
				this.Cursor = Cursors.Default;
			}
		}

		public void PanelRepositoryInstall_Previous()
		{
			this.Cursor = Cursors.WaitCursor;
			PanelRepositoryInstall_Helper(false);

			base.GoToPreviousPanel(_currentPanel, panelReportingServices);
			_currentPanel = panelReportingServices;
			textBoxReportingServicesComputer.Focus();
			PanelRepositoryInstall_Helper(true);
		}

		public void PanelRepositoryInstall_Next()
		{
			this.Cursor = Cursors.WaitCursor;
			PanelRepositoryInstall_Helper(false);

			_sqlServerName = textBoxRepositoryInstallServer.Text;
			if(textBoxRepositoryInstallServer.Text.Length == 0)
			{
				DisplayMessageBox("SQL Server instance is a required field.", "Empty Field!");
				PanelRepositoryInstall_Helper(true);
			}
			else if(textBoxRepositoryInstallLogin.Text.Length == 0)
			{
				DisplayMessageBox("Logon ID is a required field.", "Empty Field!");
				PanelRepositoryInstall_Helper(true);
			}
			else if(textBoxRepositoryInstallPassword.Text.Length == 0)
			{
				DisplayMessageBox("Password is a required field.", "Empty Field!");
				PanelRepositoryInstall_Helper(true);
			}
			else if (textBoxRepositoryInstallFolder.Text.Length == 0)
			{
				DisplayMessageBox("Report Server Folder is a required field.", "Empty Field!");
				PanelRepositoryInstall_Helper(true);
			}
			else
			{
				if (modelAdapter.ValidateAccountName(textBoxRepositoryInstallLogin.Text) == false)
				{
				}
				else if(modelAdapter.TestConnection(SqlServerName) == false)
				{
				}
				else if(modelAdapter.ConfirmPassword(textBoxRepositoryInstallLogin.Text, textBoxRepositoryInstallPassword.Text) == false)
				{
				}
				else if(!modelAdapter.ValidateFolderName(_computerPortName, textBoxReportingServicesAdvancedDirectory.Text,
					textBoxRepositoryInstallFolder.Text))
				{
				}
				else
				{
					if (modelAdapter.DoesFolderExist(_computerPortName, textBoxReportingServicesAdvancedDirectory.Text,
						textBoxRepositoryInstallFolder.Text))
					{
						_createNewFolder = false;
						if (modelAdapter.DoesDataSourceExist(_computerPortName, textBoxReportingServicesAdvancedDirectory.Text,
							textBoxRepositoryInstallFolder.Text))
						{
							_createNewDataSource = false;
						}
						else
						{
							_createNewDataSource = true;
						}

						base.GoToNextPanel(_currentPanel, panelReports);
						_currentPanel = panelReports;
						if (_radioUpdateChecked)
						{
							radioUpdate.Focus();
						}
						else if (_radioAddChecked)
						{
							radioAdd.Focus();
						}
						else
						{
							radioCustom.Focus();
						}
					}
					else
					{
						bool contin = DisplayMessageBoxYesNo("The folder <" +textBoxRepositoryInstallFolder.Text+ 
									"> does not exist.  The installer can create a new folder with that name and path for you.  Is this okay?", "Create New Folder?");
						if (contin)
						{
							_createNewFolder = true;
							_createNewDataSource = true;

							base.GoToNextPanel(_currentPanel, panelReports);
							_currentPanel = panelReports;
							if (_radioUpdateChecked)
							{
								radioUpdate.Focus();
							}
							else if (_radioAddChecked)
							{
								radioAdd.Focus();
							}
							else
							{
								radioCustom.Focus();
							}
						}
					}	
				}
				PanelRepositoryInstall_Helper(true);
			}
		}

		private void PanelRepositoryInstall_Helper(bool tf)
		{
			buttonNext.Enabled = tf;
			buttonPrev.Enabled = tf;
			buttonCancel.Enabled = tf;
			textBoxRepositoryInstallServer.Enabled = tf;
			buttonRepositoryInstallBrowse.Enabled = tf;
			textBoxRepositoryInstallLogin.Enabled = tf;
			textBoxRepositoryInstallPassword.Enabled = tf;
			textBoxRepositoryInstallFolder.Enabled = tf;
			buttonRepositoryInstallBrowseFolder.Enabled = tf;
			if (tf)
			{
				this.AcceptButton = buttonNext;
				this.Cursor = Cursors.Default;
			}
		}

		public void PanelRepositoryChange_Previous()
		{
			this.Cursor = Cursors.WaitCursor;
			PanelRepositoryChange_Helper(false);

			base.GoToPreviousPanel(_currentPanel, panelReportingServices);
			_currentPanel = panelReportingServices;
			textBoxReportingServicesComputer.Focus();
			PanelRepositoryChange_Helper(true);
		}

		public void PanelRepositoryChange_Next()
		{
			this.Cursor = Cursors.WaitCursor;
			PanelRepositoryChange_Helper(false);

			_sqlServerName = textBoxRepositoryChangeServer.Text;
			if(textBoxRepositoryChangeServer.Text.Length == 0)
			{
				DisplayMessageBox("SQL Server instance is a required field.", "Empty Field!");
				PanelRepositoryChange_Helper(true);
			}
			else if(textBoxRepositoryChangeLogin.Text.Length == 0)
			{
				DisplayMessageBox("Logon ID is a required field.", "Empty Field!");
				PanelRepositoryChange_Helper(true);
			}
			else if(textBoxRepositoryChangePassword.Text.Length == 0)
			{
				DisplayMessageBox("Password is a required field.", "Empty Field!");
				PanelRepositoryChange_Helper(true);
			}
			else if (textBoxRepositoryChangeFolder.Text.Length == 0)
			{
				DisplayMessageBox("Report Server Folder is a required field.", "Empty Field!");
				PanelRepositoryChange_Helper(true);
			}
			else
			{
				if (modelAdapter.ValidateAccountName(textBoxRepositoryChangeLogin.Text) == false)
				{
					PanelRepositoryChange_Helper(true);
				}
				else if(modelAdapter.TestConnection(SqlServerName) == false)
				{
					PanelRepositoryChange_Helper(true);
				}
				else if(modelAdapter.ConfirmPassword(textBoxRepositoryChangeLogin.Text, textBoxRepositoryChangePassword.Text) == false)
				{
					PanelRepositoryChange_Helper(true);
				}
				else if(!modelAdapter.ValidateFolderName(_computerPortName, textBoxReportingServicesAdvancedDirectory.Text,
					textBoxRepositoryChangeFolder.Text))
				{
					PanelRepositoryChange_Helper(true);
				}
				else if (!modelAdapter.DoesFolderExist(_computerPortName, textBoxReportingServicesAdvancedDirectory.Text,
						textBoxRepositoryChangeFolder.Text))
				{
					DisplayMessageBox(textBoxRepositoryChangeFolder.Text+" does not exist.  Please specify a folder that already exists.", "Invalid Folder Name!");
					PanelRepositoryChange_Helper(true);
				}
				else
				{
					labelSummaryAdded.Text = "- Change your data source.";
					labelSummaryUpdate.Text = "";
					labelSummaryFolderDs.Text = "";
					base.GoToNextPanel(_currentPanel, panelSummary);
					_currentPanel = panelSummary;

					PanelRepositoryChange_Helper(true);
					buttonNext.Enabled = false;
					buttonFinish.Enabled = true;
					buttonFinish.Focus();
					this.AcceptButton = buttonFinish;
				}
			}
		}

		private void PanelRepositoryChange_Helper(bool tf)
		{
			buttonNext.Enabled = tf;
			buttonPrev.Enabled = tf;
			buttonCancel.Enabled = tf;
			textBoxRepositoryChangeServer.Enabled = tf;
			buttonRepositoryChangeBrowse.Enabled = tf;
			textBoxRepositoryChangeLogin.Enabled = tf;
			textBoxRepositoryChangePassword.Enabled = tf;
			textBoxRepositoryChangeFolder.Enabled = tf;
			buttonRepositoryChangeBrowseFolder.Enabled = tf;

			if (tf)
			{
				this.AcceptButton = buttonNext;
				this.Cursor = Cursors.Default;
			}
		}

		public void PanelReports_Previous()
		{
			this.Cursor = Cursors.WaitCursor;
			PanelReports_Helper(false);

			if (_radioInstallChecked)
			{
				base.GoToPreviousPanel(_currentPanel, panelRepositoryInstall);
				_currentPanel = panelRepositoryInstall;
				textBoxRepositoryInstallServer.Focus();
			}
			else
			{
				base.GoToPreviousPanel(_currentPanel, panelRepositoryChange);
				_currentPanel = panelRepositoryChange;
				textBoxRepositoryChangeServer.Focus();
			}
			PanelReports_Helper(true);
		}

		public void PanelReports_Next()
		{
			this.Cursor = Cursors.WaitCursor;
			PanelReports_Helper(false);

			checkedListBoxReportsCustomAdd.Items.Clear();
			checkedListBoxReportsCustomUpdate.Items.Clear();

			if (_createNewFolder)
			{
				modelAdapter.AvailableReports();
			}
			else
			{
				modelAdapter.ReportsFoundInFolder(_computerPortName, textBoxReportingServicesAdvancedDirectory.Text,
					textBoxRepositoryInstallFolder.Text);
			}

			if (_radioUpdateChecked)
			{
				int added = checkedListBoxReportsCustomAdd.Items.Count;
				int updated = checkedListBoxReportsCustomUpdate.Items.Count;

				for ( int i=0; i < added; i++ )
				{
					checkedListBoxReportsCustomAdd.SetItemChecked(i, true);
				}
				for ( int i=0; i < updated; i++ )
				{
					checkedListBoxReportsCustomUpdate.SetItemChecked(i, true);
				}

				if  ((checkedListBoxReportsCustomUpdate.CheckedItems.Count == 0) &&
					(checkedListBoxReportsCustomAdd.CheckedItems.Count == 0))
				{
					DisplayMessageBox("At least one report must be selected.", "Error!");
					
					PanelReports_Helper(true);
				}
				else
				{
					if (updated > 0)
					{
						MessageBox.Show(this, "Warning: The Reports Installer will overwrite existing reports.  "+
							"If you made changes  to existing reports, back up or rename the modified reports before "+
							"applying this upgrade.", "Warning!", MessageBoxButtons.OK,
							MessageBoxIcon.Warning);
					}

					labelSummaryAdded.Text = "- Add "+added+" reports.";
					labelSummaryUpdate.Text = "- Update "+updated+" reports.";
					if (_createNewFolder && _createNewDataSource)
					{
						labelSummaryFolderDs.Text = "- Create a new folder and a new data source.";
					}
					else if(_createNewDataSource)
					{
						labelSummaryFolderDs.Text = "- Create a new data source.";
					}
					else
					{
						labelSummaryFolderDs.Text = "";
					}
				
					base.GoToNextPanel(_currentPanel, panelSummary);
					_currentPanel = panelSummary;
					buttonFinish.Focus();
					
					PanelReports_Helper(true);
					buttonNext.Enabled = false;
					buttonFinish.Enabled = true;
					this.AcceptButton = buttonFinish;
				}
			}
			else if (_radioAddChecked)
			{
				int added = checkedListBoxReportsCustomAdd.Items.Count;
				int updated = checkedListBoxReportsCustomUpdate.Items.Count;
				
				for ( int i=0; i < added; i++ )
				{
					checkedListBoxReportsCustomAdd.SetItemChecked(i, true);
				}
				for ( int i=0; i < updated; i++ )
				{
					checkedListBoxReportsCustomUpdate.SetItemChecked(i, false);
				}

				if  ((checkedListBoxReportsCustomUpdate.CheckedItems.Count == 0) &&
					(checkedListBoxReportsCustomAdd.CheckedItems.Count == 0))
				{
					DisplayMessageBox("At least one report must be selected.", "Error!");
					PanelReports_Helper(true);
				}
				else
				{
					labelSummaryAdded.Text = "- Add "+added+" reports.";
					labelSummaryUpdate.Text = "- Update 0 reports.";
					if (_createNewFolder)
					{
						labelSummaryFolderDs.Text = "- Create a new folder and a new data source.";
					}
					else if(_createNewDataSource)
					{
						labelSummaryFolderDs.Text = "- Create a new data source.";
					}
					else
					{
						labelSummaryFolderDs.Text = "";
					}

					base.GoToNextPanel(_currentPanel, panelSummary);
					_currentPanel = panelSummary;
					buttonFinish.Focus();
					
					PanelReports_Helper(true);
					buttonNext.Enabled = false;
					buttonFinish.Enabled = true;
					this.AcceptButton = buttonFinish;
				}
			}
			else
			{
				int added = checkedListBoxReportsCustomAdd.Items.Count;
				int updated = checkedListBoxReportsCustomUpdate.Items.Count;

				base.GoToNextPanel(_currentPanel, panelReportsCustom);
				_currentPanel = panelReportsCustom;
				checkedListBoxReportsCustomAdd.Focus();
				for ( int i=0; i < added; i++ )
				{
					checkedListBoxReportsCustomAdd.SetItemChecked(i, true);
				}
				for ( int i=0; i < updated; i++ )
				{
					checkedListBoxReportsCustomUpdate.SetItemChecked(i, true);
				}
				PanelReports_Helper(true);
			}
		}

		private void PanelReports_Helper(bool tf)
		{
			buttonNext.Enabled =  tf;
			buttonPrev.Enabled =  tf;
			buttonCancel.Enabled =  tf;
			radioUpdate.Enabled =  tf;
			radioAdd.Enabled =  tf;
			radioCustom.Enabled =  tf;
			if (tf)
			{
				this.AcceptButton = buttonNext;
				this.Cursor = Cursors.Default;
			}
		}

		public void PanelReportsCustom_Previous()
		{
			this.Cursor = Cursors.WaitCursor;
			PanelReportsCustom_Helper(false);

			base.GoToPreviousPanel(_currentPanel, panelReports);
			_currentPanel = panelReports;
			if (_radioUpdateChecked)
			{
				radioUpdate.Focus();
			}
			else if (_radioAddChecked)
			{
				radioAdd.Focus();
			}
			else
			{
				radioCustom.Focus();
			}
			PanelReportsCustom_Helper(true);
		}

		public void PanelReportsCustom_Next()
		{
			this.Cursor = Cursors.WaitCursor;
			PanelReportsCustom_Helper(false);

			if  ((checkedListBoxReportsCustomUpdate.CheckedItems.Count == 0) &&
				(checkedListBoxReportsCustomAdd.CheckedItems.Count == 0))
			{
				DisplayMessageBox("At least one report must be selected.", "Error!");
				PanelReportsCustom_Helper(true);
			}
			else
			{
				int added = checkedListBoxReportsCustomAdd.CheckedItems.Count;
				int updated = checkedListBoxReportsCustomUpdate.CheckedItems.Count;

				if (updated > 0)
				{
					MessageBox.Show(this, "Warning: The Reports Installer will overwrite existing reports.  "+
						"If you made changes  to existing reports, back up or rename the modified reports before "+
						"applying this upgrade.", "Warning!", MessageBoxButtons.OK,
						MessageBoxIcon.Warning);
				}

				labelSummaryAdded.Text = "- Add "+added+" reports.";
				labelSummaryUpdate.Text = "- Update "+updated+" reports.";

				if (_createNewFolder && _createNewDataSource)
				{
					labelSummaryFolderDs.Text = "- Create a new folder and a new data source.";
				}
				else if(_createNewDataSource)
				{
					labelSummaryFolderDs.Text = "- Create a new data source.";
				}
				else
				{
					labelSummaryFolderDs.Text = "";
				}
			
				base.GoToNextPanel(_currentPanel, panelSummary);
				_currentPanel = panelSummary;
				PanelReportsCustom_Helper(true);
				buttonNext.Enabled = false;
				buttonFinish.Enabled = true;
				this.AcceptButton = buttonFinish;
				buttonFinish.Focus();
			}
		}

		private void PanelReportsCustom_Helper(bool tf)
		{
			buttonPrev.Enabled = tf;
			buttonNext.Enabled = tf;
			buttonCancel.Enabled = tf;
			checkedListBoxReportsCustomAdd.Enabled = tf;
			checkedListBoxReportsCustomUpdate.Enabled = tf;
			buttonReportsCustomAddSelect.Enabled = tf;
			buttonReportsCustomAddUnselect.Enabled = tf;
			buttonReportsCustomUpdateSelect.Enabled = tf;
			buttonReportsCustomUpdateUnselect.Enabled = tf;
			if (tf)
			{
				this.AcceptButton = buttonNext;
				this.Cursor = Cursors.Default;
			}
		}

		public void PanelSummary_Previous()
		{
			this.Cursor = Cursors.WaitCursor;
			buttonPrev.Enabled = false;
			buttonFinish.Enabled = false;
			buttonCancel.Enabled = false;

			if (!_radioInstallChecked)
			{
				base.GoToPreviousPanel(_currentPanel, panelRepositoryChange);
				_currentPanel = panelRepositoryChange;
				textBoxRepositoryChangeServer.Focus();
			}
			else if (_radioUpdateChecked)
			{
				base.GoToPreviousPanel(_currentPanel, panelReports);
				_currentPanel = panelReports;
				radioUpdate.Focus();
			}
			else if (_radioAddChecked)
			{
				base.GoToPreviousPanel(_currentPanel, panelReports);
				_currentPanel = panelReports;
				radioAdd.Focus();
			}
			else
			{
				base.GoToPreviousPanel(_currentPanel, panelReportsCustom);
				_currentPanel = panelReportsCustom;
				checkedListBoxReportsCustomAdd.Focus();
			}

			buttonPrev.Enabled = true;
			buttonNext.Enabled = true;
			buttonCancel.Enabled = true;
			this.AcceptButton = buttonNext;
			this.Cursor = Cursors.Default;
		}

		public void PanelSummary_Next()
		{
		}

		public void PanelProgress_Previous()
		{
			this.Cursor = Cursors.WaitCursor;
			buttonPrev.Enabled = false;
			buttonCancel.Enabled = false;
			base.GoToPreviousPanel(_currentPanel, panelSummary);
			_currentPanel = panelSummary;
			buttonPrev.Enabled = true;
			buttonFinish.Enabled = true;
			buttonCancel.Enabled = true;
			this.AcceptButton = buttonFinish;
			this.Cursor = Cursors.Default;
		}

		public void PanelProgress_Next()
		{
		}

		public void PanelComplete_Previous()
		{
		}

		public void PanelComplete_Next()
		{
		}
		#endregion

		private void radioInstall_CheckedChanged(object sender, System.EventArgs e)
		{
			_radioInstallChecked = true;
		}

		private void radioChange_CheckedChanged(object sender, System.EventArgs e)
		{
			_radioInstallChecked = false;
		}

		private void buttonReportingServicesAdvanced_Click(object sender, System.EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			PanelReportingServices_Helper(false);
			if (textBoxReportingServicesComputer.Text.Length == 0)
			{
				DisplayMessageBox("Report Server Computer is a required field.", "Empty Field!");
				PanelReportingServices_Helper(true);
			}
			else
			{
				base.GoToNextPanel(_currentPanel, panelReportingServicesAdvanced);
				_currentPanel = panelReportingServicesAdvanced;
				textBoxReportingServicesAdvancedPort.Focus();
				PanelReportingServices_Helper(true);
			}
		}

		private void buttonReportingServicesAdvancedDefault_Click(object sender, System.EventArgs e)
		{
			textBoxReportingServicesAdvancedPort.Text = "80";
			checkBoxReportingServicesAdvancedSsl.Checked = false;
			textBoxReportingServicesAdvancedDirectory.Text = "reportServer";
			textBoxReportingServicesAdvancedManager.Text = "reports";
			textBoxReportingServicesAdvancedPort.Focus();
		}

		private void buttonRepositoryInstallBrowse_Click(object sender, System.EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			PanelRepositoryInstall_Helper(false);

			modelAdapter.ListAvailableServers();
			this.Cursor = Cursors.Default;
			_sqlInstanceForm.ShowDialog(this);
			PanelRepositoryInstall_Helper(true);
		}

		private void buttonRepositoryInstallBrowseFolder_Click(object sender, System.EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			PanelRepositoryInstall_Helper(false);

			modelAdapter.PopulateFolderList(_computerPortName, textBoxReportingServicesAdvancedDirectory.Text);
			this.Cursor = Cursors.Default;
			_folderForm.ShowDialog(this);
			PanelRepositoryInstall_Helper(true);
		}

		private void buttonRepositoryChangeBrowse_Click(object sender, System.EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			PanelRepositoryChange_Helper(false);

			modelAdapter.ListAvailableServers();
			this.Cursor = Cursors.Default;
			_sqlInstanceForm.ShowDialog(this);
			PanelRepositoryChange_Helper(true);
		}

		private void buttonRepositoryChangeBrowseFolder_Click(object sender, System.EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			PanelRepositoryChange_Helper(false);
			modelAdapter.PopulateFolderList(_computerPortName, textBoxReportingServicesAdvancedDirectory.Text);
			this.Cursor = Cursors.Default;
			_folderForm.ShowDialog(this);
			PanelRepositoryChange_Helper(true);
		}

		private void radioUpdate_CheckedChanged(object sender, System.EventArgs e)
		{
			_radioUpdateChecked = true;
			_radioAddChecked = false;
		}

		private void radioAdd_CheckedChanged(object sender, System.EventArgs e)
		{
			_radioUpdateChecked = false;
			_radioAddChecked = true;
		}

		private void radioCustom_CheckedChanged(object sender, System.EventArgs e)
		{
			_radioUpdateChecked = false;
			_radioAddChecked = false;
		}

		private void buttonReportsCustomAddSelect_Click(object sender, System.EventArgs e)
		{
			for ( int i=0; i < checkedListBoxReportsCustomAdd.Items.Count; i++ )
			{
				checkedListBoxReportsCustomAdd.SetItemChecked(i, true);
			}
		}

		private void buttonReportsCustomAddUnselect_Click(object sender, System.EventArgs e)
		{
			for ( int i=0; i < checkedListBoxReportsCustomAdd.Items.Count; i++ )
			{
				checkedListBoxReportsCustomAdd.SetItemChecked(i, false);
			}
		}

		private void buttonReportsCustomUpdateSelect_Click(object sender, System.EventArgs e)
		{
			for ( int i=0; i < checkedListBoxReportsCustomUpdate.Items.Count; i++ )
			{
				checkedListBoxReportsCustomUpdate.SetItemChecked(i, true);
			}
		}

		private void buttonReportsCustomUpdateUnselect_Click(object sender, System.EventArgs e)
		{
			for ( int i=0; i < checkedListBoxReportsCustomUpdate.Items.Count; i++ )
			{
				checkedListBoxReportsCustomUpdate.SetItemChecked(i, false);
			}
		}

		private void linkLabelComplete_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(linkLabelComplete.Text);
		}

		#region Shortcut Keys for text fields
		public void labelReportingServicesComputer_Enter(Object sender, System.EventArgs e)
		{
			this.textBoxReportingServicesComputer.Focus();
		}

		public void labelReportingServicesAdvancedPort_Enter(Object sender, System.EventArgs e)
		{
			this.textBoxReportingServicesAdvancedPort.Focus();
		}

		public void labelReportingServicesAdvancedDirectory_Enter(Object sender, System.EventArgs e)
		{
			this.textBoxReportingServicesAdvancedDirectory.Focus();
		}

		public void labelReportingServicesAdvancedManager_Enter(Object sender, System.EventArgs e)
		{
			this.textBoxReportingServicesAdvancedManager.Focus();
		}

		public void labelRepositoryChangeServer_Enter(Object sender, System.EventArgs e)
		{
			this.textBoxRepositoryChangeServer.Focus();
		}

		public void labelRepositoryChangeLogin_Enter(Object sender, System.EventArgs e)
		{
			this.textBoxRepositoryChangeLogin.Focus();
		}

		public void labelRepositoryChangePassword_Enter(Object sender, System.EventArgs e)
		{
			this.textBoxRepositoryChangePassword.Focus();
		}

		public void labelRepositoryChangeFolder_Enter(Object sender, System.EventArgs e)
		{
			this.textBoxRepositoryChangeFolder.Focus();
		}

		public void labelRepositoryInstallServer_Enter(Object sender, System.EventArgs e)
		{
			this.textBoxRepositoryInstallServer.Focus();
		}

		public void labelRepositoryInstallLogin_Enter(Object sender, System.EventArgs e)
		{
			this.textBoxRepositoryInstallLogin.Focus();
		}

		public void labelRepositoryInstallPassword_Enter(Object sender, System.EventArgs e)
		{
			this.textBoxRepositoryInstallPassword.Focus();
		}

		public void labelRepositoryInstallFolder_Enter(Object sender, System.EventArgs e)
		{
			this.textBoxRepositoryInstallFolder.Focus();
		}
		#endregion

		private void buttonCompleteError_Click(object sender, System.EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			buttonCancel.Enabled = false;
			buttonCancel.Text = "&OK";
			buttonCompleteError.Enabled = false;
			base.GoToPreviousPanel(_currentPanel, panelProgress);
			_currentPanel = panelProgress;
			buttonCompleteError.Enabled = true;
			buttonCancel.Enabled = true;
			this.AcceptButton = buttonCancel;
			this.Cursor = Cursors.Default;
		
		}
		
	}
}

