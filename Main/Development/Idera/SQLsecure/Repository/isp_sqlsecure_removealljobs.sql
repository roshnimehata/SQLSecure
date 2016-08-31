SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_removealljobs]'))
drop procedure [dbo].[isp_sqlsecure_removealljobs]
GO

CREATE procedure [dbo].[isp_sqlsecure_removealljobs] 
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Remove all SQLsecurejobs

	declare @jobid uniqueidentifier

	declare myc100 cursor for
		select job_id from msdb.dbo.sysjobs a, msdb.dbo.syscategories b where a.category_id = b.category_id and UPPER(b.name) IN ('SQLSECUREJOBS', 'SQLSECUREGROOMINGJOBS')
	
	open myc100
	fetch next from myc100
	into @jobid
	
	while @@fetch_status = 0
	begin
		exec isp_sqlsecure_removejob @jobid = @jobid

		fetch next from myc100
		into @jobid

	end
	
	close myc100
	deallocate myc100	


GO

