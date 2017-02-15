DECLARE @ver INT;
SELECT
    @ver = schemaversion
FROM
    currentversion;
IF ( ISNULL(@ver, 900) >= 3100 )	
    BEGIN
		--START(Barkha Khatri) updating applicableonazuredb value for supported metrics
		UPDATE metric
		set applicableonazuredb=1
		WHERE metricid in (
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
		114
		)
		--END(Barkha Khatri) updating applicableonazuredb value for supported metrics
        DECLARE
            @metricid INT ,
            @strval NVARCHAR(512)

        DECLARE @startmetricid INT
        SELECT
            @startmetricid = 117


-----Always Encrypted
        SELECT
            @metricid = 117;
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
                          valuedescription,
						  applicableonazuredb,
						  applicableonazurevm,
						  applicableonpremise
		                )
                VALUES
                        (
                          @metricid , -- metricid - int
                          N'Access' , -- metrictype - nvarchar(32)
                          N'Always Encrypted' , -- metricname - nvarchar(256)
                          N'Determine whether always encryption is configured for any databases.' , -- metricdescription - nvarchar(1024)
                          1 , -- isuserentered - bit
                          1 , -- ismultiselect - bit
                          N'' , -- validvalues - nvarchar(1024)
                          N'When enabled, this check will identify a risk if XXXXXXXX. \nPlease specify in [Server].[Database].[Schema].Table].[Column] format.',  -- valuedescription - nvarchar(1024),
						  1, -- applicableonazuredb bit
						  1, -- applicableonazurevm bit
						  1	-- applicableonpremise bit								        
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
                          N'Are any databases using always encryption to protect sensitive data?' , -- reporttext - nvarchar(4000)
                          3 , -- severity - int
                          N'' , -- severityvalues - nvarchar(4000)
                          0  -- assessmentid - int
                        );
            END;

-----Transparent Data Encryption
        SELECT
            @metricid = 118;
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
                          valuedescription,
						  applicableonazuredb,
						  applicableonazurevm,
						  applicableonpremise
		                )
                VALUES
                        (
                          @metricid , -- metricid - int
                          N'Access' , -- metrictype - nvarchar(32)
                          N'Transparent Data Encryption' , -- metricname - nvarchar(256)
                          N'Determine whether TDE is configured for any databases.' , -- metricdescription - nvarchar(1024)
                          1 , -- isuserentered - bit
                          1 , -- ismultiselect - bit
                          N'' , -- validvalues - nvarchar(1024)
                          N'When enabled, this check will identify a risk if XXXXXXXX.\n Please specify in format [SERVER].[DATABASE] for DBs to be excluded.',  -- valuedescription - nvarchar(1024)
						  1, -- applicableonazuredb bit
						  1, -- applicableonazurevm bit
						  1	-- applicableonpremise bit											        
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
                          N'Is TDE enabled at the database level?' , -- reporttext - nvarchar(4000)
                          3 , -- severity - int
                          N'' , -- severityvalues - nvarchar(4000)
                          0  -- assessmentid - int
                        );
            END;

-----Backup Encryption
        SELECT
            @metricid = 119;
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
                          valuedescription,
						  applicableonazuredb,
						  applicableonazurevm,
						  applicableonpremise
		                )
                VALUES
                        (
                          @metricid , -- metricid - int
                          N'Access' , -- metrictype - nvarchar(32)
                          N'Backup Encryption' , -- metricname - nvarchar(256)
                          N'Determine whether backup encryption was configured.' , -- metricdescription - nvarchar(1024)
                          0 , -- isuserentered - bit
                          0 , -- ismultiselect - bit
                          N'' , -- validvalues - nvarchar(1024)
                          N'When enabled, this check will identify a risk if XXXXXXXX.' , -- valuedescription - nvarchar(1024)	
						  0, -- applicableonazuredb bit
						  1, -- applicableonazurevm bit
						  1	-- applicableonpremise bit									        
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
                          N'Are any backups configured for encryption?' , -- reporttext - nvarchar(4000)
                          3 , -- severity - int
                          '''master'',''msdb'',''model'',''tempdb''' , -- severityvalues - nvarchar(4000)
                          0  -- assessmentid - int
                        );
            END;


-----Row-Level Security
        SELECT
            @metricid = 120;
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
                          valuedescription,
						  applicableonazuredb,
						  applicableonazurevm,
						  applicableonpremise
		                )
                VALUES
                        (
                          @metricid , -- metricid - int
                          N'Access' , -- metrictype - nvarchar(32)
                          N'Row-Level Security' , -- metricname - nvarchar(256)
                          N'Determine whether row-level security is configured for any databases.' , -- metricdescription - nvarchar(1024)
                          1 , -- isuserentered - bit
                          1 , -- ismultiselect - bit
                          N'' , -- validvalues - nvarchar(1024)
                          N'When enabled, this check will identify a risk if XXXXXXXX. \nPlease specify in [Server].[Database].[Schema].[Table] format.',  -- valuedescription - nvarchar(1024)	
						  1, -- applicableonazuredb bit
						  1, -- applicableonazurevm bit
						  1	-- applicableonpremise bit									        
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
                          N'Are any databases using row-level security?' , -- reporttext - nvarchar(4000)
                          3 , -- severity - int
                          N'' , -- severityvalues - nvarchar(4000)
                          0  -- assessmentid - int
                        );
            END;


-----Dynamic Data Masking
        SELECT
            @metricid = 121;
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
                          valuedescription,
						  applicableonazuredb,
						  applicableonazurevm,
						  applicableonpremise
		                )
                VALUES
                        (
                          @metricid , -- metricid - int
                          N'Access' , -- metrictype - nvarchar(32)
                          N'Dynamic Data Masking' , -- metricname - nvarchar(256)
                          N'Determine whether dynamic database masking is configured for any databases.' , -- metricdescription - nvarchar(1024)
                          1 , -- isuserentered - bit
                          1 , -- ismultiselect - bit
                          N'' , -- validvalues - nvarchar(1024)
                          N'When enabled, this check will identify a risk if XXXXXXXX. \nPlease specify in [Server].[Database].[Schema].[Table].[Column] format.',  -- valuedescription - nvarchar(1024)
						  1, -- applicableonazuredb bit
						  1, -- applicableonazurevm bit
						  1	-- applicableonpremise bit										        
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
                          N'Are any databases using dynamic data masking?' , -- reporttext - nvarchar(4000)
                          2 , -- severity - int
                          N'' , -- severityvalues - nvarchar(4000)
                          0  -- assessmentid - int
                        );
            END;


-----Signed Objects
        SELECT
            @metricid = 122;
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
                          valuedescription,
						  applicableonazuredb,
						  applicableonazurevm,
						  applicableonpremise
		                )
                VALUES
                        (
                          @metricid , -- metricid - int
                          N'Access' , -- metrictype - nvarchar(32)
                          N'Signed Objects' , -- metricname - nvarchar(256)
                          N'Determine whether a digital signature has been added to any stored procedure, function, assembly or trigger.' , -- metricdescription - nvarchar(1024)
                          1 , -- isuserentered - bit
                          1 , -- ismultiselect - bit
                          N'' , -- validvalues - nvarchar(1024)
                          N'When enabled, this check will identify a risk if XXXXXXXX. \nPlease specify in [Server].[Database].[Schema].[Object] format for stored procedure, function, trigger or [Server].[Database].[Assembly] for assembly.',  -- valuedescription - nvarchar(1024)		
						  0, -- applicableonazuredb bit
						  1, -- applicableonazurevm bit
						  1	-- applicableonpremise bit								        
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
                          N'Are digital signatures present for any stored procedures, functions, assemblies or triggers?' , -- reporttext - nvarchar(4000)
                          1 , -- severity - int
                          N'' , -- severityvalues - nvarchar(4000)
                          0  -- assessmentid - int
                        );
            END;

-----Server-level Firewall Rules
        SELECT
            @metricid = 123;
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
                          valuedescription,
						  applicableonazuredb,
						  applicableonazurevm,
						  applicableonpremise
		                )
                VALUES
                        (
                          @metricid , -- metricid - int
                          N'Configuration' , -- metrictype - nvarchar(32)
                          N'Server-level Firewall Rules' , -- metricname - nvarchar(256)
                          N'Determine whether unapproved server-level firewall rules have been configured.' , -- metricdescription - nvarchar(1024)
                          1 , -- isuserentered - bit
                          1 , -- ismultiselect - bit
                          N'' , -- validvalues - nvarchar(1024)
                          N'When enabled, this check will identify a risk if XXXXXXXX. \nPlease specify authorized firewall rules in START_IP-END_IP format.',  -- valuedescription - nvarchar(1024)		
						  1, -- applicableonazuredb bit
						  0, -- applicableonazurevm bit
						  0	-- applicableonpremise bit								        
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
                          valuedescription,
						  applicableonazuredb,
						  applicableonazurevm,
						  applicableonpremise
		                )
                VALUES
                        (
                          @metricid , -- metricid - int
                          N'Configuration' , -- metrictype - nvarchar(32)
                          N'Database-level Firewall Rules' , -- metricname - nvarchar(256)
                          N'Determine whether unapproved database-level firewall rules have been configured.' , -- metricdescription - nvarchar(1024)
                          1 , -- isuserentered - bit
                          1 , -- ismultiselect - bit
                          N'' , -- validvalues - nvarchar(1024)
                          N'When enabled, this check will identify a risk if XXXXXXXX. \nPlease specify authorized firewall rules in START_IP-END_IP format.',  -- valuedescription - nvarchar(1024)	
						  1, -- applicableonazuredb bit
						  0, -- applicableonazurevm bit
						  0	-- applicableonpremise bit									        
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
                          N'Are there any unapproved database-level firewall rules?' , -- reporttext - nvarchar(4000)
                          3 , -- severity - int
                          N'' , -- severityvalues - nvarchar(4000)
                          0  -- assessmentid - int
                        );
            END;

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

    END;
GO



