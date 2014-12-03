SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_groomsnapshots]'))
drop procedure [dbo].[isp_sqlsecure_groomsnapshots]
GO

CREATE procedure [dbo].[isp_sqlsecure_groomsnapshots]
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --             Grooming all snapshots older than retention period. Also groom snapshots whose server got deleted.
   -- 	           

	declare @connectionname nvarchar(400)
	declare @retentionperiod int
	declare @count int
	declare @daydiff int
	declare @expireddate datetime
	declare @deletestarttime datetime
	declare @deleteendtime datetime
	declare @snapshotid int
	declare @snapshotcount int
	declare @activityid int

	set @deletestarttime = GETUTCDATE()
	set @count = 0

	insert into groomingactivityhistory (activitystarttime, status) values (@deletestarttime, 'P')

	select @activityid = max(groomingactivityid) from groomingactivityhistory

	declare myc1 cursor for
			select distinct connectionname, snapshotretentionperiod 
				from registeredserver 
				where snapshotretentionperiod IS NOT NULL
	
	open myc1
	fetch next from myc1
	into @connectionname, @retentionperiod

	while @@fetch_status = 0
	begin
		set @daydiff = @retentionperiod * -1

		set @expireddate = DATEADD(day, @daydiff, GETUTCDATE())

		print 'expired date ' + CONVERT(NVARCHAR(32), @expireddate)

		-- delete snapshot whose end date is before the retention period
		declare myc2 cursor for
				select snapshotid 
					from serversnapshot 
					where connectionname = @connectionname 
						and endtime IS NOT NULL 
						and endtime < @expireddate
						-- don't delete baseline snapshots
						and isnull(baseline, N'N') <> N'Y'
						-- don't allow deleting snapshots that are used by working assessments
						and snapshotid not in (select distinct a.snapshotid 
													from policyassessment a inner join assessment b on a.policyid = b.policyid
														and a.assessmentid = b.assessmentid
													where b.assessmentstate IN (N'D', N'P', N'A'))

		open myc2
		fetch next from myc2
		into @snapshotid

		while @@fetch_status = 0
		begin
			print 'deleting snapshotid ' + CONVERT(NVARCHAR(128), @snapshotid)
			exec isp_sqlsecure_removesnapshot @snapshotid=@snapshotid

			set @count = @count + 1			

			fetch next from myc2
			into @snapshotid		
		end

		close myc2
		deallocate myc2

		fetch next from myc1
		into @connectionname, @retentionperiod
	end

	close myc1
	deallocate myc1


	-- delete orphan snapshots
	declare myc3 cursor for
			select distinct snapshotid from serversnapshot a where a.connectionname NOT IN (select distinct connectionname from registeredserver)

	open myc3
	fetch next from myc3
	into @snapshotid
	
	while @@fetch_status = 0
	begin
		print 'deleting orphan snapshotid ' + CONVERT(NVARCHAR(128), @snapshotid)
		exec isp_sqlsecure_removesnapshot @snapshotid=@snapshotid

		set @count = @count + 1			
		
		fetch next from myc3
		into @snapshotid
	end

	close myc3
	deallocate myc3

	set @deleteendtime = GETUTCDATE()
	declare @str nvarchar(256)
	set @str = 'Groomed ' + CONVERT(NVARCHAR(32), @count) + ' snapshots.'

	update groomingactivityhistory set activityendtime = @deleteendtime, status = 'C', comment=@str where groomingactivityid = @activityid

	declare @programname nvarchar(128)
	select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)

	exec isp_sqlsecure_addactivitylog @activitytype=N'Success Audit', @source=@programname, @eventcode=N'Groom', @category=N'Job', @description=@str, @connectionname = null

GO


