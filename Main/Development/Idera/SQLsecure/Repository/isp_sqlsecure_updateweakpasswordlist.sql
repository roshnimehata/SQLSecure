SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_updateweakpasswordlist]'))
drop procedure [dbo].[isp_sqlsecure_updateweakpasswordlist]
GO

CREATE procedure [dbo].[isp_sqlsecure_updateweakpasswordlist] ( @passwordListId int,
																@customPasswordList nvarchar(max),
																@customListUpdated DateTime,
																@additionalPasswordList nvarchar(max),
																@additionalListUpdated DateTime,
																@passwordCheckingEnabled nchar(1))
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Update policy with new info
   --			   Note system policies cannot be updated and a policy cannot be changed to or from type system

		BEGIN TRAN

		UPDATE configuration SET lastupdated = GETUTCDATE(), isweakpassworddetectionenabled = @passwordCheckingEnabled
		
		declare @rowsFound int
		
		select @rowsFound = count(*) from weakwordlist where passwordlistid = @passwordListId
		
		if (@rowsFound = 1)
		BEGIN
			UPDATE weakwordlist SET custompasswordlist = @customPasswordList,
									customlistupdated = @customListUpdated,
									additionalpasswordlist = @additionalPasswordList,
									additionallistupdated = @additionalListUpdated 
			WHERE passwordlistid = @passwordListId
		END
		ELSE
		BEGIN
			INSERT INTO weakwordlist (	custompasswordlist,
										customlistupdated,
										additionalpasswordlist,
										additionallistupdated)
							  VALUES (	@customPasswordList,
										@customListUpdated,
										@additionalPasswordList,
										@additionalListUpdated)
		END
		COMMIT TRAN

GO

