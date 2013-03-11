SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getserverprincipalname]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getserverprincipalname]
GO


CREATE  function [dbo].[getserverprincipalname] (@snapshotid int, @principalid int) returns nvarchar(128)
begin
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the principal name associated with a principalid.
	declare @name nvarchar(128)
	
	select @name = name from serverprincipal where snapshotid = @snapshotid and principalid = @principalid
	
	return @name

end

GO

GRANT EXECUTE ON [dbo].[getserverprincipalname] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

