SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwcurrentversion]'))
drop view [dbo].[vwcurrentversion]
GO

CREATE VIEW [dbo].[vwcurrentversion] as
select top 1 dalversion, schemaversion 
from currentversion 
order by dalversion, schemaversion

GO

GRANT SELECT ON [dbo].[vwcurrentversion] TO [SQLSecureView]

GO
