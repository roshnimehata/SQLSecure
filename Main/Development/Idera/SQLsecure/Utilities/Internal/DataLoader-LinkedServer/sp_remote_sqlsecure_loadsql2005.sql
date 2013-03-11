SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


-- Idera SQLsecure Version 0.9
   --
   -- (c) Copyright 2004-2006 Idera, a division of BBS Technologies, Inc., all rights reserved.
   -- SQLsecure, Idera and the Idera Logo are trademarks or registered trademarks
   -- of BBS Technologies or its subsidiaries in the United States and other jurisdictions.
   --
   -- Description :
   --              Use the server link and retrieve sql server 2005 info and populate sqlsecure repository tables
   -- 	           Use for testing purpose

create procedure [dbo].[sp_remote_sqlsecure_load2005] (
@servername nvarchar(256),
@status nchar(1),
@snapshotcomment nvarchar(1000),
@connectionname nvarchar(2000)
)
as

	SET ANSI_NULLS ON
	SET QUOTED_IDENTIFIER OFF
	SET ANSI_WARNINGS ON

	-- sp parameters
	declare @snapshotid int

	SET DATEFORMAT mdy

	--SET @status='P'
	--SET @snapshotcomment='NA'
	--SET @connectionname='localhost/sqlsecure'

	declare @snapshottm datetime
	SET @snapshottm = GETUTCDATE()

	declare @tm varchar(26)
	set @tm = CONVERT(varchar(26), @snapshottm, 109)

	--CREATE TABLE #srvos ([Index] integer, Name sysname, Internal_Value integer, Character_Value sysname)

	--exec('INSERT #srvos exec [' + @servername + '].master.dbo.xp_msver ''WindowsVersion'' ')

	-- serversnapshot table
	exec('INSERT INTO dbo.serversnapshot (connectionname, baseline, snapshottimestamp, status, snapshotcomment, serverid, servername, instancename, os, authenticationmode, version, edition) (SELECT distinct connectionname='''+ @connectionname + ''', baseline=''N'', snapshottimestamp=''' + @snapshottm + ''', status=''' + @status + ''', snapshotcomment=''' + @snapshotcomment + ''', serverid=srvid, servername=srvname, b.instancename, os=Character_Value, b.authenticationmode , b.version, b.edition from [' + @servername + '].master.dbo.sysservers, [' + @servername + '].master.dbo.vw_systeminfo b, [' + @servername + '].master.dbo.srvos)')

	--DROP TABLE #srvos 

	print @tm

	-- get the snapshot id
	-- TODO: Need to use time instead of connectionname
	declare myc0 cursor for
	 select snapshotid from serversnapshot where connectionname >= @connectionname order by 1 desc

	open myc0
	fetch next from myc0
	into @snapshotid

	print @snapshotid

	close myc0
	deallocate myc0

	-- endpoint table
	EXEC('INSERT INTO endpoint (snapshotid, endpointid, principalid, name, type, protocol, isadminendpoint) (SELECT ' + @snapshotid + ', endpoint_id, principal_id, name, type, protocol, isadminpoint=CASE WHEN is_admin_endpoint = 1 THEN ''Y'' ELSE ''N'' END FROM [' + @servername + '].master.sys.endpoints)')

	-- fixedserverrole table
	EXEC('INSERT INTO fixedserverrole (snapshotid, rolename, description) (SELECT distinct ' + @snapshotid + ', srv_role_name, srv_description FROM [' + @servername +'].master.dbo.srvroles)')

	-- serverprincipal table, only works for sql 2000
	EXEC('INSERT INTO serverprincipal (snapshotid, principalid, sid, name, type, disabled, ispolicychecked, isexpirationchecked, serveraccess, serverdeny) (select distinct ' + @snapshotid + ', principalid=a.principal_id, a.sid, a.name, a.type,isdisabled=CASE WHEN a.is_disabled = 0 THEN ''Y'' ELSE ''N'' END, ispolicychecked=CASE WHEN b.is_policy_checked = 1 THEN ''Y'' ELSE ''N'' END, isexpirationchecked=CASE WHEN b.is_expiration_checked = 1 THEN ''Y'' ELSE ''N'' END, serveraccess=CASE WHEN (c.state is null) THEN ''N'' ELSE ''Y'' END, serverdeny=CASE WHEN (c.state is null) THEN ''Y'' ELSE ''N'' END from [' + @servername + '].master.sys.server_principals a left outer join [' + @servername + '].master.sys.sql_logins b on a.principal_id = b.principal_id left outer join [' + @servername + '].master.sys.server_permissions c on c.grantee_principal_id = a.principal_id and c.type = N''COSQ'')')

	--print 'serverrolemember'

	-- serverrolemember table
	EXEC('INSERT INTO serverrolemember (snapshotid, rolename, rolemembersid) (SELECT distinct ' + @snapshotid + ', rolename=srv_role_name, rolemembersid=role_member_sid FROM [' + @servername + '].master.dbo.srvrolemembers)')

	--DROP TABLE #srvrolemembers

	--print 'serverpermission'

	-- server permission table
	EXEC('INSERT INTO serverpermission (snapshotid, class, grantee, grantor, majorid, minorid, permissiontype, permission, isgrant, isgrantwith, isdeny, isrevoke) (select distinct ' + @snapshotid + ', class, grantee=b.sid, grantor=c.sid, majorid=major_id, minorid=minor_id, permissiontype=a.type, permission=permission_name, isgrant=CASE WHEN state = ''G'' THEN ''Y'' ELSE ''N'' END, isgrantwith=CASE WHEN state = ''W'' THEN ''Y'' ELSE ''N'' END, isdeny=CASE WHEN state = ''D'' THEN ''Y'' ELSE ''N'' END, isrevoke=CASE WHEN state = ''R'' THEN ''Y'' ELSE ''N'' END from [' + @servername + '].master.sys.server_permissions a, [' + @servername + '].master.sys.server_principals b, [' + @servername + '].master.sys.server_principals c where a.grantee_principal_id = b.principal_id and a.grantor_principal_id = c.principal_id )')

	--print 'sqldatabase'

	-- sqldatabase table, only works for sql 2000
	-- TODO: check guest enabled option
	EXEC('INSERT INTO sqldatabase (snapshotid, dbid, databasename, owner) (SELECT distinct ' + @snapshotid + ', dbid=CAST(dbid AS int), databasename=a.name, owner=b.name FROM [' + @servername + '].master.dbo.sysdatabases a LEFT OUTER JOIN [' + @servername + '].master.dbo.syslogins b on a.sid = b.sid)')


	-- perform database level processing
	declare @dbid int
	declare @dbname nvarchar(128)

	declare @objectid int
	declare @objectuid int
	declare @columnbitmap varbinary(4000)
	declare @multiplier bigint
	declare @counter int
	declare @columnname nvarchar(256)
	declare @grantee int
	declare @grantor int
	declare @action int
	declare @protecttype int
	declare @permission nvarchar(128)
	declare @isgrant nchar(1)
	declare @isgrantwith nchar(1)
	declare @isrevoke nchar(1)

	exec('create view vw_sqlsecure_databases (dbid, name) as select distinct CAST(dbid AS int), name from [' + @servername + '].master.dbo.sysdatabases ')
		
	declare myc1 cursor for
	 select distinct dbid, name from vw_sqlsecure_databases

	open myc1
	fetch next from myc1
	into @dbid, @dbname
	while @@fetch_status = 0
	begin   
		print @dbid
		print @dbname

		--print 'insert into databaseprincipal'

		-- databaseprincipal table, sql 2000 only
		exec('INSERT INTO databaseprincipal (snapshotid, dbid, uid, name, usersid, type, isalias, owner, hasaccess, isdeny) (SELECT distinct ' + @snapshotid + ', dbid=' + @dbid + ', uid=CAST(uid AS int), a.name, usersid=a.sid, type=CASE WHEN a.issqlrole = 1 THEN ''R'' WHEN a.isapprole = 1 THEN ''A'' WHEN a.isntname = 0 THEN ''S'' WHEN a.isntname = 1 AND a.isntuser = 1 THEN ''U'' WHEN a.isntname = 1 AND a.isntgroup = 1 THEN ''G'' END, isaliased, owner=b.sid, hasaccess=CASE WHEN hasdbaccess = ''1'' THEN ''Y'' ELSE ''N'' END, isdeny=CASE WHEN hasdbaccess = ''1'' THEN ''N'' ELSE ''Y'' END FROM [' + @servername + '].' + @dbname + '.dbo.sysusers a LEFT OUTER JOIN [' + @servername + '].master.dbo.syslogins b on a.sid = b.sid)')

		--print 'insert into databaserolemember'

		-- databaserolemember
		exec('INSERT INTO databaserolemember(snapshotid, dbid, rolememberuid, groupuid) (select distinct ' + @snapshotid + ', dbid=' + @dbid + ', rolememberuid=memberuid, groupuid from [' + @servername + '].' + @dbname + '.dbo.sysmembers)')
		
		--print 'insert into databaseobject'

		-- databaseobject table
		exec('INSERT INTO databaseobject (snapshotid, class, parentobjectid, dbid, objectid, name, type, owner, schemaname) (select ' + @snapshotid + ',1 , parentobjectid=a.parent_object_id, ' + @dbid + ',objectid=a.object_id, a.name, a.type, owner=CASE WHEN a.principal_id IS NULL THEN b.principal_id ELSE a.principal_id END, schemaname=b.name from [' + @servername + '].' + @dbname + '.sys.all_objects a, [' + @servername + '].' + @dbname + '.sys.schemas b where a.schema_id = b.schema_id)')
		
		--print 'INSERT INTO databaseobject (snapshotid, dbid, objectid, parentobjectid, class, owner, schemaname, name, type) (select distinct ' + CONVERT(varchar, @snapshotid) + ', ' + CONVERT(varchar, @dbid) + ', objectid=b.column_id, parentobjectid=b.object_id, class=1, owner=CASE WHEN c.principal_id IS NULL THEN d.principal_id ELSE c.principal_id END, schemaname=d.name, name=b.name, type=''CO'' from [' + @servername +'].sys.database_permissions a, [' + @servername + '].sys.all_columns b, [' + @servername + '].sys.all_objects c, [' + @servername + '].sys.schemas d where a.minor_id = 1 and a.major_id = b.object_id and b.object_id = c.object_id and c.schema_id = d.schema_id)'

		--Column objects
		exec('INSERT INTO databaseobject (snapshotid, dbid, objectid, parentobjectid, class, owner, schemaname, name, type) (select distinct ' + @snapshotid + ', ' + @dbid + ', objectid=b.column_id, parentobjectid=b.object_id, class=1, owner=CASE WHEN c.principal_id IS NULL THEN d.principal_id ELSE c.principal_id END, schemaname=d.name, name=b.name, type=''CO'' from [' + @servername + '].' + @dbname + '.sys.database_permissions a, [' + @servername + '].' + @dbname + '.sys.all_columns b, [' + @servername + '].' + @dbname + '.sys.all_objects c, [' + @servername + '].' + @dbname + '.sys.schemas d where a.minor_id = 1 and a.major_id = b.object_id and b.object_id = c.object_id and c.schema_id = d.schema_id)')

		--print 'insert into databasepermission'

		--print 'INSERT INTO databasepermission (snapshotid, dbid, class, permission, uid, grantor, grantee, objectid, parentobjectid, isgrant, isgrantwith, isdeny, isrevoke) (select distinct ' + CONVERT(varchar, @snapshotid) + ', ' + CONVERT(varchar, @dbid) + ', a.class, permission=permission_name, uid=grantor_principal_id, grantor=grantor_principal_id, grantee=grantee_principal_id, objectid=major_id, parentobjectid=0 ,isgrant=CASE WHEN state = ''G'' THEN ''Y'' ELSE ''N'' END, isgrantwith=CASE WHEN state = ''W'' THEN ''Y'' ELSE ''N'' END, isdeny=CASE WHEN state = ''D'' THEN ''Y'' ELSE ''N'' END, isrevoke=CASE WHEN state = ''R'' THEN ''Y'' ELSE ''N'' END from ' + @dbname + '.sys.database_permissions a, databaseobject b where a.major_id = b.objectid and a.class = 1 and b.snapshotid = ' + CONVERT(varchar, @snapshotid) + ' and b.dbid = ' + CONVERT(varchar, @dbid) + ')'


		-- databasepermission table, parentobjectid = 0 is for non-column object
		exec('INSERT INTO databasepermission (snapshotid, dbid, class, permission, uid, grantor, grantee, objectid, parentobjectid, isgrant, isgrantwith, isdeny, isrevoke) (select distinct ' + @snapshotid + ', ' + @dbid + ', a.class, permission=permission_name, uid=grantor_principal_id, grantor=grantor_principal_id, grantee=grantee_principal_id, objectid=major_id, parentobjectid=0 ,isgrant=CASE WHEN state = ''G'' THEN ''Y'' ELSE ''N'' END, isgrantwith=CASE WHEN state = ''W'' THEN ''Y'' ELSE ''N'' END, isdeny=CASE WHEN state = ''D'' THEN ''Y'' ELSE ''N'' END, isrevoke=CASE WHEN state = ''R'' THEN ''Y'' ELSE ''N'' END from [' + @servername + '].' + @dbname + '.sys.database_permissions a, databaseobject b where a.major_id = b.objectid and a.class = 1 and b.snapshotid = ' + @snapshotid + ' and b.dbid = ' + @dbid + ')')

		--print 'next loop!!!'


		fetch next from myc1
		into @dbid, @dbname


	end

	close myc1
	deallocate myc1

	exec('drop view vw_sqlsecure_databases')

	print 'Done!'

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

