declare @ver int
SELECT @ver=schemaversion FROM currentversion
if (isnull(@ver, 900) <= 2799)	-- Check to prevent this from running in future upgrades, but run for all prior versions at any version level because the version gets updated later
BEGIN
	-- update default security checks
	declare @metricid int, @strval nvarchar(512)

	set @metricid = 16
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
        update
            policymetric
        set 
            severity = 3
        where
            policyid = 0
            and assessmentid = 0
            and metricid = @metricid
        if ( @ver is null )	-- this is a new install, so fix the All Servers policy
           update
            policymetric
           set
            severity = 3
           where
            policyid = 1
            and assessmentid = 1
            and metricid = @metricid
	end
 
 	set @metricid = 72
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
        update
            policymetric
        set 
            severity = 3
        where
            policyid = 0
            and assessmentid = 0
            and metricid = @metricid
        if ( @ver is null )	-- this is a new install, so fix the All Servers policy
           update
            policymetric
           set
            severity = 3
           where
            policyid = 1
            and assessmentid = 1
            and metricid = @metricid
	end   

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
                ,N'Symmetric key'
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
                ,N'Assembly host policy'
                ,N'Determine whether there are user defined assemblies with host policy other than SAFE'
                ,0
                ,0
                ,N''
                ,N'When enabled, this check will identify a risk if there are user defined assemblies with host policy other than SAFE'
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
                ,N'Is there are user defined assembiles with host policy other than SAFE?'
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
                ,N'Contained database authentication type'
                ,N'Determine whether authentication type set to Mixed mode with contained databases exists on instance'
                ,0
                ,0
                ,N''
                ,N'When enabled, this check will identify a risk if authentication type set to Mixed mode with contained databases exists on instance'
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
                ,N'Is authentication mode set to Windows Only on servers with contained databases?'
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
                ,N'Startup Stored Procedures permissions'
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
                  3,
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
                  3,
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
                  N'Have you check information about database roles and their memebers?',
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
                  N'Have you check information about server roles and their members?',
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
			          N'Unacceptable Database Ownership', -- metricname - nvarchar(256)
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
			          0, -- isenabled - bit
			          N'', -- reportkey - nvarchar(32)
			          N'Are there  databases with an unacceptable owner?', -- reporttext - nvarchar(4000)
			          1, -- severity - int
			          N'sa', -- severityvalues - nvarchar(4000)
			          0  -- assessmentid - int
			        )
	end   
        select
            @metricid = 103
        if not exists ( select
                            *
                        from
                            metric
                        where
                            metricid = @metricid ) 
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
                          N'Public role permissions', -- metricname - nvarchar(256)
                          N'Determine whether the public roles have permissions to user defined objects', -- metricdescription - nvarchar(1024)
                          0, -- isuserentered - bit
                          0, -- ismultiselect - bit
                          N'', -- validvalues - nvarchar(1024)
                          N'When enabled, this check will identify a risk if permissions have been granted to the public roles '  -- valuedescription - nvarchar(1024)									        
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
                          N'Is there objects with permissions granted to public role?', -- reporttext - nvarchar(4000)
                          1, -- severity - int
                          N'', -- severityvalues - nvarchar(4000)
                          0  -- assessmentid - int
                        )
           end
         select
            @metricid = 104
        if not exists ( select
                            *
                        from
                            metric
                        where
                            metricid = @metricid ) 
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
                          N'Configuration', -- metrictype - nvarchar(32)
                          N'CLR Enabled', -- metricname - nvarchar(256)
                          N'Determine whether the CLR is Enabled on the server', -- metricdescription - nvarchar(1024)
                          0, -- isuserentered - bit
                          0, -- ismultiselect - bit
                          N'', -- validvalues - nvarchar(1024)
                          N'When enabled, this check will identify a risk if CLR is enabled on the server'  -- valuedescription - nvarchar(1024)									        
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
                          N'Is there CLR Enabled on the server?', -- reporttext - nvarchar(4000)
                          1, -- severity - int
                          N'', -- severityvalues - nvarchar(4000)
                          0  -- assessmentid - int
                        )
           end      
        select
            @metricid = 105
        if not exists ( select
                            *
                        from
                            metric
                        where
                            metricid = @metricid ) 
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
                          N'Configuration', -- metrictype - nvarchar(32)
                          N'Default Trace Enabled', -- metricname - nvarchar(256)
                          N'Determine whether the Default Trace Enabled on the server', -- metricdescription - nvarchar(1024)
                          0, -- isuserentered - bit
                          0, -- ismultiselect - bit
                          N'', -- validvalues - nvarchar(1024)
                          N'When enabled, this check will identify a risk if Default Trace is disabled on the server'  -- valuedescription - nvarchar(1024)									        
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
                          N'Is there Default Trace disabled on the server?', -- reporttext - nvarchar(4000)
                          1, -- severity - int
                          N'', -- severityvalues - nvarchar(4000)
                          0  -- assessmentid - int
                        )
           end 
        select
            @metricid = 106
        if not exists ( select
                            *
                        from
                            metric
                        where
                            metricid = @metricid ) 
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
                          N'Configuration', -- metrictype - nvarchar(32)
                          N'Maximum number of error log files', -- metricname - nvarchar(256)
                          N'Determine whether the Maximum number of error log files is more than 11', -- metricdescription - nvarchar(1024)
                          0, -- isuserentered - bit
                          0, -- ismultiselect - bit
                          N'', -- validvalues - nvarchar(1024)
                          N'When enabled, this check will identify a risk if Maximum number of error log files is less than 12'  -- valuedescription - nvarchar(1024)									        
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
                          N'Is maximum number of error log files is grater than or equal to 12?', -- reporttext - nvarchar(4000)
                          1, -- severity - int
                          N'', -- severityvalues - nvarchar(4000)
                          0  -- assessmentid - int
                        )
           end 
		select @metricid = 107
        if not exists ( select
                            *
                        from
                            metric
                        where
                            metricid = @metricid ) 
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
                          N'Login', -- metrictype - nvarchar(32)
                          N'Orphaned users', -- metricname - nvarchar(256)
                          N'Determine whether any orphaned users exist in databases.', -- metricdescription - nvarchar(1024)
                          1, -- isuserentered - bit
                          1, -- ismultiselect - bit
                          N'''MS_DataCollectorInternalUser''', -- validvalues - nvarchar(1024)
                          N'When enabled, this check will identify a risk if there any orphaned user exists.'  -- valuedescription - nvarchar(1024)									        
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
                          N'Is there any orphaned users?', -- reporttext - nvarchar(4000)
                          1, -- severity - int
                          N'''MS_DataCollectorInternalUser''', -- severityvalues - nvarchar(4000)
                          0  -- assessmentid - int
                        )
           end 
		select @metricid = 108
        if not exists ( select
                            *
                        from
                            metric
                        where
                            metricid = @metricid ) 
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
                          N'Configuration', -- metrictype - nvarchar(32)
                          N'Ole automation procedures', -- metricname - nvarchar(256)
                          N'Determine whether the Ole automation procedures are enabled.', -- metricdescription - nvarchar(1024)
                          0, -- isuserentered - bit
                          0, -- ismultiselect - bit
                          N'', -- validvalues - nvarchar(1024)
                          N'When enabled, this check will identify a risk if Ole automation procedures are enabled on the SQL Server.'  -- valuedescription - nvarchar(1024)									        
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
                          N'Is there Ole automation procedures enabled on the server?', -- reporttext - nvarchar(4000)
                          1, -- severity - int
                          N'', -- severityvalues - nvarchar(4000)
                          0  -- assessmentid - int
                        )
           end 
		select @metricid = 109
        if not exists ( select
                            *
                        from
                            metric
                        where
                            metricid = @metricid ) 
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
                          N'Configuration', -- metrictype - nvarchar(32)
                          N'Common criteria compliance', -- metricname - nvarchar(256)
                          N'Determine whether the Common criteria compliance is enabled.', -- metricdescription - nvarchar(1024)
                          0, -- isuserentered - bit
                          0, -- ismultiselect - bit
                          N'', -- validvalues - nvarchar(1024)
                          N'When enabled, this check will identify a risk if Common criteria compliance is disabled on the SQL Server.'  -- valuedescription - nvarchar(1024)									        
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
                          N'Is there Common criteria compliance disabled on the server?', -- reporttext - nvarchar(4000)
                          1, -- severity - int
                          N'', -- severityvalues - nvarchar(4000)
                          0  -- assessmentid - int
                        )
           end 
		   select @metricid = 110
        if not exists ( select
                            *
                        from
                            metric
                        where
                            metricid = @metricid ) 
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
                          N'Permissions', -- metrictype - nvarchar(32)
                          N'Integration Services Users Permissions Not Acceptable', -- metricname - nvarchar(256)
                          N'Determine whether unapproved users have been granted permissions on an Integration Services stored procedure.', -- metricdescription - nvarchar(1024)
                          1, -- isuserentered - bit
                          1, -- ismultiselect - bit
                          N'', -- validvalues - nvarchar(1024)
                          N'When enabled, this check will identify a risk if an unapproved user has been granted permissions on an Integration Services stored procedure. Specify the stored procedures.'  -- valuedescription - nvarchar(1024)									        
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
                          N'Do unapproved users have permissions on SSIS stored procedures?', -- reporttext - nvarchar(4000)
                          1, -- severity - int
                          N'''sp_add_dtspackage'',''sp_drop_dtspackage'',''sp_dts_addfolder'',''sp_dts_addlogentry'',''sp_dts_checkexists'',''sp_dts_deletefolder'',''sp_dts_deletepackage'',''sp_dts_getfolder'',''sp_dts_getpackage'',''sp_dts_getpackageroles'',''sp_dts_listfolders'',''sp_dts_listpackages'',''sp_dts_putpackage'',''sp_dts_renamefolder'',''sp_dts_setpackageroles'',''sp_dump_dtslog_all'',''sp_dump_dtspackagelog'',''sp_dump_dtssteplog'',''sp_dump_dtstasklog'',''sp_enum_dtspackagelog'',''sp_enum_dtspackages'',''sp_enum_dtssteplog'',''sp_enum_dtstasklog'',''sp_get_dtspackage'',''sp_get_dtsversion'',''sp_log_dtspackage_begin'',''sp_log_dtspackage_end'',''sp_log_dtsstep_begin'',''sp_log_dtsstep_end'',''sp_log_dtstask'',''sp_make_dtspackagename'',''sp_reassign_dtspackageowner'',''sp_ssis_addfolder'',''sp_ssis_addlogentry'',''sp_ssis_checkexists'',''sp_ssis_deletefolder'',''sp_ssis_deletepackage'',''sp_ssis_getfolder'',''sp_ssis_getpackage''', -- severityvalues - nvarchar(4000)
                          0  -- assessmentid - int
                        )
		   end
		   
		   
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
		   
	select @metricid = 24
		if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
		begin
			update policymetric 
				set isenabled=1,severity=2,severityvalues='''xp_cmdshell'',''xp_available_media'',''xp_dirtree'',''xp_dsninfo'',''xp_enumdsn'',''xp_enumerrorlogs'',''xp_enumgroups'',''xp_eventlog'',''xp_fixeddrives'',''xp_getfiledetails'',''xp_getnetname'',''xp_logevent'',''xp_loginconfig'',''xp_msver'',''xp_readerrorlog'',''xp_servicecontrol'',''xp_sprintf'',''xp_sscanf'',''xp_subdirs'',''xp_deletemail'',''xp_findnextmsg'',''xp_get_mapi_default_profile'',''xp_get_mapi_profiles'',''xp_readmail'',''xp_sendmail'',''xp_startmail'',''xp_stopmail'',''xp_cleanupwebtask'',''xp_convertwebtask'',''xp_dropwebtask'',''xp_enumcodepages'',''xp_makewebtask'',''xp_readwebtask'',''xp_runwebtask'',''sp_OACreate'',''xp_regaddmultistring'',''xp_regdeletekey'',''xp_regdeletevalue'',''xp_regenumvalues'',''xp_regremovemultistring'',''xp_regwrite'',''xp_regread'''
				where metricid = @metricid
			if (@ver is null)	-- this is a new install, so fix the All Servers policy
				update policymetric 
					set isenabled=1,severity=2,severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
					where metricid = @metricid
		end
	select @metricid = 4
		if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
		begin
			update policymetric 
				set severityvalues='''All'''
				where metricid = @metricid
			if (@ver is null)	-- this is a new install, so fix the All Servers policy
				update policymetric 
					set isenabled=1,severity=1,severityvalues= '''All''', reportkey='', reporttext ='Is the login auditing configuration acceptable?'
					where metricid = @metricid
	end 

	select @metricid = 91
		if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
		begin
		 update metric  
			SET [metrictype] = 'Permissions'
			  ,[metricname] = 'Integration Services Roles Permissions Not Acceptable'
			  ,[metricdescription] = 'Determine whether unapproved roles have been granted permissions on an Integration Services stored procedure.'
			  ,[valuedescription] = 'When enabled, this check will identify a risk if an unapproved role has been granted permissions on an Integration Services stored procedure. Specify the stored procedures.'
			where metricid = @metricid     

			update policymetric 
				set reporttext = 'Do unapproved roles have permissions on SSIS stored procedures?'
				where metricid = @metricid
			if (@ver is null)	-- this is a new install, so fix the All Servers policy
				update policymetric 
					set isenabled=1, severity=1, severityvalues= N'''sp_add_dtspackage'',''sp_drop_dtspackage'',''sp_dts_addfolder'',''sp_dts_addlogentry'',''sp_dts_checkexists'',''sp_dts_deletefolder'',''sp_dts_deletepackage'',''sp_dts_getfolder'',''sp_dts_getpackage'',''sp_dts_getpackageroles'',''sp_dts_listfolders'',''sp_dts_listpackages'',''sp_dts_putpackage'',''sp_dts_renamefolder'',''sp_dts_setpackageroles'',''sp_dump_dtslog_all'',''sp_dump_dtspackagelog'',''sp_dump_dtssteplog'',''sp_dump_dtstasklog'',''sp_enum_dtspackagelog'',''sp_enum_dtspackages'',''sp_enum_dtssteplog'',''sp_enum_dtstasklog'',''sp_get_dtspackage'',''sp_get_dtsversion'',''sp_log_dtspackage_begin'',''sp_log_dtspackage_end'',''sp_log_dtsstep_begin'',''sp_log_dtsstep_end'',''sp_log_dtstask'',''sp_make_dtspackagename'',''sp_reassign_dtspackageowner'',''sp_ssis_addfolder'',''sp_ssis_addlogentry'',''sp_ssis_checkexists'',''sp_ssis_deletefolder'',''sp_ssis_deletepackage'',''sp_ssis_getfolder'',''sp_ssis_getpackage'''
					where metricid = @metricid
		end
				  
	-- note: the following uses the @metricid to determine the ending value for the metrics to apply to all of the policies


-- Adde new filter type for sequence objets
insert  into dbo.objecttype
        (
          objecttype,
          objecttypename,
          hashkey
        )
values
        (
          N'SO', -- objecttype - nvarchar(5)
          N'Sequence Object', -- objecttypename - nvarchar(500)
          N'' 
        )
insert  into dbo.filterruleclass
        ( objectclass, objectvalue )
values
        ( 48, -- objectclass - int
          N'Sequence Objects'  -- objectvalue - nvarchar(128)
          )
			  end
 if exists (select 1 from dbo.metric where metricid=71)
	begin 
		update dbo.metric
		set valuedescription='When enabled, this check will identify a risk if any unauthorized accounts are members of the sysadmin server role. Specify the authorized accounts. Can use ''%'' as wildcard.'
		where metricid=71
	end
insert into dbo.classtype
        ( classid, classname, hashkey )
values
        ( 108, -- classid - int
          N'Availability Group', -- classname - nvarchar(500)
          null -- hashkey - nvarchar(256)
          )
go