/* ---------------------------------------------------------------------- */
/* Schema changes for SQLsecure 3.0 schema version 3001                   */
/* ---------------------------------------------------------------------- */

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
IF (ISNULL(@ver, 900) >= 3001)
    BEGIN
        DECLARE @msg NVARCHAR(500);
        SET @msg = N'Database schema is not at a level that can be upgraded to version 3000';
        IF (@ver IS NOT NULL)
            EXEC isp_sqlsecure_addactivitylog @activitytype = 'Failure Audit',
                @source = 'Install', @eventcode = 'Upgrade',
                @category = 'Schema', @description = @msg,
                @connectionname = NULL;
        RAISERROR (@msg, 16, 1);
    END;
GO
