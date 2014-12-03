SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getdatabaseprincipalnamewithobjectid]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getdatabaseprincipalnamewithobjectid]
GO


CREATE  function [dbo].[getdatabaseprincipalnamewithobjectid] (@snapshotid int, @dbid int, @objectid int) returns nvarchar(128)
begin
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the principal name associated with the objectid in database with dbid.
	declare @name nvarchar(128)
	
	select @name = c.name from databaseobject a, databaseschema b, databaseprincipal c where a.snapshotid = @snapshotid and a.dbid = @dbid and a.objectid = @objectid and b.snapshotid = a.snapshotid and b.dbid = a.dbid and b.schemaid = a.schemaid and c.snapshotid = b.snapshotid and c.dbid = b.dbid and c.uid = b.uid and UPPER(a.type) = 'U'
	
	return @name

end

GO

GRANT EXECUTE ON [dbo].[getdatabaseprincipalnamewithobjectid] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

