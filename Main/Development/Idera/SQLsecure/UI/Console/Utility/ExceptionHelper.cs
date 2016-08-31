using System;
using System.Collections.Generic;
using System.Text;

using System.Data.SqlClient;
using System.Diagnostics;

namespace Idera.SQLsecure.UI.Console.Utility
{
    internal static class ExceptionHelper
    {
        internal static bool IsSqlLoginFailed(Exception ex)
        {
            Debug.Assert(ex != null);
            // Cast to sql excetion.
            SqlException sqlException = ex as SqlException;
            if(sqlException == null) { return false; }

            // Check the error code.
            return sqlException.Number == 18452 || sqlException.Number == 18456;
        }
    }
}
