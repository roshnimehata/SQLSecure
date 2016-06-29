SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getdatabaseprincipalname]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getdatabaseprincipalname]
GO


CREATE  function [dbo].[getdatabaseprincipalname] (@snapshotid int, @dbid int, @uid int) returns nvarchar(128)
begin
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the principal name associated with a database userid.
	declare @name nvarchar(128)
	
	select @name = name from databaseprincipal where snapshotid = @snapshotid and dbid = @dbid and uid = @uid
	
	return @name

end

GO

GRANT EXECUTE ON [dbo].[getdatabaseprincipalname] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

