SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwwindowsgroupmembers]'))
drop view [dbo].[vwwindowsgroupmembers]
GO

CREATE VIEW [vwwindowsgroupmembers] as

select distinct a.*, b.groupsid
from 
windowsaccount a, 
windowsgroupmember b 
where 
a.snapshotid = b.snapshotid and 
a.sid = b.groupmember

GO

GRANT SELECT ON [dbo].[vwwindowsgroupmembers] TO [SQLSecureView]

GO
