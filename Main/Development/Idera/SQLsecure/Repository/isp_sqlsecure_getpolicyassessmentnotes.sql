SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_getpolicyassessmentnotes]'))
drop procedure [dbo].[isp_sqlsecure_getpolicyassessmentnotes]
GO

CREATE procedure [dbo].[isp_sqlsecure_getpolicyassessmentnotes]
(
	@policyid int, 
	@assessmentid int = null,		-- default to policy settings for backward compatibility
	@metricid int
)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Return a table with all of the servers in a policy and any assessment notes on the server or null values if none

	select @assessmentid = isnull(@assessmentid,[dbo].[getdefaultassessmentid](@policyid))
	create table #tmpservers (registeredserverid int)
	insert #tmpservers
		exec [dbo].[isp_sqlsecure_getpolicymemberlist]
			@policyid = @policyid,
			@assessmentid = @assessmentid

	-- use variables to work around sql 2000 syntax limitations
	declare @assessmentdate datetime, @usebaseline bit
	select @assessmentdate=assessmentdate, @usebaseline=usebaseline 
		from assessment 
		where policyid = @policyid
			and assessmentid = @assessmentid
			and assessmentdate is not null

	select	a.connectionname, b.policyid, b.assessmentid, @metricid, a.snapshotid, isnull(d.isexplained,0) as isexplained, isnull(d.notes,N'') as notes, isnull(c.severitycode,0) as severitycode, e.metricname
	from	serversnapshot a 
				inner join assessment b on a.snapshotid in (select snapshotid from dbo.getsnapshotlist(@assessmentdate, @usebaseline))
				left join policyassessment c on b.policyid = c.policyid and b.assessmentid = c.assessmentid and c.metricid = @metricid and c.snapshotid = a.snapshotid
				left join policyassessmentnotes d on b.policyid = d.policyid and b.assessmentid = d.assessmentid and d.metricid = @metricid and d.snapshotid = a.snapshotid
				inner join metric e on e.metricid = @metricid
	where	a.registeredserverid in (select registeredserverid from #tmpservers) 
			and b.policyid = @policyid
			and b.assessmentid = @assessmentid
			and b.assessmentdate is not null

	drop table #tmpservers


GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_getpolicyassessmentnotes] TO [SQLSecureView]

GO