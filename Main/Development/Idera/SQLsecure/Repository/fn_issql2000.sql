SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[issql2000]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[issql2000]
GO


CREATE  function [dbo].[issql2000] (@snapshotid int) returns nchar(1)
begin
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the version and return Y if SQL 2000 or N if greater.
	declare @fg nchar(1)
	
	if exists (select * from serversnapshot where snapshotid = @snapshotid and version like '8.%')
		set @fg = 'Y'
	else
		set @fg = 'N'
	
	return @fg

end

GO

GRANT EXECUTE ON [dbo].[issql2000] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

