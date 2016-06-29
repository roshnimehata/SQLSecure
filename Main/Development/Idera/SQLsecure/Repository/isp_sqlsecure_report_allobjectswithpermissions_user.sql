SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_report_allobjectswithpermissions_user]'))
drop procedure [dbo].[isp_sqlsecure_report_allobjectswithpermissions_user]
GO

CREATE procedure [dbo].[isp_sqlsecure_report_allobjectswithpermissions_user]
(
	@rundate datetime = null,	--defaults to all
	@policyid int = 1,			--defaults to all
	@usebaseline bit = 0,		--defaults to false
	@serverName nvarchar(400),
	@databaseName nvarchar(4000) 	--we don't use nvarchar(max) here for sQL Server 2000 support
)
AS

CREATE TABLE #tmpservers (registeredserverid int)
INSERT #tmpservers
    EXEC [dbo].[isp_sqlsecure_getpolicymemberlist]
         @policyid = @policyid    


SELECT	e.connectionname AS 'Server',
		d.databasename AS 'DB',
		a.objectname AS 'Object',
		c.objecttypename AS 'Type',
		b.name AS 'User',
		dbo.getdatabaseprincipaltypename(isnull(dbp.type, isnull(m.type, b.type))) as 'User Type',
		dbo.getaccesstype(a.isdeny, a.isgrantwith, a.isgrant, 'N') AS 'Access Type',
		a.permission AS Privilege,
		CASE m.sid WHEN b.usersid THEN '' ELSE isnull(m.name, dbp.name) END as 'Login',
		CASE isnull(sp.disabled, 'N') WHEN 'N' THEN isnull(wa.enabled, 1) ELSE 0 END as 'Enabled'
FROM	vwdatabaseobjectpermission AS a,
		databaseprincipal AS b left join vwwindowsgroupmembers as m
			on m.snapshotid = b.snapshotid
			AND (m.groupsid = b.usersid or m.sid = b.usersid)
			left join windowsaccount as wa
			on wa.snapshotid = b.snapshotid
			and wa.sid=m.sid
			left join vwdatabaserolemember as gm2
			on gm2.snapshotid = b.snapshotid
			AND (gm2.uid = b.uid)
			AND gm2.dbid = b.dbid
			left join databaseprincipal as dbp
			on gm2.rolememberuid = dbp.uid and gm2.snapshotid = dbp.snapshotid and gm2.dbid = dbp.dbid
			left join serverprincipal as sp
			on sp.snapshotid = b.snapshotid
			and (sp.sid=b.usersid OR sp.sid = dbp.usersid),
		objecttype AS c,
		sqldatabase AS d,
		dbo.getsnapshotlist(@rundate, @usebaseline) e
WHERE	e.registeredserverid IN (SELECT registeredserverid FROM #tmpservers)
		AND UPPER(e.connectionname) LIKE UPPER(@serverName)
		AND a.snapshotid = e.snapshotid
		AND a.snapshotid = b.snapshotid
		AND a.dbid = b.dbid
		AND a.grantee = b.uid
		AND a.objecttype = c.objecttype
		AND d.snapshotid = a.snapshotid
		AND d.dbid = a.dbid
		AND (@databaseName = '%' OR d.databasename in (select LTRIM(Value) from dbo.splitbydelimiter(@databaseName, ',')))
DROP TABLE #tmpservers

GO
GRANT EXECUTE ON [dbo].[isp_sqlsecure_report_allobjectswithpermissions_user] TO [SQLSecureView]
GO