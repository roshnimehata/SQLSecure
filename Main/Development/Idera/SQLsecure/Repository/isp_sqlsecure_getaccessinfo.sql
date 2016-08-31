SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_getaccessinfo]'))
drop procedure [dbo].[isp_sqlsecure_getaccessinfo]
GO

CREATE procedure [dbo].[isp_sqlsecure_getaccessinfo] 
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get all logins in the repository and get all the SQLsecure login information

	create table #tmplogins (sid varbinary(85), loginname nvarchar(500), logintype nvarchar(32), serveraccess nvarchar(16), applicationpermission nvarchar(16))

	declare @sid varbinary(85)
	declare @loginname nvarchar(500)
	declare @logintype nvarchar(32)
	declare @access nvarchar(16)
	declare @applicationpermission nvarchar(16)
	declare @uid smallint

	declare @guestenabled nchar(1)

	-- checks if sqlsecure database exists, then no one has permission
	if not exists (select 1 from [master].[dbo].[sysdatabases] where UPPER(name) = 'SQLSECURE')
	begin
		insert into #tmplogins (sid, loginname, logintype, serveraccess, applicationpermission)
		select sid, name, logintype= CASE WHEN isntname = 1 THEN CASE WHEN isntgroup = 1 THEN 'Windows Group' ELSE 'Windows User' END  ELSE 'SQL Server Login' END, accesstype=CASE WHEN denylogin = 1 or hasaccess = 0 THEN 'Deny' ELSE 'Grant' END, permission='None' from [master].[dbo].[syslogins]

		select * from #tmplogins
		return
	end

	-- checks if guest user is enabled
	if exists (select * from [SQLsecure].[dbo].[sysusers] where UPPER(name) = 'GUEST' and status = 2)
	begin
		set @guestenabled = 'Y'
	end

	declare cursor1 cursor for
		select sid, name, logintype= CASE WHEN isntname = 1 THEN CASE WHEN isntgroup = 1 THEN 'Windows Group' ELSE 'Windows User' END  ELSE 'SQL Server Login' END, accesstype=CASE WHEN denylogin = 1 or hasaccess = 0 THEN 'Deny' ELSE 'Grant' END, permission=CASE WHEN sysadmin = 1 THEN 'Permit' ELSE 'None' END from [master].[dbo].[syslogins]
	
	open cursor1
	fetch next from cursor1
	into @sid, @loginname, @logintype, @access, @applicationpermission
	
	while @@fetch_status = 0
	begin
		if (@loginname = SUSER_SNAME(0x01))
		begin
			insert into #tmplogins (sid, loginname, logintype, serveraccess, applicationpermission) values 
			(@sid, @loginname, @logintype, @access, 'Permit')			
		end
		else if (@applicationpermission = 'Permit')
		begin
			insert into #tmplogins (sid, loginname, logintype, serveraccess, applicationpermission) values 
			(@sid, @loginname, @logintype, @access, @applicationpermission)			
		end

		else
		begin

			-- check if access to sqlsecure database
			if exists (select 1 from [SQLsecure].[dbo].[sysusers] b where b.sid = @sid and b.hasdbaccess = 1)
			begin
				-- check if user has db_owner role, if so then permit, otherwise readonly permission
				select @uid = uid from [SQLsecure].[dbo].[sysusers] where sid = @sid 
	
				if exists (select 1 from [SQLsecure].[dbo].[sysmembers] a, [SQLsecure].[dbo].[sysusers] b where a.memberuid = @uid and a.groupuid = b.uid and UPPER(b.name) = 'DB_OWNER')
				begin
					insert into #tmplogins (sid, loginname, logintype, serveraccess, applicationpermission) values 
					(@sid, @loginname, @logintype, @access, 'Readonly')			
				end
				else if exists (select 1 from [SQLsecure].[dbo].[sysmembers] a, [SQLsecure].[dbo].[sysusers] b where a.memberuid = @uid and a.groupuid = b.uid and UPPER(b.name) = 'SQLSECUREVIEW')
				begin
					insert into #tmplogins (sid, loginname, logintype, serveraccess, applicationpermission) values 
					(@sid, @loginname, @logintype, @access, 'Readonly')			
				end
				else 
				begin
					insert into #tmplogins (sid, loginname, logintype, serveraccess, applicationpermission) values 
					(@sid, @loginname, @logintype, @access, 'None')			
				end
			end	
			else
			begin
				-- if not a database user but guest still have permission
				if (@guestenabled = 'Y')
				begin
					insert into #tmplogins (sid, loginname, logintype, serveraccess, applicationpermission) values 
					(@sid, @loginname, @logintype, @access, 'ReadOnly')
				end
				else 
				begin
					insert into #tmplogins (sid, loginname, logintype, serveraccess, applicationpermission) values 
					(@sid, @loginname, @logintype, @access, 'None')
				end
	
			end
		end

	fetch next from cursor1
	into @sid, @loginname, @logintype, @access, @applicationpermission

	end

	close cursor1
	deallocate cursor1

	select * from #tmplogins
	
GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_getaccessinfo] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

