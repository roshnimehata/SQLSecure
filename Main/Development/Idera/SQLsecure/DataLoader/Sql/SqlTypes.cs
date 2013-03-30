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
using System.Text;

namespace Idera.SQLsecure.Collector.Sql
{

    public class SQLTypes
    {
        public static string GetTextFromType(SqlObjectType objType)
        {
            string strType = "UnknownType";
            switch (objType)
            {
                case SqlObjectType.Assembly:
                    strType = "Assembly";
                    break;
                case SqlObjectType.Certificate:
                    strType = "Certificate"; 
                    break;
                case SqlObjectType.Database:
                    strType = "Database";
                    break;
                case SqlObjectType.Endpoint:
                    strType = "Endpoint";
                    break;
                case SqlObjectType.ExtendedStoredProcedure:
                    strType = "ExtendedStoredProcedure";
                    break;
                case SqlObjectType.FullTextCatalog:
                    strType = "FullTextCatalog";
                    break;
                case SqlObjectType.Function:
                    strType = "Function";
                    break;
                case SqlObjectType.Key:
                    strType = "Key";
                    break;
                case SqlObjectType.Login:
                    strType = "Login";
                    break;
                case SqlObjectType.Role:
                    strType = "Role";
                    break;
                case SqlObjectType.Schema:
                    strType = "Schema";
                    break;
                case SqlObjectType.Server:
                    strType = "Server";
                    break;
                case SqlObjectType.StoredProcedure:
                    strType = "StoredProcedure";
                    break;
                case SqlObjectType.Synonym:
                    strType = "Synonym";
                    break;
                case SqlObjectType.Table:
                    strType = "Table";
                    break;
                case SqlObjectType.User:
                    strType = "User";
                    break;
                case SqlObjectType.UserDefinedDataType:
                    strType = "UserDefinedDataType";
                    break;
                case SqlObjectType.View:
                    strType = "View";
                    break;
                case SqlObjectType.XMLSchemaCollection:
                    strType = "XMLSchemaCollection";
                    break;
                case SqlObjectType.Column:
                    strType = "Column";
                    break;
                default:
                    strType = "UnKnown";
                    System.Diagnostics.Debug.Assert(false, "Unknown Database Type");
                    break;
            }
            return strType;
        }
    }
    public enum ServerVersion
    {
        SQL2000,
        SQL2005,
        SQL2008,
        SQL2008R2,
        SQL2012,
        Unsupported
    }

    public enum SqlObjectType
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
        Column = 47
    }

    public enum FilterScope
    {
        Any,
        System,
        User
    }

    public enum FilterType
    {
        ServerLevel,
        DatabaseLevel,
        Unknown
    }

    public static class Constants
    {
        #region General

        internal const string SqlAppName = @"Idera SQLsecure Data Loader";
        internal const string Sql2000VerPrefix = @"08";
        internal const string Sql2005VerPrefix = @"09";
        internal const string Sql2008VerPrefix = @"10";
        internal const string Sql2008R2VerPrefix = @"10.50";
        internal const string Sql2012VerPrefix = @"11";

        #endregion

        #region Server Principal(Login)

        internal const int Sql2000PidStart = 100;

        public const string WindowsGroup = "G";
        public const string WindowsUser = "U";
        public const string SQLLogin = "S";

        #endregion

        #region Database Principal

        internal const int GuestUser = 2;

        #endregion

        #region Query/Insertion

        internal const int PermissionBatchSize = 10;
        internal const int RowBatchSize = 1000;

        #endregion
    }
}
