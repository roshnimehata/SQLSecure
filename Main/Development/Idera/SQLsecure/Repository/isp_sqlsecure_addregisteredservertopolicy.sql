SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_addregisteredservertopolicy]'))
drop procedure [dbo].[isp_sqlsecure_addregisteredservertopolicy]
GO


CREATE procedure [dbo].[isp_sqlsecure_addregisteredservertopolicy] 
(
	@registeredserverid int, 
	@policyid int,
	@assessmentid int = null		-- default to policy settings for backward compatibility
)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Add a registered server as a member of a policy 
   -- 	           

	declare @err int, @msg nvarchar(500)

	declare @programname nvarchar(128), @action nvarchar(32), @category nvarchar(32), @category2 nvarchar(32), @success nvarchar(32), @failure nvarchar(32)
	select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)
	select @action=N'Add', @category=N'Policy Member', @category2=N'Assessment Member', @success=N'Success Audit', @failure=N'Failure Audit'

	declare @ans int
	exec @ans = [isp_sqlsecure_isadmin]

	if (@ans = 0)
	begin
		set @msg = N'Error: Insufficient privileges to ' + lower(@action) + ' a ' + lower(@category)
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end

	declare @connection nvarchar(400)
	select @connection=connectionname from [registeredserver] where registeredserverid = @registeredserverid

	if (@connection is null)
	begin
		set @msg = 'Error: Invalid registered server id ' + CONVERT(nvarchar, @registeredserverid)
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end

	declare @policy nvarchar(128),
			@state nchar(1)
	select @assessmentid = isnull(@assessmentid,[dbo].[getdefaultassessmentid](@policyid))
	select @policy = [dbo].[getassessmentname](@policyid, @assessmentid)
	select @state=assessmentstate 
		from assessment
		where policyid = @policyid 
			and assessmentid = @assessmentid

	set @category= case when @state in (N'D', N'P') then @category2 else @category end

	if (@policy is null)
	begin
		set @msg = N'Error: Registered Server "' + @connection + '" cannot be added. Invalid policy id ' + CONVERT(nvarchar, @policyid) + ' and assessment id ' + CONVERT(NVARCHAR, @assessmentid)
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end

	if exists (select * from [assessment] where policyid = @policyid and assessmentid = @assessmentid and isdynamic=1)
	begin
		set @msg = N'Error: Registered Server "' + @connection + '" cannot be added to "' + @policy + '" because the policy members are created dynamically'
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end

	if exists (select * from [policymember] where policyid = @policyid and assessmentid = @assessmentid and registeredserverid = @registeredserverid)
	begin
--		set @msg = 'Error: ' + @category + ' "' + @connection + '" already exists for "'  + @policy + '"'
--		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
--		RAISERROR (@msg, 16, 1)
--		return -1
		return	-- it already exists, which is the correct result so just return without any logging or messaging
	end

	BEGIN TRAN

		insert into [policymember] (policyid, assessmentid, registeredserverid)
			 values (@policyid, @assessmentid, @registeredserverid)

		select @err = @@error

		if @err <> 0
		begin
			set @msg = 'Error: Failed to ' + lower(@action) + ' ' + lower(@category) + ' "' + @connection + '" to "'  + @policy + '"'
			exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
			RAISERROR (@msg, 16, 1)
			ROLLBACK TRAN
			return -1
		end

		set @msg = N'Added server ' + @connection
		set @state=dbo.getassessmentstatename(@state)
		exec isp_sqlsecure_addpolicychangelog @policyid=@policyid, @assessmentid=@assessmentid, @state=@state, @description=@msg

		set @msg = @category + N' "' + @connection + '" to '  + @policy
		exec isp_sqlsecure_addactivitylog @activitytype=@success, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname=null

	COMMIT TRAN

GO
