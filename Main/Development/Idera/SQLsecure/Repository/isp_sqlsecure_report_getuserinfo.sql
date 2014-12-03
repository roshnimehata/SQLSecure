SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO
 
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_report_getuserinfo]'))
drop procedure [dbo].[isp_sqlsecure_report_getuserinfo]
GO

CREATE procedure [dbo].[isp_sqlsecure_report_getuserinfo] 
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
   --              Get all user information from all servers and databases
   -- 	           

CREATE TABLE #tmpservers (registeredserverid int)
INSERT #tmpservers
    EXEC [dbo].[isp_sqlsecure_getpolicymemberlist]
         @policyid = @policyid  

SELECT	d.connectionname,
		a.name as loginname,
		dbo.getserverprincipaltypename(a.type) as logintype,
		c.databasename,
		b.name as username, 
		dbo.getyesnotext(b.isalias) as isalias,
		dbo.getyesnotext(b.hasaccess) as hasaccess, 
		b.defaultschemaname as defaultschema
FROM	serverprincipal a,
		databaseprincipal b,
		sqldatabase c,
		dbo.getsnapshotlist(@rundate, @usebaseline) d
WHERE	d.registeredserverid IN (SELECT registeredserverid FROM #tmpservers)
		AND UPPER(d.connectionname) LIKE UPPER(@serverName)
		AND a.snapshotid = d.snapshotid
		AND b.snapshotid = a.snapshotid 
		AND c.snapshotid = b.snapshotid 
		AND a.sid = b.usersid 
		AND b.dbid = c.dbid 
		AND a.type IN ('U', 'G', 'S')

ORDER BY connectionname, loginname, logintype, databasename, username, isalias, hasaccess, defaultschema

DROP TABLE #tmpservers

GO
 
GRANT EXECUTE ON [dbo].[isp_sqlsecure_report_getuserinfo] TO [SQLSecureView]
 
GO

