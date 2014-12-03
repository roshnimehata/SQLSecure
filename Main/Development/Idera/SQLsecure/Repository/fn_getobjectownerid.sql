SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getobjectownerid]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getobjectownerid]
GO


CREATE  function [dbo].[getobjectownerid] (@snapshotid int, @dbid int, @parentobjectid int) returns int
begin
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the ownerid associated with a database object.
	declare @id int
	
	select @id = owner from databaseobject where snapshotid = @snapshotid and dbid = @dbid and objectid = @parentobjectid and type = 'U'
		
	return @id

end

GO

GRANT EXECUTE ON [dbo].[getobjectownerid] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

