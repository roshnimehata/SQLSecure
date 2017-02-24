/******************************************************************
 * Name: SqlTypes.cs
 *
 * Description: SQL Server related basic types, enums and constants.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlTypes;

namespace Idera.SQLsecure.UI.Console.Sql
{
    public enum ServerVersion
    {
        SQL2000,
        SQL2005,
        SQL2008,
        SQL2008R2,
        SQL2012,
        SQL2014,
        SQL2016,
        Unsupported
    }
    public struct VersionName
    {
        public const string SQL2000 = @"SQL Server 2000";
        public const string SQL2005 = @"SQL Server 2005";
        public const string SQL2008 = @"SQL Server 2008";
        public const string SQL2008R2 = @"SQL Server 2008 R2";
        public const string SQL2012 = @"SQL Server 2012";
        public const string SQL2014 = @"SQL Server 2014";
        public const string SQL2016 = @"SQL Server 2016";
        public const string Unsupported = @"Unknown version";
    }
    public class ServicePack
    {
        public class SQL2000
        {
            public static string[] Builds = new string[] { @"194", @"384", @"534", @"760", @"2039" };
            public static string[] BuildNames = new string[] { string.Empty, @"SP1", @"SP2", @"SP3", @"SP4" };
            //public const string RTM = @"194";       // included for reference
            //public const string SP1 = @"384";
            //public const string SP2 = @"534";     // repaired SP2, the original was 532
            //public const string SP3 = @"760";
            //public const string SP3a = @"760";    // we don't distinguish between 3 & 3a
            //public const string SP4 = @"2039";
        }
        public class SQL2005
        {
            public static string[] Builds = new string[] { @"1399", @"2047", @"3042", @"3043", @"4035", @"5000" };
            public static string[] BuildNames = new string[] { string.Empty, @"SP1", @"SP2", @"SP2", @"SP3", @"SP4" };
            //public const string RTM = @"1399";      // included for reference
            //public const string SP1 = @"2047";
            //public const string SP2_CTP = @"3027", @"3033";
            //public const string SP2 = @"3042", @"3043";   // fixed as 3043 because of problem in 3042, but 3042 was also later replaced and is valid too
            //public const string SP3 = @"4035";
        }
        public class SQL2008
        {
            public static string[] Builds = new string[] { @"1600", @"2531", @"4000", @"5500" };
            public static string[] BuildNames = new string[] { string.Empty, @"SP1", @"SP2", @"SP3" };
            //public const string Beta_CTP = @"1019", @"1049", @"1300";     // removed, but kept for reference
            //public const string RTM = @"1600";      // included for reference
        }
        public class SQL2008R2
        {
            public static string[] Builds = new string[] { @"1600", @"2500" };
            public static string[] BuildNames = new string[] { string.Empty, @"SP1" };
            //public const string RTM = @"1600";      // included for reference
        }
        public class SQL2012
        {
            public static string[] Builds = new string[] {     @"2100" };
            public static string[] BuildNames = new string[] { string.Empty };
            //public const string Beta_CTP = @"1103", @"1440", @"1750";     // removed, but kept for reference
            //public const string RTM = @"2100";      // included for reference
        }
        public class SQL2014
        {
            public static string[] Builds = new string[] { @"2000" };
            public static string[] BuildNames = new string[] { string.Empty };
        }
        public class SQL2016
        {
            public static string[] Builds = new string[] { @"700" };
            public static string[] BuildNames = new string[] { "CTP3" };
        }
    }

    public enum RuleObjectType : byte
    {
        Server = 0,
        Login = 1,
        Endpoint = 2,
        Database = 20,
        User = 21,
        Role = 22,
        Assembly = 26,
        Certificate = 27,
        FullTextCatalog = 28,
        Key = 29,
        Schema = 30,
        UserDefinedDataType = 31,
        XMLSchemaCollection = 32,
        Table = 41,
        StoredProcedure = 42,
        ExtendedStoredProcedure = 43,
        View = 44,
        Function = 45,
        Synonym = 46,
        SequenceObject = 48,
        LinkedServer = 50,
        Unknown = 0xFF
    }

    public enum RuleScope
    {
        [Description("System and User")]
        All,
        System,
        User,
        Unknown
    }

    public static class LoginType
    {
        internal const string SqlLogin = "S";
        internal const string WindowsLogin = "W";
        internal const string WindowsUser = "U";
        internal const string WindowsGroup = "G";
        internal const string AzureADAccount = "EX";    // SQLsecure 3.1 (Anshul Aggarwal) - Add support for Azure AD Users or Groups
        internal const string AzureADUser = "E";   
        internal const string AzureADGroup = "X";   
        
        internal const string SqlLoginText = "Login";
        internal const string WindowsUserText = "User";
        internal const string WindowsGroupText = "Group";
    }

    public static class ServerPrincipalTypes
    {
        internal const string SqlLogin = "S";
        internal const string WindowsUser = "U";
        internal const string WindowsGroup = "G";
        internal const string ServerRole = "R";
        internal const string Certificate = "C";
        internal const string AsymmetricKey = "K";
        //SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
        internal const string AzureADUSer = "E";
        internal const string AzureADGroup = "X";

        internal const string SqlLoginText = "SQL Login";
        internal const string WindowsUserText = "Windows User";
        internal const string WindowsGroupText = "Windows Group";
        internal const string ServerRoleText = "Server Role";
        internal const string CertificateText = "Certificate Mapped Login";
        internal const string AsymmetricKeyText = "Asymmetric Key Mapped Login";
        //SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
        internal const string AzureADUSerText = "Azure AD User";
        internal const string AzureADGroupText = "Azure AD Group";
    }

    public static class DatabasePrincipalTypes
    {
        internal const string SqlLogin = "S";
        internal const string WindowsUser = "U";
        internal const string WindowsGroup = "G";
        internal const string ApplicationRole = "A";
        internal const string DatabaseRole = "R";
        internal const string Certificate = "C";
        internal const string AsymmetricKey = "K";
        //SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
        internal const string AzureADUser = "E";
        internal const string AzureADGroup = "X";

        internal const string SqlLoginText = "SQL Login";
        internal const string WindowsUserText = "Windows User";
        internal const string WindowsGroupText = "Windows Group";
        internal const string ApplicationRoleText = "Application Role";
        internal const string DatabaseRoleText = "Database Role";
        internal const string CertificateText = "Certificate Mapped User";
        internal const string AsymmetricKeyText = "Asymmetric Key Mapped User";
        //SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
        internal const string AzureADUsertext = "Azure AD USer";
        internal const string AzureADGrouptext = "Azure AD Group";
    }

    public static class ServerLoginTypes
    {
        internal const string SqlLogin = "S";       //Same as LoginType
        internal const string WindowsUser = "U";    //Same as LoginType
        internal const string WindowsGroup = "Group";
        internal const string LocalGroup = "LocalGroup";
        internal const string GlobalGroup = "GlobalGroup";
        internal const string UniversalGroup = "UniversalGroup";
        internal const string DistributionGroup = "DistributionGroup";
        internal const string WellknownGroup = "WellknownGroup";
        internal const string User = "User";
        //SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
        internal const string AzureADUser = "E";
        internal const string AzureADGroup = "X";

        internal const string SqlLoginText = "SQL Login";
        internal const string WindowsUserText = "Windows User";
        internal const string WindowsGroupText = "Windows Group";
        internal const string LocalGroupText = "Local Group";
        internal const string GlobalGroupText = "Global Group";
        internal const string UniversalGroupText = "Universal Group";
        internal const string DistributionGroupText = "Distribution Group";
        internal const string WellknownGroupText = "Well-known Group";
        internal const string UserText = "Windows User";
        //SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
        internal const string AzureADUserText = "Azure AD user";
        internal const string AzureADGrouptext = "Azure AD group";
    }

    public static class ServerAccessTypes
    {
        internal const string Direct = "Y";
        internal const string Indirect = "N";

        internal const string DirectText = "SQL Login";
        internal const string IndirectText = "via group";
    }

    public class LoginStates
    {
        // Values for Windows Account State verified against a domain
        public const string Good = @"G";
        public const string Suspect = @"S";

        public const string GoodText = @"";
        public const string SuspectText = @"Suspect";
    }

    public static class ObjectType
    {
        #region Types

        // Enum for object types.
        public enum TypeEnum
        {
            Snapshot = 0,
            Server,
            ServerSecurity,
            Logins,
            WindowsUserLogin,
            WindowsGroupLogin,  //5
            SqlLogin,
            ServerRoles,
            ServerRole,
            ServerObjects,
            Endpoints,          //10
            Endpoint,
            Databases,
            Database,
            DatabaseSecurity,
            Users,              //15
            User,
            DatabaseRoles,
            DatabaseRole,
            ApplicationRole,
            Schemas,            //20
            Schema,
            Keys,
            Key,
            KeySymmetric,
            KeyAsymmetric,      //25
            Certificates,
            Certificate,
            Tables,
            Table,
            TableSystem,        //30
            Views,
            View,
            Synonyms,
            Synonym,
            StoredProcedures,   //35
            StoredProcedure,
            StoredProcedureCLR,
            Functions,
            Function,
            FunctionAggregate,  //40
            FunctionScalar,
            FunctionScalarCLR,
            FunctionTableValuedCLR,
            FunctionInlineTableValued,
            FunctionTableValued,    //45
            ExtendedStoredProcedures,
            ExtendedStoredProcedure,
            Assemblies,
            Assembly,
            UserDefinedDataTypes,   //50
            UserDefinedDataType,
            XMLSchemaCollections,
            XMLSchemaCollection,
            FullTextCatalogs,
            FullTextCatalog,        //55
            Column,
            Environment,
            FileSystem,
            File,
            Registry,               //60
            RegistryKey,
            Services,
            Service,
            Unknown,
            SequenceObjects,
            AvailabilityGroups,
            AvailabilityGroup,
            AvailabilityGroupReplica,
            SequenceObject,
            LinkedServer,
            //SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
            AzureADUser,
            AzureADGroup
        }

        // TypeEnum/Object type string name map element.
        private class TypeStringElement
        {
            private string m_TypeStr;
            private TypeEnum m_Type;

            public TypeStringElement(
                    string typeStr,
                    TypeEnum type
                )
            {
                m_TypeStr = typeStr;
                m_Type = type;
            }

            public bool IsTypeStrMatch(string type)
            {
                if (string.IsNullOrEmpty(type)) { return false; }
                return string.Compare(m_TypeStr, type, true) == 0;
            }

            public TypeEnum TypeEnum
            {
                get { return m_Type; }
            }
        }

        #endregion

        #region Fields

        private static string[] m_TypeString = new string[] { 
            "Snapshot",
            "SQL Server",
            "Security",
            "Logins",
            "Windows User",
            "Windows Group",
            "SQL Login",
            "Server Roles",
            "Server Role",
            "Server Objects",
            "Endpoints",
            "Endpoint",
            "Databases",
            "Database",
            "Security",
            "Users",
            "User",
            "Roles",
            "Database Role",
            "Application Role",
            "Schemas",
            "Schema",
            "Keys",
            "Key",
            "Key - Symmetric",
            "Key - Asymmetric",
            "Certificates",
            "Certificate",
            "Tables",
            "Table",
            "System Table",
            "Views",
            "View",
            "Synonyms",
            "Synonym",
            "Stored Procedures",
            "Stored Procedure",
            "Stored Procedure (CLR)",
            "Functions",
            "Function",
            "Function - Aggregate (CLR)",
            "Function - Scalar",
            "Function - Scalar (CLR)",
            "Function - Table-valued (CLR)",
            "Function - Inline Table-valued",
            "Function - Table-valued",
            "Extended Stored Procedures",
            "Extended Stored Procedure",
            "Assemblies",
            "Assembly",
            "User-defined Data Types",
            "User-defined Data Type",
            "XML Schema Collections",
            "XML Schema Collection",
            "Full Text Catalogs",
            "Full Text Catalog",
            "Column",
            "Environment",
            "FileSystem",
            "File",
            "Registry",
            "Registry Key",
            "Services",
            "Service",
            "Unknown",
            "Sequence Objects",
            "Always On Availability Groups",
            "Always On Availability Group",
            "Availability Group Replica",
            "Sequence Object",
            "LinkedServer",
            //Start-SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
            "AzureADUser",
            "AzureADGroup"
        };

        private static TypeStringElement[] m_TypeStringMap = new TypeStringElement[] {
            new TypeStringElement("Server",TypeEnum.Server),
            new TypeStringElement("Server Role",TypeEnum.ServerRole),
            new TypeStringElement("Login",TypeEnum.SqlLogin),
            new TypeStringElement("Endpoint",TypeEnum.Endpoint),
            new TypeStringElement("iSRV",TypeEnum.Server),
            new TypeStringElement("iLOGN",TypeEnum.SqlLogin),
            new TypeStringElement("iENDP",TypeEnum.Endpoint),
            new TypeStringElement("AF",TypeEnum.FunctionAggregate),
            new TypeStringElement("DB",TypeEnum.Database),
            new TypeStringElement("FN",TypeEnum.FunctionScalar),
            new TypeStringElement("FS",TypeEnum.FunctionScalarCLR),
            new TypeStringElement("FT",TypeEnum.FunctionTableValuedCLR),
            new TypeStringElement("iAK",TypeEnum.KeyAsymmetric),
            new TypeStringElement("iASM",TypeEnum.Assembly),
            new TypeStringElement("iCERT",TypeEnum.Certificate),
            new TypeStringElement("iCO", TypeEnum.Column),
            new TypeStringElement("iDRLE",TypeEnum.DatabaseRole),
            new TypeStringElement("iDUSR",TypeEnum.User),
            new TypeStringElement("IF",TypeEnum.FunctionInlineTableValued),
            new TypeStringElement("iFTXT",TypeEnum.FullTextCatalog),
            new TypeStringElement("iSCM",TypeEnum.Schema),
            new TypeStringElement("iSK",TypeEnum.KeySymmetric),
            new TypeStringElement("IT",TypeEnum.TableSystem),
            new TypeStringElement("iUDT",TypeEnum.UserDefinedDataType),
            new TypeStringElement("iXMLS",TypeEnum.XMLSchemaCollection),
            new TypeStringElement("P",TypeEnum.StoredProcedure),
            new TypeStringElement("PC",TypeEnum.StoredProcedureCLR),
            new TypeStringElement("S",TypeEnum.TableSystem),
            new TypeStringElement("SN",TypeEnum.Synonym),
            new TypeStringElement("TF",TypeEnum.FunctionTableValued),
            new TypeStringElement("U",TypeEnum.Table),
            new TypeStringElement("V",TypeEnum.View),
            new TypeStringElement("X",TypeEnum.ExtendedStoredProcedure),
            new TypeStringElement("SO",TypeEnum.SequenceObjects)
        };

        private static Controls.ObjectImages m_ObjectImages = new Controls.ObjectImages();

        #endregion

        #region Helpers

        #endregion

        #region Ctors

        #endregion

        #region Methods

        // This method takes repository object type string and
        // returns the equivalent TypeEnum.
        static public TypeEnum ToTypeEnum(string repositoryObjectType)
        {
            Debug.Assert(!string.IsNullOrEmpty(repositoryObjectType));

            string temp = repositoryObjectType.Trim();
            TypeEnum retType = TypeEnum.Unknown;
            for (int i = 0; i < m_TypeStringMap.Length; ++i)
            {
                if (m_TypeStringMap[i].IsTypeStrMatch(temp))
                {
                    retType = m_TypeStringMap[i].TypeEnum;
                    break;
                }
            }
            Debug.Assert(retType != TypeEnum.Unknown);

            return retType;
        }

        // Returns TypeEnum name.
        static public string TypeName(TypeEnum type)
        {
            Debug.Assert((int)type < m_TypeString.Length);
            return m_TypeString[(int)type];
        }

        // Returns image index of the type.
        static public int TypeImageIndex(TypeEnum type)
        {
            return ((int)type);
        }

        // Returns type image list (16x16)
        static public ImageList TypeImageList16()
        {
            return m_ObjectImages.ImageListSQLTypes16;
        }

        // Returns type image (16x16)
        static public Image TypeImage16(TypeEnum type)
        {
            return TypeImageList16().Images[(int)type];
        }

        // Get login/user type
        static public TypeEnum LoginType (SqlString type)
        {
            if (type.IsNull)
            {
                Debug.Assert(false, "Null login type");
                return TypeEnum.Unknown;
            }
            else
            {
                if (string.Compare(type.Value, "U", true) == 0) { return TypeEnum.WindowsUserLogin; }
                else if (string.Compare(type.Value, "G", true) == 0) { return TypeEnum.WindowsGroupLogin; }
                else if (string.Compare(type.Value, "S", true) == 0) { return TypeEnum.SqlLogin; }
                else if (string.Compare(type.Value, "R", true) == 0) { return TypeEnum.ServerRole; }
                //SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
                else if (string.Compare(type.Value, "E", true) == 0) { return TypeEnum.AzureADUser; }
                else if (string.Compare(type.Value, "X", true) == 0) { return TypeEnum.AzureADGroup; }
                else 
                { 
                    Debug.Assert(false, "Unknown login type"); 
                    return TypeEnum.Unknown; 
                }
            }
        }
        static public TypeEnum UserType(SqlString type)
        {
            if (type.IsNull)
            {
                Debug.Assert(false, "Null login type");
                return TypeEnum.Unknown;
            }
            else
            {
                if (string.Compare(type.Value, "U", true) == 0) { return TypeEnum.WindowsUserLogin; }
                else if (string.Compare(type.Value, "G", true) == 0) { return TypeEnum.WindowsGroupLogin; }
                else if (string.Compare(type.Value, "S", true) == 0) { return TypeEnum.SqlLogin; }
                else if (string.Compare(type.Value, "R", true) == 0) { return TypeEnum.DatabaseRole; }
                else if (string.Compare(type.Value, "A", true) == 0) { return TypeEnum.ApplicationRole; }
                else
                {
                    Debug.Assert(false, "Unknown login type");
                    return TypeEnum.Unknown;
                }
            }
        }

        // Convert to boolean.
        static public bool ToBoolean(SqlString flag)
        {
            if (flag.IsNull)
            {
                Debug.Assert(false, "Null boolean flag");
                return false;
            }
            else
            {
                return string.Compare(flag.Value, "Y", true) == 0;
            }
        }

        // Convert to boolean.
        static public bool? ToNullableBoolean(SqlString flag)
        {
            if (flag.IsNull || flag.ToString().Trim().Length == 0)
            {
                return null;
            }
            else
            {
                return string.Compare(flag.Value, "Y", true) == 0;
            }
        }

        #endregion
    }

    public class ObjectTag
    {
        #region Constants

        private const string NodeSnapshot = "Snapshot";
        private const string NodeServer = "SQL Server";
        private const string NodeEnvironment = "Server Environment";
        private const string NodeFileSystem = "Files & Directories";
        private const string NodeRegistry = "Registry Keys";
        private const string NodeServices = "Services";
        private const string NodeServerSecurity = "Security";
        private const string NodeLogins = "Logins";
        private const string NodeServerRoles = "Server Roles";
        private const string NodeServerObjects = "Server Objects";
        private const string NodeEndpoints = "Endpoints";
        private const string NodeDatabases = "Databases";
        private const string NodeDatabaseSecurity = "Security";
        private const string NodeUsers = "Users";
        private const string NodeRoles = "Roles";
        private const string NodeSchemas = "Schemas";
        private const string NodeKeys = "Keys";
        private const string NodeCertificates = "Certificates";
        private const string NodeTables = "Tables";
        private const string NodeViews = "Views";
        private const string NodeSynonyms = "Synonyms";
        private const string NodeStoredProcedures = "Stored Procedures";
        private const string NodeFunctions = "Functions";
        private const string NodeExtendedStoredProcedures = "Extended Stored Procedures";
        private const string NodeAssemblies = "Assemblies";
        private const string NodeUserDefinedDataTypes = "User-defined Data Types";
        private const string NodeXMLSchemaCollections = "XML Schema Collections";
        private const string NodeFullTextCatalogs = "Full Text Catalogs";
        private const string NodeSequenceObjects = "Sequence Objects";
        private const string NodeAvailabilityGroups = "Always On Availability Groups";

        #endregion

        #region Fields

        private int m_SnapshotId = 0;

        private ObjectType.TypeEnum m_ObjType = Sql.ObjectType.TypeEnum.Unknown;
        private Database m_Database = null;
        private int m_ClassId = -1;
        private int m_ParentObjectId = -1;
        private int m_ObjectId = -1;

        private string m_ObjectName = string.Empty;
        private object _tag;

        #endregion

        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        #region Ctors

        public ObjectTag( // snapshot, server and server level tree view containers.
                int snapshotId,
                Sql.ObjectType.TypeEnum objType
            )
        {
            Debug.Assert(objType == ObjectType.TypeEnum.Snapshot
                         || objType == ObjectType.TypeEnum.Server
                         || objType == ObjectType.TypeEnum.Environment
                         || objType == ObjectType.TypeEnum.FileSystem
                         || objType == ObjectType.TypeEnum.Registry
                         || objType == ObjectType.TypeEnum.Services
                         || objType == ObjectType.TypeEnum.ServerSecurity
                         || objType == ObjectType.TypeEnum.Logins
                         || objType == ObjectType.TypeEnum.ServerRoles
                         || objType == ObjectType.TypeEnum.ServerObjects
                         || objType == ObjectType.TypeEnum.Endpoints
                         || objType == ObjectType.TypeEnum.AvailabilityGroups
                         || objType == ObjectType.TypeEnum.Databases);
            Debug.Assert(objType == ObjectType.TypeEnum.Snapshot ? true : snapshotId != 0);

            m_SnapshotId = snapshotId;
            m_ObjType = objType;
            m_ClassId = m_ObjType == ObjectType.TypeEnum.Server ? 100 : 0;
        }

        public ObjectTag( // server level objects
                int snapshotId,
                Sql.ObjectType.TypeEnum objType,
                int objectId,
                string objectName,
                object tag 
            )
        {
            Debug.Assert(snapshotId != 0);
            Debug.Assert(
                         objType == ObjectType.TypeEnum.File
                         || objType == ObjectType.TypeEnum.RegistryKey
                         || objType == ObjectType.TypeEnum.Service
                         || objType == ObjectType.TypeEnum.SqlLogin
                         || objType == ObjectType.TypeEnum.WindowsGroupLogin
                         || objType == ObjectType.TypeEnum.WindowsUserLogin
                         || objType == ObjectType.TypeEnum.ServerRole
                           || objType == ObjectType.TypeEnum.ServerRole
                           || objType == ObjectType.TypeEnum.AvailabilityGroup
                           || objType == ObjectType.TypeEnum.AvailabilityGroupReplica
                         || objType == ObjectType.TypeEnum.Endpoint
                         || objType == ObjectType.TypeEnum.AzureADUser
                         || objType == ObjectType.TypeEnum.AzureADGroup
                         );
            Debug.Assert(!string.IsNullOrEmpty(objectName));

            m_SnapshotId = snapshotId;
            m_ObjType = objType;
            m_ObjectId = objectId;
            m_ObjectName = objectName;
            m_ClassId = m_ObjType == ObjectType.TypeEnum.Endpoint ? 105 : 101;
            Tag = tag;
        }

        public ObjectTag( // database and database level tree view containers
                int snapshotId,
                Sql.ObjectType.TypeEnum objType,
                Sql.Database database
            )
        {
            Debug.Assert(snapshotId != 0);
            Debug.Assert(objType == ObjectType.TypeEnum.DatabaseSecurity
                         || objType == ObjectType.TypeEnum.Users
                         || objType == ObjectType.TypeEnum.DatabaseRoles
                         || objType == ObjectType.TypeEnum.Schemas
                         || objType == ObjectType.TypeEnum.Keys
                         || objType == ObjectType.TypeEnum.Certificates
                         || objType == ObjectType.TypeEnum.Tables
                         || objType == ObjectType.TypeEnum.Views
                         || objType == ObjectType.TypeEnum.Synonyms
                         || objType == ObjectType.TypeEnum.StoredProcedures
                         || objType == ObjectType.TypeEnum.Functions
                         || objType == ObjectType.TypeEnum.ExtendedStoredProcedures
                         || objType == ObjectType.TypeEnum.Assemblies
                         || objType == ObjectType.TypeEnum.UserDefinedDataTypes
                         || objType == ObjectType.TypeEnum.XMLSchemaCollections
                         || objType == ObjectType.TypeEnum.FullTextCatalogs
                          || objType == ObjectType.TypeEnum.SequenceObjects);
            Debug.Assert(database != null);

            m_SnapshotId = snapshotId;
            m_ObjType = objType;
            m_Database = database;
        }

        public ObjectTag( // Database user, role, schema, cert and key objects.
                int snapshotid,
                Sql.ObjectType.TypeEnum objType,
                Sql.Database database,
                int objectId,
                string objectName
            )
        {
            Debug.Assert(snapshotid != 0);
            Debug.Assert(objType == ObjectType.TypeEnum.User
                         || objType == ObjectType.TypeEnum.DatabaseRole
                         || objType == ObjectType.TypeEnum.ApplicationRole
                         || objType == ObjectType.TypeEnum.Key
                         || objType == ObjectType.TypeEnum.Certificate);
            Debug.Assert(database != null);
            Debug.Assert(!string.IsNullOrEmpty(objectName));

            m_SnapshotId = snapshotid;
            m_ObjType = objType;
            m_Database = database;
            switch (m_ObjType)
            {
                case ObjectType.TypeEnum.User:
                case ObjectType.TypeEnum.DatabaseRole:
                case ObjectType.TypeEnum.ApplicationRole:
                case ObjectType.TypeEnum.Key:
                case ObjectType.TypeEnum.Certificate:
                    m_ClassId = 4;
                    break;
            }
            m_ObjectId = objectId;
            m_ObjectName = objectName;
        }

        public ObjectTag( // Database, and schema objects.
                int snapshotid,
                Sql.ObjectType.TypeEnum objType,
                Sql.Database database,
                int classId,
                int objectId,
                string objectName
            )
        {
            Debug.Assert(snapshotid != 0);
            Debug.Assert(objType == ObjectType.TypeEnum.Database
                         || objType == ObjectType.TypeEnum.Schema);
            Debug.Assert(database != null);
            Debug.Assert(!string.IsNullOrEmpty(objectName));

            m_SnapshotId = snapshotid;
            m_ObjType = objType;
            m_Database = database;
            m_ClassId = classId;
            m_ObjectId = objectId;
            m_ObjectName = objectName;
        }

        public ObjectTag( // Database objects - table, view, function, stored proc, etc.
                int snapshotid,
                Sql.ObjectType.TypeEnum objType,
                Sql.Database database,
                int classId,
                int parentObjectId,
                int objectId,
                string objectName
            )
        {
            Debug.Assert(snapshotid != 0);
            Debug.Assert(objType == ObjectType.TypeEnum.Table
                         || objType == ObjectType.TypeEnum.View
                         || objType == ObjectType.TypeEnum.Synonym
                         || objType == ObjectType.TypeEnum.StoredProcedure
                         || objType == ObjectType.TypeEnum.Function
                         || objType == ObjectType.TypeEnum.ExtendedStoredProcedure
                         || objType == ObjectType.TypeEnum.Assembly
                         || objType == ObjectType.TypeEnum.UserDefinedDataType
                         || objType == ObjectType.TypeEnum.XMLSchemaCollection
                         || objType == ObjectType.TypeEnum.FullTextCatalog
                         || objType == ObjectType.TypeEnum.SequenceObjects
                         || objType == ObjectType.TypeEnum.SequenceObject);
            Debug.Assert(database != null);
            Debug.Assert(!string.IsNullOrEmpty(objectName));

            m_SnapshotId = snapshotid;
            m_ObjType = objType;
            m_Database = database;
            m_ClassId = classId;
            m_ParentObjectId = parentObjectId;
            m_ObjectId = objectId;
            m_ObjectName = objectName;
        }

        #endregion

        #region Properties

        public Sql.ObjectType.TypeEnum ObjType
        {
            get { return m_ObjType; }
        }

        public Sql.Database Database
        {
            get { return m_Database; }
        }
        public int SnapshotId
        {
            get { return m_SnapshotId; }
        }
        public int DatabaseId
        {
            get { return (m_Database != null ?  m_Database.DbId : -1); }
        }
        public int ClassId
        {
            get
            {
                return m_ObjType == ObjectType.TypeEnum.AvailabilityGroup ? 108 : m_ClassId;
            }
        }
        public int ParentObjectId
        {
            get { return m_ParentObjectId; }
        }
        public int ObjectId
        {
            get { return m_ObjectId; }
        }

        public string DatabaseName
        {
            get { return (m_Database != null ? m_Database.Name : string.Empty); }
        }

        public string ObjectName
        {
            get
            {
                if (m_ObjType == ObjectType.TypeEnum.Database)
                {
                    return DatabaseName;
                }
                else
                {
                    return m_ObjectName;
                }
            }
        }

        public int ImageIndex
        {
            get { return ObjectType.TypeImageIndex(m_ObjType); }
        }

        public Image Image16
        {
            get { return ObjectType.TypeImage16(m_ObjType); }
        }

        public string NodeName
        {
            get
            {
                string name = string.Empty;
                switch (m_ObjType)
                {
                    case ObjectType.TypeEnum.Snapshot:
                        name = NodeSnapshot;
                        break;
                    case ObjectType.TypeEnum.Server:
                        name = NodeServer;
                        break;
                    case ObjectType.TypeEnum.Environment:
                        name = NodeEnvironment;
                        break;
                    case ObjectType.TypeEnum.FileSystem:
                        name = NodeFileSystem;
                        break;
                    case ObjectType.TypeEnum.Registry:
                        name = NodeRegistry;
                        break;
                    case ObjectType.TypeEnum.Services:
                        name = NodeServices;
                        break;
                    case ObjectType.TypeEnum.ServerSecurity:
                        name = NodeServerSecurity;
                        break;
                    case ObjectType.TypeEnum.Logins:
                        name = NodeLogins;
                        break;
                    case ObjectType.TypeEnum.ServerRoles:
                        name = NodeServerRoles;
                        break;
                    case ObjectType.TypeEnum.ServerObjects:
                        name = NodeServerObjects;
                        break;
                    case ObjectType.TypeEnum.Endpoints:
                        name = NodeEndpoints;
                        break;
                    case ObjectType.TypeEnum.Databases:
                        name = NodeDatabases;
                        break;
                    case ObjectType.TypeEnum.Database:
                        name = DatabaseName;
                        break;
                    case ObjectType.TypeEnum.DatabaseSecurity:
                        name = NodeDatabaseSecurity;
                        break;
                    case ObjectType.TypeEnum.Users:
                        name = NodeUsers;
                        break;
                    case ObjectType.TypeEnum.DatabaseRoles:
                        name = NodeRoles;
                        break;
                    case ObjectType.TypeEnum.Schemas:
                        name = NodeSchemas;
                        break;
                    case ObjectType.TypeEnum.Keys:
                        name = NodeKeys;
                        break;
                    case ObjectType.TypeEnum.Certificates:
                        name = NodeCertificates;
                        break;
                    case ObjectType.TypeEnum.Tables:
                        name = NodeTables;
                        break;
                    case ObjectType.TypeEnum.Views:
                        name = NodeViews;
                        break;
                    case ObjectType.TypeEnum.Synonyms:
                        name = NodeSynonyms;
                        break;
                    case ObjectType.TypeEnum.StoredProcedures:
                        name = NodeStoredProcedures;
                        break;
                    case ObjectType.TypeEnum.Functions:
                        name = NodeFunctions;
                        break;
                    case ObjectType.TypeEnum.ExtendedStoredProcedures:
                        name = NodeExtendedStoredProcedures;
                        break;
                    case ObjectType.TypeEnum.Assemblies:
                        name = NodeAssemblies;
                        break;
                    case ObjectType.TypeEnum.UserDefinedDataTypes:
                        name = NodeUserDefinedDataTypes;
                        break;
                    case ObjectType.TypeEnum.XMLSchemaCollections:
                        name = NodeXMLSchemaCollections;
                        break;
                    case ObjectType.TypeEnum.FullTextCatalogs:
                        name = NodeFullTextCatalogs;
                        break;
                    case ObjectType.TypeEnum.SequenceObjects:
                        name = NodeSequenceObjects;
                        break;
                    case ObjectType.TypeEnum.AvailabilityGroups:
                        name = NodeAvailabilityGroups;
                        break;
                    default:
                        Debug.Assert(false);
                        break;
                }

                return name;
            }
        }

        public string TypeName
        {
            get { return ObjectType.TypeName(m_ObjType); }
        }

        public bool IsLeafTag
        {
            get
            {
                return (m_ObjType == ObjectType.TypeEnum.File
                       || m_ObjType == ObjectType.TypeEnum.RegistryKey
                       || m_ObjType == ObjectType.TypeEnum.Service
                       || m_ObjType == ObjectType.TypeEnum.WindowsUserLogin
                       || m_ObjType == ObjectType.TypeEnum.WindowsGroupLogin
                       || m_ObjType == ObjectType.TypeEnum.SqlLogin
                       || m_ObjType == ObjectType.TypeEnum.ServerRole
                       || m_ObjType == ObjectType.TypeEnum.Endpoint
                       || m_ObjType == ObjectType.TypeEnum.User
                       || m_ObjType == ObjectType.TypeEnum.DatabaseRole
                       || m_ObjType == ObjectType.TypeEnum.ApplicationRole
                       || m_ObjType == ObjectType.TypeEnum.Schema
                       || m_ObjType == ObjectType.TypeEnum.Key
                       || m_ObjType == ObjectType.TypeEnum.Certificate
                       || m_ObjType == ObjectType.TypeEnum.Table
                       || m_ObjType == ObjectType.TypeEnum.View
                       || m_ObjType == ObjectType.TypeEnum.Synonym
                       || m_ObjType == ObjectType.TypeEnum.StoredProcedure
                       || m_ObjType == ObjectType.TypeEnum.Function
                       || m_ObjType == ObjectType.TypeEnum.ExtendedStoredProcedure
                       || m_ObjType == ObjectType.TypeEnum.Assembly
                       || m_ObjType == ObjectType.TypeEnum.UserDefinedDataType
                       || m_ObjType == ObjectType.TypeEnum.XMLSchemaCollection
                       || m_ObjType == ObjectType.TypeEnum.AvailabilityGroupReplica
                       || m_ObjType == ObjectType.TypeEnum.AvailabilityGroup
                       || m_ObjType == ObjectType.TypeEnum.SequenceObject
                       || m_ObjType == ObjectType.TypeEnum.FullTextCatalog
                       //SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
                       || m_ObjType == ObjectType.TypeEnum.AzureADUser
                       || m_ObjType == ObjectType.TypeEnum.AzureADGroup);
            }
        }

        public bool IsPropertyTag
        {
            get
            {
                return (m_ObjType == ObjectType.TypeEnum.Snapshot
                        || m_ObjType == ObjectType.TypeEnum.Server
                        || m_ObjType == ObjectType.TypeEnum.Database
                        || IsLeafTag);
            }
        }

        #endregion

        #region Methods

        public string Path(
                string snapshot,
                string server
            )
        {
            string path = string.Empty;
            switch (m_ObjType)
            {
                case ObjectType.TypeEnum.Snapshot:
                    path = snapshot;
                    break;
                case ObjectType.TypeEnum.Server:
                    path = snapshot + @"\" + server;
                    break;
                case ObjectType.TypeEnum.ServerSecurity:
                    path = snapshot + @"\" + server + @"\Security";
                    break;
                case ObjectType.TypeEnum.Logins:
                    path = snapshot + @"\" + server + @"\Security\Logins";
                    break;
                case ObjectType.TypeEnum.ServerRoles:
                    path = snapshot + @"\" + server + @"\Security\Roles";
                    break;
                case ObjectType.TypeEnum.ServerObjects:
                    path = snapshot + @"\" + server + @"\Server Objects";
                    break;
                case ObjectType.TypeEnum.Endpoints:
                    path = snapshot + @"\" + server + @"\Server Objects\Endpoints";
                    break;
                case ObjectType.TypeEnum.Databases:
                    path = snapshot + @"\" + server + @"\Databases";
                    break;
                case ObjectType.TypeEnum.Database:
                    path = snapshot + @"\" + server + @"\Databases\" + DatabaseName;
                    break;
                case ObjectType.TypeEnum.DatabaseSecurity:
                    path = snapshot + @"\" + server + @"\Databases\" + DatabaseName + @"\Security";
                    break;
                case ObjectType.TypeEnum.Users:
                    path = snapshot + @"\" + server + @"\Databases\" + DatabaseName + @"\Security\Users";
                    break;
                case ObjectType.TypeEnum.DatabaseRoles:
                    path = snapshot + @"\" + server + @"\Databases\" + DatabaseName + @"\Security\Roles";
                    break;
                case ObjectType.TypeEnum.Schemas:
                    path = snapshot + @"\" + server + @"\Databases\" + DatabaseName + @"\Security\Schemas";
                    break;
                case ObjectType.TypeEnum.Keys:
                    path = snapshot + @"\" + server + @"\Databases\" + DatabaseName + @"\Security\Keys";
                    break;
                case ObjectType.TypeEnum.Certificates:
                    path = snapshot + @"\" + server + @"\Databases\" + DatabaseName + @"\Certificates";
                    break;
                case ObjectType.TypeEnum.Tables:
                    path = snapshot + @"\" + server + @"\Databases\" + DatabaseName + @"\Tables";
                    break;
                case ObjectType.TypeEnum.Views:
                    path = snapshot + @"\" + server + @"\Databases\" + DatabaseName + @"\Views";
                    break;
                case ObjectType.TypeEnum.Synonyms:
                    path = snapshot + @"\" + server + @"\Databases\" + DatabaseName + @"\Synonyms";
                    break;
                case ObjectType.TypeEnum.StoredProcedures:
                    path = snapshot + @"\" + server + @"\Databases\" + DatabaseName + @"\Stored Procedures";
                    break;
                case ObjectType.TypeEnum.Functions:
                    path = snapshot + @"\" + server + @"\Databases\" + DatabaseName + @"\Functions";
                    break;
                case ObjectType.TypeEnum.ExtendedStoredProcedures:
                    path = snapshot + @"\" + server + @"\Databases\" + DatabaseName + @"\Extended Stored Procedures";
                    break;
                case ObjectType.TypeEnum.Assemblies:
                    path = snapshot + @"\" + server + @"\Databases\" + DatabaseName + @"\Assemblies";
                    break;
                case ObjectType.TypeEnum.UserDefinedDataTypes:
                    path = snapshot + @"\" + server + @"\Databases\" + DatabaseName + @"\User-defined Data Types";
                    break;
                case ObjectType.TypeEnum.XMLSchemaCollections:
                    path = snapshot + @"\" + server + @"\Databases\" + DatabaseName + @"\XML Schema Collections";
                    break;
                case ObjectType.TypeEnum.FullTextCatalogs:
                    path = snapshot + @"\" + server + @"\Databases\" + DatabaseName + @"\Full Text Catalogs";
                    break;
                case ObjectType.TypeEnum.SequenceObjects:
                    path = snapshot + @"\" + server + @"\Databases\" + DatabaseName + @"\Full Sequence Objects";
                    break;

                default:
                    Debug.Assert(false);
                    break;
            }

            return path;
        }

        #endregion
    }

    public static class Constants
    {
        #region General

        internal const string SqlAppName = @"Idera SQLsecure";
        internal const string Sql2000VerPrefix = @"8";
        internal const string Sql2005VerPrefix = @"9";
        internal const string Sql2008VerPrefix = @"10";
        internal const string Sql2008R2VerPrefix = @"10.50.";
        internal const string Sql2012VerPrefix = @"11";
        internal const string Sql2014VerPrefix = @"12";
        internal const string Sql2016VerPrefix = @"13";

        #endregion

        #region Registered  Server

        internal const string SqlAuthMode = "S";

        #endregion

        #region Server Principal(Login)

        internal const string WindowsGroup = "G";
        internal const string WindowsUser = "U";
        internal const string SQLLogin = "S";

        #endregion

        #region Data Collection Filter

        internal const int InvalidId = -1;

        internal const string RuleScopeAll = "A";
        internal const string RuleScopeSystem = "S";
        internal const string RuleScopeUser = "U";

        #endregion
    }
}
