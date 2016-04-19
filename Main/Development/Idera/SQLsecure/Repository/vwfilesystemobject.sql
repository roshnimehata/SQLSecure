SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwfilesystemobject]'))
drop view [dbo].[vwfilesystemobject]
GO

create VIEW [vwfilesystemobject] as

SELECT 
	obj.snapshotid, 
	obj.osobjectid,
	obj.objecttype,
	obj.dbid,
	isnull(db.databasename,'') AS databasename,
	obj.objectname, 
	isnull(obj.longname,obj.objectname) AS longname,
	obj.ownersid,
	isnull(own.name, master.dbo.fn_varbintohexstr(obj.ownersid)) AS ownername,
	obj.disktype
FROM serverosobject AS obj
	LEFT JOIN sqldatabase AS db ON (obj.snapshotid = db.snapshotid AND obj.dbid = db.dbid)
	LEFT JOIN serveroswindowsaccount own ON (obj.snapshotid = own.snapshotid AND obj.ownersid = own.sid)
WHERE obj.objecttype != 'Reg'

GO

GRANT SELECT ON [dbo].[vwfilesystemobject] TO [SQLSecureView]

GO

