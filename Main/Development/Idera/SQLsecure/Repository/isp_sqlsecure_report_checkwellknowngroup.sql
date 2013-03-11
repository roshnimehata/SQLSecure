SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_report_checkwellknowngroup]'))
drop procedure [dbo].[isp_sqlsecure_report_checkwellknowngroup]
GO

CREATE procedure [dbo].[isp_sqlsecure_report_checkwellknowngroup]
(
	@rundate datetime = null,	--defaults to all
	@policyid int = 1,			--defaults to all
	@usebaseline bit = 0,		--defaults to false
	@serverName nvarchar(400)
)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Checks all servers for well known windows groups like Everyone
   -- 

	CREATE TABLE #tmpservers (registeredserverid int)
	INSERT #tmpservers
		EXEC [dbo].[isp_sqlsecure_getpolicymemberlist]
			 @policyid = @policyid  
	           
	declare @snapshotid int
	declare @connectionname nvarchar(500)
	declare @status nchar(1)
	declare @name nvarchar(512)
	declare @sid varbinary(85)
	declare @access nvarchar(16)

	create table #tmpdata (connectionname nvarchar(400), name nvarchar(512), access nvarchar(16), groupname nvarchar(512))
	create table #tmpsid (sid varbinary(85))

	-- Go thru' all the snapshots and get the server
	DECLARE cursor1 CURSOR FOR SELECT snapshotid, connectionname FROM dbo.getsnapshotlist(@rundate, @usebaseline) WHERE registeredserverid IN (SELECT registeredserverid FROM #tmpservers) AND UPPER(connectionname) LIKE UPPER(@serverName)

	open cursor1
	fetch next from cursor1
	into @snapshotid, @connectionname

	while @@fetch_status = 0
	begin
		declare cursor2 cursor for
			select a.windowsgroupname, b.sid, case when c.sid is null then 'via group' else 'SQL Login' end as access
			from ancillarywindowsgroup a
				inner join windowsaccount b on b.snapshotid = a.snapshotid
				left join serverprincipal c on c.snapshotid = a.snapshotid and b.sid = c.sid
			where a.snapshotid = @snapshotid
				and upper(a.windowsgroupname) = upper(b.name)

		open cursor2
		fetch next from cursor2
		into @name, @sid, @access

		while @@fetch_status = 0
		begin
			-- Get all direct logins
			insert into #tmpdata (connectionname, [name], access, groupname) (
				select @connectionname, @name, @access, '' from serverprincipal where snapshotid = @snapshotid and [sid] = @sid
			)

			-- Get all permissions via group membership			
			delete from #tmpsid

			--insert into #tmpsid (sid) values (@sid)
			insert into #tmpsid exec isp_sqlsecure_getwindowsgroupparents @snapshotid, @sid

			insert into #tmpdata (connectionname, [name], access, groupname) (
				select @connectionname, @name, @access, [name] from serverprincipal where snapshotid = @snapshotid and [sid] in (select [sid] from #tmpsid)
			)

			fetch next from cursor2
			into @name, @sid, @access

		end

		close cursor2
		deallocate cursor2

	fetch next from cursor1
	into @snapshotid, @connectionname

	end

	close cursor1
	deallocate cursor1

	DROP TABLE #tmpservers

	select distinct connectionname, [name], access, groupname from #tmpdata order by connectionname, [name], groupname

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_report_checkwellknowngroup] TO [SQLSecureView]

GO