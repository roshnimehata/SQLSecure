#pragma once

#ifndef _WIN32_WINNT		// Allow use of features specific to Windows XP or later.                   
#define _WIN32_WINNT 0x0501	// Change this to the appropriate value to target other versions of Windows.
#endif						

#pragma warning(push, 3)
# include <windows.h>
# include <wincrypt.h>
# include <exception>
#pragma warning(pop)

#pragma comment (lib, "advapi32.lib");

typedef struct PROV_PARAMS_T
{
    const WCHAR* lpwsz;
    DWORD dwType;
    DWORD dwFlags;
} PROV_PARAMS, PPROV_PARAMS;

typedef struct PROVIDERS_T {
    PROV_PARAMS params;
} PROVIDERS, PPROVIDERS;

#ifndef MS_ENH_RSA_AES_PROV_XP
#define MS_ENH_RSA_AES_PROV_XP   L"Microsoft Enhanced RSA and AES Cryptographic Provider (Prototype)"
#endif

const PROVIDERS AesProviders[] = 
{
    // http://msdn.microsoft.com/en-us/library/aa375545(VS.85).aspx
    { MS_ENH_RSA_AES_PROV, PROV_RSA_AES, CRYPT_VERIFYCONTEXT },
    { MS_ENH_RSA_AES_PROV, PROV_RSA_AES, CRYPT_VERIFYCONTEXT | CRYPT_NEWKEYSET },
    { MS_ENH_RSA_AES_PROV_XP, PROV_RSA_AES, CRYPT_VERIFYCONTEXT },
    { MS_ENH_RSA_AES_PROV_XP, PROV_RSA_AES, CRYPT_VERIFYCONTEXT | CRYPT_NEWKEYSET },
    { MS_ENHANCED_PROV, PROV_RSA_FULL, CRYPT_VERIFYCONTEXT },
    { MS_ENHANCED_PROV, PROV_RSA_FULL, CRYPT_VERIFYCONTEXT | CRYPT_NEWKEYSET }
};

class CapiException : public std::exception
{
public:
    CapiException( const char* message )
        : exception( message ) { };
};

enum KeySize   { KEYSIZE_40 = 5, KEYSIZE_64 = 8, KEYSIZE_128 = 16, KEYSIZE_192 = 24, KEYSIZE_256 = 32 };
enum BlockSize { BLOCKSIZE_8 = 8, BLOCKSIZE_16 = 16 };

class CapiProvider
{
protected:
	HCRYPTPROV hProvider;
	HCRYPTKEY  hKey;
	
	int keyLength;
	int blockSize;
	ALG_ID	algorithmID;
	DWORD cryptMode;
	
	int providerIndex;
	bool ivSet;
public:
	CapiProvider(ALG_ID algId, DWORD cryptMode, int keySize);
	virtual ~CapiProvider(void);
	virtual void AcquireProviderContext();
	virtual bool IsKeySizeValid(int ksize) { return false; }

	void SetKey(const unsigned __int8* key);
	void SetIv(const BYTE* iv);

    bool MaxCipherTextSize(int psize, int& csize);
    bool MaxPlainTextSize(int csize, int& psize);

    // Encrpyt a buffer in-place. bsize is the size of the buffer,
    //  psize is the size of the plaintext. If successful,
    //  csize is the size of the ciphertext. On entry, bsize >= csize.
    void Encrypt( BYTE* buffer, int bsize, int psize, int& csize);

    // Decrpyt a buffer in-place. bsize is the size of the buffer,
    //  csize is the size of the ciphertext. If successful,
    //  psize is the size of the recovered plaintext.
    //  On entry, bsize >= psize.
    void Decrypt(BYTE* buffer, int bsize, int csize, int& psize );

    // Encrypt plaintext. psize is the size of the plaintext.
    //  If successful, csize is the size of the ciphertext.
    void Encrypt2(const BYTE* plaintext, int psize, BYTE* ciphertext, int& csize );

    // Decrypt plaintext. csize is the size of the ciphertext.
    //  If successful, psize is the size of the plaintext.
    void Decrypt2(const BYTE* ciphertext, int csize, BYTE* plaintext, int& psize );

protected:
	typedef struct _CipherKey
    {
        BLOBHEADER Header;
        DWORD dwKeyLength;
        BYTE cbKey[KEYSIZE_256];

        _CipherKey() 
		{
            ZeroMemory( this, sizeof(_CipherKey) );
            Header.bType = PLAINTEXTKEYBLOB;
            Header.bVersion = CUR_BLOB_VERSION;
            Header.reserved = 0;
        }

		#pragma optimize( "", off )
        ~_CipherKey() 
		{                
            SecureZeroMemory( this, sizeof(this) );
        }
		#pragma optimize( "", on )
    } CipherKey;

	static const char* ErrorToDefine( DWORD dwError );
};

class AesCapiProvider : public CapiProvider
{
public:
	AesCapiProvider(ALG_ID algId, DWORD cryptMode, int keyLength);
	
	bool IsKeySizeValid(int ksize) 
	{
		return ksize == KEYSIZE_128 || ksize == KEYSIZE_256;
	}

	void FillCounterModeBuffer(BYTE* buffer, int& bsize, BYTE* initialCounter);
};

class DesCapiProvider : public CapiProvider
{
public:
	DesCapiProvider(DWORD cryptMode);
	
	bool IsKeySizeValid(int ksize) 
	{
		return ksize == KEYSIZE_64;
	}
};

class Rc2CapiProvider : public CapiProvider
{
public:
	Rc2CapiProvider(DWORD cryptMode);
	
	bool IsKeySizeValid(int ksize) 
	{
		return ksize == KEYSIZE_40;
	}
};

class TripleDesCapiProvider : public CapiProvider
{
public:
	TripleDesCapiProvider(DWORD cryptMode);
	
	bool IsKeySizeValid(int ksize) 
	{
		return ksize == KEYSIZE_128;
	}
};
