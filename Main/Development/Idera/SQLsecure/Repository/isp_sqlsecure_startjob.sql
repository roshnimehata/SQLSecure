SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_startjob]'))
drop procedure [dbo].[isp_sqlsecure_startjob]
GO

CREATE procedure [dbo].[isp_sqlsecure_startjob] (@jobid uniqueidentifier)
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --             Start a job.
   -- 	           

	EXEC msdb.dbo.sp_start_job @job_id = @jobid

GO

