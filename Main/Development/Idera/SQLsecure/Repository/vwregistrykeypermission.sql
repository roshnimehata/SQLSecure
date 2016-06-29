SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwregistrykeypermission]'))
drop view [dbo].[vwregistrykeypermission]
GO

create VIEW [vwregistrykeypermission] as

SELECT 
	a.snapshotid, 
	a.osobjectid,
	a.objecttype,
	a.objectname, 
	a.longname,
	a.ownersid,
	a.ownername,
	b.sid,
	isnull(c.name, master.dbo.fn_varbintohexstr(b.sid)) AS grantee,
	b.auditflags,
	b.filesystemrights,
	b.accesstype,
	b.isinherited
FROM [vwregistrykey] a
	INNER JOIN serverosobjectpermission b ON (a.snapshotid = b.snapshotid AND a.osobjectid = b.osobjectid)
	LEFT JOIN serveroswindowsaccount c ON (b.snapshotid = c.snapshotid AND b.sid = c.sid)

GO

GRANT SELECT ON [dbo].[vwregistrykeypermission] TO [SQLSecureView]

GO
