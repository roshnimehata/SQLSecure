SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_report_orphanedlogins]'))
drop procedure [dbo].[isp_sqlsecure_report_orphanedlogins]
GO

CREATE procedure [dbo].[isp_sqlsecure_report_orphanedlogins]
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
   --              Checks all servers for orphaned windows account
   -- 	           

CREATE TABLE #tmpservers (registeredserverid int)
INSERT #tmpservers
    EXEC [dbo].[isp_sqlsecure_getpolicymemberlist]
         @policyid = @policyid  

SELECT	d.connectionname, a.name,
		[type] = CASE WHEN a.type = 'G' THEN 'Group' ELSE 'User' END,
		-- if there are any resolved accounts in the domain
		--		then this account is likely an orphan
		--		otherwise the entire domain is suspect
		[state] = CASE WHEN EXISTS (SELECT snapshotid FROM windowsaccount WHERE substring([name], 0, charindex('\', [name])) = substring(a.name, 0, charindex('\',a.name)) AND [state] = 'G') THEN 'Orphan' ELSE 'Suspect' END
FROM	serverprincipal a 
		INNER JOIN windowsaccount b ON a.snapshotid = b.snapshotid AND a.sid = b.sid
		LEFT JOIN ancillarywindowsgroup c ON a.snapshotid = c.snapshotid AND a.name = c.windowsgroupname
		INNER JOIN serversnapshot d ON a.snapshotid = d.snapshotid
WHERE	a.snapshotid IN (SELECT snapshotid FROM dbo.getsnapshotlist(@rundate, @usebaseline))
		AND d.registeredserverid IN (SELECT registeredserverid FROM #tmpservers)
		AND UPPER(d.connectionname) LIKE UPPER(@serverName)
		AND a.type IN ('G', 'U')	-- Principal type is Windows Group or User
		AND b.state = 'S'			-- State is suspect
		AND c.windowsgroupname IS NULL	-- Account is not OS controlled well-known

ORDER BY d.connectionname, a.name

DROP TABLE #tmpservers

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_report_orphanedlogins] TO [SQLSecureView]

GO