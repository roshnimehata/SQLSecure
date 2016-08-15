DECLARE @ver INT;
SELECT
    @ver = schemaversion
FROM
    currentversion;
IF ( ISNULL(@ver, 900) <= 2999 )	-- Check to prevent this from running in future upgrades, but run for all prior versions at any version level because the version gets updated later
    BEGIN
        DECLARE
            @metricid INT ,
            @strval NVARCHAR(512)

        DECLARE @startmetricid INT
        SELECT
            @startmetricid = 111


---------------------Health checks-----------------------------------------------------------------------------------------------------------
-----SQL Server Browser Running
        IF ( EXISTS ( SELECT
                        1
                      FROM
                        metric
                      WHERE
                        metricid = 28 ) )
            BEGIN
                UPDATE
                    metric
                SET
                    metricname = 'SQL Server Browser Running'
                WHERE
                    metricid = 28;
            END;
        ELSE
            INSERT  INTO metric
                    (
                      metricid ,
                      metricname ,
                      metrictype ,
                      isuserentered ,
                      ismultiselect ,
                      validvalues ,
                      valuedescription ,
                      metricdescription
                    )
            VALUES
                    (
                      28 ,
                      'SQL Server Browser Running' ,
                      'Surface Area' ,
                      0 ,
                      0 ,
                      '' ,
                      'When enabled, this check will identify a risk if SQL Server is visible for browsing from client computers.' ,
                      'Determine whether the SQL Server is hidden from client computers'
                    );
    
-----SQL Server Version (https://support.microsoft.com/en-us/kb/321185 https://support.microsoft.com/en-us/kb/957826)
--	Current			-		New
--	''				-	'13.0.1601'	(SQL Server 2016 Mainstream Support)
--	''				-	'12.0.2000'	(SQL Server 2014 Mainstream Support)
--	'11.0.2100'		-	'11.0.2100'	SQL Server 2012 - supported
--	'10.50.2500'	-	''			SQL Server 2008 R2 - not supported. extended support only
--	'10.0.5500'		-	''			SQL Server 2008 - not supported. extended support only
--	'9.00.5000'		-	''			SQL Server 2005 - not supported
--	'8.00.2039'		-	''			SQL Server 2000 - not supported
        SELECT
            @metricid = 2;
        IF EXISTS ( SELECT
                        *
                    FROM
                        policymetric
                    WHERE
                        policyid = 0
                        AND assessmentid = 0
                        AND metricid = @metricid )
            BEGIN
                UPDATE
                    policymetric
                SET
                    severityvalues = N'''11.0.2100'',''12.0.2000'',''13.0.1601'''
                WHERE
                    policyid = 0
                    AND assessmentid = 0
                    AND metricid = @metricid;
                IF ( @ver IS NULL )	-- this is a new install, so fix the All Servers policy
                    UPDATE
                        policymetric
                    SET
                        severityvalues = (
                                           SELECT
                                            severityvalues
                                           FROM
                                            policymetric
                                           WHERE
                                            policyid = 0
                                            AND assessmentid = 0
                                            AND metricid = @metricid
                                         )
                    WHERE
                        policyid = 1
                        AND assessmentid = 1
                        AND metricid = @metricid;
            END;
				--- new versions for Operation System checks.
        SELECT
            @metricid = 39;
        IF EXISTS ( SELECT
                        *
                    FROM
                        policymetric
                    WHERE
                        policyid = 0
                        AND assessmentid = 0
                        AND metricid = @metricid )
            BEGIN
                UPDATE
                    policymetric
                SET
                    severityvalues = N'''Microsoft Windows Server 2008 R2 Enterprise , Service Pack 1'',''Microsoft Windows Server 2008 R2 Standard , Service Pack 1'',''Microsoft Windows Web Server 2008 R2 , Service Pack 1'',''Microsoft® Windows Server® 2008 Enterprise , Service Pack 2'',''Microsoft® Windows Server® 2008 Enterprise without Hyper-V , Service Pack 2'',''Microsoft® Windows Server® 2008 Datacenter , Service Pack 2'',''Microsoft® Windows Server® 2008 Datacenter without Hyper-V , Service Pack 2'',''Microsoft® Windows Server® 2008 Standard , Service Pack 2'',''Microsoft® Windows Server® 2008 Standard without Hyper-V , Service Pack 2'',''Microsoft® Windows® Web Server 2008 , Service Pack 2'',''Microsoft® Windows® 2012 Datacenter'',''Microsoft® Windows® 2012 Essentials'',''Microsoft® Windows® 2012 Foundation'',''Microsoft® Windows® 2012 R2 Datacenter'',''Microsoft® Windows® 2012 R2 Essentials'',''Microsoft® Windows® 2012 R2 Foundation'',''Microsoft® Windows® 2012 R2 Standard'',''Microsoft® Windows® 2012 Standard'''
                WHERE
                    policyid = 0
                    AND assessmentid = 0
                    AND metricid = @metricid;
                IF ( @ver IS NULL )	-- this is a new install, so fix the All Servers policy
                    UPDATE
                        policymetric
                    SET
                        severityvalues = (
                                           SELECT
                                            severityvalues
                                           FROM
                                            policymetric
                                           WHERE
                                            policyid = 0
                                            AND assessmentid = 0
                                            AND metricid = @metricid
                                         )
                    WHERE
                        policyid = 1
                        AND assessmentid = 1
                        AND metricid = @metricid;
            END;


-----Other General Domain Accounts Check
        SELECT
            @metricid = 111;
        IF NOT EXISTS ( SELECT
                            *
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
                          N'Other General Domain Accounts' , -- metricname - nvarchar(256)
                          N'Determine whether general domain accounts added to the instance.' , -- metricdescription - nvarchar(1024)
                          0 , -- isuserentered - bit
                          0 , -- ismultiselect - bit
                          N'' , -- validvalues - nvarchar(1024)
                          N'When enabled, this check will identify a risk if  general domain accounts added to the instance.'  -- valuedescription - nvarchar(1024)									        
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
                          N'Is there any general domain accounts added to the instance?' , -- reporttext - nvarchar(4000)
                          3 , -- severity - int
                          N'' , -- severityvalues - nvarchar(4000)
                          0  -- assessmentid - int
                        );
            END;


-----SQL Jobs and Agent Check
        SELECT
            @metricid = 112;
        IF NOT EXISTS ( SELECT
                            *
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
                          N'SQL Jobs and Agent' , -- metricname - nvarchar(256)
                          N'Determine whether job steps are running on behalf of proxy account.' , -- metricdescription - nvarchar(1024)
                          0 , -- isuserentered - bit
                          0 , -- ismultiselect - bit
                          N'' , -- validvalues - nvarchar(1024)
                          N'When enabled, this check will identify a risk if job step has no proxy account.'  -- valuedescription - nvarchar(1024)									        
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
                          N'Is there any job step that is running without proxy account?' , -- reporttext - nvarchar(4000)
                          3 , -- severity - int
                          N'' , -- severityvalues - nvarchar(4000)
                          0  -- assessmentid - int
                        );
            END;


---Encryption Check

        SELECT
            @metricid = 113;
        IF NOT EXISTS ( SELECT
                            *
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
                          N'Encryption Methods' , -- metricname - nvarchar(256)
                          N'Determine whether there are encryption keys with algorithms other than supported.' , -- metricdescription - nvarchar(1024)
                          1 , -- isuserentered - bit
                          1 , -- ismultiselect - bit
                          N'''AES_128'',''AES_192'',''AES_256''' , -- validvalues - nvarchar(1024)
                          N'When enabled, this check will identify a risk if there are encryption keys with encryption methods other than selected.'  -- valuedescription - nvarchar(1024)									        
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
                          N'Is there any encryption keys with algorithms other than selected?' , -- reporttext - nvarchar(4000)
                          3 , -- severity - int
                          N'''AES_128'',''AES_192'',''AES_256''' , -- severityvalues - nvarchar(4000)
                          0  -- assessmentid - int
                        );
            END;

	--Certificates check
        SELECT
            @metricid = 114
        IF NOT EXISTS ( SELECT
                            *
                        FROM
                            metric
                        WHERE
                            metricid = @metricid )
            BEGIN
                INSERT  INTO metric
                        (
                          metricid ,
                          metricname ,
                          metrictype ,
                          isuserentered ,
                          ismultiselect ,
                          validvalues ,
                          valuedescription ,
                          metricdescription
                        )
                VALUES
                        (
                          @metricid ,
                          'Certificate private keys were never exported' ,
                          'Access' ,
                          0 , --isuserentered
                          0 , --ismultiselect
                          N'' ,
                          'When enabled, this check will identify a risk if certtificate private keys were not exported' ,
                          'Determine whether certificate private keys were not exported'
                        )


                INSERT  INTO policymetric
                        (
                          policyid ,
                          assessmentid ,
                          metricid ,
                          isenabled ,
                          severity ,
                          severityvalues ,
                          reportkey ,
                          reporttext
                        )
                VALUES
                        (
                          0 , --policyid
                          0 ,--assesmentid
                          @metricid ,
                          1 , --isenabled
                          3 , --severity
                          N'' , --severityvalues
                          N'' ,--reportkey
                          N'Does certificate private keys were exported?' --reporttext
                        )
            END
		--Linked server security check
		SELECT
            @metricid = 115
        IF NOT EXISTS ( SELECT
                            *
                        FROM
                            metric
                        WHERE
                            metricid = @metricid )
            BEGIN
                INSERT  INTO metric
                        (
                          metricid ,
                          metricname ,
                          metrictype ,
                          isuserentered ,
                          ismultiselect ,
                          validvalues ,
                          valuedescription ,
                          metricdescription
                        )
                VALUES
                        (
                          @metricid ,
                          'Linked servers are configured' ,
                          'Configuration' ,
                          0 , --isuserentered
                          0 , --ismultiselect
                          N'' ,
                          'When enabled, this check will identify a risk if there are configured linked servers' ,
                          'Determine whether linked servers are configured'
                        )


                INSERT  INTO policymetric
                        (
                          policyid ,
                          assessmentid ,
                          metricid ,
                          isenabled ,
                          severity ,
                          severityvalues ,
                          reportkey ,
                          reporttext
                        )
                VALUES
                        (
                          0 , --policyid
                          0 ,--assesmentid
                          @metricid ,
                          1 , --isenabled
                          2 , --severity
                          N'' , --severityvalues
                          N'' ,--reportkey
                          N'Does linked servers are configured?' --reporttext
                        )
            END

		--Linked server users security check
		SELECT
            @metricid = 116
        IF NOT EXISTS ( SELECT
                            *
                        FROM
                            metric
                        WHERE
                            metricid = @metricid )
            BEGIN
                INSERT  INTO metric
                        (
                          metricid ,
                          metricname ,
                          metrictype ,
                          isuserentered ,
                          ismultiselect ,
                          validvalues ,
                          valuedescription ,
                          metricdescription
                        )
                VALUES
                        (
                          @metricid ,
                          'Linked server is running as a member of sysadmin group' ,
                          'Access' ,
                          0 , --isuserentered
                          0 , --ismultiselect
                          N'' ,
                          'When enabled, this check will identify a risk if thara linked servers that are running as a member of sysadmin group' ,
                          'Determine whether linked servers are running as a member of sysadmin group'
                        )


                INSERT  INTO policymetric
                        (
                          policyid ,
                          assessmentid ,
                          metricid ,
                          isenabled ,
                          severity ,
                          severityvalues ,
                          reportkey ,
                          reporttext
                        )
                VALUES
                        (
                          0 , --policyid
                          0 ,--assesmentid
                          @metricid ,
                          1 , --isenabled
                          3 , --severity
                          N'' , --severityvalues
                          N'' ,--reportkey
                          N'Does linked servers are running as a member of sysadmin group?' --reporttext
                        )
            END

---------------------Update Policy Metrics with new checks--------------------------------------------------------------------------------------------------------

        IF (
             @ver IS NULL	-- this is a new install, so fix the All Servers policy to use the default values for the new security checks
             AND NOT EXISTS ( SELECT
                                *
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



-----Unauthorized Accounts Check
		UPDATE metric 
		SET
			metricname = 'Unauthorized Account Check', 
			valuedescription = 'When enabled, this check will identify a risk if any unauthorized accounts are members of the sysadmin server role or extended SoD roles. Specify the unauthorized accounts. Can use ''%'' as wildcard.',
			metricdescription =  'Determine whether unauthorized accounts have sysadmin privileges on the SQL Server or has SoD roles like "CONNECT ANY DATABASE", "IMPERSONATE ANY LOGIN", "SELECT ALL USER SECURABLES", "ALTER ANY COLUMN MASTER KEY", "ALTER ANY COLUMN ENCRYPTION KEY", "VIEW ANY COLUMN MASTER KEY DEFINITION", "VIEW ANY COLUMN ENCRYPTION KEY DEFINITION", "ALTER ANY SECURITY POLICY", "ALTER ANY MASK", "UNMASK"'
		WHERE metricid = 71
		
		UPDATE policymetric
		SET 
			reporttext = 'Do unauthorized accounts have sysadmin privileges or extended SoD roles?'
		WHERE metricid = 71

---------------------End Health checks--------------------------------------------------------------------------------------------------------





				----default tags 
        IF (
             NOT EXISTS ( SELECT
                            1
                          FROM
                            [dbo].[tags]
                          WHERE
                            name = 'All Servers' )
           )
            BEGIN
                INSERT  INTO [dbo].[tags]
                        (
                          [name] ,
                          [description] ,
                          [is_default]
                        )
                VALUES
                        (
                          'All Servers' ,
                          'All Servers' ,
                          1
                        );
            END

        INSERT  INTO dbo.server_tags
                SELECT
                    r.registeredserverid ,
                    tag_id
                FROM
                    tags t
                CROSS JOIN registeredserver r
                WHERE
                    is_default = 1
                    AND NOT EXISTS ( SELECT
                                        1
                                     FROM
                                        server_tags st
                                     WHERE
                                        st.tag_id = t.tag_id
                                        AND st.server_id = r.registeredserverid );



    END;
GO

insert into filterruleclass (objectclass, objectvalue) values (50, 'LinkedServer')