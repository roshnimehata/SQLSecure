SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_getweakpasswordlist]'))
drop procedure [dbo].[isp_sqlsecure_getweakpasswordlist]
GO

CREATE procedure [dbo].[isp_sqlsecure_getweakpasswordlist] (@passwordListId int = null)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Update policy with new info
   --			   Note system policies cannot be updated and a policy cannot be changed to or from type system

SELECT	passwordlistid,
		custompasswordlist,
		customlistupdated,
		additionalpasswordlist,
		additionallistupdated,
		isweakpassworddetectionenabled as 'passwordCheckingEnabled'
FROM	weakwordlist 
RIGHT OUTER JOIN configuration ON 1=1
				
GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_getweakpasswordlist] TO [SQLSecureView]

GO