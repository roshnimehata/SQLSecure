SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getdatabasename]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getdatabasename]
GO

CREATE  function [dbo].[getdatabasename]
(
	@snapshotid int, 
	@dbid int
)
RETURNS nvarchar(128)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the object name from the os object table for display.
BEGIN
	DECLARE @name nvarchar(260)

	SELECT @name = databasename 
		FROM sqldatabase 
		WHERE snapshotid = @snapshotid 
			and [dbid] = @dbid

	RETURN @name
END

GO

GRANT EXECUTE ON [dbo].[getdatabasename] TO [SQLSecureView]
