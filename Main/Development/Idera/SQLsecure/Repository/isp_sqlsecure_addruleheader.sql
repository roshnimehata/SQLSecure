SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_addruleheader]'))
drop procedure [dbo].[isp_sqlsecure_addruleheader]
GO

CREATE procedure [dbo].[isp_sqlsecure_addruleheader] (@connectionname nvarchar(500), @rulename nvarchar(256), @description nvarchar(80))
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Add a new rule header and return the new header id back to the sp caller
   -- 	           

	declare @currenttm datetime
	declare @result int
	declare @err int
	declare @errmsg nvarchar(500)
	declare @ans int

	declare @programname nvarchar(128)
	select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)

	exec @ans = [isp_sqlsecure_isadmin]

	if (@ans = 0)
	begin
		set @errmsg = 'Error: Insufficient privileges to add filter for SQL Server instance ' + @connectionname
		exec isp_sqlsecure_addactivitylog @activitytype=N'Failure Audit', @source=@programname, @eventcode=N'Add', @category=N'Filter', @description=@errmsg, @connectionname = @connectionname
		RAISERROR (@errmsg, 16, 1)
		return -1
	end

	set @currenttm = GETUTCDATE()
	
	BEGIN TRAN
	
		insert into filterruleheader (connectionname, rulename, description, createdby, createdtm, lastmodifiedby, lastmodifiedtm) values (@connectionname, @rulename, @description, SYSTEM_USER, @currenttm, SYSTEM_USER,  @currenttm)
	
		select @err = @@error
	
		if @err <> 0
		begin
			set @errmsg = 'Error: Failed to insert into filterruleheader table with connection name ' + @connectionname
			RAISERROR (@errmsg, 16, 1)
			ROLLBACK TRAN
			return -1
		end
	
		if object_id('#tmpruleheader') is null 
		begin
			create table #tmpruleheader (data int)
		end
	
		delete from #tmpruleheader
	
		insert into #tmpruleheader (data) (select filterruleheaderid from filterruleheader where createdtm = @currenttm)
	
	
		select @err = @@error
	
		if @err <> 0
		begin
			set @errmsg = 'Error: Failed to insert into filterruleheader table with connection name ' + @connectionname
			RAISERROR (@errmsg, 16, 1)
			ROLLBACK TRAN
			return -1
		end
	
		declare @str nvarchar(500)
		set @str = N'Added filter ' + @rulename + N' for SQL Server instance ' + @connectionname
	
		exec isp_sqlsecure_addactivitylog @activitytype=N'Success Audit', @source=@programname, @eventcode=N'Add', @category=N'Filter', @description=@str, @connectionname = @connectionname

	COMMIT TRAN

	exec('select * from #tmpruleheader')
	
GO

