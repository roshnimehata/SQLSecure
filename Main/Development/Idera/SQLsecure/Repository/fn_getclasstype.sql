SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getclasstype]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getclasstype]
GO


CREATE  function [dbo].[getclasstype] (@classid int) returns nvarchar(128)
begin
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the class type associated with a classid.
	declare @name nvarchar(128)
	
	select @name = classname from classtype where classid = @classid
	
	return @name

end

GO

GRANT EXECUTE ON [dbo].[getclasstype] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

