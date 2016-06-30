declare @ver int
SELECT @ver=schemaversion FROM currentversion
IF (isnull(@ver, 900) <= 2999)	-- Check to prevent this from running in future upgrades, but run for all prior versions at any version level because the version gets updated later
BEGIN

	IF(EXISTS(SELECT 1 FROM metric WHERE metricid = 28))
	BEGIN
		UPDATE metric
		SET metricname = 'SQL Server Browser Running'
		WHERE metricid = 28
	END
	ELSE
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
		values (28, 'SQL Server Browser Running', 'Surface Area', 0, 0, '', 'When enabled, this check will identify a risk if SQL Server is visible for browsing from client computers.', 'Determine whether the SQL Server is hidden from client computers')
	END

GO
