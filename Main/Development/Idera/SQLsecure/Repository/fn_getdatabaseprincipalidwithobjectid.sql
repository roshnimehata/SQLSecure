SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getdatabaseprincipalidwithobjectid]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getdatabaseprincipalidwithobjectid]
GO


CREATE  function [dbo].[getdatabaseprincipalidwithobjectid] (@snapshotid int, @dbid int, @objectid int) returns int
begin
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the principalid associated with the objectid in database with dbid.
	declare @id int
	
	select @id = c.uid from databaseobject a, databaseschema b, databaseprincipal c where a.snapshotid = @snapshotid and a.dbid = @dbid and a.objectid = @objectid and b.snapshotid = a.snapshotid and b.dbid = a.dbid and b.schemaid = a.schemaid and c.snapshotid = b.snapshotid and c.dbid = b.dbid and c.uid = b.uid and UPPER(a.type) = 'U'
	
	return @id

end

GO

GRANT EXECUTE ON [dbo].[getdatabaseprincipalidwithobjectid] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

