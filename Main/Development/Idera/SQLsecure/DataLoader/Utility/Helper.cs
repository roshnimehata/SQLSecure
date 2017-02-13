using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Idera.SQLsecure.Collector.Utility
{
    /// <summary>
    /// Helper class is having utility methods 
    /// </summary>
    internal static class Helper
    {
        /// <summary>
        /// Converting server type string to server type Enum
        /// Default value is on Premise
        /// </summary>
        /// <param name="stype"></param>
        /// <returns></returns>
        public static ServerType ConvertSQLTypeStringToEnum(string stype)
        {
            return (stype == "ADB" ? ServerType.AzureSQLDatabase : (stype == "AVM" ? ServerType.SQLServerOnAzureVM : ServerType.OnPremise));
        }
    }
    public enum ServerType
    {
        OnPremise,//On-Premise
        AzureSQLDatabase,//Azure SqlDatabase
        SQLServerOnAzureVM//Azure VM
    }
}
