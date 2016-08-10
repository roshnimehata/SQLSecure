SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fn_getversionasdecimal]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[fn_getversionasdecimal]
GO

CREATE FUNCTION [dbo].[fn_getversionasdecimal] 
(
	@ver varchar(30) = '0.0.0.0'	
)
RETURNS decimal
AS
BEGIN
	DECLARE @dot1	int;
	DECLARE @dot2	int;
	DECLARE @dot3	int;
	DECLARE @result decimal;

	SET @dot1 = CHARINDEX('.', @ver);
	SET @dot2 = CHARINDEX('.', @ver, @dot1 + 1);
	SET @dot3 = CHARINDEX('.', @ver, @dot2 + 1);
	
	SET @result =	(CONVERT(decimal, SUBSTRING(@ver, 0, @dot1)) * POWER(CONVERT(decimal, 2), 48)) + 
					(CONVERT(decimal, SUBSTRING(@ver, @dot1 + 1, @dot2 - @dot1 - 1)) * POWER(CONVERT(decimal, 2), 32))
					
		if @dot3 <> 0
		begin
			set @result = @result+
			(CONVERT(decimal, SUBSTRING(@ver, @dot2 + 1, @dot3 - @dot2 - 1)) * POWER(CONVERT(decimal, 2), 16)) +
			CONVERT(decimal, SUBSTRING(@ver, @dot3 + 1, LEN(@ver) - @dot3));
		end			
	RETURN @result;
END

GO

GRANT EXECUTE ON [dbo].[fn_getversionasdecimal]   TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


