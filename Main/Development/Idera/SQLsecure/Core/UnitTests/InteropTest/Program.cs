using System;
using System.Collections.Generic;
using System.Text;

using Idera.SQLsecure.Core.Interop;
using Idera.SQLsecure.Core.Accounts;
using System.Security.Principal;

namespace InteropTest
{
    class Program
    {
        static void testPath()
        {
            // Construct various paths.
            string DN = "cn=test1, cn=users, dc    =     secure2k-dom,    dc    =dev, dc=         hq";
            string LdapPath = "LDAP://cn=test1, cn=users, dc=secure2k-dom, dc=dev, dc=hq";
            string WinNT = @"WinNT://dom/group";
            string Computer = @"WinNT://dom/computer/group";

            Path dnPath = Path.LdapPathFromDN(DN);
            Path ldapPath = new Path(LdapPath);
            Path winNtPath = new Path(WinNT);
            Path compPath = new Path(Computer);

            string dnDom = dnPath.DomainName;
            string ldapDom = ldapPath.DomainName;
            string winntDom = winNtPath.DomainName;
            string compDom = compPath.DomainName;
        }

        static void Main(string[] Args)
        {
            testPath();
            /*
            string computerName = null;
            string domainName = "secure2k-dom.dev.hq";
            GuidClass domGuid = new GuidClass();
            string siteName = null;
            uint flags = (uint)(DsGetDcFlags.DS_RETURN_FLAT_NAME | DsGetDcFlags.DS_IS_DNS_NAME
                                    | DsGetDcFlags.DS_FORCE_REDISCOVERY);
            DOMAIN_CONTROLLER_INFO domainControllerInfo;
            uint rc = DS.DsGetDcName(computerName, domainName, domGuid, siteName, flags,
                                out domainControllerInfo);

            int a = 1;

            string myServer = @"secure2kms0";
            string myUser = @"secure2k-dom\svcacct";
            string myPwd = @"svcacct";
            string acct1 = @"secure2kms0\test1";
            string acct2 = @"secure2k-dom\svaidya";
            string acct3 = @"builtin\administrators";
            SecurityIdentifier sid1, sid2, sid3;

            Server srvr = new Server(myServer, myUser, myPwd);

            srvr.Bind();
            string refDomain;
            SID_NAME_USE peUse;
            Authorization.LookupAccountName(srvr.Name, acct1, out sid1, out refDomain, out peUse);
            Authorization.LookupAccountName(srvr.Name, acct2, out sid2, out refDomain, out peUse);
            Authorization.LookupAccountName(srvr.Name, acct3, out sid3, out refDomain, out peUse);
            srvr.Unbind();

            AccountLocation l = srvr.SidLocation(sid1);
            l = srvr.SidLocation(sid2);
            l = srvr.SidLocation(sid3);

            srvr.Bind();
            string name;
            string domain;
            Authorization.LookupAccountSid(srvr.Name, sid1, out name, out domain, out peUse);
            Authorization.LookupAccountSid(srvr.Name, sid2, out name, out domain, out peUse);
            Authorization.LookupAccountSid(srvr.Name, sid3, out name, out domain, out peUse);
            srvr.Unbind();

            byte[] bad = new byte[100];
            //SecurityIdentifier sidBad = new SecurityIdentifier(bad, 0);

            bool isOk = Authorization.IsValidSid(sid1);
            isOk = Authorization.IsValidSid(sid2);
            isOk = Authorization.IsValidSid(sid3);
            isOk = Authorization.IsValidSid(bad);
            */

        }
        /*
        static private void testSid()
        {
            SecurityIdentifier sid = new SecurityIdentifier("S-1-5-32-544");
            byte[] sidBin = new byte[sid.BinaryLength];
            sid.GetBinaryForm(sidBin, 0);

            // Bind IPC to remote machine.
            uint rc = Win32.BindIpc("svw2kms", "svw2k", "svcacct", "svcacct");

            // Lookup the account name on the remote machine.
            {
                SID_NAME_USE peUse;
                StringBuilder name = new StringBuilder(100);
                StringBuilder domName = new StringBuilder(100);
                uint nameSize = (uint)name.Capacity;
                uint domSize = (uint)domName.Capacity;
                rc = Authorization.LookupAccountSid("svw2kms", sidBin, name, ref nameSize, domName, ref domSize, out peUse);
            }

            // Lookup remote account by name.
            String n, d, s;
            {
                string computerName = "svw2kms";
                string acctName = @"svw2kms";
                byte[] bsid = new byte[100];
                StringBuilder referencedDomain = new StringBuilder(100);
                uint bsidSize = (uint)bsid.Length;
                uint domLen = (uint)referencedDomain.Capacity;
                SID_NAME_USE peUse;
                rc = Authorization.LookupAccountName(computerName, acctName,
                                bsid, ref bsidSize, referencedDomain, ref domLen, out peUse);
                SecurityIdentifier lsid = new SecurityIdentifier(bsid, 0);
                string sidstr = lsid.Value;
                int l = lsid.BinaryLength;

                StringBuilder name = new StringBuilder(100);
                StringBuilder domName = new StringBuilder(100);
                uint nameSize = (uint)name.Capacity;
                uint domSize = (uint)domName.Capacity;
                rc = Authorization.LookupAccountSid("svw2kms", bsid, name, ref nameSize, domName, ref domSize, out peUse);
                n = name.ToString();
                d = domName.ToString();
                s = sidstr;
            }
            {
                string computerName = "svw2kms";
                string acctName = @"svw2kms\test1";
                byte[] bsid = new byte[100];
                StringBuilder referencedDomain = new StringBuilder(100);
                uint bsidSize = (uint)bsid.Length;
                uint domLen = (uint)referencedDomain.Capacity;
                SID_NAME_USE peUse;
                rc = Authorization.LookupAccountName(computerName, acctName,
                                bsid, ref bsidSize, referencedDomain, ref domLen, out peUse);
                SecurityIdentifier lsid = new SecurityIdentifier(bsid,0);
                string sidstr = lsid.Value;
                int l = lsid.BinaryLength;

                StringBuilder name = new StringBuilder(100);
                StringBuilder domName = new StringBuilder(100);
                uint nameSize = (uint)name.Capacity;
                uint domSize = (uint)domName.Capacity;
                rc = Authorization.LookupAccountSid("svw2kms", bsid, name, ref nameSize, domName, ref domSize, out peUse);

            }
            {
                string computerName = "svw2kms";
                string acctName = @"builtin\administrators";
                byte[] bsid = new byte[100];
                StringBuilder referencedDomain = new StringBuilder(100);
                uint bsidSize = (uint)bsid.Length;
                uint domLen = (uint)referencedDomain.Capacity;
                SID_NAME_USE peUse;
                rc = Authorization.LookupAccountName(computerName, acctName,
                                bsid, ref bsidSize, referencedDomain, ref domLen, out peUse);
                SecurityIdentifier lsid = new SecurityIdentifier(bsid, 0);
                string sidstr = lsid.Value;
                int l = lsid.BinaryLength;

                StringBuilder name = new StringBuilder(100);
                StringBuilder domName = new StringBuilder(100);
                uint nameSize = (uint)name.Capacity;
                uint domSize = (uint)domName.Capacity;
                rc = Authorization.LookupAccountSid("svw2kms", bsid, name, ref nameSize, domName, ref domSize, out peUse);
            }

            // Unbind IPC
            rc = Win32.UnbindIPC("svw2kms");

            /*
            // Convert string sid to sid.
            byte[] sid;
            uint rc = Authorization.ConvertSidStringToSid("S-1-5-32-544", out sid);
            string strSid;
            rc = Authorization.ConvertSidToStringSid(sid, out strSid);

            // Bind IPC to remote machine.
            rc = Win32.BindIpc("svw2kms", "svw2k", "svcacct", "svcacct");

            // Lookup the account name on the remote machine.
            {
                SID_NAME_USE peUse;
                StringBuilder name = new StringBuilder(100);
                StringBuilder domName = new StringBuilder(100);
                uint nameSize = (uint)name.Capacity;
                uint domSize = (uint)domName.Capacity;
                rc = Authorization.LookupAccountSid("svw2kms", sid, name, ref nameSize, domName, ref domSize, out peUse);
            }

            // Lookup remote account by name.
            {
                string computerName = "svw2kms";
                string acctName = @"svw2kms\test1";

                byte[] bsid = new byte[100];
                StringBuilder referencedDomain = new StringBuilder(100);
                uint bsidSize = (uint)bsid.Length;
                uint domLen = (uint)referencedDomain.Capacity;
                SID_NAME_USE peUse;
                rc = Authorization.LookupAccountName(computerName, acctName,
                                bsid, ref bsidSize, referencedDomain, ref domLen, out peUse);
                uint sidsize = Authorization.GetLengthSid(bsid);

                acctName = @"builtin\administrators";
                rc = Authorization.LookupAccountName(computerName, acctName,
                                bsid, ref bsidSize, referencedDomain, ref domLen, out peUse);
                sidsize = Authorization.GetLengthSid(bsid);
            }
        }

        static void Main(string[] args)
        {
            string computerName = "svw2kdc";
            string acctName =  @"svw2k\svaidya";
            string domainName = "svw2k";

            byte[] bsid = new byte[100];
            StringBuilder referencedDomain = new StringBuilder(100);
            uint bsidSize = (uint) bsid.Length;
            uint domLen = (uint) referencedDomain.Capacity;
            SID_NAME_USE peUse;

            uint rc = Authorization.LookupAccountName(computerName, acctName,
                            bsid, ref bsidSize, referencedDomain, ref domLen, out peUse);

            StringBuilder name = new StringBuilder(100);
            StringBuilder domName = new StringBuilder(100);
            uint nameSize = (uint)name.Capacity;
            uint domSize = (uint)domName.Capacity;
            rc = Authorization.LookupAccountSid(computerName, bsid, name, ref nameSize, domName, ref domSize,
                    out peUse);

            GuidClass domGuid = new GuidClass();
            string siteName = null;
            uint flags = (uint)(DsGetDcFlags.DS_RETURN_FLAT_NAME | DsGetDcFlags.DS_IS_FLAT_NAME
                                    | DsGetDcFlags.DS_FORCE_REDISCOVERY);
            DOMAIN_CONTROLLER_INFO domainControllerInfo;
            rc = DS.DsGetDcName(computerName,domainName,domGuid,siteName,flags,
                                out domainControllerInfo);

            // isAD = isDnsDomain || isDnsForest  (isLdapFlag ???)
            bool isLdapFlag = (domainControllerInfo.Flags & (uint) DomainControllerInfoFlags.DS_LDAP_FLAG) 
                                    == (uint) DomainControllerInfoFlags.DS_LDAP_FLAG;
            bool isDnsDomain = (domainControllerInfo.Flags & (uint) DomainControllerInfoFlags.DS_DNS_DOMAIN_FLAG)
                                    == (uint) DomainControllerInfoFlags.DS_DNS_DOMAIN_FLAG;
            bool isDnsForest = (domainControllerInfo.Flags & (uint) DomainControllerInfoFlags.DS_DNS_FOREST_FLAG)
                                    == (uint) DomainControllerInfoFlags.DS_DNS_FOREST_FLAG; 


            // BindIPC
            string computer = "svw2kms";
            string dom = "svw2k";
            string acct = @"svcacct";
            string pwd = "svcacct";
            rc = Win32.BindIpc(computer, dom, acct, pwd);
            if (rc == 0)
            {
                rc = Win32.UnbindIPC(computer);
            }

            testSid();
        }
        */
    }
}
