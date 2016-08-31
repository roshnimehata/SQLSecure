SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwsql2005implicitdatabasecolumnpermission]'))
drop view [dbo].[vwsql2005implicitdatabasecolumnpermission]
GO

create VIEW vwsql2005implicitdatabasecolumnpermission as

	select distinct
	a.snapshotid, 
	a.dbid, 
	a.objectid, 
	a.classid, 
	a.parentobjectid,
	objectname=a.name,
	objecttype=a.type,
	b.permission, 
	b.grantee, 
	b.grantor, 
	a.owner,
	isgrant='Y', 
	isgrantwith='N',
	isrevoke='N',
	isdeny='N'
	from 
	databaseobject a, 
	databaseobjectpermission b
	where 
	a.snapshotid = b.snapshotid and 
	a.dbid = b.dbid and 
	a.classid = b.classid and 
	(a.parentobjectid = b.objectid or a.parentobjectid = b.parentobjectid) and 
	b.classid = 1 and
	b.permission IN ('SELECT', 'UPDATE', 'REFERENCES') and
	a.type = 'iCO'
	UNION
	select distinct
	a.snapshotid, 
	a.dbid, 
	a.objectid, 
	a.classid, 
	a.parentobjectid,
	objectname=a.name,
	objecttype=a.type,
	c.permission, 
	b.grantee, 
	b.grantor, 
	a.owner,
	isgrant='Y', 
	isgrantwith='N',
	isrevoke='N',
	isdeny='N'
	from 
	databaseobject a, 
	databaseobjectpermission b,
	columnpermissionreference c
	where 
	a.snapshotid = b.snapshotid and 
	a.dbid = b.dbid and 
	a.classid = b.classid and 
	(a.parentobjectid = b.objectid or a.parentobjectid = b.parentobjectid) and 
	b.classid = 1 and
	b.permission = 'CONTROL' and
	b.permission = c.parentpermission and
	a.type = 'iCO'

GO

GRANT SELECT ON [dbo].[vwsql2005implicitdatabasecolumnpermission] TO [SQLSecureView]

GO

