declare @ver int
SELECT @ver=schemaversion FROM currentversion
if (isnull(@ver, 900) <= 2799)	-- Check to prevent this from running in future upgrades, but run for all prior versions at any version level because the version gets updated later
BEGIN
	-- update default security checks
	declare @metricid int, @strval nvarchar(512)

	---- ADD NEW SECURITY CHECKS ***************************************************************


	declare @startmetricid int
	select  @startmetricid = 93
	
SET @metricid = 93
IF NOT EXISTS ( SELECT  *
                FROM    metric
                WHERE   metricid = @metricid ) 
   BEGIN
         INSERT [dbo].[metric]
                (
                 [metricid]
                ,[metrictype]
                ,[metricname]
                ,[metricdescription]
                ,[isuserentered]
                ,[ismultiselect]
                ,[validvalues]
                ,[valuedescription]
                )
         VALUES (
                 @metricid
                ,N'Access'
                ,N'Symetric key'
                ,N'Determine whether master, msdb, model or tempdb have user-created symmetric keys'
                ,0
                ,0
                ,N''
                ,N'When enabled, this check will identify a risk if master, msdb, model or tempdb have user-created symmetric keys'
                )
         INSERT [dbo].[policymetric]
                (
                 [policyid]
                ,[metricid]
                ,[isenabled]
                ,[reportkey]
                ,[reporttext]
                ,[severity]
                ,[severityvalues]
                ,[assessmentid]
                )
         VALUES (
                 0
                ,@metricid
                ,1
                ,N''
                ,N'Does master, msdb, model or tempdb have user-created symmetric keys? '
                ,3
                ,N''
                ,0
                )
   END
SET @metricid = 94
IF NOT EXISTS ( SELECT  *
                FROM    metric
                WHERE   metricid = @metricid ) 
   BEGIN
         INSERT [dbo].[metric]
                (
                 [metricid]
                ,[metrictype]
                ,[metricname]
                ,[metricdescription]
                ,[isuserentered]
                ,[ismultiselect]
                ,[validvalues]
                ,[valuedescription]
                )
         VALUES (
                 @metricid
                ,N'Access'
                ,N'Assambly host policy'
                ,N'Determine whether there are user defined assembies with host policy other than SAFE'
                ,0
                ,0
                ,N''
                ,N'When enabled, this check will identify a risk if there are user defined assembies with host policy other than SAFE'
                )
         INSERT [dbo].[policymetric]
                (
                 [policyid]
                ,[metricid]
                ,[isenabled]
                ,[reportkey]
                ,[reporttext]
                ,[severity]
                ,[severityvalues]
                ,[assessmentid]
                )
         VALUES (
                 0
                ,@metricid
                ,1
                ,N''
                ,N'Is there are user defined assambiles with host policy other than SAFE?'
                ,3
                ,N''
                ,0
                )
   END
SET @metricid = 95
IF NOT EXISTS ( SELECT  *
                FROM    metric
                WHERE   metricid = @metricid ) 
   BEGIN
         INSERT [dbo].[metric]
                (
                 [metricid]
                ,[metrictype]
                ,[metricname]
                ,[metricdescription]
                ,[isuserentered]
                ,[ismultiselect]
                ,[validvalues]
                ,[valuedescription]
                )
         VALUES (
                 @metricid
                ,N'Access'
                ,N'Contained database authentification type'
                ,N'Determine whether Authentification type set to Moxed mode with contained databases exists on instance'
                ,0
                ,0
                ,N''
                ,N'When enabled, this check will identify a risk if Authentification type set to Moxed mode with contained databases exists on instance'
                )
         INSERT [dbo].[policymetric]
                (
                 [policyid]
                ,[metricid]
                ,[isenabled]
                ,[reportkey]
                ,[reporttext]
                ,[severity]
                ,[severityvalues]
                ,[assessmentid]
                )
         VALUES (
                 0
                ,@metricid
                ,1
                ,N''
                ,N'Is Authentification mode set to Windows Only on servers with contained databases?'
                ,3
                ,N''
                ,0
                )
   END
SET @metricid = 96
IF NOT EXISTS ( SELECT  *
                FROM    metric
                WHERE   metricid = @metricid ) 
   BEGIN
         INSERT [dbo].[metric]
                (
                 [metricid]
                ,[metrictype]
                ,[metricname]
                ,[metricdescription]
                ,[isuserentered]
                ,[ismultiselect]
                ,[validvalues]
                ,[valuedescription]
                )
         VALUES (
                 @metricid
                ,N'Access'
                ,N'Startup stored procedures permissions'
                ,N'Determine whether startup stored procedures can be run or are owned by accounts without sysadmin permissions '
                ,0
                ,0
                ,N''
                ,N'When enabled, this check will identify a risk if there are startup stored procedures can be run or are owned by accounts without sysadmin permissions'
                )
         INSERT [dbo].[policymetric]
                (
                 [policyid]
                ,[metricid]
                ,[isenabled]
                ,[reportkey]
                ,[reporttext]
                ,[severity]
                ,[severityvalues]
                ,[assessmentid]
                )
         VALUES (
                 0
                ,@metricid
                ,1
                ,N''
                ,N'Are there startup stored procedures that can be run or are owned by accounts without sysadmin permissions?'
                ,3
                ,N''
                ,0
                )
   END
SET @metricid = 97
IF NOT EXISTS ( SELECT  *
                FROM    metric
                WHERE   metricid = @metricid ) 
   BEGIN
         INSERT [dbo].[metric]
                (
                 [metricid]
                ,[metrictype]
                ,[metricname]
                ,[metricdescription]
                ,[isuserentered]
                ,[ismultiselect]
                ,[validvalues]
                ,[valuedescription]
                )
         VALUES (
                 @metricid
                ,N'Access'
                ,N'SQL Job permissions'
                ,N'Determine whether SQL Server Agent account or job proxies are members of local Administrators group'
                ,1
                ,1
                ,N'''ActiveScripting'',''CmdExec'',''SSIS'',''PowerShell'''
                ,N'When enabled, this check will identify a risk if there are SQL Server Agent account or job proxies that are in Administrators group'
                )
         INSERT [dbo].[policymetric]
                (
                 [policyid]
                ,[metricid]
                ,[isenabled]
                ,[reportkey]
                ,[reporttext]
                ,[severity]
                ,[severityvalues]
                ,[assessmentid]
                )
         VALUES (
                 0
                ,@metricid
                ,1
                ,N''
                ,N'Are there SQL Server Agent job steps configured using with defined subsystems ?'
                ,3
                ,N'''ActiveScripting'',''CmdExec'',''SSIS'',''PowerShell'''
                ,0
                )
   END


	set @metricid = 98
	if not exists (select * from metric where metricid = @metricid)
	begin
        insert  dbo.metric
                (
                  metricid,
                  metrictype,
                  metricname,
                  metricdescription,
                  isuserentered,
                  ismultiselect,
                  validvalues,
                  valuedescription
				)
        values
                (
                  @metricid,
                  N'Login',
                  N'DISTRIBUTOR_ADMIN Login',
                  N'Determine whether DISTRIBUTOR_ADMIN account should be deleted.',
                  0,
                  0,
                  N'',
                  N'When enabled, this check will identify a risk if the SQL Server  DISTRIBUTOR_ADMIN account hasn''t been deleted if server is not distributor or publisher.'
				)

        insert  dbo.policymetric
                (
                  policyid,
                  metricid,
                  isenabled,
                  reportkey,
                  reporttext,
                  severity,
                  severityvalues,
                  assessmentid
				)
        values
                (
                  0,
                  @metricid,
                  1,
                  N'',
                  N'Does the DISTRIBUTOR_ADMIN account exist?',
                  2,
                  N'',
                  0
				)


	end

	set @metricid = 99
	if not exists (select * from metric where metricid = @metricid)
    begin
          insert    dbo.metric
                    (
                      metricid,
                      metrictype,
                      metricname,
                      metricdescription,
                      isuserentered,
                      ismultiselect,
                      validvalues,
                      valuedescription
			        )
          values
                    (
                      @metricid,
                      N'Login',
                      N'SQL Server SYSADMIN accounts',
                      N'Determine whether SQL SYSADMIN accounts that are in the local Administrator role for the physical server.',
                      1,
                      1,
                      N'''%\Administrator''',
                      N'When enabled, this check will identify a risk if SQL SYSADMIN account is in the local Administrator role for the physical server.'
			        )

        insert  dbo.policymetric
                (
                  policyid,
                  metricid,
                  isenabled,
                  reportkey,
                  reporttext,
                  severity,
                  severityvalues,
                  assessmentid
				)
        values
                (
                  0,
                  @metricid,
                  1,
                  N'',
                  N'Is SQL SYSADMIN account in the local Administrator role?',
                  2,
                  N'''%\Administrator''',
                  0
				)
    end   

	select @metricid = 100
	if not exists (select * from metric where metricid = @metricid)
	begin
		insert  dbo.metric
				(
				  metricid,
				  metrictype,
				  metricname,
				  metricdescription,
				  isuserentered,
				  ismultiselect,
				  validvalues,
				  valuedescription
				)
		values
				(
				  @metricid,
				  N'Access',
				  N'Database roles and members',
				  N'Shows information about database roles and their members',
				  0,
				  0,
				  N'',
				  N'When enabled, this check will list database, database role, corresponding members, login type, windows group, permissions.'
				)

        insert  dbo.policymetric
                (
                  policyid,
                  metricid,
                  isenabled,
                  reportkey,
                  reporttext,
                  severity,
                  severityvalues,
                  assessmentid
				)
        values
                (
                  0,
                  @metricid,
                  1,
                  N'',
                  N'Have you check infromation about database roles and their memebers?',
                  2,
                  N'',
                  0
				)
	end   

	select @metricid = 101
	if not exists (select * from metric where metricid = @metricid)
	begin
 
        insert  dbo.metric
                (
                  metricid,
                  metrictype,
                  metricname,
                  metricdescription,
                  isuserentered,
                  ismultiselect,
                  validvalues,
                  valuedescription
				)
        values
                (
                  @metricid,
                  N'Access',
                  N'Server roles and members',
                  N'Shows information about server roles and their members',
                  0,
                  0,
                  N'',
                  N'When enabled, this check will list SQL instance, role, corresponding members, login type, windows group, disabled.'
				)
			
			    
        insert  dbo.policymetric
                (
                  policyid,
                  metricid,
                  isenabled,
                  reportkey,
                  reporttext,
                  severity,
                  severityvalues,
                  assessmentid
				)
        values
                (
                  0,
                  @metricid,
                  1,
                  N'',
                  N'Have you check infromation about server roles and their members?',
                  2,
                  N'',
                  0
				)    
	end   


	select @metricid = 102
	if not exists (select * from metric where metricid = @metricid)
	begin
 
		     insert into dbo.metric
			        (
			          metricid,
			          metrictype,
			          metricname,
			          metricdescription,
			          isuserentered,
			          ismultiselect,
			          validvalues,
			          valuedescription
			        )
			values
			        (
			          @metricid, -- metricid - int
			          N'Access', -- metrictype - nvarchar(32)
			          N' Unacceptable Database Owneship', -- metricname - nvarchar(256)
			          N'Determine whether if a database is found with an unacceptable owner', -- metricdescription - nvarchar(1024)
			          1, -- isuserentered - bit
			          1, -- ismultiselect - bit
			          N'', -- validvalues - nvarchar(1024)
			          N'When enabled, this check will identify a risk if a database is found with an unacceptable owner.'  -- valuedescription - nvarchar(1024)
			        )

			insert into dbo.policymetric
			        (
			          policyid,
			          metricid,
			          isenabled,
			          reportkey,
			          reporttext,
			          severity,
			          severityvalues,
			          assessmentid
			        )
			values
			        (
			          0, -- policyid - int
			          @metricid, -- metricid - int
			          1, -- isenabled - bit
			          N'', -- reportkey - nvarchar(32)
			          N'Are there  databases with an unacceptable owner?', -- reporttext - nvarchar(4000)
			          1, -- severity - int
			          N'sa', -- severityvalues - nvarchar(4000)
			          0  -- assessmentid - int
			        )
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


			  end
go