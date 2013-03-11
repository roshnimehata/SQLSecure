SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwoswindowsaccount]'))
drop view [dbo].[vwoswindowsaccount]
GO

CREATE VIEW [dbo].[vwoswindowsaccount] 
AS 
select snapshotid, sid, type, name, state from serveroswindowsaccount

GO

GRANT SELECT ON [dbo].[vwoswindowsaccount] TO [SQLSecureView]

GO
