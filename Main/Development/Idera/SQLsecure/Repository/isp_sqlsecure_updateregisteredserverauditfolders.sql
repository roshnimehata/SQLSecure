SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_updateregisteredserverauditfolders]'))
drop procedure [dbo].[isp_sqlsecure_updateregisteredserverauditfolders]
GO

CREATE procedure [dbo].[isp_sqlsecure_updateregisteredserverauditfolders] (@connectionname nvarchar(500), @auditfoldersstring nvarchar(max))
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Update registered server audit folders string.   The audit folders are used by collector to get file system permission informations

	declare @err int
	declare @errmsg nvarchar(500)
	declare @ans int

	declare @programname nvarchar(128)
	select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)

	exec @ans = [isp_sqlsecure_isadmin]

	if (@ans = 0)
	begin
		set @errmsg = 'Error: Insufficient privileges to update credentials for SQL Server instance ' + @connectionname
		exec isp_sqlsecure_addactivitylog @activitytype=N'Failure Audit', @source=@programname, @eventcode=N'Update Audit Folders', @category=N'Server', @description=@errmsg, @connectionname = @connectionname
		RAISERROR (@errmsg, 16, 1)
		return -1
	end

	if not exists (select * from registeredserver where UPPER(connectionname) = UPPER(@connectionname))
	begin
		set @errmsg = 'Error: Invalid connection name - ' + @connectionname
		RAISERROR (@errmsg, 16, 1)
	end
	else
	begin
		BEGIN TRAN

		update registeredserver set auditfoldersstring = @auditfoldersstring where UPPER(connectionname) = UPPER(@connectionname)

		select @err = @@error

		if @err <> 0
		begin
			set @errmsg = 'Error: Failed to update registeredserver table with audit folders information'
			RAISERROR (@errmsg, 16, 1)
			ROLLBACK TRAN
			return -1
		end

		declare @str nvarchar(500)
		set @str = N'Updated audit folders for SQL Server instance ' + @connectionname

		exec isp_sqlsecure_addactivitylog @activitytype=N'Success Audit', @source=@programname, @eventcode=N'Update Audit Folders', @category=N'Server', @description=@str, @connectionname = @connectionname
		
		COMMIT TRAN
	
	end
GO

