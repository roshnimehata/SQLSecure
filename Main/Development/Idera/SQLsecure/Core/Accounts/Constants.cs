/******************************************************************
 * Name: Constants.cs
 *
 * Description: Constants used by the Accounts dll are defined here.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Idera.SQLsecure.Core.Accounts
{
    public static class Constants
    {
        #region Pulic constants

        #endregion

        #region Internal constants
        internal const string SingleBackSlashStr = @"\";
        internal const string DoubleBackSlashStr = @"\\";
        internal const string SingleSlashStr = "/";
        internal const string DoubleSlashStr = "//";

        // Path related constants.
        internal const string GcPrefix = @"GC://";
        internal const string LdapPrefix = @"LDAP://";
        internal const string WinNTPrefix = @"WinNT://";
        internal const string LdapDcAttrib = @"dc";
        internal const string LdapCnAttrib = @"cn";
        internal const char EqChar = '=';
        internal const int AdsSetTypeDn = 4;

        // ADSI related constants (LDAP/WinNT).
        internal const string PropSamAccountName = "sAMAccountName";
        internal const string PropObjectCategory = "objectCategory";
        internal const string PropObjectSid = "objectSid";
        internal const string PropGroupType = "groupType";
        internal const string PropMembers = "members";
        internal const string PropPrimaryGroupToken = "primaryGroupToken";

        internal const string PrimaryGroupID = "primaryGroupID";

        // Values for SchemaClassName
        internal const string ClassUser = "User";
        internal const string ClassGroup = "Group";
        internal const string ClassComputer = "Computer";
        internal const string ClassContact = "Contact";
        internal const string ClassInetOrgPerson = "InetOrgPerson";
        internal const string ClassFSP = "foreignSecurityPrincipal";
        internal const string ClassDomainDNS = "domainDNS";

        internal const int AdsGroupTypeGlobalGroup = 0x00000002;
        internal const int AdsGroupTypeDomainLocalGroup = 0x00000004;
        internal const int AdsGroupTypeUniversalGroup = 0x00000008;
        internal const uint AdsGroupTypeSecurityEnabled = 0x80000000;

        internal const int RangeEnumMemberCount = 1000;


        // WMI query related constants.
        internal const string AuthorityNTLM = "ntlmdomain:";
        public const string Cimv2Root = @"\root\cimv2";
        internal const string Win32OperatingSystemQuery = @"SELECT Caption, Version, BuildNumber, CSDVersion, WindowsDirectory FROM Win32_OperatingSystem";
        internal const string Win32ComputerSystemQuery = @"SELECT Domain, DomainRole, CurrentTimeZone FROM Win32_ComputerSystem";
        internal const string OSProductProperty = "Caption";
        internal const string OSVersionProperty = "Version";
        internal const string OSBuildNumberProperty = "BuildNumber";
        internal const string OSServicePackProperty = "CSDVersion";
        internal const string OSWindowsDirectory = "WindowsDirectory";
        internal const string CSDomainProperty = "Domain";
        internal const string CSDomainRoleProperty = "DomainRole";
        internal const string CSTimeZoneOffset = "CurrentTimeZone";

        #endregion
    }
}
