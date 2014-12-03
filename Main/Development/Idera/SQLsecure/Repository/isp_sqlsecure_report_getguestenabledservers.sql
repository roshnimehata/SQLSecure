SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_report_getguestenabledservers]'))
drop procedure [dbo].[isp_sqlsecure_report_getguestenabledservers]
GO

CREATE procedure [dbo].[isp_sqlsecure_report_getguestenabledservers]
(
	@rundate datetime = null,	--defaults to all
	@policyid int = 1,			--defaults to all
	@usebaseline bit = 0,		--defaults to false
	@connectionname nvarchar(400)
)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get all the databases that have guest users enabled.

CREATE TABLE #tmpservers (registeredserverid int)
INSERT #tmpservers
    EXEC [dbo].[isp_sqlsecure_getpolicymemberlist]
         @policyid = @policyid  

SELECT b.connectionname,
		a.databasename,
		a.[owner]
FROM sqldatabase a,
		serversnapshot b
WHERE a.snapshotid = b.snapshotid
		AND b.snapshotid IN (SELECT snapshotid FROM dbo.getsnapshotlist(@rundate, @usebaseline)) 
		AND b.registeredserverid IN (SELECT registeredserverid FROM #tmpservers)
		AND ((case when @connectionname = '%' or @connectionname = '' then 1 else 0 end = 1)
				or (UPPER(b.connectionname) = UPPER(@connectionname)))
		AND a.guestenabled = 'Y'
ORDER BY b.connectionname, a.databasename

DROP TABLE #tmpservers

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_report_getguestenabledservers] TO [SQLSecureView]
