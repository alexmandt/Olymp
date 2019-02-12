using System.Linq;
using System.Text;
using NUnit.Framework;
using Olymp.Util;

namespace NUnitTest.Olymp.Util
{
    [TestFixture]
    public class RijndaelManagerNTest
    {
        [Test]
        public void EnsureWorksRijndaelManagerProperly()
        {
            const string TestPassword = "VeryS3cur3P0ssw0rd";
            var TextToCheck = Encoding.ASCII.GetBytes("this text is a test.");

            var calculatedByteArray = RijndaelManager.Encrypt(TextToCheck, TestPassword);

            Assert.That(RijndaelManager.Decrypt(calculatedByteArray, TestPassword).SequenceEqual(TextToCheck));
        }
    }
}