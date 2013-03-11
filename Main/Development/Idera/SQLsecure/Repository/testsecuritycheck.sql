/***********************************************************************

	Security Check Tester

	This script contains a freamework of useful definitions and values for testing most of the security checks without having to run a complete assessment
	For new ones, I had the best results by finding a somewhat similar existing check and copying all of that code to the new one and then 
	copying all of that here for testing.  When I got it working, I could just plug it back into the assessment sp easily

************************************************************************/


/*	use these to clear new assessments so they can be reloaded from the footer script and tested with the new metric info
	declare @metricid int
	select @metricid=67
	delete from policyassessmentdetail where metricid >= @metricid
	delete from policyassessment where metricid >= @metricid
	delete from policymetric where metricid >= @metricid
	delete from metric where metricid >= @metricid
*/

declare @severityvalues nvarchar(256), @sql nvarchar(1024), @intval int, @intval2 int, @strval nvarchar(1024), @strval2 nvarchar(1024), @snapshotid int, @metricval nvarchar(max),
		@sevcode int, @metricthreshold nvarchar(1024), @systemdrive nchar(2), @sevcodeok int, @severity int, @version nvarchar(25), @err int
declare @tblval table (val nvarchar(1024) collate database_default)
declare @everyonesid varbinary(85), @sysadminsid varbinary(85), @builtinadminsid varbinary(85)

select @everyonesid=0x01010000000000010000000000000000000000000000000000000000000000000000000000000000,
		@sysadminsid=0x03,
		@builtinadminsid=0x01020000000000052000000020020000
select @sevcodeok=0, @sql='', @sevcode=0, @metricval='', @metricthreshold=''



/******************************************* set configuration values here *************************************/

select @snapshotid=75,
	@version='9.0.0.0',
	@severity=1, 
	@severityvalues = '''msdb''', 
	@systemdrive='C:'

/***************************************************************************************************************/


--/*	create sysadmins table if needed after snapshotid is set
if exists (select * from tempdb.dbo.sysobjects where xtype='U' and id = object_id( N'tempdb..#sysadminstbl'))
	drop table #sysadminstbl
create table #sysadminstbl (id int, name nvarchar(256))
insert into #sysadminstbl
	select distinct a.memberprincipalid, c.name 
		from serverrolemember a, serverprincipal b, serverprincipal c 
		where a.snapshotid = @snapshotid 
			and a.snapshotid = b.snapshotid 
			and a.principalid = b.principalid 
			and b.sid = @sysadminsid
			and a.snapshotid = c.snapshotid 
			and a.memberprincipalid = c.principalid
--*/

begin try
--	Put security check code here for testing. Can copy everything inside the test for metricid.
--	Note: just comment out the insert into policyassessmentdetail if block as shown below when it exists


						select @sql = N'declare dbcursor cursor static for
											select databasename
												from sqldatabase
												where snapshotid = ' + convert(nvarchar, @snapshotid) + N'
													and databasename not in (' + @severityvalues + N')
													and owner in (select name from #sysadminstbl)
												order by databasename'
						exec (@sql)
print @sql
						open dbcursor
						select @intval2 = 0
						fetch next from dbcursor into @strval
						while @@fetch_status = 0
						begin
							if (@intval2 = 1 or len(@metricval) + len(@strval) > 1010)
							begin
								if @intval2 = 0
									select @metricval = @metricval + N', more...',
											@intval2 = 1
							end
							else
								select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval + N''''

--							if (@isadmin = 1)
--								insert into policyassessmentdetail ( policyid,
--																	 assessmentid,
--																	 metricid,
--																	 snapshotid,
--																	 detailfinding,
--																	 databaseid,
--																	 objecttype,
--																	 objectid,
--																	 objectname )
--															values ( @policyid,
--																	 @assessmentid,
--																	 @metricid,
--																	 @snapshotid,
--																	 N'Database with Trustworthy bit enabled: ''' + @strval + N'''',
--																	 null, -- database ID,
--																	 N'DB', -- object type
--																	 null, -- object id
--																	 @strval )
														         
							fetch next from dbcursor into @strval
						end

						if (len(@metricval) = 0)
							select @sevcode=@sevcodeok,
									@metricval = N'None found.'
						else
							select @sevcode=@severity,
									@metricval = N'Trustworthy databases: ' + @metricval

						select @metricthreshold = N'Server is vulnerable if any unapproved database are trustworthy on SQL Server 2005 or later other than: ' + @severityvalues

						close dbcursor
						deallocate dbcursor




	print '@sql=' + @sql
	print '@sevcode=' + cast(@sevcode as nvarchar)
	print '@metricval=' + @metricval
	print '@metricthreshold=' + @metricthreshold
end try
begin catch
	print 'Line ' + cast(error_line() as nvarchar) + ': Error ' + cast(error_number() as nvarchar) + ' - ' + error_message()
end catch

if exists (select * from tempdb.dbo.sysobjects where xtype='U' and id = object_id( N'tempdb..#sysadminstbl'))
	drop table #sysadminstbl
