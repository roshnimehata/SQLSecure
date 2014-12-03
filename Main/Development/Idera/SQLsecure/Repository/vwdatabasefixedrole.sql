SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwdatabasefixedrole]'))
drop view [dbo].[vwdatabasefixedrole]
GO

create VIEW [vwdatabasefixedrole] as

select distinct 
a.snapshotid, 
a.sid,
serverprincipalname=a.name, 
serverprincipaltype=a.type, 
a.serveraccess, 
uid =d.uid,
dbid=d.dbid, 
databaseprincipalname=b.name, 
databaseprincipaltype=b.type, 
isalias=d.isalias, 
role=d.name
from 
serverprincipal a, 
databaseprincipal b,
databaserolemember c,
vwalldatabaserole d
where 
a.snapshotid = b.snapshotid and
a.sid = b.usersid and
c.snapshotid = b.snapshotid and
c.dbid = b.dbid and
c.rolememberuid = b.uid and 
d.snapshotid = c.snapshotid and
d.dbid = c.dbid and
d.uid = c.groupuid

GO

GRANT SELECT ON [dbo].[vwdatabasefixedrole] TO [SQLSecureView]

GO
