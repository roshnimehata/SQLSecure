SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_removerule]'))
drop procedure [dbo].[isp_sqlsecure_removerule]
GO

CREATE procedure [dbo].[isp_sqlsecure_removerule] (@filterruleheaderid int)
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Remove all the rules pertaining to this header id
   -- 	           
	declare @err int
	declare @errmsg nvarchar(500)
	declare @ans int
	
	declare @connectionname nvarchar(400)
	select @connectionname = connectionname from filterruleheader where filterruleheaderid = @filterruleheaderid

	declare @programname nvarchar(128)
	select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)

	exec @ans = [isp_sqlsecure_isadmin]

	if (@ans = 0)
	begin
		set @errmsg = 'Error: Insufficient privileges to remove filter rule for SQL Server instance ' + @connectionname
		exec isp_sqlsecure_addactivitylog @activitytype=N'Success Audit', @source=@programname, @eventcode=N'Remove', @category=N'Filter Rule', @description=@errmsg, @connectionname = @connectionname
		RAISERROR (@errmsg, 16, 1)
		return -1
	end

	BEGIN TRAN
	
		delete from filterrule where filterruleheaderid = @filterruleheaderid

		select @err = @@error

		if @err <> 0
		begin
			set @errmsg = 'Error: Failed to delete from filterrule table with header id ' + CONVERT(nvarchar(64), @filterruleheaderid)
			RAISERROR (@errmsg, 16, 1)
			ROLLBACK TRAN
			return -1
		end

		declare @str nvarchar(500)
		set @str = N'Removed filter rules for SQL Server instance ' + @connectionname + N', with Rule Header Id =  ' + CONVERT(nvarchar(64), @filterruleheaderid)
		
		exec isp_sqlsecure_addactivitylog @activitytype=N'Success Audit', @source=@programname, @eventcode=N'Remove', @category=N'Filter Rule', @description=@str, @connectionname = @connectionname
	
	COMMIT TRAN

GO

