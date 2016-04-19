declare @ver int
SELECT @ver=schemaversion FROM currentversion
if (isnull(@ver, 900) <= 2699)	-- Check to prevent this from running in future upgrades, but run for all prior versions at any version level because the version gets updated later
BEGIN
	-- fix coveringpermissions for VIEW TRACKING CHANGES permission
	insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('SCHEMA', 'VIEW CHANGE TRACKING', 'CONTROL', 'DATABASE', 'CONTROL')
	insert into coveringpermissionhierarchy (permissionlevel, permissionname, coveringpermissionname, parentpermissionlevel, parentcoveringpermission) values ('OBJECT', 'VIEW CHANGE TRACKING', 'CONTROL', 'SCHEMA', 'CONTROL')

	-- update default security checks
	declare @metricid int, @strval nvarchar(512)

	-- update the sql server version security check default settings with the latest versions and service packs
	select @metricid = 2
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set severityvalues= N'''11.0.2100'',''10.50.2500'',''10.0.5500'',''9.00.5000'',''8.00.2039'''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end

	-- update the o/s server version security check default settings with the latest versions and service packs
	select @metricid = 39
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set severityvalues= N'''Microsoft Windows Server 2008 R2 Enterprise , Service Pack 1'',''Microsoft Windows Server 2008 R2 Standard , Service Pack 1'',''Microsoft Windows Web Server 2008 R2 , Service Pack 1''' +
								N',''Microsoft® Windows Server® 2008 Enterprise , Service Pack 2'',''Microsoft® Windows Server® 2008 Enterprise without Hyper-V , Service Pack 2'',''Microsoft® Windows Server® 2008 Datacenter , Service Pack 2'',''Microsoft® Windows Server® 2008 Datacenter without Hyper-V , Service Pack 2'',''Microsoft® Windows Server® 2008 Standard , Service Pack 2'',''Microsoft® Windows Server® 2008 Standard without Hyper-V , Service Pack 2'',''Microsoft® Windows® Web Server 2008 , Service Pack 2''' +
								N',''Microsoft(R) Windows(R) Server 2003, Enterprise Edition, Service Pack 2'',''Microsoft(R) Windows(R) Server 2003 Enterprise x64 Edition, Service Pack 2'',''Microsoft(R) Windows(R) Server 2003, Standard Edition, Service Pack 2'',''Microsoft(R) Windows(R) Server 2003 Standard x64 Edition, Service Pack 2'''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end

	-- set the new default settings for existing security checks
	select @metricid = 11
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set isenabled=0
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set isenabled=0
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 58
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set isenabled=0
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set isenabled=0
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 59
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set isenabled=0
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set isenabled=0
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end	
	
	---- ADD NEW SECURITY CHECKS ***************************************************************

	---- NOTE: last metricid used in previous version was 88
	----		I have left a sample new one here

	declare @startmetricid int
	select  @startmetricid = 89

	select @metricid = 89
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'Public Role Has Permissions on User Database Objects', 'Permissions', 0, 0, '', 'When enabled, this check will identify a risk if the public database role has been granted permissions on any user objects within a user database. Specify the approved databases.',
										'Determine whether the public database role has been granted permissions on user database objects.')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 3, '', '',
										'Has the public database role been granted permissions on user database objects?')
	end

	select @metricid = 90
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'Integration Services Roles Have Dangerous Security Principals', 'Permissions', 1, 1, '', 'When enabled, this check will identify a risk if a dangerous security principal belongs to an SSIS database role. Specify which security principals you consider dangerous.',
										'Determine whether dangerous security principals belong to any SQL Server Information Services (SSIS) database roles.')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 2, '''public'',''guest''', '',
										'Have any dangerous principals been added to the SSIS database roles?')
	end

	select @metricid = 91
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'Integration Services Permissions Not Acceptable', 'Permissions', 1, 1, '', 'When enabled, this check will identify a risk if an unapproved user or role has been granted permissions on an Integration Services stored procedure. Specify the stored procedures.',
										'Determine whether unapproved users or roles have been granted permissions on an Integration Services stored procedure.')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 1, '''sp_add_dtspackage'',''sp_drop_dtspackage'',''sp_dts_addfolder'',''sp_dts_addlogentry'',''sp_dts_checkexists'',''sp_dts_deletefolder'',''sp_dts_deletepackage'',''sp_dts_getfolder'',''sp_dts_getpackage'',''sp_dts_getpackageroles'',''sp_dts_listfolders'',''sp_dts_listpackages'',''sp_dts_putpackage'',''sp_dts_renamefolder'',''sp_dts_setpackageroles'',''sp_dump_dtslog_all'',''sp_dump_dtspackagelog'',''sp_dump_dtssteplog'',''sp_dump_dtstasklog'',''sp_enum_dtspackagelog'',''sp_enum_dtspackages'',''sp_enum_dtssteplog'',''sp_enum_dtstasklog'',''sp_get_dtspackage'',''sp_get_dtsversion'',''sp_log_dtspackage_begin'',''sp_log_dtspackage_end'',''sp_log_dtsstep_begin'',''sp_log_dtsstep_end'',''sp_log_dtstask'',''sp_make_dtspackagename'',''sp_reassign_dtspackageowner'',''sp_ssis_addfolder'',''sp_ssis_addlogentry'',''sp_ssis_checkexists'',''sp_ssis_deletefolder'',''sp_ssis_deletepackage'',''sp_ssis_getfolder'',''sp_ssis_getpackage''', '',
										'Do unapproved users or roles have permissions on SSIS stored procedures?')
	end
	
	select @metricid = 92
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'Weak Passwords', 'Login', 0, 0, '', 'When enabled, this check will identify a risk if a SQL login on the target instance has a weak password. Specify which SQL logins should not be checked.',
										'Determine whether any SQL login passwords match the login name or a list of common and restricted passwords.')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 3, '', '', 'Does this SQL login have a weak password?')
	end
		

	-- note: the following uses the @metricid to determine the ending value for the metrics to apply to all of the policies

	if (@ver is null	-- this is a new install, so fix the All Servers policy to use the default values for the new security checks
		and not exists (select * from policymetric where policyid = 1 and assessmentid=1 and metricid between @startmetricid and @metricid))
			insert into policymetric (policyid, assessmentid, metricid, isenabled, reportkey, reporttext, severity, severityvalues)
				select 1, 1, m.metricid, m.isenabled, m.reportkey, m.reporttext, m.severity, m.severityvalues
					from policymetric m
					where m.policyid = 0
						and m.assessmentid = 0
						and m.metricid between @startmetricid and @metricid

	-- now add the new security checks to all existing policies, but disable it by default so it won't interfere with the current assessment values
	insert into policymetric (policyid, assessmentid, metricid, isenabled, reportkey, reporttext, severity, severityvalues)
		select a.policyid, a.assessmentid, m.metricid, 0, m.reportkey, m.reporttext, m.severity, m.severityvalues
			from policymetric m, assessment a
			where m.policyid = 0
				and m.assessmentid = 0
				and m.metricid between @startmetricid and @metricid
				and a.policyid > 0
				-- this check makes it restartable
				and a.assessmentid not in (select distinct assessmentid from policymetric where metricid between @startmetricid and @metricid)
END

GO
