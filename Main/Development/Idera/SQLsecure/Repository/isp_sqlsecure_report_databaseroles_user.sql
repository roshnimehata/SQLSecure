SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO
 
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_report_databaseroles_user]'))
drop procedure [dbo].[isp_sqlsecure_report_databaseroles_user]
GO

CREATE procedure [dbo].[isp_sqlsecure_report_databaseroles_user] 
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
   --              List all members of Database Roles on all databases on all Servers
   --              note, this explodes groups only for one level of nested roles. 
   --

CREATE TABLE #tmpservers (registeredserverid int)
INSERT #tmpservers
    EXEC [dbo].[isp_sqlsecure_getpolicymemberlist]
         @policyid = @policyid    

;WITH cte_snapshots ( snapshotid, connectionname )
AS
(
	SELECT snapshotid, connectionname
		FROM dbo.getsnapshotlist(@rundate, @usebaseline) AS s
		WHERE s.registeredserverid IN (SELECT registeredserverid FROM #tmpservers)
			AND UPPER(s.connectionname) LIKE UPPER(@serverName)
		
)
,cte_allgroupmembers ( snapshotid, sid, type, name, groupsid, grouptype, groupname, grouplevel, rootsid, rootname )
AS
(
	SELECT snapshotid, sid, type, name, cast(null as varbinary(85)), cast(null as nvarchar(128)), CAST(null as nvarchar(200)), 0, sid, name
		FROM windowsaccount
		WHERE type <> 'User'
	UNION ALL
	SELECT m.snapshotid, m.sid, m.type, m.name, g.sid, g.type, g.name, g.grouplevel + 1, g.rootsid, g.rootname
		FROM cte_allgroupmembers g
			INNER JOIN vwwindowsgroupmembers m
				ON g.sid = m.groupsid
					and g.snapshotid = m.snapshotid
			INNER JOIN cte_snapshots s
				ON m.snapshotid = s.snapshotid
)
SELECT	s.connectionname AS [Server],
		db.databasename AS [Database],
		r.name AS [Role],
		m.name AS Member,
		dbo.getdatabaseprincipaltypename(m.type) AS logintype,
		'' AS Access,
		dbo.getyesnotext(m.isalias) AS isalias,
		dbo.getyesnotext(m.hasaccess) AS hasaccess,
		CASE gm.sid WHEN m.usersid THEN '' ELSE isnull(gm.name, dbp.name) END as 'Login',
		CASE isnull(sp.disabled, 'N') WHEN 'N' THEN isnull(wa.enabled, 1) ELSE 0 END as 'Enabled',
		dbo.getdatabaseprincipaltypename(isnull(dbp.type, gm.type)) as 'UserType'
FROM	cte_snapshots AS s,
		sqldatabase AS db,
		databaseprincipal AS r,
		databaserolemember AS rm,
		databaseprincipal AS m left join vwwindowsgroupmembers as gm
			on gm.snapshotid = m.snapshotid
			AND (gm.groupsid = m.usersid or gm.sid = m.usersid)
		left join vwdatabaserolemember as gm2
			on gm2.snapshotid = m.snapshotid
			AND (gm2.uid = m.uid)
			AND gm2.dbid = m.dbid
			left join databaseprincipal as dbp
			on gm2.rolememberuid = dbp.uid and gm2.snapshotid = dbp.snapshotid and gm2.dbid = dbp.dbid
		left join serverprincipal as sp
			on sp.snapshotid = m.snapshotid
			and (sp.sid=m.usersid OR sp.sid = dbp.usersid)
		left join windowsaccount as wa
			on wa.snapshotid = gm.snapshotid
			and wa.sid=gm.sid
WHERE	db.snapshotid = s.snapshotid
		AND r.snapshotid = db.snapshotid
		AND r.dbid = db.dbid
		AND r.type = 'R'
		AND rm.snapshotid = r.snapshotid
		AND rm.dbid = r.dbid
		AND rm.groupuid = r.uid
		AND m.snapshotid = rm.snapshotid
		AND m.dbid = rm.dbid
		AND m.uid = rm.rolememberuid

DROP TABLE #tmpservers

GO
 
GRANT EXECUTE ON [dbo].[isp_sqlsecure_report_databaseroles_user] TO [SQLSecureView]

GO

