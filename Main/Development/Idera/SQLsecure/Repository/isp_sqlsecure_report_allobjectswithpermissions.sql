SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO
 
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_report_allobjectswithpermissions]'))
drop procedure [dbo].[isp_sqlsecure_report_allobjectswithpermissions]
GO

CREATE procedure [dbo].[isp_sqlsecure_report_allobjectswithpermissions]
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
   --              Display all objects with permissions in databases for all servers
   --              Notes:
   --               will not show any server level permissions that affect databases
   --               will not differentiate objects with same name in different schemas
   --               will not designate the table which owns a column 

	
CREATE TABLE #tmpservers (registeredserverid int)
INSERT #tmpservers
    EXEC [dbo].[isp_sqlsecure_getpolicymemberlist]
         @policyid = @policyid    

SELECT	e.connectionname AS 'Server',
		d.databasename AS 'DB',
		a.objectname AS 'Object',
		c.objecttypename AS 'Type',
		b.name AS 'User',
		dbo.getdatabaseprincipaltypename(b.type) AS 'User Type',
		dbo.getaccesstype(a.isdeny, a.isgrantwith, a.isgrant, 'N') AS 'Access Type',
		a.permission AS Privilege
FROM	vwdatabaseobjectpermission AS a,
		databaseprincipal AS b,
		objecttype AS c,
		sqldatabase AS d,
		dbo.getsnapshotlist(@rundate, @usebaseline) e
WHERE	e.registeredserverid IN (SELECT registeredserverid FROM #tmpservers)
		AND UPPER(e.connectionname) LIKE UPPER(@serverName)
		AND a.snapshotid = e.snapshotid
		AND a.snapshotid = b.snapshotid
		AND a.dbid = b.dbid
		AND a.grantee = b.uid
		AND a.objecttype = c.objecttype
		AND d.snapshotid = a.snapshotid
		AND d.dbid = a.dbid

DROP TABLE #tmpservers

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_report_allobjectswithpermissions] TO [SQLSecureView]

GO

