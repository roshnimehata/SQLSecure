SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getcoveringpermission]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getcoveringpermission]
GO

CREATE FUNCTION [dbo].[getcoveringpermission] (@parentpermissionlevel nvarchar(128), @parentcoveringpermission nvarchar(128), @permissionlevel nvarchar(128))
RETURNS  TABLE  
AS 
RETURN (  
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --             Get a list of covering or container permissions
   -- 	           
	select distinct permissionname 
	from coveringpermissionhierarchy 
	where 	( UPPER(permissionname) not in 
		      ( 'RECEIVE', 
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
			'ALTER ANY EVENT NOTIFICATION'
		       )
		)
 	and 
	( 
		( UPPER(parentpermissionlevel) = UPPER(@parentpermissionlevel) 
			and UPPER(parentcoveringpermission) = UPPER(@parentcoveringpermission) 
			and UPPER(permissionlevel) = UPPER(@permissionlevel) 
		) 
		or 
		( UPPER(parentpermissionlevel) = UPPER(@parentpermissionlevel) 
			and UPPER(permissionlevel) = UPPER(@permissionlevel) 
			and UPPER(coveringpermissionname) in (
				select distinct UPPER(permissionname) 
				from coveringpermissionhierarchy 
				where UPPER(parentpermissionlevel) = UPPER(@parentpermissionlevel) 
				and UPPER(parentcoveringpermission) = UPPER(@parentcoveringpermission) 
				and UPPER(permissionlevel) = UPPER(@permissionlevel)
			)

		)
	)   
	
) 

GO

GRANT SELECT ON [dbo].[getcoveringpermission] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

