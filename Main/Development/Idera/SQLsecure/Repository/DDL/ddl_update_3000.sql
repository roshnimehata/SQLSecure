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
