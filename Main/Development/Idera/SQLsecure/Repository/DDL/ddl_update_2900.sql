/* ---------------------------------------------------------------------- */
/* Schema changes for SQLsecure 2.8 schema version 2800                   */
/* ---------------------------------------------------------------------- */

SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON

/* ---------------------------------------------------------------------- */
/* Validate the db is correct version for upgrade                         */
/*     >= 2000 or < 2900 or null are valid values		                  */
/* ---------------------------------------------------------------------- */

declare @ver int
SELECT @ver=schemaversion FROM currentversion
if (isnull(@ver,900) >= 2900)
BEGIN
		declare @msg nvarchar(500)
		set @msg = N'Database schema is not at a level that can be upgraded to version 2900'
		if (@ver is not null)
			exec isp_sqlsecure_addactivitylog @activitytype='Failure Audit', @source='Install', @eventcode='Upgrade', @category='Schema', @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
END
else
Begin
	BEGIN TRANSACTION
	ALTER TABLE dbo.[windowsaccount]
	ADD [enabled] [bit] NOT NULL CONSTRAINT [DF_windowsaccount_enabled]  DEFAULT ((1))

	DECLARE @SQL as varchar(4000)
	SET @SQL ='
	ALTER VIEW [dbo].[vwwindowsgroupmembers]
	AS
	SELECT DISTINCT a.snapshotid, a.sid, a.type, a.name, a.state, a.hashkey, b.groupsid
	FROM         dbo.windowsaccount AS a INNER JOIN
	                      dbo.windowsgroupmember AS b ON a.snapshotid = b.snapshotid AND a.sid = b.groupmember'
	EXEC(@SQL)
    	COMMIT
end
GO	
