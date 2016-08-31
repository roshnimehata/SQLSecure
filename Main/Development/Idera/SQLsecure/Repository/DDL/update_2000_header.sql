   -- Idera SQLsecure Version 2.0
   --
   -- (c) Copyright 2005-2008 Idera, a division of BBS Technologies, Inc., all rights reserved.
   -- SQLsecure, Idera and the Idera Logo are trademarks or registered trademarks
   -- of BBS Technologies or its subsidiaries in the United States and other jurisdictions.
   -- 
   -- Description :
   --              Update previous SQLsecure databases to version 2000


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


USE SQLsecure
GO


-- fix issues in schema 900 before upgrading to 2000
declare @ver int
SELECT @ver=schemaversion FROM currentversion
if (isnull(@ver,900) = 900)
BEGIN
	-- ancillarywindowsgroup is not linked to snapshot correctly and not deleted when a snapshot is deleted
	DELETE FROM ancillarywindowsgroup WHERE snapshotid not in (SELECT snapshotid FROM serversnapshot)

	IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FK_ancillarywindowsgroup_serversnapshot]') AND type = 'F')
		ALTER TABLE [dbo].[ancillarywindowsgroup]  WITH CHECK ADD CONSTRAINT [FK_ancillarywindowsgroup_serversnapshot]
			FOREIGN KEY([snapshotid])
			REFERENCES [dbo].[serversnapshot] ([snapshotid])

	ALTER TABLE [dbo].[ancillarywindowsgroup] CHECK CONSTRAINT [FK_ancillarywindowsgroup_serversnapshot]
END
GO

-- fix primary keys on permissions objects to avoid duplicates with multiple permission grants from different grantors

declare @ver int
SELECT @ver=schemaversion FROM currentversion
if (isnull(@ver,900) = 900)
BEGIN
	-- serverpermission table

	-- to make it restartable, check if the index doesn't exist or the grantor column is not in the primary key
	IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[PK__serverpermission__2B3F6F97]') AND type = N'K')
		OR NOT EXISTS (SELECT a.id FROM dbo.sysindexes a, dbo.sysindexkeys b, dbo.syscolumns c WHERE a.name = N'PK__serverpermission__2B3F6F97' and a.id = b.id and a.indid = b.indid and b.id = c.id and b.colid = c.colid and c.name = 'grantor')
	BEGIN
		UPDATE [serverpermission] SET [grantor]=1 WHERE [grantor] is null

		ALTER TABLE [serverpermission]
			ALTER COLUMN [grantor] int not null

		IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[PK__serverpermission__2B3F6F97]') AND type = N'K')
			ALTER TABLE [serverpermission]
				DROP CONSTRAINT [PK__serverpermission__2B3F6F97] 

		ALTER TABLE [serverpermission]
			ADD CONSTRAINT [PK__serverpermission__2B3F6F97] PRIMARY KEY CLUSTERED 
			(
				[snapshotid] ASC,
				[majorid] ASC,
				[minorid] ASC,
				[classid] ASC,
				[permission] ASC,
				[grantee] ASC,
				[grantor] ASC
			)
	END
END

GO

declare @ver int
SELECT @ver=schemaversion FROM currentversion
if (isnull(@ver,900) = 900)
BEGIN
	-- databaseprincipalpermission table

	-- to make it restartable, check if the index doesn't exist or the grantor column is not in the primary key
	IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[PK__databaseprincipa__0CBAE877]') AND type = N'K')
		OR NOT EXISTS (SELECT a.id FROM dbo.sysindexes a, dbo.sysindexkeys b, dbo.syscolumns c WHERE a.name = N'PK__databaseprincipa__0CBAE877' and a.id = b.id and a.indid = b.indid and b.id = c.id and b.colid = c.colid and c.name = 'grantor')
	BEGIN
		UPDATE [databaseprincipalpermission] SET [grantor]=1 WHERE [grantor] is null

		ALTER TABLE [databaseprincipalpermission]
			ALTER COLUMN [grantor] int not null

		IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[PK__databaseprincipa__0CBAE877]') AND type = N'K')
			ALTER TABLE [databaseprincipalpermission]
				DROP CONSTRAINT [PK__databaseprincipa__0CBAE877] 

		ALTER TABLE [databaseprincipalpermission]
			ADD CONSTRAINT [PK__databaseprincipa__0CBAE877] PRIMARY KEY CLUSTERED 
			(
				[snapshotid] ASC,
				[dbid] ASC,
				[uid] ASC,
				[classid] ASC,
				[permission] ASC,
				[grantee] ASC,
				[grantor] ASC
			)
	END
END

GO

declare @ver int
SELECT @ver=schemaversion FROM currentversion
if (isnull(@ver,900) = 900)
BEGIN
	-- databaseschemapermission table

	-- to make it restartable, check if the index doesn't exist or the grantor column is not in the primary key
	IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[PK__databaseschemape__07020F21]') AND type = N'K')
		OR NOT EXISTS (SELECT a.id FROM dbo.sysindexes a, dbo.sysindexkeys b, dbo.syscolumns c WHERE a.name = N'PK__databaseschemape__07020F21' and a.id = b.id and a.indid = b.indid and b.id = c.id and b.colid = c.colid and c.name = 'grantor')
	BEGIN
		UPDATE [databaseschemapermission] SET [grantor]=1 WHERE [grantor] is null

		ALTER TABLE [databaseschemapermission]
			ALTER COLUMN [grantor] int not null

		IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[PK__databaseschemape__07020F21]') AND type = N'K')
			ALTER TABLE [databaseschemapermission]
				DROP CONSTRAINT [PK__databaseschemape__07020F21] 

		ALTER TABLE [databaseschemapermission]
			ADD CONSTRAINT [PK__databaseschemape__07020F21] PRIMARY KEY CLUSTERED 
			(
				[snapshotid] ASC,
				[dbid] ASC,
				[schemaid] ASC,
				[classid] ASC,
				[permission] ASC,
				[grantee] ASC,
				[grantor] ASC
			)
	END
END

GO

declare @ver int
SELECT @ver=schemaversion FROM currentversion
if (isnull(@ver,900) = 900)
BEGIN
	-- databaseobjectpermission table

	-- to make it restartable, check if the index doesn't exist or the grantor column is not in the primary key
	IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[PK__databaseobjectpe__3F466844]') AND type = N'K')
		OR NOT EXISTS (SELECT a.id FROM dbo.sysindexes a, dbo.sysindexkeys b, dbo.syscolumns c WHERE a.name = N'PK__databaseobjectpe__3F466844' and a.id = b.id and a.indid = b.indid and b.id = c.id and b.colid = c.colid and c.name = 'grantor')
	BEGIN
		UPDATE [databaseobjectpermission] SET [grantor]=1 WHERE [grantor] is null

		ALTER TABLE [databaseobjectpermission]
			ALTER COLUMN [grantor] int not null

		IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[PK__databaseobjectpe__3F466844]') AND type = N'K')
			ALTER TABLE [databaseobjectpermission]
				DROP CONSTRAINT [PK__databaseobjectpe__3F466844] 

		ALTER TABLE [databaseobjectpermission]
			ADD CONSTRAINT [PK__databaseobjectpe__3F466844] PRIMARY KEY CLUSTERED 
			(
				[snapshotid] ASC,
				[dbid] ASC,
				[objectid] ASC,
				[parentobjectid] ASC,
				[classid] ASC,
				[permission] ASC,
				[grantee] ASC,
				[grantor] ASC
			)
	END
END

GO

declare @ver int
SELECT @ver=schemaversion FROM currentversion
if (isnull(@ver,900) = 900)
BEGIN
	-- drop unused tables
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sqlserverobjecttype]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
		DROP TABLE [dbo].[sqlserverobjecttype]

	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[serverstatistic]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
		DROP TABLE [dbo].[serverstatistic]
END

GO
