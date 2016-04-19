/******************************************************************
 * Name: Sid.cs
 *
 * Description: Win32 SID is encapsulated in this class.
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
using System.Security.Principal;
using Idera.SQLsecure.Core.Interop;

namespace Idera.SQLsecure.Core.Accounts
{
    public class DomainId
    {
        public const int BufSize = 3;
        public static DomainId Null = new DomainId(new int[] { 0, 0, 0 });

        #region Fields
        private int[] m_Id;
        #endregion

        #region Ctors

        internal DomainId(
                int[] id
            )
        {
            Debug.Assert(id.Length == BufSize);
            m_Id = new int[BufSize];
            for (int i = 0; i < BufSize; ++i)
            {
                m_Id[i] = id[i];
            }
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Override of Equals method.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj == null) { return false; }
            if (this.GetType() != obj.GetType()) { return false; }
            DomainId other = (DomainId)obj;
            return (m_Id[0] == other.m_Id[0] && m_Id[1] == other.m_Id[1] && m_Id[2] == other.m_Id[2]);
        }

        /// <summary>
        /// Static implementation of op==.
        /// </summary>
        public static bool operator==(
                DomainId lhs,
                DomainId rhs
            )
        {
            return Object.Equals(lhs, rhs);
        }

        /// <summary>
        /// Static implementation of op!=.
        /// </summary>
        public static bool operator!=(
                DomainId lhs,
                DomainId rhs
            )
        {
            return !(lhs == rhs);
        }

        /// <summary>
        /// Override implementation of GetHashCode
        /// </summary>
        public override int GetHashCode()
        {
            return ((m_Id[0] ^ m_Id[1]) ^ m_Id[2]);
        }

        #endregion
    }

    public class Sid
    {
        #region Fields
        private const int DomSidLen1 = 24;
        private const int DomSidLen2 = 28;
        private const int DomIdOffset = 12;

        private byte[] m_SidBuffer;
        bool m_IsValid;

        #endregion

        #region Ctors

        public Sid(byte[] sidBuffer)
        {
            m_SidBuffer = sidBuffer;
            m_IsValid = Interop.Authorization.IsValidSid(m_SidBuffer);
        }

        public Sid(string strSid)
        {
            SecurityIdentifier sid = new SecurityIdentifier(strSid);
            m_SidBuffer = new byte[sid.BinaryLength];
            sid.GetBinaryForm(m_SidBuffer, 0);
            m_IsValid = Interop.Authorization.IsValidSid(m_SidBuffer);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Is the SID object valid.
        /// </summary>
        public bool IsValid
        {
            get { return m_IsValid; }
        }

        /// <summary>
        /// Returns length of the SID.
        /// </summary>
        public int Length
        {
            get
            {
                return (IsValid ? (int)Interop.Authorization.GetLengthSid(m_SidBuffer) : 0);
            }
        }

        /// <summary>
        /// Returns binary SID.
        /// </summary>
        public byte[] BinarySid
        {
            get { return m_SidBuffer; }
        }

        /// <summary>
        /// Is this a domain SID, i.e. not one of the
        /// pre-defined SIDs.
        /// </summary>
        public bool IsDomainSid
        {
            get
            {
                int len = Length;
                return (len == DomSidLen1 || len == DomSidLen2);
            }
        }

        /// <summary>
        /// Get the domain ID of the sid.
        /// </summary>
        public DomainId DomainId
        {
            get
            {
                // Not a domain SID return null domain id.
                if (!IsDomainSid) { return DomainId.Null; }

                // Create and populate buffer with domain id.
                int[] buf = new int[DomainId.BufSize];
                int off = DomIdOffset;
                for (int i = 0; i < DomainId.BufSize; ++i)
                {
                    buf[i] = BitConverter.ToInt32(m_SidBuffer, off);
                    off += sizeof(int);
                }

                // Return the domain id object.
                return new DomainId(buf);
            }
        }

        /// <summary>
        /// SDDL format string
        /// </summary>
        public string SidString
        {
            get
            {
                Debug.Assert(IsValid);

                SecurityIdentifier sid = new SecurityIdentifier(m_SidBuffer, 0);
                return sid.Value;
            }
        }

        /// <summary>
        /// Hex format string
        /// </summary>
        public string HexString
        {
            get
            {
                string sidHexString = "0x";
                foreach (byte sidbyte in m_SidBuffer)
                {
                    sidHexString += sidbyte.ToString("x2");
                }
                return sidHexString;
            }
        }

        /// <summary>
        /// LDAP directory SID search filter.
        /// </summary>
        public string LdapFilter
        {
            get
            {
                StringBuilder bldr = new StringBuilder();
                bldr.Append("(objectSID=");
                for (int i = 0; i < m_SidBuffer.Length; ++i)
                {
                    bldr.Append("\\");
                    bldr.Append(m_SidBuffer[i].ToString("x2"));
                }
                bldr.Append(")");
                return bldr.ToString();
            }
        }

        #endregion

        #region Methods

        public bool IsSpecialWellKnownSid()
        {
            bool isSpecial = false;
            SecurityIdentifier si = new SecurityIdentifier(SidString);

            if (si.IsWellKnown(WellKnownSidType.WorldSid))
            {
                 isSpecial = true;
            }
            else if (si.IsWellKnown(WellKnownSidType.DialupSid))
            {
                isSpecial = true;
            }
            else if (si.IsWellKnown(WellKnownSidType.NetworkSid))
            {
                isSpecial = true;
            }
            else if (si.IsWellKnown(WellKnownSidType.BatchSid))
            {
                isSpecial = true;
            }
            else if (si.IsWellKnown(WellKnownSidType.InteractiveSid))
            {
                isSpecial = true;
            }
            else if (si.IsWellKnown(WellKnownSidType.ServiceSid))
            {
                isSpecial = true;
            }
            else if (si.IsWellKnown(WellKnownSidType.AnonymousSid))
            {
                isSpecial = true;
            }
            else if (si.IsWellKnown(WellKnownSidType.AuthenticatedUserSid))
            {
                isSpecial = true;
            }
            else if (si.IsWellKnown(WellKnownSidType.TerminalServerSid))
            {
                isSpecial = true;
            }

            return isSpecial;
        }

        /// <summary>
        /// Compares the domain ID portion of the
        /// two SID objects.
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public bool IsEqualDomainId(Sid sid)
        {
            if (!sid.IsValid) { return false; }
            return DomainId.Equals(sid.DomainId);
        }

        /// <summary>
        /// Looks up the account based on the SID.   
        /// Static version of the method.
        /// </summary>
        /// <param name="server"></param>
        /// <param name="sid"></param>
        /// <param name="name"></param>
        /// <param name="domain"></param>
        /// <param name="peUse"></param>
        /// <returns></returns>
        public static bool LookupAccountName(
                string server,
                Sid sid,
                out string name,
                out string domain,
                out Interop.SID_NAME_USE peUse
            )
        {
            // Init returns.
            name = null;
            domain = null;
            peUse = SID_NAME_USE.SidTypeUnknown;

            if (!sid.IsValid) { return false; }
            return Interop.Authorization.LookupAccountSid(server, sid.m_SidBuffer, out name, out domain, out peUse);
        }

        /// <summary>
        /// Look up the account based on the SID.  
        /// Non-static version of the method.
        /// </summary>
        /// <param name="server"></param>
        /// <param name="name"></param>
        /// <param name="domain"></param>
        /// <param name="peUse"></param>
        /// <returns></returns>
        public bool LookupAccount(
                string server,
                out string name,
                out string domain,
                out Interop.SID_NAME_USE peUse
            )
        {
            return LookupAccountName(server, this, out name, out domain, out peUse);
        }

        /// <summary>
        /// Get the account name for the sid in dom\acct format.
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        public string AccountName(
                string server
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(server));

            string acctname = string.Empty;
            string dom = string.Empty;
            string acct = string.Empty;
            Interop.SID_NAME_USE peUse = SID_NAME_USE.SidTypeUnknown;
            if (LookupAccount(server, out acct, out dom, out peUse))
            {
                acctname = dom + @"\" + acct;
            }
            else
            {
                acctname = SidString;
            }

            return acctname;
        }

        /// <summary>
        /// Override of ToString method.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return SidString;
        }

        /// <summary>
        /// Performs a byte by byte compare to another Sid and return true if the values are equal.
        /// </summary>
        public bool Equals(Sid sid)
        {
            return (Compare(this, sid) == 0);
        }

        /// <summary>
        /// Compares this instance to another Sid Byte array and returns an indication of their relative values.
        /// </summary>
        public int CompareTo(Sid sid)
        {
            return Compare(this, sid);
        }
        /// <summary>
        /// Compare two Sid Byte arrays and return an indication of their relative values.
        /// </summary>
        public static int Compare(Sid sid1, Sid sid2)
        {
            if (sid1 == null)
            {
                if (sid2 == null)
                {
                    return 0;
                }
                return -1;
            }
            if (sid2 == null)
            {
                return 1;
            }

            int result = 0;
            if ((result = sid1.Length.CompareTo(sid2.Length)) == 0)
            {
                for (int cnt = 0; cnt < sid1.Length; cnt++)
                {
                    if ((result = sid1.BinarySid[cnt].CompareTo(sid2.BinarySid[cnt])) != 0)
                    {
                        return result;
                    }
                }
            }

            return result;
        }
        #endregion
    }

}
