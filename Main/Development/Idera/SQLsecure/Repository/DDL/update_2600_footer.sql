declare @ver int
SELECT @ver=schemaversion FROM currentversion
if (isnull(@ver, 900) <= 2500)	-- Check to prevent this from running in future upgrades, but run for all prior versions at any version level because the version gets updated later
BEGIN
	declare @metricid int, @strval nvarchar(512)

	-- update the All Servers policy description
	if (exists (select * from policy where policyid=1) and @ver is null)
	begin
		update policy set policydescription ='Global security checks that should be performed on all SQL Servers; based on the Idera Level 2 Balanced Protection policy.'
			where policyid=1
	end

	-- correct some metric names and descriptions

	-- fix the description on the tcp/ip port security check
	select @metricid = 27, @strval= 'When enabled, this check will identify a risk if unacceptable TCP ports are used by SQL Server. Specify unacceptable TCP ports (specify ''dynamic'' if dynamic port allocation should not be used).'
	if exists (select * from metric where metricid = @metricid )
	begin
		update metric 
			set valuedescription = @strval
			where metricid = @metricid
	end

	-- correct the case in names of previous service checks
	select @metricid = 13, @strval = 'SQL Mail Or Database Mail Enabled'
	if exists (select * from metric where metricid = @metricid )
	begin
		update metric 
			set metricname = @strval
			where metricid = @metricid
		update policyassessment
			set metricname = @strval
			where metricid = @metricid
				and assessmentid not in (select assessmentid from assessment where assessmentstate='A')
	end
	select @metricid = 16, @strval = 'sa Account Not Disabled Or Renamed'
	if exists (select * from metric where metricid = @metricid )
	begin
		update metric 
			set metricname = @strval
			where metricid = @metricid
		update policyassessment
			set metricname = @strval
			where metricid = @metricid
				and assessmentid not in (select assessmentid from assessment where assessmentstate='A')
	end

	select @metricid = 60, @strval = 'Fixed Roles Assigned To public Or guest'
	if exists (select * from metric where metricid = @metricid )
	begin
		update metric
			set metricname = @strval
			where metricid = @metricid
		update policyassessment
			set metricname = @strval
			where metricid = @metricid
				and assessmentid not in (select assessmentid from assessment where assessmentstate='A')
	end

	select @metricid = 61, @strval = 'BUILTIN/Administrators Is sysadmin'
	if exists (select * from metric where metricid = @metricid )
	begin
		update metric 
			set metricname = @strval
			where metricid = @metricid
		update policyassessment
			set metricname = @strval
			where metricid = @metricid
				and assessmentid not in (select assessmentid from assessment where assessmentstate='A')
	end

	select @metricid = 63, @strval = 'Server Is Domain Controller'
	if exists (select * from metric where metricid = @metricid )
	begin
		update metric 
			set metricname = @strval
			where metricid = @metricid
		update policyassessment
			set metricname = @strval
			where metricid = @metricid
				and assessmentid not in (select assessmentid from assessment where assessmentstate='A')
	end

	-- update the password policy security check to use consistent text with the new one
	select @metricid = 57, @strval = 'Determine whether password policy is enforced on all SQL Logins'
	if exists (select * from metric where metricid = @metricid )
	begin
		update metric 
			set metricdescription = @strval
			where metricid = @metricid
		update policyassessment 
			set metricdescription = @strval
			where metricid = @metricid
				and assessmentid not in (select assessmentid from assessment where assessmentstate='A')
	end

	-- update all security checks that specify 2005 only info to be 2005 or later
	select @metricid = 16, @strval = 'SQL Server 2005 or later'
	if not exists (select 1 from metric where metricid = @metricid and valuedescription like '%' + @strval + '%' )
	begin
		update metric 
			set valuedescription = replace(valuedescription, 'SQL Server 2005', @strval)
			where metricid = @metricid
	end

	if not exists (select 1 from metric where metricid = @metricid and metricdescription like '%' + @strval + '%' )
	begin
		update metric 
			set metricdescription = replace(metricdescription, 'SQL Server 2005', @strval)
			where metricid = @metricid
	end
	select @metricid = 18
	if not exists (select 1 from metric where metricid = @metricid and metricdescription like '%' + @strval + '%' )
	begin
		update metric 
			set metricdescription = replace(metricdescription, 'SQL Server 2005', @strval)
			where metricid = @metricid
	end
	select @metricid = 26, @strval = '(2005 or later)'
	if not exists (select 1 from metric where metricid = @metricid and metricdescription like '%' + @strval + '%' )
	begin
		update metric 
			set validvalues = replace(validvalues, '(2005)', @strval)
			where metricid = @metricid
	end


	-- update the sql server version security check default settings with the latest versions and service packs
	select @metricid = 2
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set severityvalues= N'''8.00.2039'',''9.00.4035'',''10.0.2531'''
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
			set severityvalues= N'''Microsoft(R) Windows(R) Server 2003, Standard Edition, Service Pack 2'',''Microsoft(R) Windows(R) Server 2003 Standard x64 Edition, Service Pack 2'',''Microsoft® Windows Server® 2008 Standard , Service Pack 2'',''Microsoft® Windows Server® 2008 Standard without Hyper-V , Service Pack 2'',''Microsoft Windows Server 2008 R2 Standard ''' + 
								N',''Microsoft(R) Windows(R) Server 2003, Enterprise Edition, Service Pack 2'',''Microsoft(R) Windows(R) Server 2003 Enterprise x64 Edition, Service Pack 2'',''Microsoft® Windows Server® 2008 Enterprise , Service Pack 2'',''Microsoft® Windows Server® 2008 Enterprise without Hyper-V , Service Pack 2'',''Microsoft Windows Server 2008 R2 Enterprise '''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end

	-- set the new default settings for existing security checks
	select @metricid = 8
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
	select @metricid = 9
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set severity=2
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set severity=2
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 15
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set severityvalues= N'''Northwind'',''pubs'',''AdventureWorks'',''AdventureWorksAS'',''AdventureWorksDW'''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 16
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set severity=2
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set severity=2
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 20
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set isenabled=1
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set isenabled=1
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 21
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set isenabled=1
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set isenabled=1
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 24
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set isenabled=1,severity=2,severityvalues='''xp_cmdshell'',''xp_available_media'',''xp_dirtree'',''xp_dsninfo'',''xp_enumdsn'',''xp_enumerrorlogs'',''xp_enumgroups'',''xp_eventlog'',''xp_fixeddrives'',''xp_getfiledetails'',''xp_getnetname'',''xp_logevent'',''xp_loginconfig'',''xp_msver'',''xp_readerrorlog'',''xp_servicecontrol'',''xp_sprintf'',''xp_sscanf'',''xp_subdirs'',''xp_deletemail'',''xp_findnextmsg'',''xp_get_mapi_default_profile'',''xp_get_mapi_profiles'',''xp_readmail'',''xp_sendmail'',''xp_startmail'',''xp_stopmail'',''xp_cleanupwebtask'',''xp_convertwebtask'',''xp_dropwebtask'',''xp_enumcodepages'',''xp_makewebtask'',''xp_readwebtask'',''xp_runwebtask'',''sp_OACreate'''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set isenabled=1,severity=2,severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 26
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set severityvalues= N'''Shared Memory'',''TCP/IP'''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 27
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set severityvalues= N'''1433'',''1434'',''dynamic'''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 28
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
	select @metricid = 32
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set severityvalues= N'''INSTALL_ACCT_OR_OWNER_ACCT'''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 35
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set isenabled=1,severityvalues='''INSTALL_ACCT_OR_OWNER_ACCT'''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set isenabled=1,severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 36
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set isenabled=1,severityvalues='''INSTALL_ACCT_OR_OWNER_ACCT'''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set isenabled=1,severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 38
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set isenabled=1,severityvalues='''INSTALL_ACCT_OR_OWNER_ACCT'''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set isenabled=1,severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 39
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
	select @metricid = 40
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set isenabled=1,severityvalues='''SERVICE_SPECIFIC_ACCOUNT'''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set isenabled=1,severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 43
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set isenabled=1,severityvalues='''SERVICE_SPECIFIC_ACCOUNT'''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set isenabled=1,severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 45
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set isenabled=1,severityvalues='''SERVICE_SPECIFIC_ACCOUNT'''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set isenabled=1,severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 47
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set isenabled=1,severityvalues='''SERVICE_SPECIFIC_ACCOUNT'''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set isenabled=1,severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 48
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
	select @metricid = 49
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set isenabled=1,severityvalues='''LOW_PRIVILEDGE_SVC_ACCT'''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set isenabled=1,severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 51
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set isenabled=1,severityvalues='''SERVICE_SPECIFIC_ACCOUNT'''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set isenabled=1,severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 53
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set isenabled=1,severityvalues='''SERVICE_SPECIFIC_ACCOUNT'''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set isenabled=1,severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 54
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set severity=3
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set severity=3
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end
	select @metricid = 55
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


	-- update the password policy security check details to reflect the new text if appropriate
	select @metricid = 57
	if exists (select * from policymetric where metricid = @metricid )
	begin
		update policyassessment 
			set thresholdvalue= N'Server is vulnerable if password policy is not enforced on all SQL Logins.'
			where metricid = @metricid
				and thresholdvalue = N'Server is vulnerable if SQL Logins are found that do not implement the password policy.'
				and assessmentid not in (select assessmentid from assessment where assessmentstate='A')
		update policymetric 
			set reporttext= N'Is password policy enforced on all SQL Logins?'
			where metricid = @metricid
				and reporttext = N'Is the password policy enabled for all SQL Logins?'
				and assessmentid not in (select assessmentid from assessment where assessmentstate='A')
	end



	-- ADD NEW SECURITY CHECKS ***************************************************************

	-- NOTE: metricids 64-66 were reserved for comparison security checks that were not implemented in 2500
	--		I am going to go ahead and leave them reserved and skip over them just in case

	-- NOTE: metricids 67-80 work with 2.5 data and require no schema or data collection changes, but do need the associated update to the getpolicyassessment stored procecure
	--		These metrics were provided as a 2.6 technology preview to Wells Fargo
	declare @startmetricid int
	select @metricid = 67, @startmetricid = 67
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'Active Directory Helper Login Account Not Acceptable', 'Login', 1, 1, '', 'When enabled, this check will identify a risk if the Active Directory Helper account is not acceptable. Specify the acceptable login accounts.',
										'Determine whether the Active Directory Helper account is acceptable')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 1, '''SERVICE_SPECIFIC_ACCOUNT''', '',
										'Is an unacceptable Active Directory Helper account being used?')
	end

	select @metricid = 68
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'Reporting Services Login Account Not Acceptable', 'Login', 1, 1, '', 'When enabled, this check will identify a risk if the Reporting Services account is not acceptable. Specify the acceptable login accounts.',
										'Determine whether the Reporting Services account is acceptable')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 1, '''SERVICE_SPECIFIC_ACCOUNT''', '',
										'Is an unacceptable Reporting Services account being used?')
	end

	select @metricid = 69
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'VSS Writer Login Account Not Acceptable', 'Login', 1, 1, '', 'When enabled, this check will identify a risk if the VSS Writer account is not acceptable. Specify the acceptable login accounts.',
										'Determine whether the VSS Writer account is acceptable')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 1, '''SERVICE_SPECIFIC_ACCOUNT''', '',
										'Is an unacceptable VSS Writer account being used?')
	end

	select @metricid = 70
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'VSS Writer Running', 'Configuration', 0, 0, '', 'When enabled, this check will identify a risk if the VSS Writer is running on the SQL Server.',
										'Determine whether VSS Writer is running on the SQL Server')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 0, 1, '', '',
										'Is VSS Writer running?')
	end

	select @metricid = 71
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'Unauthorized Accounts Are Sysadmins', 'Configuration', 1, 1, '', 'When enabled, this check will identify a risk if any unauthorized accounts are members of the sysadmin server role. Specify the unauthorized accounts. Can use ''%'' as wildcard.',
										'Determine whether unauthorized accounts have sysadmin privileges on the SQL Server')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 0, 2, 'UNAUTHORIZED_ADMIN_ACCT', '',
										'Do unauthorized accounts have sysadmin privileges?')
	end

	select @metricid = 72
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'sa Account Not Disabled', 'Configuration', 0, 0, '', 'When enabled, this check will identify a risk if the SQL Server sa account is enabled on SQL Server 2005 or later.',
										'Determine whether the SQL Server sa account has been disabled on SQL Server 2005 or later')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 1, '', '',
										'Is the SQL Server sa account enabled?')
	end

	select @metricid = 73
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'ALTER TRACE Permission Granted To Unauthorized Users', 'Permissions', 1, 1, '', 'When enabled, this check will identify a risk if any unauthorized accounts have been granted the ALTER TRACE permission on SQL Server 2005 or later. Specify which accounts are authorized to have this permission. Can use ''%'' as wildcard.',
										'Determine whether unauthorized users have been granted the ALTER TRACE permission on SQL Server 2005 or later')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 2, '''none''', '',
										'Do unauthorized users have the ALTER TRACE permission?')
	end

	select @metricid = 74
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'CONTROL SERVER Permission Granted To Unauthorized Users', 'Permissions', 1, 1, '', 'When enabled, this check will identify a risk if any unauthorized accounts have been granted the CONTROL SERVER permission on SQL Server 2005 or later. Specify which accounts are authorized to have this permission. Can use ''%'' as wildcard.',
										'Determine whether unauthorized users have been granted the CONTROL SERVER permission on SQL Server 2005 or later')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 0, 2, '''none''', '',
										'Do unauthorized users have the CONTROL SERVER permission?')
	end

	select @metricid = 75
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'xp_cmdshell Enabled', 'Configuration', 0, 0, '', 'When enabled, this check will identify a risk if xp_cmdshell is enabled.',
										'Determine whether the xp_cmdshell extended stored procedure is enabled on SQL Server 2005 or later')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 2, '', '',
										'Is xp_cmdshell enabled on the SQL Server?')
	end

	select @metricid = 76
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'Required Administrative Accounts Do Not Exist', 'Access', 1, 1, '', 'When enabled, this check will identify a risk if any required administrative accounts are missing from the SQL Server. Specify the required accounts.',
										'Determine whether the required administrative accounts exist on the SQL Server')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 1, '''sa''', '',
										'Do required administrative accounts exist?')
	end

	select @metricid = 77
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'sa Account Not Using Password Policy', 'Login', 0, 0, '', 'When enabled, this check will identify a risk if the sa account is not protected by Windows password policy.',
										'Determine whether password policy is enforced on the sa account')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 2, '', '',
										'Is password policy enforced on the sa account?')
	end

	select @metricid = 78
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'Database Files Missing Required Administrative Permissions', 'Permissions', 1, 1, '', 'When enabled, this security check will identify a risk if the required accounts do not have administrative permissions on all data files. Specify the required accounts. Can use ''%'' as wildcard.',
										'Determine whether the required administrative accounts have access to all database files')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 0, 1, '', '',    -- disabled by default, user must enter list of accounts
										'Do the required administrative permissions exist on database files?')
	end

	select @metricid = 79
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'Executable Files Missing Required Administrative Permissions', 'Permissions', 1, 1, '', 'When enabled, this security check will identify a risk if the required accounts do not have administrative permissions on all executable files. Specify the required accounts.  Can use ''%'' as wildcard.',
										'Determine whether the required administrative accounts have access to all executable files (any .exe or .dll file)')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 0, 1, '', '',    -- disabled by default, user must enter list of accounts
										'Do the required administrative permissions exist on executable files?')
	end

	select @metricid = 80
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'Registry Keys Missing Required Administrative Permissions', 'Permissions', 1, 1, '', 'When enabled, this security check will identify a risk if the required accounts do not have administrative permissions on all SQL Server registry keys. Specify the required accounts.  Can use ''%'' as wildcard.',
										'Determine whether the required administrative accounts have access to all SQL Server registry keys')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 0, 1, '', '',    -- disabled by default, user must enter list of accounts
										'Do the required administrative permissions exist on registry keys?')
	end

	-- NOTE: metricids 81-88 are the remaining 2.6 checks and may require schema and data collection changes
	select @metricid = 81
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'Data Files On System Drive', 'Configuration', 1, 1, '', 'When enabled, this check will identify a risk if unapproved data files are located on the system drive.  Specify the approved data files. Can use ''%'' as wildcard.',
										'Determine whether data files exist on the system drive')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 1, '''%\master.mdf'',''%\mastlog.ldf'',''%\msdbdata.mdf'',''%\msdblog.ldf'',''%\model.mdf'',''%\modellog.ldf'',''%\distmdl.%'',''%\mssqlsystemresource.%'',''%\tempdb.mdf'',''%\templog.ldf''', '',    -- disabled by default, user may enter a list of files
										'Are unapproved data files located on the system drive?')
	end

	select @metricid = 82
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'SQL Server Installation Directories On System Drive', 'Configuration', 1, 1, '', 'When enabled, this check will identify a risk if unapproved SQL Server installation directories exist on the system drive. Specify directories approved for the system drive. Can use "%" as wildcard.',
										'Determine whether SQL Server installation directories are on the system drive')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 1, '''C:\Program Files\Microsoft SQL Server\90\Shared'',''C:\Program Files\Microsoft SQL Server\100\Shared'',''C:\Program Files\Common Files\System\MSSearch\Bin'',''C:\Program Files (x86)\Microsoft SQL Server\90\Shared'',''C:\Program Files\Microsoft SQL Server\80\Tools\Binn''', '',    -- disabled by default because it is common to install it on the system drive
										'Are unapproved SQL Server installation directories on the system drive?')
	end

	select @metricid = 83
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'Ad Hoc Distributed Queries Enabled', 'Surface Area', 0, 0, '', 'When enabled, this check will identify a risk if Ad Hoc Distributed Queries are enabled on SQL Server 2005 or later.',
										'Determine whether Ad Hoc Distributed Queries are enabled on SQL Server 2005 or later')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 1, '', '',
										'Are Ad Hoc Distributed Queries enabled?')
	end

	select @metricid = 84
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'Unauthorized SQL Logins Exist', 'Login', 1, 1, '', 'When enabled, this check will identify a risk if any unauthorized SQL Logins exist on the SQL Server. Specify the authorized logins.',
										'Determine whether unauthorized SQL Logins have been created on the SQL Server')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 1, '''sa''', '',
										'Do unauthorized SQL Logins exist on the SQL Server?')
	end

	select @metricid = 85
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'Public Server Role Has Permissions', 'Permissions', 0, 0, '', 'When enabled, this check will identify a risk if  the public server role has been granted any permissions on SQL Server 2005 or later.',
										'Determine whether the public server role has been granted permissions')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 3, '', '',
										'Are any permissions granted to the public server role?')
	end

	select @metricid = 86
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'Databases Are Trustworthy', 'Configuration', 1, 1, '', 'When enabled, this check will identify a risk if any unapproved databases are trustworthy on SQL Server 2005 or later. Specify the approved databases.',
										'Determine whether any unapproved databases are trustworthy on SQL Server 2005 or later')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 0, 2, '''msdb''', '',
										'Is the trustworthy bit on for any unapproved databases?')
	end

	select @metricid = 87
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'Sysadmins Own Databases', 'Permissions', 1, 1, '', 'When enabled, this check will identify a risk if any unapproved databases are owned by system administrators. Specify the approved databases.',
										'Determine whether any databases are owned by a system administrator')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 2, '''master'',''msdb'',''model'',''tempdb''', '',
										'Are any unapproved databases owned by a system administrator?')
	end

	select @metricid = 88
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (@metricid, 'Sysadmins Own Trustworthy Databases', 'Access', 1, 1, '', 'When enabled, this check will identify a risk if any unapproved databases have the trustworthy bit set on and the owner has system administrator privileges on SQL Server 2005 or later. Specify the approved databases.',
										'Determine whether any trustworthy databases are owned by system administrators on SQL Server 2005 or later')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, @metricid, 1, 3, '''msdb''', '',
										'Are any unapproved trustworthy databases owned by a system administrator?')
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
