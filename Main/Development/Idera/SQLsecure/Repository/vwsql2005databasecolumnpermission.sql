SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwsql2005databasecolumnpermission]'))
drop view [dbo].[vwsql2005databasecolumnpermission]
GO

create VIEW vwsql2005databasecolumnpermission as
select distinct
a.snapshotid, 
a.dbid, 
a.objectid, 
a.classid, 
a.parentobjectid,
owner=dbo.getdatabaseprincipalidwithobjectid(a.snapshotid, a.dbid, a.parentobjectid),
ownername=CASE WHEN a.owner IS NULL THEN dbo.getdatabaseprincipalnamewithobjectid(a.snapshotid, a.dbid, a.parentobjectid) ELSE dbo.getdatabaseprincipalname(a.snapshotid, a.dbid, a.owner) END,
schemaid=dbo.getschemaidwithobjectid(a.snapshotid, a.dbid, a.parentobjectid),
schemaname=dbo.getschemanamewithobjectid(a.snapshotid, a.dbid, a.parentobjectid),
objectname=a.name,
objecttype=a.type,
b.permission, 
b.grantee, 
b.grantor, 
b.isgrant, 
b.isgrantwith, 
b.isrevoke, 
b.isdeny 
FROM 
databaseobject a, 
databaseobjectpermission b 
where 
a.snapshotid = b.snapshotid and 
a.dbid = b.dbid and 
a.objectid = b.objectid and 
a.classid = b.classid and 
a.parentobjectid = b.parentobjectid and
a.owner is null and
a.schemaid is null and
UPPER(a.type) = 'ICO'

GO

GRANT SELECT ON [dbo].[vwsql2005databasecolumnpermission] TO [SQLSecureView]

GO
