#include "stdafx.h"

using namespace std;

void printLogSettings(char *logIn, DWORD typeIn, DWORD daysIn)
{
   cout << logIn << ", TYPE : ";
   switch(typeIn)
   {
   case 0:
      cout << "Overwrite as needed, RETENTION : ";
      break;

   case 1:
      cout << "Do not overwrite, RETENTION : ";
      break;

   case 2:
      cout << "Overwrite events older then, RETENTION : ";
      break;

   default:
      cout << "ERROR, RETENTION : ";
      break;
   }
   cout << daysIn << endl;
}

void main(int argc, char *argv[])
{
//domain=host1&product=SQLsafe.0100.29991231.22&license=JwVGd
//domain=host1&product=SQLsafe_Enterprise.0100.29991231.22&license=lsop*

   TCHAR *lic = "host1&product=SQLsafe.0100.29991231.22&license=JwVGd";

   DWORD rc = ValidateSQLsafeLicense(lic);

#if 0
//   VerifyPassword("redhouse\\svaidya","svaidya");

   //DWORD xx = AddNtfsPermissionsBySid ("S-1-5-32-544","C:\\Public",GENERIC_ALL);
   //DWORD xx = AddNtfsPermissions ("redhouse\\svaidya","C:\\Public",GENERIC_ALL);
   DWORD xx = SetObjectAccess("S-1-5-32-544",1,"C:\\test",FILE_ALL_ACCESS);
   cout << "Dir Code : " << xx << endl;
   xx = SetObjectAccess("S-1-5-32-544",4,"MACHINE\\Software\\Idera\\Compliance",KEY_ALL_ACCESS);
   cout << "Reg Code : " << xx << endl;
#if 0
   DWORD xx = CreateDirAndGiveFullControl ("C:\\test1","S-1-5-32-544");
   WCHAR *in = L"0041",//L"0041004200430044",
         out[100];
   DWORD xx = ConvertHexEncodedString(in,out);
#endif
#if 0
   DWORD dtype = 0;
   int z = GetMediaType (0,&dtype);
   z = GetMediaType ("c:",&dtype);
   z = GetMediaType ("c",&dtype);
   z = GetMediaType ("c:\\",&dtype);

	if (argc < 2 || argc >= 3) 
	{
		cout << "ERROR : invalid arguments" << endl;
		cout << "USAGE : UnitTest <domain\\user name>" << endl;
		return;
	}

	int x = GiveLogonAsServicePriv(argv[1]);
	if (x != 0)
	{
		cout << "ERROR giving privs to " << argv[1] 
			<< " code : " << x << endl;
	}
	else
	{
		cout << "Assigned logon on as service privs to " 
			<< argv[1] << endl;
	}

	x = GiveActAsPartOfOSPriv(argv[1]);
	if (x != 0)
	{
		cout << "ERROR giving privs to " << argv[1] 
			<< " code : " << x << endl;
	}
	else
	{
		cout << "Assigned logon on as service privs to " 
			<< argv[1] << endl;
	}

   DWORD type, days;
   x = GetEventLogRetentionInfo("Application",&type,&days);
   printLogSettings("Application",type,days);
   x = GetEventLogRetentionInfo("System",&type,&days);
   printLogSettings("System",type,days);
   x = GetEventLogRetentionInfo("Security",&type,&days);
   printLogSettings("Security",type,days);
#endif

#endif
}