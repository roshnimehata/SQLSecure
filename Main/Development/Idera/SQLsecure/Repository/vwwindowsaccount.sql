SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwwindowsaccount]'))
drop view [dbo].[vwwindowsaccount]
GO

CREATE VIEW [dbo].[vwwindowsaccount] 
AS 
select snapshotid, sid, type, name, state from windowsaccount

GO

GRANT SELECT ON [dbo].[vwwindowsaccount] TO [SQLSecureView]

GO
