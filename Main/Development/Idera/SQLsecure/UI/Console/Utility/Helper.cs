using Idera.SQLsecure.Core.Logger;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Idera.SQLsecure.UI.Console.Utility
{
    class Helper
    {
        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Utility.Helper");
        public static ServerType ConvertSQLTypeStringToEnum(string stype)
        {
            stype = stype.ToUpper();
            return (stype == "ADB" ? ServerType.AzureSQLDatabase : (stype == "AVM" ? ServerType.SQLServerOnAzureVM : ServerType.OnPremise));
        }

        //Start-SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
        public static int AzureADUsersAndGroupCount(int snapshotId)
        {
            int numOfAzureADAccounts = 0;
            string query = string.Format("select COUNT(*) from SQLsecure.dbo.serverprincipal a where a.snapshotid = {0} and type in ('E', 'X')", snapshotId);
            using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.CommandType = CommandType.Text;
                    try
                    {
                        numOfAzureADAccounts = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        logX.loggerX.Info("ERROR - unable to load Azure AD Accounts from the selected Snapshot.", ex);
                        Utility.MsgBox.ShowError(Utility.ErrorMsgs.SnapshotPropertiesCaption, ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return numOfAzureADAccounts;
        }
        //End-SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
    }
}
