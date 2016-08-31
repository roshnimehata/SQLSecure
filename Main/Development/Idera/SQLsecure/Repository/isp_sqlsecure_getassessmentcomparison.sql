SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_getassessmentcomparison]'))
drop procedure [dbo].[isp_sqlsecure_getassessmentcomparison]
GO


CREATE procedure [dbo].[isp_sqlsecure_getassessmentcomparison] 
(
	@policyid int,
	@assessmentid1 int,
	@assessmentid2 int,
	@registeredserverid int=0,
	@diffsonly bit=0
)
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Compare two policy assessments and return a table of comparison results by metric and server
   -- 	           
   -- 	           Parameters:
   -- 	             @policyid - the id of the policy to compare assessments on
   -- 	             @assessmentid1 - the id of the first assessment
   -- 	             @assessmentid1 - the id of the first assessment
   -- 	             @registeredserverid - the optional id of a server for a server level comparison
   -- 	             @diffsonly - return only the items with differences
   --
   --

	declare @err int, @msg nvarchar(500)
	declare @programname nvarchar(128), @action nvarchar(32), @category nvarchar(32), @success nvarchar(32), @failure nvarchar(32)
	select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)
	select @action=N'Compare', @category=N'Assessment', @success=N'Success Audit', @failure=N'Failure Audit'

	declare @outtbl table (policyid int,
							assessmentid1 int,
							assessmentid2 int,
							registeredserverid int,
							connectionname nvarchar(400),
							metricid int,
							metricname nvarchar(256),
							metrictype nvarchar(32),
							differencesfound bit,
							diffreportsettings bit,
							diffreportsettingstext nvarchar(1024),
							diffmetricsettings bit,
							diffmetricsettingstext nvarchar(1024),
							difffindings bit,
							difffindingstext nvarchar(1024),
							diffnotes bit,
							diffnotestext nvarchar(1024),

							snapshotid1 int,
							collectiontime1 datetime,
							metricseveritycode1 int,
							metricseverity1 nvarchar(16),
							metricseverityvalues1 nvarchar(4000),
							metricdescription1 nvarchar(4000),
							metricreportkey1 nvarchar(32),
							metricreporttext1 nvarchar(4000),
							severitycode1 int,
							severity1 nvarchar(16),
							currentvalue1 nvarchar(1500),
							thresholdvalue1 nvarchar(1500),
							isexplained1 bit,
							severitycodeexplained1 int,
							notes1 nvarchar(4000),

							snapshotid2 int,
							collectiontime2 datetime,
							metricseveritycode2 int,
							metricseverity2 nvarchar(16),
							metricseverityvalues2 nvarchar(4000),
							metricdescription2 nvarchar(4000),
							metricreportkey2 nvarchar(32),
							metricreporttext2 nvarchar(4000),
							severitycode2 int,
							severity2 nvarchar(16),
							currentvalue2 nvarchar(1500),
							thresholdvalue2 nvarchar(1500),
							isexplained2 bit,
							severitycodeexplained2 int,
							notes2 nvarchar(4000)
							)

	declare @policyname nvarchar(128),
			@valid bit

	-- check to make sure assessment1 exists and the data is current before comparison or invalid data may be returned
	select @policyname = policyassessmentname 
		from SQLsecure.dbo.vwpolicy 
		where 
			policyid = @policyid 
			and assessmentid = @assessmentid1

	if (@policyname is null)
	begin
		set @msg = 'Error: Failed to compare assessments. Unable to retrieve Assessment with  policy id ' + CONVERT(NVARCHAR, @policyid) + ' and assessment id ' + CONVERT(NVARCHAR, @assessmentid1)
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end

	create table #snapshottbl (snapshotid int, registeredserverid int, connectionname nvarchar(400), starttime datetime, status nchar(1), baseline bit, version nvarchar(256))
	declare @assessmentdate datetime, @usebaseline bit

	exec [dbo].[isp_sqlsecure_isassessmentdatacurrent]
			@policyid = @policyid,
			@assessmentid = @assessmentid1,
			@valid = @valid output
	if (@valid = 0)
	begin
		-- will return invalid if there are no snapshots, so make sure there is any valid data for the servers with the selections
		select @assessmentdate=assessmentdate, @usebaseline=usebaseline 
			from assessment 
			where policyid = @policyid
				and assessmentid = @assessmentid1

		insert #snapshottbl
			exec @err = [dbo].[isp_sqlsecure_getpolicysnapshotlist] 
				@policyid = @policyid, 
				@assessmentid = @assessmentid1
		if (exists (select snapshotid from #snapshottbl))
		begin
			drop table #snapshottbl
			set @msg = 'Error: Failed to compare assessments. Assessment "' + @policyname + '" with id ' + CONVERT(NVARCHAR, @policyid) +  + ' and assessment id ' + CONVERT(NVARCHAR, @assessmentid1) + ' does not have current assessment data.'
			exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
			RAISERROR (@msg, 16, 1)
			return -1
		end
	end

	-- check to make sure assessment2 exists and the data is current before comparison or invalid data may be returned
	select @policyname = policyassessmentname 
		from SQLsecure.dbo.vwpolicy 
		where 
			policyid = @policyid 
			and assessmentid = @assessmentid2

	if (@policyname is null)
	begin
		drop table #snapshottbl
		set @msg = 'Error: Failed to compare assessments. Unable to retrieve Assessment with  policy id ' + CONVERT(NVARCHAR, @policyid) + ' and assessment id ' + CONVERT(NVARCHAR, @assessmentid2)
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end

	exec [dbo].[isp_sqlsecure_isassessmentdatacurrent]
			@policyid = @policyid,
			@assessmentid = @assessmentid2,
			@valid = @valid output
	if (@valid = 0)
	begin
		-- will return invalid if there are no snapshots, so make sure there is any valid data for the servers with the selections
		select @assessmentdate=assessmentdate, @usebaseline=usebaseline 
			from assessment 
			where policyid = @policyid
				and assessmentid = @assessmentid2

		insert #snapshottbl
			exec @err = [dbo].[isp_sqlsecure_getpolicysnapshotlist] 
				@policyid = @policyid, 
				@assessmentid = @assessmentid2
		if (exists (select snapshotid from #snapshottbl))
		begin
			drop table #snapshottbl
			set @msg = 'Error: Failed to compare assessments. Assessment "' + @policyname + '" with id ' + CONVERT(NVARCHAR, @policyid) +  + ' and assessment id ' + CONVERT(NVARCHAR, @assessmentid2) + ' does not have current assessment data.'
			exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
			RAISERROR (@msg, 16, 1)
			return -1
		end
	end

	drop table #snapshottbl


	-- get the list of servers for the selected policy
	create table #servertbl (registeredserverid int)

	if (@registeredserverid > 0)
	begin
		insert #servertbl (registeredserverid) values (@registeredserverid)
	end
	else
	begin
		-- add the values from both assessments to include all. There may be duplicates, but that is ok
		insert #servertbl
			exec @err = [dbo].[isp_sqlsecure_getpolicymemberlist] 
				@policyid = @policyid, 
				@assessmentid = @assessmentid1
		insert #servertbl
			exec @err = [dbo].[isp_sqlsecure_getpolicymemberlist] 
				@policyid = @policyid, 
				@assessmentid = @assessmentid2
	end

	insert into @outtbl (
				policyid,
				assessmentid1,
				assessmentid2,
				registeredserverid,
				connectionname,
				metricid,
				metricname,
				metrictype,
				diffreportsettings,
				diffreportsettingstext,
				diffmetricsettings,
				diffmetricsettingstext,
				difffindings,
				difffindingstext,
				diffnotes,
				diffnotestext,
				snapshotid1,
				collectiontime1,
				metricseveritycode1,
				metricseverity1,
				metricseverityvalues1,
				metricdescription1,
				metricreportkey1,
				metricreporttext1,
				severitycode1,
				severity1,
				currentvalue1,
				thresholdvalue1,
				isexplained1,
				severitycodeexplained1, 
				notes1,
				snapshotid2,
				collectiontime2,
				metricseveritycode2,
				metricseverity2,
				metricseverityvalues2,
				metricdescription2,
				metricreportkey2,
				metricreporttext2,
				severitycode2,
				severity2,
				currentvalue2,
				thresholdvalue2,
				isexplained2,
				severitycodeexplained2, 
				notes2
					)
		select 
				m1.policyid,
				m1.assessmentid,
				m2.assessmentid,
				isnull(a1.registeredserverid, a2.registeredserverid),
				isnull(a1.connectionname, a2.connectionname),
				m1.metricid,
				isnull(a1.metricname, a2.metricname),
				isnull(a1.metrictype, a2.metrictype), 
				case when (m1.severity <> m2.severity or m1.reportkey <> m2.reportkey or m1.reporttext <> m2.reporttext) then 1 else 0 end,
				ltrim(case when (m1.severity <> m2.severity) then N'Risk Level' else N'' end
						+ case when (m1.reportkey <> m2.reportkey) then N' External Cross Reference' else N'' end 
						+ case when (m1.reporttext <> m2.reporttext) then N' Report Text' else N'' end),
				case when (m1.isenabled <> m2.isenabled or m1.severityvalues <> m2.severityvalues) then 1 else 0 end,
				case when (m1.isenabled <> m2.isenabled) then N'Enabled' else case when (m1.severityvalues <> m2.severityvalues) then N'Criteria' else N'' end end,
				case when (isnull(a1.currentvalue, 1) <> isnull(a2.currentvalue, 2)) then 1 else 0 end,
				case when (isnull(a1.currentvalue, 1) <> isnull(a2.currentvalue, 2)) then N'Findings' else N'' end,
				case when (isnull(convert(int,n1.isexplained),-1) <> isnull(convert(int,n2.isexplained),-1) or isnull(n1.notes,N'') <> isnull(n2.notes,N'')) then 1 else 0 end,
				ltrim(case when (isnull(convert(int,n1.isexplained),-1) <> isnull(convert(int,n2.isexplained),-1)) then N'Explained' else N'' end
						+ case when (isnull(n1.notes,N'') <> isnull(n2.notes,N'')) then N' Notes' else N'' end) as diffnotestext,
				a1.snapshotid,
				a1.collectiontime,
				a1.metricseveritycode,
				a1.metricseverity,
				a1.metricseverityvalues,
				a1.metricdescription,
				a1.metricreportkey,
				a1.metricreporttext,
				a1.severitycode,
				a1.severity,
				a1.currentvalue,
				a1.thresholdvalue,
				n1.isexplained,
				a1.severitycode + case when a1.severitycode > 0 and isnull(n1.isexplained,0) = 0 then 10 else 0 end,
				n1.notes,
				a2.snapshotid,
				a2.collectiontime,
				a2.metricseveritycode,
				a2.metricseverity,
				a2.metricseverityvalues,
				a2.metricdescription,
				a2.metricreportkey,
				a2.metricreporttext,
				a2.severitycode,
				a2.severity,
				a2.currentvalue,
				a2.thresholdvalue,
				n2.isexplained,
				a2.severitycode + case when a2.severitycode > 0 and isnull(n2.isexplained,0) = 0 then 10 else 0 end,
				n2.notes
			from (policymetric m1 
						left join policyassessment a1 
							on a1.policyid = m1.policyid 
								and a1.assessmentid = m1.assessmentid 
								and a1.metricid=m1.metricid 
						left join policyassessmentnotes n1 
							on a1.policyid = n1.policyid 
								and a1.assessmentid = n1.assessmentid 
								and a1.metricid = n1.metricid 
								and a1.snapshotid = n1.snapshotid
					) 
					inner join (policymetric m2
						left join policyassessment a2 
							on a2.policyid = m2.policyid 
								and a2.assessmentid = m2.assessmentid 
								and a2.metricid=m2.metricid 
						left join policyassessmentnotes n2 
							on a2.policyid = n2.policyid 
								and a2.assessmentid = n2.assessmentid 
								and a2.metricid = n2.metricid 
								and a2.snapshotid = n2.snapshotid
								) 
						on m1.policyid = m2.policyid 
							and m1.metricid=m2.metricid 
							and isnull(a1.registeredserverid,a2.registeredserverid) = isnull(a2.registeredserverid,a1.registeredserverid)
			where m1.policyid = @policyid
				and m1.assessmentid = @assessmentid1
				and m2.assessmentid = @assessmentid2
				and (m1.isenabled = 1 or m2.isenabled = 1)
				and (a1.registeredserverid in (select registeredserverid from #servertbl)
						or a2.registeredserverid in (select registeredserverid from #servertbl))

	update @outtbl set differencesfound = diffreportsettings | diffmetricsettings | difffindings | diffnotes

	if (@diffsonly = 1)
		select * from @outtbl where differencesfound = 1
	else
		select * from @outtbl

	drop table #servertbl
GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_getassessmentcomparison] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
