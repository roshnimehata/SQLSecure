SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_removedatabasedata]'))
drop procedure [dbo].[isp_sqlsecure_removedatabasedata]
GO

CREATE procedure [dbo].[isp_sqlsecure_removedatabasedata] (@snapshotid int, @dbid int)
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Remove all the database data based on dbid

	declare @err int
	declare @errmsg nvarchar(500)
	declare @ans int

	declare @programname nvarchar(128)
	select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)

	exec @ans = [isp_sqlsecure_isadmin]

	if (@ans = 0)
	begin
		set @errmsg = N'Error: Insufficient privileges to delete database data.'
		exec isp_sqlsecure_addactivitylog @activitytype=N'Failure Audit', @source=@programname, @eventcode=N'Remove', 	  @category=N'Database Data', @description=@errmsg, @connectionname = NULL
		RAISERROR (@errmsg, 16, 1)
		return -1
	end

	BEGIN TRAN
	
	delete from databaseobjectpermission where snapshotid = @snapshotid and dbid = @dbid

	delete from databaseobject where snapshotid = @snapshotid and dbid = @dbid

	delete from databaseschemapermission where snapshotid = @snapshotid and dbid = @dbid

	delete from databaseprincipalpermission where snapshotid = @snapshotid and dbid = @dbid

	delete from databaserolemember where snapshotid = @snapshotid and dbid = @dbid

	delete from databaseschema where snapshotid = @snapshotid and dbid = @dbid

	delete from databaseprincipal where snapshotid = @snapshotid and dbid = @dbid

	select @err = @@error

	if @err <> 0
	begin
		set @errmsg = 'Error: Failed to remove database data with snapshot id ' + CONVERT(nvarchar(12), @snapshotid) + ' and ' + CONVERT(nvarchar(12), @dbid)
		RAISERROR (@errmsg, 16, 1)
		ROLLBACK TRAN
		return -1
	end

	declare @str nvarchar(500)
	set @str = N'Remove all database data with snapshot id ' + CONVERT(nvarchar(12), @snapshotid) + ' and ' + CONVERT(nvarchar(12), @dbid)

	exec isp_sqlsecure_addactivitylog @activitytype=N'Success Audit', @source=@programname, @eventcode=N'Remove', @category=N'Database', @description=@str, @connectionname = NULL

	COMMIT TRAN

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

