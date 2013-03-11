using System;
using System.Collections.Generic;
using System.Text;

namespace Idera.SQLsecure.Core.Interop
{
    public static class Win32Errors
    {
        public const uint ERROR_SUCCESS = 0;
        public const uint NERR_SUCCESS = 0;
        public const uint ERROR_NOT_ENOUGH_MEMORY = 8;
        public const uint ERROR_SESSION_CREDENTIAL_CONFLICT = 1219;
        public const uint ERROR_INVALID_SID = 1337;
    }
}
