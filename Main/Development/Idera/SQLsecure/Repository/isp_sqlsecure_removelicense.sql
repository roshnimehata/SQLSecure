SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_removelicense]'))
drop procedure [dbo].[isp_sqlsecure_removelicense]
GO

CREATE procedure [dbo].[isp_sqlsecure_removelicense] (@licenseid int)
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Remove a single license with license id
   -- 	           
	
	declare @result int
	declare @err int
	declare @errmsg nvarchar(500)
	declare @ans int

	-- Get application program name
	declare @programname nvarchar(128)
	select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)

	declare @connectionname nvarchar(128)
	set @connectionname = NULL
		
	exec @ans = [isp_sqlsecure_isadmin]

	if (@ans = 0)
	begin
		set @errmsg = 'Error: Insufficient privileges to remove license'
		exec isp_sqlsecure_addactivitylog @activitytype=N'Failure Audit', @source=@programname, @eventcode=N'Remove', @category=N'License Key', @description=@errmsg, @connectionname = @connectionname
		RAISERROR (@errmsg, 16, 1)
		return -1
	end
		
	BEGIN TRAN
	
		delete from applicationlicense where licenseid = @licenseid

		select @err = @@error

		if @err <> 0
		begin
			set @errmsg = 'Error: Failed to remove a license key with license id ' + CONVERT(NVARCHAR(256), @licenseid)
			RAISERROR (@errmsg, 16, 1)
			ROLLBACK TRAN
			return -1
		end

		declare @str nvarchar(500)
		set @str = N'Remove license with license id ' + CONVERT(NVARCHAR(256),@licenseid) + N'.'

		exec isp_sqlsecure_addactivitylog @activitytype=N'Success Audit', @source=@programname, @eventcode=N'Remove', @category=N'License Key', @description=@str, @connectionname = @connectionname

	COMMIT TRAN	
GO
