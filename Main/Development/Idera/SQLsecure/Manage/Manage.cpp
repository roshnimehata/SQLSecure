// Idera.SQLsecure.Core.Common.cpp : Defines the entry point for the DLL application.
//
#include "Manage.h"
#include "CapiProvider.h"
#include "sha2.h"

#ifdef _MANAGED
#pragma managed(push, off)
#endif

enum CryptMode
{
	CRYPTMODE_CBC	= 1,
	CRYPTMODE_ECB	= 2
};


BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
					 )
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
	}
    return TRUE;
}

#ifdef _MANAGED
#pragma managed(pop)
#endif

__int32 Validate(const BYTE* lpInBuffer, __int32 nInSize, BYTE* lpOutBuffer, _int32 nOutSize, unsigned char *key, unsigned char* IV, bool encrypt)
{
	try
	{		
		AesCapiProvider prov(CALG_AES_256, CRYPTMODE_CBC, KEYSIZE_256);
		prov.SetKey(key);
      prov.SetIv(IV);

		if (encrypt)
		{
			prov.Encrypt2(lpInBuffer, nInSize, lpOutBuffer, nOutSize);
			return nOutSize;
		}
		else
		{
			prov.Decrypt2(lpInBuffer, nInSize, lpOutBuffer, nOutSize);
			return nOutSize;
		}
	}
	catch (...)
	{
		return 0;
	}
}


__int32 ComputeHash(const BYTE* input, __int32 inputSize, BYTE* output)
{
   try
   {
      if (input != NULL)
      {
         sha512(input, inputSize, output);
         return 1;
      }
      return 0;
   }
   catch (...)
   {
      return 0;
   }
}

