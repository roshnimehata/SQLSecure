SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_report_getuserserveraccess]'))
drop procedure [dbo].[isp_sqlsecure_report_getuserserveraccess]
GO

CREATE PROCEDURE [dbo].[isp_sqlsecure_report_getuserserveraccess]
(
	@rundate datetime = null,	--defaults to all
	@policyid int = 1,			--defaults to all
	@usebaseline bit = 0,		--defaults to false
	@logintype nchar(1), 
	@user nvarchar(128),
	@servername nvarchar(400)
)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Gets the servers that can be accessed by a user SQL login or Windows account
   -- 
   
-- Replace any SQL injection characters in the user string.
SELECT @user = REPLACE(@user, '*', '%')
SELECT @user = REPLACE(@user, '--', '')
SELECT @user = REPLACE(@user, ';', '')
SELECT @user = REPLACE(@user, '''', '')

-- Note that this SP has a small but definite potential to return the wrong user
-- if two snapshots for the selected rundate have the same name with different SIDs

-- If windows user get the SID of the user.
DECLARE @tempsid varbinary(85)
IF @logintype = 'W' 
	SELECT TOP 1 @tempsid = a.sid FROM windowsaccount a, dbo.getsnapshotlist(@rundate, @usebaseline) b WHERE a.snapshotid = b.snapshotid AND UPPER([name]) LIKE UPPER(@user)

-- Query using the second stored proc for servers that the user has access to.	
EXEC isp_sqlsecure_report_getserveraccess
	@rundate = @rundate,
	@logintype = @logintype, 
	@inputsid = @tempsid, 
	@sqllogin = @user,
	@policyid = @policyid,
	@usebaseline = @usebaseline,
	@serverName = @servername

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_report_getuserserveraccess] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO