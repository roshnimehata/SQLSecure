using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;

namespace Idera.SQLsecure.Core.Accounts
{
    public class Impersonation
    {

        /// <summary>
        /// Parses the Windows domain from a full domain\username string
        /// </summary>
        /// <param name="fullUsername">The full domain\username string</param>
        /// <returns>The Windows domain</returns>
        private static string ParseDomainFromFullUsername(string fullUsername)
        {
            int index = fullUsername.IndexOf(@"\");
            if (index > 0)
            {
                return fullUsername.Substring(0, index);
            }
            else
            {
                //throw new SQLsafeException(SharedConstants.Exception_ParseDomainFailed);
                return "."; // use local account database
            }
        }

        /// <summary>
        /// Parses the username from a full domain\username string
        /// </summary>
        /// <param name="fullUsername">The full domain\username string</param>
        /// <returns>The username</returns>
        private static string ParseUsernameFromFullUsername(string fullUsername)
        {
            int index = fullUsername.IndexOf(@"\") + 1;
            if (index > 0)
            {
                return fullUsername.Substring(index, (fullUsername.Length - index));
            }
            else
            {
                //throw new SQLsafeException(SharedConstants.Exception_ParseUsernameFailed);
                return fullUsername;
            }
        }


        /// <summary>
        /// Gets the current thread's WindowsIdentity.
        /// </summary>
        /// <param name="fullUsername">The full username.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        /// The WindowsIdentity that can be used for impersonation
        /// </returns>
        /// <remarks>
        /// If fullUsername is present, uses Win32 LogonUser call to return identity;
        /// otherwise, gets WindowsIdentity directly from thread's credentials passed from remoting call
        /// </remarks>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static WindowsIdentity GetCurrentIdentity(string fullUsername, string password)
        {
            WindowsIdentity currentIdentity = null;
            IntPtr tokenHandle = IntPtr.Zero;
            IntPtr dupeTokenHandle = IntPtr.Zero;
            bool returnValue;

            if (!string.IsNullOrEmpty(fullUsername))
            {
                // User authenticated manually              
                string domain = ParseDomainFromFullUsername(fullUsername);
                string username = ParseUsernameFromFullUsername(fullUsername);
//                Logger.Instance.Debug(String.Format("Authenticating username {0}\\{1}", domain, username));

                // Call LogonUser to obtain a handle to an access token 
                returnValue = NativeMethods.LogonUser(username,
                                                           domain,
                                                           password,
                                                           NativeMethods.LOGON32_LOGON_INTERACTIVE,
                                                           NativeMethods.LOGON32_PROVIDER_DEFAULT,
                                                           ref tokenHandle);

                if (!returnValue)
                {
                    int returnCode = Marshal.GetLastWin32Error();
                    if (returnCode == 0x00000522)
                    {
                        // "A required privilege was not held by the user"
                        throw new Exception(                           
                                String.Format("Could not Impersonate User {0}", fullUsername) +
                                String.Format("\r\n     Details: {0}", NativeMethods.GetErrorMessage(returnCode)) +
                                String.Format("\r\nThe Windows account '{0}' must have the 'Act as part of Operating System' privilage enabled", WindowsIdentity.GetCurrent().Name));
                    }
                    else
                    {
                        throw new Exception(
                            String.Format("Could not logon user, error message: {0}",
                                          NativeMethods.GetErrorMessage(returnCode)));
                    }
                }
            }
            else
            {
                // Get Local user identity, 
                if (currentIdentity == null)
                {
                    tokenHandle = WindowsIdentity.GetCurrent(TokenAccessLevels.MaximumAllowed).Token;
                }
            }

            if(tokenHandle != IntPtr.Zero)
            {
                // Duplicate the token, so that we can get a primary token for impersonation
                returnValue =
                    NativeMethods.DuplicateToken(tokenHandle, NativeMethods.SECURITY_IMPERSONATION, ref dupeTokenHandle);

                if (!returnValue)
                {
                    int returnCode = Marshal.GetLastWin32Error();
                    throw new Exception(
                        String.Format("Could not logon user (with impersonation token), error message: {0}",
                                      NativeMethods.GetErrorMessage(returnCode)));
                }

                // Generate the WindowsIdentity from the duplicate (primary) token
                currentIdentity = new WindowsIdentity(dupeTokenHandle);

                // Close the access token handle
                NativeMethods.CloseHandle(tokenHandle);
                NativeMethods.CloseHandle(dupeTokenHandle);
            }

            return currentIdentity;
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool LogonUser(String lpszUsername,
                                              String lpszDomain,
                                              String lpszPassword,
                                              int dwLogonType,
                                              int dwLogonProvider,
                                              ref IntPtr phToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DuplicateToken(IntPtr ExistingTokenHandle,
                                                   int SECURITY_IMPERSONATION_LEVEL,
                                                   ref IntPtr DuplicateTokenHandle);

        [
                    SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"),
                    DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr handle);

    }

    public static class NativeMethods
    {
        #region Win32 authentication methods

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool LogonUser(String lpszUsername,
                                              String lpszDomain,
                                              String lpszPassword,
                                              int dwLogonType,
                                              int dwLogonProvider,
                                              ref IntPtr phToken);

        [
            SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"),
            DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr handle);

        [
            SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"),
            DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(SafeHandle handle);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DuplicateToken(IntPtr ExistingTokenHandle,
                                                   int SECURITY_IMPERSONATION_LEVEL,
                                                   ref IntPtr DuplicateTokenHandle);

        [
            SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "4#"),
            DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern unsafe int FormatMessage(int dwFlags,
                                                       ref IntPtr lpSource,
                                                       int dwMessageId,
                                                       int dwLanguageId,
                                                       ref String lpBuffer,
                                                       int nSize,
                                                       IntPtr* arguments);

        internal const int LOGON32_PROVIDER_DEFAULT = 0;
        //This parameter causes LogonUser to create a primary token.
        internal const int LOGON32_LOGON_INTERACTIVE = 2;
        internal const int LOGON32_LOGON_NETWORK = 3;
        internal const int LOGON32_LOGON_NETWORK_CLEARTEXT = 8;
        internal const int SECURITY_IMPERSONATION = 2;

        /// <summary>
        /// Formats and returns an error message corresponding to the input errorCode.
        /// </summary>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        public static unsafe string GetErrorMessage(int errorCode)
        {
            int FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100;
            int FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200;
            int FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;

            int messageSize = 255;
            String lpMsgBuf = "";
            int dwFlags = FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS;

            IntPtr ptrlpSource = IntPtr.Zero;
            IntPtr prtArguments = IntPtr.Zero;

            FormatMessage(dwFlags, ref ptrlpSource, errorCode, 0, ref lpMsgBuf, messageSize, &prtArguments);

            return lpMsgBuf;
        }

        #endregion
    }
}
