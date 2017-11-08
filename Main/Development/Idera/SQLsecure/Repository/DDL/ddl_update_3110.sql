/* ---------------------------------------------------------------------- */
/* Schema changes for SQLsecure 3.1.1 schema version 3110                   */
/* ---------------------------------------------------------------------- */

/* START SQL Secure 3.1.1 (XOR) Ability to remove a server without having to remove it first from an assessment or draft */ 
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;

/* ---------------------------------------------------------------------- */
/* Validate the db is correct version for upgrade                         */
/*     >= 2000 or < 3110 or null are valid values		                  */
/* ---------------------------------------------------------------------- */

DECLARE @ver INT;
SELECT
    @ver = schemaversion
FROM
    currentversion;
IF (ISNULL(@ver, 900) >= 3110)
    BEGIN
        DECLARE @msg NVARCHAR(500);
        SET @msg = N'Database schema is not at a level that can be upgraded to version 3110';
        IF (@ver IS NOT NULL)
            EXEC isp_sqlsecure_addactivitylog @activitytype = 'Failure Audit',
                @source = 'Install', @eventcode = 'Upgrade',
                @category = 'Schema', @description = @msg,
                @connectionname = NULL;
        RAISERROR (@msg, 16, 1);
    END;
GO

IF OBJECT_ID('policymember', 'U') IS NOT NULL 
BEGIN
	DECLARE @foreignKey nvarchar(100)
	SELECT
		@foreignKey = name 
		FROM sys.foreign_keys
		WHERE parent_object_id = object_id('policymember')
		and referenced_object_id = object_id('registeredserver')
	if(@foreignKey is not null)
		exec ('alter table policymember drop constraint '+ @foreignKey )
END

IF NOT EXISTS ( SELECT TOP 1 *
	FROM   dbo.sysobjects
	WHERE  id = OBJECT_ID(N'[dbo].[unregisteredserver]')
		AND xtype = N'U' ) 
	begin
		CREATE TABLE [dbo].[unregisteredserver](
			[connectionname] [nvarchar](400) NOT NULL,
			[servername] [nvarchar](128) NULL,
			[instancename] [nvarchar](128) NULL,
			[snapshotretentionperiod] [int] NULL,
			[authenticationmode] [nvarchar](1) NULL,
			[sqlserverlogin] [nvarchar](128) NULL,
			[sqlserverpassword] [nvarchar](300) NULL,
			[sqlserverauthtype] [nchar](1) NULL,
			[serverlogin] [nvarchar](128) NULL,
			[serverpassword] [nvarchar](300) NULL,
			[os] [nvarchar](512) NULL,
			[version] [nvarchar](256) NULL,
			[edition] [nvarchar](256) NULL,
			[loginauditmode] [nvarchar](20) NULL,
			[enableproxyaccount] [nchar](1) NULL,
			[enablec2audittrace] [nchar](1) NULL,
			[crossdbownershipchaining] [nchar](1) NULL,
			[casesensitivemode] [nchar](1) NULL,
			[jobid] [uniqueidentifier] NULL,
			[lastcollectiontm] [datetime] NULL,
			[lastcollectionsnapshotid] [int] NULL,
			[currentcollectiontm] [datetime] NULL,
			[currentcollectionstatus] [nchar](1) NULL,
			[registeredserverid] [int],
			[serverisdomaincontroller] [nchar](1) NULL,
			[replicationenabled] [nchar](1) NULL,
			[sapasswordempty] [nchar](1) NULL,
			[connectionport] [int] NULL,
			[auditfoldersstring] [nvarchar](max) NULL,
			[servertype] [nvarchar](3) NULL,
			PRIMARY KEY ([registeredserverid])
		)
	end
GO

