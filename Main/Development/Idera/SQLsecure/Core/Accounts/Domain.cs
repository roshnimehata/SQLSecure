/******************************************************************
 * Name: Domain.cs
 *
 * Description: Encapsulates the NT domain object.
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

using Idera.SQLsecure.Core.Interop;
using Idera.SQLsecure.Core.Logger;

namespace Idera.SQLsecure.Core.Accounts
{
    internal static class DomainCollection
    {
        #region Fields
        private static LogX logX = new LogX("Idera.SQLsecure.Core.Accounts.ComainCollection");

        private static Dictionary<string, Domain> m_DictionaryFlat = new Dictionary<string, Domain>(StringComparer.CurrentCultureIgnoreCase);
        private static Dictionary<string, Domain> m_DictionaryDns = new Dictionary<string, Domain>(StringComparer.CurrentCultureIgnoreCase);

        #endregion

        #region Helpers

        private static DomainType getDomainType(Interop.DOMAIN_CONTROLLER_INFO dcInfo)
        {
            bool isDomainDNS = ((dcInfo.Flags & (uint) Interop.DomainControllerInfoFlags.DS_DNS_DOMAIN_FLAG)
                                        == (uint) Interop.DomainControllerInfoFlags.DS_DNS_DOMAIN_FLAG) 
                               && dcInfo.DomainName != null && dcInfo.DomainName.Length > 0;
            bool isForestDNS = ((dcInfo.Flags & (uint) Interop.DomainControllerInfoFlags.DS_DNS_FOREST_FLAG)
                                        == (uint) Interop.DomainControllerInfoFlags.DS_DNS_FOREST_FLAG)
                               && dcInfo.DnsForestName != null && dcInfo.DnsForestName.Length > 0;
            if (isDomainDNS || isForestDNS)
            {
                return DomainType.AD;
            }
            else
            {
                return DomainType.SAM;
            }
        }

        #endregion

        #region Methods

        public static Domain GetDomainFromCache(string domainName, bool isNameFlat)
        {
            Domain domain = null;
            if(isNameFlat)
            {
                m_DictionaryFlat.TryGetValue(domainName, out domain);
            }
            else
            {
                m_DictionaryDns.TryGetValue(domainName, out domain);
            }
            return domain;
        }
        /// <summary>
        /// Gets the domain object for the specified domain.  Attempt
        /// is first made to get the object from the cache, if its not
        /// found in the cache then a new object is created, added to the
        /// cache and returned.
        /// </summary>
        /// <param name="computer"></param>
        /// <param name="domainName"></param>
        /// <param name="isNameFlat">domain name format is flat or DNS</param>
        /// <returns></returns>
        public static Domain GetDomain(
                string computer,
                string domainName,
                bool isNameFlat,
                string userName,
                string password
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(computer));
            Debug.Assert(!string.IsNullOrEmpty(domainName));

            // Validate input.
            if (string.IsNullOrEmpty(computer) || string.IsNullOrEmpty(domainName))
            {
                logX.loggerX.Error("ERROR - computer or domain name not specified");
                return null;
            }

            // Check the dictionary to see if the domain is already
            // in the dictionary.
            Domain domain = null;
            if(isNameFlat)
            {
                m_DictionaryFlat.TryGetValue(domainName, out domain);
            }
            else
            {
                m_DictionaryDns.TryGetValue(domainName, out domain);
            }

            // If domain not in dictionary then create a domain object
            // and add it to the dictionary.   
            if (domain == null)
            {
                // Call DsGetDcName to get the flat domain info.
                Interop.DOMAIN_CONTROLLER_INFO dcInfo;
                uint nameInFlag = isNameFlat ? (uint)DsGetDcFlags.DS_IS_FLAT_NAME : (uint)DsGetDcFlags.DS_IS_DNS_NAME;
                uint flags = ((uint)DsGetDcFlags.DS_RETURN_FLAT_NAME | nameInFlag);
                uint rc = Interop.DS.DsGetDcName(computer, domainName, flags, out dcInfo);
                if (rc != Interop.Win32Errors.ERROR_SUCCESS)
                {
                    // Try get DC information using other type of name.
                    nameInFlag = !isNameFlat ? (uint)DsGetDcFlags.DS_IS_FLAT_NAME : (uint)DsGetDcFlags.DS_IS_DNS_NAME;
                    flags = ((uint)DsGetDcFlags.DS_RETURN_FLAT_NAME | nameInFlag);
                    rc = Interop.DS.DsGetDcName(computer, domainName, flags, out dcInfo);
                    if (rc != Interop.Win32Errors.ERROR_SUCCESS)
                    {
                        logX.loggerX.Error("ERROR - Failed to get flat name DC info for the domain", domainName);
                        return null;
                    }
                }

                // Get the flat name and determine domain type.
                string domFlatName = dcInfo.DomainName;
                string dcFlatName = dcInfo.DomainControllerName;
                DomainType domType = getDomainType(dcInfo);

                // If the domain is AD, then get the dns names.
                string domDnsName, dcDnsName;
                if (domType == DomainType.AD)
                {
                    // Get the dns versions of the name.
                    flags = ((uint)DsGetDcFlags.DS_RETURN_DNS_NAME | nameInFlag);
                    rc = Interop.DS.DsGetDcName(computer, domainName, flags, out dcInfo);
                    if (rc != Interop.Win32Errors.ERROR_SUCCESS)
                    {
                        logX.loggerX.Error("ERROR - Failed to get DNS name DC info for the domain ", domainName);
                        return null;
                    }

                    // Get dns name.
                    domDnsName = dcInfo.DomainName;
                    dcDnsName = dcInfo.DomainControllerName;
                }
                else
                {
                    domDnsName = domFlatName;
                    dcDnsName = dcFlatName;
                }

                // Create the doman object and add to the dictionary.
                domain = new Domain(domFlatName, domDnsName, domType, dcFlatName, dcDnsName, userName, password);
                m_DictionaryFlat.Add(domain.FlatName, domain);
                m_DictionaryDns.Add(domain.DnsName, domain);
            }

            return domain;
        }

        /// <summary>
        /// When the target server is a DC, then the special builtin
        /// SID's domain is added to the map.
        /// </summary>
        /// <param name="domain"></param>
        public static void AddBuiltInDomain(Domain domain)
        {
            Debug.Assert(domain != null);

            if (domain != null)
            {
                // Note: do not add the same domain back into the map,
                // its real name.   Because the domain is only created
                // with a GetDomain call that adds the domain to the maps
                // with real names.
                m_DictionaryFlat.Add("BUILTIN", domain);
                m_DictionaryDns.Add("BUILTIN", domain);
            }
            else
            {
                logX.loggerX.Error("ERROR - builtin domain object is null");
            }
        }

        #endregion
    }

    /// <summary>
    /// Encapsulates Windows domain object.
    /// </summary>
    public class Domain
    {
        #region Fields
        private static LogX logX = new LogX("Idera.SQLsecure.Core.Accounts.Domain");

        private string m_FlatName;
        private string m_DnsName;
        private DomainType m_Type;
        private string m_FlatDC;
        private string m_DnsDC;
        private string m_defaultNamingContext;
        private string m_password;
        private string m_userName;
        #endregion
        
        #region Helpers

        #endregion
        
        #region Ctors
        public Domain (
                string flatName,
                string dnsName,
                DomainType type,
                string flatDc,
                string dnsDc,
            string userName,
            string password
            )
        {
            m_FlatName = flatName;
            m_DnsName = dnsName;
            m_Type = type;
            m_FlatDC = Path.NonWhackPrefixComputer(flatDc);
            m_DnsDC = Path.NonWhackPrefixComputer(dnsDc);
            m_userName = userName;
            m_password = password;

            if(type == DomainType.AD)
            {
                try
                {
                    DirectoryEntry de = new DirectoryEntry("LDAP://" + m_FlatDC + "/RootDSE", userName, password);
                    m_defaultNamingContext = (string)de.Properties["defaultNamingContext"].Value;
                }
                catch (Exception ex)
                {
                    string Message = ex.Message;
                }
            }
        }  
        #endregion
        
        #region Properties
        public string FlatName
        {
            get { return m_FlatName; }
        }
        public string DnsName
        {
            get { return m_DnsName; }
        }
        public DomainType Type
        {
            get { return m_Type; }
        }
        public string FlatDC
        {
            get { return m_FlatDC; }
        }
        public string DnsDC
        {
            get { return m_DnsDC; }
        }
        public string DefaultNamingContext
        {
            get { return m_defaultNamingContext; }
        }
        public string Password
        {
            get { return m_password; }
        }
        public string User
        {
            get { return m_userName; }
        }
        public string AdsiPath
        {
            get
            {
                string path = string.Empty;
                if (Type == DomainType.AD)
                {
                    path = Path.DomainDnsNameToLdapPath(m_DnsName, m_FlatDC);
                }
                else if (Type == DomainType.SAM)
                {
                    path = Path.MakeWinntPath(FlatName,null);
                }
                else
                {
                    Debug.Assert(false);
                }

                return path;
            }
        }
        public string LdapRangeEnumPrefix
        {
            get
            {
                StringBuilder bldr = new StringBuilder();
                bldr.Append(Constants.LdapPrefix);
                bldr.Append(FlatDC);
                bldr.Append(Constants.SingleSlashStr);
                return bldr.ToString();
            }
        }
        #endregion
        
        #region Methods
        public DirectoryEntry GetDomainDirectoryEntry(
                string user,
                string password
            )
        {
            DirectoryEntry de = new DirectoryEntry(AdsiPath, user, password, 
                                    System.DirectoryServices.AuthenticationTypes.Secure);
            return de;
        }

        public DirectoryEntry GetDirectoryEntry(
                string user,
                string password,
                Sid sid,
                string account
            )
        {
            Debug.Assert(sid.IsValid);
            Debug.Assert(!string.IsNullOrEmpty(account));

            // Get directoyr entry, the processing is based on the domain type.
            DirectoryEntry dirEntry = null;
            try
            {
                switch (Type)
                {
                    case DomainType.AD:
                        logX.loggerX.Verbose("Get Directory Entry for domain type AD");
                        // Get the domain directory entry.
                        using (DirectoryEntry domDirEntry = new DirectoryEntry(AdsiPath, user, password,
                                            System.DirectoryServices.AuthenticationTypes.Secure))
                        {
                            if (domDirEntry != null)
                            {
                                // Now do a search for the object based on the SID.
                                using (DirectorySearcher dirSearch = new DirectorySearcher())
                                {
                                    // Setup the directory searcher object.
                                    dirSearch.SearchRoot = domDirEntry;
                                    dirSearch.SearchScope = SearchScope.Subtree;
                                    dirSearch.Filter = sid.LdapFilter;

                                    // Find the result and get the directory entry based
                                    // on the object path.
                                    SearchResult result = dirSearch.FindOne();
                                    if (result != null)
                                    {
                                        dirEntry = new DirectoryEntry(result.Path, user, password,
                                                System.DirectoryServices.AuthenticationTypes.Secure);
                                    }
                                    else
                                    {
                                        logX.loggerX.Error("ERROR - <", sid.ToString(), "> directory entry not found");
                                        dirEntry = null;
                                    }
                                }
                            }
                            else
                            {
                                logX.loggerX.Error("ERROR - failed to get directory entry for the domain of the sid, ", m_FlatName);
                                dirEntry = null;
                            }
                        }
                        break;
                    case DomainType.SAM:
                        logX.loggerX.Verbose("Get Directory Entry for domain type SAM");
                        dirEntry = new DirectoryEntry(Path.MakeWinntPath(FlatName, account), user, password, AuthenticationTypes.Secure);
                        break;
                    default:
                        logX.loggerX.Error("ERROR - unknown domain type");
                        Debug.Assert(false);
                        break;
                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error("ERROR - exception raised when getting directory entry for the sid <",
                                    sid.ToString(), ">, exception: ", ex.Message);
                if (dirEntry != null)
                {
                    dirEntry.Dispose();
                    dirEntry = null;
                }
            }

            return dirEntry;
        }

        public DirectoryEntry GetDirectoryEntry(
                string user,
                string password,
                string dn
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(dn));

            // Get directory entry, the processing is based on the domain type.
            DirectoryEntry dirEntry = null;
            try
            {
                // Get the dir entry object.
                dirEntry =
                    new DirectoryEntry(dn, user, password, System.DirectoryServices.AuthenticationTypes.Secure);

                // When a member server group or NT4 domain group is enumerated, the 
                // path returned is a WinNT path.  But the member can be from an AD domain.   
                // This path wil cause a problem, so we need to get an LDAP path.  To do this
                // we get the SID of the object, and find the dir entry object based on the SID.
                if (Type == DomainType.AD && Path.IsWinntPath(dn))
                {
                    // Get the SID of the object.
                    byte[] bSid = (byte[]) dirEntry.Properties["objectSID"].Value;
                    Sid sid = new Sid(bSid);
                    if (sid.IsValid)
                    {
                        dirEntry = GetDirectoryEntry(user, password, sid, dirEntry.Name);
                    }
                    else
                    {
                        logX.loggerX.Error("ERROR - SID is invalid");
                        dirEntry.Dispose();
                        dirEntry = null;
                    }
                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error("ERROR - exception raised when getting directory entry, ", dn, ", ", ex.Message);
                if (dirEntry != null)
                {
                    dirEntry.Dispose();
                    dirEntry = null;
                }
            }

            return dirEntry;
        }

        #endregion
    }
          
}
