SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- SQLsecure 3.1 (Anshul Aggarwal) - New function to convert IPv4 address to bigint.
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fn_getIPAddressToInteger]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[fn_getIPAddressToInteger]
GO

create function [dbo].[fn_getIPAddressToInteger](@ip as varchar(15))
  returns bigint
as
--   -- <Idera SQLsecure version and copyright>
--   --
--   -- Description :
--   --              Get bigint value of IPv4 address.
  begin
    return (
      convert(bigint, parsename(@ip, 1)) +
      convert(bigint, parsename(@ip, 2)) * 256 +
      convert(bigint, parsename(@ip, 3)) * 65536 +
      convert(bigint, parsename(@ip, 4)) * 16777216
    )
  end

GO

GRANT EXECUTE ON [dbo].[fn_getIPAddressToInteger] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


