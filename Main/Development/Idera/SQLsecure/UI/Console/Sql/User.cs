/******************************************************************
 * Name: User.cs
 *
 * Description: Encapsulates a user object.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;

using Idera.SQLsecure.Core.Accounts;
using Idera.SQLsecure.Core.Interop;
using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Utility;

namespace Idera.SQLsecure.UI.Console.Sql
{
    /// <summary>
    /// Encapsulates a User instance
    /// </summary>
    public class User
    {
        public enum UserSource
        {
            Snapshot,
            ActiveDirectory,
            UserEntry,
            Unknown
        }

        #region Ctors

        //public User(string name, Sid sid, string loginType, string userType)
        //{
        //    m_Sid = sid;
        //    m_Name = name;
        //    m_LoginType = loginType;
        //    m_UserType = userType;
        //    m_UserSource = UserSource.Unknown;
        //    m_isVerified = false;
        //}

        public User(string name, Sid sid, string loginType, UserSource source)
        {
            m_Sid = sid;
            m_Name = name;
            m_LoginType = loginType;
            //m_UserType = "";
            m_UserSource = source;
            m_isVerified = false;
        }

        #endregion

        #region Fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.User.UserSource");
        private Sid m_Sid;
        private String m_Name;
        private String m_LoginType;
        //private String m_UserType;
        private UserSource m_UserSource;
        private bool m_isVerified;

        #endregion

        #region Properties

        public Sid Sid { get { return m_Sid; } }
        public String Name { get { return m_Name; } }
        public String LoginType { get { return m_LoginType; } }
        //public String UserType { get { return m_UserType; } }
        public UserSource Source { get { return m_UserSource; } }
        public bool IsVerified { get { return m_isVerified; } set { m_isVerified = value; } }

        public String Domain
        {
            get
            {
                string domain = "";
                string user;

                Path.SplitSamPath(Name, out domain, out user);
                return domain;
            }
        }

        #endregion

        #region Queries

        // make these queries case insensitive in case repository is on a case sensitive server
        private static string QueryGetSnapshotUser = @"SELECT name, type, sid FROM SQLsecure.dbo.vwserverprincipal WHERE snapshotid = {0} AND lower(type) IN ({1}) AND lower(name) = '{2}'";
        private static string QueryGetSnapshotWindowsUser = @"SELECT name, type, sid FROM SQLsecure.dbo.vwwindowsaccount WHERE snapshotid = {0} AND lower(name) = '{1}'";
        private static string QueryGetSnapshotWindowsSid = @"SELECT name, type, sid FROM SQLsecure.dbo.vwwindowsaccount WHERE snapshotid = {0} AND sid = {1}";
        private static string QueryGetSnapshotWindowsUserForAzureSQLDatabase = "select name, type, sid from SQLsecure.dbo.serverprincipal a where a.snapshotid = {0} and name = '{1}'";//SQLsecure 3.1 (Tushar)--Fix for issue SQLSECU-1971

        // Columns for handling the Snapshot query results
        private enum UserColumn
        {
            colName = 0,
            colType,
            colSid
        }
            
        #endregion

        #region Helpers

        static public User GetSnapshotUser(int snapshotId, string name, string loginType, bool showErrorMsg)
        {
            // Create a user object from the name.
            User user = null;
            string query;

            try
            {
                logX.loggerX.Info(@"Retrieve user from snapshot by name and type");
                if (loginType == Sql.LoginType.SqlLogin)
                {
                    query = string.Format(QueryGetSnapshotUser, snapshotId, "'" + ((string)Sql.LoginType.SqlLogin).ToLower() + "'", name.ToLower());
                }
                else
                {
                    query = string.Format(QueryGetSnapshotUser, snapshotId, "'" + ((string)Sql.LoginType.WindowsUser).ToLower() + "','" + ((string)Sql.LoginType.WindowsGroup).ToLower() + "'", name.ToLower());
                }
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    connection.Open();
                    using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                    query, null))
                    {
                        if (rdr.Read())
                        {
                            user = new User(rdr.GetString((int)UserColumn.colName),
                                            new Sid(rdr.GetSqlBytes((int)UserColumn.colSid).Value),
                                            loginType,
                                            UserSource.Snapshot
                                            );
                        }
                        else
                        {
                            if (showErrorMsg)
                            {
                                MsgBox.ShowError(Utility.ErrorMsgs.CantGetUsersCaption, ErrorMsgs.UserNotFoundMsg);
                            }
                        }
                        //Debug.Assert(rdr.Read(),"More than one user returned");
                    }
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error(string.Format(@"Error - Unable to retrieve user {0} from snapshot id {1}", name, snapshotId), ex);
                MsgBox.ShowError(Utility.ErrorMsgs.CantGetUserMsg, ex);
            }

            return user;
        }

        //SQLsecure 3.1 (Tushar)--Fix for issue SQLSECU-1971
        static public User GetSnapshotWindowsUser(int snapshotId, string name, bool showErrorMsg,ServerType serverType)
        {
            // Create a user object from the name passed.
            User user = null;
            string query;

            try
            {
                logX.loggerX.Info(@"Retrieve Windows Login user from snapshot by name");
                
                //Start-SQLsecure 3.1 (Tushar)--Fix for issue SQLSECU-1971
                if (serverType == ServerType.AzureSQLDatabase)
                {
                    query = string.Format(QueryGetSnapshotWindowsUserForAzureSQLDatabase, snapshotId, name);
                }

                else
                {
                    query = string.Format(QueryGetSnapshotWindowsUser, snapshotId, name.ToLower());
                }
                //End-SQLsecure 3.1 (Tushar)--Fix for issue SQLSECU-1971
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    connection.Open();
                    using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                    query, null))
                    {
                        if (rdr.Read())
                        {
                            user = new User(rdr.GetString((int)UserColumn.colName),
                                            new Sid(rdr.GetSqlBytes((int)UserColumn.colSid).Value),
                                            Sql.LoginType.WindowsLogin,
                                            UserSource.Snapshot
                                            );
                        }
                        else
                        {
                            if (showErrorMsg)
                            {
                                MsgBox.ShowError(Utility.ErrorMsgs.CantGetUsersCaption, ErrorMsgs.UserNotFoundMsg);
                            }
                        }
                        //Debug.Assert(rdr.Read(),"More than one user returned");
                    }
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error(string.Format(@"Error - Unable to retrieve user {0} from snapshot id {1}", name, snapshotId), ex);
                MsgBox.ShowError(Utility.ErrorMsgs.CantGetUserMsg, ex);
            }

            return user;
        }

        static public User GetSnapshotWindowsUser(int snapshotId, Sid sid, bool showErrorMsg)
        {
            // Create a user object from the name passed.
            User user = null;
            string query;

            try
            {
                logX.loggerX.Info(@"Retrieve Windows Login user from snapshot by Sid");

                query = string.Format(QueryGetSnapshotWindowsSid, snapshotId, sid.HexString);

                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    connection.Open();
                    using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                    query, null))
                    {
                        if (rdr.Read())
                        {
                            user = new User(rdr.GetString((int)UserColumn.colName),
                                            new Sid(rdr.GetSqlBytes((int)UserColumn.colSid).Value),
                                            Sql.LoginType.WindowsLogin,
                                            UserSource.Snapshot
                                            );
                        }
                        else
                        {
                            if (showErrorMsg)
                            {
                                MsgBox.ShowError(Utility.ErrorMsgs.CantGetUsersCaption, ErrorMsgs.UserNotFoundMsg);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error(string.Format(@"Error - Unable to retrieve Sid {0} from snapshot id {1}", sid.SidString, snapshotId), ex);
                MsgBox.ShowError(Utility.ErrorMsgs.CantGetUserMsg, ex);
            }

            return user;
        }

        static public User GetDomainUser(string name, string loginType, bool showErrorMsg)
        {
            // Create a user object from the name.
            User user = null;
            System.Security.Principal.SecurityIdentifier retSid;
            string retDomain;
            SID_NAME_USE retUse;

            try
            {
                logX.loggerX.Info(@"Retrieve user from domain by name");

                if (Authorization.LookupAccountName("", name, out retSid, out retDomain, out retUse))
                {
                    //switch (retUse)
                    //{
                    //    case SID_NAME_USE.SidTypeUser:
                    //        userType = Sql.LoginType.WindowsUser;
                    //        break;
                    //    case SID_NAME_USE.SidTypeGroup:
                    //        userType = Sql.LoginType.WindowsGroup;
                    //        break;
                    //    case SID_NAME_USE.SidTypeWellKnownGroup:
                    //        userType = Sql.LoginType.WindowsGroup;
                    //        break;
                    //    default:
                    //        DiagLog.LogWarn(@"Unexpected Account Use type received");
                    //        userType = "U";
                    //        break;
                    //}

                    user = new User(name,
                                    new Sid(retSid.ToString()),
                                    loginType,
                                    UserSource.ActiveDirectory
                                    );
                }
                else
                {
                    if (showErrorMsg)
                    {
                        MsgBox.ShowError(Utility.ErrorMsgs.CantGetUsersCaption, ErrorMsgs.UserNotFoundMsg);
                    }
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error(string.Format(@"Error - Unable to retrieve user {0} from Active Directory", name), ex);
                MsgBox.ShowError(Utility.ErrorMsgs.CantGetUserMsg, ex);
            }

            return user;
        }
        static public User GetDomainUser(Sid sid, string loginType, bool showErrorMsg)
        {
            // Create a user object from the sid.
            User user = null;
            string retName;
            string retDomain;
            SID_NAME_USE retUse;

            try
            {
                logX.loggerX.Info(@"Retrieve user from domain by sid");

                if (Authorization.LookupAccountSid("", sid.BinarySid, out retName, out retDomain, out retUse))
                {
                    user = new User(Path.MakeSamPath(retDomain, retName),
                                    sid,
                                    loginType,
                                    UserSource.ActiveDirectory
                                    );
                }
                else
                {
                    if (showErrorMsg)
                    {
                        MsgBox.ShowError(Utility.ErrorMsgs.CantGetUsersCaption, ErrorMsgs.UserNotFoundMsg);
                    }
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error(string.Format(@"Error - Unable to retrieve sid {0} from Active Directory", sid.SidString), ex);
                MsgBox.ShowError(Utility.ErrorMsgs.CantGetUserMsg, ex);
            }

            return user;
        }

        #endregion
    }
}
