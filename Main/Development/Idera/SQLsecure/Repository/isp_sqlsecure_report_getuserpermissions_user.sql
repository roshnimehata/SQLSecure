SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_report_getuserpermissions_user]'))
drop procedure [dbo].[isp_sqlsecure_report_getuserpermissions_user]
GO

CREATE PROCEDURE [dbo].[isp_sqlsecure_report_getuserpermissions_user]
(
	@user nvarchar(400),
	@server nvarchar(400),
	@usertype nvarchar(1),
	@rundate datetime = null,	--defaults to all
	@policyid int = 1,			--defaults to all
	@usebaseline bit = 0		--defaults to false
)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description : 
   --              Generate data for Users Permissions Reports

	SET NOCOUNT ON
	DECLARE @snapshotid int
	DECLARE @sid varbinary(85)
	DECLARE @databasename nvarchar(200)
	declare @validuser nchar(1)

	IF EXISTS (SELECT name FROM sysobjects WHERE xtype='u' AND name='#tmpserverpermission')
	BEGIN
		DROP TABLE #tmpserverpermission
	END

	-- Prevent SQL injection
	select @user = replace(@user, '*', '%')
	select @user = replace(@user, '--', '')
	select @user = replace(@user, ';', '')
	select @user = replace(@user, '''', '')

	CREATE TABLE #tmpservers (registeredserverid int)
		INSERT #tmpservers
			EXEC [dbo].[isp_sqlsecure_getpolicymemberlist]
				 @policyid = @policyid 

	CREATE TABLE #tmpserverpermission (snapshotid int, permissionlevel nvarchar(15), logintype nchar(1), loginname nvarchar(256), connectionname nvarchar(400), databasename nvarchar(256), principalid int, principalname nvarchar(128), principaltype nchar(1), 
	databaseprincipal nvarchar(128), databaseprincipaltype nchar(1), grantor int, grantorname nvarchar(128), grantee int, granteename nvarchar(128), classid int, permissiontype nvarchar(4), coveringfrom nvarchar(15), permission nvarchar(256), isgrant nchar(1), 
	isgrantwith nchar(1), isrevoke nchar(1), isdeny nchar(1), parentobjectid int, objectid int, objectname nvarchar(256), qualifiedname nvarchar(256), objecttype nvarchar(64), schemaid int, schemaname nvarchar(128), owner int, ownername nvarchar(128), 
	isaliased nchar(1), inherited nchar(1), sourcename nvarchar(256), sourcetype nvarchar(256), sourcepermission nvarchar(256))

	CREATE TABLE #tmpserver (connectionname nvarchar(500), loginname nvarchar(250), logintype nvarchar(32), serveraccess nvarchar(32), [disabled] nvarchar(16), [role] nvarchar(500), [userlogin] nvarchar(250))

	CREATE TABLE #tmpserverbase (connectionname nvarchar(500), loginname nvarchar(250), logintype nvarchar(32), serveraccess nvarchar(32), [disabled] nvarchar(16), [role] nvarchar(500))

	CREATE TABLE #tmproles (rolename nvarchar(256))

	declare @login nvarchar(500)
	declare @logintype nvarchar(32)
	declare @serveraccess nvarchar(32)
	declare @disabled nvarchar(16)
	declare @roles nvarchar(1000)
	declare @rolename nvarchar(256)
	set @roles = ''
	declare @count int

	declare @iscasesensitive nchar(1)

	--If the user wants information about ALL servers
	IF(@server = '%')
	BEGIN
		DECLARE @connectionname nvarchar(400)
		
		DECLARE cc CURSOR FOR SELECT snapshotid, connectionname FROM dbo.getsnapshotlist(@rundate, @usebaseline) WHERE registeredserverid IN (SELECT registeredserverid FROM #tmpservers) AND UPPER(connectionname) LIKE UPPER(@server)
		
		OPEN cc

		FETCH NEXT FROM cc INTO @snapshotid, @connectionname

		WHILE @@FETCH_STATUS = 0
		BEGIN

			select @iscasesensitive = casesensitivemode from serversnapshot where snapshotid = @snapshotid

			set @validuser = 'N'
	
			if (UPPER(@usertype) = 'W')
			begin
				if exists (select 1 from windowsaccount where snapshotid = @snapshotid and UPPER([name])= UPPER(@user))
					set @validuser = 'Y'
	
			end
			else if (UPPER(@usertype) = 'A')	-- SQLsecure 3.1 (Anshul Aggarwal) - Add support for Azure AD Users or Groups
			begin
				if (@iscasesensitive = 'Y')
				begin

					if exists (select 1 from serverprincipal where snapshotid = @snapshotid and CONVERT(varbinary(256), name)= CONVERT(varbinary(256), @user) and type in ('E', 'X'))
						set @validuser = 'Y'

				end
				else
				begin
					if exists (select 1 from serverprincipal where snapshotid = @snapshotid and UPPER(name)= UPPER(@user) and type in ('E', 'X'))
						set @validuser = 'Y'

				end

			end
			else if (UPPER(@usertype) = 'E' or UPPER(@usertype) = 'X')	-- SQLsecure 3.1 (Anshul Aggarwal) - Add support for Azure AD Users or Groups
			begin
				if (@iscasesensitive = 'Y')
				begin

					if exists (select 1 from serverprincipal where snapshotid = @snapshotid and CONVERT(varbinary(256), name)= CONVERT(varbinary(256), @user) and type = UPPER(@usertype))
						set @validuser = 'Y'

				end
				else
				begin
					if exists (select 1 from serverprincipal where snapshotid = @snapshotid and UPPER(name)= UPPER(@user) and type = UPPER(@usertype))
						set @validuser = 'Y'

				end

			end
			else
			begin
				if (@iscasesensitive = 'Y')
				begin

					if exists (select 1 from serverprincipal where snapshotid = @snapshotid and CONVERT(varbinary(256), name)= CONVERT(varbinary(256), @user) and type = 'S')
						set @validuser = 'Y'

				end
				else
				begin
					if exists (select 1 from serverprincipal where snapshotid = @snapshotid and UPPER(name)= UPPER(@user) and type = 'S')
						set @validuser = 'Y'

				end
	
			end
	
	
			if (@validuser = 'Y')
				begin

				--Server Level Permissions
				IF (LEN(@snapshotid) > 0)
				BEGIN
					IF(UPPER(@usertype) = 'W')
					BEGIN
						SELECT @sid = sid FROM windowsaccount WHERE snapshotid = @snapshotid and UPPER(name) = UPPER(@user)
					END
				END
	
				set @login = @user
		
				if (UPPER(@usertype) = 'W')
					set @logintype = 'Windows Account'
				else if (UPPER(@usertype) = 'A')
					set @logintype = 'Azure AD Account'
				else if (UPPER(@usertype) = 'E')		-- SQLsecure 3.1 (Anshul Aggarwal) - Azure AD User
					set @logintype = 'Azure AD User'
				else if (UPPER(@usertype) = 'X')		-- SQLsecure 3.1 (Anshul Aggarwal) - Azure AD Group
					set @logintype = 'Azure AD Group'
				else
					set @logintype = 'SQL Login'
			
		
				if (UPPER(@iscasesensitive) = 'Y')
				begin
					select @serveraccess = serveraccess, @disabled=[disabled] from serverprincipal where snapshotid = @snapshotid and CONVERT(VARBINARY, name)=CONVERT(VARBINARY, @login)
				end
				else
				begin
					select @serveraccess = serveraccess, @disabled=[disabled] from serverprincipal where snapshotid = @snapshotid and UPPER(name)=UPPER(@login)
				end
		
				delete from #tmproles
				set @roles = ''
		
				exec isp_sqlsecure_getfixedloginrole @snapshotid=@snapshotid, @logintype=@usertype, @inputsid=@sid, @sqllogin=@user
		
				if exists (select 1 from #tmproles)
				begin
					
					declare c1 cursor for select distinct rolename from #tmproles where rolename is not null and rolename <> ''
				
					open c1
			
					fetch next from c1 into @rolename

					set @count = 0
			
					while @@fetch_status = 0
					begin
						if (@count > 0)
						begin
							set @roles = @roles + ', ' + @rolename
						end
						else
						begin
							set @roles = @rolename
						end
	
						set @count = @count + 1
		
						fetch next from c1 into @rolename
					end
		
					close c1
					deallocate c1
		
				end			
		
				-- insert server info 
				insert into #tmpserverbase (connectionname, loginname, logintype, serveraccess, [disabled], [role]) values 
					(@connectionname, @login, @logintype, @serveraccess, @disabled, @roles)

				INSERT INTO #tmpserver
				SELECT connectionname, loginname, logintype, isnull(sp.serveraccess, m.serveraccess), 
					CASE isnull(sp.[disabled], m.[disabled]) WHEN 'N' THEN (CASE isnull(wa.enabled, 1) WHEN 0 THEN 'Y' ELSE 'N' END) ELSE 'Y' END,
					[role], isnull(gm.name, loginname)
				FROM #tmpserverbase m left join vwwindowsgroupmembers as gm
					on gm.snapshotid = @snapshotid
					AND @usertype = 'W'
					AND (gm.groupsid = @sid or gm.sid = @sid)
					left join serverprincipal as sp
					on sp.snapshotid = @snapshotid
					and (sp.sid=gm.sid)
					left join windowsaccount as wa
					on wa.snapshotid = gm.snapshotid
					and wa.sid=gm.sid

			end

			FETCH NEXT FROM cc INTO @snapshotid, @connectionname
		END
		CLOSE cc
		DEALLOCATE cc
	END
	ELSE
	BEGIN
		SELECT @snapshotid = snapshotid FROM dbo.getsnapshotlist(@rundate, @usebaseline) WHERE registeredserverid IN (SELECT registeredserverid FROM #tmpservers) AND UPPER(connectionname) LIKE UPPER(@server)
		
		select @iscasesensitive = casesensitivemode from serversnapshot where snapshotid = @snapshotid

		set @validuser = 'N'

		if (UPPER(@usertype) = 'W')
		begin
			if exists (select 1 from windowsaccount where snapshotid = @snapshotid and UPPER(name)= UPPER(@user))
				set @validuser = 'Y'

		end
		else if (UPPER(@usertype) = 'A')	-- SQLsecure 3.1 (Anshul Aggarwal) - Add support for Azure AD Users or Groups
		begin
			if (@iscasesensitive = 'Y')
			begin

				if exists (select 1 from serverprincipal where snapshotid = @snapshotid and CONVERT(varbinary(256), name)= CONVERT(varbinary(256), @user) and type in ('E', 'X'))
					set @validuser = 'Y'

			end
			else
			begin
				if exists (select 1 from serverprincipal where snapshotid = @snapshotid and UPPER(name)= UPPER(@user) and type in ('E', 'X'))
					set @validuser = 'Y'

			end

		end
		else if (UPPER(@usertype) = 'E' or UPPER(@usertype) = 'X')	-- SQLsecure 3.1 (Anshul Aggarwal) - Add support for Azure AD Users or Groups
		begin
			if (@iscasesensitive = 'Y')
			begin

				if exists (select 1 from serverprincipal where snapshotid = @snapshotid and CONVERT(varbinary(256), name)= CONVERT(varbinary(256), @user) and type = UPPER(@usertype))
					set @validuser = 'Y'

			end
			else
			begin
				if exists (select 1 from serverprincipal where snapshotid = @snapshotid and UPPER(name)= UPPER(@user) and type = UPPER(@usertype))
					set @validuser = 'Y'

			end

		end
		else
		begin
			if (@iscasesensitive = 'Y')
			begin

				if exists (select 1 from serverprincipal where snapshotid = @snapshotid and CONVERT(varbinary(256), name)= CONVERT(varbinary(256), @user) and type = 'S')
					set @validuser = 'Y'

			end
			else
			begin
				if exists (select 1 from serverprincipal where snapshotid = @snapshotid and UPPER(name)= UPPER(@user) and type = 'S')
					set @validuser = 'Y'

			end

		end

		if (@validuser = 'Y')
		begin

			--Server Level Permissions
			IF (LEN(@snapshotid) > 0)
			BEGIN
				IF(UPPER(@usertype) = 'W')
				BEGIN
					SELECT @sid = sid FROM windowsaccount WHERE snapshotid = @snapshotid and UPPER(name) = UPPER(@user)
				END
			END
	
			set @login = @user
	
			if (UPPER(@usertype) = 'W')
				set @logintype = 'Windows Account'
			else if (UPPER(@usertype) = 'A')
				set @logintype = 'Azure AD Account'		-- SQLsecure 3.1 (Anshul Aggarwal) - Azure AD User
			else if (UPPER(@usertype) = 'E')
				set @logintype = 'Azure AD User'		-- SQLsecure 3.1 (Anshul Aggarwal) - Azure AD User
			else if (UPPER(@usertype) = 'X')
				set @logintype = 'Azure AD Group'		-- SQLsecure 3.1 (Anshul Aggarwal) - Azure AD Group
			else
				set @logintype = 'SQL Login'
		
			select @iscasesensitive = casesensitivemode from serversnapshot where snapshotid = @snapshotid
	
			if (UPPER(@iscasesensitive) = 'Y')
			begin
				select @serveraccess = serveraccess, @disabled=[disabled] from serverprincipal where snapshotid = @snapshotid and CONVERT(VARBINARY, name)=CONVERT(VARBINARY, @login)
			end
			else
			begin
				select @serveraccess = serveraccess, @disabled=[disabled] from serverprincipal where snapshotid = @snapshotid and UPPER(name)=UPPER(@login)
			end

			delete from #tmproles
			set @roles = ''

			exec isp_sqlsecure_getfixedloginrole @snapshotid=@snapshotid, @logintype=@usertype, @inputsid=@sid, @sqllogin=@user

			if exists (select 1 from #tmproles)
			begin
				declare c1 cursor for select distinct rolename from #tmproles where rolename is not null and rolename <> ''
			
				open c1

				set @count = 0

				fetch next from c1 into @rolename
		
				while @@fetch_status = 0
				begin

					if (@count > 0)
					begin
						set @roles = @roles + ', ' + @rolename
					end
					else
					begin
						set @roles = @rolename
					end

					set @count = @count + 1
	
					fetch next from c1 into @rolename
				end
	
				close c1
				deallocate c1
	
			end			
	
			-- insert server info 
			insert into #tmpserverbase (connectionname, loginname, logintype, serveraccess, [disabled], [role]) values 
				(@server, @login, @logintype, @serveraccess, @disabled, @roles)

			INSERT INTO #tmpserver
			SELECT connectionname, loginname, logintype, isnull(sp.serveraccess, m.serveraccess), 
				CASE isnull(sp.[disabled], m.[disabled]) WHEN 'N' THEN (CASE isnull(wa.enabled, 1) WHEN 0 THEN 'Y' ELSE 'N' END) ELSE 'Y' END, 
				[role], isnull(gm.name, loginname)
			FROM #tmpserverbase m left join vwwindowsgroupmembers as gm
				on gm.snapshotid = @snapshotid
				AND @usertype = 'W'
				AND (gm.groupsid = @sid or gm.sid = @sid)
				left join serverprincipal as sp
				on sp.snapshotid = @snapshotid
				and sp.sid=gm.sid
				left join windowsaccount as wa
				on wa.snapshotid = gm.snapshotid
				and wa.sid=gm.sid
		end
	END

	-- display the top login info
	select distinct connectionname, loginname, logintype, serveraccess, [disabled], [role], [userlogin] from #tmpserver

	DROP TABLE #tmpserverpermission

	DROP TABLE #tmpservers
	DROP TABLE #tmpserverbase


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_report_getuserpermissions_user] TO [SQLSecureView]

GO
