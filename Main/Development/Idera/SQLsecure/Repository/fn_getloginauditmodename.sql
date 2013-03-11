SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getloginauditmodename]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getloginauditmodename]
GO

CREATE  function [dbo].[getloginauditmodename]
	(@auditmode nvarchar(20))
	returns nvarchar(60)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the name of the login audit mode mode for display.
BEGIN
	DECLARE @name nvarchar(60)

	SELECT @name = 
		CASE WHEN @auditmode IS NULL THEN 'Unknown' ELSE
		CASE @auditmode
			WHEN 'Success' THEN 'Successful logins only'
			WHEN 'Failure' THEN 'Failed logins only'
			WHEN 'All' THEN 'Both failed and successful logins'

			ELSE 'Unknown'
		END END
	
	RETURN @name
END

GO

GRANT EXECUTE ON [dbo].[getloginauditmodename] TO [SQLSecureView]
