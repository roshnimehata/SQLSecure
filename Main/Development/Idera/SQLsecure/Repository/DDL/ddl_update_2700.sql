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


