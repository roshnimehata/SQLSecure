SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getserverosobjecttypename]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getserverosobjecttypename]
GO


CREATE  function [dbo].[getserverosobjecttypename] (@objecttype nvarchar(128)) returns nvarchar(32)
begin
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the object type name associated with a server os object type.
	declare @name nvarchar(32)
	
	select @name = case upper(@objecttype)
						when 'DB'
							then 'Database File'
						when 'FDIR'
							then 'Data Directory'
						when 'IDIR'
							then 'Install Directory'
						when 'REG'
							then 'Registry Key'
						when 'UNK'	-- this may not be a valid type, but it was in the class in the Console, so I am repeating it here
							then 'Unknown'
						else 
							@objecttype
						end
	return @name

end

GO

GRANT EXECUTE ON [dbo].[getserverosobjecttypename] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

