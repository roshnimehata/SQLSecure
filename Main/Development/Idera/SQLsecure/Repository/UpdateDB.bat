rem This bat file will create a sql script to create a database completely from scratch
rem in a development environment. It is not intended for build use.


cd DDL
call UpdateDDL.bat

cd ..
call CreateMerge.bat

copy /b DDL\sqlsecure_ddl_update.sql + merge_fn.sql + merge_vw.sql + merge_sp.sql + SQLsecure_version.sql updatedb.sql





