SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO
 
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_report_serverroles]'))
drop procedure [dbo].[isp_sqlsecure_report_serverroles]
GO

CREATE procedure [dbo].[isp_sqlsecure_report_serverroles] 
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
   --              List all direct members of Server Roles on all Servers
   --   

CREATE TABLE #tmpservers (registeredserverid int)
INSERT #tmpservers
    EXEC [dbo].[isp_sqlsecure_getpolicymemberlist]
         @policyid = @policyid  

SELECT	s.connectionname as [Server],
		r.name as [Role],
		m.name as Member,
		dbo.getserverprincipaltypename(m.type) as [Type],
		dbo.getyesnotext(m.serveraccess) as serveraccess,
		dbo.getyesnotext(m.serverdeny) as serverdeny,
		dbo.getyesnotext(m.disabled) as [disabled]
FROM	serversnapshot as s,
		serverprincipal as r,
		serverrolemember as rm,
		serverprincipal as m
WHERE	s.snapshotid IN (SELECT snapshotid FROM dbo.getsnapshotlist(@rundate, @usebaseline) WHERE registeredserverid IN (SELECT registeredserverid FROM #tmpservers))
		AND UPPER(s.connectionname) LIKE UPPER(@serverName)
		AND r.snapshotid = s.snapshotid
		AND r.type IN ('R', 'S')
		AND rm.snapshotid = r.snapshotid
		AND rm.principalid = r.principalid
		AND m.snapshotid = rm.snapshotid
		AND m.principalid = rm.memberprincipalid

ORDER BY s.connectionname, r.name, m.type, m.name

DROP TABLE #tmpservers

GO
 
GRANT EXECUTE ON [dbo].[isp_sqlsecure_report_serverroles] TO [SQLSecureView]

GO