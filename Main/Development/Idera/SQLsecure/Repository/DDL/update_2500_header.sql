   -- Idera SQLsecure Version 2.5
   --
   -- (c) Copyright 2005-2009 Idera, a division of BBS Technologies, Inc., all rights reserved.
   -- SQLsecure, Idera and the Idera Logo are trademarks or registered trademarks
   -- of BBS Technologies or its subsidiaries in the United States and other jurisdictions.
   -- 
   -- Description :
   --              Update previous SQLsecure databases to version 2500


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


USE SQLsecure
GO


-- fix issues with incorrect values in security checks in version 2.0 causing comparisons not to work correctly in 2.5
declare @ver int
SELECT @ver=schemaversion FROM currentversion
if (@ver >= 2000 and @ver <2500)
BEGIN
	update policymetric 
		set severityvalues = N'' 
		where metricid in (select metricid from metric where isuserentered=0 and ismultiselect=0 and len(severityvalues) > 0)

	update policymetric 
		set reporttext = replace(reporttext, 'SQL Server Agent', 'Notification')
		where metricid=45 and reporttext like '%SQL Server Agent%'
END
 
GO
