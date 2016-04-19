SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwfilesystemobjectpermission]'))
drop view [dbo].[vwfilesystemobjectpermission]
GO

create VIEW [vwfilesystemobjectpermission] as

SELECT 
	a.snapshotid, 
	a.osobjectid,
	a.objecttype,
	a.dbid,
	a.databasename,
	a.objectname, 
	a.longname,
	a.ownersid,
	a.ownername,
	a.disktype,
	b.sid,
	isnull(c.name, master.dbo.fn_varbintohexstr(b.sid)) AS grantee,
	b.auditflags,
	b.filesystemrights,
	b.accesstype,
	b.isinherited
FROM [vwfilesystemobject] a
	INNER JOIN serverosobjectpermission b ON (a.snapshotid = b.snapshotid AND a.osobjectid = b.osobjectid)
	LEFT JOIN serveroswindowsaccount c ON (b.snapshotid = c.snapshotid AND b.sid = c.sid)

GO

GRANT SELECT ON [dbo].[vwfilesystemobjectpermission] TO [SQLSecureView]

GO
