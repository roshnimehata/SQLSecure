SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_getmsdbvalidroleslist]'))
drop procedure [dbo].[isp_sqlsecure_getmsdbvalidroleslist]
GO


CREATE procedure [dbo].[isp_sqlsecure_getmsdbvalidroleslist]
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --             Get a list of valid roles that have permissions on stored procedures in the MSDB database
   --				by default, this includes the Agent roles and Reporting Services roles installed by SQL Server
   --
   --			  Any roles listed here will not show as risks when they have access to stored procedures in MSDB

	DECLARE @mytable table (rolename nvarchar(128))

	-- Note: these names ARE case sensitive on a case sensitive server

	--SQL Server Agent 2000
	INSERT INTO @mytable VALUES ('TargetServersRole')		-- this role does not provide create capability by default

	--SQL Server Agent 2005
	INSERT INTO @mytable VALUES ('SQLAgentUserRole')
	INSERT INTO @mytable VALUES ('SQLAgentReaderRole')
	INSERT INTO @mytable VALUES ('SQLAgentOperatorRole')

	--SQL Server Reporting Services
	INSERT INTO @mytable VALUES ('RSExecRole')


	SELECT rolename FROM @mytable

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_getmsdbvalidroleslist] TO [SQLSecureView]

GO

