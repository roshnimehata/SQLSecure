SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwservice]'))
drop view [dbo].[vwservice]
GO

create VIEW [vwservice] as

SELECT DISTINCT 
	svc.snapshotid,
	svc.servicetype,
	svc.servicename,
	svc.displayname,
	svc.servicepath,
	svc.startuptype,
	svc.state,
	svc.loginname
FROM serverservice AS svc

GO

GRANT SELECT ON [dbo].[vwservice] TO [SQLSecureView]

GO
