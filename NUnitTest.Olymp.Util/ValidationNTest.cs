using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;
using Olymp.Exceptions;
using Olymp.Util;

namespace NUnitTest.Olymp.Util
{
    public class ValidationNTest
    {
        private dynamic _ipv4MockData;
        private dynamic _ipv6MockData;

        [SetUp]
        public void Setup()
        {
            LoadIPv4Data();
            LoadIPv6Data();
        }

        [Test]
        public void EnsureIPv4AddressValidationWorksProperly()
        {
            Assert.IsNotNull(_ipv4MockData);
            Assert.IsNotEmpty(_ipv4MockData);

            foreach (var item in _ipv4MockData) Assert.IsTrue(Validator.ValidateAddress(item.ipv4.ToString()));
        }

        [Test]
        public void EnsureInvalidIPv4AddressValidationWorksProperly()
        {
            const string invalidIPv4 = "0.0.0.@";

            Assert.Throws<InvalidIpOrHostnameException>(() => Validator.ValidateAddress(invalidIPv4));
        }

        [Test]
        public void EnsureIPv6AddressValidationWorksProperly()
        {
            Assert.IsNotNull(_ipv6MockData);
            Assert.IsNotEmpty(_ipv6MockData);

            foreach (var item in _ipv6MockData) Assert.IsTrue(Validator.ValidateAddress(item.ipv6.ToString()));
        }

        [Test]
        public void EnsureInvalidIPv6AddressValidationWorksProperly()
        {
            const string invalidIPv6 = "912G:2609:6f3a:2be5:4b37:f48b:a554:4751";

            Assert.That(() => Validator.ValidateAddress(invalidIPv6),
                Throws.Exception
                    .TypeOf<InvalidIpOrHostnameException>());
        }

        [Test]
        public void EnsureHostnameValidationWorksProperly()
        {
            Assert.IsTrue(Validator.ValidateAddress("example.local"));
        }

        [Test]
        public void EnsureInvalidHostnameValidationWorksProperly()
        {
            const string invalidHostname = " ";

            Assert.That(() => Validator.ValidateAddress(invalidHostname),
                Throws.Exception
                    .TypeOf<InvalidIpOrHostnameException>());
        }


        #region SetupHelper

        private void LoadIPv4Data()
        {
            string json;
            using (var streamReader = new StreamReader("testfiles/ipv4.json"))
            {
                json = streamReader.ReadToEnd();
            }

            _ipv4MockData = JsonConvert.DeserializeObject(json);
        }

        private void LoadIPv6Data()
        {
            string json;
            using (var streamReader = new StreamReader("testfiles/ipv6.json"))
            {
                json = streamReader.ReadToEnd();
            }

            _ipv6MockData = JsonConvert.DeserializeObject(json);
        }

        #endregion
    }
}