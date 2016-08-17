/* ---------------------------------------------------------------------- */
/* Schema changes for SQLsecure 3.0 schema version 3000                   */
/* ---------------------------------------------------------------------- */

SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;

/* ---------------------------------------------------------------------- */
/* Validate the db is correct version for upgrade                         */
/*     >= 2000 or < 3000 or null are valid values		                  */
/* ---------------------------------------------------------------------- */

DECLARE @ver INT;
SELECT  @ver = schemaversion
FROM    currentversion;
IF ( ISNULL(@ver, 900) >= 3000 )
BEGIN
        DECLARE @msg NVARCHAR(500);
        SET @msg = N'Database schema is not at a level that can be upgraded to version 3000';
        IF ( @ver IS NOT NULL )
            EXEC isp_sqlsecure_addactivitylog @activitytype = 'Failure Audit',
                @source = 'Install', @eventcode = 'Upgrade',
                @category = 'Schema', @description = @msg,
                @connectionname = NULL;
        RAISERROR (@msg, 16, 1);
    END;

----SQL Jobs and Agent Check
IF NOT EXISTS(
	SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'sqljob' AND COLUMN_NAME = 'ProxyId'
) 
BEGIN
	ALTER TABLE dbo.[sqljob]
	ADD [ProxyId] [int] NULL;
END


SET QUOTED_IDENTIFIER ON;


CREATE TABLE [dbo].[tags]
    (
      [tag_id] [INT] IDENTITY(1, 1)
                     NOT NULL ,
      [name] [NVARCHAR](250) COLLATE SQL_Latin1_General_CP1_CI_AS
                             NULL ,
      [description] [NVARCHAR](500) NULL ,
      [is_default] [BIT] NULL
                         CONSTRAINT [DF_tags_is_default] DEFAULT ( (0) ) ,
      CONSTRAINT [PK_tags] PRIMARY KEY CLUSTERED ( [tag_id] ASC )
        WITH ( PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF,
               IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON,
               ALLOW_PAGE_LOCKS = ON ) ON [PRIMARY]
    )
ON  [PRIMARY];



ALTER TABLE tags ADD CONSTRAINT
tag_unique_name UNIQUE NONCLUSTERED
(
name
);

GO 

CREATE TABLE [dbo].[server_tags]
    (
      [server_tag_id] [INT] IDENTITY(1, 1)
                            NOT NULL ,
      [server_id] [INT] NULL ,
      [tag_id] [INT] NULL ,
      CONSTRAINT [PK_server_tags] PRIMARY KEY CLUSTERED
        ( [server_tag_id] ASC )
        WITH ( PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF,
               IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON,
               ALLOW_PAGE_LOCKS = ON ) ON [PRIMARY]
    )
ON  [PRIMARY];


ALTER TABLE [dbo].[server_tags]  WITH CHECK ADD  CONSTRAINT [FK_server_tags_registeredserver] FOREIGN KEY([server_id])
REFERENCES [dbo].[registeredserver] ([registeredserverid])
ON DELETE CASCADE;


ALTER TABLE [dbo].[server_tags] CHECK CONSTRAINT [FK_server_tags_registeredserver];

ALTER TABLE [dbo].[server_tags]  WITH CHECK ADD  CONSTRAINT [FK_server_tags_tags] FOREIGN KEY([tag_id])
REFERENCES [dbo].[tags] ([tag_id]);


ALTER TABLE [dbo].[server_tags] CHECK CONSTRAINT [FK_server_tags_tags];


GO	

/****** Object:  Table [dbo].[encryptionkey]    Script Date: 7/29/2016 4:46:46 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[encryptionkey](
	[keyid] [INT] IDENTITY(1,1) NOT NULL,
	[name] [NVARCHAR](255) NOT NULL,
	[principalid] [INT] NULL,
	[dbkeyid] [INT] NULL,
	[keylength] [INT] NULL,
	[algorithm] [NVARCHAR](50) NULL,
	[algorithmdesc] [NVARCHAR](60) NULL,
	[providertype] [NVARCHAR](60) NULL,
	[snapshotid] [INT] NOT NULL,
	[databaseid] [INT] NOT NULL,
	[key_type] [NVARCHAR](50) NULL,
 CONSTRAINT [PK_encryptionkey] PRIMARY KEY CLUSTERED 
(
	[keyid] ASC,
	[snapshotid] ASC,
	[databaseid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[encryptionkey]  WITH NOCHECK ADD  CONSTRAINT [FK_encryptionkey_serversnapshot] FOREIGN KEY([snapshotid])
REFERENCES [dbo].[serversnapshot] ([snapshotid]) ON DELETE CASCADE;
GO

ALTER TABLE [dbo].[encryptionkey] CHECK CONSTRAINT [FK_encryptionkey_serversnapshot]
GO

ALTER TABLE [dbo].[encryptionkey]  WITH CHECK ADD  CONSTRAINT [FK_encryptionkey_sqldatabase] FOREIGN KEY([snapshotid], [databaseid])
REFERENCES [dbo].[sqldatabase] ([snapshotid], [dbid]) ON DELETE CASCADE;
go

ALTER TABLE [dbo].[encryptionkey] CHECK CONSTRAINT [FK_encryptionkey_sqldatabase]
GO




CREATE TABLE [dbo].[databasecertificates]
    (
      [certid] [INT] IDENTITY(1, 1) NOT NULL ,
      [snapshotid] [INT] NOT NULL ,
      [dbid] [INT] NOT NULL ,
      [name] NVARCHAR(128) NOT NULL ,
      [certificate_id] INT NOT NULL ,
      [principal_id] INT NULL ,
      [pvt_key_encryption_type] CHAR(2) NOT NULL ,
      [pvt_key_encryption_type_desc] NVARCHAR(60) NULL ,
      [is_active_for_begin_dialog] BIT NULL ,
      [issuer_name] NVARCHAR(442) NULL ,
      [cert_serial_number] NVARCHAR(64) NULL ,
      [sid] VARBINARY(85) NULL ,
      [string_sid] NVARCHAR(128) NULL ,
      [subject] NVARCHAR(4000) NULL ,
      [expiry_date] DATETIME NULL ,
      [start_date] DATETIME NULL ,
      [thumbprint] VARBINARY(32) NOT NULL ,
      [attested_by] NVARCHAR(260) NULL ,
      [pvt_key_last_backup_date] DATETIME NULL ,
      CONSTRAINT [PK_databasecertificates] PRIMARY KEY CLUSTERED
        ( [certid], [snapshotid], [dbid] )
    ) 
ALTER TABLE [dbo].[databasecertificates] WITH CHECK ADD CONSTRAINT [FK_sqldatabase] FOREIGN KEY ([snapshotid],[dbid])
REFERENCES [dbo].[sqldatabase] ([snapshotid], [dbid])

IF NOT EXISTS ( SELECT
                    1
                FROM
                    sysobjects
                WHERE
                    name = 'linkedserver'
                    AND xtype = 'U' )
    BEGIN
        CREATE TABLE [dbo].[linkedserver]
            (
              [snapshotid] [INT] NOT NULL ,
              [serverid] [INT] NOT NULL ,
              [servername] [NVARCHAR](255) NOT NULL ,
              CONSTRAINT [PK_linkedservers] PRIMARY KEY CLUSTERED
                ( [snapshotid],[serverid] )
            )
    END;

IF NOT EXISTS ( SELECT
                    1
                FROM
                    sysobjects
                WHERE
                    name = 'linkedserverprincipal'
                    AND xtype = 'U' )
    BEGIN
        CREATE TABLE [dbo].[linkedserverprincipal]
            (
              [lspid] INT IDENTITY(1, 1)
                          NOT NULL ,
              [snapshotid] [INT] NOT NULL ,
              [serverid] [INT] NOT NULL ,
              [principal] [NVARCHAR](255) NOT NULL ,
              CONSTRAINT [PK_linkedserverprincipals] PRIMARY KEY CLUSTERED
                ( [lspid], [snapshotid] )
            )
        ALTER TABLE [dbo].[linkedserverprincipal] WITH CHECK ADD CONSTRAINT[FK_linkedserver] FOREIGN KEY([snapshotid],[serverid]) REFERENCES [dbo].[linkedserver]([snapshotid], [serverid])
    END;

GO
