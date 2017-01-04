using Idera.SQLsecure.UI.Console.Sql;
using System;

namespace Idera.SQLsecure.UI.Console.Data
{
    public class ServerInfo
    {
        #region Private_Members

        private ServerVersion m_version;
        private string m_login;
        private string m_password;
        private string m_connectionName;
        private bool m_windowsAuth;
        private string m_serverType;

        #endregion

        public ServerInfo(ServerVersion version, bool windowsAuth, string login, string password, string connectionName,string serverType)
        {
            this.m_connectionName = connectionName;
            this.m_login = login;
            this.m_password = password;
            this.m_version = version;
            this.m_windowsAuth = windowsAuth;
            this.m_serverType = serverType;
        }

        public ServerVersion version
        {
            get { return m_version; }
        }

        public string login
        {
            get { return m_login; }
        }

        public string password
        {
            get { return m_password; }
        }

        public string connectionName
        {
            get { return m_connectionName; }
        }

        public bool windowsAuth
        {
            get { return m_windowsAuth; }
        }

        public string serverType
        {
            get { return m_serverType; }
        }
    }
}
