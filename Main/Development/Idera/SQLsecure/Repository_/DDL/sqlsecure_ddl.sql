   -- Idera SQLsecure Version 0.9
   --
   -- (c) Copyright 2004-2006 Idera, a division of BBS Technologies, Inc., all rights reserved.
   -- SQLsecure, Idera and the Idera Logo are trademarks or registered trademarks
   -- of BBS Technologies or its subsidiaries in the United States and other jurisdictions.
   -- 
   -- Description :
   --              Creates the SQLsecure database if does not exist. 
   --              Also, creates all the objects in the repository.


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

if not exists (select * from master.dbo.sysdatabases where upper(name) = N'SQLSECURE')
begin
	create database [SQLsecure]
end

GO


USE SQLsecure
GO

-- create user view role

sp_addrole 'SQLSecureView'

/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v4.1.0                     */
/* Target DBMS:           MS SQL Server 2000                              */
/* Project file:          sqlsecure.dez                                   */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Database creation script                        */
/* Created on:            2006-09-18 09:29                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Tables                                                                 */
/* ---------------------------------------------------------------------- */

/* ---------------------------------------------------------------------- */
/* Add table "ancillarywindowsgroup"                                      */
/* ---------------------------------------------------------------------- */

CREATE TABLE [ancillarywindowsgroup] (
    [snapshotid] INTEGER NOT NULL,
    [windowsgroupname] NVARCHAR(400) NOT NULL,
    [comment] NVARCHAR(1000),
    CONSTRAINT [PK__ancillarywindows__534D60F1] PRIMARY KEY ([snapshotid], [windowsgroupname])
)
GO

/* ---------------------------------------------------------------------- */
/* Add table "applicationactivity"                                        */
/* ---------------------------------------------------------------------- */

CREATE TABLE [applicationactivity] (
    [eventid] INTEGER IDENTITY(1,1) NOT NULL,
    [eventtimestamp] DATETIME NOT NULL,
    [activitytype] NVARCHAR(256) NOT NULL,
    [applicationsource] NVARCHAR(64),
    [connectionname] NVARCHAR(400),
    [serverlogin] NVARCHAR(500) NOT NULL,
    [eventcode] NVARCHAR(64) NOT NULL,
    [category] NVARCHAR(64) NOT NULL,
    [description] NVARCHAR(1000),
    CONSTRAINT [PK__applicationactiv__1B0907CE] PRIMARY KEY ([eventid])
)
GO

/* ---------------------------------------------------------------------- */
/* Add table "applicationlicense"                                         */
/* ---------------------------------------------------------------------- */

CREATE TABLE [applicationlicense] (
    [licenseid] INTEGER IDENTITY(1,1) NOT NULL,
    [licensekey] NVARCHAR(256) NOT NULL,
    [createdby] NVARCHAR(500) NOT NULL,
    [createdtm] DATETIME NOT NULL,
    CONSTRAINT [PK__applicationlicen__46E78A0C] PRIMARY KEY ([licenseid])
)
GO

/* ---------------------------------------------------------------------- */
/* Add table "classtype"                                                  */
/* ---------------------------------------------------------------------- */

CREATE TABLE [classtype] (
    [classid] INTEGER NOT NULL,
    [classname] NVARCHAR(500) NOT NULL,
    [hashkey] NVARCHAR(256),
    CONSTRAINT [PK__classtype__023D5A04] PRIMARY KEY ([classid])
)
GO

/* ---------------------------------------------------------------------- */
/* Add table "compatibleversion"                                          */
/* ---------------------------------------------------------------------- */

CREATE TABLE [compatibleversion] (
    [objecttype] NVARCHAR(16) NOT NULL,
    [compatibleversion] INTEGER NOT NULL,
    CONSTRAINT [PK__compatibleversio__1CF15040] PRIMARY KEY ([objecttype], [compatibleversion])
)
GO

/* ---------------------------------------------------------------------- */
/* Add table "currentversion"                                             */
/* ---------------------------------------------------------------------- */

CREATE TABLE [currentversion] (
    [schemaversion] INTEGER NOT NULL,
    [dalversion] INTEGER NOT NULL,
    CONSTRAINT [PK__currentversion__1ED998B2] PRIMARY KEY ([schemaversion], [dalversion])
)
GO

/* ---------------------------------------------------------------------- */
/* Add table "databaseobject"                                             */
/* ---------------------------------------------------------------------- */

CREATE TABLE [databaseobject] (
    [snapshotid] INTEGER NOT NULL,
    [dbid] INTEGER NOT NULL,
    [classid] INTEGER NOT NULL,
    [parentobjectid] INTEGER NOT NULL,
    [objectid] INTEGER NOT NULL,
    [schemaid] INTEGER,
    [type] NVARCHAR(5) NOT NULL,
    [owner] INTEGER,
    [name] NVARCHAR(128) NOT NULL,
    [hashkey] NVARCHAR(256),
    CONSTRAINT [PK__databaseobject__38996AB5] PRIMARY KEY ([snapshotid], [dbid], [classid], [parentobjectid], [objectid])
)
GO

/* ---------------------------------------------------------------------- */
/* Add table "databaseobjectpermission"                                   */
/* ---------------------------------------------------------------------- */

CREATE TABLE [databaseobjectpermission] (
    [snapshotid] INTEGER NOT NULL,
    [dbid] INTEGER NOT NULL,
    [objectid] INTEGER NOT NULL,
    [parentobjectid] INTEGER NOT NULL,
    [classid] INTEGER NOT NULL,
    [permission] NVARCHAR(128) NOT NULL,
    [grantee] INTEGER NOT NULL,
    [grantor] INTEGER,
    [isgrant] NCHAR(1) NOT NULL,
    [isgrantwith] NCHAR(1) NOT NULL,
    [isrevoke] NCHAR(1) NOT NULL,
    [isdeny] NCHAR(1) NOT NULL,
    [hashkey] NVARCHAR(256),
    CONSTRAINT [PK__databaseobjectpe__3F466844] PRIMARY KEY ([snapshotid], [dbid], [objectid], [parentobjectid], [classid], [permission], [grantee])
)
GO

/* ---------------------------------------------------------------------- */
/* Add table "databaseprincipal"                                          */
/* ---------------------------------------------------------------------- */

CREATE TABLE [databaseprincipal] (
    [snapshotid] INTEGER NOT NULL,
    [dbid] INTEGER NOT NULL,
    [uid] INTEGER NOT NULL,
    [owner] INTEGER,
    [name] NVARCHAR(128) NOT NULL,
    [usersid] VARBINARY(85),
    [type] NCHAR(1) NOT NULL,
    [isalias] NCHAR(1) NOT NULL,
    [altuid] INTEGER,
    [hasaccess] NCHAR(1) NOT NULL,
    [defaultschemaname] NVARCHAR(128),
    [hashkey] NVARCHAR(256),
    CONSTRAINT [PK__databaseprincipa__7E6CC920] PRIMARY KEY ([snapshotid], [dbid], [uid])
)
GO

/* ---------------------------------------------------------------------- */
/* Add table "databaseprincipalpermission"                                */
/* ---------------------------------------------------------------------- */

CREATE TABLE [databaseprincipalpermission] (
    [snapshotid] INTEGER NOT NULL,
    [dbid] INTEGER NOT NULL,
    [uid] INTEGER NOT NULL,
    [classid] INTEGER NOT NULL,
    [permission] NVARCHAR(128) NOT NULL,
    [grantee] INTEGER NOT NULL,
    [grantor] INTEGER,
    [isgrant] NCHAR(1) NOT NULL,
    [isgrantwith] NCHAR(1) NOT NULL,
    [isrevoke] NCHAR(1) NOT NULL,
    [isdeny] NCHAR(1) NOT NULL,
    [hashkey] NVARCHAR(256),
    CONSTRAINT [PK__databaseprincipa__0CBAE877] PRIMARY KEY ([snapshotid], [dbid], [uid], [classid], [permission], [grantee])
)
GO

/* ---------------------------------------------------------------------- */
/* Add table "databaserolemember"                                         */
/* ---------------------------------------------------------------------- */

CREATE TABLE [databaserolemember] (
    [snapshotid] INTEGER NOT NULL,
    [dbid] INTEGER NOT NULL,
    [groupuid] INTEGER NOT NULL,
    [rolememberuid] INTEGER NOT NULL,
    [hashkey] NVARCHAR(256),
    CONSTRAINT [PK__databaserolememb__32E0915F] PRIMARY KEY ([snapshotid], [dbid], [groupuid], [rolememberuid])
)
GO

/* ---------------------------------------------------------------------- */
/* Add table "databaseschema"                                             */
/* ---------------------------------------------------------------------- */

CREATE TABLE [databaseschema] (
    [snapshotid] INTEGER NOT NULL,
    [dbid] INTEGER NOT NULL,
    [schemaid] INTEGER NOT NULL,
    [uid] INTEGER,
    [schemaname] NVARCHAR(128) NOT NULL,
    [hashkey] NVARCHAR(256),
    CONSTRAINT [PK__databaseschema__0425A276] PRIMARY KEY ([snapshotid], [dbid], [schemaid])
)
GO

/* ---------------------------------------------------------------------- */
/* Add table "databaseschemapermission"                                   */
/* ---------------------------------------------------------------------- */

CREATE TABLE [databaseschemapermission] (
    [snapshotid] INTEGER NOT NULL,
    [dbid] INTEGER NOT NULL,
    [schemaid] INTEGER NOT NULL,
    [classid] INTEGER NOT NULL,
    [permission] NVARCHAR(128) NOT NULL,
    [grantee] INTEGER NOT NULL,
    [grantor] INTEGER,
    [isgrant] NCHAR(1) NOT NULL,
    [isgrantwith] NCHAR(1) NOT NULL,
    [isrevoke] NCHAR(1) NOT NULL,
    [isdeny] NCHAR(1) NOT NULL,
    [hashkey] NVARCHAR(256),
    CONSTRAINT [PK__databaseschemape__07020F21] PRIMARY KEY ([snapshotid], [dbid], [schemaid], [classid], [permission], [grantee])
)
GO

/* ---------------------------------------------------------------------- */
/* Add table "endpoint"                                                   */
/* ---------------------------------------------------------------------- */

CREATE TABLE [endpoint] (
    [snapshotid] INTEGER NOT NULL,
    [endpointid] INTEGER NOT NULL,
    [principalid] INTEGER NOT NULL,
    [name] NVARCHAR(128) NOT NULL,
    [type] NVARCHAR(60),
    [protocol] NVARCHAR(60),
    [state] NVARCHAR(60),
    [isadminendpoint] NCHAR(1) NOT NULL,
    [hashkey] NVARCHAR(256),
    CONSTRAINT [PK__endpoint__286302EC] PRIMARY KEY ([snapshotid], [endpointid])
)
GO

/* ---------------------------------------------------------------------- */
/* Add table "filterrule"                                                 */
/* ---------------------------------------------------------------------- */

CREATE TABLE [filterrule] (
    [filterruleheaderid] INTEGER NOT NULL,
    [filterruleid] INTEGER IDENTITY(1,1) NOT NULL,
    [class] INTEGER NOT NULL,
    [scope] NVARCHAR(64),
    [matchstring] NVARCHAR(1000),
    CONSTRAINT [PK__filterrule__4D94879B] PRIMARY KEY ([filterruleheaderid], [filterruleid])
)
GO

/* ---------------------------------------------------------------------- */
/* Add table "filterruleclass"                                            */
/* ---------------------------------------------------------------------- */

CREATE TABLE [filterruleclass] (
    [objectclass] INTEGER NOT NULL,
    [objectvalue] NVARCHAR(128) NOT NULL,
    CONSTRAINT [PK__filterruleclass__4BAC3F29] PRIMARY KEY ([objectclass])
)
GO

/* ---------------------------------------------------------------------- */
/* Add table "filterruleheader"                                           */
/* ---------------------------------------------------------------------- */

CREATE TABLE [filterruleheader] (
    [filterruleheaderid] INTEGER IDENTITY(1,1) NOT NULL,
    [connectionname] NVARCHAR(400) NOT NULL,
    [rulename] NVARCHAR(256) NOT NULL,
    [description] NVARCHAR(80),
    [createdby] NVARCHAR(500),
    [createdtm] DATETIME,
    [lastmodifiedtm] DATETIME,
    [lastmodifiedby] NVARCHAR(500),
    CONSTRAINT [PK__filterruleheader__48CFD27E] PRIMARY KEY ([filterruleheaderid])
)
GO

/* ---------------------------------------------------------------------- */
/* Add table "objecttype"                                                 */
/* ---------------------------------------------------------------------- */

CREATE TABLE [objecttype] (
    [objecttype] NVARCHAR(5) NOT NULL,
    [objecttypename] NVARCHAR(500) NOT NULL,
    [hashkey] NVARCHAR(256),
    CONSTRAINT [PK__objecttype__36B12243] PRIMARY KEY ([objecttype])
)
GO

/* ---------------------------------------------------------------------- */
/* Add table "registeredserver"                                           */
/* ---------------------------------------------------------------------- */

CREATE TABLE [registeredserver] (
    [connectionname] NVARCHAR(400) NOT NULL,
    [servername] NVARCHAR(128),
    [instancename] NVARCHAR(128),
    [snapshotretentionperiod] INTEGER,
    [authenticationmode] NVARCHAR(1),
    [sqlserverlogin] NVARCHAR(128),
    [sqlserverpassword] NVARCHAR(300),
    [sqlserverauthtype] NCHAR(1),
    [serverlogin] NVARCHAR(128),
    [serverpassword] NVARCHAR(300),
    [os] NVARCHAR(512),
    [version] NVARCHAR(256),
    [edition] NVARCHAR(256),
    [loginauditmode] NVARCHAR(20),
    [enableproxyaccount] NCHAR(1),
    [enablec2audittrace] NCHAR(1),
    [crossdbownershipchaining] NCHAR(1),
    [casesensitivemode] NCHAR(1),
    [jobid] UNIQUEIDENTIFIER,
    [lastcollectiontm] DATETIME,
    [lastcollectionsnapshotid] INTEGER,
    [currentcollectiontm] DATETIME,
    [currentcollectionstatus] NCHAR(1),
    CONSTRAINT [PK__registeredserver__20C1E124] PRIMARY KEY ([connectionname])
)
GO

/* ---------------------------------------------------------------------- */
/* Add table "serverfilterrule"                                           */
/* ---------------------------------------------------------------------- */

CREATE TABLE [serverfilterrule] (
    [snapshotid] INTEGER NOT NULL,
    [filterruleheaderid] INTEGER NOT NULL,
    [filterruleid] INTEGER NOT NULL,
    [scope] NVARCHAR(64),
    [class] INTEGER NOT NULL,
    [matchstring] NVARCHAR(1000),
    [hashkey] NVARCHAR(256),
    CONSTRAINT [PK__serverfilterrule__182C9B23] PRIMARY KEY ([snapshotid], [filterruleheaderid], [filterruleid])
)
GO

/* ---------------------------------------------------------------------- */
/* Add table "serverfilterruleheader"                                     */
/* ---------------------------------------------------------------------- */

CREATE TABLE [serverfilterruleheader] (
    [snapshotid] INTEGER NOT NULL,
    [filterruleheaderid] INTEGER NOT NULL,
    [rulename] NVARCHAR(256) NOT NULL,
    [description] NVARCHAR(80),
    [createdtm] DATETIME,
    [createdby] NVARCHAR(500),
    [lastmodifiedby] NVARCHAR(500),
    [lastmodifiedtm] DATETIME,
    [hashkey] NVARCHAR(256),
    CONSTRAINT [PK__serverfilterrule__15502E78] PRIMARY KEY ([snapshotid], [filterruleheaderid])
)
GO

/* ---------------------------------------------------------------------- */
/* Add table "serverpermission"                                           */
/* ---------------------------------------------------------------------- */

CREATE TABLE [serverpermission] (
    [snapshotid] INTEGER NOT NULL,
    [majorid] INTEGER NOT NULL,
    [minorid] INTEGER NOT NULL,
    [classid] INTEGER NOT NULL,
    [permission] NVARCHAR(128) NOT NULL,
    [grantee] INTEGER NOT NULL,
    [grantor] INTEGER NOT NULL,
    [isgrant] NCHAR(1),
    [isgrantwith] NCHAR(1),
    [isrevoke] NCHAR(1),
    [isdeny] NCHAR(1),
    [hashkey] NVARCHAR(256),
    CONSTRAINT [PK__serverpermission__2B3F6F97] PRIMARY KEY ([snapshotid], [majorid], [minorid], [classid], [permission], [grantee])
)
GO

/* ---------------------------------------------------------------------- */
/* Add table "serverprincipal"                                            */
/* ---------------------------------------------------------------------- */

CREATE TABLE [serverprincipal] (
    [snapshotid] INTEGER NOT NULL,
    [principalid] INTEGER NOT NULL,
    [sid] VARBINARY(85) NOT NULL,
    [name] NVARCHAR(128) NOT NULL,
    [type] NCHAR(1) NOT NULL,
    [serveraccess] NCHAR(1),
    [serverdeny] NCHAR(1),
    [disabled] NCHAR(1),
    [isexpirationchecked] NCHAR(1),
    [ispolicychecked] NCHAR(1),
    [hashkey] NVARCHAR(256),
    CONSTRAINT [PK__serverprincipal__78B3EFCA] PRIMARY KEY ([snapshotid], [principalid])
)
GO

/* ---------------------------------------------------------------------- */
/* Add table "serverrolemember"                                           */
/* ---------------------------------------------------------------------- */

CREATE TABLE [serverrolemember] (
    [snapshotid] INTEGER NOT NULL,
    [principalid] INTEGER NOT NULL,
    [memberprincipalid] INTEGER NOT NULL,
    [hashkey] NVARCHAR(256),
    CONSTRAINT [PK__serverrolemember__25869641] PRIMARY KEY ([snapshotid], [principalid], [memberprincipalid])
)
GO

/* ---------------------------------------------------------------------- */
/* Add table "serversnapshot"                                             */
/* ---------------------------------------------------------------------- */

CREATE TABLE [serversnapshot] (
    [snapshotid] INTEGER IDENTITY(1,1) NOT NULL,
    [connectionname] NVARCHAR(400) NOT NULL,
    [servername] NVARCHAR(128),
    [instancename] NVARCHAR(128),
    [authenticationmode] NCHAR(1) NOT NULL,
    [os] NVARCHAR(512),
    [version] NVARCHAR(256),
    [edition] NVARCHAR(256),
    [status] NCHAR(1),
    [starttime] DATETIME,
    [endtime] DATETIME,
    [automated] NCHAR(1),
    [numobject] INTEGER,
    [numpermission] INTEGER,
    [numlogin] INTEGER,
    [numwindowsgroupmember] INTEGER,
    [baseline] NCHAR(1),
    [baselinecomment] NVARCHAR(400),
    [snapshotcomment] NVARCHAR(400),
    [loginauditmode] NVARCHAR(20),
    [enableproxyaccount] NCHAR(1),
    [enablec2audittrace] NCHAR(1),
    [crossdbownershipchaining] NCHAR(1),
    [casesensitivemode] NCHAR(1),
    [hashkey] NVARCHAR(256),
    CONSTRAINT [PK__serversnapshot__76CBA758] PRIMARY KEY ([snapshotid])
)
GO

/* ---------------------------------------------------------------------- */
/* Add table "serverstatistic"                                            */
/* ---------------------------------------------------------------------- */

CREATE TABLE [serverstatistic] (
    [connectionname] NVARCHAR(400) NOT NULL,
    [servername] NVARCHAR(128),
    [databasenotaudited] INTEGER,
    CONSTRAINT [PK__serverstatistic__4316F928] PRIMARY KEY ([connectionname])
)
GO

/* ---------------------------------------------------------------------- */
/* Add table "snapshothistory"                                            */
/* ---------------------------------------------------------------------- */

CREATE TABLE [snapshothistory] (
    [snapshothistoryid] INTEGER IDENTITY(1,1) NOT NULL,
    [snapshotid] INTEGER,
    [scheduletime] DATETIME,
    [starttime] DATETIME,
    [endtime] DATETIME,
    [numberoferror] INTEGER,
    [status] NCHAR(1),
    CONSTRAINT [PK__snapshothistory__44FF419A] PRIMARY KEY ([snapshothistoryid])
)
GO

/* ---------------------------------------------------------------------- */
/* Add table "sqldatabase"                                                */
/* ---------------------------------------------------------------------- */

CREATE TABLE [sqldatabase] (
    [snapshotid] INTEGER NOT NULL,
    [dbid] INTEGER NOT NULL,
    [databasename] NVARCHAR(128) NOT NULL,
    [owner] NVARCHAR(128),
    [available] NCHAR(1) NOT NULL,
    [status] NVARCHAR(128),
    [guestenabled] NCHAR(1) NOT NULL,
    [hashkey] NVARCHAR(256),
    CONSTRAINT [PK__sqldatabase__7B905C75] PRIMARY KEY ([snapshotid], [dbid])
)
GO

/* ---------------------------------------------------------------------- */
/* Add table "sqlserverobjecttype"                                        */
/* ---------------------------------------------------------------------- */

CREATE TABLE [sqlserverobjecttype] (
    [sqlserverversion] NVARCHAR(256) NOT NULL,
    [objecttype] NVARCHAR(128),
    CONSTRAINT [PK__sqlserverobjectt__5165187F] PRIMARY KEY ([sqlserverversion])
)
GO

/* ---------------------------------------------------------------------- */
/* Add table "windowsaccount"                                             */
/* ---------------------------------------------------------------------- */

CREATE TABLE [windowsaccount] (
    [snapshotid] INTEGER NOT NULL,
    [sid] VARBINARY(85) NOT NULL,
    [type] NVARCHAR(128),
    [name] NVARCHAR(200),
    [state] NCHAR(1),
    [hashkey] NVARCHAR(256),
    CONSTRAINT [PK__windowsaccount__1273C1CD] PRIMARY KEY ([snapshotid], [sid])
)
GO

/* ---------------------------------------------------------------------- */
/* Add table "windowsgroupmember"                                         */
/* ---------------------------------------------------------------------- */

CREATE TABLE [windowsgroupmember] (
    [snapshotid] INTEGER NOT NULL,
    [groupsid] VARBINARY(85) NOT NULL,
    [groupmember] VARBINARY(85) NOT NULL,
    [hashkey] NVARCHAR(256),
    CONSTRAINT [PK__windowsgroupmemb__2F10007B] PRIMARY KEY ([snapshotid], [groupsid], [groupmember])
)
GO

/* ---------------------------------------------------------------------- */
/* Add table "coveringpermissionhierarchy"                                */
/* ---------------------------------------------------------------------- */

CREATE TABLE [coveringpermissionhierarchy] (
    [permissionlevel] NVARCHAR(64),
    [permissionname] NVARCHAR(128),
    [coveringpermissionname] NVARCHAR(128),
    [parentpermissionlevel] NVARCHAR(64),
    [parentcoveringpermission] NVARCHAR(128)
)
GO

/* ---------------------------------------------------------------------- */
/* Add table "columnpermissionreference"                                  */
/* ---------------------------------------------------------------------- */

CREATE TABLE [columnpermissionreference] (
    [parentpermission] NVARCHAR(32),
    [permission] NVARCHAR(32)
)
GO

/* ---------------------------------------------------------------------- */
/* Add table "fixedrolepermission"                                        */
/* ---------------------------------------------------------------------- */

CREATE TABLE [fixedrolepermission] (
    [rolename] NVARCHAR(256),
    [rolepermission] NVARCHAR(256),
    [roletype] NCHAR(1),
    [isgrant] NCHAR(1),
    [isgrantwith] NCHAR(1),
    [isrevoke] NCHAR(1),
    [isdeny] NCHAR(1)
)
GO

/* ---------------------------------------------------------------------- */
/* Add table "collectorinfo"                                              */
/* ---------------------------------------------------------------------- */

CREATE TABLE [collectorinfo] (
    [name] NVARCHAR(64) NOT NULL,
    [value] NVARCHAR(1000) NOT NULL,
    [lastmodifiedby] NVARCHAR(512),
    [lastmodifiedtm] DATETIME,
    CONSTRAINT [PK_collectorinfo] PRIMARY KEY ([name])
)
GO

/* ---------------------------------------------------------------------- */
/* Add table "groomingactivityhistory"                                    */
/* ---------------------------------------------------------------------- */

CREATE TABLE [groomingactivityhistory] (
    [groomingactivityid] INTEGER IDENTITY(1,1) NOT NULL,
    [activitystarttime] DATETIME,
    [activityendtime] DATETIME,
    [status] NCHAR(1),
    [comment] NVARCHAR(1000),
    CONSTRAINT [PK_groomingactivityhistory] PRIMARY KEY ([groomingactivityid])
)
GO

/* ---------------------------------------------------------------------- */
/* Add table "reports"                                                    */
/* ---------------------------------------------------------------------- */

CREATE TABLE [reports] (
    [reportserver] NVARCHAR(128),
    [reportfolder] NVARCHAR(256),
    [reportingservicestemplate] NVARCHAR(128),
    [maintemplate] NVARCHAR(128),
    [reportstemplate] NVARCHAR(128),
    [name1] NVARCHAR(128),
    [url1] NVARCHAR(128),
    [desc1] NVARCHAR(256),
    [name2] NVARCHAR(128),
    [url2] NVARCHAR(128),
    [desc2] NVARCHAR(256),
    [name3] NVARCHAR(128),
    [url3] NVARCHAR(128),
    [desc3] NVARCHAR(256),
    [name4] NVARCHAR(128),
    [url4] NVARCHAR(128),
    [desc4] NVARCHAR(256),
    [name5] NVARCHAR(128),
    [url5] NVARCHAR(128),
    [desc5] NVARCHAR(256)
)
GO

/* ---------------------------------------------------------------------- */
/* Foreign key constraints                                                */
/* ---------------------------------------------------------------------- */

ALTER TABLE [databaseobject] ADD CONSTRAINT [FK__databaseo__class__398D8EEE] 
    FOREIGN KEY ([classid]) REFERENCES [classtype] (classid)
GO

ALTER TABLE [databaseobject] ADD CONSTRAINT [FK__databaseobject__3A81B327] 
    FOREIGN KEY ([snapshotid], [dbid], [owner]) REFERENCES [databaseprincipal] (snapshotid,dbid,uid)
GO

ALTER TABLE [databaseobject] ADD CONSTRAINT [FK__databaseobject__3B75D760] 
    FOREIGN KEY ([snapshotid], [dbid], [schemaid]) REFERENCES [databaseschema] (snapshotid,dbid,schemaid)
GO

ALTER TABLE [databaseobject] ADD CONSTRAINT [FK__databaseob__type__3C69FB99] 
    FOREIGN KEY ([type]) REFERENCES [objecttype] (objecttype)
GO

ALTER TABLE [databaseobject] ADD CONSTRAINT [FK__databaseobject__3D5E1FD2] 
    FOREIGN KEY ([snapshotid], [dbid]) REFERENCES [sqldatabase] (snapshotid,dbid)
GO

ALTER TABLE [databaseobjectpermission] ADD CONSTRAINT [FK__databaseobjectpe__412EB0B6] 
    FOREIGN KEY ([snapshotid], [dbid], [classid], [parentobjectid], [objectid]) REFERENCES [databaseobject] (snapshotid,dbid,classid,parentobjectid,objectid)
GO

ALTER TABLE [databaseobjectpermission] ADD CONSTRAINT [FK__databaseobjectpe__403A8C7D] 
    FOREIGN KEY ([snapshotid], [dbid], [grantor]) REFERENCES [databaseprincipal] (snapshotid,dbid,uid)
GO

ALTER TABLE [databaseprincipal] ADD CONSTRAINT [FK__databaseprincipa__7F60ED59] 
    FOREIGN KEY ([snapshotid], [owner]) REFERENCES [serverprincipal] (snapshotid,principalid)
GO

ALTER TABLE [databaseprincipal] ADD CONSTRAINT [FK__databaseprincipa__00551192] 
    FOREIGN KEY ([snapshotid], [dbid]) REFERENCES [sqldatabase] (snapshotid,dbid)
GO

ALTER TABLE [databaseprincipalpermission] ADD CONSTRAINT [FK__databasep__class__0F975522] 
    FOREIGN KEY ([classid]) REFERENCES [classtype] (classid)
GO

ALTER TABLE [databaseprincipalpermission] ADD CONSTRAINT [FK__databaseprincipa__108B795B] 
    FOREIGN KEY ([snapshotid], [dbid], [uid]) REFERENCES [databaseprincipal] (snapshotid,dbid,uid)
GO

ALTER TABLE [databaseprincipalpermission] ADD CONSTRAINT [FK__databaseprincipa__0DAF0CB0] 
    FOREIGN KEY ([snapshotid], [dbid], [grantee]) REFERENCES [databaseprincipal] (snapshotid,dbid,uid)
GO

ALTER TABLE [databaseprincipalpermission] ADD CONSTRAINT [FK__databaseprincipa__0EA330E9] 
    FOREIGN KEY ([snapshotid], [dbid], [grantor]) REFERENCES [databaseprincipal] (snapshotid,dbid,uid)
GO

ALTER TABLE [databaserolemember] ADD CONSTRAINT [FK__databaserolememb__33D4B598] 
    FOREIGN KEY ([snapshotid], [dbid], [rolememberuid]) REFERENCES [databaseprincipal] (snapshotid,dbid,uid)
GO

ALTER TABLE [databaserolemember] ADD CONSTRAINT [FK__databaserolememb__34C8D9D1] 
    FOREIGN KEY ([snapshotid], [dbid], [groupuid]) REFERENCES [databaseprincipal] (snapshotid,dbid,uid)
GO

ALTER TABLE [databaseschema] ADD CONSTRAINT [FK__databaseschema__0519C6AF] 
    FOREIGN KEY ([snapshotid], [dbid], [uid]) REFERENCES [databaseprincipal] (snapshotid,dbid,uid)
GO

ALTER TABLE [databaseschemapermission] ADD CONSTRAINT [FK__databases__class__08EA5793] 
    FOREIGN KEY ([classid]) REFERENCES [classtype] (classid)
GO

ALTER TABLE [databaseschemapermission] ADD CONSTRAINT [FK__databaseschemape__09DE7BCC] 
    FOREIGN KEY ([snapshotid], [dbid], [grantee]) REFERENCES [databaseprincipal] (snapshotid,dbid,uid)
GO

ALTER TABLE [databaseschemapermission] ADD CONSTRAINT [FK__databaseschemape__07F6335A] 
    FOREIGN KEY ([snapshotid], [dbid], [grantor]) REFERENCES [databaseprincipal] (snapshotid,dbid,uid)
GO

ALTER TABLE [databaseschemapermission] ADD CONSTRAINT [FK__databaseschemape__0AD2A005] 
    FOREIGN KEY ([snapshotid], [dbid], [schemaid]) REFERENCES [databaseschema] (snapshotid,dbid,schemaid)
GO

ALTER TABLE [endpoint] ADD CONSTRAINT [FK__endpoint__snapsh__29572725] 
    FOREIGN KEY ([snapshotid]) REFERENCES [serversnapshot] (snapshotid)
GO

ALTER TABLE [filterrule] ADD CONSTRAINT [FK__filterrul__class__4E88ABD4] 
    FOREIGN KEY ([class]) REFERENCES [filterruleclass] (objectclass)
GO

ALTER TABLE [filterrule] ADD CONSTRAINT [FK__filterrul__filte__4F7CD00D] 
    FOREIGN KEY ([filterruleheaderid]) REFERENCES [filterruleheader] (filterruleheaderid)
GO

ALTER TABLE [filterruleheader] ADD CONSTRAINT [FK__filterrul__conne__49C3F6B7] 
    FOREIGN KEY ([connectionname]) REFERENCES [registeredserver] (connectionname)
GO

ALTER TABLE [serverfilterrule] ADD CONSTRAINT [FK__serverfilterrule__1920BF5C] 
    FOREIGN KEY ([snapshotid], [filterruleheaderid]) REFERENCES [serverfilterruleheader] (snapshotid,filterruleheaderid)
GO

ALTER TABLE [serverfilterruleheader] ADD CONSTRAINT [FK__serverfil__snaps__164452B1] 
    FOREIGN KEY ([snapshotid]) REFERENCES [serversnapshot] (snapshotid)
GO

ALTER TABLE [serverpermission] ADD CONSTRAINT [FK__serverper__class__2C3393D0] 
    FOREIGN KEY ([classid]) REFERENCES [classtype] (classid)
GO

ALTER TABLE [serverpermission] ADD CONSTRAINT [FK__serverpermission__2D27B809] 
    FOREIGN KEY ([snapshotid], [grantee]) REFERENCES [serverprincipal] (snapshotid,principalid)
GO

ALTER TABLE [serverprincipal] ADD CONSTRAINT [FK__serverpri__snaps__79A81403] 
    FOREIGN KEY ([snapshotid]) REFERENCES [serversnapshot] (snapshotid)
GO

ALTER TABLE [serverrolemember] ADD CONSTRAINT [FK__serverrolemember__267ABA7A] 
    FOREIGN KEY ([snapshotid], [principalid]) REFERENCES [serverprincipal] (snapshotid,principalid)
GO

ALTER TABLE [sqldatabase] ADD CONSTRAINT [FK__sqldataba__snaps__7C8480AE] 
    FOREIGN KEY ([snapshotid]) REFERENCES [serversnapshot] (snapshotid)
GO

ALTER TABLE [windowsaccount] ADD CONSTRAINT [FK__windowsac__snaps__1367E606] 
    FOREIGN KEY ([snapshotid]) REFERENCES [serversnapshot] (snapshotid)
GO

ALTER TABLE [windowsgroupmember] ADD CONSTRAINT [FK__windowsgroupmemb__300424B4] 
    FOREIGN KEY ([snapshotid], [groupsid]) REFERENCES [windowsaccount] (snapshotid,sid)
GO

ALTER TABLE [windowsgroupmember] ADD CONSTRAINT [FK__windowsgroupmemb__30F848ED] 
    FOREIGN KEY ([snapshotid], [groupmember]) REFERENCES [windowsaccount] (snapshotid,sid)
GO
-- initial data loading

insert into filterruleclass (objectclass, objectvalue) values (0, 'Server')
insert into filterruleclass (objectclass, objectvalue) values (1, 'Logins')
insert into filterruleclass (objectclass, objectvalue) values (2, 'Endpoint')
insert into filterruleclass (objectclass, objectvalue) values (20, 'Database')
insert into filterruleclass (objectclass, objectvalue) values (21, 'Users')
insert into filterruleclass (objectclass, objectvalue) values (22, 'Roles')
insert into filterruleclass (objectclass, objectvalue) values (26, 'Assemblies')
insert into filterruleclass (objectclass, objectvalue) values (27, 'Certificates')
insert into filterruleclass (objectclass, objectvalue) values (28, 'Full-Text Catalogs')
insert into filterruleclass (objectclass, objectvalue) values (29, 'Keys')
insert into filterruleclass (objectclass, objectvalue) values (30, 'Schemas')
insert into filterruleclass (objectclass, objectvalue) values (31, 'User-Defined Data Types')
insert into filterruleclass (objectclass, objectvalue) values (32, 'XML Schema Collections')
insert into filterruleclass (objectclass, objectvalue) values (41, 'Tables')
insert into filterruleclass (objectclass, objectvalue) values (42, 'Stored Procedures')
insert into filterruleclass (objectclass, objectvalue) values (43, 'Extended Stored Procedures')
insert into filterruleclass (objectclass, objectvalue) values (44, 'Views')
insert into filterruleclass (objectclass, objectvalue) values (45, 'Functions')
insert into filterruleclass (objectclass, objectvalue) values (46, 'Synonyms')

insert into classtype (classid, classname) values (-1, 'Unknown')
insert into classtype (classid, classname) values (0, 'Database')
insert into classtype (classid, classname) values (1, 'Object or Column')
insert into classtype (classid, classname) values (3, 'Schema')
insert into classtype (classid, classname) values (4, 'Database Principal')
insert into classtype (classid, classname) values (5, 'Assembly')
insert into classtype (classid, classname) values (6, 'Type')
insert into classtype (classid, classname) values (10, 'XML Schema Collection')
insert into classtype (classid, classname) values (15, 'Message Type')
insert into classtype (classid, classname) values (16, 'Service Contract')
insert into classtype (classid, classname) values (17, 'Service')
insert into classtype (classid, classname) values (18, 'Remote Service Binding')
insert into classtype (classid, classname) values (19, 'Route')
insert into classtype (classid, classname) values (23, 'Full-Text Catalog')
insert into classtype (classid, classname) values (24, 'Symmetric Key')
insert into classtype (classid, classname) values (25, 'Certificate')
insert into classtype (classid, classname) values (26, 'Asymmetric Key')
insert into classtype (classid, classname) values (100, 'Server')
insert into classtype (classid, classname) values (101, 'Login')
insert into classtype (classid, classname) values (105, 'Endpoint')

insert into objecttype (objecttype, objecttypename) values ('AF', 'Aggregate Function (CLR)')
insert into objecttype (objecttype, objecttypename) values ('C', 'CHECK constraint')
insert into objecttype (objecttype, objecttypename) values ('D', 'DEFAULT (constraint or stand-alone)')
insert into objecttype (objecttype, objecttypename) values ('F', 'FOREIGN KEY constraint')
insert into objecttype (objecttype, objecttypename) values ('PK', 'PRIMARY KEY constraint')
insert into objecttype (objecttype, objecttypename) values ('P', 'Stored Procedure')
insert into objecttype (objecttype, objecttypename) values ('S', 'System Table')
insert into objecttype (objecttype, objecttypename) values ('PC', 'Stored Procedure (CLR)')
insert into objecttype (objecttype, objecttypename) values ('FN', 'Scalar Function')
insert into objecttype (objecttype, objecttypename) values ('FS', 'Scalar Function (CLR)')
insert into objecttype (objecttype, objecttypename) values ('FT', 'Table-valued Function (CLR)')
insert into objecttype (objecttype, objecttypename) values ('R', 'Rule (old-style, stand-alone)')
insert into objecttype (objecttype, objecttypename) values ('RF', 'Replication-filter-procedure')
insert into objecttype (objecttype, objecttypename) values ('SN', 'Synonym')
insert into objecttype (objecttype, objecttypename) values ('SQ', 'Service Queue')
insert into objecttype (objecttype, objecttypename) values ('TA', 'Assembly (CLR) DML trigger')
insert into objecttype (objecttype, objecttypename) values ('TR', 'SQL DML trigger')
insert into objecttype (objecttype, objecttypename) values ('TF', 'Table-valued Function')
insert into objecttype (objecttype, objecttypename) values ('U', 'Table')
insert into objecttype (objecttype, objecttypename) values ('UQ', 'UNIQUE constraint')
insert into objecttype (objecttype, objecttypename) values ('V', 'View')
insert into objecttype (objecttype, objecttypename) values ('X', 'Extended Stored Procedure')
insert into objecttype (objecttype, objecttypename) values ('DB', 'Database')
insert into objecttype (objecttype, objecttypename) values ('iCO', 'Column')
insert into objecttype (objecttype, objecttypename) values ('iASM', 'Assembly')
insert into objecttype (objecttype, objecttypename) values ('iCERT', 'Certificate')
insert into objecttype (objecttype, objecttypename) values ('IF', 'Inline Table-valued Function')
insert into objecttype (objecttype, objecttypename) values ('iFTXT', 'Full Text Catalog')
insert into objecttype (objecttype, objecttypename) values ('IT', 'System Table')
insert into objecttype (objecttype, objecttypename) values ('iUDT', 'User-defined Data Type')
insert into objecttype (objecttype, objecttypename) values ('iXMLS', 'XML Schema Collection')
insert into objecttype (objecttype, objecttypename) values ('iSK', 'Symmetric Key')
insert into objecttype (objecttype, objecttypename) values ('iAK', 'Asymmetric Key')
insert into objecttype (objecttype, objecttypename) values ('iSCM', 'Schema')
insert into objecttype (objecttype, objecttypename) values ('iDUSR', 'Database User')
insert into objecttype (objecttype, objecttypename) values ('iDRLE', 'Database Role')
insert into objecttype (objecttype, objecttypename) values ('iSRV', 'Server')
insert into objecttype (objecttype, objecttypename) values ('iLOGN', 'Login')
insert into objecttype (objecttype, objecttypename) values ('iENDP', 'Endpoint')


insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DB2000', 'CREATE TABLE', 'CONTROL', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DB2000', 'CREATE PROCEDURE', 'CONTROL', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DB2000', 'CREATE VIEW', 'CONTROL', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DB2000', 'CREATE DEFAULT', 'CONTROL', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DB2000', 'CREATE RULE', 'CONTROL', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DB2000', 'CREATE FUNCTION', 'CONTROL', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DB2000', 'BACKUP DATABASE', 'CONTROL', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DB2000', 'BACKUP LOG', 'CONTROL', 'SERVER', 'CONTROL SERVER')

insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'CREATE TABLE', 'ALTER', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'CREATE VIEW', 'ALTER', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'CREATE PROCEDURE', 'ALTER', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'CREATE FUNCTION', 'ALTER', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'CREATE RULE', 'ALTER', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'CREATE DEFAULT', 'ALTER', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'BACKUP DATABASE', 'CONTROL', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'BACKUP LOG', 'CONTROL', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'CREATE DATABASE', '', 'SERVER', 'CREATE ANY DATABASE')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'CREATE TYPE', 'ALTER', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'CREATE ASSEMBLY', 'ALTER ANY ASSEMBLY', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'CREATE XML SCHEMA COLLECTION', 'ALTER', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'CREATE SCHEMA', 'ALTER ANY SCHEMA', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'CREATE SYNONYM', 'ALTER', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'CREATE AGGREGATE', 'ALTER', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'CREATE ROLE', 'ALTER ANY ROLE', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'CREATE MESSAGE TYPE', 'ALTER ANY MESSAGE TYPE', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'CREATE SERVICE', 'ALTER ANY SERVICE', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'CREATE CONTRACT', 'ALTER ANY CONTRACT', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'CREATE REMOTE SERVICE BINDING', 'ALTER ANY REMOTE SERVICE BINDING', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'CREATE ROUTE', 'ALTER ANY ROUTE', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'CREATE QUEUE', 'ALTER', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'CREATE SYMMETRIC KEY', 'ALTER ANY SYMMETRIC KEY', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'CREATE ASYMMETRIC KEY', 'ALTER ANY ASYMMETRIC KEY', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'CREATE FULLTEXT CATALOG', 'ALTER ANY FULLTEXT CATALOG', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'CREATE CERTIFICATE', 'ALTER ANY CERTIFICATE', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'CREATE DATABASE DDL EVENT NOTIFICATION', 'ALTER ANY DATABASE EVENT NOTIFICATION', 'SERVER', 'CREATE DDL EVENT NOTIFICATION')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'CONNECT', 'CONNECT REPLICATION', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'CONNECT REPLICATION', 'CONTROL', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'CHECKPOINT', 'CONTROL', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'SUBSCRIBE QUERY NOTIFICATIONS', 'CONTROL', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'AUTHENTICATE', 'CONTROL', 'SERVER', 'AUTHENTICATE SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'SHOWPLAN', 'CONTROL', 'SERVER', 'ALTER TRACE')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'ALTER ANY USER', 'ALTER', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'ALTER ANY ROLE', 'ALTER', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'ALTER ANY APPLICATION ROLE', 'ALTER', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'ALTER ANY SCHEMA', 'ALTER', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'ALTER ANY ASSEMBLY', 'ALTER', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'ALTER ANY DATASPACE', 'ALTER', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'ALTER ANY MESSAGE TYPE', 'ALTER', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'ALTER ANY CONTRACT', 'ALTER', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'ALTER ANY SERVICE', 'ALTER', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'ALTER ANY REMOTE SERVICE BINDING', 'ALTER', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'ALTER ANY ROUTE', 'ALTER', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'ALTER ANY FULLTEXT CATALOG', 'ALTER', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'ALTER ANY SYMMETRIC KEY', 'ALTER', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'ALTER ANY ASYMMETRIC KEY', 'ALTER', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'ALTER ANY CERTIFICATE', 'ALTER', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'SELECT', 'CONTROL', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'INSERT', 'CONTROL', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'UPDATE', 'CONTROL', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'DELETE', 'CONTROL', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'REFERENCES', 'CONTROL', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'EXECUTE', 'CONTROL', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'ALTER ANY DATABASE DDL TRIGGER', 'ALTER', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'ALTER ANY DATABASE EVENT NOTIFICATION', 'ALTER', 'SERVER', 'ALTER ANY EVENT NOTIFICATION')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'VIEW DATABASE STATE', 'CONTROL', 'SERVER', 'VIEW SERVER STATE')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'VIEW DEFINITION', 'CONTROL', 'SERVER', 'VIEW ANY DEFINITION')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'TAKE OWNERSHIP', 'CONTROL', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'ALTER', 'CONTROL', 'SERVER', 'ALTER ANY DATABASE')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('DATABASE', 'CONTROL', '', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('OBJECT', 'SELECT', 'RECEIVE', 'SCHEMA', 'SELECT')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('OBJECT', 'UPDATE', 'CONTROL', 'SCHEMA', 'UPDATE')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('OBJECT', 'REFERENCES', 'CONTROL', 'SCHEMA', 'REFERENCES')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('OBJECT', 'INSERT', 'CONTROL', 'SCHEMA', 'INSERT')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('OBJECT', 'DELETE', 'CONTROL', 'SCHEMA', 'DELETE')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('OBJECT', 'EXECUTE', 'CONTROL', 'SCHEMA', 'EXECUTE')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('OBJECT', 'RECEIVE', 'CONTROL', 'SCHEMA', 'CONTROL')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('OBJECT', 'SELECT', 'CONTROL', 'SCHEMA', 'CONTROL')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('OBJECT', 'VIEW DEFINITION', 'CONTROL', 'SCHEMA', 'VIEW DEFINITION')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('OBJECT', 'ALTER', 'CONTROL', 'SCHEMA', 'ALTER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('OBJECT', 'TAKE OWNERSHIP', 'CONTROL', 'SCHEMA', 'CONTROL')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('OBJECT', 'CONTROL', '', 'SCHEMA', 'CONTROL')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('TYPE', 'REFERENCES', 'CONTROL', 'SCHEMA', 'REFERENCES')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('TYPE', 'EXECUTE', 'CONTROL', 'SCHEMA', 'EXECUTE')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('TYPE', 'VIEW DEFINITION', 'CONTROL', 'SCHEMA', 'VIEW DEFINITION')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('TYPE', 'TAKE OWNERSHIP', 'CONTROL', 'SCHEMA', 'CONTROL')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('TYPE', 'CONTROL', '', 'SCHEMA', 'CONTROL')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SCHEMA', 'SELECT', 'CONTROL', 'DATABASE', 'SELECT')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SCHEMA', 'INSERT', 'CONTROL', 'DATABASE', 'INSERT')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SCHEMA', 'UPDATE', 'CONTROL', 'DATABASE', 'UPDATE')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SCHEMA', 'DELETE', 'CONTROL', 'DATABASE', 'DELETE')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SCHEMA', 'REFERENCES', 'CONTROL', 'DATABASE', 'REFERENCES')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SCHEMA', 'EXECUTE', 'CONTROL', 'DATABASE', 'EXECUTE')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SCHEMA', 'VIEW DEFINITION', 'CONTROL', 'DATABASE', 'VIEW DEFINITION')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SCHEMA', 'ALTER', 'CONTROL', 'DATABASE', 'ALTER ANY SCHEMA')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SCHEMA', 'ALTER', 'CONTROL', 'DATABASE', 'ALTER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SCHEMA', 'TAKE OWNERSHIP', 'CONTROL', 'DATABASE', 'CONTROL')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SCHEMA', 'CONTROL', '', 'DATABASE', 'CONTROL')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('XML SCHEMA COLLECTION', 'REFERENCES', 'CONTROL', 'SCHEMA', 'REFERENCES')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('XML SCHEMA COLLECTION', 'EXECUTE', 'CONTROL', 'SCHEMA', 'EXECUTE')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('XML SCHEMA COLLECTION', 'VIEW DEFINITION', 'CONTROL', 'SCHEMA', 'VIEW DEFINITION')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('XML SCHEMA COLLECTION', 'ALTER', 'CONTROL', 'SCHEMA', 'ALTER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('XML SCHEMA COLLECTION', 'TAKE OWNERSHIP', 'CONTROL', 'SCHEMA', 'CONTROL')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('XML SCHEMA COLLECTION', 'CONTROL', '', 'SCHEMA', 'CONTROL')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('ASSEMBLY', 'REFERENCES', 'CONTROL', 'DATABASE', 'REFERENCES')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('ASSEMBLY', 'EXECUTE', 'CONTROL', 'DATABASE', 'EXECUTE')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('ASSEMBLY', 'VIEW DEFINITION', 'CONTROL', 'DATABASE', 'VIEW DEFINITION')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('ASSEMBLY', 'ALTER', 'CONTROL', 'DATABASE', 'ALTER ANY ASSEMBLY')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('ASSEMBLY', 'ALTER', 'CONTROL', 'DATABASE', 'ALTER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('ASSEMBLY', 'TAKE OWNERSHIP', 'CONTROL', 'DATABASE', 'CONTROL')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('ASSEMBLY', 'CONTROL', '', 'DATABASE', 'CONTROL')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('USER', 'IMPERSONATE', 'CONTROL', 'DATABASE', 'CONTROL')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('USER', 'VIEW DEFINITION', 'CONTROL', 'DATABASE', 'VIEW DEFINITION')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('USER', 'ALTER', 'CONTROL', 'DATABASE', 'ALTER ANY USER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('USER', 'ALTER', 'CONTROL', 'DATABASE', 'ALTER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('USER', 'CONTROL', '', 'DATABASE', 'CONTROL')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('ROLE', 'VIEW DEFINITION', 'CONTROL', 'DATABASE', 'VIEW DEFINITION')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('ROLE', 'ALTER', 'CONTROL', 'DATABASE', 'ALTER ANY ROLE')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('ROLE', 'ALTER', 'CONTROL', 'DATABASE', 'ALTER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('ROLE', 'TAKE OWNERSHIP', 'CONTROL', 'DATABASE', 'CONTROL')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('ROLE', 'CONTROL', '', 'DATABASE', 'CONTROL')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('APPLICATION ROLE', 'VIEW DEFINITION', 'CONTROL', 'DATABASE', 'VIEW DEFINITION')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('APPLICATION ROLE', 'ALTER', 'CONTROL', 'DATABASE', 'ALTER ANY APPLICATION ROLE')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('APPLICATION ROLE', 'ALTER', 'CONTROL', 'DATABASE', 'ALTER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('APPLICATION ROLE', 'CONTROL', '', 'DATABASE', 'CONTROL')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('MESSAGE TYPE', 'REFERENCES', 'CONTROL', 'DATABASE', 'REFERENCES')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('MESSAGE TYPE', 'VIEW DEFINITION', 'CONTROL', 'DATABASE', 'VIEW DEFINITION')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('MESSAGE TYPE', 'ALTER', 'CONTROL', 'DATABASE', 'ALTER ANY MESSAGE TYPE')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('MESSAGE TYPE', 'ALTER', 'CONTROL', 'DATABASE', 'ALTER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('MESSAGE TYPE', 'TAKE OWNERSHIP', 'CONTROL', 'DATABASE', 'CONTROL')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('MESSAGE TYPE', 'CONTROL', '', 'DATABASE', 'CONTROL')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('CONTRACT', 'REFERENCES', 'CONTROL', 'DATABASE', 'REFERENCES')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('CONTRACT', 'VIEW DEFINITION', 'CONTROL', 'DATABASE', 'VIEW DEFINITION')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('CONTRACT', 'ALTER', 'CONTROL', 'DATABASE', 'ALTER ANY CONTRACT')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('CONTRACT', 'ALTER', 'CONTROL', 'DATABASE', 'ALTER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('CONTRACT', 'TAKE OWNERSHIP', 'CONTROL', 'DATABASE', 'CONTROL')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('CONTRACT', 'CONTROL', '', 'DATABASE', 'CONTROL')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SERVICE', 'SEND', 'CONTROL', 'DATABASE', 'CONTROL')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SERVICE', 'VIEW DEFINITION', 'CONTROL', 'DATABASE', 'VIEW DEFINITION')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SERVICE', 'ALTER', 'CONTROL', 'DATABASE', 'ALTER ANY SERVICE')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SERVICE', 'TAKE OWNERSHIP', 'CONTROL', 'DATABASE', 'CONTROL')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SERVICE', 'CONTROL', '', 'DATABASE', 'CONTROL')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('REMOTE SERVICE BINDING', 'VIEW DEFINITION', 'CONTROL', 'DATABASE', 'VIEW DEFINITION')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('REMOTE SERVICE BINDING', 'ALTER', 'CONTROL', 'DATABASE', 'ALTER ANY REMOTE SERVICE BINDING')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('REMOTE SERVICE BINDING', 'ALTER', 'CONTROL', 'DATABASE', 'ALTER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('REMOTE SERVICE BINDING', 'TAKE OWNERSHIP', 'CONTROL', 'DATABASE', 'CONTROL')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('REMOTE SERVICE BINDING', 'CONTROL', '', 'DATABASE', 'CONTROL')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('ROUTE', 'VIEW DEFINITION', 'CONTROL', 'DATABASE', 'VIEW DEFINITION')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('ROUTE', 'ALTER', 'CONTROL', 'DATABASE', 'ALTER ANY ROUTE')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('ROUTE', 'ALTER', 'CONTROL', 'DATABASE', 'ALTER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('ROUTE', 'TAKE OWNERSHIP', 'CONTROL', 'DATABASE', 'CONTROL')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('ROUTE', 'CONTROL', '', 'DATABASE', 'CONTROL')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('FULLTEXT CATALOG', 'REFERENCES', 'CONTROL', 'DATABASE', 'REFERENCES')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('FULLTEXT CATALOG', 'VIEW DEFINITION', 'CONTROL', 'DATABASE', 'VIEW DEFINITION')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('FULLTEXT CATALOG', 'ALTER', 'CONTROL', 'DATABASE', 'ALTER ANY FULLTEXT CATALOG')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('FULLTEXT CATALOG', 'ALTER', 'CONTROL', 'DATABASE', 'ALTER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('FULLTEXT CATALOG', 'TAKE OWNERSHIP', 'CONTROL', 'DATABASE', 'CONTROL')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('FULLTEXT CATALOG', 'CONTROL', '', 'DATABASE', 'CONTROL')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SYMMETRIC KEY', 'REFERENCES', 'CONTROL', 'DATABASE', 'REFERENCES')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SYMMETRIC KEY', 'VIEW DEFINITION', 'CONTROL', 'DATABASE', 'VIEW DEFINITION')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SYMMETRIC KEY', 'ALTER', 'CONTROL', 'DATABASE', 'ALTER ANY SYMMETRIC KEY')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SYMMETRIC KEY', 'ALTER', 'CONTROL', 'DATABASE', 'ALTER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SYMMETRIC KEY', 'TAKE OWNERSHIP', 'CONTROL', 'DATABASE', 'CONTROL')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SYMMETRIC KEY', 'CONTROL', '', 'DATABASE', 'CONTROL')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('CERTIFICATE', 'VIEW DEFINITION', 'CONTROL', 'DATABASE', 'VIEW DEFINITION')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('CERTIFICATE', 'REFERENCES', 'CONTROL', 'DATABASE', 'REFERENCES')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('CERTIFICATE', 'ALTER', 'CONTROL', 'DATABASE', 'ALTER ANY CERTIFICATE')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('CERTIFICATE', 'ALTER', 'CONTROL', 'DATABASE', 'ALTER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('CERTIFICATE', 'TAKE OWNERSHIP', 'CONTROL', 'DATABASE', 'CONTROL')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('CERTIFICATE', 'CONTROL', '', 'DATABASE', 'CONTROL')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SERVER', 'CONNECT SQL', 'CONTROL SERVER', '', '')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SERVER', 'SHUTDOWN', 'CONTROL SERVER', '', '')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SERVER', 'CREATE ENDPOINT', 'ALTER ANY ENDPOINT', '', '')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SERVER', 'CREATE ANY DATABASE', 'ALTER ANY DATABASE', '', '')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SERVER', 'ALTER ANY LOGIN', 'CONTROL SERVER', '', '')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SERVER', 'ALTER ANY CREDENTIAL', 'CONTROL SERVER', '', '')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SERVER', 'ALTER ANY ENDPOINT', 'CONTROL SERVER', '', '')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SERVER', 'ALTER ANY LINKED SERVER', 'CONTROL SERVER', '', '')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SERVER', 'ALTER ANY CONNECTION', 'CONTROL SERVER', '', '')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SERVER', 'ALTER ANY DATABASE', 'CONTROL SERVER', '', '')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SERVER', 'ALTER RESOURCES', 'CONTROL SERVER', '', '')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SERVER', 'ALTER SETTINGS', 'CONTROL SERVER', '', '')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SERVER', 'ALTER TRACE', 'CONTROL SERVER', '', '')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SERVER', 'ADMINISTER BULK OPERATIONS', 'CONTROL SERVER', '', '')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SERVER', 'AUTHENTICATE SERVER', 'CONTROL SERVER', '', '')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SERVER', 'EXTERNAL ACCESS ASSEMBLY', 'UNSAFE ASSEMBLY', '', '')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SERVER', 'VIEW ANY DATABASE', 'VIEW ANY DEFINITION', '', '')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SERVER', 'VIEW ANY DEFINITION', 'CONTROL SERVER', '', '')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SERVER', 'VIEW SERVER STATE', 'ALTER SERVER STATE', '', '')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SERVER', 'CREATE DDL EVENT NOTIFICATION', 'ALTER ANY EVENT NOTIFICATION', '', '')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SERVER', 'CREATE TRACE EVENT NOTIFICATION', 'ALTER ANY EVENT NOTIFICATION', '', '')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SERVER', 'ALTER ANY EVENT NOTIFICATION', 'CONTROL SERVER', '', '')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SERVER', 'ALTER SERVER STATE', 'CONTROL SERVER', '', '')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SERVER', 'UNSAFE ASSEMBLY', 'CONTROL SERVER', '', '')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SERVER', 'CONTROL SERVER', '', '', '')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('ENDPOINT', 'CONNECT', 'CONTROL', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('ENDPOINT', 'VIEW DEFINITION', 'CONTROL', 'SERVER', 'VIEW ANY DEFINITION')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('ENDPOINT', 'ALTER', 'CONTROL', 'SERVER', 'ALTER ANY ENDPOINT')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('ENDPOINT', 'TAKE OWNERSHIP', 'CONTROL', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('ENDPOINT', 'CONTROL', '', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('LOGIN', 'IMPERSONATE', 'CONTROL', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('LOGIN', 'VIEW DEFINITION', 'CONTROL', 'SERVER', 'VIEW ANY DEFINITION')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('LOGIN', 'ALTER', 'CONTROL', 'SERVER', 'ALTER ANY LOGIN')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('LOGIN', 'CONTROL', '', 'SERVER', 'CONTROL SERVER')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('ASYMMETRIC KEY', 'REFERENCES', 'CONTROL', 'DATABASE', 'REFERENCES')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('ASYMMETRIC KEY', 'VIEW DEFINITION', 'CONTROL', 'DATABASE', 'VIEW DEFINITION')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('ASYMMETRIC KEY', 'ALTER', 'CONTROL', 'DATABASE', 'ALTER ANY ASYMMETRIC KEY')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('ASYMMETRIC KEY', 'TAKE OWNERSHIP', 'CONTROL', 'DATABASE', 'CONTROL')
insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('ASYMMETRIC KEY', 'CONTROL', '', 'DATABASE', 'CONTROL')

insert into columnpermissionreference (parentpermission, permission) values ('CONTROL', 'SELECT')
insert into columnpermissionreference (parentpermission, permission) values ('CONTROL', 'UPDATE')
insert into columnpermissionreference (parentpermission, permission) values ('CONTROL', 'REFERENCE')

insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('BULKADMIN', 'ADMINISTER BULK OPERATIONS', 'S', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DBCREATOR', 'CREATE DATABASE', 'S', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DISKADMIN', 'ALTER RESOURCES', 'S', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('PROCESSADMIN', 'ALTER ANY CONNECTION', 'S', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('PROCESSADMIN', 'ALTER SERVER STATE', 'S', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('SECURITYADMIN', 'ALTER ANY LOGIN', 'S', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('SERVERADMIN', 'ALTER ANY ENDPOINT', 'S', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('SERVERADMIN', 'ALTER RESOURCES', 'S', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('SERVERADMIN', 'ALTER SERVER STATE', 'S', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('SERVERADMIN', 'ALTER SETTINGS', 'S', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('SERVERADMIN', 'SHUTDOWN', 'S', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('SERVERADMIN', 'VIEW SERVER STATE', 'S', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('SETUPADMIN', 'ALTER ANY LINKED SERVER', 'S', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('SYSADMIN', 'CONTROL SERVER', 'S', 'N', 'Y', 'N', 'N')

insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('BULKADMIN', 'BULK INSERT', 'P', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DBCREATOR', 'ALTER DATABASE', 'P', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DBCREATOR', 'CREATE DATABASE', 'P', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DBCREATOR', 'DROP DATABASE', 'P', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DBCREATOR', 'RESTORE DATABASE', 'P', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DBCREATOR', 'RESTORE LOG', 'P', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DISKADMIN', 'DISK INIT', 'P', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('PROCESSADMIN', 'KILL', 'P', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('SECURITYADMIN', 'CREATE DATABASE', 'P', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('SERVERADMIN', 'RECONFIGURE', 'P', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('SERVERADMIN', 'SHUTDOWN', 'P', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('SETUPADMIN', 'ALTER ANY LINKED SERVER', 'P', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('SYSADMIN', 'CONTROL SERVER', 'P', 'N', 'Y', 'N', 'N')

insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_ACCESSADMIN', 'ALTER ANY USER', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_ACCESSADMIN', 'CREATE SCHEMA', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_ACCESSADMIN', 'CONNECT', 'D', 'N', 'Y', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_BACKUPOPERATOR', 'BACKUP DATABASE', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_BACKUPOPERATOR', 'BACKUP LOG', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_BACKUPOPERATOR', 'CHECKPOINT', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_DATAREADER', 'SELECT', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_DATAWRITER', 'DELETE', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_DATAWRITER', 'INSERT', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_DATAWRITER', 'UPDATE', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_DDLADMIN', 'ALTER ANY ASSEMBLY', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_DDLADMIN', 'ALTER ANY ASYMMETRIC KEY', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_DDLADMIN', 'ALTER ANY CERTIFICATE', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_DDLADMIN', 'ALTER ANY CONTRACT', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_DDLADMIN', 'ALTER ANY DATABASE DDL TRIGGER', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_DDLADMIN', 'ALTER ANY DATABASE EVENT', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_DDLADMIN', 'NOTIFICATION', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_DDLADMIN', 'ALTER ANY DATASPACE', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_DDLADMIN', 'ALTER ANY FULLTEXT CATALOG', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_DDLADMIN', 'ALTER ANY MESSAGE TYPE', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_DDLADMIN', 'ALTER ANY REMOTE SERVICE BINDING', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_DDLADMIN', 'ALTER ANY ROUTE', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_DDLADMIN', 'ALTER ANY SCHEMA', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_DDLADMIN', 'ALTER ANY SERVICE', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_DDLADMIN', 'ALTER ANY SYMMETRIC KEY', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_DDLADMIN', 'CHECKPOINT', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_DDLADMIN', 'CREATE AGGREGATE', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_DDLADMIN', 'CREATE DEFAULT', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_DDLADMIN', 'CREATE FUNCTION', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_DDLADMIN', 'CREATE PROCEDURE', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_DDLADMIN', 'CREATE QUEUE', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_DDLADMIN', 'CREATE RULE', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_DDLADMIN', 'CREATE SYNONYM', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_DDLADMIN', 'CREATE TABLE', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_DDLADMIN', 'CREATE TYPE', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_DDLADMIN', 'CREATE VIEW', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_DDLADMIN', 'CREATE XML SCHEMA COLLECTION', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_DDLADMIN', 'CREATE REFERENCES', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_DENYDATAREADER', 'SELECT', 'D', 'N', 'N', 'N', 'Y')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_DENYDATAWRITER', 'DELETE', 'D', 'N', 'N', 'N', 'Y')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_DENYDATAWRITER', 'INSERT', 'D', 'N', 'N', 'N', 'Y')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_DENYDATAWRITER', 'UPDATE', 'D', 'N', 'N', 'N', 'Y')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_OWNER', 'CONTROL', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_SECURITYADMIN', 'ALTER ANY APPLICATION ROLE', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_SECURITYADMIN', 'ALTER ANY ROLE', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_SECURITYADMIN', 'CREATE SCHEMA', 'D', 'Y', 'N', 'N', 'N')
insert into fixedrolepermission (rolename, rolepermission, roletype, isgrant, isgrantwith, isrevoke, isdeny) values ('DB_SECURITYADMIN', 'VIEW DEFINITION', 'D', 'Y', 'N', 'N', 'N')

GRANT SELECT ON [ancillarywindowsgroup] TO [SQLSecureView]
GRANT SELECT ON [applicationactivity] TO [SQLSecureView]
GRANT SELECT ON [applicationlicense] TO [SQLSecureView]
GRANT SELECT ON [classtype] TO [SQLSecureView]
GRANT SELECT ON [collectorinfo] TO [SQLSecureView]
GRANT SELECT ON [columnpermissionreference] TO [SQLSecureView]
GRANT SELECT ON [compatibleversion] TO [SQLSecureView]
GRANT SELECT ON [coveringpermissionhierarchy] TO [SQLSecureView]
GRANT SELECT ON [currentversion] TO [SQLSecureView]
GRANT SELECT ON [databaseobject] TO [SQLSecureView]
GRANT SELECT ON [databaseobjectpermission] TO [SQLSecureView]
GRANT SELECT ON [databaseprincipal] TO [SQLSecureView]
GRANT SELECT ON [databaseprincipalpermission] TO [SQLSecureView]
GRANT SELECT ON [databaserolemember] TO [SQLSecureView]
GRANT SELECT ON [databaseschema] TO [SQLSecureView]
GRANT SELECT ON [databaseschemapermission] TO [SQLSecureView]
GRANT SELECT ON [endpoint] TO [SQLSecureView]
GRANT SELECT ON [filterrule] TO [SQLSecureView]
GRANT SELECT ON [filterruleclass] TO [SQLSecureView]
GRANT SELECT ON [filterruleheader] TO [SQLSecureView]
GRANT SELECT ON [fixedrolepermission] TO [SQLSecureView]
GRANT SELECT ON [groomingactivityhistory] TO [SQLSecureView]
GRANT SELECT ON [objecttype] TO [SQLSecureView]
GRANT SELECT ON [registeredserver] TO [SQLSecureView]
GRANT SELECT ON [serverfilterrule] TO [SQLSecureView]
GRANT SELECT ON [serverfilterruleheader] TO [SQLSecureView]
GRANT SELECT ON [serverpermission] TO [SQLSecureView]
GRANT SELECT ON [serverprincipal] TO [SQLSecureView]
GRANT SELECT ON [serverrolemember] TO [SQLSecureView]
GRANT SELECT ON [serversnapshot] TO [SQLSecureView]
GRANT SELECT ON [serverstatistic] TO [SQLSecureView]
GRANT SELECT ON [snapshothistory] TO [SQLSecureView]
GRANT SELECT ON [sqldatabase] TO [SQLSecureView]
GRANT SELECT ON [sqlserverobjecttype] TO [SQLSecureView]
GRANT SELECT ON [windowsaccount] TO [SQLSecureView]
GRANT SELECT ON [windowsgroupmember] TO [SQLSecureView]
GRANT SELECT ON [reports] TO [SQLSecureView]
GO

INSERT INTO [reports] 
	(
		[reportserver],
		[reportfolder], 
   		[reportingservicestemplate],
   		[maintemplate],
   		[reportstemplate],
   		[name1],[url1],[desc1],
   		[name2],[url2],[desc2],
   		[name3],[url3],[desc3],
   		[name4],[url4],[desc4],
   		[name5],[url5],[desc5])
 VALUES (
 		'','',
  		'http://{0}/Reports',
  		'http://{0}/Reports/Pages/Folder.aspx?ItemPath=%2f{1}',
  		'http://{0}/Reports/Pages/Report.aspx?ItemPath=%2f{1}%2f{2}',
  		'User Permissions',
              		'Security Report - User Permissions',
              		'List permissions assigned to a user.',
  		'Data Collection Filter',
              		'Control Report - Data Collection Filters',
              		'List data collection filters.',
  		'SQLsecure Users',
              		'Control Report - SQLsecure Users',
              		'List SQLsecure users.',
  		'SQLsecure Activity History',
              		'Control Report - SQLsecure Activity History',
              		'List SQLsecure activity.',
  		'Empty',
              		'Empty',
              		'Empty.'
  	)
GO

   -- Idera SQLsecure Version 2.0
   --
   -- (c) Copyright 2005-2008 Idera, a division of BBS Technologies, Inc., all rights reserved.
   -- SQLsecure, Idera and the Idera Logo are trademarks or registered trademarks
   -- of BBS Technologies or its subsidiaries in the United States and other jurisdictions.
   -- 
   -- Description :
   --              Update previous SQLsecure databases to version 2000


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


USE SQLsecure
GO


-- fix issues in schema 900 before upgrading to 2000

-- ancillarywindowsgroup is not linked to snapshot correctly and not deleted when a snapshot is deleted
DELETE FROM ancillarywindowsgroup WHERE snapshotid not in (SELECT snapshotid FROM serversnapshot)

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FK_ancillarywindowsgroup_serversnapshot]') AND type = 'F')
	ALTER TABLE [dbo].[ancillarywindowsgroup]  WITH CHECK ADD CONSTRAINT [FK_ancillarywindowsgroup_serversnapshot]
		FOREIGN KEY([snapshotid])
		REFERENCES [dbo].[serversnapshot] ([snapshotid])

ALTER TABLE [dbo].[ancillarywindowsgroup] CHECK CONSTRAINT [FK_ancillarywindowsgroup_serversnapshot]
GO

-- fix primary keys on permissions objects to avoid duplicates with multiple permission grants from different grantors

-- serverpermission table

-- to make it restartable, check if the index doesn't exist or the grantor column is not in the primary key
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[PK__serverpermission__2B3F6F97]') AND type = N'K')
	OR NOT EXISTS (SELECT a.id FROM dbo.sysindexes a, dbo.sysindexkeys b, dbo.syscolumns c WHERE a.name = N'PK__serverpermission__2B3F6F97' and a.id = b.id and a.indid = b.indid and b.id = c.id and b.colid = c.colid and c.name = 'grantor')
BEGIN
	UPDATE [serverpermission] SET [grantor]=1 WHERE [grantor] is null

	ALTER TABLE [serverpermission]
		ALTER COLUMN [grantor] int not null

	IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[PK__serverpermission__2B3F6F97]') AND type = N'K')
		ALTER TABLE [serverpermission]
			DROP CONSTRAINT [PK__serverpermission__2B3F6F97] 

	ALTER TABLE [serverpermission]
		ADD CONSTRAINT [PK__serverpermission__2B3F6F97] PRIMARY KEY CLUSTERED 
		(
			[snapshotid] ASC,
			[majorid] ASC,
			[minorid] ASC,
			[classid] ASC,
			[permission] ASC,
			[grantee] ASC,
			[grantor] ASC
		)
END

GO


-- databaseprincipalpermission table

-- to make it restartable, check if the index doesn't exist or the grantor column is not in the primary key
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[PK__databaseprincipa__0CBAE877]') AND type = N'K')
	OR NOT EXISTS (SELECT a.id FROM dbo.sysindexes a, dbo.sysindexkeys b, dbo.syscolumns c WHERE a.name = N'PK__databaseprincipa__0CBAE877' and a.id = b.id and a.indid = b.indid and b.id = c.id and b.colid = c.colid and c.name = 'grantor')
BEGIN
	UPDATE [databaseprincipalpermission] SET [grantor]=1 WHERE [grantor] is null

	ALTER TABLE [databaseprincipalpermission]
		ALTER COLUMN [grantor] int not null

	IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[PK__databaseprincipa__0CBAE877]') AND type = N'K')
		ALTER TABLE [databaseprincipalpermission]
			DROP CONSTRAINT [PK__databaseprincipa__0CBAE877] 

	ALTER TABLE [databaseprincipalpermission]
		ADD CONSTRAINT [PK__databaseprincipa__0CBAE877] PRIMARY KEY CLUSTERED 
		(
			[snapshotid] ASC,
			[dbid] ASC,
			[uid] ASC,
			[classid] ASC,
			[permission] ASC,
			[grantee] ASC,
			[grantor] ASC
		)
END

GO


-- databaseschemapermission table

-- to make it restartable, check if the index doesn't exist or the grantor column is not in the primary key
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[PK__databaseschemape__07020F21]') AND type = N'K')
	OR NOT EXISTS (SELECT a.id FROM dbo.sysindexes a, dbo.sysindexkeys b, dbo.syscolumns c WHERE a.name = N'PK__databaseschemape__07020F21' and a.id = b.id and a.indid = b.indid and b.id = c.id and b.colid = c.colid and c.name = 'grantor')
BEGIN
	UPDATE [databaseschemapermission] SET [grantor]=1 WHERE [grantor] is null

	ALTER TABLE [databaseschemapermission]
		ALTER COLUMN [grantor] int not null

	IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[PK__databaseschemape__07020F21]') AND type = N'K')
		ALTER TABLE [databaseschemapermission]
			DROP CONSTRAINT [PK__databaseschemape__07020F21] 

	ALTER TABLE [databaseschemapermission]
		ADD CONSTRAINT [PK__databaseschemape__07020F21] PRIMARY KEY CLUSTERED 
		(
			[snapshotid] ASC,
			[dbid] ASC,
			[schemaid] ASC,
			[classid] ASC,
			[permission] ASC,
			[grantee] ASC,
			[grantor] ASC
		)
END

GO


-- databaseobjectpermission table

-- to make it restartable, check if the index doesn't exist or the grantor column is not in the primary key
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[PK__databaseobjectpe__3F466844]') AND type = N'K')
	OR NOT EXISTS (SELECT a.id FROM dbo.sysindexes a, dbo.sysindexkeys b, dbo.syscolumns c WHERE a.name = N'PK__databaseobjectpe__3F466844' and a.id = b.id and a.indid = b.indid and b.id = c.id and b.colid = c.colid and c.name = 'grantor')
BEGIN
	UPDATE [databaseobjectpermission] SET [grantor]=1 WHERE [grantor] is null

	ALTER TABLE [databaseobjectpermission]
		ALTER COLUMN [grantor] int not null

	IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[PK__databaseobjectpe__3F466844]') AND type = N'K')
		ALTER TABLE [databaseobjectpermission]
			DROP CONSTRAINT [PK__databaseobjectpe__3F466844] 

	ALTER TABLE [databaseobjectpermission]
		ADD CONSTRAINT [PK__databaseobjectpe__3F466844] PRIMARY KEY CLUSTERED 
		(
			[snapshotid] ASC,
			[dbid] ASC,
			[objectid] ASC,
			[parentobjectid] ASC,
			[classid] ASC,
			[permission] ASC,
			[grantee] ASC,
			[grantor] ASC
		)
END

GO


-- drop unused tables
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sqlserverobjecttype]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE [dbo].[sqlserverobjecttype]

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[serverstatistic]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE [dbo].[serverstatistic]

/* ---------------------------------------------------------------------- */
/* Schema changes for SQLsecure 2.0 schema version 2000                   */
/* ---------------------------------------------------------------------- */

SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON

/* ---------------------------------------------------------------------- */
/* Validate the db is correct version for upgrade                         */
/*     upgrade dal version 900 or new installs                            */
/* ---------------------------------------------------------------------- */

declare @ver int
SELECT @ver=schemaversion FROM currentversion
if (isnull(@ver,900) = 900)
BEGIN

	/* ---------------------------------------------------------------------- */
	/* Update RegisteredServer Table                                          */
	/* ---------------------------------------------------------------------- */

	BEGIN TRANSACTION
	ALTER TABLE dbo.filterruleheader
		DROP CONSTRAINT FK__filterrul__conne__49C3F6B7

	ALTER TABLE dbo.registeredserver ADD
		registeredserverid int NOT NULL IDENTITY (1, 1),
		serverisdomaincontroller nchar(1) NULL,
		replicationenabled nchar(1) NULL,
		sapasswordempty nchar(1) NULL,
		connectionport int NULL

	ALTER TABLE dbo.registeredserver
		DROP CONSTRAINT PK__registeredserver__20C1E124

	ALTER TABLE dbo.registeredserver ADD CONSTRAINT
		PK__registeredserver__20C1E124 PRIMARY KEY CLUSTERED ([registeredserverid])

	CREATE UNIQUE NONCLUSTERED INDEX IX_registeredserver_connection ON dbo.registeredserver	(
		[connectionname]
	)

	ALTER TABLE [dbo].[filterruleheader]  WITH CHECK ADD  CONSTRAINT [FK__filterrul__conne__49C3F6B7]
		FOREIGN KEY ([connectionname])
		REFERENCES [dbo].[registeredserver] ([connectionname])

	ALTER TABLE [dbo].[filterruleheader] CHECK CONSTRAINT [FK__filterrul__conne__49C3F6B7]


	COMMIT



	/* ---------------------------------------------------------------------- */
	/* Add Policy Tables		                                          */
	/* ---------------------------------------------------------------------- */

	BEGIN TRANSACTION

	CREATE TABLE [dbo].[policy] (
		[policyid] [int] IDENTITY(1,1) NOT NULL,
		[policyname] [nvarchar](128) NOT NULL,
		[policydescription] [nvarchar](2048) NOT NULL,
		[issystempolicy] [bit] NOT NULL,
		[isdynamic] [bit] NOT NULL,
		[dynamicselection] [nvarchar](4000) NULL,
		CONSTRAINT [PK_policy] PRIMARY KEY CLUSTERED ([policyid])
	)


	CREATE NONCLUSTERED INDEX [IX_policyname] ON [dbo].[policy] (
		[policyname]
	)




	CREATE TABLE [dbo].[policymember] (
		[policyid] [int] NOT NULL,
		[registeredserverid] [int] NOT NULL,
		CONSTRAINT [PK_policymember] PRIMARY KEY CLUSTERED (
			[policyid],
			[registeredserverid]
		)
	)


	ALTER TABLE [dbo].[policymember]  WITH CHECK ADD CONSTRAINT [FK_policymember_policy]
		FOREIGN KEY ([policyid])
		REFERENCES [dbo].[policy] ([policyid])

	ALTER TABLE [dbo].[policymember] CHECK CONSTRAINT [FK_policymember_policy]

	ALTER TABLE [dbo].[policymember]  WITH CHECK ADD CONSTRAINT [FK_policymember_registeredserver]
		FOREIGN KEY ([registeredserverid])
		REFERENCES [dbo].[registeredserver] ([registeredserverid])

	ALTER TABLE [dbo].[policymember] CHECK CONSTRAINT [FK_policymember_registeredserver]




	CREATE TABLE [dbo].[policyinterview] (
		[policyinterviewid] [int] IDENTITY(1,1) NOT NULL,
		[policyid] [int] NOT NULL,
		[istemplate] [bit] NOT NULL,
		[interviewname] [nvarchar](256) NOT NULL,
		[interviewtext] [ntext] NULL,
		CONSTRAINT [PK_policyinterview] PRIMARY KEY CLUSTERED ([policyinterviewid])
	)


	CREATE NONCLUSTERED INDEX [IX_policyinterviewname] ON [dbo].[policyinterview] (
		[interviewname]
	)


	CREATE NONCLUSTERED INDEX [IX_policyinterviewtemplate] ON [dbo].[policyinterview] (
		[istemplate],
		[interviewname]
	)

	ALTER TABLE [dbo].[policyinterview]  WITH CHECK ADD CONSTRAINT [FK_policyinterview_policy]
		FOREIGN KEY([policyid])
		REFERENCES [dbo].[policy] ([policyid])

	ALTER TABLE [dbo].[policyinterview] CHECK CONSTRAINT [FK_policyinterview_policy]





	CREATE TABLE [dbo].[metric] (
		[metricid] [int] NOT NULL,
		[metrictype] [nvarchar](32) NOT NULL,
		[metricname] [nvarchar](256) NOT NULL,
		[metricdescription] [nvarchar](1024) NOT NULL,
		[isuserentered] [bit] NOT NULL,
		[ismultiselect] [bit] NOT NULL,
		[validvalues] [nvarchar](1024) NOT NULL,
		[valuedescription] [nvarchar](1024) NOT NULL
	 	CONSTRAINT [PK_metric] PRIMARY KEY CLUSTERED ([metricid])
	)

	CREATE NONCLUSTERED INDEX [IX_metric] ON [dbo].[metric] (
		[metrictype],
		[metricname]
	)





	CREATE TABLE [dbo].[policymetric] (
		[policyid] [int] NOT NULL,
		[metricid] [int] NOT NULL,
		[isenabled] [bit] NOT NULL,
		[reportkey] [nvarchar](32) NOT NULL,
		[reporttext] [nvarchar](4000) NOT NULL,
		[severity] [int] NOT NULL,
		[severityvalues] [nvarchar](4000) NULL,
	 	CONSTRAINT [PK_policymetric] PRIMARY KEY CLUSTERED (
			[policyid],
			[metricid]
		)
	)

	ALTER TABLE [dbo].[policymetric]  WITH CHECK ADD CONSTRAINT [FK_policymetric_metric]
		FOREIGN KEY([metricid])
		REFERENCES [dbo].[metric] ([metricid])

	ALTER TABLE [dbo].[policymetric] CHECK CONSTRAINT [FK_policymetric_metric]

	ALTER TABLE [dbo].[policymetric]  WITH CHECK ADD CONSTRAINT [FK_policymetric_policy]
		FOREIGN KEY([policyid])
		REFERENCES [dbo].[policy] ([policyid])

	ALTER TABLE [dbo].[policymetric] CHECK CONSTRAINT [FK_policymetric_policy]


	COMMIT




	/* ---------------------------------------------------------------------- */
	/* Add Notification Tables		                                  */
	/* ---------------------------------------------------------------------- */

	BEGIN TRANSACTION

	CREATE TABLE [dbo].[notificationprovider] (
		[notificationproviderid] [int] IDENTITY(1,1) NOT NULL,
		[providername] [nvarchar](64) NOT NULL,
		[providertype] [nvarchar](32) NOT NULL,
		[servername] [nvarchar](256) NOT NULL,
		[port] [int] NOT NULL,
		[timeout] [int] NULL,
		[requiresauthentication] [bit] NOT NULL,
		[username] [nvarchar](128) NULL,
		[password] [nvarchar](128) NULL,
		[sendername] [nvarchar](128) NULL,
		[senderemail] [nvarchar](128) NULL,
	 	CONSTRAINT [PK_notificationprovider] PRIMARY KEY CLUSTERED ([notificationproviderid])
	)

	CREATE NONCLUSTERED INDEX [IX_notificationprovider] ON [dbo].[notificationprovider] (
		[providertype],
		[providername]
	)




	CREATE TABLE [dbo].[registeredservernotification] (
		[registeredserverid] [int] NOT NULL,
		[notificationproviderid] [int] NOT NULL,
		[snapshotstatus] nchar(1) NOT NULL,
		[policymetricseverity] int NOT NULL,
		[recipients] nvarchar(1024) NOT NULL
	 	CONSTRAINT [PK_registeredservernotification] PRIMARY KEY CLUSTERED ([registeredserverid], [notificationproviderid])
	)

	ALTER TABLE [dbo].[registeredservernotification]  WITH CHECK ADD CONSTRAINT [FK_registeredservernotification_registeredserver]
		FOREIGN KEY ([registeredserverid])
		REFERENCES [dbo].[registeredserver] ([registeredserverid])

	ALTER TABLE [dbo].[registeredservernotification] CHECK CONSTRAINT [FK_registeredservernotification_registeredserver]

	ALTER TABLE [dbo].[registeredservernotification]  WITH CHECK ADD CONSTRAINT [FK_registeredservernotification_notificationprovider]
		FOREIGN KEY ([notificationproviderid])
		REFERENCES [dbo].[notificationprovider] ([notificationproviderid])

	ALTER TABLE [dbo].[registeredservernotification] CHECK CONSTRAINT [FK_registeredservernotification_notificationprovider]



	COMMIT





	/* ---------------------------------------------------------------------- */
	/* Add Server OS Tables		                                          */
	/* ---------------------------------------------------------------------- */

	BEGIN TRANSACTION


	CREATE TABLE [dbo].[serverosobject] (
		[snapshotid] [int] NOT NULL,
		[osobjectid] [int] NOT NULL,
		[objecttype] [nvarchar](16) NOT NULL,
		[dbid] [int] NULL,
		[objectname] [nvarchar](260) NOT NULL,
		[longname] [ntext] NULL,
		[ownersid] [varbinary](85) NULL,
		[disktype] [nvarchar](16) NULL,
	 	CONSTRAINT [PK_serverosobject] PRIMARY KEY CLUSTERED (
			[snapshotid],
			[osobjectid]
		)
	)

	CREATE NONCLUSTERED INDEX [IX_serverosobject] ON [dbo].[serverosobject] (
		[snapshotid],
		[objecttype],
		[objectname]
	)

	ALTER TABLE [dbo].[serverosobject]  WITH CHECK ADD CONSTRAINT [FK_serverosobject_serversnapshot]
		FOREIGN KEY([snapshotid])
		REFERENCES [dbo].[serversnapshot] ([snapshotid])

	ALTER TABLE [dbo].[serverosobject] CHECK CONSTRAINT [FK_serverosobject_serversnapshot]





	CREATE TABLE [dbo].[serverosobjectpermission] (
		[snapshotid] [int] NOT NULL,
		[osobjectid] [int] NOT NULL,
		[auditflags] [int] NULL,
		[filesystemrights] [int] NOT NULL,
		[sid] [varbinary](85) NOT NULL,
		[accesstype] [int] NULL,
		[isinherited] [nchar](1) NOT NULL,
	)

	CREATE CLUSTERED INDEX [IX_serverosobjectpermission] ON [dbo].[serverosobjectpermission] (
		[snapshotid],
		[osobjectid],
		[sid]
	)

	ALTER TABLE [serverosobjectpermission] WITH CHECK ADD CONSTRAINT [FK__serverosobjectpermission__serverosobject] 
		FOREIGN KEY ([snapshotid], [osobjectid])
		REFERENCES [serverosobject] ([snapshotid], [osobjectid])

	ALTER TABLE [dbo].[serverosobjectpermission] CHECK CONSTRAINT [FK__serverosobjectpermission__serverosobject]





	CREATE TABLE [serveroswindowsaccount] (
		[snapshotid] INTEGER NOT NULL,
		[sid] VARBINARY(85) NOT NULL,
		[type] NVARCHAR(128),
		[name] NVARCHAR(200),
		[state] NCHAR(1),
		[hashkey] NVARCHAR(256),
		CONSTRAINT [PK__serveroswindowsaccount__1273C1CD] PRIMARY KEY (
			[snapshotid],
			[sid]
		)
	)

	ALTER TABLE [serveroswindowsaccount] ADD CONSTRAINT [FK__serveroswindowsac__snaps__1367E606] 
		FOREIGN KEY ([snapshotid])
		REFERENCES [serversnapshot] ([snapshotid])





	CREATE TABLE [serveroswindowsgroupmember] (
		[snapshotid] INTEGER NOT NULL,
		[groupsid] VARBINARY(85) NOT NULL,
		[groupmember] VARBINARY(85) NOT NULL,
		[hashkey] NVARCHAR(256),
		CONSTRAINT [PK__serveroswindowsgroupmemb__2F10007B] PRIMARY KEY (
			[snapshotid],
			[groupsid],
			[groupmember]
		)
	)

	ALTER TABLE [serveroswindowsgroupmember] ADD CONSTRAINT [FK__serveroswindowsgroupmemb__300424B4] 
		FOREIGN KEY (
			[snapshotid],
			[groupsid]
		)
		REFERENCES [serveroswindowsaccount] (
			[snapshotid],
			[sid]
		)

	ALTER TABLE [serveroswindowsgroupmember] ADD CONSTRAINT [FK__serveroswindowsgroupmemb__30F848ED] 
		FOREIGN KEY (
			[snapshotid],
			[groupmember]
		)
		REFERENCES [serveroswindowsaccount] (
			snapshotid,
			sid
		)





	CREATE TABLE [dbo].[serverservice] (
		[snapshotid] [int] NOT NULL,
		[servicetype] [int] NOT NULL,
		[servicename] [nvarchar](256) NOT NULL,
		[displayname] [nvarchar](256) NOT NULL,
		[servicepath] [nvarchar](1024) NOT NULL,
		[startuptype] [nvarchar](32) NOT NULL,
		[state] [nvarchar](32) NOT NULL,
		[loginname] [nvarchar](200) NOT NULL,
		CONSTRAINT [PK__serverservice] PRIMARY KEY (
			[snapshotid],
			[servicetype],
			[servicename]
		)
	)

	ALTER TABLE [dbo].[serverservice]  WITH CHECK ADD CONSTRAINT [FK_serverservice_serversnapshot]
		FOREIGN KEY([snapshotid])
		REFERENCES [dbo].[serversnapshot] ([snapshotid])

	ALTER TABLE [dbo].[serverservice] CHECK CONSTRAINT [FK_serverservice_serversnapshot]





	CREATE TABLE [dbo].[serverprotocol] (
		[snapshotid] [int] NOT NULL,
		[protocolname] [nvarchar](256) NOT NULL,
		[ipaddress] [nvarchar](64) NULL,
		[dynamicport] [nchar](1) NULL,
		[port] [nvarchar](2047) NULL,
		[enabled] [nchar](1) NULL,
		[active] [nchar](1) NULL
	)

	CREATE CLUSTERED INDEX [IX_serverprotocol] ON [dbo].[serverprotocol] (
		[snapshotid],
		[protocolname],
		[ipaddress]
	)

	ALTER TABLE [dbo].[serverprotocol]  WITH CHECK ADD CONSTRAINT [FK_serverprotocol_serversnapshot]
		FOREIGN KEY([snapshotid])
		REFERENCES [dbo].[serversnapshot] ([snapshotid])

	ALTER TABLE [dbo].[serverprotocol] CHECK CONSTRAINT [FK_serverprotocol_serversnapshot]


	COMMIT





	/* ---------------------------------------------------------------------- */
	/* Add Snapshot	Collection fields                                         */
	/* ---------------------------------------------------------------------- */


	ALTER TABLE dbo.serversnapshot ADD
		[registeredserverid] int NULL,
		[collectorversion] nvarchar(32) NULL,
		[allowsystemtableupdates] nchar(1) NULL,
		[remoteadminconnectionsenabled] nchar(1) NULL,
		[remoteaccessenabled] nchar(1) NULL,
		[scanforstartupprocsenabled] nchar(1) NULL,
		[sqlmailxpsenabled] nchar(1) NULL,
		[databasemailxpsenabled] nchar(1) NULL,
		[oleautomationproceduresenabled] nchar(1) NULL,
		[webassistantproceduresenabled] nchar(1) NULL,
		[xp_cmdshellenabled] nchar(1) NULL,
		[agentmailprofile] nvarchar(128) NULL,
		[hideinstance] nchar(1) NULL,
		[agentsysadminonly] nchar(1) NULL,
		[serverisdomaincontroller] nchar(1) NULL,
		[replicationenabled] nchar(1) NULL,
		[sapasswordempty] nchar(1) NULL




	ALTER TABLE dbo.sqldatabase ADD
		[dbfilename] nvarchar(260) NULL,
		[replicationcategory] int NULL,
		[isaudited] nchar(1) NULL




	ALTER TABLE dbo.databaseobject ADD
		[runatstartup] nchar(1) NULL,
		[isencrypted] nchar(1) NULL,
		[userdefined] nchar(1) NULL




	ALTER TABLE dbo.serverprincipal ADD
		[ispasswordnull] nchar(1) NULL,
		[defaultdatabase] nvarchar(128) NULL,
		[defaultlanguage] nvarchar(128) NULL


	-- allow for longer warning messages with new metrics
	ALTER TABLE serversnapshot 
		ALTER COLUMN snapshotcomment nvarchar(1024) NULL

	declare @err int

	SELECT @err=@@ERROR
	if (@err > 0)
		ROLLBACK
END

GO


/* ---------------------------------------------------------------------- */
/*	Updates to new fields must be done after finalizing the changes		  */
/* ---------------------------------------------------------------------- */
declare @ver int
SELECT @ver=schemaversion FROM currentversion
if (isnull(@ver,900) = 900)
BEGIN
	BEGIN TRANSACTION

	-- set any existing snapshots to the correct registeredserverid
	if exists (select * from dbo.syscolumns where name = 'registeredserverid' and id = object_id('serversnapshot'))
	begin
		UPDATE dbo.serversnapshot SET [registeredserverid] = registeredserver.[registeredserverid]
			 FROM dbo.registeredserver WHERE registeredserver.connectionname = serversnapshot.connectionname

		-- fix special case where snapshots are waiting to be groomed and there is no server to connect them to
		--		so just set the server to a dummy id that should never be used
		UPDATE dbo.serversnapshot SET [registeredserverid] = -1
			 WHERE [registeredserverid] is null

		-- require that every snapshot have a registeredserverid
		ALTER TABLE dbo.serversnapshot ALTER COLUMN registeredserverid int NOT NULL

		-- Note: do not add a foreign key to registeredserver because removing a server leaves snapshots until grooming


		-- set all new fields to unknown or empty for existing snapshots to prevent null errors
		UPDATE dbo.serversnapshot SET [allowsystemtableupdates] = N'U',
										[remoteadminconnectionsenabled] = N'U',
										[remoteaccessenabled] = N'U',
										[scanforstartupprocsenabled] = N'U',
										[sqlmailxpsenabled] = N'U',
										[databasemailxpsenabled] = N'U',
										[oleautomationproceduresenabled] = N'U',
										[webassistantproceduresenabled] = N'U',
										[xp_cmdshellenabled] = N'U',
										[agentmailprofile] = N'',
										[hideinstance] = N'U',
										[agentsysadminonly] = N'U',
										[serverisdomaincontroller] = N'U',
										[replicationenabled] = N'U',
										[sapasswordempty] = N'U'
	end


	COMMIT



	-- set all existing dbs to isaudited = 'Y'
	if exists (select * from dbo.syscolumns where name = 'isaudited' and id = object_id('sqldatabase'))
		UPDATE dbo.sqldatabase SET [isaudited] = N'Y'
END

GO

declare @ver int
SELECT @ver=schemaversion FROM currentversion
if (isnull(@ver,900) = 900)
BEGIN
	-- provide view access to tables

	GRANT SELECT ON dbo.[policy] TO SQLSecureView
	GRANT SELECT ON dbo.[policymember] TO SQLSecureView
	GRANT SELECT ON dbo.[policyinterview] TO SQLSecureView
	GRANT SELECT ON dbo.[policymetric] TO SQLSecureView
	GRANT SELECT ON dbo.[notificationprovider] TO SQLSecureView
	GRANT SELECT ON dbo.[registeredservernotification] TO SQLSecureView
	GRANT SELECT ON dbo.[serverosobject] TO SQLSecureView
	GRANT SELECT ON dbo.[serverosobjectpermission] TO SQLSecureView
	GRANT SELECT ON dbo.[serveroswindowsaccount] TO SQLSecureView
	GRANT SELECT ON dbo.[serveroswindowsgroupmember] TO SQLSecureView
	GRANT SELECT ON dbo.[serverservice] TO SQLSecureView
	GRANT SELECT ON dbo.[serverprotocol] TO SQLSecureView
	--GO

	-- initial data loading for policies and metrics
	delete from policymetric
	delete from metric
	delete from policy

	SET IDENTITY_INSERT dbo.policy ON
	--GO

	declare @sql nvarchar(2048)
	-- add the default policy to be used for setting the default values when a new policy is created
	select @sql = 'insert into policy (policyid, policyname, policydescription, issystempolicy, isdynamic)
						values (0, ''Default'', ''Default policy settings used when creating a new policy'', 1, 0);'
	-- add the "All Servers" policy
	select @sql = @sql + ' insert into policy (policyid, policyname, policydescription, issystempolicy, isdynamic)
							values (1, ''All Servers'', ''Global security checks that should be performed on all audited SQL Servers'', 1, 1)'

	exec (@sql)		-- use dynamic sql so there won't be missing column errors on future upgrades since isdynamic has been removed

	--GO
	SET IDENTITY_INSERT dbo.policy OFF
	--GO


	-- add metrics and default policy values

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (1, 'Audit Data Is Stale', 'Data Integrity', 1, 0, '', 'When enabled, this check will identify a risk if audit data was not collected within the specified timeframe. Specify the number of days audit data is considered valid.',
									'Determine whether the nearest snapshot collection occurred within an acceptable timeframe from the selected date')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 1, 1, 2, '''30''', '',
									'Was the most recent snapshot collected within an acceptable timeframe?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (2, 'SQL Server Version', 'Configuration', 1, 1, '', 'When enabled, this check will identify a risk if the SQL Server version is below the minimum acceptable level. Specify the minimum acceptable level for each SQL Server version.',
									'Determine whether the SQL Server software is at an acceptable minimum version')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 2, 1, 1, '''8.00.2039'',''9.00.4035'',''10.0.1600''', '',
									'Is SQL Server below the minimum acceptable version?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (3, 'SQL Authentication Enabled', 'Login', 0, 0, '', 'When enabled, this check will identify a risk if SQL Authentication is enabled on the SQL Server.',
									'Determine whether SQL Authentication is allowed on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 3, 1, 1, '', '',
									'Is SQL Authentication enabled on the SQL Server?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (4, 'Login Audit Level', 'Auditing', 0, 1, '''None'':''None'', ''Sucessful logins only'':''Success'', ''Failed logins only'':''Failure'',''Both failed and successful logins'':''All''', 'When enabled, this check will identify a risk if the expected login auditing configuration is not being used by the SQL Server. Specify the expected login auditing configuration.',
									'Determine whether the SQL Server login auditing configuration is acceptable')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 4, 1, 1, '''Failure'',''All''', '',
									'Is the login auditing configuration acceptable?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (5, 'Cross Database Ownership Chaining Enabled', 'Surface Area', 0, 0, '', 'When enabled, this check will identify a risk if Cross Database Chaining is enabled on the SQL Server.',
									'Determine whether Cross Database Ownership Chaining is enabled on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 5, 1, 1, '', '',
									'Is Cross Database Ownership Chaining enabled on the SQL Server?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (6, 'Guest User Enabled', 'Access', 1, 1, '', 'When enabled, this check will identify a risk if Guest user access is available on unapproved databases on the SQL Server. Specify the databases on which Guest user access is approved.',
									'Determine whether Guest user access is available on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 6, 1, 1, '''master'',''tempdb''', '',
									'Is Guest user access available on the SQL Server?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (7, 'Suspect Logins', 'Login', 0, 0, '', 'When enabled, this check will identify a risk if Windows logins exist that could not be resolved in Active Directory.',
									'Determine whether suspect logins exist on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 7, 1, 1, '', '',
									'Do suspect logins exist on the SQL Server?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (8, 'C2 Audit Trace Enabled', 'Auditing', 0, 0, '', 'When enabled, this check will identify a risk if C2 Audit Trace is not enabled on the SQL Server.',
									'Determine whether C2 Audit Trace is enabled on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 8, 1, 1, '', '',
									'Is C2 Audit Trace enabled on the SQL Server?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (9, 'xp_cmdshell Proxy Account Exists', 'Configuration', 0, 0, '', 'When enabled, this check will identify a risk if a Proxy Account is enabled on the SQL Server.',
									'Determine whether a Proxy Account is enabled on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 9, 1, 1, '', '',
									'Is a Proxy Account enabled on the SQL Server?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (10, 'DAC Remote Access', 'Access', 0, 0, '', 'When enabled, this check will identify a risk if Dedicated Administrator Connection is available remotely on the SQL Server.',
									'Determine whether the Dedicated Administrator Connection is available remotely')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 10, 1, 1, '', '',
									'Is Dedicated Administrator Connection enabled remotely on the SQL Server?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (11, 'Integration Services', 'Configuration', 1, 1, '', 'When enabled, this check will identify a risk if any Integration Services stored procedures have been assigned permissions. Specify the stored procedures.',
									'Determine whether permissions have been granted on Integration Services stored procedures')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 11, 1, 1, '''sp_add_dtspackage'',''sp_enum_dtspackages'',''sp_add_job'',''sp_add_jobstep''', '',
									'Has anyone been granted permission to execute Integration Services stored procedures on the SQL Server?')

	--this check has been removed, but a placeholder is left here so it is not reused
	-- this one may be reactivated in the future after further definition
	--insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
	--                values (12, 'OLAP SQL Authentication Enabled', 'Configuration', 0, 0, '', 'When enabled, this check will identify a risk if OLAP SQL Authentication is enabled on the SQL Server.',
	--                                'Determine whether OLAP SQL Authentication is enabled on the SQL Server')
	--insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
	--                values (0, 12, 1, 1, '', '',
	--                                'Is OLAP SQL Authentication enabled?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (13, 'SQL Mail or Database Mail Enabled', 'Configuration', 0, 0, '', 'When enabled, this check will identify a risk if SQL Mail or Database Mail are enabled on the SQL Server.',
									'Determine whether SQL Mail or Database Mail has been enabled on the SQL Server') 
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 13, 1, 1, '', '',
									'Is SQL Mail or Database Mail enabled?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (14, 'SQL Agent Mail', 'Configuration', 0, 0, '', 'When enabled, this check will identify a risk if a SQL Agent Mail profile exists on the SQL Server.',
									'Determine whether the SQL Server Agent has been configured to allow email')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 14, 1, 1, '', '',
									'Is SQL Agent Mail enabled?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (15, 'Sample Databases Exist', 'Configuration', 1, 1, '', 'When enabled, this check will identify a risk if any sample databases exist on the SQL Server. Specify the sample databases.',
									'Determine whether sample databases exist on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 15, 1, 1, '''Northwind'',''pubs'',''AdventureWorks'',''AdventureWorksDW''', '',
									'Do the SQL Server sample databases exist?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (16, 'sa Account Not Disabled or Renamed', 'Configuration', 0, 0, '', 'When enabled, this check will identify a risk if the SQL Server sa account is enabled and not renamed on SQL Server 2005.',
									'Determine whether the SQL Server sa account has been disabled or renamed on SQL Server 2005')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 16, 1, 1, '', '',
									'Does the SQL Server sa account exist unchanged?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (17, 'sa Account Has Blank Password', 'Login', 0, 0, '', 'When enabled, this check will identify a risk if the SQL Server sa account has a blank password.',
									'Determine whether the SQL Server sa account has a blank password')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 17, 1, 3, '', '',
									'Does SQL Server sa account have a blank password?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (18, 'System Table Updates', 'Configuration', 0, 0, '', 'When enabled, this check will identify a risk if System Table Updates is configured.',
									'Determine whether the "Allow Updates to System Tables" configuration option is enabled on SQL Server 2005')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 18, 1, 1, '', '',
									'Are System Table Updates allowed?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (19, 'Everyone System Table Access', 'Permissions', 0, 0, '', 'When enabled, this check will identify a risk if Everyone has access to system tables on the SQL Server.',
									'Determine whether the Everyone group has read access to system tables on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 19, 1, 1, '', '',
									'Does Everyone have read access to system tables?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (20, 'Startup Stored Procedures Enabled', 'Access', 0, 0, '', 'When enabled, this check will identify a risk if "Scan for startup stored procedures" is enabled on the SQL Server.',
									'Determine whether the "Scan for startup stored procedures" configuration option has been enabled on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 20, 1, 1, '', '',
									'Are startup stored procedures enabled?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (21, 'Startup Stored Procedures', 'Access', 1, 1, '', 'When enabled, this check will identify a risk if an unapproved stored procedure is set to run at startup on the SQL Server. Specify the approved startup stored procedures.',
									'Determine whether there are unapproved stored procedures set to run at startup on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 21, 0, 1, '''none''', '',    -- disabled by default, the list must be entered by user
									'Are any unapproved stored procedures set to run at startup?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (22, 'Stored Procedures Encrypted', 'Access', 0, 0, '', 'When enabled, this check will identify a risk if any user stored procedures are not encrypted on the SQL Server.',
									'Determine whether user stored procedures are encrypted on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 22, 1, 1, '', '',
									'Are any user stored procedures not encrypted?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (23, 'User Defined Extended Stored Procedures (XSPs)', 'Access', 1, 1, '', 'When enabled, this check will identify a risk if any unapproved user-defined Extended Stored Procedures (XSPs) exist on the SQL Server. Specify the approved user-defined XSPs.',
									'Determine whether unapproved user-defined Extended Stored Procedures (XSPs) exist')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 23, 1, 1, '''none''', '',    -- by default, any user extended stored procedures are bad
									'Do user-defined Extended Stored Procedures (XSPs) exist?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (24, 'Dangerous Extended Stored Procedures (XSPs)', 'Access', 1, 1, '', 'When enabled, this check will identify a risk if any dangerous Extended Stored Procedure (XSPs) have been assigned permissions. Specify which XSPs you consider dangerous.',
									'Determine whether permissions have been granted on dangerous Extended Stored Procedures (XSPs)')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 24, 0, 1, '', '',    -- by default, other alerts cover most of them, so this is the list of unusual ones for the user
									'Has anyone been granted permission to execute dangerous Extended Stored Procedures (XSPs)?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (25, 'Remote Access', 'Access', 0, 0, '', 'When enabled, this check will identify a risk if Remote Access is enabled on the SQL Server.',
									'Determine whether Remote Access is enabled on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 25, 1, 1, '', '',
									'Is Remote Access enabled?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (26, 'Unapproved Protocols', 'Surface Area', 0, 1, '''Named Pipes'':''Named Pipes'',''NWLink IPX/SPX (2000)'':''NWLink IPX/SPX'',''Shared Memory (2005)'':''Shared Memory'',''TCP/IP'':''TCP/IP'',''VIA'':''VIA''', 'When enabled, this check will identify a risk if unapproved protocols are enabled. Specify the approved protocols.',
									'Determine whether unapproved protocols are enabled on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 26, 1, 1, '''TCP/IP''', '',
									'Are unapproved protocols enabled?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (27, 'Common TCP Port Used', 'Surface Area', 1, 1, '', 'When enabled, this check will identify a risk if common TCP ports are used by SQL Server. Specify the common TCP ports.',
									'Determine whether TCP is using a common port on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 27, 1, 1, '''1433'',''1434''', '',
									'Are common TCP ports used?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (28, 'SQL Server Available For Browsing', 'Surface Area', 0, 0, '', 'When enabled, this check will identify a risk if SQL Server is visible for browsing from client computers.',
									'Determine whether the SQL Server is hidden from client computers')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 28, 1, 1, '', '',
									'Is SQL Server visible to client computers?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (29, 'Agent Job Execution', 'Permissions', 0, 0, '', 'When enabled, this check will identify a risk if anyone who is not an administrator has permission to execute SQL Agent CmdExec jobs on the SQL Server.',
									'Determine whether only administrators can execute SQL Agent CmdExec Jobs')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 29, 1, 1, '', '',
									'Can anyone besides administrators execute SQL Agent CmdExec jobs on the SQL Server?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (30, 'Replication Enabled', 'Configuration', 0, 0, '', 'When enabled, this check will identify a risk if replication is enabled on the SQL Server.',
									'Determine whether replication is enabled on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 30, 1, 1, '', '',
									'Is replication enabled on the SQL Server?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (31, 'Registry Key Owners Not Acceptable', 'Permissions', 1, 1, '', 'When enabled, this check will identify a risk if unapproved registry key owners exist. Specify the approved owners. Can use ''%'' as wildcard.',
									'Determine whether registry keys that can affect SQL Server security have unapproved owners')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 31, 0, 1, '', '',    -- disabled by default, user must enter list of valid owners
									'Do unapproved registry key owners exist?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (32, 'Registry Key Permissions Not Acceptable', 'Permissions', 1, 1, '', 'When enabled, this check will identify a risk if any unapproved user has permissions on a registry key that affects SQL Server security. Specify the approved users. Can use ''%'' as wildcard.',
									'Determine whether users have unapproved access to registry keys')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 32, 0, 1, '', '',    -- disabled by default, user must enter list of valid permission grantees
									'Do users have unapproved access to registry keys?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (33, 'Files On Drives Not Using NTFS', 'Access', 0, 0, '', 'When enabled, this check will identify a risk if any SQL Server files are not stored on drives using NTFS.',
									'Determine whether all SQL Server files are stored on drives that use NTFS')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 33, 1, 2, '', '',
									'Are any SQL Server files on drives not using NTFS?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (34, 'Executable File Owners Not Acceptable', 'Permissions', 1, 1, '', 'When enabled, this check will identify a risk if unapproved executable file owners exist. Specify the approved owners. Can use ''%'' as wildcard.',
									'Determine whether SQL Server executable files have unapproved owners')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 34, 0, 1, '', '',    -- disabled by default, user must enter list of valid owners
									'Do unapproved executable file owners exist?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (35, 'Executable File Permissions Not Acceptable', 'Permissions', 1, 1, '', 'When enabled, this check will identify a risk if any unapproved user has permissions on SQL Server executable file. Specify the approved users. Can use ''%'' as wildcard.',
									'Determine whether users have unapproved access to SQL Server executable files')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 35, 0, 2, '', '',    -- disabled by default, user must enter list of valid permission grantees
									'Do users have unapproved access to executable files?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (36, 'Database File Owners Not Acceptable', 'Permissions', 1, 1, '', 'When enabled, this check will identify a risk if unapproved database file owners exist. Specify the approved owners. Can use ''%'' as wildcard.',
									'Determine whether SQL Server database files have unapproved owners')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 36, 0, 1, '', '',    -- disabled by default, user must enter list of valid owners
									'Do unapproved database file owners exist?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (37, 'Everyone Database File Access', 'Permissions', 0, 0, '', 'When enabled, this check will identify a risk if Everyone has access to database files.',
									'Determine whether the Everyone group has access to SQL Server database files')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 37, 1, 3, '', '',
									'Does Everyone have access to database files?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (38, 'Database File Permissions Not Acceptable', 'Permissions', 1, 1, '', 'When enabled, this check will identify a risk if any unapproved user has permissions on database files. Specify the approved users. Can use ''%'' as wildcard.',
									'Determine whether users have unapproved access to SQL Server database files')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 38, 0, 1, '', '',    -- disabled by default, user must enter list of valid permission grantees
									'Do users have unapproved access to database files?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (39, 'Operating System Version', 'Access', 1, 1, '', 'When enabled, this check will identify a risk if the Operating System is not at an acceptable version. Specify the acceptable OS versions.',
									'Determine whether the Operating System version is at an acceptable level')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 39, 1, 1, '''Microsoft(R) Windows(R) Server 2003, Standard Edition, Service Pack 2''', '',
									'Is OS version at an acceptable level?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (40, 'SQL Server Service Login Account Not Acceptable', 'Login', 1, 1, '', 'When enabled, this check will identify a risk if the SQL Server Service account is not acceptable. Specify the acceptable login accounts.',
									'Determine whether the SQL Server Service account is acceptable')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 40, 0, 1, '', '',    -- disabled by default, user must enter list of logins
									'Is an unacceptable SQL Server Service account being used?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (41, 'Reporting Services Running', 'Surface Area', 0, 0, '', 'When enabled, this check will identify a risk if Microsoft Reporting Services is running on the SQL Server.',
									'Determine whether Microsoft Reporting Services is running on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 41, 1, 1, '', '',
									'Are Microsoft Reporting Services running?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (42, 'Analysis Services Running', 'Configuration', 0, 0, '', 'When enabled, this check will identify a risk if Analysis Services (OLAP) is running on the SQL Server.',
									'Determine whether Analysis Services (OLAP) is running on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 42, 1, 1, '', '',
									'Are Analysis Services (OLAP) running?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (43, 'Analysis Services Login Account Not Acceptable', 'Login', 1, 1, '', 'When enabled, this check will identify a risk if the Analysis Services account is not acceptable. Specify the acceptable login accounts.',
									'Determine whether the Analysis Services account is acceptable')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 43, 0, 1, '', '',    -- disabled by default, user must enter list of logins
									'Is an unacceptable Analysis Services account being used?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (44, 'Notification Services Running', 'Surface Area', 0, 0, '', 'When enabled, this check will identify a risk if Notification Services is running on the SQL Server.',
									'Determine whether Notification Services is running on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 44, 1, 1, '', '',
									'Are Notification Services running?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (45, 'Notification Services Login Account Not Acceptable', 'Login', 1, 1, '', 'When enabled, this check will identify a risk if the Notification Services account is not acceptable. Specify the acceptable login accounts.',
									'Determine whether the Notification Services account is acceptable')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 45, 0, 1, '', '',    -- disabled by default, user must enter list of logins
									'Is an unacceptable Notification Services account being used?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (46, 'Integration Services Running', 'Surface Area', 0, 0, '', 'When enabled, this check will identify a risk if Integration Services is running on the SQL Server.',
									'Determine whether Integration Services is running on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 46, 1, 1, '', '',
									'Are Integration Services running?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (47, 'Integration Services Login Account Not Acceptable', 'Login', 1, 1, '', 'When enabled, this check will identify a risk if the Integration Service account is not acceptable. Specify the acceptable login accounts.',
									'Determine whether the Integration Services account is acceptable')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 47, 0, 1, '', '',    -- disabled by default, user must enter list of logins
									'Is an unacceptable Integration Services account being used?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (48, 'SQL Server Agent Running', 'Surface Area', 0, 0, '', 'When enabled, this check will identify a risk if SQL Server Agent is running on the SQL Server.',
									'Determine whether the SQL Server Agent is running on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 48, 1, 1, '', '',
									'Is the SQL Server Agent running?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (49, 'SQL Server Agent Login Account Not Acceptable', 'Login', 1, 1, '', 'When enabled, this check will identify a risk if the SQL Server Agent Service account is not acceptable. Specify the acceptable login accounts.',
									'Determine whether the SQL Server Agent Service account is acceptable')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 49, 0, 1, '', '',    -- disabled by default, user must enter list of logins
									'Is an unacceptable SQL Server Agent Service account being used?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (50, 'Full-Text Search Running', 'Configuration', 0, 0, '', 'When enabled, this check will identify a risk if Full-Text Search is running on the SQL Server.',
									'Determine whether Full-Text Search is running on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 50, 1, 1, '', '',
									'Is Full-Text Search running?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (51, 'Full-Text Search Login Account Not Acceptable', 'Login', 1, 1, '', 'When enabled, this check will identify a risk if the Full-Text Search Service account is not acceptable. Specify the acceptable login accounts.',
									'Determine whether the Full-Text Search Service account is acceptable')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 51, 0, 1, '', '',    -- disabled by default, user must enter list of logins
									'Is an unacceptable Full-Text Search Service account being used?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (52, 'SQL Server Browser Running', 'Access', 0, 0, '', 'When enabled, this check will identify a risk if SQL Server Browser is running on the SQL Server.',
									'Determine whether the SQL Server Browser is running on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 52, 1, 1, '', '',
									'Is the SQL Server Browser running?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (53, 'SQL Server Browser Login Account Not Acceptable', 'Login', 1, 1, '', 'When enabled, this check will identify a risk if the SQL Server Browser Service account is not acceptable. Specify the acceptable login accounts.',
									'Determine whether the SQL Server Browser Service account is acceptable')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 53, 0, 1, '', '',    -- disabled by default, user must enter list of logins
									'Is an unacceptable SQL Server Browser Service account being used?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (54, 'Snapshot Not Found', 'Data Integrity', 0, 0, '', 'When enabled, this check will identify a risk if audit data is missing.',
									'Determine whether all servers in the policy have valid audit data for the selected timeframe')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 54, 1, 2, '', '',
									'Are any servers in the policy missing audit data?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (55, 'Snapshot May Be Missing Data', 'Data Integrity', 0, 0, '', 'When enabled, this check will identify a risk if audit data is incomplete or the snapshot returned warnings.',
									'Determine whether all audit data for the selected servers is complete and without warnings')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 55, 1, 2, '', '',
									'Is audit data incomplete?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (56, 'Baseline Data Not Being Used', 'Data Integrity', 0, 0, '', 'When enabled, this check will identify a risk if audit data is not from baseline snapshot.',
									'Determine whether all audit data for the selected timeframe is from baseline snapshots')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 56, 0, 2, '', '',
									'Is any audit data from a non-baseline snapshot?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (57, 'SQL Logins Not Using Password Policy', 'Login', 0, 0, '', 'When enabled, this check will identify a risk if any SQL Login is not protected by the password policy.',
									'Determine whether the password policy is enabled for all SQL Logins')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 57, 1, 2, '', '',
									'Is the password policy enabled for all SQL Logins?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (58, 'Public Database Role Has Permissions', 'Permissions', 0, 0, '', 'When enabled, this check will identify a risk if the public database role has been granted any permissions or been made a member of any other role.',
									'Determine whether the public database role has any permissions')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 58, 1, 3, '', '',
									'Are any permissions granted to the public database role?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (59, 'Blank Passwords', 'Login', 0, 0, '', 'When enabled, this check will identify a risk if any SQL Login has a blank password.',
									'Determine whether any SQL Logins have blank passwords')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 59, 1, 3, '', '',
									'Does any SQL Login have a blank password?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (60, 'Fixed roles assigned to public or guest', 'Access', 0, 0, '', 'When enabled, this check will identify a risk if the public role or guest user are members of any fixed database roles.',
									'Determine whether public or guest are members of any fixed database roles')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 60, 1, 3, '', '',
									'Are any fixed roles assigned to the public role or guest user?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (61, 'BUILTIN/Administrators is sysadmin', 'Configuration', 0, 0, '', 'When enabled, this check will identify a risk if the BUILTIN/Administrators local Windows group is a member of the sysadmin fixed server role.',
									'Determine whether BUILTIN/Administrators is a member of the sysadmin fixed server role')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 61, 1, 2, '', '',
									'Is the BUILTIN/Administrators group assigned to the sysadmin role?')



	-- now copy all of the defaults to the All Servers policy
	insert into policymetric (policyid, metricid, isenabled, reportkey, reporttext, severity, severityvalues)
		select 1, metricid, isenabled, reportkey, reporttext, severity, severityvalues
			from policymetric
			where policyid = 0


	--GO




	-- add default notification provider to prevent errors in saving registeredserver email notifications on upgrade
	SET IDENTITY_INSERT dbo.notificationprovider ON
	--GO

	INSERT INTO [SQLsecure].[dbo].[notificationprovider]
		   (
				[notificationproviderid],
				[providername],
				[providertype],
				[servername],
				[port],
				[timeout],
				[requiresauthentication],
				[username],
				[password],
				[sendername],
				[senderemail]
			)
		VALUES (
				1,
				'SQLsecure',
				'Email',
				'',
				25,
				90,
				0,
				'',
				'xcGYVljYAdunbhSctVITVg==',
				'',
				''
				)
	--GO

	SET IDENTITY_INSERT dbo.notificationprovider OFF
END
GO
/* ---------------------------------------------------------------------- */
/* Schema changes for SQLsecure 2.5 schema version 2500                   */
/* ---------------------------------------------------------------------- */

SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON

/* ---------------------------------------------------------------------- */
/* Validate the db is correct version for upgrade                         */
/*     >= 2000 or < 2500 or null are valid values		                  */
/* ---------------------------------------------------------------------- */

declare @ver int
SELECT @ver=schemaversion FROM currentversion
if (isnull(@ver,900) < 2500)
BEGIN

	/* ---------------------------------------------------------------------- */
	/* Add Assessment Table and update Policy tables to add AssessmentId column as a primary key value      */
	/* ---------------------------------------------------------------------- */

	-- to make it restartable, check if the assessment table
	IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[assessment]') AND type = N'U')
--		OR NOT EXISTS (SELECT * FROM dbo.syscolumns WHERE id = OBJECT_ID(N'[dbo].[policy]') and name = 'assessmentid')
	BEGIN
		BEGIN TRANSACTION

		CREATE TABLE [dbo].[assessment] (
			[assessmentid] int IDENTITY(1,1) NOT NULL,
			[policyid] [int] NOT NULL,
			[assessmentstate] nchar(1) NOT NULL,	-- S is default settings, C is current, D is saved draft, P is saved published, A is saved approved
			[assessmentname] nvarchar(128) NOT NULL,
			[assessmentdescription] nvarchar(2048) NOT NULL,
			[assessmentnotes] ntext NOT NULL,
			[assessmentdate] datetime NULL,
			[usebaseline] [bit] NOT NULL,
			[isdynamic] [bit] NOT NULL,
			[dynamicselection] [nvarchar](4000) NULL
			CONSTRAINT [PK_assessment] PRIMARY KEY CLUSTERED ([assessmentid])
		)

		ALTER TABLE [dbo].[assessment]  WITH CHECK ADD CONSTRAINT [FK_assessment_policy]
			FOREIGN KEY ([policyid])
			REFERENCES [dbo].[policy] ([policyid])

		CREATE NONCLUSTERED INDEX [IX_assessmentid] ON [dbo].[assessment] (
			[policyid], [assessmentid]
		)

		CREATE NONCLUSTERED INDEX [IX_assessmentname] ON [dbo].[assessment] (
			[policyid], [assessmentname]
		)

		ALTER TABLE [policyinterview]
			ADD [assessmentid] int null

		ALTER TABLE [dbo].[policyinterview]  WITH CHECK ADD CONSTRAINT [FK_policyinterview_assessment]
			FOREIGN KEY([assessmentid])
			REFERENCES [dbo].[assessment] ([assessmentid])

		ALTER TABLE [policymember]
			ADD [assessmentid] int null

		ALTER TABLE [dbo].[policymember]  WITH CHECK ADD CONSTRAINT [FK_policymember_assessment]
			FOREIGN KEY([assessmentid])
			REFERENCES [dbo].[assessment] ([assessmentid])

		ALTER TABLE [policymetric]
			ADD [assessmentid] int null

		ALTER TABLE [dbo].[policymetric]  WITH CHECK ADD CONSTRAINT [FK_policymetric_assessment]
			FOREIGN KEY([assessmentid])
			REFERENCES [dbo].[assessment] ([assessmentid])


		CREATE TABLE [dbo].[policyassessment] (
			[policyassessmentid] [int] IDENTITY(1,1) NOT NULL,
			[policyid] [int] NOT NULL,
			[assessmentid] [int] NOT NULL,
			[metricid] int NOT NULL,
			[snapshotid] int NULL,					-- can be null for missing snapshot or policy level checks
			[registeredserverid] int NULL,			-- can be null for policy level checks
			[connectionname] nvarchar(400) NOT NULL,
			[collectiontime] datetime  NULL,
			[metricname] nvarchar(256) NOT NULL,
			[metrictype] nvarchar(32) NOT NULL,
			[metricseveritycode] int NOT NULL,
			[metricseverity] nvarchar(16) NOT NULL,
			[metricseverityvalues] nvarchar(4000) NOT NULL,
			[metricdescription] nvarchar(4000) NOT NULL,
			[metricreportkey] nvarchar(32) NOT NULL,
			[metricreporttext] nvarchar(4000) NOT NULL,
			[severitycode] int NOT NULL,
			[severity] nvarchar(16) NOT NULL,
			[currentvalue] nvarchar(1500) NOT NULL,
			[thresholdvalue] nvarchar(1500) NOT NULL,
			CONSTRAINT [PK_policyassessment] PRIMARY KEY CLUSTERED ([policyassessmentid])
		)

		ALTER TABLE [dbo].[policyassessment]  WITH CHECK ADD CONSTRAINT [FK_policyassessment_policy]
			FOREIGN KEY ([policyid])
			REFERENCES [dbo].[policy] ([policyid])

		ALTER TABLE [dbo].[policyassessment]  WITH CHECK ADD CONSTRAINT [FK_policyassessment_assessment]
			FOREIGN KEY ([assessmentid])
			REFERENCES [dbo].[assessment] ([assessmentid])

		ALTER TABLE [dbo].[policyassessment] WITH CHECK ADD CONSTRAINT [FK_policyassessment_metric]
			FOREIGN KEY([metricid])
			REFERENCES [dbo].[metric] ([metricid])

-- NOTE: these foreign key constraints will not work because the snapshotid is used as a foreign key to the policy assessment
--			by the detail and notes tables and must be preserved to continue to link them together after snapshot deletion on approved assessments
--		ALTER TABLE [dbo].[policyassessment] WITH CHECK ADD CONSTRAINT [FK_policyassessment_snapshot]
--			FOREIGN KEY([snapshotid])
--			REFERENCES [dbo].[serversnapshot] ([snapshotid])
--				ON DELETE SET NULL
--
--		ALTER TABLE [dbo].[policyassessment] WITH CHECK ADD CONSTRAINT [FK_policyassessment_server]
--			FOREIGN KEY([registeredserverid])
--			REFERENCES [dbo].[registeredserver] ([registeredserverid])
--				ON DELETE SET NULL


		CREATE NONCLUSTERED INDEX [IX_policyassessment_snapshot] ON [dbo].[policyassessment] (
			[policyid], 
			[assessmentid], 
			[metricid], 
			[snapshotid]
		)

		CREATE NONCLUSTERED INDEX IX_policyassessment_connection ON dbo.[policyassessment] (
			[policyid],
			[assessmentid],
			[connectionname],
			[metricid]
		)


		CREATE TABLE [dbo].[policyassessmentnotes] (
			[policyassessmentnotesid] [int] IDENTITY(1,1) NOT NULL,
			[policyid] [int] NOT NULL,
			[assessmentid] [int] NOT NULL,
			[metricid] int NOT NULL,
			[snapshotid] int NOT NULL,
			[isexplained] [bit] NOT NULL,
			[notes] [nvarchar](4000) NOT NULL,
			CONSTRAINT [PK_policyassessmentnotes] PRIMARY KEY CLUSTERED ([policyassessmentnotesid])
		)

		ALTER TABLE [dbo].[policyassessmentnotes]  WITH CHECK ADD CONSTRAINT [FK_policyassessmentnotes_policy]
			FOREIGN KEY ([policyid])
			REFERENCES [dbo].[policy] ([policyid])

		ALTER TABLE [dbo].[policyassessmentnotes]  WITH CHECK ADD CONSTRAINT [FK_policyassessmentnotes_assessment]
			FOREIGN KEY ([assessmentid])
			REFERENCES [dbo].[assessment] ([assessmentid])

		ALTER TABLE [dbo].[policyassessmentnotes] WITH CHECK ADD CONSTRAINT [FK_policyassessmentnotes_metric]
			FOREIGN KEY([metricid])
			REFERENCES [dbo].[metric] ([metricid])

-- NOTE: this foreign key constraint will not work because the snapshot can be deleted for approved assessments, but the id is still needed
--		ALTER TABLE [dbo].[policyassessmentnotes] WITH CHECK ADD CONSTRAINT [FK_policyassessmentnotes_snapshot]
--			FOREIGN KEY([snapshotid])
--			REFERENCES [dbo].[serversnapshot] ([snapshotid])
--				ON DELETE SET NULL



		CREATE TABLE [dbo].[policyassessmentdetail] (
			[policyassessmentdetailid] [int] IDENTITY(1,1) NOT NULL,
			[policyid] [int] NOT NULL,
			[assessmentid] [int] NOT NULL,
			[metricid] int NOT NULL,
			[snapshotid] int NULL,						-- null to allow snapshot delete and for policy level checks
			[detailfinding] [nvarchar](2048) NOT NULL,
			[databaseid] [int] NULL,
			[objecttype] [nvarchar](5) NOT NULL,
			[objectid] [int] NULL,
			[objectname] [nvarchar](400) NOT NULL
			CONSTRAINT [PK_policyassessmentdetail] PRIMARY KEY CLUSTERED ([policyassessmentdetailid])
		)


		ALTER TABLE [dbo].[policyassessmentdetail]  WITH CHECK ADD CONSTRAINT [FK_policyassessmentdetail_policy]
			FOREIGN KEY ([policyid])
			REFERENCES [dbo].[policy] ([policyid])

		ALTER TABLE [dbo].[policyassessmentdetail]  WITH CHECK ADD CONSTRAINT [FK_policyassessmentdetail_assessment]
			FOREIGN KEY ([assessmentid])
			REFERENCES [dbo].[assessment] ([assessmentid])

		ALTER TABLE [dbo].[policyassessmentdetail] WITH CHECK ADD CONSTRAINT [FK_policyassessmentdetail_metric]
			FOREIGN KEY([metricid])
			REFERENCES [dbo].[metric] ([metricid])


-- NOTE: this foreign key constraint will not work because the snapshot can be deleted for approved assessments, but the id is still needed
--		ALTER TABLE [dbo].[policyassessmentdetail] WITH CHECK ADD CONSTRAINT [FK_policyassessmentdetail_snapshot]
--			FOREIGN KEY([snapshotid])
--			REFERENCES [dbo].[serversnapshot] ([snapshotid])
--				ON DELETE SET NULL


		CREATE TABLE [dbo].[policychangelog] (
			[policychangelogid] [int] IDENTITY(1,1) NOT NULL,
			[policyid] [int] NOT NULL,
			[assessmentid] [int] NOT NULL,
			[assessmentstate] [nchar](1) NOT NULL,
			[changedate] [datetime] NOT NULL,
			[changedby] [nvarchar](128) NOT NULL,
			[changedescription] [nvarchar](4000) NOT NULL
			CONSTRAINT [PK_policychangelog] PRIMARY KEY CLUSTERED ([policychangelogid])
		)

		ALTER TABLE [dbo].[policychangelog]  WITH CHECK ADD CONSTRAINT [FK_policychangelog_policy]
			FOREIGN KEY ([policyid])
			REFERENCES [dbo].[policy] ([policyid])

		ALTER TABLE [dbo].[policychangelog]  WITH CHECK ADD CONSTRAINT [FK_policychangelog_assessment]
			FOREIGN KEY ([assessmentid])
			REFERENCES [dbo].[assessment] ([assessmentid])


		COMMIT
	END
END

if (@@ERROR > 0)
BEGIN
	ROLLBACK
END

GO


/* ---------------------------------------------------------------------- */
/*	Updates to new fields must be done after finalizing the changes		  */
/* ---------------------------------------------------------------------- */

-- create the default assessment rows and link everything up to them
if EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[assessment]') AND type = N'U')
	AND NOT EXISTS (SELECT * FROM dbo.[assessment])
begin
	BEGIN TRANSACTION

	-- create the assessment rows for each policy
	declare @sql nvarchar(4000)
	select @sql = '
					SET IDENTITY_INSERT [assessment] ON;
					INSERT INTO dbo.[assessment] ([assessmentid], [policyid], [assessmentstate], [assessmentname], [assessmentdescription], [usebaseline], [assessmentnotes], [isdynamic], [dynamicselection])
						SELECT [policyid] as assessmentid, [policyid], N''S'', N'''', N'''', 0, [policydescription], [isdynamic], [dynamicselection] from dbo.[policy] order by [policyid];
					SET IDENTITY_INSERT [assessment] OFF;'
	-- these columns will be deleted later, but cause an error if deleted within the transaction, so just empty them to avoid confusion
	select @sql = @sql + ' UPDATE [policy] SET [isdynamic]=0, [dynamicselection]=null;'
	exec (@sql)		-- use dynamic sql so there won't be missing column errors on future upgrades since isdynamic has been removed
 
	-- set all existing policies to have the new assessmentid for the policy settings
	UPDATE dbo.[policyinterview] SET [assessmentid] = b.[assessmentid] FROM [policyinterview] a INNER JOIN [assessment] b ON a.[policyid] = b.[policyid] AND a.[assessmentid] IS NULL
	UPDATE dbo.[policymember] SET [assessmentid] = b.[assessmentid] FROM [policymember] a INNER JOIN [assessment] b ON a.[policyid] = b.[policyid] AND a.[assessmentid] IS NULL
	UPDATE dbo.[policymetric] SET [assessmentid] = b.[assessmentid] FROM [policymetric] a INNER JOIN [assessment] b ON a.[policyid] = b.[policyid] AND a.[assessmentid] IS NULL

	-- fix the assessmentid to not be null now because the column will be included in the primary keys
	ALTER TABLE [policyinterview]
		ALTER COLUMN [assessmentid] int not null

	ALTER TABLE [policymember]
		ALTER COLUMN [assessmentid] int not null

	ALTER TABLE [policymetric]
		ALTER COLUMN [assessmentid] int not null

	-- now fix the old primary keys that include policyid to include the assessmentid
	IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[PK_policymember]') AND type = N'K')
		ALTER TABLE [policymember]
			DROP CONSTRAINT [PK_policymember] 

	ALTER TABLE [policymember]
		ADD CONSTRAINT [PK_policymember] PRIMARY KEY CLUSTERED 
		(
			[policyid] ASC,
			[assessmentid] ASC,
			[registeredserverid] ASC
		)

	IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[PK_policymetric]') AND type = N'K')
		ALTER TABLE [policymetric]
			DROP CONSTRAINT [PK_policymetric] 

	ALTER TABLE [policymetric]
		ADD CONSTRAINT [PK_policymetric] PRIMARY KEY CLUSTERED 
		(
			[policyid] ASC,
			[assessmentid] ASC,
			[metricid] ASC
		)

	ALTER TABLE [dbo].[policyassessment] WITH CHECK ADD CONSTRAINT [FK_policyassessment_policymetric]
		FOREIGN KEY([policyid], [assessmentid], [metricid])
		REFERENCES [dbo].[policymetric] ([policyid], [assessmentid], [metricid])

	ALTER TABLE [dbo].[policyassessmentnotes] WITH CHECK ADD CONSTRAINT [FK_policyassessmentnotes_policyassessment]
		FOREIGN KEY([policyid], [assessmentid], [metricid])
		REFERENCES [dbo].[policymetric] ([policyid], [assessmentid], [metricid])

	ALTER TABLE [dbo].[policyassessmentdetail] WITH CHECK ADD CONSTRAINT [FK_policyassessmentdetail_policyassessment]
		FOREIGN KEY([policyid], [assessmentid], [metricid])
		REFERENCES [dbo].[policymetric] ([policyid], [assessmentid], [metricid])


	COMMIT
end

if (@@ERROR > 0)
BEGIN
	ROLLBACK
END

GO

declare @ver int
SELECT @ver=schemaversion FROM currentversion
if (isnull(@ver,900) < 2500)
BEGIN
	if exists(select * from dbo.syscolumns where name = 'isdynamic' and id = object_id('policy'))
	begin
		-- remove the server selection columns that were moved to the assessment
		-- note this causes an error when inside the transaction on sql server 2000
		declare @sql nvarchar (1000)
		select @sql = 
			'ALTER TABLE [dbo].[policy]
				DROP COLUMN [isdynamic];
			ALTER TABLE [dbo].[policy]
				DROP COLUMN [dynamicselection];'
		exec (@sql)		-- use dynamic sql so there won't be missing column errors on future upgrades
	end
END

-- provide view access to new tables

GRANT SELECT ON dbo.[assessment] TO SQLSecureView
GRANT SELECT ON dbo.[policyassessment] TO SQLSecureView
GRANT SELECT ON dbo.[policyassessmentnotes] TO SQLSecureView
GRANT SELECT ON dbo.[policyassessmentdetail] TO SQLSecureView
GRANT SELECT ON dbo.[policychangelog] TO SQLSecureView
GO


declare @ver int
SELECT @ver=schemaversion FROM currentversion
if (isnull(@ver,2500) >= 2000)
BEGIN
	-- update the server version security check default settings with the latest versions and service packs
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = 2 and severityvalues = '''8.00.2187'',''9.00.3054''')
	begin
		update policymetric 
			set severityvalues= '''8.00.2039'',''9.00.4035'',''10.0.1600'''
			where policyid = 0 and assessmentid=0 and metricid = 2
	end

	-- add new security checks
	if not exists (select * from metric where metricid = 62)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (62, 'SQL Logins Not Using Password Expiration', 'Login', 0, 0, '', 'When enabled, this check will identify a risk if any SQL Login is not protected by password expiration.',
										'Determine whether password expiration is enabled for all SQL Logins')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, 62, 1, 2, '', '',
										'Is password expiration enabled for all SQL Logins?')
	end

	if not exists (select * from metric where metricid = 63)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (63, 'Server is Domain Controller', 'Configuration', 0, 0, '', 'When enabled, this check will identify a risk if the Server is a primary or backup domain controller.',
										'Determine whether the Server is a domain controller')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, 63, 1, 1, '', '',
										'Is the server a Domain Controller?')
	end

	-- update the All Servers policy with the new security checks so they are enabled correctly if this is a new install
	if not exists (select * from policymetric where policyid = 1 and assessmentid=1 and metricid in (62, 63))
		insert into policymetric (policyid, assessmentid, metricid, isenabled, reportkey, reporttext, severity, severityvalues)
			select 1, 1, m.metricid, m.isenabled, m.reportkey, m.reporttext, m.severity, m.severityvalues
				from policymetric m
				where m.policyid = 0
					and m.assessmentid = 0
					and m.metricid in (62, 63)

	-- now add the new security checks to all existing policies, but disable it by default so it won't interfere with current settings
	insert into policymetric (policyid, assessmentid, metricid, isenabled, reportkey, reporttext, severity, severityvalues)
		select a.policyid, a.assessmentid, m.metricid, 0, m.reportkey, m.reporttext, m.severity, m.severityvalues
			from policymetric m, assessment a
			where m.policyid = 0
				and m.assessmentid = 0
				and m.metricid in (62, 63)
				and a.policyid > 0
				-- this check makes it restartable
				and a.assessmentid not in (select assessmentid from policymetric where metricid in (62, 63))
END

-- these have been deferred out of this release because of issues 
--
---- add comparison security checks that are not server specific
--
--insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
--                values (64, 'Security Check Settings are Different', 'Data Integrity', 0, 1, '''Current Assessment'' : ''c'',''Last Draft Assessment'' : ''d'', ''Last Published Assessment'' : ''p'', ''Last Approved Assessment'' : ''a''', 'When enabled, this check will identify a risk if the security check settings are different from the settings of the selected assessment.',
--                                'Determine whether the security check settings are different from the selected assessment.')
--insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
--                values (0, 0, 64, 0, 3, '', '',
--                                'Are the security check settings different?')
--
--insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
--                values (65, 'Assessment Findings are Different', 'Data Integrity', 0, 1, '''Current Assessment'' : ''c'',''Last Draft Assessment'' : ''d'', ''Last Published Assessment'' : ''p'', ''Last Approved Assessment'' : ''a''', 'When enabled, this check will identify a risk if the assessment findings are different from the findings of the selected assessment.',
--                                'Determine whether the results of this assessment are different from the selected assessment.')
--insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
--                values (0, 0, 65, 0, 3, '', '',
--                                'Are the assessment findings different?')
--
--insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
--                values (66, 'Policy Servers are Different', 'Data Integrity', 0, 1, '''Current Assessment'' : ''c'',''Last Draft Assessment'' : ''d'', ''Last Published Assessment'' : ''p'', ''Last Approved Assessment'' : ''a''', 'When enabled, this check will identify a risk if the servers used for this assessment are diffferent from the servers used for the selected assessment.',
--                                'Determine whether the list of servers is different from the selected assessment.')
--insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
--                values (0, 0, 66, 0, 3, '', '',
--                                'Are the policy servers different?')



GO
/* ---------------------------------------------------------------------- */
/* Schema changes for SQLsecure 2.6 schema version 2600                   */
/* ---------------------------------------------------------------------- */

SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON

/* ---------------------------------------------------------------------- */
/* Validate the db is correct version for upgrade                         */
/*     >= 2000 or < 2600 or null are valid values		                  */
/* ---------------------------------------------------------------------- */

declare @ver int
SELECT @ver=schemaversion FROM currentversion
if (isnull(@ver,900) < 2600)
BEGIN
	/* ---------------------------------------------------------------------- */
	/* Add Snapshot	Collection fields                                         */
	/* ---------------------------------------------------------------------- */

	BEGIN TRANSACTION

	ALTER TABLE dbo.serversnapshot ADD
		[systemdrive] nvarchar(2) NULL,
		[adhocdistributedqueriesenabled] nchar(1) NULL

	ALTER TABLE dbo.sqldatabase ADD
		[trustworthy] nchar(1) NULL

	COMMIT


	-- Update Reports Table
	IF NOT EXISTS (SELECT c.name,c.id from syscolumns as c,sysobjects as o where c.name='port' and o.name='reports' and c.id=o.id)
	BEGIN
	  DROP TABLE [reports];
      CREATE TABLE [reports] (
      	[reportserver] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
      	[servervirtualdirectory] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
      	[managervirtualdirectory] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
      	[port] [int] NULL ,
      	[usessl] [tinyint] NULL ,
      	[username] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
      	[repository] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
      	[targetdirectory] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
      ) ON [PRIMARY];
	  GRANT SELECT ON [reports] TO [SQLSecureView];
	END



END


GO


/* ---------------------------------------------------------------------- */
/*	Updates to new fields must be done after finalizing the changes		  */
/* ---------------------------------------------------------------------- */
declare @ver int
SELECT @ver=schemaversion FROM currentversion
if (isnull(@ver,900) < 2600)
BEGIN
	BEGIN TRANSACTION

	-- set all new fields to a value for existing snapshots to prevent null errors
	if exists (select * from dbo.syscolumns where name = 'systemdrive' and id = object_id('serversnapshot'))
	begin
		UPDATE dbo.serversnapshot SET [systemdrive] = N''
			WHERE [systemdrive] is null
	end

	if exists (select * from dbo.syscolumns where name = 'adhocdistributedqueriesenabled' and id = object_id('serversnapshot'))
	begin
		UPDATE dbo.serversnapshot SET [adhocdistributedqueriesenabled] = N'U'
			WHERE [adhocdistributedqueriesenabled] is null
	end

	if exists (select * from dbo.syscolumns where name = 'trustworthy' and id = object_id('sqldatabase'))
	begin
		UPDATE dbo.sqldatabase SET [trustworthy] = N'U'
			WHERE [trustworthy] is null
	end

	COMMIT
END

GO	


declare @ver int
SELECT @ver=schemaversion FROM currentversion
if (isnull(@ver, 900) <= 2500)	-- Check to prevent this from running in future upgrades, but run for all prior versions at any version level because the version gets updated later
BEGIN
	declare @metricid int, @strval nvarchar(512)

	-- update the All Servers policy description
	if (exists (select * from policy where policyid=1) and @ver is null)
	begin
		update policy set policydescription ='Global security checks that should be performed on all SQL Servers; based on the Idera Level 2 Balanced Protection policy.'
			where policyid=1
	end

	-- correct some metric names and descriptions

	-- fix the description on the tcp/ip port security check
	select @metricid = 27, @strval= 'When enabled, this check will identify a risk if unacceptable TCP ports are used by SQL Server. Specify unacceptable TCP ports (specify ''dynamic'' if dynamic port allocation should not be used).'
	if exists (select * from metric where metricid = @metricid )
	begin
		update metric 
			set valuedescription = @strval
			where metricid = @metricid
	end

	-- correct the case in names of previous service checks
	select @metricid = 13, @strval = 'SQL Mail Or Database Mail Enabled'
	if exists (select * from metric where metricid = @metricid )
	begin
		update metric 
			set metricname = @strval
			where metricid = @metricid
		update policyassessment
			set metricname = @strval
			where metricid = @metricid
				and assessmentid not in (select assessmentid from assessment where assessmentstate='A')
	end
	select @metricid = 16, @strval = 'sa Account Not Disabled Or Renamed'
	if exists (select * from metric where metricid = @metricid )
	begin
		update metric 
			set metricname = @strval
			where metricid = @metricid
		update policyassessment
			set metricname = @strval
			where metricid = @metricid
				and assessmentid not in (select assessmentid from assessment where assessmentstate='A')
	end

	select @metricid = 60, @strval = 'Fixed Roles Assigned To public Or guest'
	if exists (select * from metric where metricid = @metricid )
	begin
		update metric
			set metricname = @strval
			where metricid = @metricid
		update policyassessment
			set metricname = @strval
			where metricid = @metricid
				and assessmentid not in (select assessmentid from assessment where assessmentstate='A')
	end

	select @metricid = 61, @strval = 'BUILTIN/Administrators Is sysadmin'
	if exists (select * from metric where metricid = @metricid )
	begin
		update metric 
			set metricname = @strval
			where metricid = @metricid
		update policyassessment
			set metricname = @strval
			where metricid = @metricid
				and assessmentid not in (select assessmentid from assessment where assessmentstate='A')
	end

	select @metricid = 63, @strval = 'Server Is Domain Controller'
	if exists (select * from metric where metricid = @metricid )
	begin
		update metric 
			set metricname = @strval
			where metricid = @metricid
		update policyassessment
			set metricname = @strval
			where metricid = @metricid
				and assessmentid not in (select assessmentid from assessment where assessmentstate='A')
	end

	-- update the password policy security check to use consistent text with the new one
	select @metricid = 57, @strval = 'Determine whether password policy is enforced on all SQL Logins'
	if exists (select * from metric where metricid = @metricid )
	begin
		update metric 
			set metricdescription = @strval
			where metricid = @metricid
		update policyassessment 
			set metricdescription = @strval
			where metricid = @metricid
				and assessmentid not in (select assessmentid from assessment where assessmentstate='A')
	end

	-- update all security checks that specify 2005 only info to be 2005 or later
	select @metricid = 16, @strval = 'SQL Server 2005 or later'
	if not exists (select 1 from metric where metricid = @metricid and valuedescription like '%' + @strval + '%' )
	begin
		update metric 
			set valuedescription = replace(valuedescription, 'SQL Server 2005', @strval)
			where metricid = @metricid
	end

	if not exists (select 1 from metric where metricid = @metricid and metricdescription like '%' + @strval + '%' )
	begin
		update metric 
			set metricdescription = replace(metricdescription, 'SQL Server 2005', @strval)
			where metricid = @metricid
	end
	select @metricid = 18
	if not exists (select 1 from metric where metricid = @metricid and metricdescription like '%' + @strval + '%' )
	begin
		update metric 
			set metricdescription = replace(metricdescription, 'SQL Server 2005', @strval)
			where metricid = @metricid
	end
	select @metricid = 26, @strval = '(2005 or later)'
	if not exists (select 1 from metric where metricid = @metricid and metricdescription like '%' + @strval + '%' )
	begin
		update metric 
			set validvalues = replace(validvalues, '(2005)', @strval)
			where metricid = @metricid
	end


	-- update the sql server version security check default settings with the latest versions and service packs
	select @metricid = 2
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set severityvalues= N'''8.00.2039'',''9.00.4035'',''10.0.2531'''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end

	-- update the o/s server version security check default settings with the latest versions and service packs
	select @metricid = 39
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set severityvalues= N'''Microsoft(R) Windows(R) Server 2003, Standard Edition, Service Pack 2'',''Microsoft(R) Windows(R) Server 2003 Standard x64 Edition, Service Pack 2'',''Microsoft® Windows Server® 2008 Standard , Service Pack 2'',''Microsoft® Windows Server® 2008 Standard without Hyper-V , Service Pack 2'',''Microsoft Windows Server 2008 R2 Standard ''' + 
								N',''Microsoft(R) Windows(R) Server 2003, Enterprise Edition, Service Pack 2'',''Microsoft(R) Windows(R) Server 2003 Enterprise x64 Edition, Service Pack 2'',''Microsoft® Windows Server® 2008 Enterprise , Service Pack 2'',''Microsoft® Windows Server® 2008 Enterprise without Hyper-V , Service Pack 2'',''Microsoft Windows Server 2008 R2 Enterprise '''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end

	-- set the new default settings for existing security checks
	select @metricid = 8
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set isenabled=0
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set isenabled=0
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 9
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set severity=2
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set severity=2
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 15
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set severityvalues= N'''Northwind'',''pubs'',''AdventureWorks'',''AdventureWorksAS'',''AdventureWorksDW'''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 16
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set severity=2
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set severity=2
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 20
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set isenabled=1
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set isenabled=1
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 21
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set isenabled=1
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set isenabled=1
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 24
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set isenabled=1,severity=2,severityvalues='''xp_cmdshell'',''xp_available_media'',''xp_dirtree'',''xp_dsninfo'',''xp_enumdsn'',''xp_enumerrorlogs'',''xp_enumgroups'',''xp_eventlog'',''xp_fixeddrives'',''xp_getfiledetails'',''xp_getnetname'',''xp_logevent'',''xp_loginconfig'',''xp_msver'',''xp_readerrorlog'',''xp_servicecontrol'',''xp_sprintf'',''xp_sscanf'',''xp_subdirs'',''xp_deletemail'',''xp_findnextmsg'',''xp_get_mapi_default_profile'',''xp_get_mapi_profiles'',''xp_readmail'',''xp_sendmail'',''xp_startmail'',''xp_stopmail'',''xp_cleanupwebtask'',''xp_convertwebtask'',''xp_dropwebtask'',''xp_enumcodepages'',''xp_makewebtask'',''xp_readwebtask'',''xp_runwebtask'',''sp_OACreate'''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set isenabled=1,severity=2,severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 26
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set severityvalues= N'''Shared Memory'',''TCP/IP'''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 27
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set severityvalues= N'''1433'',''1434'',''dynamic'''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 28
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set isenabled=0
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set isenabled=0
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 32
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set severityvalues= N'''INSTALL_ACCT_OR_OWNER_ACCT'''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 35
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set isenabled=1,severityvalues='''INSTALL_ACCT_OR_OWNER_ACCT'''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set isenabled=1,severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 36
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set isenabled=1,severityvalues='''INSTALL_ACCT_OR_OWNER_ACCT'''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set isenabled=1,severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 38
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set isenabled=1,severityvalues='''INSTALL_ACCT_OR_OWNER_ACCT'''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set isenabled=1,severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 39
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set isenabled=0
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set isenabled=0
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 40
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set isenabled=1,severityvalues='''SERVICE_SPECIFIC_ACCOUNT'''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set isenabled=1,severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 43
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set isenabled=1,severityvalues='''SERVICE_SPECIFIC_ACCOUNT'''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set isenabled=1,severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 45
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set isenabled=1,severityvalues='''SERVICE_SPECIFIC_ACCOUNT'''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set isenabled=1,severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 47
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set isenabled=1,severityvalues='''SERVICE_SPECIFIC_ACCOUNT'''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set isenabled=1,severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 48
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set isenabled=0
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set isenabled=0
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 49
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set isenabled=1,severityvalues='''LOW_PRIVILEDGE_SVC_ACCT'''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set isenabled=1,severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 51
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set isenabled=1,severityvalues='''SERVICE_SPECIFIC_ACCOUNT'''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set isenabled=1,severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 53
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set isenabled=1,severityvalues='''SERVICE_SPECIFIC_ACCOUNT'''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set isenabled=1,severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 54
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set severity=3
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set severity=3
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 55
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set isenabled=0
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set isenabled=0
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end


	-- update the password policy security check details to reflect the new text if appropriate
	select @metricid = 57
	if exists (select * from policymetric where metricid = @metricid )
	begin
		update policyassessment 
			set thresholdvalue= N'Server is vulnerable if password policy is not enforced on all SQL Logins.'
			where metricid = @metricid
				and thresholdvalue = N'Server is vulnerable if SQL Logins are found that do not implement the password policy.'
				and assessmentid not in (select assessmentid from assessment where assessmentstate='A')
		update policymetric 
			set reporttext= N'Is password policy enforced on all SQL Logins?'
			where metricid = @metricid
				and reporttext = N'Is the password policy enabled for all SQL Logins?'
				and assessmentid not in (select assessmentid from assessment where assessmentstate='A')
	end



	-- ADD NEW SECURITY CHECKS ***************************************************************

	-- NOTE: metricids 64-66 were reserved for comparison security checks that were not implemented in 2500
	--		I am going to go ahead and leave them reserved and skip over them just in case

	-- NOTE: metricids 67-80 work with 2.5 data and require no schema or data collection changes, but do need the associated update to the getpolicyassessment stored procecure
	--		These metrics were provided as a 2.6 technology preview to Wells Fargo
	declare @startmetricid int
	select @metricid = 67, @startmetricid = 67
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'Active Directory Helper Login Account Not Acceptable', 'Login', 1, 1, '', 'When enabled, this check will identify a risk if the Active Directory Helper account is not acceptable. Specify the acceptable login accounts.',
										'Determine whether the Active Directory Helper account is acceptable')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 1, '''SERVICE_SPECIFIC_ACCOUNT''', '',
										'Is an unacceptable Active Directory Helper account being used?')
	end

	select @metricid = 68
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'Reporting Services Login Account Not Acceptable', 'Login', 1, 1, '', 'When enabled, this check will identify a risk if the Reporting Services account is not acceptable. Specify the acceptable login accounts.',
										'Determine whether the Reporting Services account is acceptable')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 1, '''SERVICE_SPECIFIC_ACCOUNT''', '',
										'Is an unacceptable Reporting Services account being used?')
	end

	select @metricid = 69
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'VSS Writer Login Account Not Acceptable', 'Login', 1, 1, '', 'When enabled, this check will identify a risk if the VSS Writer account is not acceptable. Specify the acceptable login accounts.',
										'Determine whether the VSS Writer account is acceptable')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 1, '''SERVICE_SPECIFIC_ACCOUNT''', '',
										'Is an unacceptable VSS Writer account being used?')
	end

	select @metricid = 70
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'VSS Writer Running', 'Configuration', 0, 0, '', 'When enabled, this check will identify a risk if the VSS Writer is running on the SQL Server.',
										'Determine whether VSS Writer is running on the SQL Server')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 0, 1, '', '',
										'Is VSS Writer running?')
	end

	select @metricid = 71
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'Unauthorized Accounts Are Sysadmins', 'Configuration', 1, 1, '', 'When enabled, this check will identify a risk if any unauthorized accounts are members of the sysadmin server role. Specify the unauthorized accounts. Can use ''%'' as wildcard.',
										'Determine whether unauthorized accounts have sysadmin privileges on the SQL Server')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 0, 2, 'UNAUTHORIZED_ADMIN_ACCT', '',
										'Do unauthorized accounts have sysadmin privileges?')
	end

	select @metricid = 72
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'sa Account Not Disabled', 'Configuration', 0, 0, '', 'When enabled, this check will identify a risk if the SQL Server sa account is enabled on SQL Server 2005 or later.',
										'Determine whether the SQL Server sa account has been disabled on SQL Server 2005 or later')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 1, '', '',
										'Is the SQL Server sa account enabled?')
	end

	select @metricid = 73
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'ALTER TRACE Permission Granted To Unauthorized Users', 'Permissions', 1, 1, '', 'When enabled, this check will identify a risk if any unauthorized accounts have been granted the ALTER TRACE permission on SQL Server 2005 or later. Specify which accounts are authorized to have this permission. Can use ''%'' as wildcard.',
										'Determine whether unauthorized users have been granted the ALTER TRACE permission on SQL Server 2005 or later')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 2, '''none''', '',
										'Do unauthorized users have the ALTER TRACE permission?')
	end

	select @metricid = 74
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'CONTROL SERVER Permission Granted To Unauthorized Users', 'Permissions', 1, 1, '', 'When enabled, this check will identify a risk if any unauthorized accounts have been granted the CONTROL SERVER permission on SQL Server 2005 or later. Specify which accounts are authorized to have this permission. Can use ''%'' as wildcard.',
										'Determine whether unauthorized users have been granted the CONTROL SERVER permission on SQL Server 2005 or later')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 0, 2, '''none''', '',
										'Do unauthorized users have the CONTROL SERVER permission?')
	end

	select @metricid = 75
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'xp_cmdshell Enabled', 'Configuration', 0, 0, '', 'When enabled, this check will identify a risk if xp_cmdshell is enabled.',
										'Determine whether the xp_cmdshell extended stored procedure is enabled on SQL Server 2005 or later')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 2, '', '',
										'Is xp_cmdshell enabled on the SQL Server?')
	end

	select @metricid = 76
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'Required Administrative Accounts Do Not Exist', 'Access', 1, 1, '', 'When enabled, this check will identify a risk if any required administrative accounts are missing from the SQL Server. Specify the required accounts.',
										'Determine whether the required administrative accounts exist on the SQL Server')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 1, '''sa''', '',
										'Do required administrative accounts exist?')
	end

	select @metricid = 77
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'sa Account Not Using Password Policy', 'Login', 0, 0, '', 'When enabled, this check will identify a risk if the sa account is not protected by Windows password policy.',
										'Determine whether password policy is enforced on the sa account')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 2, '', '',
										'Is password policy enforced on the sa account?')
	end

	select @metricid = 78
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'Database Files Missing Required Administrative Permissions', 'Permissions', 1, 1, '', 'When enabled, this security check will identify a risk if the required accounts do not have administrative permissions on all data files. Specify the required accounts. Can use ''%'' as wildcard.',
										'Determine whether the required administrative accounts have access to all database files')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 0, 1, '', '',    -- disabled by default, user must enter list of accounts
										'Do the required administrative permissions exist on database files?')
	end

	select @metricid = 79
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'Executable Files Missing Required Administrative Permissions', 'Permissions', 1, 1, '', 'When enabled, this security check will identify a risk if the required accounts do not have administrative permissions on all executable files. Specify the required accounts.  Can use ''%'' as wildcard.',
										'Determine whether the required administrative accounts have access to all executable files (any .exe or .dll file)')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 0, 1, '', '',    -- disabled by default, user must enter list of accounts
										'Do the required administrative permissions exist on executable files?')
	end

	select @metricid = 80
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'Registry Keys Missing Required Administrative Permissions', 'Permissions', 1, 1, '', 'When enabled, this security check will identify a risk if the required accounts do not have administrative permissions on all SQL Server registry keys. Specify the required accounts.  Can use ''%'' as wildcard.',
										'Determine whether the required administrative accounts have access to all SQL Server registry keys')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 0, 1, '', '',    -- disabled by default, user must enter list of accounts
										'Do the required administrative permissions exist on registry keys?')
	end

	-- NOTE: metricids 81-88 are the remaining 2.6 checks and may require schema and data collection changes
	select @metricid = 81
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'Data Files On System Drive', 'Configuration', 1, 1, '', 'When enabled, this check will identify a risk if unapproved data files are located on the system drive.  Specify the approved data files. Can use ''%'' as wildcard.',
										'Determine whether data files exist on the system drive')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 1, '''%\master.mdf'',''%\mastlog.ldf'',''%\msdbdata.mdf'',''%\msdblog.ldf'',''%\model.mdf'',''%\modellog.ldf'',''%\distmdl.%'',''%\mssqlsystemresource.%'',''%\tempdb.mdf'',''%\templog.ldf''', '',    -- disabled by default, user may enter a list of files
										'Are unapproved data files located on the system drive?')
	end

	select @metricid = 82
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'SQL Server Installation Directories On System Drive', 'Configuration', 1, 1, '', 'When enabled, this check will identify a risk if unapproved SQL Server installation directories exist on the system drive. Specify directories approved for the system drive. Can use "%" as wildcard.',
										'Determine whether SQL Server installation directories are on the system drive')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 1, '''C:\Program Files\Microsoft SQL Server\90\Shared'',''C:\Program Files\Microsoft SQL Server\100\Shared'',''C:\Program Files\Common Files\System\MSSearch\Bin'',''C:\Program Files (x86)\Microsoft SQL Server\90\Shared'',''C:\Program Files\Microsoft SQL Server\80\Tools\Binn''', '',    -- disabled by default because it is common to install it on the system drive
										'Are unapproved SQL Server installation directories on the system drive?')
	end

	select @metricid = 83
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'Ad Hoc Distributed Queries Enabled', 'Surface Area', 0, 0, '', 'When enabled, this check will identify a risk if Ad Hoc Distributed Queries are enabled on SQL Server 2005 or later.',
										'Determine whether Ad Hoc Distributed Queries are enabled on SQL Server 2005 or later')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 1, '', '',
										'Are Ad Hoc Distributed Queries enabled?')
	end

	select @metricid = 84
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'Unauthorized SQL Logins Exist', 'Login', 1, 1, '', 'When enabled, this check will identify a risk if any unauthorized SQL Logins exist on the SQL Server. Specify the authorized logins.',
										'Determine whether unauthorized SQL Logins have been created on the SQL Server')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 1, '''sa''', '',
										'Do unauthorized SQL Logins exist on the SQL Server?')
	end

	select @metricid = 85
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'Public Server Role Has Permissions', 'Permissions', 0, 0, '', 'When enabled, this check will identify a risk if  the public server role has been granted any permissions on SQL Server 2005 or later.',
										'Determine whether the public server role has been granted permissions')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 3, '', '',
										'Are any permissions granted to the public server role?')
	end

	select @metricid = 86
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'Databases Are Trustworthy', 'Configuration', 1, 1, '', 'When enabled, this check will identify a risk if any unapproved databases are trustworthy on SQL Server 2005 or later. Specify the approved databases.',
										'Determine whether any unapproved databases are trustworthy on SQL Server 2005 or later')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 0, 2, '''msdb''', '',
										'Is the trustworthy bit on for any unapproved databases?')
	end

	select @metricid = 87
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'Sysadmins Own Databases', 'Permissions', 1, 1, '', 'When enabled, this check will identify a risk if any unapproved databases are owned by system administrators. Specify the approved databases.',
										'Determine whether any databases are owned by a system administrator')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 2, '''master'',''msdb'',''model'',''tempdb''', '',
										'Are any unapproved databases owned by a system administrator?')
	end

	select @metricid = 88
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'Sysadmins Own Trustworthy Databases', 'Access', 1, 1, '', 'When enabled, this check will identify a risk if any unapproved databases have the trustworthy bit set on and the owner has system administrator privileges on SQL Server 2005 or later. Specify the approved databases.',
										'Determine whether any trustworthy databases are owned by system administrators on SQL Server 2005 or later')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 3, '''msdb''', '',
										'Are any unapproved trustworthy databases owned by a system administrator?')
	end

	-- note: the following uses the @metricid to determine the ending value for the metrics to apply to all of the policies

	if (@ver is null	-- this is a new install, so fix the All Servers policy to use the default values for the new security checks
		and not exists (select * from policymetric where policyid = 1 and assessmentid=1 and metricid between @startmetricid and @metricid))
			insert into policymetric (policyid, assessmentid, metricid, isenabled, reportkey, reporttext, severity, severityvalues)
				select 1, 1, m.metricid, m.isenabled, m.reportkey, m.reporttext, m.severity, m.severityvalues
					from policymetric m
					where m.policyid = 0
						and m.assessmentid = 0
						and m.metricid between @startmetricid and @metricid

	-- now add the new security checks to all existing policies, but disable it by default so it won't interfere with the current assessment values
	insert into policymetric (policyid, assessmentid, metricid, isenabled, reportkey, reporttext, severity, severityvalues)
		select a.policyid, a.assessmentid, m.metricid, 0, m.reportkey, m.reporttext, m.severity, m.severityvalues
			from policymetric m, assessment a
			where m.policyid = 0
				and m.assessmentid = 0
				and m.metricid between @startmetricid and @metricid
				and a.policyid > 0
				-- this check makes it restartable
				and a.assessmentid not in (select distinct assessmentid from policymetric where metricid between @startmetricid and @metricid)
END

GO
/* ---------------------------------------------------------------------- */
/* Schema changes for SQLsecure 2.7 schema version 2700                   */
/* ---------------------------------------------------------------------- */

SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON

/* ---------------------------------------------------------------------- */
/* Validate the db is correct version for upgrade                         */
/*     >= 2000 or < 2700 or null are valid values		                  */
/* ---------------------------------------------------------------------- */

declare @ver int
SELECT @ver=schemaversion FROM currentversion
if (isnull(@ver,900) >= 2700)
BEGIN
		declare @msg nvarchar(500)
		set @msg = N'Database schema is not at a level that can be upgraded to version 2700'
		if (@ver is not null)
			exec isp_sqlsecure_addactivitylog @activitytype='Failure Audit', @source='Install', @eventcode='Upgrade', @category='Schema', @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
END
ELSE
BEGIN
	/* ---------------------------------------------------------------------- */
	/* Add Snapshot	Collection fields                                         */
	/* ---------------------------------------------------------------------- */

	BEGIN TRANSACTION

	ALTER TABLE dbo.serverprincipal ADD
		[passwordstatus] int NULL
		
	CREATE TABLE [dbo].[configuration](
		[lastupdated] [datetime] NOT NULL,
		[isweakpassworddetectionenabled] [nchar](1) NOT NULL
	) ON [PRIMARY]		
		
	CREATE TABLE [dbo].[weakwordlist](
		[passwordlistid] [int] IDENTITY(1,1) NOT NULL,
		[custompasswordlist] [nvarchar](max) NULL,
		[customlistupdated] [datetime] NULL,
		[additionalpasswordlist] [nvarchar](max) NULL,
		[additionallistupdated] [datetime] NULL,
	 CONSTRAINT [PK_weakwordlist] PRIMARY KEY CLUSTERED ([passwordlistid] ASC)
	)
	
	ALTER TABLE dbo.serversnapshot ADD
		[isweakpassworddetectionenabled] [nchar](1) NULL
	COMMIT
END


GO

/* ---------------------------------------------------------------------- */
/*	Updates to new fields must be done after finalizing the changes		  */
/* ---------------------------------------------------------------------- */
declare @ver int
SELECT @ver=schemaversion FROM currentversion
if (isnull(@ver,900) < 2700)
BEGIN
	BEGIN TRANSACTION

	--insert into the newly created configuration table
	INSERT INTO dbo.configuration (lastupdated, isweakpassworddetectionenabled) VALUES (GETUTCDATE(), N'Y') 
	
	COMMIT
END

GO	


declare @ver int
SELECT @ver=schemaversion FROM currentversion
if (isnull(@ver, 900) <= 2699)	-- Check to prevent this from running in future upgrades, but run for all prior versions at any version level because the version gets updated later
BEGIN
	-- fix coveringpermissions for VIEW TRACKING CHANGES permission
	insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SCHEMA', 'VIEW CHANGE TRACKING', 'CONTROL', 'DATABASE', 'CONTROL')
	insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('OBJECT', 'VIEW CHANGE TRACKING', 'CONTROL', 'SCHEMA', 'CONTROL')

	-- update default security checks
	declare @metricid int, @strval nvarchar(512)

	-- update the sql server version security check default settings with the latest versions and service packs
	select @metricid = 2
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set severityvalues= N'''11.0.2100'',''10.50.2500'',''10.0.5500'',''9.00.5000'',''8.00.2039'''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end

	-- update the o/s server version security check default settings with the latest versions and service packs
	select @metricid = 39
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set severityvalues= N'''Microsoft Windows Server 2008 R2 Enterprise , Service Pack 1'',''Microsoft Windows Server 2008 R2 Standard , Service Pack 1'',''Microsoft Windows Web Server 2008 R2 , Service Pack 1''' +
								N',''Microsoft® Windows Server® 2008 Enterprise , Service Pack 2'',''Microsoft® Windows Server® 2008 Enterprise without Hyper-V , Service Pack 2'',''Microsoft® Windows Server® 2008 Datacenter , Service Pack 2'',''Microsoft® Windows Server® 2008 Datacenter without Hyper-V , Service Pack 2'',''Microsoft® Windows Server® 2008 Standard , Service Pack 2'',''Microsoft® Windows Server® 2008 Standard without Hyper-V , Service Pack 2'',''Microsoft® Windows® Web Server 2008 , Service Pack 2''' +
								N',''Microsoft(R) Windows(R) Server 2003, Enterprise Edition, Service Pack 2'',''Microsoft(R) Windows(R) Server 2003 Enterprise x64 Edition, Service Pack 2'',''Microsoft(R) Windows(R) Server 2003, Standard Edition, Service Pack 2'',''Microsoft(R) Windows(R) Server 2003 Standard x64 Edition, Service Pack 2'''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end

	-- set the new default settings for existing security checks
	select @metricid = 11
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set isenabled=0
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set isenabled=0
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 58
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set isenabled=0
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set isenabled=0
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 59
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set isenabled=0
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set isenabled=0
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end	
	
	---- ADD NEW SECURITY CHECKS ***************************************************************

	---- NOTE: last metricid used in previous version was 88
	----		I have left a sample new one here

	declare @startmetricid int
	select  @startmetricid = 89

	select @metricid = 89
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'Public Role Has Permissions on User Database Objects', 'Permissions', 0, 0, '', 'When enabled, this check will identify a risk if the public database role has been granted permissions on any user objects within a user database. Specify the approved databases.',
										'Determine whether the public database role has been granted permissions on user database objects.')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 3, '', '',
										'Has the public database role been granted permissions on user database objects?')
	end

	select @metricid = 90
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'Integration Services Roles Have Dangerous Security Principals', 'Permissions', 1, 1, '', 'When enabled, this check will identify a risk if a dangerous security principal belongs to an SSIS database role. Specify which security principals you consider dangerous.',
										'Determine whether dangerous security principals belong to any SQL Server Information Services (SSIS) database roles.')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 2, '''public'',''guest''', '',
										'Have any dangerous principals been added to the SSIS database roles?')
	end

	select @metricid = 91
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'Integration Services Permissions Not Acceptable', 'Permissions', 1, 1, '', 'When enabled, this check will identify a risk if an unapproved user or role has been granted permissions on an Integration Services stored procedure. Specify the stored procedures.',
										'Determine whether unapproved users or roles have been granted permissions on an Integration Services stored procedure.')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 1, '''sp_add_dtspackage'',''sp_drop_dtspackage'',''sp_dts_addfolder'',''sp_dts_addlogentry'',''sp_dts_checkexists'',''sp_dts_deletefolder'',''sp_dts_deletepackage'',''sp_dts_getfolder'',''sp_dts_getpackage'',''sp_dts_getpackageroles'',''sp_dts_listfolders'',''sp_dts_listpackages'',''sp_dts_putpackage'',''sp_dts_renamefolder'',''sp_dts_setpackageroles'',''sp_dump_dtslog_all'',''sp_dump_dtspackagelog'',''sp_dump_dtssteplog'',''sp_dump_dtstasklog'',''sp_enum_dtspackagelog'',''sp_enum_dtspackages'',''sp_enum_dtssteplog'',''sp_enum_dtstasklog'',''sp_get_dtspackage'',''sp_get_dtsversion'',''sp_log_dtspackage_begin'',''sp_log_dtspackage_end'',''sp_log_dtsstep_begin'',''sp_log_dtsstep_end'',''sp_log_dtstask'',''sp_make_dtspackagename'',''sp_reassign_dtspackageowner'',''sp_ssis_addfolder'',''sp_ssis_addlogentry'',''sp_ssis_checkexists'',''sp_ssis_deletefolder'',''sp_ssis_deletepackage'',''sp_ssis_getfolder'',''sp_ssis_getpackage''', '',
										'Do unapproved users or roles have permissions on SSIS stored procedures?')
	end
	
	select @metricid = 92
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'Weak Passwords', 'Login', 0, 0, '', 'When enabled, this check will identify a risk if a SQL login on the target instance has a weak password. Specify which SQL logins should not be checked.',
										'Determine whether any SQL login passwords match the login name or a list of common and restricted passwords.')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 3, '', '', 'Does this SQL login have a weak password?')
	end
		

	-- note: the following uses the @metricid to determine the ending value for the metrics to apply to all of the policies

	if (@ver is null	-- this is a new install, so fix the All Servers policy to use the default values for the new security checks
		and not exists (select * from policymetric where policyid = 1 and assessmentid=1 and metricid between @startmetricid and @metricid))
			insert into policymetric (policyid, assessmentid, metricid, isenabled, reportkey, reporttext, severity, severityvalues)
				select 1, 1, m.metricid, m.isenabled, m.reportkey, m.reporttext, m.severity, m.severityvalues
					from policymetric m
					where m.policyid = 0
						and m.assessmentid = 0
						and m.metricid between @startmetricid and @metricid

	-- now add the new security checks to all existing policies, but disable it by default so it won't interfere with the current assessment values
	insert into policymetric (policyid, assessmentid, metricid, isenabled, reportkey, reporttext, severity, severityvalues)
		select a.policyid, a.assessmentid, m.metricid, 0, m.reportkey, m.reporttext, m.severity, m.severityvalues
			from policymetric m, assessment a
			where m.policyid = 0
				and m.assessmentid = 0
				and m.metricid between @startmetricid and @metricid
				and a.policyid > 0
				-- this check makes it restartable
				and a.assessmentid not in (select distinct assessmentid from policymetric where metricid between @startmetricid and @metricid)
END

GO
