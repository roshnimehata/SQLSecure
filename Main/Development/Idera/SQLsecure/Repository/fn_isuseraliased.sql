SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isuseraliased]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[isuseraliased]
GO


CREATE  function [dbo].[isuseraliased] (@snapshotid int, @dbid int, @uid int) returns nchar(1)
begin
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get if a database user is aliased.
	declare @result nchar(1)

	set @result = 'N'

	if exists (select * from databaseprincipal where snapshotid = @snapshotid and dbid = @dbid and altuid = @uid and UPPER(isalias) = 'Y')
	begin
		set @result = 'Y'	
	end

	return @result

end

GO

GRANT EXECUTE ON [dbo].[isuseraliased] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

