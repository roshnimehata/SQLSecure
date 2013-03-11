SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_updatereportconfigureinfo]'))
drop procedure [dbo].[isp_sqlsecure_updatereportconfigureinfo]
GO

CREATE procedure [dbo].[isp_sqlsecure_updatereportconfigureinfo] 
(
	@reportserver nvarchar(128), 
	@servervirtualdirectory nvarchar(256), 
	@managervirtualdirectory nvarchar(256),
	@port int,
	@usessl tinyint,
	@username nvarchar(128),
	@repository nvarchar(128), 
	@targetdirectory nvarchar(256)
)
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Update report configuration table

	declare @err int
	declare @errmsg nvarchar(500)
	declare @ans int

	declare @programname nvarchar(128)
	select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)

	exec @ans = [isp_sqlsecure_isadmin]

	if (@ans = 0)
	begin
		set @errmsg = N'Error: Insufficient privileges to update report configuration.'
		exec isp_sqlsecure_addactivitylog @activitytype=N'Failure Audit', @source=@programname, @eventcode=N'Update', @category=N'Report', @description=@errmsg, @connectionname = NULL
		RAISERROR (@errmsg, 16, 1)
		return -1
	end

	BEGIN TRAN
	
	-- remove any old data and just insert the new values
	delete from reports
	insert into reports 
		values(@reportserver,
			@servervirtualdirectory,
			@managervirtualdirectory,
			@port,
			@usessl,
			@username,
			@repository,
			@targetdirectory)

	select @err = @@error

	if @err <> 0
	begin
		set @errmsg = 'Error: Failed to update report configuration'
		RAISERROR (@errmsg, 16, 1)
		ROLLBACK TRAN
		return -1
	end

	declare @str nvarchar(500)
	set @str = N'Updated report configuration table'

	exec isp_sqlsecure_addactivitylog @activitytype=N'Success Audit', @source=@programname, @eventcode=N'Update', @category=N'Report', @description=@str, @connectionname = NULL

	COMMIT TRAN

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

