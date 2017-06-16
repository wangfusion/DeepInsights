using DeepInsights.Shell.Infrastructure.Utilities.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;
using System.Text;

namespace DeepInsights.Shell.Infrastructure.UnitTests.Utilities.Security
{
    [TestClass]
    public class DigitalSigningHelperTest
    {
        [TestMethod]
        public void DigitalSigningHelper_Signing_Works()
        {
            string message = "Message requiring signature";
            byte[] data = Encoding.UTF8.GetBytes(message);

            using (var cryptoProvider = new RSACryptoServiceProvider())
            {
                byte[] privatePublicKeys = cryptoProvider.ExportCspBlob(true);
                byte[] publicKeyOnly = cryptoProvider.ExportCspBlob(false);
                byte[] signature = DigitalSigningHelper.SignMessage(data, privatePublicKeys);
                object hasher = SHA256.Create();

                Assert.IsTrue(DigitalSigningHelper.IsSignatureCorrect(signature, data, publicKeyOnly));

                // Tamper with the original data
                data[0] = 0;
                Assert.IsFalse(DigitalSigningHelper.IsSignatureCorrect(signature, data, publicKeyOnly));
            }
        }
    }
}
