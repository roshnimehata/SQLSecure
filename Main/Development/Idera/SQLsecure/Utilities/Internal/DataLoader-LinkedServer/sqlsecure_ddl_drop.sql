/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v4.0.3                     */
/* Target DBMS:           MS SQL Server 2000                              */
/* Project file:          sqlsecure.dez                                   */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Database drop script                            */
/* Created on:            2006-06-29 16:17                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

ALTER TABLE [collectionschedule] DROP CONSTRAINT [FK__collectio__conne__239E4DCF]
GO

ALTER TABLE [databaseobject] DROP CONSTRAINT [FK__databaseo__class__398D8EEE]
GO

ALTER TABLE [databaseobject] DROP CONSTRAINT [FK__databaseobject__3A81B327]
GO

ALTER TABLE [databaseobject] DROP CONSTRAINT [FK__databaseobject__3B75D760]
GO

ALTER TABLE [databaseobject] DROP CONSTRAINT [FK__databaseob__type__3C69FB99]
GO

ALTER TABLE [databaseobject] DROP CONSTRAINT [FK__databaseobject__3D5E1FD2]
GO

ALTER TABLE [databaseobjectpermission] DROP CONSTRAINT [FK__databaseobjectpe__412EB0B6]
GO

ALTER TABLE [databaseobjectpermission] DROP CONSTRAINT [FK__databaseobjectpe__403A8C7D]
GO

ALTER TABLE [databaseprincipal] DROP CONSTRAINT [FK__databaseprincipa__7F60ED59]
GO

ALTER TABLE [databaseprincipal] DROP CONSTRAINT [FK__databaseprincipa__00551192]
GO

ALTER TABLE [databaseprincipalpermission] DROP CONSTRAINT [FK__databasep__class__0F975522]
GO

ALTER TABLE [databaseprincipalpermission] DROP CONSTRAINT [FK__databaseprincipa__108B795B]
GO

ALTER TABLE [databaseprincipalpermission] DROP CONSTRAINT [FK__databaseprincipa__0DAF0CB0]
GO

ALTER TABLE [databaseprincipalpermission] DROP CONSTRAINT [FK__databaseprincipa__0EA330E9]
GO

ALTER TABLE [databaserolemember] DROP CONSTRAINT [FK__databaserolememb__33D4B598]
GO

ALTER TABLE [databaserolemember] DROP CONSTRAINT [FK__databaserolememb__34C8D9D1]
GO

ALTER TABLE [databaseschema] DROP CONSTRAINT [FK__databaseschema__0519C6AF]
GO

ALTER TABLE [databaseschemapermission] DROP CONSTRAINT [FK__databases__class__08EA5793]
GO

ALTER TABLE [databaseschemapermission] DROP CONSTRAINT [FK__databaseschemape__09DE7BCC]
GO

ALTER TABLE [databaseschemapermission] DROP CONSTRAINT [FK__databaseschemape__07F6335A]
GO

ALTER TABLE [databaseschemapermission] DROP CONSTRAINT [FK__databaseschemape__0AD2A005]
GO

ALTER TABLE [endpoint] DROP CONSTRAINT [FK__endpoint__snapsh__29572725]
GO

ALTER TABLE [filterrule] DROP CONSTRAINT [FK__filterrul__class__4E88ABD4]
GO

ALTER TABLE [filterrule] DROP CONSTRAINT [FK__filterrul__filte__4F7CD00D]
GO

ALTER TABLE [filterruleheader] DROP CONSTRAINT [FK__filterrul__conne__49C3F6B7]
GO

ALTER TABLE [serverfilterrule] DROP CONSTRAINT [FK__serverfilterrule__1920BF5C]
GO

ALTER TABLE [serverfilterruleheader] DROP CONSTRAINT [FK__serverfil__snaps__164452B1]
GO

ALTER TABLE [serverpermission] DROP CONSTRAINT [FK__serverper__class__2C3393D0]
GO

ALTER TABLE [serverpermission] DROP CONSTRAINT [FK__serverpermission__2D27B809]
GO

ALTER TABLE [serverprincipal] DROP CONSTRAINT [FK__serverpri__snaps__79A81403]
GO

ALTER TABLE [serverrolemember] DROP CONSTRAINT [FK__serverrolemember__267ABA7A]
GO

ALTER TABLE [sqldatabase] DROP CONSTRAINT [FK__sqldataba__snaps__7C8480AE]
GO

ALTER TABLE [windowsaccount] DROP CONSTRAINT [FK__windowsac__snaps__1367E606]
GO

ALTER TABLE [windowsgroupmember] DROP CONSTRAINT [FK__windowsgroupmemb__300424B4]
GO

ALTER TABLE [windowsgroupmember] DROP CONSTRAINT [FK__windowsgroupmemb__30F848ED]
GO

/* ---------------------------------------------------------------------- */
/* Drop table "ancillarywindowsgroup"                                     */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [ancillarywindowsgroup] DROP CONSTRAINT [PK__ancillarywindows__534D60F1]
GO

/* Drop table */

DROP TABLE [ancillarywindowsgroup]
GO

/* ---------------------------------------------------------------------- */
/* Drop table "applicationactivity"                                       */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [applicationactivity] DROP CONSTRAINT [PK__applicationactiv__1B0907CE]
GO

/* Drop table */

DROP TABLE [applicationactivity]
GO

/* ---------------------------------------------------------------------- */
/* Drop table "applicationlicense"                                        */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [applicationlicense] DROP CONSTRAINT [PK__applicationlicen__46E78A0C]
GO

/* Drop table */

DROP TABLE [applicationlicense]
GO

/* ---------------------------------------------------------------------- */
/* Drop table "classtype"                                                 */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [classtype] DROP CONSTRAINT [PK__classtype__023D5A04]
GO

/* Drop table */

DROP TABLE [classtype]
GO

/* ---------------------------------------------------------------------- */
/* Drop table "collectionschedule"                                        */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [collectionschedule] DROP CONSTRAINT [PK__collectionschedu__22AA2996]
GO

/* Drop table */

DROP TABLE [collectionschedule]
GO

/* ---------------------------------------------------------------------- */
/* Drop table "compatibleversion"                                         */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [compatibleversion] DROP CONSTRAINT [PK__compatibleversio__1CF15040]
GO

/* Drop table */

DROP TABLE [compatibleversion]
GO

/* ---------------------------------------------------------------------- */
/* Drop table "currentversion"                                            */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [currentversion] DROP CONSTRAINT [PK__currentversion__1ED998B2]
GO

/* Drop table */

DROP TABLE [currentversion]
GO

/* ---------------------------------------------------------------------- */
/* Drop table "databaseobject"                                            */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [databaseobject] DROP CONSTRAINT [PK__databaseobject__38996AB5]
GO

/* Drop table */

DROP TABLE [databaseobject]
GO

/* ---------------------------------------------------------------------- */
/* Drop table "databaseobjectpermission"                                  */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [databaseobjectpermission] DROP CONSTRAINT [PK__databaseobjectpe__3F466844]
GO

/* Drop table */

DROP TABLE [databaseobjectpermission]
GO

/* ---------------------------------------------------------------------- */
/* Drop table "databaseprincipal"                                         */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [databaseprincipal] DROP CONSTRAINT [PK__databaseprincipa__7E6CC920]
GO

/* Drop table */

DROP TABLE [databaseprincipal]
GO

/* ---------------------------------------------------------------------- */
/* Drop table "databaseprincipalpermission"                               */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [databaseprincipalpermission] DROP CONSTRAINT [PK__databaseprincipa__0CBAE877]
GO

/* Drop table */

DROP TABLE [databaseprincipalpermission]
GO

/* ---------------------------------------------------------------------- */
/* Drop table "databaserolemember"                                        */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [databaserolemember] DROP CONSTRAINT [PK__databaserolememb__32E0915F]
GO

/* Drop table */

DROP TABLE [databaserolemember]
GO

/* ---------------------------------------------------------------------- */
/* Drop table "databaseschema"                                            */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [databaseschema] DROP CONSTRAINT [PK__databaseschema__0425A276]
GO

/* Drop table */

DROP TABLE [databaseschema]
GO

/* ---------------------------------------------------------------------- */
/* Drop table "databaseschemapermission"                                  */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [databaseschemapermission] DROP CONSTRAINT [PK__databaseschemape__07020F21]
GO

/* Drop table */

DROP TABLE [databaseschemapermission]
GO

/* ---------------------------------------------------------------------- */
/* Drop table "endpoint"                                                  */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [endpoint] DROP CONSTRAINT [PK__endpoint__286302EC]
GO

/* Drop table */

DROP TABLE [endpoint]
GO

/* ---------------------------------------------------------------------- */
/* Drop table "filterrule"                                                */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [filterrule] DROP CONSTRAINT [PK__filterrule__4D94879B]
GO

/* Drop table */

DROP TABLE [filterrule]
GO

/* ---------------------------------------------------------------------- */
/* Drop table "filterruleclass"                                           */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [filterruleclass] DROP CONSTRAINT [PK__filterruleclass__4BAC3F29]
GO

/* Drop table */

DROP TABLE [filterruleclass]
GO

/* ---------------------------------------------------------------------- */
/* Drop table "filterruleheader"                                          */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [filterruleheader] DROP CONSTRAINT [PK__filterruleheader__48CFD27E]
GO

/* Drop table */

DROP TABLE [filterruleheader]
GO

/* ---------------------------------------------------------------------- */
/* Drop table "objecttype"                                                */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [objecttype] DROP CONSTRAINT [PK__objecttype__36B12243]
GO

/* Drop table */

DROP TABLE [objecttype]
GO

/* ---------------------------------------------------------------------- */
/* Drop table "registeredserver"                                          */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [registeredserver] DROP CONSTRAINT [PK__registeredserver__20C1E124]
GO

/* Drop table */

DROP TABLE [registeredserver]
GO

/* ---------------------------------------------------------------------- */
/* Drop table "serverfilterrule"                                          */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [serverfilterrule] DROP CONSTRAINT [PK__serverfilterrule__182C9B23]
GO

/* Drop table */

DROP TABLE [serverfilterrule]
GO

/* ---------------------------------------------------------------------- */
/* Drop table "serverfilterruleheader"                                    */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [serverfilterruleheader] DROP CONSTRAINT [PK__serverfilterrule__15502E78]
GO

/* Drop table */

DROP TABLE [serverfilterruleheader]
GO

/* ---------------------------------------------------------------------- */
/* Drop table "serverpermission"                                          */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [serverpermission] DROP CONSTRAINT [PK__serverpermission__2B3F6F97]
GO

/* Drop table */

DROP TABLE [serverpermission]
GO

/* ---------------------------------------------------------------------- */
/* Drop table "serverprincipal"                                           */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [serverprincipal] DROP CONSTRAINT [PK__serverprincipal__78B3EFCA]
GO

/* Drop table */

DROP TABLE [serverprincipal]
GO

/* ---------------------------------------------------------------------- */
/* Drop table "serverrolemember"                                          */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [serverrolemember] DROP CONSTRAINT [PK__serverrolemember__25869641]
GO

/* Drop table */

DROP TABLE [serverrolemember]
GO

/* ---------------------------------------------------------------------- */
/* Drop table "serversnapshot"                                            */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [serversnapshot] DROP CONSTRAINT [PK__serversnapshot__76CBA758]
GO

/* Drop table */

DROP TABLE [serversnapshot]
GO

/* ---------------------------------------------------------------------- */
/* Drop table "serverstatistic"                                           */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [serverstatistic] DROP CONSTRAINT [PK__serverstatistic__4316F928]
GO

/* Drop table */

DROP TABLE [serverstatistic]
GO

/* ---------------------------------------------------------------------- */
/* Drop table "snapshothistory"                                           */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [snapshothistory] DROP CONSTRAINT [PK__snapshothistory__44FF419A]
GO

/* Drop table */

DROP TABLE [snapshothistory]
GO

/* ---------------------------------------------------------------------- */
/* Drop table "sqldatabase"                                               */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [sqldatabase] DROP CONSTRAINT [PK__sqldatabase__7B905C75]
GO

/* Drop table */

DROP TABLE [sqldatabase]
GO

/* ---------------------------------------------------------------------- */
/* Drop table "sqlserverobjecttype"                                       */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [sqlserverobjecttype] DROP CONSTRAINT [PK__sqlserverobjectt__5165187F]
GO

/* Drop table */

DROP TABLE [sqlserverobjecttype]
GO

/* ---------------------------------------------------------------------- */
/* Drop table "windowsaccount"                                            */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [windowsaccount] DROP CONSTRAINT [PK__windowsaccount__1273C1CD]
GO

/* Drop table */

DROP TABLE [windowsaccount]
GO

/* ---------------------------------------------------------------------- */
/* Drop table "windowsgroupmember"                                        */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [windowsgroupmember] DROP CONSTRAINT [PK__windowsgroupmemb__2F10007B]
GO

/* Drop table */

DROP TABLE [windowsgroupmember]
GO

/* ---------------------------------------------------------------------- */
/* Drop table "coveringpermissionhierarchy"                               */
/* ---------------------------------------------------------------------- */

/* Drop table */

DROP TABLE [coveringpermissionhierarchy]
GO

/* ---------------------------------------------------------------------- */
/* Drop table "columnpermissionreference"                                 */
/* ---------------------------------------------------------------------- */

/* Drop table */

DROP TABLE [columnpermissionreference]
GO
