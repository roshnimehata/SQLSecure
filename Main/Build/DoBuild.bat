@echo off
if "%COMPUTERNAME%" == "SQLSECUREBLDVM" goto DOSTARTUP
echo. NOTE:  This script can only be run from the SQLSECUREBLDVM machine
goto END

REM ******************************************
REM ********** DO STARTUP ********************
REM ******************************************
:DOSTARTUP

if (%1)==(dev) GOTO BUILD_DEV
if (%1)==(official) GOTO BUILD_OFFICIAL
if (%1)==(incremental) GOTO BUILD_INCREMENTAL
if (%1)==(installdoc) GOTO BUILD_INSTALLDOCONLY
echo. BUILD ERROR: Missing or invalid command line parameter
echo.
echo. Syntax:
echo.    DOBUILD type 
echo.    type = dev, official, incremental, installdoc
echo.
goto END

:BUILD_DEV
echo.
echo. Starting Development Build
set BUILDTARGET="Build.Dev"
goto DOBUILD

:BUILD_OFFICIAL
echo.
echo. Starting Official Build
set BUILDTARGET="Build.Official"
goto DOBUILD

:BUILD_INCREMENTAL
echo.
echo. Starting Incremental Build
set BUILDTARGET="Build.Incremental"
goto DOBUILD


:BUILD_INSTALLDOCONLY
echo.
echo.  Starting InstallDoc Build
if (%2)==() GOTO NO_BUILD_SUFFIX
set BUILDTARGET="Build.InstallDocOnly"
goto DOBUILD



:NO_BUILD_SUFFIX
echo.
echo. BUILD ERROR: Missing incremental build suffix label.
echo.
goto END

REM **********************
REM *** DO BUILD SETUP ***
REM **********************
:DOBUILD

REM ********************************************************************
REM Setup user environment so the script has the appropriate permissions
REM ********************************************************************
echo.
echo. Setting up build user environment
p4 set p4port=perforce01.redhouse.hq:1666
p4 set p4user=build
p4 set p4tickets="c:\p4ticket\ticket.txt"
p4 set p4client=build_SQLsecureBldVM
call "C:\Program Files\Microsoft Visual Studio 8\Common7\Tools\vsvars32.bat"
set SQLsecureBuild=c:\Perforce\SQLsecure\Main

REM ****************************
REM Nuke the build output stuff.
REM ****************************
echo.
echo. Deleting build output directories
rd /q /s c:\perforce\SQLsecure\Main\Build\BuildOutput
rd /q /s c:\perforce\SQLsecure\Main\Build\BuildCompileOutput

echo. BuildTarget = %BUILDTARGET%

if (%BUILDTARGET%)==("Build.Incremental") GOTO INCREMENTAL_BUILD
if (%BUILDTARGET%)==("Build.InstallDocOnly") GOTO INSTALLDOCONLY_BUILD

REM ******************
REM *** FULL BUILD ***
REM ******************
:FULL BUILD

echo. Deleting development, documentation and install directories
rd /q /s c:\perforce\SQLsecure\Main\Development
rd /q /s c:\perforce\SQLsecure\Main\Documentation
rd /q /s c:\perforce\SQLsecure\Main\Install

REM ** fall though incremental as well **

REM ************************
REM *** INCREMENTALBUILD ***
REM ************************
:INCREMENTAL_BUILD

REM *********************************************************
REM Make sure we have the latest build script.  
REM Everything else should be handled within the build script
REM *********************************************************
echo.
echo. Fetching the latest build script
p4 sync -f //sqlsecure/Main/Build/...

REM ************************
REM Execute the build script
REM ************************
echo.
echo. Building the specified target.
nant.exe -f:SQLsecure.build %BUILDTARGET% -l:c:\perforce\SQLsecure\Main\build.log -logger:NAnt.Core.MailLogger
GOTO END

REM **********************
REM *** DOC ONLY BUILD ***
REM **********************
:INSTALLDOCONLY_BUILD
echo.
echo. Building the specified target.
nant.exe -f:SQLsecure.build %BUILDTARGET% -D:SQLsecure.Incremental.Label=%2 -l:c:\perforce\SQLsecure\Main\build.log -logger:NAnt.Core.MailLogger
GOTO END


:END

echo.  Build script execution is complete