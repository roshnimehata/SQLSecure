SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getschemaname]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getschemaname]
GO


CREATE  function [dbo].[getschemaname] (@snapshotid int, @dbid int, @schemaid int) returns nvarchar(128)
begin
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the schema name associated with a database schema.
	declare @name nvarchar(128)
	
	select @name = schemaname from databaseschema where snapshotid = @snapshotid and dbid = @dbid and schemaid = @schemaid
	
	return @name

end

GO

GRANT EXECUTE ON [dbo].[getschemaname] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

