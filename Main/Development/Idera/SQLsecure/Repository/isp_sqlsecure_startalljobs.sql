SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_startalljobs]'))
drop procedure [dbo].[isp_sqlsecure_startalljobs]
GO

CREATE procedure [dbo].[isp_sqlsecure_startalljobs] 
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Start all SQLsecure SQL Server jobs

	declare @jobid uniqueidentifier

	declare myc100 cursor for
		select job_id from msdb.dbo.sysjobs a, msdb.dbo.syscategories b where a.category_id = b.category_id and a.enabled = 1 and UPPER(b.name) IN ('SQLSECUREJOBS', 'SQLSECUREGROOMINGJOBS')
	
	open myc100
	fetch next from myc100
	into @jobid
	
	while @@fetch_status = 0
	begin
		exec isp_sqlsecure_startjob @jobid = @jobid

		fetch next from myc100
		into @jobid

	end
	
	close myc100
	deallocate myc100	


GO

