SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwnotificationprovider]'))
drop view [dbo].[vwnotificationprovider]
GO

CREATE VIEW [dbo].[vwnotificationprovider] 
(
	notificationproviderid,
	providername, 
	providertype, 
	servername, 
	port,
	[timeout], 
	requiresauthentication, 
	username, 
	[password], 
	sendername, 
	senderemail
) 
AS
SELECT 
	notificationproviderid,
	providername, 
	providertype, 
	servername, 
	port,
	[timeout], 
	requiresauthentication, 
	username, 
	[password], 
	sendername, 
	senderemail
FROM 
	[notificationprovider]

GO

GRANT SELECT ON [dbo].[vwnotificationprovider] TO [SQLSecureView]

GO
