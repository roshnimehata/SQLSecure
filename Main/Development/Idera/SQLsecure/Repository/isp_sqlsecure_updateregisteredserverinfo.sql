SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_updateregisteredserverinfo]'))
drop procedure [dbo].[isp_sqlsecure_updateregisteredserverinfo]
GO

CREATE procedure [dbo].[isp_sqlsecure_updateregisteredserverinfo] (@connectionname nvarchar(500), @authmode nvarchar(1), @os nvarchar(512), @version nvarchar(256), @edition nvarchar(256), @enableproxyaccount nchar(1), @enablec2 nchar(10), @ownerchaining nchar(1))
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Update registered server with auditing and versioning information.
   -- 	           This information might change from time to time so there is a need to update the master registered server record

	declare @err int
	declare @errmsg nvarchar(500)
	declare @ans int

	exec @ans = [isp_sqlsecure_isadmin]

	if (@ans = 0)
	begin
		set @errmsg = 'Error: Access denial. Insufficient privilege.'
		RAISERROR (@errmsg, 16, 1)
		return -1
	end

	if not exists (select * from registeredserver where UPPER(connectionname) = UPPER(@connectionname))
	begin
		set @errmsg = 'Error: Invalid connection name - ' + @connectionname
		RAISERROR (@errmsg, 16, 1)
	end
	else
	begin
		BEGIN TRAN

		update registeredserver set authenticationmode = @authmode, os = @os, version = @version, edition = @edition, enableproxyaccount = @enableproxyaccount, enablec2audittrace = @enablec2, crossdbownershipchaining = @ownerchaining where UPPER(connectionname) = UPPER(@connectionname)

		select @err = @@error

		if @err <> 0
		begin
			set @errmsg = 'Error: Failed to update registeredserver table'
			RAISERROR (@errmsg, 16, 1)
			ROLLBACK TRAN
			return -1
		end

		declare @str nvarchar(500)
		set @str = N'Update server information with connection name ' + @connectionname

		declare @programname nvarchar(128)
		select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)

		exec isp_sqlsecure_addactivitylog @activitytype=N'Success Audit', @source=@programname, @eventcode=N'UPDATE', @category=N'SERVER', @description=@str, @connectionname = @connectionname
		
		COMMIT TRAN
	
	end


GO

