SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_addnewsnapshotjob]'))
drop procedure [dbo].[isp_sqlsecure_addnewsnapshotjob]
GO

CREATE procedure [dbo].[isp_sqlsecure_addnewsnapshotjob] (@connectionname nvarchar(400), @snapshotjobname nvarchar(128), @jobdescription nvarchar(512)=null, @targetserver nvarchar(500), @repositoryname nvarchar(500), @freqtype int=4, @freqinterval int=1, @freqsubdaytype int=0, @freqsubdayinterval int=0, @freqrelativeinterval int=0, @freqrecurencefactor int=0, @activestartdate int=NULL, @activeenddate int=99991231, @activestarttime int=000000, @activeendtime int=235959 ,@isenabled tinyint=1,  @notifylevelemail int=0, @notifyemailoperatorname nvarchar(500)=null)
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Create new job to capture server snapshot.
   -- 	           

	declare @err int
	declare @errmsg nvarchar(500)
	declare @snapshotjobid uniqueidentifier
	declare @jobstepname nvarchar(128)
	declare @collectorpath nvarchar(1000)
	declare @command nvarchar(1000)
	declare @errorflag int
	declare @programname nvarchar(128)
	declare @adminname nvarchar(128)

	set @jobstepname = 'SQLsecure Collector Job'

	select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)
	select @adminname = SUSER_SNAME(0x01)

	-- GET COLLECTOR EXECUTABLE FILE PATH
	select @collectorpath =  value from collectorinfo where UPPER(name) = 'FILEPATH'

	if (@collectorpath IS NULL)
	begin
		set @errmsg = 'Error: SQLsecure Collector file path is not valid.'
		exec isp_sqlsecure_addactivitylog @activitytype=N'Error', @source=@programname, @eventcode=N'Add', @category=N'Job', @description=@errmsg, @connectionname = @connectionname
		RAISERROR (@errmsg, 16, 1)
		return -1
	end

	-- VALIDATION HERE
	-- CHECK IF JOB ALREADY EXISTS AND DELETE IT
	if exists (select 1 from msdb.dbo.sysjobs where UPPER(name) = UPPER(@snapshotjobname))
	begin
		exec isp_sqlsecure_removejob @jobid=null, @jobname=@snapshotjobname
	end
	-- CHECK AND MAKE SURE IT WAS DELETED
	if exists (select 1 from msdb.dbo.sysjobs where UPPER(name) = UPPER(@snapshotjobname))
	begin
		set @errmsg = 'Error: Job name ' + @snapshotjobname + ' already exists.'
		exec isp_sqlsecure_addactivitylog @activitytype=N'Error', @source=@programname, @eventcode=N'Add', @category=N'Job', @description=@errmsg, @connectionname = @connectionname
		RAISERROR (@errmsg, 16, 1)
		return -1
	end

	if (@snapshotjobname IS NULL or @snapshotjobname = '')
	begin
		set @errmsg = 'Error: Job name cannot be empty.'
		exec isp_sqlsecure_addactivitylog @activitytype=N'Error', @source=@programname, @eventcode=N'Add', @category=N'Job', @description=@errmsg, @connectionname = @connectionname
		RAISERROR (@errmsg, 16, 1)
		return -1
	end
	
	-- TODO: check servername, repository name...etc

	set @command = '"' + @collectorpath + '" -TargetInstance "' + @targetserver + '" -Repository "' + @repositoryname + '"'

	--print 'command ' + @command

	-- CREATE JOB

	--print 'job name ' + @snapshotjobname

	EXEC msdb.dbo.sp_add_job 
		@job_name = @snapshotjobname, 
		@enabled = @isenabled, 
		@description = @jobdescription, 
		@start_step_id=1, 
		@category_name='SQLsecureJobs', 
		@category_id=NULL, 
		@owner_login_name=@adminname, 
		@notify_level_eventlog=2, 
		@notify_level_email = @notifylevelemail, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@notify_email_operator_name = @notifyemailoperatorname, 
		@notify_netsend_operator_name=NULL, 
		@notify_page_operator_name =NULL, 
		@delete_level =0,
		@job_id=@snapshotjobid OUTPUT

	--print 'after add job'

	--select @snapshotjobid = data from #tmpdata

	--print 'snapshot job id ' + CONVERT(nvarchar(128), @snapshotjobid)

	-- TODO: VALIDATE JOB ID HERE
	if (@snapshotjobid IS NULL or CONVERT(nvarchar(128),@snapshotjobid) = '')
	begin
		set @errmsg = 'Error: Job id cannot be empty.'
		exec isp_sqlsecure_addactivitylog @activitytype=N'Error', @source=@programname, @eventcode=N'Add', @category=N'Job', @description=@errmsg, @connectionname = @connectionname
		RAISERROR (@errmsg, 16, 1)
		return -1
	end

	-- ADD JOB SERVER
	EXEC msdb.dbo.sp_add_jobserver 
		@job_id = @snapshotjobid

	--print 'before creating step'

	-- CREATE STEP
	EXEC msdb.dbo.sp_add_jobstep 
		@job_id = @snapshotjobid, 
		@step_name = @jobstepname, 
		@subsystem = 'CMDEXEC', 
		@command = @command

	select @err = @@error

	if @err <> 0
	begin
		set @errmsg = 'Error: Failed to create job step.'
		exec isp_sqlsecure_addactivitylog @activitytype=N'Error', @source=@programname, @eventcode=N'Add', @category=N'Job', @description=@errmsg, @connectionname = @connectionname
		RAISERROR (@errmsg, 16, 1)
		return -1
	end

	-- CREATE SCHEDULE
	EXEC msdb.dbo.sp_add_jobschedule @job_id = @snapshotjobid,
	    @name = 'default',
	    @enabled = @isenabled,
	    @freq_type = @freqtype,
	    @freq_interval = @freqinterval,
	    @freq_subday_type = @freqsubdaytype,
	    @freq_subday_interval = @freqsubdayinterval,
	    @freq_relative_interval = @freqrelativeinterval,
	    @freq_recurrence_factor = @freqrecurencefactor,
	    @active_start_date = @activestartdate,
	    @active_end_date = @activeenddate,
	    @active_start_time = @activestarttime,
	    @active_end_time = @activeendtime

	if @err <> 0
	begin
		set @errmsg = 'Error: Failed to create job schedule.'
		exec isp_sqlsecure_addactivitylog @activitytype=N'Error', @source=@programname, @eventcode=N'Add', @category=N'Job', @description=@errmsg, @connectionname = @connectionname
		RAISERROR (@errmsg, 16, 1)
		return -1
	end

	-- INSERT NEW JOB TO REGISTERED SERVER TABLE
	if exists (select 1 from registeredserver where UPPER(connectionname) = UPPER(@connectionname))
	begin
		update registeredserver set jobid = @snapshotjobid where UPPER(connectionname) = UPPER(@connectionname)

		select @err = @@error

		if @err <> 0
		begin
			set @errmsg = 'Error: Failed to update registeredserver table with job id ' + CONVERT(nvarchar(128), @snapshotjobid)
			exec isp_sqlsecure_addactivitylog @activitytype=N'Error', @source=@programname, @eventcode=N'Add', @category=N'Job', @description=@errmsg, @connectionname = @connectionname
			RAISERROR (@errmsg, 16, 1)
			return -1
		end
	end
	
	declare @str nvarchar(256)
	set @str = 'Snapshot job ' + @snapshotjobname

	exec isp_sqlsecure_addactivitylog @activitytype=N'Success Audit', @source=@programname, @eventcode=N'Add', @category=N'Job', @description=@str, @connectionname = @connectionname

	if (@isenabled = 0)
	begin
		set @str = 'The Agent Job for server ' + @connectionname + ' has been disabled and automatic auditing is not active for this server.'
		exec isp_sqlsecure_addactivitylog @activitytype=N'Warning', @source=@programname, @eventcode=N'Warning', @category=N'Registered Server', @description=@str, @connectionname = @connectionname
	end

	create table #tmpdata (data uniqueidentifier)

	-- RETURN THE JOB ID
	insert into #tmpdata (data) values (@snapshotjobid)
	select * from #tmpdata

GO

