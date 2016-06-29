SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_addpolicychangelog]'))
drop procedure [dbo].[isp_sqlsecure_addpolicychangelog]
GO

CREATE procedure [dbo].[isp_sqlsecure_addpolicychangelog] (@policyid int, @assessmentid int, @state nchar(1), @description nvarchar(4000))
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Add an entry to the policy change log table
   --

	declare @err int, @msg nvarchar(500)
	declare @programname nvarchar(128), @action nvarchar(32), @category nvarchar(32), @success nvarchar(32), @failure nvarchar(32)
	select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)
	select @action=N'Add', @category=N'Policy Change Log', @success=N'Success Audit', @failure=N'Failure Audit'

	-- only log values for published and approved assessments
	if (@state in (N'P', N'A'))
	begin
		BEGIN TRAN
			insert into policychangelog (
					policyid, 
					assessmentid, 
					assessmentstate, 
					changedate, 
					changedby, 
					changedescription
				) 
				values (
					@policyid, 
					@assessmentid, 
					@state, 
					GETUTCDATE(), 
					SYSTEM_USER, 
					@description
				)

			-- if there is an error writing to the change log, log the failure to the activity log
			if @err <> 0
			begin
				set @msg = 'Error: Unable to create policy change log entry. '
				exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
			end

		COMMIT TRAN
	end

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_addpolicychangelog] TO [SQLSecureView]

GO
