/******************************************************************
 * Name: DS.cs
 *
 * Description: Directory Service interop wrapper functions.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;



namespace Idera.SQLsecure.Core.Interop
{
    #region Enums & Types

    [Flags]
    public enum DsGetDcFlags : uint
    {
        DS_FORCE_REDISCOVERY = 0x00000001,
        DS_DIRECTORY_SERVICE_REQUIRED = 0x00000010,
        DS_DIRECTORY_SERVICE_PREFERRED = 0x00000020,
        DS_GC_SERVER_REQUIRED = 0x00000040,
        DS_PDC_REQUIRED = 0x00000080,
        DS_BACKGROUND_ONLY = 0x00000100,
        DS_IP_REQUIRED = 0x00000200,
        DS_KDC_REQUIRED = 0x00000400,
        DS_TIMESERV_REQUIRED = 0x00000800,
        DS_WRITABLE_REQUIRED = 0x00001000,
        DS_GOOD_TIMESERV_PREFERRED = 0x00002000,
        DS_AVOID_SELF = 0x00004000,
        DS_ONLY_LDAP_NEEDED = 0x00008000,
        DS_IS_FLAT_NAME = 0x00010000,
        DS_IS_DNS_NAME = 0x00020000,
        DS_RETURN_DNS_NAME = 0x40000000,
        DS_RETURN_FLAT_NAME = 0x80000000
    };

    [Flags]
    public enum DomainControllerInfoFlags : uint
    {
        DS_PDC_FLAG = 0x00000001,    // DC is PDC of Domain
        DS_GC_FLAG = 0x00000004,    // DC is a GC of forest
        DS_LDAP_FLAG = 0x00000008,    // Server supports an LDAP server
        DS_DS_FLAG = 0x00000010,    // DC supports a DS and is a Domain Controller
        DS_KDC_FLAG = 0x00000020,    // DC is running KDC service
        DS_TIMESERV_FLAG = 0x00000040,    // DC is running time service
        DS_CLOSEST_FLAG = 0x00000080,    // DC is in closest site to client
        DS_WRITABLE_FLAG = 0x00000100,    // DC has a writable DS
        DS_GOOD_TIMESERV_FLAG = 0x00000200,    // DC is running time service (and has clock hardware)
        DS_NDNC_FLAG = 0x00000400,    // DomainName is non-domain NC serviced by the LDAP server
        DS_PING_FLAGS = 0x0000FFFF,    // Flags returned on ping
        DS_DNS_CONTROLLER_FLAG = 0x20000000,    // DomainControllerName is a DNS name
        DS_DNS_DOMAIN_FLAG = 0x40000000,    // DomainName is a DNS name
        DS_DNS_FOREST_FLAG = 0x80000000    // DnsForestName is a DNS name
    };

    [StructLayout(LayoutKind.Sequential)]
    internal class GuidClass
    {
        public Guid TheGuid = Guid.Empty;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct DOMAIN_CONTROLLER_INFO
    {
        public string DomainControllerName;
        public string DomainControllerAddress;
        public uint DomainControllerAddressType;
        public Guid DomainGuid;
        public string DomainName;
        public string DnsForestName;
        public uint Flags;
        public string DcSiteName;
        public string ClientSiteName;
    }
    #endregion

    public static class DS
    {
        #region Wrappers
        [EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
        public static uint DsGetDcName(
                string computerName,
                string domainName,
                uint flags,
                out DOMAIN_CONTROLLER_INFO domainControllerInfo
            )
        {
            GuidClass guid = new GuidClass();
            String site = null;
            IntPtr pDcInfo = IntPtr.Zero;
            uint rc = _DsGetDcName(computerName, domainName, guid, site, flags,
                                    out pDcInfo);
            if (rc == 0)
            {
                domainControllerInfo = (DOMAIN_CONTROLLER_INFO)Marshal.PtrToStructure
                                            (pDcInfo, typeof(DOMAIN_CONTROLLER_INFO));
                //MemoryMgmt.NetApiBufferFree(pDcInfo);
            }
            else
            {
                domainControllerInfo = new DOMAIN_CONTROLLER_INFO();
            }
            return rc;
        }

        #endregion

        #region Private Interops
        [DllImport("Netapi32.dll", EntryPoint = "DsGetDcName", CharSet = CharSet.Auto)]
        private static extern uint _DsGetDcName(
                String computerName,
                String DomainName,
                [In] GuidClass DomainGuid,
                String SiteName,
                uint Flags,
                out IntPtr DomainControllerInfo
            );

        #endregion
    }
}
