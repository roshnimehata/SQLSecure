
SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_getdatabaseuserpermissions]'))
drop procedure [dbo].[isp_sqlsecure_getdatabaseuserpermissions]
GO

CREATE procedure [dbo].[isp_sqlsecure_getdatabaseuserpermissions] (@connectionname nvarchar(400), @database nvarchar(256), @rundate datetime=null, @usertablesonly bit=0, @includesource bit=0)
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Return all permissions for all users who can access a database
   -- 	           

	declare @snapshotid int, @logintype nchar(1), @sid varbinary(85), @name nvarchar(128), @databaseid int, @errmsg nvarchar(500)


	-- get the snapshotid for the server and rundate
	-- if the server is not valid then return error
	if not exists (select * from serversnapshot where  upper(connectionname) = upper(@connectionname))
	begin
		set @errmsg = 'Error: SQL Server ' + @connectionname + ' not valid'
		RAISERROR (@errmsg, 16, 1)
		return -1
	end

	select @snapshotid=snapshotid from dbo.getsnapshotlist(@rundate, 0) where upper(connectionname) = upper(@connectionname)

	-- if there is not a valid snapshot then return error
	if (@snapshotid is null)
	begin
		set @errmsg = 'Error: A valid snapshot was not found for the specified run date'
		RAISERROR (@errmsg, 16, 1)
		return -1
	end


	-- get the databaseid
	-- if the database is not valid then return error
	if (@database IS NOT NULL and @database <> '')
	begin
		if exists (select * from serversnapshot where snapshotid = @snapshotid and casesensitivemode = 'Y')
		begin

			if not exists (select 1 from sqldatabase where snapshotid = @snapshotid and CONVERT(varbinary, databasename) = CONVERT(varbinary, @database))
			begin
				set @errmsg = 'Error: Invalid database name'
				RAISERROR (@errmsg, 16, 1)
				return
			end 
		end
		else
		begin
			if not exists (select 1 from sqldatabase where snapshotid = @snapshotid and UPPER(databasename) = UPPER(@database))
			begin
				set @errmsg = 'Error: Invalid database name'
				RAISERROR (@errmsg, 16, 1)
				return
			end 
		end
	end

	select @databaseid = dbid from sqldatabase where snapshotid = @snapshotid and databasename = @database

	-- pull the logins to a temp table to combine from the different sources and eliminate duplicates
	create table #tmplogin ([type] nchar(1), [sid] varbinary(85), [name] nvarchar(128))

	-- get a list of all the serverprincipals that have directly defined access to the database
	insert into #tmplogin
		select distinct p.type, p.sid, p.name
			from databaseprincipal u
				inner join serverprincipal p on (u.snapshotid = p.snapshotid and u.usersid = p.sid)
				inner join sqldatabase d on (u.snapshotid = d.snapshotid and u.dbid = d.dbid)
			where d.snapshotid=@snapshotid
				and d.dbid=@databaseid
	-- add all the sysadmins not already included
	insert into #tmplogin
		select p.type, p.sid, p.name
			from serverprincipal p
				inner join serverrolemember m on (m.snapshotid = p.snapshotid and m.memberprincipalid = p.principalid)
			where p.snapshotid=@snapshotid
				and m.principalid = dbo.getserverroleprincipalid(N'sysadmin')
				and p.sid not in (select [sid] from #tmplogin)
	-- add all windows accounts not already included
	insert into #tmplogin
		select 'W' as [type], p.sid, p.name
			from windowsaccount p
			where p.snapshotid=@snapshotid
				and p.sid not in (select [sid] from #tmplogin)

	-- loop through the logins getting the permissions for each one into the #tmpserverpermission table
	declare @type nchar(1), @inputsid varbinary(85), @sqllogin nvarchar(400)
	create table #tmpserverpermission (snapshotid int, permissionlevel nvarchar(15), logintype nchar(1), loginname nvarchar(256), connectionname nvarchar(400), databasename nvarchar(256), principalid int, principalname nvarchar(128), principaltype nchar(1), databaseprincipal nvarchar(128), databaseprincipaltype nchar(1), grantor int, grantorname nvarchar(128), grantee int, granteename nvarchar(128), classid int, permissiontype nvarchar(4), coveringfrom nvarchar(15), permission nvarchar(256), isgrant nchar(1), isgrantwith nchar(1), isrevoke nchar(1), isdeny nchar(1), parentobjectid int, objectid int, objectname nvarchar(256), qualifiedname nvarchar(256), objecttype nvarchar(64), schemaid int, schemaname nvarchar(128), owner int, ownername nvarchar(128), isaliased nchar(1), inherited nchar(1), sourcename nvarchar(256), sourcetype nvarchar(256), sourcepermission nvarchar(256))

	-- used for tracking statistics
	declare @exectime datetime, @count int, @lastcount int
	select @lastcount=0

	declare mycursor cursor for
		select [type], [sid], [name] from #tmplogin
	open mycursor
	fetch next from mycursor into @logintype, @sid, @name

	while (@@fetch_status = 0)
	begin
		select @type=case @logintype when N'S' then N'S' else N'W' end
		select @inputsid=case @type when N'S' then NULL else @sid end,
				@sqllogin=case when @type = N'S' then @name else NULL end
		-- used for tracking statistics
		print 'Processing login ' + @name + '   type=' + @type + ' (' + @logintype + ')'
		select @exectime = getdate(), @count=0

		exec [dbo].[isp_sqlsecure_getuserpermission]
				@snapshotid=@snapshotid,
				@logintype=@type,
				@inputsid=@inputsid,
				@sqllogin=@sqllogin,
				@databasename=@database,
				@permissiontype=N'E'

		-- used for tracking statistics
		select @count=count(*) from #tmpserverpermission
		print '    Added ' + right(space(6) + cast(@count-@lastcount as nvarchar), 7) + ' records in ' + right(space(8) + cast(datediff(ms, @exectime, getdate()) as nvarchar), 9) + ' ms'
		select @lastcount = @count

		fetch next from mycursor into @logintype, @sid, @name
	end

	close mycursor
	deallocate mycursor


	--if user tables and views only, remove all objects except tables, views, columns and the database and server
	if (@usertablesonly = 1)
		delete from #tmpserverpermission
			where permissionlevel='SCH'
					or (permissionlevel='OBJ' and objecttype not in ('V', 'U', 'iCO'))
					or (permissionlevel='SV' and objecttype not in ('iSRV'))
					--or (permissionlevel='OBJ' and objecttype = ('V') and ownername in ('sys', 'INFORMATION_SCHEMA'))

	declare @sql nvarchar(4000)
	select @sql = 'select distinct
						snapshotid, 
						permissionlevel,
						logintype, 
						loginname, 
						connectionname, 
						databasename, 
						principalid, 
						principalname, 
						principaltype,
						databaseprincipal, 
						databaseprincipaltype, 
						grantor, 
						grantorname,
						grantee, 
						granteename,
						classid, 
						permissiontype, 
						permission, 
						isgrant, 
						isgrantwith, 
						isrevoke, 
						isdeny, 
						parentobjectid,
						objectid,
						objectname, 
						qualifiedname,
						objecttype=CASE WHEN a.objecttype IS NULL THEN dbo.getclassobjecttype(classid) ELSE a.objecttype END, 
						schemaid, 
						schemaname,
						owner,
						ownername,
						isaliased,
						objecttypename=CASE WHEN b.objecttypename IS NULL THEN dbo.getclasstype(classid) ELSE b.objecttypename END,
						inherited'

	if (@includesource = 1)
		select @sql = @sql + ',
						sourcename,
						sourcetype,
						sourcetypename=CASE WHEN c.objecttypename IS NULL THEN sourcetype ELSE c.objecttypename END,
						sourcepermission'

	select @sql = @sql + ' from #tmpserverpermission a
							left outer join objecttype b on a.objecttype = b.objecttype'

	if (@includesource = 1)
		select @sql = @sql + ' left outer join objecttype c on a.sourcetype = c.objecttype'

	select @sql = @sql + ' order by logintype, loginname, permissionlevel, objecttype, objectname, permission'

	exec (@sql)

	drop table #tmpserverpermission

	drop table #tmplogin

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_getdatabaseuserpermissions] TO [SQLSecureView]

GO
