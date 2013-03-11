SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_removeregisteredserver]'))
drop procedure [dbo].[isp_sqlsecure_removeregisteredserver]
GO

CREATE procedure [dbo].[isp_sqlsecure_removeregisteredserver] (@connectionname nvarchar(500))
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Remove registered server from the registeredserver table
   -- 	           

	declare @err int, @msg nvarchar(500)

	declare @programname nvarchar(128), @action nvarchar(32), @category nvarchar(32), @success nvarchar(32), @failure nvarchar(32)
	select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)
	select @action=N'Remove', @category=N'Registered Server', @success=N'Success Audit', @failure=N'Failure Audit'

	declare @ans int
	exec @ans = [isp_sqlsecure_isadmin]

	if (@ans = 0)
	begin
		set @msg = N'Error: Insufficient privileges to remove ' + lower(@category) + ' "' + @connectionname + '"'
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname=@connectionname
		RAISERROR (@msg, 16, 1)
		return -1
	end

	declare @registeredserverid int
	select @registeredserverid=registeredserverid from registeredserver where UPPER(connectionname) = UPPER(@connectionname)

	if (@registeredserverid is null)
	begin
		set @msg = 'Error: Invalid ' + @category + ' "' + @connectionname + '"'
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname=@connectionname
		RAISERROR (@msg, 16, 1)
		return -1
	end

	if exists (select distinct a.registeredserverid 
				from policyassessment a 
					inner join assessment b on a.policyid = b.policyid 
						and a.assessmentid = b.assessmentid 
				where a.registeredserverid = @registeredserverid 
					and b.assessmentstate in (N'D', N'P', N'A'))
	begin
		set @msg = 'Error: ' + @category + ' "' + @connectionname + '" must be removed from all draft and published assessments before it can be removed from auditing.'
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname=@connectionname
		RAISERROR (@msg, 16, 1)
		return -1
	end
	else
	begin
		BEGIN TRAN

		delete from filterrule where filterruleheaderid in (select filterruleheaderid from filterruleheader where UPPER(connectionname) = UPPER(@connectionname))
		delete from filterruleheader where  UPPER(connectionname) = UPPER(@connectionname)
		delete from policymember where  registeredserverid = @registeredserverid
		update policyassessment set registeredserverid = null where registeredserverid = @registeredserverid
		delete from registeredservernotification where  registeredserverid = @registeredserverid
		delete from registeredserver where  UPPER(connectionname) = UPPER(@connectionname)

		select @err = @@ERROR

		if @err <> 0
		begin
			set @msg = 'Error: Failed to ' + lower(@action) + ' ' + lower(@category) + ' "' + @connectionname + '"'
			exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname=@connectionname
			RAISERROR (@msg, 16, 1)
			ROLLBACK TRAN
			return -1
		end


		set @msg = @category + N' "' + @connectionname + '" with id ' + CONVERT(NVARCHAR, @registeredserverid)
		exec isp_sqlsecure_addactivitylog @activitytype=@success, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname=@connectionname

		COMMIT TRAN
	end


GO

