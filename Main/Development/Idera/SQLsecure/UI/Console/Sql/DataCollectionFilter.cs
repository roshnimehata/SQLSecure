/******************************************************************
 * Name: DataCollectionFilter.cs
 *
 * Description: Encapsulates data collection filter.
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Diagnostics;

using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Utility;

namespace Idera.SQLsecure.UI.Console.Sql
{
    public class DataCollectionFilter
    {
        #region Types

        public enum Disposition
        {
            Unchanged,
            New,
            Modified,
            Deleted
        }

        public enum RuleType
        {
            Server,
            ExtendedSP,
            Database,
            Unknown
        }

        public class Rule
        {
            #region Fields

            private int m_RuleId = Constants.InvalidId;
            private RuleObjectType m_ObjectType = RuleObjectType.Database;
            private RuleScope m_ObjectScope = RuleScope.All;
            private string m_MatchString = string.Empty;

            #endregion

            #region Helpers

            private static RuleObjectType getRuleObjectType(SqlInt32 objType)
            {
                Debug.Assert(!objType.IsNull);
                return (objType.IsNull ? RuleObjectType.Unknown : ((RuleObjectType)objType.Value));
            }

            private static RuleScope getRuleScope(SqlString objScope)
            {
                Debug.Assert(!objScope.IsNull);
                RuleScope fs = RuleScope.Unknown;
                if (objScope.IsNull)
                {
                    fs = RuleScope.Unknown;
                }
                else
                {
                    if (string.Compare(objScope.Value, Sql.Constants.RuleScopeAll, true) == 0)
                    {
                        fs = RuleScope.All;
                    }
                    else if (string.Compare(objScope.Value, Sql.Constants.RuleScopeSystem, true) == 0)
                    {
                        fs = RuleScope.System;
                    }
                    else if (string.Compare(objScope.Value, Sql.Constants.RuleScopeUser, true) == 0)
                    {
                        fs = RuleScope.User;
                    }
                    else
                    {
                        fs = RuleScope.Unknown;
                    }
                }

                return fs;
            }

            private static string getRuleScope(RuleScope objScope)
            {
                string rs = string.Empty;
                switch (objScope)
                {
                    case RuleScope.All:
                        rs = Constants.RuleScopeAll;
                        break;
                    case RuleScope.System:
                        rs = Constants.RuleScopeSystem;
                        break;
                    case RuleScope.User:
                        rs = Constants.RuleScopeUser;
                        break;
                    case RuleScope.Unknown:
                    default:
                        Debug.Assert(false);
                        rs = string.Empty;
                        break;
                }
                return rs;
            }

            #endregion

            #region Ctors

            public Rule(
                    RuleObjectType objectType,
                    RuleScope objectScope,
                    string matchString
                )
            {
                m_ObjectType = objectType;
                m_ObjectScope = objectScope;
                m_MatchString = matchString;
            }

            public Rule(
                    SqlInt32 ruleId,
                    SqlInt32 objType,
                    SqlString objScope,
                    SqlString matchString
                )
            {
                Debug.Assert(!ruleId.IsNull);
                Debug.Assert(!objType.IsNull);

                m_RuleId = ruleId.IsNull ? Constants.InvalidId : ruleId.Value;
                m_ObjectType = getRuleObjectType(objType);
                m_ObjectScope = getRuleScope(objScope);
                m_MatchString = matchString.IsNull ? string.Empty : matchString.Value;
            }

            #endregion

            #region Properties

            public int RuleId
            {
                get { return m_RuleId; }
            }

            public RuleObjectType ObjectType
            {
                get { return m_ObjectType; }
            }

            public RuleScope ObjectScope
            {
                get { return m_ObjectScope; }
            }

            public string ObjectScopeString
            {
                get { return getRuleScope(m_ObjectScope); }
            }

            public string MatchString
            {
                get { return m_MatchString; }
            }

            #endregion
        }

        #endregion

        #region Constants

        private const string FilterDetailsPrefix = @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fswiss\fcharset0 Microsoft Sans Serif;}{\f1\fmodern\fcharset0 Courier New;}{\f2\fnil\fcharset2 Symbol;}}
{\*\generator Msftedit 5.41.15.1507;}\viewkind4\uc1\pard\f0\fs16\par";
        private const string AlwaysCollected = @"\pard{\pntext\f2\'B7\tab}{\*\pn\pnlvlblt\pnf2\pnindent360{\pntxtb\'B7}}\fi-360\li720\tx720 All Server objects, Database Security objects, Stored Procedures and Extended Stored Procedures \par";

        private const string FilterDetailsPrefixForSubReport = @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fswiss\fcharset0 Microsoft Sans Serif;}{\f1\fmodern\fcharset0 Courier New;}{\f2\fnil\fcharset2 Symbol;}}
{\*\generator Msftedit 5.41.15.1507;}\viewkind4\uc1\pard\f0\fs16\par";

        private const string RuleSrvrPrefix = @"\pard{\pntext\f2\'B7\tab}{\*\pn\pnlvlblt\pnf2\pnindent360{\pntxtb\'B7}}\fi-360\li720\tx720 All ";
        private const string RuleSrvrSuffix = @"\par";

        private const string RuleAllExtendedSPs = @"\pard{\pntext\f2\'B7\tab}{\*\pn\pnlvlblt\pnf2\pnindent360{\pntxtb\'B7}}\fi-360\li720\tx720 All Extended Stored Procedures\par";

        private const string RuleDbPrefix = @"{\pntext\f2\'B7\tab}";
        private const string RuleDbSuffix = @"\par";
//        private const string RuleDbLastSuffix = @"{\*\pn\pnlvlcont\pnf0\pnindent0\pnstart1\pndec }\fi-360\li1440\tx1440\f1";
        private const string RuleDbLastSuffix = @"\pard";
        private const string RuleTblPrefix = @"\f1\tab o  \f0";
        private const string RuleTblSuffix = @"\par";
        private const string RuleSpPrefix = @"\f1\tab o  \f0";
        private const string RuleSpSuffix = @"\par";
        private const string RuleOtherPrefix = @"\f1\tab o  \f0 All ";
        private const string RuleOtherSuffix = @"\par";

        #endregion

        #region Queries

        // Check if a specified rule name exists.
        private const string QueryGetRegisteredServerFilterByName
                                = @"SELECT 
                                        filterruleheaderid, 
                                        filterruleid
                                    FROM SQLsecure.dbo.vwfilterrules
                                    WHERE connectionname = @instance AND rulename = @name";
        private const string ParamGetRegisteredServerFilterByNameInstance = "instance";
        private const string ParamGetRegisteredServerFilterByNameName = "name";

        // Write rule to repository.
        private const string QueryWriteFilterToRepository = @"SQLsecure.dbo.isp_sqlsecure_addruleheader";
		private const string ParamWriteFilterToRepositoryConnectionname = "@connectionname";
        private const string ParamWriteFilterToRepositoryRulename = "@rulename";
        private const string ParamWriteFilterToRepositoryDescription = "@description";

        // Write filter to repository.
        private const string QueryWriteRuleToRepository = @"SQLsecure.dbo.isp_sqlsecure_addrule";
        private const string ParamWriteRuleToRepositoryRuleheaderid = "@ruleheaderid";
		private const string ParamWriteRuleToRepositoryClass = "@class";
		private const string ParamWriteRuleToRepositoryScope = "@scope";
        private const string ParamWriteRuleToRepositoryMatchstring = "@matchstring";

        // Delete filter from repository.
        private const string NonQueryDeleteFilterFromRepository = @"SQLsecure.dbo.isp_sqlsecure_removeruleheader";
        private const string ParamDeleteFilterFromRepositoryFilterruleheaderid = @"filterruleheaderid";

        // Delete rules from repository.
        private const string NonQueryDeleteRulesFromRepository = @"SQLsecure.dbo.isp_sqlsecure_removerule";
        private const string ParamDeleteRulesFromRepositoryFilterruleheaderid = @"filterruleheaderid";

        // Update rule header.
        private const string NonQueryUpdateFilterHeader = @"SQLsecure.dbo.isp_sqlsecure_updateruleheader";
        private const string ParamUpdateFilterHeaderFilterruleheaderid = @"filterruleheaderid";
        private const string ParamUpdateFilterHeaderRulename = @"rulename";
        private const string ParamUpdateFilterHeaderDescription = @"description";

        // Get filters for a registered server.
        private const string QueryGetRegisteredServerFilters
                                = @"SELECT 
                                        filterruleheaderid, 
                                        connectionname, 
                                        rulename, 
                                        description,
                                        createdby, 
                                        createdtm, 
                                        lastmodifiedtm, 
                                        lastmodifiedby, 
                                        filterruleid, 
                                        class, 
                                        scope, 
                                        matchstring 
                                    FROM SQLsecure.dbo.vwfilterrules
                                    WHERE connectionname = @instance";
        private const string ParamQueryGetRegisteredServerFiltersInstance = "instance";
        private enum RegisteredServerFiltersColumn
        {
            FilterRuleHeaderId = 0,
            ConnectionName,
            RuleName,
            Description,
            CreatedBy,
            CreatedTm,
            LastModifiedTm,
            LastModifiedBy,
            FilterRuleId,
            Class,
            Scope,
            MatchString
        };

        // Get filters for a snapshot.
        private const string QueryGetSnapshotFilters
                                = @"SELECT 
                                        filterruleheaderid, 
                                        snapshotid, 
                                        rulename, 
                                        description,
                                        createdby, 
                                        createdtm, 
                                        lastmodifiedtm, 
                                        lastmodifiedby, 
                                        filterruleid, 
                                        class, 
                                        scope, 
                                        matchstring 
                                    FROM SQLsecure.dbo.vwsnapshotfilterrules
                                    WHERE snapshotid = @snapshotid";
        private const string ParamQueryGetSnapshotFiltersSnapshotid = "snapshotid";
        private enum SnapshotFiltersColumn
        {
            FilterRuleHeaderId = 0,
            SnapshotId,
            RuleName,
            Description,
            CreatedBy,
            CreatedTm,
            LastModifiedTm,
            LastModifiedBy,
            FilterRuleId,
            Class,
            Scope,
            MatchString
        };

        #endregion

        #region Fields

        private Disposition m_Disposition;
        private int m_FilterId = Constants.InvalidId;
        private string m_Instance;
        private string m_FilterName;
        private string m_CreatedBy = string.Empty;
        private DateTime m_CreationTime = DateTime.Now;
        private string m_LastModifiedBy = string.Empty;
        private DateTime m_LastModificationTime = DateTime.Now;
        private string m_Description;
        private List<Rule> m_Rules = new List<Rule>();
        private RuleType m_RuleType = RuleType.Unknown;

        private bool m_FilterDetailsSubReport;

        #endregion

        #region Helpers

        private static void createNewFilter(
                SqlConnection connection,
                string instance,
                DataCollectionFilter filter
            )
        {
            Debug.Assert(connection != null);
            Debug.Assert(!string.IsNullOrEmpty(instance));
            Debug.Assert(filter != null);
            Debug.Assert(filter.FilterDisposition == Disposition.New);

            // Setup register server params.
            SqlParameter paramConnectionname = new SqlParameter(ParamWriteFilterToRepositoryConnectionname, instance);
            SqlParameter paramRulename = new SqlParameter(ParamWriteFilterToRepositoryRulename, filter.FilterName);
            SqlParameter paramDescription = new SqlParameter(ParamWriteFilterToRepositoryDescription, filter.Description);

            // Execure stored procedure and get the header id.
            int hdrid = 0;
            using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.StoredProcedure,
                        QueryWriteFilterToRepository, new SqlParameter[] { paramConnectionname, paramRulename, paramDescription }))
            {
                if (rdr.Read())
                {
                    hdrid = rdr.GetInt32(0);
                }
            }

            // Add all the filters to the filter table.
            SqlParameter paramRuleheaderid = new SqlParameter(ParamWriteRuleToRepositoryRuleheaderid, hdrid);
            foreach (Rule rule in filter.Rules)
            {
                SqlParameter paramClass = new SqlParameter(ParamWriteRuleToRepositoryClass, rule.ObjectType);
                SqlParameter paramScope = new SqlParameter(ParamWriteRuleToRepositoryScope, rule.ObjectScopeString);
                SqlParameter paramMatchstring = new SqlParameter(ParamWriteRuleToRepositoryMatchstring, rule.MatchString);

                Sql.SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure,
                                QueryWriteRuleToRepository, new SqlParameter[] { paramRuleheaderid, paramClass, 
                                                        paramScope, paramMatchstring });

            }
        }

        private static void deleteFilter(
                SqlConnection connection,
                string instance,
                DataCollectionFilter filter
            )
        {
            Debug.Assert(connection != null);
            Debug.Assert(!string.IsNullOrEmpty(instance));
            Debug.Assert(filter != null);
            Debug.Assert(filter.FilterDisposition == Disposition.Deleted);

            // Setup filter id param, and delete the filter.
            SqlParameter paramHeaderId = new SqlParameter(ParamDeleteFilterFromRepositoryFilterruleheaderid, filter.FilterId);
            Sql.SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure,
                            NonQueryDeleteFilterFromRepository, new SqlParameter[] { paramHeaderId });
        }

        private static void updateFilter(
                SqlConnection connection,
                string instance,
                DataCollectionFilter filter
            )
        {
            Debug.Assert(connection != null);
            Debug.Assert(!string.IsNullOrEmpty(instance));
            Debug.Assert(filter != null);
            Debug.Assert(filter.FilterDisposition == Disposition.Modified);

            // Delete the filter rules.
            SqlParameter paramDeleteHeaderId = new SqlParameter(ParamDeleteRulesFromRepositoryFilterruleheaderid, filter.FilterId);
            Sql.SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure,
                            NonQueryDeleteRulesFromRepository, new SqlParameter[] { paramDeleteHeaderId });

            // Update filter header.
            SqlParameter paramUpdateHeaderId = new SqlParameter(ParamUpdateFilterHeaderFilterruleheaderid, filter.FilterId);
            SqlParameter paramRulename = new SqlParameter(ParamUpdateFilterHeaderRulename, filter.FilterName);
            SqlParameter paramDescription = new SqlParameter(ParamUpdateFilterHeaderDescription, filter.Description);
            SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure,
                            NonQueryUpdateFilterHeader, new SqlParameter[] { paramUpdateHeaderId, paramRulename, paramDescription });

            // Add new rules.
            SqlParameter paramAddRuleHeaderId = new SqlParameter(ParamWriteRuleToRepositoryRuleheaderid, filter.FilterId);
            foreach (Rule rule in filter.Rules)
            {
                SqlParameter paramClass = new SqlParameter(ParamWriteRuleToRepositoryClass, rule.ObjectType);
                SqlParameter paramScope = new SqlParameter(ParamWriteRuleToRepositoryScope, rule.ObjectScopeString);
                SqlParameter paramMatchstring = new SqlParameter(ParamWriteRuleToRepositoryMatchstring, rule.MatchString);

                Sql.SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure,
                                QueryWriteRuleToRepository, new SqlParameter[] { paramAddRuleHeaderId, paramClass, 
                                                        paramScope, paramMatchstring });

            }
        }

        private string serverDetails()
        {
            Debug.Assert(Type == RuleType.Server);

            // Initialize the string builder object.
            StringBuilder details = new StringBuilder();
            details.Append(FilterDetailsPrefix);
            details.Append(AlwaysCollected);
            StringBuilder objs = new StringBuilder();

            // Process each of the rules.
            foreach (DataCollectionFilter.Rule rule in m_Rules)
            {
                switch (rule.ObjectType)
                {
                    case RuleObjectType.Login:
                        switch (objs.Length)
                        {
                            case 0:
                                objs.Append("Server Principals");
                                break;
                            default:
                                objs.Append(" and Server Principals");
                                break;
                        }
                        break;
                    case RuleObjectType.Endpoint:
                        switch (objs.Length)
                        {
                            case 0:
                                objs.Append("Endpoints");
                                break;
                            default:
                                objs.Append(" and Endpoints");
                                break;
                        }
                        break;
                    default:
                        Debug.Assert(false);
                        break;
                }
            }

            // Fill the details.
            details.Append(RuleSrvrPrefix);
            details.Append(objs.ToString());
            details.Append(RuleSrvrSuffix);

            return details.ToString();
        }

        private string xspDetails()
        {
            Debug.Assert(Type == RuleType.ExtendedSP);

            // Initialize the string builder object.
            StringBuilder details = new StringBuilder();
            details.Append(FilterDetailsPrefix);

            // Process each of the rules.
            foreach (DataCollectionFilter.Rule rule in m_Rules)
            {
                switch (rule.ObjectType)
                {
                    case RuleObjectType.ExtendedStoredProcedure:
                        details.Append(RuleAllExtendedSPs);
                        break;
                    case RuleObjectType.Database:
                        // Do nothing
                        break;
                    default:
                        Debug.Assert(false);
                        break;
                }
            }

            return details.ToString();
        }

        private string databaseDetails()
        {
            Debug.Assert(Type == RuleType.Database);

            // Initialize the string builder objects.
            StringBuilder details = new StringBuilder();
            if (m_FilterDetailsSubReport)
            {
                details.Append(FilterDetailsPrefixForSubReport);
            }
            else
            {
                details.Append(FilterDetailsPrefix);
            }
            details.Append(AlwaysCollected);
            StringBuilder dbDetails = new StringBuilder();
            StringBuilder tblDetails = new StringBuilder();
            StringBuilder tblMatchString = new StringBuilder();
            StringBuilder viewDetails = new StringBuilder();
            StringBuilder viewMatchString = new StringBuilder();
            StringBuilder functionDetails = new StringBuilder();
            StringBuilder functionMatchString = new StringBuilder();
            List<string> otherObjs = new List<string>();

            // Process each of the rules.
            foreach (DataCollectionFilter.Rule rule in m_Rules)
            {
                switch (rule.ObjectType)
                {
                    case RuleObjectType.Database:
                        dbDetails.Append(RuleDbPrefix);
                        dbDetails.Append("All ");
                        dbDetails.Append(DescriptionHelper.GetEnumDescription(rule.ObjectScope));
                        dbDetails.Append(" Databases");
                        if (!string.IsNullOrEmpty(rule.MatchString))
                        {
                            dbDetails.Append(" with names matching '" + rule.MatchString + "'");
                        }
                        if (m_Rules.Count > 1) { dbDetails.Append(RuleDbSuffix); }
                        break;
                    case RuleObjectType.Table:
                        if (tblDetails.Length == 0)
                        {
                            tblDetails.Append(RuleTblPrefix);
                            tblDetails.Append("All ");
                            tblDetails.Append(DescriptionHelper.GetEnumDescription(rule.ObjectScope));
                            tblDetails.Append(" Tables");
                        }
                        if (!string.IsNullOrEmpty(rule.MatchString))
                        {
                            switch (tblMatchString.Length)
                            {
                                case 0:
                                    tblMatchString.Append(" with names matching '");
                                    break;
                                default:
                                    tblMatchString.Append(", ");
                                    break;
                            }
                            tblMatchString.Append(rule.MatchString);
                        }
                        break;
                    case RuleObjectType.StoredProcedure:
                        break;
                    case RuleObjectType.View:
                        if (viewDetails.Length == 0)
                        {
                            viewDetails.Append(RuleSpPrefix);
                            viewDetails.Append("All ");
                            viewDetails.Append(DescriptionHelper.GetEnumDescription(rule.ObjectScope));
                            viewDetails.Append(" Views");
                        }
                        if (!string.IsNullOrEmpty(rule.MatchString))
                        {
                            switch (viewMatchString.Length)
                            {
                                case 0:
                                    viewMatchString.Append(" with names matching '");
                                    break;
                                default:
                                    viewMatchString.Append(", ");
                                    break;
                            }
                            viewMatchString.Append(rule.MatchString);
                        }
                        break;
                    case RuleObjectType.Function:
                        if (functionDetails.Length == 0)
                        {
                            functionDetails.Append(RuleSpPrefix);
                            functionDetails.Append("All ");
                            functionDetails.Append(DescriptionHelper.GetEnumDescription(rule.ObjectScope));
                            functionDetails.Append(" Functions");
                        }
                        if (!string.IsNullOrEmpty(rule.MatchString))
                        {
                            switch (functionMatchString.Length)
                            {
                                case 0:
                                    functionMatchString.Append(" with names matching '");
                                    break;
                                default:
                                    functionMatchString.Append(", ");
                                    break;
                            }
                            functionMatchString.Append(rule.MatchString);
                        }
                        break;
                    case RuleObjectType.ExtendedStoredProcedure:
                        break;
                    case RuleObjectType.User:
                        break;
                    case RuleObjectType.Role:
                        break;
                    case RuleObjectType.Assembly:
                        otherObjs.Add("Assemblies");
                        break;
                    case RuleObjectType.Certificate:
                        otherObjs.Add("Certificates");
                        break;
                    case RuleObjectType.FullTextCatalog:
                        otherObjs.Add("Full Text Catalogs");
                        break;
                    case RuleObjectType.Key:
                        otherObjs.Add("Keys");
                        break;
                    case RuleObjectType.UserDefinedDataType:
                        otherObjs.Add("User-defined Data Types");
                        break;
                    case RuleObjectType.XMLSchemaCollection:
                        otherObjs.Add("XML Schema Collections");
                        break;
                    case RuleObjectType.Synonym:
                        otherObjs.Add("Synonyms");
                        break;
                    default:
                        Debug.Assert(false);
                        break;
                }
            }

            // Fill the details string builder.
            //if (espDetail.Length > 0)
            //{
            //    details.Append(RuleDbPrefix);
            //    details.Append(espDetail.ToString());
            //    details.Append(RuleDbSuffix);
            //}
            details.Append(dbDetails.ToString());
            if (tblDetails.Length > 0 || viewDetails.Length > 0 || functionDetails.Length > 0 || otherObjs.Count > 0)
            {
                details.Append(RuleDbLastSuffix);
            }
            if (tblDetails.Length > 0) 
            { 
                details.Append(tblDetails.ToString());
                if (tblMatchString.Length > 0)
                {
                    details.Append(tblMatchString.ToString());
                    details.Append("'");
                }
                details.Append(RuleTblSuffix);
            }
            if (viewDetails.Length > 0)
            {
                details.Append(viewDetails.ToString());
                if (viewMatchString.Length > 0)
                {
                    details.Append(viewMatchString.ToString());
                    details.Append("'");
                }
                details.Append(RuleSpSuffix);
            }

            if (functionDetails.Length > 0)
            {
                details.Append(functionDetails.ToString());
                if (functionMatchString.Length > 0)
                {
                    details.Append(functionMatchString.ToString());
                    details.Append("'");
                }
                details.Append(RuleSpSuffix);
            }
            if (otherObjs.Count > 0)
            {
                details.Append(RuleOtherPrefix);
                if (otherObjs.Count == 1)
                {
                    details.Append(otherObjs[0]);
                }
                else
                {
                    int last = otherObjs.Count - 1;
                    for(int i = 0; i < otherObjs.Count; ++i)
                    {
                        if (i == 0)
                        {
                            details.Append(otherObjs[i]);
                        }
                        else
                        {
                            details.Append(((i != last) ? ", " : " and ") + otherObjs[i]);
                        }
                    }
                }
                details.Append(RuleOtherSuffix);
            }

            return details.ToString();
        }

        #endregion

        #region Ctors

        public DataCollectionFilter(
                string instance,
                string filterName,
                string description
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(instance));
            Debug.Assert(!string.IsNullOrEmpty(filterName));

            m_Disposition = Disposition.New;
            m_Instance = instance;
            m_FilterName = filterName;
            m_Description = description;
        }

        public DataCollectionFilter(
                SqlInt32 filterId,
                SqlString instance,
                SqlString filterName,
                SqlString description,
                SqlString createdBy,
                SqlDateTime creationTime,
                SqlString lastModifiedBy,
                SqlDateTime lastModificationTime
            )
        {
            Debug.Assert(!filterId.IsNull);
            Debug.Assert(!instance.IsNull);
            Debug.Assert(!filterName.IsNull);

            m_Disposition = Disposition.Unchanged;
            m_FilterId = filterId.IsNull ? Constants.InvalidId : filterId.Value;
            m_Instance = instance.IsNull ? string.Empty : instance.Value;
            m_FilterName = filterName.IsNull ? string.Empty : filterName.Value;
            m_Description = description.IsNull ? string.Empty : description.Value;
            m_CreatedBy = createdBy.IsNull ? string.Empty : createdBy.Value;
            m_CreationTime = creationTime.IsNull ? DateTime.Now : creationTime.Value;
            m_LastModifiedBy = lastModifiedBy.IsNull ? string.Empty : lastModifiedBy.Value;
            m_LastModificationTime = lastModificationTime.IsNull ? DateTime.Now : lastModificationTime.Value;
        }

        public DataCollectionFilter(
                SqlInt32 filterId,
                string instance,
                SqlString filterName,
                SqlString description,
                SqlString createdBy,
                SqlDateTime creationTime,
                SqlString lastModifiedBy,
                SqlDateTime lastModificationTime
            )
        {
            Debug.Assert(!filterId.IsNull);
            Debug.Assert(!string.IsNullOrEmpty(instance));
            Debug.Assert(!filterName.IsNull);

            m_Disposition = Disposition.Unchanged;
            m_FilterId = filterId.IsNull ? Constants.InvalidId : filterId.Value;
            m_Instance = instance;
            m_FilterName = filterName.IsNull ? string.Empty : filterName.Value;
            m_Description = description.IsNull ? string.Empty : description.Value;
            m_CreatedBy = createdBy.IsNull ? string.Empty : createdBy.Value;
            m_CreationTime = creationTime.IsNull ? DateTime.Now : creationTime.Value;
            m_LastModifiedBy = lastModifiedBy.IsNull ? string.Empty : lastModifiedBy.Value;
            m_LastModificationTime = lastModificationTime.IsNull ? DateTime.Now : lastModificationTime.Value;
        }

        #endregion

        #region Properties

        public Disposition FilterDisposition
        {
            get { return m_Disposition; }
            set 
            {
                // Change the disposition, only if the original
                // disposition is not new and new disposition is modify.
                // Because if the filter is new, then modification means
                // its still new.   If its set to modified it will 
                // have a database exception because the filterruleheader
                // row will not have been created.
                if (!(m_Disposition == Disposition.New && value == Disposition.Modified))
                {
                    m_Disposition = value;
                }
            }
        }

        public int FilterId
        {
            get { return m_FilterId; }
        }

        public string Instance
        {
            get { return m_Instance; }
        }

        public string FilterName
        {
            get { return m_FilterName; }
            set { m_FilterName = value; }
        }

        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }

        public string CreatedBy
        {
            get { return m_CreatedBy; }
        }

        public DateTime CreationTime
        {
            get { return m_CreationTime; }
        }

        public string LastModifiedBy
        {
            get { return m_LastModifiedBy; }
        }

        public DateTime LastModificationTime
        {
            get { return m_LastModificationTime; }
        }

        public List<Rule> Rules
        {
            get { return m_Rules; }
        }

        public RuleType Type
        {
            get { return m_RuleType; }
        }

        public string GetFilterDetailsForSubReport()
        {
            string text;
            m_FilterDetailsSubReport = true;
            text = FilterDetails;
            m_FilterDetailsSubReport = false;
            return text;
        }

        public string FilterDetails
        {
            get
            {
                Debug.Assert(m_Rules.Count > 0);

                // Process based on the rule type.
                string details = string.Empty;
                switch (Type)
                {
                    case RuleType.Server:
                        details = serverDetails();
                        break;
                    case RuleType.ExtendedSP:
                        details = xspDetails();
                        break;
                    case RuleType.Database:
                        details = databaseDetails();
                        break;
                    default:
                        Debug.Assert(false);
                        break;
                }

                // Return the detail string.
                return details;
            }
        }

        #endregion

        #region Methods

        public void AddDatabaseRules(List<Rule> Rules)
        {
            m_Rules.AddRange(Rules);
            m_RuleType = RuleType.Database;
        }

        public void AddRule(Rule rule)
        {
            Debug.Assert(rule != null);

            // No rule, do nothing.
            if (rule == null) { return; }

            // Determine type of filter.
            switch (rule.ObjectType)
            {
                case RuleObjectType.Login:
                case RuleObjectType.Endpoint:
                    m_RuleType = RuleType.Server;
                    break;
                case RuleObjectType.ExtendedStoredProcedure:
                case RuleObjectType.Database:
                case RuleObjectType.User:
                case RuleObjectType.Role:
                case RuleObjectType.Assembly:
                case RuleObjectType.Certificate:
                case RuleObjectType.FullTextCatalog:
                case RuleObjectType.Key:
                case RuleObjectType.Schema:
                case RuleObjectType.UserDefinedDataType:
                case RuleObjectType.XMLSchemaCollection:
                case RuleObjectType.Table:
                case RuleObjectType.StoredProcedure:
                case RuleObjectType.View:
                case RuleObjectType.Function:
                case RuleObjectType.Synonym:
                    m_RuleType = RuleType.Database;
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }

            // Add rule to 
            m_Rules.Add(rule);
        }

        public void ClearRules()
        {
            m_Rules.Clear();
        }

        public static bool DoesFilterExistForInstance(
                string connectionString,
                string instance,
                string ruleName
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(connectionString));
            Debug.Assert(!string.IsNullOrEmpty(instance));
            Debug.Assert(!string.IsNullOrEmpty(ruleName));
 
            // Open connection to repository and retrieve rule for the given instance
            // and rule name.
            bool hasRuleWithName = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open the connection.
                connection.Open();

                // Query the repository for filter rules for the server.
                SqlParameter paramInstance = new SqlParameter(ParamGetRegisteredServerFilterByNameInstance, instance);
                SqlParameter paramName = new SqlParameter(ParamGetRegisteredServerFilterByNameName, ruleName);
                using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                            QueryGetRegisteredServerFilterByName, new SqlParameter[] { paramInstance, paramName }))
                {
                    hasRuleWithName = rdr.HasRows;
                }
            }

            return hasRuleWithName;
        }

        public void CreateFilter(
                string connectionString,
                string instance
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(connectionString));
            Debug.Assert(!string.IsNullOrEmpty(instance));
            Debug.Assert(m_Disposition == Disposition.New);

            // Open connection to repository and update rule.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open the connection.
                connection.Open();

                // Create filter.
                createNewFilter(connection, instance, this);
            }
        }

        public static void GetFilters(
                string connectionString,
                string instance,
                out List<DataCollectionFilter> filters
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(connectionString));
            Debug.Assert(!string.IsNullOrEmpty(instance));

            // Init return.
            filters = new List<DataCollectionFilter>();
 
            // Open connection to repository and retrieve rules.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open the connection.
                connection.Open();

                // Query the repository for filter rules for the server.
                Dictionary<int, DataCollectionFilter> filterDictionary = new Dictionary<int, DataCollectionFilter>();
                SqlParameter paramInstance = new SqlParameter(ParamQueryGetRegisteredServerFiltersInstance, instance);
                using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                            QueryGetRegisteredServerFilters, new SqlParameter[] { paramInstance }))
                {
                    while(rdr.Read())
                    {
                        // Read the filter id.
                        SqlInt32 filterId = rdr.GetSqlInt32((int)RegisteredServerFiltersColumn.FilterRuleHeaderId);
                        Debug.Assert(!filterId.IsNull);
                        if(filterId.IsNull) { continue; } // invalid filter id continue.

                        // If the filter is not in the dictionary, create object and add to the dictionary.
                        DataCollectionFilter filter = null;
                        if (!filterDictionary.TryGetValue(filterId.Value, out filter))
                        {
                            SqlString connectionName = rdr.GetSqlString((int)RegisteredServerFiltersColumn.ConnectionName);
                            SqlString filterName = rdr.GetSqlString((int)RegisteredServerFiltersColumn.RuleName);
                            SqlString description = rdr.GetSqlString((int)RegisteredServerFiltersColumn.Description);
                            SqlString createdBy = rdr.GetSqlString((int)RegisteredServerFiltersColumn.CreatedBy);
                            SqlDateTime creationTime = rdr.GetSqlDateTime((int)RegisteredServerFiltersColumn.CreatedTm);
                            SqlString lastModifiedBy = rdr.GetSqlString((int)RegisteredServerFiltersColumn.LastModifiedBy);
                            SqlDateTime lastModificationTime = rdr.GetSqlDateTime((int)RegisteredServerFiltersColumn.LastModifiedTm);
                            filter = new DataCollectionFilter(filterId, connectionName, filterName, description, createdBy, creationTime,
                                                                lastModifiedBy, lastModificationTime);
                            filterDictionary.Add(filterId.Value, filter);
                        }

                        // Query for rule fields and add new rule to filter.
                        SqlInt32 ruleId = rdr.GetInt32((int)RegisteredServerFiltersColumn.FilterRuleId);
                        SqlInt32 objClass = rdr.GetSqlInt32((int)RegisteredServerFiltersColumn.Class);
                        SqlString objScope = rdr.GetSqlString((int)RegisteredServerFiltersColumn.Scope);
                        SqlString matchString = rdr.GetSqlString((int)RegisteredServerFiltersColumn.MatchString);
                        Rule rule = new Rule(ruleId, objClass, objScope, matchString);
                        filter.AddRule(rule);
                    }

                    // Add the filters to the list.
                    foreach (KeyValuePair<int, DataCollectionFilter> kvp in filterDictionary)
                    {
                         filters.Add(kvp.Value);
                    }
                }
            }
       }

        public static List<DataCollectionFilter> GetSnapshotFilters(
                string instance,
                int snapshotid
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(instance));
            Debug.Assert(snapshotid != 0);

            // Init return.
            List<DataCollectionFilter> filters = new List<DataCollectionFilter>();

            try
            {
                // Open connection to repository and retrieve rules.
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Query the repository for filter rules for the server.
                    Dictionary<int, DataCollectionFilter> filterDictionary = new Dictionary<int, DataCollectionFilter>();
                    SqlParameter paramSnapshotid = new SqlParameter(ParamQueryGetSnapshotFiltersSnapshotid, snapshotid);
                    using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                QueryGetSnapshotFilters, new SqlParameter[] { paramSnapshotid }))
                    {
                        while (rdr.Read())
                        {
                            // Read the filter id.
                            SqlInt32 filterId = rdr.GetSqlInt32((int)SnapshotFiltersColumn.FilterRuleHeaderId);
                            Debug.Assert(!filterId.IsNull);
                            if (filterId.IsNull) { continue; } // invalid filter id continue.

                            // If the filter is not in the dictionary, create object and add to the dictionary.
                            DataCollectionFilter filter = null;
                            if (!filterDictionary.TryGetValue(filterId.Value, out filter))
                            {
                                SqlString filterName = rdr.GetSqlString((int)SnapshotFiltersColumn.RuleName);
                                SqlString description = rdr.GetSqlString((int)SnapshotFiltersColumn.Description);
                                SqlString createdBy = rdr.GetSqlString((int)SnapshotFiltersColumn.CreatedBy);
                                SqlDateTime creationTime = rdr.GetSqlDateTime((int)SnapshotFiltersColumn.CreatedTm);
                                SqlString lastModifiedBy = rdr.GetSqlString((int)SnapshotFiltersColumn.LastModifiedBy);
                                SqlDateTime lastModificationTime = rdr.GetSqlDateTime((int)SnapshotFiltersColumn.LastModifiedTm);
                                filter = new DataCollectionFilter(filterId, instance, filterName, description, createdBy, creationTime,
                                                                    lastModifiedBy, lastModificationTime);
                                filterDictionary.Add(filterId.Value, filter);
                            }

                            // Query for rule fields and add new rule to filter.
                            SqlInt32 ruleId = rdr.GetInt32((int)SnapshotFiltersColumn.FilterRuleId);
                            SqlInt32 objClass = rdr.GetSqlInt32((int)SnapshotFiltersColumn.Class);
                            SqlString objScope = rdr.GetSqlString((int)SnapshotFiltersColumn.Scope);
                            SqlString matchString = rdr.GetSqlString((int)SnapshotFiltersColumn.MatchString);
                            Rule rule = new Rule(ruleId, objClass, objScope, matchString);
                            filter.AddRule(rule);
                        }

                        // Add the filters to the list.
                        foreach (KeyValuePair<int, DataCollectionFilter> kvp in filterDictionary)
                        {
                            filters.Add(kvp.Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ErrorMsgs.CantGetSnapshotFilters, ex);
                filters = null;
            }

            return filters;
        }

        public static void UpdateFilters(
                string connectionString,
                string instance,
                List<DataCollectionFilter> filters
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(connectionString));
            Debug.Assert(!string.IsNullOrEmpty(instance));

            // Open connection to repository and update rule.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open the connection.
                connection.Open();

                // Update repository.
                foreach (DataCollectionFilter filter in filters)
                {
                    // Process based on filter disposition.
                    switch (filter.FilterDisposition)
                    {
                        case Disposition.Unchanged: // No changes
                            // Do nothing.
                            break;
                        case Disposition.New: // Create new entry in the repository.
                            createNewFilter(connection, instance, filter);
                            break;
                        case Disposition.Modified: // Update repository.
                            updateFilter(connection, instance, filter);
                            break;
                        case Disposition.Deleted: // Remove from the repository.
                            deleteFilter(connection, instance, filter);
                            break;
                        default:
                            Debug.Assert(false);
                            break;
                    }
                }
            }
        }

        #endregion
    }
}
