DECLARE @ver INT;
SELECT
    @ver = schemaversion
FROM
    currentversion;
IF ( ISNULL(@ver, 900) <= 3000 )	-- Check to prevent this from running in future upgrades, but run for all prior versions at any version level because the version gets updated later
    BEGIN
        DECLARE
            @metricid INT ,
            @strval NVARCHAR(512)

        DECLARE @startmetricid INT


		--Typo fix
		SELECT
            @metricid = 116
                UPDATE metric
			        SET valuedescription  = 'When enabled, this check will identify a risk if there are linked servers that are running as a member of sysadmin group'
					WHERE  metricid = @metricid



	END;
GO