--
-- This scripts the following setup to be done.
-- 
-- 1. Run sqlsecure_ddl.sql creates the database and tables.
-- 2. Run the following scripts in order to create stored procs
--		isp_sqlsecure_isadmin.sql
--		isp_sqlsecure_addactivitylog.sql
--		isp_sqlsecure_addregisteredserver.sql
--		isp_sqlsecure_getuserapplicationrole
--		isp_sqlsecure_addruleheader.sql
--		isp_sqlsecure_addrule.sql
--
USE [SQLsecure]
GO

-- Register SQL Servers, and filter rules
EXEC [dbo].[isp_sqlsecure_addregisteredserver] 
	@connectionname= 'secure2kdc0', 
	@servername= 'secure2kdc0', 
	@instancename= '', 
	@loginname= 'sa', 
	@loginpassword= 'control*88',
	@authmode= 'S',
	@serverlogin='secure2k-dom\svcacct',
	@serverpassword = 'svcacct'
GO

EXEC [dbo].[isp_sqlsecure_addregisteredserver] 
	@connectionname= 'secure2kms0', 
	@servername= 'secure2kms0', 
	@instancename= '', 
	@loginname= 'sa', 
	@loginpassword= 'control*88',
	@authmode= 'S',
	@serverlogin='secure2k-dom\svcacct',
	@serverpassword = 'svcacct'
GO

EXEC [dbo].[isp_sqlsecure_addregisteredserver] 
	@connectionname= 'secure2kms1\secondinstance', 
	@servername= 'secure2kms1', 
	@instancename= 'secondinstance', 
	@loginname= 'sa', 
	@loginpassword= 'control*88',
	@authmode= 'S',
	@serverlogin='secure2k-dom\svcacct',
	@serverpassword = 'svcacct'
GO


-- Add filter header & get filter rule header id.
declare @new_instance nvarchar(500)
declare @new_hdrname nvarchar(100)
declare @new_ruleheaderid int

SET @new_instance = 'secure2kdc0'
SET @new_hdrname = 'AllObjectsSrvr'

	EXEC [dbo].[isp_sqlsecure_addruleheader] 
		@connectionname= @new_instance, 
		@rulename = @new_hdrname,
		@description = ''
	DECLARE cursor_filterruleid CURSOR FOR
		SELECT filterruleheaderid FROM filterruleheader 
		WHERE connectionname >= @new_instance 
		ORDER BY 1 DESC
	OPEN cursor_filterruleid
	FETCH NEXT FROM cursor_filterruleid  INTO @new_ruleheaderid
	CLOSE cursor_filterruleid
	DEALLOCATE cursor_filterruleid

	-- Add rules
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 1,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 2,
		@scope = 'A',
		@matchstring = '*'

SET @new_hdrname = 'AllObjectsDb'

	EXEC [dbo].[isp_sqlsecure_addruleheader] 
		@connectionname= @new_instance, 
		@rulename = @new_hdrname,
		@description = ''
	DECLARE cursor_filterruleid CURSOR FOR
		SELECT filterruleheaderid FROM filterruleheader 
		WHERE connectionname >= @new_instance 
		ORDER BY 1 DESC
	OPEN cursor_filterruleid
	FETCH NEXT FROM cursor_filterruleid  INTO @new_ruleheaderid
	CLOSE cursor_filterruleid
	DEALLOCATE cursor_filterruleid
	-- Add rules
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 20,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 21,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 22,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 26,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 27,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 28,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 29,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 30,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 31,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 32,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 41,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 42,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 43,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 44,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 45,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 46,
		@scope = 'A',
		@matchstring = '*'
GO	

declare @new_instance nvarchar(500)
declare @new_hdrname nvarchar(100)
declare @new_ruleheaderid int

SET @new_instance = 'secure2kms0'
SET @new_hdrname = 'AllObjectsSrvr'

	EXEC [dbo].[isp_sqlsecure_addruleheader] 
		@connectionname= @new_instance, 
		@rulename = @new_hdrname,
		@description = ''
	DECLARE cursor_filterruleid CURSOR FOR
		SELECT filterruleheaderid FROM filterruleheader 
		WHERE connectionname >= @new_instance 
		ORDER BY 1 DESC
	OPEN cursor_filterruleid
	FETCH NEXT FROM cursor_filterruleid  INTO @new_ruleheaderid
	CLOSE cursor_filterruleid
	DEALLOCATE cursor_filterruleid

	-- Add rules
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 1,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 2,
		@scope = 'A',
		@matchstring = '*'

SET @new_hdrname = 'AllObjectsDb'

	EXEC [dbo].[isp_sqlsecure_addruleheader] 
		@connectionname= @new_instance, 
		@rulename = @new_hdrname,
		@description = ''
	DECLARE cursor_filterruleid CURSOR FOR
		SELECT filterruleheaderid FROM filterruleheader 
		WHERE connectionname >= @new_instance 
		ORDER BY 1 DESC
	OPEN cursor_filterruleid
	FETCH NEXT FROM cursor_filterruleid  INTO @new_ruleheaderid
	CLOSE cursor_filterruleid
	DEALLOCATE cursor_filterruleid
	-- Add rules
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 20,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 21,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 22,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 26,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 27,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 28,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 29,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 30,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 31,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 32,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 41,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 42,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 43,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 44,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 45,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 46,
		@scope = 'A',
		@matchstring = '*'

GO	

declare @new_instance nvarchar(500)
declare @new_hdrname nvarchar(100)
declare @new_ruleheaderid int

SET @new_instance = 'secure2kms1\secondinstance'
SET @new_hdrname = 'AllObjectsSrvr'

	EXEC [dbo].[isp_sqlsecure_addruleheader] 
		@connectionname= @new_instance, 
		@rulename = @new_hdrname,
		@description = ''
	DECLARE cursor_filterruleid CURSOR FOR
		SELECT filterruleheaderid FROM filterruleheader 
		WHERE connectionname >= @new_instance 
		ORDER BY 1 DESC
	OPEN cursor_filterruleid
	FETCH NEXT FROM cursor_filterruleid  INTO @new_ruleheaderid
	CLOSE cursor_filterruleid
	DEALLOCATE cursor_filterruleid

	-- Add rules
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 1,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 2,
		@scope = 'A',
		@matchstring = '*'

SET @new_hdrname = 'AllObjectsDb'

	EXEC [dbo].[isp_sqlsecure_addruleheader] 
		@connectionname= @new_instance, 
		@rulename = @new_hdrname,
		@description = ''
	DECLARE cursor_filterruleid CURSOR FOR
		SELECT filterruleheaderid FROM filterruleheader 
		WHERE connectionname >= @new_instance 
		ORDER BY 1 DESC
	OPEN cursor_filterruleid
	FETCH NEXT FROM cursor_filterruleid  INTO @new_ruleheaderid
	CLOSE cursor_filterruleid
	DEALLOCATE cursor_filterruleid
	-- Add rules
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 20,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 21,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 22,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 26,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 27,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 28,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 29,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 30,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 31,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 32,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 41,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 42,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 43,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 44,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 45,
		@scope = 'A',
		@matchstring = '*'
	EXEC [dbo].[isp_sqlsecure_addrule] 
		@ruleheaderid = @new_ruleheaderid,
		@class = 46,
		@scope = 'A',
		@matchstring = '*'
GO	
