SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwosobjectpermission]'))
drop view [dbo].[vwosobjectpermission]
GO

create VIEW [vwosobjectpermission] as

SELECT 
	obj.snapshotid, 
	obj.osobjectid,
	obj.objecttype,
	obj.dbid,
	isnull(db.databasename,'') AS databasename,
	obj.objectname, 
	isnull(obj.longname,obj.objectname) AS longname,
	obj.ownersid,
	isnull(own.name, master.dbo.fn_varbintohexstr(obj.ownersid)) AS ownername,
	perm.filesystemrights,
	perm.sid,
	isnull(usr.name, master.dbo.fn_varbintohexstr(perm.sid)) AS username,
	perm.accesstype,
	perm.auditflags,
	perm.isinherited
FROM serverosobject AS obj
	LEFT JOIN sqldatabase AS db ON (obj.snapshotid = db.snapshotid AND obj.dbid = db.dbid)
	INNER JOIN serverosobjectpermission perm ON (obj.snapshotid = perm.snapshotid AND obj.osobjectid = perm.osobjectid)
	LEFT JOIN serveroswindowsaccount own ON (obj.snapshotid = own.snapshotid AND obj.ownersid = own.sid)
	LEFT JOIN serveroswindowsaccount usr ON (perm.snapshotid = usr.snapshotid AND perm.sid = usr.sid)

GO

GRANT SELECT ON [dbo].[vwosobjectpermission] TO [SQLSecureView]

GO

