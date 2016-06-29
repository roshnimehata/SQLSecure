declare @ver int
SELECT @ver=schemaversion FROM currentversion
if (isnull(@ver,900) = 900)
BEGIN
	-- provide view access to tables

	GRANT SELECT ON dbo.[policy] TO SQLSecureView
	GRANT SELECT ON dbo.[policymember] TO SQLSecureView
	GRANT SELECT ON dbo.[policyinterview] TO SQLSecureView
	GRANT SELECT ON dbo.[policymetric] TO SQLSecureView
	GRANT SELECT ON dbo.[notificationprovider] TO SQLSecureView
	GRANT SELECT ON dbo.[registeredservernotification] TO SQLSecureView
	GRANT SELECT ON dbo.[serverosobject] TO SQLSecureView
	GRANT SELECT ON dbo.[serverosobjectpermission] TO SQLSecureView
	GRANT SELECT ON dbo.[serveroswindowsaccount] TO SQLSecureView
	GRANT SELECT ON dbo.[serveroswindowsgroupmember] TO SQLSecureView
	GRANT SELECT ON dbo.[serverservice] TO SQLSecureView
	GRANT SELECT ON dbo.[serverprotocol] TO SQLSecureView
	--GO

	-- initial data loading for policies and metrics
	delete from policymetric
	delete from metric
	delete from policy

	SET IDENTITY_INSERT dbo.policy ON
	--GO

	declare @sql nvarchar(2048)
	-- add the default policy to be used for setting the default values when a new policy is created
	select @sql = 'insert into policy (policyid, policyname, policydescription, issystempolicy, isdynamic)
						values (0, ''Default'', ''Default policy settings used when creating a new policy'', 1, 0);'
	-- add the "All Servers" policy
	select @sql = @sql + ' insert into policy (policyid, policyname, policydescription, issystempolicy, isdynamic)
							values (1, ''All Servers'', ''Global security checks that should be performed on all audited SQL Servers'', 1, 1)'

	exec (@sql)		-- use dynamic sql so there won't be missing column errors on future upgrades since isdynamic has been removed

	--GO
	SET IDENTITY_INSERT dbo.policy OFF
	--GO


	-- add metrics and default policy values

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (1, 'Audit Data Is Stale', 'Data Integrity', 1, 0, '', 'When enabled, this check will identify a risk if audit data was not collected within the specified timeframe. Specify the number of days audit data is considered valid.',
									'Determine whether the nearest snapshot collection occurred within an acceptable timeframe from the selected date')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 1, 1, 2, '''30''', '',
									'Was the most recent snapshot collected within an acceptable timeframe?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (2, 'SQL Server Version', 'Configuration', 1, 1, '', 'When enabled, this check will identify a risk if the SQL Server version is below the minimum acceptable level. Specify the minimum acceptable level for each SQL Server version.',
									'Determine whether the SQL Server software is at an acceptable minimum version')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 2, 1, 1, '''8.00.2039'',''9.00.4035'',''10.0.1600''', '',
									'Is SQL Server below the minimum acceptable version?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (3, 'SQL Authentication Enabled', 'Login', 0, 0, '', 'When enabled, this check will identify a risk if SQL Authentication is enabled on the SQL Server.',
									'Determine whether SQL Authentication is allowed on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 3, 1, 1, '', '',
									'Is SQL Authentication enabled on the SQL Server?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (4, 'Login Audit Level', 'Auditing', 0, 1, '''None'':''None'', ''Sucessful logins only'':''Success'', ''Failed logins only'':''Failure'',''Both failed and successful logins'':''All''', 'When enabled, this check will identify a risk if the expected login auditing configuration is not being used by the SQL Server. Specify the expected login auditing configuration.',
									'Determine whether the SQL Server login auditing configuration is acceptable')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 4, 1, 1, '''Failure'',''All''', '',
									'Is the login auditing configuration acceptable?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (5, 'Cross Database Ownership Chaining Enabled', 'Surface Area', 0, 0, '', 'When enabled, this check will identify a risk if Cross Database Chaining is enabled on the SQL Server.',
									'Determine whether Cross Database Ownership Chaining is enabled on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 5, 1, 1, '', '',
									'Is Cross Database Ownership Chaining enabled on the SQL Server?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (6, 'Guest User Enabled', 'Access', 1, 1, '', 'When enabled, this check will identify a risk if Guest user access is available on unapproved databases on the SQL Server. Specify the databases on which Guest user access is approved.',
									'Determine whether Guest user access is available on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 6, 1, 1, '''master'',''tempdb''', '',
									'Is Guest user access available on the SQL Server?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (7, 'Suspect Logins', 'Login', 0, 0, '', 'When enabled, this check will identify a risk if Windows logins exist that could not be resolved in Active Directory.',
									'Determine whether suspect logins exist on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 7, 1, 1, '', '',
									'Do suspect logins exist on the SQL Server?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (8, 'C2 Audit Trace Enabled', 'Auditing', 0, 0, '', 'When enabled, this check will identify a risk if C2 Audit Trace is not enabled on the SQL Server.',
									'Determine whether C2 Audit Trace is enabled on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 8, 1, 1, '', '',
									'Is C2 Audit Trace enabled on the SQL Server?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (9, 'xp_cmdshell Proxy Account Exists', 'Configuration', 0, 0, '', 'When enabled, this check will identify a risk if a Proxy Account is enabled on the SQL Server.',
									'Determine whether a Proxy Account is enabled on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 9, 1, 1, '', '',
									'Is a Proxy Account enabled on the SQL Server?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (10, 'DAC Remote Access', 'Access', 0, 0, '', 'When enabled, this check will identify a risk if Dedicated Administrator Connection is available remotely on the SQL Server.',
									'Determine whether the Dedicated Administrator Connection is available remotely')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 10, 1, 1, '', '',
									'Is Dedicated Administrator Connection enabled remotely on the SQL Server?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (11, 'Integration Services', 'Configuration', 1, 1, '', 'When enabled, this check will identify a risk if any Integration Services stored procedures have been assigned permissions. Specify the stored procedures.',
									'Determine whether permissions have been granted on Integration Services stored procedures')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 11, 1, 1, '''sp_add_dtspackage'',''sp_enum_dtspackages'',''sp_add_job'',''sp_add_jobstep''', '',
									'Has anyone been granted permission to execute Integration Services stored procedures on the SQL Server?')

	--this check has been removed, but a placeholder is left here so it is not reused
	-- this one may be reactivated in the future after further definition
	--insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
	--                values (12, 'OLAP SQL Authentication Enabled', 'Configuration', 0, 0, '', 'When enabled, this check will identify a risk if OLAP SQL Authentication is enabled on the SQL Server.',
	--                                'Determine whether OLAP SQL Authentication is enabled on the SQL Server')
	--insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
	--                values (0, 12, 1, 1, '', '',
	--                                'Is OLAP SQL Authentication enabled?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (13, 'SQL Mail or Database Mail Enabled', 'Configuration', 0, 0, '', 'When enabled, this check will identify a risk if SQL Mail or Database Mail are enabled on the SQL Server.',
									'Determine whether SQL Mail or Database Mail has been enabled on the SQL Server') 
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 13, 1, 1, '', '',
									'Is SQL Mail or Database Mail enabled?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (14, 'SQL Agent Mail', 'Configuration', 0, 0, '', 'When enabled, this check will identify a risk if a SQL Agent Mail profile exists on the SQL Server.',
									'Determine whether the SQL Server Agent has been configured to allow email')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 14, 1, 1, '', '',
									'Is SQL Agent Mail enabled?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (15, 'Sample Databases Exist', 'Configuration', 1, 1, '', 'When enabled, this check will identify a risk if any sample databases exist on the SQL Server. Specify the sample databases.',
									'Determine whether sample databases exist on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 15, 1, 1, '''Northwind'',''pubs'',''AdventureWorks'',''AdventureWorksDW''', '',
									'Do the SQL Server sample databases exist?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (16, 'sa Account Not Disabled or Renamed', 'Configuration', 0, 0, '', 'When enabled, this check will identify a risk if the SQL Server sa account is enabled and not renamed on SQL Server 2005.',
									'Determine whether the SQL Server sa account has been disabled or renamed on SQL Server 2005')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 16, 1, 1, '', '',
									'Does the SQL Server sa account exist unchanged?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (17, 'sa Account Has Blank Password', 'Login', 0, 0, '', 'When enabled, this check will identify a risk if the SQL Server sa account has a blank password.',
									'Determine whether the SQL Server sa account has a blank password')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 17, 1, 3, '', '',
									'Does SQL Server sa account have a blank password?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (18, 'System Table Updates', 'Configuration', 0, 0, '', 'When enabled, this check will identify a risk if System Table Updates is configured.',
									'Determine whether the "Allow Updates to System Tables" configuration option is enabled on SQL Server 2005')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 18, 1, 1, '', '',
									'Are System Table Updates allowed?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (19, 'Everyone System Table Access', 'Permissions', 0, 0, '', 'When enabled, this check will identify a risk if Everyone has access to system tables on the SQL Server.',
									'Determine whether the Everyone group has read access to system tables on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 19, 1, 1, '', '',
									'Does Everyone have read access to system tables?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (20, 'Startup Stored Procedures Enabled', 'Access', 0, 0, '', 'When enabled, this check will identify a risk if "Scan for startup stored procedures" is enabled on the SQL Server.',
									'Determine whether the "Scan for startup stored procedures" configuration option has been enabled on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 20, 1, 1, '', '',
									'Are startup stored procedures enabled?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (21, 'Startup Stored Procedures', 'Access', 1, 1, '', 'When enabled, this check will identify a risk if an unapproved stored procedure is set to run at startup on the SQL Server. Specify the approved startup stored procedures.',
									'Determine whether there are unapproved stored procedures set to run at startup on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 21, 0, 1, '''none''', '',    -- disabled by default, the list must be entered by user
									'Are any unapproved stored procedures set to run at startup?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (22, 'Stored Procedures Encrypted', 'Access', 0, 0, '', 'When enabled, this check will identify a risk if any user stored procedures are not encrypted on the SQL Server.',
									'Determine whether user stored procedures are encrypted on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 22, 1, 1, '', '',
									'Are any user stored procedures not encrypted?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (23, 'User Defined Extended Stored Procedures (XSPs)', 'Access', 1, 1, '', 'When enabled, this check will identify a risk if any unapproved user-defined Extended Stored Procedures (XSPs) exist on the SQL Server. Specify the approved user-defined XSPs.',
									'Determine whether unapproved user-defined Extended Stored Procedures (XSPs) exist')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 23, 1, 1, '''none''', '',    -- by default, any user extended stored procedures are bad
									'Do user-defined Extended Stored Procedures (XSPs) exist?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (24, 'Dangerous Extended Stored Procedures (XSPs)', 'Access', 1, 1, '', 'When enabled, this check will identify a risk if any dangerous Extended Stored Procedure (XSPs) have been assigned permissions. Specify which XSPs you consider dangerous.',
									'Determine whether permissions have been granted on dangerous Extended Stored Procedures (XSPs)')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 24, 0, 1, '', '',    -- by default, other alerts cover most of them, so this is the list of unusual ones for the user
									'Has anyone been granted permission to execute dangerous Extended Stored Procedures (XSPs)?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (25, 'Remote Access', 'Access', 0, 0, '', 'When enabled, this check will identify a risk if Remote Access is enabled on the SQL Server.',
									'Determine whether Remote Access is enabled on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 25, 1, 1, '', '',
									'Is Remote Access enabled?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (26, 'Unapproved Protocols', 'Surface Area', 0, 1, '''Named Pipes'':''Named Pipes'',''NWLink IPX/SPX (2000)'':''NWLink IPX/SPX'',''Shared Memory (2005)'':''Shared Memory'',''TCP/IP'':''TCP/IP'',''VIA'':''VIA''', 'When enabled, this check will identify a risk if unapproved protocols are enabled. Specify the approved protocols.',
									'Determine whether unapproved protocols are enabled on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 26, 1, 1, '''TCP/IP''', '',
									'Are unapproved protocols enabled?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (27, 'Common TCP Port Used', 'Surface Area', 1, 1, '', 'When enabled, this check will identify a risk if common TCP ports are used by SQL Server. Specify the common TCP ports.',
									'Determine whether TCP is using a common port on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 27, 1, 1, '''1433'',''1434''', '',
									'Are common TCP ports used?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (28, 'SQL Server Available For Browsing', 'Surface Area', 0, 0, '', 'When enabled, this check will identify a risk if SQL Server is visible for browsing from client computers.',
									'Determine whether the SQL Server is hidden from client computers')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 28, 1, 1, '', '',
									'Is SQL Server visible to client computers?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (29, 'Agent Job Execution', 'Permissions', 0, 0, '', 'When enabled, this check will identify a risk if anyone who is not an administrator has permission to execute SQL Agent CmdExec jobs on the SQL Server.',
									'Determine whether only administrators can execute SQL Agent CmdExec Jobs')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 29, 1, 1, '', '',
									'Can anyone besides administrators execute SQL Agent CmdExec jobs on the SQL Server?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (30, 'Replication Enabled', 'Configuration', 0, 0, '', 'When enabled, this check will identify a risk if replication is enabled on the SQL Server.',
									'Determine whether replication is enabled on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 30, 1, 1, '', '',
									'Is replication enabled on the SQL Server?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (31, 'Registry Key Owners Not Acceptable', 'Permissions', 1, 1, '', 'When enabled, this check will identify a risk if unapproved registry key owners exist. Specify the approved owners. Can use ''%'' as wildcard.',
									'Determine whether registry keys that can affect SQL Server security have unapproved owners')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 31, 0, 1, '', '',    -- disabled by default, user must enter list of valid owners
									'Do unapproved registry key owners exist?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (32, 'Registry Key Permissions Not Acceptable', 'Permissions', 1, 1, '', 'When enabled, this check will identify a risk if any unapproved user has permissions on a registry key that affects SQL Server security. Specify the approved users. Can use ''%'' as wildcard.',
									'Determine whether users have unapproved access to registry keys')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 32, 0, 1, '', '',    -- disabled by default, user must enter list of valid permission grantees
									'Do users have unapproved access to registry keys?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (33, 'Files On Drives Not Using NTFS', 'Access', 0, 0, '', 'When enabled, this check will identify a risk if any SQL Server files are not stored on drives using NTFS.',
									'Determine whether all SQL Server files are stored on drives that use NTFS')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 33, 1, 2, '', '',
									'Are any SQL Server files on drives not using NTFS?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (34, 'Executable File Owners Not Acceptable', 'Permissions', 1, 1, '', 'When enabled, this check will identify a risk if unapproved executable file owners exist. Specify the approved owners. Can use ''%'' as wildcard.',
									'Determine whether SQL Server executable files have unapproved owners')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 34, 0, 1, '', '',    -- disabled by default, user must enter list of valid owners
									'Do unapproved executable file owners exist?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (35, 'Executable File Permissions Not Acceptable', 'Permissions', 1, 1, '', 'When enabled, this check will identify a risk if any unapproved user has permissions on SQL Server executable file. Specify the approved users. Can use ''%'' as wildcard.',
									'Determine whether users have unapproved access to SQL Server executable files')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 35, 0, 2, '', '',    -- disabled by default, user must enter list of valid permission grantees
									'Do users have unapproved access to executable files?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (36, 'Database File Owners Not Acceptable', 'Permissions', 1, 1, '', 'When enabled, this check will identify a risk if unapproved database file owners exist. Specify the approved owners. Can use ''%'' as wildcard.',
									'Determine whether SQL Server database files have unapproved owners')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 36, 0, 1, '', '',    -- disabled by default, user must enter list of valid owners
									'Do unapproved database file owners exist?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (37, 'Everyone Database File Access', 'Permissions', 0, 0, '', 'When enabled, this check will identify a risk if Everyone has access to database files.',
									'Determine whether the Everyone group has access to SQL Server database files')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 37, 1, 3, '', '',
									'Does Everyone have access to database files?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (38, 'Database File Permissions Not Acceptable', 'Permissions', 1, 1, '', 'When enabled, this check will identify a risk if any unapproved user has permissions on database files. Specify the approved users. Can use ''%'' as wildcard.',
									'Determine whether users have unapproved access to SQL Server database files')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 38, 0, 1, '', '',    -- disabled by default, user must enter list of valid permission grantees
									'Do users have unapproved access to database files?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (39, 'Operating System Version', 'Access', 1, 1, '', 'When enabled, this check will identify a risk if the Operating System is not at an acceptable version. Specify the acceptable OS versions.',
									'Determine whether the Operating System version is at an acceptable level')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 39, 1, 1, '''Microsoft(R) Windows(R) Server 2003, Standard Edition, Service Pack 2''', '',
									'Is OS version at an acceptable level?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (40, 'SQL Server Service Login Account Not Acceptable', 'Login', 1, 1, '', 'When enabled, this check will identify a risk if the SQL Server Service account is not acceptable. Specify the acceptable login accounts.',
									'Determine whether the SQL Server Service account is acceptable')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 40, 0, 1, '', '',    -- disabled by default, user must enter list of logins
									'Is an unacceptable SQL Server Service account being used?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (41, 'Reporting Services Running', 'Surface Area', 0, 0, '', 'When enabled, this check will identify a risk if Microsoft Reporting Services is running on the SQL Server.',
									'Determine whether Microsoft Reporting Services is running on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 41, 1, 1, '', '',
									'Are Microsoft Reporting Services running?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (42, 'Analysis Services Running', 'Configuration', 0, 0, '', 'When enabled, this check will identify a risk if Analysis Services (OLAP) is running on the SQL Server.',
									'Determine whether Analysis Services (OLAP) is running on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 42, 1, 1, '', '',
									'Are Analysis Services (OLAP) running?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (43, 'Analysis Services Login Account Not Acceptable', 'Login', 1, 1, '', 'When enabled, this check will identify a risk if the Analysis Services account is not acceptable. Specify the acceptable login accounts.',
									'Determine whether the Analysis Services account is acceptable')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 43, 0, 1, '', '',    -- disabled by default, user must enter list of logins
									'Is an unacceptable Analysis Services account being used?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (44, 'Notification Services Running', 'Surface Area', 0, 0, '', 'When enabled, this check will identify a risk if Notification Services is running on the SQL Server.',
									'Determine whether Notification Services is running on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 44, 1, 1, '', '',
									'Are Notification Services running?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (45, 'Notification Services Login Account Not Acceptable', 'Login', 1, 1, '', 'When enabled, this check will identify a risk if the Notification Services account is not acceptable. Specify the acceptable login accounts.',
									'Determine whether the Notification Services account is acceptable')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 45, 0, 1, '', '',    -- disabled by default, user must enter list of logins
									'Is an unacceptable Notification Services account being used?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (46, 'Integration Services Running', 'Surface Area', 0, 0, '', 'When enabled, this check will identify a risk if Integration Services is running on the SQL Server.',
									'Determine whether Integration Services is running on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 46, 1, 1, '', '',
									'Are Integration Services running?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (47, 'Integration Services Login Account Not Acceptable', 'Login', 1, 1, '', 'When enabled, this check will identify a risk if the Integration Service account is not acceptable. Specify the acceptable login accounts.',
									'Determine whether the Integration Services account is acceptable')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 47, 0, 1, '', '',    -- disabled by default, user must enter list of logins
									'Is an unacceptable Integration Services account being used?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (48, 'SQL Server Agent Running', 'Surface Area', 0, 0, '', 'When enabled, this check will identify a risk if SQL Server Agent is running on the SQL Server.',
									'Determine whether the SQL Server Agent is running on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 48, 1, 1, '', '',
									'Is the SQL Server Agent running?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (49, 'SQL Server Agent Login Account Not Acceptable', 'Login', 1, 1, '', 'When enabled, this check will identify a risk if the SQL Server Agent Service account is not acceptable. Specify the acceptable login accounts.',
									'Determine whether the SQL Server Agent Service account is acceptable')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 49, 0, 1, '', '',    -- disabled by default, user must enter list of logins
									'Is an unacceptable SQL Server Agent Service account being used?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (50, 'Full-Text Search Running', 'Configuration', 0, 0, '', 'When enabled, this check will identify a risk if Full-Text Search is running on the SQL Server.',
									'Determine whether Full-Text Search is running on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 50, 1, 1, '', '',
									'Is Full-Text Search running?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (51, 'Full-Text Search Login Account Not Acceptable', 'Login', 1, 1, '', 'When enabled, this check will identify a risk if the Full-Text Search Service account is not acceptable. Specify the acceptable login accounts.',
									'Determine whether the Full-Text Search Service account is acceptable')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 51, 0, 1, '', '',    -- disabled by default, user must enter list of logins
									'Is an unacceptable Full-Text Search Service account being used?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (52, 'SQL Server Browser Running', 'Access', 0, 0, '', 'When enabled, this check will identify a risk if SQL Server Browser is running on the SQL Server.',
									'Determine whether the SQL Server Browser is running on the SQL Server')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 52, 1, 1, '', '',
									'Is the SQL Server Browser running?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (53, 'SQL Server Browser Login Account Not Acceptable', 'Login', 1, 1, '', 'When enabled, this check will identify a risk if the SQL Server Browser Service account is not acceptable. Specify the acceptable login accounts.',
									'Determine whether the SQL Server Browser Service account is acceptable')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 53, 0, 1, '', '',    -- disabled by default, user must enter list of logins
									'Is an unacceptable SQL Server Browser Service account being used?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (54, 'Snapshot Not Found', 'Data Integrity', 0, 0, '', 'When enabled, this check will identify a risk if audit data is missing.',
									'Determine whether all servers in the policy have valid audit data for the selected timeframe')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 54, 1, 2, '', '',
									'Are any servers in the policy missing audit data?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (55, 'Snapshot May Be Missing Data', 'Data Integrity', 0, 0, '', 'When enabled, this check will identify a risk if audit data is incomplete or the snapshot returned warnings.',
									'Determine whether all audit data for the selected servers is complete and without warnings')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 55, 1, 2, '', '',
									'Is audit data incomplete?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (56, 'Baseline Data Not Being Used', 'Data Integrity', 0, 0, '', 'When enabled, this check will identify a risk if audit data is not from baseline snapshot.',
									'Determine whether all audit data for the selected timeframe is from baseline snapshots')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 56, 0, 2, '', '',
									'Is any audit data from a non-baseline snapshot?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (57, 'SQL Logins Not Using Password Policy', 'Login', 0, 0, '', 'When enabled, this check will identify a risk if any SQL Login is not protected by the password policy.',
									'Determine whether the password policy is enabled for all SQL Logins')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 57, 1, 2, '', '',
									'Is the password policy enabled for all SQL Logins?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (58, 'Public Database Role Has Permissions', 'Permissions', 0, 0, '', 'When enabled, this check will identify a risk if the public database role has been granted any permissions or been made a member of any other role.',
									'Determine whether the public database role has any permissions')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 58, 1, 3, '', '',
									'Are any permissions granted to the public database role?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (59, 'Blank Passwords', 'Login', 0, 0, '', 'When enabled, this check will identify a risk if any SQL Login has a blank password.',
									'Determine whether any SQL Logins have blank passwords')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 59, 1, 3, '', '',
									'Does any SQL Login have a blank password?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (60, 'Fixed roles assigned to public or guest', 'Access', 0, 0, '', 'When enabled, this check will identify a risk if the public role or guest user are members of any fixed database roles.',
									'Determine whether public or guest are members of any fixed database roles')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 60, 1, 3, '', '',
									'Are any fixed roles assigned to the public role or guest user?')

	insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
					values (61, 'BUILTIN/Administrators is sysadmin', 'Configuration', 0, 0, '', 'When enabled, this check will identify a risk if the BUILTIN/Administrators local Windows group is a member of the sysadmin fixed server role.',
									'Determine whether BUILTIN/Administrators is a member of the sysadmin fixed server role')
	insert into policymetric (policyid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
					values (0, 61, 1, 2, '', '',
									'Is the BUILTIN/Administrators group assigned to the sysadmin role?')



	-- now copy all of the defaults to the All Servers policy
	insert into policymetric (policyid, metricid, isenabled, reportkey, reporttext, severity, severityvalues)
		select 1, metricid, isenabled, reportkey, reporttext, severity, severityvalues
			from policymetric
			where policyid = 0


	--GO




	-- add default notification provider to prevent errors in saving registeredserver email notifications on upgrade
	SET IDENTITY_INSERT dbo.notificationprovider ON
	--GO

	INSERT INTO [SQLsecure].[dbo].[notificationprovider]
		   (
				[notificationproviderid],
				[providername],
				[providertype],
				[servername],
				[port],
				[timeout],
				[requiresauthentication],
				[username],
				[password],
				[sendername],
				[senderemail]
			)
		VALUES (
				1,
				'SQLsecure',
				'Email',
				'',
				25,
				90,
				0,
				'',
				'xcGYVljYAdunbhSctVITVg==',
				'',
				''
				)
	--GO

	SET IDENTITY_INSERT dbo.notificationprovider OFF
END
GO
