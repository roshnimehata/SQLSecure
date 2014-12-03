SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwapplicationactivity]'))
drop view [dbo].[vwapplicationactivity]
GO

CREATE VIEW [dbo].[vwapplicationactivity] 
AS 
SELECT 
eventid, 
eventtimestamp, 
activitytype, 
connectionname,
applicationsource, 
serverlogin, 
eventcode, 
category, 
description 
FROM 
[applicationactivity]

GO

GRANT SELECT ON [dbo].[vwapplicationactivity] TO [SQLSecureView]

GO
