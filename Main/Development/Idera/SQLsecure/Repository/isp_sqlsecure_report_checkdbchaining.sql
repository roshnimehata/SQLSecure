SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_report_checkdbchaining]'))
drop procedure [dbo].[isp_sqlsecure_report_checkdbchaining] 
GO

CREATE procedure [dbo].[isp_sqlsecure_report_checkdbchaining]
(
	@rundate datetime = null,	--defaults to all
	@policyid int = 1,			--defaults to all
	@usebaseline bit = 0		--defaults to false
)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Checks all servers for cross db ownership chaining
   -- 	           

CREATE TABLE #tmpservers (registeredserverid int)
INSERT #tmpservers
    EXEC [dbo].[isp_sqlsecure_getpolicymemberlist]
         @policyid = @policyid 

SELECT	DISTINCT a.connectionname,
		CASE WHEN crossdbownershipchaining = 'Y' 
			THEN 'Enabled' 
			ELSE 'Disabled'
		END	
FROM	serversnapshot a
WHERE	a.registeredserverid IN (SELECT registeredserverid FROM #tmpservers)
		AND a.snapshotid IN (SELECT snapshotid FROM dbo.getsnapshotlist(@rundate, @usebaseline))
		AND crossdbownershipchaining = 'Y'
ORDER BY a.connectionname

DROP TABLE #tmpservers

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_report_checkdbchaining] TO [SQLSecureView]

GO