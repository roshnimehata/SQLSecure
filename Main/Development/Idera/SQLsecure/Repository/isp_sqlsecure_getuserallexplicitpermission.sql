SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_getuserallexplicitpermission]'))
drop procedure [dbo].[isp_sqlsecure_getuserallexplicitpermission]
GO

CREATE procedure [dbo].[isp_sqlsecure_getuserallexplicitpermission] (@snapshotid int, @dbid int, @uid int)
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --             Get all explict permissions belong to database users or roles excluding fixed db roles
   -- 	           

	create table #tmpuserpermission (objectname nvarchar(256), objecttype nvarchar(64), permission nvarchar(64), grantor nvarchar(500), grantee nvarchar(500), isgrant nchar(1), isgrantwith nchar(1), isdeny nchar(1), owner nvarchar(500))

	create table #tmpuid (uid int)

	declare @newuid int
	set @newuid = @uid

	-- checks for alias
	if exists (select 1 from databaseprincipal where snapshotid = @snapshotid and dbid = @dbid and uid = @uid and isalias = 'Y')
	begin
		select @newuid = altuid from databaseprincipal where snapshotid = @snapshotid and dbid = @dbid and uid = @uid and isalias = 'Y'

	end

	-- only process single user
	insert into #tmpuserpermission exec isp_sqlsecure_getuserexplicitpermission @snapshotid=@snapshotid, @dbid=@dbid, @uid=@newuid

	select distinct * from #tmpuserpermission

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_getuserallexplicitpermission] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
