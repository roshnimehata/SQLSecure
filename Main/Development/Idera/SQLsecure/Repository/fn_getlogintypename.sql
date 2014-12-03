SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getlogintypename]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getlogintypename]
GO

CREATE FUNCTION [dbo].[getlogintypename] 
(
	@logintype char(1)
) 
RETURNS nvarchar(10)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the name of the login type.

BEGIN
	DECLARE @name nvarchar(40)
	
	SELECT @name = CASE @logintype
		WHEN 'W' THEN 'Windows'
		WHEN 'S' THEN 'SQL Server'

		ELSE 'Unknown'
		END
	
	RETURN @name

END

GO

GRANT EXECUTE ON [dbo].[getlogintypename]   TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


