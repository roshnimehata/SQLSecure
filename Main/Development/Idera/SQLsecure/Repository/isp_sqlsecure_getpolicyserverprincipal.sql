SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_getpolicyserverprincipal]'))
drop procedure [dbo].[isp_sqlsecure_getpolicyserverprincipal]
GO

CREATE procedure [dbo].[isp_sqlsecure_getpolicyserverprincipal] 
(
	@policyid int, 
	@assessmentid int = null,		-- default to policy settings for backward compatibility
	@usebaseline bit, 
	@rundate datetime=null 
)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Return a table with the server principals of all policy members for the selected run date
   --
   -- Returns:  a table with all the columns of view vwserverprincipal

SELECT @assessmentid = isnull(@assessmentid,[dbo].[getdefaultassessmentid](@policyid))
CREATE TABLE #tmpservers (registeredserverid int)
INSERT #tmpservers
	EXEC [dbo].[isp_sqlsecure_getpolicymemberlist]
		@policyid = @policyid,
		@assessmentid = @assessmentid

SELECT	a.snapshotid,
		a.[name],
		a.[type],
		dbo.getserverprincipaltypename(a.[type]) as typename,
		a.[sid],
		a.principalid,
		b.connectionname,
		a.serveraccess,
		a.serverdeny,
		a.[disabled],
		a.isexpirationchecked,
		a.ispolicychecked,
		passwordstatus = [dbo].[getpasswordstatusdescription](a.passwordstatus),
		a.defaultdatabase,
		a.defaultlanguage
FROM	serverprincipal a
		INNER JOIN serversnapshot b ON a.snapshotid = b.snapshotid
WHERE	a.snapshotid IN	(
						SELECT	snapshotid
						FROM	dbo.getsnapshotlist(@rundate, @usebaseline)
						WHERE	registeredserverid IN (SELECT registeredserverid FROM #tmpservers)
						)

DROP TABLE #tmpservers

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_getpolicyserverprincipal] TO [SQLSecureView]

GO