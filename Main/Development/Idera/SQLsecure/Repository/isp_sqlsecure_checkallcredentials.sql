SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_checkallcredentials]'))
drop procedure [dbo].[isp_sqlsecure_checkallcredentials]
GO

CREATE procedure [dbo].[isp_sqlsecure_checkallcredentials] 
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Checks if all registeredservers have windows credentials entered.
   --				If not, then this is an upgrade to 2.0
   --				Returns a Y or N
   -- 	    

	declare @result nchar(1)
	select @result = case when count(*) = 0 then 'Y' else 'N' end
		from registeredserver
		where sqlserverauthtype <> 'S' and len(sqlserverlogin) = 0

	select @result

	
GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_checkallcredentials] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
