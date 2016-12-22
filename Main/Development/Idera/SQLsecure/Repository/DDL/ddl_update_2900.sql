/* ---------------------------------------------------------------------- */
/* Schema changes for SQLsecure 2.9 schema version 2900                   */
/* ---------------------------------------------------------------------- */

SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON

/* ---------------------------------------------------------------------- */
/* Validate the db is correct version for upgrade                         */
/*     >= 2000 or < 2900 or null are valid values		                  */
/* ---------------------------------------------------------------------- */

declare @ver int
SELECT @ver=schemaversion FROM currentversion
if (isnull(@ver,900) < 2900) 
Begin
	ALTER TABLE dbo.[windowsaccount]
	ADD [enabled] [bit] NOT NULL CONSTRAINT [DF_windowsaccount_enabled]  DEFAULT ((1))
end
GO	
