SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getyesnotext]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getyesnotext]
GO

CREATE FUNCTION [dbo].[getyesnotext] 
(
	@answer char(1)
) 
RETURNS nvarchar(3)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the text for 'Y' or 'N' (yes or no)
   --					'U' means not available for this version of Sql Server

BEGIN
	DECLARE @answertext nvarchar(3)
	
	SELECT @answertext = case @answer
		WHEN 'Y' THEN 'Yes'
		WHEN 'N' THEN 'No'
		WHEN 'U' THEN 'N/A'

		ELSE '-'
		END
	
	RETURN @answertext

END

GO

GRANT EXECUTE ON [dbo].[getyesnotext]   TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


