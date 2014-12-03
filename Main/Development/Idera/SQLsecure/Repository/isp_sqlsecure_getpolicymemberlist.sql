SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_getpolicymemberlist]'))
drop procedure [dbo].[isp_sqlsecure_getpolicymemberlist]
GO


CREATE procedure [dbo].[isp_sqlsecure_getpolicymemberlist]
(
	@policyid int,
	@assessmentid int = null		-- default to policy settings for backward compatibility
)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Return a table with a list of all policy members of a group whether explicit or dynamic
   --			   Note this works like a function, but cannot be a function because of the constructed select for dynamic groups
   -- 	           
   -- Usages:
   --		create table #servers (registeredserverid int)
   --		insert #servers
   --			EXEC	@return_value = [dbo].[isp_sqlsecure_getpolicymemberlist]
   --				@policyid = @policyid
   --				@assessmentid = @assessmentid
   --		...
   --		select * from registeredserver where registeredserverid in (select registeredserverid from #servers)
   --		drop table #servers
   -- 	           
   -- 	    or, to prevent the insert exec limitation, it can be called as follows and will fill this temporary table if it exists     
   -- 	           
   --		create table #tmp_sqlsecure_getpolicymemberlist (registeredserverid int)
   --		EXEC	@return_value = [dbo].[isp_sqlsecure_getpolicymemberlist]
   --			@policyid = @policyid
   --			@assessmentid = @assessmentid
   --		...
   --		select * from registeredserver where registeredserverid in (select registeredserverid from #tmp_sqlsecure_getpolicymemberlist)
   --		drop table #tmp_sqlsecure_getpolicymemberlist

	declare @isdynamic bit, 
		@str nvarchar(4000) 
	declare @memberstable table (registeredserverid int)

	select @assessmentid = isnull(@assessmentid,[dbo].[getdefaultassessmentid](@policyid))

	select @isdynamic=isdynamic, @str=rtrim(isnull(dynamicselection,N''))
		from assessment 
		where policyid = @policyid 
			and assessmentid = @assessmentid

	if (@isdynamic = 1)
	begin
		if (len(@str) = 0)
		begin
			insert into @memberstable
				select registeredserverid
					from registeredserver
					order by connectionname
		end
		else
		begin
			create table #memberstable (registeredserverid int)
			select @str = 'insert into #memberstable' + 
							' select registeredserverid' + 
							'	from registeredserver' + 
							'	where (' + @str + ')' + 
							'	order by connectionname;'
			exec (@str)
			if (object_id('tempdb..#tmp_sqlsecure_getpolicymemberlist') is not null)
			begin
				insert into #tmp_sqlsecure_getpolicymemberlist
					select registeredserverid from #memberstable
			end
			else
				select * from #memberstable

			drop table #memberstable

			return
		end
	end
	else
	begin
		insert into @memberstable
			select m.registeredserverid
				from policymember m inner join registeredserver s on m.registeredserverid = s.registeredserverid
				where policyid = @policyid
					and assessmentid = @assessmentid
				order by s.connectionname
	end

	if (object_id('tempdb..#tmp_sqlsecure_getpolicymemberlist') is not null)
	begin
		insert into #tmp_sqlsecure_getpolicymemberlist
			select registeredserverid from @memberstable
	end
	else
		select * from @memberstable

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_getpolicymemberlist] TO [SQLSecureView]

GO