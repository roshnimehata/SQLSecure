/******************************************************************
 * Name: NetworkMgmt.cs
 *
 * Description: Network management interop wrappers.
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

namespace Idera.SQLsecure.Core.Interop
{
    #region Enums & Types
    public enum AsgType : uint
    {
        USE_WILDCARD,
        USE_DISKDEV,
        USE_SPOOLDEV,
        USE_IPC
    };

    public enum ForceCond : uint
    {
        USE_NOFORCE,
        USE_FORCE,
        USE_LOTS_OF_FORCE
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct USE_INFO_2
    {
        public string ui2_local;
        public string ui2_remote;
        public string ui2_password;
        public uint ui2_status;
        public uint ui2_asg_type;
        public uint ui2_refcount;
        public uint ui2_usecount;
        public string ui2_username;
        public string ui2_domainname;
    }

    #endregion

    public static class NetworkMgmt
    {
        public const uint MAX_PREFERRED_LENGTH = unchecked((uint) -1);
        #region Wrappers
        public static uint NetUseAdd(
                string uncServerName,
                uint level,
                ref USE_INFO_2 buf,
                out uint parmError
            )
        {
            return _NetUseAdd(uncServerName, level, ref buf, out parmError);
        }

        public static uint NetUseDel(
                string uncServerName,
                string useName,
                ForceCond forceCond
            )
        {
            return _NetUseDel(uncServerName, useName, (uint)forceCond);
        }

        public static uint NetUseEnum(
                string UncServerName,
                uint Level,
                out IntPtr Buf,
                uint PerferedMaxiumSize,
                out uint EntriesRead,
                out uint TotalEntries,
                out uint ResumeHandle)
        {
            return
                _NetUseEnum(UncServerName, Level, out Buf, PerferedMaxiumSize, out EntriesRead, out TotalEntries,
                            out ResumeHandle);
        }

        #endregion

        #region Interops
        [DllImport("Netapi32.dll", EntryPoint = "NetUseAdd", CharSet = CharSet.Auto)]
        private static extern uint _NetUseAdd(
                string UncServerName,
                uint Level,
                ref USE_INFO_2 Buf,
                out uint ParmError
            );

        [DllImport("Netapi32.dll", EntryPoint = "NetUseDel", CharSet = CharSet.Auto)]
        private static extern uint _NetUseDel(
                string UncServerName,
                string UseName,
                uint ForceCond
            );

        [DllImport("Netapi32.dll", EntryPoint = "NetUseEnum", CharSet = CharSet.Auto)]
        private static extern uint _NetUseEnum(
                string UncServerName,
                uint Level,
                out IntPtr Buf,
                uint PerferedMaxiumSize,
                out uint EntriesRead,
                out uint TotalEntries,
                out uint ResumeHandle
            );

        #endregion
    }
}

