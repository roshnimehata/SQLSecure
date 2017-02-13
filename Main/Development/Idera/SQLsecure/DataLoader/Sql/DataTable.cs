/******************************************************************
 * Name: DataTable.cs
 *
 * Description: The data tables for updating the repository are
 * encapsulated in this file.
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
    internal static class ServerPrincipalDataTable
    {
        public static DataTable Create()
        {
            DataTable dataTable = null;
            using (DataColumn
                    colSnapshotId = new DataColumn(ParamSnapshotid, typeof(SqlInt32)),
                    colPrincipalId = new DataColumn(ParamPrincipalid, typeof(SqlInt32)),
                    colSid = new DataColumn(ParamSid, typeof(SqlBinary)),
                    colName = new DataColumn(ParamName, typeof(SqlString)),
                    colType = new DataColumn(ParamType, typeof(SqlString)),
                    colServerAccess = new DataColumn(ParamServeraccess, typeof(SqlString)),
                    colServerDeny = new DataColumn(ParamServerdeny, typeof(SqlString)),
                    colDisabled = new DataColumn(ParamDisabled, typeof(SqlString)),
                    colIsExpirationChecked = new DataColumn(ParamIsexpirationchecked, typeof(SqlString)),
                    colIsPolicyChecked = new DataColumn(ParamIspolicychecked, typeof(SqlString)),
                    colHashkey = new DataColumn(ParamHashkey, typeof(SqlString)),
                    colIsPasswordNull = new DataColumn(ParamIsPasswordNull, typeof(SqlString)),
                    colDefaultDatabase = new DataColumn(ParamDefaultDatabase, typeof(SqlString)),
                    colDefaultLanguage = new DataColumn(ParamDefaultLanguage, typeof(SqlString)),
                    colPasswordStatus = new DataColumn(ParamPasswordStatus, typeof(SqlInt32)))
            {
                // Create the data table object & define its columns.
                // NOTE : THE ORDER OF THE COLUMNS MUST MATCH WHAT IS IN THE REPOSITORY
                dataTable = new DataTable("serverprincipal");
                dataTable.Columns.AddRange(new DataColumn[] {
                                                                colSnapshotId,
                                                                colPrincipalId,
                                                                colSid,
                                                                colName,
                                                                colType,
                                                                colServerAccess,
                                                                colServerDeny,
                                                                colDisabled,
                                                                colIsExpirationChecked,
                                                                colIsPolicyChecked,
                                                                colHashkey,
                                                                colIsPasswordNull,
                                                                colDefaultDatabase,
                                                                colDefaultLanguage,
                                                                colPasswordStatus
                                                            });
            }

            return dataTable;
        }

        internal const string ParamSnapshotid = "snapshotid";
        internal const string ParamPrincipalid = "principalid";
        internal const string ParamSid = "sid";
        internal const string ParamName = "name";
        internal const string ParamType = "type";
        internal const string ParamServeraccess = "serveraccess";
        internal const string ParamServerdeny = "serverdeny";
        internal const string ParamDisabled = "disabled";
        internal const string ParamIsexpirationchecked = "isexpirationchecked";
        internal const string ParamIspolicychecked = "ispolicychecked";
        internal const string ParamHashkey = "hashkey";
        internal const string ParamIsPasswordNull = "ispasswordnull";
        internal const string ParamDefaultDatabase = "defaultdatabase";
        internal const string ParamDefaultLanguage = "defaultlanguage";
        internal const string ParamPasswordStatus = "passwordStatus";

        internal const string RepositoryTable = "SQLsecure.dbo.serverprincipal";
    }

    internal static class ServerRoleMemberDataTable
    {
        public static DataTable Create()
        {
            DataTable dataTable = null;
            using (DataColumn
                    colSnapshotId = new DataColumn(ParamSnapshotid, typeof(SqlInt32)),
                    colPrincipalId = new DataColumn(ParamPrincipalid, typeof(SqlInt32)),
                    colMemberPrincipalId = new DataColumn(ParamMemberprincipalid, typeof(SqlInt32)),
                    colHashkey = new DataColumn(ParamHashkey, typeof(SqlString)))
            {
                // Create the data table object & define its columns.
                // NOTE : THE ORDER OF THE COLUMNS MUST MATCH WHAT IS IN THE REPOSITORY
                dataTable = new DataTable("serverprincipal");
                dataTable.Columns.AddRange(new DataColumn[] {
                                                                colSnapshotId,
                                                                colPrincipalId,
                                                                colMemberPrincipalId,
                                                                colHashkey
                                                            });
            }

            return dataTable;
        }

        internal const string ParamSnapshotid = "snapshotid";
        internal const string ParamPrincipalid = "principalid";
        internal const string ParamMemberprincipalid = "memberprincipalid";
        internal const string ParamHashkey = "hashkey";

        internal const string RepositoryTable = "SQLsecure.dbo.serverrolemember";
    }

    internal static class EndPointDataTable
    {
        public static DataTable Create()
        {
            DataTable dataTable = null;
            using (DataColumn
                    colSnapshotId = new DataColumn(ParamSnapshotid, typeof(SqlInt32)),
                    colPrincipalId = new DataColumn(ParamPrincipalid, typeof(SqlInt32)),
                    colEndpointId = new DataColumn(ParamEndpointid, typeof(SqlInt32)),
                    colName = new DataColumn(ParamName, typeof(SqlString)),
                    colType = new DataColumn(ParamType, typeof(SqlString)),
                    colProtocol = new DataColumn(ParamProtocol, typeof(SqlString)),
                    colState = new DataColumn(ParamState, typeof(SqlString)),
                    colIsadminendpoint = new DataColumn(ParamIsadminendpoint, typeof(SqlString)),
                    colHashkey = new DataColumn(ParamHashkey, typeof(SqlString)))
            {
                // Create the data table object & define its columns.
                // NOTE : THE ORDER OF THE COLUMNS MUST MATCH WHAT IS IN THE REPOSITORY
                dataTable = new DataTable("endpoint");
                dataTable.Columns.AddRange(new DataColumn[] {
                                                                colSnapshotId,
                                                                colEndpointId,
                                                                colPrincipalId,
                                                                colName,
                                                                colType,
                                                                colProtocol,
                                                                colState,
                                                                colIsadminendpoint,
                                                                colHashkey
                                                            });
            }

            return dataTable;
        }

        internal const string ParamSnapshotid = "snapshotid";
        internal const string ParamPrincipalid = "principalid";
        internal const string ParamEndpointid = "endpointid";
        internal const string ParamName = "name";
        internal const string ParamType = "type";
        internal const string ParamProtocol = "protocol";
        internal const string ParamState = "state";
        internal const string ParamIsadminendpoint = "isadminendpoint";
        internal const string ParamHashkey = "hashkey";

        internal const string RepositoryTable = "SQLsecure.dbo.endpoint";
    }

    internal static class ServerPermissionDataTable
    {
        public static DataTable Create()
        {
            DataTable dataTable = null;
            using (DataColumn
                    colSnapshotId = new DataColumn(ParamSnapshotid, typeof(SqlInt32)),
                    colClassId = new DataColumn(ParamClassId, typeof(SqlInt32)),
                    colGrantee = new DataColumn(ParamGrantee, typeof(SqlInt32)),
                    colMajorId = new DataColumn(ParamMajorId, typeof(SqlInt32)),
                    colMinorId = new DataColumn(ParamMinorId, typeof(SqlInt32)),
                    colPermission = new DataColumn(ParamPermission, typeof(SqlString)),
                    colGrantor = new DataColumn(ParamGrantor, typeof(SqlInt32)),
                    colIsGrant = new DataColumn(ParamIsgrant, typeof(SqlString)),
                    colIsGrantWith = new DataColumn(ParamIsgrantwith, typeof(SqlString)),
                    colIsRevoke = new DataColumn(ParamIsrevoke, typeof(SqlString)),
                    colIsDeny = new DataColumn(ParamIsdeny, typeof(SqlString)),
                    colHashkey = new DataColumn(ParamHashkey, typeof(SqlString)))
            {
                // Create the data table object & define its columns.
                // NOTE : THE ORDER OF THE COLUMNS MUST MATCH WHAT IS IN THE REPOSITORY
                dataTable = new DataTable("serverpermission");
                dataTable.Columns.AddRange(new DataColumn[] {
                                                                colSnapshotId,
                                                                colMajorId,
                                                                colMinorId,
                                                                colClassId,
                                                                colPermission,
                                                                colGrantee,
                                                                colGrantor,
                                                                colIsGrant,
                                                                colIsGrantWith,
                                                                colIsRevoke,
                                                                colIsDeny,
                                                                colHashkey
                                                            });
            }

            return dataTable;
        }

        internal const string ParamSnapshotid = "snapshotid";
        internal const string ParamMajorId = "majorid";
        internal const string ParamMinorId = "minorid";
        internal const string ParamClassId = "classid";
        internal const string ParamPermission = "permission";
        internal const string ParamGrantee = "grantee";
        internal const string ParamGrantor = "grantor";
        internal const string ParamIsgrant = "isgrant";
        internal const string ParamIsgrantwith = "isgrantwith";
        internal const string ParamIsrevoke = "isrevoke";
        internal const string ParamIsdeny = "isdeny";
        internal const string ParamHashkey = "hashkey";

        internal const string RepositoryTable = "SQLsecure.dbo.serverpermission";
    }

    internal static class AncillaryWindowsGroupDataTable
    {
        public static DataTable Create()
        {
            DataTable dataTable = null;
            using (DataColumn
                    colSnapshotId = new DataColumn(ParamSnapshotid, typeof(SqlInt32)),
                    colWindowsGroupName = new DataColumn(ParamWindowsGroupName, typeof(SqlString)))
            {
                // Create the data table object & define its columns.
                // NOTE : THE ORDER OF THE COLUMNS MUST MATCH WHAT IS IN THE REPOSITORY
                dataTable = new DataTable("serverfilterruleheader");
                dataTable.Columns.AddRange(new DataColumn[] {
                                                                colSnapshotId,
                                                                colWindowsGroupName,
                                                           });
            }

            return dataTable;
        }

        internal const string ParamSnapshotid = "snapshotid";
        internal const string ParamWindowsGroupName = "windowsgroupname";
        internal const string RepositoryTable = "SQLsecure.dbo.ancillarywindowsgroup";

    }

    internal static class ServerFilterRuleHeaderDataTable
    {
        public static DataTable Create()
        {
            DataTable dataTable = null;
            using (DataColumn
                    colSnapshotId = new DataColumn(ParamSnapshotid, typeof(SqlInt32)),
                    colFilteruleheaderid = new DataColumn(ParamFilterruleheaderid, typeof(SqlInt32)),
                    colRulename = new DataColumn(ParamRulename, typeof(SqlString)),
                    colDescription = new DataColumn(ParamDescription, typeof(SqlString)),
                    colCreatedtm = new DataColumn(ParamCreatedtm, typeof(SqlDateTime)),
                    colCreatedby = new DataColumn(ParamCreatedby, typeof(SqlString)),
                    colLastmodifiedby = new DataColumn(ParamLastmodifiedby, typeof(SqlString)),
                    colLastmodifiedtm = new DataColumn(ParamLastmodifiedtm, typeof(SqlDateTime)),
                    colHashkey = new DataColumn(ParamHashkey, typeof(SqlString)))
            {
                // Create the data table object & define its columns.
                // NOTE : THE ORDER OF THE COLUMNS MUST MATCH WHAT IS IN THE REPOSITORY
                dataTable = new DataTable("serverfilterruleheader");
                dataTable.Columns.AddRange(new DataColumn[] {
                                                                colSnapshotId,
                                                                colFilteruleheaderid,
                                                                colRulename,
                                                                colDescription,
                                                                colCreatedtm,
                                                                colCreatedby,
                                                                colLastmodifiedby,
                                                                colLastmodifiedtm,
                                                                colHashkey
                                                            });
            }

            return dataTable;
        }

        internal const string ParamSnapshotid = "snapshotid";
        internal const string ParamFilterruleheaderid = "filterruleheaderid";
        internal const string ParamRulename = "rulename";
        internal const string ParamDescription = "description";
        internal const string ParamCreatedby = "createdby";
        internal const string ParamCreatedtm = "createdtm";
        internal const string ParamLastmodifiedby = "lastmodifiedby";
        internal const string ParamLastmodifiedtm = "lastmodifiedtm";
        internal const string ParamHashkey = "hashkey";

        internal const string RepositoryTable = "SQLsecure.dbo.serverfilterruleheader";
    }

    internal static class ServerFilterRuleDataTable
    {
        public static DataTable Create()
        {
            DataTable dataTable = null;
            using (DataColumn
                    colSnapshotId = new DataColumn(ParamSnapshotid, typeof(SqlInt32)),
                    colFilteruleheaderid = new DataColumn(ParamFilterruleheaderid, typeof(SqlInt32)),
                    colFilterruleid = new DataColumn(ParamFilterruleid, typeof(SqlInt32)),
                    colScope = new DataColumn(ParamScope, typeof(SqlString)),
                    colClass = new DataColumn(ParamClass, typeof(SqlInt32)),
                    colMatchstring = new DataColumn(ParamMatchstring, typeof(SqlString)),
                    colHashkey = new DataColumn(ParamHashkey, typeof(SqlString)))
            {
                // Create the data table object & define its columns.
                // NOTE : THE ORDER OF THE COLUMNS MUST MATCH WHAT IS IN THE REPOSITORY
                dataTable = new DataTable("serverfilterrule");
                dataTable.Columns.AddRange(new DataColumn[] {
                                                                colSnapshotId,
                                                                colFilteruleheaderid,
                                                                colFilterruleid,
                                                                colScope,
                                                                colClass,
                                                                colMatchstring,
                                                                colHashkey
                                                            });
            }

            return dataTable;
        }

        internal const string ParamSnapshotid = "snapshotid";
        internal const string ParamFilterruleheaderid = "filterruleheaderid";
        internal const string ParamFilterruleid = "filterruleid";
        internal const string ParamScope = "scope";
        internal const string ParamClass = "class";
        internal const string ParamMatchstring = "matchstring";
        internal const string ParamHashkey = "hashkey";

        internal const string RepositoryTable = "SQLsecure.dbo.serverfilterrule";
    }

    internal static class WindowsAccountDataTable
    {
        public static DataTable Create()
        {
            DataTable dataTable = null;
            using (DataColumn
                    colSnapshotId = new DataColumn(ParamSnapshotid, typeof(SqlInt32)),
                    colSid = new DataColumn(ParamSid, typeof(SqlBinary)),
                    colType = new DataColumn(ParamType, typeof(SqlString)),
                    colName = new DataColumn(ParamName, typeof(SqlString)),
                    colState = new DataColumn(ParamState, typeof(SqlString)),
                    colHashkey = new DataColumn(ParamHashkey, typeof(SqlString)),
                    colEnabled = new DataColumn(ParamEnabled, typeof(SqlBoolean)))
            {
                // Create the data table object & define its columns.
                // NOTE : THE ORDER OF THE COLUMNS MUST MATCH WHAT IS IN THE REPOSITORY
                dataTable = new DataTable("windowsaccount");
                dataTable.Columns.AddRange(new DataColumn[] {
                                                                colSnapshotId,
                                                                colSid,
                                                                colType,
                                                                colName,
                                                                colState,
                                                                colHashkey,
                                                                colEnabled
                                                            });
            }

            return dataTable;
        }

        internal const string ParamSnapshotid = "snapshotid";
        internal const string ParamSid = "sid";
        internal const string ParamType = "type";
        internal const string ParamName = "name";
        internal const string ParamState = "state";
        internal const string ParamHashkey = "hashkey";
        internal const string ParamEnabled = "enabled";

        internal const string RepositoryTable = "SQLsecure.dbo.windowsaccount";
    }

    internal static class WindowsGroupMemberDataTable
    {
        public static DataTable Create()
        {
            DataTable dataTable = null;
            using (DataColumn
                    colSnapshotId = new DataColumn(ParamSnapshotid, typeof(SqlInt32)),
                    colGroupsid = new DataColumn(ParamGroupsid, typeof(SqlBinary)),
                    colGroupmember = new DataColumn(ParamGroupmember, typeof(SqlBinary)),
                    colHashkey = new DataColumn(ParamHashkey, typeof(SqlString)))
            {
                // Create the data table object & define its columns.
                // NOTE : THE ORDER OF THE COLUMNS MUST MATCH WHAT IS IN THE REPOSITORY
                dataTable = new DataTable("windowsgroupmember");
                dataTable.Columns.AddRange(new DataColumn[] {
                                                                colSnapshotId,
                                                                colGroupsid,
                                                                colGroupmember,
                                                                colHashkey
                                                            });
            }

            return dataTable;
        }

        internal const string ParamSnapshotid = "snapshotid";
        internal const string ParamGroupsid = "groupsid";
        internal const string ParamGroupmember = "groupmember";
        internal const string ParamHashkey = "hashkey";

        internal const string RepositoryTable = "SQLsecure.dbo.windowsgroupmember";
    }

    internal static class WindowsOSAccountDataTable
    {
        public static DataTable Create()
        {
            DataTable dataTable = null;
            using (DataColumn
                    colSnapshotId = new DataColumn(ParamSnapshotid, typeof(SqlInt32)),
                    colSid = new DataColumn(ParamSid, typeof(SqlBinary)),
                    colType = new DataColumn(ParamType, typeof(SqlString)),
                    colName = new DataColumn(ParamName, typeof(SqlString)),
                    colState = new DataColumn(ParamState, typeof(SqlString)),
                    colHashkey = new DataColumn(ParamHashkey, typeof(SqlString)))
            {
                // Create the data table object & define its columns.
                // NOTE : THE ORDER OF THE COLUMNS MUST MATCH WHAT IS IN THE REPOSITORY
                dataTable = new DataTable("serveroswindowsaccount");
                dataTable.Columns.AddRange(new DataColumn[] {
                                                                colSnapshotId,
                                                                colSid,
                                                                colType,
                                                                colName,
                                                                colState,
                                                                colHashkey
                                                            });
            }

            return dataTable;
        }

        internal const string ParamSnapshotid = "snapshotid";
        internal const string ParamSid = "sid";
        internal const string ParamType = "type";
        internal const string ParamName = "name";
        internal const string ParamState = "state";
        internal const string ParamHashkey = "hashkey";

        internal const string RepositoryTable = "SQLsecure.dbo.serveroswindowsaccount";
    }

    internal static class WindowsOSGroupMemberDataTable
    {
        public static DataTable Create()
        {
            DataTable dataTable = null;
            using (DataColumn
                    colSnapshotId = new DataColumn(ParamSnapshotid, typeof(SqlInt32)),
                    colGroupsid = new DataColumn(ParamGroupsid, typeof(SqlBinary)),
                    colGroupmember = new DataColumn(ParamGroupmember, typeof(SqlBinary)),
                    colHashkey = new DataColumn(ParamHashkey, typeof(SqlString)))
            {
                // Create the data table object & define its columns.
                // NOTE : THE ORDER OF THE COLUMNS MUST MATCH WHAT IS IN THE REPOSITORY
                dataTable = new DataTable("serveroswindowsgroupmember");
                dataTable.Columns.AddRange(new DataColumn[] {
                                                                colSnapshotId,
                                                                colGroupsid,
                                                                colGroupmember,
                                                                colHashkey
                                                            });
            }

            return dataTable;
        }

        internal const string ParamSnapshotid = "snapshotid";
        internal const string ParamGroupsid = "groupsid";
        internal const string ParamGroupmember = "groupmember";
        internal const string ParamHashkey = "hashkey";

        internal const string RepositoryTable = "SQLsecure.dbo.serveroswindowsgroupmember";
    }
    internal static class DatabasePrincipalDataTable
    {
        public static DataTable Create()
        {
            DataTable dataTable = null;
            using (DataColumn
                    colSnapshotId = new DataColumn(ParamSnapshotid, typeof(SqlInt32)),
                    colOwner = new DataColumn(ParamOwner, typeof(SqlInt32)),
                    colDbid = new DataColumn(ParamDbid, typeof(SqlInt32)),
                    colUid = new DataColumn(ParamUid, typeof(SqlInt32)),
                    colName = new DataColumn(ParamName, typeof(SqlString)),
                    colUsersid = new DataColumn(ParamUsersid, typeof(SqlBinary)),
                    colType = new DataColumn(ParamType, typeof(SqlString)),
                    colIsalias = new DataColumn(ParamIsalias, typeof(SqlString)),
                    colAltuid = new DataColumn(ParamAltuid, typeof(SqlInt32)),
                    colHasaccess = new DataColumn(ParamHasaccess, typeof(SqlString)),
                    colDefaultschemaname = new DataColumn(ParamDefaultschemaname, typeof(SqlString)),
                    colHashkey = new DataColumn(ParamHashkey, typeof(SqlString)),
                    colIsContained = new DataColumn(ParamIsContained, typeof(SqlBoolean)),
                    colAuthenticationType = new DataColumn(ParamAuthenticationType, typeof(SqlString))
                    )
            {
                // Create the data table object & define its columns.
                // NOTE : THE ORDER OF THE COLUMNS MUST MATCH WHAT IS IN THE REPOSITORY
                dataTable = new DataTable("databaseprincipal");
                dataTable.Columns.AddRange(new DataColumn[] {
                                                                colSnapshotId,
                                                                colDbid,
                                                                colUid,
                                                                colOwner,
                                                                colName,
                                                                colUsersid,
                                                                colType,
                                                                colIsalias,
                                                                colAltuid,
                                                                colHasaccess,
                                                                colDefaultschemaname,
                                                                colHashkey,
                                                                colIsContained,
                                                                colAuthenticationType
                                                            });
            }

            return dataTable;
        }

        internal const string ParamSnapshotid = "snapshotid";
        internal const string ParamOwner = "owner";
        internal const string ParamDbid = "dbid";
        internal const string ParamUid = "uid";
        internal const string ParamName = "name";
        internal const string ParamUsersid = "usersid";
        internal const string ParamType = "type";
        internal const string ParamIsalias = "isalias";
        internal const string ParamAltuid = "altuid";
        internal const string ParamHasaccess = "hasaccess";
        internal const string ParamDefaultschemaname = "defaultschemaname";
        internal const string ParamHashkey = "hashkey";
        internal const string ParamIsContained = "iscontaineduser";
        internal const string ParamAuthenticationType = "AuthenticationType";
        internal const string RepositoryTable = "SQLsecure.dbo.databaseprincipal";
    }

    internal static class DatabaseRoleMemberDataTable
    {
        public static DataTable Create()
        {
            DataTable dataTable = null;
            using (DataColumn
                    colSnapshotId = new DataColumn(ParamSnapshotid, typeof(SqlInt32)),
                    colDbid = new DataColumn(ParamDbid, typeof(SqlInt32)),
                    colGroupuid = new DataColumn(ParamGroupuid, typeof(SqlInt32)),
                    colRolememberuid = new DataColumn(ParamRolememberuid, typeof(SqlInt32)),
                    colHashkey = new DataColumn(ParamHashkey, typeof(SqlString)))
            {
                // Create the data table object & define its columns.
                // NOTE : THE ORDER OF THE COLUMNS MUST MATCH WHAT IS IN THE REPOSITORY
                dataTable = new DataTable("serverprincipal");
                dataTable.Columns.AddRange(new DataColumn[] {
                                                                colSnapshotId,
                                                                colDbid,
                                                                colGroupuid,
                                                                colRolememberuid,
                                                                colHashkey
                                                            });
            }

            return dataTable;
        }

        internal const string ParamDbid = "dbid";
        internal const string ParamGroupuid = "groupuid";
        internal const string ParamRolememberuid = "rolememberuid";
        internal const string ParamSnapshotid = "snapshotid";
        internal const string ParamHashkey = "hashkey";

        internal const string RepositoryTable = "SQLsecure.dbo.databaserolemember";
    }

    internal static class DatabaseObjectDataTable
    {
        public static DataTable Create()
        {
            DataTable dataTable = null;
            using (DataColumn
                    colSnapshotId = new DataColumn(ParamSnapshotid, typeof(SqlInt32)),
                    colType = new DataColumn(ParamType, typeof(SqlString)),
                    colOwner = new DataColumn(ParamOwner, typeof(SqlInt32)),
                    colDbid = new DataColumn(ParamDbid, typeof(SqlInt32)),
                    colClassid = new DataColumn(ParamClassid, typeof(SqlInt32)),
                    colParentobjectid = new DataColumn(ParamParentobjectid, typeof(SqlInt32)),
                    colObjectid = new DataColumn(ParamObjectid, typeof(SqlInt32)),
                    colName = new DataColumn(ParamName, typeof(SqlString)),
                    colSchemaid = new DataColumn(ParamSchemaid, typeof(SqlInt32)),
                    colHashkey = new DataColumn(ParamHashkey, typeof(SqlString)),
                    colRunAtStartup = new DataColumn(ParamRunAtStartup, typeof(SqlString)),
                    colIsEncypted = new DataColumn(ParamIsEncypted, typeof(SqlString)),
                    colUserDefined = new DataColumn(ParamUserDefined, typeof(SqlString)),
                    colPermissionSet = new DataColumn(ParamPermissionSet, typeof(SqlInt32)),
                    colCreateDate = new DataColumn(ParamCreateDate, typeof(SqlDateTime)),
                    colModifyDate = new DataColumn(ParamModifyDate, typeof(SqlDateTime)),
                    colAlwaysEncryptionType = new DataColumn(ParamAlwaysEncryptionType, typeof(SqlInt32)),  // SQLSecure 3.1(Anshul Aggarwal) - New columns for new risk assessments.
                    colSignedCryptType = new DataColumn(ParamSignedCryptType, typeof(SqlString)),
                    colIsDataMasked = new DataColumn(ParamIsDataMasked, typeof(SqlBoolean)),
                    colIsRowSecurityEnabled = new DataColumn(ParamIsRowSecurityEnabled, typeof(SqlBoolean))
                    )
            {
                // Create the data table object & define its columns.
                // NOTE : THE ORDER OF THE COLUMNS MUST MATCH WHAT IS IN THE REPOSITORY
                dataTable = new DataTable("databaseobject");
                dataTable.Columns.AddRange(new DataColumn[] {
                                                                colSnapshotId,
                                                                colDbid,
                                                                colClassid,
                                                                colParentobjectid,
                                                                colObjectid,
                                                                colSchemaid,
                                                                colType,
                                                                colOwner,
                                                                colName,
                                                                colHashkey,
                                                                colRunAtStartup,
                                                                colIsEncypted,
                                                                colUserDefined,
                                                                colPermissionSet,
                                                                colCreateDate,
                                                                colModifyDate,
                                                                colAlwaysEncryptionType,    // SQLSecure 3.1(Anshul Aggarwal) - New columns for new risk assessments.
                                                                colSignedCryptType,
                                                                colIsDataMasked,
                                                                colIsRowSecurityEnabled
                                                            });
            }

            return dataTable;
        }

        internal const string ParamSnapshotid = "snapshotid";
        internal const string ParamType = "type";
        internal const string ParamOwner = "owner";
        internal const string ParamSchemaid = "schemaid";
        internal const string ParamClassid = "classid";
        internal const string ParamDbid = "dbid";
        internal const string ParamParentobjectid = "parentobjectid";
        internal const string ParamObjectid = "objectid";
        internal const string ParamName = "name";
        internal const string ParamHashkey = "hashkey";
        internal const string ParamRunAtStartup = "runatstartup";
        internal const string ParamIsEncypted = "isencrypted";
        internal const string ParamUserDefined = "userdefined";
        internal const string ParamPermissionSet = "permission_set";
        internal const string ParamCreateDate = "createdate";
        internal const string ParamModifyDate = "modifydate";

        // SQLSecure 3.1(Anshul Aggarwal) - New columns for new risk assessments.
        internal const string ParamAlwaysEncryptionType = "alwaysencryptiontype";
        internal const string ParamIsDataMasked = "isdatamasked";
        internal const string ParamSignedCryptType = "signedcrypttype";
        internal const string ParamIsRowSecurityEnabled = "isrowsecurityenabled";

        internal const string RepositoryTable = "SQLsecure.dbo.databaseobject";
    }

    internal static class DatabaseSchemaDataTable
    {
        public static DataTable Create()
        {
            DataTable dataTable = null;
            using (DataColumn
                    colDbid = new DataColumn(ParamDbid, typeof(SqlInt32)),
                    colUid = new DataColumn(ParamUid, typeof(SqlInt32)),
                    colSnapshotId = new DataColumn(ParamSnapshotid, typeof(SqlInt32)),
                    colSchemaid = new DataColumn(ParamSchemaid, typeof(SqlInt32)),
                    colSchemaname = new DataColumn(ParamSchemaname, typeof(SqlString)),
                    colHashkey = new DataColumn(ParamHashkey, typeof(SqlString)))
            {
                // Create the data table object & define its columns.
                // NOTE : THE ORDER OF THE COLUMNS MUST MATCH WHAT IS IN THE REPOSITORY
                dataTable = new DataTable("databaseschema");
                dataTable.Columns.AddRange(new DataColumn[] {
                                                                colSnapshotId,
                                                                colDbid,
                                                                colSchemaid,
                                                                colUid,
                                                                colSchemaname,
                                                                colHashkey
                                                            });
            }

            return dataTable;
        }

        internal const string ParamDbid = "dbid";
        internal const string ParamUid = "uid";
        internal const string ParamSnapshotid = "snapshotid";
        internal const string ParamSchemaid = "schemaid";
        internal const string ParamSchemaname = "schemaname";
        internal const string ParamHashkey = "hashkey";

        internal const string RepositoryTable = "SQLsecure.dbo.databaseschema";
    }

    internal static class DatabaseObjectPermissionDataTable
    {
        public static DataTable Create()
        {
            DataTable dataTable = null;
            using (DataColumn
                    colClassid = new DataColumn(ParamClassid, typeof(SqlInt32)),
                    colObjectid = new DataColumn(ParamObjectid, typeof(SqlInt32)),
                    colParentobjectid = new DataColumn(ParamParentobjectid, typeof(SqlInt32)),
                    colGrantor = new DataColumn(ParamGrantor, typeof(SqlInt32)),
                    colSnapshotId = new DataColumn(ParamSnapshotid, typeof(SqlInt32)),
                    colDbid = new DataColumn(ParamDbid, typeof(SqlInt32)),
                    colPermission = new DataColumn(ParamPermission, typeof(SqlString)),
                    colGrantee = new DataColumn(ParamGrantee, typeof(SqlInt32)),
                    colIsgrant = new DataColumn(ParamIsgrant, typeof(SqlString)),
                    colIsgrantwith = new DataColumn(ParamIsgrantwith, typeof(SqlString)),
                    colIsrevoke = new DataColumn(ParamIsrevoke, typeof(SqlString)),
                    colIsdeny = new DataColumn(ParamIsdeny, typeof(SqlString)),
                    colHashkey = new DataColumn(ParamHashkey, typeof(SqlString)))
            {
                // Create the data table object & define its columns.
                // NOTE : THE ORDER OF THE COLUMNS MUST MATCH WHAT IS IN THE REPOSITORY
                dataTable = new DataTable("databaseobjectpermission");
                dataTable.Columns.AddRange(new DataColumn[] {
                                                                colSnapshotId,
                                                                colDbid,
                                                                colObjectid,
                                                                colParentobjectid,
                                                                colClassid,
                                                                colPermission,
                                                                colGrantee,
                                                                colGrantor,
                                                                colIsgrant,
                                                                colIsgrantwith,
                                                                colIsrevoke,
                                                                colIsdeny,
                                                                colHashkey
                                                            });
            }

            return dataTable;
        }

        internal const string ParamClassid = "classid";
        internal const string ParamObjectid = "objectid";
        internal const string ParamParentobjectid = "parentobjectid";
        internal const string ParamGrantor = "grantor";
        internal const string ParamSnapshotid = "snapshotid";
        internal const string ParamDbid = "dbid";
        internal const string ParamPermission = "permission";
        internal const string ParamGrantee = "grantee";
        internal const string ParamIsgrant = "isgrant";
        internal const string ParamIsgrantwith = "isgrantwith";
        internal const string ParamIsrevoke = "isrevoke";
        internal const string ParamIsdeny = "isdeny";
        internal const string ParamHashkey = "hashkey";

        internal const string RepositoryTable = "SQLsecure.dbo.databaseobjectpermission";
    }

    internal static class DatabasePrincipalPermissionDataTable
    {
        public static DataTable Create()
        {
            DataTable dataTable = null;
            using (DataColumn
                    colDbid = new DataColumn(ParamDbid, typeof(SqlInt32)),
                    colClassid = new DataColumn(ParamClassid, typeof(SqlInt32)),
                    colGrantor = new DataColumn(ParamGrantor, typeof(SqlInt32)),
                    colUid = new DataColumn(ParamUid, typeof(SqlInt32)),
                    colSnapshotId = new DataColumn(ParamSnapshotid, typeof(SqlInt32)),
                    colGrantee = new DataColumn(ParamGrantee, typeof(SqlInt32)),
                    colPermission = new DataColumn(ParamPermission, typeof(SqlString)),
                    colIsgrant = new DataColumn(ParamIsgrant, typeof(SqlString)),
                    colIsgrantwith = new DataColumn(ParamIsgrantwith, typeof(SqlString)),
                    colIsrevoke = new DataColumn(ParamIsrevoke, typeof(SqlString)),
                    colIsdeny = new DataColumn(ParamIsdeny, typeof(SqlString)),
                    colHashkey = new DataColumn(ParamHashkey, typeof(SqlString)))
            {
                // Create the data table object & define its columns.
                // NOTE : THE ORDER OF THE COLUMNS MUST MATCH WHAT IS IN THE REPOSITORY
                dataTable = new DataTable("databaseobjectpermission");
                dataTable.Columns.AddRange(new DataColumn[] {
                                                                colSnapshotId,
                                                                colDbid,
                                                                colUid,
                                                                colClassid,
                                                                colPermission,
                                                                colGrantee,
                                                                colGrantor,
                                                                colIsgrant,
                                                                colIsgrantwith,
                                                                colIsrevoke,
                                                                colIsdeny,
                                                                colHashkey
                                                            });
            }

            return dataTable;
        }

        internal const string ParamDbid = "dbid";
        internal const string ParamClassid = "classid";
        internal const string ParamGrantor = "grantor";
        internal const string ParamUid = "uid";
        internal const string ParamSnapshotid = "snapshotid";
        internal const string ParamGrantee = "grantee";
        internal const string ParamPermission = "permission";
        internal const string ParamIsgrant = "isgrant";
        internal const string ParamIsgrantwith = "isgrantwith";
        internal const string ParamIsrevoke = "isrevoke";
        internal const string ParamIsdeny = "isdeny";
        internal const string ParamHashkey = "hashkey";

        internal const string RepositoryTable = "SQLsecure.dbo.databaseprincipalpermission";
    }

    internal static class DatabaseSchemaPermissionDataTable
    {
        public static DataTable Create()
        {
            DataTable dataTable = null;
            using (DataColumn
                    colSnapshotId = new DataColumn(ParamSnapshotid, typeof(SqlInt32)),
                    colSchemaid = new DataColumn(ParamSchemaid, typeof(SqlInt32)),
                    colGrantor = new DataColumn(ParamGrantor, typeof(SqlInt32)),
                    colGrantee = new DataColumn(ParamGrantee, typeof(SqlInt32)),
                    colDbid = new DataColumn(ParamDbid, typeof(SqlInt32)),
                    colClassid = new DataColumn(ParamClassid, typeof(SqlInt32)),
                    colPermission = new DataColumn(ParamPermission, typeof(SqlString)),
                    colIsgrant = new DataColumn(ParamIsgrant, typeof(SqlString)),
                    colIsgrantwith = new DataColumn(ParamIsgrantwith, typeof(SqlString)),
                    colIsrevoke = new DataColumn(ParamIsrevoke, typeof(SqlString)),
                    colIsdeny = new DataColumn(ParamIsdeny, typeof(SqlString)),
                    colHashkey = new DataColumn(ParamHashkey, typeof(SqlString)))
            {
                // Create the data table object & define its columns.
                // NOTE : THE ORDER OF THE COLUMNS MUST MATCH WHAT IS IN THE REPOSITORY
                dataTable = new DataTable("databaseschemapermission");
                dataTable.Columns.AddRange(new DataColumn[] {
                                                                colSnapshotId,
                                                                colDbid,
                                                                colSchemaid,
                                                                colClassid,
                                                                colPermission,
                                                                colGrantee,
                                                                colGrantor,
                                                                colIsgrant,
                                                                colIsgrantwith,
                                                                colIsrevoke,
                                                                colIsdeny,
                                                                colHashkey
                                                            });
            }

            return dataTable;
        }

        internal const string ParamSnapshotid = "snapshotid";
        internal const string ParamSchemaid = "schemaid";
        internal const string ParamGrantor = "grantor";
        internal const string ParamGrantee = "grantee";
        internal const string ParamDbid = "dbid";
        internal const string ParamClassid = "classid";
        internal const string ParamPermission = "permission";
        internal const string ParamIsgrant = "isgrant";
        internal const string ParamIsgrantwith = "isgrantwith";
        internal const string ParamIsrevoke = "isrevoke";
        internal const string ParamIsdeny = "isdeny";
        internal const string ParamHashkey = "hashkey";

        internal const string RepositoryTable = "SQLsecure.dbo.databaseschemapermission";
    }

    internal static class SQLServicesDataTable
    {
        public static DataTable Create()
        {
            DataTable dataTable = null;
            using (DataColumn
                    colSnapshotId = new DataColumn(ParamSnapshotid, typeof(SqlInt32)),
                    colServiceType = new DataColumn(ParamServiceType, typeof(SqlInt32)),
                    colName = new DataColumn(ParamName, typeof(SqlString)),
                    colDisplayName = new DataColumn(ParamDisplayName, typeof(SqlString)),
                    colServicePath = new DataColumn(ParamServicePath, typeof(SqlString)),
                    colState = new DataColumn(ParamState, typeof(SqlString)),
                    colStartupType = new DataColumn(ParamStartupType, typeof(SqlString)),
                    colLogonName = new DataColumn(ParamLogonName, typeof(SqlString)))
            {
                // Create the data table object & define its columns.
                // NOTE : THE ORDER OF THE COLUMNS MUST MATCH WHAT IS IN THE REPOSITORY
                dataTable = new DataTable("serverservice");
                dataTable.Columns.AddRange(new DataColumn[] {
                                                                colSnapshotId,
                                                                colServiceType,
                                                                colName,
                                                                colDisplayName,
                                                                colServicePath,
                                                                colStartupType,
                                                                colState,
                                                                colLogonName
                                                            });
            }

            return dataTable;
        }

        internal const string ParamSnapshotid = "snapshotid";
        internal const string ParamServiceType = "servicetype";
        internal const string ParamName = "servicename";
        internal const string ParamDisplayName = "displayname";
        internal const string ParamServicePath = "servicepath";
        internal const string ParamStartupType = "startuptype";
        internal const string ParamState = "state";
        internal const string ParamLogonName = "loginname";

        internal const string RepositoryTable = "SQLsecure.dbo.serverservice";
    }

    internal static class SQLServerObjectTable
    {
        public static DataTable Create()
        {
            DataTable dataTable = null;
            using (DataColumn
                    colSnapshotId = new DataColumn(ParamSnapshotid, typeof(SqlInt32)),
                    osObjectId = new DataColumn(ParamObjectId, typeof(SqlInt32)),
                    colObjectType = new DataColumn(ParamObjectType, typeof(SqlString)),
                    colDbId = new DataColumn(ParamDbId, typeof(SqlInt32)),
                    colObjectName = new DataColumn(ParamObjectName, typeof(SqlString)),
                    colLongName = new DataColumn(ParamLongName, typeof(SqlString)),
                    colOwnerSid = new DataColumn(ParamOwnerSid, typeof(SqlBinary)),
                    colDiskType = new DataColumn(ParamDiskType, typeof(SqlString)))
            {
                // Create the data table object & define its columns.
                // NOTE : THE ORDER OF THE COLUMNS MUST MATCH WHAT IS IN THE REPOSITORY
                dataTable = new DataTable("serverosobject");
                dataTable.Columns.AddRange(new DataColumn[] {
                                                                colSnapshotId,
                                                                osObjectId,
                                                                colObjectType,
                                                                colDbId,
                                                                colObjectName,
                                                                colLongName,
                                                                colOwnerSid,
                                                                colDiskType
                                                            });
            }

            return dataTable;
        }

        internal const string ParamSnapshotid = "snapshotid";
        internal const string ParamObjectId = "osobjectid";
        internal const string ParamObjectType = "objecttype";
        internal const string ParamDbId = "dbid";
        internal const string ParamObjectName = "objectname";
        internal const string ParamLongName = "longname";
        internal const string ParamOwnerSid = "ownersid";
        internal const string ParamDiskType = "disktype";

        internal const string RepositoryTable = "SQLsecure.dbo.serverosobject";
    }

    internal static class SQLServerObjectPermissionTable
    {
        public static DataTable Create()
        {
            DataTable dataTable = null;
            using (DataColumn
                    colSnapshotId = new DataColumn(ParamSnapshotid, typeof(SqlInt32)),
                    osObjectId = new DataColumn(ParamObjectId, typeof(SqlInt32)),
                    colAuditFlags = new DataColumn(ParamAuditFlags, typeof(SqlInt32)),
                    colFileSystemRights = new DataColumn(ParamFileSystemRights, typeof(SqlInt32)),
                    colSID = new DataColumn(ParamSID, typeof(SqlBinary)),
                    colAccessType = new DataColumn(ParamAccessType, typeof(SqlInt32)),
                    colIsInherited = new DataColumn(ParamIsInherited, typeof(SqlString)))
            {
                // Create the data table object & define its columns.
                // NOTE : THE ORDER OF THE COLUMNS MUST MATCH WHAT IS IN THE REPOSITORY
                dataTable = new DataTable("serverosobjectpermission");
                dataTable.Columns.AddRange(new DataColumn[] {
                                                                colSnapshotId,
                                                                osObjectId,
                                                                colAuditFlags,
                                                                colFileSystemRights,
                                                                colSID,
                                                                colAccessType,
                                                                colIsInherited
                                                            });
            }

            return dataTable;
        }

        internal const string ParamSnapshotid = "snapshotid";
        internal const string ParamObjectId = "osobjectid";
        internal const string ParamAuditFlags = "auditflags";
        internal const string ParamFileSystemRights = "filesystemrights";
        internal const string ParamSID = "sid";
        internal const string ParamAccessType = "accesstype";
        internal const string ParamIsInherited = "isinherited";

        internal const string RepositoryTable = "SQLsecure.dbo.serverosobjectpermission";
    }


    internal static class SQLserverprotocolTable
    {
        public static DataTable Create()
        {
            DataTable dataTable = null;
            using (DataColumn
                    colSnapshotId = new DataColumn(ParamSnapshotid, typeof(SqlInt32)),
                    colProtocolName = new DataColumn(ParamProtocolName, typeof(SqlString)),
                    colIPAddress = new DataColumn(ParamIPAddress, typeof(SqlString)),
                    colDynamicPort = new DataColumn(ParamDynamicPort, typeof(SqlString)),
                    colPort = new DataColumn(ParamPort, typeof(SqlString)),
                    colEnabled = new DataColumn(ParamEnabled, typeof(SqlString)),
                    colActive = new DataColumn(ParamActive, typeof(SqlString)))
            {
                // Create the data table object & define its columns.
                // NOTE : THE ORDER OF THE COLUMNS MUST MATCH WHAT IS IN THE REPOSITORY
                dataTable = new DataTable("serverprotocol");
                dataTable.Columns.AddRange(new DataColumn[] {
                                                                colSnapshotId,
                                                                colProtocolName,
                                                                colIPAddress,
                                                                colDynamicPort,
                                                                colPort,
                                                                colEnabled,
                                                                colActive
                                                            });
            }

            return dataTable;
        }

        internal const string ParamSnapshotid = "snapshotid";
        internal const string ParamProtocolName = "protocolname";
        internal const string ParamIPAddress = "ipaddress";
        internal const string ParamDynamicPort = "dynamicport";
        internal const string ParamPort = "port";
        internal const string ParamEnabled = "enabled";
        internal const string ParamActive = "active";

        internal const string RepositoryTable = "SQLsecure.dbo.serverprotocol";
    }

    internal static class SqlJobDataTable
    {
        public static DataTable Create()
        {
            DataTable dataTable = null;
            using (DataColumn
                    colJobId = new DataColumn(Paramjobid, typeof(SqlInt32)),
                    colSnapshotid = new DataColumn(ParamSnapshotId, typeof(SqlInt32)),
                    colName = new DataColumn(ParamName, typeof(SqlString)),
                    colDescription = new DataColumn(ParamDescription, typeof(SqlString)),
                    colStepName = new DataColumn(ParamStepName, typeof(SqlString)),
                    colLastRunDate = new DataColumn(ParamLastRunDate, typeof(SqlDateTime)),
                    colComand = new DataColumn(ParamCommand, typeof(SqlString)),
                    colSubSystem = new DataColumn(ParamSubSystem, typeof(SqlString)),
                    colOwnerSid = new DataColumn(ParamOwnerSid, typeof(SqlBinary)),
                    colEnabled = new DataColumn(ParamEnabled, typeof(SqlInt16)),
                    colProxyId = new DataColumn(Paramproxyid, typeof(SqlInt32)))
            {
                // Create the data table object & define its columns.
                // NOTE : THE ORDER OF THE COLUMNS MUST MATCH WHAT IS IN THE REPOSITORY
                dataTable = new DataTable("sqljob");
                dataTable.Columns.AddRange(new DataColumn[] {
                                                                colJobId,
                                                                colSnapshotid,
                                                                colName,
                                                                colDescription,
                                                                colStepName,
                                                                colLastRunDate,
                                                                colComand,
                                                                colSubSystem,
                                                                colOwnerSid,
                                                                colEnabled,
                                                                colProxyId
                                                            });
            }

            return dataTable;
        }

        internal const string Paramjobid = "jobid";
        internal const string ParamName = "name";
        internal const string ParamOwnerSid = "ownersid";
        internal const string ParamDescription = "description";
        internal const string ParamLastRunDate = "lastrundate";
        internal const string ParamEnabled = "enabled";
        internal const string ParamSubSystem = "subsystem";
        internal const string ParamStepName = "stepname";
        internal const string ParamCommand = "command";
        internal const string ParamSnapshotId = "SnapshotId";
        internal const string Paramproxyid = "ProxyId";


        internal const string RepositoryTable = "SQLsecure.dbo.sqljob";
    }

    internal static class SQLJobProxy
    {
        public static DataTable Create()
        {
            DataTable dataTable = null;
            using (DataColumn
                    colProxyId = new DataColumn(ParamProxyId, typeof(SqlInt32)),
                    colSnapshotid = new DataColumn(ParamSnapshotId, typeof(SqlInt32)),
                    colName = new DataColumn(ParamName, typeof(SqlString)),
                    colSubSystemId = new DataColumn(ParamSubSystemId, typeof(SqlInt32)),
                    colUserId = new DataColumn(ParamUserSid, typeof(SqlBinary)),
                    colSubSystem = new DataColumn(ParamSubSystem, typeof(SqlString)),
                    colCredentialId = new DataColumn(ParamCredentialId, typeof(SqlInt32)),
                    colCredentialName = new DataColumn(ParamCredentialName, typeof(SqlString)),
                    colCredentialIdentity = new DataColumn(ParamCredentialIdentity, typeof(SqlString)),
                    colEnabled = new DataColumn(ParamEnabled, typeof(SqlByte)))
            {
                // Create the data table object & define its columns.
                // NOTE : THE ORDER OF THE COLUMNS MUST MATCH WHAT IS IN THE REPOSITORY
                dataTable = new DataTable("sqljobproxy");
                dataTable.Columns.AddRange(new DataColumn[] {
                                                                colProxyId,
                                                                colSnapshotid,
                                                                colName,
                                                                colEnabled,
                                                                colUserId,
                                                                colSubSystemId,
                                                                colSubSystem,
                                                                colCredentialId,
                                                                colCredentialName,
                                                                colCredentialIdentity
                                                            });
            }

            return dataTable;
        }

        internal const string ParamProxyId = "proxyid";
        internal const string ParamSnapshotId = "snapshotid";
        internal const string ParamName = "proxyName";
        internal const string ParamUserSid = "usersid";
        internal const string ParamSubSystemId = "subsystemid";
        internal const string ParamSubSystem = "subsystem";
        internal const string ParamEnabled = "enabled";
        internal const string ParamCredentialId = "credentialId";
        internal const string ParamCredentialName = "credentialName";
        internal const string ParamCredentialIdentity = "credentialIdentity";

        internal const int ColProxyId = 0;
        internal const int ColName = 1;
        internal const int ColCredentialId = 2;
        internal const int ColEnabled = 3;
        internal const int ColUserSid = 4;
        internal const int ColSubSystemId = 5;
        internal const int ColSubSystem = 6;
        internal const int ColCredentialName = 7;
        internal const int ColCredentialIdentity = 8;


        internal const string RepositoryTable = "SQLsecure.dbo.sqljobproxy";
    }


    internal static class AvailabilityGroupTable
    {
        public static DataTable Create()
        {
            DataTable dataTable = null;
            using (DataColumn
                   colGroupId = new DataColumn(ParamGroupId, typeof(SqlGuid)),
                   colName = new DataColumn(ParamName, typeof(SqlString)),
                   colResourceId = new DataColumn(ParamResourceId, typeof(SqlString)),
                   colResourceGroupId = new DataColumn(ParamResourceGroupId, typeof(SqlString)),
                   colFailureConditionLevel = new DataColumn(ParamFailureConditionLevel, typeof(SqlInt32)),
                   colHealthCheckTimeout = new DataColumn(ParamHealthCheckTimeout, typeof(SqlInt32)),
                   colAutomatedBackuppReference = new DataColumn(ParamAutomatedBackuppReference, typeof(SqlByte)),
                   colAutomatedBackuppReferenceDesc = new DataColumn(ParamAutomatedBackuppReferenceDesc, typeof(SqlString)),
                      colSnapshotid = new DataColumn(ParamSnapshotid, typeof(SqlInt32)))
            {
                // Create the data table object & define itsParamumns.
                // NOTE : THE ORDER OF THEParamUMNS MUST MATCH WHAT IS IN THE REPOSITORY
                dataTable = new DataTable("availabilitygroups");
                dataTable.Columns.AddRange(new DataColumn[] {
                                                               colGroupId,
                                                               colName,
                                                               colResourceId,
                                                               colResourceGroupId,
                                                               colFailureConditionLevel,
                                                               colHealthCheckTimeout,
                                                               colAutomatedBackuppReference,
                                                               colAutomatedBackuppReferenceDesc,
                                                               colSnapshotid
                                                            });
            }

            return dataTable;
        }


        internal const string ParamGroupId = "[groupid]";
        internal const string ParamSnapshotid = "[snapshotid]";
        internal const string ParamName = "[name]";
        internal const string ParamResourceId = "[resourceid]";
        internal const string ParamResourceGroupId = "[resourcegroupid]";
        internal const string ParamFailureConditionLevel = "[failureconditionlevel]";
        internal const string ParamHealthCheckTimeout = "[healthchecktimeout]";
        internal const string ParamAutomatedBackuppReference = "[automatedbackuppreference]";
        internal const string ParamAutomatedBackuppReferenceDesc = "[automatedbackuppreferencedesc]";






        internal const string RepositoryTable = "SQLsecure.dbo.availabilitygroups";
    }

    internal static class AvailabilityReplicas
    {
        public static DataTable Create()
        {
            DataTable dataTable = null;
            using (DataColumn
                            colReplicaid = new DataColumn(ParamReplicaId, typeof(SqlGuid)),
                            colGroupid = new DataColumn(ParamGroupid, typeof(SqlGuid)),
                            colSnapshotid = new DataColumn(ParamSnapshotid, typeof(SqlInt32)),
                            colReplicaServerName = new DataColumn(ParamReplicaServerName, typeof(SqlString)),
                            colOwnersid = new DataColumn(ParamOwnersid, typeof(SqlBinary)),
                            colEndpointUrl = new DataColumn(ParamEndpointUrl, typeof(SqlString)),
                            colAvailabilityMode = new DataColumn(ParamAvailabilityMode, typeof(SqlByte)),
                            colAvailabilityModeDesc = new DataColumn(ParamAvailabilityModeDesc, typeof(SqlString)),
                            colFailovermode = new DataColumn(ParamFailoverMode, typeof(SqlByte)),
                            colFailoverModeDesc = new DataColumn(ParamFailoverModeDesc, typeof(SqlString)),
                            colCreateDate = new DataColumn(ParamCreateDate, typeof(SqlDateTime)),
                            colModifyDate = new DataColumn(ParamModifyDate, typeof(SqlDateTime)),
                            colReplicaMetadataId = new DataColumn(ParamReplicaMetadataId, typeof(SqlInt32))
                            )
            {
                // Create the data table object & define itsParamumns.
                // NOTE : THE ORDER OF THEParamUMNS MUST MATCH WHAT IS IN THE REPOSITORY
                dataTable = new DataTable("availabilityreplicas");
                dataTable.Columns.AddRange(new DataColumn[] {
                                                                colReplicaid ,
                                                                colSnapshotid ,
                                                                colGroupid ,
                                                                colReplicaServerName ,
                                                                colOwnersid ,
                                                                colEndpointUrl ,
                                                                colAvailabilityMode ,
                                                                colAvailabilityModeDesc ,
                                                                colFailovermode ,
                                                                colFailoverModeDesc ,
                                                                colCreateDate ,
                                                                colModifyDate ,
                                                                colReplicaMetadataId
                                                             });
            }

            return dataTable;
        }




        internal const string ParamReplicaId = "replicaid";
        internal const string ParamServerReplicaId = "serverreplicaid";
        internal const string ParamSnapshotid = "snapshotid";
        internal const string ParamGroupid = "groupid";
        internal const string ParamReplicaServerName = "replicaservername";
        internal const string ParamOwnersid = "ownersid";
        internal const string ParamEndpointUrl = "endpointurl";
        internal const string ParamAvailabilityMode = "availabilitymode";
        internal const string ParamAvailabilityModeDesc = "availabilitymodedesc";
        internal const string ParamFailoverMode = "failovermode";
        internal const string ParamFailoverModeDesc = "failovermodedesc";
        internal const string ParamCreateDate = "createdate";
        internal const string ParamModifyDate = "modifydate";
        internal const string ParamReplicaMetadataId = "replicametadataid";



        internal const int ColReplicaid = 0;
        internal const int ColGroupId = 1;
        internal const int ColReplicaServerName = 2;
        internal const int ColOwnersid = 3;
        internal const int ColEndpointUrl = 4;
        internal const int ColAvailabilityMode = 5;
        internal const int ColAvailabilityModeDesc = 6;
        internal const int ColFailoverMode = 7;
        internal const int ColFailoverModeDesc = 8;
        internal const int ColCreateDate = 9;
        internal const int ColModifyDate = 10;
        internal const int ColReplicaMetadataId = 11;



        internal const string RepositoryTable = "SQLsecure.dbo.availabilityreplicas";
    }


    internal static class LinkedServersDataTable
    {
        public static DataTable Create()
        {
            DataTable dataTable;

            using (DataColumn colSnapshotId = new DataColumn(ParamSnapshotId, typeof (SqlInt32)),
                colServerId = new DataColumn(ParamServerId, typeof (SqlInt32)),
                colServerName = new DataColumn(ParamServerName, typeof (SqlString)))
            {
                // Create the data table object & define itsParamumns.
                // NOTE : THE ORDER OF THEParamUMNS MUST MATCH WHAT IS IN THE REPOSITORY
                dataTable = new DataTable("linkedserver");
                dataTable.Columns.AddRange(new DataColumn[]
                {
                    colSnapshotId,
                    colServerId,
                    colServerName
                });

            }


            return dataTable;
        }


        internal const string ParamSnapshotId = "snapshotid";
        internal const string ParamServerId = "serverid";
        internal const string ParamServerName = "servername";

        internal const int ColServerId = 0;
        internal const int ColServerName = 1;
        internal const int ColSnapshotId = 2;

        internal const string RepositoryTable = "SQLsecure.dbo.linkedserver";
    }

    internal static class LinkedServerPrincipalDataTable
    {
        public static DataTable Create()
        {
            DataTable dataTable;
            using (DataColumn colId = new DataColumn(Id, typeof(SqlInt32)), 
                colSnapshotId = new DataColumn(ParamSnapshotId, typeof(SqlInt32)),
                colServerId = new DataColumn(ParamServerId, typeof(SqlInt32)),
                colPrincipalName = new DataColumn(ParamPrincipalName, typeof (SqlString)))
            {
                // Create the data table object & define itsParamumns.
                // NOTE : THE ORDER OF THEParamUMNS MUST MATCH WHAT IS IN THE REPOSITORY
                dataTable = new DataTable("linkedserverprincipal");
                dataTable.Columns.AddRange(new DataColumn[]
                {
                    colId,
                    colSnapshotId,
                    colServerId,
                    colPrincipalName
                });

            }

            return dataTable;
        }

        internal const string Id = "lspid";
        internal const string ParamSnapshotId = "snapshotid";
        internal const string ParamServerId = "serverid";
        internal const string ParamPrincipalName = "principal";

        internal const int ColServerId = 0;
        internal const int ColServerName = 1;
        internal const int ColPrincipalName = 2;

        internal const string RepositoryTable = "SQLsecure.dbo.linkedserverprincipal";
    }

    internal static class EncryptionKeyDataTable
    {
        public static DataTable Create()
        {
            DataTable dataTable = null;
            using (DataColumn colKeyId = new DataColumn(ParamKeyId, typeof(SqlInt32)),
                              colPrincipalId = new DataColumn(ParamPrincipalId, typeof(SqlInt32)),
                              colName = new DataColumn(ParamName, typeof(SqlString)),
                              colDbKeyId = new DataColumn(ParamDbKeyId, typeof(SqlInt32)),
                              colKeyLength = new DataColumn(ParamKeyLength, typeof(SqlInt32)),
                              colAlgorithm = new DataColumn(ParamAlgorithm, typeof(SqlString)),
                              colAlgorithmDesc = new DataColumn(ParamAlgorithmDesc, typeof(SqlString)),
                              colProviderType = new DataColumn(ParamProviderType, typeof(SqlString)),
                              colDbId = new DataColumn(ParamDatabaseId, typeof(SqlInt32)),
                              colSnapshotId = new DataColumn(ParamSnapshotId, typeof(SqlInt32)),
                              colType = new DataColumn(ParamType, typeof(SqlString)))
            {
                // Create the data table object & define itsParamumns.
                // NOTE : THE ORDER OF THEParamUMNS MUST MATCH WHAT IS IN THE REPOSITORY
                dataTable = new DataTable("encryptionkey");
                dataTable.Columns.AddRange(new DataColumn[] {
                                                              colKeyId,
                                                              colName,
                                                              colPrincipalId,
                                                              colDbKeyId,
                                                              colKeyLength,
                                                              colAlgorithm,
                                                              colAlgorithmDesc,
                                                              colProviderType,
                                                              colSnapshotId,
                                                              colDbId,
                                                              colType
                                                             });

            }
            //todo add database id 
            return dataTable;
        }




        internal const string ParamKeyId = "keyid";
        internal const string ParamName = "name";
        internal const string ParamPrincipalId = "principalid";
        internal const string ParamDbKeyId = "dbkeyid";
        internal const string ParamKeyLength = "keylength";
        internal const string ParamAlgorithm = "algorithm";
        internal const string ParamAlgorithmDesc = "algorithmdesc";
        internal const string ParamProviderType = "providertype";
        internal const string ParamDatabaseId = "databaseid";
        internal const string ParamSnapshotId = "snapshotid";
        internal const string ParamType = "key_type";






        internal const int ColKeyId = 0;
        internal const int ColName = 1;
        internal const int ColPrincipalId = 2;
        internal const int ColDbKeyId = 3;
        internal const int ColKeyLength = 4;
        internal const int ColAlgorithm = 5;
        internal const int ColAlgorithmDesc = 6;
        internal const int ColProviderType = 7;
        internal const int ColSnapshotId = 8;
        internal const int ColType = 9;





        internal const string RepositoryTable = "SQLsecure.dbo.encryptionkey";
    }

    internal static class CertificateTable
    {
        internal const string RepositoryTable = "SQLsecure.dbo.databasecertificates";

        internal const string CertId = "certid";
        internal const string SnapshotId = "snapshotid";
        internal const string DbId = "dbid";
        internal const string CertificateName = "name";
        internal const string CertificateId = "certificate_id";
        internal const string PrincipalId = "principal_id";
        internal const string KeyEncryptionType = "pvt_key_encryption_type";
        internal const string KeyEncryptionTypeDesc = "pvt_key_encryption_type_desc";
        internal const string IsActiveForBeginDialog = "is_active_for_begin_dialog";
        internal const string IssuerName = "issuer_name";
        internal const string CertSerialNumber = "cert_serial_number";
        internal const string Sid = "sid";
        internal const string StringSid = "string_sid";
        internal const string Subject = "subject";
        internal const string ExpiryDate = "expiry_date";
        internal const string StartDate = "start_date";
        internal const string Thumbprint = "thumbprint";
        internal const string AttestedBy = "attested_by";
        internal const string KeyLastBackupDate = "pvt_key_last_backup_date";



        public static DataTable Create()
        {
            DataTable dataTable;
            using (DataColumn
                certId = new DataColumn(CertId, typeof(SqlInt32)),
                snapshotId = new DataColumn(SnapshotId, typeof(SqlInt32)),
                dbId = new DataColumn(DbId, typeof(SqlInt32)),
                certificateName = new DataColumn(CertificateName, typeof(SqlString)),
                certificateId = new DataColumn(CertificateId, typeof(SqlInt32)),
                principalId = new DataColumn(PrincipalId, typeof(SqlInt32)),
                keyEncryptionType = new DataColumn(KeyEncryptionType, typeof(SqlString)),
                keyEncryptionTypeDesc = new DataColumn(KeyEncryptionTypeDesc, typeof(SqlString)),
                isActiveForBeginDialog = new DataColumn(IsActiveForBeginDialog, typeof(SqlBoolean)),
                issuerName = new DataColumn(IssuerName, typeof(SqlString)),
                certSerialNumber = new DataColumn(CertSerialNumber, typeof(SqlString)),
                sid = new DataColumn(Sid, typeof(SqlBinary)),
                stringSid = new DataColumn(StringSid, typeof(SqlString)),
                subject = new DataColumn(Subject, typeof(SqlString)),
                expiryDate = new DataColumn(ExpiryDate, typeof(SqlDateTime)),
                startDate = new DataColumn(StartDate, typeof(SqlDateTime)),
               thumbprint = new DataColumn(Thumbprint, typeof(SqlBinary)),
                attestedBy = new DataColumn(AttestedBy, typeof(SqlString)),
                keyLastBackupDate = new DataColumn(KeyLastBackupDate, typeof(SqlDateTime)))
            {
                dataTable = new DataTable("databasecertificates");
                dataTable.Columns.AddRange(new DataColumn[]
                {
                    certId,
                    snapshotId,
                    dbId,
                    certificateName,
                    certificateId,
                    principalId,
                    keyEncryptionType,
                    keyEncryptionTypeDesc,
                    isActiveForBeginDialog,
                    issuerName,
                    certSerialNumber,
                    sid,
                    stringSid,
                    subject,
                    expiryDate,
                    startDate,
                    thumbprint,
                    attestedBy,
                    keyLastBackupDate
                });
            }


            return dataTable;

        }
    }

    /// <summary>
    /// SQLSecure 3.1(Anshul Aggarwal) - DataTable for Azure Sql DM Firewall rules collectes for new risk assessment "Server-level 
    /// Firewall Rules" and "Database-level Firewall Rules".
    /// </summary>
    internal static class AzureSqlDBFirewallRulesObjectTable
    {
        public static DataTable Create()
        {
            DataTable dataTable = null;
            using (DataColumn
                    colSnapshotId = new DataColumn(ParamSnapshotid, typeof(SqlInt32)),
                    colName = new DataColumn(ParamName, typeof(SqlString)),
                    colDBId = new DataColumn(ParamDBId, typeof(SqlInt32)),
                    colIsServerLevel = new DataColumn(ParamIsServerLevel, typeof(SqlBoolean)),
                    colStartIPAddress = new DataColumn(ParamStartIPAddress, typeof(SqlString)),
                    colEndIPAddress = new DataColumn(ParamEndIPAddress, typeof(SqlString)))
            {
                // Create the data table object & define its columns.
                // NOTE : THE ORDER OF THE COLUMNS MUST MATCH WHAT IS IN THE REPOSITORY
                dataTable = new DataTable("azuresqldbfirewallrules");
                dataTable.Columns.AddRange(new DataColumn[] {
                                                            colSnapshotId,
                                                            colDBId,
                                                            colIsServerLevel,
                                                            colName,
                                                            colStartIPAddress,
                                                            colEndIPAddress
                                                        });
            }

            return dataTable;
        }

        internal const string ParamSnapshotid = "snapshotid";
        internal const string ParamDBId = "dbid";
        internal const string ParamIsServerLevel = "isserverlevel";
        internal const string ParamName = "name";
        internal const string ParamStartIPAddress = "startipaddress";
        internal const string ParamEndIPAddress = "endipaddress";

        internal const string RepositoryTable = "SQLsecure.dbo.azuresqldbfirewallrules";
    }


}
