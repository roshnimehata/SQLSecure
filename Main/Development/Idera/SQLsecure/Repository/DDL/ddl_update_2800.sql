/* ---------------------------------------------------------------------- */
/* Schema changes for SQLsecure 2.8 schema version 2800                   */
/* ---------------------------------------------------------------------- */

SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON

/* ---------------------------------------------------------------------- */
/* Validate the db is correct version for upgrade                         */
/*     >= 2000 or < 2800 or null are valid values		                  */
/* ---------------------------------------------------------------------- */

declare @ver int
SELECT @ver=schemaversion FROM currentversion
if (isnull(@ver,900) >= 2800)
BEGIN
		declare @msg nvarchar(500)
		set @msg = N'Database schema is not at a level that can be upgraded to version 2800'
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

         ALTER TABLE dbo.serversnapshot
         ADD isdistributor NCHAR(1) NULL

         ALTER TABLE dbo.serversnapshot
         ADD ispublisher NCHAR(1) NULL

         ALTER TABLE dbo.serversnapshot
         ADD hasremotepublisher NCHAR(1) NULL

         ALTER TABLE dbo.databaseobject
         ADD PERMISSION_SET  INT NULL,
		 CreateDate DATETIME NULL,
		 ModifyDate DATETIME NULL

         ALTER TABLE dbo.sqldatabase
         ADD IsContained BIT null
 
		ALTER TABLE dbo.serverosobject
		ALTER COLUMN longname NVARCHAR(max) null

		ALTER TABLE dbo.policyinterview
		ALTER COLUMN interviewtext NVARCHAR(max) null
   
		ALTER TABLE dbo.assessment
		ALTER COLUMN assessmentnotes NVARCHAR(max) not null

         IF NOT EXISTS ( SELECT *
                         FROM   dbo.sysobjects
                         WHERE  id = OBJECT_ID(N'[dbo].[sqljob]')
                                AND xtype = N'U' ) 
            BEGIN
				
                  CREATE TABLE [dbo].[sqljob]
                         (
                          [JobId] [int] IDENTITY(1 , 1) NOT NULL
                         ,[SnapshotId] [int] NOT NULL
                         ,[Name] [nvarchar](128) NOT NULL
                         ,[Desciprion] [nvarchar](512) NULL
                         ,[Step] [nvarchar](128) NULL
                         ,[LastRunDate] [datetime] NULL
                         ,[Command] [nvarchar](MAX) NULL
                         ,[SubSystem] [nvarchar](40) NULL
                         ,[Ownersid] [varbinary](85) NOT NULL
                         ,[Enabled] [smallint] NULL
                         )
                  ON     [PRIMARY] TEXTIMAGE_ON [PRIMARY]


                  SET ANSI_PADDING OFF

                  ALTER TABLE [dbo].[sqljob]  WITH NOCHECK ADD  CONSTRAINT [FK_sqljob_serversnapshot] FOREIGN KEY([SnapshotId])
                  REFERENCES [dbo].[serversnapshot] ([snapshotid])

                  ALTER TABLE [dbo].[sqljob] CHECK CONSTRAINT [FK_sqljob_serversnapshot]
            END 
         IF NOT EXISTS ( SELECT *
                         FROM   dbo.sysobjects
                         WHERE  id = OBJECT_ID(N'[dbo].[sqljobproxy]')
                                AND xtype = N'U' ) 
            BEGIN
                  CREATE TABLE [dbo].[sqljobproxy]
                         (
                          [proxyId] [int] IDENTITY(1 , 1) NOT NULL
                         ,[snapshotid] [int] NOT NULL
                         ,[proxyName] [nvarchar](128) NOT NULL
                         ,[enabled] [tinyint] NULL
                         ,[usersid] [varbinary](85) NULL
                         ,[subsystemid] [int] NULL
                         ,[subsystem] [nvarchar](40) NULL
                         ,[credentialId] [int] NULL
                         ,[credentialName] [nvarchar](128) NULL
                         ,[credentialIdentity] [nvarchar](128) NULL
                         )
                  ON     [PRIMARY]


                  SET ANSI_PADDING OFF

                  ALTER TABLE [dbo].[sqljobproxy]  WITH NOCHECK ADD  CONSTRAINT [FK_sqljobproxie_serversnapshot] FOREIGN KEY([snapshotid])
                  REFERENCES [dbo].[serversnapshot] ([snapshotid])

                  ALTER TABLE [dbo].[sqljobproxy] CHECK CONSTRAINT [FK_sqljobproxie_serversnapshot]
            END 			       

			--Support for contained users
			alter table dbo.databaseprincipal
			add IsContainedUser bit,
			AuthenticationType nvarchar(60)
			--Support audit folders
			alter table dbo.registeredserver
			add auditfoldersstring nvarchar(max) null
 
			alter table dbo.serversnapshot
			add isclrenabled nchar(1) null
 
 			alter table dbo.serversnapshot
			add isdefaulttraceenabled nchar(1) null 
			
			alter table dbo.serversnapshot
			add numerrorlogs smallint null           

			  IF NOT EXISTS ( SELECT *
                         FROM   dbo.sysobjects
                         WHERE  id = OBJECT_ID(N'[dbo].[availabilitygroups]')
                                AND xtype = N'U' ) 
								begin
										SET ANSI_NULLS ON
										SET QUOTED_IDENTIFIER ON

										CREATE TABLE [dbo].[availabilitygroups](
											[groupid] [uniqueidentifier] NOT NULL,
											[name] [nvarchar](60) NOT NULL,
											[resourceid] [nvarchar](40) NULL,
											[resourcegroupid] [nvarchar](40) NULL,
											[failureconditionlevel] [int] NULL,
											[healthchecktimeout] [int] NULL,
											[automatedbackuppreference] [tinyint] NULL,
											[automatedbackuppreferencedesc] [nvarchar](60) NULL,
											[snapshotid] [int] NOT NULL,
											[servergroupId] [int] IDENTITY(1,1) NOT NULL,
										 CONSTRAINT [PK_availabilitygroups] PRIMARY KEY CLUSTERED 
										(
											[servergroupId] ASC,
											[snapshotid] ASC,
											[groupid] ASC
										)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
										) ON [PRIMARY]


								 end
								   IF NOT EXISTS ( SELECT *
                         FROM   dbo.sysobjects
                         WHERE  id = OBJECT_ID(N'[dbo].[availabilityreplicas]')
                                AND xtype = N'U' ) 
								begin
									SET QUOTED_IDENTIFIER ON

									SET ANSI_PADDING ON

									CREATE TABLE [dbo].[availabilityreplicas](
										[replicaid] [uniqueidentifier] NOT NULL,
										[snapshotid] [int] NOT NULL,
										[groupid] [uniqueidentifier] NOT NULL,
										[replicaservername] [nvarchar](256) NULL,
										[ownersid] [binary](85) NULL,
										[endpointurl] [nvarchar](256) NULL,
										[availabilitymode] [tinyint] NULL,
										[availabilitymodedesc] [nvarchar](60) NULL,
										[failovermode] [tinyint] NULL,
										[failovermodedesc] [nvarchar](60) NULL,
										[createdate] [datetime] NULL,
										[modifydate] [datetime] NULL,
										[replicametadataid] [int] NULL,
										[serverreplicaid] [int] IDENTITY(1,1) NOT NULL,
									 CONSTRAINT [PK_availabilityreplicas] PRIMARY KEY CLUSTERED 
									(
										[serverreplicaid] ASC,
										[snapshotid] ASC,
										[groupid] ASC
									)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
									) ON [PRIMARY]


									SET ANSI_PADDING OFF

									ALTER TABLE [dbo].[availabilityreplicas]  WITH NOCHECK ADD  CONSTRAINT [FK_availabilityreplicas_availabilityreplicas] FOREIGN KEY([serverreplicaid], [snapshotid], [groupid])
									REFERENCES [dbo].[availabilityreplicas] ([serverreplicaid], [snapshotid], [groupid])

									ALTER TABLE [dbo].[availabilityreplicas] CHECK CONSTRAINT [FK_availabilityreplicas_availabilityreplicas]




								end
			           
	COMMIT
END

GO	


