SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getaccesstypename]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getaccesstypename]
GO


CREATE  function [dbo].[getaccesstypename] (@AccessType int) returns nvarchar(512)
begin
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the access type name associated with an os object permission.
	declare @name nvarchar(512)

	select @name = case @AccessType
						when 0 
							then N'Allow' 
						when 1 
							then N'Deny' 
						else N'Unknown' 
					end

	return @name

end

GO

GRANT EXECUTE ON [dbo].[getaccesstypename] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

