SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getclassobjecttype]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getclassobjecttype]
GO


CREATE  function [dbo].[getclassobjecttype] 
(
	@classid int
)
returns nvarchar(128)
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the object type associated with a classid.
begin
	declare @name nvarchar(128)
	
	if (@classid = 100)
		select @name = 'iSRV'
	else if (@classid = 101)
		select @name = 'iLOGN'
	else if (@classid = 105)
		select @name = 'iENDP'
	else if (@classid = 25)
		select @name = 'iCERT'
	
	return @name

end

GO

GRANT EXECUTE ON [dbo].[getclassobjecttype] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

