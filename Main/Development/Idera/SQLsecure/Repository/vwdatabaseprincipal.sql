SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwdatabaseprincipal]'))
drop view [dbo].[vwdatabaseprincipal]
GO

create VIEW [vwdatabaseprincipal] as

SELECT DISTINCT
	dbuser1.snapshotid, 
	dbuser1.dbid, 
	dbuser1.uid, 
	dbuser1.name, 
	dbuser2.name AS owner, 
	login.name AS login, 
	dbuser1.type, 
	dbuser1.isalias, 
	dbuser3.name AS altname, 
	dbuser1.hasaccess, 
	dbuser1.defaultschemaname,
	dbuser1.IsContainedUser,
	dbuser1.AuthenticationType
FROM databaseprincipal AS dbuser1
	LEFT OUTER JOIN databaseprincipal AS dbuser2 ON (dbuser1.snapshotid = dbuser2.snapshotid AND dbuser1.dbid = dbuser2.dbid AND dbuser1.owner = dbuser2.uid)
	LEFT OUTER JOIN serverprincipal AS login ON (dbuser1.snapshotid = login.snapshotid AND dbuser1.usersid = login.sid)
	LEFT OUTER JOIN databaseprincipal AS dbuser3 ON (dbuser1.snapshotid = dbuser3.snapshotid AND dbuser1.dbid = dbuser3.dbid AND dbuser3.altuid = dbuser3.uid)

GO

GRANT SELECT ON [dbo].[vwdatabaseprincipal] TO [SQLSecureView]

GO
