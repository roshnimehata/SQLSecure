SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_createassessmentfrompolicy]'))
drop procedure [dbo].[isp_sqlsecure_createassessmentfrompolicy]
GO


CREATE procedure [dbo].[isp_sqlsecure_createassessmentfrompolicy] 
(
	@policyid int,
	@assessmentid int = null,
	@name nvarchar(128) = null,
	@description nvarchar(2048) = null,
	@assessmentdate datetime = null,
	@usebaseline bit = null,
	@type nchar(1) = N'D',
	@copy int = 1,
	@newassessmentid int output
)
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Create a new assessment by copying the chosen policy or assessment
   -- 	           
   -- 	           Parameters:
   -- 	             @policyid - the id of the policy to copy
   -- 	             @assessmentid - the id of the assessment to copy or null to use the default policy settings
   -- 	             @name - the name of the new assessment. If null, a name will be generated. The current assessment will always be named 'Current'
   -- 	             @description - the description of the new assessment
   -- 	             @assessmentdate - the rundate to use when copying the policy
   -- 	             @usebaseline - the usebaseline flag to use when copying the policy
   -- 	             @type - determines what type of assessment is being created
   --					Values: (case does not matter)
   --						D - draft	(default)
   --						C - current
   --                @copy - determines what type of copy operation to perform
   --					Values:
   --						1 - a new assessment is created with a full copy of the policy settings and assessment values
   --						2 - a new assessment is created with only a copy of the policy settings
   --						3 - delete the settings and data for the current assessment and recopy the settings from the policy default
   --                @newassessmentid - will contain the assessmentid of the assessment that was created or refreshed
   --
   --				Valid combinations for @type and @copy are:
   --					D,1
   --					D,2
   --					C,2
   --					C,3
   --

	declare @err int, @msg nvarchar(500)
	declare @policyname nvarchar(128)

	declare @programname nvarchar(128), @action nvarchar(32), @category nvarchar(32), @success nvarchar(32), @failure nvarchar(32)
	select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)
	select @action=N'Add', @category=N'Assessment', @success=N'Success Audit', @failure=N'Failure Audit'

	declare @ans int
	exec @ans = [isp_sqlsecure_isadmin]

	if (@ans = 0)
	begin
		set @msg = N'Error: Insufficient privileges to ' + lower(@action) + ' a ' + lower(@category)
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end

	declare @typedraft nchar(1), @typecurrent nchar(1), @typesettings nchar(1), @assessmentstate nchar(1)
	declare @copyall int, @copysettings int, @copyrefresh int
	select @typedraft=N'D',@typecurrent=N'C',@typesettings=N'S',
			@copyall=1, @copysettings=2, @copyrefresh=3

	select @type = upper(@type)			--make type parameter case insensitive
	if (@type not in (@typedraft, @typecurrent))
	begin
		begin
			set @msg = 'Error: Failed to ' + lower(@action) + ' ' + lower(@category) + ' for Policy id ' + CONVERT(NVARCHAR, @policyid) + '. ' + @type + ' is not a valid type to create.'
			exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
			RAISERROR (@msg, 16, 1)
			return -1
		end
	end

	if (@copy not in (@copyall, @copysettings, @copyrefresh))
	begin
		begin
			set @msg = 'Error: Failed to ' + lower(@action) + ' ' + lower(@category) + ' for Policy id ' + CONVERT(NVARCHAR, @policyid) + '. ' + CONVERT(NVARCHAR, @copy) + ' is not a valid copy operation.'
			exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
			RAISERROR (@msg, 16, 1)
			return -1
		end
	end

	if ((@type = @typedraft and @copy not in (@copyall, @copysettings)) or
		(@type = @typecurrent and @copy not in (@copysettings, @copyrefresh)))
	begin
		begin
			set @msg = 'Error: Failed to ' + lower(@action) + ' ' + lower(@category) + ' for Policy id ' + CONVERT(NVARCHAR, @policyid) + '. The parameters @type=' + @type + ' and @copy=' + @copy + ' are not compatible.'
			exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
			RAISERROR (@msg, 16, 1)
			return -1
		end
	end

	-- there can only be one current assessment
	if (@type = @typecurrent)
	begin
		select @assessmentid = null, @assessmentstate = @typecurrent, @name = N'Current', @assessmentdate = null, @usebaseline = 0
		if exists (select * from assessment where policyid = @policyid and assessmentstate = @typecurrent)
		begin
			-- if refreshing it must exist
			if (@copy <> @copyrefresh)
			begin
				set @msg = 'Error: Failed to ' + lower(@action) + ' current ' + lower(@category) + ' for Policy id ' + CONVERT(NVARCHAR, @policyid) + '. A current ' + lower(@category) + ' already exists.'
				exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
				RAISERROR (@msg, 16, 1)
				return -1
			end
		end

		-- make sure the assessmentid passed is the current assessment
		select @newassessmentid = assessmentid from assessment where policyid = @policyid and assessmentstate = @typecurrent
		if (@assessmentid is not null and @assessmentid <> @newassessmentid)
		begin
			set @msg = 'Error: Failed to update current ' + lower(@category) + ' for Policy id ' + CONVERT(NVARCHAR, @policyid) + '. The ' + lower(@category) + ' with id ' + CONVERT(NVARCHAR, @policyid) + ' is not a current ' + lower(@category) + '.'
			exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
			RAISERROR (@msg, 16, 1)
			return -1
		end

		select @assessmentid = [dbo].[getdefaultassessmentid](@policyid)
	end
	else if exists (select * from assessment where policyid = @policyid and assessmentname = @name)
		begin
			if (@copy <> @copyrefresh)
			begin
				set @msg = 'Error: Failed to ' + lower(@action) + ' ' + lower(@category) + ' for Policy id ' + CONVERT(NVARCHAR, @policyid) + '. An ' + lower(@category) + ' with the name ' + @name + ' already exists.'
				exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
				RAISERROR (@msg, 16, 1)
				return -1
			end
		end
	else
	begin
		select @assessmentid = isnull(@assessmentid,[dbo].[getdefaultassessmentid](@policyid)),
					@assessmentstate = @type

		declare @policydate datetime, @policybaseline bit, @issettings bit, @currentid int
		select @policydate=assessmentdate, 
				@policybaseline=usebaseline, 
				@issettings = case when assessmentstate = @typesettings then 1 else 0 end
			from assessment 
			where policyid = @policyid 
				and assessmentid = @assessmentid
		if (@issettings = 1)
			select @currentid = assessmentid 
				from assessment 
				where policyid = @policyid 
					and assessmentstate = @typecurrent
		select @assessmentdate = isnull(@assessmentdate, isnull(@policydate, getutcdate()))
		select @usebaseline = isnull(@usebaseline, isnull(@policybaseline, 0))
	end

	select @policyname = dbo.[getassessmentname](@policyid, @assessmentid)
	if (@policyname is null)
	begin
		set @msg = 'Error: Failed to copy ' + lower(@category) + '. Policy id ' + CONVERT(NVARCHAR, @policyid) + ' does not exist' 
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end

	BEGIN TRAN

	if (@copy in (@copyall, @copysettings))
	begin
		-- create the assessment
		insert into [assessment] (
					[policyid], 
					[assessmentstate], 
					[assessmentname], 
					[assessmentdescription], 
					[assessmentnotes], 
					[assessmentdate], 
					[usebaseline], 
					[isdynamic], 
					[dynamicselection]
			)
			select
					@policyid, 
					@assessmentstate, 
					isnull(@name, N'Assessment created ' + convert(nvarchar,getdate())), 
					isnull(@description, a.[policydescription]), 
					N'', 
					@assessmentdate, 
					@usebaseline, 
					0, 
					N''
				from [policy] a,
					[assessment] b
				where a.[policyid] = @policyid
					and a.[policyid] = b.[policyid]
					and b.[assessmentid] = @assessmentid

		select @err = @@error

		if @err <> 0
		begin
			ROLLBACK TRAN
			set @msg = 'Error: Failed to ' + lower(@action) + ' ' + lower(@category) + ' "' + @policyname
			exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
			RAISERROR (@msg, 16, 1)
			return -1
		end

		set @newassessmentid = @@IDENTITY
	end
	else if (@copy = @copyrefresh)
	begin
		-- clear the data
		delete from policyassessmentdetail
			where policyid = @policyid
				and assessmentid = @newassessmentid

		select @err = @@error

		if @err <> 0
		begin
			ROLLBACK TRAN
			set @msg = 'Error: Failed to refresh current ' + lower(@category) + ' for "' + @policyname + '". The assessment detail could not be deleted. '
			exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
			RAISERROR (@msg, 16, 1)
			return -1
		end
		delete from policyassessmentnotes
			where policyid = @policyid
				and assessmentid = @newassessmentid

		select @err = @@error

		if @err <> 0
		begin
			ROLLBACK TRAN
			set @msg = 'Error: Failed to refresh current ' + lower(@category) + ' for "' + @policyname + '". The assessment notes could not be deleted. '
			exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
			RAISERROR (@msg, 16, 1)
			return -1
		end
		delete from policyassessment
			where policyid = @policyid
				and assessmentid = @newassessmentid

		select @err = @@error

		if @err <> 0
		begin
			ROLLBACK TRAN
			set @msg = 'Error: Failed to refresh current ' + lower(@category) + ' for "' + @policyname + '". The assessment info could not be deleted. '
			exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
			RAISERROR (@msg, 16, 1)
			return -1
		end

		-- clear the settings
		delete from policymetric
			where policyid = @policyid
				and assessmentid = @newassessmentid

		select @err = @@error

		if @err <> 0
		begin
			ROLLBACK TRAN
			set @msg = 'Error: Failed to refresh current ' + lower(@category) + ' for "' + @policyname + '". The security check settings could not be deleted. '
			exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
			RAISERROR (@msg, 16, 1)
			return -1
		end

		delete from policymember
			where policyid = @policyid
				and assessmentid = @newassessmentid

		select @err = @@error

		if @err <> 0
		begin
			ROLLBACK TRAN
			set @msg = 'Error: Failed to refresh current ' + lower(@category) + ' for "' + @policyname + '". The member servers could not be deleted. '
			exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
			RAISERROR (@msg, 16, 1)
			return -1
		end

		delete from policyinterview
			where policyid = @policyid
				and assessmentid = @newassessmentid

		select @err = @@error

		if @err <> 0
		begin
			ROLLBACK TRAN
			set @msg = 'Error: Failed to refresh current ' + lower(@category) + ' for "' + @policyname + '". The interview could not be deleted. '
			exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
			RAISERROR (@msg, 16, 1)
			return -1
		end
	end

	-- now copy all of the metrics to the new assessment
	insert into policymetric (
				policyid,
				assessmentid,
				metricid,
				isenabled,
				reportkey,
				reporttext,
				severity,
				severityvalues)
		select policyid,
				@newassessmentid,
				metricid,
				isenabled,
				reportkey,
				reporttext,
				severity,
				severityvalues
			from policymetric
			where policyid = @policyid
				and assessmentid = @assessmentid

	select @err = @@error

	if @err <> 0
	begin
		ROLLBACK TRAN
		set @msg = 'Error: Failed to ' + lower(@action) + ' ' + lower(@category) + ' "' + @policyname + '". The security check settings could not be copied. '
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end

	-- now copy all of the servers to the new assessment
	-- assessments cannot be dynamic, so get the server list and add them all manually
	create table #tmp_createassessmentfrompolicy_servertbl (registeredserverid int)
	insert into #tmp_createassessmentfrompolicy_servertbl
		exec SQLsecure.dbo.isp_sqlsecure_getpolicymemberlist
				@policyid = @policyid,
				@assessmentid = @assessmentid

	insert into policymember (
				policyid,
				assessmentid,
				registeredserverid)
		select @policyid,
				@newassessmentid,
				registeredserverid
			from #tmp_createassessmentfrompolicy_servertbl

	drop table #tmp_createassessmentfrompolicy_servertbl

	select @err = @@error

	if @err <> 0
	begin
		ROLLBACK TRAN
		set @msg = 'Error: Failed to ' + lower(@action) + ' ' + lower(@category) + ' "' + @policyname + '". The member servers could not be copied. '
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end

	insert into [policyinterview] (
				[policyid],
				[assessmentid],
				[istemplate],
				[interviewname],
				[interviewtext])
		select [policyid],
				@newassessmentid,
				[istemplate],
				[interviewname],
				[interviewtext]
			from [policyinterview]
			where policyid = @policyid
				and assessmentid = @assessmentid

	select @err = @@error

	if @err <> 0
	begin
		ROLLBACK TRAN
		set @msg = 'Error: Failed to ' + lower(@action) + ' ' + lower(@category) + ' "' + @policyname + '". The policy internal review notes could not be copied. '
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end

	-- copy all saved assessment data from the existing policy if creating a draft
	if (@copy in (@copyall))
	begin
		insert into [policyassessment] (
					[policyid],
					[assessmentid],
					[metricid],
					[snapshotid],
					[registeredserverid],
					[connectionname],
					[collectiontime],
					[metricname],
					[metrictype],
					[metricseveritycode],
					[metricseverity],
					[metricseverityvalues],
					[metricdescription],
					[metricreportkey],
					[metricreporttext],
					[severitycode],
					[severity],
					[currentvalue],
					[thresholdvalue])
			select [policyid],
					@newassessmentid,
					[metricid],
					[snapshotid],
					[registeredserverid],
					[connectionname],
					[collectiontime],
					[metricname],
					[metrictype],
					[metricseveritycode],
					[metricseverity],
					[metricseverityvalues],
					[metricdescription],
					[metricreportkey],
					[metricreporttext],
					[severitycode],
					[severity],
					[currentvalue],
					[thresholdvalue]
				from [policyassessment]
				where policyid = @policyid
					and assessmentid = isnull(@currentid, @assessmentid)

		select @err = @@error

		if @err <> 0
		begin
			ROLLBACK TRAN
			set @msg = 'Error: Failed to ' + lower(@action) + ' ' + lower(@category) + ' "' + @policyname + '". The policy assessment info could not be copied. '
			exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
			RAISERROR (@msg, 16, 1)
			return -1
		end

--		insert into [policyassessmentnotes] (
--					[policyid],
--					[assessmentid],
--					[metricid],
--					[snapshotid],
--					[isexplained],
--					[notes])
--			select [policyid],
--					@newassessmentid,
--					[metricid],
--					[snapshotid],
--					[isexplained],
--					[notes]
--				from [policyassessmentnotes]
--				where policyid = @policyid
--					and assessmentid = isnull(@currentid, @assessmentid)
--
--		select @err = @@error
--
--		if @err <> 0
--		begin
--			set @msg = 'Error: Failed to ' + lower(@action) + ' ' + lower(@category) + ' "' + @policyname + '". The policy assessment notes could not be copied. '
--			exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
--			RAISERROR (@msg, 16, 1)
--			ROLLBACK TRAN
--			return -1
--		end

		insert into [policyassessmentdetail] (
					[policyid],
					[assessmentid],
					[metricid],
					[snapshotid],
					[detailfinding],
					[databaseid],
					[objecttype],
					[objectid],
					[objectname])
			select [policyid],
					@newassessmentid,
					[metricid],
					[snapshotid],
					[detailfinding],
					[databaseid],
					[objecttype],
					[objectid],
					[objectname]
				from [policyassessmentdetail]
				where policyid = @policyid
					and assessmentid = isnull(@currentid, @assessmentid)

		select @err = @@error

		if @err <> 0
		begin
			ROLLBACK TRAN
			set @msg = 'Error: Failed to ' + lower(@action) + ' ' + lower(@category) + ' "' + @policyname + '". The policy assessment detail could not be copied. '
			exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
			RAISERROR (@msg, 16, 1)
			return -1
		end

		-- log the creation to the change log
		set @msg = case when @copy = @copyrefresh then N'Refreshed assessment settings' else N'Assessment created' end
		exec isp_sqlsecure_addpolicychangelog @policyid=@policyid, @assessmentid=@assessmentid, @state=@assessmentstate, @description=@msg

	end

	set @msg = @category + N' "' + @policyname + '" with id ' + CONVERT(NVARCHAR, @policyid) + ' and assessmentid ' + CONVERT(NVARCHAR, @policyid) + ' with state ' + dbo.getassessmentstatename(@assessmentstate)
	exec isp_sqlsecure_addactivitylog @activitytype=@success, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname=null

	COMMIT TRAN

GO
