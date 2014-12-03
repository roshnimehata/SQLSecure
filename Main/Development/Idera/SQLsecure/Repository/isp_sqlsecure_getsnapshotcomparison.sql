SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_getsnapshotcomparison]'))
drop procedure [dbo].[isp_sqlsecure_getsnapshotcomparison]
GO


CREATE procedure [dbo].[isp_sqlsecure_getsnapshotcomparison] 
(
	@snapshotid int,
	@snapshotid2 int,
	@returnstatements bit = 0
)
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Compare two snapshots and return a table of comparison results by object and permission
   -- 	           
   -- 	           Parameters:
   -- 	             @snapshotid - the id of the first snapshot to compare
   -- 	             @snapshotid2 - the id of the second snapshot to compare
   --
   --

--	declare @err int, @msg nvarchar(500)
--	declare @programname nvarchar(128), @action nvarchar(32), @category nvarchar(32), @success nvarchar(32), @failure nvarchar(32)
--	select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)
--	select @action=N'Compare', @category=N'Snapshot', @success=N'Success Audit', @failure=N'Failure Audit'

	declare @connectionname nvarchar(400)
	select @connectionname = connectionname
		from serversnapshot where snapshotid=@snapshotid


	create table #tempdiff (
					snapshotid int,
					snapshotidold int,
					connectionname nvarchar(400),
					difftype nvarchar(400),
					difflevel nvarchar(15), 
					diffobjecttable nvarchar(400),
					diffobjecttype nvarchar(256),
					diffobjectname nvarchar(400),
					diffusername nvarchar(400),
					diffvaluename nvarchar(400),
					diffdbname nvarchar(256), 
					oldid nvarchar(400),
					newid nvarchar(400),
					oldvalue nvarchar(4000),
					newvalue nvarchar(4000))

	DECLARE @SQL nvarchar(max),
		@debug bit, @runtime datetime
	DECLARE @SQLTable table (tablename nvarchar(128), comparetype nvarchar(20), sqltext nvarchar(max))
	DECLARE @TempDbId int

	SELECT @runtime = getdate(),
		@debug = 1

	if (@debug = 0)
		set nocount on
	else
	begin
		set nocount off
		print convert(nvarchar, @runtime, 8) + ' begin execution'
	end

	-- build the sql strings to find changed values in existing records
	INSERT INTO @SQLTable
		SELECT TABLE_NAME, N'change', CAST(N'declare @snapshotid1 int, @snapshotid2 int;'
			+ N'select @snapshotid1=' + convert(nvarchar, @snapshotid) 
				 + N', @snapshotid2=' + convert(nvarchar, @snapshotid2) + N';'
			+ N'SELECT snapshotid=@snapshotid1, snapshotidold=@snapshotid2,'
				 + N'connectionname=N''' + @connectionname + N''',' 
				 + N'difftype=N''changed'','
				 + N'difflevel=' + case		-- difflevel - DB for database level objects or SV for server level objects
						when TABLE_NAME IN (select A.TABLE_NAME
												from INFORMATION_SCHEMA.COLUMNS A
													INNER JOIN INFORMATION_SCHEMA.TABLES B ON A.TABLE_NAME = B.TABLE_NAME 
												where A.COLUMN_NAME = N'dbid'
													AND B.TABLE_TYPE = N'BASE TABLE' 
													AND A.IS_NULLABLE = N'NO' )
							then N'N''DB'''
						else N'N''SV'''
						end + N','
				 + N'diffobjecttable=N''' + TABLE_NAME + N''','
				 + N'diffobjecttype=' + case		-- diffobjecttype - for tables with multiple types of data
						when TABLE_NAME = N'serversnapshot'
							then N'N''Server Setting'''
						when TABLE_NAME = N'serverosobject'
							then N'dbo.getserverosobjecttypename(new.objecttype)'
						when TABLE_NAME = N'serverosobjectpermission'
							then N'dbo.getserverosobjecttypename(dbo.getserverosobjecttype(new.snapshotid, new.osobjectid))'
						when TABLE_NAME = N'serverservice'
							then N'N''Service'''
						when TABLE_NAME = N'serveroswindowsaccount'
							then N'N''OS Windows '' + new.type'
						when TABLE_NAME = N'windowsaccount'
							then N'N''Windows '' + new.type'
						when TABLE_NAME = N'endpoint'
							then N'new.type'
						when TABLE_NAME = N'serverprincipal'
							then N'case when new.type <> N''S'' then N''SQL '' else N'''' end + dbo.getserverprincipaltypename(new.type)'
						when TABLE_NAME = N'serverpermission'
							then N'case when dbo.getserverprincipaltype(new.snapshotid, new.grantee) <> N''S'' then N''SQL '' else N'''' end + dbo.getserverprincipaltypename(dbo.getserverprincipaltype(new.snapshotid, new.grantee))'
						when TABLE_NAME = N'sqldatabase'
							then N'N'' Database'''		-- the extra preceeding space is intentional here for sorting purposes
						when TABLE_NAME = N'databaseprincipal'
							then N'dbo.getdatabaseprincipaltypename(dbo.getdatabaseprincipaltype(new.snapshotid, new.dbid, new.uid))'
						when TABLE_NAME LIKE (N'databaseschema%')
							then N'N''Schema'''
						when TABLE_NAME = N'databaseobject'
							then N'dbo.getobjecttypename(new.type)'
						when TABLE_NAME = N'databaseobjectpermission'
							then N'dbo.getobjecttypenamebyobjectid(new.snapshotid, new.dbid, new.classid, new.parentobjectid, new.objectid)'
						else N'N'''''
						end + N','
				 + N'diffobjectname=' + case		-- diffobjectname
						when TABLE_NAME LIKE N'serverosobject%'
							then N'dbo.getserverosobjectname(new.snapshotid, new.osobjectid)'
						when TABLE_NAME IN (N'serveroswindowsaccount', N'windowsaccount', N'endpoint', N'serverprincipal')
							then N'new.name'
						when TABLE_NAME = N'serverpermission'
							then N'new.permission'
						when TABLE_NAME = N'serverservice'
							then N'new.servicename'
						when TABLE_NAME LIKE N'databaseschema%'
							then N'dbo.getschemaname(new.snapshotid, new.dbid, new.schemaid)'
						when TABLE_NAME = N'databaseobject'
							then N'dbo.getdatabaseobjectname(new.snapshotid, new.dbid, new.classid, new.parentobjectid, new.objectid)'
						when TABLE_NAME = N'databaseobjectpermission'
							then N'dbo.getdatabaseobjectname(new.snapshotid, new.dbid, new.classid, new.parentobjectid, new.objectid)'
						when TABLE_NAME LIKE N'databaseprincipal%'
							then N'dbo.getdatabaseprincipalname(new.snapshotid, new.dbid, new.uid)'
						else N'N'''''
						end + N','
				 + N'diffusername=' + case		-- diffusername - for permission users, owners, etc.
						when TABLE_NAME = N'serverosobjectpermission'
							then 'dbo.getserveroswindowsaccountname(new.snapshotid, new.sid)'
						when TABLE_NAME = N'endpoint'
							then 'dbo.getserverprincipalname(new.snapshotid, new.principalid)'
						when TABLE_NAME = N'serverpermission'
							then 'dbo.getserverprincipalname(new.snapshotid, new.grantee)'
						when TABLE_NAME IN (N'databaseschema', N'databaseprincipal', N'databaseprincipalpermission')
							then N'dbo.getdatabaseprincipalname(new.snapshotid, new.dbid, new.uid)'
						when TABLE_NAME IN (N'databaseschemapermission', N'databaseobjectpermission')
							then N'dbo.getdatabaseprincipalname(new.snapshotid, new.dbid, new.grantee)'
						else N'N'''''
						end + N','
				 + N'diffvaluename=' + case		-- diffvaluename - for column level value differences
						when COLUMN_NAME = N'authenticationmode'
							then 'N''Authentication Mode'''
						when COLUMN_NAME = N'os'
							then 'N''Windows OS'''
						when COLUMN_NAME = N'version'
							then 'N''SQL Server Version'''
						when COLUMN_NAME = N'edition'
							then 'N''SQL Server Edition'''
						when COLUMN_NAME = N'loginauditmode'
							then 'N''Login Audit Mode'''
						when COLUMN_NAME = N'enableproxyaccount'
							then 'N''Proxy Account Enabled'''
						when COLUMN_NAME = N'enablec2audittrace'
							then 'N''C2 Audit Trace Enabled'''
						when COLUMN_NAME = N'crossdbownershipchaining'
							then 'N''Cross DB Ownership Chaining Enabled'''
						when COLUMN_NAME = N'casesensitivemode'
							then 'N''Case Sensitive'''
						when COLUMN_NAME = N'allowsystemtableupdates'
							then 'N''Allow System Table Updates'''
						when COLUMN_NAME = N'remoteadminconnectionsenabled'
							then 'N''Remote DAC Enabled'''
						when COLUMN_NAME = N'remoteaccessenabled'
							then 'N''Remote Connections Allowed'''
						when COLUMN_NAME = N'scanforstartupprocsenabled'
							then 'N''Scan For Startup Procs Enabled'''
						when COLUMN_NAME = N'sqlmailxpsenabled'
							then 'N''SQL Mail Enabled'''
						when COLUMN_NAME = N'databasemailxpsenabled'
							then 'N''Database Mail Enabled'''
						when COLUMN_NAME = N'oleautomationproceduresenabled'
							then 'N''OLE Automation Enabled'''
						when COLUMN_NAME = N'webassistantproceduresenabled'
							then 'N''Web Assistant Enabled'''
						when COLUMN_NAME = N'xp_cmdshellenabled'
							then 'N''xp_cmdshell Enabled'''
						when COLUMN_NAME = N'agentmailprofile'
							then 'N''Agent Mail Profile'''
						when COLUMN_NAME = N'hideinstance'
							then 'N''Hide Instance'''
						when COLUMN_NAME = N'agentsysadminonly'
							then 'N''Agent sysadmin only'''
						when COLUMN_NAME = N'serverisdomaincontroller'
							then 'N''Server is Domain Controller'''
						when COLUMN_NAME = N'replicationenabled'
							then 'N''Replication Enabled'''
						when COLUMN_NAME = N'sapasswordempty'
							then 'N''sa Account Password Empty'''
						when COLUMN_NAME = N'disktype'
							then 'N''Disk Type'''
						when COLUMN_NAME = N'auditflags'
							then 'N''Auditing'''
						when COLUMN_NAME = N'filesystemrights'
							then 'N''Rights'''
						when COLUMN_NAME = N'accesstype'
							then 'N''Access Type'''
						when COLUMN_NAME = N'isinherited'
							then 'N''Inherited'''
						when COLUMN_NAME = N'isadminendpoint'
							then 'N''Is Admin Endpoint'''
						when COLUMN_NAME = N'ipaddress'
							then 'N''IP Address'''
						when COLUMN_NAME = N'dynamicport'
							then 'N''Dynamic Port'''
						when COLUMN_NAME = N'port'
							then 'N''TCP Port'''
						when COLUMN_NAME = N'displayname'
							then 'N''Display Name'''
						when COLUMN_NAME = N'servicepath'
							then 'N''Path to executable'''
						when COLUMN_NAME = N'startuptype'
							then 'N''Startup Type'''
						when COLUMN_NAME = N'loginname'
							then 'N''Login Name'''
						when COLUMN_NAME = N'serveraccess'
							then 'N''Server Access'''
						when COLUMN_NAME = N'serverdeny'
							then 'N''Server Deny'''
						when COLUMN_NAME = N'isexpirationchecked'
							then 'N''Expiration Checked'''
						when COLUMN_NAME = N'ispolicychecked'
							then 'N''Policy Checked'''
						when COLUMN_NAME = N'ispasswordnull'
							then 'N''Password Null'''
						when COLUMN_NAME = N'defaultdatabase'
							then 'N''Default Database'''
						when COLUMN_NAME = N'defaultlanguage'
							then 'N''Default Language'''
						when COLUMN_NAME = N'guestenabled'
							then 'N''Is Guest Enabled'''
						when COLUMN_NAME = N'dbfilename'
							then 'N''File Name'''
						when COLUMN_NAME = N'replicationcategory'
							then 'N''Replication Category'''
						when COLUMN_NAME = N'isalias'
							then 'N''Is Aliased'''
						when COLUMN_NAME = N'altuid'
							then 'N''Aliased To'''
						when COLUMN_NAME = N'hasaccess'
							then 'N''Has Access'''
						when COLUMN_NAME = N'defaultschemaname'
							then 'N''Default Schema'''
						when COLUMN_NAME = N'isgrant'
							then 'N''Grant'''
						when COLUMN_NAME = N'isgrantwith'
							then 'N''With Grant'''
						when COLUMN_NAME = N'isdeny'
							then 'N''Deny'''
						when COLUMN_NAME = N'runatstartup'
							then 'N''Run At Startup'''
						when COLUMN_NAME = N'isencrypted'
							then 'N''Encrypted'''
						when COLUMN_NAME = N'userdefined'
							then 'N''User Defined'''
						else N'''' + upper(left(COLUMN_NAME,1)) + substring(COLUMN_NAME,2,128) + N''''
						end + N','
				 + N'diffdbname=' + case		-- diffdbname - for database level objects
						when TABLE_NAME IN (select A.TABLE_NAME
											from INFORMATION_SCHEMA.COLUMNS A
												INNER JOIN INFORMATION_SCHEMA.TABLES B ON A.TABLE_NAME = B.TABLE_NAME 
											where A.COLUMN_NAME = N'dbid'
												AND B.TABLE_TYPE = N'BASE TABLE' 
												AND A.IS_NULLABLE = N'NO' )
							then N'dbo.getdatabasename(new.snapshotid, new.dbid)'
						else N''''''
						end + N','
				 + N'oldid=' + case		-- oldid - when the changed value is an id, the original value goes here and the related name is looked up for the value
						when COLUMN_NAME IN (N'schemaid', N'grantor', N'usersid', N'uid')
							then N'old.[' + COLUMN_NAME + ']'
						when TABLE_NAME = N'databaseobject' AND COLUMN_NAME = N'type'
							then N'old.[' + COLUMN_NAME + ']'
						else N'null'
						end + N','
				 + N'newid=' + case		-- newid - when the changed value is an id, the new value goes here and the related name is looked up for the value
						when COLUMN_NAME IN (N'schemaid', N'grantor', N'usersid', N'uid')
							then N'new.[' + COLUMN_NAME + ']'
						when TABLE_NAME = N'databaseobject' AND COLUMN_NAME = N'type'
							then N'new.[' + COLUMN_NAME + ']'
						else N'null'
						end + N','
				 + N'oldvalue=' + case		-- oldvalue - the original value or name associated with the original id
						when TABLE_NAME = N'serverosobjectpermission'
							then N'case when old.auditflags is null then dbo.getaccesstypename(old.accesstype) else N''Audit '' + dbo.getauditflagsnames(old.auditflags) end + N'' '' 
										+ case when dbo.getserverosobjecttype(old.snapshotid, old.osobjectid) = N''Reg'' then dbo.getregistryrightsnames(old.filesystemrights) else dbo.getfilesystemrightsnames(old.filesystemrights) end'
						when COLUMN_NAME = N'schemaid'
							then N'dbo.getschemaname(old.snapshotid, old.dbid, old.schemaid)'
						when TABLE_NAME = N'databaseobject' AND COLUMN_NAME = N'type'
							then N'dbo.getobjecttypename(old.type)'
						when TABLE_NAME = N'serverpermission' AND COLUMN_NAME = N'grantor'		-- put before general grantor check
							then N'dbo.getserverprincipalname(old.snapshotid, old.' + COLUMN_NAME + N')'
						when COLUMN_NAME IN (N'grantee', N'grantor', N'uid')
							then N'dbo.getdatabaseprincipalname(old.snapshotid, old.dbid, old.' + COLUMN_NAME + N')'
						when COLUMN_NAME = N'owner'
							then case when TABLE_NAME = N'sqldatabase'
									then N'old.[' + COLUMN_NAME + ']'
									else N'dbo.getdatabaseprincipalname(old.snapshotid, old.dbid, old.' + COLUMN_NAME + N')'
									end
						when COLUMN_NAME IN (N'ownersid')
							then N'dbo.getserverprincipalname(old.snapshotid, old.' + COLUMN_NAME + N')'
						else N'old.[' + COLUMN_NAME + ']'
						end + N','
				 + N'newvalue=' + case		-- newvalue - the changed value or name associated with a changed id
						when TABLE_NAME = N'serverosobjectpermission'
							then N'case when new.auditflags is null then dbo.getaccesstypename(new.accesstype) else N''Audit '' + dbo.getauditflagsnames(new.auditflags) end + N'' '' 
										+ case when dbo.getserverosobjecttype(new.snapshotid, new.osobjectid) = N''Reg'' then dbo.getregistryrightsnames(new.filesystemrights) else dbo.getfilesystemrightsnames(new.filesystemrights) end'
						when COLUMN_NAME = N'schemaid'
							then N'dbo.getschemaname(new.snapshotid, new.dbid, new.schemaid)'
						when TABLE_NAME = N'databaseobject' AND COLUMN_NAME = N'type'
							then N'dbo.getobjecttypename(new.type)'
						when TABLE_NAME = N'serverpermission' AND COLUMN_NAME = N'grantor'		-- put before general grantor check
							then N'dbo.getserverprincipalname(new.snapshotid, new.' + COLUMN_NAME + N')'
						when COLUMN_NAME IN (N'grantee', N'grantor', N'uid')
							then N'dbo.getdatabaseprincipalname(new.snapshotid, new.dbid, new.' + COLUMN_NAME + N')'
						when COLUMN_NAME = N'owner'
							then case when TABLE_NAME = N'sqldatabase'
									then N'new.[' + COLUMN_NAME + ']'
									else N'dbo.getdatabaseprincipalname(new.snapshotid, new.dbid, new.' + COLUMN_NAME + N')'
									end
						when COLUMN_NAME IN (N'ownersid')
							then N'dbo.getserverprincipalname(new.snapshotid, new.' + COLUMN_NAME + N')'
						else N'new.[' + COLUMN_NAME + ']'
						end
		+ N' INTO #tempdiff_temp'
		+ N' FROM ' + TABLE_SCHEMA + N'.[' + TABLE_NAME + N'] (NOLOCK) new,' + TABLE_SCHEMA + N'.[' + TABLE_NAME + N'] (NOLOCK) old'
		+ N' WHERE new.snapshotid = @snapshotid1'--+ convert(varchar, @snapshotid)
				 + N' and old.snapshotid = @snapshotid2'--+ convert(varchar, @snapshotid2)
				 + case when TABLE_NAME = N'serverosobject'
							then N' and new.objectname = old.objectname'
						else N''
						end
				 + case when TABLE_NAME = N'serverosobjectpermission'
							then N' and dbo.getserverosobjectname(new.snapshotid, new.osobjectid) = dbo.getserverosobjectname(old.snapshotid, old.osobjectid)'
							   + N' and dbo.getserveroswindowsaccountname(new.snapshotid, new.sid) = dbo.getserveroswindowsaccountname(old.snapshotid, old.sid)'
							   + N' and isnull(new.[auditflags],0) = isnull(old.[auditflags],0)'
							   + N' and new.[isinherited] = old.[isinherited]'
						else N''
						end
				 + case when TABLE_NAME in (select A.TABLE_NAME
											from INFORMATION_SCHEMA.COLUMNS A
												INNER JOIN INFORMATION_SCHEMA.TABLES B ON A.TABLE_NAME = B.TABLE_NAME 
											where A.COLUMN_NAME = N'name'
												AND B.TABLE_TYPE = N'BASE TABLE' )
							then N' and new.name = old.name'
						else N''
						end
				 + case when TABLE_NAME = N'serverprotocol'
							then N' and new.protocolname = old.protocolname'
						else N''
						end
				 + case when TABLE_NAME = N'serverservice'
							then N' and new.servicetype = old.servicetype and new.servicename = old.servicename'
						else N''
						end
				 + case when TABLE_NAME = N'serverpermission'
							then N' and new.majorid = old.majorid and new.minorid = old.minorid'
						else N''
						end
				 + case when TABLE_NAME = N'sqldatabase'
							then N' and new.databasename = old.databasename'
						else case when TABLE_NAME in (select A.TABLE_NAME
														from INFORMATION_SCHEMA.COLUMNS A
															INNER JOIN INFORMATION_SCHEMA.TABLES B ON A.TABLE_NAME = B.TABLE_NAME 
														where A.COLUMN_NAME = N'dbid'
															AND B.TABLE_TYPE = N'BASE TABLE' 
															AND A.IS_NULLABLE = N'NO' )
--										NOTE: BE VERY CAREFUL WHEN CHANGING THIS IT HAS BEEN CREATED IN THIS FORMAT SPECIFICALLY FOR PERFORMANCE REASONS
--												databases must be compared by name and calling functions in the where clause causes bad performance and works much better with the subselects
--										then N' and dbo.getdatabasename(new.snapshotid, new.dbid) = dbo.getdatabasename(old.snapshotid, old.dbid) and new.dbid <> 2'
										then N'	and (select databasename from sqldatabase (NOLOCK) where snapshotid = new.snapshotid and dbid = new.dbid) = (select databasename from sqldatabase (NOLOCK) where snapshotid = old.snapshotid and dbid = old.dbid)'
												+ N' and new.dbid <> 2 and old.dbid <> 2'
									else N''
									end
						end
				 + case when TABLE_NAME in (select A.TABLE_NAME
												from INFORMATION_SCHEMA.COLUMNS A
													INNER JOIN INFORMATION_SCHEMA.TABLES B ON A.TABLE_NAME = B.TABLE_NAME 
												where A.COLUMN_NAME = N'type'
													AND B.TABLE_TYPE = N'BASE TABLE' )
							then N'	and new.type = old.type'
						else N''
						end
				 + case when TABLE_NAME = N'databaseprincipalpermission'
							then N' and dbo.getdatabaseprincipalname(new.snapshotid, new.dbid, new.uid) = dbo.getdatabaseprincipalname(old.snapshotid, old.dbid, old.uid)'
						else N''
						end
				 + case when TABLE_NAME = N'databaseschema'
							then N' and new.schemaname = old.schemaname'
						else case when TABLE_NAME in (select A.TABLE_NAME
														from INFORMATION_SCHEMA.COLUMNS A
															INNER JOIN INFORMATION_SCHEMA.TABLES B ON A.TABLE_NAME = B.TABLE_NAME 
														where A.COLUMN_NAME = N'schemaid'
															AND B.TABLE_TYPE = N'BASE TABLE' )
--										NOTE: BE VERY CAREFUL WHEN CHANGING THIS IT HAS BEEN CREATED IN THIS FORMAT SPECIFICALLY FOR PERFORMANCE REASONS
--												schemas are compared by id for now
										then N' and ((new.schemaid is null and old.schemaid is null) 
														or new.schemaid = old.schemaid)'
--										then N' and 1 = (select count(*) from databaseschema a, databaseschema b 
--															where a.snapshotid = new.snapshotid 
--																and a.dbid = new.dbid 
--																and a.schemaid = new.schemaid 
--																and b.snapshotid = old.snapshotid 
--																and b.dbid = old.dbid 
--																and b.schemaid = old.schemaid 
--																and a.schemaname = b.schemaname)'
									else N''
									end
						end
				 + case when TABLE_NAME in (select A.TABLE_NAME
											from INFORMATION_SCHEMA.COLUMNS A
												INNER JOIN INFORMATION_SCHEMA.TABLES B ON A.TABLE_NAME = B.TABLE_NAME 
											where A.COLUMN_NAME = N'classid'
												AND B.TABLE_TYPE = N'BASE TABLE'  )
							then N' and new.classid = old.classid'
						else N''
						end
				 + case when TABLE_NAME = N'databaseobject'
							then N' and new.parentobjectid = old.parentobjectid'
						else N''
						end
				 + case when TABLE_NAME = N'databaseobjectpermission'
							then N' 		and 1 = (select count(*) 
														from databaseobject (NOLOCK) a, databaseobject (NOLOCK) b 
														where a.snapshotid = new.snapshotid 
															and a.dbid = new.dbid 
															and a.classid = new.classid 
															and a.parentobjectid = new.parentobjectid
															and b.snapshotid = old.snapshotid 
															and b.dbid = old.dbid 
															and b.classid = old.classid 
															and b.parentobjectid = old.parentobjectid
															and a.name = b.name) '
						else N''
						end
				 + case when TABLE_NAME in (select A.TABLE_NAME
												from INFORMATION_SCHEMA.COLUMNS A
													INNER JOIN INFORMATION_SCHEMA.TABLES B ON A.TABLE_NAME = B.TABLE_NAME 
												where A.COLUMN_NAME = N'permission'
													AND B.TABLE_TYPE = N'BASE TABLE' )
							then N' and new.permission = old.permission'
						else N''
						end
				 + case when TABLE_NAME in (select A.TABLE_NAME
												from INFORMATION_SCHEMA.COLUMNS A
													INNER JOIN INFORMATION_SCHEMA.TABLES B ON A.TABLE_NAME = B.TABLE_NAME 
												where A.COLUMN_NAME = N'grantee'
													AND B.TABLE_TYPE = N'BASE TABLE' )
							then N' and new.grantee = old.grantee'
						else N''
						end
				 + case when TABLE_NAME in (select A.TABLE_NAME
												from INFORMATION_SCHEMA.COLUMNS A
													INNER JOIN INFORMATION_SCHEMA.TABLES B ON A.TABLE_NAME = B.TABLE_NAME 
												where A.COLUMN_NAME = N'grantor'
													AND B.TABLE_TYPE = N'BASE TABLE' )
							then N' and new.grantor = old.grantor'
						else N''
						end
				 + case when COLUMN_NAME = N'runatstartup'
							then N' and new.dbid = 1'
						else N''
						end
				 + case when COLUMN_NAME in (N'runatstartup', N'isencrypted')
							then N' and new.type = N''P'''
						else N''
						end
				 + case when COLUMN_NAME = N'userdefined'
							then N' and new.type <> N''iCO'' and old.type <> N''iCO'''
						else N''
						end
				 + N' and new.[' + COLUMN_NAME + N'] <> old.[' + COLUMN_NAME + N']'
				 + N'; '-- print ''' + TABLE_NAME + '.' + COLUMN_NAME + ''';'
				 + N' INSERT INTO #tempdiff SELECT * FROM #tempdiff_temp; DROP TABLE #tempdiff_temp;'

		AS NVARCHAR(MAX)) 
	FROM INFORMATION_SCHEMA.COLUMNS  
	WHERE TABLE_NAME in (select A.TABLE_NAME from INFORMATION_SCHEMA.TABLES A where A.TABLE_NAME = TABLE_NAME and A.TABLE_TYPE = N'BASE TABLE')
		AND TABLE_NAME IN (select A.TABLE_NAME
								from INFORMATION_SCHEMA.COLUMNS A
									INNER JOIN INFORMATION_SCHEMA.TABLES B ON A.TABLE_NAME = B.TABLE_NAME 
								where A.COLUMN_NAME = N'snapshotid'
									AND B.TABLE_TYPE = N'BASE TABLE' )
		AND (TABLE_NAME NOT IN (N'serverosobjectpermission', N'serveroswindowsaccount', N'windowsaccount') OR COLUMN_NAME <> N'sid')
		AND (TABLE_NAME <> N'sqldatabase' OR COLUMN_NAME <> N'status')
		AND (TABLE_NAME NOT IN (N'databaseprincipal',N'databaseprincipalpermission') OR COLUMN_NAME <> N'uid')
		AND (TABLE_NAME NOT LIKE (N'databaseschema%') OR COLUMN_NAME <> N'schemaid')
		AND TABLE_NAME NOT LIKE (N'%member')
		AND TABLE_NAME NOT LIKE (N'policy%')
		AND TABLE_NAME NOT LIKE (N'serverfilterrule%')
		AND TABLE_NAME NOT IN (N'snapshothistory', N'ancillarywindowsgroup')
		AND COLUMN_NAME not in (N'connectionname', N'servername', N'instancename', N'starttime', N'endtime', N'automated', 
								N'numobject',N'numpermission', N'numlogin', N'numwindowsgroupmember', 
								N'baseline', N'baselinecomment', N'snapshotcomment', N'collectorversion', N'hashkey',
								N'snapshotid', N'registeredserverid', N'osobjectid', N'endpointid', N'protocolname', N'servicetype', N'servicename',
								N'auditflags', N'accesstype', N'isinherited', N'principalid', N'majorid', N'minorid',
								N'dbid', N'classid', N'parentobjectid', N'objectid', N'schemaid', N'permission', N'grantee', N'grantor', 
								N'type', N'name', N'isrevoke', N'isaudited')
		AND DATA_TYPE <>  N'nvarchar(max)'

		--AND TABLE_NAME LIKE 'sql%'	--	restriction for testing on certain tables


	-- build the sql strings to find records in the compare snapshot (snapshotid2) that were not in the current snapshot (@snapshotid)
	INSERT INTO @SQLTable
		SELECT TABLE_NAME, N'missing', CAST(N'declare @snapshotid1 int, @snapshotid2 int;'
			+ N'select @snapshotid1=' + convert(nvarchar, @snapshotid) 
				 + N', @snapshotid2=' + convert(nvarchar, @snapshotid2) + N';'
			+ N'SELECT snapshotid=@snapshotid1, snapshotidold=@snapshotid2,'
				 + N'connectionname=N''' + @connectionname + N''',' 
				 + N'difftype=N''missing'','
				 + N'difflevel=' + case		-- difflevel - DB for database level objects or SV for server level objects
						when TABLE_NAME IN (select A.TABLE_NAME
												from INFORMATION_SCHEMA.COLUMNS A
													INNER JOIN INFORMATION_SCHEMA.TABLES B ON A.TABLE_NAME = B.TABLE_NAME 
												where A.COLUMN_NAME = N'dbid'
													AND B.TABLE_TYPE = N'BASE TABLE' 
													AND A.IS_NULLABLE = N'NO' )
							then N'N''DB'''
						else N'N''SV'''
						end + N','
				 + N'diffobjecttable=N''' + TABLE_NAME + N''','
				 + N'diffobjecttype=' + case		-- diffobjecttype - for tables with multiple types of data
						when TABLE_NAME = N'serverosobject'
							then N'dbo.getserverosobjecttypename(new.objecttype)'
						when TABLE_NAME = N'serverosobjectpermission'
							then N'dbo.getserverosobjecttypename(dbo.getserverosobjecttype(new.snapshotid, new.osobjectid))'
						when TABLE_NAME = N'serveroswindowsaccount'
							then N'N''OS Windows '' + new.type'
						when TABLE_NAME = N'windowsaccount'
							then N'N''Windows '' + new.type'
						when TABLE_NAME = N'endpoint'
							then N'new.type'
						when TABLE_NAME = N'serverservice'
							then N'''Service'''
						when TABLE_NAME = N'endpoint'
							then N'''Endpoint'''
						when TABLE_NAME = N'serverprincipal'
							then N'case when new.type <> N''S'' then N''SQL '' else N'''' end + dbo.getserverprincipaltypename(new.type)'
						when TABLE_NAME = N'serverpermission'
							then N'case when dbo.getserverprincipaltype(new.snapshotid, new.grantee) <> N''S'' then N''SQL '' else N'''' end + dbo.getserverprincipaltypename(dbo.getserverprincipaltype(new.snapshotid, new.grantee))'
						when TABLE_NAME = N'sqldatabase'
							then N''' Database'''
						when TABLE_NAME = N'databaseprincipal'
							then N'dbo.getdatabaseprincipaltypename(new.type)'
						when TABLE_NAME LIKE (N'databaseschema%')
							then N'''Schema'''
						when TABLE_NAME = N'databaseobject'
							then N'dbo.getobjecttypename(new.type)'
						when TABLE_NAME = N'databaseobjectpermission'
							then N'dbo.getobjecttypenamebyobjectid(new.snapshotid, new.dbid, new.classid, new.parentobjectid, new.objectid)'
						else N''''''
						end + N','
				 + N'diffobjectname=' + case		-- diffobjectname
						when TABLE_NAME LIKE N'serverosobject%'
							then N'dbo.getserverosobjectname(new.snapshotid, new.osobjectid)'
						when TABLE_NAME IN (N'serveroswindowsaccount', N'windowsaccount', N'endpoint', N'serverprincipal')
							then N'new.name'
						when TABLE_NAME = N'serverservice'
							then N'new.servicename'
						when TABLE_NAME = N'serverpermission'
							then N'new.permission'
						when TABLE_NAME LIKE N'databaseschema%'
							then N'dbo.getschemaname(new.snapshotid, new.dbid, new.schemaid)'
						when TABLE_NAME IN (N'databaseobject', N'databaseobjectpermission')
							then N'dbo.getdatabaseobjectname(new.snapshotid, new.dbid, new.classid, new.parentobjectid, new.objectid)'
						when TABLE_NAME LIKE N'databaseprincipal%'
							then N'dbo.getdatabaseprincipalname(new.snapshotid, new.dbid, new.uid)'
						else N''''''
						end + N','
				 + N'diffusername=' + case		-- diffusername - for permission users, owners, etc.
						when TABLE_NAME = N'serverosobjectpermission'
							then N'dbo.getserveroswindowsaccountname(new.snapshotid, new.sid)'
						when TABLE_NAME = N'endpoint'
							then N'dbo.getserverprincipalname(new.snapshotid, new.principalid)'
						when TABLE_NAME = N'serverpermission'
							then N'dbo.getserverprincipalname(new.snapshotid, new.grantee)'
						when TABLE_NAME IN (N'databaseschema', N'databaseprincipal', N'databaseprincipalpermission')
							then N'dbo.getdatabaseprincipalname(new.snapshotid, new.dbid, new.uid)'
						when TABLE_NAME IN (N'databaseschemapermission', N'databaseobjectpermission')
							then N'dbo.getdatabaseprincipalname(new.snapshotid, new.dbid, new.grantee)'
						else N''''''
						end + N','
				 + N'diffvaluename=' + case		-- diffvaluename
						when TABLE_NAME LIKE N'%permission'
							then N'N''Permission'''
						when TABLE_NAME = N'serverservice'
							then N'N''Service'''
						when TABLE_NAME = N'serverprotocol'
							then N'N''Protocol'''
						when TABLE_NAME = N'serverprincipal'
							then N'case when new.type <> N''S'' then N''SQL '' else N'''' end + dbo.getserverprincipaltypename(new.type)'
						when TABLE_NAME = N'endpoint'
							then N'N''Endpoint'''
						when TABLE_NAME = N'serveroswindowsaccount'
							then N'N''OS Windows '' + new.type'
						when TABLE_NAME = N'windowsaccount'
							then N'N''Windows '' + new.type'
						when TABLE_NAME = N'serverosobject'
							then N'dbo.getserverosobjecttypename(new.objecttype)'
						when TABLE_NAME = N'sqldatabase'
							then N'N''Database'''
						when TABLE_NAME = N'databaseprincipal'
							then N'dbo.getdatabaseprincipaltypename(new.type)'
						when TABLE_NAME = N'databaseschema'
							then N'N''Schema'''
						when TABLE_NAME = N'databaseobject'
							then N'dbo.getobjecttypename(new.type)'
						else N''''''
						end + N','
				 + N'diffdbname=' + case		-- diffdbname - for database level objects
						when TABLE_NAME IN (select A.TABLE_NAME
												from INFORMATION_SCHEMA.COLUMNS A
													INNER JOIN INFORMATION_SCHEMA.TABLES B ON A.TABLE_NAME = B.TABLE_NAME 
												where A.COLUMN_NAME = N'dbid'
													AND B.TABLE_TYPE = N'BASE TABLE' 
													AND A.IS_NULLABLE = N'NO' )
							then N'dbo.getdatabasename(new.snapshotid, new.dbid)'
						else N''''''
						end + N','
				 + N'oldid=' + case 	-- oldid	oldids will always be the same as newids or null values will be fixed after running the queries
						when TABLE_NAME LIKE (N'serverosobject%')
							then N'new.osobjectid'
						when TABLE_NAME = N'serverprincipal'
							then N'new.principalid'
						when TABLE_NAME = N'endpoint'
							then N'new.endpointid'
						when TABLE_NAME = N'sqldatabase'
							then N'new.dbid'
						when TABLE_NAME LIKE (N'databaseprincipal%')
							then N'new.uid'
						when TABLE_NAME LIKE (N'databaseschema%')
							then N'new.schemaid'
						when TABLE_NAME LIKE (N'databaseobject%')
							then N'new.objectid'
						else N'null'
						end + N','
				 + N'newid=' + case 	-- newid	null values will be fixed after running the queries
						when TABLE_NAME LIKE (N'serverosobject%')
							then N'new.osobjectid'
						when TABLE_NAME = N'serverprincipal'
							then N'new.principalid'
						when TABLE_NAME = N'endpoint'
							then N'new.endpointid'
						when TABLE_NAME = N'sqldatabase'
							then N'new.dbid'
						when TABLE_NAME LIKE (N'databaseprincipal%')
							then N'new.uid'
						when TABLE_NAME LIKE (N'databaseschema%')
							then N'new.schemaid'
						when TABLE_NAME LIKE (N'databaseobject%')
							then N'new.objectid'
						else N'null'
						end + N','
				 + N'oldvalue=null,'	-- oldvalue - the original value or name associated with the original id
				 + N'newvalue=' + case		-- newvalue - the changed value or name associated with a changed id
						when TABLE_NAME = N'serverosobjectpermission'
							then N'case when new.auditflags is null then dbo.getaccesstypename(new.accesstype) else N''Audit '' + dbo.getauditflagsnames(new.auditflags) end + N'' '' 
										+ case when dbo.getserverosobjecttype(new.snapshotid, new.osobjectid) = N''Reg'' then dbo.getregistryrightsnames(new.filesystemrights) else dbo.getfilesystemrightsnames(new.filesystemrights) end'
						when TABLE_NAME = N'databasepermission'
							then N'case when new.isgrantwith = 1 then N''With Grant'' else case when new.isgrant = 1 then N''Grant'' else case when new.isdeny = 1 then N''Deny'' else N'' end end end'
						else N'null'
						end
		+ N' INTO #tempdiff_temp'
		+ N' FROM ' + case when TABLE_NAME = N'databaseobjectpermission' 
								then N'(' + TABLE_SCHEMA + N'.[' + TABLE_NAME + N'] (NOLOCK) new
											inner join ' + TABLE_SCHEMA + N'.[databaseobject] (NOLOCK) newobj 
												on (new.snapshotid = newobj.snapshotid
													and newobj.snapshotid = @snapshotid1
													and new.dbid = newobj.dbid
													and newobj.dbid <> 2 
													and new.classid = newobj.classid
													and new.parentobjectid = newobj.parentobjectid
													and new.objectid = newobj.objectid))'
									+ N' left join (' + TABLE_SCHEMA + N'.[' + TABLE_NAME + N'] (NOLOCK) old 
														inner join ' + TABLE_SCHEMA + N'.[databaseobject] (NOLOCK) oldobj 
															on (old.snapshotid = oldobj.snapshotid
																and oldobj.snapshotid = @snapshotid2
																and old.dbid = oldobj.dbid
																and old.classid = oldobj.classid
																and old.parentobjectid = oldobj.parentobjectid
																and old.objectid = oldobj.objectid))'
							else 
								+ TABLE_SCHEMA + N'.[' + TABLE_NAME + N'] (NOLOCK) new '
								+ N' left join ' + TABLE_SCHEMA + N'.[' + TABLE_NAME + N'] (NOLOCK) old'
							end

				+ N' on (old.snapshotid = @snapshotid2'
				 + case when TABLE_NAME = N'serverosobject'
							then N' and new.objectname = old.objectname'
						else N''
						end
				 + case when TABLE_NAME = N'serverosobjectpermission'
							then N' and dbo.getserverosobjectname(new.snapshotid, new.osobjectid) = dbo.getserverosobjectname(old.snapshotid, old.osobjectid)'
							   + N' and dbo.getserveroswindowsaccountname(new.snapshotid, new.sid) = dbo.getserveroswindowsaccountname(old.snapshotid, old.sid)'
							   + N' and isnull(new.[auditflags],0) = isnull(old.[auditflags],0)'
							   + N' and isnull(new.[accesstype],0) = isnull(old.[accesstype],0)'
							   + N' and new.[isinherited] = old.[isinherited]'
						else N''
						end
				 + case when TABLE_NAME in (select A.TABLE_NAME
												from INFORMATION_SCHEMA.COLUMNS A
													INNER JOIN INFORMATION_SCHEMA.TABLES B ON A.TABLE_NAME = B.TABLE_NAME 
												where A.COLUMN_NAME = N'name'
													AND B.TABLE_TYPE = N'BASE TABLE' )
							then N' and new.name = old.name'
						else N''
						end
				 + case when TABLE_NAME = N'serverprotocol'
							then N' and new.protocolname = old.protocolname'
						else N''
						end
				 + case when TABLE_NAME = N'serverservice'
							then N' and new.servicetype = old.servicetype and new.servicename = old.servicename'
						else N''
						end
				 + case when TABLE_NAME = N'serverpermission'
							then N' and new.majorid = old.majorid and new.minorid = old.minorid'
						else N''
						end
				 + case when TABLE_NAME = N'sqldatabase'
							then N' and new.databasename = old.databasename'
						else case when TABLE_NAME in (select A.TABLE_NAME
														from INFORMATION_SCHEMA.COLUMNS A
															INNER JOIN INFORMATION_SCHEMA.TABLES B ON A.TABLE_NAME = B.TABLE_NAME 
														where A.COLUMN_NAME = N'dbid'
															AND B.TABLE_TYPE = N'BASE TABLE' 
															AND A.IS_NULLABLE = N'NO' )
--										NOTE: BE VERY CAREFUL WHEN CHANGING THIS IT HAS BEEN CREATED IN THIS FORMAT SPECIFICALLY FOR PERFORMANCE REASONS
--												databases must be compared by name and calling functions in the where clause causes bad performance and works much better with the subselects
--										then N' and dbo.getdatabasename(new.snapshotid, new.dbid) = dbo.getdatabasename(old.snapshotid, old.dbid) and new.dbid <> 2'
										then N'	and (select databasename from sqldatabase (NOLOCK) where snapshotid = new.snapshotid and dbid = new.dbid) = (select databasename from sqldatabase (NOLOCK) where snapshotid = old.snapshotid and dbid = old.dbid)'
									else N''
									end
						end
				 + case when TABLE_NAME in (select A.TABLE_NAME
												from INFORMATION_SCHEMA.COLUMNS A
													INNER JOIN INFORMATION_SCHEMA.TABLES B ON A.TABLE_NAME = B.TABLE_NAME 
												where A.COLUMN_NAME = N'type'
													AND B.TABLE_TYPE = N'BASE TABLE' )
							then N'	and new.type = old.type'
						else N''
						end
				 + case when TABLE_NAME = N'databaseprincipalpermission'
							then N' and dbo.getdatabaseprincipalname(new.snapshotid, new.dbid, new.uid) = dbo.getdatabaseprincipalname(old.snapshotid, old.dbid, old.uid)'
						else N''
						end
				 + case when TABLE_NAME = N'databaseschema'
							then N' and new.schemaname = old.schemaname'
						else case when TABLE_NAME in (select A.TABLE_NAME
														from INFORMATION_SCHEMA.COLUMNS A
															INNER JOIN INFORMATION_SCHEMA.TABLES B ON A.TABLE_NAME = B.TABLE_NAME 
														where A.COLUMN_NAME = N'schemaid'
															AND B.TABLE_TYPE = N'BASE TABLE' )
--										NOTE: BE VERY CAREFUL WHEN CHANGING THIS IT HAS BEEN CREATED IN THIS FORMAT SPECIFICALLY FOR PERFORMANCE REASONS
--												schemas are compared by id for now
										then N' and ((new.schemaid is null and old.schemaid is null) 
														or new.schemaid = old.schemaid)'
--										then N' and ((new.schemaid is null and old.schemaid is null) 
--														or 1 = (select count(*) from databaseschema a, databaseschema b 
--																	where a.snapshotid = new.snapshotid 
--																		and a.dbid = new.dbid 
--																		and a.schemaid = new.schemaid 
--																		and b.snapshotid = old.snapshotid 
--																		and b.dbid = old.dbid 
--																		and b.schemaid = old.schemaid 
--																		and a.schemaname = b.schemaname))'
									else N''
									end
						end
				 + case when TABLE_NAME in (select A.TABLE_NAME
												from INFORMATION_SCHEMA.COLUMNS A
													INNER JOIN INFORMATION_SCHEMA.TABLES B ON A.TABLE_NAME = B.TABLE_NAME 
												where A.COLUMN_NAME = N'classid'
													AND B.TABLE_TYPE = N'BASE TABLE' )
							then N' and new.classid = old.classid'
						else N''
						end
				 + case when TABLE_NAME = N'databaseobject'
							then N' and new.parentobjectid = old.parentobjectid'
						else N''
						end
				 + case when TABLE_NAME in (select A.TABLE_NAME
												from INFORMATION_SCHEMA.COLUMNS A
													INNER JOIN INFORMATION_SCHEMA.TABLES B ON A.TABLE_NAME = B.TABLE_NAME 
												where A.COLUMN_NAME = N'permission'
													AND B.TABLE_TYPE = N'BASE TABLE' )
							then N' and new.permission = old.permission'
						else N''
						end
				 + case when TABLE_NAME in (select A.TABLE_NAME
												from INFORMATION_SCHEMA.COLUMNS A
													INNER JOIN INFORMATION_SCHEMA.TABLES B ON A.TABLE_NAME = B.TABLE_NAME 
												where A.COLUMN_NAME = N'grantee'
													AND B.TABLE_TYPE = N'BASE TABLE' )
							then N' and new.grantee = old.grantee'
						else N''
						end
				 + case when TABLE_NAME in (select A.TABLE_NAME
												from INFORMATION_SCHEMA.COLUMNS A
													INNER JOIN INFORMATION_SCHEMA.TABLES B ON A.TABLE_NAME = B.TABLE_NAME 
												where A.COLUMN_NAME = N'grantor'
													AND B.TABLE_TYPE = N'BASE TABLE' )
							then N' and new.grantor = old.grantor'
						else N''
						end
				 + N')'
		+ N' WHERE new.snapshotid = @snapshotid1'
		+ case when TABLE_NAME in (select A.TABLE_NAME
										from INFORMATION_SCHEMA.COLUMNS A
											INNER JOIN INFORMATION_SCHEMA.TABLES B ON A.TABLE_NAME = B.TABLE_NAME 
										where A.COLUMN_NAME = N'dbid'
											AND B.TABLE_TYPE = N'BASE TABLE' 
											AND A.IS_NULLABLE = N'NO' )
					then N'	and new.dbid <> 2'
				else N''
				end
				 + N' and old.snapshotid is null'
				 + N';'
				 + N' INSERT INTO #tempdiff SELECT * FROM #tempdiff_temp; DROP TABLE #tempdiff_temp;'
		AS NVARCHAR(MAX)) 
	FROM INFORMATION_SCHEMA.TABLES 
	WHERE   
			TABLE_NAME IN (select A.TABLE_NAME
							from INFORMATION_SCHEMA.COLUMNS A
								INNER JOIN INFORMATION_SCHEMA.TABLES B ON A.TABLE_NAME = B.TABLE_NAME 
							where A.COLUMN_NAME = N'snapshotid'
								AND B.TABLE_TYPE = N'BASE TABLE' )
		AND TABLE_NAME NOT LIKE (N'%member')
		AND TABLE_NAME NOT LIKE (N'policy%')
		AND TABLE_NAME NOT LIKE (N'serverfilterrule%')
		AND TABLE_NAME NOT IN (N'snapshothistory', N'ancillarywindowsgroup')

		--AND TABLE_NAME LIKE ('database%')

	-- modify the sql strings to find records in the current snapshot (@snapshotid) that were not in the compare snapshot (snapshotid2)
	INSERT INTO @SQLTable
		SELECT tablename, N'new', sqltext
			FROM @SQLTable
			WHERE comparetype = N'missing'

	UPDATE @SQLTable SET sqltext = replace(sqltext, N'''missing''', N'''new''') WHERE comparetype = N'new'
	UPDATE @SQLTable SET sqltext = replace(sqltext, N'.snapshotid = @snapshotid1', N'.snapshotid = @snapshotid3') WHERE comparetype = N'new'
	UPDATE @SQLTable SET sqltext = replace(sqltext, N'.snapshotid = @snapshotid2', N'.snapshotid = @snapshotid1') WHERE comparetype = N'new'
	UPDATE @SQLTable SET sqltext = replace(sqltext, N'.snapshotid = @snapshotid3', N'.snapshotid = @snapshotid2') WHERE comparetype = N'new'

	declare sqlcursor cursor for
		select sqltext from @SQLTable order by tablename, comparetype

	open sqlcursor
	fetch next from sqlcursor into @SQL
	while (@@fetch_status = 0)
	begin
		if (@debug = 1)
		begin
			select @runtime = getdate()
			print convert(nvarchar, @runtime, 8) + ' executing:' + @SQL
		end

		exec (@SQL)

		if (@debug = 1)
			print 'execution took ' + convert (nvarchar, datediff(second, @runtime, getdate())) + ' seconds'

		fetch next from sqlcursor into @SQL
	end

	close sqlcursor
	deallocate sqlcursor

	-- fix the values for new and missing records here to only return the matching columns for performance
	update #tempdiff set newid = null, newvalue = null where difftype = N'missing'
	update #tempdiff set oldid = null, oldvalue = null where difftype = N'new'

	select * from #tempdiff order by difflevel, diffdbname, diffobjecttype, diffobjectname, diffusername, diffvaluename, difftype desc

	drop table #tempdiff

	-- must be returned second to prevent the reports from failing
	if (@returnstatements = 1)
		select * from @SQLTable

--
--	declare @tempdiff table (
--					snapshotid int,
--					snapshotidold int,
--					connectionname nvarchar(400),
--					difftype nvarchar(400),
--					difflevel nvarchar(15), 
--					diffobjecttable nvarchar(400),
--					diffobjecttype nvarchar(256),
--					diffobjectname nvarchar(400),
--					diffusername nvarchar(400),
--					diffvaluename nvarchar(400),
--					diffdbname nvarchar(256), 
--					oldid nvarchar(400),
--					newid nvarchar(400),
--					oldvalue nvarchar(4000),
--					newvalue nvarchar(4000))
--
--	select * from @tempdiff
GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_getsnapshotcomparison] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
