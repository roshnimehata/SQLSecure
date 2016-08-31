SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwschemapermission]'))
drop view [dbo].[vwschemapermission]
GO

create VIEW vwschemapermission as
select distinct
a.snapshotid,
a.dbid,
a.schemaid,
a.uid,
a.schemaname,
b.classid,
b.permission,
b.grantee,
b.grantor,
b.isgrant,
b.isgrantwith,
b.isrevoke,
b.isdeny
FROM 
databaseschema a, 
databaseschemapermission b 
where 
a.snapshotid = b.snapshotid and 
a.dbid = b.dbid and 
a.schemaid = b.schemaid

 GO
 
 GRANT SELECT ON [dbo].[vwschemapermission] TO [SQLSecureView]
 
 GO
