SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getsnapshotlist]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getsnapshotlist]
GO

CREATE FUNCTION [dbo].[getsnapshotlist]
(
	@selection datetime,
	@usebaseline bit = 0
)
RETURNS TABLE
AS
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --             Get a list of valid snapshots (Success or Warn status) for a passed date,
   --				or for the current date if @selection is null
   --
   -- Returns: snapshotid, connectionname, registeredserverid
RETURN
(
	-- Join snapshot to registeredserver to make sure the server isn't deleted
	SELECT	MAX(a.snapshotid) as snapshotid, a.connectionname, a.registeredserverid
	FROM	serversnapshot a INNER JOIN (select * from registeredserver union 
				select * from unregisteredserver) b ON a.connectionname = b.connectionname
	WHERE	(@usebaseline = 0 AND a.status IN ('S', 'W') OR a.baseline = 'Y')
			AND (@selection IS NULL	OR a.starttime < @selection)
	
	GROUP BY a.connectionname, a.registeredserverid
)

GO

GRANT SELECT ON [dbo].[getsnapshotlist] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO