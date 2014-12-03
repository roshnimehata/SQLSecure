SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_updatenotificationprovider]'))
drop procedure [dbo].[isp_sqlsecure_updatenotificationprovider]
GO

CREATE procedure [dbo].[isp_sqlsecure_updatenotificationprovider]
(
	@notificationproviderid int, 
	@providername nvarchar(64), 
	@providertype nvarchar(32), 
	@servername nvarchar(256), 
	@port int, 
	@timeout int, 
	@requiresauthentication bit, 
	@username nvarchar(128), 
	@password nvarchar(128), 
	@sendername nvarchar(128), 
	@senderemail nvarchar(128) 
)
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Update Notification Provider with new info

	declare @err int, @msg nvarchar(500)

	declare @programname nvarchar(128), @action nvarchar(32), @category nvarchar(32), @success nvarchar(32), @failure nvarchar(32)
	select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)
	select @action=N'Update', @category=N'Notification Provider', @success=N'Success Audit', @failure=N'Failure Audit'

	declare @ans int
	exec @ans = [isp_sqlsecure_isadmin]

	if (@ans = 0)
	begin
		set @msg = N'Error: Insufficient privileges to ' + lower(@action) + ' a ' + lower(@category)
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end

	declare @name nvarchar(128)
	select @name=providername from notificationprovider where notificationproviderid = @notificationproviderid

	if (@name is null)
	begin
		set @msg = 'Error: Invalid ' + @category + ' id ' + CONVERT(nvarchar, @notificationproviderid)
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end
	else if exists(select * from notificationprovider where notificationproviderid <> @notificationproviderid AND UPPER(providername) = UPPER(@providername))
	begin
		set @msg = 'Error: Cannot rename ' + lower(@category) + ' "' + @name + '" to "' + @providername + '". Name already exists'
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end
	else
	begin
		BEGIN TRAN

		update notificationprovider
		set 
			providername=@providername, 
			providertype=@providertype, 
			servername=@servername, 
			port=@port,
			[timeout]=@timeout, 
			requiresauthentication=@requiresauthentication, 
			username=@username, 
			[password]=@password, 
			sendername=@sendername, 
			senderemail=@senderemail 
		where notificationproviderid = @notificationproviderid

		select @err = @@error

		if @err <> 0
		begin
			set @msg = 'Error: Failed to ' + lower(@action) + ' ' + lower(@category) + ' "' + @name + '" with id ' + CONVERT(NVARCHAR, @notificationproviderid)
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

