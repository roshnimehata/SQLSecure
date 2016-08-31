SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_updatepolicymetric]'))
drop procedure [dbo].[isp_sqlsecure_updatepolicymetric]
GO

CREATE procedure [dbo].[isp_sqlsecure_updatepolicymetric]
(
	@policyid int,
	@assessmentid int = null,		-- default to policy settings for backward compatibility
	@metricid int,
	@isenabled bit,
	@reportkey nvarchar(32),
	@reporttext nvarchar(4000),
	@severity int,
	@severityvalues nvarchar(4000)
)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Update policymetric with new metric configuration

	declare @err int, @msg nvarchar(500)

	declare @programname nvarchar(128), @action nvarchar(32), @category nvarchar(32), @category2 nvarchar(32), @success nvarchar(32), @failure nvarchar(32)
	select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)
	select @action=N'Update', @category=N'Security Check', @category2=N'Policy', @success=N'Success Audit', @failure=N'Failure Audit'

	declare @ans int
	exec @ans = [isp_sqlsecure_isadmin]

	if (@ans = 0)
	begin
		set @msg = N'Error: Insufficient privileges to ' + lower(@action) + ' a ' + lower(@category)
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end

	declare @policyname nvarchar(128)
	select @assessmentid = isnull(@assessmentid,[dbo].[getdefaultassessmentid](@policyid))
	select @policyname=[dbo].[getassessmentname](@policyid,@assessmentid)
	if (@policyname is null)
	begin
		set @msg = 'Error: Invalid policy id ' + CONVERT(nvarchar, @policyid) + ' and assessment id ' + CONVERT(NVARCHAR, @assessmentid)
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end

	select @category2 = case when assessmentstate in (N'D', N'P', N'A') then 'Assessment' else 'Policy' end
		from assessment
		where policyid = @policyid
			and assessmentid = @assessmentid

	declare @metricname nvarchar(256)
	select @metricname=metricname from metric where metricid = @metricid and metricid > 0
	if (@metricname is null)
	begin
		set @msg = 'Error: Invalid metric id ' + CONVERT(nvarchar, @metricid)
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end

	BEGIN TRAN

	-- see if any values have changed for logging
	declare @oldisenabled bit,
			@oldreportkey nvarchar(32),
			@oldreporttext nvarchar(4000),
			@oldseverity int,
			@oldseverityvalues nvarchar(4000),
			@assessmentstate nvarchar(20) 
	select @oldisenabled = a.isenabled, 
			@oldreportkey = a.reportkey, 
			@oldreporttext = a.reporttext, 
			@oldseverity = a.severity, 
			@oldseverityvalues = a.severityvalues, 
			@assessmentstate = b.assessmentstate
		from [policymetric] a, assessment b 
		where a.policyid = @policyid 
			and a.assessmentid = @assessmentid 
			and a.metricid = @metricid 
			and b.policyid = @policyid 
			and b.assessmentid = @assessmentid 

	select @err = @@error

	if (@err <> 0 or @assessmentstate is null)
	begin
		ROLLBACK TRAN
		set @msg = 'Error: Failed to ' + lower(@action) + ' ' + lower(@category) + ' with policy id ' + CONVERT(nvarchar, @policyid) + ' and assessment id ' + CONVERT(NVARCHAR, @assessmentid) + ' and metric id ' + CONVERT(nvarchar, @metricid) + '. Unable to retrieve policy metric.'
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end

	if (@oldisenabled <> @isenabled
		or @oldreportkey <> @reportkey 
		or @oldreporttext <> @reporttext
		or @oldseverity <> @severity
		or @oldseverityvalues <> @severityvalues)
	begin
		update [policymetric] set 
				isenabled=@isenabled, 
				reportkey=@reportkey, 
				reporttext=@reporttext, 
				severity=@severity, 
				severityvalues=@severityvalues 
			where policyid = @policyid 
				and assessmentid = @assessmentid
				and metricid = @metricid

		select @err = @@error

		if @err <> 0
		begin
			set @msg = 'Error: Failed to ' + lower(@action) + ' ' + lower(@category) + ' with policy id ' + CONVERT(nvarchar, @policyid) + ' and assessment id ' + CONVERT(NVARCHAR, @assessmentid) + ' and metric id ' + CONVERT(nvarchar, @metricid)
			exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
			RAISERROR (@msg, 16, 1)
			ROLLBACK TRAN
			return -1
		end

		select @assessmentstate=assessmentstate 
			from [assessment] 
			where policyid = @policyid 
				and assessmentid = @assessmentid

		if (@oldisenabled <> @isenabled)
		begin
			set @msg = N'Security Check ' + @metricname + case when @isenabled = 1 then N' enabled' else N' disabled' end
			exec isp_sqlsecure_addpolicychangelog @policyid=@policyid, @assessmentid=@assessmentid, @state=@assessmentstate, @description=@msg
		end

		if (@oldreportkey <> @reportkey)
		begin
			set @msg = N'Security Check ' + @metricname + N' External Cross Reference changed from ' + @oldreportkey + N' to ' + @reportkey
			exec isp_sqlsecure_addpolicychangelog @policyid=@policyid, @assessmentid=@assessmentid, @state=@assessmentstate, @description=@msg
		end

		if (@oldreporttext <> @reporttext)
		begin
			set @msg = N'Security Check ' + @metricname + N' External Cross Reference changed from ' + @oldreporttext + N' to ' + @reporttext
			exec isp_sqlsecure_addpolicychangelog @policyid=@policyid, @assessmentid=@assessmentid, @state=@assessmentstate, @description=@msg
		end

		if (@oldseverity <> @severity)
		begin
			set @msg = N'Security Check ' + @metricname + N' Risk Level changed from ' + dbo.getpolicyseverityname(@oldseverity) + N' to ' + dbo.getpolicyseverityname(@severity)
			exec isp_sqlsecure_addpolicychangelog @policyid=@policyid, @assessmentid=@assessmentid, @state=@assessmentstate, @description=@msg
		end

		if (@oldseverityvalues <> @severityvalues)
		begin
			set @msg = N'Security Check ' + @metricname + N' configured values changed from ' + @oldseverityvalues + N' to ' + @severityvalues
			exec isp_sqlsecure_addpolicychangelog @policyid=@policyid, @assessmentid=@assessmentid, @state=@assessmentstate, @description=@msg
		end

		set @msg = @category + ' "' + @metricname + '" with id ' + CONVERT(NVARCHAR, @metricid)+ N' on ' + @category2 + ' "' + @policyname + '" with id ' + CONVERT(NVARCHAR, @policyid) + ' and assessment id ' + CONVERT(NVARCHAR, @assessmentid)
		exec isp_sqlsecure_addactivitylog @activitytype=@success, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname=null
	end

	COMMIT TRAN

GO

