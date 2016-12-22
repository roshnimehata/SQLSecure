/******************************************************************
 * Name: Constants.cs
 *
 * Description: Data loader constants are defined in this file.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

using Idera.SQLsecure.Core.Logger;

namespace Idera.SQLsecure.Collector
{
    internal class Constants
    {
        internal const int SQLsecureProductID = 1000;
        internal const string SQLsecureLicenseProductVersionStr = "1.1";
        internal const string SQLsecureCollectorVersion = "2.0.0.0";
        internal const string SpecialMasterDatabase = "SpecialMasterDB987654321";

        #region Logging

        public enum CollectionStatus
        {
            StatusSuccess,
            StatusWarning,
            StatusError
        }


        // Snapshot status
        // ---------------
        internal const char StatusSuccess = 'S';
        internal const char StatusError = 'E';
        internal const char StatusWarning = 'W';
        internal const char StatusInProgress = 'I';

        // Application Activy table
        // ------------------------
        internal const string ActivityCategory_CollectData  = "Data Collection";
        internal const string ActivityType_Error            = "Error";
        internal const string ActivityType_Warning          = "Warning";
        internal const string ActivityType_Info             = "Information";
        internal const string ActivityEvent_Start           = "Start";
        internal const string ActivityEvent_Metrics         = "Metrics";
        internal const string ActivityEvent_Error           = "Error";


        #endregion

        #region Param processing
        internal const string TargetInstance        = "-TargetInstance";        // Audited target instance
        internal const string Repository            = "-Repository";            // SQLsecure Repository
        internal const string Verbose               = "-Verbose";
        internal const string Manual                = "-Manual";                // Collector was started manually 
                                                                                // default is automated 
        

        // These are advanced options that are not publicized.
        internal const string RepositoryDatabase    = "-RepositoryDatabase";     // SQLsecure Repository database
        internal const string RepositoryUser        = "-RepositoryUser";         // SQL login credentials for connecting to the repository
        internal const string RepositoryPassword    = "-RepositoryPassword";
        internal const string EncryptPassword       = "-EncryptPassword";       // Command output the encrypted version of the supplied password
        internal const string EncryptedRepositoryPassword    = "-EncryptedRepositoryPassword";    // For supplying an ecrypted string representing the password for repository access

        internal const string CopyrightMsg          = "\nCopyright © 2005-2012 Idera, Inc.";
        internal const string UsageMsg = "\nSQLsecure Collector utility loads SQL Server security data and associated\n"
                                                      + "Windows accounts.\n\n"
                                                      + "Usage: Idera.SQLsecure.Collector -TargetInstance TARGETSQLSERVER\n"
                                                      + "       -Repository REPOSITORYSERVER \n"
                                                      + "       [-RepositoryUser REPOSITORYUSER] \n"
                                                      + "       [-RepositoryPassword REPOSITORYPASSWORD] \n"
                                                      + "       [-EncryptedRepositoryPassword ENCRYPTEDREPOSITORYPASSWORD] \n"
                                                      + "       [-Manual] [-Verbose]\n"
                                                      + "       [-EncryptPassword PASSWORDTOENCRYPT] \n"
                                                      + "\n(identifiers are shown in uppercase, [] means optional)\n\n"
                                                      + "   TargetInstance              : Target SQL Server instance to collect security data.\n"
                                                      + "   Repository                  : Repository SQL Server to save the collected data.\n"
                                                      + "   RepositoryUser              : User name for Repository SQL Server.\n"
                                                      + "   RepositoryPassword          : Password for Repository SQL Server.\n"
                                                      + "   EncryptedRepositoryPassword : Encrypted Password for Repository SQL Server.\n"
                                                      + "   Manual                      : If collector is started manually vs automated.\n"
                                                      + "   Verbose                     : Displays messages to the console.\n"
                                                      + "   EncryptPassword             : Encrypts a password for use with the EncryptedRepositoryPassword option.\n";
        #endregion

        #region SQL Server Stuff
        internal const int DalVersion = 3002;
        internal const int SchemaVersion = 3002;

        internal const string AdminRole = @"Admin";
        internal const string LoaderRole = @"Loader";

        internal const char MixedAuthentication = 'M';
        internal const char WindowsAuthentication = 'W';

        internal const int LoginAuditModeNone = 0;
        internal const int LoginAuditModeSuccess = 1;
        internal const int LoginAuditModeFailure = 2;
        internal const int LoginAuditModeAll = 3;
        internal const string LoginAuditModeNoneStr = "None";
        internal const string LoginAuditModeSuccessStr = "Success";
        internal const string LoginAuditModeFailureStr = "Failure";
        internal const string LoginAuditModeAllStr = "All";

        internal const char Yes = 'Y';
        internal const char No = 'N';
        internal const char Unknown = 'U';

        internal const char Grant = 'G';
        internal const char Deny = 'D';

        #endregion

        #region Audit Folders
   
        public const string AUDIT_FOLDER_DELIMITER = "|";
   
        #endregion

    }
}
