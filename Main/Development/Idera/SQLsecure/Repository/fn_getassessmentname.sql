SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getassessmentname]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getassessmentname]
GO

CREATE FUNCTION [dbo].[getassessmentname]
(
	@policyid int,
	@assessmentid int
)
RETURNS nvarchar(260)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Return the combined name of the assessment and the policy

BEGIN

	DECLARE @name nvarchar(260)

	if (@policyid > 0)
	begin
		SELECT @name = policyname FROM [policy] WHERE policyid = @policyid

		if (@assessmentid is not null and @assessmentid <> [dbo].[getdefaultassessmentid](@policyid))
			SELECT @name = @name + N' - ' + assessmentname from [assessment] WHERE policyid = @policyid and assessmentid = @assessmentid
	end

	RETURN (@name)

END

GO

GRANT EXECUTE ON [dbo].[getassessmentname]   TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


