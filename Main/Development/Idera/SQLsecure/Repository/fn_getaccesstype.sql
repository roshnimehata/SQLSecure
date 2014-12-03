SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getaccesstype]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getaccesstype]
GO

CREATE FUNCTION [dbo].[getaccesstype] 
(
	@isdeny char(1),
	@isgrantwith char(1),
	@isgrant char(1),
	@isrevoke char(1)
)
RETURNS nvarchar(20)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the effective access type based on the passed permissions.

BEGIN
	DECLARE @accesstype nvarchar(40)
	
	SELECT @accesstype = 
		CASE WHEN @isdeny = 'Y' THEN 'Deny' ELSE
		CASE WHEN @isgrantwith = 'Y' THEN 'Grant With' ELSE
		CASE WHEN @isgrant = 'Y' THEN 'Grant' ELSE
		CASE WHEN @isrevoke = 'Y' THEN 'Revoke'
		ELSE 'Revoke'
		END END END END
	
	RETURN @accesstype

END

GO

GRANT EXECUTE ON [dbo].[getaccesstype]   TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


