SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_getuserexplicitpermission]'))
drop procedure [dbo].[isp_sqlsecure_getuserexplicitpermission]
GO

CREATE procedure [dbo].[isp_sqlsecure_getuserexplicitpermission] (@snapshotid int, @dbid int, @uid int)
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --             Get all explict permissions belong to database users or roles excluding fixed db roles
   -- 	           

	select 
	objectname=b.schemaname, 
	objecttype='iSCM',
	permission=a.permission, 
	grantor=dbo.getdatabaseprincipalname(a.snapshotid, a.dbid, a.grantor),
	grantee=dbo.getdatabaseprincipalname(a.snapshotid, a.dbid, a.grantee),
	a.isgrant, 
	a.isgrantwith, 
	a.isdeny,
	owner=dbo.getdatabaseprincipalname(b.snapshotid, b.dbid, b.uid)
	from 
	databaseschemapermission a,
	databaseschema b
	where
	a.snapshotid = @snapshotid and
	a.dbid = @dbid and
	a.grantee = @uid and
	b.snapshotid = a.snapshotid and
	b.dbid = a.dbid and
	b.schemaid = a.schemaid
	union
	select 
	objectname=b.name, 
	objecttype=CASE WHEN b.type = 'R' THEN 'iDRLE' WHEN b.type = 'U' THEN 'iDUSR' WHEN b.type = 'A' THEN 'iDRLE' WHEN b.type = 'F' THEN 'iDRLE' ELSE 'iDUSR' END ,
	permission=a.permission, 
	grantor=dbo.getdatabaseprincipalname(a.snapshotid, a.dbid, a.grantor),
	grantee=dbo.getdatabaseprincipalname(a.snapshotid, a.dbid, a.grantee),
	a.isgrant, 
	a.isgrantwith, 
	a.isdeny,
	owner=dbo.getdatabaseprincipalname(b.snapshotid, b.dbid, b.uid)
	from 
	databaseprincipalpermission a,
	databaseprincipal b
	where
	a.snapshotid = @snapshotid and
	a.dbid = @dbid and
	a.grantee = @uid and
	b.snapshotid = a.snapshotid and
	b.dbid = a.dbid and
	b.uid = a.uid
	union
	select 
	objectname=a.name, 
	objecttype=a.type,
	permission=b.permission, 
	grantor=dbo.getdatabaseprincipalname(b.snapshotid, b.dbid, b.grantor),
	grantee=dbo.getdatabaseprincipalname(b.snapshotid, b.dbid, b.grantee),
	b.isgrant, 
	b.isgrantwith, 
	b.isdeny,
	owner=dbo.getdatabaseprincipalname(a.snapshotid, a.dbid, a.owner)
	from 
	databaseobject a, 
	databaseobjectpermission b
	where
	b.snapshotid = @snapshotid and
	b.dbid = @dbid and
	b.grantee = @uid and
	a.snapshotid = b.snapshotid and
	a.dbid = b.dbid and
	a.objectid = b.objectid and
	a.parentobjectid = b.parentobjectid and
	a.classid = b.classid and 
	a.type <> 'iCO'
	union
	select 
	objectname=dbo.gettablename(a.snapshotid, a.dbid, a.parentobjectid) + '.' +  a.name, 
	objecttype=a.type,
	permission=b.permission, 
	grantor=dbo.getdatabaseprincipalname(b.snapshotid, b.dbid, b.grantor),
	grantee=dbo.getdatabaseprincipalname(b.snapshotid, b.dbid, b.grantee),
	b.isgrant, 
	b.isgrantwith, 
	b.isdeny,
	owner=dbo.getdatabaseprincipalname(a.snapshotid, a.dbid, dbo.getobjectownerid(a.snapshotid, a.dbid, a.parentobjectid))
	from 
	databaseobject a, 
	databaseobjectpermission b
	where
	b.snapshotid = @snapshotid and
	b.dbid = @dbid and
	b.grantee = @uid and
	a.snapshotid = b.snapshotid and
	a.dbid = b.dbid and
	a.objectid = b.objectid and
	a.parentobjectid = b.parentobjectid and
	a.classid = b.classid and 
	a.type = 'iCO'



GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_getuserexplicitpermission] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
