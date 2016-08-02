DECLARE @ver INT;
SELECT  @ver = schemaversion
FROM    currentversion;
IF ( ISNULL(@ver, 900) <= 2999 )	-- Check to prevent this from running in future upgrades, but run for all prior versions at any version level because the version gets updated later
BEGIN
	declare @metricid int, @strval nvarchar(512)

---------------------Health checks-----------------------------------------------------------------------------------------------------------
-----SQL Server Browser Running
        IF ( EXISTS ( SELECT    1
                      FROM      metric
                      WHERE     metricid = 28 ) )
	BEGIN
                UPDATE  metric
                SET     metricname = 'SQL Server Browser Running'
                WHERE   metricid = 28;
            END;
	ELSE
            INSERT  INTO metric
                    ( metricid ,
                      metricname ,
                      metrictype ,
                      isuserentered ,
                      ismultiselect ,
                      validvalues ,
                      valuedescription ,
                      metricdescription
                    )
            VALUES  ( 28 ,
                      'SQL Server Browser Running' ,
                      'Surface Area' ,
                      0 ,
                      0 ,
                      '' ,
                      'When enabled, this check will identify a risk if SQL Server is visible for browsing from client computers.' ,
                      'Determine whether the SQL Server is hidden from client computers'
                    );
    END;
-----SQL Server Version (https://support.microsoft.com/en-us/kb/321185 https://support.microsoft.com/en-us/kb/957826)
--	Current			-		New
--	''				-	'13.0.1601'	(SQL Server 2016 Mainstream Support)
--	''				-	'12.0.2000'	(SQL Server 2014 Mainstream Support)
--	'11.0.2100'		-	'11.0.2100'	SQL Server 2012 - supported
--	'10.50.2500'	-	''			SQL Server 2008 R2 - not supported. extended support only
--	'10.0.5500'		-	''			SQL Server 2008 - not supported. extended support only
--	'9.00.5000'		-	''			SQL Server 2005 - not supported
--	'8.00.2039'		-	''			SQL Server 2000 - not supported
SELECT  @metricid = 2;
IF EXISTS ( SELECT  *
            FROM    policymetric
            WHERE   policyid = 0
                    AND assessmentid = 0
                    AND metricid = @metricid )
    BEGIN
        UPDATE  policymetric
        SET     severityvalues = N'''11.0.2100'',''12.0.2000'',''13.0.1601'''
        WHERE   policyid = 0
                AND assessmentid = 0
                AND metricid = @metricid;
        IF ( @ver IS NULL )	-- this is a new install, so fix the All Servers policy
            UPDATE  policymetric
            SET     severityvalues = ( SELECT   severityvalues
                                       FROM     policymetric
                                       WHERE    policyid = 0
                                                AND assessmentid = 0
                                                AND metricid = @metricid
                                     )
            WHERE   policyid = 1
                    AND assessmentid = 1
                    AND metricid = @metricid;
    END;

-----Other General Domain Accounts Check
SELECT  @metricid = 111;
IF NOT EXISTS ( SELECT  *
                FROM    metric
                WHERE   metricid = @metricid )
    BEGIN  
        INSERT  INTO dbo.metric
                ( metricid ,
                  metrictype ,
                  metricname ,
                  metricdescription ,
                  isuserentered ,
                  ismultiselect ,
                  validvalues ,
			valuedescription
		)
        VALUES  ( @metricid , -- metricid - int
                  N'Configuration' , -- metrictype - nvarchar(32)
                  N'Other General Domain Accounts' , -- metricname - nvarchar(256)
                  N'Determine whether general domain accounts added to the instance.' , -- metricdescription - nvarchar(1024)
                  0 , -- isuserentered - bit
                  0 , -- ismultiselect - bit
                  N'' , -- validvalues - nvarchar(1024)
            N'When enabled, this check will identify a risk if  general domain accounts added to the instance.'  -- valuedescription - nvarchar(1024)									        
                );
		
        INSERT  INTO dbo.policymetric
                ( policyid ,
                  metricid ,
                  isenabled ,
                  reportkey ,
                  reporttext ,
                  severity ,
                  severityvalues ,
			assessmentid
		)
        VALUES  ( 0 , -- policyid - int
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
SELECT  @metricid = 112;
IF NOT EXISTS ( SELECT  *
                FROM    metric
                WHERE   metricid = @metricid )
    BEGIN  
        INSERT  INTO dbo.metric
                ( metricid ,
                  metrictype ,
                  metricname ,
                  metricdescription ,
                  isuserentered ,
                  ismultiselect ,
                  validvalues ,
			valuedescription
		)
        VALUES  ( @metricid , -- metricid - int
                  N'Access' , -- metrictype - nvarchar(32)
                  N'SQL Jobs and Agent' , -- metricname - nvarchar(256)
                  N'Determine whether job steps are running on behalf of proxy account.' , -- metricdescription - nvarchar(1024)
                  0 , -- isuserentered - bit
                  0 , -- ismultiselect - bit
                  N'' , -- validvalues - nvarchar(1024)
            N'When enabled, this check will identify a risk if job step has no proxy account.'  -- valuedescription - nvarchar(1024)									        
                );
		
        INSERT  INTO dbo.policymetric
                ( policyid ,
                  metricid ,
                  isenabled ,
                  reportkey ,
                  reporttext ,
                  severity ,
                  severityvalues ,
			assessmentid
		)
        VALUES  ( 0 , -- policyid - int
                  @metricid , -- metricid - int
                  1 , -- isenabled - bit
                  N'' , -- reportkey - nvarchar(32)
                  N'Is there any job step that is running without proxy account?' , -- reporttext - nvarchar(4000)
                  3 , -- severity - int
                  N'' , -- severityvalues - nvarchar(4000)
            0  -- assessmentid - int
                );
    END;
	--- new versions for Operation System checks.
SELECT  @metricid = 39;
IF EXISTS ( SELECT  *
            FROM    policymetric
            WHERE   policyid = 0
                    AND assessmentid = 0
                    AND metricid = @metricid )
    BEGIN
        UPDATE  policymetric
        SET     severityvalues = N'''Microsoft Windows Server 2008 R2 Enterprise , Service Pack 1'',''Microsoft Windows Server 2008 R2 Standard , Service Pack 1'',''Microsoft Windows Web Server 2008 R2 , Service Pack 1'',''Microsoft® Windows Server® 2008 Enterprise , Service Pack 2'',''Microsoft® Windows Server® 2008 Enterprise without Hyper-V , Service Pack 2'',''Microsoft® Windows Server® 2008 Datacenter , Service Pack 2'',''Microsoft® Windows Server® 2008 Datacenter without Hyper-V , Service Pack 2'',''Microsoft® Windows Server® 2008 Standard , Service Pack 2'',''Microsoft® Windows Server® 2008 Standard without Hyper-V , Service Pack 2'',''Microsoft® Windows® Web Server 2008 , Service Pack 2'',''Microsoft® Windows® 2012 Datacenter'',''Microsoft® Windows® 2012 Essentials'',''Microsoft® Windows® 2012 Foundation'',''Microsoft® Windows® 2012 R2 Datacenter'',''Microsoft® Windows® 2012 R2 Essentials'',''Microsoft® Windows® 2012 R2 Foundation'',''Microsoft® Windows® 2012 R2 Standard'',''Microsoft® Windows® 2012 Standard'''
        WHERE   policyid = 0
                AND assessmentid = 0
                AND metricid = @metricid;
        IF ( @ver IS NULL )	-- this is a new install, so fix the All Servers policy
            UPDATE  policymetric
            SET     severityvalues = ( SELECT   severityvalues
                                       FROM     policymetric
                                       WHERE    policyid = 0
                                                AND assessmentid = 0
                                                AND metricid = @metricid
         )
            WHERE   policyid = 1
                    AND assessmentid = 1
                    AND metricid = @metricid;
    END;

---------------------End Health checks--------------------------------------------------------------------------------------------------------

----default tags 
INSERT  INTO [dbo].[tags]
        ( [name] ,
          [description] ,
          [is_default]
        )
VALUES  ( 'All Servers' ,
          'All Servers' ,
          1
        );


INSERT  INTO dbo.server_tags
        SELECT  r.registeredserverid ,
                tag_id
        FROM    tags t
                CROSS JOIN registeredserver r
        WHERE   is_default = 1
                AND NOT EXISTS ( SELECT 1
                                 FROM   server_tags st
                                 WHERE  st.tag_id = t.tag_id
                                        AND st.server_id = r.registeredserverid );

		


---Encryption Check


-----SQL Jobs and Agent Check
SELECT  @metricid = 113;
IF NOT EXISTS ( SELECT  *
                FROM    metric
                WHERE   metricid = @metricid )
    BEGIN  
        INSERT  INTO dbo.metric
                ( metricid ,
                  metrictype ,
                  metricname ,
                  metricdescription ,
                  isuserentered ,
                  ismultiselect ,
                  validvalues ,
			valuedescription
		)
        VALUES  ( @metricid , -- metricid - int
                  N'Access' , -- metrictype - nvarchar(32)
                  N'Encryption Methodst' , -- metricname - nvarchar(256)
                  N'Determine whether there are encryption keys with algorithms other than supported.' , -- metricdescription - nvarchar(1024)
                  1 , -- isuserentered - bit
                  1 , -- ismultiselect - bit
                  N'''AES_128'',''AES_192'',''AES_256''' , -- validvalues - nvarchar(1024)
            N'When enabled, this check will identify a risk if there are encryption keys with encryption methods other than selected.'  -- valuedescription - nvarchar(1024)									        
                );
		
        INSERT  INTO dbo.policymetric
                ( policyid ,
                  metricid ,
                  isenabled ,
                  reportkey ,
                  reporttext ,
                  severity ,
                  severityvalues ,
			assessmentid
		)
        VALUES  ( 0 , -- policyid - int
                  @metricid , -- metricid - int
                  1 , -- isenabled - bit
                  N'' , -- reportkey - nvarchar(32)
                  N'Is there any encryption keys with algorithms other than selected?' , -- reporttext - nvarchar(4000)
                  3 , -- severity - int
                  N'''AES_128'',''AES_192'',''AES_256''' , -- severityvalues - nvarchar(4000)
            0  -- assessmentid - int
                );
    END;
		
GO
