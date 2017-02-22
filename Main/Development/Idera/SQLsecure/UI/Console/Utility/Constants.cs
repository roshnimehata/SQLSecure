using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Microsoft.Win32;

using Idera.SQLsecure.Core.Logger;

namespace Idera.SQLsecure.UI.Console.Utility
{
    internal enum ExplorerBarGroup
    {
        SecuritySummary,
        ExplorePermissions,
        Reports,
        ManageSQLsecure
    }

    internal enum View
    {
        StartPage,
        Main_SecuritySummary,
        Main_ExplorePermissions,
        Main_Reports,
        Main_ManageSQLsecure,
        PolicyAssessment,
        SQLsecureActivity,
        Server,
        PermissionExplorer,
        Logins,
        ManagePolicies,
        Report_GuestEnabledDatabases,
        Report_AuditedServers,
        Report_Filters,
        Report_CrossServerLoginCheck,
        Report_MixedModeAuthentication,
        Report_ServersWithDangerousGroups,
        Report_SystemAdministratorVulnerability,
        Report_SuspectWindowsAccounts,
        Report_VulnerableFixedRoles,
        Report_CMDShellVulnerability,
        Report_ServerLoginsAndUserMappings,
        Report_DatabaseChaining,
        Report_MailVulnerability,
        Report_OSVulnerability,
        Report_UsersPermissions,
        Report_AllObjectsWithPermissions,
        Report_DatabaseRoles,
        Report_ServerRoles,
        Report_Users,
        Report_ActivityHistory,
        Report_RiskAssessment,
        Report_CompareAssessments,
        Report_CompareSnapshots,
        Report_LoginVulnerability,
        ServerGroupTags,
        Report_SuspectSqlLogins
    }

    // These are the commands that need to be processed
    // by the specific view that is handling the menu
    // events.
    enum ViewSpecificCommand
    {
        Delete,
        Properties,
        Refresh,
        Configure,
        Collect,
        GroupByBox,
        UserPermissions,
        ObjectPermissions,
        Snapshots,
        Baseline,
        NewLogin,
        NewAuditServer,
        NewPolicy
    }

    internal static class Constants
    {
        #region General

        public const int NOT_FOUND = -1;
        public const int NOT_SELECTED = -1;
        public const string AUDIT_FOLDER_DELIMITER = "|";
        public const string FILE_SCHEME = "file";

        #endregion

        #region General Product info

        public static String COMPANY_STR = @"Idera";
        public static String PRODUCT_STR = @"SQLsecure";
        public static String PRODUCT_VER_STR = @"3.1";
        // Previous version strings (newest first) to find previous option files
        public static String[] PRODUCT_VER_STR_PREV = { @"3.0",@"2.9",@"2.8",@"2.7",@"2.6", @"2.5", @"2.0", @"1.2", @"1.1" };
        public static String COMPONENT_STR = @"Console";

        public static String APP_TITLE_STR = COMPANY_STR + @" " + PRODUCT_STR + @" - {0}";

        public static String OPTIONS_FILE_EXTENSION_STR = @".options.xml";

        public const int DalVersion = 3100;
        public const int SchemaVersion = 3100;

        public const string COPYRIGHT_MSG = @"© Copyright 2005-2016 Idera, Inc., all rights reserved. SQLsecure, Idera and the Idera Logo are trademarks or registered trademarks of Idera or its subsidiaries in the United States and other jurisdictions.";

        public const string BETA_VERSION = ""; //" (BETA VERSION)"; 

        internal const int SQLsecureProductID = 1000;
        internal const string SQLsecureLicenseProductVersionStr = "1.1";

        public const string ProductsPageText = "Idera SQLsecure helps you detect and verify security holes in your SQL Server security model. SQLsecure does this by performing rights analysis across SQL Server, Active Directory and Windows and calculating the effective access rights for any user, object or access control.";

        internal enum ExitCode
        {
            Success = 0,
            ScriptNotExist = -999,
            ScriptFailure = -1000
        }

        //SQLsecure 3.1 (Tushar)--Enums for type of server and authentication.
        public enum typeOfServer
        {
            azureVM,
            azureDB,
            remoteVM,
            onPremise
        };

        public enum type_of_authentication
        {
            windows,
            sa
        };

        #endregion

        #region Date & Time

        public static string DATE_FORMAT = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
        public static string TIME_FORMAT = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern;

        internal static string DATETIME_FORMAT = DATE_FORMAT + @" " + TIME_FORMAT;

        public static string PARAM_DATETIME_FORMAT = System.Globalization.CultureInfo.GetCultureInfo("en-US").DateTimeFormat.SortableDateTimePattern;

        #endregion

        #region SQL Server Stuff

        internal const string Role_Admin = @"Admin";
        internal const string Role_View = @"View";
        internal const string Role_NoAccess = @"No Access";

        internal const string RepositoryServerDefault = "";

        #endregion

        #region Grids

        // Infragistics uses the Hidden property instead of Visible so this is consistent with that
        internal const bool InitialState_GroupByBoxHidden = true;

        #endregion

        #region SQLsecure Registry

        internal static String REGISTRY_ROOT = @"HKEY_LOCAL_MACHINE\SOFTWARE\";
        internal static String REGISTRY_SEP = @"\";
        internal static String REGISTRY_PATH = REGISTRY_ROOT +
                                                COMPANY_STR +
                                                REGISTRY_SEP +
                                                PRODUCT_STR +
                                                REGISTRY_SEP +
                                                COMPONENT_STR;

        internal const String RegKey_DefaultRepository = @"DefaultRepository";

        internal const String RegKey_IderaProducts = @"SOFTWARE\Idera\RegisteredProducts\";

        #endregion

        #region Logging
        //internal const LoggingLevel DfltLogLevel = LoggingLevel.Error;

        //internal static String RegKey_SQLsecureLogging_Str = @"Software\Idera\SQLsecure\Logging";
        //internal const string RegValue_LogFolder_Str = @"LogFolder";
        //internal const string RegValue_UILogLevel_Str = @"UILogLevel";

        ////internal const string RegKey_DefaultServer = @"\Logs";
        //internal const string DfltLogFolderLeaf = @"Logs";
        //internal const string DfltLogFileExtension = @".log";

        #endregion

        #region Param processing

        internal const string Repository = "-Repository";        // SQLsecure Repository
        internal const string SetDefault = "-DefaultRepository"; // Save Installation Repository
        internal const string Verbose = "-Verbose";

        internal const string CopyrightMsg = "\nCopyright © 2005-2016, Idera, Inc.";
        internal static string UsageMsg = "\n" + PRODUCT_STR + @"  audits SQL Server security data and associated\n"
                                                      + "Windows accounts.\n\n"
                                                      + "Usage: Idera." + PRODUCT_STR + @".Console\n"
                                                      + "       [-DefaultRepository REPOSITORYSERVER]\n"
                                                      + "       [-Repository REPOSITORYSERVER] [-Verbose]\n"
                                                      + "\n(identifiers are shown in uppercase, [] means optional)\n\n"
                                                      + "   DefaultRepository : Set default Repository SQL Server to use /n"
                                                      + "                     :    without opening console.\n"
                                                      + "   Repository        : Repository SQL Server to open at startup.\n"
                                                      + "   Verbose           : Displays messages to the console.\n";
        #endregion

        #region MainForm - Explorer Bar

        // Explorer Bar Group keys
        public const String ExplorerBar_GroupKey_Summary = @"SecuritySummary";
        public const String ExplorerBar_GroupKey_Explore = @"ExplorePermissions";
        public const String ExplorerBar_GroupKey_Reports = @"Reports";
        public const String ExplorerBar_GroupKey_Manage = @"ManageSQLsecure";

        // Explorer Tree Root Nodes
        public static String RootNode_Policy = "No policies found";
        public static String RootNode_Summary = "(Select a Policy)";
        public static String RootNode_Explore = "Audited SQL Servers";
        public static String RootNode_Reports = PRODUCT_STR + @" Reports";
        public static String RootNode_Manage = PRODUCT_STR + @" Management";

        // Report Tree Category Nodes
        public static String ReportsNode_Category_General = "General";
        public static String ReportsNode_Category_Entitlement = "Entitlement";
        public static String ReportsNode_Category_Vulnerability = "Vulnerability";
        public static String ReportsNode_Category_Comparison = "Comparison";

        // Report Tree Report Nodes
        public static String ReportNode_AuditedServers = ReportTitle_AuditedServers;
        public static String ReportNode_GuestEnabledServers = ReportTitle_GuestEnabledDatabases;
        public static String ReportNode_CrossServerLoginCheck = ReportTitle_CrossServerLoginCheck;
        public static String ReportNode_MixedModeAuthentication = ReportTitle_MixedModeAuthentication;
        public static String ReportNode_ServersWithDangerousGroups = ReportTitle_ServersWithDangerousGroups;
        public static String ReportNode_SystemAdministratorVulnerability = ReportTitle_SystemAdministratorVulnerability;
        public static String ReportNode_SuspectWindowsAccounts = ReportTitle_SuspectWindowsAccounts;
        public static String ReportNode_SuspectSqlLogins = ReportTitle_SuspectSqlLogins;
        public static String ReportNode_VulnerableFixedRoles = ReportTitle_VulnerableFixedRoles;
        public static String ReportNode_CMDShellVulnerability = ReportTitle_CMDShellVulnerability;
        public static String ReportNode_MailVulnerability = ReportTitle_MailVulnerability;
        public static String ReportNode_LoginVulnerability = ReportTitle_LoginVulnerability;
        public static String ReportNode_ServerLoginsAndUserMappings = ReportTitle_ServerLoginsAndUserMappings;
        public static String ReportNode_UserPermissions = ReportTitle_UsersPermissions;
        public static String ReportNode_DatabaseChaining = ReportTitle_DatabaseChaining;
        public static String ReportNode_OSVulnerability = ReportTitle_OSVulnerability;
        public static String ReportNode_AllObjectsWithPermissions = ReportTitle_AllObjectsWithPermissions;
        public static String ReportNode_DatabaseRoles = ReportTitle_DatabaseRoles;
        public static String ReportNode_ServerRoles = ReportTitle_ServerRoles;
        public static String ReportNode_ActivityHistory = ReportTitle_ActivityHistory;
        public static String ReportNode_Users = ReportTitle_Users;
        public static String ReportNode_RiskAssessment = ReportTitle_RiskAssessment;
        public static String reportNode_CompareAssessments = ReportTitle_CompareAssessments;
        public static String reportNode_CompareSnapshots = ReportTitle_CompareSnapshots;

        // Manage SQLsecure Root Nodes
        public const String ManagementNode_Servers = @"Repository Status";
        public const String ManagementNode_Logins = @"Logins";
        public const String ManagementNode_ManagePolicies = @"Manage Policies";
        public static String ManagementNode_Activity = PRODUCT_STR + @" Activity";
        public const String TManagementNode_TagsNode = "Server Group Tags";


        // Snapshot Nodes
        public const string MoreSnapshots = "more Snapshots...";
        public const int SnapshotCount = 5;

        // Assessment Nodes
        public const string MoreAssessments = "more Assessments...";
        public const int AssessmentCount = 5;

        #endregion

        #region Menus

        #region Main Menus

        public const String Menu_Descr_Deploy_Repository = @"Deploy SQLsecure Repository";
        public const String Menu_Descr_File_Connect = @"Connect to another SQLsecure Repository";
        public const String Menu_Descr_File_ConnectionProperties = @"View connection properties for the current Repository";
        public const String Menu_Descr_File_NewSQLServer = @"Register a new SQL Server instance to audit";
        public const String Menu_Descr_File_NewLogin = @"Create SQLsecure login and assign permissions";
        public const String Menu_Descr_File_License = @"Update SQLsecure license";

        public const String Menu_Descr_Edit_Remove = @"Remove selected object from Repository";
        public const String Menu_Descr_Edit_Configure = @"Specify how permissions are audited";
        public const String Menu_Descr_Edit_Properties = @"View properties of selected object";

        public const String Menu_Descr_View_Tasks = @"Show or hide common tasks";
        public const String Menu_Descr_View_ConsoleTree = @"Show or hide the console tree";
        public const String Menu_Descr_View_Toolbar = @"Show or hide the tool bar";

        public const String Menu_Descr_Permissions_User = @"Find permissions assigned to a user";
        public const String Menu_Descr_Permissions_Object = @"Find permissions on a SQL Server object";

        public const String Menu_Descr_Snapshots_Collect = @"Collect SQL Server security data from selected instance";
        public const String Menu_Descr_Snapshots_Baseline = @"Mark snapshot as a baseline";
        public const String Menu_Descr_Snapshots_CheckIntegrity = @"Ensure snapshot data has not been modified";

        #endregion

        #endregion

        #region Tasks

        public const String ShowTasks = "Show tasks";
        public const String HideTasks = "Hide tasks";

        #region Common Tasks

        public const String Task_Title_Register = @"Register a SQL Server";
        public const String Task_Title_Import = @"Import SQL Servers";
        public const String Task_Descr_Register = @"Start auditing security data for a SQL Server and its databases";
        public const String Task_Title_Configure = @"Configure Audit Settings";
        public const String Task_Title_Configure_Short = @"Configure";
        public const String Task_Descr_Configure = @"Configure collection schedule and select objects to audit";
        public const String Task_Title_Collect = @"Take Snapshot";
        public const String Task_Descr_Collect = @"Collect SQL Server security data from this server now";
        public const String Task_Title_User_Short = @"Explore Users";
        public const String Task_Title_User = @"Explore User Permissions";
        public const String Task_Descr_User = @"Determine effective security permissions for a user";
        public const String Task_Title_Object = @"Explore Object Permissions";
        public const String Task_Title_Object_Short = @"Explore Objects";
        public const String Task_Descr_Object = @"Determine security permissions effective for a given object";
        public const String Task_Title_Monitor = @"Monitor Compliance";
        public const String Task_Descr_Monitor = @"Be notified of entitlement changes for a user or object";
        public const String Task_Title_Reports = @"Generate Entitlement Reports";
        public const String Task_Descr_Reports = @"Generate security audit and compliance reports";
        public const String Task_Title_NewUser = @"New SQLsecure Login";
        public const String Task_Descr_NewUser = @"Create a new SQLsecure login";
        public const String Task_Title_License = @"Manage License";
        public const String Task_Descr_License = @"View and update server licenses for SQLsecure";

        public const String Task_Title_UserPermissions = "Find User Permissions";
        public const String Task_Title_CollectData = "Take Snapshot Now";
        public const String Task_Title_NewLogins = "New Login";

        #endregion

        #endregion

        #region Views

        // The main page title is used differently from the other titles
        // It displays as a title for the summary in the task area replacement
        public const String ViewTitle_Main_Explore = "Assess && audit security risks and access rights for SQL Server";
        public static String ViewSummary_Main_Explore = PRODUCT_STR + " helps you ensure the integrity of the Microsoft "
                                + "SQL Server security model. With " + PRODUCT_STR + " you can analyze entitlements "
                                + "and identify discrepancies. " + PRODUCT_STR + " provides extensive entitlement "
                                + "reports to satisfy audit and compliance requirements.";

        public const String ViewTitle_ManageSQLsecure = "Repository Status";
        public static String ViewSummary_ManageSQLsecure = "This window lists the SQL Server instances that are registered with SQLsecure.  Registering a SQL Server instance allows you to perform comprehensive capture, analysis and reporting on SQL Server and related AD and OS security.";

        public static String ViewTitle_SQLsecureActivity = PRODUCT_STR + " Activity";
        public static String ViewSummary_SQLsecureActivity = "This window lists changes and events related to SQLsecure administration, data collection, filter rules, and jobs management.   This allows you to monitor SQLsecure operations.";

        public const String ViewTitle_Main_Reports = "Report on SQL Server permissions";
        public const String ViewSummary_Main_Reports = "This window allows you to generate reports on SQL Server permissions. Use reports to confirm regulatory compliance and enforce security policies.";

        public const String ViewTitle_Permissions = @"Explore User Permissions";
        public const String ViewSummary_Permissions = @"This window allows you to explore user and object permissions and view snapshot details.";

        public const String ViewTitle_Server = @"Server Summary";
        public const String ViewSummary_Server = @"This window allows you to view your audited SQL Server instance and Audit History statistics and information.";

        public const String ViewTitle_PermissionExplorer = @"Permission Explorer";
        public const String ViewSummary_PermissionExplorer = @"Find and view user permissions on various objects";

        public const String ViewSummary_Logins = "This window allows you to manage SQLsecure users.   Use this window to create new users and modify rights of an existing user.";

        #endregion

        #region Reports

        // Reports Services Reports Installer
        public const String ReportsInstaller = "Idera.SQLsecure.Reports.Installer.exe";
        public static String REPORTS_TITLE_STR = "{0}";
        public const String ReportDateFormat = "Most current audit data as of {0}";
        public const String ReportDateTimeFormat = "Most current audit data as of {0} at {1}";

        // These are the report titles, descriptions, and instructions
        public const string ReportRunInstructions_MultiStep = @"Follow these steps to create a report:";
        public const string ReportRunInstructions_NoParameters = @"Click the ""View Report"" button to generate your report.";
        public const string ReportRunInstructions_LoginType = @"Choose the type of Login.";
        public const string ReportRunInstructions_UserName = @"Type or browse for the User name.";
        public const string ReportRunInstructions_Server = @"Select a target SQL Server instance.";
        public const string ReportRunInstructions_Database = @"Select a Database you would like to analyze.";
        public const string ReportRunInstructions_PermissionType = @"Select a permisson type.";
        public const string ReportRunInstructions_StartDate = @"Choose a start date and time for the report. (in UTC)";
        public const string ReportRunInstructions_EndDate = @"Choose an end date and time for the report. (in UTC)";
        public const string ReportRunInstructions_ActivityType = @"Select the activity type you want the report to cover.";
        public const string ReportRunInstructions_ShowAlertsOnly = @"Check ""Show Risks Only"" to only show risks.";
        public const string ReportRunInstructions_ShowDifferencesOnly = @"Check ""Show Differences Only"" to only show items that do not match.";
        public const string ReportRunInstructions_UseSelection = @"Select Date, Policy, and Baseline options from the Report Settings box.";
        public const string ReportRunInstructions_Assessment = @"Select an assessment.";
        public const string ReportRunInstructions_SnapshotsToCompare = @"Select the snapshots to compare.";
        public const string ReportRunInstructions_AssessmentsToCompare = @"Select the assessments to compare.";
        public const string ReportRunInstructions_UsePolicy = @"Select Policy from the Report Settings box.";

        public const string ReportWarning_Resources = @"This report can take significant time to generate and may heavily use your system resources. Consider running it from Microsoft Reporting Services instead.";

        // General Reports
        public const string ReportTitle_AuditedServers = @"Audited SQL Servers";
        public static string ReportSummary_AuditedServers = @"Show all the SQL Server instances that are being audited by " + PRODUCT_STR + ".";

        public const string ReportTitle_CrossServerLoginCheck = @"Cross Server Login Check";
        public static string ReportSummary_CrossServerLoginCheck = @"Show all SQL Servers where a selected user has access.";

        public const string ReportTitle_Filters = @"Data Collection Filters";
        public static string ReportSummary_Filters = @"Show the data collection filters for all SQL Server instances.";

        public const string ReportTitle_ActivityHistory = @"Activity History";
        public static string ReportSummary_ActivityHistory = @"Show all SQLsecure activity history.";

        public const string ReportTitle_Users = @"SQLsecure Users";
        public static string ReportSummary_Users = @"Show all SQLsecure users.";

        // Entitlement Reports
        public const string ReportTitle_SuspectWindowsAccounts = @"Suspect Windows Accounts";
        public static string ReportSummary_SuspectWindowsAccounts = @"Show all the unresolved Windows Accounts that have Server Logins.";

        public const string ReportTitle_SuspectSqlLogins = @"Suspect SQL Logins";
        public static string ReportSummary_SuspectSqlLogins = @"Show all SQL server logins that do not have permissions.";
        
        public const string ReportTitle_ServerLoginsAndUserMappings = @"Server Logins and User Mappings";
        public static string ReportSummary_ServerLoginsAndUserMappings = @"Show all Server Logins and associated Database User Mappings for each SQL Server instance being audited.";

        public const string ReportTitle_UsersPermissions = @"User Permissions";
        public static string ReportSummary_UsersPermissions = @"Show permissions for a user across all servers.";

        public const string ReportTitle_DatabaseRoles = @"Database Roles";
        public static string ReportSummary_DatabaseRoles = @"Show all direct members of Database Roles on all SQL Servers.";

        public const string ReportTitle_ServerRoles = @"Server Roles";
        public static string ReportSummary_ServerRoles = @"Show all direct members of Server Roles on all SQL Servers.";

        public const string ReportTitle_AllObjectsWithPermissions = @"All User Permissions";
        public static string ReportSummary_AllObjectsWithPermissions = @"Show all objects with permissions in databases for all servers.";

        // Vulnerability Reports
        public const string ReportTitle_GuestEnabledDatabases = @"Guest Enabled Databases";
        public static string ReportSummary_GuestEnabledDatabases = @"Show all databases on a SQL Server instance where the Guest user has access.";

        public const string ReportTitle_MixedModeAuthentication = @"Mixed Mode Authentication";
        public static string ReportSummary_MixedModeAuthentication = @"Show all SQL Server instances where Windows Authentication is not the only login method.";

        public const string ReportTitle_SystemAdministratorVulnerability = @"System Administrator Vulnerability";
        public static string ReportSummary_SystemAdministratorVulnerability = @"Show all SQL Server instances that include Built-in Administrators as members of the sysadmin role.";

        public const string ReportTitle_VulnerableFixedRoles = @"Vulnerable Fixed Roles";
        public static string ReportSummary_VulnerableFixedRoles = @"Show all SQL Server instances that contain fixed roles assigned to public or guest.";

        public const string ReportTitle_ServersWithDangerousGroups = @"Dangerous Windows Groups";
        public static string ReportSummary_ServersWithDangerousGroups = @"Show all SQL Server instances that grant access to any OS controlled Windows Group.";

        public const string ReportTitle_DatabaseChaining = @"Database Chaining Enabled";
        public static string ReportSummary_DatabaseChaining = @"Show all SQL Server instances that have cross-database ownership chaining enabled.";

        public const string ReportTitle_CMDShellVulnerability = @"CMD Shell Vulnerability";
        public static string ReportSummary_CMDShellVulnerability = @"Show all SQL Server instances that have xp_cmdshell extended stored procedures available.";

        public const string ReportTitle_MailVulnerability = @"Mail Vulnerability";
        public static string ReportSummary_MailVulnerability = @"Show all SQL Server instances with SQL Mail stored procedures.";

        public const string ReportTitle_LoginVulnerability = @"Login Vulnerability";
        public static string ReportSummary_LoginVulnerability = @"Show all SQL Server instance whose SQL logins have weak passwords.";

        public const string ReportTitle_RiskAssessment = @"Risk Assessment";
        public static string ReportSummary_RiskAssessment = @"Show all policy and risk assessment results.";

        public const string ReportTitle_OSVulnerability = @"OS Vulnerability via XSPs";
        public static string ReportSummary_OSVulnerability = @"Show all extended stored procedures that grant non-Administrator users permission to access operating system functions.";

        // Comparison Reports
        public const string ReportTitle_CompareAssessments = @"Assessment Comparison";
        public static string ReportSummary_CompareAssessments = @"Identify discrepancies in the findings and security check configurations of two different asssessments.";

        public const string ReportTitle_CompareSnapshots = @"Snapshot Comparison";
        public static string ReportSummary_CompareSnapshots = @"Identify discrepancies in the audit data collected by two different snapshots.";



        // Reporting Services
        public static string ReportTitle_ReportingServices = PRODUCT_STR + @" reports pack is also available for Microsoft Reporting Services.";
        public static string ReportSummary_ReportingServices = @"You can create your own reports and use the more advanced features for execution, scheduling and report generation that are available with Microsoft Reporting Services to create the reporting environment that suits your needs.";

        public const string ReportSelect_AllServers = @"All servers in policy";
        public const string ReportSelect_LoginTypes_All = "All Types";
        public const string ReportSelect_LoginTypes_AllWindows = "All Windows Accounts";
        public const string ReportSelect_LoginTypes_WindowsUsers = "Windows Users";
        public const string ReportSelect_LoginTypes_WindowsGroup = "Windows Groups";
        public const string ReportSelect_LoginTypes_SQLLogins = "SQL Logins";

        #endregion

        #region Policies

        public const string Policy_Install_Folder = "Policies";

        #endregion

        #region Password Validation Constants

        public const int MINIMUM_PASSWORD_LENGTH = 6;
        public const string PASSWORD_LENGTH_MESSAGE_FORMAT = @"SQLSecure application requires password length not less {0} characters to save it properly into database!";

        #endregion

        #region Import/Export Policies Constants

        public const string TITLE_POLICY = "Policy";
        public const string TITLE_ASSESSMENT = "Assessment";
        public const string EXPORTING = "Exporting";
        public const string IMPORTING = "Importing";
        public const string EXPORT_COLUMN_TEXT = "Export";
        public const string IMPORT_COLUMN_TEXT = "Import";
        public const string IMPORTING_EXPORTING_DESCRIPTION_FORMAT = "Check security checks in {0} column for {1} operation.";
        public const string IMPORTING_EXPORTING_FORM_TITLE_FORMAT = "{0} {1} Security Checks - {2}";

        #endregion
    }

    #region Policies

    public class Policy
    {
        public enum Severity
        {
            [Description("Unknown")]
            Undetermined = -1,
            [Description("OK")]
            Ok = 0,
            Low = 1,
            Medium = 2,
            High = 3
        }

        public enum SeverityExplained
        {
            [Description("Unknown")]
            Undetermined = -1,
            [Description("OK")]
            Ok = 0,
            [Description("OK, Explained Low")]
            LowExplained = 1,
            [Description("OK, Explained Medium")]
            MediumExplained = 2,
            [Description("OK, Explained High")]
            HighExplained = 3,
            Low = 11,
            Medium = 12,
            High = 13
        }

        public static class AssessmentState
        {
            [Description("Policy Settings")]
            public const String Settings = @"S";
            [Description("Current Values")]
            public const String Current = @"C";
            [Description("Draft Assessments")]
            public const String Draft = @"D";
            [Description("Published Assessments")]
            public const String Published = @"P";
            [Description("Approved Assessments")]
            public const String Approved = @"A";
            public const String Unknown = @"U";

            public static string DisplayName(string state)
            {
                switch (state)
                {
                    case AssessmentState.Settings:
                        return DescriptionHelper.GetDescription(typeof(AssessmentState), "Settings");
                    case AssessmentState.Current:
                        return DescriptionHelper.GetDescription(typeof(AssessmentState), "Current");
                    case AssessmentState.Draft:
                        return DescriptionHelper.GetDescription(typeof(AssessmentState), "Draft");
                    case AssessmentState.Published:
                        return DescriptionHelper.GetDescription(typeof(AssessmentState), "Published");
                    case AssessmentState.Approved:
                        return DescriptionHelper.GetDescription(typeof(AssessmentState), "Approved");
                }
                return DescriptionHelper.GetDescription(typeof(AssessmentState), "Unknown");
            }

            public static string StateName(string state)
            {
                switch (state)
                {
                    case AssessmentState.Settings:
                        return "Settings";
                    case AssessmentState.Current:
                        return "Current";
                    case AssessmentState.Draft:
                        return "Draft";
                    case AssessmentState.Published:
                        return "Published";
                    case AssessmentState.Approved:
                        return "Approved";
                }
                return "Unknown";
            }
        }

        public enum CopyType
        {
            CopyAll = 1,
            CopySettings = 2,
            Refresh = 1
        }
    }

    #endregion

    #region Snapshots

    public class Snapshot
    {
        public const String StatusSuccessful = @"S";
        public const String StatusWarning = @"W";
        public const String StatusError = @"E";
        public const String StatusInProgress = @"I";

        public const String StatusBaselineText = @"Successful (Baseline)";
        public const String StatusSuccessfulText = @"Successful";
        public const String StatusWarningText = @"Warnings";
        public const String StatusErrorText = @"Errors";
        public const String StatusInProgressText = @"In Progress";
        public const String StatusUnknownText = @"Unknown";

        public const String AutomatedTrue = @"Y";
        public const String AutomatedFalse = @"N";

        public const String BaselineTrue = @"Y";
        public const String BaselineFalse = @"N";

        public const String ToolTipDate = @"The date the Snapshot was created";
        public const String ToolTipTime = @"The time the Snapshot was created";
        public const String ToolTipAutomated = @"Snapshot was created automatically via a schedule";
        public const String ToolTipStatus = @"The status of the Snapshot";
        public const String ToolTipComment = @"Snapshot comments";
        public const String ToolTipBaseline = @"Snapshot has been marked as a Baseline Snapshot";
        public const String ToolTipBaselineComment = @"Baseline comments";
        public const String ToolTipObjects = @"The number of objects captured in this Snapshot";
        public const String ToolTipPermissions = @"The number of permissions captured in this Snapshot";
        public const String ToolTipLogins = @"The number of Logins captured in this Snapshot";
        public const String ToolTipGroupMembers = @"The number of Windows Accounts captured in this Snapshot";

        public const String MenuDelete = @"Delete Snapshot";
        public const String MenuDeleteMultiple = MenuDelete + @"s";
    }

    #endregion

    #region Activity

    public class Activity
    {
        public const String TypeInfo = @"Information";
        public const String TypeWarning = @"Warning";
        public const String TypeError = @"Error";
        public const string TypeAuditSuccess = "Success Audit";
        public const string TypeAuditFailure = "Failure Audit";

        public const String TypeInfoText = @"Information";
        public const String TypeWarningText = @"Warning";
        public const String TypeErrorText = @"Error";
        public const string TypeAuditSuccessText = @"Success Audit";
        public const string TypeAuditFailureText = @"Failure Audit";
        public const String TypeUnknownText = @"Unknown";

        public const String ToolTipType = @"The type of Activity entry";
        public const String ToolTipDate = @"The date the Activity occurred";
        public const String ToolTipTime = @"The time the Activity occurred";
        public static String ToolTipSource = @"The " + Constants.PRODUCT_STR + " component that performed the action";
        public const String ToolTipLogin = @"The user that performed the action";
        public const String ToolTipCode = @"The event code for the Activity";
        public const String ToolTipCategory = @"The category of the Activity";
        public const String ToolTipDescription = @"The description of the Activity";

        public const string TypeServerOnPremise = @"On-Premise SQL Server";
        public const string TypeServerAzureVM = @"SQL Server on Azure Virtual Machine";
        public const string TypeServerAzureDB = @"Azure SQL Database";
    }

    #endregion

    #region User Credentials

    public class LoginCredentials
    {
        public const String TypeWindows = @"W";
        public const String TypeSQLServer = @"S";
    }

    public class Logins
    {
        // Login lables
        public const string Login_Role = "Role";
        public const string Login_WindowsUser = "Windows User";
        public const string Login_WindowsGroup = "Windows Group";
        public const string Login_SQLLogin = "SQL Login";
        public const string Login_Administrator = "SQLsecure manager Administrator";
        public const string Login_Auditor = "SQLsecure manager Auditor";
        public const string Login_Deny = "Deny";
        public const string Login_Permit = "Grant";
        public const string Login_ViaGroup = "Via group membership";
        public const string Login_CanConfigure = "Can configure settings and view audit data";
        public const string Login_CanView = "Can view audit data";
        public const string Login_None = "None";
    }

    #endregion

    #region Permissions

    public class Permissions
    {
        public class Type
        {
            public const String Effective = @"E";
            public const String Explicit = @"X";
        }

        public class Level
        {
            public const String Database = @"DB";
            public const String Column = @"COL";
            public const String Object = @"OBJ";
            public const String Schema = @"SCH";
            public const String Server = @"SV";
        }

        public class Aliased
        {
            public const String True = @"Y";
            public const String False = @"N";
        }

        public class LoginDisabled
        {
            public const String True = @"Y";
            public const String False = @"N";
        }

        public class Grants
        {
            public const String True = @"Y";
            public const String False = @"N";
        }
    }

    #endregion

    #region Object Explorer

    public static class ObjectExplorerConstants
    {
        public enum ObjType
        {
            Snapshot = 0,
            Server,
            ServerSecurity,
            Logins,
            WindowsUserLogin,
            WindowsGroupLogin,
            SqlLogin,
            ServerRoles,
            ServerRole,
            ServerObjects,
            Endpoints,
            Endpoint,
            Databases,
            Database,
            DatabaseSecurity,
            Users,
            User,
            Roles,
            Role,
            Schemas,
            Schema,
            Keys,
            Key,
            Certificates,
            Certificate,
            Tables,
            Table,
            Views,
            View,
            Synonyms,
            Synonym,
            StoredProcedures,
            StoredProcedure,
            Functions,
            Function,
            ExtendedStoredProcedures,
            ExtendedStoredProcedure,
            Assemblies,
            Assembly,
            UserDefinedDataTypes,
            UserDefinedDataType,
            XMLSchemaCollections,
            XMLSchemaCollection,
            FullTextCatalogs,
            FullTextCatalog,

            Unknown
        }
    }

    public static class OsObjectType
    {
        [Description("Database File")]
        public const String DB = @"DB";
        public const String Disk = @"Disk";

        [Description("Data Directory")]
        public const String FileDirectory = @"FDir";
        public const String File = @"File";

        [Description("Install Directory")]
        public const String InstallDirectory = @"IDir";

        [Description("Registry Key")]
        public const String RegistryKey = @"Reg";
        public const String Unknown = @"Unk";
    }

    #endregion

    #region SQL Command Timeout

    public class SQLCommandTimeout
    {
        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Utility.SQLCommandTimeout");
        private const string SQLTimeoutReg = "SQL Command Timeout";

        private static int errorCount = 0;
        private const int ERROR_COUNT_LIMIT = 3;

        public static int GetSQLCommandTimeoutFromRegistry()
        {
            int timeout = 180;
            string strTimeout = null;
            RegistryKey hkcu;
            RegistryKey hkSoftware = null;
            RegistryKey hkIdera = null;
            RegistryKey hkSQLsecure = null;

            if (errorCount < ERROR_COUNT_LIMIT)
            {
                try
                {
                    hkcu = Registry.CurrentUser;
                    hkSoftware = hkcu.OpenSubKey("SOFTWARE");
                    hkIdera = hkSoftware.OpenSubKey("Idera", true);

                    if (hkIdera != null)
                    {
                        hkSQLsecure = hkIdera.OpenSubKey("SQLsecure", true);
                        if (hkSQLsecure != null)
                        {
                            strTimeout = (string) hkSQLsecure.GetValue(SQLTimeoutReg);
                        }
                    }

                    if (string.IsNullOrEmpty(strTimeout))
                    {
                        WriteDefaultSQLCommandTimeout(timeout);
                    }
                    else
                    {
                        timeout = Convert.ToInt32(strTimeout);
                        errorCount = 0;
                    }
                }
                catch (Exception ex)
                {
                    logX.loggerX.Error("Error Reading SQL Command from Registry: ", ex.Message);
                    errorCount++;
                }
                finally
                {
                    if (hkSoftware != null) hkSoftware.Close();
                    if (hkIdera != null) hkIdera.Close();
                    if (hkSQLsecure != null) hkSQLsecure.Close();
                }
            }
            return timeout;
        }

        private static void WriteDefaultSQLCommandTimeout(int defaultTimeout)
        {
            RegistryKey hkcu;
            RegistryKey hkSoftware = null;
            RegistryKey hkIdera = null;
            RegistryKey hkSQLsecure = null;

            try
            {
                hkcu = Registry.CurrentUser;
                hkSoftware = hkcu.OpenSubKey("SOFTWARE", true);
                hkIdera = hkSoftware.OpenSubKey("Idera", true);

                if (hkIdera == null) hkIdera = hkSoftware.CreateSubKey("Idera");

                hkSQLsecure = hkIdera.OpenSubKey("SQLsecure", true);

                if (hkSQLsecure == null) hkSQLsecure = hkIdera.CreateSubKey("SQLsecure");

                hkSQLsecure.SetValue(SQLTimeoutReg, defaultTimeout.ToString(), RegistryValueKind.String);
            }
            catch (Exception ex)
            {
                logX.loggerX.Error("Error Writing SQL Command to Registry: ", ex.Message);
                errorCount++;
            }
            finally
            {
                if (hkSoftware != null) hkSoftware.Close();
                if (hkIdera != null) hkIdera.Close();
                if (hkSQLsecure != null) hkSQLsecure.Close();
            }
        }
    }

    #endregion

    public enum ServerType
    {
        OnPremise,//On-Premise
        AzureSQLDatabase,//Azure SqlDatabase
        SQLServerOnAzureVM//Azure VM
    }
}
