SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwapplicationlicense]'))
drop view [dbo].[vwapplicationlicense]
GO

CREATE VIEW [dbo].[vwapplicationlicense] 
AS SELECT 
	licenseid,
	licensekey,
	createdtm,
	createdby
 FROM [applicationlicense]
 
 GO
 
 GRANT SELECT ON [dbo].[vwapplicationlicense] TO [SQLSecureView]
 
 GO
