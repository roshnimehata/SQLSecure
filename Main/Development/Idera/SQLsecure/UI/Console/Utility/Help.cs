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
        public const String LINK_FMT = "SQLsecure Help\\{0}";
        #endregion

        #region Links

        public const string IderaHomePage = @"http://www.idera.com";
        public const string SQLsecureHomePage = @"http://www.idera.com/Products/SQLsecure";
        public const string SupportHomePage = @"http://www.idera.com/support";
        public const string KnowledgeBaseHomePage = @"http://www.idera.com/support/Service.aspx";
        public const string IderaProducts = @"http://www.idera.com/Products/Default.aspx";
        public const string CheckUpdates = @"http://www.idera.com/webscripts/VersionCheck.aspx?productid={0}&v={1}";
        public const string productID = "sqlsecure";
        public const string productVersion = "27000";

        #endregion

        public static string GetHelpFilePath()
        {
            return string.Format(FILE_FMT, Path.GetDirectoryName(Application.ExecutablePath), SQLSECURE_CHM_FILE_STR);
        }

        #region Private helpers

        public static void showHelp(Control control, HelpNavigator navIn, string paramIn)
        {
            string topic = string.Empty;
            if (!string.IsNullOrEmpty(paramIn))
            {
                topic = string.Format(LINK_FMT, paramIn);
            }

            IntPtr hWnd;
            if (!Idera.WebHelp.WebHelpLauncher.TryShowWebHelp(topic, out hWnd))
            {
                showHelpChm(control, navIn, paramIn);
            }
        }

        private static void showHelpChm(
                System.Windows.Forms.Control controlIn,
                HelpNavigator navIn,
                String paramIn
            )
        {
            // Get full path of the chm file, assumes its in the same directory
            // as the Console binary.
            String helpFile = string.Format(FILE_FMT, Path.GetDirectoryName(Application.ExecutablePath), SQLSECURE_CHM_FILE_STR);

            // Display the help file.
            try
            {
                System.Windows.Forms.Help.ShowHelp(controlIn, helpFile, navIn, string.Format(LINK_FMT, paramIn));
            }
            catch (Exception ex)
            {
                MsgBox.ShowError("SQLsecure Help", ErrorMsgs.CantLoadHelpFile, ex);
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


        public const String DefaultHelpPage = @"Start Help.htm";

        // View - Main Security Summary - Policy Report Card
        public const String SecuritySummaryPolicyReportCardHelpTopic = @"View Enterprise Report Card.htm";

        // View - Main Security Summary - Server Report Card
        public const String SecuritySummaryServerReportCardHelpTopic = @"Viewing the Server Security.htm";

        // View - Main Security Summary - Policy Settings
        public const String SecuritySummaryPolicySettingsHelpTopic = @"View Settings Across Servers.htm";

        // View - Main Security Summary - Policy Users
        public const String SecuritySummaryPolicyUsersHelpTopic = @"View Users Across Servers.htm";

        // View - Main Security Summary - Server Settings
        public const String SecuritySummaryServerSettingsHelpTopic = @"Viewing the Server Settings.htm";

        // View - Main Security Summary - Server Users
        public const String SecuritySummaryServerUsersHelpTopic = @"Viewing the Server Users.htm";

        // View - Assessment Draft - All Servers
        public const String AssessmentSummaryDraftAllServersHelpTopic = @"Working with Draft Assessments.htm";

        // View - Assessment Draft - Server
        public const String AssessmentSummaryDraftServerHelpTopic = @"Working with Draft Assessments.htm";

        // View - Assessment Published - All Servers
        public const String AssessmentSummaryPublishedAllServersHelpTopic = @"Working with Published Assessments.htm";

        // View - Assessment Published - Server
        public const String AssessmentSummaryPublishedServerHelpTopic = @"Working with Published Assessments.htm";

        // View - Assessment Approved - All Servers
        public const String AssessmentSummaryApprovedAllServersHelpTopic = @"Working with Approved Assessments.htm";

        // View - Assessment Approved - Server
        public const String AssessmentSummaryApprovedServerHelpTopic = @"Working with Approved Assessments.htm";



        // View - Man Security Summary - Change Log
        public const String SecuritySummaryChangeLogHelpTopic = @"View Assessment Change Log.htm";
        public const String SecuritySummaryChangeLogConceptTopic = @"View Assessment Change Log.htm";      

        // View - Main Explore Permissions - now Audited SQL Servers
        public const String ExplorePermissionsHelpTopic = @"View All Servers Summary.htm";
        public const String ExplorePermissionsConceptTopic = @"View All Servers Summary.htm";

        // View - Server
        public const String ServerHelpTopic = @"View Server Summary.htm";
        public const String ServerConceptTopic = @"View Server Summary.htm";

        // View - Explore Permissions - User Permissions
        public const String UserPermissionsHelpTopic = @"Explore User Permissions.htm";
        public const String UserPermissionsConceptTopic = @"Explore User Permissions.htm";

        // View - Explore Permissions - Role Permissions
        public const String RolePermissionsHelpTopic = @"Explore Role Permissions.htm";
        public const String RolePermissionsConceptTopic = @"Explore Role Permissions.htm";

        // View - Explore Permissions - Object Permissions
        public const String ObjectPermissionsHelpTopic = @"Explore Object Permissions.htm";
        public const String ObjectPermissionsConceptTopic = @"Explore Object Permissions.htm";

        // View - Explore Permissions - Snapshots
        public const String ExploreSnapshotsHelpTopic = @"View Snapshot Summary.htm";
        public const String ExploreSnapshotsConceptTopic = @"View Snapshot Summary.htm";

        // View - Permission Explorer
        public const String PermissionExplorerHelpTopic = @"View Snapshot Summary.htm";
        public const String PermissionExplorerConceptTopic = @"View Snapshot Summary.htm";

        // View - Reports
        public const String ReportHelpTopic = @"Reports.htm";
        public const String ReportConceptTopic = @"Reports.htm";

        // View - Manage SQLsecure
        public const String ManageSQLsecureHelpTopic = @"View Repository Status.htm";
        public const String ManageSQLsecureConceptTopic = @"View Repository Status.htm";

        // View - SQLsecure Activity
        public const String SQLsecureActivityHelpTopic = @"View SQLsecure Activity.htm";
        public const String SQLsecureActivityConceptTopic = @"View SQLsecure Activity.htm";

        // View - Logins
        public const String LoginsHelpTopic = @"Manage SQLsecure Logins.htm";
        public const String LoginsConceptTopic = @"Manage SQLsecure Logins.htm";

        // View - Manage Policies
        public const String ManagePoliciesHelpTopic = @"Manage Policies.htm";
        public const String ManagePoliciesConceptTopic = @"Manage Policies.htm";

        //// Dialog Help Topics
        public const string CompareAssessmentsSummaryHelpTopic = @"Compare Assessment Summaries.htm";
        public const string CompareAssessmentsSecurityChecksHelpTopic = @"Compare Assessment Security Checks.htm";
        public const string CompareAssessmentsInternalReviewNotesHelpTopic = @"Compare Internal Review Notes.htm";

        // Register Server Wizard
        public const string RegisterSQLServerWizardHelpTopic = @"Register a SQL Server.htm";
        public const string AddServerGeneralHelpTopic = @"Select a SQL Server.htm";
        public const string AddServerCredentialsHelpTopic = @"Specify Connection Credentials.htm";
        public const string AddServerFiltersHelpTopic = @"Select SQL Server Objects to Audit.htm";
        public const string AddServerScheduleHelpTopic = @"Schedule Snapshots.htm";
        public const string AddServerEmailHelpTopic = @"Configure Email Notification.htm";
        public const string AddServerCollectionTopic = @"Choose to Take Snapshot.htm";
        public const string AddServerPoliciesHelpTopic = @"Add Server to Policies.htm";
        public const string AddServerReviewHelpTopic = @"Review Registration Summary.htm";

        // Register Server Property Pages
        public const string ServerGeneralHelpTopic = @"ASP General tab.htm";
        public const string ServerCredentialsHelpTopic = @"ASP Credentials tab.htm";
        public const string ServerFiltersHelpTopic = @"ASP Filters tab.htm";
        public const string ServerScheduleHelpTopic = @"ASP Schedule tab.htm";
        public const string ServerEmailHelpTopic = @"ASP Email tab.htm";
        public const string ServerPoliciesHelpTopic = @"ASP Policies tab.htm";

        // Create Policy Wizard
        public const string CreatePolicyWizardHelpTopic = @"Add New Policy.htm";
        public const string AddPolicyTypeHelpTopic = @"Select Policy Template.htm";
        public const string AddPolicyNameHelpTopic = @"Specify Policy Properties.htm";
        public const string AddPolicySecurityChecksHelpTopic = @"Select Security Checks.htm";
        public const string AddPolicyServersHelpTopic = @"Assign SQL Server.htm";
        public const string AddPolicyInterviewHelpTopic = @"Enter IR Notes.htm";
        public const string AddPolicySummaryHelpTopic = @"Review Policy Summary.htm";

        // Policy Property Pages
        public const string PolicyGeneralHelpTopic = @"Change Policy Properties.htm";
        public const string PolicySecurityChecksHelpTopic = @"Change Security Checks.htm";
        public const string PolicyServersHelpTopic = @"Change Server Membership.htm";
        public const string PolicyInterviewHelpTopic = @"Change IR Notes.htm";       

        // Assessment Property Pages
        public const string AssessmentGeneralHelpTopic = @"Change Assessment Properties.htm";
        public const string AssessmentSecurityChecksHelpTopic = @"Change Assessment Security Checks.htm";
        public const string AssessmentServersHelpTopic = @"Change SQL Servers Audited by Assessment.htm";
        public const string AssessmentInterviewHelpTopic = @"Change Internal Review Notes in Assessment.htm";       


        // Import Policy Dialog
        public const string PolicyImportHelpTopic = @"Import Policy.htm";

        public const string RegisterNewLoginWizardHelpTopic = @"Add New Login.htm";
        public const string RegisterNewLoginWizardSpecifyLoginHelpTopic = @"Specify Login Properties.htm";
        public const string RegisterNewLoginWizardPermissionsHelpTopic = @"Select Permissions.htm";
        public const string RegisterNewLoginWizardFinishHelpTopic = @"Review Login Summary.htm";

        public const string ConfigureEmailProviderHelpTopic = @"Configure Email Settings.htm";
        public const string ConfigureWeakPasswordDetectionHelpTopic = @"Configure Password Detection.htm";
        public const string GroomingScheduleHelpTopic = @"Grooming Snapshots.htm";
        public const string ManageLicenseHelpTopic = @"Manage Your License.htm";

        // Snapshot summary tabs on bottom of view
        public const string MissingDatabasesHelpTopic = @"Unavailable Databases.htm";
        public const string MissingUsersHelpTopic = @"Suspect Windows Accounts.htm";
        public const string MissingOsUsersHelpTopic = @"Suspect OS Windows Accounts.htm";
        public const string WindowsAccountsHelpTopic = @"Windows Accounts.htm";
        public const string WindowsOSAccountsHelpTopic = @"OS Windows Accounts.htm";
        public const string SnapshotFiltersHelpTopic = @"View Filters for a Snapshot.htm";
              

        // Wizard - Add Filter
        public const string AddFilterWizardHelpTopic = @"Using the Add Filter wizard.htm";
        // Dialog - Filter Properties
        public const string FilterPropertiesHelpTopic = @"Edit Filter Properties.htm";
        // Wizard - User Permissions
        public const String UserPermissionWizardTopic = @"Explore User Permissions.htm";
        // Wizard - Object Permissions
        public const String ObjectPermissionWizardTopic = @"Explore Object Permissions.htm";
        // Dialog - Form Snapshot Properties
        public const string SnapshotPropertiesHelpTopic = @"Viewing Snapshot Properties.htm";
        // Dialog - Form_SnapshotDatabaseProperties
        public const string DatabasePropertiesHelpTopic = @"Database Properties.htm";
        // Dialog - Delete Snapshot
        public const string DeleteSnapshotHelpTopic = @"Delete a data snapshot.htm";
        // Dialog - Missing Credentials (for upgrading from 1.x to 2.x)
        public const string UpdateMissingCredentials = @"Upgrade Credentials.htm";
        // Dialog - Form Schedule Job
        public const string ScheduleHelpTopic = @"ASP Schedule tab.htm";
        // Dialog - Select Database
        public const string SelectDatabaseHelpTopic = @"Selecting Databases.htm";
        // Dialog - Select Registered Server
        public const string SelectServerHelpTopic = @"Selecting a Server.htm";
        // Dialog - Select Snapshot
        public const string SelectSnapshotHelpTopic = @"Change Audit Data Settings.htm";
        // Dialog - Select User
        public const string SelectWindowsUserHelpTopic = @"Selecting a Windows User.htm";
        public const string SelectSQLUserHelpTopic = @"Selecting a SQL Server Login.htm";
        // Dialog - Form Select Roles
        public const string SelectRoleHelpTopic = @"Selecting Roles.htm";
        // Dialog - View Group Members
        public const string ViewGroupMemberHelpTopic = @"Explore User Permissions.htm";
        // Dialog - Baseline Snapshot
        public const string BaselineSnapshotHelpTopic = @"Designate a baseline snapshot.htm";
        // Dialog - Set Report Server
        public const string SetReportServerHelpTopic = @"Use RS to Gen Reports.htm";

        #region New for 2.5

        public const string CreateAssessmentHelpTopic = @"Create or Save New Assessment.htm";
        public const string RefreshAuditDataHelpTopic = @"Refresh Audit Data.htm";
        public const string SelectAuditDataHelpTopic = @"Select Audit Data for Assessment.htm";
        public const string EditExplanationNotesHelpTopic = @"Edit Explanation Notes.htm";
        public const string ConnectRepositoryHelpTopic = @"Connect to Repository.htm";
        public const string LoginPropertiesHelpTopic = @"Edit Login Settings.htm";
                
        #endregion

        #region New for 2.6 - Deploy Reports Wizard
        public const string DeployReportsWizard_Welcome = @"Deploy Reports Wizard Welcome.htm";
        public const string DeployReportsWizard_Location = @"Deploy Reports Wizard Location.htm";
        public const string DeployReportsWizard_Connect = @"Deploy Reports Wizard Connect.htm";
        public const string DeployReportsWizard_Repository = @"Deploy Reports Wizard Repository.htm";
        public const string DeployReportsWizard_Summary = @"Deploy Reports Wizard Summary.htm";
        // Dialog - Select Report Server Target Folder
        public const string SelectTargetFolderHelpTopic = DeployReportsWizard_Location;
        public const string PolicyTemplatesHelpTopic = @"Policy Templates.htm";

        #endregion

        // How Do I Topics
        public const string HowDoIAsssessSecurity = @"Assess Your Security Model.htm";
        public const string HowDoIExplorePermissionsHelpTopic = @"Explore Permissions.htm";
        public const string HowDoIGenerateReportsHelpTopic = @"Use Console to Gen Report.htm";
        public const string HowDoIManageSQLsecureHelpTopic = @"Using the Manage SQLsecure.htm";

        #endregion
    }
}
