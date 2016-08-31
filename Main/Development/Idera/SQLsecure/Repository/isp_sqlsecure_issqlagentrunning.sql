SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_issqlagentrunning]'))
drop procedure [dbo].[isp_sqlsecure_issqlagentrunning]
GO

CREATE procedure [dbo].[isp_sqlsecure_issqlagentrunning] 
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Checks if SQL agent is running or not. Returns a Y or N
   -- 	    

	create table #tmpdata (data nvarchar(64))

	insert into #tmpdata exec master.dbo.xp_servicecontrol 'QUERYSTATE', 'SQLServerAgent' 

	select isrunning=CASE WHEN UPPER(data collate database_default) = N'RUNNING.' THEN N'Y' ELSE N'N' END from #tmpdata

	drop table #tmpdata

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_issqlagentrunning] TO [SQLSecureView]

GO
