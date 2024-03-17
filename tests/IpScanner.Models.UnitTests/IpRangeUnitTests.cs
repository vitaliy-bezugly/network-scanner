using System.Net;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace IpScanner.Models.UnitTests
{
    [TestClass]
    public class IpRangeUnitTests
    {
        [TestMethod]
        [DataRow("192.168.0.1-2", "192.168.0.1,192.168.0.2")]
        [DataRow("26.0.0.16-18", "26.0.0.16,26.0.0.17,26.0.0.18")]
        [DataRow("192.168.0.104-106", "192.168.0.104,192.168.0.105,192.168.0.106")]
        [DataRow("192.168.0.201-202, 192.168.0.235", "192.168.0.201,192.168.0.202,192.168.0.235")]
        [DataRow("192.168.0.1, 192.168.0.104, 192.168.0.105", "192.168.0.1,192.168.0.104,192.168.0.105")]
        public void CreateBasedOnIpRange_ShouldReturnIpScanner_WhenIpRangeIsValid(string range, string expectedAsString)
        {
            // Arrange
            var ipRange = new IpRange(range);

            // Act
            List<IPAddress> result = ipRange.GenerateIPAddresses();

            // Assert
            List<IPAddress> expectedIpAddresses = expectedAsString.Split(',')
                .Select(x => IPAddress.Parse(x)).ToList();

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedIpAddresses.Count, result.Count);
            CollectionAssert.AreEqual(expectedIpAddresses, result);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(null)]
        public void CreateBasedOnIpRange_ShouldThrowArgumentException_WhenIpRangeIsEmptyOrNull(string range)
        {
            // Arrange
            var ipRange = new IpRange(range);

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => ipRange.GenerateIPAddresses());
        }

        [TestMethod]
        [DataRow("192.168.0.1-")]
        [DataRow("192.168.0.105-")]
        [DataRow("192.168.0.200-150")]
        [DataRow("192.168.0.")]
        [DataRow("192.168.0")]
        public void CreateBasedOnIpRange_ShouldThrowArgumentException_WhenIpRangeIsInvalid(string range)
        {
            // Arrange
            var ipRange = new IpRange(range);

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => ipRange.GenerateIPAddresses());
        }
    }
}
