SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vwserverprincipal]'))
drop view [dbo].[vwserverprincipal]
GO

create VIEW [vwserverprincipal] as

select distinct
				snapshotid,
				[name],
				[type],
				[sid],
				principalid,
				serveraccess,
				serverdeny,
				[disabled],
				isexpirationchecked,
				ispolicychecked,
				ispasswordnull,
				defaultdatabase,
				defaultlanguage,
				passwordstatus
	from serverprincipal
	--START(Barkha Khatri) adding 2 types (E,X) to support external users and groups
	where type IN ('U', 'G', 'S','E','X')
	--END(Barkha Khatri) adding 2 types (E,X) to support external users and groups

GO

GRANT SELECT ON [dbo].[vwserverprincipal] TO [SQLSecureView]

GO
