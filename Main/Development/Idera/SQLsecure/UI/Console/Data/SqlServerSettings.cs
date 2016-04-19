/******************************************************************
 * Name: SqlServerSettings.cs
 *
 * Description: Controls.SqlServerSettings data class.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Idera.SQLsecure.UI.Console.Data
{
    class SqlServerSettings : Interfaces.IDataContext
    {
        #region Fields

        private String m_Name;
        private Sql.Policy m_Policy;
        private Sql.RegisteredServer m_Server = null;
        private bool m_UseBaseline = false;
        private DateTime? m_SelectionDate = null;

        #endregion

        #region Ctors

        public SqlServerSettings()
        {
            m_Policy = null;
            m_Name = "No Policy Selected";
        }

        public SqlServerSettings(Sql.Policy policyIn, bool useBaselineIn, DateTime? selectionDateIn)
        {
            Debug.Assert(policyIn != null, "Sql Server Settings called with null Policy");

            m_Policy = policyIn;
            m_UseBaseline = useBaselineIn;
            m_SelectionDate = selectionDateIn;

            m_Name = m_Policy.PolicyName;
        }

        public SqlServerSettings(Sql.Policy policyIn, bool useBaselineIn, DateTime? selectionDateIn, Sql.RegisteredServer serverIn)
        {
            Debug.Assert(policyIn != null, "Sql Server Settings called with null Policy");
            Debug.Assert(serverIn != null, "Sql Server Settings called with null Server");

            m_Policy = policyIn;
            m_UseBaseline = useBaselineIn;
            m_SelectionDate = selectionDateIn;
            m_Server = serverIn;

            m_Name = m_Server.ConnectionName;
        }

        #endregion

        #region IDataContext

        String Interfaces.IDataContext.Name
        {
            get { return m_Name; }
        }

        public Sql.Policy Policy
        {
            get { return m_Policy; }
        }

        public Sql.RegisteredServer Server
        {
            get { return m_Server; }
        }

        public bool UseBaseline
        {
            get { return m_UseBaseline; }
        }

        public DateTime? SelectionDate
        {
            get { return m_SelectionDate; }
        }

        #endregion
    }
}
