SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_report_getauditfilters]'))
drop procedure [dbo].[isp_sqlsecure_report_getauditfilters]
GO

CREATE PROCEDURE [dbo].[isp_sqlsecure_report_getauditfilters]
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
   --              Gets audit filters setup for servers.
   -- 	           

CREATE TABLE #tmpservers (registeredserverid int)
INSERT #tmpservers
    EXEC [dbo].[isp_sqlsecure_getpolicymemberlist]
         @policyid = @policyid 

SELECT	servername = s.connectionname,
		rulename = sfheader.rulename,
		ruledescription = sfheader.description,
		modifiedby = sfheader.lastmodifiedby,
		modifiedon = sfheader.lastmodifiedtm,
		[type] = fclass.objectvalue,
		scope =	CASE
					WHEN sfrule.scope = 'A' THEN 'User and System'
					WHEN sfrule.scope = 'S' THEN 'System'
					WHEN sfrule.scope = 'U' THEN 'User'
				END,
		namematchstring =	CASE
								WHEN sfrule.matchstring = '' THEN '*'
								WHEN sfrule.matchstring IS NULL THEN '*'
								ELSE sfrule.matchstring
							END
FROM	serverfilterrule AS sfrule,
		serverfilterruleheader AS sfheader,
		filterruleclass AS fclass,
		dbo.getsnapshotlist(@rundate, @usebaseline) AS s
WHERE	s.registeredserverid IN (SELECT registeredserverid FROM #tmpservers)
		AND UPPER(s.connectionname) LIKE UPPER(@serverName)
		AND sfheader.snapshotid = s.snapshotid
		AND sfheader.filterruleheaderid = sfrule.filterruleheaderid
		AND sfheader.snapshotid = sfrule.snapshotid
		AND sfrule.class = fclass.objectclass

DROP TABLE #tmpservers

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_report_getauditfilters] TO [SQLSecureView]

GO

