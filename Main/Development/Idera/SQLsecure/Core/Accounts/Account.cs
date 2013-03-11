/******************************************************************
 * Name: Account.cs
 *
 * Description: Base class for users, groups, etc.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

using System.Diagnostics;
using System.DirectoryServices;
using System.Security.Principal;

using Idera.SQLsecure.Core.Logger;

namespace Idera.SQLsecure.Core.Accounts
{

    /// <summary>
    /// Encapsulates a windows account.
    /// </summary>
    public class Account
    {


        #region Fields & Enums
        private static LogX logX = new LogX("Idera.SQLsecure.Core.Accounts.Account");
        private bool m_IsLocal;
        private string m_DomainFlat;
        private Sid m_Sid;
        private string m_SamPath;
        private string m_AdsiPath;
        private ObjectClass m_ObjectClass;
        private AccountStatusEnum m_accountStatus;

        public enum AccountStatusEnum
        {
            Account_Good,
            Account_Suspected
        }

        #endregion




        #region Helpers

        public static string GetStatusStringFromEnum(AccountStatusEnum accountStatus)
        {
            string status = string.Empty;
            switch (accountStatus)
            {
                case AccountStatusEnum.Account_Good:
                    status = "G";
                    break;
                case AccountStatusEnum.Account_Suspected:
                    status = "S";
                    break;
                default:
                    Debug.Assert(true, "Unknown accountStatus");
                    status = "S";
                    break;
            }
            return status;
        }

        public static ObjectClass getObjectClass(string className)
        {
            Debug.Assert(!string.IsNullOrEmpty(className));

            ObjectClass oClass = ObjectClass.Unknown;

            if (String.Compare(className, Constants.ClassUser, true) == 0)
            {
                oClass = ObjectClass.User;
            }
            else if (String.Compare(className, Constants.ClassGroup, true) == 0)
            {
                oClass = ObjectClass.Group;
            }
            else if (String.Compare(className, Constants.ClassComputer, true) == 0)
            {
                oClass = ObjectClass.Computer;
            }
            else if (String.Compare(className, Constants.ClassInetOrgPerson, true) == 0)
            {
                oClass = ObjectClass.User;
            }
            else
            {
                Debug.Assert(false);
                oClass = ObjectClass.Unknown;
            }

            return oClass;
        }
        #endregion
        
        #region Ctors
        public static bool CreateAccount(
                Accounts.Domain domain,
                Sid sid,
                DirectoryEntry dirEntry,
                out Account account
            )
        {
            Debug.Assert(domain != null);
            Debug.Assert(sid != null && sid.IsValid);
            Debug.Assert(dirEntry != null);

            // Init return.
            bool isOk = true;
            account = null;

            // Get samAccountName and object class.
            string samAccountName = string.Empty;
            ObjectClass oClass = ObjectClass.Unknown;
            int gType = 0;
            string adsiPath = string.Empty;
            try
            {
                switch (domain.Type)
                {
                    case DomainType.AD:
                        // Get sam account name.
                        if (dirEntry.Properties[Constants.PropSamAccountName].Count == 1
                            && dirEntry.Properties[Constants.PropSamAccountName][0].GetType() == typeof(String))
                        {
                            samAccountName = (string)dirEntry.Properties[Constants.PropSamAccountName][0];
                        }
                        break;

                    case DomainType.SAM:
                        samAccountName = dirEntry.Name;
                        break;

                    default:
                        break;
                }
                if (String.IsNullOrEmpty(samAccountName))
                {
                    logX.loggerX.Error("ERROR - failed to get SAM account name for the SID:< ", sid.ToString(), ">");
                    isOk = false;
                }

                // Get object class.
                if (isOk)
                {
                    oClass = getObjectClass(dirEntry.SchemaClassName);
                    if (oClass == ObjectClass.Unknown)
                    {
                        logX.loggerX.Error("ERROR - failed to get object category for the SID:< ", sid.ToString(), ">");
                        isOk = false;
                    }
                }

                // Get group type if the class is Group.
                if (isOk)
                {
                    if (oClass == ObjectClass.Group)
                    {
                        if (dirEntry.Properties[Constants.PropGroupType].Count == 1
                            && dirEntry.Properties[Constants.PropGroupType][0].GetType() == typeof(int))
                        {
                            gType = (int)dirEntry.Properties[Constants.PropGroupType][0];
                        }
                        else
                        {
                            logX.loggerX.Error("ERROR - failed to get group type");
                            isOk = false;
                        }
                    }
                }

                // Get adsi path.
                if (isOk)
                {
                    adsiPath = dirEntry.Path;
                    if (string.IsNullOrEmpty(adsiPath))
                    {
                        logX.loggerX.Error("ERROR - failed to get adsi path");
                        isOk = false;
                    }
                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error("ERROR - exception raised when querying dir entry object, ", ex.Message);
                isOk = false;
            }

            // If the object category is a group, determine the type of group.
            if(oClass == ObjectClass.Group)
            {
                // Determine the type of group.
                if ((gType & Constants.AdsGroupTypeGlobalGroup) == Constants.AdsGroupTypeGlobalGroup)
                {
                    oClass = ObjectClass.GlobalGroup;
                }
                else if ((gType & Constants.AdsGroupTypeDomainLocalGroup) == Constants.AdsGroupTypeDomainLocalGroup)
                {
                    oClass = ObjectClass.LocalGroup;
                }
                else if ((gType & Constants.AdsGroupTypeUniversalGroup) == Constants.AdsGroupTypeUniversalGroup)
                {
                    oClass = ObjectClass.UniversalGroup;
                }
                else
                {
                    Debug.Assert(false);
                    oClass = ObjectClass.Unknown;
                    isOk = false;
                }

                // For AD domains check if its a security group.
                if (isOk)
                {
                    if (domain.Type == DomainType.AD)
                    {
                        if ((gType & Constants.AdsGroupTypeSecurityEnabled) != Constants.AdsGroupTypeSecurityEnabled)
                        {
                            oClass = ObjectClass.DistributionGroup;
                        }
                    }
                }
            }

            // Create the account object.
            if (isOk)
            {
                account = new Account(false, domain.FlatName, sid, Path.MakeSamPath(domain.FlatName, samAccountName), adsiPath, oClass);
            }

            return isOk;
        }

        public static bool CreateAccount(
                Accounts.Domain domain,
                DirectoryEntry dirEntry,
                out Account account
            )
        {
            Debug.Assert(domain != null);
            Debug.Assert(dirEntry != null);

            // Init return.
            bool isOk = true;
            account = null;

            // Get samAccountName, SID and object class.
            string samAccountName = string.Empty;
            Sid sid = null;
            ObjectClass oClass = ObjectClass.Unknown;
            int gType = 0;
            string adsiPath = string.Empty;
            try
            {
                // Get the SID of the object.
                sid = DirectoryEntrySid(dirEntry);
                if (sid == null)
                {
                    logX.loggerX.Error("ERROR - SID is invalid");
                    isOk = false;
                }

                // Get sam account name.
                if (isOk)
                {
                    switch (domain.Type)
                    {
                        case DomainType.AD:
                            // Get sam account name.
                            if (dirEntry.Properties[Constants.PropSamAccountName].Count == 1
                                && dirEntry.Properties[Constants.PropSamAccountName][0].GetType() == typeof(String))
                            {
                                samAccountName = (string)dirEntry.Properties[Constants.PropSamAccountName][0];
                            }
                            break;

                        case DomainType.SAM:
                            samAccountName = dirEntry.Name;
                            break;

                        default:
                            break;
                    }
                    if (String.IsNullOrEmpty(samAccountName))
                    {
                        logX.loggerX.Error("ERROR - failed to get SAM account name for the SID:< ", sid.ToString(), ">");
                        isOk = false;
                    }
                }

                // Get object class.
                if (isOk)
                {
                    oClass = getObjectClass(dirEntry.SchemaClassName);
                    if (oClass == ObjectClass.Unknown)
                    {
                        logX.loggerX.Error("ERROR - failed to get object category for the member:  ", dirEntry.Path);
                        isOk = false;
                    }
                }

                // Get group type if the class is Group.
                if (isOk)
                {
                    if (oClass == ObjectClass.Group)
                    {
                        if (dirEntry.Properties[Constants.PropGroupType].Count == 1
                            && dirEntry.Properties[Constants.PropGroupType][0].GetType() == typeof(int))
                        {
                            gType = (int)dirEntry.Properties[Constants.PropGroupType][0];
                        }
                        else
                        {
                            logX.loggerX.Error("ERROR - failed to get group type");
                            isOk = false;
                        }
                    }
                }

                // Get ADSI path.
                if (isOk)
                {
                    adsiPath = dirEntry.Path;
                    if (string.IsNullOrEmpty(adsiPath))
                    {
                        logX.loggerX.Error("ERROR - failed to get adsi path");
                        isOk = false;
                    }
                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error("ERROR - exception raised when querying dir entry object, ", ex.Message);
                isOk = false;
            }

            // If the object category is a group, determine the type of group.
            if (oClass == ObjectClass.Group)
            {
                // Determine the type of group.
                if ((gType & Constants.AdsGroupTypeGlobalGroup) == Constants.AdsGroupTypeGlobalGroup)
                {
                    oClass = ObjectClass.GlobalGroup;
                }
                else if ((gType & Constants.AdsGroupTypeDomainLocalGroup) == Constants.AdsGroupTypeDomainLocalGroup)
                {
                    oClass = ObjectClass.LocalGroup;
                }
                else if ((gType & Constants.AdsGroupTypeUniversalGroup) == Constants.AdsGroupTypeUniversalGroup)
                {
                    oClass = ObjectClass.UniversalGroup;
                }
                else
                {
                    Debug.Assert(false);
                    oClass = ObjectClass.Unknown;
                    isOk = false;
                }

                // For AD domains check if its a security group.
                if (isOk)
                {
                    if (domain.Type == DomainType.AD)
                    {
                        if ((gType & Constants.AdsGroupTypeSecurityEnabled) != Constants.AdsGroupTypeSecurityEnabled)
                        {
                            oClass = ObjectClass.DistributionGroup;
                        }
                    }
                }
            }

            // Create the account object.
            if (isOk)
            {
                account = new Account(false, domain.FlatName, sid, Path.MakeSamPath(domain.FlatName, samAccountName), adsiPath, oClass);
            }

            return isOk;
        }

        public static bool CreateAccount(
            string domainFlatName, 
            Sid sid, 
            string samPath, 
            string adsiPath, 
            ObjectClass objClass,
            out Account account)
        {
            Debug.Assert(sid != null && sid.IsValid);
            Debug.Assert(!string.IsNullOrEmpty(samPath));
            Debug.Assert(!string.IsNullOrEmpty(adsiPath));
            Debug.Assert(!string.IsNullOrEmpty(domainFlatName));

            account = new Account(false, domainFlatName, sid, samPath, adsiPath, objClass);
            
            return true;
        }

        public static bool CreateWellknownGroup(
                string authority,
                Sid sid,
                string samPath,
                string winNtPath,
                out Account account
            )
        {
            Debug.Assert(sid != null && sid.IsValid);
            Debug.Assert(!string.IsNullOrEmpty(samPath));
            Debug.Assert(!string.IsNullOrEmpty(winNtPath));

            account = new Account(true, authority, sid, samPath, winNtPath, ObjectClass.WellknownGroup);

            return true;
        }

        public static bool CreateLocalAccount(
                string serverName,
                Sid sid,
                DirectoryEntry dirEntry,
                out Account account
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(serverName));
            Debug.Assert(sid != null && sid.IsValid);
            Debug.Assert(dirEntry != null);

            // Init returns.
            bool isOk = true;
            account = null;

            // Get sam name and object class.
            string samAccountName = string.Empty;
            ObjectClass oClass = ObjectClass.Unknown;
            string adsiPath = string.Empty;
            try
            {
                // Get samAccountName.
                samAccountName = dirEntry.Name;
                isOk = !string.IsNullOrEmpty(samAccountName);

                // Get object class.
                if (isOk)
                {
                    oClass = getObjectClass(dirEntry.SchemaClassName);
                    if (oClass == ObjectClass.Group)
                    {
                        oClass = ObjectClass.LocalGroup;
                    }
                }

                // Get adsi path.
                if (isOk)
                {
                    adsiPath = dirEntry.Path;
                    if (string.IsNullOrEmpty(adsiPath))
                    {
                        logX.loggerX.Error("ERROR - failed to get adsi path");
                        isOk = false;
                    }
                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error("ERROR - error in querying dir entry object for properties, ", ex.Message);
                isOk = false;
            }

            // Create the account object.
            if (isOk)
            {
                account = new Account(true, serverName, sid, Path.MakeSamPath(serverName, samAccountName), adsiPath, oClass);
            }

            return isOk;
        }

        public static bool CreateLocalAccount(
                string serverName,
                DirectoryEntry dirEntry,
                out Account account
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(serverName));
            Debug.Assert(dirEntry != null);

            // Init returns.
            bool isOk = true;
            account = null;

            // Get dir entry properties.
            Sid sid = null;
            string samAccountName = string.Empty;
            ObjectClass oClass = ObjectClass.Unknown;
            string adsiPath = string.Empty;
            try
            {
                // Get the SID of the object.
                sid = DirectoryEntrySid(dirEntry);
                if (sid == null)
                {
                    logX.loggerX.Error("ERROR - SID is invalid");
                    isOk = false;
                }

                // Get samAccountName.
                if (isOk)
                {
                    samAccountName = dirEntry.Name;
                    if (string.IsNullOrEmpty(samAccountName))
                    {
                        logX.loggerX.Error("ERROR - invalid sam account name");
                        isOk = false;
                    }
                }

                // Get object class.
                if (isOk)
                {
                    oClass = getObjectClass(dirEntry.SchemaClassName);
                    if (oClass == ObjectClass.Group)
                    {
                        oClass = ObjectClass.LocalGroup;
                    }
                    if (oClass == ObjectClass.Unknown)
                    {
                        logX.loggerX.Error("ERROR - failed to determine class name");
                        isOk = false;
                    }
                }

                // Get adsi path.
                if (isOk)
                {
                    adsiPath = dirEntry.Path;
                    if (string.IsNullOrEmpty(adsiPath))
                    {
                        logX.loggerX.Error("ERROR - failed to get adsi path");
                        isOk = false;
                    }
                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error("ERROR - failed to retrieve dir entry object properties, ", ex.Message);
                isOk = false;
            }

            // Create the account object.
            if (isOk)
            {
                account = new Account(true, serverName, sid, Path.MakeSamPath(serverName, samAccountName), adsiPath, oClass);
            }

            return isOk;
        }

        public static bool CreateUnknownAccount(Sid sid,
                string samPath,
                out Account account
            )
        {
            account = new Account(sid, samPath, ObjectClass.Unknown);
            return true;
        }

        public static bool CreateUserAccount(
                Sid sid,
                string samPath,
                out Account account
            )
        {
            account = new Account(sid, samPath, ObjectClass.User);
            return true;
        }

        public static bool CreateGroupAccount(
                        Sid sid,
                        string samPath,
                        out Account account
                    )
        {
            account = new Account(sid, samPath, ObjectClass.Group);
            return true;
        }

        private Account(
                bool isLocal,
                string domainFlat,
                Sid sid,
                string samPath,
                string adsiPath,
                ObjectClass objectClass
            )
        {
            m_IsLocal = isLocal;
            m_DomainFlat = domainFlat;
            m_Sid = sid;
            m_SamPath = samPath;
            m_AdsiPath = adsiPath;
            m_ObjectClass = objectClass;
            m_accountStatus = AccountStatusEnum.Account_Good;
        }

        private Account(
                Sid sid,
                string samPath,
                ObjectClass objectClass
            )
        {
            m_Sid = sid;
            m_SamPath = samPath;
            m_ObjectClass = objectClass;
            m_accountStatus = AccountStatusEnum.Account_Good;
        }

        #endregion
        
        #region Properties

        public AccountStatusEnum AccountStatus
        {
            get { return m_accountStatus; }
            set { m_accountStatus = value; }
        }

        public bool IsLocal
        {
            get { return m_IsLocal; }
        }
        public string DomainFlat
        {
            get { return m_DomainFlat; }
        }
        public Sid SID
        {
            get { return m_Sid; }
        }
        public string SamPath
        {
            get { return m_SamPath; }
        }
        public string AdsiPath
        {
            get { return m_AdsiPath; }
        }
        public ObjectClass Class
        {
            get { return m_ObjectClass; }
            set { m_ObjectClass = value; }
        }
        public bool IsNonDistributionGroup
        {
            get
            {
                bool flag = false;
                switch (m_ObjectClass)
                {
                    case ObjectClass.LocalGroup:
                    case ObjectClass.GlobalGroup:
                    case ObjectClass.UniversalGroup:
                        flag = true;
                        break;
                    default:
                        flag = false;
                        break;
                }

                return flag;
            }
        }
        #endregion
        
        #region Methods

        public static bool IsSecurityObject(DirectoryEntry dirEntry)
        {
            bool isSecurityObject = false;

            if (string.Compare(dirEntry.SchemaClassName, Constants.ClassGroup, true) == 0
                || string.Compare(dirEntry.SchemaClassName, Constants.ClassUser, true) == 0
                || string.Compare(dirEntry.SchemaClassName, Constants.ClassComputer, true) == 0
                || string.Compare(dirEntry.SchemaClassName, Constants.ClassFSP, true) == 0 
                || string.Compare(dirEntry.SchemaClassName, Constants.ClassInetOrgPerson, true) == 0)
            {
                isSecurityObject = true;
                // Check if disturbution group
                // ---------------------------
                //if (string.Compare(dirEntry.SchemaClassName, Constants.ClassGroup, true) == 0)
                //{
                //    int groupType = 0;
                //    if (dirEntry.Properties[Constants.PropGroupType].Count == 1
                //        && dirEntry.Properties[Constants.PropGroupType][0].GetType() == typeof(int))
                //    {
                //        groupType = (int)dirEntry.Properties[Constants.PropGroupType][0];
                //        if (((groupType & Constants.AdsGroupTypeDomainLocalGroup) == Constants.AdsGroupTypeDomainLocalGroup
                //               || (groupType & Constants.AdsGroupTypeGlobalGroup) == Constants.AdsGroupTypeGlobalGroup 
                //               || (groupType & Constants.AdsGroupTypeUniversalGroup) == Constants.AdsGroupTypeUniversalGroup)
                //           && (groupType & Constants.AdsGroupTypeSecurityEnabled) != Constants.AdsGroupTypeSecurityEnabled)
                //        {
                //            isSecurityObject = false;
                //        }
                //    }
                //}
            }

            return isSecurityObject;
        }

        public bool IsValidSid(string server)
        {
            string name;
            string domain;
            Interop.SID_NAME_USE peUse;
            bool isOK = m_Sid.LookupAccount(server, out name, out domain, out peUse);
            if (!isOK)
            {
                SecurityIdentifier si = new SecurityIdentifier(m_Sid.SidString);
                Type wellKnownSid = typeof(WellKnownSidType);
                foreach (WellKnownSidType type in Enum.GetValues(wellKnownSid) )
                {
                    if(si.IsWellKnown(type))
                    {
                        isOK = true;
                        break;
                    }
                }
            }
            return isOK;
        }

        public static bool IsForeignSecurityPrincipal (DirectoryEntry dirEntry)
        {
            Debug.Assert(dirEntry != null);
            bool isFsp = false;
            if (dirEntry != null) 
            {
                try
                {
                    isFsp = string.Compare(Constants.ClassFSP,dirEntry.SchemaClassName,true) == 0;
                }
                catch (Exception ex)
                {
                    logX.loggerX.Error("ERROR - exception raised when getting class of the dir entry object, ", ex.Message);
                    isFsp = false;
                }
            }
            return isFsp;
        }

        public static Sid DirectoryEntrySid (DirectoryEntry dirEntry)
        {
            Debug.Assert(dirEntry != null);
            Sid sid = null;
            if (dirEntry != null)
            {
                try
                {
                    byte[] bSid = (byte[])dirEntry.Properties["objectSID"].Value;
                    sid = new Sid(bSid);
                    if (!sid.IsValid)
                    {
                        logX.loggerX.Error("ERROR - SID is invalid");
                        sid = null;
                    }
                }
                catch (Exception ex)
                {
                    logX.loggerX.Error("ERROR - exception raised when getting the sid from dir entry, ", ex.Message);
                    sid = null;
                }
            }
            return sid;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) { return false; }
            if (this.GetType() != obj.GetType()) { return false; }
            Account other = (Account)obj;
            SecurityIdentifier lSid = new SecurityIdentifier(m_Sid.BinarySid, 0);
            SecurityIdentifier rSid = new SecurityIdentifier(other.m_Sid.BinarySid, 0);
            return lSid.CompareTo(rSid) == 0;
        }

        public override int GetHashCode()
        {
            SecurityIdentifier lSid = new SecurityIdentifier(m_Sid.BinarySid, 0);
            return lSid.GetHashCode();
        }

        #endregion
    }
          
}
