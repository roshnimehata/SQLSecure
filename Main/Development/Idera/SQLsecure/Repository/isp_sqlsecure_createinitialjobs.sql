SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_createinitialjobs]'))
drop procedure [dbo].[isp_sqlsecure_createinitialjobs]
GO

CREATE procedure [dbo].[isp_sqlsecure_createinitialjobs] 
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Creates initial categories for snapshot and grooming jobs.


	if not exists (select 1 from msdb.dbo.syscategories where category_class = 1 and category_type = 1 and UPPER(name) = 'SQLSECUREJOBS')
	begin
		EXEC msdb.dbo.sp_add_category 'JOB', 'LOCAL', 'SQLsecureJobs'
	end
	
	
	if not exists (select 1 from msdb.dbo.syscategories where category_class = 1 and category_type = 1 and UPPER(name) = 'SQLSECUREGROOMINGJOBS')
	begin
		EXEC msdb.dbo.sp_add_category 'JOB', 'LOCAL', 'SQLsecureGroomingJobs'
	end


	if not exists (select 1 from msdb.dbo.sysjobs where name = 'SQLsecure Grooming Job')
	begin
		EXEC SQLsecure.dbo.isp_sqlsecure_addnewgroomingjob @groomjobname='SQLsecure Grooming Job', @jobdescription='Deletes all SQLsecure snapshots where the server no longer exists or that are older than the server data retention period.', @isenabled=1
	end


GO

