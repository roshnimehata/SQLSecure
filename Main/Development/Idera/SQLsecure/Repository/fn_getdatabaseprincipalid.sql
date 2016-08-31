SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getdatabaseprincipalid]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getdatabaseprincipalid]
GO


CREATE  function [dbo].[getdatabaseprincipalid] (@snapshotid int, @dbid int, @name nvarchar(256)) returns int
begin
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the principalid associated with the principal name in the database with the selected databaseid .
	declare @id int
	
	select @id = uid from databaseprincipal where snapshotid = @snapshotid and dbid = @dbid and UPPER(name) = UPPER(@name)
	
	return @id

end

GO

GRANT EXECUTE ON [dbo].[getdatabaseprincipalid] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

