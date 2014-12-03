SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getpasswordstatusdescription]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getpasswordstatusdescription]
GO

CREATE  function [dbo].[getpasswordstatusdescription] (@passwordStatus int)
RETURNS nvarchar(256)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the object type of the object from the os object table for display.
BEGIN
	DECLARE @description nvarchar(260)

	SELECT @description =
			CASE @passwordStatus
			WHEN 0 THEN 'OK'
			WHEN 1 THEN 'Blank'
			WHEN 2 THEN 'Weak'
			WHEN 4 THEN 'Matches Login Name'
			ELSE 'N/A'
		END

	RETURN @description
END

GO

GRANT EXECUTE ON [dbo].[getpasswordstatusdescription] TO [SQLSecureView]
