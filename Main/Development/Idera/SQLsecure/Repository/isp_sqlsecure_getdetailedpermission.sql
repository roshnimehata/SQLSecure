SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_getdetailedpermission]'))
drop procedure [dbo].[isp_sqlsecure_getdetailedpermission]
GO

CREATE procedure [dbo].[isp_sqlsecure_getdetailedpermission] (@permissionlevel nvarchar(128), @coveringpermission nvarchar(128))
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get a list of detailed permissions
   -- 	           
	
	create table #tmpdetailedpermission (permission nvarchar(128))

	insert into #tmpdetailedpermission (permission) select distinct permissionname from coveringpermissionhierarchy where UPPER(coveringpermissionname) = @coveringpermission and UPPER(permissionlevel) = @permissionlevel

	exec('select * from #tmpdetailedpermission')
	
	drop table #tmpdetailedpermission

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_getdetailedpermission] TO [SQLSecureView]

GO

