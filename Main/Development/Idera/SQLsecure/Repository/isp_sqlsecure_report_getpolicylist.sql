SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_report_getpolicylist]'))
drop procedure [dbo].[isp_sqlsecure_report_getpolicylist]
GO

CREATE procedure [dbo].[isp_sqlsecure_report_getpolicylist]
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Return a list of policies for selection by reports

SELECT	policyid, assessmentid, policyname 
	FROM vwpolicy WHERE assessmentstate = N'S'

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_report_getpolicylist] TO [SQLSecureView]

GO

