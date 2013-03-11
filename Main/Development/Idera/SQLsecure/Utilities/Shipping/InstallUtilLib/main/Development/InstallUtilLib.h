//**********************************************************************
//*
//* File: InstallUtilLib.h
//*
//* Copyright Idera (BBS Technologies) 2005
//*
//**********************************************************************
#pragma once

extern "C" {
   //----------------------------------------------------------------------------------------
   // Assign specific privileges to users.
   //----------------------------------------------------------------------------------------
	DWORD __stdcall
		GiveLogonAsServicePriv (
			LPCTSTR  accountIn            // account in domain\user format
		);

	DWORD __stdcall
		GiveActAsPartOfOSPriv (
			LPCTSTR  accountIn            // account in domain\user format
		);

   //----------------------------------------------------------------------------------------
   // Verify password.
   //----------------------------------------------------------------------------------------
   DWORD __stdcall
      VerifyPassword (
			LPCTSTR  accountIn,           // account in domain\user format
			LPCTSTR  passwordIn           // password
   );

   //----------------------------------------------------------------------------------------
   // Set permissions (directories, registries, files, etc.)
   //----------------------------------------------------------------------------------------
   DWORD __stdcall
      AddNtfsPermissions (
			LPCTSTR  accountIn,           // account in domain\user format
			LPCTSTR  fileOrDirPathIn,     // file or directory path.
         DWORD    accessMaskIn         // access rights.
   );

   DWORD __stdcall
      AddNtfsPermissionsBySid (
			LPCTSTR  sidIn,               // SID string.
			LPCTSTR  fileOrDirPathIn,     // file or directory path.
         DWORD    accessMaskIn         // access rights.
   );

   DWORD __stdcall
      CreateDirAndGiveFullControl (
			LPCTSTR  dirPathIn,           // Path of directory to create.
			LPCTSTR  sidIn                // Account SID to give full control to.
   );

   DWORD __stdcall
      SetObjectAccess (
			LPCTSTR  sidIn,               // SID string.
         DWORD    objTypeIn,           // Object type 
                                       // 1 - file/dir (it doesn't distinguish between file and dir).
                                       // 4 - registry.
			LPCTSTR  pathIn,              // Object path.
         DWORD    accessMaskIn         // Access rights.
   );

   //----------------------------------------------------------------------------------------
   // NT event log utility functions.
   //----------------------------------------------------------------------------------------
	DWORD __stdcall
		GetEventLogRetentionInfo (
			LPCTSTR   eventLogNameIn,     // event log name - Application, System, Security, etc.
         DWORD    *retentionTypeOut,   // retention type - 0:overwrite events as needed, 1: do not overwrite, 2:overwrite events older then N days.
         DWORD    *retentionDaysOut    // retention days - if retentionTypeOut is 2, than this value is number of days events are retained.
		);

   //----------------------------------------------------------------------------------------
   // Drive directory utility functions.
   //----------------------------------------------------------------------------------------
	DWORD __stdcall
		GetMediaType (
			LPCTSTR  driveIn,             // drive letter in c:\ format.
         DWORD   *driveTypeOut         // drive type returned from the API.
		);

   //----------------------------------------------------------------------------------------
   // License validation functions.
   //----------------------------------------------------------------------------------------
	DWORD __stdcall
		ValidateSQLsafeLicense (
			LPCTSTR  licenseStringIn      // license string.
		);

   //----------------------------------------------------------------------------------------
   // Misc. functions.
   //----------------------------------------------------------------------------------------
   DWORD __stdcall
      ConvertHexEncodedString (
         WCHAR *stringIn,              // Hex coded string in.
         WCHAR *stringOut              // Decoded string out.
      );
}
