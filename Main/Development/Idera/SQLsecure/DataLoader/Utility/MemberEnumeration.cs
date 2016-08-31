using Idera.SQLsecure.Core.Accounts;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Security.Principal;
using System.Text;

namespace Idera.SQLsecure.Collector.Utility
{
    public class MemberEnumeration
    {
        private List<TrustRelationshipInformation> _trustedDomainsForest;
        //public List<string> DomainNames { get; private set; }
        public WindowsImpersonationContext TargetImpersonationContext;

        public MemberEnumeration()
        {

        }

        public void EnterImpersonation(string fullName, string password)
        {
            TargetImpersonationContext = Impersonation.GetCurrentIdentity(fullName, password)
                .Impersonate();
        }

        public void LeaveImpersonation()
        {
            TargetImpersonationContext.Undo();
        }

        public void GetTrustedDomainsAndForests()
        {
            List<TrustRelationshipInformation> result = new List<TrustRelationshipInformation>();
            try
            {
                //Getting Forest trusts
                TrustRelationshipInformationCollection forestTrusts = Forest.GetCurrentForest().GetAllTrustRelationships();
                //Getting Domain trusts
                TrustRelationshipInformationCollection domainTrusts = System.DirectoryServices.ActiveDirectory.Domain.GetCurrentDomain().GetAllTrustRelationships();

                foreach (TrustRelationshipInformation trust in forestTrusts)
                    result.Add(trust);

                foreach (TrustRelationshipInformation trust in domainTrusts)
                    result.Add(trust);

#if DEBUG
                foreach (TrustRelationshipInformation trust in result)
                    Console.WriteLine("From: {0} to {1}. Direction: {2}",
                        trust.SourceName, trust.TargetName, trust.TrustDirection);
#endif
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            _trustedDomainsForest = result;
        }
        public void GetTrustedDomainsAndForests(string domainAdminName, string domainAdminPass)
        {
            EnterImpersonation(domainAdminName, domainAdminPass);
            GetTrustedDomainsAndForests();
            LeaveImpersonation();
        }

        /// <summary>
        /// Enumerates members of given group
        /// </summary>
        /// <param name="account">Account representing group</param>
        /// <returns>Dictionary: Group Account - key, List of members - value</returns>
        public List<Account> EnumerateADGroupMembers(Account account)
        {
            List<Account> result = new List<Account>();
            string[] _domainLogin = account.SamPath.Split('\\');
            string _ldapPath = GetGCPath(GetFQDNbyName(_domainLogin[0]));

            result = GetMembersOfGroup(_domainLogin[1], _ldapPath);


            return result;
        }

        /// <summary>
        /// Provide the friendly name of the group to get all members of the group.
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public List<Account> GetMembersOfGroup(string groupName, string _ldapPath)
        {
            List<Account> members = new List<Account>();

            using (DirectoryEntry directoryEntry = new DirectoryEntry(_ldapPath))
            {
                try
                {
                    //var groupDistinguishedName = GetGroupDistinguishedName(directoryEntry, groupName);
                    members = new List<Account>(GetMembersOfGroup(directoryEntry, groupName));
                }
                catch (Exception ex)
                {
                    //global::System.Windows.Forms.MessageBox.Show("Group: " + groupName + " not found");
                    Console.WriteLine("Failed to query LDAP users {0} on {1}. {2}", groupName, _ldapPath, ex.Message);
                }
            }
            return members;
        }
        private IEnumerable<Account> GetMembersOfGroup(DirectoryEntry directoryEntry, string groupDistinguishedName)
        {
            List<Account> members = new List<Account>();

            if (string.IsNullOrEmpty(groupDistinguishedName))
            {
                throw new Exception("Group name not provided. Cannot look for group members.");
            }

            string filter = string.Format("(&(objectClass=user)(memberof={0}))", groupDistinguishedName);

            using (DirectorySearcher ds = new DirectorySearcher(directoryEntry, filter))
            {
                SetupDefaultPropertiesOnDirectorySearcher(ds);
                // get all members in a group
                try
                {
                    foreach (SearchResult result in ds.FindAll())
                    {
                        Account _member;
                        Account.CreateAccount(null, result.GetDirectoryEntry(), out _member);
                        members.Add(_member);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return members;
        }
        private void SetupDefaultPropertiesOnDirectorySearcher(DirectorySearcher searcher)
        {
            // allow us to use references to other active dir domains.
            searcher.ReferralChasing = ReferralChasingOption.All;
        }
        private string GetGroupDistinguishedName(DirectoryEntry directoryEntry, string groupName)
        {
            string distinguishedName = "";

            string filter = string.Format("(&(objectClass=group)(name={0}))", groupName);
            string[] propertiesToLoad = new string[] { "distinguishedName" };

            using (DirectorySearcher ds = new DirectorySearcher(directoryEntry, filter, propertiesToLoad))
            {
                SetupDefaultPropertiesOnDirectorySearcher(ds);

                SearchResult result = ds.FindOne();
                if (result != null)
                {
                    distinguishedName = result.Properties["distinguishedName"][0].ToString();
                }
            }

            return distinguishedName;
        }

        /// <summary>
        /// Creating Global Catalog Paths. Need to be FQDN.
        /// </summary>
        /// <param name="domainName">domain.name</param>
        /// <returns>GC://dc=domain,dc=name</returns>
        static string GetGCPath(string domainName)
        {
            string result = string.Empty;

            string[] distinguishedNames = domainName.Split('.');

            StringBuilder sb = new StringBuilder();
            sb.Append("LDAP://");

            for (int i = 0; i < distinguishedNames.Length; i++)
            {
                sb.Append("dc=" + distinguishedNames[i]);

                if (i != distinguishedNames.Length - 1)
                    sb.Append(",");
            }
            result = sb.ToString();

            return result;
        }
        private string GetFQDNbyName(string name)
        {
            string result = string.Empty;
            if (_trustedDomainsForest != null && _trustedDomainsForest.Count > 0)
            {

                foreach (TrustRelationshipInformation domain in _trustedDomainsForest)
                {
                    if (domain.TargetName.ToLower().Contains(name.ToLower()))
                    {
                        result = domain.TargetName;
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine("[GetFQDNbyName]\t No trusted domains or forests.");
            }

            return result;
        }
    }
}
