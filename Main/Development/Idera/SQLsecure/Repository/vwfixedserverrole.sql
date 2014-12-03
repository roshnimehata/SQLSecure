SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwfixedserverrole]'))
drop view [dbo].[vwfixedserverrole]
GO

create VIEW [vwfixedserverrole] as

select distinct snapshotid, name, type, sid, principalid from serverprincipal where type = 'R'

GO

GRANT SELECT ON [dbo].[vwfixedserverrole] TO [SQLSecureView]

GO
