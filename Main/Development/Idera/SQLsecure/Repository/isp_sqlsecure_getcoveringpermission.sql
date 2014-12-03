SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_getcoveringpermission]'))
drop procedure [dbo].[isp_sqlsecure_getcoveringpermission]
GO

CREATE procedure [dbo].[isp_sqlsecure_getcoveringpermission] (@parentpermissionlevel nvarchar(128), @parentcoveringpermission nvarchar(128), @permissionlevel nvarchar(128))
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get a list of covering or container permissions
   -- 	           

	create table #tmpcoveringpermission (permission nvarchar(128))

	insert into #tmpcoveringpermission (permission) select distinct permissionname from coveringpermissionhierarchy where UPPER(parentpermissionlevel) = UPPER(@parentpermissionlevel) and UPPER(parentcoveringpermission) = UPPER(@parentcoveringpermission) and UPPER(permissionlevel) = UPPER(@permissionlevel) union select distinct permissionname from coveringpermissionhierarchy where UPPER(parentpermissionlevel) = UPPER(@parentpermissionlevel) and UPPER(permissionlevel) = UPPER(@permissionlevel) and UPPER(coveringpermissionname) in (select distinct UPPER(permissionname) from coveringpermissionhierarchy where UPPER(parentpermissionlevel) = UPPER(@parentpermissionlevel) and UPPER(parentcoveringpermission) = UPPER(@parentcoveringpermission) and UPPER(permissionlevel) = UPPER(@permissionlevel))

	delete from #tmpcoveringpermission where UPPER(permission) in ('RECEIVE', 
'ALTER ANY ASSEMBLY',
'ALTER ANY SCHEMA',
'ALTER ANY ROLE',
'ALTER ANY MESSAGE TYPE',
'ALTER ANY SERVICE',
'ALTER ANY CONTRACT',
'ALTER ANY REMOTE SERVICE BINDING',
'ALTER ANY ROUTE',
'ALTER ANY SYMMETRIC KEY',
'ALTER ANY ASYMMETRIC KEY',
'ALTER ANY FULLTEXT CATALOG',
'ALTER ANY CERTIFICATE',
'ALTER ANY DATABASE EVENT NOTIFICATION',
'ALTER ANY ENDPOINT',
'ALTER ANY DATABASE',
'ALTER ANY EVENT NOTIFICATION')


	exec('select * from #tmpcoveringpermission')
	
	drop table #tmpcoveringpermission

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_getcoveringpermission] TO [SQLSecureView]

GO

