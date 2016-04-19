SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwschema]'))
drop view [dbo].[vwschema]
GO

create VIEW [vwschema] as

SELECT DISTINCT 
	dbschema.snapshotid, 
	dbschema.dbid, 
	dbschema.schemaid, 
	dbschema.schemaname, 
	dbschema.uid AS ownerid,
	dbuser.name AS ownername,
	dbuser.type AS ownertype
FROM databaseschema AS dbschema
	LEFT OUTER JOIN databaseprincipal AS dbuser ON (dbuser.snapshotid = dbschema.snapshotid AND dbuser.dbid = dbschema.dbid AND dbuser.uid = dbschema.uid)

GO

GRANT SELECT ON [dbo].[vwschema] TO [SQLSecureView]

GO
