using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Idera.SQLsecure.Collector.Constants;

namespace Idera.SQLsecure.Collector.Sql
{
    /// <summary>
    /// SQLSecure 3.1 (Anshul Aggarwal) - New model class to represent Firewall rules for Azure SQL Database. 
    /// </summary>
    internal class AzureSqlDBFirewallRule
    {
        #region fields
        private string m_name;
        private string m_startIPAddress;
        private string m_endIPAddress;
        private bool m_isServerLevel;
        private int m_dbid;
        #endregion

        #region CTOR

        public AzureSqlDBFirewallRule(string name, int dbid, bool isServerLevel, string startIPAddress, string endIPAddress)
        {
            m_name = name;
            m_isServerLevel = isServerLevel;
            m_startIPAddress = startIPAddress;
            m_endIPAddress = endIPAddress;
            m_dbid = dbid;
        }

        #endregion

        public string Name
        {
            get
            {
                return m_name;
            }
        }

        public int DBId
        {
            get
            {
                return m_dbid;
            }
        }

        public bool IsServerLevel
        {
            get
            {
                return m_isServerLevel;
            }
        }

        public string StartIPAddress
        {
            get
            {
                return m_startIPAddress;
            }
        }

        public string EndIPAddress
        {
            get
            {
                return m_endIPAddress;
            }
        }
    }
}
