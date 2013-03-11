/******************************************************************
 * Name: EventMessages.cs
 *
 * Description: The event log messages are defined in this file. 
 * These entries should match those defined in EventMessages\EventMessages.mc
 * file.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Idera.SQLsecure.Core.Logger
{
    /// <summary>
    /// SQLsecure NT event log categories
    /// </summary>
    public enum SQLsecureCat : int
    {
        #region Data Loader (DL)

        DlStartCat      = 1,
        DlEndCat        = 2,
        DlValidationCat = 3,
        DlFilterLoadCat = 4,
        DlDataLoadCat = 5

        #endregion
    }

    /// <summary>
    /// SQLsecure NT event log event messages
    /// </summary>
    public enum SQLsecureEvent : long
    {
        #region Data Loader (DL)

        DlInfoStartMsg = 5000,
        DlInfoEndMsg = 5001,
        DlErrInvalidRepositoryVersionMsg = 5002,
        DlErrSQLsecureDbNotFound = 5003,
        DlErrSchemaVerNotCompatible = 5004,
        DlErrNoSQLsecurePermissions = 5005,
        DlErrOpenRepositoryConnectionFailed = 5006,
        DlErrNoLicense = 5007,

        DlErrTargetNotRegistered = 5020,
        DlErrInvalidTargetVersionMsg = 5021,
        DlErrOpenTargetConnectionFailed = 5022,
        DlErrNoTargetPermissions = 5023,
        DlErrGeneralMsg = 5024,
        #endregion

        #region Exceptions (EX)
        ExErrExceptionRaised = 5900
        #endregion
    }
}
