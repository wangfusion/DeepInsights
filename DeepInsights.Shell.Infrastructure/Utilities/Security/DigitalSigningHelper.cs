using System.Security.Cryptography;
using System.Text;

namespace DeepInsights.Shell.Infrastructure.Utilities.Security
{
    // A helper class using the public key algorithm to digitally sign messages or documents
    public static class DigitalSigningHelper
    {
        public static byte[] SignMessage(byte[] data, byte[] publicPrivateKeys)
        {
            object hasher = SHA256.Create();

            using (var cryptoProvider = new RSACryptoServiceProvider())
            {
                cryptoProvider.ImportCspBlob(publicPrivateKeys);
                return cryptoProvider.SignData(data, hasher);
            }
        }

        public static bool IsSignatureCorrect(byte[] signature, byte[] data, byte[] publicKey)
        {
            object hasher = SHA256.Create();

            using (var cryptoProvider = new RSACryptoServiceProvider())
            {
                cryptoProvider.ImportCspBlob(publicKey);
                return cryptoProvider.VerifyData(data, hasher, signature);
            }
        }
    }
}
