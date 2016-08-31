USE master
GO

create view vw_systeminfo (instancename, authenticationmode, version, edition ) as select CONVERT(NVARCHAR(128), SERVERPROPERTY ('InstanceName')), CASE WHEN SERVERPROPERTY ('IsIntegratedSecurityOnly') = 1 THEN 'M' ELSE 'W' END , CONVERT(NVARCHAR(128),SERVERPROPERTY('ProductVersion')), CONVERT(NVARCHAR(128),SERVERPROPERTY('Edition'))
