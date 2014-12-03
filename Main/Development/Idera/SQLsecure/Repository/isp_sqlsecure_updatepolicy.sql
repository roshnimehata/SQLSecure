SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_updatepolicy]'))
drop procedure [dbo].[isp_sqlsecure_updatepolicy]
GO

CREATE procedure [dbo].[isp_sqlsecure_updatepolicy]
(
	@policyid int, 
	@assessmentid int,
	@policyname nvarchar(128), 
	@policydescription nvarchar(2048), 
	@assessmentstate nchar(1), 
	@isdynamic bit, 
	@dynamicselection nvarchar(4000), 
	@assessmentname nvarchar(128), 
	@assessmentdescription nvarchar(2048), 
	@assessmentnotes nvarchar(4000), 
	@assessmentdate datetime, 
	@usebaseline bit, 
	@interviewname nvarchar(256) = null, 
	@interviewtext nvarchar(max) = null
)
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Update policy with new info
   --			   Note system policies cannot be updated and a policy cannot be changed to or from type system

	declare @err int, @msg nvarchar(500)

	declare @programname nvarchar(128), @action nvarchar(32), @category nvarchar(32), @category2 nvarchar(32), @success nvarchar(32), @failure nvarchar(32)
	select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)
	select @action=N'Update', @category=N'Policy', @category2=N'Assessment', @success=N'Success Audit', @failure=N'Failure Audit'

	-- because it is passed in, we can use the state here before the first message to fix the category
	declare @isassessment bit
	select @isassessment = case when @assessmentstate in (N'D', N'P', N'A') then 1 else 0 end
	if (@isassessment = 1)
		set @category = @category2

	declare @ans int
	exec @ans = [isp_sqlsecure_isadmin]

	if (@ans = 0)
	begin
		set @msg = N'Error: Insufficient privileges to ' + lower(@action) + ' a ' + lower(@category)
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end

	declare @name nvarchar(128), @aname nvarchar(128),@issystem bit
	select @assessmentid = isnull(@assessmentid,[dbo].[getdefaultassessmentid](@policyid))
	select @name=policyname, @issystem=issystempolicy from policy where policyid = @policyid
	select @aname=assessmentname from assessment where policyid = @policyid and assessmentid = @assessmentid

	if (@name is null)
	begin
		set @msg = 'Error: Invalid ' + @category + ' id ' + CONVERT(nvarchar, @policyid)
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end
	else if (@issystem = 1 and @name <> @policyname)
	begin
		set @msg = 'Error: ' + @category + ' "' + @name + '" with id ' + CONVERT(NVARCHAR, @policyid) + ' is a system ' + lower(@category) + ' and cannot be updated'
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end
	else if exists(select * from [policy] where policyid <> @policyid AND UPPER(policyname) = UPPER(@policyname))
	begin
		set @msg = 'Error: Cannot rename ' + lower(@category) + ' "' + @name + '" to "' + @policyname + '". This name already exists and must be unique.'
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end
	else if (@isassessment = 1 and exists(select * from [assessment] where policyid = @policyid and assessmentid <> @assessmentid AND UPPER(assessmentname) = UPPER(@assessmentname)))
	begin
		set @msg = 'Error: Cannot rename ' + lower(@category2) + ' "' + @aname + '" to "' + @assessmentname + '". This name already exists and must be unique within the policy.'
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end
	else
	begin
		BEGIN TRAN

		if exists (select * 
					from [policy] 
					where 
						policyid = @policyid 
						and (policyname <> @policyname 
								or policydescription <> @policydescription))
		begin
			update [policy] set
					policyname=@policyname, 
					policydescription=@policydescription
				where 
					policyid = @policyid 

			select @err = @@error

			if @err <> 0
			begin
				ROLLBACK TRAN
				set @msg = 'Error: Failed to ' + lower(@action) + ' ' + lower(@category) + ' "' + @name + '" with id ' + CONVERT(NVARCHAR, @policyid)
				exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
				RAISERROR (@msg, 16, 1)
				return -1
			end
		end

		select @msg = N''
		-- see if any values have changed for logging
		declare @oldassessmentstate nchar(1), 
				@oldisdynamic bit, 
				@olddynamicselection nvarchar(4000), 
				@oldassessmentname nvarchar(128), 
				@oldassessmentdescription nvarchar(2048), 
				@oldassessmentnotes nvarchar(4000), 
				@oldassessmentdate datetime, 
				@oldusebaseline bit 
		select @oldassessmentstate = assessmentstate,
				@oldisdynamic = isdynamic, 
				@olddynamicselection = isnull(dynamicselection, N''), 
				@oldassessmentname = assessmentname, 
				@oldassessmentdescription = assessmentdescription, 
				@oldassessmentnotes = assessmentnotes, 
				@oldassessmentdate = assessmentdate, 
				@oldusebaseline = usebaseline
			from [assessment]
			where 
				policyid = @policyid 
				and assessmentid = @assessmentid

		if (@oldassessmentstate <> @assessmentstate 
				or @oldisdynamic <> @isdynamic 
				or @olddynamicselection <> @dynamicselection 
				or @oldassessmentname <> @assessmentname 
				or @oldassessmentdescription <> @assessmentdescription 
				or @oldassessmentnotes <> @assessmentnotes 
				or @oldassessmentdate <> @assessmentdate 
				or @oldusebaseline <> @usebaseline)
		begin
			update [assessment] set
					assessmentstate=@assessmentstate,
					isdynamic=@isdynamic, 
					dynamicselection=@dynamicselection, 
					assessmentname=@assessmentname, 
					assessmentdescription=@assessmentdescription, 
					assessmentnotes=@assessmentnotes, 
					assessmentdate=@assessmentdate, 
					usebaseline=@usebaseline 
				where 
					policyid = @policyid 
					and assessmentid = @assessmentid

			select @err = @@error

			if @err <> 0
			begin
				ROLLBACK TRAN
				set @msg = 'Error: Failed to ' + lower(@action) + ' assessment for "' + @name + '" with id ' + CONVERT(NVARCHAR, @policyid) + ' and assessmentid ' + CONVERT(NVARCHAR, @assessmentid)
				exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
				RAISERROR (@msg, 16, 1)
				return -1
			end

			if (@oldassessmentstate <> @assessmentstate)
			begin
				set @msg = N'Assessment state changed from ' + dbo.getassessmentstatename(@oldassessmentstate) + N' to ' + dbo.getassessmentstatename(@assessmentstate)
				exec isp_sqlsecure_addpolicychangelog @policyid=@policyid, @assessmentid=@assessmentid, @state=@assessmentstate, @description=@msg
			end

			if (@oldisdynamic <> @isdynamic)
			begin
				set @msg = N'Server selection changed to ' + case when @isdynamic = 1 then N' dynamic' else N' manual' end
				exec isp_sqlsecure_addpolicychangelog @policyid=@policyid, @assessmentid=@assessmentid, @state=@assessmentstate, @description=@msg
			end

			if (@isdynamic = 1 and @olddynamicselection <> @dynamicselection)
			begin
				set @msg = N'Server selection changed to include all servers ' + @dynamicselection
				exec isp_sqlsecure_addpolicychangelog @policyid=@policyid, @assessmentid=@assessmentid, @state=@assessmentstate, @description=@msg
			end

			if (@oldassessmentname <> @assessmentname)
			begin
				set @msg = N'Assessment name changed from ' + @oldassessmentname + N' to ' + @assessmentname
				exec isp_sqlsecure_addpolicychangelog @policyid=@policyid, @assessmentid=@assessmentid, @state=@assessmentstate, @description=@msg
			end

			if (@oldassessmentdescription <> @assessmentdescription)
			begin
				set @msg = N'Assessment description changed from ' + @oldassessmentdescription + N' to ' + @assessmentdescription
				exec isp_sqlsecure_addpolicychangelog @policyid=@policyid, @assessmentid=@assessmentid, @state=@assessmentstate, @description=@msg
			end

			if (@oldassessmentnotes <> @assessmentnotes)
			begin
				set @msg = N'Assessment notes changed from ' + @oldassessmentnotes + N' to ' + @assessmentnotes
				exec isp_sqlsecure_addpolicychangelog @policyid=@policyid, @assessmentid=@assessmentid, @state=@assessmentstate, @description=@msg
			end

			if (@oldassessmentdate <> @assessmentdate)
			begin
				set @msg = N'Assessment data selection date changed from ' + CONVERT(nvarchar, @oldassessmentdate, 109) + N' to ' + convert(nvarchar, @assessmentdate, 109)
				exec isp_sqlsecure_addpolicychangelog @policyid=@policyid, @assessmentid=@assessmentid, @state=@assessmentstate, @description=@msg
			end

			if (@oldusebaseline <> @usebaseline)
			begin
				set @msg = N'Assessment data selection changed to use ' + case when @usebaseline = 1 then N' only baseline snapshots' else N' any snapshots' end
				exec isp_sqlsecure_addpolicychangelog @policyid=@policyid, @assessmentid=@assessmentid, @state=@assessmentstate, @description=@msg
			end
		end

		-- delete policymembers if the policy has switched to dynamic
		if (@isdynamic=1 and exists (SELECT * FROM policymember WHERE policyid = @policyid and assessmentid = @assessmentid))
		begin
			DELETE policymember 
				WHERE policyid = @policyid 
					and assessmentid = @assessmentid

			select @err = @@error

			if @err <> 0
			begin
				ROLLBACK TRAN
				set @msg = 'Error: Failed to delete policy members for dynamic ' + lower(@category) + ' "' + @policyname + '" with id ' + CONVERT(NVARCHAR, @policyid) + ' and assessment id ' + CONVERT(NVARCHAR, @assessmentid)
				exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
				RAISERROR (@msg, 16, 1)
				return -1
			end
		end

		-- update the interview if it is passed, if it is null, then leave it alone
		if (@interviewname IS NOT NULL OR @interviewtext IS NOT NULL)
		begin
			if (LEN(RTRIM(ISNULL(@interviewname, '') + ISNULL(CONVERT(NVARCHAR(100), @interviewtext), ''))) = 0)
			begin
				-- if the passed value is empty, make sure the interview doesn't exist
				if exists (SELECT * FROM policyinterview WHERE policyid = @policyid and assessmentid = @assessmentid)
				begin
					DELETE policyinterview 
						WHERE policyid = @policyid 
							and assessmentid = @assessmentid

					select @err = @@error

					if @err <> 0
					begin
						ROLLBACK TRAN
						set @msg = 'Error: Failed to delete policy interview for ' + lower(@category) + ' "' + @policyname + '" with id ' + CONVERT(NVARCHAR, @policyid) + ' and assessment id ' + CONVERT(NVARCHAR, @assessmentid)
						exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
						RAISERROR (@msg, 16, 1)
						return -1
					end

					set @msg = N'Internal Review Notes removed'
					exec isp_sqlsecure_addpolicychangelog @policyid=@policyid, @assessmentid=@assessmentid, @state=@assessmentstate, @description=@msg
				end
			end
			else
			begin
				-- if the interview exists, add it, otherwise update it
				if exists (SELECT * FROM policyinterview WHERE policyid = @policyid and assessmentid = @assessmentid)
				begin
					declare @oldinterviewname nvarchar(256), 
							@oldpointer bigint,
							@newpointer bigint,
							@textlength bigint,
							@oldtext nvarchar(4000),
							@newtext nvarchar(4000),
							@interviewtextchanged bit

					select @oldinterviewname = interviewname,
							@interviewtextchanged = case when datalength(isnull(interviewtext,N'')) <> datalength(isnull(@interviewtext,N'')) then 1 else 0 end,
							@textlength = datalength(isnull(@interviewtext,N''))
						from policyinterview
						where 
							policyid = @policyid 
							and assessmentid = @assessmentid

					if (@interviewtextchanged = 0)
					begin
						select @oldpointer = 0,
								@newpointer = 0

						while (@oldpointer < @textlength
								and @newpointer < datalength(@interviewtext))
						begin
							select @interviewtextchanged = case when substring(interviewtext,@oldpointer,4000) <> substring(@interviewtext,@newpointer,4000) then 1 else 0 end,
									@oldpointer = @oldpointer + 4000,
									@newpointer = @newpointer + 4000
								from policyinterview
								where 
									policyid = @policyid 
									and assessmentid = @assessmentid
							
							if (@interviewtextchanged = 1)
								break
						end					
					end

					if (@oldinterviewname <> @interviewname
						or  @interviewtextchanged = 1)
					begin
						update policyinterview set
							interviewname = isnull(@interviewname,''),
							interviewtext = isnull(@interviewtext,'')
						where policyid = @policyid
							and assessmentid = @assessmentid

						select @err = @@error

						if @err <> 0
						begin
							ROLLBACK TRAN
							set @msg = 'Error: Failed to update policy interview for ' + lower(@category) + ' "' + @policyname + '" with id ' + CONVERT(NVARCHAR, @policyid) + ' and assessment id ' + CONVERT(NVARCHAR, @assessmentid)
							exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
							RAISERROR (@msg, 16, 1)
							return -1
						end

						if (@oldinterviewname <> @interviewname)
						begin
							set @msg = N'Internal Review Notes Title changed from ' + @oldinterviewname + N' to ' + @interviewname
							exec isp_sqlsecure_addpolicychangelog @policyid=@policyid, @assessmentid=@assessmentid, @state=@assessmentstate, @description=@msg
						end

						if (@interviewtextchanged = 1)
						begin
							set @msg = N'Internal Review Notes changed'
							exec isp_sqlsecure_addpolicychangelog @policyid=@policyid, @assessmentid=@assessmentid, @state=@assessmentstate, @description=@msg
						end
					end
				end
				else
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
							isnull(@interviewname,''),
							isnull(@interviewtext,'')
							)

					select @err = @@error

					if @err <> 0
					begin
						ROLLBACK TRAN
						set @msg = 'Error: Failed to add policy interview for ' + lower(@category) + ' "' + @policyname + '" with id ' + CONVERT(NVARCHAR, @policyid) + ' and assessment id ' + CONVERT(NVARCHAR, @assessmentid)
						exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
						RAISERROR (@msg, 16, 1)
						return -1
					end

					set @msg = N'Internal Review Notes added'
					exec isp_sqlsecure_addpolicychangelog @policyid=@policyid, @assessmentid=@assessmentid, @state=@assessmentstate, @description=@msg
				end
			end

			-- if it is the policy settings then make sure to match the current assessment interview to the policy interview
			if (@assessmentstate = N'S')
			begin
				declare @currentid int
				select @currentid=assessmentid 
					from assessment
					where
						policyid = @policyid
						and assessmentstate = N'C'

				-- if there is no current assessment, then it will be fixed when it is created, so don't worry about it here
				if (@currentid is not null)
				begin
					DELETE policyinterview 
						WHERE policyid = @policyid 
							and assessmentid = @currentid

					INSERT INTO policyinterview (
								policyid,
								assessmentid,
								istemplate,
								interviewname,
								interviewtext
								)
						SELECT policyid,
								@currentid,
								istemplate,
								interviewname,
								interviewtext
							FROM policyinterview 
						WHERE policyid = @policyid 
							and assessmentid = @assessmentid
				end
			end
		end

		set @msg = @category + N' "' + @policyname + '" with policy id ' + CONVERT(NVARCHAR, @policyid) + ' and assessment id ' + CONVERT(NVARCHAR, @assessmentid)
		exec isp_sqlsecure_addactivitylog @activitytype=@success, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname=null

		COMMIT TRAN
	end


GO

