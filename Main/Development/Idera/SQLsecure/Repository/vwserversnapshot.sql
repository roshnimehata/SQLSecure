SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwserversnapshot]'))
drop view [dbo].[vwserversnapshot]
GO

CREATE VIEW [dbo].[vwserversnapshot] 
AS SELECT 
	a.[connectionname],
	a.[servername],
	a.[instancename],
	a.[authenticationmode],
	a.[os],
	a.[version],
	a.[edition],
	a.[status],
	a.[starttime],
	a.[endtime],
	a.[automated],
	a.[numobject],
	a.[numpermission],
	a.[numlogin],
	a.[numwindowsgroupmember],	
	a.[baseline],
	a.[baselinecomment],
	a.[snapshotcomment],
	a.[loginauditmode],
	a.[enableproxyaccount],
	a.[enablec2audittrace],
	a.[crossdbownershipchaining],
	a.[casesensitivemode],
	a.[hashkey],
	a.[snapshotid],
	a.[registeredserverid],
	isnull(a.[collectorversion], '1.2.0.0') as collectorversion,
	a.[allowsystemtableupdates],
	a.[remoteadminconnectionsenabled],
	a.[remoteaccessenabled],
	a.[scanforstartupprocsenabled],
	a.[sqlmailxpsenabled],
	a.[databasemailxpsenabled],
	a.[oleautomationproceduresenabled],
	a.[webassistantproceduresenabled],
	a.[xp_cmdshellenabled],
	a.[agentmailprofile],
	a.[hideinstance],
	a.[agentsysadminonly],
	a.[serverisdomaincontroller],
	a.[replicationenabled],
	a.[sapasswordempty],
	a.[systemdrive],
	a.[adhocdistributedqueriesenabled],
	a.[isweakpassworddetectionenabled] as weakpassworddectectionenabled
 FROM [serversnapshot] a
  INNER JOIN [registeredserver] b ON a.[connectionname] = b.[connectionname]
 
 GO
 
 GRANT SELECT ON [dbo].[vwserversnapshot] TO [SQLSecureView]
 
 GO
