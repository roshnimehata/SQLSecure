DECLARE @ver INT;
SELECT
    @ver = schemaversion
FROM
    currentversion;
--IF ( ISNULL(@ver, 900) >= 3100 )	
    --BEGIN

        DECLARE
            @metricid INT ,
            @strval NVARCHAR(512)

        DECLARE @startmetricid INT


-- SQLsecure 3.1 (Anshul Aggarwal) - Add support for Azure SQL Database

		select @startmetricid = 1
		select @metricid = @startmetricid
		if not exists (select TOP 1 * from metricextendedinfo where metricid = @metricid)
		begin
			
			insert into metricextendedinfo(metricid, servertype, metricname, metricdescription, validvalues, valuedescription)
							values (@metricid, 'ADB', 'Audit Data Is Stale', 'Determine whether the nearest snapshot collection occurred within an acceptable timeframe from the selected date', '', 'When enabled, this check will identify a risk if audit data was not collected within the specified timeframe. Specify the number of days audit data is considered valid.')

			insert into policymetricextendedinfo (policyid, metricid, assessmentid, servertype, severity, severityvalues, reportkey, reporttext)
							values (0, @metricid, 0, 'ADB', 2, '''30''', '',
											'Was the most recent snapshot collected within an acceptable timeframe?')
		end

		select @metricid = 2
		if not exists (select TOP 1 * from metricextendedinfo where metricid = @metricid)
		begin

			insert into metricextendedinfo(metricid, servertype, metricname, metricdescription, validvalues, valuedescription)
							values (@metricid,  'ADB', 'Azure SQL Database Version', 'Determine whether the Azure SQL Database software is at an acceptable minimum version', '', 'When enabled, this check will identify a risk if the Azure SQL Database version is below the minimum acceptable level. Specify the minimum acceptable level for each Azure SQL Database.')

			insert into policymetricextendedinfo (policyid, metricid, assessmentid, servertype, severity, severityvalues, reportkey, reporttext)
							values (0, @metricid, 0, 'ADB', 1, '''12.0.2000.8''', '',
											'Is Azure SQL Database below the minimum acceptable version?')
		end

		select @metricid = 15
		if not exists (select TOP 1 * from metricextendedinfo where metricid = @metricid)
		begin

			insert into metricextendedinfo(metricid, servertype, metricname, metricdescription, validvalues, valuedescription)
							values (@metricid, 'ADB', 'Sample Databases Exist','Determine whether sample databases exist on the Azure SQL Database', '', 'When enabled, this check will identify a risk if any sample databases exist on the Azure SQL Database. Specify the sample databases.')

			insert into policymetricextendedinfo (policyid, metricid, assessmentid, servertype, severity, severityvalues, reportkey, reporttext)
							values (0, @metricid, 0, 'ADB', 1, '''Northwind'',''pubs'',''AdventureWorks'',''AdventureWorksAS'',''AdventureWorksDW''', '',
											'Do the Azure SQL Database sample databases exist?')

		end

		select @metricid = 22
		if not exists (select TOP 1 * from metricextendedinfo where metricid = @metricid)
		begin

			insert into metricextendedinfo(metricid, servertype, metricname, metricdescription, validvalues, valuedescription)
							values (@metricid, 'ADB', 'Stored Procedures Encrypted', 'Determine whether user stored procedures are encrypted on the Azure SQL Database', '', 'When enabled, this check will identify a risk if any user stored procedures are not encrypted on the Azure SQL Database.')

			insert into policymetricextendedinfo (policyid, metricid, assessmentid, servertype, severity, severityvalues, reportkey, reporttext)
							values (0, @metricid, 0, 'ADB', 1, '', '',
											'Are any user stored procedures not encrypted?')	
		end
		
		select @metricid = 54
		if not exists (select TOP 1 * from metricextendedinfo where metricid = @metricid)
		begin			

			insert into metricextendedinfo(metricid, servertype, metricname, metricdescription, validvalues, valuedescription)
							values (@metricid, 'ADB', 'Snapshot Not Found', 'Determine whether all servers in the policy have valid audit data for the selected timeframe', '', 'When enabled, this check will identify a risk if audit data is missing.')
			insert into policymetricextendedinfo (policyid, metricid, assessmentid, servertype, severity, severityvalues, reportkey, reporttext)
							values (0, @metricid, 0, 'ADB', 3, '', '',
											'Are any servers in the policy missing audit data?')
		end

		select @metricid = 55
		if not exists (select TOP 1 * from metricextendedinfo where metricid = @metricid)
		begin

			insert into metricextendedinfo(metricid, servertype, metricname, metricdescription, validvalues, valuedescription)
							values (@metricid, 'ADB', 'Snapshot May Be Missing Data', 'Determine whether all audit data for the selected servers is complete and without warnings', '', 'When enabled, this check will identify a risk if audit data is incomplete or the snapshot returned warnings.')
			insert into policymetricextendedinfo (policyid, metricid, assessmentid, servertype, severity, severityvalues, reportkey, reporttext)
							values (0, @metricid, 0, 'ADB', 2, '', '',
											'Is audit data incomplete?')
		end

		select @metricid = 56
		if not exists (select TOP 1 * from metricextendedinfo where metricid = @metricid)
		begin

			insert into metricextendedinfo(metricid, servertype, metricname, metricdescription, validvalues, valuedescription)
							values (@metricid, 'ADB', 'Baseline Data Not Being Used', 'Determine whether all audit data for the selected timeframe is from baseline snapshots', '', 'When enabled, this check will identify a risk if audit data is not from baseline snapshot.')

			insert into policymetricextendedinfo (policyid, metricid, assessmentid, servertype, severity, severityvalues, reportkey, reporttext)
							values (0, @metricid, 0, 'ADB', 2, '', '',
											'Is any audit data from a non-baseline snapshot?')
		end

		select @metricid = 58
		if not exists (select TOP 1 * from metricextendedinfo where metricid = @metricid)
		begin

			insert into metricextendedinfo(metricid, servertype, metricname, metricdescription, validvalues, valuedescription)
							values (@metricid, 'ADB', 'Public Database Role Has Permissions', 'Determine whether the public database role has any permissions', '','When enabled, this check will identify a risk if the public database role has been granted any permissions or been made a member of any other role.')

			insert into policymetricextendedinfo (policyid, metricid, assessmentid, servertype, severity, severityvalues, reportkey, reporttext)
							values (0, @metricid, 0, 'ADB', 3, '', '',
											'Are any permissions granted to the public database role?')		
		end
		
		select @metricid = 76
		if not exists (select TOP 1 * from metricextendedinfo where metricid = @metricid)
		begin
							
			insert into metricextendedinfo(metricid, servertype, metricname, metricdescription, validvalues, valuedescription)
						values (@metricid, 'ADB', 'Required Administrative Accounts Do Not Exist', 'Determine whether the required administrative accounts exist on the Azure SQL Database', '', 'When enabled, this check will identify a risk if any required administrative accounts are missing from the Azure SQL Database. Specify the required accounts.')

			insert into policymetricextendedinfo (policyid, metricid, assessmentid, servertype, severity, severityvalues, reportkey, reporttext)
							values (0, @metricid, 0, 'ADB', 1, '''none''', '',
											'Do required administrative accounts exist?')
		end
			
		select @metricid = 86
		if not exists (select TOP 1 * from metricextendedinfo where metricid = @metricid)
		begin
						
			insert into metricextendedinfo(metricid, servertype, metricname, metricdescription, validvalues, valuedescription)
						values (@metricid, 'ADB','Databases Are Trustworthy', 'Determine whether any unapproved databases are trustworthy on Azure SQL Database', '', 'When enabled, this check will identify a risk if any unapproved databases are trustworthy on Azure SQL Database. Specify the approved databases.')
			insert into policymetricextendedinfo (policyid, metricid, assessmentid, servertype, severity, severityvalues, reportkey, reporttext)
						values (0, @metricid, 0,  'ADB', 2, '''none''', '',
										'Is the trustworthy bit on for any unapproved databases?')
		end
		
		
		select @metricid = 87
		if not exists (select TOP 1 * from metricextendedinfo where metricid = @metricid)
		begin
			insert into metricextendedinfo(metricid, servertype, metricname, metricdescription, validvalues, valuedescription)
							values (@metricid, 'ADB', 'Sysadmins Own Databases', 'Determine whether any databases are owned by a system administrator', '', 'When enabled, this check will identify a risk if any unapproved databases are owned by system administrators. Specify the approved databases.')

			insert into policymetricextendedinfo (policyid, metricid, assessmentid, servertype, severity, severityvalues, reportkey, reporttext)
							values (0, @metricid, 0, 'ADB', 2, '''master''', '',
											'Are any unapproved databases owned by a system administrator?')
		end

		select @metricid = 88
		if not exists (select TOP 1 * from metricextendedinfo where metricid = @metricid)
		begin
			insert into metricextendedinfo(metricid, servertype, metricname, metricdescription, validvalues, valuedescription)
							values (@metricid, 'ADB', 'Sysadmins Own Trustworthy Databases', 'Determine whether any trustworthy databases are owned by system administrators on Azure SQL Database', '', 'When enabled, this check will identify a risk if any unapproved databases have the trustworthy bit set on and the owner has system administrator privileges on Azure SQL Database. Specify the approved databases.')

			insert into policymetricextendedinfo (policyid, metricid, assessmentid, servertype, severity, severityvalues, reportkey, reporttext)
							values (0, @metricid, 0, 'ADB', 3, '''none''', '',
											'Are any unapproved trustworthy databases owned by a system administrator?')
		end

		select @metricid = 89
		if not exists (select TOP 1 * from metricextendedinfo where metricid = @metricid)
		begin
			insert into metricextendedinfo(metricid, servertype, metricname, metricdescription, validvalues, valuedescription)
							values (@metricid, 'ADB', 'Public Role Has Permissions on User Database Objects', 'Determine whether the public database role has been granted permissions on user database objects.', '', 'When enabled, this check will identify a risk if the public database role has been granted permissions on any user objects within a user database. Specify the approved databases.')
			insert into policymetricextendedinfo (policyid, metricid, assessmentid, servertype, severity, severityvalues, reportkey, reporttext)
							values (0, @metricid, 0, 'ADB', 3, '', '',
											'Has the public database role been granted permissions on user database objects?')
		end

		
		select @metricid = 92
		if not exists (select TOP 1 * from metricextendedinfo where metricid = @metricid)
		begin
			insert into metricextendedinfo(metricid, servertype, metricname, metricdescription, validvalues, valuedescription)
							values (@metricid, 'ADB', 'Weak Passwords','Determine whether any SQL login passwords match the login name or a list of common and restricted passwords.', '', 'When enabled, this check will identify a risk if a SQL login on the target instance has a weak password. Specify which SQL logins should not be checked.')
			insert into policymetricextendedinfo (policyid, metricid, assessmentid, servertype, severity, severityvalues, reportkey, reporttext)
							values (0, @metricid, 0, 'ADB', 3, '', '', 'Does this SQL login have a weak password?')
		end

		select @metricid = 93
		if not exists (select TOP 1 * from metricextendedinfo where metricid = @metricid)
		begin
			insert into metricextendedinfo(metricid, servertype, metricname, metricdescription, validvalues, valuedescription)
			values (
					 @metricid
					,N'ADB'
					,N'Symmetric key'
					,N'Determine whether master have user-created symmetric keys'
					,N''
					,N'When enabled, this check will identify a risk if master have user-created symmetric keys'
					)

			insert into policymetricextendedinfo (policyid, metricid, assessmentid, servertype, severity, severityvalues, reportkey, reporttext)
			values (
					 0
					,@metricid
					,0
					,N'ADB'
					,3
					,N''
					,N''
					,N'Does master have user-created symmetric keys?')
		end

		
		select @metricid = 102
		if not exists (select TOP 1 * from metricextendedinfo where metricid = @metricid)
		begin
 
				 insert into dbo.metricextendedinfo
						(
						  metricid,
						  servertype,
						  metricname,
						  metricdescription,
						  validvalues,
						  valuedescription
						)
				values
						(
						  @metricid, -- metricid - int
						  N'ADB', -- servertype nvarchar(3)
						  N'Unacceptable Database Ownership', -- metricname - nvarchar(256)
						  N'Determine whether if a database is found with an unacceptable owner', -- metricdescription - nvarchar(1024)
						  N'', -- validvalues - nvarchar(1024)
						  N'When enabled, this check will identify a risk if a database is found with an unacceptable owner.'  -- valuedescription - nvarchar(1024)
						)

				insert into dbo.policymetricextendedinfo
						(
						  policyid,
						  metricid,
						  assessmentid,
						  servertype,
						  reportkey,
						  reporttext,
						  severity,
						  severityvalues
						)
				values
						(
						  0, -- policyid - int
						  @metricid, -- metricid - int,
						  0 , -- assessmentid - int,
						  N'ADB', -- servertype nvarchar(3)
						  N'', -- reportkey - nvarchar(32)
						  N'Are there  databases with an unacceptable owner?', -- reporttext - nvarchar(4000)
						  1, -- severity - int
						  N'none' -- severityvalues - nvarchar(4000)
						)
			end   

			select @metricid = 103
			if not exists (select TOP 1 * from metricextendedinfo where metricid = @metricid)
			begin
					insert into dbo.metricextendedinfo
					(
						metricid,
						servertype,
						metricname,
						metricdescription,
						validvalues,
						valuedescription
					)
					values
						(
							@metricid, -- metricid - int
							N'ADB', -- servertype - nvarchar(3)
							N'Public role permissions', -- metricname - nvarchar(256)
							N'Determine whether the public roles have permissions to user defined objects', -- metricdescription - nvarchar(1024)
							N'', -- validvalues - nvarchar(1024)
							N'When enabled, this check will identify a risk if permissions have been granted to the public roles.'  -- valuedescription - nvarchar(1024)									        
						)
					insert into dbo.policymetricextendedinfo
					(
						policyid,
						metricid,
						assessmentid,
						servertype,
						reportkey,
						reporttext,
						severity,
						severityvalues
					)
					values
						(
							0, -- policyid - int
							@metricid, -- metricid - int
							0,  -- assessmentid - int
							N'ADB', -- servertype - nvarchar(3)
							N'', -- reportkey - nvarchar(32)
							N'Is there objects with permissions granted to public role?', -- reporttext - nvarchar(4000)
							1, -- severity - int
							N'' -- severityvalues - nvarchar(4000)
						)
			end

		
        SELECT @metricid = 113;
       	if not exists (select TOP 1 * from metricextendedinfo where metricid = @metricid)
        BEGIN  
            insert into dbo.metricextendedinfo
					(
						metricid,
						servertype,
						metricname,
						metricdescription,
						validvalues,
						valuedescription
					)
            VALUES
                    (
                        @metricid , -- metricid - int
                        N'ADB', -- servertype - nvarchar(3)
                        N'Encryption Methods' , -- metricname - nvarchar(256)
                        N'Determine whether there are encryption keys with algorithms other than supported.' , -- metricdescription - nvarchar(1024)
                        N'''AES_128'',''AES_192'',''AES_256''' , -- validvalues - nvarchar(1024)
                        N'When enabled, this check will identify a risk if there are encryption keys with encryption methods other than selected.'  -- valuedescription - nvarchar(1024)									        
                    );
		
            insert into dbo.policymetricextendedinfo
					(
						policyid,
						metricid,
						assessmentid,
						servertype,
						reportkey,
						reporttext,
						severity,
						severityvalues
					)
            VALUES
                    (
                        0 , -- policyid - int
                        @metricid , -- metricid - int
						0,  -- assessmentid - int
						N'ADB', -- servertype - nvarchar(3)
                        N'' , -- reportkey - nvarchar(32)
                        N'Is there any encryption keys with algorithms other than selected?' , -- reporttext - nvarchar(4000)
                        3 , -- severity - int
                        N'''AES_128'',''AES_192'',''AES_256'''  -- severityvalues - nvarchar(4000)
                    );
        END;

	--Certificates check
        SELECT @metricid = 114
      	if not exists (select TOP 1 * from metricextendedinfo where metricid = @metricid)
        BEGIN
            insert into dbo.metricextendedinfo
					(
						metricid,
						servertype,
						metricname,
						metricdescription,
						validvalues,
						valuedescription
					)
            VALUES
                    (
                        @metricid ,
						N'ADB', -- servertype - nvarchar(3)
                        'Certificate private keys were never exported' ,
                        'Determine whether certificate private keys were not exported' ,
                        N'' ,
                        'When enabled, this check will identify a risk if certificate private keys were not exported.' 
                    )


            insert into dbo.policymetricextendedinfo
					(
						policyid,
						metricid,
						assessmentid,
						servertype,
						reportkey,
						reporttext,
						severity,
						severityvalues
					)
            VALUES
                    (
                        0 , --policyid
						@metricid ,
                        0 ,--assesmentid
                        N'ADB', -- servertype - nvarchar(3)
						N'' ,--reportkey
                        N'Does certificate private keys were exported?', --reporttext
                        3 , --severity
                        N''  --severityvalues
                    )
        END

			
--------------------- Update Policy Metrics for server type Azure SQL Database --------------------------------------------------------------------------------------------------------

		IF (NOT EXISTS ( SELECT  TOP 1 * FROM policymetricextendedinfo where policyid <> 0 and metricid between @startmetricid AND @metricid))
		BEGIN
			INSERT INTO policymetricextendedinfo
			SELECT pm.policyid, pm.metricid, pm.assessmentid, 'ADB', pme.reportkey, pme.reporttext, pm.severity, pme.severityvalues
			FROM policymetric pm INNER JOIN policymetricextendedinfo pme ON pm.metricid = pme.metricid
			WHERE pm.policyid <> 0 AND 
			pme.policyid = 0 AND pme.assessmentid = 0 AND
		    pm.metricid IN (
			1,
			2,
			15,
			22,
			54,
			55,
			56,
			58,
			76,
			86,
			87,
			88,
			89,
			92,
			93,
			102,
			103,
			113,
			114
			)
		END

-- SQLsecure 3.1 (Anshul Aggarwal) - New Risk Assessments
        SELECT @startmetricid = 117

-----Always Encrypted
        SELECT
            @metricid = 117;
        IF NOT EXISTS ( SELECT
                            TOP 1 * 
                        FROM
                            metric
                        WHERE
                            metricid = @metricid )
        BEGIN  
            INSERT  INTO dbo.metric
                    (
                        metricid ,
                        metrictype ,
                        metricname ,
                        metricdescription ,
                        isuserentered ,
                        ismultiselect ,
                        validvalues ,
                        valuedescription
		            )
            VALUES
                    (
                        @metricid , -- metricid - int
                        N'Access' , -- metrictype - nvarchar(32)
                        N'Always Encrypted' , -- metricname - nvarchar(256)
                        N'Determine whether always encryption is configured for specified columns on SQL Server 2016 or later' , -- metricdescription - nvarchar(1024)
                        1 , -- isuserentered - bit
                        1 , -- ismultiselect - bit
                        N'' , -- validvalues - nvarchar(1024)
                        N'When enabled, this check will identify a risk if always encryption is not configured for specified columns on SQL Server 2016 or later. Please specify in [Server].[Database].[Schema].[Table].[Column] format.'  -- valuedescription - nvarchar(1024)							        
                    );
		
            INSERT  INTO dbo.policymetric
                    (
                        policyid ,
                        metricid ,
                        isenabled ,
                        reportkey ,
                        reporttext ,
                        severity ,
                        severityvalues ,
                        assessmentid
		            )
            VALUES
                    (
                        0 , -- policyid - int
                        @metricid , -- metricid - int
                        0 , -- isenabled - bit
                        N'' , -- reportkey - nvarchar(32)
                        N'Are any databases using always encryption to protect sensitive data on SQL Server 2016 or later?' , -- reporttext - nvarchar(4000)
                        3 , -- severity - int
                        N'' , -- severityvalues - nvarchar(4000)
                        0  -- assessmentid - int
                    );

			INSERT  INTO dbo.metricextendedinfo
				(
					metricid,
					servertype,
					metricname,
					metricdescription,
					validvalues,
					valuedescription
				)
            VALUES
                    (
                        @metricid , -- metricid - int
                        N'ADB', -- servertype - nvarchar(3)
                        N'Always Encrypted' , -- metricname - nvarchar(256)
                        N'Determine whether always encryption is configured for specified columns on Azure SQL Database' , -- metricdescription - nvarchar(1024)
                        N'' , -- validvalues - nvarchar(1024)
                        N'When enabled, this check will identify a risk if always encryption is not configured for specified columns on Azure SQL Database. Please specify in [Server].[Database].[Schema].[Table].[Column] format.'  -- valuedescription - nvarchar(1024)							        
                    );
		
            INSERT  INTO dbo.policymetricextendedinfo
				(
					policyid,
					metricid,
					assessmentid,
					servertype,
					reportkey,
					reporttext,
					severity,
					severityvalues
				)
            VALUES
                    (
                        0 , -- policyid - int
                        @metricid , -- metricid - int
                        0 , -- assessmentid - int,
						N'ADB', -- servertype - nvarchar(3)
                        N'' , -- reportkey - nvarchar(32)
                        N'Are any databases using always encryption to protect sensitive data on Azure SQL Database?' , -- reporttext - nvarchar(4000)
                        3 , -- severity - int
                        N''  -- severityvalues - nvarchar(4000)
                    );

        END;

-----Transparent Data Encryption
        SELECT
            @metricid = 118;
        IF NOT EXISTS ( SELECT
                            TOP 1 * 
                        FROM
                            metric
                        WHERE
                            metricid = @metricid )
        BEGIN  
            INSERT  INTO dbo.metric
                    (
                        metricid ,
                        metrictype ,
                        metricname ,
                        metricdescription ,
                        isuserentered ,
                        ismultiselect ,
                        validvalues ,
                        valuedescription
		            )
            VALUES
                    (
                        @metricid , -- metricid - int
                        N'Access' , -- metrictype - nvarchar(32)
                        N'Transparent Data Encryption' , -- metricname - nvarchar(256)
                        N'Determine whether transparent data encryption is configured for any databases on SQL Server 2008 or later' , -- metricdescription - nvarchar(1024)
                        1 , -- isuserentered - bit
                        1 , -- ismultiselect - bit
                        N'' , -- validvalues - nvarchar(1024)
                        N'When enabled, this check will identify a risk if transparent data encryption is not configured for any databases on SQL Server 2008 or later. Please specify in format [SERVER].[DATABASE] for DBs to be excluded or none.'  -- valuedescription - nvarchar(1024)									        
                    );
		
            INSERT  INTO dbo.policymetric
                    (
                        policyid ,
                        metricid ,
                        isenabled ,
                        reportkey ,
                        reporttext ,
                        severity ,
                        severityvalues ,
                        assessmentid
		            )
            VALUES
                    (
                        0 , -- policyid - int
                        @metricid , -- metricid - int
                        1 , -- isenabled - bit
                        N'' , -- reportkey - nvarchar(32)
                        N'Is transparent data encryption enabled at the database level on SQL Server 2008 or later?' , -- reporttext - nvarchar(4000)
                        3 , -- severity - int
                        N'''none''' , -- severityvalues - nvarchar(4000)
                        0  -- assessmentid - int
                    );

				INSERT  INTO dbo.metricextendedinfo
				(
					metricid,
					servertype,
					metricname,
					metricdescription,
					validvalues,
					valuedescription
				)
            VALUES
                    (
                        @metricid , -- metricid - int
                        N'ADB', -- servertype - nvarchar(3)
                        N'Transparent Data Encryption' , -- metricname - nvarchar(256)
                        N'Determine whether transparent data encryption is configured for any databases on Azure SQL Database' , -- metricdescription - nvarchar(1024)
                        N'' , -- validvalues - nvarchar(1024)
                        N'When enabled, this check will identify a risk if transparent data encryption is not configured for any databases on Azure SQL Database. Please specify in format [SERVER].[DATABASE] for DBs to be excluded or none.'  -- valuedescription - nvarchar(1024)									        
                    );
		
            INSERT  INTO dbo.policymetricextendedinfo
				(
					policyid,
					metricid,
					assessmentid,
					servertype,
					reportkey,
					reporttext,
					severity,
					severityvalues
				)
            VALUES
                    (
                        0 , -- policyid - int
                        @metricid , -- metricid - int
                        0 , -- assessmentid - int,
						N'ADB', -- servertype - nvarchar(3)
                        N'' , -- reportkey - nvarchar(32)
                        N'Is transparent data encryption enabled at the database level on Azure SQL Database?' , -- reporttext - nvarchar(4000)
                        3 , -- severity - int
                        N'''none'''  -- severityvalues - nvarchar(4000)
                    );
        END;

----- Native Backup Encryption
        SELECT
            @metricid = 119;
        IF NOT EXISTS ( SELECT
                            TOP 1 * 
                        FROM
                            metric
                        WHERE
                            metricid = @metricid )
        BEGIN  
            INSERT  INTO dbo.metric
                    (
                        metricid ,
                        metrictype ,
                        metricname ,
                        metricdescription ,
                        isuserentered ,
                        ismultiselect ,
                        validvalues ,
                        valuedescription
		            )
            VALUES
                    (
                        @metricid , -- metricid - int
                        N'Access' , -- metrictype - nvarchar(32)
                        N'Backup Encryption' , -- metricname - nvarchar(256)
                        N'Determine whether native backup encryption was configured on SQL Server 2014 or later' , -- metricdescription - nvarchar(1024)
                        0 , -- isuserentered - bit
                        0 , -- ismultiselect - bit
                        N'' , -- validvalues - nvarchar(1024)
                        N'When enabled, this check will identify a risk if native backup encryption was not configured on SQL Server 2014 or later.'  -- valuedescription - nvarchar(1024)	
                    );
		
            INSERT  INTO dbo.policymetric
                    (
                        policyid ,
                        metricid ,
                        isenabled ,
                        reportkey ,
                        reporttext ,
                        severity ,
                        severityvalues ,
                        assessmentid
		            )
            VALUES
                    (
                        0 , -- policyid - int
                        @metricid , -- metricid - int
                        1 , -- isenabled - bit
                        N'' , -- reportkey - nvarchar(32)
                        N'Are any native backups configured for encryption on SQL Server 2014 or later?' , -- reporttext - nvarchar(4000)
                        3 , -- severity - int
                        N'''master'',''msdb'',''model'',''tempdb''' , -- severityvalues - nvarchar(4000)
                        0  -- assessmentid - int
                    );
        END;

-----Row-Level Security
        SELECT
            @metricid = 120;
        IF NOT EXISTS ( SELECT
                            TOP 1 * 
                        FROM
                            metric
                        WHERE
                            metricid = @metricid )
        BEGIN  
            INSERT  INTO dbo.metric
                    (
                        metricid ,
                        metrictype ,
                        metricname ,
                        metricdescription ,
                        isuserentered ,
                        ismultiselect ,
                        validvalues ,
                        valuedescription
		            )
            VALUES
                    (
                        @metricid , -- metricid - int
                        N'Access' , -- metrictype - nvarchar(32)
                        N'Row-Level Security' , -- metricname - nvarchar(256)
                        N'Determine whether row-level security is configured for specified databases on SQL Server 2016 or later' , -- metricdescription - nvarchar(1024)
                        1 , -- isuserentered - bit
                        1 , -- ismultiselect - bit
                        N'' , -- validvalues - nvarchar(1024)
                        N'When enabled, this check will identify a risk if row-level security is not configured for specified databases on SQL Server 2016 or later. Please specify in [Server].[Database].[Schema].[Table] format.'  -- valuedescription - nvarchar(1024)							        
                    );
		
            INSERT  INTO dbo.policymetric
                    (
                        policyid ,
                        metricid ,
                        isenabled ,
                        reportkey ,
                        reporttext ,
                        severity ,
                        severityvalues ,
                        assessmentid
		            )
            VALUES
                    (
                        0 , -- policyid - int
                        @metricid , -- metricid - int
                        0 , -- isenabled - bit
                        N'' , -- reportkey - nvarchar(32)
                        N'Are specified databases using row-level security on SQL Server 2016 or later?' , -- reporttext - nvarchar(4000)
                        3 , -- severity - int
                        N'' , -- severityvalues - nvarchar(4000)
                        0  -- assessmentid - int
                    );

			INSERT  INTO dbo.metricextendedinfo
				(
					metricid,
					servertype,
					metricname,
					metricdescription,
					validvalues,
					valuedescription
				)
            VALUES
                    (
                        @metricid , -- metricid - int
                        N'ADB', -- servertype - nvarchar(3)
						N'Row-Level Security' , -- metricname - nvarchar(256)
                        N'Determine whether row-level security is configured for specified databases on Azure SQL Database' , -- metricdescription - nvarchar(1024)
                        N'' , -- validvalues - nvarchar(1024)
                        N'When enabled, this check will identify a risk if row-level security is not configured for specified databases on Azure SQL Database. Please specify in [Server].[Database].[Schema].[Table] format.'  -- valuedescription - nvarchar(1024)							        								        
                    );
		
            INSERT  INTO dbo.policymetricextendedinfo
				(
					policyid,
					metricid,
					assessmentid,
					servertype,
					reportkey,
					reporttext,
					severity,
					severityvalues
				)
            VALUES
                    (
                        0 , -- policyid - int
                        @metricid , -- metricid - int
                        0 , -- assessmentid - int,
						N'ADB', -- servertype - nvarchar(3)
						N'' , -- reportkey - nvarchar(32)
                        N'Are specified databases using row-level security on Azure SQL Database?' , -- reporttext - nvarchar(4000)
                        3 , -- severity - int
                        N''  -- severityvalues - nvarchar(4000)
                    );
        END;


-----Dynamic Data Masking
        SELECT
            @metricid = 121;
        IF NOT EXISTS ( SELECT
                            TOP 1 * 
                        FROM
                            metric
                        WHERE
                            metricid = @metricid )
        BEGIN  
            INSERT  INTO dbo.metric
                    (
                        metricid ,
                        metrictype ,
                        metricname ,
                        metricdescription ,
                        isuserentered ,
                        ismultiselect ,
                        validvalues ,
                        valuedescription
		            )
            VALUES
                    (
                        @metricid , -- metricid - int
                        N'Access' , -- metrictype - nvarchar(32)
                        N'Dynamic Data Masking' , -- metricname - nvarchar(256)
                        N'Determine whether dynamic data masking is configured for specified databases on SQL Server 2016 or later' , -- metricdescription - nvarchar(1024)
                        1 , -- isuserentered - bit
                        1 , -- ismultiselect - bit
                        N'' , -- validvalues - nvarchar(1024)
                        N'When enabled, this check will identify a risk if dynamic data masking is not configured for specified databases on SQL Server 2016 or later. Please specify in [Server].[Database].[Schema].[Table].[Column] format.'  -- valuedescription - nvarchar(1024)								        
                    );
		
            INSERT  INTO dbo.policymetric
                    (
                        policyid ,
                        metricid ,
                        isenabled ,
                        reportkey ,
                        reporttext ,
                        severity ,
                        severityvalues ,
                        assessmentid
		            )
            VALUES
                    (
                        0 , -- policyid - int
                        @metricid , -- metricid - int
                        0 , -- isenabled - bit
                        N'' , -- reportkey - nvarchar(32)
                        N'Are specified databases using dynamic data masking on SQL Server 2016 or later?' , -- reporttext - nvarchar(4000)
                        2 , -- severity - int
                        N'' , -- severityvalues - nvarchar(4000)
                        0  -- assessmentid - int
                    );
				
			INSERT  INTO dbo.metricextendedinfo
				(
					metricid,
					servertype,
					metricname,
					metricdescription,
					validvalues,
					valuedescription
				)
            VALUES
                    (
                        @metricid , -- metricid - int
                        N'ADB', -- servertype - nvarchar(3)
						N'Dynamic Data Masking' , -- metricname - nvarchar(256)
                        N'Determine whether dynamic data masking is configured for specified databases on Azure SQL Database' , -- metricdescription - nvarchar(1024)
                        N'' , -- validvalues - nvarchar(1024)
                        N'When enabled, this check will identify a risk if dynamic data masking is not configured for specified databases on Azure SQL Database. Please specify in [Server].[Database].[Schema].[Table].[Column] format.'  -- valuedescription - nvarchar(1024)							        								        
                    );
		
            INSERT  INTO dbo.policymetricextendedinfo
				(
					policyid,
					metricid,
					assessmentid,
					servertype,
					reportkey,
					reporttext,
					severity,
					severityvalues
				)
            VALUES
                    (
                        0 , -- policyid - int
                        @metricid , -- metricid - int
                        0 , -- assessmentid - int,
						N'ADB', -- servertype - nvarchar(3)
						N'' , -- reportkey - nvarchar(32)
                        N'Are specified databases using dynamic data masking on Azure SQL Database?' , -- reporttext - nvarchar(4000)
                        2 , -- severity - int
                        N'' -- severityvalues - nvarchar(4000)
                    );

        END;


-----Signed Objects
        SELECT
            @metricid = 122;
        IF NOT EXISTS ( SELECT
                            TOP 1 * 
                        FROM
                            metric
                        WHERE
                            metricid = @metricid )
        BEGIN  
            INSERT  INTO dbo.metric
                    (
                        metricid ,
                        metrictype ,
                        metricname ,
                        metricdescription ,
                        isuserentered ,
                        ismultiselect ,
                        validvalues ,
                        valuedescription
		            )
            VALUES
                    (
                        @metricid , -- metricid - int
                        N'Access' , -- metrictype - nvarchar(32)
                        N'Signed Objects' , -- metricname - nvarchar(256)
                        N'Determine whether a digital signature has been added to sepcified stored procedure, function, assembly or trigger on SQL Server 2008 or later' , -- metricdescription - nvarchar(1024)
                        1 , -- isuserentered - bit
                        1 , -- ismultiselect - bit
                        N'' , -- validvalues - nvarchar(1024)
                        N'When enabled, this check will identify a risk if digital signature has not been added to specified stored procedure, function, assembly or trigger on SQL Server 2008 or later. Please specify in [Server].[Database].[Schema].[Object] format for stored procedure, function, trigger or [Server].[Database].[Assembly] for assembly.'  -- valuedescription - nvarchar(1024)	
                    );
		
            INSERT  INTO dbo.policymetric
                    (
                        policyid ,
                        metricid ,
                        isenabled ,
                        reportkey ,
                        reporttext ,
                        severity ,
                        severityvalues ,
                        assessmentid
		            )
            VALUES
                    (
                        0 , -- policyid - int
                        @metricid , -- metricid - int
                        0 , -- isenabled - bit
                        N'' , -- reportkey - nvarchar(32)
                        N'Are digital signatures present for specified stored procedures, functions, assemblies or triggers on SQL Server 2008 or later?' , -- reporttext - nvarchar(4000)
                        1 , -- severity - int
                        N'' , -- severityvalues - nvarchar(4000)
                        0  -- assessmentid - int
                    );
        END;

-----Server-level Firewall Rules
        SELECT
            @metricid = 123;
        IF NOT EXISTS ( SELECT
                            TOP 1 * 
                        FROM
                            metric
                        WHERE
                            metricid = @metricid )
        BEGIN  
            INSERT  INTO dbo.metric
                    (
                        metricid ,
                        metrictype ,
                        metricname ,
                        metricdescription ,
                        isuserentered ,
                        ismultiselect ,
                        validvalues ,
                        valuedescription
		            )
            VALUES
                    (
                        @metricid , -- metricid - int
                        N'Configuration' , -- metrictype - nvarchar(32)
                        N'NA' , -- metricname - nvarchar(256)
                        N'NA' , -- metricdescription - nvarchar(1024)
                        1 , -- isuserentered - bit
                        1 , -- ismultiselect - bit
                        N'NA' , -- validvalues - nvarchar(1024)
                        N'NA.'  -- valuedescription - nvarchar(1024)								        
                    );
		
            INSERT  INTO dbo.policymetric
                    (
                        policyid ,
                        metricid ,
                        isenabled ,
                        reportkey ,
                        reporttext ,
                        severity ,
                        severityvalues ,
                        assessmentid
		            )
            VALUES
                    (
                        0 , -- policyid - int
                        @metricid , -- metricid - int
                        0 , -- isenabled - bit
                        N'NA' , -- reportkey - nvarchar(32)
                        N'NA' , -- reporttext - nvarchar(4000)
                        0 , -- severity - int
                        N'NA' , -- severityvalues - nvarchar(4000)
                        0  -- assessmentid - int
                    );

				
				INSERT  INTO dbo.metricextendedinfo
                    (
                        metricid ,
                        servertype ,
                        metricname ,
                        metricdescription ,
                        validvalues ,
                        valuedescription
		            )
            VALUES
                    (
                        @metricid , -- metricid - int
                        N'ADB' , -- metrictype - nvarchar(32)
                        N'Server-level Firewall Rules' , -- metricname - nvarchar(256)
                        N'Determine whether unapproved server-level firewall rules have been configured on Azure SQL Database' , -- metricdescription - nvarchar(1024)
                        N'' , -- validvalues - nvarchar(1024)
                        N'When enabled, this check will identify a risk if unapproved server-level firewall rules have been configured on Azure SQL Database. Please specify authorized firewall rules in START_IP-END_IP format.'  -- valuedescription - nvarchar(1024)
						);

				INSERT  INTO dbo.policymetricextendedinfo
                    (
                        policyid ,
                        metricid ,
						servertype,
                        reportkey ,
                        reporttext ,
                        severity ,
                        severityvalues ,
                        assessmentid
		            )
            VALUES
                    (
                        0 , -- policyid - int
                        @metricid , -- metricid - int
						N'ADB', -- servertype nvarchar(3),
                        N'' , -- reportkey - nvarchar(32)
                        N'Are there any unapproved server-level firewall rules?' , -- reporttext - nvarchar(4000)
                        3 , -- severity - int
                        N'' , -- severityvalues - nvarchar(4000)
                        0  -- assessmentid - int
                    );
        END;


-----Database-level Firewall Rules
        SELECT
            @metricid = 124;
        IF NOT EXISTS ( SELECT
                            TOP 1 * 
                        FROM
                            metric
                        WHERE
                            metricid = @metricid )
        BEGIN  
            INSERT  INTO dbo.metric
                    (
                        metricid ,
                        metrictype ,
                        metricname ,
                        metricdescription ,
                        isuserentered ,
                        ismultiselect ,
                        validvalues ,
                        valuedescription
		            )
            VALUES
                    (
                        @metricid , -- metricid - int
                        N'Configuration' , -- metrictype - nvarchar(32)
                        N'NA' , -- metricname - nvarchar(256)
                        N'NA' , -- metricdescription - nvarchar(1024)
                        1 , -- isuserentered - bit
                        1 , -- ismultiselect - bit
                        N'NA' , -- validvalues - nvarchar(1024)
                        N'NA'  -- valuedescription - nvarchar(1024)							        
                    );
				
            INSERT  INTO dbo.policymetric
                    (
                        policyid ,
                        metricid ,
                        isenabled ,
                        reportkey ,
                        reporttext ,
                        severity ,
                        severityvalues ,
                        assessmentid
		            )
            VALUES
                    (
                        0 , -- policyid - int
                        @metricid , -- metricid - int
                        0 , -- isenabled - bit
                        N'NA' , -- reportkey - nvarchar(32)
                        N'NA' , -- reporttext - nvarchar(4000)
                        0 , -- severity - int
                        N'NA' , -- severityvalues - nvarchar(4000)
                        0  -- assessmentid - int
                    );

				
			INSERT  INTO dbo.metricextendedinfo
                    (
                        metricid ,
                        servertype ,
                        metricname ,
                        metricdescription ,
                        validvalues ,
                        valuedescription
		            )
            VALUES
                    (
                        @metricid , -- metricid - int
                        N'ADB' , -- metrictype - nvarchar(32)
                        N'Database-level Firewall Rules' , -- metricname - nvarchar(256)
                        N'Determine whether unapproved database-level firewall rules have been configured on Azure SQL Database' , -- metricdescription - nvarchar(1024)
                        N'' , -- validvalues - nvarchar(1024)
                        N'When enabled, this check will identify a risk if unapproved database-level firewall rules have been configured on Azure SQL Database. Please specify authorized firewall rules in START_IP-END_IP format.'  -- valuedescription - nvarchar(1024)							        
                    );


			INSERT  INTO dbo.policymetricextendedinfo
                    (
                        policyid ,
                        metricid ,
                        servertype ,
                        reportkey ,
                        reporttext ,
                        severity ,
                        severityvalues ,
                        assessmentid
		            )
            VALUES
                    (
                        0 , -- policyid - int
                        @metricid , -- metricid - int
                        N'ADB', -- servertype nvarchar(3),
                        N'' , -- reportkey - nvarchar(32)
                        N'Are there any unapproved database-level firewall rules on Azure SQL Database?' , -- reporttext - nvarchar(4000)
                        3 , -- severity - int
                        N'' , -- severityvalues - nvarchar(4000)
                        0  -- assessmentid - int
                    );
        END;

		
----- NTFS Folder Level Encryption
        SELECT
            @metricid = 125;
        IF NOT EXISTS ( SELECT
                            TOP 1 * 
                        FROM
                            metric
                        WHERE
                            metricid = @metricid )
        BEGIN  
            INSERT  INTO dbo.metric
                    (
                        metricid ,
                        metrictype ,
                        metricname ,
                        metricdescription ,
                        isuserentered ,
                        ismultiselect ,
                        validvalues ,
                        valuedescription
		            )
            VALUES
                    (
                        @metricid , -- metricid - int
                        N'Access' , -- metrictype - nvarchar(32)
                        N'NTFS Folder Level Encryption' , -- metricname - nvarchar(256)
                        N'Determine whether NTFS folder level encryption is configured for Windows folders' , -- metricdescription - nvarchar(1024)
                        0 , -- isuserentered - bit
                        0 , -- ismultiselect - bit
                        N'' , -- validvalues - nvarchar(1024)
                        N'When enabled, this check will identify Windows folders that do not have NTFS encryption enabled.'  -- valuedescription - nvarchar(1024)	
                    );
		
            INSERT  INTO dbo.policymetric
                    (
                        policyid ,
                        metricid ,
                        isenabled ,
                        reportkey ,
                        reporttext ,
                        severity ,
                        severityvalues ,
                        assessmentid
		            )
            VALUES
                    (
                        0 , -- policyid - int
                        @metricid , -- metricid - int
                        0 , -- isenabled - bit
                        N'' , -- reportkey - nvarchar(32)
                        N'Are Windows folders not configured for NTFS encryption?' , -- reporttext - nvarchar(4000)
                        1 , -- severity - int
                        N'' , -- severityvalues - nvarchar(4000)
                        0  -- assessmentid - int
                    );
        END;


---------------------Update Policy Metrics with new checks--------------------------------------------------------------------------------------------------------

        IF (
             @ver IS NULL	-- this is a new install, so fix the All Servers policy to use the default values for the new security checks
             AND NOT EXISTS ( SELECT
                                TOP 1 * 
                              FROM
                                policymetric
                              WHERE
                                policyid = 1
                                AND assessmentid = 1
                                AND metricid BETWEEN @startmetricid AND @metricid )
           )
            INSERT  INTO policymetric
                    (
                      policyid ,
                      assessmentid ,
                      metricid ,
                      isenabled ,
                      reportkey ,
                      reporttext ,
                      severity ,
                      severityvalues
                    )
                    SELECT
                        1 ,
                        1 ,
                        m.metricid ,
                        m.isenabled ,
                        m.reportkey ,
                        m.reporttext ,
                        m.severity ,
                        m.severityvalues
                    FROM
                        policymetric m
                    WHERE
                        m.policyid = 0
                        AND m.assessmentid = 0
                        AND m.metricid BETWEEN @startmetricid AND @metricid

	-- now add the new security checks to all existing policies, but disable it by default so it won't interfere with the current assessment values
        INSERT  INTO policymetric
                (
                  policyid ,
                  assessmentid ,
                  metricid ,
                  isenabled ,
                  reportkey ,
                  reporttext ,
                  severity ,
                  severityvalues
                )
                SELECT
                    a.policyid ,
                    a.assessmentid ,
                    m.metricid ,
                    0 ,
                    m.reportkey ,
                    m.reporttext ,
                    m.severity ,
                    m.severityvalues
                FROM
                    policymetric m ,
                    assessment a
                WHERE
                    m.policyid = 0
                    AND m.assessmentid = 0
                    AND m.metricid BETWEEN @startmetricid AND @metricid
                    AND a.policyid > 0
				-- this check makes it restartable
                    AND a.assessmentid NOT IN (
                    SELECT DISTINCT
                        assessmentid
                    FROM
                        policymetric
                    WHERE
                        metricid BETWEEN @startmetricid AND @metricid )

						
--------------------- Update New Policy Metrics for server type Azure SQL Database --------------------------------------------------------------------------------------------------------

		IF (NOT EXISTS ( SELECT  TOP 1 * FROM policymetricextendedinfo where policyid <> 0 AND metricid between @startmetricid AND @metricid ))
		BEGIN
			INSERT INTO policymetricextendedinfo
			SELECT pm.policyid, pm.metricid, pm.assessmentid, 'ADB', pme.reportkey, pme.reporttext, pme.severity, pme.severityvalues
			FROM policymetric pm INNER JOIN policymetricextendedinfo pme ON pm.metricid = pme.metricid
			WHERE pm.policyid <> 0 AND 
			pme.policyid = 0 AND pme.assessmentid = 0 AND
		    pm.metricid IN (
			117,
			118,
			120,
			121,
			123,
			124
			)
		END
				
    --END;
GO



