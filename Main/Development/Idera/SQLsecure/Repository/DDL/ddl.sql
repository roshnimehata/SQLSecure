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
	--SQL Secure 3.1 (Barkha Khatri) Register azure server
	[servertype] NVARCHAR(3) DEFAULT 'OP',
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
