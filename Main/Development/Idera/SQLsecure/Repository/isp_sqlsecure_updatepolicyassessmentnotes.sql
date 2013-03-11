SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_updatepolicyassessmentnotes]'))
drop procedure [dbo].[isp_sqlsecure_updatepolicyassessmentnotes]
GO

CREATE procedure [dbo].[isp_sqlsecure_updatepolicyassessmentnotes]
(
	@policyid int,
	@assessmentid int = null,		-- default to policy settings for backward compatibility
	@metricid int,
	@snapshotid int,
	@isexplained bit,
	@notes nvarchar(4000)
)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Update policyassessmentnotes with new metric configuration

	declare @err int, @msg nvarchar(500)

	declare @programname nvarchar(128), @action nvarchar(32), @category nvarchar(32), @success nvarchar(32), @failure nvarchar(32)
	select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)
	select @action=N'Update', @category=N'Explanation Notes', @success=N'Success Audit', @failure=N'Failure Audit'

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
	declare @policyassessmentnotesid int,
			@oldisexplained bit,
			@oldnotes nvarchar(4000),
			@assessmentstate nvarchar(20) 
	select @oldisexplained = 0, @oldnotes= N''
	select @assessmentstate = assessmentstate
		from assessment
		where policyid = @policyid 
			and assessmentid = @assessmentid 
	select @err = @@error

	if (@err <> 0 or @assessmentstate is null)
	begin
		ROLLBACK TRAN
		set @msg = 'Error: Failed to ' + lower(@action) + ' ' + lower(@category) + ' with policy id ' + CONVERT(nvarchar, @policyid) + ' and assessment id ' + CONVERT(NVARCHAR, @assessmentid) + ' and metric id ' + CONVERT(nvarchar, @metricid) + '. Unable to retrieve assessment.'
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end

	select @policyassessmentnotesid = policyassessmentnotesid,
			@oldisexplained = isexplained, 
			@oldnotes = notes
		from [policyassessmentnotes]
		where policyid = @policyid 
			and assessmentid = @assessmentid 
			and metricid = @metricid 
			and snapshotid = @snapshotid

	if (@oldisexplained <> @isexplained
		or @oldnotes <> @notes)
	begin
		if (len(rtrim(isnull(@notes, N''))) = 0 and @isexplained = 0)
		begin
			if (@policyassessmentnotesid is not null)
			begin
				delete 
					from [policyassessmentnotes]
					where policyassessmentnotesid = @policyassessmentnotesid

			select @err = @@error

			if @err <> 0
			begin
				set @msg = 'Error: Failed to remove ' + lower(@category) + ' with policy id ' + CONVERT(nvarchar, @policyid) + ' and assessment id ' + CONVERT(NVARCHAR, @assessmentid) + ' and metric id ' + CONVERT(nvarchar, @metricid) + ' and snapshot id ' + CONVERT(nvarchar, @snapshotid)
				exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
				RAISERROR (@msg, 16, 1)
				ROLLBACK TRAN
				return -1
			end
			end
		end
		else if (@policyassessmentnotesid is null)
		begin
			insert into [policyassessmentnotes] (
					policyid,
					assessmentid,
					metricid,
					snapshotid,
					isexplained, 
					notes)
				values (
					@policyid, 
					@assessmentid,
					@metricid,
					@snapshotid,
					@isexplained, 
					@notes )

			select @err = @@error

			if @err <> 0
			begin
				set @msg = 'Error: Failed to add ' + lower(@category) + ' with policy id ' + CONVERT(nvarchar, @policyid) + ' and assessment id ' + CONVERT(NVARCHAR, @assessmentid) + ' and metric id ' + CONVERT(nvarchar, @metricid) + ' and snapshot id ' + CONVERT(nvarchar, @snapshotid)
				exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
				RAISERROR (@msg, 16, 1)
				ROLLBACK TRAN
				return -1
			end
		end
		else
		begin
			update [policyassessmentnotes] set 
					isexplained=@isexplained, 
					notes=@notes 
				where policyassessmentnotesid = @policyassessmentnotesid

			select @err = @@error

			if @err <> 0
			begin
				set @msg = 'Error: Failed to ' + lower(@action) + ' ' + lower(@category) + ' with policy id ' + CONVERT(nvarchar, @policyid) + ' and assessment id ' + CONVERT(NVARCHAR, @assessmentid) + ' and metric id ' + CONVERT(nvarchar, @metricid) + ' and snapshot id ' + CONVERT(nvarchar, @snapshotid)
				exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
				RAISERROR (@msg, 16, 1)
				ROLLBACK TRAN
				return -1
			end
		end

		if (@oldisexplained <> @isexplained)
		begin
			set @msg = N'Security Check ' + @metricname + N' has been marked' + case when @isexplained = 1 then N' explained'  else N' not explained'  end
			exec isp_sqlsecure_addpolicychangelog @policyid=@policyid, @assessmentid=@assessmentid, @state=@assessmentstate, @description=@msg
		end

		if (@oldnotes <> @notes)
		begin
			set @msg = N'Security Check ' + @metricname + N' explanation notes changed from ' + @oldnotes + N' to ' + @notes
			exec isp_sqlsecure_addpolicychangelog @policyid=@policyid, @assessmentid=@assessmentid, @state=@assessmentstate, @description=@msg
		end

		set @msg = @category + N' policy "' + @policyname + '" with id ' + CONVERT(NVARCHAR, @policyid) + ' and assessment id ' + CONVERT(NVARCHAR, @assessmentid) + ' and metric "' + @metricname + '" with id ' + CONVERT(NVARCHAR, @metricid)
		exec isp_sqlsecure_addactivitylog @activitytype=@success, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname=null
	end

	COMMIT TRAN

GO

