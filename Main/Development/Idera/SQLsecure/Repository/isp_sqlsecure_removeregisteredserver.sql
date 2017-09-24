SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_removeregisteredserver]'))
drop procedure [dbo].[isp_sqlsecure_removeregisteredserver]
GO

CREATE procedure [dbo].[isp_sqlsecure_removeregisteredserver] (@connectionname nvarchar(500), @removefromassessments bit = 0)
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

	BEGIN TRAN
	if(@removefromassessments = 0)
	begin
		insert into unregisteredserver ([connectionname]
			,[servername]
			,[instancename]
			,[snapshotretentionperiod]
			,[authenticationmode]
			,[sqlserverlogin]
			,[sqlserverpassword]
			,[sqlserverauthtype]
			,[serverlogin]
			,[serverpassword]
			,[os]
			,[version]
			,[edition]
			,[loginauditmode]
			,[enableproxyaccount]
			,[enablec2audittrace]
			,[crossdbownershipchaining]
			,[casesensitivemode]
			,[jobid]
			,[lastcollectiontm]
			,[lastcollectionsnapshotid]
			,[currentcollectiontm]
			,[currentcollectionstatus]
			,[registeredserverid]
			,[serverisdomaincontroller]
			,[replicationenabled]
			,[sapasswordempty]
			,[connectionport]
			,[auditfoldersstring]
			,[servertype]) select * from registeredserver where registeredserverid = @registeredserverid
			delete p from policymember p
			inner join assessment a on p.assessmentid = a.assessmentid and p.policyid = a.policyid
			where p.registeredserverid = @registeredserverid and a.assessmentstate not in (N'D', N'P', N'A')
	end
	else
	begin		
		delete from policymember where  registeredserverid = @registeredserverid
		update policyassessment set registeredserverid = null where registeredserverid = @registeredserverid
	end
	delete from filterrule where filterruleheaderid in (select filterruleheaderid from filterruleheader where UPPER(connectionname) = UPPER(@connectionname))
	delete from filterruleheader where  UPPER(connectionname) = UPPER(@connectionname)
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


GO

