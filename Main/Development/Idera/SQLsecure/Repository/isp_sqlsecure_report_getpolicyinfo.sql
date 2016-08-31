SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_report_getpolicyinfo]'))
drop procedure [dbo].[isp_sqlsecure_report_getpolicyinfo]
GO

CREATE procedure [dbo].[isp_sqlsecure_report_getpolicyinfo]
(
	@policyid int,
	@assessmentid int
)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Gets all the policy information

SELECT * 
FROM vwpolicy
WHERE policyid = @policyid
	and assessmentid = @assessmentid
	
GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_report_getpolicyinfo] TO [SQLSecureView]

GO

