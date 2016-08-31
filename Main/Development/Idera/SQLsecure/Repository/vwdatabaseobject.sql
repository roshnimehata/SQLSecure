SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwdatabaseobject]'))
drop view [dbo].[vwdatabaseobject]
GO

create VIEW [vwdatabaseobject] as

select distinct snapshotid, dbid, classid, parentobjectid, objectid, schemaid, type, owner, name, runatstartup, isencrypted, userdefined
	from databaseobject

GO

GRANT SELECT ON [dbo].[vwdatabaseobject] TO [SQLSecureView]

GO
