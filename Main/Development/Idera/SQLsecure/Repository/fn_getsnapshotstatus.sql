SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getsnapshotstatus]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getsnapshotstatus]
GO

CREATE FUNCTION [dbo].[getsnapshotstatus]
(
	@status nchar(3)
)
RETURNS nvarchar(10)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the status description of a snapshot status

BEGIN
	DECLARE @name nvarchar(40)
	
	SELECT @name = case upper(@status)
		WHEN 'S' THEN 'Successful'
		WHEN 'W' THEN 'Warnings'
		WHEN 'E' THEN 'Errors'
		WHEN 'I' THEN 'In Progress'

		ELSE 'Unknown'
		END
	
	RETURN @name

END

GO

GRANT EXECUTE ON [dbo].[getsnapshotstatus]   TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


