SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_addlicense]'))
drop procedure [dbo].[isp_sqlsecure_addlicense]
GO

CREATE procedure [dbo].[isp_sqlsecure_addlicense] (@licensekey nvarchar(256))
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Insert a single license
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
		set @errmsg = 'Error: Insufficient privileges to add license'
		exec isp_sqlsecure_addactivitylog @activitytype=N'Failure Audit', @source=@programname, @eventcode=N'Add', @category=N'License Key', @description=@errmsg, @connectionname = @connectionname
		RAISERROR (@errmsg, 16, 1)
		return -1
	end

	if (@licensekey IS NULL)
	begin
		set @errmsg = N'License key cannot be null.'
		exec isp_sqlsecure_addactivitylog @activitytype=N'Failure Audit', @source=@programname, @eventcode=N'Add', @category=N'License Key', @description=@errmsg, @connectionname = @connectionname
		RAISERROR (@errmsg, 16, 1)
		return -1
	end 
	
	declare @mxlen int
	--set @mxlen = LEN(@licensekey)

	if (LEN(@licensekey) > 256)
	begin
		set @errmsg = N'License key cannot be longer than 256 characters.'
		exec isp_sqlsecure_addactivitylog @activitytype=N'Failure Audit', @source=@programname, @eventcode=N'Add', @category=N'License Key', @description=@errmsg, @connectionname = @connectionname
		RAISERROR (@errmsg, 16, 1)
		return -1
	end
	
	BEGIN TRAN
	
		insert into applicationlicense (licensekey, createdby, createdtm) values (@licensekey, SYSTEM_USER, GETUTCDATE())

		select @err = @@error

		if @err <> 0
		begin
			set @errmsg = 'Error: Failed to insert a new license key to applicationlicense table with licence ' + @licensekey
			RAISERROR (@errmsg, 16, 1)
			ROLLBACK TRAN
			return -1
		end


		declare @str nvarchar(500)
		set @str = N'Added new license key ' + @licensekey + N'.'

		exec isp_sqlsecure_addactivitylog @activitytype=N'Success Audit', @source=@programname, @eventcode=N'Add', @category=N'License Key', @description=@str, @connectionname = @connectionname


	COMMIT TRAN

	exec('select max(licenseid) from applicationlicense')
	
GO
