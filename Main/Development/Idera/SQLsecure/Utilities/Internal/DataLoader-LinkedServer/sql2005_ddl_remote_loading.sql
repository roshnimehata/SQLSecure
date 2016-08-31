USE master
GO

create view vw_systeminfo (instancename, authenticationmode, version, edition ) as select CONVERT(NVARCHAR(128), SERVERPROPERTY ('InstanceName')), CASE WHEN SERVERPROPERTY ('IsIntegratedSecurityOnly') = 1 THEN 'M' ELSE 'W' END , CONVERT(NVARCHAR(128),SERVERPROPERTY('ProductVersion')), CONVERT(NVARCHAR(128),SERVERPROPERTY('Edition'))

CREATE TABLE srvos ([Index] integer, Name sysname, Internal_Value integer, Character_Value sysname)
exec('INSERT srvos exec master.dbo.xp_msver ''WindowsVersion'' ')

CREATE TABLE srvroles (srv_role_name sysname, srv_description sysname)
exec('INSERT srvroles EXEC master.dbo.sp_helpsrvrole')

CREATE TABLE srvrolemembers (srv_role_name sysname, role_member_name sysname, role_member_sid varbinary(85))
exec('INSERT srvrolemembers EXEC master.dbo.sp_helpsrvrolemember')
