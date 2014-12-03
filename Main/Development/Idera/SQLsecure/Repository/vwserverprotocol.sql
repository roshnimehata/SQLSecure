SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwserverprotocol]'))
drop view [dbo].[vwserverprotocol]
GO

create VIEW [vwserverprotocol] as

SELECT
     	a.snapshotid,
	a.connectionname,
	a.registeredserverid,
	b.protocolname,
	b.ipaddress,
	b.dynamicport,
	b.port,
	b.enabled,
	b.active
FROM serversnapshot a
	INNER JOIN serverprotocol b ON a.snapshotid = b.snapshotid

GO

GRANT SELECT ON [dbo].[vwserverprotocol] TO [SQLSecureView]

GO
