rem This bat file will create a sql script to create a database completely from scratch
rem in a development environment. It is not intended for build use.


cd DDL
call CreateDDL.bat

cd ..
call CreateMerge.bat

if exist curdir.txt del curdir.txt
echo declare @dir nvarchar(1000) > curdir.txt
echo select @dir=' >> curdir.txt
cd ..\bin\debug
cd >> ..\..\repository\curdir.txt
cd ..\..\repository
echo \Idera.SQLsecure.Collector.exe' >> curdir.txt
echo select @dir=ltrim(replace(@dir, char(13) + char(10), '')) >> curdir.txt
echo exec dbo.isp_sqlsecure_addcollectorinfo @infoname='FILEPATH', @infovalue=@dir >> curdir.txt
echo exec dbo.isp_sqlsecure_createinitialjobs >> curdir.txt


copy /b DDL\sqlsecure_ddl.sql + merge_fn.sql + merge_vw.sql + merge_sp.sql + SQLsecure_version.sql + curdir.txt createdb.sql

del curdir.txt




