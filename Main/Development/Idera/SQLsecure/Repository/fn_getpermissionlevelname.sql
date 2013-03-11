SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getpermissionlevelname]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getpermissionlevelname]
GO

CREATE FUNCTION [dbo].[getpermissionlevelname]
(
	@permissionlevel varchar(3)
)
RETURNS nvarchar(10)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the name of the object type.

BEGIN
	DECLARE @name nvarchar(40)
	
	SELECT @name = case @permissionlevel
		WHEN 'DB' THEN 'Database'
		WHEN 'SV' THEN 'Server'
		WHEN 'OBJ' THEN 'Object'

		ELSE 'Unknown'
		END
	
	RETURN @name

END

GO

GRANT EXECUTE ON [dbo].[getpermissionlevelname]   TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


