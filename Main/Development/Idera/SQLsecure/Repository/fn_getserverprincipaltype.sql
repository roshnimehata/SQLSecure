SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getserverprincipaltype]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getserverprincipaltype]
GO


CREATE  function [dbo].[getserverprincipaltype] (@snapshotid int, @principalid int) returns nvarchar(128)
begin
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the principal type associated with a principalid.
	declare @type nvarchar(128)
	
	select @type = type from serverprincipal where snapshotid = @snapshotid and principalid = @principalid
	
	return @type

end

GO

GRANT EXECUTE ON [dbo].[getserverprincipaltype] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

