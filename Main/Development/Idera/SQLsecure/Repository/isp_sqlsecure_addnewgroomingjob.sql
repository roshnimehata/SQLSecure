SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_addnewgroomingjob]'))
drop procedure [dbo].[isp_sqlsecure_addnewgroomingjob]
GO

CREATE procedure [dbo].[isp_sqlsecure_addnewgroomingjob] (@groomjobname nvarchar(128), @jobdescription nvarchar(512)=null, @freqtype int=8, @freqinterval int=2, @freqsubdaytype int=0, @freqsubdayinterval int=0, @freqrelativeinterval int=0, @freqrecurencefactor int=1, @activestartdate int=NULL, @activeenddate int=99991231, @activestarttime int=000000, @activeendtime int=235959 ,@isenabled tinyint=1,  @notifylevelemail int=0, @notifyemailoperatorname nvarchar(500)=null)
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Create new job to groom all out-dated snapshot data or snapshot data belonging to registered server that got deleted.
   -- 	           

	declare @err int
	declare @errmsg nvarchar(500)
	declare @command nvarchar(1000)
	declare @snapshotjobid uniqueidentifier
	declare @jobstepname nvarchar(128)
	declare @errorflag int
	declare @programname nvarchar(128)
	declare @adminname nvarchar(128)

	set @jobstepname = 'SQLsecure Grooming Job'

	select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)
	select @adminname = SUSER_SNAME(0x01)

	-- VALIDATION HERE
	-- CHECK IF JOB NAME ALREADY EXISTS
	if exists (select 1 from msdb.dbo.sysjobs where UPPER(name) = @groomjobname)
	begin
		set @errmsg = 'Error: Job name ' + @groomjobname + ' already exists.'
		exec isp_sqlsecure_addactivitylog @activitytype=N'Error', @source=@programname, @eventcode=N'Add', @category=N'Job', @description=@errmsg, @connectionname = null
		RAISERROR (@errmsg, 16, 1)
		return -1
	end

	if (@groomjobname IS NULL or @groomjobname = '')
	begin
		set @errmsg = 'Error: Job name cannot be empty.'
		exec isp_sqlsecure_addactivitylog @activitytype=N'Error', @source=@programname, @eventcode=N'Add', @category=N'Job', @description=@errmsg, @connectionname = null
		RAISERROR (@errmsg, 16, 1)
		return -1
	end


	-- CREATE JOB

	--print 'job name ' + @groomjobname

	EXEC msdb.dbo.sp_add_job 
		@job_name = @groomjobname, 
		@enabled = @isenabled, 
		@description = @jobdescription, 
		@start_step_id=1, 
		@category_name='SQLsecureGroomingJobs', 
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

	-- TODO: VALIDATE JOB ID HERE
	if (@snapshotjobid IS NULL or CONVERT(nvarchar(128),@snapshotjobid) = '')
	begin
		set @errmsg = 'Error: Job id cannot be empty.'
		exec isp_sqlsecure_addactivitylog @activitytype=N'Error', @source=@programname, @eventcode=N'Add', @category=N'Job', @description=@errmsg, @connectionname = null
		RAISERROR (@errmsg, 16, 1)
		return -1
	end

	-- ADD JOB SERVER
	EXEC msdb.dbo.sp_add_jobserver 
		@job_id = @snapshotjobid

	select @err = @@error

	if @err <> 0
	begin
		exec isp_sqlsecure_removejob @snapshotjobid

		set @errmsg = 'Error: Failed to create grooming job.'
		exec isp_sqlsecure_addactivitylog @activitytype=N'Error', @source=@programname, @eventcode=N'Add', @category=N'Job', @description=@errmsg, @connectionname = null
		RAISERROR (@errmsg, 16, 1)
		return -1
	end

	-- groom snapshot data
	set @command = 'exec SQLsecure.dbo.isp_sqlsecure_groomsnapshots'

	-- CREATE STEP
	EXEC msdb.dbo.sp_add_jobstep 
		@job_id = @snapshotjobid, 
		@step_name = @jobstepname, 
		@subsystem = 'TSQL', 
		@command = @command

	select @err = @@error

	if @err <> 0
	begin
		exec isp_sqlsecure_removejob @snapshotjobid

		set @errmsg = 'Error: Failed to create grooming job step.'
		exec isp_sqlsecure_addactivitylog @activitytype=N'Error', @source=@programname, @eventcode=N'Add', @category=N'Job', @description=@errmsg, @connectionname = null
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

		exec isp_sqlsecure_removejob @snapshotjobid

		set @errmsg = 'Error: Failed to create grooming job schedule.'
		exec isp_sqlsecure_addactivitylog @activitytype=N'Error', @source=@programname, @eventcode=N'Add', @category=N'Job', @description=@errmsg, @connectionname = null
		RAISERROR (@errmsg, 16, 1)
		return -1
	end	
	
	declare @str nvarchar(256)
	set @str = 'Grooming job ' + @groomjobname

	-- add an entry to grooming activity table
	insert into groomingactivityhistory (activitystarttime, activityendtime, status, comment) values (GETUTCDATE(), GETUTCDATE(), 'S', @str + ' created.')

	exec isp_sqlsecure_addactivitylog @activitytype=N'Success Audit', @source=@programname, @eventcode=N'Add', @category=N'Job', @description=@str, @connectionname = null

	create table #tmpdata (data uniqueidentifier)

	-- RETURN THE JOB ID
	insert into #tmpdata (data) values (@snapshotjobid)
	select * from #tmpdata

GO
