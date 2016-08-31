/******************************************************************
 * Name: Path.cs
 *
 * Description: Encapsulate directory paths and provides parsing 
 * functions.
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
using Idera.SQLsecure.Core.Logger;

namespace Idera.SQLsecure.Core.Accounts
{
    /// <summary>
    /// 
    /// </summary>
    public class Path
    {
        private static LogX logX = new LogX("Idera.SQLsecure.Core.Accounts.Path");

        #region Helpers
        private enum PathType
        {
            AD,
            WinNT,
            Unknown
        }

        private static PathType getPathType(string path)
        {
            Debug.Assert(!string.IsNullOrEmpty(path));

            PathType type = PathType.Unknown;
            if (string.IsNullOrEmpty(path) || path.Length == 0)
            {
                type = PathType.Unknown;
            }
            else if (path.StartsWith(Constants.LdapPrefix) || 
                        path.StartsWith(Constants.GcPrefix))
            {
                type = PathType.AD;
            }
            else if (path.StartsWith(Constants.WinNTPrefix))
            {
                type = PathType.WinNT;
            }
            else
            {
                Debug.Assert(false);
                type = PathType.Unknown;
            }
            return type;
        }

        #endregion

        #region Methods

        public static string GetComputerFromSQLServerInstance(string sqlInstance)
        {
            string computer = sqlInstance;
            Debug.Assert(!String.IsNullOrEmpty(sqlInstance));
            if (sqlInstance.Contains(Constants.SingleBackSlashStr))
            {
                computer = sqlInstance.Substring(0, sqlInstance.IndexOf(Constants.SingleBackSlashStr));
            }

            return computer;
        }

        public static string GetInstanceFromSQLServerInstance(string sqlInstance)
        {
            string instance = string.Empty;
            Debug.Assert(!String.IsNullOrEmpty(sqlInstance));
            if (sqlInstance.Contains(Constants.SingleBackSlashStr))
            {
                instance = sqlInstance.Substring(sqlInstance.IndexOf(Constants.SingleBackSlashStr)+1);
            }

            return instance;
        }
    

        public static string NonWhackPrefixComputer(string computer)
        {
            Debug.Assert(!String.IsNullOrEmpty(computer));

            if (computer.StartsWith(Constants.DoubleBackSlashStr))
            {
                return computer.Substring(2);
            }
            else
            {
                return computer;
            }
        }

        public static string WhackPrefixComputer (string computer)
        {
            Debug.Assert(!String.IsNullOrEmpty(computer));
            string ret;
            if (computer.StartsWith(Constants.DoubleBackSlashStr))
            {
                ret = computer;
            }
            else
            {
                StringBuilder bldr = new StringBuilder();
                bldr.Append(Constants.DoubleBackSlashStr);
                bldr.Append(computer);
                ret = bldr.ToString();
            }

            return ret;
        }

        public static string DomainDnsNameToLdapPath(
                string dnsName,
                string dc
            )
        {
            string[] dcs = dnsName.Split('.');
            StringBuilder bldr = new StringBuilder();
            bldr.Append(Constants.LdapPrefix);
            if (!String.IsNullOrEmpty(dc))
            {
                bldr.Append(dc);
                bldr.Append(Constants.SingleSlashStr);
            }
            int i = 0;
            foreach (string s in dcs)
            {
                bldr.Append(Constants.LdapDcAttrib);
                bldr.Append("=");
                bldr.Append(s);
                if (++i < dcs.Length) { bldr.Append(", "); }
            }
            return bldr.ToString();
        }

        public static bool ExtractDomainFromPath(
                string objPath,
                out string domain,
                out bool isFlat
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(objPath));

            // Init returns.
            domain = null;
            isFlat = false;
            bool isOk = true;

            // Get the path type.
            PathType type = getPathType(objPath);
            if (type == PathType.Unknown)
            {
                logX.loggerX.Error("ERROR - failed to detect path type");
                isOk = false;
            }

            // Construct pathname object and get num elements.
            ActiveDs.Pathname path = null;
            int numElements = 0;
            if(isOk)
            {
                path = new ActiveDs.Pathname();
                path.Set(objPath, 1);//Constants.AdsSetTypeDn);

                // Get number of DN elements in the path.
                numElements = path.GetNumElements();
                if (numElements == 0)
                {
                    logX.loggerX.Error("ERROR - no elements in the LDAP path, ", objPath);
                    isOk = false;
                }
            }

            // Process based on the path type.
            if (isOk)
            {
                switch (type)
                {
                    case PathType.AD:

                        // Parse pased on the type of path.
                        StringBuilder domBldr = new StringBuilder();
                        for (int i = 0; i < numElements; ++i)
                        {
                            // If the element has a dc attribute, take everything
                            // to the right of = sign.  No blank trimming is needed,
                            // because the pathname coclass handles it correctly.
                            string pathElement = path.GetElement(i);
                            if (string.Compare(Constants.LdapDcAttrib, 0,
                                        pathElement, 0, Constants.LdapDcAttrib.Length, true) == 0)
                            {
                                int eqIndex = pathElement.IndexOf('='); // find = sign
                                if (eqIndex != -1)
                                {
                                    domBldr.Append(pathElement.Substring(eqIndex + 1)); // take everything right of it
                                    if (i != (numElements - 1)) { domBldr.Append("."); } // append . if not last element
                                }
                            }
                        }

                        // Set the name type to non-flat (i.e. DNS), and get the DNS domain name.
                        isFlat = false;
                        domain = domBldr.ToString();
                        if(string.IsNullOrEmpty(domain))
                        {
                            logX.loggerX.Error("ERROR - failed to parse to get the DNS domain name for, ", objPath);
                            isOk = false;
                        }
                        break;

                    case PathType.WinNT:
                        switch (numElements)
                        {
                            case 1:
                                domain = string.Empty;
                                break;

                            case 2:
                                domain = path.GetElement(1);
                                break;

                            case 3:
                                domain = path.GetElement(1);
                                break;

                            default:
                                //Debug.Assert(false);
                                logX.loggerX.Error("ERROR - not valid number of elements in the path, ", objPath);
                                isOk = false;
                                break;
                        }

                        if(isOk)
                        {
                            isFlat = true;
                            if(numElements != 1 && string.IsNullOrEmpty(domain))
                            {
                                logX.loggerX.Error("ERROR - failed to parse to get the Flat domain name for, ", objPath);
                                isOk = false;
                            }
                        }

                        break;

                    case PathType.Unknown:
                    default:
                        Debug.Assert(false);
                        break;
                }
            }

            return isOk;
        }

        public static bool GetSamPathFromWinNTPath(
                string objPath,
                out string samPath
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(objPath));
            Debug.Assert(IsWinntPath(objPath));

            // Init returns.
            bool isOk = true;
            samPath = null;

            // Construct pathname object and get num elements.
            ActiveDs.Pathname path = null;
            int numElements = 0;
            if (isOk)
            {
                path = new ActiveDs.Pathname();
                path.Set(objPath, 1);//Constants.AdsSetTypeDn);

                // Get number of DN elements in the path.
                numElements = path.GetNumElements();
                if (numElements == 0)
                {
                    logX.loggerX.Error("ERROR - no elements in the LDAP path, ", objPath);
                    isOk = false;
                }
            }

            // Get domain and account.
            string domain = string.Empty;
            string account = string.Empty;
            if (isOk)
            {
                switch (numElements)
                {
                    case 1:
                        domain = string.Empty;
                        account = path.GetElement(0);
                        break;

                    case 2:
                        domain = path.GetElement(1);
                        account = path.GetElement(0);
                        break;

                    case 3:
                        domain = path.GetElement(1);
                        account = path.GetElement(0);
                        break;

                    default:
                        //Debug.Assert(false);
                        logX.loggerX.Error("ERROR - not valid number of elements in the path, ", objPath);
                        isOk = false;
                        break;
                }
            }

            // Create sam path.
            if (isOk)
            {
                samPath = MakeSamPath(domain, account);
            }

            return isOk;
        }

        public static string MakeSamPath(
                string domain,
                string account
            )
        {
            StringBuilder bldr = new StringBuilder();
            if (!string.IsNullOrEmpty(domain))
            {
                bldr.Append(domain);
                bldr.Append(Constants.SingleBackSlashStr);
            }
            bldr.Append(account);
            return bldr.ToString();
        }

        public static void SplitSamPath (
                string account,
                out string domain,
                out string user
            )
        {
            Debug.Assert(!String.IsNullOrEmpty(account));

            // Init returns.
            domain = string.Empty;
            user = string.Empty;

            // If no account return.
            if(string.IsNullOrEmpty(account)) { return; }

            // Get domain and user based on single whack location.
            int index = account.IndexOf(Constants.SingleBackSlashStr);
            if (index != -1)
            {
                domain = account.Substring(0, index);
                user = account.Substring(index + 1, account.Length - index - 1);
            }
            else
            {
                user = account;
            }
        }


        public static string MakeWinntPath(
                string domain,
                string account
            )
        {
            StringBuilder bldr = new StringBuilder();
            bldr.Append(Constants.WinNTPrefix);
            bldr.Append(domain);
            if(account != null)
            {
                bldr.Append(Constants.SingleSlashStr);
                bldr.Append(account);
            }
            return bldr.ToString();
        }

        public static bool IsWinntPath(string path)
        {
            Debug.Assert(!string.IsNullOrEmpty(path));
            if (string.IsNullOrEmpty(path)) { return false; }
            return getPathType(path) == PathType.WinNT;
        }

        #endregion
    }
                    
}
