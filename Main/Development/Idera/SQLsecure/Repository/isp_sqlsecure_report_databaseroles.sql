SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO
 
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_report_databaseroles]'))
drop procedure [dbo].[isp_sqlsecure_report_databaseroles]
GO

CREATE procedure [dbo].[isp_sqlsecure_report_databaseroles] 
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
		dbo.getyesnotext(m.hasaccess) AS hasaccess
FROM	cte_snapshots AS s,
		sqldatabase AS db,
		databaseprincipal AS r,
		databaserolemember AS rm,
		databaseprincipal AS m
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

UNION

SELECT	s.connectionname AS [Server],
		db.databasename AS [Database],
		r.name AS [Role],
		m.name AS Member,
		dbo.getdatabaseprincipaltypename(m.type) AS logintype,
		'via role ' + mr.name AS Access,
		dbo.getyesnotext(m.isalias) AS isalias,
		dbo.getyesnotext(m.hasaccess) AS hasaccess
FROM	cte_snapshots AS s,
		sqldatabase AS db,
		databaseprincipal AS r,
		databaserolemember AS rm,
		databaseprincipal AS mr,
		databaserolemember AS mrm,
		databaseprincipal AS m		
WHERE	db.snapshotid = s.snapshotid
		AND r.snapshotid = db.snapshotid
		AND r.dbid = db.dbid
		AND r.type = 'R'
		AND rm.snapshotid = r.snapshotid
		AND rm.dbid = r.dbid
		AND rm.groupuid = r.uid
		AND mr.snapshotid = rm.snapshotid
		AND mr.dbid = rm.dbid
		AND mr.uid = rm.rolememberuid
		AND mr.type = 'R'
		AND mrm.snapshotid = mr.snapshotid
		AND mrm.dbid = mr.dbid
		AND mrm.groupuid = mr.uid
		AND m.snapshotid = mrm.snapshotid
		AND m.dbid = mrm.dbid
		AND m.uid = mrm.rolememberuid

UNION

SELECT	s.connectionname AS [Server],
		db.databasename AS [Database],
		r.name AS [Role],
		gm.name AS Member,
		CASE WHEN gm.type = 'User' THEN 'Windows Login' ELSE 'Windows Group' END AS logintype,
		'via group ' + gm.groupname AS Access,
		'-',
		'-'
FROM	cte_snapshots AS s
		INNER JOIN sqldatabase AS db
			ON db.snapshotid = s.snapshotid
		INNER JOIN databaseprincipal AS r
			ON r.snapshotid = db.snapshotid
				AND r.dbid = db.dbid
				AND r.type = 'R'
		INNER JOIN databaserolemember AS rm
			ON rm.snapshotid = r.snapshotid
				AND rm.dbid = r.dbid
				AND rm.groupuid = r.uid
		INNER JOIN databaseprincipal AS mg
			ON mg.snapshotid = rm.snapshotid
				AND mg.dbid = rm.dbid
				AND mg.uid = rm.rolememberuid
				AND mg.type = 'G'
		INNER JOIN cte_allgroupmembers gm
			ON gm.snapshotid = mg.snapshotid
		AND gm.rootsid = mg.usersid
WHERE	gm.grouplevel > 0

ORDER BY s.connectionname, db.databasename, r.name

DROP TABLE #tmpservers

GO
 
GRANT EXECUTE ON [dbo].[isp_sqlsecure_report_databaseroles] TO [SQLSecureView]

GO

