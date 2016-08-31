SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getauditflagsnames]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getauditflagsnames]
GO


CREATE  function [dbo].[getauditflagsnames] (@AuditFlags int) returns nvarchar(512)
begin
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the access type name associated with an os object permission.
	declare @name nvarchar(512)

	if (@AuditFlags & 1 = 1)
	begin
		select @name = N'Successful' 
	end

	if (@AuditFlags & 2 = 2)
	begin
		select @name = @name + case when len(@name) > 0 then N' and ' else N'' end + N'Failed' 
	end

	return @name

end

GO

GRANT EXECUTE ON [dbo].[getauditflagsnames] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

