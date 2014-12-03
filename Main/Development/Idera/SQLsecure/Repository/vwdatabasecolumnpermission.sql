SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwdatabasecolumnpermission]'))
drop view [dbo].[vwdatabasecolumnpermission]
GO

create VIEW vwdatabasecolumnpermission as
select distinct
a.snapshotid, 
a.dbid, 
a.objectid, 
a.classid, 
a.parentobjectid,
owner=dbo.getobjectownerid(a.snapshotid, a.dbid, a.parentobjectid),
a.schemaid,
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
a.type = 'iCO'

GO

GRANT SELECT ON [dbo].[vwdatabasecolumnpermission] TO [SQLSecureView]

GO
