SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getserverrolesid]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getserverrolesid]
GO


CREATE  function [dbo].[getserverrolesid] (@rolename nvarchar(128)) returns varbinary
begin
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the sid associated with a role.
	if (UPPER(@rolename) = 'SYSADMIN')
 		return 0x3
	else if (UPPER(@rolename) = 'SECURITYADMIN')
 		return 0x4
	else if (UPPER(@rolename) = 'SERVERADMIN')
 		return 0x5
	else if (UPPER(@rolename) = 'SETUPADMIN')
 		return 0x6
	else if (UPPER(@rolename) = 'PROCESSADMIN')
 		return 0x7
	else if (UPPER(@rolename) = 'DISKADMIN')
 		return 0x8
	else if (UPPER(@rolename) = 'DBCREATORADMIN')
 		return 0xB
	else if (UPPER(@rolename) = 'DBCREATOR')
 		return 0x9
	else if (UPPER(@rolename) = 'BULKADMIN')
 		return 0xA

	return 0x19
end

GO

GRANT EXECUTE ON [dbo].[getserverrolesid] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

