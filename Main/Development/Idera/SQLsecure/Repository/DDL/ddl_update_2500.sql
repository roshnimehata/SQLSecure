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

