using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Idera.SQLsecure.UI.Console.Utility
{
    class Helper
    {
        public static ServerType ConvertSQLTypeStringToEnum(string stype)
        {
            stype = stype.ToUpper();
            return (stype == "ADB" ? ServerType.AzureSQLDatabase : (stype == "AVM" ? ServerType.SQLServerOnAzureVM : ServerType.OnPremise));
        }
    }
}
