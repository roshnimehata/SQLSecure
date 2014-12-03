SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getserverroleprincipalid]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getserverroleprincipalid]
GO

CREATE  function [dbo].[getserverroleprincipalid] (@rolename nvarchar(128)) returns int
begin
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the principalid associated with a role.
	if (UPPER(@rolename) = 'SA')
	 	return 1
	else if (UPPER(@rolename) = 'PUBLIC')
 		return 2
	else if (UPPER(@rolename) = 'SYSADMIN')
 		return 3
	else if (UPPER(@rolename) = 'SECURITYADMIN')
 		return 4
	else if (UPPER(@rolename) = 'SERVERADMIN')
 		return 5
	else if (UPPER(@rolename) = 'SETUPADMIN')
 		return 6
	else if (UPPER(@rolename) = 'PROCESSADMIN')
 		return 7
	else if (UPPER(@rolename) = 'DISKADMIN')
 		return 8
	else if (UPPER(@rolename) = 'DBCREATORADMIN')
 		return 11
	else if (UPPER(@rolename) = 'BULKADMIN')
 		return 10
	else if (UPPER(@rolename) = 'DBCREATOR')
 		return 9

	return -1
end

GO

GRANT EXECUTE ON [dbo].[getserverroleprincipalid] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

