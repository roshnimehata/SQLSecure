SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_getserverpolicylist]'))
drop procedure [dbo].[isp_sqlsecure_getserverpolicylist]
GO


CREATE procedure [dbo].[isp_sqlsecure_getserverpolicylist] (@registeredserverid int)
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Return a table with the policy name and id of all policies where the selected server is a member

	create table #tmppolicies (policyid int, assessmentid int)
	create table #tmpservers (registeredserverid int)
	declare @policyid int, @assessmentid int, @count int

	-- first add all the explicit defined policies so their lists don't have to be built individually
	insert into #tmppolicies select policyid, assessmentid from policymember where registeredserverid = @registeredserverid and assessmentid = [dbo].[getdefaultassessmentid](policyid)

	-- now get those from dynamic policies
	declare mycursor cursor static for
		select policyid, assessmentid from assessment where assessmentstate=N'S' and isdynamic=1
	open mycursor

	fetch next from mycursor into @policyid, @assessmentid
	while (@@fetch_status = 0)
	begin
		insert #tmpservers
			EXEC [dbo].[isp_sqlsecure_getpolicymemberlist]
				@policyid = @policyid,
				@assessmentid = @assessmentid
		if exists (select * from #tmpservers where registeredserverid = @registeredserverid)
			insert into #tmppolicies values (@policyid, @assessmentid)

		fetch next from mycursor into @policyid, @assessmentid
	end

	close mycursor
	deallocate mycursor

	select a.policyid, a.assessmentid, a.policyname
		from vwpolicy a, #tmppolicies b
		where 
			a.policyid = b.policyid
			and a.assessmentid = b.assessmentid

	drop table #tmppolicies
	drop table #tmpservers

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_getserverpolicylist] TO [SQLSecureView]

GO

