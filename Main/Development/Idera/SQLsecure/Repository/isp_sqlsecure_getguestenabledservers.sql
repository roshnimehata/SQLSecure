SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_getguestenabledservers]'))
drop procedure [dbo].[isp_sqlsecure_getguestenabledservers]
GO

CREATE procedure [dbo].[isp_sqlsecure_getguestenabledservers] (@connectionname nvarchar(400)='ALL')
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get all the databases that have guest users enabled.


	if (@connectionname = 'ALL' or @connectionname ='')
		select distinct a.connectionname, b.databasename, b.owner from serversnapshot a, sqldatabase b where a.snapshotid = b.snapshotid
	else
		select distinct a.connectionname, b.databasename, b.owner from serversnapshot a, sqldatabase b where a.snapshotid = b.snapshotid and UPPER(a.connectionname) = @connectionname

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_getguestenabledservers] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

