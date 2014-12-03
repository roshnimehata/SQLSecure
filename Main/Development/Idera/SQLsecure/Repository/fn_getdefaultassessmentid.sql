SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getdefaultassessmentid]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getdefaultassessmentid]
GO

CREATE FUNCTION [dbo].[getdefaultassessmentid]
(
	@policyid int
)
RETURNS int
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Return the id of the default assessment for a policy

BEGIN
	
	RETURN (SELECT assessmentid from assessment where policyid = @policyid and assessmentstate = 'S')

END

GO

GRANT EXECUTE ON [dbo].[getdefaultassessmentid]   TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


