SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_addpolicy]'))
drop procedure [dbo].[isp_sqlsecure_addpolicy]
GO


CREATE procedure [dbo].[isp_sqlsecure_addpolicy] 
(
	@policyname nvarchar(128), 
	@policydescription nvarchar(2048), 
	@issystempolicy bit, 
	@isdynamic bit, 
	@dynamicselection nvarchar(4000), 
	@interviewname nvarchar(256) = null,
	@interviewtext ntext = null,
	@policyid int output
)
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Insert a new policy into the policy table
   -- 	           

	declare @err int, @msg nvarchar(500)
	declare @assessmentid int

	declare @programname nvarchar(128), @action nvarchar(32), @category nvarchar(32), @success nvarchar(32), @failure nvarchar(32)
	select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)
	select @action=N'Add', @category=N'Policy', @success=N'Success Audit', @failure=N'Failure Audit'

	declare @ans int
	exec @ans = [isp_sqlsecure_isadmin]

	if (@ans = 0)
	begin
		set @msg = N'Error: Insufficient privileges to ' + lower(@action) + ' a ' + lower(@category)
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end

	if exists (select * from [policy] where UPPER(policyname) = UPPER(@policyname))
	begin
		set @msg = 'Error: ' + @category + ' "' + @policyname + '" already exists' 
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end

	BEGIN TRAN

	insert into [policy] (
			policyname, 
			policydescription, 
			issystempolicy)
		 values (
			@policyname, 
			@policydescription, 
			@issystempolicy)

	select @err = @@error

	if @err <> 0
	begin
		set @msg = 'Error: Failed to ' + lower(@action) + ' ' + lower(@category) + ' "' + @policyname + '"'
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		ROLLBACK TRAN
		return -1
	end

    set @policyid = @@IDENTITY

	-- create the default assessment
	insert into [assessment] (
			[policyid], 
			[assessmentstate], 
			[assessmentname], 
			[assessmentdescription], 
			[assessmentnotes], 
			[assessmentdate], 
			[usebaseline], 
			[isdynamic], 
			[dynamicselection])
		 values (
			@policyid, 
			N'S', 
			N'', 
			@policydescription, 
			N'', 
			null, 
			0,
			@isdynamic, 
			@dynamicselection)

	select @err = @@error

	if @err <> 0
	begin
		set @msg = 'Error: Failed to ' + lower(@action) + ' ' + lower(@category) + ' "' + @policyname + '" Assessment'
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		ROLLBACK TRAN
		return -1
	end

    set @assessmentid = @@IDENTITY

	-- save the interview if there is one
	if (LEN(RTRIM(ISNULL(@interviewname, '') + ISNULL(CONVERT(NVARCHAR(100), @interviewtext), ''))) > 0)
	begin
		insert into policyinterview (
				policyid,
				assessmentid,
				istemplate,
				interviewname,
				interviewtext
				)
			values (
				@policyid,
				@assessmentid,
				0,
				@interviewname,
				@interviewtext
				)

		select @err = @@error

		if @err <> 0
		begin
			set @msg = 'Error: Failed to ' + lower(@action) + ' ' + lower(@category) + ' "' + @policyname + '" Interview'
			exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
			RAISERROR (@msg, 16, 1)
			ROLLBACK TRAN
			return -1
		end
	end

	-- now copy all of the default metrics to the new policy
	insert into policymetric (policyid,
								assessmentid,
								metricid,
								isenabled,
								reportkey,
								reporttext,
								severity,
								severityvalues)
		select @policyid,
				@assessmentid,
				metricid,
				isenabled,
				reportkey,
				reporttext,
				severity,
				severityvalues
			from policymetric
			where policyid = 0

	select @err = @@error

	if @err <> 0
	begin
		set @msg = 'Error: Failed to ' + lower(@action) + ' ' + lower(@category) + ' "' + @policyname + '". The default metric settings could not be saved. '
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		ROLLBACK TRAN
		return -1
	end


	set @msg = @category + N' "' + @policyname + '" with policy id ' + CONVERT(NVARCHAR, @policyid)
	exec isp_sqlsecure_addactivitylog @activitytype=@success, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname=null

	COMMIT TRAN

GO
