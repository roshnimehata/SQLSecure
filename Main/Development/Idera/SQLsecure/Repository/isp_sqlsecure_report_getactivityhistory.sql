SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_report_getactivityhistory]'))
drop procedure [dbo].[isp_sqlsecure_report_getactivityhistory]
GO

CREATE PROCEDURE [dbo].[isp_sqlsecure_report_getactivityhistory]
(
	@server varchar(255),
	@status varchar(100),
	@startdate varchar(50),
	@enddate varchar(50),
	@login varchar(255),
	@policyid int = 1		--defaults to all
)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Returns matching events for the Activity History Report
   -- 

CREATE TABLE #tmpservers (registeredserverid int)
INSERT #tmpservers
    EXEC [dbo].[isp_sqlsecure_getpolicymemberlist]
         @policyid = @policyid               

SELECT	activitytype,
		CONVERT(nvarchar(10), eventtimestamp, 101) AS datestamp,
		CONVERT(nvarchar(8), eventtimestamp, 108) AS [timestamp],
		connectionname,
		serverlogin,
		[description]
FROM	SQLsecure.dbo.applicationactivity
WHERE	connectionname IN (SELECT r.connectionname FROM registeredserver r, #tmpservers t WHERE r.registeredserverid = t.registeredserverid)
		AND connectionname LIKE @server
		AND activitytype LIKE @status 
		AND eventtimestamp BETWEEN CONVERT(datetime, @startdate) AND CONVERT(datetime, @enddate) 
		AND UPPER(serverlogin) LIKE UPPER(@login)

DROP TABLE #tmpservers

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_report_getactivityhistory] TO [SQLSecureView]

GO
