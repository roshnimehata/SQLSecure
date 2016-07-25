SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_removejob]'))
drop procedure [dbo].[isp_sqlsecure_removejob]
GO

CREATE procedure [dbo].[isp_sqlsecure_removejob] (@jobid uniqueidentifier, @jobname nvarchar(512) = '')
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --             Remove a job by either the jobid or job name.
   -- 	           
	declare @errmsg nvarchar(500)
	declare @programname nvarchar(128)
	declare @jobid2 uniqueidentifier

	set @jobid2 = null

	select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)

	select @jobid2 = job_id from msdb.dbo.sysjobs where job_id = @jobid or UPPER(name) = UPPER(@jobname)
	if (@jobid2 is null)
	begin
		set @errmsg = 'Error: Failed to remove job.'
		if (@jobid is not null)
			set @errmsg = @errmsg + ' Job id ' + CONVERT(nvarchar(128), @jobid) + ' does not exist.'
		else if (@jobname is not null and len(rtrim(@jobname)) > 0)
			set @errmsg = @errmsg + ' Job name ' + @jobname + ' does not exist.'
		else 
			set @errmsg = @errmsg + ' No valid Job id or name was passed.'
		exec isp_sqlsecure_addactivitylog @activitytype=N'Failure Audit', @source=@programname, @eventcode=N'Add', @category=N'Job', @description=@errmsg, @connectionname = null
		RAISERROR (@errmsg, 16, 1)
		return -1
	end

	EXEC msdb.dbo.sp_delete_job @job_id = @jobid2

	update registeredserver set jobid = NULL where jobid = @jobid2
GO


