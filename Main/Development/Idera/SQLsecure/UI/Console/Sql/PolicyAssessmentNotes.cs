using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Diagnostics;

using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Utility;


namespace Idera.SQLsecure.UI.Console.Sql
{
    /// <summary>
    /// Encapsulates a SQLsecure PolicyAssessmentNotes record
    /// </summary>
    class PolicyAssessmentNotes
    {
        #region Fields and Enums

        private SqlInt32 m_PolicyId;
        private SqlInt32 m_AssessmentId;
        private SqlInt32 m_MetricId;
        private SqlInt32 m_SnapshotId;
        private SqlBoolean m_IsExplained;
        private SqlString m_Notes;

        private string m_ServerName;
        private int m_SeverityCode;
        private string m_MetricName;

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Sql.PolicyAssessmentNote");
        private bool m_dirty = false;

        //private const string colPolicyId = "policyid";
        //private const string colAssessmentId = "assessmentid";
        //private const string colMetricId = "metricid";
        //private const string colSnapshotId = "snapshotid";
        //private const string colIsExplained = "isexplained";
        //private const string colNotes = "notes";

        private enum NotesColumn
        {
            ConnectionName=0,
            PolicyId,
            AssessmentId,
            MetricId,
            SnapshotId,
            IsExplained,
            Notes,
            SeverityCode,
            MetricName
       }

        #endregion

        #region Queries

        private const string QueryGetNotes =
            @"SQLsecure.dbo.isp_sqlsecure_getpolicyassessmentnotes"; 


        private const string NonQueryUpdatePolicyMetrics = @"SQLsecure.dbo.isp_sqlsecure_updatepolicyassessmentnotes";

        private const string SPParamPolicyId = "@policyid";
        private const string SPParamAssessmentId = "@assessmentid";
        private const string SPParamMetricId = "@metricid";
        private const string SPParamSnapshotId = "@snapshotid";
        private const string SPParamIsExplained = "@isexplained";
        private const string SPParamNotes = "@notes";

        #endregion

        #region Ctors

        public PolicyAssessmentNotes(SqlDataReader rdr)
        {
            setValues(rdr);
        }

        public PolicyAssessmentNotes()
        { }

        #endregion

        #region Properties

        public int PolicyId
        {
            get { return m_PolicyId.IsNull ? 0 : m_PolicyId.Value; }
            set
            {
                if (m_PolicyId.IsNull || value != m_PolicyId.Value)
                {
                    //m_dirty = true;
                    m_PolicyId = value;
                }
            }
        }
        public int AssessmentId
        {
            get { return m_AssessmentId.IsNull ? 0 : m_AssessmentId.Value; }
            set
            {
                if (m_AssessmentId.IsNull || value != m_AssessmentId.Value)
                {
                    m_dirty = true;
                    m_AssessmentId = value;
                }
            }
        }
        public int MetricId
        {
            get { return m_MetricId.IsNull ? 0 : m_MetricId.Value; }
            set
            {
                if (m_MetricId.IsNull || value != m_MetricId.Value)
                {
                    //m_dirty = true;
                    m_MetricId = value;
                }
            }
        }
        public int SnapshotId
        {
            get { return m_SnapshotId.IsNull ? 0 : m_SnapshotId.Value; }
            set
            {
                if (m_SnapshotId.IsNull || value != m_SnapshotId.Value)
                {
                    m_dirty = true;
                    m_SnapshotId = value;
                }
            }
        }
        public bool IsExplained
        {
            get { return m_IsExplained.IsNull ? false : m_IsExplained.Value; }
            set
            {
                if (m_IsExplained.IsNull || value != m_IsExplained.Value)
                {
                    m_dirty = true;
                    m_IsExplained = value;
                }
            }
        }
        public string Notes
        {
            get { return m_Notes.IsNull ? string.Empty : m_Notes.Value; }
            set
            {
                if (m_Notes.IsNull || value != m_Notes.Value)
                {
                    m_dirty = true;
                    m_Notes = value;
                }
            }
        }
        public string ServerName
        {
            get { return m_ServerName; }
        }
        public int SeverityCode
        {
            get { return m_SeverityCode; }
        }
        public string MetricName
        {
            get { return m_MetricName; }
        }

        #endregion

        #region Methods

        public bool UpdatePolicyAssessmentNotesToRepository()
        {
            return UpdatePolicyAssessmentNotesToRepository(Program.gController.Repository.ConnectionString);
        }

        public bool UpdatePolicyAssessmentNotesToRepository(string connectionString)
        {
            bool saved = false;

            if (m_dirty)
            {
                try
                {
                    if (!string.IsNullOrEmpty(connectionString))
                    {
                        // Retrieve server information.
                        logX.loggerX.Info("Update Assessment Explanation Notes");

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            // Open the connection.
                            connection.Open();
                            SqlParameter paramPolicyId = new SqlParameter(SPParamPolicyId, PolicyId);
                            SqlParameter paramAssessmentId = new SqlParameter(SPParamAssessmentId, AssessmentId);
                            SqlParameter paramMetricId = new SqlParameter(SPParamMetricId, MetricId);
                            SqlParameter paramSnapshotId = new SqlParameter(SPParamSnapshotId, SnapshotId);
                            SqlParameter paramIsExplained = new SqlParameter(SPParamIsExplained, IsExplained);
                            SqlParameter paramNotes = new SqlParameter(SPParamNotes, Notes);

                            Sql.SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure,
                                                          NonQueryUpdatePolicyMetrics, new SqlParameter[]
                                                                                           {
                                                                                               paramPolicyId,
                                                                                               paramAssessmentId,
                                                                                               paramMetricId,
                                                                                               paramSnapshotId,
                                                                                               paramIsExplained,
                                                                                               paramNotes
                                                                                           });
                        }

                        saved = true;
                        m_dirty = false;
                    }
                }
                catch (SqlException ex)
                {
                    logX.loggerX.Error(string.Format(Utility.ErrorMsgs.ErrorStub, Utility.ErrorMsgs.CantUpdatePolicy), ex);
                    MsgBox.ShowError(Utility.ErrorMsgs.CantUpdatePolicy, ex.Message);
                }
                catch (Exception ex)
                {
                    logX.loggerX.Error(string.Format(Utility.ErrorMsgs.ErrorStub, Utility.ErrorMsgs.CantUpdatePolicy), ex);
                    MsgBox.ShowError(Utility.ErrorMsgs.CantUpdatePolicy, ex.Message);
                }
            }

            return saved;
        }

        public static List<PolicyAssessmentNotes> GetPolicyAssessmentNotes(int policyId, int assessmentId, int metricId, string serverName)
        {
            List<PolicyAssessmentNotes> serverList = new List<PolicyAssessmentNotes>();

            List<PolicyAssessmentNotes> notesList = GetPolicyAssessmentNotes(Program.gController.Repository.ConnectionString, policyId, assessmentId, metricId);

            foreach(PolicyAssessmentNotes notes in notesList)
            {
                if (notes.ServerName == serverName)
                {
                    serverList.Add(notes);
                    break;
                }
            }

            return serverList;
        }

        public static List<PolicyAssessmentNotes> GetPolicyAssessmentNotes(int policyId, int assessmentId, int metricId)
        {
            return GetPolicyAssessmentNotes(Program.gController.Repository.ConnectionString, policyId, assessmentId, metricId);
        }

        public static List<PolicyAssessmentNotes> GetPolicyAssessmentNotes(string connectionString, int policyId, int assessmentId, int metricId)
        {
            List<PolicyAssessmentNotes> notesList = new List<PolicyAssessmentNotes>();
            try
            {
                if (!string.IsNullOrEmpty(connectionString))
                {
                    // Retrieve server information.
                    logX.loggerX.Info("Retrieve Policies Assessment Notes");

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        // Open the connection.
                        connection.Open();
                        SqlParameter paramPolicyId = new SqlParameter(SPParamPolicyId, policyId);
                        SqlParameter paramAssessmentId = new SqlParameter(SPParamAssessmentId, assessmentId);
                        SqlParameter paramMetricId = new SqlParameter(SPParamMetricId, metricId);
                        using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.StoredProcedure,
                                                                                QueryGetNotes,
                                                                                new SqlParameter[] { paramPolicyId, paramAssessmentId, paramMetricId }))
                        {
                            while (rdr.Read())
                            {
                                PolicyAssessmentNotes policyAssessmentNotes = new PolicyAssessmentNotes(rdr);

                                notesList.Add(policyAssessmentNotes);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error(string.Format(Utility.ErrorMsgs.ErrorStub, Utility.ErrorMsgs.CantGetAssessmentNotes), ex);
                MsgBox.ShowError(Utility.ErrorMsgs.CantGetAssessmentNotes, ex.Message);
            }
            catch (Exception ex)
            {
                logX.loggerX.Error(string.Format(Utility.ErrorMsgs.ErrorStub, Utility.ErrorMsgs.CantGetAssessmentNotes), ex);
                MsgBox.ShowError(Utility.ErrorMsgs.CantGetAssessmentNotes, ex.Message);
            }

            return notesList;
        }

        #endregion

        #region Helpers

        private void setValues(SqlDataReader rdr)
        {
            m_PolicyId = rdr.GetSqlInt32((int)NotesColumn.PolicyId);
            m_AssessmentId = rdr.GetSqlInt32((int)NotesColumn.AssessmentId);
            m_MetricId = rdr.GetSqlInt32((int)NotesColumn.MetricId);
            m_SnapshotId = rdr.GetSqlInt32((int)NotesColumn.SnapshotId);
            m_IsExplained = rdr.GetSqlBoolean((int)NotesColumn.IsExplained);
            m_Notes = rdr.GetSqlString((int)NotesColumn.Notes);

            m_ServerName = rdr.GetSqlString((int)NotesColumn.ConnectionName).Value;
            m_SeverityCode = rdr.GetSqlInt32((int)NotesColumn.SeverityCode).Value;
            m_MetricName = rdr.GetSqlString((int)NotesColumn.MetricName).Value;
        }

        #endregion
    }
}
