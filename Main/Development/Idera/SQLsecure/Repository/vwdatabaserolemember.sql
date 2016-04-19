SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwdatabaserolemember]'))
drop view [dbo].[vwdatabaserolemember]
GO

CREATE VIEW [dbo].[vwdatabaserolemember] as
select distinct 
a.snapshotid, 
a.uid, 
a.dbid, 
a.name, 
a.type, 
b.rolememberuid 
from databaseprincipal a, databaserolemember b 
where 
a.snapshotid = b.snapshotid and 
a.uid = b.groupuid and
a.dbid = b.dbid and
a.type = 'R'

GO

GRANT SELECT ON [dbo].[vwdatabaserolemember] TO [SQLSecureView]

GO
