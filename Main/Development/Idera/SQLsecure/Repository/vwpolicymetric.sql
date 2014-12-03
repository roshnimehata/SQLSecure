SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwpolicymetric]'))
drop view [dbo].[vwpolicymetric]
GO

CREATE VIEW [dbo].[vwpolicymetric] 
(
	policyid,
	assessmentid,
	policyname,
	policydescription,
	assessmentstate,
	assessmentstatename,
	metricid,
	metrictype,
	metricname,
	metricdescription,
	isuserentered,
	ismultiselect,
	validvalues,
	valuedescription,
	isenabled,
	reportkey,
	reporttext,
	severity,
	severityname,
	severityvalues)
AS
SELECT
	a.policyid,
	a.assessmentid,
	dbo.[getassessmentname](a.policyid, a.assessmentid),
	case when d.assessmentstate = N'S' then b.policydescription else isnull(d.assessmentdescription, b.policydescription) end,
	d.assessmentstate,
	dbo.getassessmentstatename(d.assessmentstate), 
	a.metricid,
	c.metrictype,
	c.metricname,
	c.metricdescription,
	c.isuserentered,
	c.ismultiselect,
	c.validvalues,
	c.valuedescription,
	a.isenabled,
	a.reportkey,
	a.reporttext,
	a.severity,
	dbo.getpolicyseverityname(a.severity),
	a.severityvalues
FROM 
	[policymetric] a,
	[policy] b,
	[metric] c,
	[assessment] d
WHERE 
	a.policyid  = b.policyid
	and a.metricid = c.metricid
	and a.policyid = d.policyid
	and a.assessmentid = d.assessmentid

GO

GRANT SELECT ON [dbo].[vwpolicymetric] TO [SQLSecureView]

GO
