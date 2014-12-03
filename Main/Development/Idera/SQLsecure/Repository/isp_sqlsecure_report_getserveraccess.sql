SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_report_getserveraccess]'))
drop procedure [dbo].[isp_sqlsecure_report_getserveraccess]
GO

CREATE procedure [dbo].[isp_sqlsecure_report_getserveraccess]
(
	@rundate datetime = null,	--defaults to all
	@policyid int = 1,			--defaults to all
	@usebaseline bit = 0,		--defaults to false
	@logintype nchar(1), 
	@inputsid varbinary(85), 
	@sqllogin nvarchar(128),
	@serverName nvarchar(400)
)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get all the server logins given the input sid or sql login

DECLARE @snapshotid int
DECLARE @connectionname nvarchar(500)
DECLARE @loginname nvarchar(128)
DECLARE @iscasesensitive nchar(1)

SET @iscasesensitive = 'Y'

CREATE TABLE #tmpserveraccess (snapshotid int, connectionname nvarchar(500), logintype nvarchar(40), loginname nvarchar(500))
CREATE TABLE #tmplogins ([sid] varbinary(85), principalid int, [name] nvarchar(128), [type] nchar(1), serveraccess nchar(1), serverdeny nchar(1), [disabled] nchar(1))

CREATE TABLE #tmpservers (registeredserverid int)
INSERT #tmpservers
    EXEC [dbo].[isp_sqlsecure_getpolicymemberlist]
         @policyid = @policyid  

-- Go thru' all the snapshots and get the server
DECLARE cursor1 CURSOR FOR (SELECT snapshotid, connectionname FROM dbo.getsnapshotlist(@rundate, @usebaseline) WHERE registeredserverid IN (SELECT registeredserverid FROM #tmpservers) AND connectionname LIKE @serverName)

OPEN cursor1
FETCH NEXT FROM cursor1
INTO @snapshotid, @connectionname

WHILE @@fetch_status = 0
	BEGIN
		DELETE FROM #tmplogins
		SELECT @iscasesensitive = casesensitivemode FROM serversnapshot WHERE snapshotid = @snapshotid

		IF (@logintype = 'W')
			BEGIN
				IF EXISTS (SELECT * FROM windowsaccount WHERE snapshotid = @snapshotid AND [sid] = @inputsid)
					BEGIN
						CREATE TABLE #tmpsid ([sid] varbinary(85))
					
						-- insert current login to tmp table
						INSERT INTO #tmplogins ([sid], principalid, [name], [type], serveraccess, serverdeny, [disabled]) (SELECT DISTINCT [sid], principalid, [name], [type], serveraccess, serverdeny, [disabled] FROM serverprincipal WHERE snapshotid = @snapshotid AND [sid] = @inputsid)
					
						-- get all windows parents groups
						INSERT INTO #tmpsid EXEC isp_sqlsecure_getwindowsgroupparents @snapshotid, @inputsid
					
						-- insert all groups in serverprincipal table
						INSERT INTO #tmplogins ([sid], principalid, [name], [type], serveraccess, [disabled]) (SELECT DISTINCT a.sid, a.principalid, a.name, a.type, a.serveraccess, a.disabled FROM serverprincipal a, #tmpsid b WHERE a.snapshotid = @snapshotid AND a.sid = b.sid)
				
						SELECT @loginname = [name] FROM serverprincipal WHERE [sid] = @inputsid
				
						DROP TABLE #tmpsid				
					END
			END
		ELSE -- sql login type
			BEGIN
				IF (@iscasesensitive = 'Y')
					INSERT INTO #tmplogins ([sid], principalid, [name], [type], serveraccess, serverdeny, [disabled]) (SELECT DISTINCT a.sid, a.principalid, a.name, a.type, a.serveraccess, a.serverdeny, a.disabled FROM serverprincipal a WHERE a.snapshotid = @snapshotid AND [type] = 'S' AND CONVERT(VARBINARY, [name])=CONVERT(VARBINARY, @sqllogin))
				ELSE
					INSERT INTO #tmplogins ([sid], principalid, [name], [type], serveraccess, serverdeny, [disabled]) (SELECT DISTINCT a.sid, a.principalid, a.name, a.type, a.serveraccess, a.serverdeny, a.disabled FROM serverprincipal a WHERE a.snapshotid = @snapshotid AND [type] = 'S' AND UPPER([name])=UPPER(@sqllogin))

				SET @loginname = @sqllogin
			END

		-- Check if any of these logins have deny or no access, if so then don't show
		IF EXISTS (SELECT 1 FROM #tmplogins)
			BEGIN
				IF EXISTS (SELECT 1 FROM #tmplogins WHERE serveraccess = 'Y' AND (serverdeny IS NULL OR serverdeny <> 'Y') AND ([disabled] = '' OR [disabled] <> 'Y'))
					BEGIN
						INSERT INTO #tmpserveraccess (snapshotid, connectionname, logintype, loginname)
						SELECT DISTINCT @snapshotid, @connectionname, dbo.getserverprincipaltypename([type]) AS [type], [name] FROM #tmplogins				
					END
			END

		FETCH NEXT FROM cursor1
		INTO @snapshotid, @connectionname

	END

CLOSE cursor1
DEALLOCATE cursor1

DROP TABLE #tmpservers

SELECT DISTINCT * FROM #tmpserveraccess

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_report_getserveraccess] TO [SQLSecureView]

GO

