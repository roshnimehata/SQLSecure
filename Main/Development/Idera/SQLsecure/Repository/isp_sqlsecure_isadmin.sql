SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_isadmin]'))
drop procedure [dbo].[isp_sqlsecure_isadmin]
GO

CREATE procedure [dbo].[isp_sqlsecure_isadmin]
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Check if the user is an admin
   -- 	           

	declare @result nvarchar(24)

	select @result = CASE WHEN IS_SRVROLEMEMBER('sysadmin') = 1 THEN 'admin' ELSE 'no access' END
	
	if (@result = 'admin')
		return 1
	else
		return 0


GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_isadmin] TO [SQLSecureView]

GO
