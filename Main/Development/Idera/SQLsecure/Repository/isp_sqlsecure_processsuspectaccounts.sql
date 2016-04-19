SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_processsuspectaccounts]'))
drop procedure [dbo].[isp_sqlsecure_processsuspectaccounts]
GO

CREATE procedure [dbo].[isp_sqlsecure_processsuspectaccounts] (@snapshotid int)
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Find and mark all windows accounts as suspect

	declare @inputsid varbinary(85)
	declare @type nvarchar(128)
	declare @name nvarchar(200)

	create table #tmpsid (sid varbinary(85))

	declare myc100 cursor for
		select sid, type, name from windowsaccount where snapshotid = @snapshotid and UPPER(state) = 'S'
	
	open myc100
	fetch next from myc100
	into @inputsid, @type, @name
	
	while @@fetch_status = 0
	begin
		-- get all the parent groups and their associated members
		insert into #tmpsid exec isp_sqlsecure_getwindowsgroupparents @snapshotid, @inputsid

		select * from #tmpsid

		update windowsaccount set state = 'S' where snapshotid = @snapshotid and sid IN (select a.sid from #tmpsid a, windowsaccount b where b.snapshotid = @snapshotid and a.sid = b.sid and (b.state IS NULL or b.state NOT IN ('S')))

		fetch next from myc100
		into @inputsid, @type, @name
	end
	
	close myc100
	deallocate myc100		
GO

