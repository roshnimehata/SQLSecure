SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_listallgroomingjobs]'))
drop procedure [dbo].[isp_sqlsecure_listallgroomingjobs]
GO

CREATE procedure [dbo].[isp_sqlsecure_listallgroomingjobs] 
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --             Return all SQLsecure grooming jobs.
   -- 	           

	EXEC msdb.dbo.sp_help_job @category_name  = 'SQLsecureGroomingJobs', @job_type = 'LOCAL'

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_listallgroomingjobs] TO [SQLSecureView]

GO
