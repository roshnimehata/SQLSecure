SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getdatabaseobjectname]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getdatabaseobjectname]
GO

CREATE  function [dbo].[getdatabaseobjectname]
(
	@snapshotid int, 
	@dbid int, 
	@classid int, 
	@parentobjectid int, 
	@objectid int
)
RETURNS nvarchar(400)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the object name of a database object for display.
BEGIN
--	declare @database nvarchar(128)
	declare @schemaid int
	declare @name nvarchar(400)
	declare @type nvarchar(5)

--	select @database = d.databasename
--		from sqldatabase d
--		where d.snapshotid = @snapshotid
--				and d.dbid = @dbid

	select @name = o.name, @type = o.type, @schemaid = o.schemaid
		from databaseobject o
		where o.snapshotid = @snapshotid
			and o.dbid = @dbid
			and o.classid = @classid
			and o.parentobjectid = @parentobjectid
			and o.objectid = @objectid

	if (@schemaid is not null)
	begin
		select @name = s.schemaname + '.' + @name
			from databaseschema s
			where s.snapshotid = @snapshotid
				and s.dbid = @dbid
				and s.schemaid = @schemaid
	end

	if (@type = N'iCO')
	begin
		declare @parentname nvarchar(400)
		select @parentname = dbo.getdatabaseobjectname(@snapshotid, @dbid, 1, 0, @parentobjectid)
		select @name = case when len(@parentname) = 0 then N'' else @parentname + N'.' end + @name
	end
--	else
--	begin
--		select @name = @database + '.' + @name
--	end
		
	return isnull(@name, N'')
END

GO

GRANT EXECUTE ON [dbo].[getdatabaseobjectname] TO [SQLSecureView]
