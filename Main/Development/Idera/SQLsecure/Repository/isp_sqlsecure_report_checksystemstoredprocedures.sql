SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_report_checksystemstoredprocedures]'))
drop procedure [dbo].[isp_sqlsecure_report_checksystemstoredprocedures] 
GO

CREATE procedure [dbo].[isp_sqlsecure_report_checksystemstoredprocedures]
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
   --              Checks all servers master db for direct/explicit permission on
   -- 				highly sensitive extended stored procedures

CREATE TABLE #tmpservers (registeredserverid int)
INSERT #tmpservers
    EXEC [dbo].[isp_sqlsecure_getpolicymemberlist]
         @policyid = @policyid 

SELECT	e.connectionname, b.databasename, username = a.name, 
		objectname = c.name, d.permission, d.isgrant, d.isgrantwith
FROM	databaseprincipal a,
		sqldatabase b,
		databaseobject c,
		databaseobjectpermission d,
		dbo.getsnapshotlist(@rundate, @usebaseline) e
WHERE	e.registeredserverid IN (SELECT registeredserverid FROM #tmpservers)
		AND UPPER(e.connectionname) LIKE UPPER(@serverName)		
		AND a.snapshotid = e.snapshotid		
		AND a.snapshotid = b.snapshotid
		AND a.dbid = b.dbid
		AND b.databasename = 'master'
		AND c.snapshotid = b.snapshotid
		AND c.dbid = b.dbid
		AND d.grantee = a.uid 
		AND c.objectid = d.objectid
		AND d.snapshotid = c.snapshotid
		AND d.dbid = c.dbid
		AND c.type = 'X'
		AND (c.name = 'xp_cmdshell'
				OR c.name LIKE 'sp_OA%'
				OR (c.name LIKE 'xp_reg%' AND c.name <> 'xp_regread'))
		AND (d.isgrant = 'Y' OR d.isgrantwith = 'Y')
		AND a.name <> 'system_function_schema'

ORDER BY connectionname, databasename, username, objectname, permission

DROP TABLE #tmpservers

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_report_checksystemstoredprocedures] TO [SQLSecureView]

GO

