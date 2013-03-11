SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getactiontype]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getactiontype]
GO

CREATE function [dbo].[getactiontype]
(
	@action int
)
returns varchar(128)
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the action type associated with an actionid.
begin
	declare @data nvarchar(128)
	set @data = 'NULL'

	if (@action = 26)
 		set @data = 'REFERENCES'
	else if (@action = 178)
 		set @data = 'CREATE FUNCTION'
	else if (@action = 193)
 		set @data = 'SELECT'
	else if (@action = 195)
 		set @data = 'INSERT'
	else if (@action = 196)
 		set @data = 'DELETE'
	else if (@action = 197)
 		set @data = 'UPDATE'
	else if (@action = 198)
 		set @data = 'CREATE TABLE'
	else if (@action = 203)
 		set @data = 'CREATE DATABASE'
	else if (@action = 207)
 		set @data = 'CREATE VIEW'
	else if (@action = 222)
 		set @data = 'CREATE PROCEDURE'
	else if (@action = 224)
 		set @data = 'EXECUTE'
	else if (@action = 228)
 		set @data = 'BACKUP DATABASE'
	else if (@action = 233)
 		set @data = 'CREATE DEFAULT'
	else if (@action = 235)
 		set @data = 'BACKUP LOG'
	else if (@action = 236)
 		set @data = 'CREATE RULE'

	return @data
end

GO

GRANT EXECUTE ON [dbo].[getactiontype]   TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


