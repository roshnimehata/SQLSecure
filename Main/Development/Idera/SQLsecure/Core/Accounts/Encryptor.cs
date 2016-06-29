/******************************************************************
 * Name: Encryptor.cs
 *
 * Description: Provides text encryption functions.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace Idera.SQLsecure.Core.Accounts
{
    public static class Encryptor
    {
        #region Constants

        public enum HashVersion
        {
            SHA1,
            SHA512
        }

        private static byte[] m_Key = new byte[] { 6, 26, 73, 251, 99, 83, 166, 99, 60, 128, 119, 76, 
                                      (byte) 'U', (byte) 's', (byte) 'h', (byte) 'a', (byte) ' ', (byte) 'M',
                                      (byte) 'a', (byte) 'd', (byte) 'h', (byte) 'u', (byte) 'k', (byte) 'a',
                                      (byte) 'r', (byte) ' ', (byte) 'V',  (byte) 'a',  (byte) 'i',  (byte) 'd',
                                      (byte) 'y', (byte) 'a'
                                    };
        private static byte[] m_IV = new byte[] { 6, 26, 73, 251, 99, 83, 166, 99, 60, 128, 119, 76, 8, 165, 226, 66 };

        
        private static CryptDecrypt ApiCryptDecrypt;

        #endregion

        #region Methods

        static Encryptor()
        {
           if (Environment.OSVersion.Version < new Version(5, 1))
           {
              ApiCryptDecrypt = Encryptor.CryptDecryptNET;
           }
           else
           {
               ApiCryptDecrypt = Encryptor.Validate;
           }
        }

        private static string decryptedString(byte[] decryptedBuf)
        {
            // If null or empty buffer return empty string.
            if (decryptedBuf == null || decryptedBuf.Length == 0)
            {
                return string.Empty;
            }

            // If buffer elements are 0, then return empty string.
            bool isAllZero = true;
            for (int i = 0; i < decryptedBuf.Length; ++i)
            {
                isAllZero = isAllZero && decryptedBuf[i] == 0;
            }
            if (isAllZero) { return string.Empty; }

            // Convert the buffer to a string, removing any 
            // filler \0 characters.
            string ret = (new ASCIIEncoding()).GetString(decryptedBuf);
            int nullCharStart = ret.IndexOf('\0', 0);
            if (nullCharStart != -1) { ret = ret.Remove(nullCharStart); }

            return ret;
        }

        public static string Encrypt(string text)
        {
            return Encrypt(text, m_Key);
        }

        public static string Encrypt(string text, byte[] m_Key)
        {
            // Get the source text.
            string srcText = text != null ? text : string.Empty;

            //Convert the data to a byte array.
            byte[] toEncrypt = (new ASCIIEncoding()).GetBytes(srcText);
            byte[] outBuffer = new byte[toEncrypt.Length * 2];

            //encrypt
            int outSize = ApiCryptDecrypt(toEncrypt, toEncrypt.Length, outBuffer, outBuffer.Length, m_Key, m_IV, true);

            //Get encrypted array of bytes.
            byte[] encrypted = new byte[outSize];
            Buffer.BlockCopy(outBuffer, 0, encrypted, 0, outSize);

            
            return Convert.ToBase64String(encrypted);
        }

        public static string Decrypt(string cipherText)
        {
            return Decrypt(cipherText, m_Key);
        }

        public static string Decrypt(string cipherText, byte[] m_Key)
        {
            // Decode the cipher text.
            byte[] cipherBuf = Convert.FromBase64String(cipherText);
            byte[] outBuffer = new byte[cipherBuf.Length * 2];

            //Get decrypted array of bytes.
            int outSize = ApiCryptDecrypt(cipherBuf, cipherBuf.Length, outBuffer, outBuffer.Length, m_Key, m_IV, false);

            byte[] fromEncrypt = new byte[outSize];
            Buffer.BlockCopy(outBuffer, 0, fromEncrypt, 0, outSize);

            //Convert the byte array back into a string.
            return decryptedString(fromEncrypt);
        }

        public static byte[] GenerateHash(byte[] input, HashVersion version)
        {
            byte[] output = null;

            switch (version)
            {
                case HashVersion.SHA1:
                    output = GenerateSHA1Hash(input);
                    break;
                case HashVersion.SHA512:
                    output = GenerateSHA512Hash(input);
                    break;
                default:
                    output = null;
                    break;
            }
            return output;
        }

        private static byte[] GenerateSHA1Hash(byte[] input)
        {
            SHA1 hash = SHA1CryptoServiceProvider.Create();
            return hash.ComputeHash(input);
        }

        private static byte[] GenerateSHA512Hash(byte[] input)
        {
            byte[] output = new byte[64];
            ComputeHash(input, input.Length, output);
            return output;
        }

        private delegate int CryptDecrypt(byte[] inBuffer, int inSize, byte[] outBuffer, int outSize, byte[] key, byte[] IV, bool encrypt);
        private delegate int ComputeSHA512Hash(byte[] inBuffer, int inSize, byte[] outBuffer);


        private static int CryptDecryptNET(byte[] inBuffer, int inSize, byte[] outBuffer, int outSize, byte[] key, byte[] IV, bool encrypt)
        {
            if (encrypt)
                return EncryptNet(inBuffer, inSize, outBuffer, outSize);
            else
                return DecryptNet(inBuffer, inSize, outBuffer, outSize);
        }

        private static int EncryptNet(byte[] inBuffer, int inSize, byte[] outBuffer, int outSize)
        {
            ICryptoTransform encryptor = (new RijndaelManaged()).CreateEncryptor(m_Key, m_IV);

            MemoryStream memStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(inBuffer, 0, inSize);
            cryptoStream.FlushFinalBlock();

            //Get encrypted array of bytes.
            byte[] result = memStream.ToArray();
            if (result.Length > outSize)
                return 0;
            Buffer.BlockCopy(result, 0, outBuffer, 0, result.Length);
            return result.Length;
        }

        private static int DecryptNet(byte[] inBuffer, int inSize, byte[] outBuffer, int outSize)
        {
            ICryptoTransform decryptor = (new RijndaelManaged()).CreateDecryptor(m_Key, m_IV);
            MemoryStream memStream = new MemoryStream(inBuffer);
            CryptoStream cryptoStream = new CryptoStream(memStream, decryptor, CryptoStreamMode.Read);

            cryptoStream.Read(outBuffer, 0, outSize);
            return outSize;
        }

        #endregion

        #region DllImport
        [DllImport("Manage.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Validate(byte[] inBuffer,
                                                    int inSize,
                                                    byte[] outBuffer,
                                                    int outSize,
                                                    byte[] key,
                                                    byte[] IV,
                                                    bool encrypt);

        //the hash output is always 64 bytes so you need to pass in a 64 byte buffer
        [DllImport("Manage.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int ComputeHash(byte[] input, int inputSize, byte[] output);

        #endregion

    }
}
