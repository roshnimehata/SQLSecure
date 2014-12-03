SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getschemanamewithobjectid]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getschemanamewithobjectid]
GO


CREATE  function [dbo].[getschemanamewithobjectid] (@snapshotid int, @dbid int, @objectid int) returns nvarchar(128)
begin
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the schema name associated with a database object.
	declare @name nvarchar(128)
	
	select @name = schemaname from databaseobject a, databaseschema b where a.snapshotid = @snapshotid and a.dbid = @dbid and a.objectid = @objectid and b.snapshotid = a.snapshotid and b.dbid = a.dbid and b.schemaid = a.schemaid and UPPER(a.type) = 'U'
	
	return @name

end

GO

GRANT EXECUTE ON [dbo].[getschemanamewithobjectid] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

