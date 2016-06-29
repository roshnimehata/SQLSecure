SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwallaccounts]'))
drop view [dbo].[vwallaccounts]
GO

CREATE VIEW [vwallaccounts] as

select distinct a.snapshotid, a.sid, a.type, a.name, login=CASE WHEN b.sid IS NULL THEN 'N' ELSE 'Y' END, state=state 
from windowsaccount a left outer join serverprincipal b on a.snapshotid = b.snapshotid and a.sid = b.sid
union
select distinct a.snapshotid, a.sid, a.type, a.name, login='Y', state='G' from serverprincipal a where type IN ('S')

GO

GRANT SELECT ON [dbo].[vwallaccounts] TO [SQLSecureView]

GO
