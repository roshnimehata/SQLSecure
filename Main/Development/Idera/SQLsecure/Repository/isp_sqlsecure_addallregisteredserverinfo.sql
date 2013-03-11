SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_addallregisteredserverinfo]'))
drop procedure [dbo].[isp_sqlsecure_addallregisteredserverinfo]
GO

CREATE procedure [dbo].[isp_sqlsecure_addallregisteredserverinfo] (@connectionname nvarchar(500), @servername nvarchar(128), @instancename nvarchar(128), @loginname nvarchar(128), @loginpassword nvarchar(300), @authmode nvarchar(1), @loginauthmode nvarchar(1), @os nvarchar(512), @version nvarchar(256), @edition nvarchar(256), @enableproxyaccount nchar(1), @enablec2 nchar(10), @ownerchaining nchar(1), @serverlogin nvarchar(256), @serverpassword nvarchar(256), @retentionperiod int=50)
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              
   -- 	           

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


	BEGIN TRAN

		if exists (select * from registeredserver where UPPER(connectionname) = UPPER(@connectionname))
		begin
			set @errmsg = 'Error: Connection name ' + @connectionname + ' already exists' 
			RAISERROR (@errmsg, 16, 1)
			ROLLBACK TRAN
			return -1
		end

		insert into registeredserver (connectionname, servername, instancename, sqlserverlogin, sqlserverpassword, sqlserverauthtype, authenticationmode, os, version, edition, enableproxyaccount, enablec2audittrace, crossdbownershipchaining, serverlogin, serverpassword, snapshotretentionperiod) values (@connectionname, @servername, @instancename, @loginname, @loginpassword, @loginauthmode, @authmode, @os, @version, @edition, @enableproxyaccount, @enablec2, @ownerchaining, @serverlogin, @serverpassword, @retentionperiod)

		select @err = @@error

		if @err <> 0
		begin
			set @errmsg = 'Error: Failed to insert into registeredserver table with connection name ' + @connectionname
			RAISERROR (@errmsg, 16, 1)
			ROLLBACK TRAN
			return -1
		end

		if exists (Select snapshotid from serversnapshot where UPPER(connectionname) = UPPER(@connectionname))
		begin
			update registeredserver set currentcollectionstatus = (select top 1 status from serversnapshot where UPPER(connectionname) = UPPER(@connectionname)),
										currentcollectiontm = (select max(starttime) from serversnapshot where UPPER(connectionname) = UPPER(@connectionname)),
										lastcollectionsnapshotid = (select max(a.snapshotid) from serversnapshot a inner join dbo.getsnapshotlist(null, 0) b on UPPER(a.connectionname) = UPPER(b.connectionname) where UPPER(a.connectionname) = UPPER(@connectionname)),
										lastcollectiontm = (select max(a.starttime) from serversnapshot a inner join dbo.getsnapshotlist(null, 0) b on UPPER(a.connectionname) = UPPER(b.connectionname) where UPPER(a.connectionname) = UPPER(@connectionname))
			where UPPER(connectionname) = UPPER(@connectionname)
		end


		declare @str nvarchar(500)
		set @str = N'Added ' + @connectionname + N' SQL Server'

		declare @programname nvarchar(128)
		select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)

		exec isp_sqlsecure_addactivitylog @activitytype=N'Success Audit', @source=@programname, @eventcode=N'ADD', @category=N'SERVER', @description=@str, @connectionname = @connectionname

	COMMIT TRAN

GO
