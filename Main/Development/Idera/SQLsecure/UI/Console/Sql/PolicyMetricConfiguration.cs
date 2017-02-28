/******************************************************************
 * Name: PolicyMetric.cs
 *
 * Description: Encapsulates a SQLsecure security policy PolicyMetric (now called security check).
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
using System.Xml.Serialization;

using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Utility;

namespace Idera.SQLsecure.UI.Console.Sql
{
    /// <summary>
    /// Encapsulates a SQLsecure security PolicyMetricConfiguration
    /// </summary>
    public class PolicyMetricConfiguration
    {
        #region Fields and Enums

        private SqlInt32 m_PolicyId;
        private SqlInt32 m_AssessmentId;
        private ServerType m_ServerType;
        private SqlInt32 m_MetricId;
        private SqlString m_MetricName;
        private SqlString m_MetricDescription;
        private SqlString m_ValidValues;
        private SqlString m_ValueDescription;
        private SqlString m_ReportKey;
        private SqlString m_ReportText;
        private SqlInt32 m_Severity;
        private SqlString m_SeverityValues;

        [XmlIgnoreAttribute]
        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Sql.PolicyMetricConfiguration");

        #endregion

        #region Ctors

        public PolicyMetricConfiguration()
        { }

        public PolicyMetricConfiguration(SqlDataReader rdr)
        {
            setValues(rdr);
        }

        #endregion

        #region Properties
        
        public int PolicyId
        {
            get { return m_PolicyId.IsNull ? 0 : m_PolicyId.Value; }
        }
        
        public int AssessmentId
        {
            get { return m_AssessmentId.IsNull ? 0 : m_AssessmentId.Value; }
        }

        public ServerType ServerType
        {
            get { return m_ServerType; }
        }

        public int MetricId
        {
            get { return m_MetricId.IsNull ? 0 : m_MetricId.Value; }
        }

        public string MetricName
        {
            get { return m_MetricName.IsNull ? string.Empty : m_MetricName.Value; }
        }

        public string MetricDescription
        {
            get { return m_MetricDescription.IsNull ? string.Empty : m_MetricDescription.Value; }
        }

        public string ReportKey
        {
            get { return m_ReportKey.IsNull ? string.Empty : m_ReportKey.Value; }
            set
            {
                if (m_ReportKey.IsNull || value != m_ReportKey.Value)
                {
                    m_ReportKey = value;
                }
            }
        }

        public string ReportText
        {
            get { return m_ReportText.IsNull ? string.Empty : m_ReportText.Value; }
            set
            {
                if (m_ReportText.IsNull || value != m_ReportText.Value)
                {
                    m_ReportText = value;
                }
            }
        }

        public int Severity
        {
            get { return m_Severity.IsNull ? 0 : m_Severity.Value; }
            set
            {
                if (m_Severity.IsNull || value != m_Severity.Value)
                {
                    m_Severity = value;
                }
            }
        }

        public string SeverityValues
        {
            get { return m_SeverityValues.IsNull ? string.Empty : m_SeverityValues.Value; }
            set
            {
                if (m_SeverityValues.IsNull || value != m_SeverityValues.Value)
                {
                    m_SeverityValues = value;
                }
            }
        }

        public string ValidValues
        {
            get { return m_ValidValues.IsNull ? string.Empty : m_ValidValues.Value; }
        }

        public string ValueDescription
        {
            get { return m_ValueDescription.IsNull ? string.Empty : m_ValueDescription.Value; }
        }
        #endregion

        #region Methods

        public static PolicyMetricConfiguration GetNAConfiguration()
        {
            var naConfiguration = new PolicyMetricConfiguration();
            naConfiguration.m_MetricName =
            naConfiguration.m_MetricDescription =
            naConfiguration.m_ValidValues =
            naConfiguration.m_ValueDescription =
            naConfiguration.m_ReportKey =
            naConfiguration.m_ReportText =
            naConfiguration.m_SeverityValues =
                new SqlString(Utility.Constants.POLICY_METRIC_CONSTANT_NOT_APPLICABLE);
            return naConfiguration;
        }

        private void setValues(SqlDataReader rdr)
        {
           m_PolicyId = rdr.GetSqlInt32((int)PolicyMetricConfigurationColumn.PolicyId);
           m_AssessmentId = rdr.GetSqlInt32((int)PolicyMetricConfigurationColumn.AssessmentId);

           var serverType = rdr.GetSqlString((int)PolicyMetricConfigurationColumn.ServerType);
           m_ServerType = Helper.ConvertSQLTypeStringToEnum(serverType.IsNull ? string.Empty : serverType.Value);

           m_MetricId = rdr.GetSqlInt32((int)PolicyMetricConfigurationColumn.MetricId);
           m_MetricName = rdr.GetSqlString((int)PolicyMetricConfigurationColumn.MetricName);
           m_MetricDescription = rdr.GetSqlString((int)PolicyMetricConfigurationColumn.MetricDescription);
           m_ValidValues = rdr.GetSqlString((int)PolicyMetricConfigurationColumn.ValidValues);
           m_ValueDescription = rdr.GetSqlString((int)PolicyMetricConfigurationColumn.ValueDescription);
           m_ReportKey = rdr.GetSqlString((int)PolicyMetricConfigurationColumn.ReportKey);
           m_ReportText = rdr.GetSqlString((int)PolicyMetricConfigurationColumn.ReportText);
           m_Severity = rdr.GetSqlInt32((int)PolicyMetricConfigurationColumn.Severity);
           m_SeverityValues = rdr.GetSqlString((int)PolicyMetricConfigurationColumn.SeverityValues);
        }


        #endregion

    }
}
