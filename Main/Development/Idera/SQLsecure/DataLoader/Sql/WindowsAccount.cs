/******************************************************************
 * Name: WindowsAccount.cs
 *
 * Description: Encapsulates storing of windows account and group
 * membership information into the repository.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Diagnostics;
using Idera.SQLsecure.Core.Accounts;
using Idera.SQLsecure.Core.Logger;

namespace Idera.SQLsecure.Collector.Sql
{
    internal static class WindowsAccount
    {
        private static LogX logX = new LogX("Idera.SQLsecure.Collector.Sql.WindowsAccount");

        private static bool atBatchSize(
                DataTable dtAccount,
                DataTable dtMembership
            )
        {
            return dtAccount.Rows.Count > Constants.RowBatchSize
                    || dtMembership.Rows.Count > Constants.RowBatchSize;
        }

        private static void writeDataTables (
                SqlBulkCopy bcpAccount,
                DataTable dtAccount,
                SqlBulkCopy bcpMembership,
                DataTable dtMembership
            )
        {
            // Write the account table first, because there are FK constraints.
            if (dtAccount.Rows.Count != 0)
            {
                bcpAccount.BulkCopyTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                bcpAccount.WriteToServer(dtAccount);
                dtAccount.Clear();
            }

            // Write the membership table.
            if (dtMembership.Rows.Count != 0)
            {
                bcpMembership.BulkCopyTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                bcpMembership.WriteToServer(dtMembership);
                dtMembership.Clear();
            }
        }

        private static bool atBatchSize(DataTable dtAccount)
        {
            return dtAccount.Rows.Count > Constants.RowBatchSize;
        }

        private static void writeDataTable(
                SqlBulkCopy bcpAccount,
                DataTable dtAccount
            )
        {
            // Write the account table.
            if (dtAccount.Rows.Count != 0)
            {
                bcpAccount.WriteToServer(dtAccount);
                dtAccount.Clear();
            }
        }

        private static void addToAccountDataTable(
                DataTable dtAccount,
                int snapshotid,
                Account account,
                bool windowsOSAccount
            )
        {
            DataRow dr = dtAccount.NewRow();
            dr[WindowsAccountDataTable.ParamSnapshotid] = snapshotid;
            dr[WindowsAccountDataTable.ParamSid] = account.SID.BinarySid;
            dr[WindowsAccountDataTable.ParamType] = account.Class.ToString();
            dr[WindowsAccountDataTable.ParamState] = Account.GetStatusStringFromEnum(account.AccountStatus);
            if (string.IsNullOrEmpty(account.SamPath))
            {
                dr[WindowsAccountDataTable.ParamName] = account.SID.SidString;
            }
            else
            {
                dr[WindowsAccountDataTable.ParamName] = account.SamPath;
            }
            if (!windowsOSAccount)
            {
                dr[WindowsAccountDataTable.ParamEnabled] = account.Enabled;
            }
            dtAccount.Rows.Add(dr);
        }

        private static void addToMembershipDataTable(
                DataTable dtMembership,
                int snapshotid,
                Account group,
                Account member
            )
        {
            Target.numGroupMembersCollected++;
            DataRow dr = dtMembership.NewRow();
            dr[WindowsGroupMemberDataTable.ParamSnapshotid] = snapshotid;
            dr[WindowsGroupMemberDataTable.ParamGroupsid] = group.SID.BinarySid;
            dr[WindowsGroupMemberDataTable.ParamGroupmember] = member.SID.BinarySid;
            dr[WindowsGroupMemberDataTable.ParamHashkey] = "";
            dtMembership.Rows.Add(dr);
        }

        public static bool Process(
                string repositoryConnection,
                int snapshotid,
                List<Account> users,
                Dictionary<Account, List<Account>> groupMemberhips
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(repositoryConnection));
            Debug.Assert(groupMemberhips != null);

            // If no group membership or users info, return.
            if (groupMemberhips.Count == 0 && users.Count == 0)
            {
                return true;
            }

            // Create a set to keep track of accounts already processed.
            List<string> accountSID = new List<string>();

            // Open the connections.
            bool isOk = true;
            Program.ImpersonationContext wi = Program.SetLocalImpersonationContext();
            try
            {
                using (SqlConnection repository = new SqlConnection(repositoryConnection))
                {
                    // Open repository connection.
                    repository.Open();
                    if (groupMemberhips.Count > 0)
                    {
                        // Use bulk copy object to write group members to repository.
                        using (SqlBulkCopy
                                    bcpAccount = new SqlBulkCopy(repository),
                                    bcpMembership = new SqlBulkCopy(repository)
                              )
                        {
                            // Set the destination tables.
                            bcpAccount.DestinationTableName = WindowsAccountDataTable.RepositoryTable;
                            bcpAccount.BulkCopyTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                            bcpMembership.DestinationTableName = WindowsGroupMemberDataTable.RepositoryTable;
                            bcpMembership.BulkCopyTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                            // Create the datatable to write to the repository.
                            using (DataTable
                                        dtAccount = WindowsAccountDataTable.Create(),
                                        dtMembership = WindowsGroupMemberDataTable.Create()
                                  )
                            {
                                // Store group members to the repository.
                                foreach (KeyValuePair<Account, List<Account>> group in groupMemberhips)
                                {
                                    // If group is not in the account set, then update.
                                    if (!accountSID.Contains(group.Key.SID.SidString))
                                    {
                                        // Add to the set for subsequent checks.
                                        accountSID.Add(group.Key.SID.SidString);

                                        // Add the group to the account data table.
                                        addToAccountDataTable(dtAccount, snapshotid, group.Key, false);
                                    }

                                    // Process group members.
                                    foreach (Account member in group.Value)
                                    {
                                        // If member is not in the account set, then update.
                                        if (!accountSID.Contains(member.SID.SidString))
                                        {
                                            // Add to the set for subsequent checks.
                                            accountSID.Add(member.SID.SidString);

                                            // Add the member to the account data table.
                                            addToAccountDataTable(dtAccount, snapshotid, member, false);
                                        }

                                        // Add the member to the member data table.
                                        addToMembershipDataTable(dtMembership, snapshotid, group.Key, member);

                                        // Write to the repository if reached the batch size.
                                        if (atBatchSize(dtAccount, dtMembership))
                                        {
                                            writeDataTables(bcpAccount, dtAccount, bcpMembership, dtMembership);
                                        }
                                    }
                                }

                                // Write any remaining data to the repository.
                                writeDataTables(bcpAccount, dtAccount, bcpMembership, dtMembership);
                            }
                        }
                    }

                    if (users.Count > 0)
                    {

                        // Use bulk copy to write users to the repository.
                        using (SqlBulkCopy bcpAccount = new SqlBulkCopy(repository))
                        {
                            // Set the destination tables.
                            bcpAccount.DestinationTableName = WindowsAccountDataTable.RepositoryTable;
                            bcpAccount.BulkCopyTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                            // Create the datatable to write to the repository.
                            using (DataTable dtAccount = WindowsAccountDataTable.Create())
                            {
                                // Process each user.
                                foreach (Account user in users)
                                {
                                    // If member is not in the account set, then update.
                                    if (!accountSID.Contains(user.SID.SidString))
                                    {
                                        // Add to the set for subsequent checks.
                                        accountSID.Add(user.SID.SidString);

                                        // Add the member to the account data table.
                                        addToAccountDataTable(dtAccount, snapshotid, user, false);
                                    }

                                    // Write to the repository if reached the batch size.
                                    if (atBatchSize(dtAccount))
                                    {
                                        writeDataTable(bcpAccount, dtAccount);
                                    }
                                }

                                // Write any remaining data to the repository.
                                writeDataTable(bcpAccount, dtAccount);
                            }
                        }
                    }
                }
            }
            catch ( Exception ex /*SqlException ex*/)
            {
                string strMessage = "Saving windows accounts";
                logX.loggerX.Error("WARNING -" + strMessage, ex);
                Sql.Database.CreateApplicationActivityEventInRepository(repositoryConnection, 
                                                                        snapshotid, 
                                                                        Collector.Constants.ActivityType_Warning, 
                                                                        Collector.Constants.ActivityEvent_Error, 
                                                                        strMessage + ex.Message);
                AppLog.WriteAppEventWarning(SQLsecureEvent.ExErrExceptionRaised, SQLsecureCat.DlDataLoadCat,
                                            strMessage, ex.Message);

                isOk = false;
            }
            finally
            {
                Program.RestoreImpersonationContext(wi);
            }
            return isOk;
        }

        public static bool ProcessOSObjects(
                string repositoryConnection,
                int snapshotid,
                List<Account> users,
                Dictionary<Account, List<Account>> groupMemberhips
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(repositoryConnection));
            Debug.Assert(groupMemberhips != null);

            // If no group membership or users info, return.
            if (groupMemberhips.Count == 0 && users.Count == 0)
            {
                return true;
            }

            // Create a set to keep track of accounts already processed.
            List<string> accountSID = new List<string>();

            // Open the connections.
            bool isOk = true;
            Program.ImpersonationContext wi = Program.SetLocalImpersonationContext();
            try
            {
                using (SqlConnection repository = new SqlConnection(repositoryConnection))
                {
                    // Open repository connection.
                    repository.Open();
                    if (groupMemberhips.Count > 0)
                    {
                        // Use bulk copy object to write group members to repository.
                        using (SqlBulkCopy
                                    bcpAccount = new SqlBulkCopy(repository),
                                    bcpMembership = new SqlBulkCopy(repository)
                              )
                        {
                            // Set the destination tables.
                            bcpAccount.DestinationTableName = WindowsOSAccountDataTable.RepositoryTable;
                            bcpAccount.BulkCopyTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                            bcpMembership.DestinationTableName = WindowsOSGroupMemberDataTable.RepositoryTable;
                            bcpMembership.BulkCopyTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();

                            // Create the datatable to write to the repository.
                            using (DataTable
                                        dtAccount = WindowsOSAccountDataTable.Create(),
                                        dtMembership = WindowsOSGroupMemberDataTable.Create()
                                  )
                            {
                                // Store group members to the repository.
                                foreach (KeyValuePair<Account, List<Account>> group in groupMemberhips)
                                {
                                    // If group is not in the account set, then update.
                                    if (!accountSID.Contains(group.Key.SID.SidString))
                                    {
                                        // Add to the set for subsequent checks.
                                        accountSID.Add(group.Key.SID.SidString);

                                        // Add the group to the account data table.
                                        addToAccountDataTable(dtAccount, snapshotid, group.Key, true);
                                    }

                                    // Process group members.
                                    foreach (Account member in group.Value)
                                    {
                                        // If member is not in the account set, then update.
                                        if (!accountSID.Contains(member.SID.SidString))
                                        {
                                            // Add to the set for subsequent checks.
                                            accountSID.Add(member.SID.SidString);

                                            // Add the member to the account data table.
                                            addToAccountDataTable(dtAccount, snapshotid, member, true);
                                        }

                                        // Add the member to the member data table.
                                        addToMembershipDataTable(dtMembership, snapshotid, group.Key, member);

                                        // Write to the repository if reached the batch size.
                                        if (atBatchSize(dtAccount, dtMembership))
                                        {
                                            writeDataTables(bcpAccount, dtAccount, bcpMembership, dtMembership);
                                        }
                                    }
                                }

                                // Write any remaining data to the repository.
                                writeDataTables(bcpAccount, dtAccount, bcpMembership, dtMembership);
                            }
                        }
                    }

                    if (users.Count > 0)
                    {

                        // Use bulk copy to write users to the repository.
                        using (SqlBulkCopy bcpAccount = new SqlBulkCopy(repository))
                        {
                            // Set the destination tables.
                            bcpAccount.DestinationTableName = WindowsOSAccountDataTable.RepositoryTable;
                            bcpAccount.BulkCopyTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                            // Create the datatable to write to the repository.
                            using (DataTable dtAccount = WindowsOSAccountDataTable.Create())
                            {
                                // Process each user.
                                foreach (Account user in users)
                                {
                                    // If member is not in the account set, then update.
                                    if (!accountSID.Contains(user.SID.SidString))
                                    {
                                        // Add to the set for subsequent checks.
                                        accountSID.Add(user.SID.SidString);

                                        // Add the member to the account data table.
                                        addToAccountDataTable(dtAccount, snapshotid, user, true);
                                    }

                                    // Write to the repository if reached the batch size.
                                    if (atBatchSize(dtAccount))
                                    {
                                        writeDataTable(bcpAccount, dtAccount);
                                    }
                                }

                                // Write any remaining data to the repository.
                                writeDataTable(bcpAccount, dtAccount);
                            }
                        }
                    }
                }
            }
            catch (Exception ex /*SqlException ex*/)
            {
                string strMessage = "Saving windows accounts";
                logX.loggerX.Error("WARNING -" + strMessage, ex);
                Sql.Database.CreateApplicationActivityEventInRepository(repositoryConnection,
                                                                        snapshotid,
                                                                        Collector.Constants.ActivityType_Warning,
                                                                        Collector.Constants.ActivityEvent_Error,
                                                                        strMessage + ex.Message);
                AppLog.WriteAppEventWarning(SQLsecureEvent.ExErrExceptionRaised, SQLsecureCat.DlDataLoadCat,
                                            strMessage, ex.Message);

                isOk = false;
            }
            finally
            {
                Program.RestoreImpersonationContext(wi);
            }
            return isOk;
        }

    }
}
