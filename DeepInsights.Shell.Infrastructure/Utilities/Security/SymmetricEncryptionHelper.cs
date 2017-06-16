using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DeepInsights.Shell.Infrastructure.Utilities.Security
{
    public static class SymmetricEncryptionHelper
    {
        #region Memory Stream Encryption/Decryption Helpers

        public static string MemoryEncrypt(string data, byte[] key, byte[] iv)
        {
            return Convert.ToBase64String(MemoryEncrypt(Encoding.UTF8.GetBytes(data), key, iv));
        }

        public static string MemoryDecrypt(string data, byte[] key, byte[] iv)
        {
            return Encoding.UTF8.GetString(MemoryDecrypt(Convert.FromBase64String(data), key, iv));
        }

        public static byte[] MemoryEncrypt(byte[] data, byte[] key, byte[] iv)
        {
            using (Aes algorithm = Aes.Create())
            using (ICryptoTransform encryptor = algorithm.CreateEncryptor(key, iv))
            {
                return Crypt(data, encryptor);
            }
        }

        public static byte[] MemoryDecrypt(byte[] data, byte[] key, byte[] iv)
        {
            using (Aes algorithm = Aes.Create())
            using (ICryptoTransform decryptor = algorithm.CreateDecryptor(key, iv))
            {
                return Crypt(data, decryptor);
            }
        }

        static byte[] Crypt(byte[] data, ICryptoTransform cryptor)
        {
            MemoryStream m = new MemoryStream();
            using (Stream c = new CryptoStream(m, cryptor, CryptoStreamMode.Write))
            {
                c.Write(data, 0, data.Length);
            }

            return m.ToArray();
        }

        #endregion

        #region File Encryption/Decryption Helpers

        public static async Task FileEncrypt(string data, string fileName, byte[] key, byte[] iv)
        {
            using (Aes algorithm = Aes.Create())
            {
                using (ICryptoTransform encryptor = algorithm.CreateEncryptor(key, iv))
                using (Stream f = File.Create(fileName))
                using (Stream c = new CryptoStream(f, encryptor, CryptoStreamMode.Write))
                using (Stream d = new DeflateStream(c, CompressionMode.Compress))
                using (StreamWriter w = new StreamWriter(d))
                {
                    await w.WriteAsync(data);
                }
            }
        }

        public static async Task<string> FileDecrypt(string fileName, byte[] key, byte[] iv)
        {
            using (Aes algorithm = Aes.Create())
            {
                using (ICryptoTransform decryptor = algorithm.CreateDecryptor())
                using (Stream f = File.OpenRead(fileName))
                using (Stream c = new CryptoStream(f, decryptor, CryptoStreamMode.Read))
                using (Stream d = new DeflateStream(c, CompressionMode.Decompress))
                using (StreamReader r = new StreamReader(d))
                {
                    return await r.ReadToEndAsync();
                }
            }
        }

        #endregion
    }
}
