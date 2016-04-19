SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_addnotificationprovider]'))
drop procedure [dbo].[isp_sqlsecure_addnotificationprovider]
GO


CREATE procedure [dbo].[isp_sqlsecure_addnotificationprovider]
(
	@providername nvarchar(64), 
	@providertype nvarchar(32), 
	@servername nvarchar(256), 
	@port int, 
	@timeout int, 
	@requiresauthentication bit, 
	@username nvarchar(128), 
	@password nvarchar(128), 
	@sendername nvarchar(128), 
	@senderemail nvarchar(128), 
	@notificationproviderid int output
)
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Insert a new notification provider into the notificationprovider table
   -- 	           

	declare @err int, @msg nvarchar(500)

	declare @programname nvarchar(128), @action nvarchar(32), @category nvarchar(32), @success nvarchar(32), @failure nvarchar(32)
	select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)
	select @action=N'Add', @category=N'Notification Provider', @success=N'Success Audit', @failure=N'Failure Audit'

	declare @ans int
	exec @ans = [isp_sqlsecure_isadmin]

	if (@ans = 0)
	begin
		set @msg = N'Error: Insufficient privileges to ' + lower(@action) + ' a ' + lower(@category)
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end

	if exists (select * from [notificationprovider] where UPPER(providername) = UPPER(@providername))
	begin
		set @msg = 'Error: ' + @category + ' "' + @providername + '" already exists' 
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end

	BEGIN TRAN

	insert into [notificationprovider] ( 
			providername, 
			providertype, 
			servername, 
			port, 
			[timeout], 
			requiresauthentication, 
			username, 
			[password], 
			sendername, 
			senderemail
			)
		values ( 
			@providername, 
			@providertype, 
			@servername, 
			@port, 
			@timeout, 
			@requiresauthentication, 
			@username, 
			@password, 
			@sendername, 
			@senderemail
			)

	select @err = @@error

	if @err <> 0
	begin
		set @msg = 'Error: Failed to ' + lower(@action) + ' ' + lower(@category) + ' "' + @providername + '"'
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		ROLLBACK TRAN
		return -1
	end

    set @notificationproviderid = @@IDENTITY


	set @msg = @category + N' "' + @providername + '" with id ' + CONVERT(NVARCHAR, @notificationproviderid)
	exec isp_sqlsecure_addactivitylog @activitytype=@success, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname=null

	COMMIT TRAN

GO
