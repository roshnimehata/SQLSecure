using Microsoft.Win32;
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
		
		
        // SQLSecure 3.1 (Biresh Kumar Mishra) - Add support for Azure VM
        public static RegistryKey OpenRemoteBaseKey(RegistryHive enumRegistryHive, string serverName, bool checkForDomain = true)
        {
            RegistryKey remoteBaseKey = null;
            try
            {
                remoteBaseKey = RegistryKey.OpenRemoteBaseKey(enumRegistryHive, serverName);

                // test for valid key
                switch(enumRegistryHive)
                {
                    case RegistryHive.LocalMachine:
                        remoteBaseKey.OpenSubKey(@"Software");
                        break;
                    case RegistryHive.CurrentUser:
                        remoteBaseKey.OpenSubKey(@"System");
                        break;
                    case RegistryHive.ClassesRoot:
                        remoteBaseKey.OpenSubKey(@"Windows Media");
                        break;
                    case RegistryHive.Users:
                        remoteBaseKey.OpenSubKey(@"S-1-5-18");
                        break;
                    case RegistryHive.CurrentConfig:
                        remoteBaseKey.OpenSubKey(@"System");
                        break;
                }
                
            }
            catch (Exception ex)
            {                
                if (checkForDomain && (serverName.IndexOf(".") != -1))
                {
                    remoteBaseKey =
                   RegistryKey.OpenRemoteBaseKey(enumRegistryHive,
                                                  serverName.Substring(0, serverName.IndexOf(".")));

                    // test for valid key
                    switch (enumRegistryHive)
                    {
                        case RegistryHive.LocalMachine:
                            remoteBaseKey.OpenSubKey(@"Software");
                            break;
                        case RegistryHive.CurrentUser:
                            remoteBaseKey.OpenSubKey(@"System");
                            break;
                        case RegistryHive.ClassesRoot:
                            remoteBaseKey.OpenSubKey(@"Windows Media");
                            break;
                        case RegistryHive.Users:
                            remoteBaseKey.OpenSubKey(@"S-1-5-18");
                            break;
                        case RegistryHive.CurrentConfig:
                            remoteBaseKey.OpenSubKey(@"System");
                            break;
                    }
                }
                else
                {
                    throw; //caller should handle this exception
                }
            }
            return remoteBaseKey;
        }

    }
    public enum ServerType
    {
        OnPremise,//On-Premise
        AzureSQLDatabase,//Azure SqlDatabase
        SQLServerOnAzureVM//Azure VM
    }
}
