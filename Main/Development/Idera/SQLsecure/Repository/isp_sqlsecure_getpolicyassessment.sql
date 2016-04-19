SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_getpolicyassessment]'))
drop procedure [dbo].[isp_sqlsecure_getpolicyassessment]
GO

CREATE procedure [dbo].[isp_sqlsecure_getpolicyassessment]
(
	@policyid int,
	@assessmentid int = null,		-- default to policy settings for backward compatibility
	@registeredserverid int=0,
	@alertsonly bit=0,
	@usebaseline bit=0,
	@rundate datetime=null
)
WITH ENCRYPTION
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --             Perform a security assessment for the policy or assessment using the configured metrics
   --				for the servers in the policy and return a table of results flagging all metrics that exceed the configured thresholds
   --
   --				If @registeredserverid is 0, then all servers in the policy will be evaluated
   --					otherwise, only the selected server will have the policy applied to it
   --				If @alertsonly is 1, only return the risks
   --				If the assessmentid is for a saved assessment, then usebaseline and rundate parameters are ignored
   --					and the values are pulled from the assessment table

	declare @outtbl table (snapshotid int,
							registeredserverid int,
							connectionname nvarchar(400),
							collectiontime datetime,
							metricid int,
							metricname nvarchar(256),
							metrictype nvarchar(32),
							metricseveritycode int,
							metricseverity nvarchar(16),
							metricseverityvalues nvarchar(4000),
							metricdescription nvarchar(4000),
							metricreportkey nvarchar(32),
							metricreporttext nvarchar(4000),
							severitycode int,
							severity nvarchar(16),
							currentvalue nvarchar(1500),
							thresholdvalue nvarchar(1500)
							)
		 create table #tempdetails
              (
                policyid int,
                assessmentid int,
                metricid int,
                snapshotid int,
                detailfinding varchar(2048),
                databaseid int null,
                objecttype varchar(5),
                objectid int null,
                objectname varchar(400)
              ) 
	declare @err int,
			@sevcodeok int,
			@valid bit,
			@isadmin bit,
			@debug bit,
			@runtime datetime,
			@serverruntime datetime
	select @sevcodeok=0, 
			@valid = 0, 
			@isadmin = 0,
			@debug = 0

	if (@debug=1)
	begin
		set nocount off
		print '@policyid='+convert(nvarchar,@policyid)+', @assessmentid='+convert(nvarchar,isnull(@assessmentid,-1))+', @registeredserverid='+convert(nvarchar,isnull(@registeredserverid,-1))
		print '@alertsonly='+convert(nvarchar,isnull(@alertsonly,-1))+', @usebaseline='+convert(nvarchar,isnull(@usebaseline,-1))+', @rundate='+convert(nvarchar,isnull(@rundate,-1))
	end
	else
	begin
		SET NOCOUNT ON
	end

	exec @isadmin = [isp_sqlsecure_isadmin]

	-- replace the settings assessmentid with the current assessmentid
	if (@assessmentid is null or exists (select * from assessment where assessmentid = @assessmentid and assessmentstate = N'S'))
	begin
		select @assessmentid = null		-- make sure it is null so the current assessment will be created if it doesn't exist
		select @assessmentid = assessmentid from assessment where policyid = @policyid and assessmentstate = N'C'
		if (@debug=1)
		begin
			set nocount off
			print 'Policy passed, @assessmentid='+convert(nvarchar,isnull(@assessmentid,-1))
		end
	end

	-- if it still wasn't found, then create the current assessment
	if (@assessmentid is null and @isadmin = 1)
	begin
		if (@debug=1)
		begin
			set nocount off
			print 'creating new current assessment'
		end
		EXEC [dbo].[isp_sqlsecure_createassessmentfrompolicy]
				@policyid = @policyid,
				@assessmentid = null,
				@type = N'C',
				@copy = 2,
				@newassessmentid = @assessmentid output
		if (@debug=1)
		begin
			set nocount off
			print 'New @assessmentid='+convert(nvarchar,@assessmentid)
		end
	end

	--get the id of the settings to update with selection criteria
	declare @state nchar(1), @settingsid int
	select @state = assessmentstate from assessment where policyid = @policyid and assessmentid = @assessmentid

	if (@state is null)
	begin
		select @state = N'S', @assessmentid = assessmentid from assessment where policyid = @policyid and assessmentstate = N'S'
	end
	if (@state = N'S')
		select @settingsid = @assessmentid
	else if (@state = N'C')
		select @settingsid = assessmentid from assessment where policyid = @policyid and assessmentstate = N'S'

	if (@debug=1)
	begin
		print '@state=' + @state
		print '@assessmentid=' + convert(nvarchar, isnull(@assessmentid,-1))
		print '@settingsid=' + convert(nvarchar, isnull(@settingsid,-1))
	end
	if (@isadmin = 1)
	begin
		if (@settingsid is not null)
			update [assessment] 
				set assessmentdate=@rundate, usebaseline=@usebaseline
				where policyid = @policyid
						and assessmentid = @settingsid

		-- check to make sure the assessment data is current.
		exec [dbo].[isp_sqlsecure_isassessmentdatacurrent]
				@policyid = @policyid,
				@assessmentid = @assessmentid,
				@valid = @valid output
	end

	if (@debug=1)
	begin
		set nocount off
		print '@valid='+convert(nvarchar,@valid)
	end

	-- get the list of servers for the selected policy
	create table #servertbl (registeredserverid int)
	insert #servertbl
		exec @err = [dbo].[isp_sqlsecure_getpolicymemberlist] 
			@policyid = @policyid, 
			@assessmentid = @assessmentid

	declare @returnservertbl table (registeredserverid int)
	insert into @returnservertbl
		select registeredserverid from #servertbl
	if (@registeredserverid > 0)
	begin
		delete from @returnservertbl where registeredserverid != @registeredserverid
	end

	if (@valid = 1)
	begin
		insert into @outtbl (
						snapshotid,
						registeredserverid,
						connectionname,
						collectiontime,
						metricid,
						metricname,
						metrictype,
						metricseveritycode,
						metricseverity,
						metricseverityvalues,
						metricdescription,
						metricreportkey,
						metricreporttext,
						severitycode,
						severity,
						currentvalue,
						thresholdvalue)
			( SELECT 	snapshotid,
						registeredserverid,
						connectionname,
						collectiontime,
						metricid,
						metricname,
						metrictype,
						metricseveritycode,
						metricseverity,
						metricseverityvalues,
						metricdescription,
						metricreportkey,
						metricreporttext,
						severitycode,
						severity,
						currentvalue,
						thresholdvalue
					FROM policyassessment
					WHERE policyid = @policyid 
					    and assessmentid = @assessmentid
						and registeredserverid in (select registeredserverid from @returnservertbl) )
	end
	else
	begin
		begin transaction

		if (@isadmin = 1)
		begin
			delete policyassessmentdetail where policyid = @policyid and assessmentid = @assessmentid
			delete policyassessment where policyid = @policyid and assessmentid = @assessmentid
		end

		-- if it is a current assessment, then refresh the metric settings
		if (@state = N'C')
		begin
			if (@isadmin = 1)
			begin
				if (@debug=1)
				begin
					print 'Refreshing assessment from policy settings @assessmentid='+convert(nvarchar,@assessmentid)
				end
				EXEC [dbo].[isp_sqlsecure_createassessmentfrompolicy]
						@policyid = @policyid,
						@assessmentid = @assessmentid,
						@type = N'C',
						@copy = 3,
						@newassessmentid = @assessmentid output
			end
		end
		else if (@state in (N'D', N'P'))
		begin
			select @rundate=assessmentdate, 
					@usebaseline=usebaseline 
				from assessment 
				where policyid = @policyid 
					and assessmentid = @assessmentid
		end

		if exists (select * from #servertbl)
		begin
			-- create constants for use on metrics
			declare @everyonesid varbinary(85), @sysadminsid varbinary(85), @builtinadminsid varbinary(85)
			select @everyonesid=0x01010000000000010000000000000000000000000000000000000000000000000000000000000000,
					@sysadminsid=0x03,
					@builtinadminsid=0x01020000000000052000000020020000


			-- get the list of metrics for the policy
			declare @metricid int, @metricname nvarchar(256), @metrictype nvarchar(32),
					@metricdescription nvarchar(1024), @metricreportkey nvarchar(32), @metricreporttext nvarchar(4000),
					@severity int, @severityvalues nvarchar(4000), @configuredvalues nvarchar(4000)

			declare metriccursor cursor static for 
				select metricid, metricname, metrictype, metricdescription, reportkey, reporttext,
						severity, severityvalues
					from vwpolicymetric
				where  policyid = @policyid
					and assessmentid = @assessmentid
					and isenabled = 1
			open metriccursor

			-- process the snapshots for each metric
			declare @snapshotid int, @connection nvarchar(400), 
					@snapshottime datetime, @status nchar(1), @baseline nchar(1), @collectorversion nvarchar(32), 
					@version nvarchar(256), @os nvarchar(512), @authentication nchar(1),
					@loginauditmode nvarchar(20), @c2audittrace nchar(1), @crossdb nchar(1),
					@proxy nchar(1), @remotedac nchar(1), @remoteaccess nchar(1),
					@startupprocs nchar(1), @sqlmail nchar(1), @databasemail nchar(1),
					@ole nchar(1), @webassistant nchar(1), @xp_cmdshell nchar(1),
					@agentmailprofile nvarchar(128), @hide nchar(1), @agentsysadmin nchar(1),
					@dc nchar(1), @replication nchar(1), @sapassword nchar(1),
					@systemtables nchar(1), @systemdrive nchar(2), @adhocqueries nchar(1),
					@weakpasswordenabled nchar(1)

			declare snapcursor cursor static for
					select a.snapshotid, a.registeredserverid, a.connectionname, 
							a.endtime, a.status, a.baseline, a.collectorversion,
							a.version, a.os, a.authenticationmode,
							a.loginauditmode, a.enablec2audittrace, a.crossdbownershipchaining,
							a.enableproxyaccount, a.remoteadminconnectionsenabled, a.remoteaccessenabled,
							a.scanforstartupprocsenabled, a.sqlmailxpsenabled, a.databasemailxpsenabled,
							a.oleautomationproceduresenabled, a.webassistantproceduresenabled, a.xp_cmdshellenabled,
							a.agentmailprofile, a.hideinstance, a.agentsysadminonly,
							a.serverisdomaincontroller, a.replicationenabled, a.sapasswordempty,
							a.allowsystemtableupdates, a.systemdrive, a.adhocdistributedqueriesenabled,
							a.isweakpassworddetectionenabled
					from serversnapshot a,
						dbo.getsnapshotlist(@rundate, @usebaseline) b
					where a.registeredserverid in (select registeredserverid from #servertbl)
							and a.snapshotid = b.snapshotid
			open snapcursor

			declare @sevcode int, @sev nvarchar(16), @metricval nvarchar(1500), @metricthreshold nvarchar(1500)
			declare @loginname nvarchar(200), @intval int, @intval2 int
			declare @strval nvarchar(1024), @strval2 nvarchar(1024), @strval3 nvarchar(1024), @sql nvarchar(4000)
			declare @tblval table (val nvarchar(1024) collate database_default)
			-- store sysadmin users in table that can be used with dynamic sql for multiple checks
			create table #sysadminstbl (id int, name nvarchar(256) collate database_default)

			fetch next from snapcursor into @snapshotid, @registeredserverid, @connection, 
											@snapshottime, @status, @baseline, @collectorversion,
											@version, @os, @authentication,
											@loginauditmode, @c2audittrace, @crossdb,
											@proxy, @remotedac, @remoteaccess,
											@startupprocs, @sqlmail, @databasemail,
											@ole, @webassistant, @xp_cmdshell,
											@agentmailprofile, @hide, @agentsysadmin,
											@dc, @replication, @sapassword,
											@systemtables, @systemdrive, @adhocqueries,
											@weakpasswordenabled

			while @@fetch_status = 0
			begin
				if (@debug=1)
				begin
					select @serverruntime=getdate()
					print convert(nvarchar,@serverruntime,8) + ': @connection=' + @connection
					print '@snapshotid=' + convert(nvarchar, @snapshotid)
				end

				-- save a list of sysadmin members in this snapshot for use by multiple metrics
				delete from #sysadminstbl
				insert into #sysadminstbl
					select distinct a.memberprincipalid, c.name 
						from serverrolemember a, serverprincipal b, serverprincipal c 
						where a.snapshotid = @snapshotid 
							and a.snapshotid = b.snapshotid 
							and a.principalid = b.principalid 
							and b.sid = @sysadminsid
							and a.snapshotid = c.snapshotid 
							and a.memberprincipalid = c.principalid

				fetch first from metriccursor into @metricid, @metricname, @metrictype,
													@metricdescription, @metricreportkey, @metricreporttext,
													@severity, @severityvalues

				while @@fetch_status = 0
				begin
					begin try
						if (@debug=1)
						begin
							select @runtime=getdate()
							print convert(nvarchar,@runtime,8) + ': @metricid=' + convert(nvarchar, @metricid)
						end
						-- This sets the metric so it will not be displayed if no value is found
						--     each metric should handle this situation appropriately
						select @err=0, @sevcode=-1, @metricval=N'', @metricthreshold=N'', @configuredvalues=@severityvalues
						-- clean up old values
						select @intval=0, @intval2=0, @strval=N'', @strval2=N'', @strval3=N'', @sql=N''
						delete from @tblval

						-- Collection Time
						if (@metricid = 1)
						begin
							-- remove the quotes surrounding the value and test for numeric
							set @severityvalues=replace(@severityvalues,'''','')
							if (isnumeric(@severityvalues) = 1)
								select @intval = cast(@severityvalues as int)
							set @err = @@ERROR
							if (@err = 0)
							begin
								select @intval2 = datediff(d, @snapshottime, isnull(@rundate, getdate()))
								if (@intval2 <= @intval)
									select @sevcode=@sevcodeok,
										@metricval = N'Audit data is within the selected date range.'
								else
									select @sevcode=@severity,
										@metricval = convert(nvarchar, @snapshottime) + N': Audit data is ' + convert(nvarchar, @intval2) + N' days from the selected date.'
							end

							select @metricthreshold = N'Audit data is acceptable if within ' + @severityvalues + N' days of the selected date.'
						end
						-- SQL Server version
						else if (@metricid = 2)
						begin
							--make sure the version doesn't start with a 0 before comparing
							select @strval = case when left(@version,1) = '0' then substring(@version,2,len(@version)-1) else @version end

                            --find the matching entry based on major and minor version
                            select @intval = charindex(''''+left(@strval,charindex('.',@strval, charindex('.',@strval) + 1)),@severityvalues)

							select @intval2 = 1	-- force a finding if it can't be parsed

							if (@intval > 0)
							begin
								-- set the threshold to the matching version
								select @metricthreshold = substring(@severityvalues, @intval + 1,
																	charindex('''',@severityvalues, @intval + 1) - @intval - 1)
								declare @v1 int, @v2 int, @v3 int, @v4 int,
										@t1 int, @t2 int, @t3 int, @t4 int,
										@ver nvarchar(20)
								select @v1=0,@v2=0,@v3=0,@v4=0,@t1=0,@t2=0,@t3=0,@t4=0

								--parse the server version into component numbers to compare string values of different lengths
								select @intval = charindex('.',@strval)
								if (@intval > 1)
								begin
									select @ver = left(@strval, @intval - 1)
									select @v1 = case when isnumeric(@ver)=1 then convert(int, @ver) else 0 end,
											@strval = right(@strval, len(@strval) - @intval)

									select @intval = charindex('.',@strval)
									if (@intval > 1)
									begin
										select @v2 = convert(int, left(@strval, @intval - 1)),
												@strval = right(@strval, len(@strval) - @intval)

										select @intval = charindex('.',@strval)
										if (@intval > 1)
										begin
											select @v3 = convert(int, left(@strval, @intval - 1)),
													@strval = right(@strval, len(@strval) - @intval)

											if (len(@strval) > 0 and charindex('.',@strval) = 0 and isnumeric(@strval)=1)
												select @v4 = convert(int, @strval)
										end
										else
											select @v3 = case when isnumeric(@strval) = 1 and charindex('.',@strval) = 0 then convert(int, @strval) else 0 end
									end
									else
										select @v2 = case when isnumeric(@strval) = 1 and charindex('.',@strval) = 0 then convert(int, @strval) else 0 end
								end
								else
									select @v1 = case when isnumeric(@strval) = 1 and charindex('.',@strval) = 0 then convert(int, @strval) else 0 end

								--parse the threshold version into component numbers to compare string values of different lengths
								select @strval=@metricthreshold
								select @intval = charindex('.',@strval)
								if (@intval > 1)
								begin
									select @ver = left(@strval, @intval - 1)
									select @t1 = case when isnumeric(@ver) = 1 and charindex('.',@ver) = 0 then convert(int, @ver) else 0 end,
											@strval = right(@strval, len(@strval) - @intval)

									select @intval = charindex('.',@strval)
									if (@intval > 1)
									begin
										select @t2 = convert(int, left(@strval, @intval - 1)),
												@strval = right(@strval, len(@strval) - @intval)

										select @intval = charindex('.',@strval)
										if (@intval > 1)
										begin
											select @t3 = convert(int, left(@strval, @intval - 1)),
													@strval = right(@strval, len(@strval) - @intval)

											if (len(@strval) > 0 and charindex('.',@strval) = 0 and isnumeric(@strval)=1)
												select @t4 = convert(int, @strval)
										end
										else
											select @t3 = case when isnumeric(@strval) = 1 and charindex('.',@strval) = 0 then convert(int, @strval) else 0 end
									end
									else
										select @t2 = case when isnumeric(@strval) = 1 and charindex('.',@strval) = 0 then convert(int, @strval) else 0 end
								end
								else
									select @t1 = case when isnumeric(@strval) = 1 and charindex('.',@strval) = 0 then convert(int, @strval) else 0 end

								--compare level by level to see if there is a mismatch
								if ((@t1 > @v1) 
									or (@t1 = @v1 and @t2 > @v2) 
									or (@t1 = @v1 and @t2 = @v2 and @t3 > @v3) 
									or (@t1 = @v1 and @t2 = @v2 and @t3 = @v3 and @t4 > @v4))
									set @intval2 = 1
								else
									set @intval2 = 0
							end

							if (@intval2 = 0)
								select @sevcode=@sevcodeok
							else
								select @sevcode=@severity

							select @metricval = N'Current version is ' + @version
							select @metricthreshold = N'Acceptable levels for each SQL Server version are ' + @severityvalues + N' and above.'
						end
						-- SQL Authentication Enabled
						else if (@metricid = 3)
						begin
							select @severityvalues = N'M'
							if (@authentication <> @severityvalues)
								select @sevcode=@sevcodeok
							else
								select @sevcode=@severity

							select @metricval = dbo.getauthenticationmodename(@authentication)
							select @metricthreshold = N'Server is vulnerable if ''' + dbo.getauthenticationmodename(@severityvalues) + N''' is enabled.'
						end
						-- Login Audit Level
						else if (@metricid = 4)
						begin
							if (charindex('''' + @loginauditmode + '''', @severityvalues) > 0)
								select @sevcode=@sevcodeok
							else
								select @sevcode=@severity
							-- convert values to display values
							select @strval = @severityvalues, @metricthreshold='',
								@intval = charindex(''',''',@strval)
							while (@intval > 0)
							begin
								if len(@metricthreshold) > 0
									select @metricthreshold = @metricthreshold + ','
								select @metricthreshold = @metricthreshold + '''' + dbo.getloginauditmodename(substring(@strval,2,@intval-2)) + ''''
								select @strval = substring(@strval,@intval+2, len(@strval)-(@intval+1)),
									@intval = charindex(''',''',@strval)
							end
							if len(@strval) > 0
							begin
								if len(@metricthreshold) > 0
									select @metricthreshold = @metricthreshold + ', '
								select @metricthreshold = @metricthreshold + '''' + dbo.getloginauditmodename(substring(@strval,2,len(@strval)-2)) + ''''
							end

							select @metricval = dbo.getloginauditmodename(@loginauditmode)
							select @metricthreshold = N'Login auditing is acceptable if set to ' + @metricthreshold + '.'
						end
						-- Cross Database Ownership Chaining Enabled
						else if (@metricid = 5)
						begin
							select @severityvalues = N'Y'
							if (@crossdb <> @severityvalues)
								select @sevcode=@sevcodeok
							else
								select @sevcode=@severity

							select @metricval = dbo.getyesnotext(@crossdb)
							select @metricthreshold = N'Server is vulnerable if Cross Database Ownership Chaining is enabled.'
						end
						-- Guest User Enabled
						else if (@metricid = 6)
						begin
							select @sql = N'declare dbcursor cursor static for
												select databasename
													from sqldatabase
													where snapshotid = ' + convert(nvarchar, @snapshotid) + N'
														and databasename not in (' + @severityvalues + N')
														and guestenabled = ''Y''
													order by databasename'
							exec (@sql)
							open dbcursor
							select @intval2 = 0
							fetch next from dbcursor into @strval
							while @@fetch_status = 0
							begin
								if (@intval2 = 1 or len(@metricval) + len(@strval) > 1010)
								begin
									if @intval2 = 0
										select @metricval = @metricval + N', more...',
												@intval2 = 1
								end
								else
									select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval + N''''

								if (@isadmin = 1)
									insert into policyassessmentdetail ( policyid,
																		 assessmentid,
																		 metricid,
																		 snapshotid,
																		 detailfinding,
																		 databaseid,
																		 objecttype,
																		 objectid,
																		 objectname )
																values ( @policyid,
																		 @assessmentid,
																		 @metricid,
																		 @snapshotid,
																		 N'Database with Guest user access: ''' + @strval + N'''',
																		 null, -- database ID,
																		 N'DB', -- object type
																		 null, -- object id
																		 @strval )
															         
								fetch next from dbcursor into @strval
							end

							if (len(@metricval) = 0)
								select @sevcode=@sevcodeok,
										@metricval = N'None found.'
							else
								select @sevcode=@severity,
										@metricval = N'Databases with Guest user access: ' + @metricval

							select @metricthreshold = N'Server is vulnerable if Guest user access is available on databases other than: ' + @severityvalues

							close dbcursor
							deallocate dbcursor
						end
						-- Suspect Logins
						else if (@metricid = 7)
						begin
							-- This should return the same results as the report SP
							select @severityvalues = N'Y'
							select @intval=count(*)
								from serverprincipal a 
									INNER JOIN windowsaccount b ON a.snapshotid = b.snapshotid AND a.sid = b.sid
									LEFT JOIN ancillarywindowsgroup c ON a.snapshotid = c.snapshotid AND a.name = c.windowsgroupname
								where a.snapshotid = @snapshotid
									AND a.type IN ('G', 'U')	-- Principal type is Windows Group or User
									AND b.state = 'S'			-- State is suspect
									AND c.windowsgroupname IS NULL	-- Account is not OS controlled well-known

							if (@intval = 0)
								select @sevcode=@sevcodeok,
										@strval = N'N'
							else
								select @sevcode=@severity,
										@strval = N'Y'

							select @metricval = dbo.getyesnotext(@strval)
							select @metricthreshold = N'Server is vulnerable if any Windows Accounts have permissions on the server, but could not be verified with Active Directory.'
						end
						-- C2 Audit Trace Enabled
						else if (@metricid = 8)
						begin
							select @strval = @c2audittrace, @severityvalues = N'Y'
							if (@strval = @severityvalues)
								select @sevcode=@sevcodeok
							else
								select @sevcode=@severity

							select @metricval = dbo.getyesnotext(@strval)
							select @metricthreshold = N'Server is vulnerable if the C2 audit trace is not enabled.'
						end
						-- Proxy Account Enabled
						else if (@metricid = 9)
						begin
							select @strval = @proxy, @severityvalues = N'N'
							if (@strval = @severityvalues)
								select @sevcode=@sevcodeok
							else
								select @sevcode=@severity

							select @metricval = dbo.getyesnotext(@strval)
							select @metricthreshold = N'Server is vulnerable if the proxy account is enabled.'
						end
						-- DAC Remote Access Enabled
						else if (@metricid = 10)
						begin
							select @strval = @remotedac, @severityvalues = N'N'
							-- this is a 2005 and up only value, so just mark it ok in 2000
							if (@version > N'9.' or @version < N'6.')
								if (@strval = @severityvalues)
									select @sevcode=@sevcodeok
								else
									select @sevcode=@severity
							else 
								select @sevcode=@sevcodeok, @strval=N'N/A'

							select @metricval = dbo.getyesnotext(@strval)
							select @metricthreshold = N'Server is vulnerable if remote access through a Dedicated Administrator Connection is enabled.'
						end
						-- Integration Services
						else if (@metricid = 11)
						begin
							if  (len(@severityvalues) > 0)
							begin
								create table #roletbl (rolename nvarchar(128) collate database_default)

								insert into #roletbl
									exec @err = [dbo].[isp_sqlsecure_getmsdbvalidroleslist]

								select @sql = N'declare proccursur cursor static for
													select a.name as objectname, c.name as granteename, b.permission
														from databaseobject a,
															databaseobjectpermission b,
															databaseprincipal c
														where a.snapshotid = ' + convert(nvarchar, @snapshotid) + N' 
															and a.dbid = 4 and a.type = N''P''
															and a.name in (' + @severityvalues + N')
															and a.snapshotid = b.snapshotid
															and a.dbid = b.dbid
															and a.objectid = b.objectid
															and b.snapshotid = c.snapshotid
															and b.dbid = c.dbid
															and (b.isgrant = N''Y'' or b.isgrantwith = N''Y'')
															and b.grantee = c.uid
															and c.name not in (select rolename from #roletbl)
														order by objectname, granteename'

								exec (@sql)
								open proccursur

								select @intval2 = 0
								fetch next from proccursur into @strval, @strval2, @strval3
								while @@fetch_status = 0
								begin
									if (@intval2 = 1 or len(@metricval) + len(@strval) > 1010)
									begin
									if @intval2 = 0
										select @metricval = @metricval + N', more...',
												@intval2 = 1
									end
									else
										select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + @strval3 + N' granted to ''' + @strval2 + N''' on ''' + @strval + N'''' 

									if (@isadmin = 1)
										insert into policyassessmentdetail ( policyid,
																			 assessmentid,
																			 metricid,
																			 snapshotid,
																			 detailfinding,
																			 databaseid,
																			 objecttype,
																			 objectid,
																			 objectname )
																	values ( @policyid,
																			 @assessmentid,
																			 @metricid,
																			 @snapshotid,
																			 N'Permissions on stored procedure found: ' + @strval3 + N' granted to ''' + @strval2 + N''' on ''' + @strval + N'''',
																			 4, -- database ID,
																			 N'P', -- object type
																			 null,
																			 @strval )
															         
									fetch next from proccursur into @strval, @strval2, @strval3
								end
								close proccursur
								deallocate proccursur

								if (len(@metricval) = 0)
									select @sevcode=@sevcodeok,
											@metricval = N'No permissions found.'
								else
									select @sevcode=@severity,
											@metricval = N'Permissions on stored procedures found: ' + @metricval

								drop table #roletbl
							end
							else
								select @sevcode=@sevcodeok,
										@metricval = N'No list of Integration Services stored procedures was provided.'

							select @metricthreshold = N'Server is vulnerable if permissions have been granted on any of these stored procedures: ' + @severityvalues
						end
						-- 'OLAP SQL Authentication Enabled
						else if (@metricid = 12)
						begin
							-- this metric has been removed, but a placeholder is left here so it won't be reused
							select @err=0, @sevcode=-1, @metricval=N'', @metricthreshold=@severityvalues
						end
						-- SQL Mail Enabled
						else if (@metricid = 13)
						begin
							-- test for 2005 or later
							if ((@version > N'9.' or @version < N'6.') and @sqlmail = N'N' and @databasemail = N'N')
							begin
								select @sevcode=@sevcodeok,
										@metricval=N'SQL Mail is not enabled.'
							end
							else
							begin
								-- This should return the same results as the report SP
								select @severityvalues = N'Y'
								select @intval=count(*)
									from databaseobject
									where snapshotid = @snapshotid
										and [type] = 'X'
										and [name] like 'xp_%mail'

								if (@intval = 0)
									select @sevcode=@sevcodeok,
											@metricval = N'No SQL Mail stored procedures were found.'
								else
								begin
									select @sevcode=@severity,
											@metricval = N'SQL Mail stored procedures found'
									if (@sqlmail = @severityvalues)
										select @metricval = @metricval + N' and SQL Mail is enabled'
									if (@databasemail = @severityvalues)
										select @metricval = @metricval + N' and Database Mail is enabled'
								end
								select @metricval = @metricval + '.'
							end

							select @metricthreshold = N'Server is vulnerable if Sql Mail stored procedures like ''xp_%mail'' are found and SQL Mail or Database Mail are enabled (Sql 2005 only).'
						end
						-- SQL Agent Mail Enabled
						else if (@metricid = 14)
						begin
							if (len(rtrim(@agentmailprofile)) = 0)
								select @sevcode=@sevcodeok,
										@metricval = N'SQL Agent Mail profile not found.'
							else
								select @sevcode=@severity,
										@metricval = N'SQL Agent Mail profile found: ' + @agentmailprofile

							select @metricthreshold = N'Server is vulnerable if a Sql Agent Mail profile exists.'
						end
						-- Sample Databases Exist
						else if (@metricid = 15)
						begin
							if len(@severityvalues) > 0
							begin
								select @sql = N'declare dbcursor cursor static for
													select [databasename]
														from vwdatabases 
														where snapshotid = ' + convert(nvarchar, @snapshotid) + N'
															and [databasename] in (' + @severityvalues + N')
														order by [databasename]'
								exec (@sql)
								open dbcursor

								select @strval = '', @intval2 = 0
								fetch next from dbcursor into @strval
								while @@fetch_status = 0
								begin
									if (@intval2 = 1 or len(@metricval) + len(@strval) > 1010)
									begin
										if @intval2 = 0
											select @metricval = @metricval + N', more...',
													@intval2 = 1
									end
									else
										select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval + N''''

									if (@isadmin = 1)
										insert into policyassessmentdetail ( policyid,
																			 assessmentid,
																			 metricid,
																			 snapshotid,
																			 detailfinding,
																			 databaseid,
																			 objecttype,
																			 objectid,
																			 objectname )
																	values ( @policyid,
																			 @assessmentid,
																			 @metricid,
																			 @snapshotid,
																			 N'Sample database found: ''' + @strval + N'''',
																			 null, -- database ID,
																			 N'DB', -- object type
																			 null,
																			 @strval )

									fetch next from dbcursor into @strval
								end

								close dbcursor
								deallocate dbcursor

								if (len(@metricval) = 0)
									select @sevcode=@sevcodeok,
											@metricval = N'None found.'
								else
								begin
									select @sevcode=@severity,
											@metricval = N'Sample databases found: ' + @metricval
								end
							end
							else
								select @sevcode=@sevcodeok,
										@metricval = N'No sample databases were provided.'

							select @metricthreshold = N'Server is vulnerable if these sample databases exist: ' + @severityvalues
						end
						-- sa Account Exists
						else if (@metricid = 16)
						begin
								-- only apply this check if the version is 2005 or greater
								if (@version > N'9.' or @version < N'6.')
								begin
									-- check to make sure the sa account is either disabled or renamed
									select @severityvalues = N'N'
									select @metricval=[name], @strval=[disabled] from serverprincipal where snapshotid = @snapshotid and sid = 0x01
									if (lower(@metricval) = N'sa' and @strval = @severityvalues)
										select @sevcode=@severity
									else
										select @sevcode=@sevcodeok

									select @metricval = N'The sa account is named ''' + @metricval + N'''' + case when @strval = @severityvalues then N'.' else N' and is enabled.' end
								end
								else
								begin
										select @sevcode = @sevcodeok,
												@metricval = N'The sa account is always enabled on SQL Server 2000.'
								end

								select @metricthreshold = N'Server is vulnerable if the sa account has not been renamed and is enabled.'
						end
						-- sa Account Blank Password
						else if (@metricid = 17)
						begin
							select @strval = @sapassword, @severityvalues = N'N'
							if (@strval = @severityvalues)
								select @sevcode=@sevcodeok
							else
								select @sevcode=@severity

							select @metricval = dbo.getyesnotext(@strval)
							select @metricthreshold = N'Server is vulnerable if the sa account has a blank password.'
						end
						-- System Table Updates
						else if (@metricid = 18)
						begin
							select @strval = @systemtables, @severityvalues = N'N'
							if (@strval = @severityvalues)
								select @sevcode=@sevcodeok
							else
								select @sevcode=@severity

							select @metricval = dbo.getyesnotext(@strval)
							select @metricthreshold = N'Server is vulnerable if System Table Updates are enabled.'
						end
						-- Everyone System Table Access
						else if (@metricid = 19)
						begin
							-- This uses the same logic as the report SP
							if not exists (select * from windowsaccount where snapshotid = @snapshotid and sid=@everyonesid)
								select @sevcode=@sevcodeok,
										@metricval = N'Everyone account not found on server.'
							else
							begin
								-- this code is based on getuserpermissions to setup the list of db users
								create table #logintbl (sid varbinary(85))

								-- insert everyone user in the table, just in case even though it shouldn't
								insert into #logintbl (sid) values (@everyonesid)

								insert into #logintbl
									exec @err = [dbo].[isp_sqlsecure_getwindowsgroupparents]
										@snapshotid = @snapshotid,
										@inputsid = @everyonesid

								create table #usertbl (dbid int, uid int)

								declare @dbid int, @uid int

								declare logincursor cursor for
										select distinct a.dbid, a.uid
											from databaseprincipal a, #logintbl b
											where a.snapshotid = @snapshotid
												and a.usersid = b.sid
												-- select any object from master db to get system tables for now
												and a.dbid = 1
								
								open logincursor
								fetch next from logincursor into @dbid, @uid
								
								while @@fetch_status = 0
								begin
									insert into #usertbl
										exec isp_sqlsecure_getdatabaseuserparents @snapshotid, @dbid, @uid

									fetch next from logincursor into @dbid, @uid

								end
								
								close logincursor
								deallocate logincursor	

								-- check if user 'guest' is valid. If so, then current login will have public database role even if there is
								--	no database user mapped to it.
								if exists (select * from databaseprincipal a where UPPER(a.name) = 'GUEST' and UPPER(a.hasaccess) ='Y' and a.snapshotid = @snapshotid) 
								begin
									-- public uid is always 0
									insert into #usertbl (dbid, uid)
										select dbid, 0 from databaseprincipal a where UPPER(a.name) = 'GUEST' and UPPER(a.hasaccess) ='Y' and a.snapshotid = @snapshotid

									-- insert guest user as well
									insert into #usertbl (dbid, uid)
										select distinct dbid, uid from databaseprincipal a where UPPER(a.name) = 'GUEST' and UPPER(a.hasaccess) ='Y' and snapshotid = @snapshotid
								end

								-- insert alias users
								insert into #usertbl (dbid, uid) select  distinct dbid, altuid from databaseprincipal where snapshotid = @snapshotid and isalias = 'Y' and altuid IS NOT NULL and usersid IN (select distinct sid from #logintbl)

								select @intval = count(*)
									from vwdatabaseobjectpermission a, #usertbl b, databaseprincipal c
									where
										a.snapshotid = @snapshotid and 
										a.dbid = b.dbid and 
										a.grantee = b.uid and
										a.snapshotid = c.snapshotid and
										a.dbid = c.dbid and
										(b.uid = c.uid or (UPPER(c.isalias) = 'Y' and c.altuid = b.uid))
								if (@intval = 0)
									select @sevcode=@sevcodeok,
											@metricval = N'Everyone does not have access to any system tables.'
								else
									select @sevcode=@severity,
											@metricval = N'Everyone has access to ' + convert(nvarchar,@intval) + N' system tables.'

								drop table #usertbl
								drop table #logintbl
							end

							select @metricthreshold = N'Server is vulnerable if the Everyone Windows group has access to any system tables.'
						end
						-- Startup Stored Procedures Enabled
						else if (@metricid = 20)
						begin
							select @strval = @startupprocs, @severityvalues = N'N'
							if (@strval = @severityvalues)
								select @sevcode=@sevcodeok
							else
								select @sevcode=@severity

							select @metricval = dbo.getyesnotext(@strval)
							select @metricthreshold = N'Server is vulnerable if scan for stored procedures at startup is enabled.'
						end
						-- Startup Stored Procedures
						else if (@metricid = 21)
						begin
							select @sql = N'declare proccursur cursor static for
													select [name]
														from vwdatabaseobject
														where snapshotid = ' + convert(nvarchar,@snapshotid) + N'
															and [type] in (N''P'', N''X'')
															 and runatstartup = N''Y'''
							if (len(@severityvalues) > 0)
								select @sql = @sql + ' and [name] not in (' + @severityvalues + N')'
							select @sql = @sql + ' order by [name]'

							exec (@sql)
							open proccursur

							select @intval2 = 0
							fetch next from proccursur into @strval
							while @@fetch_status = 0
							begin
								if (@intval2 = 1 or len(@metricval) + len(@strval) > 1010)
								begin
									if @intval2 = 0
										select @metricval = @metricval + N', more...',
												@intval2 = 1
								end
								else
									select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval + N''''

									if (@isadmin = 1)
										insert into policyassessmentdetail ( policyid,
																			 assessmentid,
																			 metricid,
																			 snapshotid,
																			 detailfinding,
																			 databaseid,
																			 objecttype,
																			 objectid,
																			 objectname )
																	values ( @policyid,
																			 @assessmentid,
																			 @metricid,
																			 @snapshotid,
																			 N'Unapproved stored procedures found: ''' + @strval + N'''',
																			 null, -- database ID,
																			 N'P', -- object type
																			 null,
																			 @strval )

								fetch next from proccursur into @strval
							end
						
							if (len(@metricval) = 0)
								select @sevcode=@sevcodeok,
										@metricval = N'None found'
							else
								select @sevcode=@severity,
										@metricval = N'Unapproved stored procedures found: ' + @metricval

							close proccursur
							deallocate proccursur

							select @metricthreshold = N'Server is vulnerable if startup stored procedures exist' + case when len(@severityvalues) > 0 then N' and are not in ' + @severityvalues + N'.' else N'.' end
						end
						-- Stored Procedures Encrypted
						else if (@metricid = 22)
						begin
							declare proccursur cursor static for
								select b.databasename, count(a.objectid)
									from vwdatabaseobject a, vwdatabases b
									where a.snapshotid = @snapshotid
										and a.snapshotid = b.snapshotid
										and a.dbid = b.dbid
										and a.[type] = N'P'
										and a.isencrypted = N'N'
									group by b.databasename

							open proccursur

							select @intval2 = 0
							fetch next from proccursur into @strval, @intval
							while @@fetch_status = 0
							begin
								select @strval = @strval + N' (' + convert(nvarchar, @intval) + N')', @strval2 = 'Unencrypted stored procedures found in these databases: '
								if ( @intval2 = 1 or len(@metricval) + len(@strval) > (1010 - len(@strval2)))
								begin
									if @intval2 = 0
										select @metricval = @metricval + N', more...',
												@intval2 = 1
								end
								else
									select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval + N''''

									if (@isadmin = 1)
										insert into policyassessmentdetail ( policyid,
																			 assessmentid,
																			 metricid,
																			 snapshotid,
																			 detailfinding,
																			 databaseid,
																			 objecttype,
																			 objectid,
																			 objectname )
																	values ( @policyid,
																			 @assessmentid,
																			 @metricid,
																			 @snapshotid,
																			 N'Unencrypted stored procedures found in the database: ''' + @strval + N'''',
																			 null, -- database ID,
																			 N'DB', -- object type
																			 null,
																			 @strval )

								fetch next from proccursur into @strval, @intval
							end
						
							if (len(@metricval) = 0)
								select @sevcode=@sevcodeok,
										@metricval = N'No unencrypted stored procedures were found.'
							else
								select @sevcode=@severity,
										@metricval = @strval2 + @metricval

							close proccursur
							deallocate proccursur

							select @metricthreshold = 'Server is vulnerable if unencrypted stored procedures exist.'
						end
						-- User Defined Extended Stored Procedures (XPs)
						else if (@metricid = 23)
						begin
							select @sql = N'declare proccursur cursor static for
													select [name]
														from vwdatabaseobject
														where snapshotid = ' + convert(nvarchar,@snapshotid) + N'
															and [type] = N''X''
															and userdefined = N''Y'''
							if (len(@severityvalues) > 0)
								select @sql = @sql + ' and [name] not in (' + @severityvalues + N')'
							select @sql = @sql + ' order by [name]'

							exec (@sql)
							open proccursur

							select @intval2 = 0
							fetch next from proccursur into @strval
							while @@fetch_status = 0
							begin
								if ( @intval2 = 1 or len(@metricval) + len(@strval) > 1010)
								begin
									if @intval2 = 0
										select @metricval = @metricval + N', more...',
												@intval2 = 1
								end
								else
									select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval + N''''

								if (@isadmin = 1)
									insert into policyassessmentdetail ( policyid,
																		 assessmentid,
																		 metricid,
																		 snapshotid,
																		 detailfinding,
																		 databaseid,
																		 objecttype,
																		 objectid,
																		 objectname )
																values ( @policyid,
																		 @assessmentid,
																		 @metricid,
																		 @snapshotid,
																		 N'Unapproved extended stored procedure found: ''' + @strval + N'''',
																		 null, -- database ID,
																		 N'X', -- object type
																		 null,
																		 @strval )

								fetch next from proccursur into @strval
							end

							if (len(@metricval) = 0)
								select @sevcode=@sevcodeok,
										@metricval = N'None found'
							else
								select @sevcode=@severity,
										@metricval = N'Unapproved extended stored procedures found: ' + @metricval

							close proccursur
							deallocate proccursur

							select @metricthreshold = N'Server is vulnerable if user defined extended stored procedures exist' + case when len(@severityvalues) > 0 then N' not in ' + @severityvalues else N'' end + N'.'
						end
						-- Dangerous Extended Stored Procedures (XPs)
						else if (@metricid = 24)
						begin
							if (len(@severityvalues) > 0)
							begin
								select @sql = N'declare proccursur cursor static for
													select a.name as objectname, c.name as granteename, b.permission
														from databaseobject a,
															databaseobjectpermission b,
															databaseprincipal c
														where a.snapshotid = ' + convert(nvarchar, @snapshotid) + N' 
															and a.type = N''X''
															and a.name in (' + @severityvalues + N')
															and a.snapshotid = b.snapshotid
															and a.dbid = b.dbid
															and a.objectid = b.objectid
															and b.snapshotid = c.snapshotid
															and b.dbid = c.dbid
															and (b.isgrant = N''Y'' or b.isgrantwith = N''Y'')
															and b.grantee = c.uid
														order by objectname, granteename'
								exec (@sql)
								open proccursur

								select @intval2 = 0
								fetch next from proccursur into @strval, @strval2, @strval3
								while @@fetch_status = 0
								begin
									if ( @intval2 = 1 or len(@metricval) + len(@strval) > 1010)
									begin
										if @intval2 = 0
											select @metricval = @metricval + N', more...',
													@intval2 = 1
									end
									else
										select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + @strval3 + N' granted to ''' + @strval2 + N''' on ''' + @strval + N'''' 

									if (@isadmin = 1)
										insert into policyassessmentdetail ( policyid,
																			 assessmentid,
																			 metricid,
																			 snapshotid,
																			 detailfinding,
																			 databaseid,
																			 objecttype,
																			 objectid,
																			 objectname )
																	values ( @policyid,
																			 @assessmentid,
																			 @metricid,
																			 @snapshotid,
																			 N'Permission on extended stored procedures found: ''' + @strval3 + N' granted to ''' + @strval2 + N''' on ''' + @strval + N'''',
																			 null, -- database ID,
																			 N'X', -- object type
																			 null,
																			 @strval )

									fetch next from proccursur into @strval, @strval2, @strval3
								end
								close proccursur
								deallocate proccursur

								if (len(@metricval) = 0)
									select @sevcode=@sevcodeok,
											@metricval = N'No permissions found.'
								else
									select @sevcode=@severity,
											@metricval = N'Permissions on extended stored procedures found: ' + @metricval
							end
							else
								select @sevcode=@sevcodeok,
										@metricval = N'No list of dangerous extended stored procedures was provided.'

							select @metricthreshold = N'Server is vulnerable if permissions have been granted on any of these extended stored procedures: ' + @severityvalues
						end
						-- Remote Access
						else if (@metricid = 25)
						begin
							select @strval = @remoteaccess, @severityvalues = N'N'
							if (@strval = @severityvalues)
								select @sevcode=@sevcodeok
							else
								select @sevcode=@severity

							select @metricval = dbo.getyesnotext(@strval)
							select @metricthreshold = N'Server is vulnerable if remote access is enabled.'
						end
						-- Protocols
						else if (@metricid = 26)
						begin
							select @sql = N'declare protocolcursur cursor static for
												select protocolname
													from serverprotocol
													where snapshotid = ' + convert(nvarchar, @snapshotid) + N'
														and protocolname not in (' + @severityvalues + N')
													order by protocolname'
							exec (@sql)
							open protocolcursur

							select @intval2 = 0
							fetch next from protocolcursur into @strval
							while @@fetch_status = 0
							begin
								if ( @intval2 = 1 or len(@metricval) + len(@strval) > 1010)
								begin
									if @intval2 = 0
										select @metricval = @metricval + N', more...',
												@intval2 = 1
								end
								else
									select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval + N''''

								if (@isadmin = 1)
									insert into policyassessmentdetail ( policyid,
																		 assessmentid,
																		 metricid,
																		 snapshotid,
																		 detailfinding,
																		 databaseid,
																		 objecttype,
																		 objectid,
																		 objectname )
																values ( @policyid,
																		 @assessmentid,
																		 @metricid,
																		 @snapshotid,
																		 N'Unapproved protocol found: ''' + @strval + N'''',
																		 null, -- database ID,
																		 N'PR', -- there is no type for protocols
																		 null,
																		 @strval )

								fetch next from protocolcursur into @strval
							end
						
							if (len(@metricval) = 0)
								select @sevcode=@sevcodeok,
										@metricval = N'None found.'
							else
								select @sevcode=@severity,
										@metricval = N'Unapproved protocols found: ' + @metricval

							select @metricthreshold = N'Server is vulnerable if using protocols other than: ' + @severityvalues

							close protocolcursur
							deallocate protocolcursur
						end
						-- Common TCP Port Used
						else if (@metricid = 27)
						begin
							select @sql = N'declare portcursur cursor static for
												select case when dynamicport = N''Y'' then N''dynamic'' else isnull([port],'''') end as port
													from serverprotocol
													where snapshotid = ' + convert(nvarchar, @snapshotid) + N'
														and protocolname = ''TCP/IP''
														and case when dynamicport = N''Y'' then N''dynamic'' else isnull([port],'''') end in (''''' + case when len(@severityvalues) > 0 then N',' + @severityvalues else N'' end + N')
													order by port'
							--	Check for dynamic removed from where clause per PR 802421 02/05/2010
							--	dynamic must now be entered in the list to be a finding
															--	or dynamicport = N''Y'')
							exec (@sql)
							open portcursur

							select @intval2 = 0
							fetch next from portcursur into @strval
							while @@fetch_status = 0
							begin
								if (@intval2 = 1 or len(@metricval) + len(@strval) > 1010)
								begin
									if @intval2 = 0
										select @metricval = @metricval + N', more...',
												@intval2 = 1
								end
								else
									select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval + N''''

								if (@isadmin = 1)
									insert into policyassessmentdetail ( policyid,
																		 assessmentid,
																		 metricid,
																		 snapshotid,
																		 detailfinding,
																		 databaseid,
																		 objecttype,
																		 objectid,
																		 objectname )
																values ( @policyid,
																		 @assessmentid,
																		 @metricid,
																		 @snapshotid,
																		 N'Unapproved TCP/IP port found: ''' + @strval + N'''',
																		 null, -- database ID,
																		 N'PR', -- there is no type for ports
																		 null,
																		 @strval )

								fetch next from portcursur into @strval
							end
						
							if (len(@metricval) = 0)
								select @sevcode=@sevcodeok,
										@metricval = N'None found.'
							else
								select @sevcode=@severity,
										@metricval = N'Unapproved TCP/IP ports found: ' + @metricval

							select @metricthreshold = N'Server is vulnerable if TCP/IP uses any of these ports: ' + @severityvalues

							close portcursur
							deallocate portcursur
						end
						-- Hidden From Browsing
						else if (@metricid = 28)
						begin
							select @strval = @hide, @severityvalues = N'Y'
							if (@strval = @severityvalues)
								select @sevcode=@sevcodeok
							else
								select @sevcode=@severity

							select @metricval = dbo.getyesnotext(@strval)
							select @metricthreshold = N'Server is vulnerable if Hide from Browsing is not enabled.'
						end
						-- Agent Job Execution
						else if (@metricid = 29)
						begin
							select @strval = @agentsysadmin, @severityvalues = N'Y'
							if (@strval = @severityvalues)
								select @sevcode=@sevcodeok
							else
								select @sevcode=@severity

							select @metricval = dbo.getyesnotext(@strval)
							select @metricthreshold = N'Server is vulnerable if SQL Agent CmdExec job execution is not restricted to system administrators.'
						end
						-- Replication Enabled
						else if (@metricid = 30)
						begin
							select @strval = @replication, @severityvalues = N'N'
							if (@strval = @severityvalues)
								select @sevcode=@sevcodeok
							else
								select @sevcode=@severity

							select @metricval = dbo.getyesnotext(@strval)
							select @metricthreshold = N'Server is vulnerable if replication is enabled.'
						end
						-- Unexpected Registry Key Owners
						else if (@metricid = 31)
						begin
							if (len(@severityvalues) > 0)
							begin
								select @sql = N'declare keycursor cursor static for
													select objectname, ownername
														from vwregistrykey
														where snapshotid = ' + convert(nvarchar, @snapshotid) + N' and lower(ownername) not in (' + lower(@severityvalues) + N')'
								if (charindex('%', @severityvalues) > 0)
								begin
									select @strval = lower(@severityvalues),
											@intval = charindex(''',''',@strval)
									while (@intval > 0)
									begin
										select @intval2 = charindex('%', @strval)
										if (@intval2 < @intval)		-- this item contains a wildcard
											select @sql = @sql + ' and lower(ownername) not like ' + substring(@strval,1,@intval)
											select @strval = substring(@strval,@intval+2, len(@strval)-(@intval+1))
											select @intval = charindex(''',''',@strval)
									end
									if (len(@strval) > 0)
									begin
										select @intval2 = charindex('%', @strval)
										if (@intval2 > 0)		-- this item contains a wildcard
											select @sql = @sql + ' and lower(ownername) not like ' + @strval
									end
								end
								select @sql = @sql + N' order by objectname, ownername'

								exec (@sql)
								open keycursor

								select @intval = 0, @intval2 = 0
								fetch next from keycursor into @strval, @strval2
								while @@fetch_status = 0
								begin
									select @intval = @intval + 1
									if ( @intval2 = 1 or len(@metricval) + len(@strval) + len(@strval2) > 1400)
									begin
										if @intval2 = 0
											select @metricval = @metricval + N', more...',
													@intval2 = 1
									end
									else
										select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval + N''' has owner ''' + @strval2 + ''''

									if (@isadmin = 1)
										insert into policyassessmentdetail ( policyid,
																			 assessmentid,
																			 metricid,
																			 snapshotid,
																			 detailfinding,
																			 databaseid,
																			 objecttype,
																			 objectid,
																			 objectname )
																	values ( @policyid,
																			 @assessmentid,
																			 @metricid,
																			 @snapshotid,
																			 N'Registry key with unapproved owners found: ''' + @strval + N'''',
																			 null, -- database ID,
																			 N'RK', -- no registry key type
																			 null,
																			 @strval )

									fetch next from keycursor into @strval, @strval2
								end

								close keycursor
								deallocate keycursor

								if (len(@metricval) = 0)
									select @sevcode=@sevcodeok,
											@metricval = N'No keys found.'
								else
									select @sevcode=@severity,
											@metricval = convert(nvarchar, @intval) + N' keys with unapproved owners found: ' + @metricval
							end
							else
								select @sevcode=@sevcodeok,
										@metricval = N'No list of approved owners was provided.'

							select @metricthreshold = N'Server is vulnerable if registry key owners are not these users: ' + @severityvalues
						end
						-- Unexpected Registry Key Permissions
						else if (@metricid = 32)
						begin
							if (len(@severityvalues) > 0)
							begin
								select @sql = N'declare filecursor cursor static for
													select objectname, grantee
														from vwregistrykeypermission
														where snapshotid = ' + convert(nvarchar, @snapshotid) + N' and accesstype is not null and lower(grantee) not in (' + lower(@severityvalues) + N')'
								if (charindex('%', @severityvalues) > 0)
								begin
									select @strval = lower(@severityvalues),
											@intval = charindex(''',''',@strval)
									while (@intval > 0)
									begin
										select @intval2 = charindex('%', @strval)
										if (@intval2 < @intval)		-- this item contains a wildcard
											select @sql = @sql + ' and lower(grantee) not like ' + substring(@strval,1,@intval)
											select @strval = substring(@strval,@intval+2, len(@strval)-(@intval+1))
											select @intval = charindex(''',''',@strval)
									end
									if (len(@strval) > 0)
									begin
										select @intval2 = charindex('%', @strval)
										if (@intval2 > 0)		-- this item contains a wildcard
											select @sql = @sql + ' and lower(grantee) not like ' + @strval
									end
								end
								select @sql = @sql + N' order by objectname, grantee'

								exec (@sql)
								open filecursor

								select @intval = 0, @intval2 = 0
								fetch next from filecursor into @strval, @strval2
								while @@fetch_status = 0
								begin
									if (@strval <> @strval3)
										select @intval = @intval + 1
									if (len(@metricval) + case when (@strval3 = @strval) then 13 else len(@strval) end + len(@strval2) > 1400 or @intval2 = 1)
									begin
										if @intval2 = 0
											select @metricval = @metricval + N', more...',
													@intval2 = 1
									end
									else
									begin
										if (@strval = @strval3)
											select @metricval = @metricval + N' and grantee ''' + @strval2 + ''''
										else
											select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval + N''' has grantee ''' + @strval2 + ''''
									end

									if (@isadmin = 1)
										insert into policyassessmentdetail ( policyid,
																			 assessmentid,
																			 metricid,
																			 snapshotid,
																			 detailfinding,
																			 databaseid,
																			 objecttype,
																			 objectid,
																			 objectname )
																	values ( @policyid,
																			 @assessmentid,
																			 @metricid,
																			 @snapshotid,
																			 N'Registry key with unapproved permissions found: ''' + @strval + N''' has grantee ''' + @strval2 + N'''',
																			 null, -- database ID,
																			 N'RK', -- no registry key object type
																			 null,
																			 @strval )

									select @strval3 = @strval

									fetch next from filecursor into @strval, @strval2
								end

								close filecursor
								deallocate filecursor

								if (len(@metricval) = 0)
									select @sevcode=@sevcodeok,
											@metricval = N'No keys found.'
								else
									select @sevcode=@severity,
											@metricval = convert(nvarchar, @intval) + N' keys with unapproved permissions found: ' + @metricval
							end
							else
								select @sevcode=@sevcodeok,
											@metricval = N'No list of approved users was provided.'

							select @metricthreshold = N'Server is vulnerable if registry key permissions are granted to users other than: ' + @severityvalues
						end
						-- Files on NTFS Drives
						else if (@metricid = 33)
						begin
							select @severityvalues = N'NTFS'
							declare drivecursor cursor for
								select distinct lower(left(objectname,2)) as drive, disktype
									from vwfilesystemobject
									where snapshotid = @snapshotid and upper(disktype) <> @severityvalues

							declare @disktype nvarchar(16)

							open drivecursor
							fetch next from drivecursor into @strval, @disktype
							select @intval2 = 0
							while @@fetch_status = 0
							begin
								if (@intval2 = 1 or len(@metricval) + len(@strval) > 1010)
								begin
									if @intval2 = 0
										select @metricval = @metricval + N', more...',
												@intval2 = 1
								end
								else
									select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + @strval + N' is ' + @disktype

								if (@isadmin = 1)
									insert into policyassessmentdetail ( policyid,
																		 assessmentid,
																		 metricid,
																		 snapshotid,
																		 detailfinding,
																		 databaseid,
																		 objecttype,
																		 objectid,
																		 objectname )
																values ( @policyid,
																		 @assessmentid,
																		 @metricid,
																		 @snapshotid,
																		 N'Non-NTFS drive found: ' + @strval + N' is ' + @disktype,
																		 null, -- database ID,
																		 N'DR', -- no disk drive object type
																		 null,
																		 @strval )

								fetch next from drivecursor into @strval, @disktype
							end

							close drivecursor
							deallocate drivecursor	

							if (len(@metricval) = 0)
								select @sevcode=@sevcodeok,
										@metricval = 'All files are on NTFS drives.'
							else
								select @sevcode=@severity

							select @metricthreshold = N'Server is vulnerable if files are found on drives that are not formatted as NTFS.'
						end
						-- Unexpected Executable File Owners
						else if (@metricid = 34)
						begin
							if (len(@severityvalues) > 0)
							begin
								select @sql = N'declare filecursor cursor static for
													select distinct lower(objectname) as objectname, ownername
														from vwfilesystemobject
														where snapshotid = ' + convert(nvarchar, @snapshotid) + N' and lower(right(objectname,4)) = N''.exe'' and lower(ownername) not in (' + lower(@severityvalues) + N')'
								if (charindex('%', @severityvalues) > 0)
								begin
									select @strval = lower(@severityvalues),
											@intval = charindex(''',''',@strval)
									while (@intval > 0)
									begin
										select @intval2 = charindex('%', @strval)
										if (@intval2 < @intval)		-- this item contains a wildcard
											select @sql = @sql + ' and lower(ownername) not like ' + substring(@strval,1,@intval)
											select @strval = substring(@strval,@intval+2, len(@strval)-(@intval+1))
											select @intval = charindex(''',''',@strval)
									end
									if (len(@strval) > 0)
									begin
										select @intval2 = charindex('%', @strval)
										if (@intval2 > 0)		-- this item contains a wildcard
											select @sql = @sql + ' and lower(ownername) not like ' + @strval
									end
								end
								select @sql = @sql + N' order by objectname, ownername'

								exec (@sql)
								open filecursor

								select @intval = 0, @intval2 = 0
								fetch next from filecursor into @strval, @strval2
								while @@fetch_status = 0
								begin
									select @intval = @intval + 1
									if (len(@metricval) + len(@strval) + len(@strval2) > 1400)
									begin
										if @intval2 = 0
											select @metricval = @metricval + N', more...',
													@intval2 = 1
									end
									else
										select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval + N''' has owner ''' + @strval2 + N''''

									if (@isadmin = 1)
										insert into policyassessmentdetail ( policyid,
																			 assessmentid,
																			 metricid,
																			 snapshotid,
																			 detailfinding,
																			 databaseid,
																			 objecttype,
																			 objectid,
																			 objectname )
																	values ( @policyid,
																			 @assessmentid,
																			 @metricid,
																			 @snapshotid,
																			 N'File with unapproved owner found: ''' + @strval + N''' has owner ''' + @strval2 + N'''',
																			 null, -- database ID,
																			 N'FI', -- no file object type
																			 null,
																			 @strval )

									fetch next from filecursor into @strval, @strval2
								end

								close filecursor
								deallocate filecursor

								if (len(@metricval) = 0)
									select @sevcode=@sevcodeok,
											@metricval = N'No files found.'
								else
									select @sevcode=@severity,
											@metricval = convert(nvarchar, @intval) + N' files with unapproved owners found: ' + @metricval
							end
							else
								select @sevcode=@sevcodeok,
											@metricval = N'No list of approved owners was provided.'

							select @metricthreshold = N'Server is vulnerable if executable file owners are not these users: ' + @severityvalues
						end
						-- Unexpected Executable File Permissions
						else if (@metricid = 35)
						begin
							if (len(@severityvalues) > 0)
							begin
								select @sql = N'declare filecursor cursor static for
													select distinct lower(objectname) as objectname, grantee
														from vwfilesystemobjectpermission
														where snapshotid = ' + convert(nvarchar, @snapshotid) + N' and lower(right(objectname,4)) = N''.exe'' and accesstype is not null and lower(grantee) not in (' + lower(@severityvalues) + N')'
								if (charindex('%', @severityvalues) > 0)
								begin
									select @strval = lower(@severityvalues),
											@intval = charindex(''',''',@strval)
									while (@intval > 0)
									begin
										select @intval2 = charindex('%', @strval)
										if (@intval2 < @intval)		-- this item contains a wildcard
											select @sql = @sql + ' and lower(grantee) not like ' + substring(@strval,1,@intval)
											select @strval = substring(@strval,@intval+2, len(@strval)-(@intval+1))
											select @intval = charindex(''',''',@strval)
									end
									if (len(@strval) > 0)
									begin
										select @intval2 = charindex('%', @strval)
										if (@intval2 > 0)		-- this item contains a wildcard
											select @sql = @sql + ' and lower(grantee) not like ' + @strval
									end
								end
								select @sql = @sql + N' order by objectname, grantee'

								exec (@sql)
								open filecursor

								select @intval = 0, @intval2 = 0
								fetch next from filecursor into @strval, @strval2
								while @@fetch_status = 0
								begin
									if (@strval <> @strval3)
										select @intval = @intval + 1
									if (len(@metricval) + case when (@strval3 = @strval) then 13 else len(@strval) end + len(@strval2) > 1400 or @intval2 = 1)
									begin
										if @intval2 = 0
											select @metricval = @metricval + N', more...',
													@intval2 = 1
									end
									else
									begin
										if (@strval = @strval3)
											select @metricval = @metricval + N' and grantee ''' + @strval2 + ''''
										else
											select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval + N''' has grantee ''' + @strval2 + ''''
									end

									if (@isadmin = 1)
										insert into policyassessmentdetail ( policyid,
																			 assessmentid,
																			 metricid,
																			 snapshotid,
																			 detailfinding,
																			 databaseid,
																			 objecttype,
																			 objectid,
																			 objectname )
																	values ( @policyid,
																			 @assessmentid,
																			 @metricid,
																			 @snapshotid,
																			 N'File with unapproved permission found: ''' + @strval + N''' has grantee ''' + @strval2 + N'''',
																			 null, -- database ID,
																			 N'FI', -- no file object type
																			 null,
																			 @strval )

									select @strval3 = @strval

									fetch next from filecursor into @strval, @strval2
								end

								close filecursor
								deallocate filecursor

								if (len(@metricval) = 0)
									select @sevcode=@sevcodeok,
											@metricval = N'No files found.'
								else
									select @sevcode=@severity,
											@metricval = convert(nvarchar, @intval) + N' files with unapproved permissions found: ' + @metricval
							end
							else
								select @sevcode=@sevcodeok,
											@metricval = N'No list of approved users was provided.'

							select @metricthreshold = N'Server is vulnerable if executable file permissions are granted to users other than: ' + @severityvalues
						end
						-- Unexpected Database File Owners
						else if (@metricid = 36)
						begin
							if (len(@severityvalues) > 0)
							begin
								select @sql = N'declare filecursor cursor static for
													select distinct lower(objectname) as objectname, ownername
														from vwfilesystemobject
														where snapshotid = ' + convert(nvarchar, @snapshotid) + N' and objecttype = N''DB'' and lower(ownername) not in (' + lower(@severityvalues) + N')'
								if (charindex('%', @severityvalues) > 0)
								begin
									select @strval = lower(@severityvalues),
											@intval = charindex(''',''',@strval)
									while (@intval > 0)
									begin
										select @intval2 = charindex('%', @strval)
										if (@intval2 < @intval)		-- this item contains a wildcard
											select @sql = @sql + ' and lower(ownername) not like ' + substring(@strval,1,@intval)
											select @strval = substring(@strval,@intval+2, len(@strval)-(@intval+1))
											select @intval = charindex(''',''',@strval)
									end
									if (len(@strval) > 0)
									begin
										select @intval2 = charindex('%', @strval)
										if (@intval2 > 0)		-- this item contains a wildcard
											select @sql = @sql + ' and lower(ownername) not like ' + @strval
									end
								end
								select @sql = @sql + N' order by objectname, ownername'

								exec (@sql)
								open filecursor

								select @intval = 0, @intval2 = 0
								fetch next from filecursor into @strval, @strval2
								while @@fetch_status = 0
								begin
									select @intval = @intval + 1
									if (len(@metricval) + len(@strval) + len(@strval2) > 1400)
									begin
										if @intval2 = 0
											select @metricval = @metricval + N', more...',
													@intval2 = 1
									end
									else
										select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval + N''' has owner ''' + @strval2 + ''''

									if (@isadmin = 1)
										insert into policyassessmentdetail ( policyid,
																			 assessmentid,
																			 metricid,
																			 snapshotid,
																			 detailfinding,
																			 databaseid,
																			 objecttype,
																			 objectid,
																			 objectname )
																	values ( @policyid,
																			 @assessmentid,
																			 @metricid,
																			 @snapshotid,
																			 N'File with unapproved owner found: ''' + @strval + N''' has owner ''' + @strval2 + N'''',
																			 null, -- database ID,
																			 N'FI', -- no file object type
																			 null,
																			 @strval )

									fetch next from filecursor into @strval, @strval2
								end

								close filecursor
								deallocate filecursor

								if (len(@metricval) = 0)
									select @sevcode=@sevcodeok,
											@metricval = N'No files found.'
								else
									select @sevcode=@severity,
											@metricval = convert(nvarchar, @intval) + N' files with unapproved owners found: ' + @metricval
 							end
							else
								select @sevcode=@sevcodeok,
											@metricval = N'No list of approved owners was provided.'

							select @metricthreshold = N'Server is vulnerable if database file owners are not these users: ' + @severityvalues
						end
						-- Everyone Database File Access
						else if (@metricid = 37)
						begin
							if not exists (select * from serveroswindowsaccount where snapshotid = @snapshotid and sid=@everyonesid)
								select @sevcode=@sevcodeok,
										@metricval = N'Everyone account not found in file permissions on server.'
							else
							begin
								create table #oslogintbl (sid varbinary(85))

								-- insert everyone user in the table
								insert into #oslogintbl (sid) values (@everyonesid)

								insert into #oslogintbl
									exec @err = [dbo].[isp_sqlsecure_getwindowsgroupparentsos]
										@snapshotid = @snapshotid,
										@inputsid = @everyonesid

								--check for permissions
								declare filecursor cursor static for
									select a.objectname, a.grantee as perm, N'P' as [type]
										from vwfilesystemobjectpermission a
										where
											a.snapshotid = @snapshotid and
											a.objecttype = N'DB' and
											a.accesstype is not null and 
											a.sid in (select [sid] from #oslogintbl)
									union
									--check for file owners of no permissions found
									select a.objectname, a.ownername as perm, N'O' as [type]
										from vwfilesystemobject a
										where
											a.snapshotid = @snapshotid and
											a.objecttype = N'DB' and
											a.ownersid in (select [sid] from #oslogintbl)
									order by objectname, [type], perm

								open filecursor

								select @intval = 0, @intval2 = 0
								declare @type char
								fetch next from filecursor into @strval, @strval2, @type
								while @@fetch_status = 0
								begin
									if (@strval <> @strval3)
										select @intval = @intval + 1
									if (len(@metricval) + len(@strval) + len(@strval2) > 1400 or @intval2 = 1)
									begin
										if @intval2 = 0
											select @metricval = @metricval + N', more...',
													@intval2 = 1
									end
									else
										if (@strval = @strval3)
											select @metricval = @metricval + N' and ' + case when @type = 'O' then 'owner' else 'grantee' end + N' ''' + @strval2 + ''''
										else
											select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval + N''' has ' + case when @type = 'O' then 'owner' else 'grantee' end + N' ''' + @strval2 + ''''

									select @strval3 = @strval

									if (@isadmin = 1)
										insert into policyassessmentdetail ( policyid,
																			 assessmentid,
																			 metricid,
																			 snapshotid,
																			 detailfinding,
																			 databaseid,
																			 objecttype,
																			 objectid,
																			 objectname )
																	values ( @policyid,
																			 @assessmentid,
																			 @metricid,
																			 @snapshotid,
																			 N'File with Everyone access found: ''' + @strval + N''' has ' + case when @type = 'O' then 'owner' else 'grantee' end + N' ''' + @strval2 + N'''',
																			 null, -- database ID,
																			 N'FI', -- object type
																			 null,
																			 @strval )

									fetch next from filecursor into @strval, @strval2, @type
								end

								close filecursor
								deallocate filecursor

								if (len(@metricval) = 0)
									select @sevcode=@sevcodeok,
											@metricval = N'Everyone does not have access to any database files.'
								else
									select @sevcode=@severity,
											@metricval = convert(nvarchar, @intval) + N' files with Everyone access found: ' + @metricval

								drop table #oslogintbl
							end

							select @metricthreshold = N'Server is vulnerable if the Everyone Windows group has access to any database files.'
						end
						-- Unexpected Database File Permissions
						else if (@metricid = 38)
						begin
							if (len(@severityvalues) > 0)
							begin
								select @sql = N'declare filecursor cursor static for
													select distinct lower(objectname) as objectname, grantee
														from vwfilesystemobjectpermission
														where snapshotid = ' + convert(nvarchar, @snapshotid) + N' and objecttype = N''DB'' and accesstype is not null and lower(grantee) not in (' + lower(@severityvalues) + N')'
								if (charindex('%', @severityvalues) > 0)
								begin
									select @strval = lower(@severityvalues),
											@intval = charindex(''',''',@strval)
									while (@intval > 0)
									begin
										select @intval2 = charindex('%', @strval)
										if (@intval2 < @intval)		-- this item contains a wildcard
											select @sql = @sql + ' and lower(grantee) not like ' + substring(@strval,1,@intval)
											select @strval = substring(@strval,@intval+2, len(@strval)-(@intval+1))
											select @intval = charindex(''',''',@strval)
									end
									if (len(@strval) > 0)
									begin
										select @intval2 = charindex('%', @strval)
										if (@intval2 > 0)		-- this item contains a wildcard
											select @sql = @sql + ' and lower(grantee) not like ' + @strval
									end
								end
								select @sql = @sql + N' order by objectname, grantee'

								exec (@sql)
								open filecursor

								select @intval = 0, @intval2 = 0
								fetch next from filecursor into @strval, @strval2
								while @@fetch_status = 0
								begin
									if (@strval <> @strval3)
										select @intval = @intval + 1
									if (len(@metricval) + case when (@strval3 = @strval) then 13 else len(@strval) end + len(@strval2) > 1400 or @intval2 = 1)
									begin
										if @intval2 = 0
											select @metricval = @metricval + N', more...',
													@intval2 = 1
									end
									else
									begin
										if (@strval = @strval3)
											select @metricval = @metricval + N' and grantee ''' + @strval2 + ''''
										else
											select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval + N''' has grantee ''' + @strval2 + ''''
									end

									if (@isadmin = 1)
										insert into policyassessmentdetail ( policyid,
																			 assessmentid,
																			 metricid,
																			 snapshotid,
																			 detailfinding,
																			 databaseid,
																			 objecttype,
																			 objectid,
																			 objectname )
																	values ( @policyid,
																			 @assessmentid,
																			 @metricid,
																			 @snapshotid,
																			 N'File with unapproved permission found: ''' + @strval + N''' has grantee ''' + @strval2 + N'''',
																			 null, -- database ID,
																			 N'FI', -- object type
																			 null,
																			 @strval )

									select @strval3 = @strval

									fetch next from filecursor into @strval, @strval2
								end

								close filecursor
								deallocate filecursor

								if (len(@metricval) = 0)
									select @sevcode=@sevcodeok,
											@metricval = N'No files found.'
								else
									select @sevcode=@severity,
											@metricval = convert(nvarchar, @intval) + N' files with unapproved permissions found: ' + @metricval
							end
							else
								select @sevcode=@sevcodeok,
											@metricval = N'No list of approved users was provided.'

							select @metricthreshold = N'Server is vulnerable if database file permissions are granted to users other than: ' + @severityvalues
						end
						-- Operating System Version
						else if (@metricid = 39)
						begin
							-- changing this to ignore beginning and ending spaces which are strangely included in some of the version strings
							-- they will be trimmed from both the os version and the match list for consistency
							if (len(@severityvalues) > 0)
							begin
								select @intval=1;
								select @intval2 = charindex(''',''', @severityvalues);
								while @intval2 > 0
								begin
									insert into @tblval values(ltrim(rtrim(substring(@severityvalues, @intval+1, @intval2 - @intval - 1))))
									select @intval = @intval2+2, @intval2 = charindex(''',''', @severityvalues, @intval+1)
								end
								insert into @tblval values(ltrim(rtrim(substring(@severityvalues, @intval+1, len(@severityvalues) - @intval - 1))))
							end
							select @strval = ltrim(rtrim(@os))
							if exists (select 1 from @tblval where val = @strval)
								select @sevcode=@sevcodeok
							else
								select @sevcode=@severity
							select @metricval = N'Current version is ' + case when @strval is null then N'unknown. Check the snapshot status and the activity log for possible causes.' 
																									else @os + '.' end

							select @metricthreshold = N'Server is vulnerable if OS version is not in ' + @severityvalues + '.'
						end
						-- SQL Server Service Login Account Acceptable
						else if (@metricid = 40)
						begin
							if (len(@severityvalues) > 0)
							begin
								select @strval=loginname from vwservice where snapshotid = @snapshotid and servicetype = 0
								if (len(@strval) > 0)
								begin
									if (charindex('''' + @strval + '''', @severityvalues) > 0)
										select @sevcode=@sevcodeok
									else
										select @sevcode=@severity
									select @metricval = N'Current login account is ' + @strval + '.'
								end
								else
									select @sevcode=@sevcodeok,
											@metricval = N'No login account or service not found.'
							end
							else
								select @sevcode=@sevcodeok,
										@metricval = N'No approved login account was provided.'

							select @metricthreshold = N'Server is vulnerable if SQL Server Service Login Account is a user other than: ' + @severityvalues
						end
						-- Reporting Services Enabled
						else if (@metricid = 41)
						begin
							select @severityvalues = N'Running'
							select @strval=[state] from vwservice where snapshotid = @snapshotid and servicetype = 6
							if (len(@strval) > 0)
							begin
								if (@strval <> @severityvalues)
									select @sevcode=@sevcodeok
								else
									select @sevcode=@severity
								select @metricval = N'Current state is ' + @strval + '.'
							end
							else
								select @sevcode=@sevcodeok,
									@metricval = N'Service not found or state undetermined.'

							select @metricthreshold = N'Server is vulnerable if Reporting Services are ' + @severityvalues + '.'
						end
						-- Analysis Services Enabled
						else if (@metricid = 42)
						begin
							select @severityvalues = N'Running'
							select @strval=[state] from vwservice where snapshotid = @snapshotid and servicetype = 3
							if (len(@strval) > 0)
							begin
								if (@strval <> @severityvalues)
									select @sevcode=@sevcodeok
								else
									select @sevcode=@severity
								select @metricval = N'Current state is ' + @strval + '.'
							end
							else
								select @sevcode=@sevcodeok,
									@metricval = N'Service not found or state undetermined.'

							select @metricthreshold = N'Server is vulnerable if Analysis Services are ' + @severityvalues + '.'
						end
						-- Analysis Services Login Account Acceptable
						else if (@metricid = 43)
						begin
							if (len(@severityvalues) > 0)
							begin
								select @strval=loginname from vwservice where snapshotid = @snapshotid and servicetype = 3
								if (len(@strval) > 0)
								begin
									if (charindex('''' + @strval + '''', @severityvalues) > 0)
										select @sevcode=@sevcodeok
									else
										select @sevcode=@severity
									select @metricval = N'Current login account is ' + @strval + '.'
								end
								else
									select @sevcode=@sevcodeok,
											@metricval = N'No login account or service not found.'
							end
							else
								select @sevcode=@sevcodeok,
										@metricval = N'No approved login account was provided.'

							select @metricthreshold = N'Server is vulnerable if Analysis Services Service Login Account is a user other than: ' + @severityvalues
						end
						-- Notification Services Enabled
						else if (@metricid = 44)
						begin
							select @severityvalues = N'Running'
							select @strval=[state] from vwservice where snapshotid = @snapshotid and servicetype in (9, 11)
							if (len(@strval) > 0)
							begin
								if (@strval <> @severityvalues)
									select @sevcode=@sevcodeok
								else
									select @sevcode=@severity
								select @metricval = N'Current state is ' + @strval + '.'
							end
							else
								select @sevcode=@sevcodeok,
									@metricval = N'Service not found or state undetermined.'

							select @metricthreshold = N'Server is vulnerable if Notification Services are ' + @severityvalues + '.'
						end
						-- Notification Services Login Account Acceptable
						else if (@metricid = 45)
						begin
							if (len(@severityvalues) > 0)
							begin
								select @strval=loginname from vwservice where snapshotid = @snapshotid and servicetype in (9, 11)
								if (len(@strval) > 0)
								begin
									if (charindex('''' + @strval + '''', @severityvalues) > 0)
										select @sevcode=@sevcodeok
									else
										select @sevcode=@severity
									select @metricval = N'Current login account is ' + @strval + '.'
								end
								else
									select @sevcode=@sevcodeok,
											@metricval = N'No login account or service not found.'
							end
							else
								select @sevcode=@sevcodeok,
										@metricval = N'No approved login account was provided.'

							select @metricthreshold = N'Server is vulnerable if Notification Services Service Login Account is a user other than: ' + @severityvalues
						end
						-- Integration Services Enabled
						else if (@metricid = 46)
						begin
							select @severityvalues = N'Running'
							select @strval=[state] from vwservice where snapshotid = @snapshotid and servicetype in (5,12,15)
							if (len(@strval) > 0)
							begin
								if (@strval <> @severityvalues)
									select @sevcode=@sevcodeok
								else
									select @sevcode=@severity
								select @metricval = N'Current state is ' + @strval + '.'
							end
							else
								select @sevcode=@sevcodeok,
									@metricval = N'Service not found or state undetermined.'

							select @metricthreshold = N'Server is vulnerable if Integration Services are ' + @severityvalues + '.'
						end
						-- Integration Services Login Account Acceptable
						else if (@metricid = 47)
						begin
							if (len(@severityvalues) > 0)
							begin
								select @strval=loginname from vwservice where snapshotid = @snapshotid and servicetype in (5,12,15)
								if (len(@strval) > 0)
								begin
									if (charindex('''' + @strval + '''', @severityvalues) > 0)
										select @sevcode=@sevcodeok
									else
										select @sevcode=@severity
									select @metricval = N'Current login account is ' + @strval + '.'
								end
								else
									select @sevcode=@sevcodeok,
											@metricval = N'No login account or service not found.'
							end
							else
								select @sevcode=@sevcodeok,
										@metricval = N'No approved login account was provided.'

							select @metricthreshold = N'Server is vulnerable if Integration Services Service Login Account is a user other than: ' + @severityvalues
						end
						-- SQL Server Agent Enabled
						else if (@metricid = 48)
						begin
							select @severityvalues = N'Running'
							select @strval=[state] from vwservice where snapshotid = @snapshotid and servicetype = 1
							if (len(@strval) > 0)
							begin
								if (@strval <> @severityvalues)
									select @sevcode=@sevcodeok
								else
									select @sevcode=@severity
								select @metricval = N'Current state is ' + @strval + '.'
							end
							else
								select @sevcode=@sevcodeok,
									@metricval = N'Service not found or state undetermined.'

							select @metricthreshold = N'Server is vulnerable if SQL Server Agent is ' + @severityvalues + '.'
						end
						-- SQL Server Agent Login Account Acceptable
						else if (@metricid = 49)
						begin
							if (len(@severityvalues) > 0)
							begin
								select @strval=loginname from vwservice where snapshotid = @snapshotid and servicetype = 1
								if (len(@strval) > 0)
								begin
									if (charindex('''' + @strval + '''', @severityvalues) > 0)
										select @sevcode=@sevcodeok
									else
										select @sevcode=@severity
									select @metricval = N'Current login account is ' + @strval + '.'
								end
								else
									select @sevcode=@sevcodeok,
											@metricval = N'No login account or service not found.'
							end
							else
								select @sevcode=@sevcodeok,
										@metricval = N'No approved login account was provided.'

							select @metricthreshold = N'Server is vulnerable if SQL Server Agent Service Login Account is a user other than: ' + @severityvalues
						end
						-- Full-Text Search Enabled
						else if (@metricid = 50)
						begin
							select @severityvalues = N'Running'
							select @strval=[state] from vwservice where snapshotid = @snapshotid and servicetype in (2,10,14)
							if (len(@strval) > 0)
							begin
								if (@strval <> @severityvalues)
									select @sevcode=@sevcodeok
								else
									select @sevcode=@severity
								select @metricval = N'Current state is ' + @strval + '.'
							end
							else
								select @sevcode=@sevcodeok,
									@metricval = N'Service not found or state undetermined.'

							select @metricthreshold = N'Server is vulnerable if Full-Text Search is ' + @severityvalues + '.'
						end
						-- Full-Text Search Login Account Acceptable
						else if (@metricid = 51)
						begin
							if (len(@severityvalues) > 0)
							begin
								-- process all FT services because collector can collect multiples in a snapshot if 2000 and 2005 or later are installed together
								select @sql = N'declare svclogincursor cursor for
													select displayname, loginname 
														from vwservice 
														where snapshotid = ' + convert(nvarchar, @snapshotid) + N'
															and servicetype in (2,10,14)
															and lower(loginname) not in (' + lower(@severityvalues) + N')'

								exec (@sql)

								open svclogincursor
								fetch next from svclogincursor into @strval, @strval2

								select @intval2 = 0
								while @@fetch_status = 0
								begin
									if (@intval2 = 1 or len(@metricval) + len(@strval) > 1010)
									begin
										if @intval2 = 0
											select @metricval = @metricval + N', more...',
													@intval2 = 1
									end
									else
										select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval2 + N''' for ''' + @strval + N''''

									if (@isadmin = 1)
										insert into policyassessmentdetail ( policyid,
																			 assessmentid,
																			 metricid,
																			 snapshotid,
																			 detailfinding,
																			 databaseid,
																			 objecttype,
																			 objectid,
																			 objectname )
																	values ( @policyid,
																			 @assessmentid,
																			 @metricid,
																			 @snapshotid,
																			 N'Login Account is ''' + @strval2 + N''' for ''' + @strval + N'''',
																			 null, -- database ID,
																			 N'iLOGN', -- object type
																			 null,
																			 @strval )

									fetch next from svclogincursor into @strval, @strval2
								end

								close svclogincursor
								deallocate svclogincursor	

								if (len(@metricval) = 0)
									select @sevcode=@sevcodeok,
											@metricval = 'The service login account is acceptable or the service is not installed.'
								else
									select @sevcode=@severity,
											@metricval = N'Current login account is ' + @metricval + '.'
							end
							else
								select @sevcode=@sevcodeok,
										@metricval = N'No approved login account was provided.'

							select @metricthreshold = N'Server is vulnerable if Full-Text Search Service Login Account is a user other than: ' + @severityvalues
						end
						-- SQL Server Browser Enabled
						else if (@metricid = 52)
						begin
							select @severityvalues = N'Running'
							select @strval=[state] from vwservice where snapshotid = @snapshotid and servicetype = 4
							if (len(@strval) > 0)
							begin
								if (@strval <> @severityvalues)
									select @sevcode=@sevcodeok
								else
									select @sevcode=@severity
								select @metricval = N'Current state is ' + @strval + '.'
							end
							else
								select @sevcode=@sevcodeok,
									@metricval = N'Service not found or state undetermined.'

							select @metricthreshold = N'Server is vulnerable if SQL Server Browser is ' + @severityvalues + '.'
						end
						-- SQL Server Browser Login Account Acceptable
						else if (@metricid = 53)
						begin
							if (len(@severityvalues) > 0)
							begin
								select @strval=loginname from vwservice where snapshotid = @snapshotid and servicetype = 4
								if (len(@strval) > 0)
								begin
									if (charindex('''' + @strval + '''', @severityvalues) > 0)
										select @sevcode=@sevcodeok
									else
										select @sevcode=@severity
									select @metricval = N'Current login account is ' + @strval + '.'
								end
								else
									select @sevcode=@sevcodeok,
											@metricval = N'No login account or service not found.'
							end
							else
								select @sevcode=@sevcodeok,
										@metricval = N'No approved login account was provided.'

							select @metricthreshold = N'Server is vulnerable if SQL Server Browser Service Login Account is a user other than: ' + @severityvalues
						end
						-- Audited Servers
						else if (@metricid = 54)
						begin
							-- any server found is valid. See post server loop processing for findings.
							select @sevcode=@sevcodeok
							select @metricval = N'Server has audit data.'

							select @metricthreshold = N'Assessment may not be valid if all servers do not have audit data.'
						end
						-- Complete Audits
						else if (@metricid = 55)
						begin
							select @strval=@status, @severityvalues=N'S'
							if (@strval = @severityvalues)
								select @sevcode=@sevcodeok,
										@metricval = N'Snapshot was successful'
							else
								select @sevcode=@severity,
										@metricval = N'Snapshot has warnings'

							-- Make sure snapshot is a 2.0 snapshot or issue finding
							if (@collectorversion is not null and @collectorversion >= '2.')
								-- don't set sevcode to ok because it may already be a finding for warnings
								select @metricval = @metricval + N'.'
							else
							begin
								select @sevcode=@severity,
										@metricval = @metricval + case when right(@metricval,10)='successful' then N', but' else N' and' end + 
																	N' was created using SQLsecure version 1.2 or earlier.'
							end
							
							--check to see if weak password detection was enabled during this snapshot
							if (@weakpasswordenabled != 'Y')
							begin
								select @sevcode=@severity,
										@metricval = @metricval + N'   Password Health may have been omitted because Weak password detection was disabled during data collection.'
							end
							

							-- Check that filters do not exclude anything
							declare @vermatch nvarchar(256), @currule nvarchar(256), @rule nvarchar(256), @classname nvarchar(128), @scope nvarchar(64)

							declare filtercursor cursor for
								select rulename, class, classname, scope, matchstring
									from SQLsecure.dbo.vwsnapshotfilterrules
									where snapshotid = @snapshotid
									order by rulename, classname

							open filtercursor
							fetch next from filtercursor into @rule, @intval, @classname, @scope, @strval

							select @currule = N''
							while @@fetch_status = 0
							begin
								if (@intval <> 43)	-- skip checking extended stored procedures because they are system only and have no match strings
									if (@scope <> N'A' or len(@strval) > 0)	-- if not processing all or there is a match string then it is a finding
										 select @sevcode = @severity 

								-- add all filter rules to the output regardless of finding
								select @scope = case @scope when N'A' then N'All' when N'S' then N'System' else N'User' end
								select @strval = case when @rule = @currule then N', ' else N'  Filter:' + @rule + N': ' end + 
													@scope  + N' ' + @classname + case when len(@strval) = 0 then N'' else N' matching ''' + @strval + '''' end
								if (@intval2 = 1 or len(@metricval) + len(@strval) > 1010)
								begin
									if @intval2 = 0
										select @metricval = @metricval + N', more...',
												@intval2 = 1
								end
								else
									select @metricval = @metricval + @strval
									
								if (@isadmin = 1)
									insert into policyassessmentdetail ( policyid,
																		 assessmentid,
																		 metricid,
																		 snapshotid,
																		 detailfinding,
																		 databaseid,
																		 objecttype,
																		 objectid,
																		 objectname )
																values ( @policyid,
																		 @assessmentid,
																		 @metricid,
																		 @snapshotid,
																		 N'Filter found: ' + @rule + N': ' + @scope  + N' ' + @classname + case when len(@strval) = 0 then N'' else N' matching ''' + @strval end + N'''',
																		 null, -- database ID,
																		 N'RL', -- no filter rule object type
																		 null,
																		 @rule )

								select @currule = @rule

								fetch next from filtercursor into @rule, @intval, @classname, @scope, @strval
							end

							close filtercursor
							deallocate filtercursor	

							-- if everything else is ok, then check and make sure all objects were included
							if (@sevcode < @severity)
							begin
								select @intval=count(*)
									from filterruleclass a left join vwsnapshotfilterrules b on (a.objectclass = b.class and b.snapshotid=@snapshotid)
									where b.snapshotid is null
								if (@version > N'9.' or @version < N'6.')
								begin
									if (@intval <> 6)
										select @sevcode = @severity,
												@metricval = @metricval + N'  Some objects may have been omitted by filtering during data collection.'
								end
								else
								begin
									if (@intval <> 13)
										select @sevcode = @severity,
												@metricval = @metricval + N'  Some objects may have been omitted by filtering during data collection.'
								end
							end
							
							select @metricthreshold = N'Server may be vulnerable if Snapshot status is not ' + dbo.getsnapshotstatus(@severityvalues) + ' or data collection filters are excluding data.'
						end
						-- Baseline Data
						else if (@metricid = 56)
						begin
							select @strval=isnull(@baseline,N''), @severityvalues = N'Y'
							if (@strval = @severityvalues)
								select @sevcode=@sevcodeok,
										@metricval = N'Snapshot is marked as baseline.'
							else
								select @sevcode=@severity,
										@metricval = N'Snapshot is not marked as baseline.' + @strval

							select @metricthreshold = N'Audit data may not be valid if snapshot is not marked as baseline.'
						end
						-- Password Policy Enabled
						else if (@metricid = 57)
						begin
							select @severityvalues = N'N'
							declare logincursor cursor for
								select name
									from vwserverprincipal
									where snapshotid = @snapshotid and  type = N'S' and ispolicychecked = @severityvalues

							open logincursor
							fetch next from logincursor into @strval
							
							select @intval2 = 0
							while @@fetch_status = 0
							begin
								if (@intval2 = 1 or len(@metricval) + len(@strval) > 1010)
								begin
									if @intval2 = 0
										select @metricval = @metricval + N', more...',
												@intval2 = 1
								end
								else
									select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval + N''''

								if (@isadmin = 1)
									insert into policyassessmentdetail ( policyid,
																		 assessmentid,
																		 metricid,
																		 snapshotid,
																		 detailfinding,
																		 databaseid,
																		 objecttype,
																		 objectid,
																		 objectname )
																values ( @policyid,
																		 @assessmentid,
																		 @metricid,
																		 @snapshotid,
																		 N'Password policy not enforced on ''' + @strval + N'''',
																		 null, -- database ID,
																		 N'iLOGN', -- object type
																		 null,
																		 @strval )

								fetch next from logincursor into @strval
							end

							close logincursor
							deallocate logincursor	

							if (len(@metricval) = 0)
								select @sevcode=@sevcodeok,
										@metricval = 'All SQL Logins have password policy enforced.'
							else
								select @sevcode=@severity,
										@metricval = N'Password policy not enforced on ' + @metricval

							select @metricthreshold = N'Server is vulnerable if password policy is not enforced on all SQL Logins.'
						end
						-- 'Public database role permissions
						else if (@metricid = 58)
						begin
							select @severityvalues = N'Y'
							declare databasecursor cursor for
								select distinct b.databasename 
									from databaseobjectpermission a, sqldatabase b
									where a.snapshotid = @snapshotid
										and a.grantee = 0			--public uid is 0
										and a.snapshotid = b.snapshotid
										and a.dbid = b.dbid
								union
								select distinct b.databasename 
									from databaserolemember a, sqldatabase b
									where a.snapshotid = @snapshotid
										and a.rolememberuid = 0		--public uid is 0
										and a.snapshotid = b.snapshotid
										and a.dbid = b.dbid
 							open databasecursor
							fetch next from databasecursor into @strval

							select @intval2 = 0
							while @@fetch_status = 0
							begin
								if (@intval2 = 1 or len(@metricval) + len(@strval) > 1010)
								begin
									if @intval2 = 0
										select @metricval = @metricval + N', more...',
												@intval2 = 1
								end
								else
									select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval + N''''

								if (@isadmin = 1)
									insert into policyassessmentdetail ( policyid,
																		 assessmentid,
																		 metricid,
																		 snapshotid,
																		 detailfinding,
																		 databaseid,
																		 objecttype,
																		 objectid,
																		 objectname )
																values ( @policyid,
																		 @assessmentid,
																		 @metricid,
																		 @snapshotid,
																		 N'Public has permissions on ''' + @strval + N'''',
																		 null, -- database ID,
																		 N'DB', -- object type
																		 null,
																		 @strval )

								fetch next from databasecursor into @strval
							end

							close databasecursor
							deallocate databasecursor	

							if (len(@metricval) = 0)
								select @sevcode=@sevcodeok,
										@metricval = 'No databases found with public permissions.'
							else
								select @sevcode=@severity,
										@metricval = N'Public has permissions on ' + @metricval

							select @metricthreshold = N'Server is vulnerable if the public role has been granted any permissions or is a role member.'
						end
						-- 'Blank Passwords
						else if (@metricid = 59)
						begin
							select @severityvalues = N'Y'
							declare logincursor cursor for
								select name
									from vwserverprincipal
									where snapshotid = @snapshotid and  type = N'S' and ispasswordnull = @severityvalues

							open logincursor
							fetch next from logincursor into @strval

							select @intval2 = 0
							while @@fetch_status = 0
							begin
								if (@intval2 = 1 or len(@metricval) + len(@strval) > 1010)
								begin
									if @intval2 = 0
										select @metricval = @metricval + N', more...',
												@intval2 = 1
								end
								else
									select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval + N''''

								if (@isadmin = 1)
									insert into policyassessmentdetail ( policyid,
																		 assessmentid,
																		 metricid,
																		 snapshotid,
																		 detailfinding,
																		 databaseid,
																		 objecttype,
																		 objectid,
																		 objectname )
																values ( @policyid,
																		 @assessmentid,
																		 @metricid,
																		 @snapshotid,
																		 N'Login with blank password found: ''' + @strval + N'''',
																		 null, -- database ID,
																		 N'iLOGN', -- object type
																		 null,
																		 @strval )
																	 
								fetch next from logincursor into @strval
							end

							close logincursor
							deallocate logincursor	

							if (len(@metricval) = 0)
								select @sevcode=@sevcodeok,
										@metricval = N'No SQL Logins have blank passwords.'
							else
								select @sevcode=@severity,
										@metricval = N'These SQL Logins have blank passwords: ' + @metricval

							select @metricthreshold = N'Server is vulnerable if SQL Logins are found that have a blank password.'
						end
						-- 'Fixed roles assigned to public or guest
						else if (@metricid = 60)
						begin
							select @severityvalues = N'Y'
							declare databasecursor cursor for
								select distinct b.databasename 
									from databaserolemember a, sqldatabase b 
									where a.snapshotid = @snapshotid 
										and a.rolememberuid in (0, 2)		--public uid is 0 and guest uid is 2
										and a.groupuid > 16383		--database fixed roles are 16384-16393
										and a.groupuid < 16394 
										and a.snapshotid = b.snapshotid 
										and a.dbid = b.dbid 
 							open databasecursor
							fetch next from databasecursor into @strval
							select @intval2 = 0
							while @@fetch_status = 0
							begin
								if (@intval2 = 1 or len(@metricval) + len(@strval) > 1010)
								begin
									if @intval2 = 0
										select @metricval = @metricval + N', more...',
												@intval2 = 1
								end
								else
									select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval + N''''

								if (@isadmin = 1)
									insert into policyassessmentdetail ( policyid,
																		 assessmentid,
																		 metricid,
																		 snapshotid,
																		 detailfinding,
																		 databaseid,
																		 objecttype,
																		 objectid,
																		 objectname )
																values ( @policyid,
																		 @assessmentid,
																		 @metricid,
																		 @snapshotid,
																		 N'Public or guest is a member of a fixed role on ''' + @strval + N'''',
																		 null, -- database ID,
																		 N'DB', -- object type
																		 null,
																		 @strval )

								fetch next from databasecursor into @strval
							end

							close databasecursor
							deallocate databasecursor	

							if (len(@metricval) = 0)
								select @sevcode=@sevcodeok,
										@metricval = 'No databases found with public or guest assigned to fixed database roles.'
							else
								select @sevcode=@severity,
										@metricval = N'Public or guest is a member of a fixed role on ' + @metricval

							select @metricthreshold = N'Server is vulnerable if the public role or the guest user is a member of any fixed database role.'
						end
						-- Builtin/administrators
						else if (@metricid = 61)
						begin
							select @severityvalues = N'Y'
							select @intval=1 
								from serverrolemember a,
									serverprincipal b,		-- Builtin\admin principal
									serverprincipal c		-- sysadmin principal
								where a.snapshotid = @snapshotid 
									and a.snapshotid = b.snapshotid 
									and a.memberprincipalid = b.principalid 
									and b.sid = @builtinadminsid 
									and a.snapshotid = c.snapshotid 
									and a.principalid = c.principalid 
									and c.sid = @sysadminsid 

							if (@intval = 0)
								select @sevcode=@sevcodeok,
										@strval = N'N'
							else
								select @sevcode=@severity,
										@strval = N'Y'

							select @metricval = dbo.getyesnotext(@strval)
							select @metricthreshold = N'Server is vulnerable if BUILTIN\Administrators is a member of the sysadmin server role.'
						end
		--************************************************* version 2.5 security checks
						-- Password Expiration Enabled
						else if (@metricid = 62)
						begin
							select @severityvalues = N'N'
							declare logincursor cursor for
								select name
									from vwserverprincipal
									where snapshotid = @snapshotid and  type = N'S' and isexpirationchecked = @severityvalues

							open logincursor
							fetch next from logincursor into @strval

							select @intval2 = 0
							while @@fetch_status = 0
							begin
								if (@intval2 = 1 or len(@metricval) + len(@strval) > 1010)
								begin
									if @intval2 = 0
										select @metricval = @metricval + N', more...',
												@intval2 = 1
								end
								else
									select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval + N''''

								if (@isadmin = 1)
									insert into policyassessmentdetail ( policyid,
																		 assessmentid,
																		 metricid,
																		 snapshotid,
																		 detailfinding,
																		 databaseid,
																		 objecttype,
																		 objectid,
																		 objectname )
																values ( @policyid,
																		 @assessmentid,
																		 @metricid,
																		 @snapshotid,
																		 N'Password expiration not enabled for ''' + @strval + N'''',
																		 null, -- database ID,
																		 N'iLOGN', -- object type
																		 null,
																		 @strval )

								fetch next from logincursor into @strval
							end

							close logincursor
							deallocate logincursor	

							if (len(@metricval) = 0)
								select @sevcode=@sevcodeok,
										@metricval = 'All SQL Logins have password expiration enabled.'
							else
								select @sevcode=@severity,
										@metricval = N'Password expiration not enabled for ' + @metricval

							select @metricthreshold = N'Server is vulnerable if SQL Logins are found that do not implement password expiration.'
						end

						-- Server is Domain Controller
						else if (@metricid = 63)
						begin
							select @severityvalues = N'Y'
							if (@dc <> @severityvalues)
								select @sevcode=@sevcodeok
							else
								select @sevcode=@severity

							select @metricval = dbo.getyesnotext(@dc)
							select @metricthreshold = N'Server is vulnerable if it is a primary or backup domain controller.'
						end

		--************************************************* version 2.6 security checks
						-- Active Directory Helper Service Login Account Acceptable
						else if (@metricid = 67)
						begin
							if (len(@severityvalues) > 0)
							begin
								select @strval=loginname from vwservice where snapshotid = @snapshotid and servicetype in (8,13)
								if (len(@strval) > 0)
								begin
									if (charindex('''' + @strval + '''', @severityvalues) > 0)
										select @sevcode=@sevcodeok
									else
										select @sevcode=@severity
									select @metricval = N'Current login account is ' + @strval + '.'
								end
								else
									select @sevcode=@sevcodeok,
											@metricval = N'No login account or service not found.'
							end
							else
								select @sevcode=@sevcodeok,
										@metricval = N'No approved login account was provided.'

							select @metricthreshold = N'Server is vulnerable if Active Directory Helper Service Login Account is a user other than: ' + @severityvalues
						end
						-- Reporting Services Service Login Account Acceptable
						else if (@metricid = 68)
						begin
							if (len(@severityvalues) > 0)
							begin
								select @strval=loginname from vwservice where snapshotid = @snapshotid and servicetype = 6
								if (len(@strval) > 0)
								begin
									if (charindex('''' + @strval + '''', @severityvalues) > 0)
										select @sevcode=@sevcodeok
									else
										select @sevcode=@severity
									select @metricval = N'Current login account is ' + @strval + '.'
								end
								else
									select @sevcode=@sevcodeok,
											@metricval = N'No login account or service not found.'
							end
							else
								select @sevcode=@sevcodeok,
										@metricval = N'No approved login account was provided.'

							select @metricthreshold = N'Server is vulnerable if Reporting Services Service Login Account is a user other than: ' + @severityvalues
						end
						-- Volume Shadow Copy Service (VSS) Writer Login Account Acceptable
						else if (@metricid = 69)
						begin
							if (len(@severityvalues) > 0)
							begin
								select @strval=loginname from vwservice where snapshotid = @snapshotid and servicetype = 7
								if (len(@strval) > 0)
								begin
									if (charindex('''' + @strval + '''', @severityvalues) > 0)
										select @sevcode=@sevcodeok
									else
										select @sevcode=@severity
									select @metricval = N'Current login account is ' + @strval + '.'
								end
								else
									select @sevcode=@sevcodeok,
											@metricval = N'No login account or service not found.'
							end
							else
								select @sevcode=@sevcodeok,
										@metricval = N'No approved login account was provided.'

							select @metricthreshold = N'Server is vulnerable if VSS Writer Service Login Account is a user other than: ' + @severityvalues
						end
						-- VSS Writer Enabled
						else if (@metricid = 70)
						begin
							select @severityvalues = N'Running'
							select @strval=[state] from vwservice where snapshotid = @snapshotid and servicetype = 7
							if (len(@strval) > 0)
							begin
								if (@strval <> @severityvalues)
									select @sevcode=@sevcodeok
								else
									select @sevcode=@severity
								select @metricval = N'Current state is ' + @strval + '.'
							end
							else
								select @sevcode=@sevcodeok,
									@metricval = N'Service not found or state undetermined.'

							select @metricthreshold = N'Server is vulnerable if VSS Writer is ' + @severityvalues + '.'
						end
						-- Unauthorized Accounts are sysadmins
						else if (@metricid = 71)
						begin
							if (len(@severityvalues) > 0)
							begin
								select @sql = N'declare sysadmincursor cursor for
													select distinct name 
														from #sysadminstbl 
														where lower(name) not in (' + lower(@severityvalues) + N')'
 								if (charindex('%', @severityvalues) > 0)
								begin
									select @strval = lower(@severityvalues),
											@intval = charindex(''',''',@strval)
									while (@intval > 0)
									begin
										select @intval2 = charindex('%', @strval)
										if (@intval2 < @intval)		-- this item contains a wildcard
											select @sql = @sql + ' and lower(name) not like ' + substring(@strval,1,@intval)
											select @strval = substring(@strval,@intval+2, len(@strval)-(@intval+1))
											select @intval = charindex(''',''',@strval)
									end
									if (len(@strval) > 0)
									begin
										select @intval2 = charindex('%', @strval)
										if (@intval2 > 0)		-- this item contains a wildcard
											select @sql = @sql + ' and lower(name) not like ' + @strval
									end
								end
								select @sql = @sql + N' order by name'

								exec (@sql)
								open sysadmincursor

								select @intval = 0, @intval2 = 0
								fetch next from sysadmincursor into @strval
								while @@fetch_status = 0
								begin
									select @intval = @intval + 1
									if (len(@metricval) + len(@strval) + len(@strval2) > 1400)
									begin
										if @intval2 = 0
											select @metricval = @metricval + N', more...',
													@intval2 = 1
									end
									else
										select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval + N''''

									if (@isadmin = 1)
										insert into policyassessmentdetail ( policyid,
																			 assessmentid,
																			 metricid,
																			 snapshotid,
																			 detailfinding,
																			 databaseid,
																			 objecttype,
																			 objectid,
																			 objectname )
																	values ( @policyid,
																			 @assessmentid,
																			 @metricid,
																			 @snapshotid,
																			 N'Unauthorized sysadmin member found: ''' + @strval + N'''',
																			 null, -- database ID,
																			 N'iLOGN', -- object type
																			 null,
																			 @strval )

									fetch next from sysadmincursor into @strval
								end

								close sysadmincursor
								deallocate sysadmincursor

								if (len(@metricval) = 0)
									select @sevcode=@sevcodeok,
											@metricval = N'No logins found.'
								else
									select @sevcode=@severity,
											@metricval = N'sysadmin role has unauthorized members: ' + @metricval
 							end
							else
								select @sevcode=@sevcodeok,
											@metricval = N'No list of unapproved logins was provided.'

							select @metricthreshold = N'Server is vulnerable if the sysadmin server role members include other logins than: ' + @severityvalues
						end
						-- sa Account disabled  (this is a subset of metric 16)
						else if (@metricid = 72)
						begin
							-- only apply this check if the version is 2005 or greater
							if (@version > N'9.' or @version < N'6.')
							begin
								-- check to make sure the sa account is either disabled or renamed
								select @severityvalues = N'N'
								select @metricval=[name], @strval=[disabled] from serverprincipal where snapshotid = @snapshotid and sid = 0x01

								if (lower(@metricval) = N'sa' and @strval = @severityvalues)
									select @sevcode=@severity
								else if (lower(@metricval) <> N'sa' and @strval = @severityvalues)
									select @sevcode= 2
								else
									select @sevcode=@sevcodeok

								select @metricval = N'The sa account is enabled.'
							end
							else
							begin
									select @sevcode = @sevcodeok,
											@metricval = N'The sa account is always enabled on SQL Server 2000.'
							end

							select @metricthreshold = N'Server is vulnerable if the sa account is enabled.'
						end
						-- 'ALTER TRACE permissions
						else if (@metricid = 73)
						begin
							-- only apply this check if the version is 2005 or greater
							if (@version > N'9.' or @version < N'6.')
							begin
								if (len(@severityvalues) > 0)
								begin
									select @sql = N'declare altertracecursor cursor for
														select b.name 
															from serverpermission a, serverprincipal b
															where a.snapshotid=' + convert(nvarchar,@snapshotid) + N' 
																and a.classid=100 
																and a.permission = ''ALTER TRACE''
																and a.snapshotid = b.snapshotid
																and a.grantee = b.principalid
																and a.grantee not in (select id from #sysadminstbl)
																and lower(b.name) not in (' + lower(@severityvalues) + N')'
 									if (charindex('%', @severityvalues) > 0)
									begin
										select @strval = lower(@severityvalues),
												@intval = charindex(''',''',@strval)
										while (@intval > 0)
										begin
											select @intval2 = charindex('%', @strval)
											if (@intval2 < @intval)		-- this item contains a wildcard
												select @sql = @sql + ' and lower(b.name) not like ' + substring(@strval,1,@intval)
												select @strval = substring(@strval,@intval+2, len(@strval)-(@intval+1))
												select @intval = charindex(''',''',@strval)
										end
										if (len(@strval) > 0)
										begin
											select @intval2 = charindex('%', @strval)
											if (@intval2 > 0)		-- this item contains a wildcard
												select @sql = @sql + ' and lower(b.name) not like ' + @strval
										end
									end
									select @sql = @sql + N' order by a.permission'

									exec (@sql)
 									open altertracecursor
									fetch next from altertracecursor into @strval

									select @intval2 = 0
									while @@fetch_status = 0
									begin
										if (@intval2 = 1 or len(@metricval) + len(@strval) > 1400)
										begin
											if @intval2 = 0
												select @metricval = @metricval + N', more...',
														@intval2 = 1
										end
										else
											select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval + N''''

										if (@isadmin = 1)
											insert into policyassessmentdetail ( policyid,
																				 assessmentid,
																				 metricid,
																				 snapshotid,
																				 detailfinding,
																				 databaseid,
																				 objecttype,
																				 objectid,
																				 objectname )
																		values ( @policyid,
																				 @assessmentid,
																				 @metricid,
																				 @snapshotid,
																				 N'ALTER TRACE permission granted to ''' + @strval + N'''',
																				 null, -- database ID,
																				 N'iSRV', -- object type
																				 null,
																				 @strval )

										fetch next from altertracecursor into @strval
									end

									close altertracecursor
									deallocate altertracecursor	

									if (len(@metricval) = 0)
										select @sevcode=@sevcodeok,
												@metricval = 'No ALTER TRACE permissions found.'
									else
										select @sevcode=@severity,
												@metricval = N'Unauthorized logins found: ' + @metricval
 								end
								else
									select @sevcode=@sevcodeok,
												@metricval = N'No list of approved logins was provided.'
							end
							else
							begin
									select @sevcode = @sevcodeok,
											@metricval = N'The ALTER TRACE permission does not exist on SQL Server 2000.'
							end

							select @metricthreshold = N'Server is vulnerable if the ALTER TRACE permission has been granted to a login that is not a sysadmin and is not: ' + @severityvalues
						end
						-- CONTROL SERVER permissions
						else if (@metricid = 74)
						begin
							-- only apply this check if the version is 2005 or greater
							if (@version > N'9.' or @version < N'6.')
							begin
								if (len(@severityvalues) > 0)
								begin
									select @sql = N'declare controlcursor cursor for
														select b.name 
															from serverpermission a, serverprincipal b
															where a.snapshotid=' + convert(nvarchar,@snapshotid) + N' 
																and a.classid=100 
																and a.permission = ''CONTROL SERVER''
																and a.snapshotid = b.snapshotid
																and a.grantee = b.principalid
																and a.grantee not in (select id from #sysadminstbl)
																and lower(b.name) not in (' + lower(@severityvalues) + N')'
 									if (charindex('%', @severityvalues) > 0)
									begin
										select @strval = lower(@severityvalues),
												@intval = charindex(''',''',@strval)
										while (@intval > 0)
										begin
											select @intval2 = charindex('%', @strval)
											if (@intval2 < @intval)		-- this item contains a wildcard
												select @sql = @sql + ' and lower(b.name) not like ' + substring(@strval,1,@intval)
												select @strval = substring(@strval,@intval+2, len(@strval)-(@intval+1))
												select @intval = charindex(''',''',@strval)
										end
										if (len(@strval) > 0)
										begin
											select @intval2 = charindex('%', @strval)
											if (@intval2 > 0)		-- this item contains a wildcard
												select @sql = @sql + ' and lower(b.name) not like ' + @strval
										end
									end
									select @sql = @sql + N' order by a.permission'

									exec (@sql)
 									open controlcursor
									fetch next from controlcursor into @strval

									select @intval2 = 0
									while @@fetch_status = 0
									begin
										if (@intval2 = 1 or len(@metricval) + len(@strval) > 1400)
										begin
											if @intval2 = 0
												select @metricval = @metricval + N', more...',
														@intval2 = 1
										end
										else
											select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval + N''''

										if (@isadmin = 1)
											insert into policyassessmentdetail ( policyid,
																				 assessmentid,
																				 metricid,
																				 snapshotid,
																				 detailfinding,
																				 databaseid,
																				 objecttype,
																				 objectid,
																				 objectname )
																		values ( @policyid,
																				 @assessmentid,
																				 @metricid,
																				 @snapshotid,
																				 N'CONTROL SERVER permission granted to ''' + @strval + N'''',
																				 null, -- database ID,
																				 N'iSRV', -- object type
																				 null,
																				 @strval )

										fetch next from controlcursor into @strval
									end

									close controlcursor
									deallocate controlcursor	

									if (len(@metricval) = 0)
										select @sevcode=@sevcodeok,
												@metricval = 'No CONTROL SERVER permissions found.'
									else
										select @sevcode=@severity,
												@metricval = N'Unauthorized logins found: ' + @metricval
 								end
								else
									select @sevcode=@sevcodeok,
												@metricval = N'No list of approved logins was provided.'
							end
							else
							begin
									select @sevcode = @sevcodeok,
											@metricval = N'The CONTROL SERVER permission does not exist on SQL Server 2000.'
							end

							select @metricthreshold = N'Server is vulnerable if the CONTROL SERVER permission has been granted to a login that is not a sysadmin and is not: ' + @severityvalues
						end
						-- xp_cmdshell Enabled
						else if (@metricid = 75)
						begin
							-- only apply this check if the version is 2005 or greater
							if (@version > N'9.' or @version < N'6.')
							begin
								select @strval = @xp_cmdshell, @severityvalues = N'N'
								if (@strval = @severityvalues)
									select @sevcode=@sevcodeok
								else
									select @sevcode=@severity

								select @metricval = dbo.getyesnotext(@strval)
							end
							else
							begin
									select @sevcode = @sevcodeok,
											@metricval = N'The ability to disable xp_cmdshell does not exist on SQL Server 2000.'
							end

							select @metricthreshold = N'Server is vulnerable if the xp_cmdshell extended stored procedure is enabled.'
						end

						-- Required Admin Accounts
						else if (@metricid = 76)
						begin
							if (len(@severityvalues) > 0)
							begin
								-- convert the severityvalues into a table for reverse selection
								select @intval=1;
								select @intval2 = charindex(''',''', @severityvalues);
								while @intval2 > 0
								begin
									insert into @tblval values(substring(@severityvalues, @intval+1, @intval2 - @intval - 1))
									select @intval = @intval2+2, @intval2 = charindex(''',''', @severityvalues, @intval+1)
								end
								insert into @tblval values(substring(@severityvalues, @intval+1, len(@severityvalues) - @intval - 1))

								declare reqadmincursor cursor for
									select val 
										from @tblval 
										where lower(val) not in (select lower(name) from #sysadminstbl)
										order by val

								open reqadmincursor
								fetch next from reqadmincursor into @strval

								select @intval2 = 0
								while @@fetch_status = 0
								begin
									if (@intval2 = 1 or len(@metricval) + len(@strval) > 1400)
									begin
										if @intval2 = 0
											select @metricval = @metricval + N', more...',
													@intval2 = 1
									end
									else
										select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval + N''''

									if (@isadmin = 1)
										insert into policyassessmentdetail ( policyid,
																			 assessmentid,
																			 metricid,
																			 snapshotid,
																			 detailfinding,
																			 databaseid,
																			 objecttype,
																			 objectid,
																			 objectname )
																	values ( @policyid,
																			 @assessmentid,
																			 @metricid,
																			 @snapshotid,
																			 N'Required administrative login not found: ''' + @strval + N'''',
																			 null, -- database ID,
																			 N'iSRV', -- object type
																			 null,
																			 @strval )

									fetch next from reqadmincursor into @strval
								end

								close reqadmincursor
								deallocate reqadmincursor	

								if (len(@metricval) = 0)
									select @sevcode=@sevcodeok,
											@metricval = 'All required administrative logins were found.'
								else
									select @sevcode=@severity,
											@metricval = N'These administrative logins are missing or are not sysadmin members:  ' + @metricval
							end
							else
								select @sevcode=@sevcodeok,
										@metricval = N'No required administrative login account was provided.'

							select @metricthreshold = N'Server is not standardized if the sysadmin server role does not include these accounts: ' + @severityvalues
						end

						-- Password Policy Enabled for sa
						else if (@metricid = 77)
						begin
							select @severityvalues = N'N'
							select @strval = ispolicychecked from serverprincipal where snapshotid=@snapshotid and sid = 0x01

							if (@strval <> @severityvalues)
								select @sevcode=@sevcodeok,
										@metricval = 'The password policy is enforced on the sa account.'
							else
								select @sevcode=@severity,
										@metricval = N'Password policy not enforced on the sa account.'

							select @metricthreshold = N'Server is vulnerable if password policy is not enforced on the sa account.'
						end

						-- Database files missing required administrative permissions
						else if (@metricid = 78)
						begin
							if (len(@severityvalues) > 0)
							begin
								select @intval=1;
								select @intval2 = charindex(''',''', @severityvalues);
								while @intval2 > 0
								begin
									insert into @tblval values(substring(@severityvalues, @intval+1, @intval2 - @intval - 1))
									select @intval = @intval2+2, @intval2 = charindex(''',''', @severityvalues, @intval+1)
								end
								insert into @tblval values(substring(@severityvalues, @intval+1, len(@severityvalues) - @intval - 1))

								declare filecursor cursor static for
									select o.objectname, a.val
										from (vwfilesystemobject o cross join @tblval a)
											where o.snapshotid = @snapshotid
												and o.objecttype= N'DB'
												and not exists (select 1 
																	from vwfilesystemobjectpermission p 
																	where p.snapshotid = @snapshotid 
																		and p.osobjectid = o.osobjectid 
																		and lower(p.grantee) like lower(a.val)
																		and p.filesystemrights = 2032127)	-- make sure they have full control
											order by o.objectname, a.val

								open filecursor

								select @intval = 0, @intval2 = 0
								fetch next from filecursor into @strval, @strval2
								while @@fetch_status = 0
								begin
									select @intval = @intval + 1
									if (len(@metricval) + len(@strval) + len(@strval2) > 1400)
									begin
										if @intval2 = 0
											select @metricval = @metricval + N', more...',
													@intval2 = 1
									end
									else
										select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval2 + N''' on ''' + @strval + N''''

									if (@isadmin = 1)
										insert into policyassessmentdetail ( policyid,
																			 assessmentid,
																			 metricid,
																			 snapshotid,
																			 detailfinding,
																			 databaseid,
																			 objecttype,
																			 objectid,
																			 objectname )
																	values ( @policyid,
																			 @assessmentid,
																			 @metricid,
																			 @snapshotid,
																			 N'Database file missing required permission found: ''' + @strval + N''' is missing Full Control for ''' + @strval2 + N'''',
																			 null, -- database ID,
																			 N'FI', -- no file object type
																			 null,
																			 @strval )

									fetch next from filecursor into @strval, @strval2
								end

								close filecursor
								deallocate filecursor

								if (len(@metricval) = 0)
									select @sevcode=@sevcodeok,
											@metricval = 'All database files have the required administrative account permissions.'
								else
									select @sevcode=@severity,
											@metricval = N'Administrative accounts are missing Full Control permission on database files: ' + @metricval
 							end
							else
								select @sevcode=@sevcodeok,
											@metricval = N'No list of required administrative accounts was provided.'

							select @metricthreshold = N'Server is not standardized if database files do not include Full Control permission for ' + @severityvalues
						end

						-- Executable files missing required administrative permissions
						else if (@metricid = 79)
						begin
							if (len(@severityvalues) > 0)
							begin
								select @intval=1;
								select @intval2 = charindex(''',''', @severityvalues);
								while @intval2 > 0
								begin
									insert into @tblval values(substring(@severityvalues, @intval+1, @intval2 - @intval - 1))
									select @intval = @intval2+2, @intval2 = charindex(''',''', @severityvalues, @intval+1)
								end
								insert into @tblval values(substring(@severityvalues, @intval+1, len(@severityvalues) - @intval - 1))

								declare filecursor cursor static for
									select o.objectname, a.val
										from (vwfilesystemobject o cross join @tblval a)
											where o.snapshotid = @snapshotid
												and o.objecttype= N'File'
												and (o.objectname like '%.exe' or o.objectname like '%.dll')
												and not exists (select 1 
																	from vwfilesystemobjectpermission p 
																	where p.snapshotid = @snapshotid 
																		and p.osobjectid = o.osobjectid 
																		and lower(p.grantee) like lower(a.val)
																		and p.filesystemrights = 2032127)	-- make sure they have full control
											order by o.objectname, a.val

								open filecursor

								select @intval = 0, @intval2 = 0
								fetch next from filecursor into @strval, @strval2
								while @@fetch_status = 0
								begin
									select @intval = @intval + 1
									if (len(@metricval) + len(@strval) + len(@strval2) > 1400)
									begin
										if @intval2 = 0
											select @metricval = @metricval + N', more...',
													@intval2 = 1
									end
									else
										select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval2 + N''' on ''' + @strval + N''''

									if (@isadmin = 1)
										insert into policyassessmentdetail ( policyid,
																			 assessmentid,
																			 metricid,
																			 snapshotid,
																			 detailfinding,
																			 databaseid,
																			 objecttype,
																			 objectid,
																			 objectname )
																	values ( @policyid,
																			 @assessmentid,
																			 @metricid,
																			 @snapshotid,
																			 N'Executable file missing required permission found: ''' + @strval + N''' is missing Full Control for ''' + @strval2 + N'''',
																			 null, -- database ID,
																			 N'FI', -- no file object type
																			 null,
																			 @strval )

									fetch next from filecursor into @strval, @strval2
								end

								close filecursor
								deallocate filecursor

								if (len(@metricval) = 0)
									select @sevcode=@sevcodeok,
											@metricval = 'All executable files have the required administrative account permissions.'
								else
									select @sevcode=@severity,
											@metricval = N'Administrative accounts are missing Full Control permission on executable files: ' + @metricval
 							end
							else
								select @sevcode=@sevcodeok,
											@metricval = N'No list of required administrative accounts was provided.'

							select @metricthreshold = N'Server is not standardized if executable files do not include Full Control permissions for ' + @severityvalues
						end

						-- Registry Keys missing required administrative permissions
						else if (@metricid = 80)
						begin
							if (len(@severityvalues) > 0)
							begin
								select @intval=1;
								select @intval2 = charindex(''',''', @severityvalues);
								while @intval2 > 0
								begin
									insert into @tblval values(substring(@severityvalues, @intval+1, @intval2 - @intval - 1))
									select @intval = @intval2+2, @intval2 = charindex(''',''', @severityvalues, @intval+1)
								end
								insert into @tblval values(substring(@severityvalues, @intval+1, len(@severityvalues) - @intval - 1))

								declare filecursor cursor static for
									select o.objectname, a.val
										from (vwregistrykey o cross join @tblval a)
											where o.snapshotid = @snapshotid
												and not exists (select 1 
																	from vwregistrykeypermission p 
																	where p.snapshotid = @snapshotid 
																		and p.osobjectid = o.osobjectid 
																		and lower(p.grantee) like lower(a.val)
																		and p.filesystemrights = 983103)	-- make sure they have registry rights full control
											order by o.objectname, a.val

								open filecursor

								select @intval = 0, @intval2 = 0
								fetch next from filecursor into @strval, @strval2
								while @@fetch_status = 0
								begin
									select @intval = @intval + 1
									if (len(@metricval) + len(@strval) + len(@strval2) > 1400)
									begin
										if @intval2 = 0
											select @metricval = @metricval + N', more...',
													@intval2 = 1
									end
									else
										select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval2 + N''' on ''' + @strval + N''''

									if (@isadmin = 1)
										insert into policyassessmentdetail ( policyid,
																			 assessmentid,
																			 metricid,
																			 snapshotid,
																			 detailfinding,
																			 databaseid,
																			 objecttype,
																			 objectid,
																			 objectname )
																	values ( @policyid,
																			 @assessmentid,
																			 @metricid,
																			 @snapshotid,
																			 N'Registry key missing required administrative permission found: ''' + @strval + N''' is missing Full Control for ''' + @strval2 + N'''',
																			 null, -- database ID,
																			 N'Reg', -- no registry object type
																			 null,
																			 @strval )

									fetch next from filecursor into @strval, @strval2
								end

								close filecursor
								deallocate filecursor

								if (len(@metricval) = 0)
									select @sevcode=@sevcodeok,
											@metricval = 'All registry keys have the required administrative account permissions.'
								else
									select @sevcode=@severity,
											@metricval = N'Administrative accounts are missing Full Control permission on registry keys: ' + @metricval
 							end
							else
								select @sevcode=@sevcodeok,
											@metricval = N'No list of required administrative accounts was provided.'

							select @metricthreshold = N'Server is not standardized if registry keys do not include Full Control permission for ' + @severityvalues
						end

						-- Data Files on System Drive
						else if (@metricid = 81)
						begin
							select @sql = N'declare filecursor cursor static for
												select objectname
														from vwfilesystemobject
															where snapshotid = ' + convert(nvarchar, @snapshotid) + N'
																and objecttype= N''DB''
																and lower(left(objectname,2)) = ''' + lower(@systemdrive) + N''''
							-- This one is ok to process with no values although there isn't really a way to configure it like that
							-- It will just produce findings on any data file
							if (len(@severityvalues) > 0)
							begin
								select @sql =  @sql + N'				and lower(objectname) not in (' + lower(@severityvalues) + N')'
								if (charindex('%', @severityvalues) > 0)
								begin
									select @strval = lower(@severityvalues),
											@intval = charindex(''',''',@strval)
									while (@intval > 0)
									begin
										select @intval2 = charindex('%', @strval)
										if (@intval2 < @intval)		-- this item contains a wildcard
											select @sql = @sql + N' and lower(objectname) not like ' + substring(@strval,1,@intval)
										select @strval = substring(@strval,@intval+2, len(@strval)-(@intval+1))
										select @intval = charindex(''',''',@strval)
									end
									if (len(@strval) > 0)
									begin
										select @intval2 = charindex('%', @strval)
										if (@intval2 > 0)		-- this item contains a wildcard
											select @sql = @sql + N' and lower(objectname) not like ' + @strval
									end
								end
							end

							select @sql = @sql + N' order by objectname'

							exec (@sql)

							open filecursor

							select @intval = 0, @intval2 = 0
							fetch next from filecursor into @strval
							while @@fetch_status = 0
							begin
								select @intval = @intval + 1
								if (len(@metricval) + len(@strval) + len(@strval2) > 1400)
								begin
									if @intval2 = 0
										select @metricval = @metricval + N', more...',
												@intval2 = 1
								end
								else
									select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval + N''''

								if (@isadmin = 1)
									insert into policyassessmentdetail ( policyid,
																		 assessmentid,
																		 metricid,
																		 snapshotid,
																		 detailfinding,
																		 databaseid,
																		 objecttype,
																		 objectid,
																		 objectname )
																values ( @policyid,
																		 @assessmentid,
																		 @metricid,
																		 @snapshotid,
																		 N'Data file found on system drive: ''' + @strval + N'''',
																		 null, -- database ID,
																		 N'FI', -- no file object type
																		 null,
																		 @strval )

								fetch next from filecursor into @strval
							end

							close filecursor
							deallocate filecursor

							if (len(@metricval) = 0)
								select @sevcode=@sevcodeok,
										@metricval = N'There are no data files on the system drive.'
							else
								select @sevcode=@severity,
										@metricval = N'Data files found on the system drive: ' + @metricval

							select @metricthreshold = N'Server is vulnerable if any data files are located on the ' + @systemdrive + N' system drive and are not in ' + @severityvalues
						end

						-- SQL Server Installation on System Drive
						else if (@metricid = 82)
						begin
							if (len(rtrim(@systemdrive)) > 0)
							begin
								select @sql = N'declare sysfilecursor cursor static for
													select objectname
														from vwfilesystemobject
															where snapshotid = ' + convert(nvarchar, @snapshotid) + N'
																and objecttype= N''IDir''
																and lower(left(objectname,2)) = ''' + lower(@systemdrive) + N''''
								-- This one is ok to process with no values although there isn't really a way to configure it like that
								-- It will just produce findings on any data file
								if (len(@severityvalues) > 0)
								begin
									select @sql =  @sql + N'				and lower(objectname) not in (' + lower(@severityvalues) + N')'
									if (charindex('%', @severityvalues) > 0)
									begin
										select @strval = lower(@severityvalues),
												@intval = charindex(''',''',@strval)
										while (@intval > 0)
										begin
											select @intval2 = charindex('%', @strval)
											if (@intval2 < @intval)		-- this item contains a wildcard
												select @sql = @sql + N' and lower(objectname) not like ' + substring(@strval,1,@intval)
											select @strval = substring(@strval,@intval+2, len(@strval)-(@intval+1))
											select @intval = charindex(''',''',@strval)
										end
										if (len(@strval) > 0)
										begin
											select @intval2 = charindex('%', @strval)
											if (@intval2 > 0)		-- this item contains a wildcard
												select @sql = @sql + N' and lower(objectname) not like ' + @strval
										end
									end
								end

								select @sql = @sql + N' order by objectname'

								exec (@sql)

								open sysfilecursor

								select @intval = 0, @intval2 = 0
								fetch next from sysfilecursor into @strval
								while @@fetch_status = 0
								begin
									select @intval = @intval + 1
									if (len(@metricval) + len(@strval) + len(@strval2) > 1400)
									begin
										if @intval2 = 0
											select @metricval = @metricval + N', more...',
													@intval2 = 1
									end
									else
										select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval + N''''

									if (@isadmin = 1)
										insert into policyassessmentdetail ( policyid,
																			 assessmentid,
																			 metricid,
																			 snapshotid,
																			 detailfinding,
																			 databaseid,
																			 objecttype,
																			 objectid,
																			 objectname )
																	values ( @policyid,
																			 @assessmentid,
																			 @metricid,
																			 @snapshotid,
																			 N'Installation directory found on system drive: ''' + @strval + N'''',
																			 null, -- database ID,
																			 N'IDir', -- no file object type
																			 null,
																			 @strval )

									fetch next from sysfilecursor into @strval
								end

								close sysfilecursor
								deallocate sysfilecursor

								if (len(@metricval) = 0)
									select @sevcode=@sevcodeok,
											@metricval = N'No unapproved SQL Server installation directories found on the system drive.'
								else
									select @sevcode=@severity,
											@metricval = N'Unapproved SQL Server installation directories found on the system drive: ' + @metricval
							end
							else
								select @sevcode=@sevcodeok,
										@metricval = N'The system drive is not known. No check is performed.'

							select @metricthreshold = N'Server is vulnerable if the ' + case when len(rtrim(@systemdrive)) = 0 then N'' else @systemdrive + N' ' end + N' system drive hosts SQL Server installation directories other than: ' + @severityvalues
						end

						-- Ad Hoc Distributed Queries Enabled
						else if (@metricid = 83)
						begin
							-- only apply this check if the version is 2005 or greater
							if (@version > N'9.' or @version < N'6.')
							begin
								select @strval = @adhocqueries, @severityvalues = N'Y'
								if (@strval <> @severityvalues)
									select @sevcode=@sevcodeok,
										@metricval = N'Ad Hoc Distributed Queries are not enabled.'
								else
									select @sevcode=@severity,
										@metricval = N'Ad Hoc Distributed Queries are enabled.'
							end
							else
							begin
									select @sevcode = @sevcodeok,
											@metricval = N'The ability to disable Ad Hoc Distributed Queries does not exist on SQL Server 2000.'
							end

							select @metricthreshold = N'Server is vulnerable if Ad Hoc Distributed Queries are enabled.'
						end

						-- Unauthorized SQL Logins exist
						else if (@metricid = 84)
						begin
							select @sql = N'declare logincursor cursor for
												select name
													from vwserverprincipal
													where snapshotid = ' + convert(nvarchar, @snapshotid) + N'
														and type = N''S'''
							-- This is ok to process with no values even though there is no way to configure it that way
							-- It will just produce findings on anything although I guess there will always be an sa account and a finding.
							if (len(@severityvalues) > 0)
								select @sql =  @sql + N'		and name not in (' + @severityvalues + N')'

							select @sql = @sql + N' order by name'

							exec (@sql)

							open logincursor
							fetch next from logincursor into @strval
							
							select @intval2 = 0
							while @@fetch_status = 0
							begin
								if (@intval2 = 1 or len(@metricval) + len(@strval) > 1010)
								begin
									if @intval2 = 0
										select @metricval = @metricval + N', more...',
												@intval2 = 1
								end
								else
									select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval + N''''

								if (@isadmin = 1)
									insert into policyassessmentdetail ( policyid,
																		 assessmentid,
																		 metricid,
																		 snapshotid,
																		 detailfinding,
																		 databaseid,
																		 objecttype,
																		 objectid,
																		 objectname )
																values ( @policyid,
																		 @assessmentid,
																		 @metricid,
																		 @snapshotid,
																		 N'Unauthorized SQL Login found: ''' + @strval + N'''',
																		 null, -- database ID,
																		 N'iLOGN', -- object type
																		 null,
																		 @strval )

								fetch next from logincursor into @strval
							end

							close logincursor
							deallocate logincursor	

							if (len(@metricval) = 0)
								select @sevcode=@sevcodeok,
										@metricval = 'No unauthorized SQL Logins exist.'
							else
								select @sevcode=@severity,
										@metricval = N'Unauthorized SQL Logins exist: ' + @metricval

							select @metricthreshold = N'Server is vulnerable if SQL Logins exist that are not ' + @severityvalues
						end

						-- Public Server Role Has Permissions
						else if (@metricid = 85)
						begin
							-- only apply this check if the version is 2005 or greater
							if (@version > N'9.' or @version < N'6.')
							begin
								declare permissioncursor cursor for
									select a.permission,
											-- note this matches the functionality of isp_sqlsecure_getserverprincipalpermission for returning the object name
											case when classid=101 then dbo.getclasstype(a.classid) + N' ''' + (select name from serverprincipal where snapshotid = @snapshotid and principalid = a.majorid) + N'''' 
													when classid=105 then dbo.getclasstype(a.classid) + N' ''' + (select name from endpoint where snapshotid = @snapshotid and endpointid = a.majorid) + N'''' 
													else 'Server' end
										from serverpermission a, serverprincipal b
										where a.snapshotid=@snapshotid
											and a.snapshotid = b.snapshotid
											and a.grantee = b.principalid
											and a.grantee = 2		--	principalid of public server role is 2
										order by a.permission

								exec (@sql)
								open permissioncursor
								fetch next from permissioncursor into @strval, @strval2

								select @intval2 = 0
								while @@fetch_status = 0
								begin
									if (@intval2 = 1 or len(@metricval) + len(@strval) > 1400)
									begin
										if @intval2 = 0
											select @metricval = @metricval + N', more...',
													@intval2 = 1
									end
									else
										select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'' + @strval + N' on ' + @strval2

										if (@isadmin = 1)
											insert into policyassessmentdetail ( policyid,
																				 assessmentid,
																				 metricid,
																				 snapshotid,
																				 detailfinding,
																				 databaseid,
																				 objecttype,
																				 objectid,
																				 objectname )
																		values ( @policyid,
																				 @assessmentid,
																				 @metricid,
																				 @snapshotid,
																				 N'Public Permission found: ''' + @strval + N''' on ''' + @strval2 + N'''',
																				 null, -- database ID,
																				 N'iSRV', -- object type
																				 null,
																				 @strval )

									fetch next from permissioncursor into @strval, @strval2
								end

								close permissioncursor
								deallocate permissioncursor	

								if (len(@metricval) = 0)
									select @sevcode=@sevcodeok,
											@metricval = 'No permissions found.'
								else
									select @sevcode=@severity,
											@metricval = N'Public has permissions: ' + @metricval
							end
							else
							begin
									select @sevcode = @sevcodeok,
											@metricval = N'The public server role does not exist on SQL Server 2000.'
							end

							select @metricthreshold = N'Server is vulnerable if the public server role has been granted permissions.'
						end

						-- Databases Are Trustworthy
						else if (@metricid = 86)
						begin
							select @sql = N'declare dbcursor cursor static for
												select databasename
													from sqldatabase
													where snapshotid = ' + convert(nvarchar, @snapshotid) + N'
														and trustworthy = ''Y''
														and databasename not in (' + @severityvalues + N')
													order by databasename'
							exec (@sql)
							open dbcursor
							select @intval2 = 0
							fetch next from dbcursor into @strval
							while @@fetch_status = 0
							begin
								if (@intval2 = 1 or len(@metricval) + len(@strval) > 1010)
								begin
									if @intval2 = 0
										select @metricval = @metricval + N', more...',
												@intval2 = 1
								end
								else
									select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval + N''''

								if (@isadmin = 1)
									insert into policyassessmentdetail ( policyid,
																		 assessmentid,
																		 metricid,
																		 snapshotid,
																		 detailfinding,
																		 databaseid,
																		 objecttype,
																		 objectid,
																		 objectname )
																values ( @policyid,
																		 @assessmentid,
																		 @metricid,
																		 @snapshotid,
																		 N'Database with Trustworthy bit enabled: ''' + @strval + N'''',
																		 null, -- database ID,
																		 N'DB', -- object type
																		 null, -- object id
																		 @strval )
															         
								fetch next from dbcursor into @strval
							end

							if (len(@metricval) = 0)
								select @sevcode=@sevcodeok,
										@metricval = N'None found.'
							else
								select @sevcode=@severity,
										@metricval = N'Trustworthy databases: ' + @metricval

							select @metricthreshold = N'Server is vulnerable if any SQL Server 2005 or later databases are trustworthy other than: ' + @severityvalues

							close dbcursor
							deallocate dbcursor
						end

						-- Sysadmins Own Databases
						else if (@metricid = 87)
						begin
							select @sql = N'declare dbcursor cursor static for
												select databasename
													from sqldatabase
													where snapshotid = ' + convert(nvarchar, @snapshotid) + N'
														and owner in (select name from #sysadminstbl)
														and databasename not in (' + @severityvalues + N')
													order by databasename'
							exec (@sql)
							open dbcursor
							select @intval2 = 0
							fetch next from dbcursor into @strval
							while @@fetch_status = 0
							begin
								if (@intval2 = 1 or len(@metricval) + len(@strval) > 1010)
								begin
									if @intval2 = 0
										select @metricval = @metricval + N', more...',
												@intval2 = 1
								end
								else
									select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval + N''''

								if (@isadmin = 1)
									insert into policyassessmentdetail ( policyid,
																		 assessmentid,
																		 metricid,
																		 snapshotid,
																		 detailfinding,
																		 databaseid,
																		 objecttype,
																		 objectid,
																		 objectname )
																values ( @policyid,
																		 @assessmentid,
																		 @metricid,
																		 @snapshotid,
																		 N'Database owned by sysadmin: ''' + @strval + N'''',
																		 null, -- database ID,
																		 N'DB', -- object type
																		 null, -- object id
																		 @strval )
															         
								fetch next from dbcursor into @strval
							end

							if (len(@metricval) = 0)
								select @sevcode=@sevcodeok,
										@metricval = N'None found.'
							else
								select @sevcode=@severity,
										@metricval = N'Databases owned by system administrators: ' + @metricval

							select @metricthreshold = N'Server is vulnerable if a login who is a member of sysadmin or has the CONTROL SERVER permission owns any databases other than: ' + @severityvalues

							close dbcursor
							deallocate dbcursor
						end

						-- Sysadmins Own Trustworthy Databases
						else if (@metricid = 88)
						begin
							select @sql = N'declare dbcursor cursor static for
												select databasename
													from sqldatabase
													where snapshotid = ' + convert(nvarchar, @snapshotid) + N'
														and trustworthy = ''Y''
														and owner in (select name from #sysadminstbl)
														and databasename not in (' + @severityvalues + N')
													order by databasename'
							exec (@sql)
							open dbcursor
							select @intval2 = 0
							fetch next from dbcursor into @strval
							while @@fetch_status = 0
							begin
								if (@intval2 = 1 or len(@metricval) + len(@strval) > 1010)
								begin
									if @intval2 = 0
										select @metricval = @metricval + N', more...',
												@intval2 = 1
								end
								else
									select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval + N''''

								if (@isadmin = 1)
									insert into policyassessmentdetail ( policyid,
																		 assessmentid,
																		 metricid,
																		 snapshotid,
																		 detailfinding,
																		 databaseid,
																		 objecttype,
																		 objectid,
																		 objectname )
																values ( @policyid,
																		 @assessmentid,
																		 @metricid,
																		 @snapshotid,
																		 N'Trustworthy Database owned by sysadmin: ''' + @strval + N'''',
																		 null, -- database ID,
																		 N'DB', -- object type
																		 null, -- object id
																		 @strval )
															         
								fetch next from dbcursor into @strval
							end

							if (len(@metricval) = 0)
								select @sevcode=@sevcodeok,
										@metricval = N'None found.'
							else
								select @sevcode=@severity,
										@metricval = N'Databases owned by system administrators: ' + @metricval

							select @metricthreshold = N'Server is vulnerable if a login who is a member of sysadmin or has the CONTROL SERVER permission owns any trustworthy databases other than: ' + @severityvalues

							close dbcursor
							deallocate dbcursor
						end

						-- Public Roles Have Permissions on User Databases
						else if (@metricid = 89)
						begin
							select @severityvalues = N'Y'
							declare databasecursor cursor for
								select distinct b.databasename 
								from vwdatabaseobjectpermission a
								inner join sqldatabase b
									on ((a.dbid = b.dbid) and (a.snapshotid = b.snapshotid))
								inner join databaseschema c
									on ((a.schemaid = c.schemaid) and (a.dbid = c.dbid) and (a.snapshotid = c.snapshotid))
								where a.snapshotid = @snapshotid
									and a.grantee = 0			--public uid is 0
									and (a.isgrant = N'Y' OR a.isgrantwith = N'Y')
									and (b.databasename not in (N'master', N'msdb', N'distribution', N'tempdb'))
									and (c.schemaname <> N'sys')
 							open databasecursor
							fetch next from databasecursor into @strval

							select @intval2 = 0
							while @@fetch_status = 0
							begin
								if (@intval2 = 1 or len(@metricval) + len(@strval) > 1010)
								begin
									if @intval2 = 0
										select @metricval = @metricval + N', more...',
												@intval2 = 1
								end
								else
									select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval + N''''

								if (@isadmin = 1)
									insert into policyassessmentdetail ( policyid,
																		 assessmentid,
																		 metricid,
																		 snapshotid,
																		 detailfinding,
																		 databaseid,
																		 objecttype,
																		 objectid,
																		 objectname )
																values ( @policyid,
																		 @assessmentid,
																		 @metricid,
																		 @snapshotid,
																		 N'Public has permissions on ''' + @strval + N'''',
																		 null, -- database ID,
																		 N'DB', -- object type
																		 null,
																		 @strval )

								fetch next from databasecursor into @strval
							end

							close databasecursor
							deallocate databasecursor	

							if (len(@metricval) = 0)
								select @sevcode=@sevcodeok,
										@metricval = 'No user databases found with public permissions.'
							else
								select @sevcode=@severity,
										@metricval = N'Public has permissions on ' + @metricval

							select @metricthreshold = N'Server is vulnerable if the public role has been granted permissions on user databases.'
						end

						-- Dangerous Security Principals
						else if (@metricid = 90)
						begin
						declare @memberid int
						select @sql = N'declare databasecursor cursor for
								select dpmember.uid, dpmember.name, dpgroup.name
								from databaserolemember drm
								inner join databaseprincipal dpgroup 
									on ((dpgroup.snapshotid = drm.snapshotid) and (dpgroup.uid = drm.groupuid) and (dpgroup.dbid = drm.dbid))
								inner join databaseprincipal dpmember 
									on ((dpmember.snapshotid = drm.snapshotid) and (dpmember.uid = drm.rolememberuid) and (dpmember.dbid = drm.dbid))
								where 
								(
									drm.snapshotid = ' + convert(nvarchar, @snapshotid) + N' 
									and drm.dbid = 4				-- 4 = msdb
									and dpgroup.name in (''db_ssisadmin'', ''db_ssisltduser'', ''db_ssisoperator'', ''db_dtsadmin'', ''db_dtsltduser'', ''db_dtsoperator'')
									and dpmember.name in (' + @severityvalues + N')
								) 
								order by dpgroup.name'
							exec (@sql)							
 							open databasecursor
							fetch next from databasecursor into @memberid, @strval2, @strval3

							select @intval2 = 0
							while @@fetch_status = 0
							begin
								select @strval = @strval2 + ' in ' + @strval3
								if (@intval2 = 1 or len(@metricval) + len(@strval) > 1010)
								begin
									if @intval2 = 0
										select @metricval = @metricval + N', more...',
												@intval2 = 1
								end
								else
									select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval + N''''

								if (@isadmin = 1)
									insert into policyassessmentdetail ( policyid,
																		 assessmentid,
																		 metricid,
																		 snapshotid,
																		 detailfinding,
																		 databaseid,
																		 objecttype,
																		 objectid,
																		 objectname )
																values ( @policyid,
																		 @assessmentid,
																		 @metricid,
																		 @snapshotid,
																		 N'Dangerous security principals found in SSIS database roles: ''' + @strval + N'''',
																		 null, -- database ID,
																		 N'iDUSR', -- object type
																		 @memberid,
																		 @strval )

								fetch next from databasecursor into @memberid, @strval2, @strval3
							end

							close databasecursor
							deallocate databasecursor	

							if (len(@metricval) = 0)
								select @sevcode=@sevcodeok,
										@metricval = 'No dangerous members found in SSIS security roles.'
							else
								select @sevcode=@severity,
										@metricval = N'Dangerous security principals found in SSIS database roles: ' + @metricval

							select @metricthreshold = N'Server is vulnerable if dangerous security principals have been added to SSIS database roles.'
						end
						
						-- Integration Services Roles Permissions Not Acceptable
						else if (@metricid = 91)
						begin
							select @sql = N'declare databasecursor cursor for
											select vdop.objectname, dp.name
											from [dbo].[vwdatabaseobjectpermission] vdop
											inner join databaseprincipal dp 
												on ((vdop.snapshotid = dp.snapshotid) and (vdop.dbid = dp.dbid) and (vdop.grantee = dp.uid))
											where 
											(
												(vdop.snapshotid = ' + convert(nvarchar, @snapshotid) + N') 
												and ((vdop.isgrant = N''Y'') or (vdop.isgrantwith = N''Y''))
												and (vdop.objectname in (' + @severityvalues + N'))
												and dp.type IN (''R'', ''A'')
												and (dp.name not in (''db_dtsadmin'', ''db_dtsltduser'', ''db_dtsoperator'', ''db_ssisadmin'', ''db_ssisltduser'', ''db_ssisoperator''))
											)'
							exec (@sql)			
 							open databasecursor
							fetch next from databasecursor into @strval2, @strval3

							select @intval2 = 0
							while @@fetch_status = 0
							begin
								select @strval = @strval3 + ' on ' + @strval2
								if (@intval2 = 1 or len(@metricval) + len(@strval) > 1010)
								begin
									if @intval2 = 0
										select @metricval = @metricval + N', more...',
												@intval2 = 1
								end
								else
									select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval + N''''

								if (@isadmin = 1)
									insert into policyassessmentdetail ( policyid,
																		 assessmentid,
																		 metricid,
																		 snapshotid,
																		 detailfinding,
																		 databaseid,
																		 objecttype,
																		 objectid,
																		 objectname )
																values ( @policyid,
																		 @assessmentid,
																		 @metricid,
																		 @snapshotid,
																		 N'Permissions on stored procedures found: ''' + @strval + N'''',
																		 null, -- database ID,
																		 N'DB', -- object type
																		 null,
																		 @strval )

								fetch next from databasecursor into @strval2, @strval3
							end

							close databasecursor
							deallocate databasecursor	

							if (len(@metricval) = 0)
								select @sevcode=@sevcodeok,
										@metricval = 'No unacceptable permissions found.'
							else
								select @sevcode=@severity,
										@metricval = N'Permissions on stored procedures found: ' + @metricval

							select @metricthreshold = N'Server is vulnerable if users other than the default SSIS database roles have been granted permissions on an Integration Services stored procedure.'
						end
						--Weak Passwords
						else if (@metricid = 92)
						begin
							select @severityvalues = N'Y'
							declare logincursor cursor for
								select name
									from vwserverprincipal
									where snapshotid = @snapshotid and  type = N'S' and passwordstatus > 0

							open logincursor
							fetch next from logincursor into @strval

							select @intval2 = 0
							while @@fetch_status = 0
							begin
								if (@intval2 = 1 or len(@metricval) + len(@strval) > 1010)
								begin
									if @intval2 = 0
										select @metricval = @metricval + N', more...',
												@intval2 = 1
								end
								else
									select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval + N''''

								if (@isadmin = 1)
									insert into policyassessmentdetail ( policyid,
																		 assessmentid,
																		 metricid,
																		 snapshotid,
																		 detailfinding,
																		 databaseid,
																		 objecttype,
																		 objectid,
																		 objectname )
																values ( @policyid,
																		 @assessmentid,
																		 @metricid,
																		 @snapshotid,
																		 N'Login with weak password found: ''' + @strval + N'''',
																		 null, -- database ID,
																		 N'iLOGN', -- object type
																		 null,
																		 @strval )
																	 
								fetch next from logincursor into @strval
							end

							close logincursor
							deallocate logincursor	

							if (len(@metricval) = 0)
							begin
								if (@weakpasswordenabled = 'N')
								begin
									select @sevcode=@sevcodeok,
											@metricval = @metricval + N'   Weak password detection was disabled during data collection.'
								end
								else
								begin
									select @sevcode=@sevcodeok,
											@metricval = N'No SQL Logins have weak passwords.'
								end
							end
							else
								select @sevcode=@severity,
										@metricval = N'These SQL Logins have weak passwords: ' + @metricval

							select @metricthreshold = N'Server is vulnerable if SQL Logins are found that have a weak password.'
						end
						if ( @metricid = 93 ) 
						begin 
							  truncate table #tempdetails 

							  if ( @isadmin = 1 ) 
								 begin 
									   insert   into #tempdetails
												select
													@policyid,
													@assessmentid,
													@metricid,
													@snapshotid,
													N'Symmetric key ''' + db.name
													+ N''' was found in system database ',
													db.dbid,
													db.type,
													db.objectid,
													db.name
												from
													dbo.databaseobject db
													join dbo.sqldatabase sd
														on db.snapshotid = sd.snapshotid
														   and db.dbid = sd.dbid
												where
													db.type = 'isk'
													and db.snapshotid = @snapshotid
													and sd.databasename in ( 'msdb', 'master', 'model',
																			 'tempdb' ) 

									   if not exists ( select
														*
													   from
														#tempdetails ) 
										  select
											@sevcode = @sevcodeok,
											@metricval = N'None found.' 
									   else 
										  begin 
												select
													@metricval = @metricval + objectname + ', '
												from
													#tempdetails 

												set @metricval = substring(@metricval, 0,
																		   len(@metricval)) 

												insert  into policyassessmentdetail
														select
															policyid,
															assessmentid,
															metricid,
															snapshotid,
															detailfinding,
															databaseid,
															objecttype,
															objectid,
															objectname
														from
															#tempdetails 

												select
													@sevcode = @severity,
													@metricval = N'Next symmetric keys found in system databases : '
													+ @metricval 
										  end 
								 end 

							  select
								@metricthreshold = N'Server is vulnerable if system database have symmetric key ' 
						end 

						else 
						if ( @metricid = 94 ) 
						begin 

							  truncate table #tempdetails 

							  if ( @isadmin = 1 ) 
								 begin 
									   insert   into #tempdetails
												select
													@policyid,
													@assessmentid,
													@metricid,
													@snapshotid,
													N'User defined assembly with unsafe access  '''
													+ db.name + N''' was found in '+sd.databasename,
													db.dbid,
													db.type,
													db.objectid,
													db.name
												from
													dbo.databaseobject db
													join dbo.sqldatabase sd
														on db.snapshotid = sd.snapshotid
														   and db.dbid = sd.dbid
												where
													db.type = 'iasm'
													and db.snapshotid = @snapshotid
													and db.userdefined = 'y'
													and permission_set <> 1 

									   if not exists ( select
														*
													   from
														#tempdetails ) 
										  select
											@sevcode = @sevcodeok,
											@metricval = N'None found.' 
									   else 
										  begin 
												select
													@metricval = @metricval + objectname + ', '
												from
													#tempdetails 

												set @metricval = substring(@metricval, 0,
																		   len(@metricval)) 

												insert  into policyassessmentdetail
														select
															policyid,
															assessmentid,
															metricid,
															snapshotid,
															detailfinding,
															databaseid,
															objecttype,
															objectid,
															objectname
														from
															#tempdetails 

												select
													@sevcode = @severity,
													@metricval = N'Next user defined assemblies with host policy other then safe was found : '
													+ @metricval 
										  end 
								 end 

					
							  select
								@metricthreshold = N'Server is vulnerable if there are user defined assemblies with host policy other than SAFE '
						end 

						else 
						if ( @metricid = 95 ) 
						begin 

							  truncate table #tempdetails 

							  if ( @isadmin = 1 ) 
								 begin

									   declare @currServerAuthMode nvarchar(1)

									   select
										@currServerAuthMode = authenticationmode
									   from
										dbo.registeredserver
									   where
										registeredserverid = @registeredserverid 

									   insert   into #tempdetails
												select
													@policyid,
													@assessmentid,
													@metricid,
													@snapshotid,
													N'Next contained databases was found  '''
													+ db.name,
													db.dbid,
													db.type,
													db.objectid,
													db.name
												from
													dbo.databaseobject db
													join dbo.sqldatabase sd
														on db.snapshotid = sd.snapshotid
														   and db.dbid = sd.dbid
												where
													@currServerAuthMode = 'M'
													and db.type = 'DB'
													and db.snapshotid = @snapshotid
													and isnull(sd.IsContained, 0) = 1 
      
									   if not exists ( select
														*
													   from
														#tempdetails ) 
										  select
											@sevcode = @sevcodeok,
											@metricval = N'None found.' 
									   else 
										  begin 
												set @metricval = 'Sql server authentication mode set to Mixed but contained databases exist on instance'

												insert  into policyassessmentdetail
														select
															policyid,
															assessmentid,
															metricid,
															snapshotid,
															detailfinding,
															databaseid,
															objecttype,
															objectid,
															objectname
														from
															#tempdetails 
					
												select
													@sevcode = @severity


										  end 
								 end 

				
							  select
								@metricthreshold = N'Server is vulnerable if there are contained databases and authentication mode set to Mixed '
						end 


						else 
						if ( @metricid = 96 ) 
						begin 

							  truncate table #tempdetails 

							  if ( @isadmin = 1 ) 
								 begin

							   ;
									   with users ( objectid, name, isOwner, snapshotid, userid, dbid, type )
											  as ( select
													db.objectid,
													db.name,
													0 as IsOwner,
													db.snapshotid,
													dp.grantee,
													db.dbid,
													db.type
												   from
													dbo.databaseobject db
													join dbo.databaseobjectpermission dp
														on db.snapshotid = dp.snapshotid
														   and db.dbid = dp.dbid
														   and db.classid = dp.classid
														   and db.parentobjectid = dp.parentobjectid
														   and db.objectid = dp.objectid
														   and dp.permission = 'EXECUTE'
												   where
													db.type in ( 'P', 'X' )
													and runatstartup is not null
													and runatstartup = 'y'
													and db.snapshotid = @snapshotid
												   union
												   select
													db.objectid,
													db.name,
													1 as IsOwner,
													db.snapshotid,
													db.owner,
													db.dbid,
													db.type
												   from
													dbo.databaseobject db
												   where
													db.type in ( 'P', 'X' )
													and runatstartup is not null
													and runatstartup = 'y'
													and db.snapshotid = @snapshotid
												 ),
											UserRoles ( userId, dbid, userDBname, userDBRole, userLogin, userServerRole, snapshotid )
											  as ( select
													m.uid,
													m.dbid,
													m.name,
													r.name,
													sm.name,
													sr.name,
													m.snapshotid
												   from
													databaseprincipal m
													join databaserolemember as rm
														on m.snapshotid = rm.snapshotid
														   and m.dbid = rm.dbid
														   and m.uid = rm.rolememberuid
													join databaseprincipal as r
														on rm.snapshotid = r.snapshotid
														   and rm.dbid = r.dbid
														   and rm.groupuid = r.uid
													join dbo.serverprincipal sm
														on m.snapshotid = sm.snapshotid
														   and m.usersid = sm.sid
													join dbo.serverrolemember srm
														on sm.snapshotid = srm.snapshotid
														   and sm.principalid = srm.memberprincipalid
													join dbo.serverprincipal sr
														on rm.snapshotid = sr.snapshotid
														   and srm.principalid = sr.principalid
												   where
													sr.sid = 0x03
													and m.snapshotid = @snapshotid
												 )
											insert  into #tempdetails
													select
														@policyid,
														@assessmentid,
														@metricid,
														@snapshotid,
														case u.isOwner
														  when 1
														  then N'Startup stored procedure ' + u.name
															   + 'are owned by user without sysadmin permissions '
														  when 0
														  then N'Startup stored procedure ' + u.name
															   + 'can be executed by user without sysadmin permissions '
														end,
														u.dbid,
														u.type,
														u.objectid,
														u.name
													from
														users u
													where
														u.userid not in ( select
																			userId
																		  from
																			UserRoles us where us.dbid=u.dbid)
													group by
														u.isOwner,
														u.dbid,
														u.type,
														u.objectid,
														u.name   

	

									   if not exists ( select
														*
													   from
														#tempdetails ) 
										  select
											@sevcode = @sevcodeok,
											@metricval = N'None found.' 
									   else 
										  begin 
												set @metricval = 'Next stored procedure can be run or are owned by accounts without sysadmin permissions '

												select
													@metricval = @metricval + objectname + ', '
												from
													#tempdetails

												set @metricval = substring(@metricval, 0,
																		   len(@metricval)) 

												insert  into policyassessmentdetail
														select
															policyid,
															assessmentid,
															metricid,
															snapshotid,
															detailfinding,
															databaseid,
															objecttype,
															objectid,
															objectname
														from
															#tempdetails 
												select
													@sevcode = @severity


										  end 
								 end 

							  select
								@metricthreshold = N'Server is vulnerable if startup stored procedures can be run or are owned by accounts without sysadmin permissions '
						end 


						else 
						if ( @metricid = 97 ) 
						begin 
						
							  truncate table #tempdetails 
							  if object_id('tempdb..#sevrVals') is not null 
								 begin
									   drop table #sevrVals
								 end

							  select
								Value
							  into
								#sevrVals
							  from
								dbo.splitbydelimiter(@severityvalues, ',')

							  if exists ( select
											1
										  from
											dbo.sqljob
										  where
											SubSystem in ( select
															Value
														   from
															#sevrVals )
											and SnapshotId = @snapshotid ) 
								 begin
									   if ( @isadmin = 1 ) 
										  begin
												declare @lname as varchar(200)

												select
													@lname = ( select
																loginname
															   from
																dbo.serverservice
															   where
																snapshotid = @snapshotid
																and servicename = 'SQLSERVERAGENT'
															 )

												insert  into #tempdetails
														select
															@policyid,
															@assessmentid,
															@metricid,
															@snapshotid,
															N'SQL Server Agent account ' + swa.name
															+ ' is a member of ' + smm.name ' group',
															null,
															'Acc',
															null,
															swa.name
														from
															dbo.serveroswindowsaccount swa
															join dbo.serveroswindowsgroupmember ss
																on swa.snapshotid = ss.snapshotid
																   and swa.sid = ss.groupmember
															join dbo.serveroswindowsaccount smm
																on ss.snapshotid = smm.snapshotid
																   and ss.groupsid = smm.sid
														where
															substring(swa.name,
																	  charindex('\', swa.name) + 1,
																	  len(swa.name)) = substring(@lname,
																					  charindex('\',
																					  @lname) + 1,
																					  len(@lname))
															and swa.snapshotid = @snapshotid
															and smm.name like '%\Administrators'


												insert  into #tempdetails
														select
															@policyid,
															@assessmentid,
															@metricid,
															@snapshotid,
															N'SQL Server Job proxy  ' + swa.name
															+ ' is a member of ' + smm.name ' group',
															null,
															'Acc',
															null,
															swa.name
														from
															dbo.sqljobproxy p
															join dbo.serveroswindowsaccount swa
																on p.snapshotid = swa.snapshotid
																   and swa.sid = p.usersid
															join dbo.serveroswindowsgroupmember ss
																on swa.snapshotid = ss.snapshotid
																   and swa.sid = ss.groupmember
															join dbo.serveroswindowsaccount smm
																on ss.snapshotid = smm.snapshotid
																   and ss.groupsid = smm.sid
														where
															p.subsystem in ( select
																				Value
																			 from
																				#sevrVals )
															and p.snapshotid = @snapshotid
															and smm.name like '%\Administrators'
														group by
															swa.name,
															smm.name


												if not exists ( select
																	*
																from
																	#tempdetails ) 
												   select
													@sevcode = @sevcodeok,
													@metricval = N'None found.' 
												else 
												   begin 
														 set @metricval = 'Next accounts are in Administrators role  '

														 select
															@metricval = @metricval + objectname
															+ ', '
														 from
															#tempdetails
														 group by
															objectname

														 set @metricval = substring(@metricval, 0,
																					len(@metricval)) 

														 set @metricval = @metricval
															 + ' and can run sql job steps in '
															 + @severityvalues + ' subsystems'
  
														 insert into policyassessmentdetail
																select
																	policyid,
																	assessmentid,
																	metricid,
																	snapshotid,
																	detailfinding,
																	databaseid,
																	objecttype,
																	objectid,
																	objectname
																from
																	#tempdetails
																group by
																	policyid,
																	assessmentid,
																	metricid,
																	snapshotid,
																	detailfinding,
																	databaseid,
																	objecttype,
																	objectid,
																	objectname 
														 select
															@sevcode = @severity


												   end 
										  end 
								 end

							  else 
								 begin
									   select
										@sevcode = @sevcodeok,
										@metricval = N'None found.'
								 end
							  select
								@metricthreshold = N'Server is vulnerable if sql job steps in  '
								+ @severityvalues
								+ ' subsystems are run by Administrators role members'
						end 

    				    --  DISTRIBUTOR_ADMIN account
						else if (@metricid = 98)
                        begin
                              set @metricval = N'None found.'                       
                              set @metricthreshold = N'Server is vulnerable if DISTRIBUTOR_ADMIN account exists when server is not distributor or DISTRIBUTOR_ADMIN account doesn''t follow password control standards when distributor server has a remote publisher.'
                              set @sevcode = @sevcodeok
                              if exists ( select
                                            SPU.name
                                          from
                                            dbo.serverrolemember as SRM
                                            inner join dbo.serverprincipal as SPU
                                                on SRM.snapshotid = SPU.snapshotid
                                                   and SRM.memberprincipalid = SPU.principalid
                                                   and SPU.type <> 'R'
                                            inner join dbo.serverprincipal as SPR
                                                on SRM.snapshotid = SPR.snapshotid
                                                   and SRM.principalid = SPR.principalid
                                                   and SPR.type = 'R'
                                                   and SPR.name = 'sysadmin'
                                          where
                                            SRM.snapshotid = @snapshotid
                                            and SPU.name = 'distributor_admin' ) 
                                 begin

                                       declare @IsDistributer nchar(1)
                                       declare @IsPublisher nchar(1)
                                       declare @HasRemotePublisher nchar(1)

                                       select
                                        @IsDistributer = SS.isdistributor,
                                        @IsPublisher = SS.ispublisher,
                                        @HasRemotePublisher = SS.hasremotepublisher
                                       from
                                        dbo.serversnapshot as SS
                                       where
                                        SS.snapshotid = @snapshotid
	
                                       if ( @IsDistributer = 'N' ) 
                                          begin
                                                set @sevcode = @severity                                        
                                                set @metricval = N'The DISTRIBUTOR_ADMIN account should be deleted as it is only needed at the distributor.'
                                          end
                                       else 
                                          if (
                                               @IsPublisher = 'N'
                                               and @HasRemotePublisher = 'Y'
                                             ) 
                                             begin
                                                   set @sevcode = @severity                                           
                                                   set @metricval = N'The password of DISTRIBUTOR_ADMIN login must be set according to password control standards using the "sp_changedistributor_password" stored procedure.'
                                             end
                                 end
                        end                        
  
  						-- sysadmin accounts with local administrator role
						else if (@metricid = 99)
                        begin

                              declare @SysadminUsers table
                                      (
                                        username nvarchar(128) not null
                                      );

                              declare @SuppressedAccounts table ( name
                                                              nvarchar(200) )

                              insert    into @SuppressedAccounts
                                        select
                                            Value
                                        from
                                            dbo.splitbydelimiter(@severityvalues,
                                                              ',')

                              insert    into @SysadminUsers
                                        select
                                            SPU.name as username
                                        from
                                            dbo.serverrolemember as SRM
                                            inner join dbo.serverprincipal as SPU
                                                on SRM.snapshotid = SPU.snapshotid
                                                   and SRM.snapshotid = @snapshotid
                                                   and SRM.memberprincipalid = SPU.principalid
                                                   and SPU.type <> 'R'
                                            inner join dbo.serverprincipal as SPR
                                                on SRM.snapshotid = SPR.snapshotid
                                                   and SRM.principalid = SPR.principalid
                                                   and SPR.type = 'R'
                                                   and SPR.name = 'sysadmin'
                                            inner join dbo.serveroswindowsgroupmember
                                            as WGM
                                                on SPU.snapshotid = WGM.snapshotid
                                                   and SPU.sid = WGM.groupmember
                                            inner join dbo.serveroswindowsaccount
                                            as WG
                                                on WGM.snapshotid = WG.snapshotid
                                                   and WGM.groupsid = WG.sid
                                                   and WG.name like '%\Administrators'
                                        where
                                            ( select
                                                count(*)
                                              from
                                                @SuppressedAccounts as sa
                                              where
                                                SPU.name like sa.name
                                            ) = 0

                              declare SysadminUsersCursor cursor
                              for
                                      select
                                        username
                                      from
                                        @SysadminUsers
                              open SysadminUsersCursor
                              select
                                @intval2 = 0
                              fetch next from SysadminUsersCursor into @strval
                              while @@fetch_status = 0 
                                    begin
                                          if (
                                               @intval2 = 1
                                               or len(@metricval)
                                               + len(@strval) > 1010
                                             ) 
                                             begin
                                                   if @intval2 = 0 
                                                      select
                                                        @metricval = @metricval
                                                        + N', more...',
                                                        @intval2 = 1
                                             end
                                          else 
                                             select
                                                @metricval = @metricval
                                                + case when len(@metricval) > 0
                                                       then N', '
                                                       else N''
                                                  end + N'''' + @strval
                                                + N''''

                                          if ( @isadmin = 1 ) 
                                             insert into policyassessmentdetail
                                                    (
                                                      policyid,
                                                      assessmentid,
                                                      metricid,
                                                      snapshotid,
                                                      detailfinding,
                                                      databaseid,
                                                      objecttype,
                                                      objectid,
                                                      objectname 
                                                    
                                                    )
                                             values
                                                    (
                                                      @policyid,
                                                      @assessmentid,
                                                      @metricid,
                                                      @snapshotid,
                                                      N'SQL SYSADMIN accounts that are in the local Administrator role: '''
                                                      + @strval + N'''',
                                                      null, -- database ID,
                                                      N'DB', -- object type
                                                      null, -- object id
                                                      @strval 
                                                    
                                                    )
															         
                                          fetch next from SysadminUsersCursor into @strval
                                    end

                              if ( len(@metricval) = 0 ) 
                                 select
                                    @sevcode = @sevcodeok,
                                    @metricval = N'None found.'
                              else 
                                 select
                                    @sevcode = @severity,
                                    @metricval = N'SQL SYSADMIN accounts that are in the local Administrator role: '
                                    + @metricval

                              select
                                @metricthreshold = N'Server is vulnerable if SQL SYSADMIN accounts that are in the local Administrator role for the physical server other than: '
                                + @severityvalues

                              close SysadminUsersCursor
                              deallocate SysadminUsersCursor
                        end                      
 
 						--information about database roles
						else if (@metricid = 100)
                        begin
                              select
                                @severityvalues = N'Y'
                              declare @RolePermissions table
                                      (
                                        rolename nvarchar(256) null,
                                        rolepermission nvarchar(max) null
                                      );

                              with  RolePermisions
                                      as ( select
                                            rolename,
                                            ( select
                                                T1.rolepermission + ', ' as [text()]
                                              from
                                                dbo.fixedrolepermission as T1
                                              where
                                                T1.rolename = T2.rolename
                                              order by
                                                T1.rolename
                                            for
                                              xml path('')
                                            ) as rolepermissions
                                           from
                                            ( select
                                                rolename
                                              from
                                                dbo.fixedrolepermission
                                              group by
                                                rolename
                                            ) as T2
                                         )
                                   insert   into @RolePermissions
                                            select
                                                rolename,
                                                left(rolepermissions,
                                                     len(rolepermissions) - 1) as rolepermissions
                                            from
                                                RolePermisions;


                              declare @DatabaseRoleUsers table
                                      (
                                        snapshotid int not null,
                                        dbid int not null,
                                        roleid varbinary(85) null,
                                        username nvarchar(128) not null,
                                        usertype nvarchar(20) not null,
                                        groupname nvarchar(200) null,
                                        rolepermissions nvarchar(max)
                                      );

                              insert    into @DatabaseRoleUsers
                                        select
                                            DRM.snapshotid,
                                            DRM.dbid,
                                            DPR.uid as roleid,
                                            DPU.name as username,
                                            case when DPU.type = 'U'
                                                 then 'Windows User'
                                                 when DPU.type = 'G'
                                                 then 'Windows Group'
                                                 when DPU.type = 'S'
                                                 then 'SQL login'
                                            end as usertype,
                                            WG.name as groupname,
                                            RP.rolepermission
                                        from
                                            dbo.databaserolemember as DRM
                                            inner join dbo.databaseprincipal
                                            as DPU
                                                on DRM.snapshotid = DPU.snapshotid
                                                   and DRM.dbid = DPU.dbid
                                                   and DRM.rolememberuid = DPU.uid
                                                   and DPU.type <> 'R'
                                            inner join dbo.databaseprincipal
                                            as DPR
                                                on DRM.snapshotid = DPR.snapshotid
                                                   and DRM.dbid = DPR.dbid
                                                   and DRM.groupuid = DPR.uid
                                                   and DPR.type = 'R'
                                            left join dbo.serveroswindowsgroupmember
                                            as WGM
                                                on DPU.snapshotid = WGM.snapshotid
                                                   and DPU.usersid = WGM.groupmember
                                            left join dbo.serveroswindowsaccount
                                            as WG
                                                on WGM.snapshotid = WG.snapshotid
                                                   and WGM.groupsid = WG.sid
                                            left join @RolePermissions as RP
                                                on DPR.name = RP.rolename
                                        order by
                                            DRM.snapshotid,
                                            DRM.dbid

                              declare @DatabaseRolesInfo table
                                      (
                                        databasename nvarchar(128),
                                        rolename nvarchar(128),
                                        username nvarchar(128),
                                        usertype nvarchar(20),
                                        groupname nvarchar(200),
                                        rolepermissions nvarchar(max)
                                      );

                              insert    into @DatabaseRolesInfo
                                        select
                                            DB.databasename,
                                            DPR.name,
                                            DRU.username,
                                            DRU.usertype,
                                            DRU.groupname,
                                            rolepermissions
                                        from
                                            dbo.databaseprincipal as DPR
                                            inner join dbo.sqldatabase as DB
                                                on DPR.snapshotid = DB.snapshotid
                                                   and DPR.dbid = DB.dbid
                                                   and DPR.type = 'R'
                                            left join @DatabaseRoleUsers as DRU
                                                on DPR.snapshotid = DRU.snapshotid
                                                   and DPR.dbid = DRU.dbid
                                                   and DPR.uid = DRU.roleid
                                        where
                                            DPR.snapshotid = @snapshotid
                                        order by
                                            DB.databasename,
                                            DPR.name

                              declare @databasename nvarchar(128)
                              declare @rolename nvarchar(128)
                              declare @username nvarchar(128)
                              declare @usertype nvarchar(20)
                              declare @groupname nvarchar(200)
                              declare @permissions nvarchar(max)
                              declare @Delimiter nvarchar(10) 
       
                              select @Delimiter  = ', '

                              declare DatabaseRolesInfoCursor cursor
                              for
                                      select
                                        DRI.databasename,
                                        DRI.rolename,
                                        DRI.username,
                                        DRI.usertype,
                                        DRI.groupname,
                                        DRI.rolepermissions
                                      from
                                        @DatabaseRolesInfo as DRI

                              open DatabaseRolesInfoCursor
                              fetch next from DatabaseRolesInfoCursor into @databasename,
                                    @rolename, @username, @usertype,
                                    @groupname, @permissions

                              select
                                @intval2 = 0
                              while @@fetch_status = 0 
                                    begin
                            
                                          set @strval = 'Database: '
                                              + @databasename + '; Role: '
                                              + @rolename

                                          if len(isnull(@username, '')) <> 0 
                                             begin
                                                   set @strval = @strval
                                                       + '; User: '
                                                       + @username	                              
                                             end                              

                                          if len(isnull(@usertype, '')) <> 0 
                                             begin
                                                   set @strval = @strval
                                                       + '; Type: '
                                                       + @usertype	                              
                                             end  

                                          if len(isnull(@groupname, '')) <> 0 
                                             begin
                                                   set @strval = @strval
                                                       + '; Windows Group: '
                                                       + @groupname	                              
                                             end  

                                          if len(isnull(@permissions, '')) <> 0 
                                             begin
                                                   set @strval = @strval
                                                       + '; Permissions: '
                                                       + @permissions	                              
                                             end    
									                          
                                          if (
                                               @intval2 = 1
                                               or len(@metricval)
                                               + len(@strval) > 1010
                                             ) 
                                             begin
                                                   if @intval2 = 0 
                                                      select
                                                        @metricval = @metricval
                                                        + N', more...',
                                                        @intval2 = 1
                                             end
                                          else 
                                             select
                                                @metricval = @metricval
                                                + case when len(@metricval) > 0
                                                       then N', '
                                                       else N''
                                                  end + N'''' + @strval
                                                + N''''

                                          if ( @isadmin = 1 ) 
                                             insert into policyassessmentdetail
                                                    (
                                                      policyid,
                                                      assessmentid,
                                                      metricid,
                                                      snapshotid,
                                                      detailfinding,
                                                      databaseid,
                                                      objecttype,
                                                      objectid,
                                                      objectname 
                                                    )
                                             values
                                                    (
                                                      @policyid,
                                                      @assessmentid,
                                                      @metricid,
                                                      @snapshotid,
                                                      N'Database Role Info found: '''
                                                      + @strval + N'''',
                                                      null, -- database ID,
                                                      N'DB', -- object type
                                                      null,
                                                      N'Database Role Metric details' 
                                                    )
																	 
                                          fetch next from
                                                DatabaseRolesInfoCursor into @databasename,
                                                @rolename, @username,
                                                @usertype, @groupname,
                                                @permissions
                                    end

                              close DatabaseRolesInfoCursor
                              deallocate DatabaseRolesInfoCursor	
                              select
                                @sevcode = @severity,
                                @metricval = N'The following database roles were found: '
                                + @metricval
                              select
                                @metricthreshold = N'Server is vulnerable if database role info hasn''t been checked.'
                        end                  
--						--information about server roles
						else if (@metricid = 101)
                        begin
                              select
                                @severityvalues = N'Y'
                              declare @ServerRoleUsers table
                                      (
                                        snapshotid int not null,
                                        roleprincipalid varbinary(85) null,
                                        username nvarchar(128) not null,
                                        usertype nvarchar(20) not null,
                                        groupname nvarchar(200) null,
                                        disabled nvarchar(3)
                                      );

                              insert    into @ServerRoleUsers
                                        select 
						--*
                                            SRM.snapshotid,
                                            SPR.principalid,
                                            SPU.name as username,
                                            case when SPU.type = 'U'
                                                 then 'Windows User'
                                                 when SPU.type = 'G'
                                                 then 'Windows Group'
                                                 when SPU.type = 'S'
                                                 then 'SQL login'
                                            end as usertype,
                                            WG.name as groupname,
                                            case when SPU.disabled = 'Y'
                                                 then 'Yes'
                                                 else 'No'
                                            end as disabled
                                        from
                                            dbo.serverrolemember as SRM
                                            inner join dbo.serverprincipal as SPU
                                                on SRM.snapshotid = SPU.snapshotid
                                                   and SRM.memberprincipalid = SPU.principalid
                                                   and SPU.type <> 'R'
                                            inner join dbo.serverprincipal as SPR
                                                on SRM.snapshotid = SPR.snapshotid
                                                   and SRM.principalid = SPR.principalid
                                                   and SPR.type = 'R'
                                            left join dbo.serveroswindowsgroupmember
                                            as WGM
                                                on SPU.snapshotid = WGM.snapshotid
                                                   and SPU.sid = WGM.groupmember
                                            left join dbo.serveroswindowsaccount
                                            as WG
                                                on WGM.snapshotid = WG.snapshotid
                                                   and WGM.groupsid = WG.sid
                                        where
                                            SRM.snapshotid = @snapshotid
                                        order by
                                            SRM.snapshotid

                              declare @ServerRolesInfo table
                                      (
                                        instancename nvarchar(400),
                                        rolename nvarchar(128),
                                        username nvarchar(128),
                                        usertype nvarchar(20),
                                        groupname nvarchar(200),
                                        disabled nvarchar(3)
                                      );

                              insert    into @ServerRolesInfo
                                        select
                                            case when len(isnull(ST.instancename,
                                                              '')) <> 0
                                                 then ST.instancename
                                                 when len(isnull(ST.servername,
                                                              '')) <> 0
                                                 then ST.servername
                                                 else ST.connectionname
                                            end as instancename,
                                            SP.name as rolename,
                                            SRU.username,
                                            SRU.usertype,
                                            SRU.groupname,
                                            SRU.disabled
                                        from
                                            dbo.serverprincipal as SP
                                            inner join dbo.serversnapshot as ST
                                                on SP.snapshotid = ST.snapshotid
                                                   and SP.type = 'R'
                                            left join @ServerRoleUsers as SRU
                                                on SP.snapshotid = SRU.snapshotid
                                                   and SP.principalid = SRU.roleprincipalid
                                        where
                                            SP.snapshotid = @snapshotid
                                        order by
                                            instancename,
                                            rolename

                              declare @Instancename nvarchar(400)
                              declare @ServerRolename nvarchar(128)
                              declare @ServerUsername nvarchar(128)
                              declare @ServerUserType nvarchar(20)
                              declare @WindowsGroupname nvarchar(200)
                              declare @ServerUserDisabled nvarchar(3)

                              declare ServerRolesInfoCursor cursor
                              for
                                      select
                                        SRI.instancename,
                                        SRI.rolename,
                                        SRI.username,
                                        SRI.usertype,
                                        SRI.groupname,
                                        SRI.disabled
                                      from
                                        @ServerRolesInfo as SRI

                              open ServerRolesInfoCursor
                              fetch next from ServerRolesInfoCursor into @Instancename,
                                    @ServerRolename, @ServerUsername,
                                    @ServerUserType, @WindowsGroupname,
                                    @ServerUserDisabled

                              select
                                @intval2 = 0
                              while @@fetch_status = 0 
                                    begin
                            
                                          set @strval = 'SQL Instance: '
                                              + @Instancename
                                              + '; Server Role: '
                                              + @ServerRolename

                                          if len(isnull(@ServerUsername, '')) <> 0 
                                             begin
                                                   set @strval = @strval
                                                       + '; User: '
                                                       + @ServerUsername	                              
                                             end                              

                                          if len(isnull(@ServerUserType, '')) <> 0 
                                             begin
                                                   set @strval = @strval
                                                       + '; Type: '
                                                       + @ServerUserType	                              
                                             end  

                                          if len(isnull(@WindowsGroupname, '')) <> 0 
                                             begin
                                                   set @strval = @strval
                                                       + '; Windows Group: '
                                                       + @WindowsGroupname	                              
                                             end  

                                          if len(isnull(@ServerUserDisabled,
                                                        '')) <> 0 
                                             begin
                                                   set @strval = @strval
                                                       + '; Disabled: '
                                                       + @ServerUserDisabled	                              
                                             end            
								                
                                          if (
                                               @intval2 = 1
                                               or len(@metricval)
                                               + len(@strval) > 1010
                                             ) 
                                             begin
                                                   if @intval2 = 0 
                                                      select
                                                        @metricval = @metricval
                                                        + N', more...',
                                                        @intval2 = 1
                                             end
                                          else 
                                             select
                                                @metricval = @metricval
                                                + case when len(@metricval) > 0
                                                       then N', '
                                                       else N''
                                                  end + N'''' + @strval
                                                + N''''

                                          if ( @isadmin = 1 ) 
                                             insert into policyassessmentdetail
                                                    (
                                                      policyid,
                                                      assessmentid,
                                                      metricid,
                                                      snapshotid,
                                                      detailfinding,
                                                      databaseid,
                                                      objecttype,
                                                      objectid,
                                                      objectname 
                                                    )
                                             values
                                                    (
                                                      @policyid,
                                                      @assessmentid,
                                                      @metricid,
                                                      @snapshotid,
                                                      N'Server Role Info found: '''
                                                      + @strval + N'''',
                                                      null, -- database ID,
                                                      N'DB', -- object type
                                                      null,
                                                      N'Server Role Metric details' 
                                                    )
																	 
                                          fetch next from
                                                ServerRolesInfoCursor into @Instancename,
                                                @ServerRolename,
                                                @ServerUsername,
                                                @ServerUserType,
                                                @WindowsGroupname,
                                                @ServerUserDisabled
                                    end

                              close ServerRolesInfoCursor
                              deallocate ServerRolesInfoCursor	
                              select
                                @sevcode = @severity,
                                @metricval = N'The following server roles were found: '
                                + @metricval
                              select
                                @metricthreshold = N'Server is vulnerable if server roles haven''t been checked.'
                        end                         
			else
			if ( @metricid = 102 ) 
		       begin 
						
                              truncate table #tempdetails 						 

                              if exists ( select
                                            1
                                          from
                                            dbo.splitbydelimiter(@severityvalues,
                                                              ',') ) 
                                 begin
                                       if ( @isadmin = 1 ) 
                                          begin
										  											
                                                insert  into #tempdetails
                                                        select
                                                            @policyid,
                                                            @assessmentid,
                                                            @metricid,
                                                            @snapshotid,
                                                            N'Database   '
                                                            + sdb.databasename
                                                            + 'has next unacceptable  '
                                                            + sdb.owner,
                                                            sdb.dbid,
                                                            'DB',
                                                            sdb.dbid,
                                                            sdb.databasename
                                                        from
                                                            dbo.sqldatabase sdb
                                                            join dbo.splitbydelimiter(@severityvalues,
                                                              ',') sp
                                                              on sdb.owner like '%'
                                                              + sp.Value
                                                        where
                                                            sdb.snapshotid = @snapshotid
															
													


                                                if not exists ( select
                                                              *
                                                              from
                                                              #tempdetails ) 
                                                   select
                                                    @sevcode = @sevcodeok,
                                                    @metricval = N'None found.' 
                                                else 
                                                   begin 
                                                         set @metricval = 'Next databases are owned by unacceptable accounts :'

                                                         select
                                                            @metricval = @metricval
                                                            + objectname
                                                            + ', '
                                                         from
                                                            #tempdetails
                                                         group by
                                                            objectname

                                                         set @metricval = substring(@metricval,
                                                              0,
                                                              len(@metricval)) 

													
  
                                                         insert
                                                              into policyassessmentdetail
                                                              select
                                                              policyid,
                                                              assessmentid,
                                                              metricid,
                                                              snapshotid,
                                                              detailfinding,
                                                              databaseid,
                                                              objecttype,
                                                              objectid,
                                                              objectname
                                                              from
                                                              #tempdetails
                                                              group by
                                                              policyid,
                                                              assessmentid,
                                                              metricid,
                                                              snapshotid,
                                                              detailfinding,
                                                              databaseid,
                                                              objecttype,
                                                              objectid,
                                                              objectname 
                                                         select
                                                            @sevcode = @severity


                                                   end 
                                          end 
                                 end

                              else 
                                 begin
                                       select
                                        @sevcode = @sevcodeok,
                                        @metricval = N'Blacklist of database ownership was not provided'
                                 end
					
                              select
                                @metricthreshold = N'Server is vulnerable if  databases are owned by one of the next accounts:  '
                                + @severityvalues
                        end 

						else if ( @metricid = 103 ) 
		       begin 
						
                              truncate table #tempdetails 						 

                                       if ( @isadmin = 1 ) 
                                          begin
										  											
                                                insert  into #tempdetails
                                                        select
                                                            @policyid,
                                                            @assessmentid,
                                                            @metricid,
                                                            @snapshotid,
                                                            N'Public role has access to  '
                                                            + dd.name
                                                            + ' object ',
                                                            dd.dbid,
                                                            dd.type,
                                                            dd.objectid,
                                                            dd.name
                                                        from
                                                            dbo.databaseobject dd
                                                            join dbo.sqldatabase sdb
                                                              on dd.snapshotid = sdb.snapshotid
                                                              and dd.dbid = sdb.dbid
                                                            join dbo.databaseschema ds
                                                              on dd.snapshotid = ds.snapshotid
                                                              and dd.dbid = ds.dbid
                                                              and dd.schemaid = ds.schemaid
                                                            join dbo.databaseobjectpermission dp
                                                              on dd.snapshotid = dp.snapshotid
                                                              and dd.dbid = dp.dbid
                                                              and dd.classid = dp.classid
                                                              and dd.parentobjectid = dp.parentobjectid
                                                              and dd.objectid = dp.objectid
                                                        where
                                                            dp.grantee = 0
                                                            and ds.schemaname <> 'sys'
                                                            and sdb.databasename not in (
                                                            'master', 'msdb',
                                                            'tempdb' )
                                                            and dp.isdeny = 'N'
                                                            and dd.snapshotid = @snapshotid
                                                        group by
                                                            dd.name,
															  dd.dbid,
                                                            dd.type,
                                                           dd.objectid,
                                                            sdb.databasename                            
															
													


                                                if not exists ( select
                                                              *
                                                              from
                                                              #tempdetails ) 
                                                   select
                                                    @sevcode = @sevcodeok,
                                                    @metricval = N'None found.' 
                                                else 
                                                   begin 
                                                         set @metricval = 'Public roles have access to next objects :'

                                                         select
                                                            @metricval = @metricval
                                                            + objectname
                                                            + ', '
                                                         from
                                                            #tempdetails
                                                         group by
                                                            objectname

                                                         set @metricval = substring(@metricval,
                                                              0,
                                                              len(@metricval)) 

													
  
                                                         insert
                                                              into policyassessmentdetail
                                                              select
                                                              policyid,
                                                              assessmentid,
                                                              metricid,
                                                              snapshotid,
                                                              detailfinding,
                                                              databaseid,
                                                              objecttype,
                                                              objectid,
                                                              objectname
                                                              from
                                                              #tempdetails
                                                              group by
                                                              policyid,
                                                              assessmentid,
                                                              metricid,
                                                              snapshotid,
                                                              detailfinding,
                                                              databaseid,
                                                              objecttype,
                                                              objectid,
                                                              objectname 
                                                         select
                                                            @sevcode = @severity


                                                   end 
                                          end 
                             
					
                              select
                                @metricthreshold = N'Server is vulnerable if public roles has access to user defined objects'
                                
                        end 

						else if ( @metricid = 104 ) 
						begin

							  if not exists ( select
												*
											  from
												serversnapshot
											  where
												snapshotid = @snapshotid
												and isclrenabled = N'Y' ) 
								 select
									@sevcode = @sevcodeok,
									@metricval = N'CLR is turned off.' 
							  else 
								 begin 
									   set @metricval = 'CLR is turned on.'													
  
									   insert   into policyassessmentdetail
												(
												  policyid,
												  assessmentid,
												  metricid,
												  snapshotid,
												  detailfinding,
												  databaseid,
												  objecttype,
												  objectid,
												  objectname 
												)
									   values
												(
												  @policyid,
												  @assessmentid,
												  @metricid,
												  @snapshotid,
												  @metricval,
												  null, -- database ID,
												  N'iSRV', -- object type
												  null,
												  @strval 
												)
									   set @sevcode = @severity


								 end 
						end                       
 
 
						else if ( @metricid = 105 ) 
						begin

							  if exists ( select
												*
											  from
												serversnapshot
											  where
												snapshotid = @snapshotid
												and isdefaulttraceenabled = N'Y' ) 
								 select
									@sevcode = @sevcodeok,
									@metricval = N'Default Trace is enabled.' 
							  else 
								 begin 
									   set @metricval = N'Default Trace is disabled.'													
  
									   insert   into policyassessmentdetail
												(
												  policyid,
												  assessmentid,
												  metricid,
												  snapshotid,
												  detailfinding,
												  databaseid,
												  objecttype,
												  objectid,
												  objectname 
												)
									   values
												(
												  @policyid,
												  @assessmentid,
												  @metricid,
												  @snapshotid,
												  @metricval,
												  null, -- database ID,
												  N'iSRV', -- object type
												  null,
												  @strval 
												)
									   set @sevcode = @severity


								 end 
						end   

						else if ( @metricid = 106 ) 
						begin
								declare @numerrorlogs smallint
                                select	@numerrorlogs = isnull(numerrorlogs, 0) from serversnapshot where snapshotid = @snapshotid
							  if ( @numerrorlogs > 11 ) 
								 select
									@sevcode = @sevcodeok,
									@metricval = N'Maximum number of error log files is ' + CONVERT(varchar,@numerrorlogs) + '.' 
							  else 
								 begin 
									   set @metricval = N'Maximum number of error log files is less than recommended (12+). Current value is ' + CONVERT(varchar, @numerrorlogs) + '.'											
  
									   insert   into policyassessmentdetail
												(
												  policyid,
												  assessmentid,
												  metricid,
												  snapshotid,
												  detailfinding,
												  databaseid,
												  objecttype,
												  objectid,
												  objectname 
												)
									   values
												(
												  @policyid,
												  @assessmentid,
												  @metricid,
												  @snapshotid,
												  @metricval,
												  null, -- database ID,
												  N'iSRV', -- object type
												  null,
												  @strval 
												)
									   set @sevcode = @severity


								 end 
						end 

						else if ( @metricid = 107 ) 
						begin
							truncate table #tempdetails 						                     

							if object_id('tempdb..#sevrVal') is not null 
							begin
								drop table #sevrVal
							end
							
							select Value
							into #sevrVal
							from dbo.splitbydelimiter(@severityvalues, ',')

							declare @orphanedUsersCount as int
							declare @orphanedUsersnames as varchar(max)                       

							;WITH OrphanedUsers (dbid, name, uid, type)
							as
							(
								SELECT dbid, name, uid, type
								FROM dbo.databaseprincipal
								where usersid not in(SELECT sid FROM dbo.serverprincipal where sid is not null and snapshotid = @snapshotid)
								and type = N'S' 
								and usersid <> 0x00 
								and usersid is not null 
								and IsContainedUser = 0
								and snapshotid = @snapshotid
								and not exists (select * from #sevrVal where Value = name)
							)
							insert  into #tempdetails
							select
								@policyid,
								@assessmentid,
								@metricid,
								@snapshotid,
								N'Orphaned user found - ' + name,
								dbid,
								type,
								uid,
								name
							from OrphanedUsers

							;with OrphanedUsersForDb(orphanedUsersCountForDb, orphanedUsersnamesForDb, dbname, dbid)
							as(
								select count(*) as orphanedUsersCount, 
										STUFF((SELECT N', ' + objectname  FROM #tempdetails c2 where c.databaseid = c2.databaseid FOR XML PATH(N'')), 1, 2, N'') as orphanedUsersnamesForDb,
										sdb.databasename,
										c.databaseid
								FROM #tempdetails c
								join dbo.sqldatabase sdb on c.databaseid = sdb.dbid and c.snapshotid = sdb.snapshotid
								group by c.databaseid, sdb.databasename
							)
							select @orphanedUsersCount =  sum(orphanedUsersCountForDb),
									@orphanedUsersnames = STUFF((SELECT N'; ' + orphanedUsersnamesForDb + N' in ' + dbname FROM OrphanedUsersForDb FOR XML PATH(N'')), 1, 2, N'')  
									from OrphanedUsersForDb  

								  if ( @orphanedUsersCount is null or @orphanedUsersCount = 0 ) 
									 select
										@sevcode = @sevcodeok,
										@metricval = N'There is no Orphaned users.' 
								  else 
									 begin 
										set @metricval = cast(@orphanedUsersCount as nvarchar) + N' orphaned users found: ' + @orphanedUsersnames + '.'	
										insert  into policyassessmentdetail
										select
											policyid,
											assessmentid,
											metricid,
											snapshotid,
											detailfinding,
											databaseid,
											objecttype,
											objectid,
											objectname
										from
												#tempdetails 
												
										set @sevcode = @severity
									 end 
						end 

						else if ( @metricid = 108 ) 
						begin
							  if not exists(select * from serversnapshot where snapshotid = @snapshotid and oleautomationproceduresenabled = N'Y')
								 select
									@sevcode = @sevcodeok,
									@metricval = N'Ole automation procedures are disabled.' 
							  else 
								 begin 
									   set @metricval = N'Ole automation procedures are enabled.'											
  
									   insert   into policyassessmentdetail
												(
												  policyid,
												  assessmentid,
												  metricid,
												  snapshotid,
												  detailfinding,
												  databaseid,
												  objecttype,
												  objectid,
												  objectname 
												)
									   values
												(
												  @policyid,
												  @assessmentid,
												  @metricid,
												  @snapshotid,
												  @metricval,
												  null, -- database ID,
												  N'DB', -- object type
												  null,
												  @strval 
												)
									   set @sevcode = @severity
								 end 
						end 

						else if ( @metricid = 109 ) 
						begin

							  if exists ( select
												1
											  from
												serversnapshot
											  where
												snapshotid = @snapshotid
												and iscommoncriteriacomplianceenabled = N'Y' ) 
								 select
									@sevcode = @sevcodeok,
									@metricval = N'Common criteria compliance is enabled.' 
							  else 
								 begin 
									   set @metricval = 'Common criteria compliance is disabled.'													
  
									   insert   into policyassessmentdetail
												(
												  policyid,
												  assessmentid,
												  metricid,
												  snapshotid,
												  detailfinding,
												  databaseid,
												  objecttype,
												  objectid,
												  objectname 
												)
									   values
												(
												  @policyid,
												  @assessmentid,
												  @metricid,
												  @snapshotid,
												  @metricval,
												  null, -- database ID,
												  N'DB', -- object type
												  null,
												  @strval 
												)
									   set @sevcode = @severity


								 end 
						end  

				-- Integration Services Users Permissions Not Acceptable
						else if (@metricid = 110)
						begin
							select @sql = N'declare databasecursor cursor for
											select vdop.objectname, dp.name
											from [dbo].[vwdatabaseobjectpermission] vdop
											inner join databaseprincipal dp 
												on ((vdop.snapshotid = dp.snapshotid) and (vdop.dbid = dp.dbid) and (vdop.grantee = dp.uid))
											where 
											(
												(vdop.snapshotid = ' + convert(nvarchar, @snapshotid) + N') 
												and ((vdop.isgrant = N''Y'') or (vdop.isgrantwith = N''Y''))
												and (vdop.objectname in (' + @severityvalues + N'))
												and dp.type IN (''S'', ''U'', ''G'')
											)'
							exec (@sql)			
 							open databasecursor
							fetch next from databasecursor into @strval2, @strval3

							select @intval2 = 0
							while @@fetch_status = 0
							begin
								select @strval = @strval3 + ' on ' + @strval2
								if (@intval2 = 1 or len(@metricval) + len(@strval) > 1010)
								begin
									if @intval2 = 0
										select @metricval = @metricval + N', more...',
												@intval2 = 1
								end
								else
									select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval + N''''

								if (@isadmin = 1)
									insert into policyassessmentdetail ( policyid,
																		 assessmentid,
																		 metricid,
																		 snapshotid,
																		 detailfinding,
																		 databaseid,
																		 objecttype,
																		 objectid,
																		 objectname )
																values ( @policyid,
																		 @assessmentid,
																		 @metricid,
																		 @snapshotid,
																		 N'Permissions on stored procedures found: ''' + @strval + N'''',
																		 null, -- database ID,
																		 N'DB', -- object type
																		 null,
																		 @strval )

								fetch next from databasecursor into @strval2, @strval3
								 end 

							close databasecursor
							deallocate databasecursor	

							if (len(@metricval) = 0)
								select @sevcode=@sevcodeok,
										@metricval = 'No unacceptable permissions found.'
							else
								select @sevcode=@severity,
										@metricval = N'Permissions on stored procedures found: ' + @metricval

							select @metricthreshold = N'Server is vulnerable if users other than the default SSIS database roles have been granted permissions on an Integration Services stored procedure.'
						end  

						--**************************** code added to handle user defined security checks, but never used (first added in version 2.5)
						-- User implemented
						else if (@metricid >= 1000)
						begin
							if exists (select * from dbo.sysobjects where name = @severityvalues and type = N'P')
							begin
								select @strval = @severityvalues
								-- this currently will accept no parameters and cannot return any list of exceptions
								-- it should read the sysobjects table to find predefined output parameters and process them appropriately
								--
								begin try
									exec @intval=@strval

									if (@intval = 0)
										select @sevcode=@sevcodeok
									else
										select @sevcode=@severity

									select @metricval = dbo.getyesnotext(N'U')
								end try
								begin catch
									select @sevcode=@severity
									select @metricval = N'Error ' + cast(ERROR_NUMBER() as nvarchar) + N' encountered while executing custom stored procedure ' + @severityvalues + N': ' + Error_Message()
									select @metricthreshold = N'The security check could not be verified.'
								end catch

								select @metricthreshold = N'Server is vulnerable if the stored procedure ' + @severityvalues + ' returns true.'
							end
							else
								select @metricval = dbo.getyesnotext(N'U')
								select @metricthreshold = N'The stored procedure ' + @severityvalues + ' was not found in the SQLsecure database.'
						end
					end try
					begin catch
						select @sevcode=@severity
						select @metricval = N'Error ' + cast(ERROR_NUMBER() as nvarchar) + N' encountered on line ' + cast(Error_Line() as nvarchar) + N' while processing security check: ' + Error_Message()
						select @metricthreshold = N'The security check could not be verified.'
					end catch

					--****************************** done processing the security check. Now write out the valid metric data ***********************************
					if (@err = 0 and (@alertsonly = 0 or @sevcode > 0))
					begin
						-- handle unexpected null values more gracefully
						select @metricval = isnull(@metricval, 'The selected snapshot does not contain a value. Check the snapshot status and the activity log for possible causes.')
						if (exists (select * from @returnservertbl where registeredserverid = @registeredserverid))
							insert into @outtbl (
										snapshotid,
										registeredserverid,
										connectionname,
										collectiontime,
										metricid,
										metricname,
										metrictype,
										metricseveritycode,
										metricseverity,
										metricseverityvalues,
										metricdescription,
										metricreportkey,
										metricreporttext,
										severitycode,
										severity,
										currentvalue,
										thresholdvalue)
								values (
										@snapshotid,
										@registeredserverid,
										@connection,
										@snapshottime,
										@metricid,
										@metricname,
										@metrictype,
										@severity,
										dbo.getpolicyseverityname(@severity),
										@configuredvalues,
										@metricdescription,
										@metricreportkey,
										@metricreporttext,
										@sevcode,
										dbo.getpolicyseverityname(@sevcode),
										@metricval,
										@metricthreshold
										)
						if (@isadmin = 1)
							insert into policyassessment (
											policyid,
											assessmentid,
											snapshotid,
											registeredserverid,
											connectionname,
											collectiontime,
											metricid,
											metricname,
											metrictype,
											metricseveritycode,
											metricseverity,
											metricseverityvalues,
											metricdescription,
											metricreportkey,
											metricreporttext,
											severitycode,
											severity,
											currentvalue,
											thresholdvalue)
									values (
											@policyid,
											@assessmentid,
											@snapshotid,
											@registeredserverid,
											@connection,
											@snapshottime,
											@metricid,
											@metricname,
											@metrictype,
											@severity,
											dbo.getpolicyseverityname(@severity),
											@configuredvalues,
											@metricdescription,
											@metricreportkey,
											@metricreporttext,
											@sevcode,
											dbo.getpolicyseverityname(@sevcode),
											@metricval, 
											@metricthreshold
											)
					end

					if (@debug = 1)
						print 'metric execution took ' + convert (nvarchar, datediff(second, @runtime, getdate())) + ' seconds'

					fetch next from metriccursor into @metricid, @metricname, @metrictype,
														@metricdescription, @metricreportkey, @metricreporttext,
														@severity, @severityvalues
				end

				-- delete from the server table after processing
				delete #servertbl where registeredserverid = @registeredserverid

				if (@debug = 1)
					print 'server execution took ' + convert (nvarchar, datediff(second, @serverruntime, getdate())) + ' seconds'

				fetch next from snapcursor into @snapshotid, @registeredserverid, @connection, 
												@snapshottime, @status, @baseline, @collectorversion,
												@version, @os, @authentication,
												@loginauditmode, @c2audittrace, @crossdb,
												@proxy, @remotedac, @remoteaccess,
												@startupprocs, @sqlmail, @databasemail,
												@ole, @webassistant, @xp_cmdshell,
												@agentmailprofile, @hide, @agentsysadmin,
												@dc, @replication, @sapassword,
												@systemtables, @systemdrive, @adhocqueries,
												@weakpasswordenabled
			end

			-- drop saved temp table after all snapshot processing is done
			drop table #sysadminstbl

			close snapcursor
			deallocate snapcursor

--			-- now process the non-server related metrics
--			fetch first from metriccursor into @metricid, @metricname, @metrictype,
--												@metricdescription, @metricreportkey, @metricreporttext,
--												@severity, @severityvalues
--			while (@@fetch_status = 0)
--			begin
--				-- This sets the metric so it will not be displayed if no value is found
--				--     each metric should handle this situation appropriately
--				select @err=0, @sevcode=-1, @metricval=N'', @metricthreshold=N'', @configuredvalues=@severityvalues
--				-- clean up old values
--				select @intval=0, @intval2=0, @strval=N'', @strval2=N'', @strval3=N'', @sql=N''
--				delete from @tblval
--
--	--************************************************* version 2.5 security checks
--				-- Security Check Settings are different (Assessment Comparison)
--				if (@metricid = 64)
--				begin
--					select @severityvalues = N'Y'
--					if (N'Y' <> @severityvalues)
--						select @sevcode=@sevcodeok
--					else
--						select @sevcode=@severity
--
--					select @metricval = dbo.getyesnotext(@severityvalues)
--					select @metricthreshold = N'Servers are vulnerable if security check settings are different from the most recent ' + dbo.getassessmentstatename(@severityvalues) + N' assessment.'
--				end
--
--				-- Assessment Findings are different (Assessment Comparison)
--				else if (@metricid = 65)
--				begin
--					select @severityvalues = N'Y'
--					if (N'Y' <> @severityvalues)
--						select @sevcode=@sevcodeok
--					else
--						select @sevcode=@severity
--
--					select @metricval = dbo.getyesnotext(@severityvalues)
--					select @metricthreshold = N'Servers are vulnerable if assessment results are different from the most recent ' + dbo.getassessmentstatename(@severityvalues) + N' assessment.'
--				end
--
--				-- Policy Servers are different (Assessment Comparison)
--				else if (@metricid = 66)
--				begin
--					-- get the assessmentid of the selected policy for comparison
--					select @strval = N'declare assessmentcursor cursor for 
--											select top 1 assessmentid 
--												from assessment 
--												where policyid = ' + convert(nvarchar,@policyid) + N'
--													and assessmentstate in (' + @severityvalues + N') 
--												order by assessmentid desc'
--					exec (@strval)
--
--					open assessmentcursor
--					fetch next from assessmentcursor into @intval
--					close assessmentcursor
--					deallocate assessmentcursor	
--
--					-- get the list of servers for the selected policy
--					if (@intval is not null and @intval > 0)
--					begin
--						declare policyservercursor cursor for
--							select a1.registeredserverid, b1.connectionname + N' added'
--								from (policymember a1 left join registeredserver b1 on a1.registeredserverid = b1.registeredserverid)
--										left join (policymember a2 left join registeredserver b2 on a2.registeredserverid = b2.registeredserverid)
--											on a2.policyid = @policyid and a2.assessmentid = @intval and a1.registeredserverid = a2.registeredserverid
--								where a1.policyid = @policyid and a1.assessmentid = @assessmentid and a2.registeredserverid is null
--							union
--							select a1.registeredserverid, b1.connectionname + N' missing'
--								from (policymember a1 left join registeredserver b1 on a1.registeredserverid = b1.registeredserverid)
--										left join (policymember a2 left join registeredserver b2 on a2.registeredserverid = b2.registeredserverid)
--											on a2.policyid = @policyid and a2.assessmentid = @assessmentid and a1.registeredserverid = a2.registeredserverid
--								where a1.policyid = @policyid and a1.assessmentid = @intval and a2.registeredserverid is null
--
--						open policyservercursor
--						fetch next from policyservercursor into @intval, @strval
--						select @intval2 = 0
--						while @@fetch_status = 0
--						begin
--							if (@intval2 = 1 or len(@metricval) + len(@strval) > 1010)
--							begin
--								if @intval2 = 0
--									select @metricval = @metricval + N', more...',
--											@intval2 = 1
--							end
--							else
--								select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval + N''''
--
--							if (@isadmin = 1)
--							insert into policyassessmentdetail ( policyid,
--																 assessmentid,
--																 metricid,
--																 snapshotid,
--																 detailfinding,
--																 databaseid,
--																 objecttype,
--																 objectid,
--																 objectname )
--														values ( @policyid,
--																 @assessmentid,
--																 @metricid,
--																 @snapshotid,
--																 @strval,
--																 null, -- database ID,
--																 N'SVR', -- object type
--																 @intval,
--																 @strval )
--
--							fetch next from policyservercursor into @intval, @strval
--						end
--
--						close policyservercursor
--						deallocate policyservercursor	
--
--						if (len(@metricval) = 0)
--							select @sevcode=@sevcodeok,
--									@metricval = 'No servers are different on the compared assessments.'
--						else
--							select @sevcode=@severity,
--									@metricval = N'These servers are different: ' + @metricval
--					end
--					else
--					begin
--						select @sevcode=@severity,
--									@metricval = N'The selected assessment was not found and all servers are different'
--					end
--
--					select @metricthreshold = N'Servers are vulnerable if the policy server list is different from the most recent ' + dbo.getassessmentstatename(substring(@severityvalues,2,1)) + N' assessment.'
--				end
--
--				-- write out the valid metric data
--				if (@err = 0 and (@alertsonly = 0 or @sevcode > 0))
--				begin
--					-- handle unexpected null values more gracefully
--					if (exists (select * from @returnservertbl where registeredserverid = @registeredserverid))
--						insert into @outtbl (
--									snapshotid,
--									registeredserverid,
--									connectionname,
--									collectiontime,
--									metricid,
--									metricname,
--									metrictype,
--									metricseveritycode,
--									metricseverity,
--									metricseverityvalues,
--									metricdescription,
--									metricreportkey,
--									metricreporttext,
--									severitycode,
--									severity,
--									currentvalue,
--									thresholdvalue)
--							values (
--									null,
--									null,
--									N'',
--									null,
--									@metricid,
--									@metricname,
--									@metrictype,
--									@severity,
--									dbo.getpolicyseverityname(@severity),
--									@configuredvalues,
--									@metricdescription,
--									@metricreportkey,
--									@metricreporttext,
--									@sevcode,
--									dbo.getpolicyseverityname(@sevcode),
--									@metricval,
--									@metricthreshold
--									)
--					if (@isadmin = 1)
--					insert into policyassessment (
--									policyid,
--									assessmentid,
--									snapshotid,
--									registeredserverid,
--									connectionname,
--									collectiontime,
--									metricid,
--									metricname,
--									metrictype,
--									metricseveritycode,
--									metricseverity,
--									metricseverityvalues,
--									metricdescription,
--									metricreportkey,
--									metricreporttext,
--									severitycode,
--									severity,
--									currentvalue,
--									thresholdvalue)
--							values (
--									@policyid,
--									@assessmentid,
--									null,
--									null,
--									N'',
--									null,
--									@metricid,
--									@metricname,
--									@metrictype,
--									@severity,
--									dbo.getpolicyseverityname(@severity),
--									@configuredvalues,
--									@metricdescription,
--									@metricreportkey,
--									@metricreporttext,
--									@sevcode,
--									dbo.getpolicyseverityname(@sevcode),
--									@metricval, 
--									@metricthreshold
--									)
--				end
--
--				fetch next from metriccursor into @metricid, @metricname, @metrictype,
--													@metricdescription, @metricreportkey, @metricreporttext,
--													@severity, @severityvalues
--			end

			close metriccursor
			deallocate metriccursor

			-- if any servers are left in the server table, there was no audit data and this is a finding
			if exists (select * from #servertbl)
			begin
				-- Audited Servers
				select @metricid=metricid,
						@metricname=metricname,
						@metrictype=metrictype,
						@severity=severity,
						@metricdescription=metricdescription,
						@metricreportkey=reportkey,
						@metricreporttext=reporttext
					from vwpolicymetric
					where  policyid = @policyid
						and assessmentid = @assessmentid
						and metricid = 54
						and isenabled = 1

				if (@metricid = 54)	--	only process if the metric was enabled
				begin
					declare servercursor cursor static for
						select registeredserverid, connectionname
							from registeredserver
							where registeredserverid in (select registeredserverid from #servertbl)
					open servercursor

					fetch next from servercursor into @registeredserverid, @connection
					while (@@fetch_status = 0)
					begin
						select @metricval = N'Server has no audit data for the selections.'

						select @metricthreshold = N'Assessment may not be valid if all servers do not have audit data.'

						if (exists (select * from @returnservertbl where registeredserverid = @registeredserverid))
							insert into @outtbl (
										registeredserverid,
										connectionname,
										metricid,
										metricname,
										metrictype,
										metricseveritycode,
										metricseverity,
										metricseverityvalues,
										metricdescription,
										metricreportkey,
										metricreporttext,
										severitycode,
										severity,
										currentvalue,
										thresholdvalue)
								values (
										@registeredserverid,
										@connection,
										@metricid,
										@metricname,
										@metrictype,
										@severity,
										dbo.getpolicyseverityname(@severity),
										N'',
										@metricdescription,
										@metricreportkey,
										@metricreporttext,
										@severity,
										dbo.getpolicyseverityname(@severity),
										@metricval,
										@metricthreshold
										)

						if (@isadmin = 1)
							insert into policyassessment (
											policyid,
											assessmentid,
											snapshotid,
											registeredserverid,
											connectionname,
											collectiontime,
											metricid,
											metricname,
											metrictype,
											metricseveritycode,
											metricseverity,
											metricseverityvalues,
											metricdescription,
											metricreportkey,
											metricreporttext,
											severitycode,
											severity,
											currentvalue,
											thresholdvalue)
									values (
											@policyid,
											@assessmentid,
											null,
											@registeredserverid,
											@connection,
											null,
											@metricid,
											@metricname,
											@metrictype,
											@severity,
											dbo.getpolicyseverityname(@severity),
											N'',
											@metricdescription,
											@metricreportkey,
											@metricreporttext,
											@severity,
											dbo.getpolicyseverityname(@severity),
											@metricval,
											@metricthreshold
											)
						fetch next from servercursor into @registeredserverid, @connection
					end

					close servercursor
					deallocate servercursor
				end
			end
		end

		drop table #servertbl

		if (@isadmin = 1)
		begin
			-- log the changes
			declare @msg nvarchar(128), @assessmentstate nchar(1)
			select @assessmentstate = assessmentstate,
					@msg = N'Refreshed assessment findings from audit data'
				from assessment
				where policyid = @policyid
					and assessmentid = @assessmentid
			exec isp_sqlsecure_addpolicychangelog @policyid=@policyid, @assessmentid=@assessmentid, @state=@assessmentstate, @description=@msg
		end

		commit transaction
	end

	-- return the results and add the explanations and the updated severitycode with an explanation
	-- severity codes will have 10 added if they are findings otherwise, if ok, then they are < 10
	select a.*, 
			isexplained=isnull(b.isexplained,0), 
			severitycodeexplained=severitycode + case when severitycode > 0 and isnull(b.isexplained,0) = 0 then 10 else 0 end,
			notes=isnull(b.notes,N'')
		from @outtbl a left join policyassessmentnotes b
					on b.policyid = @policyid
						and b.assessmentid = @assessmentid
						and a.metricid = b.metricid
						and a.snapshotid = b.snapshotid
		order by a.snapshotid, a.metricid

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_getpolicyassessment] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
