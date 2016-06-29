SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getfilesystemrightsnames]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getfilesystemrightsnames]
GO


CREATE  function [dbo].[getfilesystemrightsnames] (@Right int) returns nvarchar(512)
begin
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the object type name associated with an object type.
	declare @rightsnames nvarchar(512)
	declare @Right_AppendData int,
			@Right_ChangePermissions int,
			@Right_CreateDirectories int,
			@Right_CreateFiles int,
			@Right_Delete int,
			@Right_DeleteSubdirectoriesAndFiles int,
			@Right_ExecuteFile int,
			@Right_FullControl int,
			@Right_ListDirectory int,
			@Right_Modify int,
			@Right_Read int,
			@Right_ReadAndExecute int,
			@Right_ReadAttributes int,
			@Right_ReadData int,
			@Right_ReadExtendedAttributes int,
			@Right_ReadPermissions int,
			@Right_Synchronize int,
			@Right_TakeOwnership int,
			@Right_Traverse int,
			@Right_Write int,
			@Right_WriteAttributes int,
			@Right_WriteData int,
			@Right_WriteExtendedAttributes int

	select @Right_AppendData = 4,
			@Right_ChangePermissions = 262144,
			@Right_CreateDirectories = 4,
			@Right_CreateFiles = 2,
			@Right_Delete = 65536,
			@Right_DeleteSubdirectoriesAndFiles = 64,
			@Right_ExecuteFile = 32,
			@Right_FullControl = 2032127,
			@Right_ListDirectory = 1,
			@Right_Modify = 197055,
			@Right_Read = 131209,
			@Right_ReadAndExecute = 131241,
			@Right_ReadAttributes = 128,
			@Right_ReadData = 1,
			@Right_ReadExtendedAttributes = 8,
			@Right_ReadPermissions = 131072,
			@Right_Synchronize = 1048576,
			@Right_TakeOwnership = 524288,
			@Right_Traverse = 32,
			@Right_Write = 278,
			@Right_WriteAttributes = 256,
			@Right_WriteData = 2,
			@Right_WriteExtendedAttributes = 16

	-- these must be processed in the right order to prevent duplicates because of some names being combinations of other values
	if (@Right & @Right_FullControl = @Right_FullControl)
	begin
		select @rightsnames = N'Full Control'
	end
	else
	begin
		select @rightsnames = N''
		if (@Right & @Right_Modify = @Right_Modify)
		begin
			select @rightsnames = @rightsnames + case when len(@rightsnames) > 0 then N', ' else N'' end + N'Modify'
		end
		else
			begin
			if (@Right & @Right_ReadAndExecute = @Right_ReadAndExecute)
			begin
				select @rightsnames = @rightsnames + case when len(@rightsnames) > 0 then N', ' else N'' end + N'Read and Execute'
			end
			else
			begin
				if (@Right & @Right_Read = @Right_Read)
				begin
					select @rightsnames = @rightsnames + case when len(@rightsnames) > 0 then N', ' else N'' end + N'Read'
				end
				else
				begin
					if (@Right & @Right_ReadData = @Right_ReadData)		-- or @Right_ListDirectory for folders
					begin
						select @rightsnames = @rightsnames + case when len(@rightsnames) > 0 then N', ' else N'' end + N'List Folder / Read Data'
					end
					if (@Right & @Right_ReadExtendedAttributes = @Right_ReadExtendedAttributes)
					begin
						select @rightsnames = @rightsnames + case when len(@rightsnames) > 0 then N', ' else N'' end + N'Read Extended Attributes'
					end
					if (@Right & @Right_ReadAttributes = @Right_ReadAttributes)
					begin
						select @rightsnames = @rightsnames + case when len(@rightsnames) > 0 then N', ' else N'' end + N'Read Attributes'
					end
					if (@Right & @Right_ReadPermissions = @Right_ReadPermissions)
					begin
						select @rightsnames = @rightsnames + case when len(@rightsnames) > 0 then N', ' else N'' end + N'Read Permissions'
					end
				end

				if (@Right & @Right_ExecuteFile = @Right_ExecuteFile)
				begin
					select @rightsnames = @rightsnames + case when len(@rightsnames) > 0 then N', ' else N'' end + N'Traverse Folder / Execute File'
				end
			end

			if (@Right & @Right_Write = @Right_Write)
			begin
				select @rightsnames = @rightsnames + case when len(@rightsnames) > 0 then N', ' else N'' end + N'Write' 
			end
			else
			begin
				if (@Right & @Right_WriteData = @Right_WriteData)		-- or @Right_CreateFiles for folders
				begin
					select @rightsnames = @rightsnames + case when len(@rightsnames) > 0 then N', ' else N'' end + N'Create Files / Write Data'
				end
				if (@Right & @Right_AppendData = @Right_AppendData)			-- or @Right_CreateDirectories for folders
				begin
					select @rightsnames = @rightsnames + case when len(@rightsnames) > 0 then N', ' else N'' end + N'Create Folders / Append Data'
				end
				if (@Right & @Right_WriteExtendedAttributes = @Right_WriteExtendedAttributes)
				begin
					select @rightsnames = @rightsnames + case when len(@rightsnames) > 0 then N', ' else N'' end + N'Write Extended Attributes'
				end
				if (@Right & @Right_WriteAttributes = @Right_WriteAttributes)
				begin
					select @rightsnames = @rightsnames + case when len(@rightsnames) > 0 then N', ' else N'' end + N'Write Attributes'
				end
			end

			if (@Right & @Right_Delete = @Right_Delete)
			begin
				select @rightsnames = @rightsnames + case when len(@rightsnames) > 0 then N', ' else N'' end + N'Delete'
			end
		end

--	This right does not show in the file permissions in Windows, so don't show it here either
--		if (@Right & @Right_Traverse = @Right_Traverse)
--		begin
--			select @rightsnames = @rightsnames + case when len(@rightsnames) > 0 then N', ' else N'' end + N'Traverse Folder'
--		end

		if (@Right & @Right_DeleteSubdirectoriesAndFiles = @Right_DeleteSubdirectoriesAndFiles)
		begin
			select @rightsnames = @rightsnames + case when len(@rightsnames) > 0 then N', ' else N'' end + N'Delete Subfolders and Files'
		end

		if (@Right & @Right_ChangePermissions = @Right_ChangePermissions)
		begin
			select @rightsnames = @rightsnames + case when len(@rightsnames) > 0 then N', ' else N'' end + N'Change Permissions'
		end

		if (@Right & @Right_TakeOwnership = @Right_TakeOwnership)
		begin
			select @rightsnames = @rightsnames + case when len(@rightsnames) > 0 then N', ' else N'' end + N'Take Ownership'
		end

--	This right does not show in the file permissions in Windows, so don't show it here either
--		if (@Right & @Right_Synchronize = @Right_Synchronize)
--		begin
--			select @rightsnames = @rightsnames + case when len(@rightsnames) > 0 then N', ' else N'' end + N'Synchronize'
--		end
	end
	
	return @rightsnames

end

GO

GRANT EXECUTE ON [dbo].[getfilesystemrightsnames] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

