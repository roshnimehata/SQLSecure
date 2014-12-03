SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_getwindowsgroupparents]'))
drop procedure [dbo].[isp_sqlsecure_getwindowsgroupparents]
GO

CREATE procedure [dbo].[isp_sqlsecure_getwindowsgroupparents] (@snapshotid int, @inputsid varbinary(85))
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get all windows group parents given the snapshot id and input sid

	create table #tmpsid (sid varbinary(85))
	create table #tmpsid2 (sid varbinary(85))
	create table #tmpsid3 (sid varbinary(85))

	declare @count int
	declare @rcount int
	declare @sid varbinary(85)

	-- get all immediate parent sids
	insert into #tmpsid (sid) (select groupsid from windowsgroupmember where snapshotid = @snapshotid and groupmember = @inputsid)
	set @count = @@rowcount
	set @rcount = 1

	-- if there is at least one parent then try to find all parents
	while (@count > 0 and @rcount < 100)
	begin

		-- go thru' tmpsid table, for each sid try to find its parents
		declare myc0 cursor for
			select sid from #tmpsid
		
		open myc0
		fetch next from myc0
		into @sid
		
		while @@fetch_status = 0
		begin
			-- copy sid to tmp table
			insert into #tmpsid2 (sid) values (@sid)	
			
			-- copy all new parent sid to another tmp table
			insert into #tmpsid3 (sid) (select groupsid from windowsgroupmember where snapshotid = @snapshotid and groupmember = @sid)

			fetch next from myc0
			into @sid	
		end
	
		close myc0
		deallocate myc0	

		delete from #tmpsid

		-- copy all new parent sid to holding table
		insert into #tmpsid (sid) (select sid from #tmpsid3 where sid not in (select sid from #tmpsid2))				
		
		delete from #tmpsid3

		-- if there is no more new parent then stop
		set @count = @@rowcount

		set @rcount = @rcount + 1
	end

	-- return all rows from the holding table
	exec ('select distinct sid from #tmpsid2')

	drop table #tmpsid
	drop table #tmpsid2
	drop table #tmpsid3

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_getwindowsgroupparents] TO [SQLSecureView]

GO

