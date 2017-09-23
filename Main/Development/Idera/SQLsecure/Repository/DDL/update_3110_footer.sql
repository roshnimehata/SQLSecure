DECLARE @ver INT;
SELECT
    @ver = schemaversion
FROM
    currentversion;
IF ( ISNULL(@ver, 900) < 3110 )	
    BEGIN

        DECLARE
            @metricid INT 

-----Row-Level Security
        SELECT
            @metricid = 120;
        IF EXISTS ( SELECT
                            TOP 1 * 
                        FROM
                            metric
                        WHERE
                            metricid = @metricid )
        
		BEGIN
			UPDATE dbo.metric
			SET 
				metricdescription = N'Determine whether row-level security is configured for specified tables on SQL Server 2016 or later', -- metricdescription - nvarchar(1024)
				valuedescription = N'When enabled, this check will identify a risk if row-level security is not configured for specified tables on SQL Server 2016 or later. Please specify in [Server].[Database].[Schema].[Table] format.' -- metricdescription - nvarchar(1024)
			WHERE metricid = @metricid
		END;

		IF EXISTS ( SELECT
                            TOP 1 * 
                        FROM
                            policymetric
                        WHERE
                            metricid = @metricid )
        
		BEGIN
			UPDATE dbo.policymetric
			SET reporttext = N'Are specified tables using row-level security on SQL Server 2016 or later?'  -- reporttext - nvarchar(4000)
			WHERE metricid = @metricid
		END;

		IF EXISTS ( SELECT
                            TOP 1 * 
                        FROM
                            metricextendedinfo
                        WHERE
                            metricid = @metricid )
        
		BEGIN
			UPDATE dbo.metricextendedinfo
			SET 
				metricdescription = N'Determine whether row-level security is configured for specified tables on Azure SQL Database' , -- metricdescription - nvarchar(1024)
				valuedescription = N'When enabled, this check will identify a risk if row-level security is not configured for specified tables on Azure SQL Database. Please specify in [Server].[Database].[Schema].[Table] format.'  -- valuedescription - nvarchar(1024)
			WHERE metricid = @metricid
		END;

		IF EXISTS ( SELECT
                            TOP 1 * 
                        FROM
                            policymetricextendedinfo
                        WHERE
                            metricid = @metricid )
        
		BEGIN
			UPDATE dbo.policymetricextendedinfo
			SET reporttext = N'Are specified tables using row-level security on Azure SQL Database?'  -- reporttext - nvarchar(4000)
			WHERE metricid = @metricid
		END;

-----Dynamic Data Masking
        SELECT
            @metricid = 121;
        IF EXISTS ( SELECT
                            TOP 1 * 
                        FROM
                            metric
                        WHERE
                            metricid = @metricid )
         
		BEGIN
			UPDATE dbo.metric
			SET 
				metricdescription = N'Determine whether dynamic data masking is configured for specified columns on SQL Server 2016 or later', -- metricdescription - nvarchar(1024)
				valuedescription = N'When enabled, this check will identify a risk if dynamic data masking is not configured for specified columns on SQL Server 2016 or later. Please specify in [Server].[Database].[Schema].[Table].[Column] format.' -- metricdescription - nvarchar(1024)
			WHERE metricid = @metricid
		END;
		
		IF EXISTS ( SELECT
                            TOP 1 * 
                        FROM
                            policymetric
                        WHERE
                            metricid = @metricid )
        
		BEGIN
			UPDATE dbo.policymetric
			SET reporttext = N'Are specified columns using dynamic data masking on SQL Server 2016 or later?' -- reporttext - nvarchar(4000)
			WHERE metricid = @metricid
		END;

		IF EXISTS ( SELECT
                            TOP 1 * 
                        FROM
                            metricextendedinfo
                        WHERE
                            metricid = @metricid )
        
		BEGIN
			UPDATE dbo.metricextendedinfo
			SET 
				metricdescription = N'Determine whether dynamic data masking is configured for specified columns on Azure SQL Database' , -- metricdescription - nvarchar(1024)
				valuedescription = N'When enabled, this check will identify a risk if dynamic data masking is not configured for specified columns on Azure SQL Database. Please specify in [Server].[Database].[Schema].[Table].[Column] format.' -- metricdescription - nvarchar(1024)
			WHERE metricid = @metricid
		END;

		IF EXISTS ( SELECT
                            TOP 1 * 
                        FROM
                            policymetricextendedinfo
                        WHERE
                            metricid = @metricid )
        
		BEGIN
			UPDATE dbo.policymetricextendedinfo
			SET reporttext = N'Are specified columns using dynamic data masking on Azure SQL Database?'  -- reporttext - nvarchar(4000)
			WHERE metricid = @metricid
		END;

-----Unauthorized Accounts Check
		SELECT
            @metricid = 71;
        IF EXISTS ( SELECT
                            TOP 1 * 
                        FROM
                            metric
                        WHERE
                            metricid = @metricid )
         
		BEGIN
			UPDATE dbo.metric
			SET valuedescription = 'When enabled, this check will identify a risk if any unauthorized accounts are members of the sysadmin server role or extended SoD roles. Specify the authorized accounts. Can use ''%'' as wildcard.'
			WHERE metricid = @metricid
		END;
	END;
GO