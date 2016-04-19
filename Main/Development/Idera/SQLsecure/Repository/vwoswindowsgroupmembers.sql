SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwoswindowsgroupmembers]'))
drop view [dbo].[vwoswindowsgroupmembers]
GO

CREATE VIEW [vwoswindowsgroupmembers] as

select distinct a.*, b.groupsid
from 
serveroswindowsaccount a, 
serveroswindowsgroupmember b 
where 
a.snapshotid = b.snapshotid and 
a.sid = b.groupmember

GO

GRANT SELECT ON [dbo].[vwoswindowsgroupmembers] TO [SQLSecureView]

GO
