SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_addregisteredserver]'))
drop procedure [dbo].[isp_sqlsecure_addregisteredserver]
GO

CREATE procedure [dbo].[isp_sqlsecure_addregisteredserver] (@connectionname nvarchar(500), @connectionport int, @servername nvarchar(128), @instancename nvarchar(128),
															@loginname nvarchar(128), @loginpassword nvarchar(300), @authmode nvarchar(256),
															@serverlogin nvarchar(256), @serverpassword nvarchar(256),
															@version nvarchar(256), @retentionperiod int=50, @auditfoldersstring nvarchar(max))
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Insert only sql server properties info into the registeredserver table
   -- 	           

	declare @err int
	declare @errmsg nvarchar(500)
	declare @ans int

	declare @programname nvarchar(128)
	select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)

	exec @ans = [isp_sqlsecure_isadmin]

	if (@ans = 0)
	begin
		set @errmsg = N'Error: Insufficient privileges to register SQL Server instance ' + @connectionname
		exec isp_sqlsecure_addactivitylog @activitytype=N'Failure Audit', @source=@programname, @eventcode=N'Add', @category=N'Server', @description=@errmsg, @connectionname = @connectionname
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

	declare @id int
	insert into registeredserver (connectionname, connectionport, servername, instancename, sqlserverlogin, sqlserverpassword, sqlserverauthtype, serverlogin, serverpassword, version, snapshotretentionperiod, auditfoldersstring) 
							values (@connectionname, @connectionport, @servername, @instancename, @loginname, @loginpassword, @authmode, @serverlogin, @serverpassword, @version, @retentionperiod, @auditfoldersstring)

	select @err = @@error

	if @err <> 0
	begin
		set @errmsg = 'Error: Failed to insert into registeredserver table with connection name ' + @connectionname
		RAISERROR (@errmsg, 16, 1)
		ROLLBACK TRAN
		return -1
	end

	select @id = @@IDENTITY
	if exists (Select snapshotid from serversnapshot where UPPER(connectionname) = UPPER(@connectionname))
	begin
		update serversnapshot set registeredserverid=@id where UPPER(connectionname) = UPPER(@connectionname)

		update registeredserver set currentcollectionstatus = (select top 1 status from serversnapshot where UPPER(connectionname) = UPPER(@connectionname)),
									currentcollectiontm = (select max(starttime) from serversnapshot where UPPER(connectionname) = UPPER(@connectionname)),
									lastcollectionsnapshotid = (select max(a.snapshotid) from serversnapshot a inner join dbo.getsnapshotlist(null, 0) b on UPPER(a.connectionname) = UPPER(b.connectionname) where UPPER(a.connectionname) = UPPER(@connectionname)),
									lastcollectiontm = (select max(a.starttime) from serversnapshot a inner join dbo.getsnapshotlist(null, 0) b on UPPER(a.connectionname) = UPPER(b.connectionname) where UPPER(a.connectionname) = UPPER(@connectionname))
		where UPPER(connectionname) = UPPER(@connectionname)
	end

	declare @str nvarchar(500)

	set @str = N'Registered SQL Server instance ' + @connectionname

	exec isp_sqlsecure_addactivitylog @activitytype=N'Success Audit', @source=@programname, @eventcode=N'Add', @category=N'Server', @description=@str, @connectionname = @connectionname

	
	COMMIT TRAN

GO
