SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_getuserpermission]'))
drop procedure [dbo].[isp_sqlsecure_getuserpermission]
GO

CREATE procedure [dbo].[isp_sqlsecure_getuserpermission]
(
	@snapshotid int,
	@logintype nchar(1),
	@inputsid varbinary(85),
	@sqllogin nvarchar(128),
	@databasename nvarchar(256),
	@permissiontype nchar(1)=null
)
WITH ENCRYPTION
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --       Get all user permissions given the snapshotid and selected sid (windows account), sql login or database role
   --
   --		A single result set table is returned with all of the permissions for the requested principal and database
   --
   --	After review on 11/01/2007, I found the following - MSO
   --		Note: If the temp table #tmpserverpermission exists on entry, then the result set is written there
   --					and no other table is returned. (It uses the same definition as #tmppermission below)
   --			This temp table result set is different from the standard returned set as follows:
   --					includes the "coveringfrom" column
   --					excludes the objecttypename and sourcetypename columns (see select statement below for how to add them)
   --					objecttype may be null if object is a class (see select statement below to fix this)
   --				 select distinct (other columns),
   --						objecttype=CASE WHEN a.objecttype IS NULL THEN dbo.getclassobjecttype(classid) ELSE a.objecttype END, 
   --						objecttypename=CASE WHEN b.objecttypename IS NULL THEN dbo.getclasstype(classid) ELSE b.objecttypename END,
   --						sourcetypename=CASE WHEN c.objecttypename IS NULL THEN sourcetype ELSE c.objecttypename END
   --					from #tmppermission a left outer join objecttype b on a.objecttype = b.objecttype
   --						left outer join objecttype c on a.sourcetype = c.objecttype
   --		Note: If there is no user or an invalid user, it returns an empty result set that is different from the standard result set
   --					because it includes the "coveringfrom" column
	set nocount on
	declare @loginname nvarchar(128)
	declare @connectionname nvarchar(400)
	declare @errmsg nvarchar(500)
	declare @sql2000 nvarchar(1)
	declare @geteffective nchar(1)
	declare @issysadminrole nchar(1)
	declare @iscasesensitive nchar(1)
	declare @hasserverdeny nchar(1)

	set @iscasesensitive = 'Y'
	set @issysadminrole = 'N'
	set @hasserverdeny = 'N'

	declare @databaseid int

	if (@permissiontype = 'B' or @permissiontype IS NULL)
		set @geteffective = 'Y'
	else if (@permissiontype = 'X')
		set @geteffective = 'N'
	else if (@permissiontype = 'E')
		set @geteffective = 'Y'

	-- step 1.1
	-- if there is not such snapshotid then return error
	if not exists (select * from serversnapshot where snapshotid = @snapshotid)
	begin
		set @errmsg = 'Error: Snapshot id ' + CONVERT(varchar(10), @snapshotid) + ' not found'
		RAISERROR (@errmsg, 16, 1)
		return -1
	end

	create table #tmplogins (sid varbinary(85), principalid int, name nvarchar(128), type nchar(1), serveraccess nchar(1), serverdeny nchar(1), disabled nchar(1))
	create table #tmppermission (snapshotid int, permissionlevel nvarchar(15), logintype nchar(1), loginname nvarchar(256), connectionname nvarchar(400), databasename nvarchar(256), principalid int, principalname nvarchar(128), principaltype nchar(1), databaseprincipal nvarchar(128), databaseprincipaltype nchar(1), grantor int, grantorname nvarchar(128), grantee int, granteename nvarchar(128), classid int, permissiontype nvarchar(4), coveringfrom nvarchar(15), permission nvarchar(256), isgrant nchar(1), isgrantwith nchar(1), isrevoke nchar(1), isdeny nchar(1), parentobjectid int, objectid int, objectname nvarchar(256), qualifiedname nvarchar(256), objecttype nvarchar(64), schemaid int, schemaname nvarchar(128), owner int, ownername nvarchar(128), isaliased nchar(1), inherited nchar(1), sourcename nvarchar(256), sourcetype nvarchar(256), sourcepermission nvarchar(256))
	create table #tmppermission2 (snapshotid int, permissionlevel nvarchar(15), logintype nchar(1), loginname nvarchar(256), connectionname nvarchar(400), databasename nvarchar(256), principalid int, principalname nvarchar(128), principaltype nchar(1), databaseprincipal nvarchar(128), databaseprincipaltype nchar(1), grantor int, grantorname nvarchar(128), grantee int, granteename nvarchar(128), classid int, permissiontype nvarchar(4), coveringfrom nvarchar(15), permission nvarchar(256), isgrant nchar(1), isgrantwith nchar(1), isrevoke nchar(1), isdeny nchar(1), parentobjectid int, objectid int, objectname nvarchar(256), qualifiedname nvarchar(256), objecttype nvarchar(64), schemaid int, schemaname nvarchar(128), owner int, ownername nvarchar(128), isaliased nchar(1), inherited nchar(1), sourcename nvarchar(256), sourcetype nvarchar(256), sourcepermission nvarchar(256))
	create table #tmpdenypermission (snapshotid int, permissionlevel nvarchar(15), logintype nchar(1), loginname nvarchar(256), connectionname nvarchar(400), databasename nvarchar(256), principalid int, principalname nvarchar(128), principaltype nchar(1), databaseprincipal nvarchar(128), databaseprincipaltype nchar(1), grantor int, grantorname nvarchar(128), grantee int, granteename nvarchar(128), classid int, permissiontype nvarchar(4), coveringfrom nvarchar(15), permission nvarchar(256), isgrant nchar(1), isgrantwith nchar(1), isrevoke nchar(1), isdeny nchar(1), parentobjectid int, objectid int, objectname nvarchar(256), qualifiedname nvarchar(256), objecttype nvarchar(64), schemaid int, schemaname nvarchar(128), owner int, ownername nvarchar(128), isaliased nchar(1), inherited nchar(1), sourcename nvarchar(256), sourcetype nvarchar(256), sourcepermission nvarchar(256))
	
	select @connectionname = connectionname from serversnapshot where snapshotid = @snapshotid
	select @iscasesensitive = casesensitivemode from serversnapshot where snapshotid = @snapshotid

	-- step 1.2
	-- process sa differently
	-- if user is sa, sa has all permissions
	if (UPPER(@logintype) = 'S' and exists (select 1 from serverprincipal where snapshotid = @snapshotid and type = 'S' and sid = 0x01 and name = @sqllogin))
	begin
		insert into #tmppermission (
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
			objectid,
			objectname, 
			qualifiedname,
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			coveringfrom,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		values ( 
			@snapshotid, 
			'SV',
			'S', 
			@sqllogin, 
			@connectionname, 
			NULL, 
			1, 
			@sqllogin, 
			'S', 
			NULL, 
			NULL, 
			1, 
			@sqllogin, 
			1, 
			@sqllogin, 
			100, 
			'EX',
			'CONTROL', 
			'Y', 
			'Y',
			'N', 
			'N', 
			1,
			@connectionname,
			@connectionname,
			'iSRV',
			NULL, 
			NULL,
			NULL,
			NULL,
			'N',
			'SV',
			'N',
			@sqllogin,
			'iLOGN',
			'CONTROL'
			)

		insert into #tmppermission (
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
			objectid,
			objectname, 
			qualifiedname,
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			coveringfrom,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		values ( 
			@snapshotid, 
			'SV',
			'S', 
			@sqllogin, 
			@connectionname, 
			NULL, 
			1, 
			@sqllogin, 
			'S', 
			NULL, 
			NULL, 
			1, 
			@sqllogin, 
			1, 
			@sqllogin, 
			100, 
			'EF',
			'CONTROL', 
			'Y', 
			'Y',
			'N', 
			'N', 
			1,
			@connectionname,
			@connectionname,
			'iSRV',
			NULL, 
			NULL,
			NULL,
			NULL,
			'N',
			'SV',
			'N',
			@sqllogin,
			'iLOGN',
			'CONTROL')

		insert into #tmppermission (
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
			objectid,
			objectname, 
			qualifiedname,
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			coveringfrom,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		values ( 
			@snapshotid, 
			'DB',
			'S', 
			@sqllogin, 
			@connectionname, 
			@databasename, 
			1, 
			@sqllogin, 
			'S', 
			NULL, 
			NULL, 
			1, 
			@sqllogin, 
			1, 
			@sqllogin, 
			0, 
			'EF',
			'CONTROL', 
			'Y', 
			'Y',
			'N', 
			'N', 
			1,
			@databasename,
			@databasename,
			'DB',
			NULL, 
			NULL,
			NULL,
			NULL,
			'Y',
			'SV',
			'N',
			@sqllogin,
			'iLOGN',
			'CONTROL')

		if (UPPER(@permissiontype) = 'X')
		begin
			delete from #tmppermission where permissiontype = 'EF'
		end

		if (UPPER(@permissiontype) = 'E')
		begin
			delete from #tmppermission where permissiontype = 'EX'
		end

		if exists (select 'x' from tempdb..sysobjects where type = 'U' and lower(name) like '#tmpserverpermission%')
		begin
			insert into #tmpserverpermission select distinct * from #tmppermission
		end
		else -- else just show all data, the caller is UI
		begin
		-- testing show all permission
		exec ('select distinct 
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
				inherited,
				sourcename,
				sourcetype,
				sourcetypename=CASE WHEN c.objecttypename IS NULL THEN sourcetype ELSE c.objecttypename END,
				sourcepermission
				from #tmppermission a left outer join objecttype b on a.objecttype = b.objecttype
									left outer join objecttype c on a.sourcetype = c.objecttype
				')

		end

		return
	end

	-- step 1.3
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

	-- check if the snapshot server is sql 2000 or sql 2005 and greater, it is important to differentiate them for getting column permission processing
	select @sql2000 = dbo.issql2000(@snapshotid)

	-- step 1.4.1
	if (@logintype = 'W')
	begin
	
		create table #tmpsid (sid varbinary(85))
	
		-- insert current login to tmp table
		insert into #tmplogins (sid, principalid, name, type, serveraccess, serverdeny, disabled) (select distinct  sid, principalid, name, type, serveraccess, serverdeny, disabled from serverprincipal where snapshotid = @snapshotid and sid = @inputsid)
	
		-- get all windows parents groups
		insert into #tmpsid exec isp_sqlsecure_getwindowsgroupparents @snapshotid, @inputsid
	
		-- insert all groups in serverprincipal table
		insert into #tmplogins (sid, principalid, name, type, serveraccess, disabled) (select  distinct  a.sid, a.principalid, a.name, a.type, a.serveraccess, a.disabled from serverprincipal a, #tmpsid b where a.snapshotid = @snapshotid and a.sid = b.sid)

		select @loginname = name from serverprincipal where sid = @inputsid

		drop table #tmpsid				

	end
	else if (@logintype = 'S') -- sql login type
	begin
	-- step 1.4.2
		if (@iscasesensitive = 'Y')
			insert into #tmplogins (sid, principalid, name, type, serveraccess, serverdeny, disabled) (select  distinct  a.sid, a.principalid, a.name, a.type, a.serveraccess, a.serverdeny, a.disabled from serverprincipal a where a.snapshotid = @snapshotid and type='S' and CONVERT(VARBINARY, name)=CONVERT(VARBINARY, @sqllogin))
		else
			insert into #tmplogins (sid, principalid, name, type, serveraccess, serverdeny, disabled) (select  distinct  a.sid, a.principalid, a.name, a.type, a.serveraccess, a.serverdeny, a.disabled from serverprincipal a where a.snapshotid = @snapshotid and type='S' and UPPER(name)=UPPER(@sqllogin))

		set @loginname = @sqllogin
	end
	else if (@logintype = 'R') -- database role
	begin
	-- step 1.4.3
		set @loginname = @sqllogin
	end

	-- skip processing server level for roles
	if (@logintype <> 'R')
	begin
		-- if there is no user or invalid user then return nothing
		if not exists (select 1 from #tmplogins)
		begin
			select *, objecttypename=NULL, sourcetypename=null from #tmppermission
			return
		end	

		--select * from #tmplogins

		-- step 1.5
		-- checks if any of the group or users have server deny 
		if exists (select 1 from #tmplogins where serverdeny = 'Y')
		begin
			set @hasserverdeny = 'Y'
		end

		-- step 1.6
		create table #tmpresult (result nchar(1))

		exec isp_sqlsecure_issysadmin @snapshotid=@snapshotid, @logintype=@logintype, @inputsid=@inputsid, @sqllogin=@sqllogin

		select @issysadminrole = result from #tmpresult
	
		-- step 1.7
		-- GET SERVER PERMISSIONS
		insert into #tmppermission 
			(snapshotid, 
			permissionlevel, 
			logintype, 
			loginname, 
			connectionname, 
			databasename, 
			principalid, 
			principalname, 
			principaltype, 
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
			objectid,
			objectname, 
			objecttype,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission) 
		select distinct 
			a.snapshotid, 
			'SV',
			@logintype, 
			@loginname, 
			@connectionname, 
			NULL, 	
			a.principalid, 
			a.name, 
			a.type, 
			d.grantor, 
			dbo.getserverprincipalname(d.snapshotid, d.grantor),
			d.grantee, 
			dbo.getserverprincipalname(d.snapshotid, d.grantee),
			d.classid, 
			'EX', 
			d.permission, 
			d.isgrant, 
			d.isgrantwith, 
			d.isrevoke, 
			d.isdeny, 
			d.majorid,
			@connectionname, 
			dbo.getclassobjecttype(d.classid),
			'N',
			@connectionname,
			'iSRV',
			d.permission
		from 
			serverprincipal a, 
			serverpermission d 
		where 
			a.snapshotid = @snapshotid and 
			d.snapshotid = a.snapshotid and
			d.grantee = a.principalid and 
			d.classid = 100 and 
			exists ( select 1 from #tmplogins f 
					left join serverrolemember g
						on g.snapshotid = @snapshotid and  g.memberprincipalid = f.principalid
				where a.principalid = f.principalid or a.principalid = g.principalid
					)
	end

	-- step 1.8.1
	create table #tmpuid (dbid int, uid int)
	
	declare @dbid int
	declare @uid int

	if (@logintype = 'R')
		-- handle case sensitivity for role names
		if (@iscasesensitive = 'Y')
			declare myc100 cursor for
					select distinct a.dbid, a.uid from databaseprincipal a where a.snapshotid = @snapshotid and a.dbid = @databaseid and a.name = @loginname
		else
			declare myc100 cursor for
					select distinct a.dbid, a.uid from databaseprincipal a where a.snapshotid = @snapshotid and a.dbid = @databaseid and UPPER(a.name) = UPPER(@loginname)
	else
		declare myc100 cursor for
				select distinct a.dbid, a.uid from databaseprincipal a, #tmplogins b where a.snapshotid = @snapshotid and a.usersid = b.sid and a.dbid = @databaseid

	open myc100
	fetch next from myc100
	into @dbid, @uid
	
	while @@fetch_status = 0
	begin
		insert into #tmpuid exec isp_sqlsecure_getdatabaseuserparents @snapshotid, @dbid, @uid

		fetch next from myc100
		into @dbid, @uid

	end
	
	close myc100
	deallocate myc100	

	-- do not add guest permissions or process aliases for roles
	if (@logintype <> 'R')
	begin
		-- step 1.8.2
		-- check if user 'guest' is valid. If so, then current login will have public database role even there is
   		-- no databse user map to it.
		if exists (select * from databaseprincipal a where UPPER(a.name) = 'GUEST' and UPPER(a.hasaccess) ='Y' and a.snapshotid = @snapshotid and a.dbid = @databaseid) 
		begin
			-- public uid is always 0
			insert into #tmpuid (dbid, uid) values (@databaseid, 0)

			-- insert guest user as well
			insert into #tmpuid (dbid, uid) select distinct dbid, uid from databaseprincipal a where UPPER(a.name) = 'GUEST' and snapshotid = @snapshotid and dbid = @databaseid
		end

		-- step 1.8.3
		-- NEED TO ADD ALIAS USER TO THE TMPUID AS WELL
		insert into #tmpuid (dbid, uid) select  distinct  dbid, altuid from databaseprincipal where snapshotid = @snapshotid and isalias = 'Y' and altuid IS NOT NULL and dbid = @databaseid and usersid IN (select  distinct  sid from #tmplogins)
	end

	--select * from #tmpuid

	-- step 1.9
	-- GET DATABASE EXPLICIT PERMISSIONS
	insert into #tmppermission (
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
		objectid,
		objectname, 
		objecttype, 
		schemaid, 
		schemaname,
		owner,
		ownername,
		isaliased,
		inherited,
		sourcename,
		sourcetype,
		sourcepermission) 
	select distinct 
		a.snapshotid, 
		'DB',
		@logintype,
		@loginname,
		@connectionname,
		b.databasename, 
		principalid=e.principalid,
		principalname=e.name,
		principaltype=e.type,
		databaseprincipal=CASE WHEN UPPER(a.isalias) = 'Y' THEN dbo.getdatabaseprincipalname(a.snapshotid, a.dbid, a.altuid) ELSE a.name END, 
		databaseprincipaltype=a.type, 
		d.grantor, 
		dbo.getdatabaseprincipalname(d.snapshotid, d.dbid, d.grantor),
		d.grantee, 
		dbo.getdatabaseprincipalname(d.snapshotid, d.dbid, d.grantee),
		d.classid,
		'EX',
		d.permission,
		d.isgrant, 
		d.isgrantwith, 
		d.isrevoke, 
		d.isdeny,
		b.dbid, 
		b.databasename, 
		'DB', 
		NULL, 
		NULL,
		dbo.getdatabaseprincipalid(d.snapshotid, d.dbid, d.owner),
		b.owner,
		a.isalias,
		'N',
		b.databasename,
		'DB',
		d.permission
	from 
		databaseprincipal a left outer join serverprincipal e on a.snapshotid = e.snapshotid and a.usersid = e.sid, 
		sqldatabase b, 
		#tmpuid c, 
		vwdatabasepermission d
	where 
		a.snapshotid = @snapshotid and 
		b.snapshotid = a.snapshotid and
		b.dbid = a.dbid and 
		(a.uid = c.uid or (UPPER(a.isalias) = 'Y' and a.altuid = c.uid)) and 
		a.dbid = c.dbid and
		d.snapshotid = a.snapshotid and 
		d.classid = 0 and 
		d.grantee = c.uid and
		d.dbid = c.dbid and  
		b.dbid = @databaseid

	-- step 1.10
	-- MSO - Change to check for not 2000 so it will process 2008 as 2005
	if (@sql2000 = 'N')
	begin
		-- do not process server level objects for roles
		if (@logintype <> 'R')
		begin
		-- step 2.01
			-- NEED TO ADDRESS SERVER ACCESS OR DENY IN REGARD TO CONNECT SQL AND VIEW ANY DATABASE PERMISSION
			if (@permissiontype = 'E' or @permissiontype = 'B')
			begin
				if (@hasserverdeny = 'N')
				begin
					insert into #tmppermission (
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
						objectid,
						objectname, 
						qualifiedname,
						objecttype, 
						schemaid, 
						schemaname,
						owner,
						ownername,
						isaliased,
						coveringfrom,
						inherited,
						sourcename,
						sourcetype,
						sourcepermission)
					values ( 
						@snapshotid, 
						'SV',
						@logintype, 
						@loginname, 
						@connectionname, 
						NULL, 
						null, 
						@loginname, 
						@logintype, 
						NULL, 
						NULL, 
						1, 
						'sa', 
						null,
						@loginname, 
						100, 
						'EF',
						'CONNECT SQL', 
						'Y', 
						'N',
						'N', 
						'N', 
						0,
						@connectionname,
						@connectionname,
						'iSRV',
						NULL, 
						NULL,
						NULL,
						NULL,
						'N',
						'SV',
						'N',
						@connectionname,
						'iSRV',
						'CONNECT SQL'
						)
				end
		
			-- step 2.02
				insert into #tmppermission (
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
					objectid,
					objectname, 
					qualifiedname,
					objecttype, 
					schemaid, 
					schemaname,
					owner,
					ownername,
					isaliased,
					coveringfrom,
					inherited,
					sourcename,
					sourcetype,
					sourcepermission)
				values ( 
					@snapshotid, 
					'SV',
					@logintype, 
					@loginname, 
					@connectionname, 
					NULL, 
					null, 
					@loginname, 
					@logintype, 
					NULL, 
					NULL, 
					1, 
					'sa', 
					null,
					@loginname, 
					100, 
					'EF',
					'VIEW ANY DATABASE', 
					'Y', 
					'N',
					'N', 
					'N', 
					0,
					@connectionname,
					@connectionname,
					'iSRV',
					NULL, 
					NULL,
					NULL,
					NULL,
					'N',
					'SV',
					'N',
					@connectionname,
					'iSRV',
					'VIEW ANY DATABASE'
					)
			end

		-- step 2.03
			--GET ALL LOGINS PERMISSION
			insert into #tmppermission 
				(snapshotid, 
				permissionlevel, 
				logintype, 
				loginname, 
				connectionname, 
				databasename, 
				principalid, 
				principalname, 
				principaltype, 
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
				objectname, 
				objectid,
				objecttype,
				inherited,
				sourcename,
				sourcetype,
				sourcepermission) 
			select distinct 
				a.snapshotid, 
				'SV',
				@logintype, 
				@loginname, 
				@connectionname, 
				NULL, 	
				a.principalid, 
				a.name, 
				a.type, 
				d.grantor, 
				dbo.getserverprincipalname(d.snapshotid, d.grantor),
				d.grantee, 
				dbo.getserverprincipalname(d.snapshotid, d.grantee),
				d.classid, 
				'EX', 
				d.permission, 
				d.isgrant, 
				d.isgrantwith, 
				d.isrevoke, 
				d.isdeny, 
				a.name, 
				a.principalid,
				dbo.getclassobjecttype(d.classid),
				'Y',
				a.name,
				'iLOGN',
				d.permission
			from 
				serverprincipal a, 
				serverpermission d 
			where 
				a.snapshotid = @snapshotid and 
				d.snapshotid = a.snapshotid and
				d.grantee = a.principalid and 
				a.type IN ('U', 'G') and 
				d.classid = 101 and 
				(d.grantee in (select  distinct  principalid from #tmplogins) or 
				 (d.grantee in (select  distinct  principalid from serverrolemember where memberprincipalid in (select distinct   principalid from #tmplogins))))
	
		-- step 2.04
			-- ENDPOINT PERMISSION
			-- REQUIRES SOME CUSTOMIZATION BECAUSE THE ENDPOINT COVERING HAS AT LEAST A CONNECT PERMISSION EVEN IF THERE IS NO PARENT PERMISSION
			-- ALSO IF THERE IS NO ENDPOINT EXPLICIT NEED TO CREATE ROWS DUE TO COVERING
			insert into #tmppermission 
				(snapshotid, 
				permissionlevel, 
				logintype, 
				loginname, 
				connectionname, 
				databasename, 
				principalid, 
				principalname, 
				principaltype, 
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
				objectid,
				objectname, 
				objecttype,
				inherited,
				sourcename,
				sourcetype,
				sourcepermission) 
			select distinct 
				a.snapshotid, 
				'SV',
				@logintype, 
				@loginname, 
				@connectionname, 
				NULL,
				a.principalid, 
				dbo.getserverprincipalname(a.snapshotid, a.principalid),
				dbo.getserverprincipaltype(a.snapshotid, a.principalid), 
				d.grantor, 
				dbo.getserverprincipalname(d.snapshotid, d.grantor),
				d.grantee, 
				dbo.getserverprincipalname(d.snapshotid, d.grantee),
				d.classid, 
				'EX', 
				d.permission, 
				d.isgrant, 
				d.isgrantwith, 
				d.isrevoke, 
				d.isdeny, 
				a.endpointid,
				a.name, 
				dbo.getclassobjecttype(d.classid),
				'N',
				a.name,
				'iENDP',
				d.permission
			from 
				endpoint a, 
				serverpermission d 
			where 
				a.snapshotid = @snapshotid and 
				d.snapshotid = a.snapshotid and
				d.majorid = a.endpointid and 
				d.classid = 105 and 
				(d.grantee in (select  distinct  principalid from #tmplogins) or 
				 (d.grantee in (select  distinct  principalid from serverrolemember where memberprincipalid in (select  distinct  principalid from #tmplogins))))
		end

	-- step 2.05
		-- GET ALL THE DATABASE PRINCIPAL PERMISSION
		insert into #tmppermission (
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
			objectid,
			objectname, 
			objecttype, 
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission) 
		select distinct 
			a.snapshotid, 
			'USR',
			@logintype,
			@loginname,
			@connectionname,
			@databasename, 
			e.principalid,
			principalname=e.name,
			principaltype=e.type,
			databaseprincipal=CASE WHEN UPPER(a.isalias) = 'Y' THEN dbo.getdatabaseprincipalname(a.snapshotid, a.dbid, a.altuid) ELSE a.name END, 
			databaseprincipaltype=a.type, 
			d.grantor, 
			dbo.getdatabaseprincipalname(d.snapshotid, d.dbid, d.grantor),
			d.grantee, 
			dbo.getdatabaseprincipalname(d.snapshotid, d.dbid, d.grantee),
			d.classid,
			'EX',
			d.permission,
			d.isgrant, 
			d.isgrantwith, 
			d.isrevoke, 
			d.isdeny,
			d.uid,
			-- 12/29/2006 MSO fix to use uid for object instead of grantee to fix PR#801382
			dbo.getdatabaseprincipalname(d.snapshotid, d.dbid, d.uid),
			dbo.getdatabaseprincipaltype(d.snapshotid, d.dbid, d.uid), 
			-- 12/29/2006 MSO fix owner to always be dbo to fix PR#801382
			1,
			dbo.getdatabaseprincipalname(d.snapshotid, d.dbid, 1),
			a.isalias,
			'N',
			-- 12/29/2006 MSO fix source to be uid instead of grantee to fix PR#801382
			dbo.getdatabaseprincipalname(d.snapshotid, d.dbid, d.uid),
			dbo.getdatabaseprincipaltype(d.snapshotid, d.dbid, d.uid), 
			d.permission
		from
			databaseprincipal a left outer join serverprincipal e on a.snapshotid = e.snapshotid and a.usersid = e.sid, 
			#tmpuid c, 
			databaseprincipalpermission d
		where
			a.snapshotid = @snapshotid and 
			(a.uid = c.uid or (UPPER(a.isalias) = 'Y' and a.altuid = c.uid)) and 
			a.dbid = c.dbid and
			d.snapshotid = a.snapshotid and 
			d.grantee = c.uid and 
			d.dbid = c.dbid and  
			a.dbid = @databaseid

	-- step 2.06
		-- GET ALL THE SCHEMA PERMISSION
		insert into #tmppermission (
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
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission) 
		select distinct 
			a.snapshotid, 
			'SCH',
			@logintype,
			@loginname,
			@connectionname,
			@databasename, 
			e.principalid,
			principalname=e.name,
			principaltype=e.type,
			databaseprincipal=CASE WHEN UPPER(a.isalias) = 'Y' THEN dbo.getdatabaseprincipalname(a.snapshotid, a.dbid, a.altuid) ELSE a.name END, 
			databaseprincipaltype=a.type, 
			d.grantor, 
			dbo.getdatabaseprincipalname(d.snapshotid, d.dbid, d.grantor),
			d.grantee, 
			dbo.getdatabaseprincipalname(d.snapshotid, d.dbid, d.grantee),
			d.classid,
			'EX',
			d.permission,
			d.isgrant, 
			d.isgrantwith, 
			d.isrevoke, 
			d.isdeny,
			d.schemaid,
			d.schemaname,	
			'iSCM', 
			d.schemaid, 
			d.schemaname,	
			d.uid,
			dbo.getdatabaseprincipalname(d.snapshotid, d.dbid, d.uid),
			a.isalias,
			'N',
			d.schemaname,	
			'iSCM', 
			d.permission
		from 
			databaseprincipal a left outer join serverprincipal e on a.snapshotid = e.snapshotid and a.usersid = e.sid, 
			#tmpuid c, 
			vwschemapermission d
		where 
			a.snapshotid = @snapshotid and 
			(a.uid = c.uid or (UPPER(a.isalias) = 'Y' and a.altuid = c.uid)) and -- TODO: CHECKS IF ROLES SHOULD BE CONSIDERED AS EXPLICIT PERMISSION
			a.dbid = c.dbid and
			d.snapshotid = a.snapshotid and 
			d.grantee = c.uid and 
			d.dbid = c.dbid and  
			a.dbid = @databaseid 

	-- step 2.07
		-- GET SQL 2005 ALL OBJECTS EXCEPT COLUMN
		insert into #tmppermission (
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
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission) 
		select distinct 
			a.snapshotid, 
			'OBJ',
			@logintype,
			@loginname,
			@connectionname,
			@databasename, 
			e.principalid,
			principalname=e.name,
			principaltype=e.type,
			databaseprincipal=CASE WHEN UPPER(a.isalias) = 'Y' THEN dbo.getdatabaseprincipalname(a.snapshotid, a.dbid, a.altuid) ELSE a.name END, 
			databaseprincipaltype=a.type, 
			d.grantor, 
			dbo.getdatabaseprincipalname(d.snapshotid, d.dbid, d.grantor),
			d.grantee, 
			dbo.getdatabaseprincipalname(d.snapshotid, d.dbid, d.grantee),
			d.classid,
			'EX',
			d.permission,
			d.isgrant, 
			d.isgrantwith, 
			d.isrevoke, 
			d.isdeny,
			d.objectid,
			d.objectname, 
			d.objecttype, 
			d.schemaid, 
			dbo.getschemaname(d.snapshotid, d.dbid, d.schemaid),
			d.owner,
			dbo.getdatabaseprincipalname(d.snapshotid, d.dbid, d.owner),
			a.isalias,
			'N',
			d.objectname, 
			d.objecttype, 
			d.permission
		from 
			databaseprincipal a left outer join serverprincipal e on a.snapshotid = e.snapshotid and a.usersid = e.sid, 
			#tmpuid c, 
			vwdatabaseobjectpermission d,
			databaseschema f
		where 
			a.snapshotid = @snapshotid and 
			(a.uid = c.uid or (UPPER(a.isalias) = 'Y' and a.altuid = c.uid)) and 
			a.dbid = c.dbid and
			d.snapshotid = a.snapshotid and 
			d.dbid = a.dbid and
			f.snapshotid = d.snapshotid and
			f.dbid = c.dbid and
			d.grantee = c.uid and
			d.schemaid = f.schemaid and
			d.dbid = f.dbid and
			d.snapshotid = f.snapshotid and
			d.schemaid is not null and
			a.dbid = @databaseid

	--select * from #tmppermission where permissionlevel = 'OBJ'	

	-- step 2.08
		-- GET SQL 2005 ALL COLUMN WHERE SCHEMA IS NULL AND OWNER IS NULL
		insert into #tmppermission (
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
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission) 
		select distinct 
			a.snapshotid, 
			'COL',
			@logintype,
			@loginname,
			@connectionname,
			@databasename, 
			e.principalid,
			principalname=e.name,
			principaltype=e.type,
			databaseprincipal=CASE WHEN UPPER(a.isalias) = 'Y' THEN dbo.getdatabaseprincipalname(a.snapshotid, a.dbid, a.altuid) ELSE a.name END, 
			databaseprincipaltype=a.type, 
			d.grantor, 
			dbo.getdatabaseprincipalname(d.snapshotid, d.dbid, d.grantor),
			d.grantee, 
			dbo.getdatabaseprincipalname(d.snapshotid, d.dbid, d.grantee),
			d.classid,
			'EX',
			d.permission,
			d.isgrant, 
			d.isgrantwith, 
			d.isrevoke, 
			d.isdeny,
			d.parentobjectid,
			d.objectid,
			d.objectname, 
			dbo.gettablename(@snapshotid, @databaseid, d.parentobjectid) + '.' + d.objectname,
			d.objecttype,
			d.schemaid, 
			d.schemaname,
			d.owner,
			d.ownername,
			a.isalias,
			'N',
			dbo.gettablename(@snapshotid, @databaseid, d.parentobjectid) + '.' + d.objectname,
			'iCO',
			d.permission
		from 
			databaseprincipal a left outer join serverprincipal e on a.snapshotid = e.snapshotid and a.usersid = e.sid, 
			#tmpuid c, 
			vwsql2005databasecolumnpermission d
		where 
			a.snapshotid = @snapshotid and 
			(a.uid = c.uid or (UPPER(a.isalias) = 'Y' and a.altuid = c.uid)) and 
			a.dbid = c.dbid and
			d.snapshotid = a.snapshotid and 
			d.dbid = a.dbid and
			d.grantee = a.uid and
			a.dbid = @databaseid	


	-- step 2.09
		if (@geteffective = 'Y')
		begin

			-- EFFECTIVE PERMISSION --

			-- PROCESS FIXED SERVER ROLE PERMISSION
			-- CHECK IF LOGINS BELONGS TO FIXED SERVER ROLES, IF SO THEN ASSIGN FIXED PERMISSION TO SERVER LEVEL
			insert into #tmppermission (
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
				objectid,
				objectname, 
				objecttype, 
				schemaid, 
				schemaname,
				owner,
				ownername,
				isaliased,
				coveringfrom,
				inherited,
				sourcename,
				sourcetype,
				sourcepermission)
			select  distinct   a.snapshotid, 
				'SV',
				@logintype, 
				@loginname, 
				@connectionname, 
				NULL, 
				a.memberprincipalid, 
				dbo.getserverprincipalname(a.snapshotid, a.memberprincipalid), 
				dbo.getserverprincipaltype(a.snapshotid, a.memberprincipalid), 
				NULL, 
				NULL, 
				a.principalid, 
				dbo.getserverprincipalname(a.snapshotid, a.principalid), 
				a.memberprincipalid, 
				dbo.getserverprincipalname(a.snapshotid, a.memberprincipalid), 
				100, 
				'EF', 
				rolepermission, 
				c.isgrant, 
				c.isgrantwith, 
				c.isrevoke, 
				c.isdeny, 
				0,
				@connectionname, 
				'iSRV', 
				NULL, 
				NULL,
				NULL,
				NULL,
				'N',
				'FXROLE',
				'N',
				c.rolename,
				'Server Role',
				rolepermission
			from 
				vwfixedserverrolemember a, 
				#tmplogins b ,
				fixedrolepermission c
			where 
				a.snapshotid = @snapshotid and 
				a.memberprincipalid = b.principalid and
				UPPER(a.name) = UPPER(c.rolename) and
				c.roletype = 'S'

		-- step 2.10
			-- IF THERE IS SYSADMIN THEN JUST RETURN CONTROL AT SERVER LEVEL
			if (@issysadminrole = 'Y')
			begin
				declare @syadminprincipal int
				declare @syadminprincipalname nvarchar(128)

				select @syadminprincipal = principalid, @syadminprincipalname = principalname from #tmppermission where permissiontype = 'EF' and UPPER(grantorname) = 'SYSADMIN'			

				delete from #tmppermission where permissiontype = 'EF'

				insert into #tmppermission (
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
					objectid,
					objectname, 
					qualifiedname,
					objecttype, 
					schemaid, 
					schemaname,
					owner,
					ownername,
					isaliased,
					coveringfrom,
					inherited,
					sourcename,
					sourcetype,
					sourcepermission)
				values ( 
					@snapshotid, 
					'SV',
					@logintype, 
					@loginname, 
					@connectionname, 
					NULL, 
					3, 
					'sysadmin', 
					'R', 
					NULL, 
					NULL, 
					3, 
					'sysadmin', 
					@syadminprincipal, 
					@syadminprincipalname, 
					100, 
					'EF',
					'CONTROL', 
					'Y', 
					'Y',
					'N', 
					'N', 
					1,
					@connectionname,
					@connectionname,
					'iSRV',
					NULL, 
					NULL,
					NULL,
					NULL,
					'N',
					'SV',
					'N',
					'sysadmin',
					'Server Role',
					'CONTROL'
					)

				insert into #tmppermission (
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
					objectid,
					objectname, 
					qualifiedname,
					objecttype, 
					schemaid, 
					schemaname,
					owner,
					ownername,
					isaliased,
					coveringfrom,
					inherited,
					sourcename,
					sourcetype,
					sourcepermission)
				values ( 
					@snapshotid, 
					'DB',
					@logintype, 
					@loginname, 
					@connectionname, 
					@databasename, 
					3, 
					'sysadmin', 
					'R', 
					NULL, 
					NULL, 
					3, 
					'sysadmin', 
					@syadminprincipal, 
					@syadminprincipalname, 
					0, 
					'EF',
					'CONTROL', 
					'Y', 
					'Y',
					'N', 
					'N', 
					1,
					@databasename,
					@databasename,
					'DB',
					NULL, 
					NULL,
					NULL,
					NULL,
					'Y',
					'SV',
					'Y',
					'sysadmin',
					'Server Role',
					'CONTROL'
					)

				if (UPPER(@permissiontype) = 'X')
					delete from #tmppermission where permissiontype = 'EF'
		
				if (UPPER(@permissiontype) = 'E')
					delete from #tmppermission where permissiontype = 'EX'
		
				if exists (select 'x' from tempdb..sysobjects where type = 'U' and lower(name) like '#tmpserverpermission%')
				begin
					insert into #tmpserverpermission select distinct * from #tmppermission
				end
				else -- else just show all data, the caller is UI
				begin
	/*	This should be the fix for the following code
					select distinct 
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
						objecttype=isnull(a.objecttype, dbo.getclassobjecttype(classid)), 
						schemaid,
						schemaname,
						owner,
						ownername,
						isaliased,
						objecttypename=isnull(b.objecttypename, dbo.getclasstype(classid)),
						inherited,
						sourcename,
						sourcetype,
						sourcetypename=isnull(c.objecttypename, sourcetype),
						sourcepermission
					from #tmppermission a
						left outer join objecttype b on a.objecttype = b.objecttype
						left outer join objecttype c on a.sourcetype = c.objecttype
					where permissiontype = case upper(@permissiontype)
												when 'E' then 'EF'
												when 'X' then 'EX'
												else permissiontype
											end
	*/

					if (@permissiontype = 'E') 
					begin
						select distinct 
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
							inherited,
							sourcename,
							sourcetype,
							sourcetypename=CASE WHEN c.objecttypename IS NULL THEN sourcetype ELSE c.objecttypename END,
							sourcepermission
						from #tmppermission a
							left outer join objecttype b on a.objecttype = b.objecttype
							left outer join objecttype c on a.sourcetype = c.objecttype
						where permissiontype = 'EF'
					end
					else if (@permissiontype = 'E') 
					begin
						select distinct 
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
							inherited,
							sourcename,
							sourcetype,
							sourcetypename=CASE WHEN c.objecttypename IS NULL THEN sourcetype ELSE c.objecttypename END,
							sourcepermission
						from #tmppermission a
							left outer join objecttype b on a.objecttype = b.objecttype
							left outer join objecttype c on a.sourcetype = c.objecttype
						where permissiontype = 'EX'
					end
					else
					begin
						select distinct 
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
							inherited,
							sourcename,
							sourcetype,
							sourcetypename=CASE WHEN c.objecttypename IS NULL THEN sourcetype ELSE c.objecttypename END,
							sourcepermission
						from #tmppermission a
							left outer join objecttype b on a.objecttype = b.objecttype
							left outer join objecttype c on a.sourcetype = c.objecttype
					end
				end

				return
			end

		-- step 2.11
			-- COPY ALL EXISTING SERVER PERMISSION AS EFFECTIVE PERMISSION
			insert into #tmppermission (
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
				objectid,
				objectname, 
				objecttype, 
				schemaid, 
				schemaname,
				owner,
				ownername,
				isaliased,
				inherited,
				sourcename,
				sourcetype,
				sourcepermission)
			select  distinct   snapshotid, 
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
				'EF', 
				permission, 
				isgrant, 
				isgrantwith, 
				isrevoke, 
				isdeny, 
				objectid,
				objectname, 
				objecttype, 
				schemaid, 
				schemaname,
				owner,
				ownername,
				isaliased,
				inherited,
				sourcename,
				sourcetype,
				sourcepermission
			from #tmppermission
			where UPPER(permissionlevel) = 'SV'

			-- NEED TO BREAK DOWN HIGH LEVEL PERMISSION LIKE CONTROL TO MORE DETAIL PERMISSION PRIOR TO ADDING COVERING PERMISSION TO LOWER LEVEL
			insert into #tmppermission (
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
				objectid,
				objectname, 
				objecttype, 
				schemaid, 
				schemaname,
				owner,
				ownername,
				isaliased,
				inherited,
				sourcename,
				sourcetype,
				sourcepermission)
			select  distinct   snapshotid, 
				a.permissionlevel,
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
				'EF', 
				b.permissionname, 
				isgrant, 
				isgrantwith, 
				isrevoke, 
				isdeny, 
				objectid,
				objectname, 
				objecttype, 
				schemaid, 
				schemaname,
				owner,
				ownername,
				isaliased,
				inherited,
				sourcename,
				sourcetype,
				sourcepermission
			from #tmppermission a, coveringpermissionhierarchy b
			where classid = 100 and b.coveringpermissionname = a.permission

			-- If there is control server, then needs to expand in into more detail permission again.
			-- 2 level expansion needed.
			if exists (select 1 from #tmppermission where permission = 'CONTROL SERVER')
			begin
		
				insert into #tmppermission (
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
					objectid,
					objectname, 
					objecttype, 
					schemaid, 
					schemaname,
					owner,
					ownername,
					isaliased,
					inherited,
					sourcename,
					sourcetype,
					sourcepermission)
				select  distinct   snapshotid, 
					a.permissionlevel,
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
					'EF', 
					b.permissionname, 
					isgrant, 
					isgrantwith, 
					isrevoke, 
					isdeny, 
					objectid,
					objectname, 
					objecttype, 
					schemaid, 
					schemaname,
					owner,
					ownername,
					isaliased,
					inherited,
					sourcename,
					sourcetype,
					sourcepermission
				from #tmppermission a, coveringpermissionhierarchy b
				where classid = 100 and b.coveringpermissionname = a.permission
			end

		-- step 2.12
			declare @endpointid int
			declare @endpointprincipalid int
			declare @endpointname nvarchar(128)

			-- COPY ALL THE ENDPOINT PERMISSION AS EFFECTIVE PERMISSION	
			insert into #tmppermission (
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
				objectid,
				objectname, 
				objecttype, 
				schemaid, 
				schemaname,
				owner,
				ownername,
				isaliased,
				inherited,
				sourcename,
				sourcetype,
				sourcepermission)
			select  distinct   snapshotid, 
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
				'EF', 
				permission, 
				isgrant, 
				isgrantwith, 
				isrevoke, 
				isdeny, 
				objectid,
				objectname, 
				objecttype, 
				schemaid, 
				schemaname,
				owner,
				ownername,
				isaliased,
				inherited,
				sourcename,
				sourcetype,
				sourcepermission
			from #tmppermission
			where classid = 105


			-- CHECK IF ENDPOINT NEEDS COVERING FROM SERVER
			if exists (select * from #tmppermission where classid = 100 and UPPER(permission) = 'CONTROL SERVER') 
			begin
				-- IF SERVER HAS CONTROL PERMISSION ALL ENDPOINT WILL HAVE ALL PERMISSIONS
				delete from #tmppermission where UPPER(objecttype) = 'IENDP'

				declare myc1000 cursor for
						select distinct a.endpointid, a.principalid, a.name from endpoint a where snapshotid = @snapshotid 
				
				open myc1000
				fetch next from myc1000
				into @endpointid, @endpointprincipalid, @endpointname
				
				while @@fetch_status = 0
				begin

					insert into #tmppermission (
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
						objectid,
						objectname, 
						objecttype, 
						schemaid, 
						schemaname,
						owner,
						ownername,
						isaliased,
						coveringfrom,
						inherited,
						sourcename,
						sourcetype,
						sourcepermission)
					select top 1 
						snapshotid, 
						a.permissionlevel,
						logintype, 
						loginname, 
						connectionname, 
						databasename, 
						@endpointprincipalid, 
						dbo.getserverprincipalname(@snapshotid, @endpointprincipalid), 
						dbo.getserverprincipaltype(@snapshotid, @endpointprincipalid), 
						NULL, 
						NULL, 
						grantor, 
						grantorname,
						grantee, 
						granteename,
						105, 
						'EF', 
						'CONTROL', 
						isgrant, 
						isgrantwith, 
						isrevoke, 
						isdeny, 
						@endpointid,
						@endpointname, 
						'iENDP',
						NULL, 
						NULL,
						owner,
						ownername,
						isaliased,
						'SV',
						'Y',
						@connectionname,
						'iSRV',
						'CONTROL SERVER'
					from #tmppermission a
					where a.classid = 100 and UPPER(permission) = 'CONTROL SERVER'

					fetch next from myc1000
					into @endpointid, @endpointprincipalid, @endpointname
				end

				close myc1000
				deallocate myc1000	

				-- GET ALL COVERING PERMISSION
				insert into #tmppermission (
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
					objectid,
					objectname, 
					objecttype, 
					schemaid, 
					schemaname,
					owner,
					ownername,
					isaliased,
					inherited,
					sourcename,
					sourcetype,
					sourcepermission)
				select  distinct   snapshotid, 
					a.permissionlevel,
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
					'EF', 
					b.permissionname, 
					isgrant, 
					isgrantwith, 
					isrevoke, 
					isdeny, 
					objectid,
					objectname, 
					objecttype, 
					schemaid, 
					schemaname,
					owner,
					ownername,
					isaliased,
					'Y',
					sourcename,
					sourcetype,
					sourcepermission
				from #tmppermission a, coveringpermissionhierarchy b
				where a.classid = 105 and UPPER(a.permissionlevel) = 'SV' and UPPER(b.permissionlevel) = 'ENDPOINT' and UPPER(b.coveringpermissionname) = 'CONTROL'
			end 	
			else -- ELSE CHECK IF THERE IS OTHER SERVER COVERING PERMISSION, IF SO MANUALLY CREATE COVERING ROWS. ALSO, CREATE A DEFAULT CONNECT FOR ALL ENDPOINTS EXCEPT ADMIN
			begin
				declare myc1000 cursor for
						select distinct a.endpointid, a.principalid, a.name from endpoint a where snapshotid = @snapshotid 
				
				open myc1000
				fetch next from myc1000
				into @endpointid, @endpointprincipalid, @endpointname
				
				while @@fetch_status = 0
				begin

					if exists (select * from #tmppermission where classid = 100 and UPPER(permission) IN ('ALTER ANY ENDPOINT', 'VIEW ANY DEFINITION')) 
					begin

						insert into #tmppermission (
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
							objectid,
							objectname, 
							objecttype, 
							schemaid, 
							schemaname,
							owner,
							ownername,
							isaliased,
							coveringfrom,
							inherited,
							sourcename,
							sourcetype,
							sourcepermission)
						select top 1 
							snapshotid, 
							a.permissionlevel,
							logintype, 
							loginname, 
							connectionname, 
							databasename, 
							@endpointprincipalid, 
							dbo.getserverprincipalname(@snapshotid, @endpointprincipalid), 
							dbo.getserverprincipaltype(@snapshotid, @endpointprincipalid), 
							NULL, 
							NULL, 
							grantor, 
							grantorname,
							grantee, 
							granteename,
							105, 
							'EF', 
							CASE WHEN permission = 'ALTER ANY ENDPOINT' THEN 'ALTER' ELSE 'VIEW DEFINITION' END, 
							isgrant, 
							isgrantwith, 
							isrevoke, 
							isdeny, 
							@endpointid,
							@endpointname, 
							'iENDP',
							NULL, 
							NULL,
							owner,
							ownername,
							isaliased,
							'SV',
							'Y',
							sourcename,
							sourcetype,
							sourcepermission
						from #tmppermission a
						where a.classid = 100 and UPPER(permission) IN ('ALTER ANY ENDPOINT', 'VIEW ANY DEFINITION')
					end

			-- step 2.13
					-- INSERT CONNECT EXCEPT ADMIN
					if (UPPER(@endpointname) in ('TSQL LOCAL MACHINE', 'TSQL NAMED PIPES', 'TSQL DEFAULT TCP', 'TSQL DEFAULT VIA'))
					begin
						insert into #tmppermission (
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
							objectid,
							objectname, 
							objecttype, 
							schemaid, 
							schemaname,
							owner,
							ownername,
							isaliased,
							coveringfrom,
							inherited,
							sourcename,
							sourcetype,
							sourcepermission)
						select top 1 
							snapshotid, 
							a.permissionlevel,
							logintype, 
							loginname, 
							connectionname, 
							databasename, 
							@endpointprincipalid, 
							dbo.getserverprincipalname(@snapshotid, @endpointprincipalid), 
							dbo.getserverprincipaltype(@snapshotid, @endpointprincipalid), 
							NULL, 
							NULL, 
							grantor, 
							grantorname,
							grantee, 
							granteename,
							105, 
							'EF', 
							'CONNECT', 
							isgrant, 
							isgrantwith, 
							isrevoke, 
							isdeny, 
							@endpointid,
							@endpointname, 
							'iENDP',
							NULL, 
							NULL,
							owner,
							ownername,
							isaliased,
							'SV',
							'N',
							sourcename,
							sourcetype,
							sourcepermission
						from #tmppermission a
						where a.classid = 100
					end

					fetch next from myc1000
					into @endpointid, @endpointprincipalid, @endpointname
				end

				close myc1000
				deallocate myc1000	


				-- NEED TO BREAK DOWN HIGH LEVEL PERMISSION LIKE CONTROL TO MORE DETAIL PERMISSION PRIOR TO ADDING COVERING PERMISSION TO LOWER LEVEL
				insert into #tmppermission (
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
					objectid,
					objectname, 
					objecttype, 
					schemaid, 
					schemaname,
					owner,
					ownername,
					isaliased,
					inherited,
					sourcename,
					sourcetype,
					sourcepermission)
				select  distinct   snapshotid, 
					a.permissionlevel,
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
					'EF', 
					b.permissionname, 
					isgrant, 
					isgrantwith, 
					isrevoke, 
					isdeny, 
					objectid,
					objectname, 
					objecttype, 
					schemaid, 
					schemaname,
					owner,
					ownername,
					isaliased,
					'Y',
					sourcename,
					sourcetype,
					sourcepermission
				from #tmppermission a, coveringpermissionhierarchy b
				where a.classid = 105 and UPPER(b.permissionlevel) = 'ENDPOINT' and b.coveringpermissionname = a.permission	
			end

		-- step 2.14
			create table #tmpcovering (name nvarchar(128))
		
			declare @tmpepgrantee int
			declare @tmpepobjectid int

			declare @tmpdatabasename nvarchar(128)
			declare @#tmppermission nvarchar(128)
			declare @tmpprincipalid int
			declare @tmpgrantor int
			declare @tmpgrantorname nvarchar(128)
			declare @tmpgranteename nvarchar(128)
			declare @tmpcoveringpermission nvarchar(128)
			declare @tmpgrant nchar(1)
			declare @tmpgrantwith nchar(1)
			declare @tmpdeny nchar(1)
			declare @tmprevoke nchar(1)

			declare @tmploginid int
			declare @tmploginname nvarchar(256)
			declare @tmplogintype nchar(1)
			declare @tmpprincipaltype nchar(1)
			declare @tmpprincipalname nvarchar(256)

			-- LOGIN: IF SERVER LOGIN IS CONTROL THEN LOGIN WILL HAVE ALL PERMISSION FROM ALL USERS OR GROUPS
			if exists (select * from #tmppermission where classid = 100 and UPPER(permission) = 'CONTROL SERVER') 
			begin
				select distinct @tmpgrant = isgrant, @tmpgrantwith = isgrantwith, @tmprevoke = isrevoke, @tmpdeny = isrevoke from #tmppermission where classid = 100 and UPPER(permission) = 'CONTROL SERVER'

				-- IF SERVER HAS CONTROL PERMISSION ALL LOGINS WILL HAVE ALL PERMISSIONS
				delete from #tmppermission where classid = 101 and permissiontype = 'EF'			

				-- ALL USERS AND GROUPS ARE GRANTOR
				declare myca1000 cursor for
						select distinct a.principalid, a.name, a.type from serverprincipal a where snapshotid = @snapshotid and type IN ('G', 'U')
				
				open myca1000
				fetch next from myca1000
				into @tmpprincipalid, @tmpprincipalname, @tmpprincipaltype
				
				while @@fetch_status = 0
				begin
					declare myca1000a cursor for
							select distinct a.principalid, a.name, a.type from serverprincipal a where snapshotid = @snapshotid and a.principalid in (select  distinct  principalid from #tmplogins) or (a.principalid in (select distinct   principalid from serverrolemember where memberprincipalid in (select distinct   principalid from #tmplogins))) and UPPER(a.type) IN ('U', 'G')
			
					open myca1000a
					fetch next from myca1000a
					into @tmploginid, @tmploginname, @tmplogintype

					while @@fetch_status = 0
					begin
						--print 'tmplogin info ' + @tmploginname + ' ,' + @tmplogintype

						insert into #tmppermission (
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
							objectid,
							objectname, 
							objecttype, 
							schemaid, 
							schemaname,
							owner,
							ownername,
							isaliased,
							coveringfrom,
							inherited,
							sourcename,
							sourcetype,
							sourcepermission)
						values ( 
							@snapshotid, 
							'SV',
							@tmplogintype, 
							@tmploginname, 
							@connectionname, 
							NULL, 
							@tmploginid, 
							@tmploginname, 
							@tmplogintype, 
							NULL, 
							NULL, 
							@tmpprincipalid, 
							@tmpprincipalname, 
							@tmploginid, 
							@tmploginname, 
							101, 
							'EF',
							'CONTROL', 
							@tmpgrant, 
							@tmpgrantwith, 
							@tmprevoke, 
							@tmpdeny, 
							@tmploginid,
							@tmploginname, 
							dbo.getclassobjecttype(101),
							NULL, 
							NULL,
							NULL,
							NULL,
							'N',
							'SV',
							'Y',
							@connectionname,
							'iSRV',
							'CONTROL SERVER'
							)

						fetch next from myca1000a
						into @tmploginid, @tmploginname, @tmplogintype
					end


					close myca1000a
					deallocate myca1000a	

					fetch next from myca1000
					into @tmpprincipalid, @tmpprincipalname, @tmpprincipaltype
				end

				close myca1000
				deallocate myca1000	
			end 	

			-- LOGIN: IF SERVER LOGIN IS VIEW ANY DEFINITION THEN LOGIN WILL HAVE VIEW DEFINITION PERMISSION FROM ALL USERS OR GROUPS
			if exists (select * from #tmppermission where classid=100 and UPPER(permission) = 'VIEW ANY DEFINITION') 
			begin
				select distinct @tmpgrant = isgrant, @tmpgrantwith = isgrantwith, @tmprevoke = isrevoke, @tmpdeny = isrevoke from #tmppermission where classid = 100 and UPPER(permission) = 'VIEW ANY DEFINITION'

				-- ALL USERS AND GROUPS ARE GRANTOR
				declare myca1000 cursor for
						select distinct a.principalid, a.name, a.type from serverprincipal a where snapshotid = @snapshotid and type IN ('G', 'U')
				
				open myca1000
				fetch next from myca1000
				into @tmpprincipalid, @tmpprincipalname, @tmpprincipaltype
				
				while @@fetch_status = 0
				begin
					declare myca1000a cursor for
							select distinct a.principalid, a.name, a.type from serverprincipal a where snapshotid = @snapshotid and a.principalid in (select distinct   principalid from #tmplogins) or (a.principalid in (select distinct   principalid from serverrolemember where memberprincipalid in (select distinct   principalid from #tmplogins))) and UPPER(a.type) IN ('U', 'G')
			
					open myca1000a
					fetch next from myca1000a
					into @tmploginid, @tmploginname, @tmplogintype

					while @@fetch_status = 0
					begin
						--print 'tmplogin info ' + @tmploginname + ' ,' + @tmplogintype

						insert into #tmppermission (
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
							objectid,
							objectname, 
							objecttype, 
							schemaid, 
							schemaname,
							owner,
							ownername,
							isaliased,
							coveringfrom,
							inherited,
							sourcename,
							sourcetype,
							sourcepermission)
						values ( 
							@snapshotid, 
							'SV',
							@tmplogintype, 
							@tmploginname, 
							@connectionname, 
							NULL, 
							@tmploginid, 
							@tmploginname, 
							@tmplogintype, 
							NULL, 
							NULL, 
							@tmpprincipalid, 
							@tmpprincipalname, 
							@tmploginid, 
							@tmploginname, 
							101, 
							'EF',
							'VIEW DEFINITION', 
							@tmpgrant, 
							@tmpgrantwith, 
							@tmprevoke, 
							@tmpdeny, 
							@tmploginid,
							@tmploginname, 
							dbo.getclassobjecttype(101),
							NULL, 
							NULL,
							NULL,
							NULL,
							'N',
							'SV',
							'Y',
							@connectionname,
							'iSRV',
							'VIEW ANY DEFINITION'
							)

						fetch next from myca1000a
						into @tmploginid, @tmploginname, @tmplogintype
					end


					close myca1000a
					deallocate myca1000a	

					fetch next from myca1000
					into @tmpprincipalid, @tmpprincipalname, @tmpprincipaltype
				end

				close myca1000
				deallocate myca1000	

			end 	


			-- LOGIN: IF SERVER LOGIN IS ALTER ANY LOGIN THEN LOGIN WILL HAVE ALTER PERMISSION FROM ALL USERS OR GROUPS
			if exists (select * from #tmppermission where classid = 100 and UPPER(permission) = 'ALTER ANY LOGIN') 
			begin
				select distinct @tmpgrant = isgrant, @tmpgrantwith = isgrantwith, @tmprevoke = isrevoke, @tmpdeny = isrevoke from #tmppermission where classid = 100 and UPPER(permission) = 'ALTER ANY LOGIN'

				-- ALL USERS AND GROUPS ARE GRANTOR
				declare myca1000 cursor for
						select distinct a.principalid, a.name, a.type from serverprincipal a where snapshotid = @snapshotid and type IN ('G', 'U')
				
				open myca1000
				fetch next from myca1000
				into @tmpprincipalid, @tmpprincipalname, @tmpprincipaltype
				
				while @@fetch_status = 0
				begin
					declare myca1000a cursor for
							select distinct a.principalid, a.name, a.type from serverprincipal a where snapshotid = @snapshotid and a.principalid in (select distinct   principalid from #tmplogins) or (a.principalid in (select distinct   principalid from serverrolemember where memberprincipalid in (select  distinct  principalid from #tmplogins))) and UPPER(a.type) IN ('U', 'G')
			
					open myca1000a
					fetch next from myca1000a
					into @tmploginid, @tmploginname, @tmplogintype

					while @@fetch_status = 0
					begin
						--print 'tmplogin info ' + @tmploginname + ' ,' + @tmplogintype

						insert into #tmppermission (
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
							objectid,
							objectname, 
							objecttype, 
							schemaid, 
							schemaname,
							owner,
							ownername,
							isaliased,
							coveringfrom,
							inherited,
							sourcename,
							sourcetype,
							sourcepermission)
						values ( 
							@snapshotid, 
							'SV',
							@tmplogintype, 
							@tmploginname, 
							@connectionname, 
							NULL, 
							@tmploginid, 
							@tmploginname, 
							@tmplogintype, 
							NULL, 
							NULL, 
							@tmpprincipalid, 
							@tmpprincipalname, 
							@tmploginid, 
							@tmploginname, 
							101, 
							'EF',
							'ALTER', 
							@tmpgrant, 
							@tmpgrantwith, 
							@tmprevoke, 
							@tmpdeny, 
							@tmploginid,
							@tmploginname, 
							dbo.getclassobjecttype(101),
							NULL, 
							NULL,
							NULL,
							NULL,
							'N',
							'SV',
							'Y',
							@connectionname,
							'iSRV',
							'ALTER ANY LOGIN'
							)

						fetch next from myca1000a
						into @tmploginid, @tmploginname, @tmplogintype
					end


					close myca1000a
					deallocate myca1000a	

					fetch next from myca1000
					into @tmpprincipalid, @tmpprincipalname, @tmpprincipaltype
				end

				close myca1000
				deallocate myca1000	

			end 	
					

			-- LOGIN - NEED TO BREAK DOWN HIGH LEVEL PERMISSION LIKE CONTROL TO MORE DETAIL PERMISSION PRIOR TO ADDING COVERING PERMISSION TO LOWER LEVEL
			insert into #tmppermission (
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
				objectid,
				objectname, 
				objecttype, 
				schemaid, 
				schemaname,
				owner,
				ownername,
				isaliased,
				inherited,
				sourcename,
				sourcetype,
				sourcepermission)
			select  distinct   snapshotid, 
				a.permissionlevel,
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
				'EF', 
				b.permissionname, 
				isgrant, 
				isgrantwith, 
				isrevoke, 
				isdeny, 
				objectid,
				objectname, 
				objecttype, 
				schemaid, 
				schemaname,
				owner,
				ownername,
				isaliased,
				'Y',
				sourcename,
				sourcetype,
				sourcepermission
			from #tmppermission a, coveringpermissionhierarchy b
			where a.classid = 101 and UPPER(b.permissionlevel) = 'LOGIN' and b.coveringpermissionname = a.permission

		-- step 2.15
			-- COPY ALL THE DATABASE LEVEL PERMISSION AS EFFECTIVE PERMISSION	
			insert into #tmppermission (
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
				objectid,
				objectname, 
				objecttype, 
				schemaid, 
				schemaname,
				owner,
				ownername,
				isaliased,
				inherited,
				sourcename,
				sourcetype,
				sourcepermission
				)
			select  distinct   snapshotid, 
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
				'EF', 
				permission, 
				isgrant, 
				isgrantwith, 
				isrevoke, 
				isdeny, 
				objectid,
				objectname, 
				objecttype, 
				schemaid, 
				schemaname,
				owner,
				ownername,
				isaliased,
				inherited,
				sourcename,
				sourcetype,
				sourcepermission
			from #tmppermission
			where classid = 0 and permissiontype = 'EX'

	-- step 2.16
		-- PROVIDE SERVER TO DATABASE COVERING PERMISSIONS
		insert into #tmppermission (
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
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct a.snapshotid, 
			'DB',
			logintype, 
			loginname, 
			connectionname, 
			c.databasename, 
			principalid, 
			principalname, 
			principaltype,
			null, 
			null, 
			1,
			'dbo',
			d.uid,
			dbo.getdatabaseprincipalname(c.snapshotid, c.dbid, d.uid),
			0,
			'EF', 
			b.permissionname, 
			isgrant, 
			isgrantwith, 
			isrevoke, 
			isdeny, 
			c.dbid,
			c.databasename, 
			'DB', 
			null, 
			null,
			dbo.getdatabaseprincipalid(c.snapshotid, c.dbid, c.owner),
			c.owner, 
			'N',
			'Y',
			sourcename,
			sourcetype,
			sourcepermission
		from #tmppermission a, coveringpermissionhierarchy b, sqldatabase c, #tmpuid d
		where 	a.classid = 100 and 
			UPPER(b.permissionlevel) = 'DATABASE' and 
			b.parentcoveringpermission = a.permission and
			c.snapshotid = @snapshotid and
			c.dbid = @databaseid and
			d.dbid = c.dbid and 
			d.uid > 4 -- do not process guest, dbo and public, info, sys
		

		--select * from #tmppermission where permissionlevel = 'DB'

	-- step 2.17
		-- PROCESS FIXED DATABASE ROLE PERMISSION
		-- GET ALL EFFECTIVE PERMISSION ASSOCIATES WITH THE FIXED DATABASE ROLE AND INSERT THEM AS EFFECTIVE PERMISSIONS
		insert into #tmppermission (
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
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			coveringfrom,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select distinct    a.snapshotid, 
			'DB',
			@logintype, 
			@loginname, 
			@connectionname, 
			d.databasename, 
			f.principalid, 
			f.name, 
			f.type, 
			dbo.getdatabaseprincipalname(e.snapshotid, e.dbid, e.uid), 
			e.type, 
			a.uid, 
			a.name, 
			b.uid, 
			dbo.getdatabaseprincipalname(a.snapshotid, a.dbid, a.rolememberuid), 
			0, 
			'EF', 
			rolepermission, 
			c.isgrant, 
			c.isgrantwith, 
			c.isrevoke, 
			c.isdeny, 
			d.dbid,
			d.databasename, 
			'DB', 
			NULL, 
			NULL,
			dbo.getdatabaseprincipalid(d.snapshotid, d.dbid, d.owner),
			d.owner, 
			'N',
			'DBFXROLE',
			'N',
			c.rolename,
			'iDRLE',
			rolepermission
		from 
			vwfixeddatabaserolemember a, 
			#tmpuid b,
			fixedrolepermission c,
			sqldatabase d,
			databaseprincipal e left outer join serverprincipal f on e.snapshotid = f.snapshotid and e.usersid = f.sid
		where 
			a.snapshotid = @snapshotid and 
			a.dbid = b.dbid and 
			a.rolememberuid = b.uid and
			e.snapshotid = a.snapshotid and
			e.uid = b.uid and 
			e.dbid = b.dbid and
			UPPER(a.name) = UPPER(c.rolename) and
			d.dbid = a.dbid and
			d.snapshotid = a.snapshotid and
			d.databasename = @databasename and 
			c.roletype = 'D'


		-- NEED TO BREAK DOWN HIGH LEVEL PERMISSION LIKE CONTROL TO MORE DETAIL PERMISSION PRIOR TO ADDING COVERING PERMISSION TO LOWER LEVEL
		insert into #tmppermission (
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
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct   snapshotid, 
			a.permissionlevel,
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
			'EF', 
			b.permissionname, 
			isgrant, 
			isgrantwith, 
			isrevoke, 
			isdeny, 
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission
		from #tmppermission a, coveringpermissionhierarchy b
		where a.classid = 0 and UPPER(b.permissionlevel) = 'DATABASE' and b.coveringpermissionname = a.permission
	
		-- FOR EACH DATABASE, COPY THE DATA TO ANOTHER TABLE EXCEPT THE PERMISSION AND GRANTING/DENY ATTRIBUTES
		insert into #tmppermission2 (
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
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select 	distinct snapshotid, 
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
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			'Y',
			sourcename,
			sourcetype,
			sourcepermission
		from #tmppermission
		where classid = 0 and UPPER(permissiontype) = 'EF' and principalid in (select distinct grantee from #tmppermission where classid = 100)

		-- FOR EACH DATABASE, CONVERT EACH ROW TO COVERING PERMISSION BY COPYING OR EXPANDING EACH ROW TO THE NEW COVERING PERMISISON
		declare myc102 cursor for
			select distinct  cc.databasename, cc.principalid , aa.permission, aa.isgrant, aa.isgrantwith, aa.isdeny, aa.isrevoke  
			from #tmppermission aa, #tmppermission2 cc
			where UPPER(aa.permissionlevel) = 'SV' 
				and UPPER(aa.permissiontype) = 'EF' 
				and UPPER(aa.objecttype) = 'SERVER' 
				and cc.principalname IS NOT NULL 
				and cc.principalid IS NOT NULL
				and aa.principalid = cc.principalid
				
		open myc102
		fetch next from myc102
		into @tmpdatabasename, @tmpprincipalid, @#tmppermission, @tmpgrant, @tmpgrantwith, @tmpdeny, @tmprevoke
		while @@fetch_status = 0
		begin
				-- COPY A ROW FROM #tmppermission2 
				insert into #tmppermission (
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
					objectid,
					objectname, 
					objecttype, 
					schemaid, 
					schemaname,
					owner,
					ownername,
					isaliased,
					coveringfrom,
					inherited,
					sourcename,
					sourcetype,
					sourcepermission)
				select  distinct snapshotid, 
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
					'EF', 
					b.[permissionname] , 
					@tmpgrant, 
					@tmpgrantwith, 
					@tmprevoke, 
					@tmpdeny, 
					objectid,
					objectname, 
					objecttype, 
					schemaid, 
					schemaname,
					owner,
					ownername,
					isaliased,
					'SV',
					'Y',
					sourcename,
					sourcetype,
					sourcepermission
				from 	#tmppermission2 a
					, getcoveringpermission('SERVER',@#tmppermission,'DATABASE' ) b
				where a.databasename = @tmpdatabasename 
					and a.principalid = @tmpprincipalid

		fetch next from myc102
		into @tmpdatabasename, @tmpprincipalid, @#tmppermission, @tmpgrant, @tmpgrantwith, @tmpdeny, @tmprevoke
		end

		close myc102
		deallocate myc102

	-- step 2.18
		-- NEED TO ADD SCHEMA COVERING PERMISSION FROM DATABASE PERMISSION
		-- BY EXPANDING ON EXISTING DATABASE PERMISSION TO SCHEMA LEVEL
		insert into #tmppermission (
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
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			coveringfrom,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct a.snapshotid, 
			'SCH',
			logintype, 
			loginname, 
			connectionname, 
			@databasename, 
			principalid, 
			principalname, 
			principaltype,
			databaseprincipal, 
			databaseprincipaltype, 
			grantor, 
			grantorname,
			grantee, 
			granteename,
			3, 
			'EF', 
			b.permissionname, 
			isgrant, 
			isgrantwith, 
			isrevoke, 
			isdeny, 
			c.schemaid,
			c.schemaname, 
			'iSCM', 
			c.schemaid, 
			c.schemaname,
			c.uid,
			dbo.getdatabaseprincipalname(a.snapshotid, @databaseid, c.uid),
			isaliased,
			'DB',
			'Y',
			sourcename,
			sourcetype,
			sourcepermission
		from 	#tmppermission a, 
			coveringpermissionhierarchy b, 
			databaseschema c
		where 
			c.snapshotid = a.snapshotid and 
			c.dbid = @databaseid and 
			c.schemaid < 16383 and
			UPPER(b.permissionlevel) = 'SCHEMA' and 
			UPPER(b.parentpermissionlevel) = 'DATABASE' and 
			UPPER(b.parentcoveringpermission) = UPPER(a.permission) and 
			UPPER(a.permissionlevel) = 'DB' and 
			UPPER(a.objecttype) = 'DB' and 
			UPPER(a.permissiontype) = 'EF' and 
			UPPER(a.permission) IN ('SELECT', 'INSERT', 'UPDATE', 'DELETE', 'REFERENCES', 'EXECUTE', 'VIEW DEFINITION', 'ALTER ANY SCHEMA', 'ALTER', 'CONTROL')	

		-- schema id greater than 16382 is database built in roles

		-- COPY ALL THE SCHEMA EXPLICIT PERMISSION AS EFFECTIVE
		insert into #tmppermission (
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
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct snapshotid, 
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
			'EF', 
			permission, 
			isgrant, 
			isgrantwith, 
			isrevoke, 
			isdeny, 
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission
		from #tmppermission
		where UPPER(permissionlevel) = 'SCH'		


		-- TODO: NEED TO BREAK DOWN HIGH LEVEL PERMISSION LIKE CONTROL TO MORE DETAIL PERMISSION PRIOR TO ADDING COVERING PERMISSION TO LOWER LEVEL
		insert into #tmppermission (
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
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct snapshotid, 
			a.permissionlevel,
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
			'EF', 
			b.permissionname, 
			isgrant, 
			isgrantwith, 
			isrevoke, 
			isdeny, 
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission
		from #tmppermission a, coveringpermissionhierarchy b
		where a.classid = 3 and UPPER(b.permissionlevel) = 'SCHEMA' and b.coveringpermissionname = a.permission

		delete from #tmppermission2	

		-- COPY THE EXISTING SCHEMA DATA TO ANOTHER TABLE EXCEPT THE PERMISSION AND GRANTING/DENY ATTRIBUTES
		insert into #tmppermission2 (
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
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select 	distinct snapshotid, 
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
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission
		from #tmppermission
		where UPPER(permissionlevel) = 'SCH' and UPPER(permissiontype) = 'EF'

		declare @tmpgrantee int
		declare @tmpschemaid int

		-- GET ALL THE SCHEMA THAT HAS THE SAME GRANTEE AS THE DATABASE
		declare myc202 cursor for
			select distinct cc.grantor, cc.grantorname, cc.databasename, cc.schemaid, cc.grantee, aa.permission, aa.isgrant, aa.isgrantwith, aa.isdeny, aa.isrevoke
			from #tmppermission aa, #tmppermission2 cc
			where UPPER(aa.permissionlevel) = 'DB' 
				and aa.objecttype = 'DB' 
				and UPPER(aa.permissiontype) = 'EF' 
				and aa.grantee = cc.grantee
				and aa.grantor = cc.grantor
		open myc202
		fetch next from myc202
		into @tmpgrantor, @tmpgrantorname, @tmpdatabasename, @tmpschemaid, @tmpgrantee,@#tmppermission, @tmpgrant, @tmpgrantwith, @tmpdeny, @tmprevoke
		while @@fetch_status = 0
		begin
	
			-- COPY A ROW FROM #tmppermission2 
			insert into #tmppermission (
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
				objectid,
				objectname, 
				objecttype, 
				schemaid, 
				schemaname,
				owner,
				ownername,
				isaliased,
				coveringfrom,
				inherited,
				sourcename,
				sourcetype,
				sourcepermission)
			select  distinct   snapshotid, 
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
				@tmpgrantor, 
				@tmpgrantorname,
				grantee, 
				granteename,
				classid, 
				'EF', 
				b.[permissionname] , 
				@tmpgrant, 
				@tmpgrantwith, 
				@tmprevoke, 
				@tmpdeny, 
				objectid,
				objectname, 
				objecttype, 
				schemaid, 
				schemaname,
				owner,
				ownername,
				isaliased,
				'DB',
				'Y',
				sourcename,
				sourcetype,
				sourcepermission
			from 	#tmppermission2 a
				, getcoveringpermission('DATABASE',@#tmppermission,'SCHEMA' ) b
			where UPPER(a.permissionlevel) = 'SCH' 
				and a.grantee = @tmpgrantee
				and a.databasename = @tmpdatabasename
				and a.schemaid = @tmpschemaid
				-- 12/29/06 MSO added the following line to fix PR#801374
				and a.permission = b.permissionname
		fetch next from myc202
		into @tmpgrantor, @tmpgrantorname, @tmpdatabasename, @tmpschemaid, @tmpgrantee,@#tmppermission, @tmpgrant, @tmpgrantwith, @tmpdeny, @tmprevoke
		end

		close myc202
		deallocate myc202
		
	-- step 2.19
		-- NEED TO ADD OBJECT COVERING PERMISSION FROM SCHEMA PERMISSION
		-- BY EXPANDING ON EXISTING SCHEMA PERMISSION TO OBJECT LEVEL

		-- THIS COVERING ONLY PERTAINS TO SCHEMA LEVEL PERMISSION. NO DATABASE LEVEL OR FIXED ROLE HERE.
		insert into #tmppermission (
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
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			coveringfrom,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select distinct a.snapshotid, 
			'OBJ',
			logintype, 
			loginname, 
			connectionname, 
			@databasename, 
			principalid, 
			principalname, 
			principaltype,
			databaseprincipal, 
			databaseprincipaltype, 
			grantor, 
			grantorname,
			grantee, 
			granteename,
			c.classid, 
			'EF', 
			b.permissionname, 
			isgrant, 
			isgrantwith, 
			isrevoke, 
			isdeny, 
			c.objectid,
			c.name, 
			c.type, 
			a.schemaid, 
			a.schemaname,
			c.owner,
			dbo.getdatabaseprincipalname(a.snapshotid, @databaseid, c.owner),
			isaliased,
			'DB',
			'Y',
			sourcename,
			sourcetype,
			sourcepermission
		from #tmppermission a, coveringpermissionhierarchy b, databaseobject c
		where 
			a.snapshotid = @snapshotid and
			c.snapshotid = a.snapshotid and 
			c.schemaid = a.schemaid and 
			c.dbid = @databaseid and 
			UPPER(b.permissionlevel) = 'OBJECT' and 
			UPPER(b.parentpermissionlevel) = 'SCHEMA' and 
			UPPER(b.parentcoveringpermission) = UPPER(a.permission) and 
			a.classid = 3 and 
			UPPER(a.permissiontype) = 'EF' and 
			UPPER(a.permission) IN ('SELECT', 'INSERT', 'UPDATE', 'DELETE', 'REFERENCES', 'EXECUTE', 'ALTER', 'CONTROL', 'VIEW DEFINITION') 
			and a.sourcetype not in ('DB', 'iDRLE')

		-- COPY ALL OBJECT EXPLICIT PERMISSION AS EFFECTIVE
		insert into #tmppermission (
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
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct   snapshotid, 
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
			'EF', 
			permission, 
			isgrant, 
			isgrantwith, 
			isrevoke, 
			isdeny, 
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission
		from #tmppermission
		where UPPER(permissionlevel) IN ('OBJ')

		-- OBJECT LEVEL - NEED TO BREAK DOWN HIGH LEVEL PERMISSION LIKE CONTROL TO MORE DETAIL PERMISSION PRIOR TO ADDING COVERING PERMISSION TO LOWER LEVEL
		insert into #tmppermission (
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
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct snapshotid, 
			a.permissionlevel,
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
			'EF', 
			b.permissionname, 
			isgrant, 
			isgrantwith, 
			isrevoke, 
			isdeny, 
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission
		from #tmppermission a, coveringpermissionhierarchy b
		where 
			a.classid = 1 and
			UPPER(b.permissionlevel) = 'OBJECT' and
			b.coveringpermissionname = a.permission

	-- step 2.20
		-- REMOTE SERVICE BINDING LEVEL - NEED TO BREAK DOWN HIGH LEVEL PERMISSION LIKE CONTROL TO MORE DETAIL PERMISSION PRIOR TO ADDING COVERING PERMISSION TO LOWER LEVEL
		insert into #tmppermission (
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
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct snapshotid, 
			a.permissionlevel,
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
			'EF', 
			b.permissionname, 
			isgrant, 
			isgrantwith, 
			isrevoke, 
			isdeny, 
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission
		from #tmppermission a, coveringpermissionhierarchy b
		where 
			a.classid = 18 and
			UPPER(b.permissionlevel) = 'REMOTE SERVICE BINDING' and
			b.coveringpermissionname = a.permission


		-- ROUTE LEVEL - NEED TO BREAK DOWN HIGH LEVEL PERMISSION LIKE CONTROL TO MORE DETAIL PERMISSION PRIOR TO ADDING COVERING PERMISSION TO LOWER LEVEL
		insert into #tmppermission (
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
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct  snapshotid, 
			a.permissionlevel,
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
			'EF', 
			b.permissionname, 
			isgrant, 
			isgrantwith, 
			isrevoke, 
			isdeny, 
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission
		from #tmppermission a, coveringpermissionhierarchy b
		where
			a.classid = 19 and
			UPPER(b.permissionlevel) = 'ROUTE'
			and b.coveringpermissionname = a.permission

		-- FULLTEXT CATELOG LEVEL - NEED TO BREAK DOWN HIGH LEVEL PERMISSION LIKE CONTROL TO MORE DETAIL PERMISSION PRIOR TO ADDING COVERING PERMISSION TO LOWER LEVEL
		insert into #tmppermission (
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
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct   snapshotid, 
			a.permissionlevel,
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
			'EF', 
			b.permissionname, 
			isgrant, 
			isgrantwith, 
			isrevoke, 
			isdeny, 
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission
		from #tmppermission a, coveringpermissionhierarchy b
		where
			a.classid = 23 and
			UPPER(b.permissionlevel) = 'FULLTEXT CATALOG' and
			b.coveringpermissionname = a.permission

		-- SYMMETRIC KEY LEVEL - NEED TO BREAK DOWN HIGH LEVEL PERMISSION LIKE CONTROL TO MORE DETAIL PERMISSION PRIOR TO ADDING COVERING PERMISSION TO LOWER LEVEL
		insert into #tmppermission (
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
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct snapshotid, 
			a.permissionlevel,
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
			'EF', 
			b.permissionname, 
			isgrant, 
			isgrantwith, 
			isrevoke, 
			isdeny, 
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission
		from #tmppermission a, coveringpermissionhierarchy b
		where
			a.classid = 24 and
			UPPER(b.permissionlevel) = 'SYMMETRIC KEY' and
			b.coveringpermissionname = a.permission

		-- CERTIFICATE LEVEL - NEED TO BREAK DOWN HIGH LEVEL PERMISSION LIKE CONTROL TO MORE DETAIL PERMISSION PRIOR TO ADDING COVERING PERMISSION TO LOWER LEVEL
		insert into #tmppermission (
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
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct   snapshotid, 
			a.permissionlevel,
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
			'EF', 
			b.permissionname, 
			isgrant, 
			isgrantwith, 
			isrevoke, 
			isdeny, 
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission
		from #tmppermission a, coveringpermissionhierarchy b
		where
			a.classid = 25 and
			UPPER(b.permissionlevel) = 'CERTIFICATE' and
			b.coveringpermissionname = a.permission

		-- ASYMMETRIC KEY - NEED TO BREAK DOWN HIGH LEVEL PERMISSION LIKE CONTROL TO MORE DETAIL PERMISSION PRIOR TO ADDING COVERING PERMISSION TO LOWER LEVEL
		insert into #tmppermission (
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
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct  snapshotid, 
			a.permissionlevel,
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
			'EF', 
			b.permissionname, 
			isgrant, 
			isgrantwith, 
			isrevoke, 
			isdeny, 
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission
		from #tmppermission a, coveringpermissionhierarchy b
		where
			a.classid = 26 and
			UPPER(b.permissionlevel) = 'ASYMMETRIC KEY' and
			b.coveringpermissionname = a.permission

		-- TYPE - NEED TO BREAK DOWN HIGH LEVEL PERMISSION LIKE CONTROL TO MORE DETAIL PERMISSION PRIOR TO ADDING COVERING PERMISSION TO LOWER LEVEL
		insert into #tmppermission (
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
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct  snapshotid, 
			a.permissionlevel,
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
			'EF', 
			b.permissionname, 
			isgrant, 
			isgrantwith, 
			isrevoke, 
			isdeny, 
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission
		from #tmppermission a, coveringpermissionhierarchy b
		where
			a.classid = 6 and
			UPPER(b.permissionlevel) = 'TYPE' and
			b.coveringpermissionname = a.permission

		-- XML SCHEMA COLLECTION - NEED TO BREAK DOWN HIGH LEVEL PERMISSION LIKE CONTROL TO MORE DETAIL PERMISSION PRIOR TO ADDING COVERING PERMISSION TO LOWER LEVEL
		insert into #tmppermission (
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
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct snapshotid, 
			a.permissionlevel,
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
			'EF', 
			b.permissionname, 
			isgrant, 
			isgrantwith, 
			isrevoke, 
			isdeny, 
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission
		from #tmppermission a, coveringpermissionhierarchy b
		where
			a.classid = 10 and
			UPPER(b.permissionlevel) = 'XML SCHEMA COLLECTION' and
			b.coveringpermissionname = a.permission

		-- ASSEMBLY - NEED TO BREAK DOWN HIGH LEVEL PERMISSION LIKE CONTROL TO MORE DETAIL PERMISSION PRIOR TO ADDING COVERING PERMISSION TO LOWER LEVEL
		insert into #tmppermission (
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
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct snapshotid, 
			a.permissionlevel,
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
			'EF', 
			b.permissionname, 
			isgrant, 
			isgrantwith, 
			isrevoke, 
			isdeny, 
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission
		from #tmppermission a, coveringpermissionhierarchy b
		where
			a.classid = 5 and
			UPPER(b.permissionlevel) = 'ASSEMBLY' and
			b.coveringpermissionname = a.permission


		-- USER - NEED TO BREAK DOWN HIGH LEVEL PERMISSION LIKE CONTROL TO MORE DETAIL PERMISSION PRIOR TO ADDING COVERING PERMISSION TO LOWER LEVEL
		insert into #tmppermission (
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
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct   snapshotid, 
			a.permissionlevel,
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
			'EF', 
			b.permissionname, 
			isgrant, 
			isgrantwith, 
			isrevoke, 
			isdeny, 
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission
		from #tmppermission a, coveringpermissionhierarchy b
		where
			a.classid = 4 and
			UPPER(b.permissionlevel) = 'USER' and
			b.coveringpermissionname = a.permission

		-- CONTRACT - NEED TO BREAK DOWN HIGH LEVEL PERMISSION LIKE CONTROL TO MORE DETAIL PERMISSION PRIOR TO ADDING COVERING PERMISSION TO LOWER LEVEL
		insert into #tmppermission (
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
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct snapshotid, 
			a.permissionlevel,
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
			'EF', 
			b.permissionname, 
			isgrant, 
			isgrantwith, 
			isrevoke, 
			isdeny, 
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission
		from #tmppermission a, coveringpermissionhierarchy b
		where
			a.classid = 16 and
			UPPER(b.permissionlevel) = 'CONTRACT' and
			b.coveringpermissionname = a.permission

		-- SERVICE - NEED TO BREAK DOWN HIGH LEVEL PERMISSION LIKE CONTROL TO MORE DETAIL PERMISSION PRIOR TO ADDING COVERING PERMISSION TO LOWER LEVEL
		insert into #tmppermission (
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
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct   snapshotid, 
			a.permissionlevel,
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
			'EF', 
			b.permissionname, 
			isgrant, 
			isgrantwith, 
			isrevoke, 
			isdeny, 
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission
		from #tmppermission a, coveringpermissionhierarchy b
		where a.classid = 17 and UPPER(b.permissionlevel) = 'SERVICE' and b.coveringpermissionname = a.permission

		delete from #tmppermission2

		-- FOR ALL OBJECTS WHOSE SCHEMA HAS COVERING PERMISSIONS, COPY THEM TO PERMISSION2 AS STAGING AREA
		-- IT'S POSSIBLE THERE IS NO EXISTING PERMISSION ON THE OBJECT
		insert into #tmppermission2 (
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
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select 	distinct snapshotid, 
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
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission
		from #tmppermission
		where UPPER(permissionlevel) = 'OBJ' and UPPER(permissiontype) = 'EF' and schemaid IS NOT NULL

		if (@issysadminrole = 'N')
		begin

			-- IF THERE IS NO CURRENT PERMISSION ON THE OBJECT THEN CREATE A DUMMY ROW
			if not exists (select 1 from #tmppermission2)
			begin
		
				insert into #tmppermission2 (
					snapshotid, 
					permissionlevel,
					logintype, 
					loginname, 
					connectionname, 
					databasename, 
					classid, 
					permissiontype, 
					objectid,
					objectname, 
					objecttype, 
					schemaid, 
					schemaname,
					owner,
					ownername,
					isaliased) 
				select distinct 
					a.snapshotid, 
					'OBJ',
					@logintype,
					@loginname,
					@connectionname,
					@databasename, 
					d.classid,
					'TP',
					d.objectid,
					d.name, 
					d.type, 
					d.schemaid, 
					dbo.getschemaname(d.snapshotid, d.dbid, d.schemaid),
					d.owner,
					dbo.getdatabaseprincipalname(d.snapshotid, d.dbid, d.owner),
					a.isalias
				from 
					databaseprincipal a left outer join serverprincipal e on a.snapshotid = e.snapshotid and a.usersid = e.sid, 
					#tmpuid c, 
					databaseobject d,
					databaseschema f
				where 
					a.snapshotid = @snapshotid and 
					(a.uid = c.uid or (UPPER(a.isalias) = 'Y' and a.altuid = c.uid)) and 
					a.dbid = c.dbid and
					d.snapshotid = a.snapshotid and 
					d.dbid = a.dbid and
					f.snapshotid = d.snapshotid and
					f.dbid = c.dbid and
					d.schemaid = f.schemaid and
					d.dbid = f.dbid and
					d.snapshotid = f.snapshotid and
					a.dbid = @databaseid and
					d.parentobjectid = 0 and
					d.classid = 1
			end
		end
		
		-- APPLY SCHEMA COVERING PERMISSION TO THE OBJECTS FROM DATABASE FIXED ROLE	
		declare myc302 cursor for
			select distinct  aa.grantor, aa.grantorname, aa.grantee, aa.granteename, cc.databasename, cc.schemaid , aa.permission, aa.isgrant, aa.isgrantwith, aa.isdeny, aa.isrevoke  
			from #tmppermission aa, #tmppermission2 cc
			where UPPER(aa.permissionlevel) = 'SCH' 
				and UPPER(aa.permissiontype) = 'EF' 
				and aa.schemaid = cc.schemaid 
				and aa.databasename = @databasename
				and aa.sourcetype in ('DB', 'iDRLE')

		open myc302
		fetch next from myc302
			into @tmpgrantor, @tmpgrantorname, @tmpgrantee, @tmpgranteename, @tmpdatabasename, @tmpschemaid, @#tmppermission, @tmpgrant, @tmpgrantwith, @tmpdeny, @tmprevoke

		while @@fetch_status = 0
		begin
			-- THERE MIGHT BE REDUNDENCY WHEN COVERING PERMISSION PRODUCES SAME LOWER LEVEL PERMISSION		
			insert into #tmppermission (
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
				objectid,
				objectname, 
				objecttype, 
				schemaid, 
				schemaname,
				owner,
				ownername,
				isaliased,
				coveringfrom,
				inherited,
				sourcename,
				sourcetype,
				sourcepermission)
			select distinct snapshotid, 
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
				@tmpgrantor, 
				@tmpgrantorname,
				@tmpgrantee, 
				@tmpgranteename,
				classid, 
				'EF', 
				b.permissionname,
				@tmpgrant, 
				@tmpgrantwith, 
				@tmprevoke, 
				@tmpdeny, 
				objectid,
				objectname, 
				objecttype, 
				schemaid, 
				schemaname,
				owner,
				ownername,
				isaliased,
				'SCH',
				'Y',
				@tmpgrantorname,
				'iDRLE',
				b.permissionname
			from 	#tmppermission2 a
			, getcoveringpermission('SCHEMA',@#tmppermission,'OBJECT' ) b
			where UPPER(a.permissionlevel) = 'OBJ' 
			 	and a.databasename = @tmpdatabasename 
			 	and a.schemaid = @tmpschemaid

			fetch next from myc302
				into @tmpgrantor, @tmpgrantorname, @tmpgrantee, @tmpgranteename, @tmpdatabasename, @tmpschemaid, @#tmppermission, @tmpgrant, @tmpgrantwith, @tmpdeny, @tmprevoke
		end

		close myc302
		deallocate myc302
	

		--select distinct grantorname from #tmppermission where isdeny = 'Y'

	-- step 2.21
		-- CHECK COLUMN PERMISSIONS
		-- COPY ALL OBJECT EXPLICIT PERMISSION AS EFFECTIVE
		insert into #tmppermission (
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
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct   snapshotid, 
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
			'EF', 
			permission, 
			isgrant, 
			isgrantwith, 
			isrevoke, 
			isdeny, 
			parentobjectid,
			objectid,
			objectname, 
			qualifiedname,
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission
		from #tmppermission
		where UPPER(permissionlevel) = 'COL'


		delete from #tmppermission2
		drop table #tmpcovering
	
	end

	END		-- End SQL Server 2005 processing
	ELSE
	BEGIN	-- Begin SQL Server 2000 processing

		-- TODO: NEED TO EXPAND DATABASE FIXED TO COVERING OBJECT PERMISSION, SKIP SCHEMA FOR 2000

		-- GIVEN THE UID (USERS AND ROLES) NEED TO GET DATABASE PRINCIPAL PERMISSION
		-- TO GET TO OBJECTS, USE EITHER SCHEMA ID OR OWNER
		-- IF OWNER IS NULL THEN USE SCHEMAID
		-- IF OWNER AND SCHEMA ID ARE NULL THEN IT IS A COLUMN, NEED TO FIND ITS PARENT AND GET OWNER
	
	-- step 3.01
		-- GET SQL 2000 ALL OBJECTS EXCEPT COLUMN PERMISSION WHERE OWNER IS NOT NULL
		insert into #tmppermission (
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
			objectid,
			objectname, 
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission) 
		select distinct 
			a.snapshotid, 
			'OBJ',
			@logintype,
			@loginname,
			@connectionname,
			@databasename, 
			e.principalid,
			principalname=e.name,
			principaltype=e.type,
			databaseprincipal=CASE WHEN UPPER(a.isalias) = 'Y' THEN dbo.getdatabaseprincipalname(a.snapshotid, a.dbid, a.altuid) ELSE a.name END, 
			databaseprincipaltype=a.type, 
			d.grantor, 
			dbo.getdatabaseprincipalname(d.snapshotid, d.dbid, d.grantor),
			d.grantee, 
			dbo.getdatabaseprincipalname(d.snapshotid, d.dbid, d.grantee),
			d.classid,
			'EX',
			d.permission,
			d.isgrant, 
			d.isgrantwith, 
			d.isrevoke, 
			d.isdeny,
			d.objectid,
			d.objectname, 
			d.objecttype, 
			d.schemaid, 
			dbo.getschemaname(d.snapshotid, d.dbid, d.schemaid),
			d.owner,
			dbo.getdatabaseprincipalname(d.snapshotid, d.dbid, d.owner),
			a.isalias,
			'N',
			d.objectname,
			d.objecttype,
			d.permission
		from 
			databaseprincipal a left outer join serverprincipal e on a.snapshotid = e.snapshotid and a.usersid = e.sid, 
			#tmpuid c, 
			vwdatabaseobjectpermission d
		where 
			a.snapshotid = @snapshotid and 
			a.dbid = @databaseid and 
			(a.uid = c.uid or (UPPER(a.isalias) = 'Y' and a.altuid = c.uid)) and 
			a.dbid = c.dbid and
			d.snapshotid = a.snapshotid and 
			d.grantee = c.uid and 
			d.dbid = c.dbid and  
			d.owner is not null and
			d.classid <> 0 and 
			(d.schemaid = 0 or d.schemaid is null)
	
	-- step 3.02
		-- GET SQL 2000 COLUMN PERMISSION WHERE OWNER IS NULL, SO USE PARENTOBJECT ID TO GET OWNER
		insert into #tmppermission (
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
			objecttype, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission) 
		select distinct 
			a.snapshotid, 
			'COL',
			@logintype,
			@loginname,
			@connectionname,
			@databasename, 
			e.principalid,
			principalname=e.name,
			principaltype=e.type,
			databaseprincipal=CASE WHEN UPPER(a.isalias) = 'Y' THEN dbo.getdatabaseprincipalname(a.snapshotid, a.dbid, a.altuid) ELSE a.name END, 
			databaseprincipaltype=a.type, 
			d.grantor, 
			dbo.getdatabaseprincipalname(d.snapshotid, d.dbid, d.grantor),
			d.grantee, 
			dbo.getdatabaseprincipalname(d.snapshotid, d.dbid, d.grantee),
			d.classid,
			'EX',
			d.permission,
			d.isgrant, 
			d.isgrantwith, 
			d.isrevoke, 
			d.isdeny,
			d.parentobjectid,
			d.objectid,
			d.objectname, 
			dbo.gettablename(@snapshotid, @databaseid, d.parentobjectid) + '.' + d.objectname,
			d.objecttype, 
			d.schemaid, 
			dbo.getschemaname(d.snapshotid, d.dbid, d.schemaid),
			d.owner,
			dbo.getdatabaseprincipalname(d.snapshotid, d.dbid, d.owner),
			a.isalias,
			'N',
			dbo.gettablename(@snapshotid, @databaseid, d.parentobjectid) + '.' + d.objectname,
			d.objecttype, 
			d.permission
		from
			databaseprincipal a left outer join serverprincipal e on a.snapshotid = e.snapshotid and a.usersid = e.sid, 
			#tmpuid c, 
			vwdatabasecolumnpermission d
		where
			a.snapshotid = @snapshotid and 
			a.dbid = @databaseid and 
			(a.uid = c.uid or (UPPER(a.isalias) = 'Y' and a.altuid = c.uid)) and 
			c.dbid = a.dbid and
			d.snapshotid = a.snapshotid and 
			d.grantee = c.uid and 
			d.dbid = c.dbid and  
			(d.schemaid = 0 or d.schemaid is null)

	-- step 3.03
		if (@issysadminrole = 'Y')
		begin
			insert into #tmppermission (
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
				objectid,
				objectname, 
				qualifiedname,
				objecttype, 
				schemaid, 
				schemaname,
				owner,
				ownername,
				isaliased,
				coveringfrom,
				inherited,
				sourcename,
				sourcetype,
				sourcepermission)
			values ( 
				@snapshotid, 
				'SV',
				'S', 
				@loginname, 
				@connectionname, 
				NULL, 
				1, 
				'sysadmin', 
				'S', 
				NULL, 
				NULL, 
				1, 
				'sa', 
				1, 
				'sysadmin', 
				100, 
				'EX',
				'CONTROL', 
				'Y', 
				'Y',
				'N', 
				'N', 
				1,
				@connectionname,
				@connectionname,
				'iSRV',
				NULL, 
				NULL,
				NULL,
				NULL,
				'N',
				'SV',
				'N',
				'sysadmin',
				'Server Role',
				'CONTROL'
				)
	
			insert into #tmppermission (
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
				objectid,
				objectname, 
				qualifiedname,
				objecttype, 
				schemaid, 
				schemaname,
				owner,
				ownername,
				isaliased,
				coveringfrom,
				inherited,
				sourcename,
				sourcetype,
				sourcepermission)
			values ( 
				@snapshotid, 
				'SV',
				'S', 
				@loginname, 
				@connectionname, 
				NULL, 
				1, 
				'sa', 
				'S', 
				NULL, 
				NULL, 
				1, 
				'sa', 
				1, 
				'sysadmin', 
				100, 
				'EF',
				'CONTROL', 
				'Y', 
				'Y',
				'N', 
				'N', 
				1,
				@connectionname,
				@connectionname,
				'iSRV',
				NULL, 
				NULL,
				NULL,
				NULL,
				'N',
				'SV',
				'N',
				'sysadmin',
				'Server Role',
				'CONTROL'
				)
	
			insert into #tmppermission (
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
				objectid,
				objectname, 
				qualifiedname,
				objecttype, 
				schemaid, 
				schemaname,
				owner,
				ownername,
				isaliased,
				coveringfrom,
				inherited,
				sourcename,
				sourcetype,
				sourcepermission)
			values ( 
				@snapshotid, 
				'DB',
				'S', 
				@loginname, 
				@connectionname, 
				@databasename, 
				1, 
				'sa', 
				'S', 
				NULL, 
				NULL, 
				1, 
				'sa', 
				1, 
				'sysadmin', 
				0, 
				'EF',
				'CONTROL', 
				'Y', 
				'Y',
				'N', 
				'N', 
				1,
				@databasename,
				@databasename,
				'DB',
				NULL, 
				NULL,
				NULL,
				NULL,
				'N',
				'SV',
				'Y',
				'sysadmin',
				'Server Role',
				'CONTROL'
				)
		end

	-- step 3.04a	Server fixed role
		if (@geteffective = 'Y' and @issysadminrole = 'N')	
		begin

			insert into #tmppermission (
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
				objectid,
				objectname, 
				objecttype, 
				schemaid, 
				schemaname,
				owner,
				ownername,
				isaliased,
				coveringfrom,
				inherited,
				sourcename,
				sourcetype,
				sourcepermission)
			select  distinct   a.snapshotid, 
				'SV',
				@logintype, 
				@loginname, 
				@connectionname, 
				NULL, 
				a.memberprincipalid, 
				dbo.getserverprincipalname(a.snapshotid, a.memberprincipalid), 
				dbo.getserverprincipaltype(a.snapshotid, a.memberprincipalid), 
				NULL, 
				NULL, 
				a.principalid, 
				dbo.getserverprincipalname(a.snapshotid, a.principalid), 
				a.memberprincipalid, 
				dbo.getserverprincipalname(a.snapshotid, a.memberprincipalid), 
				100, 
				'EF', 
				rolepermission, 
				c.isgrant, 
				c.isgrantwith, 
				c.isrevoke, 
				c.isdeny, 
				0,
				@connectionname, 
				'iSRV', 
				NULL, 
				NULL,
				NULL,
				NULL,
				'N',
				'FXROLE',
				'N',
				c.rolename,
				'Server Role',
				rolepermission
			from 
				vwfixedserverrolemember a, 
				#tmplogins b ,
				fixedrolepermission c
			where 
				a.snapshotid = @snapshotid and 
				a.memberprincipalid = b.principalid and
				UPPER(a.name) = UPPER(c.rolename) and
				c.roletype = 'P'	

	-- step 3.05
			-- PROCESS FIXED DATABASE ROLE PERMISSION
			-- GET ALL EFFECTIVE PERMISSION ASSOCIATES WITH THE FIXED DATABASE ROLE AND INSERT THEM AS EFFECTIVE PERMISSIONS
			insert into #tmppermission (
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
				objectid,
				objectname, 
				objecttype, 
				schemaid, 
				schemaname,
				owner,
				ownername,
				isaliased,
				coveringfrom,
				inherited,
				sourcename,
				sourcetype,
				sourcepermission)
			select distinct    a.snapshotid, 
				'DB',
				@logintype, 
				@loginname, 
				@connectionname, 
				d.databasename, 
				f.principalid, 
				f.name, 
				f.type, 
				dbo.getdatabaseprincipalname(e.snapshotid, e.dbid, e.uid), 
				e.type, 
				a.uid, 
				a.name, 
				b.uid, 
				dbo.getdatabaseprincipalname(a.snapshotid, a.dbid, a.rolememberuid), 
				0, 
				'EF', 
				rolepermission, 
				c.isgrant, 
				c.isgrantwith, 
				c.isrevoke, 
				c.isdeny, 
				d.dbid,
				d.databasename, 
				'DB', 
				NULL, 
				NULL,
				dbo.getdatabaseprincipalid(d.snapshotid, d.dbid, d.owner),
				d.owner, 
				'N',
				'DBFXROLE',
				'N',
				c.rolename,
				'iDRLE',
				rolepermission
			from 
				vwfixeddatabaserolemember a, 
				#tmpuid b,
				fixedrolepermission c,
				sqldatabase d,
				databaseprincipal e left outer join serverprincipal f on e.snapshotid = f.snapshotid and e.usersid = f.sid
			where 
				a.snapshotid = @snapshotid and 
				a.dbid = b.dbid and 
				a.rolememberuid = b.uid and
				e.snapshotid = a.snapshotid and
				e.uid = b.uid and 
				e.dbid = b.dbid and
				UPPER(a.name) = UPPER(c.rolename) and
				d.dbid = a.dbid and
				d.snapshotid = a.snapshotid and
				d.databasename = @databasename and 
				c.roletype = 'D'
		
			insert into #tmppermission (
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
				objectid,
				objectname, 
				objecttype, 
				schemaid, 
				schemaname,
				owner,
				ownername,
				isaliased,
				inherited,
				sourcename,
				sourcetype,
				sourcepermission)
			select  distinct   snapshotid, 
				a.permissionlevel,
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
				'EF', 
				b.permissionname, 
				isgrant, 
				isgrantwith, 
				isrevoke, 
				isdeny, 
				objectid,
				objectname, 
				objecttype, 
				schemaid, 
				schemaname,
				owner,
				ownername,
				isaliased,
				inherited,
				sourcename,
				sourcetype,
				sourcepermission
			from #tmppermission a, coveringpermissionhierarchy b
			where a.classid = 0 and UPPER(b.permissionlevel) = 'OBJECT' and b.coveringpermissionname = a.permission

	-- step 3.06
			-- FOR 2000, ALL EXPLICIT ARE EFFECTIVE PERMISSION
			insert into #tmppermission (
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
				objecttype, 
				schemaid, 
				schemaname,
				owner,
				ownername,
				isaliased,
				inherited,
				sourcename,
				sourcetype,
				sourcepermission)
			select distinct    snapshotid, 
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
				'EF', 
				permission, 
				isgrant, 
				isgrantwith, 
				isrevoke, 
				isdeny, 
				parentobjectid,
				objectid,
				objectname, 
				qualifiedname,
				objecttype, 
				schemaid, 
				schemaname,
				owner,
				ownername,
				isaliased,
				inherited,
				sourcename,
				sourcetype,
				sourcepermission
			from #tmppermission

			--select * from #tmppermission

			-- Apply database fixed role permission to all objects
			insert into #tmppermission (
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
				objectid,
				objectname, 
				objecttype, 
				schemaid, 
				schemaname,
				owner,
				ownername,
				isaliased,
				inherited,
				sourcename,
				sourcetype,
				sourcepermission)
			select  distinct b.snapshotid, 
				'OBJ',
				b.logintype, 
				b.loginname, 
				b.connectionname, 
				b.databasename, 
				b.principalid, 
				b.principalname, 
				b.principaltype,
				b.databaseprincipal, 
				b.databaseprincipaltype, 
				b.grantor, 
				b.grantorname,
				b.grantee, 
				b.granteename,
				a.classid, 
				'EF', 
				b.permission, 
				b.isgrant, 
				b.isgrantwith, 
				b.isrevoke, 
				b.isdeny, 
				a.objectid,
				a.name, 
				a.type, 
				b.schemaid, 
				b.schemaname,
				a.owner,
				dbo.getdatabaseprincipalname(a.snapshotid, a.dbid, a.owner), 
				b.isaliased,
				'Y',
				sourcename,
				sourcetype,
				sourcepermission
			from databaseobject a, #tmppermission b
			where 
				a.snapshotid = @snapshotid and
				a.dbid = @dbid and
				a.snapshotid = b.snapshotid and
				a.classid = 1 and 
				b.objecttype ='DB' and
				a.type <> 'iCO' and
				b.permission in ('SELECT', 'INSERT', 'DELETE', 'UPDATE', 'REFERENCES', 'EXECUTE')
	
	-- step 3.07   ?????
			-- remove all databse fixed role
			delete from #tmppermission where permissionlevel = 'DB' and permission not in ('CREATE TABLE', 'CREATE PROCEDURE', 'CREATE VIEW', 'CREATE DEFAULT', 'CREATE RULE', 'CREATE FUNCTION', 'BACKUP DATABASE', 'BACKUP LOG')
			delete from #tmppermission where permissionlevel = 'DB' and permission not in ('CREATE TABLE', 'CREATE PROCEDURE', 'CREATE VIEW', 'CREATE DEFAULT', 'CREATE RULE', 'CREATE FUNCTION', 'BACKUP DATABASE', 'BACKUP LOG')
			delete from #tmppermission where objecttype = 'P' and UPPER(permission) NOT IN ('ALTER', 'CONTROL', 'EXECUTE', 'TAKE OWNERSHIP', 'VIEW DEFINITION')
			delete from #tmppermission where objecttype = 'FN' and UPPER(permission) NOT IN ('REFERENCES', 'EXECUTE')
			delete from #tmppermission where objecttype = 'iCO' and UPPER(permission) NOT IN ('REFERENCES', 'SELECT', 'UPDATE')
	
		end
	END		-- End SQL Server 2000 processing

	-- step 4.01
	-- DELETE PERMISSIONS ONLY APPLICABLE TO CERTAIN TYPE OF OBJECTS
	-- LIKE TABLE SHOULD NOT HAVE EXECUTE OR SP SHOULD NOT HAVE SELECT
	if exists (select 1 from #tmppermission where objecttype IN ('U', 'V') and UPPER(permission) NOT IN ('ALTER', 'CONTROL', 'DELETE', 'INSERT', 'REFERENCES', 'SELECT', 'TAKE OWNERSHIP', 'UPDATE', 'VIEW CHANGE TRACKING', 'VIEW DEFINITION'))
		delete from #tmppermission where objecttype IN ('U', 'V') and UPPER(permission) NOT IN ('ALTER', 'CONTROL', 'DELETE', 'INSERT', 'REFERENCES', 'SELECT', 'TAKE OWNERSHIP', 'UPDATE', 'VIEW CHANGE TRACKING', 'VIEW DEFINITION')

	if exists (select 1 from #tmppermission where objecttype IN ('U', 'S', 'iSCM') and UPPER(permission) = 'VIEW CHANGE TRACKING')
		delete from #tmppermission where objecttype IN ('U', 'S', 'iSCM') and UPPER(permission) = 'VIEW CHANGE TRACKING'

	if exists (select 1 from #tmppermission where objecttype = 'iCO' and UPPER(permission) NOT IN ('REFERENCES', 'SELECT', 'UPDATE'))
		delete from #tmppermission where objecttype = 'iCO' and UPPER(permission) NOT IN ('REFERENCES', 'SELECT', 'UPDATE')

	if exists (select 1 from #tmppermission where objecttype = 'P' and UPPER(permission) NOT IN ('ALTER', 'CONTROL', 'EXECUTE', 'TAKE OWNERSHIP', 'VIEW DEFINITION'))
		delete from #tmppermission where objecttype = 'P' and UPPER(permission) NOT IN ('ALTER', 'CONTROL', 'EXECUTE', 'TAKE OWNERSHIP', 'VIEW DEFINITION')

	-- step 4.02
	-- MORE FILTERING LATER
	if (@geteffective = 'Y')
	begin
		if (@issysadminrole = 'Y')
		begin
			-- IF SYSADMIN ROLE THEN ALL DENY CANNOT OVERRIDE THE GRANT PERMISSION
			-- SYSADMIN HAVE ALL PRIVILEGES
			update #tmppermission set isdeny='N', isgrant='Y' where permissiontype = 'EF' and isdeny = 'Y'
		end
		else
		begin

	-- step 4.03
			-- LAST STEP IS TO NEGATE THE PERMISSION BASED ON DBID, OBJECT, SNAPSHOTID, PERMISSION NAME
			insert into #tmpdenypermission (
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
				permission, 
				parentobjectid,
				objectid, 
				objectname,
				objecttype, 
				schemaid, 
				schemaname,
				owner,
				ownername)
			select distinct
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
				permission, 
				parentobjectid,
				objectid, 
				objectname,
				objecttype, 
				schemaid, 
				schemaname,
				owner,
				ownername
			from #tmppermission
			where 
				permissiontype = 'EF' and 
				isdeny = 'Y' 

			-- SQL SERVER SYNTAX ONLY DELETE
			-- DELETE ALL THE GRANT OR GRANT ALL PERMISSION THAT HAVE A DENY
			-- THE PERMISSION WITH DENY IS NOT DELETED

			delete #tmppermission 
			from #tmppermission a, #tmpdenypermission b
			where 
				a.snapshotid = b.snapshotid and 
				a.logintype = b.logintype and  
				ISNULL(a.loginname, '') = ISNULL(b.loginname, '') and  
				a.connectionname = b.connectionname and  
				ISNULL(a.databasename, '') = ISNULL(b.databasename, '') and  
				a.classid = b.classid and  
				a.permission = b.permission and  
				ISNULL(a.parentobjectid, '') = ISNULL(b.parentobjectid, '') and 
				a.objectid = b.objectid and  
				a.objectname = b.objectname and 
				a.objecttype = b.objecttype and  
				ISNULL(a.schemaid, '') = ISNULL(b.schemaid, '') and
				a.permissiontype = 'EF' and a.isdeny = 'N' 				
		end
	end

	-- if only effective then remove explicit permission before returning
	if (@permissiontype = 'E')
	begin
		delete from #tmppermission where permissiontype = 'EX'
	end

	-- FOR ALIAS USERS, ONLY THOSE BELONGS TO LOGIN MEMBERSHIP
	delete from #tmppermission from #tmppermission a, #tmplogins b where a.isaliased = 'Y' and a.principalid <> b.principalid

	-- UPDATE THE QUALIFIED NAME FOR ALL OBJECTS EXCEPT COLUMN TYPE
	update #tmppermission set qualifiedname = objectname where permissionlevel <> 'COL'

	-- If there is a high level table (avoid nested sql server error), simply insert to the parent table
	-- This is a way to collect data from tmppermission
	if exists (select 'x' from tempdb..sysobjects where type = 'U' and lower(name) like '#tmpserverpermission%')
	begin
		insert into #tmpserverpermission select  distinct  * from #tmppermission


	end
	else -- else just show all data, the caller is UI
	begin

	exec ('select distinct 
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
			inherited,
			sourcename,
			sourcetype,
			sourcetypename=CASE WHEN c.objecttypename IS NULL THEN sourcetype ELSE c.objecttypename END,
			sourcepermission
			from #tmppermission a left outer join objecttype b on a.objecttype = b.objecttype
								left outer join objecttype c on a.sourcetype = c.objecttype
			')

	end

	drop table #tmplogins
	drop table #tmpuid
	drop table #tmppermission
	drop table #tmppermission2
	drop table #tmpdenypermission

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_getuserpermission] TO [SQLSecureView]

GO
