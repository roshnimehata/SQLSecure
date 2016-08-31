SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getserverosobjecttype]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getserverosobjecttype]
GO

CREATE  function [dbo].[getserverosobjecttype]
(
	@snapshotid int, 
	@osobjectid int
)
RETURNS nvarchar(16)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the object type of the object from the os object table for display.
BEGIN
	DECLARE @type nvarchar(260)

	SELECT @type = objecttype 
		FROM serverosobject 
		WHERE snapshotid = @snapshotid 
			and osobjectid = @osobjectid

	RETURN @type
END

GO

GRANT EXECUTE ON [dbo].[getserverosobjecttype] TO [SQLSecureView]
