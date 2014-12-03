SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwloginfixedserverrole]'))
drop view [dbo].[vwloginfixedserverrole]
GO

create VIEW [vwloginfixedserverrole] as

select distinct
a.snapshotid,
a.sid,
a.principalid,
principalname=a.name, 
principaltype=a.type, 
serveraccess=a.serveraccess, 
serverdeny=a.serverdeny, 
disabled=a.disabled, 
rolename=c.name 
from serverprincipal a left outer join serverrolemember b on a.snapshotid = b.snapshotid and a.principalid = b.memberprincipalid left outer join serverprincipal c on b.snapshotid = c.snapshotid and a.snapshotid = c.snapshotid and b.principalid = c.principalid and c.type = 'R'
where
a.type IN ('U', 'G', 'S')

GO

GRANT SELECT ON [dbo].[vwloginfixedserverrole] TO [SQLSecureView]

GO
