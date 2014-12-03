SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_report_getsqlsecurelogins]'))
drop procedure [dbo].[isp_sqlsecure_report_getsqlsecurelogins]
GO

CREATE PROC [dbo].[isp_sqlsecure_report_getsqlsecurelogins]
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Gets all Windows users and their access to SQLsecure.

CREATE TABLE #tmplogins (sid varbinary(85), loginname nvarchar(500), logintype nvarchar(32), serveraccess nvarchar(16), applicationpermission nvarchar(16))

INSERT INTO #tmplogins(sid, loginname,logintype,serveraccess,applicationpermission)
EXEC isp_sqlsecure_getaccessinfo

SELECT loginname, logintype, serveraccess, applicationpermission
FROM #tmplogins
WHERE UPPER(logintype) LIKE UPPER('Windows%')

DROP TABLE #tmplogins

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_report_getsqlsecurelogins] TO [SQLSecureView]
GO

