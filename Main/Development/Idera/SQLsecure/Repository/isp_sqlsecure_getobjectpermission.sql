SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_getobjectpermission]'))
drop procedure [dbo].[isp_sqlsecure_getobjectpermission]
GO

CREATE procedure [dbo].[isp_sqlsecure_getobjectpermission]
(
	@snapshotid int,
	@databaseid int=-1,
	@objectid int=-1,
	@classid int,
	@permissiontype nchar(1)=null,
	@iscolumn nchar(1)='N',
	@parentobjectid int=null
)
--WITH ENCRYPTION
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get all object permission given the snapshotid, sid or sql login

	declare @loginname nvarchar(128)
	declare @logintype nvarchar(1)
	declare @inputsid varbinary(128)
	declare @inputprincipalid int
	declare @connectionname nvarchar(400)
	declare @errmsg nvarchar(500)
	declare @sql2000 nvarchar(1)
	declare @geteffective nchar(1)
	declare @databasename nvarchar(128)
	declare @principalid int
	declare @uid int

	if (@permissiontype = 'B' or @permissiontype IS NULL)
		set @geteffective = 'Y'
	else if (@permissiontype = 'X')
		set @geteffective = 'N'
	else if (@permissiontype = 'E')
		set @geteffective = 'Y'

	if (@iscolumn IS NULL)
		set @iscolumn = 'Y'

	-- if there is not such snapshotid then return error
	if not exists (select * from serversnapshot where snapshotid = @snapshotid)
	begin
		set @errmsg = 'Error: Snapshot id ' + CONVERT(varchar(10), @snapshotid) + ' not found'
		RAISERROR (@errmsg, 16, 1)
		return -1
	end

	if (@classid < 0)
	begin
		set @errmsg = 'Error: Class id cannot be less than zero'
		RAISERROR (@errmsg, 16, 1)
		return -1
	end

	if not exists (select 1 from classtype where classid = @classid)
	begin
		set @errmsg = 'Error: Invalid class id'
		RAISERROR (@errmsg, 16, 1)
		return -1
	end

	-- check if database name is not found
	if (@classid not in (100, 101, 105,108))
	begin
		if (@databaseid < 0)
		begin
			set @errmsg = 'Error: Invalid database name'
			RAISERROR (@errmsg, 16, 1)
			return
		end
	
	end

	select @databasename = databasename from sqldatabase where snapshotid = @snapshotid and dbid = @databaseid

	-- check if the snapshot server is sql 2000 or sql 2005, it is important to differentiate them for getting column permission processing
	select @sql2000 = dbo.issql2000(@snapshotid)

	create table #tmplogins (sid varbinary(85), principalid int, name nvarchar(128), type nchar(1), serveraccess nchar(1), serverdeny nchar(1), disabled nchar(1))
	create table #tmppermission (snapshotid int, permissionlevel nvarchar(15), logintype nchar(1), loginname nvarchar(256), connectionname nvarchar(400), databasename nvarchar(256), principalid int, principalname nvarchar(128), principaltype nchar(1), databaseprincipal nvarchar(128), databaseprincipaltype nchar(1), grantor int, grantorname nvarchar(128), grantee int, granteename nvarchar(128), classid int, permissiontype nvarchar(4), coveringfrom nvarchar(15), permission nvarchar(256), isgrant nchar(1), isgrantwith nchar(1), isrevoke nchar(1), isdeny nchar(1), parentobjectid int, objectid int, objectname nvarchar(256), objecttype nvarchar(64), objecttypename nvarchar(256), schemaid int, schemaname nvarchar(128), owner int, ownername nvarchar(128), isaliased nchar(1), inherited nchar(1), sourcename nvarchar(256), sourcetype nvarchar(256), sourcepermission nvarchar(256))
	create table #tmppermission2 (snapshotid int, permissionlevel nvarchar(15), logintype nchar(1), loginname nvarchar(256), connectionname nvarchar(400), databasename nvarchar(256), principalid int, principalname nvarchar(128), principaltype nchar(1), databaseprincipal nvarchar(128), databaseprincipaltype nchar(1), grantor int, grantorname nvarchar(128), grantee int, granteename nvarchar(128), classid int, permissiontype nvarchar(4), coveringfrom nvarchar(15), permission nvarchar(256), isgrant nchar(1), isgrantwith nchar(1), isrevoke nchar(1), isdeny nchar(1), parentobjectid int, objectid int, objectname nvarchar(256), objecttype nvarchar(64), objecttypename nvarchar(256), schemaid int, schemaname nvarchar(128), owner int, ownername nvarchar(128), isaliased nchar(1), inherited nchar(1), sourcename nvarchar(256), sourcetype nvarchar(256), sourcepermission nvarchar(256))
	create table #tmpdenypermission (snapshotid int, permissionlevel nvarchar(15), logintype nchar(1), loginname nvarchar(256), connectionname nvarchar(400), databasename nvarchar(256), principalid int, principalname nvarchar(128), principaltype nchar(1), databaseprincipal nvarchar(128), databaseprincipaltype nchar(1), grantor int, grantorname nvarchar(128), grantee int, granteename nvarchar(128), classid int, permissiontype nvarchar(4), coveringfrom nvarchar(15), permission nvarchar(256), isgrant nchar(1), isgrantwith nchar(1), isrevoke nchar(1), isdeny nchar(1), parentobjectid int, objectid int, objectname nvarchar(256), objecttype nvarchar(64), objecttypename nvarchar(256), schemaid int, schemaname nvarchar(128), owner int, ownername nvarchar(128), isaliased nchar(1), inherited nchar(1), sourcename nvarchar(256), sourcetype nvarchar(256), sourcepermission nvarchar(256))

	create table #tmpsysadmin (principalid int, sysadmin nchar(1))

	-- TODO: FIND ALL THE LOGINS ASSOCIATED WITH ALL THE DATABASE USERS

	if (@classid IN (100, 105,108))
	begin

		declare cursor1 cursor for
				select distinct a.principalid, a.name, a.sid, a.type from serverprincipal a where a.snapshotid = @snapshotid
	
	end
	else if (@classid = 101)
	begin

		declare cursor1 cursor for
				select distinct a.principalid, a.name, a.sid, a.type from serverprincipal a where a.snapshotid = @snapshotid and a.principalid = @objectid
	
	end
	else
	begin
		declare cursor1 cursor for
				select distinct a.principalid, a.name, a.sid, a.type from serverprincipal a, databaseprincipal b where a.snapshotid = @snapshotid and b.usersid = a.sid and b.snapshotid = a.snapshotid and UPPER(b.type) IN ('U', 'G', 'R', 'S') and a.sid IS NOT NULL and b.dbid = @databaseid

	end

	open cursor1
	fetch next from cursor1
	into @inputprincipalid, @loginname, @inputsid, @logintype
	
	while @@fetch_status = 0
	begin

		if (@logintype = 'W')
		begin
			-- checks if the login exists in sql server
			create table #tmpsid (sid varbinary(85))
		
			-- insert current login to tmp table
			insert into #tmplogins (sid, principalid, name, type, serveraccess, serverdeny, disabled) (select distinct /* ssz add distinct */  sid, principalid, name, type, serveraccess, serverdeny, disabled from serverprincipal where snapshotid = @snapshotid and sid = @inputsid)
		
			-- get all windows parents groups
			insert into #tmpsid exec isp_sqlsecure_getwindowsgroupparents @snapshotid, @inputsid
		
			-- insert all groups in serverprincipal table
			insert into #tmplogins (sid, principalid, name, type, serveraccess, disabled) (select distinct /* ssz add distinct */  a.sid, a.principalid, a.name, a.type, a.serveraccess, a.disabled from serverprincipal a, #tmpsid b where a.serveraccess = 'Y' and a.serverdeny = 'N' and a.snapshotid = @snapshotid and a.sid = b.sid)

			drop table #tmpsid				
		end
		else -- sql login type
		begin
			insert into #tmplogins (sid, principalid, name, type, serveraccess, serverdeny, disabled) (select distinct /* ssz add distinct */  a.sid, a.principalid, a.name, a.type, a.serveraccess, a.serverdeny, a.disabled from serverprincipal a where a.snapshotid = @snapshotid and name=@loginname)
		end

	fetch next from cursor1
	into @inputprincipalid, @loginname, @inputsid, @logintype

	end

	close cursor1
	deallocate cursor1

	select @connectionname = connectionname from serversnapshot where snapshotid = @snapshotid
	
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
	objecttypename,
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
	'iSRV',
	dbo.getclasstype(d.classid),
	'N',
	@connectionname,
	'Server',
	d.permission
	from 
	serverprincipal a, 
	serverpermission d 
	where 
	a.snapshotid = @snapshotid and 
	d.snapshotid = a.snapshotid and
	d.grantee = a.principalid and 
	d.classid = 100 and 
	a.serveraccess = 'Y' and
	a.serverdeny = 'N' and 
	(a.principalid in (select distinct /* ssz add distinct */  principalid from #tmplogins) or 
	 (a.principalid in (select distinct /* ssz add distinct */  principalid from serverrolemember where memberprincipalid in (select distinct /* ssz add distinct */  principalid from #tmplogins))))

	--TODO: if class id is 100 the return all the permission for the server
	if (@classid = 100 and UPPER(@permissiontype) = 'X')
	begin
		exec ('isp_sqlsecure_processpermission')

		exec ('select distinct * from #tmppermission')
		return
	end
	
	-- get all the database users
	create table #tmpuid (dbid int, uid int)
	insert into #tmpuid (dbid, uid) select distinct dbid, uid from databaseprincipal where snapshotid = @snapshotid and dbid = @databaseid and UPPER(name) not in ('DB_ACCESSADMIN', 'DB_BACKUPOPERATOR', 'DB_DATAREADER', 'DB_DATAWRITER', 'DB_DDLADMIN', 'DB_DENYDATAREADER', 'DB_DENYDATAWRITER', 'DB_OWNER', 'DB_SECURITYADMIN')

/*	declare @dbid int
	declare @uid int

	declare myc100 cursor for
			select distinct a.dbid, a.uid from databaseprincipal a where a.snapshotid = @snapshotid and a.dbid = @databaseid 
	
	open myc100
	fetch next from myc100
	into @dbid, @uid
	
	while @@fetch_status = 0
	begin		
		print '============ gettting uid =========='

		insert into #tmpuid exec isp_sqlsecure_getdatabaseuserparents @snapshotid, @dbid, @uid

		insert into #tmpuid (dbid, uid) select dbid, uid from databaseprincipal where dbid = @dbid and type IN ('R', 'A')

		fetch next from myc100
		into @dbid, @uid

	end
	
	close myc100
	deallocate myc100	
*/

	-- check if user 'guest' is valid. If so, then current login will have public database role even there is
   	-- no databse user map to it.
	if exists (select * from databaseprincipal a where UPPER(a.name) = 'GUEST' and a.snapshotid = @snapshotid and a.dbid = @databaseid) 
	begin
		-- public uid is always 0
		insert into #tmpuid (dbid, uid) values (@databaseid, 0)
		
		-- insert guest user as well
		insert into #tmpuid (dbid, uid) select distinct dbid, uid from databaseprincipal a where UPPER(a.name) = 'GUEST' and snapshotid = @snapshotid and dbid = @databaseid
	end

	--select * from #tmpuid

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
	objecttypename,
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
	'Database',
	NULL, 
	NULL,
	dbo.getdatabaseprincipalid(d.snapshotid, d.dbid, d.owner),
	b.owner,
	a.isalias,
	'N',
	b.databasename,
	'Database',
	d.permission
	from 
	databaseprincipal a left outer join serverprincipal e on a.snapshotid = e.snapshotid and a.usersid = e.sid, 
	sqldatabase b,
	#tmpuid c, 
	vwdatabasepermission d
	where 
	a.snapshotid = @snapshotid and 
	(a.uid = c.uid or a.altuid = c.uid) and 
	a.dbid = c.dbid and
	d.snapshotid = a.snapshotid and 
	d.classid = 0 and 
	d.grantee = c.uid and
	d.dbid = c.dbid and  
	a.dbid = b.dbid and
	b.snapshotid = a.snapshotid and
	b.dbid = @databaseid 

	if (@classid = 0 and UPPER(@permissiontype) = 'X')
	begin
		exec ('isp_sqlsecure_processpermission')

		exec ('select * from #tmppermission where classid=0 and objectid=' + @objectid)
		return
	end

	-- process for 2005 and above
	if (@sql2000 = 'N')
	begin
		if (@classid = 101)
		begin

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
			objecttypename,
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
			dbo.getclasstype(d.classid),
			'N',
			a.name,
			'Login',
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
			(d.grantee in (select distinct /* ssz add distinct */  principalid from #tmplogins) or 
			 (d.grantee in (select distinct /* ssz add distinct */  principalid from serverrolemember where memberprincipalid in (select distinct /* ssz add distinct */  principalid from #tmplogins))))
		end

		--select * from #tmppermission

		if (@classid = 101 and UPPER(@permissiontype) = 'X')
		begin
			exec ('isp_sqlsecure_processpermission')

			exec ('select distinct * from #tmppermission where classid = 101 and objectid=' + @objectid)
			return
		end

			if (@classid = 108)
		begin
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
			objecttypename,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission) 
			select distinct 
			ag.snapshotid, 
			'AV',
			@logintype, 
			@loginname, 
			@connectionname, 
			NULL, 	
			ag.servergroupId, 
			ag.name, 
			'A', 
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
			ag.name, 
			ag.servergroupId,
			dbo.getclassobjecttype(d.classid),
			dbo.getclasstype(d.classid),
			'N',
			ag.name, 
			'Login',
			d.permission
			from 
			dbo.availabilitygroups ag 
			join dbo.availabilityreplicas ar on ag.groupid = ar.groupid and ag.snapshotid = ar.snapshotid
			join serverpermission d on  d.majorid=ar.replicametadataid and ar.snapshotid = d.snapshotid
			where 
			ag.snapshotid = @snapshotid and			
			d.classid = 108 and ag.servergroupId=@objectid and 
			(d.grantee in (select distinct /* ssz add distinct */  principalid from #tmplogins) or 
			 (d.grantee in (select distinct /* ssz add distinct */  principalid from serverrolemember where memberprincipalid in (select distinct /* ssz add distinct */  principalid from #tmplogins))))
		end
		if (@classid = 108 and UPPER(@permissiontype) = 'X')
		begin
			exec ('isp_sqlsecure_processpermission')
			exec ('select distinct * from #tmppermission where classid = 108 and objectid=' + @objectid)
			return
		end
		if (@classid = 105)
		begin

			-- ENDPOINT PERMISSION
			-- REQUIRES SOME CUSTOMIZATION BECAUSE THE ENDPOINT COVERING HAS AT LEAST A CONNECT PERMISSION EVEN THERE IS NO PARENT PERMISSION
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
			objecttypename,
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
			dbo.getclasstype(d.classid),
			'N',
			a.name,
			'Endpoint',
			d.permission
			from 
			endpoint a, 
			serverpermission d 
			where 
			a.snapshotid = @snapshotid and 
			d.snapshotid = a.snapshotid and
			d.majorid = a.endpointid and 
			d.classid = 105 and 
			(d.grantee in (select distinct /* ssz add distinct */  principalid from #tmplogins) or 
			 (d.grantee in (select distinct /* ssz add distinct */  principalid from serverrolemember where memberprincipalid in (select distinct /* ssz add distinct */  principalid from #tmplogins))))
		end

		if (@classid = 105 and UPPER(@permissiontype) = 'X')
		begin
			exec ('isp_sqlsecure_processpermission')

			exec ('select distinct * from #tmppermission where classid = 105 and objectid=' + @objectid)
			return
		end
		
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
		objecttypename,
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
		principalid=e.name,
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
		d.grantee,
		dbo.getdatabaseprincipalname(d.snapshotid, d.dbid, d.grantee),
		dbo.getdatabaseprincipaltype(d.snapshotid, d.dbid, d.grantee), 
		dbo.getobjecttypename('iDUSR'),
		d.uid,
		dbo.getdatabaseprincipalname(d.snapshotid, d.dbid, d.uid),
		a.isalias,
		'N',
		dbo.getdatabaseprincipalname(d.snapshotid, d.dbid, d.grantee),
		'Database User',
		d.permission
		from 
		databaseprincipal a left outer join serverprincipal e on a.snapshotid = e.snapshotid and a.usersid = e.sid, 
		#tmpuid c, 
		databaseprincipalpermission d
		where 
		a.snapshotid = @snapshotid and 
		(a.uid = c.uid or a.altuid = c.uid) and 
		a.dbid = c.dbid and
		d.snapshotid = a.snapshotid and 
		d.grantee = c.uid and 
		d.dbid = c.dbid and  
		a.dbid = @databaseid

		if (@classid = 4 and UPPER(@permissiontype) = 'X')
		begin
			exec ('isp_sqlsecure_processpermission')

			exec ('select distinct * from #tmppermission where classid = 4 and objectid=' + @objectid + ' and databasename=''' + @databasename + '''')
			return
		end

		-- TODO: GET ALL THE SCHEMA PERMISSION
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
		objecttypename, 
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
		principalid=e.name,
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
		dbo.getobjecttypename('iSCM'),
		d.schemaid, 
		d.schemaname,	
		d.uid,
		dbo.getdatabaseprincipalname(d.snapshotid, d.dbid, d.uid),
		a.isalias,
		'N',
		d.schemaname,
		'Schema',
		d.permission
		from 
		databaseprincipal a left outer join serverprincipal e on a.snapshotid = e.snapshotid and a.usersid = e.sid, 
		#tmpuid c, 
		vwschemapermission d
		where 
		a.snapshotid = @snapshotid and 
		(a.uid = c.uid or a.altuid = c.uid) and 
		a.dbid = c.dbid and
		d.snapshotid = a.snapshotid and 
		d.grantee = c.uid and 
		d.dbid = c.dbid and  
		a.dbid = @databaseid 

		if (@classid = 3 and UPPER(@permissiontype) = 'X')
		begin
			exec ('isp_sqlsecure_processpermission')

			exec ('select distinct * from #tmppermission where classid = 3 and objectid=' + @objectid + ' and databasename=''' + @databasename + '''')
			return
		end

		-- GET SQL 2005 ALL OBJECTS EXCEPT COLUMN WHERE SCHEMA IS NOT NULL AND OWNER IS NULL
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
		objecttypename, 
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
		principalid=e.name,
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
		dbo.getobjecttypename(d.objecttype),		
		d.schemaid, 
		dbo.getschemaname(d.snapshotid, d.dbid, d.schemaid),
		d.owner,
		dbo.getdatabaseprincipalname(d.snapshotid, d.dbid, d.owner),
		a.isalias,
		'N',
		d.objectname,
		dbo.getobjecttypename(d.objecttype),		
		d.permission
		from 
		databaseprincipal a left outer join serverprincipal e on a.snapshotid = e.snapshotid and a.usersid = e.sid, 
		#tmpuid c, 
		vwdatabaseobjectpermission d,
		databaseschema f
		where 
		a.snapshotid = @snapshotid and 
		(a.uid = c.uid or a.altuid = c.uid) and 
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
		a.dbid = @databaseid  and
		(d.parentobjectid = @objectid or d.objectid = @objectid)

		-- all database objects except columns
		if (@classid IN (5, 6, 10, 15, 16, 17, 18, 19, 23, 24, 25, 6) and UPPER(@permissiontype) = 'X' and UPPER(@iscolumn) = 'N')
		begin
			exec ('isp_sqlsecure_processpermission')

			exec ('select distinct * from #tmppermission where classid = ' + @classid + ' and objectid=' + @objectid + ' and databasename=''' + @databasename + '''')
			return
		end
	
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
		objecttype, 
		objecttypename,
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
		principalid=e.name,
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
		dbo.gettablename(@snapshotid, @databaseid, d.parentobjectid) + '.' + d.objectname,
		d.objecttype, 
		dbo.getobjecttypename(d.objecttype),
		d.schemaid, 
		d.schemaname,
		d.owner,
		d.ownername,
		a.isalias,
		'N',
		d.objectname,
		'Column',
		d.permission
		from 
		databaseprincipal a left outer join serverprincipal e on a.snapshotid = e.snapshotid and a.usersid = e.sid, 
		#tmpuid c, 
		vwsql2005databasecolumnpermission d
		where 
		a.snapshotid = @snapshotid and 
		(a.uid = c.uid or a.altuid = c.uid) and 
		a.dbid = c.dbid and
		d.snapshotid = a.snapshotid and 
		d.dbid = a.dbid and
		d.grantee = a.uid and
		a.dbid = @databaseid 	

		-- for table, return all columns as well
		if (@classid = 1 and UPPER(@permissiontype) = 'X' and UPPER(@iscolumn) = 'N')
		begin
			exec ('isp_sqlsecure_processpermission')

			exec ('select distinct * from #tmppermission where classid = 1 and (parentobjectid=' + @objectid + ' or objectid=' + @objectid + ') and databasename=''' + @databasename + '''')
			return
		end

		-- all columns
		if (@classid = 1 and UPPER(@permissiontype) = 'X' and UPPER(@iscolumn) = 'Y')
		begin
			exec ('isp_sqlsecure_processpermission')

			exec ('select distinct * from #tmppermission where classid = 1 and parentobjectid=' + @parentobjectid + ' and objectid=' + @objectid + ' and databasename=''' + @databasename + '''')
			return
		end

		if (@geteffective = 'Y')
		begin
	
		-- PROCESS FIXED SERVER ROLE PERMISSION
		-- CHECK IF LOGINS BELONGS TO FIXED SERVERR ROLES, IF SO THEN ASSIGN FIXED PERMISSION TO SERVER LEVEL
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
			objecttypename,
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
		select  distinct /* ssz add distinct */  a.snapshotid, 
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
			'Y', -- force it 
			c.isgrantwith, 
			c.isrevoke, 
			c.isdeny, 
			0,
			@connectionname, 
			dbo.getclassobjecttype(100), 
			dbo.getclasstype(100), 
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


		declare @tmpsysprincipalid int

		-- TODO: COPY ALL EXISTING SERVER PERMISSION AS EFFECTIVE PERMISSION
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
			objecttypename,
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select distinct /* ssz add distinct */   snapshotid, 
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
			objecttypename,
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
			objecttypename,
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct /* ssz add distinct */  snapshotid, 
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
			objecttypename,
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
		where classid = 100 and b.permissionlevel ='SERVER' and b.coveringpermissionname = a.permission

		-- need to break down again for level 2 server level permissions
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
			objecttypename,
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct /* ssz add distinct */  snapshotid, 
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
			objecttypename,
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
		where classid = 100 and b.permissionlevel ='SERVER' and b.coveringpermissionname = a.permission and UPPER(a.permission) in ('ALTER ANY DATABASE', 'ALTER ANY ENDPOINT', 'VIEW ANY DEFINITION', 'ALTER SERVER STATE', 'ALTER ANY EVENT NOTIFICATION', 'UNSAFE ASSEMBLY')

		if (@classid = 100)
		begin
			exec ('isp_sqlsecure_processpermission')

			if (UPPER(@permissiontype) = 'E')
				exec ('select distinct * from #tmppermission where classid = 100 and permissiontype = ''EF''')
			else
				exec ('select distinct * from #tmppermission where classid = 100')

			return
		end

		if (@classid = 105)
		begin

		declare @endpointid int
		declare @endpointprincipalid int
		declare @endpointname nvarchar(128)

		-- TODO: COPY ALL THE ENDPOINT PERMISSION AS EFFECTIVE PERMISSION	
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
			objecttypename,
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct /* ssz add distinct */  snapshotid, 
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
			objecttypename,
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

		-- TODO: CHECK IF ENDPOINT NEEDS COVERING FROM SERVER
		if exists (select * from #tmppermission where classid = 100 and UPPER(permission) = 'CONTROL SERVER') 
		begin
			-- IF SERVER HAS CONTROL PERMISSION ALL ENDPOINT WILL HAVE ALL PERMISSIONS
			delete from #tmppermission where UPPER(objecttype) = 'IENDP' and permissiontype = 'EF'

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
					objecttypename,
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
					'Endpoint',
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
				objecttypename, 
				schemaid, 
				schemaname,
				owner,
				ownername,
				isaliased,
				inherited,
				sourcename,
				sourcetype,
				sourcepermission)
			select  distinct /* ssz add distinct */  snapshotid, 
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
				objecttypename, 
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
						objecttypename, 
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
						'Endpoint',
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
						objecttypename, 
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
						'Endpoint',
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
					where a.classid = 100
				end

				fetch next from myc1000
				into @endpointid, @endpointprincipalid, @endpointname
			end

			close myc1000
			deallocate myc1000	
	
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
				objecttypename, 
				schemaid, 
				schemaname,
				owner,
				ownername,
				isaliased,
				inherited,
				sourcename,
				sourcetype,
				sourcepermission)
			select distinct /* ssz add distinct */   snapshotid, 
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
				objecttypename, 
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
			where a.classid = 105 and UPPER(b.permissionlevel) = 'ENDPOINT' and b.coveringpermissionname = a.permission
		end

		if (@classid = 105)
		begin
			exec ('isp_sqlsecure_processpermission')

			if (UPPER(@permissiontype) = 'E')
				exec ('select distinct * from #tmppermission where classid = 105 and permissiontype =''EF'' and objectid=' + @objectid)
			else
				exec ('select distinct * from #tmppermission where classid = 105 and objectid=' + @objectid)

			return
		end

		end

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

		if (@classid = 101)
		begin

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
						select distinct a.principalid, a.name, a.type from serverprincipal a where snapshotid = @snapshotid and a.principalid in (select  distinct /* ssz add distinct */ principalid from #tmplogins) or (a.principalid in (select distinct /* ssz add distinct */  principalid from serverrolemember where memberprincipalid in (select distinct /* ssz add distinct */  principalid from #tmplogins))) and UPPER(a.type) IN ('U', 'G')
		
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
						objecttypename, 
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
						dbo.getclasstype(101),
						NULL, 
						NULL,
						NULL,
						NULL,
						'N',
						'SV',
						'Y',
						@tmploginname,
						'Login',
						'CONTROL')

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
						select distinct a.principalid, a.name, a.type from serverprincipal a where snapshotid = @snapshotid and a.principalid in (select distinct /* ssz add distinct */  principalid from #tmplogins) or (a.principalid in (select distinct /* ssz add distinct */  principalid from serverrolemember where memberprincipalid in (select  distinct /* ssz add distinct */ principalid from #tmplogins))) and UPPER(a.type) IN ('U', 'G')
		
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
						objecttypename, 
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
						dbo.getclasstype(101),	
						NULL, 
						NULL,
						NULL,
						NULL,
						'N',
						'SV',
						'Y',
						@tmploginname,
						'Login',
						'VIEW DEFINITION')

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
						select distinct a.principalid, a.name, a.type from serverprincipal a where snapshotid = @snapshotid and a.principalid in (select distinct /* ssz add distinct */  principalid from #tmplogins) or (a.principalid in (select distinct /* ssz add distinct */  principalid from serverrolemember where memberprincipalid in (select distinct /* ssz add distinct */  principalid from #tmplogins))) and UPPER(a.type) IN ('U', 'G')
		
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
						objecttypename, 
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
						dbo.getclasstype(101),						
						NULL, 
						NULL,
						NULL,
						NULL,
						'N',
						'SV',
						'Y',
						@tmploginname,
						'Login',
						'ALTER')

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
			objecttypename, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct /* ssz add distinct */  snapshotid, 
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
			objecttypename, 
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
		where a.classid = 101 and UPPER(b.permissionlevel) = 'LOGIN' and b.coveringpermissionname = a.permission

		if (@classid = 101)
		begin
			exec ('isp_sqlsecure_processpermission')

			if  (UPPER(@permissiontype) = 'E')
				exec ('select distinct * from #tmppermission where classid = 101 and permissiontype = ''EF'' and objectid=' + @objectid)
			else
				exec ('select distinct * from #tmppermission where classid = 101 and objectid=' + @objectid)

			return
		end

		end

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
			objecttypename, 
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct /* ssz add distinct */  snapshotid, 
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
			objecttypename, 
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



	-- get all the users associates with server permissions (except fixed server role)
	declare myc1000 cursor for
			select distinct a.principalid, c.uid from #tmppermission a, serverprincipal b, databaseprincipal c where permissionlevel = 'SV' and UPPER(sourcename) <> 'SYSADMIN' and a.classid = 100 and b.snapshotid = @snapshotid and c.snapshotid = b.snapshotid and c.dbid = @databaseid and c.usersid = b.sid and b.principalid = a.principalid
	
	open myc1000
	fetch next from myc1000
	into @principalid, @uid
	
	while @@fetch_status = 0
	begin		

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
			objecttypename,
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission,
			coveringfrom)
		select  distinct a.snapshotid, 
			'DB',
			logintype, 
			loginname, 
			connectionname, 
			c.databasename, 
			principalid, 
			principalname, 
			principaltype,
			dbo.getdatabaseprincipalname(c.snapshotid, c.dbid, d.uid),
			'',
			a.principalid,
			a.principalname,
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
			'Database',
			null, 
			null,
			dbo.getdatabaseprincipalid(c.snapshotid, c.dbid, c.owner),
			c.owner, 
			'N',
			'Y',
			sourcename,
			sourcetype,
			sourcepermission,
			'SV'
		from 	#tmppermission a, 
			coveringpermissionhierarchy b, 
			sqldatabase c,
			#tmpuid d
		where 	a.classid = 100 and 
			UPPER(b.permissionlevel) = 'DATABASE' and 
			UPPER(b.parentpermissionlevel) = 'SERVER' and 
			b.parentcoveringpermission = a.permission and
			c.snapshotid = @snapshotid and
			c.dbid = @databaseid and
			a.principalid = @principalid and
			d.dbid = c.dbid and
			d.uid = @uid and 
			UPPER(a.sourcename) <> 'SYSADMIN'

		fetch next from myc1000
		into @principalid, @uid

	end

	close myc1000
	deallocate myc1000

	-- process sysadmin
/*	declare myc1001 cursor for
		select distinct a.principalid from #tmppermission a where a.permissionlevel = 'SV' and UPPER(a.sourcename) = 'SYSADMIN' and a.classid = 100
	
	open myc1001
	fetch next from myc1001
		into @principalid
	
	while @@fetch_status = 0
	begin		

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
			objecttypename,
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission,
			coveringfrom)
		select  distinct a.snapshotid, 
			'DB',
			logintype, 
			loginname, 
			connectionname, 
			c.databasename, 
			principalid, 
			principalname, 
			principaltype,
			'dbo',
			'S',
			5,
			'sysadmin',
			1,
			'dbo',
			0,
			'EF', 
			'CONTROL', 
			isgrant, 
			isgrantwith, 
			isrevoke, 
			isdeny, 
			c.dbid,
			c.databasename, 
			'DB', 
			'Database',
			null, 
			null,
			dbo.getdatabaseprincipalid(c.snapshotid, c.dbid, c.owner),
			c.owner, 
			'N',
			'Y',
			sourcename,
			sourcetype,
			sourcepermission,
			'SV'
		from 	#tmppermission a,
			sqldatabase c
		where 	a.classid = 100 and 
			c.snapshotid = @snapshotid and
			c.dbid = @databaseid and
			a.principalid = @principalid and
			UPPER(a.sourcename) = 'SYSADMIN'			

		fetch next from myc1001
		into @principalid
	end

	close myc1001
	deallocate myc1001
*/
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
			objecttypename,
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
		select  distinct /* ssz add distinct */  a.snapshotid, 
			'DB',
			@logintype, 
			@loginname, 
			@connectionname, 
			d.databasename, 
			f.principalid, 
			f.name, 
			f.type, 
			a.name, 
			a.type, 
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
			dbo.getobjecttypename('DB'),
			NULL, 
			NULL,
			dbo.getdatabaseprincipalid(d.snapshotid, d.dbid, d.owner),
			d.owner, 
			'N',
			'DBFXROLE',
			'N',
			c.rolename,
			'Fixed Database Role',
			rolepermission
			from 
			vwfixeddatabaserolemember a, 
			#tmpuid b,
			fixedrolepermission c,
			sqldatabase d,
			databaseprincipal e left outer join serverprincipal f on e.snapshotid = f.snapshotid and e.usersid = f.sid
			where 
			a.snapshotid = @snapshotid and 
			a.dbid = d.dbid and 
			d.dbid = @databaseid and 
			d.snapshotid = a.snapshotid and 
			a.rolememberuid = b.uid and
			e.snapshotid = a.snapshotid and
			e.uid = b.uid and 
			e.dbid = b.dbid and
			UPPER(a.name) = UPPER(c.rolename) and
			c.roletype = 'D'


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
			objecttypename,
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct /* ssz add distinct */  snapshotid, 
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
			objecttypename, 
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
	
		-- TODO: FOR EACH DATABASE, COPY THE DATA TO ANOTHER TABLE EXCEPT THE PERMISSION AND GRANTING/DENY ATTRIBUTES
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
			objecttypename,
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
			objecttypename,
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
		
		-- TODO: FOR EACH DATABASE, CONVERT EACH ROW TO COVERING PERMISSION BY COPYING OR EXPANDING EACH ROW TO THE NEW COVERING PERMISISON
		declare myc102 cursor for
			select distinct  cc.databasename, cc.principalid , aa.permission, aa.isgrant, aa.isgrantwith, aa.isdeny, aa.isrevoke
			from #tmppermission aa, #tmppermission2 cc
			where   UPPER(aa.permissionlevel) = 'SV' 
				and UPPER(aa.permissiontype) = 'EF' 
				and UPPER(aa.objecttype) = 'SERVER' 
				and cc.principalname IS NOT NULL 
				and cc.principalid IS NOT NULL
				and aa.principalid = cc.principalid

		open myc102
		fetch next from myc102
		-- ssz into @#tmppermission, @tmpgrant, @tmpgrantwith, @tmpdeny, @tmprevoke
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
				objecttypename,
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
				b.permissionname, 
				@tmpgrant, 
				@tmpgrantwith, 
				@tmprevoke, 
				@tmpdeny, 
				objectid,
				objectname, 
				objecttype, 
				objecttypename,
				schemaid, 
				schemaname,
				owner,
				ownername,
				isaliased,
				'SV',
				inherited,
				sourcename,
				sourcetype,
				sourcepermission
			from 	#tmppermission2 a
				, getcoveringpermission('SERVER', @#tmppermission, 'DATABASE' ) b
			where UPPER(a.databasename) = UPPER(@tmpdatabasename) 
				and a.principalid = @tmpprincipalid

		fetch next from myc102
		into @tmpdatabasename, @tmpprincipalid, @#tmppermission, @tmpgrant, @tmpgrantwith, @tmpdeny, @tmprevoke
		end

		close myc102
		deallocate myc102

		if (@classid = 0)
		begin
			exec ('isp_sqlsecure_processpermission')

			if  (UPPER(@permissiontype) = 'E')
				exec ('select distinct * from #tmppermission where classid = 0 and permissiontype = ''EF'' and objectid=' + @objectid)
			else
				exec ('select distinct * from #tmppermission where classid = 0 and objectid=' + @objectid)

			return
		end

		-- TODO: NEED TO ADD SCHEMA COVERING PERMISSION FROM DATABASE PERMISSION
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
			objecttypename,
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
		select  distinct /* ssz add distinct */  a.snapshotid, 
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
			dbo.getobjecttypename('iSCM'),
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
		from #tmppermission a, coveringpermissionhierarchy b, databaseschema c
		where 
		c.snapshotid = a.snapshotid and 
		c.dbid = @databaseid and 
		UPPER(b.permissionlevel) = 'SCHEMA' and 
		UPPER(b.parentpermissionlevel) = 'DATABASE' and 
		UPPER(b.parentcoveringpermission) = UPPER(a.permission) and 
		UPPER(a.permissionlevel) = 'DB' and 
		UPPER(a.objecttype) = 'DB' and 
		UPPER(a.permissiontype) = 'EF' and 
		UPPER(a.permission) IN ('SELECT', 'INSERT', 'UPDATE', 'DELETE', 'REFERENCES', 'EXECUTE', 'VIEW DEFINITION', 'ALTER ANY SCHEMA', 'ALTER', 'CONTROL')	

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
			objecttypename,
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct /* ssz add distinct */  snapshotid, 
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
			objecttypename,
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
			objecttypename,
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct /* ssz add distinct */  snapshotid, 
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
			objecttypename,
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

		--declare @tmpschemaid int
		declare @tmpgrantee int
		declare @tmpschemaid int

		if (@classid = 3)
		begin
			exec ('isp_sqlsecure_processpermission')

			if (UPPER(@permissiontype) = 'E')
				exec ('select distinct * from #tmppermission where classid = 3 and objectid=' + @objectid + ' and permissiontype = ''EF'' and databasename=''' + @databasename + '''')
			else
				exec ('select distinct * from #tmppermission where classid = 3 and objectid=' + @objectid + ' and databasename=''' + @databasename + '''')

			return
		end
		
		-- TODO: NEED TO ADD OBJECT COVERING PERMISSION FROM SCHEMA PERMISSION
		-- BY EXPANDING ON EXISTING SCHEMA PERMISSION TO OBJECT LEVEL
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
		and a.sourcetype not in ('Database', 'Database Role') and
		(c.parentobjectid = @objectid or c.objectid = @objectid)

		-- TODO: COPY ALL OBJECT EXPLICIT PERMISSION AS EFFECTIVE
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
			objecttypename,
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct /* ssz add distinct */  snapshotid, 
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
			objecttypename,
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
		where UPPER(permissionlevel) IN ('OBJ', 'COL')

		-- TODO: OBJECT LEVEL - NEED TO BREAK DOWN HIGH LEVEL PERMISSION LIKE CONTROL TO MORE DETAIL PERMISSION PRIOR TO ADDING COVERING PERMISSION TO LOWER LEVEL
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
			objecttypename,
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct /* ssz add distinct */  snapshotid, 
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
			objecttypename,
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
		where a.classid = 1 and UPPER(b.permissionlevel) = 'OBJECT' and b.coveringpermissionname = a.permission

		if (@classid = 18)
		begin

		-- TODO: REMOTE SERVICE BINDING LEVEL - NEED TO BREAK DOWN HIGH LEVEL PERMISSION LIKE CONTROL TO MORE DETAIL PERMISSION PRIOR TO ADDING COVERING PERMISSION TO LOWER LEVEL
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
			objecttypename,
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct /* ssz add distinct */  snapshotid, 
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
			objecttypename,
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
		where a.classid = 18 and UPPER(b.permissionlevel) = 'REMOTE SERVICE BINDING' and b.coveringpermissionname = a.permission

		end

		if (@classid = 19)
		begin

		-- TODO: ROUTE LEVEL - NEED TO BREAK DOWN HIGH LEVEL PERMISSION LIKE CONTROL TO MORE DETAIL PERMISSION PRIOR TO ADDING COVERING PERMISSION TO LOWER LEVEL
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
			objecttypename,
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct /* ssz add distinct */  snapshotid, 
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
			objecttypename,
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
		where a.classid = 19 and UPPER(b.permissionlevel) = 'ROUTE' and b.coveringpermissionname = a.permission

		end

		if (@classid = 23)
		begin

		-- TODO: FULLTEXT CATELOG LEVEL - NEED TO BREAK DOWN HIGH LEVEL PERMISSION LIKE CONTROL TO MORE DETAIL PERMISSION PRIOR TO ADDING COVERING PERMISSION TO LOWER LEVEL
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
			objecttypename,
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct /* ssz add distinct */  snapshotid, 
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
			objecttypename,
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
		where a.classid = 23 and UPPER(b.permissionlevel) = 'FULLTEXT CATALOG' and b.coveringpermissionname = a.permission

		end

		if (@classid = 24)
		begin

		-- TODO: SYMMETRIC KEY LEVEL - NEED TO BREAK DOWN HIGH LEVEL PERMISSION LIKE CONTROL TO MORE DETAIL PERMISSION PRIOR TO ADDING COVERING PERMISSION TO LOWER LEVEL
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
			objecttypename,
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct /* ssz add distinct */  snapshotid, 
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
			objecttypename,
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
		where a.classid = 24 and UPPER(b.permissionlevel) = 'SYMMETRIC KEY' and b.coveringpermissionname = a.permission

		end

		if (@classid = 25)
		begin

		-- TODO: CERTIFICATE LEVEL - NEED TO BREAK DOWN HIGH LEVEL PERMISSION LIKE CONTROL TO MORE DETAIL PERMISSION PRIOR TO ADDING COVERING PERMISSION TO LOWER LEVEL
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
			objecttypename,
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct /* ssz add distinct */  snapshotid, 
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
			objecttypename,
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
		where a.classid = 25 and UPPER(b.permissionlevel) = 'CERTIFICATE' and b.coveringpermissionname = a.permission

		end

		if (@classid = 26)
		begin

		-- TODO: ASYMMETRIC KEY - NEED TO BREAK DOWN HIGH LEVEL PERMISSION LIKE CONTROL TO MORE DETAIL PERMISSION PRIOR TO ADDING COVERING PERMISSION TO LOWER LEVEL
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
			objecttypename,
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct /* ssz add distinct */  snapshotid, 
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
			objecttypename,
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
		where a.classid = 26 and UPPER(b.permissionlevel) = 'ASYMMETRIC KEY' and b.coveringpermissionname = a.permission

		end

		if (@classid = 6)
		begin

		-- TODO: TYPE - NEED TO BREAK DOWN HIGH LEVEL PERMISSION LIKE CONTROL TO MORE DETAIL PERMISSION PRIOR TO ADDING COVERING PERMISSION TO LOWER LEVEL
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
			objecttypename,
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  snapshotid, 
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
			objecttypename,
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
		where a.classid = 6 and UPPER(b.permissionlevel) = 'TYPE' and b.coveringpermissionname = a.permission
		
		end

		if (@classid = 10)
		begin

		-- TODO: XML SCHEMA COLLECTION - NEED TO BREAK DOWN HIGH LEVEL PERMISSION LIKE CONTROL TO MORE DETAIL PERMISSION PRIOR TO ADDING COVERING PERMISSION TO LOWER LEVEL
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
			objecttypename,
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct /* ssz add distinct */  snapshotid, 
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
			objecttypename,
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
		where a.classid = 10 and UPPER(b.permissionlevel) = 'XML SCHEMA COLLECTION' and b.coveringpermissionname = a.permission

		end

		if (@classid = 5)
		begin

		-- TODO: ASSEMBLY - NEED TO BREAK DOWN HIGH LEVEL PERMISSION LIKE CONTROL TO MORE DETAIL PERMISSION PRIOR TO ADDING COVERING PERMISSION TO LOWER LEVEL
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
			objecttypename,
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct /* ssz add distinct */  snapshotid, 
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
			objecttypename,
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
		where a.classid = 5 and UPPER(b.permissionlevel) = 'ASSEMBLY' and b.coveringpermissionname = a.permission

		end

		if (@classid = 4)
		begin

		-- TODO: USER - NEED TO BREAK DOWN HIGH LEVEL PERMISSION LIKE CONTROL TO MORE DETAIL PERMISSION PRIOR TO ADDING COVERING PERMISSION TO LOWER LEVEL
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
			objecttypename,
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct /* ssz add distinct */  snapshotid, 
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
			objecttypename,
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
		where a.classid = 4 and UPPER(b.permissionlevel) = 'USER' and b.coveringpermissionname = a.permission

			if (@permissiontype = 'E')
				exec ('select distinct * from #tmppermission where classid = 4 and objectid=' + @objectid + ' and permissiontype = ''EF'' and databasename=''' + @databasename + '''')
			else
				exec ('select distinct * from #tmppermission where classid = 4 and objectid=' + @objectid + ' and databasename=''' + @databasename + '''')

			return
		end

		if (@classid = 16)
		begin
		
		-- TODO: CONTRACT - NEED TO BREAK DOWN HIGH LEVEL PERMISSION LIKE CONTROL TO MORE DETAIL PERMISSION PRIOR TO ADDING COVERING PERMISSION TO LOWER LEVEL
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
			objecttypename,
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct /* ssz add distinct */  snapshotid, 
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
			objecttypename,
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
		where a.classid = 16 and UPPER(b.permissionlevel) = 'CONTRACT' and b.coveringpermissionname = a.permission

		end

		if (@classid = 17)
		begin

		-- TODO: SERVICE - NEED TO BREAK DOWN HIGH LEVEL PERMISSION LIKE CONTROL TO MORE DETAIL PERMISSION PRIOR TO ADDING COVERING PERMISSION TO LOWER LEVEL
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
			objecttypename,
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct /* ssz add distinct */  snapshotid, 
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
			objecttypename,
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

		end

		delete from #tmppermission2

		-- TODO: FOR ALL OBJECTS WHOSE SCHEMA HAS COVERING PERMISSIONS, COPY THEM TO PERMISSION2 AS STAGING AREA
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
			objecttypename,
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
			objecttypename,
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
			(a.uid = c.uid or a.altuid = c.uid) and 
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

		-- TODO: APPLY SCHEMA COVERING PERMISSION TO THE OBJECTS
		-- FOR EACH SCHEMA
		declare myc302 cursor for
		select distinct  aa.grantor, aa.grantorname, aa.grantee, aa.granteename, cc.databasename, cc.schemaid , aa.permission, aa.isgrant, aa.isgrantwith, aa.isdeny, aa.isrevoke  
		from #tmppermission aa, #tmppermission2 cc
		where UPPER(aa.permissionlevel) = 'SCH' 
			and UPPER(aa.permissiontype) = 'EF' 
			and aa.schemaid = cc.schemaid 
			and aa.databasename = @databasename
			and aa.sourcetype in ('Database', 'Database Role')

		open myc302
		fetch next from myc302
		into @tmpgrantor, @tmpgrantorname, @tmpgrantee, @tmpgranteename, @tmpdatabasename, @tmpschemaid, @#tmppermission, @tmpgrant, @tmpgrantwith, @tmpdeny, @tmprevoke
	
		while @@fetch_status = 0
		begin
				-- TODO: THERE MIGHT BE REDUNDENCY WHEN COVERING PERMISSION PRODUCES SAME LOWER LEVEL PERMISSION		
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
					objecttypename,
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
					objecttypename,
					schemaid, 
					schemaname,
					owner,
					ownername,
					isaliased,
					'SCH',
					'Y',
					@tmpgrantorname,
					'Database Role',
					b.permissionname
				from 	#tmppermission2 a, getcoveringpermission('SCHEMA', @#tmppermission, 'OBJECT' ) b
				where 	UPPER(a.permissionlevel) = 'OBJ' 
					and a.databasename = @tmpdatabasename 
					and a.schemaid = @tmpschemaid

		fetch next from myc302
		into @tmpgrantor, @tmpgrantorname, @tmpgrantee, @tmpgranteename, @tmpdatabasename, @tmpschemaid, @#tmppermission, @tmpgrant, @tmpgrantwith, @tmpdeny, @tmprevoke

		end

		close myc302
		deallocate myc302

		-- RETURN OBJECT LEVEL
		if (@classid IN (5, 6, 10, 15, 16, 17, 18, 19, 23, 24, 25, 6))
		begin
			exec ('isp_sqlsecure_processpermission')

			if (UPPER(@permissiontype) = 'E')
				exec ('select distinct * from #tmppermission where classid = ' + @classid + ' and permissiontype=''EF'' and objectid=' + @objectid + ' and databasename=''' + @databasename + '''')
			else
				exec ('select distinct * from #tmppermission where classid = ' + @classid + ' and objectid=' + @objectid + ' and databasename=''' + @databasename + '''')

			return
		end		

		-- TODO: CHECK COLUMN PERMISSIONS
		-- TODO: COPY ALL OBJECT EXPLICIT PERMISSION AS EFFECTIVE
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
			objecttype, 
			objecttypename,
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct /* ssz add distinct */  snapshotid, 
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
			objecttype, 
			objecttypename,
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

		-- need to return columns back with table permission
		if (@classid = 1 and UPPER(@iscolumn) = 'N')
		begin
			exec ('isp_sqlsecure_processpermission')

			if (UPPER(@permissiontype) = 'E')
				exec ('select distinct * from #tmppermission where classid = 1 and permissiontype=''EF'' and (parentobjectid=' + @objectid + ' or objectid=' + @objectid + ') and databasename=''' + @databasename + '''')
			else
				exec ('select distinct * from #tmppermission where classid = 1 and (objectid=' + @objectid + ' or parentobjectid=' + @objectid + ') and databasename=''' + @databasename + '''')

			return
		end

		if (@classid = 1 and UPPER(@iscolumn) = 'Y')
		begin
			--exec ('isp_sqlsecure_processpermission')

			if (UPPER(@permissiontype) = 'E')
				exec ('select distinct * from #tmppermission where classid = 1 and permissiontype=''EF'' and parentobjectid=' + @parentobjectid + ' and objectid=' + @objectid + ' and databasename=''' + @databasename + '''')
			else
				exec ('select distinct * from #tmppermission where classid = 1 and objectid=' + @objectid + ' and parentobjectid=' + @parentobjectid + ' and databasename=''' + @databasename + '''')

			return
		end

		delete from #tmppermission2
		drop table #tmpcovering
	
	end

	END
	ELSE -- SQL 2000 
	BEGIN

		-- TODO: GIVEN THE UID (USERS AND ROLES) NEED TO GET DATABASE PRINCIPAL PERMISSION
	
		-- TODO: TO GET TO OBJECTS, USE EITHER SCHEMA ID OR OWNER
		-- IF OWNER IS NULL THEN USE SCHEMAID
		-- IF OWNER AND SCHEMA ID ARE NULL THEN IT IS A COLUMN, NEED TO FIND ITS PARENT AND GET OWNER

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
		objecttypename,
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
		principalid=e.name,
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
		dbo.getobjecttypename(d.objecttype),
		d.schemaid, 
		dbo.getschemaname(d.snapshotid, d.dbid, d.schemaid),
		d.owner,
		dbo.getdatabaseprincipalname(d.snapshotid, d.dbid, d.owner),
		a.isalias,
		'N',
		d.objectname,
		dbo.getobjecttypename(d.objecttype),
		d.permission
		from 
		databaseprincipal a left outer join serverprincipal e on a.snapshotid = e.snapshotid and a.usersid = e.sid, 
		#tmpuid c, 
		vwdatabaseobjectpermission d
		where 
		a.snapshotid = @snapshotid and 
		a.dbid = @databaseid and 
		(a.uid = c.uid or a.altuid = c.uid) and 
		a.dbid = c.dbid and
		d.snapshotid = a.snapshotid and 
		d.grantee = c.uid and 
		d.dbid = c.dbid and  
		d.owner is not null and
		(d.schemaid = 0 or d.schemaid is null) and
		(d.parentobjectid = @objectid or d.objectid = @objectid)

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
		objecttype, 
		objecttypename,
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
		principalid=e.name,
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
		dbo.gettablename(@snapshotid, @databaseid, d.parentobjectid) + '.' + d.objectname,
		d.objecttype, 
		dbo.getobjecttypename(d.objecttype),
		d.schemaid, 
		dbo.getschemaname(d.snapshotid, d.dbid, d.schemaid),
		d.owner,
		dbo.getdatabaseprincipalname(d.snapshotid, d.dbid, d.owner),
		a.isalias,
		'N',
		d.objectname,
		'Column',
		d.permission
		from 
		databaseprincipal a left outer join serverprincipal e on a.snapshotid = e.snapshotid and a.usersid = e.sid, 
		#tmpuid c, 
		vwdatabasecolumnpermission d
		where 
		a.snapshotid = @snapshotid and 
		a.dbid = @databaseid and 
		(a.uid = c.uid or a.altuid = c.uid) and 
		a.dbid = c.dbid and
		d.snapshotid = a.snapshotid and 
		d.grantee = c.uid and 
		d.dbid = c.dbid and  
		(d.schemaid = 0 or d.schemaid is null)

		if (@geteffective = 'Y')	
		begin

		-- PROCESS FIXED SERVER ROLE PERMISSION
		-- CHECK IF LOGINS BELONGS TO FIXED SERVERR ROLES, IF SO THEN ASSIGN FIXED PERMISSION TO SERVER LEVEL
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
			objecttypename,
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
		select  distinct /* ssz add distinct */  a.snapshotid, 
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
			'Y', -- force it 
			c.isgrantwith, 
			c.isrevoke, 
			c.isdeny, 
			0,
			@connectionname, 
			dbo.getclassobjecttype(100), 
			dbo.getclasstype(100), 
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

			-- get all the users associates with server permissions (except fixed server role)
			declare myc1000 cursor for
					select distinct a.principalid, c.uid from #tmppermission a, serverprincipal b, databaseprincipal c where permissionlevel = 'SV' and UPPER(sourcename) <> 'SYSADMIN' and a.classid = 100 and b.snapshotid = @snapshotid and c.snapshotid = b.snapshotid and c.dbid = @databaseid and c.usersid = b.sid and b.principalid = a.principalid
			
			open myc1000
			fetch next from myc1000
			into @principalid, @uid
			
			while @@fetch_status = 0
			begin		
		
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
					objecttypename,
					schemaid, 
					schemaname,
					owner,
					ownername,
					isaliased,
					inherited,
					sourcename,
					sourcetype,
					sourcepermission,
					coveringfrom)
				select  distinct a.snapshotid, 
					'DB',
					logintype, 
					loginname, 
					connectionname, 
					c.databasename, 
					principalid, 
					principalname, 
					principaltype,
					dbo.getdatabaseprincipalname(c.snapshotid, c.dbid, d.uid),
					'',
					a.principalid,
					a.principalname,
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
					'Database',
					null, 
					null,
					dbo.getdatabaseprincipalid(c.snapshotid, c.dbid, c.owner),
					c.owner, 
					'N',
					'Y',
					sourcename,
					sourcetype,
					sourcepermission,
					'SV'
				from 	#tmppermission a, 
					coveringpermissionhierarchy b, 
					sqldatabase c,
					#tmpuid d
				where 	a.classid = 100 and 
					UPPER(b.permissionlevel) = 'DB2000' and 
					UPPER(b.parentpermissionlevel) = 'SERVER' and 
					b.parentcoveringpermission = a.permission and
					c.snapshotid = @snapshotid and
					c.dbid = @databaseid and
					a.principalid = @principalid and
					d.dbid = c.dbid and
					d.uid = @uid and 
					UPPER(a.sourcename) <> 'SYSADMIN'
		
				fetch next from myc1000
				into @principalid, @uid
		
			end
		
			close myc1000
			deallocate myc1000
		
			-- process sysadmin
/*			declare myc1001 cursor for
				select distinct a.principalid from #tmppermission a where a.permissionlevel = 'SV' and UPPER(a.sourcename) = 'SYSADMIN' and a.classid = 100
			
			open myc1001
			fetch next from myc1001
				into @principalid
			
			while @@fetch_status = 0
			begin		
		
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
					objecttypename,
					schemaid, 
					schemaname,
					owner,
					ownername,
					isaliased,
					inherited,
					sourcename,
					sourcetype,
					sourcepermission,
					coveringfrom)
				select  distinct a.snapshotid, 
					'DB',
					logintype, 
					loginname, 
					connectionname, 
					c.databasename, 
					principalid, 
					principalname, 
					principaltype,
					'dbo',
					'S',
					5,
					'sysadmin',
					1,
					'dbo',
					0,
					'EF', 
					'CONTROL', 
					isgrant, 
					isgrantwith, 
					isrevoke, 
					isdeny, 
					c.dbid,
					c.databasename, 
					'DB', 
					'Database',
					null, 
					null,
					dbo.getdatabaseprincipalid(c.snapshotid, c.dbid, c.owner),
					c.owner, 
					'N',
					'Y',
					sourcename,
					sourcetype,
					sourcepermission,
					'SV'
				from 	#tmppermission a,
					sqldatabase c
				where 	a.classid = 100 and 
					c.snapshotid = @snapshotid and
					c.dbid = @databaseid and
					a.principalid = @principalid and
					UPPER(a.sourcename) = 'SYSADMIN'			
		
				fetch next from myc1001
				into @principalid
			end
		
			close myc1001
			deallocate myc1001
*/

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
			'Database Role',
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
			objecttypename,
			schemaid, 
			schemaname,
			owner,
			ownername,
			isaliased,
			inherited,
			sourcename,
			sourcetype,
			sourcepermission)
		select  distinct /* ssz add distinct */  snapshotid, 
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
			objecttypename, 
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
		where a.classid = 0 and UPPER(b.permissionlevel) = 'DB2000' and b.coveringpermissionname = a.permission

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
			where a.classid = 0 and UPPER(b.permissionlevel) = 'OBJECT' and b.coveringpermissionname = a.permission

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
				objecttype, 
				objecttypename,
				schemaid, 
				schemaname,
				owner,
				ownername,
				isaliased,
				inherited,
				sourcename,
				sourcetype,
				sourcepermission)
			select  distinct /* ssz add distinct */  snapshotid, 
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
				objecttype, 
				objecttypename,
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
				objecttypename, 
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
				dbo.getobjecttypename(a.type), 
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
				a.dbid = @databaseid and
				a.objectid = @objectid and
				a.snapshotid = b.snapshotid and
				b.objecttype ='DB' and
				a.type <> 'iCO'and
				b.permission in ('SELECT', 'INSERT', 'DELETE', 'UPDATE', 'REFERENCES', 'EXECUTE') and
				(a.parentobjectid = @objectid or a.objectid = @objectid)

			--select * from #tmppermission where permissionlevel = 'OBJ'

			-- remove all databse fixed role and other objects permissions
			delete from #tmppermission where permissionlevel = 'DB' and permission not in ('CREATE TABLE', 'CREATE PROCEDURE', 'CREATE VIEW', 'CREATE DEFAULT', 'CREATE RULE', 'CREATE FUNCTION', 'BACKUP DATABASE', 'BACKUP LOG')
			delete from #tmppermission where objecttype = 'P' and UPPER(permission) NOT IN ('ALTER', 'CONTROL', 'EXECUTE', 'TAKE OWNERSHIP', 'VIEW DEFINITION')
			delete from #tmppermission where objecttype = 'FN' and UPPER(permission) NOT IN ('REFERENCES', 'EXECUTE')

			if (@classid = 0)
			begin
				exec ('isp_sqlsecure_processpermission')
	
				if  (UPPER(@permissiontype) = 'E')
					exec ('select distinct * from #tmppermission where classid = 0 and permissiontype = ''EF'' and objectid=' + @objectid)
				else
					exec ('select distinct * from #tmppermission where classid = 0 and objectid=' + @objectid)
	
				return
			end

		end


		-- SQL 2000, PROCESS OBJECT AND COLUMN PERMISSIONS
		if (@classid IN (5, 6, 10, 15, 16, 17, 18, 19, 23, 24, 25, 6) and UPPER(@iscolumn) = 'N')
		begin
			exec ('isp_sqlsecure_processpermission')

			if (UPPER(@permissiontype) = 'E')
				exec ('select distinct * from #tmppermission where classid = ' + @classid + ' and permissiontype=''EF'' and objectid=' + @objectid + ' and databasename=''' + @databasename + '''')
			else
				exec ('select distinct * from #tmppermission where classid = ' + @classid + ' and objectid=' + @objectid + ' and databasename=''' + @databasename + '''')

			return
		end		

		-- table needs to return columns
		if (@classid = 1 and UPPER(@iscolumn) = 'N')
		begin
			exec ('isp_sqlsecure_processpermission')

			if (UPPER(@permissiontype) = 'E')
				exec ('select distinct * from #tmppermission where classid = 1 and permissiontype=''EF'' and (parentobjectid=' + @objectid + ' or objectid=' + @objectid + ') and databasename=''' + @databasename + '''')
			else
				exec ('select distinct * from #tmppermission where classid = 1 and (objectid=' + @objectid + ' or parentobjectid=' + @objectid + ') and databasename=''' + @databasename + '''')

			return
		end


		if (@classid = 1 and UPPER(@iscolumn) = 'Y')
		begin
			--exec ('isp_sqlsecure_processpermission')
			delete from #tmppermission where objecttype = 'iCOL' and UPPER(permission) NOT IN ('SELECT', 'UPDATE')

			if (UPPER(@permissiontype) = 'E')
				exec ('select distinct * from #tmppermission where classid = 1 and permissiontype=''EF'' and parentobjectid=' + @parentobjectid + ' and objectid=' + @objectid + ' and databasename=''' + @databasename + '''')
			else
				exec ('select distinct * from #tmppermission where classid = 1 and objectid=' + @objectid + ' and parentobjectid=' + @parentobjectid + ' and databasename=''' + @databasename + '''')

			return
		end

	END

	--drop table #tmplogins
	--drop table #tmpuid
	--drop table #tmppermission
	--drop table #tmppermission2
	--drop table #tmpdenypermission






GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


GRANT EXECUTE ON [dbo].[isp_sqlsecure_getobjectpermission] TO [SQLSecureView]

GO


