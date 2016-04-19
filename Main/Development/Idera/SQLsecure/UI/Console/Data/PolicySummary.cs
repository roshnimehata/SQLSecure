/******************************************************************
 * Name: PolicySummary.cs
 *
 * Description: Controls.PolicySummary data class.
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
    class PolicySummary : Interfaces.IDataContext
    {
        #region Fields

        private String m_Name;
        private Sql.Policy m_Policy;
        private Sql.RegisteredServer m_Server = null;
        private bool m_UseBaseline = false;
        private DateTime? m_SelectionDate = null;

        #endregion

        #region Ctors

        public PolicySummary()
        {
            m_Policy = null;
            m_Server = null;
            m_Name = "No Policy Selected";
        }

        public PolicySummary(Sql.Policy policyIn)
        {
            Debug.Assert(policyIn != null, "Policy Summary called with null Policy");
            Debug.Assert(policyIn.IsAssessment, "Policy Summary called with Policy that should be Assessment");

            m_Policy = policyIn;
            m_UseBaseline = m_Policy.UseBaseline;
            m_SelectionDate = m_Policy.AssessmentDate;

            m_Name = m_Policy.PolicyAssessmentName;
        }

        public PolicySummary(Sql.Policy policyIn, Sql.RegisteredServer serverIn)
        {
            Debug.Assert(policyIn != null, "Policy Summary called with null Policy");
            Debug.Assert(policyIn.IsAssessment, "Policy Summary called with Policy that should be Assessment");

            m_Policy = policyIn;
            m_UseBaseline = m_Policy.UseBaseline;
            m_SelectionDate = m_Policy.AssessmentDate;
            m_Server = serverIn;

            m_Name = string.Format("{0}::{1}", m_Policy.PolicyAssessmentName, m_Server.ConnectionName);
        }

        public PolicySummary(Sql.Policy policyIn, bool useBaselineIn, DateTime? selectionDateIn)
        {
            Debug.Assert(policyIn != null, "Policy Summary called with null Policy");

            m_Policy = policyIn;
            m_UseBaseline = useBaselineIn;
            m_SelectionDate = selectionDateIn;
 
            m_Name = m_Policy.PolicyName;
        }

        public PolicySummary(Sql.Policy policyIn, bool useBaselineIn, DateTime? selectionDateIn, Sql.RegisteredServer serverIn)
        {
            Debug.Assert(policyIn != null, "Policy Summary called with null Policy");
            Debug.Assert(serverIn != null, "Policy Summary called with null Server");

            m_Policy = policyIn;
            m_UseBaseline = useBaselineIn;
            m_SelectionDate = selectionDateIn;
            m_Server = serverIn;

            m_Name = string.Format("{0}::{1}", m_Policy.PolicyName, m_Server.ConnectionName);
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
