SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwallosaccounts]'))
drop view [dbo].[vwallosaccounts]
GO

CREATE VIEW [vwallosaccounts] as

select distinct a.snapshotid, a.sid, a.type, a.name, login=CASE WHEN b.sid IS NULL THEN 'N' ELSE 'Y' END, state=state 
from serveroswindowsaccount a left outer join serverprincipal b on a.snapshotid = b.snapshotid and a.sid = b.sid

GO

GRANT SELECT ON [dbo].[vwallosaccounts] TO [SQLSecureView]

GO

