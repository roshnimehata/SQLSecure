SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO
 
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_report_getdatabases]'))
drop procedure [dbo].[isp_sqlsecure_report_getdatabases]
GO

CREATE procedure [dbo].[isp_sqlsecure_report_getdatabases]
(
	@connectionname nvarchar(400)
)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get all databases for a particular server.

SELECT databasename
FROM sqldatabase
WHERE snapshotid = (
	SELECT lastcollectionsnapshotid
	FROM registeredserver
	WHERE upper(connectionname) = upper(@connectionname))


GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_report_getdatabases] TO [SQLSecureView]

GO

