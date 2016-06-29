SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_getjobstatus]'))
drop procedure [dbo].[isp_sqlsecure_getjobstatus]
GO

CREATE procedure [dbo].[isp_sqlsecure_getjobstatus] (@jobid uniqueidentifier)
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Check the current job status
   -- 	           

	create table #tmpjobstatus (result nvarchar(32))

	create table #xp_results (
		job_id  uniqueidentifier not null,
		last_run_date         int              not null,
		last_run_time         int              not null,
		next_run_date         int              not null,
		next_run_time         int              not null,
		next_run_schedule_id  int              not null,
		requested_to_run      int              not null, -- bool
		request_source        int              not null,
		request_source_id     sysname          collate database_default null,
		running               int              not null, -- bool
		current_step          int              not null,
		current_retry_attempt int              not null,
		job_state             int              not null)

	declare @is_sysadmin int
	declare @job_owner   sysname
	declare @is_running int
	
	select @is_sysadmin = isnull(is_srvrolemember(N'sysadmin'), 0)
	select @job_owner = suser_sname()

	insert into #xp_results execute master.dbo.xp_sqlagent_enum_jobs @is_sysadmin, @job_owner, @jobid

	select @is_running = running from #xp_results

	if (@is_running = 1)
	begin
		insert into #tmpjobstatus (result) values ('Running')
	end
	else
	begin
		if not exists (select 1 from msdb.dbo.sysjobs where job_id = @jobid)
		begin
			insert into #tmpjobstatus (result) values ('Not found')
		end
		else if exists (select 1 from msdb.dbo.sysjobhistory where job_id = @jobid)
		begin
			insert into #tmpjobstatus (result) select CASE WHEN run_status = 0 THEN 'Failed' WHEN run_status = 1 THEN 'Succeeded' WHEN run_status = 2 THEN 'Retry' WHEN run_status = 3 THEN 'Canceled' WHEN run_status = 4 THEN 'In progress' ELSE 'Unknown' END from msdb.dbo.sysjobhistory where job_id=@jobid and instance_id = (select max(instance_id) from msdb.dbo.sysjobhistory where job_id = @jobid)
		end
		else
		begin
			insert into #tmpjobstatus (result) values ('Not running')
		end
	end

	select * from #tmpjobstatus

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_getjobstatus] TO [SQLSecureView]

GO
