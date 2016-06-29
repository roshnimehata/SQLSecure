SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_report_dbuserfixedrole]'))
drop procedure [dbo].[isp_sqlsecure_report_dbuserfixedrole]
GO

CREATE procedure [dbo].[isp_sqlsecure_report_dbuserfixedrole] 
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
   --				Checks all servers and databases for guest being a member of database fixed roles
   -- 	           

CREATE TABLE #tmpservers (registeredserverid int)
INSERT #tmpservers
    EXEC [dbo].[isp_sqlsecure_getpolicymemberlist]
         @policyid = @policyid 

SELECT	e.connectionname, d.databasename, username = c.name, [role] = a.name
FROM	databaseprincipal a, 
		databaserolemember b, 
		databaseprincipal c,
		sqldatabase d,
		dbo.getsnapshotlist(@rundate, @usebaseline) e
WHERE	e.registeredserverid IN (SELECT registeredserverid FROM #tmpservers)
		AND UPPER(e.connectionname) LIKE UPPER(@serverName)		
		AND a.snapshotid = e.snapshotid 
		AND a.type = 'R'
		AND a.uid > 16383		--database fixed roles are 16384-16393
		AND a.uid < 16394 
		AND b.snapshotid = a.snapshotid
		AND c.snapshotid = a.snapshotid
		AND b.dbid = a.dbid
		AND c.dbid = a.dbid
		AND d.snapshotid = a.snapshotid
		AND d.dbid = a.dbid
		AND b.groupuid = a.uid
		AND b.rolememberuid = c.uid
		AND LOWER(c.name) IN ('public', 'guest') 
		AND c.hasaccess = 'Y'

ORDER BY connectionname, databasename, username, [role]

DROP TABLE #tmpservers

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_report_dbuserfixedrole] TO [SQLSecureView]

GO

