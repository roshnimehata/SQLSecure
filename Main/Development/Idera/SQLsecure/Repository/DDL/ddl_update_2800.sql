/* ---------------------------------------------------------------------- */
/* Schema changes for SQLsecure 2.8 schema version 2800                   */
/* ---------------------------------------------------------------------- */

SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON

/* ---------------------------------------------------------------------- */
/* Validate the db is correct version for upgrade                         */
/*     >= 2000 or < 2800 or null are valid values		                  */
/* ---------------------------------------------------------------------- */

DECLARE @ver INT
SELECT  @ver = schemaversion
FROM    currentversion
IF (ISNULL(@ver , 900) >= 2800) 
   BEGIN
         DECLARE @msg NVARCHAR(500)
         SET @msg = N'Database schema is not at a level that can be upgraded to version 2800'
         IF (@ver IS NOT NULL) 
            EXEC isp_sqlsecure_addactivitylog @activitytype = 'Failure Audit' ,
                @source = 'Install' , @eventcode = 'Upgrade' ,
                @category = 'Schema' , @description = @msg ,
                @connectionname = NULL
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
		ALTER COLUMN longname VARCHAR(max) null

		ALTER TABLE dbo.policyinterview
		ALTER COLUMN interviewtext VARCHAR(max) null
   
		ALTER TABLE dbo.assessment
		ALTER COLUMN assessmentnotes VARCHAR(max) not null

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
         COMMIT
   END

GO	

GO

/* ---------------------------------------------------------------------- */
/*	Updates to new fields must be done after finalizing the changes		  */
/* ---------------------------------------------------------------------- */
declare @ver int
SELECT @ver=schemaversion FROM currentversion
if (isnull(@ver,900) < 2800)
BEGIN
	BEGIN TRANSACTION

	--insert into the newly created configuration table
	INSERT INTO dbo.configuration (lastupdated, isweakpassworddetectionenabled) VALUES (GETUTCDATE(), N'Y') 
	
	COMMIT
END

GO	



