SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getownerid]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getownerid]
GO

CREATE function [dbo].[getownerid] (@name nvarchar(128)) returns int
begin
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the principalid associated with a serverprincipal.
	declare @id int
 	set @id = NULL

	if (UPPER(@name) = 'PUBLIC')
 		set @id = 1
	else if (UPPER(@name) = 'RSEXECROLE')
 		set @id = 1
	else if (UPPER(@name) = 'DB_OWNER')
 		set @id = 1
	else if (UPPER(@name) = 'DB_ACCESSADMIN')
 		set @id = 1
	else if (UPPER(@name) = 'DB_SECURITYADMIN')
 		set @id = 1
	else if (UPPER(@name) = 'DB_DDLADMIN')
 		set @id = 1
	else if (UPPER(@name) = 'DB_BACKUPOPERATOR')
 		set @id = 1
	else if (UPPER(@name) = 'DB_DATAREADER')
 		set @id = 1
	else if (UPPER(@name) = 'DB_DATAWRITER')
 		set @id = 1
	else if (UPPER(@name) = 'DB_DENYDATAREADER')
 		set @id = 1
	else if (UPPER(@name) = 'DB_DENYDATAWRITER')
 		set @id = 1

	if (@id = NULL)
	begin
		select @id = principalid from serverprincipal where UPPER(name) = UPPER(@name)
	end

	return @id
end

GO

GRANT EXECUTE ON [dbo].[getownerid] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

