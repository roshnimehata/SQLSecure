declare @repository nvarchar(128),
	@servers xml,
	@credentialserver nvarchar(400),
	@retention int,
	@isenabled bit,
	@notifyemail nvarchar(1024),
	@notifysnapshot nchar(1),
	@notifypolicy int

--********************************************************************************************
--		enter user info here
--********************************************************************************************

-- enter the name of the SQL Server instance that hosts the SQLsecure repository
select @repository = N'msodevsecure25'

-- enter a list of servers with their SQL Server version number here.
-- only the major and minor version numbers are important here. it will be replaced with the correct value when a snapshot is taken.
select @servers = N'
<servers>
	<serverinfo name="vm09" ver="08.00.4347" />
	<serverinfo name="vm09\sql2005" ver="9.00.4262.00" />
	<serverinfo name="vm09\sql2008,51433" ver="10.00.1600" />
</servers>'

-- enter the full instance name of the audited server that contains credentials that can be copied for these servers.
select @credentialserver = N'msovm08'

-- enter the time in days that snapshots should be kept before being groomed 
select @retention = 60

-- enter whether automatic snapshots should be run or not for the server
select @isenabled = 1		-- 1 is enabled and 0 is not enabled

-- enter the email addresses to be provided for notifications after snapshots are run. 
-- Leave it empty to skip notifications.
select @notifyemail = N'me@mycompany.com;you@mycompany.com'

-- enter the setting for notifications of snapshot results
select @notifysnapshot=N'E'	--  A = Any snapshot, W = Warning & Error, E = Error (the default), N = don't notify

-- enter the setting for notifications of snapshot results
select @notifypolicy=3		--	1 = Any Risk, 2 = Medium & High Risks, 3 = High Risks (the default), 0 = don't notify

-- this script does not currently allow setting filters

-- to set a different job schedule search for isp_sqlsecure_addnewsnapshotjob in this script 
--     and update the identified sp_add_jobschedule parameters

--********************************************************************************************
--		end enter user info
--********************************************************************************************

use SQLsecure

declare @serverinstance nvarchar(400),
	@server nvarchar(128),
	@instance nvarchar(128),
	@port int,
	@SqlAuthType nchar(1),
	@SqlLogin nvarchar(128),
	@SqlPw nvarchar(300),
	@WinLogin nvarchar(128),
	@WinPw nvarchar(128),
	@ver nvarchar(256),
	@rc int,
	@rid int,
	@notify bit,
	@fid int,
	@2000 bit,
	@jobname nvarchar(256)

-- get credentials from the specified server to preserve settings and encrypted password
select
		@SqlAuthType = sqlserverauthtype,
		@SqlLogin = sqlserverlogin,				
		@SqlPw = sqlserverpassword,
		@WinLogin = serverlogin,
		@WinPw = serverpassword
	from registeredserver
	where connectionname = upper(@credentialserver)

-- make sure these aren't null for comparison purposes
select @rc = 0,
	@rid = 0,
	@fid = 0

if (@SqlAuthType is not null)
begin
	declare xmlcursor cursor forward_only static for
		select
				x.item.value('@name', 'nvarchar(400)') as name,
				x.item.value('@ver', 'nvarchar(256)') as ver
			from @servers.nodes('//servers/serverinfo') AS x(item)
	open xmlcursor

	fetch next from xmlcursor into @serverinstance, @ver
	while @@fetch_status = 0
	begin
		select @serverinstance = upper(@serverinstance)
		print N'processing server ' + @serverinstance

		-- parse the instance name to remove the port before using the name
		if (charindex(N',',@serverinstance) > 0)
		begin
			select @port = right(@serverinstance, len(@serverinstance) - charindex(N',',@serverinstance))
			select @serverinstance = left(@serverinstance, len(@serverinstance) - len(@port) - 1)
		end
		else
			select @port = null

		if (exists (select 1 from registeredserver where connectionname = @serverinstance))
		begin
			print N'server ' + @serverinstance + N' is already registered and will be skipped.'
		end
		else
		begin
			-- parse the instance name to get the server and instance values
			if (charindex(N'\',@serverinstance) > 0)
			begin
				select @server = left(@serverinstance, charindex(N'\',@serverinstance)-1),
						@instance = right(@serverinstance, len(@serverinstance) - charindex(N'\',@serverinstance))
			end
			else
			begin
				select @server = @serverinstance,
						@instance = N''
			end
			
			-- validate the retention
			if (isnull(@retention,0) < 1)
				select @retention = 1
			else if (@retention > 10000)
				select @retention = 10000


			-- register the server with sqlsecure
			exec @rc = SQLsecure.dbo.isp_sqlsecure_addregisteredserver 
				@connectionname=@serverinstance,
				@connectionport=@port,
				@servername=@server,
				@instancename=@instance,
				@authmode=@SqlAuthType,
				@loginname=@SqlLogin,
				@loginpassword=@SqlPw,
				@serverlogin=@WinLogin,
				@serverpassword=@WinPw,
				@version=@ver,
				@retentionperiod=@retention

			if (@rc = 0)
			begin
				select @rid = registeredserverid from registeredserver where connectionname = @serverinstance
				--print 'registeredserverid=' + cast(@rid as nvarchar)
			end

			--add the notification to the server if an email was specified
			if (@rc = 0 and @rid > 0)
			begin
				select @notify = case len(isnull(@notifyemail,N'')) when 0 then 0 else 1 end
				select @notifysnapshot = upper(@notifysnapshot)
				if (@notify = 0 or @notifysnapshot not in (N'A', N'W', N'E', N'N'))
					select @notify = N'N'
				if (@notify = 0 or @notifypolicy not between 0 and 3)
					select @notify = 0

				-- this currently uses the default settings for notifications of snapshot errors and high policy risks
				exec @rc = SQLsecure.dbo.isp_sqlsecure_addnotificationtoregisteredserver 
					@registeredserverid=@rid,
					@notificationproviderid=1,		-- id of the notification provider, this is currently always 1
					@snapshotstatus=@notifysnapshot,
					@policymetricseverity=@notifypolicy,
					@recipients=@notifyemail
			end

			if (@rc = 0 and @rid > 0)
			begin
				-- add the default filter rule for the server and set it to audit everything
				exec @rc = SQLsecure.dbo.isp_sqlsecure_addruleheader @connectionname=@serverinstance,@rulename=N'Default rule',@description=N'Rule created when the server was registered'
			end

			if (@rc = 0 and @rid > 0)
			begin
				select @fid = filterruleheaderid from filterruleheader where connectionname = @serverinstance
				--print 'filterruleheaderid=' + cast(@fid as nvarchar)
			end

			if (@rc = 0 and @fid > 0)
			begin
				-- not all filters apply for 2000
				select @2000 = case when (left(@ver, 2) = N'8.' or left(@ver, 3) = N'08.') then 1 else 0 end

				-- add the object type filter rules to capture everything
					exec @rc = SQLsecure.dbo.isp_sqlsecure_addrule @ruleheaderid=@fid,@class=20,@scope=N'A',@matchstring=N''
				if (@rc = 0)
					exec @rc = SQLsecure.dbo.isp_sqlsecure_addrule @ruleheaderid=@fid,@class=43,@scope=N'S',@matchstring=N''
				if (@rc = 0)
					exec @rc = SQLsecure.dbo.isp_sqlsecure_addrule @ruleheaderid=@fid,@class=41,@scope=N'A',@matchstring=N''
				if (@rc = 0)
					exec @rc = SQLsecure.dbo.isp_sqlsecure_addrule @ruleheaderid=@fid,@class=42,@scope=N'A',@matchstring=N''
				if (@rc = 0)
					exec @rc = SQLsecure.dbo.isp_sqlsecure_addrule @ruleheaderid=@fid,@class=44,@scope=N'A',@matchstring=N''
				if (@rc = 0)
					exec @rc = SQLsecure.dbo.isp_sqlsecure_addrule @ruleheaderid=@fid,@class=45,@scope=N'A',@matchstring=N''
				if (@rc = 0 and @2000 = 0)
					exec @rc = SQLsecure.dbo.isp_sqlsecure_addrule @ruleheaderid=@fid,@class=21,@scope=N'A',@matchstring=N''
				if (@rc = 0 and @2000 = 0)
					exec @rc = SQLsecure.dbo.isp_sqlsecure_addrule @ruleheaderid=@fid,@class=22,@scope=N'A',@matchstring=N''
				if (@rc = 0 and @2000 = 0)
					exec @rc = SQLsecure.dbo.isp_sqlsecure_addrule @ruleheaderid=@fid,@class=46,@scope=N'A',@matchstring=N''
				if (@rc = 0 and @2000 = 0)
					exec @rc = SQLsecure.dbo.isp_sqlsecure_addrule @ruleheaderid=@fid,@class=26,@scope=N'A',@matchstring=N''
				if (@rc = 0 and @2000 = 0)
					exec @rc = SQLsecure.dbo.isp_sqlsecure_addrule @ruleheaderid=@fid,@class=31,@scope=N'A',@matchstring=N''
				if (@rc = 0 and @2000 = 0)
					exec @rc = SQLsecure.dbo.isp_sqlsecure_addrule @ruleheaderid=@fid,@class=32,@scope=N'A',@matchstring=N''
				if (@rc = 0 and @2000 = 0)
					exec @rc = SQLsecure.dbo.isp_sqlsecure_addrule @ruleheaderid=@fid,@class=28,@scope=N'A',@matchstring=N''
			end

			select @jobname = N'SQLsecure Collector Job - ' + @serverinstance
			-- this currently uses the default run time values of 3am every Sunday 
			exec @rc = SQLsecure.dbo.isp_sqlsecure_addnewsnapshotjob 
				@connectionname=@serverinstance,
				@targetserver=@serverinstance,
				@snapshotjobname=@jobname,
				@repositoryname=@repository,
				@freqtype=8,					-- uses sp_add_jobschedule parameter values
				@freqinterval=1,				-- uses sp_add_jobschedule parameter values
				@freqsubdaytype=1,				-- uses sp_add_jobschedule parameter values
				@freqsubdayinterval=0,			-- uses sp_add_jobschedule parameter values
				@freqrelativeinterval=0,		-- uses sp_add_jobschedule parameter values
				@freqrecurencefactor=1,			-- uses sp_add_jobschedule parameter values
				@activestarttime=30000,			-- uses sp_add_jobschedule parameter values
				@activeendtime=0,				-- uses sp_add_jobschedule parameter values
				@isenabled=@isenabled

			print @serverinstance + N' registered with id ' + cast (@rid as nvarchar)
		end

		fetch next from xmlcursor into @serverinstance, @ver
	end

	close xmlcursor
	deallocate xmlcursor
end

select * from registeredserver