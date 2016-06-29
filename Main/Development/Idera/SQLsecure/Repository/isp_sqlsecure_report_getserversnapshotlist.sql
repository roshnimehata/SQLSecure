SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_report_getserversnapshotlist]'))
drop procedure [dbo].[isp_sqlsecure_report_getserversnapshotlist]
GO


CREATE procedure [dbo].[isp_sqlsecure_report_getserversnapshotlist]
(
	@registeredserverid int
)
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Return a table with a list of all snapshots for the selected server
   -- 	           

	select snapshotid, 
			convert(nvarchar, starttime, 101) + ' ' + convert(nvarchar, starttime, 108) as snapshotname
		from vwserversnapshot
		where registeredserverid = @registeredserverid 
			and [status] in (N'S', N'W')
		order by starttime desc

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_report_getserversnapshotlist] TO [SQLSecureView]

GO