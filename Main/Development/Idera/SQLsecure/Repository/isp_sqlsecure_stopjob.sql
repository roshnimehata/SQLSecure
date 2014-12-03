SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_stopjob]'))
drop procedure [dbo].[isp_sqlsecure_stopjob]
GO

CREATE procedure [dbo].[isp_sqlsecure_stopjob] (@jobid uniqueidentifier)
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --             Stop a job.
   -- 	           

	EXEC msdb.dbo.sp_stop_job @job_id = @jobid

GO

