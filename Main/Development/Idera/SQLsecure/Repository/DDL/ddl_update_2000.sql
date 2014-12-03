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

