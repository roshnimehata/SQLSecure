SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[replacenullwithhyphen]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[replacenullwithhyphen]
GO

CREATE FUNCTION [dbo].[replacenullwithhyphen] 
(
	@nulltext nvarchar(400)
) 
RETURNS nvarchar(400)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Replaces null values with a hyphen, otherwise returns the value

BEGIN
	DECLARE @notnulltext nvarchar(400)
	
	SELECT @notnulltext = 
		CASE WHEN @nulltext IS NULL THEN '-' 
		ELSE @nulltext
		END
	
	RETURN @notnulltext

END

GO

GRANT EXECUTE ON [dbo].[replacenullwithhyphen]   TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


