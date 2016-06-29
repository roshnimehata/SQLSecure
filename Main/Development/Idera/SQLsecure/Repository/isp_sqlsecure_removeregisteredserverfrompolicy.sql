
SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_removeregisteredserverfrompolicy]'))
drop procedure [dbo].[isp_sqlsecure_removeregisteredserverfrompolicy]
GO

CREATE procedure [dbo].[isp_sqlsecure_removeregisteredserverfrompolicy] 
(
	@policyid int, 
	@assessmentid int = null,		-- default to policy settings for backward compatibility
	@registeredserverid int
)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Remove a registered server from the selected policy
   -- 	           

	declare @err int, @msg nvarchar(500)

	declare @programname nvarchar(128), @action nvarchar(32), @category nvarchar(32), @category2 nvarchar(32), @success nvarchar(32), @failure nvarchar(32)
	select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)
	select @action=N'Delete', @category=N'Policy Member', @category2=N'Assessment Member', @success=N'Success Audit', @failure=N'Failure Audit'

	declare @ans int
	exec @ans = [isp_sqlsecure_isadmin]

	if (@ans = 0)
	begin
		set @msg = N'Error: Insufficient privileges to ' + lower(@action) + ' a ' + lower(@category)
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end

	declare @policyname nvarchar(128),
			@state nchar(1)
	select @assessmentid = isnull(@assessmentid,[dbo].[getdefaultassessmentid](@policyid))
	select @policyname=dbo.[getassessmentname](@policyid, @assessmentid) 
	select @state=assessmentstate 
		from assessment
		where policyid = @policyid 
			and assessmentid = @assessmentid

	if (@policyname is null)
	begin
		set @msg = 'Error: Invalid policy id ' + CONVERT(nvarchar, @policyid) + ' and assessment id ' + CONVERT(NVARCHAR, @assessmentid)
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end

	if (@state = N'A')
	begin
		set @msg = 'Error: Cannot remove server from assessment "' + @policyname + '" because it is an approved assessment.'
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end

 	declare @connection nvarchar(128)
	select @connection=connectionname from registeredserver where registeredserverid = @registeredserverid

	set @category= case when @state in (N'D', N'P') then @category2 else @category end
	if (@connection is null)
	begin
		set @msg = 'Error: Invalid registered server id ' + CONVERT(nvarchar, @registeredserverid)
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end

	BEGIN TRAN
	
		delete from policymember 
			where policyid = @policyid 
				and assessmentid = @assessmentid
				and registeredserverid = @registeredserverid

		select @err = @@error

		if @err <> 0
		begin
			set @msg = 'Error: Failed to ' + lower(@action) + ' ' + lower(@category) + ' "' + @connection + '" from "' + @policyname + '" with id ' + CONVERT(nvarchar, @policyid) + ' and assessment id ' + CONVERT(NVARCHAR, @assessmentid)
			exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
			RAISERROR (@msg, 16, 1)
			ROLLBACK TRAN
			return -1
		end

		set @msg = N'Removed server ' + @connection
		set @state=dbo.getassessmentstatename(@state)
		exec isp_sqlsecure_addpolicychangelog @policyid=@policyid, @assessmentid=@assessmentid, @state=@state, @description=@msg

		set @msg = @category + N' "' + @connection + '" from '  + @policyname
		exec isp_sqlsecure_addactivitylog @activitytype=@success, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname=null

	COMMIT TRAN

GO



