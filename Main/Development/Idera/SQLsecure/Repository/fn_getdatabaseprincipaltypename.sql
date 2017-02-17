SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getdatabaseprincipaltypename]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getdatabaseprincipaltypename]
GO

CREATE  function [dbo].[getdatabaseprincipaltypename]
	(@principaltype char(1))
	returns nvarchar(40)
AS
   -- <Idera SQLsecure version and copyright>
   --
   --
   -- Description :
   --              Get the name of the database principal type for display.
BEGIN
	DECLARE @name nvarchar(40)

	SELECT @name = 
		CASE WHEN @principaltype IS NULL THEN 'Unknown' ELSE
		CASE @principaltype
			WHEN 'S' THEN 'SQL Login'
			WHEN 'U' THEN 'Windows User'
			WHEN 'G' THEN 'Windows Group'
			WHEN 'A' THEN 'Application Role'
			WHEN 'R' THEN 'Database Role'
			WHEN 'C' THEN 'Certificate Mapped User'
			WHEN 'K' THEN 'Asymmetric Key Mapped User'
			WHEN 'E' THEN 'Azure AD User'	-- SQL Secure 3.1 (Anshul) : Add support for Azure.
			WHEN 'X' THEN 'Azure AD Group'
			ELSE 'Unknown'
		END END

	RETURN @name
END

GO

GRANT EXECUTE ON [dbo].[getdatabaseprincipaltypename] TO [SQLSecureView]
