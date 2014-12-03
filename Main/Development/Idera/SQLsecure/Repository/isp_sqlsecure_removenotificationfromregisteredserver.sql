
SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_removenotificationfromregisteredserver]'))
drop procedure [dbo].[isp_sqlsecure_removenotificationfromregisteredserver]
GO

CREATE procedure [dbo].[isp_sqlsecure_removenotificationfromregisteredserver]
(
	@registeredserverid int, 
	@notificationproviderid int 
)
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Remove a notification configuration from the selected registered server
   -- 	           

	declare @err int, @msg nvarchar(500)

	declare @programname nvarchar(128), @action nvarchar(32), @category nvarchar(32), @success nvarchar(32), @failure nvarchar(32)
	select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)
	select @action=N'Delete', @category=N'Server Notification', @success=N'Success Audit', @failure=N'Failure Audit'

	declare @ans int
	exec @ans = [isp_sqlsecure_isadmin]

	if (@ans = 0)
	begin
		set @msg = N'Error: Insufficient privileges to ' + lower(@action) + ' a ' + lower(@category)
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end

	declare @providername nvarchar(128)
	select @providername=providername from notificationprovider where notificationproviderid = @notificationproviderid

	if (@providername is null)
	begin
		set @msg = 'Error: Invalid notification provider id ' + CONVERT(nvarchar, @notificationproviderid)
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end

 	declare @connection nvarchar(128)
	select @connection=connectionname from registeredserver where registeredserverid = @registeredserverid

	if (@connection is null)
	begin
		set @msg = 'Error: Invalid registered server id ' + CONVERT(nvarchar, @registeredserverid)
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end

	BEGIN TRAN

		delete from [registeredservernotification] where notificationproviderid = @notificationproviderid and registeredserverid = @registeredserverid

		select @err = @@error

		if @err <> 0
		begin
			set @msg = 'Error: Failed to ' + lower(@action) + ' ' + lower(@category) + ' "' + @providername + '" from server "' + @connection + '"'
			exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
			RAISERROR (@msg, 16, 1)
			ROLLBACK TRAN
			return -1
		end

		set @msg = @category + N' "' + @providername + '" from server "' + @connection + '"'
		exec isp_sqlsecure_addactivitylog @activitytype=@success, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname=null

	COMMIT TRAN

GO



