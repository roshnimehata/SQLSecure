/******************************************************************
 * Name: Help.cs
 *
 * Description: Helper functions and constants.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using System.IO;
using System.Windows.Forms;

namespace Idera.SQLsecure.UI.Console.Utility
{
    internal static class Help
    {
        #region Constants
        private const String SQLSECURE_CHM_FILE_STR = @"SQLsecure Help.chm";
        private const String FILE_FMT = "{0}\\{1}";
        public const String LINK_FMT = "http://wiki.idera.com{0}";
        #endregion

        #region Links

        public const string IderaHomePage = @"http://www.idera.com";
        public const string SQLsecureHomePage = @"https://www.idera.com/productssolutions/sqlserver/sqlsecure";
        public const string SupportHomePage = @"http://www.idera.com/support";
        public const string KnowledgeBaseHomePage = @"http://www.idera.com/support/Service.aspx";
        public const string IderaProducts = @"http://www.idera.com/Products/Default.aspx";
        public const string CheckUpdates = @"http://www.idera.com/webscripts/VersionCheck.aspx?productid={0}&v={1}";
        public const string productID = "sqlsecure";
        public const string productVersion = "3110";
        public const string ABOUT_IDERA = "http://wiki.idera.com/x/0whK";

        #endregion

        public static string GetHelpFilePath()
        {
            return string.Format(FILE_FMT, Path.GetDirectoryName(Application.ExecutablePath), SQLSECURE_CHM_FILE_STR);
        }

        #region Private helpers

        public static void showHelp(Control control, HelpNavigator navIn, string paramIn)
        {
            if (control != null)
            {
                control.Cursor = Cursors.WaitCursor;
            }

            Process.Start(string.Format(LINK_FMT, paramIn));

            if (control != null)
            {
                control.Cursor = Cursors.Default;
            }
        }

        #endregion

        #region Help display functions
        /// <summary>
        /// Show help topic.
        /// </summary>
        /// <param name="controlIn">Parent control</param>
        /// <param name="topicIn">Topic keyword</param>
        public static void ShowTopic(
                System.Windows.Forms.Control controlIn,
                String topicIn
            )
        {
            showHelp(controlIn, HelpNavigator.Topic, topicIn);
        }

        public static void ShowTableOfContents(
                System.Windows.Forms.Control controlIn
            )
        {
            showHelp(controlIn, HelpNavigator.TableOfContents, "");
        }

        public static void ShowIndex(
                System.Windows.Forms.Control controlIn
            )
        {
            showHelp(controlIn, HelpNavigator.Index, "");
        }

        public static void ShowSearch(
                System.Windows.Forms.Control controlIn
            )
        {
            showHelp(controlIn, HelpNavigator.Find, "");
        }


        #endregion

        #region View Help Topic URLs


        public const String DefaultHelpPage = @"/x/5QhK";//Start Help

        // View - Main Security Summary - Policy Report Card
        public const String SecuritySummaryPolicyReportCardHelpTopic = @"/x/JQlK";//View enterprise report Card

        // View - Main Security Summary - Server Report Card
        public const String SecuritySummaryServerReportCardHelpTopic = @"/x/KQlK";//Viewing the Server Security

        // View - Main Security Summary - Policy Settings
        public const String SecuritySummaryPolicySettingsHelpTopic = @"/x/JglK";//View Settings Across Servers

        // View - Main Security Summary - Policy Users
        public const String SecuritySummaryPolicyUsersHelpTopic = @"/x/JwlK";//View Users Across Servers

        // View - Main Security Summary - Server Settings
        public const String SecuritySummaryServerSettingsHelpTopic = @"/x/KglK";//Viewing the Server Settings

        // View - Main Security Summary - Server Users
        public const String SecuritySummaryServerUsersHelpTopic = @"/x/KwlK";//Viewing the Server Users

        // View - Assessment Draft - All Servers
        public const String AssessmentSummaryDraftAllServersHelpTopic = @"/x/RAlK";//Working with Draft Assessments

        // View - Assessment Draft - Server
        public const String AssessmentSummaryDraftServerHelpTopic = @"/x/RAlK";//Working with Draft Assessments

        // View - Assessment Published - All Servers
        public const String AssessmentSummaryPublishedAllServersHelpTopic = @"/x/RQlK";//Working with Published Assessments

        // View - Assessment Published - Server
        public const String AssessmentSummaryPublishedServerHelpTopic = @"/x/RQlK";//Working with Published Assessments

        // View - Assessment Approved - All Servers
        public const String AssessmentSummaryApprovedAllServersHelpTopic = @"/x/RglK";//Working with Approved Assessments

        // View - Assessment Approved - Server
        public const String AssessmentSummaryApprovedServerHelpTopic = @"/x/RglK";//Working with Approved Assessments



        // View - Man Security Summary - Change Log
        public const String SecuritySummaryChangeLogHelpTopic = @"/x/RwlK";//View Assessment Change Log
        public const String SecuritySummaryChangeLogConceptTopic = @"/x/RwlK";//View Assessment Change Log

        // View - Main Explore Permissions - now Audited SQL Servers
        public const String ExplorePermissionsHelpTopic = @"/x/GAlK";//View All Servers Summary
        public const String ExplorePermissionsConceptTopic = @"/x/GAlK";//View All Servers Summary

        // View - Server
        public const String ServerHelpTopic = @"/x/GQlK";//View Server Summary
        public const String ServerConceptTopic = @"/x/GQlK";//View Server Summary

        // View - Explore Permissions - User Permissions
        public const String UserPermissionsHelpTopic = @"/x/EglK";//Explore User Permissions
        public const String UserPermissionsConceptTopic = @"/x/EglK";//Explore User Permissions

        // View - Explore Permissions - Role Permissions
        public const String RolePermissionsHelpTopic = @"/x/DglK";//Explore Role Permissions
        public const String RolePermissionsConceptTopic = @"/x/DglK";//Explore Role Permissions

        // View - Explore Permissions - Object Permissions
        public const String ObjectPermissionsHelpTopic = @"/x/CwlK";//Explore Object Permissions
        public const String ObjectPermissionsConceptTopic = @"/x/CwlK";//Explore Object Permissions

        // View - Explore Permissions - Snapshots
        public const String ExploreSnapshotsHelpTopic = @"/x/GglK";//View Snapshot Summary
        public const String ExploreSnapshotsConceptTopic = @"/x/GglK";//View Snapshot Summary

        // View - Permission Explorer
        public const String PermissionExplorerHelpTopic = @"/x/GglK";//View Snapshot Summary
        public const String PermissionExplorerConceptTopic = @"/x/GglK";//View Snapshot Summary

        // View - Reports
        public const String ReportHelpTopic = @"/x/TAlK";//Reports
        public const String ReportConceptTopic = @"/x/TAlK";//Reports

        // View - Manage SQLsecure
        public const String ManageSQLsecureHelpTopic = @"/x/ZAlK";//View Repository Status
        public const String ManageSQLsecureConceptTopic = @"/x/ZAlK";//View Repository Status

        // View - SQLsecure Activity
        public const String SQLsecureActivityHelpTopic = @"/x/YwlK";//View SQLsecure Activity
        public const String SQLsecureActivityConceptTopic = @"/x/YwlK";//View SQLsecure Activity

        // View - Logins
        public const String LoginsHelpTopic = @"/x/WwlK";//Manage SQLsecure Logins
        public const String LoginsConceptTopic = @"/x/WwlK";//Manage SQLsecure Logins

        // View - Manage Policies
        public const String ManagePoliciesHelpTopic = @"/x/WQlK";//Manage Policies
        public const String ManagePoliciesConceptTopic = @"/x/WQlK";//Manage Policies


        // View - Manage Tags
        public const String ManageTagsHelpTopic = @"/x/CgDwAw";//Manage tags
        public const String ManageTagsConceptTopic = @"/x/CgDwAw";//Manage tags

        //// Dialog Help Topics
        public const string CompareAssessmentsSummaryHelpTopic = @"/x/SQlK";//Compare Assessment Summaries
        public const string CompareAssessmentsSecurityChecksHelpTopic = @"/x/SglK";//Compare Assessment Security Checks
        public const string CompareAssessmentsInternalReviewNotesHelpTopic = @"/x/SwlK";//Compare Internal Review Notes

        // Register Server Wizard
        public const string RegisterSQLServerWizardHelpTopic = @"/x/9whK";//Register a SQL Server
        public const string AddServerGeneralHelpTopic = @"/x/_AhK";//Select a SQL Server
        public const string AddServerCredentialsHelpTopic = @"/x/_QhK";//Specify Connection Credentials
        public const string AddServerFiltersHelpTopic = @"/x/_ghK";//Select SQL Server Objects to Audit
        public const string AddServerScheduleHelpTopic = @"/x/_whK";//Schedule Snapshots
        public const string AddServerEmailHelpTopic = @"/x/-AhK";//Configure Email Notification
        public const string AddServerCollectionTopic = @"/x/-ghK";//Choose to Take Snapshot
        public const string AddServerPoliciesHelpTopic = @"/x/-QhK";//Add Server to Policies
        public const string AddServerReviewHelpTopic = @"/x/-whK";//Review Registration Summary

        // Register Server Property Pages
        public const string ServerGeneralHelpTopic = @"/x/VglK";//ASP General tab
        public const string ServerCredentialsHelpTopic = @"/x/VAlK";//ASP Credentials tab
        public const string ServerAuditFoldersHelpTopic = @"/x/2QRJAg";//Audit Folders tab
        public const string ServerFiltersHelpTopic = @"/x/UwlK";//ASP Filters tab
        public const string ServerScheduleHelpTopic = @"/x/VwlK";//ASP Schedule tab
        public const string ServerEmailHelpTopic = @"/x/VQlK";//ASP Email tab
        public const string ServerPoliciesHelpTopic = @"/x/WAlK";//ASP Policies tab

        // Create Policy Wizard
        public const string CreatePolicyWizardHelpTopic = @"/x/LglK";//Add New Policy
        public const string AddPolicyTypeHelpTopic = @"/x/LwlK";//Select Policy Template
        public const string AddPolicyNameHelpTopic = @"/x/MAlK";//Specify Policy Properties
        public const string AddPolicySecurityChecksHelpTopic = @"/x/MQlK";//Select Security Checks
        public const string AddPolicyServersHelpTopic = @"/x/MglK";//Assign SQL Server
        public const string AddPolicyInterviewHelpTopic = @"/x/MwlK";//Enter IR Notes
        public const string AddPolicySummaryHelpTopic = @"/x/NAlK";//Review Policy Summary

        // Policy Property Pages
        public const string PolicyGeneralHelpTopic = @"/x/NglK";//Change Policy Properties
        public const string PolicySecurityChecksHelpTopic = @"/x/NwlK";//Change Security Checks
        public const string PolicyServersHelpTopic = @"/x/OAlK";//Change Server Membership
        public const string PolicyInterviewHelpTopic = @"/x/OQlK";//Change IR notes       

        // Assessment Property Pages
        public const string AssessmentGeneralHelpTopic = @"/x/QAlK";//Change Assessment Properties
        public const string AssessmentSecurityChecksHelpTopic = @"/x/QQlK";//Change Assessment Security Checks
        public const string AssessmentServersHelpTopic = @"/x/KID3";//Change SQL Servers Audited by Assessment
        public const string AssessmentInterviewHelpTopic = @"/x/QglK";//Change Internal Review Notes in Assessment     


        // Import Policy Dialog
        public const string PolicyImportHelpTopic = @"/x/OwlK";//Import Policy

        public const string RegisterNewLoginWizardHelpTopic = @"/x/XAlK"; //Add New Login
        public const string RegisterNewLoginWizardSpecifyLoginHelpTopic = @"/x/XQlK";//Specify Login Properties
        public const string RegisterNewLoginWizardPermissionsHelpTopic = @"/x/XglK";//Select Permissions
        public const string RegisterNewLoginWizardFinishHelpTopic = @"/x/XwlK";//Review Login Summary

        public const string ConfigureEmailProviderHelpTopic = @"/x/UAlK";//Configure Email Settings
        public const string ConfigureWeakPasswordDetectionHelpTopic = @"/x/9ghK";//Configure Password Detection
        public const string GroomingScheduleHelpTopic = @"/x/CQlK";//Grooming Snapshots
        public const string ManageLicenseHelpTopic = @"/x/WglK";//Manage Your License

        // Snapshot summary tabs on bottom of view
        public const string MissingDatabasesHelpTopic = @"/x/HwlK";//Unavailable Databases
        public const string MissingUsersHelpTopic = @"/x/HQlK";//Suspect Windows Accounts
        public const string MissingOsUsersHelpTopic = @"/x/HglK";//Suspect OS Windows Accounts
        public const string WindowsAccountsHelpTopic = @"/x/GwlK";//Windows Accounts
        public const string WindowsOSAccountsHelpTopic = @"/x/HAlK";//OS Windows Accounts
        public const string SnapshotFiltersHelpTopic = @"/x/IAlK";//View Filters for a snapshot
              

        // Wizard - Add Filter
        public const string AddFilterWizardHelpTopic = @"/x/AQlK";//Using the Add Filter wizard
        // Dialog - Filter Properties
        public const string FilterPropertiesHelpTopic = @"/x/BAlK";//Edit Filter Properties
        // Wizard - User Permissions
        public const String UserPermissionWizardTopic = @"/x/EglK";//Explore User Permissions
        // Wizard - Object Permissions
        public const String ObjectPermissionWizardTopic = @"/x/CwlK";//Explore Object Permissions
        // Dialog - Form Snapshot Properties
        public const string SnapshotPropertiesHelpTopic = @"/x/IQlK";//Viewing Snapshot Properties
        // Dialog - Form_SnapshotDatabaseProperties
        public const string DatabasePropertiesHelpTopic = @"/x/DAlK";//Database Properties
        // Dialog - Delete Snapshot
        public const string DeleteSnapshotHelpTopic = @"/x/UQlK";//Delete a data snapshot
        // Dialog - Missing Credentials (for upgrading from 1.x to 2.x)
        public const string UpdateMissingCredentials = @"/x/_QhK";//Upgrade Credentials
        // Dialog - Form Schedule Job
        public const string ScheduleHelpTopic = @"/x/VwlK";//ASP Schedule tab
        // Dialog - Select Database
        public const string SelectDatabaseHelpTopic = @"/x/AwlK";//Selecting Databases
        // Dialog - Select Registered Server
        public const string SelectServerHelpTopic = @"/x/YQlK";//Selecting a Server
        // Dialog - Select Snapshot
        public const string SelectSnapshotHelpTopic = @"/x/QglK";//Change Audit Data Settings
        // Dialog - Select User
        public const string SelectWindowsUserHelpTopic = @"/x/FglK";//Selecting a Windows User
        public const string SelectSQLUserHelpTopic = @"/x/FwlK";//Selecting a SQL Server Login
        // Dialog - Form Select Roles
        public const string SelectRoleHelpTopic = @"/x/DglK ";//Selecting Roles
        // Dialog - View Group Members
        public const string ViewGroupMemberHelpTopic = @"/x/EglK";//Explore User Permissions
        // Dialog - Baseline Snapshot
        public const string BaselineSnapshotHelpTopic = @"/x/CAlK";//Designate a baseline snapshot
        // Dialog - Set Report Server
        public const string SetReportServerHelpTopic = @"/x/TglK";//Use RS to Gen reports

        public const string ImportServerHelpTopic = @"/x/nQPqAw";

        #region New for 2.5

        public const string CreateAssessmentHelpTopic = @"/x/PQlK";//Create or Save New Assessment
        public const string RefreshAuditDataHelpTopic = @"/x/PglK";//Refresh Audit Data
        public const string SelectAuditDataHelpTopic = @"/x/IwlK";//Select Audit Data for Assessments
        public const string EditExplanationNotesHelpTopic = @"/x/QglK";//Edit Explanation notes
        public const string ConnectRepositoryHelpTopic = @"/x/9QhK";//Connect to Repository
        public const string LoginPropertiesHelpTopic = @"/x/YAlK";//Edit Login Settings
                
        #endregion

        #region New for 2.6 - Deploy Reports Wizard
        public const string DeployReportsWizard_Welcome = @"/x/kAlK";//Deploy Reports Wizard Welcome
        public const string DeployReportsWizard_Location = @"/x/mwlK";//Deploy Reports Wizard Location
        public const string DeployReportsWizard_Connect = @"/x/lQlK";//Deploy Reports Wizard Connect
        public const string DeployReportsWizard_Repository = @"/x/mAlK";//Deploy Reports Wizard Repository
        public const string DeployReportsWizard_Summary = @"/x/nQlK";//Deploy Reports Wizard Summary
        // Dialog - Select Report Server Target Folder
        public const string SelectTargetFolderHelpTopic = DeployReportsWizard_Location;
        public const string PolicyTemplatesHelpTopic = @"/x/LQlK";//Policy Templates

        #endregion

        // How Do I Topics
        public const string HowDoIAsssessSecurity = @"/x/IglK";//Assess Your Security Model
        public const string HowDoIExplorePermissionsHelpTopic = @"/x/CglK";//Explore Permissions
        public const string HowDoIGenerateReportsHelpTopic = @"/x/TQlK";//Use Console to Gen Report
        public const string HowDoIManageSQLsecureHelpTopic = @"/x/TwlK";//Using the Manage SQLsecure

        #endregion
    }
}
