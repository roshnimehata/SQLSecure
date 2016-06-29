SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwregistrykey]'))
drop view [dbo].[vwregistrykey]
GO

create VIEW [vwregistrykey] as

SELECT 
	reg.snapshotid, 
	reg.osobjectid,
	reg.objecttype,
	reg.objectname, 
	isnull(reg.longname,reg.objectname) as longname,
	reg.ownersid,
	isnull(own.name, master.dbo.fn_varbintohexstr(reg.ownersid)) AS ownername
FROM serverosobject AS reg
	LEFT JOIN serveroswindowsaccount own ON (reg.snapshotid = own.snapshotid AND reg.ownersid = own.sid)
WHERE reg.objecttype = 'Reg'

GO

GRANT SELECT ON [dbo].[vwregistrykey] TO [SQLSecureView]

GO
