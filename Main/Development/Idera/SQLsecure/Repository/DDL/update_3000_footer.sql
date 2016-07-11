declare @ver int
SELECT @ver=schemaversion FROM currentversion
IF (isnull(@ver, 900) <= 2999)	-- Check to prevent this from running in future upgrades, but run for all prior versions at any version level because the version gets updated later
BEGIN


---------------------Health checks-----------------------------------------------------------------------------------------------------------
-----SQL Server Browser Running
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
-----SQL Server Version (https://support.microsoft.com/en-us/kb/321185 https://support.microsoft.com/en-us/kb/957826)
--	Current			-		New
--	''				-	'13.0.1601'	(SQL Server 2016 Mainstream Support)
--	''				-	'12.0.2000'	(SQL Server 2014 Mainstream Support)
--	'11.0.2100'		-	'11.0.2100'	SQL Server 2012 - supported
--	'10.50.2500'	-	''			SQL Server 2008 R2 - not supported. extended support only
--	'10.0.5500'		-	''			SQL Server 2008 - not supported. extended support only
--	'9.00.5000'		-	''			SQL Server 2005 - not supported
--	'8.00.2039'		-	''			SQL Server 2000 - not supported
	select @metricid = 2
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid )
	begin
		update policymetric 
			set severityvalues= N'''11.0.2100'',''12.0.2000'',''13.0.1601'''
			where policyid = 0 and assessmentid=0 and metricid = @metricid
		if (@ver is null)	-- this is a new install, so fix the All Servers policy
			update policymetric 
				set severityvalues= (select severityvalues from policymetric where policyid = 0 and assessmentid=0 and metricid = @metricid)
				where policyid = 1 and assessmentid = 1 and metricid = @metricid
	end

-----Other General Domain Accounts Check
	select @metricid = 111
	if not exists ( select * from metric where metricid = @metricid ) 
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
			N'Other General Domain Accounts', -- metricname - nvarchar(256)
			N'Determine whether general domain accounts added to the instance.', -- metricdescription - nvarchar(1024)
            0, -- isuserentered - bit
            0, -- ismultiselect - bit
            N'', -- validvalues - nvarchar(1024)
            N'When enabled, this check will identify a risk if  general domain accounts added to the instance.'  -- valuedescription - nvarchar(1024)									        
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
			N'Is there any general domain accounts added to the instance?', -- reporttext - nvarchar(4000)
			3, -- severity - int
			N'', -- severityvalues - nvarchar(4000)
            0  -- assessmentid - int
         )
	end
---------------------End Health checks--------------------------------------------------------------------------------------------------------


GO
