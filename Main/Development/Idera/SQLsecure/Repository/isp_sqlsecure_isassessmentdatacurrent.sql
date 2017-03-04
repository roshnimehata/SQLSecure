SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_isassessmentdatacurrent]'))
drop procedure [dbo].[isp_sqlsecure_isassessmentdatacurrent]
GO

CREATE procedure [dbo].[isp_sqlsecure_isassessmentdatacurrent]
(
	@policyid int,
	@assessmentid int,
	@valid bit out
)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Determine if the passed policy or assessment contains valid, current assessment data.
   --
   --			   Returns:
   --					0 if the assessment needs to be recreated
   --					1 if the assessment contains valid and current data
   --
   --			   If the policy is passed (state 'S') then the current assessment will be checked
   --				and if the current assessment doesn't exist it will be created and 0 will be returned
   --			   If an approved assessment is passed, 1 is always returned
   --			   If invalid data is encountered, an error will be thrown and no value is returned
   --
BEGIN
	declare @programname nvarchar(128), @action nvarchar(32), @category nvarchar(32), @success nvarchar(32), @failure nvarchar(32)
	select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)
	select @action=N'Validate', @category=N'Assessment data', @success=N'Success Audit', @failure=N'Failure Audit'

	declare @err int, @msg nvarchar(500)
	declare @policyname nvarchar(128)

	declare @currentid int, 
		@settingsid int, 
		@state nchar(1), 
		@isvalid int,
		@rc int,
		@debug bit

	select @currentid = -1, 
		@settingsid = -1, 
		@state = N'U',
		@isvalid = -1,
		@rc = 0,
		@debug = 0

	select @state=assessmentstate, @policyname=policyassessmentname from vwpolicy where policyid = @policyid and assessmentid = @assessmentid
	if (@debug=1)
	begin
		set nocount off
		print '@policyid='+convert(nvarchar,@policyid)+', @assessmentid='+convert(nvarchar,@assessmentid)+', @state='+@state
	end
	else
		set nocount on

	if (@state = N'U')
	begin
		set @msg = 'Error: Failed to ' + lower(@action) + ' ' + lower(@category) + ' for Policy id ' + CONVERT(NVARCHAR, @policyid) + ' and Assessment id ' + CONVERT(NVARCHAR, @assessmentid) + '. The assessment was not found.'
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end

	if (@state = N'A')
	begin
		if (@debug=1)
			print 'approved'
		select @isvalid = 1
	end
	else
	begin
		if (@state = N'S')
		begin
			select @settingsid=@assessmentid
			select @currentid=assessmentid from assessment where policyid = @policyid and assessmentstate = 'C'
			select @assessmentid=@currentid		-- will set to -1 if current not found
			select @state = N'C'
			if (@debug=1)
				print 'settings: @settingsid='+convert(nvarchar,@settingsid)+', @currentid='+convert(nvarchar,@currentid)
		end
		else if (@state = N'C')
		begin
			select @currentid=@assessmentid
			if (@debug=1)
				print '@currentid='+convert(nvarchar,@currentid)
		end

		declare @assessmentdate datetime, @usebaseline bit
		declare @snapshots table (registeredserverid int, snapshotid int)
		declare @snapshotssaved table (registeredserverid int, snapshotid int)
		create table #tmp_sqlsecure_getpolicymemberlist (registeredserverid int)

		if (@state = N'C')
		begin
			if (@debug=1)
				print 'current:'
--			if not exists (select * from assessment where policyid = @policyid and assessmentid = @assessmentid)
			if (@assessmentid = -1)
			begin
				if (@debug=1)
					print 'creating current'
				EXEC @rc = [dbo].[isp_sqlsecure_createassessmentfrompolicy]
					@policyid = @policyid,
					@assessmentid = @settingsid,
					@type = N'C',
					@copy = 2,
					@newassessmentid = @currentid output
				if (@rc = 0)
				begin
					select @assessmentid = @currentid,
						@isvalid = 0
					if (@debug=1)
						print 'new @currentid='+convert(nvarchar,@currentid)
				end
				else
				begin
					set @msg = 'Error: Failed to ' + lower(@action) + ' ' + lower(@category) + ' for Policy ' + @policyname + ' with id ' + CONVERT(NVARCHAR, @policyid) + ' and Assessment id ' + CONVERT(NVARCHAR, @assessmentid) + '. Unable to create current assessment for policy.'
					exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
					drop table #tmp_sqlsecure_getpolicymemberlist
					RAISERROR (@msg, 16, 1)
					return -1
				end
			end
			else 
			begin
				if (@settingsid = -1)	-- if came in as current, then get the settings id for comparison
				begin
					select @settingsid=assessmentid from assessment where policyid = @policyid and assessmentstate = 'S'
					if (@debug=1)
						print '@settingsid='+convert(nvarchar,@settingsid)
				end
			end

			-- get the server member list for the settings
			--create table #tmp_sqlsecure_getpolicymemberlist (registeredserverid int)	created above to handle duplicate use
			exec @err = [dbo].[isp_sqlsecure_getpolicymemberlist] 
				@policyid = @policyid, 
				@assessmentid = @settingsid

			if (@isvalid = -1
				and not exists (select * from policyassessment where policyid = @policyid and assessmentid = @currentid))
			begin
				if (@debug=1)
					print 'policyassessments not found'
				if (exists (select * from policymetric where policyid = @policyid and assessmentid = @settingsid and isenabled = 1)
					and exists (select * from #tmp_sqlsecure_getpolicymemberlist))
				begin
					-- use variables to work around sql 2000 syntax limitations
					select @assessmentdate=assessmentdate, @usebaseline=usebaseline 
						from assessment 
						where policyid = @policyid
							and assessmentid = @settingsid

					if (exists (select snapshotid
							from serversnapshot
							where registeredserverid in (select registeredserverid from #tmp_sqlsecure_getpolicymemberlist)
									and snapshotid in (select snapshotid from dbo.getsnapshotlist(@assessmentdate, @usebaseline))))
					begin
						select @isvalid = 0
						if (@debug=1)
							print 'metrics and valid snapshots found, not valid'
					end
					else
					begin
						select @isvalid = 1
						if (@debug=1)
							print 'metrics and servers found, but no valid snapshots found, valid'
					end
				end
				else
				begin
					select @isvalid = 1
					if (@debug=1)
						print 'metrics or servers not found, valid'
				end
			end
			else 
			begin
				-- check current metrics against the settings metrics

				-- SQLsecure 3.1 (Anshul Aggarwal) - Add support for Azure SQL Database.
				if exists (select * from policymetric a inner join policymetric b on a.policyid = b.policyid and a.metricid = b.metricid
				left join  policymetricextendedinfo c  on a.policyid = c.policyid and a.metricid = c.metricid and a.assessmentid = c.assessmentid
				left join policymetricextendedinfo d on c.policyid = d.policyid and c.metricid = d.metricid and c.servertype = d.servertype and b.assessmentid = d.assessmentid
							where a.policyid = @policyid and a.assessmentid = @settingsid and b.assessmentid = @currentid
								and (a.isenabled <> b.isenabled
									 or a. reportkey <> b.reportkey
									 or a. reporttext <> b.reporttext
									 or a. severity <> b.severity
									 or a. severityvalues <> b.severityvalues
									 or c. reportkey <> d.reportkey
									 or c. reporttext <> d.reporttext
									 or c. severity <> d.severity
									 or c. severityvalues <> d.severityvalues))
				begin
					if (@debug=1)
					begin
						print 'current policymetric not matching settings'
						select * from policymetric a inner join policymetric b on a.policyid = b.policyid and a.metricid = b.metricid
							left join  policymetricextendedinfo c  on a.policyid = c.policyid and a.metricid = c.metricid
							left join policymetricextendedinfo d on c.policyid = d.policyid and c.metricid = d.metricid and c.servertype = d.servertype 
							where a.policyid = @policyid and a.assessmentid = @settingsid and b.assessmentid = @currentid and
							      c.policyid = @policyid and c.assessmentid = @settingsid and d.assessmentid = @currentid
								and (a.isenabled <> b.isenabled
									 or a. reportkey <> b.reportkey
									 or a. reporttext <> b.reporttext
									 or a. severity <> b.severity
									 or a. severityvalues <> b.severityvalues
									 or c. reportkey <> d.reportkey
									 or c. reporttext <> d.reporttext
									 or c. severity <> d.severity
									 or c. severityvalues <> d.severityvalues)
					end
					select @isvalid = 0
				end
				else
				begin
					-- use variables to work around sql 2000 syntax limitations
					select @assessmentdate=assessmentdate, @usebaseline=usebaseline 
						from assessment 
						where policyid = @policyid
							and assessmentid = @settingsid

					insert into @snapshots
						select a.registeredserverid, b.snapshotid
							from #tmp_sqlsecure_getpolicymemberlist a left join serversnapshot b on a.registeredserverid = b.registeredserverid
								and b.snapshotid in (select snapshotid from dbo.getsnapshotlist(@assessmentdate, @usebaseline))
							-- only include null snapshots if the snapshot not found metric is enabled
							where b.snapshotid is not null or exists (select * from policymetric where policyid = @policyid and assessmentid = @settingsid and metricid = 54 and isenabled = 1)

					if (@debug=1)
						select * from @snapshots

					insert into @snapshotssaved
						select distinct registeredserverid, snapshotid 
							from policyassessment 
							where policyid = @policyid 
								and assessmentid = @assessmentid

					if (@debug=1)
						select * from @snapshotssaved

					if (exists (select * 
									from @snapshots a left join @snapshotssaved b on a.registeredserverid = b.registeredserverid and isnull(a.snapshotid,0) = isnull(b.snapshotid, 0) 
									where b.registeredserverid is null)
						or exists (select * 
									from @snapshotssaved a left join @snapshots b on a.registeredserverid = b.registeredserverid and isnull(a.snapshotid,0) = isnull(b.snapshotid, 0) 
									where b.registeredserverid is null))
					begin
						if (@debug=1)
							print 'snapshots don''t match'
						select @isvalid = 0
					end
				end
			end
		end
		else	-- process saved assessments
		begin
			-- get the server member list for the assessment
			--create table #tmp_sqlsecure_getpolicymemberlist (registeredserverid int)	created above to handle duplicate use
			exec @err = [dbo].[isp_sqlsecure_getpolicymemberlist] 
				@policyid = @policyid, 
				@assessmentid = @assessmentid

			if not exists (select * from policyassessment where policyid = @policyid and assessmentid = @assessmentid)
			begin
				if (@debug=1)
					print 'policyassessments not found'
				if (exists (select * from policymetric where policyid = @policyid and assessmentid = @assessmentid and isenabled = 1)
					and exists (select * from #tmp_sqlsecure_getpolicymemberlist))
				begin
					-- use variables to work around sql 2000 syntax limitations
					select @assessmentdate=assessmentdate, @usebaseline=usebaseline 
						from assessment 
						where policyid = @policyid
							and assessmentid = @assessmentid

					if (exists (select snapshotid
							from serversnapshot
							where registeredserverid in (select registeredserverid from #tmp_sqlsecure_getpolicymemberlist)
									and snapshotid in (select snapshotid from dbo.getsnapshotlist(@assessmentdate, @usebaseline))))
					begin
						select @isvalid = 0
						if (@debug=1)
							print 'metrics and valid snapshots found, not valid'
					end
					else
					begin
						select @isvalid = 1
						if (@debug=1)
							print 'metrics and servers found, but no valid snapshots found, valid'
					end
				end
				else
				begin
					select @isvalid = 1
					if (@debug=1)
						print 'metrics or servers not found, valid'
				end
			end
			else 
			begin 
				-- check the metrics against the assessment settings to see if any changed

				-- SQLsecure 3.1 (Anshul Aggarwal) - Add support for Azure SQL Database
				if exists (select * from policymetric a 
							left join policyassessment b on a.policyid = b.policyid and a.assessmentid = b.assessmentid and a.metricid = b.metricid
							inner join registeredserver c on b.connectionname = c.connectionname 
							left join policymetricextendedinfo d on a.policyid = d.policyid and a.assessmentid = d.assessmentid and a.metricid = d.metricid 
							where a.policyid = @policyid and a.assessmentid = @assessmentid
								and ((a.isenabled <> case when b.policyid is null then 0 else 1 end)
									 or 
									(
										 (c.servertype = 'OP' or c.servertype = 'AVM') and (a.reportkey <> b.metricreportkey
																							 or a.reporttext <> b.metricreporttext
																							 or a.severity <> b.metricseveritycode
																							 or a.severityvalues <> b.metricseverityvalues)
									 )
									 or 
									 (
										 (c.servertype = 'ADB') and (c.servertype = d.servertype) and (d.reportkey <> b.metricreportkey
																				 or d.reporttext <> b.metricreporttext
																				 or d.severity <> b.metricseveritycode
																				 or a.severityvalues <> b.metricseverityvalues)
									 )))
								-- if only metric 54 is returned, then there were no snapshots and other metrics will not be returned
								and exists (select * from policyassessment where policyid = @policyid and assessmentid = @assessmentid and metricid <> 54)
				begin
					if (@debug=1)
					begin
						print 'policymetric not matching saved values from policyassessment'
						select * from policymetric a 
							left join policyassessment b on a.policyid = b.policyid and a.assessmentid = b.assessmentid and a.metricid = b.metricid
							inner join registeredserver c on b.connectionname = c.connectionname 
							left join policymetricextendedinfo d on a.policyid = d.policyid and a.assessmentid = d.assessmentid and a.metricid = d.metricid 
							where a.policyid = @policyid and a.assessmentid = @assessmentid
								and ((a.isenabled <> case when b.policyid is null then 0 else 1 end)
									 or 
									(
										 (c.servertype = 'OP' or c.servertype = 'AVM') and (a.reportkey <> b.metricreportkey
																							 or a.reporttext <> b.metricreporttext
																							 or a.severity <> b.metricseveritycode
																							 or a.severityvalues <> b.metricseverityvalues)
									 )
									 or 
									 (
										 (c.servertype = 'ADB') and (c.servertype = d.servertype) and (d.reportkey <> b.metricreportkey
																				 or d.reporttext <> b.metricreporttext
																				 or d.severity <> b.metricseveritycode
																				 or a.severityvalues <> b.metricseverityvalues)
									 ))
					end
					select @isvalid = 0
				end
				else
				begin
					-- use variables to work around sql 2000 syntax limitations
					select @assessmentdate=assessmentdate, @usebaseline=usebaseline 
						from assessment 
						where policyid = @policyid
							and assessmentid = @assessmentid

					insert into @snapshots
						select a.registeredserverid, b.snapshotid
							from #tmp_sqlsecure_getpolicymemberlist a left join serversnapshot b on a.registeredserverid = b.registeredserverid
								and b.snapshotid in (select snapshotid from dbo.getsnapshotlist(@assessmentdate, @usebaseline))
							-- only include null snapshots if the snapshot not found metric is enabled
							where b.snapshotid is not null or exists (select * from policymetric where policyid = @policyid and assessmentid = @assessmentid and metricid = 54 and isenabled = 1)

					if (@debug=1)
						select * from @snapshots

					insert into @snapshotssaved
						select distinct registeredserverid, snapshotid 
							from policyassessment 
							where policyid = @policyid 
								and assessmentid = @assessmentid

					if (@debug=1)
						select * from @snapshotssaved

					if (exists (select * 
									from @snapshots a left join @snapshotssaved b on a.registeredserverid = b.registeredserverid and isnull(a.snapshotid,0) = isnull(b.snapshotid, 0) 
									where b.registeredserverid is null)
						or exists (select * 
									from @snapshotssaved a left join @snapshots b on a.registeredserverid = b.registeredserverid and isnull(a.snapshotid,0) = isnull(b.snapshotid, 0) 
									where b.registeredserverid is null))
					begin
						if (@debug=1)
							print 'snapshots don''t match'
						select @isvalid = 0
					end
				end
			end
		end

		-- drop the temp table used for snapshot comparison
		drop table #tmp_sqlsecure_getpolicymemberlist
	end

	-- it passed all the tests, so it should be valid
	if (@isvalid = -1)
	begin
		if (@debug=1)
			print 'assessment is valid'
		select @isvalid = 1
	end

	select @valid = @isvalid
END

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_isassessmentdatacurrent]   TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


