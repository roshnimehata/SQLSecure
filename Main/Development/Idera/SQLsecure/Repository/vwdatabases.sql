SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwdatabases]'))
drop view [dbo].[vwdatabases]
GO

create VIEW [vwdatabases] as

select distinct snapshotid, databasename, dbid, owner, guestenabled, available, status, trustworthy from sqldatabase

GO

GRANT SELECT ON [dbo].[vwdatabases] TO [SQLSecureView]

GO
