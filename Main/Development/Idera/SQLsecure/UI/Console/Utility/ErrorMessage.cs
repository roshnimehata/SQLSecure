/******************************************************************
 * Name: ErrorMessage.cs
 *
 * Description: Error message display helper functions.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006, 2007 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using System.Diagnostics;

namespace Idera.SQLsecure.UI.Console.Utility
{
    internal static class ErrorMsgs
    {
        public const String ErrorLabel = @"\n\nError:\n\n";
        public const String ErrorStub = @"Error - {0}";

        public const String CantLoadHelpFile = @"Unable to display the SQLsecure help file.";
        public const String InvalidArguments = @"Failed to parse the arguments";


        public const String CantConnectRepository = @"Connect to Repository";
        public const String FailedToConnectMsg = "Failed to connect to the specified Repository.   Check the Repository name, and verify that the Repository SQL Server is running.";
        public static String UserHasNoPermission = @"You do not have permission to access the " + Utility.Constants.PRODUCT_STR + " Repository on the selected SQL Server.\nPlease select a different Repository.";
        public const String CantConnectServer = @"Unable to connect to SQL Server";
        public const String CantGetProviders = @"Unable to retrieve Notification Providers from the Repository";
        public const String CantGetProvider = @"Unable to retrieve Notification Provider from the Repository.";
        public const String CantGetPolicies = @"Unable to retrieve Policy Assessments from the Repository";
        public const String CantGetAssessments = @"Unable to retrieve Policy Assessments from the Repository";
        public const String CantGetAssessmentComparison = @"Unable to retrieve Policy Assessment Comparison from the Repository";
        public const String CantValidateAssessmentData = @"Unable to validate Policy Assessment data";
        public const String CantGetPolicy = @"Unable to retrieve Policy from the Repository. If this policy has been deleted, refresh the Policy tree to get a current list.";
        public const String CantSavePolicy = @"Unable to save Policy to the Repository.";
        public const String CantUpdatePolicy = @"Unable to update Policy in the Repository.";
        public const String CantGetAssessmentNotes = @"Unable to retrieve Policy Assessment Notes from the Repository";
        public const String CantGetRegisteredServers = @"Unable to retrieve Registered Servers from the Repository";
        public const String CantGetRegisteredServer = @"Unable to retrieve Registered Server from the Repository. If this server has been deleted, refresh the Audited SQL Servers tree to get a current list.";
        public const String CantGetSnapshot = @"Unable to retrieve Snapshot from the Repository";
        public const string CantGetSnapshotFilters = @"Unable to retrieve Snapshot filters from the Repository";
        public const String CantGetSnapshots = @"Unable to retrieve Snapshots from the Repository";
        public const String CantSelectServers = @"Error retrieving SQL Servers";
        public const String CantGetWellKnownGroups = @"Unable to retrieve Windows Well-known Groups list from the Repository";
        public const String CantGetProtocols = @"Unable to retrieve Protocol list from the Repository";
        public const String CantValidateRepository = @"Error validating Repository";
        public const String CantWriteRepositoryToRegistry = @"Unable to write default Repository to Registry";
        public const String DalNotSupported = @"The selected Repository on {0} does not support Data Access Layer {1} which is required by this version of the console. Choose a different Repository or make sure the console and Repository versions are compatible.";
        public const String SQLServerVersionNotSupported = @"The selected SQL Server is not at a supported version";
        public const String RepositoryNotFound = @"Repository not found";
        public static String RepositoryDBNotFound = @"A " + Constants.PRODUCT_STR + " database was not found on the selected server";
        public const String SMOError = @"Exception raised using SMO to retrieve Server List";
        public const String VersionNotSupported = @"Version not supported";

        // Add server wizard error messages.
        public const string RegisterSqlServerCaption = "Register SQL Server";

        public const string RegisteredServerCaption = "Registered Server";
        public const string RegisterSqlServerSuccessPromptToRunJob = "SQL Server {0} was successfully registered.  However, you will not be able to view security metrics or permissions until data has been collected.\n\r\n\rWould you like to collect data now?";
        public const string RegisterSqlServerCantFindServer = "SQL Server < {0} > was not found, make sure the SQL Server name is valid and the SQL Server is running.";
        public const string ServerRegistrationCheckFailedMsg = @"Error was encountered when checking if the server was already registered.";
        public const string ServerAlreadyRegisteredMsg = @"The SQL Server instance is already registered for auditing.";
        public const string ServerVersionNotSupportedMsg = @"The SQL Server version is not supported for auditing.   SQLsecure only supports auditing SQL Server 2000 through SQL Server 2014.";
        public const string RetrieveServerPropertiesFailedMsg = @"Error was encountered when retrieving SQL Server properties for registration.";
        public const string SqlLoginNotSpecifiedMsg = @"SQL login credentials have not been entered.";
        public const string SqlLoginFailureNoCredentialsMsg = @"Login to SQL Server failed, please specify SQL login credentials.";
        public const string SqlLoginFailureMsg = @"Login to SQL Server failed using the specified SQL login credentials.";
        public const string TargetServerLogonFailureMsg = @"Login to Target Server failed using the specified credentials.";
        public const string SqlLoginWindowsUserNotSpecifiedMsg = @"SQL Server Windows authentication login has been entered incorrectly.  Make sure that the user is specified in domain\username format and a password has been entered.";
        public const string SqlLoginWindowsUserPasswordNoMatchMsg = @"The passwords entered for the Windows user, for SQL Server login, do not match.";
        public const string WindowsUserNotSpecifiedMsg = @"Windows user, for gathering OS and AD objects, has been entered incorrectly.  Make sure that the user is specified in domain\username format and a password has been entered.";
        public const string WindowsUserPasswordNoMatchMsg = @"The passwords entered for the Windows user, for gathering OS and AD objects, do not match.";
        public const string DbNameMatchNotSpecifiedMsg = @"Database name match string is not specified.";
        public const string TableNameMatchNotSpecifiedMsg = @"Table name match string is not specified.";
        public const string SPNameMatchNotSpecifiedMsg = @"Stored procedure name match string is not specified.";
        public const string DbNameMatchInvalidCharsMsg = @"Database name match string contains invalid characters.";
        public const string TableNameMatchInvalidCharsMsg = @"Table name match string contains invalid characters.";
        public const string SPNameMatchInvalidCharsMsg = @"Stored procedure name match string contains invalid characters.";
        public const string AddServerToRepositoryFailedMsg = @"Error was encountered when registering the server in the SQLsecure Repository.";
        public const string AddRuleToRepositoryFailedMsg = @"Error was encountered when adding snapshot rules in the SQLsecure Repository.";
        public const string RegisterSqlServerSuccessful = @" has been registered with SQLsecure for auditing";
        public const string AddJobToRepositoryFailedMsg = @"Error was encountered when adding audit job to SQL Server";
        public const string RegisterSqlServerNoLicenseMsg = "Additional license required to add a new server." +
                                                            "\n\nPlease contact Idera to acquire a license for more servers or remove an existing server.";
        public const string NameMatchInvalidCharsMsg = @"Name match string contains invalid characters.";
        public const string WarningEmailNoConfiguredTitle = @"Email Configuration";
        public const string WarningEmailNoConfiguredMsg = @"To successfully receive email notifications, you must also specify which email provider SQLsecure should use. Click Configure SMTP Email on the Tools menu.";

        // Policy
        public const string PolicyCaption = "Policy";
        public const string PolicyPropertiesCaption = "Policy Properties";
        public const string RemovePolicyCaption = "Remove Policy";
        public const string RemovePolicyConfirmMsg = @"When you remove a policy, all policy security check settings and internal review notes information will also be deleted. Continue removing Policy?";
        public const string RemoveSystemPolicyMsg = "This policy is a system policy and cannot be deleted.";
        public const string ConfigurePolicyCaption = "Configure Policy";
        public const string ConfigureDynamicPolicyMsg = "This policy has a dynamic server list and the properties cannot be changed.";
        public const string RemoveDynamicPolicyServerMsg = "This policy has a dynamic server list and servers cannot be removed manually.";
        public const string CantGetPolicyInfoMsg = "Unable to retrieve {0} from the repository.";
        public const string CantRemovePolicyMsg = "Unable to remove policy {0} from the repository.";
        public const string ErrorProcessPolicyInfo = "Error processing policy information";
        public const string PolicyNotRegistered = "Policy no longer exists.";
        public const string RemoveServerFromPolicyCaption = "Remove server from policy";
        public const string RemoveServerFromPolicyConfirmMsg = "You are about to remove server {0} from policy {1}.  Continue removing server?";
        public const string AssessmentNotFound = "Assessment no longer exists.";
        public static string RemoveAssessmentCaption = @"Remove Assessment";
        public const string RemoveAssessmentConfirmMsg = "You are removing assessment {0}. All security check settings and notes associated with this assessment will be deleted.\n\nDo you want to remove this assessment?";
        public static string RemoveServerFromAssessmentCaption = RemoveServerFromPolicyCaption.Replace("policy", "assessment");
        public static string RemoveServerFromAssessmentConfirmMsg = RemoveServerFromPolicyConfirmMsg.Replace("policy", "assessment");
        public static string RemoveDynamicAssessmentServerMsg = RemoveDynamicPolicyServerMsg.Replace("policy", "assessment");
        public const string RemoveApprovedAssessmentMsg = "This assessment is an Approved assessment and cannot be deleted.";
        public const string CantRemoveAssessmentMsg = "Unable to remove assessment {0} from the repository.";
        public const string PublishAssessmentCaption = "Publish Assessment";
        public const string PublishAssessmentMsg = "You are publishing draft assessment {0}. Published assessments can be distributed for review and all additional changes will now be tracked.\n\nDo you want to publish this assessment?";
        public const string ApproveAssessmentCaption = "Approve Assessment";
        public const string ApproveAssessmentMsg = "You are approving published assessment {0}. Approved assessments are permanent records which cannot be deleted or changed.\n\nDo you want to approve this assessment?";
        public const string NoPolicyServersMsg = "There are currently no servers selected for this policy. An assessment cannot be performed until a server exists for the policy. Do you want to continue with no servers?";
        public const string NoPolicyMetricsMsg = "There are currently no security checks selected for this policy. An assessment cannot be performed until a security check is enabled for the policy. Do you want to continue with no security checks?";

        // SQL Server Properties.
        public const string SqlServerPropertiesCaption = "SQL Server Properties";
        public const string CredentialsInvalidMsg = "The SQL Server and Windows credentials read from the Repository are invalid.";
        public const string NoCredentialsSpecifiedMsg = "Specified credentials was selected, but SQL Server or Windows credentials have not been specified.";
        public const string UpdateCredentialsFailedMsg = "Error was encountered when updating registered server credentials";
        public const string SaveChangesBeforeCancelMsg = "Do you wish to save the changes made to the registered SQL Server configuration?";
        public const string ConfirmFilterRuleDeleteMsg = "Do you wish to delete selected filters?";
        public const string UpdateRetentionPeriodFailedMsg = "Error was encountered when updating registered server retention period";


        //Add Edit Audit Folders
        public const string AddEditFolderPathCaption = "{0} Audit Folder Path";
        public const string AadNewCaption = "Add New";
        public const string EditCaption = "Edit";
        public const string FolderExistsCaption = "Folder Exists";
        public const string FolderExistsMsg = "The '{0}' folder is already in the list!";
        public const string DeleteAuditFolderCaption = "Deleting Audit Folder";
        public const string ConfirmAuditFolderPathDeleteMsg = "Do you wish to delete '{0}' folder?";
        public const string FolderPathNotValidCaption = "Folder Path Is Not Valid";
        public const string FolderPathNotValidMsg = "Please specify valid a mapped drive folder or a folder in UNC format.";
        public const string FolderPathMissingCaption = "Folder Path Missing";
        public const string FolderPathMissingMsg = "Folder Path Missing";
        public const string UpdateAuditFoldersFailedCaption = "Failed Updating Audit Folder";
        public const string UpdateAuditFoldersFailedMsg = "Error was encountered when updating registered server audit folders";

        // Filter properties.
        public const string FilterPropertiesCaption = "Filter Properties";
        public const string FilterSaveChangesBeforeCancelMsg = "Do you wish to save the changes made to the filter?";
        public const string FilterNoNameSpecificedMsg = "No filter name is specified.";
        public const string FilterNoDbSpecifiedMsg = "No database has been selected.";
        public const string FilterNoServerObjectSelectedMsg = "No server level object has been selected.";

        // New Filter Rule
        public const string NewFilterRuleCaption = "New Filter Rule";
        public const string FilterRuleAlreadyExistsMsg = "Filter rule with this name already exists for this SQL Server.   Enter a different filter rule name.";

        // Remove server.
        public const string RemoveSqlServerCaption = "Remove SQL Server";
        public const string RemoveSQLServerConfirmMsg = @"Warning: When you remove a registered SQL Server, you stop collecting permissions data for the server.  Any permissions data that has been collected for the server will also be deleted.

Do you wish to remove SQL Server now?";
        public const string RemoveSqlServerFailedMsg = "Error was encountered when removing the SQL Server from SQLsecure.";
        public const string RemoveSqlServerSuccessful = @" has been removed.";

        // Snapshot baseline
        public const string BaselineSnapshotCaption = "Baseline Snapshot";
        public const string BaselineSnapshotInProgressMsg = "A Snapshot that is In Progress, cannot be baselined.";
        public const string BaselineSnapshotNoSelectionMsg = "No Snapshots were selected for baselining.";
        public const string BaselineSnapshotError = "Error baselining snapshot";


        // Snapshot delete
        public const string DeleteSnapshotCaption = "Delete Snapshot";
        public const string DeleteSnapshotInProgressMsg = "A Snapshot that is In Progress, cannot be deleted.";
        public const string DeleteSnapshotNoSelectionMsg = "No Snapshots were selected for deletion.";

        // SQLsecure Activity
        public const string ActivityCaption = "SQLsecure Activity";
        public const string CantGetActivityMsg = "Unable to retrieve the SQLsecure activity records.";

        // SQLsecure Logins
        public const string CantGetLoginsCaption = "SQLsecure Logins";
        public const string CantGetLoginsMsg = "Unable to retrieve the SQLsecure logins";
        public static string DeleteLoginCaption = "Delete Login";
        public static string DeleteLoginWarningMsg = "Warning: This will delete the SQL Server login, {0}, and all associated database users (if any) and their access within SQL Server.\n\nAre you sure you want to remove this login?";
        public static string AddLoginCaption = "Add Login";
        public static string AddLoginErrorMsg = "Error adding the specified login {0}.";
        public static string AddLoginErrorInvalidUser = "{0} is not recognized as a valid Windows user or group name."
                                                        + "\nUse the \"domain\\username” syntax when specifying Windows accounts."
                                                        + "\nNote that Windows account name may be case-sensitive.";
        public static string AddLoginWarningMsg = "Warning: You are creating a login with the following permission settings:\n\n\t{0}\n\t{1}\n\nIf a login named {2} already exists, SQLsecure will change the permissions for that login.\n\nDo you want to continue?";
        public static string SaveLoginCaption = "Update Login";
        public static string SaveLoginWarningMsg = "Warning: You are changing the following permission settings:\n\n\t{0}\n\t{1}\n\nDo you want to continue?";
        public static string LoginGrantAccessMsg = "• Will have access to the SQL Server instance that hosts the Repository";
        public static string LoginRevokeAccessMsg = "• Will not have access to the SQL Server instance that hosts the Repository";
        public static string LoginSysadminAccessMsg = "• Will belong to the sysadmin fixed server role";
        public static string LoginNotSysadminAccessMsg = "• Will not belong to the sysadmin fixed server role";

        // SQLsecure License
        public const string LicenseCaption = "SQLsecure License";
        public const string LicenseInvalid = "License {0} is invalid.";
        public const string CantAddTrialToPermamentLicense = "Can't add a trial license to a production license.";
        public const string LicenseInvalidRepository = "This license is invalid for current repository {0}.";
        public const string LicenseInvalidProductID = "This license is invalid for SQLsecure.";
        public const string LicenseInvalidProductVersion = "This license is invalid for this version of SQLsecure.";
        public const string LicenseExpired = "This license has expired.";
        public const string LicenseInvalidDuplicate = "This license has already been registered.";
        public const string LicenseExpiring = "The license {0} will expire in {1} days.";
        public const string LicenseNoValidLicense = "SQLsecure is not licensed. You must have a valid license to run SQLsecure."
                                                  + "\n\nPlease contact Idera to obtain a valid license for SQLsecure.";
        public const string LicenseTooManyRegisteredServers = "The number of registered servers is more than the available licenses."
                                                      + "\nNo more audited data will be collected until more licenses are added or a registered server is deleted."
                                                      + "\n\nPlease contact Idera to obtain more license for SQLsecure or remove a registered server.";
        public const string LicenseInterestText = "Thank you for your interest in SQLsecure.";
        public const string LicenseEnterProductionText = "Please enter your production license";
        public const string LicenseTrialExpiredText = "SQLsecure requires a valid license. The trial license has expired, please enter your production license.";
        public const string LicenseUnsupportedConfigText = "This configuration requires a production license. To use a Trial license you must run the Console on the same server as the repository.";
        public const string DeleteLicenseCaption = "Delete SQLsecure License";
        public const string DeleteConfirmMsg = "Do you wish to delete the SQLsecure license?";

        // Schedule & Job
        public const string ScheduleCaption = "Job Schedule";
        public const string ScheduleInvalidNoWeekdaySelected = "Weekly schedule requires at least day to be selected";
        public const string JobSucceededMsg = "Snapshot collection has finished successfully.";
        public const string JobSucceededWarningsMsg = "Snapshot created successfully, but Warnings occurred.";
        public const string JobFailedMsg = "Snapshot collection has failed. \r\nSee your activity log (in the Manage SQLsecure view) for details.";
        public const string JobRunningMsg = "Snapshot collection has started successfully.";
        public const string ScheduleNotAvaliableForNonAdmin = "Data collection schedule information is only available when running as a SQLsecure administrator.";
        public const string ScheduleCanNotBeReadByNonAdmin = "Schedule can't be retrived from Repository by non-admin users";

        // Permissions
        public const string ServerNoSnapshots = @"No audit data available";
        public const string ServerMissingSnapshot = @"Unable to retrieve selected Snapshot";
        public const string NoUserPermissionsShown = @"Select a User and Database and click Show Permissions to view permissions";
        public const string NoRolePermissionsShown = @"Select a Database and Role and click Show Permissions to view permissions";
        public const string CantGetUserPermissionsCaption = "Explore User Permissions";
        public const string CantGetUserPermissionsMsg = "Unable to retrieve the requested permissions for user.";
        public const string CantGetRolePermissionsCaption = "Explore Role Permissions";
        public const string CantGetRolePermissionsMsg = "Unable to retrieve the requested permissions for role.";
        public const string CantGetObjectPermissionsCaption = "Explore Object Permissions";
        public const string CantGetObjectPermissionsMsg = "Unable to retrieve the requested object permissions.";
        public const string HasSaPermissionsMsg = "User sa has all permissions.";
        public const string HasSysadminPermissionsMsg = "User has sysadmin privileges.";
        public const string HasWellKnownGroupsMsg = "Permissions may be missing because Well-known groups are present. See Snapshot Properties for more info.";
        public const string HasMissingUsersMsg = "Snapshot may be missing Windows Accounts or permissions for Windows Users. See Snapshot Properties for more info.";

        // SQLsecure Data collection
        public const string SQLsecureDataCollection = "SQLsecure Snapshot Collection";
        public const string CantRunDataCollection = "Error was encountered when running data collection";
        public const string JobAlreadyRunning = "A snapshot collection is already in progress. Please wait until the current snapshot has completed before starting a new collection.";
        public const string SQLServerAgentNotStarted = "The SQL Server Agent is not started on the instance hosting the SQLsecure Repository.\nSQLsecure uses this agent for data collection and grooming.\n\nPlease start the SQL Server Agent and retry.";
        public const string SQLServerNoJobFound = "Error: No collection job configured for this server";
        public const string DataCollectionErrorGettingJobStatus = "Error getting Data Collector job status";
        public const string DataCollectionErrorGettingAgentStatus = "Error getting SQL Server Agent status";
        public const string DataCollectionErrorAddingGroomJob = "Error adding grooming job";
        public const string DataCollectionErrorGettingGroomJob = "Error getting grooming job";
        public const string DataCollectionErrorGettingJob = "Error getting data collector job";
        public const string DataCollectionErrorRemovingJob = "Error removing data collector job";
        public const string DataCollectionErrorAddingJob = "Error adding data collector job";
        public const string SQLServerNoJobFoundCreateYesNo = "Error: No collection job is configured for this server."
                                                           + "\n\nWould you like to create the collection job with scheduling disabled?";
        public const string SQLServerNoJobFoundCreateWarning = "Warning: No collection job is configured for this server."
                                                            + "\nThe collection job was created with scheduling disabled.";
        public const string SQLServerAgentStarted = "Started";
        public const string SQLServerAgentStopped = "Stopped";
        public const string SQLServerAgentUnknown = "Status unknown";

        // Select Databases
        public const string CantGetDatabasesCaption = "Select Database";
        public const string CantGetDatabasesMsg = "Unable to list Databases for the selected Snapshot.";
        public const string CantFindDatabaseMsg = "The Database '{0}' was not found in the selected Snapshot.";
        public const string PartialMatchDatabaseMsg = "Warning: The Database name you entered does not have an exact match on the SQL Server and has been matched as {0}.\n\nIs this the intended Database?";
        public const string DatabaseNotSelectedMsg = "You must enter a Database or choose 'Server Only' in order to Show Permissions.";
        public const string NoDatabaseMsg = "You must enter a Database in order to Show Permissions.";

        // User Permissions, Select Users & User validation
        public const string UserPermissionsCaption = "User Permissions";
        public const string ErrorProcessUserPermissions = "Error processing User Permissions";
        public const string GroupMembersCaption = "View Group Members";
        public const string OSPermissionsCaption = "View OS Permissions";
        public const string CantGetUsersCaption = "Select User";
        public const string CantGetUsersMsg = "Unable to list Users for the selected Snapshot.";
        public const string CantGetUserMsg = "Unable to retrieve User info from the selected Snapshot.";
        public const string UserNotFoundMsg = "The User was not found in the selected Snapshot.";
        public const string UserNotMatchedMsg = "No match found for User '{0}' . There are no permissions.";
        public const string UserNotMatchedSnapshotQuestion = "A different user with the same name was found in Active Directory.\nChoose Yes to continue with your original selection or choose No to use the Active Directory user.\n\nDo you wish to use your original selection?";
        public const string UserNotMatchedADQuestion = "A different user with the same name was found in this Snapshot.\nChoose Yes to continue with your original selection or choose No to use the Snapshot user.\n\nDo you wish to use your original selection?";
        public const string UserNameChangedSnapshotMsg = "Warning: This user's name has changed in Active Directory to '{0}'.\n\nProcessing will continue with the selected user, but please verify that this is the correct user.";
        public const string UserNameChangedADMsg = "Warning: This user's name in this snapshot is '{0}'.\n\nProcessing will continue with the selected user, but please verify that this is the correct user.";
        public const string UserHasNoPermissionMsg = "User has no permissions on the server.";
        public const string WindowsUserNotFoundDomainMsg = "The account '{0}' was not found in the Windows Domain, but an account with the same name has been found in the selected Snapshot.\n\nDo you wish to continue with this account?";
        public const string WindowsUserNotFoundMsg = "The User '{0}' was not found in the Windows Domain or in the selected Snapshot.";
        public const string WindowsUserNotMatchedMsg = "The User '{0}' was not found in a Windows Domain.";
        public const string NoUserSelectedMsg = "Select a User before attempting to Show Permissions.";

        // Role Permissions
        public const string RolePermissionsCaption = "Role Permissions";
        public const string ErrorProcessRolePermissions = "Error processing Role Permissions";
        public const string NoRoleSelectedMsg = "Select a Role before attempting to Show Permissions.";
        public const string RoleNotFoundMsg = "The Role '{0}' was not found in Database '{1}' in the selected Snapshot.";

        // Export to excel.
        public const string ExportToExcelCaption = "Save as Excel file";
        public const string FailedToExportToExcelFile = "Failed to save the data to an Excel file.";

        // Object explorer.
        public const string ObjectExplorerCaption = "Object Explorer";
        public const string ServerNotRegistered = "SQL Server instance is not registered with SQLsecure for auditing.";
        public const string GetAuditServerInfoFromRepositoryFailed = "Failed to retrieve audited server information from the Repository.";
        public const string GetSnapshotFilesFailed = "Failed to retrieve the Server File System Objects from the Repository.";
        public const string GetSnapshotRegistryFailed = "Failed to retrieve the Server Registry Keys from the Repository.";
        public const string GetSnapshotServicesFailed = "Failed to retrieve the Server Services from the Repository.";
        public const string GetSnapshotLoginsFailed = "Failed to retrieve the Login objects from the Repository.";
        public const string GetSnapshotServerRolesFailed = "Failed to retrieve the Server Role objects from the Repository.";
        public const string GetSnapshotEndpointsFailed = "Failed to retrieve the Endpoint objects from the Repository.";
        public const string GetSnapshotUsersFailed = "Failed to retrieve the User objects from the Repository.";
        public const string GetSnapshotRolesFailed = "Failed to retrieve the Role objects from the Repository.";
        public const string GetSnapshotSchemasFailed = "Failed to retrieve the Schema objects from the Repository.";
        public const string GetSnapshotKeysFailed = "Failed to retrieve the Key objects from the Repository.";
        public const string GetSnapshotCertificatesFailed = "Failed to retrieve the Certificate objects from the Repository.";
        public const string GetSnapshotTablesFailed = "Failed to retrieve the Table objects from the Repository.";
        public const string GetSnapshotViewsFailed = "Failed to retrieve the View objects from the Repository.";
        public const string GetSnapshotSynonymsFailed = "Failed to retrieve the Synonym objects from the Repository.";
        public const string GetSnapshotStoredProceduresFailed = "Failed to retrieve the Stored Procedure objects from the Repository.";
        public const string GetSnapshotFunctionsFailed = "Failed to retrieve the Function objects from the Repository.";
        public const string GetSnapshotExtendedStoredProceduresFailed = "Failed to retrieve the Extended Stored Procedure objects from the Repository.";
        public const string GetSnapshotAssembliesFailed = "Failed to retrieve the Assembly objects from the Repository.";
        public const string GetSnapshotUserDefinedDataTypeFailed = "Failed to retrieve the User-defined Data Type objects from the Repository.";
        public const string GetSnapshotXMLSchemaCollectionsFailed = "Failed to retrieve the XML Schema Collection objects from the Repository.";
        public const string GetSnapshotFullTextCatalogsFailed = "Failed to retrieve the Full Text Catalog objects from the Repository.";
        public const string CantGetServerObjectPermissions = @"Unable to retrieve server object permissions.";
        public const string CantGetServerOsObjectPermissions = @"Unable to retrieve server os object permissions.";
        public const string CantGetDbObjectProperties = @"Failed to retrieve database object properties.";
        public const string CantGetDbPrincipalPermissions = @"Unable to retrieve database principal permissions.";
        public const string CantGetServerPrincipalPermissions = @"Unable to retrieve server principal permissions.";

        // Snapshot Properties
        public const string SnapshotPropertiesCaption = "Snapshot Summary";

        // Windows suspect accounts.
        public const string UnresolvedWindowsAccountsCaption = @"Suspect Windows Accounts";
        public const string CantGetUnresolvedWindowsAccounts = @"Failed to retrieve list of suspect Windows Accounts.";

        // Reports
        public const string ReportsCaption = @"Reporting Services";
        public const string CantRunReportingServices = @"Unable to launch Reporting Services.";
        public const string CantGetReportData = @"Unable to retrieve requested report data.";
        public const string CantGetReportsRecord = "Unable to retrieve information on the Reporting Services configuration from the database.";
        public const string CantSaveReportsRecord = "Unable to save the Reporting Services configuration to the database.";
        public const string ReportsComputerBad = "The Reporting Services computer may not be blank.";
        public const string ReportsFolderBad = "The Reporting Services folder may not be blank.";

        public const string ReportAllServersWarning = "You have selected to run this report for all servers which can take significant time to generate and may heavily use your system resources. It is recommended that you run it from Microsoft Reporting Services instead.\n\nContinue running report?";

        //Import Servers
        public const string ImportServersCaption = @"Import SQL Server";

        public const string AllowSqlServersUpdate =
            "There is at least one already registered server found in the import file and it's credentials data will be updated based on those specified in the file.\n Do you want to proceed?";

        public const string InvalidImportFileFormat = @"The file you are trying to import have invalid format";
        public const string PleaseProvideValidFile = @"File does not exists!\nPlease provide file to import";

        public const string ImportedWithErrors =
            @"Import operation complete with errors. Some servers cold be not imported.";

        public const string ImportSuccessfull = @"Import operation successfull";
        public const string ImportCancelled = @"Import operation cancelled";
        public const string RepositoryObjectNull = @"Repository object not specified";

        public const string ManageTags = "Manage Server Group Tags";
    }
}
