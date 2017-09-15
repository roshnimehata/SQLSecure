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
			SET metricdescription = N'Determine whether row-level security is configured for specified tables on SQL Server 2016 or later' -- metricdescription - nvarchar(1024)
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
			SET metricdescription = N'Determine whether dynamic data masking is configured for specified columns on SQL Server 2016 or later' -- metricdescription - nvarchar(1024)
			WHERE metricid = @metricid
		END;

		END;
GO