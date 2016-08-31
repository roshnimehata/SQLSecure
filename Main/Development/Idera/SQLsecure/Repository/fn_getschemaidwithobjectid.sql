SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getschemaidwithobjectid]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getschemaidwithobjectid]
GO


CREATE  function [dbo].[getschemaidwithobjectid] (@snapshotid int, @dbid int, @objectid int) returns int
begin
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the schema id associated with a database object.
	declare @id int
	
	select @id = schemaid from databaseobject where snapshotid = @snapshotid and dbid = @dbid and objectid = @objectid and UPPER(type) = 'U'
	
	return @id

end

GO

GRANT EXECUTE ON [dbo].[getschemaidwithobjectid] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

