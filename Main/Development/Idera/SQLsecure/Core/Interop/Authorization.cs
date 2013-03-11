/******************************************************************
 * Name: Authorization.cs
 *
 * Description: Windows authorization interop function wrappers.
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
using System.Security.Principal;

namespace Idera.SQLsecure.Core.Interop
{
    #region Enums & Types
    public enum SID_NAME_USE
    {
        SidTypeUser = 1,
        SidTypeGroup,
        SidTypeDomain,
        SidTypeAlias,
        SidTypeWellKnownGroup,
        SidTypeDeletedAccount,
        SidTypeInvalid,
        SidTypeUnknown,
        SidTypeComputer
    };
    #endregion

    public static class Authorization
    {
        #region Constants
        private const int NameLength = 100;
        private const int SidLength = 40;
        #endregion

        #region Wrappers
        [EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
        public static bool LookupAccountSid(
                string lpSystemName,
                SecurityIdentifier sid,
                out string name,
                out string refDomain,
                out SID_NAME_USE peUse
            )
        {
            // Init returns.
            name = null;
            refDomain = null;
            peUse = SID_NAME_USE.SidTypeUnknown;

            // Validate input.
            if (sid == null) { return false; }

            // Get binary form of the SID.
            byte[] bSid = new byte[sid.BinaryLength];
            if (bSid == null) { return false; }
            sid.GetBinaryForm(bSid, 0);

            // Call overloaded Lookup function.
            return LookupAccountSid(lpSystemName, bSid, out name,
                        out refDomain, out peUse);
        }

        [EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
        public static bool LookupAccountSid(
                string lpSystemName,
                byte[] sid,
                out string name,
                out string refDomain,
                out SID_NAME_USE peUse
            )
        {
            // Initialize returns.
            name = null;
            refDomain = null;
            peUse = SID_NAME_USE.SidTypeUnknown;

            // Validate input.
            if (sid == null) { return false; }

            // Allocate buffers.
            StringBuilder nameBldr = new StringBuilder(NameLength);
            if (nameBldr == null) { return false; }
            StringBuilder refDomBldr = new StringBuilder(NameLength);
            if (refDomBldr == null) { return false; }
            uint nameBldrSize = (uint)nameBldr.Capacity;
            uint refDomBldrSize = (uint)refDomBldr.Capacity;

            // Lookup account by SID.
            uint rc = Win32Errors.ERROR_SUCCESS;
            if (!_LookupAccountSid(lpSystemName, sid, nameBldr, ref nameBldrSize,
                    refDomBldr, ref refDomBldrSize, out peUse))
            {
                rc = checked((uint)Marshal.GetLastWin32Error());
            }
            else
            {
                name = nameBldr.ToString();
                refDomain = refDomBldr.ToString();
            }

            return rc == Win32Errors.ERROR_SUCCESS;
        }

        [EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
        public static bool LookupAccountName(
                String lpSystemName,
                String lpAccountName,
                out SecurityIdentifier sid,
                out string refDomain,
                out SID_NAME_USE peUse
            )
        {
            // Init returns.
            sid = null;
            refDomain = null;
            peUse = SID_NAME_USE.SidTypeUnknown;

            // Validate input.
            if (lpAccountName == null || lpAccountName.Length == 0) { return false; }

            // Call overloaded Lookup...
            byte[] bSid;
            if(LookupAccountName(lpSystemName, lpAccountName, out bSid, out refDomain,
                                        out peUse))
            {
                sid = new SecurityIdentifier(bSid, 0);
            }

            return sid != null;
        }

        [EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
        public static bool LookupAccountName(
                String lpSystemName,
                String lpAccountName,
                out byte[] sid,
                out string refDomain,
                out SID_NAME_USE peUse
            )
        {
            // Init returns.
            sid = null;
            refDomain = null;
            peUse = SID_NAME_USE.SidTypeUnknown;

            // Validate inputs.
            if (lpAccountName == null || lpAccountName.Length == 0) { return false; }

            // Allocate buffers.
            byte[] bSid = new byte[SidLength];
            if (bSid == null) { return false; }
            StringBuilder refDomBldr = new StringBuilder(NameLength);
            if (refDomBldr == null) { return false; }
            uint bSidSize = (uint)bSid.Length;
            uint refDomBldrSize = (uint) refDomBldr.Capacity;

            // Lookup account by name.
            uint rc = Win32Errors.ERROR_SUCCESS;
            if (!_LookupAccountName(lpSystemName, lpAccountName, bSid, ref bSidSize,
                    refDomBldr, ref refDomBldrSize, out peUse))
            {
                rc = checked((uint)Marshal.GetLastWin32Error());
            }
            else
            {
                sid = bSid;
                refDomain = refDomBldr.ToString();
            }

            return rc == Win32Errors.ERROR_SUCCESS;
        }

        public static bool IsValidSid(
                SecurityIdentifier sid
            )
        {
            if (sid == null) { return false; }
            byte[] bSid = new byte[sid.BinaryLength];
            if (bSid == null) { return false; }
            sid.GetBinaryForm(bSid, 0);
            return IsValidSid(bSid);
        }

        public static bool IsValidSid(
                byte[] sid
            )
        {
            if (sid == null) { return false; }
            return _IsValidSid(sid);
        }

        public static uint GetLengthSid(
                byte[] sid
            )
        {
            if (sid == null) { return 0; }
            if (!IsValidSid(sid)) { return 0; }
            return _GetLengthSid(sid);
        }
        #endregion

        #region Private Interops
        [DllImport("advapi32.dll", EntryPoint = "LookupAccountSid", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool _LookupAccountSid(
                string lpSystemName,
                byte[] Sid,
                StringBuilder lpName,
                ref uint cchName,
                StringBuilder lpReferencedDomainName,
                ref uint cchReferencedDomainName,
                out SID_NAME_USE peUse
            );

        [DllImport("advapi32.dll", EntryPoint = "LookupAccountName", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool _LookupAccountName(
                String lpSystemName,
                String lpAccountName,
                byte[] Sid,
                ref uint cbSid,
                StringBuilder lpReferencedDomainName,
                ref uint cchReferencedDomainName,
                out SID_NAME_USE peUse
            );

        [DllImport("advapi32.dll", EntryPoint = "IsValidSid", CharSet = CharSet.Auto)]
        private static extern bool _IsValidSid(
		        [MarshalAs(UnmanagedType.LPArray)] byte [] pSID
            );

        [DllImport("advapi32.dll", EntryPoint = "GetLengthSid", CharSet = CharSet.Auto)]
        private static extern uint _GetLengthSid(
                [MarshalAs(UnmanagedType.LPArray)] byte[] pSID
            );


        #endregion
    }
}
