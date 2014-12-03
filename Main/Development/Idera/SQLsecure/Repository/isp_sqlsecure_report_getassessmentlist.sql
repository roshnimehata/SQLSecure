SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_report_getassessmentlist]'))
drop procedure [dbo].[isp_sqlsecure_report_getassessmentlist]
GO

CREATE procedure [dbo].[isp_sqlsecure_report_getassessmentlist]
(
	@policyid int
)
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Return a list of the current and saved assessments belonging to the policyid for selection by reports

SELECT	policyid, assessmentid, assessmentname
	FROM vwpolicy WHERE policyid = @policyid and assessmentstate <> N'S'

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_report_getassessmentlist] TO [SQLSecureView]

GO

