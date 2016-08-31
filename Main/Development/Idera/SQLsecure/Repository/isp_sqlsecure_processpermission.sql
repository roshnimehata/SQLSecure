SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_processpermission]'))
drop procedure [dbo].[isp_sqlsecure_processpermission]
GO

CREATE procedure [dbo].[isp_sqlsecure_processpermission] (@permissiontype nchar(1)='E')
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Get process the permission set by processing the grant and deny permissions by inspecting various database rules

		
	delete from #tmppermission where objecttype IN ('U', 'V') and UPPER(permission) NOT IN ('ALTER', 'CONTROL', 'DELETE', 'INSERT', 'REFERENCES', 'SELECT', 'TAKE OWNERSHIP', 'UPDATE', 'VIEW CHANGE TRACKING', 'VIEW DEFINITION')
	delete from #tmppermission where objecttype NOT IN ('U', 'S', 'iSCM') and UPPER(permission) = 'VIEW CHANGE TRACKING'
	delete from #tmppermission where objecttype = 'iCOL' and UPPER(permission) NOT IN ('REFERENCES', 'SELECT', 'UPDATE')

	-- IF EXPLICIT TYPE THEN JUST DELETE EXTRA PERMISSION IF EXISTS THEN RETURN
	if (@permissiontype = 'X')
		return

	if exists (select 'x' from tempdb..sysobjects where type = 'U' and lower(name) like '#tmppermission%')
	begin
		--print '========= isp_sqlsecure_processpermission'

		if exists (select 'x' from tempdb..sysobjects where type = 'U' and lower(name) like '#tmpsysadmin%')
		begin

			--print '========= has a sysadmin'

			-- IF SYSADMIN ROLE THEN ALL DENY CANNOT OVERRIDE THE GRANT PERMISSION
			-- SYSADMIN HAVE ALL PRIVILEGES
			update #tmppermission set isdeny = 'N', isgrant='Y' where permissiontype = 'EF' and objecttype <> 'iCO' and principalid in (select principalid from #tmpsysadmin)
		end

		--print '========= processing deleting extra grant permission'
		--select * from #tmppermission where isdeny = 'Y'

		delete from #tmpdenypermission

		-- LAST STEP IS TO NEGATE THE PERMISSION BASED ON DBID, OBJECT, SNAPSHOTID, PERMISSION NAME
		insert into #tmpdenypermission (
		snapshotid, 
		permissionlevel,
		logintype, 
		loginname, 
		connectionname, 
		databasename, 
		principalid, 
		principalname, 
		principaltype,
		databaseprincipal, 
		databaseprincipaltype, 
		grantor, 
		grantorname,
		grantee, 
		granteename,
		classid, 
		permission, 
		parentobjectid,
		objectid, 
		objectname,
		objecttype, 
		schemaid, 
		schemaname,
		owner,
		ownername)
		select distinct
		snapshotid, 
		permissionlevel,
		logintype, 
		loginname, 
		connectionname, 
		databasename, 
		a.principalid, 
		principalname, 
		principaltype,
		databaseprincipal, 
		databaseprincipaltype, 
		grantor, 
		grantorname,
		grantee, 
		granteename,
		classid, 
		permission, 
		parentobjectid,
		objectid, 
		objectname,
		objecttype, 
		schemaid, 
		schemaname,
		owner,
		ownername
		from #tmppermission a
		where 
		permissiontype = 'EF' and 
		isdeny = 'Y'


		-- SQL SERVER SYNTAX ONLY DELETE
		-- DELETE ALL THE GRANT OR GRANT ALL PERMISSION THAT HAVE A DENY
		-- THE PERMISSION WITH DENY IS NOT DELETED
		delete #tmppermission 
		from #tmppermission a, #tmpdenypermission b
		where 
		a.snapshotid = b.snapshotid and 
		a.logintype = b.logintype and  
		a.loginname = b.loginname and  
		a.connectionname = b.connectionname and  
		a.databasename = b.databasename and  
		a.classid = b.classid and  
		a.permission = b.permission and  
		ISNULL(a.parentobjectid, '') = ISNULL(b.parentobjectid, '') and 
		a.objectid = b.objectid and  
		a.objectname = b.objectname and 
		a.objecttype = b.objecttype and  
		ISNULL(a.schemaid, '') = ISNULL(b.schemaid, '') and
		a.permissiontype = 'EF' and a.isdeny = 'N' and
		a.grantee = b.grantee and
		a.grantor = b.grantor
	end
GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_processpermission] TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

