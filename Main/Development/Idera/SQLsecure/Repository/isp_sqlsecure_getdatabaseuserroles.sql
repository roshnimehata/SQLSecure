SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_getdatabaseuserroles]'))
drop procedure [dbo].[isp_sqlsecure_getdatabaseuserroles]
GO

CREATE procedure [dbo].[isp_sqlsecure_getdatabaseuserroles] (@snapshotid int, @dbid int, @inputuid int, @idonly bit=1)
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get all database roles given the snapshot id and dbid and uid

	create table #tmpuid (uid int)
	create table #tmpuid2 (uid int)
	create table #tmpuid3 (uid int)

	declare @count int
	declare @rcount int
	declare @uid int

	-- get all immediate parent uids
	insert into #tmpuid (uid) (select groupuid from databaserolemember where snapshotid = @snapshotid and dbid = @dbid and rolememberuid = @inputuid)
	set @count = @@rowcount
	set @rcount = 1

	-- if there is at least one parent then try to find all parents
	while (@count > 0 and @rcount < 100)
	begin

		-- go thru' tmpuid table, for each uid try to find its parents
		declare myc0 cursor for
			select uid from #tmpuid
		
		open myc0
		fetch next from myc0
		into @uid
		
		while @@fetch_status = 0
		begin
			-- copy uid to tmp table
			insert into #tmpuid2 (uid) values (@uid)	
			
			-- copy all new parent uid to another tmp table
			insert into #tmpuid3 (uid) (select groupuid from databaserolemember where snapshotid = @snapshotid and dbid = @dbid and rolememberuid = @uid)

			fetch next from myc0
			into @uid	
		end
	
		close myc0
		deallocate myc0	

		delete from #tmpuid

		-- copy all new parent uid to holding table
		insert into #tmpuid (uid) (select uid from #tmpuid3 where uid not in (select uid from #tmpuid2))				
		
		delete from #tmpuid3

		-- if there is no more new parent then stop
		set @count = @@rowcount

		set @rcount = @rcount + 1
	end

	-- add itself to the list
	insert into #tmpuid2 (uid) values (@inputuid)

	-- return all rows from the holding table
	declare @sql nvarchar(1024)
	if (@idonly = 1)
		select @sql = 'select distinct uid from #tmpuid2'
	else
		select @sql = 'select * from vwdatabaseprincipal where snapshotid = ' + convert(nvarchar, @snapshotid) + ' and dbid = ' + convert(nvarchar, @dbid) + ' and uid in (select distinct uid from #tmpuid2)'
	exec (@sql)

	drop table #tmpuid
	drop table #tmpuid2
	drop table #tmpuid3

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_getdatabaseuserroles] TO [SQLSecureView]

GO

