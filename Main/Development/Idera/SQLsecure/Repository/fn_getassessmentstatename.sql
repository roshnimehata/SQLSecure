SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getassessmentstatename]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getassessmentstatename]
GO

CREATE  function [dbo].[getassessmentstatename]
	(@assessmentstate nchar(1))
	returns nvarchar(20)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the name of the assessment state for display.
BEGIN
	DECLARE @name nvarchar(20)

	SELECT @name = 
		CASE WHEN @assessmentstate IS NULL THEN 'Unknown' ELSE
		CASE @assessmentstate
			WHEN 'S' THEN 'Settings'
			WHEN 'C' THEN 'Current'
			WHEN 'D' THEN 'Draft'
			WHEN 'P' THEN 'Published'
			WHEN 'A' THEN 'Approved'

			ELSE 'Unknown'
		END END
	
	RETURN @name
END

GO

GRANT EXECUTE ON [dbo].[getassessmentstatename] TO [SQLSecureView]
