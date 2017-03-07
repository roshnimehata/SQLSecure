/* ---------------------------------------------------------------------- */
/* Schema changes for SQLsecure 3.1 schema version 3100                   */
/* ---------------------------------------------------------------------- */

/* START SQL Secure 3.1 (Barkha Khatri) Register azure servers */ 
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;

/* ---------------------------------------------------------------------- */
/* Validate the db is correct version for upgrade                         */
/*     >= 2000 or < 3000 or null are valid values		                  */
/* ---------------------------------------------------------------------- */

DECLARE @ver INT;
SELECT
    @ver = schemaversion
FROM
    currentversion;
IF (ISNULL(@ver, 900) >= 3100)
    BEGIN
        DECLARE @msg NVARCHAR(500);
        SET @msg = N'Database schema is not at a level that can be upgraded to version 3100';
        IF (@ver IS NOT NULL)
            EXEC isp_sqlsecure_addactivitylog @activitytype = 'Failure Audit',
                @source = 'Install', @eventcode = 'Upgrade',
                @category = 'Schema', @description = @msg,
                @connectionname = NULL;
        RAISERROR (@msg, 16, 1);
    END;
GO


IF OBJECT_ID('registeredserver', 'U') IS NOT NULL 
BEGIN
IF COL_LENGTH('registeredserver','servertype') IS NULL
 BEGIN
	ALTER TABLE registeredserver
	ADD servertype NVARCHAR(3) NOT NULL DEFAULT('OP')
 END
END

IF OBJECT_ID('sqldatabase', 'U') IS NOT NULL 
BEGIN
	IF COL_LENGTH('sqldatabase','istdeencrypted') IS NULL
	 BEGIN
		ALTER TABLE sqldatabase
		ADD istdeencrypted BIT NOT NULL DEFAULT(0)
	END
	IF COL_LENGTH('sqldatabase','wasbackupnotencrypted') IS NULL
	 BEGIN
		ALTER TABLE sqldatabase
		ADD wasbackupnotencrypted BIT NOT NULL DEFAULT(0)
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
		ADD isdatamasked BIT NOT NULL DEFAULT(0)
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
		ADD isrowsecurityenabled bit NOT NULL DEFAULT(0)
	 END
	 IF COL_LENGTH('databaseobject','FQN') IS NULL
	 BEGIN
		ALTER TABLE databaseobject
		ADD FQN nvarchar(1000)
	END
END
GO


IF NOT EXISTS ( SELECT *
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


	end

/* END SQL Secure 3.1 (Barkha Khatri) Register azure servers */ 
/* START SQL Secure 3.1 (Barkha Khatri) Risk assessments */ 
IF OBJECT_ID('metric', 'U') IS NOT NULL 
BEGIN
IF COL_LENGTH('metric','applicableonazuredb') IS NULL
 BEGIN
	ALTER TABLE metric
	ADD applicableonazuredb int  
	CONSTRAINT col_applicableonazuredb_def  
	DEFAULT 0  NOT NULL; 
 END
 IF COL_LENGTH('metric','applicableonazurevm') IS NULL
 BEGIN
	ALTER TABLE metric
	ADD applicableonazurevm int  
	CONSTRAINT col_applicableonazurevm_def  
	DEFAULT 0  NOT NULL; 
 END
 IF COL_LENGTH('metric','applicableonpremise') IS NULL
 BEGIN
	ALTER TABLE metric
	ADD applicableonpremise int  
	CONSTRAINT col_applicableonpremise_def  
	DEFAULT 1  NOT NULL; 

 END
END
/* END SQL Secure 3.1 (Barkha Khatri) Risk assessments */
GO
