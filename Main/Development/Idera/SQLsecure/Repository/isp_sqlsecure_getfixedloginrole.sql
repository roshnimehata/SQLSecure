SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_getfixedloginrole]'))
drop procedure [dbo].[isp_sqlsecure_getfixedloginrole]
GO

CREATE procedure [dbo].[isp_sqlsecure_getfixedloginrole] (@snapshotid int, @logintype nchar(1), @inputsid varbinary(85), @sqllogin nvarchar(128))
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get all the fixed server role for the given Windows login or sql login

	declare @loginname nvarchar(128)
	declare @errmsg nvarchar(500)

	-- if there is not such snapshotid then return error
	if not exists (select * from serversnapshot where snapshotid = @snapshotid)
	begin
		set @errmsg = 'Error: Snapshot id ' + CONVERT(varchar(10), @snapshotid) + ' not found'
		RAISERROR (@errmsg, 16, 1)
		return -1
	end

	create table #tmplogins (sid varbinary(85), principalid int, name nvarchar(128), type nchar(1), serveraccess nchar(1), serverdeny nchar(1), disabled nchar(1))

	if (UPPER(@logintype) = 'W')
	begin
	
		create table #tmpsid (sid varbinary(85))
	
		-- insert current login to tmp table
		insert into #tmplogins (sid, principalid, name, type, serveraccess, serverdeny, disabled) (select sid, principalid, name, type, serveraccess, serverdeny, disabled from serverprincipal where snapshotid = @snapshotid and sid = @inputsid)
	
		-- get all windows parents groups
		insert into #tmpsid exec isp_sqlsecure_getwindowsgroupparents @snapshotid, @inputsid
	
		-- insert all groups in serverprincipal table
		insert into #tmplogins (sid, principalid, name, type, serveraccess, disabled) (select a.sid, a.principalid, a.name, a.type, a.serveraccess, a.disabled from serverprincipal a, #tmpsid b where a.snapshotid = @snapshotid and a.sid = b.sid)

		select @loginname = name from serverprincipal where sid = @inputsid

		drop table #tmpsid				
	end
	else -- sql login type
	begin
		insert into #tmplogins (sid, principalid, name, type, serveraccess, serverdeny, disabled) (select a.sid, a.principalid, a.name, a.type, a.serveraccess, a.serverdeny, a.disabled from serverprincipal a where a.snapshotid = @snapshotid and name=@sqllogin)
		set @loginname = @sqllogin
	end

	if exists (select 'x' from tempdb..sysobjects where type = 'U' and lower(name) like '#tmproles%')
	begin
		--select a.*, access=CASE WHEN a.serveraccess='Y' and a.serverdeny='N' THEN 'Permit' WHEN a.serverdeny='Y' THEN 'Deny' ELSE 'None'  END from vwloginfixedserverrole a, #tmplogins b where a.snapshotid = @snapshotid and a.principalid = b.principalid
		insert into #tmproles (rolename) select distinct CASE WHEN a.rolename IS NULL THEN '' ELSE a.rolename END from vwloginfixedserverrole a, #tmplogins b where a.snapshotid = @snapshotid and a.principalid = b.principalid
		return
	end

	select a.*, access=CASE WHEN a.serveraccess='Y' and a.serverdeny='N' THEN 'Permit' WHEN a.serverdeny='Y' THEN 'Deny' ELSE 'None'  END from vwloginfixedserverrole a, #tmplogins b where a.snapshotid = @snapshotid and a.principalid = b.principalid


GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_getfixedloginrole] TO [SQLSecureView]

GO


