SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_getcompatibleversions]'))
drop procedure [dbo].[isp_sqlsecure_getcompatibleversions]
GO

CREATE procedure [dbo].[isp_sqlsecure_getcompatibleversions] (@dtype nvarchar(16))
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Return all compatible version correspond to the given type. Type can be schema or dal
   -- 	           

	exec('select compatibleversion from compatibleversion where UPPER(objecttype) = UPPER(''' + @dtype + ''')')

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_getcompatibleversions]  TO [SQLSecureView]

GO
