SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_updatesnapshotstatus]'))
drop procedure [dbo].[isp_sqlsecure_updatesnapshotstatus]
GO

CREATE procedure [dbo].[isp_sqlsecure_updatesnapshotstatus] (@connectionname nvarchar(400))
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Find all previous snapshots marked as running, log and change to error status

	declare @err int, @msg nvarchar(500)

	declare @programname nvarchar(128), @action nvarchar(32), @category nvarchar(32), @success nvarchar(32), @failure nvarchar(32)
	select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)
	select @action=N'Update', @category=N'Snapshot Status', @success=N'Success Audit', @failure=N'Failure Audit'

	declare @ans int
	exec @ans = [isp_sqlsecure_isadmin]

	if (@ans = 0)
	begin
		set @msg = N'Error: Insufficient privileges to ' + lower(@action) + ' a ' + lower(@category)
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = @connectionname
		RAISERROR (@msg, 16, 1)
		return -1
	end

	-- if there is no such server then return error
	if not exists (select * from registeredserver where upper(connectionname) = upper(@connectionname))
	begin
		set @msg = 'Error: Server ' + @connectionname + ' not found'
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = @connectionname
		RAISERROR (@msg, 16, 1)
		return -1
	end

	declare @count int
	select @count=count(*) from serversnapshot
		where upper(connectionname) = upper(@connectionname) and [status] = 'I'
	if (@count > 0)
	begin
		set @msg = N'Collector aborted during processing. See log for details.'
		update serversnapshot set status='E',snapshotcomment=@msg
			where upper(connectionname) = upper(@connectionname) and [status] = 'I'
		set @msg = N'Updated snapshot status for ' + convert(nvarchar(10), @count) + ' hung snapshots'
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = @connectionname
	end

	return @count

GO

