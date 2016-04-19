SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getobjecttypenamebyobjectid]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getobjecttypenamebyobjectid]
GO


CREATE  function [dbo].[getobjecttypenamebyobjectid] 
(
	@snapshotid int, 
	@dbid int, 
	@classid int, 
	@parentobjectid int, 
	@objectid int
) returns nvarchar(256)
begin
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the object type name associated with an object type.
	declare @name nvarchar(256)
	declare @objecttype nvarchar(5)

	select @objecttype = [type] 
		from databaseobject
		where snapshotid = @snapshotid
			and [dbid] = @dbid
			and classid = @classid
			and parentobjectid = @parentobjectid
			and objectid = @objectid

	select @name = objecttypename from objecttype where UPPER(objecttype) = UPPER(@objecttype)
	
	return @name

end

GO

GRANT EXECUTE ON [dbo].[getobjecttypenamebyobjectid] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

