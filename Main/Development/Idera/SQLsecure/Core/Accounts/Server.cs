/******************************************************************
 * Name: Server.cs
 *
 * Description: Encapsulates member server or workstation object.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

using ActiveDs;
using System.Diagnostics;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Management;
using System.Net;
using System.Security.Principal;
using System.IO;
using System.Security.AccessControl;
using Microsoft.Win32;

using Idera.SQLsecure.Core.Interop;
using Idera.SQLsecure.Core.Logger;
using System.Runtime.InteropServices;

namespace Idera.SQLsecure.Core.Accounts
{
    public class Server
    {
        public enum ForceLocalStatusEnum
        {
            Unknown,
            Succeeded,
            Failed
        }

        #region Fields
        private static LogX logX = new LogX("Idera.SQLsecure.Core.Accounts.Server");
        private int m_numWarnings;
        private bool m_IsValid;
        private bool m_IsAdmin = false;
        private string m_Name;
        private string m_BindUser;
        private string m_BindDomain;
        private string m_BindAccount;
        private string m_BindPassword;

        private string m_Product;
        private string m_ServicePack;
        private string m_Version;
        private string m_BuildNumber;
        private string m_SystemDrive;
        private int m_TimeZoneOffset;
        private Sid m_ComputerSid;
        private string m_DomainName;
        private OSDomainRole m_DomainRole;
        private string m_SQLPath = string.Empty;

        public ForceLocalStatusEnum ForceLocalStatus = ForceLocalStatusEnum.Unknown;

        #endregion

        #region Helpers

        private static bool isDomainController(OSDomainRole role)
        {
            return (role == OSDomainRole.PDC || role == OSDomainRole.BDC);
        }

        private static bool isMemberServer(OSDomainRole role)
        {
            return (role == OSDomainRole.MemberServer || role == OSDomainRole.MemberWorkstation);
        }

        private static bool isInDomain(OSDomainRole role)
        {
            return (isDomainController(role) || isMemberServer(role));
        }

        private static bool isStandalone(OSDomainRole role)
        {
            return (role == OSDomainRole.StandaloneServer || role == OSDomainRole.StandaloneWorkstation);
        }

        public void setBindCredentials(
                string account,
                string password
            )
        {
            Debug.Assert(!String.IsNullOrEmpty(account) ? !String.IsNullOrEmpty(password) : true);

            if (!string.IsNullOrEmpty(account))
            {
                Path.SplitSamPath(account, out m_BindDomain, out m_BindUser);
                m_BindAccount = account;    
                m_BindPassword = password;

                RealBind();
            }
        }

        private bool isCredentialSet
        {
            get { return (m_BindUser != null && m_BindUser.Length != 0); }
        }

        static private bool isLocalComputer(
                string computer
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(computer));
            return string.Compare(computer, Environment.MachineName, true) == 0
                    || string.Compare(computer, Dns.GetHostName(), true) == 0;
        }

        public static string GetActiveComputerName(string computerName)
        { 
             RegistryKey remoteBaseKey = null;
             RegistryKey valueKey = null;
             string activeComputerName = string.Empty;

             try
             {
                remoteBaseKey =
                   RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine,
                                                  computerName);
                valueKey = remoteBaseKey.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\ComputerName\ActiveComputerName");
                activeComputerName = (string)valueKey.GetValue("ComputerName");
             }
             catch (Exception ex)
             {
                throw; //caller should handle this exception
             }
             finally
             {
                if (valueKey != null)
                   valueKey.Close();
                if (remoteBaseKey != null)
                   remoteBaseKey.Close();
             }

             return activeComputerName;
        }

        #endregion

        #region Group Membership Functions

        private enum AccountLocation
        {
            Unknown,
            Local,
            Domain,
            Wellknown
        }

        private AccountLocation sidLocation(
                Sid sidObj,
                SID_NAME_USE peUse
            )
        {
            Debug.Assert(IsValid);

            // If property loading has failed or the SID is invalid, return UNKNOWN
            if (!IsValid || !sidObj.IsValid)
            {
                return AccountLocation.Unknown;
            }

            // If this is a well-known group, return its location.
            if (peUse == SID_NAME_USE.SidTypeWellKnownGroup)
            {
                return AccountLocation.Wellknown;
            }

            // Determine location based on the server role.
            AccountLocation location = AccountLocation.Unknown;
            if (isDomainController(m_DomainRole)) // DC
            {
                // All SIDs belong to a domain.
                location = AccountLocation.Domain;
            }
            else if(isMemberServer(m_DomainRole))// Member server
            {
                // If the SID does not have a DomainID or the DomainID matches
                // the local server SID's domain ID, then its local.  Else its
                // domain.
                if (!sidObj.IsDomainSid || (m_ComputerSid != null && m_ComputerSid.IsEqualDomainId(sidObj)))
                {
                    location = AccountLocation.Local;
                }
                else
                {
                    location = AccountLocation.Domain;
                }
            }
            else if (isStandalone(m_DomainRole)) // Standalone
            {
                // All SIDs are local.
                location = AccountLocation.Local;
            }

            return location;
        }

        // return   0 = no error
        //          < 0 = error
        //          > 0 = warning
        private int enumerateGroup(
                DirectoryEntry dirEntry,
                string ldapRangeEnumPrefix,
                List<String> groupMembers,
                ref List<Account> memberAccounts
            )
        {
            Debug.Assert(dirEntry != null);
            Debug.Assert(groupMembers != null);
            bool bSAMDir = false;
            int numWarnings = 0;
            using (logX.loggerX.DebugCall())
            {
                // Get a count of members.   If its a SAM dir entry, its going
                // to throw a non implemented exception.   In which case set the
                // count to 0.
                IADsMembers members = dirEntry.Invoke(Constants.PropMembers, null) as IADsMembers;
                int count = 0;
                try
                {
                    count = members.Count;
                    bSAMDir = false;
                }
                catch (System.NotImplementedException)
                {
                    bSAMDir = true;
                    count = 0;
                }

                // If count is less then 1000, use invoke method to get the group
                // membership.   Otherwise page through the memberships using directory
                // searcher object.
                // NOTE:  WinNT PROVIDER DOES NOT SUPPORT RANGE ENUMERATION, THIS IS NOT
                // AN ISSUE BECAUSE THE EARLIER CALL TO GET COUNT DOES NOT WORK FOR THIS
                // PROVIDER, AND THE COUNT IS ALWAYS A 0.
                logX.loggerX.Verbose(string.Format("Members in directory {0}: {1}", dirEntry.Path, count));
                try
                {
                    if (count < Constants.RangeEnumMemberCount)
                    {
                        foreach (object member in (System.Collections.IEnumerable) members)
                        {
                            using (DirectoryEntry memDirEntry = new DirectoryEntry(member))
                            {
                                if (!Account.IsSecurityObject(memDirEntry))
                                {
                                    continue;
                                }
                                groupMembers.Add((string) memDirEntry.Path);
                            }
                        }
                    }
                    else
                    {
                        Debug.Assert(!string.IsNullOrEmpty(ldapRangeEnumPrefix));

                        // Enumerate group members by doing a range enumeration.
                        DirectorySearcher searcher = new DirectorySearcher(dirEntry);
                        searcher.Filter = "(objectClass=*)";

                        uint rangeStep = 500;
                        uint rangeLow = 0;
                        uint rangeHigh = rangeLow + (rangeStep - 1);
                        bool lastQuery = false;
                        bool quitLoop = false;

                        do
                        {
                            string attributeWithRange;
                            if (!lastQuery)
                            {
                                attributeWithRange = String.Format("member;range={0}-{1}", rangeLow, rangeHigh);
                            }
                            else
                            {
                                attributeWithRange = String.Format("member;range={0}-*", rangeLow);
                            }
                            searcher.PropertiesToLoad.Clear();
                            searcher.PropertiesToLoad.Add(attributeWithRange);
                            SearchResult results = searcher.FindOne();
                            if (results.Properties.Contains(attributeWithRange))
                            {
                                foreach (object obj in results.Properties[attributeWithRange])
                                {
                                    StringBuilder strBldr = new StringBuilder();
                                    strBldr.Append(ldapRangeEnumPrefix);
                                    strBldr.Append(obj.ToString());
                                    groupMembers.Add(strBldr.ToString());
                                }
                                if (lastQuery)
                                {
                                    quitLoop = true;
                                }
                            }
                            else
                            {
                                if (lastQuery)
                                {
                                    quitLoop = true;
                                }
                                else
                                {
                                    lastQuery = true;
                                }
                            }
                            if (!lastQuery)
                            {
                                rangeLow = rangeHigh + 1;
                                rangeHigh = rangeLow + (rangeStep - 1);
                            }
                        } while (!quitLoop);
                    }

                    // Now add all users that have this group as primary Group ID
                    // Only do this for AD Directories
                    // ----------------------------------------------------------
                    if (!bSAMDir)
                    {
                        using (DirectorySearcher dirSearch1 = new DirectorySearcher())
                        {
                            dirEntry.RefreshCache(new string[] {Constants.PropPrimaryGroupToken});
                            if (dirEntry.Properties[Constants.PropPrimaryGroupToken].Count > 0)
                            {
                                int PrimaryGroupToken = (int) dirEntry.Properties[Constants.PropPrimaryGroupToken][0];
                                // Setup the directory searcher object.
                                // -----------------------------------
                                string domainName;
                                bool isFlat;
                                if (Path.ExtractDomainFromPath(dirEntry.Path, out domainName, out isFlat))
                                {
                                    Domain domain = DomainCollection.GetDomainFromCache(domainName, isFlat);
                                    DirectoryEntry dirRoot =
                                        new DirectoryEntry(
                                            "LDAP://" + domain.FlatDC + "/" + domain.DefaultNamingContext,
                                            domain.User, domain.Password);
                                    logX.loggerX.Info(
                                        string.Format(
                                            "Look for users with group {0} as Primary Group ID in domain: {1}",
                                            dirEntry.Name, dirRoot.Path));
                                    dirSearch1.SearchRoot = dirRoot;
                                    dirSearch1.PropertiesToLoad.AddRange(new string[] { "objectsid", "samaccountname", "objectclass" });                                    
                                    dirSearch1.PageSize = 1000;
                                    dirSearch1.SizeLimit = 1000;
                                    dirSearch1.CacheResults = false;
                                    dirSearch1.SearchScope = SearchScope.Subtree;
                                    dirSearch1.Filter = Constants.PrimaryGroupID + "=" + PrimaryGroupToken;
                                    logX.loggerX.Info("Start Search for users...");
                                    SearchResultCollection srcollection = dirSearch1.FindAll();
                                    logX.loggerX.Info(string.Format("Finished Search, found {0} users.", srcollection.Count));
                                    SearchResult[] searchlist = new SearchResult[srcollection.Count];
                                    srcollection.CopyTo(searchlist, 0);
                                    logX.loggerX.Verbose(string.Format("Copied Search Results."));
                                    int addedUsers = 0;
                                    foreach (SearchResult result in searchlist)
                                    {
                                        addedUsers++;
                                        logX.loggerX.Verbose(string.Format("Found user {0} : {1}", addedUsers, result.Path));
                                        Sid sid = new Sid((byte[])result.Properties["objectsid"][0]);
                                        string samPath = domain.FlatName + Constants.SingleBackSlashStr + (string)result.Properties["samaccountname"][0];
                                        ResultPropertyValueCollection classCollection = result.Properties["objectclass"];
                                        string sClass = (string)classCollection[classCollection.Count - 1];
                                        ObjectClass objClass = Account.getObjectClass(sClass);
                                        Account act = null;
                                        Account.CreateAccount(domain.FlatName, sid, samPath, result.Path, objClass,
                                                              out act);
                                        memberAccounts.Add(act);
                                    }
                                    logX.loggerX.Info(string.Format("Added {0} users from Search Results.", addedUsers));
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    logX.loggerX.Error("ERROR - exception raised when enumerating group members, ", ex.Message);
                    numWarnings++;
                }
            }

            return numWarnings;
        }

        private bool fillMemberAccounts(
                List<String> membersDn,
                ref List<Account> members,
                ref List<String> wellKnownGroups
            )
        {
            // Init returns.
            bool isAllOk = true;
            using (logX.loggerX.DebugCall())
            {
                // Process each member.
                foreach (string dn in membersDn)
                {
                    // If its a WinNT path, we need to check if its a well-known account.
                    bool isOk = true;
                    byte[] bSid = null;
                    string samPath = string.Empty;
                    string refDom = string.Empty;
                    SID_NAME_USE peUseType = SID_NAME_USE.SidTypeUnknown;
                    if (Path.IsWinntPath(dn))
                    {
                        // Get sam path from winnt path.
                        isOk = Path.GetSamPathFromWinNTPath(dn, out samPath);

                        // Lookup account.
                        if (isOk)
                        {
                            isOk =
                                Interop.Authorization.LookupAccountName(Name, samPath, out bSid, out refDom,
                                                                        out peUseType);
                        }
                    }

                    // If this is a well known group then create and add the well known account
                    // to the members list.  And continue to the next DN.
                    if (isOk)
                    {
                        if (peUseType == SID_NAME_USE.SidTypeWellKnownGroup)
                        {
                            Account wellknownacct = null;
                            isOk = Account.CreateWellknownGroup(refDom, new Sid(bSid), samPath, dn, out wellknownacct);
                            if (isOk)
                            {
                                members.Add(wellknownacct);
                                Sid wellKnownSid = new Sid(bSid);
                                if (wellKnownSid.IsSpecialWellKnownSid())
                                {
                                    if (!wellKnownGroups.Contains(samPath))
                                    {
                                        wellKnownGroups.Add(samPath);
                                    }
                                }
                                continue;
                            }
                        }
                    }

                    // Process the regular group if there are no errors.
                    if (isOk)
                    {
                        // Parse the path object to get the domain name.
                        string dnDom = string.Empty;
                        bool isDnDomFlat = false;
                        Account dnAcct = null;
                        if (Path.ExtractDomainFromPath(dn, out dnDom, out isDnDomFlat))
                        {
                            if (string.Compare(dnDom, Name, true) == 0) // local server
                            {
                                using (DirectoryEntry localDirEntry = new DirectoryEntry(dn))
                                {
                                    if (localDirEntry != null)
                                    {
                                        // Create the group object.
                                        isOk = Account.CreateLocalAccount(Name, localDirEntry, out dnAcct);
                                        if (!isOk)
                                        {
                                            logX.loggerX.Error("ERROR - account object creation failed, ", dn);
                                            isOk = false;
                                        }
                                    }
                                    else
                                    {
                                        logX.loggerX.Error("ERROR - failed to get local dir entry, ", dn);
                                        isOk = false;
                                    }
                                }
                            }
                            else // Domain - account object
                            {
                                // Get the domain for the DN.
                                Domain domain =
                                    DomainCollection.GetDomain(Name, dnDom, isDnDomFlat, m_BindAccount, m_BindPassword);
                                if (domain == null)
                                {
                                    logX.loggerX.Error("ERROR - failed to get domain, DN: ", dn);
                                    isOk = false;
                                }

                                // Get the member directory entry.
                                if (isOk)
                                {
                                    using (
                                        DirectoryEntry domDirEntry =
                                            domain.GetDirectoryEntry(m_BindAccount, m_BindPassword, dn))
                                    {
                                        if (domDirEntry != null)
                                        {
                                            // If this is a foreign security principal, then we need to get the
                                            // domain of the foreign security principal and get the corresponding
                                            // directory entry.
                                            if (Account.IsForeignSecurityPrincipal(domDirEntry))
                                            {
                                                logX.loggerX.Verbose("Foreing Security Principal found ", dn);
                                                // Get the sid of the object.
                                                Sid fspSid = Account.DirectoryEntrySid(domDirEntry);

                                                // Lookup the account name and get the domain object.
                                                string fspDomName;
                                                string fspAccount;
                                                SID_NAME_USE peUse;
                                                if (
                                                    fspSid.LookupAccount(Name, out fspAccount, out fspDomName, out peUse))
                                                {
                                                    Domain fspDomain =
                                                        DomainCollection.GetDomain(Name, fspDomName, true, m_BindAccount,
                                                                                   m_BindPassword);
                                                    if (fspDomain != null)
                                                    {
                                                        using (
                                                            DirectoryEntry fspDirEntry =
                                                                fspDomain.GetDirectoryEntry(m_BindAccount,
                                                                                            m_BindPassword,
                                                                                            fspSid, fspAccount))
                                                        {
                                                            if (fspDirEntry != null)
                                                            {
                                                                isOk =
                                                                    Account.CreateAccount(fspDomain, fspSid, fspDirEntry,
                                                                                          out dnAcct);
                                                            }
                                                            else
                                                            {
                                                                logX.loggerX.Error(
                                                                    "ERROR - FSP group directory entry not found for <",
                                                                    fspSid.ToString(), ">");
                                                                isOk = false;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        logX.loggerX.Error("ERROR - failed to get domain object for ",
                                                                           fspDomName);
                                                        isOk = false;
                                                    }
                                                }
                                                else
                                                {
                                                    logX.loggerX.Error("ERROR - failed to lookup account for <",
                                                                       fspSid.ToString(), ">");
                                                    isOk = false;
                                                }
                                            }
                                            else
                                            {
                                                // Create the member account object.
                                                isOk = Account.CreateAccount(domain, domDirEntry, out dnAcct);
                                            }
                                        }
                                        else
                                        {
                                            logX.loggerX.Error("ERROR - failed to get dom dir entry, ", dn);
                                            isOk = false;
                                        }
                                    }
                                }
                            }

                            // Add the account object to the list.
                            if (isOk)
                            {
                                members.Add(dnAcct);
                            }
                        }
                        else
                        {
                            logX.loggerX.Error("ERROR - failed to parse the member DN, ", dn);
                            isOk = false;
                        }
                    }
                    if (!isOk)
                    {
                        isAllOk = false;
                    }
                }
            }
            return isAllOk;
        }
        // ------------------------------
        // return   0 = no error
        //          <0 = error
        //          > 0 = number warnings
        // ------------------------------
        private int getGroupMembers(
                Sid groupSid,
                out Account group,
                out List<Account> members,
                ref List<string> wellKnownGroups
            )
        {
            Debug.Assert(groupSid.IsValid);
            // Init returns.
            group = null;
            members = new List<Account>();
            int numTotalWarnings = 0;

            using (logX.loggerX.DebugCall())
            {

                // Bind to the remote server and get the SID netbios domain 
                // and sam account names.
                string groupName = string.Empty;
                string domainName = string.Empty;
                SID_NAME_USE peUse;
                if (groupSid.LookupAccount(Name, out groupName, out domainName, out peUse))
                {
                    if (peUse != SID_NAME_USE.SidTypeAlias && peUse != SID_NAME_USE.SidTypeGroup
                        && peUse != SID_NAME_USE.SidTypeWellKnownGroup)
                    {
                        logX.loggerX.Error("ERROR - this SID is not a group, SID: <", groupSid.ToString(), ">");
                        numTotalWarnings++;
                    }
                }
                else
                {
                    logX.loggerX.Error("ERROR - failed to lookup account for <", groupSid.ToString(), ">");
                    numTotalWarnings++;
                }
                logX.loggerX.Verbose("Getting members for group: ", groupName);
                // Determine the location of the account.
                AccountLocation location = AccountLocation.Unknown;
                if (numTotalWarnings == 0)
                {
                    location = sidLocation(groupSid, peUse);
                    if (location == AccountLocation.Unknown)
                    {
                        logX.loggerX.Error("ERROR - unable to determine the location of the SID <", groupSid.ToString(),
                                           ">");
                        numTotalWarnings++;
                    }
                }

                // Process group membership based on location.
                List<string> membersDn = new List<string>();
                if (numTotalWarnings == 0)
                {
                    switch (location)
                    {
                        case AccountLocation.Wellknown:
                            logX.loggerX.Verbose(
                                string.Format("Account {0} domain {1} is a wellknown group", groupName, domainName));

                            // Create the group object.
                            string samPath = Path.MakeSamPath(domainName, groupName);
                            Account.CreateWellknownGroup(Name, groupSid, samPath,
                                                         Path.MakeWinntPath(Name, groupName), out group);

                            // Write to WellKnownGroupList
                            if (groupSid.IsSpecialWellKnownSid())
                            {
                                if (!wellKnownGroups.Contains(samPath))
                                {
                                    wellKnownGroups.Add(samPath);
                                }
                            }

                            // No member enumeration is done.
                            break;

                        case AccountLocation.Local: // Local server group
                            logX.loggerX.Verbose(string.Format("Account {0} is a local group", groupName));
                            string path = Path.MakeWinntPath(Name, groupName);
                            using (DirectoryEntry groupDirEntry = new DirectoryEntry(path))
                            {
                                // If group dir entry is found, then get its members.
                                if (groupDirEntry != null)
                                {
                                    // Create the group object.
                                    bool isOk = Account.CreateLocalAccount(Name, groupSid, groupDirEntry, out group);
                                    if (!isOk)
                                    {
                                        logX.loggerX.Error("ERROR - group account object creation failed for <",
                                                           groupSid.ToString(), ">");
                                        numTotalWarnings++;
                                    }

                                    // Enumerate the members if its not a distribution group.
                                    if (numTotalWarnings == 0)
                                    {
                                        if (group.IsNonDistributionGroup)
                                        {
                                            int numWarnings = enumerateGroup(groupDirEntry, null, membersDn, ref members);
                                            if (numWarnings != 0)
                                            {
                                                logX.loggerX.Error("ERROR - enumeration of group failed for <",
                                                                   groupSid.ToString(), ">");
                                                numTotalWarnings += numWarnings;
                                            }
                                        }
                                        else
                                        {
                                            logX.loggerX.Info("This is not a security enabled group, <",
                                                              groupSid.ToString(), ">");
                                        }
                                    }
                                }
                                else
                                {
                                    logX.loggerX.Error("ERROR - group directory entry not found for <",
                                                       groupSid.ToString(), ">");
                                    numTotalWarnings++;
                                }
                            }
                            break;

                        case AccountLocation.Domain: // Domain group
                            logX.loggerX.Verbose(string.Format("Account {0} in domain {1}", groupName, domainName));
                            // Get the domain object.
                            Domain domain =
                                DomainCollection.GetDomain(Name, domainName, true, m_BindAccount, m_BindPassword);
                            if (domain == null)
                            {
                                logX.loggerX.Error("ERROR - failed to get domain object for <", groupSid.ToString(), ">");
                                numTotalWarnings++;
                            }

                            // Get the group directory entry, and then enumerate the members.
                            if (numTotalWarnings == 0)
                            {
                                // Get the directory entry for the group.
                                using (
                                    DirectoryEntry groupDirEntry =
                                        domain.GetDirectoryEntry(m_BindAccount, m_BindPassword, groupSid, groupName))
                                {
                                    // If group dir entry is found, then get its members.
                                    if (groupDirEntry != null)
                                    {
                                        logX.loggerX.Verbose("Create Account...");
                                        // Create the group object.
                                        bool isOk = Account.CreateAccount(domain, groupSid, groupDirEntry, out group);
                                        if (!isOk)
                                        {
                                            logX.loggerX.Error(
                                                "ERROR - domain group account object creation failed for <",
                                                groupSid.ToString(), ">");
                                            numTotalWarnings++;
                                        }


                                        // Enumerate the members if its not a distribution group.
                                        if (numTotalWarnings == 0)
                                        {
                                            if (group.IsNonDistributionGroup)
                                            {
                                                int numWarnings =
                                                    enumerateGroup(groupDirEntry, domain.LdapRangeEnumPrefix, membersDn, ref members);
                                                if (numWarnings != 0)
                                                {
                                                    logX.loggerX.Error("ERROR - domain group enumeration failed for <",
                                                                       groupSid.ToString(), ">");
                                                    numTotalWarnings += numWarnings;
                                                }
                                            }
                                            else
                                            {
                                                logX.loggerX.Info("This is not a security enabled group, <",
                                                                  groupSid.ToString(), ">");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        logX.loggerX.Error("ERROR - group directory entry not found for <",
                                                           groupSid.ToString(), ">");
                                        numTotalWarnings++;
                                    }
                                }
                            }
                            break;

                        case AccountLocation.Unknown:
                            logX.loggerX.Verbose("Account Location Unknown for ", groupName);                            
                            break;
                        default:
                            logX.loggerX.Verbose("Account Location hit default case for ", groupName);
                            Debug.Assert(false);
                            break;
                    }
                }

                // Get the account object for each of the member DNs,
                // and add it to the list.
                if (numTotalWarnings == 0)
                {
                    if (!fillMemberAccounts(membersDn, ref members, ref wellKnownGroups))
                    {
                        numTotalWarnings++;
                    }
                }
            }
            return numTotalWarnings;
        }

        private int getGroupMembers(
                Account group,
                out List<Account> members,
                ref List<string> wellKnownGroups
            )
        {
            Debug.Assert(group != null);
            Debug.Assert(group.IsNonDistributionGroup);

            // Init returns.
            int numTotalWarnings = 0;
            members = new List<Account>();

            // Get directory entry from the local server or domain.
            List<string> membersDn = new List<string>();
            if (group.IsLocal)
            {
                using (DirectoryEntry groupDirEntry = new DirectoryEntry(group.AdsiPath))
                {
                    if (groupDirEntry != null)
                    {
                        int numWarnings = enumerateGroup(groupDirEntry, null, membersDn, ref members);
                        if(numWarnings != 0)
                        {
                            logX.loggerX.Error("ERROR - local group enumeration failed for ", group.SamPath);
                            numTotalWarnings += numWarnings;
                        }
                    }
                    else
                    {
                        logX.loggerX.Error("ERROR - group directory entry not found for ", group.SamPath);
                        numTotalWarnings++;
                    }
                }
            }
            else
            {
                // Get the domain.
                Domain domain = DomainCollection.GetDomain(Name, group.DomainFlat, true, m_BindAccount, m_BindPassword);
                if (domain == null)
                {
                    logX.loggerX.Error("ERROR - failed to get domain object for ", group.SamPath);
                    numTotalWarnings++;
                }

                // Get the group directory entry.
                if (numTotalWarnings == 0)
                {
                    using (DirectoryEntry groupDirEntry = domain.GetDirectoryEntry(m_BindAccount, m_BindPassword, group.AdsiPath))
                    {
                        if (groupDirEntry != null)
                        {
                            int numWarnings = enumerateGroup(groupDirEntry, domain.LdapRangeEnumPrefix, membersDn, ref members);
                            if (numWarnings != 0)
                            {
                                logX.loggerX.Error("ERROR - domain group enumeration failed for ", group.SamPath);
                                numTotalWarnings += numWarnings;
                            }
                        }
                        else
                        {
                            logX.loggerX.Error("ERROR - group directory entry not found for ", group.SamPath);
                            numTotalWarnings++;
                        }
                    }
                }
            }

            // Fill the members list.
            if (numTotalWarnings == 0)
            {
                if (!fillMemberAccounts(membersDn, ref members, ref wellKnownGroups))
                {
                    numTotalWarnings++;
                }
            }

            return numTotalWarnings;
        }

        #endregion

        #region Load Server Property Functions

        private int loadWmiProperties()
        {
            int numWarnings = 0;

            // Set connection option credentials if they are available.
            ConnectionOptions options = null;

            if (isCredentialSet && !IsLocalComputer)  
            {
                if ((ForceLocalStatus == ForceLocalStatusEnum.Unknown) ||   // if we have already forced a local 
                    (ForceLocalStatus == ForceLocalStatusEnum.Failed))      // connection, then don't even try to
                {                                                           // give a username/password
                    options = new ConnectionOptions();
                    options.Authority = Constants.AuthorityNTLM + m_BindDomain;
                    options.Username = m_BindUser;
                    options.Password = m_BindPassword;
                }
            }

            // Create management scope string.
            StringBuilder scopeStr = null;
            scopeStr = new StringBuilder();
            scopeStr.Append(Path.WhackPrefixComputer(m_Name));
            scopeStr.Append(Constants.Cimv2Root);

            try
            {
                // Create management scope and connect.
                ManagementScope scope = null;
                if (options != null)
                {
                    scope = new ManagementScope(scopeStr.ToString(), options);
                }
                else
                {
                    scope = new ManagementScope(scopeStr.ToString());
                }

                try
                {
                    scope.Connect();        //let's see if we can connect
                }
                catch (Exception ConnectionAttempException1)
                {
                    if (ForceLocalStatus == ForceLocalStatusEnum.Unknown)   // if we haven't tried making a local connection
                    {
                        logX.loggerX.Error("First connection attempt failed - retrying as a local connection.");
                        ManagementScope ForcedLocalScope = new ManagementScope(scopeStr.ToString());
                        try
                        {
                            ForcedLocalScope.Connect();
                            ForceLocalStatus = Server.ForceLocalStatusEnum.Succeeded;
                            scope = ForcedLocalScope;
                            logX.loggerX.Info("Local connection attempt succeeded.");
                        }
                        catch (Exception ConnectionAttempException2)
                        {
                            ForceLocalStatus = Server.ForceLocalStatusEnum.Failed;
                            logX.loggerX.Error("Local connection attempt failed.");
                        }
                    }
                }

                scope.Connect();

                // If connect was successfull, then account has admin rights
                m_IsAdmin = true;

                // Get Caption, Version, BuildNumber, CSDVersion from the OS.
                SelectQuery query = new SelectQuery(Constants.Win32OperatingSystemQuery);
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
                {
                    using (ManagementObjectCollection queryCollection = searcher.Get())
                    {
                        Debug.Assert(queryCollection.Count == 1);
                        foreach (ManagementObject mo in queryCollection)
                        {
                            m_Product = checked((string)mo[Constants.OSProductProperty]);
                            m_ServicePack = checked((string)mo[Constants.OSServicePackProperty]);
                            m_Version = checked((string)mo[Constants.OSVersionProperty]);
                            m_BuildNumber = checked((string)mo[Constants.OSBuildNumberProperty]);
                            m_SystemDrive = checked((string)mo[Constants.OSWindowsDirectory]);
                            if (m_SystemDrive.Length > 2)
                            {
                                m_SystemDrive = m_SystemDrive.Substring(0, 2);
                            }
                            break;
                        }
                    }
                }
                logX.loggerX.Info(string.Format(@"{0} - {1} {2} {3} {4}", m_Name, m_Product, m_Version, m_ServicePack, m_SystemDrive));

                // Get Domain, DomainRole and CurrentTimeZone from CS.
                query = new SelectQuery(Constants.Win32ComputerSystemQuery);
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
                {
                    using (ManagementObjectCollection queryCollection = searcher.Get())
                    {
                        Debug.Assert(queryCollection.Count == 1);
                        foreach (ManagementObject mo in queryCollection)
                        {
                            m_DomainName = checked((string)mo[Constants.CSDomainProperty]);
                            m_DomainRole = (OSDomainRole)checked((ushort)mo[Constants.CSDomainRoleProperty]);
                            m_TimeZoneOffset = checked((short)mo[Constants.CSTimeZoneOffset]);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string strMessage = "exception raised when loading WMI properties ";
                logX.loggerX.Error("WARNING - " + strMessage, ex.Message);
//                WriteApplicationActivityToRepository("Warning", "Warning", strMessage + ex.Message);
                numWarnings++;
            }

            return numWarnings;
        }

        private bool loadWellknownSids()
        {
            bool isOk = true;

            string acctName;
            string domName;
            SID_NAME_USE peUse;

            Accounts.Sid everyoneSid = new Sid("S-1-1-0");
            Accounts.Sid.LookupAccountName(Name, everyoneSid, out acctName, out domName, out peUse);
            Accounts.Sid dialupSid = new Sid("S-1-5-1");
            Accounts.Sid.LookupAccountName(Name, dialupSid, out acctName, out domName, out peUse);
            Accounts.Sid networkSid = new Sid("S-1-5-2");
            Accounts.Sid.LookupAccountName(Name, networkSid, out acctName, out domName, out peUse);
            Accounts.Sid batchSid = new Sid("S-1-5-3");
            Accounts.Sid.LookupAccountName(Name, batchSid, out acctName, out domName, out peUse);
            Accounts.Sid interactiveSid = new Sid("S-1-5-4");
            Accounts.Sid.LookupAccountName(Name, interactiveSid, out acctName, out domName, out peUse);
            Accounts.Sid serviceSid = new Sid("S-1-5-6");
            Accounts.Sid.LookupAccountName(Name, serviceSid, out acctName, out domName, out peUse);
            Accounts.Sid anonymousSid = new Sid("S-1-5-7");
            Accounts.Sid.LookupAccountName(Name, anonymousSid, out acctName, out domName, out peUse);
            Accounts.Sid proxySid = new Sid("S-1-5-8");
            Accounts.Sid.LookupAccountName(Name, proxySid, out acctName, out domName, out peUse);
            Accounts.Sid enterpriseDCsSid = new Sid("S-1-5-9");
            Accounts.Sid.LookupAccountName(Name, enterpriseDCsSid, out acctName, out domName, out peUse);
            Accounts.Sid selfSid = new Sid("S-1-5-10");
            Accounts.Sid.LookupAccountName(Name, selfSid, out acctName, out domName, out peUse);
            Accounts.Sid authUsersSid = new Sid("S-1-5-11");
            Accounts.Sid.LookupAccountName(Name, authUsersSid, out acctName, out domName, out peUse);
            Accounts.Sid termServerUsersSid = new Sid("S-1-5-13");
            Accounts.Sid.LookupAccountName(Name, termServerUsersSid, out acctName, out domName, out peUse);
            Accounts.Sid builtinAdminSid = new Sid("S-1-5-32-544");
            Accounts.Sid.LookupAccountName(Name, builtinAdminSid, out acctName, out domName, out peUse);


            return isOk;
        }

        private int loadRegistryProperties()
        {
            int numWarnings = 0;

            


            return numWarnings;
        }


 
        

        private int loadProperties()
        {
            // Use WMI to load Product, Version, Build, ServicePack, SystemDrive
            //    TimeZoneOffset, Domain and DomainRole properties.
            int numWarnings = 0;
            if( Bind() )
            {
                // Don't abort if loadWmiProperties() fails
                numWarnings = loadWmiProperties();

                bool isOk = true;
                // Load the wellknown sids, so that they can be processed for
                // group membership enumerations.
                isOk = loadWellknownSids();

                // If the machine is not a DC, get its computer SID.
                if (isOk && !isDomainController(m_DomainRole))
                {
                    // Lookup account name to get the SID.
                    SID_NAME_USE peUse;
                    string refDom;
                    byte[] bSid;

                    if (Core.Interop.Authorization.LookupAccountName(m_Name,
                                   m_Name, out bSid,
                                   out refDom, out peUse) || peUse == SID_NAME_USE.SidTypeDomain)
                    {
                        m_ComputerSid = new Sid(bSid);
                    }
                    else
                    {
                        logX.loggerX.Error("ERROR - failed to lookup the computer SID for ", m_Name);
                        isOk = false;
                        numWarnings++;
                    }
                }

                // If the computer is a DC, add builtin domain information to the collection.
                if (isOk && isDomainController(m_DomainRole))
                {
                    Domain domain = DomainCollection.GetDomain(Name, m_DomainName, true, m_BindAccount, m_BindPassword);
                    if (domain != null)
                    {
                        DomainCollection.AddBuiltInDomain(domain);
                    }
                    else
                    {
                        logX.loggerX.Error("ERROR - failed to get the parent domain of the server, ", m_Name);
                        isOk = false;
                        numWarnings++;
                    }
                }

                Unbind();
            }
            else
            {
                logX.loggerX.Error("ERROR - bind to server failed");
                numWarnings++;
            }

            return numWarnings;
        }

        #endregion

        public delegate void WriteActivityToRepositoryDelegate(string activityType,
                                                                string eventcode,
                                                                string description);
        WriteActivityToRepositoryDelegate WriteApplicationActivityToRepository;

        #region Ctors
        public Server(
                string name,
                string bindAcct,
                string bindPassword,
                WriteActivityToRepositoryDelegate WriteAppActivityToRepositoryParam
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(name));
            Debug.Assert(string.IsNullOrEmpty(bindAcct) || (!string.IsNullOrEmpty(bindAcct) && !string.IsNullOrEmpty(bindPassword)));
            WriteApplicationActivityToRepository = WriteAppActivityToRepositoryParam;
            m_Name = name;
            try
            {
                m_Name = GetActiveComputerName(name);
                logX.loggerX.Info(string.Format(@"Active Computer Name: {0}", m_Name));
            }
            catch (Exception e)
            {
                logX.loggerX.Warn(String.Format("Warning - failed to get active computer name for {0}. Exception: {1}", name, e.Message));
                m_numWarnings++;
            }
            setBindCredentials(bindAcct, bindPassword);
            m_numWarnings = loadProperties();
            m_IsValid = (m_numWarnings >= 0);
            if (WriteAppActivityToRepositoryParam != null && m_numWarnings > 0)
            {
                WriteApplicationActivityToRepository("Warning", "Warning", "failed to load server properties");
            }
        }
        #endregion

        #region Properties
        public int NumWarnings
        {
            get { return m_numWarnings; }
        }

        public bool IsValid
        {
            get { return m_IsValid; }
        }
        public bool IsAdmin
        {
            get { return m_IsAdmin; }
        }
        public string Name
        {
            get { return m_Name; }
        }
        public string Product
        {
            get { return m_Product; }
        }
        public string ServicePack
        {
            get { return m_ServicePack; }
        }
        public string Version
        {
            get { return m_Version; }
        }
        public string BuildNumber
        {
            get { return m_BuildNumber; }
        }
        public string SystemDrive
        {
            get { return m_SystemDrive; }
        }
        public int TimeZoneOffset
        {
            get { return m_TimeZoneOffset; }
        }
        public string DomainName
        {
            get { return m_DomainName; }
        }
        public string BindDomain
        {
            get { return m_BindDomain; }
        }
        public string BindUser
        {
            get { return m_BindUser; }
        }
        public string BindPassword
        {
            get { return m_BindPassword; }
        }
        public OSDomainRole DomainRole
        {
            get { return m_DomainRole; }
        }
        public Sid ComputerSid
        {
            get { return m_ComputerSid; }
        }
        public bool IsLocalComputer
        {
            get 
            {
                return isLocalComputer(m_Name);
            }
        }
        public bool IsDomainController
        {
            get { return isDomainController(m_DomainRole); }   
        }

        #endregion

        #region Methods

        /// <summary>
        /// Bind to the remote computer, using binding credentials specified.
        /// This function does a net use on IPC$ share.  If no credentials were 
        /// specified then it just returns true.
        /// </summary>
        /// <returns>Bind status</returns>
        static public bool Bind(
                string computer,
                string user,
                string domain,
                string password
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(computer));
            Debug.Assert(!string.IsNullOrEmpty(user));
            Debug.Assert(!string.IsNullOrEmpty(domain));
            Debug.Assert(!string.IsNullOrEmpty(password));

            // If no computer, or no credentials return okay.
            if (string.IsNullOrEmpty(computer) 
                || string.IsNullOrEmpty(user) || string.IsNullOrEmpty(domain) 
                || string.IsNullOrEmpty(password)
                )
            {
                return true;
            }

            // Create IPC$ share path.
            string ipcShare = Path.WhackPrefixComputer(computer);
            ipcShare += @"\IPC$";

            // Set use_info2 structure.
            USE_INFO_2 ui2 = new USE_INFO_2();
            ui2.ui2_remote = ipcShare;
            ui2.ui2_password = password;
            ui2.ui2_asg_type = (uint)AsgType.USE_IPC;
            ui2.ui2_username = user;
            ui2.ui2_domainname = domain;

            logX.loggerX.Verbose(string.Format("Binding to {0} for user {1}//{2}", ipcShare, domain, user));
            // Bind IPC.
            uint parmError;
            uint rc = NetworkMgmt.NetUseAdd(null, 2, ref ui2, out parmError);
            if (rc != Interop.Win32Errors.ERROR_SUCCESS)
            {
                //if (rc == Interop.Win32Errors.ERROR_SESSION_CREDENTIAL_CONFLICT)
                //{
                //    IntPtr ui2Array;
                //    uint EntriesRead, TotalEntries, ResumeHandle;
                //    NetworkMgmt.NetUseEnum(null, 2, out ui2Array, NetworkMgmt.MAX_PREFERRED_LENGTH, out EntriesRead,
                //                           out TotalEntries, out ResumeHandle);
                //    for (int x = 0; x < EntriesRead; x++)
                //    {
                //        USE_INFO_2 ui = (USE_INFO_2)Marshal.PtrToStructure(new IntPtr(ui2Array.ToInt32() + x * Marshal.SizeOf(typeof(USE_INFO_2))), typeof(USE_INFO_2));
                //    }
                //}
                // Bind failed, so do an unbind and retry.
                logX.loggerX.Error("ERROR - bind to server failed so retrying after an unbind, rc = ", rc);
                Unbind(computer);
                rc = NetworkMgmt.NetUseAdd(null, 2, ref ui2, out parmError);
                if (rc != Interop.Win32Errors.ERROR_SUCCESS)
                {
                    logX.loggerX.Error("ERROR - retry bind to server failed, rc = ", rc);
                }
            }
            return rc == Interop.Win32Errors.ERROR_SUCCESS;
        }

        public bool RealBind()
        {
            // If no binding credentials are specified, bail out no binding is required.
            if (!isCredentialSet || IsLocalComputer)
            {
                return true;
            }

            return Bind(m_Name, m_BindUser, m_BindDomain, m_BindPassword);
        }

        public bool Bind()
        {
            // If no binding credentials are specified, bail out no binding is required.
            if (!isCredentialSet || IsLocalComputer) 
            {
                return true;
            }
            return true;
//            return Bind(m_Name, m_BindUser, m_BindDomain, m_BindPassword);
        }

        /// <summary>
        /// Unbind from earlier bind.
        /// </summary>
        public static void Unbind(string computer)
        {
            Debug.Assert(!string.IsNullOrEmpty(computer));

            // If no computer, or no credentials or local computer
            // return okay.
            if (string.IsNullOrEmpty(computer))
            {
                return;
            }

            // Create IPC$ share path.
            string ipcShare = Path.WhackPrefixComputer(computer);
            ipcShare += @"\IPC$";

            // Unbind IPC.
            uint rc = NetworkMgmt.NetUseDel(null, ipcShare, ForceCond.USE_LOTS_OF_FORCE);
            if (rc != Interop.Win32Errors.ERROR_SUCCESS)
            {
                logX.loggerX.Error("ERROR - unbind from server failed, rc = ", rc);
            }
        }

        public void RealUnbind()
        {
            // If no binding credentials are specified, bail out no unbinding is required.
            if (!isCredentialSet || IsLocalComputer)
            {
                return;
            }
            Unbind(m_Name);
        }

        public void Unbind()
        {
            // If no binding credentials are specified, bail out no unbinding is required.
            if (!isCredentialSet || IsLocalComputer)
            {
                return;
            }
//            Unbind(m_Name);
        }

        public int ResolveGroupMembers(
                List<Account> groupList,
                out Dictionary<Account, List<Account>> groupMemberships,
                out List<string> wellKnownGroups
            )
        {
            int numTotalWarnings = 0;
            // Init the group memberships object.
            groupMemberships = new Dictionary<Account, List<Account>>();
            wellKnownGroups = new List<string>();
            try
            {
                using (logX.loggerX.DebugCall())
                {
                    // Bind to the remote server.
                    if (Bind())
                    {
                        int numLoopWarnings = 0;
                        // Process the top level groups and get their members.
                        int nGroupNumberForDebugMessage = 1;
                        foreach (Account topGroup in groupList)
                        {
                            logX.loggerX.Verbose(
                                string.Format("Processing Initial Group {0} of {1}", nGroupNumberForDebugMessage++,
                                              groupList.Count));
                            DateTime st = DateTime.Now;
                            Account group;
                            List<Account> members;
                            numLoopWarnings = getGroupMembers(topGroup.SID, out group, out members, ref wellKnownGroups);
                            numTotalWarnings += numLoopWarnings;
                            if (group != null)
                            {
                                // Update the dictionary.
                                if (members == null)
                                {
                                    members = new List<Account>();
                                    group.AccountStatus = Account.AccountStatusEnum.Account_Suspected;
                                    logX.loggerX.Error("ERROR : failed to load <", group.SamPath, "> group members");
                                }
                                else
                                {
                                    if (numLoopWarnings > 0)
                                    {
                                        group.AccountStatus = Account.AccountStatusEnum.Account_Suspected;
                                    }
                                }

                                groupMemberships.Add(group, members);

                                // Update metrics.
                                DateTime end = DateTime.Now;
                                TimeSpan elapsed = end - st;
                                logX.loggerX.Verbose("Time to process ", group.SamPath, " is ", elapsed.ToString(),
                                                     " SID: ",
                                                     group.SID.ToString());
                            }
                            else
                            {
                                members = new List<Account>();
                                topGroup.AccountStatus = Account.AccountStatusEnum.Account_Suspected;
                                groupMemberships.Add(topGroup, members);

                                logX.loggerX.Error("ERROR : failed to read <" + topGroup.SamPath + "> AD group object");
                            }
                        }
                        logX.loggerX.Verbose(
                            string.Format("Found {0} Top Level Groups in initial group list.", groupMemberships.Count));
                        logX.loggerX.Verbose(string.Format("Adding Second-Level groups from top level groups."));
                        // Create a stack of all group members, not in the dictionary.
                        Stack<Account> memberGroupStack = new Stack<Account>();
                        foreach (KeyValuePair<Account, List<Account>> kvp in groupMemberships)
                        {
                            foreach (Account topMember in kvp.Value)
                            {
                                logX.loggerX.Verbose(
                                    string.Format("From Top Group, found member {0} from {1}.", topMember.SamPath, kvp.Key.SamPath));
                                if (topMember.IsNonDistributionGroup && !groupMemberships.ContainsKey(topMember))
                                {
                                    logX.loggerX.Verbose(
                                        string.Format("From Top Group, adding group {0} from {1}.", topMember.SamPath, kvp.Key.SamPath));
                                    memberGroupStack.Push(topMember);
                                }
                            }
                        }
                        logX.loggerX.Verbose(
                            string.Format("Top Level Groups contained {0} second level groups", memberGroupStack.Count));

                        // Process all the members in the stack.
                        logX.loggerX.Verbose(string.Format("Starting Processing Member Group Stack Loop"));
                        while (memberGroupStack.Count != 0)
                        {
                            // Pop stack group account.
                            Account group = memberGroupStack.Pop();
                            Debug.Assert(group.IsNonDistributionGroup);

                            // If group has not been enumerated, get its members.
                            if (!groupMemberships.ContainsKey(group))
                            {
                                int numWarnings = 0;
                                List<Account> members = null;
                                DateTime st = DateTime.Now;
                                numWarnings = getGroupMembers(group, out members, ref wellKnownGroups);
                                numTotalWarnings += numWarnings;
                                DateTime end = DateTime.Now;
                                TimeSpan elapsed = end - st;
                                logX.loggerX.Verbose("Time to process (group stack loop)", group.SamPath, " is ",
                                                     elapsed.ToString(),
                                                     " SID: ", group.SID.ToString());
                                if (members != null)
                                {
                                    if (numWarnings > 0)
                                    {
                                        group.AccountStatus = Account.AccountStatusEnum.Account_Suspected;
                                    }

                                    // Add the group and its members to the dictionary.
                                    groupMemberships.Add(group, members);

                                    // Add each group member that has not been already
                                    // enumerated to the stack.
                                    foreach (Account member in members)
                                    {
                                        logX.loggerX.Verbose(
                                            string.Format("Found sub-member {0} from group {1}.", member.SamPath, group.SamPath));
                                        if (member.IsNonDistributionGroup && !groupMemberships.ContainsKey(member))
                                        {
                                            logX.loggerX.Verbose(
                                                string.Format("Adding sub-group {0} from group {1}.", member.SamPath,
                                                              group.SamPath));
                                            memberGroupStack.Push(member);
                                        }
                                    }
                                }
                                else
                                {
                                    members = new List<Account>();
                                    group.AccountStatus = Account.AccountStatusEnum.Account_Suspected;
                                    groupMemberships.Add(group, members);
                                }
                            }
                        }
                        logX.loggerX.Verbose(string.Format("Done Processing Member Group Stack Loop"));


                        Unbind();
                    }
                    else
                    {
                        logX.loggerX.Error("ERROR - bind to server ", Name, " failed");
                    }

                    if (groupList.Count > 0 && groupMemberships.Count == 0)
                    {
                        foreach (Account topGroup in groupList)
                        {
                            List<Account> members = new List<Account>();
                            topGroup.AccountStatus = Account.AccountStatusEnum.Account_Suspected;
                            groupMemberships.Add(topGroup, members);
                        }
                        numTotalWarnings++;
                    }
                }
            }
            catch(Exception ex)
            {
                logX.loggerX.Error(string.Format("ERROR - Unexpected Error Resolving Group Memberships: {0}", ex.Message));
            }

            return numTotalWarnings;
        }

        public enum ServerAccess
        {
            OK,
            ERROR_CONNECT,
            ERROR_RPC,
            ERROR_DCOM,
            ERROR_OTHER
        }

        public static ServerAccess CheckServerAccess(string computer, string account, string password, out string errorMessage)
        {
            return CheckServerAccess(computer, account, password, out errorMessage, false);
        }

        public static ServerAccess CheckServerAccess(string computer, string account, string password, out string errorMessage, bool forceLocal)
        {
            ServerAccess retCode = ServerAccess.OK;
            errorMessage = string.Empty;
            try
            {

                string domain = string.Empty;
                string user = string.Empty;
                Path.SplitSamPath(account, out domain, out user);
                
                // First see if we can bind to target computer

                // Create IPC$ share path.
                string ipcShare = Path.WhackPrefixComputer(computer);
                ipcShare += @"\IPC$";

                // Set use_info2 structure.
                USE_INFO_2 ui2 = new USE_INFO_2();
                ui2.ui2_remote = ipcShare;
                ui2.ui2_password = password;
                ui2.ui2_asg_type = (uint)AsgType.USE_IPC;
                ui2.ui2_username = user;
                ui2.ui2_domainname = domain;

                logX.loggerX.Verbose(string.Format("Binding to {0} for user {1}//{2}", ipcShare, domain, user));
                // Bind IPC.
                uint parmError;
                uint rc = NetworkMgmt.NetUseAdd(null, 2, ref ui2, out parmError);
                if (rc != Interop.Win32Errors.ERROR_SUCCESS)
                {
                    errorMessage =  NativeMethods.GetErrorMessage((int)rc);
                    retCode = ServerAccess.ERROR_CONNECT;
                }
                else
                {
                    rc = NetworkMgmt.NetUseDel(null, ipcShare, ForceCond.USE_LOTS_OF_FORCE);
                    if (rc != Interop.Win32Errors.ERROR_SUCCESS)
                    {
                        logX.loggerX.Error("ERROR - unbind from server failed, rc = ", rc);
                    }
                    
                }

                if (retCode == ServerAccess.OK)
                {
                    ConnectionOptions options = null;

                    if (!forceLocal)
                    {
                        if (!isLocalComputer(computer))
                        {
                            options = new ConnectionOptions();
                            options.Authority = Constants.AuthorityNTLM + domain;
                            options.Username = user;
                            options.Password = password;
                        }
                    }

                    // Create management scope string.
                    StringBuilder scopeStr = null;
                    scopeStr = new StringBuilder();
                    scopeStr.Append(Path.WhackPrefixComputer(computer));
                    scopeStr.Append(Constants.Cimv2Root);

                    // Create management scope and connect.
                    ManagementScope scope = null;
                    if (options != null)
                    {
                        scope = new ManagementScope(scopeStr.ToString(), options);
                    }
                    else
                    {
                        scope = new ManagementScope(scopeStr.ToString());
                    }
                    scope.Connect();
                }
            }
            catch (Exception ex)
            {
                //                if (ex.HResult == -2147024891)

                if (!forceLocal)    // don't retry as local if we're currently retrying
                {
                    ManagementException ManEx = ex as ManagementException;
                    if (ManEx != null)
                    {
                        if (ManEx.ErrorCode == ManagementStatus.LocalCredentials)
                        {
                            retCode = CheckServerAccess(computer, account, password, out errorMessage, true); // retry with forceLocal = true
                            return retCode;
                        }
                    }
                }
                retCode = ServerAccess.ERROR_OTHER;
                errorMessage = ex.Message;
            }            

            return retCode;
        }

        #endregion
    }
}
