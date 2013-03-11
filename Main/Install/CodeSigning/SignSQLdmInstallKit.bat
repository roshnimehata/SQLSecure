set WEBSITE=http://www.idera.com
set CODESIGNING=%SQLdmBUILD%\Source\Idera\SQLdm\Install\CodeSigning
set SIGNCODE=%CODESIGNING%\SignTool.exe
set PRIVATEKEY=%CODESIGNING%\ideracredentials.pfx
set TIMESTAMP=http://timestamp.verisign.com/scripts/timstamp.dll 

set SIGNINGARGS=/f %PRIVATEKEY% /p idera.redhouse.hq /t %TIMESTAMP% /du %WEBSITE%

set CDIMAGE=c:\SQLsafeCDimage
set INSTALLERS=%CDIMAGE%\intel
set SQLDMINSTKIT="%SQLdmBUILD%\Source\Idera\SQLdm\Install\SQLdmInstallationKit\Release 1\DiskImages\DISK1\SQLdmInstallationKit.exe"


@echo.   
@echo.
@echo Attempting to sign code...
@echo.

attrib -r %SQLDMINSTKIT%
 %SIGNCODE% sign %SIGNINGARGS% %SQLDMINSTKIT%


@echo Completed signing.  Ensure that all succeeded above.
