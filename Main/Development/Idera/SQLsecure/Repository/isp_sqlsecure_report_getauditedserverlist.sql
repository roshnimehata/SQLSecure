SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_report_getauditedserverlist]'))
drop procedure [dbo].[isp_sqlsecure_report_getauditedserverlist]
GO

CREATE PROC [dbo].[isp_sqlsecure_report_getauditedserverlist]
(
	@policyid int = 1,				-- default to All Servers policy
	@assessmentid int = null,		-- default to policy settings for backward compatibility
	@assessmentid2 int = null		-- default to null for no multiple assessment processing
)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get all available servers in the selected policy or assessment for Reporting Services reports
   --				The value 'All servers in policy' is always added as the first item with id 0
   --				If assessmentid2 is passed, then it will return only servers that are in both assessment server lists
   --
   --				If no assessmentid is passed, then it will default to the policy
   --				If no policyid is passed, then it will default to all servers

create table #tmpservers (registeredserverid int)
	insert #tmpservers
		EXEC [dbo].[isp_sqlsecure_getpolicymemberlist]
			 @policyid = @policyid,
			 @assessmentid = @assessmentid

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[#servers]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE #servers

CREATE TABLE #servers ([server] nvarchar(400), registeredserverid int)

INSERT INTO #servers VALUES('All servers in policy', 0)

INSERT INTO #servers 
	SELECT	connectionname AS [server], registeredserverid 
	FROM	vwregisteredserver
	WHERE	registeredserverid IN (SELECT registeredserverid FROM #tmpservers)
	ORDER BY connectionname

if @assessmentid2 is null
	SELECT [server], registeredserverid FROM #servers
else
begin
	-- process the second assessment
	delete #tmpservers
	insert #tmpservers
		EXEC [dbo].[isp_sqlsecure_getpolicymemberlist]
			 @policyid = @policyid,
			 @assessmentid = @assessmentid2

	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[#servers2]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE #servers2

	CREATE TABLE #servers2 ([server] nvarchar(400), registeredserverid int)

	INSERT INTO #servers2 VALUES('All servers in policy', 0)	-- create the matching all servers record

	INSERT INTO #servers2 
		SELECT	connectionname AS [server], registeredserverid 
		FROM	vwregisteredserver
		WHERE	registeredserverid IN (SELECT registeredserverid FROM #tmpservers)

	SELECT [server], registeredserverid FROM #servers where registeredserverid in (SELECT registeredserverid FROM #servers2)

	DROP TABLE #servers2
end	

DROP TABLE #servers
DROP TABLE #tmpservers

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_report_getauditedserverlist] TO [SQLSecureView]

GO

