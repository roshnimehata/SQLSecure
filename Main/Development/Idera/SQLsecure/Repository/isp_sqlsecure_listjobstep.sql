SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_listjobstep]'))
drop procedure [dbo].[isp_sqlsecure_listjobstep]
GO

CREATE procedure [dbo].[isp_sqlsecure_listjobstep] (@jobid uniqueidentifier)
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --             Return all SQLsecure job steps or collector command including snapshot and grooming jobs.
   -- 	           

	EXEC msdb.dbo.sp_help_job @job_id = @jobid, @job_aspect = 'STEPS'

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_listjobstep] TO [SQLSecureView]

GO

