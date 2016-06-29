SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_marksnapshotbaseline]'))
drop procedure [dbo].[isp_sqlsecure_marksnapshotbaseline]
GO

CREATE procedure [dbo].[isp_sqlsecure_marksnapshotbaseline] (@snapshotid int, @baselinecomment nvarchar(500)='')
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Mark snapshot as baseline

	declare @err int
	declare @errmsg nvarchar(500)
	declare @ans int

	-- Get application program name
	declare @programname nvarchar(128)
	select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)

	declare @connectionname nvarchar(128)
	set @connectionname = NULL
		
	exec @ans = [isp_sqlsecure_isadmin]

	if (@ans = 0)
	begin
		set @errmsg = 'Error: Insufficient privileges to mark snapshot as baseline'
		exec isp_sqlsecure_addactivitylog @activitytype=N'Failure Audit', @source=@programname, @eventcode=N'Update', @category=N'Baseline', @description=@errmsg, @connectionname = @connectionname
		RAISERROR (@errmsg, 16, 1)
		return -1
	end

	-- if there is not such snapshotid then return error
	if not exists (select * from serversnapshot where snapshotid = @snapshotid)
	begin
		set @errmsg = 'Error: Snapshot id ' + CONVERT(varchar(10), @snapshotid) + ' not found'
		exec isp_sqlsecure_addactivitylog @activitytype=N'Failure Audit', @source=@programname, @eventcode=N'Update', @category=N'Baseline', @description=@errmsg, @connectionname = @connectionname
		RAISERROR (@errmsg, 16, 1)
		return -1
	end

	declare @str nvarchar(500)
	set @str = N'Update snapshot as baseline.'

	update serversnapshot set baseline = 'Y', baselinecomment = @baselinecomment where snapshotid = @snapshotid

	exec isp_sqlsecure_addactivitylog @activitytype=N'Success Audit', @source=@programname, @eventcode=N'Update', @category=N'Baseline', @description=@str, @connectionname = @connectionname

GO




