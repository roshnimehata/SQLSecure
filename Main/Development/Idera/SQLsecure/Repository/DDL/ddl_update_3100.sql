/* ---------------------------------------------------------------------- */
/* Schema changes for SQLsecure 3.1 schema version 3100                   */
/* ---------------------------------------------------------------------- */

/* START SQL Secure 3.1 (Barkha Khatri) Register azure servers */ 
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF OBJECT_ID('registeredserver', 'U') IS NOT NULL 
BEGIN
IF COL_LENGTH('registeredserver','servertype') IS NULL
 BEGIN
	ALTER TABLE registeredserver
	ADD servertype NVARCHAR(3) NOT NULL DEFAULT('OP')
 END
END

/* END SQL Secure 3.1 (Barkha Khatri) Register azure servers */ 
/* START SQL Secure 3.1 (Anshul Aggarwal) New risk assessments */ 
IF OBJECT_ID('sqldatabase', 'U') IS NOT NULL 
BEGIN
	IF COL_LENGTH('sqldatabase','istdeencrypted') IS NULL
	 BEGIN
		ALTER TABLE sqldatabase
		ADD istdeencrypted bit
	END
	IF COL_LENGTH('sqldatabase','nativebackupnotencrypted') IS NULL
	 BEGIN
		ALTER TABLE sqldatabase
		ADD wasbackupnotencrypted bit
	END
	IF COL_LENGTH('sqldatabase','FQN') IS NULL
	 BEGIN
		ALTER TABLE sqldatabase
		ADD FQN nvarchar(1000)
	END
END

IF OBJECT_ID('databaseobject', 'U') IS NOT NULL 
	BEGIN
	IF COL_LENGTH('databaseobject','isdatamasked') IS NULL
	 BEGIN
		ALTER TABLE databaseobject
		ADD isdatamasked bit
	 END
	 IF COL_LENGTH('databaseobject','alwaysencryptiontype') IS NULL
	 BEGIN
		ALTER TABLE databaseobject
		ADD alwaysencryptiontype int
	 END
	 IF COL_LENGTH('databaseobject','signedcrypttype') IS NULL
	 BEGIN
		ALTER TABLE databaseobject
		ADD signedcrypttype char(4)
	 END
	 IF COL_LENGTH('databaseobject','isrowsecurityenabled') IS NULL
	 BEGIN
		ALTER TABLE databaseobject
		ADD isrowsecurityenabled bit
	 END
	 IF COL_LENGTH('databaseobject','FQN') IS NULL
	 BEGIN
		ALTER TABLE databaseobject
		ADD FQN nvarchar(1000)
	END
END

-- SQLsecure 3.1 (Anshul Aggarwal) - New columns for NTFS Encrytion.
IF OBJECT_ID('serverosobject', 'U') IS NOT NULL 
BEGIN
	IF COL_LENGTH('serverosobject','issqldatabasefolder') IS NULL
	 BEGIN
		ALTER TABLE serverosobject
		ADD issqldatabasefolder BIT NOT NULL DEFAULT(0)
	END
	IF COL_LENGTH('serverosobject','isencrypted') IS NULL
	 BEGIN
		ALTER TABLE serverosobject
		ADD isencrypted BIT NULL
	END
END

GO


IF NOT EXISTS ( SELECT TOP 1 *
FROM   dbo.sysobjects
WHERE  id = OBJECT_ID(N'[dbo].[azuresqldbfirewallrules]')
    AND xtype = N'U' ) 
	begin
			CREATE TABLE [dbo].[azuresqldbfirewallrules](
						[snapshotid] [int] NOT NULL,
						[dbid] [int] NOT NULL,
						[isserverlevel] bit NOT NULL,
						[name] [nvarchar](128) NOT NULL,
						[startipaddress] [varchar](50) NULL,
						[endipaddress] [varchar](50) NOT NULL,
			CONSTRAINT [PK_azuresqldbfirewallrules] PRIMARY KEY CLUSTERED 
			(
				[snapshotid] ASC,
				[dbid] ASC,
				[isserverlevel] ASC,
				[name] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
			) ON [PRIMARY]

	ALTER TABLE [dbo].[azuresqldbfirewallrules]  WITH NOCHECK ADD  CONSTRAINT [FK_azuresqldbfirewallrules_serversnapshot] FOREIGN KEY([snapshotid])
	REFERENCES [dbo].[serversnapshot] ([snapshotid]) ON DELETE CASCADE;

	ALTER TABLE [dbo].[azuresqldbfirewallrules] CHECK CONSTRAINT [FK_azuresqldbfirewallrules_serversnapshot]

	ALTER TABLE [dbo].[azuresqldbfirewallrules]  WITH CHECK ADD  CONSTRAINT [FK_azuresqldbfirewallrules_sqldatabase] FOREIGN KEY([snapshotid], [dbid])
	REFERENCES [dbo].[sqldatabase] ([snapshotid], [dbid]) ON DELETE CASCADE;

	ALTER TABLE [dbo].[azuresqldbfirewallrules] CHECK CONSTRAINT [FK_azuresqldbfirewallrules_sqldatabase]

	end
/* END SQL Secure 3.1 (Anshul Aggarwal) New risk assessments */ 
/* START SQL Secure 3.1 (Anshul Aggarwal) Support different metric settings based on type of server */ 
IF NOT EXISTS ( SELECT TOP 1 *
FROM   dbo.sysobjects
WHERE  id = OBJECT_ID(N'[dbo].[metricextendedinfo]')
    AND xtype = N'U' ) 
	begin
			
	CREATE TABLE [dbo].[metricextendedinfo](
		[metricid] [int] NOT NULL,
		[servertype] [nvarchar](3) NOT NULL,
		[metricname] [nvarchar](256) NOT NULL,
		[metricdescription] [nvarchar](1024) NOT NULL,
		[validvalues] [nvarchar](1024) NOT NULL,
		[valuedescription] [nvarchar](1024) NOT NULL
	 CONSTRAINT [PK_metricextendedinfo] PRIMARY KEY CLUSTERED 
	(
		[metricid] ASC,
		[servertype] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[metricextendedinfo]  WITH CHECK ADD  CONSTRAINT [FK_metricextendedinfo_metric] FOREIGN KEY([metricid])
	REFERENCES [dbo].[metric] ([metricid])

	ALTER TABLE [dbo].[metricextendedinfo] CHECK CONSTRAINT [FK_metricextendedinfo_metric]

	end

IF NOT EXISTS ( SELECT TOP 1 *
FROM   dbo.sysobjects
WHERE  id = OBJECT_ID(N'[dbo].[policymetricextendedinfo]')
    AND xtype = N'U' ) 
	begin
			
	CREATE TABLE [dbo].[policymetricextendedinfo](
	[policyid] [int] NOT NULL,
	[metricid] [int] NOT NULL,
	[assessmentid] [int] NOT NULL,
	[servertype] [nvarchar](3) NOT NULL,
	[reportkey] [nvarchar](32) NOT NULL,
	[reporttext] [nvarchar](4000) NOT NULL,
	[severity] [int] NOT NULL,
	[severityvalues] [nvarchar](4000) NULL
	 CONSTRAINT [PK_policymetricextendedinfo] PRIMARY KEY CLUSTERED 
	(
		[policyid] ASC,
		[assessmentid] ASC,
		[metricid] ASC,
		[servertype] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[policymetricextendedinfo]  WITH CHECK ADD  CONSTRAINT [FK_policymetricextendedinfo_assessment] FOREIGN KEY([assessmentid])
	REFERENCES [dbo].[assessment] ([assessmentid])

	ALTER TABLE [dbo].[policymetricextendedinfo] CHECK CONSTRAINT [FK_policymetricextendedinfo_assessment]

	ALTER TABLE [dbo].[policymetricextendedinfo]  WITH CHECK ADD  CONSTRAINT [FK_policymetricextendedinfo_metric] FOREIGN KEY([metricid])
	REFERENCES [dbo].[metric] ([metricid])
	
	ALTER TABLE [dbo].[policymetricextendedinfo] CHECK CONSTRAINT [FK_policymetricextendedinfo_metric]

	ALTER TABLE [dbo].[policymetricextendedinfo]  WITH CHECK ADD  CONSTRAINT [FK_policymetricextendedinfo_policy] FOREIGN KEY([policyid])
	REFERENCES [dbo].[policy] ([policyid])
	
	ALTER TABLE [dbo].[policymetricextendedinfo] CHECK CONSTRAINT [FK_policymetricextendedinfo_policy]
	
	end
/* END SQL Secure 3.1 (Anshul Aggarwal) Support different metric settings based on type of server */ 
GO
