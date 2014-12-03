SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getserverprincipalidwithsid]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getserverprincipalidwithsid]
GO


CREATE  function [dbo].[getserverprincipalidwithsid] (@snapshotid int, @sid varbinary(85)) returns int
begin
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get the principalid associated with a sid.
	declare @id int
	
	select @id = principalid from serverprincipal where snapshotid = @snapshotid and sid = @sid
	
	return @id

end

GO

GRANT EXECUTE ON [dbo].[getserverprincipalidwithsid] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

