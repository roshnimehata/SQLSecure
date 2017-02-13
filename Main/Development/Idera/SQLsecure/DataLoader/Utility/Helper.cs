using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Idera.SQLsecure.Collector.Utility
{
    internal static class Helper
    {
        public static ServerType ConvertSQLTypeStringToEnum(string stype)
        {
            return stype == "OP" ? ServerType.OnPremise : (stype == "ADB" ? ServerType.AzureSQLDatabase : (stype == "AVM" ? ServerType.SQLServerOnAzureVM : ServerType.Null));
        }
    }
    public enum ServerType
    {
        Null,
        OnPremise,//On-Premise
        AzureSQLDatabase,//Azure SqlDatabase
        SQLServerOnAzureVM//Azure VM
    }
}
