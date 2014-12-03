SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_report_checkxpmail]'))
drop procedure [dbo].[isp_sqlsecure_report_checkxpmail]
GO

CREATE procedure [dbo].[isp_sqlsecure_report_checkxpmail]
(
	@rundate datetime = null,	--defaults to all
	@policyid int = 1,			--defaults to all
	@usebaseline bit = 0,		--defaults to false
	@serverName nvarchar(400)
)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Checks all servers for xp mail
   -- 	           

CREATE TABLE #tmpservers (registeredserverid int)
INSERT #tmpservers
    EXEC [dbo].[isp_sqlsecure_getpolicymemberlist]
         @policyid = @policyid  

SELECT	DISTINCT d.connectionname, a.name
FROM	databaseobject a,
		sqldatabase b,
		dbo.getsnapshotlist(@rundate, @usebaseline) d
WHERE	d.registeredserverid IN (SELECT registeredserverid FROM #tmpservers)
		AND a.snapshotid = d.snapshotid
		AND UPPER(d.connectionname) LIKE UPPER(@serverName)		
		AND a.snapshotid = b.snapshotid
		AND a.dbid = b.dbid
		AND a.type = 'X'
		AND a.name LIKE 'xp_%mail'

ORDER BY d.connectionname, a.name

DROP TABLE #tmpservers

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_report_checkxpmail] TO [SQLSecureView]

GO

