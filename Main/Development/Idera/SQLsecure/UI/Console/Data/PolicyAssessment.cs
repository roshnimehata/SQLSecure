/******************************************************************
 * Name: PolicyAssessment.cs
 *
 * Description: PolicyAssessment view data class.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2008 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Idera.SQLsecure.UI.Console.Data
{
    class PolicyAssessment : Interfaces.IDataContext
    {
        #region Fields

        private String m_Name;
        private Sql.Policy m_Policy;
        private Sql.RegisteredServer m_Server;
        private Views.View_PolicyAssessment.AssessmentView m_AssessmentView = Views.View_PolicyAssessment.AssessmentView.None;

        #endregion

        #region Ctors

        public PolicyAssessment()
        {
            m_Policy = null;
            m_Server = null;
            m_Name = "No Assessment Selected";
        }

        public PolicyAssessment(Sql.Policy policyIn)
        {
            Debug.Assert(policyIn != null, "Policy Assessment called with null Policy");

            m_Policy = policyIn;
            m_Server = null;
            m_Name = m_Policy.PolicyAssessmentName;
        }

        public PolicyAssessment(Sql.Policy policyIn, Views.View_PolicyAssessment.AssessmentView showTabIn)
        {
            Debug.Assert(policyIn != null, "Security Summary called with null Policy");

            m_Policy = policyIn;
            m_Server = null;
            m_Name = m_Policy.PolicyAssessmentName;
            m_AssessmentView = showTabIn;
        }

        public PolicyAssessment(Sql.Policy policyIn, Sql.RegisteredServer serverIn)
        {
            Debug.Assert(policyIn != null, "Policy Assessment called with null Policy");
            Debug.Assert(serverIn != null, "Policy Assessment called with null Server");

            m_Policy = policyIn;
            m_Server = serverIn;
            m_Name = string.Format("{0}::{1}", m_Policy.PolicyAssessmentName, m_Server.ConnectionName);

            Program.gController.SetCurrentPolicyAssessment(m_Policy);
        }

        public PolicyAssessment(Sql.Policy policyIn, Sql.RegisteredServer serverIn, Views.View_PolicyAssessment.AssessmentView showTabIn)
        {
            Debug.Assert(policyIn != null, "Policy Assessment called with null Policy");
            Debug.Assert(serverIn != null, "Policy Assessment called with null Server");

            m_Policy = policyIn;
            m_Server = serverIn;
            m_Name = string.Format("{0}::{1}", m_Policy.PolicyAssessmentName, m_Server.ConnectionName);
            m_AssessmentView = showTabIn;

            Program.gController.SetCurrentPolicyAssessment(m_Policy);
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

        public Views.View_PolicyAssessment.AssessmentView AssessmentView
        {
            get { return m_AssessmentView; }
        }

        #endregion
    }
}
