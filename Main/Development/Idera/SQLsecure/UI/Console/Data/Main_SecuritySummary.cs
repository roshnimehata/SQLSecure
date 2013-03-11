/******************************************************************
 * Name: Main_SecuritySummary.cs
 *
 * Description: Main_SecuritySummary view data class.
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
    class Main_SecuritySummary : Interfaces.IDataContext
    {
        #region Fields

        private String m_Name;
        private Sql.Policy m_Policy;
        private Sql.RegisteredServer m_Server;
        private Views.View_Main_SecuritySummary.SecurityView m_SecurityView = Views.View_Main_SecuritySummary.SecurityView.None;

        #endregion

        #region Ctors

        public Main_SecuritySummary()
        {
            m_Policy = null;
            m_Server = null;
            m_Name = "No Policy Selected";
        }

        public Main_SecuritySummary(Sql.Policy policyIn)
        {
            Debug.Assert(policyIn != null, "Security Summary called with null Policy");

            m_Policy = policyIn;
            m_Server = null;
            m_Name = m_Policy.PolicyName;
        }

        public Main_SecuritySummary(Sql.Policy policyIn, Views.View_Main_SecuritySummary.SecurityView showTabIn)
        {
            Debug.Assert(policyIn != null, "Security Summary called with null Policy");

            m_Policy = policyIn;
            m_Server = null;
            m_Name = m_Policy.PolicyName;
            m_SecurityView = showTabIn;
        }

        public Main_SecuritySummary(Sql.Policy policyIn, Sql.RegisteredServer serverIn)
        {
            Debug.Assert(policyIn != null, "Security Summary called with null Policy");
            Debug.Assert(serverIn != null, "Security Summary called with null Server");

            m_Policy = policyIn;
            m_Server = serverIn;
            m_Name = string.Format("{0}::{1}", m_Policy.PolicyName, m_Server.ConnectionName);

            Program.gController.SetCurrentPolicy(m_Policy);
        }

        public Main_SecuritySummary(Sql.Policy policyIn, Sql.RegisteredServer serverIn, Views.View_Main_SecuritySummary.SecurityView showTabIn)
        {
            Debug.Assert(policyIn != null, "Security Summary called with null Policy");
            Debug.Assert(serverIn != null, "Security Summary called with null Server");

            m_Policy = policyIn;
            m_Server = serverIn;
            m_Name = string.Format("{0}::{1}", m_Policy.PolicyName, m_Server.ConnectionName);
            m_SecurityView = showTabIn;

            Program.gController.SetCurrentPolicy(m_Policy);
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

        public Views.View_Main_SecuritySummary.SecurityView SecurityView
        {
            get { return m_SecurityView; }
        }

        #endregion
    }
}
