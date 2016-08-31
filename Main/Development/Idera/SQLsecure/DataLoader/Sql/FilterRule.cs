/******************************************************************
 * Name: FilterRule.cs
 *
 * Description: The data collection filters are encapsulated in this 
 * class.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using Idera.SQLsecure.Core.Logger;

namespace Idera.SQLsecure.Collector.Sql
{
    internal static class CollectionFilter
    {
        private static LogX logX = new LogX("Idera.SQLsecure.Collector.Sql.CollectionFilter");

        // ------------- Helpers -------------
        #region Helpers
        private static uint processServerLevelFilter(
                Filter filter,
                List<Filter.Rule> serverObjectRules
            )
        {
            Debug.Assert(filter != null);
            Debug.Assert(serverObjectRules != null);
            Debug.Assert(filter.Type == FilterType.ServerLevel);
            uint numServerRulesProcessed = 0;

            // If similar rule does not exist, then add it to the list.
            foreach (Filter.Rule filterRule in filter.ServerLevelRules)
            {
                numServerRulesProcessed++;
                bool isSimilar = false;
                foreach (Filter.Rule srvrRule in serverObjectRules)
                {
                    isSimilar = srvrRule.IsSimilar(filterRule);
                    if (isSimilar) { break; }
                }
                if (!isSimilar) { serverObjectRules.Add(filterRule); }
            }
            return numServerRulesProcessed;
        }

        private static uint processDatabaseLevelFilter(
                Filter filter,
                List<Database> databases,
                Dictionary<string, Dictionary<int, List<Filter.Rule>>> databaseRules
            )
        {
            Debug.Assert(filter != null);
            Debug.Assert(databases != null);
            Debug.Assert(databaseRules != null);

            uint numDatabaseOjectsProcessed = 0;

            // For each database collect rules that apply to it.
            foreach (Database db in databases)
            {
                // Create/get database rules.
                bool isDbRulesNew = false;
                Dictionary<int, List<Filter.Rule>> dbRules = null;
                if (!databaseRules.TryGetValue(db.Name, out dbRules))
                {
                    dbRules = new Dictionary<int, List<Filter.Rule>>();
                    isDbRulesNew = true;
                }

                // If filter matches the database.
                if (filter.IsDatabaseMatch(db))
                {
                    // If similar rule does not exist, then add it to the list.
                    foreach (Filter.Rule filterRule in filter.DatabaseLevelRules)
                    {
                        numDatabaseOjectsProcessed++;
                        // Create/get object class rules.
                        bool isClassRulesNew = false;
                        List<Filter.Rule> classRules = null;
                        if (!dbRules.TryGetValue((int)filterRule.ObjectType, out classRules))
                        {
                            classRules = new List<Filter.Rule>();
                            isClassRulesNew = true;
                        }

                        // If there is no similar rule add it to the list.
                        bool isSimilar = false;
                        foreach (Filter.Rule classRule in classRules)
                        {
                            isSimilar = classRule.IsSimilar(filterRule);
                            if (isSimilar) { break; }
                        }
                        if (!isSimilar) { classRules.Add(filterRule); }

                        // If a new class list was created, then add it to the class dictionary.
                        if (isClassRulesNew && classRules.Count > 0)
                        {
                            dbRules.Add((int)filterRule.ObjectType, classRules);
                        }
                    }

                    // If a new db list was created, then add it to the dictionary.
                    if (isDbRulesNew)
                    {
                        databaseRules.Add(db.Name, dbRules);
                    }
                }
            }

            return numDatabaseOjectsProcessed;
        }

        private static void addRuleToDataRow(
                int snapshotid,
                int headerid,
                Filter.Rule rule,
                DataRow dr
            )
        {
            Debug.Assert(rule != null);
            Debug.Assert(dr != null);

            dr[ServerFilterRuleDataTable.ParamSnapshotid] = snapshotid;
            dr[ServerFilterRuleDataTable.ParamFilterruleheaderid] = headerid;
            dr[ServerFilterRuleDataTable.ParamFilterruleid] = rule.RuleId;
            dr[ServerFilterRuleDataTable.ParamScope] = rule.Scope;
            dr[ServerFilterRuleDataTable.ParamClass] = rule.ObjectType;
            dr[ServerFilterRuleDataTable.ParamMatchstring] = rule.MatchString;
            dr[ServerFilterRuleDataTable.ParamHashkey] = "";
        }

        #endregion

        // ------------- Methods -------------
        #region Methods
        public static bool GetFilterRules(
                string repositoryConnectionString,
                string targetInstance,
                out List<Filter> filterList
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(repositoryConnectionString));
            Debug.Assert(!string.IsNullOrEmpty(targetInstance));

            // Init returns.
            bool isOk = true;
            filterList = new List<Filter>();

            // Check inputs.
            if (string.IsNullOrEmpty(repositoryConnectionString) || string.IsNullOrEmpty(targetInstance))
            {
                logX.loggerX.Error("ERROR - invalid connection string or instance name");
                isOk = false;
            }

            // Query the repository for filter rules.
            if (isOk)
            {
                using (SqlConnection connection = new SqlConnection(repositoryConnectionString))
                {
                    try
                    {
                        // Open the connection.
                        connection.Open();

                        // Read filter from the repository.
                        SqlParameter param = new SqlParameter(ParamTargetInstance, targetInstance);
                        using (SqlDataReader filterRdr = Sql.SqlHelper.ExecuteReader(connection, null,
                                            CommandType.Text, QueryFilter, new SqlParameter[] { param }))
                        {
                            while (filterRdr.Read())
                            {
                                SqlInt32 filterruleheaderid = filterRdr.GetSqlInt32(FieldFilterruleheaderid);
                                SqlString rulename = filterRdr.GetSqlString(FieldRulename);
                                SqlString createdby = filterRdr.GetSqlString(FieldCreatedby);
                                SqlDateTime createdtm = filterRdr.GetSqlDateTime(FieldCreatedtm);
                                SqlString modifiedby = filterRdr.GetSqlString(FieldLastmodifiedby);
                                SqlDateTime modfiedtm = filterRdr.GetSqlDateTime(FieldLastmodifiedtm);

                                Filter filter = new Filter(filterruleheaderid, rulename, createdby, createdtm, modifiedby, modfiedtm);
                                filterList.Add(filter);
                            }
                        }

                        // Read the rules for each of the filters.
                        foreach (Filter filter in filterList)
                        {
                            param = new SqlParameter(ParamHeaderId, filter.HeaderId);
                            using (SqlDataReader ruleRdr = Sql.SqlHelper.ExecuteReader(connection, null,
                                            CommandType.Text, QueryRule, new SqlParameter[] { param }))
                            {
                                while (ruleRdr.Read())
                                {
                                    SqlInt32 ruleid = ruleRdr.GetSqlInt32(FieldRuleid);
                                    SqlInt32 ruleclass = ruleRdr.GetSqlInt32(FieldRuleclass);
                                    SqlString rulescope = ruleRdr.GetSqlString(FieldRulescope);
                                    SqlString rulematchstring = ruleRdr.GetSqlString(FieldRuleMatchstring);

                                    Filter.Rule rule = new Filter.Rule(ruleid, ruleclass, rulescope, rulematchstring);
                                    filter.AddRule(rule);
                                }
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        logX.loggerX.Error("ERROR - reading data collection filter rules raised an exception.", ex);
                        AppLog.WriteAppEventError(SQLsecureEvent.ExErrExceptionRaised, SQLsecureCat.DlFilterLoadCat,
                                                    "Load data collection filters", ex.Message);
                        filterList.Clear();
                        filterList = null;
                        isOk = false;
                    }
                }
            }

            return isOk;
        }

        public static bool SaveSnapshotFilterRules(
                string repositoryConnectionString,
                int snapshotid,
                List<Filter> filterList
            )
        {

            Debug.Assert(!string.IsNullOrEmpty(repositoryConnectionString));
            Debug.Assert(filterList != null && filterList.Count > 0);

            // Validate inputs.
            if (string.IsNullOrEmpty(repositoryConnectionString))
            {
                logX.loggerX.Error("ERROR - invalid connection string specified for saving filter rules to snapshot");
                return false;
            }

            // Empty filter list, return.
            if (filterList == null || filterList.Count == 0)
            {
                logX.loggerX.Info("INFO - no filters to save");
                return true;
            }

            // Save each filter header and rule to repository.
            bool isOk = true;
            Program.ImpersonationContext ic = Program.SetLocalImpersonationContext();
            using (SqlConnection connection = new SqlConnection(repositoryConnectionString))
            {
                try
                {
                    // Open the connection.
                    connection.Open();

                    // Use bulk copy to write to repository.
                    using (SqlBulkCopy bcp = new SqlBulkCopy(connection))
                    {
                        // Write all the filter headers to the repository.
                        bcp.DestinationTableName = ServerFilterRuleHeaderDataTable.RepositoryTable;
                        bcp.BulkCopyTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                        using (DataTable dataTable = ServerFilterRuleHeaderDataTable.Create())
                        {
                            foreach (Filter f in filterList)
                            {
                                //if (f.Name == Collector.Constants.SpecialMasterDatabase)
                                //{
                                //    continue;
                                //}
                                DataRow dr = dataTable.NewRow();
                                dr[ServerFilterRuleHeaderDataTable.ParamSnapshotid] = snapshotid;
                                dr[ServerFilterRuleHeaderDataTable.ParamFilterruleheaderid] = f.HeaderId;
                                dr[ServerFilterRuleHeaderDataTable.ParamRulename] = f.Name;
                                dr[ServerFilterRuleHeaderDataTable.ParamDescription] = "";
                                dr[ServerFilterRuleHeaderDataTable.ParamCreatedby] = f.CreatedBy;
                                dr[ServerFilterRuleHeaderDataTable.ParamCreatedtm] = f.CreatedOn;
                                dr[ServerFilterRuleHeaderDataTable.ParamLastmodifiedby] = f.LastModifiedBy;
                                dr[ServerFilterRuleHeaderDataTable.ParamLastmodifiedtm] = f.LastModifiedOn;
                                dr[ServerFilterRuleHeaderDataTable.ParamHashkey] = "";
                                dataTable.Rows.Add(dr);
                            }

                            // Write to repository.
                            bcp.WriteToServer(dataTable);
                        }

                        // Write all the filter rules to the repository.
                        bcp.DestinationTableName = ServerFilterRuleDataTable.RepositoryTable;
                        using (DataTable dataTable = ServerFilterRuleDataTable.Create())
                        {
                            foreach (Filter f in filterList)
                            {
                                //if (f.Name == Collector.Constants.SpecialMasterDatabase)
                                //{
                                //    continue;
                                //}
                                // Write server rules to the data table.
                                if (f.ServerLevelRules != null)
                                {
                                    foreach (Filter.Rule sr in f.ServerLevelRules)
                                    {
                                        DataRow dr = dataTable.NewRow();
                                        addRuleToDataRow(snapshotid, f.HeaderId.Value, sr, dr);
                                        dataTable.Rows.Add(dr);
                                    }
                                }
                                // Write db rules to the data table.
                                if (f.DatabaseRules != null)
                                {
                                    foreach (Filter.Rule dbr in f.DatabaseRules)
                                    {
                                        DataRow dr = dataTable.NewRow();
                                        addRuleToDataRow(snapshotid, f.HeaderId.Value, dbr, dr);
                                        dataTable.Rows.Add(dr);
                                    }
                                }
                                // Write db obj rules to the data table.
                                if (f.DatabaseLevelRules != null)
                                {
                                    foreach (Filter.Rule dblr in f.DatabaseLevelRules)
                                    {
                                        DataRow dr = dataTable.NewRow();
                                        addRuleToDataRow(snapshotid, f.HeaderId.Value, dblr, dr);
                                        dataTable.Rows.Add(dr);
                                    }
                                }
                            }

                            // Write to repository.
                            bcp.WriteToServer(dataTable);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    string strMessage = "Update snapshot filter tables";
                    logX.loggerX.Error("ERROR - " + strMessage, ex);
                    Sql.Database.CreateApplicationActivityEventInRepository(repositoryConnectionString,
                                                                            snapshotid,
                                                                            Collector.Constants.ActivityType_Error,
                                                                            Collector.Constants.ActivityEvent_Error,
                                                                            strMessage + ex.Message);
                    AppLog.WriteAppEventError(SQLsecureEvent.ExErrExceptionRaised, SQLsecureCat.DlDataLoadCat,
                        strMessage, ex.Message);

                    isOk = false;
                }
            }
            Program.RestoreImpersonationContext(ic);

            return isOk;
        }

        private static bool DoesExtendedStoredProceedureExistInFilter(List<Filter> filters, out SqlInt32 id)
        {
            bool found = false;
            id = 0;
            foreach (Filter filter in filters)
            {
                if (filter.Type == FilterType.DatabaseLevel)
                {
                    foreach (Filter.Rule filterRule in filter.DatabaseLevelRules)
                    {

                        if (filterRule.ObjectTypeEnum == SqlObjectType.ExtendedStoredProcedure)
                        {
                            found = true;
                            id = filterRule.RuleId;
                            break;
                        }
                    }
                }
                if (found) break;
            }
            return found;
        }


        public static bool OptimizeRules(
                List<Database> databases,
                List<Filter> filters,
                out List<Filter.Rule> serverObjectRules,
                out Dictionary<string, Dictionary<int, List<Filter.Rule>>> databaseRules
            )
        {
            // Init return.
            bool isOk = true;
            serverObjectRules = new List<Filter.Rule>();
            databaseRules = new Dictionary<string, Dictionary<int, List<Filter.Rule>>>();
            uint numDatabaseRulesProcessed = 0;
            uint numServerRulesProcessed = 0;
            SqlInt32 id = 0;
            //if (DoesExtendedStoredProceedureExistInFilter(filters, out id))
            //{
            //    Filter filter = new Filter(id, Collector.Constants.SpecialMasterDatabase, "", DateTime.Now, "", DateTime.Now);
            //    Filter.Rule rule = new Filter.Rule(1, (int)SqlObjectType.Database, "S", "master");
            //    filter.AddRule(rule);
            //    rule = new Filter.Rule(id, (int)SqlObjectType.ExtendedStoredProcedure, "S", "");
            //    filter.AddRule(rule);
            //    filters.Add(filter);
            //}
            // Process each filter in the filters list.
            foreach (Filter filter in filters)
            {
                switch (filter.Type)
                {
                    case FilterType.ServerLevel:
                        numServerRulesProcessed += processServerLevelFilter(filter, serverObjectRules);
                        break;
                    case FilterType.DatabaseLevel:
                        numDatabaseRulesProcessed += processDatabaseLevelFilter(filter, databases, databaseRules);
                        break;
                    case FilterType.Unknown:
                    default:
                        logX.loggerX.Warn("WARN - unknown filter type, hdr id: ", filter.HeaderId, ", name: ", filter.Name);
                        Debug.Assert(false);
                        break;
                }
            }
            logX.loggerX.Info("INFO - Optimize Filters: " + numServerRulesProcessed + " Server filters reduced to "
                + serverObjectRules.Count);
            int numDatabaseRules = 0;
            foreach (KeyValuePair<string, Dictionary<int, List<Sql.Filter.Rule>>> dbObjRules in databaseRules)
            {
                numDatabaseRules += dbObjRules.Value.Count;
            }
            logX.loggerX.Info("INFO - Optimize Filters: " + numDatabaseRulesProcessed + " Database filters reduced to "
                + numDatabaseRules);
            return isOk;
        }
        #endregion

        // ------------- SQL Queries -------------
        #region SQL Queries
        private const string QueryFilter =
                    @"SELECT 
                        filterruleheaderid, 
                        rulename, 
                        createdby, 
                        createdtm, 
                        lastmodifiedby, 
                        lastmodifiedtm
                      FROM SQLsecure.dbo.filterruleheader
                      WHERE connectionname = @targetinstance";
        private const string QueryRule =
                    @"SELECT 
                        filterruleid, 
                        class, 
                        scope, 
                        matchstring
                      FROM SQLsecure.dbo.filterrule
                      WHERE filterruleheaderid = @headerid";

        private const string ParamTargetInstance = "targetinstance";
        private const string ParamHeaderId = "headerid";

        private const int FieldFilterruleheaderid = 0;
        private const int FieldRulename = 1;
        private const int FieldCreatedby = 2;
        private const int FieldCreatedtm = 3;
        private const int FieldLastmodifiedby = 4;
        private const int FieldLastmodifiedtm = 5;

        private const int FieldRuleid = 0;
        private const int FieldRuleclass = 1;
        private const int FieldRulescope = 2;
        private const int FieldRuleMatchstring = 3;

        #endregion
    }


    /// <summary>
    /// Collection filter, contains multiple rules for
    /// selecting database objects.
    /// </summary>
    internal class Filter
    {
        private static LogX logX = new LogX("Idera.SQLsecure.Collector.Sql.Filter");
        // ------------- Rule Class -------------
        #region Rule Class
        /// <summary>
        /// Data collection filter rule class.
        /// </summary>
        internal class Rule
        {
            // ------------- Fields -------------
            #region Fields
            private SqlInt32 m_RuleId;
            private SqlInt32 m_ObjectType;
            private SqlString m_Scope;
            private SqlString m_MatchString;
            private SqlObjectType m_ObjectTypeEnum;
            private FilterScope m_ScopeEnum;
            private Utility.WildMatch m_WildMatch;
            #endregion

            // ------------- Helpers -------------
            #region Helpers
            private static SqlObjectType getObjectClass(SqlInt32 oclass)
            {
                return (SqlObjectType)oclass.Value;
            }
            private static FilterScope getScope(SqlString scope)
            {
                Debug.Assert(!string.IsNullOrEmpty(scope.Value));
                if (string.Compare(scope.Value, "A", true) == 0) { return FilterScope.Any; }
                else if (string.Compare(scope.Value, "S", true) == 0) { return FilterScope.System; }
                else if (string.Compare(scope.Value, "U", true) == 0) { return FilterScope.User; }
                else
                {
                    Debug.Assert(false);
                    return FilterScope.User;
                }
            }
            private static bool matchStringsSame(
                    string match1,
                    string match2
                )
            {
                // If both strings are null, return true.
                if (string.IsNullOrEmpty(match1) && string.IsNullOrEmpty(match2))
                {
                    return true;
                }
                // One of the strings is empty, return false.
                if (string.IsNullOrEmpty(match1) != string.IsNullOrEmpty(match2))
                {
                    return false;
                }
                // Case in-sensitive string compare.
                return string.Compare(match1, match2, true) == 0;
            }
            #endregion

            // ------------- Ctors -------------
            #region Ctors
            public Rule(
                    SqlInt32 ruleId,
                    SqlInt32 objectType,
                    SqlString scope,
                    SqlString matchString
                )
            {
                m_RuleId = ruleId;
                m_ObjectType = objectType;
                m_Scope = scope;
                m_MatchString = matchString;

                m_ObjectTypeEnum = getObjectClass(m_ObjectType.Value);
                m_ScopeEnum = getScope(m_Scope.Value);
                m_WildMatch = new Utility.WildMatch(m_MatchString.Value);
            }
            #endregion

            // ------------- Properties -------------
            #region Properties
            public SqlInt32 RuleId
            {
                get { return m_RuleId; }
            }
            public SqlInt32 ObjectType
            {
                get { return m_ObjectType; }
            }
            public SqlString Scope
            {
                get { return m_Scope; }
            }
            public SqlString MatchString
            {
                get { return m_MatchString; }
            }
            public SqlObjectType ObjectTypeEnum
            {
                get { return m_ObjectTypeEnum; }
            }
            public FilterScope ScopeEnum
            {
                get { return m_ScopeEnum; }
            }
            #endregion

            // ------------- Methods -------------
            #region Methods

            public bool IsSimilar(Rule rule)
            {
                // No rule, return false.
                if (rule == null) { return false; }

                // Compare object class
                if (rule.ObjectType == ObjectType)
                {
                    // If not database, stored procedure or table, then return true.
                    // The reason is that for those objects we can't specify scope and
                    // match string.
                    if (ObjectTypeEnum != SqlObjectType.Database
                        && ObjectTypeEnum != SqlObjectType.Table
                        && ObjectTypeEnum != SqlObjectType.StoredProcedure
                        && ObjectTypeEnum != SqlObjectType.View
                        && ObjectTypeEnum != SqlObjectType.Function)
                    {
                        return true;
                    }

                    // If this rules scope is bigger or the two scopes are the same.
                    if (ScopeEnum == FilterScope.Any || ScopeEnum == rule.ScopeEnum)
                    {
                        // If match strings are the same, return true.
                        if (matchStringsSame(m_WildMatch.MatchString, rule.m_WildMatch.MatchString))
                        {
                            return true;
                        }

                        // If match strings wildcards select objects already selected.
                        // -----------------------------------------------------------
                        for (int i = 0; i < rule.m_WildMatch.MatchString.Length && i < m_WildMatch.MatchString.Length; i++)
                        {
                            // So long as chars match keep looking for '*' at last char in string
                            // ------------------------------------------------------------------
                            if (m_WildMatch.MatchString[i] == '*' && i == m_WildMatch.MatchString.Length - 1)
                            {
                                return true;
                            }
                            if (m_WildMatch.MatchString[i] != rule.m_WildMatch.MatchString[i])
                            {
                                break;
                            }
                        }
                    }
                }

                return false;
            }

            public bool IsNameMatch(string name)
            {
                return m_WildMatch.Match(name);
            }
            #endregion
        }

        #endregion

        // ------------- Fields -------------
        #region Fields
        private SqlInt32 m_HeaderId;
        private SqlString m_Name;
        private SqlString m_CreatedBy;
        private SqlDateTime m_CreatedOn;
        private SqlString m_LastModifiedBy;
        private SqlDateTime m_LastModifiedOn;
        private List<Filter.Rule> m_SrvrLvlRules = new List<Rule>();
        private List<Filter.Rule> m_DbRules = new List<Rule>();
        private List<Filter.Rule> m_DbLvlRules = new List<Rule>();
        #endregion

        // ------------- Helpers -------------
        #region Helpers
        #endregion

        // ------------- Ctors -------------
        #region Ctors
        public Filter(
                SqlInt32 headerId,
                SqlString name,
                SqlString createdBy,
                SqlDateTime createdOn,
                SqlString modifiedBy,
                SqlDateTime modifiedOn
            )
        {
            m_HeaderId = headerId;
            m_Name = name;
            m_CreatedBy = createdBy;
            m_CreatedOn = createdOn;
            m_LastModifiedBy = modifiedBy;
            m_LastModifiedOn = modifiedOn;
        }
        #endregion

        // ------------- Properties -------------
        #region Properties
        public SqlInt32 HeaderId
        {
            get { return m_HeaderId; }
        }
        public SqlString Name
        {
            get { return m_Name; }
        }
        public SqlString CreatedBy
        {
            get { return m_CreatedBy; }
        }
        public SqlDateTime CreatedOn
        {
            get { return m_CreatedOn; }
        }
        public SqlString LastModifiedBy
        {
            get { return m_LastModifiedBy; }
        }
        public SqlDateTime LastModifiedOn
        {
            get { return m_LastModifiedOn; }
        }
        public FilterType Type
        {
            get
            {
                if (m_SrvrLvlRules.Count != 0)
                {
                    return FilterType.ServerLevel;
                }
                else if (m_DbRules.Count != 0 || m_DbLvlRules.Count != 0)
                {
                    return FilterType.DatabaseLevel;
                }
                else
                {
                    logX.loggerX.Warn("WARN - unknown filter type, hdr id: ", HeaderId.Value, ", name: ", Name.Value);
                    Debug.Assert(false);
                    return FilterType.Unknown;
                }
            }
        }

        public List<Filter.Rule> ServerLevelRules
        {
            get
            {
                if (Type != FilterType.ServerLevel) { return null; }
                else { return m_SrvrLvlRules; }
            }
        }

        public List<Filter.Rule> DatabaseRules
        {
            get
            {
                if (Type != FilterType.DatabaseLevel) { return null; }
                else { return m_DbRules; }
            }
        }

        public List<Filter.Rule> DatabaseLevelRules
        {
            get
            {
                if (Type != FilterType.DatabaseLevel) { return null; }
                else { return m_DbLvlRules; }
            }
        }

        #endregion

        // ------------- Methods -------------
        #region Methods
        public void AddRule(Rule rule)
        {
            if (rule != null)
            {
                switch (rule.ObjectTypeEnum)
                {
                    case SqlObjectType.Login:
                    case SqlObjectType.Endpoint:
                    case SqlObjectType.LinkedServer:
                        if (m_DbLvlRules.Count == 0)
                        {
                            m_SrvrLvlRules.Add(rule);
                        }
                        else
                        {
                            logX.loggerX.Warn("WARN - this rule contains db level & srvr level rules");
                            Debug.Assert(false);
                        }
                        break;
                    case SqlObjectType.Database:
                    case SqlObjectType.User:
                    case SqlObjectType.Role:
                    case SqlObjectType.Assembly:
                    case SqlObjectType.Certificate:
                    case SqlObjectType.FullTextCatalog:
                    case SqlObjectType.Key:
                    case SqlObjectType.Schema:
                    case SqlObjectType.UserDefinedDataType:
                    case SqlObjectType.XMLSchemaCollection:
                    case SqlObjectType.Table:
                    case SqlObjectType.StoredProcedure:
                    case SqlObjectType.ExtendedStoredProcedure:
                    case SqlObjectType.View:
                    case SqlObjectType.Function:
                    case SqlObjectType.Synonym:
                    case SqlObjectType.SequenceObject:
                        if (m_SrvrLvlRules.Count == 0)
                        {
                            if (rule.ObjectTypeEnum == SqlObjectType.Database)
                            {
                                m_DbRules.Add(rule);
                            }
                            else
                            {
                                m_DbLvlRules.Add(rule);
                            }
                        }
                        else
                        {
                            logX.loggerX.Warn("WARN - this rule contains srvr level & db level rules");
                            Debug.Assert(false);
                        }
                        break;
                    case SqlObjectType.Server: // No rule with server object should be created.
                    default:
                        logX.loggerX.Warn("WARN - rule class is unsupported, hdr id: ", HeaderId, ", rule id: ", rule.RuleId);
                        Debug.Assert(false);
                        break;
                }
            }
            else
            {
                logX.loggerX.Error("ERROR - rule object is null");
            }
        }

        public bool IsDatabaseMatch(Database db)
        {
            Debug.Assert(Type == FilterType.DatabaseLevel);
            bool isMatch = false;
            foreach (Rule dbRule in m_DbRules)
            {
                if (dbRule.ScopeEnum == FilterScope.Any
                    || (dbRule.ScopeEnum == FilterScope.System ? db.IsSystemDb : !db.IsSystemDb))
                {
                    isMatch = dbRule.IsNameMatch(db.Name);
                }
                if (isMatch) { break; }
            }
            return isMatch;
        }
        #endregion
    }


}
