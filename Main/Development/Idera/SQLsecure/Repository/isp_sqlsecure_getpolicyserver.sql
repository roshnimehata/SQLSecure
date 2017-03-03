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

--SQLsecure 3.1 (Tushar)--Returning server type to support azure sql databases.
SELECT	vwss.[connectionname]
      ,vwss.[servername]
      ,vwss.[instancename]
      ,vwss.[authenticationmode]
      ,vwss.[os]
      ,vwss.[version]
      ,vwss.[edition]
      ,vwss.[status]
      ,vwss.[starttime]
      ,vwss.[endtime]
      ,vwss.[automated]
      ,vwss.[numobject]
      ,vwss.[numpermission]
      ,vwss.[numlogin]
      ,vwss.[numwindowsgroupmember]
      ,vwss.[baseline]
      ,vwss.[baselinecomment]
      ,vwss.[snapshotcomment]
      ,vwss.[loginauditmode]
      ,vwss.[enableproxyaccount]
      ,vwss.[enablec2audittrace]
      ,vwss.[crossdbownershipchaining]
      ,vwss.[casesensitivemode]
      ,vwss.[hashkey]
      ,vwss.[snapshotid]
      ,vwss.[registeredserverid]
      ,vwss.[collectorversion]
      ,vwss.[allowsystemtableupdates]
      ,vwss.[remoteadminconnectionsenabled]
      ,vwss.[remoteaccessenabled]
      ,vwss.[scanforstartupprocsenabled]
      ,vwss.[sqlmailxpsenabled]
      ,vwss.[databasemailxpsenabled]
      ,vwss.[oleautomationproceduresenabled]
      ,vwss.[webassistantproceduresenabled]
      ,vwss.[xp_cmdshellenabled]
      ,vwss.[agentmailprofile]
      ,vwss.[hideinstance]
      ,vwss.[agentsysadminonly]
      ,vwss.[serverisdomaincontroller]
      ,vwss.[replicationenabled]
      ,vwss.[sapasswordempty]
      ,vwss.[systemdrive]
      ,vwss.[adhocdistributedqueriesenabled]
      ,vwss.[weakpassworddectectionenabled] , r.servertype
FROM	vwserversnapshot vwss JOIN registeredserver r on r.connectionname = vwss.connectionname
WHERE	snapshotid IN (SELECT snapshotid FROM dbo.getsnapshotlist(@rundate, @usebaseline))
		AND vwss.registeredserverid IN (SELECT registeredserverid FROM #tmpservers)
		AND vwss.registeredserverid = case when @registeredserverid = 0 then vwss.registeredserverid else @registeredserverid end

DROP TABLE #tmpservers

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_getpolicyserver] TO [SQLSecureView]

GO