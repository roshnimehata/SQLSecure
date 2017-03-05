SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_removesnapshot]'))
drop procedure [dbo].[isp_sqlsecure_removesnapshot]
GO

CREATE procedure [dbo].[isp_sqlsecure_removesnapshot] (@snapshotid int)
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Remove all snapshot information from repository
   -- 	           

	declare @err int
	declare @errmsg nvarchar(500)
	declare @ans int
	declare @programname nvarchar(128)
	select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)

	declare @connectionname nvarchar(400), 
		@registeredserverid int,
		@snapshotname nvarchar(30)
	select @connectionname = connectionname, 
		@registeredserverid = registeredserverid,
		@snapshotname = convert(nvarchar, starttime, 101) + ' ' + convert(nvarchar, starttime, 108) + ' (UTC)' from serversnapshot where snapshotid = @snapshotid

	exec @ans = [isp_sqlsecure_isadmin]

	if (@ans = 0)
	begin
		set @errmsg = 'Error: Insufficient privileges to remove snapshot for SQL Server instance ' + @connectionname
		exec isp_sqlsecure_addactivitylog @activitytype=N'Failure Audit', @source=@programname, @eventcode=N'Remove', @category=N'Snapshot', @description=@errmsg, @connectionname = @connectionname
		RAISERROR (@errmsg, 16, 1)
		return -1
	end

	if (@connectionname is null)
	begin
		set @errmsg = 'Error: Invalid snapshot id ' + CONVERT(nvarchar, @snapshotid)
		exec isp_sqlsecure_addactivitylog @activitytype=N'Failure Audit', @source=@programname, @eventcode=N'Remove', @category=N'Snapshot', @description=@errmsg, @connectionname = @connectionname
		RAISERROR (@errmsg, 16, 1)
	end

	if exists (select distinct a.registeredserverid 
				from policyassessment a 
					inner join assessment b on a.policyid = b.policyid 
						and a.assessmentid = b.assessmentid 
				where a.registeredserverid = @registeredserverid 
					and a.snapshotid = @snapshotid
					and b.assessmentstate in (N'D', N'P', N'A'))
	begin
		set @errmsg = 'Error: Snapshot "' + @snapshotname + '" for server "' + @connectionname + '" must be removed from all saved assessments before it can be removed from auditing. If it is used in an approved assessment, it can no longer be removed from auditing.'
		exec isp_sqlsecure_addactivitylog @activitytype=N'Failure Audit', @source=@programname, @eventcode=N'Remove', @category=N'Snapshot', @description=@errmsg, @connectionname = @connectionname
		RAISERROR (@errmsg, 16, 1)
	end
	else
	begin
		BEGIN TRAN
		
			
			delete from dbo.sqljob where SnapshotId = @snapshotid
			delete from dbo.sqljobproxy where snapshotid = @snapshotid
			delete from dbo.availabilitygroups where snapshotid = @snapshotid
			delete from dbo.availabilityreplicas where snapshotid = @snapshotid
			
			-- SQLsecure 3.1 (Anshul Aggarwal) - New Risk Assessments (Server-Level and DB-Level Firewall rules for Azure SQL DB)
			delete from azuresqldbfirewallrules where snapshotid = @snapshotid

			delete from databaseprincipalpermission where snapshotid = @snapshotid
			delete from databaseobjectpermission where snapshotid = @snapshotid
			delete from databaseschemapermission where snapshotid = @snapshotid
			delete from databaseobject where snapshotid = @snapshotid
			delete from databaseschema where snapshotid = @snapshotid
			delete from databaserolemember where snapshotid = @snapshotid
			delete from databaseprincipal where snapshotid = @snapshotid
			delete from sqldatabase where snapshotid = @snapshotid
			delete from windowsgroupmember where snapshotid = @snapshotid
			delete from windowsaccount where snapshotid = @snapshotid
			delete from serverpermission where snapshotid = @snapshotid
			delete from endpoint where snapshotid = @snapshotid
			delete from serverrolemember where snapshotid = @snapshotid
			delete from serverfilterrule where snapshotid = @snapshotid
			delete from serverfilterruleheader where snapshotid = @snapshotid
			delete from serverprincipal where snapshotid = @snapshotid
			delete from serverservice where snapshotid = @snapshotid
			delete from serverosobjectpermission where snapshotid = @snapshotid
			delete from serverosobject where snapshotid = @snapshotid
			delete from serverprotocol where snapshotid = @snapshotid
			delete from ancillarywindowsgroup where snapshotid = @snapshotid
			delete from serveroswindowsgroupmember where snapshotid = @snapshotid
			delete from serveroswindowsaccount where snapshotid = @snapshotid
			delete from serversnapshot where snapshotid = @snapshotid
		

			if @err <> 0
			begin
				set @errmsg = 'Error: Failed to delete snapshot information from repository'
				exec isp_sqlsecure_addactivitylog @activitytype=N'Failure Audit', @source=@programname, @eventcode=N'Remove', @category=N'Snapshot', @description=@errmsg, @connectionname = @connectionname
				RAISERROR (@errmsg, 16, 1)
				ROLLBACK TRAN
				return -1
			end

			declare @str nvarchar(500)
			set @str = N'Removed snapshot ' + @snapshotname + ', id=' + CONVERT(NVARCHAR, @snapshotid) + N' for SQL Server instance ' + @connectionname

			exec isp_sqlsecure_addactivitylog @activitytype=N'Success Audit', @source=@programname, @eventcode=N'Remove', @category=N'Snapshot', @description=@str, @connectionname = @connectionname

		COMMIT TRAN

		-- AFTER REMOVE THE SNAPSHOT, NEED TO UPDATE THE REGISTEREDSERVER SNAPSHOTID COLUMN
		declare @lastsnapshotid int
		declare @lastsnapshottm datetime

		declare @currentstatus nchar(1)
		declare @currentsnapshottm datetime

		set @lastsnapshotid = 0

		if exists (select 1 from serversnapshot where UPPER(connectionname) = UPPER(@connectionname))
		begin
			select @lastsnapshotid = snapshotid, @lastsnapshottm = endtime from serversnapshot where snapshotid = (select max(snapshotid) from serversnapshot where UPPER(connectionname) = UPPER(@connectionname) and status IN ('S', 'W'))
			select @currentstatus = status, @currentsnapshottm = endtime from serversnapshot where snapshotid = (select max(snapshotid) from serversnapshot where UPPER(connectionname) = UPPER(@connectionname))
		end

		update registeredserver set lastcollectionsnapshotid = @lastsnapshotid, lastcollectiontm = @lastsnapshottm, currentcollectiontm = @currentsnapshottm, currentcollectionstatus = @currentstatus where UPPER(connectionname) = UPPER(@connectionname)
	end
GO



