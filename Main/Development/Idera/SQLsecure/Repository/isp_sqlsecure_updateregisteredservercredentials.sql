SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_updateregisteredservercredentials]'))
drop procedure [dbo].[isp_sqlsecure_updateregisteredservercredentials]
GO

CREATE procedure [dbo].[isp_sqlsecure_updateregisteredservercredentials] (@connectionname nvarchar(500), @loginname nvarchar(128), @loginpassword nvarchar(300), @authmode nvarchar(256), @serverlogin nvarchar(256), @serverpassword nvarchar(256))
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Update registered server credentials.   The credentials are used to connect to a SQL Server and also to get group memberships.
   -- 	           This information might change from time to time so there is a need to update the master registered server record

	declare @err int
	declare @errmsg nvarchar(500)
	declare @ans int

	declare @programname nvarchar(128)
	select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)

	exec @ans = [isp_sqlsecure_isadmin]

	if (@ans = 0)
	begin
		set @errmsg = 'Error: Insufficient privileges to update credentials for SQL Server instance ' + @connectionname
		exec isp_sqlsecure_addactivitylog @activitytype=N'Failure Audit', @source=@programname, @eventcode=N'Update Credentials', @category=N'Server', @description=@errmsg, @connectionname = @connectionname
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

		update registeredserver set sqlserverlogin = @loginname, sqlserverpassword = @loginpassword, sqlserverauthtype = @authmode, serverlogin = @serverlogin, serverpassword = @serverpassword where UPPER(connectionname) = UPPER(@connectionname)

		select @err = @@error

		if @err <> 0
		begin
			set @errmsg = 'Error: Failed to update registeredserver table with credentials information'
			RAISERROR (@errmsg, 16, 1)
			ROLLBACK TRAN
			return -1
		end

		declare @str nvarchar(500)
		set @str = N'Updated credentials for SQL Server instance ' + @connectionname

		exec isp_sqlsecure_addactivitylog @activitytype=N'Success Audit', @source=@programname, @eventcode=N'Update Credentials', @category=N'Server', @description=@str, @connectionname = @connectionname
		
		COMMIT TRAN
	
	end
GO

