Option Explicit

' *******************************************************************
'
' SQLsecure install script used to copy individual t-sql script files
' and update version and copyright info in the script header comments
'
' *******************************************************************

dim objFSO, objFile, objReadFile, objWriteFile
dim strFileIn, strFileOut, strContents
dim strSearch, strReplace
dim strVersion

' these are the values to search for and replace with current info
strSearch = "-- <Idera SQLsecure version and copyright>"

' *******************************************************************
'
' UPDATE LATEST VERSION HERE
'
' *******************************************************************
strVersion = "2.7"

strReplace = "-- Idera SQLsecure Version " + strVersion + vbCRLF _
            + "   --" + vbCRLF _
            + "   -- (c) Copyright 2005-" + Cstr(Year(Date())) + " Idera, Inc., all rights reserved." + vbCRLF _
            + "   -- SQLsecure, Idera and the Idera Logo are trademarks or registered trademarks" + vbCRLF _
            + "   -- of Idera, Inc. or its subsidiaries in the United States and other jurisdictions."

If Wscript.Arguments.Length = 2 Then
	strFileIn = Wscript.Arguments(0)
	strFileOut = Wscript.Arguments(1)
	'Wscript.echo strFileIn, strFileOut

	Set objFSO = CreateObject("Scripting.FileSystemObject")
	Set objFile = objFSO.GetFile(strFileIn)
	If objFile.Size > 0 Then
    	Set objReadFile = objFSO.OpenTextFile(strFileIn, 1)
    	strContents = objReadFile.ReadAll
		strContents = Replace(strContents, strSearch, strReplace)

		Set objWriteFile = objFSO.OpenTextFile (strFileOut, 8, True)

		objWriteFile.WriteLine(strContents)

    		'Wscript.Echo strContents
    		objReadFile.Close
    		objWriteFile.Close
	Else
    		Wscript.Echo "The file is empty."
	End If
Else
    	Wscript.Echo "Wrong number of arguments passed. It must always be 2: FileIn and FileOut."
End If
