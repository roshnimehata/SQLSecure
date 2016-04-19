   -- Idera SQLsecure Version 2.6
   --
   -- (c) Copyright 2005-2010 Idera, a division of BBS Technologies, Inc., all rights reserved.
   -- SQLsecure, Idera and the Idera Logo are trademarks or registered trademarks
   -- of BBS Technologies or its subsidiaries in the United States and other jurisdictions.
   -- 
   -- Description :
   --              Update previous SQLsecure databases to version 2600


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


USE SQLsecure
GO


-- fix issues with incorrect values in security checks in version 2.5
--declare @ver int
--SELECT @ver=schemaversion FROM currentversion
--if (@ver >= 2500 and @ver <2600)
--BEGIN
--
--	--UPDATE HERE
--
--END
 
GO
