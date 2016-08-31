SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_report_authentication]'))
drop procedure [dbo].[isp_sqlsecure_report_authentication]
GO

CREATE procedure [dbo].[isp_sqlsecure_report_authentication]
(
	@rundate datetime = null,	--defaults to all
	@policyid int = 1,			--defaults to all
	@usebaseline bit = 0,		--defaults to false
	@serverName nvarchar(400)
)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Check all servers for mixed mode authentication
   -- 	           

CREATE TABLE #tmpservers (registeredserverid int)
INSERT #tmpservers
    EXEC [dbo].[isp_sqlsecure_getpolicymemberlist]
         @policyid = @policyid  

SELECT	DISTINCT a.connectionname
FROM	serversnapshot a, 
		dbo.getsnapshotlist(@rundate, @usebaseline) b
WHERE	b.registeredserverid IN (SELECT registeredserverid FROM #tmpservers) 
		AND a.snapshotid = b.snapshotid
		AND UPPER(a.connectionname) LIKE UPPER(@serverName)
		AND a.authenticationmode = 'M'

ORDER BY a.connectionname

DROP TABLE #tmpservers

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_report_authentication] TO [SQLSecureView]

GO

