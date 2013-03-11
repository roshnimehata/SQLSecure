SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_getdatabaseobjectinfo]'))
drop procedure [dbo].[isp_sqlsecure_getdatabaseobjectinfo]
GO

CREATE procedure [dbo].[isp_sqlsecure_getdatabaseobjectinfo] (@snapshotid int, @dbid int, @classid int, @parentobjectid int, @objectid int)
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Return database object information identified by ids.
   -- 	           
	
SELECT 
	dbobj.name as name,
	dbobj.type as type,
	dbobj.owner as ownerid,
	dbuser.name AS owner, 
	dbobj.schemaid AS schemaid,
	dbschema.schemaname AS schemaname, 
	dbuser2.name AS schemaowner 
FROM databaseobject AS dbobj
    LEFT OUTER JOIN databaseprincipal AS dbuser ON (dbuser.snapshotid = dbobj.snapshotid AND dbuser.dbid = dbobj.dbid AND dbobj.owner = dbuser.uid)
    LEFT OUTER JOIN databaseschema AS dbschema ON (dbschema.snapshotid = dbobj.snapshotid AND dbschema.dbid = dbobj.dbid AND dbobj.schemaid = dbschema.schemaid)
    LEFT OUTER JOIN databaseprincipal AS dbuser2 ON (dbuser2.snapshotid = dbobj.snapshotid AND dbuser2.dbid = dbobj.dbid AND dbobj.schemaid = dbschema.schemaid AND dbuser2.uid = dbschema.uid)
WHERE 
    dbobj.snapshotid = @snapshotid
    AND dbobj.dbid = @dbid 
    AND dbobj.classid = @classid 
    AND dbobj.parentobjectid = @parentobjectid 
    AND dbobj.objectid = @objectid

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_getdatabaseobjectinfo]  TO [SQLSecureView]

GO
