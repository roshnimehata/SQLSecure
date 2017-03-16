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
            stype = stype.ToUpper();
            return (stype == "ADB" ? ServerType.AzureSQLDatabase : (stype == "AVM" ? ServerType.SQLServerOnAzureVM : ServerType.OnPremise));
        }

        /// <summary>
        /// Checking for default sql users in case of Azure SQL DB
        /// </summary>
        /// <param name="name">User name</param>
        /// <returns></returns>
        public static bool CheckForSystemSqlUsers(string name)
        {
            if(String.Compare((string)name, Constants.sysUser, true) == 0 || String.Compare((string)name, Constants.dboUser, true) == 0 || String.Compare((string)name, Constants.guestUser, true) == 0 || String.Compare((string)name, Constants.informationSchemaUser, true) == 0)
            {
                return true;
            }
            return false;
        }
    }
    public enum ServerType
    {
        OnPremise,//On-Premise
        AzureSQLDatabase,//Azure SqlDatabase
        SQLServerOnAzureVM//Azure VM
    }
}
