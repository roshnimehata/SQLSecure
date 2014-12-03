SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwservernotification]'))
drop view [dbo].[vwservernotification]
GO

CREATE VIEW [dbo].[vwservernotification] 
(
	notificationproviderid,
	registeredserverid,
	connectionname,
	providername, 
	providertype, 
	snapshotstatus,
	policymetricseverity,
	recipients
) 
AS
SELECT 
	a.notificationproviderid,
	a.registeredserverid,
	c.connectionname,
	b.providername, 
	b.providertype, 
	a.snapshotstatus,
	a.policymetricseverity,
	a.recipients
FROM 
	[registeredservernotification] a
	INNER JOIN [notificationprovider] b ON a.notificationproviderid = b.notificationproviderid
	INNER JOIN [registeredserver] c ON a.registeredserverid = c.registeredserverid

GO

GRANT SELECT ON [dbo].[vwservernotification] TO [SQLSecureView]

GO
