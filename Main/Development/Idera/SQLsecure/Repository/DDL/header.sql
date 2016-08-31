   -- Idera SQLsecure Version 0.9
   --
   -- (c) Copyright 2004-2006 Idera, a division of BBS Technologies, Inc., all rights reserved.
   -- SQLsecure, Idera and the Idera Logo are trademarks or registered trademarks
   -- of BBS Technologies or its subsidiaries in the United States and other jurisdictions.
   -- 
   -- Description :
   --              Creates the SQLsecure database if does not exist. 
   --              Also, creates all the objects in the repository.


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

if not exists (select * from master.dbo.sysdatabases where upper(name) = N'SQLSECURE')
begin
	create database [SQLsecure]
end

GO


USE SQLsecure
GO

-- create user view role

sp_addrole 'SQLSecureView'

