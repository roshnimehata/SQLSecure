SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_isversioncompatible]'))
drop procedure [dbo].[isp_sqlsecure_isversioncompatible]
GO

CREATE procedure [dbo].[isp_sqlsecure_isversioncompatible] (@dversion int, @dtype nvarchar(16))
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Checks if the version for the type (schema or dal) is compatible.
   -- 	           The compatibleversion table only has currently supported versions


      if exists (select 1 from compatibleversion where compatibleversion = @dversion and UPPER(objecttype) = @dtype)

            select 'Y'

      else

            select 'N'

 
go

GRANT EXECUTE ON [dbo].[isp_sqlsecure_isversioncompatible] TO [SQLSecureView]

GO
