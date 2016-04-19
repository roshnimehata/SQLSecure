copy /b MergeHeader.sql merge_fn.sql
for %%b in (fn*.sql) do cscript //B //U updatescriptheader.vbs %%b merge_fn.sql

copy /b MergeHeader.sql + vw*.sql merge_vw.sql

copy /b MergeHeader.sql merge_sp.sql
for %%b in (isp*.sql) do cscript //B //U updatescriptheader.vbs %%b merge_sp.sql

copy /b MergeHeader.sql + version.sql Sqlsecure_version.sql




