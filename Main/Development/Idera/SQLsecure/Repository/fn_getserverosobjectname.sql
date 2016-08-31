SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getserverosobjectname]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getserverosobjectname]
GO

CREATE  function [dbo].[getserverosobjectname]
(
	@snapshotid int, 
	@osobjectid int
)
RETURNS nvarchar(260)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the object name from the os object table for display.
BEGIN
	DECLARE @name nvarchar(260)

	SELECT @name = objectname 
		FROM serverosobject 
		WHERE snapshotid = @snapshotid 
			and osobjectid = @osobjectid

	RETURN @name
END

GO

GRANT EXECUTE ON [dbo].[getserverosobjectname] TO [SQLSecureView]
