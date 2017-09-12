
SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_removepolicy]'))
drop procedure [dbo].[isp_sqlsecure_removepolicy]
GO

CREATE procedure [dbo].[isp_sqlsecure_removepolicy]
(
	@policyid int,
	@assessmentid int = null		-- default to policy settings for backward compatibility
)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Remove all policy information for the selected policy from repository
   --				Note that system policies cannot be removed
   -- 	           

	declare @err int, @msg nvarchar(500)

	declare @programname nvarchar(128), @action nvarchar(32), @category nvarchar(32), @category2 nvarchar(32),@success nvarchar(32), @failure nvarchar(32)
	select @programname = program_name from master..sysprocesses where spid= @@spid and sid = SUSER_SID(SYSTEM_USER)
	select @action=N'Delete', @category=N'Policy', @category2=N'Assessment', @success=N'Success Audit', @failure=N'Failure Audit'

	declare @ans int
	exec @ans = [isp_sqlsecure_isadmin]

	if (@ans = 0)
	begin
		set @msg = N'Error: Insufficient privileges to ' + lower(@action) + ' a ' + lower(@category)
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end

	declare @policyname nvarchar(128), @issystem bit, @hasassessments bit, @assessmentstate nchar(1)
	select @assessmentid = isnull(@assessmentid,[dbo].[getdefaultassessmentid](@policyid))
	select @policyname=dbo.[getassessmentname](@policyid, @assessmentid) 
	select @issystem=issystempolicy from policy where policyid = @policyid  
	select @assessmentstate=assessmentstate from assessment where policyid = @policyid and assessmentid = @assessmentid

	if (@policyname is null)
	begin
		set @msg = 'Error: Invalid ' + @category + ' id ' + CONVERT(nvarchar, @policyid) + ' and assessment id ' + CONVERT(NVARCHAR, @assessmentid)
		exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
		RAISERROR (@msg, 16, 1)
		return -1
	end
	else if (@issystem = 1 and @assessmentstate = N'S')	-- system policy assessments can be deleted, but the policy settings cannot
		begin
			set @msg = 'Error: ' + @category + ' "' + @policyname + '" with id ' + CONVERT(NVARCHAR, @policyid) + ' is a system ' + lower(@category) + ' and cannot be deleted'
			exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
			RAISERROR (@msg, 16, 1)
			return -1
		end
	else if (@assessmentstate = N'A')		-- approved assessments cannot be deleted
		begin
			set @msg = 'Error: ' + @category2 + ' "' + @policyname + '" with id ' + CONVERT(NVARCHAR, @policyid) + ' and assessment id ' + CONVERT(NVARCHAR, @assessmentid) + ' is an approved ' + @category2 + ' and cannot be deleted'
			exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
			RAISERROR (@msg, 16, 1)
			return -1
		end
	else
	begin
		-- don't allow deleting the policy if there are any assessments
		declare @cid int
		select @cid = -1
		if (@assessmentstate = N'S')
		begin
			select @cid = assessmentid from assessment where policyid = @policyid and assessmentstate = N'C'
			select @hasassessments = cast(count(*) as bit)
				from assessment
				where 
					policyid = @policyid
					and assessmentid <> @assessmentid
					and assessmentstate not in (N'S',N'C')
		end
		else
			select @hasassessments = 0

		if (@hasassessments = 1)
		begin
			set @msg = 'Error: ' + @category + ' "' + @policyname + '" with id ' + CONVERT(NVARCHAR, @policyid) + ' has saved ' + @category2 + 's and cannot be deleted'
			exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
			RAISERROR (@msg, 16, 1)
			return -1
		end
		else
		begin
			BEGIN TRAN
				delete from policychangelog where policyid = @policyid and assessmentid in (@assessmentid, @cid)
				delete from policyassessmentdetail where policyid = @policyid and assessmentid in (@assessmentid, @cid)
				delete from policyassessmentnotes where policyid = @policyid and assessmentid in (@assessmentid, @cid)
				delete from policyassessment where policyid = @policyid and assessmentid in (@assessmentid, @cid)
				delete from policyinterview where policyid = @policyid and assessmentid in (@assessmentid, @cid)
				delete from policymember where policyid = @policyid and assessmentid in (@assessmentid, @cid)
				
				-- SQLsecure 3.1 (Anshul Aggarwal) - Add support for Azure SQL Database.
				delete from policymetricextendedinfo where policyid = @policyid and assessmentid in (@assessmentid, @cid)	

				delete from policymetric where policyid = @policyid and assessmentid in (@assessmentid, @cid)
				delete from assessment where policyid = @policyid and assessmentid in (@assessmentid, @cid)
				if (@assessmentstate = N'S')
					delete from policy where policyid = @policyid

				select @err = @@error

				if @err <> 0
				begin
					set @msg = 'Error: Failed to ' + lower(@action) + ' ' + lower(@category) + ' "' + @policyname + '" with id ' + CONVERT(NVARCHAR, @policyid) + ' and assessment id ' + CONVERT(NVARCHAR, @assessmentid)
					exec isp_sqlsecure_addactivitylog @activitytype=@failure, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname = null
					RAISERROR (@msg, 16, 1)
					ROLLBACK TRAN
					return -1
				end

				set @msg = @category + N' "' + @policyname + '" with id ' + CONVERT(NVARCHAR, @policyid) + ' and assessment id ' + CONVERT(NVARCHAR, @assessmentid)
				exec isp_sqlsecure_addactivitylog @activitytype=@success, @source=@programname, @eventcode=@action, @category=@category, @description=@msg, @connectionname=null

			COMMIT TRAN
		end
	end
GO



