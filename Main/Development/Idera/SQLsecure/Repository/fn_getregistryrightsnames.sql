SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getregistryrightsnames]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getregistryrightsnames]
GO


CREATE  function [dbo].[getregistryrightsnames] (@Right int) returns nvarchar(512)
begin
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the object type name associated with an object type.
	declare @rightsnames nvarchar(512)
	declare @Right_QueryValues int,
			@Right_SetValue int,
			@Right_CreateSubKey int,
			@Right_EnumerateSubKeys int,
			@Right_Notify int,
			@Right_CreateLink int,
			@Right_Delete int,
			@Right_ReadPermissions int,
			@Right_WriteKey int,
			@Right_ReadKey int,
			@Right_ExecuteKey int,
			@Right_ChangePermissions int,
			@Right_TakeOwnership int,
			@Right_FullControl int

	select @Right_QueryValues			= 0x00000001,
			@Right_SetValue				= 0x00000002,
			@Right_CreateSubKey			= 0x00000004,
			@Right_EnumerateSubKeys		= 0x00000008,
			@Right_Notify				= 0x00000010,
			@Right_CreateLink			= 0x00000020,
			@Right_Delete				= 0x00010000,
			@Right_ReadPermissions		= 0x00020000,
			@Right_WriteKey				= 0x00020006,
			@Right_ReadKey				= 0x00020019,
			@Right_ExecuteKey			= 0x00020019,
			@Right_ChangePermissions	= 0x00040000,
			@Right_TakeOwnership		= 0x00080000,
			@Right_FullControl			= 0x000F003F

	-- these must be processed in the right order to prevent duplicates because of some names being combinations of other values
	if (@Right & @Right_FullControl = @Right_FullControl)
	begin
		select @rightsnames = N'Full Control'
	end
	else
	begin
		select @rightsnames = N''

		if (@Right & @Right_ReadKey = @Right_ReadKey)		-- or @Right_ExecuteKey
		begin
			select @rightsnames = @rightsnames + case when len(@rightsnames) > 0 then N', ' else N'' end + N'Read'
		end
		else
		begin
			if (@Right & @Right_QueryValues = @Right_QueryValues)
			begin
				select @rightsnames = @rightsnames + case when len(@rightsnames) > 0 then N', ' else N'' end + N'Query Value'
			end
			if (@Right & @Right_EnumerateSubKeys = @Right_EnumerateSubKeys)
			begin
				select @rightsnames = @rightsnames + case when len(@rightsnames) > 0 then N', ' else N'' end + N'Enumerate Subkeys'
			end
			if (@Right & @Right_Notify = @Right_Notify)
			begin
				select @rightsnames = @rightsnames + case when len(@rightsnames) > 0 then N', ' else N'' end + N'Notify'
			end
			if (@Right & @Right_ReadPermissions = @Right_ReadPermissions)
			begin
				select @rightsnames = @rightsnames + case when len(@rightsnames) > 0 then N', ' else N'' end + N'Read Permissions'
			end
		end

		if (@Right & @Right_WriteKey = @Right_WriteKey)
		begin
			select @rightsnames = @rightsnames + case when len(@rightsnames) > 0 then N', ' else N'' end + N'Write' 
		end
		else
		begin
			if (@Right & @Right_SetValue = @Right_SetValue)
			begin
				select @rightsnames = @rightsnames + case when len(@rightsnames) > 0 then N', ' else N'' end + N'Set Value'
			end
			if (@Right & @Right_CreateSubKey = @Right_CreateSubKey)	
			begin
				select @rightsnames = @rightsnames + case when len(@rightsnames) > 0 then N', ' else N'' end + N'Create Subkey'
			end
		end

		if (@Right & @Right_CreateLink = @Right_CreateLink)
		begin
			select @rightsnames = @rightsnames + case when len(@rightsnames) > 0 then N', ' else N'' end + N'Create Link'
		end

		if (@Right & @Right_Delete = @Right_Delete)
		begin
			select @rightsnames = @rightsnames + case when len(@rightsnames) > 0 then N', ' else N'' end + N'Delete'
		end

		if (@Right & @Right_ChangePermissions = @Right_ChangePermissions)
		begin
			select @rightsnames = @rightsnames + case when len(@rightsnames) > 0 then N', ' else N'' end + N'Write DAC'
		end

		if (@Right & @Right_TakeOwnership = @Right_TakeOwnership)
		begin
			select @rightsnames = @rightsnames + case when len(@rightsnames) > 0 then N', ' else N'' end + N'Take Ownership'
		end
	end
	
	return @rightsnames

end

GO

GRANT EXECUTE ON [dbo].[getregistryrightsnames] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

