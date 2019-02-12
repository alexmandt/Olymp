using NUnit.Framework;
using Olymp.Util;

namespace NUnitTest.Olymp.Util
{
    [TestFixture]
    public class MD5HelperNTest
    {
        [Test]
        public void EnsureMD5CalculationWorksProperly()
        {
            const string HashToCheck = "428F28BDFD1051F3CEAAD7E16C9F7E9C";
            const string PasswordToHash = "VeryS3cur3P0ssw0rd";

            var calculatedHash = MD5Helper.CalculateMD5Hash(PasswordToHash);
            Assert.That(calculatedHash.Equals(HashToCheck));
        }
    }
}