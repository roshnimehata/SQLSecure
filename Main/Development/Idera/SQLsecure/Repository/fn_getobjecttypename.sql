SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getobjecttypename]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getobjecttypename]
GO


CREATE  function [dbo].[getobjecttypename] (@objecttype nvarchar(128)) returns nvarchar(256)
begin
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the object type name associated with an object type.
	declare @name nvarchar(256)
	
	select @name = objecttypename from objecttype where UPPER(objecttype) = UPPER(@objecttype)
	
	return @name

end

GO

GRANT EXECUTE ON [dbo].[getobjecttypename] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

