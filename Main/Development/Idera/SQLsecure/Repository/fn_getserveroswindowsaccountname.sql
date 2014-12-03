SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getserveroswindowsaccountname]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getserveroswindowsaccountname]
GO

CREATE  function [dbo].[getserveroswindowsaccountname]
(
	@snapshotid int, 
	@sid varbinary(85)
)
RETURNS nvarchar(200)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the name of the windows account from the os windows accounts for display.
BEGIN
	DECLARE @name nvarchar(200)

	SELECT @name = [name] 
		FROM serveroswindowsaccount 
		WHERE snapshotid = @snapshotid 
			and [sid] = @sid
	
	RETURN @name
END

GO

GRANT EXECUTE ON [dbo].[getserveroswindowsaccountname] TO [SQLSecureView]
