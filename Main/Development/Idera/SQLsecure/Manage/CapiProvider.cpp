
#include "CapiProvider.h"

#include <string>
#include <iostream>
#include <sstream>

using std::string;
using std::ostringstream;
using std::endl;

#ifndef NTE_BUFFER_TOO_SMALL
#define NTE_BUFFER_TOO_SMALL             _HRESULT_TYPEDEF_(0x80090028L)
#endif
#ifndef NTE_INVALID_PARAMETER
#define NTE_INVALID_PARAMETER            _HRESULT_TYPEDEF_(0x80090027L)
#endif
#ifndef NTE_BUFFERS_OVERLAP
#define NTE_BUFFERS_OVERLAP              _HRESULT_TYPEDEF_(0x8009002BL)
#endif
#ifndef NTE_INVALID_HANDLE
#define NTE_INVALID_HANDLE               _HRESULT_TYPEDEF_(0x80090026L)
#endif
#ifndef NTE_DECRYPTION_FAILURE
#define NTE_DECRYPTION_FAILURE           _HRESULT_TYPEDEF_(0x8009002CL)
#endif
#ifndef NTE_INTERNAL_ERROR
#define NTE_INTERNAL_ERROR               _HRESULT_TYPEDEF_(0x8009002DL)
#endif

CapiProvider::CapiProvider(ALG_ID algId, DWORD cryptMode, int keySize) : hProvider(NULL), hKey(NULL), keyLength(keySize), algorithmID(algId), blockSize(BLOCKSIZE_16), cryptMode(cryptMode)
{
	providerIndex = -1;

	AcquireProviderContext();
	
	if(providerIndex == -1)
        throw CapiException("AcquireContext: CryptAcquireContext failed");
}

CapiProvider::~CapiProvider(void)
{
    // Destroy the key
    if( NULL != hKey ) {
        CryptDestroyKey( hKey );
        hKey = NULL;
    }

    // Destroy the provider
    if( NULL != hProvider ) {
        CryptReleaseContext( hProvider, 0 );
        hProvider = NULL;
    }
	
	providerIndex = -1;
}

void CapiProvider::AcquireProviderContext()
{
	providerIndex = -1;
	for( int i = 0; i < _countof(AesProviders); i++ )
    {
        if( CryptAcquireContext(&hProvider, 
								NULL, 
								AesProviders[i].params.lpwsz,
								AesProviders[i].params.dwType, 
								AesProviders[i].params.dwFlags)) 
		{
                providerIndex = i;
                break;
        }
    }

	if(providerIndex == -1)
        throw CapiException("AcquireContext: CryptAcquireContext failed");
}

bool CapiProvider::MaxCipherTextSize(int psize, int& csize)
{
    size_t blocks = psize / blockSize + 1;
    csize = blocks * blockSize;
	return true;
}

bool CapiProvider::MaxPlainTextSize(int csize, int& psize)
{
    if( 0 != csize % blockSize ) 
		return false;
    psize = csize;
	return true;
}

void CapiProvider::SetKey(const unsigned __int8* key)
{
    // Is someone is re-keying, we need to release the old key here...
    if(hKey != NULL) {
        // Non-fatal
        CryptDestroyKey(hKey);
        hKey  = NULL;
        ivSet = false;
    }

    if( NULL == hProvider ) {
        throw CapiException("SetKey: Provider is not valid");
    }

    if( NULL == key ) {
        throw CapiException("SetKey: Key buffer is NULL");
    }

	if (!IsKeySizeValid(keyLength)) {
        throw CapiException("SetKey: Key size is not valid");
    }

	CipherKey cipherkey;

	cipherkey.Header.aiKeyAlg = algorithmID;
    cipherkey.dwKeyLength = keyLength;

    errno_t err = 0;
    err = memcpy_s(cipherkey.cbKey, cipherkey.dwKeyLength, key, keyLength);
    if( 0 != err ) {
        throw CapiException( "SetKey: Unable to copy key" );
    }

    // When the key is imported, the size of the AesKey structure should be exact.
    //  We declared aeskey.cbKey[ KEYSIZE_256 ] to allow a stack allocation.
    //  There are enough bytes available for AES 128, 192, and 256. We do the
    //  proper book keeping now.
    const unsigned structsize = sizeof(CipherKey) - KEYSIZE_256 + keyLength;

    // Import key
    if(!CryptImportKey(hProvider, (CONST BYTE*)&cipherkey, structsize, NULL, CRYPT_NO_SALT, &hKey ) ) {
        throw CapiException("SetKey: Import key failed");
    }

    // Set Mode
    //DWORD dwMode = CRYPT_MODE_ECB;
    if(!CryptSetKeyParam(hKey, KP_MODE, (BYTE*)&cryptMode, 0)) {
        throw CapiException("SetKey: Set EBC mode failed");
    }
}

void CapiProvider::SetIv(const BYTE* iv)
{
    if( NULL == hKey ) {
        throw CapiException( "SetIv: key is not valid" );
    }

    if( NULL == iv) {
        throw CapiException( "SetIv: IV buffer is NULL" );
    }

	if(!CryptSetKeyParam(hKey, KP_IV, (BYTE*)iv, 0)) {
        throw CapiException( "SetIv: Set IV failed" );
    }

    // Set Mode
    //DWORD dwMode = CRYPT_MODE_ECB;
    if(!CryptSetKeyParam(hKey, KP_MODE, (BYTE*)&cryptMode, 0)) {
        throw CapiException("SetIv: Set EBC mode failed");
    }

    ivSet = true;
}

void CapiProvider::Encrypt( BYTE* buffer, int bsize, int psize, int& csize )
{
    // sanity check
    if(NULL == hKey) {
        SetLastError( (DWORD)NTE_BAD_KEY );
//		printf("Encrypt(1): Key is not valid\n");
        throw CapiException( "Encrypt(1): Key is not valid" );
    }

    // sanity check
    if(NULL == buffer) {
        SetLastError( ERROR_INVALID_USER_BUFFER );
//		printf("Encrypt(1): Buffer is NULL\n");
        throw CapiException( "Encrypt(1): Buffer is NULL" );
    }

    // sanity check
    int s = 0;
    if( MaxCipherTextSize( psize, s ) && bsize < s ) {
        SetLastError( (DWORD)NTE_BUFFER_TOO_SMALL );
//		printf("Encrypt(1): Buffer is too small\n");
//		printf("bsize=%d, psize=%d, s=%d, blockSize=%d\n", bsize, psize, s, blockSize);
        throw CapiException( "Encrypt(1): Buffer is too small" );
    }

    // temporary for API
    DWORD d = (DWORD)psize;
    if (!CryptEncrypt(hKey, NULL, TRUE, 0, buffer, &d, (DWORD)bsize))
    {          
		DWORD ec = GetLastError();
//        printf("CryptEncrypt returned error %d\n", ec);
		
		// Build a useful message
//        ostringstream emessage;
  //      emessage << "Encrypt(1): CryptEncrypt failed - " ;
//        emessage << ErrorToDefine(GetLastError());
//        emessage << " (0x" << std::hex << GetLastError() << ")";

        csize = 0;
        throw CapiException("CryptEncrypt failed"); // emessage.str().c_str());
    }

    csize = d;
}

void CapiProvider::Decrypt( BYTE* buffer, int bsize, int csize, int& psize )
{
    // sanity check
    if( NULL == hKey ) {
        SetLastError( (DWORD)NTE_BAD_KEY );
        throw CapiException( "Decrypt(1): Key is not valid" );
    }

    // sanity check
    if( NULL == buffer ) {
        SetLastError( ERROR_INVALID_USER_BUFFER );
        throw CapiException( "Decrypt(1): Buffer is NULL" );
    }

    // sanity check
    if( !(0 == csize % blockSize) ) {
        SetLastError( (DWORD)NTE_BAD_DATA );
        throw CapiException( "Decrypt(1): Data size is not a multple of block size" );
    }

    // sanity check
    int s = 0;
    if( MaxPlainTextSize( csize, s ) && !(bsize > s-blockSize) ) {
        SetLastError( (DWORD)NTE_BUFFER_TOO_SMALL );
        throw CapiException( "Decrypt(1): Buffer is too small" );
    }

    // Temporary for API
    DWORD d = (DWORD)csize;
    if(!CryptDecrypt( hKey, NULL, TRUE, 0, buffer, &d )) {
        // Build a useful message
        ostringstream emessage;
        emessage << "Decrypt(1): CryptDecrypt failed - " ;
        emessage << ErrorToDefine( GetLastError() );
        emessage << " (0x" << std::hex << GetLastError() << ")";

        psize = 0;
        throw CapiException( emessage.str().c_str() );
    }

    psize = d;
}

void CapiProvider::Encrypt2( const BYTE* plaintext, int psize, BYTE* ciphertext, int& csize )
{
    // sanity check
    if( !(plaintext != NULL || ( plaintext == NULL && 0 == psize )) ) {
        SetLastError( ERROR_INVALID_USER_BUFFER );
        throw CapiException( "Encrypt(2): Plain text buffer is not valid" );
    }

    // sanity check
    if( NULL == ciphertext ) {
        SetLastError( ERROR_INVALID_USER_BUFFER );
        throw CapiException( "Encrypt(2): Cipher text buffer is not valid" );
    }

	if (ciphertext != plaintext)
	{
		// Buffers cannot overlap
	    if( !(((size_t)ciphertext+csize < (size_t)plaintext) ||
			((size_t)plaintext+psize < (size_t)ciphertext) ) )
		{
			SetLastError( (DWORD)NTE_BUFFERS_OVERLAP );
			throw CapiException( "Encrypt(2): Buffers overlap" );
		}

		errno_t err = memcpy_s( ciphertext, csize, plaintext, psize );
		if( 0 != err )
			throw CapiException( "Encrypt(2): Unable to prepare plaintext buffer" );
	}

    Encrypt( ciphertext, csize, psize, csize );
}

void CapiProvider::Decrypt2( const BYTE* ciphertext, int csize, BYTE* plaintext, int& psize )
{
    // sanity check
    if( NULL == ciphertext || NULL == plaintext ) {
        SetLastError( ERROR_INVALID_USER_BUFFER );
        throw CapiException( "Decrypt(2): Buffer is NULL" );
    }

	if (plaintext != ciphertext)
	{
		// Buffers cannot overlap
		if( !(((size_t)ciphertext+csize < (size_t)plaintext) ||
			((size_t)plaintext+psize < (size_t)ciphertext) ) )
		{
			SetLastError( (DWORD)NTE_BUFFERS_OVERLAP );
			throw CapiException( "Decrypt(2): Buffers overlap" );
		}

		errno_t err = memcpy_s( plaintext, psize, ciphertext, csize );
		if( 0 != err ) 
			throw CapiException( "Decrypt(2): Unable to prepare decryption buffer" );
    }

    Decrypt( plaintext, csize, csize, psize );
}

const char* CapiProvider::ErrorToDefine(DWORD dwError)
{
    switch( dwError )
    {
    case ERROR_ACCESS_DENIED:
        return "ERROR_ACCESS_DENIED";  
    case ERROR_INVALID_HANDLE:
        return "ERROR_INVALID_HANDLE";  
    case ERROR_INVALID_PARAMETER:
        return "ERROR_INVALID_PARAMETER";
    case ERROR_DEV_NOT_EXIST:
        return "ERROR_DEV_NOT_EXIST";
    case NTE_BAD_HASH:
        return "NTE_BAD_HASH";        
    case NTE_BAD_HASH_STATE:
        return "NTE_BAD_HASH_STATE";
    case NTE_BAD_UID:
        return "NTE_BAD_UID";
    case NTE_BAD_KEY:
        return "NTE_BAD_KEY";
    case NTE_BAD_LEN:
        return "NTE_BAD_LEN";
    case NTE_BAD_DATA:
        return "NTE_BAD_DATA";
    case NTE_BAD_VER:
        return "NTE_BAD_VER";
    case NTE_BAD_ALGID:
        return "NTE_BAD_ALGID";
    case NTE_BAD_FLAGS:
        return "NTE_BAD_FLAGS";
    case NTE_BAD_TYPE:
        return "NTE_BAD_TYPE";
    case NTE_BAD_KEY_STATE:
        return "NTE_BAD_KEY_STATE";
    case NTE_NO_KEY:
        return "NTE_NO_KEY";
    case NTE_NO_MEMORY:
        return "NTE_NO_MEMORY";
    case NTE_EXISTS:
        return "NTE_EXISTS";
    case NTE_PERM:
        return "NTE_PERM";
    case NTE_NOT_FOUND:
        return "NTE_NOT_FOUND";
    case NTE_DOUBLE_ENCRYPT:
        return "NTE_DOUBLE_ENCRYPT";
    case NTE_BAD_PROVIDER:
        return "NTE_BAD_PROVIDER";
    case NTE_BAD_PROV_TYPE:
        return "NTE_BAD_PROV_TYPE";
    case NTE_BAD_KEYSET:
        return "NTE_BAD_KEYSET";
    case NTE_PROV_TYPE_NOT_DEF:
        return "NTE_PROV_TYPE_NOT_DEF";
    case NTE_PROV_TYPE_ENTRY_BAD:
        return "NTE_PROV_TYPE_ENTRY_BAD";
    case NTE_KEYSET_NOT_DEF:
        return "NTE_KEYSET_NOT_DEF";
    case NTE_KEYSET_ENTRY_BAD:
        return "NTE_KEYSET_ENTRY_BAD";
    case NTE_BAD_KEYSET_PARAM:
        return "NTE_BAD_KEYSET_PARAM";
    case NTE_FAIL:
        return "NTE_FAIL";
    case NTE_SYS_ERR:
        return "NTE_SYS_ERR";
    case NTE_SILENT_CONTEXT:
        return "NTE_SILENT_CONTEXT";
    case NTE_FIXEDPARAMETER:
        return "NTE_FIXEDPARAMETER";
    case NTE_INVALID_HANDLE:
        return "NTE_INVALID_HANDLE";
    case NTE_INVALID_PARAMETER:
        return "NTE_INVALID_PARAMETER";
    case NTE_BUFFER_TOO_SMALL:
        return "NTE_BUFFER_TOO_SMALL";
    case NTE_BUFFERS_OVERLAP:
        return "NTE_BUFFERS_OVERLAP";
    case NTE_DECRYPTION_FAILURE:
        return "NTE_DECRYPTION_FAILURE";
    case NTE_INTERNAL_ERROR:
        return "NTE_INTERNAL_ERROR";
    default: ;
    }

    return "Unknown";
}

AesCapiProvider::AesCapiProvider(ALG_ID algId, DWORD cryptMode, int keyLength) : CapiProvider(algId, cryptMode, keyLength)
{
}

void AesCapiProvider::FillCounterModeBuffer(BYTE* buffer, int& bsize, BYTE* initialCounter)
{
//	printf("Enter FillCounterModeBuffer\n");
	if ((bsize % blockSize) == 0)
		bsize -= blockSize;  // have to leave room for padding required by even block sizes

	int nBlocks = bsize / blockSize;
	
	DWORD *bptr = (DWORD *)buffer;
	DWORD *cptr = (DWORD *)initialCounter;

	DWORD counter = cptr[0];
	DWORD nonce1  = cptr[1];
	DWORD nonce2  = cptr[2];
	DWORD nonce3  = cptr[3];

	for (register int i = 0; i < nBlocks; i++)
	{
		*bptr++ = counter++;
		*bptr++ = nonce1;
		*bptr++ = nonce2;
		*bptr++ = nonce3;
	}
//	printf("Exit FillCounterModeBuffer\n");
}

DesCapiProvider::DesCapiProvider(DWORD cryptMode) : CapiProvider(CALG_DES, cryptMode, KEYSIZE_64)
{
	blockSize = BLOCKSIZE_8;
}

Rc2CapiProvider::Rc2CapiProvider(DWORD cryptMode) : CapiProvider(CALG_RC2, cryptMode, KEYSIZE_40)
{
	blockSize = BLOCKSIZE_8;
}

TripleDesCapiProvider::TripleDesCapiProvider(DWORD cryptMode) : CapiProvider(CALG_3DES_112, cryptMode, KEYSIZE_128)
{
	blockSize = BLOCKSIZE_8;
}
