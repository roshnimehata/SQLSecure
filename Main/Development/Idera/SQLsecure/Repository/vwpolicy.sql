SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwpolicy]'))
drop view [dbo].[vwpolicy]
GO

CREATE VIEW [dbo].[vwpolicy] 
(
	policyid,
	assessmentid,
	assessmentstate,
	assessmentstatename,
	policyname, 
	policydescription, 
	issystempolicy, 
	isdynamic, 
	dynamicselection,
	assessmentname,
	assessmentdescription,
	assessmentnotes,
	assessmentdate,
	usebaseline,
	interviewname,
	interviewtext,
	policyassessmentname,
	metriccounthigh,
	metriccountmedium,
	metriccountlow,
	findingcounthigh,
	findingcountmedium,
	findingcountlow,
	findingcounthighexplained,
	findingcountmediumexplained,
	findingcountlowexplained
) 
AS
SELECT 
	a.policyid, 
	b.assessmentid, 
	b.assessmentstate, 
	dbo.getassessmentstatename(b.assessmentstate), 
	a.policyname, 
	a.policydescription, 
	a.issystempolicy, 
	b.isdynamic, 
	b.dynamicselection,
	b.assessmentname,
	b.assessmentdescription,
	b.assessmentnotes,
	b.assessmentdate,
	b.usebaseline,
	c.interviewname,
	c.interviewtext,
	dbo.getassessmentname(a.policyid, b.assessmentid),
	(select count(*) from [policymetric] where policyid = a.policyid and assessmentid = b.assessmentid and isenabled = 1 and severity = 3),
	(select count(*) from [policymetric] where policyid = a.policyid and assessmentid = b.assessmentid and isenabled = 1 and severity = 2),
	(select count(*) from [policymetric] where policyid = a.policyid and assessmentid = b.assessmentid and isenabled = 1 and severity = 1),
	(select count(distinct aa.metricid) from [policyassessment] aa left join [policyassessmentnotes] ab on aa.policyid = ab.policyid and aa.assessmentid = ab.assessmentid and aa.metricid = ab.metricid and aa.snapshotid = ab.snapshotid where aa.policyid = a.policyid and aa.assessmentid = b.assessmentid and aa.metricseveritycode = 3 and aa.severitycode > 0 and isnull(ab.isexplained, 0) = 0),
	(select count(distinct aa.metricid) from [policyassessment] aa left join [policyassessmentnotes] ab on aa.policyid = ab.policyid and aa.assessmentid = ab.assessmentid and aa.metricid = ab.metricid and aa.snapshotid = ab.snapshotid where aa.policyid = a.policyid and aa.assessmentid = b.assessmentid and aa.metricseveritycode = 2 and aa.severitycode > 0 and isnull(ab.isexplained, 0) = 0),
	(select count(distinct aa.metricid) from [policyassessment] aa left join [policyassessmentnotes] ab on aa.policyid = ab.policyid and aa.assessmentid = ab.assessmentid and aa.metricid = ab.metricid and aa.snapshotid = ab.snapshotid where aa.policyid = a.policyid and aa.assessmentid = b.assessmentid and aa.metricseveritycode = 1 and aa.severitycode > 0 and isnull(ab.isexplained, 0) = 0),
	(select count(distinct aa.metricid) from [policyassessment] aa left join [policyassessmentnotes] ab on aa.policyid = ab.policyid and aa.assessmentid = ab.assessmentid and aa.metricid = ab.metricid and aa.snapshotid = ab.snapshotid where aa.policyid = a.policyid and aa.assessmentid = b.assessmentid and aa.metricseveritycode = 3 and aa.severitycode > 0 and isnull(ab.isexplained, 0) = 1),
	(select count(distinct aa.metricid) from [policyassessment] aa left join [policyassessmentnotes] ab on aa.policyid = ab.policyid and aa.assessmentid = ab.assessmentid and aa.metricid = ab.metricid and aa.snapshotid = ab.snapshotid where aa.policyid = a.policyid and aa.assessmentid = b.assessmentid and aa.metricseveritycode = 2 and aa.severitycode > 0 and isnull(ab.isexplained, 0) = 1),
	(select count(distinct aa.metricid) from [policyassessment] aa left join [policyassessmentnotes] ab on aa.policyid = ab.policyid and aa.assessmentid = ab.assessmentid and aa.metricid = ab.metricid and aa.snapshotid = ab.snapshotid where aa.policyid = a.policyid and aa.assessmentid = b.assessmentid and aa.metricseveritycode = 1 and aa.severitycode > 0 and isnull(ab.isexplained, 0) = 1)
FROM 
	[policy] a
	LEFT JOIN [assessment] b on a.policyid = b.policyid
	LEFT JOIN [policyinterview] c on a.policyid = c.policyid and b.assessmentid = c.assessmentid
WHERE 
	a.policyid > 0

GO

GRANT SELECT ON [dbo].[vwpolicy] TO [SQLSecureView]

GO
