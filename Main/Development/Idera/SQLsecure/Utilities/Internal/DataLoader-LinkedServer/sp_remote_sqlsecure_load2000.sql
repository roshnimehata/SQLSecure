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
   --              Use the server link and retrieve sql server 2000 info and populate sqlsecure repository tables
   -- 	           Use for testing purpose

create procedure [dbo].[sp_remote_sqlsecure_load2000] (
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
	
	declare @snapshottm datetime
	SET @snapshottm = GETUTCDATE()
	
	declare @tm varchar(26)
	set @tm = CONVERT(varchar(26), @snapshottm, 109)	
		
	CREATE TABLE #srvos ([Index] integer, Name sysname, Internal_Value integer, Character_Value sysname)
	
	exec('INSERT #srvos exec [' + @servername + '].master.dbo.xp_msver ''WindowsVersion'' ')
	
	-- serversnapshot table
	exec('INSERT INTO dbo.serversnapshot (connectionname, baseline, snapshottimestamp, status, snapshotcomment, serverid, servername, instancename, os, authenticationmode, version, edition) (SELECT distinct connectionname='''+ @connectionname + ''', baseline=''N'', snapshottimestamp=''' + @snapshottm + ''', status=''' + @status + ''', snapshotcomment=''' + @snapshotcomment + ''', serverid=srvid, servername=srvname, b.instancename, os=Character_Value, b.authenticationmode , b.version, b.edition from [' + @servername + '].master.dbo.sysservers, [' + @servername + '].master.dbo.vw_systeminfo b, #srvos)')
	
	DROP TABLE #srvos 
	
	--print @tm
	
	-- get the snapshot id
	-- TODO: Need to use time instead of connectionname
	declare myc0 cursor for
	 select snapshotid from serversnapshot where connectionname >= @connectionname order by 1 desc
	
	open myc0
	fetch next from myc0
	into @snapshotid
	
	--print @snapshotid
	
	close myc0
	deallocate myc0
		
	CREATE TABLE #srvroles (srv_role_name sysname, srv_description sysname)
	exec('INSERT #srvroles EXEC [' + @servername + '].master.dbo.sp_helpsrvrole')
	
	-- fixedserverrole table
	EXEC('INSERT INTO fixedserverrole (snapshotid, rolename, description) (SELECT distinct ' + @snapshotid + ', srv_role_name, srv_description FROM #srvroles)')
		
	-- serverprincipal table, only works for sql 2000
	EXEC('INSERT INTO serverprincipal (snapshotid, sid, name, serveraccess, serverdeny, type) (SELECT distinct ' + @snapshotid + ', sid, name, serveraccess=CASE WHEN hasaccess = 1 THEN ''Y'' ELSE ''N'' END, serverdeny=CASE WHEN denylogin = 1 THEN ''Y'' ELSE ''N'' END, type=CASE WHEN isntname = 0 THEN ''S'' WHEN isntname = 1 AND isntuser = 1 THEN ''U'' WHEN isntname = 1 AND isntgroup = 1 THEN ''G'' END from [' + @servername + '].master.dbo.syslogins)')
	
	DROP TABLE #srvroles
		
	CREATE TABLE #srvrolemembers (srv_role_name sysname, role_member_name sysname, role_member_sid varbinary(85))
	exec('INSERT #srvrolemembers EXEC [' + @servername + '].master.dbo.sp_helpsrvrolemember')
	
	-- serverrolemember table
	EXEC('INSERT INTO serverrolemember (snapshotid, rolename, rolemembersid) (SELECT distinct ' + @snapshotid + ', rolename=srv_role_name, rolemembersid=role_member_sid FROM #srvrolemembers)')
	
	DROP TABLE #srvrolemembers
	
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
	
		print 'insert into databaseprincipal'
	
		-- databaseprincipal table, sql 2000 only
		exec('INSERT INTO databaseprincipal (snapshotid, dbid, uid, name, usersid, type, isalias, owner, hasaccess, isdeny) (SELECT distinct ' + @snapshotid + ', dbid=' + @dbid + ', uid=CAST(uid AS int), a.name, usersid=a.sid, type=CASE WHEN a.issqlrole = 1 THEN ''R'' WHEN a.isapprole = 1 THEN ''A'' WHEN a.isntname = 0 THEN ''S'' WHEN a.isntname = 1 AND a.isntuser = 1 THEN ''U'' WHEN a.isntname = 1 AND a.isntgroup = 1 THEN ''G'' END, isaliased, owner=b.sid, hasaccess=CASE WHEN hasdbaccess = ''1'' THEN ''Y'' ELSE ''N'' END, isdeny=CASE WHEN hasdbaccess = ''1'' THEN ''N'' ELSE ''Y'' END FROM [' + @servername + '].' + @dbname + '.dbo.sysusers a LEFT OUTER JOIN [' + @servername + '].master.dbo.syslogins b on a.sid = b.sid)')
	
		print 'insert into databaserolemember'
	
		-- databaserolemember
		exec('INSERT INTO databaserolemember(snapshotid, dbid, rolememberuid, groupuid) (select distinct ' + @snapshotid + ', dbid=' + @dbid + ', rolememberuid=memberuid, groupuid from [' + @servername + '].' + @dbname + '.dbo.sysmembers)')
		
		print 'insert into databaseobject'
	
		-- databaseobject table
		exec('INSERT INTO databaseobject (snapshotid, class, parentobjectid, dbid, objectid, name, type, owner) (select distinct ' + @snapshotid + ', -1, -1, dbid=' + @dbid + ', objectid=id, name, type=xtype, owner=uid from [' + @servername + '].' + @dbname + '.dbo.sysobjects)')
		
		-- insert objectid = 0 for statement level
		-- use uid=-1 as indicator for statement level ownership
		exec('INSERT INTO databaseobject (snapshotid, class, parentobjectid, dbid, objectid, name, type, owner) values (' + @snapshotid + ', -1, -1, ' + @dbid + ', 0, ''STATEMENT'', ''ST'', -1)')
	
		print 'insert into databasepermission'
	
		-- databasepermission table
		exec('INSERT INTO databasepermission (snapshotid, class, parentobjectid, dbid, uid, objectid, grantee, grantor, isgrantwith, isgrant, isrevoke, permission) (select distinct ' + @snapshotid + ', -1, -1, dbid=' + @dbid + ', uid=a.uid, objectid=a.id, b.grantee, b.grantor, isgrantwith=CASE WHEN a.protecttype = 204 THEN ''Y'' ELSE ''N'' END, isgrant=CASE WHEN a.protecttype = 205 THEN ''Y'' ELSE ''N'' END, isrevoke=CASE WHEN a.protecttype = 206 THEN ''Y'' ELSE ''N'' END, permission=CASE WHEN a.protecttype = 205 and a.action = 26 THEN ''REFERENCES'' WHEN a.protecttype = 205 and a.action = 178 THEN ''CREATE FUNCTION'' WHEN a.protecttype = 205 and a.action = 193 THEN ''SELECT'' WHEN a.protecttype = 205 and a.action = 195 THEN ''INSERT'' WHEN a.protecttype = 205 and a.action = 196 THEN ''DELETE'' WHEN a.protecttype = 205 and a.action = 197 THEN ''UPDATE'' WHEN a.action = 198 THEN ''CREATE TABLE'' ELSE ''UNKNOWN'' END from [' + @servername + '].' + @dbname + '.dbo.sysprotects a, [' + @servername + '].' + @dbname + '.dbo.syspermissions b where a.id = b.id )')

		print 'after insert into databasepermission'

	
		-- create temp view to point to this database to solve the cursor limitations
		exec('create view vw_sqlsecure_sysprotect (id, uid, columns, action, protecttype) as select id, uid, columns, action, protecttype from [' +@servername + '].' + @dbname + '.dbo.sysprotects')
		exec('create view vw_sqlsecure_syscolumns (name, id, colorder) as select name, id, colorder from [' + @servername + '].' + @dbname + '.dbo.syscolumns')
		exec('create view vw_sqlsecure_syspermissions (id, grantee, grantor) as select id, grantee, grantor from [' + @servername + '].' + @dbname + '.dbo.syspermissions')

		print 'after creating views'
	
		declare myc10 cursor for
		 select distinct id, uid, columns, action, protecttype from vw_sqlsecure_sysprotect 
		
		open myc10
		fetch next from myc10
		into @objectid, @objectuid, @columnbitmap, @action, @protecttype
		while @@fetch_status = 0
		begin   
		
			set @counter = 1
			set @multiplier = 1
		
			if @columnbitmap > 1
			begin
				print @objectid
				print 'has column permissions'
		
				--print @columnbitmap
		
				while(@counter < 30)
				begin
					--select columns, CASE WHEN action = 193 THEN 'SELECT' WHEN action = 197 THEN 'UPDATE' END 'PERMISSION TYPE', columns & POWER(2, 4) 'COLUMN PERMISSION' from sysprotects where id = 149575571
					set @multiplier = POWER(2, @counter)
		
					-- starting value should be 2 not 1
					if (@multiplier = 1)
						set @multiplier = 2
		
					--print 'multiplier'
					--print @multiplier
		
					if (@columnbitmap & @multiplier > 0)
					begin
						--print 'Found column for object id '
	
						--print @dbid
						--print @objectid 
						--print @columnbitmap 
						--print 'column order is '
						--print @counter
	
						if not exists (select * from databaseobject where snapshotid=@snapshotid and dbid=@dbid and objectid=@counter and parentobjectid=@objectid and class=-1)
						begin
							--print 'new, does not exist'
	
							-- get the column name
							declare myc100 cursor for
							 select distinct name from vw_sqlsecure_syscolumns where id=@objectid and colorder=@counter
							
							open myc100
							fetch next from myc100
							into @columnname
	
							close myc100
							deallocate myc100
	
							--print 'column name ' + @columnname
	
							-- insert into databaseobject table, parentobjectid is objectid and objectid is counter, owner is uid
							exec('INSERT INTO databaseobject (snapshotid, class, parentobjectid, dbid, objectid, name, type, owner) values (' + @snapshotid + ', -1, ' + @objectid + ', ' + @dbid + ', ' + @counter + ', ''' + @columnname + ''', ''CO'', ' + @objectuid + ')')
	
							declare myc1000 cursor for
							 select distinct grantee, grantor from vw_sqlsecure_syspermissions where id=@objectid 
							
							open myc1000
							fetch next from myc1000
							into @grantee, @grantor
	
							close myc1000
							deallocate myc1000
	
							if @protecttype = 204
								set @isgrantwith = 'Y'
							else
								set @isgrantwith = 'N'
	
							if @protecttype = 205
								set @isgrant = 'Y'
							else
								set @isgrant = 'N'
	
							if @protecttype = 206
								set @isrevoke = 'Y'
							else
								set @isrevoke = 'N'
	
							if @action = 26 
								set @permission = 'REFERENCES'
							else if @action = 178
								set @permission = 'CREATE FUNCTION'
							else if @action = 193
								set @permission = 'SELECT'
							else if @action = 195
								set @permission = 'INSERT'
							else if @action = 196
								set @permission = 'DELETE'
							else if @action = 197
								set @permission = 'UPDATE'
							else if @action = 198
								set @permission = 'CREATE TABLE'
							else if @action = 203
								set @permission = 'CREATE DATABASE'
							else if @action = 207
								set @permission = 'CREATE VIEW'
							else if @action = 222
								set @permission = 'CREATE PROCEDURE'
							else if @action = 224
								set @permission = 'EXECUTE'
							else if @action = 228
								set @permission = 'BACKUP DATABASE'
							else if @action = 233
								set @permission = 'CREATE DEFAULT'
							else if @action = 235
								set @permission = 'BACKUP LOG'
							else if @action = 236
								set @permission = 'CREATE RULE'
							else
								set @permission = 'UNKNOWN'
	
							if (@permission = 'UNKNOWN')
							BEGIN
							print '==============================================='
							print @protecttype
							print @action
	
							END
	
							-- insert into permission table
							--declare @tmpstr nvarchar(1000)
							exec ('INSERT INTO databasepermission (snapshotid, class, parentobjectid, dbid, uid, objectid, grantee, grantor, isgrantwith, isgrant, isrevoke, permission) values (' + @snapshotid + ', -1, ' + @objectid + ', ' + @dbid + ', ' + @objectuid + ', ' + @counter + ',' + @grantee + ',' + @grantor + ',''' + @isgrantwith + ''',''' + @isgrant + ''',''' + @isrevoke + ''',''' + @permission + ''')')
							--print @tmpstr
	
						end
					end
		
				    	select @counter = @counter + 1
				end 
			
		
			end
		
		
			fetch next from myc10
			into @objectid, @objectuid, @columnbitmap, @action, @protecttype
		
		
		end
		
		close myc10
		deallocate myc10
	
		exec('drop view vw_sqlsecure_sysprotect')
		exec('drop view vw_sqlsecure_syscolumns')
		exec('drop view vw_sqlsecure_syspermissions')
	
		print 'next loop!!!'
	
	
		fetch next from myc1
		into @dbid, @dbname
	
	
	end
	
	close myc1
	deallocate myc1

	exec('drop view vw_sqlsecure_databases')

	print 'Completed'

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

