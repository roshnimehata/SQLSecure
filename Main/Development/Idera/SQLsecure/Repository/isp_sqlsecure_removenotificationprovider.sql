
SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_removenotificationprovider]'))
drop procedure [dbo].[isp_sqlsecure_removenotificationprovider]
GO

CREATE procedure [dbo].[isp_sqlsecure_removenotificationprovider]
(
	@notificationproviderid int
)
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Remove all notification provider information for the selected notification provider from repository
   -- 	           

	declare @err int, @msg nvarchar(500)

	declare @programname nvarchar(128), @action nvarchar(32), @category nvarchar(32), @success nvarchar(32), @failure nvarchar(32)
	select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)
	select @action=N'Delete', @category=N'Notification Provider', @success=N'Success Audit', @failure=N'Failure Audit'

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
		set @msg = 'Error: Invalid ' + @category + ' id ' + CONVERT(nvarchar, @notificationproviderid)
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end
	else
	begin
		BEGIN TRAN
		
			delete from registeredservernotification where notificationproviderid = @notificationproviderid
			delete from notificationprovider where notificationproviderid = @notificationproviderid

			select @err = @@error

			if @err <> 0
			begin
				set @msg = 'Error: Failed to ' + lower(@action) + ' ' + lower(@category) + ' "' + @providername + '" with id ' + CONVERT(NVARCHAR, @notificationproviderid)
				exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
				RAISERROR (@msg, 16, 1)
				ROLLBACK TRAN
				return -1
			end

			set @msg = @category + N' "' + @providername + '" with id ' + CONVERT(NVARCHAR, @notificationproviderid)
			exec isp_sqlsecure_addactivitylog @activitytype=@success, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname=null

		COMMIT TRAN
	end
GO



