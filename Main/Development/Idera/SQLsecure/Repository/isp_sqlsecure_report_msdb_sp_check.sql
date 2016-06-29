SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_report_msdb_sp_check]'))
drop procedure [dbo].[isp_sqlsecure_report_msdb_sp_check]
GO

CREATE procedure [dbo].[isp_sqlsecure_report_msdb_sp_check] @rundate datetime = null
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Checks all servers msdb for direct/explicit permission on sp
   -- 	           

	select e.connectionname, b.databasename, username=a.name, objectname=c.name, d.permission, d.isgrant, d.isgrantwith
		from 
			databaseprincipal a,
			sqldatabase b,
			databaseobject c,
			databaseobjectpermission d,
			dbo.getsnapshotlist(@rundate, 0) e
		where a.snapshotid = e.snapshotid and 
			a.snapshotid = b.snapshotid and
			a.dbid = b.dbid and
			b.databasename in ('msdb') and
			c.snapshotid = b.snapshotid and
			c.dbid = b.dbid and
			d.grantee = a.uid and 
			c.objectid = d.objectid and
			d.snapshotid = c.snapshotid and
			d.dbid = c.dbid and
			c.type = 'P' and
			(d.isgrant = 'Y' or d.isgrantwith = 'Y') and
			lower(a.name) in ('public', 'guest')
		order by connectionname, databasename, username, objectname, permission

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_report_msdb_sp_check] TO [SQLSecureView]

GO
