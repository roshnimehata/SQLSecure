SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_report_getuserdatabasepermissions]'))
drop procedure [dbo].[isp_sqlsecure_report_getuserdatabasepermissions]
GO

CREATE PROCEDURE [dbo].[isp_sqlsecure_report_getuserdatabasepermissions]
	@rundate datetime = null,
	@user nvarchar(400),
	@server nvarchar(400),
	@permission nvarchar(1),
	@usertype nvarchar(1),
	@usebaseline bit = 0
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Generate data for Users Permissions Reports

	SET NOCOUNT ON
	DECLARE @snapshotid int
	DECLARE @sid varbinary(85)
	DECLARE @databasename nvarchar(200)
	declare @logintype nchar(1)
	declare @validuser nchar(1)
	declare @iscasesensitive nchar(1)

	IF EXISTS (SELECT name FROM sysobjects WHERE xtype='u' AND name='#tmpserverpermission')
	BEGIN
		DROP TABLE #tmpserverpermission
	END

	CREATE TABLE #tmpserverpermission (snapshotid int, permissionlevel nvarchar(15), logintype nchar(1), loginname nvarchar(256), connectionname nvarchar(400), databasename nvarchar(256), principalid int, principalname nvarchar(128), principaltype nchar(1), 
	databaseprincipal nvarchar(128), databaseprincipaltype nchar(1), grantor int, grantorname nvarchar(128), grantee int, granteename nvarchar(128), classid int, permissiontype nvarchar(4), coveringfrom nvarchar(15), permission nvarchar(256), isgrant nchar(1),
	isgrantwith nchar(1), isrevoke nchar(1), isdeny nchar(1), parentobjectid int, objectid int, objectname nvarchar(256), qualifiedname nvarchar(256), objecttype nvarchar(64), schemaid int, schemaname nvarchar(128), owner int, ownername nvarchar(128), isaliased nchar(1), inherited nchar(1), sourcename nvarchar(256), sourcetype nvarchar(256), sourcepermission nvarchar(256))

	--If the user wants information about ALL servers

	IF(@server = '%')
	BEGIN
		DECLARE @connectionname nvarchar(400)
		DECLARE c CURSOR FOR SELECT DISTINCT connectionname FROM serversnapshot
	
		OPEN c

		FETCH NEXT FROM c INTO @connectionname

		WHILE @@FETCH_STATUS = 0
		BEGIN
			SELECT @snapshotid = snapshotid FROM dbo.getsnapshotlist(@rundate, @usebaseline) WHERE upper(connectionname) = upper(@connectionname)

			select @iscasesensitive = casesensitivemode from serversnapshot where snapshotid = @snapshotid

			set @validuser = 'N'
	
			if (UPPER(@usertype) = 'W')
			begin
				if exists (select 1 from windowsaccount where snapshotid = @snapshotid and UPPER(name)= UPPER(@user))
					set @validuser = 'Y'
	
			end
			else if (UPPER(@usertype) = 'A') -- SQLsecure 3.1 (Anshul Aggarwal) - Azure AD Account
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
			else if (UPPER(@usertype) = 'E' or UPPER(@usertype) = 'X') -- SQLsecure 3.1 (Anshul Aggarwal) - Azure AD User or Group
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

				IF (LEN(@snapshotid) > 0)
				BEGIN
					-- Get Databases info
					DECLARE cdb CURSOR FOR SELECT databasename FROM sqldatabase WHERE snapshotid = @snapshotid
					OPEN cdb
					FETCH NEXT FROM cdb INTO @databasename
					WHILE @@FETCH_STATUS = 0
					BEGIN
						IF(@usertype = 'W')
						BEGIN
							SELECT @sid = sid FROM windowsaccount WHERE UPPER(name) = @user
							EXEC isp_sqlsecure_getuserpermission @snapshotid=@snapshotid, @logintype='W', @inputsid=@sid, @sqllogin='', @databasename=@databasename, @permissiontype=@permission
						END
						ELSE IF(@usertype = 'A')
						BEGIN
							declare @azurelogintype nchar(1)
							if (@iscasesensitive = 'Y')
							begin
								if exists (select 1 from serverprincipal where snapshotid = @snapshotid and CONVERT(varbinary(256), name)= CONVERT(varbinary(256), @user) and type = 'E')
									SET @azurelogintype = 'E'
								else if exists (select 1 from serverprincipal where snapshotid = @snapshotid and CONVERT(varbinary(256), name)= CONVERT(varbinary(256), @user) and type = 'X')
									SET @azurelogintype = 'X'
							end
							else
							begin
								if exists (select 1 from serverprincipal where snapshotid = @snapshotid and UPPER(name)= UPPER(@user) and type = 'E')
									SET @azurelogintype = 'E'
								else if exists (select 1 from serverprincipal where snapshotid = @snapshotid and UPPER(name)= UPPER(@user) and type = 'X')
									SET @azurelogintype = 'X'
							end

							IF(@azurelogintype = 'E' or @azurelogintype = 'X')
								EXEC isp_sqlsecure_getuserpermission @snapshotid=@snapshotid, @logintype=@azurelogintype ,@inputsid=null, @sqllogin=@user, @databasename=@databasename, @permissiontype=@permission
						END
						ELSE IF(@usertype = 'E' or @usertype = 'X')
						BEGIN
							EXEC isp_sqlsecure_getuserpermission @snapshotid=@snapshotid, @logintype=@usertype ,@inputsid=null, @sqllogin=@user, @databasename=@databasename, @permissiontype=@permission
						END
						ELSE
						BEGIN
							EXEC isp_sqlsecure_getuserpermission @snapshotid=@snapshotid, @logintype='S' ,@inputsid=null, @sqllogin=@user, @databasename=@databasename, @permissiontype=@permission
						END
	
						FETCH NEXT FROM c INTO @databasename
					END
	
					CLOSE cdb
					DEALLOCATE cdb
				END
				FETCH NEXT FROM c INTO @connectionname
			END
			CLOSE c
			DEALLOCATE c
		end
	END
	ELSE
	BEGIN
		SELECT @snapshotid = snapshotid FROM dbo.getsnapshotlist(@rundate, @usebaseline) WHERE upper(connectionname) = upper(@server)

		select @iscasesensitive = casesensitivemode from serversnapshot where snapshotid = @snapshotid

		set @validuser = 'N'

		if (UPPER(@usertype) = 'W')
		begin
			if exists (select 1 from windowsaccount where snapshotid = @snapshotid and UPPER(name)= UPPER(@user))
				set @validuser = 'Y'

		end
		else if (UPPER(@usertype) = 'A') -- SQLsecure 3.1 (Anshul Aggarwal) - Azure AD Account
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
		else if (UPPER(@usertype) = 'E' or UPPER(@usertype) = 'X') -- SQLsecure 3.1 (Anshul Aggarwal) - Azure AD User or Group
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
	
		delete from #tmpserverpermission

		if (@validuser = 'Y')
		begin

			IF (LEN(@snapshotid) > 0)
			BEGIN
				-- Get Databases info
				DECLARE c CURSOR FOR SELECT databasename FROM sqldatabase WHERE snapshotid = @snapshotid
				OPEN c
				FETCH NEXT FROM c INTO @databasename
				WHILE @@FETCH_STATUS = 0
				BEGIN
	
					IF(UPPER(@usertype) = 'W')
					BEGIN
						SELECT @sid = sid FROM windowsaccount WHERE snapshotid = @snapshotid and UPPER(name)= UPPER(@user)
						EXEC isp_sqlsecure_getuserpermission @snapshotid=@snapshotid, @logintype='W', @inputsid=@sid, @sqllogin='', @databasename=@databasename, @permissiontype=@permission
					END
					ELSE IF(UPPER(@usertype) = 'A') -- SQLsecure 3.1 (Anshul Aggarwal) - Azure AD Account
					BEGIN

						if (@iscasesensitive = 'Y')
						begin
							if exists (select 1 from serverprincipal where snapshotid = @snapshotid and CONVERT(varbinary(256), name)= CONVERT(varbinary(256), @user) and type = 'E')
								SET @azurelogintype = 'E'
							else if exists (select 1 from serverprincipal where snapshotid = @snapshotid and CONVERT(varbinary(256), name)= CONVERT(varbinary(256), @user) and type = 'X')
								SET @azurelogintype = 'X'
						end
						else
						begin
							if exists (select 1 from serverprincipal where snapshotid = @snapshotid and UPPER(name)= UPPER(@user) and type = 'E')
								SET @azurelogintype = 'E'
							else if exists (select 1 from serverprincipal where snapshotid = @snapshotid and UPPER(name)= UPPER(@user) and type = 'X')
								SET @azurelogintype = 'X'
						end

						IF(@azurelogintype = 'E' or @azurelogintype = 'X')
							EXEC isp_sqlsecure_getuserpermission @snapshotid=@snapshotid, @logintype= @azurelogintype ,@inputsid=NULL, @sqllogin=@user, @databasename=@databasename, @permissiontype=@permission
						
					END
					ELSE IF(UPPER(@usertype) = 'E' or UPPER(@usertype) = 'X') -- SQLsecure 3.1 (Anshul Aggarwal) - Azure AD User or Group
					BEGIN
						EXEC isp_sqlsecure_getuserpermission @snapshotid=@snapshotid, @logintype=@usertype ,@inputsid=NULL, @sqllogin=@user, @databasename=@databasename, @permissiontype=@permission
					END
					ELSE
					BEGIN
						EXEC isp_sqlsecure_getuserpermission @snapshotid=@snapshotid, @logintype='S' ,@inputsid=NULL, @sqllogin=@user, @databasename=@databasename, @permissiontype=@permission
					END
	
					FETCH NEXT FROM c INTO @databasename
				END
	
				CLOSE c
				DEALLOCATE c
			END
		end
	END

	SELECT DISTINCT 
		granteename, grantorname, permission, isgrant, isgrantwith, isdeny, qualifiedname, ownername, isaliased, objecttypename= dbo.getobjecttypename(objecttype), databasename
	FROM #tmpserverpermission 
	ORDER BY databasename, objecttypename, qualifiedname, permission

	DROP TABLE #tmpserverpermission

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_report_getuserdatabasepermissions] TO [SQLSecureView]

GO
