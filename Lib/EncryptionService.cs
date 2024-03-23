using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;

namespace RestApi.Lib
{
    public class EncryptionService
    {
        private const string EncryptionKey = "AAECAwQFBgcICQoLDA0ODw==";
        private const string IV = "HR$2pIjHR$2pIj12"; // Change this to your IV

        public static string Encrypt(string plainText)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(EncryptionKey);
            byte[] ivBytes = Encoding.UTF8.GetBytes(IV);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBytes;
                aesAlg.IV = ivBytes;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
        }

        public static string Decrypt(string cipherText)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(EncryptionKey);
            byte[] ivBytes = Encoding.UTF8.GetBytes(IV);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBytes;
                aesAlg.IV = ivBytes;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
