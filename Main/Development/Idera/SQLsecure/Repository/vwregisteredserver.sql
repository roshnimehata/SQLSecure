SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwregisteredserver]'))
drop view [dbo].[vwregisteredserver]
GO

CREATE VIEW [dbo].[vwregisteredserver] 
(
lastcollectionsnapshotid,
connectionname, 
connectionport,
servername, 
instancename, 
authenticationmode, 
sqlserverlogin, 
sqlserverpassword, 
sqlserverauthtype, 
os, 
version, 
edition, 
loginauditmode, 
enableproxyaccount, 
enablec2audittrace, 
crossdbownershipchaining, 
casesensitivemode,
lastcollectiontm, 
currentcollectiontm, 
currentcollectionstatus, 
serverlogin, 
serverpassword,
jobid,
snapshotretentionperiod,
registeredserverid,
serverisdomaincontroller,
replicationenabled,
sapasswordempty
) 
AS SELECT 
lastcollectionsnapshotid,
connectionname, 
connectionport,
servername, 
instancename, 
authenticationmode, 
sqlserverlogin, 
sqlserverpassword, 
sqlserverauthtype, 
os, 
version, 
edition, 
loginauditmode, 
enableproxyaccount, 
enablec2audittrace, 
crossdbownershipchaining, 
casesensitivemode,
lastcollectiontm, 
currentcollectiontm, 
currentcollectionstatus, 
serverlogin, 
serverpassword,
jobid,
snapshotretentionperiod,
registeredserverid,
serverisdomaincontroller,
replicationenabled,
sapasswordempty
FROM 
[registeredserver]

GO

GRANT SELECT ON [dbo].[vwregisteredserver] TO [SQLSecureView]

GO
