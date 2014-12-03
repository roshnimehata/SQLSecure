SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_getpolicyserverlist]'))
drop procedure [dbo].[isp_sqlsecure_getpolicyserverlist]
GO


CREATE procedure [dbo].[isp_sqlsecure_getpolicyserverlist]
(
	@policyid int, 
	@assessmentid int = null		-- default to policy settings for backward compatibility
)
AS

   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Return a table with the server name and id of all policy members

	select @assessmentid = isnull(@assessmentid,[dbo].[getdefaultassessmentid](@policyid))
	create table #tmpservers (registeredserverid int)
	insert #tmpservers
		EXEC [dbo].[isp_sqlsecure_getpolicymemberlist]
			@policyid = @policyid,
			@assessmentid = @assessmentid

	select registeredserverid, connectionname from vwregisteredserver
		where registeredserverid in (select registeredserverid from #tmpservers)

	drop table #tmpservers

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_getpolicyserverlist] TO [SQLSecureView]

GO

