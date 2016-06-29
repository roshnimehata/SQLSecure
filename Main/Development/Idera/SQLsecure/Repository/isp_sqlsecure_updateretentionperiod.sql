SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_updateretentionperiod]'))
drop procedure [dbo].[isp_sqlsecure_updateretentionperiod]
GO

CREATE procedure [dbo].[isp_sqlsecure_updateretentionperiod] (@connectionname nvarchar(400), @retentionperiod int=60)
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Update the snapshot retention period associated with the given registered server
   -- 	           

	declare @err int
	declare @errmsg nvarchar(500)
	declare @ans int

	declare @programname nvarchar(128)
	select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)

	exec @ans = [isp_sqlsecure_isadmin]

	if (@ans = 0)
	begin
		set @errmsg = N'Error: Insufficient privileges to update registered server retention period.'
		exec isp_sqlsecure_addactivitylog @activitytype=N'Failure Audit', @source=@programname, @eventcode=N'Update', 	  @category=N'Server', @description=@errmsg, @connectionname = @connectionname
		RAISERROR (@errmsg, 16, 1)
		return -1
	end

	BEGIN TRAN

	if not exists (select * from registeredserver where UPPER(connectionname) = UPPER(@connectionname))
	begin
			set @errmsg = 'Error: Connection name ' + @connectionname + ' not found.' 
			RAISERROR (@errmsg, 16, 1)
			ROLLBACK TRAN
			return -1
	end
		
	update registeredserver set snapshotretentionperiod = @retentionperiod where UPPER(connectionname) = UPPER(@connectionname)

	select @err = @@error

	if @err <> 0
	begin
		set @errmsg = 'Error: Failed to update snapshot retention period in registeredserver table with connection name ' + @connectionname
		RAISERROR (@errmsg, 16, 1)
		ROLLBACK TRAN
		return -1
	end

	declare @str nvarchar(500)
	set @str = N'Updated registered server retention period to ' + CONVERT(nvarchar(10), @retentionperiod)

	exec isp_sqlsecure_addactivitylog @activitytype=N'Success Audit', @source=@programname, @eventcode=N'Update', @category=N'Server', @description=@str, @connectionname = @connectionname

	
	COMMIT TRAN

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_updateretentionperiod] TO [SQLSecureView]

GO
