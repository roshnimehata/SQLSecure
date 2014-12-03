SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_addrule]'))
drop procedure [dbo].[isp_sqlsecure_addrule]
GO

CREATE procedure [dbo].[isp_sqlsecure_addrule] (@ruleheaderid int, @class tinyint, @scope nvarchar(64), @matchstring nvarchar(1000))
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Insert a single rule to the filterrue table
   -- 	           

	declare @result int
	declare @err int
	declare @errmsg nvarchar(500)
	declare @ans int

	-- Get application program name
	declare @programname nvarchar(128)
	select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)
	
	-- Get connection name for the filterheaderid
	declare @connectionname nvarchar(400)
	select @connectionname = connectionname from filterruleheader where filterruleheaderid = @ruleheaderid
	
	exec @ans = [isp_sqlsecure_isadmin]

	if (@ans = 0)
	begin
		set @errmsg = 'Error: Insufficient privileges to add filter rule for SQL Server instance ' + @connectionname
		exec isp_sqlsecure_addactivitylog @activitytype=N'Failure Audit', @source=@programname, @eventcode=N'Add', @category=N'Filter Rule', @description=@errmsg, @connectionname = @connectionname
		RAISERROR (@errmsg, 16, 1)
		return -1
	end
	 
	BEGIN TRAN
	
		insert into filterrule (filterruleheaderid, class, scope, matchstring) values (@ruleheaderid, @class, @scope, @matchstring)

		select @err = @@error

		if @err <> 0
		begin
			set @errmsg = 'Error: Failed to insert into filterrule table with filterruleheaderid ' + CONVERT(nvarchar(64), @ruleheaderid)
			RAISERROR (@errmsg, 16, 1)
			ROLLBACK TRAN
			return -1
		end

		if object_id('#tmpruleheader') is null 
		begin
			create table #tmprule (data int)
		end

		delete from #tmprule

		insert into #tmprule (data) (select filterruleid from filterrule where filterruleheaderid = @ruleheaderid and class = @class and scope = @scope and matchstring = @matchstring)

		select @err = @@error

		if @err <> 0
		begin
			set @errmsg = 'Error: Failed to insert into filterrule table with filterruleheaderid ' + CONVERT(nvarchar(64), @ruleheaderid)
			RAISERROR (@errmsg, 16, 1)
			ROLLBACK TRAN
			return -1
		end

		declare @str nvarchar(500)
		set @str = N'Added filter rule for SQL Server instance ' + @connectionname + N', Scope: ' + @scope + N'Match String: ' + @matchstring + N' (ruleheaderid=' + CONVERT(nvarchar(64), @ruleheaderid) + ')'

		exec isp_sqlsecure_addactivitylog @activitytype=N'Success Audit', @source=@programname, @eventcode=N'Add', @category=N'Filter Rule', @description=@str, @connectionname = @connectionname

	COMMIT TRAN

	exec('select * from #tmprule')
	


GO


