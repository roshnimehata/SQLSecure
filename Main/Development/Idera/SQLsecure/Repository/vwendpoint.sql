SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwendpoint]'))
drop view [dbo].[vwendpoint]
GO

create VIEW [vwendpoint] as

SELECT DISTINCT 
	endp.snapshotid, 
	endp.endpointid, 
	sprin.name AS principalname,
	endp.name, 
	endp.type, 
	endp.protocol, 
	endp.state, 
	endp.isadminendpoint 
FROM endpoint AS endp
	LEFT OUTER JOIN serverprincipal AS sprin ON (endp.snapshotid = sprin.snapshotid AND endp.principalid = sprin.principalid)

GO

GRANT SELECT ON [dbo].[vwendpoint] TO [SQLSecureView]

GO
