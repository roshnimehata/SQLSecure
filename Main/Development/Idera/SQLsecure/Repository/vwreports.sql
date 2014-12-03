SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwreports]'))
drop view [dbo].[vwreports]
GO

CREATE VIEW [dbo].[vwreports] 
AS 
select    [reportserver],
    [servervirtualdirectory],
    [managervirtualdirectory],
    [port],
    [usessl],
    [username],
    [repository],
    [targetdirectory]
 from reports

GO

GRANT SELECT ON [dbo].[vwreports] TO [SQLSecureView]

GO
