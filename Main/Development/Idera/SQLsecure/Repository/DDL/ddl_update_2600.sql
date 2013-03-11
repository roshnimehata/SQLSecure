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


