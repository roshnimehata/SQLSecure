SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_report_getauditedserverlistnoall]'))
drop procedure [dbo].[isp_sqlsecure_report_getauditedserverlistnoall]
GO

CREATE PROC [dbo].[isp_sqlsecure_report_getauditedserverlistnoall]
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get all available servers/connections for Reporting Services - doesn't bring back 
   --              "ALL" value like isp_sqlsecure_report_getauditedserverlist

	SELECT connectionname AS [server], registeredserverid FROM vwregisteredserver

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_report_getauditedserverlistnoall] TO [SQLSecureView]
GO

