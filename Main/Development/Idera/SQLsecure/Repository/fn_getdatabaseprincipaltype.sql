SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getdatabaseprincipaltype]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getdatabaseprincipaltype]
GO


CREATE  function [dbo].[getdatabaseprincipaltype] (@snapshotid int, @dbid int, @uid int) returns nvarchar(12)
begin
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the principal type associated with a database user.
	declare @name nvarchar(15)
	
	select @name = type from databaseprincipal where snapshotid = @snapshotid and dbid = @dbid and uid = @uid
	
	if (@name in ('S','U','G') )
		set @name = 'iDUSR'
	else if (@name in ('A','R')) 
		set @name = 'iDRLE'

	return @name
	
end

GO

GRANT EXECUTE ON [dbo].[getdatabaseprincipaltype] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

