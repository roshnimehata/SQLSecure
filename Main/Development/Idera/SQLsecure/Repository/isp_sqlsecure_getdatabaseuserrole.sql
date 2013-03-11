SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_getdatabaseuserrole]'))
drop procedure [dbo].[isp_sqlsecure_getdatabaseuserrole]
GO

CREATE procedure [dbo].[isp_sqlsecure_getdatabaseuserrole] (@snapshotid int, @logintype nchar(1), @inputsid varbinary(85), @sqllogin nvarchar(128), @databasename nvarchar(256))
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get all the database roles for the given Windows login or sql login

	declare @loginname nvarchar(128)
	declare @errmsg nvarchar(500)
	declare @databaseid int

	-- if there is not such snapshotid then return error
	if not exists (select * from serversnapshot where snapshotid = @snapshotid)
	begin
		set @errmsg = 'Error: Snapshot id ' + CONVERT(varchar(10), @snapshotid) + ' not found'
		RAISERROR (@errmsg, 16, 1)
		return -1
	end

	declare @iscasesensitive nchar(1)
	set @iscasesensitive = 'N'

	select @iscasesensitive = casesensitivemode from serversnapshot where snapshotid = @snapshotid

	set @databaseid = -1

	if (@databasename IS NOT NULL and @databasename <> '')
	begin
		if (@iscasesensitive = 'Y')
		begin
			if not exists (select 1 from sqldatabase where snapshotid = @snapshotid and CONVERT(varbinary, databasename) = CONVERT(varbinary, @databasename))
			begin
				set @errmsg = 'Error: Invalid database name'
				RAISERROR (@errmsg, 16, 1)
				return
			end 
		end
		else
		begin
			if not exists (select 1 from sqldatabase where snapshotid = @snapshotid and UPPER(databasename) = UPPER(@databasename))
			begin
				set @errmsg = 'Error: Invalid database name'
				RAISERROR (@errmsg, 16, 1)
				return
			end 

		end
	end

	select @databaseid = dbid from sqldatabase where snapshotid = @snapshotid and databasename = @databasename

	create table #tmplogins (sid varbinary(85), principalid int, name nvarchar(128), type nchar(1), serveraccess nchar(1), serverdeny nchar(1), disabled nchar(1))

	if (@logintype = 'W')
	begin
		-- checks if the login exists in sql server
		if exists (select * from windowsaccount where snapshotid = @snapshotid and sid = @inputsid)
		begin
	
			create table #tmpsid (sid varbinary(85))
		
			-- insert current login to tmp table
			insert into #tmplogins (sid, principalid, name, type, serveraccess, serverdeny, disabled) (select sid, principalid, name, type, serveraccess, serverdeny, disabled from serverprincipal where snapshotid = @snapshotid and sid = @inputsid)
		
			-- get all windows parents groups
			insert into #tmpsid exec isp_sqlsecure_getwindowsgroupparents @snapshotid, @inputsid
		
			-- insert all groups in serverprincipal table
			insert into #tmplogins (sid, principalid, name, type, serveraccess, disabled) (select a.sid, a.principalid, a.name, a.type, a.serveraccess, a.disabled from serverprincipal a, #tmpsid b where a.serveraccess = 'Y' and a.serverdeny = 'N' and a.snapshotid = @snapshotid and a.sid = b.sid)

			select @loginname = name from serverprincipal where sid = @inputsid

			drop table #tmpsid				
			end 
	end
	else -- sql login type
	begin
		insert into #tmplogins (sid, principalid, name, type, serveraccess, serverdeny, disabled) (select a.sid, a.principalid, a.name, a.type, a.serveraccess, a.serverdeny, a.disabled from serverprincipal a where a.snapshotid = @snapshotid and name=@sqllogin)
		set @loginname = @sqllogin
	end

	-- Create a temp table to store all database users information

	create table #tmpdbusers (snapshotid int, sid varbinary(85), serverprincipalname nvarchar(400), serverprincipaltype nchar(1), serveraccess nchar(1), dbid int, databaseprincipalname nvarchar(400), databaseprincipaltype nchar(1), isalias nchar(1), role nvarchar(64), hasalias nchar(1))

	-- if there is no user or invalid user then return nothing
	if not exists (select 1 from #tmplogins)
	begin
		select * from #tmpdbusers
		return
	end

	-- check if user 'guest' is valid. If so, then current login will have public database role even there is
   	-- no databse user map to it.
	if exists (select * from databaseprincipal a where UPPER(a.name) = 'GUEST' and UPPER(a.hasaccess) ='Y' and a.snapshotid = @snapshotid and a.dbid = @databaseid) 
	begin
		insert into #tmpdbusers (snapshotid, sid, serverprincipalname, serverprincipaltype, serveraccess, dbid, databaseprincipalname, databaseprincipaltype, isalias, role, hasalias)
		(
		select a.snapshotid, a.sid, serverprincipalname, serverprincipaltype, a.serveraccess, a.dbid, databaseprincipalname, databaseprincipaltype, isalias, role, dbo.isuseraliased(a.snapshotid, a.dbid, a.uid) from vwdatabasefixedrole a, #tmplogins b where a.snapshotid = @snapshotid and a.sid = b.sid and a.dbid = @databaseid 
		union
		select @snapshotid, null, null, null, null,  dbid=a.dbid, databaseprincipalname='guest', databaseprincipaltype=a.type, isalias=a.isalias, role=a.name, 'N' from databaseprincipal a where a.snapshotid = @snapshotid and a.dbid = @databaseid and type = 'R' and uid = 0
		union
		select b.snapshotid, b.sid, serverprincipalname=b.name, serverprincipaltype=b.type, b.serveraccess, dbid=a.dbid, databaseprincipalname=c.name, databaseprincipaltype=c.type, isalias=a.isalias, role=null, 'Y' from databaseprincipal a, serverprincipal b, databaseprincipal c where a.snapshotid = @snapshotid and a.dbid = @databaseid and a.isalias = 'Y' and a.usersid = b.sid and b.snapshotid = a.snapshotid and c.snapshotid = a.snapshotid and c.dbid = a.dbid and c.uid = a.altuid and b.name IN (select name from #tmplogins)
		)
	end
	else
	begin
		insert into #tmpdbusers (snapshotid, sid, serverprincipalname, serverprincipaltype, serveraccess, dbid, databaseprincipalname, databaseprincipaltype, isalias, role, hasalias)
		(
		select a.snapshotid, a.sid, serverprincipalname, serverprincipaltype, a.serveraccess, a.dbid, databaseprincipalname, databaseprincipaltype, isalias, role, dbo.isuseraliased(a.snapshotid, a.dbid, a.uid) from vwdatabasefixedrole a, #tmplogins b where a.snapshotid = @snapshotid and a.sid = b.sid and a.dbid = @databaseid 
		union
		select b.snapshotid, b.sid, serverprincipalname=b.name, serverprincipaltype=b.type, b.serveraccess, dbid=a.dbid, databaseprincipalname=c.name, databaseprincipaltype=c.type, isalias=a.isalias, role=null, 'Y' from databaseprincipal a, serverprincipal b, databaseprincipal c where a.snapshotid = @snapshotid and a.dbid = @databaseid and a.isalias = 'Y' and a.usersid = b.sid and b.snapshotid = a.snapshotid and c.snapshotid = a.snapshotid and c.dbid = a.dbid and c.uid = a.altuid and b.name IN (select name from #tmplogins)
		)
	end

	declare @sid varbinary(85)
	declare @dbid nvarchar(500)
	declare @databaseprincipalname nvarchar(500)
	declare @isalias nchar(1)

	if not exists (select 1 from #tmpdbusers where UPPER(databaseprincipalname) <> 'GUEST')
	begin
		-- if there is no roles then just all a single row for the login and user
		insert into #tmpdbusers (snapshotid, sid, serverprincipalname, serverprincipaltype, serveraccess, dbid, databaseprincipalname, databaseprincipaltype)
		select a.snapshotid, a.sid, a.name, a.type, a.serveraccess, b.dbid, b.name, b.type from serverprincipal a, databaseprincipal b, #tmplogins c where a.snapshotid = @snapshotid and a.sid = b.usersid and b.dbid = @databaseid and b.snapshotid = a.snapshotid and c.principalid = a.principalid and c.sid = a.sid
	end

	declare myc100 cursor for
		select sid, dbid, databaseprincipalname from #tmpdbusers where isalias = 'N' and databaseprincipaltype ='U'
	
	open myc100
	fetch next from myc100
	into @sid, @dbid, @databaseprincipalname
	
	while @@fetch_status = 0
	begin
		if exists (select 1 from databaseprincipal a, databaseprincipal b where b.snapshotid = a.snapshotid and b.dbid = a.dbid and b.altuid = a.uid and b.isalias = 'Y' and a.snapshotid = @snapshotid and a.dbid = @dbid)
		begin
			update #tmpdbusers set hasalias = 'Y' where sid = @sid and dbid = @dbid and databaseprincipalname = @databaseprincipalname
		end

		fetch next from myc100
		into @sid, @dbid, @databaseprincipalname
	end
	
	close myc100
	deallocate myc100	

	select distinct * from #tmpdbusers

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_getdatabaseuserrole] TO [SQLSecureView]

GO

