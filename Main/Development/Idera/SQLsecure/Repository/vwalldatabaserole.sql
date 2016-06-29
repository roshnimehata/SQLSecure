SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwalldatabaserole]'))
drop view [dbo].[vwalldatabaserole]
GO

create VIEW [vwalldatabaserole] as

select distinct 
snapshotid, 
dbid,
uid,
owner,
name,
usersid,
type,
isalias,
hasaccess
from databaseprincipal where type = 'R'

GO

GRANT SELECT ON [dbo].[vwalldatabaserole] TO [SQLSecureView]

GO
