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
	 print 'test'
   END

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



