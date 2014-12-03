SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwsnapshotfilterrules]'))
drop view [dbo].[vwsnapshotfilterrules]
GO

CREATE VIEW [dbo].[vwsnapshotfilterrules] 
AS 
SELECT DISTINCT 
a.snapshotid,
a.filterruleheaderid, 
a.rulename, 
a.description,
a.createdby, 
a.createdtm, 
a.lastmodifiedtm, 
a.lastmodifiedby, 
b.filterruleid, 
b.class, 
b.scope, 
b.matchstring, 
c.objectvalue as classname 
FROM  
[serverfilterruleheader] a, 
[serverfilterrule] b, 
[filterruleclass] c 
WHERE 
a.filterruleheaderid = b.filterruleheaderid 
AND b.class = c.objectclass 
AND a.snapshotid = b.snapshotid 

GO

GRANT SELECT ON [dbo].[vwsnapshotfilterrules] TO [SQLSecureView]

GO
