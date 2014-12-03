SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gettablename]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[gettablename]
GO


CREATE  function [dbo].[gettablename] (@snapshotid int, @dbid int, @objectid int) returns nvarchar(256)
begin
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the table name associated with a database object.
	declare @name nvarchar(256)
	
	select @name = name from databaseobject where snapshotid = @snapshotid and dbid = @dbid and objectid = @objectid
	
	return @name

end

GO

GRANT EXECUTE ON [dbo].[gettablename] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

