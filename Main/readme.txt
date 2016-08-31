How to change version
1) Change version number in Development\Idera\SQLsecure\Repository\version.sql
2) Add changes to all setup.rul files in Install\InstallShield\ folder (in check version cases)
3) Change version number in Build\build.number
Build info
Change commented line for VS2005 in Development\Idera\SQLsecure\UI\Console\Controls\BaseReport.Designer.cs near comment "next line replaced because of compilation error for studio 2013"