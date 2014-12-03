SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwfixedserverrolemember]'))
drop view [dbo].[vwfixedserverrolemember]
GO

CREATE VIEW [dbo].[vwfixedserverrolemember] as
select a.snapshotid, a.principalid, a.sid, a.name, a.type, b.memberprincipalid 
from serverprincipal a, serverrolemember b 
where 
a.snapshotid = b.snapshotid and 
a.principalid = b.principalid and
a.type = 'R'

GO

GRANT SELECT ON [dbo].[vwfixedserverrolemember] TO [SQLSecureView]

GO
