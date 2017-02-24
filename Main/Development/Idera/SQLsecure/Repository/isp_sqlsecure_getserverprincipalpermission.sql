SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_getserverprincipalpermission]'))
drop procedure [dbo].[isp_sqlsecure_getserverprincipalpermission]
GO

CREATE procedure [dbo].[isp_sqlsecure_getserverprincipalpermission] (@snapshotid int, @uid int)
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --             Get all explict server permissions belonging to a server login or role
   -- 	           
   --SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database --if @serverType = 1 then snapshot is of ADB type server.      
   DECLARE @serverType int;
   SELECT serverType =case when exists(select 1 from registeredserver r join serversnapshot s on r.connectionname = s.connectionname where s.snapshotid = @snapshotid and servertype = 'ADB') then 1 else 0 end
	
	select 
		objectname=case 
					when classid=101 then (select name from serverprincipal where snapshotid = @snapshotid and principalid = a.majorid) 
					when classid=105 then (select name from endpoint where snapshotid = @snapshotid and endpointid = a.majorid) 
					else 'Server' end, 
		objecttype = case when @serverType = 1 then 'Server' else dbo.getclasstype(a.classid) end ,
		permission=a.permission, 
		grantor=dbo.getserverprincipalname(a.snapshotid, a.grantor),
		grantee=dbo.getserverprincipalname(a.snapshotid, a.grantee),
		a.isgrant, 
		a.isgrantwith, 
		a.isdeny
	from 
		serverpermission a
	where
		a.snapshotid = @snapshotid and
		a.grantee = @uid

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_getserverprincipalpermission] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

