SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_updaterule]'))
drop procedure [dbo].[isp_sqlsecure_updaterule]
GO

CREATE procedure [dbo].[isp_sqlsecure_updaterule] (@filterruleheaderid int, @filterruleid int, @class int, @scope nvarchar(64), @matchstring nvarchar(1000))
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Update rule info.
   -- 	           
	declare @err int
	declare @ans int
	declare @errmsg nvarchar(500)

	declare @programname nvarchar(128)
	select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)
		
	declare @connectionname nvarchar(400)
	select @connectionname = connectionname from filterruleheader where filterruleheaderid = @filterruleheaderid

	exec @ans = [isp_sqlsecure_isadmin]

	if (@ans = 0)
	begin
		set @errmsg = 'Error: Insufficient privileges to update filter rule for SQL Server instance ' + @connectionname
		exec isp_sqlsecure_addactivitylog @activitytype=N'Success Audit', @source=@programname, @eventcode=N'Update', @category=N'Filter Rule', @description=@errmsg, @connectionname = ''
		RAISERROR (@errmsg, 16, 1)
		return -1
	end

	BEGIN TRAN
	
		update filterrule set class = @class, scope = @scope, matchstring = @matchstring where filterruleheaderid = @filterruleheaderid and filterruleid = @filterruleid

		select @err = @@error

		if @err <> 0
		begin
			set @errmsg = 'Error: Failed to update filterrule table with header id ' + CONVERT(nvarchar(64), @filterruleheaderid) + ' and rule id ' + CONVERT(nvarchar(64), @filterruleid)
			RAISERROR (@errmsg, 16, 1)
			ROLLBACK TRAN
			return -1
		end


		declare @str nvarchar(500)
		set @str = N'Updated filter rule for SQL Server instance ' + @connectionname + ', Rule Header Id = ' + CONVERT(nvarchar(64), @filterruleheaderid) + N', Rule Id = ' + CONVERT(nvarchar(64), @filterruleid) + N', Class = ' + CONVERT(nvarchar(32), @class) + N', Scope = ' + @scope + N', MatchString = ' + @matchstring

		exec isp_sqlsecure_addactivitylog @activitytype=N'Success Audit', @source=@programname, @eventcode=N'Update', @category=N'Filter Rule', @description=@str, @connectionname = ''
	
	COMMIT TRAN

GO

