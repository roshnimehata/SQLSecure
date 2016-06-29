SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_getpolicyserver]'))
drop procedure [dbo].[isp_sqlsecure_getpolicyserver]
GO

CREATE procedure [dbo].[isp_sqlsecure_getpolicyserver]
(
	@policyid int, 
	@assessmentid int = null,		-- default to policy settings for backward compatibility
	@registeredserverid int=0, 
	@usebaseline bit, 
	@rundate datetime = null
)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Return a table with the server metrics of all policy members for the run date
   --				If the assessment is current then the passed parameters for usebaseline and rundate are used
   --				If it is a saved assessment then those values are pulled from the assessment table

SELECT @assessmentid = isnull(@assessmentid,[dbo].[getdefaultassessmentid](@policyid))
CREATE TABLE #tmpservers (registeredserverid int)
INSERT #tmpservers
	EXEC [dbo].[isp_sqlsecure_getpolicymemberlist]
		@policyid = @policyid,
		@assessmentid = @assessmentid

SELECT @usebaseline = case when assessmentstate in (N'C', N'S') then @usebaseline else usebaseline end,
		@rundate = case when assessmentstate in (N'C', N'S') then @rundate else assessmentdate end
	FROM assessment
	WHERE
		policyid = @policyid
		and assessmentid = @assessmentid

SELECT	* 
FROM	vwserversnapshot
WHERE	snapshotid IN (SELECT snapshotid FROM dbo.getsnapshotlist(@rundate, @usebaseline))
		AND registeredserverid IN (SELECT registeredserverid FROM #tmpservers)
		AND registeredserverid = case when @registeredserverid = 0 then registeredserverid else @registeredserverid end

DROP TABLE #tmpservers

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_getpolicyserver] TO [SQLSecureView]

GO