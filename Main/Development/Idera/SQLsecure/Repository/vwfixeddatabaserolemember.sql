SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwfixeddatabaserolemember]'))
drop view [dbo].[vwfixeddatabaserolemember]
GO

CREATE VIEW [dbo].[vwfixeddatabaserolemember] as
select distinct 
a.snapshotid, 
a.uid, 
a.dbid, 
a.name, 
a.type, 
b.rolememberuid 
from databaseprincipal a, databaserolemember b 
where 
a.snapshotid = b.snapshotid and 
a.uid = b.groupuid and
a.dbid = b.dbid and 
UPPER(a.name) IN ('DB_ACCESSADMIN', 'DB_BACKUPOPERATOR' ,'DB_DATAREADER','DB_DATAWRITER','DB_DDLADMIN','DB_DENYDATAREADER','DB_DENYDATAWRITER','DB_OWNER','DB_SECURITYADMIN','DB_ACCESSADMIN') and
a.type = 'R'

GO

GRANT SELECT ON [dbo].[vwfixeddatabaserolemember] TO [SQLSecureView]

GO
