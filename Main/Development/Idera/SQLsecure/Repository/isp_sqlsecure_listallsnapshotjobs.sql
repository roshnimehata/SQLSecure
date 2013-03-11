SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_listallsnapshotjobs]'))
drop procedure [dbo].[isp_sqlsecure_listallsnapshotjobs]
GO

CREATE procedure [dbo].[isp_sqlsecure_listallsnapshotjobs] 
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --             Return all SQLsecure snapshot jobs not grooming jobs.
   -- 	           


	EXEC msdb.dbo.sp_help_job @category_name  = 'SQLsecureJobs', @job_type = 'LOCAL'

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_listallsnapshotjobs] TO [SQLSecureView]

GO

