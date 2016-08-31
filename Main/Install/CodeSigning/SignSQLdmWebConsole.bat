set WEBSITE=http://www.idera.com
set CODESIGNING=%SQLdmBUILD%\Source\Idera\SQLdm\Install\CodeSigning
set SIGNCODE=%CODESIGNING%\SignTool.exe
set PRIVATEKEY=%CODESIGNING%\ideracredentials.pfx
set TIMESTAMP=http://timestamp.verisign.com/scripts/timstamp.dll 

set SIGNINGARGS=/f %PRIVATEKEY% /p idera.redhouse.hq /t %TIMESTAMP% /du %WEBSITE%

set CDIMAGE=c:\SQLsafeCDimage
set INSTALLERS=%CDIMAGE%\intel
set SQLDMWEB="%SQLdmBUILD%\Source\Idera\SQLdm\Install\SQLdm\SQLdm\Release 1\DiskImages\DISK1\SQLdmWebConsole.exe"


@echo.   
@echo.
@echo Attempting to sign code...
@echo.

attrib -r %SQLDMWEB%
 %SIGNCODE% sign %SIGNINGARGS% %SQLDMWEB%


@echo Completed signing.  Ensure that all succeeded above.
