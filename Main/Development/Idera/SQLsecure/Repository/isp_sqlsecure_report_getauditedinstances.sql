SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_report_getauditedinstances]'))
drop procedure [dbo].[isp_sqlsecure_report_getauditedinstances]
GO

CREATE procedure [dbo].[isp_sqlsecure_report_getauditedinstances]
(
	@rundate datetime = null,		-- defaults to all
	@connectionname nvarchar(256),
	@usebaseline bit = 0,			-- defaults to false
	@policyid int = 1,				-- defaults to all
	@assessmentid int = null		-- default to policy settings for backward compatibility
)
AS
   -- <Idera SQLsecure version and copyright>
	--
	-- Description :
	--              Returns a list of all servers being audited at the selected rundate
	--				and optionally can just return the selected server
	-- 	           

CREATE TABLE #tmpservers (registeredserverid int)
INSERT #tmpservers
    EXEC [dbo].[isp_sqlsecure_getpolicymemberlist]
        @policyid = @policyid, 
		@assessmentid = @assessmentid

SELECT	a.connectionname,
		a.servername,
		a.instancename,
		version =	CASE 
						WHEN b.servertype = 'ADB' -- SQLsecure 3.1 (Anshul Aggarwal) - Add support for Azure SQL Database.
							THEN 'Microsoft SQL Azure v'
						WHEN SUBSTRING(a.version,1,3) = '08.' OR SUBSTRING(a.version,1,2) = '8.' 
							THEN 'SQL Server 2000 v'
						WHEN SUBSTRING(a.version,1,3) = '09.' OR SUBSTRING(a.version,1,2) = '9.'
							THEN 'SQL Server 2005 v'
						WHEN SUBSTRING(a.version,1,6) = '10.50.' -- check before the broader 2008 check
							THEN 'SQL Server 2008 R2 v'
						WHEN SUBSTRING(a.version,1,3) = '10.'
							THEN 'SQL Server 2008 v'
						WHEN SUBSTRING(a.version,1,3) = '11.'
							THEN 'SQL Server 2012 v'
						WHEN SUBSTRING(a.version,1,3) = '12.'
							THEN 'SQL Server 2014 v'
						WHEN SUBSTRING(a.version,1,3) = '13.'
							THEN 'SQL Server 2016 v'
						ELSE ''
					END + a.version,
		a.edition,
		a.authenticationmode,
		os = CASE WHEN b.servertype = 'ADB' THEN 'Azure' ELSE a.os END,
		loginauditmode = CASE WHEN b.servertype = 'ADB' THEN 'NA' ELSE a.loginauditmode END,
		a.enableproxyaccount,
		a.enablec2audittrace,
		a.crossdbownershipchaining,
		a.casesensitivemode,
		numwindowsuser = (SELECT COUNT(1) FROM serverprincipal WHERE snapshotid = a.snapshotid AND [type] = 'U' AND serveraccess = 'Y' AND serverdeny = 'N'),
		numwindowsgroup = (SELECT COUNT(1) FROM serverprincipal WHERE snapshotid = a.snapshotid AND [type] = 'G' AND serveraccess = 'Y' AND serverdeny = 'N'),
		numsqllogin = (SELECT COUNT(1) FROM serverprincipal WHERE snapshotid = a.snapshotid AND [type] = 'S' AND serveraccess = 'Y' AND serverdeny = 'N'),
		a.starttime AS snapshottime,
		numazureaduser = (SELECT COUNT(1) FROM serverprincipal WHERE snapshotid = a.snapshotid AND [type] = 'E' AND serveraccess = 'Y' AND serverdeny = 'N'),
		numazureadgroup = (SELECT COUNT(1) FROM serverprincipal WHERE snapshotid = a.snapshotid AND [type] = 'X' AND serveraccess = 'Y' AND serverdeny = 'N')
FROM	serversnapshot a INNER JOIN registeredserver b ON a.connectionname = b.connectionname 
WHERE	snapshotid IN (SELECT snapshotid FROM dbo.getsnapshotlist(@rundate, @usebaseline))
		AND a.registeredserverid IN (SELECT registeredserverid FROM #tmpservers)
		AND	(
			(UPPER(@connectionname) = 'ALL')
			OR 
			(UPPER(a.connectionname) = UPPER(@connectionname))
			)


DROP TABLE #tmpservers

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_report_getauditedinstances] TO [SQLSecureView]

GO
