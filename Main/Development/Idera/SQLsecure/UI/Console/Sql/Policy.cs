/******************************************************************
 * Name: Policy.cs
 *
 * Description: Encapsulates a SQLsecure security Policy including saved assessments.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2007 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Controls;
using Idera.SQLsecure.UI.Console.Utility;

namespace Idera.SQLsecure.UI.Console.Sql
{
    /// <summary>
    /// Encapsulates a SQLsecure security Policy
    /// </summary>
    public class Policy
    {
        #region Fields

        private SqlInt32 m_PolicyId;
        private SqlInt32 m_AssessmentId;
        private SqlString m_PolicyName;
        private SqlString m_PolicyDescription;
        private SqlBoolean m_IsSystemPolicy;
        private SqlBoolean m_IsDynamic;
        private SqlString m_DynamicSelection = string.Empty;
        private SqlString m_AssessmentName;
        private SqlString m_AssessmentDescription;
        private SqlString m_AssessmentNotes;
        private SqlDateTime m_AssessmentDate;
        private SqlBoolean m_UseBaseline;
        private SqlString m_AssessmentState;

        private List<PolicyMetric> m_PolicyMetrics = null;
        private Repository.AssessmentList m_Assessments = null;

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Sql.Policy");
        private bool m_dirty = false;        
        private List<RegisteredServer> m_MemberList = null;
        private Dictionary<int, int> m_SnapshotList = null;
        private MetricComparer metricComparer = new MetricComparer();

        private SqlBoolean m_InterviewIsTemplate;
        private SqlString m_InterviewName;
        private SqlString m_InterviewText;
        private string m_InterviewTextNew = null;

        private SqlInt32 m_MetricCountHigh;
        private SqlInt32 m_MetricCountMedium;
        private SqlInt32 m_MetricCountLow;
        private SqlInt32 m_FindingCountHigh;
        private SqlInt32 m_FindingCountMedium;
        private SqlInt32 m_FindingCountLow;
        private SqlInt32 m_FindingCountHighExplained;
        private SqlInt32 m_FindingCountMediumExplained;
        private SqlInt32 m_FindingCountLowExplained;

        #endregion

        #region Ctors

        private Policy(SqlDataReader rdr)
        {
            setValues(rdr);
        }

        public Policy()
        {
            // For creating a new Policy
            m_PolicyId = 0;
            m_AssessmentId = 0;
            m_PolicyName = string.Empty;
            m_PolicyDescription = string.Empty;
            m_IsDynamic = false;
            m_IsSystemPolicy = false;
            m_AssessmentName = string.Empty;
            m_AssessmentDescription = string.Empty;
            m_AssessmentNotes = string.Empty;
            m_AssessmentDate = SqlDateTime.Null;
            m_UseBaseline = false;
            m_AssessmentState = string.Empty;
            m_InterviewIsTemplate = false;
            m_InterviewName = @"Primary";
            m_InterviewText = string.Empty;
            m_MetricCountHigh = 0;
            m_MetricCountMedium = 0;
            m_MetricCountLow = 0;
            m_FindingCountHigh = 0;
            m_FindingCountMedium = 0;
            m_FindingCountLow = 0;
            m_FindingCountHighExplained = 0;
            m_FindingCountMediumExplained = 0;
            m_FindingCountLowExplained = 0;
        }

        #endregion

        #region Properties

        [XmlIgnoreAttribute]
        public int PolicyId
        {
            get { return m_PolicyId.Value; }
            set { m_PolicyId = value; }
        }
        [XmlIgnoreAttribute]
        public int AssessmentId
        {
            get { return m_AssessmentId.Value; }
            set { m_AssessmentId = value; }
        }
        public string PolicyName
        {
            get { return m_PolicyName.Value; }
            set { m_PolicyName = value; }
        }
        public string PolicyDescription
        {
            get { return m_PolicyDescription.Value; }
            set
            {
                if (m_PolicyDescription.IsNull || value != m_PolicyDescription.Value)
                {
                    m_dirty = true;
                    m_PolicyDescription = value;
                }
            }
        }
        [XmlIgnoreAttribute]
        public bool IsSystemPolicy
        {
            get { return m_IsSystemPolicy.IsNull ? false : m_IsSystemPolicy.Value; }
            set { m_IsSystemPolicy = value; }
        }
        public bool IsDynamic
        {
            get { return m_IsDynamic.IsNull ? false : m_IsDynamic.Value; }
            set 
            {
                if (m_IsDynamic.IsNull || m_IsDynamic.Value != value)
                {
                    m_IsDynamic = value;
                    m_dirty = true;
                }
            }
        }
        public string DynamicSelection
        {
            get { return (m_DynamicSelection.IsNull ? string.Empty : m_DynamicSelection.Value); }
            set
            {
                if (m_DynamicSelection.IsNull || m_DynamicSelection.Value != value)
                {
                    m_DynamicSelection = value;
                    m_dirty = true;
                }
            }
        }
        /// <summary>
        /// The combined policy and assessment name
        /// </summary>
        [XmlIgnoreAttribute]
        public string PolicyAssessmentName
        {
            get { return string.Format("{0}{1}{2}", m_PolicyName.Value, IsAssessment ? " - " : string.Empty, m_AssessmentName.Value); }
        }
        [XmlIgnoreAttribute]
        public string AssessmentName
        {
            get { return m_AssessmentName.Value; }
            set
            {
                if (m_AssessmentName.IsNull || value != m_AssessmentName.Value)
                {
                    m_dirty = true;
                    m_AssessmentName = value;
                }
            }
        }
        [XmlIgnoreAttribute]
        public string AssessmentDescription
        {
            get { return m_AssessmentDescription.Value; }
            set
            {
                if (m_AssessmentDescription.IsNull || value != m_AssessmentDescription.Value)
                {
                    m_dirty = true;
                    m_AssessmentDescription = value;
                }
            }
        }
        [XmlIgnoreAttribute]
        public string AssessmentNotes
        {
            get { return m_AssessmentNotes.Value; }
            set
            {
                if (m_AssessmentNotes.IsNull || value != m_AssessmentNotes.Value)
                {
                    m_dirty = true;
                    m_AssessmentNotes = value;
                }
            }
        }
        [XmlIgnoreAttribute]
        public DateTime? AssessmentDate
        {
            get { return m_AssessmentDate.IsNull ? null : (DateTime?)m_AssessmentDate.Value; }
            set
            {
                if (m_AssessmentDate.IsNull || value != m_AssessmentDate.Value)
                {
                    m_dirty = true;
                    m_AssessmentDate = value.HasValue ? value.Value : SqlDateTime.Null;
                }
            }
        }
        public bool UseBaseline
        {
            get { return m_UseBaseline.IsNull ? false : m_UseBaseline.Value; }
            set
            {
                if (m_UseBaseline.IsNull || m_UseBaseline.Value != value)
                {
                    m_UseBaseline = value;
                    m_dirty = true;
                }
            }
        }
        [XmlIgnoreAttribute]
        public string AssessmentState
        {
            get { return m_AssessmentState.Value; }
            set { m_AssessmentState = value; }
        }
        [XmlIgnoreAttribute]
        public string AssessmentStateName
        {
            get
            {
                return Utility.Policy.AssessmentState.StateName(AssessmentState);
            }
        }
        [XmlIgnoreAttribute]
        public bool IsPolicy
        {
            get
            {
                return m_AssessmentState.Value.Equals(string.Empty) ||
                       m_AssessmentState.Value.Equals(Utility.Policy.AssessmentState.Settings) ||
                       m_AssessmentState.Value.Equals(Utility.Policy.AssessmentState.Current);
            }
        }
        [XmlIgnoreAttribute]
        public bool IsAssessment
        {
            get
            {
                return !IsPolicy;
            }
        }
        [XmlIgnoreAttribute]
        public bool IsPolicySettings
        {
            get { return m_AssessmentState.Value.Equals(Utility.Policy.AssessmentState.Settings); }
        }
        [XmlIgnoreAttribute]
        public bool IsCurrentAssessment
        {
            get { return m_AssessmentState.Value.Equals(Utility.Policy.AssessmentState.Current); }
        }
        [XmlIgnoreAttribute]
        public bool IsDraftAssessment
        {
            get { return m_AssessmentState.Value.Equals(Utility.Policy.AssessmentState.Draft); }
        }
        [XmlIgnoreAttribute]
        public bool IsPublishedAssessment
        {
            get { return m_AssessmentState.Value.Equals(Utility.Policy.AssessmentState.Published); }
        }
        [XmlIgnoreAttribute]
        public bool IsApprovedAssessment
        {
            get { return m_AssessmentState.Value.Equals(Utility.Policy.AssessmentState.Approved); }
        }

        [XmlIgnoreAttribute]
        public bool InterviewIsTemplate
        {
            get { return m_InterviewIsTemplate.IsNull ? false : m_InterviewIsTemplate.Value; }
        }
        public string InterviewName
        {
            get { return (m_InterviewName.IsNull ? string.Empty : m_InterviewName.Value); }
            set
            {
                if (m_InterviewName.IsNull  || m_InterviewName.Value != value)
                {
                    m_InterviewName = value;
                    m_dirty = true;
                }
            }
        }
        public string InterviewText
        {
            get
            {
                // return new text if it exists
                return (m_InterviewTextNew ?? (m_InterviewText.IsNull ? string.Empty : m_InterviewText.Value));
            }
            set
            {
                // don't allow null value because that is the not passed value
                m_InterviewTextNew = (value == null ? string.Empty : value.TrimEnd());
                if (m_InterviewTextNew != (m_InterviewText.IsNull ? string.Empty : m_InterviewText))
                {
                    m_dirty = true;
                }
            }
        }

        [XmlIgnoreAttribute]
        public int MetricCountHigh
        {
            get { return m_MetricCountHigh.Value; }
        }
        [XmlIgnoreAttribute]
        public int MetricCountMedium
        {
            get { return m_MetricCountMedium.Value; }
        }
        [XmlIgnoreAttribute]
        public int MetricCountLow
        {
            get { return m_MetricCountLow.Value; }
        }
        [XmlIgnoreAttribute]
        public int MetricCount
        {
            get { return MetricCountHigh + MetricCountMedium + MetricCountLow; }
        }
        [XmlIgnoreAttribute]
        public int FindingCountHigh
        {
            get { return m_FindingCountHigh.Value; }
        }
        [XmlIgnoreAttribute]
        public int FindingCountMedium
        {
            get { return m_FindingCountMedium.Value; }
        }
        [XmlIgnoreAttribute]
        public int FindingCountLow
        {
            get { return m_FindingCountLow.Value; }
        }
        [XmlIgnoreAttribute]
        public int FindingCountHighExplained
        {
            get { return m_FindingCountHighExplained.Value; }
        }
        [XmlIgnoreAttribute]
        public int FindingCountMediumExplained
        {
            get { return m_FindingCountMediumExplained.Value; }
        }
        [XmlIgnoreAttribute]
        public int FindingCountLowExplained
        {
            get { return m_FindingCountLowExplained.Value; }
        }
        [XmlIgnoreAttribute]
        public int FindingIconIndex
        {
            get
            {
                int idx = -1;
                if ((IsCurrentAssessment || IsAssessment) && MetricCount > 0)
                {
                    if (FindingCountHigh > 0)
                    {
                        idx = AppIcons.AppImageIndex16(AppIcons.Enum.High_Risk);
                    }
                    else if (FindingCountMedium > 0)
                    {
                        idx = AppIcons.AppImageIndex16(AppIcons.Enum.Medium_Risk);
                    }
                    else if (FindingCountLow > 0)
                    {
                        idx = AppIcons.AppImageIndex16(AppIcons.Enum.Low_Risk);
                    }
                    else if (FindingCountHighExplained > 0)
                    {
                        idx = AppIcons.AppImageIndex16(AppIcons.Enum.High_Risk_Explained);
                    }
                    else if (FindingCountMediumExplained > 0)
                    {
                        idx = AppIcons.AppImageIndex16(AppIcons.Enum.Medium_Risk_Explained);
                    }
                    else if (FindingCountLowExplained > 0)
                    {
                        idx = AppIcons.AppImageIndex16(AppIcons.Enum.Low_Risk_Explained);
                    }
                    else
                    {
                        idx = AppIcons.AppImageIndex16(AppIcons.Enum.No_Risk);
                    }
                }
                else
                {
                    idx = AppIcons.AppImageIndex16(AppIcons.Enum.Unknown);
                }

                return idx;
            }
        }

        public List<PolicyMetric> PolicyMetrics
        {
            get { return m_PolicyMetrics; }
            set { m_PolicyMetrics = value; m_PolicyMetrics.Sort(metricComparer);}
        }

        [XmlIgnoreAttribute]
        public Repository.AssessmentList Assessments
        {
            get
            {
                if (m_Assessments == null)
                {
                    if (IsPolicy)
                    {
                        LoadAssessments();
                    }
                    else
                    {
                        m_Assessments = new Repository.AssessmentList();
                    }
                }

                return m_Assessments;
            }
        }

        [XmlIgnoreAttribute]
        public Repository.AssessmentList CurrentAssessments
        {
            get { return Assessments.FindByState(Utility.Policy.AssessmentState.Current); }
        }
        [XmlIgnoreAttribute]
        public Repository.AssessmentList DraftAssessments
        {
            get { return Assessments.FindByState(Utility.Policy.AssessmentState.Draft); }
        }
        [XmlIgnoreAttribute]
        public Repository.AssessmentList PublishedAssessments
        {
            get { return Assessments.FindByState(Utility.Policy.AssessmentState.Published); }
        }
        [XmlIgnoreAttribute]
        public Repository.AssessmentList ApprovedAssessments
        {
            get { return Assessments.FindByState(Utility.Policy.AssessmentState.Approved); }
        }

        [XmlIgnoreAttribute]
        public Dictionary<int, int> PolicySnapshotList
        {
            get
            {
                // note this must be created manually for a policy because there is no associated assessmentdate with them
                if (IsAssessment && m_SnapshotList == null)
                {
                    m_SnapshotList = GetPolicySnapshotIds();
                }

                return m_SnapshotList;
            }
        }

        [XmlIgnoreAttribute]
        public List<RegisteredServer> Members
        {
            get
            {
                if (m_MemberList == null)
                {
                    m_MemberList = GetMemberServers();
                }

                return m_MemberList;
            }
        }

        #endregion

        #region Queries

        // Get Findings for a given Server
        private const string QueryGetServerFindings =
            @"SELECT 
              (select count(distinct metricid) 
                from SQLsecure.dbo.vwpolicyassessment 
                where policyid = @policyid 
                    and assessmentid = @assessmentid 
                    and registeredserverid = @serverid 
                    and metricseveritycode = 3 
                    and severitycode > 0) as 'HighFindingCount',
              (select count(distinct metricid) 
                from SQLsecure.dbo.vwpolicyassessment 
                where policyid = @policyid 
                    and assessmentid = @assessmentid 
                    and registeredserverid = @serverid 
                    and metricseveritycode = 2 
                    and severitycode > 0) as 'MediumFindingCount',
              (select count(distinct metricid) 
                from SQLsecure.dbo.vwpolicyassessment 
                where policyid = @policyid 
                    and assessmentid = @assessmentid 
                    and registeredserverid = @serverid 
                    and metricseveritycode = 1 
                    and severitycode > 0) as 'LowFindingCount'";

        // Get registered servers.
        private const string QueryGetPolicies =
            @"SELECT 
                        policyid,
                        assessmentid,
                        policyname,
                        policydescription,
                        issystempolicy,
                        isdynamic, 
                        dynamicselection,
                        assessmentname,
                        assessmentdescription,
                        assessmentnotes,
                        assessmentdate,
                        usebaseline,
                        assessmentstate,
                        interviewname,
                        interviewtext,
                        metriccounthigh,
                        metriccountmedium,
	                    metriccountlow,
	                    findingcounthigh,
	                    findingcountmedium,
	                    findingcountlow,
                        findingcounthighexplained,
                        findingcountmediumexplained,
                        findingcountlowexplained
                      FROM SQLsecure.dbo.vwpolicy";
        private static string QueryGetAllPolicies = QueryGetPolicies +
                    @" WHERE assessmentstate = N'" + Utility.Policy.AssessmentState.Settings + @"'
                        ORDER BY issystempolicy desc, policyname";
        private static string QueryGetPolicy = QueryGetPolicies +
                    @" WHERE policyid = @policyid
                            AND assessmentstate = N'" + Utility.Policy.AssessmentState.Settings + @"'";
        private static string QueryGetAssessment = QueryGetPolicies +
                    @" WHERE policyid = @policyid
                            AND assessmentid = @assessmentid";
        private static string QueryGetAssessments = QueryGetPolicies +
                    @" WHERE policyid = @policyid
                            AND assessmentstate NOT IN (N'" + Utility.Policy.AssessmentState.Settings + @"')
                        ORDER BY assessmentid desc";

        private const string ParamId = "policyid";
        private const string ParamIdAssessment = "assessmentid";
        private const string ParamServerId = "serverid";

        private const string NonQueryUpdatePolicy = @"SQLsecure.dbo.isp_sqlsecure_updatepolicy";

        // This is the column index to use when obtaining fields from the policy query
        private enum PolicyColumn
        {
            PolicyId = 0,
            AssessmentId,
            PolicyName,
            PolicyDescription,
            IsSystemPolicy,
            IsDynamic,
            DynamicSelection,
            AssessmentName,
            AssessmentDescription,
            AssessmentNotes,
            AssessmentDate,
            UseBaseline,
            AssessmentState,
            InterviewName,
            InterviewText,
            MetricCountHigh,
            MetricCountMedium,
            MetricCountLow,
            FindingCountHigh,
            FindingCountMedium,
            FindingCountLow,
            FindingCountHighExplained,
            FindingCountMediumExplained,
            FindingCountLowExplained
        }

        // Add policy.
        private const string NonQueryAddPolicy = @"SQLsecure.dbo.isp_sqlsecure_addpolicy";
        private const string ParamPolicyId = "@policyid";
        private const string ParamAssessmentId = "@assessmentid";
        private const string ParamPolicyName = "@policyname";
        private const string ParamPolicyDescription = "@policydescription";
        private const string ParamIsSystem = "@issystempolicy";
        private const string ParamIsDynamic = "@isdynamic";
        private const string ParamDynamicSelection = "@dynamicselection";
        private const string ParamAssessmentName = "@assessmentname";
        private const string ParamAssessmentDescription = "@assessmentdescription";
        private const string ParamAssessmentNotes = "@assessmentnotes";
        private const string ParamAssessmentDate = "@assessmentdate";
        private const string ParamUseBaseline = "@usebaseline";
        private const string ParamAssessmentState = "@assessmentstate";
        private const string ParamInterviewName = "@interviewname";
        private const string ParamInterviewText = "@interviewtext";

        // Create Assessment.
        private const string NonQueryCreateAssessment = @"SQLsecure.dbo.isp_sqlsecure_createassessmentfrompolicy";
        private const string ParamNewAssessmentId = "@newassessmentid";

        // Assessment Data is valid
        private const string NonQueryAssessementDataValid = @"SQLsecure.dbo.isp_sqlsecure_isassessmentdatacurrent";
        private const string ParamValid = "@valid";

        //  Policy Snapshots
        private const string ParamName = "@name";
        private const string ParamDescription = "@description";
        private const string ParamType = "@type";
        private const string ParamCopy = "@copy";

        // Delete Policy.
        private const string NonQueryRemovePolicy = @"SQLsecure.dbo.isp_sqlsecure_removepolicy";

        //  Policy Members
        private const string QueryPolicyMembers = @"SQLsecure.dbo.isp_sqlsecure_getpolicymemberlist";

        //  Policy Snapshots
        private const string ParamRunDate = "@rundate";

        private const string ParamRegisteredServerId = "@serverId";

        private const string QueryPolicySnapshots = @"SQLsecure.dbo.isp_sqlsecure_getpolicysnapshotlist";

        #endregion

        #region Helpers

        private void setValues(SqlDataReader rdr)
        {
            m_PolicyId = rdr.GetSqlInt32((int)PolicyColumn.PolicyId);
            m_AssessmentId = rdr.GetSqlInt32((int)PolicyColumn.AssessmentId);
            m_PolicyName = rdr.GetSqlString((int)PolicyColumn.PolicyName);
            m_PolicyDescription = rdr.GetSqlString((int)PolicyColumn.PolicyDescription);
            m_IsSystemPolicy = rdr.GetSqlBoolean((int)PolicyColumn.IsSystemPolicy);
            m_IsDynamic = rdr.GetSqlBoolean((int)PolicyColumn.IsDynamic);
            m_DynamicSelection = rdr.GetSqlString((int)PolicyColumn.DynamicSelection);
            m_AssessmentName = rdr.GetSqlString((int)PolicyColumn.AssessmentName);
            m_AssessmentDescription = rdr.GetSqlString((int)PolicyColumn.AssessmentDescription);
            m_AssessmentNotes = rdr.GetSqlString((int)PolicyColumn.AssessmentNotes);
            m_AssessmentDate = rdr.GetSqlDateTime((int)PolicyColumn.AssessmentDate).IsNull ? rdr.GetSqlDateTime((int)PolicyColumn.AssessmentDate) : rdr.GetSqlDateTime((int)PolicyColumn.AssessmentDate).Value;
            m_UseBaseline = rdr.GetSqlBoolean((int)PolicyColumn.UseBaseline);
            m_AssessmentState = rdr.GetSqlString((int)PolicyColumn.AssessmentState);
            m_InterviewName = rdr.GetSqlString((int)PolicyColumn.InterviewName);
            m_InterviewText = rdr.GetSqlString((int)PolicyColumn.InterviewText);
            m_MetricCountHigh = rdr.GetSqlInt32((int)PolicyColumn.MetricCountHigh);
            m_MetricCountMedium = rdr.GetSqlInt32((int)PolicyColumn.MetricCountMedium);
            m_MetricCountLow = rdr.GetSqlInt32((int)PolicyColumn.MetricCountLow);
            m_FindingCountHigh = rdr.GetSqlInt32((int)PolicyColumn.FindingCountHigh);
            m_FindingCountMedium = rdr.GetSqlInt32((int)PolicyColumn.FindingCountMedium);
            m_FindingCountLow = rdr.GetSqlInt32((int)PolicyColumn.FindingCountLow);
            m_FindingCountHighExplained = rdr.GetSqlInt32((int)PolicyColumn.FindingCountHighExplained);
            m_FindingCountMediumExplained = rdr.GetSqlInt32((int)PolicyColumn.FindingCountMediumExplained);
            m_FindingCountLowExplained = rdr.GetSqlInt32((int)PolicyColumn.FindingCountLowExplained);
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return (string)m_PolicyName;
        }

        public void SetPolicyName(string name)
        {
            if (m_PolicyName.IsNull || name != m_PolicyName.Value)
            {
                m_dirty = true;
                m_PolicyName = name;
            }
        }

        public void RefreshPolicy()
        {
            try
            {
                // Open connection to repository and get policy properties.
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Check if the instance is registered.
                    SqlParameter param = new SqlParameter(ParamId, m_PolicyId);
                    SqlParameter param2 = new SqlParameter(ParamIdAssessment, m_AssessmentId);
                    using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                    m_AssessmentId.IsNull ? QueryGetPolicy : QueryGetAssessment,
                                                    m_AssessmentId.IsNull ? new SqlParameter[] { param } : new SqlParameter[] { param, param2 }))
                    {
                        if (rdr.HasRows && rdr.Read())
                        {
                            setValues(rdr);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error("Error - Unable to refresh Policy from the Repository", ex);
                MsgBox.ShowError(Utility.ErrorMsgs.CantGetRegisteredServer, ex.Message);
            }
        }

        public static Repository.PolicyList RefreshPolicies()
        {
            Repository.PolicyList policylist = LoadPolicies(Program.gController.Repository.ConnectionString);

            if (policylist.Count > 0 && Program.gController.Repository.RegisteredServers.Count > 0)
            {
                foreach (Policy newpolicy in policylist)
                {
                    Policy oldpolicy = Program.gController.Repository.Policies.Find(newpolicy.PolicyId);
                    if (oldpolicy != null)
                    {
                        oldpolicy = newpolicy;
                    }
                }
            }

            return policylist;
        }

        public static Repository.PolicyList LoadPolicies(string connectionString)
        {
            Repository.PolicyList policylist = new Repository.PolicyList();

            try
            {
                if (!string.IsNullOrEmpty(connectionString))
                {
                    // Retrieve policy information.
                    logX.loggerX.Info("Retrieve Policies");

                    using (SqlConnection connection = new SqlConnection(connectionString)
                        )
                    {
                        // Open the connection.
                        connection.Open();

                        using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                                               QueryGetAllPolicies, null))
                        {
                            while (rdr.Read())
                            {
                                Policy policy = new Policy(rdr);
                                policy.LoadAssessments(connectionString);
                                policylist.Add(policy);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error(string.Format(Utility.ErrorMsgs.ErrorStub, Utility.ErrorMsgs.CantGetPolicies), ex);
                MsgBox.ShowError(Utility.ErrorMsgs.CantGetPolicies, ex.Message);
            }
            catch (Exception ex)
            {
                logX.loggerX.Error(string.Format(Utility.ErrorMsgs.ErrorStub, Utility.ErrorMsgs.CantGetPolicies), ex);
                MsgBox.ShowError(Utility.ErrorMsgs.CantGetPolicies, ex.Message);
            }

            return policylist;
        }

        public void LoadAssessments()
        {
            LoadAssessments(Program.gController.Repository.ConnectionString);
        }

        public void LoadAssessments(string connectionString)
        {
            m_Assessments = LoadAssessments(connectionString, PolicyId);
        }

        public static Repository.AssessmentList LoadAssessments(int policyId)
        {
            return LoadAssessments(Program.gController.Repository.ConnectionString, policyId);
        }

        public static Repository.AssessmentList LoadAssessments(string connectionString, int policyId)
        {
            Repository.AssessmentList assessmentlist = new Repository.AssessmentList();

            try
            {
                if (!string.IsNullOrEmpty(connectionString))
                {
                    // Retrieve policy information.
                    logX.loggerX.Verbose("Retrieve Policy Assessments");

                    using (SqlConnection connection = new SqlConnection(connectionString)
                        )
                    {
                        // Open the connection.
                        connection.Open();

                        SqlParameter param = new SqlParameter(ParamId, policyId);
                        using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                                               QueryGetAssessments, new SqlParameter[] { param }))
                        {
                            while (rdr.Read())
                            {
                                Policy policy = new Policy(rdr);

                                assessmentlist.Add(policy);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error(string.Format(Utility.ErrorMsgs.ErrorStub, Utility.ErrorMsgs.CantGetAssessments), ex);
                MsgBox.ShowError(Utility.ErrorMsgs.CantGetAssessments, ex.Message);
            }
            catch (Exception ex)
            {
                logX.loggerX.Error(string.Format(Utility.ErrorMsgs.ErrorStub, Utility.ErrorMsgs.CantGetAssessments), ex);
                MsgBox.ShowError(Utility.ErrorMsgs.CantGetAssessments, ex.Message);
            }

            return assessmentlist;
        }

        /// <summary>
        /// Check the stored assessment data for the policy and make sure the criteria to produce it hasn't changed
        /// </summary>
        /// <returns>true if the data is current and valid or false if the data needs to be created</returns>
        public bool IsAssessmentDataValid()
        {
            return IsAssessmentDataValid(Program.gController.Repository.ConnectionString, m_PolicyId.Value, m_AssessmentId.Value);
        }

        /// <summary>
        /// Check the stored assessment data for the selected policy and make sure the criteria to produce it hasn't changed
        /// </summary>
        /// <param name="policyId">The policyid of the assessment to validate</param>
        /// <param name="assessmentId">The assessmentid of the assessment to validate</param>
        /// <returns>true if the data is current and valid or false if the data needs to be created</returns>
        static public bool IsAssessmentDataValid(int policyId, int assessmentId)
        {
            return IsAssessmentDataValid(Program.gController.Repository.ConnectionString, policyId, assessmentId);
        }

        /// <summary>
        /// Check the stored assessment data for the selected policy and make sure the criteria to produce it hasn't changed
        /// </summary>
        /// <param name="connectionString">The connection string to use when connecting to the repository</param>
        /// <param name="policyId">The policyid of the assessment to validate</param>
        /// <param name="assessmentId">The assessmentid of the assessment to validate</param>
        /// <returns>true if the data is current and valid or false if the data needs to be created</returns>
        static public bool IsAssessmentDataValid(string connectionString, int policyId, int assessmentId)
        {
            // validate assessment data is current.
            logX.loggerX.Info("Validate assessment data is current");

            try
            {
                if (!string.IsNullOrEmpty(connectionString))
                {
                    using (SqlConnection connection = new SqlConnection(connectionString)
                        )
                    {
                        // Open the connection.
                        connection.Open();

                        SqlParameter param = new SqlParameter(ParamId, policyId);
                        SqlParameter param2 = new SqlParameter(ParamAssessmentId, assessmentId);
                        SqlParameter paramOut = new SqlParameter(ParamValid, DbType.Boolean);
                        paramOut.Direction = ParameterDirection.Output;
                        using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                                               NonQueryAssessementDataValid, new SqlParameter[] { param, param2, paramOut }))
                        {
                            return (bool)paramOut.Value;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error(string.Format(Utility.ErrorMsgs.ErrorStub, Utility.ErrorMsgs.CantValidateAssessmentData), ex);
                MsgBox.ShowError(Utility.ErrorMsgs.CantValidateAssessmentData, ex.Message);
            }
            catch (Exception ex)
            {
                logX.loggerX.Error(string.Format(Utility.ErrorMsgs.ErrorStub, Utility.ErrorMsgs.CantValidateAssessmentData), ex);
                MsgBox.ShowError(Utility.ErrorMsgs.CantValidateAssessmentData, ex.Message);
            }

            return false;
        }

        static public bool IsPolicyRegistered(int policyId)
        {
            return IsPolicyRegistered(Program.gController.Repository.ConnectionString, policyId);
        }

        static public bool IsPolicyRegistered(
                string connectionString,
                int policyId
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(connectionString));

            // Init return.
            bool isRegistered = (GetPolicy(policyId) != null);

            return isRegistered;
        }

        public bool IsAssessmentFound(int assessmentId)
        {
            return IsAssessmentFound(Program.gController.Repository.ConnectionString, PolicyId, assessmentId);
        }

        static public bool IsAssessmentFound(int policyId, int assessmentId)
        {
            return IsAssessmentFound(Program.gController.Repository.ConnectionString, policyId, assessmentId);
        }

        static public bool IsAssessmentFound(
                string connectionString,
                int policyId,
                int assessmentId
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(connectionString));

            bool isfound = false;

            // Init return.
            Policy policy = GetPolicy(policyId);
            if (policy != null)
            {
                isfound = policy.HasAssessment(assessmentId);
            }

            return isfound;
        }

        public bool HasAssessment(int assessmentId)
        {
            bool isfound = false;

            foreach (Policy assessment in Assessments)
            {
                if (assessment.AssessmentId == assessmentId)
                {
                    isfound = true;
                    break;
                }
            }

            return isfound;
        }

        public bool HasAssessment(string assessmentName)
        {
            bool isfound = false;

            foreach (Policy assessment in Assessments)
            {
                if (string.Compare(assessment.AssessmentName, assessmentName, true) == 0)
                {
                    isfound = true;
                    break;
                }
            }

            return isfound;
        }

        public bool HasAssessments()
        {
            bool isfound = false;

            foreach (Policy assessment in Assessments)
            {
                if (assessment.AssessmentState != Utility.Policy.AssessmentState.Current)
                {
                    isfound = true;
                    break;
                }
            }

            return isfound;
        }

        public Policy GetAssessment(string assessmentName)
        {
            Policy assessment = null;

            foreach (Policy a in Assessments)
            {
                if (string.Compare(a.AssessmentName, assessmentName, true) == 0)
                {
                    assessment = a;
                    break;
                }
            }

            return assessment;
        }

        /// <summary>
        /// Add a new user policy
        /// </summary>
        /// <param name="policyName"></param>
        /// <param name="policyDescription"></param>
        /// <param name="isDynamic"></param>
        /// <param name="dynamicText"></param>
        /// <param name="interviewName"></param>
        /// <param name="interviewText"></param>
        public static int AddPolicy(
                string policyName,
                string policyDescription,
                bool isDynamic,
                string dynamicText,
                string interviewName,
                string interviewText
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(policyName));

            return AddPolicy(policyName, policyDescription, false, isDynamic, dynamicText, interviewName, interviewText);
        }

        /// <summary>
        /// Add a new Policy of the specified type
        /// </summary>
        /// <param name="policyName"></param>
        /// <param name="policyDescription"></param>
        /// <param name="isSystem"></param>
        /// <param name="isDynamic"></param>
        /// <param name="dynamicText"></param>
        /// <param name="interviewName"></param>
        /// <param name="interviewText"></param>
        public static int AddPolicy(
                string policyName,
                string policyDescription,
                bool isSystem,
                bool isDynamic,
                string dynamicText,
                string interviewName,
                string interviewText
            )
        {
            int id = -1;
            Debug.Assert(!string.IsNullOrEmpty(policyName));
            try
            {
                // Open connection to repository and add server.
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Setup policy params.
                    SqlParameter paramPolicyName = new SqlParameter(ParamPolicyName, policyName);
                    SqlParameter paramPolicyDescription = new SqlParameter(ParamPolicyDescription, policyDescription);
                    SqlParameter paramIsSystem = new SqlParameter(ParamIsSystem, isSystem);
                    SqlParameter paramIsDynamic = new SqlParameter(ParamIsDynamic, isDynamic);
                    SqlParameter paramDynamicSelection = new SqlParameter(ParamDynamicSelection, dynamicText ?? String.Empty);
                    SqlParameter paramInterviewName = new SqlParameter(ParamInterviewName, interviewName);
                    SqlParameter paramInterviewText = new SqlParameter(ParamInterviewText, interviewText);
                    SqlParameter paramOutId = new SqlParameter(ParamId, DbType.Int32);
                    paramOutId.Direction = ParameterDirection.Output;

                    Sql.SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure,
                                                  NonQueryAddPolicy, new SqlParameter[]
                                                                         {
                                                                             paramPolicyName,
                                                                             paramPolicyDescription,
                                                                             paramIsSystem,
                                                                             paramIsDynamic, 
                                                                             paramDynamicSelection,
                                                                             paramInterviewName,
                                                                             paramInterviewText,
                                                                             paramOutId
                                                                         });
                    id = (int) paramOutId.Value;
                }
            }
            catch(Exception ex)
            {
                string title = "Error Creating Policy";
                string msg =
                    string.Format("Failed to add policy {0} error message: {1}", policyName,
                                  ex.Message);
                logX.loggerX.Error(msg);
                MsgBox.ShowError(title, msg);
            }

            return id;
        }

        public static void RemovePolicy(int id)
        {
            GetPolicy(id);

            // Open connection to repository and remove policy.
            using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
            {
                // Open the connection.
                connection.Open();

                // Setup policy params.
                SqlParameter param = new SqlParameter(ParamId, id);
                SqlParameter param2 = new SqlParameter(ParamIdAssessment, SqlInt32.Null);
                Sql.SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure,
                                NonQueryRemovePolicy, new SqlParameter[] { param, param2 });

            }
        }

        public static void RemoveAssessment(int id, int assessmentid)
        {
            GetPolicy(id, assessmentid);

            // Open connection to repository and remove policy.
            using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
            {
                // Open the connection.
                connection.Open();

                // Setup policy params.
                SqlParameter param = new SqlParameter(ParamId, id);
                SqlParameter param2 = new SqlParameter(ParamIdAssessment, assessmentid);
                Sql.SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure,
                                NonQueryRemovePolicy, new SqlParameter[] { param, param2 });

            }
        }

        /// <summary>
        /// Create an assessment from an existing policy or assessment using a default name, keeping the description and returning the new assessmentid
        /// </summary>
        /// <param name="id">Policy Id of the policy or assessment to copy</param>
        /// <param name="assessmentId">Assessment Id of the policy or assessment to copy</param>
        /// <returns></returns>
        public static int CreateAssessment(int id, int assessmentId)
        {
            Policy policy = Program.gController.Repository.GetPolicy(id);
            if (policy != null)
            {
                return CreateAssessment(id, assessmentId, "Assessment created " + DateTime.Now, policy.AssessmentDescription);
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Create an assessment from an existing policy or assessment and return the new assessmentid
        /// </summary>
        /// <param name="id">Policy Id of the policy or assessment to copy</param>
        /// <param name="assessmentId">Assessment Id of the policy or assessment to copy</param>
        /// <param name="assessmentName">The name of the assessment specified by the user</param>
        /// <param name="assessmentDescription">A description of the assessment specified by the user</param>
        /// <returns></returns>
        public static int CreateAssessment(int id, int assessmentId, string assessmentName, string assessmentDescription)
        {
            Policy policy = Program.gController.Repository.GetPolicy(id);
            if (policy != null)
            {
                return CreateAssessment(id, assessmentId, assessmentName, assessmentDescription, null, false);
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Create an assessment from an existing policy or assessment and return the new assessmentid
        /// </summary>
        /// <param name="id">Policy Id of the policy or assessment to copy</param>
        /// <param name="assessmentId">Assessment Id of the policy or assessment to copy</param>
        /// <param name="assessmentName">The name of the assessment specified by the user</param>
        /// <param name="assessmentDescription">A description of the assessment specified by the user</param>
        /// <param name="assessmentDate">The rundate used for selecting snapshots for the assessment</param>
        /// <param name="useBaseline">Use only baseline snapshots when performing an assessment</param>
        /// <returns></returns>
        public static int CreateAssessment(int id, int assessmentId, string assessmentName, string assessmentDescription, DateTime? assessmentDate, bool useBaseline)
        {
            int newid = -1;
            Policy policy = GetPolicy(id, assessmentId);
            if (policy != null)
            {
                try
                {
                    // Open connection to repository and remove policy.
                    using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString)
                        )
                    {
                        // Open the connection.
                        connection.Open();

                        // Setup policy params.
                        SqlParameter param = new SqlParameter(ParamId, id);
                        SqlParameter param2 = new SqlParameter(ParamIdAssessment, assessmentId);
                        SqlParameter param3 = new SqlParameter(ParamName, assessmentName);
                        SqlParameter param4 = new SqlParameter(ParamDescription, assessmentDescription);
                        SqlParameter param5 = new SqlParameter(ParamAssessmentDate, assessmentDate.HasValue ? assessmentDate.Value : assessmentDate);
                        SqlParameter param6 = new SqlParameter(ParamUseBaseline, useBaseline);
                        SqlParameter param7 = new SqlParameter(ParamType, Utility.Policy.AssessmentState.Draft);
                        SqlParameter param8 = new SqlParameter(ParamCopy, Utility.Policy.CopyType.CopyAll);
                        SqlParameter paramOutId = new SqlParameter(ParamNewAssessmentId, DbType.Int32);
                        paramOutId.Direction = ParameterDirection.Output;
                        Sql.SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure,
                                                      NonQueryCreateAssessment,
                                                      new SqlParameter[] { param, param2, param3, param4, param5, param6, param7, param8, paramOutId });

                        newid = (int)paramOutId.Value;
                    }
                }
                catch (Exception ex)
                {
                    string title = "Error Creating Assessment";
                    string msg =
                        string.Format("Failed to copy {0} {1}. Error message: {2}",
                                        policy.IsPolicy ? "policy" : "assessment",
                                        policy.PolicyAssessmentName,
                                        ex.Message);
                    logX.loggerX.Error(msg);
                    MsgBox.ShowError(title, msg);
                }
            }

            return newid;
        }

        public static Policy GetPolicy(int id)
        {
            return GetPolicy(id, null);
        }

        public static Policy GetPolicy(int id, int? assessmentId)
        {
            Policy policy = null;

            try
            {
                // Open connection to repository and get policy properties.
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Get the policy.
                    SqlParameter param = new SqlParameter(ParamId, id);
                    SqlParameter param2 = new SqlParameter(ParamIdAssessment, assessmentId.HasValue ? (SqlInt32)assessmentId.Value : SqlInt32.Null);
                    using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                    !assessmentId.HasValue ? QueryGetPolicy : QueryGetAssessment,
                                                    !assessmentId.HasValue ? new SqlParameter[] { param } : new SqlParameter[] { param, param2 }))
                    {
                        if (rdr.HasRows && rdr.Read())
                        {
                            policy = new Policy(rdr);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error(
                    string.Format(Utility.ErrorMsgs.ErrorStub, Utility.ErrorMsgs.CantGetPolicy), ex);
                MsgBox.ShowError(Utility.ErrorMsgs.PolicyCaption, Utility.ErrorMsgs.CantGetPolicy, ex);
            }

            return policy;
        }

        public void LoadDefaultPolicy()
        {
            // Open connection to repository and get policy properties.
            using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
            {
                // Open the connection.
                connection.Open();

                // Get the policy.
                SqlParameter param = new SqlParameter(ParamId, 0);
                SqlParameter param2 = new SqlParameter(ParamIdAssessment, 0);
                using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                QueryGetAssessment, new SqlParameter[] { param, param2 }))
                {
                    if (rdr.HasRows && rdr.Read())
                    {
                        setValues(rdr);
                    }
                }
            }
        }

        public bool PromoteAssessment()
        {
            bool save = false;

            if (m_AssessmentState == Utility.Policy.AssessmentState.Draft)
            {
                if (MsgBox.ShowConfirmHelp(ErrorMsgs.PublishAssessmentCaption, 
                                            string.Format(ErrorMsgs.PublishAssessmentMsg, m_AssessmentName),
                                            Utility.Help.AssessmentSummaryPublishedServerHelpTopic
                                            ) == DialogResult.Yes)
                {
                    m_AssessmentState = Utility.Policy.AssessmentState.Published;
                    m_dirty = true;
                    save = true;
                }
            }
            else if (m_AssessmentState == Utility.Policy.AssessmentState.Published)
            {
                if (MsgBox.ShowConfirmHelp(ErrorMsgs.ApproveAssessmentCaption, 
                                            string.Format(ErrorMsgs.ApproveAssessmentMsg, m_AssessmentName),
                                            Utility.Help.AssessmentSummaryApprovedServerHelpTopic
                                            ) == DialogResult.Yes)
                {
                    m_AssessmentState = Utility.Policy.AssessmentState.Approved;
                    m_dirty = true;
                    save = true;
                }
            }

            if (save)
            {
                save = SavePolicyToRepository(Program.gController.Repository.ConnectionString, false);
            }

            return save;
        }

        public bool SavePolicyToRepository(string connectionString)
        {
            return SavePolicyToRepository(connectionString, true);
        }

        public bool SavePolicyToRepository(string connectionString, bool saveInterview)
        {
            bool saved = false;

            if (m_dirty)
            {
                try
                {
                    if (!string.IsNullOrEmpty(connectionString))
                    {
                        // Retrieve server information.
                        logX.loggerX.Info("Update Policy");

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            // Open the connection.
                            connection.Open();
                            SqlParameter paramPolicyId = new SqlParameter(ParamPolicyId, PolicyId);
                            SqlParameter paramAssessmentId = new SqlParameter(ParamAssessmentId, AssessmentId);
                            SqlParameter paramPolicyName = new SqlParameter(ParamPolicyName, PolicyName);
                            SqlParameter paramPolicyDescription = new SqlParameter(ParamPolicyDescription, PolicyDescription);
                            SqlParameter paramAssessmentState = new SqlParameter(ParamAssessmentState, AssessmentState);
                            SqlParameter paramIsDynamic = new SqlParameter(ParamIsDynamic, IsDynamic);
                            SqlParameter paramDynamicSelection = new SqlParameter(ParamDynamicSelection, DynamicSelection);
                            SqlParameter paramAssessmentName = new SqlParameter(ParamAssessmentName, AssessmentName);
                            SqlParameter paramAssessmentDescription = new SqlParameter(ParamAssessmentDescription, AssessmentDescription);
                            SqlParameter paramAssessmentNotes = new SqlParameter(ParamAssessmentNotes, AssessmentNotes);
                            SqlParameter paramAssessmentDate = new SqlParameter(ParamAssessmentDate, AssessmentDate.HasValue ? (SqlDateTime)AssessmentDate.Value : SqlDateTime.Null);
                            SqlParameter paramUseBaseline = new SqlParameter(ParamUseBaseline, UseBaseline);

                            SqlParameter[] parameters;
                            
                            if (saveInterview)
                            {
                                SqlParameter paramInterviewName = new SqlParameter(ParamInterviewName, m_InterviewName);
                                SqlParameter paramInterviewText = new SqlParameter(ParamInterviewText, m_InterviewTextNew);

                                parameters = new SqlParameter[] 
                                                            {
                                                                paramPolicyId,
                                                                paramAssessmentId,
                                                                paramPolicyName,
                                                                paramPolicyDescription,
                                                                paramAssessmentState,
                                                                paramIsDynamic,
                                                                paramDynamicSelection,
                                                                paramAssessmentName,
                                                                paramAssessmentDescription,
                                                                paramAssessmentNotes,
                                                                paramAssessmentDate,
                                                                paramUseBaseline,
                                                                paramInterviewName,
                                                                paramInterviewText
                                                            };
                            }
                            else
                            {
                                parameters = new SqlParameter[]
                                                            {
                                                                paramPolicyId,
                                                                paramAssessmentId,
                                                                paramPolicyName,
                                                                paramPolicyDescription,
                                                                paramAssessmentState,
                                                                paramIsDynamic,
                                                                paramDynamicSelection,
                                                                paramAssessmentName,
                                                                paramAssessmentDescription,
                                                                paramAssessmentNotes,
                                                                paramAssessmentDate,
                                                                paramUseBaseline
                                                            };
                            }

                            Sql.SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure,
                                                          NonQueryUpdatePolicy, parameters);
                        }
                        if (saveInterview)
                        {
                            m_InterviewText = m_InterviewTextNew;
                            m_InterviewTextNew = null;
                        }
                    }

                    saved = true;
                    m_dirty = false;
                }
                catch (SqlException ex)
                {
                    logX.loggerX.Error(
                        string.Format(Utility.ErrorMsgs.ErrorStub, Utility.ErrorMsgs.CantSavePolicy), ex);
                    MsgBox.ShowError(Utility.ErrorMsgs.CantSavePolicy, ex.Message);
                }
                catch (Exception ex)
                {
                    logX.loggerX.Error(
                        string.Format(Utility.ErrorMsgs.ErrorStub, Utility.ErrorMsgs.CantSavePolicy), ex);
                    MsgBox.ShowError(Utility.ErrorMsgs.CantSavePolicy, ex.Message);
                }
            }

            // save policymetrics
            if (m_PolicyMetrics != null)
            {
                foreach (PolicyMetric m in m_PolicyMetrics)
                {
                    if (m.UpdatePolicyMetricsToRepository(Program.gController.Repository.ConnectionString))
                    {
                        saved = true;
                    }
                }
            }

            return saved;
        }

        public void GetPolicyFindingForServer(int registeredServerId, out int highFindings, out int mediumFindings, out int lowFindings)
        {
            highFindings = 0;
            mediumFindings = 0;
            lowFindings = 0;
            try
            {
                // Open connection to repository and get policy properties.
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Check if the instance is registered.
                    SqlParameter param = new SqlParameter(ParamId, m_PolicyId);
                    SqlParameter param2 = new SqlParameter(ParamIdAssessment, m_AssessmentId);
                    SqlParameter param3 = new SqlParameter(ParamServerId, registeredServerId);
                    using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                    QueryGetServerFindings,
                                                    new SqlParameter[] { param, param2, param3 }))
                    {
                        if (rdr.HasRows && rdr.Read())
                        {
                            highFindings = rdr.GetSqlInt32(0).Value;
                            mediumFindings = rdr.GetSqlInt32(1).Value;
                            lowFindings = rdr.GetSqlInt32(2).Value;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error("Error - Unable to get Policy Findings for Server", ex);
                MsgBox.ShowError(Utility.ErrorMsgs.CantGetRegisteredServer, ex.Message);
            }
            
        }

        public List<RegisteredServer> GetMemberServers()
        {
            return GetMemberServers(PolicyId, AssessmentId, AssessmentState);
        }
                
        public static List<RegisteredServer> GetMemberServers(int policyId, int? assessmentId,string assessmentState)
        {
            SortedList<string, RegisteredServer> members = new SortedList<string, RegisteredServer>();

            try
            {
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Setup stored procedure
                    SqlCommand cmd = new SqlCommand(QueryPolicyMembers, connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Build parameters
                    SqlParameter paramPolicyId = new SqlParameter(ParamId, policyId);
                    SqlParameter paramAssessmentId = new SqlParameter(ParamIdAssessment, assessmentId.HasValue ? (SqlInt32)assessmentId.Value : SqlInt32.Null);
                    cmd.Parameters.Add(paramPolicyId);
                    cmd.Parameters.Add(paramAssessmentId);

                    // Get data
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        int svrId = (int)row[0];
                        RegisteredServer svr = Program.gController.Repository.RegisteredServers.Find(svrId);
                        if (svr != null)
                        {
                            if (!members.ContainsKey(svr.ConnectionName))
                            {
                                members.Add(svr.ConnectionName, svr);
                            }
                        }
                        else if(assessmentState == "A" || assessmentState == "D" || assessmentState == "P")
                        {
                            svr = new RegisteredServer();
                            svr.LoadUnregisteredServer(svrId);
                            if (!string.IsNullOrEmpty(svr.ConnectionName))
                            {
                                if (!members.ContainsKey(svr.ConnectionName))
                                {
                                    members.Add(svr.ConnectionName, svr);
                                }
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error(string.Format(Utility.ErrorMsgs.ErrorStub, Utility.ErrorMsgs.CantGetRegisteredServers), ex);
                MsgBox.ShowError(Utility.ErrorMsgs.PolicyCaption, Utility.ErrorMsgs.CantGetRegisteredServers, ex);
            }

            return new List<RegisteredServer>(members.Values);
        }

        public Dictionary<int, int> GetPolicySnapshotIds()
        {
            m_SnapshotList = GetPolicySnapshotIds(PolicyId, AssessmentId, AssessmentDate, UseBaseline);

            return m_SnapshotList;
        }

        public Dictionary<int, int> GetPolicySnapshotIds(DateTime? selectionDate, bool useBaseline)
        {
            m_SnapshotList = GetPolicySnapshotIds(PolicyId, AssessmentId, selectionDate, useBaseline);

            return m_SnapshotList;
        }

        public static Dictionary<int, int> GetPolicySnapshotIds(int policyId, int? assessmentId, DateTime? selectionDate, bool useBaseline)
        {
            Dictionary<int, int> snapshots = new Dictionary<int, int>();

            using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
            {
                // Open the connection.
                connection.Open();

                // Setup stored procedure
                SqlCommand cmd = new SqlCommand(QueryPolicySnapshots, connection);
                cmd.CommandType = CommandType.StoredProcedure;

                // Build parameters
                SqlParameter paramPolicyId = new SqlParameter(ParamId, policyId);
                SqlParameter paramAssessmentId = new SqlParameter(ParamIdAssessment, assessmentId.HasValue ? (SqlInt32)assessmentId.Value : SqlInt32.Null);
                SqlParameter paramRunDate = new SqlParameter(ParamRunDate, selectionDate);
                SqlParameter paramUseBaseline = new SqlParameter(ParamUseBaseline, useBaseline);
                cmd.Parameters.Add(paramPolicyId);
                cmd.Parameters.Add(paramAssessmentId);
                cmd.Parameters.Add(paramRunDate);
                cmd.Parameters.Add(paramUseBaseline);

                // Get data
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    int snapshotId = (int)row[0];
                    int registeredServerId = (int)row[1];

                    snapshots.Add(registeredServerId, snapshotId);
                }
            }

            return snapshots;
        }

        public static DataTable GetPolicySnapshots(int policyId, int? assessmentId, DateTime? selectionDate, bool useBaseline)
        {
            //DataTable snapshots = new DataTable();
            //snapshots.Columns.Add(new DataColumn("snapshotid", typeof(int)));
            //snapshots.Columns.Add(new DataColumn("registeredServerId", typeof(int)));
            //snapshots.Columns.Add(new DataColumn("connectionname", typeof(string)));
            //snapshots.Columns.Add(new DataColumn("endtime", typeof(DateTime)));
            //snapshots.Columns.Add(new DataColumn("status", typeof(string)));
            //snapshots.Columns.Add(new DataColumn("baseline", typeof(bool)));

            using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
            {
                // Open the connection.
                connection.Open();

                // Setup stored procedure
                SqlCommand cmd = new SqlCommand(QueryPolicySnapshots, connection);
                cmd.CommandType = CommandType.StoredProcedure;

                // Build parameters
                SqlParameter paramPolicyId = new SqlParameter(ParamId, policyId);
                SqlParameter paramAssessmentId = new SqlParameter(ParamIdAssessment, assessmentId.HasValue ? (SqlInt32)assessmentId.Value : SqlInt32.Null);
                SqlParameter paramRunDate = new SqlParameter(ParamRunDate, selectionDate);
                SqlParameter paramUseBaseline = new SqlParameter(ParamUseBaseline, useBaseline);
                cmd.Parameters.Add(paramPolicyId);
                cmd.Parameters.Add(paramAssessmentId);
                cmd.Parameters.Add(paramRunDate);
                cmd.Parameters.Add(paramUseBaseline);

                // Get data
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                //    DataRow newrow = snapshots.NewRow();

                //    newrow[0] = (int)row[0];
                //    newrow[1] = (int)row[1];
                //    newrow[2] = (string)row[2];
                //    newrow[3] = (DateTime)row[3];
                //    newrow[4] = (string)row[4];
                //    newrow[5] = (bool)row[5];

                //    snapshots.Rows.Add(newrow);
                    row[3] = ((DateTime) row[3]).ToLocalTime();
                }

                return ds.Tables[0];
            }
        }

        public List<PolicyMetric> GetPolicyMetrics()
        {
            return GetPolicyMetrics(Program.gController.Repository.ConnectionString);
        }

        public List<PolicyMetric> GetPolicyMetrics(string connectionString)
        {
            m_PolicyMetrics = PolicyMetric.GetPolicyMetrics(connectionString, (int)m_PolicyId, (int)m_AssessmentId);
            m_PolicyMetrics.Sort(metricComparer);

            return m_PolicyMetrics;
        }

        public List<PolicyMetric> GetExistingPolicyMetrics(string connectionString)
        {
            if (m_PolicyMetrics == null)
            {
                return GetPolicyMetrics(connectionString);
            }

            return m_PolicyMetrics;
        }

        public void SetPolicyMetrics(List<PolicyMetric> metrics)
        {
            m_PolicyMetrics = metrics;

            // Update PolicyId foreach metric
            for(int x = 0; x < m_PolicyMetrics.Count; x++)
            {
                m_PolicyMetrics[x].PolicyId = m_PolicyId.Value;
                m_PolicyMetrics[x].AssessmentId = m_AssessmentId.Value;
            }
        }

        public void ImportPolicyFromXMLFile(string fileName, bool addToRepository)
        {
            if(!string.IsNullOrEmpty(fileName))
            {
                m_PolicyMetrics = GetPolicyMetrics(Program.gController.Repository.ConnectionString);
                UpdatePolicyFromXMLFile(fileName);
                if (addToRepository)
                {
                    m_PolicyId = AddPolicy(PolicyName, PolicyDescription, IsDynamic, DynamicSelection, InterviewName, InterviewText);
                }
            }
        }

        private void UpdatePolicyMetricsFromImport(Policy newPolicy)
        {
            foreach (PolicyMetric p in m_PolicyMetrics)
            {
                int i = newPolicy.PolicyMetrics.BinarySearch(p, metricComparer);

                if (i >= 0)
                {
                    p.IsSelected = newPolicy.PolicyMetrics[i].IsSelected;
                    p.IsEnabled = newPolicy.PolicyMetrics[i].IsEnabled;
                    p.ReportKey = newPolicy.PolicyMetrics[i].ReportKey;
                    p.ReportText = newPolicy.PolicyMetrics[i].ReportText.Replace("\n", "\r\n");
                    p.Severity = newPolicy.PolicyMetrics[i].Severity;
                    p.SeverityValues = newPolicy.PolicyMetrics[i].SeverityValues;   

                    // SQLsecure 3.1 (Anshul Aggarwal) - Add support for Azure SQL Database
                    if(newPolicy.PolicyMetrics[i].AzureDB != null)
                    {
                        p.AzureDB.ReportKey = newPolicy.PolicyMetrics[i].AzureDB.ReportKey;
                        p.AzureDB.ReportText = newPolicy.PolicyMetrics[i].AzureDB.ReportText.Replace("\n", "\r\n");
                        p.AzureDB.Severity = newPolicy.PolicyMetrics[i].AzureDB.Severity;
                        p.AzureDB.SeverityValues = newPolicy.PolicyMetrics[i].AzureDB.SeverityValues;
                    }
                }
                else
                {
                     p.IsEnabled = false;
                }
                if (p.IsEnabled)
                {
                    switch (p.Severity)
                    {
                        case 1:
                            m_MetricCountLow = m_MetricCountLow.Value + 1;
                            break;
                        case 2:
                            m_MetricCountMedium = m_MetricCountMedium.Value + 1;
                            break;
                        case 3:
                            m_MetricCountHigh = m_MetricCountHigh.Value + 1;
                            break;
                    }
                }
            }
        }

        public void UpdatePolicyMetricsFromSelectedSecurityChecks(Policy newPolicy)
        {
            foreach (PolicyMetric p in m_PolicyMetrics)
            {
                int i = newPolicy.PolicyMetrics.BinarySearch(p, metricComparer);

                if (i >= 0)
                {
                    if (newPolicy.PolicyMetrics[i].IsSelected)
                    {
                        p.IsEnabled = newPolicy.PolicyMetrics[i].IsEnabled;
                        p.ReportKey = newPolicy.PolicyMetrics[i].ReportKey;
                        p.ReportText = newPolicy.PolicyMetrics[i].ReportText.Replace("\n", "\r\n");
                        p.Severity = newPolicy.PolicyMetrics[i].Severity;
                        p.SeverityValues = newPolicy.PolicyMetrics[i].SeverityValues;

                        // SQLsecure 3.1 (Anshul Aggarwal) - Add support for Azure SQL Database
                        if (newPolicy.PolicyMetrics[i].AzureDB != null)
                        {
                            p.AzureDB.ReportKey = newPolicy.PolicyMetrics[i].AzureDB.ReportKey;
                            p.AzureDB.ReportText = newPolicy.PolicyMetrics[i].AzureDB.ReportText.Replace("\n", "\r\n");
                            p.AzureDB.Severity = newPolicy.PolicyMetrics[i].AzureDB.Severity;
                            p.AzureDB.SeverityValues = newPolicy.PolicyMetrics[i].AzureDB.SeverityValues;
                        }
                    }
                }
                else
                {
                    p.IsEnabled = false;
                }
                if (p.IsEnabled)
                {
                    switch (p.Severity)
                    {
                        case 1:
                            m_MetricCountLow = m_MetricCountLow.Value + 1;
                            break;
                        case 2:
                            m_MetricCountMedium = m_MetricCountMedium.Value + 1;
                            break;
                        case 3:
                            m_MetricCountHigh = m_MetricCountHigh.Value + 1;
                            break;
                    }
                }
            }
        }

        private Policy ReadPolicyFromXMLFile(string filename)
        {
            Policy policy = null;
            XmlSerializer reader = new XmlSerializer(typeof(Policy));

            using (StreamReader file = new StreamReader(filename))
            {
                policy = (Policy)reader.Deserialize(file);    
            }
            
            return policy;
        }

        public void LoadPolicyFromXMLFile(string filename)
        {
            try
            {
                Policy policy = ReadPolicyFromXMLFile(filename);

                if (policy.IsPolicy)
                {
                    m_PolicyName = policy.m_PolicyName;
                    m_PolicyDescription = policy.PolicyDescription.Replace("\n", "\r\n");
                    m_IsSystemPolicy = policy.IsSystemPolicy;
                    m_IsDynamic = policy.IsDynamic;
                    m_InterviewIsTemplate = policy.InterviewIsTemplate;
                    m_InterviewName = policy.InterviewName;
                    m_InterviewText = policy.InterviewText;
                    m_PolicyMetrics = policy.PolicyMetrics;
                }

            }
            catch(Exception ex)
            {
                logX.loggerX.Error("Failed to Import Policy:  ", ex.Message);
            }
        }

        public void UpdatePolicyFromXMLFile(string filename)
        {
            try
            {
                Policy policy = ReadPolicyFromXMLFile(filename);

                if (IsPolicy)
                {
                    m_PolicyName = policy.m_PolicyName;
                    m_PolicyDescription = policy.PolicyDescription.Replace("\n", "\r\n");
                    m_IsSystemPolicy = policy.IsSystemPolicy;
                    m_IsDynamic = policy.IsDynamic;
                    m_InterviewIsTemplate = policy.InterviewIsTemplate;
                    m_InterviewName = policy.InterviewName;
                    m_InterviewText = policy.InterviewText;
                }
                UpdatePolicyMetricsFromImport(policy);
            }
            catch(Exception ex)
            {
                logX.loggerX.Error("Failed to Import Policy:  ", ex.Message);
            }
        }

        public void UpdatePolicyFromImporting(Policy policy)
        {
            try
            {
                if (IsPolicy)
                {
                    m_PolicyName = policy.m_PolicyName;
                    m_PolicyDescription = policy.PolicyDescription.Replace("\n", "\r\n");
                    m_IsSystemPolicy = policy.IsSystemPolicy;
                    m_IsDynamic = policy.IsDynamic;
                    m_InterviewIsTemplate = policy.InterviewIsTemplate;
                    m_InterviewName = policy.InterviewName;
                    m_InterviewText = policy.InterviewText;
                }
                m_PolicyMetrics = GetPolicyMetrics(Program.gController.Repository.ConnectionString);
                UpdatePolicyMetricsFromSelectedSecurityChecks(policy);
            }
            catch (Exception ex)
            {
                logX.loggerX.Error("Failed to Import Policy:  ", ex.Message);
            }
        }

        public void SaveToXMLFile(string filename)
        {
            try
            {
                if(m_PolicyMetrics == null)
                {
                    GetPolicyMetrics(Program.gController.Repository.ConnectionString);
                }
                XmlSerializer writer = new XmlSerializer(typeof(Policy));
                StreamWriter file = new StreamWriter(filename);
                writer.Serialize(file, this);
                file.Close();
            }
            catch(Exception ex)
            {
                logX.loggerX.Error("Failed to save Policy to XML file: ", ex.Message);
            }
        }

    #endregion
    }
}
