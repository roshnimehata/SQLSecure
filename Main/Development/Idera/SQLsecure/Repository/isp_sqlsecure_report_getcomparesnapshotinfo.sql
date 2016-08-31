SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_report_getcomparesnapshotinfo]'))
drop procedure [dbo].[isp_sqlsecure_report_getcomparesnapshotinfo]
GO


CREATE procedure [dbo].[isp_sqlsecure_report_getcomparesnapshotinfo]
(
	@snapshotid int,
	@snapshotid2 int
)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Return a table with a list of all snapshots for the selected server
   -- 	           

	select *, convert(nvarchar, starttime, 101) + ' ' + convert(nvarchar, starttime, 108) as snapshotname
		from vwserversnapshot
		where snapshotid in (@snapshotid, @snapshotid2) 
		order by snapshotid desc

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_report_getcomparesnapshotinfo] TO [SQLSecureView]

GO