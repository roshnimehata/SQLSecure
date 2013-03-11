/******************************************************************
* Name: MemoryMgmt.cs
*
* Description: Win32 memory management interop wrappers.
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
    internal static class MemoryMgmt
    {
        #region Wrappers
        public static uint LocalFree(IntPtr hMem)
        {
            uint rc = Win32Errors.ERROR_SUCCESS;

            if (_LocalFree(hMem) != null)
            {
                rc = checked((uint)Marshal.GetLastWin32Error());
            }

            return rc;
        }

        public static uint NetApiBufferFree(IntPtr hMem)
        {
            uint rc = Win32Errors.NERR_SUCCESS;

            if (NetApiBufferFree(hMem) != Win32Errors.NERR_SUCCESS)
            {
                rc = checked((uint)Marshal.GetLastWin32Error());
            }

            return rc;
        }
        #endregion

        #region Interops
        [DllImport("kernel32.dll", EntryPoint = "LocalFree", SetLastError = true)]
        private static extern IntPtr _LocalFree(
                IntPtr hMem
            );

        [DllImport("Netapi32.dll", EntryPoint = "NetApiBufferFree", SetLastError = true)]
        private static extern int _NetApiBufferFree(IntPtr Buffer);

        #endregion
    }
}
