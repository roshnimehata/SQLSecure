SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_getserveruserpermission]'))
drop procedure [dbo].[isp_sqlsecure_getserveruserpermission]
GO

CREATE procedure [dbo].[isp_sqlsecure_getserveruserpermission] (@snapshotid int, @logintype nchar(1), @inputsid varbinary(85), @sqllogin nvarchar(128), @permissiontype nchar(1)=null)
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the user permissions at the server associated with a user.
	declare @dbname nvarchar(256)

	create table #tmpserverpermission (snapshotid int, permissionlevel nvarchar(15), logintype nchar(1), loginname nvarchar(256), connectionname nvarchar(400), databasename nvarchar(256), principalid int, principalname nvarchar(128), principaltype nchar(1), databaseprincipal nvarchar(128), databaseprincipaltype nchar(1), grantor int, grantorname nvarchar(128), grantee int, granteename nvarchar(128), classid int, permissiontype nvarchar(4), coveringfrom nvarchar(15), permission nvarchar(256), isgrant nchar(1), isgrantwith nchar(1), isrevoke nchar(1), isdeny nchar(1), parentobjectid int, objectid int, objectname nvarchar(256), qualifiedname nvarchar(256), objecttype nvarchar(64), schemaid int, schemaname nvarchar(128), owner int, ownername nvarchar(128), isaliased nchar(1))

	declare myc100aa cursor for
			select distinct databasename from sqldatabase where snapshotid = @snapshotid
	
	open myc100aa
	fetch next from myc100aa
	into @dbname
	
	while @@fetch_status = 0
	begin
		print 'Processing database ' + @dbname

		--select 'x' from tempdb..sysobjects where type = 'U' and lower(name) like '#tmpserverpermission%'
	
		exec isp_sqlsecure_getuserpermission @snapshotid, @logintype, @inputsid , @sqllogin, @dbname, @permissiontype

		fetch next from myc100aa
		into @dbname
	end

	close myc100aa
	deallocate myc100aa

	if (UPPER(@permissiontype) = 'X')		
		exec ('select *, b.objecttypename from #tmpserverpermission a left outer join objecttype b on a.objecttype = b.objecttype where permissiontype = ''EX''')
	else if (UPPER(@permissiontype) = 'E')
		exec ('select *, b.objecttypename from #tmpserverpermission a left outer join objecttype b on a.objecttype = b.objecttype where permissiontype = ''EF''')
	else
		exec ('select *, b.objecttypename from #tmpserverpermission a left outer join objecttype b on a.objecttype = b.objecttype')

	drop table #tmpserverpermission

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_getserveruserpermission] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

