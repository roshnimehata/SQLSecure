SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

create procedure [dbo].[sp_sqlsecure_deleteall] (@databasename nvarchar(128))
as
   -- Idera SQLsecure Version 0.9
   --
   -- (c) Copyright 2004-2006 Idera, a division of BBS Technologies, Inc., all rights reserved.
   -- SQLsecure, Idera and the Idera Logo are trademarks or registered trademarks
   -- of BBS Technologies or its subsidiaries in the United States and other jurisdictions.
   --
   -- Description :
   --              Delete all snapshot related tables
   -- 	           


	declare @err int
	declare @ans int
	declare @errmsg nvarchar(500)

	exec @ans = [sp_sqlsecure_isadmin]

	if (@ans = 0)
	begin
		set @errmsg = 'Error: Access denial. Insufficient privilege.'
		RAISERROR (@errmsg, 16, 1)
		return -1
	end

	BEGIN TRAN
	
		exec ('delete from ' + @databasename + '.dbo.databasepermission')
		exec ('delete from ' + @databasename + '.dbo.databaseobject')
		exec ('delete from ' + @databasename + '.dbo.databaserolemember')
		exec ('delete from ' + @databasename + '.dbo.databaseprincipal')
		exec ('delete from ' + @databasename + '.dbo.sqldatabase')
		exec ('delete from ' + @databasename + '.dbo.windowsgroupmember')
		exec ('delete from ' + @databasename + '.dbo.windowsaccount')
		exec ('delete from ' + @databasename + '.dbo.serverpermission')
		exec ('delete from ' + @databasename + '.dbo.endpoint')
		exec ('delete from ' + @databasename + '.dbo.serverrolemember')
		exec ('delete from ' + @databasename + '.dbo.fixedserverrole')
		exec ('delete from ' + @databasename + '.dbo.serverprincipal')
		exec ('delete from ' + @databasename + '.dbo.serversnapshot')

		select @err = @@error

		if @err <> 0
		begin
			ROLLBACK TRAN
			return -1
		end

	COMMIT TRAN
	
	print 'Done!'

GO