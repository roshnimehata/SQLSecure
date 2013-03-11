SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getauthenticationmodename]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getauthenticationmodename]
GO

CREATE  function [dbo].[getauthenticationmodename]
	(@authenticationmode char(1))
	returns nvarchar(60)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the name of the authentication mode for display.
BEGIN
	DECLARE @name nvarchar(60)

	SELECT @name = 
		CASE WHEN @authenticationmode IS NULL THEN 'Unknown' ELSE
		CASE @authenticationmode
			WHEN 'W' THEN 'Windows Authentication Mode'
			WHEN 'M' THEN 'SQL Server and Windows Authentication Mode'

			ELSE 'Unknown'
		END END
	
	RETURN @name
END

GO

GRANT EXECUTE ON [dbo].[getauthenticationmodename] TO [SQLSecureView]
