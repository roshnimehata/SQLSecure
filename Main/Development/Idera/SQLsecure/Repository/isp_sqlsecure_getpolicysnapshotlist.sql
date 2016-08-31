SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_getpolicysnapshotlist]'))
drop procedure [dbo].[isp_sqlsecure_getpolicysnapshotlist]
GO


CREATE procedure [dbo].[isp_sqlsecure_getpolicysnapshotlist]
(
	@policyid int,
	@assessmentid int = null,		-- default to policy settings for backward compatibility
	@usebaseline bit=0,
	@rundate datetime=null
)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Return a table with a list of all snapshots used by the policy with the given run date and baseline options
   -- 	           

	declare @err int

	-- get the list of servers for the selected policy
	select @assessmentid = isnull(@assessmentid,[dbo].[getdefaultassessmentid](@policyid))

	create table #tmp_sqlsecure_getpolicymemberlist (registeredserverid int)

	EXEC @err = [dbo].[isp_sqlsecure_getpolicymemberlist]
		@policyid = @policyid, 
		@assessmentid = @assessmentid

	-- return the snapshot info
	select a.snapshotid, a.registeredserverid, a.connectionname, a.starttime, a.status, a.baseline,
			a.version
	from serversnapshot a,
		dbo.getsnapshotlist(@rundate, @usebaseline) b
	where a.registeredserverid in (select registeredserverid from #tmp_sqlsecure_getpolicymemberlist)
			and a.snapshotid = b.snapshotid

	drop table #tmp_sqlsecure_getpolicymemberlist

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_getpolicysnapshotlist] TO [SQLSecureView]

GO