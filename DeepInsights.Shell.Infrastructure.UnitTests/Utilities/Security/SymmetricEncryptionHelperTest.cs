using DeepInsights.Shell.Infrastructure.Utilities.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;

namespace DeepInsights.Shell.Infrastructure.UnitTests
{
    [TestClass]
    public class SymmetricEncryptionHelperTest
    {
        [TestMethod]
        public void SymmetricEncryptionHelper_StringEncryption_Works()
        {
            byte[] kiv = new byte[16];
            RandomNumberGenerator.Create().GetBytes(kiv);

            string data = "secret";
            string encrypted = SymmetricEncryptionHelper.MemoryEncrypt(data, kiv, kiv);
            string decrypted = SymmetricEncryptionHelper.MemoryDecrypt(encrypted, kiv, kiv);

            Assert.AreEqual(data, decrypted);
        }
    }
}
