/******************************************************************
 * Name: DatabaseObject.cs
 *
 * Description: Encapsulates a database object.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Diagnostics;
using Idera.SQLsecure.Core.Accounts;
using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.Collector.Utility;
namespace Idera.SQLsecure.Collector.Sql
{
	internal class ObjIdCollection
	{
		// ------------- Fields -------------
		#region Fields

		private Wintellect.PowerCollections.Set<ObjId> m_ObjIdSet = new Wintellect.PowerCollections.Set<ObjId>();

		#endregion

		// ------------- Helpers -------------
		#region Helpers
		#endregion

		// ------------- Ctors -------------
		#region Ctors

		public ObjIdCollection()
		{
		}

		#endregion

		// ------------- Properties -------------
		#region Properties

		public Wintellect.PowerCollections.Set<ObjId> ObjIdSet
		{
			get { return m_ObjIdSet; }
		}

		#endregion

		// ------------- Methods -------------
		#region Methods

		public bool IsInCollection(ObjId id)
		{
			ObjId founditem;
			return m_ObjIdSet.TryGetItem(id, out founditem);
		}

		public void Add(ObjId id)
		{
			m_ObjIdSet.Add(id);
		}
		#endregion
	}

	internal class ObjId
	{
		// ------------- Fields -------------
		#region Fields

		private int m_ClassId;
		private int m_ParentObjectId;
		private int m_ObjectId;

		#endregion

		// ------------- Ctors -------------
		#region Ctors

		public ObjId(
				int classid,
				int parentobjectid,
				int objectid
			)
		{
			m_ClassId = classid;
			m_ParentObjectId = parentobjectid;
			m_ObjectId = objectid;
		}

		#endregion

		// ------------- Properties -------------
		#region Properties

		public int ClassId
		{
			get { return m_ClassId; }
		}
		public int ParentObjectId
		{
			get { return m_ParentObjectId; }
		}
		public int ObjectId
		{
			get { return m_ObjectId; }
		}

		#endregion

		// ------------- Methods -------------
		#region Methods

		public override bool Equals(object obj)
		{
			if (obj == null) { return false; }
			if (this.GetType() != obj.GetType()) { return false; }
			ObjId other = (ObjId)obj;
			return other.m_ClassId == m_ClassId && other.m_ParentObjectId == m_ParentObjectId && other.m_ObjectId == m_ObjectId;
		}

		public override int GetHashCode()
		{
			return m_ClassId ^ (m_ParentObjectId ^ m_ObjectId);
		}

		public override string ToString()
		{
			StringBuilder bldr = new StringBuilder();
			bldr.Append("Classid: ");
			bldr.Append(m_ClassId);
			bldr.Append(", Parentobjectid: ");
			bldr.Append(m_ParentObjectId);
			bldr.Append(", Objectid: ");
			bldr.Append(m_ObjectId);
			return bldr.ToString();
		}

		#endregion
	}

	/// <summary>
	/// Database object class.
	/// </summary>
	internal static class DatabaseObject
	{
		private static LogX logX = new LogX("Idera.SQLsecure.Collector.Sql.DatabaseObject");
		private const int FieldType = 0;
		private const int FieldOwner = 1;
		private const int FieldSchemaid = 2;
		private const int FieldClassid = 3;
		private const int FieldParentobjectid = 4;
		private const int FieldObjectid = 5;
		private const int FieldName = 6;
		private const int FieldRunAtStartup = 7;
		private const int FieldIsEncytped = 8;
		private const int FieldUserdefined = 9;
		private const int FieldPermissionSet = 10;
		private const int FieldCreateDate = 11;
		private const int FieldModifyDate = 12;
        private const int FieldSignedCryptType = 13;    // SQLsecure 3.1 (Anshul Aggarwal) - New columns for new risk assessments
        private const int FieldIsRowSecurityEnabled = 14;
        private const int FieldObjectFQN = 15;

        private const int FieldAlwaysEncryptionType = 7;     // SQLsecure 3.1 (Anshul Aggarwal) - New columns for new risk assessments
        private const int FieldIsDataMasked = 8;
        private const int FieldColumnFQN = 9;

        // SQLsecure 3.1 (Anshul Aggarwal) - Queries to construct Fully-Qualified names for various objects for new risk assessments
        private const string FQN_OBJECT_QUERY_2K8 = @"QUOTENAME('{0}') + '.' + QUOTENAME('{1}') + '.' + QUOTENAME(OBJECT_SCHEMA_NAME({2}, {3})) + '.' + QUOTENAME({4})";
        private const string FQN_ASSEMBLY_QUERY_2K8 = @"QUOTENAME('{0}') + '.' + QUOTENAME('{1}') + '.' + QUOTENAME({2})";
        private const string FQN_COLUMN_QUERY_2K8 = @"QUOTENAME('{0}') + '.' + QUOTENAME('{1}') + '.' + QUOTENAME(OBJECT_SCHEMA_NAME({2}, {3})) + '.'" +
     @" + QUOTENAME(OBJECT_NAME({2}, {3})) + '.' + QUOTENAME(name)";

        private const string FQN_OBJECT_QUERY = @"CONCAT(QUOTENAME('{0}'), '.', QUOTENAME('{1}'), '.', QUOTENAME(OBJECT_SCHEMA_NAME({2}, {3})),'.',QUOTENAME({4}))";
        private const string FQN_ASSEMBLY_QUERY = @"CONCAT(QUOTENAME('{0}'), '.', QUOTENAME('{1}'),'.',QUOTENAME({2}))";
        private const string FQN_COLUMN_QUERY = @"CONCAT(QUOTENAME('{0}'), '.', QUOTENAME('{1}'), '.', QUOTENAME(OBJECT_SCHEMA_NAME({2}, {3})),'.'" +
     @",QUOTENAME(OBJECT_NAME({2}, {3})),'.', QUOTENAME(name))";

        private const string FQN_ADB_OBJECT_QUERY = @"CONCAT(QUOTENAME('{0}'), '.', QUOTENAME('{1}'), '.', QUOTENAME({2}),'.',QUOTENAME({3}))";
        private const string FQN_ADB_COLUMN_QUERY = @"CONCAT(QUOTENAME('{0}'), '.', QUOTENAME('{1}'), '.', QUOTENAME(OBJECT_SCHEMA_NAME({2})),'.'" +
       @",QUOTENAME(OBJECT_NAME({2})),'.', QUOTENAME(name))";

        // ------------- Database Objects -------------
        #region Database Objects

        private static bool isSupportedType(
				ServerVersion version,
				SqlObjectType type
			)
		{
			// If common types, return true.
			if (type == SqlObjectType.Table
				|| type == SqlObjectType.StoredProcedure
				|| type == SqlObjectType.ExtendedStoredProcedure
				|| type == SqlObjectType.View
				|| type == SqlObjectType.Function
                || type == SqlObjectType.Trigger)    // SQLsecure 3.1 (Anshul Aggarwal) - Collect Trigger objects for new risk assessment "Signed Objects"
            {
				return true;
			}

			// If SQL Server 2005 types, return true.
			if (version != ServerVersion.SQL2000)
			{
				if (type == SqlObjectType.Assembly
					|| type == SqlObjectType.Certificate
					|| type == SqlObjectType.FullTextCatalog
					|| type == SqlObjectType.Key
					|| type == SqlObjectType.UserDefinedDataType
					|| type == SqlObjectType.XMLSchemaCollection
					|| type == SqlObjectType.Synonym)
				{
					return true;
				}
			}
			if (version >= ServerVersion.SQL2012 && 
				type == SqlObjectType.SequenceObject)
			{
					return true;
			}


			return false;
		}

		// CleanseQueryString is used to protect against SQLInjection
		// in any text entered by the user.
		// return values
		//      If queryText string is found to be possibly unsafe then,
		//          returns false;
		//      If queryText is OK or can be modified to be safe then,
		//          return true and queryText contains safe query string
		// -------------------------------------------------------------
		private static bool CleanseQueryString(ref string queryText)
		{
			bool bSafe = true;
			string[] rejectChars = { ";", "'", "/*", "--", "Xp_", "XP_", "xP_", "xp_" };

			foreach (string str in rejectChars)
			{
				if (queryText.Contains(str))
				{
					logX.loggerX.Error(@"Match string contained an illegal charater: " + queryText);
					logX.loggerX.Error(@"Match strings can't contain:  ; ' /* -- Xp_");
					bSafe = false;
					break;
				}
			}

			if (bSafe)
			{
				// GP ToDo: check for wildcards as literals
				// ----------------------------------------
				queryText = queryText.Replace("*", "%");
				queryText = queryText.Replace("?", "_");
			}

			return bSafe;
		}


		private static string createScopeQueryText(ServerVersion version, SqlObjectType type, Filter.Rule rule)
		{
			string strTypes = null;
			string strQueryText = "";

			// Build up text based on type of object
			// -------------------------------------
			switch (type)
			{
				case SqlObjectType.Table:
					if (version == ServerVersion.SQL2000)
					{
						strTypes = @" (type = 'U' or type = 'S' or type = 'IT')";
					}
					else
					{
						strTypes = @" a.schema_id = b.schema_id and (type = 'U' or type = 'S' or type = 'IT')";
					}
					break;
				case SqlObjectType.StoredProcedure:
					if (version == ServerVersion.SQL2000)
					{
						strTypes = @" (type = 'P' or type = 'RF')";
					}
					else
					{
						strTypes = @" a.schema_id = b.schema_id and (type = 'P' or type = 'RF')";
					}
					break;
				case SqlObjectType.View:
					if (version == ServerVersion.SQL2000)
					{
						strTypes = @" (type = 'V')";
					}
					else
					{
						strTypes = @" a.schema_id = b.schema_id and (type = 'V')";
					}
					break;
				case SqlObjectType.Function:
					if (version == ServerVersion.SQL2000)
					{
						strTypes = @" (type IN ('FN', 'IF', 'TF'))";
					}
					else
					{
						strTypes = @" a.schema_id = b.schema_id and (type IN ('AF', 'FN', 'FS', 'FT', 'IF', 'TF'))";
					}
					break;
                case SqlObjectType.Trigger:   // SQLsecure 3.1 (Anshul Aggarwal) - Collect Trigger objects for new risk assessment "Signed Objects"
                    strTypes = @" a.schema_id = b.schema_id and (type IN ('TR'))";
                    break;
                default:
					strTypes = null;
					break;
			}

			if (!string.IsNullOrEmpty(strTypes))
			{
				string strScopeModifer = "";
				string strSQLVersionDependant = "1";

				// Build up Text based on Scope of Object, this is SQL Version specific
				// --------------------------------------------------------------------
				if (rule.ScopeEnum != FilterScope.Any)
				{
					if (version == ServerVersion.SQL2000)
					{
						strSQLVersionDependant = @"
							CASE WHEN (OBJECTPROPERTY(a.id, N'IsMSShipped')=1) THEN 1 
								 WHEN (OBJECTPROPERTY(a.id, N'IsSystemTable')=1) THEN 1 
								 ELSE 0 
							END";
					}
					else
					{
						strSQLVersionDependant = @"
								 case 
									when is_ms_shipped = 1 then 1
									when (
										select 
										  major_id 
										from 
											sys.extended_properties 
										where 
										   major_id = object_id and 
											minor_id = 0 and 
											class = 1 and 
										   name = N'microsoft_database_tools_support') 
										is not null then 1
									else 0
								end";
					}

					// Set Scoop modifier
					// = empty string in scoop is System or Any
					// = "NOT" if User
					// ----------------------------------------
					if (rule.ScopeEnum == FilterScope.User)
					{
						strScopeModifer = " NOT ";
					}
				}

				// The WHERE clause contains parent_object_id = 0 because it 
				// was picking up some message queue system tables.   This is only
				// needed when looking for system tables.
				// ---------------------------------------------------------------
				if (version != ServerVersion.SQL2000 &&
					 rule.ScopeEnum == FilterScope.System &&
					 type == SqlObjectType.Table)
				{
					strTypes += @" AND parent_object_id = 0";
				}

				strQueryText = strTypes + @" and " + strScopeModifer + @"( cast ( " + strSQLVersionDependant +
						@" AS bit) = 1) ";
			}

			return strQueryText;
		}

		private static string createQuery(
				ServerVersion version,
				Database database,
				SqlObjectType type,
				Filter.Rule rule,
                ServerType serverType,
                string targetServerName

            )
		{

			Debug.Assert(version != ServerVersion.Unsupported);
			Debug.Assert(database != null);
			Debug.Assert(type == SqlObjectType.Table
						 || type == SqlObjectType.StoredProcedure
						 || type == SqlObjectType.ExtendedStoredProcedure
						 || type == SqlObjectType.View
						 || type == SqlObjectType.Function
						 || type == SqlObjectType.Synonym
						 || type == SqlObjectType.Assembly
						 || type == SqlObjectType.Certificate
						 || type == SqlObjectType.FullTextCatalog
						 || type == SqlObjectType.Key
						 || type == SqlObjectType.UserDefinedDataType
						 || type == SqlObjectType.XMLSchemaCollection
						 || type == SqlObjectType.SequenceObject
                         || type == SqlObjectType.Trigger);   // SQLsecure 3.1 (Anshul Aggarwal) - Collect Trigger objects for new risk assessment "Signed Objects"
            Debug.Assert(rule != null);

			string query = null;
			string ruleFilterMatchString = null;
			string LIKEClause = "";

			// Create query based on type and version.
			switch (type)
			{
				case SqlObjectType.Assembly:
					if (version == ServerVersion.SQL2000)
					{
						Debug.Assert(false);
					}
					else
					{
						if (version == ServerVersion.SQL2005)
						{
							query = @"SELECT
									type = 'iASM', 
									owner = a.principal_id,
									schemaid = NULL,  
									classid = 5,  
									parentobjectid = 0,  
									objectid = a.assembly_id,
									a.name,
									runatstartup = null,
									isencypted = null,
									userdefined = 'N' ,
									permission_set=CAST(a.permission_set AS INT) 
									, createdate=a.create_date,
									modifydate=a.modify_date, signedcrypttype = null, isrowsecurityenabled = cast(0 as bit), FQN = NULL "
                                    + @"FROM " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + @".sys.assemblies a ";
						}
						else  // 2008 and above
						{
							query = @"SELECT
									type = 'iASM', 
									owner = a.principal_id,
									schemaid = NULL,  
									classid = 5,  
									parentobjectid = 0,  
									objectid = a.assembly_id,
									a.name,
									runatstartup = null,
									isencypted = null,
									userdefined = CASE a.is_user_defined WHEN 0 THEN 'N' WHEN 1 THEN 'Y' END,
									permission_set=CAST(a.permission_set AS INT) 
									, createdate=a.create_date,
									modifydate=a.modify_date,
                                    signedcrypttype = c.crypt_type, isrowsecurityenabled = cast(0 as bit), " +
                                    @"FQN = " + GetFullyQualifidAssemblyQuery(version, database, targetServerName, "a.name") + @" "
                                    + @"FROM " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + @".sys.assemblies a " +
                                    "LEFT JOIN sys.crypt_properties c ON a.assembly_id = c.major_id AND c.class_desc = 'ASSEMBLY' ";
						}
					}
					break;

				case SqlObjectType.Certificate:
					if (version == ServerVersion.SQL2000)
					{
						Debug.Assert(false);
					}
					else
					{
						query = @"SELECT
									type = 'iCERT', 
									owner = a.principal_id,
									schemaid = null,  
									classid = 25,  
									parentobjectid = 0,  
									objectid = a.certificate_id,                                   
									a.name,
									runatstartup = null,
									isencypted = null,
									userdefined = null ,
									permission_set=null 
									, createdate=null,
									modifydate=null, signedcrypttype = null, isrowsecurityenabled = cast(0 as bit), FQN = null "
                                + @"FROM " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + @".sys.certificates a ";
					}
					break;

				case SqlObjectType.FullTextCatalog:
					if (version == ServerVersion.SQL2000)
					{
						Debug.Assert(false);
					}
					else
					{
						query = @"SELECT
									type = 'iFTXT', 
									owner = a.principal_id,
									schemaid = null,  
									classid = 23,  
									parentobjectid = 0,  
									objectid = a.fulltext_catalog_id,  	                                
									a.name,
									runatstartup = null,
									isencypted = null,
									userdefined = null ,
									 permission_set=null 
									, createdate=null,
									modifydate=null, signedcrypttype = null, isrowsecurityenabled = cast(0 as bit), FQN = null "
                                + @"FROM " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + @".sys.fulltext_catalogs a";
					}
					break;

				case SqlObjectType.Key:
					if (version == ServerVersion.SQL2000)
					{
						Debug.Assert(false);
					}
					else
					{
						query = @"SELECT
									type = 'iAK', 
									owner = a.principal_id,
									schemaid = NULL,  
									classid = 26,  
									parentobjectid = 0,  
									objectid = a.asymmetric_key_id,                                       
									a.name,
									runatstartup = null,
									isencypted = null,
									userdefined = null,
									permission_set=null ,
									createdate=null,
									modifydate=null, signedcrypttype = null, isrowsecurityenabled = cast(0 as bit), FQN = null "
                                + @"FROM " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + @".sys.asymmetric_keys a "
								+ @"UNION ALL "
								+
                                @"SELECT
									type = 'iSK', 
									owner = c.principal_id,
									schemaid = NULL,  
									classid = 24,  
									parentobjectid = 0,  
									objectid = c.symmetric_key_id,  
									c.name,
									runatstartup = null,
									isencypted = null,
									userdefined = null,
									permission_set=null ,
									createdate=c.create_date,
									modifydate=c.modify_date, signedcrypttype = null, isrowsecurityenabled = cast(0 as bit), FQN = null "
                                + @"FROM " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + @".sys.symmetric_keys c ";

					}
					break;

				case SqlObjectType.UserDefinedDataType:
					if (version == ServerVersion.SQL2000)
					{
						Debug.Assert(false);
					}
					else
					{
						query = @"SELECT
									type = 'iUDT', 
									owner = b.principal_id,
									schemaid = a.schema_id,  
									classid = 6,  
									parentobjectid = 0,  
									objectid = a.user_type_id,  
									a.name,
									runatstartup = null,
									isencypted = null,
									userdefined = null ,
									permission_set=null,
									createdate=null,
									modifydate=null, signedcrypttype = null, isrowsecurityenabled = cast(0 as bit), FQN = null "
                                + @"FROM " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + @".sys.types a, "
								+ Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + ".sys.schemas b "
								+ "WHERE a.schema_id = b.schema_id";
					}
					break;

				case SqlObjectType.XMLSchemaCollection:
					if (version == ServerVersion.SQL2000)
					{
						Debug.Assert(false);
					}
					else
					{
						query = @"SELECT
									type = 'iXMLS', 
									owner = b.principal_id,
									schemaid = a.schema_id,  
									classid = 10,  
									parentobjectid = 0,  
									objectid = a.xml_collection_id,  
									a.name,
									runatstartup = null,
									isencypted = null,
									userdefined = null,
									permission_set=null ,
									createdate=null,
									modifydate=null, signedcrypttype = null, isrowsecurityenabled = cast(0 as bit), FQN = null "
                                + @"FROM " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + @".sys.xml_schema_collections a, "
								+ Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + ".sys.schemas b "
								+ "WHERE a.schema_id = b.schema_id";
					}
					break;

				case SqlObjectType.Table:
				case SqlObjectType.StoredProcedure:
				case SqlObjectType.Function:
				case SqlObjectType.View:
                case SqlObjectType.Trigger:   // SQLsecure 3.1 (Anshul Aggarwal) - Collect Trigger objects for new risk assessment "Signed Objects"

                    // Table and Stored Procedures are filtered, by scope and name
                    // If the name match string is empty or null then the LIKE clause
                    // is not needed.
                    // -----------------------------------------------------------                    
                    ruleFilterMatchString = (string)rule.MatchString;
					if (!string.IsNullOrEmpty(ruleFilterMatchString))
					{
						if (CleanseQueryString(ref ruleFilterMatchString))
						{
							if (version == ServerVersion.SQL2000)
							{
								LIKEClause = " AND name LIKE '" + ruleFilterMatchString + "'";
							}
							else
							{
								LIKEClause = " AND a.name LIKE '" + ruleFilterMatchString + "'";
							}
						}
					}
					string strScopeText = createScopeQueryText(version, type, rule);

                    if (serverType == ServerType.AzureSQLDatabase)
                    {
                        //Barkha Khatri - metric Id 22 
                        //changing isencrypted logic - using sys.sql_modules definition column for it
                        query = @" SELECT 
									a.type, 
									owner = b.principal_id,         
									schemaid = a.schema_id, 
									classid = 1, 
									parentobjectid = a.parent_object_id, 
									objectid = a.object_id,                                     
									a.name,
									runatstartup = CASE WHEN ObjectProperty(a.object_id, 'ExecIsStartup') = 1 THEN 'Y' ELSE 'N' END,
									isencypted =CASE WHEN c.definition is null THEN 'Y' ELSE 'N' END,
									userdefined = case 
													when is_ms_shipped = 1 then 'N'
													when (
														select 
														  major_id 
														from 
															sys.extended_properties 
														where 
														   major_id = a.object_id and 
															minor_id = 0 and 
															class = 1 and 
														   name = N'microsoft_database_tools_support') 
														is not null then 'N'
													else 'Y'
												  end ,
								permission_set=null, 
								createdate=null, 
								modifydate=null, signedcrypttype = null, isrowsecurityenabled = cast(0 as bit), " +
                                    @"FQN = " + GetADBFullyQualifidObjectQuery(database, targetServerName, "b.name", "a.name") + @" " +
                                    "FROM  " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + ".sys.all_objects a "
                                  + "INNER JOIN " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + ".sys.schemas b ON a.schema_id = b.schema_id "
                                  + "LEFT JOIN  " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + ".sys.sql_modules c ON (a.object_id = c.object_id )"
                                  + "WHERE " + strScopeText + LIKEClause;
                    }
                    else if (version == ServerVersion.SQL2000) // 2000
					{
						query = @" USE " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name)
								+ @" SELECT 
									type = a.xtype, 
									owner = CAST (a.uid AS int),  
									schemaid = null,  
									classid = 1,  
									parentobjectid = 0,  
									objectid = a.id,                                      
									name = a.name,
									runatstartup = CASE WHEN ObjectProperty(a.id, 'ExecIsStartup') = 1 THEN 'Y' ELSE 'N' END,
									isencypted = CASE WHEN isnull(b.encrypted,0) = 0 THEN 'N' ELSE 'Y' END,
									userdefined = CASE WHEN (OBJECTPROPERTY(a.id, N'IsMSShipped')=1) THEN 'N' WHEN (OBJECTPROPERTY(a.id, N'IsSystemTable')=1) THEN 'N' ELSE 'Y' END ,"
								+ "permission_set=null, "
								+ "createdate=null, "
								+ "modifydate=null, signedcrypttype = null, isrowsecurityenabled = cast(0 as bit), FQN = null "
                                + "FROM " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + ".dbo.sysobjects a "
								+ "LEFT JOIN  " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + ".dbo.syscomments b ON (a.id = b.id and b.colid=1)"
								+ "WHERE " + strScopeText + LIKEClause;
					}
                    else if (version >= ServerVersion.SQL2016) // 2016
                    {
                        query = @" USE " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name)
                                + @" SELECT 
									a.type, 
									owner = b.principal_id,         
									schemaid = a.schema_id, 
									classid = 1, 
									parentobjectid = a.parent_object_id, 
									objectid = a.object_id,                                     
									a.name,
									runatstartup = CASE WHEN ObjectProperty(a.object_id, 'ExecIsStartup') = 1 THEN 'Y' ELSE 'N' END,
									isencypted = CASE WHEN isnull(c.encrypted,0) = 0 THEN 'N' ELSE 'Y' END,
									userdefined = case 
													when a.is_ms_shipped = 1 then 'N'
													when (
														select 
														  major_id 
														from 
															sys.extended_properties 
														where 
														   major_id = a.object_id and 
															minor_id = 0 and 
															class = 1 and 
														   name = N'microsoft_database_tools_support') 
														is not null then 'N'
													else 'Y'
												  end ,
								permission_set=null, 
								createdate=null, 
								modifydate=null, signedcrypttype = d.crypt_type, isrowsecurityenabled = cast(isnull(spo.is_enabled, 0) as bit), " +
                                    @"FQN = " + GetFullyQualifidObjectQuery(version, database, targetServerName, "a.object_id", "a.name") + @" " +
									@"FROM  " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) +  @".sys.all_objects a INNER JOIN " + 
                                    Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + @".sys.schemas b ON a.schema_id = b.schema_id LEFT JOIN  " + 
                                    Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + @".sys.syscomments c ON (a.object_id = c.id and c.colid=1) LEFT JOIN  " + 
                                    Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + @".sys.crypt_properties AS d ON (a.object_id = d.major_id) AND d.class_desc = 'OBJECT_OR_COLUMN' LEFT JOIN  " +
                                    Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + @".sys.security_predicates AS spd ON (a.object_id = spd.target_object_id) LEFT JOIN " +
                                    Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + @".sys.security_policies spo ON (spd.object_id = spo.object_id) WHERE " + 
                                    strScopeText.Replace("type", "a.type") + LIKEClause;
                    }
                    else if(version >= ServerVersion.SQL2008) // 2008 2012
                    {
                        query = @" USE " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name)
                                + @" SELECT 
									a.type, 
									owner = b.principal_id,         
									schemaid = a.schema_id, 
									classid = 1, 
									parentobjectid = a.parent_object_id, 
									objectid = a.object_id,                                     
									a.name,
									runatstartup = CASE WHEN ObjectProperty(a.object_id, 'ExecIsStartup') = 1 THEN 'Y' ELSE 'N' END,
									isencypted = CASE WHEN isnull(c.encrypted,0) = 0 THEN 'N' ELSE 'Y' END,
									userdefined = case 
													when is_ms_shipped = 1 then 'N'
													when (
														select 
														  major_id 
														from 
															sys.extended_properties 
														where 
														   major_id = object_id and 
															minor_id = 0 and 
															class = 1 and 
														   name = N'microsoft_database_tools_support') 
														is not null then 'N'
													else 'Y'
												  end ,
								permission_set=null, 
								createdate=null, 
								modifydate=null, signedcrypttype = d.crypt_type, isrowsecurityenabled = cast(0 as bit), " +  
                                    @"FQN = " + GetFullyQualifidObjectQuery(version, database, targetServerName, "a.object_id", "a.name") +  @" " +
									@"FROM  " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + 
                                    @".sys.all_objects a INNER JOIN " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + 
                                    @".sys.schemas b ON a.schema_id = b.schema_id LEFT JOIN  " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) +
                                    @".sys.syscomments c ON (a.object_id = c.id and c.colid=1) LEFT JOIN  " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + 
                                    @".sys.crypt_properties AS d ON a.object_id = d.major_id AND d.class_desc = 'OBJECT_OR_COLUMN' WHERE " + strScopeText + LIKEClause;
                    }
					else // 2005
					{
                        query = @" USE " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name)
                            + @" SELECT 
									a.type, 
									owner = b.principal_id,         
									schemaid = a.schema_id, 
									classid = 1, 
									parentobjectid = a.parent_object_id, 
									objectid = a.object_id,                                     
									a.name,
									runatstartup = CASE WHEN ObjectProperty(a.object_id, 'ExecIsStartup') = 1 THEN 'Y' ELSE 'N' END,
									isencypted = CASE WHEN isnull(c.encrypted,0) = 0 THEN 'N' ELSE 'Y' END,
									userdefined = case 
													when is_ms_shipped = 1 then 'N'
													when (
														select 
														  major_id 
														from 
															sys.extended_properties 
														where 
														   major_id = object_id and 
															minor_id = 0 and 
															class = 1 and 
														   name = N'microsoft_database_tools_support') 
														is not null then 'N'
													else 'Y'
												  end ,
								permission_set=null, 
								createdate=null, 
								modifydate=null, signedcrypttype = null, isrowsecurityenabled = cast(0 as bit), FQN = null 
									FROM  " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + ".sys.all_objects a "
                                + "INNER JOIN " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + ".sys.schemas b ON a.schema_id = b.schema_id "
                                + "LEFT JOIN  " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + ".sys.syscomments c ON (a.object_id = c.id and c.colid=1)"
                                + "WHERE " + strScopeText + LIKEClause;
                    }
					break;


				case SqlObjectType.ExtendedStoredProcedure:
					if (string.Compare(database.Name, "master", true) == 0)
					{
						if (version == ServerVersion.SQL2000)
						{
							query = @"SELECT 
									type = a.xtype, 
									owner = CAST (a.uid AS int),  
									schemaid = null,  
									classid = 1,  
									parentobjectid = 0,  
									objectid = a.id,
									a.name,
									runatstartup = null,
									isencypted = null,
									userdefined = CASE WHEN c.category = 0 THEN 'Y' ELSE 'N' END ,"
							   + "permission_set=null, "
								+ "createdate=a.crdate, "
								+ "modifydate=a.refdate, signedcrypttype = null, isrowsecurityenabled = cast(0 as bit), FQN = null "
                                    + @"FROM " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + ".dbo.sysobjects a, "
									+ Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + ".dbo.sysobjects c "
									+ @"WHERE a.xtype = 'X' and a.id = c.id";
						}
						else
						{
							query = @"SELECT 
									a.type, 
									owner = b.principal_id, 
									schemaid = a.schema_id, 
									classid = 1, 
									parentobjectid = a.parent_object_id, 
									objectid = a.object_id, 
									a.name,
									runatstartup = null,
									isencypted = null,
									userdefined = CASE WHEN c.category = 0 THEN 'Y' ELSE 'N' END ,"
								+ "permission_set=null, "
								+ "createdate=a.create_date, "
								+ "modifydate=a.modify_date, signedcrypttype = null, isrowsecurityenabled = cast(0 as bit), FQN = null "
                                    + "FROM " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + ".sys.all_objects a, "
									+ Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + ".sys.schemas b, "
									+ Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + ".sys.sysobjects c "
									+ "WHERE a.type = 'X' and a.schema_id = b.schema_id and a.object_id = c.id";
						}
					}
					break;
				//                case SqlObjectType.View:
				//                    if (version == ServerVersion.SQL2000)
				//                    {
				//                        query = @"SELECT 
				//                                    type = xtype, 
				//                                    owner = CAST (uid AS int),  
				//                                    schemaid = null,  
				//                                    classid = 1,  
				//                                    parentobjectid = 0,  
				//                                    objectid = id,  
				//                                    name "
				//                                + @"FROM " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + ".dbo.sysobjects "
				//                                + @"WHERE type = 'V'";
				//                    }
				//                    else
				//                    {
				//                        query = @"SELECT 
				//                                    a.type, 
				//                                    owner = b.principal_id, 
				//                                    schemaid = a.schema_id, 
				//                                    classid = 1, 
				//                                    parentobjectid = a.parent_object_id, 
				//                                    objectid = a.object_id, 
				//                                    a.name "
				//                                + @"FROM " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + ".sys.all_objects a, "
				//                                + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + ".sys.schemas b "
				//                                + @"WHERE a.type = 'V'";
				//                    }
				//                    break;

				//                case SqlObjectType.Function:
				//                    if (version == ServerVersion.SQL2000)
				//                    {
				//                        query = @"SELECT 
				//                                    type = xtype, 
				//                                    owner = CAST (uid AS int),  
				//                                    schemaid = null,  
				//                                    classid = 1,  
				//                                    parentobjectid = 0,  
				//                                    objectid = id,  
				//                                    name "
				//                                + @"FROM " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + ".dbo.sysobjects "
				//                                + @"WHERE type IN ('FN', 'IF', 'TF')";
				//                    }
				//                    else
				//                    {
				//                        query = @"SELECT 
				//                                    a.type, 
				//                                    owner = b.principal_id, 
				//                                    schemaid = a.schema_id, 
				//                                    classid = 1, 
				//                                    parentobjectid = a.parent_object_id, 
				//                                    objectid = a.object_id, 
				//                                    a.name "
				//                                + @"FROM " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + ".sys.all_objects a, "
				//                                + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + ".sys.schemas b "
				//                                + @"WHERE a.type IN ('AF', 'FN', 'FS', 'FT', 'IF', 'TF')";
				//                    }
				//                    break;

				case SqlObjectType.Synonym:
					if (version == ServerVersion.SQL2000)
					{
						Debug.Assert(false);
					}
					else
					{
						query = @"SELECT 
									a.type, 
									owner = b.principal_id, 
									schemaid = a.schema_id, 
									classid = 1, 
									parentobjectid = a.parent_object_id, 
									objectid = a.object_id, 
									a.name,
									runatstartup = null,
									isencypted = null,
									userdefined = null ,"
							+ "permission_set=null, "
								+ "createdate=a.create_date, "
								+ "modifydate=a.modify_date, signedcrypttype = null, isrowsecurityenabled = cast(0 as bit), FQN = null "
                                + @"FROM " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + ".sys.all_objects a, "
								+ Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + ".sys.schemas b "
								+ @"WHERE a.type = 'SN' and a.schema_id = b.schema_id";
					}
					break;
				case SqlObjectType.SequenceObject :
					if (version < ServerVersion.SQL2012)
					{
						Debug.Assert(false);
					}
					else
					{
						query =string.Format(@"SELECT 
								 ob.type,
								 owner = ob.principal_id,
								 schemaid = ob.schema_id,
								 classid = 1,
								 parentobjectid = ob.parent_object_id,
								 objectid = ob.object_id,
								 ob.name,
								 runatstartup = null,
								 isencypted = null,
								 userdefined = null,
								 permission_set = null,
								 createdate = ob.create_date,
								 modifydate = ob.modify_date, signedcrypttype = null, isrowsecurityenabled = cast(0 as bit), FQN = null "
                                + @" FROM  {0}.sys.sequences ob "
								+ @" WHERE ob.type = 'SO' ", Sql.SqlHelper.CreateSafeDatabaseName(database.Name));
					}
					break;

				default:
					logX.loggerX.Error("ERROR - query cannot be created for this database object type ", type.ToString());
					Debug.Assert(false);
					break;
			}

			return query;
		}

		public static bool Process(
				ServerVersion version,
				string targetConnection,
				string repositoryConnection,
				SqlObjectType objType,
				List<Filter.Rule> rules,
				int snapshotid,
				Database database,
                ServerType serverType,
                string targetServerName,
				ref Dictionary<Sql.SqlObjectType, Dictionary<MetricMeasureType, uint>> metricsData
			)
		{
			Debug.Assert(version != ServerVersion.Unsupported);
			Debug.Assert(!string.IsNullOrEmpty(targetConnection));
			Debug.Assert(!string.IsNullOrEmpty(repositoryConnection));
			Debug.Assert(rules != null);
			Debug.Assert(database != null);
			Stopwatch sw = new Stopwatch();
			uint numObjectsProcessed = 0;
			sw.Start();
			// If the type is not supported, bail out.
			if (!isSupportedType(version, objType))
			{
				//logX.loggerX.Info("INFO - ", objType.ToString(), " is not supported for ", version.ToString());
				return true;
			}

			// Process the database object.
			bool isOk = true;
			targetConnection = Sql.SqlHelper.AppendDatabaseToConnectionString(targetConnection, database.Name);
			ObjIdCollection objIdCollection = new ObjIdCollection();
			Program.ImpersonationContext wi = Program.SetLocalImpersonationContext();
			using (SqlConnection target = new SqlConnection(targetConnection),
					repository = new SqlConnection(repositoryConnection))
			{
				try
				{
					// Open repository and target connections.
					repository.Open();
					Program.SetTargetSQLServerImpersonationContext();
					target.Open();

					// Use bulk copy object to write to repository.
					using (SqlBulkCopy bcp = new SqlBulkCopy(repository))
					{
						// Set the destination table.
						bcp.DestinationTableName = DatabaseObjectDataTable.RepositoryTable;
						bcp.BulkCopyTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
						// Create the datatable to write to the repository.
						using (DataTable dataTable = DatabaseObjectDataTable.Create())
						{
							// Process each rule to collect the table objects.
							foreach (Filter.Rule rule in rules)
							{
								// Create the query based on the rule.
								string query = createQuery(version, database, objType, rule,serverType, targetServerName);
								if (query != null)
								{
									Debug.Assert(!string.IsNullOrEmpty(query));

									// Query to get the table objects.
									using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(target, null,
														CommandType.Text, query, null))
									{
										while (rdr.Read())
										{
											// Retrieve the object information.
											SqlString type = rdr.GetSqlString(FieldType);
											SqlInt32 owner = rdr.GetSqlInt32(FieldOwner);
											SqlInt32 schemaid = rdr.GetSqlInt32(FieldSchemaid);
											SqlInt32 classid = rdr.GetSqlInt32(FieldClassid);
											SqlInt32 parentobjectid = rdr.GetSqlInt32(FieldParentobjectid);
											SqlInt32 objectid = rdr.GetSqlInt32(FieldObjectid);
											SqlString name = rdr.GetSqlString(FieldName);
											SqlString runatstartup = rdr.IsDBNull(FieldRunAtStartup)
																		? null
																		: rdr.GetSqlString(FieldRunAtStartup);
											SqlString isencypted = rdr.IsDBNull(FieldIsEncytped)
																		? null
																		: rdr.GetSqlString(FieldIsEncytped);
											SqlString userdefined = rdr.IsDBNull(FieldUserdefined)
																		? null
																		: rdr.GetSqlString(FieldUserdefined);

											SqlInt32 permissionSet = rdr.GetSqlInt32(FieldPermissionSet);

											SqlDateTime createDate = rdr.IsDBNull(FieldCreateDate) ? SqlDateTime.Null
																   : rdr.GetDateTime(FieldCreateDate);
											SqlDateTime modifyDate = rdr.IsDBNull(FieldModifyDate) ? SqlDateTime.Null
																   : rdr.GetDateTime(FieldModifyDate);
                                            SqlString signedCryptType = rdr.IsDBNull(FieldSignedCryptType)  // SQLsecure 3.1 (Anshul Aggarwal) - New columns for new risk assessments.
                                                                        ? null
                                                                        : rdr.GetSqlString(FieldSignedCryptType);
                                            SqlBoolean isRowSecurityEnabled = rdr.GetBoolean(FieldIsRowSecurityEnabled);
                                            SqlString fqn = rdr.IsDBNull(FieldObjectFQN)  // SQLsecure 3.1 (Anshul Aggarwal) - New columns for new risk assessments.
                                                                       ? null
                                                                       : rdr.GetSqlString(FieldObjectFQN);

											// If symmetric_keysthe object was not processed, then add to the
											// list of objects seen and process it.
											ObjId objId = new ObjId(classid.Value, parentobjectid.Value, objectid.Value);
											if (!objIdCollection.IsInCollection(objId))
											{
												numObjectsProcessed++;
												// Add to object id collection.
												objIdCollection.Add(objId);

												// Update the datatable.
												DataRow dr = dataTable.NewRow();
												dr[DatabaseObjectDataTable.ParamSnapshotid] = snapshotid;
												dr[DatabaseObjectDataTable.ParamType] = type;
												dr[DatabaseObjectDataTable.ParamOwner] = owner;
												dr[DatabaseObjectDataTable.ParamSchemaid] = schemaid;
												dr[DatabaseObjectDataTable.ParamClassid] = classid;
												dr[DatabaseObjectDataTable.ParamDbid] = database.DbId;
												dr[DatabaseObjectDataTable.ParamParentobjectid] = parentobjectid;
												dr[DatabaseObjectDataTable.ParamObjectid] = objectid;
												dr[DatabaseObjectDataTable.ParamName] = name;
												dr[DatabaseObjectDataTable.ParamHashkey] = "";
												dr[DatabaseObjectDataTable.ParamRunAtStartup] = runatstartup;
												dr[DatabaseObjectDataTable.ParamIsEncypted] = isencypted;
												dr[DatabaseObjectDataTable.ParamUserDefined] = userdefined;
												dr[DatabaseObjectDataTable.ParamPermissionSet] = permissionSet;
												dr[DatabaseObjectDataTable.ParamCreateDate] = createDate;
												dr[DatabaseObjectDataTable.ParamModifyDate] = modifyDate;
                                                dr[DatabaseObjectDataTable.ParamSignedCryptType] = signedCryptType; // SQLsecure 3.1 (Anshul Aggarwal) - New columns for new risk assessments.
                                                dr[DatabaseObjectDataTable.ParamIsRowSecurityEnabled] = isRowSecurityEnabled;
                                                dr[DatabaseObjectDataTable.ParamIsDataMasked] = false; // SQLsecure 3.1 (Anshul Aggarwal) - Only columns support data masking.
                                                dr[DatabaseObjectDataTable.ParamFQN] = fqn;

                                                dataTable.Rows.Add(dr);

												// Write to repository if exceeds threshold.
												if (dataTable.Rows.Count > Constants.RowBatchSize)
												{
													try
													{
														bcp.WriteToServer(dataTable);
														dataTable.Clear();
													}
													catch (SqlException ex)
													{
														string strMessage = "Writing to Repository " + objType.ToString() + "s ";
														logX.loggerX.Error("ERROR - " + strMessage, ex);
														throw ex;
													}
												}
											}
										}

                                        // Write any items still in the data table.
                                        if (dataTable.Rows.Count > 0)
                                        {
                                            try
                                            {
                                                bcp.WriteToServer(dataTable);
                                                dataTable.Clear();
                                            }
                                            catch (SqlException ex)
                                            {
                                                string strMessage = string.Format("Writing to Repository {0}s ", objType.ToString());
                                                logX.loggerX.Error("ERROR - " + strMessage, ex);
                                                throw ;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (isOk&&objType == SqlObjectType.Key)
                    {
                        try
                        {
                            EncryptionKeys.Process(version, targetConnection, repositoryConnection, snapshotid,
                                              database.DbId, database.Name,serverType);
                        }
                        catch (SqlException ex)
                        {
                            string strMessage = string.Format("Writing to Repository {0}s ", objType);
                            logX.loggerX.Error("ERROR - " + strMessage, ex);
                            throw ;
                        }
                    }

                }
                catch (SqlException ex)
                {
                    string strMessage = string.Format("Processing {0}s", objType.ToString());
                    logX.loggerX.Error(string.Format("ERROR - {0}", strMessage), ex);
                    Sql.Database.CreateApplicationActivityEventInRepository(repositoryConnection,
                                                                            snapshotid,
                                                                            Collector.Constants.ActivityType_Error,
                                                                            Collector.Constants.ActivityEvent_Error,
                                                                            strMessage + ex.Message);
                    AppLog.WriteAppEventError(SQLsecureEvent.ExErrExceptionRaised, SQLsecureCat.DlDataLoadCat,
                        " SQL Server = " + new SqlConnectionStringBuilder(targetConnection).DataSource +
                        strMessage, ex.Message);
                    isOk = false;
                }
                finally
                {
                    Program.RestoreImpersonationContext(wi);
                }
            }
            sw.Stop();

			// Process column information for table, etc.
			if (isOk)
			{
				if (objType == SqlObjectType.Table
					|| objType == SqlObjectType.View
					|| objType == SqlObjectType.Function)
				{
					if (!processColumns(version, targetConnection, repositoryConnection, snapshotid, database, objIdCollection,
                        targetServerName, serverType, ref metricsData))
					{
						logX.loggerX.Error("ERROR - error encountered in processing ", objType.ToString(), " columns");
						isOk = false;
					}
				}
			}

			// Load object permissions.
			if (isOk)
			{
				if (!DatabaseObjectPermission.Process(version, targetConnection, repositoryConnection, snapshotid, database, objIdCollection))
				{
					logX.loggerX.Error("ERROR - error encountered in processing ", objType.ToString(), " permissions");
					isOk = false;
				}
			}
            //Process Certificates
            if (isOk && objType == SqlObjectType.Certificate)
            {
                if (!Certificate.Process(targetConnection, repositoryConnection, snapshotid, database, ref metricsData))
                {
                    logX.loggerX.Error("ERROR - error encountered in processing ", objType.ToString(), " data");
                    isOk = false;
                };
            }


            // See if Object is already in Metrics Dictionary
            // ----------------------------------------------
            Dictionary<MetricMeasureType, uint> de;
			uint oldMetricCount = 0;
			uint oldMetricTime = 0;
			if (metricsData.TryGetValue(objType, out de))
			{
				de.TryGetValue(MetricMeasureType.Count, out oldMetricCount);
				de.TryGetValue(MetricMeasureType.Time, out oldMetricTime);
			}
			else
			{
				de = new Dictionary<MetricMeasureType, uint>();
			}
			de[MetricMeasureType.Count] = oldMetricCount + numObjectsProcessed;
			de[MetricMeasureType.Time] = (uint)sw.ElapsedMilliseconds + oldMetricTime;
			metricsData[objType] = de;
			sw.Reset();

			return isOk;
		}

		#endregion

		// ------------- Columns -------------
		#region Columns

		private static string createColumnQuery(
				ServerVersion version,
				Database database,
				ObjId objid,
                ServerType serverType,
                string targetServerName
			)
		{
			Debug.Assert(version != ServerVersion.Unsupported);
			Debug.Assert(database != null);
			Debug.Assert(objid != null);

			string query = null;
            
            if (serverType == ServerType.AzureSQLDatabase)   // Azure SQL database
            {
                // SQLSECU-1622 Secure 3.1 : 'Always Encryption' fails for Azure DB.
                query = @"SELECT 
							type = 'iCO', 
							owner = null, 
							schemaid = null, 
							classid = 1,
							parentobjectid = " + objid.ObjectId.ToString() + @", "
                          + @"objectid = column_id, 
							name, alwaysencryptiontype = encryption_type, isdatamasked = cast(isnull(is_masked,0) as bit), "
                       + @"FQN = " + GetFullyQualifidColumnQuery(version, serverType, database, targetServerName, objid.ObjectId.ToString()) + @" "
                      + @"FROM " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + ".sys.columns "
                      + @"WHERE object_id = " + objid.ObjectId.ToString();
            }
			else if (version == ServerVersion.SQL2000) // 2000
			{
				// For table valued function this query will return columns and parameters.
				// We need to get rid of the parameters, otherwise we will have wrong information
				// and also violate the table constraint in the repository.
				query = @"SELECT 
							type = 'iCO', 
							owner = null, 
							schemaid = null, 
							classid = 1, 
							parentobjectid = " + objid.ObjectId.ToString() + @", "
							+ @"objectid = CAST(colid AS int), 
							name, alwaysencryptiontype = null, isdatamasked = cast(0 as bit), FQN = null "
                        + @"FROM " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + @".dbo.syscolumns "
						+ @"WHERE SUBSTRING(name,1,1) != '@' AND id=" + objid.ObjectId.ToString();
			}
            else if (version >= ServerVersion.SQL2016) // 2016 and above
            {
                query = @"SELECT 
							type = 'iCO', 
							owner = null, 
							schemaid = null, 
							classid = 1,
							parentobjectid = " + objid.ObjectId.ToString() + @", "
                            + @"objectid = column_id, 
							name, alwaysencryptiontype = encryption_type, isdatamasked = cast(isnull(is_masked,0) as bit), "
                         + @"FQN = " + GetFullyQualifidColumnQuery(version, serverType, database, targetServerName, objid.ObjectId.ToString()) + @" "
                        + @"FROM " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + ".sys.columns "
                        + @"WHERE object_id = " + objid.ObjectId.ToString();

            }
            else if (version >= ServerVersion.SQL2008)   // 2008 and above
            {
                query = @"SELECT 
							type = 'iCO', 
							owner = null, 
							schemaid = null, 
							classid = 1,
							parentobjectid = " + objid.ObjectId.ToString() + @", "
                            + @"objectid = column_id, 
							name, alwaysencryptiontype = null, isdatamasked = cast(0 as bit), "
                            + @"FQN = " + GetFullyQualifidColumnQuery(version, serverType, database, targetServerName, objid.ObjectId.ToString()) + @" "
                        + @"FROM " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + ".sys.columns "
                        + @"WHERE object_id = " + objid.ObjectId.ToString();
            }
			else
			{
				query = @"SELECT 
							type = 'iCO', 
							owner = null, 
							schemaid = null, 
							classid = 1,
							parentobjectid = " + objid.ObjectId.ToString() + @", "
							+ @"objectid = column_id, 
							name, alwaysencryptiontype = null, isdatamasked = cast(0 as bit), FQN = null "
                        + @"FROM " + Sql.SqlHelper.CreateSafeDatabaseName(database.Name) + ".sys.columns "
						+ @"WHERE object_id = " + objid.ObjectId.ToString();
			}

			return query;
		}


		private static bool processColumns(
				ServerVersion version,
				string targetConnection,
				string repositoryConnection,
				int snapshotid,
				Database database,
				ObjIdCollection objIdCollection,
                string targetServerName,
                ServerType serverType,
				ref Dictionary<Sql.SqlObjectType, Dictionary<MetricMeasureType, uint>> metricsData
			)
		{
			Debug.Assert(version != ServerVersion.Unsupported);
			Debug.Assert(!string.IsNullOrEmpty(targetConnection));
			Debug.Assert(!string.IsNullOrEmpty(repositoryConnection));
			Debug.Assert(database != null);
			Debug.Assert(objIdCollection != null);
			uint numColumnsProcessed = 0;
			Stopwatch sw = new Stopwatch();
			sw.Start();
			Program.ImpersonationContext wi = Program.SetLocalImpersonationContext();
			bool isOk = true;
			using (SqlConnection target = new SqlConnection(targetConnection),
					repository = new SqlConnection(repositoryConnection))
			{
				try
				{
					// Open repository and target connections.
					repository.Open();
					Program.SetTargetSQLServerImpersonationContext();
					target.Open();

					// Use bulk copy object to write to repository.
					using (SqlBulkCopy bcp = new SqlBulkCopy(repository))
					{
						// Set the destination table.
						bcp.DestinationTableName = DatabaseObjectDataTable.RepositoryTable;
						bcp.BulkCopyTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
						// Create the datatable to write to the repository.
						using (DataTable dataTable = DatabaseObjectDataTable.Create())
						{
							// Process each object id in the obj id collection.
							foreach (ObjId objId in objIdCollection.ObjIdSet)
							{
								// Create the query based on the object.
								string query = createColumnQuery(version, database, objId, serverType, targetServerName);
								Debug.Assert(!string.IsNullOrEmpty(query));

								// Query to get the column objects.
								using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(target, null,
													CommandType.Text, query, null))
								{
									while (rdr.Read())
									{
										// Retrieve the object information.
										SqlString type = rdr.GetSqlString(FieldType);
										SqlInt32 owner = rdr.GetSqlInt32(FieldOwner);
										SqlInt32 schemaid = rdr.GetSqlInt32(FieldSchemaid);
										SqlInt32 classid = rdr.GetSqlInt32(FieldClassid);
										SqlInt32 parentobjectid = rdr.GetSqlInt32(FieldParentobjectid);
										SqlInt32 objectid = rdr.GetSqlInt32(FieldObjectid);
										SqlString name = rdr.GetSqlString(FieldName);
                                        SqlInt32 alwaysEncryptionType = rdr.GetSqlInt32(FieldAlwaysEncryptionType);  // SQLsecure 3.1 (Anshul Aggarwal) - New columns for new risk assessments.
                                        SqlBoolean isDataMasked = rdr.GetBoolean(FieldIsDataMasked);
                                        SqlString objectFQN = rdr.GetSqlString(FieldColumnFQN);

                                        // Update the datatable.
                                        DataRow dr = dataTable.NewRow();
										dr[DatabaseObjectDataTable.ParamSnapshotid] = snapshotid;
										dr[DatabaseObjectDataTable.ParamType] = type;
										dr[DatabaseObjectDataTable.ParamOwner] = owner;
										dr[DatabaseObjectDataTable.ParamSchemaid] = schemaid;
										dr[DatabaseObjectDataTable.ParamClassid] = classid;
										dr[DatabaseObjectDataTable.ParamDbid] = database.DbId;
										dr[DatabaseObjectDataTable.ParamParentobjectid] = parentobjectid;
										dr[DatabaseObjectDataTable.ParamObjectid] = objectid;
										dr[DatabaseObjectDataTable.ParamName] = name;
										dr[DatabaseObjectDataTable.ParamHashkey] = "";
                                        dr[DatabaseObjectDataTable.ParamAlwaysEncryptionType] = alwaysEncryptionType;
                                        dr[DatabaseObjectDataTable.ParamIsDataMasked] = isDataMasked;
                                        dr[DatabaseObjectDataTable.ParamIsRowSecurityEnabled] = false;  // SQLSecure 3.1 (Anshul Aggarwal) - Columns don't have row level security feature.
                                        dr[DatabaseObjectDataTable.ParamFQN] = objectFQN;

                                        dataTable.Rows.Add(dr);

										numColumnsProcessed++;

										// Write to repository if exceeds threshold.
										if (dataTable.Rows.Count > Constants.RowBatchSize)
										{
											try
											{
												bcp.WriteToServer(dataTable);
												dataTable.Clear();
											}
											catch (SqlException ex)
											{
												string strMessage = "Writing columns to Repository ";
												logX.loggerX.Error("ERROR - " + strMessage, ex);
												throw ex;
											}
										}
									}

									// Write any items still in the data table.
									if (dataTable.Rows.Count > 0)
									{
										try
										{
											bcp.WriteToServer(dataTable);
											dataTable.Clear();
										}
										catch (SqlException ex)
										{
											string strMessage = "Writing columns to Repository ";
											logX.loggerX.Error("ERROR - " + strMessage, ex);
											throw ex;
										}
									}
								}
							}
						}
					}
				}
				catch (SqlException ex)
				{
					string strMessage = "Processing columns";
					logX.loggerX.Error("ERROR - " + strMessage, ex);
					Sql.Database.CreateApplicationActivityEventInRepository(repositoryConnection,
																			snapshotid,
																			Collector.Constants.ActivityType_Error,
																			Collector.Constants.ActivityEvent_Error,
																			strMessage + ex.Message);
					AppLog.WriteAppEventError(SQLsecureEvent.ExErrExceptionRaised, SQLsecureCat.DlDataLoadCat,
						" SQL Server = " + new SqlConnectionStringBuilder(targetConnection).DataSource +
						strMessage, ex.Message);
					isOk = false;
				}
				finally
				{
					Program.RestoreImpersonationContext(wi);
				}
			}


			// See if Object is already in Metrics Dictionary
			// ----------------------------------------------
			sw.Stop();
			Dictionary<MetricMeasureType, uint> de;
			uint oldMetricCount = 0;
			uint oldMetricTime = 0;
			if (metricsData.TryGetValue(SqlObjectType.Column, out de))
			{
				de.TryGetValue(MetricMeasureType.Count, out oldMetricCount);
				de.TryGetValue(MetricMeasureType.Time, out oldMetricTime);
			}
			else
			{
				de = new Dictionary<MetricMeasureType, uint>();
			}
			de[MetricMeasureType.Count] = oldMetricCount + numColumnsProcessed;
			de[MetricMeasureType.Time] = (uint)sw.ElapsedMilliseconds + oldMetricTime;
			metricsData[SqlObjectType.Column] = de;

			return isOk;
		}


        /// <summary>
        /// SQLsecure 3.1 (Anshul Aggarwal) - Returns query to fetch full qualified object name.
        /// </summary>
        private static string GetADBFullyQualifidObjectQuery(Database database, string serverName, string schemaNameParam, string objectNameParam)
        {
            return string.Format(FQN_ADB_OBJECT_QUERY, serverName, database.Name, schemaNameParam, objectNameParam);
        }

        /// <summary>
        /// SQLsecure 3.1 (Anshul Aggarwal) - Returns query to fetch full qualified object name.
        /// </summary>
        private static string GetFullyQualifidObjectQuery(ServerVersion version, Database database, string serverName, string objectIdParam, string objectNameParam)
        {
            return string.Format(version >= ServerVersion.SQL2012 ? FQN_OBJECT_QUERY : FQN_OBJECT_QUERY_2K8, serverName, database.Name, objectIdParam, database.DbId, objectNameParam);
        }

        /// <summary>
        /// SQLsecure 3.1 (Anshul Aggarwal) - Returns query to fetch full qualified object name.
        /// </summary>
        private static string GetFullyQualifidAssemblyQuery(ServerVersion version, Database database, string serverName, string objectNameParam)
        {
            return string.Format(version >= ServerVersion.SQL2012 ? FQN_ASSEMBLY_QUERY : FQN_ASSEMBLY_QUERY_2K8, serverName, database.Name, objectNameParam);
        }

        /// <summary>
        /// SQLsecure 3.1 (Anshul Aggarwal) - Returns query to fetch full qualified column name.
        /// </summary>
        private static string GetFullyQualifidColumnQuery(ServerVersion version, ServerType serverType, Database database, string serverName, string objectId)
        {
            return serverType == ServerType.AzureSQLDatabase ?
                string.Format(FQN_ADB_COLUMN_QUERY, serverName, database.Name, objectId) :
                string.Format(version >= ServerVersion.SQL2012 ? FQN_COLUMN_QUERY : FQN_COLUMN_QUERY_2K8, serverName, database.Name, objectId, database.DbId);
        }

        #endregion
    }

}
