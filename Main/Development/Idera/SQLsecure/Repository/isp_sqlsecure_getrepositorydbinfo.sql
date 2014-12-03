SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_getrepositorydbinfo]'))
drop procedure [dbo].[isp_sqlsecure_getrepositorydbinfo]
GO

CREATE procedure [dbo].[isp_sqlsecure_getrepositorydbinfo] 
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --             Return db info for SQLsecure database.
   -- 	           

	EXEC sp_helpdb 'SQLsecure'

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_getrepositorydbinfo] TO [SQLSecureView]

GO


