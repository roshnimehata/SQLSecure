SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwancillarywindowsgroup]'))
drop view [dbo].[vwancillarywindowsgroup]
GO

create VIEW [vwancillarywindowsgroup] as

select distinct snapshotid, windowsgroupname, comment from ancillarywindowsgroup

GO

GRANT SELECT ON [dbo].[vwancillarywindowsgroup] TO [SQLSecureView]

GO
