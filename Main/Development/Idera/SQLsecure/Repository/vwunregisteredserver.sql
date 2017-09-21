SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwunregisteredserver]'))
drop view [dbo].[vwunregisteredserver]
GO

CREATE VIEW [dbo].[vwunregisteredserver] 
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
sapasswordempty,
auditfoldersstring,
servertype
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
sapasswordempty,
auditfoldersstring,
servertype
FROM 
[unregisteredserver]

GO

GRANT SELECT ON [dbo].[vwunregisteredserver] TO [SQLSecureView]

GO
