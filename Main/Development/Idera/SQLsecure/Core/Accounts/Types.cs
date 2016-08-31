/******************************************************************
 * Name: Types.cs
 *
 * Description: Accounts module types and constants.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace Idera.SQLsecure.Core.Accounts
{
    /// <summary>
    /// Computer role.
    /// </summary>
    public enum OSDomainRole
    {
        StandaloneWorkstation,
        MemberWorkstation,
        StandaloneServer,
        MemberServer,
        BDC,
        PDC
    }

    /// <summary>
    /// Domain type
    /// </summary>
    public enum DomainType
    {
        AD,
        SAM
    }

    public enum ObjectClass
    {
        Unknown,
        Group,
        LocalGroup,
        GlobalGroup,
        UniversalGroup,
        DistributionGroup,
        WellknownGroup,
        User,
        Domain,
        Computer,
        InetOrgPerson
    }

    [Flags]
    public enum PasswordStatus
    {
        [Description("Ok")]
        Ok = 0,
        [Description("Blank")]
        Blank = 1,
        [Description("Weak")]
        Weak = 2,
        [Description("Matches Login Name")]
        SameAsLogin = 4,
    }
}
