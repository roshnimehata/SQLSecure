using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using Idera.SQLsecure.Core.Logger;

namespace Idera.SQLsecure.UI.Console.Utility
{
    class Security
    {
        #region Enums
        public enum Functions : int
        {
            //Repository functions
            // always available Connect,
            // use general properties ConnectionProperties,
            AuditSQLServer,
            NewLogin,
            ManageLicense,

            //Server functions
            ConfigureAuditSettings,

            //General and multipurpose functions
            Delete,
            Refresh,
            Properties,
            Print,
            ChangeUI,           // generic value for menu options to manipulate look & feel
            UserPermissions,
            ObjectPermissions,

            //Snapshot functions
            Collect,
            Baseline,
            GroomingSchedule,

            SIZE // always the last item, so it tells the array size
        }

        public enum AccessLevel : int
        {
            NoAccess,
            View,
            Admin
        }

        #endregion

        #region Ctors

        public Security()
        {
            // Define the admin functions
            m_AdminFunction = new Boolean[(int)Functions.SIZE];
            m_AdminFunction[(int)Functions.AuditSQLServer] = true;
            m_AdminFunction[(int)Functions.NewLogin] = true;
            m_AdminFunction[(int)Functions.ManageLicense] = true;
            m_AdminFunction[(int)Functions.ConfigureAuditSettings] = true;
            m_AdminFunction[(int)Functions.Delete] = true;
            m_AdminFunction[(int)Functions.Collect] = true;
            m_AdminFunction[(int)Functions.Baseline] = true;
            m_AdminFunction[(int)Functions.GroomingSchedule] = true;

            // There is no valid security on creation
            this.Clear();
        }

        #endregion

        #region Fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Utility.Security");
        private AccessLevel m_AccessLevel = AccessLevel.NoAccess;

        private Boolean[] m_AdminFunction;
        private Boolean[] m_Security;

        #endregion

        #region Properties

        public AccessLevel UserAccessLevel
        {
            get { return m_AccessLevel; }
        }
        public Boolean isAdmin
        {
            get { return m_AccessLevel.Equals(AccessLevel.Admin); }
        }

        public Boolean isViewer
        {
            get { return m_AccessLevel.Equals(AccessLevel.View) || isAdmin; }
        }

        #endregion

        #region Queries

        private const string QueryGetSQLsecurePermissions =
                    @"EXEC SQLsecure.dbo.isp_sqlsecure_getuserapplicationrole";

        #endregion

        #region Helpers

        public void Clear()
        {
            // Initialize the security to all false
            m_AccessLevel = AccessLevel.NoAccess;
            m_Security = new Boolean[(int)Functions.SIZE];
        }

        public void checkRepositorySecurity(string connectionString)
        {
            // Check permissions in the repository.
            m_AccessLevel = AccessLevel.NoAccess;
            logX.loggerX.Info("Checking user permissions to Repository");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open the connection.
                connection.Open();

                using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                    QueryGetSQLsecurePermissions, null))
                {
                    while (rdr.Read())
                    {
                        string role = (string)rdr[0];
                        if (String.Compare(role, Utility.Constants.Role_Admin, true) == 0)
                        {
                            m_AccessLevel = AccessLevel.Admin;
                            // This is the highest level, so we are done
                            break;
                        }
                        else if (String.Compare(role, Utility.Constants.Role_View, true) == 0)
                        {
                            m_AccessLevel = AccessLevel.View;
                            // This is valid, but could be superseded by Admin, so keep looking
                        }
                    }
                }
            }

            // Build the current security access levels
            for (int i = 0; i < m_Security.Length; i++)
            {
                m_Security[i] = m_AdminFunction[i] ? isAdmin : isViewer;
            }
        }

        public Boolean hasSecurity(Functions function)
        {
            return m_Security[(int)function];
        }

        #endregion
    }
}
