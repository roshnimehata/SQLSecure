// Setup.cpp : Defines the entry point for the application.
//

#include "stdafx.h"
#include "Setup.h"
#include "Shellapi.h"
#include "Psapi.h"

int APIENTRY _tWinMain(HINSTANCE hInstance,
                     HINSTANCE hPrevInstance,
                     LPTSTR    lpCmdLine,
                     int       nCmdShow)
{

	wchar_t szFullPath[4196], szQuotedPath[4196];
	GetModuleFileNameEx(GetCurrentProcess(),NULL,szFullPath,MAX_PATH);
    LPWSTR pTmp = wcsrchr(szFullPath,'.');
    if (pTmp) {
        ++pTmp;
        *pTmp=0;
    }
	wcscat(szFullPath,L"hta");
	swprintf(szQuotedPath,L"\"%s\"",szFullPath);

	ShellExecute(NULL, L"open", L"mshta.exe", szQuotedPath, NULL, SW_SHOWNA);

	return 0;
}
