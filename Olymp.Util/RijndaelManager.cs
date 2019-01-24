using System.IO;
using System.Security.Cryptography;

namespace Olymp.Util
{
    public static class RijndaelManager
    {
        private static readonly byte[] PEPPER = {0x26, 0xdc, 0xff, 0x00, 0xad, 0xed, 0x7a, 0xee, 0xc5, 0xfe, 0x07, 0xaf, 0x4d, 0x08, 0x22, 0x3c};

        public static byte[] Encrypt(byte[] plain, string password)
        {
            var rijndael = Rijndael.Create();
            var pdb = new Rfc2898DeriveBytes(password, PEPPER);
            rijndael.Key = pdb.GetBytes(32);
            rijndael.IV = pdb.GetBytes(16);
            rijndael.Padding = PaddingMode.PKCS7;

            using (var memoryStream = new MemoryStream())
            using (var cryptoStream = new CryptoStream(memoryStream, rijndael.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cryptoStream.Write(plain, 0, plain.Length);
                cryptoStream.Close();
                return memoryStream.ToArray();
            }
        }

        public static byte[] Decrypt(byte[] cipher, string password)
        {
            var rijndael = Rijndael.Create();
            var pdb = new Rfc2898DeriveBytes(password, PEPPER);
            rijndael.Key = pdb.GetBytes(32);
            rijndael.IV = pdb.GetBytes(16);
            rijndael.Padding = PaddingMode.PKCS7;

            using (var memoryStream = new MemoryStream())
            using (var cryptoStream = new CryptoStream(memoryStream, rijndael.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cryptoStream.Write(cipher, 0, cipher.Length);
                cryptoStream.Close();
                return memoryStream.ToArray();
            }
        }
    }
}