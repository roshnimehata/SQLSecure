SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_addactivitylog]'))
drop procedure [dbo].[isp_sqlsecure_addactivitylog]
GO

CREATE procedure [dbo].[isp_sqlsecure_addactivitylog] (@activitytype nvarchar(256), @source nvarchar(64), @eventcode nvarchar(64), @category nvarchar(64), @description nvarchar(1000), @connectionname NVARCHAR(400))
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Add an entry to the application activity log table
   --

	declare @err int
	declare @errmsg nvarchar(500)

	BEGIN TRAN
	
		insert into applicationactivity (eventtimestamp, activitytype, applicationsource, connectionname, serverlogin, eventcode, category, description) values (GETUTCDATE(), @activitytype, @source, @connectionname, SYSTEM_USER, @eventcode, @category, @description)

		if @err <> 0
		begin
			set @errmsg = 'Error: Failed to insert into application activity log table'
			RAISERROR (@errmsg, 16, 1)
			ROLLBACK TRAN
			return -1
		end

	COMMIT TRAN

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_addactivitylog] TO [SQLSecureView]

GO
