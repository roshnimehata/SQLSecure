SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getpolicyseverityname]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getpolicyseverityname]
GO

CREATE FUNCTION [dbo].[getpolicyseverityname]
(
	@severity int
)
RETURNS nvarchar(30)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the name of the policy severity

BEGIN
	DECLARE @name nvarchar(40)
	
	SELECT @name = CASE @severity
		WHEN 0 THEN 'OK'
		WHEN 1 THEN 'Low'
		WHEN 2 THEN 'Medium'
		WHEN 3 THEN 'High'

		ELSE 'Unknown'
		END
	
	RETURN @name

END

GO

GRANT EXECUTE ON [dbo].[getpolicyseverityname]   TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


