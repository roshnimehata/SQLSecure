SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_report_sysadmin_administrator]'))
drop procedure [dbo].[isp_sqlsecure_report_sysadmin_administrator]
GO

CREATE procedure [dbo].[isp_sqlsecure_report_sysadmin_administrator]
(
	@rundate datetime = null,	--defaults to all
	@policyid int = 1,			--defaults to all
	@usebaseline bit = 0		--defaults to false
)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Checks all servers for sysadmin adminstrators
   -- 	

	CREATE TABLE #tmpservers (registeredserverid int)
	INSERT #tmpservers
		EXEC [dbo].[isp_sqlsecure_getpolicymemberlist]
			 @policyid = @policyid 
           
	declare @snapshotid int
	declare @connectionname nvarchar(500)
	declare @status nchar(1)

	create table #tmpdata (connectionname nvarchar(400), login nvarchar(256), fixedrole nvarchar(100))

	-- Go thru' all the snapshots and get the server
	declare cursor1 cursor for select snapshotid, connectionname from dbo.getsnapshotlist(@rundate, @usebaseline) WHERE registeredserverid IN (SELECT registeredserverid FROM #tmpservers)
	
	open cursor1
	fetch next from cursor1
	into @snapshotid, @connectionname

	while @@fetch_status = 0
	begin

		if exists (select * from serverrolemember 
					where snapshotid = @snapshotid 
						and principalid = 3 
						and memberprincipalid in (select principalid 
													from serverprincipal 
													where snapshotid = @snapshotid 
														and sid = 0x01020000000000052000000020020000
													)
					)
		begin
			insert into #tmpdata (connectionname, login, fixedrole) values (@connectionname, 'BUILTIN\ADMINISTRATORS', 'sysadmin')			
		end


	fetch next from cursor1
	into @snapshotid, @connectionname

	end

	close cursor1
	deallocate cursor1

	select distinct connectionname, login, fixedrole from #tmpdata order by connectionname, login, fixedrole

	DROP TABLE #tmpservers

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_report_sysadmin_administrator] TO [SQLSecureView]

GO