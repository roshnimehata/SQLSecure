SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getserverprincipalnamewithsid]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getserverprincipalnamewithsid]
GO


CREATE  function [dbo].[getserverprincipalnamewithsid] (@snapshotid int, @sid varbinary(85)) returns nvarchar(128)
begin
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the principal name associated with a sid.
	declare @name nvarchar(128)
	
	select @name = name from serverprincipal where snapshotid = @snapshotid and sid = @sid
	
	return @name

end

GO

GRANT EXECUTE ON [dbo].[getserverprincipalnamewithsid] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

