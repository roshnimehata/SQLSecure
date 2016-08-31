SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwpolicyassessment]'))
drop view [dbo].[vwpolicyassessment]
GO

CREATE VIEW [dbo].[vwpolicyassessment] 
(
	policyid,
	assessmentid,
	policyname,
	policydescription,
	assessmentstate,
	assessmentstatename,
	snapshotid,
	registeredserverid,
	connectionname,
	collectiontime,
	metricid,
	metricname,
	metrictype,
	metricseveritycode,
	metricseverity,
	metricdescription,
	metricreportkey,
	metricreporttext,
	severitycode,
	severity,
	currentvalue,
	thresholdvalue)
AS
SELECT
	a.policyid,
	a.assessmentid,
	dbo.[getassessmentname](a.policyid, a.assessmentid),
	case when c.assessmentstate = N'S' then b.policydescription else isnull(c.assessmentdescription, b.policydescription) end,
	c.assessmentstate,
	dbo.getassessmentstatename(c.assessmentstate), 
	a.snapshotid,
	a.registeredserverid,
	a.connectionname,
	a.collectiontime,
	a.metricid,
	a.metricname,
	a.metrictype,
	a.metricseveritycode,
	a.metricseverity,
	a.metricdescription,
	a.metricreportkey,
	a.metricreporttext,
	a.severitycode,
	a.severity,
	a.currentvalue,
	a.thresholdvalue
FROM 
	[policyassessment] a,
	[policy] b,
	[assessment] c
WHERE 
	a.policyid  = b.policyid
	and a.policyid = c.policyid
	and a.assessmentid = c.assessmentid

GO

GRANT SELECT ON [dbo].[vwpolicyassessment] TO [SQLSecureView]

GO
