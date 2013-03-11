SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_updateallcredentials]'))
drop procedure [dbo].[isp_sqlsecure_updateallcredentials]
GO

CREATE procedure [dbo].[isp_sqlsecure_updateallcredentials] (@serverlogin nvarchar(128), @serverpassword nvarchar(128))
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Update all registered server credentials.   The credentials are used to connect to a SQL Server and also to get group memberships.
   -- 	           This will replace any blank credentials on any registered server with the specified credentials after upgrading to version 2.0.

	declare @err int
	declare @errmsg nvarchar(500)
	declare @ans int, @str nvarchar(500)
	declare @registeredserverid int, @connectionname nvarchar(400), @setsql bit, @setos bit

	declare @programname nvarchar(128)
	select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)

	exec @ans = [isp_sqlsecure_isadmin]

	if (@ans = 0)
	begin
		set @errmsg = 'Error: Insufficient privileges to update credentials for SQL Server instance ' + @connectionname
		exec isp_sqlsecure_addactivitylog @activitytype=N'Failure Audit', @source=@programname, @eventcode=N'Update All Credentials', @category=N'Server', @description=@errmsg, @connectionname = null
		RAISERROR (@errmsg, 16, 1)
		return -1
	end

	declare servercursor cursor static for
		select 
			registeredserverid, 
			connectionname, 
			case when len(sqlserverlogin) = 0 then 1 else 0 end, 
			case when len(serverlogin) = 0 then 1 else 0 end
		from registeredserver
		where (sqlserverauthtype <> 'S' and len(sqlserverlogin) = 0)
				or len(serverlogin) = 0
	open servercursor

	fetch next from servercursor into @registeredserverid, @connectionname, @setsql, @setos
	while @@fetch_status = 0
	begin
		set @str = N''
		if (@setsql = 1)
		begin
			update registeredserver set sqlserverlogin = @serverlogin, sqlserverpassword = @serverpassword, sqlserverauthtype = N'W' where registeredserverid = @registeredserverid
			select @err = @@error
				
			set @str = N'SQL Server'
		end

		if (@setos = 1 and @err = 0)
		begin
			update registeredserver set serverlogin = @serverlogin, serverpassword = @serverpassword where registeredserverid = @registeredserverid
			select @err = @@error

			set @str = @str + case when len(@str) > 0 then N' and ' else N'' end + N'Operating System'
		end

		if @err <> 0
		begin
			set @errmsg = 'Error: Failed to update registeredserver table with credentials information'
			RAISERROR (@errmsg, 16, 1)
			return -1
		end
		else
		begin
			set @str = N'Updated empty ' + @str + N' credentials for SQL Server instance ' + @connectionname
		end

		exec isp_sqlsecure_addactivitylog @activitytype=N'Success Audit', @source=@programname, @eventcode=N'Update Credentials', @category=N'Server', @description=@str, @connectionname = @connectionname

		fetch next from servercursor into @registeredserverid, @connectionname, @setsql, @setos
	end

	close servercursor
	deallocate servercursor

GO

