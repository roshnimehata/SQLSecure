SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwpolicychangelog]'))
drop view [dbo].[vwpolicychangelog]
GO

CREATE VIEW [dbo].[vwpolicychangelog] 
(
	policyid, 
	assessmentid, 
	policyname, 
	assessmentname, 
	policyassessmentname,  
	assessmentstate, 
	assessmentstatename, 
	changedate,
	changedby, 
	changedescription 
)
AS 
SELECT 
	a.policyid, 
	a.assessmentid, 
	b.policyname, 
	b.assessmentname, 
	b.policyassessmentname,  
	a.assessmentstate, 
	dbo.getassessmentstatename(a.assessmentstate), 
	a.changedate,
	a.changedby, 
	a.changedescription 
	FROM 
	[policychangelog] a
		inner join [vwpolicy] b on a.policyid = b.policyid and  a.assessmentid = b.assessmentid


GO

GRANT SELECT ON [dbo].[vwpolicychangelog] TO [SQLSecureView]

GO
