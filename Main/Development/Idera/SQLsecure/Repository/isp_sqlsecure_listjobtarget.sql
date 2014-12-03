SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_listjobtarget]'))
drop procedure [dbo].[isp_sqlsecure_listjobtarget]
GO

CREATE procedure [dbo].[isp_sqlsecure_listjobtarget] (@jobid uniqueidentifier)
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --             Return all SQLsecure job target server (mostly local server name) including snapshot and grooming jobs.
   -- 	           

	EXEC msdb.dbo.sp_help_job @job_id = @jobid, @job_aspect = 'TARGETS'

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_listjobtarget] TO [SQLSecureView]

GO

