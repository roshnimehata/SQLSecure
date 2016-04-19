SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_updateruleheader]'))
drop procedure [dbo].[isp_sqlsecure_updateruleheader]
GO

CREATE procedure [dbo].[isp_sqlsecure_updateruleheader] (@filterruleheaderid int, @rulename nvarchar(256), @description nvarchar(80))
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Update rule name. Connection name cannot be changed.
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
		set @errmsg = 'Error: Insufficient privileges to update filter for SQL Server instance ' + @connectionname
		exec isp_sqlsecure_addactivitylog @activitytype=N'Failure Audit', @source=@programname, @eventcode=N'Update', @category=N'Filter', @description=@errmsg, @connectionname = ''
		RAISERROR (@errmsg, 16, 1)
		return -1
	end

	BEGIN TRAN
	
		update filterruleheader set rulename = @rulename, description = @description, lastmodifiedtm = GETUTCDATE() where filterruleheaderid = @filterruleheaderid

		select @err = @@error

		if @err <> 0
		begin
			set @errmsg = 'Error: Failed to update filterruleheader table with header id ' + CONVERT(nvarchar(64), @filterruleheaderid)
			RAISERROR (@errmsg, 16, 1)
			ROLLBACK TRAN
			return -1
		end
		
		declare @str nvarchar(500)
		set @str = N'Update filter rule for SQL Server instance ' + @connectionname + N', Rule Header Id = ' + CONVERT(NVARCHAR(64), @filterruleheaderid) + N' and Filter Ruler Name = ' + @rulename

		exec isp_sqlsecure_addactivitylog @activitytype=N'Success Audit', @source=@programname, @eventcode=N'Update', @category=N'Filter', @description=@str, @connectionname = ''
	
	COMMIT TRAN

GO


