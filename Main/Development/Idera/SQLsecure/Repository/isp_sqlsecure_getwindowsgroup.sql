SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_getwindowsgroup]'))
drop procedure [dbo].[isp_sqlsecure_getwindowsgroup]
GO

create procedure [dbo].[isp_sqlsecure_getwindowsgroup] (@snapshotid int, @inputsid varbinary(85))
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get all server logins and roles given a windows account sid

	declare @loginname nvarchar(128)
	declare @connectionname nvarchar(400)
	declare @errmsg nvarchar(500)

	-- if there is not such snapshotid then return error
	if not exists (select * from serversnapshot where snapshotid = @snapshotid)
	begin
		set @errmsg = 'Error: Snapshot id ' + CONVERT(varchar(10), @snapshotid) + ' not found'
		RAISERROR (@errmsg, 16, 1)
		return -1
	end

	create table #tmplogins (sid varbinary(85), principalid int, name nvarchar(128), type nchar(1), serveraccess nchar(1), serverdeny nchar(1), disabled nchar(1))

	-- checks if the login exists in sql server
	if exists (select * from serverprincipal where snapshotid = @snapshotid and sid = @inputsid)
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

	-- testing show all permission
	exec ('select * from #tmplogins')


	-- TODO: compute effective

	drop table #tmplogins

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_getwindowsgroup] TO [SQLSecureView]

GO
